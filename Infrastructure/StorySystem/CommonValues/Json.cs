using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using LitJson;
namespace StorySystem.CommonValues
{
    internal sealed class Json2StrValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "json2str" && callData.GetParamNum() > 0) {
                m_Json.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            Json2StrValue val = new Json2StrValue();
            val.m_Json = m_Json.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Json.Evaluate(instance, iterator, args);
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
            if (m_Json.HaveValue) {
                m_HaveValue = true;
                object obj = m_Json.Value;
                JsonData json = obj as JsonData;
                if (null != json) {
                    m_Value = JsonMapper.ToJson(json);
                } else {
                    m_Value = "";
                }
            }
        }
        private IStoryValue<object> m_Json = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class Str2JsonValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "str2json" && callData.GetParamNum() > 0) {
                m_String.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            Str2JsonValue val = new Str2JsonValue();
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
                m_Value = JsonMapper.ToObject(str);
            }
        }
        private IStoryValue<string> m_String = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonArrayValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {

                int num = callData.GetParamNum();
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                }
                TryUpdateValue();
            }
        }
        public IStoryValue<object> Clone()
        {
            JsonArrayValue val = new JsonArrayValue();
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
            for (int i = 0; i < m_Args.Count; i++) {
                if (!m_Args[i].HaveValue) {
                    canCalc = false;
                    break;
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                JsonData json = new JsonData();
                json.SetJsonType(JsonType.Array);
                for (int i = 0; i < m_Args.Count; i++) {
                    json.Add(m_Args[i].Value);
                }
                m_Value = json;
            }
        }
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonObjectValue : IStoryValue<object>
    {
        private class Pair
        {
            internal IStoryValue<string> m_Key = new StoryValue<string>();
            internal IStoryValue<object> m_Value = new StoryValue();
            internal Pair Clone()
            {
                Pair pair = new Pair();
                pair.m_Key = m_Key.Clone();
                pair.m_Value = m_Value.Clone();
                return pair;
            }
            internal void Evaluate(StoryInstance instance, object iterator, object[] args)
            {
                m_Key.Evaluate(instance, iterator, args);
                m_Value.Evaluate(instance, iterator, args);
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
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {

                int num = callData.GetParamNum();
                for (int i = 0; i < num; ++i) {
                    m_Args.Add(ParsePair(callData.GetParam(i)));
                }
                TryUpdateValue();
            } else {
                Dsl.FunctionData funcData = param as Dsl.FunctionData;
                if (null != funcData) {

                    int num = funcData.GetStatementNum();
                    for (int i = 0; i < num; ++i) {
                        m_Args.Add(ParsePair(funcData.GetStatement(i)));
                    }
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            JsonObjectValue val = new JsonObjectValue();
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

        private Pair ParsePair(Dsl.ISyntaxComponent param)
        {
            Pair pair = null;
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() == 2) {
                pair = new Pair();
                pair.m_Key.InitFromDsl(callData.GetParam(0));
                pair.m_Value.InitFromDsl(callData.GetParam(1));
            }
            return pair;
        }
        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
                return null;
            if (obj is JsonData)
                return (JsonData)obj;
            return new JsonData(obj);
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
                JsonData json = new JsonData();
                json.SetJsonType(JsonType.Object);
                for (int i = 0; i < m_Args.Count; i++) {
                    Pair pair = m_Args[i];
                    string key = pair.m_Key.Value;
                    if (null != key) {
                        json[key] = ToJsonData(pair.m_Value.Value);
                    }
                }
                m_Value = json;
            }
        }
        private List<Pair> m_Args = new List<Pair>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "jsonget") {
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
        public IStoryValue<object> Clone()
        {
            JsonGetValue cmd = new JsonGetValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_DefValue = m_DefValue.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 1) {
                m_Var.Evaluate(instance, iterator, args);
                m_Key.Evaluate(instance, iterator, args);
            }
            if (m_ParamNum > 2) {
                m_DefValue.Evaluate(instance, iterator, args);
            }

            if (m_ParamNum > 1) {
            }
            if (m_ParamNum > 2) {
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
            if (m_Var.HaveValue && m_Key.HaveValue && (m_ParamNum <= 2 || m_DefValue.HaveValue)) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                object key = m_Key.Value;
                object defVal = null;
                if (m_ParamNum > 2) {
                    defVal = m_DefValue.Value;
                }
                m_HaveValue = true;
                if (null != json && null != key) {
                    try {
                        ICollection coll = json as ICollection;
                        IDictionary dict = json as IDictionary;
                        if (key is int) {
                            int index = (int)key;
                            if (index >= 0 && index < coll.Count) {
                                m_Value = ToValue(json[(int)key], defVal);
                            } else {
                                m_Value = defVal;
                            }
                        } else if (key is float) {
                            int index = (int)(float)key;
                            if (index >= 0 && index < coll.Count) {
                                m_Value = ToValue(json[(int)key], defVal);
                            } else {
                                m_Value = defVal;
                            }
                        } else if (key is string) {
                            if (dict.Contains(key)) {
                                m_Value = ToValue(json[(string)key], defVal);
                            } else {
                                m_Value = defVal;
                            }
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
        private object ToValue(JsonData data, object defVal)
        {
            if (null == data)
                return defVal;
            if (data.IsInt) {
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
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
        private IStoryValue<object> m_DefValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "jsoncount") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            JsonCountValue cmd = new JsonCountValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                m_HaveValue = true;
                if (null != json) {
                    if (json.IsObject || json.IsArray) {
                        try {
                            ICollection coll = json as ICollection;
                            m_Value = coll.Count;
                        } catch {
                            m_Value = 0;
                        }
                    }
                } else {
                    m_Value = 0;
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonKeysValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "jsonkeys") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            JsonKeysValue cmd = new JsonKeysValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                m_HaveValue = true;
                if (null != json) {
                    if (json.IsObject) {
                        try {
                            IDictionary dict = json as IDictionary;
                            ArrayList list = new ArrayList();
                            IDictionaryEnumerator enumerator = dict.GetEnumerator();
                            while (enumerator.MoveNext()) {
                                list.Add(enumerator.Key);
                            }
                            m_Value = list;
                        } catch {
                            m_Value = null;
                        }
                    } else {
                        m_Value = null;
                    }
                } else {
                    m_Value = null;
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class JsonValuesValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "jsonvalues") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            JsonValuesValue cmd = new JsonValuesValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                m_HaveValue = true;
                if (null != json) {
                    if (json.IsObject) {
                        try {
                            IDictionary dict = json as IDictionary;
                            ArrayList list = new ArrayList();
                            IDictionaryEnumerator enumerator = dict.GetEnumerator();
                            while (enumerator.MoveNext()) {
                                object val = ToValue(enumerator.Value as JsonData, null);
                                if (null != val) {
                                    list.Add(val);
                                }
                            }
                            m_Value = list;
                        } catch {
                            m_Value = null;
                        }
                    } else if (json.IsArray) {
                        try {
                            IList array = json as IList;
                            ArrayList list = new ArrayList();
                            int ct = array.Count;
                            for (int i = 0; i < ct; ++i) {
                                object val = ToValue(array[i] as JsonData, null);
                                if (null != val) {
                                    list.Add(val);
                                }
                            }
                            m_Value = list;
                        } catch {
                            m_Value = null;
                        }
                    } else {
                        m_Value = null;
                    }
                } else {
                    m_Value = null;
                }
            }
        }
        private object ToValue(JsonData data, object defVal)
        {
            if (null == data)
                return defVal;
            if (data.IsInt) {
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
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsJsonArrayValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isjsonarray") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            IsJsonArrayValue cmd = new IsJsonArrayValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                m_HaveValue = true;
                if (null != json) {
                    m_Value = json.IsArray ? 1 : 0;
                } else {
                    m_Value = 0;
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsJsonObjectValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isjsonobject") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_Var.InitFromDsl(callData.GetParam(0));
                    TryUpdateValue();
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            IsJsonObjectValue cmd = new IsJsonObjectValue();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_Var.Evaluate(instance, iterator, args);
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
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                m_HaveValue = true;
                if (null != json) {
                    m_Value = json.IsObject ? 1 : 0;
                } else {
                    m_Value = 0;
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<object> m_Var = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
}
