using System;
using System.Collections.Concurrent;
using Messenger;
using GameFramework;
using GameFrameworkMessage;
using GameFramework.DataCache;

internal class DataOpSystem
{
    internal void Init(PBChannel channel)
    {
        channel_ = channel;

        channel_.Register<Msg_LD_Connect>(DSConnectHandler);
        channel_.Register<Msg_LD_Load>(DSLoadHandler);
        channel_.Register<Msg_LD_Save>(DSSaveHandler);

        InitDSNodeVersion();
        InitGlobalDataVersion();
        LogSys.Log(ServerLogType.INFO, "DataOperator initialized");
    }

    private void DSConnectHandler(Msg_LD_Connect msg, PBChannel channel, int handle, uint seq)
    {
        try {
            LogSys.Log(ServerLogType.INFO, "DataStoreClient connect :{0} ", msg.ClientName);
            var reply = new Msg_DL_Connect();
            reply.Result = true;
            reply.Error = string.Empty;
            channel.Send(reply);
        } catch (Exception e) {
            var reply = new Msg_DL_Connect();
            reply.Result = false;
            reply.Error = e.Message;
            channel.Send(reply);
            LogSys.Log(ServerLogType.ERROR, "Connect failed. ClientName:{0}", msg.ClientName);
        }
    }

    private void DSLoadHandler(Msg_LD_Load msg, PBChannel channel, int handle, uint seq)
    {
        try {
            DataCacheSystem.Instance.LoadActionQueue.QueueAction<Msg_LD_Load, PBChannel, int>(DataCacheSystem.Instance.Load, msg, channel, handle);
        } catch (Exception e) {
            var errorReply = new Msg_DL_LoadResult();
            errorReply.MsgId = msg.MsgId;
            errorReply.PrimaryKeys.AddRange(msg.PrimaryKeys);
            errorReply.SerialNo = msg.SerialNo;
            errorReply.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.Exception;
            errorReply.ErrorInfo = e.Message;
            channel.Send(errorReply);
            LogSys.Log(ServerLogType.ERROR, "Load data failed. MsgId:{0}, Key:{1} Error:{2} Detail:{3}", msg.MsgId, msg.PrimaryKeys, e.Message, e.StackTrace);
        }
    }

    private void DSSaveHandler(Msg_LD_Save msg, PBChannel channel, int handle, uint seq)
    {
        var saveResult = new Msg_DL_SaveResult();
        saveResult.MsgId = msg.MsgId;
        saveResult.PrimaryKeys.AddRange(msg.PrimaryKeys);
        saveResult.SerialNo = msg.SerialNo;
        saveResult.ErrorNo = Msg_DL_SaveResult.ErrorNoEnum.Success;
        saveResult.ErrorInfo = string.Empty;
        try {
            //写入数据缓存
            //TODO:是否将byte[]解析出protobuf对象? 提前反序列化成对象的好处:1.若数据有错误,反序列化失败,可反馈给lobby;2.protobuf对象可重用?
            //TODO:解析primaryKey和foreignKey     
            DataCacheSystem.Instance.SaveActionQueue.QueueAction(DataCacheSystem.Instance.Save, msg.MsgId, msg.PrimaryKeys, msg.ForeignKeys, msg.Data, msg.SerialNo);
        } catch (Exception e) {
            saveResult.ErrorNo = Msg_DL_SaveResult.ErrorNoEnum.PostError;
            saveResult.ErrorInfo = e.Message;
            LogSys.Log(ServerLogType.ERROR, "Save data ERROR: MsgId:{0}, Key:{1}, Error:{2}, Detail:{3}", msg.MsgId, msg.PrimaryKeys, e.Message, e.StackTrace);
        }
        channel.Send(saveResult);
    }

    internal void Release()
    {
        channel_ = null;
        LogSys.Log(ServerLogType.INFO, "DataOperator disposed");
    }

    internal int GlobalDBDataVersion
    {
        get { return m_GlobalDataVersion; }
    }

    internal void InitDSNodeVersion()
    {
        m_DBVersion = DataProcedureImplement.GetDSNodeVersion().Trim();
        if (m_DBVersion.Equals(DataCacheVersion.Version)) {
            LogSys.Log(ServerLogType.INFO, "Init DSNodeVersion Success:{0}", m_DBVersion);
        } else {
            string errorMsg = string.Format("DSNodeVersion:{0} ,DBVersion:{1} do not match! Fatel ERROR!!!", DataCacheVersion.Version, m_DBVersion);
            throw new Exception(errorMsg);
        }
    }

    internal void InitGlobalDataVersion()
    {
        m_GlobalDataVersion = DataProcedureImplement.GetGlobalDataVersion();
        if (m_GlobalDataVersion >= 0) {
            LogSys.Log(ServerLogType.INFO, "Init GlobalDataVersion Success. GlobalDataVersion = {0}", m_GlobalDataVersion);
        } else {
            string errorMsg = string.Format("Init GlobalDataVersion ERROR!!! GlobalDataVersion:{0}", m_GlobalDataVersion);
            throw new Exception(errorMsg);
        }
    }

    internal void SaveGlobalDataVersion()
    {
        DataProcedureImplement.SetGlobalDataVersion(m_GlobalDataVersion);
    }

    internal void IncreaseGlobalDataVersion()
    {
        m_GlobalDataVersion = m_GlobalDataVersion + 1;
        this.SaveGlobalDataVersion();
        LogSys.Log(ServerLogType.MONITOR, ConsoleColor.Yellow, "IncreaseGlobalDataVersion: {0}", m_GlobalDataVersion);
    }

    private PBChannel channel_;
    private string m_DBVersion = string.Empty;
    private int m_GlobalDataVersion = 0;

    internal static DataOpSystem Instance
    {
        get { return s_Instance; }
    }
    private static DataOpSystem s_Instance = new DataOpSystem();
}