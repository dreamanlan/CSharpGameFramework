"""TCP bridge to the Unity GM channel (newline-delimited JSON, one request -> one reply).

Unity side: Packages/myself.gmscript/Runtime/GmCommands/GmSocketServer.cs
  {"id":1,"cmd":"exec","code":"getposition(...)","timeout":15000}
  <- {"id":1,"ok":true,"result":"...","elapsedMs":3,"logs":[{"i":1,"lv":"W","m":"..."}]}
"""

from __future__ import annotations

import json
import os
import socket
import threading

# 39528 = editor listener (Tools/GM Socket), 39527 = runtime (Play mode / Development Build)
EDITOR_PORT = 39528
RUNTIME_PORT = 39527
DEFAULT_PORTS = (EDITOR_PORT, RUNTIME_PORT)
# "auto" keeps the historical behaviour (editor first, fall back to runtime).
# Typical workflow: start on "editor", then switch to "runtime" once Play starts.
TARGET_PORTS = {
    "auto": DEFAULT_PORTS,
    "editor": (EDITOR_PORT,),
    "runtime": (RUNTIME_PORT,),
}
VALID_TARGETS = frozenset(TARGET_PORTS)
DEFAULT_TARGET = "auto"

# Script-executing commands are channel-specific. The editor listener is for editor DSL
# (unity_editor_exec); GM DSL / DebugConsole commands belong to the runtime channel.
# ping/logs/clearlogs are not script execution and stay available on both channels.
EDITOR_CHANNEL_CMDS = frozenset({"ping", "logs", "clearlogs", "edit"})
RUNTIME_CHANNEL_CMDS = frozenset({"ping", "logs", "clearlogs", "exec", "console"})
# These commands are safe to resend after a reconnect; exec/console/edit may already have run.
SAFE_RETRY_COMMANDS = frozenset({"ping", "logs", "clearlogs"})
MAX_RESPONSE_BYTES = 8 * 1024 * 1024


class BridgeError(RuntimeError):
    """Bridge-level error (cannot connect / connection lost / unparsable reply)."""


class ChannelRejected(BridgeError):
    """The command is not allowed on the currently connected channel.

    Raised before anything is written to the socket, so the caller must NOT report
    "the command may already have run" and must not drop the connection.
    """


class GmBridge:
    def __init__(self, host: str | None = None, port: int | None = None, connect_timeout: float = 1.0,
                 target: str | None = None):
        self._host = host or os.environ.get("UC_GM_HOST", "127.0.0.1")
        env_port = os.environ.get("UC_GM_PORT")
        self._explicit_port = port or (int(env_port) if env_port else None)
        self._connect_timeout = connect_timeout
        env_target = os.environ.get("UC_GM_TARGET", DEFAULT_TARGET)
        self._target = self._normalize_target(target or env_target)
        self._sock: socket.socket | None = None
        self._port: int | None = None
        self._lock = threading.RLock()
        self._seq = 0

    @staticmethod
    def _normalize_target(target: str) -> str:
        t = (target or DEFAULT_TARGET).strip().lower()
        if t not in VALID_TARGETS:
            raise BridgeError(f"Unknown GM target {target!r}; expected auto/editor/runtime.")
        return t

    @property
    def target(self) -> str:
        return self._target

    @property
    def endpoint(self) -> str:
        return f"{self._host}:{self._port}" if self._port else f"{self._host}:not connected"

    def set_target(self, target: str) -> str:
        """Switch the channel used by all following calls. Passing a target on a single
        call has the same persistent effect, so 'editor' -> 'runtime' only needs one call
        after Play starts. Reconnects on the next request."""
        t = self._normalize_target(target)
        with self._lock:
            if t != self._target:
                self._target = t
                self._close()
        return t

    @property
    def channel(self) -> str | None:
        if self._port == EDITOR_PORT:
            return "editor"
        if self._port == RUNTIME_PORT:
            return "runtime"
        return None

    def disconnect(self) -> None:
        """Drop the live socket so the next call reconnects (used after Unity restarts)."""
        with self._lock:
            self._close()

    def channel_info(self) -> dict:
        channel = self.channel
        if channel == "editor":
            allowed = sorted(EDITOR_CHANNEL_CMDS)
        elif channel == "runtime":
            allowed = sorted(RUNTIME_CHANNEL_CMDS)
        else:
            allowed = sorted(EDITOR_CHANNEL_CMDS | RUNTIME_CHANNEL_CMDS)
        return {
            "target": self._target,
            "channel": channel,
            "endpoint": self.endpoint,
            "port": self._port,
            "allowed_cmds": allowed,
        }

    def _check_command_channel(self, cmd: str) -> None:
        """Reject script-executing commands on the wrong channel before anything is sent."""
        if self._explicit_port:
            # A forced port (UC_GM_PORT / adb forward) bypasses the editor/runtime split.
            return
        if self._sock is None:
            self._ensure_socket()
        channel = self.channel
        if channel == "editor":
            allowed = EDITOR_CHANNEL_CMDS
        elif channel == "runtime":
            allowed = RUNTIME_CHANNEL_CMDS
        else:
            return
        if cmd in allowed:
            return
        raise ChannelRejected(
            f"cmd {cmd!r} cannot run on the {channel} channel ({self.endpoint}). "
            "Only unity_editor_exec ('edit') executes scripts on the editor channel; "
            "unity_exec / unity_console need the runtime channel. "
            "Pass target='runtime' (e.g. unity_info(target='runtime')) after Play starts."
        )

    def call(self, cmd: str, timeout_ms: int = 15000, target: str | None = None, **kwargs) -> dict:
        payload = {"cmd": cmd}
        payload.update({k: v for k, v in kwargs.items() if v is not None})
        return self.request(payload, timeout_ms, target=target)

    def request(self, payload: dict, timeout_ms: int = 15000, target: str | None = None) -> dict:
        with self._lock:
            if target is not None:
                self.set_target(target)
            payload = dict(payload)
            if "id" not in payload:
                self._seq += 1
                payload["id"] = self._seq
            payload.setdefault("timeout", timeout_ms)
            try:
                self._check_command_channel(str(payload.get("cmd", "")))
                return self._roundtrip(payload)
            except ChannelRejected:
                # Client-side rejection: nothing was sent and the connection is still good.
                raise
            except (OSError, BridgeError) as exc:
                self._close()
                cmd = payload.get("cmd")
                if cmd in SAFE_RETRY_COMMANDS:
                    try:
                        return self._roundtrip(payload)
                    except (OSError, BridgeError) as exc2:
                        raise BridgeError(str(exc2)) from exc2
                raise BridgeError(
                    f"{exc}\nConnection closed. The previous {cmd!r} may already have run in Unity; "
                    "check unity_logs / unity_info before retrying."
                ) from exc

    # ------------------------------------------------------------------
    def _roundtrip(self, payload: dict) -> dict:
        sock = self._ensure_socket()
        data = (json.dumps(payload, ensure_ascii=False) + "\n").encode("utf-8")
        sock.sendall(data)
        wait = float(payload.get("timeout", 15000)) / 1000.0 + 5.0
        line = self._read_line(sock, wait)
        try:
            resp = json.loads(line)
        except json.JSONDecodeError as exc:
            raise BridgeError(f"Unity reply is not valid JSON: {line[:200]!r}") from exc
        if not isinstance(resp, dict):
            raise BridgeError(f"Unexpected Unity reply shape: {line[:200]!r}")
        return resp

    def _ensure_socket(self) -> socket.socket:
        if self._sock is not None:
            return self._sock
        ports = [self._explicit_port] if self._explicit_port else list(TARGET_PORTS[self._target])
        last: Exception | None = None
        for port in ports:
            try:
                sock = socket.create_connection((self._host, port), timeout=self._connect_timeout)
                sock.settimeout(None)
                self._sock, self._port = sock, port
                return sock
            except OSError as exc:
                last = exc
        tried = ", ".join(str(p) for p in ports)
        raise BridgeError(
            f"Cannot reach the Unity GM channel on target {self._target!r} (tried {tried}). "
            f"Editor: {EDITOR_PORT} (Tools/GM Socket/Start Editor Listener); "
            f"runtime: {RUNTIME_PORT} (Play mode / Development Build). "
            "Pass target='editor'/'runtime' on any tool to pick a channel, "
            "UC_GM_TARGET as the startup default, UC_GM_PORT to force an exact port."
        ) from last

    def _read_line(self, sock: socket.socket, timeout: float) -> str:
        sock.settimeout(timeout)
        buf = bytearray()
        while True:
            try:
                chunk = sock.recv(65536)
            except socket.timeout as exc:
                self._close()
                raise BridgeError(
                    f"Timed out waiting for a Unity reply ({timeout:.0f}s): main thread blocked "
                    "(infinite loop / breakpoint / long compile)?"
                ) from exc
            if not chunk:
                raise BridgeError("Unity closed the connection (compiling or domain reload?)")
            nl = chunk.find(b"\n")
            if nl >= 0:
                buf.extend(chunk[:nl])
                if chunk[nl + 1:]:
                    # Unexpected extra bytes: drop the connection to resync.
                    self._close()
                return buf.decode("utf-8", "replace")
            buf.extend(chunk)
            if len(buf) > MAX_RESPONSE_BYTES:
                self._close()
                raise BridgeError("Reply exceeded 8MB, aborted (narrow down the query)")

    def _close(self) -> None:
        sock, self._sock = self._sock, None
        self._port = None
        if sock is not None:
            try:
                sock.close()
            except OSError:
                pass


_bridge = GmBridge()


def get_bridge() -> GmBridge:
    return _bridge


def call(cmd: str, timeout_ms: int = 15000, **kwargs) -> dict:
    return _bridge.call(cmd, timeout_ms=timeout_ms, **kwargs)
