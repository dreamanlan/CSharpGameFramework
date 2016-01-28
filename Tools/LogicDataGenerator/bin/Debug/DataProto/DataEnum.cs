//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkData
{
	public enum DataEnum
	{
		TableGlobalParam = 1,
		TableGuid,
		TableNickname,
		TableGowStar,
		TableMailInfo,
		TableActivationCode,
		TableArenaInfo,
		TableArenaRecord,
		TableLootInfo,
		TableLootRecord,
		TableCorpsInfo,
		TableCorpsMember,
		TableFightInfo,
		TableHomeNotice,
		TableLoginLotteryRecord,
		TableInviterInfo,
		TableUndonePayment,
		TableAccount,
		TableUserInfo,
		TableUserGeneralInfo,
		TableUserSpecialInfo,
		TableUserBattleInfo,
		TableEquipInfo,
		TableFashionInfo,
		TableItemInfo,
		TableTalentInfo,
		TableLegacyInfo,
		TableXSoulInfo,
		TableSkillInfo,
		TableMissionInfo,
		TableLevelInfo,
		TableExpeditionInfo,
		TableGowInfo,
		TableMailStateInfo,
		TablePartnerInfo,
		TableFriendInfo,
		TablePaymentInfo,
		TableMpveAwardInfo,
		TableLotteryInfo,
	}

	public static class DataEnum2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_DataEnum2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2DataEnum.TryGetValue(t, out id);
			return id;
		}

		static DataEnum2Type()
		{
			s_DataEnum2Type.Add((int)DataEnum.TableAccount, typeof(TableAccount));
			s_Type2DataEnum.Add(typeof(TableAccount), (int)DataEnum.TableAccount);
			s_DataEnum2Type.Add((int)DataEnum.TableActivationCode, typeof(TableActivationCode));
			s_Type2DataEnum.Add(typeof(TableActivationCode), (int)DataEnum.TableActivationCode);
			s_DataEnum2Type.Add((int)DataEnum.TableArenaInfo, typeof(TableArenaInfo));
			s_Type2DataEnum.Add(typeof(TableArenaInfo), (int)DataEnum.TableArenaInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableArenaRecord, typeof(TableArenaRecord));
			s_Type2DataEnum.Add(typeof(TableArenaRecord), (int)DataEnum.TableArenaRecord);
			s_DataEnum2Type.Add((int)DataEnum.TableCorpsInfo, typeof(TableCorpsInfo));
			s_Type2DataEnum.Add(typeof(TableCorpsInfo), (int)DataEnum.TableCorpsInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableCorpsMember, typeof(TableCorpsMember));
			s_Type2DataEnum.Add(typeof(TableCorpsMember), (int)DataEnum.TableCorpsMember);
			s_DataEnum2Type.Add((int)DataEnum.TableEquipInfo, typeof(TableEquipInfo));
			s_Type2DataEnum.Add(typeof(TableEquipInfo), (int)DataEnum.TableEquipInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableExpeditionInfo, typeof(TableExpeditionInfo));
			s_Type2DataEnum.Add(typeof(TableExpeditionInfo), (int)DataEnum.TableExpeditionInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableFashionInfo, typeof(TableFashionInfo));
			s_Type2DataEnum.Add(typeof(TableFashionInfo), (int)DataEnum.TableFashionInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableFightInfo, typeof(TableFightInfo));
			s_Type2DataEnum.Add(typeof(TableFightInfo), (int)DataEnum.TableFightInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableFriendInfo, typeof(TableFriendInfo));
			s_Type2DataEnum.Add(typeof(TableFriendInfo), (int)DataEnum.TableFriendInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableGlobalParam, typeof(TableGlobalParam));
			s_Type2DataEnum.Add(typeof(TableGlobalParam), (int)DataEnum.TableGlobalParam);
			s_DataEnum2Type.Add((int)DataEnum.TableGowInfo, typeof(TableGowInfo));
			s_Type2DataEnum.Add(typeof(TableGowInfo), (int)DataEnum.TableGowInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableGowStar, typeof(TableGowStar));
			s_Type2DataEnum.Add(typeof(TableGowStar), (int)DataEnum.TableGowStar);
			s_DataEnum2Type.Add((int)DataEnum.TableGuid, typeof(TableGuid));
			s_Type2DataEnum.Add(typeof(TableGuid), (int)DataEnum.TableGuid);
			s_DataEnum2Type.Add((int)DataEnum.TableHomeNotice, typeof(TableHomeNotice));
			s_Type2DataEnum.Add(typeof(TableHomeNotice), (int)DataEnum.TableHomeNotice);
			s_DataEnum2Type.Add((int)DataEnum.TableInviterInfo, typeof(TableInviterInfo));
			s_Type2DataEnum.Add(typeof(TableInviterInfo), (int)DataEnum.TableInviterInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableItemInfo, typeof(TableItemInfo));
			s_Type2DataEnum.Add(typeof(TableItemInfo), (int)DataEnum.TableItemInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableLegacyInfo, typeof(TableLegacyInfo));
			s_Type2DataEnum.Add(typeof(TableLegacyInfo), (int)DataEnum.TableLegacyInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableLevelInfo, typeof(TableLevelInfo));
			s_Type2DataEnum.Add(typeof(TableLevelInfo), (int)DataEnum.TableLevelInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableLoginLotteryRecord, typeof(TableLoginLotteryRecord));
			s_Type2DataEnum.Add(typeof(TableLoginLotteryRecord), (int)DataEnum.TableLoginLotteryRecord);
			s_DataEnum2Type.Add((int)DataEnum.TableLootInfo, typeof(TableLootInfo));
			s_Type2DataEnum.Add(typeof(TableLootInfo), (int)DataEnum.TableLootInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableLootRecord, typeof(TableLootRecord));
			s_Type2DataEnum.Add(typeof(TableLootRecord), (int)DataEnum.TableLootRecord);
			s_DataEnum2Type.Add((int)DataEnum.TableLotteryInfo, typeof(TableLotteryInfo));
			s_Type2DataEnum.Add(typeof(TableLotteryInfo), (int)DataEnum.TableLotteryInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMailInfo, typeof(TableMailInfo));
			s_Type2DataEnum.Add(typeof(TableMailInfo), (int)DataEnum.TableMailInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMailStateInfo, typeof(TableMailStateInfo));
			s_Type2DataEnum.Add(typeof(TableMailStateInfo), (int)DataEnum.TableMailStateInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMissionInfo, typeof(TableMissionInfo));
			s_Type2DataEnum.Add(typeof(TableMissionInfo), (int)DataEnum.TableMissionInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMpveAwardInfo, typeof(TableMpveAwardInfo));
			s_Type2DataEnum.Add(typeof(TableMpveAwardInfo), (int)DataEnum.TableMpveAwardInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableNickname, typeof(TableNickname));
			s_Type2DataEnum.Add(typeof(TableNickname), (int)DataEnum.TableNickname);
			s_DataEnum2Type.Add((int)DataEnum.TablePartnerInfo, typeof(TablePartnerInfo));
			s_Type2DataEnum.Add(typeof(TablePartnerInfo), (int)DataEnum.TablePartnerInfo);
			s_DataEnum2Type.Add((int)DataEnum.TablePaymentInfo, typeof(TablePaymentInfo));
			s_Type2DataEnum.Add(typeof(TablePaymentInfo), (int)DataEnum.TablePaymentInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableSkillInfo, typeof(TableSkillInfo));
			s_Type2DataEnum.Add(typeof(TableSkillInfo), (int)DataEnum.TableSkillInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableTalentInfo, typeof(TableTalentInfo));
			s_Type2DataEnum.Add(typeof(TableTalentInfo), (int)DataEnum.TableTalentInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableUndonePayment, typeof(TableUndonePayment));
			s_Type2DataEnum.Add(typeof(TableUndonePayment), (int)DataEnum.TableUndonePayment);
			s_DataEnum2Type.Add((int)DataEnum.TableUserBattleInfo, typeof(TableUserBattleInfo));
			s_Type2DataEnum.Add(typeof(TableUserBattleInfo), (int)DataEnum.TableUserBattleInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableUserGeneralInfo, typeof(TableUserGeneralInfo));
			s_Type2DataEnum.Add(typeof(TableUserGeneralInfo), (int)DataEnum.TableUserGeneralInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableUserInfo, typeof(TableUserInfo));
			s_Type2DataEnum.Add(typeof(TableUserInfo), (int)DataEnum.TableUserInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableUserSpecialInfo, typeof(TableUserSpecialInfo));
			s_Type2DataEnum.Add(typeof(TableUserSpecialInfo), (int)DataEnum.TableUserSpecialInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableXSoulInfo, typeof(TableXSoulInfo));
			s_Type2DataEnum.Add(typeof(TableXSoulInfo), (int)DataEnum.TableXSoulInfo);
		}

		private static Dictionary<int, Type> s_DataEnum2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2DataEnum = new Dictionary<Type, int>();
	}
}

