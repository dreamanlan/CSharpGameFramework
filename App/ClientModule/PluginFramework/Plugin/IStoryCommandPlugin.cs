using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotnetStoryScript;

namespace ScriptableFramework.Plugin
{
    public interface IStoryCommandPlugin
    {
        IStoryCommandPlugin Clone();
        void ResetState();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);
        bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta);
        bool ExecCommandWithArgs(StoryInstance instance, long delta, object iterator, object[] args);
        bool LoadFuncData(Dsl.FunctionData funcData);
        bool LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryCommandPlugin
    {
        ISimpleStoryCommandPlugin Clone();
        void ResetState();
        bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params, long delta);
    }
}
