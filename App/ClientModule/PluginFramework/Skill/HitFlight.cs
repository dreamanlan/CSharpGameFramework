using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace ScriptableFramework.Skill
{
    internal sealed class HitFlight
    {
        internal bool IsFinish
        {
            get { return m_State == State.Finish; }
        }
        internal void Trigger(GfxSkillSenderInfo sender, GameObject target, float time, float height, float qteStartTime, float qteDuration, float qteHeight, float qteButtonDuration, string upAnim, string downAnim, string falldownAnim, float animFadeTime)
        {
            float rate = 0.0f;
            if (m_CurTime > 0 && m_CurveTime > 0) {
                float curHeight = target.transform.position.y - m_BaseHeight;
                float curTime = m_CurveTime / 2 - (float)Math.Sqrt((m_CurveHeight - curHeight) * 2.0f / m_G);
                rate = curTime / m_CurveTime;
            } else {
                m_BaseHeight = target.transform.position.y;
            }

            m_Sender = sender;
            m_Target = target;
            m_CurveTime = time;
            m_CurveHeight = height;
            m_QteStartTime = qteStartTime;
            m_QteDuration = qteDuration;
            m_QteHeight = qteHeight;
            m_QteButtonDuration = qteButtonDuration;
            m_UpAnim = upAnim;
            m_DownAnim = downAnim;
            m_FalldownAnim = falldownAnim;
            m_AnimFadeTime = animFadeTime;

            m_Animator = target.GetComponentInChildren<Animator>();
            CalcYVelocityAndG();
            CalcHeightBeforeQteAndG();

            m_State = State.Up;
            m_CurTime = m_CurveTime * rate;
            m_CurQteTime = 0;
            m_CurHitTime = 0;

            m_Animator.speed = 1.0f;
            m_Animator.Play(m_UpAnim, -1, 0);
        }
        internal void Hit(float duration)
        {
            m_CurHitTime = 0;
            m_HitDuration = duration;
            m_State = State.Hit;
        }
        internal void Tick()
        {
            if (m_State != State.Finish) {
                var view = EntityController.Instance.GetEntityView(m_Target);
                if (null != view) {
                    view.Entity.CanDead = false;
                }
            }
            switch (m_State) {
                case State.Up:
                    TickUp();
                    break;
                case State.Down:
                    TickDown();
                    break;
                case State.Qte:
                    TickQte();
                    break;
                case State.Hit:
                    TickHit();
                    break;                    
            }
        }

        private void TickUp()
        {
            Flight();

            m_CurTime += Time.deltaTime;
            if (m_CurTime >= m_CurveTime / 2) {
                m_State = State.Down;
                m_Animator.speed = 1.0f;
                m_Animator.CrossFade(m_DownAnim, m_AnimFadeTime, -1, 0);
            }
        }
        private void TickDown()
        {
            Flight();

            m_CurTime += Time.deltaTime;
            if (m_CurTime >= m_QteStartTime && m_CurQteTime < m_QteDuration && m_QteDuration > 0) {
                m_State = State.Qte;
                m_CurQteTime = 0;
                Utility.SendMessage("StartScript", "ShowQTE", new object[] { m_Sender, m_QteButtonDuration.ToString() });
            } else if (m_CurTime >= m_CurveTime) {
                m_State = State.Finish;

                var view = EntityController.Instance.GetEntityView(m_Target);
                if (null != view) {
                    var entity = view.Entity;
                    if (!entity.IsDead()) {
                        m_Animator.speed = 1.0f;
                        m_Animator.CrossFade(m_FalldownAnim, m_AnimFadeTime, -1, 0);
                    }
                }
                
                Vector3 pos = m_Target.transform.position;
                pos.y = m_BaseHeight;
                m_Target.transform.position = pos;
            }
        }
        private void TickQte()
        {
            QteFlight();

            m_CurQteTime += Time.deltaTime;
            if (m_CurQteTime > m_QteDuration) {
                m_CurTime = CalcCurTimeAfterQTE();
                m_State = State.Down;
                m_Animator.speed = 1.0f;
                m_Animator.Play(m_DownAnim);
            }
        }
        private void TickHit()
        {
            m_CurHitTime += Time.deltaTime;
            if (m_CurHitTime > m_HitDuration) {
                if (m_CurTime >= m_QteStartTime && m_CurQteTime < m_QteDuration && m_QteDuration > 0) {
                    m_State = State.Qte;
                } else {
                    m_State = State.Down;
                }
                m_Animator.speed = 1.0f;
                m_Animator.CrossFade(m_DownAnim, m_AnimFadeTime, -1, 0);
            }
        }

        private void Flight()
        {
            float height = m_YVelocity * m_CurTime - m_G * m_CurTime * m_CurTime / 2;
            Vector3 pos = m_Target.transform.position;
            pos.y = m_BaseHeight + height;
            m_Target.transform.position = pos;
        }
        private void QteFlight()
        {
            float height = m_CurveHeight - m_HeightBeforeQte - m_QteG * m_CurQteTime * m_CurQteTime / 2;
            Vector3 pos = m_Target.transform.position;
            pos.y = m_BaseHeight + height;
            m_Target.transform.position = pos;
        }
        private void CalcYVelocityAndG()
        {
            float time_div = m_CurveTime;
            time_div /= 2;
            m_YVelocity = m_CurveHeight * 2.0f / time_div;
            m_G = m_CurveHeight * 2.0f / (time_div * time_div);
        }
        private void CalcHeightBeforeQteAndG()
        {
            float time = m_QteStartTime - m_CurveTime / 2.0f;
            if (time < 0.0f) {
                time = 0.0f;
            }
            m_HeightBeforeQte = time * time * m_G / 2.0f;

            float time_div = m_QteDuration;
            m_QteG = m_QteHeight * 2.0f / (time_div * time_div);
        }
        private float CalcCurTimeAfterQTE()
        {
            float halfTime = m_CurveTime / 2.0f;
            return halfTime + (float)Math.Sqrt((m_HeightBeforeQte + m_QteHeight) * 2.0f / m_G);
        }

        private enum State
        {
            Up = 0,
            Down,
            Qte,
            Hit,
            Finish,
        }

        private State m_State = State.Up;
        private float m_CurTime = 0;

        private GfxSkillSenderInfo m_Sender = null;
        private GameObject m_Target = null;
        private float m_CurveTime = 0;
        private float m_CurveHeight = 0;
        private float m_CurQteTime = 0;
        private float m_QteStartTime = 0;
        private float m_QteDuration = 0;
        private float m_QteHeight = 0;
        private float m_QteButtonDuration = 0;
        private float m_CurHitTime = 0;
        private float m_HitDuration = 0;
        private string m_UpAnim = string.Empty;
        private string m_DownAnim = string.Empty;
        private string m_FalldownAnim = string.Empty;
        private float m_AnimFadeTime = 0.2f;

        private Animator m_Animator = null;
        private float m_BaseHeight = 0;
        private float m_YVelocity = 0;
        private float m_G = 0;
        private float m_QteG = 0;
        private float m_HeightBeforeQte = 0;
    }
    internal sealed class HitFlightManager
    {
        internal int HitFlightCount
        {
            get { return m_HitFlights.Count; }
        }
        internal void Reset()
        {
            m_HitFlights.Clear();
        }
        internal void Trigger(GfxSkillSenderInfo sender, float time, float height, float qteStartTime, float qteDuration, float qteHeight, float qteButtonDuration, string upAnim, string downAnim, string falldownAnim, float animFadeTime)
        {
            int id, senderId;
            GameObject target;
            if (sender.ConfigData.type == (int)SkillOrImpactType.Skill) {
                senderId = sender.ObjId;
                id = sender.TargetObjId;
                target = sender.TargetGfxObj;
            } else {
                senderId = sender.TargetObjId;
                id = sender.ObjId;
                target = sender.GfxObj;
            }
            if (null != target) {
                EntityInfo senderObj = PluginFramework.Instance.GetEntityById(senderId);
                EntityInfo targetObj = PluginFramework.Instance.GetEntityById(id);
                HitFlight hitFlight;
                if (!m_HitFlights.TryGetValue(id, out hitFlight)) {
                    hitFlight = new HitFlight();
                    m_HitFlights.Add(id, hitFlight);
                    EntityController.Instance.AddState(id, "float");
                }
                hitFlight.Trigger(sender, target, time, height, qteStartTime, qteDuration, qteHeight, qteButtonDuration, upAnim, downAnim, falldownAnim, animFadeTime);                
            }
        }
        internal void Hit(GfxSkillSenderInfo sender, float duration)
        {
            int id;
            GameObject target;
            if (sender.ConfigData.type == (int)SkillOrImpactType.Skill) {
                id = sender.TargetObjId;
                target = sender.TargetGfxObj;
            } else {
                id = sender.ObjId;
                target = sender.GfxObj;
            }
            if (null != target) {
                HitFlight hitFlight;
                if (m_HitFlights.TryGetValue(id, out hitFlight)) {
                    hitFlight.Hit(duration);
                }
            }
        }
        internal void Tick()
        {
            var enumer = m_HitFlights.GetEnumerator();
            while (enumer.MoveNext()) {
                var pair = enumer.Current;
                pair.Value.Tick();
                if (pair.Value.IsFinish) {
                    m_WaitDeletes.Add(pair.Key);
                }
            }
            for (int i = 0; i < m_WaitDeletes.Count; ++i) {
                int id = m_WaitDeletes[i];
                EntityController.Instance.RemoveState(id, "float");
                m_HitFlights.Remove(id);
            }
            m_WaitDeletes.Clear();
        }

        private MyDictionary<int, HitFlight> m_HitFlights = new MyDictionary<int, HitFlight>();
        private List<int> m_WaitDeletes = new List<int>();

        internal static HitFlightManager Instance
        {
            get { return s_Instance; }
        }
        private static HitFlightManager s_Instance = new HitFlightManager();
    }
}
