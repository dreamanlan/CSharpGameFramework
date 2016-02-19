using System;
using System.Collections.Generic;
using StorySystem;

namespace StorySystem.CommonValues
{
    internal sealed class AddOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "+") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(new Dsl.ValueData("0"));
                    m_Y.InitFromDsl(callData.GetParam(0));
                } else if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                m_Flag = m_X.Flag | m_Y.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            AddOperator val = new AddOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
                m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
            m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
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
                    m_Value = x + y;
                } else {
                    if (objX is int && objY is int) {
                        int x = StoryValueHelper.CastTo<int>(objX);
                        int y = StoryValueHelper.CastTo<int>(objY);
                        m_Value = x + y;
                    } else {
                        float x = StoryValueHelper.CastTo<float>(objX);
                        float y = StoryValueHelper.CastTo<float>(objY);
                        m_Value = x + y;
                    }
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class SubOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "-") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(new Dsl.ValueData("0"));
                    m_Y.InitFromDsl(callData.GetParam(0));
                } else if (callData.GetParamNum() == 2) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                }
                m_Flag = m_X.Flag | m_Y.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            SubOperator val = new SubOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
                m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
            m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is int && objY is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    int y = StoryValueHelper.CastTo<int>(objY);
                    m_Value = x - y;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    float y = StoryValueHelper.CastTo<float>(objY);
                    m_Value = x - y;
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class MulOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "*" && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Flag = m_X.Flag | m_Y.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            MulOperator val = new MulOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
                m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
            m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is int && objY is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    int y = StoryValueHelper.CastTo<int>(objY);
                    m_Value = x * y;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    float y = StoryValueHelper.CastTo<float>(objY);
                    m_Value = x * y;
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class DivOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "/" && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Flag = m_X.Flag | m_Y.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            DivOperator val = new DivOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
                m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
            m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is int && objY is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    int y = StoryValueHelper.CastTo<int>(objY);
                    m_Value = x / y;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    float y = StoryValueHelper.CastTo<float>(objY);
                    m_Value = x / y;
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class ModOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "%" && callData.GetParamNum() == 2) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Y.InitFromDsl(callData.GetParam(1));
                m_Flag = m_X.Flag | m_Y.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            ModOperator val = new ModOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
                m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
            m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is int && objY is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    int y = StoryValueHelper.CastTo<int>(objY);
                    m_Value = x % y;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    float y = StoryValueHelper.CastTo<float>(objY);
                    m_Value = x % y;
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class AbsOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "abs") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            AbsOperator val = new AbsOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                if (objX is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    m_Value = Math.Abs(x);
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    m_Value = Math.Abs(x);
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class FloorOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "floor") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            FloorOperator val = new FloorOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                if (objX is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    m_Value = x;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    m_Value = (int)Math.Floor(x);
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class CeilingOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "ceiling") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            CeilingOperator val = new CeilingOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                if (objX is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    m_Value = x;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    m_Value = (int)Math.Ceiling(x);
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class RoundOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "round") {
                if (callData.GetParamNum() == 1) {
                    m_X.InitFromDsl(callData.GetParam(0));
                }
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            RoundOperator val = new RoundOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                if (objX is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    m_Value = x;
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    m_Value = (int)Math.Round(x);
                }
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class PowOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "pow") {
                m_ParamNum = callData.GetParamNum();
                if (1 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Flag = m_X.Flag;
                } else if (2 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                    m_Flag = m_X.Flag | m_Y.Flag;
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            PowOperator val = new PowOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                if (m_ParamNum > 0)
                    m_X.Substitute(iterator, args);
                if (m_ParamNum > 1)
                    m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0)
                m_X.Evaluate(instance);
            if (m_ParamNum > 1)
                m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (1 == m_ParamNum && m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Exp(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                if (objX is int && objY is int) {
                    int x = StoryValueHelper.CastTo<int>(objX);
                    int y = StoryValueHelper.CastTo<int>(objY);
                    m_Value = (int)Math.Pow(x, y);
                } else {
                    float x = StoryValueHelper.CastTo<float>(objX);
                    float y = StoryValueHelper.CastTo<float>(objY);
                    m_Value = (float)Math.Pow(x, y);
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class LogOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "log") {
                m_ParamNum = callData.GetParamNum();
                if (1 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Flag = m_X.Flag;
                } else if (2 == m_ParamNum) {
                    m_X.InitFromDsl(callData.GetParam(0));
                    m_Y.InitFromDsl(callData.GetParam(1));
                    m_Flag = m_X.Flag | m_Y.Flag;
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            LogOperator val = new LogOperator();
            val.m_X = m_X.Clone();
            val.m_Y = m_Y.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                if (m_ParamNum > 0)
                    m_X.Substitute(iterator, args);
                if (m_ParamNum > 1)
                    m_Y.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0)
                m_X.Evaluate(instance);
            if (m_ParamNum > 1)
                m_Y.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (1 == m_ParamNum && m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Log(x);
            } else if (2 == m_ParamNum && m_X.HaveValue && m_Y.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                object objY = m_Y.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                float y = StoryValueHelper.CastTo<float>(objY);
                m_Value = (float)Math.Log(x, y);
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<object> m_X = new StoryValue();
        private IStoryValue<object> m_Y = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class SqrtOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "sqrt" && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            SqrtOperator val = new SqrtOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Sqrt(x);
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class SinOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "sin" && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            SinOperator val = new SinOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Sin(x);
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class CosOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "cos" && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            CosOperator val = new CosOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Cos(x);
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class SinhOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "sinh" && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            SinhOperator val = new SinhOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Sinh(x);
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class CoshOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "cosh" && callData.GetParamNum() == 1) {
                m_X.InitFromDsl(callData.GetParam(0));
                m_Flag = m_X.Flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            CoshOperator val = new CoshOperator();
            val.m_X = m_X.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_X.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_X.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue()
        {
            if (m_X.HaveValue) {
                m_HaveValue = true;
                object objX = m_X.Value;
                float x = StoryValueHelper.CastTo<float>(objX);
                m_Value = (float)Math.Cosh(x);
            }
        }

        private IStoryValue<object> m_X = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class MinOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "min") {
                int flag = 0;
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                    flag |= val.Flag;
                }
                m_Flag = flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            MinOperator val = new MinOperator();
            for (int i = 0; i < m_List.Count; i++) {
                val.m_List.Add(m_List[i]);
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                for (int i = 0; i < m_List.Count; i++) {
                    m_List[i].Substitute(iterator, args);
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_List.Count; i++) {
                m_List[i].Evaluate(instance);
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

        private void TryUpdateValue()
        {
            bool isInt = true;
            bool canCalc = true;
            for (int i = 0; i < m_List.Count; i++) {
                if (m_List[i].HaveValue) {
                    if (!(m_List[i].Value is int)) {
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
                        int v = StoryValueHelper.CastTo<int>(m_List[i].Value);
                        if (minV > v)
                            minV = v;
                    }
                    m_Value = minV;
                } else {
                    float minV = float.MaxValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        float v = StoryValueHelper.CastTo<float>(m_List[i].Value);
                        if (minV > v)
                            minV = v;
                    }
                    m_Value = minV;
                }
            }
        }

        private List<IStoryValue<object>> m_List = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class MaxOperator : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "max") {
                int flag = 0;
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(arg);
                    m_List.Add(val);
                    flag |= val.Flag;
                }
                m_Flag = flag;
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            MaxOperator val = new MaxOperator();
            for (int i = 0; i < m_List.Count; i++) {
                val.m_List.Add(m_List[i]);
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                for (int i = 0; i < m_List.Count; i++) {
                    m_List[i].Substitute(iterator, args);
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_List.Count; i++) {
                m_List[i].Evaluate(instance);
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

        private void TryUpdateValue()
        {
            bool isInt = true;
            bool canCalc = true;
            for (int i = 0; i < m_List.Count; i++) {
                if (m_List[i].HaveValue) {
                    if (!(m_List[i].Value is int)) {
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
                        int v = StoryValueHelper.CastTo<int>(m_List[i].Value);
                        if (maxV < v)
                            maxV = v;
                    }
                    m_Value = maxV;
                } else {
                    float maxV = float.MinValue;
                    for (int i = 0; i < m_List.Count; i++) {
                        float v = StoryValueHelper.CastTo<float>(m_List[i].Value);
                        if (maxV < v)
                            maxV = v;
                    }
                    m_Value = maxV;
                }
            }
        }

        private List<IStoryValue<object>> m_List = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
