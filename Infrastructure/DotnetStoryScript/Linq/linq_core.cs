using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.DslExpression
{
    // High-performance stream pulling contract synced with DslCalculator states
    public abstract class LinqIterator
    {
        public abstract bool MoveNext();
        public abstract IEnumerator MoveNext(AsyncCalcResult result);
        public abstract BoxedValue Current { get; }
    }

    public sealed class LinqStreamWrapper
    {
        public LinqIterator Iterator { get; private set; }
        public LinqStreamWrapper(LinqIterator iterator) { Iterator = iterator; }
    }

    // Explicitly typed operator interface natively wired with DslCalculator reference
    public interface ILinqAsyncOperator
    {
        bool IsTerminal { get; }
        LinqIterator CreateIterator(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext);
        BoxedValue ExecuteSyncTerminal(LinqIterator src, List<IExpression> exprs, DslCalculator calcContext);
        IEnumerator ExecuteAsyncTerminal(LinqIterator src, List<IExpression> exprs, AsyncCalcResult result, DslCalculator calcContext);
    }

    public partial class LinqOperatorRegistry
    {
        public bool IsLinqMethod(string n)
        {
            return m_AsyncOps.ContainsKey(n);
        }

        public bool IsTerminalOperator(string n)
        {
            return m_TerminalOps.Contains(n);
        }

        public ILinqAsyncOperator GetOperator(string n)
        {
            return m_AsyncOps.TryGetValue(n, out var op) ? op : null;
        }

        public void Register(string name, ILinqAsyncOperator op)
        {
            m_AsyncOps[name] = op;
            if (op.IsTerminal) {
                m_TerminalOps.Add(name);
            }
        }
        public void RegisterPredefined()
        {
            RegisterBasic();
            RegisterSlicePart1();
            RegisterSlicePart2();
            RegisterAggregate();
            RegisterQuantifier();
            RegisterExtract();
            RegisterCount();
            RegisterMath();
        }

        private readonly Dictionary<string, ILinqAsyncOperator> m_AsyncOps = new Dictionary<string, ILinqAsyncOperator>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> m_TerminalOps = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    // Main expression unified router injecting 'this.Calculator' directly
    internal sealed class LinqExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }

        // ============================================================================
        // 1. SYNCHRONOUS FLOW ROUTE
        // ============================================================================
        protected override BoxedValue DoCalc()
        {
            var raw = m_List.Calc().GetObject();
            var method = m_Method.Calc().GetString();
            if (string.IsNullOrEmpty(method)) {
                return BoxedValue.NullObject;
            }

            LinqIterator src = GetSourceIterator(raw);
            if (null == src) {
                return BoxedValue.NullObject;
            }

            var op = Calculator.ApiRegistry.LinqOperatorRegistry.GetOperator(method);
            if (null == op) {
                return BoxedValue.NullObject;
            }

            if (Calculator.ApiRegistry.LinqOperatorRegistry.IsTerminalOperator(method)) {
                // Inject the native DslCalculator reference directly
                return op.ExecuteSyncTerminal(src, m_Expressions, this.Calculator);
            }

            return BoxedValue.FromObject(new LinqStreamWrapper(op.CreateIterator(src, m_Expressions, this.Calculator)));
        }

        // ============================================================================
        // 2. ASYNCHRONOUS COROUTINE ROUTE
        // ============================================================================
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue rawVal;
            if (m_List.IsAsync) {
                var _ei = m_List.Calc(result);
                while (_ei.MoveNext()) {
                    yield return _ei.Current;
                }
                rawVal = result.Value;
            }
            else {
                rawVal = m_List.Calc();
            }

            var method = m_Method.Calc().GetString();
            if (string.IsNullOrEmpty(method)) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            LinqIterator src = GetSourceIterator(rawVal.GetObject());
            if (null == src) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            var op = Calculator.ApiRegistry.LinqOperatorRegistry.GetOperator(method);
            if (null == op) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            if (Calculator.ApiRegistry.LinqOperatorRegistry.IsTerminalOperator(method)) {
                var _eiTerminal = op.ExecuteAsyncTerminal(src, m_Expressions, result, this.Calculator);
                while (_eiTerminal.MoveNext()) {
                    yield return _eiTerminal.Current;
                }
                yield break;
            }

            result.Value = BoxedValue.FromObject(new LinqStreamWrapper(op.CreateIterator(src, m_Expressions, this.Calculator)));
        }

        private LinqIterator GetSourceIterator(object raw)
        {
            if (raw is LinqStreamWrapper wrapper) {
                return wrapper.Iterator;
            }
            if (raw is IEnumerable e) {
                return new SyncEnumerableAdapter(e.GetEnumerator());
            }
            return null;
        }

        protected override bool Load(Dsl.FunctionData c)
        {
            m_List = Calculator.Load(c.GetParam(0));
            m_Method = Calculator.Load(c.GetParam(1));
            for (int i = 2; i < c.GetParamNum(); ++i) {
                m_Expressions.Add(Calculator.Load(c.GetParam(i)));
            }

            m_IsAsync = m_List.IsAsync;
            if (!m_IsAsync) {
                foreach (var exp in m_Expressions) {
                    if (exp.IsAsync) {
                        m_IsAsync = true;
                        break;
                    }
                }
            }
            return true;
        }

        private IExpression m_List;
        private IExpression m_Method;
        private List<IExpression> m_Expressions = new List<IExpression>();
        private bool m_IsAsync = false;
    }

    internal sealed class SyncEnumerableAdapter : LinqIterator
    {
        private IEnumerator _enumerator;
        public SyncEnumerableAdapter(IEnumerator enumerator) { _enumerator = enumerator; }
        public override BoxedValue Current => BoxedValue.FromObject(_enumerator.Current);
        public override bool MoveNext() { return _enumerator.MoveNext(); }
        public override IEnumerator MoveNext(AsyncCalcResult result)
        {
            result.Value = BoxedValue.FromBool(_enumerator.MoveNext());
            yield break;
        }
    }
}
