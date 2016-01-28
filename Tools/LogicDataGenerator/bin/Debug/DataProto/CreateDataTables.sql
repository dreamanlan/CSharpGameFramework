#----------------------------------------------------------------------------
#！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
#----------------------------------------------------------------------------

call SetDSNodeVersion('0.3.11');

create table TableAccount
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	LogicServerId int not null,
	AccountId varchar(64) binary not null,
	IsBanned boolean not null,
	UserGuid1 bigint unsigned not null,
	UserGuid2 bigint unsigned not null,
	UserGuid3 bigint unsigned not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableAccountPrimaryIndex on TableAccount (LogicServerId,AccountId);

create table TableActivationCode
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	ActivationCode varchar(32) binary not null,
	IsActivated boolean not null,
	LogicServerId int not null,
	AccountId varchar(64) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableActivationCodePrimaryIndex on TableActivationCode (ActivationCode);

create table TableArenaInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	UserGuid bigint not null,
	LogicServerId int not null,
	Rank int not null,
	IsRobot boolean not null,
	ArenaBytes blob not null,
	LastBattleTime varchar(24) not null,
	LeftFightCount int not null,
	BuyFightCount int not null,
	LastResetFightCountTime varchar(24) not null,
	ArenaHistroyTimeList varchar(255) not null,
	ArenaHistroyRankList varchar(255) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableArenaInfoPrimaryIndex on TableArenaInfo (UserGuid);

create table TableArenaRecord
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	Rank int not null,
	IsChallengerSuccess boolean not null,
	BeginTime varchar(24) not null,
	EndTime varchar(24) not null,
	CGuid bigint not null,
	CHeroId int not null,
	CLevel int not null,
	CFightScore int not null,
	CNickname varchar(32) not null,
	CRank int not null,
	CUserDamage int not null,
	CPartnerId1 int not null,
	CPartnerDamage1 int not null,
	CPartnerId2 int not null,
	CPartnerDamage2 int not null,
	CPartnerId3 int not null,
	CPartnerDamage3 int not null,
	TGuid bigint not null,
	THeroId int not null,
	TLevel int not null,
	TFightScore int not null,
	TNickname varchar(32) not null,
	TRank int not null,
	TUserDamage int not null,
	TPartnerId1 int not null,
	TPartnerDamage1 int not null,
	TPartnerId2 int not null,
	TPartnerDamage2 int not null,
	TPartnerId3 int not null,
	TPartnerDamage3 int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableArenaRecordPrimaryIndex on TableArenaRecord (Guid);
create index TableArenaRecordIndex on  TableArenaRecord (UserGuid);

create table TableCorpsInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	CorpsGuid bigint not null,
	LogicServerId int not null,
	CorpsName varchar(32) not null,
	Level int not null,
	Score int not null,
	Rank int not null,
	Activeness int not null,
	Notice varchar(255) not null,
	LastResetActivenessTime varchar(24) not null,
	CreateTime varchar(24) not null,
	TollgateBytes blob not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableCorpsInfoPrimaryIndex on TableCorpsInfo (CorpsGuid);

create table TableCorpsMember
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	UserGuid bigint not null,
	CorpsGuid bigint not null,
	Title int not null,
	Nickname varchar(32) not null,
	HeroId int not null,
	Level int not null,
	FightScore int not null,
	DayActiveness int not null,
	WeekActiveness int not null,
	ActivenessHistoryDate varchar(255) not null,
	ActivenessHistoryValue varchar(64) not null,
	LastLoginTime bigint not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableCorpsMemberPrimaryIndex on TableCorpsMember (UserGuid);
create index TableCorpsMemberIndex on  TableCorpsMember (CorpsGuid);

create table TableEquipInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	ItemGuid bigint not null,
	Position int not null,
	ItemId int not null,
	Level int not null,
	AppendProperty int not null,
	EnhanceStarLevel int not null,
	StrengthLevel int not null,
	FailCount int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableEquipInfoPrimaryIndex on TableEquipInfo (Guid);
create index TableEquipInfoIndex on  TableEquipInfo (UserGuid);

create table TableExpeditionInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	FightingScore int not null,
	HP int not null,
	MP int not null,
	Rage int not null,
	Schedule int not null,
	MonsterCount int not null,
	BossCount int not null,
	OnePlayerCount int not null,
	Unrewarded varchar(64) not null,
	TollgateType int not null,
	EnemyList varchar(255) not null,
	EnemyAttrList varchar(255) not null,
	ImageA blob not null,
	ImageB blob not null,
	ResetCount int not null,
	PartnerIdList varchar(32) not null,
	PartnerHpList varchar(24) not null,
	LastAchievedSchedule int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableExpeditionInfoPrimaryIndex on TableExpeditionInfo (Guid);
create index TableExpeditionInfoIndex on  TableExpeditionInfo (UserGuid);

create table TableFashionInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	ItemGuid bigint not null,
	Position int not null,
	ItemId int not null,
	IsForever boolean not null,
	ExpirationTime varchar(24) not null,
	LastNoticeTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableFashionInfoPrimaryIndex on TableFashionInfo (Guid);
create index TableFashionInfoIndex on  TableFashionInfo (UserGuid);

create table TableFightInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(16) binary not null,
	LogicServerId int not null,
	Rank int not null,
	UserGuid bigint not null,
	HeroId int not null,
	Nickname varchar(32) not null,
	Level int not null,
	FightingScore int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableFightInfoPrimaryIndex on TableFightInfo (Guid);

create table TableFriendInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	FriendGuid bigint not null,
	FriendNickname varchar(32) not null,
	HeroId int not null,
	Level int not null,
	FightingScore int not null,
	IsBlack boolean not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableFriendInfoPrimaryIndex on TableFriendInfo (Guid);
create index TableFriendInfoIndex on  TableFriendInfo (UserGuid);

create table TableGlobalParam
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	ParamType varchar(32) binary not null,
	ParamValue varchar(64) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableGlobalParamPrimaryIndex on TableGlobalParam (ParamType);

create table TableGowInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	GowElo int not null,
	GowMatches int not null,
	GowWinMatches int not null,
	GowHistroyTimeList varchar(255) not null,
	GowHistroyEloList varchar(255) not null,
	RankId int not null,
	Point int not null,
	CriticalMatchCount int not null,
	CriticalWinMatchCount int not null,
	IsAcquirePrize boolean not null,
	LastTourneyDate varchar(24) not null,
	LastResetGowPrizeTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableGowInfoPrimaryIndex on TableGowInfo (Guid);
create index TableGowInfoIndex on  TableGowInfo (UserGuid);

create table TableGowStar
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(16) binary not null,
	LogicServerId int not null,
	Rank int not null,
	UserGuid bigint not null,
	Nickname varchar(32) not null,
	HeroId int not null,
	Level int not null,
	FightingScore int not null,
	GowElo int not null,
	RankId int not null,
	Point int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableGowStarPrimaryIndex on TableGowStar (Guid);

create table TableGuid
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	GuidType varchar(24) binary not null,
	GuidValue bigint unsigned not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableGuidPrimaryIndex on TableGuid (GuidType);

create table TableHomeNotice
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	LogicServerId int not null,
	Content varchar(2048) not null,
	CreateTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableHomeNoticePrimaryIndex on TableHomeNotice (LogicServerId);

create table TableInviterInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	UserGuid bigint not null,
	InviteCode varchar(24) not null,
	InviterGuid bigint not null,
	InviterLevel int not null,
	RewardedList varchar(24) not null,
	InviteeGuidList varchar(1536) not null,
	InviteeLevelList varchar(512) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableInviterInfoPrimaryIndex on TableInviterInfo (UserGuid);

create table TableItemInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	ItemGuid bigint unsigned not null,
	UserGuid bigint unsigned not null,
	ItemId int not null,
	ItemNum int not null,
	Level int not null,
	Experience int not null,
	AppendProperty int not null,
	EnhanceStarLevel int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableItemInfoPrimaryIndex on TableItemInfo (ItemGuid);
create index TableItemInfoIndex on  TableItemInfo (UserGuid);

create table TableLegacyInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	Position int not null,
	LegacyId int not null,
	LegacyLevel int not null,
	AppendProperty int not null,
	IsUnlock boolean not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLegacyInfoPrimaryIndex on TableLegacyInfo (Guid);
create index TableLegacyInfoIndex on  TableLegacyInfo (UserGuid);

create table TableLevelInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	LevelId int not null,
	LevelRecord int not null,
	ResetEliteCount int not null,
	SceneDataBytes blob not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLevelInfoPrimaryIndex on TableLevelInfo (Guid);
create index TableLevelInfoIndex on  TableLevelInfo (UserGuid);

create table TableLoginLotteryRecord
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	RecordId bigint not null,
	AccountId varchar(64) not null,
	UserGuid bigint not null,
	Nickname varchar(32) not null,
	RewardId int not null,
	CreateTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLoginLotteryRecordPrimaryIndex on TableLoginLotteryRecord (RecordId);

create table TableLootInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	UserGuid bigint not null,
	IsPool boolean not null,
	IsVisible boolean not null,
	LootKey bigint not null,
	Nickname varchar(32) not null,
	HeroId int not null,
	Level int not null,
	FightScore int not null,
	LastBattleTime varchar(24) not null,
	DomainType int not null,
	BeginTime varchar(24) not null,
	IsOpen boolean not null,
	IsGetAward boolean not null,
	LootAward float not null,
	TargetLootKey bigint not null,
	FightOrderList varchar(64) not null,
	LootOrderList varchar(64) not null,
	LootIncome int not null,
	LootLoss int not null,
	LootType int not null,
	LootBytes blob not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLootInfoPrimaryIndex on TableLootInfo (UserGuid);

create table TableLootRecord
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	IsPool boolean not null,
	IsLootSuccess boolean not null,
	LootBeginTime varchar(24) not null,
	LootEndTime varchar(24) not null,
	DomainType int not null,
	Booty int not null,
	LGuid bigint not null,
	LHeroId int not null,
	LLevel int not null,
	LFightScore int not null,
	LNickname varchar(32) not null,
	LUserDamage int not null,
	LDefenseOrderList varchar(64) not null,
	LLootOrderList varchar(64) not null,
	DGuid bigint not null,
	DHeroId int not null,
	DLevel int not null,
	DFightScore int not null,
	DNickname varchar(32) not null,
	DUserDamage int not null,
	DDefenseOrderList varchar(64) not null,
	DLootOrderList varchar(64) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLootRecordPrimaryIndex on TableLootRecord (Guid);
create index TableLootRecordIndex on  TableLootRecord (UserGuid);

create table TableLotteryInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	LotteryId int not null,
	IsFirstDraw boolean not null,
	FreeCount int not null,
	LastDrawTime varchar(24) not null,
	LastResetCountTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableLotteryInfoPrimaryIndex on TableLotteryInfo (Guid);
create index TableLotteryInfoIndex on  TableLotteryInfo (UserGuid);

create table TableMailInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint not null,
	LogicServerId int not null,
	ModuleTypeId int not null,
	Sender varchar(32) not null,
	Receiver bigint not null,
	SendDate varchar(24) not null,
	ExpiryDate varchar(24) not null,
	Title varchar(64) not null,
	Text varchar(1024) not null,
	Money int not null,
	Gold int not null,
	Stamina int not null,
	ItemIds varchar(128) not null,
	ItemNumbers varchar(64) not null,
	LevelDemand int not null,
	IsRead boolean not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableMailInfoPrimaryIndex on TableMailInfo (Guid);

create table TableMailStateInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	MailGuid bigint not null,
	IsRead boolean not null,
	IsReceived boolean not null,
	ExpiryDate varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableMailStateInfoPrimaryIndex on TableMailStateInfo (Guid);
create index TableMailStateInfoIndex on  TableMailStateInfo (UserGuid);

create table TableMissionInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	MissionId int not null,
	MissionValue int not null,
	MissionState int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableMissionInfoPrimaryIndex on TableMissionInfo (Guid);
create index TableMissionInfoIndex on  TableMissionInfo (UserGuid);

create table TableMpveAwardInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	MpveSceneId int not null,
	DareCount int not null,
	AwardCount int not null,
	IsAwardedList varchar(64) not null,
	AwardIdList varchar(64) not null,
	DifficultyList varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableMpveAwardInfoPrimaryIndex on TableMpveAwardInfo (Guid);
create index TableMpveAwardInfoIndex on  TableMpveAwardInfo (UserGuid);

create table TableNickname
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Nickname varchar(32) binary not null,
	UserGuid bigint unsigned not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableNicknamePrimaryIndex on TableNickname (Nickname);

create table TablePartnerInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	PartnerId int not null,
	AdditionLevel int not null,
	SkillLevel int not null,
	EquipList varchar(48) not null,
	ActiveOrder int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TablePartnerInfoPrimaryIndex on TablePartnerInfo (Guid);
create index TablePartnerInfoIndex on  TablePartnerInfo (UserGuid);

create table TablePaymentInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	OrderId varchar(48) not null,
	GoodsRegisterId varchar(48) not null,
	Diamond int not null,
	PaymentTime varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TablePaymentInfoPrimaryIndex on TablePaymentInfo (Guid);
create index TablePaymentInfoIndex on  TablePaymentInfo (UserGuid);

create table TableSkillInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	SkillId int not null,
	Level int not null,
	Preset int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableSkillInfoPrimaryIndex on TableSkillInfo (Guid);
create index TableSkillInfoIndex on  TableSkillInfo (UserGuid);

create table TableTalentInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	ItemGuid bigint not null,
	Position int not null,
	ItemId int not null,
	Level int not null,
	Experience int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableTalentInfoPrimaryIndex on TableTalentInfo (Guid);
create index TableTalentInfoIndex on  TableTalentInfo (UserGuid);

create table TableUndonePayment
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	OrderId varchar(48) binary not null,
	GoodsRegisterId varchar(48) not null,
	GoodsNum int not null,
	GoodsPrice float not null,
	UserGuid bigint not null,
	ChannelId varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUndonePaymentPrimaryIndex on TableUndonePayment (OrderId);

create table TableUserBattleInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint not null,
	SceneId int not null,
	StartTime bigint not null,
	SumGold int not null,
	Exp int not null,
	RewardItemId int not null,
	RewardItemCount int not null,
	DeadCount int not null,
	ReliveCount int not null,
	IsClearing boolean not null,
	MatchKey int not null,
	PartnerFinishedCount int not null,
	PartnerRemainCount int not null,
	PartnerBuyCount int not null,
	PartnerList varchar(48) not null,
	PartnerSelectIndex int not null,
	PartnerLastResetTime varchar(24) not null,
	DungeonQueryCount int not null,
	DungeonLeftFightCount int not null,
	DungeonBuyFightCount int not null,
	DungeonMatchTargetList varchar(64) not null,
	DungeonMatchDropList varchar(48) not null,
	DungeonLastResetTime varchar(24) not null,
	SecretCurrentFight int not null,
	SecretHpRateList varchar(24) not null,
	SecretMpRateList varchar(24) not null,
	SecretSegmentList varchar(24) not null,
	SecretFightCountList varchar(24) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUserBattleInfoPrimaryIndex on TableUserBattleInfo (Guid);

create table TableUserGeneralInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint not null,
	GuideFlag bigint not null,
	NewbieFlag varchar(255) not null,
	NewbieActionFlag varchar(255) not null,
	BuyMoneyCount int not null,
	LastBuyMoneyTimestamp double not null,
	LastResetMidasTouchTime varchar(24) not null,
	SellIncome int not null,
	LastSellTimestamp double not null,
	LastResetSellTime varchar(24) not null,
	ExchangeGoodList varchar(255) not null,
	ExchangeGoodNumber varchar(64) not null,
	ExchangeGoodRefreshCount varchar(255) not null,
	LastResetExchangeGoodTime varchar(24) not null,
	LastResetDaySignCountTime varchar(24) not null,
	MonthSignCount int not null,
	LastResetMonthSignCountTime varchar(24) not null,
	MonthCardExpireTime varchar(24) not null,
	IsWeeklyLoginRewarded boolean not null,
	WeeklyLoginRewardList varchar(64) not null,
	LastResetWeeklyLoginRewardTime varchar(24) not null,
	DailyOnlineDuration int not null,
	DailyOnlineRewardList varchar(48) not null,
	LastResetDailyOnlineTime varchar(24) not null,
	IsFirstPaymentRewarded boolean not null,
	VipRewaredList varchar(255) not null,
	MorrowRewardId int not null,
	MorrowActiveTime varchar(24) not null,
	IsMorrowActive boolean not null,
	LevelupCostTime double not null,
	IsChatForbidden boolean not null,
	GrowthFund int not null,
	ChapterIdList varchar(24) not null,
	ChapterAwardList varchar(255) not null,
	LastResetConsumeCountTime varchar(24) not null,
	DayRestSignCount int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUserGeneralInfoPrimaryIndex on TableUserGeneralInfo (Guid);

create table TableUserInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint unsigned not null,
	LogicServerId int not null,
	AccountId varchar(64) binary not null,
	Nickname varchar(32) not null,
	HeroId int not null,
	CreateTime varchar(24) not null,
	Level int not null,
	Money int not null,
	Gold int not null,
	ExpPoints int not null,
	CitySceneId int not null,
	Vip int not null,
	LastLogoutTime varchar(24) not null,
	AchievedScore int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUserInfoPrimaryIndex on TableUserInfo (Guid);
create index TableUserInfoIndex on  TableUserInfo (LogicServerId,AccountId);

create table TableUserSpecialInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint not null,
	Vigor int not null,
	LastAddVigorTimestamp double not null,
	Stamina int not null,
	BuyStaminaCount int not null,
	LastAddStaminaTimestamp double not null,
	UsedStamina int not null,
	LastResetStaminaTime varchar(24) not null,
	CompleteSceneList varchar(255) not null,
	CompleteSceneNumber varchar(64) not null,
	LastResetSceneCountTime varchar(24) not null,
	LastResetDailyMissionTime varchar(24) not null,
	ActiveFashionId int not null,
	IsFashionShow boolean not null,
	ActiveWingId int not null,
	IsWingShow boolean not null,
	ActiveWeaponId int not null,
	IsWeaponShow boolean not null,
	Vitality int not null,
	LastAddVitalityTimestamp double not null,
	LastResetExpeditionTime varchar(24) not null,
	CorpsGuid bigint not null,
	LastQuitCorpsTime varchar(24) not null,
	IsAcquireCorpsSignInPrize boolean not null,
	LastResetCorpsSignInPrizeTime varchar(24) not null,
	CorpsChapterIdList varchar(24) not null,
	CorpsChapterDareList varchar(24) not null,
	LastResetSecretAreaTime varchar(24) not null,
	RecentLoginState bigint not null,
	SumLoginDayCount int not null,
	UsedLoginLotteryDrawCount int not null,
	LastSaveRecentLoginTime varchar(24) not null,
	DiamondBoxList varchar(24) not null,
	LastResetMpveAwardTime varchar(24) not null,
	FinishedActivityList varchar(255) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUserSpecialInfoPrimaryIndex on TableUserSpecialInfo (Guid);

create table TableXSoulInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	Position int not null,
	XSoulType int not null,
	XSoulId int not null,
	XSoulLevel int not null,
	XSoulExp int not null,
	XSoulModelLevel int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableXSoulInfoPrimaryIndex on TableXSoulInfo (Guid);
create index TableXSoulInfoIndex on  TableXSoulInfo (UserGuid);


#----------------------------------------------------------------------------------------------------------------------

drop procedure if exists SaveTableAccount;
delimiter $$
create procedure SaveTableAccount(
	in _IsValid boolean
	,in _DataVersion int
	,in _LogicServerId int
	,in _AccountId varchar(64)
	,in _IsBanned boolean
	,in _UserGuid1 bigint unsigned
	,in _UserGuid2 bigint unsigned
	,in _UserGuid3 bigint unsigned
)
begin
	insert into TableAccount (AutoKey,IsValid,DataVersion,LogicServerId,AccountId,IsBanned,UserGuid1,UserGuid2,UserGuid3)
		values 
			(null,_IsValid,_DataVersion
			,_LogicServerId
			,_AccountId
			,_IsBanned
			,_UserGuid1
			,_UserGuid2
			,_UserGuid3
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			IsBanned =  if(DataVersion < _DataVersion, _IsBanned, IsBanned),
			UserGuid1 =  if(DataVersion < _DataVersion, _UserGuid1, UserGuid1),
			UserGuid2 =  if(DataVersion < _DataVersion, _UserGuid2, UserGuid2),
			UserGuid3 =  if(DataVersion < _DataVersion, _UserGuid3, UserGuid3),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableActivationCode;
delimiter $$
create procedure SaveTableActivationCode(
	in _IsValid boolean
	,in _DataVersion int
	,in _ActivationCode varchar(32)
	,in _IsActivated boolean
	,in _LogicServerId int
	,in _AccountId varchar(64)
)
begin
	insert into TableActivationCode (AutoKey,IsValid,DataVersion,ActivationCode,IsActivated,LogicServerId,AccountId)
		values 
			(null,_IsValid,_DataVersion
			,_ActivationCode
			,_IsActivated
			,_LogicServerId
			,_AccountId
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			ActivationCode =  if(DataVersion < _DataVersion, _ActivationCode, ActivationCode),
			IsActivated =  if(DataVersion < _DataVersion, _IsActivated, IsActivated),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableArenaInfo;
delimiter $$
create procedure SaveTableArenaInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _UserGuid bigint
	,in _LogicServerId int
	,in _Rank int
	,in _IsRobot boolean
	,in _ArenaBytes blob
	,in _LastBattleTime varchar(24)
	,in _LeftFightCount int
	,in _BuyFightCount int
	,in _LastResetFightCountTime varchar(24)
	,in _ArenaHistroyTimeList varchar(255)
	,in _ArenaHistroyRankList varchar(255)
)
begin
	insert into TableArenaInfo (AutoKey,IsValid,DataVersion,UserGuid,LogicServerId,Rank,IsRobot,ArenaBytes,LastBattleTime,LeftFightCount,BuyFightCount,LastResetFightCountTime,ArenaHistroyTimeList,ArenaHistroyRankList)
		values 
			(null,_IsValid,_DataVersion
			,_UserGuid
			,_LogicServerId
			,_Rank
			,_IsRobot
			,_ArenaBytes
			,_LastBattleTime
			,_LeftFightCount
			,_BuyFightCount
			,_LastResetFightCountTime
			,_ArenaHistroyTimeList
			,_ArenaHistroyRankList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			Rank =  if(DataVersion < _DataVersion, _Rank, Rank),
			IsRobot =  if(DataVersion < _DataVersion, _IsRobot, IsRobot),
			ArenaBytes =  if(DataVersion < _DataVersion, _ArenaBytes, ArenaBytes),
			LastBattleTime =  if(DataVersion < _DataVersion, _LastBattleTime, LastBattleTime),
			LeftFightCount =  if(DataVersion < _DataVersion, _LeftFightCount, LeftFightCount),
			BuyFightCount =  if(DataVersion < _DataVersion, _BuyFightCount, BuyFightCount),
			LastResetFightCountTime =  if(DataVersion < _DataVersion, _LastResetFightCountTime, LastResetFightCountTime),
			ArenaHistroyTimeList =  if(DataVersion < _DataVersion, _ArenaHistroyTimeList, ArenaHistroyTimeList),
			ArenaHistroyRankList =  if(DataVersion < _DataVersion, _ArenaHistroyRankList, ArenaHistroyRankList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableArenaRecord;
delimiter $$
create procedure SaveTableArenaRecord(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _Rank int
	,in _IsChallengerSuccess boolean
	,in _BeginTime varchar(24)
	,in _EndTime varchar(24)
	,in _CGuid bigint
	,in _CHeroId int
	,in _CLevel int
	,in _CFightScore int
	,in _CNickname varchar(32)
	,in _CRank int
	,in _CUserDamage int
	,in _CPartnerId1 int
	,in _CPartnerDamage1 int
	,in _CPartnerId2 int
	,in _CPartnerDamage2 int
	,in _CPartnerId3 int
	,in _CPartnerDamage3 int
	,in _TGuid bigint
	,in _THeroId int
	,in _TLevel int
	,in _TFightScore int
	,in _TNickname varchar(32)
	,in _TRank int
	,in _TUserDamage int
	,in _TPartnerId1 int
	,in _TPartnerDamage1 int
	,in _TPartnerId2 int
	,in _TPartnerDamage2 int
	,in _TPartnerId3 int
	,in _TPartnerDamage3 int
)
begin
	insert into TableArenaRecord (AutoKey,IsValid,DataVersion,Guid,UserGuid,Rank,IsChallengerSuccess,BeginTime,EndTime,CGuid,CHeroId,CLevel,CFightScore,CNickname,CRank,CUserDamage,CPartnerId1,CPartnerDamage1,CPartnerId2,CPartnerDamage2,CPartnerId3,CPartnerDamage3,TGuid,THeroId,TLevel,TFightScore,TNickname,TRank,TUserDamage,TPartnerId1,TPartnerDamage1,TPartnerId2,TPartnerDamage2,TPartnerId3,TPartnerDamage3)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_Rank
			,_IsChallengerSuccess
			,_BeginTime
			,_EndTime
			,_CGuid
			,_CHeroId
			,_CLevel
			,_CFightScore
			,_CNickname
			,_CRank
			,_CUserDamage
			,_CPartnerId1
			,_CPartnerDamage1
			,_CPartnerId2
			,_CPartnerDamage2
			,_CPartnerId3
			,_CPartnerDamage3
			,_TGuid
			,_THeroId
			,_TLevel
			,_TFightScore
			,_TNickname
			,_TRank
			,_TUserDamage
			,_TPartnerId1
			,_TPartnerDamage1
			,_TPartnerId2
			,_TPartnerDamage2
			,_TPartnerId3
			,_TPartnerDamage3
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			Rank =  if(DataVersion < _DataVersion, _Rank, Rank),
			IsChallengerSuccess =  if(DataVersion < _DataVersion, _IsChallengerSuccess, IsChallengerSuccess),
			BeginTime =  if(DataVersion < _DataVersion, _BeginTime, BeginTime),
			EndTime =  if(DataVersion < _DataVersion, _EndTime, EndTime),
			CGuid =  if(DataVersion < _DataVersion, _CGuid, CGuid),
			CHeroId =  if(DataVersion < _DataVersion, _CHeroId, CHeroId),
			CLevel =  if(DataVersion < _DataVersion, _CLevel, CLevel),
			CFightScore =  if(DataVersion < _DataVersion, _CFightScore, CFightScore),
			CNickname =  if(DataVersion < _DataVersion, _CNickname, CNickname),
			CRank =  if(DataVersion < _DataVersion, _CRank, CRank),
			CUserDamage =  if(DataVersion < _DataVersion, _CUserDamage, CUserDamage),
			CPartnerId1 =  if(DataVersion < _DataVersion, _CPartnerId1, CPartnerId1),
			CPartnerDamage1 =  if(DataVersion < _DataVersion, _CPartnerDamage1, CPartnerDamage1),
			CPartnerId2 =  if(DataVersion < _DataVersion, _CPartnerId2, CPartnerId2),
			CPartnerDamage2 =  if(DataVersion < _DataVersion, _CPartnerDamage2, CPartnerDamage2),
			CPartnerId3 =  if(DataVersion < _DataVersion, _CPartnerId3, CPartnerId3),
			CPartnerDamage3 =  if(DataVersion < _DataVersion, _CPartnerDamage3, CPartnerDamage3),
			TGuid =  if(DataVersion < _DataVersion, _TGuid, TGuid),
			THeroId =  if(DataVersion < _DataVersion, _THeroId, THeroId),
			TLevel =  if(DataVersion < _DataVersion, _TLevel, TLevel),
			TFightScore =  if(DataVersion < _DataVersion, _TFightScore, TFightScore),
			TNickname =  if(DataVersion < _DataVersion, _TNickname, TNickname),
			TRank =  if(DataVersion < _DataVersion, _TRank, TRank),
			TUserDamage =  if(DataVersion < _DataVersion, _TUserDamage, TUserDamage),
			TPartnerId1 =  if(DataVersion < _DataVersion, _TPartnerId1, TPartnerId1),
			TPartnerDamage1 =  if(DataVersion < _DataVersion, _TPartnerDamage1, TPartnerDamage1),
			TPartnerId2 =  if(DataVersion < _DataVersion, _TPartnerId2, TPartnerId2),
			TPartnerDamage2 =  if(DataVersion < _DataVersion, _TPartnerDamage2, TPartnerDamage2),
			TPartnerId3 =  if(DataVersion < _DataVersion, _TPartnerId3, TPartnerId3),
			TPartnerDamage3 =  if(DataVersion < _DataVersion, _TPartnerDamage3, TPartnerDamage3),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableCorpsInfo;
delimiter $$
create procedure SaveTableCorpsInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _CorpsGuid bigint
	,in _LogicServerId int
	,in _CorpsName varchar(32)
	,in _Level int
	,in _Score int
	,in _Rank int
	,in _Activeness int
	,in _Notice varchar(255)
	,in _LastResetActivenessTime varchar(24)
	,in _CreateTime varchar(24)
	,in _TollgateBytes blob
)
begin
	insert into TableCorpsInfo (AutoKey,IsValid,DataVersion,CorpsGuid,LogicServerId,CorpsName,Level,Score,Rank,Activeness,Notice,LastResetActivenessTime,CreateTime,TollgateBytes)
		values 
			(null,_IsValid,_DataVersion
			,_CorpsGuid
			,_LogicServerId
			,_CorpsName
			,_Level
			,_Score
			,_Rank
			,_Activeness
			,_Notice
			,_LastResetActivenessTime
			,_CreateTime
			,_TollgateBytes
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			CorpsGuid =  if(DataVersion < _DataVersion, _CorpsGuid, CorpsGuid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			CorpsName =  if(DataVersion < _DataVersion, _CorpsName, CorpsName),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			Score =  if(DataVersion < _DataVersion, _Score, Score),
			Rank =  if(DataVersion < _DataVersion, _Rank, Rank),
			Activeness =  if(DataVersion < _DataVersion, _Activeness, Activeness),
			Notice =  if(DataVersion < _DataVersion, _Notice, Notice),
			LastResetActivenessTime =  if(DataVersion < _DataVersion, _LastResetActivenessTime, LastResetActivenessTime),
			CreateTime =  if(DataVersion < _DataVersion, _CreateTime, CreateTime),
			TollgateBytes =  if(DataVersion < _DataVersion, _TollgateBytes, TollgateBytes),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableCorpsMember;
delimiter $$
create procedure SaveTableCorpsMember(
	in _IsValid boolean
	,in _DataVersion int
	,in _UserGuid bigint
	,in _CorpsGuid bigint
	,in _Title int
	,in _Nickname varchar(32)
	,in _HeroId int
	,in _Level int
	,in _FightScore int
	,in _DayActiveness int
	,in _WeekActiveness int
	,in _ActivenessHistoryDate varchar(255)
	,in _ActivenessHistoryValue varchar(64)
	,in _LastLoginTime bigint
)
begin
	insert into TableCorpsMember (AutoKey,IsValid,DataVersion,UserGuid,CorpsGuid,Title,Nickname,HeroId,Level,FightScore,DayActiveness,WeekActiveness,ActivenessHistoryDate,ActivenessHistoryValue,LastLoginTime)
		values 
			(null,_IsValid,_DataVersion
			,_UserGuid
			,_CorpsGuid
			,_Title
			,_Nickname
			,_HeroId
			,_Level
			,_FightScore
			,_DayActiveness
			,_WeekActiveness
			,_ActivenessHistoryDate
			,_ActivenessHistoryValue
			,_LastLoginTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			CorpsGuid =  if(DataVersion < _DataVersion, _CorpsGuid, CorpsGuid),
			Title =  if(DataVersion < _DataVersion, _Title, Title),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			FightScore =  if(DataVersion < _DataVersion, _FightScore, FightScore),
			DayActiveness =  if(DataVersion < _DataVersion, _DayActiveness, DayActiveness),
			WeekActiveness =  if(DataVersion < _DataVersion, _WeekActiveness, WeekActiveness),
			ActivenessHistoryDate =  if(DataVersion < _DataVersion, _ActivenessHistoryDate, ActivenessHistoryDate),
			ActivenessHistoryValue =  if(DataVersion < _DataVersion, _ActivenessHistoryValue, ActivenessHistoryValue),
			LastLoginTime =  if(DataVersion < _DataVersion, _LastLoginTime, LastLoginTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableEquipInfo;
delimiter $$
create procedure SaveTableEquipInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _ItemGuid bigint
	,in _Position int
	,in _ItemId int
	,in _Level int
	,in _AppendProperty int
	,in _EnhanceStarLevel int
	,in _StrengthLevel int
	,in _FailCount int
)
begin
	insert into TableEquipInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,ItemGuid,Position,ItemId,Level,AppendProperty,EnhanceStarLevel,StrengthLevel,FailCount)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_ItemGuid
			,_Position
			,_ItemId
			,_Level
			,_AppendProperty
			,_EnhanceStarLevel
			,_StrengthLevel
			,_FailCount
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ItemGuid =  if(DataVersion < _DataVersion, _ItemGuid, ItemGuid),
			Position =  if(DataVersion < _DataVersion, _Position, Position),
			ItemId =  if(DataVersion < _DataVersion, _ItemId, ItemId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			AppendProperty =  if(DataVersion < _DataVersion, _AppendProperty, AppendProperty),
			EnhanceStarLevel =  if(DataVersion < _DataVersion, _EnhanceStarLevel, EnhanceStarLevel),
			StrengthLevel =  if(DataVersion < _DataVersion, _StrengthLevel, StrengthLevel),
			FailCount =  if(DataVersion < _DataVersion, _FailCount, FailCount),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableExpeditionInfo;
delimiter $$
create procedure SaveTableExpeditionInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _FightingScore int
	,in _HP int
	,in _MP int
	,in _Rage int
	,in _Schedule int
	,in _MonsterCount int
	,in _BossCount int
	,in _OnePlayerCount int
	,in _Unrewarded varchar(64)
	,in _TollgateType int
	,in _EnemyList varchar(255)
	,in _EnemyAttrList varchar(255)
	,in _ImageA blob
	,in _ImageB blob
	,in _ResetCount int
	,in _PartnerIdList varchar(32)
	,in _PartnerHpList varchar(24)
	,in _LastAchievedSchedule int
)
begin
	insert into TableExpeditionInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,FightingScore,HP,MP,Rage,Schedule,MonsterCount,BossCount,OnePlayerCount,Unrewarded,TollgateType,EnemyList,EnemyAttrList,ImageA,ImageB,ResetCount,PartnerIdList,PartnerHpList,LastAchievedSchedule)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_FightingScore
			,_HP
			,_MP
			,_Rage
			,_Schedule
			,_MonsterCount
			,_BossCount
			,_OnePlayerCount
			,_Unrewarded
			,_TollgateType
			,_EnemyList
			,_EnemyAttrList
			,_ImageA
			,_ImageB
			,_ResetCount
			,_PartnerIdList
			,_PartnerHpList
			,_LastAchievedSchedule
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			FightingScore =  if(DataVersion < _DataVersion, _FightingScore, FightingScore),
			HP =  if(DataVersion < _DataVersion, _HP, HP),
			MP =  if(DataVersion < _DataVersion, _MP, MP),
			Rage =  if(DataVersion < _DataVersion, _Rage, Rage),
			Schedule =  if(DataVersion < _DataVersion, _Schedule, Schedule),
			MonsterCount =  if(DataVersion < _DataVersion, _MonsterCount, MonsterCount),
			BossCount =  if(DataVersion < _DataVersion, _BossCount, BossCount),
			OnePlayerCount =  if(DataVersion < _DataVersion, _OnePlayerCount, OnePlayerCount),
			Unrewarded =  if(DataVersion < _DataVersion, _Unrewarded, Unrewarded),
			TollgateType =  if(DataVersion < _DataVersion, _TollgateType, TollgateType),
			EnemyList =  if(DataVersion < _DataVersion, _EnemyList, EnemyList),
			EnemyAttrList =  if(DataVersion < _DataVersion, _EnemyAttrList, EnemyAttrList),
			ImageA =  if(DataVersion < _DataVersion, _ImageA, ImageA),
			ImageB =  if(DataVersion < _DataVersion, _ImageB, ImageB),
			ResetCount =  if(DataVersion < _DataVersion, _ResetCount, ResetCount),
			PartnerIdList =  if(DataVersion < _DataVersion, _PartnerIdList, PartnerIdList),
			PartnerHpList =  if(DataVersion < _DataVersion, _PartnerHpList, PartnerHpList),
			LastAchievedSchedule =  if(DataVersion < _DataVersion, _LastAchievedSchedule, LastAchievedSchedule),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableFashionInfo;
delimiter $$
create procedure SaveTableFashionInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _ItemGuid bigint
	,in _Position int
	,in _ItemId int
	,in _IsForever boolean
	,in _ExpirationTime varchar(24)
	,in _LastNoticeTime varchar(24)
)
begin
	insert into TableFashionInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,ItemGuid,Position,ItemId,IsForever,ExpirationTime,LastNoticeTime)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_ItemGuid
			,_Position
			,_ItemId
			,_IsForever
			,_ExpirationTime
			,_LastNoticeTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ItemGuid =  if(DataVersion < _DataVersion, _ItemGuid, ItemGuid),
			Position =  if(DataVersion < _DataVersion, _Position, Position),
			ItemId =  if(DataVersion < _DataVersion, _ItemId, ItemId),
			IsForever =  if(DataVersion < _DataVersion, _IsForever, IsForever),
			ExpirationTime =  if(DataVersion < _DataVersion, _ExpirationTime, ExpirationTime),
			LastNoticeTime =  if(DataVersion < _DataVersion, _LastNoticeTime, LastNoticeTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableFightInfo;
delimiter $$
create procedure SaveTableFightInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(16)
	,in _LogicServerId int
	,in _Rank int
	,in _UserGuid bigint
	,in _HeroId int
	,in _Nickname varchar(32)
	,in _Level int
	,in _FightingScore int
)
begin
	insert into TableFightInfo (AutoKey,IsValid,DataVersion,Guid,LogicServerId,Rank,UserGuid,HeroId,Nickname,Level,FightingScore)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_LogicServerId
			,_Rank
			,_UserGuid
			,_HeroId
			,_Nickname
			,_Level
			,_FightingScore
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			Rank =  if(DataVersion < _DataVersion, _Rank, Rank),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			FightingScore =  if(DataVersion < _DataVersion, _FightingScore, FightingScore),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableFriendInfo;
delimiter $$
create procedure SaveTableFriendInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _FriendGuid bigint
	,in _FriendNickname varchar(32)
	,in _HeroId int
	,in _Level int
	,in _FightingScore int
	,in _IsBlack boolean
)
begin
	insert into TableFriendInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,FriendGuid,FriendNickname,HeroId,Level,FightingScore,IsBlack)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_FriendGuid
			,_FriendNickname
			,_HeroId
			,_Level
			,_FightingScore
			,_IsBlack
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			FriendGuid =  if(DataVersion < _DataVersion, _FriendGuid, FriendGuid),
			FriendNickname =  if(DataVersion < _DataVersion, _FriendNickname, FriendNickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			FightingScore =  if(DataVersion < _DataVersion, _FightingScore, FightingScore),
			IsBlack =  if(DataVersion < _DataVersion, _IsBlack, IsBlack),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableGlobalParam;
delimiter $$
create procedure SaveTableGlobalParam(
	in _IsValid boolean
	,in _DataVersion int
	,in _ParamType varchar(32)
	,in _ParamValue varchar(64)
)
begin
	insert into TableGlobalParam (AutoKey,IsValid,DataVersion,ParamType,ParamValue)
		values 
			(null,_IsValid,_DataVersion
			,_ParamType
			,_ParamValue
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			ParamType =  if(DataVersion < _DataVersion, _ParamType, ParamType),
			ParamValue =  if(DataVersion < _DataVersion, _ParamValue, ParamValue),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableGowInfo;
delimiter $$
create procedure SaveTableGowInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _GowElo int
	,in _GowMatches int
	,in _GowWinMatches int
	,in _GowHistroyTimeList varchar(255)
	,in _GowHistroyEloList varchar(255)
	,in _RankId int
	,in _Point int
	,in _CriticalMatchCount int
	,in _CriticalWinMatchCount int
	,in _IsAcquirePrize boolean
	,in _LastTourneyDate varchar(24)
	,in _LastResetGowPrizeTime varchar(24)
)
begin
	insert into TableGowInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,GowElo,GowMatches,GowWinMatches,GowHistroyTimeList,GowHistroyEloList,RankId,Point,CriticalMatchCount,CriticalWinMatchCount,IsAcquirePrize,LastTourneyDate,LastResetGowPrizeTime)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_GowElo
			,_GowMatches
			,_GowWinMatches
			,_GowHistroyTimeList
			,_GowHistroyEloList
			,_RankId
			,_Point
			,_CriticalMatchCount
			,_CriticalWinMatchCount
			,_IsAcquirePrize
			,_LastTourneyDate
			,_LastResetGowPrizeTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			GowElo =  if(DataVersion < _DataVersion, _GowElo, GowElo),
			GowMatches =  if(DataVersion < _DataVersion, _GowMatches, GowMatches),
			GowWinMatches =  if(DataVersion < _DataVersion, _GowWinMatches, GowWinMatches),
			GowHistroyTimeList =  if(DataVersion < _DataVersion, _GowHistroyTimeList, GowHistroyTimeList),
			GowHistroyEloList =  if(DataVersion < _DataVersion, _GowHistroyEloList, GowHistroyEloList),
			RankId =  if(DataVersion < _DataVersion, _RankId, RankId),
			Point =  if(DataVersion < _DataVersion, _Point, Point),
			CriticalMatchCount =  if(DataVersion < _DataVersion, _CriticalMatchCount, CriticalMatchCount),
			CriticalWinMatchCount =  if(DataVersion < _DataVersion, _CriticalWinMatchCount, CriticalWinMatchCount),
			IsAcquirePrize =  if(DataVersion < _DataVersion, _IsAcquirePrize, IsAcquirePrize),
			LastTourneyDate =  if(DataVersion < _DataVersion, _LastTourneyDate, LastTourneyDate),
			LastResetGowPrizeTime =  if(DataVersion < _DataVersion, _LastResetGowPrizeTime, LastResetGowPrizeTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableGowStar;
delimiter $$
create procedure SaveTableGowStar(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(16)
	,in _LogicServerId int
	,in _Rank int
	,in _UserGuid bigint
	,in _Nickname varchar(32)
	,in _HeroId int
	,in _Level int
	,in _FightingScore int
	,in _GowElo int
	,in _RankId int
	,in _Point int
)
begin
	insert into TableGowStar (AutoKey,IsValid,DataVersion,Guid,LogicServerId,Rank,UserGuid,Nickname,HeroId,Level,FightingScore,GowElo,RankId,Point)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_LogicServerId
			,_Rank
			,_UserGuid
			,_Nickname
			,_HeroId
			,_Level
			,_FightingScore
			,_GowElo
			,_RankId
			,_Point
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			Rank =  if(DataVersion < _DataVersion, _Rank, Rank),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			FightingScore =  if(DataVersion < _DataVersion, _FightingScore, FightingScore),
			GowElo =  if(DataVersion < _DataVersion, _GowElo, GowElo),
			RankId =  if(DataVersion < _DataVersion, _RankId, RankId),
			Point =  if(DataVersion < _DataVersion, _Point, Point),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableGuid;
delimiter $$
create procedure SaveTableGuid(
	in _IsValid boolean
	,in _DataVersion int
	,in _GuidType varchar(24)
	,in _GuidValue bigint unsigned
)
begin
	insert into TableGuid (AutoKey,IsValid,DataVersion,GuidType,GuidValue)
		values 
			(null,_IsValid,_DataVersion
			,_GuidType
			,_GuidValue
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			GuidType =  if(DataVersion < _DataVersion, _GuidType, GuidType),
			GuidValue =  if(DataVersion < _DataVersion, _GuidValue, GuidValue),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableHomeNotice;
delimiter $$
create procedure SaveTableHomeNotice(
	in _IsValid boolean
	,in _DataVersion int
	,in _LogicServerId int
	,in _Content varchar(2048)
	,in _CreateTime varchar(24)
)
begin
	insert into TableHomeNotice (AutoKey,IsValid,DataVersion,LogicServerId,Content,CreateTime)
		values 
			(null,_IsValid,_DataVersion
			,_LogicServerId
			,_Content
			,_CreateTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			Content =  if(DataVersion < _DataVersion, _Content, Content),
			CreateTime =  if(DataVersion < _DataVersion, _CreateTime, CreateTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableInviterInfo;
delimiter $$
create procedure SaveTableInviterInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _UserGuid bigint
	,in _InviteCode varchar(24)
	,in _InviterGuid bigint
	,in _InviterLevel int
	,in _RewardedList varchar(24)
	,in _InviteeGuidList varchar(1536)
	,in _InviteeLevelList varchar(512)
)
begin
	insert into TableInviterInfo (AutoKey,IsValid,DataVersion,UserGuid,InviteCode,InviterGuid,InviterLevel,RewardedList,InviteeGuidList,InviteeLevelList)
		values 
			(null,_IsValid,_DataVersion
			,_UserGuid
			,_InviteCode
			,_InviterGuid
			,_InviterLevel
			,_RewardedList
			,_InviteeGuidList
			,_InviteeLevelList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			InviteCode =  if(DataVersion < _DataVersion, _InviteCode, InviteCode),
			InviterGuid =  if(DataVersion < _DataVersion, _InviterGuid, InviterGuid),
			InviterLevel =  if(DataVersion < _DataVersion, _InviterLevel, InviterLevel),
			RewardedList =  if(DataVersion < _DataVersion, _RewardedList, RewardedList),
			InviteeGuidList =  if(DataVersion < _DataVersion, _InviteeGuidList, InviteeGuidList),
			InviteeLevelList =  if(DataVersion < _DataVersion, _InviteeLevelList, InviteeLevelList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableItemInfo;
delimiter $$
create procedure SaveTableItemInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _ItemGuid bigint unsigned
	,in _UserGuid bigint unsigned
	,in _ItemId int
	,in _ItemNum int
	,in _Level int
	,in _Experience int
	,in _AppendProperty int
	,in _EnhanceStarLevel int
)
begin
	insert into TableItemInfo (AutoKey,IsValid,DataVersion,ItemGuid,UserGuid,ItemId,ItemNum,Level,Experience,AppendProperty,EnhanceStarLevel)
		values 
			(null,_IsValid,_DataVersion
			,_ItemGuid
			,_UserGuid
			,_ItemId
			,_ItemNum
			,_Level
			,_Experience
			,_AppendProperty
			,_EnhanceStarLevel
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			ItemGuid =  if(DataVersion < _DataVersion, _ItemGuid, ItemGuid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ItemId =  if(DataVersion < _DataVersion, _ItemId, ItemId),
			ItemNum =  if(DataVersion < _DataVersion, _ItemNum, ItemNum),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			Experience =  if(DataVersion < _DataVersion, _Experience, Experience),
			AppendProperty =  if(DataVersion < _DataVersion, _AppendProperty, AppendProperty),
			EnhanceStarLevel =  if(DataVersion < _DataVersion, _EnhanceStarLevel, EnhanceStarLevel),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLegacyInfo;
delimiter $$
create procedure SaveTableLegacyInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _Position int
	,in _LegacyId int
	,in _LegacyLevel int
	,in _AppendProperty int
	,in _IsUnlock boolean
)
begin
	insert into TableLegacyInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,Position,LegacyId,LegacyLevel,AppendProperty,IsUnlock)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_Position
			,_LegacyId
			,_LegacyLevel
			,_AppendProperty
			,_IsUnlock
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			Position =  if(DataVersion < _DataVersion, _Position, Position),
			LegacyId =  if(DataVersion < _DataVersion, _LegacyId, LegacyId),
			LegacyLevel =  if(DataVersion < _DataVersion, _LegacyLevel, LegacyLevel),
			AppendProperty =  if(DataVersion < _DataVersion, _AppendProperty, AppendProperty),
			IsUnlock =  if(DataVersion < _DataVersion, _IsUnlock, IsUnlock),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLevelInfo;
delimiter $$
create procedure SaveTableLevelInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _LevelId int
	,in _LevelRecord int
	,in _ResetEliteCount int
	,in _SceneDataBytes blob
)
begin
	insert into TableLevelInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,LevelId,LevelRecord,ResetEliteCount,SceneDataBytes)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_LevelId
			,_LevelRecord
			,_ResetEliteCount
			,_SceneDataBytes
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			LevelId =  if(DataVersion < _DataVersion, _LevelId, LevelId),
			LevelRecord =  if(DataVersion < _DataVersion, _LevelRecord, LevelRecord),
			ResetEliteCount =  if(DataVersion < _DataVersion, _ResetEliteCount, ResetEliteCount),
			SceneDataBytes =  if(DataVersion < _DataVersion, _SceneDataBytes, SceneDataBytes),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLoginLotteryRecord;
delimiter $$
create procedure SaveTableLoginLotteryRecord(
	in _IsValid boolean
	,in _DataVersion int
	,in _RecordId bigint
	,in _AccountId varchar(64)
	,in _UserGuid bigint
	,in _Nickname varchar(32)
	,in _RewardId int
	,in _CreateTime varchar(24)
)
begin
	insert into TableLoginLotteryRecord (AutoKey,IsValid,DataVersion,RecordId,AccountId,UserGuid,Nickname,RewardId,CreateTime)
		values 
			(null,_IsValid,_DataVersion
			,_RecordId
			,_AccountId
			,_UserGuid
			,_Nickname
			,_RewardId
			,_CreateTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			RecordId =  if(DataVersion < _DataVersion, _RecordId, RecordId),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			RewardId =  if(DataVersion < _DataVersion, _RewardId, RewardId),
			CreateTime =  if(DataVersion < _DataVersion, _CreateTime, CreateTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLootInfo;
delimiter $$
create procedure SaveTableLootInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _UserGuid bigint
	,in _IsPool boolean
	,in _IsVisible boolean
	,in _LootKey bigint
	,in _Nickname varchar(32)
	,in _HeroId int
	,in _Level int
	,in _FightScore int
	,in _LastBattleTime varchar(24)
	,in _DomainType int
	,in _BeginTime varchar(24)
	,in _IsOpen boolean
	,in _IsGetAward boolean
	,in _LootAward float
	,in _TargetLootKey bigint
	,in _FightOrderList varchar(64)
	,in _LootOrderList varchar(64)
	,in _LootIncome int
	,in _LootLoss int
	,in _LootType int
	,in _LootBytes blob
)
begin
	insert into TableLootInfo (AutoKey,IsValid,DataVersion,UserGuid,IsPool,IsVisible,LootKey,Nickname,HeroId,Level,FightScore,LastBattleTime,DomainType,BeginTime,IsOpen,IsGetAward,LootAward,TargetLootKey,FightOrderList,LootOrderList,LootIncome,LootLoss,LootType,LootBytes)
		values 
			(null,_IsValid,_DataVersion
			,_UserGuid
			,_IsPool
			,_IsVisible
			,_LootKey
			,_Nickname
			,_HeroId
			,_Level
			,_FightScore
			,_LastBattleTime
			,_DomainType
			,_BeginTime
			,_IsOpen
			,_IsGetAward
			,_LootAward
			,_TargetLootKey
			,_FightOrderList
			,_LootOrderList
			,_LootIncome
			,_LootLoss
			,_LootType
			,_LootBytes
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			IsPool =  if(DataVersion < _DataVersion, _IsPool, IsPool),
			IsVisible =  if(DataVersion < _DataVersion, _IsVisible, IsVisible),
			LootKey =  if(DataVersion < _DataVersion, _LootKey, LootKey),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			FightScore =  if(DataVersion < _DataVersion, _FightScore, FightScore),
			LastBattleTime =  if(DataVersion < _DataVersion, _LastBattleTime, LastBattleTime),
			DomainType =  if(DataVersion < _DataVersion, _DomainType, DomainType),
			BeginTime =  if(DataVersion < _DataVersion, _BeginTime, BeginTime),
			IsOpen =  if(DataVersion < _DataVersion, _IsOpen, IsOpen),
			IsGetAward =  if(DataVersion < _DataVersion, _IsGetAward, IsGetAward),
			LootAward =  if(DataVersion < _DataVersion, _LootAward, LootAward),
			TargetLootKey =  if(DataVersion < _DataVersion, _TargetLootKey, TargetLootKey),
			FightOrderList =  if(DataVersion < _DataVersion, _FightOrderList, FightOrderList),
			LootOrderList =  if(DataVersion < _DataVersion, _LootOrderList, LootOrderList),
			LootIncome =  if(DataVersion < _DataVersion, _LootIncome, LootIncome),
			LootLoss =  if(DataVersion < _DataVersion, _LootLoss, LootLoss),
			LootType =  if(DataVersion < _DataVersion, _LootType, LootType),
			LootBytes =  if(DataVersion < _DataVersion, _LootBytes, LootBytes),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLootRecord;
delimiter $$
create procedure SaveTableLootRecord(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _IsPool boolean
	,in _IsLootSuccess boolean
	,in _LootBeginTime varchar(24)
	,in _LootEndTime varchar(24)
	,in _DomainType int
	,in _Booty int
	,in _LGuid bigint
	,in _LHeroId int
	,in _LLevel int
	,in _LFightScore int
	,in _LNickname varchar(32)
	,in _LUserDamage int
	,in _LDefenseOrderList varchar(64)
	,in _LLootOrderList varchar(64)
	,in _DGuid bigint
	,in _DHeroId int
	,in _DLevel int
	,in _DFightScore int
	,in _DNickname varchar(32)
	,in _DUserDamage int
	,in _DDefenseOrderList varchar(64)
	,in _DLootOrderList varchar(64)
)
begin
	insert into TableLootRecord (AutoKey,IsValid,DataVersion,Guid,UserGuid,IsPool,IsLootSuccess,LootBeginTime,LootEndTime,DomainType,Booty,LGuid,LHeroId,LLevel,LFightScore,LNickname,LUserDamage,LDefenseOrderList,LLootOrderList,DGuid,DHeroId,DLevel,DFightScore,DNickname,DUserDamage,DDefenseOrderList,DLootOrderList)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_IsPool
			,_IsLootSuccess
			,_LootBeginTime
			,_LootEndTime
			,_DomainType
			,_Booty
			,_LGuid
			,_LHeroId
			,_LLevel
			,_LFightScore
			,_LNickname
			,_LUserDamage
			,_LDefenseOrderList
			,_LLootOrderList
			,_DGuid
			,_DHeroId
			,_DLevel
			,_DFightScore
			,_DNickname
			,_DUserDamage
			,_DDefenseOrderList
			,_DLootOrderList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			IsPool =  if(DataVersion < _DataVersion, _IsPool, IsPool),
			IsLootSuccess =  if(DataVersion < _DataVersion, _IsLootSuccess, IsLootSuccess),
			LootBeginTime =  if(DataVersion < _DataVersion, _LootBeginTime, LootBeginTime),
			LootEndTime =  if(DataVersion < _DataVersion, _LootEndTime, LootEndTime),
			DomainType =  if(DataVersion < _DataVersion, _DomainType, DomainType),
			Booty =  if(DataVersion < _DataVersion, _Booty, Booty),
			LGuid =  if(DataVersion < _DataVersion, _LGuid, LGuid),
			LHeroId =  if(DataVersion < _DataVersion, _LHeroId, LHeroId),
			LLevel =  if(DataVersion < _DataVersion, _LLevel, LLevel),
			LFightScore =  if(DataVersion < _DataVersion, _LFightScore, LFightScore),
			LNickname =  if(DataVersion < _DataVersion, _LNickname, LNickname),
			LUserDamage =  if(DataVersion < _DataVersion, _LUserDamage, LUserDamage),
			LDefenseOrderList =  if(DataVersion < _DataVersion, _LDefenseOrderList, LDefenseOrderList),
			LLootOrderList =  if(DataVersion < _DataVersion, _LLootOrderList, LLootOrderList),
			DGuid =  if(DataVersion < _DataVersion, _DGuid, DGuid),
			DHeroId =  if(DataVersion < _DataVersion, _DHeroId, DHeroId),
			DLevel =  if(DataVersion < _DataVersion, _DLevel, DLevel),
			DFightScore =  if(DataVersion < _DataVersion, _DFightScore, DFightScore),
			DNickname =  if(DataVersion < _DataVersion, _DNickname, DNickname),
			DUserDamage =  if(DataVersion < _DataVersion, _DUserDamage, DUserDamage),
			DDefenseOrderList =  if(DataVersion < _DataVersion, _DDefenseOrderList, DDefenseOrderList),
			DLootOrderList =  if(DataVersion < _DataVersion, _DLootOrderList, DLootOrderList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableLotteryInfo;
delimiter $$
create procedure SaveTableLotteryInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _LotteryId int
	,in _IsFirstDraw boolean
	,in _FreeCount int
	,in _LastDrawTime varchar(24)
	,in _LastResetCountTime varchar(24)
)
begin
	insert into TableLotteryInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,LotteryId,IsFirstDraw,FreeCount,LastDrawTime,LastResetCountTime)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_LotteryId
			,_IsFirstDraw
			,_FreeCount
			,_LastDrawTime
			,_LastResetCountTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			LotteryId =  if(DataVersion < _DataVersion, _LotteryId, LotteryId),
			IsFirstDraw =  if(DataVersion < _DataVersion, _IsFirstDraw, IsFirstDraw),
			FreeCount =  if(DataVersion < _DataVersion, _FreeCount, FreeCount),
			LastDrawTime =  if(DataVersion < _DataVersion, _LastDrawTime, LastDrawTime),
			LastResetCountTime =  if(DataVersion < _DataVersion, _LastResetCountTime, LastResetCountTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableMailInfo;
delimiter $$
create procedure SaveTableMailInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint
	,in _LogicServerId int
	,in _ModuleTypeId int
	,in _Sender varchar(32)
	,in _Receiver bigint
	,in _SendDate varchar(24)
	,in _ExpiryDate varchar(24)
	,in _Title varchar(64)
	,in _Text varchar(1024)
	,in _Money int
	,in _Gold int
	,in _Stamina int
	,in _ItemIds varchar(128)
	,in _ItemNumbers varchar(64)
	,in _LevelDemand int
	,in _IsRead boolean
)
begin
	insert into TableMailInfo (AutoKey,IsValid,DataVersion,Guid,LogicServerId,ModuleTypeId,Sender,Receiver,SendDate,ExpiryDate,Title,Text,Money,Gold,Stamina,ItemIds,ItemNumbers,LevelDemand,IsRead)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_LogicServerId
			,_ModuleTypeId
			,_Sender
			,_Receiver
			,_SendDate
			,_ExpiryDate
			,_Title
			,_Text
			,_Money
			,_Gold
			,_Stamina
			,_ItemIds
			,_ItemNumbers
			,_LevelDemand
			,_IsRead
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			ModuleTypeId =  if(DataVersion < _DataVersion, _ModuleTypeId, ModuleTypeId),
			Sender =  if(DataVersion < _DataVersion, _Sender, Sender),
			Receiver =  if(DataVersion < _DataVersion, _Receiver, Receiver),
			SendDate =  if(DataVersion < _DataVersion, _SendDate, SendDate),
			ExpiryDate =  if(DataVersion < _DataVersion, _ExpiryDate, ExpiryDate),
			Title =  if(DataVersion < _DataVersion, _Title, Title),
			Text =  if(DataVersion < _DataVersion, _Text, Text),
			Money =  if(DataVersion < _DataVersion, _Money, Money),
			Gold =  if(DataVersion < _DataVersion, _Gold, Gold),
			Stamina =  if(DataVersion < _DataVersion, _Stamina, Stamina),
			ItemIds =  if(DataVersion < _DataVersion, _ItemIds, ItemIds),
			ItemNumbers =  if(DataVersion < _DataVersion, _ItemNumbers, ItemNumbers),
			LevelDemand =  if(DataVersion < _DataVersion, _LevelDemand, LevelDemand),
			IsRead =  if(DataVersion < _DataVersion, _IsRead, IsRead),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableMailStateInfo;
delimiter $$
create procedure SaveTableMailStateInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _MailGuid bigint
	,in _IsRead boolean
	,in _IsReceived boolean
	,in _ExpiryDate varchar(24)
)
begin
	insert into TableMailStateInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,MailGuid,IsRead,IsReceived,ExpiryDate)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_MailGuid
			,_IsRead
			,_IsReceived
			,_ExpiryDate
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			MailGuid =  if(DataVersion < _DataVersion, _MailGuid, MailGuid),
			IsRead =  if(DataVersion < _DataVersion, _IsRead, IsRead),
			IsReceived =  if(DataVersion < _DataVersion, _IsReceived, IsReceived),
			ExpiryDate =  if(DataVersion < _DataVersion, _ExpiryDate, ExpiryDate),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableMissionInfo;
delimiter $$
create procedure SaveTableMissionInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _MissionId int
	,in _MissionValue int
	,in _MissionState int
)
begin
	insert into TableMissionInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,MissionId,MissionValue,MissionState)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_MissionId
			,_MissionValue
			,_MissionState
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			MissionId =  if(DataVersion < _DataVersion, _MissionId, MissionId),
			MissionValue =  if(DataVersion < _DataVersion, _MissionValue, MissionValue),
			MissionState =  if(DataVersion < _DataVersion, _MissionState, MissionState),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableMpveAwardInfo;
delimiter $$
create procedure SaveTableMpveAwardInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _MpveSceneId int
	,in _DareCount int
	,in _AwardCount int
	,in _IsAwardedList varchar(64)
	,in _AwardIdList varchar(64)
	,in _DifficultyList varchar(24)
)
begin
	insert into TableMpveAwardInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,MpveSceneId,DareCount,AwardCount,IsAwardedList,AwardIdList,DifficultyList)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_MpveSceneId
			,_DareCount
			,_AwardCount
			,_IsAwardedList
			,_AwardIdList
			,_DifficultyList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			MpveSceneId =  if(DataVersion < _DataVersion, _MpveSceneId, MpveSceneId),
			DareCount =  if(DataVersion < _DataVersion, _DareCount, DareCount),
			AwardCount =  if(DataVersion < _DataVersion, _AwardCount, AwardCount),
			IsAwardedList =  if(DataVersion < _DataVersion, _IsAwardedList, IsAwardedList),
			AwardIdList =  if(DataVersion < _DataVersion, _AwardIdList, AwardIdList),
			DifficultyList =  if(DataVersion < _DataVersion, _DifficultyList, DifficultyList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableNickname;
delimiter $$
create procedure SaveTableNickname(
	in _IsValid boolean
	,in _DataVersion int
	,in _Nickname varchar(32)
	,in _UserGuid bigint unsigned
)
begin
	insert into TableNickname (AutoKey,IsValid,DataVersion,Nickname,UserGuid)
		values 
			(null,_IsValid,_DataVersion
			,_Nickname
			,_UserGuid
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTablePartnerInfo;
delimiter $$
create procedure SaveTablePartnerInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _PartnerId int
	,in _AdditionLevel int
	,in _SkillLevel int
	,in _EquipList varchar(48)
	,in _ActiveOrder int
)
begin
	insert into TablePartnerInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,PartnerId,AdditionLevel,SkillLevel,EquipList,ActiveOrder)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_PartnerId
			,_AdditionLevel
			,_SkillLevel
			,_EquipList
			,_ActiveOrder
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			PartnerId =  if(DataVersion < _DataVersion, _PartnerId, PartnerId),
			AdditionLevel =  if(DataVersion < _DataVersion, _AdditionLevel, AdditionLevel),
			SkillLevel =  if(DataVersion < _DataVersion, _SkillLevel, SkillLevel),
			EquipList =  if(DataVersion < _DataVersion, _EquipList, EquipList),
			ActiveOrder =  if(DataVersion < _DataVersion, _ActiveOrder, ActiveOrder),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTablePaymentInfo;
delimiter $$
create procedure SaveTablePaymentInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _OrderId varchar(48)
	,in _GoodsRegisterId varchar(48)
	,in _Diamond int
	,in _PaymentTime varchar(24)
)
begin
	insert into TablePaymentInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,OrderId,GoodsRegisterId,Diamond,PaymentTime)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_OrderId
			,_GoodsRegisterId
			,_Diamond
			,_PaymentTime
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			OrderId =  if(DataVersion < _DataVersion, _OrderId, OrderId),
			GoodsRegisterId =  if(DataVersion < _DataVersion, _GoodsRegisterId, GoodsRegisterId),
			Diamond =  if(DataVersion < _DataVersion, _Diamond, Diamond),
			PaymentTime =  if(DataVersion < _DataVersion, _PaymentTime, PaymentTime),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableSkillInfo;
delimiter $$
create procedure SaveTableSkillInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _SkillId int
	,in _Level int
	,in _Preset int
)
begin
	insert into TableSkillInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,SkillId,Level,Preset)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_SkillId
			,_Level
			,_Preset
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			SkillId =  if(DataVersion < _DataVersion, _SkillId, SkillId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			Preset =  if(DataVersion < _DataVersion, _Preset, Preset),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableTalentInfo;
delimiter $$
create procedure SaveTableTalentInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _ItemGuid bigint
	,in _Position int
	,in _ItemId int
	,in _Level int
	,in _Experience int
)
begin
	insert into TableTalentInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,ItemGuid,Position,ItemId,Level,Experience)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_ItemGuid
			,_Position
			,_ItemId
			,_Level
			,_Experience
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ItemGuid =  if(DataVersion < _DataVersion, _ItemGuid, ItemGuid),
			Position =  if(DataVersion < _DataVersion, _Position, Position),
			ItemId =  if(DataVersion < _DataVersion, _ItemId, ItemId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			Experience =  if(DataVersion < _DataVersion, _Experience, Experience),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableUndonePayment;
delimiter $$
create procedure SaveTableUndonePayment(
	in _IsValid boolean
	,in _DataVersion int
	,in _OrderId varchar(48)
	,in _GoodsRegisterId varchar(48)
	,in _GoodsNum int
	,in _GoodsPrice float
	,in _UserGuid bigint
	,in _ChannelId varchar(24)
)
begin
	insert into TableUndonePayment (AutoKey,IsValid,DataVersion,OrderId,GoodsRegisterId,GoodsNum,GoodsPrice,UserGuid,ChannelId)
		values 
			(null,_IsValid,_DataVersion
			,_OrderId
			,_GoodsRegisterId
			,_GoodsNum
			,_GoodsPrice
			,_UserGuid
			,_ChannelId
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			OrderId =  if(DataVersion < _DataVersion, _OrderId, OrderId),
			GoodsRegisterId =  if(DataVersion < _DataVersion, _GoodsRegisterId, GoodsRegisterId),
			GoodsNum =  if(DataVersion < _DataVersion, _GoodsNum, GoodsNum),
			GoodsPrice =  if(DataVersion < _DataVersion, _GoodsPrice, GoodsPrice),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ChannelId =  if(DataVersion < _DataVersion, _ChannelId, ChannelId),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableUserBattleInfo;
delimiter $$
create procedure SaveTableUserBattleInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint
	,in _SceneId int
	,in _StartTime bigint
	,in _SumGold int
	,in _Exp int
	,in _RewardItemId int
	,in _RewardItemCount int
	,in _DeadCount int
	,in _ReliveCount int
	,in _IsClearing boolean
	,in _MatchKey int
	,in _PartnerFinishedCount int
	,in _PartnerRemainCount int
	,in _PartnerBuyCount int
	,in _PartnerList varchar(48)
	,in _PartnerSelectIndex int
	,in _PartnerLastResetTime varchar(24)
	,in _DungeonQueryCount int
	,in _DungeonLeftFightCount int
	,in _DungeonBuyFightCount int
	,in _DungeonMatchTargetList varchar(64)
	,in _DungeonMatchDropList varchar(48)
	,in _DungeonLastResetTime varchar(24)
	,in _SecretCurrentFight int
	,in _SecretHpRateList varchar(24)
	,in _SecretMpRateList varchar(24)
	,in _SecretSegmentList varchar(24)
	,in _SecretFightCountList varchar(24)
)
begin
	insert into TableUserBattleInfo (AutoKey,IsValid,DataVersion,Guid,SceneId,StartTime,SumGold,Exp,RewardItemId,RewardItemCount,DeadCount,ReliveCount,IsClearing,MatchKey,PartnerFinishedCount,PartnerRemainCount,PartnerBuyCount,PartnerList,PartnerSelectIndex,PartnerLastResetTime,DungeonQueryCount,DungeonLeftFightCount,DungeonBuyFightCount,DungeonMatchTargetList,DungeonMatchDropList,DungeonLastResetTime,SecretCurrentFight,SecretHpRateList,SecretMpRateList,SecretSegmentList,SecretFightCountList)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_SceneId
			,_StartTime
			,_SumGold
			,_Exp
			,_RewardItemId
			,_RewardItemCount
			,_DeadCount
			,_ReliveCount
			,_IsClearing
			,_MatchKey
			,_PartnerFinishedCount
			,_PartnerRemainCount
			,_PartnerBuyCount
			,_PartnerList
			,_PartnerSelectIndex
			,_PartnerLastResetTime
			,_DungeonQueryCount
			,_DungeonLeftFightCount
			,_DungeonBuyFightCount
			,_DungeonMatchTargetList
			,_DungeonMatchDropList
			,_DungeonLastResetTime
			,_SecretCurrentFight
			,_SecretHpRateList
			,_SecretMpRateList
			,_SecretSegmentList
			,_SecretFightCountList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			SceneId =  if(DataVersion < _DataVersion, _SceneId, SceneId),
			StartTime =  if(DataVersion < _DataVersion, _StartTime, StartTime),
			SumGold =  if(DataVersion < _DataVersion, _SumGold, SumGold),
			Exp =  if(DataVersion < _DataVersion, _Exp, Exp),
			RewardItemId =  if(DataVersion < _DataVersion, _RewardItemId, RewardItemId),
			RewardItemCount =  if(DataVersion < _DataVersion, _RewardItemCount, RewardItemCount),
			DeadCount =  if(DataVersion < _DataVersion, _DeadCount, DeadCount),
			ReliveCount =  if(DataVersion < _DataVersion, _ReliveCount, ReliveCount),
			IsClearing =  if(DataVersion < _DataVersion, _IsClearing, IsClearing),
			MatchKey =  if(DataVersion < _DataVersion, _MatchKey, MatchKey),
			PartnerFinishedCount =  if(DataVersion < _DataVersion, _PartnerFinishedCount, PartnerFinishedCount),
			PartnerRemainCount =  if(DataVersion < _DataVersion, _PartnerRemainCount, PartnerRemainCount),
			PartnerBuyCount =  if(DataVersion < _DataVersion, _PartnerBuyCount, PartnerBuyCount),
			PartnerList =  if(DataVersion < _DataVersion, _PartnerList, PartnerList),
			PartnerSelectIndex =  if(DataVersion < _DataVersion, _PartnerSelectIndex, PartnerSelectIndex),
			PartnerLastResetTime =  if(DataVersion < _DataVersion, _PartnerLastResetTime, PartnerLastResetTime),
			DungeonQueryCount =  if(DataVersion < _DataVersion, _DungeonQueryCount, DungeonQueryCount),
			DungeonLeftFightCount =  if(DataVersion < _DataVersion, _DungeonLeftFightCount, DungeonLeftFightCount),
			DungeonBuyFightCount =  if(DataVersion < _DataVersion, _DungeonBuyFightCount, DungeonBuyFightCount),
			DungeonMatchTargetList =  if(DataVersion < _DataVersion, _DungeonMatchTargetList, DungeonMatchTargetList),
			DungeonMatchDropList =  if(DataVersion < _DataVersion, _DungeonMatchDropList, DungeonMatchDropList),
			DungeonLastResetTime =  if(DataVersion < _DataVersion, _DungeonLastResetTime, DungeonLastResetTime),
			SecretCurrentFight =  if(DataVersion < _DataVersion, _SecretCurrentFight, SecretCurrentFight),
			SecretHpRateList =  if(DataVersion < _DataVersion, _SecretHpRateList, SecretHpRateList),
			SecretMpRateList =  if(DataVersion < _DataVersion, _SecretMpRateList, SecretMpRateList),
			SecretSegmentList =  if(DataVersion < _DataVersion, _SecretSegmentList, SecretSegmentList),
			SecretFightCountList =  if(DataVersion < _DataVersion, _SecretFightCountList, SecretFightCountList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableUserGeneralInfo;
delimiter $$
create procedure SaveTableUserGeneralInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint
	,in _GuideFlag bigint
	,in _NewbieFlag varchar(255)
	,in _NewbieActionFlag varchar(255)
	,in _BuyMoneyCount int
	,in _LastBuyMoneyTimestamp double
	,in _LastResetMidasTouchTime varchar(24)
	,in _SellIncome int
	,in _LastSellTimestamp double
	,in _LastResetSellTime varchar(24)
	,in _ExchangeGoodList varchar(255)
	,in _ExchangeGoodNumber varchar(64)
	,in _ExchangeGoodRefreshCount varchar(255)
	,in _LastResetExchangeGoodTime varchar(24)
	,in _LastResetDaySignCountTime varchar(24)
	,in _MonthSignCount int
	,in _LastResetMonthSignCountTime varchar(24)
	,in _MonthCardExpireTime varchar(24)
	,in _IsWeeklyLoginRewarded boolean
	,in _WeeklyLoginRewardList varchar(64)
	,in _LastResetWeeklyLoginRewardTime varchar(24)
	,in _DailyOnlineDuration int
	,in _DailyOnlineRewardList varchar(48)
	,in _LastResetDailyOnlineTime varchar(24)
	,in _IsFirstPaymentRewarded boolean
	,in _VipRewaredList varchar(255)
	,in _MorrowRewardId int
	,in _MorrowActiveTime varchar(24)
	,in _IsMorrowActive boolean
	,in _LevelupCostTime double
	,in _IsChatForbidden boolean
	,in _GrowthFund int
	,in _ChapterIdList varchar(24)
	,in _ChapterAwardList varchar(255)
	,in _LastResetConsumeCountTime varchar(24)
	,in _DayRestSignCount int
)
begin
	insert into TableUserGeneralInfo (AutoKey,IsValid,DataVersion,Guid,GuideFlag,NewbieFlag,NewbieActionFlag,BuyMoneyCount,LastBuyMoneyTimestamp,LastResetMidasTouchTime,SellIncome,LastSellTimestamp,LastResetSellTime,ExchangeGoodList,ExchangeGoodNumber,ExchangeGoodRefreshCount,LastResetExchangeGoodTime,LastResetDaySignCountTime,MonthSignCount,LastResetMonthSignCountTime,MonthCardExpireTime,IsWeeklyLoginRewarded,WeeklyLoginRewardList,LastResetWeeklyLoginRewardTime,DailyOnlineDuration,DailyOnlineRewardList,LastResetDailyOnlineTime,IsFirstPaymentRewarded,VipRewaredList,MorrowRewardId,MorrowActiveTime,IsMorrowActive,LevelupCostTime,IsChatForbidden,GrowthFund,ChapterIdList,ChapterAwardList,LastResetConsumeCountTime,DayRestSignCount)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_GuideFlag
			,_NewbieFlag
			,_NewbieActionFlag
			,_BuyMoneyCount
			,_LastBuyMoneyTimestamp
			,_LastResetMidasTouchTime
			,_SellIncome
			,_LastSellTimestamp
			,_LastResetSellTime
			,_ExchangeGoodList
			,_ExchangeGoodNumber
			,_ExchangeGoodRefreshCount
			,_LastResetExchangeGoodTime
			,_LastResetDaySignCountTime
			,_MonthSignCount
			,_LastResetMonthSignCountTime
			,_MonthCardExpireTime
			,_IsWeeklyLoginRewarded
			,_WeeklyLoginRewardList
			,_LastResetWeeklyLoginRewardTime
			,_DailyOnlineDuration
			,_DailyOnlineRewardList
			,_LastResetDailyOnlineTime
			,_IsFirstPaymentRewarded
			,_VipRewaredList
			,_MorrowRewardId
			,_MorrowActiveTime
			,_IsMorrowActive
			,_LevelupCostTime
			,_IsChatForbidden
			,_GrowthFund
			,_ChapterIdList
			,_ChapterAwardList
			,_LastResetConsumeCountTime
			,_DayRestSignCount
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			GuideFlag =  if(DataVersion < _DataVersion, _GuideFlag, GuideFlag),
			NewbieFlag =  if(DataVersion < _DataVersion, _NewbieFlag, NewbieFlag),
			NewbieActionFlag =  if(DataVersion < _DataVersion, _NewbieActionFlag, NewbieActionFlag),
			BuyMoneyCount =  if(DataVersion < _DataVersion, _BuyMoneyCount, BuyMoneyCount),
			LastBuyMoneyTimestamp =  if(DataVersion < _DataVersion, _LastBuyMoneyTimestamp, LastBuyMoneyTimestamp),
			LastResetMidasTouchTime =  if(DataVersion < _DataVersion, _LastResetMidasTouchTime, LastResetMidasTouchTime),
			SellIncome =  if(DataVersion < _DataVersion, _SellIncome, SellIncome),
			LastSellTimestamp =  if(DataVersion < _DataVersion, _LastSellTimestamp, LastSellTimestamp),
			LastResetSellTime =  if(DataVersion < _DataVersion, _LastResetSellTime, LastResetSellTime),
			ExchangeGoodList =  if(DataVersion < _DataVersion, _ExchangeGoodList, ExchangeGoodList),
			ExchangeGoodNumber =  if(DataVersion < _DataVersion, _ExchangeGoodNumber, ExchangeGoodNumber),
			ExchangeGoodRefreshCount =  if(DataVersion < _DataVersion, _ExchangeGoodRefreshCount, ExchangeGoodRefreshCount),
			LastResetExchangeGoodTime =  if(DataVersion < _DataVersion, _LastResetExchangeGoodTime, LastResetExchangeGoodTime),
			LastResetDaySignCountTime =  if(DataVersion < _DataVersion, _LastResetDaySignCountTime, LastResetDaySignCountTime),
			MonthSignCount =  if(DataVersion < _DataVersion, _MonthSignCount, MonthSignCount),
			LastResetMonthSignCountTime =  if(DataVersion < _DataVersion, _LastResetMonthSignCountTime, LastResetMonthSignCountTime),
			MonthCardExpireTime =  if(DataVersion < _DataVersion, _MonthCardExpireTime, MonthCardExpireTime),
			IsWeeklyLoginRewarded =  if(DataVersion < _DataVersion, _IsWeeklyLoginRewarded, IsWeeklyLoginRewarded),
			WeeklyLoginRewardList =  if(DataVersion < _DataVersion, _WeeklyLoginRewardList, WeeklyLoginRewardList),
			LastResetWeeklyLoginRewardTime =  if(DataVersion < _DataVersion, _LastResetWeeklyLoginRewardTime, LastResetWeeklyLoginRewardTime),
			DailyOnlineDuration =  if(DataVersion < _DataVersion, _DailyOnlineDuration, DailyOnlineDuration),
			DailyOnlineRewardList =  if(DataVersion < _DataVersion, _DailyOnlineRewardList, DailyOnlineRewardList),
			LastResetDailyOnlineTime =  if(DataVersion < _DataVersion, _LastResetDailyOnlineTime, LastResetDailyOnlineTime),
			IsFirstPaymentRewarded =  if(DataVersion < _DataVersion, _IsFirstPaymentRewarded, IsFirstPaymentRewarded),
			VipRewaredList =  if(DataVersion < _DataVersion, _VipRewaredList, VipRewaredList),
			MorrowRewardId =  if(DataVersion < _DataVersion, _MorrowRewardId, MorrowRewardId),
			MorrowActiveTime =  if(DataVersion < _DataVersion, _MorrowActiveTime, MorrowActiveTime),
			IsMorrowActive =  if(DataVersion < _DataVersion, _IsMorrowActive, IsMorrowActive),
			LevelupCostTime =  if(DataVersion < _DataVersion, _LevelupCostTime, LevelupCostTime),
			IsChatForbidden =  if(DataVersion < _DataVersion, _IsChatForbidden, IsChatForbidden),
			GrowthFund =  if(DataVersion < _DataVersion, _GrowthFund, GrowthFund),
			ChapterIdList =  if(DataVersion < _DataVersion, _ChapterIdList, ChapterIdList),
			ChapterAwardList =  if(DataVersion < _DataVersion, _ChapterAwardList, ChapterAwardList),
			LastResetConsumeCountTime =  if(DataVersion < _DataVersion, _LastResetConsumeCountTime, LastResetConsumeCountTime),
			DayRestSignCount =  if(DataVersion < _DataVersion, _DayRestSignCount, DayRestSignCount),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableUserInfo;
delimiter $$
create procedure SaveTableUserInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint unsigned
	,in _LogicServerId int
	,in _AccountId varchar(64)
	,in _Nickname varchar(32)
	,in _HeroId int
	,in _CreateTime varchar(24)
	,in _Level int
	,in _Money int
	,in _Gold int
	,in _ExpPoints int
	,in _CitySceneId int
	,in _Vip int
	,in _LastLogoutTime varchar(24)
	,in _AchievedScore int
)
begin
	insert into TableUserInfo (AutoKey,IsValid,DataVersion,Guid,LogicServerId,AccountId,Nickname,HeroId,CreateTime,Level,Money,Gold,ExpPoints,CitySceneId,Vip,LastLogoutTime,AchievedScore)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_LogicServerId
			,_AccountId
			,_Nickname
			,_HeroId
			,_CreateTime
			,_Level
			,_Money
			,_Gold
			,_ExpPoints
			,_CitySceneId
			,_Vip
			,_LastLogoutTime
			,_AchievedScore
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			LogicServerId =  if(DataVersion < _DataVersion, _LogicServerId, LogicServerId),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			CreateTime =  if(DataVersion < _DataVersion, _CreateTime, CreateTime),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			Money =  if(DataVersion < _DataVersion, _Money, Money),
			Gold =  if(DataVersion < _DataVersion, _Gold, Gold),
			ExpPoints =  if(DataVersion < _DataVersion, _ExpPoints, ExpPoints),
			CitySceneId =  if(DataVersion < _DataVersion, _CitySceneId, CitySceneId),
			Vip =  if(DataVersion < _DataVersion, _Vip, Vip),
			LastLogoutTime =  if(DataVersion < _DataVersion, _LastLogoutTime, LastLogoutTime),
			AchievedScore =  if(DataVersion < _DataVersion, _AchievedScore, AchievedScore),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableUserSpecialInfo;
delimiter $$
create procedure SaveTableUserSpecialInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint
	,in _Vigor int
	,in _LastAddVigorTimestamp double
	,in _Stamina int
	,in _BuyStaminaCount int
	,in _LastAddStaminaTimestamp double
	,in _UsedStamina int
	,in _LastResetStaminaTime varchar(24)
	,in _CompleteSceneList varchar(255)
	,in _CompleteSceneNumber varchar(64)
	,in _LastResetSceneCountTime varchar(24)
	,in _LastResetDailyMissionTime varchar(24)
	,in _ActiveFashionId int
	,in _IsFashionShow boolean
	,in _ActiveWingId int
	,in _IsWingShow boolean
	,in _ActiveWeaponId int
	,in _IsWeaponShow boolean
	,in _Vitality int
	,in _LastAddVitalityTimestamp double
	,in _LastResetExpeditionTime varchar(24)
	,in _CorpsGuid bigint
	,in _LastQuitCorpsTime varchar(24)
	,in _IsAcquireCorpsSignInPrize boolean
	,in _LastResetCorpsSignInPrizeTime varchar(24)
	,in _CorpsChapterIdList varchar(24)
	,in _CorpsChapterDareList varchar(24)
	,in _LastResetSecretAreaTime varchar(24)
	,in _RecentLoginState bigint
	,in _SumLoginDayCount int
	,in _UsedLoginLotteryDrawCount int
	,in _LastSaveRecentLoginTime varchar(24)
	,in _DiamondBoxList varchar(24)
	,in _LastResetMpveAwardTime varchar(24)
	,in _FinishedActivityList varchar(255)
)
begin
	insert into TableUserSpecialInfo (AutoKey,IsValid,DataVersion,Guid,Vigor,LastAddVigorTimestamp,Stamina,BuyStaminaCount,LastAddStaminaTimestamp,UsedStamina,LastResetStaminaTime,CompleteSceneList,CompleteSceneNumber,LastResetSceneCountTime,LastResetDailyMissionTime,ActiveFashionId,IsFashionShow,ActiveWingId,IsWingShow,ActiveWeaponId,IsWeaponShow,Vitality,LastAddVitalityTimestamp,LastResetExpeditionTime,CorpsGuid,LastQuitCorpsTime,IsAcquireCorpsSignInPrize,LastResetCorpsSignInPrizeTime,CorpsChapterIdList,CorpsChapterDareList,LastResetSecretAreaTime,RecentLoginState,SumLoginDayCount,UsedLoginLotteryDrawCount,LastSaveRecentLoginTime,DiamondBoxList,LastResetMpveAwardTime,FinishedActivityList)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_Vigor
			,_LastAddVigorTimestamp
			,_Stamina
			,_BuyStaminaCount
			,_LastAddStaminaTimestamp
			,_UsedStamina
			,_LastResetStaminaTime
			,_CompleteSceneList
			,_CompleteSceneNumber
			,_LastResetSceneCountTime
			,_LastResetDailyMissionTime
			,_ActiveFashionId
			,_IsFashionShow
			,_ActiveWingId
			,_IsWingShow
			,_ActiveWeaponId
			,_IsWeaponShow
			,_Vitality
			,_LastAddVitalityTimestamp
			,_LastResetExpeditionTime
			,_CorpsGuid
			,_LastQuitCorpsTime
			,_IsAcquireCorpsSignInPrize
			,_LastResetCorpsSignInPrizeTime
			,_CorpsChapterIdList
			,_CorpsChapterDareList
			,_LastResetSecretAreaTime
			,_RecentLoginState
			,_SumLoginDayCount
			,_UsedLoginLotteryDrawCount
			,_LastSaveRecentLoginTime
			,_DiamondBoxList
			,_LastResetMpveAwardTime
			,_FinishedActivityList
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			Vigor =  if(DataVersion < _DataVersion, _Vigor, Vigor),
			LastAddVigorTimestamp =  if(DataVersion < _DataVersion, _LastAddVigorTimestamp, LastAddVigorTimestamp),
			Stamina =  if(DataVersion < _DataVersion, _Stamina, Stamina),
			BuyStaminaCount =  if(DataVersion < _DataVersion, _BuyStaminaCount, BuyStaminaCount),
			LastAddStaminaTimestamp =  if(DataVersion < _DataVersion, _LastAddStaminaTimestamp, LastAddStaminaTimestamp),
			UsedStamina =  if(DataVersion < _DataVersion, _UsedStamina, UsedStamina),
			LastResetStaminaTime =  if(DataVersion < _DataVersion, _LastResetStaminaTime, LastResetStaminaTime),
			CompleteSceneList =  if(DataVersion < _DataVersion, _CompleteSceneList, CompleteSceneList),
			CompleteSceneNumber =  if(DataVersion < _DataVersion, _CompleteSceneNumber, CompleteSceneNumber),
			LastResetSceneCountTime =  if(DataVersion < _DataVersion, _LastResetSceneCountTime, LastResetSceneCountTime),
			LastResetDailyMissionTime =  if(DataVersion < _DataVersion, _LastResetDailyMissionTime, LastResetDailyMissionTime),
			ActiveFashionId =  if(DataVersion < _DataVersion, _ActiveFashionId, ActiveFashionId),
			IsFashionShow =  if(DataVersion < _DataVersion, _IsFashionShow, IsFashionShow),
			ActiveWingId =  if(DataVersion < _DataVersion, _ActiveWingId, ActiveWingId),
			IsWingShow =  if(DataVersion < _DataVersion, _IsWingShow, IsWingShow),
			ActiveWeaponId =  if(DataVersion < _DataVersion, _ActiveWeaponId, ActiveWeaponId),
			IsWeaponShow =  if(DataVersion < _DataVersion, _IsWeaponShow, IsWeaponShow),
			Vitality =  if(DataVersion < _DataVersion, _Vitality, Vitality),
			LastAddVitalityTimestamp =  if(DataVersion < _DataVersion, _LastAddVitalityTimestamp, LastAddVitalityTimestamp),
			LastResetExpeditionTime =  if(DataVersion < _DataVersion, _LastResetExpeditionTime, LastResetExpeditionTime),
			CorpsGuid =  if(DataVersion < _DataVersion, _CorpsGuid, CorpsGuid),
			LastQuitCorpsTime =  if(DataVersion < _DataVersion, _LastQuitCorpsTime, LastQuitCorpsTime),
			IsAcquireCorpsSignInPrize =  if(DataVersion < _DataVersion, _IsAcquireCorpsSignInPrize, IsAcquireCorpsSignInPrize),
			LastResetCorpsSignInPrizeTime =  if(DataVersion < _DataVersion, _LastResetCorpsSignInPrizeTime, LastResetCorpsSignInPrizeTime),
			CorpsChapterIdList =  if(DataVersion < _DataVersion, _CorpsChapterIdList, CorpsChapterIdList),
			CorpsChapterDareList =  if(DataVersion < _DataVersion, _CorpsChapterDareList, CorpsChapterDareList),
			LastResetSecretAreaTime =  if(DataVersion < _DataVersion, _LastResetSecretAreaTime, LastResetSecretAreaTime),
			RecentLoginState =  if(DataVersion < _DataVersion, _RecentLoginState, RecentLoginState),
			SumLoginDayCount =  if(DataVersion < _DataVersion, _SumLoginDayCount, SumLoginDayCount),
			UsedLoginLotteryDrawCount =  if(DataVersion < _DataVersion, _UsedLoginLotteryDrawCount, UsedLoginLotteryDrawCount),
			LastSaveRecentLoginTime =  if(DataVersion < _DataVersion, _LastSaveRecentLoginTime, LastSaveRecentLoginTime),
			DiamondBoxList =  if(DataVersion < _DataVersion, _DiamondBoxList, DiamondBoxList),
			LastResetMpveAwardTime =  if(DataVersion < _DataVersion, _LastResetMpveAwardTime, LastResetMpveAwardTime),
			FinishedActivityList =  if(DataVersion < _DataVersion, _FinishedActivityList, FinishedActivityList),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableXSoulInfo;
delimiter $$
create procedure SaveTableXSoulInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid varchar(24)
	,in _UserGuid bigint
	,in _Position int
	,in _XSoulType int
	,in _XSoulId int
	,in _XSoulLevel int
	,in _XSoulExp int
	,in _XSoulModelLevel int
)
begin
	insert into TableXSoulInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,Position,XSoulType,XSoulId,XSoulLevel,XSoulExp,XSoulModelLevel)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_Position
			,_XSoulType
			,_XSoulId
			,_XSoulLevel
			,_XSoulExp
			,_XSoulModelLevel
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			Position =  if(DataVersion < _DataVersion, _Position, Position),
			XSoulType =  if(DataVersion < _DataVersion, _XSoulType, XSoulType),
			XSoulId =  if(DataVersion < _DataVersion, _XSoulId, XSoulId),
			XSoulLevel =  if(DataVersion < _DataVersion, _XSoulLevel, XSoulLevel),
			XSoulExp =  if(DataVersion < _DataVersion, _XSoulExp, XSoulExp),
			XSoulModelLevel =  if(DataVersion < _DataVersion, _XSoulModelLevel, XSoulModelLevel),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;


#----------------------------------------------------------------------------------------------------------------------

drop procedure if exists LoadAllTableAccount;
delimiter $$
create procedure LoadAllTableAccount(in _Start int, in _Count int)
begin
	select * from TableAccount where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableAccount;
delimiter $$
create procedure LoadSingleTableAccount(
	in _LogicServerId int
	,in _AccountId varchar(64)
)
begin
	select * from TableAccount where IsValid = 1 
		and LogicServerId = _LogicServerId 
		and AccountId = _AccountId 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableActivationCode;
delimiter $$
create procedure LoadAllTableActivationCode(in _Start int, in _Count int)
begin
	select * from TableActivationCode where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableActivationCode;
delimiter $$
create procedure LoadSingleTableActivationCode(
	in _ActivationCode varchar(32)
)
begin
	select * from TableActivationCode where IsValid = 1 
		and ActivationCode = _ActivationCode 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableArenaInfo;
delimiter $$
create procedure LoadAllTableArenaInfo(in _Start int, in _Count int)
begin
	select * from TableArenaInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableArenaInfo;
delimiter $$
create procedure LoadSingleTableArenaInfo(
	in _UserGuid bigint
)
begin
	select * from TableArenaInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableArenaRecord;
delimiter $$
create procedure LoadAllTableArenaRecord(in _Start int, in _Count int)
begin
	select * from TableArenaRecord where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableArenaRecord;
delimiter $$
create procedure LoadSingleTableArenaRecord(
	in _Guid varchar(24)
)
begin
	select * from TableArenaRecord where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableArenaRecord;
delimiter $$
create procedure LoadMultiTableArenaRecord(
	in _UserGuid bigint
)
begin
	select * from TableArenaRecord where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableCorpsInfo;
delimiter $$
create procedure LoadAllTableCorpsInfo(in _Start int, in _Count int)
begin
	select * from TableCorpsInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableCorpsInfo;
delimiter $$
create procedure LoadSingleTableCorpsInfo(
	in _CorpsGuid bigint
)
begin
	select * from TableCorpsInfo where IsValid = 1 
		and CorpsGuid = _CorpsGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableCorpsMember;
delimiter $$
create procedure LoadAllTableCorpsMember(in _Start int, in _Count int)
begin
	select * from TableCorpsMember where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableCorpsMember;
delimiter $$
create procedure LoadSingleTableCorpsMember(
	in _UserGuid bigint
)
begin
	select * from TableCorpsMember where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableCorpsMember;
delimiter $$
create procedure LoadMultiTableCorpsMember(
	in _CorpsGuid bigint
)
begin
	select * from TableCorpsMember where IsValid = 1 
		and CorpsGuid = _CorpsGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableEquipInfo;
delimiter $$
create procedure LoadAllTableEquipInfo(in _Start int, in _Count int)
begin
	select * from TableEquipInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableEquipInfo;
delimiter $$
create procedure LoadSingleTableEquipInfo(
	in _Guid varchar(24)
)
begin
	select * from TableEquipInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableEquipInfo;
delimiter $$
create procedure LoadMultiTableEquipInfo(
	in _UserGuid bigint
)
begin
	select * from TableEquipInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableExpeditionInfo;
delimiter $$
create procedure LoadAllTableExpeditionInfo(in _Start int, in _Count int)
begin
	select * from TableExpeditionInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableExpeditionInfo;
delimiter $$
create procedure LoadSingleTableExpeditionInfo(
	in _Guid varchar(24)
)
begin
	select * from TableExpeditionInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableExpeditionInfo;
delimiter $$
create procedure LoadMultiTableExpeditionInfo(
	in _UserGuid bigint
)
begin
	select * from TableExpeditionInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableFashionInfo;
delimiter $$
create procedure LoadAllTableFashionInfo(in _Start int, in _Count int)
begin
	select * from TableFashionInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableFashionInfo;
delimiter $$
create procedure LoadSingleTableFashionInfo(
	in _Guid varchar(24)
)
begin
	select * from TableFashionInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableFashionInfo;
delimiter $$
create procedure LoadMultiTableFashionInfo(
	in _UserGuid bigint
)
begin
	select * from TableFashionInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableFightInfo;
delimiter $$
create procedure LoadAllTableFightInfo(in _Start int, in _Count int)
begin
	select * from TableFightInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableFightInfo;
delimiter $$
create procedure LoadSingleTableFightInfo(
	in _Guid varchar(16)
)
begin
	select * from TableFightInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableFriendInfo;
delimiter $$
create procedure LoadAllTableFriendInfo(in _Start int, in _Count int)
begin
	select * from TableFriendInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableFriendInfo;
delimiter $$
create procedure LoadSingleTableFriendInfo(
	in _Guid varchar(24)
)
begin
	select * from TableFriendInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableFriendInfo;
delimiter $$
create procedure LoadMultiTableFriendInfo(
	in _UserGuid bigint
)
begin
	select * from TableFriendInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableGlobalParam;
delimiter $$
create procedure LoadAllTableGlobalParam(in _Start int, in _Count int)
begin
	select * from TableGlobalParam where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableGlobalParam;
delimiter $$
create procedure LoadSingleTableGlobalParam(
	in _ParamType varchar(32)
)
begin
	select * from TableGlobalParam where IsValid = 1 
		and ParamType = _ParamType 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableGowInfo;
delimiter $$
create procedure LoadAllTableGowInfo(in _Start int, in _Count int)
begin
	select * from TableGowInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableGowInfo;
delimiter $$
create procedure LoadSingleTableGowInfo(
	in _Guid varchar(24)
)
begin
	select * from TableGowInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableGowInfo;
delimiter $$
create procedure LoadMultiTableGowInfo(
	in _UserGuid bigint
)
begin
	select * from TableGowInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableGowStar;
delimiter $$
create procedure LoadAllTableGowStar(in _Start int, in _Count int)
begin
	select * from TableGowStar where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableGowStar;
delimiter $$
create procedure LoadSingleTableGowStar(
	in _Guid varchar(16)
)
begin
	select * from TableGowStar where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableGuid;
delimiter $$
create procedure LoadAllTableGuid(in _Start int, in _Count int)
begin
	select * from TableGuid where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableGuid;
delimiter $$
create procedure LoadSingleTableGuid(
	in _GuidType varchar(24)
)
begin
	select * from TableGuid where IsValid = 1 
		and GuidType = _GuidType 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableHomeNotice;
delimiter $$
create procedure LoadAllTableHomeNotice(in _Start int, in _Count int)
begin
	select * from TableHomeNotice where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableHomeNotice;
delimiter $$
create procedure LoadSingleTableHomeNotice(
	in _LogicServerId int
)
begin
	select * from TableHomeNotice where IsValid = 1 
		and LogicServerId = _LogicServerId 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableInviterInfo;
delimiter $$
create procedure LoadAllTableInviterInfo(in _Start int, in _Count int)
begin
	select * from TableInviterInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableInviterInfo;
delimiter $$
create procedure LoadSingleTableInviterInfo(
	in _UserGuid bigint
)
begin
	select * from TableInviterInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableItemInfo;
delimiter $$
create procedure LoadAllTableItemInfo(in _Start int, in _Count int)
begin
	select * from TableItemInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableItemInfo;
delimiter $$
create procedure LoadSingleTableItemInfo(
	in _ItemGuid bigint unsigned
)
begin
	select * from TableItemInfo where IsValid = 1 
		and ItemGuid = _ItemGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableItemInfo;
delimiter $$
create procedure LoadMultiTableItemInfo(
	in _UserGuid bigint unsigned
)
begin
	select * from TableItemInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLegacyInfo;
delimiter $$
create procedure LoadAllTableLegacyInfo(in _Start int, in _Count int)
begin
	select * from TableLegacyInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLegacyInfo;
delimiter $$
create procedure LoadSingleTableLegacyInfo(
	in _Guid varchar(24)
)
begin
	select * from TableLegacyInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableLegacyInfo;
delimiter $$
create procedure LoadMultiTableLegacyInfo(
	in _UserGuid bigint
)
begin
	select * from TableLegacyInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLevelInfo;
delimiter $$
create procedure LoadAllTableLevelInfo(in _Start int, in _Count int)
begin
	select * from TableLevelInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLevelInfo;
delimiter $$
create procedure LoadSingleTableLevelInfo(
	in _Guid varchar(24)
)
begin
	select * from TableLevelInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableLevelInfo;
delimiter $$
create procedure LoadMultiTableLevelInfo(
	in _UserGuid bigint
)
begin
	select * from TableLevelInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLoginLotteryRecord;
delimiter $$
create procedure LoadAllTableLoginLotteryRecord(in _Start int, in _Count int)
begin
	select * from TableLoginLotteryRecord where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLoginLotteryRecord;
delimiter $$
create procedure LoadSingleTableLoginLotteryRecord(
	in _RecordId bigint
)
begin
	select * from TableLoginLotteryRecord where IsValid = 1 
		and RecordId = _RecordId 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLootInfo;
delimiter $$
create procedure LoadAllTableLootInfo(in _Start int, in _Count int)
begin
	select * from TableLootInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLootInfo;
delimiter $$
create procedure LoadSingleTableLootInfo(
	in _UserGuid bigint
)
begin
	select * from TableLootInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLootRecord;
delimiter $$
create procedure LoadAllTableLootRecord(in _Start int, in _Count int)
begin
	select * from TableLootRecord where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLootRecord;
delimiter $$
create procedure LoadSingleTableLootRecord(
	in _Guid varchar(24)
)
begin
	select * from TableLootRecord where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableLootRecord;
delimiter $$
create procedure LoadMultiTableLootRecord(
	in _UserGuid bigint
)
begin
	select * from TableLootRecord where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableLotteryInfo;
delimiter $$
create procedure LoadAllTableLotteryInfo(in _Start int, in _Count int)
begin
	select * from TableLotteryInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableLotteryInfo;
delimiter $$
create procedure LoadSingleTableLotteryInfo(
	in _Guid varchar(24)
)
begin
	select * from TableLotteryInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableLotteryInfo;
delimiter $$
create procedure LoadMultiTableLotteryInfo(
	in _UserGuid bigint
)
begin
	select * from TableLotteryInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableMailInfo;
delimiter $$
create procedure LoadAllTableMailInfo(in _Start int, in _Count int)
begin
	select * from TableMailInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableMailInfo;
delimiter $$
create procedure LoadSingleTableMailInfo(
	in _Guid bigint
)
begin
	select * from TableMailInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableMailStateInfo;
delimiter $$
create procedure LoadAllTableMailStateInfo(in _Start int, in _Count int)
begin
	select * from TableMailStateInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableMailStateInfo;
delimiter $$
create procedure LoadSingleTableMailStateInfo(
	in _Guid varchar(24)
)
begin
	select * from TableMailStateInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableMailStateInfo;
delimiter $$
create procedure LoadMultiTableMailStateInfo(
	in _UserGuid bigint
)
begin
	select * from TableMailStateInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableMissionInfo;
delimiter $$
create procedure LoadAllTableMissionInfo(in _Start int, in _Count int)
begin
	select * from TableMissionInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableMissionInfo;
delimiter $$
create procedure LoadSingleTableMissionInfo(
	in _Guid varchar(24)
)
begin
	select * from TableMissionInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableMissionInfo;
delimiter $$
create procedure LoadMultiTableMissionInfo(
	in _UserGuid bigint
)
begin
	select * from TableMissionInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableMpveAwardInfo;
delimiter $$
create procedure LoadAllTableMpveAwardInfo(in _Start int, in _Count int)
begin
	select * from TableMpveAwardInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableMpveAwardInfo;
delimiter $$
create procedure LoadSingleTableMpveAwardInfo(
	in _Guid varchar(24)
)
begin
	select * from TableMpveAwardInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableMpveAwardInfo;
delimiter $$
create procedure LoadMultiTableMpveAwardInfo(
	in _UserGuid bigint
)
begin
	select * from TableMpveAwardInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableNickname;
delimiter $$
create procedure LoadAllTableNickname(in _Start int, in _Count int)
begin
	select * from TableNickname where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableNickname;
delimiter $$
create procedure LoadSingleTableNickname(
	in _Nickname varchar(32)
)
begin
	select * from TableNickname where IsValid = 1 
		and Nickname = _Nickname 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTablePartnerInfo;
delimiter $$
create procedure LoadAllTablePartnerInfo(in _Start int, in _Count int)
begin
	select * from TablePartnerInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTablePartnerInfo;
delimiter $$
create procedure LoadSingleTablePartnerInfo(
	in _Guid varchar(24)
)
begin
	select * from TablePartnerInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTablePartnerInfo;
delimiter $$
create procedure LoadMultiTablePartnerInfo(
	in _UserGuid bigint
)
begin
	select * from TablePartnerInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTablePaymentInfo;
delimiter $$
create procedure LoadAllTablePaymentInfo(in _Start int, in _Count int)
begin
	select * from TablePaymentInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTablePaymentInfo;
delimiter $$
create procedure LoadSingleTablePaymentInfo(
	in _Guid varchar(24)
)
begin
	select * from TablePaymentInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTablePaymentInfo;
delimiter $$
create procedure LoadMultiTablePaymentInfo(
	in _UserGuid bigint
)
begin
	select * from TablePaymentInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableSkillInfo;
delimiter $$
create procedure LoadAllTableSkillInfo(in _Start int, in _Count int)
begin
	select * from TableSkillInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableSkillInfo;
delimiter $$
create procedure LoadSingleTableSkillInfo(
	in _Guid varchar(24)
)
begin
	select * from TableSkillInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableSkillInfo;
delimiter $$
create procedure LoadMultiTableSkillInfo(
	in _UserGuid bigint
)
begin
	select * from TableSkillInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableTalentInfo;
delimiter $$
create procedure LoadAllTableTalentInfo(in _Start int, in _Count int)
begin
	select * from TableTalentInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableTalentInfo;
delimiter $$
create procedure LoadSingleTableTalentInfo(
	in _Guid varchar(24)
)
begin
	select * from TableTalentInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableTalentInfo;
delimiter $$
create procedure LoadMultiTableTalentInfo(
	in _UserGuid bigint
)
begin
	select * from TableTalentInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableUndonePayment;
delimiter $$
create procedure LoadAllTableUndonePayment(in _Start int, in _Count int)
begin
	select * from TableUndonePayment where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableUndonePayment;
delimiter $$
create procedure LoadSingleTableUndonePayment(
	in _OrderId varchar(48)
)
begin
	select * from TableUndonePayment where IsValid = 1 
		and OrderId = _OrderId 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableUserBattleInfo;
delimiter $$
create procedure LoadAllTableUserBattleInfo(in _Start int, in _Count int)
begin
	select * from TableUserBattleInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableUserBattleInfo;
delimiter $$
create procedure LoadSingleTableUserBattleInfo(
	in _Guid bigint
)
begin
	select * from TableUserBattleInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableUserGeneralInfo;
delimiter $$
create procedure LoadAllTableUserGeneralInfo(in _Start int, in _Count int)
begin
	select * from TableUserGeneralInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableUserGeneralInfo;
delimiter $$
create procedure LoadSingleTableUserGeneralInfo(
	in _Guid bigint
)
begin
	select * from TableUserGeneralInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableUserInfo;
delimiter $$
create procedure LoadAllTableUserInfo(in _Start int, in _Count int)
begin
	select * from TableUserInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableUserInfo;
delimiter $$
create procedure LoadSingleTableUserInfo(
	in _Guid bigint unsigned
)
begin
	select * from TableUserInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableUserInfo;
delimiter $$
create procedure LoadMultiTableUserInfo(
	in _LogicServerId int
	,in _AccountId varchar(64)
)
begin
	select * from TableUserInfo where IsValid = 1 
		and LogicServerId = _LogicServerId 
		and AccountId = _AccountId 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableUserSpecialInfo;
delimiter $$
create procedure LoadAllTableUserSpecialInfo(in _Start int, in _Count int)
begin
	select * from TableUserSpecialInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableUserSpecialInfo;
delimiter $$
create procedure LoadSingleTableUserSpecialInfo(
	in _Guid bigint
)
begin
	select * from TableUserSpecialInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableXSoulInfo;
delimiter $$
create procedure LoadAllTableXSoulInfo(in _Start int, in _Count int)
begin
	select * from TableXSoulInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableXSoulInfo;
delimiter $$
create procedure LoadSingleTableXSoulInfo(
	in _Guid varchar(24)
)
begin
	select * from TableXSoulInfo where IsValid = 1 
		and Guid = _Guid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableXSoulInfo;
delimiter $$
create procedure LoadMultiTableXSoulInfo(
	in _UserGuid bigint
)
begin
	select * from TableXSoulInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

