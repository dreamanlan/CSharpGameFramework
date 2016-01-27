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
            AbstractAiStateLogic.OnAiFace += this.OnAiFace;
            AbstractAiStateLogic.OnAiMove += this.OnAiMove;
            AbstractAiStateLogic.OnAiSkill += this.OnAiSkill;
            AbstractAiStateLogic.OnAiStopSkill += this.OnAiStopSkill;
            AbstractAiStateLogic.OnAiAddImpact += this.OnAiAddImpact;
            AbstractAiStateLogic.OnAiRemoveImpact += this.OnAiRemoveImpact;
            AbstractAiStateLogic.OnAiMeetEnemy += this.OnAiMeetEnemy;
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
                    aiInfo.AiStoryInstanceInfo.m_StoryInstance.Start();
                }
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
        private void OnAiMove(EntityInfo entity)
        {
            if (null != entity) {
                MovementStateInfo msi = entity.GetMovementStateInfo();
                EntityViewModel npcViewModel = EntityViewModelManager.Instance.GetEntityViewById(entity.GetId());
                if (!msi.IsMoving) {
                    npcViewModel.StopMove();
                } else {
                    npcViewModel.MoveTo(msi.TargetPosition.X, msi.TargetPosition.Y, msi.TargetPosition.Z);
                }
            }
        }
        private void OnAiSkill(EntityInfo entity, int skillId)
        {
            if (null != entity) {
                SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    if (GfxSkillSystem.Instance.StartSkill(entity.GetId(), skillInfo.ConfigData, 0)) {
                        Utility.EventSystem.Publish("ui_skill_cooldown", "ui", entity.GetId(), skillId, skillInfo.ConfigData.cooldown);
                    }
                }
            }
        }
        private void OnAiStopSkill(EntityInfo entity)
        {
            if (null != entity) {
                GfxSkillSystem.Instance.StopAllSkill(entity.GetId(), true);
            }
        }
        private void OnAiAddImpact(EntityInfo entity, int impactId)
        {
        }
        private void OnAiRemoveImpact(EntityInfo entity, int impactId)
        {
        }
        private void OnAiMeetEnemy(EntityInfo entity)
        {
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
