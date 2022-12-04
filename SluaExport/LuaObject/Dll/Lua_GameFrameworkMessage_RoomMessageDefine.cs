﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_RoomMessageDefine : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.RoomMessageDefine");
		addMember(l,1,"Msg_Ping");
		addMember(l,2,"Msg_Pong");
		addMember(l,3,"Position");
		addMember(l,4,"Msg_CR_ShakeHands");
		addMember(l,5,"Msg_RC_ShakeHands_Ret");
		addMember(l,6,"Msg_CR_Observer");
		addMember(l,7,"Msg_CR_Enter");
		addMember(l,8,"Msg_CR_Quit");
		addMember(l,9,"Msg_CR_UserMoveToPos");
		addMember(l,10,"Msg_CR_Skill");
		addMember(l,11,"Msg_CR_StopSkill");
		addMember(l,12,"Msg_CR_SwitchDebug");
		addMember(l,13,"Msg_CR_GmCommand");
		addMember(l,14,"Msg_RC_CreateNpc");
		addMember(l,15,"Msg_RC_NpcDead");
		addMember(l,16,"Msg_RC_DestroyNpc");
		addMember(l,17,"Msg_RC_NpcMove");
		addMember(l,18,"Msg_RC_NpcFace");
		addMember(l,19,"Msg_RC_NpcSkill");
		addMember(l,20,"Msg_RC_NpcStopSkill");
		addMember(l,21,"Msg_RC_AddImpact");
		addMember(l,22,"Msg_RC_RemoveImpact");
		addMember(l,23,"Msg_RC_AddSkill");
		addMember(l,24,"Msg_RC_RemoveSkill");
		addMember(l,25,"Msg_RC_AdjustPosition");
		addMember(l,26,"Msg_RC_SyncProperty");
		addMember(l,27,"Msg_RC_ImpactDamage");
		addMember(l,28,"Msg_RC_ChangeScene");
		addMember(l,29,"Msg_RC_SyncNpcOwnerId");
		addMember(l,30,"Msg_RC_CampChanged");
		addMember(l,31,"Msg_RC_DebugSpaceInfo");
		addMember(l,32,"Msg_CRC_StoryMessage");
		addMember(l,33,"Msg_RC_PublishEvent");
		addMember(l,34,"Msg_RC_SendGfxMessage");
		addMember(l,35,"Msg_RC_HighlightPrompt");
		addMember(l,36,"Msg_RC_ShowDlg");
		addMember(l,37,"Msg_CR_DlgClosed");
		addMember(l,38,"Msg_RC_LockFrame");
		addMember(l,39,"Msg_RC_PlayAnimation");
		addMember(l,40,"MaxNum");
		LuaDLL.lua_pop(l, 1);
	}
}
