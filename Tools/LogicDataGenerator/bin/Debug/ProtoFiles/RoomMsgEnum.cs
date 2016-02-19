//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/RoomMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum RoomMessageDefine
	{
		Msg_Ping = 1,
		Msg_Pong,
		Position,
		EncodePosition,
		EncodePosition3D,
		Msg_CR_ShakeHands,
		Msg_RC_ShakeHands_Ret,
		Msg_CR_Observer,
		Msg_CR_Enter,
		Msg_CR_Quit,
		Msg_CR_UserMoveToPos,
		Msg_CR_Skill,
		Msg_CR_OperateMode,
		Msg_CR_GiveUpBattle,
		Msg_CR_SwitchDebug,
		Msg_CR_GmCommand,
		Msg_RC_CreateNpc,
		Msg_RC_NpcDead,
		Msg_RC_DestroyNpc,
		Msg_RC_NpcMove,
		Msg_RC_NpcFace,
		Msg_RC_NpcSkill,
		Msg_RC_NpcStopSkill,
		Msg_RC_AddImpact,
		Msg_RC_RemoveImpact,
		Msg_RC_AddSkill,
		Msg_RC_RemoveSkill,
		Msg_RC_AdjustPosition,
		Msg_RC_SyncProperty,
		Msg_RC_ImpactDamage,
		Msg_RC_ChangeScene,
		Msg_RC_SyncNpcOwnerId,
		Msg_RC_CampChanged,
		Msg_RC_DebugSpaceInfo,
		Msg_CRC_StoryMessage,
		Msg_RC_PublishEvent,
		Msg_RC_SendGfxMessage,
		Msg_RC_HighlightPrompt,
		Msg_RC_ShowDlg,
		Msg_CR_DlgClosed,
		Msg_RC_LockFrame,
		Msg_RC_PlayAnimation,
		MaxNum
	}

	public static class RoomMessageDefine2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_RoomMessageDefine2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2RoomMessageDefine.TryGetValue(t, out id);
			return id;
		}

		static RoomMessageDefine2Type()
		{
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.EncodePosition, typeof(EncodePosition));
			s_Type2RoomMessageDefine.Add(typeof(EncodePosition), (int)RoomMessageDefine.EncodePosition);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.EncodePosition3D, typeof(EncodePosition3D));
			s_Type2RoomMessageDefine.Add(typeof(EncodePosition3D), (int)RoomMessageDefine.EncodePosition3D);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_DlgClosed, typeof(Msg_CR_DlgClosed));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_DlgClosed), (int)RoomMessageDefine.Msg_CR_DlgClosed);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_Enter, typeof(Msg_CR_Enter));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_Enter), (int)RoomMessageDefine.Msg_CR_Enter);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_GiveUpBattle, typeof(Msg_CR_GiveUpBattle));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_GiveUpBattle), (int)RoomMessageDefine.Msg_CR_GiveUpBattle);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_GmCommand, typeof(Msg_CR_GmCommand));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_GmCommand), (int)RoomMessageDefine.Msg_CR_GmCommand);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_Observer, typeof(Msg_CR_Observer));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_Observer), (int)RoomMessageDefine.Msg_CR_Observer);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_OperateMode, typeof(Msg_CR_OperateMode));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_OperateMode), (int)RoomMessageDefine.Msg_CR_OperateMode);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_Quit, typeof(Msg_CR_Quit));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_Quit), (int)RoomMessageDefine.Msg_CR_Quit);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_ShakeHands, typeof(Msg_CR_ShakeHands));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_ShakeHands), (int)RoomMessageDefine.Msg_CR_ShakeHands);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_Skill, typeof(Msg_CR_Skill));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_Skill), (int)RoomMessageDefine.Msg_CR_Skill);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_SwitchDebug, typeof(Msg_CR_SwitchDebug));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_SwitchDebug), (int)RoomMessageDefine.Msg_CR_SwitchDebug);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CR_UserMoveToPos, typeof(Msg_CR_UserMoveToPos));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CR_UserMoveToPos), (int)RoomMessageDefine.Msg_CR_UserMoveToPos);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_CRC_StoryMessage, typeof(Msg_CRC_StoryMessage));
			s_Type2RoomMessageDefine.Add(typeof(Msg_CRC_StoryMessage), (int)RoomMessageDefine.Msg_CRC_StoryMessage);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_Ping, typeof(Msg_Ping));
			s_Type2RoomMessageDefine.Add(typeof(Msg_Ping), (int)RoomMessageDefine.Msg_Ping);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_Pong, typeof(Msg_Pong));
			s_Type2RoomMessageDefine.Add(typeof(Msg_Pong), (int)RoomMessageDefine.Msg_Pong);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_AddImpact, typeof(Msg_RC_AddImpact));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_AddImpact), (int)RoomMessageDefine.Msg_RC_AddImpact);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_AddSkill, typeof(Msg_RC_AddSkill));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_AddSkill), (int)RoomMessageDefine.Msg_RC_AddSkill);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_AdjustPosition, typeof(Msg_RC_AdjustPosition));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_AdjustPosition), (int)RoomMessageDefine.Msg_RC_AdjustPosition);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_CampChanged, typeof(Msg_RC_CampChanged));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_CampChanged), (int)RoomMessageDefine.Msg_RC_CampChanged);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_ChangeScene, typeof(Msg_RC_ChangeScene));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_ChangeScene), (int)RoomMessageDefine.Msg_RC_ChangeScene);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_CreateNpc, typeof(Msg_RC_CreateNpc));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_CreateNpc), (int)RoomMessageDefine.Msg_RC_CreateNpc);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_DebugSpaceInfo, typeof(Msg_RC_DebugSpaceInfo));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_DebugSpaceInfo), (int)RoomMessageDefine.Msg_RC_DebugSpaceInfo);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_DestroyNpc, typeof(Msg_RC_DestroyNpc));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_DestroyNpc), (int)RoomMessageDefine.Msg_RC_DestroyNpc);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_HighlightPrompt, typeof(Msg_RC_HighlightPrompt));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_HighlightPrompt), (int)RoomMessageDefine.Msg_RC_HighlightPrompt);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_ImpactDamage, typeof(Msg_RC_ImpactDamage));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_ImpactDamage), (int)RoomMessageDefine.Msg_RC_ImpactDamage);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_LockFrame, typeof(Msg_RC_LockFrame));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_LockFrame), (int)RoomMessageDefine.Msg_RC_LockFrame);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_NpcDead, typeof(Msg_RC_NpcDead));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_NpcDead), (int)RoomMessageDefine.Msg_RC_NpcDead);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_NpcFace, typeof(Msg_RC_NpcFace));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_NpcFace), (int)RoomMessageDefine.Msg_RC_NpcFace);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_NpcMove, typeof(Msg_RC_NpcMove));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_NpcMove), (int)RoomMessageDefine.Msg_RC_NpcMove);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_NpcSkill, typeof(Msg_RC_NpcSkill));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_NpcSkill), (int)RoomMessageDefine.Msg_RC_NpcSkill);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_NpcStopSkill, typeof(Msg_RC_NpcStopSkill));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_NpcStopSkill), (int)RoomMessageDefine.Msg_RC_NpcStopSkill);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_PlayAnimation, typeof(Msg_RC_PlayAnimation));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_PlayAnimation), (int)RoomMessageDefine.Msg_RC_PlayAnimation);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_PublishEvent, typeof(Msg_RC_PublishEvent));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_PublishEvent), (int)RoomMessageDefine.Msg_RC_PublishEvent);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_RemoveImpact, typeof(Msg_RC_RemoveImpact));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_RemoveImpact), (int)RoomMessageDefine.Msg_RC_RemoveImpact);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_RemoveSkill, typeof(Msg_RC_RemoveSkill));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_RemoveSkill), (int)RoomMessageDefine.Msg_RC_RemoveSkill);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_SendGfxMessage, typeof(Msg_RC_SendGfxMessage));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_SendGfxMessage), (int)RoomMessageDefine.Msg_RC_SendGfxMessage);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_ShakeHands_Ret, typeof(Msg_RC_ShakeHands_Ret));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_ShakeHands_Ret), (int)RoomMessageDefine.Msg_RC_ShakeHands_Ret);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_ShowDlg, typeof(Msg_RC_ShowDlg));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_ShowDlg), (int)RoomMessageDefine.Msg_RC_ShowDlg);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_SyncNpcOwnerId, typeof(Msg_RC_SyncNpcOwnerId));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_SyncNpcOwnerId), (int)RoomMessageDefine.Msg_RC_SyncNpcOwnerId);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Msg_RC_SyncProperty, typeof(Msg_RC_SyncProperty));
			s_Type2RoomMessageDefine.Add(typeof(Msg_RC_SyncProperty), (int)RoomMessageDefine.Msg_RC_SyncProperty);
			s_RoomMessageDefine2Type.Add((int)RoomMessageDefine.Position, typeof(Position));
			s_Type2RoomMessageDefine.Add(typeof(Position), (int)RoomMessageDefine.Position);
		}

		private static Dictionary<int, Type> s_RoomMessageDefine2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2RoomMessageDefine = new Dictionary<Type, int>();
	}
}

