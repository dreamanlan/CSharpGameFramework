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

        internal static void CalcHitConfig(Dictionary<string, object> variables, TableConfig.Skill cfg, out string hitEffect, out string hitEffectBone, out int hitEffectStartTime, out int hitEffectDeleteTime, out string hitAnim, out int hitAnimTime)
        {
            hitEffect = RefixResourceByConfig("hitEffect", variables, cfg);
            hitEffectBone = RefixStringVariable("hitEffectBone", variables, cfg);
            hitEffectStartTime = RefixIntVariable("hitEffectStartTime", variables, cfg);
            hitEffectDeleteTime = RefixIntVariable("hitEffectDeleteTime", variables, cfg);
            hitAnim = RefixStringVariable("hitAnim", variables, cfg);
            hitAnimTime = RefixIntVariable("hitAnimTime", variables, cfg);
        }

        internal static string RefixResourceByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            string ret;
            if (cfg.resources.TryGetValue(key, out ret)) {
                return ret;
            }
            return key;
        }

        internal static string RefixStringVariable(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            return key;
        }
        internal static int RefixIntVariable(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return (int)val;
            }
            return 0;
        }
        internal static int RefixEffectStartTime(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixIntVariable("hitEffectStartTime", variables, cfg);
            else
                return timeTag;
        }        
        internal static int RefixEffectDeleteTime(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixIntVariable("hitEffectDeleteTime", variables, cfg);
            else
                return timeTag;
        }
        internal static int RefixAnimTime(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixIntVariable("hitAnimTime", variables, cfg);
            else
                return timeTag;
        }
        internal static int RefixStartTime(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            int index = -timeTag - 1;
            switch (index) {
                case 0:
                    return RefixIntVariable("hitAnimTime", variables, cfg);
                case 1:
                    return RefixIntVariable("hitEffectStartTime", variables, cfg);
                default:
                    return timeTag;
            }
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
                angleOrLength = Helper.DegreeToRadian(angleOrLength);
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
                            isMatch = Geometry.IsObbDiskIntersect(new ScriptRuntime.Vector2(center.X, center.Z), new ScriptRuntime.Vector2(range / 2, angleOrLength / 2), radian, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
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
