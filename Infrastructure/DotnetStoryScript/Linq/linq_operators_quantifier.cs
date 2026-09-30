using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterQuantifier()
        {
            Register("any", new OpAnyOperator());
            Register("all", new OpAllOperator());
        }
    }

    internal sealed class OpAnyOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (src.MoveNext()) {
                    if (exprs.Count == 0) {
                        return BoxedValue.FromBool(true);
                    }
                    calcContext.SetVariable("$$", src.Current);
                    if (Enumerable.First(exprs).Calc().GetLong() != 0) {
                        return BoxedValue.FromBool(true);
                    }
                }
                return BoxedValue.FromBool(false);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
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

                    if (exprs.Count == 0) {
                        result.Value = BoxedValue.FromBool(true);
                        yield break;
                    }

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
                        result.Value = BoxedValue.FromBool(true);
                        yield break;
                    }
                }
                result.Value = BoxedValue.FromBool(false);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }
    }

    internal sealed class OpAllOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (src.MoveNext()) {
                    if (exprs.Count == 0) {
                        continue;
                    }
                    calcContext.SetVariable("$$", src.Current);
                    if (Enumerable.First(exprs).Calc().GetLong() == 0) {
                        return BoxedValue.FromBool(false);
                    }
                }
                return BoxedValue.FromBool(true);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
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

                    if (exprs.Count == 0) {
                        continue;
                    }

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

                    if (condRes.GetLong() == 0) {
                        result.Value = BoxedValue.FromBool(false);
                        yield break;
                    }
                }
                result.Value = BoxedValue.FromBool(true);
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }
    }
}
