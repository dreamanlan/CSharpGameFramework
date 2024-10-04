using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework;

namespace DotnetStoryScript.CommonFunctions
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
                    StoryFunction val = new StoryFunction();
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
        private IStoryFunction m_Format = new StoryFunction();
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
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private IStoryFunction<int> m_Start = new StoryFunction<int>();
        private IStoryFunction<int> m_Length = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringReplaceFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_Str.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Val.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            StringReplaceFunction val = new StringReplaceFunction();
            val.m_Str = m_Str.Clone();
            val.m_Key = m_Key.Clone();
            val.m_Val = m_Val.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Str.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Val.Evaluate(instance, handler, iterator, args);
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
            if (m_Str.HaveValue && m_Key.HaveValue && m_Val.HaveValue) {
                m_HaveValue = true;
                var str = m_Str.Value;
                var key = m_Key.Value;
                var val = m_Val.Value;
                if (null != str && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                    m_Value = str.Replace(key, val);
                }
            }
        }
        private IStoryFunction<string> m_Str = new StoryFunction<string>();
        private IStoryFunction<string> m_Key = new StoryFunction<string>();
        private IStoryFunction<string> m_Val = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringReplaceCharFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 3) {
                m_Str.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Val.InitFromDsl(callData.GetParam(2));
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            StringReplaceCharFunction val = new StringReplaceCharFunction();
            val.m_Str = m_Str.Clone();
            val.m_Key = m_Key.Clone();
            val.m_Val = m_Val.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Str.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Val.Evaluate(instance, handler, iterator, args);
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
            if (m_Str.HaveValue && m_Key.HaveValue && m_Val.HaveValue) {
                m_HaveValue = true;
                var str = m_Str.Value;
                var key = m_Key.Value;
                var val = m_Val.Value;
                if (null != str && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                    m_Value = str.Replace(key[0], val[0]);
                }
            }
        }
        private IStoryFunction<string> m_Str = new StoryFunction<string>();
        private IStoryFunction<string> m_Key = new StoryFunction<string>();
        private IStoryFunction<string> m_Val = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class MakeStringFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                for (int i = 0; i < num; ++i) {
                    StoryFunction val = new StoryFunction();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            MakeStringFunction val = new MakeStringFunction();
            for (int i = 0; i < m_Args.Count; i++) {
                val.m_Args.Add(m_Args[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
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
            for (int i = 0; i < m_Args.Count; i++) {
                if (!m_Args[i].HaveValue) {
                    canCalc = false;
                    break;
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                var chars = new List<char>();
                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Value;
                    var str = v.AsString;
                    if (null != str) {
                        char c = '\0';
                        if (str.Length > 0) {
                            c = str[0];
                        }
                        chars.Add(c);
                    }
                    else {
                        char c = v.GetChar();
                        chars.Add(c);
                    }
                }
                m_Value = new String(chars.ToArray());
            }
        }

        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
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
                    StoryFunction<string> val = new StoryFunction<string>();
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
        private IStoryFunction<string> m_String = new StoryFunction<string>();
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
                    StoryFunction<string> val = new StoryFunction<string>();
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
        private IStoryFunction<string> m_String = new StoryFunction<string>();
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
                    StoryFunction<string> val = new StoryFunction<string>();
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
        private IStoryFunction<string> m_String = new StoryFunction<string>();
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
                    StoryFunction<string> val = new StoryFunction<string>();
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
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private List<IStoryFunction<string>> m_KeyArgs = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NewStringBuilderFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            NewStringBuilderFunction val = new NewStringBuilderFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;

            m_Value = BoxedValue.FromObject(new StringBuilder());
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringBuilderToStringFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() > 0)
                    m_StringBuilder.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            StringBuilderToStringFunction val = new StringBuilderToStringFunction();
            val.m_StringBuilder = m_StringBuilder.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StringBuilder.Evaluate(instance, handler, iterator, args);
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
            if (m_StringBuilder.HaveValue) {
                m_HaveValue = true;
                var sb = m_StringBuilder.Value;
                m_Value = sb.ToString();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryFunction<StringBuilder> m_StringBuilder = new StoryFunction<StringBuilder>();
    }
    public sealed class StringJoinFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_Sep.InitFromDsl(callData.GetParam(0));
                    m_Strs.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            StringJoinFunction val = new StringJoinFunction();
            val.m_Sep = m_Sep.Clone();
            val.m_Strs = m_Strs.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Sep.Evaluate(instance, handler, iterator, args);
            m_Strs.Evaluate(instance, handler, iterator, args);
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
            if (m_Sep.HaveValue && m_Strs.HaveValue) {
                m_HaveValue = true;
                var sep = m_Sep.Value;
                var list = m_Strs.Value;
                if (null != sep && null != list) {
                    string[] strs = new string[list.Count];
                    for (int i = 0; i < list.Count; ++i) {
                        strs[i] = list[i].ToString();
                    }
                    m_Value = string.Join(sep, strs);
                }
                else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction<string> m_Sep = new StoryFunction<string>();
        private IStoryFunction<IList> m_Strs = new StoryFunction<IList>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringSplitFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_String.InitFromDsl(callData.GetParam(0));
                    m_Seps.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            StringSplitFunction val = new StringSplitFunction();
            val.m_String = m_String.Clone();
            val.m_Seps = m_Seps.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_String.Evaluate(instance, handler, iterator, args);
            m_Seps.Evaluate(instance, handler, iterator, args);
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
            if (m_String.HaveValue && m_Seps.HaveValue) {
                m_HaveValue = true;
                var str = m_String.Value;
                var seps = m_Seps.Value;
                if (null != str && null != seps) {
                    char[] cs = new char[seps.Count];
                    for (int i = 0; i < seps.Count; ++i) {
                        string sep = seps[i].ToString();
                        if (sep.Length > 0) {
                            cs[i] = sep[0];
                        }
                        else {
                            cs[i] = '\0';
                        }
                    }
                    m_Value = BoxedValue.FromObject(str.Split(cs));
                }
                else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private IStoryFunction<IList> m_Seps = new StoryFunction<IList>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class StringTrimFunction : IStoryFunction
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
            StringTrimFunction val = new StringTrimFunction();
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
                m_Value = str.Trim();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
    }
    public sealed class StringTrimStartFunction : IStoryFunction
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
            StringTrimStartFunction val = new StringTrimStartFunction();
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
                m_Value = str.TrimStart();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
    }
    public sealed class StringTrimEndFunction : IStoryFunction
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
            StringTrimEndFunction val = new StringTrimEndFunction();
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
                m_Value = str.TrimEnd();
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
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

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
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

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
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

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
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

        private IStoryFunction<string> m_StringVal = new StoryFunction<string>();
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
                int.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Str2UintFunction : IStoryFunction
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
            Str2UintFunction val = new Str2UintFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                uint.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Str2LongFunction : IStoryFunction
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
            Str2LongFunction val = new Str2LongFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                long.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Str2UlongFunction : IStoryFunction
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
            Str2UlongFunction val = new Str2UlongFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                ulong.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
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
                float.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Str2DoubleFunction : IStoryFunction
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
            Str2DoubleFunction val = new Str2DoubleFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                double.TryParse(str, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Hex2IntFunction : IStoryFunction
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
            Hex2IntFunction val = new Hex2IntFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Hex2UintFunction : IStoryFunction
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
            Hex2UintFunction val = new Hex2UintFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                uint.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Hex2LongFunction : IStoryFunction
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
            Hex2LongFunction val = new Hex2LongFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                long.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class Hex2UlongFunction : IStoryFunction
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
            Hex2UlongFunction val = new Hex2UlongFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                ulong.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out var v);
                m_Value = v;
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DatetimeStrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_String.InitFromDsl(callData.GetParam(0));
                }
                TryUpdateValue();
            }
        }
        public IStoryFunction Clone()
        {
            DatetimeStrFunction val = new DatetimeStrFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_String = m_String.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_String.Evaluate(instance, handler, iterator, args);
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
            if (m_ParamNum > 0 && m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                m_Value = DateTime.Now.ToString(str);
            }
            else if(m_ParamNum == 0) {
                m_HaveValue = true;
                m_Value = DateTime.Now.ToString();
            }
        }

        private int m_ParamNum = 0;
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LongDateStrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            LongDateStrFunction val = new LongDateStrFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;

            m_Value = DateTime.Now.ToLongDateString();
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class LongTimeStrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            LongTimeStrFunction val = new LongTimeStrFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;

            m_Value = DateTime.Now.ToLongTimeString();
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ShortDateStrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            ShortDateStrFunction val = new ShortDateStrFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;

            m_Value = DateTime.Now.ToShortDateString();
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ShortTimeStrFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            ShortTimeStrFunction val = new ShortTimeStrFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;

            m_Value = DateTime.Now.ToShortTimeString();
            m_HaveValue = true;
        }
        public void Analyze(StoryInstance instance)
        {
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

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class IsNullOrEmptyFunction : IStoryFunction
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
            IsNullOrEmptyFunction val = new IsNullOrEmptyFunction();
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
            if (m_String.HaveValue) {
                string str = m_String.Value;
                m_HaveValue = true;
                m_Value = string.IsNullOrEmpty(str);
            }
        }
        private IStoryFunction<string> m_String = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
