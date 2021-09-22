using System;
using System.Collections.Generic;
using StorySystem;
namespace StorySystem.CommonValues
{
    internal sealed class AndOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            AndOperator val = new AndOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            if (m_X.HaveValue) {
                int x = m_X.Value.Get<int>();
                if (x == 0) {
                    m_HaveValue = true;
                    m_Value = 0;
                } else {
                    m_Y.Evaluate(instance, handler, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = m_Y.Value.Get<int>();
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
        public BoxedValue Value
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
                int x = m_X.Value.Get<int>();
                int y = m_Y.Value.Get<int>();
                m_Value = ((x != 0 && y != 0) ? 1 : 0);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class OrOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            OrOperator val = new OrOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            if (m_X.HaveValue) {
                int x = m_X.Value.Get<int>();
                if (x != 0) {
                    m_HaveValue = true;
                    m_Value = 1;
                } else {
                    m_Y.Evaluate(instance, handler, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = m_Y.Value.Get<int>();
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
        public BoxedValue Value
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
                int x = m_X.Value.Get<int>();
                int y = m_Y.Value.Get<int>();
                m_Value = ((x != 0 || y != 0) ? 1 : 0);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NotOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            NotOperator val = new NotOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                int x = m_X.Value.Get<int>();
                m_Value = (x == 0 ? 1 : 0);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
