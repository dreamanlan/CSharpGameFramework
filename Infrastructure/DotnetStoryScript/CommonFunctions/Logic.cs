using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.CommonFunctions
{
    public sealed class AndOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                int x = m_X.Value.GetInt();
                if (x == 0) {
                    m_HaveValue = true;
                    m_Value = 0;
                } else {
                    m_Y.Evaluate(instance, handler, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = m_Y.Value.GetInt();
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
                int x = m_X.Value.GetInt();
                int y = m_Y.Value.GetInt();
                m_Value = ((x != 0 && y != 0) ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class OrOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                int x = m_X.Value.GetInt();
                if (x != 0) {
                    m_HaveValue = true;
                    m_Value = 1;
                } else {
                    m_Y.Evaluate(instance, handler, iterator, args);
                    if (m_Y.HaveValue) {
                        int y = m_Y.Value.GetInt();
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
                int x = m_X.Value.GetInt();
                int y = m_Y.Value.GetInt();
                m_Value = ((x != 0 || y != 0) ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NotOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                int x = m_X.Value.GetInt();
                m_Value = (x == 0 ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ConditionalOperator : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            var statementData = param as Dsl.StatementData;
            if (null != statementData) {
                Dsl.FunctionData funcData1 = statementData.First.AsFunction;
                Dsl.FunctionData funcData2 = statementData.Second.AsFunction;
                if (funcData1.IsHighOrder && funcData1.HaveLowerOrderParam() && funcData2.GetId() == ":" && funcData2.HaveParamOrStatement()) {
                    Dsl.ISyntaxComponent cond = funcData1.LowerOrderFunction.GetParam(0);
                    Dsl.ISyntaxComponent op1 = funcData1.GetParam(0);
                    Dsl.ISyntaxComponent op2 = funcData2.GetParam(0);
                    m_Op1.InitFromDsl(cond);
                    m_Op2.InitFromDsl(op1);
                    m_Op3.InitFromDsl(op2);
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            ConditionalOperator val = new ConditionalOperator();
            val.m_Op1 = m_Op1.Clone();
            val.m_Op2 = m_Op2.Clone();
            val.m_Op3 = m_Op3.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Op1.Evaluate(instance, handler, iterator, args);
            if (m_Op1.HaveValue) {
                if (m_Op1.Value.GetBool())
                    m_Op2.Evaluate(instance, handler, iterator, args);
                else
                    m_Op3.Evaluate(instance, handler, iterator, args);
            }
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }

        private void TryUpdateValue()
        {
            if (m_Op1.HaveValue) {
                m_HaveValue = true;
                var v1 = m_Op1.Value;
                BoxedValue v = v1.GetLong() != 0 ? m_Op2.Value : m_Op3.Value;
                m_Value = v;
            }
        }
        private IStoryFunction m_Op1 = new StoryFunction();
        private IStoryFunction m_Op2 = new StoryFunction();
        private IStoryFunction m_Op3 = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
