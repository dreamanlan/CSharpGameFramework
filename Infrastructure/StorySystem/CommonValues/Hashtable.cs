using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using StorySystem;
using LitJson;
namespace StorySystem.CommonValues
{
    internal sealed class ReadAllLinesValue : IStoryValue
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
            ReadAllLinesValue val = new ReadAllLinesValue();
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
                m_Value = BoxedValue.NullObject;
                if (File.Exists(str)) {
                    m_Value = BoxedValue.From(File.ReadAllLines(str));
                }
            }
        }

        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class ReadFileValue : IStoryValue
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
            ReadFileValue val = new ReadFileValue();
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
                m_Value = BoxedValue.NullObject;
                if (File.Exists(str)) {
                    m_Value = File.ReadAllText(str);
                }
            }
        }

        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class ToJsonValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_Hashtable.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            ToJsonValue val = new ToJsonValue();
            val.m_Hashtable = m_Hashtable.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Hashtable.Evaluate(instance, handler, iterator, args);
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
            if (m_Hashtable.HaveValue) {
                m_HaveValue = true;
                object obj = m_Hashtable.Value.Get<object>();
                if (null != obj) {
                    JsonData json = ToJson(obj);
                    m_Value = JsonMapper.ToJson(json);
                } else {
                    m_Value = string.Empty;
                }
            }
        }

        private IStoryValue m_Hashtable = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        private static JsonData ToJson(object obj)
        {
            var dict = obj as IDictionary;
            var enumer = obj as IEnumerable;
            if (null != dict) {
                var jsonData = new JsonData();
                var e = dict.GetEnumerator();
                while (e.MoveNext()) {
                    string key = e.Key.ToString();
                    if (!string.IsNullOrEmpty(key)) {
                        jsonData[key] = ToJson(e.Value);
                    }
                }
                return jsonData;
            } else if (null != enumer) {
                var jsonData = new JsonData();
                var e = enumer.GetEnumerator();
                while (e.MoveNext()) {
                    var o = ToJson(e.Current);
                    jsonData.Add(o);
                }
                return jsonData;
            } else if (null == obj) {
                return new JsonData();
            } else if (obj is int) {
                return new JsonData((int)obj);
            } else if (obj is long) {
                return new JsonData((long)obj);
            } else if (obj is bool) {
                return new JsonData((bool)obj);
            } else if (obj is float) {
                return new JsonData((double)(float)obj);
            } else if (obj is double) {
                return new JsonData((double)obj);
            } else if (obj is string) {
                return new JsonData(obj as string);
            } else {
                return new JsonData(obj);
            }
        }
    }
    internal sealed class FromJsonValue : IStoryValue
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
            FromJsonValue val = new FromJsonValue();
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
                var json = JsonMapper.ToObject(str);
                m_Value = BoxedValue.From(ToValue(json, null));
            }
        }

        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;

        private static object ToValue(JsonData data, object defVal)
        {
            if (null == data) {
                return defVal;
            }
            if (data.IsObject) {
                var dict = new Hashtable();
                foreach (var key in data.Keys) {
                    var val = ToValue(data[key], defVal);
                    dict.Add(key, val);
                }
                return dict;
            } else if (data.IsArray) {
                var list = new ArrayList();
                for (int i = 0; i < data.Count; ++i) {
                    var val = ToValue(data[i], defVal);
                    list.Add(val);
                }
                return list;
            } else if (data.IsInt) {
                return (int)data;
            } else if (data.IsLong) {
                return (int)(long)data;
            } else if (data.IsDouble) {
                return (float)(double)data;
            } else if (data.IsBoolean) {
                return ((bool)data ? 1 : 0);
            } else if (data.IsString) {
                return (string)data;
            } else if (data.IsObject || data.IsArray) {
                return data;
            } else {
                return defVal;
            }
        }
    }
    internal sealed class HashtableValue : IStoryValue
    {
        private class Pair
        {
            internal IStoryValue m_Key = new StoryValue();
            internal IStoryValue m_Value = new StoryValue();
            internal Pair Clone()
            {
                Pair pair = new Pair();
                pair.m_Key = m_Key.Clone();
                pair.m_Value = m_Value.Clone();
                return pair;
            }
            internal void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
            {
                m_Key.Evaluate(instance, handler, iterator, args);
                m_Value.Evaluate(instance, handler, iterator, args);
            }
            internal bool HaveValue
            {
                get
                {
                    return m_Key.HaveValue && m_Value.HaveValue;
                }
            }
        }
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {

                int num = callData.GetParamNum();
                for (int i = 0; i < num; ++i) {
                    m_Args.Add(ParsePair(callData.GetParam(i)));
                }
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            HashtableValue val = new HashtableValue();
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

        private Pair ParsePair(Dsl.ISyntaxComponent param)
        {
            Pair pair = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                pair = new Pair();
                pair.m_Key.InitFromDsl(callData.GetParam(0));
                pair.m_Value.InitFromDsl(callData.GetParam(1));
            }
            return pair;
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
                var dict = new Hashtable();
                for (int i = 0; i < m_Args.Count; i++) {
                    Pair pair = m_Args[i];
                    var key = pair.m_Key.Value.ToString();
                    if (null != key) {
                        dict.Add(key, pair.m_Value.Value.Get<object>());
                    }
                }
                m_Value = BoxedValue.From(dict);
            }
        }

        private List<Pair> m_Args = new List<Pair>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class HashtableGetValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    m_Key.InitFromDsl(callData.GetParam(1));
                    if (m_ParamNum > 2) {
                        m_DefValue.InitFromDsl(callData.GetParam(2));
                    }
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue Clone()
        {
            HashtableGetValue cmd = new HashtableGetValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_DefValue = m_DefValue.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 1) {
                m_Var.Evaluate(instance, handler, iterator, args);
                m_Key.Evaluate(instance, handler, iterator, args);
            }
            if (m_ParamNum > 2) {
                m_DefValue.Evaluate(instance, handler, iterator, args);
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
            if (m_Var.HaveValue && m_Key.HaveValue && (m_ParamNum <= 2 || m_DefValue.HaveValue)) {
                object obj = m_Var.Value.Get<object>();
                var dict = obj as IDictionary;
                object key = m_Key.Value.Get<object>();
                BoxedValue defVal = BoxedValue.NullObject;
                if (m_ParamNum > 2) {
                    defVal = m_DefValue.Value;
                }
                m_HaveValue = true;
                if (null != dict && null != key) {
                    try {
                        if (dict.Contains(key)) {
                            m_Value = BoxedValue.From(dict[key]);
                        } else {
                            m_Value = defVal;
                        }
                    } catch {
                        m_Value = defVal;
                    }
                } else {
                    m_Value = defVal;
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue m_Var = new StoryValue();
        private IStoryValue m_Key = new StoryValue();
        private IStoryValue m_DefValue = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class HashtableSizeValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue Clone()
        {
            HashtableSizeValue cmd = new HashtableSizeValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, handler, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                var dict = obj as IDictionary;
                m_HaveValue = true;
                if (null != dict) {
                    m_Value = dict.Count;
                } else {
                    m_Value = 0;
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue m_Var = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class HashtableKeysValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue Clone()
        {
            HashtableKeysValue cmd = new HashtableKeysValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, handler, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                var dict = obj as IDictionary;
                m_HaveValue = true;
                if (null != dict) {
                    ArrayList list = new ArrayList();
                    list.AddRange(dict.Keys);
                    m_Value = BoxedValue.From(list);
                } else {
                    m_Value.SetNullObject();
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue m_Var = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class HashtableValuesValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue Clone()
        {
            HashtableValuesValue cmd = new HashtableValuesValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, handler, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                var dict = obj as IDictionary;
                m_HaveValue = true;
                if (null != dict) {
                    ArrayList list = new ArrayList();
                    list.AddRange(dict.Values);
                    m_Value = BoxedValue.From(list);
                } else {
                    m_Value.SetNullObject();
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue m_Var = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
