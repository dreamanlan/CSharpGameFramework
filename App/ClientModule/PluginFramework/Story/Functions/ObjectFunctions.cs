using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class UnitId2ObjIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int unitId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                return obj.GetId();
            }
            return 0;
        }
    }
    internal sealed class ObjId2UnitIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.GetUnitId();
            }
            return 0;
        }
    }
    internal sealed class UnitId2UniqueIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int unitId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                if (obj.UniqueId <= 0) {
                    obj.UniqueId = PluginFramework.Instance.SceneContext.GenUniqueId();
                }
                return obj.UniqueId;
            }
            return 0;
        }
    }
    internal sealed class ObjId2UniqueIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                if (obj.UniqueId <= 0) {
                    obj.UniqueId = PluginFramework.Instance.SceneContext.GenUniqueId();
                }
                return obj.UniqueId;
            }
            return 0;
        }
    }
    internal sealed class GetPositionFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return (Vector3Obj)Vector3.Zero;
            var objPathVal = operands[0];
            int worldOrLocal = 1;
            if (operands.Count > 1) {
                worldOrLocal = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == worldOrLocal)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return (Vector3Obj)new ScriptRuntime.Vector3(pt.x, pt.y, pt.z);
            }
            return (Vector3Obj)ScriptRuntime.Vector3.Zero;
        }
    }
    internal sealed class GetPositionXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.x;
            }
            return 0.0f;
        }
    }
    internal sealed class GetPositionYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.y;
            }
            return 0.0f;
        }
    }
    internal sealed class GetPositionZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.z;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return (Vector3Obj)Vector3.Zero;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return (Vector3Obj)new ScriptRuntime.Vector3(pt.x, pt.y, pt.z);
            }
            return (Vector3Obj)ScriptRuntime.Vector3.Zero;
        }
    }
    internal sealed class GetRotationXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.x;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.y;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0.0f;
            var objPathVal = operands[0];
            int localOrWorld = 1;
            if (operands.Count > 1) {
                localOrWorld = operands[1].GetInt();
            }
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == localOrWorld)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.z;
            }
            return 0.0f;
        }
    }
    internal sealed class GetScaleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return (Vector3Obj)new ScriptRuntime.Vector3(1, 1, 1);
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt = obj.transform.localScale;
                return (Vector3Obj)new ScriptRuntime.Vector3(pt.x, pt.y, pt.x);
            }
            return (Vector3Obj)new ScriptRuntime.Vector3(1, 1, 1);
        }
    }
    internal sealed class GetScaleXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 1.0f;
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.x;
            }
            return 1.0f;
        }
    }
    internal sealed class GetScaleYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 1.0f;
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.y;
            }
            return 1.0f;
        }
    }
    internal sealed class GetScaleZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 1.0f;
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.z;
            }
            return 1.0f;
        }
    }
    internal sealed class Vector3Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: vector3(x, y, z)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            Vector3Obj vecObj = new Vector3Obj { Value = new Vector3(x, y, z) };
            return BoxedValue.FromObject(vecObj);
        }
    }
    internal sealed class GetCampFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.GetCampId();
            }
            return 0;
        }
    }
    internal sealed class IsEnemyFunction : SimpleExpressionBase
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
    internal sealed class IsFriendFunction : SimpleExpressionBase
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
    internal sealed class GetHpFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.Hp;
            }
            return 0;
        }
    }
    internal sealed class GetEnergyFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.Energy;
            }
            return 0;
        }
    }
    internal sealed class GetMaxHpFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.HpMax;
            }
            return 0;
        }
    }
    internal sealed class GetMaxEnergyFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.EnergyMax;
            }
            return 0;
        }
    }
    internal sealed class CalcOffsetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return (Vector3Obj)Vector3.Zero;
            int objId = operands[0].GetInt();
            int targetId = operands[1].GetInt();
            Vector3Obj offsetObj = operands[2];
            Vector3 offset = offsetObj.Value;
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
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
            return (Vector3Obj)Vector3.Zero;
        }
    }
    internal sealed class CalcDirFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0.0f;
            int objId = operands[0].GetInt();
            int targetId = operands[1].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
            if (null != obj && null != target) {
                Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                Vector2 targetPos = target.GetMovementStateInfo().GetPosition2D();
                return Geometry.GetYRadian(srcPos, targetPos);
            } else if (null != obj) {
                return obj.GetMovementStateInfo().GetFaceDir();
            }
            return 0.0f;
        }
    }
    internal sealed class ObjGetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int uniqueId = operands[0].GetInt();
            string localName = operands[1].ToString();
            object v;
            if (PluginFramework.Instance.SceneContext.ObjectTryGet(uniqueId, localName, out v)) {
                return BoxedValue.FromObject(v);
            }
            if (operands.Count > 2) {
                return operands[2];
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetTableIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.GetTableId();
            }
            return 0;
        }
    }
    internal sealed class GetLevelFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.Level;
            }
            return 0;
        }
    }
    internal sealed class GetAttrFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int attrId = operands[1].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.ActualProperty.GetLong((CharacterPropertyEnum)attrId);
            }
            if (operands.Count > 2) {
                return operands[2];
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class IsCombatNpcFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityById(objId);
            if (null != entity) {
                return (entity.IsCombatNpc() ? 1 : 0);
            }
            return 0;
        }
    }
    internal sealed class IsControlByStoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.IsControlByStory;
            }
            return false;
        }
    }
    internal sealed class CanCastSkillFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            int objId = operands[0].GetInt();
            int skillId = 0;
            bool haveSkillId = false;
            if (operands.Count > 1) {
                skillId = operands[1].GetInt();
                haveSkillId = true;
            }
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
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
            return false;
        }
    }
    internal sealed class IsUnderControlFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return false;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.IsUnderControl();
            }
            return false;
        }
    }
    internal sealed class ObjGetFormationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.GetMovementStateInfo().FormationIndex;
            }
            return 0;
        }
    }
    internal sealed class ObjFindImpactSeqByIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return 0;
            int objId = operands[0].GetInt();
            int impactId = operands[1].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityById(objId);
            if (null != entity) {
                ImpactInfo impactInfo = entity.GetSkillStateInfo().FindImpactInfoById(impactId);
                if (null != impactInfo) {
                    return impactInfo.Seq;
                }
            }
            return 0;
        }
    }
    internal sealed class ObjGetNpcTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.EntityType;
            }
            return 0;
        }
    }
    internal sealed class ObjGetSummonerIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.SummonerId;
            }
            return 0;
        }
    }
    internal sealed class ObjGetSummonSkillIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                return obj.SummonSkillId;
            }
            return 0;
        }
    }
}
