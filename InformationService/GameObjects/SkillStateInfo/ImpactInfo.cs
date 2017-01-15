using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public class ImpactInfo
    {
        public int Seq = 0;                        // 顺序号（添加到Character上时生成，在Character实例上唯一）
        public int ImpactId = -1;                  // 效果ID
        public int SkillId = -1;                   // 技能ID
        public int ImpactSenderId = -1;            // 效果触发者的Id
        public Vector3 SenderPosition;
        public int TargetType = 0;

        public long StartTime = 0;                 // 效果开始生效起始时间
        public int DurationTime = 0;               // 持续时间

        public int CurDamageCount = 0;

        public int ImpactToTarget = 0;        
        public TableConfig.Skill ConfigData;

        public TableConfig.SkillDamageData DamageData;
        public CharacterProperty SenderProperty;

        public ImpactInfo(int impactId)
        {
            ImpactId = impactId;
            ConfigData = TableConfig.SkillProvider.Instance.GetSkill(ImpactId);
            DamageData = new TableConfig.SkillDamageData();
            DamageData.Init();
            SenderProperty = new CharacterProperty();
        }
        public ImpactInfo(TableConfig.Skill cfg)
        {
            if (null != cfg) {
                ImpactId = cfg.id;
            }
            ConfigData = cfg;
            DamageData = new TableConfig.SkillDamageData();
            DamageData.Init();
            SenderProperty = new CharacterProperty();
        }

        public void RefixCharacterProperty(EntityInfo entity)
        {
            if (ConfigData.attrValues.Count > 0) {
                foreach (var pair in ConfigData.attrValues) {
                    entity.ActualProperty.IncreaseInt((CharacterPropertyEnum)pair.Key, pair.Value);
                }
            }
        }
    }
}
