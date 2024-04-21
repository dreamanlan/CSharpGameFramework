using System;
using System.Collections.Generic;

using System.Text;

namespace GameFramework
{
    public class SkillStateInfo
    {
        private List<SkillInfo> m_SkillList;   // skill container
        private SkillInfo m_CurSkillInfo;      // Point skills currently released
        private List<ImpactInfo> m_ImpactList; // effects container
        private bool m_BuffChanged;            // Whether the BUFF status changes
        private int m_NextImpactSeq;

        public SkillStateInfo()
        {
            m_SkillList = new List<SkillInfo>();
            m_CurSkillInfo = null;
            m_ImpactList = new List<ImpactInfo>();
            m_NextImpactSeq = 1;
        }

        public void Reset()
        {
            m_SkillList.Clear();
            m_ImpactList.Clear();
            m_CurSkillInfo = null;
            m_BuffChanged = false;
            m_NextImpactSeq = 1;
        }

        public bool IsSkillActivated()
        {
            return (null == m_CurSkillInfo) ? false : m_CurSkillInfo.IsSkillActivated;
        }
        public void AddSkill(int index, SkillInfo info)
        {
            if (m_SkillList.Count == index) {
                m_SkillList.Insert(index, info);
            } else if (m_SkillList.Count > index) {
                m_SkillList[index] = info;
            }
        }
        public void AddSkill(SkillInfo info)
        {
            m_SkillList.Add(info);
        }
        public int GetSkillLevel(int skillIndex)
        {
            if (m_SkillList.Count > skillIndex) {
                return m_SkillList[skillIndex].SkillLevel;
            }
            return 0;
        }
        public int GetTotalSkillLevel()
        {
            int totalLevel = 0;
            for (int i = 0; i < m_SkillList.Count; i++) {
                if (m_SkillList[i] != null)
                    totalLevel += m_SkillList[i].SkillLevel;
            }
            return totalLevel;
        }
        public SkillInfo GetCurSkillInfo()
        {
            return m_CurSkillInfo;
        }
        public SkillInfo GetSkillInfoById(int skillId)
        {
            return m_SkillList.Find(
                delegate(SkillInfo info) {
                    if (info == null) return false;
                    return info.SkillId == skillId;
                }
                );
        }
        public SkillInfo GetSkillInfoByIndex(int skillIndex)
        {
            if (m_SkillList.Count > skillIndex) {
                return m_SkillList[skillIndex];
            }

            return null;
        }
        public List<SkillInfo> GetAllSkill()
        {
            return m_SkillList;
        }
        public void SetCurSkillInfo(int skillId)
        {
            SkillInfo skillInfo = m_SkillList.Find(
                delegate(SkillInfo info) {
                    if (info == null) return false;
                    return info.SkillId == skillId;
                }
                );
            if (null != skillInfo) {
                skillInfo.Reset();
                m_CurSkillInfo = skillInfo;
            }
        }
        public void RemoveSkill(int skillId)
        {
            SkillInfo oriSkill = GetSkillInfoById(skillId);
            if (oriSkill != null) {
                m_SkillList.Remove(oriSkill);
            }
        }
        public void RemoveAllSkill()
        {
            m_SkillList.Clear();
        }

        public bool BuffChanged
        {
            get { return m_BuffChanged; }
            set { m_BuffChanged = value; }
        }
        public void AddImpact(ImpactInfo info)
        {
            if (null == info.ConfigData) {
                LogSystem.Error("impact {0} config can't found !", info.ImpactId);
                return;
            }
            m_ImpactList.Add(info);
            info.Seq = m_NextImpactSeq++;
            if ((int)SkillOrImpactType.Buff == info.ConfigData.type) {
                m_BuffChanged = true;
            }
        }
        public ImpactInfo GetImpactInfoBySeq(int seq)
        {
            return m_ImpactList.Find(
                delegate(ImpactInfo info) {
                    return info.Seq == seq;
                }
                );
        }
        public ImpactInfo FindImpactInfoById(int impactId)
        {
            return m_ImpactList.Find(
                delegate(ImpactInfo info) {
                    return info.ImpactId == impactId;
                }
                );
        }
        public List<ImpactInfo> GetAllImpact()
        {
            return m_ImpactList;
        }
        public void RemoveImpact(int seq)
        {
            ImpactInfo oriImpact = GetImpactInfoBySeq(seq);
            if (oriImpact != null) {
                if ((int)SkillOrImpactType.Buff == oriImpact.ConfigData.type) {
                    m_BuffChanged = true;
                }
                m_ImpactList.Remove(oriImpact);
            }
        }
        public void RemoveAllImpact()
        {
            m_ImpactList.Clear();
            m_BuffChanged = true;
        }
    }
}
