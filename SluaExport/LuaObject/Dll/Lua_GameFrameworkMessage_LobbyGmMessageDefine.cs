using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_LobbyGmMessageDefine : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.LobbyGmMessageDefine");
		addMember(l,1,"GmUserBasic");
		addMember(l,2,"GmUserInfo");
		addMember(l,3,"Msg_CL_GmQueryUserByGuid");
		addMember(l,4,"Msg_CL_GmQueryUserByNickname");
		addMember(l,5,"Msg_LC_GmQueryUser");
		addMember(l,6,"Msg_CL_GmQueryUserByFuzzyNickname");
		addMember(l,7,"Msg_LC_GmQueryUserByFuzzyNickname");
		addMember(l,8,"Msg_CL_GmQueryAccount");
		addMember(l,9,"Msg_LC_GmQueryAccount");
		addMember(l,10,"Msg_CL_GmKickUser");
		addMember(l,11,"Msg_LC_GmKickUser");
		addMember(l,12,"Msg_CL_GmLockUser");
		addMember(l,13,"Msg_LC_GmLockUser");
		addMember(l,14,"Msg_CL_GmUnlockUser");
		addMember(l,15,"Msg_LC_GmUnlockUser");
		addMember(l,16,"Msg_CL_GmAddExp");
		addMember(l,17,"Msg_LC_GmAddExp");
		addMember(l,18,"Msg_CL_GmUpdateMaxUserCount");
		addMember(l,19,"Msg_CL_PublishNotice");
		addMember(l,20,"Msg_LC_PublishNotice");
		addMember(l,21,"Msg_CL_SendMail");
		addMember(l,22,"Msg_LC_SendMail");
		addMember(l,23,"Msg_CL_GmHomeNotice");
		addMember(l,24,"Msg_LC_GmHomeNotice");
		addMember(l,25,"Msg_CL_GmForbidChat");
		addMember(l,26,"Msg_LC_GmForbidChat");
		addMember(l,27,"Msg_CL_GmCode");
		addMember(l,28,"Msg_CL_GmGeneralOperation");
		addMember(l,29,"Msg_LC_GmGeneralOperation");
		addMember(l,30,"ItemDataMsg");
		addMember(l,31,"MaxNum");
		LuaDLL.lua_pop(l, 1);
	}
}
