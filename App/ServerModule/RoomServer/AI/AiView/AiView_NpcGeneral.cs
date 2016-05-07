using System;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace GameFramework
{
    internal class AiView_NpcGeneral
    {
        internal AiView_NpcGeneral()
        {
            AbstractAiStateLogic.OnAiPursue += this.OnAiPursue;
            AbstractAiStateLogic.OnAiStopPursue += this.OnAiStopPursue;
            AbstractAiStateLogic.OnAiFace += this.OnAiFace;
            AbstractAiStateLogic.OnAiSelectSkill += this.OnAiSelectSkill;
            AbstractAiStateLogic.OnAiSkill += this.OnAiSkill;
            AbstractAiStateLogic.OnAiStopSkill += this.OnAiStopSkill;
            AbstractAiStateLogic.OnAiAddImpact += this.OnAiAddImpact;
            AbstractAiStateLogic.OnAiRemoveImpact += this.OnAiRemoveImpact;
            AbstractAiStateLogic.OnAiSendStoryMessage += this.OnAiSendStoryMessage;
        }
        private void OnAiPursue(EntityInfo npc, ScriptRuntime.Vector3 target)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                npc.GetMovementStateInfo().TargetPosition = target;
                float dir = Geometry.GetYRadian(npc.GetMovementStateInfo().GetPosition3D(), target);
                npc.GetMovementStateInfo().SetFaceDir(dir);
                npc.GetMovementStateInfo().SetMoveDir(dir);
                npc.GetMovementStateInfo().IsMoving = true;

                if (npc.GetMovementStateInfo().IsMoveStatusChanged) {
                    npc.GetMovementStateInfo().IsMoveStatusChanged = false;

                    Msg_RC_NpcMove npcMoveBuilder = DataSyncUtility.BuildNpcMoveMessage(npc);
                    if (null != npcMoveBuilder)
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcMove, npcMoveBuilder);
                }
            }
        }
        private void OnAiStopPursue(EntityInfo npc)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                npc.GetMovementStateInfo().IsMoving = false;

                if (npc.GetMovementStateInfo().IsMoveStatusChanged) {
                    npc.GetMovementStateInfo().IsMoveStatusChanged = false;

                    Msg_RC_NpcMove npcMoveBuilder = DataSyncUtility.BuildNpcMoveMessage(npc);
                    if (null != npcMoveBuilder)
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcMove, npcMoveBuilder);
                }
            }
        }
        private void OnAiFace(EntityInfo npc)
        {
            if (npc.GetMovementStateInfo().IsFaceDirChanged) {
                npc.GetMovementStateInfo().IsFaceDirChanged = false;
                if (null != npc) {
                    float dir = npc.GetMovementStateInfo().GetFaceDir();
                    npc.GetMovementStateInfo().SetFaceDir(dir);
                }
                if (!npc.GetMovementStateInfo().IsMoving) {
                    Scene scene = npc.SceneContext.CustomData as Scene;
                    if (null != scene) {
                        Msg_RC_NpcFace npcFaceBuilder = DataSyncUtility.BuildNpcFaceMessage(npc);
                        if (null != npcFaceBuilder)
                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcFace, npcFaceBuilder);
                    }
                }
            }
        }
        private void OnAiSelectSkill(EntityInfo npc, SkillInfo skill)
        {
            if (skill == null)
                npc.GetSkillStateInfo().SetCurSkillInfo(0);
            else
                npc.GetSkillStateInfo().SetCurSkillInfo(skill.SkillId);
        }
        private void OnAiSkill(EntityInfo npc, int skillId)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                SkillInfo skillInfo = npc.GetSkillStateInfo().GetCurSkillInfo();
                if (null == skillInfo || !skillInfo.IsSkillActivated) {
                    SkillInfo curSkillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                    if (null != curSkillInfo) {
                        long curTime = TimeUtility.GetLocalMilliseconds();
                        if (!curSkillInfo.IsInCd(curTime)) {
                            if (scene.SkillSystem.StartSkill(npc.GetId(), curSkillInfo.ConfigData, 0)) {
                                Msg_RC_NpcSkill skillBuilder = DataSyncUtility.BuildNpcSkillMessage(npc, skillId);

                                LogSystem.Info("Send Msg_RC_NpcSkill, EntityId={0}, SkillId={1}",
                                  npc.GetId(), skillId);
                                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcSkill, skillBuilder);
                            }
                        }
                    }
                }
            }
        }
        private void OnAiStopSkill(EntityInfo npc)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                SkillInfo skillInfo = npc.GetSkillStateInfo().GetCurSkillInfo();
                if (null == skillInfo || skillInfo.IsSkillActivated) {
                    scene.SkillSystem.StopAllSkill(npc.GetId(), true);
                }

                Msg_RC_NpcStopSkill skillBuilder = DataSyncUtility.BuildNpcStopSkillMessage(npc);

                LogSystem.Info("Send Msg_RC_NpcStopSkill, EntityId={0}",
                  npc.GetId());
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, skillBuilder);
            }
        }
        private void OnAiAddImpact(EntityInfo npc, int impactId)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                ImpactInfo impactInfo = new ImpactInfo(impactId);
                impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                impactInfo.ImpactSenderId = npc.GetId();
                impactInfo.SkillId = 0;
                if (null != impactInfo.ConfigData) {
                    npc.GetSkillStateInfo().AddImpact(impactInfo);
                    int seq = impactInfo.Seq;
                    if (scene.SkillSystem.StartSkill(npc.GetId(), impactInfo.ConfigData, seq)) {
                        Msg_RC_AddImpact addImpactBuilder = new Msg_RC_AddImpact();
                        addImpactBuilder.sender_id = npc.GetId();
                        addImpactBuilder.target_id = npc.GetId();
                        addImpactBuilder.impact_id = impactId;
                        addImpactBuilder.skill_id = -1;
                        addImpactBuilder.duration = impactInfo.DurationTime;
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AddImpact, addImpactBuilder);
                    }
                }                
            }
        }
        private void OnAiRemoveImpact(EntityInfo npc, int impactId)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                ImpactInfo impactInfo = npc.GetSkillStateInfo().FindImpactInfoById(impactId);
                if (null != impactInfo) {
                    Msg_RC_RemoveImpact removeImpactBuilder = new Msg_RC_RemoveImpact();
                    removeImpactBuilder.obj_id = npc.GetId();
                    removeImpactBuilder.impact_id = impactId;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_RemoveImpact, removeImpactBuilder);

                    scene.SkillSystem.StopSkill(npc.GetId(), impactId, impactInfo.Seq, false);
                }
            }
        }
        private void OnAiSendStoryMessage(EntityInfo npc, string msgId, object[] args)
        {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                scene.StorySystem.SendMessage(msgId, args);
            }
        }
    }
}
