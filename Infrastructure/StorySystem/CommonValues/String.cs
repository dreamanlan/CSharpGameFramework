using System;
using System.Collections;
using System.Collections.Generic;
namespace StorySystem.CommonValues
{
    internal sealed class FormatValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "format") {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_Format.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_FormatArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            FormatValue val = new FormatValue();
            val.m_Format = m_Format.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                val.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Format.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, iterator, args);
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

        private void TryUpdateValue()
        {
            bool canCalc = true;
            if (!m_Format.HaveValue) {
                canCalc = false;
            } else {
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    if (!m_FormatArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                string format = m_Format.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    arglist.Add(m_FormatArgs[i].Value);
                }
                object[] args = arglist.ToArray();
                m_Value = string.Format(format, args);
            }
        }
        private IStoryValue<string> m_Format = new StoryValue<string>();
        private List<IStoryValue<object>> m_FormatArgs = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class SubstringValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "substring" && callData.GetParamNum() > 0) {
                m_ParamNum = callData.GetParamNum();
                m_String.InitFromDsl(callData.GetParam(0));
                if (m_ParamNum > 1) {
                    m_Start.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_Length.InitFromDsl(callData.GetParam(2));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            SubstringValue val = new SubstringValue();
            val.m_ParamNum = m_ParamNum;
            val.m_String = m_String.Clone();
            val.m_Start = m_Start.Clone();
            val.m_Length = m_Length.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, iterator, args);
            if (m_ParamNum > 1)
                m_Start.Evaluate(instance, iterator, args);
            if (m_ParamNum > 2)
                m_Length.Evaluate(instance, iterator, args);
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
            if (m_String.HaveValue) {
                bool canCalc = true;
                string str = m_String.Value;
                int start = 0;
                int len = 0;
                if (m_ParamNum == 1 && m_String.HaveValue) {
                    len = str.Length;
                }
                if (m_ParamNum == 2 && !m_Start.HaveValue) {
                    canCalc = false;
                } else {
                    start = m_Start.Value;
                    len = str.Length - start;
                }
                if (m_ParamNum == 3 && (!m_Start.HaveValue || !m_Length.HaveValue)) {
                    canCalc = false;
                } else {
                    start = m_Start.Value;
                    len = m_Length.Value;
                }
                if (canCalc) {
                    m_HaveValue = true;
                    m_Value = str.Substring(start, len);
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<string> m_String = new StoryValue<string>();
        private IStoryValue<int> m_Start = new StoryValue<int>();
        private IStoryValue<int> m_Length = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class Str2IntValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "str2int" && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            Str2IntValue val = new Str2IntValue();
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, iterator, args);
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                try {
                    m_Value = int.Parse(str);
                } catch {
                    m_Value = 0;
                }
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class Str2FloatValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "str2float" && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            Str2FloatValue val = new Str2FloatValue();
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, iterator, args);
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                try {
                    m_Value = float.Parse(str);
                } catch {
                    m_Value = 0.0f;
                }
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DictFormatValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "dictformat") {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_DictId.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_FormatArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            DictFormatValue val = new DictFormatValue();
            val.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                val.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_DictId.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, iterator, args);
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

        private void TryUpdateValue()
        {
            bool canCalc = true;
            if (!m_DictId.HaveValue) {
                canCalc = false;
            } else {
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    if (!m_FormatArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                object dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    arglist.Add(m_FormatArgs[i].Value);
                }
                object[] args = arglist.ToArray();
                m_Value = Dict.Format((string)dictId, args);
            }
        }
        private IStoryValue<object> m_DictId = new StoryValue();
        private List<IStoryValue<object>> m_FormatArgs = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DictGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "dictget" && callData.GetParamNum() > 0) {
                m_DictId.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            DictGetValue val = new DictGetValue();
            val.m_DictId = m_DictId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_DictId.Evaluate(instance, iterator, args);
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
            if (m_DictId.HaveValue) {
                object dictId = m_DictId.Value;
                m_HaveValue = true;
                m_Value = Dict.Get((string)dictId);
            }
        }
        private IStoryValue<object> m_DictId = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DictParseValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "dictparse" && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            DictParseValue val = new DictParseValue();
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, iterator, args);
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                m_Value = Dict.Parse(str);
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
