using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableConfig
{
    public partial class SkillEvent
    {
        public List<int> args = new List<int>();
    }
    public partial class SkillEventProvider
    {
        public Dictionary<int, Dictionary<int, Dictionary<int, TableConfig.SkillEvent>>> skillEventTable;
    }
}
