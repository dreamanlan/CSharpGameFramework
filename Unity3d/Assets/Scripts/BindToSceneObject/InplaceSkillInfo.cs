using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Skill;
using SkillSystem;

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

            m_Properties.Clear();
            foreach (int skillId in args) {
                if (!m_Properties.ContainsKey(skillId)) {
                    GfxSkillSystem.Instance.PreloadSkillInstance(skillId);                    
                    InplaceSkillPropertyInfoGroup group = new InplaceSkillPropertyInfoGroup();
                    group.PropertyList = ClientModule.Instance.GetInplaceSkillPropertyInfos(skillId);
                    m_Properties.Add(skillId, group);
                }
            }
        }
    }

    private Dictionary<int, InplaceSkillPropertyInfoGroup> m_Properties = new Dictionary<int, InplaceSkillPropertyInfoGroup>();
}
