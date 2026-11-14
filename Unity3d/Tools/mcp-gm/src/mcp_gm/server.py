"""MCP server exposing the Unity GM channel as tools.

Tool results are the raw Unity reply dict:
  success {"id":..,"ok":true,"result":...,"elapsedMs":..,"logs":[..]}
  failure {"ok":false,"error":".."}

Channels: every tool accepts an optional ``target`` ("editor" / "runtime" / "auto").
The default is "auto" (editor port 39528 first, runtime 39527 as fallback). Passing a
target switches the channel for this AND all following calls, so the usual workflow is
to work on the editor channel, then pass target="runtime" on the first call after Play
starts. UC_GM_TARGET sets the startup default; UC_GM_PORT forces an exact port.

Script execution is channel-restricted (enforced by the bridge before sending):
  editor channel  -> unity_editor_exec only
  runtime channel -> unity_exec / unity_console only
  both channels   -> unity_info / unity_logs / unity_clearlogs
unity_info reports ``channel`` and ``allowed_cmds`` for the live connection.

Interpreters (Unity side, all shared with the existing systems):
  unity_exec        -> one persistent DslCalculator in GmCommandApi, sharing the GM
                       command/function registry with the story-based GM scripts.
  unity_editor_exec -> the same DslCalculator used by the editor command window and
                       editor_startup.dsl (ResourceEditUtility.GetCommandCalculator),
                       with editor APIs only; runtime GM commands are not merged in.
  unity_console     -> DebugConsole.ExecuteForResult when a DebugConsole exists.
                       "gm <dsl>" / "command <dsl>" go through GmRootScript into the
                       story-based GM script runtime there.

Discovering DSL APIs (IMPORTANT - the two channels have different registries):
  unity_editor_exec('api_list("regex")', target='editor')   editor interpreter registry
  unity_exec('api_list("regex")', target='runtime')         runtime interpreter registry
api_list() reads the CURRENT interpreter's live registry and takes zero or more REGEX
patterns (like helpall), matched case-insensitively against "name: doc"; any hit includes
the row. Examples:
    api_list()                       every API (incl. built-in operators)
    api_list("window_")              substring-style regex
    api_list("^scene_", "cam_")      multiple patterns, OR semantics
    api_list("(?i)focus.*object")    full regex syntax
It returns `source=Calculator.ApiDocs total=N patterns=M`, the matched `name: doc` lines,
and `shown=K`. Prefer it over help() / '/?': help() is runtime-only (returns 0 on the
editor channel) and unity_console('/?') is rejected there.
All DSL calls need a LIVE GM socket. If the editor is closed, run unity_start first;
then use api_list() to confirm an API is actually registered before calling it.

Editor lifecycle tools (local process management, no GM socket involved):
  unity_start       -> launch the Unity Editor with this project (-projectPath).
  unity_stop        -> terminate the running Unity Editor.
  unity_start(dry_run=True) reports resolved paths/command without launching anything.

Supported on macOS and Windows. Paths are discovered in this order (first hit wins):
  UNITY_EDITOR_APP / UNITY_EDITOR_EXE   editor .app (macOS) or Unity.exe (Windows)
  UNITY_PROJECT_ROOT                    project root (default: parent of Tools/mcp-gm)
  UNITY_ENGINE_ROOT                     engine source root (default: <project>/../unity_engine)
  UNITY_EDITOR_LOG                      Editor.log path, if it is not in the default place
If no explicit editor path is given, platform build outputs and Unity Hub installs are
probed. These variables can be set in the MCP client config's ``env`` block, so the same
mcp.json works on both platforms when the values are provided there.
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

from .bridge import BridgeError, call, get_bridge

_IS_WIN = sys.platform == "win32"
_IS_MAC = sys.platform == "darwin"
_MTL_VARS = ("MTL_DEBUG_LAYER", "MTL_SHADER_VALIDATION", "MTL_SHADER_VALIDATION_REPORT_TO_STDERR")
_DIAG_VAR = "UNITY_METALRT_DIAG"

# Expose the module docstring as MCP server instructions so clients always receive the
# channel rules and the api_list() discovery workflow up front, instead of only seeing
# them when they happen to inspect a tool description.
mcp = FastMCP("unity-gm", instructions=__doc__)


def _run(cmd: str, timeout_ms: int = 15000, target: str | None = None, **kwargs) -> dict:
    try:
        return call(cmd, timeout_ms=timeout_ms, target=target, **kwargs)
    except BridgeError as exc:
        return {"ok": False, "error": str(exc)}


@mcp.tool()
def unity_info(target: str | None = None) -> dict:
    """Query the current Unity state and verify the channel is reachable.

    Returns mode (editor / editor-play / player), unityVersion, platform, scene, frame,
    pid and pendingRequests. Call this first when unsure whether you are talking to the
    editor or to the running game, or when the connection is suspect.
    The reply also carries ``channel`` (target / endpoint / port) so the mode can be
    cross-checked against the port actually connected.
    ``target`` switches channel ("editor" / "runtime" / "auto") for this and later calls.
    """
    reply = _run("ping", timeout_ms=5000, target=target)
    if isinstance(reply, dict):
        reply["channel"] = get_bridge().channel_info()
    return reply


@mcp.tool()
def unity_exec(code: str, timeout_ms: int = 15000, target: str | None = None) -> dict:
    """Run a GM DSL snippet synchronously inside Unity and return the value of the last statement.

    Write plain DSL statements separated by ';' (it is wrapped into script{ ... } before evaluation).
    Common functions: findobj(name,type) / searchobjs(name_key,type) / getposition(obj_or_path)
                      getcomponent / getcomponents / matgetfloat(mat,key) / screenwidth() / iseditor()
    Commands (log only, no return value): logobjs(name_key,type) / logcomps(...) / setmat(...) / setactive(...)
    To discover APIs, call unity_exec('api_list("regex")', target='runtime') - it reads
    the LIVE interpreter registry (zero or more regex patterns, e.g. api_list("^cam_");
    see the module instructions for full syntax). help() also works here (runtime channel
    only), but '/?' via unity_console is limited. A live GM socket is required.

    RUNTIME CHANNEL ONLY (rejected on the editor channel; switch with target='runtime').
    Uses a persistent DslCalculator that shares the GM command/function API registry with
    the story-based GM scripts, so variables defined in an earlier call are still visible.
    It does NOT share the story runtime itself (stories run across ticks with no return value).

    Note: the DSL runs synchronously on the main thread, so an infinite loop freezes Unity
    (the client only sees a timeout). The logs field holds entries produced during this call;
    most GM commands report through logs only, so always read it.
    """
    return _run("exec", timeout_ms=timeout_ms, target=target, code=code)


@mcp.tool()
def unity_editor_exec(code: str, timeout_ms: int = 30000, target: str | None = None) -> dict:
    """Run editor DSL (resource processing / AssetDatabase / editor APIs). Unity Editor only.

    code can use editor-side APIs such as check_pipeline_assets(), check_pipeline_asset_guid(),
    gettexturesetting(path), grep(...), subst(...). Calls from a built player return
    ok=false (there is no editor executor).

    To discover APIs, call unity_editor_exec('api_list("regex")', target='editor') - it
    reads the LIVE editor interpreter registry (zero or more regex patterns, e.g.
    api_list("^scene_cam_"); see the module instructions for full syntax).
    NOTE: help() and unity_console('/?') do NOT work on this channel - help() is
    runtime-only (returns 0) and the console command is rejected here. A live GM socket
    is required.

    EDITOR CHANNEL ONLY. Uses the same DslCalculator instance as the editor command window
    and editor_startup.dsl, so global variables/functions are shared. Only editor/resource
    APIs are available here (no runtime GM commands). A private script name is used for
    socket calls so the command window's "main" script is never overwritten.
    """
    return _run("edit", timeout_ms=timeout_ms, target=target, code=code)


@mcp.tool()
def unity_console(command: str, timeout_ms: int = 15000, target: str | None = None) -> dict:
    """Run a DebugConsole command (plain text legacy command) and return the handler's value.

    RUNTIME CHANNEL ONLY (rejected on the editor channel; switch with target='runtime').
    This IS the DebugConsole channel: with a live DebugConsole it calls
    DebugConsole.ExecuteForResult directly. Examples: 'sys' (environment info),
    '/? setmat' (search GM commands), 'script <file>'.

    'gm <dsl>' and 'command <dsl>' are forwarded to GmRootScript.OnExecCommand ->
    ClientGmStorySystem, i.e. the story-based GM script runtime. Use this when you need
    GM scripts specifically (asynchronous, runs across ticks, no return value); results
    show up in the logs field / Editor.log rather than as a return value.

    In edit mode there is no DebugConsole instance, so the command falls back to
    synchronous DSL evaluation instead.
    """
    return _run("console", timeout_ms=timeout_ms, target=target, code=command)


@mcp.tool()
def unity_logs(since: int = -1, count: int = 200, filter: str = "",
               target: str | None = None) -> dict:
    """Read the Unity log ring buffer (warnings, errors and stack traces included).

    since = -1 returns the most recent count entries; otherwise entries with seq > since are
    returned. filter is a case-insensitive substring filter (e.g. 'Metal', 'Shader').
    next is the cursor for the following call; use it with filter when logs are noisy.
    """
    return _run("logs", timeout_ms=5000, target=target, since=since, count=count, filter=filter)


@mcp.tool()
def unity_clearlogs(target: str | None = None) -> dict:
    """Clear the log ring buffer (does not touch Unity's own Editor.log)."""
    return _run("clearlogs", timeout_ms=5000, target=target)


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


def _project_root() -> Path:
    override = os.environ.get("UNITY_PROJECT_ROOT")
    if override:
        return Path(override).expanduser().resolve()
    # <project>/Tools/mcp-gm/src/mcp_gm/server.py -> parents[4] is <project>
    return Path(__file__).resolve().parents[4]


def _engine_root() -> Path:
    override = os.environ.get("UNITY_ENGINE_ROOT")
    if override:
        return Path(override).expanduser()
    return _project_root().parent / "unity_engine"


def _explicit_editor() -> Path | None:
    for key in ("UNITY_EDITOR_APP", "UNITY_EDITOR_EXE"):
        value = os.environ.get(key)
        if value:
            return Path(value).expanduser()
    return None


def _editor_candidates() -> list[Path]:
    explicit = _explicit_editor()
    if explicit:
        return [explicit]
    engine = _engine_root()
    if _IS_WIN:
        out = [engine / "build" / "WindowsEditor" / "x64" / "Debug" / "Unity.exe",
               engine / "build" / "WinEditor" / "x64" / "Debug" / "Unity.exe"]
        hub = Path(os.environ.get("PROGRAMFILES", r"C:\Program Files")) / "Unity" / "Hub" / "Editor"
        if hub.is_dir():
            out.extend(sorted(hub.glob("*/Editor/Unity.exe"), reverse=True))
        return out
    if _IS_MAC:
        out = [engine / "build" / "MacEditor" / "arm64" / "Debug" / "Unity.app",
               engine / "build" / "MacEditor" / "x64" / "Debug" / "Unity.app"]
        hub = Path("/Applications/Unity/Hub/Editor")
        if hub.is_dir():
            out.extend(sorted(hub.glob("*/Unity.app"), reverse=True))
        return out
    return [engine / "build" / "LinuxEditor" / "x64" / "Debug" / "Unity"]


def _resolve_editor() -> tuple[Path | None, list[str]]:
    candidates = _editor_candidates()
    for candidate in candidates:
        if candidate.is_dir() or candidate.is_file():
            return candidate, [str(c) for c in candidates]
    return None, [str(c) for c in candidates]


def _editor_executable(app: Path) -> Path:
    if _IS_MAC and app.is_dir() and app.suffix == ".app":
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
        # Fallback for PowerShell builds without the CIM cmdlets: tasklist still gives us
        # the pids (but not command lines).
        t = _proc(["tasklist", "/FI", "IMAGENAME eq Unity.exe", "/FO", "CSV", "/NH"], timeout=15)
        pids = []
        for line in t["stdout"].splitlines():
            fields = [f.strip('" ') for f in line.split('","')]
            if len(fields) >= 2 and fields[1].isdigit():
                pids.append((int(fields[1]), ""))
        return pids
    r = _proc(["ps", "-axo", "pid=,command="], timeout=10)
    out = []
    explicit = _explicit_editor()
    explicit_exe = str(_editor_executable(explicit)) if explicit else None
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
        # match this very MCP/shell command whenever it mentions the Unity path (e.g.
        # `pgrep -fl 'Unity.app/Contents/MacOS/Unity'`), making unity_start think an
        # editor is already running.
        argv0 = cmd.split(None, 1)[0].strip().strip('"').strip("'")
        is_main = argv0.endswith("/Contents/MacOS/Unity") or argv0.endswith("/Unity.exe")
        if explicit_exe and argv0 == explicit_exe:
            is_main = True
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
    # observed to deadlock in InitializeProject -> GetRecentProjects -> open() (main thread
    # stuck for tens of minutes, UPM idle in uv__io_poll, GM socket never opens). Direct
    # Popen bypasses the AppleEvent entirely: same project, ready in ~12s. Verified over
    # a dozen failed `open` launches. Set UNITY_MCP_USE_OPEN=1 for the old behaviour.
    if _IS_MAC and os.environ.get("UNITY_MCP_USE_OPEN") == "1" and app.is_dir() and app.suffix == ".app":
        return ["open", "-n", "-a", str(app), "--args", "-projectPath", str(project)] + extra_args
    return [str(_editor_executable(app)), "-projectPath", str(project)] + extra_args


def _uses_open(cmd: list[str]) -> bool:
    return cmd[0] == "open"


@mcp.tool()
def unity_start(force: bool = False, diag: int | None = None,
                validation: bool = False, dry_run: bool = False,
                extra_env: dict[str, str] | None = None) -> dict:
    """Launch the Unity Editor and open the current project (macOS / Windows).

    Always passes -projectPath, so the editor never stops at the project picker.
    macOS: open -n -a <Unity.app> --args -projectPath <project> ...
    Windows: launches Unity.exe directly with the same arguments.

    Paths are configurable via UNITY_EDITOR_APP / UNITY_EDITOR_EXE / UNITY_PROJECT_ROOT /
    UNITY_ENGINE_ROOT / UNITY_EDITOR_LOG (see module docs). Set them in the MCP client's
    env block to use one config on both platforms; otherwise platform defaults are probed.

    force:       allow a new instance even if one is already running (two instances would
                 otherwise fight over Temp/UnityLockfile). Usually leave False.
    diag:        one-shot startup seed for EngineGlobalVar 112 (Metal RT diagnostic level),
                 e.g. 1 = RT off, 6 = quarter grid, 8 = 1x1 grid. macOS-only in practice.
    validation:  enable Metal API / GPU / Shader Validation (macOS only). NOTE this can
                 abort the editor on an API violation and bring up UnityBugReporter.
    extra_env:   additional one-shot environment variables injected into the launched
                 editor, e.g. {"MTL_DEBUG_LAYER": "1"} to isolate one Metal validation
                 layer without enabling the others. These are cleared after launch.
    dry_run:     report resolved paths, command and env without launching anything.

    Returns the launched pid, resolved paths, the command, the Editor.log backup (Unity
    rewrites Editor.log on every launch) and any startup env vars. The editor can take
    several minutes to finish importing; poll unity_info() for readiness.
    """
    project = _project_root()
    app, tried = _resolve_editor()
    if app is None:
        return {"ok": False, "error": "Unity Editor not found. Set UNITY_EDITOR_APP (macOS .app) "
                "or UNITY_EDITOR_EXE (Windows Unity.exe).", "tried": tried,
                "project": str(project), "platform": sys.platform}
    if not (project / "ProjectSettings" / "ProjectVersion.txt").is_file():
        return {"ok": False, "error": f"Not a Unity project (no ProjectVersion.txt): {project}. "
                "Set UNITY_PROJECT_ROOT.", "editor": str(app)}

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
                    "project": str(project), "editor": str(app), "editor_executable": str(_editor_executable(app)),
                    "editor_log": str(log), "command": " ".join(cmd),
                    "startup_env": startup_env, "tried": tried}
    if warnings:
        result["warnings"] = warnings

    existing = _unity_pids()
    result["preexisting_pids"] = existing
    if existing and not force:
        result["ok"] = False
        result["error"] = "An editor is already running; stop it first or pass force=True."
        return result
    if dry_run:
        return result

    # A crashed/stopped editor leaves /tmp/Unity-Upm-<pid>.sock behind. The next launch
    # can then stall indefinitely at "Successfully changed project path" (main thread
    # idle in nextEventMatchingMask, CPU 0%, GM socket never opens). Verified over five
    # separate sessions: removing stale sockets before launch fixes it. Only do this
    # when no editor is running, so a live editor's socket is never touched.
    if not existing:
        # UPM writes to the literal /tmp on macOS, NOT $TMPDIR (which is
        # /var/folders/.../T/ in a GUI session). Scan both or the cleanup is a no-op.
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
        # GUI apps launched via `open` do not inherit this process's environment. launchctl
        # setenv is the verified way to inject one-shot seeds; always clear them afterwards
        # so later launches (including outside CodeBuddy) are unaffected.
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
            time.sleep(2)  # let the launched process inherit the launchctl environment
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
        # Direct launch: the child inherits the environment explicitly, no global state.
        env = os.environ.copy()
        env.update(launch_env)
        kwargs: dict = {"env": env, "stdin": subprocess.DEVNULL,
                        "stdout": subprocess.DEVNULL, "stderr": subprocess.DEVNULL}
        if not _IS_WIN:
            # Detach into its own session so the editor survives the MCP stdio process
            # exiting, and never shares the JSON-RPC stdout pipe.
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
    result["hint"] = "Import may take several minutes; poll unity_info() until it reports the scene."
    get_bridge().disconnect()  # force a reconnect to the new editor process
    return result


@mcp.tool()
def unity_stop(force: bool = False, dry_run: bool = False) -> dict:
    """Quit the running Unity Editor (macOS / Windows).

    macOS: sends SIGTERM (SIGKILL with force=True) to Unity.app editor processes.
    Windows: uses taskkill /PID /T (adds /F with force=True) on Unity.exe processes.
    Waits up to ~8s for a graceful shutdown and reports anything left. Also drops the
    cached GM socket so the next call reconnects cleanly.

    force:   force-kill survivors (use when the GPU is wedged and the editor no longer
             responds).
    dry_run: report what would be terminated without signalling anything.
    """
    processes = _unity_processes()
    pids = [pid for pid, _ in processes]
    if not pids:
        get_bridge().disconnect()
        return {"ok": True, "already_stopped": True, "pids": [], "platform": sys.platform}
    if dry_run:
        graceful = "taskkill /PID /T" if _IS_WIN else "SIGTERM"
        forced = "taskkill /PID /T /F" if _IS_WIN else "SIGKILL"
        return {"ok": True, "executed": False, "platform": sys.platform, "pids": pids,
                "processes": [{"pid": pid, "command": cmd[:200]} for pid, cmd in processes],
                "would_send": graceful + (f" then {forced}" if force else "")}

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

    get_bridge().disconnect()
    return {"ok": not remaining, "executed": True, "platform": sys.platform, "pids": pids,
            "stopped": [p for p in pids if p not in remaining],
            "forced": forced, "remaining": remaining, "errors": errors,
            "hint": None if not remaining else "Editor did not exit; retry with force=True."}


@mcp.tool()
def unity_activate() -> dict:
    """Bring the Unity Editor window to the front (macOS / Windows).

    IMPORTANT for GPU/RT verification: a background editor is throttled by the OS and
    barely repaints, so RT dispatch may happen only once every tens of seconds. Several
    past "no hang, fix works" conclusions were false negatives caused by exactly this.
    Call this before measuring, and periodically during long runs.
    """
    if _IS_WIN:
        pids = _unity_pids()
        if not pids:
            return {"ok": False, "error": "No Unity process to activate."}
        script = f"(New-Object -ComObject WScript.Shell).AppActivate({pids[0]})"
        r = _proc(["powershell", "-NoProfile", "-NonInteractive", "-Command", script], timeout=12)
        return {"ok": r["rc"] == 0, "platform": sys.platform, "pid": pids[0],
                "stdout": r["stdout"][:200], "error": r["stderr"][:200] or None}
    if _IS_MAC:
        # AppleScript `activate` LAUNCHES the app if it is not running, which would create
        # an editor with no -projectPath. Never do that: unity_start() is the only launcher.
        procs = _unity_processes()
        if not procs:
            return {"ok": False, "platform": sys.platform,
                    "error": "No Unity Editor is running. Activating would launch an instance "
                             "without a project; call unity_start() first."}
        project = str(_project_root())
        if not any("-projectPath" in cmd and project in cmd for _, cmd in procs):
            return {"ok": False, "platform": sys.platform,
                    "error": "A Unity Editor is running but its command line does not show "
                             f"-projectPath {project}. It may be stuck at the project picker; "
                             "stop it and call unity_start() instead of activating.",
                    "processes": [{"pid": pid, "command": cmd[:200]} for pid, cmd in procs]}
        r = _proc(["osascript", "-e", 'tell application "Unity" to activate'], timeout=12)
        return {"ok": r["rc"] == 0, "platform": sys.platform,
                "error": (r["stderr"] or r["stdout"])[:200] or None,
                "hint": None if r["rc"] == 0 else "AppleEvent timed out: editor may be wedged."}
    return {"ok": False, "error": f"Unsupported platform: {sys.platform}"}


@mcp.tool()
def unity_wait_ready(timeout_seconds: float = 180.0, interval: float = 2.0,
                     target: str = "editor") -> dict:
    """Block until Unity's GM socket answers ping (i.e. the editor finished importing).

    Unity sometimes takes several minutes to get past the project/asset import stage;
    polling here avoids reading a truncated Editor.log too early. Returns ok=True with
    mode/channel on success, ok=False with the last connection error on timeout.
    Note: this blocks this MCP server for up to timeout_seconds; keep the client timeout
    larger than that value.
    """
    started = time.monotonic()
    deadline = started + max(1.0, timeout_seconds)
    attempts = 0
    last_error = None
    while time.monotonic() < deadline:
        attempts += 1
        reply = _run("ping", timeout_ms=5000, target=target)
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
            "last_error": last_error}


@mcp.tool()
def unity_editor_log(lines: int = 80, filter: str = "", path: str | None = None) -> dict:
    """Read the tail of Unity's Editor.log (the file on disk, not the GM log ring buffer).

    lines:  max number of lines to return (1..2000).
    filter: optional case-insensitive substring, e.g. 'Caused GPU Hang', 'RT-Hang',
            'clamping skin pose'. Applied before taking the tail.
    path:   override the log path (default from UNITY_EDITOR_LOG or platform default).

    Returns the matched lines plus path/size/mtime, so a hang snapshot can be read without
    shell access. Without a filter only the last 512 KB is scanned (fast tail); with a
    filter the whole file is scanned, since startup lines (diag level, project path) can
    be far behind the tail in a long session.
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
    mcp.run()


if __name__ == "__main__":
    main()
