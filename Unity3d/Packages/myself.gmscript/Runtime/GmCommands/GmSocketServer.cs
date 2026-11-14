using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using StoryScript;

// GM channel for AI / external tools: one JSON request per line, one reply per request.
//   {"id":1,"cmd":"exec","code":"getposition(findobj(\"Main Camera\"))"}
//   <- {"id":1,"ok":true,"result":"...","elapsedMs":1,"logs":[...]}
// Plain text lines are still accepted (executed as DebugConsole commands).
public sealed class GmSocketServer
{
    public GmSocketServer(int port)
    {
        m_Port = port;
    }

    public int Port
    {
        get { return m_Port; }
    }
    public bool IsRunning
    {
        get { return m_Running; }
    }

    public bool Start()
    {
        if (m_Running) {
            return true;
        }
        try {
            var listener = new TcpListener(IPAddress.Loopback, m_Port);
            listener.Start();
            m_Listener = listener;
        }
        catch (Exception ex) {
            m_Listener = null;
            m_Running = false;
            LogSystem.Error("GmSocketServer can't listen on 127.0.0.1:{0}, exception:{1}", m_Port, ex.Message);
            return false;
        }
        m_Running = true;
        m_ListenThread = new Thread(ListenLoop);
        m_ListenThread.IsBackground = true;
        m_ListenThread.Name = "GmSocketServer:" + m_Port;
        m_ListenThread.Start();
        LogSystem.Warn("GmSocketServer started on 127.0.0.1:{0}", m_Port);
        return true;
    }

    public void Stop()
    {
        if (!m_Running) {
            return;
        }
        m_Running = false;
        try {
            if (null != m_Listener) {
                m_Listener.Stop();
            }
        }
        catch {
        }
        m_Listener = null;

        lock (m_ClientLock) {
            foreach (var client in m_Clients) {
                try { client.Close(); } catch { }
            }
            m_Clients.Clear();
        }

        var thread = m_ListenThread;
        m_ListenThread = null;
        if (null != thread) {
            try {
                if (thread != Thread.CurrentThread) {
                    thread.Join(1000);
                }
            }
            catch {
            }
        }
        LogSystem.Warn("GmSocketServer stopped. port:{0}", m_Port);
    }

    private void ListenLoop()
    {
        while (m_Running) {
            try {
                var client = m_Listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(_ => HandleClient(client));
            }
            catch (SocketException) {
                break;
            }
            catch (ObjectDisposedException) {
                break;
            }
            catch (Exception ex) {
                if (m_Running) {
                    LogSystem.Error("GmSocketServer accept error: {0}", ex.Message);
                }
            }
        }
    }

    private void HandleClient(TcpClient client)
    {
        lock (m_ClientLock) {
            m_Clients.Add(client);
        }
        try {
            client.NoDelay = true;
            using (var stream = client.GetStream()) {
                var readBuffer = new byte[4096];
                // Accumulate bytes then split on '\n': 0x0A never appears inside a UTF-8 multi-byte sequence.
                var pending = new List<byte>(4096);
                while (m_Running) {
                    int read;
                    try {
                        read = stream.Read(readBuffer, 0, readBuffer.Length);
                    }
                    catch (IOException) {
                        break;
                    }
                    catch (ObjectDisposedException) {
                        break;
                    }
                    if (read <= 0) {
                        // EOF: treat leftover data as one final command, for legacy clients that shutdown without a newline.
                        if (pending.Count > 0) {
                            string tail = Encoding.UTF8.GetString(pending.ToArray());
                            pending.Clear();
                            if (!SendResponse(stream, tail)) {
                                break;
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
        }
        catch (IOException) {
            // client disconnected
        }
        catch (ObjectDisposedException) {
        }
        catch (Exception ex) {
            if (m_Running) {
                LogSystem.Warn("GmSocketServer client error: {0}", ex.Message);
            }
        }
        finally {
            lock (m_ClientLock) {
                m_Clients.Remove(client);
            }
            try { client.Close(); } catch { }
        }
    }

    private bool SendResponse(Stream stream, string requestLine)
    {
        if (string.IsNullOrEmpty(requestLine) || requestLine.Trim().Length == 0) {
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
    private static bool WriteRaw(Stream stream, string response)
    {
        try {
            var bytes = Encoding.UTF8.GetBytes(response + "\n");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            return true;
        }
        catch (IOException) {
            return false;
        }
        catch (ObjectDisposedException) {
            return false;
        }
        catch (Exception) {
            return false;
        }
    }

    private const int c_MaxLineBytes = 4 * 1024 * 1024;
    private readonly int m_Port;
    private TcpListener m_Listener;
    private Thread m_ListenThread;
    private volatile bool m_Running;
    private readonly List<TcpClient> m_Clients = new List<TcpClient>();
    private readonly object m_ClientLock = new object();
}
