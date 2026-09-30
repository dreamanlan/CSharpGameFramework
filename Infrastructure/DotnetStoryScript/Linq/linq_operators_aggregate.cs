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
            try {
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
                return acc;
            }
            finally {
                calcContext.SetVariable("$$", prevDollar);
                calcContext.SetVariable("$$acc", prevAcc);
            }
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            BoxedValue acc = BoxedValue.NullObject;
            BoxedValue prevDollar = calcContext.GetVariable("$$");
            BoxedValue prevAcc = calcContext.GetVariable("$$acc");
            try {
                if (exprs.Count >= 2) {
                    var seedExpr = Enumerable.First(exprs);
                    if (seedExpr.IsAsync) {
                        var _ei = seedExpr.Calc(result);
                        try {
                            while (_ei.MoveNext()) {
                                yield return _ei.Current;
                            }
                        }
                        finally {
                            (_ei as IDisposable)?.Dispose();
                        }
                        acc = result.Value;
                    }
                    else {
                        acc = seedExpr.Calc();
                    }

                    var folder = Enumerable.Last(exprs);
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

                        calcContext.SetVariable("$$acc", acc);
                        calcContext.SetVariable("$$", src.Current);

                        if (folder.IsAsync) {
                            var _eiCalc = folder.Calc(result);
                            try {
                                while (_eiCalc.MoveNext()) {
                                    yield return _eiCalc.Current;
                                }
                            }
                            finally {
                                (_eiCalc as IDisposable)?.Dispose();
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
                    try {
                        while (_eiSrcInit.MoveNext()) {
                            yield return _eiSrcInit.Current;
                        }
                    }
                    finally {
                        (_eiSrcInit as IDisposable)?.Dispose();
                    }
                    if (!result.Value.GetBool()) {
                        result.Value = BoxedValue.NullObject;
                        yield break;
                    }

                    acc = src.Current;
                    var folder = Enumerable.First(exprs);
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

                        calcContext.SetVariable("$$acc", acc);
                        calcContext.SetVariable("$$", src.Current);

                        if (folder.IsAsync) {
                            var _eiCalc = folder.Calc(result);
                            try {
                                while (_eiCalc.MoveNext()) {
                                    yield return _eiCalc.Current;
                                }
                            }
                            finally {
                                (_eiCalc as IDisposable)?.Dispose();
                            }
                            acc = result.Value;
                        }
                        else {
                            acc = folder.Calc();
                        }
                    }
                }
                result.Value = acc;
            }
            finally {
                calcContext.SetVariable("$$", prevDollar);
                calcContext.SetVariable("$$acc", prevAcc);
            }
        }
    }
}
