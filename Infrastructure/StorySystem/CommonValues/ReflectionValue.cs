using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace StorySystem.CommonValues
{
    internal sealed class GetTypeValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetTypeValue val = new GetTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, handler, iterator, args);
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
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                m_Value = Type.GetType(typeName);
                if (null == m_Value) {
                    GameFramework.LogSystem.Warn("null == Type.GetType({0})", typeName);
                }
            }
        }
        private IStoryValue<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DotnetCallValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_Object.InitFromDsl(callData.GetParam(0));
                    m_Method.InitFromDsl(callData.GetParam(1));
                }
                for (int i = 2; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                }
            }
        }
        public IStoryValue Clone()
        {
            DotnetCallValue val = new DotnetCallValue();
            val.m_Object = m_Object.Clone();
            val.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                val.m_Args.Add(m_Args[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            } else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = null;
                object obj = m_Object.Value;
                object methodObj = m_Method.Value;
                string method = methodObj as string;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value);
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    if (null != method) {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
                            var d = dict[method] as Delegate;
                            if (null != d) {
                                m_Value = d.DynamicInvoke(args);
                            }
                        } else {
                            Type t = obj as Type;
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                    m_Value = t.InvokeMember(method, flags, null, null, args);
                                } catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetCall {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                    m_Value = null;
                                }
                            } else {
                                t = obj.GetType();
                                if (null != t) {
                                    try {
                                        BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                        GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                        m_Value = t.InvokeMember(method, flags, null, obj, args);
                                    } catch (Exception ex) {
                                        GameFramework.LogSystem.Warn("DotnetCall {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                        m_Value = null;
                                    }
                                }
                            }
                        }
                    } else {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(methodObj)) {
                            var d = dict[methodObj] as Delegate;
                            if (null != d) {
                                m_Value = d.DynamicInvoke(args);
                            }
                        } else {
                            IEnumerable enumer = obj as IEnumerable;
                            if (null != enumer && methodObj is int) {
                                int index = (int)methodObj;
                                var e = enumer.GetEnumerator();
                                for (int i = 0; i <= index; ++i) {
                                    e.MoveNext();
                                }
                                var d = e.Current as Delegate;
                                if (null != d) {
                                    m_Value = d.DynamicInvoke(args);
                                }
                            }
                        }
                    }
                }
            }
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DotnetGetValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_Object.InitFromDsl(callData.GetParam(0));
                    m_Method.InitFromDsl(callData.GetParam(1));
                }
                for (int i = 2; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                }
            }
        }
        public IStoryValue Clone()
        {
            DotnetGetValue val = new DotnetGetValue();
            val.m_Object = m_Object.Clone();
            val.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                val.m_Args.Add(m_Args[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            } else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = null;
                object obj = m_Object.Value;
                object methodObj = m_Method.Value;
                string method = methodObj as string;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value);
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    if (null != method) {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(method)) {
                            m_Value = dict[method];
                        } else {
                            Type t = obj as Type;
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForGet(t, method, flags, args);
                                    m_Value = t.InvokeMember(method, flags, null, null, args);
                                } catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetGet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                    m_Value = null;
                                }
                            } else {
                                t = obj.GetType();
                                if (null != t) {
                                    try {
                                        BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                        GameFramework.Converter.CastArgsForGet(t, method, flags, args);
                                        m_Value = t.InvokeMember(method, flags, null, obj, args);
                                    } catch (Exception ex) {
                                        GameFramework.LogSystem.Warn("DotnetGet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                        m_Value = null;
                                    }
                                }
                            }
                        }
                    } else {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(methodObj)) {
                            m_Value = dict[methodObj];
                        } else {
                            IEnumerable enumer = obj as IEnumerable;
                            if (null != enumer && methodObj is int) {
                                int index = (int)methodObj;
                                var e = enumer.GetEnumerator();
                                for (int i = 0; i <= index; ++i) {
                                    e.MoveNext();
                                }
                                m_Value = e.Current;
                            }
                        }
                    }
                }
            }
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class ChangeTypeValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_Object.InitFromDsl(callData.GetParam(0));
                    m_Type.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            ChangeTypeValue val = new ChangeTypeValue();
            val.m_Object = m_Object.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
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
            if (m_Object.HaveValue && m_Type.HaveValue) {
                m_HaveValue = true;
                object obj = m_Object.Value;
                object objType = m_Type.Value;
                try {
                    string type = objType as string;
                    if (null != type) {
                        if (0 == type.CompareTo("sbyte")) {
                            m_Value = StoryValueHelper.CastTo<sbyte>(obj);
                        } else if (0 == type.CompareTo("byte")) {
                            m_Value = StoryValueHelper.CastTo<byte>(obj);
                        } else if (0 == type.CompareTo("short")) {
                            m_Value = StoryValueHelper.CastTo<short>(obj);
                        } else if (0 == type.CompareTo("ushort")) {
                            m_Value = StoryValueHelper.CastTo<ushort>(obj);
                        } else if (0 == type.CompareTo("int")) {
                            m_Value = StoryValueHelper.CastTo<int>(obj);
                        } else if (0 == type.CompareTo("uint")) {
                            m_Value = StoryValueHelper.CastTo<uint>(obj);
                        } else if (0 == type.CompareTo("long")) {
                            m_Value = StoryValueHelper.CastTo<long>(obj);
                        } else if (0 == type.CompareTo("ulong")) {
                            m_Value = StoryValueHelper.CastTo<ulong>(obj);
                        } else if (0 == type.CompareTo("float")) {
                            m_Value = StoryValueHelper.CastTo<float>(obj);
                        } else if (0 == type.CompareTo("double")) {
                            m_Value = StoryValueHelper.CastTo<double>(obj);
                        } else if (0 == type.CompareTo("string")) {
                            m_Value = StoryValueHelper.CastTo<string>(obj);
                        } else if (0 == type.CompareTo("bool")) {
                            m_Value = StoryValueHelper.CastTo<bool>(obj);
                        } else {
                            Type t = Type.GetType(type);
                            if (null != t) {
                                m_Value = Convert.ChangeType(obj, t);
                            } else {
                                GameFramework.LogSystem.Warn("null == Type.GetType({0})", type);
                            }
                        }
                    } else {
                        var t = objType as Type;
                        if (null != t) {
                            m_Value = Convert.ChangeType(obj, t);
                        }
                    }
                } catch (Exception ex) {
                    GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_Value = null;
                }
            }
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Type = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class ParseEnumValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() == 2) {
                    m_Type.InitFromDsl(callData.GetParam(0));
                    m_Val.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            ParseEnumValue val = new ParseEnumValue();
            val.m_Type = m_Type.Clone();
            val.m_Val = m_Val.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Type.Evaluate(instance, handler, iterator, args);
            m_Val.Evaluate(instance, handler, iterator, args);
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
            if (m_Type.HaveValue && m_Val.HaveValue) {
                m_HaveValue = true;
                object objType = m_Type.Value;
                string val = m_Val.Value;
                try {
                    var t = objType as Type;
                    string type = objType as string;
                    if (null == t && null != type) {
                        t = Type.GetType(type);
                    }
                    if (null != t) {
                        m_Value = Enum.Parse(t, val, true);
                    } else {
                        GameFramework.LogSystem.Warn("null == Type.GetType({0})", type);
                    }
                } catch (Exception ex) {
                    GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_Value = null;
                }
            }
        }
        private IStoryValue m_Type = new StoryValue();
        private IStoryValue<string> m_Val = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class PgrepValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() >= 1) {
                    m_Filter.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            PgrepValue val = new PgrepValue();
            val.m_Filter = m_Filter.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Filter.Evaluate(instance, handler, iterator, args);
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
            if (m_Filter.HaveValue) {
                m_HaveValue = true;
                string filter = m_Filter.Value;
                if (null == filter)
                    filter = string.Empty;
                try {
                    int ct = 0;
                    Process[] ps = Process.GetProcesses();
                    for (int i = 0; i < ps.Length; ++i) {
                        Process p = ps[i];
                        try {
                            if (!p.HasExited) {
                                if (p.ProcessName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) {
                                    ++ct;
                                }
                            }
                        } catch {
                        }
                    }
                    m_Value = ct;
                } catch {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<string> m_Filter = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class PlistValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetParamNum() >= 1) {
                    m_Filter.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            PlistValue val = new PlistValue();
            val.m_Filter = m_Filter.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Filter.Evaluate(instance, handler, iterator, args);
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
            m_HaveValue = true;
            string filter = m_Filter.Value;
            if (null == filter)
                filter = string.Empty;
            try {
                List<string> pss = new List<string>();
                Process[] ps = Process.GetProcesses();
                for (int i = 0; i < ps.Length; ++i) {
                    Process p = ps[i];
                    try {
                        if (!p.HasExited) {
                            if (p.ProcessName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) {
                                pss.Add(p.ProcessName);
                            }
                        }
                    } catch {
                    }
                }
                m_Value = string.Join(",", pss.ToArray());
            } catch {
                m_Value = "";
            }
        }

        private IStoryValue<string> m_Filter = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
