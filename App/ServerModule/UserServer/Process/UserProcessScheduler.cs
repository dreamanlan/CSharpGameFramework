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
    /// 玩家数据处理调度器，玩家数据请求将被放到并行的若干个线程里进行处理。
    /// 有2类线程：
    /// 1、由GetUserThread().QueueAction或DefaultUserThread.QueueAction调用发起的操作，此时执行线程无法指定。
    /// 2、调度器内部实例化一个线程，用以进行必须在一个线程里进行的操作。（未对外提供接口，目前假定用于为1中操作需要有序的服务。）
    /// </summary>
    /// <remarks>
    /// 这个类采用多线程操作数据，每个UserInfo在进游戏后到离线期间，相应操作都在一个线程处理，但由于全局线程与内部线程也会用到用户数据，仍然要考虑多线程问题。
    /// 请注意几条约定：
    /// 1、UserInfo一旦实例化，内存不会被释放（只回收到池子里供重用，RoomInfo也是这样）。
    /// 2、对于只操作小于等于机器字长的数据的函数，不加锁（操作本来就是原子的）。
    /// 3、在全局线程操作用户数据时，需要将操作排队到用户线程处理后再返回全局线程处理（会麻烦一些，但这样能避免加锁）。
    /// 4、UserInfo中的集合数据在读写时要考虑所有访问这些数据的地方的并发情况，需要加锁的加锁。
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
        internal void DispatchJsonMessage(bool isGmTool, uint seq, int sourceHandle, int destHandle, byte[] data)
        {
            m_NodeMessageManager.DispatchMessage(isGmTool, seq, sourceHandle, destHandle, data);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //供外部直接调用的方法，需要保证多线程安全。
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
        //当前在线玩家中根据Nickname查找UserGuid
        internal ulong GetGuidByNickname(string nickname)
        {
            ulong guid = 0;
            m_GuidByNickname.TryGetValue(nickname, out guid);
            return guid;
        }
        //所有玩家根据Nickname查找UserGuid
        internal ulong FindUserGuidByNickname(string nickname)
        {
            return m_NicknameSystem.FindUserGuidByNickname(nickname);
        }
        //所有玩家根据Nickname模糊查找UserGuid
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
                LogSys.Log(LOG_TYPE.ERROR, "GetUserInfo ERROR. UserGuid:{0}", guid);
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
            //服务器关闭前的最后一次存储操作
            var ds_thread = UserServer.Instance.DataCacheThread;
            if (ds_thread.DataStoreAvailable == true) {
                //1.通知客户端服务器关闭        
                foreach (var guidPair in m_ActiveUserGuids) {
                    UserInfo user = GetUserInfo(guidPair.Key);
                    if (user != null) {
                        NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.ServerShutdown, user.Guid);
                        NodeMessageDispatcher.SendNodeMessage(user.NodeName, retMsg);
                    }
                }
                LogSys.Log(LOG_TYPE.MONITOR, "DoLastSaveUserData Step_1: Notice game client ServerShutdown. UserCount:{0}", m_ActiveUserGuids.Count);
                //2.等待10s
                Thread.Sleep(10000);
                LogSys.Log(LOG_TYPE.MONITOR, "DoLastSaveUserData Step_2: Wait for 10s.");
                //3.关闭Node
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
                LogSys.Log(LOG_TYPE.MONITOR, "DoLastSaveUserData Step_3: Close Servers.");
                //4.保存玩家数据
                foreach (var guidPair in m_ActiveUserGuids) {
                    UserInfo user = GetUserInfo(guidPair.Key);
                    if (user != null) {
                        user.NextUserSaveCount = DataCacheThread.UltimateSaveCount;
                        this.GetUserThread(user).QueueAction(ds_thread.DSPSaveUser, user, user.NextUserSaveCount);
                    }
                }
                LogSys.Log(LOG_TYPE.MONITOR, "DoLastSaveUserData Step_4: Start to save UserData for last. UserCount:{0}", m_ActiveUserGuids.Count);
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
        //--------------------------------------------------------------------------------------------------------------------------
        //供GM消息调用的方法，实际执行线程是在线程池的某个线程里执行，实现时需要注意并发问题，需要加锁的加锁。
        //逻辑功能不要直接调用此类处理（会有gm权限判断，正式服会导致功能失效），应该调用逻辑上一个功能相同的方法（通常以Do开头命名）。
        //--------------------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------
        //供外部通过QueueAction调用的方法，实际执行线程是在线程池的某个线程里执行，实现时需要注意并发问题，需要加锁的加锁。
        //--------------------------------------------------------------------------------------------------------------------------
        //登录流程相关方法
        internal void DoAccountLogin(string accountId, string password, string clientInfo, string nodeName)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            QueueingThread queueingThread = UserServer.Instance.QueueingThread;
            if (null != accountInfo || !queueingThread.NeedQueueing()) {
                LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_3a: DoLogin without queueing. AccountId:{0}", accountId);
                DoAccountLoginWithoutQueueing(accountId, password, clientInfo, nodeName);
            } else {
                if (queueingThread.IsQueueingFull()) {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "LoginStep_3b1: Start queueing but queue full. AccountId:{0}", accountId);

                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.QueueFull;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                } else {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "LoginStep_3b2: Start queueing. AccountId:{0}", accountId);

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
                //当前accountId不在线
                accountInfo = new AccountInfo();
                accountInfo.AccountId = accountId;
                accountInfo.Password = password;
                accountInfo.ClientInfo = clientInfo;
                accountInfo.NodeName = nodeName;
                accountInfo.CurrentState = AccountState.Unloaded;
                accountInfo.UserGuid = UserServer.Instance.GlobalProcessThread.GenerateUserGuid();
                var dsThread = UserServer.Instance.DataCacheThread;
                if (dsThread.DataStoreAvailable == true) {
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_4: Load account from DataStore . AccountId:{0}", accountId);
                    dsThread.DispatchAction(dsThread.DSPLoadAccount, accountId);
                } else {
                    accountInfo.CurrentState = AccountState.Online;
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    LogSys.Log(LOG_TYPE.INFO, "Account login success. Account:{0}", accountId);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.FirstLogin;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                }
            } else {
                //当前账号在线
                if (accountInfo.CurrentState == AccountState.Dropped || clientInfo == accountInfo.ClientInfo) {
                    //账号处在离线状态或同一设备重复登录，登录成功
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_5a: Account is dropped. Login SUCCESS. AccountId:{0}", accountId);
                    accountInfo.AccountId = accountId;
                    accountInfo.Password = password;
                    accountInfo.ClientInfo = clientInfo;
                    accountInfo.NodeName = nodeName;
                    accountInfo.CurrentState = AccountState.Online;
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    if (null != GetUserInfo(accountInfo.UserGuid))
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Success;
                    else
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.FirstLogin;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                } else {
                    //账号在别的设备上登录，登录失败
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "LoginStep_5b: Account is online. Login FAILED. AccountId:{0}", accountId);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.AccountLoginResult, accountId);
                    GameFrameworkMessage.AccountLoginResult protoMsg = new GameFrameworkMessage.AccountLoginResult();
                    protoMsg.m_AccountId = accountId;
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.AlreadyOnline;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(nodeName, replyMsg);
                }
            }
        }
        internal void DSPLoadAccountCallback(string accountId, Msg_DL_LoadResult ret)
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
                                    accountInfo.TableAccount.FromProto(_msg as TableAccount);
                                    break;
                                default:
                                    LogSys.Log(LOG_TYPE.ERROR, ConsoleColor.Red, "Decode account data ERROR. Wrong message id. Account:{0}, WrongId:{1}", result.PrimaryKeys[0], msgEnum);
                                    break;
                            }
                        }
                    }
                    if (accountInfo.TableAccount.IsBanned) {
                        //账号被封停
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Banned;
                    } else {
                        accountInfo.CurrentState = AccountState.Online;
                        accountInfo.LastLoginTime = TimeUtility.CurTimestamp;
                        m_AccountSystem.AddAccountById(accountId, accountInfo);
                        LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_4a: Account login SUCCESS . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                        protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Success;
                        protoMsg.m_UserGuid = accountInfo.UserGuid;                        
                    }
                } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                    //账号首次进入游戏
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_4b: Load account NotFound . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                    accountInfo.CurrentState = AccountState.Online;
                    accountInfo.TableAccount.AccountId = accountId;
                    accountInfo.TableAccount.IsBanned = false;
                    m_AccountSystem.AddAccountById(accountId, accountInfo);
                    protoMsg.m_Result = AccountLoginResult.AccountLoginResultEnum.Success;
                    protoMsg.m_UserGuid = accountInfo.UserGuid;
                    //norm log    
                    accountInfo.LastLoginTime = TimeUtility.CurTimestamp;
                } else {
                    //数据加载失败       
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "LoginStep_4c: Load account FAILED . AccountId:{0}, LogicServerId:{1}, AccountId:{2}", accountId, 0, accountId);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Red, "ERROR LoginStep_4d. AccountId:{0}, LogicServerId:{1}, AccountId:{2}\nERROR Message:{3}\nStackTrace:{4}",
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
                //检查当前账号是否有角色在线          
                bool isAccountKickable = true;
                UserInfo ui = GetUserInfo(accountInfo.UserGuid);
                if (ui != null) {
                    isAccountKickable = false;
                }
                if (isAccountKickable) {
                    //踢掉账号
                    accountInfo.CurrentState = AccountState.Offline;
                    m_AccountSystem.RemoveAccountById(accountInfo.AccountId);
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LogoutStep: Account LOGOUT directly. AccountId:{0}",
                      accountInfo.AccountId);
                } else {
                    //AccountInfo设置为离线状态
                    accountInfo.CurrentState = AccountState.Dropped;
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LogoutStep: Account dropped. AccountId:{0}",
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
                        //用户处在下线过程中，需要等待lobby离线流程完成
                        NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountId, user.Guid);
                        GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                        protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.Wait;
                        roleEnterResultMsg.m_ProtoData = protoData;
                        NodeMessageDispatcher.SendNodeMessage(user.NodeName, roleEnterResultMsg);
                        LogSys.Log(LOG_TYPE.WARN, "RoleEnter AccountId:{0} Guid:{1} Wait Offline", accountId, userGuid);
                    } else {
                        if (user.AccountId.Equals(accountInfo.AccountId)) {         
                            user.NodeName = accountInfo.NodeName;
                            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
                            DoUserRelogin(user);
                            LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_8a: Role Reenter SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}",
                                accountId, userGuid, user.Nickname);
                            //回复客户端
                            NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, user.AccountId, user.Guid);
                            GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(user);
                            protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.Success;
                            roleEnterResultMsg.m_ProtoData = protoData;
                            NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, roleEnterResultMsg);
                        } else {
                            //角色AccountId与账号AccountId不匹配,进入游戏失败
                            NodeMessage roleEnterResultMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountId, user.Guid);
                            GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                            protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.UnknownError;
                            roleEnterResultMsg.m_ProtoData = protoData;
                            LogSys.Log(LOG_TYPE.ERROR, "LoginStep_8a: Role Reenter FAILED. AccountId:{0}, UserGuid:{1}, Nickname:{2}",
                                accountId, userGuid, user.Nickname);
                        }
                    }
                } else {
                    var ds_thread = UserServer.Instance.DataCacheThread;
                    if (ds_thread.DataStoreAvailable == true) {
                        //注意这里的回调在DataCacheThread里执行
                        LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_8b: Load UserInfo. AccountId:{0}, UserGuid:{1}", accountId, userGuid);
                        ds_thread.DispatchAction(ds_thread.DSPLoadUser, userGuid, accountId);
                    } else {
                        CreateRole(accountId, nickname, 1);
                    }
                }
            }
        }
        internal void DSPLoadUserCallback(Msg_DL_LoadResult ret, string accountId)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            if (accountInfo == null) {
                return;
            }
            ulong userGuid = 0;// accountInfo.UserGuid;
            if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
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
                #region 由数据库数据构建UserInfo
                UserInfo ui = NewUserInfo();
                //基础数据
                ui.FromProto(dataUser);
                ui.SetMoneyForDB(dataUser.Money);
                ui.SetGoldForDB(dataUser.Gold);
                #endregion
                ui.NodeName = accountInfo.NodeName;
                this.DoUserLogin(ui);
                NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, ui.AccountId, ui.Guid);
                GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(ui);
                protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.Success;
                replyMsg.m_ProtoData = protoData;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            } else {
                NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, accountInfo.AccountId, userGuid);
                GameFrameworkMessage.RoleEnterResult protoData = new GameFrameworkMessage.RoleEnterResult();
                protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.UnknownError;
                replyMsg.m_ProtoData = protoData;
                NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
            }
            LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler-OnRoleEnter-EndLoadUser. AccountId:{0}, UserGuid:{1}, Result:{2}",
              accountId, userGuid, ret.ErrorNo);
        }
        internal void DoChangeName(ulong guid, string nickname)
        {
            UserInfo ui = GetUserInfo(guid);
            if (null != ui) {
                NicknameSystem.CheckNicknameResult ret = m_NicknameSystem.CheckNickname(ui.AccountId, nickname);
                if (ret == NicknameSystem.CheckNicknameResult.Success) {
                    ui.Nickname = nickname;
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "ChangeName SUCCESS. AccountId:{0}, Nickname:{1}",
                      ui.AccountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, ui.AccountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.Success;
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(ui.NodeName, replyMsg);
                } else {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "ChangeName FAILED. AccountId:{0}, Nickname:{1}",
                      ui.AccountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, ui.AccountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    if (ret == NicknameSystem.CheckNicknameResult.AlreadyUsed) {
                        //昵称已经被使用
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.NicknameError;
                    } else {
                        //昵称不合法
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.UnknownError;
                    }
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(ui.NodeName, replyMsg);
                }
            }
        }
        internal void DoUserLogin(UserInfo user)
        {
            user.FightingCapacity = 0;
            user.IsDisconnected = false;
            user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
            user.LastSaveTime = TimeUtility.GetLocalMilliseconds();
            //在这里生成游戏中要用的key
            if (user.Key == 0)
                user.Key = GenerateKey();
            user.CurrentState = UserState.Online;
            AllocUserThread(user);
            UserInfo oldInfo;
            if (m_UserInfos.TryGetValue(user.Guid, out oldInfo)) {
                if (oldInfo != user) {
                    //这里需要回收旧玩家数据（复位状态等），保证引用旧数据的地方能正确处理。
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
                if (user.CurrentState == UserState.Room || user.CurrentState == UserState.DropOrOffline || forceLogoff) {//仅心跳超时(5分钟)或已经是离线状态才走离线流程
                    QueueingThread queueingThread = UserServer.Instance.QueueingThread;

                    double onlinetimes = TimeUtility.CurTimestamp - user.LastLoginTime;
                    user.LastLogoutTime = DateTime.Now;
                    var ds_thread = UserServer.Instance.DataCacheThread;
                    if (ds_thread.DataStoreAvailable == true) {
                        user.NextUserSaveCount = DataCacheThread.UltimateSaveCount;
                        this.GetUserThread(user.Guid).QueueAction(ds_thread.DSPSaveUser, user, user.NextUserSaveCount);
                        LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler-DoUserLogoff-StartSaveUser. UserGuid:{0}", guid);
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
                LogSys.Log(LOG_TYPE.DEBUG, "DoUserHeartbeat,guid:{0} can't found.", guid);
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
        //这些方法是一些工具方法，后面需要重新考虑并发相关的处理。
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
            //分配到当前人数最少的线程, 均匀分布
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
            ui.FightingCapacity = 200;
            //加4个队员
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
            replyMsg.m_Nickname = ui.Nickname;
            replyMsg.m_HeroId = ui.HeroId;
            replyMsg.m_Level = ui.Level;
            replyMsg.m_Money = ui.Money;
            replyMsg.m_Gold = ui.Gold;
            replyMsg.m_Level = ui.Level;
            replyMsg.m_SceneId = ui.SceneId;
            replyMsg.m_WorldId = UserServerConfig.WorldId;
            return replyMsg;
        }
        private void CreateRole(string accountId, string nickname, int heroId)
        {
            AccountInfo accountInfo = m_AccountSystem.FindAccountById(accountId);
            var ds_thread = UserServer.Instance.DataCacheThread;
            if (accountInfo != null) {
                //检查昵称是否可用
                NicknameSystem.CheckNicknameResult ret = m_NicknameSystem.CheckNickname(accountId, nickname);
                if (ret == NicknameSystem.CheckNicknameResult.Success) {
                    UserInfo ui = NewUserInfo();
                    ui.Guid = accountInfo.UserGuid;
                    ui.AccountId = accountInfo.AccountId;
                    ui.Nickname = nickname;
                    ui.HeroId = heroId;
                    InitUserinfo(ui);
                    m_NicknameSystem.UpdateUsedNickname(nickname, ui.Guid);
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_7a: Create new role SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}, HeroId:{3}",
                      accountId, accountInfo.AccountId, ui.Guid, ui.Nickname, ui.HeroId);
                    if (ds_thread.DataStoreAvailable) {
                        ds_thread.DSPSaveCreateUser(accountInfo, nickname, ui.Guid);
                        ds_thread.DSPSaveUser(ui, ui.NextUserSaveCount);
                    }
                    //游戏角色创建成功，直接进入游戏
                    ui.NodeName = accountInfo.NodeName;
                    this.DoUserLogin(ui);
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Green, "LoginStep_8c: New role enter SUCCESS. AccountId:{0}, UserGuid:{1}, Nickname:{2}",
                      accountId, ui.Guid, ui.Nickname);
                    NodeMessage enterMsg = new NodeMessage(LobbyMessageDefine.RoleEnterResult, ui.AccountId, ui.Guid);
                    GameFrameworkMessage.RoleEnterResult protoData = CreateRoleEnterResultMsg(ui);
                    protoData.m_Result = RoleEnterResult.RoleEnterResultEnum.Success;
                    enterMsg.m_ProtoData = protoData;
                    NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, enterMsg);
                } else {
                    LogSys.Log(LOG_TYPE.INFO, ConsoleColor.Yellow, "LoginStep_7b: Create new role FAILED. AccountId:{0}, Nickname:{1}",
                      accountId, nickname);
                    NodeMessage replyMsg = new NodeMessage(LobbyMessageDefine.ChangeNameResult, accountId);
                    GameFrameworkMessage.ChangeNameResult protoMsg = new GameFrameworkMessage.ChangeNameResult();
                    if (ret == NicknameSystem.CheckNicknameResult.AlreadyUsed) {
                        //昵称已经被使用
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.NicknameError;
                    } else {
                        //昵称不合法
                        protoMsg.m_Result = ChangeNameResult.ChangeNameResultEnum.UnknownError;
                    }
                    replyMsg.m_ProtoData = protoMsg;
                    NodeMessageDispatcher.SendNodeMessage(accountInfo.NodeName, replyMsg);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //后面的方法都是在内部线程执行的方法，不涉及多线程操作，不用加锁，串行执行。
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
                        LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler.DispatchActionQueue {0} {1}", threadIx, msg);
                    });
                    LogSys.Log(LOG_TYPE.MONITOR, "UserProcessScheduler.DispatchActionQueue Current Action {0} {1}", threadIx, thread.CurActionNum);
                }
                for (int threadIx = 0; threadIx < m_UserThreadUserCounts.Length; ++threadIx) {
                    LogSys.Log(LOG_TYPE.MONITOR, "UserProcessScheduler.UserThread User Count {0} {1}", threadIx, m_UserThreadUserCounts[threadIx]);
                }
                m_Thread.DebugPoolCount((string msg) => {
                    LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler.ThreadActionQueue {0}", msg);
                });
                LogSys.Log(LOG_TYPE.MONITOR, "UserProcessScheduler.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);

                m_NodeMessageManager.TickMonitor();

                LogSys.Log(LOG_TYPE.MONITOR, "GameFramework User Count:{0} ElapsedTickTime:{1}", m_ActiveUserGuids.Count, elapsedTickTime);
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
                    //定期存储UserInfo
                    if (ds_thread.DataStoreAvailable) {
                        if (user.CurrentState != UserState.DropOrOffline && curTime - user.LastSaveTime > UserServerConfig.UserDSSaveInterval && user.NextUserSaveCount > 0) {
                            this.GetUserThread(user.Guid).QueueAction(ds_thread.DSPSaveUser, user, user.NextUserSaveCount);
                            user.NextUserSaveCount++;
                            //随机扰动
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
                            LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler-OnUserTick-UserLogout. UserGuid:{0}", user.Guid);
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
                    LogSys.Log(LOG_TYPE.MONITOR, "DoLastSaveUserData Step_5: Save UserData done. UserCount:{0}", userSaveDoneCount);
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
                        //user所属的AccountInfo下线
                        AccountInfo accountInfo = m_AccountSystem.FindAccountById(user.AccountId);
                        if (accountInfo != null) {
                            m_AccountSystem.RemoveAccountById(accountInfo.AccountId);
                            LogSys.Log(LOG_TYPE.INFO, "UserProcessScheduler-OnUserTick-AccountOfflineWithUser. AccountId:{0}, UserGuid:{1}",
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
        private const int c_UserSaveRandom = 15000;   //15s,随机扰动UserInfo的存储时间间隔,LobbyConfig.UserDSSaveInterval +- 15s 
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
        private bool m_IsLastSave = false;          //玩家数据最后存储是否正在执行
        private bool m_LastSaveFinished = false;    //玩家数据最后存储完成状态    

        //全局数据系统，因为与注册登录流程密切相关，故在UserProcessScheduler中实现
        private NicknameSystem m_NicknameSystem = new NicknameSystem();
    }
}