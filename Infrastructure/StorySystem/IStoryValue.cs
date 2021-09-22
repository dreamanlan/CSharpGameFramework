using System;
using System.Collections.Generic;
namespace StorySystem
{
    /// <summary>
    /// 描述剧情命令中用到的值，此接口用以支持参数、局部变量、全局变量与内建函数（返回一个剧情命令用到的值）。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStoryValue<T>
    {
        void InitFromDsl(Dsl.ISyntaxComponent param);//从DSL语言初始化值实例
        IStoryValue<T> Clone();//克隆一个新实例，每个值只从DSL语言初始化一次，之后的实例由克隆产生，提升性能
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);//参数替换为参数值并计算StoryValue的值
        bool HaveValue { get; }//是否已经有值，对常量初始化后即产生值，对参数、变量与函数则在Evaluate后产生值
        T Value { get; }//具体的值
    }
    public interface IStoryValue
    {
        void InitFromDsl(Dsl.ISyntaxComponent param);//从DSL语言初始化值实例
        IStoryValue Clone();//克隆一个新实例，每个值只从DSL语言初始化一次，之后的实例由克隆产生，提升性能
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);//参数替换为参数值并计算StoryValue的值
        bool HaveValue { get; }//是否已经有值，对常量初始化后即产生值，对参数、变量与函数则在Evaluate后产生值
        BoxedValue Value { get; }//具体的值
    }
    public class StoryArgValue : IStoryValue
    {
        public const int c_NotArg = -1;
        public const int c_Iterator = -2;
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {}
        public IStoryValue Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_ArgIndex >= 0 && m_ArgIndex < args.Count) {
                m_Value = args[m_ArgIndex];
                m_HaveValue = true;
            }
            else if (m_ArgIndex == c_Iterator) {
                m_Value = iterator;
                m_HaveValue = true;
            }
            else {
                m_Value.SetNullObject();
                m_HaveValue = true;
            }
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

        internal void SetArgument(int index)
        {
            m_HaveValue = false;
            m_ArgIndex = index;
            m_Value = BoxedValue.NullObject;
        }

        private StoryArgValue NewValueObject()
        {
            StoryArgValue obj = new StoryArgValue();
            return obj;
        }
        private void CopyFrom(StoryArgValue other)
        {
            m_ArgIndex = other.m_ArgIndex;
            m_Value = other.m_Value;
            m_HaveValue = other.m_HaveValue;
        }

        private int m_ArgIndex = c_NotArg;
        private BoxedValue m_Value;
        private bool m_HaveValue = false;
    }
    public class StoryVarValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {}
        public IStoryValue Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            string name = m_VariableName;
            if (!string.IsNullOrEmpty(name)) {
                m_HaveValue = instance.TryGetVariable(name, out m_Value);
            }
            else {
                m_Value.SetNullObject();
                m_HaveValue = true;
            }
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

        internal void SetVariable(string name)
        {
            m_HaveValue = false;
            m_VariableName = name;
            m_Value = BoxedValue.NullObject;
        }

        private StoryVarValue NewValueObject()
        {
            StoryVarValue obj = new StoryVarValue();
            return obj;
        }
        private void CopyFrom(StoryVarValue other)
        {
            m_VariableName = other.m_VariableName;
            m_Value = other.m_Value;
            m_HaveValue = other.m_HaveValue;
        }

        private string m_VariableName = null;
        private BoxedValue m_Value;
        private bool m_HaveValue = false;
    }
    public class StoryConstValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {}
        public IStoryValue Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {}
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

        internal void SetValue<T>(T val)
        {
            m_HaveValue = true;
            m_Value.Set(val);
        }

        protected virtual StoryConstValue NewValueObject()
        {
            StoryConstValue obj = new StoryConstValue();
            return obj;
        }
        private void CopyFrom(StoryConstValue other)
        {
            m_Value = other.m_Value;
            m_HaveValue = other.m_HaveValue;
        }

        private BoxedValue m_Value;
        private bool m_HaveValue = false;
    }
    public class StoryValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.ValueData valueData = param as Dsl.ValueData;
            if (null != valueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("$")) {
                    if (0 == id.CompareTo("$$")) {
                        SetArgument(StoryArgValue.c_Iterator);
                    }
                    else {
                        string idName = id.Substring(1);
                        if (idName.Length > 0 && char.IsDigit(idName[0])) {
                            SetArgument(int.Parse(idName));
                        }
                        else {
                            SetVariable(id);
                        }
                    }
                }
                else if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("@")) {
                    SetVariable(id);
                }
                else {
                    CalcInitValue(param);
                }
            }
            else {
                CalcInitValue(param);
            }
        }
        public IStoryValue Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (null != m_Proxy) {
                m_Proxy.Evaluate(instance, handler, iterator, args);
            }
        }
        public bool HaveValue
        {
            get {
                return null == m_Proxy ? true : m_Proxy.HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return null == m_Proxy ? BoxedValue.NullObject : m_Proxy.Value;
            }
        }

        private StoryValue NewValueObject()
        {
            StoryValue obj = new StoryValue();
            return obj;
        }
        private void CopyFrom(StoryValue other)
        {
            if (null != other.m_Proxy) {
                m_Proxy = other.m_Proxy.Clone();
            }
        }
        private void SetArgument(int index)
        {
            StoryArgValue proxy = new StoryArgValue();
            proxy.SetArgument(index);
            m_Proxy = proxy;
        }
        private void SetVariable(string name)
        {
            StoryVarValue proxy = new StoryVarValue();
            proxy.SetVariable(name);
            m_Proxy = proxy;
        }
        private void SetProxy(IStoryValue proxy)
        {
            m_Proxy = proxy;
        }
        private void SetValue<T>(T val)
        {
            StoryConstValue proxy = new StoryConstValue();
            proxy.SetValue(val);
            m_Proxy = proxy;
        }
        private void CalcInitValue(Dsl.ISyntaxComponent param)
        {
            IStoryValue val = StoryValueManager.Instance.CalcValue(param);
            if (null != val) {
                //对初始化即能求得值的函数，不需要再记录函数表达式，直接转换为常量值。
                if (val.HaveValue) {
                    SetValue(val.Value);
                }
                else {
                    SetProxy(val);
                }
            }
            else if (param is Dsl.ValueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.NUM_TOKEN) {
                    if (id.IndexOf('.') >= 0 || id.IndexOf('e') > 0 || id.IndexOf('E') > 0)
                        SetValue(float.Parse(id, System.Globalization.NumberStyles.Float));
                    else if (id.StartsWith("0x"))
                        SetValue(uint.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber));
                    else
                        SetValue(int.Parse(id, System.Globalization.NumberStyles.Integer));
                }
                else if (idType == Dsl.ValueData.ID_TOKEN && (id == "true" || id == "false")) {
                    SetValue(id == "true");
                }
                else {
                    SetValue(id);
                }
            }
            else {
#if DEBUG
                string err = string.Format("Unknown value, id:{0} line:{1}", param.GetId(), param.GetLine());
                throw new Exception(err);
#else
        GameFramework.LogSystem.Error("Unknown value, id:{0}", param.GetId());
#endif
            }
        }

        private IStoryValue m_Proxy = null;
    }
    public class StoryValue<T> : IStoryValue<T>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.ValueData valueData = param as Dsl.ValueData;
            if (null != valueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("$")) {
                    if (0 == id.CompareTo("$$")) {
                        SetArgument(StoryArgValue.c_Iterator);
                    }
                    else {
                        string idName = id.Substring(1);
                        if (idName.Length > 0 && char.IsDigit(idName[0])) {
                            SetArgument(int.Parse(id.Substring(1)));
                        }
                        else {
                            SetVariable(id);
                        }
                    }
                }
                else if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("@")) {
                    SetVariable(id);
                }
                else {
                    CalcInitValue(param);
                }
            }
            else {
                CalcInitValue(param);
            }
        }
        public IStoryValue<T> Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (null != m_Proxy) {
                m_Proxy.Evaluate(instance, handler, iterator, args);
            }
        }
        public bool HaveValue
        {
            get {
                return null == m_Proxy ? true : m_Proxy.HaveValue;
            }
        }
        public T Value
        {
            get {
                return null == m_Proxy ? default(T) : m_Proxy.Value.Get<T>();
            }
        }

        private StoryValue<T> NewValueObject()
        {
            StoryValue<T> obj = new StoryValue<T>();
            return obj;
        }
        private void CopyFrom(StoryValue<T> other)
        {
            if (null != other.m_Proxy) {
                m_Proxy = other.m_Proxy.Clone();
            }
        }
        private void SetArgument(int index)
        {
            StoryArgValue proxy = new StoryArgValue();
            proxy.SetArgument(index);
            m_Proxy = proxy;
        }
        private void SetVariable(string name)
        {
            StoryVarValue proxy = new StoryVarValue();
            proxy.SetVariable(name);
            m_Proxy = proxy;
        }
        private void SetProxy(IStoryValue proxy)
        {
            m_Proxy = proxy;
        }
        private void SetValue(T val)
        {
            StoryConstValue proxy = new StoryConstValue();
            proxy.SetValue(val);
            m_Proxy = proxy;
        }
        private void CalcInitValue(Dsl.ISyntaxComponent param)
        {
            IStoryValue val = StoryValueManager.Instance.CalcValue(param);
            if (null != val) {
                //对初始化即能求得值的函数，不需要再记录函数表达式，直接转换为常量值。
                if (val.HaveValue) {
                    SetValue(val.Value.Get<T>());
                }
                else {
                    SetProxy(val);
                }
            }
            else if (param is Dsl.ValueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.NUM_TOKEN) {
                    if (id.IndexOf('.') >= 0 || id.IndexOf('e') > 0 || id.IndexOf('E') > 0)
                        SetValue(StoryValueHelper.CastTo<T>(float.Parse(id, System.Globalization.NumberStyles.Float)));
                    else if (id.StartsWith("0x"))
                        SetValue(StoryValueHelper.CastTo<T>(uint.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber)));
                    else
                        SetValue(StoryValueHelper.CastTo<T>(int.Parse(id, System.Globalization.NumberStyles.Integer)));
                }
                else if (idType == Dsl.ValueData.ID_TOKEN && (id == "true" || id == "false")) {
                    SetValue(StoryValueHelper.CastTo<T>(id == "true"));
                }
                else {
                    SetValue(StoryValueHelper.CastTo<T>(id));
                }
            }
            else {
#if DEBUG
                string err = string.Format("Unknown value, id:{0} line:{1}", param.GetId(), param.GetLine());
                throw new Exception(err);
#else
        GameFramework.LogSystem.Error("Unknown value, id:{0}", param.GetId());
#endif
            }
        }

        private IStoryValue m_Proxy = null;
    }
}
