using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CSharpCenterClient;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    internal partial class UserServer
    {
        /// <summary>
        /// Note that message processing from node needs to be distributed to the user thread of DataProcess for processing!
        /// </summary>
        private void InstallNodeHandlers()
        {
            NodeMessageDispatcher.Init(UserServerConfig.WorldId);
            NodeMessageDispatcher.SetMessageFilter(this.FilterMessage);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.NodeRegister, typeof(GameFrameworkMessage.NodeRegister), typeof(GameFrameworkMessage.NodeRegister), HandleNodeRegister);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.AccountLogin, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.AccountLogin), HandleAccountLogin);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestNickname, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.RequestNickname), HandleRequestNickname);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RoleEnter, typeof(GameFrameworkMessage.NodeMessageWithAccount), typeof(GameFrameworkMessage.RoleEnter), HandleRoleEnter);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.UserHeartbeat, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.UserHeartbeat), HandleUserHeartbeat);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.ChangeName, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.ChangeName), HandleChangeName);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.EnterScene, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.EnterScene), HandleEnterScene);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.ChangeSceneRoom, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.ChangeSceneRoom), HandleChangeSceneRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomInfo, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.RequestSceneRoomInfo), HandleRequestSceneRoomInfo);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomList, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.RequestSceneRoomList), HandleRequestSceneRoomList);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.QuitRoom, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.QuitRoom), HandleQuitRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_GetMailList, typeof(GameFrameworkMessage.NodeMessageWithGuid), null, HandleGetMailList);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_ReceiveMail, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_ReceiveMail), HandleReceiveMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_ReadMail, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_ReadMail), HandleReadMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_DeleteMail, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_DeleteMail), HandleDeleteMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_AddFriend, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_AddFriend), HandleAddFriend);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_RemoveFriend, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_RemoveFriend), HandleRemoveFriend);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_MarkBlack, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_MarkBlack), HandleMarkBlack);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_UseItem, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_UseItem), HandleUseItem);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_DiscardItem, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CL_DiscardItem), HandleDiscardItem);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CLC_StoryMessage, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.Msg_CLC_StoryMessage), HandleStoryMessage);
            //---------------------------------------------------------------------------------------------------------------
            //The observer handles the messages sent by the big world to the client. The handle of such messages is set to 0.
            //If it is non-0, it should not be processed (the message is forged by the client).
            //---------------------------------------------------------------------------------------------------------------
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.EnterSceneResult, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.EnterSceneResult), ObserveEnterSceneResult);

            //--------------------------------------------------------------------------------------
            //GM command messages are placed together, and the processing functions are placed in LobbyServer_GmJsonHandler.cs to facilitate security checks.
            //Don't register messages after this comment! ! !
            //--------------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------------------------
        private bool FilterMessage(NodeMessage msg, ulong handle, uint seq)
        {
            bool isContinue = true;
            if (handle > 0) {
                if (msg.m_ID == 0) {
                    //These messages will not be counted.
                }
                else {
                    GameFrameworkMessage.NodeMessageWithGuid msgWithGuid = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
                    if (null != msgWithGuid) {
                        ulong guid = msgWithGuid.m_Guid;
                        isContinue = OperationMeasure.Instance.CheckOperation(msgWithGuid.m_Guid);
                        if (!isContinue) {
                            NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.TooManyOperations, guid);
                            NodeMessageDispatcher.SendNodeMessage(handle, retMsg);
                        }
                    }
                }
            }
            return isContinue;
        }
        //------------------------------------------------------------------------------------------------------
        private void ObserveEnterSceneResult(NodeMessage msg, ulong handle, uint seq)
        {
            if (handle != 0)
                return;
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                GameFrameworkMessage.EnterSceneResult protoMsg = msg.m_ProtoData as GameFrameworkMessage.EnterSceneResult;
                if (null != protoMsg) {
                    UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
                    UserInfo user = dataProcess.GetUserInfo(headerMsg.m_Guid);
                    if (user != null && protoMsg.result == 0) {
                        user.CurrentState = UserState.Room;
                        user.SceneId = protoMsg.scene_type;
                    }

                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        private void HandleNodeRegister(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeRegister nodeRegMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeRegister;
            if (null != nodeRegMsg) {
                NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.NodeRegisterResult);
                GameFrameworkMessage.NodeRegisterResult nodeRegResultMsg = new NodeRegisterResult();
                nodeRegResultMsg.m_IsOk = true;
                retMsg.m_NodeHeader = nodeRegResultMsg;
                NodeMessageDispatcher.SendNodeMessage(handle, retMsg);
            }
        }
        //------------------------------------------------------------------------------------------------------
        private void HandleAccountLogin(NodeMessage msg, ulong handle, uint seq)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            int size = stringBuilder.Capacity;
            CenterHubApi.TargetName(UserServerConfig.WorldId, handle, stringBuilder, size);
            string node_name = stringBuilder.ToString();

            GameFrameworkMessage.NodeMessageWithAccount loginMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithAccount;
            if (null != loginMsg) {
                GameFrameworkMessage.AccountLogin protoData = msg.m_ProtoData as GameFrameworkMessage.AccountLogin;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoAccountLogin, loginMsg.m_Account, protoData.m_Password, protoData.m_ClientInfo, node_name);
                }
            }
        }
        private void HandleRequestNickname(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithAccount nickMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithAccount;
            if (null != nickMsg) {
                GameFrameworkMessage.RequestNickname protoData = msg.m_ProtoData as GameFrameworkMessage.RequestNickname;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoRequestNickname, nickMsg.m_Account);
                }
            }
        }
        private void HandleRoleEnter(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithAccount enterMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithAccount;
            if (null != enterMsg) {
                GameFrameworkMessage.RoleEnter protoData = msg.m_ProtoData as GameFrameworkMessage.RoleEnter;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoRoleEnter, enterMsg.m_Account, protoData.m_Nickname);
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        private void HandleUserHeartbeat(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid heartbeatMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != heartbeatMsg) {
                if (null == m_UserProcessScheduler.GetUserInfo(heartbeatMsg.m_Guid)) {
                    NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.KickUser, heartbeatMsg.m_Guid);
                    NodeMessageDispatcher.SendNodeMessage(handle, retMsg);
                    LogSys.Log(ServerLogType.DEBUG, "HandleUserHeartbeat, guid:{0} can't found, kick.", heartbeatMsg.m_Guid);
                } else {
                    //echo
                    NodeMessageDispatcher.SendNodeMessage(handle, msg);
                    //logical processing
                    m_UserProcessScheduler.GetUserThread(heartbeatMsg.m_Guid).QueueAction(m_UserProcessScheduler.DoUserHeartbeat, heartbeatMsg.m_Guid);
                }
            }
        }
        private void HandleChangeName(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nameMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nameMsg) {
                GameFrameworkMessage.ChangeName protoData = msg.m_ProtoData as GameFrameworkMessage.ChangeName;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoChangeName, nameMsg.m_Guid, protoData.m_Nickname);
                }
            }
        }
        private void HandleEnterScene(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                GameFrameworkMessage.EnterScene protoMsg = msg.m_ProtoData as GameFrameworkMessage.EnterScene;
                if (null != protoMsg) {
                    ulong guid = headerMsg.m_Guid;
                    int sceneId = protoMsg.m_SceneId;
                    int wantRoomId = protoMsg.m_RoomId;

                    UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
                    UserInfo user = dataProcess.GetUserInfo(guid);
                    if (user != null) {
                        Msg_LB_RequestEnterScene builder = new Msg_LB_RequestEnterScene();
                        Msg_LB_BigworldUserBaseInfo baseInfoBuilder = new Msg_LB_BigworldUserBaseInfo();

                        baseInfoBuilder.AccountId = user.AccountId;
                        baseInfoBuilder.NodeName = user.NodeName;
                        baseInfoBuilder.WorldId = UserServerConfig.WorldId;
                        baseInfoBuilder.ClientInfo = user.ClientInfo;
                        baseInfoBuilder.StartServerTime = UserServerConfig.StartServerTime;
                        baseInfoBuilder.SceneId = user.SceneId;

                        builder.BaseInfo = baseInfoBuilder;
                        builder.User = UserThread.BuildRoomUserInfo(user, 0, 0);
                        builder.SceneId = sceneId;
                        builder.WantRoomId = wantRoomId;
                        builder.FromSceneId = user.SceneId;
                        UserServer.Instance.BigworldChannel.Send(builder);
                    }
                }
            }
        }
        private void HandleChangeSceneRoom(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                GameFrameworkMessage.ChangeSceneRoom protoMsg = msg.m_ProtoData as GameFrameworkMessage.ChangeSceneRoom;
                if (null != protoMsg) {
                    ulong guid = headerMsg.m_Guid;
                    int sceneId = protoMsg.m_SceneId;
                    int wantRoomId = protoMsg.m_RoomId;
                    byte[] originalMsgData = msg.m_OriginalMsgData;
                    
                    UserInfo info = UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
                    if (null != info) {
                        UserServer.Instance.ForwardToBigworld(info, originalMsgData);
                    }
                }
            }
        }
        private void HandleRequestSceneRoomInfo(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ulong guid = headerMsg.m_Guid;
                byte[] originalMsgData = msg.m_OriginalMsgData;

                UserInfo info = UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
                if (null != info) {
                    UserServer.Instance.ForwardToBigworld(info, originalMsgData);
                }
            }
        }
        private void HandleRequestSceneRoomList(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ulong guid = headerMsg.m_Guid;
                byte[] originalMsgData = msg.m_OriginalMsgData;

                UserInfo info = UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
                if (null != info) {
                    UserServer.Instance.ForwardToBigworld(info, originalMsgData);
                }
            }
        }
        private void HandleQuitRoom(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                GameFrameworkMessage.QuitRoom protoMsg = msg.m_ProtoData as GameFrameworkMessage.QuitRoom;
                if (null != protoMsg) {
                    ulong guid = headerMsg.m_Guid;
                    bool is_quit_room = protoMsg.m_IsQuitRoom;
                    byte[] originalMsgData = msg.m_OriginalMsgData;

                    UserInfo user = UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
                    if (user != null) {
                        if (user.CurrentState == UserState.Room) {
                            UserServer.Instance.ForwardToBigworld(user, originalMsgData);
                        } else {
                            user.CurrentState = UserState.Online;
                        }
                        LogSys.Log(ServerLogType.INFO, "QuitRoom Guid {0} state {1}", guid, user.CurrentState);
                    }
                }
            }
        }
        private void HandleGetMailList(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid getMailListMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != getMailListMsg) {
                if (m_GlobalProcessThread.CurActionNum > m_MaxGlobalActionNum) {
                    NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.TooManyOperations, getMailListMsg.m_Guid);

                    NodeMessageDispatcher.SendNodeMessage(handle, retMsg);
                    return;
                }
                m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.GetMailList, getMailListMsg.m_Guid);
            }
        }
        private void HandleReadMail(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid readMailMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != readMailMsg) {
                GameFrameworkMessage.Msg_CL_ReadMail protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_ReadMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.ReadMail, readMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleReceiveMail(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid receiveMailMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != receiveMailMsg) {
                GameFrameworkMessage.Msg_CL_ReceiveMail protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_ReceiveMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.ReceiveMail, receiveMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleDeleteMail(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid deleteMailMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != deleteMailMsg) {
                GameFrameworkMessage.Msg_CL_DeleteMail protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_DeleteMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.DeleteMail, deleteMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleAddFriend(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                GameFrameworkMessage.Msg_CL_AddFriend protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_AddFriend;
                if (null != protoData) {
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(nodeMsg.m_Guid);
                    if (null != userThread) {
                        userThread.QueueAction(userThread.AddFriend, nodeMsg.m_Guid, protoData);
                    }
                }
            }
        }
        private void HandleRemoveFriend(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                GameFrameworkMessage.Msg_CL_RemoveFriend protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_RemoveFriend;
                if (null != protoData) {
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(nodeMsg.m_Guid);
                    if (null != userThread) {
                        userThread.QueueAction(userThread.RemoveFriend, nodeMsg.m_Guid, protoData);
                    }
                }
            }
        }
        private void HandleMarkBlack(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                GameFrameworkMessage.Msg_CL_MarkBlack protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_MarkBlack;
                if (null != protoData) {
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(nodeMsg.m_Guid);
                    if (null != userThread) {
                        userThread.QueueAction(userThread.MarkBlack, nodeMsg.m_Guid, protoData);
                    }
                }
            }
        }
        private void HandleUseItem(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                GameFrameworkMessage.Msg_CL_UseItem protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_UseItem;
                if (null != protoData) {
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(nodeMsg.m_Guid);
                    if (null != userThread) {
                        userThread.QueueAction(userThread.UseItem, nodeMsg.m_Guid, protoData);
                    }
                }
            }
        }
        private void HandleDiscardItem(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                GameFrameworkMessage.Msg_CL_DiscardItem protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CL_DiscardItem;
                if (null != protoData) {
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(nodeMsg.m_Guid);
                    if (null != userThread) {
                        userThread.QueueAction(userThread.DiscardItem, nodeMsg.m_Guid, protoData);
                    }
                }
            }
        }
        private void HandleStoryMessage(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid storyMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != storyMsg) {
                GameFrameworkMessage.Msg_CLC_StoryMessage protoData = msg.m_ProtoData as GameFrameworkMessage.Msg_CLC_StoryMessage;
                if (null != protoData) {
                    ulong guid = storyMsg.m_Guid;
                    UserThread userThread = m_UserProcessScheduler.GetUserThread(guid);
                    if (null != userThread) {
                        //Messages sent from the client are prefixed with client to prevent direct calls to server-side logic
                        //(server messages cannot be prefixed with client!)
                        string msgId = string.Format("client:{0}", protoData.m_MsgId);
                        ArrayList args = new ArrayList();
                        args.Add(guid);
                        for (int i = 0; i < protoData.m_Args.Count; i++) {
                            switch (protoData.m_Args[i].val_type) {
                                case LobbyArgType.NULL://null
                                    args.Add(null);
                                    break;
                                case LobbyArgType.INT://int
                                    args.Add(int.Parse(protoData.m_Args[i].str_val));
                                    break;
                                case LobbyArgType.FLOAT://float
                                    args.Add(float.Parse(protoData.m_Args[i].str_val));
                                    break;
                                default://string
                                    args.Add(protoData.m_Args[i].str_val);
                                    break;
                            }
                        }
                        object[] objArgs = args.ToArray();
                        userThread.QueueAction(userThread.SendStoryMessage, msgId, objArgs);
                    }
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
    }
}
