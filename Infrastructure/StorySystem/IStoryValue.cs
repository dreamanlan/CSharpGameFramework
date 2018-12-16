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
        void Evaluate(StoryInstance instance, object iterator, object[] args);//参数替换为参数值并计算StoryValue的值
        bool HaveValue { get; }//是否已经有值，对常量初始化后即产生值，对参数、变量与函数则在Evaluate后产生值
        T Value { get; }//具体的值
    }
    public interface IStoryValue : IStoryValue<object>
    { }
    public class StoryValue : IStoryValue
    {
        public const int c_Iterator = -2;
        public const int c_NotArg = -1;
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.ValueData valueData = param as Dsl.ValueData;
            if (null != valueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("$")) {
                    if (0 == id.CompareTo("$$")) {
                        SetArgument(c_Iterator);
                    } else {
                        string idName = id.Substring(1);
                        if (idName.Length > 0 && char.IsDigit(idName[0])) {
                            SetArgument(int.Parse(idName));
                        } else {
                            SetStack(id);
                        }
                    }
                } else if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("@")) {
                    if (id.StartsWith("@@"))
                        SetGlobal(id);
                    else
                        SetLocal(id);
                } else {
                    CalcInitValue(param);
                }
            } else {
                CalcInitValue(param);
            }
        }
        public IStoryValue<object> Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (IsConst)
                return;
            if (m_ArgIndex >= 0 && m_ArgIndex < args.Length) {
                m_Value = args[m_ArgIndex];
                m_HaveValue = true;
            } else if (m_ArgIndex == c_Iterator) {
                m_Value = iterator;
                m_HaveValue = true;
            } else if (null != m_Proxy) {
                m_Proxy.Evaluate(instance, iterator, args);
                if (m_Proxy.HaveValue) {
                    m_Value = m_Proxy.Value;
                    m_HaveValue = true;
                } else {
                    m_HaveValue = false;
                }
            } else {
                string name = string.Empty;
                if (null != m_LocalName) {
                    name = m_LocalName;
                } else if (null != m_GlobalName) {
                    name = m_GlobalName;
                } else if (null != m_StackName) {
                    name = m_StackName;
                }
                if (!string.IsNullOrEmpty(name)) {
                    m_HaveValue = instance.TryGetVariable(name, out m_Value);
                }
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
        public bool IsConst
        {
            get
            {
                return m_IsConst;
            }
        }
        
        protected virtual StoryValue NewValueObject()
        {
            StoryValue obj = new StoryValue();
            return obj;
        }

        private void CopyFrom(StoryValue other)
        {
            m_ArgIndex = other.m_ArgIndex;
            m_LocalName = other.m_LocalName;
            m_GlobalName = other.m_GlobalName;
            m_StackName = other.m_StackName;
            if (null != other.m_Proxy) {
                m_Proxy = other.m_Proxy.Clone();
            }
            m_Value = other.m_Value;
            m_HaveValue = other.m_HaveValue;
            m_IsConst = other.m_IsConst;
        }
        private void SetArgument(int index)
        {
            m_HaveValue = false;
            m_ArgIndex = index;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = null;
            m_Value = null;
            m_IsConst = false;
        }
        private void SetLocal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = name;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = null;
            m_Value = null;
            m_IsConst = false;
        }
        private void SetGlobal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = name;
            m_StackName = null;
            m_Proxy = null;
            m_Value = null;
            m_IsConst = false;
        }
        private void SetStack(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = name;
            m_Proxy = null;
            m_Value = null;
            m_IsConst = false;
        }
        private void SetProxy(IStoryValue<object> proxy)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = proxy;
            m_Value = null;
            m_IsConst = false;
        }
        private void SetValue(object val)
        {
            m_HaveValue = true;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = null;
            m_Value = val;
            m_IsConst = true;
        }
        private void CalcInitValue(Dsl.ISyntaxComponent param)
        {
            IStoryValue<object> val = StoryValueManager.Instance.CalcValue(param);
            if (null != val) {
                //对初始化即能求得值的函数，不需要再记录函数表达式，直接转换为常量值。
                if (val.HaveValue) {
                    SetValue(val.Value);
                } else {
                    SetProxy(val);
                }
            } else if (param is Dsl.ValueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.NUM_TOKEN) {
                    if (id.IndexOf('.') >= 0)
                        SetValue(float.Parse(id, System.Globalization.NumberStyles.Float));
                    else if (id.StartsWith("0x"))
                        SetValue(uint.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber));
                    else
                        SetValue(int.Parse(id, System.Globalization.NumberStyles.Integer));
                } else if (idType == Dsl.ValueData.BOOL_TOKEN) {
                    SetValue(id == "true");
                } else {
                    SetValue(id);
                }
            } else {
#if DEBUG
                string err = string.Format("Unknown value, id:{0} line:{1}", param.GetId(), param.GetLine());
                throw new Exception(err);
#else
        GameFramework.LogSystem.Error("Unknown value, id:{0}", param.GetId());
#endif
            }
        }
        private int m_ArgIndex = c_NotArg;
        private string m_LocalName = null;
        private string m_GlobalName = null;
        private string m_StackName = null;
        private IStoryValue<object> m_Proxy = null;
        private object m_Value;
        private bool m_HaveValue = false;
        private bool m_IsConst = false;
    }
    public class StoryValue<T> : IStoryValue<T>
    {
        public const int c_Iterator = -2;
        public const int c_NotArg = -1;
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.ValueData valueData = param as Dsl.ValueData;
            if (null != valueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("$")) {
                    if (0 == id.CompareTo("$$")) {
                        SetArgument(c_Iterator);
                    } else {
                        string idName = id.Substring(1);
                        if (idName.Length > 0 && char.IsDigit(idName[0])) {
                            SetArgument(int.Parse(id.Substring(1)));
                        } else {
                            SetStack(id);
                        }
                    }
                } else if (idType == Dsl.ValueData.ID_TOKEN && id.StartsWith("@")) {
                    if (id.StartsWith("@@"))
                        SetGlobal(id);
                    else
                        SetLocal(id);
                } else {
                    CalcInitValue(param);
                }
            } else {
                CalcInitValue(param);
            }
        }
        public IStoryValue<T> Clone()
        {
            var obj = NewValueObject();
            obj.CopyFrom(this);
            return obj;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (IsConst)
                return;
            if (m_ArgIndex >= 0 && m_ArgIndex < args.Length) {
                m_Value = StoryValueHelper.CastTo<T>(args[m_ArgIndex]);
                m_HaveValue = true;
            } else if (m_ArgIndex == c_Iterator) {
                m_Value = StoryValueHelper.CastTo<T>(iterator);
                m_HaveValue = true;
            } else if (null != m_Proxy) {
                m_Proxy.Evaluate(instance, iterator, args);
                if (m_Proxy.HaveValue) {
                    m_Value = StoryValueHelper.CastTo<T>(m_Proxy.Value);
                    m_HaveValue = true;
                } else {
                    m_HaveValue = false;
                }
            } else {
                string name = string.Empty;
                if (null != m_LocalName) {
                    name = m_LocalName;
                } else if (null != m_GlobalName) {
                    name = m_GlobalName;
                } else if (null != m_StackName) {
                    name = m_StackName;
                }
                if (!string.IsNullOrEmpty(name)) {
                    object val;
                    m_HaveValue = instance.TryGetVariable(name, out val);
                    if (m_HaveValue) {
                        m_Value = StoryValueHelper.CastTo<T>(val);
                    }
                }
            }
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public T Value
        {
            get
            {
                return m_Value;
            }
        }
        public bool IsConst
        {
            get
            {
                return m_IsConst;
            }
        }

        protected virtual StoryValue<T> NewValueObject()
        {
            StoryValue<T> obj = new StoryValue<T>();
            return obj;
        }

        private void CopyFrom(StoryValue<T> other)
        {
            m_ArgIndex = other.m_ArgIndex;
            m_LocalName = other.m_LocalName;
            m_GlobalName = other.m_GlobalName;
            m_StackName = other.m_StackName;
            if (null != other.m_Proxy) {
                m_Proxy = other.m_Proxy.Clone();
            }
            m_Value = other.m_Value;
            m_HaveValue = other.m_HaveValue;
            m_IsConst = other.m_IsConst;
        }
        private void SetArgument(int index)
        {
            m_HaveValue = false;
            m_ArgIndex = index;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = default(T);
            m_IsConst = false;
        }
        private void SetLocal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = name;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = null;
            m_Value = default(T);
            m_IsConst = false;
        }
        private void SetGlobal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = name;
            m_StackName = null;
            m_Proxy = null;
            m_Value = default(T);
            m_IsConst = false;
        }
        private void SetStack(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = name;
            m_Proxy = null;
            m_Value = default(T);
            m_IsConst = false;
        }
        private void SetProxy(IStoryValue<object> proxy)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = proxy;
            m_Value = default(T);
        }
        private void SetValue(T val)
        {
            m_HaveValue = true;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_StackName = null;
            m_Proxy = null;
            m_Value = val;
            m_IsConst = true;
        }
        private void CalcInitValue(Dsl.ISyntaxComponent param)
        {
            IStoryValue<object> val = StoryValueManager.Instance.CalcValue(param);
            if (null != val) {
                //对初始化即能求得值的函数，不需要再记录函数表达式，直接转换为常量值。
                if (val.HaveValue) {
                    SetValue(StoryValueHelper.CastTo<T>(val.Value));
                } else {
                    SetProxy(val);
                }
            } else if (param is Dsl.ValueData) {
                string id = param.GetId();
                int idType = param.GetIdType();
                if (idType == Dsl.ValueData.NUM_TOKEN) {
                    if (id.IndexOf('.') >= 0)
                        SetValue(StoryValueHelper.CastTo<T>(float.Parse(id, System.Globalization.NumberStyles.Float)));
                    else if (id.StartsWith("0x"))
                        SetValue(StoryValueHelper.CastTo<T>(uint.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber)));
                    else
                        SetValue(StoryValueHelper.CastTo<T>(int.Parse(id, System.Globalization.NumberStyles.Integer)));
                } else if (idType == Dsl.ValueData.BOOL_TOKEN) {
                    SetValue(StoryValueHelper.CastTo<T>(id == "true"));
                } else {
                    SetValue(StoryValueHelper.CastTo<T>(id));
                }
            } else {
#if DEBUG
                string err = string.Format("Unknown value, id:{0} line:{1}", param.GetId(), param.GetLine());
                throw new Exception(err);
#else
        GameFramework.LogSystem.Error("Unknown value, id:{0}", param.GetId());
#endif
            }
        }
        private bool m_HaveValue = false;
        private int m_ArgIndex = c_NotArg;
        private string m_LocalName = null;
        private string m_GlobalName = null;
        private string m_StackName = null;
        private IStoryValue<object> m_Proxy = null;
        private T m_Value;
        private bool m_IsConst = false;
    }
    internal sealed class StoryValueAdapter<T> : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_Original.InitFromDsl(param);
        }
        public IStoryValue<object> Clone()
        {
            IStoryValue<T> newOriginal = m_Original.Clone();
            StoryValueAdapter<T> val = new StoryValueAdapter<T>(newOriginal);
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Original.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get
            {
                return m_Original.HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Original.Value;
            }
        }
        public StoryValueAdapter(IStoryValue<T> original)
        {
            m_Original = original;
        }
        private IStoryValue<T> m_Original = null;
    }
}
