using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace StorySystem.CommonFunctions
{
    public sealed class GetTypeFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetTypeFunction val = new GetTypeFunction();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, handler, iterator, args);
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
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                m_Value = BoxedValue.FromObject(Type.GetType(typeName));
                if (null == m_Value.ObjectVal) {
                    GameFramework.LogSystem.Warn("null == Type.GetType({0})", typeName);
                }
            }
        }
        private IStoryFunction<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DotnetCallFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            DotnetCallFunction val = new DotnetCallFunction();
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = BoxedValue.NullObject;
                object obj = m_Object.Value.GetObject();
                var methodObj = m_Method.Value;
                string method = methodObj.IsString ? methodObj.StringVal : null;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    if (null != method) {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
                            var d = dict[method] as Delegate;
                            if (null != d) {
                                m_Value = BoxedValue.FromObject(d.DynamicInvoke(args));
                            }
                        }
                        else {
                            Type t = obj as Type;
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                    m_Value = BoxedValue.FromObject(t.InvokeMember(method, flags, null, null, args));
                                }
                                catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetCall {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                    m_Value = BoxedValue.NullObject;
                                }
                            }
                            else {
                                t = obj.GetType();
                                if (null != t) {
                                    try {
                                        BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                        GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                        m_Value = BoxedValue.FromObject(t.InvokeMember(method, flags, null, obj, args));
                                    }
                                    catch (Exception ex) {
                                        GameFramework.LogSystem.Warn("DotnetCall {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                        m_Value = BoxedValue.NullObject;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction m_Object = new StoryValue();
        private IStoryFunction m_Method = new StoryValue();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class DotnetGetFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            DotnetGetFunction val = new DotnetGetFunction();
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = BoxedValue.NullObject;
                object obj = m_Object.Value.GetObject();
                var methodObj = m_Method.Value;
                string method = methodObj.IsString ? methodObj.StringVal : null;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    if (null != method) {
                        IDictionary dict = obj as IDictionary;
                        if (null != dict && dict.Contains(method)) {
                            m_Value = BoxedValue.FromObject(dict[method]);
                        }
                        else {
                            Type t = obj as Type;
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForGet(t, method, flags, args);
                                    m_Value = BoxedValue.FromObject(t.InvokeMember(method, flags, null, null, args));
                                }
                                catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetGet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                    m_Value = BoxedValue.NullObject;
                                }
                            }
                            else {
                                t = obj.GetType();
                                if (null != t) {
                                    try {
                                        BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                        GameFramework.Converter.CastArgsForGet(t, method, flags, args);
                                        m_Value = BoxedValue.FromObject(t.InvokeMember(method, flags, null, obj, args));
                                    }
                                    catch (Exception ex) {
                                        GameFramework.LogSystem.Warn("DotnetGet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                        m_Value = BoxedValue.NullObject;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction m_Object = new StoryValue();
        private IStoryFunction m_Method = new StoryValue();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CollectionCallFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            CollectionCallFunction val = new CollectionCallFunction();
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = BoxedValue.NullObject;
                object obj = m_Object.Value.GetObject();
                var methodObj = m_Method.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    IDictionary dict = obj as IDictionary;
                    var mobj = methodObj.GetObject();
                    if (null != dict && dict.Contains(mobj)) {
                        var d = dict[mobj] as Delegate;
                        if (null != d) {
                            m_Value = BoxedValue.FromObject(d.DynamicInvoke(args));
                        }
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && methodObj.IsInteger) {
                            int index = methodObj.GetInt();
                            var e = enumer.GetEnumerator();
                            for (int i = 0; i <= index; ++i) {
                                e.MoveNext();
                            }
                            var d = e.Current as Delegate;
                            if (null != d) {
                                m_Value = BoxedValue.FromObject(d.DynamicInvoke(args));
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction m_Object = new StoryValue();
        private IStoryFunction m_Method = new StoryValue();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CollectionGetFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            CollectionGetFunction val = new CollectionGetFunction();
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
            if (!m_Object.HaveValue || !m_Method.HaveValue) {
                canCalc = false;
            }
            else {
                for (int i = 0; i < m_Args.Count; i++) {
                    if (!m_Args[i].HaveValue) {
                        canCalc = false;
                        break;
                    }
                }
            }
            if (canCalc) {
                m_HaveValue = true;
                m_Value = BoxedValue.NullObject;
                object obj = m_Object.Value.GetObject();
                var methodObj = m_Method.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; i++) {
                    arglist.Add(m_Args[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                if (null != obj) {
                    IDictionary dict = obj as IDictionary;
                    var mobj = methodObj.GetObject();
                    if (null != dict && dict.Contains(mobj)) {
                        m_Value = BoxedValue.FromObject(dict[mobj]);
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && methodObj.IsInteger) {
                            int index = methodObj.GetInt();
                            var e = enumer.GetEnumerator();
                            for (int i = 0; i <= index; ++i) {
                                e.MoveNext();
                            }
                            m_Value = BoxedValue.FromObject(e.Current);
                        }
                    }
                }
            }
        }
        private IStoryFunction m_Object = new StoryValue();
        private IStoryFunction m_Method = new StoryValue();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ChangeTypeFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            ChangeTypeFunction val = new ChangeTypeFunction();
            val.m_Object = m_Object.Clone();
            val.m_Type = m_Type.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Type.Evaluate(instance, handler, iterator, args);
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
            if (m_Object.HaveValue && m_Type.HaveValue) {
                m_HaveValue = true;
                var obj = m_Object.Value;
                var objType = m_Type.Value;
                try {
                    string str = obj.AsString;
                    string type = objType.IsString ? objType.StringVal : null;
                    if (null != type) {
                        if (obj.IsString) {
                            if (0 == type.CompareTo("sbyte")) {
                                m_Value = StoryValueHelper.CastTo<sbyte>(str);
                            }
                            else if (0 == type.CompareTo("byte")) {
                                m_Value = StoryValueHelper.CastTo<byte>(str);
                            }
                            else if (0 == type.CompareTo("short")) {
                                m_Value = StoryValueHelper.CastTo<short>(str);
                            }
                            else if (0 == type.CompareTo("ushort")) {
                                m_Value = StoryValueHelper.CastTo<ushort>(str);
                            }
                            else if (0 == type.CompareTo("int")) {
                                m_Value = StoryValueHelper.CastTo<int>(str);
                            }
                            else if (0 == type.CompareTo("uint")) {
                                m_Value = StoryValueHelper.CastTo<uint>(str);
                            }
                            else if (0 == type.CompareTo("long")) {
                                m_Value = StoryValueHelper.CastTo<long>(str);
                            }
                            else if (0 == type.CompareTo("ulong")) {
                                m_Value = StoryValueHelper.CastTo<ulong>(str);
                            }
                            else if (0 == type.CompareTo("float")) {
                                m_Value = StoryValueHelper.CastTo<float>(str);
                            }
                            else if (0 == type.CompareTo("double")) {
                                m_Value = StoryValueHelper.CastTo<double>(str);
                            }
                            else if (0 == type.CompareTo("string")) {
                                m_Value = str;
                            }
                            else if (0 == type.CompareTo("bool")) {
                                m_Value = StoryValueHelper.CastTo<bool>(str);
                            }
                            else {
                                Type t = Type.GetType(type);
                                if (null != t) {
                                    m_Value = BoxedValue.FromObject(StoryValueHelper.CastTo(t, str));
                                }
                                else {
                                    GameFramework.LogSystem.Warn("null == Type.GetType({0})", type);
                                }
                            }
                        }
                        else {
                            if (0 == type.CompareTo("sbyte")) {
                                m_Value = obj.GetSByte();
                            }
                            else if (0 == type.CompareTo("byte")) {
                                m_Value = obj.GetByte();
                            }
                            else if (0 == type.CompareTo("short")) {
                                m_Value = obj.GetShort();
                            }
                            else if (0 == type.CompareTo("ushort")) {
                                m_Value = obj.GetUShort();
                            }
                            else if (0 == type.CompareTo("int")) {
                                m_Value = obj.GetInt();
                            }
                            else if (0 == type.CompareTo("uint")) {
                                m_Value = obj.GetUInt();
                            }
                            else if (0 == type.CompareTo("long")) {
                                m_Value = obj.GetLong();
                            }
                            else if (0 == type.CompareTo("ulong")) {
                                m_Value = obj.GetULong();
                            }
                            else if (0 == type.CompareTo("float")) {
                                m_Value = obj.GetFloat();
                            }
                            else if (0 == type.CompareTo("double")) {
                                m_Value = obj.GetDouble();
                            }
                            else if (0 == type.CompareTo("decimal")) {
                                m_Value = obj.GetDecimal();
                            }
                            else if (0 == type.CompareTo("bool")) {
                                m_Value = obj.GetBool();
                            }
                            else if (0 == type.CompareTo("char")) {
                                m_Value = obj.GetChar();
                            }
                            else if (0 == type.CompareTo("string")) {
                                m_Value = obj.GetString();
                            }
                            else {
                                Type t = Type.GetType(type);
                                if (null != t) {
                                    m_Value = BoxedValue.FromObject(obj.CastTo(t));
                                }
                                else {
                                    GameFramework.LogSystem.Warn("null == Type.GetType({0})", type);
                                }
                            }
                        }
                    }
                    else {
                        var t = objType.IsObject ? objType.ObjectVal as Type : null;
                        if (null != t) {
                            m_Value = BoxedValue.FromObject(StoryValueHelper.CastTo(t, obj.GetObject()));
                        }
                    }
                }
                catch (Exception ex) {
                    GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction m_Object = new StoryValue();
        private IStoryFunction m_Type = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class ParseEnumFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            ParseEnumFunction val = new ParseEnumFunction();
            val.m_Type = m_Type.Clone();
            val.m_Val = m_Val.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Type.Evaluate(instance, handler, iterator, args);
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
            if (m_Type.HaveValue && m_Val.HaveValue) {
                m_HaveValue = true;
                var objType = m_Type.Value;
                string val = m_Val.Value;
                try {
                    var t = objType.IsObject ? objType.ObjectVal as Type : null;
                    string type = objType.IsString ? objType.StringVal : null;
                    if (null == t && null != type) {
                        t = Type.GetType(type);
                    }
                    if (null != t) {
                        m_Value = BoxedValue.FromObject(Enum.Parse(t, val, true));
                    }
                    else {
                        GameFramework.LogSystem.Warn("null == Type.GetType({0})", type);
                    }
                }
                catch (Exception ex) {
                    GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction m_Type = new StoryValue();
        private IStoryFunction<string> m_Val = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class PgrepFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            PgrepFunction val = new PgrepFunction();
            val.m_Filter = m_Filter.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Filter.Evaluate(instance, handler, iterator, args);
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
                        }
                        catch {
                        }
                    }
                    m_Value = ct;
                }
                catch {
                    m_Value = 0;
                }
            }
        }

        private IStoryFunction<string> m_Filter = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class PlistFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            PlistFunction val = new PlistFunction();
            val.m_Filter = m_Filter.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Filter.Evaluate(instance, handler, iterator, args);
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
                    }
                    catch {
                    }
                }
                m_Value = string.Join(",", pss.ToArray());
            }
            catch {
                m_Value = "";
            }
        }

        private IStoryFunction<string> m_Filter = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
