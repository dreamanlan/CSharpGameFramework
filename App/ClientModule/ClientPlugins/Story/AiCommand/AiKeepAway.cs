using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using SkillSystem;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class AiKeepAway : ISimpleStoryApiPlugin
{
    public void Init(DslCalculatorCodeStructure code)
    {
        m_CodeStructure = code;
    }

    public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        // Read args every call: story instances are pooled and recycled (Reset does not touch
        // plugin fields), a cached objid would point at a destroyed entity after battle restart.
        m_ObjId = args[0];
        m_SkillInfo = args[1].ObjectVal as SkillInfo;
        m_Ratio = args[2].GetFloat();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc && !npc.IsUnderControl()) {
            AiStateInfo info = npc.GetAiStateInfo();
            EntityInfo target = PluginFramework.Instance.GetEntityById(info.Target);
            if (null != target && null != m_SkillInfo) {
                info.Time += (long)(Time.deltaTime * 1000);
                if (info.Time > 100) {
                    info.Time = 0;
                } else {
                    return true;
                }
                ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
                ScriptRuntime.Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                float distSqr = Geometry.DistanceSquare(srcPos, targetPos);
                ScriptRuntime.Vector3 dir = srcPos - targetPos;
                dir.Normalize();
                targetPos = targetPos + dir * m_Ratio * m_SkillInfo.Distance;
                if (distSqr < m_Ratio * m_Ratio * m_SkillInfo.Distance * m_SkillInfo.Distance) {
                    AiCommand.AiPursue(npc, targetPos);
                    return true;
                }
            }
        }
        return false;
    }

    private DslCalculatorCodeStructure m_CodeStructure = null;
    private int m_ObjId = 0;
    private SkillInfo m_SkillInfo = null;
    private float m_Ratio = 1.0f;
}
