using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    internal partial class EntityViewModel
    {
        public const string c_StandAnim = "stand";
        public const string c_MoveAnim = "move";
        public const float c_CrossFadeTime = 0.1f;
        internal EntityInfo Entity
        {
            get { return m_Entity; }
        }
        internal NavMeshAgent Agent
        {
            get { return m_Agent; }
        }
        internal Animator Animator
        {
            get { return m_Animator; }
        }
        internal void Create(EntityInfo entity)
        {
            if (null != entity) {
                m_Entity = entity;
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                ScriptRuntime.Vector3 pos = msi.GetPosition3D();
                float dir = msi.GetFaceDir();
                CreateActor(m_Entity.GetId(), m_Entity.GetModel(), pos.X, pos.Y, pos.Z, dir, m_Entity.Scale, m_Entity.GetRadius(), m_Entity.GetActualProperty().MoveSpeed);
                if (null != Actor) {
                    m_Agent = Actor.GetComponent<NavMeshAgent>();
                    if (m_Agent == null) {
                        m_Agent = Actor.AddComponent<NavMeshAgent>();
                        m_Agent.angularSpeed = c_AngularSpeed;
                        m_Agent.acceleration = c_Acceleration;
                        m_Agent.radius = entity.GetRadius();
                        m_Agent.speed = entity.GetActualProperty().MoveSpeed;
                        m_Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                        m_Agent.ResetPath();
                    }
                    m_Animator = Actor.GetComponent<Animator>();
                    EntityDrawGizmos gizmos = Actor.GetComponent<EntityDrawGizmos>();
                    if (null == gizmos) {
                        gizmos = Actor.AddComponent<EntityDrawGizmos>();
                        gizmos.npcInfo = m_Entity;
                    } else {
                        gizmos.npcInfo = m_Entity;
                    }
                    SetMoveAgentEnable(true);
                }
            }
        }

        internal void Destroy()
        {
            DestroyActor();
            m_Entity = null;
        }

        internal void Update()
        {
            UpdateSpatial();
            UpdateEdgeColor();
        }
        internal void SyncSpatial()
        {
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                UnityEngine.Vector3 v3 = Actor.transform.position;
                msi.SetPosition(v3.x, v3.y, v3.z);
                float dir = Utility.DegreeToRadian(Actor.transform.localEulerAngles.y);
                msi.SetFaceDir(dir);
                msi.SetMoveDir(dir);

                if (msi.IsMoving) {
                    Vector3 tpos = new Vector3(msi.TargetPosition.X, msi.TargetPosition.Y, msi.TargetPosition.Z);
                    if ((tpos - v3).sqrMagnitude < c_StopDistSqr) {
                        msi.IsMoving = false;
                        if (null != Animator) {
                            //Animator.CrossFade(c_StandAnim, c_CrossFadeTime);
                            Animator.Play(c_StandAnim);
                            msi.IsMoving = false;
                        }
                    }
                }
            }
        }

        private bool UpdateVisible(bool visible)
        {
            SetVisible(visible);
            return visible;
        }

        private void UpdateSpatial()
        {
            SyncSpatial();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public void Death()
        {
            SetMoveAgentEnable(false);
        }

        /// <summary>
        /// 是否使用NavAgent
        /// </summary>
        /// <param name="enable"></param>
        public void SetMoveAgentEnable(bool enable)
        {
            if (null != Agent) {
                Agent.enabled = enable;
            }
        }

        public void MoveTo(float x, float y, float z)
        {
            if (null != Agent) {
                Agent.SetDestination(new Vector3(x, y, z));
                Agent.Resume();
            }
            if (null != Animator) {
                if (ObjId == ClientModule.Instance.LeaderID) {
                    //Animator.CrossFade(c_MoveAnim, c_CrossFadeTime);
                    Animator.Play(c_MoveAnim);
                } else {
                    Animator.Play(c_MoveAnim);
                }
            }
        }

        public void StopMove()
        {
            if (null != Agent) {
                Agent.Stop();
            }
            if (null != Animator) {
                if (ObjId == ClientModule.Instance.LeaderID) {
                    //Animator.CrossFade(c_StandAnim, c_CrossFadeTime);
                    Animator.Play(c_StandAnim);
                } else {
                    Animator.Play(c_StandAnim);
                }
            }
        }

        public void PlayAnimation(string anim)
        {
            if (null != Animator) {
                if (ObjId == ClientModule.Instance.LeaderID) {
                    //Animator.CrossFade(anim, c_CrossFadeTime);
                    Animator.Play(anim);
                } else {
                    Animator.Play(anim);
                }
            }
        }

        private EntityInfo m_Entity = null;
        private NavMeshAgent m_Agent = null;
        private Animator m_Animator = null;

        private const float c_StopDistSqr = 0.25f;
        private const float c_AngularSpeed = 3600;
        private const float c_Acceleration = 64;
    }
}
