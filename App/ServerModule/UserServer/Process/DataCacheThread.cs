using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Messenger;
using GameFramework;
using GameFrameworkMessage;
using GameFrameworkData;

namespace GameFramework
{
    /// <summary>
    /// 数据访问层线程。此类主要用于数据存储相关的操作
    /// </summary>  
    internal sealed class DataCacheThread : MyServerTaskDispatcher
    {
        private class LoadRequestInfo : IServerConcurrentPoolAllocatedObject<LoadRequestInfo>
        {
            internal long m_LastSendTime;
            internal Msg_LD_Load m_Request;
            internal MyAction<Msg_DL_LoadResult> m_Callback;

            public void InitPool(ServerConcurrentObjectPool<LoadRequestInfo> pool)
            {
                m_Pool = pool;
            }
            public LoadRequestInfo Downcast()
            {
                return this;
            }

            ServerConcurrentObjectPool<LoadRequestInfo> m_Pool = null;
        }
        private class SaveRequestInfo : IServerConcurrentPoolAllocatedObject<SaveRequestInfo>
        {
            internal long m_LastSendTime;
            internal Msg_LD_Save m_Request;
            internal MyAction<Msg_DL_SaveResult> m_Callback;

            public void InitPool(ServerConcurrentObjectPool<SaveRequestInfo> pool)
            {
                m_Pool = pool;
            }
            public SaveRequestInfo Downcast()
            {
                return this;
            }

            ServerConcurrentObjectPool<SaveRequestInfo> m_Pool = null;
        }
        private enum ConnectStatus
        {
            None = 0,
            Connecting,
            Connected,
            Disconnect,
        }
        //全局数据加载状态
        internal enum DataInitStatus
        {
            Unload = 0,   //未加载
            Loading,      //从DS加载中
            Done          //完成加载
        }
        internal DataCacheThread()
            : base(3)
        {
            m_Thread = new MyServerThread();
            m_Thread.TickSleepTime = 10;
            m_Thread.ActionNumPerTick = 4096;
            m_Thread.OnStartEvent += this.OnStart;
            m_Thread.OnTickEvent += this.OnTick;
        }
        internal void Init(PBChannel channel)
        {
            m_DataStoreChannel = channel;

            channel.Register<Msg_DL_Connect>(OnConnectDataStore);
            channel.Register<Msg_DL_LoadResult>(OnLoadReply);
            channel.Register<Msg_DL_SaveResult>(OnSaveReply);
        }
        internal void Start()
        {
            m_Thread.Start();
        }
        internal void Stop()
        {
            StopTaskThreads();
            m_Thread.Stop();
        }
        internal bool DataStoreAvailable
        {
            get { return m_DataStoreAvailable; }
        }

        internal static char[] DSStringSeparator = new char[] { '|' };
        internal static string DSDateTimeStringFormat = "yyyyMMddHHmmss";
        internal const int UltimateSaveCount = -1;         //最后一次存储操作的存储计数 

        internal int CalcLoadRequestCount()
        {
            int ct = 0;
            foreach (var pair in m_LoadRequests) {
                ct += pair.Value.Count;
            }
            return ct;
        }
        internal int CalcSaveRequestCount()
        {
            int ct = 0;
            foreach (var pair in m_SaveRequestQueues) {
                foreach (var recPair in pair.Value) {
                    ct += recPair.Value.Count;
                }
            }
            return ct;
        }
        internal void RequestLoad(Msg_LD_Load msg, MyAction<Msg_DL_LoadResult> callback)
        {
            msg.SerialNo = GenNextSerialNo();
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, LoadRequestInfo> dict = null;
            if (!m_LoadRequests.TryGetValue(msg.MsgId, out dict)) {
                dict = m_LoadRequests.AddOrUpdate(msg.MsgId, new ConcurrentDictionary<KeyString, LoadRequestInfo>(), (k, v) => v);
            }
            LoadRequestInfo info;
            if (!dict.TryGetValue(key, out info)) {
                info = dict.AddOrUpdate(key, m_LoadRequestPool.Alloc(), (k, v) => v);
            }
            info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
            info.m_Request = msg;
            info.m_Callback = callback;
            m_DataStoreChannel.Send(msg);
        }
        internal void RequestSave(Msg_LD_Save msg, MyAction<Msg_DL_SaveResult> callback = null)
        {
            lock (m_SaveRequestQueuesLock) {
                msg.SerialNo = GenNextSerialNo();
                KeyString key = KeyString.Wrap(msg.PrimaryKeys);
                ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict;
                if (!m_SaveRequestQueues.TryGetValue(msg.MsgId, out dict)) {
                    dict = m_SaveRequestQueues.AddOrUpdate(msg.MsgId, new ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>(), (k, v) => v);
                }
                ConcurrentQueue<SaveRequestInfo> queue;
                if (!dict.TryGetValue(key, out queue)) {
                    queue = dict.AddOrUpdate(key, new ConcurrentQueue<SaveRequestInfo>(), (k, v) => v);
                }
                SaveRequestInfo info = m_SaveRequestPool.Alloc();
                info.m_Request = msg;
                info.m_Callback = callback;
                if (queue.Count == 0) {
                    //当前队列为空时直接发送消息
                    info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
                    m_DataStoreChannel.Send(msg);
                }
                queue.Enqueue(info);
            }
        }
        //与DataCache建立连接
        private void ConnectDataStore()
        {
            Msg_LD_Connect connectMsg = new Msg_LD_Connect();
            connectMsg.ClientName = "UserSvr";
            if (m_DataStoreChannel.Send(connectMsg)) {
                m_CurrentStatus = ConnectStatus.Connecting;
            }
        }
        private void OnConnectDataStore(Msg_DL_Connect msg, PBChannel channel, int src, uint session)
        {
            LoadGlobalData();
        }
        private void LoadGlobalData()
        {
            InitGuidData();
            InitNicknameData();
        }
        private void OnLoadReply(Msg_DL_LoadResult msg, PBChannel channel, int src, uint session)
        {
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, LoadRequestInfo> dict;
            if (m_LoadRequests.TryGetValue(msg.MsgId, out dict)) {
                LoadRequestInfo info;
                if (dict.TryGetValue(key, out info)) {
                    if (info.m_Request.SerialNo == msg.SerialNo) {
                        if (null != info.m_Callback) {
                            info.m_Callback(msg);
                        }
                        LoadRequestInfo delInfo;
                        if (dict.TryRemove(key, out delInfo)) {
                            m_LoadRequestPool.Recycle(delInfo);
                        }
                    }
                }
            }
        }
        private void OnSaveReply(Msg_DL_SaveResult msg, PBChannel channel, int src, uint session)
        {
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict;
            if (m_SaveRequestQueues.TryGetValue(msg.MsgId, out dict)) {
                ConcurrentQueue<SaveRequestInfo> reqQueue;
                if (dict.TryGetValue(key, out reqQueue)) {
                    SaveRequestInfo info;
                    if (reqQueue.TryPeek(out info)) {
                        if (info.m_Request.SerialNo == msg.SerialNo) {
                            if (null != info.m_Callback) {
                                info.m_Callback(msg);
                            }
                            SaveRequestInfo delInfo;
                            if (reqQueue.TryDequeue(out delInfo)) {
                                m_SaveRequestPool.Recycle(delInfo);
                            }
                            //发送队列中的下一个消息
                            if (reqQueue.TryPeek(out info)) {
                                info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
                                m_DataStoreChannel.Send(info.m_Request);
                            }
                        }
                    }
                }
            }
        }

        private void InitGuidData()
        {
            if (m_GuidInitStatus == DataInitStatus.Unload) {
                if (UserServerConfig.DataStoreAvailable == true) {
                    m_GuidInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableGuid;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableGuid;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<GuidInfo> guidList = new List<GuidInfo>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableGuid dataGuid = _msg as TableGuid;
                                    if (null != dataGuid) {
                                        GuidInfo guidinfo = new GuidInfo();
                                        guidinfo.GuidType = dataGuid.GuidType;
                                        guidinfo.NextGuid = (long)dataGuid.GuidValue;
                                        guidList.Add(guidinfo);
                                    }
                                }
                            }
                            UserServer.Instance.GlobalProcessThread.InitGuidData(guidList);
                            m_GuidInitStatus = DataInitStatus.Done;
                            LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:DSG_Guid");
                        } else {
                            m_GuidInitStatus = DataInitStatus.Unload;
                            LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "DSG_Guid");
                        }
                    }
                   ));
                } else {
                    m_GuidInitStatus = DataInitStatus.Done;
                    LogSys.Log(LOG_TYPE.INFO, "init guid done!");
                }
            }
        }
        private void InitNicknameData()
        {
            if (m_NicknameInitStatus == DataInitStatus.Unload && m_GuidInitStatus == DataInitStatus.Done) {
                if (m_DataStoreAvailable) {
                    m_NicknameInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableNicknameInfo;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableNicknameInfo;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<TableNicknameInfo> nickList = new List<TableNicknameInfo>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableNicknameInfo dataNickname = _msg as TableNicknameInfo;
                                    if (null != dataNickname) {
                                        nickList.Add(dataNickname);
                                    }
                                }
                            }
                            UserServer.Instance.UserProcessScheduler.InitNicknameData(nickList);
                            m_NicknameInitStatus = DataInitStatus.Done;
                            LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableNickname");
                        } else {
                            m_NicknameInitStatus = DataInitStatus.Unload;
                            LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "TableNickname");
                        }
                    }
                   ));
                } else {
                    List<TableNicknameInfo> nicknameList = new List<TableNicknameInfo>();
                    UserServer.Instance.UserProcessScheduler.InitNicknameData(nicknameList);
                    m_NicknameInitStatus = DataInitStatus.Done;
                    LogSys.Log(LOG_TYPE.INFO, "load NickName done!");
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        // 供外部直接调用的方法，实际执行线程是调用线程。
        //--------------------------------------------------------------------------------------------------------------------------
        internal void DSPSaveActivateAccount(AccountInfo ai, string activationCode)
        {
            try {
                TableAccount dataAccount = new TableAccount();
                dataAccount.AccountId = ai.AccountId;
                //dataAccount.IsBanned = ai.IsBanned;
                //dataAccount.UserGuid = ai.UserGuid;
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableAccount;
                msg.PrimaryKeys.Add(dataAccount.AccountId);
                msg.Data = DbDataSerializer.Encode(dataAccount);
                DispatchAction(DSSaveInternal, msg);
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR. Msg:ActivateAccount, Key:{0}, Error:{1},\nStacktrace:{2}", ai.AccountId, e.Message, e.StackTrace);
            }
        }
        internal void DSPSaveCreateUser(AccountInfo ai, string nickname, ulong userGuid)
        {
            try {
                TableAccount dataAccount = new TableAccount();
                dataAccount.AccountId = ai.AccountId;
                //dataAccount.IsBanned = ai.IsBanned;
                //dataAccount.UserGuid = ai.UserGuid;
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableAccount;
                msg.PrimaryKeys.Add(dataAccount.AccountId);
                msg.Data = DbDataSerializer.Encode(dataAccount);
                DispatchAction(DSSaveInternal, msg);

                TableNicknameInfo dataNickname = new TableNicknameInfo();
                dataNickname.Nickname = nickname;
                dataNickname.UserGuid = userGuid;
                Msg_LD_Save msgNickname = new Msg_LD_Save();
                msgNickname.MsgId = (int)DataEnum.TableNicknameInfo;
                msgNickname.PrimaryKeys.Add(dataNickname.Nickname);
                msgNickname.Data = DbDataSerializer.Encode(dataNickname);
                DispatchAction(DSSaveInternal, msgNickname);
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR. Msg:DSP_CreateUser, Key:{0}, Error:{1},\nStacktrace:{2}", ai.AccountId, e.Message, e.StackTrace);
            }
        }
        internal void DSPSaveUser(UserInfo ui, int saveCount)
        {
            try {
                ulong userGuid = ui.Guid;
                string key = userGuid.ToString();
                if (ui.Modified) {
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableUserInfo;
                    msg.PrimaryKeys.AddRange(ui.PrimaryKeys);
                    msg.ForeignKeys.AddRange(ui.ForeignKeys);
                    msg.Data = DbDataSerializer.Encode(ui.ToProto());
                    DispatchAction(DSSaveInternal, msg);
                    ui.Modified = false;
                }
                ui.CurrentUserSaveCount = saveCount;
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR. Msg:DSP_User, Key:{0}, SaveCount:{1}, Error:{2},\nStacktrace:{3}", ui.Guid, saveCount, e.Message, e.StackTrace);
            }
        }
        internal void DSSaveGuid(string guidType, ulong guidValue)
        {
            try {
                TableGuid dataGuid = new TableGuid();
                dataGuid.GuidType = guidType;
                dataGuid.GuidValue = guidValue;
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableGuid;
                msg.PrimaryKeys.Add(dataGuid.GuidType);
                msg.Data = DbDataSerializer.Encode(dataGuid);
                DispatchAction(DSSaveInternal, msg);
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void DSGSaveGuid(List<GuidInfo> guidList, int saveCount)
        {
            try {
                foreach (var guidinfo in guidList) {
                    TableGuid dataGuid = new TableGuid();
                    dataGuid.GuidType = guidinfo.GuidType;
                    dataGuid.GuidValue = (ulong)guidinfo.NextGuid;
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableGuid;
                    msg.PrimaryKeys.Add(dataGuid.GuidType);
                    msg.Data = DbDataSerializer.Encode(dataGuid);
                    DispatchAction(DSSaveInternal, msg);
                }
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal int DSGSaveNickname(TableNicknameInfo nick)
        {
            try {
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableNicknameInfo;
                msg.PrimaryKeys.Add(nick.Nickname);
                msg.ForeignKeys.Clear();
                msg.Data = DbDataSerializer.Encode(nick);
                RequestSave(msg);
                return 0;
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
                return 0;
            }
        }
        internal void DSDSaveBanAccount(string accountId, bool isBan)
        {
            try {
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void DSDSaveForbidChat(ulong userGuid, bool isForbid)
        {
            try {
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void DSSaveGmAccount(string gmAccount, string passwordMd5)
        {
            try {
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktracinite:{1}", e.Message, e.StackTrace);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //供内部通过DispatchAction(调用的方法，实际执行线程是DataCacheThread。
        //--------------------------------------------------------------------------------------------------------------------------
        private void DSSaveInternal(Msg_LD_Save msg)
        {
            try {
                RequestSave(msg, (ret) => {
                    KeyString primaryKey = KeyString.Wrap(msg.PrimaryKeys);
                    if (ret.ErrorNo == Msg_DL_SaveResult.ErrorNoEnum.Success) {
                        LogSys.Log(LOG_TYPE.INFO, "Save data success. MsgId:{0}, SerialNo:{1}, Key:{2}", msg.MsgId, msg.SerialNo, primaryKey);
                    } else {
                        LogSys.Log(LOG_TYPE.ERROR, "Save data failed. MsgId:{0}, SerialNo:{1}, Key:{2}, Error:{3}, ErrorInfo:{4}",
                                                    msg.MsgId, msg.SerialNo, primaryKey, ret.ErrorNo, ret.ErrorInfo);
                    }
                });
            } catch (Exception e) {
                LogSys.Log(LOG_TYPE.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //供外部通过QueueAction调用的方法，实际执行线程是DataCacheThread。
        //--------------------------------------------------------------------------------------------------------------------------
        internal void DSPLoadAccount(string accountId)
        {
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableAccount;
            msg.PrimaryKeys.Add(accountId);
            Msg_LD_SingleLoadRequest reqAccount = new Msg_LD_SingleLoadRequest();
            reqAccount.MsgId = (int)DataEnum.TableAccount;
            reqAccount.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqAccount.Keys.Add(accountId);
            msg.LoadRequests.Add(reqAccount);
            RequestLoad(msg, (ret) => {
                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "DataCache Load Success: Msg:{0}, Key:{1}", "Account", primaryKey);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "Account", primaryKey);
                } else {
                    LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "Account", primaryKey, ret.ErrorInfo);
                }
                UserProcessScheduler dataProcessScheduler = UserServer.Instance.UserProcessScheduler;
                dataProcessScheduler.DefaultUserThread.QueueAction(dataProcessScheduler.DSPLoadAccountCallback, accountId, ret);
            });
        }
        internal void DSPLoadUser(ulong userGuid, string accountId)
        {
            string key = userGuid.ToString();
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableUserInfo;
            msg.PrimaryKeys.Add(key);
            Msg_LD_SingleLoadRequest reqUser = new Msg_LD_SingleLoadRequest();
            reqUser.MsgId = (int)DataEnum.TableUserInfo;
            reqUser.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqUser.Keys.Add(key);
            msg.LoadRequests.Add(reqUser);
            Msg_LD_SingleLoadRequest reqItem = new Msg_LD_SingleLoadRequest();
            reqItem.MsgId = (int)DataEnum.TableItemInfo;
            reqItem.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti;
            reqItem.Keys.Add(key);
            msg.LoadRequests.Add(reqItem);
            Msg_LD_SingleLoadRequest reqFriend = new Msg_LD_SingleLoadRequest();
            reqFriend.MsgId = (int)DataEnum.TableFriendInfo;
            reqFriend.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti;
            reqFriend.Keys.Add(key);
            msg.LoadRequests.Add(reqFriend);
            RequestLoad(msg, (ret) => {
                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, "DataCache Load Success: Msg:{0}, Key:{1}", "DSP_User", primaryKey);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "DSP_User", primaryKey);
                } else {
                    LogSys.Log(LOG_TYPE.ERROR, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "DSP_User", primaryKey, ret.ErrorInfo);
                }
                UserProcessScheduler dataProcessScheduler = UserServer.Instance.UserProcessScheduler;
                dataProcessScheduler.DefaultUserThread.QueueAction(dataProcessScheduler.DSPLoadUserCallback, ret, accountId);
            });
        }
        //
        internal void GMLoadAccount(string gmAccount, string accountId, int nodeHandle)
        {
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableAccount;
            msg.PrimaryKeys.Add(accountId);
            Msg_LD_SingleLoadRequest reqAccount = new Msg_LD_SingleLoadRequest();
            reqAccount.MsgId = (int)DataEnum.TableAccount;
            reqAccount.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqAccount.Keys.Add(accountId);
            msg.LoadRequests.Add(reqAccount);
            RequestLoad(msg, (ret) => {
                JsonMessage resultMsg = new JsonMessage(LobbyGmMessageDefine.Msg_CL_GmQueryAccount, gmAccount);
                GameFrameworkMessage.Msg_LC_GmQueryAccount protoData = new GameFrameworkMessage.Msg_LC_GmQueryAccount();
                protoData.m_Result = GameFrameworkMessage.GmResultEnum.Failed;
                protoData.m_QueryAccount = accountId;
                protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Offline;
                resultMsg.m_ProtoData = protoData;

                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "DataCache Load Success: Msg:{0}, Key:{1}", "Account", primaryKey);
                    TableAccount dataAccount = null;
                    foreach (var result in ret.Results) {
                        object _msg;
                        if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                            DataEnum msgEnum = (DataEnum)result.MsgId;
                            switch (msgEnum) {
                                case DataEnum.TableAccount:
                                    dataAccount = _msg as TableAccount;
                                    break;
                                default:
                                    LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "Decode account data ERROR. Wrong message id. Account:{0}, WrongId:{1}", result.PrimaryKeys[0], msgEnum);
                                    break;
                            }
                        }
                    }
                    protoData.m_Result = GameFrameworkMessage.GmResultEnum.Success;
                    protoData.m_QueryAccount = dataAccount.AccountId;
                    protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Online;
                    if (dataAccount.IsBanned) {
                        protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Banned;
                    }
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "Account", primaryKey);
                } else {
                    LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "Account", primaryKey, ret.ErrorInfo);
                }
                resultMsg.m_ProtoData = protoData;
                JsonGmMessageDispatcher.SendNodeMessage(nodeHandle, resultMsg);
            });
        }
        internal void GMLoadUser(string gmAccount, ulong userGuid, LobbyGmMessageDefine jsonMsgId, int nodeHandle)
        {
            string key = userGuid.ToString();
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableUserInfo;
            msg.PrimaryKeys.Add(key);
            Msg_LD_SingleLoadRequest reqUser = new Msg_LD_SingleLoadRequest();
            reqUser.MsgId = (int)DataEnum.TableUserInfo;
            reqUser.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqUser.Keys.Add(key);
            msg.LoadRequests.Add(reqUser);
            RequestLoad(msg, (ret) => {
                JsonMessage resultMsg = new JsonMessage(jsonMsgId, gmAccount);
                GameFrameworkMessage.Msg_LC_GmQueryUser protoData = new GameFrameworkMessage.Msg_LC_GmQueryUser();
                protoData.m_Result = GameFrameworkMessage.GmResultEnum.Failed;
                protoData.m_Info = null;

                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, "DataCache Load Success: Msg:{0}, Key:{1}", "GmUser", primaryKey);
                    TableUserInfo dataUser = null;
                    foreach (var result in ret.Results) {
                        object _msg;
                        if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                            DataEnum msgEnum = (DataEnum)result.MsgId;
                            switch (msgEnum) {
                                case DataEnum.TableUserInfo:
                                    dataUser = _msg as TableUserInfo;
                                    break;
                                default:
                                    LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "Decode user data ERROR. Wrong message id. UserGuid:{0}, WrongId:{1}", userGuid, msgEnum);
                                    break;
                            }
                        }
                    }
                    protoData.m_Result = GameFrameworkMessage.GmResultEnum.Success;
                    protoData.m_Info = CreateGmUserInfo(dataUser);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "GmUser", primaryKey);
                } else {
                    LogSys.Log(LOG_TYPE.ERROR, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "GmUser", primaryKey, ret.ErrorInfo);
                }
                resultMsg.m_ProtoData = protoData;
                JsonGmMessageDispatcher.SendNodeMessage(nodeHandle, resultMsg);
            });
        }
        private GameFrameworkMessage.GmUserInfo CreateGmUserInfo(TableUserInfo user)
        {
            if (null == user)
                return null;
            GameFrameworkMessage.GmUserInfo gmUserInfo = new GameFrameworkMessage.GmUserInfo();
            gmUserInfo.m_Guid = (ulong)user.Guid;
            gmUserInfo.m_AccountId = user.AccountId;
            gmUserInfo.m_Nickname = user.Nickname;
            gmUserInfo.m_UserState = GameFrameworkMessage.GmStateEnum.Offline;
            //基础信息
            GameFrameworkMessage.GmUserBasic gmUserBasic = new GameFrameworkMessage.GmUserBasic();
            gmUserBasic.m_HeroId = user.HeroId;
            gmUserBasic.m_Level = user.Level;
            gmUserBasic.m_Money = user.Money;
            gmUserBasic.m_Gold = user.Gold;
            gmUserBasic.m_CreateTime = user.CreateTime;
            gmUserBasic.m_LastLogoutTime = user.LastLogoutTime;
            gmUserInfo.m_UserBasic = gmUserBasic;
            return gmUserInfo;
        }
        //--------------------------------------------------------------------------------------
        //内部线程，用于需要排队处理的逻辑。
        //--------------------------------------------------------------------------------------
        private void OnStart()
        {
            m_DataStoreAvailable = UserServerConfig.DataStoreAvailable;
            if (!m_DataStoreAvailable) {
                LoadGlobalData();
            }
        }
        private void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(LOG_TYPE.MONITOR, "DataCacheThread Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;
                DebugPoolCount((string msg) => {
                    LogSys.Log(LOG_TYPE.INFO, "DataCacheThread.DispatchActionQueue {0}", msg);
                });
                DebugActionCount((string msg) => {
                    LogSys.Log(LOG_TYPE.MONITOR, "DataCacheThread.DispatchActionQueue {0}", msg);
                });
                LogSys.Log(LOG_TYPE.MONITOR, "DataCacheThread.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);
                LogSys.Log(LOG_TYPE.MONITOR, "DataCacheThread Load Request Count {0} Save Request Count {1}", CalcLoadRequestCount(), CalcSaveRequestCount());
            }
            if (UserServerConfig.DataStoreAvailable == true) {
                if (m_LastOperateTickTime + c_OperateTickInterval < curTime) {
                    m_LastOperateTickTime = curTime;
                    //Load操作
                    foreach (var pair in m_LoadRequests) {
                        int msgId = pair.Key;
                        ConcurrentDictionary<KeyString, LoadRequestInfo> dict = pair.Value;
                        foreach (var reqPair in dict) {
                            KeyString primaryKey = reqPair.Key;
                            LoadRequestInfo req = reqPair.Value;
                            if (req.m_LastSendTime + UserServerConfig.DSRequestTimeout < curTime) {
                                m_WaitDeletedLoadRequests.Add(primaryKey, req.m_Request.SerialNo);
                                Msg_DL_LoadResult result = new Msg_DL_LoadResult();
                                result.MsgId = msgId;
                                result.PrimaryKeys.AddRange(primaryKey.Keys);
                                result.SerialNo = req.m_Request.SerialNo;
                                result.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.TimeoutError;
                                result.ErrorInfo = "Timeout Error";
                                if (null != req.m_Callback) {
                                    DispatchAction(req.m_Callback, result);
                                }
                            }
                        }
                        foreach (var delPair in m_WaitDeletedLoadRequests) {
                            LoadRequestInfo info;
                            if (dict.TryRemove(delPair.Key, out info)) {
                                if (info.m_Request.SerialNo == delPair.Value) {
                                    m_LoadRequestPool.Recycle(info);
                                } else {
                                    dict.TryAdd(delPair.Key, info);
                                }
                            }
                        }
                        m_WaitDeletedLoadRequests.Clear();
                    }
                    //Save操作
                    foreach (var pair in m_SaveRequestQueues) {
                        int msgId = pair.Key;
                        ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict = pair.Value;
                        foreach (var reqPair in dict) {
                            KeyString primaryKey = reqPair.Key;
                            ConcurrentQueue<SaveRequestInfo> saveReqQueue = reqPair.Value;
                            SaveRequestInfo req;
                            if (saveReqQueue.TryPeek(out req)) {
                                if (req.m_LastSendTime + UserServerConfig.DSRequestTimeout < curTime) {
                                    //超时,重新发送
                                    LogSys.Log(LOG_TYPE.ERROR, "DataCacheThread. SaveRequest timeout. MsgId:{0}, PrimaryKey:{1}, SerialNo:{2}",
                                                                  msgId, KeyString.Wrap(req.m_Request.PrimaryKeys), req.m_Request.SerialNo);
                                    m_DataStoreChannel.Send(req.m_Request);
                                    req.m_LastSendTime = curTime;
                                }
                            }
                            if (dict.Count > c_MaxRecordPerMessage && saveReqQueue.Count == 0) {
                                m_WaitDeleteSaveRequests.Add(primaryKey);
                            }
                        }
                        foreach (KeyString key in m_WaitDeleteSaveRequests) {
                            lock (m_SaveRequestQueuesLock) {
                                ConcurrentQueue<SaveRequestInfo> queue;
                                if (dict.TryGetValue(key, out queue)) {
                                    if (queue.Count == 0) {
                                        ConcurrentQueue<SaveRequestInfo> dummy;
                                        dict.TryRemove(key, out dummy);
                                    }
                                }
                            }
                        }
                        m_WaitDeleteSaveRequests.Clear();
                    }
                }
                if (m_LastDSConnectTime + c_DSConnectInterval < curTime) {
                    m_LastDSConnectTime = curTime;
                    if (m_CurrentStatus != ConnectStatus.Connected) {
                        ConnectDataStore();
                    }
                }
            }
        }

        private bool CheckGlobalDataInitFinish()
        {
            if ((m_GuidInitStatus == DataInitStatus.Done) && (m_NicknameInitStatus == DataInitStatus.Done)) {
                LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "GlobalDataInitFinish ...");
                return true;
            } else {
                return false;
            }
        }

        private bool m_DataStoreAvailable = false;
        private DataInitStatus m_GuidInitStatus = DataInitStatus.Unload;
        private DataInitStatus m_NicknameInitStatus = DataInitStatus.Unload;

        private long GenNextSerialNo()
        {
            return Interlocked.Increment(ref m_NextSerialNo);
        }

        private long m_NextSerialNo = 0;

        private PBChannel m_DataStoreChannel = null;
        private ConnectStatus m_CurrentStatus = ConnectStatus.None;
        private ConcurrentDictionary<int, ConcurrentDictionary<KeyString, LoadRequestInfo>> m_LoadRequests = new ConcurrentDictionary<int, ConcurrentDictionary<KeyString, LoadRequestInfo>>();
        private ConcurrentDictionary<int, ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>> m_SaveRequestQueues = new ConcurrentDictionary<int, ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>>();
        private ServerConcurrentObjectPool<LoadRequestInfo> m_LoadRequestPool = new ServerConcurrentObjectPool<LoadRequestInfo>();
        private ServerConcurrentObjectPool<SaveRequestInfo> m_SaveRequestPool = new ServerConcurrentObjectPool<SaveRequestInfo>();
        private Dictionary<KeyString, long> m_WaitDeletedLoadRequests = new Dictionary<KeyString, long>();
        private List<KeyString> m_WaitDeleteSaveRequests = new List<KeyString>();
        private object m_SaveRequestQueuesLock = new object();
        private const int c_MaxRecordPerMessage = 1000;

        private long m_LastOperateTickTime = 0;
        private const long c_OperateTickInterval = 1000;    //超时Tick的时间间隔
        private long m_LastDSConnectTime = 0;
        private const long c_DSConnectInterval = 5000;      //连接DataCache的时间间隔

        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;

        private MyServerThread m_Thread = null;
    }
}
