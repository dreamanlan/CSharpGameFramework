using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// storywait(ms)[condition(exp)] - wait for specified milliseconds (can be skipped if story is skipped)
    /// storywait(ms){
    ///     condition(exp);
    /// };
    /// </summary>
    internal sealed class StoryWaitExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            long waitTime = m_Time.Calc().GetInt();

            long startTime = TimeUtility.GetLocalMilliseconds();
            long targetTime = startTime + waitTime;
            while (TimeUtility.GetLocalMilliseconds() < targetTime) {
                if (StoryConfigManager.Instance.IsStorySkipped || StoryConfigManager.Instance.IsStorySpeedup)
                    break;
                if (m_HaveCondition && m_Condition != null) {
                    var condVal = m_Condition.Calc();
                    if (condVal.GetInt() == 0) {
                        break;
                    }
                }
                yield return null;
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder) {
                callData = funcData.LowerOrderFunction;
                LoadCondition(funcData.LowerOrderFunction);
            }
            if (null != callData && callData.HaveParam()) {
                m_Time = Calculator.Load(callData.GetParam(0));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    Dsl.FunctionData cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd && cd.GetId() == "condition") {
                        LoadCondition(cd);
                    }
                }
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadCondition(second);
                }
            }
            return true;
        }

        private void LoadCondition(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "condition" && callData.GetParamNum() > 0) {
                m_HaveCondition = true;
                m_Condition = Calculator.Load(callData.GetParam(0));
            }
        }

        private IExpression m_Time;
        private bool m_HaveCondition = false;
        private IExpression m_Condition;
    }

    /// <summary>
    /// storyrealtimewait(ms)[condition(exp)] - wait for specified real time (in milliseconds, can be skipped if story is skipped)
    /// storyrealtimewait(ms){
    ///     condition(exp);
    /// };
    /// </summary>
    internal sealed class StoryRealtimeWaitExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            int ms = m_Time.Calc().GetInt();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < ms) {
                if (StoryConfigManager.Instance.IsStorySkipped || StoryConfigManager.Instance.IsStorySpeedup)
                    break;
                if (m_HaveCondition && m_Condition != null) {
                    var condVal = m_Condition.Calc();
                    if (condVal.GetInt() == 0) {
                        break;
                    }
                }
                yield return null;
            }
            sw.Stop();
            result.Value = BoxedValue.FromObject((int)sw.ElapsedMilliseconds);
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder) {
                callData = funcData.LowerOrderFunction;
                LoadCondition(funcData.LowerOrderFunction);
            }
            if (null != callData && callData.HaveParam()) {
                m_Time = Calculator.Load(callData.GetParam(0));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    Dsl.FunctionData cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd && cd.GetId() == "condition") {
                        LoadCondition(cd);
                    }
                }
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadCondition(second);
                }
            }
            return true;
        }

        private void LoadCondition(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "condition" && callData.GetParamNum() > 0) {
                m_HaveCondition = true;
                m_Condition = Calculator.Load(callData.GetParam(0));
            }
        }

        private IExpression m_Time;
        private bool m_HaveCondition = false;
        private IExpression m_Condition;
    }
}
