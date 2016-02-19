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
            if (dir.sqrMagnitude > Geometry.c_FloatPrecision) {
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, Quaternion.LookRotation(target - obj.transform.position), rotateDegree);
                EntityController.Instance.SyncFaceDir(obj);
            }
        }
        public static void Lookat(GameObject obj, Vector3 target)
        {
            if (!EntityController.Instance.IsRotatableEntity(obj))
                return;
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
            ClientModule.Instance.KdTree.Query(center.x, center.y, center.z, radius, (float distSqr, KdTreeObject kdTreeObj) => {
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

        //-----------------------------------------------------------------------------------------------------------

        public static void CalcHitConfig(Dictionary<string, object> variables, TableConfig.Skill cfg, out string hitEffect, out string hitEffectBone, out int hitEffectStartTime, out int hitEffectDeleteTime, out string hitAnim, out int hitAnimTime)
        {
            hitEffect = RefixEffectByConfig("hitEffect", variables, cfg);
            hitEffectBone = RefixBoneByConfig("hitEffectBone", variables, cfg);
            hitEffectStartTime = RefixEffectStartTimeByConfig("hitEffectStartTime", variables, cfg);
            hitEffectDeleteTime = RefixEffectDeleteTimeByConfig("hitEffectDeleteTime", variables, cfg);
            hitAnim = RefixAnimByConfig("hitAnim", variables, cfg);
            hitAnimTime = RefixAnimTimeByConfig("hitAnimTime", variables, cfg);
        }

        public static string RefixEffectByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            string ret;
            if (cfg.resources.TryGetValue(key, out ret)) {
                return ret;
            }
            if (key.IndexOf("/") < 0)
                return string.Empty;
            return key;
        }

        public static string RefixBoneByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            return key;
        }
        public static int RefixEffectStartTimeByConfig(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixEffectStartTimeByConfig("hitEffectStartTime", variables, cfg);
            else
                return timeTag;
        }        
        public static int RefixEffectStartTimeByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return (int)val;
            }
            return 0;
        }
        public static int RefixEffectDeleteTimeByConfig(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixEffectDeleteTimeByConfig("hitEffectDeleteTime", variables, cfg);
            else
                return timeTag;
        }
        public static int RefixEffectDeleteTimeByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return (int)val;
            }
            return 0;
        }
        public static string RefixAnimByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return val.ToString();
            }
            return key;
        }
        public static int RefixAnimTimeByConfig(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            if (timeTag < 0)
                return RefixEffectDeleteTimeByConfig("hitAnimTime", variables, cfg);
            else
                return timeTag;
        }
        public static int RefixAnimTimeByConfig(string key, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            object val;
            if (variables.TryGetValue(key, out val)) {
                return (int)val;
            }
            return 0;
        }
        public static int RefixStartTimeByConfig(int timeTag, Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            int index = -timeTag - 1;
            switch (index) {
                case 0:
                    return RefixAnimTimeByConfig("hitAnimTime", variables, cfg);
                case 1:
                    return RefixEffectStartTimeByConfig("hitEffectStartTime", variables, cfg);
                default:
                    return timeTag;
            }
        }

        public static void AoeQuery(GfxSkillSenderInfo senderObj, SkillInstance instance, int senderId, int targetType, Vector3 relativeCenter, bool relativeToTarget, MyFunc<float, int, bool> callback)
        {
            GameObject srcObj = senderObj.GfxObj;
            GameObject targetObj = senderObj.TargetGfxObj;

            float radian;
            Vector3 center;
            if (null != targetObj && relativeToTarget) {
                Vector3 srcPos = srcObj.transform.position;
                Vector3 targetPos = targetObj.transform.position;
                radian = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                ScriptRuntime.Vector2 newOffset = Geometry.GetRotate(new ScriptRuntime.Vector2(relativeCenter.x, relativeCenter.z), radian);
                center = targetPos + new Vector3(newOffset.X, relativeCenter.y, newOffset.Y);
            } else {
                radian = Utility.DegreeToRadian(srcObj.transform.localRotation.eulerAngles.y);
                center = srcObj.transform.TransformPoint(relativeCenter);
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
                angleOrLength = Utility.DegreeToRadian(angleOrLength);
                ClientModule.Instance.KdTree.Query(center.x, center.y, center.z, range, (float distSqr, KdTreeObject kdTreeObj) => {
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
                GameFramework.ClientModule.Instance.KdTree.Query(c.X, 0, c.Y, range + angleOrLength / 2, (float distSqr, GameFramework.KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == EntityController.Instance.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == EntityController.Instance.GetRelation(senderId, targetId)) {
                        bool isMatch = false;
                        if (aoeType == (int)SkillAoeType.Capsule) {
                            isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(center.x, center.z), angleu, range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        } else {
                            isMatch = Geometry.IsObbDiskIntersect(new ScriptRuntime.Vector2(center.x, center.z), new ScriptRuntime.Vector2(range / 2, angleOrLength / 2), radian, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
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
