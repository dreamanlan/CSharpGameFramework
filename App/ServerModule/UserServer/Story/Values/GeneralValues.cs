using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class GetUserInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getuserinfo" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UserGuid.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUserInfoValue val = new GetUserInfoValue();
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
                    m_Value = UserServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
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
}
