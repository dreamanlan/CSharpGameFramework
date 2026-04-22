using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFrameworkMessage;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// sendmail(guid, title, content, sender, levelDemand, validPeriod, money, gold, item1, item1num, item2, item2num, ...);
    /// </summary>
    internal sealed class SendMailCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 7)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong receiver = operands[0].GetULong();
                string title = operands[1].ToString();
                string content = operands[2].ToString();
                string sender = operands[3].ToString();
                int levelDemand = operands[4].GetInt();
                int validPeriod = operands[5].GetInt();
                int money = operands[6].GetInt();
                int gold = operands[7].GetInt();

                TableMailInfoWrap mailInfo = new TableMailInfoWrap();
                mailInfo.Receiver = (long)receiver;
                mailInfo.Title = title;
                mailInfo.Text = content;
                mailInfo.Sender = sender;
                mailInfo.Money = money;
                mailInfo.Gold = gold;
                mailInfo.LevelDemand = levelDemand;

                for (int i = 8; i < operands.Count - 1; i += 2) {
                    int itemId = operands[i].GetInt();
                    int itemNum = operands[i + 1].GetInt();

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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// clearmembers(guid);
    /// </summary>
    internal sealed class ClearMembersCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.MemberInfos.Clear();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// addmember(guid, tableid, level);
    /// </summary>
    internal sealed class AddMemberCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                int tableId = operands[1].GetInt();
                int level = operands[2].GetInt();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    MemberInfo mi = new MemberInfo();
                    mi.MemberGuid = UserServer.Instance.GlobalProcessThread.GenerateMemberGuid();
                    mi.HeroId = tableId;
                    mi.Level = level;
                    ui.MemberInfos.Add(mi);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// removemember(guid, id_or_guid);
    /// </summary>
    internal sealed class RemoveMemberCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                var id = operands[1];
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (id.Type == BoxedValue.c_ULongType) {
                        ulong memberGuid = id.GetULong();
                        MemberInfo mi = ui.MemberInfos.Find(info => info.MemberGuid == memberGuid);
                        if (null != mi) {
                            mi.Deleted = true;
                            ui.DeletedMemberInfos.Add(mi);
                            ui.MemberInfos.Remove(mi);
                        }
                    } else {
                        try {
                            int heroId = id.GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// syncmembers(guid);
    /// </summary>
    internal sealed class SyncMembersCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                userThread.SyncMembers(guid);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// clearitems(guid);
    /// </summary>
    internal sealed class ClearItemsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.Reset();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// additem(guid, itemid, num);
    /// </summary>
    internal sealed class AddItemCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                int itemId = operands[1].GetInt();
                int itemNum = operands[2].GetInt();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.AddItemData(itemId, itemNum);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// reduceitem(guid, itemid, num);
    /// </summary>
    internal sealed class ReduceItemCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                int itemId = operands[1].GetInt();
                int itemNum = operands[2].GetInt();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.ItemBag.ReduceItemData(itemId, itemNum);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// removeitem(guid, id_or_guid);
    /// </summary>
    internal sealed class RemoveItemCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                var id = operands[1];
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (id.Type == BoxedValue.c_ULongType) {
                        ulong itemGuid = id.GetULong();
                        ui.ItemBag.DelItemData(itemGuid);
                    } else {
                        try {
                            int itemId = id.GetInt();
                            ui.ItemBag.DelItemData(itemId);
                        } catch {

                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// syncitems(guid);
    /// </summary>
    internal sealed class SyncItemsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                userThread.SyncItems(guid);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// clearuserdatas(guid);
    /// </summary>
    internal sealed class ClearUserDatasCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    ui.IntDatas.Clear();
                    ui.FloatDatas.Clear();
                    ui.StringDatas.Clear();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// adduserdata(guid, key, val);
    /// </summary>
    internal sealed class AddUserDataCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                string key = operands[1].ToString();
                var val = operands[2];
                UserInfo ui = userThread.GetUserInfo(guid);
                if (null != ui) {
                    if (val.IsInteger) {
                        int v = val.GetInt();
                        if (ui.IntDatas.ContainsKey(key))
                            ui.IntDatas[key] = v;
                        else
                            ui.IntDatas.Add(key, v);
                    } else if (val.IsNumber) {
                        float v = val.GetFloat();
                        if (ui.FloatDatas.ContainsKey(key))
                            ui.FloatDatas[key] = v;
                        else
                            ui.FloatDatas.Add(key, v);
                    } else {
                        string v = val.StringVal;
                        if (null == v)
                            v = string.Empty;
                        if (ui.StringDatas.ContainsKey(key))
                            ui.StringDatas[key] = v;
                        else
                            ui.StringDatas.Add(key, v);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// removeuserdata(guid, key, type);
    /// </summary>
    internal sealed class RemoveUserDataCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong guid = operands[0].GetULong();
                string key = operands[1].ToString();
                string type = operands[2].ToString();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// clearglobaldatas();
    /// </summary>
    internal sealed class ClearGlobalDatasCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                GlobalData.Instance.Clear();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// addglobaldata(key, val);
    /// </summary>
    internal sealed class AddGlobalDataCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string key = operands[0].ToString();
                var val = operands[1];
                if (val.IsInteger) {
                    int v = val.GetInt();
                    GlobalData.Instance.AddInt(key, v);
                } else if (val.IsNumber) {
                    float v = val.GetFloat();
                    GlobalData.Instance.AddFloat(key, v);
                } else {
                    string v = val.StringVal;
                    if (null == v)
                        v = string.Empty;
                    GlobalData.Instance.AddStr(key, v);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// removeglobaldata(key, type);
    /// </summary>
    internal sealed class RemoveGlobalDataCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string key = operands[0].ToString();
                string type = operands[1].ToString();
                if (type == "int") {
                    GlobalData.Instance.RemoveInt(key);
                } else if (type == "float") {
                    GlobalData.Instance.RemoveFloat(key);
                } else {
                    GlobalData.Instance.RemoveStr(key);
                }
            }
            return BoxedValue.NullObject;
        }
    }
}
