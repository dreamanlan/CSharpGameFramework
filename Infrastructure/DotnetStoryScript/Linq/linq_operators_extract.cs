using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterExtract()
        {
            Register("first", new OpFirstOperator());
            Register("last", new OpLastOperator());
            Register("tolist", new OpToListOperator());
        }
    }

    // FIRST Operator (Supports async matching short-circuit)
    internal sealed class OpFirstOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (src.MoveNext()) {
                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        if (Enumerable.First(exprs).Calc().GetLong() != 0) {
                            return src.Current;
                        }
                    }
                    else {
                        return src.Current;
                    }
                }
                return BoxedValue.NullObject;
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
                        while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                    }
                    finally {
                        (_eiSrc as IDisposable)?.Dispose();
                    }
                    if (!result.Value.GetBool()) break;

                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        var condExpr = Enumerable.First(exprs);
                        BoxedValue condRes;
                        if (condExpr.IsAsync) {
                            var _eiCalc = condExpr.Calc(result);
                            try {
                                while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
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
                            result.Value = src.Current;
                            yield break;
                        }
                    }
                    else {
                        result.Value = src.Current;
                        yield break;
                    }
                }
                result.Value = BoxedValue.NullObject;
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }
    }

    // LAST Operator (Full stream consumer status machine)
    internal sealed class OpLastOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            BoxedValue last = BoxedValue.NullObject;
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (src.MoveNext()) {
                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        if (Enumerable.First(exprs).Calc().GetLong() != 0) {
                            last = src.Current;
                        }
                    }
                    else {
                        last = src.Current;
                    }
                }
                return last;
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            BoxedValue last = BoxedValue.NullObject;
            BoxedValue prev = calcContext.GetVariable("$$");
            try {
                while (true) {
                    var _eiSrc = src.MoveNext(result);
                    try {
                        while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                    }
                    finally {
                        (_eiSrc as IDisposable)?.Dispose();
                    }
                    if (!result.Value.GetBool()) break;

                    if (exprs.Count > 0) {
                        calcContext.SetVariable("$$", src.Current);
                        var condExpr = Enumerable.First(exprs);
                        BoxedValue condRes;
                        if (condExpr.IsAsync) {
                            var _eiCalc = condExpr.Calc(result);
                            try {
                                while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
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
                            last = src.Current;
                        }
                    }
                    else {
                        last = src.Current;
                    }
                }
                result.Value = last;
            }
            finally {
                calcContext.SetVariable("$$", prev);
            }
        }
    }

    // TOLIST Operator (Collects all elements into List<BoxedValue>)
    internal sealed class OpToListOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => true;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return null; }

        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            var list = new List<BoxedValue>();
            while (src.MoveNext()) {
                list.Add(src.Current);
            }
            return BoxedValue.FromObject(list);
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            var list = new List<BoxedValue>();
            while (true) {
                var _eiSrc = src.MoveNext(result);
                try {
                    while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                }
                finally {
                    (_eiSrc as IDisposable)?.Dispose();
                }
                if (!result.Value.GetBool()) break;
                list.Add(src.Current);
            }
            result.Value = BoxedValue.FromObject(list);
        }
    }
}
