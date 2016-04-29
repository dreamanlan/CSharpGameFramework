using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLibrary
{
    class AiLogic_Npc_User : AbstractAiStateLogic
    {
        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.MoveCommand, this.MoveCommandHandler);
            SetStateHandler((int)AiStateId.PursuitCommand, this.PursuitCommandHandler);
            SetStateHandler((int)AiStateId.PatrolCommand, this.PatrolCommandHandler);
            SetStateHandler((int)AiStateId.WaitCommand, this.WaitCommandHandler);
        }

        protected override void OnStateLogicInit(EntityInfo npc, long deltaTime)
        {
            AiStateInfo info = npc.GetAiStateInfo();
            info.Time = 0;
            info.HomePos = npc.GetMovementStateInfo().GetPosition3D();
            info.Target = 0;
            NotifyAiInitDslLogic(npc);
        }

        protected override bool OnStateLogicCheck(EntityInfo npc, long deltaTime)
        {
            if (npc.IsDead()) {
                if (npc.GetAiStateInfo().CurState != (int)AiStateId.Idle) {
                    NotifyAiStopPursue(npc);
                    ChangeToState(npc, (int)AiStateId.Idle);
                }
                return false;
            }
            return true;
        }

        private void IdleHandler(EntityInfo npc, long deltaTime)
        {
            AiStateInfo info = npc.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > 100) {
                info.Time = 0;
            }
        }

        private void MoveCommandHandler(EntityInfo npc, long deltaTime)
        {
            AiLogicUtility.DoMoveCommandState(npc, deltaTime, this);
        }
        private void PursuitCommandHandler(EntityInfo npc, long deltaTime)
        {
            AiLogicUtility.DoPursuitCommandState(npc, deltaTime, this);
        }
        private void PatrolCommandHandler(EntityInfo npc, long deltaTime)
        {
            AiLogicUtility.DoPatrolCommandState(npc, deltaTime, this);
        }
        private void WaitCommandHandler(EntityInfo npc, long deltaTime)
        {
        }
    }
}
