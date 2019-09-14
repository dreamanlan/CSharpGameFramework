using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class GetMemberCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            GetMemberCountValue val = new GetMemberCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetMemberInfoValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            GetMemberInfoValue val = new GetMemberInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
                            m_Value = ui.MemberInfos[index];
                        } else {
                            m_Value = null;
                        }
                    }
                }
            }
        }
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetFriendCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            GetFriendCountValue val = new GetFriendCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetFriendInfoValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            GetFriendInfoValue val = new GetFriendInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
                    object id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id is ulong) {
                            ulong guid = (ulong)id;
                            m_Value = ui.FriendInfos.Find(fi => fi.FriendGuid == guid);
                        } else {
                            try {
                                int index = (int)id;
                                if (index >= 0 && index < ui.MemberInfos.Count) {
                                    m_Value = ui.FriendInfos[index];
                                } else {
                                    m_Value = null;
                                }
                            } catch {
                                m_Value = null;
                            }
                        }
                    }
                }
            }
        }
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetItemCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            GetItemCountValue val = new GetItemCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetFreeItemCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            GetFreeItemCountValue val = new GetFreeItemCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetItemInfoValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            GetItemInfoValue val = new GetItemInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
                    object id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id is ulong) {
                            ulong guid = (ulong)id;
                            m_Value = ui.ItemBag.GetItemData(guid);
                        } else {
                            try {
                                int index = (int)id;
                                if (index >= 0 && index < ui.MemberInfos.Count) {
                                    m_Value = ui.FriendInfos[index];
                                } else {
                                    m_Value = null;
                                }
                            } catch {
                                m_Value = null;
                            }
                        }
                    }
                }
            }
        }
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class FindItemInfoValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            FindItemInfoValue val = new FindItemInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
                    object id = m_Index.Value;
                    m_HaveValue = true;
                    UserInfo ui = userThread.GetUserInfo(userGuid);
                    if (null != ui) {
                        if (id is ulong) {
                            ulong guid = (ulong)id;
                            m_Value = ui.ItemBag.GetItemData(guid);
                        } else {
                            try {
                                int itemId = (int)id;
                                m_Value = ui.ItemBag.GetItemData(itemId);
                            } catch {
                                m_Value = null;
                            }
                        }
                    }
                }
            }
        }
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class CalcItemNumValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            CalcItemNumValue val = new CalcItemNumValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_ItemId = m_ItemId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_ItemId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetUserDataValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
            }
        }
        public IStoryValue Clone()
        {
            GetUserDataValue val = new GetUserDataValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetGlobalDataValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_Key.InitFromDsl(callData.GetParam(0));
                m_Type.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            GetGlobalDataValue val = new GetGlobalDataValue();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
        public object Value
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
        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
