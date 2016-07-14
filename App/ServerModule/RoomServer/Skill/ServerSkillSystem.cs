using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;
using GameFramework.Skill;
using GameFramework.Skill.Trigers;
using GameFrameworkMessage;

namespace GameFramework
{
    internal sealed class PredefinedSkill
    {
        internal const int c_EmitSkillId = 2110000000;
        internal const int c_HitSkillId = 2120000000;

        internal TableConfig.Skill EmitSkillCfg
        {
            get { return m_EmitSkillCfg; }
        }
        internal TableConfig.Skill HitSkillCfg
        {
            get { return m_HitSkillCfg; }
        }

        internal void ReBuild()
        {
            m_EmitSkillCfg.type = (int)SkillOrImpactType.Impact;
            AddPredefinedSkill(m_EmitSkillCfg, c_EmitSkillId);
            m_HitSkillCfg.type = (int)SkillOrImpactType.Impact;
            AddPredefinedSkill(m_HitSkillCfg, c_HitSkillId);
        }
        internal void Preload(ServerSkillSystem skillSystem)
        {
            for (int i = 0; i < c_MaxEmitSkillNum; ++i) {
                skillSystem.PreloadSkillInstance(m_EmitSkillCfg);
            }
            for (int i = 0; i < c_MaxHitSkillNum; ++i) {
                skillSystem.PreloadSkillInstance(m_HitSkillCfg);
            }
        }

        internal PredefinedSkill()
        {
            ReBuild();
        }

        private TableConfig.Skill m_EmitSkillCfg = new TableConfig.Skill();
        private TableConfig.Skill m_HitSkillCfg = new TableConfig.Skill();

        private static void AddPredefinedSkill(TableConfig.Skill cfg, int id)
        {
            cfg.id = cfg.dslSkillId = id;
            cfg.dslFile = "Skill/predefined.dsl";
            //添加到技能表数据中
            var skills = TableConfig.SkillProvider.Instance.SkillMgr.GetData();
            skills[cfg.id] = cfg;
        }

        private const int c_MaxEmitSkillNum = 10;
        private const int c_MaxHitSkillNum = 10;
    }
    internal sealed class GfxSkillSenderInfo
    {
        internal int SkillId
        {
            get { return m_ConfigData.id; }
        }
        internal TableConfig.Skill ConfigData
        {
            get { return m_ConfigData; }
        }
        internal int Seq
        {
            get { return m_Seq; }
        }
        internal int ActorId
        {
            get { return m_ActorId; }
        }
        internal EntityInfo GfxObj
        {
            get
            {
                return m_GfxObj;
            }
        }
        internal int TargetActorId
        {
            get { return m_TargetActorId; }
            set { m_TargetActorId = value; }
        }
        internal EntityInfo TargetGfxObj
        {
            get { return m_TargetGfxObj; }
            set { m_TargetGfxObj = value; }
        }
        internal Scene Scene
        {
            get { return m_Scene; }
        }
        internal GfxSkillSenderInfo(TableConfig.Skill configData, int seq, int actorId, EntityInfo gfxObj, Scene scene)
        {
            m_Seq = seq;
            m_ConfigData = configData;
            m_ActorId = actorId;
            m_GfxObj = gfxObj;
            m_Scene = scene;
        }
        internal GfxSkillSenderInfo(TableConfig.Skill configData, int seq, int actorId, EntityInfo gfxObj, int targetActorId, EntityInfo targetGfxObj, Scene scene)
            : this(configData, seq, actorId, gfxObj, scene)
        {
            m_TargetActorId = targetActorId;
            m_TargetGfxObj = targetGfxObj;
        }

        private TableConfig.Skill m_ConfigData;
        private int m_Seq;
        private int m_ActorId;
        private EntityInfo m_GfxObj;
        private int m_TargetActorId;
        private EntityInfo m_TargetGfxObj;
        private Scene m_Scene;
    }
    /// <summary>
    /// </summary>
    internal sealed class ServerSkillSystem
    {
        private class SkillInstanceInfo
        {
            internal int m_SkillId;
            internal SkillInstance m_SkillInstance;
            internal bool m_IsUsed;

            internal void Recycle()
            {
                m_SkillInstance.Reset();
                m_IsUsed = false;
            }
        }
        private class SkillLogicInfo
        {
            internal int Seq
            {
                get
                {
                    return m_Sender.Seq;
                }
            }
            internal int ActorId
            {
                get { return m_Sender.ActorId; }
            }
            internal EntityInfo GfxObj
            {
                get { return m_Sender.GfxObj; }
            }
            internal GfxSkillSenderInfo Sender
            {
                get
                {
                    return m_Sender;
                }
                set { m_Sender = value; }
            }
            internal int SkillId
            {
                get
                {
                    return m_SkillInfo.m_SkillId;
                }
            }
            internal SkillInstance SkillInst
            {
                get
                {
                    return m_SkillInfo.m_SkillInstance;
                }
            }
            internal SkillInstanceInfo Info
            {
                get
                {
                    return m_SkillInfo;
                }
            }
            internal bool IsPaused
            {
                get { return m_IsPaused; }
                set { m_IsPaused = value; }
            }

            internal SkillLogicInfo(GfxSkillSenderInfo sender, SkillInstanceInfo info)
            {
                m_Sender = sender;
                m_SkillInfo = info;
            }

            private GfxSkillSenderInfo m_Sender;
            private SkillInstanceInfo m_SkillInfo;
            private bool m_IsPaused = false;
        }

        internal Scene Scene
        {
            get { return m_Scene; }
        }
        internal PredefinedSkill PredefinedSkill
        {
            get { return m_PredefinedSkill; }
        }
        internal void Init(Scene scene)
        {
            StaticInit();
            m_Scene = scene;
        }
        internal void Reset()
        {
            int count = m_SkillLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                SkillLogicInfo info = m_SkillLogicInfos[index];
                if (null != info) {
                    info.SkillInst.OnSkillStop(info.Sender);
                    StopSkillInstance(info);
                    m_SkillLogicInfos.RemoveAt(index);
                }
            }
            m_SkillLogicInfos.Clear();
        }
        internal void PreloadSkillInstance(int skillId)
        {
            TableConfig.Skill skillData = TableConfig.SkillProvider.Instance.GetSkill(skillId);
            if (null != skillData) {
                PreloadSkillInstance(skillData);
            }
        }
        internal void PreloadSkillInstance(TableConfig.Skill skillData)
        {
            if (null != skillData) {
                SkillInstanceInfo info = NewSkillInstanceImpl(skillData.id, skillData);
                RecycleSkillInstance(info);
                if (null != info.m_SkillInstance.EmitSkillInstances) {
                    foreach (var pair in info.m_SkillInstance.EmitSkillInstances) {
                        SkillInstanceInfo iinfo = NewInnerSkillInstanceImpl(PredefinedSkill.c_EmitSkillId, pair.Value);
                        RecycleSkillInstance(iinfo);
                    }
                }
                if (null != info.m_SkillInstance.HitSkillInstances) {
                    foreach (var pair in info.m_SkillInstance.HitSkillInstances) {
                        SkillInstanceInfo iinfo = NewInnerSkillInstanceImpl(PredefinedSkill.c_HitSkillId, pair.Value);
                        RecycleSkillInstance(iinfo);
                    }
                }
            }
        }
        internal void ClearSkillInstancePool()
        {
            m_SkillInstancePool.Clear();
        }

        internal Dictionary<string, object> GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        internal SkillInstance FindSkillInstanceForSkillViewer(int skillId)
        {
            GfxSkillSenderInfo sender;
            return FindSkillInstanceForSkillViewer(skillId, out sender);
        }
        internal SkillInstance FindSkillInstanceForSkillViewer(int skillId, out GfxSkillSenderInfo sender)
        {
            SkillInstance ret = null;
            SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.SkillId == skillId);
            if (null != logicInfo) {
                sender = logicInfo.Sender;
                ret = logicInfo.SkillInst;
            } else {
                sender = null;
                var instInfo = GetUnusedSkillInstanceInfoFromPool(skillId);
                if (null != instInfo) {
                    ret = instInfo.m_SkillInstance;
                }
            }
            return ret;
        }
        internal SkillInstance FindInnerSkillInstanceForSkillViewer(int skillId, SkillInstance innerInstance)
        {
            GfxSkillSenderInfo sender;
            return FindInnerSkillInstanceForSkillViewer(skillId, innerInstance, out sender);
        }
        internal SkillInstance FindInnerSkillInstanceForSkillViewer(int skillId, SkillInstance innerInstance, out GfxSkillSenderInfo sender)
        {
            SkillInstance ret = null;
            SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.SkillId == skillId && info.SkillInst.InnerDslSkillId == innerInstance.InnerDslSkillId && info.SkillInst.OuterDslSkillId == innerInstance.OuterDslSkillId);
            if (null != logicInfo) {
                sender = logicInfo.Sender;
                ret = logicInfo.SkillInst;
            } else {
                int newSkillId = CalcUniqueInnerSkillId(skillId, innerInstance);
                sender = null;
                var instInfo = GetUnusedSkillInstanceInfoFromPool(newSkillId);
                if (null != instInfo) {
                    ret = instInfo.m_SkillInstance;
                }
            }
            return ret;
        }
        internal int GetActiveSkillCount()
        {
            return m_SkillLogicInfos.Count;
        }
        internal SkillInstance GetActiveSkillInfo(int index)
        {
            GfxSkillSenderInfo sender;
            return GetActiveSkillInfo(index, out sender);
        }
        internal SkillInstance GetActiveSkillInfo(int index, out GfxSkillSenderInfo sender)
        {
            int ct = m_SkillLogicInfos.Count;
            if (index >= 0 && index < ct) {
                var info = m_SkillLogicInfos[index];
                sender = info.Sender;
                return info.SkillInst;
            } else {
                sender = null;
                return null;
            }
        }
        internal SkillInstance FindActiveSkillInstance(int actorId, int skillId, int seq)
        {
            GfxSkillSenderInfo sender;
            return FindActiveSkillInstance(actorId, skillId, seq, out sender);
        }
        internal SkillInstance FindActiveSkillInstance(int actorId, int skillId, int seq, out GfxSkillSenderInfo sender)
        {
            SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.ActorId == actorId && info.SkillId == skillId && info.Seq == seq);
            if (null != logicInfo) {
                sender = logicInfo.Sender;
                return logicInfo.SkillInst;
            }
            sender = null;
            return null;
        }
        internal bool StartSkill(int actorId, TableConfig.Skill configData, int seq, params Dictionary<string, object>[] locals)
        {
            bool ret = false;
            if (null == configData) {
                LogSystem.Error("{0} can't cast skill, config is null !", actorId, seq);
                Helper.LogCallStack();
                return false;
            }
            if (!m_Scene.EntityController.CanCastSkill(actorId, configData, seq)) {
                m_Scene.EntityController.CancelCastSkill(actorId);
                LogSystem.Warn("{0} can't cast skill {1} {2}, cancel.", actorId, configData.id, seq);
                m_Scene.EntityController.CancelIfImpact(actorId, configData, seq);
                return false;
            }
            GfxSkillSenderInfo senderInfo = m_Scene.EntityController.BuildSkillInfo(actorId, configData, seq, m_Scene);
            if (null != senderInfo && null != senderInfo.GfxObj) {
                int skillId = senderInfo.SkillId;
                EntityInfo obj = senderInfo.GfxObj;
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (logicInfo != null) {
                    LogSystem.Warn("{0} is casting skill {1} {2}, cancel.", actorId, skillId, seq);
                    m_Scene.EntityController.CancelIfImpact(actorId, configData, seq);
                    return false;
                }
                SkillInstanceInfo inst = null;
                SkillInstance innerInstance = null;
                if (skillId == PredefinedSkill.c_EmitSkillId) {
                    for (int i = 0; i < locals.Length; ++i) {
                        object instObj;
                        if (locals[i].TryGetValue("emitskill", out instObj)) {
                            innerInstance = instObj as SkillInstance;
                        }
                    }
                    if (null == innerInstance) {
                        LogSystem.Warn("{0} use predefined skill {1} {2} but not found emitskill, cancel.", actorId, skillId, seq);
                        //m_Scene.EntityController.CancelIfImpact(actorId, configData, seq);
                        //return false;
                    }
                } else if (skillId == PredefinedSkill.c_HitSkillId) {
                    for (int i = 0; i < locals.Length; ++i) {
                        object instObj;
                        if (locals[i].TryGetValue("hitskill", out instObj)) {
                            innerInstance = instObj as SkillInstance;
                        }
                    }
                    if (null == innerInstance) {
                        LogSystem.Warn("{0} use predefined skill {1} {2} but not found hitskill, cancel.", actorId, skillId, seq);
                        //m_Scene.EntityController.CancelIfImpact(actorId, configData, seq);
                        //return false;
                    }
                }
                if (null == innerInstance) {
                    inst = NewSkillInstance(skillId, senderInfo.ConfigData);
                } else {
                    inst = NewInnerSkillInstance(skillId, innerInstance);
                }
                if (null != inst) {
                    m_SkillLogicInfos.Add(new SkillLogicInfo(senderInfo, inst));
                } else {
                    LogSystem.Warn("{0} cast skill {1} {2}, alloc failed.", actorId, skillId, seq);
                    m_Scene.EntityController.CancelIfImpact(actorId, configData, seq);
                    return false;
                }

                logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    if (null != locals) {
                        int localCount = locals.Length;
                        for (int i = 0; i < localCount; ++i) {
                            foreach (KeyValuePair<string, object> pair in locals[i]) {
                                logicInfo.SkillInst.SetVariable(pair.Key, pair.Value);
                            }
                        }
                    }
                    EntityInfo target = senderInfo.TargetGfxObj;
                    if (null != target && target != obj && configData.type == (int)SkillOrImpactType.Skill) {
                        TriggerUtil.Lookat(m_Scene, obj, target.GetMovementStateInfo().GetPosition3D());
                    }
                    m_Scene.EntityController.ActivateSkill(actorId, skillId, seq);
                    logicInfo.SkillInst.Context = m_Scene;
                    logicInfo.SkillInst.Start(logicInfo.Sender);
                    ret = true;
                }
            }
            return ret;
        }
        //在技能未开始时取消技能（用于上层逻辑检查失败时）
        internal void CancelSkill(int actorId, int skillId, int seq)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    m_Scene.EntityController.DeactivateSkill(actorId, skillId, seq);
                    RecycleSkillInstance(logicInfo.Info);
                    m_SkillLogicInfos.Remove(logicInfo);
                }
            }
        }
        internal void PauseSkill(int actorId, int skillId, int seq, bool pause)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    logicInfo.IsPaused = pause;
                }
            }
        }
        internal void PauseAllSkill(int actorId, bool pause)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null == obj) {
                return;
            }
            int count = m_SkillLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                SkillLogicInfo info = m_SkillLogicInfos[index];
                if (info != null) {
                    if (info.GfxObj == obj) {
                        info.IsPaused = pause;
                    }
                }
            }
        }
        internal void StopSkill(int actorId, int skillId, int seq, bool isinterrupt)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    if (isinterrupt) {
                        logicInfo.SkillInst.OnInterrupt(logicInfo.Sender);
                    }
                    logicInfo.SkillInst.OnSkillStop(logicInfo.Sender);
                    StopSkillInstance(logicInfo, isinterrupt);
                    m_SkillLogicInfos.Remove(logicInfo);
                }
            }
        }
        internal void StopAllSkill(int actorId, bool isinterrupt)
        {
            StopAllSkill(actorId, isinterrupt, false, false);
        }
        internal void StopAllSkill(int actorId, bool isinterrupt, bool includeImpact, bool includeBuff)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null == obj) {
                return;
            }
            int count = m_SkillLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                SkillLogicInfo info = m_SkillLogicInfos[index];
                if (info != null) {
                    if (info.GfxObj == obj) {
                        if (!includeImpact && info.Sender.ConfigData.type == (int)SkillOrImpactType.Impact)
                            continue;
                        if (!includeBuff && info.Sender.ConfigData.type == (int)SkillOrImpactType.Buff)
                            continue;
                        if (isinterrupt) {
                            info.SkillInst.OnInterrupt(info.Sender);
                        }
                        info.SkillInst.OnSkillStop(info.Sender);
                        StopSkillInstance(info, isinterrupt);
                        m_SkillLogicInfos.RemoveAt(index);
                    }
                }
            }
        }
        internal void SendMessage(int actorId, int skillId, int seq, string msgId, Dictionary<string, object> locals)
        {
            EntityInfo obj = m_Scene.EntityController.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo && null != logicInfo.SkillInst) {
                    if (null != locals) {
                        foreach (KeyValuePair<string, object> pair in locals) {
                            logicInfo.SkillInst.SetVariable(pair.Key, pair.Value);
                        }
                    }
                    logicInfo.SkillInst.SendMessage(msgId);
                }
            }
        }
        internal void Tick()
        {
            try {
                int ct = m_SkillLogicInfos.Count;
                long curTime = TimeUtility.GetLocalMicroseconds();
                if (m_LastTickTime <= 0) {
                    m_LastTickTime = curTime;
                    return;
                }
                long delta = curTime - m_LastTickTime;
                m_LastTickTime = curTime;
                for (int ix = ct - 1; ix >= 0; --ix) {
                    SkillLogicInfo info = m_SkillLogicInfos[ix];
                    bool exist = m_Scene.EntityController.ExistGameObject(info.ActorId);
                    if (exist) {
                        info.SkillInst.Tick(info.Sender, delta);
                    }
                    if (!exist || info.SkillInst.IsFinished) {
                        if (!exist) {
                            info.SkillInst.OnSkillStop(info.Sender);
                        }
                        StopSkillInstance(info);
                        m_SkillLogicInfos.RemoveAt(ix);
                    }
                }
            } finally {
            }
        }

        private void StopSkillInstance(SkillLogicInfo info)
        {
            StopSkillInstance(info, false);
        }

        private void StopSkillInstance(SkillLogicInfo info, bool isInterrupt)
        {
            if (isInterrupt) {
            }

            GameFramework.LogSystem.Debug("Skill {0} finished.", info.SkillId);
            m_Scene.EntityController.DeactivateSkill(info.ActorId, info.SkillId, info.Sender.Seq);
            RecycleSkillInstance(info.Info);
        }

        private SkillInstanceInfo NewInnerSkillInstance(int skillId, SkillInstance innerInstance)
        {
            int newSkillId = CalcUniqueInnerSkillId(skillId, innerInstance);
            if (newSkillId <= 0)
                return null;
            SkillInstanceInfo instInfo = GetUnusedSkillInstanceInfoFromPool(newSkillId);
            if (null == instInfo) {
                return NewInnerSkillInstanceImpl(skillId, innerInstance);
            } else {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }
        private SkillInstanceInfo NewSkillInstance(int skillId, TableConfig.Skill skillData)
        {
            SkillInstanceInfo instInfo = GetUnusedSkillInstanceInfoFromPool(skillId);
            if (null == instInfo) {
                if (null != skillData) {
                    return NewSkillInstanceImpl(skillId, skillData);
                } else {
                    GameFramework.LogSystem.Error("Can't find skill config, skill:{0} TableConfig.Skill is null!", skillId);
                    return null;
                }
            } else {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }
        private SkillInstanceInfo NewInnerSkillInstanceImpl(int skillId, SkillInstance innerInstance)
        {
            int newSkillId = CalcUniqueInnerSkillId(skillId, innerInstance);
            if (newSkillId <= 0)
                return null;
            SkillInstance newInst = innerInstance.Clone();
            newInst.DslSkillId = skillId;

            SkillInstanceInfo res = new SkillInstanceInfo();
            res.m_SkillId = skillId;
            res.m_SkillInstance = newInst;
            res.m_IsUsed = true;

            AddSkillInstanceInfoToPool(newSkillId, res);
            return res;
        }
        private SkillInstanceInfo NewSkillInstanceImpl(int skillId, TableConfig.Skill skillData)
        {
            string filePath = GameFramework.HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + skillData.dslFile);
            SkillConfigManager.Instance.LoadSkillIfNotExist(skillData.dslSkillId, filePath);
            SkillInstance inst = SkillConfigManager.Instance.NewSkillInstance(skillData.dslSkillId);

            if (inst == null) {
                GameFramework.LogSystem.Error("Can't load skill config, skill:{0} dsl skill:{1}!", skillId, skillData.dslSkillId);
                return null;
            }
            SkillInstanceInfo res = new SkillInstanceInfo();
            res.m_SkillId = skillId;
            res.m_SkillInstance = inst;
            res.m_IsUsed = true;

            AddSkillInstanceInfoToPool(skillId, res);
            return res;
        }
        private void RecycleSkillInstance(SkillInstanceInfo info)
        {
            info.Recycle();
        }
        private void AddSkillInstanceInfoToPool(int skillId, SkillInstanceInfo info)
        {
            List<SkillInstanceInfo> infos;
            if (m_SkillInstancePool.TryGetValue(skillId, out infos)) {
                infos.Add(info);
            } else {
                infos = new List<SkillInstanceInfo>();
                infos.Add(info);
                m_SkillInstancePool.Add(skillId, infos);
            }
        }
        private SkillInstanceInfo GetUnusedSkillInstanceInfoFromPool(int skillId)
        {
            SkillInstanceInfo info = null;
            List<SkillInstanceInfo> infos;
            if (m_SkillInstancePool.TryGetValue(skillId, out infos)) {
                int ct = infos.Count;
                for (int ix = 0; ix < ct; ++ix) {
                    if (!infos[ix].m_IsUsed) {
                        info = infos[ix];
                        break;
                    }
                }
            }
            return info;
        }

        private PredefinedSkill m_PredefinedSkill = new PredefinedSkill();
        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();

        private List<SkillLogicInfo> m_SkillLogicInfos = new List<SkillLogicInfo>();
        private Dictionary<int, List<SkillInstanceInfo>> m_SkillInstancePool = new Dictionary<int, List<SkillInstanceInfo>>();
        private Scene m_Scene = null;
        private long m_LastTickTime = 0;

        internal static int CalcUniqueInnerSkillId(int skillId, SkillInstance innerInstance)
        {
            return innerInstance.InnerDslSkillId + innerInstance.OuterDslSkillId;
        }


        internal static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                //注册技能触发器
                SkillTrigerManager.Instance.RegisterTrigerFactory("timescale", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("bornfinish", new SkillTrigerFactoryHelper<BornFinishTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("deadfinish", new SkillTrigerFactoryHelper<DeadFinishTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("sendstorymessage", new SkillTrigerFactoryHelper<SendStoryMessageTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("sendgfxmessage", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("sendgfxmessagewithtag", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("publishgfxevent", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("params", new SkillTrigerFactoryHelper<ParamsTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("keeptarget", new SkillTrigerFactoryHelper<KeepTargetTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("useimpact", new SkillTrigerFactoryHelper<UseImpactTrigger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("camerafollow", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("cameralook", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("camerafollowpath", new SkillTrigerFactoryHelper<DummyTriger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("animation", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("animationspeed", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("playsound", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("stopsound", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("selfeffect", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("targeteffect", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("sceneeffect", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("emiteffect", new SkillTrigerFactoryHelper<EmitEffectTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("aoeemiteffect", new SkillTrigerFactoryHelper<AoeEmitEffectTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("stopeffect", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("lockframe", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("hiteffect", new SkillTrigerFactoryHelper<HitEffectTriger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("fadecolor", new SkillTrigerFactoryHelper<DummyTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("replaceshaderandfadecolor", new SkillTrigerFactoryHelper<DummyTriger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("enablemoveagent", new SkillTrigerFactoryHelper<EnableMoveAgentTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("charge", new SkillTrigerFactoryHelper<ChargeTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("jump", new SkillTrigerFactoryHelper<JumpTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("curvemove", new SkillTrigerFactoryHelper<CurveMovementTrigger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("damage", new SkillTrigerFactoryHelper<DamageTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("addstate", new SkillTrigerFactoryHelper<AddStateTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("removestate", new SkillTrigerFactoryHelper<RemoveStateTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("addshield", new SkillTrigerFactoryHelper<AddShieldTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("removeshield", new SkillTrigerFactoryHelper<RemoveShieldTriger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("bufftotarget", new SkillTrigerFactoryHelper<BuffToTargetTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("bufftoself", new SkillTrigerFactoryHelper<BuffToSelfTrigger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("impact", new SkillTrigerFactoryHelper<ImpactTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("aoeimpact", new SkillTrigerFactoryHelper<AoeImpactTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("periodicallyaoeimpact", new SkillTrigerFactoryHelper<PeriodicallyAoeImpactTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("chainaoeimpact", new SkillTrigerFactoryHelper<ChainAoeImpactTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("periodicallyimpact", new SkillTrigerFactoryHelper<PeriodicallyImpactTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("track", new SkillTrigerFactoryHelper<TrackTriger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("colliderimpact", new SkillTrigerFactoryHelper<ColliderImpactTriger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("selecttarget", new SkillTrigerFactoryHelper<SelectTargetTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("facetotarget", new SkillTrigerFactoryHelper<FaceToTargetTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("cleartargets", new SkillTrigerFactoryHelper<ClearTargetsTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("rotate", new SkillTrigerFactoryHelper<RotateTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("transform", new SkillTrigerFactoryHelper<TransformTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("teleport", new SkillTrigerFactoryHelper<TeleportTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("follow", new SkillTrigerFactoryHelper<FollowTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("storepos", new SkillTrigerFactoryHelper<StorePosTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("restorepos", new SkillTrigerFactoryHelper<RestorePosTrigger>());

                SkillTrigerManager.Instance.RegisterTrigerFactory("adjustsectionduration", new SkillTrigerFactoryHelper<AdjustSectionDurationTrigger>());
                SkillTrigerManager.Instance.RegisterTrigerFactory("keepsectionforbuff", new SkillTrigerFactoryHelper<KeepSectionForBuffTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopsection", new SkillTrigerFactoryHelper<StopSectionTrigger>());
            }
        }

        private static bool s_IsInited = false;
    }
}
