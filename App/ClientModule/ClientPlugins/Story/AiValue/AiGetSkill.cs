using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class AiGetSkill : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0];
        int index = args[1].GetInt();
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
                result.Value = BoxedValue.FromObject(skillInfo);
            } else {
                result.Value = BoxedValue.NullObject;
            }
        }
        return false;
    }

    private DslCalculator m_Calculator = null;
}
