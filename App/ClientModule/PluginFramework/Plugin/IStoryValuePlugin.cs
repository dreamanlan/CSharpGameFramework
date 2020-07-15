using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorySystem;

namespace GameFramework.Plugin
{
    public interface IStoryValuePlugin
    {
        void SetProxy(StoryValueResult result);
        IStoryValuePlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryValuePlugin
    {
        void SetProxy(StoryValueResult result);
        ISimpleStoryValuePlugin Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params);
    }
}
