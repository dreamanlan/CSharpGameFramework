using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.AttrCalc;

namespace ScriptableFramework.Plugin
{
    public interface IAttrExpressionPlugin
    {
        void SetCalculator(DslCalculator calc);
        long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args);
        bool LoadValue(Dsl.ValueData valData);
        bool LoadCallData(Dsl.FunctionData callData);
        bool LoadExpressions(AttrExpressionList exps);
        bool LoadFuncData(Dsl.FunctionData funcData);
        bool LoadStatementData(Dsl.StatementData statementData);
    }
}
