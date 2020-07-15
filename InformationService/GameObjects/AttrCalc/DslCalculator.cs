using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace GameFramework.AttrCalc
{
    public class AttrExpressionList : List<IAttrExpression>
    {        
        public AttrExpressionList() { }
        public AttrExpressionList(IEnumerable<IAttrExpression> coll) : base(coll) { }
    }
    public interface IAttrExpression
    {
        long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args);
        bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator);
    }
    public interface IAttrExpressionFactory
    {
        IAttrExpression Create();
    }
    public sealed class AttrExpressionFactoryHelper<T> : IAttrExpressionFactory where T : IAttrExpression, new()
    {
        public IAttrExpression Create()
        {
            return new T();
        }
    }
    public abstract class AbstractAttrExpression : IAttrExpression
    {
        public abstract long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args);
        public bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator)
        {
            m_Calculator = calculator;
            SetCalculator(calculator);
            Dsl.ValueData valueData = dsl as Dsl.ValueData;
            if (null != valueData) {
                return Load(valueData);
            } else {
                Dsl.FunctionData callData = dsl as Dsl.FunctionData;
                if (null != callData) {
                    if (Load(callData)) {
                        return true;
                    } else {
                        int num = callData.GetParamNum();
                        AttrExpressionList args = new AttrExpressionList();
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent param = callData.GetParam(ix);
                            args.Add(calculator.Load(param));
                        }
                        return Load(args);
                    }
                } else {
                    Dsl.FunctionData funcData = dsl as Dsl.FunctionData;
                    if (null != funcData) {
                        return Load(funcData);
                    } else {
                        Dsl.StatementData statementData = dsl as Dsl.StatementData;
                        if (null != statementData) {
                            return Load(statementData);
                        }
                    }
                }
            }
            return false;
        }
        protected virtual void SetCalculator(DslCalculator calculator) { }
        protected virtual bool Load(Dsl.ValueData valData) { return false; }
        protected virtual bool Load(AttrExpressionList exps) { return false; }
        protected virtual bool Load(Dsl.FunctionData funcData) { return false; }
        protected virtual bool Load(Dsl.StatementData statementData) { return false; }
        
        protected DslCalculator Calculator
        {
            get { return m_Calculator; }
        }

        private DslCalculator m_Calculator = null;
    }
    internal sealed class VarSet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("var({0})=", m_VarId);
            long v = m_Op.Calc(context, source, target, args);
            m_Variables[m_VarId] = v;
            if (GlobalVariables.s_EnableCalculatorLog) {
                LogSystem.Info("[calculator] assign var({0})={1}", m_VarId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.FunctionData param1 = callData.GetParam(0) as Dsl.FunctionData;
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(param1.GetParamId(0));
            m_Op = Calculator.Load(param2);
            return true;
        }

        private Dictionary<int, long> m_Variables;
        private int m_VarId;
        private IAttrExpression m_Op;
    }
    internal sealed class VarGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long ret = 0;
            m_Variables.TryGetValue(m_VarId, out ret);
            Calculator.Log(ret);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] var({0})={1}", m_VarId, ret);
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(callData.GetParamId(0));
            return true;
        }

        private Dictionary<int, long> m_Variables;
        private int m_VarId;
    }
    internal sealed class AttrSet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("attr({0})=", m_PropId);
            long v = m_Op.Calc(context, source, target, args);
            TableConfig.AttrDefine cfg = TableConfig.AttrDefineProvider.Instance.GetAttrDefine((int)m_PropId);
            if (null != cfg) {
                if (v < cfg.minValue)
                    v = cfg.minValue;
                if (v > cfg.maxValue)
                    v = cfg.maxValue;
            }
            source.SetLong(m_PropId, v);
            if (GlobalVariables.s_EnableCalculatorLog) {
                LogSystem.Info("[calculator] assign attr({0})={1}", m_PropId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.FunctionData param1 = callData.GetParam(0) as Dsl.FunctionData;
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_PropId = (CharacterPropertyEnum)int.Parse(param1.GetParamId(0));
            m_Op = Calculator.Load(param2);
            return true;
        }

        private CharacterPropertyEnum m_PropId;
        private IAttrExpression m_Op;
    }
    internal sealed class AttrGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = source.GetLong(m_PropId);
            source.SetLong(m_PropId, v);
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] attr({0})={1}", m_PropId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_PropId = (CharacterPropertyEnum)int.Parse(callData.GetParamId(0));
            return true;
        }

        private CharacterPropertyEnum m_PropId;
    }
    internal sealed class Attr2Get : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = target.GetLong(m_PropId);
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] attr2({0})={1}", m_PropId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_PropId = (CharacterPropertyEnum)int.Parse(callData.GetParamId(0));
            return true;
        }

        private CharacterPropertyEnum m_PropId;
    }
    internal sealed class ArgGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            if (m_Index >= 0 && m_Index < args.Length) {
                v = args[m_Index];
            }
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] arg({0})={1}", m_Index, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_Index = int.Parse(callData.GetParamId(0));
            return true;
        }

        private int m_Index;
    }
    internal sealed class CampGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("camp(");
            long v1 = m_Arg.Calc(context, source, target, args);
            Calculator.Log(")");
            EntityInfo info = context.EntityManager.GetEntityInfo((int)v1);
            long v = 0;
            if (null != info) {
                v = info.GetCampId();
            }
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] camp({0})={1}", v1, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Arg = exps[0];
            return true;
        }

        private IAttrExpression m_Arg;
    }
    internal sealed class FindExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("find(");
            long v = 0;
            if (null != m_Op1) {
                m_TableId = (int)m_Op1.Calc(context, source, target, args);
            } else {
                Calculator.Log(m_TableId);
            }
            for (var node = context.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo info = node.Value;
                if (null != info && info.GetTableId() == m_TableId) {
                    v = info.GetId();
                    break;
                }
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] find({0})={1}", m_TableId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            var syntaxComp = callData.GetParam(0);
            var valData = syntaxComp as Dsl.ValueData;
            if (null != valData) {
                m_TableId = int.Parse(valData.GetId());
                return true;
            } else {
                return false;
            }
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private int m_TableId;
        private IAttrExpression m_Op1;
    }
    internal sealed class ValueGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            TableConfig.Const cfg = TableConfig.ConstProvider.Instance.GetConst((int)m_PropId);
            if (null != cfg) {
                v = cfg.value;
            }
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] value({0})={1}", m_PropId, v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_PropId = int.Parse(callData.GetParamId(0));
            return true;
        }

        private int m_PropId;
    }
    internal sealed class SelfExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = source.Owner.GetId();
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] self()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class TargetExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = target.Owner.GetId();
            Calculator.Log(v);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] target()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class DamageExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("damage(");
            long v = 0;
            long val = m_Op.Calc(context, source, target, args);
            TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill((int)val);
            if (null != cfg)
            {
                int multipleCt = cfg.skillData.multiple.Count;
                int damageCt = cfg.skillData.damage.Count;
                for (int i = 0; i < damageCt; ++i) {
                    long multiple = 1;
                    if (i < multipleCt)
                        multiple = cfg.skillData.multiple[i];
                    long damage = cfg.skillData.damage[i];
                    v += AttrCalculator.Calc(context, source, target, "phydamage", multiple, damage, 0, 0);
                    v += AttrCalculator.Calc(context, source, target, "magdamage", multiple, damage, 0, 0);
                }
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Info("[calculator] damage({0})={1}", val, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IAttrExpression m_Op;
    }
    internal sealed class ConstGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = m_Val;
            Calculator.Log(v);
            return v;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_Val = long.Parse(valData.GetId());
            return true;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_Val = long.Parse(callData.GetParamId(0));
            return true;
        }

        private long m_Val;
    }
    internal sealed class SkillGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("skill(");
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(")");
            long v = 0;
            if (null != source && null != source.Owner) {
                var list = source.Owner.AutoSkillIds;
                if (v1 >= 0 && v1 < list.Count) {
                    v = list[(int)v1];
                }
            }
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] skill({0})={1}", v1, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IAttrExpression m_Op1;
    }
    internal sealed class Skill2Get : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("skill2(");
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(")");
            long v = 0;
            if (null != target && null != target.Owner) {
                var list = target.Owner.AutoSkillIds;
                if (v1 >= 0 && v1 < list.Count) {
                    v = list[(int)v1];
                }
            }
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] skill2({0})={1}", v1, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IAttrExpression m_Op1;
    }
    internal sealed class ActorGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("actor(");
            if (null != source && null != source.Owner) {
                v = source.Owner.GetTableId();
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] actor()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class Actor2Get : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("actor2(");
            if (null != target && null != target.Owner) {
                v = target.Owner.GetTableId();
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] actor2()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class UnitGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("unit(");
            if (null != source && null != source.Owner) {
                v = source.Owner.GetUnitId();
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] unit()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class Unit2Get : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("unit2(");
            if (null != target && null != target.Owner) {
                v = target.Owner.GetUnitId();
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] unit2()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class TeamPosGet : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("teampos(");
            if (null != source && null != source.Owner) {
                v = source.Owner.GetMovementStateInfo().FormationIndex;
            }
            Calculator.Log("(");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] teampos()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class TeamPos2Get : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("teampos2(");
            if (null != target && null != target.Owner) {
                v = target.Owner.GetMovementStateInfo().FormationIndex;
            }
            Calculator.Log(")");
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                LogSystem.Warn("[calculator] teampos2()={0}", v);
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    internal sealed class AddExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log("+");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 + v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] {0}+{1}={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class SubExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log("-");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 - v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] {0}-{1}={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class MulExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log("*");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 * v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] {0}*{1}={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class DivExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log("/");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 / v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] {0}/{1}={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class ModExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log("%");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 % v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] {0}%{1}={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class MaxExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("max(");
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(",");
            long v2 = m_Op2.Calc(context, source, target, args);
            Calculator.Log(")");
            long v = v1 >= v2 ? v1 : v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] max({0},{1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class MinExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("min(");
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(",");
            long v2 = m_Op2.Calc(context, source, target, args);
            Calculator.Log(")");
            long v = v1 <= v2 ? v1 : v2;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] min({0},{1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class AbsExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("abs(");
            long v1 = m_Op.Calc(context, source, target, args);
            Calculator.Log(")");
            long v = v1 >= 0 ? v1 : -v1;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] abs({0})={1}", v1, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IAttrExpression m_Op;
    }
    internal sealed class ClampExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("clamp(");
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(",");
            long v2 = m_Op2.Calc(context, source, target, args);
            Calculator.Log(",");
            long v3 = m_Op3.Calc(context, source, target, args);
            Calculator.Log(")");
            long v;
            if (v3 < v1)
                v = v1;
            else if (v3 > v2)
                v = v2;
            else
                v = v3;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] clamp({0},{1},{2})={3}", v1, v2, v3, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
        private IAttrExpression m_Op3;
    }
    internal sealed class GreatExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" > ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 > v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} > {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class GreatEqualExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" >= ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 >= v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} >= {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class LessExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" < ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 < v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} < {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class LessEqualExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" <= ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 <= v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} <= {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class EqualExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" == ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 == v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} == {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class NotEqualExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            Calculator.Log(" != ");
            long v2 = m_Op2.Calc(context, source, target, args);
            long v = v1 != v2 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} != {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class AndExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);            
            long v2 = 0;
            long v = 0;
            if(!GlobalVariables.s_EnableCalculatorDetailLog){
                v = v1 != 0 && (v2 = m_Op2.Calc(context, source, target, args)) != 0 ? 1 : 0;
            } else {
                Calculator.Log(" && ");
                v2 = m_Op2.Calc(context, source, target, args);
                v = v1 != 0 && v2 != 0 ? 1 : 0;
            }
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} && {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class OrExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            long v2 = 0;
            long v = 0;
            if (!GlobalVariables.s_EnableCalculatorDetailLog) {
                v = v1 != 0 || (v2 = m_Op2.Calc(context, source, target, args)) != 0 ? 1 : 0;
            } else {
                Calculator.Log(" || ");
                v2 = m_Op2.Calc(context, source, target, args);
                v = v1 != 0 || v2 != 0 ? 1 : 0;
            }
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} || {1})={2}", v1, v2, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IAttrExpression m_Op1;
        private IAttrExpression m_Op2;
    }
    internal sealed class NotExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("!");
            long val = m_Op.Calc(context, source, target, args);
            long v = val == 0 ? 1 : 0;
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] (!{0})={1}", val, v);
            }
            return v;
        }
        protected override bool Load(AttrExpressionList exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IAttrExpression m_Op;
    }
    internal sealed class CondExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v1 = m_Op1.Calc(context, source, target, args);
            long v2 = 0;
            long v3 = 0;
            long v = 0;
            if (!GlobalVariables.s_EnableCalculatorDetailLog) {
                v = v1 != 0 ? v2 = m_Op2.Calc(context, source, target, args) : v3 = m_Op3.Calc(context, source, target, args);
            } else {
                Calculator.Log(" ? ");
                v2 = m_Op2.Calc(context, source, target, args);
                Calculator.Log(" : ");
                v3 = m_Op3.Calc(context, source, target, args);
                v = v1 != 0 ? v2 : v3;
            }
            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                LogSystem.Info("[calculator] ({0} ? {1} : {2})={3}", v1, v2, v3, v);
            }
            return v;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            Dsl.FunctionData funcData1 = statementData.First;
            Dsl.FunctionData funcData2 = statementData.Second;
            if (funcData2.GetId() == ":") {
                Dsl.ISyntaxComponent cond = funcData1.LowerOrderFunction.GetParam(0);
                Dsl.ISyntaxComponent op1 = funcData1.GetParam(0);
                Dsl.ISyntaxComponent op2 = funcData2.GetParam(0);
                m_Op1 = Calculator.Load(cond);
                m_Op2 = Calculator.Load(op1);
                m_Op3 = Calculator.Load(op2);
            } else {
                //error
                LogSystem.Error("DslCalculator error, {0} line {1}", statementData.ToScriptString(false), statementData.GetLine());
            }
            return true;
        }

        private IAttrExpression m_Op1 = null;
        private IAttrExpression m_Op2 = null;
        private IAttrExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            bool matched = false;
            long v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    if (ix == 0) {
                        Calculator.Log("if(");
                    } else {
                        Calculator.Log("} elseif(");
                    }
                    long condVal = clause.Condition.Calc(context, source, target, args);
                    if (condVal != 0) {
                        matched = true;
                    }
                    Calculator.LogLine(") {");
                    if (!GlobalVariables.s_EnableCalculatorDetailLog) {
                        if (condVal != 0) {
                            for (int index = 0; index < clause.Expressions.Count; ++index) {
                                v = clause.Expressions[index].Calc(context, source, target, args);
                            }
                            if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                                LogSystem.Info("[calculator] ifexp[{0}]({1})={2}", ix, condVal, v);
                            }
                            break;
                        }
                    } else {
                        Calculator.Indent();
                        int ct = clause.Expressions.Count;
                        for (int index = 0; index < ct; ++index) {
                            Calculator.LogIndent();
                            long t = clause.Expressions[index].Calc(context, source, target, args);
                            if (condVal != 0) {
                                v = t;
                            }
                            Calculator.LogLine(";");
                        }
                        Calculator.Unindent();
                        if (condVal != 0 && GlobalVariables.s_EnableCalculatorOperatorLog) {
                            LogSystem.Info("[calculator] ifexp[{0}]({1})={2}", ix, condVal, v);
                        }
                    }
                } else if (ix == m_Clauses.Count - 1) {
                    if (!GlobalVariables.s_EnableCalculatorDetailLog) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            v = clause.Expressions[index].Calc(context, source, target, args);
                        }
                        if (GlobalVariables.s_EnableCalculatorOperatorLog) {
                            LogSystem.Info("[calculator] ifexp[{0}](else)={1}", ix, v);
                        }
                        break;
                    } else {
                        Calculator.Indent();
                        int ct = clause.Expressions.Count;
                        for (int index = 0; index < ct; ++index) {
                            Calculator.LogIndent();
                            long t = clause.Expressions[index].Calc(context, source, target, args);
                            if (!matched) {
                                v = t;
                            }
                            Calculator.LogLine(";");
                        }
                        Calculator.Unindent();
                        if (!matched && GlobalVariables.s_EnableCalculatorOperatorLog) {
                            LogSystem.Info("[calculator] ifexp[{0}](else)={1}", ix, v);
                        }

                    }
                }
            }
            Calculator.LogIndent();
            Calculator.Log("}");
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                Dsl.ISyntaxComponent cond = funcData.LowerOrderFunction.GetParam(0);
                IfExp.Clause item = new IfExp.Clause();
                item.Condition = Calculator.Load(cond);
                for (int ix = 0; ix < funcData.GetParamNum(); ++ix) {
                    IAttrExpression subExp = Calculator.Load(funcData.GetParam(ix));
                    item.Expressions.Add(subExp);
                }
                m_Clauses.Add(item);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            foreach (var fData in statementData.Functions) {
                if (fData.IsHighOrder && (fData.GetId() == "if" || fData.GetId() == "elseif")) {
                    IfExp.Clause item = new IfExp.Clause();
                    if (fData.LowerOrderFunction.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = fData.LowerOrderFunction.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    } else {
                        //error
                        LogSystem.Error("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    }
                    for (int ix = 0; ix < fData.GetParamNum(); ++ix) {
                        IAttrExpression subExp = Calculator.Load(fData.GetParam(ix));
                        item.Expressions.Add(subExp);
                    }
                    m_Clauses.Add(item);
                } else if (fData.GetId() == "else") {
                    if (fData != statementData.Last) {
                        //error
                        LogSystem.Error("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    } else {
                        IfExp.Clause item = new IfExp.Clause();
                        for (int ix = 0; ix < fData.GetParamNum(); ++ix) {
                            IAttrExpression subExp = Calculator.Load(fData.GetParam(ix));
                            item.Expressions.Add(subExp);
                        }
                        m_Clauses.Add(item);
                    }
                } else {
                    //error
                    LogSystem.Error("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                }
            }
            return true;
        }

        private sealed class Clause
        {
            internal IAttrExpression Condition;
            internal AttrExpressionList Expressions = new AttrExpressionList();
        }

        private List<Clause> m_Clauses = new List<Clause>();
    }
    internal sealed class ParenthesisExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            Calculator.Log("(");
            long v = 0;
            int ct = m_Expressions.Count;
            for (int ix = 0; ix < ct; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc(context, source, target, args);
                if (ix < ct - 1) {
                    Calculator.Log(",");
                }
            }
            Calculator.Log(")");
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

        private AttrExpressionList m_Expressions = new AttrExpressionList();
    }
    internal sealed class CommandExp : AbstractAttrExpression
    {
        public override long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args)
        {
            long v = 0;
            Calculator.Log("command([");
            CalculatorCommandInfo cmdInfo = new CalculatorCommandInfo();
            cmdInfo.Strings.AddRange(m_Strings);
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                Calculator.Log(string.Join(",", m_Strings.ToArray()));
            }
            Calculator.Log("],[");
            int ct = m_Expressions.Count;
            for (int ix = 0; ix < ct; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc(context, source, target, args);
                cmdInfo.Args.Add(v);
                if (ix < ct - 1) {
                    Calculator.Log(",");
                }
            }
            context.CommandInfos.Add(cmdInfo);
            Calculator.Log("])");
            return v;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            Dsl.FunctionData strParams = callData.GetParam(0) as Dsl.FunctionData;
            if (null != strParams) {
                foreach (Dsl.ISyntaxComponent str in strParams.Params) {
                    m_Strings.Add(str.GetId());
                }
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                IAttrExpression arg = Calculator.Load(callData.GetParam(i));
                m_Expressions.Add(arg);
            }
            return true;
        }

        private List<string> m_Strings = new List<string>();
        private AttrExpressionList m_Expressions = new AttrExpressionList();
    }
    public sealed class DslCalculator
    {
        public void Load(string dslFile)
        {
            Dsl.DslFile file = new Dsl.DslFile();
            string path = HomePath.GetAbsolutePath(dslFile);
            if (file.Load(path, LogSystem.Log)) {
                foreach (Dsl.ISyntaxComponent info in file.DslInfos) {
                    LoadDsl(info);
                }
            }
        }
        public long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, string proc, params long[] args)
        {
            m_Indent = 0;
            m_LogBuilder.Length = 0;

            if (GlobalVariables.s_EnableCalculatorLog) {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Length; ++i) {
                    sb.Append(args[i].ToString());
                    if (i + 1 < args.Length) {
                        sb.Append(",");
                    }
                }
                LogSystem.Info("[calculator] begin calc proc {0} ({1})", proc, sb.ToString());
            }
            long ret = 0;
            m_Variables.Clear();
            AttrExpressionList exps;
            if (m_Procs.TryGetValue(proc, out exps)) {
                for (int i = 0; i < exps.Count; ++i) {
                    LogLine();
                    Indent();
                    LogIndent();
                    ret = exps[i].Calc(context, source, target, args);
                    Unindent();
                    LogLine(";");
                    if (GlobalVariables.s_EnableCalculatorDetailLog) {
                        LogSystem.Info("{0}", m_LogBuilder.ToString());
                        m_LogBuilder.Length = 0;
                    }
                    if (GlobalVariables.s_EnableCalculatorLog) {
                        LogSystem.Info("[calculator] calc proc {0} index {1} result {2}", proc, i, ret);
                    }
                }
            }
            if (GlobalVariables.s_EnableCalculatorLog) {
                LogSystem.Info("[calculator] end calc proc {0}", proc);
            }
            return ret;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal void Log(string fmt, params object[] args)
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.AppendFormat(fmt, args);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Log(string val)
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.Append(val);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Log(long val)
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.Append(val);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Log(int val)
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.Append(val);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void LogLine(string line)
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.AppendLine(line);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void LogLine()
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.AppendLine();
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void LogIndent()
        {
            if (GlobalVariables.s_EnableCalculatorDetailLog) {
                m_LogBuilder.AppendFormat("{0}", c_IndentString.Substring(0, m_Indent));
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Indent()
        {
            ++m_Indent;
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Unindent()
        {
            --m_Indent;
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Indent(int indent)
        {
            m_Indent += indent;
        }
        [System.Diagnostics.Conditional("DEBUG")]
        internal void Unindent(int indent)
        {
            m_Indent -= indent;
        }

        internal Dictionary<int, long> Variables
        {
            get { return m_Variables; }
        }
        internal IAttrExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            if (null != valueData) {
                if (valueData.GetIdType() == Dsl.ValueData.NUM_TOKEN) {
                    //普通常量
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                } else {
                    //error
                    LogSystem.Error("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                    return null;
                }
            } else {
                Dsl.FunctionData callData = comp as Dsl.FunctionData;
                if (null != callData) {
                    if (!callData.HaveId()) {
#if DEBUG
                        ParenthesisExp exp = new ParenthesisExp();
                        exp.Load(comp, this);
                        return exp;
#else
                        int num = callData.GetParamNum();
                        if (num == 1) {
                            Dsl.ISyntaxComponent param = callData.GetParam(0);
                            return Load(param);
                        } else {
                            ParenthesisExp exp = new ParenthesisExp();
                            exp.Load(comp, this);
                            return exp;
                        }
#endif
                    } else {
                        string op = callData.GetId();
                        if (op == "=") {//赋值
                            string name = callData.GetParamId(0);
                            IAttrExpression exp = null;
                            if (name == "attr") {
                                exp = new AttrSet();
                            } else if (name == "var") {
                                exp = new VarSet();
                            }
                            if (null != exp) {
                                exp.Load(comp, this);
                            } else {
                                //error
                                LogSystem.Error("DslCalculator error, {0} line {1}", callData.ToScriptString(false), callData.GetLine());
                            }
                            return exp;
                        }
                    }
                }
            }
            IAttrExpression ret = Create(comp.GetId());
            if (null != ret) {
                ret.Load(comp, this);
            } else {
                //error
                LogSystem.Error("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
            }
            return ret;
        }

        private IAttrExpression Create(string name)
        {
            IAttrExpression ret = null;
            IAttrExpressionFactory factory;
            if (s_ExpressionFactories.TryGetValue(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }
        private void LoadDsl(Dsl.ISyntaxComponent info)
        {
            var func = info as Dsl.FunctionData;
            var stData = info as Dsl.StatementData;
            if(null==func && null != stData) {
                func = stData.First;
            }
            if (null == func || !func.IsHighOrder)
                return;
            string id = func.LowerOrderFunction.GetParamId(0);
            AttrExpressionList list;
            if (!m_Procs.TryGetValue(id, out list)) {
                list = new AttrExpressionList();
                m_Procs.Add(id, list);
            }
            foreach (Dsl.ISyntaxComponent comp in func.Params) {
                var exp = Load(comp);
                if (null != exp) {
                    list.Add(exp);
                }
            }
        }

        private Dictionary<string, AttrExpressionList> m_Procs = new Dictionary<string, AttrExpressionList>();
        private Dictionary<int, long> m_Variables = new Dictionary<int, long>();
        private StringBuilder m_LogBuilder = new StringBuilder();
        private int m_Indent = 0;
        
        public static void Register()
        {
            if (s_Inited)
                return;
            s_Inited = true;

            Register("attr", new AttrExpressionFactoryHelper<AttrGet>());
            Register("attr2", new AttrExpressionFactoryHelper<Attr2Get>());
            Register("value", new AttrExpressionFactoryHelper<ValueGet>());
            Register("arg", new AttrExpressionFactoryHelper<ArgGet>());
            Register("var", new AttrExpressionFactoryHelper<VarGet>());
            Register("const", new AttrExpressionFactoryHelper<ConstGet>());
            Register("skill", new AttrExpressionFactoryHelper<SkillGet>());
            Register("skill2", new AttrExpressionFactoryHelper<Skill2Get>());
            Register("actor", new AttrExpressionFactoryHelper<ActorGet>());
            Register("actor2", new AttrExpressionFactoryHelper<Actor2Get>());
            Register("unit", new AttrExpressionFactoryHelper<UnitGet>());
            Register("unit2", new AttrExpressionFactoryHelper<Unit2Get>());
            Register("teampos", new AttrExpressionFactoryHelper<TeamPosGet>());
            Register("teampos2", new AttrExpressionFactoryHelper<TeamPos2Get>());
            Register("actor", new AttrExpressionFactoryHelper<ActorGet>());
            Register("unit", new AttrExpressionFactoryHelper<UnitGet>());
            Register("camp", new AttrExpressionFactoryHelper<CampGet>());
            Register("find", new AttrExpressionFactoryHelper<FindExp>());
            Register("+", new AttrExpressionFactoryHelper<AddExp>());
            Register("-", new AttrExpressionFactoryHelper<SubExp>());
            Register("*", new AttrExpressionFactoryHelper<MulExp>());
            Register("/", new AttrExpressionFactoryHelper<DivExp>());
            Register("%", new AttrExpressionFactoryHelper<ModExp>());
            Register("max", new AttrExpressionFactoryHelper<MaxExp>());
            Register("min", new AttrExpressionFactoryHelper<MinExp>());
            Register("abs", new AttrExpressionFactoryHelper<AbsExp>());
            Register("clamp", new AttrExpressionFactoryHelper<ClampExp>());
            Register("self", new AttrExpressionFactoryHelper<SelfExp>());
            Register("target", new AttrExpressionFactoryHelper<TargetExp>());
            Register("damage", new AttrExpressionFactoryHelper<DamageExp>());
            Register(">", new AttrExpressionFactoryHelper<GreatExp>());
            Register(">=", new AttrExpressionFactoryHelper<GreatEqualExp>());
            Register("<", new AttrExpressionFactoryHelper<LessExp>());
            Register("<=", new AttrExpressionFactoryHelper<LessEqualExp>());
            Register("==", new AttrExpressionFactoryHelper<EqualExp>());
            Register("!=", new AttrExpressionFactoryHelper<NotEqualExp>());
            Register("&&", new AttrExpressionFactoryHelper<AndExp>());
            Register("||", new AttrExpressionFactoryHelper<OrExp>());
            Register("!", new AttrExpressionFactoryHelper<NotExp>());
            Register("?", new AttrExpressionFactoryHelper<CondExp>());
            Register("if", new AttrExpressionFactoryHelper<IfExp>());
            Register("command", new AttrExpressionFactoryHelper<CommandExp>());
        }
        public static void Register(string name, IAttrExpressionFactory factory)
        {
            if (!s_ExpressionFactories.ContainsKey(name)) {
                s_ExpressionFactories.Add(name, factory);
            } else {
                s_ExpressionFactories[name] = factory;
            }
        }

        private static bool s_Inited = false;
        private static Dictionary<string, IAttrExpressionFactory> s_ExpressionFactories = new Dictionary<string, IAttrExpressionFactory>();
        private const string c_IndentString = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
    }
}
