package(GameFrameworkMessage);

//=================================================
//node消息头
message(NodeMessageWithAccount)
{
  option(dontgenenum);
  member(m_Account, string, required);
};
message(NodeMessageWithGuid)
{
  option(dontgenenum);
  member(m_Guid, ulong, required);
};
message(NodeMessageWithAccountAndGuid)
{
  option(dontgenenum);
  member(m_Account, string, required);
  member(m_Guid, ulong, required);  
};
message(NodeMessageWithAccountAndLogicServerId)
{
  option(dontgenenum);
  member(m_Account, string, required);
  member(m_LogicServerId, int, required);
};
message(NodeMessageWithLogicServerId)
{
  option(dontgenenum);
  member(m_LogicServerId, int, required);
};
message(NodeRegister)
{
  member(m_Name, string, required);
};
message(NodeRegisterResult)
{
  member(m_IsOk, bool, required);
};

//=============================================
//仅由node处理的消息
message(VersionVerify){};
message(VersionVerifyResult)
{
	member(m_Result, int, required);
	member(m_EnableLog, int, required);
	member(m_ShopMask, uint, required);
};

//=================================================
message(DirectLogin){};
message(AccountLogout){};
message(Logout){};
message(GetQueueingCount){};
message(UserHeartbeat){};
message(KickUser){};
message(TooManyOperations){};
message(Msg_CL_StartGame){};
message(Msg_CL_FriendList){};
message(Msg_CL_RoleList){};
message(Msg_CL_CreateNickname){};
message(Msg_CL_BuyStamina){};
message(Msg_CL_BuyLife){};
message(Msg_CL_GetMailList){};
message(Msg_CL_QueryExpeditionInfo){};
message(Msg_CL_ExpeditionFailure){};
message(Msg_CL_MidasTouch){};
message(Msg_CL_RequestGroupInfo){};
message(Msg_CL_QuitPve){};
message(Msg_CL_RequestVigor){};
message(Msg_CL_WeeklyLoginReward){};
message(Msg_CL_GetQueueingCount){};
message(Msg_CL_RequestGowPrize){};
message(Msg_CL_GainFirstPayReward){};
message(Msg_CL_CorpsSignIn){};
message(Msg_CL_BuyPartnerCombatTicket){};
message(Msg_CL_RefreshPartnerCombat){};
message(Msg_CL_GetLootHistory){};
message(Msg_CL_GetMorrowReward){};
message(Msg_CL_GetLoginLottery){};
message(Msg_CL_QuerySkillInfos){};
message(Msg_CL_CorpsClearRequest){};
message(Msg_CL_ExpeditionSweep){};
message(Msg_CL_GMResetDailyMissions){};
message(Msg_LC_ResetCorpsSignIn){};
message(Msg_LC_SyncQuitRoom){};
message(Msg_LC_NoticeQuitGroup){};
message(Msg_LC_NotifyNewMail){};
message(Msg_LC_RequestItemUseResult){};
message(Msg_LC_ServerShutdown){};
message(Msg_LC_SyncResetGowPrize){};
message(Msg_LC_ResetConsumeGoodsCount){};
message(Msg_LC_NotifyMorrowReward){};
message(Msg_LC_ResetOnlineTimeRewardData){};
message(Msg_LC_ResetWeeklyLoginRewardData){};

//=================================================
message(Msg_LC_GmCode) {
  member(m_Content, string, required);
};

//=================================================

message(Msg_CL_AccountLogin)
{  
  member(m_OpCode, int, required);
  member(m_ChannelId, int, required);
  member(m_Data, string, required);
  member(m_ClientGameVersion, string, required);
  member(m_ClientLoginIp, string, required);
  member(m_GameChannelId, string, required);
  member(m_UniqueIdentifier, string, required);
  member(m_System, string, required);
};

message(Msg_LC_AccountLoginResult)
{
  member(m_AccountId, string, required);
  member(m_Oid, string, required);
  member(m_Token, string, required);
  member(m_Result, int, required);
};

message(Msg_LC_CreateNicnameResult)
{
  member(m_Nicknames, string, repeated);
};

message(Msg_CL_CreateRole)
{
  member(m_HeroId, int, required);
  member(m_Nickname, string, required);
};

message(Msg_LC_CreateRoleResult)
{
  member(m_Result, int, required);
  member(m_Nickname, string, required);
  member(m_HeroId, int, required);
  member(m_Level, int, required);
  member(m_UserGuid, ulong, required);
};

message(Msg_CL_ActivateAccount)
{
  member(m_ActivationCode, string, required);
};

message(Msg_LC_ActivateAccountResult)
{
  member(m_Result, int, required);
};

message(MailItemForMessage)
{
  member(m_ItemId, int, required);
  member(m_ItemNum, int, required);
};
message(MailInfoForMessage)
{
  member(m_AlreadyRead, bool, required);
  member(m_MailGuid, ulong, required);
  member(m_Module, int, required);
  member(m_Title, string, required);
  member(m_Sender, string, required);
  member(m_SendTime, string, required);
  member(m_Text, string, required);
  member(m_Items, MailItemForMessage, repeated);
  member(m_Money, int, required);
  member(m_Gold, int, required);
  member(m_Stamina, int, required);
};
message(Msg_LC_SyncMailList)
{
  member(m_Mails, MailInfoForMessage, repeated);
};

message(MissionInfoForSync)
{
  member(m_MissionId, int, required);
  member(m_IsCompleted, bool, required);
  member(m_Progress, string, required);
};
message(Msg_LC_ResetDailyMissions)
{
  member(m_Missions, MissionInfoForSync, repeated);
};

message(Msg_CL_SinglePVE)
{
  member(m_SceneType, int, required);
};

message(Msg_CL_SaveSkillPreset)
{
  member(m_SelectedPresetIndex, int, required);
};

message(Msg_LC_StartGameResult) {
  member(server_ip, string, optional);
  member(server_port, uint, optional);
  member(key, uint, optional);
  member(camp_id, int, optional);
  member(scene_type, int, optional);
  member(match_key, int, optional);
  member(result, int, required);
  member(prime, int, required);
};

message(Msg_CL_UpdateFightingScore) {
  member(score, int, required);
};

message(Msg_CL_RoleEnter) {
  member(m_Guid, ulong, required);
};

message(JsonItemDataMsg) {
  member(Guid, ulong, optional);
  member(ItemId, int, required);
  member(Level, int, optional);
  member(Experience, int, optional);
  member(Num, int, optional);
  member(AppendProperty, int, optional);
  member(EnhanceStarLevel, int, optional);
  member(StrengthLevel, int, optional);
  member(StrengthFailCount, int, optional);
  member(IsCanTrade, bool, optional);
};

message(SkillDataInfo) {
  member(ID, int, optional);
  member(Level, int, optional);
  member(Postions, int, optional);
};

message(LegacyDataMsg) {
  member(ItemId, int, optional);
  member(Level, int, optional);
  member(AppendProperty, int, optional);
  member(IsUnlock, bool, optional);
};

message(XSoulDataMsg) {
  member(ItemId, int, required);
  member(Level, int, required);
  member(ModelLevel, int, required);
  member(Experience, int, required);
};

message(JsonTalentDataMsg) {
  member(Slot, int, required);
  member(ItemGuid, ulong, required);
  member(ItemId, int, required);
  member(Level, int, required);
  member(Experience, int, required);
};

message(PaymentRebateDataMsg) {
  member(Id, int, required);
  member(Group, int, required);
  member(Describe, string, required);
  member(AnnounceTime, string, required);
  member(StartTime, string, required);
  member(EndTime, string, required);
  member(TotalDiamond, int, required); 
  member(Diamond, int, required);
  member(Gold, int, required);
  member(Exp, int, required);
  message(AwardItemInfo) {
    member(m_Id, int, required);
    member(m_Num, int, required);
  };
  member(AwardItems, AwardItemInfo, repeated);
  member(DiamondAlready, int, required);
};

message(GowDataMsg) {
  member(GowElo, int, optional);
  member(GowMatches, int, optional);
  member(GowWinMatches, int, optional);
  member(LeftMatchCount, int, optional);
  member(RankId, int, optional);
  member(Point, int, optional);
  member(CriticalTotalMatches, int, optional);
	member(CriticalAmassWinMatches, int, optional);
	member(CriticalAmassLossMatches, int, optional);
	member(IsAcquirePrize, bool, optional);
};

message(FashionMsg) {
  member(m_FsnId, int, required);
  member(m_UseForever, bool, optional);
  member(m_DeadlineSeconds, int, optional);
  member(m_DressOn, bool, optional);
};
message(FashionHideMsg) {
  member(m_IsWingHide, bool, required);
  member(m_IsWeaponHide, bool, required);
  member(m_IsClothHide, bool, required);
};
message(FashionSynMsg) {
  member(FashionId, int, required);
  member(IsHide, bool, required);
};

message(Msg_LC_RoleEnterResult) {
  member(m_Result, int, required);
	member(m_Money, int, optional);
  member(m_Gold, int, optional);
  member(m_GoldCash, int, optional);
	member(m_Stamina, int, optional);
	member(m_Exp, int, optional);
	member(m_Level, int, optional);
	member(m_CitySceneId, int, optional);
	member(m_BuyStaminaCount, int, optional);
	member(m_BuyMoneyCount, int, optional);
	member(m_CurSellItemGoldIncome, int, optional);
	member(m_Vip, int, optional);
	member(m_NewbieGuideScene, int, optional);
	member(m_Gow, GowDataMsg, optional);
	member(m_NewbieGuides, int, repeated);
	member(m_BagItems, JsonItemDataMsg, repeated);
	member(m_Equipments, JsonItemDataMsg, repeated);
	member(m_SkillInfo, SkillDataInfo, repeated);
	message(MissionInfoForSync) {
    member(m_MissionId, int, optional);
    member(m_IsCompleted, bool, optional);
    member(m_Progress, string, optional);
  };
	member(m_Missions, MissionInfoForSync, repeated);
	member(m_Legacys, LegacyDataMsg, repeated);
	message(SceneDataMsg) {
    member(m_SceneId, int, optional);
    member(m_Grade, int, optional);
  };
	member(m_SceneData, SceneDataMsg, repeated);
  message(ScenesCompletedCountDataMsg) {
  	member(m_SceneId, int, optional);
  	member(m_Count, int, optional);
  };
  member(m_SceneCompletedCountData, ScenesCompletedCountDataMsg, repeated);
  member(m_Friends, FriendInfoForMsg, repeated);
  member(m_Partners, PartnerDataMsg, repeated);
  member(m_ActivePartners, int, repeated);
  member(m_XSouls, XSoulDataMsg, repeated);
  message(ExchangeGoodsMsg) {
    member(m_Id, int, required);
    member(m_Num, int, required);
  };
  member(m_Exchanges, ExchangeGoodsMsg, repeated);
  member(m_WorldId, int, optional);
  member(m_Vigor, int, required);
  member(m_SignInCountCurMonth, int, required);
  member(m_RestSignInCountCurDay, int, required);
  member(m_NewbieFlag, long, repeated);
  message(ExchangeRefreshMsg) {
    member(m_CurrencyId, int, required);
    member(m_Num, int, required);
  };
  member(m_RefreshExchangeNum, ExchangeRefreshMsg, repeated);
  member(m_NewbieActionFlag, long, repeated);
  member(m_IsGetWeeklyReward, bool, required);
  member(m_WeeklyRewardRecord, int, repeated);
  member(m_OnlineTimeRewardedIndex, int, repeated);
  member(m_OnlineDuration, int, required);
  member(m_GuideFlag, long, required);
  member(m_EquipTalents, JsonTalentDataMsg, repeated);
  member(m_CorpsId, ulong, required);
  member(m_PaymentRebates, PaymentRebateDataMsg, repeated);
  member(m_MpveInfo, Msg_LC_SyncMpveInfo, repeated);
  message(GoodsPurchasedMsg) {
    member(m_GoodsId, string, required);
    member(m_GoodsCount, int, required);
  };
  member(m_GoodsInfo, GoodsPurchasedMsg, repeated);
  member(m_PurchasedDiamonds, int, required);
  member(m_RewardedVipLevel, int, repeated);
  member(m_IsRewardedFirstBuy, bool, required);
  member(m_FashionInfo, FashionMsg, repeated);
  message(LotteryInfo)
  {
    member(m_Id, int, required);
    member(m_CurFreeCount, int, required);
    member(m_LastDrawTime, string, required);
  };
  member(m_Lottery, LotteryInfo, repeated);
  member(m_Vitality, int, required);
  member(m_CorpsSignInState, bool, required);
  member(m_PartnerCombatInfo, Msg_LC_PartnerCombatInfo, required);
  member(m_CorpsInfo, Msg_LC_SyncCorpsInfo, optional);
  member(m_SecretAreaFought, int, required);
  member(m_SecretAreaSegments, int, repeated);
  member(m_SecretAreaFightNum, int, repeated);
  member(m_SecretAreaHp, int, repeated);
  member(m_SecretAreaMp, int, repeated);
  member(m_MorrowRewardInfo, MorrowRewardInfo, required);
  member(m_FashionHide, FashionHideMsg, required);
  member(m_RecentLoginState, uint, required);
  member(m_SumLoginDayCount, int, required);
  member(m_UsedLoginDrawLotteryCount, int, required);
  member(m_UserAccountId, string, required);
  message(ResetEliteCountInfo) {
    member(SceneId, int, required);
    member(ResetCount, int, required);
  };
  member(m_ResetEliteCount, ResetEliteCountInfo, repeated);
  member(m_ExtraDiamondBox, bool, repeated);
  member(m_GrowthFundValue, int, required);
  message(DareData) {
    member(CharpterId, int, required);
  	member(CurDareCount, int, required);
  };
	member(m_CorpsDareData, DareData, repeated);	
  member(m_ServerStartTime, string, required);
  message(ChapterAwardMsg) {
    member(ChapterId, int, required);
    member(AwardValue, int, required);
  };
  member(m_ChapterAwardData, ChapterAwardMsg, repeated);
  member(m_SecretAreaPartnerHp, int, repeated);
};

message(MorrowRewardInfo) {
    member(m_ActiveId, int, required);
    member(m_IsActive, bool, required);
    member(m_LeftSeconds, int, required);
    member(m_CanGetReward, bool, required);
};

message(Msg_CL_ExpeditionReset) {
  member(m_Hp, int, required);
  member(m_Mp, int, required);
  member(m_Rage, int, required);
  member(m_RequestNum, int, required);
  member(m_IsReset, bool, required);
  member(m_AllowCostGold, bool, required);
  member(m_Timestamp, long, required);
};
message(Msg_LC_ExpeditionResetResult) {
  member(m_Hp, int, optional);
  member(m_Mp, int, optional);
  member(m_Rage, int, optional);
  member(m_Schedule, int, required);
  member(m_CurResetCount, int, required);
  message(ImageDataMsg) {
  	member(Guid, ulong, optional);
    member(HeroId, int, optional);
    member(Nickname, string, optional);
    member(Level, int, optional);
    member(FightingScore, int, optional);
    member(EquipInfo, JsonItemDataMsg, repeated);
    member(SkillInfo, SkillDataInfo, repeated);
    member(LegacyInfo, LegacyDataMsg, repeated);
    member(Partners, PartnerDataMsg, repeated);
    member(XSouls, XSoulDataMsg, repeated);
    member(Talents, JsonTalentDataMsg, repeated);
    member(Fashions, FashionSynMsg, repeated);
  };
  message(TollgateDataForMsg) {
    member(Type, int, optional);
    member(IsFinish, bool, optional);
    member(IsAcceptedAward, bool, repeated);
    member(EnemyArray, int, repeated);
    member(EnemyAttrArray, int, repeated);
    member(UserImageArray, ImageDataMsg, repeated);
  };
  member(Tollgates, TollgateDataForMsg, optional);
  member(m_AllowCostGold, bool, optional);
  member(m_IsUnlock, bool, optional);
  member(m_Result, int, optional);
  message(ExpeditionPartner) {
	  member(Id, int, required);
	  member(Hp, int, required);
  };
  member(Partners, ExpeditionPartner, repeated);
  member(LastAchievedSchedule, int, optional);
};

message(Msg_LC_RoleListResult) {
  member(m_Result, int, required);
  member(m_UserInfoCount, int, required);
  message(UserInfoForMessage) {
		member(m_Nickname, string, required);
		member(m_HeroId, int, required);
		member(m_Level, int, required);
		member(m_UserGuid, ulong, required);
	};
  member(m_UserInfos, UserInfoForMessage, repeated);
};

message(Msg_CL_RequestExpedition) {
	member(m_SceneId, int, required);
	member(m_TollgateNum, int, required);
};

message(Msg_LC_RequestExpeditionResult) {
	member(m_Guid, ulong, required);
  member(m_ServerIp, string, required);
  member(m_ServerPort, uint, required);
  member(m_Key, uint, required);
  member(m_CampId, int, required);
  member(m_SceneType, int, required);
  member(m_ActiveTollgate, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_FinishExpedition) {
	member(m_SceneId, int, required);
	member(m_TollgateNum, int, required);
	member(m_Hp, int, required);
	member(m_Mp, int, required);
	member(m_Rage, int, required);
	member(m_PartnerId, int, required);
	member(m_PartnerHpPer, int, required);
};

message(Msg_LC_FinishExpeditionResult) {
  member(m_SceneId, int, required);
  member(m_TollgateNum, int, required);
  member(m_Hp, int, required);
  member(m_Mp, int, required);
  member(m_Rage, int, required);
  member(m_Result, int, required);
};

message(Msg_LC_ExpeditionSweepResult) {
  member(m_TollgateNum, int, required);
  member(m_Hp, int, required);
  member(m_Mp, int, required);
  member(m_Rage, int, required);
  member(m_Diamond, int, required);
  member(m_Result, int, required);
  member(m_GoldCash, int, required);
};

message(Msg_CL_DiscardItem) {
  member(m_ItemGuid, ulong, repeated);
};

message(Msg_LC_DiscardItemResult) {
	member(m_ItemGuids, ulong, repeated);
  member(m_TotalIncome, int, required);
  member(m_Gold, int, required);
  member(m_Money, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_SellItem) {
  member(ItemGuid, ulong, required);
  member(ItemNum, int, required);
};

message(Msg_LC_SellItemResult) {
  member(ItemGuid, ulong, required);
  member(ItemNum, int, required);
  member(Result, int, required);
  member(Diamand, int, optional);
  member(Money, int, optional);
  member(TotalIncome, int, optional);
  member(GoldCash, int, optional);
};

message(Msg_CL_MountEquipment) {
  member(m_EquipGuid, ulong, required);
  member(m_EquipPos, int, required);
};

message(Msg_LC_MountEquipmentResult) {
	member(m_ItemGuid, ulong, required);
  member(m_EquipPos, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_MountSkill) {
	member(m_PresetIndex, int, required);
	member(m_SkillID, int, required);
	member(m_SlotPos, int, required);
};

message(Msg_LC_MountSkillResult) {
	member(m_PresetIndex, int, required);
  member(m_SkillID, int, required);
  member(m_SlotPos, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_UnmountSkill) {
	member(m_PresetIndex, int, required);
	member(m_SlotPos, int, required);
};

message(Msg_LC_UnmountSkillResult) {
	member(m_PresetIndex, int, required);
  member(m_SlotPos, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_UpgradeSkill) {
	member(m_PresetIndex, int, required);
	member(m_SkillID, int, required);
	member(m_AllowCostGold, bool, required);
};

message(Msg_LC_UpgradeSkillResult) {
	member(m_PresetIndex, int, required);
  member(m_SkillID, int, required);
  member(m_AllowCostGold, bool, required);
  member(m_Money, int, required);
  member(m_Gold, int, required);
  member(m_Vigor, int, required);
  member(m_Result, int, required);
  member(m_GoldCash, int, required);
};

message(Msg_CL_UnlockSkill) {
	member(m_PresetIndex, int, required);
	member(m_SkillID, int, required);
};

message(Msg_LC_UnlockSkillResult) {
	member(m_PresetIndex, int, required);
  member(m_SkillID, int, required);
  member(m_UserLevel, int, required);
  member(m_SlotPos, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_LiftSkill) {
	member(m_SkillId, int, required);
};

message(Msg_LC_LiftSkillResult) {
  member(m_SkillID, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_UpgradeItem) {
	member(m_Position, int, required);
  member(m_ItemId, int, required);
  member(m_AllowCostGold, bool, required);
};

message(Msg_LC_UpgradeItemResult) {
  member(m_Position, int, required);
  member(m_Money, int, required);
  member(m_Gold, int, required);
  member(m_Result, int, required);
  member(m_GoldCash, int, required);
};

message(Msg_CL_SwapSkill) {
	member(m_PresetIndex, int, required);
  member(m_SkillID, int, required);
  member(m_SourcePos, int, required);
  member(m_TargetPos, int, required);
};

message(Msg_LC_SwapSkillResult) {
  member(m_PresetIndex, int, required);
  member(m_SkillID, int, required);
  member(m_SourcePos, int, required);
  member(m_TargetPos, int, required);
  member(m_Result, int, required);
};

message(Msg_LC_BuyStaminaResult) {
  member(m_Result, int, required);
  member(m_Gold, int, required);
  member(m_GoldCash, int, required);
};

message(Msg_CL_FinishMission) {
  member(m_MissionId, int, required);
};

message(Msg_LC_FinishMissionResult) {
  member(m_ResultCode, int, required);
  member(m_FinishMissionId, int, optional);
  member(m_Gold, int, optional);
  member(m_Exp, int, optional);
  member(m_Diamond, int, optional);
  message(MissionInfoForSync) {
	member(m_MissionId, int, required);
    member(m_IsCompleted, bool, required);
    member(m_Progress, string, required);
  };
  member(m_UnlockMissions, MissionInfoForSync, repeated);
};

message(Msg_LC_BuyLifeResult) {
	member(m_Succeed, bool, required);
    member(m_CurDiamond, int, required);
    member(m_CostReliveStone, int, required);
    member(m_GoldCash, int, required);
};

message(Msg_LC_UnlockLegacyResult) {
	member(m_Index, int, required);
	member(m_ItemID, int, required);
	member(m_Result, int, required);
};

message(Msg_CL_UpgradeLegacy) {
	member(m_Index, int, required);
	member(m_ItemID, int, required);
	member(m_AllowCostGold, bool, required);
};

message(Msg_LC_UpgradeLegacyResult) {
	member(m_Index, int, required);
	member(m_ItemID, int, required);
	member(m_Result, int, required);
};

message(Msg_CL_AddXSoulExperience) {
  member(m_XSoulPart, int, required);
  member(m_UseItemId, ulong, required);
  member(m_ItemNum, int, required);
};

message(Msg_LC_AddXSoulExperienceResult) {
  member(m_XSoulPart, int, required);
  member(m_UseItemId, ulong, required);
  member(m_ItemNum, int, required);
  member(m_Result, int, required);
  member(m_Experience, int, required);
};

message(Msg_CL_XSoulChangeShowModel) {
  member(m_XSoulPart, int, required);
  member(m_ModelLevel, int, required);
};

message(Msg_LC_XSoulChangeShowModelResult) {
  member(m_XSoulPart, int, required);
  member(m_ModelLevel, int, required);
  member(m_Result, int, required);
};

message(Msg_CL_ReadMail) {
  member(m_MailGuid, ulong, required);
};
message(Msg_CL_ReceiveMail) {
	member(m_MailGuid, ulong, required);
};

message(Msg_CL_ExpeditionAward) {
	member(m_TollgateNum, int, required);
};

message(Msg_LC_ExpeditionAwardResult) {
	member(m_TollgateNum, int, required);
	message(AwardItemInfo) {
		member(m_Id, int, required);
    member(m_Num, int, required);
	};
	member(m_Items, AwardItemInfo, repeated);
	member(m_Result, int, required);
};

message(Msg_LC_MidasTouchResult) {
	member(m_Count, int, required);
	member(m_Money, int, required);
	member(m_Gold, int, required);
	member(m_GoldCash, int, required);
	member(m_Result, int, required);
};

message(Msg_CL_UnmountEquipment) {
	member(m_EquipPos, int, required);
};

message(Msg_LC_UnmountEquipmentResult) {
	member(m_EquipPos, int, required);
	member(m_Result, int, required);
};

message(Msg_LC_UserLevelup) {
	member(m_UserId, int, required);
	member(m_UserLevel, int, required);
};

message(Msg_LC_SyncStamina) {
	member(m_Stamina, int, required);
};

message(Msg_CL_StageClear) {
	member(m_HitCount, int, optional);
	member(m_KillNpcCount, int, optional);
	member(m_MaxMultHitCount, int, optional);
	member(m_Hp, int, optional);
	member(m_Mp, int, optional);
	member(m_Gold, int, optional);
    member(m_MatchKey, int, optional);
    member(m_IsFitTime, bool, optional);
};

message(Msg_LC_StageClearResult) {
  member(m_SceneId, int, optional);
  member(m_HitCount, int, optional);
  member(m_MaxMultHitCount, int, optional);
  member(m_Duration, long, optional);
  member(m_ItemId, int, optional);
  member(m_ItemCount, int, optional);
  member(m_ExpPoint, int, optional);
  member(m_Hp, int, optional);
  member(m_Mp, int, optional);
  member(m_Gold, int, optional);
  member(m_DeadCount, int, optional);
  member(m_CompletedRewardId, int, optional);
  member(m_SceneStarNum, int, optional);
  message(MissionInfoForSync) {
    member(m_MissionId, int, required);
    member(m_IsCompleted, bool, required);
    member(m_Progress, string, required);
	};
  member(m_Missions, MissionInfoForSync, repeated);
  member(m_KillNpcCount, int, optional);
  member(m_ResultCode, int, required);
  message(Teammate) {
    member(m_Nick, string, required);
    member(m_ResId, int, required);
    member(m_Money, int, required);
    member(m_TotalDamage, float, required);
    member(m_ReliveTime, int, required);
    member(m_HitCount, int, required);
    member(m_Level, int, required);
    member(m_HitCountFit, bool, required);
    member(m_TimeFit, bool, required);
	};
  member(m_Teammate, Teammate, repeated);
  member(m_Level, int, optional);
  member(m_BattleResult, int, optional);
  member(m_StageScore, int, optional);
  member(m_RewardItemIdList, int, repeated);
  member(m_RewardItemNumList, int, repeated);
};

message(Msg_CL_AddAssets) {
	member(m_Money, int, required);
	member(m_Gold, int, required);
	member(m_Exp, int, required);
	member(m_Stamina, int, required);
};

message(Msg_LC_AddAssetsResult) {
	member(m_Money, int, required);
	member(m_Gold, int, required);
	member(m_Exp, int, required);
	member(m_Stamina, int, required);
	member(m_Result, int, required);
};

message(Msg_CL_AddItem) {
  member(m_ItemId, int, required);
  member(m_ItemNum, int, required);
};

message(Msg_LC_SyncGowStarList) {
	message(GowStarInfoForMessage) {
    member(m_Guid, ulong, required);
    member(m_GowElo, int, required);
    member(m_Nick, string, required);
    member(m_HeroId, int, required);
    member(m_Level, int, required);
    member(m_FightingScore, int, required);
    member(m_RankId, int, required);
    member(m_Point, int, required);
    member(m_CriticalTotalMatches, int, required);
    member(m_CriticalAmassWinMatches, int, required);
    member(m_CriticalAmassLossMatches, int, required);
	};
	member(m_Stars, GowStarInfoForMessage, repeated);
};

message(Msg_CL_RequestGowBattleResult) {
};

message(Msg_LC_SyncGowBattleResult) {
	member(m_Result, int, required);
  member(m_OldGowElo, int, required);
  member(m_GowElo, int, required);
  member(m_OldPoint, int, required);
  member(m_Point, int, required);
  member(m_OldRankId, int, required);
  member(m_RankId, int, required);
  member(m_MaxMultiHitCount, int, required);
  member(m_TotalDamage, int, required);
  member(m_EnemyNick, string, required);
  member(m_EnemyHeroId, int, required);
  member(m_EnemyOldGowElo, int, required);
  member(m_EnemyGowElo, int, required);
  member(m_EnemyOldPoint, int, required);
  member(m_EnemyPoint, int, required);
  member(m_EnemyMaxMultiHitCount, int, required);
  member(m_EnemyTotalDamage, int, required);
  member(m_SceneType, int, required);
};

message(Msg_CL_AddFriend) {
	member(m_TargetNick, string, optional);
	member(m_TargetGuid, ulong, optional);
};

message(FriendInfoForMsg) {
	member(Guid, ulong, required);
  member(Nickname, string, required);
  member(HeroId, int, required);
  member(Level, int, required);
  member(FightingScore, int, required);
  member(IsOnline, bool, required);
  member(IsBlack, bool, required);
};

message(Msg_LC_AddFriendResult) {
	member(m_TargetNick, string, optional);
	member(m_FriendInfo, FriendInfoForMsg, optional);
	member(m_Result, int, required);
};

message(Msg_LC_SyncFriendList) {
	member(m_FriendInfo, FriendInfoForMsg, repeated);
};

message(Msg_CL_DeleteFriend) {
	member(m_TargetGuid, ulong, required);
};

message(Msg_LC_DelFriendResult) {
	member(m_TargetGuid, ulong, required);
	member(m_Result, int, required);
};

message(Msg_CL_ConfirmFriend) {
	member(m_TargetGuid, ulong, required);
};

message(Msg_CL_QueryFriendInfo) {
	member(m_QueryType, int, required);
  member(m_TargetName, string, optional);
  member(m_TargetLevel, int, optional);
  member(m_TargetScore, int, optional);
  member(m_TargetFortune, int, optional);
  member(m_TargetGender, int, optional);
};

message(Msg_LC_QueryFriendInfoResult) {
	member(m_Friends, FriendInfoForMsg, repeated);
};

message(Msg_LC_MissionCompleted) {
    member(m_MissionId, int, required);
    member(m_Progress, string, required);
};

message(Msg_LC_SyncNoticeContent) {
	member(m_Content, string, required);
	member(m_RollNum, int, required);
};

message(Msg_LC_FriendOnline) {
	member(m_Guid, ulong, required);
};

message(Msg_LC_FriendOffline) {
	member(m_Guid, ulong, required);
};

message(Msg_LC_SyncGroupUsers) {
	message(UserInfoForGroup) {
    member(m_Guid, ulong, required);
    member(m_HeroId, int, required);
    member(m_Nick, string, required);
    member(m_Level, int, required);
    member(m_FightingScore, int, required);
    member(m_Status, int, required);
	};
	member(m_Creator, ulong, required);
	member(m_Count, int, required);
	member(m_Members, UserInfoForGroup, repeated);
	member(m_Confirms, UserInfoForGroup, repeated);
};

message(Msg_LC_RequestJoinGroupResult) {
	member(m_Result, int, required);
	member(m_Nick, string, optional);
};

message(Msg_LC_ConfirmJoinGroupResult) {
	member(m_Result, int, required);
	member(m_Nick, string, optional);
};

message(Msg_CL_PinviteTeam) {
	member(m_FirstNick, string, optional);
	member(m_SecondNick, string, optional);
	member(m_FirstGuid, ulong, optional);
	member(m_SecondGuid, ulong, optional);
};

message(Msg_CL_RequestJoinGroup) {
	member(m_InviteeGuid, ulong, required);
	member(m_GroupNick, string, required);
};

message(Msg_CL_ConfirmJoinGroup) {
	member(m_InviteeGuid, ulong, required);
	member(m_GroupNick, string, required);
};

message(Msg_CL_QuitGroup) {
	member(m_DropoutNick, string, required);
};

message(Msg_LC_SyncPinviteTeam) {
	member(m_LeaderNick, string, required);
	member(m_Sponsor, string, required);
};

message(Msg_LC_AddItemResult) {
  member(m_ItemGuid, ulong, required);
	member(m_ItemId, int, required);
	member(m_RandomProperty, int, required);
	member(m_Result, int, required);
	member(m_ItemCount, int, required);
	member(m_Exp, int, optional);
	member(m_IsCanTrade, bool, optional);
};

message(Msg_LC_AddItemsResult) {
	message(ItemInstance) {
	  member(m_ItemGuid, ulong, required);
		member(m_ItemId, int, required);
		member(m_RandomProperty, int, required);
		member(m_ItemCount, int, required);
		member(m_Exp, int, required);
		member(m_IsCanTrade, bool, required);
	};
	member(Items, ItemInstance, repeated);
	member(m_Result, int, required);
};

message(Msg_CL_GetGowStarList) {
	member(m_Start, int, required);
	member(m_Count, int, required);
};

message(Msg_LC_SyncLeaveGroup) {
	member(m_Result, int, required);
	member(m_GroupNick, string, optional);
	member(m_NeedTip, bool, optional);
};

message(Msg_CL_RefuseGroupRequest) {
	member(m_RequesterGuid, ulong, required);
};

message(Msg_CL_SelectPartner) {
    member(m_PartnerId, int, required);
};

message(Msg_CL_CancelSelectPartner) {
    member(m_PartnerId, int, required);
};

message(Msg_LC_CancelSelectPartnerResult) {
    member(m_ResultCode, int, required);
    member(m_Partners, int, repeated);
};

message(Msg_LC_SelectPartnerResult) {
    member(m_ResultCode, int, required);
    member(m_Partners, int, repeated);
};

message(Msg_CL_UpgradePartnerLevel) {
    member(m_PartnerId, int, required);
};

message(Msg_LC_UpgradePartnerLevelResult) {
    member(m_ResultCode, int, required);
    member(m_PartnerId, int, required);
    member(m_CurLevel, int, required);
    member(m_PartnerEquipState, bool, repeated);
};

message(Msg_CL_UpgradePartnerStage) {
    member(m_PartnerId, int, required);
};

message(Msg_LC_UpgradeParnerStageResult) {
    member(m_ResultCode, int, required);
    member(m_PartnerId, int, required);
    member(m_CurStage, int, required);
};

message(Msg_CL_PartnerEquip) {
    member(m_PartnerId, int, required);
    member(m_ItemGuid, ulong, required);
    member(m_EquipIndex, int, required);
};

message(Msg_LC_PartnerEquipResult) {
    member(m_ResultCode, int, required);
    member(m_PartnerId, int, required);
    member(m_ItemGuid, ulong, required);
    member(m_PartnerEquipState, bool, repeated);
};

message(Msg_LC_GetPartner) {
    member(m_PartnerId, int, required);
};

message(Msg_LC_ChangeCaptain) {
	member(m_CreatorGuid, ulong, optional);
};

message(Msg_CL_RequestMatch) {
	member(m_SceneType, int, optional);
	member(m_SceneDifficulty, int, optional);
};

message(Msg_CL_CancelMatch) {
	member(m_SceneType, int, required);
};

message(Msg_LC_MatchResult) {
	member(m_Result, int, required);
	member(m_UserLevel, int, repeated);
	member(m_UserName, string, repeated);
	member(m_UserHeroId, int, repeated);
	member(m_LogicServerId, int, repeated);
};

message(Msg_LC_MpveGeneralResult) {
	member(m_Result, int, required);
	member(m_Nick, string, optional);
	member(m_Type, int, optional);
	member(m_Difficulty, int, optional);
};

message(Msg_CL_StartMpve) {
	member(m_SceneType, int, required);
	member(m_SceneDifficulty, int, required);
};

message(Msg_LC_MpveAwardResult) {
	member(m_SceneType, int, required);
	member(m_Result, int, required);
	member(m_AwardIndex, int, required);
	member(m_AddMoney, int, required);
	member(m_AddGold, int, required);
	message(AwardItemInfo) {
		member(m_Id, int, required);
    member(m_Num, int, required);
	};
	member(m_Items, AwardItemInfo, repeated);
	member(m_DareCount, int, required);
	member(m_AwardCount, int, required);
};

message(Msg_CL_UpdatePosition) {
  member(m_X, float, required);
  member(m_Z, float, required);
  member(m_FaceDir, float, required);
};

message(Msg_CL_RequestUsers) {
  member(m_Count, int, required);
  member(m_AlreadyExists, ulong, repeated);
};

message(Msg_LC_RequestUsersResult) {
  message(UserInfo) {
    member(m_Guid, ulong, required);
    member(m_HeroId, int, required);
    member(m_Nick, string, required);
    member(m_X, float, required);
    member(m_Z, float, required);
    member(m_FaceDir, float, required);
    member(m_XSoulItemId, int, required);
    member(m_XSoulLevel, int, optional);
    member(m_XSoulExp, int, optional);
    member(m_XSoulShowLevel, int, optional);
    member(m_WingItemId, int, required);
    member(m_WingLevel, int, optional);
  	member(m_DressedFashionClothId, int, optional);
  	member(m_DressedFashionWingId, int, optional);
  	member(m_DressedFashionWeaponId, int, optional);
  };
  member(m_Users, UserInfo, repeated);
};

message(Msg_CL_RequestUserPosition) {
  member(m_User, ulong, required);
};

message(Msg_LC_RequestUserPositionResult) {
  member(m_User, ulong, required);
  member(m_X, float, required);
  member(m_Z, float, required);
  member(m_FaceDir, float, required);
};

message(Msg_CL_ChangeScene) {
  member(m_SceneId, int, required);
};

message(Msg_CL_ChangeCityRoom)
{
	member(m_SceneId, int, required);
	member(m_RoomId, int, required);
};

message(Msg_CL_CompoundPartner) {
    member(m_PartnerId, int, required);
};

message(Msg_LC_CompoundPartnerResult) {
    member(m_ResultCode, int, required);
    member(m_PartnerId, int, required);
};
message(Msg_CL_SweepStage) {
    member(m_SceneId, int, required);
    member(m_SweepTime, int, required);
};
message(Msg_LC_SweepStageResult) {
    member(m_SceneId, int, required);
    member(m_ResultCode, int, required);
    member(m_ItemInfo, JsonItemDataMsg, repeated);
    member(m_Exp, int, optional);
    member(m_Gold, int, optional);
    member(m_SweepItemCost, int, optional);
};

message(Msg_CL_RequestPlayerInfo) {
  member(m_Nick, string, required);
};

message(Msg_LC_SyncPlayerInfo) {
  member(m_Nick, string, required);
  member(m_Level, int, required);
  member(m_Score, int, required);
  member(m_CorpsId, ulong, required);
};

message(Msg_CL_UpdateActivityUnlock) {
  member(m_SceneId, int, required);
};

message(Msg_CL_RequestPlayerDetail) {
  member(m_Nick, string, required);
};

message(Msg_LC_SyncPlayerDetail) {
  member(m_Nick, string, required);
  member(m_Level, int, required);
  member(m_Score, int, required);
  member(m_CorpsId, int, required);
  member(m_VipLv, int, required);
  member(EquipInfo, JsonItemDataMsg, repeated);
  member(Partners, PartnerDataMsg, repeated);
  member(Talents, JsonTalentDataMsg, repeated);
  member(m_HeroId, int, required);
  member(FashonInfo, FashionMsg, repeated);
  member(FashionHide, FashionHideMsg, required);
  member(m_PartnerScore, int, required);
};

message(Msg_LC_NoticePlayerOffline) {
};

message(Msg_CL_ExchangeGoods) {
    member(m_ExchangeId, int, required);
	member(m_NpcId, int, required);
	member(m_RequestRefresh, bool, required);
};

message(Msg_LC_ExchangeGoodsResult) {
  member(m_ExchangeId, int, required);
	member(m_NpcId, int, required);
	member(m_ExchangeNum, int, required);
	member(m_Result, int, required);
	member(m_Refresh, bool, required);
	member(m_Money, int, required);
	member(m_Gold, int, required);
	member(m_GoldCash, int, required);
};

message(Msg_CL_SecretAreaTrial) {
    member(m_Difficulty, int, required);
	member(m_Sweept, bool, required);
	member(m_Refresh, bool, required);
};

message(Msg_LC_SecretAreaTrialResult) {
    member(m_Difficulty, int, required);
	member(m_Result, int, required);
	member(m_AlreadyFightNum, int, required);
	member(m_Segments, int, required);
	member(m_Hp, int, required);
	member(m_Mp, int, required); 
	member(m_AlreadyFought, int, required);
	member(m_Sweept, bool, required);
	member(m_Refresh, bool, required);
	member(m_PartnerHp, int, required);
	member(m_Prime, int, required);
	member(m_CheckNumbers, int, repeated);
};

message(Msg_CL_SecretAreaFightingInfo) {
    member(m_Difficulty, int, required);
	member(m_Segment, int, required);
	member(m_Hp, int, required);
	member(m_Mp, int, required);
	member(m_Finish, bool, required);
	member(m_PartnerHp, int, required);
	member(m_CheckNumber, int, required);
};

message(Msg_LC_SecretAreaTrialAward) {
    member(m_AwardId, int, required);
	member(m_Finish, bool, required);
};

message(Msg_CL_QuitRoom) {
	member(m_IsQuitRoom, bool, required);
};

message(Msg_LC_SyncVigor) {
	member(m_Vigor, int, required);
};

message(Msg_CL_SignInAndGetReward) {
    member(m_Guid, ulong, required);
};

message(Msg_LC_SignInAndGetRewardResult) {
    member(m_ResultCode, int, required);
    member(m_RewardId, int, optional);
};

message(Msg_LC_SyncSignInCount) {
    member(m_SignInCountCurMonth, int, required);
    member(m_RestSignInCountCurDay, int, required);
};

message(Msg_LC_SyncMpveInfo) {
	member(m_Type, int, required);
  member(m_Difficulty, int, repeated);
	member(m_DareCount, int, required);
	member(m_AwardCount, int, required);
	member(m_AwardIndex, int, repeated);
	member(m_IsGet, bool, repeated);
};

message(Msg_CL_SetNewbieFlag) {
  member(m_Bit, int, required);
  member(m_Num, int, required);
};

message(Msg_LC_SyncNewbieFlag) {
  member(m_NewbieFlag, long, repeated);
};

message(PartnerDataMsg) {
  member(Id, int, required);
  member(AdditionLevel, int, required);
  member(SkillStage, int, required);
  member(EquipState, bool, repeated);
};

message(ArenaInfoMsg) {
  member(Rank, int, required);
  member(Guid, ulong, required);
  member(HeroId, int, required);
  member(NickName, string, required);
  member(Level, int, required);
  member(FightScore, int, required);
  member(ActivePartners, PartnerDataMsg, repeated);
  member(EquipInfo, JsonItemDataMsg, repeated);
  member(ActiveSkills, SkillDataInfo, repeated);
  member(FightParters, PartnerDataMsg, repeated);
  member(LegacyAttr, LegacyDataMsg, repeated);
  member(XSouls, XSoulDataMsg, repeated);
  member(Talents, JsonTalentDataMsg, repeated);
  member(Fashions, FashionSynMsg, repeated);
};

message(Msg_CL_QueryArenaInfo) {
};

message(Msg_LC_ArenaInfoResult) {
  member(m_ArenaInfo, ArenaInfoMsg, required);
  member(m_LeftBattleCount, int, required);
  member(m_CurFightCountByTime, int, required);
  member(m_BattleLeftCDTime, long, required);
};

message(Msg_CL_QueryArenaMatchGroup) {
};

message(Msg_LC_ArenaMatchGroupResult) {
  message(MatchGroupData) {
    member(One, ArenaInfoMsg, required);
    member(Two, ArenaInfoMsg, required);
    member(Three, ArenaInfoMsg, required);
  };
  member(m_MatchGroups, MatchGroupData, repeated);
};

message(Msg_LC_SyncGoldTollgateInfo) {
	member(m_GoldCurAcceptedCount, int, required);
};

message(Msg_CL_RequestRefreshExchange) {
	member(m_RequestRefresh, bool, required);
	member(m_CurrencyId, int, required);
	member(m_NpcId, int, required);
};

message(Msg_LC_RefreshExchangeResult) {
	member(m_RequestRefreshResult, int, required);
	member(m_RefreshNum, int, required);
	member(m_CurrencyId, int, required);
	member(m_NpcId, int, required);
	member(m_Gold, int, required);
	member(m_GoldCash, int, required);
};

message(Msg_CL_DiamondExtraBuyBox) {
	member(m_BoxPlace, int, required);
	member(m_SceneId, int, required);
};

message(Msg_LC_DiamondExtraBuyBoxResult) {
	member(m_BoxPlace, int, required);
	member(m_Result, int, required);
	member(m_Fresh, bool, required);
	member(m_AddMoney, int, required);
	member(m_AddGold, int, required);
	message(AwardItemInfo) {
		member(m_Id, int, required);
		member(m_Num, int, required);
	};
	member(m_Items, AwardItemInfo, repeated);
	member(m_Gold, int, required);
	member(m_GoldCash, int, required);
};

message(Msg_CL_ArenaStartChallenge) {
  member(m_TargetGuid, ulong, required);
};

message(Msg_LC_ArenaStartCallengeResult) {
  member(m_TargetGuid, ulong, required);
  member(m_Sign, int, required);
  member(m_ResultCode, int, required);
  member(m_Prime, int, required);
  member(TargetInfo, ArenaInfoMsg, optional);
};

message(Msg_CL_ArenaChallengeOver) {
  member(IsSuccess, bool, required);
  member(ChallengerDamage, int, required);
  member(TargetDamage, int, required);
  member(ChallengerPartnerDamage, DamageInfoData, repeated);
  member(TargetPartnerDamage, DamageInfoData, repeated);
  member(Sign, int, required);
};

message(Msg_LC_ArenaChallengeResult) {
  member(m_ChallengeInfo, ChallengeInfoData, required);
};

message(DamageInfoData) {
  member(OwnerId, int, required);
  member(Damage, int, required);
};

message(ChallengeEntityData) {
  member(Guid, ulong, required);
  member(HeroId, int, required);
  member(Level, int, required);
  member(Rank, int, required);
  member(FightScore, int, required);
  member(NickName, string, required);
  member(UserDamage, int, required);
  member(PartnerDamage, DamageInfoData, repeated);
};

message(ChallengeInfoData) {
  member(Challenger, ChallengeEntityData, required);
  member(Target, ChallengeEntityData, required);
  member(IsChallengeSuccess, bool, required);
  member(EndTime, long, required);
};

message(Msg_CL_ArenaQueryRank) {
};

message(Msg_LC_ArenaQueryRankResult)
{
  member(RankMsg, ArenaInfoMsg, repeated);
};

message(Msg_CL_ArenaQueryHistory) {
};

message(Msg_LC_ArenaQueryHistoryResult) {
  member(ChallengeHistory, ChallengeInfoData, repeated);
};

message(Msg_CL_ArenaChangePartner) {
  member(Partners, int, repeated);
};

message(Msg_LC_ArenaChangePartnerResult) {
  member(Result, int, required);
  member(Partners, int, repeated);
};

message(Msg_CL_ArenaBuyFightCount) {
};

message(Msg_LC_ArenaBuyFightCountResult) {
  member(Result, int, required);
  member(CurFightCount, int, required);
  member(CurBuyTime, int, required);
};

message(Msg_CL_ArenaBeginFight) {
};

message(Msg_CL_ExchangeGift) {
  member(m_GiftCode, string, required);
};

message(Msg_LC_ExchangeGiftResult) {
  member(m_GiftId, int, required);
  member(m_Result, int, required);
  member(m_GiftName, string, optional);
  member(m_GiftDesc, string, optional);
  message(GiftItemInfo)
  {
    member(m_ItemId, int, required);
	member(m_ItemNumber, int, required);
  };
  member(m_GiftItems, GiftItemInfo, repeated);
};

message(Msg_CL_CompoundEquip) {
  member(m_PartId, int, required);
};

message(Msg_LC_CompoundEquipResult) {
  member(m_Result, int, required);
  member(m_PartId, int, required);
  member(m_ItemId, int, required);
};

message(Msg_CL_SetNewbieActionFlag) {
  member(m_Bit, int, required);
  member(m_Num, int, required);
};

message(Msg_LC_SyncNewbieActionFlag) {
  member(m_NewbieActionFlag, long, repeated);
};

message(Msg_LC_WeeklyLoginRewardResult) {
  member(m_ResultCode, int, required);
};

message(Msg_LC_QueueingCountResult) {
  member(m_QueueingCount, int, required);
};

message(Msg_CL_GetOnlineTimeReward) {
  member(m_Index, int, required);
};

message(Msg_LC_GetOnlineTimeRewardResult) {
  member(m_ResultCode, int, required);
  member(m_OnlineTime, int, required);
  member(m_Index, int, required);
};

message(Msg_CL_RecordNewbieFlag) {
	member(m_Bit, int, required);
};

message(SelectedPartnerDataMsg) {
	member(m_Id, int, required);
	member(m_AdditionLevel, int, required);
	member(m_SkillStage, int, required);
};

message(Msg_CL_RequestSkillInfos) {

};

message(Msg_LC_SyncSkillInfos) {
	member(m_Skills, SkillDataInfo, repeated);
};
message(Msg_LC_SyncCombatData)
{
  member(m_Legacys, LegacyDataMsg, repeated);
  member(m_Skills, SkillDataInfo, repeated);
  member(m_XSouls, XSoulDataMsg, repeated);
  member(m_Equipments, JsonItemDataMsg, repeated);
  member(m_PartnerDatas, SelectedPartnerDataMsg, repeated);
  member(m_Fashions, int, repeated);
};

message(Msg_CL_UploadFPS)
{
	member(m_Fps, string, required);
	member(m_Nickname, string, required);
};

message(Msg_LC_SyncGuideFlag)
{
	member(m_Flag, long, required);
};

message(Msg_CL_RequestDare)
{
	member(m_TargetNickname, string, required);
};

message(Msg_LC_RequestDare)
{
	member(m_ChallengerNickname, string, required);
};

message(Msg_CL_RequestDareByGuid)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_LC_RequestDareResult)
{
	member(m_Nickname, string, required);
	member(m_Result, int, required);
};

message(Msg_CL_AcceptedDare)
{
	member(m_ChallengerNickname, string, required);
};

message(Msg_LC_RequestGowPrizeResult)
{
	member(m_Money, int, optional);
	member(m_Gold, int, optional);
	message(AwardItemInfo) {
		member(m_Id, int, required);
    member(m_Num, int, required);
	};
	member(m_Items, AwardItemInfo, repeated);
	member(m_IsAcquirePrize, bool, optional);
	member(m_Result, int, required);
};

message(Msg_CL_RequestEnhanceEquipmentStar)
{
	member(m_ItemID, int, required);
};

message(Msg_LC_RequestEnhanceEquipmentStar)
{
	member(m_Result, int, required);
	member(m_ItemID, int, optional);
	member(m_NewEnhanceLv, int, optional);
	message(DeleteItemInfo) {
		member(m_ItemGuid, ulong, required);
		member(m_Num, int, required);
	};
	member(m_Items, DeleteItemInfo, repeated);
};

message(Msg_LC_SyncCorpsOpResult)
{
	member(m_Result, int, required);
	member(m_Nickname, string, optional);
	member(m_Gold, int, required);
	member(m_GoldCash, int, required);
};

message(Msg_CL_CorpsCreate)
{
	member(m_CorpsName, string, required);
};

message(Msg_CL_CorpsJoin)
{
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_CorpsQuit)
{
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_CorpsAgreeClaimer)
{
	member(m_ClaimerGuid, ulong, required);
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_CorpsRefuseClaimer)
{
	member(m_ClaimerGuid, ulong, required);
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_CorpsKickout)
{
	member(m_TargetGuid, ulong, required);
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_CorpsAppoint)
{
	member(m_TargetGuid, ulong, required);
	member(m_Title, int, required);
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_QueryCorpsInfo)
{
	member(m_CorpsGuid, ulong, required);
};

message(Msg_LC_SyncCorpsInfo)
{
	member(m_Guid, ulong, required);
	member(m_Name, string, required);
	member(m_Level, int, required);
	member(m_Score, int, required);
	member(m_Rank, int, required);
	message(CorpsMember) {
		member(m_Guid, ulong, required);
    member(m_HeroId, int, required);
    member(m_Nickname, string, required);
    member(m_Level, int, required);
    member(m_FightScore, int, required);
    member(m_Title, int, required);
    member(m_DayActiveness, int, optional);
    member(m_WeekActiveness, int, optional);
    member(m_LastLoginTime, long, optional);
    member(m_IsOnline, bool, optional);
	};
	member(m_Members, CorpsMember, repeated);
	message(CorpsClaimer) {
		member(m_Guid, ulong, required);
    member(m_HeroId, int, required);
    member(m_Nickname, string, required);
    member(m_Level, int, required);
    member(m_FightScore, int, required);
    member(m_Time, string, required);
    member(m_CorpsId, ulong, required);
	};
	member(m_Confirms, CorpsClaimer, repeated);
	member(m_Notice, string, required);
	member(m_CreateTime, string, required);
	member(m_Activeness, int, required);
	message(Charpter) {
		member(Index, int, required);
    member(IsOpen, bool, required);
    message(TopInfo) {
    	member(Guid, ulong, required);
    	member(HeroId, int, required);
    	member(Name, string, required);
    	member(Level, int, required);
    	member(Damage, long, required);
    };
    member(Top, TopInfo, repeated);
    message(BattleInfo) {
    	member(SceneId, int, required);
    	member(IsFinish, bool, required);
    	member(Monsters, int, repeated);
    	member(Hps, int, repeated);
    	member(IsFighting, bool, required);
    };
    member(Tollgates, BattleInfo, repeated);
	};
	member(Charpters, Charpter, repeated);
};

message(Msg_CL_QueryCorpsStar)
{
	member(m_Start, int, required);
	member(m_Count, int, required);
};

message(Msg_LC_SyncCorpsStar)
{
	member(m_Start, int, required);
	member(m_Count, int, required);
	member(m_Star, Msg_LC_SyncCorpsInfo, repeated);
};

message(Msg_CL_CorpsDissolve)
{
	member(m_CorpsGuid, ulong, required);
};

message(Msg_CL_EquipTalentCard)
{
  member(ItemGuid, ulong, required);
};

message(Msg_LC_EquipTalentCardResult)
{
  member(ItemGuid, ulong, required);
  member(OldItemGuid, ulong, optional);
  member(Result, int, required);
  member(Slot, int, optional);
};

message(Msg_CLC_UnequipTalentCard)
{
  member(Slot, int, required);
  member(Result, int, optional);
};

message(Msg_CL_AddTalentExperience) {
  member(Slot, int, required);
  member(ItemGuid, ulong, repeated);
  member(Result, int, optional);
  member(Experience, int, optional);
};

message(ItemLeftMsg) {
  member(ItemGuid, ulong, required);
  member(ItemNum, int, required);
};

message(Msg_LC_AddTalentExperienceResult) {
  member(Slot, int, required);
  member(ItemLefts, ItemLeftMsg, repeated);
  member(Result, int, optional);
  member(Experience, int, optional);
};

message(Msg_CL_UpgradeEquipBatch)
{

};

message(Msg_LC_UpgradeEquipBatch)
{
	member(m_Result, int, required);
	member(m_Money, int, required);
	member(m_Gold, int, required);
	member(m_Guids, ulong, repeated);
	member(m_Level, int, repeated);
	member(m_GoldCash, int, required);
};

message(Msg_CL_RequestMpveAward)
{
	member(m_Type, int, required);
	member(m_Index, int, required);
};
message(Msg_CL_SendChat)
{
	member(m_Type, int, required);
	member(m_TargetNickName, string, optional);
	member(m_Content, string, required);
};

message(Msg_LC_ChatStatus)
{
	member(m_Result, int, required);
};

message(Msg_LC_ChatResult)
{
	member(m_Type, int, required);
	member(m_SenderGuid, ulong, optional);
	member(m_SenderNickName, string, optional);
  member(m_SenderHeroId, int, optional);
	member(m_TargetGuid, ulong, optional);
	member(m_TargetNickName, string, optional);
  member(m_TargetHeroId, int, optional);
	member(m_Content, string, required);
};

message(Msg_LC_ChatWorldResult)
{
	member(m_SenderGuid, ulong, optional);
	member(m_SenderNickName, string, optional);
	member(m_SenderHeroId, int, optional);
	member(m_Content, string, required);
};

message(Msg_LC_SystemChatWorldResult)
{
	member(m_Content, string, required);
};

message(Msg_CL_ChatAddShield)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_CL_ChatAddShieldByName)
{
	member(m_TargetNickName, string, required);
};

message(Msg_LC_ChatAddShieldResult)
{
	member(m_Result, int, required);
	member(m_TargetGuid, ulong, optional);
	member(m_TargetNickName, string, optional);
	member(m_ShieldInfo, ChatShieldInfoForMsg, optional);
};

message(Msg_CL_ChatDelShield)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_LC_ChatDelShieldResult)
{
	member(m_TargetGuid, ulong, required);
	member(m_Result, int, required);
};

message(Msg_CL_RequireChatRoleInfo)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_LC_ChatRoleInfoReturn)
{
	member(m_TargetGuid, ulong, required);
	member(m_TargetNickName, string, required);
	member(m_TargetLevel, int, optional);
	member(m_TargetPower, int, optional);
	member(m_IsShield, bool, optional);
  member(m_IsOnline, bool, optional);
  member(m_HeroId, int, optional);
};

message(Msg_CL_RequireChatEquipInfo)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_LC_ChatEquipInfoReturn)
{
	member(m_TargetGuid, ulong, required);
};

message(Msg_CL_RequireChatShieldList)
{
};

message(ChatShieldInfoForMsg) {
  member(Guid, ulong, required);
  member(Nickname, string, required);
  member(HeroId, int, required);
  member(Level, int, required);
  member(FightingScore, int, required);
  member(IsOnline, bool, required);
};

message(Msg_LC_ChatShieldListReturn)
{
	member(m_ShieldInfoList, ChatShieldInfoForMsg, repeated);
};

message(Msg_LC_SendScreenTip)
{
	member(m_Content, string, required);
	member(m_Align, int, optional);
};

message(Msg_CL_SetCorpsNotice)
{
	member(m_Content, string, required);
};
message(Msg_CL_GmPay)
{
    member(m_Diamonds, int, required);
};

message(Msg_LC_GmPayResult)
{
    member(m_Vip, int, required);
    member(m_Diamonds, int, required);
    member(m_DateTime, string, required);
};

message(Msg_CL_GainVipReward)
{
    member(m_VipLevel, int, required);
};

message(Msg_LC_GainVipRewardResult)
{
    member(m_ResultCode, int, required);
    member(m_VipLevel, int, required);
};

message(Msg_CL_QueryValidCorpsList)
{
    member(m_Start, int, required);
    member(m_Count, int, required);
};

message(Msg_LC_SyncValidCorpsList)
{
    member(m_Start, int, required);
    member(m_Count, int, required);
    member(m_List, Msg_LC_SyncCorpsInfo, repeated);
};

message(Msg_LC_SyncGowRankInfo)
{
	member(m_Gow, GowDataMsg, required);
};

message(Msg_LC_GainFirstPayRewardResult)
{
    member(m_ResultCode, int, required);
};
message(Msg_CL_BuyFashion)
{
	member(m_ItemID, int, required);
	member(m_TimeType, int, required);
};

message(Msg_LC_BuyFashionResult)
{
	member(m_Result, int, required);
	member(m_ItemID, int, optional);
	member(m_TimeType, int, optional);
	member(m_DeadlineSeconds, int, optional);
	member(m_DressOn, bool, optional);
	member(m_Money, int, optional);
	member(m_Gold, int, optional);
	member(m_GoldCash, int, optional);
};
message(Msg_LC_AwardFashionResult)
{
	member(m_ItemID, int, required);
	member(m_DeadlineSeconds, int, optional);
	member(m_DressOn, bool, optional);
	member(m_UseDays, int, optional);
	member(m_IsForever, bool, optional);
};

message(Msg_CL_MountFashion)
{
	member(m_FashionID, int, required);
};

message(Msg_LC_MountFashionResult)
{
	member(m_Result, int, required);
	member(m_FashionID, int, optional);
};
message(Msg_CL_UnmountFashion)
{
	member(m_FashionID, int, required);
};

message(Msg_LC_UnmountFashionResult)
{
	member(m_Result, int, required);
	member(m_FashionID, int, optional);
};

message(Msg_LC_NoticeFashionOverdueSoon)
{
	member(m_ItemID, int, required);
};

message(Msg_LC_NoticeFashionOverdue)
{
	member(m_ItemID, int, required);
};

message(Msg_LC_BuyGoodsSucceed)
{
    member(m_GoodsId, string, required);
    member(m_IsFirstPay, bool, required);
    member(m_IsFirstBuyThis, bool, required);
    member(m_VipLevel, int, required);
    member(m_AccountId, string, required);
    member(m_OrderId, string, required);
    member(m_PayType, string, required);
    member(m_GoodsNum, int, required);
    member(m_CurrencyType, string, required);
};
message(Msg_LC_DrawRewardResult)
{
	member(m_Result, int, required);
	member(m_Money, int, optional);
	member(m_Diamond, int, optional);
	member(m_RewardId, int, repeated);
	message(LotteryInfo)
	{
    member(m_Id, int, required);
    member(m_CurFreeCount, int, required);
    member(m_LastDrawTime, string, required);
	};
	member(m_Lottery, LotteryInfo, repeated);
	member(m_LotteryType, int, required);
	member(m_GoldCash, int, optional);
};

message(Msg_CL_DrawReward)
{
	member(m_RewardType, int, required);
	member(m_LotteryType, int, required);
	member(m_Time, long, required);
};

message(Msg_CL_CombinTalentCard) {
	member(m_PartGuid, ulong, repeated);
	member(m_ItemNum, int, repeated);
};

message(Msg_CL_CombinTalentCardResult) {
	member(m_PartGuid, ulong, repeated);
	member(m_ItemNum, int, repeated);
	member(m_Result, int, required);
	member(m_gainItemId, int, optional);
};

message(Msg_CL_BuyEliteCount)
{
    member(m_EliteId, int, required);
};

message(Msg_LC_BuyEliteCountResult)
{
    member(m_EliteId, int, required);
    member(m_ResultCode, int, required);
    member(m_DiamondCount, int, required);
    member(m_GoldCash, int, required);
};

message(Msg_CL_StartPartnerBattle)
{
    member(m_IndexId, int, required);
};

message(Msg_LC_StartPartnerBattleResult)
{
    member(m_ResultCode, int, required);
    member(m_MatchKey, int, optional);
    member(m_HideAttrKey, int, optional);
    member(m_Index, int, optional);
};

message(Msg_CL_EndPartnerBattle)
{
    member(m_BattleResult, int, required);
    member(m_MatchKey, int, required);
};

message(Msg_LC_EndPartnerBattleResult)
{
    member(m_ResultCode, int, required);
    member(m_RemainCount, int, optional);
    member(m_FinishedCount, int, optional);
    member(m_BattleResult, int, optional);
    member(m_RewardItemId, int, optional);
    member(m_RewardItemNum, int, optional);
    member(m_Partners, int, repeated);
};

message(Msg_LC_BuyPartnerCombatTicketResult)
{
    member(m_ResultCode, int, required);
    member(m_CurDiamond, int, optional);
    member(m_RemainCount, int, optional);
    member(m_BuyCount, int, optional);
    member(m_GoldCash, int, optional);
};

message(Msg_CL_CorpsIndirectJoin)
{
    member(m_UserGuid, ulong, required);
};

message(TDMatchInfoMsg) {
  member(Guid, ulong, required);
  member(NickName, string, required);
  member(Level, int, required);
  member(FightScore, int, required);
  member(HeroId, int, required);
  member(DropId, int, required);
};
message(Msg_CL_QueryTDMatchGroup) {
  member(IsNext, bool, required);
};

message(Msg_LC_QueryTDMatchGroupResult) {
  member(Result, int, required);
  member(IsFull, bool, required);
  member(LeftFightCount, int, optional);
  member(QueryCount, int, optional);
  member(BuyFightCount, int, optional);
  member(MatchGroup, TDMatchInfoMsg, repeated);
};

message(Msg_CL_StartTDChallenge) {
  member(Guid, ulong, required);
};

message(Msg_LC_StartTDChallengeResult) {
  member(Result, int, required);
  member(IsFull, bool, optional);
  member(LeftFightCount, int, optional);
  member(QueryCount, int, optional);
  member(BuyFightCount, int, optional);
  member(Sign, int, optional);
  member(TargetInfo, ArenaInfoMsg, optional);
};

message(Msg_CL_TDBeginFight) {
};

message(Msg_CL_TDChallengeOver) {
  member(IsSuccess, bool, required);
  member(ChallengerDamage, int, required);
  member(TargetDamage, int, required);
  member(ChallengerPartnerDamage, DamageInfoData, repeated);
  member(TargetPartnerDamage, DamageInfoData, repeated);
  member(Sign, int, required);
};

message(Msg_LC_TDChallengeResult) {
  member(Result, int, required);
  member(ChallengeInfo, ChallengeInfoData, optional);
};

message(Msg_CL_BuyTDFightCount) {
};

message(Msg_LC_BuyTDFightCountResult) {
  member(Result, int, required);
  member(BuyCount, int, optional);
  member(CurFightCount, int, optional);
};

message(Msg_LC_SyncVitality)
{
	member(m_Vitality, int, required);
};

message(Msg_LC_CorpsSignIn)
{
	member(m_Stamina, int, required);
	member(m_CorpsSignInState, bool, required);
};

message(Msg_LC_PartnerCombatInfo)
{
    member(m_Partners, int, repeated);
    member(m_RemainCount, int, required);
    member(m_FinishedCount, int, required);
    member(m_BuyCount, int, required);
};

message(FightingScoreEntityMsg)
{
  member(Guid, ulong, required);
  member(HeroId, int, required);
  member(NickName, string, required);
  member(Level, int, required);
  member(FightingScore, int, required);
  member(Rank, int, required);
};

message(Msg_CLC_QueryFightingScoreRank)
{
  member(RankEntities, FightingScoreEntityMsg, repeated);
};
message(Msg_CL_OpenDomain)
{
	member(DomainType, int, required);
	member(Key, ulong, required);
};

message(Msg_LC_LootOpResult)
{
	member(Result, int, required);
};

message(LootInfoMsg) {
	member(Key, ulong, required);
  member(Guid, ulong, required);
  member(HeroId, int, required);
  member(NickName, string, required);
  member(Level, int, required);
  member(FightScore, int, required);
  member(IsOpen, bool, required);
  member(IsGetAward, bool, required);
  member(DomainType, int, required);
  member(SessionStartTime, string, required);
  member(SessionEndTime, string, required);
  member(EquipInfo, JsonItemDataMsg, repeated);
  member(ActiveSkills, SkillDataInfo, repeated);
  member(FightParters, PartnerDataMsg, repeated);
  member(LegacyAttr, LegacyDataMsg, repeated);
  member(XSouls, XSoulDataMsg, repeated);
  member(Talents, JsonTalentDataMsg, repeated);
  member(FightOrder, int, repeated);
  member(LootOrder, int, repeated);
  member(FashionInfo, FashionSynMsg, repeated);
  member(Income, int, required);
  member(Loss, int, required);
  member(LootType, int, required);
};

message(Msg_LC_SyncLootInfo)
{
  member(Info, LootInfoMsg, required);
};

message(Msg_LC_LootMatchResult)
{
	member(Result, int, required);
	member(Money, int, required);
	member(Income, int, optional);
	member(LootType, int, optional);
	member(IsShow, bool, optional);
  member(Target, LootInfoMsg, optional);
};

message(Msg_LC_LootChangeDefenseOrder)
{
	member(Key, ulong, required);
  member(Order, int, repeated);
};

message(Msg_CL_StartLoot)
{
	member(TargetKey, ulong, required);
	member(SelfKey, ulong, required);
};

message(Msg_LC_StartLootResult)
{
	member(m_TargetGuid, ulong, required);
  member(m_Sign, int, required);
  member(m_ResultCode, int, required);
  member(m_Prime, int, required);
};

message(Msg_CL_OverLoot) {
  member(IsSuccess, bool, required);
  member(LooterDamage, int, required);
  member(DefenderDamage, int, required);
  member(Sign, int, required);
};

message(LootEntityData) {
	member(Key, ulong, required);
  member(Guid, ulong, required);
  member(HeroId, int, required);
  member(Level, int, required);
  member(FightScore, int, required);
  member(NickName, string, required);
  member(UserDamage, int, required);
  member(DefenseOrder, int, repeated);
  member(LootOrder, int, repeated);
};

message(Msg_LC_OverLootResult) {
	member(DomainType, int, required);
  member(Booty, int, required);
  member(Looter, LootEntityData, required);
  member(Defender, LootEntityData, required);
  member(IsLootSuccess, bool, required);
  member(EndTime, string, required);
};

message(LootHistoryData) {
	member(DomainType, int, required);
	member(Booty, int, required);
  member(Looter, LootEntityData, required);
  member(IsLootSuccess, bool, required);
  member(BeginTime, string, required);
  member(EndTime, string, required);
};

message(Msg_LC_SyncLootHistory)
{
	member(History, LootHistoryData, repeated);
};

message(Msg_LC_SyncLootAward)
{
	member(ItemId, int, required);
	member(ItemNum, int, required);
	member(Result, int, required);
};

message(Msg_CL_RefusedDare)
{
	member(m_ChallengerNickname, string, required);
};

message(Msg_LC_SyncLotteryInfo)
{
  member(m_Id, int, required);
  member(m_CurFreeCount, int, required);
};

message(Msg_CL_EquipmentStrength)
{
	member(m_ItemID, int, required);
	member(m_IsProtected, bool, required);
};

message(Msg_LC_EquipmentStrengthResult)
{
	member(m_Result, int, required);
	member(m_ItemID, int, optional);
	member(m_NewStrengthLv, int, optional);
	member(m_OldStrengthLv, int, optional);
	member(m_DeductItem, bool, optional);
	member(m_IsProtected, bool, optional);
	message(DeleteItemInfo) {
		member(m_ItemGuid, ulong, required);
		member(m_Num, int, required);
	};
	member(m_Items, DeleteItemInfo, repeated);
	member(m_StrengthFailCount, int, optional);
};

message(Msg_CL_SetFashionShow)
{
	member(m_FashionPartType, int, required);
	member(m_IsHide, bool, required);
};

message(Msg_LC_SetFashionShowResult)
{
	member(m_Result, int, required);
	member(m_FashionPartType, int, required);
	member(m_IsHide, bool, required);
};

message(Msg_LC_MorrowRewardActive)
{
    member(m_ActiveIndex, int, required);
    member(m_RemainTime, int, required);
};

message(Msg_LC_GetMorrowRewardResult)
{
    member(m_ResultCode, int, required);
};

message(Msg_LC_SyncHomeNotice)
{
    member(m_Content, string, required);
};

message(Msg_LC_LootCostVitality)
{
	member(m_Result, int, required);
	member(m_Vitality, int, required);
};

message(Msg_CL_QueryLootInfo)
{
	member(m_Key, ulong, required);
};

message(Msg_CL_CostVitality)
{
	member(m_Key, ulong, required);
};

message(Msg_CL_LootMatchTarget)
{
	member(m_Key, ulong, required);
};

message(Msg_CL_GetLootAward)
{
	member(m_Key, ulong, required);
};

message(Msg_LC_SyncRecentLoginState)
{
    member(m_RecentLoginState, uint, required);
    member(m_SumLoginDayCount, int, required);
};

message(Msg_LC_SyncFightingScore)
{
	member(Guid, ulong, required);
	member(Score, int, required);
};

message(Msg_LC_GetLoginLotteryResult)
{
    member(m_ResultCode, int, required);
    member(m_RewardId, int, optional);
    member(m_UsedLoginDrawLotteryCount, int, optional);
};

message(Msg_LC_RefreshPartnerCombatResult)
{
    member(m_ResultCode, int, required);
    member(m_Partners, int, repeated);
    member(m_Gold, int, optional);
    member(m_Diamond, int, optional);
    member(m_GoldCash, int, optional);
};

message(Msg_LC_LootChangeLootOrder)
{
	member(Key, ulong, required);
  member(Order, int, repeated);
};

message(Msg_LC_ItemsGiveBack)
{
	message(ItemInfo) {
    member(m_Id, int, required);
    member(m_Num, int, required);
  };
  member(m_Items, ItemInfo, repeated);
};

message(Msg_CL_QueryCorpsByName)
{
	member(DimName, string, required);
};

message(Msg_LC_QueryCorpsByName)
{
	member(m_Count, int, required);
  member(m_List, Msg_LC_SyncCorpsInfo, repeated);
};
message(Msg_CLC_StoryMessage)
{
  message(MessageArg) {
    member(val_type, int, required);
    member(str_val, string, required);
  };
  member(m_MsgId, string, required);
  member(m_Args, MessageArg, repeated);
};

message(Msg_CL_OpenCharpter)
{
  member(CharpterId, int, required);
};

message(Msg_CL_ResetCharpter)
{
  member(CharpterId, int, required);
};

message(Msg_CL_StartCorpsTollgate)
{
	member(CarpterId, int, required);
	member(SceneId, int, required);
};

message(Msg_LC_InviteInfoAfterRoleEnter)
{
  member(m_InviteCode, string, required);
  member(m_IsInvited, bool, required);
  member(m_InviteeCount, int, required);
  member(m_OverLv30Count, int, required);
  member(m_OverLv50Count, int, required);
  member(m_RewardsHaveRecived, int, repeated);
};

message(Msg_CL_RequestInvite)
{
  member(m_InviteCode, string, required);
};

message(Msg_LC_RequestInviteResult)
{
  member(m_ResultCode, int, required);
  member(m_RewardID, int, required);
};

message(Msg_LC_UpdateInviteInfo)
{
  member(m_InviteeCount, int, required);
  member(m_OverLv30Count, int, required);
  member(m_OverLv50Count, int, required);
};

message(Msg_CL_RequestInviteReward)
{
  member(m_RewardId, int, required);
};

message(Msg_LC_RequestInviteRewardResult)
{
  member(m_ResultCode, int, required);
  member(m_RewardId, int, required);
};

message(Msg_CLC_CollectGrowthFund)
{
  member(LevelIndex, int, required);
  member(GrowthFundValue, int, optional);
  member(Result, int, optional);
  member(Diamond, int, optional);
};
message(Msg_LC_SyncCorpsDareCount)
{
	message(DareData) {
    member(CharpterId, int, required);
  	member(CurDareCount, int, required);
  };
	member(Data, DareData, repeated);	
};

message(Msg_CLC_CollectChapterAward)
{
  member(Chapter, int, required);
  member(OrderId, int, required);
  member(AwardSign, int, optional);
  member(Result, int, optional);
};

message(Msg_CL_InteractivePrize)
{
	member(m_ActorId, int, required);
  member(m_LinkId, int, required);
  member(m_StoryId, int, required);
};

message(Msg_LC_InteractivePrize)
{
	member(m_ActorId, int, required);
  member(m_LinkId, int, required);
  member(m_StoryId, int, required);
  member(m_IsValid, bool, required);
  member(m_Ids, int, repeated);
  member(m_Nums, int, repeated);
};

message(Msg_CL_PushDelayData)
{
	member(m_RoundtripTime, long, required);
};

message(Msg_CL_EnterField)
{
	member(m_SceneId, int, required);
	member(m_RoomId, int, optional);
};

message(Msg_CL_ChangeFieldRoom)
{
	member(m_SceneId, int, optional);
	member(m_RoomId, int, optional);
};

message(Msg_LC_LackOfSpace)
{
	member(m_Succeed, bool, required);
	member(m_Type, int, required);
	member(m_ReceiveNum, int, required);
	member(m_FreeNum, int, required);
	member(m_MailGuid, ulong, required);
};
message(Msg_LC_SyncGowOtherInfo)
{
	member(m_WinNum, int, required);
	member(m_LoseNum, int, required);
};
message(ItemInfo_UseItem)
{
	member(ItemID, int, required);
	member(ItemGuid, ulong, required);
	member(Num, int, required);
	member(RandomProperty, int, required);
};
message(Msg_CL_ItemUseRequest)
{
	member(m_ItemGuid, ulong, required);
};
message(Msg_LC_ItemUseResult)
{
	member(m_ResultCode, int, required);
	member(m_ItemGuids, ulong, repeated);
	member(m_nums, int, repeated);
	member(m_Items, ItemInfo_UseItem, repeated);
	member(m_arg, int, required);
};
message(Msg_CL_AuctionQuery)
{
	member(Category1, int, required);
	member(Category2, int, required);
	member(AscPrice, bool, required);
	member(ItemName, string, required);
	member(PageNo, int, required);
};
message(Msg_CL_AuctionSell)
{
	member(ItemGuid, ulong, required);
	member(ItemNum, int, required);
	member(ItemType, int, required);
	member(Price, int, required);
};
message(Msg_CL_AuctionBuy)
{
	member(AuctionGuid, ulong, required);
};
message(Msg_CL_AuctionUnshelve)
{
	member(AuctionGuid, ulong, required);
};
message(Msg_CL_AuctionReceive)
{
	member(AuctionGuid, ulong, required);
};
message(Msg_CL_AuctionSelfAuction)
{

};
message(AuctionInfo)
{
	member(auctionGuid, ulong, required);
	member(userGuid, ulong, required);
	member(userNickname, string, required);
	member(itemStatus, int, required);
	member(statusLeftTime, long, required);
	member(itemType, int, required);
	member(itemNum, int, required);
	member(itemInfo, JsonItemDataMsg, required);
	member(price, int, required);
};
message(Msg_LC_AuctionSelfAuction)
{
	member(auctionInfo, AuctionInfo, repeated);
	member(IsAuctionOpen, bool, required);
};
message(Msg_LC_AuctionAuction)
{	
	member(auctionInfo, AuctionInfo, repeated);
	member(IsLastPage, bool, required);
	member(PageNo, int, required);
};
message(Msg_LC_AuctionOpResult)
{
	member(result, int, required);
	member(ItemGuid, ulong, optional);
	member(ItemNum, int, optional);
	member(ItemType, int, optional);
	member(Price, int, optional);
	member(itemInfo, JsonItemDataMsg, optional);
	member(Cost, int, optional);
};