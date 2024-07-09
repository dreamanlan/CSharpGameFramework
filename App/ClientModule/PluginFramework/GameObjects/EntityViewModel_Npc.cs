﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace ScriptableFramework
{
    public partial class EntityViewModel
    {
        public const string c_StandAnim = "Idle";
        public const string c_MoveAnim = "Run";
        public const float c_CrossFadeTime = 0.1f;
        public EntityInfo Entity
        {
            get { return m_Entity; }
        }
        public NavMeshAgent Agent
        {
            get { return m_Agent; }
        }
        public Animator Animator
        {
            get { return m_Animator; }
        }
        public void Create(EntityInfo entity)
        {
            if (null != entity) {
                m_Entity = entity;
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                ScriptRuntime.Vector3 pos = msi.GetPosition3D();
                float dir = msi.GetFaceDir();
                CreateActor(m_Entity.GetId(), m_Entity.GetModel(), pos.X, pos.Y, pos.Z, dir, m_Entity.Scale, m_Entity.GetRadius(), entity.ActualProperty.GetFloat(CharacterPropertyEnum.x2011_最终速度));
                if (null != Actor) {
                    m_Agent = Actor.GetComponent<NavMeshAgent>();
                    if (m_Agent == null) {
                        m_Agent = Actor.AddComponent<NavMeshAgent>();
                        m_Agent.angularSpeed = c_AngularSpeed;
                        m_Agent.acceleration = c_Acceleration;
                        m_Agent.radius = entity.GetRadius();
                        m_Agent.speed = entity.ActualProperty.GetFloat(CharacterPropertyEnum.x2011_最终速度);
                        m_Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    }
                    m_Animator = Actor.GetComponentInChildren<Animator>();
                    EntityDrawGizmos gizmos = Actor.GetComponent<EntityDrawGizmos>();
                    if (null == gizmos) {
                        gizmos = Actor.AddComponent<EntityDrawGizmos>();
                        gizmos.npcInfo = m_Entity;
                    } else {
                        gizmos.npcInfo = m_Entity;
                    }
                    SetMoveAgentEnable(true);
                    try{
                        if (null != m_Agent) {
                            m_Agent.ResetPath();
                        }
                    } catch {
                        m_Agent.enabled = true;
                    }
                }
            }
        }

        public void Destroy()
        {
            DestroyActor();
            m_Entity = null;
        }

        public void Update()
        {
            UpdateSpatial();
            UpdateEdgeColor();
            SyncMovement();
        }
        public void SyncSpatial()
        {
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                UnityEngine.Vector3 v3 = Actor.transform.position;
                msi.SetPosition(v3.x, v3.y, v3.z);
                float dir = Utility.DegreeToRadian(Actor.transform.localEulerAngles.y);
                msi.SetFaceDir(dir);

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

        public void SyncFaceDir()
        {
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                float dir = Geometry.DegreeToRadian(Actor.transform.localEulerAngles.y);
                msi.SetFaceDir(dir);
            }
        }

        public void SyncPosition()
        {
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                UnityEngine.Vector3 v3 = Actor.transform.position;
                msi.SetPosition(v3.x, v3.y, v3.z);
            }
        }
        
        private void SyncMovement()
        {
            if (null != m_Entity && null != m_Actor) {
                float curTime = Time.time;
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                if (msi.IsMoving) {
                    if (m_LastSyncTime + c_SyncInterval <= curTime) {
                        m_LastSyncTime = curTime;
                        if (m_Entity.IsServerEntity) {
                            Transform t = m_Actor.transform;
                            Vector3 pos = t.TransformPoint(0, 0, m_Entity.Speed * c_SyncInterval);
                            Network.NetworkSystem.Instance.SyncPlayerMoveToPos(new ScriptRuntime.Vector3(pos.x, pos.y, pos.z));
                        }
                    }

                    if (GlobalVariables.Instance.IsDebug) {
                        Utility.EventSystem.Publish("ui_actor_name", "ui", m_Entity.GetId(), string.Format("{0}({1}):c({2},{3})", m_Entity.GetName(), m_Entity.GetId(), msi.PositionX, msi.PositionZ));
                    }

                    if (m_Entity.GetId() != PluginFramework.Instance.LeaderId) {
                        if (Geometry.DistanceSquare(msi.GetPosition3D(), msi.TargetPosition) > 0.625f) {
                            MoveTo(msi.TargetPosition.X, msi.TargetPosition.Y, msi.TargetPosition.Z);
                        } else {
                            msi.IsMoving = false;
                            StopMove();
                        }
                    }
                } else {
                    if (GlobalVariables.Instance.IsDebug) {
                        Utility.EventSystem.Publish("ui_actor_name", "ui", m_Entity.GetId(), string.Format("{0}({1}):c({2},{3})", m_Entity.GetName(), m_Entity.GetId(), msi.PositionX, msi.PositionZ));
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
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                if (Math.Abs(msi.GetWantedFaceDir() - msi.GetFaceDir()) > Geometry.c_FloatPrecision) {
                    float degree = Geometry.RadianToDegree(msi.GetWantedFaceDir());
                    Quaternion targetQ = Quaternion.Euler(0, degree, 0);
                    Actor.transform.localRotation = Quaternion.RotateTowards(Actor.transform.localRotation,
                        targetQ, c_TurnRoundSpeed * Time.deltaTime);
                    float dir = Geometry.DegreeToRadian(Actor.transform.localEulerAngles.y);
                    msi.SetFaceDir(dir, false);
                } else {
                    SyncFaceDir();
                }
            }
            SyncPosition();
        }

        public void Death()
        {
            SetMoveAgentEnable(false);
        }

        /// <summary>
        /// Whether to use NavAgent
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
            MovementStateInfo msi = m_Entity.GetMovementStateInfo();
            msi.IsMoving = true;
            msi.TargetPosition = new ScriptRuntime.Vector3(x, y, z);
            if (null != Agent && Agent.enabled) {
                try {
                    Agent.SetDestination(new Vector3(x, y, z));
                    Agent.isStopped = false;
                } catch {
                    m_Agent.enabled = true;

                    LogSystem.Error("MoveTo({0}, {1}, {2}) agent {3}({4}) failed.", x, y, z, m_ObjId, m_Entity.GetUnitId());
                }
            }
            if (null != Animator) {
                if (ObjId == PluginFramework.Instance.LeaderId) {
                    //Animator.CrossFade(c_MoveAnim, c_CrossFadeTime);
                    Animator.Play(c_MoveAnim);
                } else {
                    Animator.Play(c_MoveAnim);
                }
            }
        }

        public void StopMove()
        {
            MovementStateInfo msi = m_Entity.GetMovementStateInfo();
            msi.IsMoving = false;
            if (null != Agent && Agent.enabled) {
                try {
                    m_Agent.isStopped = true;
                } catch {
                    m_Agent.enabled = true;
                }
            }
            if (null != Animator) {
                if (ObjId == PluginFramework.Instance.LeaderId) {
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
                if (ObjId == PluginFramework.Instance.LeaderId) {
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

        private float m_LastSyncTime = 0;

        private const float c_SyncInterval = 0.5f;
        private const float c_StopDistSqr = 0.25f;
        private const float c_AngularSpeed = 3600;
        private const float c_Acceleration = 64;
        private const float c_TurnRoundSpeed = 360;
    }
}
