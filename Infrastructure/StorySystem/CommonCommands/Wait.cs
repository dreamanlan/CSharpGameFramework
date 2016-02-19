using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
    /// <summary>
    /// sleep(milliseconds);
    /// </summary>
    internal class SleepCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SleepCommand cmd = new SleepCommand();
            cmd.m_Time = m_Time.Clone();
            cmd.m_Condition = m_Condition.Clone();
            cmd.m_HaveCondition = m_HaveCondition;
            return cmd;
        }

        protected override void ResetState()
        {
            m_CurTime = 0;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Time.Substitute(iterator, args);
            if (m_HaveCondition)
                m_Condition.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Time.Evaluate(instance);
            if (m_HaveCondition)
                m_Condition.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_HaveCondition && m_Condition.HaveValue && m_Condition.Value == 0) {
                return false;
            }
            int curTime = m_CurTime;
            m_CurTime += (int)delta;
            if (curTime <= m_Time.Value)
                return true;
            else
                return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Time.InitFromDsl(callData.GetParam(0));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadCondition(second.Call);
                }
            }
        }

        private void LoadCondition(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0 && callData.GetId() == "if") {
                m_HaveCondition = true;
                m_Condition.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_Time = new StoryValue<int>();
        private IStoryValue<int> m_Condition = new StoryValue<int>();
        private bool m_HaveCondition = false;
        private int m_CurTime = 0;
    }
}
