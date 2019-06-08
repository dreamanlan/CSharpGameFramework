using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using ScriptRuntime;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// sendmail(guid, title, content, sender, levelDemand, validPeriod, money, gold, item1, item1num, item2, item2num, ...);
    /// </summary>
    internal class SendMailCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendMailCommand cmd = new SendMailCommand();
            cmd.m_Receiver = m_Receiver.Clone();
            cmd.m_Title = m_Title.Clone();
            cmd.m_Content = m_Content.Clone();
            cmd.m_Sender = m_Sender.Clone();
            cmd.m_LevelDemand = m_LevelDemand.Clone();
            cmd.m_ValidPeriod = m_ValidPeriod.Clone();
            cmd.m_Money = m_Money.Clone();
            cmd.m_Gold = m_Gold.Clone();
            for (int i = 0; i < m_MailItems.Count; ++i) {
                IStoryValue<int> val = m_MailItems[i];
                cmd.m_MailItems.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_Receiver.Evaluate(instance, handler, iterator, args);
            m_Title.Evaluate(instance, handler, iterator, args);
            m_Content.Evaluate(instance, handler, iterator, args);
            m_Sender.Evaluate(instance, handler, iterator, args);
            m_LevelDemand.Evaluate(instance, handler, iterator, args);
            m_ValidPeriod.Evaluate(instance, handler, iterator, args);
            m_Money.Evaluate(instance, handler, iterator, args);
            m_Gold.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_MailItems.Count; ++i) {
                IStoryValue<int> val = m_MailItems[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong receiver = m_Receiver.Value;
                string title = m_Title.Value;
                string content = m_Content.Value;
                string sender = m_Sender.Value;
                int levelDemand = m_LevelDemand.Value;
                int validPeriod = m_ValidPeriod.Value;
                int money = m_Money.Value;
                int gold = m_Gold.Value;

                TableMailInfoWrap mailInfo = new TableMailInfoWrap();
                mailInfo.Receiver = (long)receiver;
                mailInfo.Title = title;
                mailInfo.Text = content;
                mailInfo.Sender = sender;
                mailInfo.Money = money;
                mailInfo.Gold = gold;
                mailInfo.LevelDemand = levelDemand;

                for (int i = 0; i < m_MailItems.Count - 1; i += 2) {
                    int itemId = m_MailItems[i].Value;
                    int itemNum = m_MailItems[i + 1].Value;

                    MailItem mailItem = new MailItem();
                    mailItem.m_ItemId = itemId;
                    mailItem.m_ItemNum = itemNum;
                    mailInfo.m_Items.Add(mailItem);
                }
                GlobalProcessThread globalProcess = UserServer.Instance.GlobalProcessThread;
                if (receiver > 0) {
                    globalProcess.QueueAction(globalProcess.SendUserMail, mailInfo, validPeriod);
                } else {
                    globalProcess.QueueAction(globalProcess.SendWholeMail, mailInfo, validPeriod);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 7) {
                m_Receiver.InitFromDsl(callData.GetParam(0));
                m_Title.InitFromDsl(callData.GetParam(1));
                m_Content.InitFromDsl(callData.GetParam(2));
                m_Sender.InitFromDsl(callData.GetParam(3));
                m_LevelDemand.InitFromDsl(callData.GetParam(4));
                m_ValidPeriod.InitFromDsl(callData.GetParam(5));
                m_Money.InitFromDsl(callData.GetParam(6));
                m_Gold.InitFromDsl(callData.GetParam(7));
            }
            for (int i = 8; i < callData.GetParamNum(); ++i) {
                StoryValue<int> val = new StoryValue<int>();
                val.InitFromDsl(callData.GetParam(i));
                m_MailItems.Add(val);
            }
        }

        private IStoryValue<ulong> m_Receiver = new StoryValue<ulong>();
        private IStoryValue<string> m_Title = new StoryValue<string>();
        private IStoryValue<string> m_Content = new StoryValue<string>();
        private IStoryValue<string> m_Sender = new StoryValue<string>();
        private IStoryValue<int> m_LevelDemand = new StoryValue<int>();
        private IStoryValue<int> m_ValidPeriod = new StoryValue<int>();
        private IStoryValue<int> m_Money = new StoryValue<int>();
        private IStoryValue<int> m_Gold = new StoryValue<int>();
        private List<IStoryValue<int>> m_MailItems = new List<IStoryValue<int>>();
    }
    /// <summary>
    /// clearmembers(guid);
    /// </summary>
    internal class ClearMembersCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearMembersCommand cmd = new ClearMembersCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.MemberInfos.Clear();
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
    }
    /// <summary>
    /// addmember(guid, tableid, level);
    /// </summary>
    internal class AddMemberCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AddMemberCommand cmd = new AddMemberCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_TableId = m_TableId.Clone();
            cmd.m_Level = m_Level.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_TableId.Evaluate(instance, handler, iterator, args);
            m_Level.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                int tableId = m_TableId.Value;
                int level = m_Level.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    MemberInfo mi = new MemberInfo();
                    mi.MemberGuid = UserServer.Instance.GlobalProcessThread.GenerateMemberGuid();
                    mi.HeroId = tableId;
                    mi.Level = level;
                    ui.MemberInfos.Add(mi);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_TableId.InitFromDsl(callData.GetParam(1));
                m_Level.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_TableId = new StoryValue<int>();
        private IStoryValue<int> m_Level = new StoryValue<int>();
    }
    /// <summary>
    /// removemember(guid, id_or_guid);
    /// </summary>
    internal class RemoveMemberCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            RemoveMemberCommand cmd = new RemoveMemberCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_MemberId = m_MemberId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_MemberId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                object id = m_MemberId.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (id is ulong) {
                        ulong memberGuid = (ulong)id;
                        MemberInfo mi = ui.MemberInfos.Find(info => info.MemberGuid == memberGuid);
                        if (null != mi) {
                            mi.Deleted = true;
                            ui.DeletedMemberInfos.Add(mi);
                            ui.MemberInfos.Remove(mi);
                        }
                    } else {
                        try {
                            int heroId = (int)id;
                            MemberInfo mi = ui.MemberInfos.Find(info => info.HeroId == heroId);
                            if (null != mi) {
                                mi.Deleted = true;
                                ui.DeletedMemberInfos.Add(mi);
                                ui.MemberInfos.Remove(mi);
                            }
                        } catch {

                        }
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_MemberId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue m_MemberId = new StoryValue();
    }
    /// <summary>
    /// syncmembers(guid);
    /// </summary>
    internal class SyncMembersCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SyncMembersCommand cmd = new SyncMembersCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                userThread.SyncMembers(guid);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
    }
    /// <summary>
    /// clearitems(guid);
    /// </summary>
    internal class ClearItemsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearItemsCommand cmd = new ClearItemsCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.Reset();
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
    }
    /// <summary>
    /// additem(guid, itemid, num);
    /// </summary>
    internal class AddItemCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AddItemCommand cmd = new AddItemCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ItemId = m_ItemId.Clone();
            cmd.m_ItemNum = m_ItemNum.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ItemId.Evaluate(instance, handler, iterator, args);
            m_ItemNum.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                int itemId = m_ItemId.Value;
                int itemNum = m_ItemNum.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.AddItemData(itemId, itemNum);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
                m_ItemNum.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_ItemId = new StoryValue<int>();
        private IStoryValue<int> m_ItemNum = new StoryValue<int>();
    }
    /// <summary>
    /// reduceitem(guid, itemid, num);
    /// </summary>
    internal class ReduceItemCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ReduceItemCommand cmd = new ReduceItemCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ItemId = m_ItemId.Clone();
            cmd.m_ItemNum = m_ItemNum.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ItemId.Evaluate(instance, handler, iterator, args);
            m_ItemNum.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                int itemId = m_ItemId.Value;
                int itemNum = m_ItemNum.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.ReduceItemData(itemId, itemNum);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
                m_ItemNum.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_ItemId = new StoryValue<int>();
        private IStoryValue<int> m_ItemNum = new StoryValue<int>();
    }
    /// <summary>
    /// removeitem(guid, id_or_guid);
    /// </summary>
    internal class RemoveItemCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            RemoveItemCommand cmd = new RemoveItemCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ItemId = m_ItemId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ItemId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                object id = m_ItemId.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (id is ulong) {
                        ulong itemGuid = (ulong)id;
                        ui.ItemBag.DelItemData(itemGuid);
                    } else {
                        try {
                            int itemId = (int)id;
                            ui.ItemBag.DelItemData(itemId);
                        } catch {

                        }
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue m_ItemId = new StoryValue();
    }
    /// <summary>
    /// syncitems(guid);
    /// </summary>
    internal class SyncItemsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SyncItemsCommand cmd = new SyncItemsCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                userThread.SyncItems(guid);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
    }
    /// <summary>
    /// clearuserdatas(guid);
    /// </summary>
    internal class ClearUserDatasCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearUserDatasCommand cmd = new ClearUserDatasCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.IntDatas.Clear();
                    ui.FloatDatas.Clear();
                    ui.StringDatas.Clear();
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
    }
    /// <summary>
    /// adduserdata(guid, key, val);
    /// </summary>
    internal class AddUserDataCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AddUserDataCommand cmd = new AddUserDataCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                string key = m_Key.Value;
                object val = m_Value.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (val is int) {
                        int v = (int)val;
                        if (ui.IntDatas.ContainsKey(key))
                            ui.IntDatas[key] = v;
                        else
                            ui.IntDatas.Add(key, v);
                    } else if (val is float) {
                        float v = (float)val;
                        if (ui.FloatDatas.ContainsKey(key))
                            ui.FloatDatas[key] = v;
                        else
                            ui.FloatDatas.Add(key, v);
                    } else {
                        string v = val as string;
                        if (null == v)
                            v = string.Empty;
                        if (ui.StringDatas.ContainsKey(key))
                            ui.StringDatas[key] = v;
                        else
                            ui.StringDatas.Add(key, v);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue m_Value = new StoryValue();
    }
    /// <summary>
    /// removeuserdata(guid, key, type);
    /// </summary>
    internal class RemoveUserDataCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            RemoveUserDataCommand cmd = new RemoveUserDataCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Type = m_Type.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = m_UserGuid.Value;
                string key = m_Key.Value;
                string type = m_Type.Value;
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (type == "int") {
                        ui.IntDatas.Remove(key);
                    } else if (type == "float") {
                        ui.FloatDatas.Remove(key);
                    } else {
                        ui.StringDatas.Remove(key);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
    }
    /// <summary>
    /// clearglobaldatas(guid);
    /// </summary>
    internal class ClearGlobalDatasCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearGlobalDatasCommand cmd = new ClearGlobalDatasCommand();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
        
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                GlobalData.Instance.Clear();
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
            }
        }
    }
    /// <summary>
    /// addglobaldata(key, val);
    /// </summary>
    internal class AddGlobalDataCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AddGlobalDataCommand cmd = new AddGlobalDataCommand();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string key = m_Key.Value;
                object val = m_Value.Value;
                if (val is int) {
                    int v = (int)val;
                    GlobalData.Instance.AddInt(key, v);
                } else if (val is float) {
                    float v = (float)val;
                    GlobalData.Instance.AddFloat(key, v);
                } else {
                    string v = val as string;
                    if (null == v)
                        v = string.Empty;
                    GlobalData.Instance.AddStr(key, v);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Key.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue m_Value = new StoryValue();
    }
    /// <summary>
    /// removeglobaldata(key, type);
    /// </summary>
    internal class RemoveGlobalDataCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            RemoveGlobalDataCommand cmd = new RemoveGlobalDataCommand();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Type = m_Type.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string key = m_Key.Value;
                string type = m_Type.Value;
                if (type=="int") {
                    GlobalData.Instance.RemoveInt(key);
                } else if (type=="float") {
                    GlobalData.Instance.RemoveFloat(key);
                } else {
                    GlobalData.Instance.RemoveStr(key);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Key.InitFromDsl(callData.GetParam(0));
                m_Type.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
    }
}
