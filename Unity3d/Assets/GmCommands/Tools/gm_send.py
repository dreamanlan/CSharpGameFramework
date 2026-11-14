#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
GM Command Sender.

Protocol: one JSON request line -> one JSON reply line (Unity side: GmSocketServer.cs)
    {"id":1,"cmd":"console","code":"sys","timeout":30000}
    <- {"id":1,"ok":true,"result":"...","elapsedMs":2,"logs":[{"i":1,"lv":"W","m":"..."}]}

Ports:
    39527  runtime (Play mode / Development Build)
           HarmonyOS/Android devices need port forwarding first:
             hdc fport tcp:39527 tcp:39527
    39528  editor (menu Tools/GM Socket/Start Editor Listener)

Usage:
    # 1. (device) set up port forwarding first
    #    hdc fport tcp:39527 tcp:39527

    # 2. send one DebugConsole command (default port 39527)
    #    python3 gm_send.py "sys"
    #    python3 gm_send.py --setup "sys"          # set up port forwarding first

    # 3. send one GM DSL snippet and get the return value
    #    python3 gm_send.py --mode exec "screenwidth()+'x'+screenheight()"

    # 4. run editor DSL on port 39528 (resource / editor APIs)
    #    python3 gm_send.py --editor --mode edit "check_pipeline_assets()"

    # 5. interactive mode (type commands, result echoed per command)
    #    python3 gm_send.py -i
"""

import argparse
import json
import socket
import subprocess
import sys


DEFAULT_HOST = "127.0.0.1"
DEFAULT_PORT = 39527        # runtime
EDITOR_PORT = 39528         # editor
CONNECT_TIMEOUT = 5
DEFAULT_TIMEOUT = 30.0      # seconds; must be larger than the Unity-side request timeout
MODES = ("console", "exec", "edit")


def build_payload(command, mode, next_id, timeout_ms):
    payload = {"id": next_id, "cmd": mode, "code": command, "timeout": timeout_ms}
    return payload


def request(host, port, payload, timeout=DEFAULT_TIMEOUT):
    """Send one JSON line, read one JSON line. Raises on failure."""
    sock = socket.create_connection((host, port), timeout=CONNECT_TIMEOUT)
    try:
        data = (json.dumps(payload, ensure_ascii=False) + "\n").encode("utf-8")
        sock.sendall(data)
        sock.settimeout(timeout)
        buf = bytearray()
        while b"\n" not in buf:
            chunk = sock.recv(65536)
            if not chunk:
                break
            buf += chunk
        line = bytes(buf).split(b"\n", 1)[0].decode("utf-8", "replace").strip()
        if not line:
            raise RuntimeError("Unity returned no reply (connection closed: compiling or domain reload?)")
        return json.loads(line)
    finally:
        try:
            sock.close()
        except OSError:
            pass


def format_reply(reply):
    """Turn a reply into human readable multi-line text."""
    lines = []
    if reply.get("ok"):
        result = reply.get("result", "")
        if isinstance(result, (dict, list)):
            result = json.dumps(result, ensure_ascii=False)
        lines.append(f"[OK] {result}")
    else:
        lines.append(f"[FAIL] {reply.get('error', 'unknown error')}")
    elapsed = reply.get("elapsedMs")
    if elapsed is not None:
        lines.append(f"     ({elapsed}ms)")
    for entry in reply.get("logs") or []:
        level = entry.get("lv", "I")
        message = str(entry.get("m", ""))
        first, *rest = message.splitlines() or [""]
        lines.append(f"  [{level}] {first}")
        lines.extend(f"       {extra}" for extra in rest)
    if "next" in reply:
        lines.append(f"     next log cursor: {reply['next']}")
    return "\n".join(lines)


class Sender(object):
    def __init__(self, host, port, mode, timeout, as_json):
        self.host = host
        self.port = port
        self.mode = mode
        self.timeout = timeout
        self.as_json = as_json
        self.seq = 0

    def send(self, command, quiet=False):
        self.seq += 1
        timeout_ms = max(2000, int(self.timeout * 1000) - 5000)
        payload = build_payload(command, self.mode, self.seq, timeout_ms)
        try:
            reply = request(self.host, self.port, payload, self.timeout)
        except ConnectionRefusedError:
            if not quiet:
                print(f"[ERROR] Connection refused: {self.host}:{self.port}")
                print(f"        runtime: make sure the game is running and do: hdc fport tcp:{self.port} tcp:{self.port}")
                print(f"        editor: menu Tools/GM Socket/Start Editor Listener (port 39528)")
            return False
        except socket.timeout:
            if not quiet:
                print(f"[ERROR] Waited {self.timeout:.0f}s with no reply (Unity main thread blocked?)")
            return False
        except Exception as exc:
            if not quiet:
                print(f"[ERROR] {exc}")
            return False

        if self.as_json:
            print(json.dumps(reply, ensure_ascii=False))
        elif not quiet:
            print(f"--> [{self.mode}] {command}")
            print(format_reply(reply))

        ok = bool(reply.get("ok"))
        if not ok and quiet:
            print(format_reply(reply))
        return ok


def setup_forward(port):
    """Setup hdc port forwarding."""
    try:
        cmd = f"hdc fport tcp:{port} tcp:{port}"
        print(f"[INFO] Running: {cmd}")
        result = subprocess.run(cmd, shell=True, capture_output=True, text=True, timeout=10)
        if result.returncode == 0:
            print(f"[OK] Port forwarding established: tcp:{port} -> tcp:{port}")
            return True
        else:
            print(f"[ERROR] hdc forward failed: {result.stderr.strip()}")
            return False
    except FileNotFoundError:
        print("[ERROR] hdc not found. Make sure it's in your PATH.")
        return False
    except subprocess.TimeoutExpired:
        print("[ERROR] hdc forward timed out.")
        return False


def interactive_mode(sender):
    """Interactive mode for sending commands."""
    print(f"GM Command Sender - Interactive Mode")
    print(f"Target {sender.host}:{sender.port}, mode={sender.mode}")
    print(f"Type commands to send. 'quit' or 'exit' to leave.\n")
    while True:
        try:
            cmd = input("gm> ").strip()
        except (EOFError, KeyboardInterrupt):
            print("\nBye.")
            break
        if not cmd:
            continue
        if cmd.lower() in ("quit", "exit"):
            print("Bye.")
            break
        sender.send(cmd)


def main():
    parser = argparse.ArgumentParser(description="GM Command Sender (JSON line protocol)")
    parser.add_argument("command", nargs="?", help="command or dsl to send")
    parser.add_argument("-H", "--host", default=DEFAULT_HOST, help=f"Target host (default: {DEFAULT_HOST})")
    parser.add_argument("-p", "--port", type=int, default=DEFAULT_PORT,
                        help=f"Target port (default: {DEFAULT_PORT} runtime, {EDITOR_PORT} editor)")
    parser.add_argument("--editor", action="store_true", help=f"shortcut: use editor port {EDITOR_PORT}")
    parser.add_argument("-m", "--mode", choices=MODES, default="console",
                        help="console=DebugConsole command, exec=GM DSL, edit=editor DSL (default: console)")
    parser.add_argument("-i", "--interactive", action="store_true", help="Interactive mode")
    parser.add_argument("--setup", action="store_true", help="Auto setup hdc port forwarding before sending")
    parser.add_argument("-t", "--timeout", type=float, default=DEFAULT_TIMEOUT,
                        help=f"seconds to wait for reply (default: {DEFAULT_TIMEOUT})")
    parser.add_argument("--json", action="store_true", help="print raw JSON reply")
    args = parser.parse_args()

    port = EDITOR_PORT if args.editor else args.port

    if args.setup:
        if not setup_forward(port):
            sys.exit(1)

    sender = Sender(args.host, port, args.mode, args.timeout, args.json)

    if args.interactive:
        interactive_mode(sender)
    elif args.command:
        if not sender.send(args.command):
            sys.exit(1)
    else:
        parser.print_help()
        sys.exit(1)


if __name__ == "__main__":
    main()
