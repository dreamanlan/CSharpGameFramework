using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterCount()
        {
            Register("count", new OpCountOperator());
        }
    }

    internal sealed class OpCountOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            long ct = 0;
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (src.MoveNext()) {
                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        if (Enumerable.First(exprs).Calc().GetLong() != 0) {
                            ct++;
                        }
                    }
                    else {
                        ct++;
                    }
                }
                return BoxedValue.From((int)ct);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            long ct = 0;
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (true) {
                    var _eiSrc = src.MoveNext(result);
                    try {
                        while (_eiSrc.MoveNext()) {
                            yield return _eiSrc.Current;
                        }
                    }
                    finally {
                        (_eiSrc as IDisposable)?.Dispose();
                    }
                    if (!result.Value.GetBool()) {
                        break;
                    }

                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        var condExpr = Enumerable.First(exprs);
                        BoxedValue condRes;
                        if (condExpr.IsAsync) {
                            var _eiCalc = condExpr.Calc(result);
                            try {
                                while (_eiCalc.MoveNext()) {
                                    yield return _eiCalc.Current;
                                }
                            }
                            finally {
                                (_eiCalc as IDisposable)?.Dispose();
                            }
                            condRes = result.Value;
                        }
                        else {
                            condRes = condExpr.Calc();
                        }
                        if (condRes.GetLong() != 0) {
                            ct++;
                        }
                    }
                    else {
                        ct++;
                    }
                }
                result.Value = BoxedValue.From((int)ct);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }
    }
}
