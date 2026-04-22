using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CSharpCenterClient;
using ScriptableFramework;
using ScriptableFrameworkMessage;

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
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.NodeRegister, typeof(ScriptableFrameworkMessage.NodeRegister), typeof(ScriptableFrameworkMessage.NodeRegister), HandleNodeRegister);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.AccountLogin, typeof(ScriptableFrameworkMessage.NodeMessageWithAccount), typeof(ScriptableFrameworkMessage.AccountLogin), HandleAccountLogin);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestNickname, typeof(ScriptableFrameworkMessage.NodeMessageWithAccount), typeof(ScriptableFrameworkMessage.RequestNickname), HandleRequestNickname);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RoleEnter, typeof(ScriptableFrameworkMessage.NodeMessageWithAccount), typeof(ScriptableFrameworkMessage.RoleEnter), HandleRoleEnter);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.UserHeartbeat, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.UserHeartbeat), HandleUserHeartbeat);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.ChangeName, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.ChangeName), HandleChangeName);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.EnterScene, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.EnterScene), HandleEnterScene);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.ChangeSceneRoom, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.ChangeSceneRoom), HandleChangeSceneRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomInfo, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.RequestSceneRoomInfo), HandleRequestSceneRoomInfo);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomList, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.RequestSceneRoomList), HandleRequestSceneRoomList);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.QuitRoom, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.QuitRoom), HandleQuitRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_GetMailList, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), null, HandleGetMailList);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_ReceiveMail, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_ReceiveMail), HandleReceiveMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_ReadMail, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_ReadMail), HandleReadMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_DeleteMail, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_DeleteMail), HandleDeleteMail);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_AddFriend, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_AddFriend), HandleAddFriend);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_RemoveFriend, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_RemoveFriend), HandleRemoveFriend);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_MarkBlack, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_MarkBlack), HandleMarkBlack);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_UseItem, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_UseItem), HandleUseItem);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CL_DiscardItem, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CL_DiscardItem), HandleDiscardItem);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.Msg_CLC_StoryMessage, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.Msg_CLC_StoryMessage), HandleStoryMessage);
            //---------------------------------------------------------------------------------------------------------------
            //The observer handles the messages sent by the big world to the client. The handle of such messages is set to 0.
            //If it is non-0, it should not be processed (the message is forged by the client).
            //---------------------------------------------------------------------------------------------------------------
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.EnterSceneResult, typeof(ScriptableFrameworkMessage.NodeMessageWithGuid), typeof(ScriptableFrameworkMessage.EnterSceneResult), ObserveEnterSceneResult);

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
                    ScriptableFrameworkMessage.NodeMessageWithGuid msgWithGuid = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ScriptableFrameworkMessage.EnterSceneResult protoMsg = msg.m_ProtoData as ScriptableFrameworkMessage.EnterSceneResult;
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
            ScriptableFrameworkMessage.NodeRegister nodeRegMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeRegister;
            if (null != nodeRegMsg) {
                NodeMessage retMsg = new NodeMessage(LobbyMessageDefine.NodeRegisterResult);
                ScriptableFrameworkMessage.NodeRegisterResult nodeRegResultMsg = new NodeRegisterResult();
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

            ScriptableFrameworkMessage.NodeMessageWithAccount loginMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithAccount;
            if (null != loginMsg) {
                ScriptableFrameworkMessage.AccountLogin protoData = msg.m_ProtoData as ScriptableFrameworkMessage.AccountLogin;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoAccountLogin, loginMsg.m_Account, protoData.m_Password, protoData.m_ClientInfo, node_name);
                }
            }
        }
        private void HandleRequestNickname(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithAccount nickMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithAccount;
            if (null != nickMsg) {
                ScriptableFrameworkMessage.RequestNickname protoData = msg.m_ProtoData as ScriptableFrameworkMessage.RequestNickname;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoRequestNickname, nickMsg.m_Account);
                }
            }
        }
        private void HandleRoleEnter(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithAccount enterMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithAccount;
            if (null != enterMsg) {
                ScriptableFrameworkMessage.RoleEnter protoData = msg.m_ProtoData as ScriptableFrameworkMessage.RoleEnter;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoRoleEnter, enterMsg.m_Account, protoData.m_Nickname);
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        private void HandleUserHeartbeat(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithGuid heartbeatMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid nameMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nameMsg) {
                ScriptableFrameworkMessage.ChangeName protoData = msg.m_ProtoData as ScriptableFrameworkMessage.ChangeName;
                if (null != protoData) {
                    m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoChangeName, nameMsg.m_Guid, protoData.m_Nickname);
                }
            }
        }
        private void HandleEnterScene(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ScriptableFrameworkMessage.EnterScene protoMsg = msg.m_ProtoData as ScriptableFrameworkMessage.EnterScene;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ScriptableFrameworkMessage.ChangeSceneRoom protoMsg = msg.m_ProtoData as ScriptableFrameworkMessage.ChangeSceneRoom;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ScriptableFrameworkMessage.QuitRoom protoMsg = msg.m_ProtoData as ScriptableFrameworkMessage.QuitRoom;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid getMailListMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid readMailMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != readMailMsg) {
                ScriptableFrameworkMessage.Msg_CL_ReadMail protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_ReadMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.ReadMail, readMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleReceiveMail(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithGuid receiveMailMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != receiveMailMsg) {
                ScriptableFrameworkMessage.Msg_CL_ReceiveMail protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_ReceiveMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.ReceiveMail, receiveMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleDeleteMail(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithGuid deleteMailMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != deleteMailMsg) {
                ScriptableFrameworkMessage.Msg_CL_DeleteMail protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_DeleteMail;
                if (null != protoData) {
                    m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.DeleteMail, deleteMailMsg.m_Guid, protoData.m_MailGuid);
                }
            }
        }
        private void HandleAddFriend(NodeMessage msg, ulong handle, uint seq)
        {
            ScriptableFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                ScriptableFrameworkMessage.Msg_CL_AddFriend protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_AddFriend;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                ScriptableFrameworkMessage.Msg_CL_RemoveFriend protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_RemoveFriend;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                ScriptableFrameworkMessage.Msg_CL_MarkBlack protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_MarkBlack;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                ScriptableFrameworkMessage.Msg_CL_UseItem protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_UseItem;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid nodeMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != nodeMsg) {
                ScriptableFrameworkMessage.Msg_CL_DiscardItem protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CL_DiscardItem;
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
            ScriptableFrameworkMessage.NodeMessageWithGuid storyMsg = msg.m_NodeHeader as ScriptableFrameworkMessage.NodeMessageWithGuid;
            if (null != storyMsg) {
                ScriptableFrameworkMessage.Msg_CLC_StoryMessage protoData = msg.m_ProtoData as ScriptableFrameworkMessage.Msg_CLC_StoryMessage;
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
