using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableConfig
{
    public sealed partial class Skill
    {
        public string dslFile;
        public Dictionary<string, string> resources = new Dictionary<string, string>();
    }
}
