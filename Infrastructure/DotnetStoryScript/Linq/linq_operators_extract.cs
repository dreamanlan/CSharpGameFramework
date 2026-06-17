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
            while (src.MoveNext()) {
                if (exprs.Count > 0) {
                    calcContext.SetVariable("$$", src.Current);
                    if (Enumerable.First(exprs).Calc().GetLong() != 0) {
                        calcContext.SetVariable("$$", prev);
                        return src.Current;
                    }
                }
                else {
                    calcContext.SetVariable("$$", prev);
                    return src.Current;
                }
            }
            calcContext.SetVariable("$$", prev);
            return BoxedValue.NullObject;
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            BoxedValue prev = calcContext.GetVariable("$$");
            while (true) {
                var _eiSrc = src.MoveNext(result);
                while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                if (!result.Value.GetBool()) break;

                if (exprs.Count > 0) {
                    calcContext.SetVariable("$$", src.Current);
                    var condExpr = Enumerable.First(exprs);
                    BoxedValue condRes;
                    if (condExpr.IsAsync) {
                        var _eiCalc = condExpr.Calc(result);
                        while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
                        condRes = result.Value;
                    }
                    else {
                        condRes = condExpr.Calc();
                    }
                    if (condRes.GetLong() != 0) {
                        calcContext.SetVariable("$$", prev);
                        result.Value = src.Current;
                        yield break;
                    }
                }
                else {
                    calcContext.SetVariable("$$", prev);
                    result.Value = src.Current;
                    yield break;
                }
            }
            calcContext.SetVariable("$$", prev);
            result.Value = BoxedValue.NullObject;
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
            calcContext.SetVariable("$$", prev);
            return last;
        }

        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext)
        {
            BoxedValue last = BoxedValue.NullObject;
            BoxedValue prev = calcContext.GetVariable("$$");
            while (true) {
                var _eiSrc = src.MoveNext(result);
                while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                if (!result.Value.GetBool()) break;

                if (exprs.Count > 0) {
                    calcContext.SetVariable("$$", src.Current);
                    var condExpr = Enumerable.First(exprs);
                    BoxedValue condRes;
                    if (condExpr.IsAsync) {
                        var _eiCalc = condExpr.Calc(result);
                        while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
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
            calcContext.SetVariable("$$", prev);
            result.Value = last;
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
                while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                if (!result.Value.GetBool()) break;
                list.Add(src.Current);
            }
            result.Value = BoxedValue.FromObject(list);
        }
    }
}
