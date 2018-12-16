using System;
using System.Collections.Generic;
namespace StorySystem.CommonValues
{
    internal sealed class GreaterThanOperator : IStoryValue<object>
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
            GreaterThanOperator val = new GreaterThanOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                float y = StoryValueHelper.CastTo<float>(objY);
                m_Value = (x > y ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GreaterEqualThanOperator : IStoryValue<object>
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
            GreaterEqualThanOperator val = new GreaterEqualThanOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                float y = StoryValueHelper.CastTo<float>(objY);
                m_Value = (x >= y ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class EqualOperator : IStoryValue<object>
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
            EqualOperator val = new EqualOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is string || objY is string) {
                    string x = StoryValueHelper.CastTo<string>(objX);
                    string y = StoryValueHelper.CastTo<string>(objY);
                    m_Value = (x == y ? 1 : 0);
                } else {
                    int x = (int)Convert.ChangeType(objX, typeof(int));
                    int y = (int)Convert.ChangeType(objY, typeof(int));
                    m_Value = (x == y ? 1 : 0);
                }
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class NotEqualOperator : IStoryValue<object>
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
            NotEqualOperator val = new NotEqualOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is string || objY is string) {
                    string x = StoryValueHelper.CastTo<string>(objX);
                    string y = StoryValueHelper.CastTo<string>(objY);
                    m_Value = (x != y ? 1 : 0);
                } else {
                    int x = (int)Convert.ChangeType(objX, typeof(int));
                    int y = (int)Convert.ChangeType(objY, typeof(int));
                    m_Value = (x != y ? 1 : 0);
                }
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class LessThanOperator : IStoryValue<object>
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
            LessThanOperator val = new LessThanOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                float y = StoryValueHelper.CastTo<float>(objY);
                m_Value = (x < y ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class LessEqualThanOperator : IStoryValue<object>
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
            LessEqualThanOperator val = new LessEqualThanOperator();
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
            m_Y.Evaluate(instance, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                float y = StoryValueHelper.CastTo<float>(objY);
                m_Value = (x <= y ? 1 : 0);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsNullOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            IsNullOperator val = new IsNullOperator();
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
                object objX = m_X.Value;
                m_Value = (null == objX);
            }
        }
        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
}
