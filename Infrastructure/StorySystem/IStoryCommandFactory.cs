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
                if (first.HaveId() && !first.HaveParamOrStatement()) {
                    //命令行样式转换为函数样式
                    var func = new Dsl.FunctionData();
                    func.CopyFrom(first);
                    func.SetParamClass((int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                    for (int i = 1; i < statementData.GetFunctionNum(); ++i) {
                        var fd = statementData.GetFunction(i);
                        if (fd.HaveId() && !fd.HaveParamOrStatement()) {
                            func.AddParam(fd.Name);
                        }
                        else {
                            func.AddParam(fd);
                        }
                    }
                    result = func;
                    ret = true;
                }
            }
            return ret;
        }
    }
}
