using System;
using System.Collections.Generic;
using System.IO;
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
    public sealed class IntSkillInstanceDict : Dictionary<int, SkillInstance>
    {
        public IntSkillInstanceDict() { }
        public IntSkillInstanceDict(IDictionary<int, SkillInstance> dict) : base(dict) { }
    }
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
            for (int i = 0; i < m_InitTrigers.Count; ++i) {
                section.m_InitTrigers.Add(m_InitTrigers[i].Clone());
            }
            for (int i = 0; i < m_LoadedTrigers.Count; i++) {
                section.m_LoadedTrigers.Add(m_LoadedTrigers[i].Clone());
            }
            section.m_Duration = m_Duration;
            return section;
        }
        public void Load(Dsl.FunctionData sectionData, SkillInstance instance)
        {
            Dsl.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam()) {
                m_InitTrigers.Clear();
                int num = callData.GetParamNum();
                for (int i = 0; i < num; ++i) {
                    Dsl.ISyntaxComponent arg = callData.GetParam(i);
                    if (arg is Dsl.ValueData) {
                        m_Duration = long.Parse(arg.GetId());
                    } else {
                        ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(arg, instance);
                        if (null != triger) {
                            m_InitTrigers.Add(triger);
                        } else {
#if DEBUG
                            string err = string.Format("CreateInitTriger failed, skill:{0} line:{1} triger:{2}", instance.DslSkillId, arg.GetLine(), arg.ToScriptString(false));
                            throw new Exception(err);
#endif
                        }
                    }
                }
            } else {
                m_Duration = 0;
            }
            RefreshTrigers(sectionData, instance);
        }
        public void Reset()
        {
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = true;
            m_IsInited = false;
        }
        public void Prepare()
        {
            for (int i = 0; i < m_Trigers.Count; i++) {
                m_Trigers[i].Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = false;
            m_IsInited = false;
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
            if (!m_IsInited) {
                m_IsInited = true;
                OnInit(sender, instance);
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
            if (m_Duration <= 0 || m_CurTime / 1000 > m_Duration) {
                m_IsFinished = true;
            }
        }

        public SkillSection()
        {
            m_AccessorHelper.AddProperty("Duration", () => { return m_Duration; }, (object val) => { m_Duration = (long)Convert.ChangeType(val, typeof(long)); });
            m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; });
        }

        private void OnInit(object sender, SkillInstance instance)
        {
            for (int i = 0; i < m_InitTrigers.Count; ++i) {
                m_InitTrigers[i].Reset();
            }
            for (int i = 0; i < m_InitTrigers.Count; ++i) {
                m_InitTrigers[i].Execute(sender, instance, 0, 0);
            }
        }
        private void RefreshTrigers(Dsl.FunctionData sectionData, SkillInstance instance)
        {
            m_LoadedTrigers.Clear();
            for (int i = 0; i < sectionData.Statements.Count; i++) {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(sectionData.Statements[i], instance);
                if (null != triger) {
                    m_LoadedTrigers.Add(triger);
                } else {
#if DEBUG
                    string err = string.Format("CreateTriger failed, skill:{0} line:{1} triger:{2}", instance.DslSkillId, sectionData.Statements[i].GetLine(), sectionData.Statements[i].ToScriptString(false));
                    throw new Exception(err);
#endif
                }
            }
        }

        private long m_Duration = 0;
        private long m_CurTime = 0;
        private bool m_IsFinished = true;
        private bool m_IsInited = false;
        private List<ISkillTriger> m_Trigers = new List<ISkillTriger>();

        private List<ISkillTriger> m_InitTrigers = new List<ISkillTriger>();
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
        public void Load(Dsl.FunctionData sectionData, SkillInstance instance)
        {
            Dsl.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam()) {
                string[] args = new string[callData.GetParamNum()];
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    args[i] = callData.GetParamId(i);
                }
                m_MsgId = string.Join(":", args);
            }
            RefreshTrigers(sectionData, instance);
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

        public SkillMessageHandler()
        {
            //m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; });
        }

        private void RefreshTrigers(Dsl.FunctionData sectionData, SkillInstance instance)
        {
            m_LoadedTrigers.Clear();
            for (int i = 0; i < sectionData.Statements.Count; i++) {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(sectionData.Statements[i], instance);
                if (null != triger) {
                    m_LoadedTrigers.Add(triger);
                } else {
#if DEBUG
                    string err = string.Format("CreateTriger failed, skill:{0} line:{1} triger:{2}", instance.DslSkillId, sectionData.Statements[i].GetLine(), sectionData.Statements[i].ToScriptString(false));
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
        public int InnerDslSkillId
        {
            get { return m_InnerDslSkillId; }
        }
        public int OuterDslSkillId
        {
            get { return m_OuterDslSkillId; }
            internal set 
            { 
                m_OuterDslSkillId = value;
                m_DslSkillId = value;

                if (null != m_EmitSkillInstances) {
                    foreach (var pair in m_EmitSkillInstances) {
                        pair.Value.m_OuterDslSkillId = value;
                    }
                }
                if (null != m_HitSkillInstances) {
                    foreach (var pair in m_HitSkillInstances) {
                        pair.Value.m_OuterDslSkillId = value;
                    }
                }
            }
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
        public int CurSection
        {
            get { return m_CurSection; }
        }
        public long CurSectionDuration
        {
            get
            {
                long ret = 0;
                if (m_CurSection >= 0 && m_CurSection < m_Sections.Count) {
                    ret = m_Sections[m_CurSection].Duration;
                }
                return ret;
            }
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
        public StrObjDict Variables
        {
            get { return m_Variables; }
        }
        public CustomDataCollection CustomDatas
        {
            get { return m_CustomDatas; }
        }
        //----------------------------------------------
        public IntSkillInstanceDict EmitSkillInstances
        {
            get { return m_EmitSkillInstances; }
        }
        public IntSkillInstanceDict HitSkillInstances
        {
            get { return m_HitSkillInstances; }
        }
        //----------------------------------------------
        public int ImpactCount
        {
            get { return m_ImpactCount; }
        }
        public int DamageCount
        {
            get { return m_DamageCount; }
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
        public void SetVariable(string varName, object varValue)
        {
            if (m_Variables.ContainsKey(varName)) {
                m_Variables[varName] = varValue;
            } else {
                m_Variables.Add(varName, varValue);
            }
        }
        //----------------------------------------------
        public SkillInstance Clone()
        {
            SkillInstance instance = new SkillInstance();
            for (int i = 0; i < m_Sections.Count; i++) {
                instance.m_Sections.Add(m_Sections[i].Clone());
            }
            instance.m_InnerDslSkillId = m_InnerDslSkillId;
            instance.m_OuterDslSkillId = m_OuterDslSkillId;
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
            instance.m_EmitSkillInstances = m_EmitSkillInstances;
            instance.m_HitSkillInstances = m_HitSkillInstances;
            instance.m_ImpactCount = m_ImpactCount;
            instance.m_DamageCount = m_DamageCount;
            //用于写回dsl文件的解析后的dsl数据，调试模式在修改后可保存到代码文件
            instance.m_SkillDsl = m_SkillDsl;
            return instance;
        }
        public bool Init(Dsl.DslInfo config)
        {
            Dsl.FunctionData skill = config.First;
            return Init(skill);
        }
        public bool Init(Dsl.FunctionData skill)
        {
            bool ret = false;
            m_SkillDsl = skill;
            m_UseImpactsForInit = new List<SkillSectionOrMessageTriggers>();
            m_ImpactsForInit = new List<SkillSectionOrMessageTriggers>();
            m_DamagesForInit = new List<SkillSectionOrMessageTriggers>();
            if (null != skill && (skill.GetId() == "skill" || skill.GetId() == "skilldsl" || skill.GetId() == "emitskill" || skill.GetId() == "hitskill")) {
                ret = true;
                Dsl.CallData callData = skill.Call;
                if (null != callData && callData.HaveParam()) {
                    m_OuterDslSkillId = int.Parse(callData.GetParamId(0));
                    m_DslSkillId = m_OuterDslSkillId;
                }

                for (int i = 0; i < skill.Statements.Count; i++) {
                    if (skill.Statements[i].GetId() == "section") {
                        m_UseImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Section));
                        m_ImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Section));
                        m_DamagesForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Section));
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            SkillSection section = new SkillSection();
                            section.Load(sectionData, this);
                            m_Sections.Add(section);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, section must be a function ! line:{1} section:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, section must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "onmessage") {
                        m_UseImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Message));
                        m_ImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Message));
                        m_DamagesForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.Message));
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            SkillMessageHandler handler = new SkillMessageHandler();
                            handler.Load(sectionData, this);
                            m_MessageHandlers.Add(handler);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, onmessage must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, onmessage must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "onstop") {
                        m_UseImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnStop));
                        m_ImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnStop));
                        m_DamagesForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnStop));
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_StopSection = new SkillMessageHandler();
                            m_StopSection.Load(sectionData, this);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, onstop must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, onstop must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "oninterrupt") {
                        m_UseImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnInterrupt));
                        m_ImpactsForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnInterrupt));
                        m_DamagesForInit.Add(new SkillSectionOrMessageTriggers(SectionOrMessageType.OnInterrupt));
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            m_InterruptSection = new SkillMessageHandler();
                            m_InterruptSection.Load(sectionData, this);
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, oninterrupt must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "emitskill") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            PrepareInnerEmitSkillInstances();
                            SkillInstance inst = new SkillInstance();
                            inst.Init(sectionData);
                            Dsl.CallData header = sectionData.Call;
                            int innerId = 0;
                            if (header.GetParamNum() > 0) {
                                innerId = int.Parse(header.GetParamId(0));
                            }
                            inst.m_InnerDslSkillId = GenInnerEmitSkillId(innerId);
                            inst.m_OuterDslSkillId = m_DslSkillId;
                            inst.m_DslSkillId = m_DslSkillId;
                            if (!m_EmitSkillInstances.ContainsKey(inst.InnerDslSkillId)) {
                                m_EmitSkillInstances.Add(inst.InnerDslSkillId, inst);
                            } else {
#if DEBUG
                                string err = string.Format("Skill {0} DSL, emitskill id duplicate ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                                throw new Exception(err);
#else
                                LogSystem.Error("Skill {0} DSL, emitskill id duplicate !", m_DslSkillId);
#endif
                            }
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, emitskill must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else if (skill.Statements[i].GetId() == "hitskill") {
                        Dsl.FunctionData sectionData = skill.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            PrepareInnerHitSkillInstances();
                            SkillInstance inst = new SkillInstance();
                            inst.Init(sectionData);
                            Dsl.CallData header = sectionData.Call;
                            int innerId = 0;
                            if (header.GetParamNum() > 0) {
                                innerId = int.Parse(header.GetParamId(0));
                            }
                            inst.m_InnerDslSkillId = GenInnerHitSkillId(innerId);
                            inst.m_OuterDslSkillId = m_DslSkillId;
                            inst.m_DslSkillId = m_DslSkillId;
                            if (!m_HitSkillInstances.ContainsKey(inst.InnerDslSkillId)) {
                                m_HitSkillInstances.Add(inst.InnerDslSkillId, inst);
                            } else {
#if DEBUG
                                string err = string.Format("Skill {0} DSL, hitskill id duplicate ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                                throw new Exception(err);
#else
                                LogSystem.Error("Skill {0} DSL, hitskill id duplicate !", m_DslSkillId);
#endif
                            }
                        } else {
#if DEBUG
                            string err = string.Format("Skill {0} DSL, hitskill must be a function ! line:{1} onmessage:{2}", m_DslSkillId, skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
              LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_DslSkillId);
#endif
                        }
                    } else {
#if DEBUG
                        string err = string.Format("SkillInstance::Init, Skill {0} unknown part {1}, line:{2} section:{3}", m_DslSkillId, skill.Statements[i].GetId(), skill.Statements[i].GetLine(), skill.Statements[i].ToScriptString(false));
                        throw new Exception(err);
#else
            LogSystem.Error("SkillInstance::Init, Skill {0} unknown part {1}", m_DslSkillId, skill.Statements[i].GetId());
#endif
                    }
                }
            } else {
#if DEBUG
                string err = string.Format("SkillInstance::Init, isn't skill DSL, line:{0} skill:{1}", skill.GetLine(), skill.ToScriptString(false));
                throw new Exception(err);
#else
        LogSystem.Error("SkillInstance::Init, isn't skill DSL");
#endif
            }
            BuildImpactAndDamageInfo();
            LogSystem.Debug("SkillInstance.Init section num:{0} {1} skill {2}", m_Sections.Count, ret, m_DslSkillId);
            return ret;
        }
        public string ToScriptString()
        {
            Dsl.DslInfo info = new Dsl.DslInfo();
            info.AddFunction(m_SkillDsl);
            return info.ToScriptString(true);
        }
        public void Save(string file)
        {
#if DEBUG
            if (null != m_SkillDsl) {
                using (StreamWriter sw = new StreamWriter(file)) {
                    if (null != sw) {
                        sw.Write(ToScriptString());
                    }
                    sw.Close();
                }
            }    
#endif
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
            m_Variables.Clear();
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
            m_AccessorHelper.AddProperty("DslSkillId", () => { return m_DslSkillId; });
            m_AccessorHelper.AddProperty("CurTime", () => { return m_CurTime; });
            m_AccessorHelper.AddProperty("CurSection", () => { return m_CurSection; });
            m_AccessorHelper.AddProperty("CurSectionTime", () => { return m_CurSectionTime; });
            m_AccessorHelper.AddProperty("ImpactCount", () => { return m_ImpactCount; });
            m_AccessorHelper.AddProperty("DamageCount", () => { return m_DamageCount; });
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
                m_CurSectionTime = 0;
                section.Prepare();
                LogSystem.Debug("ChangeToSection:{0} duration:{1}", index, section.Duration);
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
        
        private void PrepareInnerEmitSkillInstances()
        {
            if (null == m_EmitSkillInstances) {
                m_EmitSkillInstances = new IntSkillInstanceDict();
            }
        }
        private void PrepareInnerHitSkillInstances()
        {
            if (null == m_HitSkillInstances) {
                m_HitSkillInstances = new IntSkillInstanceDict();
            }
        }


        //----------------------------------------------
        public void AddUseImpactForInit(ISkillTriger trigger, int impactId, bool isExternalImpact)
        {
            int ct = m_UseImpactsForInit.Count;
            if (ct > 0) {
                SkillSectionOrMessageTriggers group = m_UseImpactsForInit[ct - 1];
                if (null != group) {
                    group.Triggers.Add(new TriggerInfo { Trigger = trigger, ImpactId = impactId, IsExternalImpact = isExternalImpact, IsEmitImpact = false });
                }
            }
        }
        public void AddImpactForInit(ISkillTriger trigger)
        {
            AddImpactForInit(trigger, 0, false);
        }
        public void AddImpactForInit(ISkillTriger trigger, int impactId, bool isExternalImpact)
        {
            int ct = m_ImpactsForInit.Count;
            if (ct > 0) {
                SkillSectionOrMessageTriggers group = m_ImpactsForInit[ct - 1];
                if (null != group) {
                    group.Triggers.Add(new TriggerInfo { Trigger = trigger, ImpactId = impactId, IsExternalImpact = isExternalImpact, IsEmitImpact = true });
                }
            }
        }
        public void AddDamageForInit(ISkillTriger trigger)
        {
            int ct = m_DamagesForInit.Count;
            if (ct > 0) {
                SkillSectionOrMessageTriggers group = m_DamagesForInit[ct - 1];
                if (null != group) {
                    group.Triggers.Add(new TriggerInfo { Trigger = trigger, ImpactId = 0, IsExternalImpact = false, IsEmitImpact = false });
                }
            }
        }
        //----------------------------------------------
        private void BuildImpactAndDamageInfo()
        {
            Comparison<TriggerInfo> comp = ((left, right) => {
                if (left.Trigger.StartTime > right.Trigger.StartTime) {
                    return -1;
                } else if (left.Trigger.StartTime == right.Trigger.StartTime) {
                    return 0;
                } else {
                    return 1;
                }
            });
            //排序
            for (int i = 0; i < m_UseImpactsForInit.Count; ++i) {
                var group = m_UseImpactsForInit[i];
                Helper.BubbleSort(group.Triggers, comp);
            }
            for (int i = 0; i < m_ImpactsForInit.Count; ++i) {
                var group = m_ImpactsForInit[i];
                Helper.BubbleSort(group.Triggers, comp);
            }
            for (int i = 0; i < m_DamagesForInit.Count; ++i) {
                var group = m_DamagesForInit[i];
                Helper.BubbleSort(group.Triggers, comp);
            }
            //确定impact信息
            for (int i = 0; i < m_ImpactsForInit.Count && i < m_UseImpactsForInit.Count; ++i) {
                var refGroup = m_UseImpactsForInit[i];
                var group = m_ImpactsForInit[i];
                int impactId = 0;
                bool isExternal = false;
                long nextTime = long.MaxValue;
                int nextImpactId = 0;
                bool nextIsExternal = false;
                int refIndex = 0;
                if (refIndex < refGroup.Triggers.Count) {
                    nextTime = refGroup.Triggers[refIndex].Trigger.StartTime;
                    nextImpactId = refGroup.Triggers[refIndex].ImpactId;
                    nextIsExternal = refGroup.Triggers[refIndex].IsExternalImpact;
                }
                for (int j = 0; j < group.Triggers.Count; ++j) {
                    if (group.Triggers[j].ImpactId > 0)
                        continue;
                    long time = group.Triggers[j].Trigger.StartTime;
                    while(time >= nextTime) {
                        impactId = nextImpactId;
                        isExternal = nextIsExternal;

                        ++refIndex;
                        if (refIndex < refGroup.Triggers.Count) {
                            nextTime = refGroup.Triggers[refIndex].Trigger.StartTime;
                            nextImpactId = refGroup.Triggers[refIndex].ImpactId;
                            nextIsExternal = refGroup.Triggers[refIndex].IsExternalImpact;
                        } else {
                            nextTime = long.MaxValue;
                            break;
                        }
                    }
                    group.Triggers[j].ImpactId = impactId;
                    group.Triggers[j].IsExternalImpact = isExternal;
                    group.Triggers[j].IsEmitImpact = false;
                }
            }
            //message handler里面的impact与damage独立计数
            int ict = m_ImpactsForInit.Count;
            for (int i = 0; i < ict; ++i) {
                var group = m_ImpactsForInit[i];
                if (group.GroupType != SectionOrMessageType.Section) {
                    int jct = group.Triggers.Count;
                    for (int j = 0; j < jct; ++j) {
                        var trigger = group.Triggers[j];
                        trigger.Trigger.OrderInSection = j + 1;
                        trigger.Trigger.OrderInSkill = j + 1;
                        if (j != jct - 1) {
                            trigger.Trigger.IsFinal = false;
                        }
                    }
                }
            }
            ict = m_DamagesForInit.Count;
            for (int i = 0; i < ict; ++i) {
                var group = m_DamagesForInit[i];
                if (group.GroupType != SectionOrMessageType.Section) {
                    int jct = group.Triggers.Count;
                    for (int j = 0; j < jct; ++j) {
                        var trigger = group.Triggers[j];
                        trigger.Trigger.OrderInSection = j + 1;
                        trigger.Trigger.OrderInSkill = j + 1;
                        if (j != jct - 1) {
                            trigger.Trigger.IsFinal = false;
                        }
                    }
                }
            }
            //删除message handler
            int ct = m_ImpactsForInit.Count;
            for (int i = ct - 1; i >= 0; --i) {
                var group = m_ImpactsForInit[i];
                if (group.GroupType != SectionOrMessageType.Section) {
                    m_ImpactsForInit.Remove(group);
                }
            }
            ct = m_DamagesForInit.Count;
            for (int i = ct - 1; i >= 0; --i) {
                var group = m_DamagesForInit[i];
                if (group.GroupType != SectionOrMessageType.Section) {
                    m_DamagesForInit.Remove(group);
                }
            }
            //对section里的impact与damage按整个skill计数
            int subDamageCount = 0;
            int index = 0;
            ict = m_ImpactsForInit.Count;
            for (int i = 0; i < ict; ++i) {
                var group = m_ImpactsForInit[i];
                int jct = group.Triggers.Count;
                for (int j = 0; j < jct; ++j) {
                    var trigger = group.Triggers[j];
                    trigger.Trigger.OrderInSection = j + 1;
                    trigger.Trigger.OrderInSkill = ++index;
                    if (i != ict - 1 || j != jct - 1) {
                        trigger.Trigger.IsFinal = false;
                    }
                    if (!trigger.IsExternalImpact) {
                        if (trigger.IsEmitImpact) {
                            int innerId = GenInnerEmitSkillId(trigger.ImpactId);
                            SkillInstance inst;
                            if (null != m_EmitSkillInstances && m_EmitSkillInstances.TryGetValue(innerId, out inst)) {
                                subDamageCount += inst.DamageCount;
                            }
                        } else {
                            int innerId = GenInnerHitSkillId(trigger.ImpactId);
                            SkillInstance inst;
                            if (null != m_HitSkillInstances && m_HitSkillInstances.TryGetValue(innerId, out inst)) {
                                subDamageCount += inst.DamageCount;
                            }
                        }
                    }
                }
            }
            m_ImpactCount = index;
            index = 0;
            ict = m_DamagesForInit.Count;
            for (int i = 0; i < ict; ++i) {
                var group = m_DamagesForInit[i];
                int jct = group.Triggers.Count;
                for (int j = 0; j < jct; ++j) {
                    var trigger = group.Triggers[j];
                    trigger.Trigger.OrderInSection = j + 1;
                    trigger.Trigger.OrderInSkill = ++index;
                    if (i != ict - 1 || j != jct - 1) {
                        trigger.Trigger.IsFinal = false;
                    }
                }
            }
            m_DamageCount = index + subDamageCount;
            //信息获取完毕，清空缓存
            m_UseImpactsForInit.Clear();
            m_UseImpactsForInit = null;
            m_ImpactsForInit.Clear();
            m_ImpactsForInit = null;
            m_DamagesForInit.Clear();
            m_DamagesForInit = null;
        }
        //----------------------------------------------
        private enum SectionOrMessageType
        {
            Section = 0,
            Message,
            OnInterrupt,
            OnStop,
        }
        private sealed class TriggerInfo
        {
            internal ISkillTriger Trigger;
            internal int ImpactId;
            internal bool IsExternalImpact;
            internal bool IsEmitImpact;
        }
        private sealed class SkillSectionOrMessageTriggers
        {
            internal SectionOrMessageType GroupType = SectionOrMessageType.Section;
            internal List<TriggerInfo> Triggers = new List<TriggerInfo>();

            internal SkillSectionOrMessageTriggers(SectionOrMessageType groupType)
            {
                GroupType = groupType;
            }
        }
        //----------------------------------------------

        private bool m_IsInterrupted = false;
        private bool m_IsFinished = false;
        private bool m_IsStopCurSection = false;

        private int m_CurSection = -1;
        private int m_GoToSectionId = -1;
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

        private StrObjDict m_Variables = new StrObjDict();
        private CustomDataCollection m_CustomDatas = new CustomDataCollection();

        private int m_InnerDslSkillId = 0;
        private int m_OuterDslSkillId = 0;
        private IntSkillInstanceDict m_EmitSkillInstances = null;
        private IntSkillInstanceDict m_HitSkillInstances = null;

        private List<SkillSectionOrMessageTriggers> m_UseImpactsForInit = null;
        private List<SkillSectionOrMessageTriggers> m_ImpactsForInit = null;
        private List<SkillSectionOrMessageTriggers> m_DamagesForInit = null;

        private int m_ImpactCount = 0;
        private int m_DamageCount = 0;

        private PropertyAccessorHelper m_AccessorHelper = new PropertyAccessorHelper();
        private Dsl.FunctionData m_SkillDsl = null;

        public static int GenInnerEmitSkillId(int id)
        {
            if (id <= 0 || id > c_MaxInnerSkillId) {
                id = 1;
            }
            return (c_InnerEmitSkillIdOffset + id) * c_InnerSkillIdAmplification;
        }
        public static int GenInnerHitSkillId(int id)
        {
            if (id <= 0 || id > c_MaxInnerSkillId) {
                id = 1;
            }
            return (c_InnerHitSkillIdOffset + id) * c_InnerSkillIdAmplification;
        }

        public const int c_FirstInnerEmitSkillId = (1 + c_InnerEmitSkillIdOffset) * c_InnerSkillIdAmplification;
        public const int c_FirstInnerHitSkillId = (1 + c_InnerHitSkillIdOffset) * c_InnerSkillIdAmplification;

        private const int c_InnerSkillIdAmplification = 100000000;
        private const int c_MaxInnerSkillId = 10;
        private const int c_InnerEmitSkillIdOffset = 0;
        private const int c_InnerHitSkillIdOffset = 10;
    }
}
