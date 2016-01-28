//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkGmMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum GmMessageDefine
	{
		GmProperty = 1,
		GmUserDetail,
		GmUserBasic,
		GmUserAttribute,
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
		Msg_CL_GmRestorePayment,
		Msg_LC_GmRestorePayment,
		Msg_CL_GmResetConsumeGoodsCount,
		Msg_CL_GmGeneralOperation,
		Msg_LC_GmGeneralOperation,
		ItemDataMsg,
		TalentDataMsg,
	}

	public static class GmMessageDefine2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_GmMessageDefine2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2GmMessageDefine.TryGetValue(t, out id);
			return id;
		}

		static GmMessageDefine2Type()
		{
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.GmProperty, typeof(GmProperty));
			s_Type2GmMessageDefine.Add(typeof(GmProperty), (int)GmMessageDefine.GmProperty);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.GmUserAttribute, typeof(GmUserAttribute));
			s_Type2GmMessageDefine.Add(typeof(GmUserAttribute), (int)GmMessageDefine.GmUserAttribute);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.GmUserBasic, typeof(GmUserBasic));
			s_Type2GmMessageDefine.Add(typeof(GmUserBasic), (int)GmMessageDefine.GmUserBasic);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.GmUserDetail, typeof(GmUserDetail));
			s_Type2GmMessageDefine.Add(typeof(GmUserDetail), (int)GmMessageDefine.GmUserDetail);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.GmUserInfo, typeof(GmUserInfo));
			s_Type2GmMessageDefine.Add(typeof(GmUserInfo), (int)GmMessageDefine.GmUserInfo);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.ItemDataMsg, typeof(ItemDataMsg));
			s_Type2GmMessageDefine.Add(typeof(ItemDataMsg), (int)GmMessageDefine.ItemDataMsg);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmAddExp, typeof(Msg_CL_GmAddExp));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmAddExp), (int)GmMessageDefine.Msg_CL_GmAddExp);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmCode, typeof(Msg_CL_GmCode));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmCode), (int)GmMessageDefine.Msg_CL_GmCode);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmForbidChat, typeof(Msg_CL_GmForbidChat));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmForbidChat), (int)GmMessageDefine.Msg_CL_GmForbidChat);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmGeneralOperation, typeof(Msg_CL_GmGeneralOperation));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmGeneralOperation), (int)GmMessageDefine.Msg_CL_GmGeneralOperation);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmHomeNotice, typeof(Msg_CL_GmHomeNotice));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmHomeNotice), (int)GmMessageDefine.Msg_CL_GmHomeNotice);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmKickUser, typeof(Msg_CL_GmKickUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmKickUser), (int)GmMessageDefine.Msg_CL_GmKickUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmLockUser, typeof(Msg_CL_GmLockUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmLockUser), (int)GmMessageDefine.Msg_CL_GmLockUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmQueryAccount, typeof(Msg_CL_GmQueryAccount));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmQueryAccount), (int)GmMessageDefine.Msg_CL_GmQueryAccount);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmQueryUserByFuzzyNickname, typeof(Msg_CL_GmQueryUserByFuzzyNickname));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByFuzzyNickname), (int)GmMessageDefine.Msg_CL_GmQueryUserByFuzzyNickname);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmQueryUserByGuid, typeof(Msg_CL_GmQueryUserByGuid));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByGuid), (int)GmMessageDefine.Msg_CL_GmQueryUserByGuid);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmQueryUserByNickname, typeof(Msg_CL_GmQueryUserByNickname));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmQueryUserByNickname), (int)GmMessageDefine.Msg_CL_GmQueryUserByNickname);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmResetConsumeGoodsCount, typeof(Msg_CL_GmResetConsumeGoodsCount));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmResetConsumeGoodsCount), (int)GmMessageDefine.Msg_CL_GmResetConsumeGoodsCount);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmRestorePayment, typeof(Msg_CL_GmRestorePayment));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmRestorePayment), (int)GmMessageDefine.Msg_CL_GmRestorePayment);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmUnlockUser, typeof(Msg_CL_GmUnlockUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmUnlockUser), (int)GmMessageDefine.Msg_CL_GmUnlockUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_GmUpdateMaxUserCount, typeof(Msg_CL_GmUpdateMaxUserCount));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_GmUpdateMaxUserCount), (int)GmMessageDefine.Msg_CL_GmUpdateMaxUserCount);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_PublishNotice, typeof(Msg_CL_PublishNotice));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_PublishNotice), (int)GmMessageDefine.Msg_CL_PublishNotice);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_CL_SendMail, typeof(Msg_CL_SendMail));
			s_Type2GmMessageDefine.Add(typeof(Msg_CL_SendMail), (int)GmMessageDefine.Msg_CL_SendMail);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmAddExp, typeof(Msg_LC_GmAddExp));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmAddExp), (int)GmMessageDefine.Msg_LC_GmAddExp);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmForbidChat, typeof(Msg_LC_GmForbidChat));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmForbidChat), (int)GmMessageDefine.Msg_LC_GmForbidChat);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmGeneralOperation, typeof(Msg_LC_GmGeneralOperation));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmGeneralOperation), (int)GmMessageDefine.Msg_LC_GmGeneralOperation);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmHomeNotice, typeof(Msg_LC_GmHomeNotice));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmHomeNotice), (int)GmMessageDefine.Msg_LC_GmHomeNotice);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmKickUser, typeof(Msg_LC_GmKickUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmKickUser), (int)GmMessageDefine.Msg_LC_GmKickUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmLockUser, typeof(Msg_LC_GmLockUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmLockUser), (int)GmMessageDefine.Msg_LC_GmLockUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmQueryAccount, typeof(Msg_LC_GmQueryAccount));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmQueryAccount), (int)GmMessageDefine.Msg_LC_GmQueryAccount);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmQueryUser, typeof(Msg_LC_GmQueryUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmQueryUser), (int)GmMessageDefine.Msg_LC_GmQueryUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmQueryUserByFuzzyNickname, typeof(Msg_LC_GmQueryUserByFuzzyNickname));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmQueryUserByFuzzyNickname), (int)GmMessageDefine.Msg_LC_GmQueryUserByFuzzyNickname);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmRestorePayment, typeof(Msg_LC_GmRestorePayment));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmRestorePayment), (int)GmMessageDefine.Msg_LC_GmRestorePayment);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_GmUnlockUser, typeof(Msg_LC_GmUnlockUser));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_GmUnlockUser), (int)GmMessageDefine.Msg_LC_GmUnlockUser);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_PublishNotice, typeof(Msg_LC_PublishNotice));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_PublishNotice), (int)GmMessageDefine.Msg_LC_PublishNotice);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.Msg_LC_SendMail, typeof(Msg_LC_SendMail));
			s_Type2GmMessageDefine.Add(typeof(Msg_LC_SendMail), (int)GmMessageDefine.Msg_LC_SendMail);
			s_GmMessageDefine2Type.Add((int)GmMessageDefine.TalentDataMsg, typeof(TalentDataMsg));
			s_Type2GmMessageDefine.Add(typeof(TalentDataMsg), (int)GmMessageDefine.TalentDataMsg);
		}

		private static Dictionary<int, Type> s_GmMessageDefine2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2GmMessageDefine = new Dictionary<Type, int>();
	}
}

