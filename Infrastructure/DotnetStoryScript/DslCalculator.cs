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
    public interface IExpression
    {
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
        public BoxedValue Calc()
        {
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
        protected abstract BoxedValue DoCalc();

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
            if (type == "float" || str.IndexOfAny(s_FloatExponent) > 0) {
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
            else if (TryParseBool(str, out var bv)) {
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
    internal sealed class ArgsGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue ret = BoxedValue.FromObject(Calculator.Arguments);
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class ArgGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            var ix = m_ArgIndex.Calc().GetInt();
            var args = Calculator.Arguments;
            if (ix >= 0 && ix < args.Count) {
                ret = args[ix];
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ArgIndex = Calculator.Load(callData.GetParam(0));
            return true;
        }

        private IExpression m_ArgIndex;
    }
    internal sealed class ArgNumGet : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = Calculator.Arguments.Count;
            return BoxedValue.From(ret);
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
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
    internal sealed class AddExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsString || v2.IsString) {
                v = v1.ToString() + v2.ToString();
            }
            else if(v1.IsInteger && v2.IsInteger) {
                if(v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SubExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MulExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class DivExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class ModExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitAndExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() & v2.GetULong();
            }
            else {
                v = v1.GetLong() & v2.GetLong();
            }
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
    internal sealed class BitOrExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() | v2.GetULong();
            }
            else {
                v = v1.GetLong() | v2.GetLong();
            }
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
    internal sealed class BitXorExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger && v2.IsUnsignedInteger) {
                v = v1.GetULong() ^ v2.GetULong();
            }
            else {
                v = v1.GetLong() ^ v2.GetLong();
            }
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
    internal sealed class BitNotExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = ~v1.GetULong();
            }
            else {
                v = ~v1.GetLong();
            }
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LShiftExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = v1.GetULong() << v2.GetInt();
            }
            else {
                v = v1.GetLong() << v2.GetInt();
            }
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
    internal sealed class RShiftExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v;
            if (v1.IsUnsignedInteger) {
                v = v1.GetULong() >> v2.GetInt();
            }
            else {
                v = v1.GetLong() >> v2.GetInt();
            }
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
    internal sealed class MaxExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var opd1 = m_Op1.Calc();
            var opd2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MinExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var opd1 = m_Op1.Calc();
            var opd2 = m_Op2.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class AbsExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var opd = m_Op.Calc();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class SinExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Sin(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CosExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Cos(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Tan(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AsinExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Asin(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AcosExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Acos(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AtanExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Atan(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class Atan2Exp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = Math.Atan2(v1, v2);
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
    internal sealed class SinhExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Sinh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CoshExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Cosh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanhExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = Math.Tanh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class RndIntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long v1 = m_Op1.Calc().GetLong();
            long v2 = m_Op2.Calc().GetLong();
            BoxedValue v = (long)s_Random.Next((int)v1, (int)v2);
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

        private static Random s_Random = new Random();
    }
    internal sealed class RndFloatExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = s_Random.NextDouble() * (v2 - v1) + v1;
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

        private static Random s_Random = new Random();
    }
    internal sealed class PowExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = Math.Pow(v1, v2);
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
    internal sealed class SqrtExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Sqrt(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ExpExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Exp(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class Exp2Exp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Pow(2, v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LogExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            if (m_ArgNum == 1) {
                BoxedValue v = Math.Log(v1);
                return v;
            }
            else {
                double v2 = m_Op2.Calc().GetDouble();
                BoxedValue v = Math.Log(v1, v2);
                return v;
            }
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_ArgNum = exps.Count;
            m_Op1 = exps[0];
            if (m_ArgNum > 1) {
                m_Op2 = exps[1];
            }
            return true;
        }

        private int m_ArgNum;
        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class Log2Exp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Log(v1)/Math.Log(2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class Log10Exp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Log10(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FloorExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Floor(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CeilingExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Ceiling(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class RoundExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = Math.Round(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FloorToIntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = (int)Math.Floor(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CeilingToIntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = (int)Math.Ceiling(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class RoundToIntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = (int)Math.Round(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class BoolExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            bool v1 = m_Op1.Calc().GetBool();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class SByteExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            sbyte v1 = m_Op1.Calc().GetSByte();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ByteExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            byte v1 = m_Op1.Calc().GetByte();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CharExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            char v1 = m_Op1.Calc().GetChar();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ShortExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            short v1 = m_Op1.Calc().GetShort();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class UShortExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            ushort v1 = m_Op1.Calc().GetUShort();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class IntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int v1 = m_Op1.Calc().GetInt();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class UIntExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            uint v1 = m_Op1.Calc().GetUInt();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LongExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long v1 = m_Op1.Calc().GetLong();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ULongExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            ulong v1 = m_Op1.Calc().GetULong();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FloatExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float v1 = m_Op1.Calc().GetFloat();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class DoubleExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class DecimalExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            decimal v1 = m_Op1.Calc().GetDecimal();
            BoxedValue v = v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ItofExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int v1 = m_Op1.Calc().GetInt();
            float v2 = 0;
            unsafe {
                v2 = *(float*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FtoiExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float v1 = m_Op1.Calc().GetFloat();
            int v2 = 0;
            unsafe {
                v2 = *(int*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class UtofExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            uint v1 = m_Op1.Calc().GetUInt();
            float v2 = 0;
            unsafe {
                v2 = *(float*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FtouExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float v1 = m_Op1.Calc().GetFloat();
            uint v2 = 0;
            unsafe {
                v2 = *(uint*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LtodExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long v1 = m_Op1.Calc().GetLong();
            double v2 = 0;
            unsafe {
                v2 = *(double*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class DtolExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            long v2 = 0;
            unsafe {
                v2 = *(long*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class UtodExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            ulong v1 = m_Op1.Calc().GetULong();
            double v2 = 0;
            unsafe {
                v2 = *(double*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class DtouExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            ulong v2 = 0;
            unsafe {
                v2 = *(ulong*)&v1;
            }
            BoxedValue v = v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LerpExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double a = m_Op1.Calc().GetDouble();
            double b = m_Op2.Calc().GetDouble();
            double t = m_Op3.Calc().GetDouble();
            BoxedValue v;
            v = a + (b - a) * ClampExp.Clamp01(t);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpUnclampedExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double a = m_Op1.Calc().GetDouble();
            double b = m_Op2.Calc().GetDouble();
            double t = m_Op3.Calc().GetDouble();
            BoxedValue v = a + (b - a) * t;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpAngleExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double a = m_Op1.Calc().GetDouble();
            double b = m_Op2.Calc().GetDouble();
            double t = m_Op3.Calc().GetDouble();
            double num = Repeat(b - a, 360.0);
            if (num > 180f) {
                num -= 360f;
            }
            BoxedValue v = a + num * ClampExp.Clamp01(t);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;

        public static double Repeat(double t, double length)
        {
            return ClampExp.Clamp(t - Math.Floor(t / length) * length, 0f, length);
        }
    }
    internal sealed class SmoothStepExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double from = m_Op1.Calc().GetDouble();
            double to = m_Op2.Calc().GetDouble();
            double t = m_Op3.Calc().GetDouble();
            t = ClampExp.Clamp01(t);
            t = -2.0 * t * t * t + 3.0 * t * t;
            BoxedValue v = to * t + from * (1.0 - t);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class Clamp01Exp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op.Calc().GetDouble();
            BoxedValue v = ClampExp.Clamp01(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class ClampExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            double v3 = m_Op3.Calc().GetDouble();
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
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;

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
    internal sealed class ApproximatelyExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float v1 = m_Op1.Calc().GetFloat();
            float v2 = m_Op2.Calc().GetFloat();
            BoxedValue v = Approximately(v1, v2) ? 1 : 0;
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

        public static bool Approximately(double a, double b)
        {
            return Math.Abs(b - a) < Math.Max(1E-06 * Math.Max(Math.Abs(a), Math.Abs(b)), double.Epsilon * 8.0);
        }
    }
    internal sealed class IsPowerOfTwoExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int v1 = m_Op1.Calc().GetInt();
            int v = IsPowerOfTwo(v1) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;

        public bool IsPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n)) == v;
        }
    }
    internal sealed class ClosestPowerOfTwoExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int v1 = m_Op1.Calc().GetInt();
            int v = ClosestPowerOfTwo(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;

        public int ClosestPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n));
        }
    }
    internal sealed class NextPowerOfTwoExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int v1 = m_Op1.Calc().GetInt();
            int v = NextPowerOfTwo(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;

        public int NextPowerOfTwo(int v)
        {
            int n = (int)Math.Round(Math.Log(v) / Math.Log(2));
            return (int)Math.Round(Math.Pow(2, n + 1));
        }
    }
    internal sealed class DistExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float x1 = (float)m_Op1.Calc().GetDouble();
            float y1 = (float)m_Op2.Calc().GetDouble();
            float x2 = (float)m_Op3.Calc().GetDouble();
            float y2 = (float)m_Op4.Calc().GetDouble();
            BoxedValue v = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class DistSqrExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            float x1 = (float)m_Op1.Calc().GetDouble();
            float y1 = (float)m_Op2.Calc().GetDouble();
            float x2 = (float)m_Op3.Calc().GetDouble();
            float y2 = (float)m_Op4.Calc().GetDouble();
            BoxedValue v = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class GreatExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = v1 > v2 ? 1 : 0;
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
    internal sealed class GreatEqualExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = v1 >= v2 ? 1 : 0;
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
    internal sealed class LessExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = v1 < v2 ? 1 : 0;
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
    internal sealed class LessEqualExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            double v1 = m_Op1.Calc().GetDouble();
            double v2 = m_Op2.Calc().GetDouble();
            BoxedValue v = v1 <= v2 ? 1 : 0;
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
    internal sealed class EqualExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v = v1.ToString() == v2.ToString() ? 1 : 0;
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
    internal sealed class NotEqualExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var v1 = m_Op1.Calc();
            var v2 = m_Op2.Calc();
            BoxedValue v = v1.ToString() != v2.ToString() ? 1 : 0;
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
    internal sealed class NotExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            long val = m_Op.Calc().GetLong();
            BoxedValue v = val == 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
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
                Calculator.Log("DslCalculator error, {0} line {1}", statementData.ToScriptString(false), statementData.GetLine());
            }
            return true;
        }

        private IExpression m_Op1 = null;
        private IExpression m_Op2 = null;
        private IExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    var condVal = clause.Condition.Calc();
                    if (condVal.GetLong() != 0) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            v = clause.Expressions[index].Calc();
                            if (Calculator.RunState != RunStateEnum.Normal) {
                                return v;
                            }
                        }
                        break;
                    }
                }
                else if (ix == m_Clauses.Count - 1) {
                    for (int index = 0; index < clause.Expressions.Count; ++index) {
                        v = clause.Expressions[index].Calc();
                        if (Calculator.RunState != RunStateEnum.Normal) {
                            return v;
                        }
                    }
                    break;
                }
            }
            return v;
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
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false), funcData.GetLine());
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'if(exp) func(args);'.
            int funcNum = statementData.GetFunctionNum();
            if (funcNum == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "if" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    IfExp.Clause item = new IfExp.Clause();
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);
                    item.Expressions.Add(subExp);
                    m_Clauses.Add(item);
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
                        Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
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
                        Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
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
                    Calculator.Log("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                }
            }
            return true;
        }

        private sealed class Clause
        {
            internal IExpression Condition;
            internal List<IExpression> Expressions = new List<IExpression>();
        }

        private List<Clause> m_Clauses = new List<Clause>();
    }
    internal sealed class WhileExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            for (; ; ) {
                var condVal = m_Condition.Calc();
                if (condVal.GetLong() != 0) {
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc();
                        if (Calculator.RunState == RunStateEnum.Continue) {
                            Calculator.RunState = RunStateEnum.Normal;
                            break;
                        }
                        else if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Break)
                                Calculator.RunState = RunStateEnum.Normal;
                            return v;
                        }
                    }
                }
                else {
                    break;
                }
            }
            return v;
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
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false), funcData.GetLine());
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'while(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "while" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.GetParam(0);
                        m_Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);
                    m_Expressions.Add(subExp);
                    return true;
                }
            }
            return false;
        }

        private IExpression m_Condition;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            var count = m_Count.Calc();
            long ct = count.GetLong();
            for (int i = 0; i < ct; ++i) {
                Calculator.SetVariable("$$", i);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc();
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        return v;
                    }
                }
            }
            return v;
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
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false), funcData.GetLine());
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'loop(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "loop" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.GetParam(0);
                        m_Count = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);
                    m_Expressions.Add(subExp);
                    return true;
                }
            }
            return false;
        }

        private IExpression m_Count;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopListExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            var list = m_List.Calc();
            IEnumerable obj = list.As<IEnumerable>();
            if (null != obj) {
                IEnumerator enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    var val = BoxedValue.FromObject(enumer.Current);
                    Calculator.SetVariable("$$", val);
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc();
                        if (Calculator.RunState == RunStateEnum.Continue) {
                            Calculator.RunState = RunStateEnum.Normal;
                            break;
                        }
                        else if (Calculator.RunState != RunStateEnum.Normal) {
                            if (Calculator.RunState == RunStateEnum.Break)
                                Calculator.RunState = RunStateEnum.Normal;
                            return v;
                        }
                    }
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
                Calculator.Log("DslCalculator error, {0} line {1}", funcData.ToScriptString(false), funcData.GetLine());
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'looplist(exp) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "looplist" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.GetParam(0);
                        m_List = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);
                    m_Expressions.Add(subExp);
                    return true;
                }
            }
            return false;
        }

        private IExpression m_List;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ForeachExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            List<object> list = new List<object>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                object val = m_Elements[ix].Calc().GetObject();
                list.Add(val);
            }
            IEnumerator enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                var val = BoxedValue.FromObject(enumer.Current);
                Calculator.SetVariable("$$", val);
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc();
                    if (Calculator.RunState == RunStateEnum.Continue) {
                        Calculator.RunState = RunStateEnum.Normal;
                        break;
                    }
                    else if (Calculator.RunState != RunStateEnum.Normal) {
                        if (Calculator.RunState == RunStateEnum.Break)
                            Calculator.RunState = RunStateEnum.Normal;
                        return v;
                    }
                }
            }
            return v;
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
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //the handling of the simple syntax 'foreach(exp1,exp2,...) func(args);'
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "foreach" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    int num = first.GetParamNum();
                    if (num > 0) {
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent exp = first.GetParam(ix);
                            m_Elements.Add(Calculator.Load(exp));
                        }
                    }
                    else {
                        //error
                        Calculator.Log("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);
                    m_Expressions.Add(subExp);
                    return true;
                }
            }
            return false;
        }

        private List<IExpression> m_Elements = new List<IExpression>();
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ParenthesisExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc();
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class FormatExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue v = 0;
            string fmt = string.Empty;
            ArrayList al = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc();
                if (ix == 0)
                    fmt = v.AsString;
                else
                    al.Add(v.GetObject());
            }
            v = string.Format(fmt, al.ToArray());
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class GetTypeAssemblyNameExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().AssemblyQualifiedName;
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class GetTypeFullNameExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().FullName;
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class GetTypeNameExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().Name;
                }
                catch (Exception ex) {
                    Calculator.Log("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class GetTypeExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                string type = m_Expressions[0].Calc().AsString;
                try {
                    var r = Type.GetType(type);
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ChangeTypeExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 2) {
                var obj = m_Expressions[0].Calc();
                string type = m_Expressions[1].Calc().AsString;
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
                            Type t = Type.GetType(type);
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
                            Type t = Type.GetType(type);
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ParseEnumExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 2) {
                string type = m_Expressions[0].Calc().AsString;
                string val = m_Expressions[1].Calc().AsString;
                try {
                    Type t = Type.GetType(type);
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetCallExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            string method = null;
            List<BoxedValue> args = null;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
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
                    if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
        private int m_DispId = -1;
    }
    internal sealed class DotnetSetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            string method = null;
            BoxedValue argv = BoxedValue.NullObject;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
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
                        dict[method] = _args[0];
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
        private int m_DispId = -1;
    }
    internal sealed class DotnetGetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            string method = null;
            ArrayList arglist = null;
            IObjectDispatch disp = null;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                    disp = obj as IObjectDispatch;
                }
                else if (ix == 1) {
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
                    if (null != dict && dict.Contains(method)) {
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
        private int m_DispId = -1;
    }
    internal sealed class CollectionCallExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            object methodObj = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    methodObj = v.GetObject();
                }
                else {
                    arglist.Add(v.GetObject());
                }
            }
            object[] _args = arglist.ToArray();
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict.Contains(methodObj)) {
                    var d = dict[methodObj] as Delegate;
                    if (null != d) {
                        ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                    }
                }
                else {
                    IList list = obj as IList;
                    if (null != list && methodObj is int) {
                        int index = (int)methodObj;
                        if (index >= 0 && index < list.Count) {
                            var d = list[index] as Delegate;
                            if (null != d) {
                                ret = BoxedValue.FromObject(d.DynamicInvoke(_args));
                            }
                        }
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && methodObj is int) {
                            int index = (int)methodObj;
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class CollectionSetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            object methodObj = null;
            object arg = null;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    methodObj = v.GetObject();
                }
                else {
                    arg = v.GetObject();
                    break;
                }
            }
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict.Contains(methodObj)) {
                    dict[methodObj] = arg;
                }
                else {
                    IList list = obj as IList;
                    if (null != list && methodObj is int) {
                        int index = (int)methodObj;
                        if (index >= 0 && index < list.Count) {
                            list[index] = arg;
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class CollectionGetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            object obj = null;
            object methodObj = null;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v.GetObject();
                }
                else if (ix == 1) {
                    methodObj = v.GetObject();
                }
                else {
                    break;
                }
            }
            if (null != obj && null != methodObj) {
                IDictionary dict = obj as IDictionary;
                if (null != dict && dict.Contains(methodObj)) {
                    ret = BoxedValue.FromObject(dict[methodObj]);
                }
                else {
                    IList list = obj as IList;
                    if (null != list && methodObj is int) {
                        int index = (int)methodObj;
                        if (index >= 0 && index < list.Count) {
                            var d = list[index];
                            ret = BoxedValue.FromObject(d);
                        }
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && methodObj is int) {
                            int index = (int)methodObj;
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
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
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);
                        results.Add(val);
                    }
                    results.Sort((object o1, object o2) => {
                        Calculator.SetVariable("$$", BoxedValue.FromObject(o1));
                        var r1 = BoxedValue.NullObject;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r1 = m_Expressions[index].Calc();
                        }
                        Calculator.SetVariable("$$", BoxedValue.FromObject(o2));
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
                    List<object> results = new List<object>();
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
                    List<object> results = new List<object>();
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
    internal sealed class IsNullExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                ret = obj.IsNullObject;
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class NullExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class EqualsNullExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var ret = BoxedValue.NullObject;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                ret = object.Equals(null, obj.ObjectVal);
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetLoadExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
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
    internal sealed class SubstringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                string str = operands[0].GetString();
                if (null != str) {
                    int start = 0;
                    int len = str.Length;
                    if (operands.Count >= 2) {
                        start = operands[1].GetInt();
                        len -= start;
                    }
                    if (operands.Count >= 3) {
                        len = operands[2].GetInt();
                    }
                    r = str.Substring(start, len);
                }
            }
            return r;
        }
    }
    internal sealed class NewStringBuilderExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 0) {
                r = BoxedValue.FromObject(new StringBuilder());
            }
            return r;
        }
    }
    internal sealed class AppendFormatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var sb = operands[0].As<StringBuilder>();
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i].AsString;
                    else
                        al.Add(operands[i].GetObject());
                }
                if (null != sb && !string.IsNullOrEmpty(fmt)) {
                    sb.AppendFormat(fmt, al.ToArray());
                    r = BoxedValue.FromObject(sb);
                }
            }
            return r;
        }
    }
    internal sealed class AppendFormatLineExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var sb = operands[0].As<StringBuilder>();
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i].AsString;
                    else
                        al.Add(operands[i].GetObject());
                }
                if (null != sb) {
                    if (string.IsNullOrEmpty(fmt)) {
                        sb.AppendLine();
                    }
                    else {
                        sb.AppendFormat(fmt, al.ToArray());
                        sb.AppendLine();
                    }
                    r = BoxedValue.FromObject(sb);
                }
            }
            return r;
        }
    }
    internal sealed class StringBuilderToStringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var sb = operands[0].As<StringBuilder>();
                if (null != sb) {
                    r = sb.ToString();
                }
            }
            return r;
        }
    }
    internal sealed class StringJoinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var sep = operands[0].AsString;
                var list = operands[1].As<IList>();
                if (null != sep && null != list) {
                    string[] strs = new string[list.Count];
                    for (int i = 0; i < list.Count; ++i) {
                        strs[i] = list[i].ToString();
                    }
                    r = string.Join(sep, strs);
                }
            }
            return r;
        }
    }
    internal sealed class StringSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var str = operands[0].AsString;
                var seps = operands[1].As<IList>();
                if (null != str && null != seps) {
                    char[] cs = new char[seps.Count];
                    for (int i = 0; i < seps.Count; ++i) {
                        string sep = seps[i].ToString();
                        if (sep.Length > 0) {
                            cs[i] = sep[0];
                        }
                        else {
                            cs[i] = '\0';
                        }
                    }
                    r = BoxedValue.FromObject(str.Split(cs));
                }
            }
            return r;
        }
    }
    internal sealed class StringTrimExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.Trim();
            }
            return r;
        }
    }
    internal sealed class StringTrimStartExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.TrimStart();
            }
            return r;
        }
    }
    internal sealed class StringTrimEndExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.TrimEnd();
            }
            return r;
        }
    }
    internal sealed class StringToLowerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.ToLower();
            }
            return r;
        }
    }
    internal sealed class StringToUpperExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.ToUpper();
            }
            return r;
        }
    }
    internal sealed class StringReplaceExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var str = operands[0].AsString;
                var key = operands[1].AsString;
                var val = operands[2].AsString;
                r = str.Replace(key, val);
            }
            return r;
        }
    }
    internal sealed class StringReplaceCharExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var str = operands[0].AsString;
                var key = operands[1].AsString;
                var val = operands[2].AsString;
                if (null != str && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                    r = str.Replace(key[0], val[0]);
                }
            }
            return r;
        }
    }
    internal sealed class MakeStringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < operands.Count; ++i) {
                var v = operands[i];
                var str = v.AsString;
                if (null != str) {
                    char c = '\0';
                    if (str.Length > 0) {
                        c = str[0];
                    }
                    chars.Add(c);
                }
                else {
                    char c = operands[i].GetChar();
                    chars.Add(c);
                }
            }
            return new String(chars.ToArray());
        }
    }
    internal sealed class StringContainsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                r = true;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key) && !str.Contains(key)) {
                                return false;
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key) && !str.Contains(key)) {
                            return false;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringNotContainsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                r = true;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key) && str.Contains(key)) {
                                return false;
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key) && str.Contains(key)) {
                            return false;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool r = false;
            if (operands.Count >= 2) {
                r = true;
                string str = operands[0].AsString;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key)) {
                                if (str.Contains(key)) {
                                    return true;
                                }
                                else {
                                    r = false;
                                }
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key)) {
                            if (str.Contains(key)) {
                                return true;
                            }
                            else {
                                r = false;
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringNotContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool r = false;
            if (operands.Count >= 2) {
                r = true;
                string str = operands[0].AsString;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key)) {
                                if (!str.Contains(key)) {
                                    return true;
                                }
                                else {
                                    r = false;
                                }
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key)) {
                            if (!str.Contains(key)) {
                                return true;
                            }
                            else {
                                r = false;
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                int v;
                if (int.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                uint v;
                if (uint.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                long v;
                if (long.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                ulong v;
                if (ulong.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2FloatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                float v;
                if (float.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2DoubleExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                double v;
                if (double.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                int v;
                if (int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                uint v;
                if (uint.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                long v;
                if (long.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                ulong v;
                if (ulong.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class DatetimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var fmt = operands[0].AsString;
                r = DateTime.Now.ToString(fmt);
            }
            else {
                r = DateTime.Now.ToString();
            }
            return r;
        }
    }
    internal sealed class LongDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.FromObject(DateTime.Now.ToLongDateString());
            return r;
        }
    }
    internal sealed class LongTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.FromObject(DateTime.Now.ToShortTimeString());
            return r;
        }
    }
    internal sealed class IsNullOrEmptyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = string.IsNullOrEmpty(str);
            }
            return r;
        }
    }
    internal sealed class ArrayExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            object[] r = new object[operands.Count];
            for (int i = 0; i < operands.Count; ++i) {
                r[i] = operands[i].GetObject();
            }
            return BoxedValue.FromObject(r);
        }
    }
    internal sealed class ToArrayExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0];
                IEnumerable obj = list.As<IEnumerable>();
                if (null != obj) {
                    ArrayList al = new ArrayList();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);
                        al.Add(val);
                    }
                    r = BoxedValue.FromObject(al.ToArray());
                }
            }
            return r;
        }
    }
    internal sealed class ListSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0].As<IList>();
                if (null != list) {
                    r = list.Count;
                }
            }
            return r;
        }
    }
    internal sealed class ListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            ArrayList al = new ArrayList();
            for (int i = 0; i < operands.Count; ++i) {
                al.Add(operands[i].GetObject());
            }
            r = al;
            return r;
        }
    }
    internal sealed class ListGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                var defVal = BoxedValue.NullObject;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        r = BoxedValue.FromObject(list[index]);
                    }
                    else {
                        r = defVal;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class ListSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                var val = operands[2];
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        list[index] = val.GetObject();
                    }
                }
            }
            return r;
        }
    }
    internal sealed class ListIndexOfExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                object val = operands[1];
                if (null != list) {
                    r = list.IndexOf(val);
                }
            }
            return r;
        }
    }
    internal sealed class ListAddExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                object val = operands[1];
                if (null != list) {
                    list.Add(val);
                }
            }
            return r;
        }
    }
    internal sealed class ListRemoveExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                object val = operands[1];
                if (null != list) {
                    list.Remove(val);
                }
            }
            return r;
        }
    }
    internal sealed class ListInsertExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                object val = operands[2].GetObject();
                if (null != list) {
                    list.Insert(index, val);
                }
            }
            return r;
        }
    }
    internal sealed class ListRemoveAtExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                if (null != list) {
                    list.RemoveAt(index);
                }
            }
            return r;
        }
    }
    internal sealed class ListClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0].As<IList>();
                if (null != list) {
                    list.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class ListSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var enumer = operands[0].As<IEnumerable>();
                var ct = operands[1].GetInt();
                if (null != enumer) {
                    var e = enumer.GetEnumerator();
                    if (null != e) {
                        ArrayList al = new ArrayList();
                        ArrayList arr = new ArrayList();
                        int ix = 0;
                        while (e.MoveNext()) {
                            if (ix < ct) {
                                arr.Add(e.Current);
                                ++ix;
                            }
                            if (ix >= ct) {
                                al.Add(arr);
                                arr = new ArrayList();
                                ix = 0;
                            }
                        }
                        if (arr.Count > 0) {
                            al.Add(arr);
                        }
                        r = al;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    r = dict.Count;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue r = BoxedValue.NullObject;
            Hashtable dict = new Hashtable();
            for (int i = 0; i < m_Expressions.Count - 1; i += 2) {
                var key = m_Expressions[i].Calc().GetObject();
                var val = m_Expressions[i + 1].Calc().GetObject();
                dict.Add(key, val);
            }
            r = BoxedValue.FromObject(dict);
            return r;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            for (int i = 0; i < funcData.GetParamNum(); ++i) {
                Dsl.FunctionData callData = funcData.GetParam(i) as Dsl.FunctionData;
                if (null != callData && callData.GetParamNum() == 2) {
                    var expKey = Calculator.Load(callData.GetParam(0));
                    m_Expressions.Add(expKey);
                    var expVal = Calculator.Load(callData.GetParam(1));
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class HashtableGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var index = operands[1].GetObject();
                var defVal = BoxedValue.NullObject;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != dict && dict.Contains(index)) {
                    r = BoxedValue.FromObject(dict[index]);
                }
                else {
                    r = defVal;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var dict = operands[0].As<IDictionary>();
                var index = operands[1].GetObject();
                object val = operands[2].GetObject();
                if (null != dict) {
                    dict[index] = val;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableAddExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var dict = operands[0].As<IDictionary>();
                object key = operands[1];
                object val = operands[2];
                if (null != dict && null != key) {
                    dict.Add(key, val);
                }
            }
            return r;
        }
    }
    internal sealed class HashtableRemoveExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                object key = operands[1];
                if (null != dict && null != key) {
                    dict.Remove(key);
                }
            }
            return r;
        }
    }
    internal sealed class HashtableClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    dict.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class HashtableKeysExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    var list = new ArrayList();
                    list.AddRange(dict.Keys);
                    r = list;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableValuesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    var list = new ArrayList();
                    list.AddRange(dict.Values);
                    r = list;
                }
            }
            return r;
        }
    }
    internal sealed class ListHashtableExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    var list = new ArrayList();
                    foreach (var pair in dict) {
                        list.Add(pair);
                    }
                    r = list;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var ct = operands[1].GetInt();
                if (null != dict) {
                    var e = dict.GetEnumerator();
                    if (null != e) {
                        ArrayList al = new ArrayList();
                        Hashtable ht = new Hashtable();
                        int ix = 0;
                        while (e.MoveNext()) {
                            if (ix < ct) {
                                ht.Add(e.Key, e.Value);
                                ++ix;
                            }
                            if (ix >= ct) {
                                al.Add(ht);
                                ht = new Hashtable();
                                ix = 0;
                            }
                        }
                        if (ht.Count > 0) {
                            al.Add(ht);
                        }
                        r = al;
                    }
                }
            }
            return r;
        }
    }
    //The stack and queue share the same peek function.
    internal sealed class PeekExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<object>>();
                var queue = operands[0].As<Queue<object>>();
                if (null != stack) {
                    r = BoxedValue.FromObject(stack.Peek());
                }
                else if (null != queue) {
                    r = BoxedValue.FromObject(queue.Peek());
                }
            }
            return r;
        }
    }
    internal sealed class StackSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int r = 0;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<object>>();
                if (null != stack) {
                    r = stack.Count;
                }
            }
            return r;
        }
    }
    internal sealed class StackExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            var stack = new Stack<object>();
            for (int i = 0; i < operands.Count; ++i) {
                stack.Push(operands[i].GetObject());
            }
            r = BoxedValue.FromObject(stack);
            return r;
        }
    }
    internal sealed class PushExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var stack = operands[0].As<Stack<object>>();
                var val = operands[1];
                if (null != stack) {
                    stack.Push(val);
                }
            }
            return r;
        }
    }
    internal sealed class PopExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<object>>();
                if (null != stack) {
                    r = BoxedValue.FromObject(stack.Pop());
                }
            }
            return r;
        }
    }
    internal sealed class StackClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<object>>();
                if (null != stack) {
                    stack.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class QueueSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int r = 0;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<object>>();
                if (null != queue) {
                    r = queue.Count;
                }
            }
            return r;
        }
    }
    internal sealed class QueueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            var queue = new Queue<object>();
            for (int i = 0; i < operands.Count; ++i) {
                queue.Enqueue(operands[i].GetObject());
            }
            r = BoxedValue.FromObject(queue);
            return r;
        }
    }
    internal sealed class EnqueueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var queue = operands[0].As<Queue<object>>();
                var val = operands[1];
                if (null != queue) {
                    queue.Enqueue(val);
                }
            }
            return r;
        }
    }
    internal sealed class DequeueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<object>>();
                if (null != queue) {
                    r = BoxedValue.FromObject(queue.Dequeue());
                }
            }
            return r;
        }
    }
    internal sealed class QueueClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<object>>();
                if (null != queue) {
                    queue.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class SetEnvironmentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var key = operands[0].AsString;
                var val = operands[1].AsString;
                val = Environment.ExpandEnvironmentVariables(val);
                Environment.SetEnvironmentVariable(key, val);
            }
            return ret;
        }
    }
    internal sealed class GetEnvironmentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0].AsString;
                return Environment.GetEnvironmentVariable(key);
            }
            return ret;
        }
    }
    internal sealed class ExpandEnvironmentsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0].AsString;
                return Environment.ExpandEnvironmentVariables(key);
            }
            return ret;
        }
    }
    internal sealed class EnvironmentsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(Environment.GetEnvironmentVariables());
        }
    }
    internal sealed class SetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                Environment.CurrentDirectory = Environment.ExpandEnvironmentVariables(dir);
                ret = dir;
            }
            return ret;
        }
    }
    internal sealed class GetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Environment.CurrentDirectory;
        }
    }
    internal sealed class CommandLineExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Environment.CommandLine;
        }
    }
    internal sealed class CommandLineArgsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                string name = operands[0].AsString;
                if (!string.IsNullOrEmpty(name)) {
                    string[] args = System.Environment.GetCommandLineArgs();
                    int suffixIndex = Array.FindIndex(args, item => item == name);
                    if (suffixIndex != -1 && suffixIndex < args.Length - 1) {
                        return args[suffixIndex + 1];
                    }
                }
                return string.Empty;
            }
            else {
                return BoxedValue.FromObject(Environment.GetCommandLineArgs());
            }
        }
    }
    internal sealed class OsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Environment.OSVersion.VersionString;
        }
    }
    internal sealed class OsPlatformExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Environment.OSVersion.Platform.ToString();
        }
    }
    internal sealed class OsVersionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Environment.OSVersion.Version.ToString();
        }
    }
    internal sealed class GetFullPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetFullPath(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetPathRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetPathRoot(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetRandomFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Path.GetRandomFileName();
        }
    }
    internal sealed class GetTempFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Path.GetTempFileName();
        }
    }
    internal sealed class GetTempPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return Path.GetTempPath();
        }
    }
    internal sealed class HasExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.HasExtension(path);
                }
            }
            return ret;
        }
    }
    internal sealed class IsPathRootedExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.IsPathRooted(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileName(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetFileNameWithoutExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileNameWithoutExtension(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetExtension(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetDirectoryNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetDirectoryName(path);
                }
            }
            return r;
        }
    }
    internal sealed class CombinePathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var path1 = operands[0].AsString;
                var path2 = operands[1].AsString;
                if (null != path1 && null != path2) {
                    path1 = Environment.ExpandEnvironmentVariables(path1);
                    path2 = Environment.ExpandEnvironmentVariables(path2);
                    r = Path.Combine(path1, path2);
                }
            }
            return r;
        }
    }
    internal sealed class ChangeExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var path = operands[0].AsString;
                var ext = operands[1].AsString;
                if (null != path && null != ext) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.ChangeExtension(path, ext);
                }
            }
            return r;
        }
    }
    internal sealed class QuotePathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                bool onlyNeeded = operands.Count >= 2 ? operands[1].GetBool() : true;
                bool singleQuotes = operands.Count >= 3 ? operands[2].GetBool() : false;
                if (null != path && path.Length > 0) {
                    path = Environment.ExpandEnvironmentVariables(path).Trim();
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                        //On Windows, file names can contain single quotes, but paths must be quoted using double quotes.
                        string delim = "\"";
                        if (onlyNeeded) {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            int ix = path.IndexOf(' ');
                            if (ix > 0 && !CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                        else {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            if (!CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                    }
                    else {
                        string delim = singleQuotes ? "'" : "\"";
                        if (onlyNeeded) {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            int ix = path.IndexOf(' ');
                            if (ix > 0 && !CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                        else {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            if (!CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                    }
                    r = path;
                }
            }
            return r;
        }
        private static bool CharIsQuote(char c)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                return c == '"';
            }
            else {
                return c == '"' || c == '\'';
            }
        }
    }
    internal sealed class EchoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj.IsString) {
                    var fmt = obj.StringVal;
                    if (operands.Count > 1 && null != fmt) {
                        ArrayList arrayList = new ArrayList();
                        for (int i = 1; i < operands.Count; ++i) {
                            arrayList.Add(operands[i].GetObject());
                        }
                        Console.WriteLine(fmt, arrayList.ToArray());
                    }
                    else {
                        Console.WriteLine(obj.GetObject());
                    }
                }
                else {
                    Console.WriteLine(obj.GetObject());
                }
            }
            else {
                Console.WriteLine();
            }
            return r;
        }
    }
    internal sealed class CallStackExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = System.Environment.StackTrace;
            return BoxedValue.FromObject(r);
        }
    }
    internal sealed class CallExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
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
    internal sealed class FileEchoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                DslCalculator.FileEchoOn = operands[0].GetBool();
            }
            return DslCalculator.FileEchoOn;
        }
    }
    internal sealed class DirectoryExistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                ret = Directory.Exists(dir);
            }
            return ret;
        }
    }
    internal sealed class FileExistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                ret = File.Exists(file);
            }
            return ret;
        }
    }
    internal sealed class ListDirectoriesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListAllDirectoriesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListAllFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class CreateDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("create directory {0}", dir);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filterAndNewExts.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filterAndNewExts.Add(tempStr);
                            }
                        }
                    }
                }
                if (filterAndNewExts.Count <= 0) {
                    filterAndNewExts.Add("*");
                }
                var targetRoot = Path.GetFullPath(dir2);
                if (Directory.Exists(dir1)) {
                    CopyFolder(targetRoot, dir1, dir2, filterAndNewExts, ref ct);
                }
            }
            return ct;
        }
        private static void CopyFolder(string targetRoot, string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to))
                Directory.CreateDirectory(to);
            // sub directories
            foreach (string sub in Directory.GetDirectories(from)) {
                var srcPath = Path.GetFullPath(sub);
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) {
                    if (srcPath.IndexOf(targetRoot) == 0)
                        continue;
                }
                else {
                    if (srcPath.IndexOf(targetRoot, StringComparison.CurrentCultureIgnoreCase) == 0)
                        continue;
                }
                var sName = Path.GetFileName(sub);
                CopyFolder(targetRoot, sub, Path.Combine(to, sName), filterAndNewExts, ref ct);
            }
            // file
            for (int i = 0; i < filterAndNewExts.Count; i += 2) {
                string filter = filterAndNewExts[i];
                string newExt = string.Empty;
                if (i + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[i + 1];
                }
                foreach (string file in Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly)) {
                    string targetFile;
                    if (string.IsNullOrEmpty(newExt))
                        targetFile = Path.Combine(to, Path.GetFileName(file));
                    else
                        targetFile = Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt));
                    File.Copy(file, targetFile, true);
                    ++ct;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file, targetFile);
                    }
                }
            }
        }
    }
    internal sealed class MoveDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                if (Directory.Exists(dir1)) {
                    if (Directory.Exists(dir2)) {
                        Directory.Delete(dir2);
                    }
                    Directory.Move(dir1, dir2);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("move directory {0} => {1}", dir1, dir2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("delete directory {0}", dir);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0].AsString;
                var file2 = operands[1].AsString;
                file1 = Environment.ExpandEnvironmentVariables(file1);
                file2 = Environment.ExpandEnvironmentVariables(file2);
                if (File.Exists(file1)) {
                    var dir = Path.GetDirectoryName(file2);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    File.Copy(file1, file2, true);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file1, file2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filterAndNewExts.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filterAndNewExts.Add(tempStr);
                            }
                        }
                    }
                }
                if (filterAndNewExts.Count <= 0) {
                    filterAndNewExts.Add("*");
                }
                if (Directory.Exists(dir1)) {
                    CopyFolder(dir1, dir2, filterAndNewExts, ref ct);
                }
            }
            return ct;
        }
        private static void CopyFolder(string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to))
                Directory.CreateDirectory(to);
            // file
            for (int i = 0; i < filterAndNewExts.Count; i += 2) {
                string filter = filterAndNewExts[i];
                string newExt = string.Empty;
                if (i + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[i + 1];
                }
                foreach (string file in Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly)) {
                    string targetFile;
                    if (string.IsNullOrEmpty(newExt))
                        targetFile = Path.Combine(to, Path.GetFileName(file));
                    else
                        targetFile = Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt));
                    File.Copy(file, targetFile, true);
                    ++ct;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file, targetFile);
                    }
                }
            }
        }
    }
    internal sealed class MoveFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0].AsString;
                var file2 = operands[1].AsString;
                file1 = Environment.ExpandEnvironmentVariables(file1);
                file2 = Environment.ExpandEnvironmentVariables(file2);
                if (File.Exists(file1)) {
                    var dir = Path.GetDirectoryName(file2);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    if (File.Exists(file2)) {
                        File.Delete(file2);
                    }
                    File.Move(file1, file2);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("move file {0} => {1}", file1, file2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    File.Delete(file);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("delete file {0}", file);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filters.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filters.Add(tempStr);
                            }
                        }
                    }
                }
                if (filters.Count <= 0) {
                    filters.Add("*");
                }
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    foreach (var filter in filters) {
                        foreach (string file in Directory.GetFiles(dir, filter, SearchOption.TopDirectoryOnly)) {
                            File.Delete(file);
                            ++ct;

                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("delete file {0}", file);
                            }
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal sealed class DeleteAllFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filters.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filters.Add(tempStr);
                            }
                        }
                    }
                }
                if (filters.Count <= 0) {
                    filters.Add("*");
                }
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    foreach (var filter in filters) {
                        foreach (string file in Directory.GetFiles(dir, filter, SearchOption.AllDirectories)) {
                            File.Delete(file);
                            ++ct;

                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("delete file {0}", file);
                            }
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal sealed class GetFileInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    ret = BoxedValue.FromObject(new FileInfo(file));
                }
            }
            return ret;
        }
    }
    internal sealed class GetDirectoryInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (Directory.Exists(file)) {
                    ret = BoxedValue.FromObject(new DirectoryInfo(file));
                }
            }
            return ret;
        }
    }
    internal sealed class GetDriveInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var drive = operands[0].AsString;
                ret = BoxedValue.FromObject(new DriveInfo(drive));
            }
            return ret;
        }
    }
    internal sealed class GetDrivesInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = DriveInfo.GetDrives();
            return BoxedValue.FromObject(ret);
        }
    }
    internal sealed class ReadAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v);
                    }
                    return BoxedValue.FromObject(File.ReadAllLines(path, encoding));
                }
            }
            return BoxedValue.FromObject(new string[0]);
        }
    }
    internal sealed class WriteAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var lines = operands[1].As<IList>();
                if (!string.IsNullOrEmpty(path) && null != lines) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncoding(v);
                    }
                    var strs = new List<string>();
                    foreach (var line in lines) {
                        strs.Add(line.ToString());
                    }
                    File.WriteAllLines(path, strs, encoding);
                    return true;
                }
            }
            return false;
        }
    }
    internal sealed class ReadAllTextExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v);
                    }
                    return File.ReadAllText(path, encoding);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class WriteAllTextExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var text = operands[1].AsString;
                if (!string.IsNullOrEmpty(path) && null != text) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncoding(v);
                    }
                    File.WriteAllText(path, text, encoding);
                    return true;
                }
            }
            return false;
        }
    }
    internal sealed class CommandExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int exitCode = 0;
            MemoryStream ims = null, oms = null;
            int ct = m_CommandConfigs.Count;
            for (int i = 0; i < ct; ++i) {
                try {
                    if (i > 0) {
                        ims = oms;
                        oms = null;
                    }
                    if (i < ct - 1) {
                        oms = new MemoryStream();
                    }
                    var cfg = m_CommandConfigs[i];
                    if (cfg.m_Commands.Count > 0) {
                        exitCode = ExecCommand(cfg, ims, oms);
                    }
                    else {
                        exitCode = ExecProcess(cfg, ims, oms);
                    }
                }
                finally {
                    if (null != ims) {
                        ims.Close();
                        ims.Dispose();
                        ims = null;
                    }
                }
            }
            return exitCode;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            var id = funcData.GetId();
            if (funcData.IsHighOrder) {
                var callData = funcData.LowerOrderFunction;
                LoadCall(callData);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            else {
                var cmd = new CommandConfig();
                m_CommandConfigs.Add(cmd);
            }
            if (funcData.HaveStatement()) {
                var cmd = m_CommandConfigs[m_CommandConfigs.Count - 1];
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var comp = funcData.GetParam(i);
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        int num = cd.GetParamNum();
                        if (cd.HaveExternScript()) {
                            string os = cd.GetId();
                            string txt = cd.GetParamId(0);
                            cmd.m_Commands.Add(os, txt);
                        }
                        else if (num >= 2) {
                            string type = cd.GetId();
                            var exp = Calculator.Load(cd.GetParam(0));
                            var opt = Calculator.Load(cd.GetParam(1));
                            if (type == "output") {
                                cmd.m_Output = exp;
                                cmd.m_OutputOptArg = opt;
                            }
                            else if (type == "error") {
                                cmd.m_Error = exp;
                                cmd.m_ErrorOptArg = opt;
                            }
                            else {
                                Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false), cd.GetLine());
                            }
                        }
                        else if (num >= 1) {
                            string type = cd.GetId();
                            var exp = Calculator.Load(cd.GetParam(0));
                            if (type == "input") {
                                cmd.m_Input = exp;
                            }
                            else if (type == "output") {
                                cmd.m_Output = exp;
                            }
                            else if (type == "error") {
                                cmd.m_Error = exp;
                            }
                            else if (type == "redirecttoconsole") {
                                cmd.m_RedirectToConsole = exp;
                            }
                            else if (type == "nowait") {
                                cmd.m_NoWait = exp;
                            }
                            else if (type == "useshellexecute") {
                                cmd.m_UseShellExecute = exp;
                            }
                            else if (type == "verb") {
                                cmd.m_Verb = exp;
                            }
                            else if (type == "domain") {
                                cmd.m_Domain = exp;
                            }
                            else if (type == "user") {
                                cmd.m_UserName = exp;
                            }
                            else if (type == "password") {
                                cmd.m_Password = exp;
                            }
                            else if (type == "passwordincleartext") {
                                cmd.m_PasswordInClearText = exp;
                            }
                            else if (type == "loadprofile") {
                                cmd.m_LoadUserProfile = exp;
                            }
                            else if (type == "windowstyle") {
                                cmd.m_WindowStyle = exp;
                            }
                            else if (type == "newwindow") {
                                cmd.m_NewWindow = exp;
                            }
                            else if (type == "errordialog") {
                                cmd.m_ErrorDialog = exp;
                            }
                            else if (type == "workingdirectory") {
                                cmd.m_WorkingDirectory = exp;
                            }
                            else if (type == "encoding") {
                                cmd.m_Encoding = exp;
                            }
                            else {
                                Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false), cd.GetLine());
                            }
                        }
                        else {
                            Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false), cd.GetLine());
                        }
                    }
                    else {
                        Calculator.Log("[syntax error] {0} line:{1}", comp.ToScriptString(false), comp.GetLine());
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            for (int i = 0; i < statementData.GetFunctionNum(); ++i) {
                var func = statementData.GetFunction(i);
                var vd = func.AsValue;
                if (null != vd) {
                    Load(vd);
                }
                else {
                    var fd = func.AsFunction;
                    Load(fd);
                }
            }
            return true;
        }

        private bool LoadCall(Dsl.FunctionData callData)
        {
            var cmd = new CommandConfig();
            m_CommandConfigs.Add(cmd);

            var id = callData.GetId();
            if (id == "process") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    var param0 = callData.GetParam(0);
                    var exp0 = Calculator.Load(param0);
                    cmd.m_FileName = exp0;

                    if (num > 1) {
                        var param1 = callData.GetParam(1);
                        var exp1 = Calculator.Load(param1);
                        cmd.m_Argments = exp1;
                    }
                }
                else {
                    Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
                }
            }
            else if (id == "command") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
                }
            }
            else {
                Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
            }
            return true;
        }
        private int ExecProcess(CommandConfig cfg, Stream istream, Stream ostream)
        {
            string fileName = string.Empty;
            if (null != cfg.m_FileName) {
                fileName = cfg.m_FileName.Calc().AsString;
            }
            string args = string.Empty;
            if (null != cfg.m_Argments) {
                args = cfg.m_Argments.Calc().AsString;
            }
            bool noWait = false;
            if (null != cfg.m_NoWait) {
                noWait = cfg.m_NoWait.Calc().GetBool();
            }
            DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
            if (null != cfg.m_UseShellExecute) {
                option.UseShellExecute = cfg.m_UseShellExecute.Calc().GetBool();
            }
            if (null != cfg.m_Verb) {
                option.Verb = cfg.m_Verb.Calc().AsString;
            }
            if (null != cfg.m_Domain) {
                option.Domain = cfg.m_Domain.Calc().AsString;
            }
            if (null != cfg.m_UserName) {
                option.UserName = cfg.m_UserName.Calc().AsString;
            }
            if (null != cfg.m_Password) {
                option.Password = cfg.m_Password.Calc().AsString;
            }
            if (null != cfg.m_PasswordInClearText) {
                option.PasswordInClearText = cfg.m_PasswordInClearText.Calc().AsString;
            }
            if (null != cfg.m_LoadUserProfile) {
                option.LoadUserProfile = cfg.m_LoadUserProfile.Calc().GetBool();
            }
            if (null != cfg.m_WindowStyle) {
                var str = cfg.m_WindowStyle.Calc().AsString;
                System.Diagnostics.ProcessWindowStyle style;
                if (Enum.TryParse(str, out style)) {
                    option.WindowStyle = style;
                }
            }
            if (null != cfg.m_NewWindow) {
                option.NewWindow = cfg.m_NewWindow.Calc().GetBool();
            }
            if (null != cfg.m_ErrorDialog) {
                option.ErrorDialog = cfg.m_ErrorDialog.Calc().GetBool();
            }
            if (null != cfg.m_WorkingDirectory) {
                option.WorkingDirectory = cfg.m_WorkingDirectory.Calc().AsString;
            }
            Encoding encoding = null;
            if (null != cfg.m_Encoding) {
                var v = cfg.m_Encoding.Calc();
                var name = v.AsString;
                if (!string.IsNullOrEmpty(name)) {
                    encoding = Encoding.GetEncoding(name);
                }
                else if (v.IsInteger) {
                    int codePage = v.GetInt();
                    encoding = Encoding.GetEncoding(codePage);
                }
            }

            fileName = Environment.ExpandEnvironmentVariables(fileName);
            args = Environment.ExpandEnvironmentVariables(args);

            IList<string> input = null;
            if (null != cfg.m_Input) {
                var v = cfg.m_Input.Calc();
                try {
                    var list = v.As<IList>();
                    if (null != list) {
                        var slist = new List<string>();
                        foreach (var s in list) {
                            slist.Add(s.ToString());
                        }
                        input = slist;
                    }
                    else {
                        var str = v.AsString;
                        if (!string.IsNullOrEmpty(str)) {
                            str = Environment.ExpandEnvironmentVariables(str);
                            input = File.ReadAllLines(str);
                        }
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("input {0} failed:{1}", v, ex.Message);
                }
            }
            bool redirectToConsole = DslCalculator.FileEchoOn;
            StringBuilder outputBuilder = null;
            StringBuilder errorBuilder = null;
            var output = BoxedValue.NullObject;
            int outputIx = -1;
            var error = BoxedValue.NullObject;
            int errorIx = -1;
            if (null != cfg.m_Output) {
                var v = cfg.m_Output.Calc();
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    output = str;
                }
                else {
                    output = v;
                }
                if (null != cfg.m_OutputOptArg)
                    outputIx = cfg.m_OutputOptArg.Calc().GetInt();
                outputBuilder = new StringBuilder();
            }
            if (null != cfg.m_Error) {
                var v = cfg.m_Error.Calc();
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    error = str;
                }
                else {
                    error = v;
                }
                if (null != cfg.m_ErrorOptArg)
                    errorIx = cfg.m_ErrorOptArg.Calc().GetInt();
                errorBuilder = new StringBuilder();
            }
            if (null != cfg.m_RedirectToConsole) {
                var v = cfg.m_RedirectToConsole.Calc();
                redirectToConsole = v.GetBool();
            }
            int exitCode = DslCalculator.NewProcess(noWait, fileName, args, option, istream, ostream, input, outputBuilder, errorBuilder, redirectToConsole, encoding);
            if (DslCalculator.FileEchoOn) {
                Console.WriteLine("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);
            }

            if (null != outputBuilder && !output.IsNullObject) {
                try {
                    var file = output.AsString;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, outputBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, outputBuilder.ToString());
                        }
                    }
                    else if (outputIx >= 0) {
                        var list = output.As<IList>();
                        while (list.Count <= outputIx) {
                            list.Add(null);
                        }
                        list[outputIx] = outputBuilder.ToString();
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("output {0} failed:{1}", output, ex.Message);
                }
            }
            if (null != errorBuilder && !error.IsNullObject) {
                try {
                    var file = error.AsString;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, errorBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, errorBuilder.ToString());
                        }
                    }
                    else if (errorIx >= 0) {
                        var list = error.As<IList>();
                        while (list.Count <= errorIx) {
                            list.Add(null);
                        }
                        list[errorIx] = errorBuilder.ToString();
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("error {0} failed:{1}", error, ex.Message);
                }
            }
            return exitCode;
        }
        private int ExecCommand(CommandConfig cfg, Stream istream, Stream ostream)
        {
            int exitCode = 0;
            string os = string.Empty;
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                os = "unix";
            else
                os = "win";
            string cmd;
            if (cfg.m_Commands.TryGetValue(os, out cmd) || cfg.m_Commands.TryGetValue("common", out cmd)) {
                bool noWait = false;
                if (null != cfg.m_NoWait) {
                    noWait = cfg.m_NoWait.Calc().GetBool();
                }
                DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
                if (null != cfg.m_UseShellExecute) {
                    option.UseShellExecute = cfg.m_UseShellExecute.Calc().GetBool();
                }
                if (null != cfg.m_Verb) {
                    option.Verb = cfg.m_Verb.Calc().AsString;
                }
                if (null != cfg.m_Domain) {
                    option.Domain = cfg.m_Domain.Calc().AsString;
                }
                if (null != cfg.m_UserName) {
                    option.UserName = cfg.m_UserName.Calc().AsString;
                }
                if (null != cfg.m_Password) {
                    option.Password = cfg.m_Password.Calc().AsString;
                }
                if (null != cfg.m_PasswordInClearText) {
                    option.PasswordInClearText = cfg.m_PasswordInClearText.Calc().AsString;
                }
                if (null != cfg.m_LoadUserProfile) {
                    option.LoadUserProfile = cfg.m_LoadUserProfile.Calc().GetBool();
                }
                if (null != cfg.m_WindowStyle) {
                    var str = cfg.m_WindowStyle.Calc().AsString;
                    System.Diagnostics.ProcessWindowStyle style;
                    if (Enum.TryParse(str, out style)) {
                        option.WindowStyle = style;
                    }
                }
                if (null != cfg.m_NewWindow) {
                    option.NewWindow = cfg.m_NewWindow.Calc().GetBool();
                }
                if (null != cfg.m_ErrorDialog) {
                    option.ErrorDialog = cfg.m_ErrorDialog.Calc().GetBool();
                }
                if (null != cfg.m_WorkingDirectory) {
                    option.WorkingDirectory = cfg.m_WorkingDirectory.Calc().AsString;
                }
                Encoding encoding = null;
                if (null != cfg.m_Encoding) {
                    var v = cfg.m_Encoding.Calc();
                    var name = v.AsString;
                    if (!string.IsNullOrEmpty(name)) {
                        encoding = Encoding.GetEncoding(name);
                    }
                    else if (v.IsInteger) {
                        int codePage = v.GetInt();
                        encoding = Encoding.GetEncoding(codePage);
                    }
                }
                IList<string> input = null;
                if (null != cfg.m_Input) {
                    var v = cfg.m_Input.Calc();
                    try {
                        var list = v.As<IList>();
                        if (null != list) {
                            var slist = new List<string>();
                            foreach (var s in list) {
                                slist.Add(s.ToString());
                            }
                            input = slist;
                        }
                        else {
                            var str = v.AsString;
                            if (!string.IsNullOrEmpty(str)) {
                                str = Environment.ExpandEnvironmentVariables(str);
                                input = File.ReadAllLines(str);
                            }
                        }
                    }
                    catch (Exception ex) {
                        Calculator.Log("input {0} failed:{1}", v, ex.Message);
                    }
                }
                bool redirectToConsole = DslCalculator.FileEchoOn;
                StringBuilder outputBuilder = null;
                StringBuilder errorBuilder = null;
                var output = BoxedValue.NullObject;
                int outputIx = -1;
                var error = BoxedValue.NullObject;
                int errorIx = -1;
                if (null != cfg.m_Output) {
                    var v = cfg.m_Output.Calc();
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        output = str;
                    }
                    else {
                        output = v;
                    }
                    if (null != cfg.m_OutputOptArg)
                        outputIx = cfg.m_OutputOptArg.Calc().GetInt();
                    outputBuilder = new StringBuilder();
                }
                if (null != cfg.m_Error) {
                    var v = cfg.m_Error.Calc();
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        error = str;
                    }
                    else {
                        error = v;
                    }
                    if (null != cfg.m_ErrorOptArg)
                        errorIx = cfg.m_ErrorOptArg.Calc().GetInt();
                    errorBuilder = new StringBuilder();
                }
                if (null != cfg.m_RedirectToConsole) {
                    var v = cfg.m_RedirectToConsole.Calc();
                    redirectToConsole = v.GetBool();
                }

                cmd = cmd.Trim();
                var lines = cmd.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string oneCmd = string.Join(" ", lines).Trim();
                if (!string.IsNullOrEmpty(oneCmd)) {
                    int split = oneCmd.IndexOfAny(new char[] { ' ', '\t' });
                    string fileName = oneCmd;
                    string args = string.Empty;
                    if (split > 0) {
                        fileName = oneCmd.Substring(0, split).Trim();
                        args = oneCmd.Substring(split).Trim();
                    }

                    fileName = Environment.ExpandEnvironmentVariables(fileName);
                    args = Environment.ExpandEnvironmentVariables(args);

                    exitCode = DslCalculator.NewProcess(noWait, fileName, args, option, istream, ostream, input, outputBuilder, errorBuilder, redirectToConsole, encoding);
                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);
                    }

                    if (null != outputBuilder && !output.IsNullObject) {
                        try {
                            var file = output.AsString;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, outputBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, outputBuilder.ToString());
                                }
                            }
                            else if (outputIx >= 0) {
                                var list = output.As<IList>();
                                while (list.Count <= outputIx) {
                                    list.Add(null);
                                }
                                list[outputIx] = outputBuilder.ToString();
                            }
                        }
                        catch (Exception ex) {
                            Calculator.Log("output {0} failed:{1}", output, ex.Message);
                        }
                    }
                    if (null != errorBuilder && !error.IsNullObject) {
                        try {
                            var file = error.AsString;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, errorBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, errorBuilder.ToString());
                                }
                            }
                            else if (errorIx >= 0) {
                                var list = error.As<IList>();
                                while (list.Count <= errorIx) {
                                    list.Add(null);
                                }
                                list[errorIx] = errorBuilder.ToString();
                            }
                        }
                        catch (Exception ex) {
                            Calculator.Log("error {0} failed:{1}", error, ex.Message);
                        }
                    }
                }
            }
            return exitCode;
        }

        private class CommandConfig
        {
            internal IExpression m_FileName = null;
            internal IExpression m_Argments = null;
            internal Dictionary<string, string> m_Commands = new Dictionary<string, string>();

            internal IExpression m_NoWait = null;
            internal IExpression m_UseShellExecute = null;
            internal IExpression m_Verb = null;
            internal IExpression m_Domain = null;
            internal IExpression m_UserName = null;
            internal IExpression m_Password = null;
            internal IExpression m_PasswordInClearText = null;
            internal IExpression m_LoadUserProfile = null;
            internal IExpression m_WindowStyle = null;
            internal IExpression m_NewWindow = null;
            internal IExpression m_ErrorDialog = null;
            internal IExpression m_WorkingDirectory = null;
            internal IExpression m_Encoding = null;
            internal IExpression m_Input = null;
            internal IExpression m_Output = null;
            internal IExpression m_OutputOptArg = null;
            internal IExpression m_Error = null;
            internal IExpression m_ErrorOptArg = null;
            internal IExpression m_RedirectToConsole = null;
        }

        private List<CommandConfig> m_CommandConfigs = new List<CommandConfig>();
    }
    internal sealed class KillExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ret = 0;
            if (operands.Count >= 1) {
                int myselfId = 0;
                var myself = System.Diagnostics.Process.GetCurrentProcess();
                if (null != myself) {
                    myselfId = myself.Id;
                }
                var vObj = operands[0];
                var name = vObj.AsString;
                if (!string.IsNullOrEmpty(name)) {
                    int ct = 0;
                    var ps = System.Diagnostics.Process.GetProcessesByName(name);
                    foreach (var p in ps) {
                        if (p.Id != myselfId) {
                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                            }
                            p.Kill();
                            ++ct;
                        }
                    }
                    ret = ct;
                }
                else if (vObj.IsInteger) {
                    int pid = vObj.GetInt();
                    var p = System.Diagnostics.Process.GetProcessById(pid);
                    if (null != p && p.Id != myselfId) {
                        if (DslCalculator.FileEchoOn) {
                            Console.WriteLine("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                        }
                        p.Kill();
                        ret = 1;
                    }
                }
                else {

                }
            }
            return BoxedValue.From(ret);
        }
    }
    internal sealed class KillMeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
                int exitCode = 0;
                if (operands.Count >= 1) {
                    exitCode = operands[0].GetInt();
                }
                if (DslCalculator.FileEchoOn) {
                    Console.WriteLine("killme {0}[pid:{1},session id:{2}] exit code:{3}", p.ProcessName, p.Id, p.SessionId, exitCode);
                }
                Environment.Exit(exitCode);
            }
            return ret;
        }
    }
    internal sealed class GetCurrentProcessIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
            }
            return ret;
        }
    }
    internal sealed class ListProcessesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            IList<System.Diagnostics.Process> ret = null;
            var ps = System.Diagnostics.Process.GetProcesses();
            string filter = null;
            if (operands.Count >= 1) {
                filter = operands[0].AsString;
            }
            if (null == filter)
                filter = string.Empty;
            if (!string.IsNullOrEmpty(filter)) {
                var list = new List<System.Diagnostics.Process>();
                foreach (var p in ps) {
                    try {
                        if (!p.HasExited) {
                            if (p.ProcessName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) {
                                list.Add(p);
                            }
                        }
                    }
                    catch {
                    }
                }
                ret = list;
            }
            else {
                ret = ps;
            }
            return BoxedValue.FromObject(ret);
        }
    }
    internal sealed class WaitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var time = operands[0].GetInt();
                System.Threading.Thread.Sleep(time);
                ret = time;
            }
            return ret;
        }
    }
    internal sealed class WaitAllExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var tasks = DslCalculator.Tasks;
            int timeout = -1;
            if (operands.Count >= 1) {
                timeout = operands[0].GetInt();
            }
            List<int> results = new List<int>();
            if (Task.WaitAll(tasks.ToArray(), timeout)) {
                foreach (var task in tasks) {
                    results.Add(task.Result);
                }
            }
            return BoxedValue.FromObject(results);
        }
    }
    internal sealed class WaitStartIntervalExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                var v = operands[0];
                if (!v.IsNullObject) {
                    DslCalculator.CheckStartInterval = v.GetInt();
                }
            }
            return DslCalculator.CheckStartInterval;
        }
    }
    internal sealed class CleanupCompletedTasksExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            DslCalculator.CleanupCompletedTasks();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetTaskCountExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return DslCalculator.Tasks.Count;
        }
    }
    internal sealed class CalcMd5Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                if (null != file) {
                    r = CalcMD5(file);
                }
            }
            return r;
        }
        public string CalcMD5(string file)
        {
            byte[] array = null;
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                MD5 md5 = MD5.Create();
                array = md5.ComputeHash(stream);
                stream.Close();
            }
            if (null != array) {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++) {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
            else {
                return string.Empty;
            }
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

            public void BuildArgNameIndexes(IList<string> argNames)
            {
                if (null != argNames) {
                    for (int ix = 0; ix < argNames.Count; ++ix) {
                        LocalVarIndexes[argNames[ix]] = -1 - ix;
                    }
                }
            }
        }

        public Dsl.DslLogDelegation OnLog;
        public TryGetVariableDelegation OnTryGetVariable;
        public TrySetVariableDelegation OnTrySetVariable;
        public LoadFailbackDelegation OnLoadFailback;

        public bool Inited { get { return m_Inited; } }
        public void Init()
        {
            m_Inited = true;

            Register("args", "args() api", new ExpressionFactoryHelper<ArgsGet>());
            Register("arg", "arg(ix) api", new ExpressionFactoryHelper<ArgGet>());
            Register("argnum", "argnum() api", new ExpressionFactoryHelper<ArgNumGet>());
            Register("+", "add operator", new ExpressionFactoryHelper<AddExp>());
            Register("-", "sub operator", new ExpressionFactoryHelper<SubExp>());
            Register("*", "mul operator", new ExpressionFactoryHelper<MulExp>());
            Register("/", "div operator", new ExpressionFactoryHelper<DivExp>());
            Register("%", "mod operator", new ExpressionFactoryHelper<ModExp>());
            Register("&", "bitand operator", new ExpressionFactoryHelper<BitAndExp>());
            Register("|", "bitor operator", new ExpressionFactoryHelper<BitOrExp>());
            Register("^", "bitxor operator", new ExpressionFactoryHelper<BitXorExp>());
            Register("~", "bitnot operator", new ExpressionFactoryHelper<BitNotExp>());
            Register("<<", "left shift operator", new ExpressionFactoryHelper<LShiftExp>());
            Register(">>", "right shift operator", new ExpressionFactoryHelper<RShiftExp>());
            Register("max", "max(v1,v2) api", new ExpressionFactoryHelper<MaxExp>());
            Register("min", "min(v1,v2) api", new ExpressionFactoryHelper<MinExp>());
            Register("abs", "abs(v) api", new ExpressionFactoryHelper<AbsExp>());
            Register("sin", "sin(v) api", new ExpressionFactoryHelper<SinExp>());
            Register("cos", "cos(v) api", new ExpressionFactoryHelper<CosExp>());
            Register("tan", "tan(v) api", new ExpressionFactoryHelper<TanExp>());
            Register("asin", "asin(v) api", new ExpressionFactoryHelper<AsinExp>());
            Register("acos", "acos(v) api", new ExpressionFactoryHelper<AcosExp>());
            Register("atan", "atan(v) api", new ExpressionFactoryHelper<AtanExp>());
            Register("atan2", "atan2(v1,v2) api", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", "sinh(v) api", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", "cosh(v) api", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", "tanh(v) api", new ExpressionFactoryHelper<TanhExp>());
            Register("rndint", "rndint(min,max) api", new ExpressionFactoryHelper<RndIntExp>());
            Register("rndfloat", "rndfloat(min,max) api", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", "pow(v1,v2) api", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", "sqrt(v) api", new ExpressionFactoryHelper<SqrtExp>());
            Register("exp", "exp(v) api", new ExpressionFactoryHelper<ExpExp>());
            Register("exp2", "exp2(v) api", new ExpressionFactoryHelper<Exp2Exp>());
            Register("log", "log(v) api", new ExpressionFactoryHelper<LogExp>());
            Register("log2", "log2(v) api", new ExpressionFactoryHelper<Log2Exp>());
            Register("log10", "log10(v) api", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", "floor(v) api", new ExpressionFactoryHelper<FloorExp>());
            Register("ceiling", "ceiling(v) api", new ExpressionFactoryHelper<CeilingExp>());
            Register("round", "round(v) api", new ExpressionFactoryHelper<RoundExp>());
            Register("floortoint", "floortoint(v) api", new ExpressionFactoryHelper<FloorToIntExp>());
            Register("ceilingtoint", "ceilingtoint(v) api", new ExpressionFactoryHelper<CeilingToIntExp>());
            Register("roundtoint", "roundtoint(v) api", new ExpressionFactoryHelper<RoundToIntExp>());
            Register("bool", "bool(v) api", new ExpressionFactoryHelper<BoolExp>());
            Register("sbyte", "sbyte(v) api", new ExpressionFactoryHelper<SByteExp>());
            Register("byte", "byte(v) api", new ExpressionFactoryHelper<ByteExp>());
            Register("char", "char(v) api", new ExpressionFactoryHelper<CharExp>());
            Register("short", "short(v) api", new ExpressionFactoryHelper<ShortExp>());
            Register("ushort", "ushort(v) api", new ExpressionFactoryHelper<UShortExp>());
            Register("int", "int(v) api", new ExpressionFactoryHelper<IntExp>());
            Register("uint", "uint(v) api", new ExpressionFactoryHelper<UIntExp>());
            Register("long", "long(v) api", new ExpressionFactoryHelper<LongExp>());
            Register("ulong", "ulong(v) api", new ExpressionFactoryHelper<ULongExp>());
            Register("float", "float(v) api", new ExpressionFactoryHelper<FloatExp>());
            Register("double", "double(v) api", new ExpressionFactoryHelper<DoubleExp>());
            Register("decimal", "decimal(v) api", new ExpressionFactoryHelper<DecimalExp>());
            Register("ftoi", "ftoi(v) api", new ExpressionFactoryHelper<FtoiExp>());
            Register("itof", "itof(v) api", new ExpressionFactoryHelper<ItofExp>());
            Register("ftou", "ftou(v) api", new ExpressionFactoryHelper<FtouExp>());
            Register("utof", "utof(v) api", new ExpressionFactoryHelper<UtofExp>());
            Register("dtol", "dtol(v) api", new ExpressionFactoryHelper<DtolExp>());
            Register("ltod", "ltod(v) api", new ExpressionFactoryHelper<LtodExp>());
            Register("dtou", "dtou(v) api", new ExpressionFactoryHelper<DtouExp>());
            Register("utod", "utod(v) api", new ExpressionFactoryHelper<UtodExp>());
            Register("lerp", "lerp(a,b,t) api", new ExpressionFactoryHelper<LerpExp>());
            Register("lerpunclamped", "lerpunclamped(a,b,t) api", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerpangle", "lerpangle(a,b,t) api", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("smoothstep", "smoothstep(from,to,t) api", new ExpressionFactoryHelper<SmoothStepExp>());
            Register("clamp01", "clamp01(v) api", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", "clamp(v,v1,v2) api", new ExpressionFactoryHelper<ClampExp>());
            Register("approximately", "approximately(v1,v2) api", new ExpressionFactoryHelper<ApproximatelyExp>());
            Register("ispoweroftwo", "ispoweroftwo(v) api", new ExpressionFactoryHelper<IsPowerOfTwoExp>());
            Register("closestpoweroftwo", "closestpoweroftwo(v) api", new ExpressionFactoryHelper<ClosestPowerOfTwoExp>());
            Register("nextpoweroftwo", "nextpoweroftwo(v) api", new ExpressionFactoryHelper<NextPowerOfTwoExp>());
            Register("dist", "dist(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistExp>());
            Register("distsqr", "distsqr(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistSqrExp>());
            Register(">", "great operator", new ExpressionFactoryHelper<GreatExp>());
            Register(">=", "great equal operator", new ExpressionFactoryHelper<GreatEqualExp>());
            Register("<", "less operator", new ExpressionFactoryHelper<LessExp>());
            Register("<=", "less equal operator", new ExpressionFactoryHelper<LessEqualExp>());
            Register("==", "equal operator", new ExpressionFactoryHelper<EqualExp>());
            Register("!=", "not equal operator", new ExpressionFactoryHelper<NotEqualExp>());
            Register("&&", "logical and operator", new ExpressionFactoryHelper<AndExp>());
            Register("||", "logical or operator", new ExpressionFactoryHelper<OrExp>());
            Register("!", "logical not operator", new ExpressionFactoryHelper<NotExp>());
            Register("?", "conditional expression", new ExpressionFactoryHelper<CondExp>());
            Register("if", "if(cond)func(args); or if(cond){...}[elseif/elif(cond){...}else{...}]; statement", new ExpressionFactoryHelper<IfExp>());
            Register("while", "while(cond)func(args); or while(cond){...}; statement, iterator is $$", new ExpressionFactoryHelper<WhileExp>());
            Register("loop", "loop(ct)func(args); or loop(ct){...}; statement, iterator is $$", new ExpressionFactoryHelper<LoopExp>());
            Register("looplist", "looplist(list)func(args); or looplist(list){...}; statement, iterator is $$", new ExpressionFactoryHelper<LoopListExp>());
            Register("foreach", "foreach(args)func(args); or foreach(args){...}; statement, iterator is $$", new ExpressionFactoryHelper<ForeachExp>());
            Register("format", "format(fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<FormatExp>());
            Register("gettypeassemblyname", "gettypeassemblyname(obj) api", new ExpressionFactoryHelper<GetTypeAssemblyNameExp>());
            Register("gettypefullname", "gettypefullname(obj) api", new ExpressionFactoryHelper<GetTypeFullNameExp>());
            Register("gettypename", "gettypename(obj) api", new ExpressionFactoryHelper<GetTypeNameExp>());
            Register("gettype", "gettype(type_str) api", new ExpressionFactoryHelper<GetTypeExp>());
            Register("changetype", "changetype(obj,type_str) api", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("parseenum", "parseenum(type_str,val_str) api", new ExpressionFactoryHelper<ParseEnumExp>());
            Register("dotnetcall", "dotnetcall api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnetset", "dotnetset api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnetget", "dotnetget api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("collectioncall", "collectioncall api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionCallExp>());
            Register("collectionset", "collectionset api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionSetExp>());
            Register("collectionget", "collectionget api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionGetExp>());
            Register("linq", "linq(list,method,arg1,arg2,...) statement, internal implementation, using obj.method(arg1,arg2,...) syntax, method can be orderby/orderbydesc/where/top, iterator is $$", new ExpressionFactoryHelper<LinqExp>());
            Register("isnull", "isnull(obj) api", new ExpressionFactoryHelper<IsNullExp>());
            Register("null", "null() api", new ExpressionFactoryHelper<NullExp>());
            Register("equalsnull", "equalsnull(obj) api", new ExpressionFactoryHelper<EqualsNullExp>());
            Register("dotnetload", "dotnetload(dll_path) api", new ExpressionFactoryHelper<DotnetLoadExp>());
            Register("dotnetnew", "dotnetnew(assembly,type_name,arg1,arg2,...) api", new ExpressionFactoryHelper<DotnetNewExp>());
            Register("substring", "substring(str[,start,len]) function", new ExpressionFactoryHelper<SubstringExp>());
            Register("newstringbuilder", "newstringbuilder() api", new ExpressionFactoryHelper<NewStringBuilderExp>());
            Register("appendformat", "appendformat(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatExp>());
            Register("appendformatline", "appendformatline(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatLineExp>());
            Register("stringbuilder_tostring", "stringbuilder_tostring(sb)", new ExpressionFactoryHelper<StringBuilderToStringExp>());
            Register("stringjoin", "stringjoin(sep,list) api", new ExpressionFactoryHelper<StringJoinExp>());
            Register("stringsplit", "stringsplit(str,sep_list) api", new ExpressionFactoryHelper<StringSplitExp>());
            Register("stringtrim", "stringtrim(str) api", new ExpressionFactoryHelper<StringTrimExp>());
            Register("stringtrimstart", "stringtrimstart(str) api", new ExpressionFactoryHelper<StringTrimStartExp>());
            Register("stringtrimend", "stringtrimend(str) api", new ExpressionFactoryHelper<StringTrimEndExp>());
            Register("stringtolower", "stringtolower(str) api", new ExpressionFactoryHelper<StringToLowerExp>());
            Register("stringtoupper", "stringtoupper(str) api", new ExpressionFactoryHelper<StringToUpperExp>());
            Register("stringreplace", "stringreplace(str,key,rep_str) api", new ExpressionFactoryHelper<StringReplaceExp>());
            Register("stringreplacechar", "stringreplacechar(str,key,char_as_str) api", new ExpressionFactoryHelper<StringReplaceCharExp>());
            Register("makestring", "makestring(char1_as_str_or_int,char2_as_str_or_int,...) api", new ExpressionFactoryHelper<MakeStringExp>());
            Register("stringcontains", "stringcontains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsExp>());
            Register("stringnotcontains", "stringnotcontains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsExp>());
            Register("stringcontainsany", "stringcontainsany(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsAnyExp>());
            Register("stringnotcontainsany", "stringnotcontainsany(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsAnyExp>());
            Register("str2int", "str2int(str) api", new ExpressionFactoryHelper<Str2IntExp>());
            Register("str2uint", "str2uint(str) api", new ExpressionFactoryHelper<Str2UintExp>());
            Register("str2long", "str2long(str) api", new ExpressionFactoryHelper<Str2LongExp>());
            Register("str2ulong", "str2ulong(str) api", new ExpressionFactoryHelper<Str2UlongExp>());
            Register("str2float", "str2float(str) api", new ExpressionFactoryHelper<Str2FloatExp>());
            Register("str2double", "str2double(str) api", new ExpressionFactoryHelper<Str2DoubleExp>());
            Register("hex2int", "hex2int(str) api", new ExpressionFactoryHelper<Hex2IntExp>());
            Register("hex2uint", "hex2uint(str) api", new ExpressionFactoryHelper<Hex2UintExp>());
            Register("hex2long", "hex2long(str) api", new ExpressionFactoryHelper<Hex2LongExp>());
            Register("hex2ulong", "hex2ulong(str) api", new ExpressionFactoryHelper<Hex2UlongExp>());
            Register("datetimestr", "datetimestr(fmt) api", new ExpressionFactoryHelper<DatetimeStrExp>());
            Register("longdatestr", "longdatestr() api", new ExpressionFactoryHelper<LongDateStrExp>());
            Register("longtimestr", "longtimestr() api", new ExpressionFactoryHelper<LongTimeStrExp>());
            Register("shortdatestr", "shortdatestr() api", new ExpressionFactoryHelper<ShortDateStrExp>());
            Register("shorttimestr", "shorttimestr() api", new ExpressionFactoryHelper<ShortTimeStrExp>());
            Register("isnullorempty", "isnullorempty(str) api", new ExpressionFactoryHelper<IsNullOrEmptyExp>());
            Register("array", "[v1,v2,...] or array(v1,v2,...) object", new ExpressionFactoryHelper<ArrayExp>());
            Register("toarray", "toarray(list) api", new ExpressionFactoryHelper<ToArrayExp>());
            Register("listsize", "listsize(list) api", new ExpressionFactoryHelper<ListSizeExp>());
            Register("list", "list(v1,v2,...) object", new ExpressionFactoryHelper<ListExp>());
            Register("listget", "listget(list,index[,defval]) api", new ExpressionFactoryHelper<ListGetExp>());
            Register("listset", "listset(list,index,val) api", new ExpressionFactoryHelper<ListSetExp>());
            Register("listindexof", "listindexof(list,val) api", new ExpressionFactoryHelper<ListIndexOfExp>());
            Register("listadd", "listadd(list,val) api", new ExpressionFactoryHelper<ListAddExp>());
            Register("listremove", "listremove(list,val) api", new ExpressionFactoryHelper<ListRemoveExp>());
            Register("listinsert", "listinsert(list,index,val) api", new ExpressionFactoryHelper<ListInsertExp>());
            Register("listremoveat", "listremoveat(list,index) api", new ExpressionFactoryHelper<ListRemoveAtExp>());
            Register("listclear", "listclear(list) api", new ExpressionFactoryHelper<ListClearExp>());
            Register("listsplit", "listsplit(list,ct) api, return list of list", new ExpressionFactoryHelper<ListSplitExp>());
            Register("hashtablesize", "hashtablesize(hash) api", new ExpressionFactoryHelper<HashtableSizeExp>());
            Register("hashtable", "{k1=>v1,k2=>v2,...} or {k1:v1,k2:v2,...} or hashtable(k1=>v1,k2=>v2,...) or hashtable(k1:v1,k2:v2,...) object", new ExpressionFactoryHelper<HashtableExp>());
            Register("hashtableget", "hashtableget(hash,key[,defval]) api", new ExpressionFactoryHelper<HashtableGetExp>());
            Register("hashtableset", "hashtableset(hash,key,val) api", new ExpressionFactoryHelper<HashtableSetExp>());
            Register("hashtableadd", "hashtableadd(hash,key,val) api", new ExpressionFactoryHelper<HashtableAddExp>());
            Register("hashtableremove", "hashtableremove(hash,key) api", new ExpressionFactoryHelper<HashtableRemoveExp>());
            Register("hashtableclear", "hashtableclear(hash) api", new ExpressionFactoryHelper<HashtableClearExp>());
            Register("hashtablekeys", "hashtablekeys(hash) api", new ExpressionFactoryHelper<HashtableKeysExp>());
            Register("hashtablevalues", "hashtablevalues(hash) api", new ExpressionFactoryHelper<HashtableValuesExp>());
            Register("listhashtable", "listhashtable(hash) api, return list of pair", new ExpressionFactoryHelper<ListHashtableExp>());
            Register("hashtablesplit", "hashtablesplit(hash,ct) api, return list of hashtable", new ExpressionFactoryHelper<HashtableSplitExp>());
            Register("peek", "peek(queue_or_stack) api", new ExpressionFactoryHelper<PeekExp>());
            Register("stacksize", "stacksize(stack) api", new ExpressionFactoryHelper<StackSizeExp>());
            Register("stack", "stack(v1,v2,...) object", new ExpressionFactoryHelper<StackExp>());
            Register("push", "push(stack,v) api", new ExpressionFactoryHelper<PushExp>());
            Register("pop", "pop(stack) api", new ExpressionFactoryHelper<PopExp>());
            Register("stackclear", "stackclear(stack) api", new ExpressionFactoryHelper<StackClearExp>());
            Register("queuesize", "queuesize(queue) api", new ExpressionFactoryHelper<QueueSizeExp>());
            Register("queue", "queue(v1,v2,...) object", new ExpressionFactoryHelper<QueueExp>());
            Register("enqueue", "enqueue(queue,v) api", new ExpressionFactoryHelper<EnqueueExp>());
            Register("dequeue", "dequeue(queue) api", new ExpressionFactoryHelper<DequeueExp>());
            Register("queueclear", "queueclear(queue) api", new ExpressionFactoryHelper<QueueClearExp>());
            Register("setenv", "setenv(k,v) api", new ExpressionFactoryHelper<SetEnvironmentExp>());
            Register("getenv", "getenv(k) api", new ExpressionFactoryHelper<GetEnvironmentExp>());
            Register("expand", "expand(str) api", new ExpressionFactoryHelper<ExpandEnvironmentsExp>());
            Register("envs", "envs() api", new ExpressionFactoryHelper<EnvironmentsExp>());
            Register("cd", "cd(path) api", new ExpressionFactoryHelper<SetCurrentDirectoryExp>());
            Register("pwd", "pwd() api", new ExpressionFactoryHelper<GetCurrentDirectoryExp>());
            Register("cmdline", "cmdline() api", new ExpressionFactoryHelper<CommandLineExp>());
            Register("cmdlineargs", "cmdlineargs(prev_arg) or cmdlineargs() api, first return next arg, second return array of arg", new ExpressionFactoryHelper<CommandLineArgsExp>());
            Register("os", "os() api", new ExpressionFactoryHelper<OsExp>());
            Register("osplatform", "osplatform() api", new ExpressionFactoryHelper<OsPlatformExp>());
            Register("osversion", "osversion() api", new ExpressionFactoryHelper<OsVersionExp>());
            Register("getfullpath", "getfullpath(path) api", new ExpressionFactoryHelper<GetFullPathExp>());
            Register("getpathroot", "getpathroot(path) api", new ExpressionFactoryHelper<GetPathRootExp>());
            Register("getrandomfilename", "getrandomfilename() api", new ExpressionFactoryHelper<GetRandomFileNameExp>());
            Register("gettempfilename", "gettempfilename() api", new ExpressionFactoryHelper<GetTempFileNameExp>());
            Register("gettemppath", "gettemppath() api", new ExpressionFactoryHelper<GetTempPathExp>());
            Register("hasextension", "hasextension(path) api", new ExpressionFactoryHelper<HasExtensionExp>());
            Register("ispathrooted", "ispathrooted(path) api", new ExpressionFactoryHelper<IsPathRootedExp>());
            Register("getfilename", "getfilename(path) api", new ExpressionFactoryHelper<GetFileNameExp>());
            Register("getfilenamewithoutextension", "getfilenamewithoutextension(path) api", new ExpressionFactoryHelper<GetFileNameWithoutExtensionExp>());
            Register("getextension", "getextension(path) api", new ExpressionFactoryHelper<GetExtensionExp>());
            Register("getdirectoryname", "getdirectoryname(path) api", new ExpressionFactoryHelper<GetDirectoryNameExp>());
            Register("combinepath", "combinepath(path1,path2) api", new ExpressionFactoryHelper<CombinePathExp>());
            Register("changeextension", "changeextension(path,ext) api", new ExpressionFactoryHelper<ChangeExtensionExp>());
            Register("quotepath", "quotepath(path[,only_needed,single_quote]) api", new ExpressionFactoryHelper<QuotePathExp>());
            Register("echo", "echo(fmt,arg1,arg2,...) api, Console.WriteLine", new ExpressionFactoryHelper<EchoExp>());
            Register("callstack", "callstack() api", new ExpressionFactoryHelper<CallStackExp>());
            Register("call", "call(func_name,arg1,arg2,...) api", new ExpressionFactoryHelper<CallExp>());
            Register("return", "return([val]) api", new ExpressionFactoryHelper<ReturnExp>());
            Register("redirect", "redirect(arg1,arg2,...) api", new ExpressionFactoryHelper<RedirectExp>());

            Register("fileecho", "fileecho(bool) or fileecho() api", new ExpressionFactoryHelper<FileEchoExp>());
            Register("direxist", "direxist(dir) api", new ExpressionFactoryHelper<DirectoryExistExp>());
            Register("fileexist", "fileexist(file) api", new ExpressionFactoryHelper<FileExistExp>());
            Register("listdirs", "listdirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListDirectoriesExp>());
            Register("listfiles", "listfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListFilesExp>());
            Register("listalldirs", "listalldirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllDirectoriesExp>());
            Register("listallfiles", "listallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllFilesExp>());
            Register("createdir", "createdir(dir) api", new ExpressionFactoryHelper<CreateDirectoryExp>());
            Register("copydir", "copydir(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<CopyDirectoryExp>());
            Register("movedir", "movedir(dir1,dir2) api", new ExpressionFactoryHelper<MoveDirectoryExp>());
            Register("deletedir", "deletedir(dir) api", new ExpressionFactoryHelper<DeleteDirectoryExp>());
            Register("copyfile", "copyfile(file1,file2) api", new ExpressionFactoryHelper<CopyFileExp>());
            Register("copyfiles", "copyfiles(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<CopyFilesExp>());
            Register("movefile", "movefile(file1,file2) api", new ExpressionFactoryHelper<MoveFileExp>());
            Register("deletefile", "deletefile(file) api", new ExpressionFactoryHelper<DeleteFileExp>());
            Register("deletefiles", "deletefiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<DeleteFilesExp>());
            Register("deleteallfiles", "deleteallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<DeleteAllFilesExp>());
            Register("getfileinfo", "getfileinfo(file) api", new ExpressionFactoryHelper<GetFileInfoExp>());
            Register("getdirinfo", "getdirinfo(dir) api", new ExpressionFactoryHelper<GetDirectoryInfoExp>());
            Register("getdriveinfo", "getdriveinfo(drive) api", new ExpressionFactoryHelper<GetDriveInfoExp>());
            Register("getdrivesinfo", "getdrivesinfo() api", new ExpressionFactoryHelper<GetDrivesInfoExp>());
            Register("readalllines", "readalllines(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllLinesExp>());
            Register("writealllines", "writealllines(file,lines[,encoding]) api", new ExpressionFactoryHelper<WriteAllLinesExp>());
            Register("readalltext", "readalltext(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllTextExp>());
            Register("writealltext", "writealltext(file,txt[,encoding]) api", new ExpressionFactoryHelper<WriteAllTextExp>());
            Register("process", "process(file,arg_str) or process(file,arg_str){[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("command", "command{win{:cmd_str:};unix{:cmd_str:};common{:cmd_str:};[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("kill", "kill(name_or_pid) api", new ExpressionFactoryHelper<KillExp>());
            Register("killme", "killme([exit_code]) api", new ExpressionFactoryHelper<KillMeExp>());
            Register("pid", "pid() api", new ExpressionFactoryHelper<GetCurrentProcessIdExp>());
            Register("plist", "plist([filter]) api, return list", new ExpressionFactoryHelper<ListProcessesExp>());
            Register("wait", "wait(time) api", new ExpressionFactoryHelper<WaitExp>());
            Register("waitall", "waitall([timeout]) api, wait all task to exit", new ExpressionFactoryHelper<WaitAllExp>());
            Register("waitstartinterval", "waitstartinterval(time) or waitstartinterval() api, used in Task.Wait for process/command", new ExpressionFactoryHelper<WaitStartIntervalExp>());
            Register("cleanupcompletedtasks", "cleanupcompletedtasks() api", new ExpressionFactoryHelper<CleanupCompletedTasksExp>());
            Register("gettaskcount", "gettaskcount() api", new ExpressionFactoryHelper<GetTaskCountExp>());
            Register("calcmd5", "calcmd5(file) api", new ExpressionFactoryHelper<CalcMd5Exp>());
        }
        public void Register(string name, string doc, IExpressionFactory factory)
        {
            if (m_ApiFactories.ContainsKey(name)) {
                m_ApiFactories[name] = factory;
            }
            else {
                m_ApiFactories.Add(name, factory);
            }
            if (m_ApiDocs.ContainsKey(name)) {
                m_ApiDocs[name] = doc;
            }
            else {
                m_ApiDocs.Add(name, doc);
            }
        }
        public SortedList<string, string> ApiDocs
        {
            get { return m_ApiDocs; }
        }
        public void Clear()
        {
            m_Funcs.Clear();
            m_Stack.Clear();
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
            string path = dslFile;
            if (file.Load(path, OnLog)) {
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
            if (null != func) {
                if (func.IsHighOrder)
                    id = func.LowerOrderFunction.GetParamId(0);
                else
                    return;
            }
            else {
                var statement = info as Dsl.StatementData;
                if (null != statement && statement.GetFunctionNum() == 2) {
                    id = statement.First.AsFunction.GetParamId(0);
                    func = statement.Second.AsFunction;
                    if (func.GetId() == "args" && func.IsHighOrder) {
                        if (func.LowerOrderFunction.GetParamNum() > 0) {
                            funcInfo = new FuncInfo();
                            foreach (var p in func.LowerOrderFunction.Params) {
                                string argName = p.GetId();
                                int ix = funcInfo.LocalVarIndexes.Count;
                                funcInfo.LocalVarIndexes.Add(argName, -1 - ix);
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
            m_Funcs[id] = funcInfo;
        }
        public void LoadDsl(string func, Dsl.FunctionData dslFunc)
        {
            LoadDsl(func, null, dslFunc);
        }
        public void LoadDsl(string func, IList<string> argNames, Dsl.FunctionData dslFunc)
        {
            FuncInfo funcInfo = null;
            if (null != argNames && argNames.Count > 0) {
                funcInfo = new FuncInfo();
                foreach (var argName in argNames) {
                    int ix = funcInfo.LocalVarIndexes.Count;
                    funcInfo.LocalVarIndexes.Add(argName, -1 - ix);
                }
            }
            if (null == funcInfo)
                funcInfo = new FuncInfo();
            LoadDsl(dslFunc.Params, funcInfo.Codes);
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
        public void CheckFuncXrefs()
        {
            foreach(var func in m_FuncCalls) {
                if (!m_Funcs.ContainsKey(func.FuncName)) {
                    //error
                    Log("DslCalculator error, unknown func '{0}', {1} line {2}", func.FuncName, func.SyntaxComponent.ToScriptString(false), func.SyntaxComponent.GetLine());
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
            try {
                return CalcInCurrentContext(funcInfo.Codes);
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
        public T GetFuncContext<T>() where T : class
        {
            var stackInfo = m_Stack.Peek();
            return stackInfo.FuncContext as T;
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
                if (v[0] == '@') {
                    ret = TryGetGlobalVariable(v, out result);
                }
                else if (v[0] == '$') {
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
                if (v[0] == '@') {
                    result = GetGlobalVariable(v);
                }
                else if (v[0] == '$') {
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
                if (v[0] == '@') {
                    SetGlobalVariable(v, val);
                }
                else if (v[0] == '$') {
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
                        fd.SetParenthesisParamClass();
                        if (!p.Load(fd, this)) {
                            //error
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
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
                        if (!callData.HaveId() && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS || callData.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                            switch (callData.GetParamClass()) {
                                case (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS:
                                    int num = callData.GetParamNum();
                                    if (num == 1) {
                                        Dsl.ISyntaxComponent param = callData.GetParam(0);
                                        return Load(param);
                                    }
                                    else {
                                        ParenthesisExp exp = new ParenthesisExp();
                                        exp.Load(comp, this);
                                        return exp;
                                    }
                                case (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET: {
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
                            if (op == "=") {//assignment
                                Dsl.FunctionData innerCall = callData.GetParam(0) as Dsl.FunctionData;
                                if (null != innerCall) {
                                    //obj.property = val -> dotnetset(obj, property, val)
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                      innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        if (innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD)
                                            newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                                        else
                                            newCall.Name = new Dsl.ValueData("collectionset", Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesisParamClass();
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
                                    Log("DslCalculator error, {0} line {1}", callData.ToScriptString(false), callData.GetLine());
                                }
                                return exp;
                            }
                            else {
                                if (callData.IsHighOrder) {
                                    Dsl.FunctionData innerCall = callData.LowerOrderFunction;
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS && (
                                        innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                        innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                                        //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                        string apiName;
                                        string member = innerCall.GetParamId(0);
                                        if (member == "orderby" || member == "orderbydesc" || member == "where" || member == "top") {
                                            apiName = "linq";
                                        }
                                        else if (innerParamClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD) {
                                            apiName = "dotnetcall";
                                        }
                                        else {
                                            apiName = "collectioncall";
                                        }
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesisParamClass();
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
                                if (paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                  paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                    //obj.property or obj[property] or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                    Dsl.FunctionData newCall = new Dsl.FunctionData();
                                    if (paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD)
                                        newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                    else
                                        newCall.Name = new Dsl.ValueData("collectionget", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParenthesisParamClass();
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
                    if (null != funcData && !funcData.IsHighOrder) {
                        var fc = new FunctionCall();
                        m_FuncCalls.Add(fc);
                        ret = fc;
                    }
                    else {
                        //error
                        Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
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
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                        }
                        return ret;
                    }
                }
                if (!ret.Load(comp, this)) {
                    //error
                    Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                }
            }
            return ret;
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
            if (null != pvd && pvd.IsId() && (paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD
                || paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_POINTER)) {
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
            if (m_ApiFactories.TryGetValue(name, out factory)) {
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

        private class StackInfo
        {
            internal FuncInfo FuncInfo = null;
            internal object FuncContext = null;
            internal List<BoxedValue> Args = new List<BoxedValue>();
            internal List<BoxedValue> LocalVars = new List<BoxedValue>();

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

                s_Pool.Recycle(this);
            }
            internal static StackInfo New()
            {
                return s_Pool.Alloc();
            }
            private static SimpleObjectPool<StackInfo> s_Pool = new SimpleObjectPool<StackInfo>();
        }

        private bool m_Inited = false;
        private RunStateEnum m_RunState = RunStateEnum.Normal;
        private Dictionary<string, FuncInfo> m_Funcs = new Dictionary<string, FuncInfo>();
        private Stack<StackInfo> m_Stack = new Stack<StackInfo>();
        private Dictionary<string, int> m_NamedGlobalVariableIndexes = new Dictionary<string, int>();
        private List<BoxedValue> m_GlobalVariables = new List<BoxedValue>();
        private Dictionary<string, IExpressionFactory> m_ApiFactories = new Dictionary<string, IExpressionFactory>();
        private SortedList<string, string> m_ApiDocs = new SortedList<string, string>();
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
                psi.CreateNoWindow = !option.NewWindow;
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
            internal bool NewWindow = false;
            internal bool ErrorDialog = false;
            internal string WorkingDirectory = Environment.CurrentDirectory;
        }

        private static List<Task<int>> s_Tasks = new List<Task<int>>();
        private static int s_CheckStartInterval = 500;
        private static bool s_FileEchoOn = false;
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
