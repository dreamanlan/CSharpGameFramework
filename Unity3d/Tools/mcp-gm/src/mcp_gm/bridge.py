"""TCP bridge to the Unity GM channel (newline-delimited JSON, one request -> one reply).

Reverse connection: Unity connects OUT to this MCP-side TCP server and sends a hello
line first (the editor channel and the runtime channel are two separate connections):

  -> {"cmd":"hello","channel":"editor|runtime","editorPath":"...","projectPath":"...",
      "pid":123,"unityVersion":"...","mode":"...","platform":"...","productName":"..."}
  <- {"type":"hello_ack","ok":true,"connId":1}
MCP writes requests and Unity writes replies on the same socket:
  -> {"id":1,"cmd":"exec","code":"getposition(...)","timeout":15000}
  <- {"id":1,"ok":true,"result":"...","elapsedMs":3,"logs":[...]}

Protocol v1: a frame with a "type" key is a control frame, not a command. Unity's
GmSocketClient skips it without writing a reply. Both ends are always deployed
together, so no capability negotiation is needed.

The standalone player keeps its own GmSocketServer (port 39527), so MCP still connects
as a client for the "device" target (e.g. through adb forward).
"""

from __future__ import annotations

import json
import os
import select
import socket
import sys
import threading
import time
from dataclasses import dataclass, field
from pathlib import Path

# 39528 = MCP listener (Unity editor + editor Play mode connect here).
# 39527 = player listener, opened by the app itself (device target).
MCP_PORT = 39528
DEVICE_PORT = 39527
MAX_RESPONSE_BYTES = 8 * 1024 * 1024
HELLO_TIMEOUT = 10.0
# A background monitor drops sockets closed by Unity, pings idle connections, and
# reconnects the device channel on a fixed interval.
MONITOR_INTERVAL = 5.0
HEARTBEAT_IDLE = 30.0
HEARTBEAT_TIMEOUT_MS = 1000
HEARTBEAT_MAX_MISSES = 3
DEVICE_RETRY_INTERVAL = 10.0
DEVICE_LOG_INTERVAL = 60.0

EDITOR_CHANNEL_CMDS = frozenset({"ping", "logs", "clearlogs", "edit"})
RUNTIME_CHANNEL_CMDS = frozenset({"ping", "logs", "clearlogs", "exec", "console"})
# These commands are safe to resend after a reconnect; exec/console/edit may already have run.
SAFE_RETRY_COMMANDS = frozenset({"ping", "logs", "clearlogs"})
VALID_TARGETS = frozenset({"auto", "editor", "runtime", "device"})
DEFAULT_TARGET = "auto"


class BridgeError(RuntimeError):
    """Bridge-level error (no peer / connection lost / unparsable reply)."""


class ChannelRejected(BridgeError):
    """The command is not allowed on the selected channel.

    Raised before anything is written to the socket, so the caller must NOT report
    "the command may already have run" and must not drop the connection.
    """


def _env_int(name: str, default: int) -> int:
    try:
        return int(os.environ.get(name, default))
    except (TypeError, ValueError):
        return default


def _env_target() -> str:
    t = os.environ.get("UC_GM_TARGET", DEFAULT_TARGET).strip().lower()
    return t if t in VALID_TARGETS else DEFAULT_TARGET


def mcp_host() -> str:
    return os.environ.get("UC_GM_MCP_HOST", "127.0.0.1")


def mcp_port() -> int:
    return _env_int("UC_GM_MCP_PORT", MCP_PORT)


def device_host() -> str:
    return os.environ.get("UC_GM_HOST", "127.0.0.1")


def device_port() -> int:
    return _env_int("UC_GM_PORT", DEVICE_PORT)


def known_projects_path() -> Path:
    override = os.environ.get("UC_GM_KNOWN_PROJECTS")
    if override:
        return Path(override).expanduser()
    base = os.environ.get("LOCALAPPDATA") or os.environ.get("XDG_STATE_HOME")
    if base:
        return Path(base) / "mcp-gm" / "known_projects.json"
    return Path.home() / ".mcp-gm" / "known_projects.json"


def normalize_editor_path(editor_path: str) -> str:
    """Accept a macOS .app bundle, its inner executable, or a Windows Unity.exe."""
    if not editor_path:
        return ""
    p = Path(editor_path)
    parts = p.parts
    for i, part in enumerate(parts):
        if part.endswith(".app"):
            return str(Path(*parts[:i + 1]))
    return editor_path


def valid_project_entry(entry: object) -> bool:
    if not isinstance(entry, dict):
        return False
    editor = entry.get("editorPath")
    project = entry.get("projectPath")
    if not editor or not project:
        return False
    try:
        e = Path(str(editor))
        p = Path(str(project))
        if not (e.is_file() or e.is_dir()):
            return False
        if not p.is_dir():
            return False
        return (p / "ProjectSettings" / "ProjectVersion.txt").is_file()
    except OSError:
        return False


class KnownProjects:
    """Persistent editor/project pairs with stale-entry cleanup."""

    def __init__(self, path: Path):
        self._path = path
        self._lock = threading.RLock()
        self._data: dict = {"version": 1, "activeProject": None, "projects": {}}
        self._load()
        self.cleanup()

    def _load(self) -> None:
        try:
            data = json.loads(self._path.read_text(encoding="utf-8"))
            if isinstance(data, dict) and isinstance(data.get("projects"), dict):
                self._data = {"version": 1,
                              "activeProject": data.get("activeProject"),
                              "projects": dict(data["projects"])}
        except (OSError, ValueError, TypeError):
            pass

    def _save(self) -> None:
        try:
            self._path.parent.mkdir(parents=True, exist_ok=True)
            self._path.write_text(json.dumps(self._data, ensure_ascii=False, indent=2),
                                  encoding="utf-8")
        except OSError as exc:
            print(f"[mcp-gm] cannot write known projects: {exc}", file=sys.stderr, flush=True)

    def cleanup(self) -> list[str]:
        """Remove entries whose editor or project path no longer exists."""
        with self._lock:
            removed = []
            for name, entry in list(self._data["projects"].items()):
                if not valid_project_entry(entry):
                    del self._data["projects"][name]
                    removed.append(name)
            active = self._data.get("activeProject")
            if active and active not in self._data["projects"]:
                self._data["activeProject"] = None
            if removed:
                self._save()
            return removed

    def upsert(self, editor_path: str, project_path: str, pid: int = 0) -> str | None:
        if not editor_path or not project_path:
            return None
        name = Path(project_path).name
        if not name:
            return None
        with self._lock:
            self.cleanup()
            self._data["projects"][name] = {
                "editorPath": normalize_editor_path(editor_path),
                "projectPath": str(project_path),
                "pid": pid,
                "lastSeen": time.time(),
            }
            self._save()
            return name

    def get(self, name: str) -> dict | None:
        with self._lock:
            self.cleanup()
            entry = self._data["projects"].get(name)
            return dict(entry) if isinstance(entry, dict) else None

    def list(self) -> list[str]:
        with self._lock:
            self.cleanup()
            return sorted(self._data["projects"])

    @property
    def active(self) -> str | None:
        with self._lock:
            self.cleanup()
            return self._data.get("activeProject")

    def set_active(self, name: str | None) -> None:
        with self._lock:
            self._data["activeProject"] = name
            self._save()


@dataclass
class GmPeer:
    conn_id: int
    sock: socket.socket
    channel: str
    endpoint: str = ""
    editor_path: str = ""
    project_path: str = ""
    project_name: str = ""
    pid: int = 0
    unity_version: str = ""
    mode: str = ""
    platform: str = ""
    product_name: str = ""
    is_device: bool = False
    connected_at: float = field(default_factory=time.time)
    last_activity: float = field(default_factory=time.time)
    missed_heartbeats: int = 0
    lock: threading.RLock = field(default_factory=threading.RLock)

    def info(self) -> dict:
        return {"connId": self.conn_id, "channel": self.channel, "endpoint": self.endpoint,
                "project": self.project_name, "projectPath": self.project_path,
                "editorPath": self.editor_path, "pid": self.pid,
                "unityVersion": self.unity_version, "mode": self.mode,
                "platform": self.platform, "productName": self.product_name,
                "isDevice": self.is_device,
                "connectedAt": round(self.connected_at, 1)}


class GmConnectionManager:
    def __init__(self):
        self._peers: dict[int, GmPeer] = {}
        self._lock = threading.RLock()
        self._seq = 0
        self._conn_seq = 0
        self._last_peer_id: int | None = None
        self._target = _env_target()
        self._known = KnownProjects(known_projects_path())
        self._active_project = self._known.active
        self._started = False
        self._listener_error: str | None = None
        self._device_enabled = False
        self._last_device_attempt = 0.0
        self._last_device_log = 0.0

    # ------------------------------------------------------------------
    # Listener (Unity connects here)
    # ------------------------------------------------------------------
    def start(self) -> None:
        with self._lock:
            if self._started:
                return
            self._started = True
        threading.Thread(target=self._serve, daemon=True, name="mcp-gm-listener").start()
        threading.Thread(target=self._monitor_loop, daemon=True, name="mcp-gm-monitor").start()

    def _serve(self) -> None:
        host, port = mcp_host(), mcp_port()
        srv = None
        while True:
            srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            try:
                srv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
                srv.bind((host, port))
                srv.listen(32)
                with self._lock:
                    self._listener_error = None
                print(f"[mcp-gm] listening for Unity connections on {host}:{port}",
                      file=sys.stderr, flush=True)
                break
            except OSError as exc:
                with self._lock:
                    self._listener_error = str(exc)
                print(f"[mcp-gm] cannot listen on {host}:{port}: {exc}; retrying in 2s",
                      file=sys.stderr, flush=True)
                try:
                    srv.close()
                except OSError:
                    pass
                srv = None
                time.sleep(2.0)
        while True:
            try:
                conn, addr = srv.accept()
            except OSError:
                break
            threading.Thread(target=self._accept, args=(conn, addr), daemon=True).start()

    def _accept(self, conn: socket.socket, addr) -> None:
        try:
            line = self._read_line(conn, HELLO_TIMEOUT)
            hello = json.loads(line)
            if not isinstance(hello, dict) or hello.get("cmd") != "hello":
                raise BridgeError("first line is not a hello")
            channel = str(hello.get("channel", "")).lower()
            if channel not in ("editor", "runtime"):
                raise BridgeError(f"invalid hello channel: {channel!r}")
            editor_path = str(hello.get("editorPath") or "")
            project_path = str(hello.get("projectPath") or "")
            with self._lock:
                self._conn_seq += 1
                peer = GmPeer(conn_id=self._conn_seq, sock=conn, channel=channel,
                              endpoint=f"{addr[0]}:{addr[1]}", editor_path=editor_path,
                              project_path=project_path,
                              project_name=Path(project_path).name if project_path else "",
                              pid=int(hello.get("pid") or 0),
                              unity_version=str(hello.get("unityVersion") or ""),
                              mode=str(hello.get("mode") or ""),
                              platform=str(hello.get("platform") or ""),
                              product_name=str(hello.get("productName") or ""))
                self._peers[peer.conn_id] = peer
                self._last_peer_id = peer.conn_id
            # v1 control frame: GmSocketClient recognises "type" as a non-command frame
            # and skips it without writing a reply, so the first request stays in sync.
            ack = {"type": "hello_ack", "ok": True, "connId": peer.conn_id}
            conn.sendall((json.dumps(ack, ensure_ascii=False) + "\n").encode("utf-8"))
            self._known.upsert(editor_path, project_path, peer.pid)
            with self._lock:
                if not self._active_project and peer.project_name:
                    self._active_project = peer.project_name
                    self._known.set_active(peer.project_name)
            print(f"[mcp-gm] connected #{peer.conn_id} {channel} {peer.project_name}",
                  file=sys.stderr, flush=True)
        except Exception as exc:
            print(f"[mcp-gm] rejected connection from {addr}: {exc}", file=sys.stderr, flush=True)
            with self._lock:
                for cid, peer in list(self._peers.items()):
                    if peer.sock is conn:
                        self._peers.pop(cid, None)
            try:
                conn.close()
            except OSError:
                pass

    # ------------------------------------------------------------------
    # Peer selection
    # ------------------------------------------------------------------
    @staticmethod
    def _normalize_target(target: str) -> str:
        t = (target or DEFAULT_TARGET).strip().lower()
        if t not in VALID_TARGETS:
            raise BridgeError(f"Unknown GM target {target!r}; expected auto/editor/runtime/device.")
        return t

    @property
    def target(self) -> str:
        return self._target

    def set_target(self, target: str) -> str:
        t = self._normalize_target(target)
        self._target = t
        return t

    def _resolve_project_name(self, project: str | None) -> str | None:
        if project:
            return Path(project).name
        if self._active_project:
            return self._active_project
        names = sorted({p.project_name for p in self._peers.values() if p.project_name})
        if len(names) == 1:
            return names[0]
        if len(names) > 1:
            raise BridgeError(
                "Multiple Unity projects are connected (" + ", ".join(names) + "). "
                "Pass project=... or call unity_set_active_project(...) once.")
        return None

    def _no_peer_message(self, target: str, name: str | None) -> str:
        with self._lock:
            connected = [p.info() for p in self._peers.values()]
            err = self._listener_error
        parts = [f"No Unity GM connection for target={target!r}"]
        if name:
            parts.append(f"project={name!r}")
        if err:
            parts.append(f"listener error: {err}")
        parts.append(f"listener={mcp_host()}:{mcp_port()}")
        parts.append("Unity connects out after the project finishes compiling; it retries every 10s.")
        parts.append(f"connected={connected}")
        parts.append(f"knownProjects={self._known.list()}")
        return " ".join(parts)

    def _select_peer(self, target: str, project: str | None) -> GmPeer:
        with self._lock:
            self._known.cleanup()
            name = self._resolve_project_name(project)
            if target == "device":
                return self._ensure_device_peer()
            peers = [p for p in self._peers.values() if not p.is_device]
            if name:
                peers = [p for p in peers if p.project_name == name]
            if target in ("editor", "runtime"):
                peers = [p for p in peers if p.channel == target]
            if not peers:
                raise BridgeError(self._no_peer_message(target, name))
            if target == "auto":
                peers.sort(key=lambda p: (0 if p.channel == "editor" else 1, p.connected_at))
            else:
                peers.sort(key=lambda p: p.connected_at)
            peer = peers[0]
            self._last_peer_id = peer.conn_id
            return peer

    def _ensure_device_peer(self) -> GmPeer:
        with self._lock:
            self._device_enabled = True
            for p in self._peers.values():
                if p.is_device:
                    self._last_peer_id = p.conn_id
                    return p
            host, port = device_host(), device_port()
            try:
                sock = socket.create_connection((host, port), timeout=1.0)
                sock.settimeout(None)
            except OSError as exc:
                self._last_device_attempt = time.time()
                raise BridgeError(f"Cannot reach the player GM server on {host}:{port}: {exc}. "
                                  "Use adb forward tcp:39527 tcp:39527 for an Android device.") from exc
            self._conn_seq += 1
            peer = GmPeer(conn_id=self._conn_seq, sock=sock, channel="runtime",
                          endpoint=f"{host}:{port}", is_device=True,
                          project_name=self._active_project or "")
            self._peers[peer.conn_id] = peer
            self._last_peer_id = peer.conn_id
            return peer

    def _drop_peer(self, peer: GmPeer) -> None:
        with self._lock:
            existed = self._peers.pop(peer.conn_id, None)
        try:
            peer.sock.close()
        except OSError:
            pass
        if existed is not None:
            kind = "device" if peer.is_device else f"{peer.channel}/{peer.project_name}"
            print(f"[mcp-gm] connection #{peer.conn_id} ({kind}) dropped",
                  file=sys.stderr, flush=True)

    # ------------------------------------------------------------------
    # Monitor: active cleanup, heartbeat and device reconnect
    # ------------------------------------------------------------------
    def _monitor_loop(self) -> None:
        while True:
            time.sleep(MONITOR_INTERVAL)
            try:
                self._monitor_once()
            except Exception as exc:
                print(f"[mcp-gm] monitor error: {exc}", file=sys.stderr, flush=True)

    def _monitor_once(self) -> None:
        with self._lock:
            peers = list(self._peers.values())
        for peer in peers:
            with self._lock:
                still_present = peer.conn_id in self._peers
            if not still_present:
                continue
            if self._peer_closed(peer):
                self._drop_peer(peer)
                continue
            if not peer.is_device and time.time() - peer.last_activity >= HEARTBEAT_IDLE:
                self._heartbeat(peer)
        self._maybe_reconnect_device()

    @staticmethod
    def _peer_closed(peer: GmPeer) -> bool:
        # PEEK does not consume the buffer, so a pending reply is left for call().
        try:
            readable, _, _ = select.select([peer.sock], [], [], 0)
            if not readable:
                return False
            return peer.sock.recv(1, socket.MSG_PEEK) == b""
        except (OSError, ValueError):
            return True

    def _heartbeat(self, peer: GmPeer) -> None:
        if not peer.lock.acquire(blocking=False):
            return
        try:
            with self._lock:
                if peer.conn_id not in self._peers:
                    return
                self._seq += 1
                seq = self._seq
            payload = {"id": seq, "cmd": "ping", "timeout": HEARTBEAT_TIMEOUT_MS}
            try:
                resp = self._roundtrip(peer, payload, extra_wait=1.0)
            except (OSError, BridgeError):
                resp = None
            if isinstance(resp, dict) and resp.get("ok"):
                peer.last_activity = time.time()
                peer.missed_heartbeats = 0
                return
        finally:
            peer.lock.release()
        peer.missed_heartbeats += 1
        if peer.missed_heartbeats >= HEARTBEAT_MAX_MISSES:
            print(f"[mcp-gm] heartbeat failed {peer.missed_heartbeats}x for #{peer.conn_id}, "
                  "dropping connection", file=sys.stderr, flush=True)
            self._drop_peer(peer)

    def _maybe_reconnect_device(self) -> None:
        with self._lock:
            if not self._device_enabled:
                return
            for p in self._peers.values():
                if p.is_device:
                    return
            now = time.time()
            if now - self._last_device_attempt < DEVICE_RETRY_INTERVAL:
                return
            self._last_device_attempt = now
            host, port = device_host(), device_port()
        try:
            sock = socket.create_connection((host, port), timeout=1.0)
            sock.settimeout(None)
        except OSError as exc:
            if time.time() - self._last_device_log >= DEVICE_LOG_INTERVAL:
                self._last_device_log = time.time()
                print(f"[mcp-gm] device reconnect to {host}:{port} failed: {exc}",
                      file=sys.stderr, flush=True)
            return
        with self._lock:
            self._conn_seq += 1
            peer = GmPeer(conn_id=self._conn_seq, sock=sock, channel="runtime",
                          endpoint=f"{host}:{port}", is_device=True,
                          project_name=self._active_project or "")
            self._peers[peer.conn_id] = peer
            self._last_peer_id = peer.conn_id
        print(f"[mcp-gm] device reconnected on {host}:{port}", file=sys.stderr, flush=True)

    def disconnect(self) -> None:
        """Drop all live sockets; Unity clients reconnect on their own."""
        with self._lock:
            peers = list(self._peers.values())
            self._peers.clear()
        for peer in peers:
            try:
                peer.sock.close()
            except OSError:
                pass

    def register_project(self, editor_path: str, project_path: str, pid: int = 0) -> str | None:
        """Persist an editor/project pair (used by unity_start_by_path)."""
        name = self._known.upsert(editor_path, project_path, pid)
        if name:
            with self._lock:
                if not self._active_project:
                    self._active_project = name
                    self._known.set_active(name)
        return name

    def drop_project(self, name: str) -> None:
        """Close the sockets of one project; its Unity instance will reconnect."""
        with self._lock:
            peers = [p for p in self._peers.values() if p.project_name == name]
            for p in peers:
                self._peers.pop(p.conn_id, None)
        for p in peers:
            try:
                p.sock.close()
            except OSError:
                pass

    # ------------------------------------------------------------------
    # Requests
    # ------------------------------------------------------------------
    def call(self, cmd: str, timeout_ms: int = 15000, target: str | None = None,
             project: str | None = None, **kwargs) -> dict:
        payload = {"cmd": cmd}
        payload.update({k: v for k, v in kwargs.items() if v is not None})
        return self.request(payload, timeout_ms, target=target, project=project)

    def request(self, payload: dict, timeout_ms: int = 15000, target: str | None = None,
                project: str | None = None) -> dict:
        with self._lock:
            if target is not None:
                self.set_target(target)
            payload = dict(payload)
            self._seq += 1
            payload.setdefault("id", self._seq)
            payload.setdefault("timeout", timeout_ms)
            tgt = self._normalize_target(self._target)
            peer = self._select_peer(tgt, project)
            cmd = str(payload.get("cmd", ""))
            self._check_command_channel(peer, cmd)
            with peer.lock:
                try:
                    resp = self._roundtrip(peer, payload)
                    peer.last_activity = time.time()
                    peer.missed_heartbeats = 0
                    return resp
                except ChannelRejected:
                    raise
                except (OSError, BridgeError) as exc:
                    self._drop_peer(peer)
                    if cmd in SAFE_RETRY_COMMANDS and not peer.is_device:
                        try:
                            retry_peer = self._select_peer(tgt, project)
                            with retry_peer.lock:
                                resp = self._roundtrip(retry_peer, payload)
                                retry_peer.last_activity = time.time()
                                retry_peer.missed_heartbeats = 0
                                return resp
                        except (OSError, BridgeError):
                            pass
                    raise BridgeError(
                        f"{exc}\nConnection closed. The previous {cmd!r} may already have run in Unity; "
                        "check unity_logs / unity_info before retrying.") from exc

    def _check_command_channel(self, peer: GmPeer, cmd: str) -> None:
        if peer.channel == "editor":
            allowed = EDITOR_CHANNEL_CMDS
        elif peer.channel == "runtime":
            allowed = RUNTIME_CHANNEL_CMDS
        else:
            return
        if cmd in allowed:
            return
        raise ChannelRejected(
            f"cmd {cmd!r} cannot run on the {peer.channel} channel ({peer.endpoint}). "
            "Only unity_editor_exec ('edit') executes scripts on the editor channel; "
            "unity_exec / unity_console need the runtime channel. "
            "Pass target='runtime' after Play starts.")

    def _roundtrip(self, peer: GmPeer, payload: dict, extra_wait: float = 5.0) -> dict:
        data = (json.dumps(payload, ensure_ascii=False) + "\n").encode("utf-8")
        peer.sock.sendall(data)
        wait = float(payload.get("timeout", 15000)) / 1000.0 + extra_wait
        line = self._read_line(peer.sock, wait)
        try:
            resp = json.loads(line)
        except json.JSONDecodeError as exc:
            raise BridgeError(f"Unity reply is not valid JSON: {line[:200]!r}") from exc
        if not isinstance(resp, dict):
            raise BridgeError(f"Unexpected Unity reply shape: {line[:200]!r}")
        return resp

    @staticmethod
    def _read_line(sock: socket.socket, timeout: float) -> str:
        sock.settimeout(timeout)
        buf = bytearray()
        try:
            while True:
                try:
                    chunk = sock.recv(65536)
                except socket.timeout as exc:
                    raise BridgeError(
                        f"Timed out waiting for data from Unity ({timeout:.0f}s): main thread blocked "
                        "(infinite loop / breakpoint / long compile)?") from exc
                if not chunk:
                    raise BridgeError("Unity closed the connection (compiling or domain reload?)")
                nl = chunk.find(b"\n")
                if nl >= 0:
                    buf.extend(chunk[:nl])
                    return buf.decode("utf-8", "replace")
                buf.extend(chunk)
                if len(buf) > MAX_RESPONSE_BYTES:
                    raise BridgeError("Reply exceeded 8MB, aborted (narrow down the query)")
        finally:
            sock.settimeout(None)

    # ------------------------------------------------------------------
    # Introspection / known projects
    # ------------------------------------------------------------------
    def channel_info(self) -> dict:
        with self._lock:
            self._known.cleanup()
            peer = self._peers.get(self._last_peer_id)
            if peer and peer.channel == "editor":
                allowed = sorted(EDITOR_CHANNEL_CMDS)
            elif peer and peer.channel == "runtime":
                allowed = sorted(RUNTIME_CHANNEL_CMDS)
            else:
                allowed = sorted(EDITOR_CHANNEL_CMDS | RUNTIME_CHANNEL_CMDS)
            return {"target": self._target,
                    "channel": peer.channel if peer else None,
                    "endpoint": peer.endpoint if peer else None,
                    "port": mcp_port(),
                    "project": peer.project_name if peer else None,
                    "activeProject": self._active_project,
                    "allowed_cmds": allowed,
                    "connections": [p.info() for p in self._peers.values()],
                    "knownProjects": self._known.list(),
                    "listener": {"host": mcp_host(), "port": mcp_port(),
                                 "started": self._started, "error": self._listener_error}}

    def known_projects(self) -> dict:
        with self._lock:
            removed = self._known.cleanup()
            entries = {}
            for name in self._known.list():
                entry = self._known.get(name)
                if entry:
                    entries[name] = {"editorPath": entry.get("editorPath"),
                                     "projectPath": entry.get("projectPath")}
            return {"activeProject": self._active_project, "projects": entries,
                    "path": str(known_projects_path()), "removed": removed}

    def set_active_project(self, name: str) -> dict:
        with self._lock:
            removed = self._known.cleanup()
            known = self._known.list()
            connected = sorted({p.project_name for p in self._peers.values() if p.project_name})
            if name not in known and name not in connected:
                raise BridgeError(
                    f"Unknown project {name!r}. Known projects: {known}; connected: {connected}. "
                    "Use unity_start_by_path(...) once to register a project.")
            self._active_project = name
            self._known.set_active(name)
            return {"ok": True, "activeProject": name, "knownProjects": known,
                    "connected": connected, "removed": removed}

    def connected_project_names(self) -> list[str]:
        with self._lock:
            return sorted({p.project_name for p in self._peers.values() if p.project_name})

    def peer_pids(self, project: str | None = None) -> list[int]:
        with self._lock:
            name = self._resolve_project_name(project)
            return [p.pid for p in self._peers.values()
                    if p.pid and (not name or p.project_name == name)]


_bridge = GmConnectionManager()


def get_bridge() -> GmConnectionManager:
    return _bridge


def call(cmd: str, timeout_ms: int = 15000, **kwargs) -> dict:
    return _bridge.call(cmd, timeout_ms=timeout_ms, **kwargs)
