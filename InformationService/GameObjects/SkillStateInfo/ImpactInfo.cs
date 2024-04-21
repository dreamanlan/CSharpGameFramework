using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public class ImpactInfo
    {
        public int Seq = 0;                        // Sequence number (generated when added to Character, unique on Character instance)
        public int ImpactId = -1;                  // effect ID
        public int SkillId = -1;                   // skill ID
        public int ImpactSenderId = -1;            // The ID of the effect triggerer
        public Vector3 SenderPosition;
        public int TargetType = 0;

        public long StartTime = 0;                 // The effect starts to take effect starting time
        public int DurationTime = 0;               // duration

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
