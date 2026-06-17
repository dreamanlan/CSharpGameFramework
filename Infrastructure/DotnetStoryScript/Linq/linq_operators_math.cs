using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterMath()
        {
            Register("sum", new OpMathOperator(0));
            Register("min", new OpMathOperator(1));
            Register("max", new OpMathOperator(2));
            Register("average", new OpMathOperator(3));
        }
    }

    internal sealed class OpMathOperator : ILinqAsyncOperator
    {
        private int _type; // 0:sum, 1:min, 2:max, 3:avg
        public OpMathOperator(int type) { _type = type; }

        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            double sum = 0;
            double min = double.MaxValue;
            double max = double.MinValue;
            long ct = 0;
            BoxedValue prev = calcContext.GetVariable("$$");

            while (src.MoveNext()) {
                BoxedValue v = src.Current;
                if (exprs.Count > 0) {
                    calcContext.SetVariable("$$", src.Current);
                    v = Enumerable.First(exprs).Calc();
                }

                double val = v.GetDouble();

                ct++;
                sum += val;
                if (val < min) { min = val; }
                if (val > max) { max = val; }
            }
            calcContext.SetVariable("$$", prev);

            if (ct == 0) {
                return BoxedValue.NullObject;
            }
            if (_type == 0) { return BoxedValue.From(sum); }
            if (_type == 1) { return BoxedValue.From(min); }
            if (_type == 2) { return BoxedValue.From(max); }
            return BoxedValue.From(sum / ct);
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            double sum = 0;
            double min = double.MaxValue;
            double max = double.MinValue;
            long ct = 0;
            BoxedValue prev = calcContext.GetVariable("$$");

            while (true) {
                var _eiSrc = src.MoveNext(result);
                while (_eiSrc.MoveNext()) {
                    yield return _eiSrc.Current;
                }
                if (!result.Value.GetBool()) {
                    break;
                }

                BoxedValue v = src.Current;
                if (exprs.Count > 0) {
                    calcContext.SetVariable("$$", src.Current);
                    var mathExpr = Enumerable.First(exprs);
                    if (mathExpr.IsAsync) {
                        var _eiCalc = mathExpr.Calc(result);
                        while (_eiCalc.MoveNext()) {
                            yield return _eiCalc.Current;
                        }
                        v = result.Value;
                    }
                    else {
                        v = mathExpr.Calc();
                    }
                }

                double val = v.GetDouble();

                ct++;
                sum += val;
                if (val < min) { min = val; }
                if (val > max) { max = val; }
            }
            calcContext.SetVariable("$$", prev);

            if (ct == 0) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            if (_type == 0) { result.Value = BoxedValue.From(sum); }
            else if (_type == 1) { result.Value = BoxedValue.From(min); }
            else if (_type == 2) { result.Value = BoxedValue.From(max); }
            else { result.Value = BoxedValue.From(sum / ct); }
        }
    }
}
