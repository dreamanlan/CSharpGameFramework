using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    internal sealed class PropSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int num = operands.Count;
            if (num == 1) {
                var propId = operands[0].GetString();
                if (!calculator.TryGetVariable(propId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                calculator.SetVariable(propId, BoxedValue.NullObject);
                return v;
            }
            else if (num == 2) {
                var propId = operands[0].GetString();
                if (!calculator.TryGetVariable(propId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                var vv = operands[1];
                calculator.SetVariable(propId, vv);
                return v;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PropGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int num = operands.Count;
            if (num == 1) {
                var propId = operands[0].GetString();
                if (!calculator.TryGetVariable(propId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                return v;
            }
            else if (num == 2) {
                var propId = operands[0].GetString();
                var vv = operands[1];
                if (!calculator.TryGetVariable(propId, out var v)) {
                    v = vv;
                }
                return v;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PropExistsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool exists = false;
            int num = operands.Count;
            if (num == 1) {
                var propId = operands[0].GetString();
                exists = calculator.TryGetVariable(propId, out var v);
            }
            return BoxedValue.FromBool(exists);
        }
    }
    internal sealed class FuncExistsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool exists = false;
            int num = operands.Count;
            if (num == 1) {
                var funcId = operands[0].GetString();
                exists = calculator.TryGetFuncInfo(funcId, out var v);
            }
            return BoxedValue.FromBool(exists);
        }
    }
}
