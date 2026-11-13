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
        void Init(DslCalculatorCodeStructure code);
        bool Calc(DslCalculator calculator, AsyncCalcResult result);
        void LoadFuncData(Dsl.FunctionData funcData);
        void LoadStatementData(Dsl.StatementData statementData);
    }
    public interface ISimpleStoryApiPlugin
    {
        void Init(DslCalculatorCodeStructure code);
        bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result);
    }
}
