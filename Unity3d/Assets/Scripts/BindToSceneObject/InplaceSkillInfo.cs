using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;

public class InplaceSkillInfo : MonoBehaviour
{
    public class InplaceSkillPropertyInfoGroup
    {
        public bool IsFoldOut = false;
        public List<InplaceSkillPropertyInfo> PropertyList = null;
    }

    [System.Serializable]
    public class CustomInfo
    {}

    public CustomInfo Info = null;

    public Dictionary<int, InplaceSkillPropertyInfoGroup> Properties
    {
        get
        {
            return m_Properties;
        }
    }

    public void Rebuild(int actorId)
    {
        TableConfig.Actor actor = TableConfig.ActorProvider.Instance.GetActor(actorId);
        if (null != actor) {
            List<int> args = new List<int>();
            if (actor.skill0 > 0)
                args.Add(actor.skill0);
            if (actor.skill1 > 0)
                args.Add(actor.skill1);
            if (actor.skill2 > 0)
                args.Add(actor.skill2);
            if (actor.skill3 > 0)
                args.Add(actor.skill3);
            if (actor.skill4 > 0)
                args.Add(actor.skill4);
            if (actor.skill5 > 0)
                args.Add(actor.skill5);
            if (actor.skill6 > 0)
                args.Add(actor.skill6);
            if (actor.skill7 > 0)
                args.Add(actor.skill7);
            if (actor.skill8 > 0)
                args.Add(actor.skill8);

            args = AddDepSkills(args);
            m_Properties.Clear();
            foreach (int skillId in args) {
                if (!m_Properties.ContainsKey(skillId)) {
                    GfxSkillSystem.Instance.PreloadSkillInstance(skillId);                    
                    InplaceSkillPropertyInfoGroup group = new InplaceSkillPropertyInfoGroup();
                    group.PropertyList = PluginFramework.Instance.GetInplaceSkillPropertyInfos(skillId);
                    m_Properties.Add(skillId, group);
                }
            }
        }
    }

    private Dictionary<int, InplaceSkillPropertyInfoGroup> m_Properties = new Dictionary<int, InplaceSkillPropertyInfoGroup>();

    private static List<int> AddDepSkills(List<int> skills)
    {
        List<int> ret = new List<int>(skills);
        foreach (int skillId in skills) {
            AddDepSkillsRecursively(skillId, ret);
        }
        return ret;
    }
    private static void AddDepSkillsRecursively(int skillId, List<int> outList)
    {
        TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
        if (null != cfg) {
            if (cfg.impacttoself > 0 && !outList.Contains(cfg.impacttoself)) {
                outList.Add(cfg.impacttoself);
                AddDepSkillsRecursively(cfg.impacttoself, outList);
            }
            if (cfg.impact > 0 && !outList.Contains(cfg.impact)) {
                outList.Add(cfg.impact);
                AddDepSkillsRecursively(cfg.impact, outList);
            }
        }
    }
}
