using System;
using System.Collections.Generic;
namespace StorySystem.CommonFunctions
{
    public sealed class GreaterThanOperator : IStoryFunction
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
            GreaterThanOperator val = new GreaterThanOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                float x = valX.GetFloat();
                float y = valY.GetFloat();
                m_Value = (x > y ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GreaterEqualThanOperator : IStoryFunction
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
            GreaterEqualThanOperator val = new GreaterEqualThanOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                float x = valX.GetFloat();
                float y = valY.GetFloat();
                m_Value = (x >= y ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class EqualOperator : IStoryFunction
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
            EqualOperator val = new EqualOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsString || valY.IsString) {
                    string x = valX.GetString();
                    string y = valY.GetString();
                    m_Value = (x == y ? 1 : 0);
                }
                else if (valX.IsObject && valY.IsObject) {
                    object x = valX.GetObject();
                    object y = valY.GetObject();
                    m_Value = (x == y ? 1 : 0);
                }
                else {
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = (x == y ? 1 : 0);
                }
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NotEqualOperator : IStoryFunction
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
            NotEqualOperator val = new NotEqualOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsString || valY.IsString) {
                    string x = valX.GetString();
                    string y = valY.GetString();
                    m_Value = (x != y ? 1 : 0);
                }
                else if (valX.IsObject && valY.IsObject) {
                    object x = valX.GetObject();
                    object y = valY.GetObject();
                    m_Value = (x != y ? 1 : 0);
                }
                else {
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = (x != y ? 1 : 0);
                }
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LessThanOperator : IStoryFunction
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
            LessThanOperator val = new LessThanOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                float x = valX.GetFloat();
                float y = valY.GetFloat();
                m_Value = (x < y ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LessEqualThanOperator : IStoryFunction
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
            LessEqualThanOperator val = new LessEqualThanOperator();
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
            m_Y.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                float x = valX.GetFloat();
                float y = valY.GetFloat();
                m_Value = (x <= y ? 1 : 0);
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private IStoryFunction m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class IsNullOperator : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            IsNullOperator val = new IsNullOperator();
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
            if (m_X.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                if (valX.IsObject || valX.IsString) {
                    m_Value = object.Equals(null, valX.ObjectVal);
                }
                else {
                    m_Value = false;
                }
            }
        }
        private IStoryFunction m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
