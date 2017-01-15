using ScriptRuntime;
using System;
using System.Collections.Generic;
using GameFramework;

public enum AiStateId : int
{
    Combat = PredefinedAiStateId.MaxValue + 1,
    Gohome,
}
    
public enum AiTargetType : int
{
    NPC = 0,
    HERO,
    BOSS,
    ALL,
}

public enum TargetSelectTypeEnum
{
    RANDOM = 0,
    NEAREST = 1,
}

public sealed class AiLogicUtility
{
    public const long c_MaxComboInterval = 6000;
    public const int c_MaxViewRange = 30;
    public const int c_MaxViewRangeSqr = c_MaxViewRange * c_MaxViewRange;
    
    public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, CharacterRelation relation)
    {
        return GetNearstTargetHelper(srcObj, relation, AiTargetType.ALL);
    }

    public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, float range, CharacterRelation relation)
    {
        return GetNearstTargetHelper(srcObj, range, relation, AiTargetType.ALL);
    }

    public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, CharacterRelation relation, AiTargetType type)
    {
        return GetNearstTargetHelper(srcObj, srcObj.ViewRange, relation, type);
    }

    public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, float range, CharacterRelation relation, AiTargetType type)
    {
        EntityInfo nearstTarget = null;
        float minDistSqr = 999999;
        srcObj.SceneContext.KdTree.QueryWithAction(srcObj, range, (float distSqr, KdTreeObject kdTreeObj) => {
            StepCalcNearstTarget(srcObj, relation, type, distSqr, kdTreeObj.Object, ref minDistSqr, ref nearstTarget);
        });
        return nearstTarget;
    }

    public static EntityInfo GetLivingCharacterInfoHelper(EntityInfo srcObj, int id)
    {
        EntityInfo target = srcObj.EntityManager.GetEntityInfo(id);
        if (null != target) {
            if (target.IsDead())
                target = null;
        }
        return target;
    }

    public static EntityInfo GetSeeingLivingCharacterInfoHelper(EntityInfo srcObj, int id)
    {
        EntityInfo target = srcObj.EntityManager.GetEntityInfo(id);
        if (null != target) {
            if (target.IsDead())
                target = null;
        }
        return target;
    }

    private static void StepCalcNearstTarget(EntityInfo srcObj, CharacterRelation relation, AiTargetType type, float distSqr, EntityInfo obj, ref float minDistSqr, ref EntityInfo nearstTarget)
    {
        EntityInfo target = GetSeeingLivingCharacterInfoHelper(srcObj, obj.GetId());
        if (null != target && !target.IsDead()) {
            if (!target.IsTargetNpc()) {
                return;
            }
            if (type == AiTargetType.HERO && target.EntityType != (int)EntityTypeEnum.Hero) {
                return;
            }
            if (type == AiTargetType.BOSS && target.EntityType != (int)EntityTypeEnum.Boss) {
                return;
            }
            if (type == AiTargetType.NPC && target.EntityType != (int)EntityTypeEnum.Normal) {
                return;
            }

            if (relation == EntityInfo.GetRelation(srcObj, target)) {
                if (srcObj.EntityType == (int)EntityTypeEnum.Hero || !srcObj.IsPassive || srcObj.AttackerInfos.ContainsKey(target.GetId())) {
                    if (distSqr < minDistSqr) {
                        nearstTarget = target;
                        minDistSqr = distSqr;
                    }
                }
            }
        }
    }
    internal static SkillInfo NpcFindCanUseSkill(EntityInfo npc)
    {
        SkillStateInfo skStateInfo = npc.GetSkillStateInfo();
        int priority = -1;
        SkillInfo skInfo = null;
        long curTime = TimeUtility.GetLocalMilliseconds();
        if (npc.AutoSkillIds.Count <= 0)
            return null;
        int randIndex = Helper.Random.Next(0, npc.AutoSkillIds.Count);
        skInfo = skStateInfo.GetSkillInfoById(npc.AutoSkillIds[randIndex]);
        SkillInfo selectSkill = null;
        if (null != skInfo && !skInfo.IsInCd(curTime)) {
            selectSkill = skInfo;
        } else {
            for (int i = 0; i < npc.AutoSkillIds.Count; i++) {
                skInfo = skStateInfo.GetSkillInfoById(npc.AutoSkillIds[i]);
                if (null != skInfo && !skInfo.IsInCd(curTime) && skInfo.ConfigData.skillData.autoCast > priority) {
                    selectSkill = skInfo;
                    priority = skInfo.ConfigData.skillData.autoCast;
                }
            }
        }
        return selectSkill;
    }
}