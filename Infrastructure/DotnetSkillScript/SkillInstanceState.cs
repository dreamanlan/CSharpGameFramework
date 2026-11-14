using System.Collections.Generic;
using ScriptableFramework;

namespace SkillSystem
{
    internal sealed class SkillTriggerState
    {
        internal ISkillTriger Trigger
        {
            get { return m_Trigger; }
        }
        internal static SkillTriggerState Create(ISkillTriger config)
        {
            SkillTriggerState state = new SkillTriggerState();
            state.m_Shared = config as ISharedSkillTriger;
            if (null != state.m_Shared) {
                state.m_Trigger = config;
                state.m_Data = state.m_Shared.CreateRuntimeState();
                state.m_IsConfigShared = true;
            }
            else {
                state.m_Trigger = config.Clone();
            }
            return state;
        }
        internal static List<ISkillTriger> GetTriggerConfigs(List<SkillTriggerState> states)
        {
            List<ISkillTriger> configs = new List<ISkillTriger>(states.Count);
            for (int i = 0; i < states.Count; ++i) {
                configs.Add(states[i].Trigger);
            }
            return configs;
        }
        internal void Reset()
        {
            if (null != m_Shared) {
                m_Shared.ResetRuntimeState(m_Data);
            }
            else {
                m_Trigger.Reset();
            }
        }
        internal bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            if (null != m_Shared) {
                return m_Shared.ExecuteRuntime(m_Data, sender, instance, delta, curSectionTime);
            }
            return m_Trigger.Execute(sender, instance, delta, curSectionTime);
        }
        internal void VisitProperties(VisitPropertyDelegation callback)
        {
            if (m_IsConfigShared) {
                m_Trigger.VisitProperties((string group, string key, IProperty property) => {
                    callback(group, key, new SkillTriggerProperty(this, key));
                });
            }
            else {
                m_Trigger.VisitProperties(callback);
            }
        }

        private void DetachConfig()
        {
            if (m_IsConfigShared) {
                m_Trigger = m_Trigger.Clone();
                if (null == FindProperty(m_Trigger, "StartTime")) {
                    m_Trigger.InitProperties();
                }
                m_Shared = m_Trigger as ISharedSkillTriger;
                m_IsConfigShared = false;
                if (null == m_Shared) {
                    m_Data = null;
                }
            }
        }
        private static IProperty FindProperty(ISkillTriger trigger, string key)
        {
            IProperty ret = null;
            trigger.VisitProperties((string group, string propertyKey, IProperty property) => {
                if (propertyKey == key) {
                    ret = property;
                }
            });
            return ret;
        }

        private sealed class SkillTriggerProperty : IProperty
        {
            internal SkillTriggerProperty(SkillTriggerState owner, string key)
            {
                m_Owner = owner;
                m_Key = key;
            }
            public object Value
            {
                get {
                    IProperty property = FindProperty(m_Owner.Trigger, m_Key);
                    return null != property ? property.Value : null;
                }
                set {
                    m_Owner.DetachConfig();
                    IProperty property = FindProperty(m_Owner.Trigger, m_Key);
                    if (null != property) {
                        property.Value = value;
                    }
                }
            }

            private SkillTriggerState m_Owner;
            private string m_Key;
        }

        private ISkillTriger m_Trigger;
        private ISharedSkillTriger m_Shared;
        private object m_Data;
        private bool m_IsConfigShared;
    }
    internal sealed class SkillSectionState
    {
        internal long Duration
        {
            get { return m_Duration; }
            set { m_Duration = value; }
        }
        internal long CurTime
        {
            get { return m_CurTime; }
        }
        internal bool IsFinished
        {
            get { return m_IsFinished; }
        }
        internal SkillSectionState(long duration, List<ISkillTriger> initTriggers, List<ISkillTriger> loadedTriggers)
        {
            m_Duration = duration;
            for (int i = 0; i < initTriggers.Count; ++i) {
                m_InitTriggers.Add(SkillTriggerState.Create(initTriggers[i]));
            }
            for (int i = 0; i < loadedTriggers.Count; ++i) {
                m_LoadedTriggers.Add(SkillTriggerState.Create(loadedTriggers[i]));
            }
            m_AccessorHelper.AddProperty("Duration", () => { return m_Duration; }, (object val) => { m_Duration = (long)System.Convert.ChangeType(val, typeof(long)); });
            m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; });
        }
        internal SkillSectionState CreateDefinitionState()
        {
            return new SkillSectionState(m_Duration, SkillTriggerState.GetTriggerConfigs(m_InitTriggers), SkillTriggerState.GetTriggerConfigs(m_LoadedTriggers));
        }
        internal void VisitProperties(VisitPropertyDelegation callback)
        {
            m_AccessorHelper.VisitProperties(callback);
            for (int i = 0; i < m_LoadedTriggers.Count; ++i) {
                m_LoadedTriggers[i].VisitProperties(callback);
            }
        }
        internal void Reset()
        {
            for (int i = 0; i < m_Triggers.Count; ++i) {
                m_Triggers[i].Reset();
            }
            m_Triggers.Clear();
            m_CurTime = 0;
            m_IsFinished = true;
            m_IsInited = false;
        }
        internal void Prepare()
        {
            for (int i = 0; i < m_Triggers.Count; ++i) {
                m_Triggers[i].Reset();
            }
            m_Triggers.Clear();
            m_CurTime = 0;
            m_IsFinished = false;
            m_IsInited = false;
            for (int i = 0; i < m_LoadedTriggers.Count; ++i) {
                m_Triggers.Add(m_LoadedTriggers[i]);
            }
            Helper.BubbleSort(m_Triggers, (left, right) => {
                if (left.Trigger.StartTime > right.Trigger.StartTime) {
                    return -1;
                }
                else if (left.Trigger.StartTime == right.Trigger.StartTime) {
                    return 0;
                }
                else {
                    return 1;
                }
            });
        }
        internal void Tick(object sender, SkillInstance instance, long delta)
        {
            if (m_IsFinished) {
                return;
            }
            if (!m_IsInited) {
                m_IsInited = true;
                OnInit(sender, instance);
            }
            m_CurTime += delta;
            for (int i = m_Triggers.Count - 1; i >= 0; --i) {
                SkillTriggerState trigger = m_Triggers[i];
                if (!trigger.Execute(sender, instance, delta, m_CurTime / 1000)) {
                    trigger.Reset();
                    m_Triggers.RemoveAt(i);
                }
            }
            if (m_Duration <= 0 || m_CurTime / 1000 > m_Duration) {
                m_IsFinished = true;
            }
        }

        private void OnInit(object sender, SkillInstance instance)
        {
            for (int i = 0; i < m_InitTriggers.Count; ++i) {
                m_InitTriggers[i].Reset();
            }
            for (int i = 0; i < m_InitTriggers.Count; ++i) {
                m_InitTriggers[i].Execute(sender, instance, 0, 0);
            }
        }

        private long m_Duration;
        private long m_CurTime;
        private bool m_IsFinished = true;
        private bool m_IsInited;
        private List<SkillTriggerState> m_Triggers = new List<SkillTriggerState>();
        private List<SkillTriggerState> m_InitTriggers = new List<SkillTriggerState>();
        private List<SkillTriggerState> m_LoadedTriggers = new List<SkillTriggerState>();
        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
    }
    internal sealed class SkillMessageHandlerState
    {
        internal long CurTime
        {
            get { return m_CurTime; }
        }
        internal bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }
        internal SkillMessageHandlerState(List<ISkillTriger> loadedTriggers)
        {
            for (int i = 0; i < loadedTriggers.Count; ++i) {
                m_LoadedTriggers.Add(SkillTriggerState.Create(loadedTriggers[i]));
            }
        }
        internal SkillMessageHandlerState CreateDefinitionState()
        {
            return new SkillMessageHandlerState(SkillTriggerState.GetTriggerConfigs(m_LoadedTriggers));
        }
        internal void VisitProperties(VisitPropertyDelegation callback)
        {
            for (int i = 0; i < m_LoadedTriggers.Count; ++i) {
                m_LoadedTriggers[i].VisitProperties(callback);
            }
        }
        internal void Reset()
        {
            m_IsTriggered = false;
            m_CurTime = 0;
            for (int i = 0; i < m_Triggers.Count; ++i) {
                m_Triggers[i].Reset();
            }
            m_Triggers.Clear();
        }
        internal void Prepare()
        {
            for (int i = 0; i < m_Triggers.Count; ++i) {
                m_Triggers[i].Reset();
            }
            m_Triggers.Clear();
            m_CurTime = 0;
            for (int i = 0; i < m_LoadedTriggers.Count; ++i) {
                m_Triggers.Add(m_LoadedTriggers[i]);
            }
            Helper.BubbleSort(m_Triggers, (left, right) => {
                if (left.Trigger.StartTime > right.Trigger.StartTime) {
                    return -1;
                }
                else if (left.Trigger.StartTime == right.Trigger.StartTime) {
                    return 0;
                }
                else {
                    return 1;
                }
            });
        }
        internal void Tick(object sender, SkillInstance instance, long delta)
        {
            m_CurTime += delta;
            for (int i = m_Triggers.Count - 1; i >= 0; --i) {
                SkillTriggerState trigger = m_Triggers[i];
                if (!trigger.Execute(sender, instance, delta, m_CurTime / 1000)) {
                    trigger.Reset();
                    m_Triggers.RemoveAt(i);
                    if (m_Triggers.Count == 0) {
                        m_IsTriggered = false;
                    }
                }
            }
        }
        internal bool IsOver()
        {
            return m_Triggers.Count <= 0;
        }

        private long m_CurTime;
        private bool m_IsTriggered;
        private List<SkillTriggerState> m_Triggers = new List<SkillTriggerState>();
        private List<SkillTriggerState> m_LoadedTriggers = new List<SkillTriggerState>();
    }
    internal sealed class SkillInstanceState
    {
        internal bool IsInterrupted;
        internal bool IsFinished;
        internal bool IsStopCurSection;
        internal int CurSection = -1;
        internal int GoToSectionId = -1;
        internal long CurSectionTime;
        internal long CurTime;
        internal long OriginalDelta;
        internal float TimeScale = 1;
        internal float EffectScale = 1;
        internal float MoveScale = 1;
        internal int DslSkillId;
        internal object Context;
        internal Queue<string> MessageQueue = new Queue<string>();
        internal StrObjDict Variables = new StrObjDict();
        internal CustomDataCollection CustomDatas = new CustomDataCollection();
        internal List<SkillSectionState> Sections = new List<SkillSectionState>();
        internal List<SkillMessageHandlerState> MessageHandlers = new List<SkillMessageHandlerState>();
        internal SkillMessageHandlerState StopSection;
        internal SkillMessageHandlerState InterruptSection;
        internal bool IsInitialized;
    }
}
