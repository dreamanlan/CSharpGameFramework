using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    public sealed class NpcIdListFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                List<object> npcs = new List<object>();
                scene.EntityManager.Entities.VisitValues((EntityInfo npcInfo) => {
                    npcs.Add(npcInfo.GetId());
                });
                return BoxedValue.FromObject(npcs);
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class CombatNpcCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count > 0) {
                    int campId = operands[0].GetInt();
                    return scene.GetBattleNpcCount(campId);
                } else {
                    return scene.GetBattleNpcCount();
                }
            }
            return 0;
        }
    }
    public sealed class NpcGetFormationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    return entity.GetMovementStateInfo().FormationIndex;
                }
            }
            return 0;
        }
    }
    public sealed class NpcGetNpcTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.EntityType;
                }
            }
            return 0;
        }
    }
    public sealed class NpcGetSummonerIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.SummonerId;
                }
            }
            return 0;
        }
    }
    public sealed class NpcGetSummonSkillIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.SummonSkillId;
                }
            }
            return 0;
        }
    }
    public sealed class NpcFindImpactSeqByIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = operands[0].GetInt();
                int impactId = operands[1].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().FindImpactInfoById(impactId);
                    if (null != impactInfo) {
                        return impactInfo.Seq;
                    }
                }
            }
            return 0;
        }
    }
    public sealed class NpcCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int startUnitId = operands[0].GetInt();
                int endUnitId = operands[1].GetInt();
                return scene.GetNpcCount(startUnitId, endUnitId);
            }
            return 0;
        }
    }
}
