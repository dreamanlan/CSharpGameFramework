using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableConfig
{
    public class SkillDamageData
    {
        public float Damage;
        public int MpRecover;
        public int HpRecover;
        public int AddAttack;
        public int AddDefence;
        public int AddRps;
        public int AddCritical;
        public int AddCriticalPow;
        public float AddSpeed;
        public int AddShield;

        public void CopyFrom(SkillDamageData other)
        {
            if (null != other) {
                Damage = other.Damage;
                MpRecover = other.MpRecover;
                HpRecover = other.HpRecover;
                AddAttack = other.AddAttack;
                AddDefence = other.AddDefence;
                AddRps = other.AddRps;
                AddCritical = other.AddCritical;
                AddCriticalPow = other.AddCriticalPow;
                AddSpeed = other.AddSpeed;
                AddShield = other.AddShield;
            }
        }
        public void Merge(SkillDamageData other)
        {
            if (null != other) {
                Damage += other.Damage;
                MpRecover += other.MpRecover;
                HpRecover += other.HpRecover;
                AddAttack += other.AddAttack;
                AddDefence += other.AddDefence;
                AddRps += other.AddRps;
                AddCritical += other.AddCritical;
                AddCriticalPow += other.AddCriticalPow;
                AddSpeed += other.AddSpeed;
                AddShield += other.AddShield;
            }
        }
    }
    public sealed partial class Skill
    {
        public string dslFile;
        public Dictionary<string, string> resources = new Dictionary<string, string>();
        public SkillDamageData damageData = new SkillDamageData();        
    }
}
