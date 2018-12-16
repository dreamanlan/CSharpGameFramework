using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace StorySystem.CommonValues
{
    internal sealed class LinqValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
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
        public IStoryValue<object> Clone()
        {
            LinqValue val = new LinqValue();
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
            
            bool canCalc = true;
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            if (canCalc) {
                m_HaveValue = true;
                object obj = m_Object.Value;
                string method = m_Method.Value;
                if (null != obj && !string.IsNullOrEmpty(method)) {
                    object v = 0;
                    IEnumerable list = obj as IEnumerable;
                    if (null != list) {
                        if (method == "orderby" || method == "orderbydesc") {
                            bool desc = method == "orderbydesc";
                            List<object> results = new List<object>();
                            IEnumerator enumer = list.GetEnumerator();
                            while (enumer.MoveNext()) {
                                object val = enumer.Current;
                                results.Add(val);
                            }
                            results.Sort((object o1, object o2) => {
                                object r1 = null;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, o1, args);
                                    r1 = m_Args[i].Value;
                                }
                                object r2 = null;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, o2, args);
                                    r2 = m_Args[i].Value;
                                }
                                string rs1 = r1 as string;
                                string rs2 = r2 as string;
                                int r = 0;
                                if (null != rs1 && null != rs2) {
                                    r = rs1.CompareTo(rs2);
                                } else {
                                    float rd1 = StoryValueHelper.CastTo<float>(r1);
                                    float rd2 = StoryValueHelper.CastTo<float>(r2);
                                    r = rd1.CompareTo(rd2);
                                }
                                if (desc)
                                    r = -r;
                                return r;
                            });
                            v = results;
                        } else if (method == "where") {
                            List<object> results = new List<object>();
                            IEnumerator enumer = list.GetEnumerator();
                            while (enumer.MoveNext()) {
                                object val = enumer.Current;

                                object r = null;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, val, args);
                                    r = m_Args[i].Value;
                                }
                                if (StoryValueHelper.CastTo<int>(r) != 0) {
                                    results.Add(val);
                                }
                            }
                            v = results;
                        } else if (method == "top") {
                            object r = null;
                            for (int i = 0; i < m_Args.Count; i++) {
                                m_Args[i].Evaluate(instance, iterator, args);
                                r = m_Args[i].Value;
                            }
                            int ct = StoryValueHelper.CastTo<int>(r);
                            List<object> results = new List<object>();
                            IEnumerator enumer = list.GetEnumerator();
                            while (enumer.MoveNext()) {
                                object val = enumer.Current;
                                if (ct > 0) {
                                    results.Add(val);
                                    --ct;
                                }
                            }
                            v = results;
                        }
                    }
                    m_Value = v;
                } else {
                    m_Value = null;
                }
            }

            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, iterator, args);
            }
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

        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
