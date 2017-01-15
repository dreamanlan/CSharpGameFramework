using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableConfig
{
    public class SkillDamageData
    {
        public List<int> Multiples;
        public List<int> Damages;
        public List<int> Vampires;
        public int AddSc;
        public int AddUc;
        public bool IsCritical = false;
        public bool IsBlock = false;
        public bool IsFinal = false;
        public int ComboSource = 0;
        
        public int GetMultiple(int index)
        {
            if (index >= 0 && index < Multiples.Count) {
                return Multiples[index];
            } else {
                return 1;
            }
        }
        public int GetDamage(int index)
        {
            if (index >= 0 && index < Damages.Count) {
                return Damages[index];
            } else {
                return 0;
            }
        }
        public int GetVampire(int index)
        {
            if (index >= 0 && index < Vampires.Count) {
                return Vampires[index];
            } else {
                return 0;
            }
        }

        public void Init()
        {
            Multiples = new List<int>();
            Damages = new List<int>();
            Vampires = new List<int>();
        }
        public void CopyFrom(SkillDamageData other)
        {
            Multiples.Clear();
            Multiples.AddRange(other.Multiples);
            Damages.Clear();
            Damages.AddRange(other.Damages);
            Vampires.Clear();
            Vampires.AddRange(other.Vampires);
            AddSc = other.AddSc;
            AddUc = other.AddUc;
            IsCritical = other.IsCritical;
            IsBlock = other.IsBlock;
            IsFinal = other.IsFinal;
            ComboSource = other.ComboSource;
        }
        public void Merge(SkillDamageData other)
        {
            if (null != other) {
                Merge(Multiples, other.Multiples);
                Merge(Damages, other.Damages);
                Merge(Vampires, other.Vampires);
                if (other.AddSc > 0)
                    AddSc = other.AddSc;
                if (other.AddUc > 0)
                    AddUc = other.AddUc;
                if (other.IsCritical)
                    IsCritical = other.IsCritical;
                if (other.IsBlock)
                    IsBlock = other.IsBlock;
                if (other.IsFinal)
                    IsFinal = other.IsFinal;
                if (other.ComboSource > 0)
                    ComboSource = other.ComboSource;
            }
        }

        private static void Merge(List<int> dest, List<int> src)
        {
            if (null != dest && null != src) {
                int ct = dest.Count;
                for (int i = 0; i < src.Count; ++i) {
                    if (src[i] != 0) {
                        if (i < ct) {
                            dest[i] = src[i];
                        } else {
                            dest.Add(src[i]);
                        }
                    }
                }
            }
        }
        private static void Merge(List<bool> dest, List<bool> src)
        {
            if (null != dest && null != src) {
                int ct = dest.Count;
                for (int i = 0; i < src.Count; ++i) {
                    if (src[i]) {
                        if (i < ct) {
                            dest[i] = src[i];
                        } else {
                            dest.Add(src[i]);
                        }
                    }
                }
            }
        }
    }
    public sealed partial class Skill
    {
        public Dictionary<string, string> resources = new Dictionary<string, string>();
        public SkillDamageData damageData = new SkillDamageData();
        public Dictionary<int, int> attrValues = new Dictionary<int, int>();
        public SkillData skillData = null;
        public ImpactData impactData = null;

        public float SkillDistance
        {
            get
            {
                if (null != skillData) {
                    return skillData.distance;
                }
                return 1.0f;
            }
        }
        public int SkillCooldown
        {
            get
            {
                if (null != skillData) {
                    return skillData.cooldown;
                }
                return 0;
            }
        }
        public int ImpactDuration
        {
            get
            {
                if (null != impactData) {
                    return impactData.duration;
                }
                return 1000;
            }
        }
        public int ImpactCooldown
        {
            get
            {
                if (null != impactData) {
                    return impactData.cooldown;
                }
                return 0;
            }
        }
    }
}
