using System;
using System.Collections.Generic;
using ScriptRuntime;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class GetMemberCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    return ui.MemberInfos.Count;
                }
            }
            return 0;
        }
    }
    internal sealed class GetMemberInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                int index = operands[1].GetInt();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    if (index >= 0 && index < ui.MemberInfos.Count) {
                        return BoxedValue.FromObject(ui.MemberInfos[index]);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetFriendCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    return ui.FriendInfos.Count;
                }
            }
            return 0;
        }
    }
    internal sealed class GetFriendInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                var id = operands[1];
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    if (id.Type == BoxedValue.c_ULongType) {
                        ulong guid = id.GetULong();
                        return BoxedValue.FromObject(ui.FriendInfos.Find(fi => fi.FriendGuid == guid));
                    } else {
                        try {
                            int index = id.GetInt();
                            if (index >= 0 && index < ui.FriendInfos.Count) {
                                return BoxedValue.FromObject(ui.FriendInfos[index]);
                            }
                        } catch {
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetItemCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    return ui.ItemBag.ItemCount;
                }
            }
            return 0;
        }
    }
    internal sealed class GetFreeItemCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    return ui.ItemBag.GetFreeCount();
                }
            }
            return 0;
        }
    }
    internal sealed class GetItemInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                var id = operands[1];
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    if (id.Type == BoxedValue.c_ULongType) {
                        ulong guid = id.GetULong();
                        return BoxedValue.FromObject(ui.ItemBag.GetItemData(guid));
                    } else {
                        try {
                            int index = id.GetInt();
                            if (index >= 0 && index < ui.ItemBag.ItemCount) {
                                return BoxedValue.FromObject(ui.ItemBag.ItemInfos[index]);
                            }
                        } catch {
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindItemInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                var id = operands[1];
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    if (id.Type == BoxedValue.c_ULongType) {
                        ulong guid = id.GetULong();
                        return BoxedValue.FromObject(ui.ItemBag.GetItemData(guid));
                    } else {
                        try {
                            int itemId = id.GetInt();
                            return BoxedValue.FromObject(ui.ItemBag.GetItemData(itemId));
                        } catch {
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class CalcItemNumFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                int itemId = operands[1].GetInt();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    return ui.ItemBag.GetItemNum(itemId);
                }
            }
            return 0;
        }
    }
    internal sealed class GetUserDataFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                string key = operands[1].ToString();
                string type = operands[2].ToString();
                UserInfo ui = userThread.GetUserInfo(userGuid);
                if (null != ui) {
                    if (type == "int") {
                        int v;
                        ui.IntDatas.TryGetValue(key, out v);
                        return v;
                    } else if (type == "float") {
                        float v;
                        ui.FloatDatas.TryGetValue(key, out v);
                        return v;
                    } else {
                        string v;
                        ui.StringDatas.TryGetValue(key, out v);
                        return v;
                    }
                }
            }
            return 0;
        }
    }
    internal sealed class GetGlobalDataFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string key = operands[0].ToString();
                string type = operands[1].ToString();
                if (type == "int") {
                    return GlobalData.Instance.GetInt(key);
                } else if (type == "float") {
                    return GlobalData.Instance.GetFloat(key);
                } else {
                    return GlobalData.Instance.GetStr(key);
                }
            }
            return 0;
        }
    }
}
