//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFrameworkMessage
{
	public enum MessageDefine
	{
		Zero,
		Msg_Ping = 1,
		Msg_Pong,
		Position,
		EncodePosition,
		EncodePosition3D,
		Msg_CR_ShakeHands,
		Msg_RC_ShakeHands_Ret,
		Msg_CR_Observer,
		Msg_CRC_Create,
		Msg_RC_Enter,
		Msg_RC_Disappear,
		Msg_RC_Dead,
		Msg_RC_Revive,
		Msg_CRC_Exit,
		Msg_CRC_MoveStart,
		Msg_CRC_MoveStop,
		Msg_CRC_MoveMeetObstacle,
		Msg_CRC_Face,
		Msg_CRC_Skill,
		Msg_CRC_StopSkill,
		Msg_RC_CreateNpc,
		Msg_RC_CreatePartner,
		Msg_RC_DestroyNpc,
		Msg_RC_NpcEnter,
		Msg_RC_NpcMove,
		Msg_RC_NpcFace,
		Msg_RC_NpcSkill,
		Msg_CRC_NpcStopSkill,
		Msg_RC_NpcDead,
		Msg_RC_NpcDisappear,
		Msg_RC_SyncProperty,
		Msg_RC_DebugSpaceInfo,
		Msg_CR_SwitchDebug,
		Msg_RC_SyncCombatStatisticInfo,
		Msg_RC_SyncCombatInfo,
		Msg_CRC_SendImpactToEntity,
		Msg_CRC_SendImpactToEntityInPvp,
		Msg_CRC_StopGfxImpact,
		Msg_CRC_InteractObject,
		Msg_RC_ImpactDamage,
		Msg_RC_ImpactRage,
		Msg_RC_ControlObject,
		Msg_RC_RefreshItemSkills,
		Msg_RC_HighlightPrompt,
		Msg_CR_Quit,
		Msg_RC_UserMove,
		Msg_RC_UserFace,
		Msg_RC_UserSkill,
		Msg_CRC_UserStopSkill,
		Msg_CR_UserMoveToPos,
		Msg_CR_UserMoveToAttack,
		Msg_RC_UpdateUserBattleInfo,
		Msg_RC_MissionCompleted,
		Msg_RC_ChangeScene,
		Msg_RC_CampChanged,
		Msg_RC_EnableInput,
		Msg_RC_ShowUi,
		Msg_RC_ShowWall,
		Msg_RC_ShowDlg,
		Msg_CR_DlgClosed,
		Msg_RC_CameraLookat,
		Msg_RC_CameraFollow,
		Msg_RC_CameraYaw,
		Msg_RC_CameraHeight,
		Msg_RC_CameraDistance,
		Msg_RC_SetBlockedShader,
		Msg_CR_GfxControlMoveStart,
		Msg_CR_GfxControlMoveStop,
		Msg_CR_GiveUpBattle,
		Msg_CR_DeleteDeadNpc,
		Msg_RC_UpdateCoefficient,
		Msg_RC_AdjustPosition,
		Msg_RC_LockFrame,
		Msg_RC_PlayAnimation,
		Msg_RC_StartCountDown,
		Msg_RC_PublishEvent,
		Msg_RC_CameraEnable,
		Msg_CR_HitCountChanged,
		Msg_RC_SendGfxMessage,
		Msg_RC_SendGfxMessageById,
		Msg_RC_AddSkill,
		Msg_RC_RemoveSkill,
		Msg_RC_StopImpact,
		Msg_CR_SyncCharacterGfxState,
		Msg_CR_SummonPartner,
		Msg_CRC_SummonNpc,
		Msg_RC_SyncNpcOwnerId,
		Msg_CR_GmCommand,
		Msg_RC_DropNpc,
		Msg_CR_PickUpNpc,
		Msg_CR_PvpIsReady,
		Msg_RC_PvpIsAllReady,
		Msg_CRC_BreakSkill,
		Msg_CRC_StoryMessage,
		Msg_CR_TimeCounter,
		Msg_RC_ClientError,
		MaxNum
	}
	public static class MessageDefine2Object
	{
		public static object New(int id)
		{
			object r = null;
			MyFunc<object> t;
			if(s_MessageDefine2Object.TryGetValue(id, out t)){
				r = t();
			}
			return r;
		}

		static MessageDefine2Object()
		{
			s_MessageDefine2Object.Add((int)MessageDefine.EncodePosition, () => new EncodePosition());
			s_MessageDefine2Object.Add((int)MessageDefine.EncodePosition3D, () => new EncodePosition3D());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_DeleteDeadNpc, () => new Msg_CR_DeleteDeadNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_DlgClosed, () => new Msg_CR_DlgClosed());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_GfxControlMoveStart, () => new Msg_CR_GfxControlMoveStart());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_GfxControlMoveStop, () => new Msg_CR_GfxControlMoveStop());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_GiveUpBattle, () => new Msg_CR_GiveUpBattle());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_GmCommand, () => new Msg_CR_GmCommand());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_HitCountChanged, () => new Msg_CR_HitCountChanged());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_Observer, () => new Msg_CR_Observer());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_PickUpNpc, () => new Msg_CR_PickUpNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_PvpIsReady, () => new Msg_CR_PvpIsReady());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_Quit, () => new Msg_CR_Quit());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_ShakeHands, () => new Msg_CR_ShakeHands());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_SummonPartner, () => new Msg_CR_SummonPartner());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_SwitchDebug, () => new Msg_CR_SwitchDebug());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_SyncCharacterGfxState, () => new Msg_CR_SyncCharacterGfxState());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_TimeCounter, () => new Msg_CR_TimeCounter());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_UserMoveToAttack, () => new Msg_CR_UserMoveToAttack());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CR_UserMoveToPos, () => new Msg_CR_UserMoveToPos());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_BreakSkill, () => new Msg_CRC_BreakSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_Create, () => new Msg_CRC_Create());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_Exit, () => new Msg_CRC_Exit());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_Face, () => new Msg_CRC_Face());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_InteractObject, () => new Msg_CRC_InteractObject());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_MoveMeetObstacle, () => new Msg_CRC_MoveMeetObstacle());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_MoveStart, () => new Msg_CRC_MoveStart());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_MoveStop, () => new Msg_CRC_MoveStop());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_NpcStopSkill, () => new Msg_CRC_NpcStopSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_SendImpactToEntity, () => new Msg_CRC_SendImpactToEntity());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_SendImpactToEntityInPvp, () => new Msg_CRC_SendImpactToEntityInPvp());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_Skill, () => new Msg_CRC_Skill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_StopGfxImpact, () => new Msg_CRC_StopGfxImpact());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_StopSkill, () => new Msg_CRC_StopSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_StoryMessage, () => new Msg_CRC_StoryMessage());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_SummonNpc, () => new Msg_CRC_SummonNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_CRC_UserStopSkill, () => new Msg_CRC_UserStopSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_Ping, () => new Msg_Ping());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_Pong, () => new Msg_Pong());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_AddSkill, () => new Msg_RC_AddSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_AdjustPosition, () => new Msg_RC_AdjustPosition());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraDistance, () => new Msg_RC_CameraDistance());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraEnable, () => new Msg_RC_CameraEnable());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraFollow, () => new Msg_RC_CameraFollow());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraHeight, () => new Msg_RC_CameraHeight());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraLookat, () => new Msg_RC_CameraLookat());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CameraYaw, () => new Msg_RC_CameraYaw());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CampChanged, () => new Msg_RC_CampChanged());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ChangeScene, () => new Msg_RC_ChangeScene());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ClientError, () => new Msg_RC_ClientError());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ControlObject, () => new Msg_RC_ControlObject());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CreateNpc, () => new Msg_RC_CreateNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_CreatePartner, () => new Msg_RC_CreatePartner());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_Dead, () => new Msg_RC_Dead());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_DebugSpaceInfo, () => new Msg_RC_DebugSpaceInfo());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_DestroyNpc, () => new Msg_RC_DestroyNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_Disappear, () => new Msg_RC_Disappear());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_DropNpc, () => new Msg_RC_DropNpc());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_EnableInput, () => new Msg_RC_EnableInput());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_Enter, () => new Msg_RC_Enter());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_HighlightPrompt, () => new Msg_RC_HighlightPrompt());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ImpactDamage, () => new Msg_RC_ImpactDamage());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ImpactRage, () => new Msg_RC_ImpactRage());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_LockFrame, () => new Msg_RC_LockFrame());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_MissionCompleted, () => new Msg_RC_MissionCompleted());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcDead, () => new Msg_RC_NpcDead());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcDisappear, () => new Msg_RC_NpcDisappear());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcEnter, () => new Msg_RC_NpcEnter());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcFace, () => new Msg_RC_NpcFace());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcMove, () => new Msg_RC_NpcMove());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_NpcSkill, () => new Msg_RC_NpcSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_PlayAnimation, () => new Msg_RC_PlayAnimation());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_PublishEvent, () => new Msg_RC_PublishEvent());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_PvpIsAllReady, () => new Msg_RC_PvpIsAllReady());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_RefreshItemSkills, () => new Msg_RC_RefreshItemSkills());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_RemoveSkill, () => new Msg_RC_RemoveSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_Revive, () => new Msg_RC_Revive());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SendGfxMessage, () => new Msg_RC_SendGfxMessage());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SendGfxMessageById, () => new Msg_RC_SendGfxMessageById());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SetBlockedShader, () => new Msg_RC_SetBlockedShader());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ShakeHands_Ret, () => new Msg_RC_ShakeHands_Ret());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ShowDlg, () => new Msg_RC_ShowDlg());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ShowUi, () => new Msg_RC_ShowUi());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_ShowWall, () => new Msg_RC_ShowWall());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_StartCountDown, () => new Msg_RC_StartCountDown());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_StopImpact, () => new Msg_RC_StopImpact());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SyncCombatInfo, () => new Msg_RC_SyncCombatInfo());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SyncCombatStatisticInfo, () => new Msg_RC_SyncCombatStatisticInfo());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SyncNpcOwnerId, () => new Msg_RC_SyncNpcOwnerId());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_SyncProperty, () => new Msg_RC_SyncProperty());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_UpdateCoefficient, () => new Msg_RC_UpdateCoefficient());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_UpdateUserBattleInfo, () => new Msg_RC_UpdateUserBattleInfo());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_UserFace, () => new Msg_RC_UserFace());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_UserMove, () => new Msg_RC_UserMove());
			s_MessageDefine2Object.Add((int)MessageDefine.Msg_RC_UserSkill, () => new Msg_RC_UserSkill());
			s_MessageDefine2Object.Add((int)MessageDefine.Position, () => new Position());
		}

		private static Dictionary<int, MyFunc<object>> s_MessageDefine2Object = new Dictionary<int, MyFunc<object>>();
	}
}
