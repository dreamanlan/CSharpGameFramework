using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public delegate void DamageDelegation(int receiver, int caster, bool isNormalDamage, bool isCritical, int hpDamage, int npDamage);
        
    //角色监听类别标记位，用于监听某类事件触发剧情消息
    public enum StoryListenFlagEnum
    {
        Damage = 1,
        Story_Bit_2 = 1 << 1,
        Story_Bit_3 = 1 << 2,
        Story_Bit_4 = 1 << 3,
        Story_Bit_5 = 1 << 4,
        Story_Bit_6 = 1 << 5,
        Story_Bit_7 = 1 << 6,
        Story_Bit_8 = 1 << 7,
        Story_Bit_9 = 1 << 8,
        Story_Bit_10 = 1 << 9,
        Story_Bit_11 = 1 << 10,
        Story_Bit_12 = 1 << 11,
        Story_Bit_13 = 1 << 12,
        Story_Bit_14 = 1 << 13,
        Story_Bit_15 = 1 << 14,
        Story_Bit_16 = 1 << 15,
        Story_Bit_17 = 1 << 16,
        Story_Bit_18 = 1 << 17,
        Story_Bit_19 = 1 << 18,
        Story_Bit_20 = 1 << 19,
        Story_Bit_21 = 1 << 20,
        Story_Bit_22 = 1 << 21,
        Story_Bit_23 = 1 << 22,
        Story_Bit_24 = 1 << 23,
        Story_Bit_25 = 1 << 24,
        Story_Bit_26 = 1 << 25,
        Story_Bit_27 = 1 << 26,
        Story_Bit_28 = 1 << 27,
        Story_Bit_29 = 1 << 28,
        Story_Bit_30 = 1 << 29,           // 通用标记位 3
        Story_Bit_31 = 1 << 30,           // 通用标记位 2
        Story_Bit_32 = 1 << 31,           // 通用标记位 1
    }
    public static class StoryListenFlagUtility
    {
        public const string c_story_bit_prefix = "story_bit_";
        public static StoryListenFlagEnum FromString(string name)
        {
            if (name.CompareTo("damage") == 0) {
                return StoryListenFlagEnum.Damage;
            } else if (name.StartsWith(c_story_bit_prefix)) {
                int bit = int.Parse(name.Substring(c_story_bit_prefix.Length));
                return (StoryListenFlagEnum)(1 << (bit - 1));
            } else {
                return (StoryListenFlagEnum)0;
            }
        }
        public static string ToString(StoryListenFlagEnum type)
        {
            switch (type) {
                case (StoryListenFlagEnum)0:
                    return string.Empty;
                case StoryListenFlagEnum.Damage:
                    return "damage";
                default:
                    int bit = (int)Math.Log((int)type, 2) + 1;
                    return c_story_bit_prefix + bit;
            }
        }
    }

    public partial class EntityInfo
    {
        public struct AttackerInfo
        {
            public long m_AttackTime;
            public int m_HpDamage;
            public int m_NpDamage;
        }

        public void InitBase(int id)
        {
            m_Id = id;
            m_UnitId = 0;
            m_TableId = 0;
            m_AIEnable = true;
            m_BaseProperty = new CharacterProperty();
            m_ActualProperty = new CharacterProperty();
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
        public int GetTableId()
        {
            return m_TableId;
        }
        public void SetTableId(int id)
        {
            m_TableId = id;
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
        public int Level
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x5001_等级);
            }
            set
            {
                BaseProperty.SetInt(CharacterPropertyEnum.x5001_等级, value);
                ActualProperty.SetInt(CharacterPropertyEnum.x5001_等级, value);
                m_LevelChanged = true;
            }
        }
        public int Exp
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x5002_经验);
            }
            set
            {
                BaseProperty.SetInt(CharacterPropertyEnum.x5002_经验, value);
                ActualProperty.SetInt(CharacterPropertyEnum.x5002_经验, value);
            }
        }
        public int Hp
        {
            get 
            { 
                return ActualProperty.GetInt(CharacterPropertyEnum.x2006_当前生命值); 
            }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > HpMax)
                    value = HpMax;
                BaseProperty.SetInt(CharacterPropertyEnum.x2006_当前生命值, value);
                ActualProperty.SetInt(CharacterPropertyEnum.x2006_当前生命值, value);
            }
        }
        public int HpMax
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x2005_最大生命值);
            }
        }
        public int Energy
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x2006_当前生命值);
            }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > EnergyMax)
                    value = EnergyMax;
                BaseProperty.SetInt(CharacterPropertyEnum.x2006_当前生命值, value);
                ActualProperty.SetInt(CharacterPropertyEnum.x2006_当前生命值, value);
            }
        }
        public int EnergyMax
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x2005_最大生命值);
            }
        }
        public int Shield
        {
            get
            {
                return ActualProperty.GetInt(CharacterPropertyEnum.x2012_护盾值);
            }
            set
            {
                BaseProperty.SetInt(CharacterPropertyEnum.x2012_护盾值, value);
                ActualProperty.SetInt(CharacterPropertyEnum.x2012_护盾值, value);
            }
        }
        public float Speed
        {
            get
            {
                return ActualProperty.GetFloat(CharacterPropertyEnum.x2011_最终速度);
            }
            set
            {
                BaseProperty.SetFloat(CharacterPropertyEnum.x2011_最终速度, value);
                ActualProperty.SetFloat(CharacterPropertyEnum.x2011_最终速度, value);
            }
        }
        public bool HaveState(CharacterPropertyEnum id)
        {
            return ActualProperty.HaveState(id);
        }
        public void AddState(CharacterPropertyEnum id)
        {
            BaseProperty.AddState(id);
            ActualProperty.AddState(id);
        }
        public void RemoveState(CharacterPropertyEnum id)
        {
            BaseProperty.RemoveState(id);
            ActualProperty.RemoveState(id);
        }
        public void DisableState(CharacterPropertyEnum id)
        {
            BaseProperty.DisableState(id);
            ActualProperty.DisableState(id);
        }
        public void ClearState(CharacterPropertyEnum id)
        {
            BaseProperty.ClearState(id);
            ActualProperty.ClearState(id);
        }
        public bool HaveAnyState(params CharacterPropertyEnum[] states)
        {
            bool ret = false;
            for (int i = 0; i < states.Length; ++i) {
                if (HaveState(states[i])) {
                    ret = true;
                    break;
                }
            }
            return ret;
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

        public CharacterProperty BaseProperty
        {
            get
            {
                return m_BaseProperty;
            }
        }
        public CharacterProperty ActualProperty
        {
            get
            {
                return m_ActualProperty;
            }
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }
        public bool IsDeadSkillCasting()
        {
            return Hp <= 0 && DeadTime > 0;
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
        public void CopyBaseAttr()
        {
            m_ActualProperty.CopyFrom(m_BaseProperty);
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
            if (HaveState(CharacterPropertyEnum.x3001_眩晕) ||
                HaveState(CharacterPropertyEnum.x3002_昏睡) ||
                HaveState(CharacterPropertyEnum.x3005_浮空) ||
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
            if (HaveState(CharacterPropertyEnum.x3001_眩晕) || HaveState(CharacterPropertyEnum.x3002_昏睡) || HaveState(CharacterPropertyEnum.x3005_浮空)) {
                return false;
            }
            if (GetSkillStateInfo().IsSkillActivated()) {
                return false;
            }
            return true;
        }

        protected void ResetBaseInfo()
        {
            m_CampId = 0;
            m_UniqueId = 0;
            m_OwnerId = -1;
            m_SummonerId = -1;
            m_SummonSkillId = -1;
            SetAIEnable(true);
            m_DeadTime = 0;

            m_IsControlByStory = false;
            m_IsControlByManual = false;
            m_LevelChanged = false;
            m_PropertyChanged = false;

            m_CanUseSkill = true;
            m_KillerId = 0;

            Hp = 0;
            Energy = 0;
            Shield = 0;
            ResetAttackerInfo();
            GetMovementStateInfo().Reset();
            GetSkillStateInfo().Reset();
            GetCombatStatisticInfo().Reset();
        }

        private int m_Id = 0;
        private float m_Radius = 1.0f;
        private int m_UnitId = 0;
        private int m_TableId = 0;
        private int m_OwnerId = -1;
        private int m_SummonerId = 0;
        private int m_SummonSkillId = 0;
        private string m_Name = "";
        private string m_Model = "";
        private bool m_AIEnable = true;
        private int m_UniqueId = 0;
        private bool m_IsControlByStory = false;
        private bool m_IsControlByManual = false;
        private bool m_LevelChanged = false;
        private bool m_PropertyChanged = false;
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
                ret = true;
            }
            return ret;
        }
    }
}
