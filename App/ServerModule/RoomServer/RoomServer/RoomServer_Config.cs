using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;

namespace RoomServer
{
    internal sealed partial class RoomServer
    {
        private void LoadData()
        {
            try {
                TableConfig.LevelProvider.Instance.LoadForServer();
                TableConfig.LevelMonsterProvider.Instance.LoadForServer();
                TableConfig.LevelMonsterProvider.Instance.BuildGroupedLevelMonsters();
                TableConfig.ConstProvider.Instance.LoadForServer();
                TableConfig.AttrDefineProvider.Instance.LoadForServer();
                TableConfig.ActorProvider.Instance.LoadForServer();
                TableConfig.SkillProvider.Instance.LoadForServer();
                TableConfig.SkillResourcesProvider.Instance.LoadForServer();
                TableConfig.SkillDataProvider.Instance.LoadForServer();
                TableConfig.ImpactDataProvider.Instance.LoadForServer();
                TableConfig.SkillEventProvider.Instance.LoadForServer();
                JoinSkillDslResource();
                BuildSkillEvent();
                TableConfig.FormationProvider.Instance.LoadForServer();
                BuildFormationInfo();
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "LoadData Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void JoinSkillDslResource()
        {
            foreach (var pair in TableConfig.SkillProvider.Instance.SkillMgr.GetData()) {
                TableConfig.Skill skill = pair.Value as TableConfig.Skill;
                
                TableConfig.SkillData skillData = TableConfig.SkillDataProvider.Instance.GetSkillData(skill.id);
                if (null != skillData) {
                    skill.skillData = skillData;

                    if (skill.type == (int)SkillOrImpactType.Skill) {
                        skill.damageData.Multiples = skillData.multiple;
                        skill.damageData.Damages = skillData.damage;
                        skill.damageData.AddSc = skillData.beaddsc;
                        skill.damageData.AddUc = skillData.beadduc;
                        skill.damageData.Vampires = skillData.vampire;

                        if (skillData.attr1 > 0 && skillData.value1 > 0)
                            skill.attrValues[skillData.attr1] = skillData.value1;
                        if (skillData.attr2 > 0 && skillData.value2 > 0)
                            skill.attrValues[skillData.attr2] = skillData.value2;
                        if (skillData.attr3 > 0 && skillData.value3 > 0)
                            skill.attrValues[skillData.attr3] = skillData.value3;
                        if (skillData.attr4 > 0 && skillData.value4 > 0)
                            skill.attrValues[skillData.attr4] = skillData.value4;
                        if (skillData.attr5 > 0 && skillData.value5 > 0)
                            skill.attrValues[skillData.attr5] = skillData.value5;
                        if (skillData.attr6 > 0 && skillData.value6 > 0)
                            skill.attrValues[skillData.attr6] = skillData.value6;
                        if (skillData.attr7 > 0 && skillData.value7 > 0)
                            skill.attrValues[skillData.attr7] = skillData.value7;
                        if (skillData.attr8 > 0 && skillData.value8 > 0)
                            skill.attrValues[skillData.attr8] = skillData.value8;
                    }
                }
                TableConfig.ImpactData impactData = TableConfig.ImpactDataProvider.Instance.GetImpactData(skill.id);
                if (null != impactData) {
                    skill.impactData = impactData;

                    if (skill.type == (int)SkillOrImpactType.Buff) {
                        skill.damageData.Multiples = impactData.multiple;
                        skill.damageData.Damages = impactData.damage;
                        skill.damageData.Vampires = impactData.vampire;

                        if (impactData.attr1 > 0 && impactData.value1 > 0)
                            skill.attrValues[impactData.attr1] = impactData.value1;
                        if (impactData.attr2 > 0 && impactData.value2 > 0)
                            skill.attrValues[impactData.attr2] = impactData.value2;
                        if (impactData.attr3 > 0 && impactData.value3 > 0)
                            skill.attrValues[impactData.attr3] = impactData.value3;
                        if (impactData.attr4 > 0 && impactData.value4 > 0)
                            skill.attrValues[skillData.attr4] = impactData.value4;
                        if (impactData.attr5 > 0 && impactData.value5 > 0)
                            skill.attrValues[skillData.attr5] = impactData.value5;
                        if (impactData.attr6 > 0 && impactData.value6 > 0)
                            skill.attrValues[skillData.attr6] = impactData.value6;
                        if (impactData.attr7 > 0 && impactData.value7 > 0)
                            skill.attrValues[skillData.attr7] = impactData.value7;
                        if (impactData.attr8 > 0 && impactData.value8 > 0)
                            skill.attrValues[skillData.attr8] = impactData.value8;
                    }
                }
            }
            var resources = TableConfig.SkillResourcesProvider.Instance.SkillResourcesMgr.GetData();
            foreach (var resource in resources) {
                int skillId = resource.skillId;
                string key = resource.key;
                string res = resource.resource;
                TableConfig.Skill skill = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                if (null != skill) {
                    if (skill.resources.ContainsKey(key)) {
                        //repeat
                    } else {
                        skill.resources.Add(key, res);
                    }
                }
            }
        }

        private static void BuildSkillEvent()
        {
            Dictionary<int, Dictionary<int, Dictionary<int, TableConfig.SkillEvent>>> dict = new Dictionary<int, Dictionary<int, Dictionary<int, TableConfig.SkillEvent>>>();
            foreach (var item in TableConfig.SkillEventProvider.Instance.SkillEventMgr.GetData()) {
                item.args.Add(item.param1);
                item.args.Add(item.param2);
                item.args.Add(item.param3);
                item.args.Add(item.param4);
                item.args.Add(item.param5);
                item.args.Add(item.param6);
                item.args.Add(item.param7);
                item.args.Add(item.param8);

                Dictionary<int, Dictionary<int, TableConfig.SkillEvent>> skillEventDict;
                if (!dict.TryGetValue(item.actorId, out skillEventDict)) {
                    skillEventDict = new Dictionary<int, Dictionary<int, TableConfig.SkillEvent>>();
                    dict.Add(item.actorId, skillEventDict);
                }
                Dictionary<int, TableConfig.SkillEvent> skillEvents;
                if (!skillEventDict.TryGetValue(item.skillId, out skillEvents)) {
                    skillEvents = new Dictionary<int, TableConfig.SkillEvent>();
                    skillEventDict.Add(item.skillId, skillEvents);
                }
                skillEvents[item.eventId] = item;
            }
            TableConfig.SkillEventProvider.Instance.skillEventTable = dict;
        }

        private void BuildFormationInfo()
        {
            foreach (var pair in TableConfig.FormationProvider.Instance.FormationMgr.GetData()) {
                var record = pair.Value as TableConfig.Formation;
                if (null != record) {
                    record.BuildFormationInfo();
                }
            }
        }
    }
}
