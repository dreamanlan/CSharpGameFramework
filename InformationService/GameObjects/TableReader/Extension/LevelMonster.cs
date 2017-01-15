using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;

namespace TableConfig
{
    public sealed partial class LevelMonsterProvider
    {
        public void BuildGroupedLevelMonsters()
        {
            for (int i = 0; i < m_LevelMonsterMgr.GetDataCount();++i ) {
                LevelMonster monster = m_LevelMonsterMgr.GetData()[i];
                List<LevelMonster> list;
                int group = monster.group;
                if(m_GroupedLevelMonsters.TryGetValue(group, out list)){
                    list.Add(monster);
                } else {
                    list = new List<LevelMonster>();
                    list.Add(monster);
                    m_GroupedLevelMonsters.Add(monster.group, list);
                }
            }
        }
        public bool TryGetValue(int id, out List<LevelMonster> list)
        {
            return m_GroupedLevelMonsters.TryGetValue(id, out list);
        }

        private Dictionary<int, List<LevelMonster>> m_GroupedLevelMonsters = new Dictionary<int, List<LevelMonster>>();
    }
}
