using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public sealed class ImpactSystem
    {
        public void Tick(EntityInfo obj)
        {
            if (null == obj) return;
            List<ImpactInfo> infos = obj.GetSkillStateInfo().GetAllImpact();
            for (int i = infos.Count - 1; i >= 0; --i) {
                ImpactInfo info = infos[i];
                // The impact list is shared with the gfx skill pipeline (hit skill / emit skill
                // impacts added by EntityController.SendImpact/TrackImpact). Those have
                // ImpactLogicId == 0 and IsActivated == false by default; never touch them here.
                if (info.ImpactLogicId <= 0) continue;
                IImpactLogic logic = GetLogic(info);
                if (info.IsActivated) {
                    if (null != logic) logic.Tick(obj, info.ImpactId);
                } else {
                    ApplyState(obj, info, false);
                    if (null != logic) logic.OnInterrupted(obj, info.ImpactId);
                    obj.GetSkillStateInfo().RemoveImpactByImpactId(info.ImpactId);
                }
            }
            // This framework has no check-copy impact list; removal happens inline above.
        }

        public bool SendImpactToCharacter(EntityInfo sender, int impactId, int targetId, int skillId, int duration, Vector3 srcPos, float srcDir)
        {
            bool ret = SendImpactImpl(sender, impactId, targetId, skillId, duration);
            if (ret) {
                SceneContextInfo ctx = null != sender ? sender.SceneContext : null;
                EntityInfo target = ImpactCompat.GetEntity(ctx, targetId);
                if (null != target) OnAddImpact(target, impactId);
                if (null != ImpactEvents.EventSendImpact) {
                    ImpactEvents.EventSendImpact(sender, targetId, impactId, srcPos, srcDir);
                }
            }
            return ret;
        }

        public bool StopImpactById(EntityInfo target, int impactId)
        {
            bool ret = StopImpactImpl(target, impactId);
            if (ret && null != ImpactEvents.EventStopImpact) {
                ImpactEvents.EventStopImpact(null, target.GetId(), impactId);
            }
            return ret;
        }

        public void StopAllImpact(EntityInfo target)
        {
            if (null == target) return;
            List<ImpactInfo> infos = target.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < infos.Count; ++i) {
                if (infos[i].ImpactType != (int)ImpactType.PASSIVE) {
                    StopImpactImpl(target, infos[i].ImpactId);
                }
            }
            if (null != ImpactEvents.EventStopAllImpact) {
                ImpactEvents.EventStopAllImpact(null, target.GetId(), -1);
            }
        }

        public bool IsImpactControlTarget(EntityInfo target)
        {
            if (null == target) return false;
            List<ImpactInfo> infos = target.GetSkillStateInfo().GetAllImpact();
            for (int i = infos.Count - 1; i >= 0; --i) {
                if (infos[i].IsGfxControl) return true;
            }
            return false;
        }

        public void OnGfxStopImpact(EntityInfo target, int impactId)
        {
            if (null == target) return;
            ImpactInfo info = target.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return;
            info.IsGfxControl = false;
            EntityInfo sender = ImpactCompat.GetEntity(target.SceneContext, info.ImpactSenderId);
            if (null != ImpactEvents.EventGfxStopImpact) {
                ImpactEvents.EventGfxStopImpact(sender, target.GetId(), impactId);
            }
        }

        private void OnAddImpact(EntityInfo target, int impactId)
        {
            List<ImpactInfo> infos = target.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < infos.Count; ++i) {
                if (infos[i].ImpactId == impactId) continue;
                IImpactLogic logic = GetLogic(infos[i]);
                if (null != logic) logic.OnAddImpact(target, infos[i].ImpactId, impactId);
            }
        }

        private bool StopImpactImpl(EntityInfo target, int impactId)
        {
            if (null == target) return false;
            ImpactInfo info = target.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return false;
            IImpactLogic logic = GetLogic(info);
            if (null != logic && logic.CanInterrupt(target, impactId)) {
                ApplyState(target, info, false);
                logic.OnInterrupted(target, impactId);
                info.IsActivated = false;
                return true;
            }
            return false;
        }

        private bool SendImpactImpl(EntityInfo sender, int impactId, int targetId, int skillId, int duration)
        {
            SceneContextInfo ctx = null != sender ? sender.SceneContext : null;
            EntityInfo target = ImpactCompat.GetEntity(ctx, targetId);
            if (null == target || ImpactCompat.IsDead(target)) return false;
            ImpactLogicData cfg = ImpactLogicDataProvider.Instance.GetImpactLogicData(impactId);
            if (null == cfg) return false;
            IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(cfg.LogicId);
            if (null == logic) return false;

            ImpactInfo old = target.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != old) {
                ApplyState(target, old, false);
                logic.OnInterrupted(target, impactId);
                target.GetSkillStateInfo().RemoveImpactByImpactId(impactId);
            }

            ImpactInfo info = new ImpactInfo(impactId);
            info.IsActivated = true;
            info.ImpactLogicId = cfg.LogicId;
            info.ImpactType = cfg.ImpactType;
            info.BuffDataId = cfg.BuffDataId;
            info.StateId = cfg.StateId;
            info.SkillId = skillId;
            info.ImpactSenderId = null != sender ? sender.GetId() : -1;
            info.StartTime = ImpactTimeUtility.NowMs();
            info.DurationTime = cfg.ImpactTime;
            if (-1 != duration && duration <= cfg.ImpactTime) info.DurationTime = duration;
            info.HasEffectApplyed = false;
            info.IsGfxControl = cfg.GfxLogicId > 0 || cfg.StateId > 0;
            info.MaxMoveDistance = cfg.CalcMaxMoveDistance();
            info.LeftEnableMoveCount = cfg.GfxLogicId > 0 ? 1 : 0;
            info.ExtraParams.Clear();
            info.ExtraParams.AddRange(cfg.ExtraParams);
            info.EffectList.Clear();
            info.EffectList.AddRange(cfg.EffectList);

            target.GetSkillStateInfo().AddImpact(info);
            ApplyState(target, info, true);
            logic.StartImpact(target, impactId);
            if (info.ImpactType == (int)ImpactType.INSTANT) info.IsActivated = false;
            return true;
        }

        private static IImpactLogic GetLogic(ImpactInfo info)
        {
            if (null == info || info.ImpactLogicId <= 0) return null;
            return ImpactLogicManager.Instance.GetImpactLogic(info.ImpactLogicId);
        }

        // stateId carries the control flags applied by the impact for its lifetime.
        private static void ApplyState(EntityInfo obj, ImpactInfo info, bool on)
        {
            if (null == obj || null == info || info.StateId <= 0) return;
            CharacterPropertyEnum state = (CharacterPropertyEnum)info.StateId;
            if (state < CharacterPropertyEnum.x3001_眩晕 || state >= CharacterPropertyEnum.x4001_职业) return;
            ImpactCompat.SetState(obj, state, on);
        }

        private ImpactSystem() { }
        public static ImpactSystem Instance { get { return s_Instance; } }
        private static ImpactSystem s_Instance = new ImpactSystem();
    }
}
