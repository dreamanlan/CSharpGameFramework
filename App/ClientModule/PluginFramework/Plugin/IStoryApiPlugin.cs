using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

namespace ScriptableFramework
{
    public interface IStoryApiPlugin
    {
        void Init(DslCalculator calculator);
        bool Calc(AsyncCalcResult result);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryApiPlugin
    {
        void Init(DslCalculator calculator);
        bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result);
    }
}
