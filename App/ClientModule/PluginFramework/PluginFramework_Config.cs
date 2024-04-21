using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public partial class PluginFramework
    {
        public static void LoadTableConfig()
        {
            string root = HomePath.CurHomePath + "/Tables/";
            LogSystem.Debug("table config root = {0}", root);

            //The dictionary table needs to provide a table lookup mechanism for
            //the dictionary function
            Dict.OnFindDictionary += (Dict.FindDictionaryDelegation)((string key) => {
                TableConfig.StrDictionary cfg = TableConfig.StrDictionaryProvider.Instance.GetStrDictionary(key);
                if (null != cfg)
                    return cfg.Content;
                else
                    return key;
            });
            TableConfig.LevelProvider.Instance.Clear();
            TableConfig.LevelProvider.Instance.LoadForClient();
            TableConfig.LevelMonsterProvider.Instance.Clear();
            TableConfig.LevelMonsterProvider.Instance.LoadForClient();
            TableConfig.LevelMonsterProvider.Instance.BuildGroupedLevelMonsters();
            TableConfig.ConstProvider.Instance.Clear();
            TableConfig.ConstProvider.Instance.LoadForClient();
            TableConfig.AttrDefineProvider.Instance.Clear();
            TableConfig.AttrDefineProvider.Instance.LoadForClient();
            TableConfig.ActorProvider.Instance.Clear();
            TableConfig.ActorProvider.Instance.LoadForClient();
            TableConfig.SkillProvider.Instance.Clear();
            TableConfig.SkillProvider.Instance.LoadForClient();
            TableConfig.SkillResourcesProvider.Instance.Clear();
            TableConfig.SkillResourcesProvider.Instance.LoadForClient();
            TableConfig.SkillDataProvider.Instance.Clear();
            TableConfig.SkillDataProvider.Instance.LoadForClient();
            TableConfig.ImpactDataProvider.Instance.Clear();
            TableConfig.ImpactDataProvider.Instance.LoadForClient();
            TableConfig.SkillEventProvider.Instance.Clear();
            TableConfig.SkillEventProvider.Instance.LoadForClient();
            JoinSkillDslResource();
            BuildSkillEvent();
            TableConfig.StoryDlgProvider.Instance.Clear();
            TableConfig.StoryDlgProvider.Instance.LoadForClient();
            TableConfig.StrDictionaryProvider.Instance.Clear();
            TableConfig.StrDictionaryProvider.Instance.LoadForClient();
            TableConfig.UIProvider.Instance.Clear();
            TableConfig.UIProvider.Instance.LoadForClient();
            TableConfig.FormationProvider.Instance.Clear();
            TableConfig.FormationProvider.Instance.LoadForClient();
            BuildFormationInfo();
        }

        private static void JoinSkillDslResource()
        {
            foreach (var pair in TableConfig.SkillProvider.Instance.SkillMgr.GetData()) {
                TableConfig.Skill skill = pair.Value as TableConfig.Skill;

                TableConfig.SkillData skillData = TableConfig.SkillDataProvider.Instance.GetSkillData(skill.id);
                if (null!=skillData) {
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

        private static void BuildFormationInfo()
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
