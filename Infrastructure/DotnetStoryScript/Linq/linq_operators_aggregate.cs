using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterAggregate()
        {
            var opAggregate = new OpAggregateOperator();
            Register("aggregate", opAggregate);
            Register("reduce", opAggregate);
        }
    }

    internal sealed class OpAggregateOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            BoxedValue acc = BoxedValue.NullObject;
            BoxedValue prevDollar = calcContext.GetVariable("$$");
            BoxedValue prevAcc = calcContext.GetVariable("$$acc");
            if (exprs.Count >= 2) {
                acc = Enumerable.First(exprs).Calc();
                var folder = Enumerable.Last(exprs);
                while (src.MoveNext()) {
                    calcContext.SetVariable("$$acc", acc);
                    calcContext.SetVariable("$$", src.Current);
                    acc = folder.Calc();
                }
            }
            else if (exprs.Count == 1) {
                if (!src.MoveNext()) {
                    calcContext.SetVariable("$$", prevDollar);
                    calcContext.SetVariable("$$acc", prevAcc);
                    return BoxedValue.NullObject;
                }
                acc = src.Current;
                var folder = Enumerable.First(exprs);
                while (src.MoveNext()) {
                    calcContext.SetVariable("$$acc", acc);
                    calcContext.SetVariable("$$", src.Current);
                    acc = folder.Calc();
                }
            }
            calcContext.SetVariable("$$", prevDollar);
            calcContext.SetVariable("$$acc", prevAcc);
            return acc;
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            BoxedValue acc = BoxedValue.NullObject;
            BoxedValue prevDollar = calcContext.GetVariable("$$");
            BoxedValue prevAcc = calcContext.GetVariable("$$acc");
            if (exprs.Count >= 2) {
                var seedExpr = Enumerable.First(exprs);
                if (seedExpr.IsAsync) {
                    var _ei = seedExpr.Calc(result);
                    while (_ei.MoveNext()) {
                        yield return _ei.Current;
                    }
                    acc = result.Value;
                }
                else {
                    acc = seedExpr.Calc();
                }

                var folder = Enumerable.Last(exprs);
                while (true) {
                    var _eiSrc = src.MoveNext(result);
                    while (_eiSrc.MoveNext()) {
                        yield return _eiSrc.Current;
                    }
                    if (!result.Value.GetBool()) {
                        break;
                    }

                    calcContext.SetVariable("$$acc", acc);
                    calcContext.SetVariable("$$", src.Current);

                    if (folder.IsAsync) {
                        var _eiCalc = folder.Calc(result);
                        while (_eiCalc.MoveNext()) {
                            yield return _eiCalc.Current;
                        }
                        acc = result.Value;
                    }
                    else {
                        acc = folder.Calc();
                    }
                }
            }
            else if (exprs.Count == 1) {
                var _eiSrcInit = src.MoveNext(result);
                while (_eiSrcInit.MoveNext()) {
                    yield return _eiSrcInit.Current;
                }
                if (!result.Value.GetBool()) {
                    calcContext.SetVariable("$$", prevDollar);
                    calcContext.SetVariable("$$acc", prevAcc);
                    result.Value = BoxedValue.NullObject;
                    yield break;
                }

                acc = src.Current;
                var folder = Enumerable.First(exprs);
                while (true) {
                    var _eiSrc = src.MoveNext(result);
                    while (_eiSrc.MoveNext()) {
                        yield return _eiSrc.Current;
                    }
                    if (!result.Value.GetBool()) {
                        break;
                    }

                    calcContext.SetVariable("$$acc", acc);
                    calcContext.SetVariable("$$", src.Current);

                    if (folder.IsAsync) {
                        var _eiCalc = folder.Calc(result);
                        while (_eiCalc.MoveNext()) {
                            yield return _eiCalc.Current;
                        }
                        acc = result.Value;
                    }
                    else {
                        acc = folder.Calc();
                    }
                }
            }
            calcContext.SetVariable("$$", prevDollar);
            calcContext.SetVariable("$$acc", prevAcc);
            result.Value = acc;
        }
    }
}
