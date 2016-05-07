using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public partial class ClientModule
    {
        public static void LoadTableConfig()
        {
            string root = HomePath.CurHomePath + "/Tables/";
            LogSystem.Debug("table config root = {0}", root);

            //字典表需要为字典功能提供查表机制
            Dict.OnFindDictionary += (Dict.FindDictionaryDelegation)((string key) => {
                TableConfig.StrDictionary cfg = TableConfig.StrDictionaryProvider.Instance.GetStrDictionary(key);
                return cfg.Content;
            });
            TableConfig.LevelProvider.Instance.LoadForClient();
            TableConfig.LevelMonsterProvider.Instance.LoadForClient();
            TableConfig.LevelMonsterProvider.Instance.BuildGroupedLevelMonsters();
            TableConfig.ActorProvider.Instance.LoadForClient();
            TableConfig.SkillProvider.Instance.LoadForClient();
            TableConfig.SkillDslProvider.Instance.LoadForClient();
            TableConfig.SkillResourcesProvider.Instance.LoadForClient();
            JoinSkillDslResource();
            TableConfig.StoryDlgProvider.Instance.LoadForClient();
            TableConfig.StrDictionaryProvider.Instance.LoadForClient();
            TableConfig.UIProvider.Instance.LoadForClient();
            TableConfig.FormationProvider.Instance.LoadForClient();
            BuildFormationInfo();
        }

        private static void JoinSkillDslResource()
        {
            foreach (var pair in TableConfig.SkillProvider.Instance.SkillMgr.GetData()) {
                TableConfig.Skill skill = pair.Value as TableConfig.Skill;
                TableConfig.SkillDsl skillDsl = TableConfig.SkillDslProvider.Instance.GetSkillDsl(skill.dslSkillId);
                skill.dslFile = skillDsl.dslFile;
                skill.damageData.Damage = skill.damage;
                skill.damageData.HpRecover = skill.hpRecover;
                skill.damageData.MpRecover = skill.mpRecover;
                skill.damageData.AddAttack = skill.addAttack;
                skill.damageData.AddDefence = skill.addDefence;
                skill.damageData.AddRps = skill.addRps;
                skill.damageData.AddCritical = skill.addCritical;
                skill.damageData.AddCriticalPow = skill.addCriticalPow;
                skill.damageData.AddSpeed = skill.addSpeed;
                skill.damageData.AddShield = skill.addShield;
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
