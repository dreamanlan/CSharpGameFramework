using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace StorySystem.CommonValues
{
    internal sealed class LinqValue : IStoryValue
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            
            bool canCalc = true;
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            if (canCalc) {
                m_HaveValue = true;
                var obj = m_Object.Value;
                string method = m_Method.Value;
                if (obj.IsObject && !string.IsNullOrEmpty(method)) {
                    object v = 0;
                    IEnumerable list = obj.ObjectVal as IEnumerable;
                    if (null != list) {
                        if (method == "orderby" || method == "orderbydesc") {
                            bool desc = method == "orderbydesc";
                            ObjList results = new ObjList();
                            IEnumerator enumer = list.GetEnumerator();
                            while (enumer.MoveNext()) {
                                object val = enumer.Current;
                                results.Add(val);
                            }
                            results.Sort((object o1, object o2) => {
                                BoxedValue r1 = BoxedValue.NullObject;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, handler, BoxedValue.From(o1), args);
                                    r1 = m_Args[i].Value;
                                }
                                BoxedValue r2 = BoxedValue.NullObject;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, handler, BoxedValue.From(o2), args);
                                    r2 = m_Args[i].Value;
                                }
                                string rs1 = r1.ToString();
                                string rs2 = r2.ToString();
                                int r = 0;
                                if (null != rs1 && null != rs2) {
                                    r = rs1.CompareTo(rs2);
                                } else {
                                    float rd1 = r1.Get<float>();
                                    float rd2 = r2.Get<float>();
                                    r = rd1.CompareTo(rd2);
                                }
                                if (desc)
                                    r = -r;
                                return r;
                            });
                            v = results;
                        } else if (method == "where") {
                            ObjList results = new ObjList();
                            IEnumerator enumer = list.GetEnumerator();
                            while (enumer.MoveNext()) {
                                object val = enumer.Current;

                                BoxedValue r = BoxedValue.NullObject;
                                for (int i = 0; i < m_Args.Count; i++) {
                                    m_Args[i].Evaluate(instance, handler, BoxedValue.From(val), args);
                                    r = m_Args[i].Value;
                                }
                                if (r.Get<int>() != 0) {
                                    results.Add(val);
                                }
                            }
                            v = results;
                        } else if (method == "top") {
                            BoxedValue r = BoxedValue.NullObject;
                            for (int i = 0; i < m_Args.Count; i++) {
                                m_Args[i].Evaluate(instance, handler, iterator, args);
                                r = m_Args[i].Value;
                            }
                            int ct = r.Get<int>();
                            ObjList results = new ObjList();
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
                    m_Value = BoxedValue.From(v);
                } else {
                    m_Value = BoxedValue.NullObject;
                }
            }

            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
            }
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

        private IStoryValue m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
