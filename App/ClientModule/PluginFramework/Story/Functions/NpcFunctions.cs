using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class NpcIdListFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            List<object> npcs = new List<object>();
            PluginFramework.Instance.EntityManager.Entities.VisitValues((EntityInfo npcInfo) => {
                npcs.Add(npcInfo.GetId());
            });
            return BoxedValue.FromObject(npcs);
        }
    }
    internal sealed class CombatNpcCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int campId = operands[0].GetInt();
                return PluginFramework.Instance.GetBattleNpcCount(campId);
            } else {
                return PluginFramework.Instance.GetBattleNpcCount();
            }
        }
    }
    internal sealed class NpcGetFormationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int unitId = operands[0].GetInt();
                EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != entity) {
                    return entity.GetMovementStateInfo().FormationIndex;
                }
            }
            return 0;
        }
    }
    internal sealed class NpcGetNpcTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.EntityType;
                }
            }
            return 0;
        }
    }
    internal sealed class NpcGetSummonerIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.SummonerId;
                }
            }
            return 0;
        }
    }
    internal sealed class NpcGetSummonSkillIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int unitId = operands[0].GetInt();
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    return obj.SummonSkillId;
                }
            }
            return 0;
        }
    }
    internal sealed class NpcFindImpactSeqByIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                int unitId = operands[0].GetInt();
                int impactId = operands[1].GetInt();
                EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
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
    internal sealed class NpcCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                int startUnitId = operands[0].GetInt();
                int endUnitId = operands[1].GetInt();
                return PluginFramework.Instance.GetNpcCount(startUnitId, endUnitId);
            }
            return 0;
        }
    }
}
