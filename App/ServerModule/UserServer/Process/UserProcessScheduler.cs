using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using GameFrameworkData;
using GameFrameworkMessage;
using GameFramework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CSharpCenterClient;

namespace GameFramework
{
    /// <summary>
    /// Player data processing scheduler, player data requests will be processed in several parallel threads.
    /// There are 2 types of threads:
    /// 1. For operations initiated by GetUserThread().QueueAction or DefaultUserThread.QueueAction, the execution thread cannot be specified at this time.
    /// 2. Instantiate a thread internally in the scheduler to perform operations that must be performed in a thread. (The interface is not provided to the outside world. It is currently assumed to be used for orderly services required for operations in 1.)
    /// </summary>
    /// <remarks>
    /// This class uses multi-threading to operate data. Each UserInfo will be processed in one thread after entering the game and going offline. However, since the global thread and internal thread will also use user data, multi-threading issues still need to be considered. .
    /// Please note a few conventions:
    /// 1. Once UserInfo is instantiated, the memory will not be released (it will only be recycled into the pool for reuse, the same is true for RoomInfo).
    /// 2. For functions that only operate data less than or equal to the machine word length, no locking is performed (the operation is inherently atomic).
    /// 3. When the global thread operates user data, the operation needs to be queued to the user thread for processing and then returned to the global thread for processing (it will be more troublesome, but this can avoid locking).
    /// 4. When reading and writing the collection data in UserInfo, the concurrency of all places accessing the data must be considered, and locking is required.
    /// </remarks>
    internal sealed class UserProcessScheduler
    {
        internal UserProcessScheduler()
        {
            m_Thread = new MyServerThread();
            m_Thread.TickSleepTime = 10;
            m_Thread.ActionNumPerTick = 10240;
            m_Thread.OnTickEvent += this.OnTick;

            m_NodeMessageManager.Init(3, 10, 4096);

            m_DefaultThread = new UserThread(10, 4096);
            for (int i = 0; i < m_UserThreads.Length; ++i) {
                m_UserThreads[i] = new UserThread(10, 4096);
            }
        }
        internal void Start()
        {
            m_Thread.Start();
            m_DefaultThread.Start();
            for (int i = 0; i < m_UserThreads.Length; ++i) {
                m_UserThreads[i].Start();
            }
            m_NodeMessageManager.Start();
        }
        internal void Stop()
        {
            m_NodeMessageManager.Stop();
            for (int i = 0; i < m_UserThreads.Length; ++i) {
                MyServerThread thread = m_UserThreads[i];
                thread.Stop();
            }
            m_Thread.Stop();
        }
        internal void DispatchJsonMessage(bool isGmTool, uint seq, ulong sourceHandle, ulong destHandle, byte[] data)
        {
            m_NodeMessageManager.DispatchMessage(isGmTool, seq, sourceHandle, destHandle, data);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //Methods for direct external calls need to ensure multi-thread safety.
        //--------------------------------------------------------------------------------------------------------------------------
        internal NicknameSystem NicknameSystem
        {
            get
            {
                return m_NicknameSystem;
            }
        }
        internal void VisitUsers(MyAction<UserInfo> visitor)
        {
            foreach (KeyValuePair<ulong, UserInfo> pair in m_UserInfos) {
                UserInfo userInfo = pair.Value;
                visitor(userInfo);
            }
        }
        internal AccountInfo GetAccountInfoById(string accountId)
        {
            return m_AccountSystem.FindAccountById(accountId);
        }
        //Find UserGuid based on Nickname among current online players
        internal ulong GetGuidByNickname(string nickname)
        {
            ulong guid = 0;
            m_GuidByNickname.TryGetValue(nickname, out guid);
            return guid;
        }
        //All players find UserGuid based on Nickname
        internal ulong FindUserGuidByNickname(string nickname)
        {
            return m_NicknameSystem.FindUserGuidByNickname(nickname);
        }
        //All players fuzzy search UserGuid based on Nickname
        internal List<ulong> FindUserGuidByFuzzyNickname(string fuzzyName)
        {
            return m_NicknameSystem.FindUserGuidByFuzzyNickname(fuzzyName);
        }
        internal UserInfo GetUserInfo(ulong guid)
        {
            UserInfo info = null;
            m_UserInfos.TryGetValue(guid, out info);
            return info;
        }
        internal int GetUserCount()
        {
            return m_UserInfos.Count;
        }
        internal UserThread GetUserThread(ulong guid)
        {
            UserInfo user = GetUserInfo(guid);
            if (user == null) {
                LogSys.Log(ServerLogType.ERROR, "GetUserInfo ERROR. UserGuid:{0}", guid);
            }
            return GetUserThread(user);
        }
        internal UserThread GetUserThread(UserInfo user)
        {
            UserThread thread;
            int ix = user.UserThreadIndex;
            if (ix >= 0 && ix < m_UserThreads.Length) {
                thread = m_UserThreads[ix];
            } else {
                thread = m_UserThreads[0];
            }
            return thread;
        }
        internal UserThread DefaultUserThread
        {
            get
            {
                return m_DefaultThread;
            }
        }
        internal void DoLastSaveUserData()
        {
            m_LastSaveFinished = false;
            //The last storage operation before the server was shut down
            var ds_thread = UserServer.Instance.DataCacheThread;
            if (ds_thread.DataStoreAvailable == true) {
                //1. Notify the client that the server is shut down
                foreach (var guidPair in m_ActiveUserGuids) {
                    UserInfo user = GetUserInfo(guidPair.Key);
                    if (user != null) {
                        NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.ServerShutdown, user.Guid);
                        NodeMessageDispatcher.SendNodeMessage(user.NodeName, retMsg);
                    }
                }
                LogSys.Log(ServerLogType.MONITOR, "DoLastSaveUserData Step_1: Notice game client ServerShutdown. UserCount:{0}", m_ActiveUserGuids.Count);
                //2. Wait 10s
                Thread.Sleep(10000);
                LogSys.Log(ServerLogType.MONITOR, "DoLastSaveUserData Step_2: Wait for 10s.");
                //3. Close Node
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs1", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs2", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs3", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs4", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs5", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs6", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs7", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs8", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "NodeJs9", "QuitNodeJs");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr1", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr2", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr3", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr4", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr5", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr6", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr11", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr12", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr13", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr14", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr15", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "RoomSvr16", "QuitRoomServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "GmServer", "QuitGmServer");
                CenterHubApi.SendCommandByName(UserServerConfig.WorldId, "ServerBridge", "QuitServerBridge");
                LogSys.Log(ServerLogType.MONITOR, "DoLastSaveUserData Step_3: Close Servers.");
                //4. Save player data
                foreach (var guidPair in m_ActiveUserGuids) {
                    UserInfo user = GetUserInfo(guidPair.Key);
                    if (user != null) {
                        user.NextUserSaveCount = DataCacheThread.UltimateSaveCount;
                        this.GetUserThread(user).QueueAction(ds_thread.SaveUser, user, user.NextUserSaveCount);
                    }
                }
                LogSys.Log(ServerLogType.MONITOR, "DoLastSaveUserData Step_4: Start to save UserData for last. UserCount:{0}", m_ActiveUserGuids.Count);
            }
            m_IsLastSave = true;
        }
        internal bool LastSaveFinished
        {
            get
            {
                return m_LastSaveFinished;
            }
        }
        //------------------------------------------------ -------------------------------------------------- --------------------------
        //The method called by GM messages. The actual execution thread is executed in a thread in the thread pool. When implementing,
        //you need to pay attention to concurrency issues and locks.
        //Logical functions should not directly call this type of processing (it will be judged by gm permissions, and the official
        //server will cause the function to fail). You should call a logical method with the same function (usually named starting with Do).
        //------------------------------------------------ -------------------------------------------------- --------------------------

        //------------------------------------------------ -------------------------------------------------- --------------------------
        //For external calls through QueueAction, the actual execution thread is executed in a thread in the thread pool.
        //When implementing, you need to pay attention to concurrency issues and locks.
        //------------------------------------------------ -------------------------------------------------- --------------------------
        //Login process related methods
        internal void DoAccountLogin(string accountId, string password, string clientInfo, string nodeName)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            QueueingThread queueingThread = UserServer.Instance.QueueingThread;
            if (null != accountInfo || !queueingThread.NeedQueueing()) {
                LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_3a: DoLogin without queueing. AccountId:{0}", accountId);
                DoAccountLoginWithoutQueueing(accountId, password, clientInfo, nodeName);
            } else {
                if (queueingThread.IsQueueingFull()) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "LoginStep_3b1: Start queueing but queue full. AccountId:{0}", accountId);

                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.QueueFull;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                } else {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "LoginStep_3b2: Start queueing. AccountId:{0}", accountId);

                    queueingThread.QueueAction(queueingThread.StartQueueing, accountId, password, clientInfo, nodeName);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Queueing;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                }
            }
        }
        internal void DoAccountLoginWithoutQueueing(string accountId, string password, string clientInfo, string nodeName)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo == null) {
                //The current accountId is not online
                accountInfo = new AccountInfo();
                accountInfo.AccountId = accountId;
                accountInfo.Password = password;
                accountInfo.ClientInfo = clientInfo;
                accountInfo.NodeName = nodeName;
                accountInfo.CurrentState = AccountState.Unloaded;
                var dsThread = UserServer.Instance.DataCacheThread;
                if (dsThread.DataStoreAvailable == true) {
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_4: Load account from DataStore . AccountId:{0}", accountId);
                    dsThread.DispatchAction(dsThread.LoadAccount, accountId);
                } else {
                    accountInfo.UserGuid = UserServer.Instance.GlobalProcessThread.GenerateUserGuid();
                    accountInfo.CurrentState = AccountState.Online;
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    LogSys.Log(ServerLogType.INFO, "Account login success. Account:{0}", accountId);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.FirstLogin;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                }
            } else {
                //The current accountId is online
                if (accountInfo.CurrentState == AccountState.Dropped || clientInfo == accountInfo.ClientInfo) {
                    //The account is offline or the same device is logged in repeatedly, and the login is successful.
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_5a: Account is dropped. Login SUCCESS. AccountId:{0}", accountId);
                    accountInfo.AccountId = accountId;
                    accountInfo.Password = password;
                    accountInfo.ClientInfo = clientInfo;
                    accountInfo.NodeName = nodeName;
                    accountInfo.CurrentState = AccountState.Online;
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    if (!string.IsNullOrEmpty(accountInfo.Nickname))
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Success;
                    else
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.FirstLogin;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                } else {
                    //The account was logged in on another device, but the login failed.
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "LoginStep_5b: Account is online. Login FAILED. AccountId:{0}", accountId);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.AlreadyOnline;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                }
            }
        }
        internal void LoadAccountCallback(string accountId, Msg_DL_LoadResult ret)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo == null) {
                return;
            }
            NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
            GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
            protoMsg.m_AccountId = accountId;
            protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Error;
            try {
                if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                    foreach (var result in ret.Results) {
                        object _msg;
                        if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                            DataEnum msgEnum = (DataEnum)result.MsgId;
                            switch (msgEnum) {
                                case DataEnum.TableAccount:
                                    accountInfo.FromProto(_msg as TableAccount);
                                    break;
                                default:
                                    LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Decode account data ERROR. Wrong message id. Account:{0}, WrongId:{1}", result.PrimaryKeys[0], msgEnum);
                                    break;
                            }
                        }
                    }
                    if (accountInfo.IsBanned) {
                        //Account has been suspended
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Banned;
                    } else {
                        accountInfo.CurrentState = AccountState.Online;
                        accountInfo.LastLoginTime = TimeUtility.CurTimestamp;
                        m_AccountSystem.AddAccountById(accountId, accountInfo);
                        LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_4a: Account login SUCCESS . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Success;
                        protoMsg.m_UserGuid = accountInfo.UserGuid;                        
                    }
                } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                    //Account enters the game for the first time
                    accountInfo.UserGuid = UserServer.Instance.GlobalProcessThread.GenerateUserGuid();
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_4b: Load account NotFound . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                    accountInfo.CurrentState = AccountState.Online;
                    accountInfo.AccountId = accountId;
                    accountInfo.Password = accountInfo.Password;
                    accountInfo.IsBanned = false;
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.FirstLogin;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    //norm log    
                    accountInfo.LastLoginTime = TimeUtility.CurTimestamp;
                } else {
                    //Data loading failed
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "LoginStep_4c: Load account FAILED . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.INFO, ConsoleColor.Red, "ERROR LoginStep_4d. AccountId:{0}, LogicServerId:{1}, AccountId:{2}\nERROR Message:{3}\nStackTrace:{4}",
                            accountId, 0, accountId, ex.Message, ex.StackTrace);
            } finally {
                if (protoMsg.m_Result != AccountLoginResult.AccountLoginResultEnum.Success && protoMsg.m_Result != AccountLoginResult.AccountLoginResultEnum.FirstLogin) {
                    m_AccountSystem.RemoveAccountById(accountId);
                }
                replyMsg.m_ProtoData = protoMsg;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            }
        }

        internal void OnAccountLogout(string accountId)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo != null) {
                //Check whether there is a character online in the current account
                bool isAccountKickable = true;
                UserInfo ui = GetUserInfo(accountInfo.UserGuid);
                if (ui != null) {
                    isAccountKickable = false;
                }
                if (isAccountKickable) {
                    //Kick account
                    accountInfo.CurrentState = AccountState.Offline;
                    m_AccountSystem.RemoveAccountById(accountInfo.AccountId);
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LogoutStep: Account LOGOUT directly. AccountId:{0}",
                      accountInfo.AccountId);
                } else {
                    //AccountInfo is set to offline status
                    accountInfo.CurrentState = AccountState.Dropped;
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LogoutStep: Account dropped. AccountId:{0}",
                      accountInfo.AccountId);
                }
                m_NicknameSystem.RevertAccountNicknames(accountId);
            }
        }
        internal void DoRequestNickname(string accountId)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo != null) {
                NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.RequestNicknameResult, accountId);
                GameFrameworkMessage.RequestNicknameResult protoMsg = new GameFrameworkMessage.RequestNicknameResult();
                List<string> nicknameList = m_NicknameSystem.RequestNicknames(accountId);
                protoMsg.m_Nicknames.AddRange(nicknameList);

                replyMsg.m_ProtoData = protoMsg;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            }
        }
        internal void DoRoleEnter(string accountId, string nickname)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo != null) {
                ulong userGuid = accountInfo.UserGuid;
                UserInfo user = GetUserInfo(userGuid);
                if (null != user) {
                    if (user.CurrentState == UserState.DropOrOffline) {
                        //The user is in the offline process and needs to wait for the lobby offline process to be completed.
                        NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountId, user.Guid);
                        GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                        protoData.Result = RoleEnterResult.RoleEnterResultEnum.Wait;
                        roleEnterResultMsg.m_ProtoData = protoData;
                        NodeMessageDispatcher.SendNodeMessage(user.NodeName, roleEnterResultMsg);
                        LogSys.Log(ServerLogType.WARN, "RoleEnter AccountId:{0} Guid:{1} Wait Offline", accountId, userGuid);
                    } else {
                        if (user.AccountId.Equals(accountInfo.AccountId)) {         
                            user.NodeName = accountInfo.NodeName;
                            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
                            DoUserRelogin(user);
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_8a: Role Reenter SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}",
                                accountId, userGuid, user.Nickname);
                            //Reply to client
                            NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, user.AccountId, user.Guid);
                            GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(user);
                            protoData.Result = RoleEnterResult.RoleEnterResultEnum.Reconnect;
                            roleEnterResultMsg.m_ProtoData = protoData;
                            NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, roleEnterResultMsg);
                        } else {
                            //The character AccountId does not match the account AccountId, and entry into the game fails.
                            NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountId, user.Guid);
                            GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                            protoData.Result = RoleEnterResult.RoleEnterResultEnum.UnknownError;
                            roleEnterResultMsg.m_ProtoData = protoData;
                            LogSys.Log(ServerLogType.ERROR, "LoginStep_8a: Role Reenter FAILED. AccountId:{0}, UserGuid:{1}, Nickname:{2}, UserAccountId:{3}",
                                accountId, userGuid, user.Nickname, user.AccountId);
                        }
                    }
                } else {
                    var ds_thread = UserServer.Instance.DataCacheThread;
                    if (ds_thread.DataStoreAvailable == true) {
                        LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_8b: Load UserInfo. AccountId:{0}, UserGuid:{1}", accountId, userGuid);
                        ds_thread.DispatchAction(ds_thread.LoadUser, userGuid, accountId, nickname);
                    } else {
                        CreateRole(accountId, nickname, 1);
                    }
                }
            }
        }
        internal void LoadUserCallback(Msg_DL_LoadResult ret, string accountId, string nickname)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo == null) {
                return;
            }
            ulong userGuid = 0;// accountInfo.UserGuid;
            if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                #region Build UserInfo from database data
                UserInfo ui = NewUserInfo();
                foreach (var result in ret.Results) {
                    object _msg;
                    if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                        DataEnum msgEnum = (DataEnum)result.MsgId;
                        switch (msgEnum) {
                            case DataEnum.TableUserInfo: {
                                    var dataUser = _msg as TableUserInfo;
                                    ui.FromProto(dataUser);
                                    ui.SetMoneyForDB(dataUser.Money);
                                    ui.SetGoldForDB(dataUser.Gold);
                                }
                                break;
                            case DataEnum.TableMemberInfo: {
                                    var dataMember = _msg as TableMemberInfo;
                                    var mi = new MemberInfo();
                                    mi.FromProto(dataMember);
                                    ui.MemberInfos.Add(mi);
                                }
                                break;
                            case DataEnum.TableItemInfo: {
                                    var dataItem = _msg as TableItemInfo;
                                    var ii = new ItemInfo();
                                    ii.FromProto(dataItem);
                                    ui.ItemBag.ItemInfos.Add(ii);
                                }
                                break;
                            case DataEnum.TableFriendInfo: {
                                    var dataFriend = _msg as TableFriendInfo;
                                    var fi = new FriendInfo();
                                    fi.FromProto(dataFriend);
                                    ui.FriendInfos.Add(fi);
                                }
                                break;
                            default:
                                LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Decode user data ERROR. Wrong message id. UserGuid:{0}, WrongId:{1}", userGuid, msgEnum);
                                break;
                        }
                    }
                }
                //basic data
                #endregion
                ui.NodeName = accountInfo.NodeName;
                this.DoUserLogin(ui);
                NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, ui.AccountId, ui.Guid);
                GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(ui);
                protoData.Result = RoleEnterResult.RoleEnterResultEnum.Success;
                replyMsg.m_ProtoData = protoData;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                CreateRole(accountId, nickname, 1);
            } else {
                NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountInfo.AccountId, userGuid);
                GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                protoData.Result = RoleEnterResult.RoleEnterResultEnum.UnknownError;
                replyMsg.m_ProtoData = protoData;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            }
            LogSys.Log(ServerLogType.INFO, "UserProcessScheduler-OnRoleEnter-EndLoadUser. AccountId:{0}, UserGuid:{1}, Result:{2}",
              accountId, userGuid, ret.ErrorNo);
        }
        internal void DoChangeName(ulong guid, string nickname)
        {
            UserInfo ui = GetUserInfo(guid);
            if (null != ui) {
                NicknameSystem.CheckNicknameResult ret = m_NicknameSystem.CheckNickname(ui.AccountId, nickname);
                if (ret == NicknameSystem.CheckNicknameResult.Success) {
                    var accountInfo = GetAccountInfoById(ui.AccountId);
                    if (null != accountInfo) {
                        accountInfo.Nickname = nickname;
                    }
                    ui.Nickname = nickname;
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "ChangeName SUCCESS. AccountId:{0}, Nickname:{1}",
                      ui.AccountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, ui.AccountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.Success;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(ui.NodeName, replyMsg);
                } else {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "ChangeName FAILED. AccountId:{0}, Nickname:{1}",
                      ui.AccountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, ui.AccountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    if (ret == NicknameSystem.CheckNicknameResult.AlreadyUsed) {
                        //Nickname is already taken
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.NicknameError;
                    } else {
                        //Nickname is illegal
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.UnknownError;
                    }
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(ui.NodeName, replyMsg);
                }
            }
        }
        internal void DoUserLogin(UserInfo user)
        {
            user.SceneId = 0;
            user.IsDisconnected = false;
            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
            user.LastSaveTime = TimeUtility.GetLocalMilliseconds();
            //Generate the key to be used in the game here
            if (user.Key == 0)
                user.Key = GenerateKey();
            user.CurrentState = UserState.Online;
            AllocUserThread(user);
            UserInfo oldInfo;
            if (m_UserInfos.TryGetValue(user.Guid, out oldInfo)) {
                if (oldInfo != user) {
                    //Here you need to recycle old player data (reset status, etc.) to ensure that places
                    //that reference old data can be processed correctly.
                    FreeUserThread(oldInfo);
                    RecycleUserInfo(oldInfo);
                }
            }
            m_UserInfos.AddOrUpdate(user.Guid, user, (g, u) => user);
            m_GuidByNickname.AddOrUpdate(user.Nickname, user.Guid, (n, g) => user.Guid);
            GlobalProcessThread globalProcess = UserServer.Instance.GlobalProcessThread;
            if (null != globalProcess) {
            }
            m_Thread.QueueAction(this.ActivateUserGuid, user.Guid);
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(user.AccountId);
            if (null != accountInfo) {
                user.ClientInfo = accountInfo.ClientInfo;
                user.LastLoginTime = TimeUtility.CurTimestamp;
            }
        }
        internal void DoUserRelogin(UserInfo user)
        {
            user.IsDisconnected = false;
            if (user.CurrentState != UserState.Room) {
                user.CurrentState = UserState.Online;
            } else {
                Msg_LB_UserRelogin builder = new Msg_LB_UserRelogin();
                builder.Guid = user.Guid;
                UserServer.Instance.BigworldChannel.Send(builder);
            }
            m_Thread.QueueAction(this.ActivateUserGuid, user.Guid);
        }
        internal void DoUserLogoff(ulong guid)
        {
            DoUserLogoff(guid, false);
        }
        internal void DoUserLogoff(ulong guid, bool forceLogoff)
        {
            UserInfo user = GetUserInfo(guid);
            if (user != null) {
                user.IsDisconnected = true;
                if (user.CurrentState == UserState.Room || user.CurrentState == UserState.DropOrOffline || forceLogoff) {//Only go offline if the heartbeat times out (5 minutes) or is already offline.
                    QueueingThread queueingThread = UserServer.Instance.QueueingThread;

                    double onlinetimes = TimeUtility.CurTimestamp - user.LastLoginTime;
                    user.LastLogoutTime = DateTime.Now;
                    var ds_thread = UserServer.Instance.DataCacheThread;
                    if (ds_thread.DataStoreAvailable == true) {
                        user.NextUserSaveCount = DataCacheThread.UltimateSaveCount;
                        this.GetUserThread(user.Guid).QueueAction(ds_thread.SaveUser, user, user.NextUserSaveCount);
                        LogSys.Log(ServerLogType.INFO, "UserProcessScheduler-DoUserLogoff-StartSaveUser. UserGuid:{0}", guid);
                    }
                    m_Thread.QueueAction(this.AddWaitRecycleUser, guid);
                    user.CurrentState = UserState.DropOrOffline;
                }
            }
        }
        internal void DoUserHeartbeat(ulong guid)
        {
            UserInfo user = GetUserInfo(guid);
            if (user != null) {
                user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
            } else {
                LogSys.Log(ServerLogType.DEBUG, "DoUserHeartbeat,guid:{0} can't found.", guid);
            }
        }
        internal void AddKickedAccount(string accountId, long ms)
        {
            long removeTime = TimeUtility.GetLocalMilliseconds() + ms;
            m_KickedAccountDict[accountId] = removeTime;
        }
        internal void RemoveKickedAccount(string accountId)
        {
            m_KickedAccountDict.Remove(accountId);
        }
        internal void HandleBroadcast(BroadcastType type, string content, int roll_ct)
        {
            if (null == content || content.Length <= 0) {
                return;
            }
        }
        internal void InitNicknameData(List<TableNicknameInfo> nicknameList)
        {
            m_NicknameSystem.InitNicknameData(nicknameList);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //These methods are tool methods, and concurrency-related processing needs to be reconsidered later.
        //--------------------------------------------------------------------------------------------------------------------------
        private UserInfo NewUserInfo()
        {
            UserInfo info = null;
            if (m_UnusedUserInfos.IsEmpty) {
                info = new UserInfo();
            } else {
                if (!m_UnusedUserInfos.TryDequeue(out info)) {
                    info = new UserInfo();
                } else {
                    info.IsRecycled = false;
                }
            }
            return info;
        }
        private void RecycleUserInfo(UserInfo info)
        {
            info.IsRecycled = true;
            info.Reset();
            m_UnusedUserInfos.Enqueue(info);
        }
        private void AllocUserThread(UserInfo userInfo)
        {
            //Assigned to the thread with the smallest current number of people, evenly distributed
            int minIndex = 0;
            int minUserCount = m_UserThreadUserCounts[minIndex];
            for (int ix = 1; ix < m_UserThreadUserCounts.Length; ++ix) {
                if (minUserCount > m_UserThreadUserCounts[ix]) {
                    minUserCount = m_UserThreadUserCounts[ix];
                    minIndex = ix;
                }
            }
            for (int ct = 0; ct < 256; ++ct) {
                minUserCount = m_UserThreadUserCounts[minIndex];
                int expectCount = minUserCount + 1;
                if (Interlocked.CompareExchange(ref m_UserThreadUserCounts[minIndex], expectCount, minUserCount) == minUserCount) {
                    userInfo.UserThreadIndex = minIndex;
                    return;
                }
            }
        }
        private void FreeUserThread(UserInfo userInfo)
        {
            int ix = userInfo.UserThreadIndex;
            if (ix >= 0 && ix < m_UserThreadUserCounts.Length) {
                Interlocked.Decrement(ref m_UserThreadUserCounts[ix]);
            }
        }
        private bool IsPveScene(int sceneId)
        {
            bool ret = false;
            return ret;
        }
        private bool IsMpveScene(int sceneId)
        {
            bool ret = false;
            return ret;
        }
        private bool IsPvpScene(int sceneId)
        {
            bool ret = false;
            return ret;
        }
        private void InitUserinfo(UserInfo ui)
        {
            ui.ResetMoney();
            ui.ResetGold();
            ui.SceneId = 1010;
            ui.Level = 1;
            ui.CreateTime = DateTime.Now;
            ui.SceneId = 200;
            //Add 4 team members
            for (int id = 2; id <= 5; ++id) {
                MemberInfo member = new MemberInfo();
                member.MemberGuid = UserServer.Instance.GlobalProcessThread.GenerateMemberGuid();
                member.HeroId = id;
                member.Level = 1;
                ui.MemberInfos.Add(member);
            }

        }
        private GameFrameworkMessage.RoleEnterResult CreateRoleEnterResultMsg(UserInfo ui)
        {
            GameFrameworkMessage.RoleEnterResult replyMsg = new GameFrameworkMessage.RoleEnterResult();
            //
            replyMsg.Nickname = ui.Nickname;
            replyMsg.HeroId = ui.HeroId;
            replyMsg.Level = ui.Level;
            replyMsg.Money = ui.Money;
            replyMsg.Gold = ui.Gold;
            replyMsg.Level = ui.Level;
            replyMsg.SceneId = ui.SceneId;
            replyMsg.WorldId = UserServerConfig.WorldId;

            for (int i = 0; i < ui.MemberInfos.Count; ++i) {
                MemberInfoForMessage mi = new MemberInfoForMessage();
                mi.Hero = ui.MemberInfos[i].HeroId;
                mi.Level = ui.MemberInfos[i].Level;
            }
            return replyMsg;
        }
        private void CreateRole(string accountId, string nickname, int heroId)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            var ds_thread = UserServer.Instance.DataCacheThread;
            if (accountInfo != null) {
                //Check if nickname is available
                NicknameSystem.CheckNicknameResult ret = m_NicknameSystem.CheckNickname(accountId, nickname);
                if (ret == NicknameSystem.CheckNicknameResult.Success) {
                    accountInfo.Nickname = nickname;
                    UserInfo ui = NewUserInfo();
                    ui.Guid = accountInfo.UserGuid;
                    ui.AccountId = accountInfo.AccountId;
                    ui.Nickname = nickname;
                    ui.HeroId = heroId;
                    InitUserinfo(ui);
                    m_NicknameSystem.UpdateUsedNickname(nickname, ui.Guid);
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_7a: Create new role SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}, HeroId:{3}",
                      accountId, accountInfo.AccountId, ui.Guid, ui.Nickname, ui.HeroId);
                    if (ds_thread.DataStoreAvailable) {
                        ds_thread.SaveCreateUser(accountInfo, nickname, ui.Guid);
                        ds_thread.SaveUser(ui, ui.NextUserSaveCount);
                    }
                    //The game character is created successfully and enters the game directly.
                    ui.NodeName = accountInfo.NodeName;
                    this.DoUserLogin(ui);
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "LoginStep_8c: New role enter SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}",
                      accountId, ui.Guid, ui.Nickname);
                    NodeMessage enterMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, ui.AccountId, ui.Guid);
                    GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(ui);
                    protoData.Result = RoleEnterResult.RoleEnterResultEnum.Success;
                    enterMsg.m_ProtoData = protoData;
                    NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, enterMsg);
                } else {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "LoginStep_7b: Create new role FAILED. AccountId:{0}, Nickname:{1}",
                      accountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, accountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    if (ret == NicknameSystem.CheckNicknameResult.AlreadyUsed) {
                        //Nickname is already taken
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.NicknameError;
                    } else {
                        //Nickname is illegal
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.UnknownError;
                    }
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //The following methods are all executed in internal threads, do not involve multi-threaded operations, do not require locking, and are executed serially.
        //--------------------------------------------------------------------------------------------------------------------------
        private void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();

            int elapsedTickTime = m_Thread.TickSleepTime;
            if (m_LastTickTime > 0) {
                elapsedTickTime = (int)(curTime - m_LastTickTime);
                if (elapsedTickTime < 0) {
                    elapsedTickTime = m_Thread.TickSleepTime;
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;

                for (int threadIx = 0; threadIx < m_UserThreads.Length; ++threadIx) {
                    MyServerThread thread = m_UserThreads[threadIx];
                    thread.DebugPoolCount((string msg) => {
                        LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.DispatchActionQueue {0} {1}", threadIx, msg);
                    });
                    LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.DispatchActionQueue Current Action {0} {1}", threadIx, thread.CurActionNum);
                }
                for (int threadIx = 0; threadIx < m_UserThreadUserCounts.Length; ++threadIx) {
                    LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.UserThread User Count {0} {1}", threadIx, m_UserThreadUserCounts[threadIx]);
                }
                m_Thread.DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.ThreadActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);

                m_NodeMessageManager.TickMonitor();

                LogSys.Log(ServerLogType.MONITOR, "GameFramework User Count:{0} ElapsedTickTime:{1}", m_ActiveUserGuids.Count, elapsedTickTime);
            }

            var ds_thread = UserServer.Instance.DataCacheThread;
            m_DeactiveUserGuids.Clear();
            int userSaveDoneCount = 0;
            foreach (var guidPair in m_ActiveUserGuids) {
                ulong guid = guidPair.Key;
                UserInfo user = GetUserInfo(guid);
                if (user == null) {
                    m_DeactiveUserGuids.Add(guid);
                    userSaveDoneCount++;
                } else {
                    if (user.CurrentUserSaveCount == DataCacheThread.UltimateSaveCount) {
                        userSaveDoneCount++;
                    }
                    //Store UserInfo periodically
                    if (ds_thread.DataStoreAvailable) {
                        if (user.CurrentState != UserState.DropOrOffline && curTime - user.LastSaveTime > UserServerConfig.UserDSSaveInterval && user.NextUserSaveCount > 0) {
                            this.GetUserThread(user.Guid).QueueAction(ds_thread.SaveUser, user, user.NextUserSaveCount);
                            user.NextUserSaveCount++;
                            //random disturbance
                            int random = m_Random.Next(c_UserSaveRandom * 2);
                            if (random > c_UserSaveRandom) {
                                random = c_UserSaveRandom - random;
                            }
                            user.LastSaveTime = curTime + random;
                        }
                    }
                    //
                    user.LeftLife -= elapsedTickTime;
                    if (user.LeftLife <= 0) {
                        if (UserState.Room != user.CurrentState) {
                            this.GetUserThread(user).QueueAction(this.DoUserLogoff, guid, true);
                            LogSys.Log(ServerLogType.INFO, "UserProcessScheduler-OnUserTick-UserLogout. UserGuid:{0}", user.Guid);
                            user.LeftLife = 600000;
                        } else if (UserState.Room == user.CurrentState) {
                            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
                        } else {
                            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
                        }
                    }
                }
            }
            if (m_IsLastSave && userSaveDoneCount >= m_ActiveUserGuids.Count) {
                if (m_LastSaveFinished == false) {
                    LogSys.Log(ServerLogType.MONITOR, "DoLastSaveUserData Step_5: Save UserData done. UserCount:{0}", userSaveDoneCount);
                    m_LastSaveFinished = true;
                    m_IsLastSave = false;
                }
            }
            int tmpValue = 0;
            foreach (ulong guid in m_DeactiveUserGuids) {
                m_ActiveUserGuids.TryRemove(guid, out tmpValue);
            }

            m_DeactiveUserGuids.Clear();
            foreach (ulong guid in m_WaitRecycleUsers) {
                UserInfo user = GetUserInfo(guid);
                if (user == null) {
                    m_DeactiveUserGuids.Add(guid);
                } else {
                    if (!ds_thread.DataStoreAvailable || user.CurrentUserSaveCount == DataCacheThread.UltimateSaveCount) {
                        // norm log
                        int onlinetimes = (int)(TimeUtility.CurTimestamp - user.LastLoginTime);
                        //The AccountInfo to which user belongs is offline
                        AccountInfo accountInfo = m_AccountSystem.FindAccountById(user.AccountId);
                        if (accountInfo != null) {
                            m_AccountSystem.RemoveAccountById(accountInfo.AccountId);
                            LogSys.Log(ServerLogType.INFO, "UserProcessScheduler-OnUserTick-AccountOfflineWithUser. AccountId:{0}, UserGuid:{1}",
                                                      accountInfo.AccountId, user.Guid);
                        }
                        FreeKey(user.Key);
                        ulong g = 0;
                        m_GuidByNickname.TryRemove(user.Nickname, out g);
                        UserInfo tmp;
                        m_UserInfos.TryRemove(guid, out tmp);
                        FreeUserThread(user);
                        RecycleUserInfo(user);
                        m_DeactiveUserGuids.Add(guid);
                    }
                }
            }
            foreach (ulong guid in m_DeactiveUserGuids) {
                m_WaitRecycleUsers.Remove(guid);
            }
        }
        private void ActivateUserGuid(ulong guid)
        {
            m_ActiveUserGuids.AddOrUpdate(guid, 1, (g, v) => 1);
        }
        private void AddWaitRecycleUser(ulong guid)
        {
            if (!m_WaitRecycleUsers.Contains(guid)) {
                m_WaitRecycleUsers.Add(guid);
            }
        }
        private uint GenerateKey()
        {
            uint key = 0;
            for (; ; ) {
                key = (uint)(m_Random.NextDouble() * 0x0fffffff);
                if (!m_Keys.ContainsKey(key)) {
                    m_Keys.AddOrUpdate(key, true, (k, v) => true);
                    break;
                }
            }
            return key;
        }
        private void FreeKey(uint key)
        {
            bool nouse = false;
            m_Keys.TryRemove(key, out nouse);
        }
        
        private NodeMessageManager m_NodeMessageManager = new NodeMessageManager();

        private AccountSystem m_AccountSystem = new AccountSystem();
        private Dictionary<string, long> m_KickedAccountDict = new Dictionary<string, long>();
        private ConcurrentDictionary<ulong, UserInfo> m_UserInfos = new ConcurrentDictionary<ulong, UserInfo>();
        private ConcurrentDictionary<string, ulong> m_GuidByNickname = new ConcurrentDictionary<string, ulong>();
        private ConcurrentQueue<UserInfo> m_UnusedUserInfos = new ConcurrentQueue<UserInfo>();
        private ConcurrentDictionary<uint, bool> m_Keys = new ConcurrentDictionary<uint, bool>();

        private ConcurrentDictionary<ulong, int> m_ActiveUserGuids = new ConcurrentDictionary<ulong, int>();
        private List<ulong> m_DeactiveUserGuids = new List<ulong>();
        private List<ulong> m_WaitRecycleUsers = new List<ulong>();

        private Random m_Random = new Random();
        private const int c_UserSaveRandom = 15000;   //15s, random perturbation of UserInfo storage time interval, LobbyConfig.UserDSSaveInterval +- 15s
        private MyServerThread m_Thread = null;

        //UserThreadManager
        private const int c_MaxUserThreadNum = 12;
        private const int c_MaxUserCountPerUserThread = 200;
        private UserThread m_DefaultThread = null;
        private UserThread[] m_UserThreads = new UserThread[c_MaxUserThreadNum];
        private int[] m_UserThreadUserCounts = new int[c_MaxUserThreadNum];

        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;
        private DateTime m_BaseNormLogTime = new DateTime(2015, 1, 1, 0, 0, 0);
        private bool m_IsLastSave = false;          //Is the last storage of player data being executed?
        private bool m_LastSaveFinished = false;    //Player data final storage completion status

        //The global data system is implemented in UserProcessScheduler because it is closely related to
        //the registration and login process.
        private NicknameSystem m_NicknameSystem = new NicknameSystem();
    }
}