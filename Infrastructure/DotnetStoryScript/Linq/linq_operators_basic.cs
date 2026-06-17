using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterBasic()
        {
            var opWhere = new OpWhereOperator();
            Register("where", opWhere);
            Register("filter", opWhere);

            var opSelect = new OpSelectOperator();
            Register("select", opSelect);
            Register("map", opSelect);
        }
    }

    // ============================================================================
    // 1. WHERE / FILTER OPERATOR FACTORY & ITERATOR
    // ============================================================================
    internal sealed class OpWhereOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrWhere(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrWhere : LinqIterator
    {
        private LinqIterator _src;
        private List<IExpression> _exprs;
        private DslCalculator _calc;
        private BoxedValue _current = BoxedValue.NullObject;

        public ItrWhere(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            _src = src;
            _exprs = exprs;
            _calc = calcContext;
        }

        public override BoxedValue Current => _current;

        public override bool MoveNext()
        {
            while (_src.MoveNext()) {
                var item = _src.Current;
                BoxedValue prev = _calc.GetVariable("$$");
                _calc.SetVariable("$$", item);
                BoxedValue r = BoxedValue.NullObject;
                for (int idx = 0; idx < _exprs.Count; ++idx) {
                    r = _exprs[idx].Calc();
                }
                _calc.SetVariable("$$", prev);
                if (r.GetLong() != 0) {
                    _current = item;
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            while (true) {
                var _eiSrc = _src.MoveNext(result);
                while (_eiSrc.MoveNext()) {
                    yield return _eiSrc.Current;
                }

                if (!result.Value.GetBool()) {
                    result.Value = BoxedValue.FromBool(false);
                    yield break;
                }

                var item = _src.Current;
                BoxedValue prev = _calc.GetVariable("$$");
                _calc.SetVariable("$$", item);
                BoxedValue condVal = BoxedValue.NullObject;

                for (int idx = 0; idx < _exprs.Count; ++idx) {
                    var exp = _exprs[idx];
                    if (exp.IsAsync) {
                        var _eiCalc = exp.Calc(result);
                        while (_eiCalc.MoveNext()) {
                            yield return _eiCalc.Current;
                        }
                        condVal = result.Value;
                    }
                    else {
                        condVal = exp.Calc();
                    }
                }
                _calc.SetVariable("$$", prev);

                if (condVal.GetLong() != 0) {
                    _current = item;
                    result.Value = BoxedValue.FromBool(true);
                    yield break;
                }
            }
        }
    }

    // ============================================================================
    // 2. SELECT / MAP OPERATOR FACTORY & ITERATOR
    // ============================================================================
    internal sealed class OpSelectOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrSelect(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrSelect : LinqIterator
    {
        private LinqIterator _src;
        private List<IExpression> _exprs;
        private DslCalculator _calc;
        private BoxedValue _current = BoxedValue.NullObject;

        public ItrSelect(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            _src = src;
            _exprs = exprs;
            _calc = calcContext;
        }

        public override BoxedValue Current => _current;

        public override bool MoveNext()
        {
            if (_src.MoveNext()) {
                BoxedValue prev = _calc.GetVariable("$$");
                _calc.SetVariable("$$", _src.Current);
                BoxedValue r = BoxedValue.NullObject;
                for (int idx = 0; idx < _exprs.Count; ++idx) {
                    r = _exprs[idx].Calc();
                }
                _calc.SetVariable("$$", prev);
                _current = r;
                return true;
            }
            return false;
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            var _eiSrc = _src.MoveNext(result);
            while (_eiSrc.MoveNext()) {
                yield return _eiSrc.Current;
            }

            if (!result.Value.GetBool()) {
                result.Value = BoxedValue.FromBool(false);
                yield break;
            }

            BoxedValue prevSel = _calc.GetVariable("$$");
            _calc.SetVariable("$$", _src.Current);
            BoxedValue mappedVal = BoxedValue.NullObject;

            for (int idx = 0; idx < _exprs.Count; ++idx) {
                var exp = _exprs[idx];
                if (exp.IsAsync) {
                    var _eiCalc = exp.Calc(result);
                    while (_eiCalc.MoveNext()) {
                        yield return _eiCalc.Current;
                    }
                    mappedVal = result.Value;
                }
                else {
                    mappedVal = exp.Calc();
                }
            }
            _calc.SetVariable("$$", prevSel);

            _current = mappedVal;
            result.Value = BoxedValue.FromBool(true);
            yield break;
        }
    }
}
