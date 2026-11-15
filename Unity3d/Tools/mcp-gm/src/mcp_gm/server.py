"""MCP server exposing the Unity GM channel as tools.

Reverse connection: every Unity instance connects OUT to this MCP TCP server
(127.0.0.1:39528 by default) and sends a hello line first, carrying editorPath,
projectPath, channel (editor / runtime), pid and unityVersion. The editor channel
(edit mode) and the runtime channel (Play mode in the editor) are two separate
connections; MCP tells them apart by their channel field. The standalone player
still opens its own listener (port 39527), reached with target="device".

Tool results are the raw Unity reply dict:
  success {"id":..,"ok":true,"result":...,"elapsedMs":..,"logs":[..]}
  failure {"ok":false,"error":".."}

Channels: every tool accepts target ("editor" / "runtime" / "auto" / "device") and
an optional project name. Passing target switches the channel for this AND all
following calls. project selects one Unity instance; after unity_set_active_project()
it can be omitted. UC_GM_TARGET sets the startup default, UC_GM_MCP_HOST /
UC_GM_MCP_PORT configure the listener, UC_GM_HOST / UC_GM_PORT the device endpoint.

Script execution is channel-restricted:
  editor channel  -> unity_editor_exec only
  runtime channel -> unity_exec / unity_console only
  both channels   -> unity_info / unity_logs / unity_clearlogs
unity_info reports the live connections, the active project and the known projects.

Interpreters (Unity side, all shared with the existing systems):
  unity_exec        -> one persistent DslCalculator in GmCommandApi, sharing the GM
                       command/function registry with the story-based GM scripts.
  unity_editor_exec -> the same DslCalculator used by the editor command window and
                       editor_startup.dsl, with editor APIs only.
  unity_console     -> DebugConsole.ExecuteForResult when a DebugConsole exists.

Discovering DSL APIs (IMPORTANT - the two channels have different registries):
  unity_editor_exec('api_list("regex")', target='editor')   editor interpreter registry
  unity_exec('api_list("regex")', target='runtime')         runtime interpreter registry
api_list() reads the CURRENT interpreter's live registry and takes zero or more REGEX
patterns, matched case-insensitively against "name: doc". Prefer it over help() / '/?'.

Editor lifecycle tools (local process management, no GM socket involved):
  unity_start_by_path(editor_path, project_path)  first launch, explicit paths
  unity_start_by_name(project_name)               later launches, paths from the
                                                  persistent known-projects file
  unity_set_active_project(project_name)          default project for all tools
  unity_stop / unity_activate / unity_wait_ready  per-project process helpers
NOTE: unity_activate brings the editor WINDOW to the front; unity_set_active_project
selects which connected project the tools address. They are unrelated.

known projects are stored in %LOCALAPPDATA%/mcp-gm/known_projects.json (or
UC_GM_KNOWN_PROJECTS). Entries whose editor or project path no longer exists are
removed on every access. A project becomes known on the first successful hello or
unity_start_by_path call.
"""

from __future__ import annotations

import json
import os
import shutil
import signal
import subprocess
import sys
import time
from datetime import datetime
from pathlib import Path

from mcp.server.fastmcp import FastMCP

from .bridge import BridgeError, call, get_bridge, normalize_editor_path

_IS_WIN = sys.platform == "win32"
_IS_MAC = sys.platform == "darwin"
_MTL_VARS = ("MTL_DEBUG_LAYER", "MTL_SHADER_VALIDATION", "MTL_SHADER_VALIDATION_REPORT_TO_STDERR")
_DIAG_VAR = "UNITY_METALRT_DIAG"

# Expose the module docstring as MCP server instructions so clients always receive the
# channel rules and the api_list() discovery workflow up front.
mcp = FastMCP("unity-gm", instructions=__doc__)


def _run(cmd: str, timeout_ms: int = 15000, target: str | None = None,
         project: str | None = None, **kwargs) -> dict:
    try:
        return call(cmd, timeout_ms=timeout_ms, target=target, project=project, **kwargs)
    except BridgeError as exc:
        return {"ok": False, "error": str(exc)}


@mcp.tool()
def unity_info(target: str | None = None, project: str | None = None) -> dict:
    """Query the current Unity state and verify the channel is reachable.

    Returns mode (editor / editor-play / player), unityVersion, platform, scene, frame,
    pid and pendingRequests. The reply also carries the connection table (editor vs
    runtime channels, one entry per Unity instance), activeProject and knownProjects.
    ``target`` switches channel ("editor" / "runtime" / "auto" / "device") for this and
    later calls; ``project`` selects one Unity instance.
    """
    reply = _run("ping", timeout_ms=5000, target=target, project=project)
    if isinstance(reply, dict):
        reply["channel"] = get_bridge().channel_info()
    return reply


@mcp.tool()
def unity_exec(code: str, timeout_ms: int = 15000, target: str | None = None,
               project: str | None = None) -> dict:
    """Run a GM DSL snippet synchronously inside Unity and return the value of the last statement.

    Write plain DSL statements separated by ';' (it is wrapped into script{ ... } before evaluation).
    Common functions: findobj(name,type) / searchobjs(name_key,type) / getposition(obj_or_path)
                      getcomponent / getcomponents / matgetfloat(mat,key) / screenwidth() / iseditor()
    To discover APIs, call unity_exec('api_list("regex")', target='runtime') - it reads
    the LIVE interpreter registry. help() also works here (runtime channel only).

    RUNTIME CHANNEL ONLY (rejected on the editor channel; switch with target='runtime').
    Uses a persistent DslCalculator that shares the GM command/function API registry with
    the story-based GM scripts, so variables defined in an earlier call are still visible.

    Note: the DSL runs synchronously on the main thread, so an infinite loop freezes Unity.
    The logs field holds entries produced during this call; always read it.
    """
    return _run("exec", timeout_ms=timeout_ms, target=target, project=project, code=code)


@mcp.tool()
def unity_editor_exec(code: str, timeout_ms: int = 30000, target: str | None = None,
                      project: str | None = None) -> dict:
    """Run editor DSL (resource processing / AssetDatabase / editor APIs). Unity Editor only.

    code can use editor-side APIs such as check_pipeline_assets(), check_pipeline_asset_guid(),
    gettexturesetting(path), grep(...), subst(...). Calls from a built player return
    ok=false (there is no editor executor).

    To discover APIs, call unity_editor_exec('api_list("regex")', target='editor').
    NOTE: help() and unity_console('/?') do NOT work on this channel.

    EDITOR CHANNEL ONLY. Uses the same DslCalculator instance as the editor command window
    and editor_startup.dsl, so global variables/functions are shared. Only editor/resource
    APIs are available here (no runtime GM commands).
    """
    return _run("edit", timeout_ms=timeout_ms, target=target, project=project, code=code)


@mcp.tool()
def unity_console(command: str, timeout_ms: int = 15000, target: str | None = None,
                  project: str | None = None) -> dict:
    """Run a DebugConsole command (plain text legacy command) and return the handler's value.

    RUNTIME CHANNEL ONLY (rejected on the editor channel; switch with target='runtime').
    'gm <dsl>' and 'command <dsl>' are forwarded to GmRootScript -> ClientGmStorySystem,
    i.e. the story-based GM script runtime (asynchronous, results show up in logs).
    In edit mode there is no DebugConsole instance, so the command falls back to
    synchronous DSL evaluation instead.
    """
    return _run("console", timeout_ms=timeout_ms, target=target, project=project, code=command)


@mcp.tool()
def unity_logs(since: int = -1, count: int = 200, filter: str = "",
               target: str | None = None, project: str | None = None) -> dict:
    """Read the Unity log ring buffer (warnings, errors and stack traces included).

    since = -1 returns the most recent count entries; otherwise entries with seq > since are
    returned. filter is a case-insensitive substring filter. next is the cursor for the
    following call; use it with filter when logs are noisy.
    """
    return _run("logs", timeout_ms=5000, target=target, project=project,
                since=since, count=count, filter=filter)


@mcp.tool()
def unity_clearlogs(target: str | None = None, project: str | None = None) -> dict:
    """Clear the log ring buffer (does not touch Unity's own Editor.log)."""
    return _run("clearlogs", timeout_ms=5000, target=target, project=project)


# ---------------------------------------------------------------------------
# Editor lifecycle (local process management; deliberately independent of the GM socket).
# All child output is captured: this server speaks JSON-RPC over stdout and must never
# let subprocess output leak into the protocol stream.
# ---------------------------------------------------------------------------

def _proc(args: list[str], timeout: float = 10.0) -> dict:
    try:
        r = subprocess.run(args, capture_output=True, text=True, timeout=timeout)
        return {"rc": r.returncode, "stdout": (r.stdout or "").strip(),
                "stderr": (r.stderr or "").strip()}
    except (OSError, subprocess.TimeoutExpired) as exc:
        return {"rc": -1, "stdout": "", "stderr": str(exc)}


def _editor_executable(app: Path) -> Path:
    if app.is_dir() and app.suffix == ".app":
        return app / "Contents" / "MacOS" / "Unity"
    return app


def _editor_log() -> Path:
    override = os.environ.get("UNITY_EDITOR_LOG")
    if override:
        return Path(override).expanduser()
    if _IS_WIN:
        base = os.environ.get("LOCALAPPDATA") or (Path.home() / "AppData" / "Local")
        return Path(base) / "Unity" / "Editor" / "Editor.log"
    if _IS_MAC:
        return Path.home() / "Library" / "Logs" / "Unity" / "Editor.log"
    return Path.home() / ".config" / "unity3d" / "Editor.log"


def _unity_processes() -> list[tuple[int, str]]:
    """Return (pid, commandline) of Unity *editor main* processes."""
    if _IS_WIN:
        script = ("ConvertTo-Json -Compress -InputObject @("
                  "Get-CimInstance Win32_Process -Filter \"Name='Unity.exe'\" | "
                  "Select-Object ProcessId,CommandLine)")
        r = _proc(["powershell", "-NoProfile", "-NonInteractive", "-Command", script], timeout=20)
        if r["rc"] == 0 and r["stdout"]:
            try:
                data = json.loads(r["stdout"])
            except (ValueError, TypeError):
                data = None
            if data is not None:
                if isinstance(data, dict):
                    data = [data]
                out = []
                for item in data:
                    pid = item.get("ProcessId")
                    if pid is not None:
                        out.append((int(pid), str(item.get("CommandLine") or "")))
                if out:
                    return out
        t = _proc(["tasklist", "/FI", "IMAGENAME eq Unity.exe", "/FO", "CSV", "/NH"], timeout=15)
        pids = []
        for line in t["stdout"].splitlines():
            fields = [f.strip('" ') for f in line.split('","')]
            if len(fields) >= 2 and fields[1].isdigit():
                pids.append((int(fields[1]), ""))
        return pids
    r = _proc(["ps", "-axo", "pid=,command="], timeout=10)
    out = []
    own = {os.getpid(), os.getppid()}
    for line in r["stdout"].splitlines():
        parts = line.strip().split(None, 1)
        if len(parts) != 2 or not parts[0].isdigit():
            continue
        pid = int(parts[0])
        if pid in own:
            continue
        cmd = parts[1]
        # Only argv[0] may identify an editor. Matching the whole command line would
        # match this very MCP/shell command whenever it mentions the Unity path.
        argv0 = cmd.split(None, 1)[0].strip().strip('"').strip("'")
        is_main = argv0.endswith("/Contents/MacOS/Unity") or argv0.endswith("/Unity.exe")
        if not _IS_MAC and argv0.endswith("/Unity"):
            is_main = True
        if is_main:
            out.append((pid, cmd))
    return out


def _unity_pids() -> list[int]:
    return [pid for pid, _ in _unity_processes()]


def _alive(pids: list[int]) -> list[int]:
    alive = []
    for pid in pids:
        if _IS_WIN:
            r = _proc(["tasklist", "/FI", f"PID eq {pid}", "/NH"], timeout=5)
            if str(pid) in r["stdout"]:
                alive.append(pid)
        else:
            if _proc(["kill", "-0", str(pid)], timeout=3)["rc"] == 0:
                alive.append(pid)
    return alive


def _terminate(pid: int, force: bool) -> str | None:
    if _IS_WIN:
        args = ["taskkill", "/PID", str(pid), "/T"]
        if force:
            args.append("/F")
        r = _proc(args, timeout=15)
        return None if r["rc"] == 0 else (r["stderr"] or r["stdout"] or f"rc={r['rc']}")
    try:
        os.kill(pid, getattr(signal, "SIGKILL", signal.SIGTERM) if force else signal.SIGTERM)
        return None
    except (ProcessLookupError, PermissionError, OSError) as exc:
        return str(exc)


def _launch_command(app: Path, project: Path, extra_args: list[str]) -> list[str]:
    # Launch the bundle executable DIRECTLY on macOS. `open -n -a` delivers an AppleEvent
    # that Unity handles synchronously in applicationDidFinishLaunching; that path was
    # observed to deadlock in InitializeProject. Direct Popen bypasses the AppleEvent.
    if _IS_MAC and os.environ.get("UNITY_MCP_USE_OPEN") == "1" and app.is_dir() and app.suffix == ".app":
        return ["open", "-n", "-a", str(app), "--args", "-projectPath", str(project)] + extra_args
    return [str(_editor_executable(app)), "-projectPath", str(project)] + extra_args


def _uses_open(cmd: list[str]) -> bool:
    return cmd[0] == "open"


def _validate_launch_paths(editor_path: str, project_path: str) -> tuple[Path | None, Path | None, dict | None]:
    """Validate explicit launch paths. Returns (app, project, error)."""
    known = list(get_bridge().known_projects()["projects"])
    hint = ("Use unity_start_by_path(editor_path, project_path) with the full Unity.exe / "
            "Unity.app path and the full Unity project root. "
            "Known projects: " + (", ".join(known) if known else "(none)"))
    if not editor_path or not str(editor_path).strip():
        return None, None, {"ok": False, "error": "editor_path is empty.", "hint": hint,
                            "knownProjects": known}
    if not project_path or not str(project_path).strip():
        return None, None, {"ok": False, "error": "project_path is empty.", "hint": hint,
                            "knownProjects": known}
    app = Path(str(editor_path)).expanduser()
    if not app.exists():
        return None, None, {"ok": False, "error": f"Unity editor not found: {app}",
                            "hint": hint, "knownProjects": known}
    if app.is_dir():
        if app.suffix != ".app":
            return None, None, {"ok": False,
                                "error": f"editor_path is a directory but not a macOS .app bundle: {app}",
                                "hint": hint, "knownProjects": known}
    elif _IS_WIN and app.suffix.lower() != ".exe":
        return None, None, {"ok": False, "error": f"editor_path must be a Unity.exe on Windows: {app}",
                            "hint": hint, "knownProjects": known}
    project = Path(str(project_path)).expanduser()
    if not project.is_dir():
        return None, None, {"ok": False, "error": f"Unity project directory not found: {project}",
                            "hint": hint, "knownProjects": known}
    if not (project / "ProjectSettings" / "ProjectVersion.txt").is_file():
        return None, None, {"ok": False,
                            "error": f"Not a Unity project (no ProjectSettings/ProjectVersion.txt): {project}",
                            "hint": hint, "knownProjects": known}
    if normalize_editor_path(str(app)) != str(app):
        app = Path(normalize_editor_path(str(app)))
    return app, project, None


def _start_editor(app: Path, project: Path, force: bool, diag: int | None,
                  validation: bool, dry_run: bool, extra_env: dict[str, str] | None,
                  source: str) -> dict:
    extra_args = ["useDynamicHeapAllocator =1", "isLowMemoryPlatform=0"]
    cmd = _launch_command(app, project, extra_args)
    log = _editor_log()
    startup_env: dict[str, str] = {}
    warnings: list[str] = []
    if _IS_MAC:
        if validation:
            startup_env.update({v: "1" for v in _MTL_VARS})
        if diag is not None:
            startup_env[_DIAG_VAR] = str(diag)
    else:
        if validation:
            warnings.append("validation=True is macOS-only (Metal); ignored on this platform.")
        if diag is not None:
            warnings.append("diag sets a Metal RT level; it has no effect on this platform.")
    if extra_env:
        startup_env.update({str(k): str(v) for k, v in extra_env.items()})

    result: dict = {"ok": True, "executed": False, "platform": sys.platform,
                    "source": source, "project": str(project), "editor": str(app),
                    "editor_executable": str(_editor_executable(app)),
                    "editor_log": str(log), "command": " ".join(cmd),
                    "startup_env": startup_env}
    if warnings:
        result["warnings"] = warnings

    # Only an editor already running on THIS project conflicts (Temp/UnityLockfile).
    # Other projects are allowed to run at the same time.
    project_key = str(project).lower()
    existing = [pid for pid, command in _unity_processes()
                if project_key in command.lower()]
    result["preexisting_pids"] = existing
    if existing and not force:
        result["ok"] = False
        result["error"] = ("An editor is already running on this project; stop it first or "
                           "pass force=True.")
        return result
    if dry_run:
        return result

    # A crashed/stopped editor leaves /tmp/Unity-Upm-<pid>.sock behind. The next launch
    # can stall indefinitely at "Successfully changed project path". Only clean up when
    # no editor is running, so a live editor's socket is never touched.
    if not _unity_pids():
        dirs = [Path("/tmp")] if not _IS_WIN else []
        env_tmp = os.environ.get("TMPDIR")
        if env_tmp:
            dirs.append(Path(env_tmp))
        removed = []
        for tmp in dict.fromkeys(dirs):
            try:
                for sock in tmp.glob("Unity-Upm-*.sock"):
                    try:
                        sock.unlink()
                        removed.append(str(sock))
                    except OSError:
                        pass
            except OSError:
                pass
        if removed:
            result["stale_upm_sockets_removed"] = removed

    if log.is_file():
        stamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        backup = Path(os.environ.get("TMPDIR", "/tmp" if not _IS_WIN else str(Path.home()))) / f"editor_before_mcp_{stamp}.log"
        try:
            shutil.copy2(log, backup)
            result["log_backup"] = str(backup)
        except OSError as exc:
            result["log_backup_error"] = str(exc)

    launch_env = {k: v for k, v in startup_env.items() if not k.startswith("__")}
    pid = None
    if _uses_open(cmd):
        try:
            for key, val in launch_env.items():
                r = _proc(["launchctl", "setenv", key, val], timeout=5)
                if r["rc"] != 0:
                    result.setdefault("env_warnings", []).append(f"setenv {key}: {r['stderr']}")
            r = _proc(cmd, timeout=20)
            result["open_rc"] = r["rc"]
            if r["stderr"]:
                result["open_stderr"] = r["stderr"]
            if r["rc"] != 0:
                result["ok"] = False
                result["error"] = f"`open` failed (rc={r['rc']})."
                return result
            time.sleep(2)
        finally:
            for key in launch_env:
                _proc(["launchctl", "unsetenv", key], timeout=5)
            result["env_cleaned"] = bool(launch_env)
        for _ in range(20):
            new_pids = [p for p in _unity_pids() if p not in existing]
            if new_pids:
                pid = new_pids[0]
                break
            time.sleep(0.5)
    else:
        env = os.environ.copy()
        env.update(launch_env)
        kwargs: dict = {"env": env, "stdin": subprocess.DEVNULL,
                        "stdout": subprocess.DEVNULL, "stderr": subprocess.DEVNULL}
        if not _IS_WIN:
            kwargs["start_new_session"] = True
        if _IS_WIN:
            kwargs["creationflags"] = (getattr(subprocess, "DETACHED_PROCESS", 0) |
                                       getattr(subprocess, "CREATE_NEW_PROCESS_GROUP", 0))
            kwargs["close_fds"] = True
        try:
            process = subprocess.Popen(cmd, **kwargs)
            pid = process.pid
        except OSError as exc:
            result["ok"] = False
            result["error"] = f"Failed to launch editor: {exc}"
            return result

    result["pid"] = pid
    result["executed"] = True
    result["registeredProject"] = get_bridge().register_project(str(app), str(project), pid or 0)
    get_bridge().drop_project(Path(str(project)).name)
    result["hint"] = ("Unity connects out to the MCP server after compiling; it retries every "
                      "10s. Poll unity_wait_ready() for readiness.")
    return result


@mcp.tool()
def unity_start_by_path(editor_path: str, project_path: str, force: bool = False,
                        diag: int | None = None, validation: bool = False,
                        dry_run: bool = False,
                        extra_env: dict[str, str] | None = None) -> dict:
    """FIRST launch of a Unity project: both the editor and the project path are required.

    editor_path: full path to Unity.exe (Windows) or Unity.app (macOS). The
    .../Unity.app/Contents/MacOS/Unity executable is also accepted and normalized.
    project_path: full path to the Unity project root (must contain
    ProjectSettings/ProjectVersion.txt).
    On success the pair is persisted, so later launches only need the project name
    (unity_start_by_name). Any invalid argument returns ok=false with the reason,
    a hint and the list of known projects.

    force: allow a new instance even if one is already running on this project (two
           instances of the SAME project fight over Temp/UnityLockfile). Other
           projects running at the same time are fine.
    dry_run: report resolved paths, command and env without launching anything.
    """
    app, project, error = _validate_launch_paths(editor_path, project_path)
    if error:
        return error
    return _start_editor(app, project, force, diag, validation, dry_run, extra_env,
                         "unity_start_by_path")


@mcp.tool()
def unity_start_by_name(project_name: str, force: bool = False, diag: int | None = None,
                        validation: bool = False, dry_run: bool = False,
                        extra_env: dict[str, str] | None = None) -> dict:
    """Launch a project that MCP already knows (second and later launches).

    project_name: directory name of the Unity project, e.g. "TestUC". The editor and
    project paths come from the persisted known-projects file; stale entries (missing
    editor or project path) are removed automatically. Unknown names return ok=false
    with the list of known projects and a hint to call unity_start_by_path first.
    """
    known = get_bridge().known_projects()
    entry = known["projects"].get(Path(project_name).name)
    if not entry:
        return {"ok": False, "error": f"Unknown project {project_name!r}.",
                "hint": ("Call unity_start_by_path(editor_path, project_path) once to "
                         "register it, or unity_info() to inspect connections."),
                "knownProjects": list(known["projects"]), "removed": known["removed"]}
    app, project, error = _validate_launch_paths(entry["editorPath"], entry["projectPath"])
    if error:
        return error
    return _start_editor(app, project, force, diag, validation, dry_run, extra_env,
                         "unity_start_by_name")


@mcp.tool()
def unity_set_active_project(project_name: str) -> dict:
    """Select the current project so every following tool call can omit ``project``.

    project_name must be a known project (registered by a hello or by
    unity_start_by_path) or a currently connected project. The selection is persisted
    and survives an MCP server restart. This does NOT change the channel: use
    target='editor' / 'runtime' for that, and unity_activate() to raise the window.
    """
    try:
        return get_bridge().set_active_project(Path(project_name).name)
    except BridgeError as exc:
        return {"ok": False, "error": str(exc),
                "hint": "Call unity_info() for known/connected projects, or "
                        "unity_start_by_path(...) to register a new one.",
                "knownProjects": list(get_bridge().known_projects()["projects"])}


def _resolve_target_pids(project: str | None) -> tuple[list[int], str | None]:
    bridge = get_bridge()
    known = bridge.known_projects()
    name = Path(project).name if project else known["activeProject"]
    if name:
        pids = list(bridge.peer_pids(name))
        entry = known["projects"].get(name)
        if entry:
            key = str(entry["projectPath"]).lower()
            for pid, command in _unity_processes():
                if key in command.lower() and pid not in pids:
                    pids.append(pid)
        return pids, name
    pids = _unity_pids()
    if len(pids) > 1:
        raise BridgeError("Multiple Unity editors are running but no project is active. "
                          "Pass project=... or call unity_set_active_project(...).")
    return pids, None


@mcp.tool()
def unity_stop(project: str | None = None, force: bool = False,
               dry_run: bool = False) -> dict:
    """Quit the Unity Editor for one project (macOS / Windows).

    Targets the active project, or ``project`` when given. pid is taken from the
    hello handshake and cross-checked against the process command line. When several
    editors are running and no project is active, pass project=... explicitly.
    force: force-kill survivors. dry_run: report what would be terminated.
    """
    try:
        pids, name = _resolve_target_pids(project)
    except BridgeError as exc:
        return {"ok": False, "error": str(exc),
                "hint": "Call unity_set_active_project(...) or pass project=... ."}
    if not pids:
        return {"ok": True, "already_stopped": True, "pids": [], "project": name,
                "platform": sys.platform}
    if dry_run:
        graceful = "taskkill /PID /T" if _IS_WIN else "SIGTERM"
        forced = "taskkill /PID /T /F" if _IS_WIN else "SIGKILL"
        return {"ok": True, "executed": False, "platform": sys.platform, "project": name,
                "pids": pids, "would_send": graceful + (f" then {forced}" if force else "")}

    errors = []
    for pid in pids:
        err = _terminate(pid, force=False)
        if err:
            errors.append(f"{pid}: {err}")
    remaining = _alive(pids)
    for _ in range(16):
        if not remaining:
            break
        remaining = _alive(remaining)
        time.sleep(0.5)

    forced: list[int] = []
    if remaining and force:
        for pid in remaining:
            err = _terminate(pid, force=True)
            if err:
                errors.append(f"{pid}: {err}")
            else:
                forced.append(pid)
        for _ in range(6):
            remaining = _alive(remaining)
            if not remaining:
                break
            time.sleep(0.5)

    if name:
        get_bridge().drop_project(name)
    return {"ok": not remaining, "executed": True, "platform": sys.platform,
            "project": name, "pids": pids,
            "stopped": [p for p in pids if p not in remaining], "forced": forced,
            "remaining": remaining, "errors": errors,
            "hint": None if not remaining else "Editor did not exit; retry with force=True."}


@mcp.tool()
def unity_activate(project: str | None = None) -> dict:
    """Bring the Unity Editor WINDOW to the front (macOS / Windows).

    Targets the active project, or ``project`` when given. IMPORTANT for GPU/RT
    verification: a background editor is throttled by the OS and barely repaints.
    This is unrelated to unity_set_active_project (which selects the tool target).
    """
    try:
        pids, name = _resolve_target_pids(project)
    except BridgeError as exc:
        return {"ok": False, "error": str(exc)}
    if not pids:
        return {"ok": False, "error": "No Unity process to activate.", "project": name}
    if _IS_WIN:
        script = f"(New-Object -ComObject WScript.Shell).AppActivate({pids[0]})"
        r = _proc(["powershell", "-NoProfile", "-NonInteractive", "-Command", script], timeout=12)
        return {"ok": r["rc"] == 0, "platform": sys.platform, "project": name,
                "pid": pids[0], "stdout": r["stdout"][:200],
                "error": r["stderr"][:200] or None}
    if _IS_MAC:
        procs = _unity_processes()
        if not procs:
            return {"ok": False, "platform": sys.platform, "project": name,
                    "error": "No Unity Editor is running. Activating would launch an instance "
                             "without a project; call unity_start_by_path() first."}
        known = get_bridge().known_projects()
        entry = known["projects"].get(name) if name else None
        if entry:
            key = str(entry["projectPath"])
            if not any("-projectPath" in cmd and key in cmd for _, cmd in procs):
                return {"ok": False, "platform": sys.platform, "project": name,
                        "error": "A Unity Editor is running but its command line does not show "
                                 f"-projectPath {key}. It may be stuck at the project picker.",
                        "processes": [{"pid": pid, "command": cmd[:200]} for pid, cmd in procs]}
        r = _proc(["osascript", "-e", 'tell application "Unity" to activate'], timeout=12)
        return {"ok": r["rc"] == 0, "platform": sys.platform, "project": name,
                "error": (r["stderr"] or r["stdout"])[:200] or None,
                "hint": None if r["rc"] == 0 else "AppleEvent timed out: editor may be wedged."}
    return {"ok": False, "error": f"Unsupported platform: {sys.platform}"}


@mcp.tool()
def unity_wait_ready(timeout_seconds: float = 180.0, interval: float = 2.0,
                     target: str = "editor", project: str | None = None) -> dict:
    """Block until the requested Unity GM connection answers ping.

    Unity connects out to the MCP server after the project finishes compiling and
    retries every 10s, so this waits for that connection instead of a listening port.
    Returns ok=True with mode/channel on success, ok=False with the last error on
    timeout. Note: this blocks this MCP server for up to timeout_seconds.
    """
    started = time.monotonic()
    deadline = started + max(1.0, timeout_seconds)
    attempts = 0
    last_error = None
    while time.monotonic() < deadline:
        attempts += 1
        reply = _run("ping", timeout_ms=5000, target=target, project=project)
        if isinstance(reply, dict) and reply.get("ok"):
            info = reply.get("result")
            return {"ok": True, "attempts": attempts,
                    "elapsed_seconds": round(time.monotonic() - started, 1),
                    "mode": info.get("mode") if isinstance(info, dict) else None,
                    "channel": get_bridge().channel_info()}
        last_error = reply.get("error") if isinstance(reply, dict) else str(reply)
        time.sleep(max(0.5, interval))
    return {"ok": False, "error": "Unity GM channel did not become ready in time.",
            "attempts": attempts, "elapsed_seconds": round(time.monotonic() - started, 1),
            "last_error": last_error,
            "hint": "Check unity_info() for the listener state and connected channels."}


@mcp.tool()
def unity_editor_log(lines: int = 80, filter: str = "", path: str | None = None) -> dict:
    """Read the tail of Unity's Editor.log (the file on disk, not the GM log ring buffer).

    lines:  max number of lines to return (1..2000).
    filter: optional case-insensitive substring, e.g. 'Caused GPU Hang', 'Shader'.
    path:   override the log path (default from UNITY_EDITOR_LOG or platform default).

    Returns the matched lines plus path/size/mtime, so a hang snapshot can be read without
    shell access. Without a filter only the last 512 KB is scanned (fast tail); with a
    filter the whole file is scanned, since startup lines can be far behind the tail.
    """
    log = Path(path).expanduser() if path else _editor_log()
    if not log.is_file():
        return {"ok": False, "error": f"Editor.log not found: {log}", "path": str(log)}
    try:
        with log.open("rb") as f:
            f.seek(0, os.SEEK_END)
            size = f.tell()
            f.seek(0 if filter else max(0, size - 512 * 1024))
            data = f.read()
        text = data.decode("utf-8", "replace").splitlines()
        if filter:
            needle = filter.lower()
            text = [line for line in text if needle in line.lower()]
        wanted = max(1, min(int(lines), 2000))
        tail = text[-wanted:]
        stat = log.stat()
        return {"ok": True, "path": str(log), "size": size,
                "mtime": datetime.fromtimestamp(stat.st_mtime).isoformat(timespec="seconds"),
                "count": len(tail), "total_matches": len(text), "lines": tail}
    except OSError as exc:
        return {"ok": False, "error": str(exc), "path": str(log)}


def main() -> None:
    print("[mcp-gm] serving 'unity-gm' over stdio", file=sys.stderr, flush=True)
    get_bridge().start()
    mcp.run()


if __name__ == "__main__":
    main()
