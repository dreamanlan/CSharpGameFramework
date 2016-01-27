using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class AiLogicManager
    {
        public IAiStateLogic GetNpcStateLogic(int id)
        {
            IAiStateLogic logic;
            m_NpcStateLogics.TryGetValue(id, out logic);
            return logic;
        }
        private AiLogicManager()
        {
            //这里初始化所有的Ai状态逻辑
            m_NpcStateLogics.Add((int)AiStateLogicId.Entity_DslLogic, new AiLogic_Npc_DslLogic());
            m_NpcStateLogics.Add((int)AiStateLogicId.Entity_General, new AiLogic_Npc_General());
            m_NpcStateLogics.Add((int)AiStateLogicId.Entity_Leader, new AiLogic_Npc_Leader());
            m_NpcStateLogics.Add((int)AiStateLogicId.Entity_Member, new AiLogic_Npc_Member());
        }
        private Dictionary<int, IAiStateLogic> m_NpcStateLogics = new Dictionary<int, IAiStateLogic>();

        public static AiLogicManager Instance
        {
            get { return s_AiLogicManager; }
        }
        private static AiLogicManager s_AiLogicManager = new AiLogicManager();
    }
}
