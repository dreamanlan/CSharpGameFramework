using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CSharpCenterClient;
using Lobby;
using ScriptableFramework;
using GameFrameworkMessage;

namespace Lobby
{
    internal partial class LobbyServer
    {
        /// <summary>
        /// Note that messages from node are directly distributed to the thread pool of DataProcess for processing, so multi-thread safety needs to be considered!
        /// </summary>
        private void InstallNodeHandlers()
        {
            NodeMessageDispatcher.Init();
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.QuitRoom, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.QuitRoom), this.HandleQuitRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.EnterScene, typeof(GameFrameworkMessage.NodeMessageWithGuid), null, this.HandleEnterScene);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.ChangeSceneRoom, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.ChangeSceneRoom), HandleChangeSceneRoom);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomInfo, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.RequestSceneRoomInfo), HandleRequestSceneRoomInfo);
            NodeMessageDispatcher.RegisterMessageHandler((int)LobbyMessageDefine.RequestSceneRoomList, typeof(GameFrameworkMessage.NodeMessageWithGuid), typeof(GameFrameworkMessage.RequestSceneRoomList), HandleRequestSceneRoomList);
            //--------------------------------------------------------------------------------------
        }
        //==========================================================================================================================
        private void HandleEnterScene(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid startGameMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (startGameMsg != null) {
                GameFrameworkMessage.Msg_LB_RequestEnterScene msgData = msg.m_ProtoData as GameFrameworkMessage.Msg_LB_RequestEnterScene;
                if (null != msgData) {
                    m_RoomProcessThread.QueueAction(m_RoomProcessThread.RequestEnterScene, startGameMsg.m_Guid, msgData.SceneId, msgData.WantRoomId, msgData.FromSceneId);
                }
            }
        }
        private void HandleQuitRoom(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid quitClientMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != quitClientMsg) {
                GameFrameworkMessage.QuitRoom protoData = msg.m_ProtoData as GameFrameworkMessage.QuitRoom;
                if (null != protoData) {
                    m_RoomProcessThread.QueueAction(m_RoomProcessThread.QuitRoom, quitClientMsg.m_Guid, protoData.m_IsQuitRoom, handle);
                }
            }
        }
        private void HandleChangeSceneRoom(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid changeSceneMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != changeSceneMsg) {
                GameFrameworkMessage.ChangeSceneRoom protoData = msg.m_ProtoData as GameFrameworkMessage.ChangeSceneRoom;
                if (null != protoData) {
                    m_RoomProcessThread.QueueAction(m_RoomProcessThread.RequestChangeSceneRoom, changeSceneMsg.m_Guid, protoData.m_SceneId, protoData.m_RoomId);
                }
            }
        }
        private void HandleRequestSceneRoomInfo(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ulong guid = headerMsg.m_Guid;

                RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
                roomProcess.QueueAction(roomProcess.RequestSceneRoomInfo, guid);
            }
        }
        private void HandleRequestSceneRoomList(NodeMessage msg, ulong handle, uint seq)
        {
            GameFrameworkMessage.NodeMessageWithGuid headerMsg = msg.m_NodeHeader as GameFrameworkMessage.NodeMessageWithGuid;
            if (null != headerMsg) {
                ulong guid = headerMsg.m_Guid;

                RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
                roomProcess.QueueAction(roomProcess.RequestSceneRoomList, guid);
            }
        }
    }
}
