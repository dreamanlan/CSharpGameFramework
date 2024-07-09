using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Lidgren.Network;

using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    public class MessageDispatch
    {
        public delegate void MsgHandler(object msg, RoomPeer user);
        public delegate void LobbyMsgHandler(object msg, NetConnection conn);

        private MyDictionary<int, MsgHandler> m_DicHandler = new MyDictionary<int, MsgHandler>();

        public void RegisterHandler(RoomMessageDefine id, MsgHandler handler)
        {
            m_DicHandler[(int)id] = handler;
        }

        public void Dispatch(int id, object msg, NetConnection conn)
        {
            try {
                // Special handling of authentication messages
                if (id == (int)RoomMessageDefine.Msg_CR_ShakeHands) {
                    Msg_CR_ShakeHands shakehandsMsg = msg as Msg_CR_ShakeHands;
                    if (shakehandsMsg == null) {
                        return;
                    }
                    bool ret = RoomPeerMgr.Instance.OnPeerShakeHands(shakehandsMsg.auth_key, conn);
                    Msg_RC_ShakeHands_Ret builder = new Msg_RC_ShakeHands_Ret();
                    if (ret) {
                        builder.auth_result = Msg_RC_ShakeHands_Ret.RetType.SUCCESS;
                        IOManager.Instance.SendMessage(conn, RoomMessageDefine.Msg_RC_ShakeHands_Ret, builder);
                    } else {
                        builder.auth_result = Msg_RC_ShakeHands_Ret.RetType.ERROR;
                        IOManager.Instance.SendUnconnectedMessage(conn, RoomMessageDefine.Msg_RC_ShakeHands_Ret, builder);
                        conn.Disconnect("disconnect");
                    }
                    return;
                }

                RoomPeer peer = RoomPeerMgr.Instance.GetPeerByConnection(conn);
                // Messages without authenticated connections are not processed.
                if (peer == null) {
                    Msg_RC_ShakeHands_Ret builder = new Msg_RC_ShakeHands_Ret();
                    builder.auth_result = Msg_RC_ShakeHands_Ret.RetType.ERROR;
                    IOManager.Instance.SendUnconnectedMessage(conn, RoomMessageDefine.Msg_RC_ShakeHands_Ret, builder);

                    conn.Disconnect("unauthed");
                    LogSys.Log(ServerLogType.DEBUG, "unauthed peer {0} got message {1}, can't deal it!", conn.RemoteEndPoint.ToString(), msg.ToString());
                    return;
                }

                // Forward the message directly (or perform other processing)
                MsgHandler msghandler;
                if (m_DicHandler.TryGetValue(id, out msghandler)) {
                    msghandler(msg, peer);
                }
                if (msg is Msg_Ping)
                    return;

                // Distribute messages to peers
                RoomPeerMgr.Instance.DispatchPeerMsg(peer, id, msg);
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
