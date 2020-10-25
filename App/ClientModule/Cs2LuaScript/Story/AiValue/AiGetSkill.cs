using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiGetSkill : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiGetSkill();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        var args = _params.Values;
        int objId = args[0];
        int index = args[1].Get<int>();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            int skillId = 0;
            switch (index) {
                case 0:
                    skillId = npc.ConfigData.skill0;
                    break;
                case 1:
                    skillId = npc.ConfigData.skill1;
                    break;
                case 2:
                    skillId = npc.ConfigData.skill2;
                    break;
                case 3:
                    skillId = npc.ConfigData.skill3;
                    break;
                case 4:
                    skillId = npc.ConfigData.skill4;
                    break;
                case 5:
                    skillId = npc.ConfigData.skill5;
                    break;
                case 6:
                    skillId = npc.ConfigData.skill6;
                    break;
                case 7:
                    skillId = npc.ConfigData.skill7;
                    break;
                case 8:
                    skillId = npc.ConfigData.skill8;
                    break;
                default:
                    skillId = 0;
                    break;
            }
            if (skillId > 0) {
                SkillInfo skillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null == skillInfo) {
                    skillInfo = new SkillInfo(skillId);
                    npc.GetSkillStateInfo().AddSkill(skillInfo);
                }
                m_Proxy.Value = BoxedValue.From(skillInfo);
            } else {
                m_Proxy.Value = BoxedValue.NullObject;
            }
        }
    }

    private StoryValueResult m_Proxy = null;
}
