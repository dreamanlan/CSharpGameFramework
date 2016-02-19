using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameFrameworkMessage;

namespace GameFramework
{
    internal class UserThread : MyServerThread
    {
        internal UserThread(int tickSleepTime, int actionNumPerTick):base(tickSleepTime,actionNumPerTick)
        {

        }
        internal void QueryBigworldUserState(ulong guid)
        {
            Msg_LB_QueryUserState builder = new Msg_LB_QueryUserState();
            builder.Guid = guid;
            UserServer.Instance.BigworldChannel.Send(builder);
        }
        internal void TryBigworldUserOffline(ulong guid)
        {
            Msg_LB_UserOffline builder = new Msg_LB_UserOffline();
            builder.Guid = guid;
            UserServer.Instance.BigworldChannel.Send(builder);
        }
        internal void SendStoryMessage(string msgId, params object[] args)
        {
            m_StorySystem.SendMessage(msgId, args);
        }
        internal void SendServerMessage(object msg)
        {
            UserServer.Instance.TransmitToBigworld(msg);
        }
        internal void NotifyUser(ulong guid, LobbyMessageDefine id, object msg)
        {
            UserInfo user = UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (null != user) {
                NodeMessage retMsg = new NodeMessage(id, guid);
                retMsg.m_ProtoData = msg;
                NodeMessageDispatcher.SendNodeMessage(user.NodeName, retMsg);
            }
        }
        internal void NotifyAllUser(LobbyMessageDefine id, object msg)
        {
            UserServer.Instance.UserProcessScheduler.VisitUsers((UserInfo user) => {
                NodeMessage retMsg = new NodeMessage(id, user.Guid);
                retMsg.m_ProtoData = msg;
                NodeMessageDispatcher.SendNodeMessage(user.NodeName, retMsg);
            });
        }
        internal ServerStorySystem StorySystem
        {
            get { return m_StorySystem; }
        }
        internal GmCommands.GmStorySystem GmStorySystem
        {
            get { return m_GmStorySystem; }
        }

        //---------------------------------------------------------------------------------------------
        internal void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg)
        {
            string msgId = string.Format("server:{0}", msg.MsgId);
            ArrayList args = new ArrayList();
            args.Add(msg.UserGuid);
            for (int i = 0; i < msg.Args.Count; i++) {
                switch (msg.Args[i].val_type) {
                    case Msg_LRL_StoryMessage.ArgType.NULL://null
                        args.Add(null);
                        break;
                    case Msg_LRL_StoryMessage.ArgType.INT://int
                        args.Add(int.Parse(msg.Args[i].str_val));
                        break;
                    case Msg_LRL_StoryMessage.ArgType.FLOAT://float
                        args.Add(float.Parse(msg.Args[i].str_val));
                        break;
                    default://string
                        args.Add(msg.Args[i].str_val);
                        break;
                }
            }
            object[] objArgs = args.ToArray();
            SendStoryMessage(msgId, objArgs);
        }
        //---------------------------------------------------------------------------------------------

        protected override void OnStart()
        {
            ServerStorySystem.ThreadInitMask();
            m_GmStorySystem.Init(this);
            m_StorySystem.Init(this);
            m_StorySystem.StartStory("user_main");
        }
        protected override void OnTick()
        {
            m_GmStorySystem.Tick();
            m_StorySystem.Tick();
        }

        private ServerStorySystem m_StorySystem = new ServerStorySystem();
        private GmCommands.GmStorySystem m_GmStorySystem = new GmCommands.GmStorySystem();

        internal static Msg_LR_RoomUserInfo BuildRoomUserInfo(UserInfo info, float x, float y)
        {
            //todo:加锁
            Msg_LR_RoomUserInfo ruiBuilder = new Msg_LR_RoomUserInfo();
            ruiBuilder.Guid = info.Guid;
            ruiBuilder.Nick = info.Nickname;
            ruiBuilder.Key = info.Key;
            ruiBuilder.Hero = info.HeroId;
            ruiBuilder.Camp = info.CampId;
            ruiBuilder.IsMachine = false;
            ruiBuilder.Level = info.Level;
            ///
            int memberCount = info.MemberInfos.Count;
            if (memberCount > 0) {
                Msg_LR_RoomUserInfo.MemberInfo[] memberList = new Msg_LR_RoomUserInfo.MemberInfo[memberCount];
                for (int i = 0; i < memberCount; ++i) {
                    memberList[i] = new Msg_LR_RoomUserInfo.MemberInfo();
                    memberList[i].Hero = info.MemberInfos[i].HeroId;
                    memberList[i].Level = info.MemberInfos[i].Level;
                }
            }
            if (x > 0 && y > 0) {
                ruiBuilder.EnterX = x;
                ruiBuilder.EnterY = y;
            }
            return ruiBuilder;
        }
    }
}
