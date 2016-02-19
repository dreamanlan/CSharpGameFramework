using System;
using System.Collections.Generic;
using GameFramework;

namespace StorySystem.CommonValues
{
    internal sealed class TimeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "time") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            TimeValue val = new TimeValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
        }
        public void Evaluate(StoryInstance instance)
        {
            m_Value = (int)TimeUtility.GetLocalMilliseconds();
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
