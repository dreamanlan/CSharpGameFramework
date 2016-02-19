using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
    public class AiData_General
    {
        public int ManualSkillId
        {
            get { return m_ManualSkillId; }
            set { m_ManualSkillId = value; }
        }
        public long LastUseSkillTime
        {
            get { return m_LastUseSkillTime; }
            set { m_LastUseSkillTime = value; }
        }

        private int m_ManualSkillId = 0;
        private long m_LastUseSkillTime = 0;
    }
    public class AiData_Leader : AiData_General
    {
        public bool IsAutoOperate
        {
            get { return m_IsAutoOperate; }
            set { m_IsAutoOperate = value; }
        }
        public int FormationId
        {
            get { return m_FormationId; }
            set { m_FormationId = value; }
        }

        private bool m_IsAutoOperate = false;
        private int m_FormationId = 1;
    }
    public class AiData_ForMoveCommand
    {
        public List<Vector3> WayPoints { get; set; }
        public int Index { get; set; }
        public bool IsFinish { get; set; }

        public AiData_ForMoveCommand(List<Vector3> way_points)
        {
            WayPoints = way_points;
            Index = 0;
            IsFinish = false;
        }
    }
    public class AiData_ForPatrolCommand
    {
        public bool IsLoopPatrol
        {
            get { return m_IsLoopPatrol; }
            set { m_IsLoopPatrol = value; }
        }
        public AiPathData PatrolPath
        {
            get { return m_PatrolPath; }
        }

        private AiPathData m_PatrolPath = new AiPathData();
        private bool m_IsLoopPatrol = false;
    }
}
