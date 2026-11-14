using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
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

        // Effect runtime state: logic id selects behaviour, buffDataId selects attribute deltas.
        public int ImpactLogicId = 0;
        public int ImpactType = (int)ScriptableFramework.ImpactType.BUFF;
        public int BuffDataId = 0;
        public int StateId = 0;
        public bool IsActivated = false;
        public bool HasEffectApplyed = false;
        public bool IsGfxControl = false;
        public float Factor = 1.0f;
        public int ImpactWrapCnt = 1;
        public float MaxMoveDistance = 0.0f;
        public int LeftEnableMoveCount = 0;
        public List<string> ExtraParams = new List<string>();
        public List<string> EffectList = new List<string>();
        public ImpactLogicDatas LogicDatas = new ImpactLogicDatas();

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
            if (BuffDataId > 0) BuffRefixProperty.RefixCharacterProperty(entity, BuffDataId, Factor);
        }
    }
}
