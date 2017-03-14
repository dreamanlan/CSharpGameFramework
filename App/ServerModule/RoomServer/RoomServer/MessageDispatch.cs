using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Lidgren.Network;

using GameFramework;
using GameFrameworkMessage;

namespace GameFramework
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
                // 特殊处理认证消息
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
                // 没有认证连接的消息不进行处理
                if (peer == null) {
                    Msg_RC_ShakeHands_Ret builder = new Msg_RC_ShakeHands_Ret();
                    builder.auth_result = Msg_RC_ShakeHands_Ret.RetType.ERROR;
                    IOManager.Instance.SendUnconnectedMessage(conn, RoomMessageDefine.Msg_RC_ShakeHands_Ret, builder);

                    conn.Disconnect("unauthed");
                    LogSys.Log(LOG_TYPE.DEBUG, "unauthed peer {0} got message {1}, can't deal it!", conn.RemoteEndPoint.ToString(), msg.ToString());
                    return;
                }

                // 直接转发消息(或进行其它处理)
                MsgHandler msghandler;
                if (m_DicHandler.TryGetValue(id, out msghandler)) {
                    msghandler(msg, peer);
                }
                if (msg is Msg_Ping)
                    return;

                // 消息分发到peer
                RoomPeerMgr.Instance.DispatchPeerMsg(peer, id, msg);
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
