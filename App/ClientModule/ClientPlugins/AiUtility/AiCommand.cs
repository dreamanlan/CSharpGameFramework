using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Skill;
using GameFramework.Story;

public static class AiCommand
{
    public static void NotifyAiDeath(EntityInfo npc)
    {
        EntityViewModel view = EntityViewModelManager.Instance.GetEntityViewById(npc.GetId());
        view.Death();
    }
    public static void AiTarget(EntityInfo npc, EntityInfo target)
    {
        if (null != target) {
            if (null != PluginFramework.Instance.SelectedTarget) {
                EntityInfo curTarget = PluginFramework.Instance.GetEntityById(PluginFramework.Instance.SelectedTarget.TargetId);
                if (curTarget == PluginFramework.Instance.SelectedTarget.Target) {
                    return;
                }
            }
            PluginFramework.Instance.SetLockTarget(target.GetId());
        }
    }
    public static void AiFace(EntityInfo npc)
    {
        if (null != npc) {
            float dir = npc.GetMovementStateInfo().GetFaceDir();
            GameObject actor = EntityViewModelManager.Instance.GetGameObject(npc.GetId());
            actor.transform.localRotation = Quaternion.Euler(0, Geometry.RadianToDegree(dir), 0);
        }
    }
    public static ScriptRuntime.Vector3 AiGetValidPosition(EntityInfo npc, ScriptRuntime.Vector3 target, float maxDistance)
    {
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(new UnityEngine.Vector3(target.X, target.Y, target.Z), out navMeshHit, maxDistance, NavMesh.AllAreas);
        if (!float.IsInfinity(navMeshHit.position.x) && !float.IsInfinity(navMeshHit.position.y) && !float.IsInfinity(navMeshHit.position.z)) {
            return new ScriptRuntime.Vector3(navMeshHit.position.x, navMeshHit.position.y, navMeshHit.position.z);
        }
        return target;
    }
    public static void AiPursue(EntityInfo npc, ScriptRuntime.Vector3 target)
    {
        EntityViewModel npcView = EntityViewModelManager.Instance.GetEntityViewById(npc.GetId());
        npcView.MoveTo(target.X, target.Y, target.Z);
    }
    public static void AiStopPursue(EntityInfo npc)
    {
        EntityViewModel npcView = EntityViewModelManager.Instance.GetEntityViewById(npc.GetId());
        npcView.StopMove();
    }
    public static void AiSelectSkill(EntityInfo npc, SkillInfo skill)
    {
        if (skill == null)
            npc.GetSkillStateInfo().SetCurSkillInfo(0);
        else
            npc.GetSkillStateInfo().SetCurSkillInfo(skill.SkillId);

        Utility.EventSystem.Publish("update_debug_state", "ui", "try use skill:" + skill.SkillId);
    }
    public static void AiSkill(EntityInfo npc, int skillId)
    {
        if (null != npc) {
            /*
            if (npc.GetAiStateInfo().Target > 0) {
                PluginFramework.Instance.SetLockTarget(npc.GetAiStateInfo().Target);
            }
            */
            SkillInfo skillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
            if (null != skillInfo) {
                GfxSkillSystem.Instance.StartSkill(npc.GetId(), skillInfo.ConfigData, 0);
            }
        }
    }
    public static void AiStopSkill(EntityInfo npc)
    {
        if (null != npc) {
            GfxSkillSystem.Instance.StopAllSkill(npc.GetId(), false);
        }
    }
    public static void AiAddImpact(EntityInfo npc, int impactId)
    {
    }
    public static void AiRemoveImpact(EntityInfo npc, int impactId)
    {
    }
    public static void AiSendStoryMessage(EntityInfo npc, string msgId, params object[] args)
    {
        GfxStorySystem.Instance.SendMessage(msgId, args);
    }
}
