using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
    public sealed class CalculatorCommandInfo
    {
        public List<string> Strings
        {
            get { return m_Strings; }
        }
        public List<long> Args
        {
            get { return m_Args; }
        }
        
        private List<string> m_Strings = new List<string>();
        private List<long> m_Args = new List<long>();
    }
    public sealed class CharacterProperty
    {
        public TableConfig.Actor ConfigData
        {
            get { return m_ConfigData; }
            set { m_ConfigData = value; }
        }
        public EntityInfo Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        public float GetFloat(CharacterPropertyEnum id)
        {
            return Get(id) / 1000.0f;
        }
        public void SetFloat(CharacterPropertyEnum id, float val)
        {
            Set(id, (long)(val * 1000.0f));
        }
        public void IncreaseFloat(CharacterPropertyEnum id, float delta)
        {
            Increase(id, (long)(delta * 1000.0f));
        }
        public int GetInt(CharacterPropertyEnum id)
        {
            return (int)Get(id);
        }
        public void SetInt(CharacterPropertyEnum id, int val)
        {
            Set(id, val);
        }
        public void IncreaseInt(CharacterPropertyEnum id, int delta)
        {
            Increase(id, delta);
        }
        public long GetLong(CharacterPropertyEnum id)
        {
            return Get(id);
        }
        public void SetLong(CharacterPropertyEnum id, long val)
        {
            Set(id, val);
        }
        public void IncreaseLong(CharacterPropertyEnum id, long delta)
        {
            Increase(id, delta);
        }

        public void CopyFrom(CharacterProperty other)
        {
            m_Values.Clear();
            foreach (var pair in other.m_Values) {
                m_Values.Add(pair.Key, pair.Value);
            }
        }

        internal bool HaveState(CharacterPropertyEnum id)
        {
            long val = GetState(id);
            bool disable = IsStateDisable(val);
            long ct = GetStateCount(val);
            return ct > 0 && !disable;
        }
        internal void AddState(CharacterPropertyEnum id)
        {
            long val = GetState(id);
            long disable = GetStateDisable(val);
            long ct = GetStateCount(val);
            val = (ct + 1) | disable;
            SetState(id, val);
        }
        internal void RemoveState(CharacterPropertyEnum id)
        {
            long val = GetState(id);
            long disable = GetStateDisable(val);
            long ct = GetStateCount(val);
            val = (ct > 0 ? ct - 1 : 0) | disable;
            SetState(id, val);
        }
        internal void DisableState(CharacterPropertyEnum id)
        {
            long val = GetState(id);
            long ct = GetStateCount(val);
            if (ct <= 0)
                return;
            val = ct | c_StateDisableFlag;
            SetState(id, val);
        }
        internal void ClearState(CharacterPropertyEnum id)
        {
            SetState(id, 0);
        }

        private void Increase(CharacterPropertyEnum id, long delta)
        {
            long val = Get(id);
            val += delta;
            Set(id, val);
        }
        private long Get(CharacterPropertyEnum id)
        {
            long val = 0;
            int idVal = (int)id;
            m_Values.TryGetValue(idVal, out val);
            return val;
        }
        private void Set(CharacterPropertyEnum id, long val)
        {
            int idVal = (int)id;
            m_Values[idVal] = val;
        }
        private long GetState(CharacterPropertyEnum id)
        {
            long val = 0;
            if (IsState(id)) {
                int idVal = (int)id;
                m_Values.TryGetValue(idVal, out val);
            }
            return val;
        }
        private void SetState(CharacterPropertyEnum id, long val)
        {
            if (IsState(id)) {
                int idVal = (int)id;
                m_Values[idVal] = val;
            }
        }
        
        private Dictionary<int, long> m_Values = new Dictionary<int, long>();
        private TableConfig.Actor m_ConfigData = null;
        private EntityInfo m_Owner = null;

        private static bool IsState(CharacterPropertyEnum id)
        {
            return id >= CharacterPropertyEnum.x3001_眩晕 && id < CharacterPropertyEnum.x4001_职业;
        }
        private static bool IsStateDisable(long val)
        {
            return (val & c_StateDisableFlag) == c_StateDisableFlag;
        }
        private static long GetStateDisable(long val)
        {
            return val & c_StateDisableFlag;
        }
        private static long GetStateCount(long val)
        {
            return val & c_StateCountMask;
        }

        private const long c_StateDisableFlag = unchecked((long)0x8000000000000000);
        private const long c_StateCountMask = 0x7fffffffffffffff;
    }
}
