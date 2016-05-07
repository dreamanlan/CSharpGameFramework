using ScriptRuntime;
using System;
using System.Collections.Generic;

namespace GameFramework
{
    public enum AiTargetType : int
    {
        NPC = 0,
        HERO,
        TOWER,
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
        public const int c_MaxViewRange = 30;
        public const int c_MaxViewRangeSqr = c_MaxViewRange * c_MaxViewRange;

        public static EntityInfo GetNearstAttackerHelper(EntityInfo srcObj, CharacterRelation relation, AiData_General aidata)
        {
            EntityInfo ret = null;
            float minDistSqr = 999999;
            Vector2 dir = srcObj.GetMovementStateInfo().GetFaceDir2D();
            Vector2 pos = srcObj.GetMovementStateInfo().GetPosition2D();
            if (relation == CharacterRelation.RELATION_ENEMY) {
                foreach (var pair in srcObj.AttackerInfos) {
                    EntityInfo target = srcObj.SceneContext.GetEntityById(pair.Key);
                    if (null != target) {
                        float distSqr = Geometry.DistanceSquare(pos, target.GetMovementStateInfo().GetPosition2D());
                        if (distSqr <= (srcObj.ViewRange + target.GetRadius()) * (srcObj.ViewRange + target.GetRadius())) {
                            if (distSqr <= minDistSqr) {
                                ret = target;
                                minDistSqr = distSqr;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, CharacterRelation relation)
        {
            return GetNearstTargetHelper(srcObj, relation, AiTargetType.ALL);
        }

        public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, float range,CharacterRelation relation)
        {
            return GetHearstTargetHelper(srcObj, range, relation, AiTargetType.ALL);
        }

        public static EntityInfo GetNearstTargetHelper(EntityInfo srcObj, CharacterRelation relation, AiTargetType type)
        {
            return GetHearstTargetHelper(srcObj,srcObj.ViewRange,relation,type);
        }

        public static EntityInfo GetHearstTargetHelper(EntityInfo srcObj,float range,CharacterRelation relation, AiTargetType type)
        {
            EntityInfo nearstTarget = null;
            float minPowDist = 999999;
            srcObj.SceneContext.KdTree.Query(srcObj, range, (float distSqr, KdTreeObject kdTreeObj) => {
                StepCalcNearstTarget(srcObj, relation, type, distSqr, kdTreeObj.Object, ref minPowDist, ref nearstTarget);
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
                if (target.IsHaveStateFlag(CharacterState_Type.CST_Hidden))
                    target = null;
                else if (target.IsDead())
                    target = null;
                else if (!CanSee(srcObj, target))
                    target = null;
            }
            return target;
        }

        private static void StepCalcNearstTarget(EntityInfo srcObj, CharacterRelation relation, AiTargetType type, float powDist, EntityInfo obj, ref float minPowDist, ref EntityInfo nearstTarget)
        {
            EntityInfo target = GetSeeingLivingCharacterInfoHelper(srcObj, obj.GetId());
            if (null != target && !target.IsDead()) {
                if (!target.IsTargetNpc()) {
                    return;
                }
                if (type == AiTargetType.HERO && target.EntityType != (int)EntityTypeEnum.Hero) {
                    return;
                }
                if (type == AiTargetType.TOWER && target.EntityType != (int)EntityTypeEnum.Tower) {
                    return;
                }
                if (type == AiTargetType.BOSS && target.EntityType != (int)EntityTypeEnum.Boss) {
                    return;
                }
                if (type == AiTargetType.NPC && target.EntityType != (int)EntityTypeEnum.Normal) {
                    return;
                }
                if (relation == EntityInfo.GetRelation(srcObj, target)) {
                    if (powDist < minPowDist) {
                        if (powDist > c_MaxViewRangeSqr || CanSee(srcObj, target)) {
                            nearstTarget = target;
                            minPowDist = powDist;
                        }
                    }
                }
            }
        }

        private static bool CanSee(EntityInfo src, EntityInfo target)
        {
            int srcCampId = src.GetCampId();
            int targetCampId = target.GetCampId();
            if (srcCampId == targetCampId)
                return true;
            else if (srcCampId == (int)CampIdEnum.Hostile || targetCampId == (int)CampIdEnum.Hostile) {
                return EntityInfo.CanSee(src, target);
            } else {
                return true;
            }
        }

        internal static void DoSkillCommandState(EntityInfo entity, long deltaTime, AbstractAiStateLogic logic, int skillId)
        {
            if (entity.GetMovementStateInfo().IsMoving) {
                logic.NotifyAiStopPursue(entity);
            }
            if (skillId > 0) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    if (aiInfo.Target <= 0) {
                        EntityInfo info;
                        if (skillInfo.ConfigData.targetType == (int)SkillTargetType.Enemy || skillInfo.ConfigData.targetType == (int)SkillTargetType.RandEnemy) {
                            info = GetNearstTargetHelper(entity, CharacterRelation.RELATION_ENEMY);
                        } else {
                            info = GetNearstTargetHelper(entity, CharacterRelation.RELATION_FRIEND);
                        }
                        if (null != info) {
                            aiInfo.Target = info.GetId();
                        }
                    }
                    if (aiInfo.Target > 0) {
                        logic.NotifyAiSkill(entity, skillId);
                    }
                }
            } else if(!entity.GetSkillStateInfo().IsSkillActivated()) {
                logic.AiSendStoryMessage(entity, "npc_skill_finish:" + entity.GetUnitId(), entity.GetId());
                logic.AiSendStoryMessage(entity, "obj_skill_finish", entity.GetId());
                logic.ChangeToState(entity, (int)AiStateId.Idle);
            }
        }
        internal static void DoMoveCommandState(EntityInfo entity, long deltaTime, AbstractAiStateLogic logic)
        {
            //执行状态处理
            AiData_ForMoveCommand data = GetAiDataForMoveCommand(entity);
            if (null == data) return;

            if (!data.IsFinish) {
                if (WayPointArrived(entity, data)) {
                    Vector3 targetPos = new Vector3();
                    MoveToNext(entity, data, ref targetPos);
                    if (!data.IsFinish) {
                        logic.NotifyAiPursue(entity, targetPos);
                    }
                } else {
                    AiStateInfo info = entity.GetAiStateInfo();
                    info.Time += deltaTime;
                    if (info.Time > 500) {
                        info.Time = 0;
                        Vector3 targetPos = data.WayPoints[data.Index];
                        logic.NotifyAiPursue(entity, targetPos);
                    }
                }
            }

            //判断是否状态结束并执行相应处理
            if (data.IsFinish) {
                logic.AiSendStoryMessage(entity, "npc_arrived:" + entity.GetUnitId(), entity.GetId());
                logic.AiSendStoryMessage(entity, "obj_arrived", entity.GetId());
                logic.NotifyAiStopPursue(entity);
                logic.ChangeToState(entity, (int)AiStateId.Idle);
            }
        }
        private static AiData_ForMoveCommand GetAiDataForMoveCommand(EntityInfo entity)
        {
            AiData_ForMoveCommand data = entity.GetAiStateInfo().AiDatas.GetData<AiData_ForMoveCommand>();
            return data;
        }
        private static void MoveToNext(EntityInfo charObj, AiData_ForMoveCommand data, ref Vector3 targetPos)
        {
            if (++data.Index >= data.WayPoints.Count) {
                data.IsFinish = true;
                return;
            }

            var move_info = charObj.GetMovementStateInfo();
            targetPos = data.WayPoints[data.Index];
            move_info.TargetPosition = targetPos;
        }
        private static bool WayPointArrived(EntityInfo charObj, AiData_ForMoveCommand data)
        {
            bool ret = false;
            var move_info = charObj.GetMovementStateInfo();            
            float powDistDest = move_info.CalcDistancSquareToTarget();
            if (powDistDest <= 1f) {
                ret = true;
            }
            return ret;
        }
        internal static void DoPursuitCommandState(EntityInfo entity, long deltaTime, AbstractAiStateLogic logic)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > 200) {
                EntityInfo target = AiLogicUtility.GetLivingCharacterInfoHelper(entity, info.Target);
                if (null != target) {
                    float minDist = entity.GetRadius() + target.GetRadius();
                    float dist = (float)entity.GetActualProperty().AttackRange + minDist;
                    float distGoHome = entity.GohomeRange;
                    Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                    ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                    float dir = Geometry.GetYRadian(new Vector2(targetPos.X, targetPos.Z), new Vector2(srcPos.X, srcPos.Z));
                    targetPos.X += (float)(minDist * Math.Sin(dir));
                    targetPos.Z += (float)(minDist * Math.Cos(dir));
                    float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                    if (powDist < dist * dist) {
                        logic.AiSendStoryMessage(entity, "npc_pursuit_finish:" + entity.GetUnitId(), entity.GetId());
                        logic.AiSendStoryMessage(entity, "obj_pursuit_finish", entity.GetId());
                        logic.NotifyAiStopPursue(entity);
                        logic.ChangeToState(entity, (int)AiStateId.Idle);
                    } else {
                        logic.NotifyAiPursue(entity, targetPos);
                    }
                } else {
                    logic.AiSendStoryMessage(entity, "npc_pursuit_exit:" + entity.GetUnitId(), entity.GetId());
                    logic.AiSendStoryMessage(entity, "obj_pursuit_exit", entity.GetId());
                    logic.NotifyAiStopPursue(entity);
                    logic.ChangeToState(entity, (int)AiStateId.Idle);
                }
            }
        }
        internal static void DoPatrolCommandState(EntityInfo entity, long deltaTime, AbstractAiStateLogic logic)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > 100) {
                info.Time = 0;
                EntityInfo target = null;
                if (info.IsExternalTarget) {
                    target = AiLogicUtility.GetSeeingLivingCharacterInfoHelper(entity, info.Target);
                    if (null == target) {
                        target = AiLogicUtility.GetNearstTargetHelper(entity, CharacterRelation.RELATION_ENEMY);
                        if (null != target)
                            info.Target = target.GetId();
                    }
                } else {
                    target = AiLogicUtility.GetNearstTargetHelper(entity, CharacterRelation.RELATION_ENEMY);
                    if (null != target)
                        info.Target = target.GetId();
                }
                if (null != target) {
                    logic.AiSendStoryMessage(entity, "obj_patrol_exit", entity.GetId());
                    logic.AiSendStoryMessage(entity, string.Format("npc_patrol_exit:{0}", entity.GetUnitId()), entity.GetId());
                    logic.ChangeToState(entity, (int)AiStateId.Idle);
                } else {
                    AiData_ForPatrolCommand data = GetAiDataForPatrolCommand(entity);
                    if (null != data) {
                        ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                        if (data.PatrolPath.HavePathPoint && !data.PatrolPath.IsReached(srcPos)) {
                            logic.NotifyAiPursue(entity, data.PatrolPath.CurPathPoint);
                        } else {
                            data.PatrolPath.UseNextPathPoint();
                            if (data.PatrolPath.HavePathPoint) {
                                logic.NotifyAiPursue(entity, data.PatrolPath.CurPathPoint);
                            } else {
                                if (data.IsLoopPatrol) {
                                    logic.AiSendStoryMessage(entity, "obj_patrol_restart", entity.GetId());
                                    logic.AiSendStoryMessage(entity, string.Format("npc_patrol_restart:{0}", entity.GetUnitId()), entity.GetId());
                                    data.PatrolPath.Restart();
                                } else {
                                    logic.AiSendStoryMessage(entity, "obj_patrol_finish", entity.GetId());
                                    logic.AiSendStoryMessage(entity, string.Format("npc_patrol_finish:{0}", entity.GetUnitId()), entity.GetId());
                                    logic.NotifyAiStopPursue(entity);
                                    logic.ChangeToState(entity, (int)AiStateId.Idle);
                                }
                            }
                        }
                        info.HomePos = entity.GetMovementStateInfo().GetPosition3D();
                    } else {
                        logic.NotifyAiStopPursue(entity);
                        logic.ChangeToState(entity, (int)AiStateId.Idle);
                    }
                }
            }
        }
        private static AiData_ForPatrolCommand GetAiDataForPatrolCommand(EntityInfo entity)
        {
            AiData_ForPatrolCommand data = entity.GetAiStateInfo().AiDatas.GetData<AiData_ForPatrolCommand>();
            return data;
        }
        internal static SkillInfo NpcFindCanUseSkill(EntityInfo npc, AiData_General aidata, bool includeManualSkill)
        {
            SkillInfo selectSkill = null;
            SkillStateInfo skStateInfo = npc.GetSkillStateInfo();
            int priority = -1;
            SkillInfo skInfo = null;
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (includeManualSkill && npc.ManualSkillId > 0) {
                skInfo = skStateInfo.GetSkillInfoById(npc.ManualSkillId);
                if (null != skInfo && !skInfo.IsInCd(curTime)) {
                    selectSkill = skInfo;
                }
            }
            if (null == selectSkill) {
                if (npc.AutoSkillIds.Count <= 0)
                    return null;
                int randIndex = Helper.Random.Next(0, npc.AutoSkillIds.Count);
                skInfo = skStateInfo.GetSkillInfoById(npc.AutoSkillIds[randIndex]);
                if (null != skInfo && !skInfo.IsInCd(curTime)) {
                    selectSkill = skInfo;
                } else {
                    for (int i = 0; i < npc.AutoSkillIds.Count; i++) {
                        skInfo = skStateInfo.GetSkillInfoById(npc.AutoSkillIds[i]);
                        if (null != skInfo && !skInfo.IsInCd(curTime) && skInfo.InterruptPriority > priority) {
                            selectSkill = skInfo;
                            priority = skInfo.InterruptPriority;
                        }
                    }
                }
            }
            if (null != selectSkill) {
                aidata.LastUseSkillTime = TimeUtility.GetLocalMilliseconds();
            }
            return selectSkill;
        }
    }
}