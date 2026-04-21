using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    internal sealed class PropSetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            if (m_OpNum == 1) {
                if (!Calculator.TryGetVariable(m_VarId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                Calculator.SetVariable(m_VarId, BoxedValue.NullObject);
                return v;
            }
            else if (m_OpNum == 2) {
                if (!Calculator.TryGetVariable(m_VarId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                var vv = m_Op.Calc();
                Calculator.SetVariable(m_VarId, vv);
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
    internal sealed class PropGetExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            if (m_OpNum == 1) {
                if (!Calculator.TryGetVariable(m_VarId, out var v)) {
                    v = BoxedValue.NullObject;
                }
                return v;
            }
            else if (m_OpNum == 2) {
                var vv = m_Op.Calc();
                if (!Calculator.TryGetVariable(m_VarId, out var v)) {
                    v = vv;
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
}
