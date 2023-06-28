using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using CSharpCenterClient;
using Messenger;
using GameFramework;
using GameFrameworkMessage;

class DataCache
{
    private void Init(string[] args)
    {
        m_NameHandleCallback = this.OnNameHandleChanged;
        m_MsgCallback = this.OnMessage;
        m_MsgResultCallback = this.OnMessageResultCallback;
        m_CmdCallback = this.OnCommand;
        m_LogHandler = this.OnCenterLog;
        CenterClientApi.SetCenterLogHandler(m_LogHandler);
        CenterClientApi.Init("datacache", args.Length, args, m_NameHandleCallback, m_MsgCallback, m_MsgResultCallback, m_CmdCallback);

        m_Channel = new PBChannel(DataMessageEnum2Type.Query,
                      DataMessageEnum2Type.Query);
        m_Channel.DefaultServiceName = "UserSvr";
        LogSys.Init("./config/logconfig.xml");
        DataCacheConfig.Init();

        GlobalVariables.Instance.IsClient = false;

        string key = "防君子不防小人";
        byte[] xor = Encoding.UTF8.GetBytes(key);

        ResourceReadProxy.OnReadAsArray = ((string filePath) => {
            byte[] buffer = null;
            try {
                buffer = File.ReadAllBytes(filePath);
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "Exception:{0}\n{1}", e.Message, e.StackTrace);
                return null;
            }
            return buffer;
        });
        LogSystem.OnOutput += (GameLogType type, string msg) => {
            switch (type) {
                case GameLogType.Debug:
                    LogSys.Log(ServerLogType.DEBUG, msg);
                    break;
                case GameLogType.Info:
                    LogSys.Log(ServerLogType.INFO, msg);
                    break;
                case GameLogType.Warn:
                    LogSys.Log(ServerLogType.WARN, msg);
                    break;
                case GameLogType.Error:
                case GameLogType.Assert:
                    LogSys.Log(ServerLogType.ERROR, msg);
                    break;
            }
        };

        DbThreadManager.Instance.Init(DataCacheConfig.LoadThreadNum, DataCacheConfig.SaveThreadNum);
        DataOpSystem.Instance.Init(m_Channel);
        DataCacheSystem.Instance.Init();
        LogSys.Log(ServerLogType.INFO, "DataCache initialized");
    }
    private void Loop()
    {
        try {
            while (CenterClientApi.IsRun()) {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastTickTime != 0) {
                    long elapsedTickTime = curTime - m_LastTickTime;
                    if (elapsedTickTime > c_WarningTickTime) {
                        LogSys.Log(ServerLogType.MONITOR, "DataCache Network Tick:{0}", curTime - m_LastTickTime);
                    }
                }
                m_LastTickTime = curTime;

                CenterClientApi.Tick();
                Thread.Sleep(10);
                if (m_WaitQuit) {
                    if (PersistentSystem.Instance.StartLastSaveResult == PersistentSystem.SaveState.Failed) {
                        //发起存盘操作失败,重置m_WaitQuit
                        m_WaitQuit = false;
                    }
                    if (PersistentSystem.Instance.LastSaveState == PersistentSystem.SaveState.Success) {
                        LogSys.Log(ServerLogType.MONITOR, "DataCache LastSave Success. DataCache quit ...");
                        Thread.Sleep(10000);
                        CenterClientApi.Quit();
                    } else if (PersistentSystem.Instance.LastSaveState == PersistentSystem.SaveState.Failed) {
                        LogSys.Log(ServerLogType.MONITOR, "DataCache LastSave Failed. DataCache NOT quit ...");
                        PersistentSystem.Instance.LastSaveState = PersistentSystem.SaveState.Initial;
                        m_WaitQuit = false;
                    }
                }
                if (m_LastEchoTime + 5000 < curTime) {
                    m_LastEchoTime = curTime;

                    CenterClientApi.SendCommandByName("DataCache", "Echo");
                }
            }
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "DataCache.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void Release()
    {
        LogSys.Log(ServerLogType.INFO, "DataCache release");
        DataCacheSystem.Instance.Stop();
        DbThreadManager.Instance.Stop();
        CenterClientApi.Release();
        LogSys.Release();
    }
    private void OnCenterLog(string msg, int size)
    {
        LogSys.Log(ServerLogType.INFO, "{0}", msg);
    }
    private void OnNameHandleChanged(bool addOrUpdate, string name, int handle)
    {
        try {
            m_Channel.OnUpdateNameHandle(addOrUpdate, name, handle);
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnCommand(int src, int dest, string command)
    {
        const string c_QuitDataStore = "QuitDataStore";
        const string c_ReloadConfig = "ReloadConfig";
        try {
            if (0 == command.CompareTo(c_QuitDataStore)) {
                LogSys.Log(ServerLogType.MONITOR, "receive {0} command, save data and then quitting ...", command);
                if (!m_WaitQuit) {
                    DataCacheSystem.Instance.QueueAction(DataCacheSystem.Instance.DoLastSave);
                    m_WaitQuit = true;
                }
            } else if (0 == command.CompareTo(c_ReloadConfig)) {
                CenterClientApi.ReloadConfigScript();
                DataCacheConfig.Init();
                LogSys.Log(ServerLogType.WARN, "receive {0} command.", command);
            }
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnMessage(uint seq, int source_handle, int dest_handle,
        IntPtr data, int len)
    {
        try {
            byte[] bytes = new byte[len];
            Marshal.Copy(data, bytes, 0, len);
            m_Channel.Dispatch(source_handle, seq, bytes);
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnMessageResultCallback(uint seq, int src, int dest, int result)
    {

    }

    private bool m_WaitQuit = false;

    private const long c_WarningTickTime = 2000;
    private long m_LastTickTime = 0;

    private PBChannel m_Channel = null;
    private CenterClientApi.HandleNameHandleChangedCallback m_NameHandleCallback = null;
    private CenterClientApi.HandleMessageCallback m_MsgCallback = null;
    private CenterClientApi.HandleMessageResultCallback m_MsgResultCallback = null;
    private CenterClientApi.HandleCommandCallback m_CmdCallback = null;
    private CenterClientApi.CenterLogHandler m_LogHandler = null;

    private long m_LastEchoTime = 0;

    internal static void Main(string[] args)
    {
        DataCache svr = new DataCache();
        svr.Init(args);
        svr.Loop();
        svr.Release();
    }
}
