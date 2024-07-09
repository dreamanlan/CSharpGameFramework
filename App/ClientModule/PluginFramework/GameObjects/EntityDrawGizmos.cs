using System;
using UnityEngine;

namespace ScriptableFramework
{
    class EntityDrawGizmos : MonoBehaviour
    {
        public EntityInfo npcInfo =  null;

#if UNITY_EDITOR
        /// <summary>
        /// Viewable range
        /// </summary>
        public Color viewRangeColor = Color.green;

        /// <summary>
        /// Skill range
        /// </summary>
        public Color skillRangeColor = Color.red;

        /// <summary>
        /// Raises the draw gizmos selected event.
        /// </summary>
        protected virtual void OnDrawGizmosSelected() 
		{
            if (npcInfo == null)
                return;
            UnityEditor.Handles.color = viewRangeColor;
            UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up,npcInfo.ViewRange);

            SkillInfo skillInfo = npcInfo.GetSkillStateInfo().GetCurSkillInfo();
            if (skillInfo != null)
            {
                UnityEditor.Handles.color = skillRangeColor;
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, skillInfo.distance);

                TableConfig.Skill cfg = skillInfo.ConfigData;
                if(cfg.aoeType==(int)SkillAoeType.Circle) {
                    UnityEditor.Handles.color = Color.black;
                    UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up,cfg.aoeSize);
                } else if (cfg.aoeType == (int)SkillAoeType.Sector) {
                    UnityEditor.Handles.color = Color.black;
                    float radian = Utility.DegreeToRadian(transform.localRotation.eulerAngles.y);
                    ScriptRuntime.Vector2 dir = Geometry.GetRotate(new ScriptRuntime.Vector2(0, 1), radian - cfg.aoeAngleOrLength / 2);
                    UnityEditor.Handles.DrawWireArc(transform.position,Vector3.up,new Vector3(dir.X,0,dir.Y),cfg.aoeAngleOrLength,cfg.aoeSize);
                } else {
                    float radian = Utility.DegreeToRadian(transform.localRotation.eulerAngles.y);
                    ScriptRuntime.Vector2 dir = Geometry.GetRotate(new ScriptRuntime.Vector2(0, 1), radian) * cfg.aoeAngleOrLength;
                    Vector3 pos1 = transform.position;
                    Vector3 pos2 = pos1 + new Vector3(dir.X, 0, dir.Y);
                    UnityEditor.Handles.color = Color.black;
                    UnityEditor.Handles.DrawLine(pos1, pos2);
                }
            }
        }
#endif
    }
}
