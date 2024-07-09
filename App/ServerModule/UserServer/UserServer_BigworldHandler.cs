using System;
using System.Collections.Generic;
using CSharpCenterClient;
using Messenger;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    internal partial class UserServer
    {
        /// <summary>
        /// Note that bigworld messages have been distributed to the RoomProcessThread thread for processing,
        /// and there is no need to QueueAction to the RoomProcessThread thread.
        /// </summary>
        private void InstallBigworldHandlers()
        {
            if (UserServerConfig.WorldIdNum > 0) {
                m_BigworldChannel = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
                m_BigworldChannel.WorldId = UserServerConfig.WorldId1;
                m_BigworldChannel.DefaultServiceName = "Lobby";
                m_BigworldChannel.Register<Msg_LBL_Message>(HandleGeneralMessage);
                m_BigworldChannel.Register<Msg_BL_QueryUserStateResult>(HandleQueryUserStateResult);
                m_BigworldChannel.Register<Msg_BL_UserOffline>(HandleUserOffline);
                m_BigworldChannel.Register<Msg_BL_BroadcastText>(HandleBroadcastText);
                m_BigworldChannel.Register<Msg_BL_UserChangeScene>(HandleUserChangeScene);
                m_BigworldChannel.Register<Msg_RL_UserDrop>(HandleUserDrop);
                m_BigworldChannel.Register<Msg_RL_UserQuit>(HandleUserQuit);
                m_BigworldChannel.Register<Msg_RL_PickMoney>(HandlePickMoney);
                m_BigworldChannel.Register<Msg_RL_PickItem>(HandlePickItem);
                m_BigworldChannel.Register<Msg_LRL_StoryMessage>(HandleRoomStoryMessage);
            }
        }

        private void HandleGeneralMessage(Msg_LBL_Message msg_, PBChannel channel, ulong src, uint session)
        {
            try {
                if (msg_.MsgType == Msg_LBL_Message.MsgTypeEnum.Node) {
                    byte[] msgData = msg_.Data;
                    //observer
                    m_UserProcessScheduler.DispatchJsonMessage(false, session, 0, 0, msgData);
                    //forward
                    NodeMessageDispatcher.ForwardMessage(msg_.TargetName, msgData);
                } else if (msg_.MsgType == Msg_LBL_Message.MsgTypeEnum.Room) {
                    m_BigworldChannel.Dispatch(src, session, msg_.Data);
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void HandleQueryUserStateResult(Msg_BL_QueryUserStateResult msg_, PBChannel channel, ulong src, uint session)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(msg_.Guid);
            if (user != null && user.CurrentState == UserState.Room) {
                user.CurrentState = UserState.Online;
            }
        }
        private void HandleUserOffline(Msg_BL_UserOffline msg_, PBChannel channel, ulong src, uint session)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(msg_.Guid);
            if (user != null && user.CurrentState == UserState.Room) {
                user.CurrentState = UserState.Online;
                user.LeftLife = 0;
            }
        }
        private void HandleBroadcastText(Msg_BL_BroadcastText msg_, PBChannel channel, ulong src, uint session)
        {
            m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.HandleBroadcast, (BroadcastType)msg_.BroadcastType, msg_.Content, msg_.RollCount);
        }
        private void HandleUserChangeScene(Msg_BL_UserChangeScene msg_, PBChannel channel, ulong src, uint session)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(msg_.Guid);
            if (user != null) {
                user.SceneId = msg_.SceneId;
            }
        }
        private void HandleUserDrop(Msg_RL_UserDrop msg_, PBChannel channel, ulong src, uint session)
        {
        }
        private void HandleUserQuit(Msg_RL_UserQuit msg_, PBChannel channel, ulong src, uint session)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserThread userThread = dataProcess.GetUserThread(msg_.UserGuid);
            if (null != userThread) {
                userThread.QueueAction(userThread.HandleUserQuit, msg_);
            } else {
                dataProcess.DefaultUserThread.QueueAction(dataProcess.DefaultUserThread.HandleUserQuit, msg_);
            }
        }
        private void HandlePickMoney(Msg_RL_PickMoney msg_, PBChannel channel, ulong src, uint session)
        {
        }
        private void HandlePickItem(Msg_RL_PickItem msg_, PBChannel channel, ulong src, uint session)
        {
        }
        private void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg_, PBChannel channel, ulong src, uint session)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserThread userThread = dataProcess.GetUserThread(msg_.UserGuid);
            if (null != userThread) {
                userThread.QueueAction(userThread.HandleRoomStoryMessage, msg_);
            } else {
                dataProcess.DefaultUserThread.QueueAction(dataProcess.DefaultUserThread.HandleRoomStoryMessage, msg_);
            }
        }
    }
}
