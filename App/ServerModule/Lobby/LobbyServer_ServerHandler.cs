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
        /// 注意，room来的消息已经分发到RoomProcessThread线程里进行处理，不需要再QueueAction到RoomProcessThread线程
        /// </summary>
        private void InstallServerHandlers()
        {
            m_RoomSvrChannel = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
            m_RoomSvrChannel.Register<Msg_RL_RegisterRoomServer>(HandleRegisterRoomServer);
            m_RoomSvrChannel.Register<Msg_RL_RoomServerUpdateInfo>(HandleRoomServerUpdateInfo);
            m_RoomSvrChannel.Register<Msg_RL_EnterSceneResult>(HandleEnterSceneResult);
            m_RoomSvrChannel.Register<Msg_RL_ChangeScene>(HandleChangeScene);
            m_RoomSvrChannel.Register<Msg_RL_ChangeSceneResult>(HandleChangeSceneResult);
            m_RoomSvrChannel.Register<Msg_RL_UserDrop>(HandleUserDrop);
            m_RoomSvrChannel.Register<Msg_RL_ReplyReconnectUser>(HandelReplyReconnectUser);
            m_RoomSvrChannel.Register<Msg_RL_UserQuit>(HandleUserQuit);
            m_RoomSvrChannel.Register<Msg_RL_PickMoney>(HandlePickMoney);
            m_RoomSvrChannel.Register<Msg_RL_PickItem>(HandlePickItem);
            m_RoomSvrChannel.Register<Msg_LRL_StoryMessage>(HandleStoryMessageFromRoom);
        }
        private void HandleRegisterRoomServer(Msg_RL_RegisterRoomServer msg_, PBChannel channel, int src, uint session)
        {
            m_RoomProcessThread.RegisterRoomServer(new RoomServerInfo {
                RoomServerName = msg_.ServerName,
                MaxRoomNum = msg_.MaxRoomNum,
                ServerIp = msg_.ServerIp,
                ServerPort = msg_.ServerPort
            });
        }
        private void HandleRoomServerUpdateInfo(Msg_RL_RoomServerUpdateInfo updateMsg, PBChannel channel, int src, uint session)
        {
            //更新RoomServer信息
            m_RoomProcessThread.UpdateRoomServerInfo(new RoomServerInfo {
                RoomServerName = updateMsg.ServerName,
                IdleRoomNum = updateMsg.IdleRoomNum,
                UserNum = updateMsg.UserNum
            });
        }
        private void HandleEnterSceneResult(Msg_RL_EnterSceneResult msg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer消息，进入野外场景结果消息
            m_RoomProcessThread.OnEnterSceneResult(msg.UserGuid, msg.RoomID, msg.Result);
        }
        private void HandleChangeScene(Msg_RL_ChangeScene msg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer消息，进入野外场景结果消息
            m_RoomProcessThread.OnChangeScene(msg.UserGuid, msg.SceneID);
        }
        private void HandleChangeSceneResult(Msg_RL_ChangeSceneResult msg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer消息，进入野外场景结果消息
            int hp = 0;
            int mp = 0;
            if (msg.HP > 0 && msg.MP > 0) {
                hp = msg.HP;
                mp = msg.MP;
            }
            m_RoomProcessThread.OnChangeSceneResult(msg.UserGuid, msg.RoomID, msg.TargetRoomID, msg.Result, hp, mp);
        }
        private void HandleUserDrop(Msg_RL_UserDrop msg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer游戏客户端退出消息
            m_RoomProcessThread.OnRoomUserDrop(msg.RoomID, msg.UserGuid, msg.IsBattleEnd, msg);
        }
        private void HandleUserQuit(Msg_RL_UserQuit msg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer回复lobby Msg_LR_UserQuit的消息
            m_RoomProcessThread.OnRoomUserQuit(msg.RoomID, msg.UserGuid, msg);
        }
        private void HandlePickMoney(Msg_RL_PickMoney msg, PBChannel channel, int src, uint sesssion)
        {
            m_RoomProcessThread.OnPickMoney(msg);
        }
        private void HandlePickItem(Msg_RL_PickItem msg, PBChannel channel, int src, uint sesssion)
        {
            m_RoomProcessThread.OnPickItem(msg);
        }
        private void HandelReplyReconnectUser(Msg_RL_ReplyReconnectUser replyMsg, PBChannel channel, int src, uint session)
        {
            //响应RoomServer消息
            m_RoomProcessThread.OnReplyReconnectUser(replyMsg.UserGuid, replyMsg.RoomID, replyMsg.Result);
        }
        private void HandleStoryMessageFromRoom(Msg_LRL_StoryMessage msg, PBChannel channel, int src, uint session)
        {
            UserInfo user = m_UserProcessScheduler.GetUserInfo(msg.UserGuid);
            if(null!=user){
                ForwardToWorld(user.UserSvrName, msg);
            }
        }
    }
}

