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
        public void AddProperty(string key, GetDelegation onGet, SetDelegation onSet)
        {
            m_Properties.Add(key, new PropertyWrap { OnGet = onGet, OnSet = onSet });
        }

        private string m_Group = string.Empty;
        private Dictionary<string, PropertyWrap> m_Properties = new Dictionary<string, PropertyWrap>();
    }
    public interface ISkillTriger : IPropertyVisitor, IPropertyAccessor
    {
        ISkillTriger Clone();//克隆触发器，触发器只会从DSL实例一次，之后都通过克隆产生新实例
        long StartTime { get; set; }
        string Name { get; set; }
        void Init(Dsl.ISyntaxComponent config, int dslSkillId);//从DSL语言初始化触发器实例
        void Reset();//复位触发器到初始状态
        bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime);//执行触发器，返回false表示触发器结束，下一tick不再执行
        void Analyze(object sender, SkillInstance instance);//语义分析，配合上下文sender与instance进行语义分析，在执行前收集必要的信息
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

        public void Init(Dsl.ISyntaxComponent config, int dslSkillId)
        {
            m_Name = config.GetId();
            Dsl.CallData callData = config as Dsl.CallData;
            if (null != callData) {
                Load(callData, dslSkillId);
            } else {
                Dsl.FunctionData funcData = config as Dsl.FunctionData;
                if (null != funcData) {
                    Load(funcData, dslSkillId);
                } else {
                    Dsl.StatementData statementData = config as Dsl.StatementData;
                    if (null != statementData) {
                        //是否支持语句类型的触发器语法？
                        Load(statementData, dslSkillId);
                    } else {
                        //error
                    }
                }
            }
        }
        public ISkillTriger Clone()
        {
            ISkillTriger newObj = OnClone();
            newObj.StartTime = StartTime;
            newObj.Name = Name;
            return newObj;
        }
        public virtual void Reset() { }
        public virtual bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime) { return false; }
        public virtual void Analyze(object sender, SkillInstance instance) { }

        protected abstract ISkillTriger OnClone();

        protected virtual void Load(Dsl.CallData callData, int dslSkillId)
        {
        }
        protected virtual void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
        }
        protected virtual void Load(Dsl.StatementData statementData, int dslSkillId)
        {
        }

        //下面方法必须在子类构造函数里调用，不要在别的地方调用！
        protected void AddProperty(string key, PropertyAccessorHelper.GetDelegation onGet, PropertyAccessorHelper.SetDelegation onSet)
        {
            m_AccessorHelper.AddProperty(key, onGet, onSet);
        }

        private string m_Name = string.Empty;
        private long m_StartTime = 0;
        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
    }
    public class DummyTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DummyTriger cmd = new DummyTriger();
            return cmd;
        }
    }
}
