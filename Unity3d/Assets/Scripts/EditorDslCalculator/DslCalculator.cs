using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using GameFramework;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
#region 解释器
namespace Expression
{
    public interface IExpression
    {
        object Calc();
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
        public object Calc()
        {
            object ret = null;
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
                Dsl.CallData callData = dsl as Dsl.CallData;
                if (null != callData) {
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
                    Dsl.FunctionData funcData = dsl as Dsl.FunctionData;
                    if (null != funcData) {
                        return Load(funcData);
                    }
                    else {
                        Dsl.StatementData statementData = dsl as Dsl.StatementData;
                        if (null != statementData) {
                            return Load(statementData);
                        }
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
        protected virtual bool Load(Dsl.CallData callData) { return false; }
        protected virtual bool Load(IList<IExpression> exps) { return false; }
        protected virtual bool Load(Dsl.FunctionData funcData) { return false; }
        protected virtual bool Load(Dsl.StatementData statementData) { return false; }
        protected abstract object DoCalc();

        protected DslCalculator Calculator
        {
            get { return m_Calculator; }
        }

        private DslCalculator m_Calculator = null;
        private Dsl.ISyntaxComponent m_Dsl = null;

        protected static double ToDouble(object v)
        {
            if (null == v)
                return 0;
            return (double)Convert.ChangeType(v, typeof(double));
        }
        protected static long ToLong(object v)
        {
            if (null == v)
                return 0;
            return (long)Convert.ChangeType(v, typeof(long));
        }
        protected static float ToFloat(object v)
        {
            if (null == v)
                return 0;
            return (float)Convert.ChangeType(v, typeof(float));
        }
        protected static int ToInt(object v)
        {
            if (null == v)
                return 0;
            return (int)Convert.ChangeType(v, typeof(int));
        }
        protected static string ToString(object v)
        {
            if (null == v)
                return string.Empty;
            return v.ToString();
        }
        protected static T CastTo<T>(object obj)
        {
            if (null == obj)
                return default(T);
            if (obj is T) {
                return (T)obj;
            }
            else {
                try {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                catch {
                    return default(T);
                }
            }
        }
        protected static object CastTo(Type t, object obj)
        {
            if (null == obj)
                return null;
            Type st = obj.GetType();
            if (t.IsAssignableFrom(st) || st.IsSubclassOf(t)) {
                return obj;
            }
            else {
                try {
                    return Convert.ChangeType(obj, t);
                }
                catch {
                    return null;
                }
            }
        }
        protected static Encoding GetEncoding(object v)
        {
            var name = v as string;
            if (null != name) {
                return Encoding.GetEncoding(name);
            }
            else if (v is int) {
                int codePage = (int)Convert.ChangeType(v, typeof(int));
                return Encoding.GetEncoding(codePage);
            }
            else {
                return Encoding.UTF8;
            }
        }
    }
    public abstract class SimpleExpressionBase : AbstractExpression
    {
        protected override object DoCalc()
        {
            List<object> operands = new List<object>();
            for (int i = 0; i < m_Exps.Count; ++i) {
                object v = m_Exps[i].Calc();
                operands.Add(v);
            }
            return OnCalc(operands);
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Exps = exps;
            return true;
        }
        protected abstract object OnCalc(IList<object> operands);

        private IList<IExpression> m_Exps = null;
    }
    internal sealed class ArgsGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = Calculator.Arguments;
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            return true;
        }
    }
    internal sealed class ArgGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = null;
            var ix = (int)Convert.ChangeType(m_ArgIndex.Calc(), typeof(int));
            var args = Calculator.Arguments;
            if (ix >= 0 && ix < args.Count) {
                ret = args[ix];
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_ArgIndex = Calculator.Load(callData.GetParam(0));
            return true;
        }

        private IExpression m_ArgIndex;
    }
    internal sealed class ArgNumGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = Calculator.Arguments.Count;
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            return true;
        }
    }
    internal sealed class VarSet : AbstractExpression
    {
        protected override object DoCalc()
        {
            var varId = m_VarId.Calc();
            object v = m_Op.Calc();
            if (varId is int) {
                int id = (int)Convert.ChangeType(varId, typeof(int));
                Calculator.SetVariable(id, v);
            }
            else {
                var str = varId as string;
                if (null != str) {
                    Calculator.SetVariable(str, v);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.CallData param1 = callData.GetParam(0) as Dsl.CallData;
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_VarId = Calculator.Load(param1.GetParam(0));
            m_Op = Calculator.Load(param2);
            return true;
        }

        private IExpression m_VarId;
        private IExpression m_Op;
    }
    internal sealed class VarGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            var varId = m_VarId.Calc();
            object v = null;
            if (varId is int) {
                int id = (int)Convert.ChangeType(varId, typeof(int));
                v = Calculator.GetVariable(id);
            }
            else {
                var str = varId as string;
                if (null != str) {
                    v = Calculator.GetVariable(str);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_VarId = Calculator.Load(callData.GetParam(0));
            return true;
        }

        private IExpression m_VarId;
    }
    internal sealed class NamedVarSet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object v = m_Op.Calc();
            if (m_VarId.Length > 0) {
                Calculator.SetVariable(m_VarId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_VarId = param1.GetId();
            m_Op = Calculator.Load(param2);
            return true;
        }

        private string m_VarId;
        private IExpression m_Op;
    }
    internal sealed class NamedVarGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = 0;
            if (m_VarId == "break") {
                Calculator.RunState = RunStateEnum.Break;
            }
            else if (m_VarId == "continue") {
                Calculator.RunState = RunStateEnum.Continue;
            }
            else if (m_VarId.Length > 0) {
                ret = Calculator.GetVariable(m_VarId);
            }
            return ret;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_VarId = valData.GetId();
            return true;
        }

        private string m_VarId;
    }
    internal sealed class ConstGet : AbstractExpression
    {
        protected override object DoCalc()
        {
            object v = m_Val;
            return v;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            string id = valData.GetId();
            int idType = valData.GetIdType();
            if (idType == Dsl.ValueData.NUM_TOKEN) {
                if (id.StartsWith("0x")) {
                    long v = long.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    if (v >= int.MinValue && v <= int.MaxValue) {
                        m_Val = (int)v;
                    }
                    else {
                        m_Val = v;
                    }
                }
                else if (id.IndexOf('.') < 0) {
                    long v = long.Parse(id);
                    if (v >= int.MinValue && v <= int.MaxValue) {
                        m_Val = (int)v;
                    }
                    else {
                        m_Val = v;
                    }
                }
                else {
                    double v = double.Parse(id);
                    if (v >= float.MinValue && v <= float.MaxValue) {
                        m_Val = (float)v;
                    }
                    else {
                        m_Val = v;
                    }
                }
            }
            else if (idType == Dsl.ValueData.BOOL_TOKEN) {
                m_Val = id == "true";
            }
            else {
                m_Val = id;
            }
            return true;
        }

        private object m_Val;
    }
    internal sealed class AddExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v;
            if (v1 is string || v2 is string) {
                v = ToString(v1) + ToString(v2);
            }
            else {
                v = ToDouble(v1) + ToDouble(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToDouble(v1) - ToDouble(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToDouble(v1) * ToDouble(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToDouble(v1) / ToDouble(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToDouble(v1) % ToDouble(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToLong(v1) & ToLong(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToLong(v1) | ToLong(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToLong(v1) ^ ToLong(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v = ~ToLong(v1);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToLong(v1) << ToInt(v2);
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToLong(v1) >> ToInt(v2);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 >= v2 ? v1 : v2;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 <= v2 ? v1 : v2;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = v1 >= 0 ? v1 : -v1;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Sin((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Cos((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Tan((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Asin((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Acos((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Atan((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = (double)Mathf.Atan2((float)v1, (float)v2);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = Math.Sinh(v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = Math.Cosh(v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = Math.Tanh(v1);
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
        protected override object DoCalc()
        {
            long v1 = ToLong(m_Op1.Calc());
            long v2 = ToLong(m_Op2.Calc());
            object v = (long)UnityEngine.Random.Range((int)v1, (int)v2);
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
    internal sealed class RndFloatExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = (double)UnityEngine.Random.Range((float)v1, (float)v2);
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
    internal sealed class PowExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = (double)Mathf.Pow((float)v1, (float)v2);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            object v = (double)Mathf.Sqrt((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            object v = (double)Mathf.Log((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            object v = (double)Mathf.Log10((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            object v = (double)Mathf.Floor((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CeilExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            object v = (double)Mathf.Ceil((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            double v3 = ToDouble(m_Op3.Calc());
            object v = (double)Mathf.Lerp((float)v1, (float)v2, (float)v3);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            double v3 = ToDouble(m_Op3.Calc());
            object v = (double)Mathf.LerpUnclamped((float)v1, (float)v2, (float)v3);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            double v3 = ToDouble(m_Op3.Calc());
            object v = (double)Mathf.LerpAngle((float)v1, (float)v2, (float)v3);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op.Calc());
            object v = (double)Mathf.Clamp01((float)v1);
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            double v3 = ToDouble(m_Op3.Calc());
            object v;
            if (v3 < v1)
                v = v1;
            else if (v3 > v2)
                v = v2;
            else
                v = v3;
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
    internal sealed class ApproximatelyExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            float v1 = ToFloat(m_Op1.Calc());
            float v2 = ToFloat(m_Op2.Calc());
            object v = Mathf.Approximately(v1, v2) ? 1 : 0;
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
    internal sealed class IsPowerOfTwoExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            int v1 = ToInt(m_Op1.Calc());
            int v = Mathf.IsPowerOfTwo(v1) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class ClosestPowerOfTwoExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            int v1 = ToInt(m_Op1.Calc());
            int v = Mathf.ClosestPowerOfTwo(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class NextPowerOfTwoExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            int v1 = ToInt(m_Op1.Calc());
            int v = Mathf.NextPowerOfTwo(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class DistExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            float x1 = (float)ToDouble(m_Op1.Calc());
            float y1 = (float)ToDouble(m_Op2.Calc());
            float x2 = (float)ToDouble(m_Op3.Calc());
            float y2 = (float)ToDouble(m_Op4.Calc());
            object v = Geometry.Distance(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
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
        protected override object DoCalc()
        {
            float x1 = (float)ToDouble(m_Op1.Calc());
            float y1 = (float)ToDouble(m_Op2.Calc());
            float x2 = (float)ToDouble(m_Op3.Calc());
            float y2 = (float)ToDouble(m_Op4.Calc());
            object v = Geometry.DistanceSquare(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 > v2 ? 1 : 0;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 >= v2 ? 1 : 0;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 < v2 ? 1 : 0;
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
        protected override object DoCalc()
        {
            double v1 = ToDouble(m_Op1.Calc());
            double v2 = ToDouble(m_Op2.Calc());
            object v = v1 <= v2 ? 1 : 0;
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToString(v1) == ToString(v2) ? 1 : 0;
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = m_Op2.Calc();
            object v = ToString(v1) != ToString(v2) ? 1 : 0;
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
        protected override object DoCalc()
        {
            long v1 = ToLong(m_Op1.Calc());
            long v2 = 0;
            object v = v1 != 0 && (v2 = ToLong(m_Op2.Calc())) != 0 ? 1 : 0;
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
        protected override object DoCalc()
        {
            long v1 = ToLong(m_Op1.Calc());
            long v2 = 0;
            object v = v1 != 0 || (v2 = ToLong(m_Op2.Calc())) != 0 ? 1 : 0;
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
        protected override object DoCalc()
        {
            long val = ToLong(m_Op.Calc());
            object v = val == 0 ? 1 : 0;
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
        protected override object DoCalc()
        {
            object v1 = m_Op1.Calc();
            object v2 = null;
            object v3 = null;
            object v = ToLong(v1) != 0 ? v2 = m_Op2.Calc() : v3 = m_Op3.Calc();
            return v;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            Dsl.FunctionData funcData1 = statementData.First;
            Dsl.FunctionData funcData2 = statementData.Second;
            if (funcData2.GetId() == ":") {
                Dsl.ISyntaxComponent cond = funcData1.Call.GetParam(0);
                Dsl.ISyntaxComponent op1 = funcData1.GetStatement(0);
                Dsl.ISyntaxComponent op2 = funcData2.GetStatement(0);
                m_Op1 = Calculator.Load(cond);
                m_Op2 = Calculator.Load(op1);
                m_Op3 = Calculator.Load(op2);
            }
            else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", statementData.ToScriptString(false), statementData.GetLine());
            }
            return true;
        }

        private IExpression m_Op1 = null;
        private IExpression m_Op2 = null;
        private IExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    object condVal = clause.Condition.Calc();
                    if (ToLong(condVal) != 0) {
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
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            IfExp.Clause item = new IfExp.Clause();
            item.Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                item.Expressions.Add(subExp);
            }
            m_Clauses.Add(item);
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //简化语法if(exp) func(args);语法的处理
            int funcNum = statementData.GetFunctionNum();
            if (funcNum == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "if" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    IfExp.Clause item = new IfExp.Clause();
                    if (first.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.Call.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
                    }
                    IExpression subExp = Calculator.Load(second);

                    item.Expressions.Add(subExp);
                    m_Clauses.Add(item);
                    return true;
                }
            }
            //标准if语句的处理
            foreach (var fData in statementData.Functions) {
                if (fData.GetId() == "if" || fData.GetId() == "elseif") {
                    IfExp.Clause item = new IfExp.Clause();
                    if (fData.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = fData.Call.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    }
                    for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                        IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                        item.Expressions.Add(subExp);
                    }
                    m_Clauses.Add(item);
                }
                else if (fData.GetId() == "else") {
                    if (fData != statementData.Last) {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    }
                    else {
                        IfExp.Clause item = new IfExp.Clause();
                        for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                            IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                            item.Expressions.Add(subExp);
                        }
                        m_Clauses.Add(item);
                    }
                }
                else {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
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
        protected override object DoCalc()
        {
            object v = 0;
            for (; ; ) {
                object condVal = m_Condition.Calc();
                if (ToLong(condVal) != 0) {
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
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            m_Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //简化语法while(exp) func(args);语法的处理
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "while" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = first.Call.GetParam(0);
                        m_Condition = Calculator.Load(cond);
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
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
        protected override object DoCalc()
        {
            object v = 0;
            object count = m_Count.Calc();
            long ct = ToLong(count);
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
            Dsl.ISyntaxComponent count = funcData.Call.GetParam(0);
            m_Count = Calculator.Load(count);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //简化语法loop(exp) func(args);语法的处理
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "loop" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.Call.GetParam(0);
                        m_Count = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
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
        protected override object DoCalc()
        {
            object v = 0;
            object list = m_List.Calc();
            IEnumerable obj = list as IEnumerable;
            if (null != obj) {
                IEnumerator enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    object val = enumer.Current;
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
            Dsl.ISyntaxComponent list = funcData.Call.GetParam(0);
            m_List = Calculator.Load(list);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //简化语法looplist(exp) func(args);语法的处理
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "looplist" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    if (first.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent exp = first.Call.GetParam(0);
                        m_List = Calculator.Load(exp);
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
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
        protected override object DoCalc()
        {
            object v = 0;
            List<object> list = new List<object>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                object val = m_Elements[ix].Calc();
                list.Add(val);
            }
            IEnumerator enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                object val = enumer.Current;
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
            Dsl.CallData callData = funcData.Call;
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent exp = funcData.Call.GetParam(ix);
                m_Elements.Add(Calculator.Load(exp));
            }
            int fnum = funcData.GetStatementNum();
            for (int ix = 0; ix < fnum; ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            //简化语法foreach(exp1,exp2,...) func(args);语法的处理
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                var firstId = first.GetId();
                var secondId = second.GetId();
                if (firstId == "foreach" && !first.HaveStatement() && !first.HaveExternScript() &&
                        !string.IsNullOrEmpty(secondId) && !second.HaveStatement() && !second.HaveExternScript()) {
                    int num = first.Call.GetParamNum();
                    if (num > 0) {
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent exp = first.Call.GetParam(ix);
                            m_Elements.Add(Calculator.Load(exp));
                        }
                    }
                    else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", first.ToScriptString(false), first.GetLine());
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
        protected override object DoCalc()
        {
            object v = 0;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc();
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object v = 0;
            string fmt = string.Empty;
            ArrayList al = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc();
                if (ix == 0)
                    fmt = v as string;
                else
                    al.Add(v);
            }
            v = string.Format(fmt, al.ToArray());
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().AssemblyQualifiedName;
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().FullName;
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                try {
                    ret = obj.GetType().Name;
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                string type = m_Expressions[0].Calc() as string;
                try {
                    ret = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == ret) {
                        ret = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == ret) {
                        ret = Type.GetType("UnityEditor." + type + ", UnityEditor");
                    }
                    if (null == ret) {
                        ret = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null == ret) {
                        ret = Type.GetType(type);
                    }
                    if (null == ret) {
                        Debug.LogWarningFormat("null == Type.GetType({0})", type);
                    }
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 2) {
                object obj = m_Expressions[0].Calc();
                string type = m_Expressions[1].Calc() as string;
                try {
                    if (0 == type.CompareTo("sbyte")) {
                        ret = CastTo<sbyte>(obj);
                    }
                    else if (0 == type.CompareTo("byte")) {
                        ret = CastTo<byte>(obj);
                    }
                    else if (0 == type.CompareTo("short")) {
                        ret = CastTo<short>(obj);
                    }
                    else if (0 == type.CompareTo("ushort")) {
                        ret = CastTo<ushort>(obj);
                    }
                    else if (0 == type.CompareTo("int")) {
                        ret = CastTo<int>(obj);
                    }
                    else if (0 == type.CompareTo("uint")) {
                        ret = CastTo<uint>(obj);
                    }
                    else if (0 == type.CompareTo("long")) {
                        ret = CastTo<long>(obj);
                    }
                    else if (0 == type.CompareTo("ulong")) {
                        ret = CastTo<ulong>(obj);
                    }
                    else if (0 == type.CompareTo("float")) {
                        ret = CastTo<float>(obj);
                    }
                    else if (0 == type.CompareTo("double")) {
                        ret = CastTo<double>(obj);
                    }
                    else if (0 == type.CompareTo("string")) {
                        ret = CastTo<string>(obj);
                    }
                    else if (0 == type.CompareTo("bool")) {
                        ret = CastTo<bool>(obj);
                    }
                    else {
                        Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                        if (null == t) {
                            t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                        }
                        if (null == t) {
                            t = Type.GetType("UnityEditor." + type + ", UnityEditor");
                        }
                        if (null == t) {
                            t = Type.GetType(type + ", Assembly-CSharp");
                        }
                        if (null == t) {
                            t = Type.GetType(type);
                        }
                        if (null != t) {
                            ret = Convert.ChangeType(obj, t);
                        }
                        else {
                            Debug.LogWarningFormat("null == Type.GetType({0})", type);
                        }
                    }
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 2) {
                string type = m_Expressions[0].Calc() as string;
                string val = m_Expressions[1].Calc() as string;
                try {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType("UnityEditor." + type + ", UnityEditor");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null == t) {
                        t = Type.GetType(type);
                    }
                    if (null != t) {
                        ret = Enum.Parse(t, val, true);
                    }
                    else {
                        Debug.LogWarningFormat("null == Type.GetType({0})", type);
                    }
                }
                catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            object obj = null;
            object methodObj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v;
                }
                else if (ix == 1) {
                    methodObj = v;
                    method = v as string;
                }
                else {
                    arglist.Add(v);
                }
            }
            object[] _args = arglist.ToArray();
            if (null != obj) {
                if (null != method) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
                        var d = dict[method] as Delegate;
                        if (null != d) {
                            ret = d.DynamicInvoke();
                        }
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                Converter.CastArgsForCall(t, method, flags, _args);
                                ret = t.InvokeMember(method, flags, null, null, _args);
                            }
                            catch (Exception ex) {
                                Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                    Converter.CastArgsForCall(t, method, flags, _args);
                                    ret = t.InvokeMember(method, flags, null, obj, _args);
                                }
                                catch (Exception ex) {
                                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                else if (null != methodObj) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(methodObj)) {
                        var d = dict[methodObj] as Delegate;
                        if (null != d) {
                            ret = d.DynamicInvoke();
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
                                ret = d.DynamicInvoke();
                            }
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetSetExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = null;
            object obj = null;
            object methodObj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v;
                }
                else if (ix == 1) {
                    methodObj = v;
                    method = v as string;
                }
                else {
                    arglist.Add(v);
                }
            }
            object[] _args = arglist.ToArray();
            if (null != obj) {
                if (null != method) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && null == obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic)) {
                        dict[method] = _args[0];
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                Converter.CastArgsForSet(t, method, flags, _args);
                                ret = t.InvokeMember(method, flags, null, null, _args);
                            }
                            catch (Exception ex) {
                                Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                    Converter.CastArgsForSet(t, method, flags, _args);
                                    ret = t.InvokeMember(method, flags, null, obj, _args);
                                }
                                catch (Exception ex) {
                                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                else if (null != methodObj) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(methodObj)) {
                        dict[methodObj] = _args[0];
                    }
                    else {
                        IList list = obj as IList;
                        if (null != list && methodObj is int) {
                            int index = (int)methodObj;
                            if (index >= 0 && index < list.Count) {
                                list[index] = _args[0];
                            }
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetGetExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = null;
            object obj = null;
            object methodObj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc();
                if (ix == 0) {
                    obj = v;
                }
                else if (ix == 1) {
                    methodObj = v;
                    method = v as string;
                }
                else {
                    arglist.Add(v);
                }
            }
            object[] _args = arglist.ToArray();
            if (null != obj) {
                if (null != method) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(method)) {
                        ret = dict[method];
                    }
                    else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                Converter.CastArgsForGet(t, method, flags, _args);
                                ret = t.InvokeMember(method, flags, null, null, _args);
                            }
                            catch (Exception ex) {
                                Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                    Converter.CastArgsForGet(t, method, flags, _args);
                                    ret = t.InvokeMember(method, flags, null, obj, _args);
                                }
                                catch (Exception ex) {
                                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                else if (null != methodObj) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(methodObj)) {
                        ret = dict[methodObj];
                    }
                    else {
                        IEnumerable enumer = obj as IEnumerable;
                        if (null != enumer && methodObj is int) {
                            int index = (int)methodObj;
                            var e = enumer.GetEnumerator();
                            for (int i = 0; i <= index; ++i) {
                                e.MoveNext();
                            }
                            ret = e.Current;
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object v = 0;
            object list = m_List.Calc();
            string method = m_Method.Calc() as string;
            IEnumerable obj = list as IEnumerable;
            if (null != obj && !string.IsNullOrEmpty(method)) {
                if (method == "orderby" || method == "orderbydesc") {
                    bool desc = method == "orderbydesc";
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;
                        results.Add(val);
                    }
                    results.Sort((object o1, object o2) => {
                        Calculator.SetVariable("$$", o1);
                        object r1 = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r1 = m_Expressions[index].Calc();
                        }
                        Calculator.SetVariable("$$", o2);
                        object r2 = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r2 = m_Expressions[index].Calc();
                        }
                        string rs1 = r1 as string;
                        string rs2 = r2 as string;
                        int r = 0;
                        if (null != rs1 && null != rs2) {
                            r = rs1.CompareTo(rs2);
                        }
                        else {
                            double rd1 = ToDouble(r1);
                            double rd2 = ToDouble(r2);
                            r = rd1.CompareTo(rd2);
                        }
                        if (desc)
                            r = -r;
                        return r;
                    });
                    v = results;
                }
                else if (method == "where") {
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;

                        Calculator.SetVariable("$$", val);
                        object r = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r = m_Expressions[index].Calc();
                        }
                        if (ToLong(r) != 0) {
                            results.Add(val);
                        }
                    }
                    v = results;
                }
                else if (method == "top") {
                    object r = null;
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        r = m_Expressions[index].Calc();
                    }
                    long ct = ToLong(r);
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;
                        if (ct > 0) {
                            results.Add(val);
                            --ct;
                        }
                    }
                    v = results;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
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
        protected override object DoCalc()
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc();
                UnityEngine.Object uo = obj as UnityEngine.Object;
                if (!System.Object.ReferenceEquals(null, uo))
                    ret = null == uo;
                else
                    ret = null == obj;
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal class AssetPath2GUIDExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = string.Empty;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var assetPath = operands[0] as string;
                if (null != assetPath) {
                    r = AssetDatabase.AssetPathToGUID(assetPath);
                }
            }
#endif
            return r;
        }
    }
    internal class GUID2AssetPathExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = string.Empty;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var guid = operands[0] as string;
                if (null != guid) {
                    r = AssetDatabase.GUIDToAssetPath(guid);
                }
            }
#endif
            return r;
        }
    }
    internal class GetAssetPathExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    var pobj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                    if (null != pobj)
                        r = AssetDatabase.GetAssetPath(pobj);
                    else
                        r = AssetDatabase.GetAssetPath(obj);
                }
            }
#endif
            return r;
        }
    }
    internal class GetDependenciesExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var str = operands[i] as string;
                    if (null != str) {
                        list.Add(str);
                    }
                    else {
                        var strList = operands[i] as IList<string>;
                        if (null != strList) {
                            list.AddRange(strList);
                        }
                    }
                }
                if (list.Count == 1) {
                    ret = AssetDatabase.GetDependencies(list[0]);
                }
                else if (list.Count > 1) {
                    ret = AssetDatabase.GetDependencies(list.ToArray());
                }
            }
#endif
            return ret;
        }
    }
    internal class GetAssetImporterExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    r = AssetImporter.GetAtPath(path);
                }
            }
#endif
            return r;
        }
    }
    internal class LoadAssetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    r = AssetDatabase.LoadMainAssetAtPath(path);
                }
            }
#endif
            return r;
        }
    }
    internal class UnloadAssetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    Resources.UnloadAsset(obj);
                }
            }
            return r;
        }
    }
    internal class GetPrefabTypeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    r = PrefabUtility.GetPrefabType(obj);
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabObjectExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    r = PrefabUtility.GetPrefabObject(obj);
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabParentExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    r = PrefabUtility.GetPrefabParent(obj);
                }
            }
#endif
            return r;
        }
    }
    internal class DestroyObjectExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                bool modifyAsset = false;
                if (operands.Count >= 2) {
                    modifyAsset = (bool)Convert.ChangeType(operands[1], typeof(bool));
                }
                if (null != obj) {
                    UnityEngine.Object.DestroyImmediate(obj, modifyAsset);
                    r = modifyAsset;
                }
            }
            return r;
        }
    }
    internal class GetComponentExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponent(t);
                    }
                }
            }
            return r;
        }
    }
    internal class GetComponentsExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponents(t);
                    }
                }
            }
            return r;
        }
    }
    internal class GetComponentInChildrenExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponentInChildren(t);
                    }
                }
            }
            return r;
        }
    }
    internal class GetComponentsInChildrenExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponentsInChildren(t);
                    }
                }
            }
            return r;
        }
    }
    internal class NewStringBuilderExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                r = new StringBuilder();
            }
            return r;
        }
    }
    internal class AppendFormatExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var sb = operands[0] as StringBuilder;
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i] as string;
                    else
                        al.Add(operands[i]);
                }
                if (null != sb && !string.IsNullOrEmpty(fmt)) {
                    sb.AppendFormat(fmt, al.ToArray());
                    r = sb;
                }
            }
            return r;
        }
    }
    internal class AppendLineFormatExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var sb = operands[0] as StringBuilder;
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i] as string;
                    else
                        al.Add(operands[i]);
                }
                if (null != sb) {
                    if (string.IsNullOrEmpty(fmt)) {
                        sb.AppendLine();
                    }
                    else {
                        sb.AppendFormat(fmt, al.ToArray());
                        sb.AppendLine();
                    }
                    r = sb;
                }
            }
            return r;
        }
    }
    internal class StringBuilderToStringExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var sb = operands[0] as StringBuilder;
                if (null != sb) {
                    r = sb.ToString();
                }
            }
            return r;
        }
    }
    internal class StringJoinExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var sep = operands[0] as string;
                var list = operands[1] as IList;
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
    internal class StringSplitExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var str = operands[0] as string;
                var seps = operands[1] as IList;
                if (!string.IsNullOrEmpty(str) && null != seps) {
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
                    r = str.Split(cs);
                }
            }
            return r;
        }
    }
    internal class StringTrimExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = str.Trim();
            }
            return r;
        }
    }
    internal class StringTrimStartExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = str.TrimStart();
            }
            return r;
        }
    }
    internal class StringTrimEndExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = str.TrimEnd();
            }
            return r;
        }
    }
    internal class StringLowerExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = str.ToLower();
            }
            return r;
        }
    }
    internal class StringUpperExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = str.ToUpper();
            }
            return r;
        }
    }
    internal class StringReplaceExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var str = operands[0] as string;
                var key = operands[1] as string;
                var val = operands[2] as string;
                r = str.Replace(key, val);
            }
            return r;
        }
    }
    internal class StringReplaceCharExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var str = operands[0] as string;
                var key = operands[1] as string;
                var val = operands[2] as string;
                if (null != str && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                    r = str.Replace(key[0], val[0]);
                }
            }
            return r;
        }
    }
    internal class MakeStringExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < operands.Count; ++i) {
                var v = operands[i];
                var str = v as string;
                if (null != str) {
                    char c = '\0';
                    if (str.Length > 0) {
                        c = str[0];
                    }
                    chars.Add(c);
                }
                else {
                    char c = (char)Convert.ChangeType(operands[i], typeof(char));
                    chars.Add(c);
                }
            }
            return new String(chars.ToArray());
        }
    }
    internal class Str2IntExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                int v;
                if (int.TryParse(str, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal class Str2FloatExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                float v;
                if (float.TryParse(str, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal class Hex2IntExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                int v;
                if (int.TryParse(str, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal class DatetimeStrExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var fmt = operands[0] as string;
                r = DateTime.Now.ToString(fmt);
            } else {
                r = DateTime.Now.ToString();
            }
            return r;
        }
    }
    internal class LongDateStrExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = DateTime.Now.ToLongDateString();
            return r;
        }
    }
    internal class LongTimeStrExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = DateTime.Now.ToShortDateString();
            return r;
        }
    }
    internal class ShortDateStrExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = DateTime.Now.ToShortDateString();
            return r;
        }
    }
    internal class ShortTimeStrExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = DateTime.Now.ToShortTimeString();
            return r;
        }
    }
    internal class IsNullOrEmptyExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                r = string.IsNullOrEmpty(str);
            }
            return r;
        }
    }
    internal class ArrayExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object[] r = new object[operands.Count];
            for (int i = 0; i < operands.Count; ++i) {
                r[i] = operands[i];
            }
            return r;
        }
    }
    internal class ToArrayExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var list = operands[0];
                IEnumerable obj = list as IEnumerable;
                if (null != obj) {
                    ArrayList al = new ArrayList();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;
                        al.Add(val);
                    }
                    r = al.ToArray();
                }
            }
            return r;
        }
    }
    internal class ListSizeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var list = operands[0] as IList;
                if (null != list) {
                    r = list.Count;
                }
            }
            return r;
        }
    }
    internal class ListExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            ArrayList al = new ArrayList();
            for (int i = 0; i < operands.Count; ++i) {
                al.Add(operands[i]);
            }
            r = al;
            return r;
        }
    }
    internal class ListGetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object defVal = null;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        r = list[index];
                    }
                    else {
                        r = defVal;
                    }
                }
            }
            return r;
        }
    }
    internal class ListSetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object val = operands[2];
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        list[index] = val;
                    }
                }
            }
            return r;
        }
    }
    internal class ListIndexOfExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    r = list.IndexOf(val);
                }
            }
            return r;
        }
    }
    internal class ListAddExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    list.Add(val);
                }
            }
            return r;
        }
    }
    internal class ListRemoveExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    list.Remove(val);
                }
            }
            return r;
        }
    }
    internal class ListInsertExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object val = operands[2];
                if (null != list) {
                    list.Insert(index, val);
                }
            }
            return r;
        }
    }
    internal class ListRemoveAtExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                if (null != list) {
                    list.RemoveAt(index);
                }
            }
            return r;
        }
    }
    internal class ListClearExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var list = operands[0] as IList;
                if (null != list) {
                    list.Clear();
                }
            }
            return r;
        }
    }
    internal class ListSplitExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var enumer = operands[0] as IEnumerable;
                var ct = ToInt(operands[1]);
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
    internal class HashtableSizeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = operands[0] as IDictionary;
                if (null != dict) {
                    r = dict.Count;
                }
            }
            return r;
        }
    }
    internal class HashtableExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object r = null;
            Hashtable dict = new Hashtable();
            for (int i = 0; i < m_Expressions.Count - 1; i += 2) {
                var key = m_Expressions[i].Calc();
                var val = m_Expressions[i + 1].Calc();
                dict.Add(key, val);
            }
            r = dict;
            return r;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.CallData paramCallData = callData.GetParam(i) as Dsl.CallData;
                if (null != paramCallData && paramCallData.GetParamNum() == 2) {
                    var expKey = Calculator.Load(paramCallData.GetParam(0));
                    m_Expressions.Add(expKey);
                    var expVal = Calculator.Load(paramCallData.GetParam(1));
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            for (int i = 0; i < funcData.GetStatementNum(); ++i) {
                Dsl.CallData callData = funcData.GetStatement(i) as Dsl.CallData;
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
    internal class HashtableGetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var dict = operands[0] as IDictionary;
                var index = operands[1];
                object defVal = null;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != dict && dict.Contains(index)) {
                    r = dict[index];
                }
            }
            return r;
        }
    }
    internal class HashtableSetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var dict = operands[0] as IDictionary;
                var index = operands[1];
                object val = operands[2];
                if (null != dict) {
                    dict[index] = val;
                }
            }
            return r;
        }
    }
    internal class HashtableAddExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var dict = operands[0] as IDictionary;
                object key = operands[1];
                object val = operands[2];
                if (null != dict && null != key) {
                    dict.Add(key, val);
                }
            }
            return r;
        }
    }
    internal class HashtableRemoveExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var dict = operands[0] as IDictionary;
                object key = operands[1];
                if (null != dict && null != key) {
                    dict.Remove(key);
                }
            }
            return r;
        }
    }
    internal class HashtableClearExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = operands[0] as IDictionary;
                if (null != dict) {
                    dict.Clear();
                }
            }
            return r;
        }
    }
    internal class HashtableKeysExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = operands[0] as IDictionary;
                if (null != dict) {
                    var list = new ArrayList();
                    list.AddRange(dict.Keys);
                    r = list;
                }
            }
            return r;
        }
    }
    internal class HashtableValuesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = operands[0] as IDictionary;
                if (null != dict) {
                    var list = new ArrayList();
                    list.AddRange(dict.Values);
                    r = list;
                }
            }
            return r;
        }
    }
    internal class ListHashtableExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = operands[0] as IDictionary;
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
    internal class HashtableSplitExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var dict = operands[0] as IDictionary;
                var ct = ToInt(operands[1]);
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
    //stack与queue共用peek函数
    internal class PeekExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var stack = operands[0] as Stack<object>;
                var queue = operands[0] as Queue<object>;
                if (null != stack) {
                    r = stack.Peek();
                }
                else if (null != queue) {
                    r = queue.Peek();
                }
            }
            return r;
        }
    }
    internal class StackSizeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = 0;
            if (operands.Count >= 1) {
                var stack = operands[0] as Stack<object>;
                if (null != stack) {
                    r = stack.Count;
                }
            }
            return r;
        }
    }
    internal class StackExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            var stack = new Stack<object>();
            for (int i = 0; i < operands.Count; ++i) {
                stack.Push(operands[i]);
            }
            r = stack;
            return r;
        }
    }
    internal class PushExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var stack = operands[0] as Stack<object>;
                var val = operands[1];
                if (null != stack) {
                    stack.Push(val);
                }
            }
            return r;
        }
    }
    internal class PopExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var stack = operands[0] as Stack<object>;
                if (null != stack) {
                    r = stack.Pop();
                }
            }
            return r;
        }
    }
    internal class StackClearExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var stack = operands[0] as Stack<object>;
                if (null != stack) {
                    stack.Clear();
                }
            }
            return r;
        }
    }
    internal class QueueSizeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = 0;
            if (operands.Count >= 1) {
                var queue = operands[0] as Queue<object>;
                if (null != queue) {
                    r = queue.Count;
                }
            }
            return r;
        }
    }
    internal class QueueExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            var queue = new Queue<object>();
            for (int i = 0; i < operands.Count; ++i) {
                queue.Enqueue(operands[i]);
            }
            r = queue;
            return r;
        }
    }
    internal class EnqueueExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var queue = operands[0] as Queue<object>;
                var val = operands[1];
                if (null != queue) {
                    queue.Enqueue(val);
                }
            }
            return r;
        }
    }
    internal class DequeueExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var queue = operands[0] as Queue<object>;
                if (null != queue) {
                    r = queue.Dequeue();
                }
            }
            return r;
        }
    }
    internal class QueueClearExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var queue = operands[0] as Queue<object>;
                if (null != queue) {
                    queue.Clear();
                }
            }
            return r;
        }
    }
    internal class SetEnvironmentExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 2) {
                var key = operands[0] as string;
                var val = operands[1] as string;
                val = Environment.ExpandEnvironmentVariables(val);
                Environment.SetEnvironmentVariable(key, val);
            }
            return ret;
        }
    }
    internal class GetEnvironmentExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0] as string;
                return Environment.GetEnvironmentVariable(key);
            }
            return ret;
        }
    }
    internal class ExpandEnvironmentsExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0] as string;
                return Environment.ExpandEnvironmentVariables(key);
            }
            return ret;
        }
    }
    internal class EnvironmentsExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.GetEnvironmentVariables();
        }
    }
    internal class SetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = string.Empty;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                Environment.CurrentDirectory = Environment.ExpandEnvironmentVariables(dir);
                ret = dir;
            }
            return ret;
        }
    }
    internal class GetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.CurrentDirectory;
        }
    }
    internal class CommandLineExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.CommandLine;
        }
    }
    internal class CommandLineArgsExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count == 1 && operands[0] is string) {
                string name = operands[0] as string;
                string[] args = System.Environment.GetCommandLineArgs();
                int suffixIndex = Array.FindIndex(args, item => item == name);
                if (suffixIndex != -1 && suffixIndex < args.Length - 1) {
                    return args[suffixIndex + 1];
                }
                return "";
            }
            else {
                return Environment.GetCommandLineArgs();
            }
        }
    }
    internal class OsExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.OSVersion.VersionString;
        }
    }
    internal class OsPlatformExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.OSVersion.Platform.ToString();
        }
    }
    internal class OsVersionExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Environment.OSVersion.Version.ToString();
        }
    }
    internal class GetFullPathExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetFullPath(path);
                }
            }
            return ret;
        }
    }
    internal class GetPathRootExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetPathRoot(path);
                }
            }
            return ret;
        }
    }
    internal class GetRandomFileNameExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Path.GetRandomFileName();
        }
    }
    internal class GetTempFileNameExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Path.GetTempFileName();
        }
    }
    internal class GetTempPathExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            return Path.GetTempPath();
        }
    }
    internal class HasExtensionExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = false;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.HasExtension(path);
                }
            }
            return ret;
        }
    }
    internal class IsPathRootedExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = false;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.IsPathRooted(path);
                }
            }
            return ret;
        }
    }
    internal class GetFileNameExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileName(path);
                }
            }
            return r;
        }
    }
    internal class GetFileNameWithoutExtensionExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileNameWithoutExtension(path);
                }
            }
            return r;
        }
    }
    internal class GetExtensionExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetExtension(path);
                }
            }
            return r;
        }
    }
    internal class GetDirectoryNameExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetDirectoryName(path);
                }
            }
            return r;
        }
    }
    internal class CombinePathExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var path1 = operands[0] as string;
                var path2 = operands[1] as string;
                if (null != path1 && null != path2) {
                    path1 = Environment.ExpandEnvironmentVariables(path1);
                    path2 = Environment.ExpandEnvironmentVariables(path2);
                    r = Path.Combine(path1, path2);
                }
            }
            return r;
        }
    }
    internal class ChangeExtensionExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var path = operands[0] as string;
                var ext = operands[1] as string;
                if (null != path && null != ext) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.ChangeExtension(path, ext);
                }
            }
            return r;
        }
    }
    internal class DebugBreakExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                Debug.Break();
            }
            return r;
        }
    }
    internal class DebugLogExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var fmt = operands[0] as string;
                ArrayList al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    al.Add(operands[i]);
                }
                Debug.LogFormat(fmt, al.ToArray());
            }
            return r;
        }
    }
    internal class DebugWarningExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var fmt = operands[0] as string;
                ArrayList al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    al.Add(operands[i]);
                }
                Debug.LogWarningFormat(fmt, al.ToArray());
            }
            return r;
        }
    }
    internal class DebugErrorExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var fmt = operands[0] as string;
                ArrayList al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    al.Add(operands[i]);
                }
                Debug.LogErrorFormat(fmt, al.ToArray());
            }
            return r;
        }
    }
    internal class CallExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var proc = operands[0] as string;
                if (null != proc) {
                    ArrayList arrayList = new ArrayList();
                    for (int i = 1; i < operands.Count; ++i) {
                        arrayList.Add(operands[i]);
                    }
                    r = Calculator.Calc(proc, arrayList.ToArray());
                }
            }
            return r;
        }
    }
    internal class ReturnExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            Calculator.RunState = RunStateEnum.Return;
            object r = null;
            if (operands.Count >= 1) {
                r = operands[0];
            }
            return r;
        }
    }
    internal class RedirectExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            Calculator.RunState = RunStateEnum.Redirect;
            if (operands.Count >= 1) {
                List<string> args = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var arg = operands[i] as string;
                    args.Add(arg);
                }
                return args;
            }
            return null;
        }
    }
    internal class DirectoryExistExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                dir = Environment.ExpandEnvironmentVariables(dir);
                ret = Directory.Exists(dir);
            }
            return ret;
        }
    }
    internal class FileExistExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                file = Environment.ExpandEnvironmentVariables(file);
                ret = File.Exists(file);
            }
            return ret;
        }
    }
    internal class ListDirectoriesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var baseDir = operands[0] as string;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var strList = operands[1] as IList<string>;
                    if (null != strList && operands.Count == 2) {
                        filterList = strList;
                    }
                    else {
                        var list = new List<string>();
                        for (int i = 1; i < operands.Count; ++i) {
                            var str = operands[i] as string;
                            if (null != str) {
                                list.Add(str);
                            }
                        }
                        filterList = list;
                    }
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = fullList;
                }
            }
            return ret;
        }
    }
    internal class ListFilesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var baseDir = operands[0] as string;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var strList = operands[1] as IList<string>;
                    if (null != strList && operands.Count == 2) {
                        filterList = strList;
                    }
                    else {
                        var list = new List<string>();
                        for (int i = 1; i < operands.Count; ++i) {
                            var str = operands[i] as string;
                            if (null != str) {
                                list.Add(str);
                            }
                        }
                        filterList = list;
                    }
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = fullList;
                }
            }
            return ret;
        }
    }
    internal class ListAllDirectoriesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var baseDir = operands[0] as string;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var strList = operands[1] as IList<string>;
                    if (null != strList && operands.Count == 2) {
                        filterList = strList;
                    }
                    else {
                        var list = new List<string>();
                        for (int i = 1; i < operands.Count; ++i) {
                            var str = operands[i] as string;
                            if (null != str) {
                                list.Add(str);
                            }
                        }
                        filterList = list;
                    }
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = fullList;
                }
            }
            return ret;
        }
    }
    internal class ListAllFilesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var baseDir = operands[0] as string;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var strList = operands[1] as IList<string>;
                    if (null != strList && operands.Count == 2) {
                        filterList = strList;
                    }
                    else {
                        var list = new List<string>();
                        for (int i = 1; i < operands.Count; ++i) {
                            var str = operands[i] as string;
                            if (null != str) {
                                list.Add(str);
                            }
                        }
                        filterList = list;
                    }
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = fullList;
                }
            }
            return ret;
        }
    }
    internal class CreateDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                    ret = true;
                    Debug.LogFormat("create directory {0}", dir);
                }
            }
            return ret;
        }
    }
    internal class CopyDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0] as string;
                var dir2 = operands[1] as string;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i] as string;
                    if (null != str) {
                        filterAndNewExts.Add(str);
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
            // 子文件夹
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
            // 文件
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
                    Debug.LogFormat("copy file {0} => {1}", file, targetFile);
                }
            }
        }
    }
    internal class MoveDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var dir1 = operands[0] as string;
                var dir2 = operands[1] as string;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                if (Directory.Exists(dir1)) {
                    if (Directory.Exists(dir2)) {
                        Directory.Delete(dir2);
                    }
                    Directory.Move(dir1, dir2);
                    ret = true;
                    Debug.LogFormat("move directory {0} => {1}", dir1, dir2);
                }
            }
            return ret;
        }
    }
    internal class DeleteDirectoryExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                    ret = true;
                    Debug.LogFormat("delete directory {0}", dir);
                }
            }
            return ret;
        }
    }
    internal class CopyFileExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0] as string;
                var file2 = operands[1] as string;
                file1 = Environment.ExpandEnvironmentVariables(file1);
                file2 = Environment.ExpandEnvironmentVariables(file2);
                if (File.Exists(file1)) {
                    var dir = Path.GetDirectoryName(file2);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    File.Copy(file1, file2, true);
                    ret = true;
                    Debug.LogFormat("copy file {0} => {1}", file1, file2);
                }
            }
            return ret;
        }
    }
    internal class CopyFilesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0] as string;
                var dir2 = operands[1] as string;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i] as string;
                    if (null != str) {
                        filterAndNewExts.Add(str);
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
            // 文件
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
                    Debug.LogFormat("copy file {0} => {1}", file, targetFile);
                }
            }
        }
    }
    internal class MoveFileExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0] as string;
                var file2 = operands[1] as string;
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
                    Debug.LogFormat("move file {0} => {1}", file1, file2);
                }
            }
            return ret;
        }
    }
    internal class DeleteFileExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    File.Delete(file);
                    ret = true;
                    Debug.LogFormat("delete file {0}", file);
                }
            }
            return ret;
        }
    }
    internal class DeleteFilesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i] as string;
                    if (null != str) {
                        filters.Add(str);
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
                            Debug.LogFormat("delete file {0}", file);
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal class DeleteAllFilesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0] as string;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i] as string;
                    if (null != str) {
                        filters.Add(str);
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
                            Debug.LogFormat("delete file {0}", file);
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal class GetFileInfoExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    ret = new FileInfo(file);
                }
            }
            return ret;
        }
    }
    internal class GetDirectoryInfoExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                file = Environment.ExpandEnvironmentVariables(file);
                if (Directory.Exists(file)) {
                    ret = new DirectoryInfo(file);
                }
            }
            return ret;
        }
    }
    internal class GetDriveInfoExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var drive = operands[0] as string;
                ret = new DriveInfo(drive);
            }
            return ret;
        }
    }
    internal class GetDrivesInfoExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = DriveInfo.GetDrives();
            return ret;
        }
    }
    internal class ReadAllLinesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 1) {
                string path = operands[0] as string;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v);
                    }
                    return File.ReadAllLines(path, encoding);
                }
            }
            return new string[0];
        }
    }
    internal class WriteAllLinesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 2) {
                string path = operands[0] as string;
                var lines = operands[1] as IList;
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
                    File.WriteAllLines(path, strs.ToArray(), encoding);
                    return true;
                }
            }
            return false;
        }
    }
    internal class ReadAllTextExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 1) {
                string path = operands[0] as string;
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
            return null;
        }
    }
    internal class WriteAllTextExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 2) {
                string path = operands[0] as string;
                var text = operands[1] as string;
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
    internal class CommandExp : AbstractExpression
    {
        protected override object DoCalc()
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
        protected override bool Load(Dsl.CallData callData)
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
                    Debug.LogFormat("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
                }
            }
            else if (id == "command") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    Debug.LogFormat("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
                }
            }
            else {
                Debug.LogFormat("[syntax error] {0} line:{1}", callData.ToScriptString(false), callData.GetLine());
            }
            return true;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            var id = funcData.GetId();
            var callData = funcData.Call;
            Load(callData);
            if (funcData.HaveStatement()) {
                var cmd = m_CommandConfigs[m_CommandConfigs.Count - 1];
                for (int i = 0; i < funcData.GetStatementNum(); ++i) {
                    var comp = funcData.GetStatement(i);
                    var cd = comp as Dsl.CallData;
                    if (null != cd) {
                        int num = cd.GetParamNum();
                        if (num >= 1) {
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
                                Debug.LogFormat("[syntax error] {0} line:{1}", cd.ToScriptString(false), cd.GetLine());
                            }
                        }
                        else {
                            Debug.LogFormat("[syntax error] {0} line:{1}", cd.ToScriptString(false), cd.GetLine());
                        }
                    }
                    else {
                        var fd = comp as Dsl.FunctionData;
                        if (null != fd && fd.Call.GetParamNum() == 0 && fd.HaveExternScript()) {
                            string os = fd.GetId();
                            string txt = fd.GetExternScript();
                            cmd.m_Commands.Add(os, txt);
                        }
                        else {
                            Debug.LogFormat("[syntax error] {0} line:{1}", comp.ToScriptString(false), comp.GetLine());
                        }
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            for (int i = 0; i < statementData.GetFunctionNum(); ++i) {
                var funcData = statementData.GetFunction(i);
                Load(funcData);
            }
            return true;
        }
        private int ExecProcess(CommandConfig cfg, Stream istream, Stream ostream)
        {
            string fileName = string.Empty;
            if (null != cfg.m_FileName) {
                fileName = cfg.m_FileName.Calc() as string;
            }
            string args = string.Empty;
            if (null != cfg.m_Argments) {
                args = cfg.m_Argments.Calc() as string;
            }
            bool noWait = false;
            if (null != cfg.m_NoWait) {
                noWait = (bool)Convert.ChangeType(cfg.m_NoWait.Calc(), typeof(bool));
            }
            DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
            if (null != cfg.m_UseShellExecute) {
                option.UseShellExecute = (bool)Convert.ChangeType(cfg.m_UseShellExecute.Calc(), typeof(bool));
            }
            if (null != cfg.m_Verb) {
                option.Verb = cfg.m_Verb.Calc() as string;
            }
            if (null != cfg.m_Domain) {
                option.Domain = cfg.m_Domain.Calc() as string;
            }
            if (null != cfg.m_UserName) {
                option.UserName = cfg.m_UserName.Calc() as string;
            }
            if (null != cfg.m_Password) {
                option.Password = cfg.m_Password.Calc() as string;
            }
            if (null != cfg.m_PasswordInClearText) {
                option.PasswordInClearText = cfg.m_PasswordInClearText.Calc() as string;
            }
            if (null != cfg.m_LoadUserProfile) {
                option.LoadUserProfile = (bool)Convert.ChangeType(cfg.m_LoadUserProfile.Calc(), typeof(bool));
            }
            if (null != cfg.m_WindowStyle) {
                var str = cfg.m_WindowStyle.Calc() as string;
                System.Diagnostics.ProcessWindowStyle style;
                if (Enum.TryParse(str, out style)) {
                    option.WindowStyle = style;
                }
            }
            if (null != cfg.m_NewWindow) {
                option.NewWindow = (bool)Convert.ChangeType(cfg.m_NewWindow.Calc(), typeof(bool));
            }
            if (null != cfg.m_ErrorDialog) {
                option.ErrorDialog = (bool)Convert.ChangeType(cfg.m_ErrorDialog.Calc(), typeof(bool));
            }
            if (null != cfg.m_WorkingDirectory) {
                option.WorkingDirectory = cfg.m_WorkingDirectory.Calc() as string;
            }
            Encoding encoding = null;
            if (null != cfg.m_Encoding) {
                var v = cfg.m_Encoding.Calc();
                var name = v as string;
                if (!string.IsNullOrEmpty(name)) {
                    encoding = Encoding.GetEncoding(name);
                }
                else if (v is int) {
                    int codePage = (int)Convert.ChangeType(v, typeof(int));
                    encoding = Encoding.GetEncoding(codePage);
                }
            }
            if (null == encoding) {
                encoding = Encoding.UTF8;
            }

            fileName = Environment.ExpandEnvironmentVariables(fileName);
            args = Environment.ExpandEnvironmentVariables(args);

            IList<string> input = null;
            if (null != cfg.m_Input) {
                var v = cfg.m_Input.Calc();
                try {
                    var str = v as string;
                    if (!string.IsNullOrEmpty(str)) {
                        if (str[0] == '@' || str[0] == '$') {
                            object val = Calculator.GetVariable(str);
                            if (null != val) {
                                var slist = new List<string>();
                                var list = val as IList;
                                foreach (var s in list) {
                                    slist.Add(s.ToString());
                                }
                                input = slist;
                            }
                        }
                        else {
                            str = Environment.ExpandEnvironmentVariables(str);
                            input = File.ReadAllLines(str);
                        }
                    }
                    else {
                        int vn = (int)Convert.ChangeType(v, typeof(int));
                        object val = Calculator.GetVariable(vn);
                        if (null != val) {
                            var slist = new List<string>();
                            var list = val as IList;
                            foreach (var s in list) {
                                slist.Add(s.ToString());
                            }
                            input = slist;
                        }
                    }
                }
                catch (Exception ex) {
                    Debug.LogFormat("input {0} failed:{1}", v, ex.Message);
                }
            }
            bool redirectToConsole = false;
            StringBuilder outputBuilder = null;
            StringBuilder errorBuilder = null;
            object output = null;
            object error = null;
            if (null != cfg.m_Output) {
                var v = cfg.m_Output.Calc();
                var str = v as string;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    output = str;
                }
                else {
                    output = v;
                }
                outputBuilder = new StringBuilder();
            }
            if (null != cfg.m_Error) {
                var v = cfg.m_Error.Calc();
                var str = v as string;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    error = str;
                }
                else {
                    error = v;
                }
                errorBuilder = new StringBuilder();
            }
            if (null != cfg.m_RedirectToConsole) {
                var v = cfg.m_RedirectToConsole.Calc();
                if (null != v) {
                    redirectToConsole = (bool)Convert.ChangeType(v, typeof(bool));
                }
            }
            int exitCode = DslCalculator.NewProcess(noWait, fileName, args, option, istream, ostream, input, outputBuilder, errorBuilder, redirectToConsole, encoding);
            Debug.LogFormat("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);

            if (null != outputBuilder && null != output) {
                try {
                    var file = output as string;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, outputBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, outputBuilder.ToString());
                        }
                    }
                    else {
                        int v = (int)Convert.ChangeType(output, typeof(int));
                        Calculator.SetVariable(v, outputBuilder.ToString());
                    }
                }
                catch (Exception ex) {
                    Debug.LogFormat("output {0} failed:{1}", output, ex.Message);
                }
            }
            if (null != errorBuilder && null != error) {
                try {
                    var file = error as string;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, errorBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, errorBuilder.ToString());
                        }
                    }
                    else {
                        int v = (int)Convert.ChangeType(error, typeof(int));
                        Calculator.SetVariable(v, errorBuilder.ToString());
                    }
                }
                catch (Exception ex) {
                    Debug.LogFormat("error {0} failed:{1}", error, ex.Message);
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
                    noWait = (bool)Convert.ChangeType(cfg.m_NoWait.Calc(), typeof(bool));
                }
                DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
                if (null != cfg.m_UseShellExecute) {
                    option.UseShellExecute = (bool)Convert.ChangeType(cfg.m_UseShellExecute.Calc(), typeof(bool));
                }
                if (null != cfg.m_Verb) {
                    option.Verb = cfg.m_Verb.Calc() as string;
                }
                if (null != cfg.m_Domain) {
                    option.Domain = cfg.m_Domain.Calc() as string;
                }
                if (null != cfg.m_UserName) {
                    option.UserName = cfg.m_UserName.Calc() as string;
                }
                if (null != cfg.m_Password) {
                    option.Password = cfg.m_Password.Calc() as string;
                }
                if (null != cfg.m_PasswordInClearText) {
                    option.PasswordInClearText = cfg.m_PasswordInClearText.Calc() as string;
                }
                if (null != cfg.m_LoadUserProfile) {
                    option.LoadUserProfile = (bool)Convert.ChangeType(cfg.m_LoadUserProfile.Calc(), typeof(bool));
                }
                if (null != cfg.m_WindowStyle) {
                    var str = cfg.m_WindowStyle.Calc() as string;
                    System.Diagnostics.ProcessWindowStyle style;
                    if (Enum.TryParse(str, out style)) {
                        option.WindowStyle = style;
                    }
                }
                if (null != cfg.m_NewWindow) {
                    option.NewWindow = (bool)Convert.ChangeType(cfg.m_NewWindow.Calc(), typeof(bool));
                }
                if (null != cfg.m_ErrorDialog) {
                    option.ErrorDialog = (bool)Convert.ChangeType(cfg.m_ErrorDialog.Calc(), typeof(bool));
                }
                if (null != cfg.m_WorkingDirectory) {
                    option.WorkingDirectory = cfg.m_WorkingDirectory.Calc() as string;
                }
                Encoding encoding = null;
                if (null != cfg.m_Encoding) {
                    var v = cfg.m_Encoding.Calc();
                    var name = v as string;
                    if (!string.IsNullOrEmpty(name)) {
                        encoding = Encoding.GetEncoding(name);
                    }
                    else if (v is int) {
                        int codePage = (int)Convert.ChangeType(v, typeof(int));
                        encoding = Encoding.GetEncoding(codePage);
                    }
                }
                if (null == encoding) {
                    encoding = Encoding.UTF8;
                }
                IList<string> input = null;
                if (null != cfg.m_Input) {
                    var v = cfg.m_Input.Calc();
                    try {
                        var str = v as string;
                        if (!string.IsNullOrEmpty(str)) {
                            if (str[0] == '@' || str[0] == '$') {
                                object val = Calculator.GetVariable(str);
                                if (null != val) {
                                    var slist = new List<string>();
                                    var list = val as IList;
                                    foreach (var s in list) {
                                        slist.Add(s.ToString());
                                    }
                                    input = slist;
                                }
                            }
                            else {
                                str = Environment.ExpandEnvironmentVariables(str);
                                input = File.ReadAllLines(str);
                            }
                        }
                        else {
                            int vn = (int)Convert.ChangeType(v, typeof(int));
                            object val = Calculator.GetVariable(vn);
                            if (null != val) {
                                var slist = new List<string>();
                                var list = val as IList;
                                foreach (var s in list) {
                                    slist.Add(s.ToString());
                                }
                                input = slist;
                            }
                        }
                    }
                    catch (Exception ex) {
                        Debug.LogFormat("input {0} failed:{1}", v, ex.Message);
                    }
                }
                bool redirectToConsole = false;
                StringBuilder outputBuilder = null;
                StringBuilder errorBuilder = null;
                object output = null;
                object error = null;
                if (null != cfg.m_Output) {
                    var v = cfg.m_Output.Calc();
                    var str = v as string;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        output = str;
                    }
                    else {
                        output = v;
                    }
                    outputBuilder = new StringBuilder();
                }
                if (null != cfg.m_Error) {
                    var v = cfg.m_Error.Calc();
                    var str = v as string;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        error = str;
                    }
                    else {
                        error = v;
                    }
                    errorBuilder = new StringBuilder();
                }
                if (null != cfg.m_RedirectToConsole) {
                    var v = cfg.m_RedirectToConsole.Calc();
                    if (null != v) {
                        redirectToConsole = (bool)Convert.ChangeType(v, typeof(bool));
                    }
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
                    Debug.LogFormat("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);

                    if (null != outputBuilder && null != output) {
                        try {
                            var file = output as string;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, outputBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, outputBuilder.ToString());
                                }
                            }
                            else {
                                int v = (int)Convert.ChangeType(output, typeof(int));
                                Calculator.SetVariable(v, outputBuilder.ToString());
                            }
                        }
                        catch (Exception ex) {
                            Debug.LogFormat("output {0} failed:{1}", output, ex.Message);
                        }
                    }
                    if (null != errorBuilder && null != error) {
                        try {
                            var file = error as string;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, errorBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, errorBuilder.ToString());
                                }
                            }
                            else {
                                int v = (int)Convert.ChangeType(error, typeof(int));
                                Calculator.SetVariable(v, errorBuilder.ToString());
                            }
                        }
                        catch (Exception ex) {
                            Debug.LogFormat("error {0} failed:{1}", error, ex.Message);
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
            internal IExpression m_Error = null;
            internal IExpression m_RedirectToConsole = null;
        }

        private List<CommandConfig> m_CommandConfigs = new List<CommandConfig>();
    }
    internal class KillExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = 0;
            if (operands.Count >= 1) {
                int myselfId = 0;
                var myself = System.Diagnostics.Process.GetCurrentProcess();
                if (null != myself) {
                    myselfId = myself.Id;
                }
                var vObj = operands[0];
                var name = vObj as string;
                if (!string.IsNullOrEmpty(name)) {
                    int ct = 0;
                    var ps = System.Diagnostics.Process.GetProcessesByName(name);
                    foreach (var p in ps) {
                        if (p.Id != myselfId) {
                            Debug.LogFormat("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                            p.Kill();
                            ++ct;
                        }
                    }
                    ret = ct;
                }
                else if (vObj is int) {
                    int pid = (int)Convert.ChangeType(vObj, typeof(int));
                    var p = System.Diagnostics.Process.GetProcessById(pid);
                    if (null != p && p.Id != myselfId) {
                        Debug.LogFormat("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                        p.Kill();
                        ret = 1;
                    }
                }
                else {

                }
            }
            return ret;
        }
    }
    internal class KillMeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
                int exitCode = 0;
                if (operands.Count >= 1) {
                    exitCode = (int)Convert.ChangeType(operands[0], typeof(int));
                }
                Debug.LogFormat("killme {0}[pid:{1},session id:{2}] exit code:{3}", p.ProcessName, p.Id, p.SessionId, exitCode);
                Environment.Exit(exitCode);
            }
            return ret;
        }
    }
    internal class GetCurrentProcessIdExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
            }
            return ret;
        }
    }
    internal class ListProcessesExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            IList<System.Diagnostics.Process> ret = null;
            var ps = System.Diagnostics.Process.GetProcesses();
            string filter = null;
            if (operands.Count >= 1) {
                filter = operands[0] as string;
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
            return ret;
        }
    }
    internal class WaitExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                var v = operands[0];
                if (null != v) {
                    int time = (int)Convert.ChangeType(v, typeof(int));
                    System.Threading.Thread.Sleep(time);
                    ret = time;
                }
            }
            return ret;
        }
    }
    internal class WaitAllExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            var tasks = DslCalculator.Tasks;
            int timeout = -1;
            if (operands.Count >= 1) {
                timeout = (int)Convert.ChangeType(operands[0], typeof(int));
            }
            List<int> results = new List<int>();
            if (Task.WaitAll(tasks.ToArray(), timeout)) {
                foreach (var task in tasks) {
                    results.Add(task.Result);
                }
            }
            return results;
        }
    }
    internal class WaitStartIntervalExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 1) {
                var v = operands[0];
                if (null != v) {
                    DslCalculator.CheckStartInterval = (int)Convert.ChangeType(v, typeof(int));
                }
            }
            return DslCalculator.CheckStartInterval;
        }
    }

    internal class DisplayProgressBarExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            if (operands.Count >= 3) {
                var title = operands[0] as string;
                var text = operands[1] as string;
                var progress = ToFloat(operands[2]);
                if (null != title && null != text) {
                    EditorUtility.DisplayProgressBar(title, text, progress);
                }
            }
#endif
            return true;
        }
    }
    internal class DisplayCancelableProgressBarExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            bool ret = false;
#if UNITY_EDITOR
            if (operands.Count >= 3) {
                var title = operands[0] as string;
                var text = operands[1] as string;
                var progress = ToFloat(operands[2]);
                if (null != title && null != text) {
                    ret = EditorUtility.DisplayCancelableProgressBar(title, text, progress);
                }
            }
#endif
            return ret;
        }
    }
    internal class ClearProgressBarExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            return true;
        }
    }
    internal class OpenWithDefaultAppExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                string file = operands[0] as string;
                if (!string.IsNullOrEmpty(file)) {
                    EditorUtility.OpenWithDefaultApp(file);
                }
            }
#endif
            return null;
        }
    }
    internal class OpenFolderPanelExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0] as string;
                string dir = operands[1] as string;
                string def = operands[2] as string;
                if (null != title && null != dir && null != def) {
                    ret = EditorUtility.OpenFolderPanel(title, dir, def);
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class OpenFilePanelExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0] as string;
                string dir = operands[1] as string;
                string ext = operands[2] as string;
                if (null != title && null != dir && null != ext) {
                    ret = EditorUtility.OpenFilePanel(title, dir, ext);
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class OpenFilePanelWithFiltersExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0] as string;
                string dir = operands[1] as string;
                List<string> filters = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    string filter = operands[i] as string;
                    if (!string.IsNullOrEmpty(filter)) {
                        filters.Add(filter);
                    }
                }
                if (null != title && null != dir) {
                    ret = EditorUtility.OpenFilePanelWithFilters(title, dir, filters.ToArray());
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class SaveFilePanelExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 4) {
                string title = operands[0] as string;
                string dir = operands[1] as string;
                string def = operands[2] as string;
                string ext = operands[3] as string;
                if (null != title && null != dir && null != def && null != ext) {
                    ret = EditorUtility.SaveFilePanel(title, dir, def, ext);
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class SaveFilePanelInProjectExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 4) {
                string title = operands[0] as string;
                string def = operands[1] as string;
                string ext = operands[2] as string;
                string msg = operands[3] as string;
                string path = string.Empty;
                if (operands.Count >= 5) {
                    path = operands[4] as string;
                }
                if (null != title && null != def && null != ext && null != msg) {
                    if (!string.IsNullOrEmpty(path)) {
                        ret = EditorUtility.SaveFilePanelInProject(title, def, ext, msg, path);
                    }
                    else {
                        ret = EditorUtility.SaveFilePanelInProject(title, def, ext, msg);
                    }
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class SaveFolderPanelExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0] as string;
                string dir = operands[1] as string;
                string def = operands[2] as string;
                if (null != title && null != dir && null != def) {
                    ret = EditorUtility.SaveFolderPanel(title, dir, def);
                }
            }
            return ret;
#else
            return null;
#endif
        }
    }
    internal class DisplayDialogExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
#if UNITY_EDITOR
            int ret = -1;
            if (operands.Count >= 3) {
                string title = operands[0] as string;
                string msg = operands[1] as string;
                string ok = operands[2] as string;
                if (null != title && null != msg && null != ok) {
                    if (operands.Count >= 4) {
                        string cancel = operands[3] as string;
                        if (operands.Count >= 5) {
                            string alt = operands[4] as string;
                            ret = EditorUtility.DisplayDialogComplex(title, msg, ok, cancel, alt);
                        }
                        else {
                            ret = EditorUtility.DisplayDialog(title, msg, ok, cancel) ? 1 : 0;
                        }
                    }
                    else {
                        ret = EditorUtility.DisplayDialog(title, msg, ok) ? 1 : 0;
                    }
                }
            }
            return ret;
#else
            return -1;
#endif
        }
    }
    internal class CalcMd5Exp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
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
#if QINSHI
    internal class DebugValueExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            string ret = string.Empty;
            if (operands.Count >= 1) {
                string name = operands[0] as string;
                if (operands.Count >= 2) {
                    var val = operands[1];
                    if (!string.IsNullOrEmpty(name)) {
                        string valStr = null;
                        if (null != val)
                            valStr = val.ToString();
                        GameFramework.GmCommands.GlobalDebugValues.SetValue(name, valStr);
                        ret = valStr;
                    }
                } else {
                    ret = GameFramework.GmCommands.GlobalDebugValues.GetStringValue(name);
                }
            }
            return ret;
        }
    }
    internal class StoryVarExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = null;
            if (operands.Count >= 1) {
                string name = operands[0] as string;
                if (operands.Count >= 2) {
                    var val = operands[1];
                    if (!string.IsNullOrEmpty(name)) {
                        var instance = CsLibrary.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
                        if (null == instance) {
                            string txt = "script(main){onmessage(\"start\"){};};";
                            CsLibrary.GmCommands.ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                            instance = CsLibrary.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
                        }
                        instance.SetVariable(name, val);
                        ret = val;
                    }
                }
                else {
                    var instance = CsLibrary.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
                    if (null == instance) {
                        string txt = "script(main){onmessage(\"start\"){};};";
                        CsLibrary.GmCommands.ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                        instance = CsLibrary.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
                    }
                    instance.TryGetVariable(name, out ret);
                }
            }
            return ret;
        }
    }
    internal class StoryValueExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            object ret = null;
            var instance = GameFramework.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
            if (null == instance) {
                string txt = "script(main){onmessage(\"start\"){};};";
                GameFramework.GmCommands.ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                GameFramework.GmCommands.ClientGmStorySystem.Instance.StartStory("main");
                instance = GameFramework.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
            }
            var handler = instance.GetMessageHandler("start");
            foreach (var exp in m_Values) {
                exp.Evaluate(instance, handler, null, null);
                if (exp.HaveValue) {
                    ret = exp.Value;
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent param = callData.GetParam(ix);
                var exp = StorySystem.StoryValueManager.Instance.CalcValue(param);
                m_Values.Add(exp);
            }
            return true;
        }

        private List<StorySystem.IStoryValue> m_Values = new List<StorySystem.IStoryValue>();
    }
    internal class StoryCommandExp : AbstractExpression
    {
        protected override object DoCalc()
        {
            var instance = GameFramework.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
            if (null == instance) {
                string txt = "script(main){onmessage(\"start\"){};};";
                GameFramework.GmCommands.ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                instance = GameFramework.GmCommands.ClientGmStorySystem.Instance.GetStory("main");
            }
            var handler = instance.GetMessageHandler("start");
            foreach (var cmd in m_Commands) {
                cmd.Reset();
            }
            foreach (var cmd in m_Commands) {
                cmd.Execute(instance, handler, 0, null, null);
            }
            return null;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent param = callData.GetParam(ix);
                var cmd = StorySystem.StoryCommandManager.Instance.CreateCommand(param);
                m_Commands.Add(cmd);
            }
            return true;
        }

        private List<StorySystem.IStoryCommand> m_Commands = new List<StorySystem.IStoryCommand>();
    }
#endif
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
        public IDictionary<string, object> NamedGlobalVariables
        {
            get { return m_NamedGlobalVariables; }
        }
        public void Init()
        {
            Register("args", new ExpressionFactoryHelper<ArgsGet>());
            Register("arg", new ExpressionFactoryHelper<ArgGet>());
            Register("argnum", new ExpressionFactoryHelper<ArgNumGet>());
            Register("var", new ExpressionFactoryHelper<VarGet>());
            Register("+", new ExpressionFactoryHelper<AddExp>());
            Register("-", new ExpressionFactoryHelper<SubExp>());
            Register("*", new ExpressionFactoryHelper<MulExp>());
            Register("/", new ExpressionFactoryHelper<DivExp>());
            Register("%", new ExpressionFactoryHelper<ModExp>());
            Register("&", new ExpressionFactoryHelper<BitAndExp>());
            Register("|", new ExpressionFactoryHelper<BitOrExp>());
            Register("^", new ExpressionFactoryHelper<BitXorExp>());
            Register("~", new ExpressionFactoryHelper<BitNotExp>());
            Register("<<", new ExpressionFactoryHelper<LShiftExp>());
            Register(">>", new ExpressionFactoryHelper<RShiftExp>());
            Register("max", new ExpressionFactoryHelper<MaxExp>());
            Register("min", new ExpressionFactoryHelper<MinExp>());
            Register("abs", new ExpressionFactoryHelper<AbsExp>());
            Register("sin", new ExpressionFactoryHelper<SinExp>());
            Register("cos", new ExpressionFactoryHelper<CosExp>());
            Register("tan", new ExpressionFactoryHelper<TanExp>());
            Register("asin", new ExpressionFactoryHelper<AsinExp>());
            Register("acos", new ExpressionFactoryHelper<AcosExp>());
            Register("atan", new ExpressionFactoryHelper<AtanExp>());
            Register("atan2", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", new ExpressionFactoryHelper<TanhExp>());
            Register("rndint", new ExpressionFactoryHelper<RndIntExp>());
            Register("rndfloat", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", new ExpressionFactoryHelper<SqrtExp>());
            Register("log", new ExpressionFactoryHelper<LogExp>());
            Register("log10", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", new ExpressionFactoryHelper<FloorExp>());
            Register("ceil", new ExpressionFactoryHelper<CeilExp>());
            Register("lerp", new ExpressionFactoryHelper<LerpExp>());
            Register("lerpunclamped", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerpangle", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("clamp01", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", new ExpressionFactoryHelper<ClampExp>());
            Register("approximately", new ExpressionFactoryHelper<ApproximatelyExp>());
            Register("ispoweroftwo", new ExpressionFactoryHelper<IsPowerOfTwoExp>());
            Register("closestpoweroftwo", new ExpressionFactoryHelper<ClosestPowerOfTwoExp>());
            Register("nextpoweroftwo", new ExpressionFactoryHelper<NextPowerOfTwoExp>());
            Register("dist", new ExpressionFactoryHelper<DistExp>());
            Register("distsqr", new ExpressionFactoryHelper<DistSqrExp>());
            Register(">", new ExpressionFactoryHelper<GreatExp>());
            Register(">=", new ExpressionFactoryHelper<GreatEqualExp>());
            Register("<", new ExpressionFactoryHelper<LessExp>());
            Register("<=", new ExpressionFactoryHelper<LessEqualExp>());
            Register("==", new ExpressionFactoryHelper<EqualExp>());
            Register("!=", new ExpressionFactoryHelper<NotEqualExp>());
            Register("&&", new ExpressionFactoryHelper<AndExp>());
            Register("||", new ExpressionFactoryHelper<OrExp>());
            Register("!", new ExpressionFactoryHelper<NotExp>());
            Register("?", new ExpressionFactoryHelper<CondExp>());
            Register("if", new ExpressionFactoryHelper<IfExp>());
            Register("while", new ExpressionFactoryHelper<WhileExp>());
            Register("loop", new ExpressionFactoryHelper<LoopExp>());
            Register("looplist", new ExpressionFactoryHelper<LoopListExp>());
            Register("foreach", new ExpressionFactoryHelper<ForeachExp>());
            Register("format", new ExpressionFactoryHelper<FormatExp>());
            Register("gettypeassemblyname", new ExpressionFactoryHelper<GetTypeAssemblyNameExp>());
            Register("gettypefullname", new ExpressionFactoryHelper<GetTypeFullNameExp>());
            Register("gettypename", new ExpressionFactoryHelper<GetTypeNameExp>());
            Register("gettype", new ExpressionFactoryHelper<GetTypeExp>());
            Register("changetype", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("parseenum", new ExpressionFactoryHelper<ParseEnumExp>());
            Register("dotnetcall", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnetset", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnetget", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("linq", new ExpressionFactoryHelper<LinqExp>());
            Register("isnull", new ExpressionFactoryHelper<IsNullExp>());
            Register("assetpath2guid", new ExpressionFactoryHelper<AssetPath2GUIDExp>());
            Register("guid2assetpath", new ExpressionFactoryHelper<GUID2AssetPathExp>());
            Register("getassetpath", new ExpressionFactoryHelper<GetAssetPathExp>());
            Register("getdependencies", new ExpressionFactoryHelper<GetDependenciesExp>());
            Register("getassetimporter", new ExpressionFactoryHelper<GetAssetImporterExp>());
            Register("loadasset", new ExpressionFactoryHelper<LoadAssetExp>());
            Register("unloadasset", new ExpressionFactoryHelper<UnloadAssetExp>());
            Register("getprefabtype", new ExpressionFactoryHelper<GetPrefabTypeExp>());
            Register("getprefabobject", new ExpressionFactoryHelper<GetPrefabObjectExp>());
            Register("getprefabparent", new ExpressionFactoryHelper<GetPrefabParentExp>());
            Register("destroyobject", new ExpressionFactoryHelper<DestroyObjectExp>());
            Register("getcomponent", new ExpressionFactoryHelper<GetComponentExp>());
            Register("getcomponents", new ExpressionFactoryHelper<GetComponentsExp>());
            Register("getcomponentinchildren", new ExpressionFactoryHelper<GetComponentInChildrenExp>());
            Register("getcomponentsinchildren", new ExpressionFactoryHelper<GetComponentsInChildrenExp>());
            Register("newstringbuilder", new ExpressionFactoryHelper<NewStringBuilderExp>());
            Register("appendformat", new ExpressionFactoryHelper<AppendFormatExp>());
            Register("appendlineformat", new ExpressionFactoryHelper<AppendLineFormatExp>());
            Register("stringbuildertostring", new ExpressionFactoryHelper<StringBuilderToStringExp>());
            Register("stringjoin", new ExpressionFactoryHelper<StringJoinExp>());
            Register("stringsplit", new ExpressionFactoryHelper<StringSplitExp>());
            Register("stringtrim", new ExpressionFactoryHelper<StringTrimExp>());
            Register("stringtrimstart", new ExpressionFactoryHelper<StringTrimStartExp>());
            Register("stringtrimend", new ExpressionFactoryHelper<StringTrimEndExp>());
            Register("stringlower", new ExpressionFactoryHelper<StringLowerExp>());
            Register("stringupper", new ExpressionFactoryHelper<StringUpperExp>());
            Register("stringreplace", new ExpressionFactoryHelper<StringReplaceExp>());
            Register("stringreplacechar", new ExpressionFactoryHelper<StringReplaceCharExp>());
            Register("makestring", new ExpressionFactoryHelper<MakeStringExp>());
            Register("str2int", new ExpressionFactoryHelper<Str2IntExp>());
            Register("str2float", new ExpressionFactoryHelper<Str2FloatExp>());
            Register("hex2int", new ExpressionFactoryHelper<Hex2IntExp>());
            Register("datetimestr", new ExpressionFactoryHelper<DatetimeStrExp>());
            Register("longdatestr", new ExpressionFactoryHelper<LongDateStrExp>());
            Register("longtimestr", new ExpressionFactoryHelper<LongTimeStrExp>());
            Register("shortdatestr", new ExpressionFactoryHelper<ShortDateStrExp>());
            Register("shorttimestr", new ExpressionFactoryHelper<ShortTimeStrExp>());
            Register("isnullorempty", new ExpressionFactoryHelper<IsNullOrEmptyExp>());
            Register("array", new ExpressionFactoryHelper<ArrayExp>());
            Register("toarray", new ExpressionFactoryHelper<ToArrayExp>());
            Register("listsize", new ExpressionFactoryHelper<ListSizeExp>());
            Register("list", new ExpressionFactoryHelper<ListExp>());
            Register("listget", new ExpressionFactoryHelper<ListGetExp>());
            Register("listset", new ExpressionFactoryHelper<ListSetExp>());
            Register("listindexof", new ExpressionFactoryHelper<ListIndexOfExp>());
            Register("listadd", new ExpressionFactoryHelper<ListAddExp>());
            Register("listremove", new ExpressionFactoryHelper<ListRemoveExp>());
            Register("listinsert", new ExpressionFactoryHelper<ListInsertExp>());
            Register("listremoveat", new ExpressionFactoryHelper<ListRemoveAtExp>());
            Register("listclear", new ExpressionFactoryHelper<ListClearExp>());
            Register("listsplit", new ExpressionFactoryHelper<ListSplitExp>());
            Register("hashtablesize", new ExpressionFactoryHelper<HashtableSizeExp>());
            Register("hashtable", new ExpressionFactoryHelper<HashtableExp>());
            Register("hashtableget", new ExpressionFactoryHelper<HashtableGetExp>());
            Register("hashtableset", new ExpressionFactoryHelper<HashtableSetExp>());
            Register("hashtableadd", new ExpressionFactoryHelper<HashtableAddExp>());
            Register("hashtableremove", new ExpressionFactoryHelper<HashtableRemoveExp>());
            Register("hashtableclear", new ExpressionFactoryHelper<HashtableClearExp>());
            Register("hashtablekeys", new ExpressionFactoryHelper<HashtableKeysExp>());
            Register("hashtablevalues", new ExpressionFactoryHelper<HashtableValuesExp>());
            Register("listhashtable", new ExpressionFactoryHelper<ListHashtableExp>());
            Register("hashtablesplit", new ExpressionFactoryHelper<HashtableSplitExp>());
            Register("peek", new ExpressionFactoryHelper<PeekExp>());
            Register("stacksize", new ExpressionFactoryHelper<StackSizeExp>());
            Register("stack", new ExpressionFactoryHelper<StackExp>());
            Register("push", new ExpressionFactoryHelper<PushExp>());
            Register("pop", new ExpressionFactoryHelper<PopExp>());
            Register("stackclear", new ExpressionFactoryHelper<StackClearExp>());
            Register("queuesize", new ExpressionFactoryHelper<QueueSizeExp>());
            Register("queue", new ExpressionFactoryHelper<QueueExp>());
            Register("enqueue", new ExpressionFactoryHelper<EnqueueExp>());
            Register("dequeue", new ExpressionFactoryHelper<DequeueExp>());
            Register("queueclear", new ExpressionFactoryHelper<QueueClearExp>());
            Register("setenv", new ExpressionFactoryHelper<SetEnvironmentExp>());
            Register("getenv", new ExpressionFactoryHelper<GetEnvironmentExp>());
            Register("expand", new ExpressionFactoryHelper<ExpandEnvironmentsExp>());
            Register("envs", new ExpressionFactoryHelper<EnvironmentsExp>());
            Register("cd", new ExpressionFactoryHelper<SetCurrentDirectoryExp>());
            Register("pwd", new ExpressionFactoryHelper<GetCurrentDirectoryExp>());
            Register("cmdline", new ExpressionFactoryHelper<CommandLineExp>());
            Register("cmdlineargs", new ExpressionFactoryHelper<CommandLineArgsExp>());
            Register("os", new ExpressionFactoryHelper<OsExp>());
            Register("osplatform", new ExpressionFactoryHelper<OsPlatformExp>());
            Register("osversion", new ExpressionFactoryHelper<OsVersionExp>());
            Register("getfullpath", new ExpressionFactoryHelper<GetFullPathExp>());
            Register("getpathroot", new ExpressionFactoryHelper<GetPathRootExp>());
            Register("getrandomfilename", new ExpressionFactoryHelper<GetRandomFileNameExp>());
            Register("gettempfilename", new ExpressionFactoryHelper<GetTempFileNameExp>());
            Register("gettemppath", new ExpressionFactoryHelper<GetTempPathExp>());
            Register("hasextension", new ExpressionFactoryHelper<HasExtensionExp>());
            Register("ispathrooted", new ExpressionFactoryHelper<IsPathRootedExp>());
            Register("getfilename", new ExpressionFactoryHelper<GetFileNameExp>());
            Register("getfilenamewithoutextension", new ExpressionFactoryHelper<GetFileNameWithoutExtensionExp>());
            Register("getextension", new ExpressionFactoryHelper<GetExtensionExp>());
            Register("getdirectoryname", new ExpressionFactoryHelper<GetDirectoryNameExp>());
            Register("combinepath", new ExpressionFactoryHelper<CombinePathExp>());
            Register("changeextension", new ExpressionFactoryHelper<ChangeExtensionExp>());
            Register("debugbreak", new ExpressionFactoryHelper<DebugBreakExp>());
            Register("debuglog", new ExpressionFactoryHelper<DebugLogExp>());
            Register("debugwarning", new ExpressionFactoryHelper<DebugWarningExp>());
            Register("debugerror", new ExpressionFactoryHelper<DebugErrorExp>());
            Register("call", new ExpressionFactoryHelper<CallExp>());
            Register("return", new ExpressionFactoryHelper<ReturnExp>());
            Register("redirect", new ExpressionFactoryHelper<RedirectExp>());

            Register("direxist", new ExpressionFactoryHelper<DirectoryExistExp>());
            Register("fileexist", new ExpressionFactoryHelper<FileExistExp>());
            Register("listdirs", new ExpressionFactoryHelper<ListDirectoriesExp>());
            Register("listfiles", new ExpressionFactoryHelper<ListFilesExp>());
            Register("listalldirs", new ExpressionFactoryHelper<ListAllDirectoriesExp>());
            Register("listallfiles", new ExpressionFactoryHelper<ListAllFilesExp>());
            Register("createdir", new ExpressionFactoryHelper<CreateDirectoryExp>());
            Register("copydir", new ExpressionFactoryHelper<CopyDirectoryExp>());
            Register("movedir", new ExpressionFactoryHelper<MoveDirectoryExp>());
            Register("deletedir", new ExpressionFactoryHelper<DeleteDirectoryExp>());
            Register("copyfile", new ExpressionFactoryHelper<CopyFileExp>());
            Register("copyfiles", new ExpressionFactoryHelper<CopyFilesExp>());
            Register("movefile", new ExpressionFactoryHelper<MoveFileExp>());
            Register("deletefile", new ExpressionFactoryHelper<DeleteFileExp>());
            Register("deletefiles", new ExpressionFactoryHelper<DeleteFilesExp>());
            Register("deleteallfiles", new ExpressionFactoryHelper<DeleteAllFilesExp>());
            Register("getfileinfo", new ExpressionFactoryHelper<GetFileInfoExp>());
            Register("getdirinfo", new ExpressionFactoryHelper<GetDirectoryInfoExp>());
            Register("getdriveinfo", new ExpressionFactoryHelper<GetDriveInfoExp>());
            Register("getdrivesinfo", new ExpressionFactoryHelper<GetDrivesInfoExp>());
            Register("readalllines", new ExpressionFactoryHelper<ReadAllLinesExp>());
            Register("writealllines", new ExpressionFactoryHelper<WriteAllLinesExp>());
            Register("readalltext", new ExpressionFactoryHelper<ReadAllTextExp>());
            Register("writealltext", new ExpressionFactoryHelper<WriteAllTextExp>());
            Register("process", new ExpressionFactoryHelper<CommandExp>());
            Register("command", new ExpressionFactoryHelper<CommandExp>());
            Register("kill", new ExpressionFactoryHelper<KillExp>());
            Register("killme", new ExpressionFactoryHelper<KillMeExp>());
            Register("pid", new ExpressionFactoryHelper<GetCurrentProcessIdExp>());
            Register("plist", new ExpressionFactoryHelper<ListProcessesExp>());
            Register("wait", new ExpressionFactoryHelper<WaitExp>());
            Register("waitall", new ExpressionFactoryHelper<WaitAllExp>());
            Register("waitstartinterval", new ExpressionFactoryHelper<WaitStartIntervalExp>());

            Register("displayprogressbar", new ExpressionFactoryHelper<DisplayProgressBarExp>());
            Register("displaycancelableprogressbar", new ExpressionFactoryHelper<DisplayCancelableProgressBarExp>());
            Register("clearprogressbar", new ExpressionFactoryHelper<ClearProgressBarExp>());
            Register("openwithdefaultapp", new ExpressionFactoryHelper<OpenWithDefaultAppExp>());
            Register("openfilepanel", new ExpressionFactoryHelper<OpenFilePanelExp>());
            Register("openfilepanelwithfilters", new ExpressionFactoryHelper<OpenFilePanelWithFiltersExp>());
            Register("openfolderpanel", new ExpressionFactoryHelper<OpenFolderPanelExp>());
            Register("savefilepanel", new ExpressionFactoryHelper<SaveFilePanelExp>());
            Register("savefilepanelinproject", new ExpressionFactoryHelper<SaveFilePanelInProjectExp>());
            Register("savefolderpanel", new ExpressionFactoryHelper<SaveFolderPanelExp>());
            Register("displaydialog", new ExpressionFactoryHelper<DisplayDialogExp>());
            Register("calcmd5", new ExpressionFactoryHelper<CalcMd5Exp>());
#if QINSHI
            Register("debugvalue", new ExpressionFactoryHelper<DebugValueExp>());
            Register("storyvar", new ExpressionFactoryHelper<StoryVarExp>());
            Register("storyvalue", new ExpressionFactoryHelper<StoryValueExp>());
            Register("storycommand", new ExpressionFactoryHelper<StoryCommandExp>());
#endif
        }
        public void Register(string name, IExpressionFactory factory)
        {
            if (!m_ExpressionFactories.ContainsKey(name)) {
                m_ExpressionFactories.Add(name, factory);
            }
            else {
                m_ExpressionFactories[name] = factory;
            }
        }
        public void Cleanup()
        {
            m_Procs.Clear();
            m_Stack.Clear();
            m_NamedGlobalVariables.Clear();
        }
        public void ClearGlobalVariables()
        {
            m_NamedGlobalVariables.Clear();
        }
        public bool TryGetGlobalVariable(string v, out object result)
        {
            return m_NamedGlobalVariables.TryGetValue(v, out result);
        }
        public object GetGlobalVariable(string v)
        {
            object result = null;
            m_NamedGlobalVariables.TryGetValue(v, out result);
            return result;
        }
        public void SetGlobalVariable(string v, object val)
        {
            var vars = m_NamedGlobalVariables;
            vars[v] = val;
        }
        public bool RemoveGlobalVariable(string v)
        {
            return m_NamedGlobalVariables.Remove(v);
        }
        public void LoadDsl(string dslFile)
        {
            Dsl.DslFile file = new Dsl.DslFile();
            string path = dslFile;
            if (file.Load(path, (string s) => { Debug.LogError(s); })) {
                foreach (Dsl.DslInfo info in file.DslInfos) {
                    LoadDsl(info);
                }
            }
        }
        public void LoadDsl(Dsl.DslInfo info)
        {
            if (info.GetId() != "script")
                return;
            string id = info.First.Call.GetParamId(0);
            Dsl.FunctionData func = null;
            if (info.GetFunctionNum() == 1) {
                func = info.First;
            }
            else if (info.GetFunctionNum() == 2) {
                func = info.Second;

                if (func.GetId() == "args") {
                    if (func.Call.GetParamNum() > 0) {
                        List<string> names;
                        if (!m_ProcArgNames.TryGetValue(id, out names)) {
                            names = new List<string>();
                            m_ProcArgNames.Add(id, names);
                        }
                        else {
                            names.Clear();
                        }
                        foreach (var p in func.Call.Params) {
                            names.Add(p.GetId());
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
            List<IExpression> list;
            if (!m_Procs.TryGetValue(id, out list)) {
                list = new List<IExpression>();
                m_Procs.Add(id, list);
            } else {
                list.Clear();
            }
            foreach (Dsl.ISyntaxComponent comp in func.Statements) {
                var exp = Load(comp);
                if (null != exp) {
                    list.Add(exp);
                }
            }
        }
        public void LoadDsl(string proc, Dsl.FunctionData func)
        {
            LoadDsl(proc, null, func);
        }
        public void LoadDsl(string proc, IList<string> argNames, Dsl.FunctionData func)
        {
            if (null != argNames && argNames.Count > 0) {
                List<string> names;
                if (!m_ProcArgNames.TryGetValue(proc, out names)) {
                    names = new List<string>(argNames);
                    m_ProcArgNames.Add(proc, names);
                }
                else {
                    names.Clear();
                    names.AddRange(argNames);
                }
            }
            List<IExpression> list;
            if (!m_Procs.TryGetValue(proc, out list)) {
                list = new List<IExpression>();
                m_Procs.Add(proc, list);
            } else {
                list.Clear();
            }
            foreach (Dsl.ISyntaxComponent comp in func.Statements) {
                var exp = Load(comp);
                if (null != exp) {
                    list.Add(exp);
                }
            }
        }
        public object Calc(string proc, params object[] args)
        {
            object ret = 0;
            List<IExpression> exps;
            if (m_Procs.TryGetValue(proc, out exps)) {
                var si = new StackInfo();
                si.Args = args;
                m_Stack.Push(si);
                try {
                    List<string> names;
                    if (m_ProcArgNames.TryGetValue(proc, out names)) {
                        for (int i = 0; i < names.Count; ++i) {
                            if (i < args.Length)
                                SetVariable(names[i], args[i]);
                            else
                                SetVariable(names[i], null);
                        }
                    }
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
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex5.Message, ex5.StackTrace);
                            OutputInnerException(ex5);
                        }
                        catch (FileNotFoundException ex4) {
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex4.Message, ex4.StackTrace);
                            OutputInnerException(ex4);
                        }
                        catch (IOException ex3) {
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex3.Message, ex3.StackTrace);
                            OutputInnerException(ex3);
                            ret = -1;
                        }
                        catch (UnauthorizedAccessException ex2) {
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex2.Message, ex2.StackTrace);
                            OutputInnerException(ex2);
                            ret = -1;
                        }
                        catch (NotSupportedException ex1) {
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex1.Message, ex1.StackTrace);
                            OutputInnerException(ex1);
                            ret = -1;
                        }
                        catch (Exception ex) {
                            Debug.LogErrorFormat("calc:[{0}] exception:{1}\n{2}", exp.ToString(), ex.Message, ex.StackTrace);
                            OutputInnerException(ex);
                            ret = -1;
                            break;
                        }
                    }
                }
                finally {
                    m_Stack.Pop();
                }
            }
            return ret;
        }
        public RunStateEnum RunState
        {
            get { return m_RunState; }
            internal set { m_RunState = value; }
        }
        public IList<object> Arguments
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.Args;
            }
        }
        public bool TryGetVariable(int v, out object result)
        {
            return Variables.TryGetValue(v, out result);
        }
        public object GetVariable(int v)
        {
            object result = null;
            Variables.TryGetValue(v, out result);
            return result;
        }
        public void SetVariable(int v, object val)
        {
            Variables[v] = val;
        }
        public bool RemoveVariable(int v)
        {
            return Variables.Remove(v);
        }
        public bool TryGetVariable(string v, out object result)
        {
            bool ret = false;
            if (v.Length > 0) {
                if (v[0] == '@') {
                    ret = TryGetGlobalVariable(v, out result);
                }
                else if (v[0] == '$') {
                    ret = NamedVariables.TryGetValue(v, out result);
                }
                else {
                    ret = TryGetGlobalVariable(v, out result);
                }
            }
            else {
                result = null;
            }
            return ret;
        }
        public object GetVariable(string v)
        {
            object result = null;
            if (v.Length > 0) {
                if (v[0] == '@') {
                    result = GetGlobalVariable(v);
                }
                else if (v[0] == '$') {
                    NamedVariables.TryGetValue(v, out result);
                }
                else {
                    result = GetGlobalVariable(v);
                }
            }
            return result;
        }
        public void SetVariable(string v, object val)
        {
            if (v.Length > 0) {
                if (v[0] == '@') {
                    SetGlobalVariable(v, val);
                }
                else if (v[0] == '$') {
                    NamedVariables[v] = val;
                }
                else {
                    SetGlobalVariable(v, val);
                }
            }
        }
        public bool RemoveVariable(string v)
        {
            bool ret = false;
            if (v.Length > 0) {
                if (v[0] == '@') {
                    ret = RemoveGlobalVariable(v);
                }
                else if (v[0] == '$') {
                    ret = NamedVariables.Remove(v);
                }
                else {
                    ret = RemoveGlobalVariable(v);
                }
            }
            return ret;
        }
        public IExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            if (null != valueData) {
                int idType = valueData.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    NamedVarGet varExp = new NamedVarGet();
                    varExp.Load(comp, this);
                    return varExp;
                }
                else {
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                }
            }
            else {
                Dsl.CallData callData = comp as Dsl.CallData;
                if (null != callData) {
                    if (!callData.HaveId() && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS || callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                        switch (callData.GetParamClass()) {
                            case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS:
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
                            case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                    ArrayExp exp = new ArrayExp();
                                    exp.Load(comp, this);
                                    return exp;
                                }
                            default:
                                return null;
                        }
                    }
                    else if (!callData.HaveParam()) {
                        //退化
                        valueData = callData.Name;
                        return Load(valueData);
                    }
                    else {
                        int paramClass = callData.GetParamClass();
                        string op = callData.GetId();
                        if (op == "=") {//赋值
                            Dsl.CallData innerCall = callData.GetParam(0) as Dsl.CallData;
                            if (null != innerCall) {
                                //obj.property = val -> dotnetset(obj, property, val)
                                int innerParamClass = innerCall.GetParamClass();
                                if (innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                                    Dsl.CallData newCall = new Dsl.CallData();
                                    newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                    if (innerCall.IsHighOrder) {
                                        newCall.Params.Add(innerCall.Call);
                                        newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                        newCall.Params.Add(callData.GetParam(1));
                                    }
                                    else {
                                        newCall.Params.Add(innerCall.Name);
                                        newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                        newCall.Params.Add(callData.GetParam(1));
                                    }

                                    var setExp = new DotnetSetExp();
                                    setExp.Load(newCall, this);
                                    return setExp;
                                }
                            }
                            IExpression exp = null;
                            string name = callData.GetParamId(0);
                            if (name == "var") {
                                exp = new VarSet();
                            }
                            else {
                                exp = new NamedVarSet();
                            }
                            if (null != exp) {
                                exp.Load(comp, this);
                            }
                            else {
                                //error
                                //Debug.LogErrorFormat("DslCalculator error, {0} line {1}", callData.ToScriptString(false), callData.GetLine());
                            }
                            return exp;
                        }
                        else {
                            if (callData.IsHighOrder) {
                                Dsl.CallData innerCall = callData.Call;
                                int innerParamClass = innerCall.GetParamClass();
                                if (paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS && (
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS)) {
                                    //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                    string apiName;
                                    string member = innerCall.GetParamId(0);
                                    if (member == "orderby" || member == "orderbydesc" || member == "where" || member == "top") {
                                        apiName = "linq";
                                    }
                                    else {
                                        apiName = "dotnetcall";
                                    }
                                    Dsl.CallData newCall = new Dsl.CallData();
                                    newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                    if (innerCall.IsHighOrder) {
                                        newCall.Params.Add(innerCall.Call);
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

                                    if (apiName == "dotnetcall") {
                                        var callExp = new DotnetCallExp();
                                        callExp.Load(newCall, this);
                                        return callExp;
                                    }
                                    else {
                                        var callExp = new LinqExp();
                                        callExp.Load(newCall, this);
                                        return callExp;
                                    }
                                }
                            }
                            if (paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                                //obj.property or obj[property] or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                Dsl.CallData newCall = new Dsl.CallData();
                                newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                if (callData.IsHighOrder) {
                                    newCall.Params.Add(callData.Call);
                                    newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                }
                                else {
                                    newCall.Params.Add(callData.Name);
                                    newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                }

                                var getExp = new DotnetGetExp();
                                getExp.Load(newCall, this);
                                return getExp;
                            }
                        }
                    }
                }
                else {
                    Dsl.FunctionData funcData = comp as Dsl.FunctionData;
                    if (null != funcData) {
                        if (funcData.HaveStatement()) {
                            callData = funcData.Call;
                            if (null == callData || !callData.HaveId() && !callData.HaveParam()) {
                                HashtableExp exp = new HashtableExp();
                                exp.Load(comp, this);
                                return exp;
                            }
                        }
                        else if (!funcData.HaveExternScript()) {
                            //退化
                            callData = funcData.Call;
                            if (callData.HaveParam()) {
                                return Load(callData);
                            }
                            else {
                                valueData = callData.Name;
                                return Load(valueData);
                            }
                        }
                    }
                }
            }
            IExpression ret = Create(comp.GetId());
            if (null != ret) {
                if (!ret.Load(comp, this)) {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                }
            }
            else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
            }
            return ret;
        }
        private Dsl.ISyntaxComponent ConvertMember(Dsl.ISyntaxComponent p, int paramClass)
        {
            var pvd = p as Dsl.ValueData;
            if (null != pvd && pvd.IsId() && (paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD
                || paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_POINTER
                || paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_QUESTION_PERIOD)) {
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
            if (m_ExpressionFactories.TryGetValue(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }
        private void OutputInnerException(Exception ex)
        {
            while (null != ex.InnerException) {
                ex = ex.InnerException;
                Debug.LogErrorFormat("\t=> exception:{0} stack:{1}", ex.Message, ex.StackTrace);
            }
        }

        private Dictionary<int, object> Variables
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.Vars;
            }
        }
        private Dictionary<string, object> NamedVariables
        {
            get {
                var stackInfo = m_Stack.Peek();
                return stackInfo.NamedVars;
            }
        }

        private class StackInfo
        {
            internal IList<object> Args = null;
            internal Dictionary<int, object> Vars = new Dictionary<int, object>();
            internal Dictionary<string, object> NamedVars = new Dictionary<string, object>();
        }

        private RunStateEnum m_RunState = RunStateEnum.Normal;
        private Dictionary<string, List<string>> m_ProcArgNames = new Dictionary<string, List<string>>();
        private Dictionary<string, List<IExpression>> m_Procs = new Dictionary<string, List<IExpression>>();
        private Stack<StackInfo> m_Stack = new Stack<StackInfo>();
        private Dictionary<string, object> m_NamedGlobalVariables = new Dictionary<string, object>();
        private Dictionary<string, IExpressionFactory> m_ExpressionFactories = new Dictionary<string, IExpressionFactory>();

        internal static int CheckStartInterval
        {
            get { return s_CheckStartInterval; }
            set { s_CheckStartInterval = value; }
        }
        internal static List<Task<int>> Tasks
        {
            get { return s_Tasks; }
        }
        internal static int NewProcess(bool noWait, string fileName, string args, ProcessStartOption option, Stream istream, Stream ostream, IList<string> input, StringBuilder output, StringBuilder error, bool redirectToConsole, Encoding encoding)
        {
            if (noWait) {
                var task = Task.Run<int>(() => NewProcessTask(fileName, args, option, istream, ostream, input, output, error, redirectToConsole, encoding));
                s_Tasks.Add(task);
                while (task.Status == TaskStatus.Created || task.Status == TaskStatus.WaitingForActivation || task.Status == TaskStatus.WaitingToRun) {
                    Debug.LogFormat("wait {0}[{1}] start", Path.GetFileName(fileName), task.Status);
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
            //考虑到跨平台兼容性，不使用特定进程环境变量
            try {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                var psi = p.StartInfo;
                psi.FileName = fileName;
                psi.Arguments = args;
                psi.UseShellExecute = option.UseShellExecute;
                if (null != option.Verb) {
                    psi.Verb = option.Verb;
                }
                if (null != option.Domain) {
                    psi.Domain = option.Domain;
                }
                if (null != option.UserName) {
                    psi.UserName = option.UserName;
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
                psi.WindowStyle = option.WindowStyle;
                psi.CreateNoWindow = !option.NewWindow;
                psi.ErrorDialog = option.ErrorDialog;
                psi.WorkingDirectory = option.WorkingDirectory;

                if (null != istream || null != input) {
                    psi.RedirectStandardInput = true;
                }
                if (null != ostream || null != output || redirectToConsole) {
                    psi.RedirectStandardOutput = true;
                    psi.StandardOutputEncoding = encoding;
                    p.OutputDataReceived += (sender, e) => OnOutputDataReceived(sender, e, ostream, output, redirectToConsole, encoding);
                }
                if (null != error || redirectToConsole) {
                    psi.RedirectStandardError = true;
                    psi.StandardErrorEncoding = encoding;
                    p.ErrorDataReceived += (sender, e) => OnErrorDataReceived(sender, e, ostream, error, redirectToConsole, encoding);
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
                        p.StandardOutput.Close();
                    }
                    if (psi.RedirectStandardError) {
                        p.CancelErrorRead();
                        p.StandardError.Close();
                    }
                    int r = p.ExitCode;
                    p.Close();
                    return r;
                }
                else {
                    Debug.LogFormat("process({0} {1}) failed.", fileName, args);
                    return -1;
                }
            }
            catch (Exception ex) {
                Debug.LogFormat("process({0} {1}) exception:{2} stack:{3}", fileName, args, ex.Message, ex.StackTrace);
                while (null != ex.InnerException) {
                    ex = ex.InnerException;
                    Debug.LogFormat("\t=> exception:{0} stack:{1}", ex.Message, ex.StackTrace);
                }
                return -1;
            }
        }

        private static void OnOutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e, Stream ostream, StringBuilder output, bool redirectToConsole, Encoding encoding)
        {
            var p = sender as System.Diagnostics.Process;
            if (p.StartInfo.RedirectStandardOutput) {
                var txt = e.Data;
                if (null != ostream) {
                    var bytes = encoding.GetBytes(txt);
                    ostream.Write(bytes, 0, bytes.Length);
                }
                if (null != output) {
                    output.Append(txt);
                }
            }
        }

        private static void OnErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e, Stream ostream, StringBuilder error, bool redirectToConsole, Encoding encoding)
        {
            var p = sender as System.Diagnostics.Process;
            if (p.StartInfo.RedirectStandardError) {
                var txt = e.Data;
                if (null != ostream) {
                    var bytes = encoding.GetBytes(txt);
                    ostream.Write(bytes, 0, bytes.Length);
                }
                if (null != error) {
                    error.Append(txt);
                }
            }
        }

        internal class ProcessStartOption
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
    }
}
#endregion
#endif