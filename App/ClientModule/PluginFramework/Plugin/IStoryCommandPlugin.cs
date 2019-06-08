using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorySystem;

namespace GameFramework.Plugin
{
    public interface IStoryCommandPlugin
    {
        IStoryCommandPlugin Clone();
        void ResetState();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args);
        bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta);
        bool ExecCommandWithArgs(StoryInstance instance, long delta, object iterator, object[] args);
        void LoadCallData(Dsl.CallData callData);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryCommandPlugin
    {
        ISimpleStoryCommandPlugin Clone();
        void ResetState();
        bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta);
    }
}
