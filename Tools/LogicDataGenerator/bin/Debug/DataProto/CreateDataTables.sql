#----------------------------------------------------------------------------
#！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
#----------------------------------------------------------------------------

call SetDSNodeVersion('0.0.1');

create table TableAccount
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	AccountId varchar(64) binary not null,
	IsBanned boolean not null,
	UserGuid bigint unsigned not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableAccountPrimaryIndex on TableAccount (AccountId);

create table TableActivationCode
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	ActivationCode varchar(32) binary not null,
	IsActivated boolean not null,
	AccountId varchar(64) not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableActivationCodePrimaryIndex on TableActivationCode (ActivationCode);

create table TableFriendInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid varchar(24) binary not null,
	UserGuid bigint not null,
	FriendGuid bigint not null,
	FriendNickname varchar(32) not null,
	QQ bigint not null,
	weixin bigint not null,
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

create table TableItemInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	ItemGuid bigint unsigned not null,
	UserGuid bigint unsigned not null,
	ItemId int not null,
	ItemNum int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableItemInfoPrimaryIndex on TableItemInfo (ItemGuid);
create index TableItemInfoIndex on  TableItemInfo (UserGuid);

create table TableMailInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint not null,
	ModuleTypeId int not null,
	Sender varchar(32) not null,
	Receiver bigint not null,
	SendDate varchar(24) not null,
	ExpiryDate varchar(24) not null,
	Title varchar(64) not null,
	Text varchar(1024) not null,
	Money int not null,
	Gold int not null,
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

create table TableMemberInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	MemberGuid bigint unsigned not null,
	UserGuid bigint unsigned not null,
	HeroId int not null,
	Level int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableMemberInfoPrimaryIndex on TableMemberInfo (MemberGuid);
create index TableMemberInfoIndex on  TableMemberInfo (UserGuid);

create table TableNicknameInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Nickname varchar(32) binary not null,
	UserGuid bigint unsigned not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableNicknameInfoPrimaryIndex on TableNicknameInfo (Nickname);

create table TableUserInfo
(
	AutoKey int not null auto_increment,
	IsValid boolean not null,
	DataVersion int not null,
	Guid bigint unsigned not null,
	AccountId varchar(64) binary not null,
	Nickname varchar(32) not null,
	HeroId int not null,
	CreateTime varchar(24) not null,
	LastLogoutTime varchar(24) not null,
	Level int not null,
	ExpPoints int not null,
	SceneId int not null,
	PositionX float not null,
	PositionZ float not null,
	FaceDir float not null,
	Money int not null,
	Gold int not null,
	primary key (AutoKey)
) ENGINE=InnoDB;
create unique index TableUserInfoPrimaryIndex on TableUserInfo (Guid);
create index TableUserInfoIndex on  TableUserInfo (AccountId);


#----------------------------------------------------------------------------------------------------------------------

drop procedure if exists SaveTableAccount;
delimiter $$
create procedure SaveTableAccount(
	in _IsValid boolean
	,in _DataVersion int
	,in _AccountId varchar(64)
	,in _IsBanned boolean
	,in _UserGuid bigint unsigned
)
begin
	insert into TableAccount (AutoKey,IsValid,DataVersion,AccountId,IsBanned,UserGuid)
		values 
			(null,_IsValid,_DataVersion
			,_AccountId
			,_IsBanned
			,_UserGuid
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			IsBanned =  if(DataVersion < _DataVersion, _IsBanned, IsBanned),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
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
	,in _AccountId varchar(64)
)
begin
	insert into TableActivationCode (AutoKey,IsValid,DataVersion,ActivationCode,IsActivated,AccountId)
		values 
			(null,_IsValid,_DataVersion
			,_ActivationCode
			,_IsActivated
			,_AccountId
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			ActivationCode =  if(DataVersion < _DataVersion, _ActivationCode, ActivationCode),
			IsActivated =  if(DataVersion < _DataVersion, _IsActivated, IsActivated),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
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
	,in _QQ bigint
	,in _weixin bigint
	,in _IsBlack boolean
)
begin
	insert into TableFriendInfo (AutoKey,IsValid,DataVersion,Guid,UserGuid,FriendGuid,FriendNickname,QQ,weixin,IsBlack)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_UserGuid
			,_FriendGuid
			,_FriendNickname
			,_QQ
			,_weixin
			,_IsBlack
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			FriendGuid =  if(DataVersion < _DataVersion, _FriendGuid, FriendGuid),
			FriendNickname =  if(DataVersion < _DataVersion, _FriendNickname, FriendNickname),
			QQ =  if(DataVersion < _DataVersion, _QQ, QQ),
			weixin =  if(DataVersion < _DataVersion, _weixin, weixin),
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

drop procedure if exists SaveTableItemInfo;
delimiter $$
create procedure SaveTableItemInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _ItemGuid bigint unsigned
	,in _UserGuid bigint unsigned
	,in _ItemId int
	,in _ItemNum int
)
begin
	insert into TableItemInfo (AutoKey,IsValid,DataVersion,ItemGuid,UserGuid,ItemId,ItemNum)
		values 
			(null,_IsValid,_DataVersion
			,_ItemGuid
			,_UserGuid
			,_ItemId
			,_ItemNum
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			ItemGuid =  if(DataVersion < _DataVersion, _ItemGuid, ItemGuid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			ItemId =  if(DataVersion < _DataVersion, _ItemId, ItemId),
			ItemNum =  if(DataVersion < _DataVersion, _ItemNum, ItemNum),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableMailInfo;
delimiter $$
create procedure SaveTableMailInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint
	,in _ModuleTypeId int
	,in _Sender varchar(32)
	,in _Receiver bigint
	,in _SendDate varchar(24)
	,in _ExpiryDate varchar(24)
	,in _Title varchar(64)
	,in _Text varchar(1024)
	,in _Money int
	,in _Gold int
	,in _ItemIds varchar(128)
	,in _ItemNumbers varchar(64)
	,in _LevelDemand int
	,in _IsRead boolean
)
begin
	insert into TableMailInfo (AutoKey,IsValid,DataVersion,Guid,ModuleTypeId,Sender,Receiver,SendDate,ExpiryDate,Title,Text,Money,Gold,ItemIds,ItemNumbers,LevelDemand,IsRead)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_ModuleTypeId
			,_Sender
			,_Receiver
			,_SendDate
			,_ExpiryDate
			,_Title
			,_Text
			,_Money
			,_Gold
			,_ItemIds
			,_ItemNumbers
			,_LevelDemand
			,_IsRead
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			ModuleTypeId =  if(DataVersion < _DataVersion, _ModuleTypeId, ModuleTypeId),
			Sender =  if(DataVersion < _DataVersion, _Sender, Sender),
			Receiver =  if(DataVersion < _DataVersion, _Receiver, Receiver),
			SendDate =  if(DataVersion < _DataVersion, _SendDate, SendDate),
			ExpiryDate =  if(DataVersion < _DataVersion, _ExpiryDate, ExpiryDate),
			Title =  if(DataVersion < _DataVersion, _Title, Title),
			Text =  if(DataVersion < _DataVersion, _Text, Text),
			Money =  if(DataVersion < _DataVersion, _Money, Money),
			Gold =  if(DataVersion < _DataVersion, _Gold, Gold),
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

drop procedure if exists SaveTableMemberInfo;
delimiter $$
create procedure SaveTableMemberInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _MemberGuid bigint unsigned
	,in _UserGuid bigint unsigned
	,in _HeroId int
	,in _Level int
)
begin
	insert into TableMemberInfo (AutoKey,IsValid,DataVersion,MemberGuid,UserGuid,HeroId,Level)
		values 
			(null,_IsValid,_DataVersion
			,_MemberGuid
			,_UserGuid
			,_HeroId
			,_Level
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			MemberGuid =  if(DataVersion < _DataVersion, _MemberGuid, MemberGuid),
			UserGuid =  if(DataVersion < _DataVersion, _UserGuid, UserGuid),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			DataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);
end $$
delimiter ;

drop procedure if exists SaveTableNicknameInfo;
delimiter $$
create procedure SaveTableNicknameInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Nickname varchar(32)
	,in _UserGuid bigint unsigned
)
begin
	insert into TableNicknameInfo (AutoKey,IsValid,DataVersion,Nickname,UserGuid)
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

drop procedure if exists SaveTableUserInfo;
delimiter $$
create procedure SaveTableUserInfo(
	in _IsValid boolean
	,in _DataVersion int
	,in _Guid bigint unsigned
	,in _AccountId varchar(64)
	,in _Nickname varchar(32)
	,in _HeroId int
	,in _CreateTime varchar(24)
	,in _LastLogoutTime varchar(24)
	,in _Level int
	,in _ExpPoints int
	,in _SceneId int
	,in _PositionX float
	,in _PositionZ float
	,in _FaceDir float
	,in _Money int
	,in _Gold int
)
begin
	insert into TableUserInfo (AutoKey,IsValid,DataVersion,Guid,AccountId,Nickname,HeroId,CreateTime,LastLogoutTime,Level,ExpPoints,SceneId,PositionX,PositionZ,FaceDir,Money,Gold)
		values 
			(null,_IsValid,_DataVersion
			,_Guid
			,_AccountId
			,_Nickname
			,_HeroId
			,_CreateTime
			,_LastLogoutTime
			,_Level
			,_ExpPoints
			,_SceneId
			,_PositionX
			,_PositionZ
			,_FaceDir
			,_Money
			,_Gold
			)
		on duplicate key update 
			IsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),
			Guid =  if(DataVersion < _DataVersion, _Guid, Guid),
			AccountId =  if(DataVersion < _DataVersion, _AccountId, AccountId),
			Nickname =  if(DataVersion < _DataVersion, _Nickname, Nickname),
			HeroId =  if(DataVersion < _DataVersion, _HeroId, HeroId),
			CreateTime =  if(DataVersion < _DataVersion, _CreateTime, CreateTime),
			LastLogoutTime =  if(DataVersion < _DataVersion, _LastLogoutTime, LastLogoutTime),
			Level =  if(DataVersion < _DataVersion, _Level, Level),
			ExpPoints =  if(DataVersion < _DataVersion, _ExpPoints, ExpPoints),
			SceneId =  if(DataVersion < _DataVersion, _SceneId, SceneId),
			PositionX =  if(DataVersion < _DataVersion, _PositionX, PositionX),
			PositionZ =  if(DataVersion < _DataVersion, _PositionZ, PositionZ),
			FaceDir =  if(DataVersion < _DataVersion, _FaceDir, FaceDir),
			Money =  if(DataVersion < _DataVersion, _Money, Money),
			Gold =  if(DataVersion < _DataVersion, _Gold, Gold),
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
	in _AccountId varchar(64)
)
begin
	select * from TableAccount where IsValid = 1 
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

drop procedure if exists LoadAllTableMemberInfo;
delimiter $$
create procedure LoadAllTableMemberInfo(in _Start int, in _Count int)
begin
	select * from TableMemberInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableMemberInfo;
delimiter $$
create procedure LoadSingleTableMemberInfo(
	in _MemberGuid bigint unsigned
)
begin
	select * from TableMemberInfo where IsValid = 1 
		and MemberGuid = _MemberGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadMultiTableMemberInfo;
delimiter $$
create procedure LoadMultiTableMemberInfo(
	in _UserGuid bigint unsigned
)
begin
	select * from TableMemberInfo where IsValid = 1 
		and UserGuid = _UserGuid 
		;
end $$
delimiter ;

drop procedure if exists LoadAllTableNicknameInfo;
delimiter $$
create procedure LoadAllTableNicknameInfo(in _Start int, in _Count int)
begin
	select * from TableNicknameInfo where IsValid = 1 limit _Start, _Count;
end $$
delimiter ;

drop procedure if exists LoadSingleTableNicknameInfo;
delimiter $$
create procedure LoadSingleTableNicknameInfo(
	in _Nickname varchar(32)
)
begin
	select * from TableNicknameInfo where IsValid = 1 
		and Nickname = _Nickname 
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
	in _AccountId varchar(64)
)
begin
	select * from TableUserInfo where IsValid = 1 
		and AccountId = _AccountId 
		;
end $$
delimiter ;

