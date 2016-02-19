//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/LobbyGmMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum LobbyGmMessageDefine
	{
		GmUserBasic = 1,
		GmUserInfo,
		Msg_CL_GmQueryUserByGuid,
		Msg_CL_GmQueryUserByNickname,
		Msg_LC_GmQueryUser,
		Msg_CL_GmQueryUserByFuzzyNickname,
		Msg_LC_GmQueryUserByFuzzyNickname,
		Msg_CL_GmQueryAccount,
		Msg_LC_GmQueryAccount,
		Msg_CL_GmKickUser,
		Msg_LC_GmKickUser,
		Msg_CL_GmLockUser,
		Msg_LC_GmLockUser,
		Msg_CL_GmUnlockUser,
		Msg_LC_GmUnlockUser,
		Msg_CL_GmAddExp,
		Msg_LC_GmAddExp,
		Msg_CL_GmUpdateMaxUserCount,
		Msg_CL_PublishNotice,
		Msg_LC_PublishNotice,
		Msg_CL_SendMail,
		Msg_LC_SendMail,
		Msg_CL_GmHomeNotice,
		Msg_LC_GmHomeNotice,
		Msg_CL_GmForbidChat,
		Msg_LC_GmForbidChat,
		Msg_CL_GmCode,
		Msg_CL_GmGeneralOperation,
		Msg_LC_GmGeneralOperation,
		ItemDataMsg,
		MaxNum
	}

	public static class LobbyGmMessageDefine2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_LobbyGmMessageDefine2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2LobbyGmMessageDefine.TryGetValue(t, out id);
			return id;
		}

		static LobbyGmMessageDefine2Type()
		{
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.GmUserBasic, typeof(GmUserBasic));
			s_Type2LobbyGmMessageDefine.Add(typeof(GmUserBasic), (int)LobbyGmMessageDefine.GmUserBasic);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.GmUserInfo, typeof(GmUserInfo));
			s_Type2LobbyGmMessageDefine.Add(typeof(GmUserInfo), (int)LobbyGmMessageDefine.GmUserInfo);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.ItemDataMsg, typeof(ItemDataMsg));
			s_Type2LobbyGmMessageDefine.Add(typeof(ItemDataMsg), (int)LobbyGmMessageDefine.ItemDataMsg);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmAddExp, typeof(Msg_CL_GmAddExp));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmAddExp), (int)LobbyGmMessageDefine.Msg_CL_GmAddExp);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmCode, typeof(Msg_CL_GmCode));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmCode), (int)LobbyGmMessageDefine.Msg_CL_GmCode);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmForbidChat, typeof(Msg_CL_GmForbidChat));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmForbidChat), (int)LobbyGmMessageDefine.Msg_CL_GmForbidChat);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmGeneralOperation, typeof(Msg_CL_GmGeneralOperation));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmGeneralOperation), (int)LobbyGmMessageDefine.Msg_CL_GmGeneralOperation);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmHomeNotice, typeof(Msg_CL_GmHomeNotice));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmHomeNotice), (int)LobbyGmMessageDefine.Msg_CL_GmHomeNotice);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmKickUser, typeof(Msg_CL_GmKickUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmKickUser), (int)LobbyGmMessageDefine.Msg_CL_GmKickUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmLockUser, typeof(Msg_CL_GmLockUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmLockUser), (int)LobbyGmMessageDefine.Msg_CL_GmLockUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmQueryAccount, typeof(Msg_CL_GmQueryAccount));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmQueryAccount), (int)LobbyGmMessageDefine.Msg_CL_GmQueryAccount);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByFuzzyNickname, typeof(Msg_CL_GmQueryUserByFuzzyNickname));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByFuzzyNickname), (int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByFuzzyNickname);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByGuid, typeof(Msg_CL_GmQueryUserByGuid));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByGuid), (int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByGuid);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByNickname, typeof(Msg_CL_GmQueryUserByNickname));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByNickname), (int)LobbyGmMessageDefine.Msg_CL_GmQueryUserByNickname);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmUnlockUser, typeof(Msg_CL_GmUnlockUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmUnlockUser), (int)LobbyGmMessageDefine.Msg_CL_GmUnlockUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_GmUpdateMaxUserCount, typeof(Msg_CL_GmUpdateMaxUserCount));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_GmUpdateMaxUserCount), (int)LobbyGmMessageDefine.Msg_CL_GmUpdateMaxUserCount);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_PublishNotice, typeof(Msg_CL_PublishNotice));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_PublishNotice), (int)LobbyGmMessageDefine.Msg_CL_PublishNotice);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_CL_SendMail, typeof(Msg_CL_SendMail));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_CL_SendMail), (int)LobbyGmMessageDefine.Msg_CL_SendMail);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmAddExp, typeof(Msg_LC_GmAddExp));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmAddExp), (int)LobbyGmMessageDefine.Msg_LC_GmAddExp);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmForbidChat, typeof(Msg_LC_GmForbidChat));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmForbidChat), (int)LobbyGmMessageDefine.Msg_LC_GmForbidChat);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmGeneralOperation, typeof(Msg_LC_GmGeneralOperation));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmGeneralOperation), (int)LobbyGmMessageDefine.Msg_LC_GmGeneralOperation);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmHomeNotice, typeof(Msg_LC_GmHomeNotice));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmHomeNotice), (int)LobbyGmMessageDefine.Msg_LC_GmHomeNotice);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmKickUser, typeof(Msg_LC_GmKickUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmKickUser), (int)LobbyGmMessageDefine.Msg_LC_GmKickUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmLockUser, typeof(Msg_LC_GmLockUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmLockUser), (int)LobbyGmMessageDefine.Msg_LC_GmLockUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmQueryAccount, typeof(Msg_LC_GmQueryAccount));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmQueryAccount), (int)LobbyGmMessageDefine.Msg_LC_GmQueryAccount);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmQueryUser, typeof(Msg_LC_GmQueryUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmQueryUser), (int)LobbyGmMessageDefine.Msg_LC_GmQueryUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmQueryUserByFuzzyNickname, typeof(Msg_LC_GmQueryUserByFuzzyNickname));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmQueryUserByFuzzyNickname), (int)LobbyGmMessageDefine.Msg_LC_GmQueryUserByFuzzyNickname);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_GmUnlockUser, typeof(Msg_LC_GmUnlockUser));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_GmUnlockUser), (int)LobbyGmMessageDefine.Msg_LC_GmUnlockUser);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_PublishNotice, typeof(Msg_LC_PublishNotice));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_PublishNotice), (int)LobbyGmMessageDefine.Msg_LC_PublishNotice);
			s_LobbyGmMessageDefine2Type.Add((int)LobbyGmMessageDefine.Msg_LC_SendMail, typeof(Msg_LC_SendMail));
			s_Type2LobbyGmMessageDefine.Add(typeof(Msg_LC_SendMail), (int)LobbyGmMessageDefine.Msg_LC_SendMail);
		}

		private static Dictionary<int, Type> s_LobbyGmMessageDefine2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2LobbyGmMessageDefine = new Dictionary<Type, int>();
	}
}

