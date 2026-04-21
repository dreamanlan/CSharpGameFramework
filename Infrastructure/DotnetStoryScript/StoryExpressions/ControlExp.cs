using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// print(msg...) - print a log message
    /// </summary>
    internal sealed class PrintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < operands.Count; i++) {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(operands[i].ToString());
            }
            LogSystem.Warn("{0}", sb.ToString());
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// printf(fmt, ...) - print a log message
    /// </summary>
    internal sealed class PrintfExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: printf(fmt,arg1,arg2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj.IsString) {
                    var fmt = obj.StringVal;
                    if (operands.Count > 1 && null != fmt) {
                        ArrayList arrayList = new ArrayList();
                        for (int i = 1; i < operands.Count; ++i) {
                            arrayList.Add(operands[i].GetObject());
                        }
                        LogSystem.Warn(fmt, arrayList.ToArray());
                    }
                    else {
                        LogSystem.Warn("{0}", obj.GetObject());
                    }
                }
                else {
                    LogSystem.Warn("{0}", obj.GetObject());
                }
            }
            return r;
        }
    }

    /// <summary>
    /// suspend() - suspend the current coroutine
    /// </summary>
    internal sealed class SuspendExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null) {
                storyInst.CurrentCoroutine.IsSuspended = true;
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// terminate() - terminate the current story instance
    /// </summary>
    internal sealed class TerminateExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null) {
                storyInst.IsTerminated = true;
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// pause() - pause the current story instance
    /// </summary>
    internal sealed class PauseExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null) {
                storyInst.IsPaused = true;
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storybreak([condition]) - break until condition is met or story is skipped/speedup
    /// </summary>
    internal sealed class StoryBreakExp : SimpleAsyncExpressionBase
    {
        protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
        {
            while (true) {
                if (m_HaveCondition) {
                    int condVal = m_ConditionExp.Calc().GetInt();
                    if (condVal == 0) {
                        break;
                    }
                }
                if (StoryConfigManager.Instance.IsStorySkipped) {
                    break;
                }
                if (StoryConfigManager.Instance.IsStorySpeedup) {
                    break;
                }
                if (StoryConfigManager.Instance.StoryEditorOpen && StoryConfigManager.Instance.StoryEditorContinue) {
                    StoryConfigManager.Instance.StoryEditorContinue = false;
                    break;
                }
                yield return null;
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_HaveCondition = true;
                m_ConditionExp = Calculator.Load(callData.GetParam(0));
            }
            return true;
        }

        private IExpression m_ConditionExp = null;
        private bool m_HaveCondition = false;
    }
}
