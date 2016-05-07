using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public delegate void DamageDelegation(int receiver, int caster, bool isNormalDamage, bool isCritical, int hpDamage, int npDamage);

    //！！！注意，不要在这里添加属于CharacterProperty管理的内容，只能加人物状态数据，不能加属性数据，属性由属性计算得到
    public enum PropertyIndex
    {
        IDX_HP = 0,
        IDX_MP,
        IDX_STATE,
    }

    public enum HilightType
    {
        kNone,
        kBurnType,
        kFrozenType,
        kShineType,
    }

    public enum CharacterState_Type
    {
        CST_Invincible = 1 << 0,        // 无敌
        CST_FixedPosition = 1 << 1,     // 定身，不能移动
        CST_Silence = 1 << 2,           // 沉默，不能释放技能
        CST_Sleep = 1 << 3,             // 昏迷，不能移动，不能攻击，不能放技能
        CST_Hidden = 1 << 4,            // 隐身的
    }
    public static class CharacterStateUtility
    {
        public static CharacterState_Type FromString(string name)
        {
            if (name.CompareTo("invincible") == 0)
                return CharacterState_Type.CST_Invincible;
            else if (name.CompareTo("fixedposition") == 0)
                return CharacterState_Type.CST_FixedPosition;
            else if (name.CompareTo("silence") == 0)
                return CharacterState_Type.CST_Silence;
            else if (name.CompareTo("sleep") == 0)
                return CharacterState_Type.CST_Sleep;
            else if (name.CompareTo("hidden") == 0)
                return CharacterState_Type.CST_Hidden;
            else
                return 0;
        }
        public static string ToString(CharacterState_Type type)
        {
            switch (type) {
                case CharacterState_Type.CST_Invincible:
                    return "invincible";
                case CharacterState_Type.CST_FixedPosition:
                    return "fixedposition";
                case CharacterState_Type.CST_Silence:
                    return "silence";
                case CharacterState_Type.CST_Sleep:
                    return "sleep";
                case CharacterState_Type.CST_Hidden:
                    return "hidden";
                default:
                    return string.Empty;
            }
        }
    }

    //角色监听类别标记位，用于监听某类事件触发剧情消息
    public enum StoryListenFlagEnum
    {
        Damage = 1,
    }

    public partial class EntityInfo
    {
        public struct AttackerInfo
        {
            public long m_AttackTime;
            public int m_HpDamage;
            public int m_NpDamage;
        }

        public int UniqueId
        {
            get { return m_UniqueId; }
            set { m_UniqueId = value; }
        }
        public bool IsControlByStory
        {
            get { return m_IsControlByStory; }
            set { m_IsControlByStory = value; }
        }
        public bool IsControlByManual
        {
            get { return m_IsControlByManual; }
            set { m_IsControlByManual = value; }
        }
        public int GetId()
        {
            return m_Id;
        }
        public int GetUnitId()
        {
            return m_UnitId;
        }
        public void SetUnitId(int id)
        {
            m_UnitId = id;
        }
        public int GetLinkId()
        {
            return m_LinkId;
        }
        public void SetLinkId(int id)
        {
            m_LinkId = id;
        }
        public void SetName(string name)
        {
            m_Name = name;
        }
        public string GetName()
        {
            return m_Name;
        }
        public long DeadTime
        {
            get
            {
                return m_DeadTime;
            }
            set
            {
                m_DeadTime = value;
            }
        }
        public long DeadTimeout
        {
            get { return m_DeadTimeout; }
        }
        public int GetLevel()
        {
            return m_Level;
        }
        public void SetLevel(int level)
        {
            m_Level = level;
            m_LevelChanged = true;
        }
        public int Hp
        {
            get { return m_Hp / CharacterProperty.Key; }
        }
        public int Energy
        {
            get { return m_Energy / CharacterProperty.Key; }
        }
        public int Shield
        {
            get { return m_Shield / CharacterProperty.Key; }
        }
        public float ViewRange
        {
            get { return m_ViewRange; }
            set { m_ViewRange = value; }
        }
        public float GohomeRange
        {
            get { return m_GohomeRange; }
            set { m_GohomeRange = value; }
        }
        public string GetModel()
        {
            return m_Model;
        }
        public void SetModel(string model)
        {
            m_Model = model;
        }
        public bool GetAIEnable()
        {
            return m_AIEnable;
        }
        public void SetAIEnable(bool enable)
        {
            m_AIEnable = enable;
        }
        public int GetCampId()
        {
            return m_CampId;
        }
        public void SetCampId(int val)
        {
            m_CampId = val;
        }

        public int KillerId
        {
            get { return m_KillerId; }
            set { m_KillerId = value; }
        }
        public long LastAttackedTime
        {
            get { return m_LastAttackedTime; }
            set { m_LastAttackedTime = value; }
        }
        public long LastAttackTime
        {
            get { return m_LastAttackTime; }
            set { m_LastAttackTime = value; }
        }

        public MyDictionary<int, AttackerInfo> AttackerInfos
        {
            get { return m_AttackerInfos; }
        }

        public float GetRadius()
        {
            return m_Radius;
        }

        public bool IsHaveStateFlag(CharacterState_Type type)
        {
            return (m_StateFlag & ((int)type)) != 0;
        }
        public void SetStateFlag(Operate_Type opType, CharacterState_Type mask)
        {
            if (opType == Operate_Type.OT_AddBit) {
                m_StateFlag |= (int)mask;
            } else if (opType == Operate_Type.OT_RemoveBit) {
                m_StateFlag &= ~((int)mask);
            }
            m_PropertyChanged = true;
        }
        public int StateFlag
        {
            get { return m_StateFlag; }
            set 
            { 
                m_StateFlag = value;
                m_PropertyChanged = true;
            }
        }
        public bool IsHaveStoryFlag(StoryListenFlagEnum flag)
        {
            return (m_StoryListenFlag & ((int)flag)) != 0;
        }
        public void AddStoryFlag(StoryListenFlagEnum mask)
        {
            m_StoryListenFlag |= (int)mask;
        }
        public void RemoveStoryFlag(StoryListenFlagEnum mask)
        {
            m_StoryListenFlag &= ~((int)mask);
        }
        public int StoryListenFlag
        {
            get { return m_StoryListenFlag; }
            set { m_StoryListenFlag = value; }
        }

        /**
         * @brief 基础属性值
         */
        public CharacterProperty GetBaseProperty()
        {
            return m_BaseProperty;
        }
        /**
         * @brief 当前属性值
         */
        public CharacterProperty GetActualProperty()
        {
            return m_ActualProperty;
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }
        public void SetHp(Operate_Type opType, int tVal)
        {
            int key = CharacterProperty.Key;
            m_Hp = (int)CharacterProperty.UpdateAttr(m_Hp, m_ActualProperty.HpMax * key, opType, tVal);
            m_PropertyChanged = true;
        }
        public void SetEnergy(Operate_Type opType, int tVal)
        {
            int key = CharacterProperty.Key;
            int result = (int)CharacterProperty.UpdateAttr(m_Energy, m_ActualProperty.EnergyMax * key, opType, tVal);
            if (result > m_ActualProperty.EnergyMax * key) {
                result = m_ActualProperty.EnergyMax * key;
            } else if (result < 0) {
                result = 0;
            }
            m_Energy = result;
            m_PropertyChanged = true;
        }
        public void SetShield(Operate_Type opType, int tVal)
        {
            const int c_MaxShield = 500;
            int key = CharacterProperty.Key;
            int result = (int)CharacterProperty.UpdateAttr(m_Shield, c_MaxShield * key, opType, tVal);
            if (result > c_MaxShield * key) {
                result = c_MaxShield * key;
            } else if (result < 0) {
                result = 0;
            }
            m_Shield = result;
            m_PropertyChanged = true;
        }

        public void ResetAttackerInfo()
        {
            KillerId = 0;
            AttackerInfos.Clear();
            m_LastAttackedTime = 0;
            m_LastAttackTime = 0;
            m_WaitDeleteAttackers.Clear();
            m_LastRetireTime = 0;
        }
        public void RetireAttackerInfos(long lifetime)
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastRetireTime + lifetime < curTime) {
                m_LastRetireTime = curTime;

                m_WaitDeleteAttackers.Clear();
                foreach (var pair in AttackerInfos) {
                    AttackerInfo info = pair.Value;
                    if (info.m_AttackTime + lifetime < curTime || m_SceneContext.GetEntityById(pair.Key) == null) {
                        m_WaitDeleteAttackers.Add(pair.Key);
                    }
                }
                for (int i = 0; i < m_WaitDeleteAttackers.Count; ++i) {
                    AttackerInfos.Remove(m_WaitDeleteAttackers[i]);
                }
                m_WaitDeleteAttackers.Clear();
            }
        }
        public void SetAttackerInfo(int attackId, bool isKiller, bool isNormalAttack, bool isCritical, int hpDamage, int npDamage)
        {
            if (isKiller)
                KillerId = attackId;
            long curTime = TimeUtility.GetLocalMilliseconds();
            LastAttackedTime = curTime;
            AttackerInfo info;
            if (!AttackerInfos.TryGetValue(attackId, out info)) {
                AttackerInfos.Add(attackId, new AttackerInfo { m_AttackTime = curTime, m_HpDamage = hpDamage, m_NpDamage = npDamage });
            } else {
                info.m_AttackTime = curTime;
                info.m_HpDamage += hpDamage;
                info.m_NpDamage += npDamage;
                AttackerInfos[attackId] = info;
            }
            EntityManager.FireDamageEvent(GetId(), attackId, isNormalAttack, isCritical, hpDamage, npDamage);
        }
        public void SetAttackTime()
        {
            LastAttackTime = TimeUtility.GetLocalMilliseconds();
        }
        public void CalcBaseAttr()
        {
            float aMoveSpeed = GetBaseProperty().MoveSpeed;
            int aHpMax = GetBaseProperty().HpMax;
            int aEnergyMax = GetBaseProperty().EnergyMax;
            int aAttackBase = GetBaseProperty().AttackBase;
            int aDefenceBase = GetBaseProperty().DefenceBase;
            float aCritical = GetBaseProperty().Critical;
            float aCriticalPow = GetBaseProperty().CriticalPow;
            float aRps = GetBaseProperty().Rps;
            float aAttackRange = GetBaseProperty().AttackRange;

            aHpMax += ConfigData.addhp * GetLevel();
            aEnergyMax += ConfigData.addmp * GetLevel();
            aAttackBase += ConfigData.addattack * GetLevel();
            aDefenceBase += ConfigData.adddefence * GetLevel();

            GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, aMoveSpeed);
            GetActualProperty().SetHpMax(Operate_Type.OT_Absolute, aHpMax);
            GetActualProperty().SetEnergyMax(Operate_Type.OT_Absolute, aEnergyMax);
            GetActualProperty().SetAttackBase(Operate_Type.OT_Absolute, aAttackBase);
            GetActualProperty().SetDefenceBase(Operate_Type.OT_Absolute, aDefenceBase);
            GetActualProperty().SetCritical(Operate_Type.OT_Absolute, aCritical);
            GetActualProperty().SetCriticalPow(Operate_Type.OT_Absolute, aCriticalPow);
            GetActualProperty().SetRps(Operate_Type.OT_Absolute, aRps);
            GetActualProperty().SetAttackRange(Operate_Type.OT_Absolute, aAttackRange);
        }

        public bool LevelChanged
        {
            get { return m_LevelChanged; }
            set { m_LevelChanged = value; }
        }
        public bool PropertyChanged
        {
            get { return m_PropertyChanged; }
            set { m_PropertyChanged = value; }
        }

        public MovementStateInfo GetMovementStateInfo()
        {
            return m_MovementStateInfo;
        }
        public SkillStateInfo GetSkillStateInfo()
        {
            return m_SkillStateInfo;
        }
        public CombatStatisticInfo GetCombatStatisticInfo()
        {
            return m_CombatStatisticInfo;
        }
        public SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
            set { m_SceneContext = value; }
        }
        public SceneLogicInfoManager SceneLogicInfoManager
        {
            get
            {
                SceneLogicInfoManager mgr = null;
                if (null != m_SceneContext) {
                    mgr = m_SceneContext.SceneLogicInfoManager;
                }
                return mgr;
            }
        }
        public EntityManager EntityManager
        {
            get
            {
                EntityManager mgr = null;
                if (null != m_SceneContext) {
                    mgr = m_SceneContext.EntityManager;
                }
                return mgr;
            }
        }
        public BlackBoard BlackBoard
        {
            get
            {
                BlackBoard blackBoard = null;
                if (null != m_SceneContext) {
                    blackBoard = m_SceneContext.BlackBoard;
                }
                return blackBoard;
            }
        }

        public int OwnerId
        {
            set { m_OwnerId = value; }
            get { return m_OwnerId; }
        }
        public int SummonerId
        {
            get { return m_SummonerId; }
            set { m_SummonerId = value; }
        }
        public int SummonSkillId
        {
            get { return m_SummonSkillId; }
            set { m_SummonSkillId = value; }
        }

        public bool IsUnderControl()
        {
            if (IsHaveStateFlag(CharacterState_Type.CST_FixedPosition) ||
                IsHaveStateFlag(CharacterState_Type.CST_Sleep) ||
               IsControlByStory ||
                IsControlByManual) {
                return true;
            }
            return false;
        }

        public void SetCanUseSkill(bool can_use_skill) { m_CanUseSkill = can_use_skill; }
        public bool CanUseSkill()
        {
            if (!m_CanUseSkill) {
                return false;
            }
            if (IsDead()) {
                return false;
            }
            if (IsHaveStateFlag(CharacterState_Type.CST_Sleep)) {
                return false;
            }
            if (IsHaveStateFlag(CharacterState_Type.CST_Silence)) {
                return false;
            }
            if (GetSkillStateInfo().IsSkillActivated()) {
                return false;
            }
            return true;
        }

        private void InitBase(int id)
        {
            m_Id = id;
            m_UnitId = 0;
            m_LinkId = 0;
            m_AIEnable = true;
            m_BaseProperty = new CharacterProperty();
            m_ActualProperty = new CharacterProperty();
        }
        private void ResetBaseInfo()
        {
            m_CampId = 0;
            m_UniqueId = 0;
            m_OwnerId = -1;
            m_SummonerId = -1;
            m_SummonSkillId = -1;
            SetAIEnable(true);
            DeadTime = 0;

            m_IsControlByStory = false;
            m_IsControlByManual = false;
            m_LevelChanged = false;
            m_PropertyChanged = false;

            m_Level = 0;
            m_Hp = 0;
            m_Energy = 0;
            m_StateFlag = 0;

            m_CanUseSkill = true;
            m_KillerId = 0;

            SetHp(Operate_Type.OT_Absolute, GetActualProperty().HpMax);
            SetEnergy(Operate_Type.OT_Absolute, GetActualProperty().EnergyMax);

            ResetAttackerInfo();
            GetMovementStateInfo().Reset();
            GetSkillStateInfo().Reset();
            GetCombatStatisticInfo().Reset();
        }

        private int m_Id = 0;
        private float m_Radius = 1.0f;
        private int m_UnitId = 0;
        private int m_LinkId = 0;
        private int m_OwnerId = -1;
        private int m_SummonerId = 0;
        private int m_SummonSkillId = 0;
        private string m_Name = "";
        private int m_Level = 1;
        private string m_Model = "";
        private bool m_AIEnable = true;
        private int m_UniqueId = 0;
        private bool m_IsControlByStory = false;
        private bool m_IsControlByManual = false;
        private bool m_LevelChanged = false;
        private bool m_PropertyChanged = false;
        private int m_Hp = 0;
        private int m_Energy = 0;
        private int m_Shield = 0;
        private float m_ViewRange = 0;
        private float m_GohomeRange = 5.0f;
        private int m_CampId = 0;
        private bool m_CanUseSkill = true;
        private int m_KillerId = 0;
        /************************************************************************/
        /* 助攻列表                                                             */
        /************************************************************************/
        private MyDictionary<int, AttackerInfo> m_AttackerInfos = new MyDictionary<int, AttackerInfo>();
        private long m_LastAttackedTime = 0;
        private long m_LastAttackTime = 0;
        private List<int> m_WaitDeleteAttackers = new List<int>();
        private long m_LastRetireTime = 0;

        private int m_StateFlag = 0;
        private int m_StoryListenFlag = 0;

        private long m_DeadTime = 0;
        private long m_DeadTimeout = 10000;

        private CharacterProperty m_BaseProperty;
        private CharacterProperty m_ActualProperty;
        private MovementStateInfo m_MovementStateInfo = new MovementStateInfo();
        private SkillStateInfo m_SkillStateInfo = new SkillStateInfo();
        private CombatStatisticInfo m_CombatStatisticInfo = new CombatStatisticInfo();
        private SceneContextInfo m_SceneContext = null;

        //阵营可为Friendly、Hostile、Blue、Red
        //Friendly 全部友好
        //Hostile 全部敌对(同阵营友好)
        //Blue 与Hostile与Red敌对
        //Red 与Hostile与Blue敌对
        public static CharacterRelation GetRelation(EntityInfo pObj_A, EntityInfo pObj_B)
        {
            if (pObj_A == null || pObj_B == null) {
                return CharacterRelation.RELATION_INVALID;
            }

            if (pObj_A == pObj_B) {
                return CharacterRelation.RELATION_FRIEND;
            }

            int campA = pObj_A.GetCampId();
            int campB = pObj_B.GetCampId();
            CharacterRelation relation = GetRelation(campA, campB);
            return relation;
        }
        public static CharacterRelation GetRelation(int campA, int campB)
        {
            CharacterRelation relation = CharacterRelation.RELATION_INVALID;
            if ((int)CampIdEnum.Unkown != campA && (int)CampIdEnum.Unkown != campB) {
                if (campA == campB)
                    relation = CharacterRelation.RELATION_FRIEND;
                else if (campA == (int)CampIdEnum.Friendly || campB == (int)CampIdEnum.Friendly)
                    relation = CharacterRelation.RELATION_FRIEND;
                else if (campA == (int)CampIdEnum.Hostile || campB == (int)CampIdEnum.Hostile)
                    relation = CharacterRelation.RELATION_ENEMY;
                else
                    relation = CharacterRelation.RELATION_ENEMY;
            }
            return relation;
        }
        public static bool CanSee(EntityInfo source, EntityInfo target)
        {
            bool ret = false;
            if (null != source && null != target) {
                Vector3 pos1 = source.GetMovementStateInfo().GetPosition3D();
                Vector3 pos2 = target.GetMovementStateInfo().GetPosition3D();
                float distSqr = GameFramework.Geometry.DistanceSquare(pos1, pos2);
                return CanSee(source, target, distSqr, pos1, pos2);
            }
            return ret;
        }
        public static bool CanSee(EntityInfo source, EntityInfo target, float distSqr, Vector3 pos1, Vector3 pos2)
        {
            bool ret = false;
            if (null != source && null != target) {
                //一、先判断距离
                if (distSqr < source.ViewRange * source.ViewRange) {
                    //二、再判断逻辑
                    //后面修改的同学注意下：
                    //1、我们目前的object层是数据接口层，是不需要使用多态的。概念变化的可能性比功能变化的可能性要小很多，所以我们将多态机制应用到Logic里。
                    //2、逻辑上的影响可能是对象buff或类型产生，如果判断逻辑比较复杂，采用结构化编程的风格拆分成多个函数即可。
                    //3、另一个不建议用多态理由是这个函数的调用频率会很高。
                    if (source.GetCampId() == target.GetCampId() ||
                      (!target.IsHaveStateFlag(CharacterState_Type.CST_Hidden))) {//隐身状态判断（未考虑反隐）
                        ret = true;//移动版本不计算视野，只考虑逻辑上的几个点供ai用
                    }
                }
            }
            return ret;
        }
    }
}
