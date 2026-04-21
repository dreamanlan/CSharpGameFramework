using System;
using System.Collections.Generic;
using System.IO;
using DotnetStoryScript;

namespace DotnetStoryScript
{
    public static class DslSyntaxTransformer
    {
        public static bool TryTransformCommandLineLikeSyntax(Dsl.StatementData statementData, out Dsl.FunctionData result)
        {
            bool ret = false;
            result = null;
            if (null != statementData)
            {
                var first = statementData.First;
                if (first.HaveId() && first.IsValue)
                {
                    //Convert command line style to function style
                    var func = new Dsl.FunctionData();
                    func.Name = first.AsValue;
                    func.SetParamClass((int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES);
                    for (int i = 1; i < statementData.GetFunctionNum(); ++i)
                    {
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
