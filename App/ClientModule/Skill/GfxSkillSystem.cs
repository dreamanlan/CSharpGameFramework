using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill
{
    public sealed class PredefinedSkill
    {
        public TableConfig.Skill EmitSkillCfg
        {
            get { return m_EmitSkillCfg; }
        }
        public TableConfig.Skill HitSkillCfg
        {
            get { return m_HitSkillCfg; }
        }

        public void Preload()
        {
            for (int i = 0; i < c_MaxEmitSkillNum; ++i) {
                GfxSkillSystem.Instance.PreloadSkillInstance(m_EmitSkillCfg);
            }
            for (int i = 0; i < c_MaxHitSkillNum; ++i) {
                GfxSkillSystem.Instance.PreloadSkillInstance(m_HitSkillCfg);
            }
        }
        
        private PredefinedSkill()
        {
            m_EmitSkillCfg.type = (int)SkillOrImpactType.Impact;
            AddPredefinedSkill(m_EmitSkillCfg, PredefinedSkillEnum.EmitSkillId);
            m_HitSkillCfg.type = (int)SkillOrImpactType.Impact;
            AddPredefinedSkill(m_HitSkillCfg, PredefinedSkillEnum.HitSkillId);
        }
        private enum PredefinedSkillEnum
        {
            EmitSkillId = 90000,
            HitSkillId,
        }

        private TableConfig.Skill m_EmitSkillCfg = new TableConfig.Skill();
        private TableConfig.Skill m_HitSkillCfg = new TableConfig.Skill();

        private static void AddPredefinedSkill(TableConfig.Skill cfg, PredefinedSkillEnum id)
        {
            cfg.id = cfg.dslSkillId = (int)id;
            cfg.dslFile = "Skill/predefined.dsl";
        }
        
        private const int c_MaxEmitSkillNum = 10;
        private const int c_MaxHitSkillNum = 10;
        public static PredefinedSkill Instance
        {
            get {return s_Instance;}
        }
        private static PredefinedSkill s_Instance = new PredefinedSkill();
    }
    public sealed class GfxSkillSenderInfo
    {
        public int SkillId
        {
            get { return m_ConfigData.id; }
        }
        public TableConfig.Skill ConfigData
        {
            get { return m_ConfigData; }
        }
        public int Seq
        {
            get { return m_Seq; }
        }
        public int ActorId
        {
            get { return m_ActorId; }
        }
        public GameObject GfxObj
        {
            get
            {
                return m_GfxObj;
            }
        }
        public int TargetActorId
        {
            get { return m_TargetActorId; }
            set { m_TargetActorId = value; }
        }
        public GameObject TargetGfxObj
        {
            get { return m_TargetGfxObj; }
            set { m_TargetGfxObj = value; }
        }
        public GfxSkillSenderInfo(TableConfig.Skill configData, int seq, int actorId, GameObject gfxObj)
        {
            m_Seq = seq;
            m_ConfigData = configData;
            m_ActorId = actorId;
            m_GfxObj = gfxObj;
        }
        public GfxSkillSenderInfo(TableConfig.Skill configData, int seq, int actorId, GameObject gfxObj, int targetActorId, GameObject targetGfxObj)
            : this(configData, seq, actorId, gfxObj)
        {
            m_TargetActorId = targetActorId;
            m_TargetGfxObj = targetGfxObj;
        }

        private TableConfig.Skill m_ConfigData;
        private int m_Seq;
        private int m_ActorId;
        private GameObject m_GfxObj;
        private int m_TargetActorId;
        private GameObject m_TargetGfxObj;
    }
    public sealed class GfxSkillSystem
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
            public int Seq
            {
                get
                {
                    return m_Sender.Seq;
                }
            }
            public int ActorId
            {
                get { return m_Sender.ActorId; }
            }
            public GameObject GfxObj
            {
                get { return m_Sender.GfxObj; }
            }
            public GfxSkillSenderInfo Sender
            {
                get
                {
                    return m_Sender;
                }
                set { m_Sender = value; }
            }
            public int SkillId
            {
                get
                {
                    return m_SkillInfo.m_SkillId;
                }
            }
            public SkillInstance SkillInst
            {
                get
                {
                    return m_SkillInfo.m_SkillInstance;
                }
            }
            public SkillInstanceInfo Info
            {
                get
                {
                    return m_SkillInfo;
                }
            }

            public SkillLogicInfo(GfxSkillSenderInfo sender, SkillInstanceInfo info)
            {
                m_Sender = sender;
                m_SkillInfo = info;
            }

            private GfxSkillSenderInfo m_Sender;
            private SkillInstanceInfo m_SkillInfo;
        }
        public void Init()
        {
            //注册技能触发器
            SkillTrigerManager.Instance.RegisterTrigerFactory("timescale", new SkillTrigerFactoryHelper<Trigers.TimeScaleTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("bornfinish", new SkillTrigerFactoryHelper<Trigers.BornFinishTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("deadfinish", new SkillTrigerFactoryHelper<Trigers.DeadFinishTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sendstorymessage", new SkillTrigerFactoryHelper<Trigers.SendStoryMessageTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sendgfxmessage", new SkillTrigerFactoryHelper<Trigers.SendGfxMessageTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sendgfxmessagewithtag", new SkillTrigerFactoryHelper<Trigers.SendGfxMessageWithTagTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("publishgfxevent", new SkillTrigerFactoryHelper<Trigers.PublishGfxEventTrigger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("camerafollow", new SkillTrigerFactoryHelper<Trigers.CameraFollowTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("cameralook", new SkillTrigerFactoryHelper<Trigers.CameraLookTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("animation", new SkillTrigerFactoryHelper<Trigers.AnimationTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("animationspeed", new SkillTrigerFactoryHelper<Trigers.AnimationSpeedTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("playsound", new SkillTrigerFactoryHelper<Trigers.PlaySoundTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopsound", new SkillTrigerFactoryHelper<Trigers.StopSoundTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("selfeffect", new SkillTrigerFactoryHelper<Trigers.SelfEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("targeteffect", new SkillTrigerFactoryHelper<Trigers.TargetEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sceneeffect", new SkillTrigerFactoryHelper<Trigers.SceneEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("emiteffect", new SkillTrigerFactoryHelper<Trigers.EmitEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("aoeemiteffect", new SkillTrigerFactoryHelper<Trigers.AoeEmitEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopeffect", new SkillTrigerFactoryHelper<Trigers.StopEffectTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("lockframe", new SkillTrigerFactoryHelper<Trigers.LockFrameTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("hiteffect", new SkillTrigerFactoryHelper<Trigers.HitEffectTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("fadecolor", new SkillTrigerFactoryHelper<Trigers.FadeColorTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("replaceshaderandfadecolor", new SkillTrigerFactoryHelper<Trigers.ReplaceShaderAndFadeColorTrigger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("enablemoveagent", new SkillTrigerFactoryHelper<Trigers.EnableMoveAgentTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("charge", new SkillTrigerFactoryHelper<Trigers.ChargeTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("jump", new SkillTrigerFactoryHelper<Trigers.JumpTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("curvemove", new SkillTrigerFactoryHelper<Trigers.CurveMovementTrigger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("consume", new SkillTrigerFactoryHelper<Trigers.ConsumeTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("damage", new SkillTrigerFactoryHelper<Trigers.DamageTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addstate", new SkillTrigerFactoryHelper<Trigers.AddStateTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("removestate", new SkillTrigerFactoryHelper<Trigers.RemoveStateTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addshield", new SkillTrigerFactoryHelper<Trigers.AddShieldTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("removeshield", new SkillTrigerFactoryHelper<Trigers.RemoveShieldTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("impact", new SkillTrigerFactoryHelper<Trigers.ImpactTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("aoeimpact", new SkillTrigerFactoryHelper<Trigers.AoeImpactTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("periodicallyaoeimpact", new SkillTrigerFactoryHelper<Trigers.PeriodicallyAoeImpactTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("chainaoeimpact", new SkillTrigerFactoryHelper<Trigers.ChainAoeImpactTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("periodicallyimpact", new SkillTrigerFactoryHelper<Trigers.PeriodicallyImpactTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("track", new SkillTrigerFactoryHelper<Trigers.TrackTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("colliderimpact", new SkillTrigerFactoryHelper<Trigers.ColliderImpactTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("selecttarget", new SkillTrigerFactoryHelper<Trigers.SelectTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("facetotarget", new SkillTrigerFactoryHelper<Trigers.FaceToTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("cleartargets", new SkillTrigerFactoryHelper<Trigers.ClearTargetsTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("rotate", new SkillTrigerFactoryHelper<Trigers.RotateTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("transform", new SkillTrigerFactoryHelper<Trigers.TransformTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("teleport", new SkillTrigerFactoryHelper<Trigers.TeleportTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("follow", new SkillTrigerFactoryHelper<Trigers.FollowTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("storepos", new SkillTrigerFactoryHelper<Trigers.StorePosTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("restorepos", new SkillTrigerFactoryHelper<Trigers.RestorePosTrigger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("adjustsectionduration", new SkillTrigerFactoryHelper<Trigers.AdjustSectionDurationTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("keepsectionforbuff", new SkillTrigerFactoryHelper<Trigers.KeepSectionForBuffTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopsectionif", new SkillTrigerFactoryHelper<Trigers.StopSectionIfTrigger>());
        }
        public void Reset()
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
        public void PreloadSkillInstance(int skillId)
        {
            TableConfig.Skill skillData = TableConfig.SkillProvider.Instance.GetSkill(skillId);
            if (null != skillData) {
                PreloadSkillInstance(skillData);
            }
        }
        public void PreloadSkillInstance(TableConfig.Skill skillData)
        {
            if (null != skillData) {
                SkillInstanceInfo info = NewSkillInstanceImpl(skillData.id, skillData);
                RecycleSkillInstance(info);
            }
        }
        public void ClearSkillInstancePool()
        {
            m_SkillInstancePool.Clear();
        }

        public Dictionary<string, object> GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        public SkillInstance GetSkillInstance(int actorId, int skillId, int seq)
        {
            GfxSkillSenderInfo sender;
            return GetSkillInstance(actorId, skillId, seq, out sender);
        }
        public SkillInstance GetSkillInstance(int actorId, int skillId, int seq, out GfxSkillSenderInfo sender)
        {
            SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.ActorId == actorId && info.SkillId == skillId && info.Seq == seq);
            if (null != logicInfo) {
                sender = logicInfo.Sender;
                return logicInfo.SkillInst;
            }
            sender = null;
            return null;
        }
        public bool StartSkill(int actorId, TableConfig.Skill configData, int seq)
        {
            return StartSkill(actorId, configData, seq, null);
        }
        public bool StartSkill(int actorId, TableConfig.Skill configData, int seq, params Dictionary<string, object>[] locals)
        {
            bool ret = false;
            if (!EntityController.Instance.CanCastSkill(actorId, configData, seq)) {
                EntityController.Instance.CancelCastSkill(actorId);
                LogSystem.Warn("{0} can't cast skill {1} {2}, cancel.", actorId, configData.id, seq);
                return false;
            }
            GfxSkillSenderInfo senderInfo = EntityController.Instance.BuildSkillInfo(actorId, configData, seq);
            if (null != senderInfo && null != senderInfo.GfxObj) {
                int skillId = senderInfo.SkillId;
                GameObject obj = senderInfo.GfxObj;
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (logicInfo != null) {
                    LogSystem.Warn("{0} is casting skill {1} {2}, cancel.", actorId, skillId, seq);
                    return false;
                }
                SkillInstanceInfo inst = NewSkillInstance(skillId, senderInfo.ConfigData);
                if (null != inst) {
                    m_SkillLogicInfos.Add(new SkillLogicInfo(senderInfo, inst));
                } else {
                    LogSystem.Warn("{0} cast skill {1} {2}, alloc failed.", actorId, skillId, seq);
                    return false;
                }

                logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    logicInfo.SkillInst.GlobalVariables = m_GlobalVariables;
                    if (null != locals) {
                        int localCount = locals.Length;
                        for (int i = 0; i < localCount; ++i) {
                            foreach (KeyValuePair<string, object> pair in locals[i]) {
                                logicInfo.SkillInst.SetLocalVariable(pair.Key, pair.Value);
                            }
                        }
                    }
                    GameObject target = senderInfo.TargetGfxObj;
                    if (null != target && target != obj && configData.type == (int)SkillOrImpactType.Skill) {
                        Trigers.TriggerUtil.Lookat(obj, target.transform.position);
                    }
                    EntityController.Instance.ActivateSkill(actorId, skillId, seq);
                    logicInfo.SkillInst.Start(logicInfo.Sender);
                    ret = true;
                }
            }
            return ret;
        }
        //在技能未开始时取消技能（用于上层逻辑检查失败时）
        public void CancelSkill(int actorId, int skillId, int seq)
        {
            GameObject obj = EntityController.Instance.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo) {
                    EntityController.Instance.DeactivateSkill(actorId, skillId, seq);
                    RecycleSkillInstance(logicInfo.Info);
                    m_SkillLogicInfos.Remove(logicInfo);
                }
            }
        }
        public void StopSkill(int actorId, int skillId, int seq, bool isinterrupt)
        {
            GameObject obj = EntityController.Instance.GetGameObject(actorId);
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
        public void StopAllSkill(int actorId, bool isinterrupt)
        {
            StopAllSkill(actorId, isinterrupt, false, false);
        }
        public void StopAllSkill(int actorId, bool isinterrupt, bool includeImpact, bool includeBuff)
        {
            GameObject obj = EntityController.Instance.GetGameObject(actorId);
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
                        if(!includeBuff && info.Sender.ConfigData.type == (int)SkillOrImpactType.Buff)
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
        public void SendMessage(int actorId, int skillId, int seq, string msgId, Dictionary<string, object> locals)
        {
            GameObject obj = EntityController.Instance.GetGameObject(actorId);
            if (null != obj) {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.GfxObj == obj && info.SkillId == skillId && info.Seq == seq);
                if (null != logicInfo && null != logicInfo.SkillInst) {
                    if (null != locals) {
                        foreach (KeyValuePair<string, object> pair in locals) {
                            logicInfo.SkillInst.SetLocalVariable(pair.Key, pair.Value);
                        }
                    }
                    logicInfo.SkillInst.SendMessage(msgId);
                }
            }
        }
        public void Tick()
        {
            try {
                UnityEngine.Profiler.BeginSample("GfxSkillSystem.Tick");
                int ct = m_SkillLogicInfos.Count;
                long delta = (long)(Time.deltaTime * 1000000);
                for (int ix = ct - 1; ix >= 0; --ix) {
                    SkillLogicInfo info = m_SkillLogicInfos[ix];
                    bool exist = EntityController.Instance.ExistGameObject(info.GfxObj);
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
                UnityEngine.Profiler.EndSample();
            }
        }

        private void StopSkillInstance(SkillLogicInfo info)
        {
            StopSkillInstance(info, false);
        }

        private void StopSkillInstance(SkillLogicInfo info, bool isInterrupt)
        {
            if (isInterrupt) {
                Trigers.EffectManager effectMgr = info.SkillInst.CustomDatas.GetData<Trigers.EffectManager>();
                if (null != effectMgr) {
                    effectMgr.StopEffects();
                }
                if (info.Sender.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    EntityController.Instance.StopSkillAnimation(info.ActorId);
                }
            }

            //GameFramework.LogSystem.Debug("Skill {0} finished.", info.SkillId);
            EntityController.Instance.DeactivateSkill(info.ActorId, info.SkillId, info.Sender.Seq);
            RecycleSkillInstance(info.Info);
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
        private SkillInstanceInfo NewSkillInstanceImpl(int skillId, TableConfig.Skill skillData)
        {
            string filePath = GameFramework.HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + skillData.dslFile);
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

        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();

        private List<SkillLogicInfo> m_SkillLogicInfos = new List<SkillLogicInfo>();
        private Dictionary<int, List<SkillInstanceInfo>> m_SkillInstancePool = new Dictionary<int, List<SkillInstanceInfo>>();

        public static GfxSkillSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxSkillSystem s_Instance = new GfxSkillSystem();
    }
}
