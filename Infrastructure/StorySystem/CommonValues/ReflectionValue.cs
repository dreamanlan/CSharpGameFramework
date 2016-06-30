using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace StorySystem.CommonValues
{
    internal sealed class GetTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gettype") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetTypeValue val = new GetTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, iterator, args);
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
    internal sealed class DotnetCallValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "dotnetcall") {
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
        public IStoryValue<object> Clone()
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
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, iterator, args);
            m_Method.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, iterator, args);
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
                object obj = m_Object.Value;
                string method = m_Method.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value);
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    Type t = obj as Type;
                    if (null != t) {
                        try {
                            m_Value = t.InvokeMember(method, BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic, null, null, args);
                        } catch (Exception ex) {
                            GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            m_Value = null;
                        }
                    } else {
                        t = obj.GetType();
                        if (null != t) {
                            try {
                                m_Value = t.InvokeMember(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic, null, obj, args);
                            } catch (Exception ex) {
                                GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                m_Value = null;
                            }
                        } else {
                            m_Value = null;
                        }
                    }
                } else {
                    m_Value = null;
                }
            }
        }
        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DotnetGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "dotnetget") {
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
        public IStoryValue<object> Clone()
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
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, iterator, args);
            m_Method.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, iterator, args);
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
                object obj = m_Object.Value;
                string method = m_Method.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value);
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    Type t = obj as Type;
                    if (null != t) {
                        try {
                            m_Value = t.InvokeMember(method, BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, null, args);
                        } catch (Exception ex) {
                            GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            m_Value = null;
                        }
                    } else {
                        t = obj.GetType();
                        if (null != t) {
                            try {
                                m_Value = t.InvokeMember(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, obj, args);
                            } catch (Exception ex) {
                                GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                m_Value = null;
                            }
                        } else {
                            m_Value = null;
                        }
                    }
                } else {
                    m_Value = null;
                }
            }
        }
        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class ChangeTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "changetype") {
                if (callData.GetParamNum() == 2) {
                    m_Object.InitFromDsl(callData.GetParam(0));
                    m_Type.InitFromDsl(callData.GetParam(1));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            ChangeTypeValue val = new ChangeTypeValue();
            val.m_Object = m_Object.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, iterator, args);
            m_Type.Evaluate(instance, iterator, args);
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
                string type = m_Type.Value;
                try {
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
                } catch (Exception ex) {
                    GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_Value = null;
                }
            }
        }
        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class PgrepValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "pgrep") {
                if (callData.GetParamNum() == 1) {
                    m_Filter.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            PgrepValue val = new PgrepValue();
            val.m_Filter = m_Filter.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Filter.Evaluate(instance, iterator, args);
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
    internal sealed class PlistValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "plist") {
            }
        }
        public IStoryValue<object> Clone()
        {
            PlistValue val = new PlistValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;

            TryUpdateValue();
        }
        public void Analyze(StoryInstance instance)
        {
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
            try {
                List<string> pss = new List<string>();
                Process[] ps = Process.GetProcesses();
                for (int i = 0; i < ps.Length; ++i) {
                    Process p = ps[i];
                    try {
                        if (!p.HasExited) {
                            pss.Add(p.ProcessName);
                        }
                    } catch {
                    }
                }
                m_Value = string.Join(",", pss.ToArray());
            } catch {
                m_Value = "";
            }
        }
        private bool m_HaveValue;
        private object m_Value;
    }
}
