//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkLobbyMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFrameworkMessage
{
	public enum JsonMessageDefine
	{
		Zero,
		NodeRegister = 1,
		NodeRegisterResult,
		VersionVerify,
		VersionVerifyResult,
		DirectLogin,
		AccountLogout,
		Logout,
		GetQueueingCount,
		UserHeartbeat,
		KickUser,
		TooManyOperations,
		Msg_CL_StartGame,
		Msg_CL_FriendList,
		Msg_CL_RoleList,
		Msg_CL_CreateNickname,
		Msg_CL_BuyStamina,
		Msg_CL_BuyLife,
		Msg_CL_GetMailList,
		Msg_CL_QueryExpeditionInfo,
		Msg_CL_ExpeditionFailure,
		Msg_CL_MidasTouch,
		Msg_CL_RequestGroupInfo,
		Msg_CL_QuitPve,
		Msg_CL_RequestVigor,
		Msg_CL_WeeklyLoginReward,
		Msg_CL_GetQueueingCount,
		Msg_CL_RequestGowPrize,
		Msg_CL_GainFirstPayReward,
		Msg_CL_CorpsSignIn,
		Msg_CL_BuyPartnerCombatTicket,
		Msg_CL_RefreshPartnerCombat,
		Msg_CL_GetLootHistory,
		Msg_CL_GetMorrowReward,
		Msg_CL_GetLoginLottery,
		Msg_CL_QuerySkillInfos,
		Msg_CL_CorpsClearRequest,
		Msg_CL_ExpeditionSweep,
		Msg_CL_GMResetDailyMissions,
		Msg_LC_ResetCorpsSignIn,
		Msg_LC_SyncQuitRoom,
		Msg_LC_NoticeQuitGroup,
		Msg_LC_NotifyNewMail,
		Msg_LC_RequestItemUseResult,
		Msg_LC_ServerShutdown,
		Msg_LC_SyncResetGowPrize,
		Msg_LC_ResetConsumeGoodsCount,
		Msg_LC_NotifyMorrowReward,
		Msg_LC_ResetOnlineTimeRewardData,
		Msg_LC_ResetWeeklyLoginRewardData,
		Msg_LC_GmCode,
		Msg_CL_AccountLogin,
		Msg_LC_AccountLoginResult,
		Msg_LC_CreateNicnameResult,
		Msg_CL_CreateRole,
		Msg_LC_CreateRoleResult,
		Msg_CL_ActivateAccount,
		Msg_LC_ActivateAccountResult,
		MailItemForMessage,
		MailInfoForMessage,
		Msg_LC_SyncMailList,
		MissionInfoForSync,
		Msg_LC_ResetDailyMissions,
		Msg_CL_SinglePVE,
		Msg_CL_SaveSkillPreset,
		Msg_LC_StartGameResult,
		Msg_CL_UpdateFightingScore,
		Msg_CL_RoleEnter,
		JsonItemDataMsg,
		SkillDataInfo,
		LegacyDataMsg,
		XSoulDataMsg,
		JsonTalentDataMsg,
		PaymentRebateDataMsg,
		GowDataMsg,
		FashionMsg,
		FashionHideMsg,
		FashionSynMsg,
		Msg_LC_RoleEnterResult,
		MorrowRewardInfo,
		Msg_CL_ExpeditionReset,
		Msg_LC_ExpeditionResetResult,
		Msg_LC_RoleListResult,
		Msg_CL_RequestExpedition,
		Msg_LC_RequestExpeditionResult,
		Msg_CL_FinishExpedition,
		Msg_LC_FinishExpeditionResult,
		Msg_LC_ExpeditionSweepResult,
		Msg_CL_DiscardItem,
		Msg_LC_DiscardItemResult,
		Msg_CL_SellItem,
		Msg_LC_SellItemResult,
		Msg_CL_MountEquipment,
		Msg_LC_MountEquipmentResult,
		Msg_CL_MountSkill,
		Msg_LC_MountSkillResult,
		Msg_CL_UnmountSkill,
		Msg_LC_UnmountSkillResult,
		Msg_CL_UpgradeSkill,
		Msg_LC_UpgradeSkillResult,
		Msg_CL_UnlockSkill,
		Msg_LC_UnlockSkillResult,
		Msg_CL_LiftSkill,
		Msg_LC_LiftSkillResult,
		Msg_CL_UpgradeItem,
		Msg_LC_UpgradeItemResult,
		Msg_CL_SwapSkill,
		Msg_LC_SwapSkillResult,
		Msg_LC_BuyStaminaResult,
		Msg_CL_FinishMission,
		Msg_LC_FinishMissionResult,
		Msg_LC_BuyLifeResult,
		Msg_LC_UnlockLegacyResult,
		Msg_CL_UpgradeLegacy,
		Msg_LC_UpgradeLegacyResult,
		Msg_CL_AddXSoulExperience,
		Msg_LC_AddXSoulExperienceResult,
		Msg_CL_XSoulChangeShowModel,
		Msg_LC_XSoulChangeShowModelResult,
		Msg_CL_ReadMail,
		Msg_CL_ReceiveMail,
		Msg_CL_ExpeditionAward,
		Msg_LC_ExpeditionAwardResult,
		Msg_LC_MidasTouchResult,
		Msg_CL_UnmountEquipment,
		Msg_LC_UnmountEquipmentResult,
		Msg_LC_UserLevelup,
		Msg_LC_SyncStamina,
		Msg_CL_StageClear,
		Msg_LC_StageClearResult,
		Msg_CL_AddAssets,
		Msg_LC_AddAssetsResult,
		Msg_CL_AddItem,
		Msg_LC_SyncGowStarList,
		Msg_CL_RequestGowBattleResult,
		Msg_LC_SyncGowBattleResult,
		Msg_CL_AddFriend,
		FriendInfoForMsg,
		Msg_LC_AddFriendResult,
		Msg_LC_SyncFriendList,
		Msg_CL_DeleteFriend,
		Msg_LC_DelFriendResult,
		Msg_CL_ConfirmFriend,
		Msg_CL_QueryFriendInfo,
		Msg_LC_QueryFriendInfoResult,
		Msg_LC_MissionCompleted,
		Msg_LC_SyncNoticeContent,
		Msg_LC_FriendOnline,
		Msg_LC_FriendOffline,
		Msg_LC_SyncGroupUsers,
		Msg_LC_RequestJoinGroupResult,
		Msg_LC_ConfirmJoinGroupResult,
		Msg_CL_PinviteTeam,
		Msg_CL_RequestJoinGroup,
		Msg_CL_ConfirmJoinGroup,
		Msg_CL_QuitGroup,
		Msg_LC_SyncPinviteTeam,
		Msg_LC_AddItemResult,
		Msg_LC_AddItemsResult,
		Msg_CL_GetGowStarList,
		Msg_LC_SyncLeaveGroup,
		Msg_CL_RefuseGroupRequest,
		Msg_CL_SelectPartner,
		Msg_CL_CancelSelectPartner,
		Msg_LC_CancelSelectPartnerResult,
		Msg_LC_SelectPartnerResult,
		Msg_CL_UpgradePartnerLevel,
		Msg_LC_UpgradePartnerLevelResult,
		Msg_CL_UpgradePartnerStage,
		Msg_LC_UpgradeParnerStageResult,
		Msg_CL_PartnerEquip,
		Msg_LC_PartnerEquipResult,
		Msg_LC_GetPartner,
		Msg_LC_ChangeCaptain,
		Msg_CL_RequestMatch,
		Msg_CL_CancelMatch,
		Msg_LC_MatchResult,
		Msg_LC_MpveGeneralResult,
		Msg_CL_StartMpve,
		Msg_LC_MpveAwardResult,
		Msg_CL_UpdatePosition,
		Msg_CL_RequestUsers,
		Msg_LC_RequestUsersResult,
		Msg_CL_RequestUserPosition,
		Msg_LC_RequestUserPositionResult,
		Msg_CL_ChangeScene,
		Msg_CL_ChangeCityRoom,
		Msg_CL_CompoundPartner,
		Msg_LC_CompoundPartnerResult,
		Msg_CL_SweepStage,
		Msg_LC_SweepStageResult,
		Msg_CL_RequestPlayerInfo,
		Msg_LC_SyncPlayerInfo,
		Msg_CL_UpdateActivityUnlock,
		Msg_CL_RequestPlayerDetail,
		Msg_LC_SyncPlayerDetail,
		Msg_LC_NoticePlayerOffline,
		Msg_CL_ExchangeGoods,
		Msg_LC_ExchangeGoodsResult,
		Msg_CL_SecretAreaTrial,
		Msg_LC_SecretAreaTrialResult,
		Msg_CL_SecretAreaFightingInfo,
		Msg_LC_SecretAreaTrialAward,
		Msg_CL_QuitRoom,
		Msg_LC_SyncVigor,
		Msg_CL_SignInAndGetReward,
		Msg_LC_SignInAndGetRewardResult,
		Msg_LC_SyncSignInCount,
		Msg_LC_SyncMpveInfo,
		Msg_CL_SetNewbieFlag,
		Msg_LC_SyncNewbieFlag,
		PartnerDataMsg,
		ArenaInfoMsg,
		Msg_CL_QueryArenaInfo,
		Msg_LC_ArenaInfoResult,
		Msg_CL_QueryArenaMatchGroup,
		Msg_LC_ArenaMatchGroupResult,
		Msg_LC_SyncGoldTollgateInfo,
		Msg_CL_RequestRefreshExchange,
		Msg_LC_RefreshExchangeResult,
		Msg_CL_DiamondExtraBuyBox,
		Msg_LC_DiamondExtraBuyBoxResult,
		Msg_CL_ArenaStartChallenge,
		Msg_LC_ArenaStartCallengeResult,
		Msg_CL_ArenaChallengeOver,
		Msg_LC_ArenaChallengeResult,
		DamageInfoData,
		ChallengeEntityData,
		ChallengeInfoData,
		Msg_CL_ArenaQueryRank,
		Msg_LC_ArenaQueryRankResult,
		Msg_CL_ArenaQueryHistory,
		Msg_LC_ArenaQueryHistoryResult,
		Msg_CL_ArenaChangePartner,
		Msg_LC_ArenaChangePartnerResult,
		Msg_CL_ArenaBuyFightCount,
		Msg_LC_ArenaBuyFightCountResult,
		Msg_CL_ArenaBeginFight,
		Msg_CL_ExchangeGift,
		Msg_LC_ExchangeGiftResult,
		Msg_CL_CompoundEquip,
		Msg_LC_CompoundEquipResult,
		Msg_CL_SetNewbieActionFlag,
		Msg_LC_SyncNewbieActionFlag,
		Msg_LC_WeeklyLoginRewardResult,
		Msg_LC_QueueingCountResult,
		Msg_CL_GetOnlineTimeReward,
		Msg_LC_GetOnlineTimeRewardResult,
		Msg_CL_RecordNewbieFlag,
		SelectedPartnerDataMsg,
		Msg_CL_RequestSkillInfos,
		Msg_LC_SyncSkillInfos,
		Msg_LC_SyncCombatData,
		Msg_CL_UploadFPS,
		Msg_LC_SyncGuideFlag,
		Msg_CL_RequestDare,
		Msg_LC_RequestDare,
		Msg_CL_RequestDareByGuid,
		Msg_LC_RequestDareResult,
		Msg_CL_AcceptedDare,
		Msg_LC_RequestGowPrizeResult,
		Msg_CL_RequestEnhanceEquipmentStar,
		Msg_LC_RequestEnhanceEquipmentStar,
		Msg_LC_SyncCorpsOpResult,
		Msg_CL_CorpsCreate,
		Msg_CL_CorpsJoin,
		Msg_CL_CorpsQuit,
		Msg_CL_CorpsAgreeClaimer,
		Msg_CL_CorpsRefuseClaimer,
		Msg_CL_CorpsKickout,
		Msg_CL_CorpsAppoint,
		Msg_CL_QueryCorpsInfo,
		Msg_LC_SyncCorpsInfo,
		Msg_CL_QueryCorpsStar,
		Msg_LC_SyncCorpsStar,
		Msg_CL_CorpsDissolve,
		Msg_CL_EquipTalentCard,
		Msg_LC_EquipTalentCardResult,
		Msg_CLC_UnequipTalentCard,
		Msg_CL_AddTalentExperience,
		ItemLeftMsg,
		Msg_LC_AddTalentExperienceResult,
		Msg_CL_UpgradeEquipBatch,
		Msg_LC_UpgradeEquipBatch,
		Msg_CL_RequestMpveAward,
		Msg_CL_SendChat,
		Msg_LC_ChatStatus,
		Msg_LC_ChatResult,
		Msg_LC_ChatWorldResult,
		Msg_LC_SystemChatWorldResult,
		Msg_CL_ChatAddShield,
		Msg_CL_ChatAddShieldByName,
		Msg_LC_ChatAddShieldResult,
		Msg_CL_ChatDelShield,
		Msg_LC_ChatDelShieldResult,
		Msg_CL_RequireChatRoleInfo,
		Msg_LC_ChatRoleInfoReturn,
		Msg_CL_RequireChatEquipInfo,
		Msg_LC_ChatEquipInfoReturn,
		Msg_CL_RequireChatShieldList,
		ChatShieldInfoForMsg,
		Msg_LC_ChatShieldListReturn,
		Msg_LC_SendScreenTip,
		Msg_CL_SetCorpsNotice,
		Msg_CL_GmPay,
		Msg_LC_GmPayResult,
		Msg_CL_GainVipReward,
		Msg_LC_GainVipRewardResult,
		Msg_CL_QueryValidCorpsList,
		Msg_LC_SyncValidCorpsList,
		Msg_LC_SyncGowRankInfo,
		Msg_LC_GainFirstPayRewardResult,
		Msg_CL_BuyFashion,
		Msg_LC_BuyFashionResult,
		Msg_LC_AwardFashionResult,
		Msg_CL_MountFashion,
		Msg_LC_MountFashionResult,
		Msg_CL_UnmountFashion,
		Msg_LC_UnmountFashionResult,
		Msg_LC_NoticeFashionOverdueSoon,
		Msg_LC_NoticeFashionOverdue,
		Msg_LC_BuyGoodsSucceed,
		Msg_LC_DrawRewardResult,
		Msg_CL_DrawReward,
		Msg_CL_CombinTalentCard,
		Msg_CL_CombinTalentCardResult,
		Msg_CL_BuyEliteCount,
		Msg_LC_BuyEliteCountResult,
		Msg_CL_StartPartnerBattle,
		Msg_LC_StartPartnerBattleResult,
		Msg_CL_EndPartnerBattle,
		Msg_LC_EndPartnerBattleResult,
		Msg_LC_BuyPartnerCombatTicketResult,
		Msg_CL_CorpsIndirectJoin,
		TDMatchInfoMsg,
		Msg_CL_QueryTDMatchGroup,
		Msg_LC_QueryTDMatchGroupResult,
		Msg_CL_StartTDChallenge,
		Msg_LC_StartTDChallengeResult,
		Msg_CL_TDBeginFight,
		Msg_CL_TDChallengeOver,
		Msg_LC_TDChallengeResult,
		Msg_CL_BuyTDFightCount,
		Msg_LC_BuyTDFightCountResult,
		Msg_LC_SyncVitality,
		Msg_LC_CorpsSignIn,
		Msg_LC_PartnerCombatInfo,
		FightingScoreEntityMsg,
		Msg_CLC_QueryFightingScoreRank,
		Msg_CL_OpenDomain,
		Msg_LC_LootOpResult,
		LootInfoMsg,
		Msg_LC_SyncLootInfo,
		Msg_LC_LootMatchResult,
		Msg_LC_LootChangeDefenseOrder,
		Msg_CL_StartLoot,
		Msg_LC_StartLootResult,
		Msg_CL_OverLoot,
		LootEntityData,
		Msg_LC_OverLootResult,
		LootHistoryData,
		Msg_LC_SyncLootHistory,
		Msg_LC_SyncLootAward,
		Msg_CL_RefusedDare,
		Msg_LC_SyncLotteryInfo,
		Msg_CL_EquipmentStrength,
		Msg_LC_EquipmentStrengthResult,
		Msg_CL_SetFashionShow,
		Msg_LC_SetFashionShowResult,
		Msg_LC_MorrowRewardActive,
		Msg_LC_GetMorrowRewardResult,
		Msg_LC_SyncHomeNotice,
		Msg_LC_LootCostVitality,
		Msg_CL_QueryLootInfo,
		Msg_CL_CostVitality,
		Msg_CL_LootMatchTarget,
		Msg_CL_GetLootAward,
		Msg_LC_SyncRecentLoginState,
		Msg_LC_SyncFightingScore,
		Msg_LC_GetLoginLotteryResult,
		Msg_LC_RefreshPartnerCombatResult,
		Msg_LC_LootChangeLootOrder,
		Msg_LC_ItemsGiveBack,
		Msg_CL_QueryCorpsByName,
		Msg_LC_QueryCorpsByName,
		Msg_CLC_StoryMessage,
		Msg_CL_OpenCharpter,
		Msg_CL_ResetCharpter,
		Msg_CL_StartCorpsTollgate,
		Msg_LC_InviteInfoAfterRoleEnter,
		Msg_CL_RequestInvite,
		Msg_LC_RequestInviteResult,
		Msg_LC_UpdateInviteInfo,
		Msg_CL_RequestInviteReward,
		Msg_LC_RequestInviteRewardResult,
		Msg_CLC_CollectGrowthFund,
		Msg_LC_SyncCorpsDareCount,
		Msg_CLC_CollectChapterAward,
		Msg_CL_InteractivePrize,
		Msg_LC_InteractivePrize,
		Msg_CL_PushDelayData,
		Msg_CL_EnterField,
		Msg_CL_ChangeFieldRoom,
		Msg_LC_LackOfSpace,
		Msg_LC_SyncGowOtherInfo,
		ItemInfo_UseItem,
		Msg_CL_ItemUseRequest,
		Msg_LC_ItemUseResult,
		Msg_CL_AuctionQuery,
		Msg_CL_AuctionSell,
		Msg_CL_AuctionBuy,
		Msg_CL_AuctionUnshelve,
		Msg_CL_AuctionReceive,
		Msg_CL_AuctionSelfAuction,
		AuctionInfo,
		Msg_LC_AuctionSelfAuction,
		Msg_LC_AuctionAuction,
		Msg_LC_AuctionOpResult,
		MaxNum
	}
	public static class JsonMessageDefine2Object
	{
		public static object New(int id)
		{
			object r = null;
			MyFunc<object> t;
			if(s_JsonMessageDefine2Object.TryGetValue(id, out t)){
				r = t();
			}
			return r;
		}

		static JsonMessageDefine2Object()
		{
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.AccountLogout, () => new AccountLogout());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ArenaInfoMsg, () => new ArenaInfoMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.AuctionInfo, () => new AuctionInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ChallengeEntityData, () => new ChallengeEntityData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ChallengeInfoData, () => new ChallengeInfoData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ChatShieldInfoForMsg, () => new ChatShieldInfoForMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.DamageInfoData, () => new DamageInfoData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.DirectLogin, () => new DirectLogin());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.FashionHideMsg, () => new FashionHideMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.FashionMsg, () => new FashionMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.FashionSynMsg, () => new FashionSynMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.FightingScoreEntityMsg, () => new FightingScoreEntityMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.FriendInfoForMsg, () => new FriendInfoForMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.GetQueueingCount, () => new GetQueueingCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.GowDataMsg, () => new GowDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ItemInfo_UseItem, () => new ItemInfo_UseItem());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.ItemLeftMsg, () => new ItemLeftMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.JsonItemDataMsg, () => new JsonItemDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.JsonTalentDataMsg, () => new JsonTalentDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.KickUser, () => new KickUser());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.LegacyDataMsg, () => new LegacyDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Logout, () => new Logout());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.LootEntityData, () => new LootEntityData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.LootHistoryData, () => new LootHistoryData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.LootInfoMsg, () => new LootInfoMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.MailInfoForMessage, () => new MailInfoForMessage());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.MailItemForMessage, () => new MailItemForMessage());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.MissionInfoForSync, () => new MissionInfoForSync());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.MorrowRewardInfo, () => new MorrowRewardInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AcceptedDare, () => new Msg_CL_AcceptedDare());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AccountLogin, () => new Msg_CL_AccountLogin());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ActivateAccount, () => new Msg_CL_ActivateAccount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AddAssets, () => new Msg_CL_AddAssets());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AddFriend, () => new Msg_CL_AddFriend());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AddItem, () => new Msg_CL_AddItem());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AddTalentExperience, () => new Msg_CL_AddTalentExperience());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AddXSoulExperience, () => new Msg_CL_AddXSoulExperience());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaBeginFight, () => new Msg_CL_ArenaBeginFight());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaBuyFightCount, () => new Msg_CL_ArenaBuyFightCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaChallengeOver, () => new Msg_CL_ArenaChallengeOver());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaChangePartner, () => new Msg_CL_ArenaChangePartner());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaQueryHistory, () => new Msg_CL_ArenaQueryHistory());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaQueryRank, () => new Msg_CL_ArenaQueryRank());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ArenaStartChallenge, () => new Msg_CL_ArenaStartChallenge());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionBuy, () => new Msg_CL_AuctionBuy());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionQuery, () => new Msg_CL_AuctionQuery());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionReceive, () => new Msg_CL_AuctionReceive());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionSelfAuction, () => new Msg_CL_AuctionSelfAuction());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionSell, () => new Msg_CL_AuctionSell());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_AuctionUnshelve, () => new Msg_CL_AuctionUnshelve());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyEliteCount, () => new Msg_CL_BuyEliteCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyFashion, () => new Msg_CL_BuyFashion());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyLife, () => new Msg_CL_BuyLife());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyPartnerCombatTicket, () => new Msg_CL_BuyPartnerCombatTicket());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyStamina, () => new Msg_CL_BuyStamina());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_BuyTDFightCount, () => new Msg_CL_BuyTDFightCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CancelMatch, () => new Msg_CL_CancelMatch());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CancelSelectPartner, () => new Msg_CL_CancelSelectPartner());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChangeCityRoom, () => new Msg_CL_ChangeCityRoom());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChangeFieldRoom, () => new Msg_CL_ChangeFieldRoom());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChangeScene, () => new Msg_CL_ChangeScene());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChatAddShield, () => new Msg_CL_ChatAddShield());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChatAddShieldByName, () => new Msg_CL_ChatAddShieldByName());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ChatDelShield, () => new Msg_CL_ChatDelShield());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CombinTalentCard, () => new Msg_CL_CombinTalentCard());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CombinTalentCardResult, () => new Msg_CL_CombinTalentCardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CompoundEquip, () => new Msg_CL_CompoundEquip());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CompoundPartner, () => new Msg_CL_CompoundPartner());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ConfirmFriend, () => new Msg_CL_ConfirmFriend());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ConfirmJoinGroup, () => new Msg_CL_ConfirmJoinGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsAgreeClaimer, () => new Msg_CL_CorpsAgreeClaimer());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsAppoint, () => new Msg_CL_CorpsAppoint());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsClearRequest, () => new Msg_CL_CorpsClearRequest());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsCreate, () => new Msg_CL_CorpsCreate());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsDissolve, () => new Msg_CL_CorpsDissolve());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsIndirectJoin, () => new Msg_CL_CorpsIndirectJoin());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsJoin, () => new Msg_CL_CorpsJoin());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsKickout, () => new Msg_CL_CorpsKickout());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsQuit, () => new Msg_CL_CorpsQuit());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsRefuseClaimer, () => new Msg_CL_CorpsRefuseClaimer());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CorpsSignIn, () => new Msg_CL_CorpsSignIn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CostVitality, () => new Msg_CL_CostVitality());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CreateNickname, () => new Msg_CL_CreateNickname());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_CreateRole, () => new Msg_CL_CreateRole());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_DeleteFriend, () => new Msg_CL_DeleteFriend());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_DiamondExtraBuyBox, () => new Msg_CL_DiamondExtraBuyBox());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_DiscardItem, () => new Msg_CL_DiscardItem());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_DrawReward, () => new Msg_CL_DrawReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_EndPartnerBattle, () => new Msg_CL_EndPartnerBattle());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_EnterField, () => new Msg_CL_EnterField());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_EquipmentStrength, () => new Msg_CL_EquipmentStrength());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_EquipTalentCard, () => new Msg_CL_EquipTalentCard());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExchangeGift, () => new Msg_CL_ExchangeGift());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExchangeGoods, () => new Msg_CL_ExchangeGoods());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExpeditionAward, () => new Msg_CL_ExpeditionAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExpeditionFailure, () => new Msg_CL_ExpeditionFailure());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExpeditionReset, () => new Msg_CL_ExpeditionReset());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ExpeditionSweep, () => new Msg_CL_ExpeditionSweep());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_FinishExpedition, () => new Msg_CL_FinishExpedition());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_FinishMission, () => new Msg_CL_FinishMission());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_FriendList, () => new Msg_CL_FriendList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GainFirstPayReward, () => new Msg_CL_GainFirstPayReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GainVipReward, () => new Msg_CL_GainVipReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetGowStarList, () => new Msg_CL_GetGowStarList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetLoginLottery, () => new Msg_CL_GetLoginLottery());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetLootAward, () => new Msg_CL_GetLootAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetLootHistory, () => new Msg_CL_GetLootHistory());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetMailList, () => new Msg_CL_GetMailList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetMorrowReward, () => new Msg_CL_GetMorrowReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetOnlineTimeReward, () => new Msg_CL_GetOnlineTimeReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GetQueueingCount, () => new Msg_CL_GetQueueingCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GmPay, () => new Msg_CL_GmPay());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_GMResetDailyMissions, () => new Msg_CL_GMResetDailyMissions());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_InteractivePrize, () => new Msg_CL_InteractivePrize());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ItemUseRequest, () => new Msg_CL_ItemUseRequest());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_LiftSkill, () => new Msg_CL_LiftSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_LootMatchTarget, () => new Msg_CL_LootMatchTarget());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_MidasTouch, () => new Msg_CL_MidasTouch());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_MountEquipment, () => new Msg_CL_MountEquipment());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_MountFashion, () => new Msg_CL_MountFashion());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_MountSkill, () => new Msg_CL_MountSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_OpenCharpter, () => new Msg_CL_OpenCharpter());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_OpenDomain, () => new Msg_CL_OpenDomain());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_OverLoot, () => new Msg_CL_OverLoot());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_PartnerEquip, () => new Msg_CL_PartnerEquip());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_PinviteTeam, () => new Msg_CL_PinviteTeam());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_PushDelayData, () => new Msg_CL_PushDelayData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryArenaInfo, () => new Msg_CL_QueryArenaInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryArenaMatchGroup, () => new Msg_CL_QueryArenaMatchGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryCorpsByName, () => new Msg_CL_QueryCorpsByName());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryCorpsInfo, () => new Msg_CL_QueryCorpsInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryCorpsStar, () => new Msg_CL_QueryCorpsStar());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryExpeditionInfo, () => new Msg_CL_QueryExpeditionInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryFriendInfo, () => new Msg_CL_QueryFriendInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryLootInfo, () => new Msg_CL_QueryLootInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QuerySkillInfos, () => new Msg_CL_QuerySkillInfos());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryTDMatchGroup, () => new Msg_CL_QueryTDMatchGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QueryValidCorpsList, () => new Msg_CL_QueryValidCorpsList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QuitGroup, () => new Msg_CL_QuitGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QuitPve, () => new Msg_CL_QuitPve());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_QuitRoom, () => new Msg_CL_QuitRoom());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ReadMail, () => new Msg_CL_ReadMail());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ReceiveMail, () => new Msg_CL_ReceiveMail());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RecordNewbieFlag, () => new Msg_CL_RecordNewbieFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RefreshPartnerCombat, () => new Msg_CL_RefreshPartnerCombat());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RefusedDare, () => new Msg_CL_RefusedDare());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RefuseGroupRequest, () => new Msg_CL_RefuseGroupRequest());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestDare, () => new Msg_CL_RequestDare());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestDareByGuid, () => new Msg_CL_RequestDareByGuid());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestEnhanceEquipmentStar, () => new Msg_CL_RequestEnhanceEquipmentStar());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestExpedition, () => new Msg_CL_RequestExpedition());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestGowBattleResult, () => new Msg_CL_RequestGowBattleResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestGowPrize, () => new Msg_CL_RequestGowPrize());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestGroupInfo, () => new Msg_CL_RequestGroupInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestInvite, () => new Msg_CL_RequestInvite());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestInviteReward, () => new Msg_CL_RequestInviteReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestJoinGroup, () => new Msg_CL_RequestJoinGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestMatch, () => new Msg_CL_RequestMatch());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestMpveAward, () => new Msg_CL_RequestMpveAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestPlayerDetail, () => new Msg_CL_RequestPlayerDetail());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestPlayerInfo, () => new Msg_CL_RequestPlayerInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestRefreshExchange, () => new Msg_CL_RequestRefreshExchange());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestSkillInfos, () => new Msg_CL_RequestSkillInfos());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestUserPosition, () => new Msg_CL_RequestUserPosition());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestUsers, () => new Msg_CL_RequestUsers());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequestVigor, () => new Msg_CL_RequestVigor());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequireChatEquipInfo, () => new Msg_CL_RequireChatEquipInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequireChatRoleInfo, () => new Msg_CL_RequireChatRoleInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RequireChatShieldList, () => new Msg_CL_RequireChatShieldList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_ResetCharpter, () => new Msg_CL_ResetCharpter());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RoleEnter, () => new Msg_CL_RoleEnter());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_RoleList, () => new Msg_CL_RoleList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SaveSkillPreset, () => new Msg_CL_SaveSkillPreset());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SecretAreaFightingInfo, () => new Msg_CL_SecretAreaFightingInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SecretAreaTrial, () => new Msg_CL_SecretAreaTrial());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SelectPartner, () => new Msg_CL_SelectPartner());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SellItem, () => new Msg_CL_SellItem());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SendChat, () => new Msg_CL_SendChat());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SetCorpsNotice, () => new Msg_CL_SetCorpsNotice());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SetFashionShow, () => new Msg_CL_SetFashionShow());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SetNewbieActionFlag, () => new Msg_CL_SetNewbieActionFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SetNewbieFlag, () => new Msg_CL_SetNewbieFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SignInAndGetReward, () => new Msg_CL_SignInAndGetReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SinglePVE, () => new Msg_CL_SinglePVE());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StageClear, () => new Msg_CL_StageClear());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartCorpsTollgate, () => new Msg_CL_StartCorpsTollgate());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartGame, () => new Msg_CL_StartGame());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartLoot, () => new Msg_CL_StartLoot());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartMpve, () => new Msg_CL_StartMpve());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartPartnerBattle, () => new Msg_CL_StartPartnerBattle());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_StartTDChallenge, () => new Msg_CL_StartTDChallenge());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SwapSkill, () => new Msg_CL_SwapSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_SweepStage, () => new Msg_CL_SweepStage());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_TDBeginFight, () => new Msg_CL_TDBeginFight());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_TDChallengeOver, () => new Msg_CL_TDChallengeOver());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UnlockSkill, () => new Msg_CL_UnlockSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UnmountEquipment, () => new Msg_CL_UnmountEquipment());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UnmountFashion, () => new Msg_CL_UnmountFashion());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UnmountSkill, () => new Msg_CL_UnmountSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpdateActivityUnlock, () => new Msg_CL_UpdateActivityUnlock());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpdateFightingScore, () => new Msg_CL_UpdateFightingScore());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpdatePosition, () => new Msg_CL_UpdatePosition());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradeEquipBatch, () => new Msg_CL_UpgradeEquipBatch());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradeItem, () => new Msg_CL_UpgradeItem());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradeLegacy, () => new Msg_CL_UpgradeLegacy());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradePartnerLevel, () => new Msg_CL_UpgradePartnerLevel());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradePartnerStage, () => new Msg_CL_UpgradePartnerStage());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UpgradeSkill, () => new Msg_CL_UpgradeSkill());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_UploadFPS, () => new Msg_CL_UploadFPS());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_WeeklyLoginReward, () => new Msg_CL_WeeklyLoginReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CL_XSoulChangeShowModel, () => new Msg_CL_XSoulChangeShowModel());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CLC_CollectChapterAward, () => new Msg_CLC_CollectChapterAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CLC_CollectGrowthFund, () => new Msg_CLC_CollectGrowthFund());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CLC_QueryFightingScoreRank, () => new Msg_CLC_QueryFightingScoreRank());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CLC_StoryMessage, () => new Msg_CLC_StoryMessage());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_CLC_UnequipTalentCard, () => new Msg_CLC_UnequipTalentCard());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AccountLoginResult, () => new Msg_LC_AccountLoginResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ActivateAccountResult, () => new Msg_LC_ActivateAccountResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddAssetsResult, () => new Msg_LC_AddAssetsResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddFriendResult, () => new Msg_LC_AddFriendResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddItemResult, () => new Msg_LC_AddItemResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddItemsResult, () => new Msg_LC_AddItemsResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddTalentExperienceResult, () => new Msg_LC_AddTalentExperienceResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AddXSoulExperienceResult, () => new Msg_LC_AddXSoulExperienceResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaBuyFightCountResult, () => new Msg_LC_ArenaBuyFightCountResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaChallengeResult, () => new Msg_LC_ArenaChallengeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaChangePartnerResult, () => new Msg_LC_ArenaChangePartnerResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaInfoResult, () => new Msg_LC_ArenaInfoResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaMatchGroupResult, () => new Msg_LC_ArenaMatchGroupResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaQueryHistoryResult, () => new Msg_LC_ArenaQueryHistoryResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaQueryRankResult, () => new Msg_LC_ArenaQueryRankResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ArenaStartCallengeResult, () => new Msg_LC_ArenaStartCallengeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AuctionAuction, () => new Msg_LC_AuctionAuction());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AuctionOpResult, () => new Msg_LC_AuctionOpResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AuctionSelfAuction, () => new Msg_LC_AuctionSelfAuction());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_AwardFashionResult, () => new Msg_LC_AwardFashionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyEliteCountResult, () => new Msg_LC_BuyEliteCountResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyFashionResult, () => new Msg_LC_BuyFashionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyGoodsSucceed, () => new Msg_LC_BuyGoodsSucceed());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyLifeResult, () => new Msg_LC_BuyLifeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyPartnerCombatTicketResult, () => new Msg_LC_BuyPartnerCombatTicketResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyStaminaResult, () => new Msg_LC_BuyStaminaResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_BuyTDFightCountResult, () => new Msg_LC_BuyTDFightCountResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CancelSelectPartnerResult, () => new Msg_LC_CancelSelectPartnerResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChangeCaptain, () => new Msg_LC_ChangeCaptain());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatAddShieldResult, () => new Msg_LC_ChatAddShieldResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatDelShieldResult, () => new Msg_LC_ChatDelShieldResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatEquipInfoReturn, () => new Msg_LC_ChatEquipInfoReturn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatResult, () => new Msg_LC_ChatResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatRoleInfoReturn, () => new Msg_LC_ChatRoleInfoReturn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatShieldListReturn, () => new Msg_LC_ChatShieldListReturn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatStatus, () => new Msg_LC_ChatStatus());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ChatWorldResult, () => new Msg_LC_ChatWorldResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CompoundEquipResult, () => new Msg_LC_CompoundEquipResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CompoundPartnerResult, () => new Msg_LC_CompoundPartnerResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ConfirmJoinGroupResult, () => new Msg_LC_ConfirmJoinGroupResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CorpsSignIn, () => new Msg_LC_CorpsSignIn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CreateNicnameResult, () => new Msg_LC_CreateNicnameResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_CreateRoleResult, () => new Msg_LC_CreateRoleResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_DelFriendResult, () => new Msg_LC_DelFriendResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_DiamondExtraBuyBoxResult, () => new Msg_LC_DiamondExtraBuyBoxResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_DiscardItemResult, () => new Msg_LC_DiscardItemResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_DrawRewardResult, () => new Msg_LC_DrawRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_EndPartnerBattleResult, () => new Msg_LC_EndPartnerBattleResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_EquipmentStrengthResult, () => new Msg_LC_EquipmentStrengthResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_EquipTalentCardResult, () => new Msg_LC_EquipTalentCardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ExchangeGiftResult, () => new Msg_LC_ExchangeGiftResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ExchangeGoodsResult, () => new Msg_LC_ExchangeGoodsResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ExpeditionAwardResult, () => new Msg_LC_ExpeditionAwardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ExpeditionResetResult, () => new Msg_LC_ExpeditionResetResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ExpeditionSweepResult, () => new Msg_LC_ExpeditionSweepResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_FinishExpeditionResult, () => new Msg_LC_FinishExpeditionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_FinishMissionResult, () => new Msg_LC_FinishMissionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_FriendOffline, () => new Msg_LC_FriendOffline());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_FriendOnline, () => new Msg_LC_FriendOnline());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GainFirstPayRewardResult, () => new Msg_LC_GainFirstPayRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GainVipRewardResult, () => new Msg_LC_GainVipRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GetLoginLotteryResult, () => new Msg_LC_GetLoginLotteryResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GetMorrowRewardResult, () => new Msg_LC_GetMorrowRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GetOnlineTimeRewardResult, () => new Msg_LC_GetOnlineTimeRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GetPartner, () => new Msg_LC_GetPartner());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GmCode, () => new Msg_LC_GmCode());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_GmPayResult, () => new Msg_LC_GmPayResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_InteractivePrize, () => new Msg_LC_InteractivePrize());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_InviteInfoAfterRoleEnter, () => new Msg_LC_InviteInfoAfterRoleEnter());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ItemsGiveBack, () => new Msg_LC_ItemsGiveBack());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ItemUseResult, () => new Msg_LC_ItemUseResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LackOfSpace, () => new Msg_LC_LackOfSpace());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LiftSkillResult, () => new Msg_LC_LiftSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LootChangeDefenseOrder, () => new Msg_LC_LootChangeDefenseOrder());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LootChangeLootOrder, () => new Msg_LC_LootChangeLootOrder());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LootCostVitality, () => new Msg_LC_LootCostVitality());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LootMatchResult, () => new Msg_LC_LootMatchResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_LootOpResult, () => new Msg_LC_LootOpResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MatchResult, () => new Msg_LC_MatchResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MidasTouchResult, () => new Msg_LC_MidasTouchResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MissionCompleted, () => new Msg_LC_MissionCompleted());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MorrowRewardActive, () => new Msg_LC_MorrowRewardActive());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MountEquipmentResult, () => new Msg_LC_MountEquipmentResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MountFashionResult, () => new Msg_LC_MountFashionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MountSkillResult, () => new Msg_LC_MountSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MpveAwardResult, () => new Msg_LC_MpveAwardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_MpveGeneralResult, () => new Msg_LC_MpveGeneralResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NoticeFashionOverdue, () => new Msg_LC_NoticeFashionOverdue());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NoticeFashionOverdueSoon, () => new Msg_LC_NoticeFashionOverdueSoon());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NoticePlayerOffline, () => new Msg_LC_NoticePlayerOffline());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NoticeQuitGroup, () => new Msg_LC_NoticeQuitGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NotifyMorrowReward, () => new Msg_LC_NotifyMorrowReward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_NotifyNewMail, () => new Msg_LC_NotifyNewMail());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_OverLootResult, () => new Msg_LC_OverLootResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_PartnerCombatInfo, () => new Msg_LC_PartnerCombatInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_PartnerEquipResult, () => new Msg_LC_PartnerEquipResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_QueryCorpsByName, () => new Msg_LC_QueryCorpsByName());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_QueryFriendInfoResult, () => new Msg_LC_QueryFriendInfoResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_QueryTDMatchGroupResult, () => new Msg_LC_QueryTDMatchGroupResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_QueueingCountResult, () => new Msg_LC_QueueingCountResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RefreshExchangeResult, () => new Msg_LC_RefreshExchangeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RefreshPartnerCombatResult, () => new Msg_LC_RefreshPartnerCombatResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestDare, () => new Msg_LC_RequestDare());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestDareResult, () => new Msg_LC_RequestDareResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestEnhanceEquipmentStar, () => new Msg_LC_RequestEnhanceEquipmentStar());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestExpeditionResult, () => new Msg_LC_RequestExpeditionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestGowPrizeResult, () => new Msg_LC_RequestGowPrizeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestInviteResult, () => new Msg_LC_RequestInviteResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestInviteRewardResult, () => new Msg_LC_RequestInviteRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestItemUseResult, () => new Msg_LC_RequestItemUseResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestJoinGroupResult, () => new Msg_LC_RequestJoinGroupResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestUserPositionResult, () => new Msg_LC_RequestUserPositionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RequestUsersResult, () => new Msg_LC_RequestUsersResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ResetConsumeGoodsCount, () => new Msg_LC_ResetConsumeGoodsCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ResetCorpsSignIn, () => new Msg_LC_ResetCorpsSignIn());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ResetDailyMissions, () => new Msg_LC_ResetDailyMissions());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ResetOnlineTimeRewardData, () => new Msg_LC_ResetOnlineTimeRewardData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ResetWeeklyLoginRewardData, () => new Msg_LC_ResetWeeklyLoginRewardData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RoleEnterResult, () => new Msg_LC_RoleEnterResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_RoleListResult, () => new Msg_LC_RoleListResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SecretAreaTrialAward, () => new Msg_LC_SecretAreaTrialAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SecretAreaTrialResult, () => new Msg_LC_SecretAreaTrialResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SelectPartnerResult, () => new Msg_LC_SelectPartnerResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SellItemResult, () => new Msg_LC_SellItemResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SendScreenTip, () => new Msg_LC_SendScreenTip());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_ServerShutdown, () => new Msg_LC_ServerShutdown());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SetFashionShowResult, () => new Msg_LC_SetFashionShowResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SignInAndGetRewardResult, () => new Msg_LC_SignInAndGetRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_StageClearResult, () => new Msg_LC_StageClearResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_StartGameResult, () => new Msg_LC_StartGameResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_StartLootResult, () => new Msg_LC_StartLootResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_StartPartnerBattleResult, () => new Msg_LC_StartPartnerBattleResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_StartTDChallengeResult, () => new Msg_LC_StartTDChallengeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SwapSkillResult, () => new Msg_LC_SwapSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SweepStageResult, () => new Msg_LC_SweepStageResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncCombatData, () => new Msg_LC_SyncCombatData());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncCorpsDareCount, () => new Msg_LC_SyncCorpsDareCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncCorpsInfo, () => new Msg_LC_SyncCorpsInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncCorpsOpResult, () => new Msg_LC_SyncCorpsOpResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncCorpsStar, () => new Msg_LC_SyncCorpsStar());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncFightingScore, () => new Msg_LC_SyncFightingScore());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncFriendList, () => new Msg_LC_SyncFriendList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGoldTollgateInfo, () => new Msg_LC_SyncGoldTollgateInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGowBattleResult, () => new Msg_LC_SyncGowBattleResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGowOtherInfo, () => new Msg_LC_SyncGowOtherInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGowRankInfo, () => new Msg_LC_SyncGowRankInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGowStarList, () => new Msg_LC_SyncGowStarList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGroupUsers, () => new Msg_LC_SyncGroupUsers());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncGuideFlag, () => new Msg_LC_SyncGuideFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncHomeNotice, () => new Msg_LC_SyncHomeNotice());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncLeaveGroup, () => new Msg_LC_SyncLeaveGroup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncLootAward, () => new Msg_LC_SyncLootAward());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncLootHistory, () => new Msg_LC_SyncLootHistory());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncLootInfo, () => new Msg_LC_SyncLootInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncLotteryInfo, () => new Msg_LC_SyncLotteryInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncMailList, () => new Msg_LC_SyncMailList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncMpveInfo, () => new Msg_LC_SyncMpveInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncNewbieActionFlag, () => new Msg_LC_SyncNewbieActionFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncNewbieFlag, () => new Msg_LC_SyncNewbieFlag());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncNoticeContent, () => new Msg_LC_SyncNoticeContent());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncPinviteTeam, () => new Msg_LC_SyncPinviteTeam());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncPlayerDetail, () => new Msg_LC_SyncPlayerDetail());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncPlayerInfo, () => new Msg_LC_SyncPlayerInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncQuitRoom, () => new Msg_LC_SyncQuitRoom());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncRecentLoginState, () => new Msg_LC_SyncRecentLoginState());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncResetGowPrize, () => new Msg_LC_SyncResetGowPrize());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncSignInCount, () => new Msg_LC_SyncSignInCount());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncSkillInfos, () => new Msg_LC_SyncSkillInfos());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncStamina, () => new Msg_LC_SyncStamina());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncValidCorpsList, () => new Msg_LC_SyncValidCorpsList());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncVigor, () => new Msg_LC_SyncVigor());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SyncVitality, () => new Msg_LC_SyncVitality());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_SystemChatWorldResult, () => new Msg_LC_SystemChatWorldResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_TDChallengeResult, () => new Msg_LC_TDChallengeResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UnlockLegacyResult, () => new Msg_LC_UnlockLegacyResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UnlockSkillResult, () => new Msg_LC_UnlockSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UnmountEquipmentResult, () => new Msg_LC_UnmountEquipmentResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UnmountFashionResult, () => new Msg_LC_UnmountFashionResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UnmountSkillResult, () => new Msg_LC_UnmountSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpdateInviteInfo, () => new Msg_LC_UpdateInviteInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradeEquipBatch, () => new Msg_LC_UpgradeEquipBatch());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradeItemResult, () => new Msg_LC_UpgradeItemResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradeLegacyResult, () => new Msg_LC_UpgradeLegacyResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradeParnerStageResult, () => new Msg_LC_UpgradeParnerStageResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradePartnerLevelResult, () => new Msg_LC_UpgradePartnerLevelResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UpgradeSkillResult, () => new Msg_LC_UpgradeSkillResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_UserLevelup, () => new Msg_LC_UserLevelup());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_WeeklyLoginRewardResult, () => new Msg_LC_WeeklyLoginRewardResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.Msg_LC_XSoulChangeShowModelResult, () => new Msg_LC_XSoulChangeShowModelResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.NodeRegister, () => new NodeRegister());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.NodeRegisterResult, () => new NodeRegisterResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.PartnerDataMsg, () => new PartnerDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.PaymentRebateDataMsg, () => new PaymentRebateDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.SelectedPartnerDataMsg, () => new SelectedPartnerDataMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.SkillDataInfo, () => new SkillDataInfo());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.TDMatchInfoMsg, () => new TDMatchInfoMsg());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.TooManyOperations, () => new TooManyOperations());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.UserHeartbeat, () => new UserHeartbeat());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.VersionVerify, () => new VersionVerify());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.VersionVerifyResult, () => new VersionVerifyResult());
			s_JsonMessageDefine2Object.Add((int)JsonMessageDefine.XSoulDataMsg, () => new XSoulDataMsg());
		}

		private static Dictionary<int, MyFunc<object>> s_JsonMessageDefine2Object = new Dictionary<int, MyFunc<object>>();
	}
}
