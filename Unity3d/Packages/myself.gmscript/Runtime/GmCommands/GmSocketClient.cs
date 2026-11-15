using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using StoryScript;

// GM channel client: connects out to the MCP socket server instead of listening.
// One JSON request per line is dispatched to the main thread by GmCommandDispatcher,
// then one reply line is written back. The editor channel (edit mode) and the runtime
// channel (Play mode inside the editor) each own one instance; the standalone player
// still uses GmSocketServer (the app opens its own listener).
public sealed class GmSocketClient
{
    public const int c_DefaultPort = 39528;
    private const int c_ReconnectIntervalMs = 10000;
    private const int c_ConnectTimeoutMs = 3000;
    private const int c_MaxLineBytes = 4 * 1024 * 1024;

    public GmSocketClient(string channel)
    {
        m_Channel = channel;
        m_Host = ResolveHost();
        m_Port = ResolvePort();
        // Unity APIs are main-thread only; snapshot the immutable hello fields here.
        m_HelloFields = BuildHelloFields(channel);
        m_IsEditor = Application.isEditor;
        GmCommandDispatcher.IsPlaying = Application.isPlaying;
    }

    public string Channel
    {
        get { return m_Channel; }
    }
    public string Endpoint
    {
        get { return m_Host + ":" + m_Port; }
    }
    public bool IsConnected
    {
        get { return m_Connected; }
    }

    public void Start()
    {
        lock (m_Lock) {
            if (null != m_Thread) {
                return;
            }
            m_Running = true;
            m_StopEvent.Reset();
            m_Thread = new Thread(RunLoop);
            m_Thread.IsBackground = true;
            m_Thread.Name = "GmSocketClient:" + m_Channel;
            m_Thread.Start();
        }
    }

    public void Stop()
    {
        Thread thread;
        lock (m_Lock) {
            if (null == m_Thread) {
                return;
            }
            m_Running = false;
            m_StopEvent.Set();
            thread = m_Thread;
            m_Thread = null;
            CloseClient();
        }
        if (thread != Thread.CurrentThread) {
            try {
                if (!thread.Join(c_ConnectTimeoutMs + 500)) {
                    LogSystem.Warn("GmSocketClient({0}, {1}) thread did not exit within {2}ms.",
                        m_Channel, Endpoint, c_ConnectTimeoutMs + 500);
                }
            }
            catch { }
        }
    }

    private void RunLoop()
    {
        while (m_Running) {
            TcpClient client = null;
            try {
                client = new TcpClient();
                var result = client.BeginConnect(m_Host, m_Port, null, null);
                if (!result.AsyncWaitHandle.WaitOne(c_ConnectTimeoutMs)) {
                    throw new TimeoutException("connect timed out");
                }
                client.EndConnect(result);
                client.NoDelay = true;
                lock (m_Lock) {
                    if (!m_Running) {
                        break;
                    }
                    m_Client = client;
                    m_Connected = true;
                }
                using (var stream = client.GetStream()) {
                    // Assembled on every connect; only main-thread snapshots are read here.
                    WriteRaw(stream, BuildHello());
                    ReadLoop(stream);
                }
                if (m_Running) {
                    LogSystem.Error("GmSocketClient({0}, {1}) disconnected, retry in 10s.", m_Channel, Endpoint);
                }
            }
            catch (Exception ex) {
                if (m_Running) {
                    LogSystem.Error("GmSocketClient({0}, {1}) cannot connect: " + ex.Message + ", retry in 10s.", m_Channel, Endpoint);
                }
            }
            finally {
                lock (m_Lock) {
                    m_Connected = false;
                    m_Client = null;
                }
                if (null != client) {
                    try { client.Close(); } catch { }
                }
            }
            if (m_Running) {
                m_StopEvent.WaitOne(c_ReconnectIntervalMs);
            }
        }
    }

    private void ReadLoop(Stream stream)
    {
        var readBuffer = new byte[4096];
        // Accumulate bytes then split on '\n': 0x0A never appears inside a UTF-8 multi-byte sequence.
        var pending = new List<byte>(4096);
        while (m_Running) {
            int read = stream.Read(readBuffer, 0, readBuffer.Length);
            if (read <= 0) {
                if (pending.Count > 0) {
                    string tail = Encoding.UTF8.GetString(pending.ToArray());
                    pending.Clear();
                    if (!SendResponse(stream, tail)) {
                        return;
                    }
                }
                break;
            }
            for (int i = 0; i < read; ++i) {
                byte b = readBuffer[i];
                if (b == (byte)'\n') {
                    if (pending.Count > 0) {
                        string line = Encoding.UTF8.GetString(pending.ToArray());
                        pending.Clear();
                        if (!SendResponse(stream, line)) {
                            return;
                        }
                    }
                }
                else {
                    pending.Add(b);
                    if (pending.Count > c_MaxLineBytes) {
                        pending.Clear();
                        WriteRaw(stream, "{\"ok\":false,\"error\":\"line too long\"}");
                    }
                }
            }
        }
    }

    private bool SendResponse(Stream stream, string requestLine)
    {
        if (string.IsNullOrEmpty(requestLine) || requestLine.Trim().Length == 0) {
            return true;
        }
        // Control frames carry "type" instead of "cmd" and must not get a reply.
        if (IsControlFrame(requestLine)) {
            return true;
        }
        string response;
        try {
            response = GmCommandDispatcher.HandleLine(requestLine);
        }
        catch (Exception ex) {
            response = "{\"ok\":false,\"error\":\"" + GmJson.Escape("server error: " + ex.Message) + "\"}";
        }
        return WriteRaw(stream, response);
    }

    private static bool IsControlFrame(string line)
    {
        try {
            var dict = GmJson.ParseObject(line);
            return !string.IsNullOrEmpty(GmJson.GetString(dict, "type", null));
        }
        catch (Exception) {
            return false;
        }
    }

    private static bool WriteRaw(Stream stream, string response)
    {
        try {
            var bytes = Encoding.UTF8.GetBytes(response + "\n");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    private void CloseClient()
    {
        var client = m_Client;
        m_Client = null;
        if (null != client) {
            try { client.Close(); } catch { }
        }
    }

    // All Unity API reads happen on the main thread while constructing the client.
    private static Dictionary<string, object> BuildHelloFields(string channel)
    {
        var d = new Dictionary<string, object>();
        d["cmd"] = "hello";
        d["channel"] = channel;
        d["editorPath"] = ResolveEditorPath();
        d["projectPath"] = ResolveProjectPath();
        d["pid"] = ResolvePid();
        d["unityVersion"] = Application.unityVersion;
        d["platform"] = Application.platform.ToString();
        d["productName"] = Application.productName;
        return d;
    }

    // Assembled on every connect (not cached); IsPlaying is refreshed by the main-thread Pump.
    private string BuildHello()
    {
        var d = new Dictionary<string, object>(m_HelloFields);
        d["mode"] = m_IsEditor ? (GmCommandDispatcher.IsPlaying ? "editor-play" : "editor") : "player";
        return GmJson.Serialize(d);
    }

    private static string ResolveEditorPath()
    {
        try {
            using (var p = Process.GetCurrentProcess()) {
                return p.MainModule.FileName ?? string.Empty;
            }
        }
        catch (Exception ex) {
            LogSystem.Error("GmSocketClient cannot resolve editor path: {0}", ex.Message);
            return string.Empty;
        }
    }

    private static string ResolveProjectPath()
    {
        try {
            return Path.GetDirectoryName(Application.dataPath) ?? string.Empty;
        }
        catch (Exception ex) {
            LogSystem.Error("GmSocketClient cannot resolve project path: {0}", ex.Message);
            return string.Empty;
        }
    }

    private static int ResolvePid()
    {
        try {
            using (var p = Process.GetCurrentProcess()) {
                return p.Id;
            }
        }
        catch {
            return 0;
        }
    }

    private static string ResolveHost()
    {
        var host = Environment.GetEnvironmentVariable("UC_GM_MCP_HOST");
        return string.IsNullOrEmpty(host) ? "127.0.0.1" : host;
    }

    private static int ResolvePort()
    {
        var port = Environment.GetEnvironmentVariable("UC_GM_MCP_PORT");
        int v;
        if (!string.IsNullOrEmpty(port) && int.TryParse(port, out v) && v > 0 && v < 65536) {
            return v;
        }
        return c_DefaultPort;
    }

    private readonly string m_Channel;
    private readonly string m_Host;
    private readonly int m_Port;
    private readonly Dictionary<string, object> m_HelloFields;
    private readonly bool m_IsEditor;
    private readonly object m_Lock = new object();
    private readonly ManualResetEvent m_StopEvent = new ManualResetEvent(false);
    private TcpClient m_Client;
    private Thread m_Thread;
    private volatile bool m_Running;
    private volatile bool m_Connected;
}
