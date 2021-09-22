using System;
using System.Collections.Generic;
using StorySystem;
namespace StorySystem.CommonValues
{
    internal sealed class AddOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(new Dsl.ValueData("0"));
                    m_Y.InitFromDsl(callData.GetParam(0));
                } else if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            AddOperator val = new AddOperator();
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
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsString || valY.IsString) {
                    string x = valX.Get<string>();
                    string y = valY.Get<string>();
                    m_Value = x + y;
                } else {
                    if (valX.IsInteger && valY.IsInteger) {
                        int x = valX.Get<int>();
                        int y = valY.Get<int>();
                        m_Value = x + y;
                    } else {
                        float x = valX.Get<float>();
                        float y = valY.Get<float>();
                        m_Value = x + y;
                    }
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class SubOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(new Dsl.ValueData("0"));
                    m_Y.InitFromDsl(callData.GetParam(0));
                } else if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            SubOperator val = new SubOperator();
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
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.Get<int>();
                    int y = valY.Get<int>();
                    m_Value = x - y;
                } else {
                    float x = valX.Get<float>();
                    float y = valY.Get<float>();
                    m_Value = x - y;
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class MulOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            MulOperator val = new MulOperator();
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
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.Get<int>();
                    int y = valY.Get<int>();
                    m_Value = x * y;
                } else {
                    float x = valX.Get<float>();
                    float y = valY.Get<float>();
                    m_Value = x * y;
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class DivOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            DivOperator val = new DivOperator();
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
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.Get<int>();
                    int y = valY.Get<int>();
                    m_Value = x / y;
                } else {
                    float x = valX.Get<float>();
                    float y = valY.Get<float>();
                    m_Value = x / y;
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class ModOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            ModOperator val = new ModOperator();
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
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.Get<int>();
                    int y = valY.Get<int>();
                    m_Value = x % y;
                } else {
                    float x = valX.Get<float>();
                    float y = valY.Get<float>();
                    m_Value = x % y;
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class AbsOperator : IStoryValue
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
        public IStoryValue Clone()
        {
            AbsOperator val = new AbsOperator();
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
                var valX = m_X.Value;
                if (valX.IsInteger) {
                    int x = valX.Get<int>();
                    m_Value = Math.Abs(x);
                } else {
                    float x = valX.Get<float>();
                    m_Value = Math.Abs(x);
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class FloorOperator : IStoryValue
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
        public IStoryValue Clone()
        {
            FloorOperator val = new FloorOperator();
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
                var valX = m_X.Value;
                if (valX.IsInteger) {
                    int x = valX.Get<int>();
                    m_Value = x;
                } else {
                    float x = valX.Get<float>();
                    m_Value = (int)Math.Floor(x);
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class CeilingOperator : IStoryValue
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
        public IStoryValue Clone()
        {
            CeilingOperator val = new CeilingOperator();
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
                var valX = m_X.Value;
                if (valX.IsInteger) {
                    int x = valX.Get<int>();
                    m_Value = x;
                } else {
                    float x = valX.Get<float>();
                    m_Value = (int)Math.Ceiling(x);
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class RoundOperator : IStoryValue
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
        public IStoryValue Clone()
        {
            RoundOperator val = new RoundOperator();
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
                var valX = m_X.Value;
                if (valX.IsInteger) {
                    int x = valX.Get<int>();
                    m_Value = x;
                } else {
                    float x = valX.Get<float>();
                    m_Value = (int)Math.Round(x);
                }
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class PowOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (1 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                } else if (2 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            PowOperator val = new PowOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0)
                m_X.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 1)
                m_Y.Evaluate(instance, handler, iterator, args);
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
            if (1 == m_ParamNum && m_X.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Exp(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.Get<int>();
                    int y = valY.Get<int>();
                    m_Value = (int)Math.Pow(x, y);
                } else {
                    float x = valX.Get<float>();
                    float y = valY.Get<float>();
                    m_Value = (float)Math.Pow(x, y);
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class LogOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (1 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                } else if (2 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            LogOperator val = new LogOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0)
                m_X.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 1)
                m_Y.Evaluate(instance, handler, iterator, args);
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
            if (1 == m_ParamNum && m_X.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Log(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                float x = valX.Get<float>();
                float y = valY.Get<float>();
                m_Value = (float)Math.Log(x, y);
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue m_X = new StoryValue();
        private IStoryValue m_Y = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class SqrtOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            SqrtOperator val = new SqrtOperator();
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
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Sqrt(x);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class SinOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            SinOperator val = new SinOperator();
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
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Sin(x);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class CosOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            CosOperator val = new CosOperator();
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
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Cos(x);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class SinhOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            SinhOperator val = new SinhOperator();
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
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Sinh(x);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class CoshOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            CoshOperator val = new CoshOperator();
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
                var valX = m_X.Value;
                float x = valX.Get<float>();
                m_Value = (float)Math.Cosh(x);
            }
        }
        private IStoryValue m_X = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class MinOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            MinOperator val = new MinOperator();
            for (int i = 0; i < m_List.Count; i++) {
                val.m_List.Add(m_List[i]);
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            for (int i = 0; i < m_List.Count; i++) {
                m_List[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool isInt = true;
            bool canCalc = true;
            for (int i = 0; i < m_List.Count; i++) {
                if (m_List[i].HaveValue) {
                    if (!(m_List[i].Value.IsInteger)) {
                        isInt = false;
                    }
                } else {
                    canCalc = false;
                    break;
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                if (isInt) {
                    int minV = int.MaxValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        int v = m_List[i].Value;
                        if (minV > v)
                            minV = v;
                    }
                    m_Value = minV;
                } else {
                    float minV = float.MaxValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        float v = m_List[i].Value;
                        if (minV > v)
                            minV = v;
                    }
                    m_Value = minV;
                }
            }
        }
        private List<IStoryValue> m_List = new List<IStoryValue>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class MaxOperator : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            MaxOperator val = new MaxOperator();
            for (int i = 0; i < m_List.Count; i++) {
                val.m_List.Add(m_List[i]);
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            for (int i = 0; i < m_List.Count; i++) {
                m_List[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool isInt = true;
            bool canCalc = true;
            for (int i = 0; i < m_List.Count; i++) {
                if (m_List[i].HaveValue) {
                    if (!(m_List[i].Value.IsInteger)) {
                        isInt = false;
                    }
                } else {
                    canCalc = false;
                    break;
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                if (isInt) {
                    int maxV = int.MinValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        int v = m_List[i].Value;
                        if (maxV < v)
                            maxV = v;
                    }
                    m_Value = maxV;
                } else {
                    float maxV = float.MinValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        float v = m_List[i].Value;
                        if (maxV < v)
                            maxV = v;
                    }
                    m_Value = maxV;
                }
            }
        }
        private List<IStoryValue> m_List = new List<IStoryValue>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
