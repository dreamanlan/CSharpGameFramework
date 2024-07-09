using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameFrameworkMessage;
using DotnetStoryScript;

namespace ScriptableFramework
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
            var bvlist = m_StorySystem.NewBoxedValueList();
            foreach(var arg in args) {
                bvlist.Add(BoxedValue.FromObject(arg));
            }
            m_StorySystem.SendMessage(msgId, bvlist);
        }
        internal void SendServerMessage(object msg)
        {
            UserServer.Instance.TransmitToBigworld(msg);
        }
        internal void AddFriend(ulong guid, Msg_CL_AddFriend msg)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {
                ulong friendGuid = UserServer.Instance.UserProcessScheduler.FindUserGuidByNickname(msg.m_FriendNickname);
                UserInfo friendUserInfo = GetUserInfo(friendGuid);
                if (null != friendUserInfo) {
                    FriendInfo friendInfo = userInfo.FriendInfos.Find(fi => fi.FriendNickname == msg.m_FriendNickname);
                    if (null == friendInfo) {
                        friendInfo = new FriendInfo();
                        friendInfo.Guid = UserServer.Instance.GlobalProcessThread.GenerateFriendGuid();
                        userInfo.FriendInfos.Add(friendInfo);
                    }
                    friendInfo.FriendNickname = msg.m_FriendNickname;
                    friendInfo.FriendGuid = friendGuid;
                    friendInfo.UserGuid = guid;

                    FriendInfoForMessage msgInfo = new FriendInfoForMessage();
                    msgInfo.FriendGuid = friendInfo.FriendGuid;
                    msgInfo.FriendNickname = friendInfo.FriendNickname;
                    msgInfo.IsBlack = friendInfo.IsBlack;

                    Msg_LC_AddFriend retMsg = new Msg_LC_AddFriend();
                    retMsg.m_FriendInfo = msgInfo;
                    NotifyUser(guid, LobbyMessageDefine.Msg_LC_AddFriend, retMsg);
                }
            }
        }
        internal void RemoveFriend(ulong guid, Msg_CL_RemoveFriend msg)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {
                FriendInfo friendInfo = userInfo.FriendInfos.Find(fi => fi.FriendGuid == msg.m_FriendGuid);
                if (null != friendInfo) {
                    friendInfo.Deleted = true;

                    Msg_LC_RemoveFriend retMsg = new Msg_LC_RemoveFriend();
                    retMsg.m_FriendGuid = msg.m_FriendGuid;
                    NotifyUser(guid, LobbyMessageDefine.Msg_LC_RemoveFriend, retMsg);
                }
            }
        }
        internal void MarkBlack(ulong guid, Msg_CL_MarkBlack msg)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {
                FriendInfo friendInfo = userInfo.FriendInfos.Find(fi => fi.FriendGuid == msg.m_FriendGuid);
                if (null != friendInfo) {
                    friendInfo.IsBlack = true;

                    Msg_LC_MarkBlack retMsg = new Msg_LC_MarkBlack();
                    retMsg.m_FriendGuid = msg.m_FriendGuid;
                    NotifyUser(guid, LobbyMessageDefine.Msg_LC_MarkBlack, retMsg);
                }
            }
        }
        internal void UseItem(ulong guid, Msg_CL_UseItem msg)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {

                SyncItems(guid);
            }
        }
        internal void DiscardItem(ulong guid, Msg_CL_DiscardItem msg)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {

                SyncItems(guid);
            }
        }
        internal void AddAssets(ulong guid, int money, int gold)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {
                userInfo.IncreaseMoney(money);
                userInfo.IncreaseGold(gold);

                SyncRoleInfo(guid);
            }
        }
        internal void AddItem(ulong guid, int itemId, int itemNum)
        {
            UserInfo userInfo = GetUserInfo(guid);
            if (null != userInfo) {
                userInfo.ItemBag.AddItemData(itemId, itemNum);

                SyncItems(guid);
            }
        }
        internal UserInfo GetUserInfo(ulong guid)
        {
            return UserServer.Instance.UserProcessScheduler.GetUserInfo(guid);
        }
        internal void SyncRoleInfo(ulong guid)
        {
            UserInfo user = GetUserInfo(guid);
            if (null != user) {
                Msg_LC_SyncRoleInfo protoData = new Msg_LC_SyncRoleInfo();
                protoData.HeroId = user.HeroId;
                protoData.Level = user.Level;
                protoData.Money = user.Money;
                protoData.Gold = user.Gold;

                NotifyUser(guid, LobbyMessageDefine.Msg_LC_SyncRoleInfo, protoData);
            }
        }
        internal void SyncMembers(ulong guid)
        {
            UserInfo user = GetUserInfo(guid);
            if (null != user) {
                Msg_LC_SyncMemberList protoData = new Msg_LC_SyncMemberList();
                for (int i = 0; i < user.MemberInfos.Count; ++i) {
                    MemberInfoForMessage mi = new MemberInfoForMessage();
                    mi.Hero = user.MemberInfos[i].HeroId;
                    mi.Level = user.MemberInfos[i].Level;

                    protoData.m_Members.Add(mi);
                }
                NotifyUser(guid, LobbyMessageDefine.Msg_LC_SyncMemberList, protoData);
            }
        }
        internal void SyncItems(ulong guid)
        {
            UserInfo user = GetUserInfo(guid);
            if (null != user) {
                Msg_LC_SyncItemList protoData = new Msg_LC_SyncItemList();
                for (int i = 0; i < user.ItemBag.ItemCount; ++i) {
                    ItemInfo item = user.ItemBag.ItemInfos[i];

                    ItemInfoForMessage itemInfo = new ItemInfoForMessage();
                    itemInfo.ItemGuid = item.ItemGuid;
                    itemInfo.ItemId = item.ItemId;
                    itemInfo.ItemNum = item.ItemNum;

                    protoData.m_Items.Add(itemInfo);
                }
                NotifyUser(guid, LobbyMessageDefine.Msg_LC_SyncItemList, protoData);
            }
        }
        internal void NotifyUser(ulong guid, LobbyMessageDefine id, object msg)
        {
            UserInfo user = GetUserInfo(guid);
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
        internal SortedList<string, string> CommandDocs
        {
            get { return m_CommandDocs; }
        }
        internal SortedList<string, string> FunctionDocs
        {
            get { return m_FunctionDocs; }
        }

        //---------------------------------------------------------------------------------------------
        internal void HandleUserQuit(Msg_RL_UserQuit msg)
        {
            UserInfo user = GetUserInfo(msg.UserGuid);
            if (null != user) {
                user.CurrentState = UserState.Online;
                user.LeftLife = UserInfo.LifeTimeOfNoHeartbeat;
            }
        }
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
            m_StorySystem.LoadStory("UserServer/main.dsl");
            m_StorySystem.StartStory("user_main");

            m_CommandDocs = StoryCommandManager.Instance.GenCommandDocs();
            m_FunctionDocs = StoryFunctionManager.Instance.GenFunctionDocs();
        }
        protected override void OnTick()
        {
            m_GmStorySystem.Tick();
            m_StorySystem.Tick();
        }

        private ServerStorySystem m_StorySystem = new ServerStorySystem();
        private GmCommands.GmStorySystem m_GmStorySystem = new GmCommands.GmStorySystem();
        private SortedList<string, string> m_CommandDocs;
        private SortedList<string, string> m_FunctionDocs;

        internal static Msg_LR_RoomUserInfo BuildRoomUserInfo(UserInfo info, float x, float y)
        {
            //todo:lock
            Msg_LR_RoomUserInfo ruiBuilder = new Msg_LR_RoomUserInfo();
            ruiBuilder.Guid = info.Guid;
            ruiBuilder.Nick = info.Nickname;
            ruiBuilder.Key = info.Key;
            ruiBuilder.Hero = info.HeroId;
            ruiBuilder.Camp = info.CampId;
            ruiBuilder.IsMachine = false;
            ruiBuilder.Level = info.Level;
            if (x > 0 && y > 0) {
                ruiBuilder.EnterX = x;
                ruiBuilder.EnterY = y;
            }
            return ruiBuilder;
        }
    }
}
