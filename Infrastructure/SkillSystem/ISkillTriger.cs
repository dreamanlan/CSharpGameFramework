using System;
using System.Collections.Generic;
using System.Text;

namespace SkillSystem
{
    public delegate void VisitPropertyDelegation(string group, string key, IProperty property);
    public interface IPropertyVisitor
    {
        void VisitProperties(VisitPropertyDelegation callback);
    }
    public interface IProperty
    {
        object Value { get; set; }
    }
    public interface IPropertyAccessor
    {
        bool TryGetProperty(string key, out object val);//获取变量值
        void SetProperty(string key, object val);//设置变量值
    }
    public sealed class PropertyAccessorHelper : IPropertyVisitor, IPropertyAccessor
    {
        private class PropertyWrap : IProperty
        {
            public object Value
            {
                get
                {
                    object val = null;
                    if (null != OnGet) {
                        val = OnGet();
                    }
                    return val;
                }
                set
                {
                    if (null != OnSet) {
                        OnSet(value);
                    }
                }
            }

            internal GetDelegation OnGet;
            internal SetDelegation OnSet;
        }
        public delegate object GetDelegation();
        public delegate void SetDelegation(object val);

        public void VisitProperties(VisitPropertyDelegation callback)
        {
            try {
                foreach (var pair in m_Properties) {
                    callback(m_Group, pair.Key, pair.Value);
                }
            } catch (Exception ex) {
                GameFramework.LogSystem.Error("VisitVariables throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        public bool TryGetProperty(string key, out object val)
        {
            try {
                PropertyWrap wrap;
                if (m_Properties.TryGetValue(key, out wrap)) {
                    val = wrap.OnGet();
                    return true;
                } else {
                    val = null;
                    return false;
                }
            } catch (Exception ex) {
                GameFramework.LogSystem.Error("TryGetVariable {0} throw exception:{1}\n{2}", key, ex.Message, ex.StackTrace);
                val = null;
                return false;
            }
        }
        public void SetProperty(string key, object val)
        {
            try {
                PropertyWrap wrap;
                if (m_Properties.TryGetValue(key, out wrap)) {
                    wrap.OnSet(val);
                }
            } catch (Exception ex) {
                GameFramework.LogSystem.Error("SetVariable {0} {1} throw exception:{2}\n{3}", key, null == val ? "null" : val.ToString(), ex.Message, ex.StackTrace);
            }
        }
        public void SetGroup(string group)
        {
            m_Group = group;
        }
        public void AddProperty(string key, GetDelegation onGet)
        {
            m_Properties.Add(key, new PropertyWrap { OnGet = onGet, OnSet = null });
        }
        public void AddProperty(string key, GetDelegation onGet, SetDelegation onSet)
        {
            m_Properties.Add(key, new PropertyWrap { OnGet = onGet, OnSet = onSet });
        }

        private string m_Group = string.Empty;
        private Dictionary<string, PropertyWrap> m_Properties = new Dictionary<string, PropertyWrap>();
    }
    public interface ISkillTriger : IPropertyVisitor, IPropertyAccessor
    {
        long StartTime { get; set; }
        string Name { get; set; }
        int OrderInSkill { get; set; }
        int OrderInSection { get; set; }
        bool IsFinal { get; set; }
        ISkillTriger Clone();//克隆触发器，触发器只会从DSL实例一次，之后都通过克隆产生新实例
        void Init(Dsl.ISyntaxComponent config, SkillInstance instance);//从DSL语言初始化触发器实例
        void InitProperties();
        void Reset();//复位触发器到初始状态
        bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime);//执行触发器，返回false表示触发器结束，下一tick不再执行
    }
    public abstract class AbstractSkillTriger : ISkillTriger
    {
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                m_AccessorHelper.SetGroup(m_Name);
            }
        }
        public long StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }
        public int OrderInSkill
        {
            get { return m_OrderInSkill; }
            set { m_OrderInSkill = value; }
        }
        public int OrderInSection
        {
            get { return m_OrderInSection; }
            set { m_OrderInSection = value; }
        }
        public bool IsFinal
        {
            get { return m_IsFinal; }
            set { m_IsFinal = value; }
        }
        public void VisitProperties(VisitPropertyDelegation callback)
        {
            m_AccessorHelper.VisitProperties(callback);
        }
        public bool TryGetProperty(string key, out object val)
        {
            return m_AccessorHelper.TryGetProperty(key, out val);
        }
        public void SetProperty(string key, object val)
        {
            m_AccessorHelper.SetProperty(key, val);
        }

        public void Init(Dsl.ISyntaxComponent config, SkillInstance instance)
        {
            m_Name = config.GetId();
            Dsl.FunctionData callData = config as Dsl.FunctionData;
            if (null != callData) {
                Load(callData, instance);
            } else {
                Dsl.FunctionData funcData = config as Dsl.FunctionData;
                if (null != funcData) {
                    Load(funcData, instance);
                } else {
                    Dsl.StatementData statementData = config as Dsl.StatementData;
                    if (null != statementData) {
                        Load(statementData, instance);
                    } else {
                        //error
                    }
                }
            }
            InitProperties();
        }
        public ISkillTriger Clone()
        {
            ISkillTriger newObj = OnClone();
            CopyTo(newObj);
            return newObj;
        }
        public void InitProperties()
        {
            AddProperty("StartTime", () => { return m_StartTime; }, (object val) => { m_StartTime = (long)Convert.ChangeType(val, typeof(long)); });
            OnInitProperties();
        }
        public virtual void Reset() { }
        public virtual bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime) { return false; }
        
        protected abstract ISkillTriger OnClone();

        protected virtual void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
        }
        protected virtual void Load(Dsl.StatementData statementData, SkillInstance instance)
        {
        }

        protected virtual void OnInitProperties()
        {
        }

        //
        public void CopyTo(ISkillTriger newObj)
        {
            newObj.StartTime = StartTime;
            newObj.Name = Name;
            newObj.OrderInSkill = OrderInSkill;
            newObj.OrderInSection = OrderInSection;
            newObj.IsFinal = IsFinal;
        }

        //下面方法必须在子类的构造或重载的OnInitProperties里调用！
        public void AddProperty(string key, PropertyAccessorHelper.GetDelegation onGet, PropertyAccessorHelper.SetDelegation onSet)
        {
            m_AccessorHelper.AddProperty(key, onGet, onSet);
        }

        private string m_Name = string.Empty;
        private long m_StartTime = 0;
        private int m_OrderInSkill = 0;
        private int m_OrderInSection = 0;
        private bool m_IsFinal = true;
        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
        
        //用于同步修改回加载Dsl实例的工具方法
        public static void SetParam(Dsl.FunctionData callData, int index, string val)
        {
            Dsl.ValueData valData = callData.GetParam(index) as Dsl.ValueData;
            if (null != valData) {
                int idType = valData.GetIdType();
                valData.SetId(val);
                valData.SetType(idType);
            }
        }
        public static void SetParam(Dsl.StatementData statementData, int funcIndex, int index, string val)
        {
            if (funcIndex >= 0 && funcIndex < statementData.Functions.Count) {
                Dsl.FunctionData funcData = statementData.Functions[funcIndex].AsFunction;
                SetParam(funcData, index, val);
            }
        }
        public static void SetStatementParam(Dsl.FunctionData funcData, int stIndex, int paramIndex, string val)
        {
            Dsl.FunctionData callData = funcData.GetParam(stIndex) as Dsl.FunctionData;
            SetParam(callData, paramIndex, val);
        }
        public static void SetStatementParam(Dsl.StatementData statementData, int funcIndex, int stIndex, int paramIndex, string val)
        {
            if (funcIndex >= 0 && funcIndex < statementData.GetFunctionNum()) {
                Dsl.FunctionData funcData = statementData.GetFunction(funcIndex).AsFunction;
                SetStatementParam(funcData, stIndex, paramIndex, val);
            }
        }
    }
    public class DummyTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DummyTriger cmd = new DummyTriger();
            return cmd;
        }
    }
    public class SkillTriggerProxy
    {
        public SkillTriggerProxy(AbstractSkillTriger trigger)
        {
            m_RealTrigger = trigger;
        }
        public void DoClone(SkillTriggerProxy other)
        {
            other.m_RealTrigger.CopyTo(m_RealTrigger);
        }
        public string Name
        {
            get { return m_RealTrigger.Name; }
            set { m_RealTrigger.Name = value; }
        }
        public long StartTime
        {
            get { return m_RealTrigger.StartTime; }
            set { m_RealTrigger.StartTime = value; }
        }
        public int OrderInSkill
        {
            get { return m_RealTrigger.OrderInSkill; }
            set { m_RealTrigger.OrderInSkill = value; }
        }
        public int OrderInSection
        {
            get { return m_RealTrigger.OrderInSection; }
            set { m_RealTrigger.OrderInSection = value; }
        }
        public bool IsFinal
        {
            get { return m_RealTrigger.IsFinal; }
            set { m_RealTrigger.IsFinal = value; }
        }
        public void AddProperty(string key, PropertyAccessorHelper.GetDelegation onGet, PropertyAccessorHelper.SetDelegation onSet)
        {
            m_RealTrigger.AddProperty(key, onGet, onSet);
        }
        
        private AbstractSkillTriger m_RealTrigger = null;
    }
    public sealed class SkillParamUtility
    {
        public static string RefixResourceVariable(string key, SkillInstance instance, Dictionary<string, string> resources)
        {
            if (key.IndexOf("/") >= 0) {
                return key;
            } else {
                string ret;
                if (resources.TryGetValue(key, out ret)) {
                    return ret;
                } else {
                    object val;
                    if (instance.Variables.TryGetValue(key, out val)) {
                        return val.ToString();
                    } else {
                        return key;
                    }
                }
            }
        }
        public static string RefixStringVariable(string key, SkillInstance instance)
        {
            object val;
            if (instance.Variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            return key;
        }
        public static T RefixNonStringVariable<T>(string key, SkillInstance instance)
        {
            object val;
            if (instance.Variables.TryGetValue(key, out val)) {
                return (T)Convert.ChangeType(val,typeof(T));
            }
            return default(T);
        }
        public static object RefixObjectVariable(string key, SkillInstance instance)
        {
            object val;
            if (instance.Variables.TryGetValue(key, out val)) {
                return val;
            }
            return key;
        }
    }
    public class SkillResourceParam
    {
        public void CopyFrom(SkillResourceParam other)
        {
            m_KeyOrValue = other.m_KeyOrValue;
        }
        public void Set(string val)
        {
            m_KeyOrValue = val;
        }
        public string Get()
        {
            return m_KeyOrValue as string;
        }
        public void Set(Dsl.ISyntaxComponent p)
        {
            m_KeyOrValue = p.GetId();
        }
        public string Get(SkillInstance instance, Dictionary<string, string> resources)
        {
            return SkillParamUtility.RefixResourceVariable(m_KeyOrValue, instance, resources);
        }
        public object EditableValue
        {
            get
            {
                return m_KeyOrValue;
            }
            set
            {
                m_KeyOrValue = value as string;
            }
        }

        private string m_KeyOrValue = string.Empty;
    }
    public class SkillStringParam
    {
        public void CopyFrom(SkillStringParam other)
        {
            m_KeyOrValue = other.m_KeyOrValue;
        }
        public void Set(string val)
        {
            m_KeyOrValue = val;
        }
        public void Set(Dsl.ISyntaxComponent p)
        {
            m_KeyOrValue = p.GetId();
        }
        public string Get(SkillInstance instance)
        {
            return SkillParamUtility.RefixStringVariable(m_KeyOrValue, instance);
        }
        public object EditableValue
        {
            get
            {
                return m_KeyOrValue;
            }
            set
            {
                m_KeyOrValue = value as string;
            }
        }

        private string m_KeyOrValue = string.Empty;
    }
    public class SkillNonStringParam<T>
    {
        public void CopyFrom(SkillNonStringParam<T> other)
        {
            m_Key = other.m_Key;
            m_Value = other.m_Value;
        }
        public void Set(T val)
        {
            m_Key = string.Empty;
            m_Value = val;
        }
        public void Set(Dsl.ISyntaxComponent p)
        {
            string val = p.GetId();
            int type = p.GetIdType();
            if (!string.IsNullOrEmpty(val)) {
                if (type == Dsl.FunctionData.NUM_TOKEN) {
                    m_Key = string.Empty;
                    m_Value = (T)Convert.ChangeType(val, typeof(T));
                } else {
                    m_Key = val;
                }
            }
        }
        public T Get(SkillInstance instance)
        {
            if (string.IsNullOrEmpty(m_Key)) {
                return m_Value;
            } else {
                return SkillParamUtility.RefixNonStringVariable<T>(m_Key, instance);
            }
        }
        public object EditableValue
        {
            get
            {
                if (string.IsNullOrEmpty(m_Key))
                    return m_Value;
                else
                    return m_Key;
            }
            set
            {
                if (string.IsNullOrEmpty(m_Key))
                    m_Value = (T)Convert.ChangeType(value, typeof(T));
                else
                    m_Key = value as string;
            }
        }

        private string m_Key = string.Empty;
        private T m_Value;
    }
    public class SkillIntParam : SkillNonStringParam<int>
    { }
    public class SkillLongParam : SkillNonStringParam<long>
    { }
    public class SkillFloatParam : SkillNonStringParam<float>
    { }
    public class SkillDoubleParam : SkillNonStringParam<double>
    { }
    public class SkillObjectParam
    {
        public void CopyFrom(SkillObjectParam other)
        {
            m_Key = other.m_Key;
            m_Value = other.m_Value;
        }
        public void Set(object val)
        {
            m_Key = string.Empty;
            m_Value = val;
        }
        public void Set(Dsl.ISyntaxComponent p)
        {
            string val = p.GetId();
            int type = p.GetIdType();
            if (!string.IsNullOrEmpty(val)) {
                if (type == Dsl.FunctionData.NUM_TOKEN) {
                    m_Key = string.Empty;
                    if (val.IndexOf('.') >= 0) {
                        m_Value = Convert.ChangeType(val, typeof(float));
                    } else {
                        m_Value = Convert.ChangeType(val, typeof(int));
                    }
                } else {
                    m_Key = val;
                }
            }
        }
        public object Get(SkillInstance instance)
        {
            if (string.IsNullOrEmpty(m_Key)) {
                return m_Value;
            } else {
                return SkillParamUtility.RefixObjectVariable(m_Key, instance);
            }
        }
        public object EditableValue
        {
            get
            {
                if (string.IsNullOrEmpty(m_Key))
                    return m_Value;
                else
                    return m_Key;
            }
            set
            {
                m_Key = value as string;
                if (null == m_Key) {
                    m_Key = string.Empty;
                    m_Value = value;
                } else {
                    if (m_Key.Length > 0) {
                        if (char.IsDigit(m_Key[0]) || m_Key[0] == '.' || m_Key[0] == '+' || m_Key[0] == '-') {
                            if (m_Key.IndexOf('.') >= 0) {
                                m_Value = (float)Convert.ChangeType(m_Key, typeof(float));
                                m_Key = string.Empty;
                            } else {
                                m_Value = (int)Convert.ChangeType(m_Key, typeof(int));
                                m_Key = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private string m_Key = string.Empty;
        private object m_Value;
    }
}
