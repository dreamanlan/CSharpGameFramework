using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
    class AiLogic_Npc_General : AbstractAiStateLogic
    {
        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Combat, this.CombatHandler);
            SetStateHandler((int)AiStateId.GoHome, this.GoHomeHandler);
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

        protected override bool OnStateLogicCheck(EntityInfo npc, long deltaTime)
        {
            if (npc.IsDead()) {
                NotifyAiDead(npc);
                return false;
            }
            return true;
        }

        private void IdleHandler(EntityInfo npc, long deltaTime)
        {
            if (npc.GetMovementStateInfo().IsMoving)
                NotifyAiStopPursue(npc);

            ChangeToState(npc, (int)AiStateId.Combat);
        }

        private void CombatHandler(EntityInfo npc, long deltaTime)
        {
            if (npc.GetSkillStateInfo().IsSkillActivated()) {
                return;
            }

            AiStateInfo info = npc.GetAiStateInfo();
            Vector3 targetPos = info.HomePos;
            ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
            float distSqrToHome = Geometry.DistanceSquare(srcPos, info.HomePos);
            if (distSqrToHome > npc.GohomeRange) {
                NotifyAiStopPursue(npc);
                ChangeToState(npc, (int)AiStateId.GoHome);
                return;
            }

            ///
            EntityInfo attackTarget = null;
            SkillStateInfo currSkInfo = npc.GetSkillStateInfo();
            ///找到可以使用的技能
            SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc, this.GetAiData(npc), true);
            NotifyAiSelectSkill(npc, skInfo);
            if (skInfo == null) {
                //没有可以使用的技能就切换到Idle状态
                ChangeToState(npc, (int)AiStateId.Idle);
                return;
            }

            CharacterRelation relation =
                    (skInfo.TargetType == SkillTargetType.Friend ||
                    skInfo.TargetType == SkillTargetType.RandFriend) ?
                    CharacterRelation.RELATION_FRIEND :
                    CharacterRelation.RELATION_ENEMY;

            attackTarget = AiLogicUtility.GetNearstTargetHelper(
                npc, skInfo.Distance, relation);

            if (attackTarget != null && null != skInfo) //攻击范围内找到可攻击目标
            {
                info.Target = attackTarget.GetId();
                NotifyAiStopPursue(npc);
                NotifyAiSkill(npc, skInfo.SkillId); //攻击目标
                return;
            }
            attackTarget = AiLogicUtility.GetNearstTargetHelper(
            npc, npc.ViewRange, relation);
            if (attackTarget != null) //视野范围内找到可攻击目标
            {
                NotifyAiPursue(npc, attackTarget.GetMovementStateInfo().GetPosition3D()); // 追赶目标
                return;
            }

            currSkInfo.SetCurSkillInfo(0);
            NotifyAiStopPursue(npc);
            ChangeToState(npc, (int)AiStateId.GoHome);
        }

        private void GoHomeHandler(EntityInfo entity, long deltaTime)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > c_IntervalTime) {
                info.Time = 0;
                AiData_General data = GetAiData(entity);
                if (null != data) {
                    Vector3 targetPos = info.HomePos;
                    ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                    float powDistToHome = Geometry.DistanceSquare(srcPos, info.HomePos);
                    if (powDistToHome <= 1) {
                        NotifyAiStopPursue(entity);
                        ChangeToState(entity, (int)AiStateId.Idle);
                    } else {
                        NotifyAiPursue(entity, targetPos);
                    }
                }
            }
        }
        private void SkillCommandHandler(EntityInfo entity, long deltaTime)
        {
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
            AiLogicUtility.DoMoveCommandState(entity, deltaTime, this);
        }
        private void PursuitCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiLogicUtility.DoPursuitCommandState(entity, deltaTime, this);
        }
        private void PatrolCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiLogicUtility.DoPatrolCommandState(entity, deltaTime, this);
        }
        private void WaitCommandHandler(EntityInfo entity, long deltaTime)
        {
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


