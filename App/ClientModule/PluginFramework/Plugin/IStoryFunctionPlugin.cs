using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorySystem;

namespace GameFramework.Plugin
{
    public interface IStoryFunctionPlugin
    {
        void SetProxy(StoryValueResult result);
        IStoryFunctionPlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryFunctionPlugin
    {
        void SetProxy(StoryValueResult result);
        ISimpleStoryFunctionPlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params);
    }
}
