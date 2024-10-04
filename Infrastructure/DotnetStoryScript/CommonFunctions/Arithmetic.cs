using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.CommonFunctions
{
    public sealed class AddOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                    string x = valX.GetString();
                    string y = valY.GetString();
                    m_Value = x + y;
                } else {
                    if (valX.IsInteger && valY.IsInteger) {
                        int x = valX.GetInt();
                        int y = valY.GetInt();
                        m_Value = x + y;
                    } else {
                        double x = valX.GetDouble();
                        double y = valY.GetDouble();
                        m_Value = x + y;
                    }
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SubOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = x - y;
                } else {
                    double x = valX.GetDouble();
                    double y = valY.GetDouble();
                    m_Value = x - y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class MulOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = x * y;
                } else {
                    double x = valX.GetDouble();
                    double y = valY.GetDouble();
                    m_Value = x * y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DivOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = x / y;
                } else {
                    double x = valX.GetDouble();
                    double y = valY.GetDouble();
                    m_Value = x / y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ModOperator : IStoryFunction
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
        public IStoryFunction Clone()
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
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = x % y;
                } else {
                    double x = valX.GetDouble();
                    double y = valY.GetDouble();
                    m_Value = x % y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class BitAndOperator : IStoryFunction
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
        public IStoryFunction Clone()
        {
            BitAndOperator val = new BitAndOperator();
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
                if (valX.IsInteger && valY.IsInteger) {
                    ulong x = valX.GetULong();
                    ulong y = valY.GetULong();
                    m_Value = x & y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class BitOrOperator : IStoryFunction
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
        public IStoryFunction Clone()
        {
            BitOrOperator val = new BitOrOperator();
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
                if (valX.IsInteger && valY.IsInteger) {
                    ulong x = valX.GetULong();
                    ulong y = valY.GetULong();
                    m_Value = x | y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class BitXorOperator : IStoryFunction
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
        public IStoryFunction Clone()
        {
            BitXorOperator val = new BitXorOperator();
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
                if (valX.IsInteger && valY.IsInteger) {
                    ulong x = valX.GetULong();
                    ulong y = valY.GetULong();
                    m_Value = x ^ y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class BitNotOperator : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            BitNotOperator val = new BitNotOperator();
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
                if (valX.IsInteger) {
                    ulong x = valX.GetULong();
                    m_Value = ~x;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LShiftOperator : IStoryFunction
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
        public IStoryFunction Clone()
        {
            LShiftOperator val = new LShiftOperator();
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
                if (valX.IsInteger && valY.IsInteger) {
                    ulong x = valX.GetULong();
                    int y = valY.GetInt();
                    m_Value = x << y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class RShiftOperator : IStoryFunction
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
        public IStoryFunction Clone()
        {
            RShiftOperator val = new RShiftOperator();
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
                if (valX.IsInteger && valY.IsInteger) {
                    ulong x = valX.GetULong();
                    int y = valY.GetInt();
                    m_Value = x >> y;
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class AbsFunction : IStoryFunction
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
            AbsFunction val = new AbsFunction();
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
                    int x = valX.GetInt();
                    m_Value = Math.Abs(x);
                } else {
                    double x = valX.GetDouble();
                    m_Value = Math.Abs(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class FloorFunction : IStoryFunction
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
            FloorFunction val = new FloorFunction();
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
                    int x = valX.GetInt();
                    m_Value = x;
                } else {
                    double x = valX.GetDouble();
                    m_Value = Math.Floor(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CeilingFunction : IStoryFunction
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
            CeilingFunction val = new CeilingFunction();
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
                    int x = valX.GetInt();
                    m_Value = x;
                } else {
                    double x = valX.GetDouble();
                    m_Value = Math.Ceiling(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class RoundFunction : IStoryFunction
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
            RoundFunction val = new RoundFunction();
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
                    int x = valX.GetInt();
                    m_Value = x;
                } else {
                    double x = valX.GetDouble();
                    m_Value = Math.Round(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class FloorToIntFunction : IStoryFunction
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
            FloorToIntFunction val = new FloorToIntFunction();
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
                if (valX.IsInteger) {
                    int x = valX.GetInt();
                    m_Value = x;
                }
                else {
                    double x = valX.GetDouble();
                    m_Value = (int)Math.Floor(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CeilingToIntFunction : IStoryFunction
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
            CeilingToIntFunction val = new CeilingToIntFunction();
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
                if (valX.IsInteger) {
                    int x = valX.GetInt();
                    m_Value = x;
                }
                else {
                    double x = valX.GetDouble();
                    m_Value = (int)Math.Ceiling(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class RoundToIntFunction : IStoryFunction
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
            RoundToIntFunction val = new RoundToIntFunction();
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
                if (valX.IsInteger) {
                    int x = valX.GetInt();
                    m_Value = x;
                }
                else {
                    double x = valX.GetDouble();
                    m_Value = (int)Math.Round(x);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class BoolFunction : IStoryFunction
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
            BoolFunction val = new BoolFunction();
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
                m_Value = valX.GetBool();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SByteFunction : IStoryFunction
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
            SByteFunction val = new SByteFunction();
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
                m_Value = valX.GetSByte();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ByteFunction : IStoryFunction
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
            ByteFunction val = new ByteFunction();
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
                m_Value = valX.GetByte();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CharFunction : IStoryFunction
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
            CharFunction val = new CharFunction();
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
                m_Value = valX.GetChar();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ShortFunction : IStoryFunction
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
            ShortFunction val = new ShortFunction();
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
                m_Value = valX.GetShort();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class UShortFunction : IStoryFunction
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
            UShortFunction val = new UShortFunction();
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
                m_Value = valX.GetUShort();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class IntFunction : IStoryFunction
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
            IntFunction val = new IntFunction();
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
                m_Value = valX.GetInt();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class UIntFunction : IStoryFunction
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
            UIntFunction val = new UIntFunction();
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
                m_Value = valX.GetUInt();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LongFunction : IStoryFunction
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
            LongFunction val = new LongFunction();
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
                m_Value = valX.GetLong();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ULongFunction : IStoryFunction
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
            ULongFunction val = new ULongFunction();
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
                m_Value = valX.GetULong();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class FloatFunction : IStoryFunction
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
            FloatFunction val = new FloatFunction();
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
                m_Value = valX.GetFloat();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DoubleFunction : IStoryFunction
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
            DoubleFunction val = new DoubleFunction();
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
                m_Value = valX.GetDouble();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DecimalFunction : IStoryFunction
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
            DecimalFunction val = new DecimalFunction();
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
                m_Value = valX.GetDecimal();
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class FtoiFunction : IStoryFunction
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
            FtoiFunction val = new FtoiFunction();
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
                float v1 = valX.GetFloat();
                int v2 = 0;
                unsafe {
                    v2 = *(int*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ItofFunction : IStoryFunction
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
            ItofFunction val = new ItofFunction();
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
                int v1 = valX.GetInt();
                float v2 = 0;
                unsafe {
                    v2 = *(float*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class FtouFunction : IStoryFunction
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
            FtouFunction val = new FtouFunction();
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
                float v1 = valX.GetFloat();
                uint v2 = 0;
                unsafe {
                    v2 = *(uint*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class UtofFunction : IStoryFunction
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
            UtofFunction val = new UtofFunction();
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
                uint v1 = valX.GetUInt();
                float v2 = 0;
                unsafe {
                    v2 = *(float*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DtolFunction : IStoryFunction
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
            DtolFunction val = new DtolFunction();
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
                double v1 = valX.GetDouble();
                long v2 = 0;
                unsafe {
                    v2 = *(long*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LtodFunction : IStoryFunction
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
            LtodFunction val = new LtodFunction();
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
                long v1 = valX.GetLong();
                double v2 = 0;
                unsafe {
                    v2 = *(double*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DtouFunction : IStoryFunction
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
            DtouFunction val = new DtouFunction();
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
                double v1 = valX.GetDouble();
                ulong v2 = 0;
                unsafe {
                    v2 = *(ulong*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class UtodFunction : IStoryFunction
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
            UtodFunction val = new UtodFunction();
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
                ulong v1 = valX.GetULong();
                double v2 = 0;
                unsafe {
                    v2 = *(double*)&v1;
                }
                m_Value = v2;
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LerpFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Z.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            LerpFunction val = new LerpFunction();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_Z = m_Z.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            m_Y.Evaluate(instance, handler, iterator, args);
            m_Z.Evaluate(instance, handler, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue && m_Z.HaveValue) {
                m_HaveValue = true;
                var a = m_X.Value;
                var b = m_Y.Value;
                var t = m_Z.Value;
                m_Value = a + (b - a) * ClampFunction.Clamp01(t);
            }
        }
        private IStoryFunction<double> m_X = new StoryFunction<double>();
        private IStoryFunction<double> m_Y = new StoryFunction<double>();
        private IStoryFunction<double> m_Z = new StoryFunction<double>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LerpUnclampedFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Z.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            LerpUnclampedFunction val = new LerpUnclampedFunction();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_Z = m_Z.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            m_Y.Evaluate(instance, handler, iterator, args);
            m_Z.Evaluate(instance, handler, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue && m_Z.HaveValue) {
                m_HaveValue = true;
                var a = m_X.Value;
                var b = m_Y.Value;
                var t = m_Z.Value;
                m_Value = a + (b - a) * t;
            }
        }
        private IStoryFunction<double> m_X = new StoryFunction<double>();
        private IStoryFunction<double> m_Y = new StoryFunction<double>();
        private IStoryFunction<double> m_Z = new StoryFunction<double>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LerpAngleFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Z.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            LerpAngleFunction val = new LerpAngleFunction();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_Z = m_Z.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            m_Y.Evaluate(instance, handler, iterator, args);
            m_Z.Evaluate(instance, handler, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue && m_Z.HaveValue) {
                m_HaveValue = true;
                var a = m_X.Value;
                var b = m_Y.Value;
                var t = m_Z.Value;
                double num = Repeat(b - a, 360.0);
                if (num > 180f) {
                    num -= 360f;
                }
                m_Value = a + num * ClampFunction.Clamp01(t);
            }
        }
        private IStoryFunction<double> m_X = new StoryFunction<double>();
        private IStoryFunction<double> m_Y = new StoryFunction<double>();
        private IStoryFunction<double> m_Z = new StoryFunction<double>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public static double Repeat(double t, double length)
        {
            return ClampFunction.Clamp(t - Math.Floor(t / length) * length, 0f, length);
        }
    }
    public sealed class SmoothStepFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Z.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            SmoothStepFunction val = new SmoothStepFunction();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_Z = m_Z.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            m_Y.Evaluate(instance, handler, iterator, args);
            m_Z.Evaluate(instance, handler, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue && m_Z.HaveValue) {
                m_HaveValue = true;
                var from = m_X.Value;
                var to = m_Y.Value;
                var t = m_Z.Value;
                t = ClampFunction.Clamp01(t);
                t = -2.0 * t * t * t + 3.0 * t * t;
                m_Value = to * t + from * (1.0 - t);
            }
        }
        private IStoryFunction<double> m_X = new StoryFunction<double>();
        private IStoryFunction<double> m_Y = new StoryFunction<double>();
        private IStoryFunction<double> m_Z = new StoryFunction<double>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Clamp01Function : IStoryFunction
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
            Clamp01Function val = new Clamp01Function();
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
                if (valX.IsInteger) {
                    int v = valX.GetInt();
                    m_Value = ClampFunction.Clamp01(v);
                }
                else {
                    double v = valX.GetDouble();
                    m_Value = ClampFunction.Clamp01(v);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ClampFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Z.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            ClampFunction val = new ClampFunction();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_Z = m_Z.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X.Evaluate(instance, handler, iterator, args);
            m_Y.Evaluate(instance, handler, iterator, args);
            m_Z.Evaluate(instance, handler, iterator, args);
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
            if (m_X.HaveValue && m_Y.HaveValue && m_Z.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                var valZ = m_Z.Value;
                if (valX.IsInteger) {
                    int v = valX.GetInt();
                    int v1 = valY.GetInt();
                    int v2 = valZ.GetInt();
                    m_Value = Clamp(v, v1, v2);
                }
                else {
                    double v = valX.GetDouble();
                    double v1 = valY.GetDouble();
                    double v2 = valZ.GetDouble();
                    m_Value = Clamp(v, v1, v2);
                }
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private IStoryFunction m_Z = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) {
                value = min;
            }
            else if (value > max) {
                value = max;
            }
            return value;
        }
        public static int Clamp01(int value)
        {
            if (value < 0) {
                return 0;
            }
            if (value > 1) {
                return 1;
            }
            return value;
        }
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) {
                value = min;
            }
            else if (value > max) {
                value = max;
            }
            return value;
        }
        public static double Clamp01(double value)
        {
            if (value < 0f) {
                return 0f;
            }
            if (value > 1f) {
                return 1f;
            }
            return value;
        }
    }
    public sealed class ApproximatelyFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            ApproximatelyFunction val = new ApproximatelyFunction();
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
                double v1 = m_X.Value;
                double v2 = m_Y.Value;
                m_Value = Approximately(v1, v2) ? 1 : 0;
            }
        }
        private IStoryFunction<double> m_X = new StoryFunction<double>();
        private IStoryFunction<double> m_Y = new StoryFunction<double>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public static bool Approximately(double a, double b)
        {
            return Math.Abs(b - a) < Math.Max(1E-06 * Math.Max(Math.Abs(a), Math.Abs(b)), double.Epsilon * 8.0);
        }
    }
    public sealed class IsPowerOfTwoFunction : IStoryFunction
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
            IsPowerOfTwoFunction val = new IsPowerOfTwoFunction();
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
                m_Value = IsPowerOfTwo(valX) ? 1 : 0;
            }
        }
        private IStoryFunction<int> m_X = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public bool IsPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n)) == v;
        }
    }
    public sealed class ClosestPowerOfTwoFunction : IStoryFunction
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
            ClosestPowerOfTwoFunction val = new ClosestPowerOfTwoFunction();
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
                m_Value = ClosestPowerOfTwo(valX);
            }
        }
        private IStoryFunction<int> m_X = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public int ClosestPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n));
        }
    }
    public sealed class NextPowerOfTwoFunction : IStoryFunction
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
            NextPowerOfTwoFunction val = new NextPowerOfTwoFunction();
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
                m_Value = NextPowerOfTwo(valX);
            }
        }
        private IStoryFunction<int> m_X = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        public int NextPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n + 1));
        }
    }
    public sealed class DistFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 4) {
                m_X1.InitFromDsl(callData.GetParam(0));
                m_Y1.InitFromDsl(callData.GetParam(1));
                m_X2.InitFromDsl(callData.GetParam(2));
                m_Y2.InitFromDsl(callData.GetParam(3));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            DistFunction val = new DistFunction();
            val.m_X1 = m_X1.Clone();
            val.m_Y1 = m_Y1.Clone();
            val.m_X2 = m_X2.Clone();
            val.m_Y2 = m_Y2.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X1.Evaluate(instance, handler, iterator, args);
            m_Y1.Evaluate(instance, handler, iterator, args);
            m_X2.Evaluate(instance, handler, iterator, args);
            m_Y2.Evaluate(instance, handler, iterator, args);
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
            if (m_X1.HaveValue && m_Y1.HaveValue && m_X2.HaveValue && m_Y2.HaveValue) {
                m_HaveValue = true;
                float x1 = m_X1.Value;
                float y1 = m_Y1.Value;
                float x2 = m_X2.Value;
                float y2 = m_Y2.Value;
                m_Value = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }
        }
        private IStoryFunction<float> m_X1 = new StoryFunction<float>();
        private IStoryFunction<float> m_Y1 = new StoryFunction<float>();
        private IStoryFunction<float> m_X2 = new StoryFunction<float>();
        private IStoryFunction<float> m_Y2 = new StoryFunction<float>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DistSqrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 4) {
                m_X1.InitFromDsl(callData.GetParam(0));
                m_Y1.InitFromDsl(callData.GetParam(1));
                m_X2.InitFromDsl(callData.GetParam(2));
                m_Y2.InitFromDsl(callData.GetParam(3));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            DistSqrFunction val = new DistSqrFunction();
            val.m_X1 = m_X1.Clone();
            val.m_Y1 = m_Y1.Clone();
            val.m_X2 = m_X2.Clone();
            val.m_Y2 = m_Y2.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_X1.Evaluate(instance, handler, iterator, args);
            m_Y1.Evaluate(instance, handler, iterator, args);
            m_X2.Evaluate(instance, handler, iterator, args);
            m_Y2.Evaluate(instance, handler, iterator, args);
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
            if (m_X1.HaveValue && m_Y1.HaveValue && m_X2.HaveValue && m_Y2.HaveValue) {
                m_HaveValue = true;
                float x1 = m_X1.Value;
                float y1 = m_Y1.Value;
                float x2 = m_X2.Value;
                float y2 = m_Y2.Value;
                m_Value = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            }
        }
        private IStoryFunction<float> m_X1 = new StoryFunction<float>();
        private IStoryFunction<float> m_Y1 = new StoryFunction<float>();
        private IStoryFunction<float> m_X2 = new StoryFunction<float>();
        private IStoryFunction<float> m_Y2 = new StoryFunction<float>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class PowFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            PowFunction val = new PowFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Exp(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                if (valX.IsInteger && valY.IsInteger) {
                    int x = valX.GetInt();
                    int y = valY.GetInt();
                    m_Value = (int)Math.Pow(x, y);
                } else {
                    double x = valX.GetDouble();
                    double y = valY.GetDouble();
                    m_Value = Math.Pow(x, y);
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LogFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            LogFunction val = new LogFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Log(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                var valX = m_X.Value;
                var valY = m_Y.Value;
                double x = valX.GetDouble();
                double y = valY.GetDouble();
                m_Value = Math.Log(x, y);
            }
        }
        private int m_ParamNum = 0;
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Log2Function : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            Log2Function val = new Log2Function();
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
                double x = valX.GetDouble();
                m_Value = Math.Log(x) / Math.Log(2);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Log10Function : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            Log10Function val = new Log10Function();
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
                double x = valX.GetDouble();
                m_Value = Math.Log10(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SqrtFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            SqrtFunction val = new SqrtFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Sqrt(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ExpFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            ExpFunction val = new ExpFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Exp(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Exp2Function : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            Exp2Function val = new Exp2Function();
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
                double x = valX.GetDouble();
                m_Value = Math.Pow(2, x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SinFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            SinFunction val = new SinFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Sin(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CosFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            CosFunction val = new CosFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Cos(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class TanFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            TanFunction val = new TanFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Tan(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class AsinFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            AsinFunction val = new AsinFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Asin(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class AcosFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            AcosFunction val = new AcosFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Acos(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class AtanFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            AtanFunction val = new AtanFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Atan(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Atan2Function : IStoryFunction
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
            Atan2Function val = new Atan2Function();
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
                double x = valX.GetDouble();
                double y = valY.GetDouble();
                m_Value = Math.Atan2(x, y);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private IStoryFunction m_Y = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SinhFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            SinhFunction val = new SinhFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Sinh(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CoshFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            CoshFunction val = new CoshFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Cosh(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class TanhFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            TanhFunction val = new TanhFunction();
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
                double x = valX.GetDouble();
                m_Value = Math.Tanh(x);
            }
        }
        private IStoryFunction m_X = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class MinFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryFunction val = new StoryFunction();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            MinFunction val = new MinFunction();
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
                    double minV = double.MaxValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        double v = m_List[i].Value;
                        if (minV > v)
                            minV = v;
                    }
                    m_Value = minV;
                }
            }
        }
        private List<IStoryFunction> m_List = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class MaxFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryFunction val = new StoryFunction();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            MaxFunction val = new MaxFunction();
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
                    double maxV = double.MinValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        double v = m_List[i].Value;
                        if (maxV < v)
                            maxV = v;
                    }
                    m_Value = maxV;
                }
            }
        }
        private List<IStoryFunction> m_List = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
