using System;
using System.Collections.Generic;
using StorySystem;
namespace StorySystem.CommonValues
{
    internal sealed class AndOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            AndOperator val = new AndOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, iterator, args);
            if (m_X.HaveValue) {
                int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
                if (x == 0) {
                    m_HaveValue = true;
                    m_Value = 0;
                } else {
                    m_Y.Evaluate(instance, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
                        m_HaveValue = true;
                        m_Value = (y != 0 ? 1 : 0);
                    }
                }
            }
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

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
                int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
                m_Value = ((x != 0 && y != 0) ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class OrOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            OrOperator val = new OrOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, iterator, args);
            if (m_X.HaveValue) {
                int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
                if (x != 0) {
                    m_HaveValue = true;
                    m_Value = 1;
                } else {
                    m_Y.Evaluate(instance, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
                        m_HaveValue = true;
                        m_Value = (y != 0 ? 1 : 0);
                    }
                }
            }
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

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
                int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
                m_Value = ((x != 0 || y != 0) ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class NotOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                if (callData.GetParamNum() > 0) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            NotOperator val = new NotOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, iterator, args);
            TryUpdateValue();
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

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
                m_Value = (x == 0 ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
}
