explanationfile("Data.version.notes");
///////////////////////////////////////////////////////////////////////////////
// data definitions
version("0.3.11");
///////////////////////////////////////////////////////////////////////////////
package(GameFrameworkData);

typeconverter(DateTime)
{
	messagetype(string);
	message2logic
	{:m_{1} = DateTime.ParseExact(m_{0}.{1},"yyyyMMddHHmmss",null);:};
	logic2message
	{:m_{0}.{1} = m_{1}.ToString("yyyyMMddHHmmss");:};
};

typeconverter("List<int>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitNumericList<int>(new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinNumericList(",",m_{1});:};
	crudcode
	{:
public int Get{0}Count()
{{
  return m_{0}.Count;
}}
public void Set{0}(int ix, int val)
{{
  m_{0}[ix]=val;
  {1};
}}
public int Get{0}(int ix)
{{
  return m_{0}[ix];
}}
public void Add{0}(int val)
{{
  m_{0}.Add(val);
  {1};
}}
public void Del{0}(int ix)
{{
  m_{0}.RemoveAt(ix);
  {1};
}}
public void Visit(MyAction<int> visit)
{{
  foreach(int v in m_{0}){{
    visit(v);
  }}
}}
public int[] ToArray()
{{
  return m_{0}.ToArray();
}}:};
};

typeconverter("List<DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeList<int>(new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeList(",",m_{1});:};
	crudcode
	{:
public int Get{0}Count()
{{
  return m_{0}.Count;
}}
public void Set{0}(int ix, DateTime val)
{{
  m_{0}[ix]=val;
  {1};
}}
public DateTime Get{0}(int ix)
{{
  return m_{0}[ix];
}}
public void Add{0}(DateTime val)
{{
  m_{0}.Add(val);
  {1};
}}
public void Del{0}(int ix)
{{
  m_{0}.RemoveAt(ix);
  {1};
}}
public void Visit(MyAction<DateTime> visit)
{{
  foreach(DateTime v in m_{0}){{
    visit(v);
  }}
}}
public DateTime[] ToArray()
{{
  return m_{0}.ToArray();
}}:};
};

typeconverter("Dictionary<DateTime,DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeDictionary(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<DateTime,int>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeKeyDictionary<int>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeKeyDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<int,DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeValueDictionary<int>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeValueDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<int,string>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitNumericDictionary<int,string>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinNumericDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<string,string>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitNumericDictionary<string,string>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinNumericDictionary(";",",",m_{1});:};
};

message(GeneralRecordData)
{
	option(dontgenenum);
	option(dontgendb);
  member(PrimaryKeys, string, repeated);
  member(ForeignKeys, string, repeated);
  member(DataVersion, int32, required);
  member(Data, bytes, optional);
};

message(TableGlobalParam)
{
	member(ParamType, string, required){
		maxsize(32);
		primarykey;
	};
	member(ParamValue, string, required){
		maxsize(64);
	};
};

message(TableGuid) 
{
	member(GuidType, string, required){
		maxsize(24);
		primarykey;
	};
	member(GuidValue, ulong, required);
};

message(TableNickname)
{
	member(Nickname, string, required){
		maxsize(32);
		primarykey;
	};
	member(UserGuid, ulong, required);   
};

message(TableGowStar)
{
	member(Guid, string, required){
		maxsize(16);
		primarykey;
	};
	member(LogicServerId, int, required);
	member(Rank, int, required);
	member(UserGuid, long, required);  
	member(Nickname, string, required){
		maxsize(32);
	};  
	member(HeroId, int, required);
	member(Level, int, required);
	member(FightingScore, int, required);  
	member(GowElo, int, required); 
	member(RankId, int, required);
	member(Point, int, required);
};

message(TableMailInfo)
{
	member(Guid, long, required){
		primarykey;
	};
	member(LogicServerId, int, required);  
	member(ModuleTypeId, int, required);
	member(Sender, string, required){
		maxsize(32);
	};
	member(Receiver, long, required);  
	member(SendDate, DateTime, required){
		maxsize(24);
	};  
	member(ExpiryDate, DateTime, required){
		maxsize(24);
	};   
	member(Title, string, required){
		maxsize(64);
	};  
	member(Text, string, required){
		maxsize(1024);
	};  
	member(Money, int, required);  
	member(Gold, int, required);  
	member(Stamina, int, required);  
	member(ItemIds, string, required){
		maxsize(128);
	};  
	member(ItemNumbers, string, required){
		maxsize(64);
	};  
	member(LevelDemand, int, required);
	member(IsRead, bool, required);
};

message(TableActivationCode)
{
	member(ActivationCode, string, required){
		maxsize(32);
		primarykey;
	};
	member(IsActivated, bool, required);  
	member(LogicServerId, int, required);
	member(AccountId, string, required){
		maxsize(64);
	};
};

message(TableArenaInfo)
{
	member(UserGuid, long, required){
		primarykey;
	};
	member(LogicServerId, int, required);
	member(Rank, int, required);   
	member(IsRobot, bool, required);   
	member(ArenaBytes, "byte[]", required){
		maxsize(8192);
	};
	member(LastBattleTime, DateTime, required){
		maxsize(24);
	};
	member(LeftFightCount, int, required);
	member(BuyFightCount, int, required);
	member(LastResetFightCountTime, DateTime, required){
		maxsize(24);
	};
	member(ArenaHistroyTimeList, string, required){
		maxsize(255);
	};
	member(ArenaHistroyRankList, "List<int>", required){
		maxsize(255);
	};
};

message(TableArenaRecord)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(Rank, int, required);
	member(IsChallengerSuccess, bool, required);
	member(BeginTime, DateTime, required){
		maxsize(24);
	};
	member(EndTime, DateTime, required){
		maxsize(24);
	};  
	member(CGuid, long, required);
	member(CHeroId, int, required);
	member(CLevel, int, required);
	member(CFightScore, int, required);  
	member(CNickname, string, required){
		maxsize(32);
	};
	member(CRank, int, required);  
	member(CUserDamage, int, required); 
	member(CPartnerId1, int, required);
	member(CPartnerDamage1, int, required);
	member(CPartnerId2, int, required);
	member(CPartnerDamage2, int, required);
	member(CPartnerId3, int, required);
	member(CPartnerDamage3, int, required); 
	member(TGuid, long, required);
	member(THeroId, int, required);
	member(TLevel, int, required);
	member(TFightScore, int, required);  
	member(TNickname, string, required){
		maxsize(32);
	};
	member(TRank, int, required);  
	member(TUserDamage, int, required);
	member(TPartnerId1, int, required);
	member(TPartnerDamage1, int, required);
	member(TPartnerId2, int, required);
	member(TPartnerDamage2, int, required);
	member(TPartnerId3, int, required);
	member(TPartnerDamage3, int, required);  
};

message(TableLootInfo)
{
	member(UserGuid, long, required){
		primarykey;
	};  
	member(IsPool, bool, required);
	member(IsVisible, bool, required);
	member(LootKey, long, required);
	member(Nickname, string, required){
		maxsize(32);
	};  
	member(HeroId, int, required);
	member(Level, int, required);
	member(FightScore, int, required);  
	member(LastBattleTime, DateTime, required){
		maxsize(24);
	};
	member(DomainType, int, required);
	member(BeginTime, DateTime, required){
		maxsize(24);
	};
	member(IsOpen, bool, required);
	member(IsGetAward, bool, required);
	member(LootAward, float, required);
	member(TargetLootKey, long, required);
	member(FightOrderList, string, required){
		maxsize(64);
	};
	member(LootOrderList, string, required){
		maxsize(64);
	};
	member(LootIncome, int, required);
	member(LootLoss, int, required);
	member(LootType, int, required);
	member(LootBytes, "byte[]", required){
		maxsize(8192);
	};
};

message(TableLootRecord)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};  
	  
  member(UserGuid, long, required){
	  foreignkey;
	};
	member(IsPool, bool, required);
	member(IsLootSuccess, bool, required);
	member(LootBeginTime, DateTime, required){
		maxsize(24);
	};
	member(LootEndTime, DateTime, required){
		maxsize(24);
	};  
	member(DomainType, int, required);
	member(Booty, int, required);
	member(LGuid, long, required);
	member(LHeroId, int, required);
	member(LLevel, int, required);
	member(LFightScore, int, required);  
	member(LNickname, string, required){
		maxsize(32);
	};
	member(LUserDamage, int, required);
	member(LDefenseOrderList, string, required){
		maxsize(64);
	};
	member(LLootOrderList, string, required){
		maxsize(64);
	};
	member(DGuid, long, required);
	member(DHeroId, int, required);
	member(DLevel, int, required);
	member(DFightScore, int, required);  
	member(DNickname, string, required){
		maxsize(32);
	};
	member(DUserDamage, int, required);
	member(DDefenseOrderList, string, required){
		maxsize(64);
	};
	member(DLootOrderList, string, required){
		maxsize(64);
	};
};

message(TableCorpsInfo)
{
	member(CorpsGuid, long, required){
		primarykey;
	};  
	member(LogicServerId, int, required);
	member(CorpsName, string, required){
		maxsize(32);
	};
	member(Level, int, required);
	member(Score, int, required);
	member(Rank, int, required);
	member(Activeness, int, required);
	member(Notice, string, required){
		maxsize(255);
	};
	member(LastResetActivenessTime, DateTime, required){
		maxsize(24);
	};
	member(CreateTime, DateTime, required){
		maxsize(24);
	};
	member(TollgateBytes, "byte[]", required){
		maxsize(4096);
	};
};

message(TableCorpsMember)
{
	member(UserGuid, long, required){
		primarykey;
	};
  member(CorpsGuid, long, required){
	  foreignkey;
	};
	member(Title, int, required);
	member(Nickname, string, required){
		maxsize(32);
	};
	member(HeroId, int, required);
	member(Level, int, required);
	member(FightScore, int, required);
	member(DayActiveness, int, required);
	member(WeekActiveness, int, required);
	member(ActivenessHistoryDate, DateTime, required){
		maxsize(255);
	};
	member(ActivenessHistoryValue, string, required){
		maxsize(64);
	};
	member(LastLoginTime, long, required);
};

message(TableFightInfo)
{
	member(Guid, string, required){
		maxsize(16);
		primarykey;
	};
	member(LogicServerId, int, required);
	member(Rank, int, required);
	member(UserGuid, long, required);  
	member(HeroId, int, required);
	member(Nickname, string, required){
		maxsize(32);
	};
	member(Level, int, required);
	member(FightingScore, int, required);  
};

message(TableHomeNotice)
{
	member(LogicServerId, int, required){
		primarykey;
	};
	member(Content, string, required){
		maxsize(2048);
	};
	member(CreateTime, DateTime, required){
		maxsize(24);
	};
};

message(TableLoginLotteryRecord)
{
	member(RecordId, long, required){
		primarykey;
	};
	member(AccountId, string, required){
		maxsize(64);
	};  
	member(UserGuid, long, required);
	member(Nickname, string, required){
		maxsize(32);
	};
	member(RewardId, int, required);
	member(CreateTime, DateTime, required){
		maxsize(24);
	};
};

message(TableInviterInfo)
{
	member(UserGuid, long, required){
		primarykey;
	};   
	member(InviteCode, string, required){
		maxsize(24);
	};
	member(InviterGuid, long, required);
	member(InviterLevel, int, required);
	member(RewardedList, string, required){
		maxsize(24);
	};
	member(InviteeGuidList, string, required){
		maxsize(1536);
	};
	member(InviteeLevelList, string, required){
		maxsize(512);
	};  
};

message(TableUndonePayment)
{
	member(OrderId, string, required){
		maxsize(48);
		primarykey;
	};
	member(GoodsRegisterId, string, required){
		maxsize(48);
	};
	member(GoodsNum, int, required);
	member(GoodsPrice, float, required);
	member(UserGuid, long, required);
	member(ChannelId, string, required){
		maxsize(24);
	};  
};

//////////////////////////////////////////////////////////////////////
message(TableAccount) 
{	
	member(LogicServerId, int, required){
		primarykey;
	};  
	member(AccountId, string, required){
		maxsize(64);
		primarykey;
	};	
	member(IsBanned, bool, required); 
	member(UserGuid1, ulong, required);
	member(UserGuid2, ulong, required);
	member(UserGuid3, ulong, required); 
};

message(TableUserInfo) 
{
	member(Guid, ulong, required){
		primarykey;
	};
	member(LogicServerId, int, required){
		foreignkey;
	};
	member(AccountId, string, required){
		maxsize(64);
		foreignkey;
	};  
	member(Nickname, string, required){
		maxsize(32);
	};
	member(HeroId, int, required);	
	member(CreateTime, DateTime, required){
		maxsize(24);
	};
	member(Level, int, required); 
	member(Money, int, required);
	member(Gold, int, required);
	member(ExpPoints, int, required);
	member(CitySceneId, int, required);
	member(Vip, int, required);
	member(LastLogoutTime, DateTime, required){
		maxsize(24);
	};
	member(AchievedScore, int, required);  
};

message(TableUserGeneralInfo)
{
	member(Guid, long, required){
		primarykey;
	};
	member(GuideFlag, long, required);
	member(NewbieFlag, string, required){
		maxsize(255);
	};
	member(NewbieActionFlag, string, required){
		maxsize(255);
	};
	member(BuyMoneyCount, int, required);
	member(LastBuyMoneyTimestamp, double, required); 
	member(LastResetMidasTouchTime, DateTime, required){
		maxsize(24);
	};
	member(SellIncome, int, required);
	member(LastSellTimestamp, double, required);
	member(LastResetSellTime, DateTime, required){
		maxsize(24);
	};
	member(ExchangeGoodList, string, required){
		maxsize(255);
	};
	member(ExchangeGoodNumber, string, required){
		maxsize(64);
	};
	member(ExchangeGoodRefreshCount, string, required){
		maxsize(255);
	};
	member(LastResetExchangeGoodTime, DateTime, required){
		maxsize(24);
	};  
	member(LastResetDaySignCountTime, DateTime, required){
		maxsize(24);
	};
	member(MonthSignCount, int, required);  
	member(LastResetMonthSignCountTime, DateTime, required){
		maxsize(24);
	};
	member(MonthCardExpireTime, DateTime, required){
		maxsize(24);
	};  
	member(IsWeeklyLoginRewarded, bool, required);
	member(WeeklyLoginRewardList, string, required){
		maxsize(64);
	};
	member(LastResetWeeklyLoginRewardTime, DateTime, required){
		maxsize(24);
	};
	member(DailyOnlineDuration, int, required);
	member(DailyOnlineRewardList, string, required){
		maxsize(48);
	};
	member(LastResetDailyOnlineTime, DateTime, required){
		maxsize(24);
	};
	member(IsFirstPaymentRewarded, bool, required);  
	member(VipRewaredList, string, required){
		maxsize(255);
	};  
	member(MorrowRewardId, int, required);
	member(MorrowActiveTime, DateTime, required){
		maxsize(24);
	};
	member(IsMorrowActive, bool, required);
	member(LevelupCostTime, double, required);
	member(IsChatForbidden, bool, required);
	member(GrowthFund, int, required);
	member(ChapterIdList, string, required){
		maxsize(24);
	};
	member(ChapterAwardList, string, required){
		maxsize(255);
	};
	member(LastResetConsumeCountTime, DateTime, required){
		maxsize(24);
	};
	member(DayRestSignCount, int, required);
};

message(TableUserSpecialInfo)
{
	member(Guid, long, required){
		primarykey;
	};
	member(Vigor, int, required);
	member(LastAddVigorTimestamp, double, required); 
	member(Stamina, int, required);  
	member(BuyStaminaCount, int, required);
	member(LastAddStaminaTimestamp, double, required); 
	member(UsedStamina, int, required);
	member(LastResetStaminaTime, DateTime, required){
		maxsize(24);
	};  
	member(CompleteSceneList, string, required){
		maxsize(255);
	};
	member(CompleteSceneNumber, string, required){
		maxsize(64);
	};
	member(LastResetSceneCountTime, DateTime, required){
		maxsize(24);
	};
	member(LastResetDailyMissionTime, DateTime, required){
		maxsize(24);
	};
	member(ActiveFashionId, int, required); 
	member(IsFashionShow, bool, required);
	member(ActiveWingId, int, required);
	member(IsWingShow, bool, required);
	member(ActiveWeaponId, int, required);
	member(IsWeaponShow, bool, required); 
	member(Vitality, int, required);
	member(LastAddVitalityTimestamp, double, required);
	member(LastResetExpeditionTime, DateTime, required){
		maxsize(24);
	};  
	member(CorpsGuid, long, required);
	member(LastQuitCorpsTime, DateTime, required){
		maxsize(24);
	};
	member(IsAcquireCorpsSignInPrize, bool, required);
	member(LastResetCorpsSignInPrizeTime, DateTime, required){
		maxsize(24);
	};
	member(CorpsChapterIdList, string, required){
		maxsize(24);
	};
	member(CorpsChapterDareList, string, required){
		maxsize(24);
	};
	member(LastResetSecretAreaTime, DateTime, required){
		maxsize(24);
	};
	member(RecentLoginState, long, required);
	member(SumLoginDayCount, int, required);
	member(UsedLoginLotteryDrawCount, int, required);
	member(LastSaveRecentLoginTime, DateTime, required){
		maxsize(24);
	};
	member(DiamondBoxList, string, required){
		maxsize(24);
	};
	member(LastResetMpveAwardTime, DateTime, required){
		maxsize(24);
	};
	member(FinishedActivityList, string, required){
		maxsize(255);
	}; 
};

message(TableUserBattleInfo)
{
	member(Guid, long, required){
		primarykey;
	};
	member(SceneId, int, required);
	member(StartTime, long, required);
	member(SumGold, int, required);
	member(Exp, int, required);
	member(RewardItemId, int, required);
	member(RewardItemCount, int, required);
	member(DeadCount, int, required);
	member(ReliveCount, int, required);
	member(IsClearing, bool, required);
	member(MatchKey, int, required);
	member(PartnerFinishedCount, int, required);
	member(PartnerRemainCount, int, required);
	member(PartnerBuyCount, int, required);
	member(PartnerList, string, required){
		maxsize(48);
	};
	member(PartnerSelectIndex, int, required); 
	member(PartnerLastResetTime, DateTime, required){
		maxsize(24);
	};
	member(DungeonQueryCount, int, required);
	member(DungeonLeftFightCount, int, required);
	member(DungeonBuyFightCount, int, required);
	member(DungeonMatchTargetList, string, required){
		maxsize(64);
	};
	member(DungeonMatchDropList, string, required){
		maxsize(48);
	};
	member(DungeonLastResetTime, DateTime, required){
		maxsize(24);
	};  
	member(SecretCurrentFight, int, required);
	member(SecretHpRateList, string, required){
		maxsize(24);
	};
	member(SecretMpRateList, string, required){
		maxsize(24);
	};
	member(SecretSegmentList, string, required){
		maxsize(24);
	};
	member(SecretFightCountList, string, required){
		maxsize(24);
	};  
};

message(TableEquipInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
  member(UserGuid, long, required){
	  foreignkey;
	};
	member(ItemGuid, long, required);
	member(Position, int, required);
	member(ItemId, int, required);
	member(Level, int, required); 
	member(AppendProperty, int, required);
	member(EnhanceStarLevel, int, required);
	member(StrengthLevel, int, required);
	member(FailCount, int, required);
};

message(TableFashionInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
  member(UserGuid, long, required){
	  foreignkey;
	};
	member(ItemGuid, long, required);
	member(Position, int, required);
	member(ItemId, int, required);
	member(IsForever, bool, required);
	member(ExpirationTime, DateTime, required){
		maxsize(24);
	};
	member(LastNoticeTime, DateTime, required){
		maxsize(24);
	};
};

message(TableItemInfo)
{
	member(ItemGuid, ulong, required){
		maxsize(24);
		primarykey;
	};	
	member(UserGuid, ulong, required){
	  foreignkey;
	};	
	member(ItemId, int, required);
	member(ItemNum, int, required);
	member(Level, int, required); 
	member(Experience, int, required);
	member(AppendProperty, int, required);
	member(EnhanceStarLevel, int, required);
};

message(TableTalentInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};  
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(ItemGuid, long, required);
	member(Position, int, required);
	member(ItemId, int, required);
	member(Level, int, required); 
	member(Experience, int, required);
};

message(TableLegacyInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
  member(UserGuid, long, required){
	  foreignkey;
	};
	member(Position, int, required);
	member(LegacyId, int, required);
	member(LegacyLevel, int, required); 
	member(AppendProperty, int, required);
	member(IsUnlock, bool, required);
};

message(TableXSoulInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(Position, int, required);
	member(XSoulType, int, required);
	member(XSoulId, int, required);
	member(XSoulLevel, int, required);  
	member(XSoulExp, int, required);
	member(XSoulModelLevel, int, required);
};

message(TableSkillInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(SkillId, int, required);
	member(Level, int, required);
	member(Preset, int, required);
};

message(TableMissionInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(MissionId, int, required);
	member(MissionValue, int, required);  
	member(MissionState, int, required);
};

message(TableLevelInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(LevelId, int, required);
	member(LevelRecord, int, required);
	member(ResetEliteCount, int, required);
	member(SceneDataBytes, "byte[]", required){
		maxsize(1024);
	};
};

message(TableExpeditionInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(FightingScore, int, required);
	member(HP, int, required);
	member(MP, int, required);
	member(Rage, int, required);
	member(Schedule, int, required);
	member(MonsterCount, int, required);
	member(BossCount, int, required);
	member(OnePlayerCount, int, required);
	member(Unrewarded, string, required){
		maxsize(64);
	};  
	member(TollgateType, int, required);  
	member(EnemyList, string, required){
		maxsize(255);
	};  
	member(EnemyAttrList, string, required){
		maxsize(255);
	};  
	member(ImageA, "byte[]", required){
		maxsize(8192);
	};
	member(ImageB, "byte[]", required){
		maxsize(8192);
	};   
	member(ResetCount, int, required); 
	member(PartnerIdList, string, required){
		maxsize(32);
	};
	member(PartnerHpList, string, required){
		maxsize(24);
	};
	member(LastAchievedSchedule, int, required);  
};

message(TableGowInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(GowElo, int, required);
	member(GowMatches, int, required);
	member(GowWinMatches, int, required);
	member(GowHistroyTimeList, string, required){
		maxsize(255);
	};
	member(GowHistroyEloList, string, required){
		maxsize(255);
	};
	member(RankId, int, required);
	member(Point, int, required);
	member(CriticalMatchCount, int, required);
	member(CriticalWinMatchCount, int, required);
	member(IsAcquirePrize, bool, required);
	member(LastTourneyDate, DateTime, required){
		maxsize(24);
	};
	member(LastResetGowPrizeTime, DateTime, required){
		maxsize(24);
	};
};

message(TableMailStateInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(MailGuid, long, required);
	member(IsRead, bool, required);
	member(IsReceived, bool, required);
	member(ExpiryDate, DateTime, required){
		maxsize(24);
	};
};

message(TablePartnerInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(PartnerId, int, required);
	member(AdditionLevel, int, required);
	member(SkillLevel, int, required);
	member(EquipList, string, required){
		maxsize(48);
	};
	member(ActiveOrder, int, required);
};

message(TableFriendInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(FriendGuid, long, required);
	member(FriendNickname, string, required){
		maxsize(32);
	};
	member(HeroId, int, required);
	member(Level, int, required);
	member(FightingScore, int, required);
	member(IsBlack, bool, required);
};

message(TablePaymentInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(OrderId, string, required){
		maxsize(48);
	};
	member(GoodsRegisterId, string, required){
		maxsize(48);
	};
	member(Diamond, int, required);
	member(PaymentTime, DateTime, required){
		maxsize(24);
	}; 
};

message(TableMpveAwardInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(MpveSceneId, int, required);
	member(DareCount, int, required);
	member(AwardCount, int, required);
	member(IsAwardedList, string, required){
		maxsize(64);
	};
	member(AwardIdList, string, required){
		maxsize(64);
	};
	member(DifficultyList, string, required){
		maxsize(24);
	};  
};

message(TableLotteryInfo)
{
	member(Guid, string, required){
		maxsize(24);
		primarykey;
	};
	member(UserGuid, long, required){
	  foreignkey;
	};
	member(LotteryId, int, required);
	member(IsFirstDraw, bool, required);
	member(FreeCount, int, required);
	member(LastDrawTime, DateTime, required){
		maxsize(24);
	};
	member(LastResetCountTime, DateTime, required){
		maxsize(24);
	};   
};
