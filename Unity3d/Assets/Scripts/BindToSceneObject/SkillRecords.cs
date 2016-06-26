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
        public float distance = 10;
        public int duration = 0;
        public int interval = 0;
        public int canmove = 0;
        public int impact = 0;
        public int interruptPriority = 0;
        public bool isInterrupt = false;
        public int targetType = 1;
        public int aoeType = 1;
        public float aoeSize = 10;
        public float aoeAngleOrLength = 0;
        public int maxAoeTargetCount = 0;
        public int dslSkillId = 0;
        public string dslFile = string.Empty;
        public int startupSkillId = 0;
        public int flybackSkillId = 0;
        public int startupPositionType = 3000;
        public int autoCast = 1;
        public bool needTarget = false;
        public int cooldown = 0;
        public int damage = 100;
        public int addhp = 0;
        public int addmp = 0;
        public int addattack = 10;
        public int adddefence = 0;
        public int addshield = 0;
        public int addspeed = 0;
        public int addcritical = 0;
        public int addcriticalpow = 0;
        public int addrps = 0;
        public List<int> subsequentSkills = new List<int>();
        public List<SkillResource> resources = new List<SkillResource>();

        public void CopyFrom(TableConfig.Skill skill)
        {
            id = skill.id;
            desc = skill.desc;
            type = skill.type;
            icon = skill.icon;
            distance = skill.distance;
            duration = skill.duration;
            interval = skill.interval;
            canmove = skill.canmove;
            impact = skill.impact;
            interruptPriority = skill.interruptPriority;
            isInterrupt = skill.isInterrupt;
            targetType = skill.targetType;
            aoeType = skill.aoeType;
            aoeSize = skill.aoeSize;
            aoeAngleOrLength = skill.aoeAngleOrLength;
            maxAoeTargetCount = skill.maxAoeTargetCount;
            dslSkillId = skill.dslSkillId;
            dslFile = skill.dslFile;

            startupSkillId = skill.startupSkillId;
            flybackSkillId = skill.flybackSkillId;

            startupPositionType = skill.startupPositionType;
            autoCast = skill.autoCast;
            needTarget = skill.needTarget;
            cooldown = skill.cooldown;

            damage = skill.damage;
            addhp = skill.addhp;
            addmp = skill.addmp;
            addattack = skill.addattack;
            adddefence = skill.adddefence;
            addshield = skill.addshield;
            addspeed = skill.addspeed;
            addcritical = skill.addcritical;
            addcriticalpow = skill.addcriticalpow;
            addrps = skill.addrps;

            subsequentSkills.Clear();
            subsequentSkills.AddRange(skill.subsequentSkills);

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
            skill.distance = distance;
            skill.duration = duration;
            skill.interval = interval;
            skill.canmove = canmove;
            skill.impact = impact;
            skill.interruptPriority = interruptPriority;
            skill.isInterrupt = isInterrupt;
            skill.targetType = targetType;
            skill.aoeType = aoeType;
            skill.aoeSize = aoeSize;
            skill.aoeAngleOrLength = aoeAngleOrLength;
            skill.maxAoeTargetCount = maxAoeTargetCount;
            skill.dslSkillId = dslSkillId;
            skill.dslFile = dslFile;

            skill.startupSkillId = startupSkillId;
            skill.flybackSkillId = flybackSkillId;

            skill.startupPositionType = startupPositionType;
            skill.autoCast = autoCast;
            skill.needTarget = needTarget;
            skill.cooldown = cooldown;

            skill.damage = damage;
            skill.addhp = addhp;
            skill.addmp = addmp;
            skill.addattack = addattack;
            skill.adddefence = adddefence;
            skill.addshield = addshield;
            skill.addspeed = addspeed;
            skill.addcritical = addcritical;
            skill.addcriticalpow = addcriticalpow;
            skill.addrps = addrps;

            skill.subsequentSkills.Clear();
            skill.subsequentSkills.AddRange(subsequentSkills);

            skill.resources.Clear();
            foreach (SkillResource res in resources) {
                skill.resources.Add(res.Key, res.Resource);
            }
        }

        public string GetSkillClipboardText()
        {
            return string.Format("{0}\t\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t{29}\t{30}\t{31}\t{32}\t{33}\t{34}", id, desc, type, icon, distance, duration, interval, canmove, impact, interruptPriority, isInterrupt ? 1 : 0, targetType, aoeType, aoeSize, aoeAngleOrLength, maxAoeTargetCount, dslSkillId, ""/*dslFile*/, startupSkillId, flybackSkillId, startupPositionType, GameFramework.Converter.IntList2String(subsequentSkills), autoCast, needTarget ? 1 : 0, cooldown, damage, addhp, addmp, addattack, adddefence, addshield, addspeed, addcritical, addcriticalpow, addrps);
        }
        public string GetSkillDslClipboardText()
        {
            return string.Format("{0}\t{1}", dslSkillId, dslFile);
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
