using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterSlicePart1()
        {
            var opTake = new OpTakeOperator();
            Register("top", opTake);
            Register("take", opTake);

            Register("skip", new OpSkipOperator());
            Register("distinct", new OpDistinctOperator());
        }
    }

    // ============================================================================
    // 1. TAKE / TOP OPERATOR (Dual-State Status Machine)
    // ============================================================================
    internal sealed class OpTakeOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrTake(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrTake : LinqIterator
    {
        private LinqIterator _src;
        private List<IExpression> _exprs;
        private long _ct = -1;
        private bool _init = false;

        public ItrTake(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { _src = src; _exprs = exprs; }
        public override BoxedValue Current => _src.Current;

        private void InitCount()
        {
            if (!_init) {
                _ct = _exprs.Count > 0 ? Enumerable.First(_exprs).Calc().GetLong() : 0;
                _init = true;
            }
        }

        public override bool MoveNext()
        {
            InitCount();
            if (_ct-- > 0) {
                return _src.MoveNext();
            }
            return false;
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            if (!_init) {
                if (_exprs.Count > 0) {
                    var firstExpr = Enumerable.First(_exprs);
                    if (firstExpr.IsAsync) {
                        var _ei = firstExpr.Calc(result);
                        while (_ei.MoveNext()) {
                            yield return _ei.Current;
                        }
                        _ct = result.Value.GetLong();
                    }
                    else {
                        _ct = firstExpr.Calc().GetLong();
                    }
                }
                else {
                    _ct = 0;
                }
                _init = true;
            }

            if (_ct-- > 0) {
                var _eiSrc = _src.MoveNext(result);
                while (_eiSrc.MoveNext()) {
                    yield return _eiSrc.Current;
                }
                yield break;
            }
            result.Value = BoxedValue.FromBool(false);
        }
    }

    // ============================================================================
    // 2. SKIP OPERATOR (Dual-State Status Machine)
    // ============================================================================
    internal sealed class OpSkipOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrSkip(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrSkip : LinqIterator
    {
        private LinqIterator _src;
        private List<IExpression> _exprs;
        private long _ct = -1;
        private bool _init = false;

        public ItrSkip(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { _src = src; _exprs = exprs; }
        public override BoxedValue Current => _src.Current;

        private void InitCount()
        {
            if (!_init) {
                _ct = _exprs.Count > 0 ? Enumerable.First(_exprs).Calc().GetLong() : 0;
                _init = true;
            }
        }

        public override bool MoveNext()
        {
            InitCount();
            while (_ct > 0) {
                _ct--;
                if (!_src.MoveNext()) {
                    return false;
                }
            }
            return _src.MoveNext();
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            if (!_init) {
                if (_exprs.Count > 0) {
                    var firstExpr = Enumerable.First(_exprs);
                    if (firstExpr.IsAsync) {
                        var _ei = firstExpr.Calc(result);
                        while (_ei.MoveNext()) {
                            yield return _ei.Current;
                        }
                        _ct = result.Value.GetLong();
                    }
                    else {
                        _ct = firstExpr.Calc().GetLong();
                    }
                }
                else {
                    _ct = 0;
                }
                _init = true;
            }

            while (_ct > 0) {
                _ct--;
                var _eiSrc = _src.MoveNext(result);
                while (_eiSrc.MoveNext()) {
                    yield return _eiSrc.Current;
                }
                if (!result.Value.GetBool()) {
                    yield break;
                }
            }

            var _eiFinal = _src.MoveNext(result);
            while (_eiFinal.MoveNext()) {
                yield return _eiFinal.Current;
            }
        }
    }

    // ============================================================================
    // 3. DISTINCT OPERATOR (Dual-State Status Machine)
    // ============================================================================
    internal sealed class OpDistinctOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrDistinct(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrDistinct : LinqIterator
    {
        private LinqIterator _src;
        private List<IExpression> _exprs;
        private DslCalculator _calc;
        private HashSet<BoxedValue> _seen = new HashSet<BoxedValue>();

        public ItrDistinct(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { _src = src; _exprs = exprs; _calc = calcContext; }
        public override BoxedValue Current => _src.Current;

        public override bool MoveNext()
        {
            while (_src.MoveNext()) {
                BoxedValue key = _src.Current;
                if (_exprs.Count > 0) {
                    BoxedValue prev = _calc.GetVariable("$$");
                    _calc.SetVariable("$$", _src.Current);
                    key = Enumerable.First(_exprs).Calc();
                    _calc.SetVariable("$$", prev);
                }
                if (_seen.Add(key)) {
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
                    yield break;
                }

                BoxedValue key = _src.Current;
                if (_exprs.Count > 0) {
                    BoxedValue prev = _calc.GetVariable("$$");
                    _calc.SetVariable("$$", _src.Current);
                    var firstExpr = Enumerable.First(_exprs);
                    if (firstExpr.IsAsync) {
                        var _eiCalc = firstExpr.Calc(result);
                        while (_eiCalc.MoveNext()) {
                            yield return _eiCalc.Current;
                        }
                        key = result.Value;
                    }
                    else {
                        key = firstExpr.Calc();
                    }
                    _calc.SetVariable("$$", prev);
                }

                if (_seen.Add(key)) {
                    result.Value = BoxedValue.FromBool(true);
                    yield break;
                }
            }
        }
    }
}
