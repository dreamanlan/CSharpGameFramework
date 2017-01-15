using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public enum EntityTypeEnum
    {
        Normal = 0,
        Tower,
        Hero,
        Boss,
        Skill,
    }

    public enum DropEntityTypeEnum
    {
        GOLD = 110001,
        HP = 110002,
        MP = 110003,
        MUTI_GOLD = 110004,
        ITEM = 110005,
    }

    public partial class EntityInfo
    {
        public object CustomData
        {
            get { return m_CustomData; }
            set { m_CustomData = value; }
        }
        public int EntityType
        {
            get { return m_EntityType; }
            set { m_EntityType = value; }
        }
        public float Scale
        {
            get { return m_Scale; }
        }
        public bool CanMove
        {
            get { return m_CanMove; }
        }
        public bool CanHitMove
        {
            get { return m_CanHitMove; }
        }
        public bool CanRotate
        {
            get { return m_CanRotate; }
        }
        public bool CanDead
        {
            get { return m_CanDead; }
            set { m_CanDead = value; }
        }
        public bool IsPassive
        {
            get { return m_IsPassive; }
            set { m_IsPassive = value; }
        }
        public bool IsServerEntity
        {
            get { return m_IsServerEntity; }
            set { m_IsServerEntity = value; }
        }
        public int CreatorId
        {
            get { return m_CreatorId; }
            set { m_CreatorId = value; }
        }
        public bool IsBorning
        {
            get { return m_IsBorning; }
            set { m_IsBorning = value; }
        }
        public long BornTime
        {
            get { return m_BornTime; }
            set { m_BornTime = value; }
        }
        public long BornTimeout
        {
            get { return m_BornTimeout; }
        }
        public long ReliveTimeout
        {
            get { return m_ReliveTimeout; }
        }
        public bool NeedDelete
        {
            get { return m_NeedDelete; }
            set { m_NeedDelete = value; }
        }
        public int BornSkillId
        {
            get { return m_BornSkillId; }
        }
        public int DeadSkillId
        {
            get { return m_DeadSkillId; }
        }
        public int NormalSkillId
        {
            get { return m_NormalSkillId; }
        }
        public int ManualSkillId
        {
            get { return m_ManualSkillId; }
            set { m_ManualSkillId = value; }
        }
        public List<int> AutoSkillIds
        {
            get { return m_AutoSkillIds; }
        }
        public List<int> PassiveSkillIds
        {
            get { return m_PassiveSkillIds; }
        }
        public int DropMoney
        {
            get { return m_DropMoney; }
            set { m_DropMoney = value; }
        }
        public TableConfig.LevelMonster LevelMonsterData
        {
            get { return m_LevelMonsterData; }
            set { m_LevelMonsterData = value; }
        }
        public EntityInfo(int id)
        {
            InitBase(id);
        }
        public void InitId(int id)
        {
            m_Id = id;
        }

        public void Reset()
        {
            m_IsBorning = false;
            m_NeedDelete = false;
            m_BornTime = 0;
            m_DropMoney = 0;

            m_CanMove = true;
            m_CanRotate = true;
            m_CanHitMove = true;
            m_CanDead = true;

            m_IsPassive = false;
            
            m_IsServerEntity = true;
            m_LevelMonsterData = null;
            
            m_CreatorId = 0;
            m_BornSkillId = 0;
            m_DeadSkillId = 0;
            m_NormalSkillId = 0;
            m_EntityType = 0;
            m_Scale = 1.0f;
            
            m_AutoSkillIds.Clear();
            m_PassiveSkillIds.Clear();

            ResetBaseInfo();
            GetAiStateInfo().Reset();
        }

        public void LoadData(int unitId, int camp, TableConfig.Actor cfg, string ai, params string[] aiParams)
        {
            SetUnitId(unitId);
            SetCampId(camp);
            GetAiStateInfo().AiLogic = ai;
            for (int i = 0; i < aiParams.Length && i < AiStateInfo.c_MaxAiParamNum; ++i) {
                GetAiStateInfo().AiParam[i] = aiParams[i];
            }
            LoadData(cfg);
        }

        public void LoadData(TableConfig.Actor cfg)
        {
            m_ConfigData = cfg;
            BaseProperty.ConfigData = cfg;
            BaseProperty.Owner = this;
            ActualProperty.ConfigData = cfg;
            ActualProperty.Owner = this;

            TableConfig.SkillEventProvider.Instance.skillEventTable.TryGetValue(cfg.id, out m_SkillEventConfigData);
            
            SetLinkId(cfg.id);
            SetModel(cfg.avatar);
            EntityType = cfg.type;
            m_Radius = cfg.size;
            m_ViewRange = cfg.viewrange;
            m_GohomeRange = cfg.gohomerange;
			
            float maxAttackRange = 0;
            if (cfg.skill0 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill0);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.skillData.distance)
                    maxAttackRange = info.ConfigData.skillData.distance;

                m_AutoSkillIds.Add(cfg.skill0);
            }
            if (cfg.skill1 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill1);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.skillData.distance)
                    maxAttackRange = info.ConfigData.skillData.distance;

                m_AutoSkillIds.Add(cfg.skill1);
            }
            if (cfg.skill2 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill2);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.skillData.distance)
                    maxAttackRange = info.ConfigData.skillData.distance;

                m_AutoSkillIds.Add(cfg.skill2);
            }
            if (cfg.skill3 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill3);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.skillData.distance)
                    maxAttackRange = info.ConfigData.skillData.distance;

                m_AutoSkillIds.Add(cfg.skill3);
            }
            if (cfg.skill4 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill4);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.skillData.distance)
                    maxAttackRange = info.ConfigData.skillData.distance;

                m_ManualSkillId = cfg.skill4;
            }
            if (cfg.skill5 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill5);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill5);
            }
            if (cfg.skill6 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill6);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill6);
            }
            if (cfg.skill7 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill7);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill7);
            }
            if (cfg.skill8 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill8);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill8);
            }
            if (cfg.bornskill > 0) {
                SkillInfo info = new SkillInfo(cfg.bornskill);
                GetSkillStateInfo().AddSkill(info);

                m_BornSkillId = cfg.bornskill;
            }
            if (cfg.deadskill > 0) {
                SkillInfo info = new SkillInfo(cfg.deadskill);
                GetSkillStateInfo().AddSkill(info);

                m_DeadSkillId = cfg.deadskill;
            }

            foreach (var pair in TableConfig.AttrDefineProvider.Instance.AttrDefineMgr.GetData()) {
                TableConfig.AttrDefine def = pair.Value as TableConfig.AttrDefine;
                BaseProperty.SetInt((CharacterPropertyEnum)def.id, def.value);
            }

            InitProperty(CharacterPropertyEnum.x1001_基础物理攻击, cfg.x1001);
            InitProperty(CharacterPropertyEnum.x1002_物理攻击成长, cfg.x1002);
            InitProperty(CharacterPropertyEnum.x1006_基础法术攻击, cfg.x1006);
            InitProperty(CharacterPropertyEnum.x1007_法术攻击成长, cfg.x1007);
            InitProperty(CharacterPropertyEnum.x1011_基础物理防御, cfg.x1011);
            InitProperty(CharacterPropertyEnum.x1012_物理防御成长, cfg.x1012);
            InitProperty(CharacterPropertyEnum.x1016_基础法术防御, cfg.x1016);
            InitProperty(CharacterPropertyEnum.x1017_法术防御成长, cfg.x1017);
            InitProperty(CharacterPropertyEnum.x1021_暴击等级, cfg.x1021);
            InitProperty(CharacterPropertyEnum.x1022_抗暴击等级, cfg.x1022);
            InitProperty(CharacterPropertyEnum.x1024_命中等级, cfg.x1024);
            InitProperty(CharacterPropertyEnum.x1026_闪避等级, cfg.x1026);
            InitProperty(CharacterPropertyEnum.x1028_破击等级, cfg.x1028);
            InitProperty(CharacterPropertyEnum.x1030_格挡等级, cfg.x1030);
            InitProperty(CharacterPropertyEnum.x1032_格挡强度, cfg.x1032);
            InitProperty(CharacterPropertyEnum.x1033_暴击伤害, cfg.x1033);
            InitProperty(CharacterPropertyEnum.x1034_暴击伤害减免, cfg.x1034);

            InitProperty(CharacterPropertyEnum.x2001_基础生命, cfg.x2001);
            InitProperty(CharacterPropertyEnum.x2002_生命成长, cfg.x2002);
            InitProperty(CharacterPropertyEnum.x2007_基础速度, cfg.x2007);
            InitProperty(CharacterPropertyEnum.x2008_速度成长, cfg.x2008);

            InitProperty(CharacterPropertyEnum.x4001_职业, cfg.x4001);
            InitProperty(CharacterPropertyEnum.x4002_阵营, cfg.x4002);
            InitProperty(CharacterPropertyEnum.x4003_品质, cfg.x4003);
            InitProperty(CharacterPropertyEnum.x4004_性别, cfg.x4004);

            AttrCalculator.Calc(this);
            Hp = ActualProperty.GetInt(CharacterPropertyEnum.x2005_最大生命值);
        }

        public void ReloadData(TableConfig.Actor cfg)
        {
            GetSkillStateInfo().Reset();
            LoadData(cfg);
        }

        public AiStateInfo GetAiStateInfo()
        {
            return m_AiStateInfo;
        }
        public bool IsCombatNpc()
        {
            if ((int)EntityTypeEnum.Boss == m_EntityType || (int)EntityTypeEnum.Normal == m_EntityType || (int)EntityTypeEnum.Tower == m_EntityType || (int)EntityTypeEnum.Hero == m_EntityType) {
                return true;
            }
            return false;
        }
        public bool IsTargetNpc()
        {
            if ((int)EntityTypeEnum.Boss == m_EntityType || (int)EntityTypeEnum.Normal == m_EntityType || (int)EntityTypeEnum.Tower == m_EntityType || (int)EntityTypeEnum.Hero == m_EntityType) {
                return true;
            }
            return false;
        }
        public TableConfig.SkillEvent GetSkillEvent(int skillId, int eventId)
        {
            TableConfig.SkillEvent ret = null;
            if (null != m_SkillEventConfigData) {
                Dictionary<int, TableConfig.SkillEvent> dict;
                if (m_SkillEventConfigData.TryGetValue(skillId, out dict)) {
                    dict.TryGetValue(eventId, out ret);
                }
            }
            return ret;
        }
        public TableConfig.Actor ConfigData
        {
            get { return m_ConfigData; }
        }
        public Dictionary<int, Dictionary<int, TableConfig.SkillEvent>> SkillEventConfigData
        {
            get { return m_SkillEventConfigData; }
        }

        private void InitProperty(CharacterPropertyEnum id, int val)
        {
            if (val != 0) {
                BaseProperty.SetInt(id, val);
            }
        }
                
        private object m_CustomData = null;
        private int m_EntityType = 0;
        private float m_Scale = 1.0f;

        private bool m_CanMove = true;
        private bool m_CanRotate = true;
        private bool m_CanHitMove = true;
        private bool m_CanDead = true;

        private bool m_IsPassive = false;
        private bool m_IsServerEntity = true;

        private bool m_IsBorning = false;
        private long m_BornTime = 0;
        private long m_BornTimeout = 10000;
        private long m_ReliveTimeout = 10000;
        private bool m_NeedDelete = false;

        private float m_ViewRange = 10.0f;
        private float m_GohomeRange = 20.0f;
        private int m_DropMoney = 0;

        private int m_CreatorId = 0;
        private int m_BornSkillId = 0;
        private int m_DeadSkillId = 0;
        private int m_NormalSkillId = 0;
        private int m_ManualSkillId = 0;
        private List<int> m_AutoSkillIds = new List<int>();
        private List<int> m_PassiveSkillIds = new List<int>();
        private AiStateInfo m_AiStateInfo = new AiStateInfo();

        private TableConfig.Actor m_ConfigData;
        private TableConfig.LevelMonster m_LevelMonsterData;
        private Dictionary<int, Dictionary<int, TableConfig.SkillEvent>> m_SkillEventConfigData;
        public const int c_StartUserUnitId = 100000000;
    }
}
