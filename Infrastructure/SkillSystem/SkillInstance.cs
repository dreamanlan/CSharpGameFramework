using System;
using System.Collections.Generic;
using GameFramework;

/// <summary>
/// skill(10001)
/// {
///   section(80)
///   {
///     disableinput(10);
///     animation("jump");
///     sound("haha.mp3");
///   };
///   section(10)
///   {
///     animation("kan");
///     addimpact(123,43);
///   };
///   section(1000)
///   {
///     checkinput(skillQ,skillE);
///     complex_triger(123)
///     {
///       configdata1(1231);
///       configdata2(3432)
///       {
///         subconfigdata(1232,321321);
///         subconfigdata(12d32,3fd21321);
///       };
///     };
///   };
///   onstop
///   {
///     triger();
///   };
///   oninterrupt
///   {
///     triger();
///   };
///   onmessage("npckilled",12)
///   {
///     triger();
///   };
/// };
/// </summary>
namespace SkillSystem
{
    public sealed class SkillSection : IPropertyVisitor
    {
        public long Duration
        {
            get { return m_Duration; }
            set { m_Duration = value; }
        }
        public long CurTime
        {
            get { return m_CurTime; }
        }
        public bool IsFinished
        {
            get { return m_IsFinished; }
        }
        //----------------------------------------------
        public void VisitProperties(VisitPropertyDelegation callback)
        {
            m_AccessorHelper.VisitProperties(callback);
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_LoadedTrigers[i].VisitProperties(callback);
            }
        }
        //----------------------------------------------
        public SkillSection Clone()
        {
            SkillSection section = new SkillSection();
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                section.m_LoadedTrigers.Add(m_LoadedTrigers[i].Clone());
            }
            section.m_Duration = m_Duration;
            return section;
        }
        public void Load(Dsl.FunctionData sectionData, int dslSkillId)
        {
            Dsl.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam()) {
                if (callData.GetParamNum() > 0) {
                    m_Duration = long.Parse(callData.GetParamId(0));
                } else {
                    m_Duration = 0;
                }
            } else {
                m_Duration = 0;
            }
            RefreshTrigers(sectionData, dslSkillId);
        }
        public void Reset()
        {
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = true;
        }
        public void Prepare()
        {
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = false;
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_Trigers.Add(m_LoadedTrigers[i]);
            }
            Helper.BubbleSort(m_Trigers, (left, right) => {
                if (left.StartTime > right.StartTime) {
                    return -1;
                } else if (left.StartTime == right.StartTime) {
                    return 0;
                } else {
                    return 1;
                }
            });
        }
        public void Tick(object sender, SkillInstance instance, long delta)
        {
            if (m_IsFinished) {
                return;
            }
            m_CurTime += delta;
            int ct = m_Trigers.Count;
            for (int i = ct - 1; i >= 0; --i) {
                ISkillTriger triger = m_Trigers[i];
                if (!triger.Execute(sender, instance, delta, m_CurTime / 1000)) {
                    triger.Reset();
                    m_Trigers.RemoveAt(i);
                }
            }
            if (m_CurTime / 1000 > m_Duration) {
                m_IsFinished = true;
            }
        }
        public void Analyze(object sender, SkillInstance instance)
        {
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_LoadedTrigers[i].Analyze(sender, instance);
            }
        }
        
        public SkillSection()
        {
            m_AccessorHelper.AddProperty("Duration", () => { return m_Duration; }, (object val) => { m_Duration = (long)Convert.ChangeType(val, typeof(long)); });
            m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; }, (object val) => { m_CurTime = (long)Convert.ChangeType(val, typeof(long)); });
        }

        private void RefreshTrigers(Dsl.FunctionData sectionData, int dslSkillId)
        {
            m_LoadedTrigers.Clear();
            for (int i = 0; i < sectionData.Statements.Count; i++) {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(sectionData.Statements[i], dslSkillId);
                if (null != triger) {
                    m_LoadedTrigers.Add(triger);
                } else {
#if DEBUG
                    string err = string.Format("CreateTriger failed, skill:{0} line:{1} triger:{2}", dslSkillId, sectionData.Statements[i].GetLine(), sectionData.Statements[i].ToScriptString());
                    throw new Exception(err);
#endif
                }
            }
        }

        private long m_Duration = 0;
        private long m_CurTime = 0;
        private bool m_IsFinished = true;
        private List<ISkillTriger> m_Trigers = new List<ISkillTriger>();
        private List<ISkillTriger> m_LoadedTrigers = new List<ISkillTriger>();
        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
    }
    public sealed class SkillMessageHandler : IPropertyVisitor
    {
        public string MsgId
        {
            get { return m_MsgId; }
        }
        public long CurTime
        {
            get { return m_CurTime; }
        }
        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }
        //----------------------------------------------
        public void VisitProperties(VisitPropertyDelegation callback)
        {
            m_AccessorHelper.VisitProperties(callback);
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_LoadedTrigers[i].VisitProperties(callback);
            }
        }
        //----------------------------------------------
        public SkillMessageHandler Clone()
        {
            SkillMessageHandler section = new SkillMessageHandler();
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                section.m_LoadedTrigers.Add(m_LoadedTrigers[i].Clone());
            }
            section.m_MsgId = m_MsgId;
            return section;
        }
        public void Load(Dsl.FunctionData sectionData, int dslSkillId)
        {
            Dsl.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam()) {
                string[] args = new string[callData.GetParamNum()];
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    args[i] = callData.GetParamId(i);
                }
                m_MsgId = string.Join(":", args);
            }
            RefreshTrigers(sectionData, dslSkillId);
        }
        public void Reset()
        {
            m_IsTriggered = false;
            m_CurTime = 0;
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
        }
        public void Prepare()
        {
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_Trigers.Add(m_LoadedTrigers[i]);
            }
            Helper.BubbleSort(m_Trigers, (left, right) => {
                if (left.StartTime > right.StartTime) {
                    return -1;
                } else if (left.StartTime == right.StartTime) {
                    return 0;
                } else {
                    return 1;
                }
            });
        }
        public void Tick(object sender, SkillInstance instance, long delta)
        {
            m_CurTime += delta;
            int ct = m_Trigers.Count;
            for (int i = ct - 1; i >= 0; --i) {
                ISkillTriger triger = m_Trigers[i];
                if (!triger.Execute(sender, instance, delta, m_CurTime / 1000)) {
                    triger.Reset();
                    m_Trigers.RemoveAt(i);
                    if (m_Trigers.Count == 0) {
                        m_IsTriggered = false;
                    }
                }
            }
        }

        public bool IsOver()
        {
            return m_Trigers.Count <= 0 ? true : false;
        }

        public void Analyze(object sender, SkillInstance instance)
        {
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                m_LoadedTrigers[i].Analyze(sender, instance);
            }
        }

        public SkillMessageHandler()
        {
            //m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; }, (object val) => { m_CurTime = (long)Convert.ChangeType(val, typeof(long)); });
        }

        private void RefreshTrigers(Dsl.FunctionData sectionData, int dslSkillId)
        {
            m_LoadedTrigers.Clear();
            for (int i = 0; i < sectionData.Statements.Count; i++) {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(sectionData.Statements[i], dslSkillId);
                if (null != triger) {
                    m_LoadedTrigers.Add(triger);
                } else {
#if DEBUG
                    string err = string.Format("CreateTriger failed, skill:{0} line:{1} triger:{2}", dslSkillId, sectionData.Statements[i].GetLine(), sectionData.Statements[i].ToScriptString());
                    throw new Exception(err);
#endif
                }
            }
        }

        private string m_MsgId = "";
        private long m_CurTime = 0;
        private bool m_IsTriggered = false;
        private List<ISkillTriger> m_Trigers = new List<ISkillTriger>();
        private List<ISkillTriger> m_LoadedTrigers = new List<ISkillTriger>();
        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
    }
    public sealed class InplaceSkillPropertyInfo
    {
        public string Group;
        public string Key;
        public IProperty Property;
    }
    public sealed class SkillInstance
    {
        public int InitDslSkillId
        {
            get { return m_InitDslSkillId; }
        }
        public int DslSkillId
        {
            get { return m_DslSkillId; }
            set { m_DslSkillId = value; }
        }
        public object Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }
        public bool IsInterrupted
        {
            get { return m_IsInterrupted; }
            set { m_IsInterrupted = value; }
        }
        public bool IsFinished
        {
            get { return m_IsFinished; }
            set { m_IsFinished = value; }
        }
        public long CurTime
        {
            get { return m_CurTime / 1000; }
        }
        public long OriginalDelta
        {
            get { return m_OriginalDelta; }
        }
        public float TimeScale
        {
            get { return m_TimeScale; }
            set { m_TimeScale = value; }
        }
        public float EffectScale
        {
            get { return m_EffectScale; }
            set { m_EffectScale = value; }
        }
        public float MoveScale
        {
            get { return m_MoveScale; }
            set { m_MoveScale = value; }
        }
        public int GoToSection
        {
            get { return m_GoToSectionId; }
            set { m_GoToSectionId = value; }
        }
        public Dictionary<string, object> LocalVariables
        {
            get { return m_LocalVariables; }
        }
        public Dictionary<string, object> GlobalVariables
        {
            get { return m_GlobalVariables; }
            set { m_GlobalVariables = value; }
        }
        public TypedDataCollection CustomDatas
        {
            get { return m_CustomDatas; }
        }
        //----------------------------------------------
        public bool AlreadyAnalyzed
        {
            get { return m_AlreadyAnalyzed; }
            set { m_AlreadyAnalyzed = value; }
        }
        //下面几个property是Analyze获取的数据
        public int StartWithoutStopMoveCount
        {
            get { return m_StartWithoutMoveCount; }
            set { m_StartWithoutMoveCount = value; }
        }
        public int StopMoveCount
        {
            get { return m_StopMoveCount; }
            set { m_StopMoveCount = value; }
        }
        //
        public int EnableMoveCount
        {
            get { return m_EnableMoveCount; }
            set { m_EnableMoveCount = value; }
        }
        public List<int> EnabledImpactsToOther
        {
            get { return m_EnabledImpactsToOther; }
        }
        public List<int> EnabledImpactsToMyself
        {
            get { return m_EnabledImpactsToMyself; }
        }
        public List<int> SummonNpcSkills
        {
            get { return m_SummonNpcSkills; }
        }
        public List<int> SummonNpcs
        {
            get { return m_SummonNpcs; }
        }
        public long MaxSkillLifeTime
        {
            get { return m_MaxSkillLifeTime; }
            set { m_MaxSkillLifeTime = value; }
        }
        public List<string> Resources
        {
            get { return m_Resources; }
        }
        //----------------------------------------------
        public SkillInstance EmitSkillInstance
        {
            get { return m_EmitSkillInstance; }
        }
        public SkillInstance HitSkillInstance
        {
            get { return m_HitSkillInstance; }
        }
        //----------------------------------------------
        public List<InplaceSkillPropertyInfo> CollectProperties()
        {
            List<InplaceSkillPropertyInfo> list = new List<InplaceSkillPropertyInfo>();
            m_AccessorHelper.VisitProperties((string group, string key, IProperty property) => {
                list.Add(new InplaceSkillPropertyInfo { Group = "SkillInstance", Key = key, Property = property });
            });
            for (int i = 0; i < m_Sections.Count; ++i) {
                List<InplaceSkillPropertyInfo> temp = new List<InplaceSkillPropertyInfo>();
                temp.Add(new InplaceSkillPropertyInfo { Group = "SkillSection", Key = "Section" + i, Property = null });
                m_Sections[i].VisitProperties((string group, string key, IProperty property) => {
                    temp.Add(new InplaceSkillPropertyInfo { Group = string.IsNullOrEmpty(group) ? "SkillSection" : group, Key = key, Property = property });
                });
                if (temp.Count > 1) {
                    list.AddRange(temp);
                }
            }
            for (int i = 0; i < m_MessageHandlers.Count; ++i) {
                List<InplaceSkillPropertyInfo> temp = new List<InplaceSkillPropertyInfo>();
                temp.Add(new InplaceSkillPropertyInfo { Group = "SkillMessageHandler", Key = m_MessageHandlers[i].MsgId, Property = null });
                m_MessageHandlers[i].VisitProperties((string group, string key, IProperty property) => {
                    temp.Add(new InplaceSkillPropertyInfo { Group = string.IsNullOrEmpty(group) ? "SkillMessageHandler" : group, Key = key, Property = property });
                });
                if (temp.Count > 1) {
                    list.AddRange(temp);
                }
            }
            if (null != m_InterruptSection) {
                List<InplaceSkillPropertyInfo> temp = new List<InplaceSkillPropertyInfo>();
                temp.Add(new InplaceSkillPropertyInfo { Group = "SkillMessageHandler", Key = "oninterrupt", Property = null });
                m_InterruptSection.VisitProperties((string group, string key, IProperty property) => {
                    temp.Add(new InplaceSkillPropertyInfo { Group = string.IsNullOrEmpty(group) ? "SkillMessageHandler" : group, Key = key, Property = property });
                });
                if (temp.Count > 1) {
                    list.AddRange(temp);
                }
            }
            if (null != m_StopSection) {
                List<InplaceSkillPropertyInfo> temp = new List<InplaceSkillPropertyInfo>();
                temp.Add(new InplaceSkillPropertyInfo { Group = "SkillMessageHandler", Key = "onstop", Property = null });
                m_StopSection.VisitProperties((string group, string key, IProperty property) => {
                    temp.Add(new InplaceSkillPropertyInfo { Group = string.IsNullOrEmpty(group) ? "SkillMessageHandler" : group, Key = key, Property = property });
                });
                if (temp.Count > 1) {
                    list.AddRange(temp);
                }
            }
            return list;
        }
        //----------------------------------------------
        public void SetLocalVariable(string varName, object varValue)
        {
            if (m_LocalVariables.ContainsKey(varName)) {
                m_LocalVariables[varName] = varValue;
            } else {
                m_LocalVariables.Add(varName, varValue);
            }
        }
        public void SetGlobalVariable(string varName, object varValue)
        {
            if (null != m_GlobalVariables) {
                if (m_GlobalVariables.ContainsKey(varName)) {
                    m_GlobalVariables[varName] = varValue;
                } else {
                    m_GlobalVariables.Add(varName, varValue);
                }
            }
        }
        //----------------------------------------------
        public SkillInstance Clone()
        {
            SkillInstance instance = new SkillInstance();
            for (int i = 0; i < m_Sections.Count; i++) {
                instance.m_Sections.Add(m_Sections[i].Clone());
            }
            instance.m_IsInterrupted = false;
            instance.m_IsFinished = false;
            instance.m_CurSection = 0;
            instance.m_CurSectionDuration = 0;
            instance.m_CurSectionTime = 0;
            instance.m_CurTime = 0;
            instance.m_GoToSectionId = -1;

            instance.m_InitDslSkillId = m_InitDslSkillId;
            instance.m_DslSkillId = m_DslSkillId;
            if (m_StopSection != null) {
                instance.m_StopSection = m_StopSection.Clone();
            }
            if (m_InterruptSection != null) {
                instance.m_InterruptSection = m_InterruptSection.Clone();
            }
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                instance.m_MessageHandlers.Add(m_MessageHandlers[i].Clone());
            }
            //嵌在技能内的技能实例只用作克隆的母本，可以为多个技能实例共享
            instance.m_EmitSkillInstance = m_EmitSkillInstance;
            instance.m_HitSkillInstance = m_HitSkillInstance;
            return instance;
        }
        public bool Init(Dsl.DslInfo config)
        {
            Dsl.FunctionData skill = config.First;
            return Init(skill);
        }
        public void Reset()
        {
            m_IsInterrupted = false;
            m_IsFinished = false;
            m_IsStopCurSection = false;
            m_TimeScale = 1;
            m_CurSection = -1;
            m_GoToSectionId = -1;

            int ct = m_Sections.Count;
            for (int i = ct - 1; i >= 0; --i) {
                SkillSection section = m_Sections[i];
                section.Reset();
            }
            m_MessageQueue.Clear();
            m_LocalVariables.Clear();
            m_CustomDatas.Clear();
        }
        public void Start(object sender)
        {
            m_CurTime = 0;            
            m_CurSection = -1;
        }
        public void SendMessage(string msgId)
        {
            m_MessageQueue.Enqueue(msgId);
        }
        public void Tick(object sender, long deltaTime)
        {
            if (m_IsFinished) {
                return;
            }
            m_OriginalDelta = deltaTime;
            long delta = (long)(deltaTime * m_TimeScale);
            m_CurSectionTime += delta;
            m_CurTime += delta;
            if (m_CurSection < 0) {
                //first tick
                TickMessageHandlers(sender, 0);
            } else {
                TickMessageHandlers(sender, delta);
            }
            if (!IsSectionDone(m_CurSection)) {
                m_Sections[m_CurSection].Tick(sender, this, delta);
            }
            if (m_IsStopCurSection) {
                m_IsStopCurSection = false;
                ResetCurSection();
                ChangeToSection(m_CurSection + 1);
                DoFirstSectionTick(sender);
            }
            // do change section task
            if (m_GoToSectionId >= 0) {
                ResetCurSection();
                ChangeToSection(m_GoToSectionId);
                DoFirstSectionTick(sender);
                m_GoToSectionId = -1;
            }
            if (IsSectionDone(m_CurSection) && m_CurSection < m_Sections.Count - 1) {
                ResetCurSection();
                ChangeToSection(m_CurSection + 1);
                DoFirstSectionTick(sender);
            }
            if (IsMessageDone() && IsAllSectionDone()) {
                OnSkillStop(sender);
            }
        }        
        public void Analyze(object sender)
        {
            m_StartWithoutMoveCount = 0;
            m_StopMoveCount = 0;

            m_EnableMoveCount = 0;
            m_EnabledImpactsToOther.Clear();
            m_EnabledImpactsToMyself.Clear();
            m_SummonNpcSkills.Clear();
            m_SummonNpcs.Clear();
            m_MaxSkillLifeTime = 0;

            for (int i = 0; i < m_Sections.Count; i++) {
                m_Sections[i].Analyze(sender, this);
                m_MaxSkillLifeTime += m_Sections[i].Duration;
            }
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                m_MessageHandlers[i].Analyze(sender, this);
                m_MaxSkillLifeTime += 1000;
            }
            if (null != m_InterruptSection) {
                m_InterruptSection.Analyze(sender, this);
            }
            if (null != m_StopSection) {
                m_StopSection.Analyze(sender, this);
            }

            m_EnableMoveCount = m_StopMoveCount + (m_StartWithoutMoveCount > 0 ? 1 : 0);

            m_AlreadyAnalyzed = true;
        }
        public void OnInterrupt(object sender)
        {
            if (m_InterruptSection != null) {
                m_InterruptSection.Prepare();                
                m_InterruptSection.Tick(sender, this, m_CurTime);
            }
            ResetCurSection();
            StopMessageHandlers();
            m_IsFinished = true;
        }
        public void OnSkillStop(object sender)
        {
            if (m_StopSection != null) {
                m_StopSection.Prepare();
                m_StopSection.Tick(sender, this, m_CurTime);
            }
            ResetCurSection();
            StopMessageHandlers();
            m_IsFinished = true;
        }

        public bool IsMessageDone()
        {
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                if (m_MessageHandlers[i].IsTriggered) {
                    return false;
                }
            }
            return true;
        }
        public void SetCurSectionDuration(long duration)
        {
            if (!IsSectionDone(m_CurSection)) {
                SkillSection section = m_Sections[m_CurSection];
                section.Duration = duration;
            }
        }    
        public void AdjustCurSectionDuration(long leftDuration)
        {
            if (!IsSectionDone(m_CurSection)) {
                SkillSection section = m_Sections[m_CurSection];
                section.Duration = section.CurTime + leftDuration;
            }
        }
        public void StopCurSection()
        {
            m_IsStopCurSection = true;
        }

        public SkillInstance()
        {
            m_AccessorHelper.AddProperty("DslSkillId", () => { return m_DslSkillId; }, (object val) => { });
            m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; }, (object val) => { m_CurTime = (long)Convert.ChangeType(val, typeof(long)); });
            m_AccessorHelper.AddProperty("CurSection", () => { return m_CurSection; }, (object val) => { m_CurSection = (int)Convert.ChangeType(val, typeof(int)); });
            m_AccessorHelper.AddProperty("CurSectionTime", () => { return m_CurSectionTime; }, (object val) => { m_CurSectionTime = (long)Convert.ChangeType(val, typeof(long)); });
        }

        private bool IsSectionDone(int sectionnum)
        {
            if (sectionnum >= 0 && sectionnum < m_Sections.Count) {
                SkillSection section = m_Sections[sectionnum];
                if (section.IsFinished) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
        private bool IsAllSectionDone()
        {
            if (IsSectionDone(m_CurSection) && m_CurSection == m_Sections.Count - 1) {
                return true;
            }
            return false;
        }
        private void ResetCurSection()
        {
            if (m_CurSection >= 0 && m_CurSection < m_Sections.Count) {
                SkillSection section = m_Sections[m_CurSection];
                section.Reset();
            }
        }
        private void ChangeToSection(int index)
        {
            if (index >= 0 && index < m_Sections.Count) {
                SkillSection section = m_Sections[index];
                m_CurSection = index;
                m_CurSectionDuration = section.Duration * 1000;
                m_CurSectionTime = 0;
                section.Prepare();
                LogSystem.Debug("ChangeToSection:{0} duration:{1}", index, m_CurSectionDuration);
            }
        }
        private void DoFirstSectionTick(object sender)
        {
            if (m_CurSection >= 0 && m_CurSection < m_Sections.Count) {
                SkillSection section = m_Sections[m_CurSection];
                section.Tick(sender, this, 0);
            }
        }
        private void TickMessageHandlers(object sender, long delta)
        {
            if (m_MessageQueue.Count > 0) {
                int cantTriggerCount = 0;
                int triggerCount = 0;
                string msgId = m_MessageQueue.Peek();
                for (int i = 0; i < m_MessageHandlers.Count; i++) {
                    if (m_MessageHandlers[i].MsgId == msgId) {
                        if (m_MessageHandlers[i].IsTriggered) {
                            ++cantTriggerCount;
                        } else {
                            m_MessageHandlers[i].Prepare();
                            m_MessageHandlers[i].IsTriggered = true;
                            ++triggerCount;
                        }
                    }
                }
                if (cantTriggerCount == 0 || triggerCount > 0) {
                    m_MessageQueue.Dequeue();
                }
            }
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                if (m_MessageHandlers[i].IsTriggered) {
                    m_MessageHandlers[i].Tick(sender, this, delta);
                    if (m_MessageHandlers[i].IsOver()) {
                        m_MessageHandlers[i].Reset();
                    }
                }
            }
        }
        private void StopMessageHandlers()
        {
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                if (m_MessageHandlers[i].IsTriggered) {
                    m_MessageHandlers[i].Reset();
                }
            }
        }

        private bool Init(Dsl.FunctionData skill)
        {
            bool ret = false;
            if (null != skill && (skill.GetId() == "skill" || skill.GetId() == "emitskill" || skill.GetId() == "hitskill")) {
                ret = true;
                Dsl.CallData callData = skill.Call;
                if (null != callData && callData.HaveParam()) {
                    m_InitDslSkillId = int.Parse(callData.GetParamId(0));
                    m_DslSkillId = m_InitDslSkillId;
                }

                for (int i = 0; i < skill.Statements.Count; i++) {
                    if (skill.Statements[i].GetId() == "section") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            SkillSection section = new SkillSection();
                            section.Load(sectionData, m_DslSkillId);
                            m_Sections.Add(section);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, section must be a function ! line:{1} section:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, section must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "onmessage") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            SkillMessageHandler handler = new SkillMessageHandler();
                            handler.Load(sectionData, m_DslSkillId);
                            m_MessageHandlers.Add(handler);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, onmessage must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, onmessage must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "onstop") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_StopSection = new SkillMessageHandler();
                            m_StopSection.Load(sectionData, m_DslSkillId);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, onstop must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, onstop must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "oninterrupt") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_InterruptSection = new SkillMessageHandler();
                            m_InterruptSection.Load(sectionData, m_DslSkillId);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, oninterrupt must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "emitskill") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_EmitSkillInstance = new SkillInstance();
                            m_EmitSkillInstance.Init(sectionData);
                            m_EmitSkillInstance.m_InitDslSkillId = m_InitDslSkillId;
                            m_EmitSkillInstance.m_DslSkillId = m_DslSkillId;
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, emitskill must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "hitskill") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_HitSkillInstance = new SkillInstance();
                            m_HitSkillInstance.Init(sectionData);
                            m_HitSkillInstance.m_InitDslSkillId = m_InitDslSkillId;
                            m_HitSkillInstance.m_DslSkillId = m_DslSkillId;
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, hitskill must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else {
#if DEBUG
                        string err = string.Format("SkillInstance::Init, Skill {0} unknown part {1}, line:{2} section:{3}", m_DslSkillId, skill.Statements[i].GetId(), skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString());
                        throw new Exception(err);
#else
            LogSystem.Error("SkillInstance::Init, Skill {0} unknown part {1}", m_DslSkillId, skill.Statements[i].GetId());
#endif
                    }
                }
            } else {
#if DEBUG
                string err = string.Format("SkillInstance::Init, isn't skill DSL, line:{0} skill:{1}", skill.GetLine(), skill.ToScriptString());
                throw new Exception(err);
#else
        LogSystem.Error("SkillInstance::Init, isn't skill DSL");
#endif
            }
            LogSystem.Debug("SkillInstance.Init section num:{0} {1} skill {2}", m_Sections.Count, ret, m_DslSkillId);
            return ret;
        }

        private bool m_IsInterrupted = false;
        private bool m_IsFinished = false;
        private bool m_IsStopCurSection = false;

        private int m_CurSection = -1;
        private int m_GoToSectionId = -1;
        private long m_CurSectionDuration = 0;
        private long m_CurSectionTime = 0;
        private long m_CurTime = 0;
        private long m_OriginalDelta = 0;
        private float m_TimeScale = 1;
        private float m_EffectScale = 1;
        private float m_MoveScale = 1;

        private int m_DslSkillId = 0;
        private object m_Context = null;
        private List<SkillSection> m_Sections = new List<SkillSection>();
        private Queue<string> m_MessageQueue = new Queue<string>();
        private List<SkillMessageHandler> m_MessageHandlers = new List<SkillMessageHandler>();
        private SkillMessageHandler m_StopSection = null;
        private SkillMessageHandler m_InterruptSection = null;

        private bool m_AlreadyAnalyzed = false;

        private int m_StartWithoutMoveCount = 0;
        private int m_StopMoveCount = 0;
        private int m_EnableMoveCount = 0;

        private List<int> m_EnabledImpactsToOther = new List<int>();
        private List<int> m_EnabledImpactsToMyself = new List<int>();
        private List<int> m_SummonNpcSkills = new List<int>();
        private List<int> m_SummonNpcs = new List<int>();
        private List<string> m_Resources = new List<string>();

        private long m_MaxSkillLifeTime = 0;
        
        private Dictionary<string, object> m_LocalVariables = new Dictionary<string, object>();
        private Dictionary<string, object> m_GlobalVariables = null;
        private TypedDataCollection m_CustomDatas = new TypedDataCollection();

        private int m_InitDslSkillId = 0;
        private SkillInstance m_EmitSkillInstance = null;
        private SkillInstance m_HitSkillInstance = null;

        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
    }
}
