using System;
using System.Collections.Generic;
using System.Text;

namespace SkillSystem
{
    public interface ISkillTriger
    {
        ISkillTriger Clone();//克隆触发器，触发器只会从DSL实例一次，之后都通过克隆产生新实例
        long GetStartTime();
        void Init(Dsl.ISyntaxComponent config, int dslSkillId);//从DSL语言初始化触发器实例
        void Reset();//复位触发器到初始状态
        bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime);//执行触发器，返回false表示触发器结束，下一tick不再执行
        void Analyze(object sender, SkillInstance instance);//语义分析，配合上下文sender与instance进行语义分析，在执行前收集必要的信息
    }
    public abstract class AbstractSkillTriger : ISkillTriger
    {
        public virtual long GetStartTime()
        {
            return m_StartTime;
        }

        public void Init(Dsl.ISyntaxComponent config, int dslSkillId)
        {
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
        public abstract ISkillTriger Clone();
        public virtual void Reset() { }
        public virtual bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime) { return false; }
        public virtual void Analyze(object sender, SkillInstance instance) { }

        protected virtual void Load(Dsl.CallData callData, int dslSkillId)
        {
        }
        protected virtual void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
        }
        protected virtual void Load(Dsl.StatementData statementData, int dslSkillId)
        {
        }

        protected long m_StartTime = 0;
    }
}
