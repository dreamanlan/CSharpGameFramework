using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SkillRecords : MonoBehaviour
{
    [System.Serializable]
    public class SkillRecord
    {
        [System.Serializable]
        public class SkillResource
        {
            public string Key = string.Empty;
            public string Resource = string.Empty;
            [HideInInspector]
            public string ResourceFilter = string.Empty;
        }

        public int id = 1;
        public string desc = string.Empty;
        public int type = 0;
        public int icon = 1;
        public int impacttoself = 0;
        public int impact = 0;
        public int targetType = 1;
        public int aoeType = 1;
        public float aoeSize = 10;
        public float aoeAngleOrLength = 0;
        public int maxAoeTargetCount = 0;
        public List<SkillResource> resources = new List<SkillResource>();
        public string dslFile = string.Empty;

        public void CopyFrom(TableConfig.Skill skill)
        {
            id = skill.id;
            desc = skill.desc;
            type = skill.type;
            icon = skill.icon;
            impacttoself = skill.impacttoself;
            impact = skill.impact;
            targetType = skill.targetType;
            aoeType = skill.aoeType;
            aoeSize = skill.aoeSize;
            aoeAngleOrLength = skill.aoeAngleOrLength;
            maxAoeTargetCount = skill.maxAoeTargetCount;
            dslFile = skill.dslFile;

            resources.Clear();
            foreach (var pair in skill.resources) {
                SkillResource res = new SkillResource();
                res.Key = pair.Key;
                res.Resource = pair.Value;
                resources.Add(res);
            }
        }
        public void CopyTo(TableConfig.Skill skill)
        {
            skill.id = id;
            skill.desc = desc;
            skill.type = type;
            skill.icon = icon;
            skill.impacttoself = impacttoself;
            skill.impact = impact;
            skill.targetType = targetType;
            skill.aoeType = aoeType;
            skill.aoeSize = aoeSize;
            skill.aoeAngleOrLength = aoeAngleOrLength;
            skill.maxAoeTargetCount = maxAoeTargetCount;
            skill.dslFile = dslFile;

            skill.resources.Clear();
            foreach (SkillResource res in resources) {
                skill.resources.Add(res.Key, res.Resource);
            }
        }

        public string GetSkillClipboardText()
        {
            return string.Format("{0}\t\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}", id, desc, type, icon, impacttoself, impact, targetType, aoeType, aoeSize, aoeAngleOrLength, maxAoeTargetCount, dslFile);
        }
        public string GetSkillResourcesClipboardText()
        {
            string prestr = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach(SkillResource res in resources){
                sb.AppendFormat("{0}{1}\t{2}\t{3}", prestr, id, res.Key, res.Resource);
                prestr = "\r\n";
            }
            return sb.ToString();
        }
    }

    public List<SkillRecord> records = new List<SkillRecord>();
}
