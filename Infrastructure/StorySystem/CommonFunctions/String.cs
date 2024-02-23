using System;
using System.Collections;
using System.Collections.Generic;
namespace StorySystem.CommonFunctions
{
    public sealed class FormatFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            FormatFunction val = new FormatFunction();
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
                        arglist.Add(m_FormatArgs[i].Value.GetObject());
                    }
                    object[] args = arglist.ToArray();
                    m_Value = string.Format(format, args);
                }
                else {
                    m_Value = string.Format("{0}", formatObj.ToString());
                }
            }
        }
        private IStoryFunction m_Format = new StoryValue();
        private List<IStoryFunction> m_FormatArgs = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class SubstringFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            SubstringFunction val = new SubstringFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private IStoryFunction<int> m_Start = new StoryValue<int>();
        private IStoryFunction<int> m_Length = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringContainsFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            StringContainsFunction val = new StringContainsFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private List<IStoryFunction<string>> m_KeyArgs = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringNotContainsFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            StringNotContainsFunction val = new StringNotContainsFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private List<IStoryFunction<string>> m_KeyArgs = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringContainsAnyFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            StringContainsAnyFunction val = new StringContainsAnyFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private List<IStoryFunction<string>> m_KeyArgs = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringNotContainsAnyFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            StringNotContainsAnyFunction val = new StringNotContainsAnyFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private List<IStoryFunction<string>> m_KeyArgs = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringToLowerFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            StringToLowerFunction val = new StringToLowerFunction();
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

        private IStoryFunction<string> m_StringVal = new StoryValue<string>();
    }
    public sealed class StringToUpperFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            StringToUpperFunction val = new StringToUpperFunction();
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

        private IStoryFunction<string> m_StringVal = new StoryValue<string>();
    }
    public sealed class Str2LowerFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            Str2LowerFunction val = new Str2LowerFunction();
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

        private IStoryFunction<string> m_StringVal = new StoryValue<string>();
    }
    public sealed class Str2UpperFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringVal.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            Str2UpperFunction val = new Str2UpperFunction();
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

        private IStoryFunction<string> m_StringVal = new StoryValue<string>();
    }
    public sealed class Str2IntFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            Str2IntFunction val = new Str2IntFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Str2FloatFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            Str2FloatFunction val = new Str2FloatFunction();
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
        private IStoryFunction<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
