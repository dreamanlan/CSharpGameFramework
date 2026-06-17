using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    public partial class LinqOperatorRegistry
    {
        public void RegisterSlicePart2()
        {
            Register("groupby", new OpGroupByOperator());
            Register("concat", new OpConcatOperator());
            Register("orderby", new OpOrderByOperator(false));
            Register("orderbydesc", new OpOrderByOperator(true));
        }
    }

    internal sealed class OpConcatOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return new ItrConcat(src, exprs, calcContext); }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrConcat : LinqIterator
    {
        private LinqIterator _src; private List<IExpression> _exprs; private LinqIterator _secondSrc = null; private bool _onFirst = true;
        public ItrConcat(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { _src = src; _exprs = exprs; }
        public override BoxedValue Current => _onFirst ? _src.Current : _secondSrc.Current;
        private void InitSecond(BoxedValue val)
        {
            if (_secondSrc == null) {
                object raw = val.GetObject();
                if (raw is LinqStreamWrapper w) _secondSrc = w.Iterator;
                else if (raw is IEnumerable e) _secondSrc = new SyncEnumerableAdapter(e.GetEnumerator());
            }
        }
        public override bool MoveNext()
        {
            if (_onFirst) {
                if (_src.MoveNext()) return true;
                _onFirst = false;
                InitSecond(_exprs.Count > 0 ? Enumerable.First(_exprs).Calc() : BoxedValue.NullObject);
            }
            return _secondSrc != null && _secondSrc.MoveNext();
        }
        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            if (_onFirst) {
                var _eiSrc = _src.MoveNext(result);
                while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                if (result.Value.GetBool()) yield break;
                _onFirst = false;
                if (_exprs.Count > 0) {
                    var firstExpr = Enumerable.First(_exprs);
                    if (firstExpr.IsAsync) {
                        var _eiCalc = firstExpr.Calc(result);
                        while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
                        InitSecond(result.Value);
                    }
                    else InitSecond(firstExpr.Calc());
                }
            }
            if (_secondSrc != null) {
                var _eiSec = _secondSrc.MoveNext(result);
                while (_eiSec.MoveNext()) yield return _eiSec.Current;
                yield break;
            }
            result.Value = BoxedValue.FromBool(false);
        }
    }

    internal sealed class BufferedListIterator : LinqIterator
    {
        private List<BoxedValue> _list; private int _idx = -1;
        public BufferedListIterator(List<BoxedValue> list) { _list = list; }
        public override BoxedValue Current => _list[_idx];
        public override bool MoveNext() { return ++_idx < _list.Count; }
        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            _idx++;
            result.Value = BoxedValue.FromBool(_idx < _list.Count);
            yield break;
        }
    }

    internal sealed class OpOrderByOperator : ILinqAsyncOperator
    {
        private bool _desc; public OpOrderByOperator(bool desc) { _desc = desc; }
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            return new ItrOrderBy(src, exprs, calcContext, _desc);
        }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrOrderBy : LinqIterator
    {
        private struct KeyedItem
        {
            public BoxedValue Item;
            public BoxedValue[] Keys;
        }

        private LinqIterator _src; private List<IExpression> _exprs; private DslCalculator _calc; private bool _desc;
        private List<BoxedValue> _list; private int _idx = -1; private bool _initialized = false;
        public ItrOrderBy(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext, bool desc) { _src = src; _exprs = exprs; _calc = calcContext; _desc = desc; }
        public override BoxedValue Current => _list[_idx];

        private int CompareKeys(BoxedValue[] a, BoxedValue[] b)
        {
            for (int i = 0; i < a.Length; i++) {
                BoxedValue r1 = a[i]; BoxedValue r2 = b[i];
                int cmp = r1.IsString || r2.IsString ? string.Compare(r1.IsString ? r1.GetString() : r1.ToString(), r2.IsString ? r2.GetString() : r2.ToString(), StringComparison.Ordinal) : r1.GetDouble().CompareTo(r2.GetDouble());
                if (cmp != 0) return _desc ? -cmp : cmp;
            }
            return 0;
        }

        private void FinalizeSort(List<KeyedItem> keyed)
        {
            keyed.Sort((a, b) => CompareKeys(a.Keys, b.Keys));
            _list = new List<BoxedValue>(keyed.Count);
            for (int i = 0; i < keyed.Count; i++) _list.Add(keyed[i].Item);
        }

        public override bool MoveNext()
        {
            if (!_initialized) {
                var keyed = new List<KeyedItem>();
                BoxedValue prev = _calc.GetVariable("$$");
                while (_src.MoveNext()) {
                    BoxedValue item = _src.Current;
                    var keys = new BoxedValue[_exprs.Count];
                    _calc.SetVariable("$$", item);
                    for (int i = 0; i < _exprs.Count; i++) {
                        keys[i] = _exprs[i].Calc();
                    }
                    keyed.Add(new KeyedItem { Item = item, Keys = keys });
                }
                _calc.SetVariable("$$", prev);
                FinalizeSort(keyed);
                _initialized = true;
            }
            return ++_idx < _list.Count;
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            if (!_initialized) {
                var keyed = new List<KeyedItem>();
                BoxedValue prev = _calc.GetVariable("$$");
                while (true) {
                    var _eiSrc = _src.MoveNext(result);
                    while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                    if (!result.Value.GetBool()) break;
                    BoxedValue item = _src.Current;
                    var keys = new BoxedValue[_exprs.Count];
                    _calc.SetVariable("$$", item);
                    for (int i = 0; i < _exprs.Count; i++) {
                        var exp = _exprs[i];
                        if (exp.IsAsync) {
                            var _eiCalc = exp.Calc(result);
                            while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
                            keys[i] = result.Value;
                        }
                        else {
                            keys[i] = exp.Calc();
                        }
                    }
                    keyed.Add(new KeyedItem { Item = item, Keys = keys });
                }
                _calc.SetVariable("$$", prev);
                FinalizeSort(keyed);
                _initialized = true;
            }
            _idx++;
            result.Value = BoxedValue.FromBool(_idx < _list.Count);
            yield break;
        }
    }

    internal sealed class GroupingObject
    {
        public BoxedValue key { get; set; }
        public LinqStreamWrapper values { get; set; }
    }

    internal sealed class OpGroupByOperator : ILinqAsyncOperator
    {
        public bool IsTerminal => false;
        public LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext)
        {
            return new ItrGroupBy(src, exprs, calcContext);
        }
        public BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { return BoxedValue.NullObject; }
        public IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult r, DslCalculator calcContext) { yield break; }
    }

    internal sealed class ItrGroupBy : LinqIterator
    {
        private LinqIterator _src; private List<IExpression> _exprs; private DslCalculator _calc;
        private List<BoxedValue> _results; private int _idx = -1; private bool _initialized = false;
        public ItrGroupBy(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext) { _src = src; _exprs = exprs; _calc = calcContext; }
        public override BoxedValue Current => _results[_idx];

        private void BuildResults(Dictionary<BoxedValue, List<BoxedValue>> dict)
        {
            _results = new List<BoxedValue>();
            foreach (var kp in dict) {
                var innerIterator = new BufferedListIterator(kp.Value);
                _results.Add(BoxedValue.FromObject(new GroupingObject { key = kp.Key, values = new LinqStreamWrapper(innerIterator) }));
            }
        }

        public override bool MoveNext()
        {
            if (!_initialized) {
                if (_exprs.Count == 0) { _results = new List<BoxedValue>(); }
                else {
                    var firstExpr = Enumerable.First(_exprs);
                    var dict = new Dictionary<BoxedValue, List<BoxedValue>>();
                    BoxedValue prev = _calc.GetVariable("$$");
                    while (_src.MoveNext()) {
                        _calc.SetVariable("$$", _src.Current); BoxedValue k = firstExpr.Calc();
                        if (!dict.TryGetValue(k, out var l)) dict[k] = l = new List<BoxedValue>();
                        l.Add(_src.Current);
                    }
                    _calc.SetVariable("$$", prev);
                    BuildResults(dict);
                }
                _initialized = true;
            }
            return ++_idx < _results.Count;
        }

        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            if (!_initialized) {
                if (_exprs.Count == 0) { _results = new List<BoxedValue>(); }
                else {
                    var firstExpr = Enumerable.First(_exprs);
                    var dict = new Dictionary<BoxedValue, List<BoxedValue>>();
                    BoxedValue prev = _calc.GetVariable("$$");
                    while (true) {
                        var _eiSrc = _src.MoveNext(result);
                        while (_eiSrc.MoveNext()) yield return _eiSrc.Current;
                        if (!result.Value.GetBool()) break;
                        _calc.SetVariable("$$", _src.Current);
                        BoxedValue k;
                        if (firstExpr.IsAsync) {
                            var _eiCalc = firstExpr.Calc(result);
                            while (_eiCalc.MoveNext()) yield return _eiCalc.Current;
                            k = result.Value;
                        }
                        else {
                            k = firstExpr.Calc();
                        }
                        if (!dict.TryGetValue(k, out var l)) dict[k] = l = new List<BoxedValue>();
                        l.Add(_src.Current);
                    }
                    _calc.SetVariable("$$", prev);
                    BuildResults(dict);
                }
                _initialized = true;
            }
            _idx++;
            result.Value = BoxedValue.FromBool(_idx < _results.Count);
            yield break;
        }
    }
}
