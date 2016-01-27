using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
    class AiLogic_Npc_Leader : AbstractAiStateLogic
    {
        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Pursuit, this.PursuitHandler);
            SetStateHandler((int)AiStateId.Combat, this.CombatHandler);
            SetStateHandler((int)AiStateId.SkillCommand, this.SkillCommandHandler);
            SetStateHandler((int)AiStateId.MoveCommand, this.MoveCommandHandler);
            SetStateHandler((int)AiStateId.PursuitCommand, this.PursuitCommandHandler);
            SetStateHandler((int)AiStateId.PatrolCommand, this.PatrolCommandHandler);
            SetStateHandler((int)AiStateId.WaitCommand, this.WaitCommandHandler);
        }

        protected override void OnStateLogicInit(EntityInfo entity, long deltaTime)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time = 0;
            info.HomePos = entity.GetMovementStateInfo().GetPosition3D();
            info.Target = 0;
        }

        private void IdleHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead())
                return;
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
                    entity.GetMovementStateInfo().IsMoving = false;
                    NotifyAiMove(entity);
                    info.Time = 0;
                    ChangeToState(entity, (int)AiStateId.Pursuit);
                }
            }
        }
        private void PursuitHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > 200) {
                EntityInfo target = AiLogicUtility.GetLivingCharacterInfoHelper(entity, info.Target);
                if (null != target) {
                    float minDist = entity.GetRadius() + target.GetRadius();
                    float dist = (float)entity.GetActualProperty().AttackRange + minDist;
                    Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                    ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                    float dir = Geometry.GetYAngle(new Vector2(targetPos.X, targetPos.Z), new Vector2(srcPos.X, srcPos.Z));
                    targetPos.X += (float)(minDist * Math.Sin(dir));
                    targetPos.Z += (float)(minDist * Math.Cos(dir));
                    float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                    if (powDist < dist * dist) {
                        entity.GetMovementStateInfo().IsMoving = false;
                        NotifyAiMove(entity);
                        ChangeToState(entity, (int)AiStateId.Combat);
                    } else {
                        entity.GetMovementStateInfo().IsMoving = true;
                        entity.GetMovementStateInfo().TargetPosition = targetPos;
                        NotifyAiMove(entity);
                    }
                } else {
                    entity.GetMovementStateInfo().IsMoving = false;
                    NotifyAiMove(entity);
                    ChangeToState(entity, (int)AiStateId.Idle);
                }
            }
        }
        private void CombatHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > c_IntervalTime) {
                AiData_General data = GetAiData(entity);
                if (null != data) {
                    info.Time = 0;
                    EntityInfo target = AiLogicUtility.GetLivingCharacterInfoHelper(entity, info.Target);
                    if (null != target) {
                        float minDist = entity.GetRadius() + target.GetRadius();
                        float dist = (float)entity.GetActualProperty().AttackRange + minDist;
                        ScriptRuntime.Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                        ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                        float dir = Geometry.GetYAngle(new Vector2(targetPos.X, targetPos.Z), new Vector2(srcPos.X, srcPos.Z));
                        targetPos.X += (float)(minDist * Math.Sin(dir));
                        targetPos.Z += (float)(minDist * Math.Cos(dir));
                        float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                        if (powDist < dist * dist) {
                            if (!entity.GetSkillStateInfo().IsSkillActivated()) {
                                float rps = entity.GetActualProperty().Rps;
                                long curTime = TimeUtility.GetLocalMilliseconds();
                                NotifyAiFace(entity);
                                int skillId = 0;
                                if (data.ManualSkillId > 0) {
                                    skillId = data.ManualSkillId;
                                } else {
                                    if (entity.SceneContext.BlackBoard.IsAutoOperate && entity.ManualSkillId > 0 && Helper.Random.Next() <= 20) {
                                        skillId = entity.ManualSkillId;
                                    } else if (entity.AutoSkillIds.Count > 0) {
                                        int index = Helper.Random.Next(entity.AutoSkillIds.Count);
                                        skillId = entity.AutoSkillIds[index];
                                    }
                                }
                                if (skillId > 0) {
                                    SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                                    if (null != skillInfo && !skillInfo.IsInCd(curTime)) {
                                        data.LastUseSkillTime = curTime;
                                        NotifyAiSkill(entity, skillId);
                                    }
                                }
                            }
                        } else {
                            NotifyAiStopSkill(entity);
                            ChangeToState(entity, (int)AiStateId.Pursuit);
                        }
                    } else {
                        NotifyAiStopSkill(entity);
                        ChangeToState(entity, (int)AiStateId.Pursuit);
                    }
                } else {
                    info.Time = 0;
                }
            }
        }
        private void SkillCommandHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiData_General data = GetAiData(entity);
            if (null != data) {
                AiLogicUtility.DoSkillCommandState(entity, deltaTime, this, data.ManualSkillId);
                if (data.ManualSkillId > 0)
                    data.ManualSkillId = 0;
            } else {
                ChangeToState(entity, (int)AiStateId.Idle);
            }
        }
        private void MoveCommandHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiLogicUtility.DoMoveCommandState(entity, deltaTime, this);
        }
        private void PursuitCommandHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiLogicUtility.DoPursuitCommandState(entity, deltaTime, this);
        }
        private void PatrolCommandHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
            AiLogicUtility.DoPatrolCommandState(entity, deltaTime, this);
        }
        private void WaitCommandHandler(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                entity.GetMovementStateInfo().IsMoving = false;
                NotifyAiMove(entity);
                ChangeToState(entity, (int)AiStateId.Idle);
                return;
            }
        }
        private AiData_General GetAiData(EntityInfo entity)
        {
            AiData_General data = entity.GetAiStateInfo().AiDatas.GetData<AiData_General>();
            if (null == data) {
                data = new AiData_General();
                entity.GetAiStateInfo().AiDatas.AddData(data);
            }
            return data;
        }

        private const long c_IntervalTime = 200;
    }
}


