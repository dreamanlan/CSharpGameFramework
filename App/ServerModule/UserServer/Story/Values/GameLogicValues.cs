using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class GetMemberCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmembercount" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMemberCountValue val = new GetMemberCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetMemberInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmemberinfo" && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag | m_Index.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMemberInfoValue val = new GetMemberInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_Index.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetFriendCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getfriendcount" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetFriendCountValue val = new GetFriendCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetFriendInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getfriendinfo" && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag | m_Index.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetFriendInfoValue val = new GetFriendInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_Index.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<object> m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetItemCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getitemcount" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetItemCountValue val = new GetItemCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetFreeItemCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getfreeitemcount" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetFreeItemCountValue val = new GetFreeItemCountValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetItemInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getiteminfo" && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag | m_Index.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetItemInfoValue val = new GetItemInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_Index.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<object> m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class FindItemInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "finditeminfo" && callData.GetParamNum() == 2) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Index.InitFromDsl(callData.GetParam(1));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag | m_Index.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            FindItemInfoValue val = new FindItemInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_Index.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<object> m_Index = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class CalcItemNumValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "calcitemcount" && callData.GetParamNum() == 2) {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_ItemId.InitFromDsl(callData.GetParam(1));
                m_Flag |= m_UserGuid.Flag;
                m_Flag |= m_ItemId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            CalcItemNumValue val = new CalcItemNumValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_ItemId = m_ItemId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_ItemId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_ItemId.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<int> m_ItemId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetUserDataValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getuserdata" && callData.GetParamNum() == 3) {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
                m_Flag |= m_UserGuid.Flag;
                m_Flag |= m_Key.Flag;
                m_Flag |= m_Type.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUserDataValue val = new GetUserDataValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_UserGuid.Substitute(iterator, args);
                m_Key.Substitute(iterator, args);
                m_Type.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_Key.Evaluate(instance);
            m_Type.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetGlobalDataValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getglobaldata" && callData.GetParamNum() == 2) {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_Key.InitFromDsl(callData.GetParam(0));
                m_Type.InitFromDsl(callData.GetParam(1));
                m_Flag |= m_Key.Flag;
                m_Flag |= m_Type.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetGlobalDataValue val = new GetGlobalDataValue();
            val.m_Key = m_Key.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_Key.Substitute(iterator, args);
                m_Type.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_Key.Evaluate(instance);
            m_Type.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<string> m_Key = new StoryValue<string>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
