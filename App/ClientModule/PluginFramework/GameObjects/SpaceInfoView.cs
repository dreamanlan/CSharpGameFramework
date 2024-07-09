using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace ScriptableFramework
{
    internal class SpaceInfoView
    {
        internal int ObjId
        {
            get { return m_ObjId; }
        }
        internal bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = value;
                SetVisible(value);
            }
        }
        internal bool NeedDestroy
        {
            get { return m_NeedDestroy; }
            set { m_NeedDestroy = value; }
        }
        internal void Create(int objId, bool isPlayer)
        {
            CreateActor(objId, isPlayer ? "BlueCylinder" : "RedCylinder");
        }
        internal void Update(float x, float y, float z, float dir)
        {
            if (null != m_Actor) {
                Vector3 pt = new Vector3(x, y, z);
                if (null != m_NavMeshAgent) {
                    m_NavMeshAgent.Move(pt - m_Actor.transform.position);
                } else {
                    GameObject obj = EntityViewModelManager.Instance.GetGameObject(m_ObjId);
                    if (null != obj) {
                        pt.y = obj.transform.position.y;
                    }
                    m_Actor.transform.position = pt;
                    m_Actor.transform.localRotation = Quaternion.Euler(0, Utility.RadianToDegree(dir), 0);
                }
            }
        }
        internal void Destroy()
        {
            DestroyActor();
        }
        
        private void SetVisible(bool visible)
        {
            if (null != m_Actor) {
                ResourceSystem.Instance.SetVisible(m_Actor, visible, null);
            }
        }
        private void CreateActor(int objId, string model)
        {
            m_ObjId = objId;
            m_Actor = ResourceSystem.Instance.NewObject(model) as GameObject;
            if (null != m_Actor) {
                m_NavMeshAgent = m_Actor.GetComponent<NavMeshAgent>();
                if (null != m_NavMeshAgent) {
                    m_NavMeshAgent = m_Actor.AddComponent<NavMeshAgent>();
                }
                if (null != m_NavMeshAgent) {
                    m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                }
            }
        }
        private void DestroyActor()
        {
            if (null != m_Actor) {
                ResourceSystem.Instance.RecycleObject(m_Actor);
                m_Actor = null;
                m_NavMeshAgent = null;
            }
        }

        private int m_ObjId = 0;
        private bool m_Visible = true;
        private bool m_NeedDestroy = false;
        private GameObject m_Actor = null;
        private NavMeshAgent m_NavMeshAgent = null;
    }
}
