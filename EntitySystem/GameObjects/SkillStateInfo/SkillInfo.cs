using System;
using System.Collections.Generic;

namespace GameFramework
{
    public enum SkillOrImpactType
    {
        Skill = 0,
        Impact,
        Buff,
    }
    public enum SkillTargetType
    {
        Self = 0,
        Enemy,
        Friend,
        RandEnemy,
        RandFriend,
    }
    public enum SkillAoeType
    {
        Unknown = 0,
        Circle,
        Sector,
        Capsule,
        Rectangle,
    }
    public class SkillInfo
    {
        public int SkillId;                // 技能Id
        public int SkillLevel;             // 技能等级
        public bool IsSkillActivated;      // 是否正在释放技能
        public long CdEndTime;
        
        //校验数据
        public int m_SkillCDRefreshCount = 0;

        public TableConfig.Skill ConfigData;

        public SkillInfo(int skillId)
        {
            SkillId = skillId;
            ConfigData = TableConfig.SkillProvider.Instance.GetSkill(skillId);
        }
        public SkillInfo(TableConfig.Skill cfg)
        {
            if (null != cfg) {
                SkillId = cfg.id;
            }
            ConfigData = cfg;
        }

        /// <summary>
        /// add by laogao
        /// 获得攻击对象类型
        /// </summary>
        /// <returns></returns>
        public SkillTargetType GetTargetType() {
            return (SkillTargetType)ConfigData.targetType;
        }

        /// <summary>
        /// add by laogao
        /// 攻击距离
        /// </summary>
        public float distance
        {
            get
            {
                return ConfigData.distance;
            }
        }

        /// <summary>
        /// add by laogao
        /// 技能优先级
        /// </summary>
        /// <returns></returns>
        public int GetPriority(){
            return 0;
        }

        public void Reset()
        {
            IsSkillActivated = false;
        }

        public void AddCD(long time)
        {
            CdEndTime += time;
        }

        public float GetCD(long now)
        {
            return CdEndTime - now;
        }

        public bool IsInCd(long now)
        {
            if (now < CdEndTime) {
                return true;
            } else {
                return false;
            }
        }

        public void Refresh()
        {
            CdEndTime = 0;
            m_SkillCDRefreshCount++;
        }
    }
}
