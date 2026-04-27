using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using ScriptableFramework;
using DotnetStoryScript;

#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace DotnetStoryScript
{
    public interface IObjectDispatch
    {
        int GetDispatchId(string name);
        BoxedValue GetProperty(int dispId);
        void SetProperty(int dispId, BoxedValue val);
        BoxedValue InvokeMethod(int dispId, List<BoxedValue> args);
    }
}
namespace DotnetStoryScript.DslExpression
{
    using TupleValue1 = Tuple<BoxedValue>;
    using TupleValue2 = Tuple<BoxedValue, BoxedValue>;
    using TupleValue3 = Tuple<BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue4 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue5 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue6 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue7 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue8 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, Tuple<BoxedValue>>;

    public class BoxedValueListPool
    {
        public List<BoxedValue> Alloc()
        {
            if (m_Pool.Count > 0)
                return m_Pool.Dequeue();
            else
                return new List<BoxedValue>();
        }
        public void Recycle(List<BoxedValue> list)
        {
            if (null != list) {
                m_Pool.Enqueue(list);
            }
        }
        public void Clear()
        {
            m_Pool.Clear();
        }
        public BoxedValueListPool(int initCapacity)
        {
            m_Pool = new Queue<List<BoxedValue>>(initCapacity);
        }

        private Queue<List<BoxedValue>> m_Pool = null;
    }
    public sealed class AsyncCalcResult
    {
        public BoxedValue Value;
        public bool IsCompleted;
        public bool IsBreak;
    }
    public sealed class AsyncTaskRuntimeContext
    {
        public object Stack = null;
        public RunStateEnum RunState = RunStateEnum.Normal;
    }

    public interface IExpression
    {
        bool IsAsync { get; }
        IEnumerator Calc(AsyncCalcResult result);
        BoxedValue Calc();
        bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator);
    }
    public interface IExpressionFactory
    {
        IExpression Create();
    }
    public sealed class ExpressionFactoryHelper<T> : IExpressionFactory where T : IExpression, new()
    {
        public IExpression Create()
        {
            return new T();
        }
    }
    public abstract class AbstractExpression : IExpression
    {
        public virtual bool IsAsync { get { return false; } }
        public IEnumerator Calc(AsyncCalcResult result)
        {
            if (IsAsync) {
                var _ei = DoCalc(result);
                while (_ei.MoveNext()) { yield return _ei.Current; }
            }
            else {
                BoxedValue ret = BoxedValue.NullObject;
                try {
                    ret = DoCalc();
                }
                catch (Exception ex) {
                    var msg = string.Format("calc:[{0}]", ToString());
                    throw new Exception(msg, ex);
                }
                result.Value = ret;
            }
            result.IsCompleted = true;
        }
        public BoxedValue Calc()
        {
            Calculator.SyncCalculationPush();
            try {
                if (IsAsync) {
                    AsyncCalcResult result = new AsyncCalcResult();
                    var enumer = DoCalc(result);
                    DrainEnumerator(enumer);
                    return result.Value;
                }
                else {
                    BoxedValue ret = BoxedValue.NullObject;
                    try {
                        ret = DoCalc();
                    }
                    catch (Exception ex) {
                        var msg = string.Format("calc:[{0}]", ToString());
                        throw new Exception(msg, ex);
                    }
                    return ret;
                }
            }
            finally {
                Calculator.SyncCalculationPop();
            }
        }
        public bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator)
        {
            m_Calculator = calculator;
            m_Dsl = dsl;
            Dsl.ValueData valueData = dsl as Dsl.ValueData;
            if (null != valueData) {
                return Load(valueData);
            }
            else {
                Dsl.FunctionData funcData = dsl as Dsl.FunctionData;
                if (null != funcData) {
                    if (funcData.HaveParam()) {
                        var callData = funcData;
                        bool ret = Load(callData);
                        if (!ret) {
                            int num = callData.GetParamNum();
                            List<IExpression> args = new List<IExpression>();
                            for (int ix = 0; ix < num; ++ix) {
                                Dsl.ISyntaxComponent param = callData.GetParam(ix);
                                args.Add(calculator.Load(param));
                            }
                            return Load(args);
                        }
                        return ret;
                    }
                    else {
                        return Load(funcData);
                    }
                }
                else {
                    Dsl.StatementData statementData = dsl as Dsl.StatementData;
                    if (null != statementData) {
                        return Load(statementData);
                    }
                }
            }
            return false;
        }
        public override string ToString()
        {
            return string.Format("{0} line:{1}", base.ToString(), m_Dsl.GetLine());
        }
        protected virtual bool Load(Dsl.ValueData valData) { return false; }
        protected virtual bool Load(IList<IExpression> exps) { return false; }
        protected virtual bool Load(Dsl.FunctionData funcData) { return false; }
        protected virtual bool Load(Dsl.StatementData statementData) { return false; }
        protected virtual BoxedValue DoCalc() { return BoxedValue.NullObject; }
        protected virtual IEnumerator DoCalc(AsyncCalcResult result) { result.Value = BoxedValue.NullObject; yield break; }
        private static void DrainEnumerator(IEnumerator enumer)
        {
            while (enumer.MoveNext()) {
                var cur = enumer.Current;
                if (cur is IEnumerator nested) {
                    DrainEnumerator(nested);
                }
            }
        }

        protected DslCalculator Calculator
        {
            get { return m_Calculator; }
        }
        protected internal Dsl.ISyntaxComponent SyntaxComponent
        {
            get { return m_Dsl; }
        }

        private DslCalculator m_Calculator = null;
        private Dsl.ISyntaxComponent m_Dsl = null;

        protected static void CastArgsForCall(Type t, string method, BindingFlags flags, params object[] args)
        {
            Converter.CastArgsForCall(t, method, flags, args);
        }
        protected static void CastArgsForSet(Type t, string property, BindingFlags flags, params object[] args)
        {
            Converter.CastArgsForSet(t, property, flags, args);
        }
        protected static void CastArgsForGet(Type t, string property, BindingFlags flags, params object[] args)
        {
            Converter.CastArgsForGet(t, property, flags, args);
        }
        protected static T CastTo<T>(object obj)
        {
            return Converter.CastTo<T>(obj);
        }
        protected static object CastTo(Type t, object obj)
        {
            return Converter.CastTo(t, obj);
        }
        protected static Encoding GetEncoding(BoxedValue v)
        {
            var name = v.AsString;
            if (null != name) {
                return Encoding.GetEncoding(name);
            }
            else if (v.IsInteger) {
                int codePage = v.GetInt();
                return Encoding.GetEncoding(codePage);
            }
            else {
                return Encoding.UTF8;
            }
        }
        public static bool TryParseNumeric(string str, out BoxedValue val)
        {
            string type = string.Empty;
            return TryParseNumeric(str, ref type, out val);
        }
        public static bool TryParseNumeric(string str, ref string type, out BoxedValue val)
        {
            bool ret = false;
            val = BoxedValue.NullObject;
            if (str.Length > 2 && str[0] == '0' && str[1] == 'x') {
                char c = str[str.Length - 1];
                if (c == 'u' || c == 'U') {
                    str = str.Substring(0, str.Length - 1);
                }
                if (ulong.TryParse(str.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out var v)) {
                    str = v.ToString();
                }
                type = "uint";
            }
            else if (str.Length >= 2) {
                char c = str[str.Length - 1];
                if (c == 'u' || c == 'U') {
                    str = str.Substring(0, str.Length - 1);
                    type = "uint";
                }
                else if (c == 'f' || c == 'F') {
                    str = str.Substring(0, str.Length - 1);
                    c = str[str.Length - 1];
                    if (c == 'l' || c == 'L') {
                        str = str.Substring(0, str.Length - 1);
                    }
                    type = "float";
                }
            }
            if (type == "float" || type != "uint" && str.IndexOfAny(s_FloatExponent) > 0) {
                if (double.TryParse(str, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out var v)) {
                    if (v >= float.MinValue && v <= float.MaxValue) {
                        val.Set((float)v);
                        type = "float";
                    }
                    else {
                        val.Set(v);
                        type = "double";
                    }
                    ret = true;
                }
            }
            else if (str.Length > 1 && str[0] == '0') {
                ulong v = Convert.ToUInt64(str, 8);
                if (v >= uint.MinValue && v <= uint.MaxValue) {
                    val.Set((uint)v);
                    type = "uint";
                }
                else {
                    val.Set(v);
                    type = "ulong";
                }
                ret = true;
            }
            else if (long.TryParse(str, out var lv)) {
                if (type == "uint") {
                    ulong v = (ulong)lv;
                    if (v >= uint.MinValue && v <= uint.MaxValue) {
                        val.Set((uint)v);
                        type = "uint";
                    }
                    else {
                        val.Set(v);
                        type = "ulong";
                    }
                }
                else {
                    if (lv >= int.MinValue && lv <= int.MaxValue) {
                        val.Set((int)lv);
                        type = "int";
                    }
                    else {
                        val.Set(lv);
                        type = "long";
                    }
                }
                ret = true;
            }
            if (!ret && TryParseBool(str, out var bv)) {
                val.Set(bv);
                type = "bool";
                ret = true;
            }
            return ret;
        }
        public static bool TryParseBool(string v, out bool val)
        {
            if (bool.TryParse(v, out val)) {
                return true;
            }
            else if (int.TryParse(v, out var ival)) {
                val = ival != 0;
                return true;
            }
            else if (v == "true") {
                val = true;
                return true;
            }
            else if (v == "false") {
                val = false;
                return true;
            }
            return false;
        }

        public static char[] s_FloatExponent = new char[] { 'e', 'E', '.' };
    }
    public abstract class SimpleExpressionBase : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var operands = Calculator.NewCalculatorValueList();
            for (int i = 0; i < m_Exps.Count; ++i) {
                var v = m_Exps[i].Calc();
                operands.Add(v);
            }
            var r = OnCalc(operands);
            Calculator.RecycleCalculatorValueList(operands);
            return r;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Exps = exps;
            return true;
        }
        protected abstract BoxedValue OnCalc(IList<BoxedValue> operands);

        private IList<IExpression> m_Exps = null;
    }
    public abstract class SimpleAsyncExpressionBase : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            var operands = Calculator.NewCalculatorValueList();
            for (int i = 0; i < m_Exps.Count; ++i) {
                var _ei1 = m_Exps[i].Calc(result);
                while (_ei1.MoveNext()) { yield return _ei1.Current; }
                operands.Add(result.Value);
            }
            var _ei2 = OnCalc(operands, result);
            while (_ei2.MoveNext()) { yield return _ei2.Current; }
            Calculator.RecycleCalculatorValueList(operands);
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Exps = exps;
            return true;
        }
        protected abstract IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result);

        private IList<IExpression> m_Exps = null;
    }
    internal sealed class ArgsGet : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue ret = BoxedValue.FromObject(Calculator.Arguments);
            return ret;
        }
    }
    internal sealed class ArgGet : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            var ix = operands[0].GetInt();
            var args = Calculator.Arguments;
            if (ix >= 0 && ix < args.Count) {
                ret = args[ix];
            }
            return ret;
        }
    }
    internal sealed class ArgNumGet : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = Calculator.Arguments.Count;
            return BoxedValue.From(ret);
        }
    }
    internal sealed class GlobalVarSet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = m_Op.Calc();
            if (m_VarIx < int.MaxValue) {
                Calculator.SetGlobalVaraibleByIndex(m_VarIx, v);
            }
            else if (m_VarId.Length > 0) {
                m_VarIx = Calculator.AllocGlobalVariableIndex(m_VarId);
                if (m_VarIx < int.MaxValue) {
                    Calculator.SetGlobalVaraibleByIndex(m_VarIx, v);
                }
                else {
                    Calculator.SetGlobalVariable(m_VarId, v);
                }
            }
            if (m_VarId.Length > 0 && m_VarId[0] != '@') {
                Environment.SetEnvironmentVariable(m_VarId, v.ToString());
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_VarId = param1.GetId();
            m_Op = Calculator.Load(param2);
            return true;
        }

        private string m_VarId;
        private IExpression m_Op;
        private int m_VarIx = int.MaxValue;
    }
    internal sealed class GlobalVarGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_VarId == "break") {
                Calculator.RunState = RunStateEnum.Break;
                return ret;
            }
            else if (m_VarId == "continue") {
                Calculator.RunState = RunStateEnum.Continue;
                return ret;
            }
            else if (m_VarIx < int.MaxValue) {
                ret = Calculator.GetGlobalVaraibleByIndex(m_VarIx);
            }
            else if (m_VarId.Length > 0) {
                m_VarIx = Calculator.GetGlobalVariableIndex(m_VarId);
                if (m_VarIx < int.MaxValue) {
                    ret = Calculator.GetGlobalVaraibleByIndex(m_VarIx);
                }
                else if (Calculator.TryGetGlobalVariable(m_VarId, out var val)) {
                    ret = val;
                }
                else {
                    Calculator.Log("unassigned global var '{0}'", m_VarId);
                }
            }
            if (ret.IsNullObject && m_VarId.Length > 0 && m_VarId[0] != '@') {
                ret = Environment.ExpandEnvironmentVariables(m_VarId);
            }
            return ret;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_VarId = valData.GetId();
            return true;
        }

        private string m_VarId;
        private int m_VarIx = int.MaxValue;
    }
    internal sealed class LocalVarSet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = m_Op.Calc();
            if (m_VarIx < int.MaxValue) {
                Calculator.SetLocalVaraibleByIndex(m_VarIx, v);
            }
            else if (m_VarId.Length > 0) {
                m_VarIx = Calculator.AllocLocalVariableIndex(m_VarId);
                Calculator.SetLocalVaraibleByIndex(m_VarIx, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_VarId = param1.GetId();
            m_Op = Calculator.Load(param2);
            return true;
        }

        private string m_VarId;
        private IExpression m_Op;
        private int m_VarIx = int.MaxValue;
    }
    internal sealed class LocalVarGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_VarIx < int.MaxValue) {
                ret = Calculator.GetLocalVaraibleByIndex(m_VarIx);
            }
            else if (m_VarId.Length > 0) {
                m_VarIx = Calculator.GetLocalVariableIndex(m_VarId);
                if (m_VarIx < int.MaxValue) {
                    ret = Calculator.GetLocalVaraibleByIndex(m_VarIx);
                }
                else {
                    Calculator.Log("unassigned local var '{0}'", m_VarId);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_VarId = valData.GetId();
            return true;
        }

        private string m_VarId;
        private int m_VarIx = int.MaxValue;
    }
    internal sealed class ConstGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = m_Val;
            return v;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            string id = valData.GetId();
            int idType = valData.GetIdType();
            if (idType == Dsl.ValueData.NUM_TOKEN) {
                TryParseNumeric(id, out m_Val);
            }
            else {
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    if (TryParseBool(id, out var v)) {
                        m_Val = v;
                    }
                    else {
                        m_Val = id;
                    }
                }
                else {
                    m_Val = id;
                }
            }
            return true;
        }

        private BoxedValue m_Val;
    }
    internal sealed class FunctionCall : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var args = Calculator.NewCalculatorValueList();
            foreach (var arg in m_Args) {
                var o = arg.Calc();
                args.Add(o);
            }
            var r = Calculator.Calc(m_Func, args);
            Calculator.RecycleCalculatorValueList(args);
            return r;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (!funcData.IsHighOrder && funcData.HaveParam()) {
                m_Func = funcData.GetId();
                int num = funcData.GetParamNum();
                for (int ix = 0; ix < num; ++ix) {
                    Dsl.ISyntaxComponent param = funcData.GetParam(ix);
                    m_Args.Add(Calculator.Load(param));
                }
                return true;
            }
            return false;
        }
        internal string FuncName { get { return m_Func; } }
        private string m_Func = string.Empty;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    internal sealed class IncExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            if (m_OpNum == 1) {
                if (Calculator.TryGetVariable(m_VarId, out var v)) {
                    if (v.IsUnsignedInteger) {
                        v = v.GetULong() + 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsSignedInteger) {
                        v = v.GetLong() + 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsNumber) {
                        v = v.GetDouble() + 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                }
                return v;
            }
            else if (m_OpNum == 2) {
                var vv = m_Op.Calc();
                if (Calculator.TryGetVariable(m_VarId, out var v)) {
                    if (v.IsUnsignedInteger) {
                        v = v.GetULong() + vv.GetULong();
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsSignedInteger) {
                        v = v.GetLong() + vv.GetLong();
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsNumber) {
                        v = v.GetDouble() + vv.GetDouble();
                        Calculator.SetVariable(m_VarId, v);
                    }
                }
                return v;
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_OpNum = callData.GetParamNum();
            if (m_OpNum > 0) {
                Dsl.ISyntaxComponent param = callData.GetParam(0);
                m_VarId = param.GetId();
            }
            if (m_OpNum > 1) {
                Dsl.ISyntaxComponent param = callData.GetParam(1);
                m_Op = Calculator.Load(param);
            }
            return true;
        }

        private int m_OpNum;
        private string m_VarId;
        private IExpression m_Op;
    }
    internal sealed class DecExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            if (m_OpNum == 1) {
                if (Calculator.TryGetVariable(m_VarId, out var v)) {
                    if (v.IsUnsignedInteger) {
                        v = v.GetULong() - 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsSignedInteger) {
                        v = v.GetLong() - 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsNumber) {
                        v = v.GetDouble() - 1;
                        Calculator.SetVariable(m_VarId, v);
                    }
                }
                return v;
            }
            else if (m_OpNum == 2) {
                var vv = m_Op.Calc();
                if (Calculator.TryGetVariable(m_VarId, out var v)) {
                    if (v.IsUnsignedInteger) {
                        v = v.GetULong() - vv.GetULong();
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsSignedInteger) {
                        v = v.GetLong() - vv.GetLong();
                        Calculator.SetVariable(m_VarId, v);
                    }
                    else if (v.IsNumber) {
                        v = v.GetDouble() - vv.GetDouble();
                        Calculator.SetVariable(m_VarId, v);
                    }
                }
                return v;
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_OpNum = callData.GetParamNum();
            if (m_OpNum > 0) {
                Dsl.ISyntaxComponent param = callData.GetParam(0);
                m_VarId = param.GetId();
            }
            if (m_OpNum > 1) {
                Dsl.ISyntaxComponent param = callData.GetParam(1);
                m_Op = Calculator.Load(param);
            }
            return true;
        }

        private int m_OpNum;
        private string m_VarId;
        private IExpression m_Op;
    }
    internal sealed class AddExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count == 1) {
                var v1 = operands[0];
                BoxedValue v;
                if (v1.IsInteger) {
                    if (v1.IsUnsignedInteger) {
                        v = v1.GetULong();
                    }
                    else {
                        v = v1.GetLong();
                    }
                }
                else {
                    v = v1.GetDouble();
                }
                return v;
            }
            else if (operands.Count == 2) {
                var v1 = operands[0];
                var v2 = operands[1];
                BoxedValue v;
                if (v1.IsString || v2.IsString) {
                    v = v1.ToString() + v2.ToString();
                }
                else if (v1.IsInteger && v2.IsInteger) {
                    if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                        v = v1.GetULong() + v2.GetULong();
                    }
                    else {
                        v = v1.GetLong() + v2.GetLong();
                    }
                }
                else {
                    v = v1.GetDouble() + v2.GetDouble();
                }
                return v;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SubExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count == 1) {
                var v1 = operands[0];
                BoxedValue v;
                if (v1.IsInteger) {
                    if (v1.IsUnsignedInteger) {
                        v = (ulong)(0 - v1.GetULong());
                    }
                    else {
                        v = -v1.GetLong();
                    }
                }
                else {
                    v = -v1.GetDouble();
                }
                return v;
            }
            else if (operands.Count == 2) {
                var v1 = operands[0];
                var v2 = operands[1];
                BoxedValue v;
                if (v1.IsInteger && v2.IsInteger) {
                    if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                        v = v1.GetULong() - v2.GetULong();
                    }
                    else {
                        v = v1.GetLong() - v2.GetLong();
                    }
                }
                else {
                    v = v1.GetDouble() - v2.GetDouble();
                }
                return v;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MulExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsInteger && v2.IsInteger) {
                if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                    v = v1.GetULong() * v2.GetULong();
                }
                else {
                    v = v1.GetLong() * v2.GetLong();
                }
            }
            else {
                v = v1.GetDouble() * v2.GetDouble();
            }
            return v;
        }
    }
    internal sealed class DivExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsInteger && v2.IsInteger) {
                if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                    v = v1.GetULong() / v2.GetULong();
                }
                else {
                    v = v1.GetLong() / v2.GetLong();
                }
            }
            else {
                v = v1.GetDouble() / v2.GetDouble();
            }
            return v;
        }
    }
    internal sealed class ModExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsInteger && v2.IsInteger) {
                if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                    v = v1.GetULong() % v2.GetULong();
                }
                else {
                    v = v1.GetLong() % v2.GetLong();
                }
            }
            else {
                v = v1.GetDouble() % v2.GetDouble();
            }
            return v;
        }
    }
    internal sealed class BitAndExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() & v2.GetULong();
            }
            else {
                v = v1.GetLong() & v2.GetLong();
            }
            return v;
        }
    }
    internal sealed class BitOrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() | v2.GetULong();
            }
            else {
                v = v1.GetLong() | v2.GetLong();
            }
            return v;
        }
    }
    internal sealed class BitXorExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() ^ v2.GetULong();
            }
            else {
                v = v1.GetLong() ^ v2.GetLong();
            }
            return v;
        }
    }
    internal sealed class BitNotExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = ~v1.GetULong();
            }
            else {
                v = ~v1.GetLong();
            }
            return v;
        }
    }
    internal sealed class LShiftExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = v1.GetULong() << v2.GetInt();
            }
            else {
                v = v1.GetLong() << v2.GetInt();
            }
            return v;
        }
    }
    internal sealed class RShiftExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = v1.GetULong() >> v2.GetInt();
            }
            else {
                v = v1.GetLong() >> v2.GetInt();
            }
            return v;
        }
    }
    internal sealed class MaxExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var opd1 = operands[0];
            var opd2 = operands[1];
            BoxedValue v;
            if (opd1.IsInteger && opd2.IsInteger) {
                if (opd1.IsUnsignedInteger && opd2.IsUnsignedInteger) {
                    var v1 = opd1.GetULong();
                    var v2 = opd2.GetULong();
                    v = v1 >= v2 ? v1 : v2;
                }
                else {
                    var v1 = opd1.GetLong();
                    var v2 = opd2.GetLong();
                    v = v1 >= v2 ? v1 : v2;
                }
            }
            else {
                var v1 = opd1.GetDouble();
                var v2 = opd2.GetDouble();
                v = v1 >= v2 ? v1 : v2;
            }
            return v;
        }
    }
    internal sealed class MinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var opd1 = operands[0];
            var opd2 = operands[1];
            BoxedValue v;
            if (opd1.IsInteger && opd2.IsInteger) {
                if (opd1.IsUnsignedInteger && opd2.IsUnsignedInteger) {
                    var v1 = opd1.GetULong();
                    var v2 = opd2.GetULong();
                    v = v1 <= v2 ? v1 : v2;
                }
                else {
                    var v1 = opd1.GetLong();
                    var v2 = opd2.GetLong();
                    v = v1 <= v2 ? v1 : v2;
                }
            }
            else {
                var v1 = opd1.GetDouble();
                var v2 = opd2.GetDouble();
                v = v1 <= v2 ? v1 : v2;
            }
            return v;
        }
    }
    internal sealed class AbsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var opd = operands[0];
            BoxedValue v;
            if (opd.IsInteger) {
                long v1 = opd.GetLong();
                v = v1 >= 0 ? v1 : -v1;
            }
            else {
                double v1 = opd.GetDouble();
                v = v1 >= 0 ? v1 : -v1;
            }
            return v;
        }
    }
    internal sealed class SinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Sin(v1);
            return v;
        }
    }
    internal sealed class CosExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Cos(v1);
            return v;
        }
    }
    internal sealed class TanExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Tan(v1);
            return v;
        }
    }
    internal sealed class AsinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Asin(v1);
            return v;
        }
    }
    internal sealed class AcosExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Acos(v1);
            return v;
        }
    }
    internal sealed class AtanExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Atan(v1);
            return v;
        }
    }
    internal sealed class Atan2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = Math.Atan2(v1, v2);
            return v;
        }
    }
    internal sealed class SinhExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Sinh(v1);
            return v;
        }
    }
    internal sealed class CoshExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Cosh(v1);
            return v;
        }
    }
    internal sealed class TanhExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Tanh(v1);
            return v;
        }
    }
    internal sealed class RndIntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            int v2 = operands[1].GetInt();
            BoxedValue v = s_Random.Next(v1, v2);
            return v;
        }

        private static Random s_Random = new Random();
    }
    internal sealed class RndFloatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float v1 = operands[0].GetFloat();
            float v2 = operands[1].GetFloat();
            BoxedValue v = (float)(s_Random.NextDouble() * (v2 - v1) + v1);
            return v;
        }

        private static Random s_Random = new Random();
    }
    internal sealed class PowExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = Math.Pow(v1, v2);
            return v;
        }
    }
    internal sealed class SqrtExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Sqrt(v1);
            return v;
        }
    }
    internal sealed class ExpExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Exp(v1);
            return v;
        }
    }
    internal sealed class Exp2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Pow(2, v1);
            return v;
        }
    }
    internal sealed class LogExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            if (operands.Count == 1) {
                BoxedValue v = Math.Log(v1);
                return v;
            }
            else {
                double v2 = operands[1].GetDouble();
                BoxedValue v = Math.Log(v1, v2);
                return v;
            }
        }
    }
    internal sealed class Log2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Log(v1) / Math.Log(2);
            return v;
        }
    }
    internal sealed class Log10Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Log10(v1);
            return v;
        }
    }
    internal sealed class FloorExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Floor(v1);
            return v;
        }
    }
    internal sealed class CeilingExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Ceiling(v1);
            return v;
        }
    }
    internal sealed class RoundExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = Math.Round(v1);
            return v;
        }
    }
    internal sealed class FloorToIntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = (int)Math.Floor(v1);
            return v;
        }
    }
    internal sealed class CeilingToIntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = (int)Math.Ceiling(v1);
            return v;
        }
    }
    internal sealed class RoundToIntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = (int)Math.Round(v1);
            return v;
        }
    }
    internal sealed class BoolExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool v1 = operands[0].GetBool();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class SByteExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            sbyte v1 = operands[0].GetSByte();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class ByteExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            byte v1 = operands[0].GetByte();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class CharExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            char v1 = operands[0].GetChar();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class ShortExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            short v1 = operands[0].GetShort();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class UShortExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            ushort v1 = operands[0].GetUShort();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class UIntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            uint v1 = operands[0].GetUInt();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            long v1 = operands[0].GetLong();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class ULongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            ulong v1 = operands[0].GetULong();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class FloatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float v1 = operands[0].GetFloat();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class DoubleExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class DecimalExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            decimal v1 = operands[0].GetDecimal();
            BoxedValue v = v1;
            return v;
        }
    }
    internal sealed class ItofExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            float v2 = 0;
            unsafe {
                v2 = *(float*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class FtoiExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float v1 = operands[0].GetFloat();
            int v2 = 0;
            unsafe {
                v2 = *(int*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class UtofExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            uint v1 = operands[0].GetUInt();
            float v2 = 0;
            unsafe {
                v2 = *(float*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class FtouExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float v1 = operands[0].GetFloat();
            uint v2 = 0;
            unsafe {
                v2 = *(uint*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class LtodExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            long v1 = operands[0].GetLong();
            double v2 = 0;
            unsafe {
                v2 = *(double*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class DtolExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            long v2 = 0;
            unsafe {
                v2 = *(long*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class UtodExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            ulong v1 = operands[0].GetULong();
            double v2 = 0;
            unsafe {
                v2 = *(double*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class DtouExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            ulong v2 = 0;
            unsafe {
                v2 = *(ulong*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
    }
    internal sealed class LerpExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double a = operands[0].GetDouble();
            double b = operands[1].GetDouble();
            double t = operands[2].GetDouble();
            BoxedValue v;
            v = a + (b - a) * ClampExp.Clamp01(t);
            return v;
        }
    }
    internal sealed class LerpUnclampedExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double a = operands[0].GetDouble();
            double b = operands[1].GetDouble();
            double t = operands[2].GetDouble();
            BoxedValue v = a + (b - a) * t;
            return v;
        }
    }
    internal sealed class LerpAngleExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double a = operands[0].GetDouble();
            double b = operands[1].GetDouble();
            double t = operands[2].GetDouble();
            double num = Repeat(b - a, 360.0);
            if (num > 180f) {
                num -= 360f;
            }
            BoxedValue v = a + num * ClampExp.Clamp01(t);
            return v;
        }

        public static double Repeat(double t, double length)
        {
            return ClampExp.Clamp(t - Math.Floor(t / length) * length, 0f, length);
        }
    }
    internal sealed class SmoothStepExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double from = operands[0].GetDouble();
            double to = operands[1].GetDouble();
            double t = operands[2].GetDouble();
            t = ClampExp.Clamp01(t);
            t = -2.0 * t * t * t + 3.0 * t * t;
            BoxedValue v = to * t + from * (1.0 - t);
            return v;
        }
    }
    internal sealed class Clamp01Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            BoxedValue v = ClampExp.Clamp01(v1);
            return v;
        }
    }
    internal sealed class ClampExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            double v3 = operands[2].GetDouble();
            BoxedValue v;
            if (v2 <= v3) {
                if (v1 < v2)
                    v = v2;
                else if (v1 > v3)
                    v = v3;
                else
                    v = v1;
            }
            else {
                if (v1 > v2)
                    v = v2;
                else if (v1 < v3)
                    v = v3;
                else
                    v = v1;
            }
            return v;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min) {
                value = min;
            }
            else if (value > max) {
                value = max;
            }
            return value;
        }
        public static double Clamp01(double value)
        {
            if (value < 0f) {
                return 0f;
            }
            if (value > 1f) {
                return 1f;
            }
            return value;
        }
    }
    internal sealed class ApproximatelyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float v1 = operands[0].GetFloat();
            float v2 = operands[1].GetFloat();
            BoxedValue v = Approximately(v1, v2) ? 1 : 0;
            return v;
        }

        public static bool Approximately(double a, double b)
        {
            return Math.Abs(b - a) < Math.Max(1E-06 * Math.Max(Math.Abs(a), Math.Abs(b)), double.Epsilon * 8.0);
        }
    }
    internal sealed class IsPowerOfTwoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            int v = IsPowerOfTwo(v1) ? 1 : 0;
            return v;
        }

        public bool IsPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n)) == v;
        }
    }
    internal sealed class ClosestPowerOfTwoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            int v = ClosestPowerOfTwo(v1);
            return v;
        }

        public int ClosestPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n));
        }
    }
    internal sealed class NextPowerOfTwoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int v1 = operands[0].GetInt();
            int v = NextPowerOfTwo(v1);
            return v;
        }

        public int NextPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n + 1));
        }
    }
    internal sealed class DistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float x1 = (float)operands[0].GetDouble();
            float y1 = (float)operands[1].GetDouble();
            float x2 = (float)operands[2].GetDouble();
            float y2 = (float)operands[3].GetDouble();
            BoxedValue v = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            return v;
        }
    }
    internal sealed class DistSqrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            float x1 = (float)operands[0].GetDouble();
            float y1 = (float)operands[1].GetDouble();
            float x2 = (float)operands[2].GetDouble();
            float y2 = (float)operands[3].GetDouble();
            BoxedValue v = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            return v;
        }
    }
    internal sealed class GreatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = v1 > v2 ? 1 : 0;
            return v;
        }
    }
    internal sealed class GreatEqualExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = v1 >= v2 ? 1 : 0;
            return v;
        }
    }
    internal sealed class LessExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = v1 < v2 ? 1 : 0;
            return v;
        }
    }
    internal sealed class LessEqualExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            double v1 = operands[0].GetDouble();
            double v2 = operands[1].GetDouble();
            BoxedValue v = v1 <= v2 ? 1 : 0;
            return v;
        }
    }
    internal sealed class EqualExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v = v1.ToString() == v2.ToString() ? 1 : 0;
            return v;
        }
    }
    internal sealed class NotEqualExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var v1 = operands[0];
            var v2 = operands[1];
            BoxedValue v = v1.ToString() != v2.ToString() ? 1 : 0;
            return v;
        }
    }
    internal sealed class AndExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long v1 = m_Op1.Calc().GetLong();
            long v2 = 0;
            BoxedValue v = v1 != 0 && (v2 = m_Op2.Calc().GetLong()) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class OrExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long v1 = m_Op1.Calc().GetLong();
            long v2 = 0;
            BoxedValue v = v1 != 0 || (v2 = m_Op2.Calc().GetLong()) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class NotExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            long val = operands[0].GetLong();
            BoxedValue v = val == 0 ? 1 : 0;
            return v;
        }
    }
    internal sealed class CondExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = BoxedValue.NullObject;
            BoxedValue v3 = BoxedValue.NullObject;
            BoxedValue v = v1.GetLong() != 0 ? v2 = m_Op2.Calc() : v3 = m_Op3.Calc();
            return v;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            Dsl.FunctionData funcData1 = statementData.First.AsFunction;
            Dsl.FunctionData funcData2 = statementData.Second.AsFunction;
            if (funcData1.IsHighOrder && funcData1.HaveLowerOrderParam() && funcData2.GetId() == ":" && funcData2.HaveParamOrStatement()) {
                Dsl.ISyntaxComponent cond = funcData1.LowerOrderFunction.GetParam(0);
                Dsl.ISyntaxComponent op1 = funcData1.GetParam(0);
                Dsl.ISyntaxComponent op2 = funcData2.GetParam(0);
                m_Op1 = Calculator.Load(cond);
                m_Op2 = Calculator.Load(op1);
                m_Op3 = Calculator.Load(op2);
            }
            else {
                //error
                Calculator.Log("DslCalculator error, {0} line {1}", statementData.ToScriptString(false, Dsl.DelimiterInfo.Default), statementData.GetLine());
            }
            return true;
        }

        private IExpression m_Op1 = null;
        private IExpression m_Op2 = null;
        private IExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    var condVal = clause.Condition.Calc();
                    if (condVal.GetLong() != 0) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            BoxedValue tv = clause.Expressions[index].Calc();
                            if (Calculator.RunState != RunStateEnum.Normal) {
                                if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                    v = tv;
                                }
                                return v;
                            }
                            else {
                                v = tv;
                            }
                        }
                        break;
                    }
                }
                else if (ix == m_Clauses.Count - 1) {
                    for (int index = 0; index < clause.Expressions.Count; ++index) {
                        BoxedValue tv = clause.Expressions[index].Calc();
                        if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                v = tv;
                            }
                            return v;
                        }
                        else {
                            v = tv;
                        }
                    }
                    break;
                }
            }
            return v;
        }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    BoxedValue condVal;
                    if (clause.Condition.IsAsync) {
                        var _ei1 = clause.Condition.Calc(result);
                        while (_ei1.MoveNext()) { yield return _ei1.Current; }
                        condVal = result.Value;
                    }
                    else {
                        condVal = clause.Condition.Calc();
                    }
                    if (condVal.GetLong() != 0) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            var exp = clause.Expressions[index];
                            BoxedValue tv;
                            if (exp.IsAsync) {
                                var _ei2 = exp.Calc(result);
                                while (_ei2.MoveNext()) { yield return _ei2.Current; }
                                tv = result.Value;
                            }
                            else {
                                tv = exp.Calc();
                            }
                            if (Calculator.RunState != RunStateEnum.Normal) {
                                if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                    v = tv;
                                }
                                result.Value = v;
                                yield break;
                            }
                            else {
                                v = tv;
                            }
                        }
                        break;
                    }
                }
                else if (ix == m_Clauses.Count - 1) {
                    for (int index = 0; index < clause.Expressions.Count; ++index) {
                        var exp = clause.Expressions[index];
                        BoxedValue tv;
                        if (exp.IsAsync) {
                            var _ei3 = exp.Calc(result);
                            while (_ei3.MoveNext()) { yield return _ei3.Current; }
                            tv = result.Value;
                        }
                        else {
                            tv = exp.Calc();
                        }
                        if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                v = tv;
                            }
                            result.Value = v;
                            yield break;
                        }
                        else {
                            v = tv;
                        }
                    }
                    break;
                }
            }
            result.Value = v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.ISyntaxComponent cond = funcData.LowerOrderFunction.GetParam(0);
                IfExp.Clause item = new IfExp.Clause();
                item.Condition = Calculator.Load(cond);
                for (int ix = 0; ix < funcData.GetParamNum(); ++ix) {
                    IExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    item.Expressions.Add(subExp);
                }
                m_Clauses.Add(item);
            }
            else {
                //error
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false, Dsl.DelimiterInfo.Default), funcData.GetLine());
            }
            UpdateIsAsync();
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'if(exp) func(args);'.
            int funcNum = statementData.GetFunctionNum();
            if (funcNum == 2) {
                var first = statementData.First.AsFunction;
                var secondV = statementData.Second.AsValue;
                var secondF = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = statementData.Second.GetId();
                if (firstId == "if" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && (null != secondV || !secondF.HaveStatement() && !secondF.HaveExternScript())) {
                    IfExp.Clause item = new IfExp.Clause();
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false, Dsl.DelimiterInfo.Default), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(statementData.Second);
                    item.Expressions.Add(subExp);
                    m_Clauses.Add(item);
                    UpdateIsAsync();
                    return true;
                }
            }
            //the handling of the standard if syntax
            foreach (var fd in statementData.Functions) {
                var fData = fd.AsFunction;
                if (fData.GetId() == "if" || fData.GetId() == "elseif" || fData.GetId() == "elif") {
                    IfExp.Clause item = new IfExp.Clause();
                    if (fData.IsHighOrder && fData.LowerOrderFunction.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = fData.LowerOrderFunction.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false, Dsl.DelimiterInfo.Default), fData.GetLine());
                    }
                    for (int ix = 0; ix < fData.GetParamNum(); ++ix) {
                        IExpression subExp = Calculator.Load(fData.GetParam(ix));
                        item.Expressions.Add(subExp);
                    }
                    m_Clauses.Add(item);
                }
                else if (fData.GetId() == "else") {
                    if (fData != statementData.Last) {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false, Dsl.DelimiterInfo.Default), fData.GetLine());
                    }
                    else {
                        IfExp.Clause item = new IfExp.Clause();
                        for (int ix = 0; ix < fData.GetParamNum(); ++ix) {
                            IExpression subExp = Calculator.Load(fData.GetParam(ix));
                            item.Expressions.Add(subExp);
                        }
                        m_Clauses.Add(item);
                    }
                }
                else {
                    //error
                    Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false, Dsl.DelimiterInfo.Default), fData.GetLine());
                }
            }
            UpdateIsAsync();
            return true;
        }

        private void UpdateIsAsync()
        {
            for (int i = 0; i < m_Clauses.Count; ++i) {
                var clause = m_Clauses[i];
                if (null != clause.Condition && clause.Condition.IsAsync) { m_IsAsync = true; return; }
                for (int j = 0; j < clause.Expressions.Count; ++j) {
                    if (clause.Expressions[j].IsAsync) { m_IsAsync = true; return; }
                }
            }
        }

        private sealed class Clause
        {
            internal IExpression Condition;
            internal List<IExpression> Expressions = new List<IExpression>();
        }

        private List<Clause> m_Clauses = new List<Clause>();
        private bool m_IsAsync = false;
    }
    internal sealed class WhileExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            for (; ; ) {
                var condVal = m_Condition.Calc();
                if (condVal.GetLong() != 0) {
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        BoxedValue tv = m_Expressions[index].Calc();
                        if (Calculator.RunState == RunStateEnum.Continue) {
                            Calculator.RunState = RunStateEnum.Normal;
                            break;
                        }
                        else if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                v = tv;
                            }
                            if (Calculator.RunState == RunStateEnum.Break)
                                Calculator.RunState = RunStateEnum.Normal;
                            return v;
                        }
                        else {
                            v = tv;
                        }
                    }
                }
                else {
                    break;
                }
            }
            return v;
        }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue v = 0;
            for (; ; ) {
                BoxedValue condVal;
                if (m_Condition.IsAsync) {
                    var _ei1 = m_Condition.Calc(result);
                    while (_ei1.MoveNext()) { yield return _ei1.Current; }
                    condVal = result.Value;
                }
                else {
                    condVal = m_Condition.Calc();
                }
                if (condVal.GetLong() != 0) {
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        var exp = m_Expressions[index];
                        BoxedValue tv;
                        if (exp.IsAsync) {
                            var _ei2 = exp.Calc(result);
                            while (_ei2.MoveNext()) { yield return _ei2.Current; }
                            tv = result.Value;
                        }
                        else {
                            tv = exp.Calc();
                        }
                        if (Calculator.RunState == RunStateEnum.Continue) {
                            Calculator.RunState = RunStateEnum.Normal;
                            break;
                        }
                        else if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                                v = tv;
                            }
                            if (Calculator.RunState == RunStateEnum.Break)
                                Calculator.RunState = RunStateEnum.Normal;
                            result.Value = v;
                            yield break;
                        }
                        else {
                            v = tv;
                        }
                    }
                }
                else {
                    break;
                }
            }
            result.Value = v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.ISyntaxComponent cond = funcData.LowerOrderFunction.GetParam(0);
                m_Condition = Calculator.Load(cond);
                for (int ix = 0; ix < funcData.GetParamNum(); ++ix) {
                    IExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    m_Expressions.Add(subExp);
                }
            }
            else {
                //error
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false, Dsl.DelimiterInfo.Default), funcData.GetLine());
            }
            UpdateIsAsync();
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'while(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var secondV = statementData.Second.AsValue;
                var secondF = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = statementData.Second.GetId();
                if (firstId == "while" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && (null != secondV || !secondF.HaveStatement() && !secondF.HaveExternScript())) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.GetParam(0);
                        m_Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false, Dsl.DelimiterInfo.Default), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(statementData.Second);
                    m_Expressions.Add(subExp);
                    UpdateIsAsync();
                    return true;
                }
            }
            return false;
        }

        private void UpdateIsAsync()
        {
            m_IsAsync = false;
            if (null != m_Condition && m_Condition.IsAsync)
                m_IsAsync = true;
            if (!m_IsAsync) {
                foreach (var exp in m_Expressions) {
                    if (exp.IsAsync) {
                        m_IsAsync = true;
                        break;
                    }
                }
            }
        }

        private IExpression m_Condition;
        private bool m_IsAsync = false;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            var count = m_Count.Calc();
            long ct = count.GetLong();
            for (int i = 0; i < ct; ++i) {
                Calculator.SetVariable("$$", i);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    BoxedValue tv = m_Expressions[index].Calc();
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                            v = tv;
                        }
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        return v;
                    }
                    else {
                        v = tv;
                    }
                }
            }
            return v;
        }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue v = 0;
            BoxedValue countVal;
            if (m_Count.IsAsync) {
                var _ei1 = m_Count.Calc(result);
                while (_ei1.MoveNext()) { yield return _ei1.Current; }
                countVal = result.Value;
            }
            else {
                countVal = m_Count.Calc();
            }
            long ct = countVal.GetLong();
            for (int i = 0; i < ct; ++i) {
                Calculator.SetVariable("$$", i);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    var exp = m_Expressions[index];
                    BoxedValue tv;
                    if (exp.IsAsync) {
                        var _ei2 = exp.Calc(result);
                        while (_ei2.MoveNext()) { yield return _ei2.Current; }
                        tv = result.Value;
                    }
                    else {
                        tv = exp.Calc();
                    }
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                            v = tv;
                        }
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        result.Value = v;
                        yield break;
                    }
                    else {
                        v = tv;
                    }
                }
            }
            result.Value = v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.ISyntaxComponent count = funcData.LowerOrderFunction.GetParam(0);
                m_Count = Calculator.Load(count);
                for (int ix = 0; ix < funcData.GetParamNum(); ++ix) {
                    IExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    m_Expressions.Add(subExp);
                }
            }
            else {
                //error
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false, Dsl.DelimiterInfo.Default), funcData.GetLine());
            }
            UpdateIsAsync();
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'loop(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var secondV = statementData.Second.AsValue;
                var secondF = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = statementData.Second.GetId();
                if (firstId == "loop" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && (null != secondV || !secondF.HaveStatement() && !secondF.HaveExternScript())) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.GetParam(0);
                        m_Count = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false, Dsl.DelimiterInfo.Default), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(statementData.Second);
                    m_Expressions.Add(subExp);
                    UpdateIsAsync();
                    return true;
                }
            }
            return false;
        }

        private void UpdateIsAsync()
        {
            m_IsAsync = false;
            if (null != m_Count && m_Count.IsAsync)
                m_IsAsync = true;
            if (!m_IsAsync) {
                foreach (var exp in m_Expressions) {
                    if (exp.IsAsync) {
                        m_IsAsync = true;
                        break;
                    }
                }
            }
        }

        private IExpression m_Count;
        private List<IExpression> m_Expressions = new List<IExpression>();
        private bool m_IsAsync = false;
    }
    internal sealed class LoopListExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            var list = m_List.Calc();
            IEnumerable obj = list.As<IEnumerable>();
            if (null != obj && obj is IEnumerable<BoxedValue> bvEnumer) {
                var enumer = bvEnumer.GetEnumerator();
                while (enumer.MoveNext()) {
                    var val = enumer.Current;
                    if (LoopOnce(val, ref v))
                        return v;
                }
            }
            else if (null != obj) {
                var enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    var val = BoxedValue.FromObject(enumer.Current);
                    if (LoopOnce(val, ref v))
                        return v;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.ISyntaxComponent list = funcData.LowerOrderFunction.GetParam(0);
                m_List = Calculator.Load(list);
                for (int ix = 0; ix < funcData.GetParamNum(); ++ix) {
                    IExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    m_Expressions.Add(subExp);
                }
            }
            else {
                //error
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false, Dsl.DelimiterInfo.Default), funcData.GetLine());
            }
            UpdateIsAsync();
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'looplist(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var secondV = statementData.Second.AsValue;
                var secondF = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = statementData.Second.GetId();
                if (firstId == "looplist" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && (null != secondV || !secondF.HaveStatement() && !secondF.HaveExternScript())) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.GetParam(0);
                        m_List = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false, Dsl.DelimiterInfo.Default), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(statementData.Second);
                    m_Expressions.Add(subExp);
                    UpdateIsAsync();
                    return true;
                }
            }
            return false;
        }
        private bool LoopOnce(in BoxedValue val, ref BoxedValue ret)
        {
            Calculator.SetVariable("$$", val);
            for (int index = 0; index < m_Expressions.Count; ++index) {
                BoxedValue v = m_Expressions[index].Calc();
                if (Calculator.RunState == RunStateEnum.Continue) {
                    Calculator.RunState = RunStateEnum.Normal;
                    break;
                }
                else if (Calculator.RunState != RunStateEnum.Normal) {
                    if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                        ret = v;
                    }
                    if (Calculator.RunState == RunStateEnum.Break)
                        Calculator.RunState = RunStateEnum.Normal;
                    return true;
                }
                else {
                    ret = v;
                }
            }
            return false;
        }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue v = 0;
            var listResult = new AsyncCalcResult();
            if (m_List.IsAsync) {
                var _ei1 = m_List.Calc(listResult);
                while (_ei1.MoveNext()) { yield return _ei1.Current; }
            }
            else {
                listResult.Value = m_List.Calc();
            }
            var list = listResult.Value;
            IEnumerable obj = list.As<IEnumerable>();
            if (null != obj && obj is IEnumerable<BoxedValue> bvEnumer) {
                var enumer = bvEnumer.GetEnumerator();
                while (enumer.MoveNext()) {
                    var val = enumer.Current;
                    var loopResult = new AsyncCalcResult();
                    var _ei2 = LoopOnceAsync(val, loopResult);
                    while (_ei2.MoveNext()) { yield return _ei2.Current; }
                    v = loopResult.Value;
                    if (loopResult.IsBreak) {
                        break;
                    }
                }
            }
            else if (null != obj) {
                var enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    var val = BoxedValue.FromObject(enumer.Current);
                    var loopResult = new AsyncCalcResult();
                    var _ei3 = LoopOnceAsync(val, loopResult);
                    while (_ei3.MoveNext()) { yield return _ei3.Current; }
                    v = loopResult.Value;
                    if (loopResult.IsBreak) {
                        break;
                    }
                }
            }
            result.Value = v;
        }
        private IEnumerator LoopOnceAsync(BoxedValue val, AsyncCalcResult loopResult)
        {
            Calculator.SetVariable("$$", val);
            for (int index = 0; index < m_Expressions.Count; ++index) {
                var exp = m_Expressions[index];
                BoxedValue v;
                if (exp.IsAsync) {
                    var subResult = new AsyncCalcResult();
                    var _ei = exp.Calc(subResult);
                    while (_ei.MoveNext()) { yield return _ei.Current; }
                    v = subResult.Value;
                }
                else {
                    v = exp.Calc();
                }
                if (Calculator.RunState == RunStateEnum.Continue) {
                    Calculator.RunState = RunStateEnum.Normal;
                    break;
                }
                else if (Calculator.RunState != RunStateEnum.Normal) {
                    if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                        loopResult.Value = v;
                    }
                    if (Calculator.RunState == RunStateEnum.Break)
                        Calculator.RunState = RunStateEnum.Normal;
                    loopResult.IsBreak = true;
                    yield break;
                }
                else {
                    loopResult.Value = v;
                }
            }
        }

        private void UpdateIsAsync()
        {
            m_IsAsync = false;
            if (null != m_List && m_List.IsAsync)
                m_IsAsync = true;
            if (!m_IsAsync) {
                foreach (var exp in m_Expressions) {
                    if (exp.IsAsync) {
                        m_IsAsync = true;
                        break;
                    }
                }
            }
        }

        private IExpression m_List;
        private bool m_IsAsync = false;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ForeachExp : AbstractExpression
    {
        public override bool IsAsync { get { return m_IsAsync; } }
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            List<BoxedValue> list = new List<BoxedValue>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                var val = m_Elements[ix].Calc();
                list.Add(val);
            }
            var enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                var val = enumer.Current;
                Calculator.SetVariable("$$", val);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    BoxedValue tv = m_Expressions[index].Calc();
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                            v = tv;
                        }
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        return v;
                    }
                    else {
                        v = tv;
                    }
                }
            }
            return v;
        }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            BoxedValue v = 0;
            List<BoxedValue> list = new List<BoxedValue>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                var exp = m_Elements[ix];
                if (exp.IsAsync) {
                    var subResult = new AsyncCalcResult();
                    var _ei1 = exp.Calc(subResult);
                    while (_ei1.MoveNext()) { yield return _ei1.Current; }
                    list.Add(subResult.Value);
                }
                else {
                    list.Add(exp.Calc());
                }
            }
            var enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                var val = enumer.Current;
                Calculator.SetVariable("$$", val);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    var exp = m_Expressions[index];
                    BoxedValue tv;
                    if (exp.IsAsync) {
                        var subResult = new AsyncCalcResult();
                        var _ei2 = exp.Calc(subResult);
                        while (_ei2.MoveNext()) { yield return _ei2.Current; }
                        tv = subResult.Value;
                    }
                    else {
                        tv = exp.Calc();
                    }
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Return || Calculator.RunState == RunStateEnum.Redirect) {
                            v = tv;
                        }
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        result.Value = v;
                        yield break;
                    }
                    else {
                        v = tv;
                    }
                }
            }
            result.Value = v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.FunctionData callData = funcData.LowerOrderFunction;
                int num = callData.GetParamNum();
                for (int ix = 0; ix < num; ++ix) {
                    Dsl.ISyntaxComponent exp = callData.GetParam(ix);
                    m_Elements.Add(Calculator.Load(exp));
                }
            }
            if (funcData.HaveStatement()) {
                int fnum = funcData.GetParamNum();
                for (int ix = 0; ix < fnum; ++ix) {
                    IExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    m_Expressions.Add(subExp);
                }
            }
            UpdateIsAsync();
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'foreach(exp1,exp2,...) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var secondV = statementData.Second.AsValue;
                var secondF = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = statementData.Second.GetId();
                if (firstId == "foreach" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && (null != secondV || !secondF.HaveStatement() && !secondF.HaveExternScript())) {
                    int num = first.GetParamNum();
                    if (num > 0) {
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent exp = first.GetParam(ix);
                            m_Elements.Add(Calculator.Load(exp));
                        }
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false, Dsl.DelimiterInfo.Default), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(statementData.Second);
                    m_Expressions.Add(subExp);
                    UpdateIsAsync();
                    return true;
                }
            }
            return false;
        }
        private void UpdateIsAsync()
        {
            m_IsAsync = false;
            foreach (var exp in m_Elements) {
                if (exp.IsAsync) {
                    m_IsAsync = true;
                    break;
                }
            }
            if (!m_IsAsync) {
                foreach (var exp in m_Expressions) {
                    if (exp.IsAsync) {
                        m_IsAsync = true;
                        break;
                    }
                }
            }
        }

        private List<IExpression> m_Elements = new List<IExpression>();
        private List<IExpression> m_Expressions = new List<IExpression>();
        private bool m_IsAsync = false;
    }
    internal sealed class TupleExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v;
            int num = m_Expressions.Count;
            if (num == 0) {
                v = BoxedValue.NullObject;
            }
            else {
                v = PackValues(0);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }
        private BoxedValue PackValues(int start)
        {
            const int c_MaxTupleElementNum = 8;
            BoxedValue v1 = BoxedValue.NullObject, v2 = BoxedValue.NullObject, v3 = BoxedValue.NullObject, v4 = BoxedValue.NullObject, v5 = BoxedValue.NullObject, v6 = BoxedValue.NullObject, v7 = BoxedValue.NullObject, v8 = BoxedValue.NullObject;
            int totalNum = m_Expressions.Count;
            int num = totalNum - start;
            for (int ix = 0; ix < num && ix < c_MaxTupleElementNum; ++ix) {
                var exp = m_Expressions[start + ix];
                switch (ix) {
                    case 0:
                        v1 = exp.Calc();
                        if (num == 1) {
                            return new TupleValue1(v1);
                        }
                        break;
                    case 1:
                        v2 = exp.Calc();
                        if (num == 2) {
                            return new TupleValue2(v1, v2);
                        }
                        break;
                    case 2:
                        v3 = exp.Calc();
                        if (num == 3) {
                            return new TupleValue3(v1, v2, v3);
                        }
                        break;
                    case 3:
                        v4 = exp.Calc();
                        if (num == 4) {
                            return new TupleValue4(v1, v2, v3, v4);
                        }
                        break;
                    case 4:
                        v5 = exp.Calc();
                        if (num == 5) {
                            return new TupleValue5(v1, v2, v3, v4, v5);
                        }
                        break;
                    case 5:
                        v6 = exp.Calc();
                        if (num == 6) {
                            return new TupleValue6(v1, v2, v3, v4, v5, v6);
                        }
                        break;
                    case 6:
                        v7 = exp.Calc();
                        if (num == 7) {
                            return new TupleValue7(v1, v2, v3, v4, v5, v6, v7);
                        }
                        break;
                    case 7:
                        if (num == 8) {
                            v8 = exp.Calc();
                            return new TupleValue8(v1, v2, v3, v4, v5, v6, v7, Tuple.Create(v8));
                        }
                        else {
                            var tuple = PackValues(start + 7);
                            return new TupleValue8(v1, v2, v3, v4, v5, v6, v7, Tuple.Create(tuple));
                        }
                }
            }
            return BoxedValue.NullObject;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class TupleSetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var val = m_Op.Calc();
            bool success = true;
            var setVars = new Dictionary<string, BoxedValue>();
            MatchRecursively(ref success, setVars, val, m_VarIds, 0);
            if (success) {
                foreach (var pair in setVars) {
                    Calculator.SetVariable(pair.Key, pair.Value);
                }
            }
            return BoxedValue.FromBool(success);
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            var vars = param1 as Dsl.FunctionData;
            if (null != vars) {
                LoadRecursively(vars, m_VarIds);
            }
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Op = Calculator.Load(param2);
            return true;
        }
        private void LoadRecursively(Dsl.FunctionData vars, List<ValueTuple<string, int>> varIds)
        {
            int num = vars.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var p = vars.GetParam(i);
                var pvd = p as Dsl.ValueData;
                var pfd = p as Dsl.FunctionData;
                if (null != pvd) {
                    varIds.Add(ValueTuple.Create(pvd.GetId(), -1));
                }
                else if (null != pfd && !pfd.HaveId()) {
                    m_EmbeddedVars.Add(new List<ValueTuple<string, int>>());
                    int index = m_EmbeddedVars.Count - 1;
                    varIds.Add(ValueTuple.Create(string.Empty, index));
                    LoadRecursively(pfd, m_EmbeddedVars[index]);
                }
                else {
                    Calculator.Log("invalid tuple member {0}. code:{1} line:{2}", i, p.ToScriptString(false, Dsl.DelimiterInfo.Default), p.GetLine());
                }
            }
        }
        private void MatchRecursively(ref bool success, Dictionary<string, BoxedValue> setVars, BoxedValue val, List<ValueTuple<string, int>> varIds, int start)
        {
            int num = varIds.Count - start;
            if (num == 1) {
                //tuple1 may be a single value, in order to use it both for tuple1 and for normal parentheses usage
                if (val.IsTuple) {
                    var tuple1 = val.GetTuple1();
                    if (null != tuple1) {
                        val = tuple1.Item1;
                    }
                }
                MatchItem(ref success, setVars, varIds[start], val);
            }
            else {
                switch (val.Type) {
                    case BoxedValue.c_Tuple2Type:
                        if (num == 2) {
                            var tuple = val.GetTuple2();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple3Type:
                        if (num == 3) {
                            var tuple = val.GetTuple3();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple4Type:
                        if (num == 4) {
                            var tuple = val.GetTuple4();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple5Type:
                        if (num == 5) {
                            var tuple = val.GetTuple5();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple6Type:
                        if (num == 6) {
                            var tuple = val.GetTuple6();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple7Type:
                        if (num == 7) {
                            var tuple = val.GetTuple7();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                            MatchItem(ref success, setVars, varIds[start + 6], tuple.Item7);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple8Type:
                        if (num >= 8) {
                            var tuple = val.GetTuple8();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                            MatchItem(ref success, setVars, varIds[start + 6], tuple.Item7);
                            MatchRecursively(ref success, setVars, tuple.Rest.Item1, varIds, start + 7);
                        }
                        else {
                            success = false;
                        }
                        break;
                }
            }
        }
        private void MatchItem(ref bool success, Dictionary<string, BoxedValue> setVars, ValueTuple<string, int> var, BoxedValue val)
        {
            string varId = var.Item1;
            if (string.IsNullOrEmpty(varId)) {
                int index = var.Item2;
                if (index >= 0 && index < m_EmbeddedVars.Count) {
                    var newVarIds = m_EmbeddedVars[index];
                    MatchRecursively(ref success, setVars, val, newVarIds, 0);
                }
                else {
                    success = false;
                }
            }
            else {
                setVars[varId] = val;
            }
        }

        private List<ValueTuple<string, int>> m_VarIds = new List<ValueTuple<string, int>>();
        private List<List<ValueTuple<string, int>>> m_EmbeddedVars = new List<List<ValueTuple<string, int>>>();
        private IExpression m_Op = null;
    }
    internal sealed class FormatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue v = 0;
            string fmt = string.Empty;
            ArrayList al = new ArrayList();
            for (int ix = 0; ix < operands.Count; ++ix) {
                v = operands[ix];
                if (ix == 0)
                    fmt = v.AsString;
                else
                    al.Add(v.GetObject());
            }
            v = string.Format(fmt, al.ToArray());
            return v;
        }
    }
    internal sealed class GetTypeAssemblyNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0].GetObject();
                try {
                    if (null != obj) {
                        ret = obj.GetType().AssemblyQualifiedName;
                    }
                    else {
                        ret = "(null)";
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
    }
    internal sealed class GetTypeFullNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0].GetObject();
                try {
                    if (null != obj) {
                        ret = obj.GetType().FullName;
                    }
                    else {
                        ret = "(null)";
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
    }
    internal sealed class GetTypeNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0].GetObject();
                try {
                    if (null != obj) {
                        ret = obj.GetType().Name;
                    }
                    else {
                        ret = "(null)";
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
    }
    internal sealed class GetTypeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var type = operands[0];
                try {
                    var r = Resolve(type);
                    if (null == r) {
                        Calculator.Log("null == Type.GetType({0})", type);
                    }
                    else {
                        ret = BoxedValue.FromObject(r);
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }

        internal static Type Resolve(string type)
        {
            if (string.IsNullOrEmpty(type))
                return null;
            var r = Type.GetType("UnityEngine." + type + ", UnityEngine");
            if (null == r) {
                r = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
            }
            if (null == r) {
                r = Type.GetType("UnityEditor." + type + ", UnityEditor");
            }
            if (null == r) {
                r = Type.GetType(type + ", UnityEngine");
            }
            if (null == r) {
                r = Type.GetType(type + ", UnityEngine.UI");
            }
            if (null == r) {
                r = Type.GetType(type + ", UnityEditor");
            }
            if (null == r) {
                r = Type.GetType(type + ", Assembly-CSharp");
            }
            if (null == r) {
                r = Type.GetType(type);
            }
            return r;
        }
        internal static Type Resolve(BoxedValue typeVal)
        {
            var type = typeVal.AsString;
            if (string.IsNullOrEmpty(type)) {
                return typeVal.As<Type>();
            }
            else {
                return Resolve(type);
            }
        }
    }
    internal sealed class ChangeTypeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var obj = operands[0];
                string type = operands[1].AsString;
                try {
                    string str = obj.AsString;
                    if (obj.IsString) {
                        if (0 == type.CompareTo("sbyte")) {
                            ret = CastTo<sbyte>(str);
                        }
                        else if (0 == type.CompareTo("byte")) {
                            ret = CastTo<byte>(str);
                        }
                        else if (0 == type.CompareTo("short")) {
                            ret = CastTo<short>(str);
                        }
                        else if (0 == type.CompareTo("ushort")) {
                            ret = CastTo<ushort>(str);
                        }
                        else if (0 == type.CompareTo("int")) {
                            ret = CastTo<int>(str);
                        }
                        else if (0 == type.CompareTo("uint")) {
                            ret = CastTo<uint>(str);
                        }
                        else if (0 == type.CompareTo("long")) {
                            ret = CastTo<long>(str);
                        }
                        else if (0 == type.CompareTo("ulong")) {
                            ret = CastTo<ulong>(str);
                        }
                        else if (0 == type.CompareTo("float")) {
                            ret = CastTo<float>(str);
                        }
                        else if (0 == type.CompareTo("double")) {
                            ret = CastTo<double>(str);
                        }
                        else if (0 == type.CompareTo("string")) {
                            ret = str;
                        }
                        else if (0 == type.CompareTo("bool")) {
                            ret = CastTo<bool>(str);
                        }
                        else {
                            Type t = GetTypeExp.Resolve(type);
                            if (null != t) {
                                ret = BoxedValue.FromObject(CastTo(t, str));
                            }
                            else {
                                Calculator.Log("null == Type.GetType({0})", type);
                            }
                        }
                    }
                    else {
                        if (0 == type.CompareTo("sbyte")) {
                            ret = obj.GetSByte();
                        }
                        else if (0 == type.CompareTo("byte")) {
                            ret = obj.GetByte();
                        }
                        else if (0 == type.CompareTo("short")) {
                            ret = obj.GetShort();
                        }
                        else if (0 == type.CompareTo("ushort")) {
                            ret = obj.GetUShort();
                        }
                        else if (0 == type.CompareTo("int")) {
                            ret = obj.GetInt();
                        }
                        else if (0 == type.CompareTo("uint")) {
                            ret = obj.GetUInt();
                        }
                        else if (0 == type.CompareTo("long")) {
                            ret = obj.GetLong();
                        }
                        else if (0 == type.CompareTo("ulong")) {
                            ret = obj.GetULong();
                        }
                        else if (0 == type.CompareTo("float")) {
                            ret = obj.GetFloat();
                        }
                        else if (0 == type.CompareTo("double")) {
                            ret = obj.GetDouble();
                        }
                        else if (0 == type.CompareTo("string")) {
                            ret = obj.GetString();
                        }
                        else if (0 == type.CompareTo("bool")) {
                            ret = obj.GetBool();
                        }
                        else {
                            Type t = GetTypeExp.Resolve(type);
                            if (null != t) {
                                ret = BoxedValue.FromObject(obj.CastTo(t));
                            }
                            else {
                                Calculator.Log("null == Type.GetType({0})", type);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
    }
    internal sealed class ParseEnumExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                string type = operands[0].AsString;
                string val = operands[1].AsString;
                try {
                    Type t = GetTypeExp.Resolve(type);
                    if (null != t) {
                        ret = BoxedValue.FromObject(Enum.Parse(t, val, true));
                    }
                    else {
                        Calculator.Log("null == Type.GetType({0})", type);
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
    }
    internal sealed class DotnetCallExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            string method = null;
            List<BoxedValue> args = null;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
                    bvMethod = v;
                    method = v.AsString;
                }
                else if (null != disp) {
                    if (null == args)
                        args = Calculator.NewCalculatorValueList();
                    args.Add(v);
                }
                else {
                    if (null == arglist)
                        arglist = new ArrayList();
                    arglist.Add(v.GetObject());
                }
            }
            if (null != obj && null != method) {
                if (null != disp) {
                    if (m_DispId < 0) {
                        m_DispId = disp.GetDispatchId(method);
                    }
                    if (m_DispId >= 0) {
                        ret = disp.InvokeMethod(m_DispId, args);
                    }
                    Calculator.RecycleCalculatorValueList(args);
                }
                else {
                    if (null == arglist)
                        arglist = new ArrayList();
                    object[] _args = arglist.ToArray();
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict && bvDict.TryGetValue(bvMethod, out var val) && val.IsObject && val.ObjectVal is Delegate) {
                        var d = val.ObjectVal as Delegate;
                        if (null != d) {
                            ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                        }
                    }
                    else if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
                        var d = dict[method] as Delegate;
                        if (null != d) {
                            ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                        }
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                CastArgsForCall(t, method, flags, _args);
                                ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, null, _args));
                            }
                            catch (Exception ex) {
                                Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                    CastArgsForCall(t, method, flags, _args);
                                    ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, obj, _args));
                                }
                                catch (Exception ex) {
                                    Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private int m_DispId = -1;
    }
    internal sealed class DotnetSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            string method = null;
            BoxedValue argv = BoxedValue.NullObject;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
                    bvMethod = v;
                    method = v.AsString;
                }
                else if (null != disp) {
                    argv = v;
                    break;
                }
                else {
                    if (null == arglist)
                        arglist = new ArrayList();
                    arglist.Add(v.GetObject());
                }
            }
            if (null != obj && null != method) {
                if (null != disp) {
                    if (m_DispId < 0) {
                        m_DispId = disp.GetDispatchId(method);
                    }
                    if (m_DispId >= 0) {
                        disp.SetProperty(m_DispId, argv);
                    }
                }
                else {
                    object[] _args = arglist.ToArray();
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && null == obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)) {
                        if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                            bvDict[bvMethod] = BoxedValue.FromObject(_args[0]);
                        }
                        else {
                            dict[method] = _args[0];
                        }
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                CastArgsForSet(t, method, flags, _args);
                                ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, null, _args));
                            }
                            catch (Exception ex) {
                                Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                    CastArgsForSet(t, method, flags, _args);
                                    ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, obj, _args));
                                }
                                catch (Exception ex) {
                                    Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private int m_DispId = -1;
    }
    internal sealed class DotnetGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            string method = null;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
                    bvMethod = v;
                    method = v.AsString;
                }
                else if (null != disp) {
                    break;
                }
                else {
                    if (null == arglist)
                        arglist = new ArrayList();
                    arglist.Add(v.GetObject());
                }
            }
            if (null != obj && null != method) {
                if (null != disp) {
                    if (m_DispId < 0) {
                        m_DispId = disp.GetDispatchId(method);
                    }
                    if (m_DispId >= 0) {
                        ret = disp.GetProperty(m_DispId);
                    }
                }
                else {
                    if (null == arglist)
                        arglist = new ArrayList();
                    object[] _args = arglist.ToArray();
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict && bvDict.TryGetValue(bvMethod, out var val)) {
                        ret = val;
                    }
                    else if (null != dict && dict.Contains(method)) {
                        ret = BoxedValue.FromObject(dict[method]);
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                CastArgsForGet(t, method, flags, _args);
                                ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, null, _args));
                            }
                            catch (Exception ex) {
                                Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
                                    CastArgsForGet(t, method, flags, _args);
                                    ret = BoxedValue.FromObject(t.InvokeMember(method, flags, null, obj, _args));
                                }
                                catch (Exception ex) {
                                    Calculator.Log("InvokeMember {0} Exception:{1}\n{2}", method, ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private int m_DispId = -1;
    }
    internal sealed class CollectionCallExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            object methodObj = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    bvMethod = v;
                    methodObj = v.GetObject();
                }
                else {
                    arglist.Add(v.GetObject());
                }
            }
            object[] _args = arglist.ToArray();
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict && bvDict.TryGetValue(bvMethod, out var val)) {
                    var d = val.As<Delegate>();
                    if (null != d) {
                        ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                    }
                }
                else if (null != dict && dict.Contains(methodObj)) {
                    var d = dict[methodObj] as Delegate;
                    if (null != d) {
                        ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                    }
                }
                else {
                    IList list = obj as IList;
                    if (null != list && bvMethod.IsInteger) {
                        int index = bvMethod.GetInt();
                        if (index >= 0 && index < list.Count) {
                            var d = list[index] as Delegate;
                            if (null != d) {
                                ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                            }
                        }
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && bvMethod.IsInteger) {
                            int index = bvMethod.GetInt();
                            var e = enumer.GetEnumerator();
                            for (int i = 0; i <= index; ++i) {
                                e.MoveNext();
                            }
                            var d = e.Current as Delegate;
                            if (null != d) {
                                ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CollectionSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            object methodObj = null;
            object arg = null;
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    bvMethod = v;
                    methodObj = v.GetObject();
                }
                else {
                    arg = v.GetObject();
                    break;
                }
            }
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict[bvMethod] = BoxedValue.FromObject(arg);
                }
                else if (null != dict) {
                    dict[methodObj] = arg;
                }
                else {
                    IList list = obj as IList;
                    if (null != list && bvMethod.IsInteger) {
                        int index = bvMethod.GetInt();
                        if (index >= 0 && index < list.Count) {
                            list[index] = arg;
                        }
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CollectionGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            BoxedValue bvMethod = BoxedValue.NullObject;
            object methodObj = null;
            for (int ix = 0; ix < operands.Count; ++ix) {
                var v = operands[ix];
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    bvMethod = v;
                    methodObj = v.GetObject();
                }
                else {
                    break;
                }
            }
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict && bvDict.TryGetValue(bvMethod, out var val)) {
                    ret = val;
                }
                else if (null != dict && dict.Contains(methodObj)) {
                    ret = BoxedValue.FromObject(dict[methodObj]);
                }
                else {
                    IList list = obj as IList;
                    if (null != list && bvMethod.IsInteger) {
                        int index = bvMethod.GetInt();
                        if (index >= 0 && index < list.Count) {
                            var d = list[index];
                            ret = BoxedValue.FromObject(d);
                        }
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && bvMethod.IsInteger) {
                            int index = bvMethod.GetInt();
                            var e = enumer.GetEnumerator();
                            for (int i = 0; i <= index; ++i) {
                                e.MoveNext();
                            }
                            ret = BoxedValue.FromObject(e.Current);
                        }
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class LinqExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            var list = m_List.Calc().GetObject();
            var method = m_Method.Calc().GetString();
            IEnumerable obj = list as IEnumerable;
            if (null != obj && !string.IsNullOrEmpty(method)) {
                if (method == "orderby" || method == "orderbydesc") {
                    bool desc = method == "orderbydesc";
                    List<BoxedValue> results = new List<BoxedValue>();
                    var enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);
                        results.Add(val);
                    }
                    results.Sort((BoxedValue o1, BoxedValue o2) => {
                        Calculator.SetVariable("$$", o1);
                        var r1 = BoxedValue.NullObject;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r1 = m_Expressions[index].Calc();
                        }
                        Calculator.SetVariable("$$", o2);
                        var r2 = BoxedValue.NullObject;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r2 = m_Expressions[index].Calc();
                        }
                        int r = 0;
                        if (r1.IsString && r2.IsString) {
                            r = r1.GetString().CompareTo(r2.GetString());
                        }
                        else {
                            double rd1 = r1.GetDouble();
                            double rd2 = r2.GetDouble();
                            r = rd1.CompareTo(rd2);
                        }
                        if (desc)
                            r = -r;
                        return r;
                    });
                    v = BoxedValue.FromObject(results);
                }
                else if (method == "where") {
                    List<BoxedValue> results = new List<BoxedValue>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);

                        Calculator.SetVariable("$$", val);
                        BoxedValue r = BoxedValue.NullObject;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r = m_Expressions[index].Calc();
                        }
                        if (r.GetLong() != 0) {
                            results.Add(val);
                        }
                    }
                    v = BoxedValue.FromObject(results);
                }
                else if (method == "top") {
                    BoxedValue r = BoxedValue.NullObject;
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        r = m_Expressions[index].Calc();
                    }
                    long ct = r.GetLong();
                    List<BoxedValue> results = new List<BoxedValue>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);
                        if (ct > 0) {
                            results.Add(val);
                            --ct;
                        }
                    }
                    v = BoxedValue.FromObject(results);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.ISyntaxComponent list = callData.GetParam(0);
            m_List = Calculator.Load(list);
            Dsl.ISyntaxComponent method = callData.GetParam(1);
            m_Method = Calculator.Load(method);
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private IExpression m_List;
        private IExpression m_Method;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class IsNullExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0];
                ret = obj.IsNullObject;
            }
            return ret;
        }
    }
    internal sealed class NullExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            return ret;
        }
    }
    internal sealed class EqualsNullExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0];
                ret = object.Equals(null, obj.ObjectVal);
            }
            return ret;
        }
    }
    internal sealed class DotnetLoadExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: dotnetload(dll_path) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
                    r = BoxedValue.FromObject(Assembly.LoadFile(path));
                }
            }
            return r;
        }
    }
    internal sealed class DotnetNewExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: dotnetnew(assembly,type_name,arg1,arg2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var assem = operands[0].As<Assembly>();
                string typeName = operands[1].AsString;
                if (null != assem && !string.IsNullOrEmpty(typeName)) {
                    var al = new ArrayList();
                    for (int i = 2; i < operands.Count; ++i) {
                        al.Add(operands[i].GetObject());
                    }
                    r = BoxedValue.FromObject(assem.CreateInstance(typeName, false, BindingFlags.CreateInstance, null, al.ToArray(), System.Globalization.CultureInfo.CurrentCulture, null));
                }
            }
            return r;
        }
    }
    internal sealed class CallStackExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: callstack() api");
            var r = System.Environment.StackTrace;
            return BoxedValue.FromObject(r);
        }
    }
    internal sealed class CallExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: call(func_name,arg1,arg2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var func = operands[0].AsString;
                if (null != func) {
                    var args = Calculator.NewCalculatorValueList();
                    for (int i = 1; i < operands.Count; ++i) {
                        args.Add(operands[i]);
                    }
                    r = Calculator.Calc(func, args);
                    Calculator.RecycleCalculatorValueList(args);
                }
            }
            return r;
        }
    }
    internal sealed class ReturnExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: return([val]) api");
            Calculator.RunState = RunStateEnum.Return;
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                r = operands[0];
            }
            return r;
        }
    }
    internal sealed class RedirectExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: redirect(arg1,arg2,...) api");
            Calculator.RunState = RunStateEnum.Redirect;
            if (operands.Count >= 1) {
                List<string> args = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var arg = operands[i].ToString();
                    args.Add(arg);
                }
                return BoxedValue.FromObject(args);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class AwaitExp : SimpleAsyncExpressionBase
    {
        protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: await(\"func_name\", arg1, arg2, ...) api");
            var funcName = operands[0].AsString;
            var args = Calculator.NewCalculatorValueList();
            for (int i = 1; i < operands.Count; ++i) {
                args.Add(operands[i]);
            }
            var asyncResult = new AsyncCalcResult();
            var enumerator = Calculator.CalcAsync(funcName, args, asyncResult);
            Calculator.RecycleCalculatorValueList(args);
            var _ei = enumerator;
            while (_ei.MoveNext()) { yield return _ei.Current; }
            result.Value = asyncResult.Value;
        }
    }
    internal sealed class AwaitWhileExp : SimpleAsyncExpressionBase
    {
        protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: awaitwhile(\"func_name\", arg1, arg2, ...) api, loop call sync function while result is true");
            var funcName = operands[0].AsString;
            var args = Calculator.NewCalculatorValueList();
            for (int i = 1; i < operands.Count; ++i) {
                args.Add(operands[i]);
            }
            BoxedValue v = Calculator.Calc(funcName, args);
            while (v.GetBool()) {
                yield return null;
                v = Calculator.Calc(funcName, args);
            }
            Calculator.RecycleCalculatorValueList(args);
            result.Value = v;
        }
    }
    internal sealed class AwaitUntilExp : SimpleAsyncExpressionBase
    {
        protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: awaituntil(\"func_name\", arg1, arg2, ...) api, loop call sync function until result is true");
            var funcName = operands[0].AsString;
            var args = Calculator.NewCalculatorValueList();
            for (int i = 1; i < operands.Count; ++i) {
                args.Add(operands[i]);
            }
            BoxedValue v = Calculator.Calc(funcName, args);
            while (!v.GetBool()) {
                yield return null;
                v = Calculator.Calc(funcName, args);
            }
            Calculator.RecycleCalculatorValueList(args);
            result.Value = v;
        }
    }
    internal sealed class AwaitTimeExp : SimpleAsyncExpressionBase
    {
        protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: awaittime(milliseconds) api, async wait specified time");
            int ms = operands[0].GetInt();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < ms) {
                yield return null;
            }
            sw.Stop();
            result.Value = BoxedValue.FromObject((int)sw.ElapsedMilliseconds);
        }
    }
    public enum RunStateEnum
    {
        Normal = 0,
        Break,
        Continue,
        Return,
        Redirect,
    }
    public sealed class DslCalculator
    {
        public delegate bool TryGetVariableDelegation(string v, out BoxedValue result);
        public delegate bool TrySetVariableDelegation(string v, ref BoxedValue result);
        public delegate bool LoadFailbackDelegation(Dsl.ISyntaxComponent comp, DslCalculator calculator, out IExpression expression);
        public class FuncInfo
        {
            public Dictionary<string, int> LocalVarIndexes = new Dictionary<string, int>();
            public List<IExpression> Codes = new List<IExpression>();
            public bool IsAsync = false;
            public int ParamNum = 0;

            public void AddParamNameIndex(string paramName)
            {
                int ix = LocalVarIndexes.Count;
                LocalVarIndexes.Add(paramName, -1 - ix);
                ++ParamNum;
            }
            public void BuildParamNameIndexes(IList<string> paramNames)
            {
                if (null != paramNames) {
                    for (int ix = 0; ix < paramNames.Count; ++ix) {
                        AddParamNameIndex(paramNames[ix]);
                    }
                }
            }
        }

        public Dsl.DslLogDelegation OnLog;
        public TryGetVariableDelegation OnTryGetVariable;
        public TrySetVariableDelegation OnTrySetVariable;
        public LoadFailbackDelegation OnLoadFailback;

        public DslCalculatorApiRegistry ApiRegistry
        {
            get { return m_ApiRegistry; }
            set { m_ApiRegistry = value; }
        }

        public void NewApiRegistry()
        {
            m_ApiRegistry = new DslCalculatorApiRegistry();
            m_ApiRegistry.Init();
        }
        public void Register(string name, string doc, IExpressionFactory factory)
        {
            if (null != m_ApiRegistry) {
                m_ApiRegistry.Register(name, doc, factory);
            }
        }
        public SortedList<string, string> ApiDocs
        {
            get {
                if (null != m_ApiRegistry) {
                    return m_ApiRegistry.ApiDocs;
                }
                return null;
            }
        }
        public void Clear()
        {
            m_Funcs.Clear();
            m_Stack.Clear();
            m_GlobalSyncCalculationStack.Clear();
            m_NamedGlobalVariableIndexes.Clear();
            m_GlobalVariables.Clear();
        }
        public void ClearGlobalVariables()
        {
            m_NamedGlobalVariableIndexes.Clear();
            m_GlobalVariables.Clear();
        }
        public IEnumerable<string> GlobalVariableNames
        {
            get { return m_NamedGlobalVariableIndexes.Keys; }
        }
        public bool TryGetGlobalVariable(string v, out BoxedValue result)
        {
            if (null != OnTryGetVariable && OnTryGetVariable(v, out result)) {
                return true;
            }
            else if (m_NamedGlobalVariableIndexes.TryGetValue(v, out var index)) {
                result = GetGlobalVaraibleByIndex(index);
                return true;
            }
            else {
                result = BoxedValue.NullObject;
                return false;
            }
        }
        public BoxedValue GetGlobalVariable(string v)
        {
            BoxedValue result;
            TryGetGlobalVariable(v, out result);
            return result;
        }
        public void SetGlobalVariable(string v, BoxedValue val)
        {
            if (null != OnTrySetVariable && OnTrySetVariable(v, ref val)) {

            }
            else if (m_NamedGlobalVariableIndexes.TryGetValue(v, out var index)) {
                SetGlobalVaraibleByIndex(index, val);
            }
            else {
                int ix = m_NamedGlobalVariableIndexes.Count;
                m_NamedGlobalVariableIndexes.Add(v, ix);
                m_GlobalVariables.Add(val);
            }
        }
        public void LoadDsl(string dslFile)
        {
            Dsl.DslFile file = new Dsl.DslFile();
            ScriptableDslHelper.ForDslCalculator.SetCallbacks(file);
            string path = dslFile;
            if (file.Load(path, LogError)) {
                foreach (Dsl.ISyntaxComponent info in file.DslInfos) {
                    LoadDsl(info);
                }
            }
        }
        public void LoadDsl(Dsl.ISyntaxComponent info)
        {
            if (info.GetId() != "script")
                return;
            var func = info as Dsl.FunctionData;
            string id;
            FuncInfo funcInfo = null;
            bool isAsync = false;
            if (null != func) {
                if (func.IsHighOrder) {
                    id = func.LowerOrderFunction.GetParamId(0);
                    if (func.LowerOrderFunction.GetParamNum() > 1 && func.LowerOrderFunction.GetParamId(1) == "async")
                        isAsync = true;
                }
                else
                    return;
            }
            else {
                var statement = info as Dsl.StatementData;
                if (null != statement && statement.GetFunctionNum() == 2) {
                    id = statement.First.AsFunction.GetParamId(0);
                    if (statement.First.AsFunction.GetParamNum() > 1 && statement.First.AsFunction.GetParamId(1) == "async")
                        isAsync = true;
                    func = statement.Second.AsFunction;
                    if ((func.GetId() == "params" || func.GetId() == "args") && func.IsHighOrder) {
                        if (func.LowerOrderFunction.GetParamNum() > 0) {
                            funcInfo = new FuncInfo();
                            foreach (var p in func.LowerOrderFunction.Params) {
                                string argName = p.GetId();
                                funcInfo.AddParamNameIndex(argName);
                            }
                        }
                    }
                    else {
                        return;
                    }
                }
                else {
                    return;
                }
            }
            if (null == funcInfo)
                funcInfo = new FuncInfo();
            foreach (Dsl.ISyntaxComponent comp in func.Params) {
                var exp = Load(comp);
                if (null != exp) {
                    funcInfo.Codes.Add(exp);
                }
            }
            funcInfo.IsAsync = isAsync;
            m_Funcs[id] = funcInfo;
        }
        public void LoadDsl(string func, Dsl.FunctionData dslFunc)
        {
            LoadDsl(func, null, dslFunc);
        }
        public void LoadDsl(string func, IList<string> paramNames, Dsl.FunctionData dslFunc)
        {
            LoadDsl(func, paramNames, dslFunc, false);
        }
        public void LoadDsl(string func, IList<string> paramNames, Dsl.FunctionData dslFunc, bool isAsync)
        {
            FuncInfo funcInfo = null;
            if (null != paramNames && paramNames.Count > 0) {
                funcInfo = new FuncInfo();
                funcInfo.BuildParamNameIndexes(paramNames);
            }
            if (null == funcInfo)
                funcInfo = new FuncInfo();
            LoadDsl(dslFunc.Params, funcInfo.Codes);
            funcInfo.IsAsync = isAsync;
            m_Funcs[func] = funcInfo;
        }
        public void LoadDsl(IList<Dsl.ISyntaxComponent> statements, List<IExpression> exps)
        {
            foreach (Dsl.ISyntaxComponent comp in statements) {
                if (comp.IsValid()) {
                    var exp = Load(comp);
                    if (null != exp) {
                        exps.Add(exp);
                    }
                }
            }
        }
        public bool TryGetFuncInfo(string func, out FuncInfo funcInfo)
        {
            return m_Funcs.TryGetValue(func, out funcInfo);
        }
        public void CheckFuncXrefs()
        {
            foreach (var func in m_FuncCalls) {
                if (!m_Funcs.ContainsKey(func.FuncName)) {
                    //error
                    Log("DslCalculator error, unknown func '{0}', {1} line {2}", func.FuncName, func.SyntaxComponent.ToScriptString(false, Dsl.DelimiterInfo.Default), func.SyntaxComponent.GetLine());
                }
            }
        }
        public List<BoxedValue> NewCalculatorValueList()
        {
            return m_Pool.Alloc();
        }
        public void RecycleCalculatorValueList(List<BoxedValue> list)
        {
            list.Clear();
            m_Pool.Recycle(list);
        }
        public BoxedValue Calc(string func)
        {
            var args = NewCalculatorValueList();
            var r = Calc(func, args);
            RecycleCalculatorValueList(args);
            return r;
        }
        public BoxedValue Calc(string func, BoxedValue arg1)
        {
            var args = NewCalculatorValueList();
            args.Add(arg1);
            var r = Calc(func, args);
            RecycleCalculatorValueList(args);
            return r;
        }
        public BoxedValue Calc(string func, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewCalculatorValueList();
            args.Add(arg1);
            args.Add(arg2);
            var r = Calc(func, args);
            RecycleCalculatorValueList(args);
            return r;
        }
        public BoxedValue Calc(string func, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewCalculatorValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            var r = Calc(func, args);
            RecycleCalculatorValueList(args);
            return r;
        }
        public BoxedValue Calc(string func, IList<BoxedValue> args)
        {
            BoxedValue ret = 0;
            FuncInfo funcInfo;
            if (m_Funcs.TryGetValue(func, out funcInfo)) {
                ret = Calc<object>(args, null, funcInfo);
            }
            else {
                //error
                Log("DslCalculator error, unknown func {0}", func);
            }
            return ret;
        }
        ///funcContext is recorded on the stack and its members can be accessed through custom apis (see parsing of no-argument variables in 'Load')
        public IEnumerator CalcAsync(string func, IList<BoxedValue> args, AsyncCalcResult asyncCalcResult)
        {
            FuncInfo funcInfo;
            if (m_Funcs.TryGetValue(func, out funcInfo)) {
                var enumerator = CalcAsync<object>(args, null, funcInfo, asyncCalcResult);
                while (enumerator.MoveNext()) {
                    yield return enumerator.Current;
                }
            }
            else {
                //error
                Log("DslCalculator error, unknown func {0}", func);
            }
        }
        public bool IsFuncAsync(string func)
        {
            FuncInfo funcInfo;
            if (m_Funcs.TryGetValue(func, out funcInfo)) {
                return funcInfo.IsAsync;
            }
            return false;
        }
        ///it's like args, but with a fixed parameter name and is mainly used to invoke snippets of code.
        public BoxedValue Calc<T>(T funcContext, FuncInfo funcInfo) where T : class
        {
            return Calc(null, funcContext, funcInfo);
        }
        public BoxedValue CalcInCurrentContext(IList<IExpression> exps)
        {
            BoxedValue ret = 0;
            for (int i = 0; i < exps.Count; ++i) {
                var exp = exps[i];
                try {
                    ret = exp.Calc();
                    if (m_RunState == RunStateEnum.Return) {
                        m_RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (m_RunState == RunStateEnum.Redirect) {
                        break;
                    }
                }
                catch (DirectoryNotFoundException ex5) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex5.Message, ex5.StackTrace);
                    OutputInnerException(ex5);
                }
                catch (FileNotFoundException ex4) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex4.Message, ex4.StackTrace);
                    OutputInnerException(ex4);
                }
                catch (IOException ex3) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex3.Message, ex3.StackTrace);
                    OutputInnerException(ex3);
                    ret = -1;
                }
                catch (UnauthorizedAccessException ex2) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex2.Message, ex2.StackTrace);
                    OutputInnerException(ex2);
                    ret = -1;
                }
                catch (NotSupportedException ex1) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex1.Message, ex1.StackTrace);
                    OutputInnerException(ex1);
                    ret = -1;
                }
                catch (Exception ex) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex.Message, ex.StackTrace);
                    OutputInnerException(ex);
                    ret = -1;
                    break;
                }
            }
            return ret;
        }
        private BoxedValue Calc<T>(IList<BoxedValue> args, T funcContext, FuncInfo funcInfo) where T : class
        {
            LocalStackPush(args, funcContext, funcInfo);
            SyncCalculationPush();
            try {
                return CalcInCurrentContext(funcInfo.Codes);
            }
            finally {
                SyncCalculationPop();
                LocalStackPop();
            }
        }
        public IEnumerator CalcInCurrentContextAsync(IList<IExpression> exps, AsyncCalcResult asyncCalcResult)
        {
            BoxedValue ret = 0;
            for (int i = 0; i < exps.Count; ++i) {
                var exp = exps[i];
                IEnumerator asyncEnumerator = null;
                AsyncCalcResult asyncResult = null;
                try {
                    if (exp.IsAsync) {
                        asyncResult = new AsyncCalcResult();
                        asyncEnumerator = exp.Calc(asyncResult);
                    }
                    else {
                        ret = exp.Calc();
                    }
                    if (m_RunState == RunStateEnum.Return) {
                        m_RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (m_RunState == RunStateEnum.Redirect) {
                        break;
                    }
                }
                catch (DirectoryNotFoundException ex5) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex5.Message, ex5.StackTrace);
                    OutputInnerException(ex5);
                }
                catch (FileNotFoundException ex4) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex4.Message, ex4.StackTrace);
                    OutputInnerException(ex4);
                }
                catch (IOException ex3) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex3.Message, ex3.StackTrace);
                    OutputInnerException(ex3);
                    ret = -1;
                }
                catch (UnauthorizedAccessException ex2) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex2.Message, ex2.StackTrace);
                    OutputInnerException(ex2);
                    ret = -1;
                }
                catch (NotSupportedException ex1) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex1.Message, ex1.StackTrace);
                    OutputInnerException(ex1);
                    ret = -1;
                }
                catch (Exception ex) {
                    Log("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex.Message, ex.StackTrace);
                    OutputInnerException(ex);
                    ret = -1;
                    break;
                }
                if (asyncEnumerator != null) {
                    while (asyncEnumerator.MoveNext()) {
                        yield return asyncEnumerator.Current;
                    }
                    if (exp.IsAsync) {
                        ret = asyncResult.Value;
                    }
                }
            }
            asyncCalcResult.Value = ret;
        }
        public IEnumerator CalcAsync<T>(IList<BoxedValue> args, T funcContext, FuncInfo funcInfo, AsyncCalcResult asyncCalcResult) where T : class
        {
            LocalStackPush(args, funcContext, funcInfo);
            try {
                var enumerator = CalcInCurrentContextAsync(funcInfo.Codes, asyncCalcResult);
                while (enumerator.MoveNext()) {
                    yield return enumerator.Current;
                }
            }
            finally {
                LocalStackPop();
            }
        }
        public RunStateEnum RunState
        {
            get { return m_RunState; }
            set { m_RunState = value; }
        }
        public bool IsInSyncCalculation => SyncCalculationStack.Count > 0;
        public void SyncCalculationPush()
        {
            SyncCalculationStack.Push(true);
        }
        public void SyncCalculationPop()
        {
            var stack = SyncCalculationStack;
            if (stack.Count > 0) {
                stack.Pop();
            }
        }
        public AsyncTaskRuntimeContext CreateAsyncContext()
        {
            var ctx = new AsyncTaskRuntimeContext();
            ctx.Stack = new Stack<StackInfo>();
            return ctx;
        }
        public void SetAsyncContext(AsyncTaskRuntimeContext ctx)
        {
            m_Stack = (Stack<StackInfo>)ctx.Stack;
            m_RunState = ctx.RunState;
        }
        public void SaveAsyncContext(AsyncTaskRuntimeContext ctx)
        {
            ctx.Stack = m_Stack;
            ctx.RunState = m_RunState;
        }
        public T GetFuncContext<T>() where T : class
        {
            if (m_Stack.Count > 0) {
                var stackInfo = m_Stack.Peek();
                return stackInfo.FuncContext as T;
            }
            return null;
        }
        public IList<BoxedValue> Arguments
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.Args;
            }
        }
        public bool TryGetVariable(string v, out BoxedValue result)
        {
            bool ret = false;
            if (v.Length > 0) {
                if (v[0] == '$') {
                    ret = TryGetLocalVariable(v, out result);
                }
                else {
                    ret = TryGetGlobalVariable(v, out result);
                }
            }
            else {
                result = BoxedValue.NullObject;
            }
            return ret;
        }
        public BoxedValue GetVariable(string v)
        {
            BoxedValue result = BoxedValue.NullObject;
            if (v.Length > 0) {
                if (v[0] == '$') {
                    result = GetLocalVariable(v);
                }
                else {
                    result = GetGlobalVariable(v);
                }
            }
            return result;
        }
        public void SetVariable(string v, BoxedValue val)
        {
            if (v.Length > 0) {
                if (v[0] == '$') {
                    SetLocalVariable(v, val);
                }
                else {
                    SetGlobalVariable(v, val);
                }
            }
        }
        public IExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            Dsl.FunctionData funcData = null;
            if (null != valueData) {
                int idType = valueData.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    string id = valueData.GetId();
                    var p = Create(id);
                    if (null != p) {
                        //Convert a parameterless name into a parameterless function call.
                        Dsl.FunctionData fd = new Dsl.FunctionData();
                        fd.Name.CopyFrom(valueData);
                        fd.SetParenthesesParamClass();
                        if (!p.Load(fd, this)) {
                            //error
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                        }
                        return p;
                    }
                    else if (id == "true" || id == "false") {
                        ConstGet constExp = new ConstGet();
                        constExp.Load(comp, this);
                        return constExp;
                    }
                    else if (id.Length > 0 && id[0] == '$') {
                        LocalVarGet varExp = new LocalVarGet();
                        varExp.Load(comp, this);
                        return varExp;
                    }
                    else {
                        GlobalVarGet varExp = new GlobalVarGet();
                        varExp.Load(comp, this);
                        return varExp;
                    }
                }
                else {
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                }
            }
            else {
                funcData = comp as Dsl.FunctionData;
                if (null != funcData) {
                    if (funcData.HaveParam()) {
                        var callData = funcData;
                        if (!callData.HaveId() && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES || callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                            switch (callData.GetParamClass()) {
                                case (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES: {
                                        int num = callData.GetParamNum();
                                        if (num == 1) {
                                            Dsl.ISyntaxComponent param = callData.GetParam(0);
                                            return Load(param);
                                        }
                                        else {
                                            TupleExp exp = new TupleExp();
                                            exp.Load(comp, this);
                                            return exp;
                                        }
                                    }
                                case (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                        ArrayExp exp = new ArrayExp();
                                        exp.Load(comp, this);
                                        return exp;
                                    }
                                default:
                                    return null;
                            }
                        }
                        else if (!callData.HaveParam()) {
                            //degeneration
                            valueData = callData.Name;
                            return Load(valueData);
                        }
                        else {
                            int paramClass = callData.GetParamClass();
                            string op = callData.GetId();
                            if (op == "`") {//backtick
                                int paramNum = callData.GetParamNum();
                                if (paramNum == 2) {
                                    Dsl.ISyntaxComponent param0 = callData.GetParam(0);
                                    Dsl.ISyntaxComponent param1 = callData.GetParam(1);
                                    if (param0 is Dsl.ValueData vd) {
                                        if (vd.GetId() == "return") {
                                            Dsl.FunctionData newCall = new Dsl.FunctionData();
                                            newCall.Name = new Dsl.ValueData("return", Dsl.ValueData.ID_TOKEN);
                                            newCall.SetParenthesesParamClass();
                                            newCall.Params.Add(param1);
                                            return Load(newCall);
                                        }
                                        else {
                                            //error
                                            Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                            return null;
                                        }
                                    }
                                    else if (param0 is Dsl.FunctionData fd) {
                                        if (fd.IsOperatorParamClass() || fd.IsTernaryOperatorParamClass()) {
                                            //error
                                            Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                            return null;
                                        }
                                        else {
                                            Dsl.FunctionData newCall = new Dsl.FunctionData();
                                            newCall.LowerOrderFunction = fd;
                                            newCall.SetStatementParamClass();
                                            newCall.Params.Add(param1);
                                            return Load(newCall);
                                        }
                                    }
                                    else {
                                        //error
                                        Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                        return null;
                                    }
                                }
                            }
                            else if (op == "=") {//assignment
                                Dsl.FunctionData innerCall = callData.GetParam(0) as Dsl.FunctionData;
                                if (null != innerCall) {
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                      innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                        //obj.property = val -> dotnetset(obj, property, val)
                                        //obj[property] = val -> collectionset(obj, property, val)
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD)
                                            newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                                        else
                                            newCall.Name = new Dsl.ValueData("collectionset", Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesesParamClass();
                                        if (innerCall.IsHighOrder) {
                                            newCall.Params.Add(innerCall.LowerOrderFunction);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            newCall.Params.Add(callData.GetParam(1));
                                        }
                                        else {
                                            newCall.Params.Add(innerCall.Name);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            newCall.Params.Add(callData.GetParam(1));
                                        }

                                        return Load(newCall);
                                    }
                                    else if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES && !innerCall.HaveId()) {
                                        //(a,b,c) = val;
                                        TupleSetExp tuple = new TupleSetExp();
                                        tuple.Load(comp, this);
                                        return tuple;
                                    }
                                }
                                IExpression exp = null;
                                string name = callData.GetParamId(0);
                                if (name.Length > 0 && name[0] == '$') {
                                    exp = new LocalVarSet();
                                }
                                else {
                                    exp = new GlobalVarSet();
                                }
                                if (null != exp) {
                                    exp.Load(comp, this);
                                }
                                else {
                                    //error
                                    Log("DslCalculator error, {0} line {1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                                }
                                return exp;
                            }
                            else {
                                if (callData.IsHighOrder) {
                                    Dsl.FunctionData innerCall = callData.LowerOrderFunction;
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES && (
                                        innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                        innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                                        //obj.member(a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                        //obj[member](a,b,...) -> collectioncall(obj,member,a,b,...)
                                        string apiName;
                                        string member = innerCall.GetParamId(0);
                                        if (member == "orderby" || member == "orderbydesc" || member == "where" || member == "top") {
                                            apiName = "linq";
                                        }
                                        else if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD) {
                                            apiName = "dotnetcall";
                                        }
                                        else {
                                            apiName = "collectioncall";
                                        }
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesesParamClass();
                                        if (innerCall.IsHighOrder) {
                                            newCall.Params.Add(innerCall.LowerOrderFunction);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                                Dsl.ISyntaxComponent p = callData.Params[i];
                                                newCall.Params.Add(p);
                                            }
                                        }
                                        else {
                                            newCall.Params.Add(innerCall.Name);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                                Dsl.ISyntaxComponent p = callData.Params[i];
                                                newCall.Params.Add(p);
                                            }
                                        }

                                        return Load(newCall);
                                    }
                                }
                                if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                  paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                    //obj.property or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                    //obj[property] -> collectionget(obj,property)
                                    Dsl.FunctionData newCall = new Dsl.FunctionData();
                                    if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD)
                                        newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                    else
                                        newCall.Name = new Dsl.ValueData("collectionget", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParenthesesParamClass();
                                    if (callData.IsHighOrder) {
                                        newCall.Params.Add(callData.LowerOrderFunction);
                                        newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                    }
                                    else {
                                        newCall.Params.Add(callData.Name);
                                        newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                    }

                                    return Load(newCall);
                                }
                            }
                        }
                    }
                    else {
                        if (funcData.HaveStatement()) {
                            if (!funcData.HaveId() && !funcData.IsHighOrder) {
                                HashtableExp exp = new HashtableExp();
                                exp.Load(comp, this);
                                return exp;
                            }
                        }
                        else if (!funcData.HaveExternScript()) {
                            //degeneration
                            valueData = funcData.Name;
                            return Load(valueData);
                        }
                    }
                }
            }
            IExpression ret = Create(comp.GetId());
            if (null == ret) {
                // We enable the function to be called before it is defined, so failover is done first
                if (null == OnLoadFailback || !OnLoadFailback(comp, this, out ret)) {
                    Dsl.StatementData stData = comp as Dsl.StatementData;
                    if (null != stData) {
                        //Convert command line syntax into function call syntax.
                        if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(stData, out var fd)) {
                            funcData = fd;
                        }
                    }
                    if (null != funcData && !funcData.IsHighOrder) {
                        var fc = new FunctionCall();
                        m_FuncCalls.Add(fc);
                        ret = fc;
                    }
                    else {
                        //error
                        Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                    }
                }
            }
            if (null != ret) {
                Dsl.StatementData stData = comp as Dsl.StatementData;
                if (null != stData) {
                    //Convert command line syntax into function call syntax.
                    if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(stData, out var fd)) {
                        if (!ret.Load(fd, this)) {
                            //error
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                        }
                        return ret;
                    }
                }
                if (!ret.Load(comp, this)) {
                    //error
                    Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                }
            }
            return ret;
        }
        public void Log(string fmt, params object[] args)
        {
            if (null != OnLog) {
                if (args.Length == 0)
                    OnLog(fmt);
                else
                    OnLog(string.Format(fmt, args));
            }
        }
        public void Log(object arg)
        {
            if (null != OnLog) {
                OnLog(string.Format("{0}", arg));
            }
        }
        private void LogError(string error)
        {
            error = DslSyntaxTransformer.TranslateDslSyntaxError(error);
            Log(error);
        }
        internal int AllocGlobalVariableIndex(string name)
        {
            int ix;
            if (null != OnTryGetVariable && OnTryGetVariable(name, out var val)) {
                ix = int.MaxValue;
            }
            else if (!m_NamedGlobalVariableIndexes.TryGetValue(name, out ix)) {
                ix = m_NamedGlobalVariableIndexes.Count;
                m_NamedGlobalVariableIndexes.Add(name, ix);
                m_GlobalVariables.Add(BoxedValue.NullObject);
            }
            return ix;
        }
        internal int AllocLocalVariableIndex(string name)
        {
            int ix;
            if (!LocalVariableIndexes.TryGetValue(name, out ix)) {
                ix = LocalVariableIndexes.Count;
                LocalVariableIndexes.Add(name, ix);
                LocalVariables.Add(BoxedValue.NullObject);
            }
            return ix;
        }
        internal int GetGlobalVariableIndex(string name)
        {
            int ix;
            if (!m_NamedGlobalVariableIndexes.TryGetValue(name, out ix)) {
                ix = int.MaxValue;
            }
            return ix;
        }
        internal int GetLocalVariableIndex(string name)
        {
            int ix;
            if (!LocalVariableIndexes.TryGetValue(name, out ix)) {
                ix = int.MaxValue;
            }
            return ix;
        }
        internal BoxedValue GetGlobalVaraibleByIndex(int ix)
        {
            return m_GlobalVariables[ix];
        }
        internal BoxedValue GetLocalVaraibleByIndex(int ix)
        {
            if (ix >= 0) {
                return LocalVariables[ix];
            }
            else {
                int argIx = -1 - ix;
                if (argIx >= 0 && argIx < Arguments.Count)
                    return Arguments[argIx];
                else
                    return BoxedValue.NullObject;
            }
        }
        internal void SetGlobalVaraibleByIndex(int ix, BoxedValue val)
        {
            m_GlobalVariables[ix] = val;
        }
        internal void SetLocalVaraibleByIndex(int ix, BoxedValue val)
        {
            if (ix >= 0) {
                LocalVariables[ix] = val;
            }
            else {
                int argIx = -1 - ix;
                if (argIx >= 0 && argIx < Arguments.Count)
                    Arguments[argIx] = val;
            }
        }

        private void LocalStackPush<T>(IList<BoxedValue> args, T funcContext, FuncInfo funcInfo) where T : class
        {
            var si = StackInfo.New();
            if (null != args) {
                si.Args.AddRange(args);
                for (int ix = args.Count; ix < funcInfo.ParamNum; ++ix) {
                    si.Args.Add(BoxedValue.NullObject);
                }
            }
            si.Init(funcInfo, funcContext);
            m_Stack.Push(si);
        }
        private void LocalStackPop()
        {
            var poped = m_Stack.Pop();
            if (null != poped) {
                poped.Recycle();
            }
        }

        private Dsl.ISyntaxComponent ConvertMember(Dsl.ISyntaxComponent p, int paramClass)
        {
            var pvd = p as Dsl.ValueData;
            if (null != pvd && pvd.IsId() && (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD
                || paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_POINTER)) {
                pvd.SetType(Dsl.ValueData.STRING_TOKEN);
                return pvd;
            }
            else {
                return p;
            }
        }

        private IExpression Create(string name)
        {
            IExpression ret = null;
            IExpressionFactory factory;
            if (null != m_ApiRegistry && m_ApiRegistry.TryGetFactory(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }
        private void OutputInnerException(Exception ex)
        {
            while (null != ex.InnerException) {
                ex = ex.InnerException;
                Log("\t=> exception:{0} stack:{1}", ex.Message, ex.StackTrace);
            }
        }

        private bool TryGetLocalVariable(string v, out BoxedValue result)
        {
            int index;
            if (LocalVariableIndexes.TryGetValue(v, out index)) {
                result = GetLocalVaraibleByIndex(index);
                return true;
            }
            else {
                result = BoxedValue.NullObject;
                return false;
            }
        }

        private BoxedValue GetLocalVariable(string v)
        {
            BoxedValue result;
            TryGetLocalVariable(v, out result);
            return result;
        }
        private void SetLocalVariable(string v, BoxedValue val)
        {
            int index;
            if (LocalVariableIndexes.TryGetValue(v, out index)) {
                SetLocalVaraibleByIndex(index, val);
            }
            else {
                int ix = LocalVariableIndexes.Count;
                LocalVariableIndexes.Add(v, ix);
                LocalVariables.Add(val);
            }
        }
        private Dictionary<string, int> LocalVariableIndexes
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.FuncInfo.LocalVarIndexes;
            }
        }
        private List<BoxedValue> LocalVariables
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.LocalVars;
            }
        }
        private Stack<bool> SyncCalculationStack
        {
            get {
                if (m_Stack.Count > 0) {
                    var stackInfo = m_Stack.Peek();
                    return stackInfo.SyncCalculationStack;
                }
                else {
                    return m_GlobalSyncCalculationStack;
                }
            }
        }

        private class StackInfo
        {
            internal FuncInfo FuncInfo = null;
            internal object FuncContext = null;
            internal List<BoxedValue> Args = new List<BoxedValue>();
            internal List<BoxedValue> LocalVars = new List<BoxedValue>();
            internal Stack<bool> SyncCalculationStack = new Stack<bool>();

            internal void Init(FuncInfo funcInfo, object funcContext)
            {
                FuncInfo = funcInfo;
                FuncContext = funcContext;
                LocalVars.Capacity = funcInfo.LocalVarIndexes.Count;
                for (int ix = 0; ix < funcInfo.LocalVarIndexes.Count; ++ix) {
                    LocalVars.Add(BoxedValue.NullObject);
                }
            }
            internal void Recycle()
            {
                FuncInfo = null;
                FuncContext = null;
                Args.Clear();
                LocalVars.Clear();
                SyncCalculationStack.Clear();

                s_Pool.Recycle(this);
            }
            internal static StackInfo New()
            {
                return s_Pool.Alloc();
            }
            private static SimpleObjectPool<StackInfo> s_Pool = new SimpleObjectPool<StackInfo>();
        }

        private RunStateEnum m_RunState = RunStateEnum.Normal;
        private Dictionary<string, FuncInfo> m_Funcs = new Dictionary<string, FuncInfo>();
        private Stack<StackInfo> m_Stack = new Stack<StackInfo>();
        private Stack<bool> m_GlobalSyncCalculationStack = new Stack<bool>();
        private Dictionary<string, int> m_NamedGlobalVariableIndexes = new Dictionary<string, int>();
        private List<BoxedValue> m_GlobalVariables = new List<BoxedValue>();
        private DslCalculatorApiRegistry m_ApiRegistry = null;
        private BoxedValueListPool m_Pool = new BoxedValueListPool(16);
        private List<FunctionCall> m_FuncCalls = new List<FunctionCall>();

        public static bool FileEchoOn
        {
            get { return s_FileEchoOn; }
            set { s_FileEchoOn = value; }
        }
        public static int CheckStartInterval
        {
            get { return s_CheckStartInterval; }
            set { s_CheckStartInterval = value; }
        }
        public static List<Task<int>> Tasks
        {
            get { return s_Tasks; }
        }
        public static void CleanupCompletedTasks()
        {
            for (int ix = s_Tasks.Count - 1; ix >= 0; --ix) {
                var task = s_Tasks[ix];
                if (task.IsCompleted) {
                    s_Tasks.RemoveAt(ix);
                    task.Dispose();
                }
            }
        }
        internal static int NewProcess(bool noWait, string fileName, string args, ProcessStartOption option, Stream istream, Stream ostream, IList<string> input, StringBuilder output, StringBuilder error, bool redirectToConsole, Encoding encoding)
        {
            if (noWait) {
                var task = Task.Run<int>(() => NewProcessTask(fileName, args, option, istream, ostream, input, output, error, redirectToConsole, encoding));
                s_Tasks.Add(task);
                while (task.Status == TaskStatus.Created || task.Status == TaskStatus.WaitingForActivation || task.Status == TaskStatus.WaitingToRun) {
                    Console.WriteLine("wait {0}[{1}] start", Path.GetFileName(fileName), task.Status);
                    task.Wait(s_CheckStartInterval);
                }
                return 0;
            }
            else {
                return NewProcessTask(fileName, args, option, istream, ostream, input, output, error, redirectToConsole, encoding);
            }
        }
        private static int NewProcessTask(string fileName, string args, ProcessStartOption option, Stream istream, Stream ostream, IList<string> input, StringBuilder output, StringBuilder error, bool redirectToConsole, Encoding encoding)
        {
            //Considering cross-platform compatibility, do not use specific process environment variables.
            try {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                var psi = p.StartInfo;
                psi.FileName = fileName;
                psi.Arguments = args;
                psi.UseShellExecute = option.UseShellExecute;
                if (null != option.Verb) {
                    psi.Verb = option.Verb;
                }
                if (null != option.UserName) {
                    psi.UserName = option.UserName;
                }
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    if (null != option.Domain) {
                        psi.Domain = option.Domain;
                    }
                    if (null != option.Password) {
                        unsafe {
                            fixed (char* pchar = option.Password.ToCharArray()) {
                                psi.Password = new System.Security.SecureString(pchar, option.Password.Length);
                            }
                        }
                    }
                    if (null != option.PasswordInClearText) {
                        psi.PasswordInClearText = option.PasswordInClearText;
                    }
                    psi.LoadUserProfile = option.LoadUserProfile;
                }
                psi.WindowStyle = option.WindowStyle;
                psi.CreateNoWindow = !option.CreateWindow;
                psi.ErrorDialog = option.ErrorDialog;
                psi.WorkingDirectory = option.WorkingDirectory;

                if (null == encoding) {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                        encoding = Encoding.GetEncoding(936);
                    }
                    else {
                        encoding = Encoding.UTF8;
                    }
                }

                if (null != istream || null != input) {
                    psi.RedirectStandardInput = true;
                }
                if (null != ostream || null != output || redirectToConsole) {
                    psi.RedirectStandardOutput = true;
                    psi.StandardOutputEncoding = encoding;
                    var tempStringBuilder = new StringBuilder();
                    p.OutputDataReceived += (sender, e) => OnOutputDataReceived(sender, e, ostream, output, redirectToConsole, encoding, tempStringBuilder);
                }
                if (null != error || redirectToConsole) {
                    psi.RedirectStandardError = true;
                    psi.StandardErrorEncoding = encoding;
                    var tempStringBuilder = new StringBuilder();
                    p.ErrorDataReceived += (sender, e) => OnErrorDataReceived(sender, e, ostream, error, redirectToConsole, encoding, tempStringBuilder);
                }
                if (p.Start()) {
                    if (psi.RedirectStandardInput) {
                        if (null != istream) {
                            istream.Seek(0, SeekOrigin.Begin);
                            using (var sr = new StreamReader(istream, encoding, true, 1024, true)) {
                                string line;
                                while ((line = sr.ReadLine()) != null) {
                                    p.StandardInput.WriteLine(line);
                                    p.StandardInput.Flush();
                                }
                            }
                            p.StandardInput.Close();
                        }
                        else if (null != input) {
                            foreach (var line in input) {
                                p.StandardInput.WriteLine(line);
                                p.StandardInput.Flush();
                            }
                            p.StandardInput.Close();
                        }
                    }
                    if (null != ostream) {
                        ostream.Seek(0, SeekOrigin.Begin);
                        ostream.SetLength(0);
                    }
                    if (psi.RedirectStandardOutput)
                        p.BeginOutputReadLine();
                    if (psi.RedirectStandardError)
                        p.BeginErrorReadLine();
                    p.WaitForExit();
                    if (psi.RedirectStandardOutput) {
                        p.CancelOutputRead();
                    }
                    if (psi.RedirectStandardError) {
                        p.CancelErrorRead();
                    }
                    int r = p.ExitCode;
                    p.Close();
                    return r;
                }
                else {
                    Console.WriteLine("process({0} {1}) failed.", fileName, args);
                    return -1;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("process({0} {1}) exception:{2} stack:{3}", fileName, args, ex.Message, ex.StackTrace);
                while (null != ex.InnerException) {
                    ex = ex.InnerException;
                    Console.WriteLine("\t=> exception:{0} stack:{1}", ex.Message, ex.StackTrace);
                }
                return -1;
            }
        }

        private static void OnOutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e, Stream ostream, StringBuilder output, bool redirectToConsole, Encoding encoding, StringBuilder temp)
        {
            var p = sender as System.Diagnostics.Process;
            if (p.StartInfo.RedirectStandardOutput && null != e.Data) {
                string str = e.Data;
                if (encoding != Encoding.UTF8) {
                    var bytes = encoding.GetBytes(str);
                    bytes = Encoding.Convert(encoding, Encoding.UTF8, bytes);
                    str = Encoding.UTF8.GetString(bytes);
                }

                temp.Length = 0;
                temp.AppendLine(str);
                var txt = temp.ToString();
                if (null != ostream) {
                    var bytes = Encoding.UTF8.GetBytes(txt);
                    ostream.Write(bytes, 0, bytes.Length);
                }
                if (null != output) {
                    output.Append(txt);
                }
                if (redirectToConsole) {
                    Console.Write(txt);
                }
            }
        }

        private static void OnErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e, Stream ostream, StringBuilder error, bool redirectToConsole, Encoding encoding, StringBuilder temp)
        {
            var p = sender as System.Diagnostics.Process;
            if (p.StartInfo.RedirectStandardError && null != e.Data) {
                string str = e.Data;
                if (encoding != Encoding.UTF8) {
                    var bytes = encoding.GetBytes(str);
                    bytes = Encoding.Convert(encoding, Encoding.UTF8, bytes);
                    str = Encoding.UTF8.GetString(bytes);
                }

                temp.Length = 0;
                temp.AppendLine(str);
                var txt = temp.ToString();
                if (null != ostream) {
                    var bytes = Encoding.UTF8.GetBytes(txt);
                    ostream.Write(bytes, 0, bytes.Length);
                }
                if (null != error) {
                    error.Append(txt);
                }
                if (redirectToConsole) {
                    Console.Write(txt);
                }
            }
        }

        internal sealed class ProcessStartOption
        {
            internal bool UseShellExecute = false;
            internal string Verb = null;
            internal string Domain = null;
            internal string UserName = null;
            internal string Password = null;
            internal string PasswordInClearText = null;
            internal bool LoadUserProfile = false;
            internal System.Diagnostics.ProcessWindowStyle WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            internal bool CreateWindow = false;
            internal bool ErrorDialog = false;
            internal string WorkingDirectory = Environment.CurrentDirectory;
        }

        private static List<Task<int>> s_Tasks = new List<Task<int>>();
        private static int s_CheckStartInterval = 500;
        private static bool s_FileEchoOn = false;
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
