using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    public sealed class GetTimeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                return TimeUtility.GetLocalMilliseconds();
            }
            return 0;
        }
    }
    public sealed class GetTimeScaleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                return 1;
            }
            return 0;
        }
    }
    public sealed class GetEntityInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                return BoxedValue.FromObject(scene.SceneContext.GetEntityById(objId));
            }
            return BoxedValue.NullObject;
        }
    }
}
