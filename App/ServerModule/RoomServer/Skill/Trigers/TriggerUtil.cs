using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    internal class TriggerUtil
    {
        internal static Vector3 TransformPoint(Vector3 pos, Vector3 offset, float radian)
        {
            ScriptRuntime.Vector2 newOffset = Geometry.GetRotate(new ScriptRuntime.Vector2(offset.X, offset.Z), radian);
            Vector3 result = pos + new Vector3(newOffset.X, offset.Y, newOffset.Y);
            return result;
        }
        internal static void Lookat(Scene scene, EntityInfo obj, Vector3 target, float rotateDegree)
        {
            if (scene.EntityController.GetEntityType(obj) == (int)EntityTypeEnum.Tower)
                return;
            Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
            obj.GetMovementStateInfo().SetFaceDir(Geometry.GetYRadian(new Vector2(srcPos.X, srcPos.Z), new Vector2(target.X, target.Z)));
        }
        internal static void Lookat(Scene scene, EntityInfo obj, Vector3 target)
        {
            if (scene.EntityController.GetEntityType(obj) == (int)EntityTypeEnum.Tower)
                return;
            Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
            obj.GetMovementStateInfo().SetFaceDir(Geometry.GetYRadian(new Vector2(srcPos.X, srcPos.Z), new Vector2(target.X, target.Z)));
        }

        internal static List<EntityInfo> FindTargetInSector(Scene scene, Vector3 center,
                                                      float radius,
                                                      Vector3 direction,
                                                      Vector3 degreeCenter,
                                                      float degree)
        {
            List<EntityInfo> result = new List<EntityInfo>();
            scene.KdTree.Query(center.X, center.Y, center.Z, radius, (float distSqr, KdTreeObject kdTreeObj) => {
                ScriptRuntime.Vector3 pos = kdTreeObj.Position;
                Vector3 targetDir = new Vector3(pos.X, pos.Y, pos.Z) - degreeCenter;
                targetDir.Y = 0;
                if (Math.Abs(Vector3.Angle(targetDir, direction)) <= degree) {
                    EntityInfo obj = scene.EntityController.GetGameObject(kdTreeObj.Object.GetId());
                    result.Add(obj);
                }
            });
            return result;
        }
        
        internal static float ConvertToSecond(long delta)
        {
            return delta / 1000000.0f;
        }

        internal static void SetObjVisible(EntityInfo obj, bool isShow)
        {
        }

        internal static void MoveObjTo(EntityInfo obj, Vector3 position)
        {
            obj.GetMovementStateInfo().SetPosition(position);
        }

        internal static float GetObjFaceDir(EntityInfo obj)
        {
            return obj.GetMovementStateInfo().GetFaceDir();
        }

        internal static Vector3 GetGroundPos(Vector3 pos)
        {
            Vector3 sourcePos = pos;
            sourcePos.Y = 0;
            return sourcePos;
        }

        internal static bool FloatEqual(float a, float b)
        {
            if (Math.Abs(a - b) <= 0.0001) {
                return true;
            }
            return false;
        }

        internal static float GetHeightWithGround(EntityInfo obj)
        {
            return GetHeightWithGround(obj.GetMovementStateInfo().GetPosition3D());
        }

        internal static float GetHeightWithGround(Vector3 pos)
        {
            return pos.Y;
        }

        //-----------------------------------------------------------------------------------------------------------
        internal static void CalcImpactConfig(int emitImpact, int hitImpact, SkillInstance instance, TableConfig.Skill cfg, out Dictionary<string, object> result)
        {
            var variables = instance.Variables;
            result = new Dictionary<string, object>(variables);
            if (null != instance.EmitSkillInstances) {
                if (emitImpact <= 0)
                    emitImpact = SkillInstance.c_FirstInnerEmitSkillId;
                TableConfig.Skill impactCfg = TableConfig.SkillProvider.Instance.GetSkill(emitImpact);
                if (null != impactCfg) {
                    if (impactCfg.type == (int)SkillOrImpactType.Buff) {
                        emitImpact = SkillInstance.c_FirstInnerEmitSkillId;
                    }
                }
                SkillInstance val;
                if (instance.EmitSkillInstances.TryGetValue(emitImpact, out val)) {
                    result["emitskill"] = val;
                }
            }
            if (null != instance.HitSkillInstances) {
                if (hitImpact <= 0)
                    hitImpact = SkillInstance.c_FirstInnerHitSkillId;
                TableConfig.Skill impactCfg = TableConfig.SkillProvider.Instance.GetSkill(hitImpact); ;
                if (null != impactCfg) {
                    if (impactCfg.type == (int)SkillOrImpactType.Buff) {
                        hitImpact = SkillInstance.c_FirstInnerHitSkillId;
                    }
                }
                SkillInstance val;
                if (instance.HitSkillInstances.TryGetValue(hitImpact, out val)) {
                    result["hitskill"] = val;
                }
            }
            string hitEffect = SkillParamUtility.RefixResourceVariable("hitEffect", instance, cfg.resources);
            if (!string.IsNullOrEmpty(hitEffect)) {
                result["hitEffect"] = hitEffect;
            }
            string hitEffect1 = SkillParamUtility.RefixResourceVariable("hitEffect1", instance, cfg.resources);
            if (!string.IsNullOrEmpty(hitEffect1)) {
                result["hitEffect1"] = hitEffect1;
            }
            string hitEffect2 = SkillParamUtility.RefixResourceVariable("hitEffect2", instance, cfg.resources);
            if (!string.IsNullOrEmpty(hitEffect2)) {
                result["hitEffect2"] = hitEffect2;
            }
            string hitEffect3 = SkillParamUtility.RefixResourceVariable("hitEffect3", instance, cfg.resources);
            if (!string.IsNullOrEmpty(hitEffect3)) {
                result["hitEffect3"] = hitEffect3;
            }
            string emitEffect = SkillParamUtility.RefixResourceVariable("emitEffect", instance, cfg.resources);
            if (!string.IsNullOrEmpty(emitEffect)) {
                result["emitEffect"] = emitEffect;
            }
            string emitEffect1 = SkillParamUtility.RefixResourceVariable("emitEffect1", instance, cfg.resources);
            if (!string.IsNullOrEmpty(emitEffect1)) {
                result["emitEffect1"] = emitEffect1;
            }
            string emitEffect2 = SkillParamUtility.RefixResourceVariable("emitEffect2", instance, cfg.resources);
            if (!string.IsNullOrEmpty(emitEffect2)) {
                result["emitEffect2"] = emitEffect2;
            }
            string emitEffect3 = SkillParamUtility.RefixResourceVariable("emitEffect3", instance, cfg.resources);
            if (!string.IsNullOrEmpty(emitEffect3)) {
                result["emitEffect3"] = emitEffect3;
            }
            string targetEffect = SkillParamUtility.RefixResourceVariable("targetEffect", instance, cfg.resources);
            if (!string.IsNullOrEmpty(targetEffect)) {
                result["targetEffect"] = targetEffect;
            }
            string targetEffect1 = SkillParamUtility.RefixResourceVariable("targetEffect1", instance, cfg.resources);
            if (!string.IsNullOrEmpty(targetEffect1)) {
                result["targetEffect1"] = targetEffect1;
            }
            string targetEffect2 = SkillParamUtility.RefixResourceVariable("targetEffect2", instance, cfg.resources);
            if (!string.IsNullOrEmpty(targetEffect2)) {
                result["targetEffect2"] = targetEffect2;
            }
            string targetEffect3 = SkillParamUtility.RefixResourceVariable("targetEffect3", instance, cfg.resources);
            if (!string.IsNullOrEmpty(targetEffect3)) {
                result["targetEffect3"] = targetEffect3;
            }
            string selfEffect = SkillParamUtility.RefixResourceVariable("selfEffect", instance, cfg.resources);
            if (!string.IsNullOrEmpty(selfEffect)) {
                result["selfEffect"] = selfEffect;
            }
            string selfEffect1 = SkillParamUtility.RefixResourceVariable("selfEffect1", instance, cfg.resources);
            if (!string.IsNullOrEmpty(selfEffect1)) {
                result["selfEffect1"] = selfEffect1;
            }
            string selfEffect2 = SkillParamUtility.RefixResourceVariable("selfEffect2", instance, cfg.resources);
            if (!string.IsNullOrEmpty(selfEffect2)) {
                result["selfEffect2"] = selfEffect2;
            }
            string selfEffect3 = SkillParamUtility.RefixResourceVariable("selfEffect3", instance, cfg.resources);
            if (!string.IsNullOrEmpty(selfEffect3)) {
                result["selfEffect3"] = selfEffect3;
            }
        }
        internal static int GetSkillImpactId(Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            int impactId = cfg.impact;
            if (impactId <= 0) {
                object idObj;
                if (variables.TryGetValue("impact", out idObj)) {
                    impactId = (int)idObj;
                }
            }
            return impactId;
        }
        
        internal static void AoeQuery(GfxSkillSenderInfo senderObj, SkillInstance instance, int senderId, int targetType, Vector3 relativeCenter, bool relativeToTarget, MyFunc<float, int, bool> callback)
        {
            Scene scene = senderObj.Scene;
            EntityInfo srcObj = senderObj.GfxObj;
            EntityInfo targetObj = senderObj.TargetGfxObj;

            float radian;
            Vector3 center;
            Vector3 srcPos = srcObj.GetMovementStateInfo().GetPosition3D();
            if (null != targetObj && relativeToTarget) {
                Vector3 targetPos = targetObj.GetMovementStateInfo().GetPosition3D();
                radian = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.X, srcPos.Z), new ScriptRuntime.Vector2(targetPos.X, targetPos.Z));
                ScriptRuntime.Vector2 newOffset = Geometry.GetRotate(new ScriptRuntime.Vector2(relativeCenter.X, relativeCenter.Z), radian);
                center = targetPos + new Vector3(newOffset.X, relativeCenter.Y, newOffset.Y);
            } else {
                radian = srcObj.GetMovementStateInfo().GetFaceDir(); 
                ScriptRuntime.Vector2 newOffset = Geometry.GetRotate(new ScriptRuntime.Vector2(relativeCenter.X, relativeCenter.Z), radian);
                center = srcPos + new Vector3(newOffset.X, relativeCenter.Y, newOffset.Y);
            }

            int aoeType = 0;
            float range = 0;
            float angleOrLength = 0;
            TableConfig.Skill cfg = senderObj.ConfigData;
            if (null != cfg) {
                aoeType = cfg.aoeType;
                range = cfg.aoeSize;
                angleOrLength = cfg.aoeAngleOrLength;
            }
            if (aoeType == (int)SkillAoeType.Circle || aoeType == (int)SkillAoeType.Sector) {
                angleOrLength = Geometry.DegreeToRadian(angleOrLength);
                scene.KdTree.Query(center.X, center.Y, center.Z, range, (float distSqr, KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == scene.EntityController.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == scene.EntityController.GetRelation(senderId, targetId)) {
                        bool isMatch = false;
                        if (aoeType == (int)SkillAoeType.Circle) {
                            isMatch = true;
                        } else {
                            ScriptRuntime.Vector2 u = Geometry.GetRotate(new ScriptRuntime.Vector2(0, 1), radian);
                            isMatch = Geometry.IsSectorDiskIntersect(new ScriptRuntime.Vector2(center.X, center.Z), u, angleOrLength / 2, range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        }
                        if (isMatch) {
                            if (!callback(distSqr, kdTreeObj.Object.GetId())) {
                                return false;
                            }
                        }
                    }
                    return true;
                });
            } else {
                ScriptRuntime.Vector2 angleu = Geometry.GetRotate(new ScriptRuntime.Vector2(0, angleOrLength), radian);
                ScriptRuntime.Vector2 c = new ScriptRuntime.Vector2(center.X, center.Z) + angleu / 2;
                scene.KdTree.Query(c.X, 0, c.Y, range + angleOrLength / 2, (float distSqr, GameFramework.KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == scene.EntityController.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == scene.EntityController.GetRelation(senderId, targetId)) {
                        bool isMatch = false;
                        if (aoeType == (int)SkillAoeType.Capsule) {
                            isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(center.X, center.Z), angleu, range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        } else {
                            ScriptRuntime.Vector2 half = new ScriptRuntime.Vector2(range / 2, angleOrLength / 2);
                            isMatch = Geometry.IsObbDiskIntersect(c, half, radian, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        }
                        if (isMatch) {
                            if (!callback(distSqr, kdTreeObj.Object.GetId())) {
                                return false;
                            }
                        }
                    }
                    return true;
                });
            }
        }
    }
}
