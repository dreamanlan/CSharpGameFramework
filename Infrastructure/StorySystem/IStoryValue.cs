using System;
using System.Collections.Generic;

namespace StorySystem
{
    public enum StoryValueFlagMask : int
    {
        CONST_VALUE = 0x00,
        HAVE_ARG = 0x01,
        HAVE_VAR = 0x02,
        HAVE_ARG_AND_VAR = 0x03,
    }
    /// <summary>
    /// 描述剧情命令中用到的值，此接口用以支持参数、局部变量、全局变量与内建函数（返回一个剧情命令用到的值）。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStoryValue<T>
    {
        void InitFromDsl(Dsl.ISyntaxComponent param);//从DSL语言初始化值实例
        IStoryValue<T> Clone();//克隆一个新实例，每个值只从DSL语言初始化一次，之后的实例由克隆产生，提升性能
        void Substitute(object iterator, object[] args);//参数替换为参数值
        void Evaluate(StoryInstance instance);//计算StoryValue的值
        bool HaveValue { get; }//是否已经有值，对常量初始化后即产生值，对参数、变量与函数则在Evaluate后产生值
        T Value { get; }//具体的值
        int Flag { get; }
    }
    public sealed class StoryValue : IStoryValue<object>
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
                    if (0 == id.CompareTo("$$"))
                        SetArgument(c_Iterator);
                    else
                        SetArgument(int.Parse(id.Substring(1)));
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
            StoryValue obj = new StoryValue();
            obj.m_ArgIndex = m_ArgIndex;
            obj.m_LocalName = m_LocalName;
            obj.m_GlobalName = m_GlobalName;
            if (null != m_Proxy) {
                obj.m_Proxy = m_Proxy.Clone();
            }
            obj.m_HaveValue = m_HaveValue;
            obj.m_Value = m_Value;
            obj.m_Flag = m_Flag;
            return obj;
        }
        public void Substitute(object iterator, object[] args)
        {
            if (StoryValueHelper.IsConstValue(Flag))
                return;
            if (m_ArgIndex >= 0 && m_ArgIndex < args.Length) {
                m_Value = args[m_ArgIndex];
                m_HaveValue = true;
            } else if (m_ArgIndex == c_Iterator) {
                m_Value = iterator;
                m_HaveValue = true;
            } else if (null != m_Proxy) {
                m_Proxy.Substitute(iterator, args);
                if (m_Proxy.HaveValue) {
                    m_Value = m_Proxy.Value;
                    m_HaveValue = true;
                } else {
                    m_HaveValue = false;
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (StoryValueHelper.IsConstValue(Flag))
                return;
            if (null != m_LocalName) {
                Dictionary<string, object> locals = instance.LocalVariables;
                object val;
                if (locals.TryGetValue(m_LocalName, out val)) {
                    m_Value = val;
                    m_HaveValue = true;
                }
            } else if (null != m_GlobalName) {
                Dictionary<string, object> globals = instance.GlobalVariables;
                if (null != globals) {
                    object val;
                    if (globals.TryGetValue(m_GlobalName, out val)) {
                        m_Value = val;
                        m_HaveValue = true;
                    }
                }
            } else if (null != m_Proxy) {
                m_Proxy.Evaluate(instance);
                if (m_Proxy.HaveValue) {
                    m_Value = m_Proxy.Value;
                    m_HaveValue = true;
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }
        private void SetArgument(int index)
        {
            m_HaveValue = false;
            m_ArgIndex = index;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = null;
            m_Flag = (int)StoryValueFlagMask.HAVE_ARG;
        }
        private void SetLocal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = name;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = null;
            m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
        }
        private void SetGlobal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = name;
            m_Proxy = null;
            m_Value = null;
            m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
        }
        private void SetProxy(IStoryValue<object> proxy)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = proxy;
            m_Value = null;
            m_Flag = proxy.Flag;
        }
        private void SetValue(object val)
        {
            m_HaveValue = true;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = val;
            m_Flag = (int)StoryValueFlagMask.CONST_VALUE;
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

        private bool m_HaveValue = false;
        private int m_ArgIndex = c_NotArg;
        private string m_LocalName = null;
        private string m_GlobalName = null;
        private IStoryValue<object> m_Proxy = null;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    public sealed class StoryValue<T> : IStoryValue<T>
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
                    if (0 == id.CompareTo("$$"))
                        SetArgument(c_Iterator);
                    else
                        SetArgument(int.Parse(id.Substring(1)));
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
            StoryValue<T> obj = new StoryValue<T>();
            obj.m_ArgIndex = m_ArgIndex;
            obj.m_LocalName = m_LocalName;
            obj.m_GlobalName = m_GlobalName;
            if (null != m_Proxy) {
                obj.m_Proxy = m_Proxy.Clone();
            }
            obj.m_HaveValue = m_HaveValue;
            obj.m_Value = m_Value;
            return obj;
        }
        public void Substitute(object iterator, object[] args)
        {
            if (StoryValueHelper.IsConstValue(Flag))
                return;
            if (m_ArgIndex >= 0 && m_ArgIndex < args.Length) {
                m_Value = StoryValueHelper.CastTo<T>(args[m_ArgIndex]);
                m_HaveValue = true;
            } else if (m_ArgIndex == c_Iterator) {
                m_Value = StoryValueHelper.CastTo<T>(iterator);
                m_HaveValue = true;
            } else if (null != m_Proxy) {
                m_Proxy.Substitute(iterator, args);
                if (m_Proxy.HaveValue) {
                    m_Value = StoryValueHelper.CastTo<T>(m_Proxy.Value);
                    m_HaveValue = true;
                } else {
                    m_HaveValue = false;
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (StoryValueHelper.IsConstValue(Flag))
                return;
            if (null != m_LocalName) {
                Dictionary<string, object> locals = instance.LocalVariables;
                object val;
                if (locals.TryGetValue(m_LocalName, out val)) {
                    m_Value = StoryValueHelper.CastTo<T>(val);
                    m_HaveValue = true;
                }
            } else if (null != m_GlobalName) {
                Dictionary<string, object> globals = instance.GlobalVariables;
                if (null != globals) {
                    object val;
                    if (globals.TryGetValue(m_GlobalName, out val)) {
                        m_Value = StoryValueHelper.CastTo<T>(val);
                        m_HaveValue = true;
                    }
                }
            } else if (null != m_Proxy) {
                m_Proxy.Evaluate(instance);
                if (m_Proxy.HaveValue) {
                    m_Value = StoryValueHelper.CastTo<T>(m_Proxy.Value);
                    m_HaveValue = true;
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void SetArgument(int index)
        {
            m_HaveValue = false;
            m_ArgIndex = index;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = default(T);
            m_Flag = (int)StoryValueFlagMask.HAVE_ARG;
        }
        private void SetLocal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = name;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = default(T);
            m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
        }
        private void SetGlobal(string name)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = name;
            m_Proxy = null;
            m_Value = default(T);
            m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
        }
        private void SetProxy(IStoryValue<object> proxy)
        {
            m_HaveValue = false;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = proxy;
            m_Value = default(T);
            m_Flag = proxy.Flag;
        }
        private void SetValue(T val)
        {
            m_HaveValue = true;
            m_ArgIndex = c_NotArg;
            m_LocalName = null;
            m_GlobalName = null;
            m_Proxy = null;
            m_Value = val;
            m_Flag = (int)StoryValueFlagMask.CONST_VALUE;
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
        private IStoryValue<object> m_Proxy = null;
        private T m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
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
        public void Substitute(object iterator, object[] args)
        {
            m_Original.Substitute(iterator, args);
        }
        public void Evaluate(StoryInstance instance)
        {
            m_Original.Evaluate(instance);
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
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        public StoryValueAdapter(IStoryValue<T> original)
        {
            m_Original = original;
            m_Flag = original.Flag;
        }

        private IStoryValue<T> m_Original = null;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
