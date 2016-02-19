using System;
using System.Collections.Generic;
using CSharpCenterClient;
using Messenger;
using GameFramework;
using GameFrameworkMessage;

namespace Lobby
{
    internal partial class LobbyServer
    {
        /// <summary>
        /// 注意，userserver来的消息在主线程处理，再分发到其它线程
        /// </summary>
        private void InstallUserHandlers()
        {
            m_UserChannel = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
            m_UserChannel.Register<Msg_LBL_Message>(HandleGeneralMessage);
            m_UserChannel.Register<Msg_LB_UpdateUserServerInfo>(HandleUpdateUserServerInfo);
            m_UserChannel.Register<Msg_LB_QueryUserState>(HandleQueryUserState);
            m_UserChannel.Register<Msg_LB_UserOffline>(HandleUserOffline);
            m_UserChannel.Register<Msg_LB_UserRelogin>(HandleUserRelogin);
            m_UserChannel.Register<Msg_LB_RequestEnterScene>(HandleRequestEnterScene);
            m_UserChannel.Register<Msg_LRL_StoryMessage>(HandleStoryMessageFromUserServer);
        }

        private void DispatchUserMessage(int source_handle, uint seq, byte[] data)
        {
            m_UserChannel.Dispatch(source_handle, seq, data);
        }

        private void HandleGeneralMessage(Msg_LBL_Message msg_, PBChannel channel, int src, uint session)
        {
            try {
                if (msg_.MsgType == Msg_LBL_Message.MsgTypeEnum.Node) {
                    m_UserProcessScheduler.DispatchJsonMessage(session, src, GetMyHandle(), msg_.Data);
                } else if (msg_.MsgType == Msg_LBL_Message.MsgTypeEnum.Room) {
                    byte[] msgData = msg_.Data;
                    m_UserChannel.Dispatch(src, session, msgData);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void HandleUpdateUserServerInfo(Msg_LB_UpdateUserServerInfo msg_, PBChannel channel, int src, uint session)
        {
            m_RoomProcessThread.QueueAction(m_RoomProcessThread.UpdateUserServerInfo, msg_.WorldId, msg_.UserCount);
        }

        private void HandleQueryUserState(Msg_LB_QueryUserState msg_, PBChannel channel, int src, uint session)
        {
            Msg_BL_QueryUserStateResult builder = new Msg_BL_QueryUserStateResult();
            builder.Guid = msg_.Guid;

            UserInfo info = m_UserProcessScheduler.GetUserInfo(msg_.Guid);
            if (null == info) {
                builder.State = (int)UserState.DropOrOffline;
            } else {
                builder.State = (int)info.CurrentState;
            }

            m_UserChannel.Send(src, builder);
        }

        private void HandleUserOffline(Msg_LB_UserOffline msg_, PBChannel channel, int src, uint session)
        {
            m_RoomProcessThread.QueueAction(m_RoomProcessThread.UserOffline, msg_.Guid, src);
        }

        private void HandleUserRelogin(Msg_LB_UserRelogin msg_, PBChannel channel, int src, uint session)
        {
            m_RoomProcessThread.QueueAction(m_RoomProcessThread.UserRelogin, msg_.Guid, src);
        }

        private void HandleRequestEnterScene(Msg_LB_RequestEnterScene msg_, PBChannel channel, int src, uint session)
        {
            UserProcessScheduler dataProcess = m_UserProcessScheduler;
            dataProcess.DispatchAction(dataProcess.RequestEnterScene, msg_);
        }

        private void HandleStoryMessageFromUserServer(Msg_LRL_StoryMessage msg_, PBChannel channel, int src, uint session)
        {
            m_RoomProcessThread.QueueAction(m_RoomProcessThread.HandleRoomStoryMessage, msg_);
        }

        private int GetMyHandle()
        {
            if (0 == m_MyHandle) {
                m_MyHandle = CenterClientApi.TargetHandle("Lobby");
            }
            return m_MyHandle;
        }
    }
}
