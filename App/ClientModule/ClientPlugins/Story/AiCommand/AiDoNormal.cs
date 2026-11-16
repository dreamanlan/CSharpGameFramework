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

public class AiDoNormal : ISimpleStoryApiPlugin
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
        if (args.Count > 1) {
            m_EnableLearning = args[1].GetInt() != 0;
        }
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc && !npc.IsUnderControl()) {
            AiStateInfo info = npc.GetAiStateInfo();
            switch (info.CurState) {
                case (int)PredefinedAiStateId.Idle:
                    info.ChangeToState((int)AiStateId.Combat);
                    return true;
                case (int)AiStateId.Combat:
                    return CombatHandler(npc, info, (long)(Time.deltaTime * 1000));
                case (int)AiStateId.Gohome:
                    return GohomeHandler(npc, info, (long)(Time.deltaTime * 1000));
            }
        }
        return false;
    }

    private bool CombatHandler(EntityInfo npc, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        if (info.Time > 100) {
            info.Time = 0;
        } else {
            return true;
        }

        if (npc.GetSkillStateInfo().IsSkillActivated()) {
            return true;
        }

        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        float distSqrToHome = Geometry.DistanceSquare(srcPos, info.HomePos);
        if (distSqrToHome > npc.GohomeRange * npc.GohomeRange) {
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)AiStateId.Gohome);
            return true;
        }

        ///
        EntityInfo attackTarget = null;
        SkillStateInfo currSkInfo = npc.GetSkillStateInfo();
        ///Find skills you can use
        SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc);
        AiCommand.AiSelectSkill(npc, skInfo);
        if (skInfo == null) {
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            return false;
        }

        CharacterRelation relation = skInfo.ConfigData.targetType == (int)SkillTargetType.Friend ?
                CharacterRelation.RELATION_FRIEND :
                CharacterRelation.RELATION_ENEMY;

        attackTarget = AiLogicUtility.GetNearstTargetHelper(
            npc, skInfo.Distance, relation);

        if (attackTarget != null && null != skInfo) //Find an attackable target within the attack range
            {
            info.Target = attackTarget.GetId();
            ScriptRuntime.Vector3 targetPos = attackTarget.GetMovementStateInfo().GetPosition3D();
            float dir = Geometry.GetYRadian(srcPos, targetPos);
            float curDir = npc.GetMovementStateInfo().GetFaceDir();
            if (Mathf.Abs(dir - curDir) > 0.157f) {
                npc.GetMovementStateInfo().SetWantedFaceDir(dir);
            } else {
                AiCommand.AiStopPursue(npc);
                AiCommand.AiSkill(npc, skInfo.SkillId); //attach the target
            }
            return true;
        }
        attackTarget = AiLogicUtility.GetNearstTargetHelper(
        npc, npc.ViewRange, relation);
        if (attackTarget != null) //Find an attackable target within your field of vision
            {
            ScriptRuntime.Vector3 tp = attackTarget.GetMovementStateInfo().GetPosition3D();
            // Every mob used to path to the exact same point, stacking the
            // whole wave into one blob. Spread pursuers around the target.
            float a = (npc.GetId() % 8) * (Mathf.PI * 2f / 8f);
            tp.X += Mathf.Cos(a) * 4.0f;
            tp.Z += Mathf.Sin(a) * 4.0f;
            AiCommand.AiPursue(npc, tp);
            return true;
        }

        currSkInfo.SetCurSkillInfo(0);
        AiCommand.AiStopPursue(npc);
        return true;
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
    
    private DslCalculatorCodeStructure m_CodeStructure = null;
    private int m_ObjId = 0;
    private bool m_EnableLearning = false;
    private const long c_IntervalTime = 200;
}
