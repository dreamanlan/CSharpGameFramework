using System;
using System.Collections.Generic;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// sleep(milliseconds);
    /// </summary>
    internal sealed class SleepCommand : AbstractStoryCommand
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
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Time.Evaluate(instance, iterator, args);
            if (m_HaveCondition)
                m_Condition.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_HaveCondition && m_Condition.HaveValue && m_Condition.Value == 0) {
                return false;
            }
            int curTime = m_CurTime;
            m_CurTime += (int)delta;
            int val = m_Time.Value;
            if (curTime <= val && val < StoryValueHelper.c_MaxWaitCommandTime)
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
    /// <summary>
    /// realsleep(milliseconds);
    /// </summary>
    internal sealed class RealTimeSleepCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            RealTimeSleepCommand cmd = new RealTimeSleepCommand();
            cmd.m_Time = m_Time.Clone();
            cmd.m_Condition = m_Condition.Clone();
            cmd.m_HaveCondition = m_HaveCondition;
            return cmd;
        }
        protected override void ResetState()
        {
            m_RealStartTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Time.Evaluate(instance, iterator, args);
            if (m_HaveCondition)
                m_Condition.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_HaveCondition && m_Condition.HaveValue && m_Condition.Value == 0) {
                return false;
            }
            if (m_RealStartTime <= 0) {
                m_RealStartTime = (int)GameFramework.TimeUtility.GetLocalRealMilliseconds();
            }
            int curTime = (int)GameFramework.TimeUtility.GetLocalRealMilliseconds();
            int val = m_Time.Value;
            if (val < StoryValueHelper.c_MaxWaitCommandTime && m_RealStartTime + val < curTime)
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
        private int m_RealStartTime = 0;
    }
    /// <summary>
    /// storysleep(milliseconds);
    /// </summary>
    internal sealed class StorySleepCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            StorySleepCommand cmd = new StorySleepCommand();
            cmd.m_Time = m_Time.Clone();
            cmd.m_Condition = m_Condition.Clone();
            cmd.m_HaveCondition = m_HaveCondition;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Time.Evaluate(instance, iterator, args);
            if (m_HaveCondition)
                m_Condition.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_HaveCondition && m_Condition.HaveValue && m_Condition.Value == 0) {
                return false;
            }
            if (GameFramework.GlobalVariables.Instance.IsStorySkipped && m_CurTime > 0) {
                return false;
            }
            int curTime = m_CurTime;
            m_CurTime += (int)delta;
            int val = m_Time.Value;
            if (curTime <= val && val < StoryValueHelper.c_MaxWaitCommandTime)
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
    /// <summary>
    /// storyrealsleep(milliseconds);
    /// </summary>
    internal sealed class StoryRealTimeSleepCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            StoryRealTimeSleepCommand cmd = new StoryRealTimeSleepCommand();
            cmd.m_Time = m_Time.Clone();
            cmd.m_Condition = m_Condition.Clone();
            cmd.m_HaveCondition = m_HaveCondition;
            return cmd;
        }
        protected override void ResetState()
        {
            m_RealStartTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Time.Evaluate(instance, iterator, args);
            if (m_HaveCondition)
                m_Condition.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_HaveCondition && m_Condition.HaveValue && m_Condition.Value == 0) {
                return false;
            }
            if (GameFramework.GlobalVariables.Instance.IsStorySkipped && m_RealStartTime > 0) {
                return false;
            }
            if (m_RealStartTime <= 0) {
                m_RealStartTime = (int)GameFramework.TimeUtility.GetLocalRealMilliseconds();
            }
            int curTime = (int)GameFramework.TimeUtility.GetLocalRealMilliseconds();
            int val = m_Time.Value;
            if (val < StoryValueHelper.c_MaxWaitCommandTime && m_RealStartTime + val < curTime)
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
        private int m_RealStartTime = 0;
    }
}
