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
                entity.GetMovementStateInfo().IsMoving = false;
                logic.NotifyAiMove(entity);
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
                logic.AiSendStoryMessage(entity, "npcskillfinish:" + entity.GetUnitId(), entity.GetId());
                logic.AiSendStoryMessage(entity, "objskillfinish", entity.GetId());
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
                        entity.GetMovementStateInfo().IsMoving = true;
                        logic.NotifyAiMove(entity);
                    }
                } else {
                    AiStateInfo info = entity.GetAiStateInfo();
                    info.Time += deltaTime;
                    if (info.Time > 500) {
                        info.Time = 0;
                        Vector3 targetPos = data.WayPoints[data.Index];
                        entity.GetMovementStateInfo().TargetPosition = targetPos;
                        entity.GetMovementStateInfo().IsMoving = true;
                        logic.NotifyAiMove(entity);
                    }
                }
            }

            //判断是否状态结束并执行相应处理
            if (data.IsFinish) {
                logic.AiSendStoryMessage(entity, "npcarrived:" + entity.GetUnitId(), entity.GetId());
                logic.AiSendStoryMessage(entity, "objarrived", entity.GetId());
                entity.GetMovementStateInfo().IsMoving = false;
                logic.NotifyAiMove(entity);
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
                    float dir = Geometry.GetYAngle(new Vector2(targetPos.X, targetPos.Z), new Vector2(srcPos.X, srcPos.Z));
                    targetPos.X += (float)(minDist * Math.Sin(dir));
                    targetPos.Z += (float)(minDist * Math.Cos(dir));
                    float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                    if (powDist < dist * dist) {
                        logic.AiSendStoryMessage(entity, "npcpursuitfinish:" + entity.GetUnitId(), entity.GetId());
                        logic.AiSendStoryMessage(entity, "objpursuitfinish", entity.GetId());
                        entity.GetMovementStateInfo().IsMoving = false;
                        logic.NotifyAiMove(entity);
                        logic.ChangeToState(entity, (int)AiStateId.Idle);
                    } else {
                        entity.GetMovementStateInfo().IsMoving = true;
                        entity.GetMovementStateInfo().TargetPosition = targetPos;
                        logic.NotifyAiMove(entity);
                    }
                } else {
                    logic.AiSendStoryMessage(entity, "npcpursuitexit:" + entity.GetUnitId(), entity.GetId());
                    logic.AiSendStoryMessage(entity, "objpursuitexit", entity.GetId());
                    entity.GetMovementStateInfo().IsMoving = false;
                    logic.NotifyAiMove(entity);
                    logic.ChangeToState(entity, (int)AiStateId.Idle);
                }
            }
        }
        internal static void DoPatrolCommandState(EntityInfo entity, long deltaTime, AbstractAiStateLogic logic)
        {
            if (entity.IsDead()) {
                logic.AiSendStoryMessage(entity, "objpatrolexit", entity.GetId());
                logic.AiSendStoryMessage(entity, string.Format("npcpatrolexit:{0}", entity.GetUnitId()), entity.GetId());
                logic.ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
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
                    logic.AiSendStoryMessage(entity, "objpatrolexit", entity.GetId());
                    logic.AiSendStoryMessage(entity, string.Format("npcpatrolexit:{0}", entity.GetUnitId()), entity.GetId());
                    logic.ChangeToState(entity, (int)AiStateId.Pursuit);
                } else {
                    AiData_ForPatrolCommand data = GetAiDataForPatrolCommand(entity);
                    if (null != data) {
                        ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                        if (data.PatrolPath.HavePathPoint && !data.PatrolPath.IsReached(srcPos)) {
                            entity.GetMovementStateInfo().TargetPosition = data.PatrolPath.CurPathPoint;
                            entity.GetMovementStateInfo().IsMoving = true;
                            logic.NotifyAiMove(entity);
                        } else {
                            data.PatrolPath.UseNextPathPoint();
                            if (data.PatrolPath.HavePathPoint) {
                                entity.GetMovementStateInfo().TargetPosition = data.PatrolPath.CurPathPoint;
                                entity.GetMovementStateInfo().IsMoving = true;
                                logic.NotifyAiMove(entity);
                            } else {
                                if (data.IsLoopPatrol) {
                                    logic.AiSendStoryMessage(entity, "objpatrolrestart", entity.GetId());
                                    logic.AiSendStoryMessage(entity, string.Format("npcpatrolrestart:{0}", entity.GetUnitId()), entity.GetId());
                                    data.PatrolPath.Restart();
                                } else {
                                    logic.AiSendStoryMessage(entity, "objpatrolfinish", entity.GetId());
                                    logic.AiSendStoryMessage(entity, string.Format("npcpatrolfinish:{0}", entity.GetUnitId()), entity.GetId());
                                    entity.GetMovementStateInfo().IsMoving = false;
                                    logic.NotifyAiMove(entity);
                                    logic.ChangeToState(entity, (int)AiStateId.Idle);
                                }
                            }
                        }
                        info.HomePos = entity.GetMovementStateInfo().GetPosition3D();
                    } else {
                        entity.GetMovementStateInfo().IsMoving = false;
                        logic.NotifyAiMove(entity);
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
        
        /// <summary>
        /// 获得队长 npcInfo
        /// </summary>
        /// <returns></returns>
        internal static EntityInfo GetLeaderInfo()
        {
            return null;
        }

    }
}