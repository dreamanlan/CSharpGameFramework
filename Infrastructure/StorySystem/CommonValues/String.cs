using System;
using System.Collections;
using System.Collections.Generic;
namespace StorySystem.CommonValues
{
    internal sealed class FormatValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

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
        public IStoryValue Clone()
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Format.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, handler, iterator, args);
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
                var formatObj = m_Format.Value;
                string format = formatObj.IsString ? formatObj.StringVal : null;
                if (!string.IsNullOrEmpty(format) && m_FormatArgs.Count > 0) {
                    ArrayList arglist = new ArrayList();
                    for (int i = 0; i < m_FormatArgs.Count; i++) {
                        arglist.Add(m_FormatArgs[i].Value.Get<object>());
                    }
                    object[] args = arglist.ToArray();
                    m_Value = string.Format(format, args);
                }
                else {
                    m_Value = string.Format("{0}", formatObj.ToString());
                }
            }
        }
        private IStoryValue m_Format = new StoryValue();
        private List<IStoryValue> m_FormatArgs = new List<IStoryValue>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class SubstringValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
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
        public IStoryValue Clone()
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 1)
                m_Start.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2)
                m_Length.Evaluate(instance, handler, iterator, args);
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
            if (m_String.HaveValue) {
                bool canCalc = true;
                string str = m_String.Value;
                int start = 0;
                int len = 0;
                if (m_ParamNum == 1) {
                    len = str.Length;
                }
                else if (m_ParamNum == 2) {
                    if (!m_Start.HaveValue) {
                        canCalc = false;
                    }
                    else {
                        start = m_Start.Value;
                        len = str.Length - start;
                    }
                }
                else if (m_ParamNum == 3) {
                    if (!m_Start.HaveValue || !m_Length.HaveValue) {
                        canCalc = false;
                    }
                    else {
                        start = m_Start.Value;
                        len = m_Length.Value;
                    }
                }
                else {
                    canCalc = false;
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
        private BoxedValue m_Value;
    }
    internal sealed class StringContainsValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_String.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue<string> val = new StoryValue<string>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_KeyArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            StringContainsValue val = new StringContainsValue();
            val.m_String = m_String.Clone();
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                val.m_KeyArgs.Add(m_KeyArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                m_KeyArgs[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool canCalc = true;
            if (!m_String.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_KeyArgs.Count; i++) {
                    if (!m_KeyArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                string str = m_String.Value;
                bool r = true;
                for (int i = 0; i < m_KeyArgs.Count; ++i) {
                    var key = m_KeyArgs[i].Value;
                    if (!string.IsNullOrEmpty(key) && !str.Contains(key)) {
                        r = false;
                        break;
                    }
                }
                m_Value = r;
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private List<IStoryValue<string>> m_KeyArgs = new List<IStoryValue<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class StringNotContainsValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_String.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue<string> val = new StoryValue<string>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_KeyArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            StringNotContainsValue val = new StringNotContainsValue();
            val.m_String = m_String.Clone();
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                val.m_KeyArgs.Add(m_KeyArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                m_KeyArgs[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool canCalc = true;
            if (!m_String.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_KeyArgs.Count; i++) {
                    if (!m_KeyArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                string str = m_String.Value;
                bool r = true;
                for (int i = 0; i < m_KeyArgs.Count; ++i) {
                    var key = m_KeyArgs[i].Value;
                    if (!string.IsNullOrEmpty(key) && str.Contains(key)) {
                        r = false;
                        break;
                    }
                }
                m_Value = r;
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private List<IStoryValue<string>> m_KeyArgs = new List<IStoryValue<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class StringContainsAnyValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_String.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue<string> val = new StoryValue<string>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_KeyArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            StringContainsAnyValue val = new StringContainsAnyValue();
            val.m_String = m_String.Clone();
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                val.m_KeyArgs.Add(m_KeyArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                m_KeyArgs[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool canCalc = true;
            if (!m_String.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_KeyArgs.Count; i++) {
                    if (!m_KeyArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                string str = m_String.Value;
                bool r = true;
                for (int i = 0; i < m_KeyArgs.Count; ++i) {
                    var key = m_KeyArgs[i].Value;
                    if (!string.IsNullOrEmpty(key)) {
                        if (str.Contains(key)) {
                            r = true;
                            break;
                        }
                        else {
                            r = false;
                        }
                    }
                }
                m_Value = r;
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private List<IStoryValue<string>> m_KeyArgs = new List<IStoryValue<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class StringNotContainsAnyValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                if (num > 0) {
                    m_String.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < callData.GetParamNum(); ++i) {
                    StoryValue<string> val = new StoryValue<string>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_KeyArgs.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            StringNotContainsAnyValue val = new StringNotContainsAnyValue();
            val.m_String = m_String.Clone();
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                val.m_KeyArgs.Add(m_KeyArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_KeyArgs.Count; i++) {
                m_KeyArgs[i].Evaluate(instance, handler, iterator, args);
            }
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
            bool canCalc = true;
            if (!m_String.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_KeyArgs.Count; i++) {
                    if (!m_KeyArgs[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                string str = m_String.Value;
                bool r = true;
                for (int i = 0; i < m_KeyArgs.Count; ++i) {
                    var key = m_KeyArgs[i].Value;
                    if (!string.IsNullOrEmpty(key)) {
                        if (!str.Contains(key)) {
                            r = true;
                            break;
                        }
                        else {
                            r = false;
                        }
                    }
                }
                m_Value = r;
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private List<IStoryValue<string>> m_KeyArgs = new List<IStoryValue<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class StringToLowerValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            StringToLowerValue val = new StringToLowerValue();
            val.m_StringVal = m_StringVal.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StringVal.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_StringVal.HaveValue) {
                m_HaveValue = true;
                var str = m_StringVal.Value;
                m_Value = str.ToLower();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryValue<string> m_StringVal = new StoryValue<string>();
    }
    internal sealed class StringToUpperValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            StringToUpperValue val = new StringToUpperValue();
            val.m_StringVal = m_StringVal.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StringVal.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_StringVal.HaveValue) {
                m_HaveValue = true;
                var str = m_StringVal.Value;
                m_Value = str.ToUpper();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryValue<string> m_StringVal = new StoryValue<string>();
    }
    internal sealed class Str2LowerValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            Str2LowerValue val = new Str2LowerValue();
            val.m_StringVal = m_StringVal.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StringVal.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_StringVal.HaveValue) {
                m_HaveValue = true;
                var str = m_StringVal.Value;
                m_Value = str.StrToLower();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryValue<string> m_StringVal = new StoryValue<string>();
    }
    internal sealed class Str2UpperValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            Str2UpperValue val = new Str2UpperValue();
            val.m_StringVal = m_StringVal.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StringVal.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_StringVal.HaveValue) {
                m_HaveValue = true;
                var str = m_StringVal.Value;
                m_Value = str.StrToUpper();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryValue<string> m_StringVal = new StoryValue<string>();
    }
    internal sealed class Str2IntValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            Str2IntValue val = new Str2IntValue();
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
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
        private BoxedValue m_Value;
    }
    internal sealed class Str2FloatValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            Str2FloatValue val = new Str2FloatValue();
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
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
        private BoxedValue m_Value;
    }
}
