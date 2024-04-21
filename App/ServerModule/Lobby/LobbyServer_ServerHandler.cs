using System;
using System.Reflection;

using Lobby;
using CSharpCenterClient;
using GameFrameworkMessage;
using Messenger;

namespace Lobby
{
    internal partial class LobbyServer
    {
        /// <summary>
        /// Note that messages from the room have been distributed to the RoomProcessThread thread for processing,
        /// and there is no need to QueueAction to the RoomProcessThread thread.
        /// </summary>
        private void InstallServerHandlers()
        {
            m_RoomSvrChannel = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
            m_RoomSvrChannel.Register<Msg_RL_RegisterRoomServer>(HandleRegisterRoomServer);
            m_RoomSvrChannel.Register<Msg_RL_RoomServerUpdateInfo>(HandleRoomServerUpdateInfo);
            m_RoomSvrChannel.Register<Msg_RL_EnterSceneResult>(HandleEnterSceneResult);
            m_RoomSvrChannel.Register<Msg_RL_ChangeScene>(HandleChangeScene);
            m_RoomSvrChannel.Register<Msg_RL_ChangeSceneResult>(HandleChangeSceneResult);
            m_RoomSvrChannel.Register<Msg_RL_ActiveScene>(HandleActiveScene);
            m_RoomSvrChannel.Register<Msg_RL_ActiveSceneResult>(HandleActiveSceneResult);
            m_RoomSvrChannel.Register<Msg_RL_UserDrop>(HandleUserDrop);
            m_RoomSvrChannel.Register<Msg_RL_ReplyReconnectUser>(HandelReplyReconnectUser);
            m_RoomSvrChannel.Register<Msg_RL_UserQuit>(HandleUserQuit);
            m_RoomSvrChannel.Register<Msg_RL_PickMoney>(HandlePickMoney);
            m_RoomSvrChannel.Register<Msg_RL_PickItem>(HandlePickItem);
            m_RoomSvrChannel.Register<Msg_LRL_StoryMessage>(HandleStoryMessageFromRoom);
        }
        private void HandleRegisterRoomServer(Msg_RL_RegisterRoomServer msg_, PBChannel channel, ulong src, uint session)
        {
            m_RoomProcessThread.RegisterRoomServer(new RoomServerInfo {
                RoomServerName = msg_.ServerName,
                MaxRoomNum = msg_.MaxRoomNum,
                ServerIp = msg_.ServerIp,
                ServerPort = msg_.ServerPort
            });
        }
        private void HandleRoomServerUpdateInfo(Msg_RL_RoomServerUpdateInfo updateMsg, PBChannel channel, ulong src, uint session)
        {
            //Update RoomServer information
            m_RoomProcessThread.UpdateRoomServerInfo(new RoomServerInfo {
                RoomServerName = updateMsg.ServerName,
                IdleRoomNum = updateMsg.IdleRoomNum,
                UserNum = updateMsg.UserNum
            });
        }
        private void HandleEnterSceneResult(Msg_RL_EnterSceneResult msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to the RoomServer message and enter the wild scene result message
            m_RoomProcessThread.OnEnterSceneResult(msg.UserGuid, msg.RoomId, msg.Result);
        }
        private void HandleChangeScene(Msg_RL_ChangeScene msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to the RoomServer message and enter the wild scene result message
            m_RoomProcessThread.OnChangeScene(msg.UserGuids, msg.SceneId);
        }
        private void HandleChangeSceneResult(Msg_RL_ChangeSceneResult msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to the RoomServer message and enter the wild scene result message
            int hp = 0;
            int mp = 0;
            if (msg.HP > 0 && msg.MP > 0) {
                hp = msg.HP;
                mp = msg.MP;
            }
            m_RoomProcessThread.OnChangeSceneResult(msg.UserGuid, msg.RoomId, msg.TargetRoomId, msg.Result, hp, mp);
        }
        private void HandleActiveScene(Msg_RL_ActiveScene msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to RoomServer messages and activate replica request messages
            m_RoomProcessThread.OnActiveScene(msg.UserGuids, msg.SceneId);
        }
        private void HandleActiveSceneResult(Msg_RL_ActiveSceneResult msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to RoomServer messages and activate replica result messages
            m_RoomProcessThread.OnActiveSceneResult(msg.UserGuids, msg.RoomId, msg.Result);
        }
        private void HandleUserDrop(Msg_RL_UserDrop msg, PBChannel channel, ulong src, uint session)
        {
            //Responding to RoomServer game client exit message
            m_RoomProcessThread.OnRoomUserDrop(msg.RoomId, msg.UserGuid, msg.IsBattleEnd, msg);
        }
        private void HandleUserQuit(Msg_RL_UserQuit msg, PBChannel channel, ulong src, uint session)
        {
            //Respond to RoomServer's reply to lobby Msg_LR_UserQuit message
            m_RoomProcessThread.OnRoomUserQuit(msg.RoomId, msg.UserGuid, msg);
        }
        private void HandlePickMoney(Msg_RL_PickMoney msg, PBChannel channel, ulong src, uint sesssion)
        {
            m_RoomProcessThread.OnPickMoney(msg);
        }
        private void HandlePickItem(Msg_RL_PickItem msg, PBChannel channel, ulong src, uint sesssion)
        {
            m_RoomProcessThread.OnPickItem(msg);
        }
        private void HandelReplyReconnectUser(Msg_RL_ReplyReconnectUser replyMsg, PBChannel channel, ulong src, uint session)
        {
            //Respond to RoomServer messages
            m_RoomProcessThread.OnReplyReconnectUser(replyMsg.UserGuid, replyMsg.RoomId, replyMsg.Result);
        }
        private void HandleStoryMessageFromRoom(Msg_LRL_StoryMessage msg, PBChannel channel, ulong src, uint session)
        {
            UserInfo user = m_UserProcessScheduler.GetUserInfo(msg.UserGuid);
            if(null!=user){
                ForwardToWorld(user.UserSvrName, msg);
            }
        }
    }
}

