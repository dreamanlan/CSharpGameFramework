using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using ScriptRuntime;
using ScriptableFramework;
using GameFrameworkMessage;

public class MsgPingHandler
{
  public static void Execute(object msg, RoomPeer peer)
  {
    Msg_Ping ping = msg as Msg_Ping;
    if (ping == null) {
      LogSys.Log(ServerLogType.DEBUG, "warning: convert to ping message failed!");
      return;
    }
    LogSys.Log(ServerLogType.DEBUG, "got {0} ping msg send ping time = {1}", peer.Guid, ping.send_ping_time);
    Msg_Pong pongBuilder = new Msg_Pong();
    long curtime = TimeUtility.GetLocalMilliseconds();
    pongBuilder.send_ping_time = ping.send_ping_time;
    pongBuilder.send_pong_time = curtime;
    peer.SetLastPingTime(curtime);
    Msg_Pong msg_pong = pongBuilder;
    peer.SendMessage(RoomMessageDefine.Msg_Pong, msg_pong);
  }
}

