using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript.DslExpression;

public class AiGohome : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        if (!m_ParamReaded) {
            m_ParamReaded = true;
            m_ObjId = args[0];
        }
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc) {
            AiStateInfo info = npc.GetAiStateInfo();
            return GohomeHandler(npc, info, (long)(UnityEngine.Time.deltaTime * 1000));
        }
        return false;
    }

    private bool GohomeHandler(EntityInfo npc, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        if (info.Time > 100) {
            info.Time = 0;
        } else {
            return true;
        }

        ScriptRuntime.Vector3 targetPos = info.HomePos;
        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        float distSqr = Geometry.DistanceSquare(srcPos, info.HomePos);
        if (distSqr <= 1) {
            npc.GetMovementStateInfo().IsMoving = false;
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            return false;
        } else {
            npc.GetMovementStateInfo().IsMoving = true;
            npc.GetMovementStateInfo().TargetPosition = targetPos;
            AiCommand.AiPursue(npc, targetPos);
        }
        return true;
    }

    private DslCalculator m_Calculator = null;
    private int m_ObjId = 0;
    private bool m_ParamReaded = false;
}
