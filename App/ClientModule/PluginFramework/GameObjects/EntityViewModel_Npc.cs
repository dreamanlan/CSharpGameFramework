using System;
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
                    }
                    // Apply outside the AddComponent branch so prefabs that
                    // already carry a NavMeshAgent are covered too.
                    // Experiment switch: false disables agent-to-agent separation
                    // (the "collision" that pushed player/mobs off their
                    // authoritative path). Set true to restore design doc 22.3.
                    m_Agent.obstacleAvoidanceType = c_EnableObstacleAvoidance
                        ? ObstacleAvoidanceType.MedQualityObstacleAvoidance
                        : ObstacleAvoidanceType.NoObstacleAvoidance;
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
            CalibrateAnimLock();
            RefreshLocomotion();
        }
        public void SyncSpatial()
        {
            if (null != m_Entity && null != Actor) {
                MovementStateInfo msi = m_Entity.GetMovementStateInfo();
                // Transform is in world space; MovementStateInfo/TargetPosition
                // are logical local coordinates. Convert back through the
                // view's captured reference frame before comparing distances.
                UnityEngine.Vector3 v3 = SceneOrigin.ToLocal(Actor.transform.position, m_Origin);
                msi.SetPosition(v3.x, v3.y, v3.z);
                float dir = Utility.DegreeToRadian(Actor.transform.localEulerAngles.y);
                msi.SetFaceDir(dir);

                if (msi.IsMoving) {
                    Vector3 tpos = new Vector3(msi.TargetPosition.X, msi.TargetPosition.Y, msi.TargetPosition.Z);
                    if ((tpos - v3).sqrMagnitude < c_StopDistSqr) {
                        msi.IsMoving = false;
                        PlayLocomotion(c_StandAnim);
                        msi.IsMoving = false;
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
                UnityEngine.Vector3 v3 = SceneOrigin.ToLocal(Actor.transform.position, m_Origin);
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
            if (null != Agent && Agent.enabled && Agent.isOnNavMesh) {
                try {
                    // NavMesh is baked in world space at the battlefield
                    // origin, so the logical target must be shifted too.
                    Agent.SetDestination(SceneOrigin.ToWorld(new Vector3(x, y, z), m_Origin));
                    Agent.isStopped = false;
                } catch {
                    m_Agent.enabled = true;

                    LogSystem.Error("MoveTo({0}, {1}, {2}) agent {3}({4}) failed.", x, y, z, m_ObjId, m_Entity.GetUnitId());
                }
            }
            PlayLocomotion(c_MoveAnim);
        }

        public void StopMove()
        {
            MovementStateInfo msi = m_Entity.GetMovementStateInfo();
            msi.IsMoving = false;
            if (null != Agent && Agent.enabled && Agent.isOnNavMesh) {
                try {
                    m_Agent.isStopped = true;
                } catch {
                    m_Agent.enabled = true;
                }
            }
            PlayLocomotion(c_StandAnim);
        }

        public void PlayAnimation(string anim)
        {
            if (null != Animator) {
                Animator.Play(anim);
                LockAnimation(c_DefaultAnimLockSeconds, anim);
            }
        }

        private EntityInfo m_Entity = null;
        private NavMeshAgent m_Agent = null;
        private Animator m_Animator = null;

        private float m_LastSyncTime = 0;
        // AI ticks call MoveTo every 100ms. A skill clip must be allowed to run
        // for its whole length: locomotion is suppressed while locked, then the
        // correct clip is restored once the lock expires. The caller supplies
        // the clip length instead of a fixed 0.6s, which used to cut Attack
        // (0.83s) short and left the actor looking like it was always running.
        private float m_AnimLockUntil = 0f;
        private bool m_HasAnimLock = false;
        private bool m_AnimLockNeedsCalibration = false;
        private int m_AnimLockStateHash = 0;
        private int m_LastLocomotionHash = 0;
        private bool CanPlayLocomotion { get { return Time.time >= m_AnimLockUntil; } }

        /// <summary>
        /// Hold locomotion off while a skill clip plays. State names do not match
        /// clip names (e.g. state "Attack" uses clip "Skill_01"), so the real
        /// length is calibrated from AnimatorStateInfo on the next frame.
        /// </summary>
        public void LockAnimation(float seconds, string anim = null)
        {
            float duration = Mathf.Max(0f, seconds);
            m_AnimLockUntil = Mathf.Max(m_AnimLockUntil, Time.time + duration);
            m_HasAnimLock = true;
            m_AnimLockStateHash = string.IsNullOrEmpty(anim)
                ? 0 : UnityEngine.Animator.StringToHash(anim);
            m_AnimLockNeedsCalibration = m_AnimLockStateHash != 0;
            // A skill clip is playing now, so no locomotion state is current.
            m_LastLocomotionHash = 0;
        }

        private void CalibrateAnimLock()
        {
            if (!m_AnimLockNeedsCalibration || !m_HasAnimLock) return;
            if (null == Animator || !Animator.isInitialized) return;
            AnimatorStateInfo st = Animator.GetCurrentAnimatorStateInfo(0);
            if (st.shortNameHash != m_AnimLockStateHash) return;
            m_AnimLockNeedsCalibration = false;
            float speed = Animator.speed > 0.01f ? Animator.speed : 1.0f;
            if (!float.IsInfinity(st.length) && st.length > 0.01f) {
                float progress = Mathf.Repeat(st.normalizedTime, 1.0f);
                float remaining = st.length * (1.0f - progress) / speed;
                m_AnimLockUntil = Mathf.Max(m_AnimLockUntil, Time.time + Mathf.Max(0f, remaining));
            }
        }

        private void PlayLocomotion(string anim)
        {
            if (null == Animator || !CanPlayLocomotion) return;
            int hash = UnityEngine.Animator.StringToHash(anim);
            // MoveTo is called every AI tick; re-playing the same state restarts
            // the clip from frame 0 and stomps any skill animation.
            if (m_LastLocomotionHash == hash && Animator.isInitialized &&
                Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash) return;
            m_LastLocomotionHash = hash;
            Animator.Play(anim);
        }

        private void RefreshLocomotion()
        {
            if (!m_HasAnimLock || !CanPlayLocomotion) return;
            m_HasAnimLock = false;
            if (null == m_Entity || null == Animator) return;
            // Restore from the live movement state, not from a stale intent:
            // the actor may have arrived while the skill clip was locked.
            MovementStateInfo msi = m_Entity.GetMovementStateInfo();
            PlayLocomotion(msi.IsMoving ? c_MoveAnim : c_StandAnim);
        }

        // Experiment: false = no NavMeshAgent separation between entities.
        // Restore to true if mobs stack into one blob again (design doc 22.3).
        private const bool c_EnableObstacleAvoidance = false;
        private const float c_DefaultAnimLockSeconds = 0.6f;
        private const float c_SyncInterval = 0.5f;
        private const float c_StopDistSqr = 0.25f;
        private const float c_AngularSpeed = 3600;
        private const float c_Acceleration = 64;
        private const float c_TurnRoundSpeed = 360;
    }
}
