using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Story;
using GameFramework.Skill;
using UnityEngine;

namespace GameFramework
{
    internal sealed class AiViewModelManager
    {
        internal void Init()
        {
            AbstractAiStateLogic.OnAiInitDslLogic += this.OnAiInitDslLogic;
            AbstractAiStateLogic.OnAiTarget += this.OnAiTarget;
            AbstractAiStateLogic.OnAiFace += this.OnAiFace;
            AbstractAiStateLogic.OnAiPursue += this.OnAiPursue;
            AbstractAiStateLogic.OnAiStopPursue += this.OnAiStopPursue;
            AbstractAiStateLogic.OnAiSelectSkill += this.OnAiSelectSkill;
            AbstractAiStateLogic.OnAiDead += this.OnDeadNotify;
            AbstractAiStateLogic.OnAiSkill += this.OnAiSkill;
            AbstractAiStateLogic.OnAiStopSkill += this.OnAiStopSkill;
            AbstractAiStateLogic.OnAiAddImpact += this.OnAiAddImpact;
            AbstractAiStateLogic.OnAiRemoveImpact += this.OnAiRemoveImpact;
            AbstractAiStateLogic.OnAiSendStoryMessage += this.OnAiSendStoryMessage;
        }

        private void OnAiInitDslLogic(EntityInfo entity)
        {
            AiStateInfo aiInfo = entity.GetAiStateInfo();
            if (aiInfo.AiParam.Length >= 2) {
                string storyId = aiInfo.AiParam[0];
                string storyFile = aiInfo.AiParam[1];
                if (!string.IsNullOrEmpty(storyId) && !string.IsNullOrEmpty(storyFile)) {
                    aiInfo.AiStoryInstanceInfo = GfxStorySystem.Instance.NewAiStoryInstance(storyId, string.Empty, storyFile);
                    if (null != aiInfo.AiStoryInstanceInfo) {
                        aiInfo.AiStoryInstanceInfo.m_StoryInstance.SetVariable("@objid", entity.GetId());
                        aiInfo.AiStoryInstanceInfo.m_StoryInstance.Start();
                    }
                }
            }
        }
        private void OnAiTarget(EntityInfo npc, EntityInfo target)
        {
            if (null != target) {
                if (null != ClientModule.Instance.SelectedTarget) {
                    EntityInfo curTarget = ClientModule.Instance.GetEntityById(ClientModule.Instance.SelectedTarget.TargetId);
                    if (curTarget == ClientModule.Instance.SelectedTarget.Target) {
                        return;
                    }
                }
                ClientModule.Instance.SetLockTarget(target.GetId());
            }
        }
        private void OnAiFace(EntityInfo entity)
        {
            if (null != entity && entity.EntityType != (int)EntityTypeEnum.Tower) {
                float dir = entity.GetMovementStateInfo().GetFaceDir();
                GameObject actor = EntityController.Instance.GetGameObject(entity.GetId());
                actor.transform.localRotation = Quaternion.Euler(0, Utility.RadianToDegree(dir), 0);
            }
        }
        private void OnAiPursue(EntityInfo entity, ScriptRuntime.Vector3 target)
        {
            if (null != entity) {
                EntityViewModel npcViewModel = EntityViewModelManager.Instance.GetEntityViewById(entity.GetId());
                npcViewModel.MoveTo(target.X, target.Y, target.Z);
            }
        }
        private void OnAiStopPursue(EntityInfo entity)
        {
            EntityViewModel npcView = EntityViewModelManager.Instance.GetEntityViewById(entity.GetId());
            npcView.StopMove();
        }
        private void OnAiSelectSkill(EntityInfo npc,SkillInfo skill)
        {
            if(skill == null)
                npc.GetSkillStateInfo().SetCurSkillInfo(0);
            else
                npc.GetSkillStateInfo().SetCurSkillInfo(skill.SkillId);
        }
        private void OnAiSkill(EntityInfo entity, int skillId)
        {
            if (null != entity) {
                SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    if (GfxSkillSystem.Instance.StartSkill(entity.GetId(), skillInfo.ConfigData, 0)) {
                        Utility.EventSystem.Publish("ui_skill_cooldown", "ui", entity.GetId(), skillId, skillInfo.ConfigData.cooldown / 1000.0f);
                    }
                }
            }
        }
        private void OnAiStopSkill(EntityInfo npc)
        {
            if (null != npc) {
                GfxSkillSystem.Instance.StopAllSkill(npc.GetId(), false);
            }
        }
        private void OnAiAddImpact(EntityInfo entity, int impactId)
        {
            ImpactInfo impactInfo = new ImpactInfo(impactId);
            impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
            impactInfo.ImpactSenderId = entity.GetId();
            impactInfo.SkillId = 0;
            if (null != impactInfo.ConfigData) {
                entity.GetSkillStateInfo().AddImpact(impactInfo);
                int seq = impactInfo.Seq;
                if (GfxSkillSystem.Instance.StartSkill(entity.GetId(), impactInfo.ConfigData, seq)) {
                }
            }                
        }
        private void OnAiRemoveImpact(EntityInfo entity, int impactId)
        {
            ImpactInfo impactInfo = entity.GetSkillStateInfo().FindImpactInfoById(impactId);
            if (null != impactInfo) {
                GfxSkillSystem.Instance.StopSkill(entity.GetId(), impactId, impactInfo.Seq, false);
            }
        }
        private void OnDeadNotify(EntityInfo npc)
        {
            EntityViewModel view = EntityController.Instance.GetEntityViewById(npc.GetId());
            view.Death();
        }
        private void OnAiSendStoryMessage(EntityInfo entity, string msgId, object[] args)
        {
            GfxStorySystem.Instance.SendMessage(msgId, args);
        }

        private int m_ActorLayerMask = 0;

        private AiViewModelManager() 
        {
            m_ActorLayerMask = 1 << LayerMask.NameToLayer("Actor");
        }

        internal static AiViewModelManager Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static AiViewModelManager s_Instance = new AiViewModelManager();
    }
}
