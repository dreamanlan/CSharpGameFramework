using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    public sealed class UnitId2ObjIdFunction : SimpleExpressionBase
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
                    return obj.GetId();
                }
            }
            return 0;
        }
    }
    public sealed class ObjId2UnitIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetUnitId();
                }
            }
            return 0;
        }
    }
    public sealed class UnitId2UniqueIdFunction : SimpleExpressionBase
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
                    if (obj.UniqueId <= 0) {
                        obj.UniqueId = scene.SceneContext.GenUniqueId();
                    }
                    return obj.UniqueId;
                }
            }
            return 0;
        }
    }
    public sealed class ObjId2UniqueIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    if (obj.UniqueId <= 0) {
                        obj.UniqueId = scene.SceneContext.GenUniqueId();
                    }
                    return obj.UniqueId;
                }
            }
            return 0;
        }
    }
    public sealed class GetPositionFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return (Vector3Obj)Vector3.Zero;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return (Vector3Obj)obj.GetMovementStateInfo().GetPosition3D();
                }
            }
            return (Vector3Obj)Vector3.Zero;
        }
    }
    public sealed class GetPositionXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetMovementStateInfo().PositionX;
                }
            }
            return 0.0f;
        }
    }
    public sealed class GetPositionYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetMovementStateInfo().PositionY;
                }
            }
            return 0.0f;
        }
    }
    public sealed class GetPositionZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetMovementStateInfo().PositionZ;
                }
            }
            return 0.0f;
        }
    }
    public sealed class GetCampFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetCampId();
                }
            }
            return 0;
        }
    }
    public sealed class IsEnemyFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            int camp1 = operands[0].GetInt();
            int camp2 = operands[1].GetInt();
            return (EntityInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_ENEMY ? 1 : 0);
        }
    }
    public sealed class IsFriendFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            int camp1 = operands[0].GetInt();
            int camp2 = operands[1].GetInt();
            return (EntityInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_FRIEND ? 1 : 0);
        }
    }
    public sealed class GetHpFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.Hp;
                }
            }
            return 0;
        }
    }
    public sealed class GetEnergyFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.Energy;
                }
            }
            return 0;
        }
    }
    public sealed class GetMaxHpFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.HpMax;
                }
            }
            return 0;
        }
    }
    public sealed class GetMaxEnergyFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.EnergyMax;
                }
            }
            return 0;
        }
    }
    public sealed class CalcOffsetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return (Vector3Obj)Vector3.Zero;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                int targetId = operands[1].GetInt();
                Vector3Obj offsetObj = operands[2];
                Vector3 offset = offsetObj.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                EntityInfo target = scene.SceneContext.GetEntityById(targetId);
                if (null != obj && null != target) {
                    Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                    float y = obj.GetMovementStateInfo().PositionY;
                    Vector2 targetPos = target.GetMovementStateInfo().GetPosition2D();
                    float radian = Geometry.GetYRadian(srcPos, targetPos);
                    Vector2 newPos = srcPos + Geometry.GetRotate(new Vector2(offset.X, offset.Z), radian);
                    return (Vector3Obj)new Vector3(newPos.X, y + offset.Y, newPos.Y);
                } else if (null != obj) {
                    Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                    float y = obj.GetMovementStateInfo().PositionY;
                    float radian = obj.GetMovementStateInfo().GetFaceDir();
                    Vector2 newPos = srcPos + Geometry.GetRotate(new Vector2(offset.X, offset.Z), radian);
                    return (Vector3Obj)new Vector3(newPos.X, y + offset.Y, newPos.Y);
                }
            }
            return (Vector3Obj)Vector3.Zero;
        }
    }
    public sealed class CalcDirFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0.0f;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                int targetId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                EntityInfo target = scene.SceneContext.GetEntityById(targetId);
                if (null != obj && null != target) {
                    Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                    Vector2 targetPos = target.GetMovementStateInfo().GetPosition2D();
                    return Geometry.GetYRadian(srcPos, targetPos);
                } else if (null != obj) {
                    return obj.GetMovementStateInfo().GetFaceDir();
                }
            }
            return 0.0f;
        }
    }
    public sealed class ObjGetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int uniqueId = operands[0].GetInt();
                string localName = operands[1].ToString();
                object v;
                if (scene.SceneContext.ObjectTryGet(uniqueId, localName, out v)) {
                    return BoxedValue.FromObject(v);
                }
                if (operands.Count > 2) {
                    return operands[2];
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetTableIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetTableId();
                }
            }
            return 0;
        }
    }
    public sealed class GetLevelFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.Level;
                }
            }
            return 0;
        }
    }
    public sealed class GetAttrFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                string attrName = operands[1].ToString();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    try {
                        Type t = obj.GetType();
                        return BoxedValue.FromObject(t.InvokeMember(attrName, System.Reflection.BindingFlags.GetProperty, null, obj, null));
                    } catch (Exception ex) {
                        LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        return BoxedValue.NullObject;
                    }
                }
                if (operands.Count > 2) {
                    return operands[2];
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class IsCombatNpcFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
                if (null != entity) {
                    return (entity.IsCombatNpc() ? 1 : 0);
                }
            }
            return 0;
        }
    }
    public sealed class IsControlByStoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.IsControlByStory;
                }
            }
            return false;
        }
    }
    public sealed class CanCastSkillFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                int skillId = 0;
                bool haveSkillId = false;
                if (operands.Count > 1) {
                    skillId = operands[1].GetInt();
                    haveSkillId = true;
                }
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    if (!obj.IsDead() && !obj.IsUnderControl() && !obj.GetSkillStateInfo().IsSkillActivated()) {
                        if (!haveSkillId) {
                            return true;
                        } else {
                            SkillInfo curSkillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                            if (null != curSkillInfo) {
                                long curTime = TimeUtility.GetLocalMilliseconds();
                                if (!curSkillInfo.IsInCd(curTime)) {
                                    return true;
                                } else {
                                    LogSystem.Warn("obj {0} objcancastskill {1} is in CD {2}", obj.GetId(), skillId, curSkillInfo.GetCD(curTime));
                                }
                            }
                        }
                    } else {
                        SkillInfo oldSkillInfo = obj.GetSkillStateInfo().GetCurSkillInfo();
                        int oldSkillId = 0;
                        if (null != oldSkillInfo) {
                            oldSkillId = oldSkillInfo.SkillId;
                        }
                        LogSystem.Warn("obj {0} objcancastskill {1} return false because isdead {2} or isundercontrol {3} or isskillactivated {4} (old skill:{5})", obj.GetId(), skillId, obj.IsDead(), obj.IsUnderControl(), obj.GetSkillStateInfo().IsSkillActivated(), oldSkillId);
                    }
                }
            }
            return false;
        }
    }
    public sealed class IsUnderControlFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.IsUnderControl();
                }
            }
            return false;
        }
    }
    public sealed class ObjGetFormationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.GetMovementStateInfo().FormationIndex;
                }
            }
            return 0;
        }
    }
    public sealed class ObjFindImpactSeqByIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                int impactId = operands[1].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
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
    public sealed class ObjGetNpcTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.EntityType;
                }
            }
            return 0;
        }
    }
    public sealed class ObjGetSummonerIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.SummonerId;
                }
            }
            return 0;
        }
    }
    public sealed class ObjGetSummonSkillIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    return obj.SummonSkillId;
                }
            }
            return 0;
        }
    }
}
