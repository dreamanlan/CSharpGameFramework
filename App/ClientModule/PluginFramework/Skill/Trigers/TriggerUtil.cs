using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    public class TriggerUtil
    {
        private static float s_RayCastMaxDistance = 50;
        private static int s_TerrainLayer = 1 << LayerMask.NameToLayer("Terrain");
        public static void Lookat(GameObject obj, Vector3 target, float rotateDegree)
        {
            if (!EntityController.Instance.IsRotatableEntity(obj))
                return;
            Vector3 dir = target - obj.transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > Geometry.c_FloatPrecision) {
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, Quaternion.LookRotation(dir), rotateDegree);
                EntityController.Instance.SyncFaceDir(obj);
            }
        }
        public static void Lookat(GameObject obj, Vector3 target)
        {
            if (!EntityController.Instance.IsRotatableEntity(obj))
                return;
            target.y = obj.transform.position.y;
            Vector3 dir = target - obj.transform.position;
            if (dir.sqrMagnitude > Geometry.c_FloatPrecision) {
                obj.transform.LookAt(target, Vector3.up);
                EntityController.Instance.SyncFaceDir(obj);
            }
        }
        public static Transform GetChildNodeByName(GameObject gameobj, string name)
        {
            if (gameobj == null) {
                return null;
            }
            if (string.IsNullOrEmpty(name)) {
                return null;
            }
            Transform[] ts = gameobj.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < ts.Length; i++) {
                if (ts[i].name == name) {
                    return ts[i];
                }
            }
            return null;
        }
        public static bool AttachNodeToNode(GameObject source,
                                         string sourcenode,
                                         GameObject target,
                                         string targetnode)
        {
            Transform source_child = GetChildNodeByName(source, sourcenode);
            Transform target_child = GetChildNodeByName(target, targetnode);
            if (source_child == null || target_child == null) {
                return false;
            }
            target.transform.parent = source_child;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localPosition = Vector3.zero;
            Vector3 ss = source_child.localScale;
            Vector3 scale = new Vector3(1 / ss.x, 1 / ss.y, 1 / ss.z);
            Vector3 relative_motion = (target_child.position - target.transform.position);
            target.transform.position -= relative_motion;
            //target.transform.localPosition = Vector3.Scale(target.transform.localPosition, scale);
            return true;
        }
        public static void MoveChildToNode(GameObject obj, string childname, string nodename)
        {
            Transform child = GetChildNodeByName(obj, childname);
            if (child == null) {
                GameFramework.LogSystem.Info("----not find child! {0} on {1}", childname, obj.name);
                return;
            }
            Transform togglenode = TriggerUtil.GetChildNodeByName(obj, nodename);
            if (togglenode == null) {
                GameFramework.LogSystem.Info("----not find node! {0} on {1}", nodename, obj.name);
                return;
            }
            child.parent = togglenode;
            child.localRotation = Quaternion.identity;
            child.localPosition = Vector3.zero;
        }
        public static GameObject DrawCircle(Vector3 center, float radius, Color color, float circle_step = 0.05f)
        {
            GameObject obj = new GameObject();
            LineRenderer linerender = obj.AddComponent<LineRenderer>();
            linerender.SetWidth(0.05f, 0.05f);
            Shader shader = Shader.Find("Particles/Additive");
            if (shader != null) {
                linerender.material = new Material(shader);
            }
            linerender.SetColors(color, color);
            float step_degree = Mathf.Atan(circle_step / 2) * 2;
            int count = (int)(2 * Mathf.PI / step_degree);
            linerender.SetVertexCount(count + 1);
            for (int i = 0; i < count + 1; i++) {
                float angle = 2 * Mathf.PI / count * i;
                Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                linerender.SetPosition(i, pos);
            }
            return obj;
        }
        public static List<GameObject> FindTargetInSector(Vector3 center,
                                                      float radius,
                                                      Vector3 direction,
                                                      Vector3 degreeCenter,
                                                      float degree)
        {
            List<GameObject> result = new List<GameObject>();
            PluginFramework.Instance.KdTree.QueryWithAction(center.x, center.y, center.z, radius, (float distSqr, KdTreeObject kdTreeObj) => {
                ScriptRuntime.Vector3 pos = kdTreeObj.Position;
                Vector3 targetDir = new Vector3(pos.X, pos.Y, pos.Z) - degreeCenter;
                targetDir.y = 0;
                if (Mathf.Abs(Vector3.Angle(targetDir, direction)) <= degree) {
                    GameObject obj = EntityController.Instance.GetGameObject(kdTreeObj.Object.GetId());
                    result.Add(obj);
                }
            });
            return result;
        }
        public static GameObject GetObjectByPriority(GameObject source, List<GameObject> list,
                                                     float distance_priority, float degree_priority,
                                                     float max_distance, float max_degree)
        {
            GameObject target = null;
            float max_score = -1;
            for (int i = 0; i < list.Count; i++) {
                float distance = (list[i].transform.position - source.transform.position).magnitude;
                float distance_score = 1 - distance / max_distance;
                Vector3 targetDir = list[i].transform.position - source.transform.position;
                float angle = Vector3.Angle(targetDir, source.transform.forward);
                float degree_score = 1 - angle / max_degree;
                float final_score = distance_score * distance_priority + degree_score * degree_priority;
                if (final_score > max_score) {
                    target = list[i];
                    max_score = final_score;
                }
            }
            return target;
        }
        public static List<GameObject> FiltByRelation(GameObject source, List<GameObject> list, GameFramework.CharacterRelation relation)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < list.Count; ++i) {
                GameObject obj = list[i];
                if (EntityController.Instance.GetRelation(source, obj) == relation && source != obj) {
                    result.Add(obj);
                }
            }
            return result;
        }
        public static float ConvertToSecond(long delta)
        {
            return delta / 1000000.0f;
        }
        public static void SetObjVisible(GameObject obj, bool isShow)
        {
            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++) {
                renders[i].enabled = isShow;
            }
        }
        public static void MoveObjTo(GameObject obj, Vector3 position)
        {
            EntityViewModel npcViewModel = (EntityViewModel)EntityController.Instance.GetEntityView(obj);
            if (npcViewModel != null) {
                NavMeshAgent agent = npcViewModel.Agent;
                if (null != agent && agent.enabled) {
                    agent.Move(position - obj.transform.position);
                } else {
                    obj.transform.position = position;
                }
            }
        }
        public static float GetObjFaceDir(GameObject obj)
        {
            return obj.transform.rotation.eulerAngles.y * UnityEngine.Mathf.PI / 180.0f;
        }
        public static Vector3 GetGroundPos(Vector3 pos)
        {
            Vector3 sourcePos = pos;
            RaycastHit hit;
            pos.y += 2;
            if (Physics.Raycast(pos, -Vector3.up, out hit, s_RayCastMaxDistance, s_TerrainLayer)) {
                sourcePos.y = hit.point.y;
            }
            return sourcePos;
        }
        public static bool FloatEqual(float a, float b)
        {
            if (Math.Abs(a - b) <= 0.0001) {
                return true;
            }
            return false;
        }
        public static float GetHeightWithGround(GameObject obj)
        {
            return GetHeightWithGround(obj.transform.position);
        }
        public static float GetHeightWithGround(Vector3 pos)
        {
            if (Terrain.activeTerrain != null) {
                return pos.y - Terrain.activeTerrain.SampleHeight(pos);
            } else {
                RaycastHit hit;
                Vector3 higher_pos = pos;
                higher_pos.y += 2;
                if (Physics.Raycast(higher_pos, -Vector3.up, out hit, s_RayCastMaxDistance, s_TerrainLayer)) {
                    return pos.y - hit.point.y;
                }
                return s_RayCastMaxDistance;
            }
        }
        public static bool GetSkillStartPosition(Vector3 srcPos, TableConfig.Skill cfg, SkillInstance instance, int srcId, int targetId, ref Vector3 targetPos)
        {
            ScriptRuntime.Vector3 pos;
            object posVal;
            if (instance.Variables.TryGetValue("skill_targetpos", out posVal)) {
                pos = (ScriptRuntime.Vector3)posVal;
                targetPos = new Vector3(pos.X, pos.Y, pos.Z);
                return true;
            }
            if (instance.Variables.TryGetValue("skill_homepos", out posVal)) {
                pos = (ScriptRuntime.Vector3)posVal;
                targetPos = new Vector3(pos.X, pos.Y, pos.Z);
                return true;
            }
            float dist = cfg.skillData.distance;
            if (dist <= Geometry.c_FloatPrecision) {
                object val;
                if (instance.Variables.TryGetValue("skill_distance", out val)) {
                    dist = (float)Convert.ChangeType(val, typeof(float));
                } else {
                    dist = EntityController.Instance.CalcSkillDistance(dist, srcId, targetId);
                }
            } else {
                dist = EntityController.Instance.CalcSkillDistance(dist, srcId, targetId);
            }
            if (dist <= Geometry.c_FloatPrecision) {
                dist = 0.1f;
            }
            float dir;
            if (EntityController.Instance.CalcPosAndDir(targetId, out pos, out dir)) {
                pos += Geometry.GetRotate(new ScriptRuntime.Vector3(0, 0, dist), dir);
                targetPos = new Vector3(pos.X, pos.Y, pos.Z);
                return true;
            }
            return false;
        }
        //-----------------------------------------------------------------------------------------------------------
        public static void CalcImpactConfig(int emitImpact, int hitImpact, SkillInstance instance, TableConfig.Skill cfg, out Dictionary<string, object> result)
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
        public static int GetSkillImpactId(Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            int impactId = 0;
            object idObj;
            if (variables.TryGetValue("impact", out idObj)) {
                impactId = (int)idObj;
            }
            return impactId;
        }
        
        public static void AoeQuery(GfxSkillSenderInfo senderObj, SkillInstance instance, int senderId, int targetType, Vector3 relativeCenter, bool relativeToTarget, MyFunc<float, int, bool> callback)
        {
            GameObject srcObj = senderObj.GfxObj;
            if (null != senderObj.TrackEffectObj)
                srcObj = senderObj.TrackEffectObj;
            GameObject targetObj = senderObj.TargetGfxObj;
            int aoeType = 0;
            float range = 0;
            float angleOrLength = 0;
            TableConfig.Skill cfg = senderObj.ConfigData;
            if (null != cfg) {
                aoeType = cfg.aoeType;
                range = cfg.aoeSize;
                angleOrLength = cfg.aoeAngleOrLength;
            }
            AoeQuery(srcObj, targetObj, aoeType, range, angleOrLength, instance, senderId, targetType, relativeCenter, relativeToTarget, callback);
        }
        public static void AoeQuery(GameObject srcObj, GameObject targetObj, int aoeType, float range, float angleOrLength, SkillInstance instance, int senderId, int targetType, Vector3 relativeCenter, bool relativeToTarget, MyFunc<float, int, bool> callback)
        {
            float radian;
            Vector3 center;
            if (null != targetObj && relativeToTarget) {
                Vector3 srcPos = srcObj.transform.position;
                Vector3 targetPos = targetObj.transform.position;
                radian = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                ScriptRuntime.Vector2 newOffset = Geometry.GetRotate(new ScriptRuntime.Vector2(relativeCenter.x, relativeCenter.z), radian);
                center = targetPos + new Vector3(newOffset.X, relativeCenter.y, newOffset.Y);
            } else {
                radian = Geometry.DegreeToRadian(srcObj.transform.localRotation.eulerAngles.y);
                center = srcObj.transform.TransformPoint(relativeCenter);
            }
            if (aoeType == (int)SkillAoeType.Circle || aoeType == (int)SkillAoeType.Sector) {
                angleOrLength = Geometry.DegreeToRadian(angleOrLength);
                PluginFramework.Instance.KdTree.QueryWithFunc(center.x, center.y, center.z, range, (float distSqr, KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == EntityController.Instance.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == EntityController.Instance.GetRelation(senderId, targetId)) {
                        bool isMatch = false;
                        if (aoeType == (int)SkillAoeType.Circle) {
                            isMatch = true;
                        } else {
                            ScriptRuntime.Vector2 u = Geometry.GetRotate(new ScriptRuntime.Vector2(0, 1), radian);
                            isMatch = Geometry.IsSectorDiskIntersect(new ScriptRuntime.Vector2(center.x, center.z), u, angleOrLength / 2, range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
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
                ScriptRuntime.Vector2 c = new ScriptRuntime.Vector2(center.x, center.z) + angleu / 2;
                GameFramework.PluginFramework.Instance.KdTree.QueryWithFunc(c.X, 0, c.Y, range + angleOrLength / 2, (float distSqr, GameFramework.KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == EntityController.Instance.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == EntityController.Instance.GetRelation(senderId, targetId)) {
                        bool isMatch = false;
                        if (aoeType == (int)SkillAoeType.Capsule) {
                            isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(center.x, center.z), angleu, range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
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
