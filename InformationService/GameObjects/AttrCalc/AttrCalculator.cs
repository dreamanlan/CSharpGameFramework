using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class AttrCalculator
    {
        public static void LoadConfig()
        {
            Calculator.Init();
            SkillCalculator.Init();
            Calculator.Load("Dsl/Calc/AttrCalc.dsl");
            SkillCalculator.Load("Dsl/Calc/SkillCalc.dsl");
        }
        public static void CopyBaseProperty(EntityInfo obj)
        {
            obj.CopyBaseAttr();
        }
        public static void RefixAttrByImpact(EntityInfo obj)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < impacts.Count; ++i) {
                ImpactInfo impact = impacts[i];
                if (impact.ConfigData.type == (int)SkillOrImpactType.Buff) {
                    impact.RefixCharacterProperty(obj);
                }
            }
        }
        public static void RefixAttrBySkill(EntityInfo obj, TableConfig.Skill cfg)
        {
            if (cfg.attrValues.Count > 0) {
                foreach (var pair in cfg.attrValues) {
                    if (pair.Key != (int)CharacterPropertyEnum.x2012_护盾值) {//护盾的添加与删除放在触发器里处理了
                        obj.ActualProperty.IncreaseInt((CharacterPropertyEnum)pair.Key, pair.Value);
                    }
                }
            }
        }
        public static void Calc(EntityInfo npc)
        {
            Calc(npc, null);
        }
        public static void Calc(EntityInfo npc, TableConfig.Skill cfg)
        {
            int oldHp = npc.Hp;

            AttrCalculator.CopyBaseProperty(npc);
            AttrCalculator.RefixAttrByImpact(npc);
            if (null != cfg) {
                RefixAttrBySkill(npc, cfg);
            }
            Calculator.Calc(npc.SceneContext, npc.ActualProperty, null, "attr");

            if (oldHp > 0 && npc.Hp <= 0) {
                LogSystem.Error("hp calc to {0}", npc.Hp);
                Helper.LogCallStack();
            }
        }
        public static long Calc(SceneContextInfo context, CharacterProperty npc, CharacterProperty target, string proc, params long[] args)
        {
            return Calculator.Calc(context, npc, target, proc, args);
        }
        public static long SkillCalc(SceneContextInfo context, CharacterProperty npc, CharacterProperty target, string proc, params long[] args)
        {
            return SkillCalculator.Calc(context, npc, target, proc, args);
        }

        private static AttrCalc.DslCalculator Calculator
        {
            get
            {
                if (null == mCalculator) {
                    mCalculator = new AttrCalc.DslCalculator();
                }
                return mCalculator;
            }
        }
        private static AttrCalc.DslCalculator SkillCalculator
        {
            get
            {
                if (null == mSkillCalculator) {
                    mSkillCalculator = new AttrCalc.DslCalculator();
                }
                return mSkillCalculator;
            }
        }

        [ThreadStatic]
        private static AttrCalc.DslCalculator mCalculator = null;
        [ThreadStatic]
        private static AttrCalc.DslCalculator mSkillCalculator = null;
    }
}
