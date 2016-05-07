using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
    class AiLogic_Npc_DslLogic : AbstractAiStateLogic
    {

        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.DslLogic, this.DslLogicHandler);
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
            NotifyAiInitDslLogic(entity);
        }

        protected override bool OnStateLogicCheck(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                if (entity.GetAiStateInfo().CurState != (int)AiStateId.Idle) {
                    NotifyAiStopPursue(entity);
                    ChangeToState(entity, (int)AiStateId.Idle);
                }
                return false;
            }
            return true;
        }

        private void IdleHandler(EntityInfo entity, long deltaTime)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > 100) {
                info.Time = 0;
                ChangeToState(entity, (int)AiStateId.DslLogic);
            }
        }

        private void DslLogicHandler(EntityInfo entity, long deltaTime)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            if (null != info.AiStoryInstanceInfo) {
                long curTime = TimeUtility.GetLocalMilliseconds();
                info.AiStoryInstanceInfo.m_StoryInstance.Tick(curTime);
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
    }
}


