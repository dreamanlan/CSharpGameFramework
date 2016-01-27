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
        /// <summary>
        /// 普通攻击
        /// </summary>
        public int NormalSkillId
        {
            get { return m_NormalSkillId; }
        }
        /// <summary>
        /// 手动技能
        /// </summary>
        public int ManualSkillId
        {
            get { return m_ManualSkillId; }
            set { m_ManualSkillId = value; }
        }
        /// <summary>
        /// 自动技能
        /// </summary>
        public List<int> AutoSkillIds
        {
            get { return m_AutoSkillIds; }
        }
        /// <summary>
        /// 被动技能
        /// </summary>
        public List<int> PassiveSkillIds
        {
            get { return m_PassiveSkillIds; }
        }
        public int DropMoney
        {
            get { return m_DropMoney; }
            set { m_DropMoney = value; }
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
            
            m_CreatorId = 0;
            m_BornSkillId = 0;
            m_DeadSkillId = 0;
            m_NormalSkillId = 0;
            m_AutoSkillIds.Clear();
            m_PassiveSkillIds.Clear();

            ResetBaseInfo();
            GetAiStateInfo().Reset();
        }

        public void LoadData(int unitId, int camp, TableConfig.Actor cfg, int ai, params string[] aiParams)
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
            
            SetLinkId(cfg.id);
            SetModel(cfg.avatar);
            EntityType = cfg.type;
            m_Radius = cfg.size;

            float maxAttackRange = 0;
            if (cfg.skill0 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill0);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.distance)
                    maxAttackRange = info.ConfigData.distance;

                m_NormalSkillId = cfg.skill0;
                m_AutoSkillIds.Add(cfg.skill0);
            }
            if (cfg.skill1 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill1);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.distance)
                    maxAttackRange = info.ConfigData.distance;

                m_AutoSkillIds.Add(cfg.skill1);
            }
            if (cfg.skill2 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill2);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.distance)
                    maxAttackRange = info.ConfigData.distance;

                m_AutoSkillIds.Add(cfg.skill2);
            }
            if (cfg.skill3 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill3);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.distance)
                    maxAttackRange = info.ConfigData.distance;

                m_AutoSkillIds.Add(cfg.skill3);
            }
            if (cfg.skill4 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill4);
                GetSkillStateInfo().AddSkill(info);

                if (maxAttackRange < info.ConfigData.distance)
                    maxAttackRange = info.ConfigData.distance;

                m_ManualSkillId = cfg.skill4;
                //m_AutoSkillIds.Add(cfg.skill4);
            }
            if (cfg.passiveskill1 > 0) {
                SkillInfo info = new SkillInfo(cfg.passiveskill1);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.passiveskill1);
            }
            if (cfg.passiveskill2 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill2);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill2);
            }
            if (cfg.passiveskill3 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill3);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill3);
            }
            if (cfg.passiveskill4 > 0) {
                SkillInfo info = new SkillInfo(cfg.skill4);
                GetSkillStateInfo().AddSkill(info);

                m_PassiveSkillIds.Add(cfg.skill4);
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

            ViewRange = cfg.viewrange;
            GohomeRange = cfg.gohomerange;

            GetBaseProperty().SetRps(Operate_Type.OT_Absolute, 1);
            GetBaseProperty().SetAttackBase(Operate_Type.OT_Absolute, cfg.baseattack);
            GetBaseProperty().SetAttackRange(Operate_Type.OT_Absolute, maxAttackRange);
            GetBaseProperty().SetHpMax(Operate_Type.OT_Absolute, cfg.hp);
            GetBaseProperty().SetEnergyMax(Operate_Type.OT_Absolute, cfg.mp);
            GetBaseProperty().SetMoveSpeed(Operate_Type.OT_Absolute, cfg.speed);

            AttrCalculator.Calc(this);
            SetHp(Operate_Type.OT_Absolute, GetActualProperty().HpMax);
            SetEnergy(Operate_Type.OT_Absolute, GetActualProperty().EnergyMax);
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
        public TableConfig.Actor ConfigData
        {
            get { return m_ConfigData; }
        }

        private int m_EntityType = 0;
        private float m_Scale = 1.0f;

        private bool m_CanMove = true;
        private bool m_CanRotate = true;
        private bool m_CanHitMove = true;

        private bool m_IsBorning = false;
        private long m_BornTime = 0;
        private long m_BornTimeout = 10000;
        private bool m_NeedDelete = false;

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
    }
}
