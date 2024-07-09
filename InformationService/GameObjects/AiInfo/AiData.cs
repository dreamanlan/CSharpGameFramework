using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptRuntime;

namespace ScriptableFramework
{
    public class AiData_ForMoveCommand
    {
        public List<Vector3> WayPoints { get; set; }
        public int Index { get; set; }
        public bool IsFinish { get; set; }
        public string Event { get; set; }

        public AiData_ForMoveCommand(List<Vector3> way_points)
        {
            WayPoints = way_points;
            Index = 0;
            IsFinish = false;
            Event = string.Empty;
        }
    }
}
