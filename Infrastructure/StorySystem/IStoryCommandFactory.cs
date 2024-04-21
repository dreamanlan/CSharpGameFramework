using System;
using System.Collections.Generic;
namespace StorySystem
{
    public interface IStoryCommandFactory
    {
        IStoryCommand Create();
    }
    public class StoryCommandFactoryHelper<T> : IStoryCommandFactory where T : IStoryCommand, new()
    {
        public IStoryCommand Create()
        {
            T t = new T();
            return t;
        }
    }
    public static class DslSyntaxTransformer
    {
        public static bool TryTransformCommandLineLikeSyntax(Dsl.StatementData statementData, out Dsl.FunctionData result)
        {
            bool ret = false;
            result = null;
            if (null != statementData) {
                var first = statementData.First;
                if (first.HaveId() && first.IsValue) {
                    //Convert command line style to function style
                    var func = new Dsl.FunctionData();
                    func.Name = first.AsValue;
                    func.SetParamClass((int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                    for (int i = 1; i < statementData.GetFunctionNum(); ++i) {
                        var fd = statementData.GetFunction(i);
                        func.AddParam(fd);
                    }
                    result = func;
                    ret = true;
                }
            }
            return ret;
        }
    }
}
