using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotnetStoryScript;

namespace ScriptableFramework.Plugin
{
    public interface IStoryFunctionPlugin
    {
        void SetProxy(StoryFunctionResult result);
        IStoryFunctionPlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryFunctionPlugin
    {
        void SetProxy(StoryFunctionResult result);
        ISimpleStoryFunctionPlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params);
    }
}
