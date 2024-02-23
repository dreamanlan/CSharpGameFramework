using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Functions
{
    internal sealed class GetMemberCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetMemberCountFunction val = new GetMemberCountFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        m_Value = ui.MemberInfos.Count; 
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetMemberInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            GetMemberInfoFunction val = new GetMemberInfoFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_Index.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    int index = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (index >= 0 && index < ui.MemberInfos.Count) {
                            m_Value = BoxedValue.FromObject(ui.MemberInfos[index]);
                        } else {
                            m_Value = BoxedValue.NullObject;
                        }
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetFriendCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetFriendCountFunction val = new GetFriendCountFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        m_Value = ui.FriendInfos.Count;
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetFriendInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            GetFriendInfoFunction val = new GetFriendInfoFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_Index.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    var id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id.Type == BoxedValue.c_ULongType) {
                            ulong guid = id.GetULong();
                            m_Value = BoxedValue.FromObject(ui.FriendInfos.Find(fi => fi.FriendGuid == guid));
                        } else {
                            try {
                                int index = id.GetInt();
                                if (index >= 0 && index < ui.MemberInfos.Count) {
                                    m_Value = BoxedValue.FromObject(ui.FriendInfos[index]);
                                } else {
                                    m_Value = BoxedValue.NullObject;
                                }
                            } catch {
                                m_Value = BoxedValue.NullObject;
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction m_Index = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetItemCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetItemCountFunction val = new GetItemCountFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        m_Value = ui.ItemBag.ItemCount;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetFreeItemCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetFreeItemCountFunction val = new GetFreeItemCountFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        m_Value = ui.ItemBag.GetFreeCount();
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetItemInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            GetItemInfoFunction val = new GetItemInfoFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_Index.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    var id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id.Type == BoxedValue.c_ULongType) {
                            ulong guid = id.GetULong();
                            m_Value = BoxedValue.FromObject(ui.ItemBag.GetItemData(guid));
                        } else {
                            try {
                                int index = id.GetInt();
                                if (index >= 0 && index < ui.MemberInfos.Count) {
                                    m_Value = BoxedValue.FromObject(ui.FriendInfos[index]);
                                } else {
                                    m_Value = BoxedValue.NullObject;
                                }
                            } catch {
                                m_Value = BoxedValue.NullObject;
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction m_Index = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class FindItemInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            FindItemInfoFunction val = new FindItemInfoFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_Index.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    var id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id.Type == BoxedValue.c_ULongType) {
                            ulong guid = id.GetULong();
                            m_Value = BoxedValue.FromObject(ui.ItemBag.GetItemData(guid));
                        } else {
                            try {
                                int itemId = id.GetInt();
                                m_Value = BoxedValue.FromObject(ui.ItemBag.GetItemData(itemId));
                            } catch {
                                m_Value = BoxedValue.NullObject;
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction m_Index = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class CalcItemNumFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            CalcItemNumFunction val = new CalcItemNumFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_ItemId = m_ItemId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ItemId.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_ItemId.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        int itemId = m_ItemId.Value;
                        m_Value = ui.ItemBag.GetItemNum(itemId);
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction<int> m_ItemId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetUserDataFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
            }
        }
        public IStoryFunction Clone()
        {
            GetUserDataFunction val = new GetUserDataFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue && m_Key.HaveValue && m_Type.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    string key = m_Key.Value;
                    string type = m_Type.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (type == "int") {
                            int v;
                            ui.IntDatas.TryGetValue(key, out v);
                            m_Value = v;
                        } else if (type == "float") {
                            float v;
                            ui.FloatDatas.TryGetValue(key, out v);
                            m_Value = v;
                        } else {
                            string v;
                            ui.StringDatas.TryGetValue(key, out v);
                            m_Value = v;
                        }
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryFunction<string> m_Key = new StoryValue<string>();
        private IStoryFunction<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetGlobalDataFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_Key.InitFromDsl(callData.GetParam(0));
                m_Type.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            GetGlobalDataFunction val = new GetGlobalDataFunction();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_Key.HaveValue && m_Type.HaveValue) {
                    string key = m_Key.Value;
                    string type = m_Type.Value;
                    m_HaveValue = true;
                    if (type == "int") {
                        m_Value = GlobalData.Instance.GetInt(key);
                    } else if (type == "float") {
                        m_Value = GlobalData.Instance.GetFloat(key);
                    } else {
                        m_Value = GlobalData.Instance.GetStr(key);
                    }
                }
            }
        }
        private IStoryFunction<string> m_Key = new StoryValue<string>();
        private IStoryFunction<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
