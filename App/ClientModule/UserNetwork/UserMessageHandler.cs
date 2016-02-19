using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GameFramework;
using GameFramework.Story;
using GameFrameworkMessage;
using LitJson;

namespace GameFramework.Network
{
    public enum MpveAwardResult
    {
        Succeed = 0,
        Gained = 1,
        Nothing = 2,
        Failure = 3,
    }
    internal enum ItemLogicId
    {
        ItemLogicStart = 1,         //道具逻辑ID基数
        ItemLogic_Exchange = 2,     //兑换逻辑
        ItemLogic_GiftPackage = 3,  //礼包
        ItemLogic_Title = 4,        //称号
        ItemLogic_Drop = 5,         //掉落
        ItemLogic_VipCard = 6,      //vip卡
        MaxNum,
    }
    ///
    internal sealed partial class UserNetworkSystem
    {
        private void LobbyMessageInit()
        {
            Utility.EventSystem.Subscribe<string>("ge_select_server", "lobby", SelectServer);
            Utility.EventSystem.Subscribe<string, string, string>("ge_account_login", "lobby", AccountLoginLobby);
            Utility.EventSystem.Subscribe("ge_stop_login", "lobby", StopLoginLobby);
            Utility.EventSystem.Subscribe<string>("ge_activate_account", "lobby", ActivateAccount);
            Utility.EventSystem.Subscribe("ge_request_nickname", "lobby", RequestNickname);
            Utility.EventSystem.Subscribe<string>("ge_change_name", "lobby", ChangeName);
            Utility.EventSystem.Subscribe<int, int>("ge_enter_scene", "lobby", EnterScene);
            Utility.EventSystem.Subscribe<int, int>("ge_change_room", "lobby", ChangeRoom);
            Utility.EventSystem.Subscribe("ge_request_roominfo", "lobby", RequestRoomInfo);
            Utility.EventSystem.Subscribe("ge_request_roomlist", "lobby", RequestRoomList);
            LobbyMessageHandler();
        }
        private void LobbyMessageHandler()
        {
            RegisterMsgHandler(LobbyMessageDefine.QueueingCountResult, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.QueueingCountResult), HandleQueueingCountResult);
            RegisterMsgHandler(LobbyMessageDefine.UserHeartbeat, typeof(GameFrameworkMessage.NodeMessageWithGuid), HandleUserHeartbeat);
            RegisterMsgHandler(LobbyMessageDefine.TooManyOperations, typeof(GameFrameworkMessage.NodeMessageWithGuid), HandleTooManyOperations);
            RegisterMsgHandler(LobbyMessageDefine.VersionVerifyResult, null, typeof(GameFrameworkMessage.VersionVerifyResult), HandleVersionVerifyResult);
            RegisterMsgHandler(LobbyMessageDefine.AccountLoginResult, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.AccountLoginResult), HandleAccountLoginResult);
            RegisterMsgHandler(LobbyMessageDefine.ActivateAccountResult, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.ActivateAccountResult), HandleActivateAccountResult);
            RegisterMsgHandler(LobbyMessageDefine.RequestNicknameResult, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.RequestNicknameResult), HandleRequestNicknameResult);
            RegisterMsgHandler(LobbyMessageDefine.ChangeNameResult, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.ChangeNameResult), HandleChangeNameResult);
            RegisterMsgHandler(LobbyMessageDefine.RoleEnterResult, typeof(GameFrameworkMessage.NodeMessageWithAccountAndGuid), typeof(GameFrameworkMessage.RoleEnterResult), HandleRoleEnterResult);
            RegisterMsgHandler(LobbyMessageDefine.EnterSceneResult, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.EnterSceneResult), HandleEnterSceneResult);
            RegisterMsgHandler(LobbyMessageDefine.ServerShutdown, typeof(GameFrameworkMessage.NodeMessageWithGuid), HandleServerShutdown);
            RegisterMsgHandler(LobbyMessageDefine.GmCode, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.GmCode), HandleGmCode);
            RegisterMsgHandler(LobbyMessageDefine.Msg_CLC_StoryMessage, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CLC_StoryMessage), HandleStoryMessage);
        }

        private void SelectServer(string serverAddress)
        {
            try {
                SetUrl(serverAddress);
            } catch (Exception ex) {
                LogSystem.Error("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void AccountLoginLobby(string account, string passwd, string clientInfo)
        {
            try {
                m_AccountId = account;
                m_Password = passwd;
                m_ClientInfo = clientInfo;

                m_IsWaitStart = false;
                m_HasLoggedOn = false;

                if (!IsConnected) {
                    ConnectIfNotOpen();
                } else {
                    NodeMessage loginMsg = new NodeMessage(LobbyMessageDefine.AccountLogin, m_AccountId);
                    GameFrameworkMessage.AccountLogin protoMsg = new GameFrameworkMessage.AccountLogin();
                    loginMsg.m_ProtoData = protoMsg;
                    protoMsg.m_AccountId = account;
                    protoMsg.m_Password = passwd;
                    protoMsg.m_ClientInfo = clientInfo;
                    
                    if (null != m_AccountId) {
                        SendMessage(loginMsg);
                    }
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void StopLoginLobby()
        {
            try {
                //账号登录失败，取消自动登录，断开网络连接
                m_IsWaitStart = true;
                m_HasLoggedOn = false;
                if (IsConnected) {
                    Disconnect(true);
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void RequestNickname()
        {
            try {
                NodeMessage sendMsg = new NodeMessage(LobbyMessageDefine.RequestNickname, m_AccountId);
                SendMessage(sendMsg);
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void ChangeName(string nickname)
        {
            if (m_IsLogining) {
                RoleEnter(m_AccountId, nickname);
            } else {
                try {
                    NodeMessage sendMsg = new NodeMessage(LobbyMessageDefine.ChangeName, m_Guid);
                    GameFrameworkMessage.ChangeName protoMsg = new GameFrameworkMessage.ChangeName();
                    sendMsg.m_ProtoData = protoMsg;
                    protoMsg.m_Nickname = nickname;
                    SendMessage(sendMsg);
                } catch (Exception ex) {
                    LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
        private void RoleEnter(string accountId, string nickname)
        {
            try {
                NodeMessage msg = new NodeMessage(LobbyMessageDefine.RoleEnter, accountId);
                GameFrameworkMessage.RoleEnter protoData = new GameFrameworkMessage.RoleEnter();
                protoData.m_Nickname = nickname;
                msg.m_ProtoData = protoData;
                SendMessage(msg);
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void ActivateAccount(string activationCode)
        {
            try {
                NodeMessage sendMsg = new NodeMessage(LobbyMessageDefine.ActivateAccount, m_AccountId);
                GameFrameworkMessage.ActivateAccount protoMsg = new GameFrameworkMessage.ActivateAccount();
                sendMsg.m_ProtoData = protoMsg;

                protoMsg.m_ActivationCode = activationCode;
                SendMessage(sendMsg);
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////
        private void HandleTooManyOperations(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.TooManyOperations tooManyOperations = lobbyMsg.m_ProtoData as GameFrameworkMessage.TooManyOperations;
            if (null != tooManyOperations) {
                ClientModule.Instance.HighlightPrompt("Tip_TooManyOperations");
            }
        }
        private void HandleVersionVerifyResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.VersionVerifyResult verifyResult = lobbyMsg.m_ProtoData as GameFrameworkMessage.VersionVerifyResult;
            if (null == verifyResult)
                return;
            int ret = verifyResult.m_Result;
            int enableLog = verifyResult.m_EnableLog;
            //GlobalVariables.Instance.ShopMask = verifyResult.m_ShopMask;
            if (0 == ret) {
                //版本校验失败，提示用户需要更新版本。
                StopLoginLobby();
                GfxStorySystem.Instance.SendMessage("version_verify_failed");
            } else {
                if (enableLog == 0) {
                    GameControler.FileLogger.Enabled = false;
                } else {
                    GameControler.FileLogger.Enabled = true;
                }
                //向服务器发送登录消息
                NodeMessage loginMsg = new NodeMessage(LobbyMessageDefine.AccountLogin, m_AccountId);
                GameFrameworkMessage.AccountLogin protoMsg = new GameFrameworkMessage.AccountLogin();
                loginMsg.m_ProtoData = protoMsg;
                protoMsg.m_AccountId = m_AccountId;
                protoMsg.m_Password = m_Password;
                protoMsg.m_ClientInfo = m_ClientInfo;

                if (null != m_AccountId) {
                    SendMessage(loginMsg);
                }
            }
        }
        private void HandleAccountLoginResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.AccountLoginResult protoData = lobbyMsg.m_ProtoData as GameFrameworkMessage.AccountLoginResult;
            if (null == protoData)
                return;
            GameFrameworkMessage.AccountLoginResult.AccountLoginResultEnum ret = protoData.m_Result;
            ulong userGuid = protoData.m_UserGuid;
            UserNetworkSystem.Instance.IsQueueing = false;
            if (m_HasLoggedOn) {//重连处理
                if (ret == AccountLoginResult.AccountLoginResultEnum.Success) {
                    //登录成功，向服务器请求玩家角色
                    m_Guid = userGuid;
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.RoleEnter, m_AccountId);
                    GameFrameworkMessage.RoleEnter protoMsg = new GameFrameworkMessage.RoleEnter();
                    msg.m_ProtoData = protoMsg;
                    SendMessage(msg);
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.Queueing ||
                  ret == AccountLoginResult.AccountLoginResultEnum.QueueFull ||
                  ret == AccountLoginResult.AccountLoginResultEnum.Wait) {
                    Disconnect();
                } else {
                    //ClientModule.Instance.ReturnToLogin();
                }
            } else {//首次登录处理
                if (ret == AccountLoginResult.AccountLoginResultEnum.Success) {
                    m_Guid = userGuid;
                    //登录成功，向服务器请求玩家角色          
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.RoleEnter, m_AccountId);
                    GameFrameworkMessage.RoleEnter protoMsg = new GameFrameworkMessage.RoleEnter();
                    protoMsg.m_Nickname = string.Empty;
                    msg.m_ProtoData = protoMsg;
                    SendMessage(msg);
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.FirstLogin) {
                    //账号首次登录，需要指定昵称
                    m_Guid = userGuid;
                    RequestNickname();
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.Wait) {
                    //同时登录人太多，需要等待一段时间后再登录
                    ClientModule.Instance.HighlightPrompt("Tip_TooManyPeople");
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.Banned) {
                    //账号已被封停，禁止登录
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.Queueing) {
                    ClientModule.Instance.HighlightPrompt("Tip_Queueing");
                    UserNetworkSystem.Instance.IsQueueing = true;
                    UserNetworkSystem.Instance.QueueingNum = -1;
                } else if (ret == AccountLoginResult.AccountLoginResultEnum.QueueFull) {
                    //排队满
                    StopLoginLobby();
                    ClientModule.Instance.HighlightPrompt("Tip_QueueFull");
                } else {
                    //账号登录失败
                }
            }
        }
        private void HandleActivateAccountResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.ActivateAccountResult protoMsg = lobbyMsg.m_ProtoData as GameFrameworkMessage.ActivateAccountResult;
            if (null == protoMsg)
                return;
            ActivateAccountResult.ActivateAccountResultEnum ret = protoMsg.m_Result;
            if (ret == ActivateAccountResult.ActivateAccountResultEnum.Success) {
                //激活成功，账号成功登陆
            } else {
                //激活失败
            }
        }
        private void HandleRequestNicknameResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.RequestNicknameResult protoMsg = lobbyMsg.m_ProtoData as GameFrameworkMessage.RequestNicknameResult;
            if (null == protoMsg)
                return;
            GfxStorySystem.Instance.SendMessage("show_nickname", protoMsg.m_Nicknames);
        }
        private void HandleChangeNameResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.ChangeNameResult protoMsg = lobbyMsg.m_ProtoData as GameFrameworkMessage.ChangeNameResult;
            if (null == protoMsg)
                return;
            ChangeNameResult.ChangeNameResultEnum ret = protoMsg.m_Result;
            if (ret == ChangeNameResult.ChangeNameResultEnum.Success) {
                RoleInfo player = new RoleInfo();
                player.Guid = m_Guid;
                player.Nickname = protoMsg.m_Nickname;
                ClientInfo.Instance.CurrentRole = player;
            }
        }
        private void HandleRoleEnterResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.NodeMessageWithAccountAndGuid headerData = lobbyMsg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithAccountAndGuid;
            GameFrameworkMessage.RoleEnterResult protoData = lobbyMsg.m_ProtoData as GameFrameworkMessage.RoleEnterResult;
            if (null != protoData) {
                RoleEnterResult.RoleEnterResultEnum ret = protoData.m_Result;
                m_Guid = headerData.m_Guid;
                RoleInfo role = new RoleInfo();
                role.Guid = m_Guid;

                if (ret == RoleEnterResult.RoleEnterResultEnum.Wait) {
                    Thread.Sleep(2000);
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.RoleEnter, m_AccountId);
                    GameFrameworkMessage.RoleEnter data = new GameFrameworkMessage.RoleEnter();
                    msg.m_ProtoData = data;
                    SendMessage(msg);
                    LogSystem.Info("Retry RoleEnter {0} {1}", m_AccountId, m_Guid);
                    return;
                } else if (ret == RoleEnterResult.RoleEnterResultEnum.Success) {
                    if (role != null) {
                        //客户端接收服务器传来的数据，创建玩家对象
                        m_WorldId = protoData.m_WorldId;
                        role.Nickname = protoData.m_Nickname;
                        role.HeroId = protoData.m_HeroId;
                        role.Level = protoData.m_Level;
                        ClientInfo.Instance.CurrentRole = role;
                        ClientInfo.Instance.WorldId = protoData.m_WorldId;
                        ///
                        m_IsLogining = false;
                        m_HasLoggedOn = true;
                    }
                    GfxStorySystem.Instance.SendMessage("start_game");
                } else {
                    //进入游戏失败
                }
            }
        }
        private void HandleEnterSceneResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.EnterSceneResult protoData = lobbyMsg.m_ProtoData as GameFrameworkMessage.EnterSceneResult;
            if (null != protoData) {
                GeneralOperationResult result = (GeneralOperationResult)protoData.result;
                if (GeneralOperationResult.LC_Succeed == result) {
                    uint key = protoData.key;
                    string ip = protoData.server_ip;
                    int port = (int)protoData.server_port;
                    int campId = protoData.camp_id;
                    int sceneId = protoData.scene_type;
                    ClientInfo.Instance.PropertyKey = protoData.prime;
                    //延迟处理，防止当前正在切场景过程中
                    ClientModule.Instance.QueueAction(ClientModule.Instance.TryEnterScene, key, ip, port, campId, sceneId);
                } else {
                }
            }
        }
        private void HandleServerShutdown(NodeMessage lobbyMsg)
        {

        }
        private void HandleGmCode(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.GmCode protoMsg = lobbyMsg.m_ProtoData as GameFrameworkMessage.GmCode;
            if (null != protoMsg) {
                //ClientModule.Instance.ExecCode(protoMsg.m_Content);
            }
        }
        private void HandleStoryMessage(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.Msg_CLC_StoryMessage protoMsg = lobbyMsg.m_ProtoData as GameFrameworkMessage.Msg_CLC_StoryMessage;
            if (null != protoMsg) {
                try {
                    string msgId = protoMsg.m_MsgId;
                    ArrayList args = new ArrayList();
                    for (int i = 0; i < protoMsg.m_Args.Count; i++) {
                        switch (protoMsg.m_Args[i].val_type) {
                            case LobbyArgType.NULL://null
                                args.Add(null);
                                break;
                            case LobbyArgType.INT://int
                                args.Add(int.Parse(protoMsg.m_Args[i].str_val));
                                break;
                            case LobbyArgType.FLOAT://float
                                args.Add(float.Parse(protoMsg.m_Args[i].str_val));
                                break;
                            default://string
                                args.Add(protoMsg.m_Args[i].str_val);
                                break;
                        }
                    }
                    object[] objArgs = args.ToArray();
                    GfxStorySystem.Instance.SendMessage(msgId, objArgs);
                } catch (Exception ex) {
                    LogSystem.Error("HandleStoryMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
        internal void HandleQuitRoom(NodeMessage lobbyMsg)
        {
            if (!ClientModule.Instance.IsMainUiScene) {
            }
        }
        internal void EnterScene(int sceneId, int roomId)
        {
            try {
                NodeMessage msg = new NodeMessage(LobbyMessageDefine.EnterScene);
                msg.SetHeaderWithGuid(m_Guid);
                GameFrameworkMessage.EnterScene protoData = new GameFrameworkMessage.EnterScene();
                protoData.m_SceneId = sceneId;
                protoData.m_RoomId = roomId;
                msg.m_ProtoData = protoData;
                SendMessage(msg);
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void ChangeRoom(int sceneId, int roomId)
        {
            try {
                if (ClientModule.Instance.IsMainUiScene) {
                } else {
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.ChangeSceneRoom);
                    msg.SetHeaderWithGuid(m_Guid);
                    GameFrameworkMessage.ChangeSceneRoom protoData = new GameFrameworkMessage.ChangeSceneRoom();
                    protoData.m_SceneId = sceneId;
                    protoData.m_RoomId = roomId;
                    msg.m_ProtoData = protoData;
                    SendMessage(msg);
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void RequestRoomInfo()
        {
            try {
                if (ClientModule.Instance.IsMainUiScene) {
                } else {
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.RequestSceneRoomInfo);
                    msg.SetHeaderWithGuid(m_Guid);
                    SendMessage(msg);
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void RequestRoomList()
        {
            try {
                if (ClientModule.Instance.IsMainUiScene) {
                } else {
                    NodeMessage msg = new NodeMessage(LobbyMessageDefine.RequestSceneRoomList);
                    msg.SetHeaderWithGuid(m_Guid);
                    SendMessage(msg);
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}