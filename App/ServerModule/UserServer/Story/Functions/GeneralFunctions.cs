using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class GetUserInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                var userGuid = operands[0].GetULong();
                return BoxedValue.FromObject(UserServer.Instance.UserProcessScheduler.GetUserInfo(userGuid));
            }
            return BoxedValue.NullObject;
        }
    }
}
