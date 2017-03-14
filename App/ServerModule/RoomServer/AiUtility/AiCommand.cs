using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Story;
using GameFrameworkMessage;

public static class AiCommand
{
    public static void NotifyAiDeath(EntityInfo npc)
    {
    }
    public static void AiTarget(EntityInfo npc, EntityInfo target)
    {
        if (null != npc && null != target) {
            npc.GetAiStateInfo().Target = target.GetId();
        }
    }
    public static void AiFace(EntityInfo npc)
    {
        if (null != npc) {
            float dir = npc.GetMovementStateInfo().GetFaceDir();
        }
    }
    public static ScriptRuntime.Vector3 AiGetValidPosition(EntityInfo npc, ScriptRuntime.Vector3 target, float maxDistance)
    {
        return target;
    }
    public static void AiPursue(EntityInfo npc, ScriptRuntime.Vector3 target)
    {
        MovementStateInfo msi = npc.GetMovementStateInfo();
        msi.IsMoving = true;
        msi.TargetPosition = target;
        float dir = Geometry.GetYRadian(msi.GetPosition3D(), target);
        msi.SetFaceDir(dir);
        msi.SetMoveDir(dir);

        Msg_RC_NpcMove npcMoveBuilder = DataSyncUtility.BuildNpcMoveMessage(npc);
        if (null != npcMoveBuilder) {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcMove, npcMoveBuilder);
            }
        }
    }
    public static void AiStopPursue(EntityInfo npc)
    {
        npc.GetMovementStateInfo().IsMoving = false;
    }
    public static void AiSelectSkill(EntityInfo npc, SkillInfo skill)
    {
        if (skill == null)
            npc.GetSkillStateInfo().SetCurSkillInfo(0);
        else
            npc.GetSkillStateInfo().SetCurSkillInfo(skill.SkillId);
    }
    public static void AiSkill(EntityInfo npc, int skillId)
    {
        if (null != npc) {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                SkillInfo skillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                }
            }
        }
    }
    public static void AiStopSkill(EntityInfo npc)
    {
        if (null != npc) {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
            }
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
        if (null != npc) {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                scene.StorySystem.SendMessage(msgId, args);
            }
        }
    }
}
