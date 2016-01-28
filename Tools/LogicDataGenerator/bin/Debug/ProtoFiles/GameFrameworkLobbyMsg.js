//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkLobbyMsg.dsl生成！！！
//----------------------------------------------------------------------------

var c_Null = null;
var s_GetSubData = function(data, index, obj){
	if(data instanceof Array && data.length>index && null!=data[index] && data[index] instanceof Array){
		obj.fromJson(data[index]);
	}
};
var s_GetSubDataArray = function(data, index, obj, factory) {
	if(data instanceof Array && data.length>index && null!=data[index] && data[index] instanceof Array){
		var ct = data[index].length;
		for(var i=0;i<ct;++i){
			var val = factory();
			val.fromJson(data[index]);
			obj.push(val);
		}
	}
};
var s_AddSubData = function(data, obj) {
	if(null!=obj){
	  data.push(obj.toJson());
	} else {
	  data.push(obj);
	}
};
var s_AddSubDataArray = function(data, obj) {
	if(null!=obj){
		var subData = new Array();
		for(var i in obj){
			if(null!=obj[i]){
				subData.push(obj[i].toJson());
			} else {
				subData.push(obj[i]);
			}
		}
		data.push(subData);
	} else {
		data.push(obj);
	}
};

function AccountLogout(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function ArenaInfoMsg(){

	return {
		Rank : 0,
		Guid : 0,
		HeroId : 0,
		NickName : "",
		Level : 0,
		FightScore : 0,
		ActivePartners : new Array(),
		EquipInfo : new Array(),
		ActiveSkills : new Array(),
		FightParters : new Array(),
		LegacyAttr : new Array(),
		XSouls : new Array(),
		Talents : new Array(),
		Fashions : new Array(),

		fromJson : function(data){
			this.Rank = data[0];
			this.Guid = data[1];
			this.HeroId = data[2];
			this.NickName = data[3];
			this.Level = data[4];
			this.FightScore = data[5];
			s_GetSubDataArray(data, 6, this.ActivePartners, function(){return new PartnerDataMsg();});
			s_GetSubDataArray(data, 7, this.EquipInfo, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 8, this.ActiveSkills, function(){return new SkillDataInfo();});
			s_GetSubDataArray(data, 9, this.FightParters, function(){return new PartnerDataMsg();});
			s_GetSubDataArray(data, 10, this.LegacyAttr, function(){return new LegacyDataMsg();});
			s_GetSubDataArray(data, 11, this.XSouls, function(){return new XSoulDataMsg();});
			s_GetSubDataArray(data, 12, this.Talents, function(){return new JsonTalentDataMsg();});
			s_GetSubDataArray(data, 13, this.Fashions, function(){return new FashionSynMsg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Rank);
			data.push(this.Guid);
			data.push(this.HeroId);
			data.push(this.NickName);
			data.push(this.Level);
			data.push(this.FightScore);
			s_AddSubDataArray(data, this.ActivePartners);
			s_AddSubDataArray(data, this.EquipInfo);
			s_AddSubDataArray(data, this.ActiveSkills);
			s_AddSubDataArray(data, this.FightParters);
			s_AddSubDataArray(data, this.LegacyAttr);
			s_AddSubDataArray(data, this.XSouls);
			s_AddSubDataArray(data, this.Talents);
			s_AddSubDataArray(data, this.Fashions);
			return data;
		},
	};
}

function AuctionInfo(){

	return {
		auctionGuid : 0,
		userGuid : 0,
		userNickname : "",
		itemStatus : 0,
		statusLeftTime : 0,
		itemType : 0,
		itemNum : 0,
		itemInfo : c_Null/*JsonItemDataMsg*/,
		price : 0,

		fromJson : function(data){
			this.auctionGuid = data[0];
			this.userGuid = data[1];
			this.userNickname = data[2];
			this.itemStatus = data[3];
			this.statusLeftTime = data[4];
			this.itemType = data[5];
			this.itemNum = data[6];
			this.itemInfo = new JsonItemDataMsg();
			s_GetSubData(data, 7, this.itemInfo);
			this.price = data[8];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.auctionGuid);
			data.push(this.userGuid);
			data.push(this.userNickname);
			data.push(this.itemStatus);
			data.push(this.statusLeftTime);
			data.push(this.itemType);
			data.push(this.itemNum);
			s_AddSubData(data, this.itemInfo);
			data.push(this.price);
			return data;
		},
	};
}

function ChallengeEntityData(){

	return {
		Guid : 0,
		HeroId : 0,
		Level : 0,
		Rank : 0,
		FightScore : 0,
		NickName : "",
		UserDamage : 0,
		PartnerDamage : new Array(),

		fromJson : function(data){
			this.Guid = data[0];
			this.HeroId = data[1];
			this.Level = data[2];
			this.Rank = data[3];
			this.FightScore = data[4];
			this.NickName = data[5];
			this.UserDamage = data[6];
			s_GetSubDataArray(data, 7, this.PartnerDamage, function(){return new DamageInfoData();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.HeroId);
			data.push(this.Level);
			data.push(this.Rank);
			data.push(this.FightScore);
			data.push(this.NickName);
			data.push(this.UserDamage);
			s_AddSubDataArray(data, this.PartnerDamage);
			return data;
		},
	};
}

function ChallengeInfoData(){

	return {
		Challenger : c_Null/*ChallengeEntityData*/,
		Target : c_Null/*ChallengeEntityData*/,
		IsChallengeSuccess : false,
		EndTime : 0,

		fromJson : function(data){
			this.Challenger = new ChallengeEntityData();
			s_GetSubData(data, 0, this.Challenger);
			this.Target = new ChallengeEntityData();
			s_GetSubData(data, 1, this.Target);
			this.IsChallengeSuccess = data[2];
			this.EndTime = data[3];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubData(data, this.Challenger);
			s_AddSubData(data, this.Target);
			data.push(this.IsChallengeSuccess);
			data.push(this.EndTime);
			return data;
		},
	};
}

function ChatShieldInfoForMsg(){

	return {
		Guid : 0,
		Nickname : "",
		HeroId : 0,
		Level : 0,
		FightingScore : 0,
		IsOnline : false,

		fromJson : function(data){
			this.Guid = data[0];
			this.Nickname = data[1];
			this.HeroId = data[2];
			this.Level = data[3];
			this.FightingScore = data[4];
			this.IsOnline = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.Nickname);
			data.push(this.HeroId);
			data.push(this.Level);
			data.push(this.FightingScore);
			data.push(this.IsOnline);
			return data;
		},
	};
}

function DamageInfoData(){

	return {
		OwnerId : 0,
		Damage : 0,

		fromJson : function(data){
			this.OwnerId = data[0];
			this.Damage = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.OwnerId);
			data.push(this.Damage);
			return data;
		},
	};
}

function DirectLogin(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function FashionHideMsg(){

	return {
		m_IsWingHide : false,
		m_IsWeaponHide : false,
		m_IsClothHide : false,

		fromJson : function(data){
			this.m_IsWingHide = data[0];
			this.m_IsWeaponHide = data[1];
			this.m_IsClothHide = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_IsWingHide);
			data.push(this.m_IsWeaponHide);
			data.push(this.m_IsClothHide);
			return data;
		},
	};
}

function FashionMsg(){

	return {
		m_FsnId : 0,
		m_UseForever : false,
		m_DeadlineSeconds : 0,
		m_DressOn : false,

		fromJson : function(data){
			this.m_FsnId = data[0];
			this.m_UseForever = data[1];
			this.m_DeadlineSeconds = data[2];
			this.m_DressOn = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_FsnId);
			data.push(this.m_UseForever);
			data.push(this.m_DeadlineSeconds);
			data.push(this.m_DressOn);
			return data;
		},
	};
}

function FashionSynMsg(){

	return {
		FashionId : 0,
		IsHide : false,

		fromJson : function(data){
			this.FashionId = data[0];
			this.IsHide = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.FashionId);
			data.push(this.IsHide);
			return data;
		},
	};
}

function FightingScoreEntityMsg(){

	return {
		Guid : 0,
		HeroId : 0,
		NickName : "",
		Level : 0,
		FightingScore : 0,
		Rank : 0,

		fromJson : function(data){
			this.Guid = data[0];
			this.HeroId = data[1];
			this.NickName = data[2];
			this.Level = data[3];
			this.FightingScore = data[4];
			this.Rank = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.HeroId);
			data.push(this.NickName);
			data.push(this.Level);
			data.push(this.FightingScore);
			data.push(this.Rank);
			return data;
		},
	};
}

function FriendInfoForMsg(){

	return {
		Guid : 0,
		Nickname : "",
		HeroId : 0,
		Level : 0,
		FightingScore : 0,
		IsOnline : false,
		IsBlack : false,

		fromJson : function(data){
			this.Guid = data[0];
			this.Nickname = data[1];
			this.HeroId = data[2];
			this.Level = data[3];
			this.FightingScore = data[4];
			this.IsOnline = data[5];
			this.IsBlack = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.Nickname);
			data.push(this.HeroId);
			data.push(this.Level);
			data.push(this.FightingScore);
			data.push(this.IsOnline);
			data.push(this.IsBlack);
			return data;
		},
	};
}

function GetQueueingCount(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function GowDataMsg(){

	return {
		GowElo : 0,
		GowMatches : 0,
		GowWinMatches : 0,
		LeftMatchCount : 0,
		RankId : 0,
		Point : 0,
		CriticalTotalMatches : 0,
		CriticalAmassWinMatches : 0,
		CriticalAmassLossMatches : 0,
		IsAcquirePrize : false,

		fromJson : function(data){
			this.GowElo = data[0];
			this.GowMatches = data[1];
			this.GowWinMatches = data[2];
			this.LeftMatchCount = data[3];
			this.RankId = data[4];
			this.Point = data[5];
			this.CriticalTotalMatches = data[6];
			this.CriticalAmassWinMatches = data[7];
			this.CriticalAmassLossMatches = data[8];
			this.IsAcquirePrize = data[9];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.GowElo);
			data.push(this.GowMatches);
			data.push(this.GowWinMatches);
			data.push(this.LeftMatchCount);
			data.push(this.RankId);
			data.push(this.Point);
			data.push(this.CriticalTotalMatches);
			data.push(this.CriticalAmassWinMatches);
			data.push(this.CriticalAmassLossMatches);
			data.push(this.IsAcquirePrize);
			return data;
		},
	};
}

function ItemInfo_UseItem(){

	return {
		ItemID : 0,
		ItemGuid : 0,
		Num : 0,
		RandomProperty : 0,

		fromJson : function(data){
			this.ItemID = data[0];
			this.ItemGuid = data[1];
			this.Num = data[2];
			this.RandomProperty = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemID);
			data.push(this.ItemGuid);
			data.push(this.Num);
			data.push(this.RandomProperty);
			return data;
		},
	};
}

function ItemLeftMsg(){

	return {
		ItemGuid : 0,
		ItemNum : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
			this.ItemNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			data.push(this.ItemNum);
			return data;
		},
	};
}

function JsonItemDataMsg(){

	return {
		Guid : 0,
		ItemId : 0,
		Level : 0,
		Experience : 0,
		Num : 0,
		AppendProperty : 0,
		EnhanceStarLevel : 0,
		StrengthLevel : 0,
		StrengthFailCount : 0,
		IsCanTrade : false,

		fromJson : function(data){
			this.Guid = data[0];
			this.ItemId = data[1];
			this.Level = data[2];
			this.Experience = data[3];
			this.Num = data[4];
			this.AppendProperty = data[5];
			this.EnhanceStarLevel = data[6];
			this.StrengthLevel = data[7];
			this.StrengthFailCount = data[8];
			this.IsCanTrade = data[9];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.ItemId);
			data.push(this.Level);
			data.push(this.Experience);
			data.push(this.Num);
			data.push(this.AppendProperty);
			data.push(this.EnhanceStarLevel);
			data.push(this.StrengthLevel);
			data.push(this.StrengthFailCount);
			data.push(this.IsCanTrade);
			return data;
		},
	};
}

function JsonTalentDataMsg(){

	return {
		Slot : 0,
		ItemGuid : 0,
		ItemId : 0,
		Level : 0,
		Experience : 0,

		fromJson : function(data){
			this.Slot = data[0];
			this.ItemGuid = data[1];
			this.ItemId = data[2];
			this.Level = data[3];
			this.Experience = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Slot);
			data.push(this.ItemGuid);
			data.push(this.ItemId);
			data.push(this.Level);
			data.push(this.Experience);
			return data;
		},
	};
}

function KickUser(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function LegacyDataMsg(){

	return {
		ItemId : 0,
		Level : 0,
		AppendProperty : 0,
		IsUnlock : false,

		fromJson : function(data){
			this.ItemId = data[0];
			this.Level = data[1];
			this.AppendProperty = data[2];
			this.IsUnlock = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemId);
			data.push(this.Level);
			data.push(this.AppendProperty);
			data.push(this.IsUnlock);
			return data;
		},
	};
}

function Logout(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function LootEntityData(){

	return {
		Key : 0,
		Guid : 0,
		HeroId : 0,
		Level : 0,
		FightScore : 0,
		NickName : "",
		UserDamage : 0,
		DefenseOrder : new Array(),
		LootOrder : new Array(),

		fromJson : function(data){
			this.Key = data[0];
			this.Guid = data[1];
			this.HeroId = data[2];
			this.Level = data[3];
			this.FightScore = data[4];
			this.NickName = data[5];
			this.UserDamage = data[6];
			this.DefenseOrder = data[7];
			this.LootOrder = data[8];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Key);
			data.push(this.Guid);
			data.push(this.HeroId);
			data.push(this.Level);
			data.push(this.FightScore);
			data.push(this.NickName);
			data.push(this.UserDamage);
			data.push(this.DefenseOrder);
			data.push(this.LootOrder);
			return data;
		},
	};
}

function LootHistoryData(){

	return {
		DomainType : 0,
		Booty : 0,
		Looter : c_Null/*LootEntityData*/,
		IsLootSuccess : false,
		BeginTime : "",
		EndTime : "",

		fromJson : function(data){
			this.DomainType = data[0];
			this.Booty = data[1];
			this.Looter = new LootEntityData();
			s_GetSubData(data, 2, this.Looter);
			this.IsLootSuccess = data[3];
			this.BeginTime = data[4];
			this.EndTime = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.DomainType);
			data.push(this.Booty);
			s_AddSubData(data, this.Looter);
			data.push(this.IsLootSuccess);
			data.push(this.BeginTime);
			data.push(this.EndTime);
			return data;
		},
	};
}

function LootInfoMsg(){

	return {
		Key : 0,
		Guid : 0,
		HeroId : 0,
		NickName : "",
		Level : 0,
		FightScore : 0,
		IsOpen : false,
		IsGetAward : false,
		DomainType : 0,
		SessionStartTime : "",
		SessionEndTime : "",
		EquipInfo : new Array(),
		ActiveSkills : new Array(),
		FightParters : new Array(),
		LegacyAttr : new Array(),
		XSouls : new Array(),
		Talents : new Array(),
		FightOrder : new Array(),
		LootOrder : new Array(),
		FashionInfo : new Array(),
		Income : 0,
		Loss : 0,
		LootType : 0,

		fromJson : function(data){
			this.Key = data[0];
			this.Guid = data[1];
			this.HeroId = data[2];
			this.NickName = data[3];
			this.Level = data[4];
			this.FightScore = data[5];
			this.IsOpen = data[6];
			this.IsGetAward = data[7];
			this.DomainType = data[8];
			this.SessionStartTime = data[9];
			this.SessionEndTime = data[10];
			s_GetSubDataArray(data, 11, this.EquipInfo, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 12, this.ActiveSkills, function(){return new SkillDataInfo();});
			s_GetSubDataArray(data, 13, this.FightParters, function(){return new PartnerDataMsg();});
			s_GetSubDataArray(data, 14, this.LegacyAttr, function(){return new LegacyDataMsg();});
			s_GetSubDataArray(data, 15, this.XSouls, function(){return new XSoulDataMsg();});
			s_GetSubDataArray(data, 16, this.Talents, function(){return new JsonTalentDataMsg();});
			this.FightOrder = data[17];
			this.LootOrder = data[18];
			s_GetSubDataArray(data, 19, this.FashionInfo, function(){return new FashionSynMsg();});
			this.Income = data[20];
			this.Loss = data[21];
			this.LootType = data[22];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Key);
			data.push(this.Guid);
			data.push(this.HeroId);
			data.push(this.NickName);
			data.push(this.Level);
			data.push(this.FightScore);
			data.push(this.IsOpen);
			data.push(this.IsGetAward);
			data.push(this.DomainType);
			data.push(this.SessionStartTime);
			data.push(this.SessionEndTime);
			s_AddSubDataArray(data, this.EquipInfo);
			s_AddSubDataArray(data, this.ActiveSkills);
			s_AddSubDataArray(data, this.FightParters);
			s_AddSubDataArray(data, this.LegacyAttr);
			s_AddSubDataArray(data, this.XSouls);
			s_AddSubDataArray(data, this.Talents);
			data.push(this.FightOrder);
			data.push(this.LootOrder);
			s_AddSubDataArray(data, this.FashionInfo);
			data.push(this.Income);
			data.push(this.Loss);
			data.push(this.LootType);
			return data;
		},
	};
}

function MailInfoForMessage(){

	return {
		m_AlreadyRead : false,
		m_MailGuid : 0,
		m_Module : 0,
		m_Title : "",
		m_Sender : "",
		m_SendTime : "",
		m_Text : "",
		m_Items : new Array(),
		m_Money : 0,
		m_Gold : 0,
		m_Stamina : 0,

		fromJson : function(data){
			this.m_AlreadyRead = data[0];
			this.m_MailGuid = data[1];
			this.m_Module = data[2];
			this.m_Title = data[3];
			this.m_Sender = data[4];
			this.m_SendTime = data[5];
			this.m_Text = data[6];
			s_GetSubDataArray(data, 7, this.m_Items, function(){return new MailItemForMessage();});
			this.m_Money = data[8];
			this.m_Gold = data[9];
			this.m_Stamina = data[10];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_AlreadyRead);
			data.push(this.m_MailGuid);
			data.push(this.m_Module);
			data.push(this.m_Title);
			data.push(this.m_Sender);
			data.push(this.m_SendTime);
			data.push(this.m_Text);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Stamina);
			return data;
		},
	};
}

function MailItemForMessage(){

	return {
		m_ItemId : 0,
		m_ItemNum : 0,

		fromJson : function(data){
			this.m_ItemId = data[0];
			this.m_ItemNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemId);
			data.push(this.m_ItemNum);
			return data;
		},
	};
}

function MissionInfoForSync(){

	return {
		m_MissionId : 0,
		m_IsCompleted : false,
		m_Progress : "",

		fromJson : function(data){
			this.m_MissionId = data[0];
			this.m_IsCompleted = data[1];
			this.m_Progress = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MissionId);
			data.push(this.m_IsCompleted);
			data.push(this.m_Progress);
			return data;
		},
	};
}

function MorrowRewardInfo(){

	return {
		m_ActiveId : 0,
		m_IsActive : false,
		m_LeftSeconds : 0,
		m_CanGetReward : false,

		fromJson : function(data){
			this.m_ActiveId = data[0];
			this.m_IsActive = data[1];
			this.m_LeftSeconds = data[2];
			this.m_CanGetReward = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ActiveId);
			data.push(this.m_IsActive);
			data.push(this.m_LeftSeconds);
			data.push(this.m_CanGetReward);
			return data;
		},
	};
}

function Msg_CL_AcceptedDare(){

	return {
		m_ChallengerNickname : "",

		fromJson : function(data){
			this.m_ChallengerNickname = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ChallengerNickname);
			return data;
		},
	};
}

function Msg_CL_AccountLogin(){

	return {
		m_OpCode : 0,
		m_ChannelId : 0,
		m_Data : "",
		m_ClientGameVersion : "",
		m_ClientLoginIp : "",
		m_GameChannelId : "",
		m_UniqueIdentifier : "",
		m_System : "",

		fromJson : function(data){
			this.m_OpCode = data[0];
			this.m_ChannelId = data[1];
			this.m_Data = data[2];
			this.m_ClientGameVersion = data[3];
			this.m_ClientLoginIp = data[4];
			this.m_GameChannelId = data[5];
			this.m_UniqueIdentifier = data[6];
			this.m_System = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_OpCode);
			data.push(this.m_ChannelId);
			data.push(this.m_Data);
			data.push(this.m_ClientGameVersion);
			data.push(this.m_ClientLoginIp);
			data.push(this.m_GameChannelId);
			data.push(this.m_UniqueIdentifier);
			data.push(this.m_System);
			return data;
		},
	};
}

function Msg_CL_ActivateAccount(){

	return {
		m_ActivationCode : "",

		fromJson : function(data){
			this.m_ActivationCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ActivationCode);
			return data;
		},
	};
}

function Msg_CL_AddAssets(){

	return {
		m_Money : 0,
		m_Gold : 0,
		m_Exp : 0,
		m_Stamina : 0,

		fromJson : function(data){
			this.m_Money = data[0];
			this.m_Gold = data[1];
			this.m_Exp = data[2];
			this.m_Stamina = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Exp);
			data.push(this.m_Stamina);
			return data;
		},
	};
}

function Msg_CL_AddFriend(){

	return {
		m_TargetNick : "",
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetNick = data[0];
			this.m_TargetGuid = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetNick);
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_AddItem(){

	return {
		m_ItemId : 0,
		m_ItemNum : 0,

		fromJson : function(data){
			this.m_ItemId = data[0];
			this.m_ItemNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemId);
			data.push(this.m_ItemNum);
			return data;
		},
	};
}

function Msg_CL_AddTalentExperience(){

	return {
		Slot : 0,
		ItemGuid : new Array(),
		Result : 0,
		Experience : 0,

		fromJson : function(data){
			this.Slot = data[0];
			this.ItemGuid = data[1];
			this.Result = data[2];
			this.Experience = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Slot);
			data.push(this.ItemGuid);
			data.push(this.Result);
			data.push(this.Experience);
			return data;
		},
	};
}

function Msg_CL_AddXSoulExperience(){

	return {
		m_XSoulPart : 0,
		m_UseItemId : 0,
		m_ItemNum : 0,

		fromJson : function(data){
			this.m_XSoulPart = data[0];
			this.m_UseItemId = data[1];
			this.m_ItemNum = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_XSoulPart);
			data.push(this.m_UseItemId);
			data.push(this.m_ItemNum);
			return data;
		},
	};
}

function Msg_CL_ArenaBeginFight(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ArenaBuyFightCount(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ArenaChallengeOver(){

	return {
		IsSuccess : false,
		ChallengerDamage : 0,
		TargetDamage : 0,
		ChallengerPartnerDamage : new Array(),
		TargetPartnerDamage : new Array(),
		Sign : 0,

		fromJson : function(data){
			this.IsSuccess = data[0];
			this.ChallengerDamage = data[1];
			this.TargetDamage = data[2];
			s_GetSubDataArray(data, 3, this.ChallengerPartnerDamage, function(){return new DamageInfoData();});
			s_GetSubDataArray(data, 4, this.TargetPartnerDamage, function(){return new DamageInfoData();});
			this.Sign = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.IsSuccess);
			data.push(this.ChallengerDamage);
			data.push(this.TargetDamage);
			s_AddSubDataArray(data, this.ChallengerPartnerDamage);
			s_AddSubDataArray(data, this.TargetPartnerDamage);
			data.push(this.Sign);
			return data;
		},
	};
}

function Msg_CL_ArenaChangePartner(){

	return {
		Partners : new Array(),

		fromJson : function(data){
			this.Partners = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Partners);
			return data;
		},
	};
}

function Msg_CL_ArenaQueryHistory(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ArenaQueryRank(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ArenaStartChallenge(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_AuctionBuy(){

	return {
		AuctionGuid : 0,

		fromJson : function(data){
			this.AuctionGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.AuctionGuid);
			return data;
		},
	};
}

function Msg_CL_AuctionQuery(){

	return {
		Category1 : 0,
		Category2 : 0,
		AscPrice : false,
		ItemName : "",
		PageNo : 0,

		fromJson : function(data){
			this.Category1 = data[0];
			this.Category2 = data[1];
			this.AscPrice = data[2];
			this.ItemName = data[3];
			this.PageNo = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Category1);
			data.push(this.Category2);
			data.push(this.AscPrice);
			data.push(this.ItemName);
			data.push(this.PageNo);
			return data;
		},
	};
}

function Msg_CL_AuctionReceive(){

	return {
		AuctionGuid : 0,

		fromJson : function(data){
			this.AuctionGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.AuctionGuid);
			return data;
		},
	};
}

function Msg_CL_AuctionSelfAuction(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_AuctionSell(){

	return {
		ItemGuid : 0,
		ItemNum : 0,
		ItemType : 0,
		Price : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
			this.ItemNum = data[1];
			this.ItemType = data[2];
			this.Price = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			data.push(this.ItemNum);
			data.push(this.ItemType);
			data.push(this.Price);
			return data;
		},
	};
}

function Msg_CL_AuctionUnshelve(){

	return {
		AuctionGuid : 0,

		fromJson : function(data){
			this.AuctionGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.AuctionGuid);
			return data;
		},
	};
}

function Msg_CL_BuyEliteCount(){

	return {
		m_EliteId : 0,

		fromJson : function(data){
			this.m_EliteId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_EliteId);
			return data;
		},
	};
}

function Msg_CL_BuyFashion(){

	return {
		m_ItemID : 0,
		m_TimeType : 0,

		fromJson : function(data){
			this.m_ItemID = data[0];
			this.m_TimeType = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			data.push(this.m_TimeType);
			return data;
		},
	};
}

function Msg_CL_BuyLife(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_BuyPartnerCombatTicket(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_BuyStamina(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_BuyTDFightCount(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_CancelMatch(){

	return {
		m_SceneType : 0,

		fromJson : function(data){
			this.m_SceneType = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneType);
			return data;
		},
	};
}

function Msg_CL_CancelSelectPartner(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_CL_ChangeCityRoom(){

	return {
		m_SceneId : 0,
		m_RoomId : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_RoomId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_RoomId);
			return data;
		},
	};
}

function Msg_CL_ChangeFieldRoom(){

	return {
		m_SceneId : 0,
		m_RoomId : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_RoomId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_RoomId);
			return data;
		},
	};
}

function Msg_CL_ChangeScene(){

	return {
		m_SceneId : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			return data;
		},
	};
}

function Msg_CL_ChatAddShield(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_ChatAddShieldByName(){

	return {
		m_TargetNickName : "",

		fromJson : function(data){
			this.m_TargetNickName = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetNickName);
			return data;
		},
	};
}

function Msg_CL_ChatDelShield(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_CombinTalentCard(){

	return {
		m_PartGuid : new Array(),
		m_ItemNum : new Array(),

		fromJson : function(data){
			this.m_PartGuid = data[0];
			this.m_ItemNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartGuid);
			data.push(this.m_ItemNum);
			return data;
		},
	};
}

function Msg_CL_CombinTalentCardResult(){

	return {
		m_PartGuid : new Array(),
		m_ItemNum : new Array(),
		m_Result : 0,
		m_gainItemId : 0,

		fromJson : function(data){
			this.m_PartGuid = data[0];
			this.m_ItemNum = data[1];
			this.m_Result = data[2];
			this.m_gainItemId = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartGuid);
			data.push(this.m_ItemNum);
			data.push(this.m_Result);
			data.push(this.m_gainItemId);
			return data;
		},
	};
}

function Msg_CL_CompoundEquip(){

	return {
		m_PartId : 0,

		fromJson : function(data){
			this.m_PartId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartId);
			return data;
		},
	};
}

function Msg_CL_CompoundPartner(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_CL_ConfirmFriend(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_ConfirmJoinGroup(){

	return {
		m_InviteeGuid : 0,
		m_GroupNick : "",

		fromJson : function(data){
			this.m_InviteeGuid = data[0];
			this.m_GroupNick = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_InviteeGuid);
			data.push(this.m_GroupNick);
			return data;
		},
	};
}

function Msg_CL_CorpsAgreeClaimer(){

	return {
		m_ClaimerGuid : 0,
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_ClaimerGuid = data[0];
			this.m_CorpsGuid = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ClaimerGuid);
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsAppoint(){

	return {
		m_TargetGuid : 0,
		m_Title : 0,
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_Title = data[1];
			this.m_CorpsGuid = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_Title);
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsClearRequest(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_CorpsCreate(){

	return {
		m_CorpsName : "",

		fromJson : function(data){
			this.m_CorpsName = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CorpsName);
			return data;
		},
	};
}

function Msg_CL_CorpsDissolve(){

	return {
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_CorpsGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsIndirectJoin(){

	return {
		m_UserGuid : 0,

		fromJson : function(data){
			this.m_UserGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_UserGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsJoin(){

	return {
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_CorpsGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsKickout(){

	return {
		m_TargetGuid : 0,
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_CorpsGuid = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsQuit(){

	return {
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_CorpsGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsRefuseClaimer(){

	return {
		m_ClaimerGuid : 0,
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_ClaimerGuid = data[0];
			this.m_CorpsGuid = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ClaimerGuid);
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_CorpsSignIn(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_CostVitality(){

	return {
		m_Key : 0,

		fromJson : function(data){
			this.m_Key = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Key);
			return data;
		},
	};
}

function Msg_CL_CreateNickname(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_CreateRole(){

	return {
		m_HeroId : 0,
		m_Nickname : "",

		fromJson : function(data){
			this.m_HeroId = data[0];
			this.m_Nickname = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_HeroId);
			data.push(this.m_Nickname);
			return data;
		},
	};
}

function Msg_CL_DeleteFriend(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_DiamondExtraBuyBox(){

	return {
		m_BoxPlace : 0,
		m_SceneId : 0,

		fromJson : function(data){
			this.m_BoxPlace = data[0];
			this.m_SceneId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_BoxPlace);
			data.push(this.m_SceneId);
			return data;
		},
	};
}

function Msg_CL_DiscardItem(){

	return {
		m_ItemGuid : new Array(),

		fromJson : function(data){
			this.m_ItemGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemGuid);
			return data;
		},
	};
}

function Msg_CL_DrawReward(){

	return {
		m_RewardType : 0,
		m_LotteryType : 0,
		m_Time : 0,

		fromJson : function(data){
			this.m_RewardType = data[0];
			this.m_LotteryType = data[1];
			this.m_Time = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RewardType);
			data.push(this.m_LotteryType);
			data.push(this.m_Time);
			return data;
		},
	};
}

function Msg_CL_EndPartnerBattle(){

	return {
		m_BattleResult : 0,
		m_MatchKey : 0,

		fromJson : function(data){
			this.m_BattleResult = data[0];
			this.m_MatchKey = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_BattleResult);
			data.push(this.m_MatchKey);
			return data;
		},
	};
}

function Msg_CL_EnterField(){

	return {
		m_SceneId : 0,
		m_RoomId : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_RoomId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_RoomId);
			return data;
		},
	};
}

function Msg_CL_EquipmentStrength(){

	return {
		m_ItemID : 0,
		m_IsProtected : false,

		fromJson : function(data){
			this.m_ItemID = data[0];
			this.m_IsProtected = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			data.push(this.m_IsProtected);
			return data;
		},
	};
}

function Msg_CL_EquipTalentCard(){

	return {
		ItemGuid : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			return data;
		},
	};
}

function Msg_CL_ExchangeGift(){

	return {
		m_GiftCode : "",

		fromJson : function(data){
			this.m_GiftCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_GiftCode);
			return data;
		},
	};
}

function Msg_CL_ExchangeGoods(){

	return {
		m_ExchangeId : 0,
		m_NpcId : 0,
		m_RequestRefresh : false,

		fromJson : function(data){
			this.m_ExchangeId = data[0];
			this.m_NpcId = data[1];
			this.m_RequestRefresh = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ExchangeId);
			data.push(this.m_NpcId);
			data.push(this.m_RequestRefresh);
			return data;
		},
	};
}

function Msg_CL_ExpeditionAward(){

	return {
		m_TollgateNum : 0,

		fromJson : function(data){
			this.m_TollgateNum = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TollgateNum);
			return data;
		},
	};
}

function Msg_CL_ExpeditionFailure(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ExpeditionReset(){

	return {
		m_Hp : 0,
		m_Mp : 0,
		m_Rage : 0,
		m_RequestNum : 0,
		m_IsReset : false,
		m_AllowCostGold : false,
		m_Timestamp : 0,

		fromJson : function(data){
			this.m_Hp = data[0];
			this.m_Mp = data[1];
			this.m_Rage = data[2];
			this.m_RequestNum = data[3];
			this.m_IsReset = data[4];
			this.m_AllowCostGold = data[5];
			this.m_Timestamp = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Rage);
			data.push(this.m_RequestNum);
			data.push(this.m_IsReset);
			data.push(this.m_AllowCostGold);
			data.push(this.m_Timestamp);
			return data;
		},
	};
}

function Msg_CL_ExpeditionSweep(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_FinishExpedition(){

	return {
		m_SceneId : 0,
		m_TollgateNum : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Rage : 0,
		m_PartnerId : 0,
		m_PartnerHpPer : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_TollgateNum = data[1];
			this.m_Hp = data[2];
			this.m_Mp = data[3];
			this.m_Rage = data[4];
			this.m_PartnerId = data[5];
			this.m_PartnerHpPer = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_TollgateNum);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Rage);
			data.push(this.m_PartnerId);
			data.push(this.m_PartnerHpPer);
			return data;
		},
	};
}

function Msg_CL_FinishMission(){

	return {
		m_MissionId : 0,

		fromJson : function(data){
			this.m_MissionId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MissionId);
			return data;
		},
	};
}

function Msg_CL_FriendList(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GainFirstPayReward(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GainVipReward(){

	return {
		m_VipLevel : 0,

		fromJson : function(data){
			this.m_VipLevel = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_VipLevel);
			return data;
		},
	};
}

function Msg_CL_GetGowStarList(){

	return {
		m_Start : 0,
		m_Count : 0,

		fromJson : function(data){
			this.m_Start = data[0];
			this.m_Count = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Start);
			data.push(this.m_Count);
			return data;
		},
	};
}

function Msg_CL_GetLoginLottery(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GetLootAward(){

	return {
		m_Key : 0,

		fromJson : function(data){
			this.m_Key = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Key);
			return data;
		},
	};
}

function Msg_CL_GetLootHistory(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GetMailList(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GetMorrowReward(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GetOnlineTimeReward(){

	return {
		m_Index : 0,

		fromJson : function(data){
			this.m_Index = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Index);
			return data;
		},
	};
}

function Msg_CL_GetQueueingCount(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_GmPay(){

	return {
		m_Diamonds : 0,

		fromJson : function(data){
			this.m_Diamonds = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Diamonds);
			return data;
		},
	};
}

function Msg_CL_GMResetDailyMissions(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_InteractivePrize(){

	return {
		m_ActorId : 0,
		m_LinkId : 0,
		m_StoryId : 0,

		fromJson : function(data){
			this.m_ActorId = data[0];
			this.m_LinkId = data[1];
			this.m_StoryId = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ActorId);
			data.push(this.m_LinkId);
			data.push(this.m_StoryId);
			return data;
		},
	};
}

function Msg_CL_ItemUseRequest(){

	return {
		m_ItemGuid : 0,

		fromJson : function(data){
			this.m_ItemGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemGuid);
			return data;
		},
	};
}

function Msg_CL_LiftSkill(){

	return {
		m_SkillId : 0,

		fromJson : function(data){
			this.m_SkillId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SkillId);
			return data;
		},
	};
}

function Msg_CL_LootMatchTarget(){

	return {
		m_Key : 0,

		fromJson : function(data){
			this.m_Key = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Key);
			return data;
		},
	};
}

function Msg_CL_MidasTouch(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_MountEquipment(){

	return {
		m_EquipGuid : 0,
		m_EquipPos : 0,

		fromJson : function(data){
			this.m_EquipGuid = data[0];
			this.m_EquipPos = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_EquipGuid);
			data.push(this.m_EquipPos);
			return data;
		},
	};
}

function Msg_CL_MountFashion(){

	return {
		m_FashionID : 0,

		fromJson : function(data){
			this.m_FashionID = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_FashionID);
			return data;
		},
	};
}

function Msg_CL_MountSkill(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_SlotPos : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_SlotPos = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_SlotPos);
			return data;
		},
	};
}

function Msg_CL_OpenCharpter(){

	return {
		CharpterId : 0,

		fromJson : function(data){
			this.CharpterId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.CharpterId);
			return data;
		},
	};
}

function Msg_CL_OpenDomain(){

	return {
		DomainType : 0,
		Key : 0,

		fromJson : function(data){
			this.DomainType = data[0];
			this.Key = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.DomainType);
			data.push(this.Key);
			return data;
		},
	};
}

function Msg_CL_OverLoot(){

	return {
		IsSuccess : false,
		LooterDamage : 0,
		DefenderDamage : 0,
		Sign : 0,

		fromJson : function(data){
			this.IsSuccess = data[0];
			this.LooterDamage = data[1];
			this.DefenderDamage = data[2];
			this.Sign = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.IsSuccess);
			data.push(this.LooterDamage);
			data.push(this.DefenderDamage);
			data.push(this.Sign);
			return data;
		},
	};
}

function Msg_CL_PartnerEquip(){

	return {
		m_PartnerId : 0,
		m_ItemGuid : 0,
		m_EquipIndex : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
			this.m_ItemGuid = data[1];
			this.m_EquipIndex = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			data.push(this.m_ItemGuid);
			data.push(this.m_EquipIndex);
			return data;
		},
	};
}

function Msg_CL_PinviteTeam(){

	return {
		m_FirstNick : "",
		m_SecondNick : "",
		m_FirstGuid : 0,
		m_SecondGuid : 0,

		fromJson : function(data){
			this.m_FirstNick = data[0];
			this.m_SecondNick = data[1];
			this.m_FirstGuid = data[2];
			this.m_SecondGuid = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_FirstNick);
			data.push(this.m_SecondNick);
			data.push(this.m_FirstGuid);
			data.push(this.m_SecondGuid);
			return data;
		},
	};
}

function Msg_CL_PushDelayData(){

	return {
		m_RoundtripTime : 0,

		fromJson : function(data){
			this.m_RoundtripTime = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RoundtripTime);
			return data;
		},
	};
}

function Msg_CL_QueryArenaInfo(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_QueryArenaMatchGroup(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_QueryCorpsByName(){

	return {
		DimName : "",

		fromJson : function(data){
			this.DimName = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.DimName);
			return data;
		},
	};
}

function Msg_CL_QueryCorpsInfo(){

	return {
		m_CorpsGuid : 0,

		fromJson : function(data){
			this.m_CorpsGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CorpsGuid);
			return data;
		},
	};
}

function Msg_CL_QueryCorpsStar(){

	return {
		m_Start : 0,
		m_Count : 0,

		fromJson : function(data){
			this.m_Start = data[0];
			this.m_Count = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Start);
			data.push(this.m_Count);
			return data;
		},
	};
}

function Msg_CL_QueryExpeditionInfo(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_QueryFriendInfo(){

	return {
		m_QueryType : 0,
		m_TargetName : "",
		m_TargetLevel : 0,
		m_TargetScore : 0,
		m_TargetFortune : 0,
		m_TargetGender : 0,

		fromJson : function(data){
			this.m_QueryType = data[0];
			this.m_TargetName = data[1];
			this.m_TargetLevel = data[2];
			this.m_TargetScore = data[3];
			this.m_TargetFortune = data[4];
			this.m_TargetGender = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_QueryType);
			data.push(this.m_TargetName);
			data.push(this.m_TargetLevel);
			data.push(this.m_TargetScore);
			data.push(this.m_TargetFortune);
			data.push(this.m_TargetGender);
			return data;
		},
	};
}

function Msg_CL_QueryLootInfo(){

	return {
		m_Key : 0,

		fromJson : function(data){
			this.m_Key = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Key);
			return data;
		},
	};
}

function Msg_CL_QuerySkillInfos(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_QueryTDMatchGroup(){

	return {
		IsNext : false,

		fromJson : function(data){
			this.IsNext = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.IsNext);
			return data;
		},
	};
}

function Msg_CL_QueryValidCorpsList(){

	return {
		m_Start : 0,
		m_Count : 0,

		fromJson : function(data){
			this.m_Start = data[0];
			this.m_Count = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Start);
			data.push(this.m_Count);
			return data;
		},
	};
}

function Msg_CL_QuitGroup(){

	return {
		m_DropoutNick : "",

		fromJson : function(data){
			this.m_DropoutNick = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_DropoutNick);
			return data;
		},
	};
}

function Msg_CL_QuitPve(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_QuitRoom(){

	return {
		m_IsQuitRoom : false,

		fromJson : function(data){
			this.m_IsQuitRoom = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_IsQuitRoom);
			return data;
		},
	};
}

function Msg_CL_ReadMail(){

	return {
		m_MailGuid : 0,

		fromJson : function(data){
			this.m_MailGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MailGuid);
			return data;
		},
	};
}

function Msg_CL_ReceiveMail(){

	return {
		m_MailGuid : 0,

		fromJson : function(data){
			this.m_MailGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MailGuid);
			return data;
		},
	};
}

function Msg_CL_RecordNewbieFlag(){

	return {
		m_Bit : 0,

		fromJson : function(data){
			this.m_Bit = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Bit);
			return data;
		},
	};
}

function Msg_CL_RefreshPartnerCombat(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RefusedDare(){

	return {
		m_ChallengerNickname : "",

		fromJson : function(data){
			this.m_ChallengerNickname = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ChallengerNickname);
			return data;
		},
	};
}

function Msg_CL_RefuseGroupRequest(){

	return {
		m_RequesterGuid : 0,

		fromJson : function(data){
			this.m_RequesterGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RequesterGuid);
			return data;
		},
	};
}

function Msg_CL_RequestDare(){

	return {
		m_TargetNickname : "",

		fromJson : function(data){
			this.m_TargetNickname = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetNickname);
			return data;
		},
	};
}

function Msg_CL_RequestDareByGuid(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_RequestEnhanceEquipmentStar(){

	return {
		m_ItemID : 0,

		fromJson : function(data){
			this.m_ItemID = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			return data;
		},
	};
}

function Msg_CL_RequestExpedition(){

	return {
		m_SceneId : 0,
		m_TollgateNum : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_TollgateNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_TollgateNum);
			return data;
		},
	};
}

function Msg_CL_RequestGowBattleResult(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RequestGowPrize(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RequestGroupInfo(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RequestInvite(){

	return {
		m_InviteCode : "",

		fromJson : function(data){
			this.m_InviteCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_InviteCode);
			return data;
		},
	};
}

function Msg_CL_RequestInviteReward(){

	return {
		m_RewardId : 0,

		fromJson : function(data){
			this.m_RewardId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RewardId);
			return data;
		},
	};
}

function Msg_CL_RequestJoinGroup(){

	return {
		m_InviteeGuid : 0,
		m_GroupNick : "",

		fromJson : function(data){
			this.m_InviteeGuid = data[0];
			this.m_GroupNick = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_InviteeGuid);
			data.push(this.m_GroupNick);
			return data;
		},
	};
}

function Msg_CL_RequestMatch(){

	return {
		m_SceneType : 0,
		m_SceneDifficulty : 0,

		fromJson : function(data){
			this.m_SceneType = data[0];
			this.m_SceneDifficulty = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneType);
			data.push(this.m_SceneDifficulty);
			return data;
		},
	};
}

function Msg_CL_RequestMpveAward(){

	return {
		m_Type : 0,
		m_Index : 0,

		fromJson : function(data){
			this.m_Type = data[0];
			this.m_Index = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Type);
			data.push(this.m_Index);
			return data;
		},
	};
}

function Msg_CL_RequestPlayerDetail(){

	return {
		m_Nick : "",

		fromJson : function(data){
			this.m_Nick = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nick);
			return data;
		},
	};
}

function Msg_CL_RequestPlayerInfo(){

	return {
		m_Nick : "",

		fromJson : function(data){
			this.m_Nick = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nick);
			return data;
		},
	};
}

function Msg_CL_RequestRefreshExchange(){

	return {
		m_RequestRefresh : false,
		m_CurrencyId : 0,
		m_NpcId : 0,

		fromJson : function(data){
			this.m_RequestRefresh = data[0];
			this.m_CurrencyId = data[1];
			this.m_NpcId = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RequestRefresh);
			data.push(this.m_CurrencyId);
			data.push(this.m_NpcId);
			return data;
		},
	};
}

function Msg_CL_RequestSkillInfos(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RequestUserPosition(){

	return {
		m_User : 0,

		fromJson : function(data){
			this.m_User = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_User);
			return data;
		},
	};
}

function Msg_CL_RequestUsers(){

	return {
		m_Count : 0,
		m_AlreadyExists : new Array(),

		fromJson : function(data){
			this.m_Count = data[0];
			this.m_AlreadyExists = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Count);
			data.push(this.m_AlreadyExists);
			return data;
		},
	};
}

function Msg_CL_RequestVigor(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_RequireChatEquipInfo(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_RequireChatRoleInfo(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_CL_RequireChatShieldList(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_ResetCharpter(){

	return {
		CharpterId : 0,

		fromJson : function(data){
			this.CharpterId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.CharpterId);
			return data;
		},
	};
}

function Msg_CL_RoleEnter(){

	return {
		m_Guid : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			return data;
		},
	};
}

function Msg_CL_RoleList(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_SaveSkillPreset(){

	return {
		m_SelectedPresetIndex : 0,

		fromJson : function(data){
			this.m_SelectedPresetIndex = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SelectedPresetIndex);
			return data;
		},
	};
}

function Msg_CL_SecretAreaFightingInfo(){

	return {
		m_Difficulty : 0,
		m_Segment : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Finish : false,
		m_PartnerHp : 0,
		m_CheckNumber : 0,

		fromJson : function(data){
			this.m_Difficulty = data[0];
			this.m_Segment = data[1];
			this.m_Hp = data[2];
			this.m_Mp = data[3];
			this.m_Finish = data[4];
			this.m_PartnerHp = data[5];
			this.m_CheckNumber = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Difficulty);
			data.push(this.m_Segment);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Finish);
			data.push(this.m_PartnerHp);
			data.push(this.m_CheckNumber);
			return data;
		},
	};
}

function Msg_CL_SecretAreaTrial(){

	return {
		m_Difficulty : 0,
		m_Sweept : false,
		m_Refresh : false,

		fromJson : function(data){
			this.m_Difficulty = data[0];
			this.m_Sweept = data[1];
			this.m_Refresh = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Difficulty);
			data.push(this.m_Sweept);
			data.push(this.m_Refresh);
			return data;
		},
	};
}

function Msg_CL_SelectPartner(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_CL_SellItem(){

	return {
		ItemGuid : 0,
		ItemNum : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
			this.ItemNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			data.push(this.ItemNum);
			return data;
		},
	};
}

function Msg_CL_SendChat(){

	return {
		m_Type : 0,
		m_TargetNickName : "",
		m_Content : "",

		fromJson : function(data){
			this.m_Type = data[0];
			this.m_TargetNickName = data[1];
			this.m_Content = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Type);
			data.push(this.m_TargetNickName);
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_CL_SetCorpsNotice(){

	return {
		m_Content : "",

		fromJson : function(data){
			this.m_Content = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_CL_SetFashionShow(){

	return {
		m_FashionPartType : 0,
		m_IsHide : false,

		fromJson : function(data){
			this.m_FashionPartType = data[0];
			this.m_IsHide = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_FashionPartType);
			data.push(this.m_IsHide);
			return data;
		},
	};
}

function Msg_CL_SetNewbieActionFlag(){

	return {
		m_Bit : 0,
		m_Num : 0,

		fromJson : function(data){
			this.m_Bit = data[0];
			this.m_Num = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Bit);
			data.push(this.m_Num);
			return data;
		},
	};
}

function Msg_CL_SetNewbieFlag(){

	return {
		m_Bit : 0,
		m_Num : 0,

		fromJson : function(data){
			this.m_Bit = data[0];
			this.m_Num = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Bit);
			data.push(this.m_Num);
			return data;
		},
	};
}

function Msg_CL_SignInAndGetReward(){

	return {
		m_Guid : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			return data;
		},
	};
}

function Msg_CL_SinglePVE(){

	return {
		m_SceneType : 0,

		fromJson : function(data){
			this.m_SceneType = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneType);
			return data;
		},
	};
}

function Msg_CL_StageClear(){

	return {
		m_HitCount : 0,
		m_KillNpcCount : 0,
		m_MaxMultHitCount : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Gold : 0,
		m_MatchKey : 0,
		m_IsFitTime : false,

		fromJson : function(data){
			this.m_HitCount = data[0];
			this.m_KillNpcCount = data[1];
			this.m_MaxMultHitCount = data[2];
			this.m_Hp = data[3];
			this.m_Mp = data[4];
			this.m_Gold = data[5];
			this.m_MatchKey = data[6];
			this.m_IsFitTime = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_HitCount);
			data.push(this.m_KillNpcCount);
			data.push(this.m_MaxMultHitCount);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Gold);
			data.push(this.m_MatchKey);
			data.push(this.m_IsFitTime);
			return data;
		},
	};
}

function Msg_CL_StartCorpsTollgate(){

	return {
		CarpterId : 0,
		SceneId : 0,

		fromJson : function(data){
			this.CarpterId = data[0];
			this.SceneId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.CarpterId);
			data.push(this.SceneId);
			return data;
		},
	};
}

function Msg_CL_StartGame(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_StartLoot(){

	return {
		TargetKey : 0,
		SelfKey : 0,

		fromJson : function(data){
			this.TargetKey = data[0];
			this.SelfKey = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.TargetKey);
			data.push(this.SelfKey);
			return data;
		},
	};
}

function Msg_CL_StartMpve(){

	return {
		m_SceneType : 0,
		m_SceneDifficulty : 0,

		fromJson : function(data){
			this.m_SceneType = data[0];
			this.m_SceneDifficulty = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneType);
			data.push(this.m_SceneDifficulty);
			return data;
		},
	};
}

function Msg_CL_StartPartnerBattle(){

	return {
		m_IndexId : 0,

		fromJson : function(data){
			this.m_IndexId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_IndexId);
			return data;
		},
	};
}

function Msg_CL_StartTDChallenge(){

	return {
		Guid : 0,

		fromJson : function(data){
			this.Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			return data;
		},
	};
}

function Msg_CL_SwapSkill(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_SourcePos : 0,
		m_TargetPos : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_SourcePos = data[2];
			this.m_TargetPos = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_SourcePos);
			data.push(this.m_TargetPos);
			return data;
		},
	};
}

function Msg_CL_SweepStage(){

	return {
		m_SceneId : 0,
		m_SweepTime : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_SweepTime = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_SweepTime);
			return data;
		},
	};
}

function Msg_CL_TDBeginFight(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_TDChallengeOver(){

	return {
		IsSuccess : false,
		ChallengerDamage : 0,
		TargetDamage : 0,
		ChallengerPartnerDamage : new Array(),
		TargetPartnerDamage : new Array(),
		Sign : 0,

		fromJson : function(data){
			this.IsSuccess = data[0];
			this.ChallengerDamage = data[1];
			this.TargetDamage = data[2];
			s_GetSubDataArray(data, 3, this.ChallengerPartnerDamage, function(){return new DamageInfoData();});
			s_GetSubDataArray(data, 4, this.TargetPartnerDamage, function(){return new DamageInfoData();});
			this.Sign = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.IsSuccess);
			data.push(this.ChallengerDamage);
			data.push(this.TargetDamage);
			s_AddSubDataArray(data, this.ChallengerPartnerDamage);
			s_AddSubDataArray(data, this.TargetPartnerDamage);
			data.push(this.Sign);
			return data;
		},
	};
}

function Msg_CL_UnlockSkill(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			return data;
		},
	};
}

function Msg_CL_UnmountEquipment(){

	return {
		m_EquipPos : 0,

		fromJson : function(data){
			this.m_EquipPos = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_EquipPos);
			return data;
		},
	};
}

function Msg_CL_UnmountFashion(){

	return {
		m_FashionID : 0,

		fromJson : function(data){
			this.m_FashionID = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_FashionID);
			return data;
		},
	};
}

function Msg_CL_UnmountSkill(){

	return {
		m_PresetIndex : 0,
		m_SlotPos : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SlotPos = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SlotPos);
			return data;
		},
	};
}

function Msg_CL_UpdateActivityUnlock(){

	return {
		m_SceneId : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			return data;
		},
	};
}

function Msg_CL_UpdateFightingScore(){

	return {
		score : 0,

		fromJson : function(data){
			this.score = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.score);
			return data;
		},
	};
}

function Msg_CL_UpdatePosition(){

	return {
		m_X : 0,
		m_Z : 0,
		m_FaceDir : 0,

		fromJson : function(data){
			this.m_X = data[0];
			this.m_Z = data[1];
			this.m_FaceDir = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_X);
			data.push(this.m_Z);
			data.push(this.m_FaceDir);
			return data;
		},
	};
}

function Msg_CL_UpgradeEquipBatch(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_UpgradeItem(){

	return {
		m_Position : 0,
		m_ItemId : 0,
		m_AllowCostGold : false,

		fromJson : function(data){
			this.m_Position = data[0];
			this.m_ItemId = data[1];
			this.m_AllowCostGold = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Position);
			data.push(this.m_ItemId);
			data.push(this.m_AllowCostGold);
			return data;
		},
	};
}

function Msg_CL_UpgradeLegacy(){

	return {
		m_Index : 0,
		m_ItemID : 0,
		m_AllowCostGold : false,

		fromJson : function(data){
			this.m_Index = data[0];
			this.m_ItemID = data[1];
			this.m_AllowCostGold = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Index);
			data.push(this.m_ItemID);
			data.push(this.m_AllowCostGold);
			return data;
		},
	};
}

function Msg_CL_UpgradePartnerLevel(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_CL_UpgradePartnerStage(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_CL_UpgradeSkill(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_AllowCostGold : false,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_AllowCostGold = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_AllowCostGold);
			return data;
		},
	};
}

function Msg_CL_UploadFPS(){

	return {
		m_Fps : "",
		m_Nickname : "",

		fromJson : function(data){
			this.m_Fps = data[0];
			this.m_Nickname = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Fps);
			data.push(this.m_Nickname);
			return data;
		},
	};
}

function Msg_CL_WeeklyLoginReward(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CL_XSoulChangeShowModel(){

	return {
		m_XSoulPart : 0,
		m_ModelLevel : 0,

		fromJson : function(data){
			this.m_XSoulPart = data[0];
			this.m_ModelLevel = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_XSoulPart);
			data.push(this.m_ModelLevel);
			return data;
		},
	};
}

function Msg_CLC_CollectChapterAward(){

	return {
		Chapter : 0,
		OrderId : 0,
		AwardSign : 0,
		Result : 0,

		fromJson : function(data){
			this.Chapter = data[0];
			this.OrderId = data[1];
			this.AwardSign = data[2];
			this.Result = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Chapter);
			data.push(this.OrderId);
			data.push(this.AwardSign);
			data.push(this.Result);
			return data;
		},
	};
}

function Msg_CLC_CollectGrowthFund(){

	return {
		LevelIndex : 0,
		GrowthFundValue : 0,
		Result : 0,
		Diamond : 0,

		fromJson : function(data){
			this.LevelIndex = data[0];
			this.GrowthFundValue = data[1];
			this.Result = data[2];
			this.Diamond = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.LevelIndex);
			data.push(this.GrowthFundValue);
			data.push(this.Result);
			data.push(this.Diamond);
			return data;
		},
	};
}

function Msg_CLC_QueryFightingScoreRank(){

	return {
		RankEntities : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.RankEntities, function(){return new FightingScoreEntityMsg();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.RankEntities);
			return data;
		},
	};
}

function Msg_CLC_StoryMessage(){

	function MessageArg(){

		return {
			val_type : 0,
			str_val : "",

			fromJson : function(data){
				this.val_type = data[0];
				this.str_val = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.val_type);
				data.push(this.str_val);
				return data;
			},
		};
	}

	Msg_CLC_StoryMessage.MessageArg = MessageArg;

	return {
		m_MsgId : "",
		m_Args : new Array(),

		fromJson : function(data){
			this.m_MsgId = data[0];
			s_GetSubDataArray(data, 1, this.m_Args, function(){return new MessageArg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MsgId);
			s_AddSubDataArray(data, this.m_Args);
			return data;
		},
	};
}

function Msg_CLC_UnequipTalentCard(){

	return {
		Slot : 0,
		Result : 0,

		fromJson : function(data){
			this.Slot = data[0];
			this.Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Slot);
			data.push(this.Result);
			return data;
		},
	};
}

function Msg_LC_AccountLoginResult(){

	return {
		m_AccountId : "",
		m_Oid : "",
		m_Token : "",
		m_Result : 0,

		fromJson : function(data){
			this.m_AccountId = data[0];
			this.m_Oid = data[1];
			this.m_Token = data[2];
			this.m_Result = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_AccountId);
			data.push(this.m_Oid);
			data.push(this.m_Token);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_ActivateAccountResult(){

	return {
		m_Result : 0,

		fromJson : function(data){
			this.m_Result = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_AddAssetsResult(){

	return {
		m_Money : 0,
		m_Gold : 0,
		m_Exp : 0,
		m_Stamina : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_Money = data[0];
			this.m_Gold = data[1];
			this.m_Exp = data[2];
			this.m_Stamina = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Exp);
			data.push(this.m_Stamina);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_AddFriendResult(){

	return {
		m_TargetNick : "",
		m_FriendInfo : c_Null/*FriendInfoForMsg*/,
		m_Result : 0,

		fromJson : function(data){
			this.m_TargetNick = data[0];
			this.m_FriendInfo = new FriendInfoForMsg();
			s_GetSubData(data, 1, this.m_FriendInfo);
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetNick);
			s_AddSubData(data, this.m_FriendInfo);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_AddItemResult(){

	return {
		m_ItemGuid : 0,
		m_ItemId : 0,
		m_RandomProperty : 0,
		m_Result : 0,
		m_ItemCount : 0,
		m_Exp : 0,
		m_IsCanTrade : false,

		fromJson : function(data){
			this.m_ItemGuid = data[0];
			this.m_ItemId = data[1];
			this.m_RandomProperty = data[2];
			this.m_Result = data[3];
			this.m_ItemCount = data[4];
			this.m_Exp = data[5];
			this.m_IsCanTrade = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemGuid);
			data.push(this.m_ItemId);
			data.push(this.m_RandomProperty);
			data.push(this.m_Result);
			data.push(this.m_ItemCount);
			data.push(this.m_Exp);
			data.push(this.m_IsCanTrade);
			return data;
		},
	};
}

function Msg_LC_AddItemsResult(){

	function ItemInstance(){

		return {
			m_ItemGuid : 0,
			m_ItemId : 0,
			m_RandomProperty : 0,
			m_ItemCount : 0,
			m_Exp : 0,
			m_IsCanTrade : false,

			fromJson : function(data){
				this.m_ItemGuid = data[0];
				this.m_ItemId = data[1];
				this.m_RandomProperty = data[2];
				this.m_ItemCount = data[3];
				this.m_Exp = data[4];
				this.m_IsCanTrade = data[5];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_ItemGuid);
				data.push(this.m_ItemId);
				data.push(this.m_RandomProperty);
				data.push(this.m_ItemCount);
				data.push(this.m_Exp);
				data.push(this.m_IsCanTrade);
				return data;
			},
		};
	}

	Msg_LC_AddItemsResult.ItemInstance = ItemInstance;

	return {
		Items : new Array(),
		m_Result : 0,

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.Items, function(){return new ItemInstance();});
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.Items);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_AddTalentExperienceResult(){

	return {
		Slot : 0,
		ItemLefts : new Array(),
		Result : 0,
		Experience : 0,

		fromJson : function(data){
			this.Slot = data[0];
			s_GetSubDataArray(data, 1, this.ItemLefts, function(){return new ItemLeftMsg();});
			this.Result = data[2];
			this.Experience = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Slot);
			s_AddSubDataArray(data, this.ItemLefts);
			data.push(this.Result);
			data.push(this.Experience);
			return data;
		},
	};
}

function Msg_LC_AddXSoulExperienceResult(){

	return {
		m_XSoulPart : 0,
		m_UseItemId : 0,
		m_ItemNum : 0,
		m_Result : 0,
		m_Experience : 0,

		fromJson : function(data){
			this.m_XSoulPart = data[0];
			this.m_UseItemId = data[1];
			this.m_ItemNum = data[2];
			this.m_Result = data[3];
			this.m_Experience = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_XSoulPart);
			data.push(this.m_UseItemId);
			data.push(this.m_ItemNum);
			data.push(this.m_Result);
			data.push(this.m_Experience);
			return data;
		},
	};
}

function Msg_LC_ArenaBuyFightCountResult(){

	return {
		Result : 0,
		CurFightCount : 0,
		CurBuyTime : 0,

		fromJson : function(data){
			this.Result = data[0];
			this.CurFightCount = data[1];
			this.CurBuyTime = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.CurFightCount);
			data.push(this.CurBuyTime);
			return data;
		},
	};
}

function Msg_LC_ArenaChallengeResult(){

	return {
		m_ChallengeInfo : c_Null/*ChallengeInfoData*/,

		fromJson : function(data){
			this.m_ChallengeInfo = new ChallengeInfoData();
			s_GetSubData(data, 0, this.m_ChallengeInfo);
		},
		toJson : function(){
			var data = new Array();
			s_AddSubData(data, this.m_ChallengeInfo);
			return data;
		},
	};
}

function Msg_LC_ArenaChangePartnerResult(){

	return {
		Result : 0,
		Partners : new Array(),

		fromJson : function(data){
			this.Result = data[0];
			this.Partners = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.Partners);
			return data;
		},
	};
}

function Msg_LC_ArenaInfoResult(){

	return {
		m_ArenaInfo : c_Null/*ArenaInfoMsg*/,
		m_LeftBattleCount : 0,
		m_CurFightCountByTime : 0,
		m_BattleLeftCDTime : 0,

		fromJson : function(data){
			this.m_ArenaInfo = new ArenaInfoMsg();
			s_GetSubData(data, 0, this.m_ArenaInfo);
			this.m_LeftBattleCount = data[1];
			this.m_CurFightCountByTime = data[2];
			this.m_BattleLeftCDTime = data[3];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubData(data, this.m_ArenaInfo);
			data.push(this.m_LeftBattleCount);
			data.push(this.m_CurFightCountByTime);
			data.push(this.m_BattleLeftCDTime);
			return data;
		},
	};
}

function Msg_LC_ArenaMatchGroupResult(){

	function MatchGroupData(){

		return {
			One : c_Null/*ArenaInfoMsg*/,
			Two : c_Null/*ArenaInfoMsg*/,
			Three : c_Null/*ArenaInfoMsg*/,

			fromJson : function(data){
				this.One = new ArenaInfoMsg();
				s_GetSubData(data, 0, this.One);
				this.Two = new ArenaInfoMsg();
				s_GetSubData(data, 1, this.Two);
				this.Three = new ArenaInfoMsg();
				s_GetSubData(data, 2, this.Three);
			},
			toJson : function(){
				var data = new Array();
				s_AddSubData(data, this.One);
				s_AddSubData(data, this.Two);
				s_AddSubData(data, this.Three);
				return data;
			},
		};
	}

	Msg_LC_ArenaMatchGroupResult.MatchGroupData = MatchGroupData;

	return {
		m_MatchGroups : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_MatchGroups, function(){return new MatchGroupData();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_MatchGroups);
			return data;
		},
	};
}

function Msg_LC_ArenaQueryHistoryResult(){

	return {
		ChallengeHistory : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.ChallengeHistory, function(){return new ChallengeInfoData();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.ChallengeHistory);
			return data;
		},
	};
}

function Msg_LC_ArenaQueryRankResult(){

	return {
		RankMsg : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.RankMsg, function(){return new ArenaInfoMsg();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.RankMsg);
			return data;
		},
	};
}

function Msg_LC_ArenaStartCallengeResult(){

	return {
		m_TargetGuid : 0,
		m_Sign : 0,
		m_ResultCode : 0,
		m_Prime : 0,
		TargetInfo : c_Null/*ArenaInfoMsg*/,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_Sign = data[1];
			this.m_ResultCode = data[2];
			this.m_Prime = data[3];
			this.TargetInfo = new ArenaInfoMsg();
			s_GetSubData(data, 4, this.TargetInfo);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_Sign);
			data.push(this.m_ResultCode);
			data.push(this.m_Prime);
			s_AddSubData(data, this.TargetInfo);
			return data;
		},
	};
}

function Msg_LC_AuctionAuction(){

	return {
		auctionInfo : new Array(),
		IsLastPage : false,
		PageNo : 0,

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.auctionInfo, function(){return new AuctionInfo();});
			this.IsLastPage = data[1];
			this.PageNo = data[2];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.auctionInfo);
			data.push(this.IsLastPage);
			data.push(this.PageNo);
			return data;
		},
	};
}

function Msg_LC_AuctionOpResult(){

	return {
		result : 0,
		ItemGuid : 0,
		ItemNum : 0,
		ItemType : 0,
		Price : 0,
		itemInfo : c_Null/*JsonItemDataMsg*/,
		Cost : 0,

		fromJson : function(data){
			this.result = data[0];
			this.ItemGuid = data[1];
			this.ItemNum = data[2];
			this.ItemType = data[3];
			this.Price = data[4];
			this.itemInfo = new JsonItemDataMsg();
			s_GetSubData(data, 5, this.itemInfo);
			this.Cost = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.result);
			data.push(this.ItemGuid);
			data.push(this.ItemNum);
			data.push(this.ItemType);
			data.push(this.Price);
			s_AddSubData(data, this.itemInfo);
			data.push(this.Cost);
			return data;
		},
	};
}

function Msg_LC_AuctionSelfAuction(){

	return {
		auctionInfo : new Array(),
		IsAuctionOpen : false,

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.auctionInfo, function(){return new AuctionInfo();});
			this.IsAuctionOpen = data[1];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.auctionInfo);
			data.push(this.IsAuctionOpen);
			return data;
		},
	};
}

function Msg_LC_AwardFashionResult(){

	return {
		m_ItemID : 0,
		m_DeadlineSeconds : 0,
		m_DressOn : false,
		m_UseDays : 0,
		m_IsForever : false,

		fromJson : function(data){
			this.m_ItemID = data[0];
			this.m_DeadlineSeconds = data[1];
			this.m_DressOn = data[2];
			this.m_UseDays = data[3];
			this.m_IsForever = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			data.push(this.m_DeadlineSeconds);
			data.push(this.m_DressOn);
			data.push(this.m_UseDays);
			data.push(this.m_IsForever);
			return data;
		},
	};
}

function Msg_LC_BuyEliteCountResult(){

	return {
		m_EliteId : 0,
		m_ResultCode : 0,
		m_DiamondCount : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_EliteId = data[0];
			this.m_ResultCode = data[1];
			this.m_DiamondCount = data[2];
			this.m_GoldCash = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_EliteId);
			data.push(this.m_ResultCode);
			data.push(this.m_DiamondCount);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_BuyFashionResult(){

	return {
		m_Result : 0,
		m_ItemID : 0,
		m_TimeType : 0,
		m_DeadlineSeconds : 0,
		m_DressOn : false,
		m_Money : 0,
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_ItemID = data[1];
			this.m_TimeType = data[2];
			this.m_DeadlineSeconds = data[3];
			this.m_DressOn = data[4];
			this.m_Money = data[5];
			this.m_Gold = data[6];
			this.m_GoldCash = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_ItemID);
			data.push(this.m_TimeType);
			data.push(this.m_DeadlineSeconds);
			data.push(this.m_DressOn);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_BuyGoodsSucceed(){

	return {
		m_GoodsId : "",
		m_IsFirstPay : false,
		m_IsFirstBuyThis : false,
		m_VipLevel : 0,
		m_AccountId : "",
		m_OrderId : "",
		m_PayType : "",
		m_GoodsNum : 0,
		m_CurrencyType : "",

		fromJson : function(data){
			this.m_GoodsId = data[0];
			this.m_IsFirstPay = data[1];
			this.m_IsFirstBuyThis = data[2];
			this.m_VipLevel = data[3];
			this.m_AccountId = data[4];
			this.m_OrderId = data[5];
			this.m_PayType = data[6];
			this.m_GoodsNum = data[7];
			this.m_CurrencyType = data[8];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_GoodsId);
			data.push(this.m_IsFirstPay);
			data.push(this.m_IsFirstBuyThis);
			data.push(this.m_VipLevel);
			data.push(this.m_AccountId);
			data.push(this.m_OrderId);
			data.push(this.m_PayType);
			data.push(this.m_GoodsNum);
			data.push(this.m_CurrencyType);
			return data;
		},
	};
}

function Msg_LC_BuyLifeResult(){

	return {
		m_Succeed : false,
		m_CurDiamond : 0,
		m_CostReliveStone : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Succeed = data[0];
			this.m_CurDiamond = data[1];
			this.m_CostReliveStone = data[2];
			this.m_GoldCash = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Succeed);
			data.push(this.m_CurDiamond);
			data.push(this.m_CostReliveStone);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_BuyPartnerCombatTicketResult(){

	return {
		m_ResultCode : 0,
		m_CurDiamond : 0,
		m_RemainCount : 0,
		m_BuyCount : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_CurDiamond = data[1];
			this.m_RemainCount = data[2];
			this.m_BuyCount = data[3];
			this.m_GoldCash = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_CurDiamond);
			data.push(this.m_RemainCount);
			data.push(this.m_BuyCount);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_BuyStaminaResult(){

	return {
		m_Result : 0,
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Gold = data[1];
			this.m_GoldCash = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_BuyTDFightCountResult(){

	return {
		Result : 0,
		BuyCount : 0,
		CurFightCount : 0,

		fromJson : function(data){
			this.Result = data[0];
			this.BuyCount = data[1];
			this.CurFightCount = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.BuyCount);
			data.push(this.CurFightCount);
			return data;
		},
	};
}

function Msg_LC_CancelSelectPartnerResult(){

	return {
		m_ResultCode : 0,
		m_Partners : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_Partners = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_Partners);
			return data;
		},
	};
}

function Msg_LC_ChangeCaptain(){

	return {
		m_CreatorGuid : 0,

		fromJson : function(data){
			this.m_CreatorGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_CreatorGuid);
			return data;
		},
	};
}

function Msg_LC_ChatAddShieldResult(){

	return {
		m_Result : 0,
		m_TargetGuid : 0,
		m_TargetNickName : "",
		m_ShieldInfo : c_Null/*ChatShieldInfoForMsg*/,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_TargetGuid = data[1];
			this.m_TargetNickName = data[2];
			this.m_ShieldInfo = new ChatShieldInfoForMsg();
			s_GetSubData(data, 3, this.m_ShieldInfo);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_TargetGuid);
			data.push(this.m_TargetNickName);
			s_AddSubData(data, this.m_ShieldInfo);
			return data;
		},
	};
}

function Msg_LC_ChatDelShieldResult(){

	return {
		m_TargetGuid : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_ChatEquipInfoReturn(){

	return {
		m_TargetGuid : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			return data;
		},
	};
}

function Msg_LC_ChatResult(){

	return {
		m_Type : 0,
		m_SenderGuid : 0,
		m_SenderNickName : "",
		m_SenderHeroId : 0,
		m_TargetGuid : 0,
		m_TargetNickName : "",
		m_TargetHeroId : 0,
		m_Content : "",

		fromJson : function(data){
			this.m_Type = data[0];
			this.m_SenderGuid = data[1];
			this.m_SenderNickName = data[2];
			this.m_SenderHeroId = data[3];
			this.m_TargetGuid = data[4];
			this.m_TargetNickName = data[5];
			this.m_TargetHeroId = data[6];
			this.m_Content = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Type);
			data.push(this.m_SenderGuid);
			data.push(this.m_SenderNickName);
			data.push(this.m_SenderHeroId);
			data.push(this.m_TargetGuid);
			data.push(this.m_TargetNickName);
			data.push(this.m_TargetHeroId);
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_LC_ChatRoleInfoReturn(){

	return {
		m_TargetGuid : 0,
		m_TargetNickName : "",
		m_TargetLevel : 0,
		m_TargetPower : 0,
		m_IsShield : false,
		m_IsOnline : false,
		m_HeroId : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_TargetNickName = data[1];
			this.m_TargetLevel = data[2];
			this.m_TargetPower = data[3];
			this.m_IsShield = data[4];
			this.m_IsOnline = data[5];
			this.m_HeroId = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_TargetNickName);
			data.push(this.m_TargetLevel);
			data.push(this.m_TargetPower);
			data.push(this.m_IsShield);
			data.push(this.m_IsOnline);
			data.push(this.m_HeroId);
			return data;
		},
	};
}

function Msg_LC_ChatShieldListReturn(){

	return {
		m_ShieldInfoList : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_ShieldInfoList, function(){return new ChatShieldInfoForMsg();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_ShieldInfoList);
			return data;
		},
	};
}

function Msg_LC_ChatStatus(){

	return {
		m_Result : 0,

		fromJson : function(data){
			this.m_Result = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_ChatWorldResult(){

	return {
		m_SenderGuid : 0,
		m_SenderNickName : "",
		m_SenderHeroId : 0,
		m_Content : "",

		fromJson : function(data){
			this.m_SenderGuid = data[0];
			this.m_SenderNickName = data[1];
			this.m_SenderHeroId = data[2];
			this.m_Content = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SenderGuid);
			data.push(this.m_SenderNickName);
			data.push(this.m_SenderHeroId);
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_LC_CompoundEquipResult(){

	return {
		m_Result : 0,
		m_PartId : 0,
		m_ItemId : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_PartId = data[1];
			this.m_ItemId = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_PartId);
			data.push(this.m_ItemId);
			return data;
		},
	};
}

function Msg_LC_CompoundPartnerResult(){

	return {
		m_ResultCode : 0,
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_PartnerId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_LC_ConfirmJoinGroupResult(){

	return {
		m_Result : 0,
		m_Nick : "",

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Nick = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Nick);
			return data;
		},
	};
}

function Msg_LC_CorpsSignIn(){

	return {
		m_Stamina : 0,
		m_CorpsSignInState : false,

		fromJson : function(data){
			this.m_Stamina = data[0];
			this.m_CorpsSignInState = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Stamina);
			data.push(this.m_CorpsSignInState);
			return data;
		},
	};
}

function Msg_LC_CreateNicnameResult(){

	return {
		m_Nicknames : new Array(),

		fromJson : function(data){
			this.m_Nicknames = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nicknames);
			return data;
		},
	};
}

function Msg_LC_CreateRoleResult(){

	return {
		m_Result : 0,
		m_Nickname : "",
		m_HeroId : 0,
		m_Level : 0,
		m_UserGuid : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Nickname = data[1];
			this.m_HeroId = data[2];
			this.m_Level = data[3];
			this.m_UserGuid = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Nickname);
			data.push(this.m_HeroId);
			data.push(this.m_Level);
			data.push(this.m_UserGuid);
			return data;
		},
	};
}

function Msg_LC_DelFriendResult(){

	return {
		m_TargetGuid : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_DiamondExtraBuyBoxResult(){

	function AwardItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_DiamondExtraBuyBoxResult.AwardItemInfo = AwardItemInfo;

	return {
		m_BoxPlace : 0,
		m_Result : 0,
		m_Fresh : false,
		m_AddMoney : 0,
		m_AddGold : 0,
		m_Items : new Array(),
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_BoxPlace = data[0];
			this.m_Result = data[1];
			this.m_Fresh = data[2];
			this.m_AddMoney = data[3];
			this.m_AddGold = data[4];
			s_GetSubDataArray(data, 5, this.m_Items, function(){return new AwardItemInfo();});
			this.m_Gold = data[6];
			this.m_GoldCash = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_BoxPlace);
			data.push(this.m_Result);
			data.push(this.m_Fresh);
			data.push(this.m_AddMoney);
			data.push(this.m_AddGold);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_DiscardItemResult(){

	return {
		m_ItemGuids : new Array(),
		m_TotalIncome : 0,
		m_Gold : 0,
		m_Money : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_ItemGuids = data[0];
			this.m_TotalIncome = data[1];
			this.m_Gold = data[2];
			this.m_Money = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemGuids);
			data.push(this.m_TotalIncome);
			data.push(this.m_Gold);
			data.push(this.m_Money);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_DrawRewardResult(){

	function LotteryInfo(){

		return {
			m_Id : 0,
			m_CurFreeCount : 0,
			m_LastDrawTime : "",

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_CurFreeCount = data[1];
				this.m_LastDrawTime = data[2];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_CurFreeCount);
				data.push(this.m_LastDrawTime);
				return data;
			},
		};
	}

	Msg_LC_DrawRewardResult.LotteryInfo = LotteryInfo;

	return {
		m_Result : 0,
		m_Money : 0,
		m_Diamond : 0,
		m_RewardId : new Array(),
		m_Lottery : new Array(),
		m_LotteryType : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Money = data[1];
			this.m_Diamond = data[2];
			this.m_RewardId = data[3];
			s_GetSubDataArray(data, 4, this.m_Lottery, function(){return new LotteryInfo();});
			this.m_LotteryType = data[5];
			this.m_GoldCash = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Money);
			data.push(this.m_Diamond);
			data.push(this.m_RewardId);
			s_AddSubDataArray(data, this.m_Lottery);
			data.push(this.m_LotteryType);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_EndPartnerBattleResult(){

	return {
		m_ResultCode : 0,
		m_RemainCount : 0,
		m_FinishedCount : 0,
		m_BattleResult : 0,
		m_RewardItemId : 0,
		m_RewardItemNum : 0,
		m_Partners : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_RemainCount = data[1];
			this.m_FinishedCount = data[2];
			this.m_BattleResult = data[3];
			this.m_RewardItemId = data[4];
			this.m_RewardItemNum = data[5];
			this.m_Partners = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_RemainCount);
			data.push(this.m_FinishedCount);
			data.push(this.m_BattleResult);
			data.push(this.m_RewardItemId);
			data.push(this.m_RewardItemNum);
			data.push(this.m_Partners);
			return data;
		},
	};
}

function Msg_LC_EquipmentStrengthResult(){

	function DeleteItemInfo(){

		return {
			m_ItemGuid : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_ItemGuid = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_ItemGuid);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_EquipmentStrengthResult.DeleteItemInfo = DeleteItemInfo;

	return {
		m_Result : 0,
		m_ItemID : 0,
		m_NewStrengthLv : 0,
		m_OldStrengthLv : 0,
		m_DeductItem : false,
		m_IsProtected : false,
		m_Items : new Array(),
		m_StrengthFailCount : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_ItemID = data[1];
			this.m_NewStrengthLv = data[2];
			this.m_OldStrengthLv = data[3];
			this.m_DeductItem = data[4];
			this.m_IsProtected = data[5];
			s_GetSubDataArray(data, 6, this.m_Items, function(){return new DeleteItemInfo();});
			this.m_StrengthFailCount = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_ItemID);
			data.push(this.m_NewStrengthLv);
			data.push(this.m_OldStrengthLv);
			data.push(this.m_DeductItem);
			data.push(this.m_IsProtected);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_StrengthFailCount);
			return data;
		},
	};
}

function Msg_LC_EquipTalentCardResult(){

	return {
		ItemGuid : 0,
		OldItemGuid : 0,
		Result : 0,
		Slot : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
			this.OldItemGuid = data[1];
			this.Result = data[2];
			this.Slot = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			data.push(this.OldItemGuid);
			data.push(this.Result);
			data.push(this.Slot);
			return data;
		},
	};
}

function Msg_LC_ExchangeGiftResult(){

	function GiftItemInfo(){

		return {
			m_ItemId : 0,
			m_ItemNumber : 0,

			fromJson : function(data){
				this.m_ItemId = data[0];
				this.m_ItemNumber = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_ItemId);
				data.push(this.m_ItemNumber);
				return data;
			},
		};
	}

	Msg_LC_ExchangeGiftResult.GiftItemInfo = GiftItemInfo;

	return {
		m_GiftId : 0,
		m_Result : 0,
		m_GiftName : "",
		m_GiftDesc : "",
		m_GiftItems : new Array(),

		fromJson : function(data){
			this.m_GiftId = data[0];
			this.m_Result = data[1];
			this.m_GiftName = data[2];
			this.m_GiftDesc = data[3];
			s_GetSubDataArray(data, 4, this.m_GiftItems, function(){return new GiftItemInfo();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_GiftId);
			data.push(this.m_Result);
			data.push(this.m_GiftName);
			data.push(this.m_GiftDesc);
			s_AddSubDataArray(data, this.m_GiftItems);
			return data;
		},
	};
}

function Msg_LC_ExchangeGoodsResult(){

	return {
		m_ExchangeId : 0,
		m_NpcId : 0,
		m_ExchangeNum : 0,
		m_Result : 0,
		m_Refresh : false,
		m_Money : 0,
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_ExchangeId = data[0];
			this.m_NpcId = data[1];
			this.m_ExchangeNum = data[2];
			this.m_Result = data[3];
			this.m_Refresh = data[4];
			this.m_Money = data[5];
			this.m_Gold = data[6];
			this.m_GoldCash = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ExchangeId);
			data.push(this.m_NpcId);
			data.push(this.m_ExchangeNum);
			data.push(this.m_Result);
			data.push(this.m_Refresh);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_ExpeditionAwardResult(){

	function AwardItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_ExpeditionAwardResult.AwardItemInfo = AwardItemInfo;

	return {
		m_TollgateNum : 0,
		m_Items : new Array(),
		m_Result : 0,

		fromJson : function(data){
			this.m_TollgateNum = data[0];
			s_GetSubDataArray(data, 1, this.m_Items, function(){return new AwardItemInfo();});
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TollgateNum);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_ExpeditionResetResult(){

	function ExpeditionPartner(){

		return {
			Id : 0,
			Hp : 0,

			fromJson : function(data){
				this.Id = data[0];
				this.Hp = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.Id);
				data.push(this.Hp);
				return data;
			},
		};
	}

	function ImageDataMsg(){

		return {
			Guid : 0,
			HeroId : 0,
			Nickname : "",
			Level : 0,
			FightingScore : 0,
			EquipInfo : new Array(),
			SkillInfo : new Array(),
			LegacyInfo : new Array(),
			Partners : new Array(),
			XSouls : new Array(),
			Talents : new Array(),
			Fashions : new Array(),

			fromJson : function(data){
				this.Guid = data[0];
				this.HeroId = data[1];
				this.Nickname = data[2];
				this.Level = data[3];
				this.FightingScore = data[4];
				s_GetSubDataArray(data, 5, this.EquipInfo, function(){return new JsonItemDataMsg();});
				s_GetSubDataArray(data, 6, this.SkillInfo, function(){return new SkillDataInfo();});
				s_GetSubDataArray(data, 7, this.LegacyInfo, function(){return new LegacyDataMsg();});
				s_GetSubDataArray(data, 8, this.Partners, function(){return new PartnerDataMsg();});
				s_GetSubDataArray(data, 9, this.XSouls, function(){return new XSoulDataMsg();});
				s_GetSubDataArray(data, 10, this.Talents, function(){return new JsonTalentDataMsg();});
				s_GetSubDataArray(data, 11, this.Fashions, function(){return new FashionSynMsg();});
			},
			toJson : function(){
				var data = new Array();
				data.push(this.Guid);
				data.push(this.HeroId);
				data.push(this.Nickname);
				data.push(this.Level);
				data.push(this.FightingScore);
				s_AddSubDataArray(data, this.EquipInfo);
				s_AddSubDataArray(data, this.SkillInfo);
				s_AddSubDataArray(data, this.LegacyInfo);
				s_AddSubDataArray(data, this.Partners);
				s_AddSubDataArray(data, this.XSouls);
				s_AddSubDataArray(data, this.Talents);
				s_AddSubDataArray(data, this.Fashions);
				return data;
			},
		};
	}

	function TollgateDataForMsg(){

		return {
			Type : 0,
			IsFinish : false,
			IsAcceptedAward : new Array(),
			EnemyArray : new Array(),
			EnemyAttrArray : new Array(),
			UserImageArray : new Array(),

			fromJson : function(data){
				this.Type = data[0];
				this.IsFinish = data[1];
				this.IsAcceptedAward = data[2];
				this.EnemyArray = data[3];
				this.EnemyAttrArray = data[4];
				s_GetSubDataArray(data, 5, this.UserImageArray, function(){return new ImageDataMsg();});
			},
			toJson : function(){
				var data = new Array();
				data.push(this.Type);
				data.push(this.IsFinish);
				data.push(this.IsAcceptedAward);
				data.push(this.EnemyArray);
				data.push(this.EnemyAttrArray);
				s_AddSubDataArray(data, this.UserImageArray);
				return data;
			},
		};
	}

	Msg_LC_ExpeditionResetResult.ExpeditionPartner = ExpeditionPartner;
	Msg_LC_ExpeditionResetResult.ImageDataMsg = ImageDataMsg;
	Msg_LC_ExpeditionResetResult.TollgateDataForMsg = TollgateDataForMsg;

	return {
		m_Hp : 0,
		m_Mp : 0,
		m_Rage : 0,
		m_Schedule : 0,
		m_CurResetCount : 0,
		Tollgates : c_Null/*TollgateDataForMsg*/,
		m_AllowCostGold : false,
		m_IsUnlock : false,
		m_Result : 0,
		Partners : new Array(),
		LastAchievedSchedule : 0,

		fromJson : function(data){
			this.m_Hp = data[0];
			this.m_Mp = data[1];
			this.m_Rage = data[2];
			this.m_Schedule = data[3];
			this.m_CurResetCount = data[4];
			this.Tollgates = new TollgateDataForMsg();
			s_GetSubData(data, 5, this.Tollgates);
			this.m_AllowCostGold = data[6];
			this.m_IsUnlock = data[7];
			this.m_Result = data[8];
			s_GetSubDataArray(data, 9, this.Partners, function(){return new ExpeditionPartner();});
			this.LastAchievedSchedule = data[10];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Rage);
			data.push(this.m_Schedule);
			data.push(this.m_CurResetCount);
			s_AddSubData(data, this.Tollgates);
			data.push(this.m_AllowCostGold);
			data.push(this.m_IsUnlock);
			data.push(this.m_Result);
			s_AddSubDataArray(data, this.Partners);
			data.push(this.LastAchievedSchedule);
			return data;
		},
	};
}

function Msg_LC_ExpeditionSweepResult(){

	return {
		m_TollgateNum : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Rage : 0,
		m_Diamond : 0,
		m_Result : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_TollgateNum = data[0];
			this.m_Hp = data[1];
			this.m_Mp = data[2];
			this.m_Rage = data[3];
			this.m_Diamond = data[4];
			this.m_Result = data[5];
			this.m_GoldCash = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TollgateNum);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Rage);
			data.push(this.m_Diamond);
			data.push(this.m_Result);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_FinishExpeditionResult(){

	return {
		m_SceneId : 0,
		m_TollgateNum : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Rage : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_TollgateNum = data[1];
			this.m_Hp = data[2];
			this.m_Mp = data[3];
			this.m_Rage = data[4];
			this.m_Result = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_TollgateNum);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Rage);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_FinishMissionResult(){

	function MissionInfoForSync(){

		return {
			m_MissionId : 0,
			m_IsCompleted : false,
			m_Progress : "",

			fromJson : function(data){
				this.m_MissionId = data[0];
				this.m_IsCompleted = data[1];
				this.m_Progress = data[2];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_MissionId);
				data.push(this.m_IsCompleted);
				data.push(this.m_Progress);
				return data;
			},
		};
	}

	Msg_LC_FinishMissionResult.MissionInfoForSync = MissionInfoForSync;

	return {
		m_ResultCode : 0,
		m_FinishMissionId : 0,
		m_Gold : 0,
		m_Exp : 0,
		m_Diamond : 0,
		m_UnlockMissions : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_FinishMissionId = data[1];
			this.m_Gold = data[2];
			this.m_Exp = data[3];
			this.m_Diamond = data[4];
			s_GetSubDataArray(data, 5, this.m_UnlockMissions, function(){return new MissionInfoForSync();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_FinishMissionId);
			data.push(this.m_Gold);
			data.push(this.m_Exp);
			data.push(this.m_Diamond);
			s_AddSubDataArray(data, this.m_UnlockMissions);
			return data;
		},
	};
}

function Msg_LC_FriendOffline(){

	return {
		m_Guid : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			return data;
		},
	};
}

function Msg_LC_FriendOnline(){

	return {
		m_Guid : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			return data;
		},
	};
}

function Msg_LC_GainFirstPayRewardResult(){

	return {
		m_ResultCode : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			return data;
		},
	};
}

function Msg_LC_GainVipRewardResult(){

	return {
		m_ResultCode : 0,
		m_VipLevel : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_VipLevel = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_VipLevel);
			return data;
		},
	};
}

function Msg_LC_GetLoginLotteryResult(){

	return {
		m_ResultCode : 0,
		m_RewardId : 0,
		m_UsedLoginDrawLotteryCount : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_RewardId = data[1];
			this.m_UsedLoginDrawLotteryCount = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_RewardId);
			data.push(this.m_UsedLoginDrawLotteryCount);
			return data;
		},
	};
}

function Msg_LC_GetMorrowRewardResult(){

	return {
		m_ResultCode : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			return data;
		},
	};
}

function Msg_LC_GetOnlineTimeRewardResult(){

	return {
		m_ResultCode : 0,
		m_OnlineTime : 0,
		m_Index : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_OnlineTime = data[1];
			this.m_Index = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_OnlineTime);
			data.push(this.m_Index);
			return data;
		},
	};
}

function Msg_LC_GetPartner(){

	return {
		m_PartnerId : 0,

		fromJson : function(data){
			this.m_PartnerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PartnerId);
			return data;
		},
	};
}

function Msg_LC_GmCode(){

	return {
		m_Content : "",

		fromJson : function(data){
			this.m_Content = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_LC_GmPayResult(){

	return {
		m_Vip : 0,
		m_Diamonds : 0,
		m_DateTime : "",

		fromJson : function(data){
			this.m_Vip = data[0];
			this.m_Diamonds = data[1];
			this.m_DateTime = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Vip);
			data.push(this.m_Diamonds);
			data.push(this.m_DateTime);
			return data;
		},
	};
}

function Msg_LC_InteractivePrize(){

	return {
		m_ActorId : 0,
		m_LinkId : 0,
		m_StoryId : 0,
		m_IsValid : false,
		m_Ids : new Array(),
		m_Nums : new Array(),

		fromJson : function(data){
			this.m_ActorId = data[0];
			this.m_LinkId = data[1];
			this.m_StoryId = data[2];
			this.m_IsValid = data[3];
			this.m_Ids = data[4];
			this.m_Nums = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ActorId);
			data.push(this.m_LinkId);
			data.push(this.m_StoryId);
			data.push(this.m_IsValid);
			data.push(this.m_Ids);
			data.push(this.m_Nums);
			return data;
		},
	};
}

function Msg_LC_InviteInfoAfterRoleEnter(){

	return {
		m_InviteCode : "",
		m_IsInvited : false,
		m_InviteeCount : 0,
		m_OverLv30Count : 0,
		m_OverLv50Count : 0,
		m_RewardsHaveRecived : new Array(),

		fromJson : function(data){
			this.m_InviteCode = data[0];
			this.m_IsInvited = data[1];
			this.m_InviteeCount = data[2];
			this.m_OverLv30Count = data[3];
			this.m_OverLv50Count = data[4];
			this.m_RewardsHaveRecived = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_InviteCode);
			data.push(this.m_IsInvited);
			data.push(this.m_InviteeCount);
			data.push(this.m_OverLv30Count);
			data.push(this.m_OverLv50Count);
			data.push(this.m_RewardsHaveRecived);
			return data;
		},
	};
}

function Msg_LC_ItemsGiveBack(){

	function ItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_ItemsGiveBack.ItemInfo = ItemInfo;

	return {
		m_Items : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Items, function(){return new ItemInfo();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Items);
			return data;
		},
	};
}

function Msg_LC_ItemUseResult(){

	return {
		m_ResultCode : 0,
		m_ItemGuids : new Array(),
		m_nums : new Array(),
		m_Items : new Array(),
		m_arg : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_ItemGuids = data[1];
			this.m_nums = data[2];
			s_GetSubDataArray(data, 3, this.m_Items, function(){return new ItemInfo_UseItem();});
			this.m_arg = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_ItemGuids);
			data.push(this.m_nums);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_arg);
			return data;
		},
	};
}

function Msg_LC_LackOfSpace(){

	return {
		m_Succeed : false,
		m_Type : 0,
		m_ReceiveNum : 0,
		m_FreeNum : 0,
		m_MailGuid : 0,

		fromJson : function(data){
			this.m_Succeed = data[0];
			this.m_Type = data[1];
			this.m_ReceiveNum = data[2];
			this.m_FreeNum = data[3];
			this.m_MailGuid = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Succeed);
			data.push(this.m_Type);
			data.push(this.m_ReceiveNum);
			data.push(this.m_FreeNum);
			data.push(this.m_MailGuid);
			return data;
		},
	};
}

function Msg_LC_LiftSkillResult(){

	return {
		m_SkillID : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_SkillID = data[0];
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SkillID);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_LootChangeDefenseOrder(){

	return {
		Key : 0,
		Order : new Array(),

		fromJson : function(data){
			this.Key = data[0];
			this.Order = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Key);
			data.push(this.Order);
			return data;
		},
	};
}

function Msg_LC_LootChangeLootOrder(){

	return {
		Key : 0,
		Order : new Array(),

		fromJson : function(data){
			this.Key = data[0];
			this.Order = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Key);
			data.push(this.Order);
			return data;
		},
	};
}

function Msg_LC_LootCostVitality(){

	return {
		m_Result : 0,
		m_Vitality : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Vitality = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Vitality);
			return data;
		},
	};
}

function Msg_LC_LootMatchResult(){

	return {
		Result : 0,
		Money : 0,
		Income : 0,
		LootType : 0,
		IsShow : false,
		Target : c_Null/*LootInfoMsg*/,

		fromJson : function(data){
			this.Result = data[0];
			this.Money = data[1];
			this.Income = data[2];
			this.LootType = data[3];
			this.IsShow = data[4];
			this.Target = new LootInfoMsg();
			s_GetSubData(data, 5, this.Target);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.Money);
			data.push(this.Income);
			data.push(this.LootType);
			data.push(this.IsShow);
			s_AddSubData(data, this.Target);
			return data;
		},
	};
}

function Msg_LC_LootOpResult(){

	return {
		Result : 0,

		fromJson : function(data){
			this.Result = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			return data;
		},
	};
}

function Msg_LC_MatchResult(){

	return {
		m_Result : 0,
		m_UserLevel : new Array(),
		m_UserName : new Array(),
		m_UserHeroId : new Array(),
		m_LogicServerId : new Array(),

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_UserLevel = data[1];
			this.m_UserName = data[2];
			this.m_UserHeroId = data[3];
			this.m_LogicServerId = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_UserLevel);
			data.push(this.m_UserName);
			data.push(this.m_UserHeroId);
			data.push(this.m_LogicServerId);
			return data;
		},
	};
}

function Msg_LC_MidasTouchResult(){

	return {
		m_Count : 0,
		m_Money : 0,
		m_Gold : 0,
		m_GoldCash : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_Count = data[0];
			this.m_Money = data[1];
			this.m_Gold = data[2];
			this.m_GoldCash = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Count);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_MissionCompleted(){

	return {
		m_MissionId : 0,
		m_Progress : "",

		fromJson : function(data){
			this.m_MissionId = data[0];
			this.m_Progress = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MissionId);
			data.push(this.m_Progress);
			return data;
		},
	};
}

function Msg_LC_MorrowRewardActive(){

	return {
		m_ActiveIndex : 0,
		m_RemainTime : 0,

		fromJson : function(data){
			this.m_ActiveIndex = data[0];
			this.m_RemainTime = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ActiveIndex);
			data.push(this.m_RemainTime);
			return data;
		},
	};
}

function Msg_LC_MountEquipmentResult(){

	return {
		m_ItemGuid : 0,
		m_EquipPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_ItemGuid = data[0];
			this.m_EquipPos = data[1];
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemGuid);
			data.push(this.m_EquipPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_MountFashionResult(){

	return {
		m_Result : 0,
		m_FashionID : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_FashionID = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_FashionID);
			return data;
		},
	};
}

function Msg_LC_MountSkillResult(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_SlotPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_SlotPos = data[2];
			this.m_Result = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_SlotPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_MpveAwardResult(){

	function AwardItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_MpveAwardResult.AwardItemInfo = AwardItemInfo;

	return {
		m_SceneType : 0,
		m_Result : 0,
		m_AwardIndex : 0,
		m_AddMoney : 0,
		m_AddGold : 0,
		m_Items : new Array(),
		m_DareCount : 0,
		m_AwardCount : 0,

		fromJson : function(data){
			this.m_SceneType = data[0];
			this.m_Result = data[1];
			this.m_AwardIndex = data[2];
			this.m_AddMoney = data[3];
			this.m_AddGold = data[4];
			s_GetSubDataArray(data, 5, this.m_Items, function(){return new AwardItemInfo();});
			this.m_DareCount = data[6];
			this.m_AwardCount = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneType);
			data.push(this.m_Result);
			data.push(this.m_AwardIndex);
			data.push(this.m_AddMoney);
			data.push(this.m_AddGold);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_DareCount);
			data.push(this.m_AwardCount);
			return data;
		},
	};
}

function Msg_LC_MpveGeneralResult(){

	return {
		m_Result : 0,
		m_Nick : "",
		m_Type : 0,
		m_Difficulty : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Nick = data[1];
			this.m_Type = data[2];
			this.m_Difficulty = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Nick);
			data.push(this.m_Type);
			data.push(this.m_Difficulty);
			return data;
		},
	};
}

function Msg_LC_NoticeFashionOverdue(){

	return {
		m_ItemID : 0,

		fromJson : function(data){
			this.m_ItemID = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			return data;
		},
	};
}

function Msg_LC_NoticeFashionOverdueSoon(){

	return {
		m_ItemID : 0,

		fromJson : function(data){
			this.m_ItemID = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ItemID);
			return data;
		},
	};
}

function Msg_LC_NoticePlayerOffline(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_NoticeQuitGroup(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_NotifyMorrowReward(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_NotifyNewMail(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_OverLootResult(){

	return {
		DomainType : 0,
		Booty : 0,
		Looter : c_Null/*LootEntityData*/,
		Defender : c_Null/*LootEntityData*/,
		IsLootSuccess : false,
		EndTime : "",

		fromJson : function(data){
			this.DomainType = data[0];
			this.Booty = data[1];
			this.Looter = new LootEntityData();
			s_GetSubData(data, 2, this.Looter);
			this.Defender = new LootEntityData();
			s_GetSubData(data, 3, this.Defender);
			this.IsLootSuccess = data[4];
			this.EndTime = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.DomainType);
			data.push(this.Booty);
			s_AddSubData(data, this.Looter);
			s_AddSubData(data, this.Defender);
			data.push(this.IsLootSuccess);
			data.push(this.EndTime);
			return data;
		},
	};
}

function Msg_LC_PartnerCombatInfo(){

	return {
		m_Partners : new Array(),
		m_RemainCount : 0,
		m_FinishedCount : 0,
		m_BuyCount : 0,

		fromJson : function(data){
			this.m_Partners = data[0];
			this.m_RemainCount = data[1];
			this.m_FinishedCount = data[2];
			this.m_BuyCount = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Partners);
			data.push(this.m_RemainCount);
			data.push(this.m_FinishedCount);
			data.push(this.m_BuyCount);
			return data;
		},
	};
}

function Msg_LC_PartnerEquipResult(){

	return {
		m_ResultCode : 0,
		m_PartnerId : 0,
		m_ItemGuid : 0,
		m_PartnerEquipState : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_PartnerId = data[1];
			this.m_ItemGuid = data[2];
			this.m_PartnerEquipState = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_PartnerId);
			data.push(this.m_ItemGuid);
			data.push(this.m_PartnerEquipState);
			return data;
		},
	};
}

function Msg_LC_QueryCorpsByName(){

	return {
		m_Count : 0,
		m_List : new Array(),

		fromJson : function(data){
			this.m_Count = data[0];
			s_GetSubDataArray(data, 1, this.m_List, function(){return new Msg_LC_SyncCorpsInfo();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Count);
			s_AddSubDataArray(data, this.m_List);
			return data;
		},
	};
}

function Msg_LC_QueryFriendInfoResult(){

	return {
		m_Friends : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Friends, function(){return new FriendInfoForMsg();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Friends);
			return data;
		},
	};
}

function Msg_LC_QueryTDMatchGroupResult(){

	return {
		Result : 0,
		IsFull : false,
		LeftFightCount : 0,
		QueryCount : 0,
		BuyFightCount : 0,
		MatchGroup : new Array(),

		fromJson : function(data){
			this.Result = data[0];
			this.IsFull = data[1];
			this.LeftFightCount = data[2];
			this.QueryCount = data[3];
			this.BuyFightCount = data[4];
			s_GetSubDataArray(data, 5, this.MatchGroup, function(){return new TDMatchInfoMsg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.IsFull);
			data.push(this.LeftFightCount);
			data.push(this.QueryCount);
			data.push(this.BuyFightCount);
			s_AddSubDataArray(data, this.MatchGroup);
			return data;
		},
	};
}

function Msg_LC_QueueingCountResult(){

	return {
		m_QueueingCount : 0,

		fromJson : function(data){
			this.m_QueueingCount = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_QueueingCount);
			return data;
		},
	};
}

function Msg_LC_RefreshExchangeResult(){

	return {
		m_RequestRefreshResult : 0,
		m_RefreshNum : 0,
		m_CurrencyId : 0,
		m_NpcId : 0,
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_RequestRefreshResult = data[0];
			this.m_RefreshNum = data[1];
			this.m_CurrencyId = data[2];
			this.m_NpcId = data[3];
			this.m_Gold = data[4];
			this.m_GoldCash = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RequestRefreshResult);
			data.push(this.m_RefreshNum);
			data.push(this.m_CurrencyId);
			data.push(this.m_NpcId);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_RefreshPartnerCombatResult(){

	return {
		m_ResultCode : 0,
		m_Partners : new Array(),
		m_Gold : 0,
		m_Diamond : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_Partners = data[1];
			this.m_Gold = data[2];
			this.m_Diamond = data[3];
			this.m_GoldCash = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_Partners);
			data.push(this.m_Gold);
			data.push(this.m_Diamond);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_RequestDare(){

	return {
		m_ChallengerNickname : "",

		fromJson : function(data){
			this.m_ChallengerNickname = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ChallengerNickname);
			return data;
		},
	};
}

function Msg_LC_RequestDareResult(){

	return {
		m_Nickname : "",
		m_Result : 0,

		fromJson : function(data){
			this.m_Nickname = data[0];
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nickname);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_RequestEnhanceEquipmentStar(){

	function DeleteItemInfo(){

		return {
			m_ItemGuid : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_ItemGuid = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_ItemGuid);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_RequestEnhanceEquipmentStar.DeleteItemInfo = DeleteItemInfo;

	return {
		m_Result : 0,
		m_ItemID : 0,
		m_NewEnhanceLv : 0,
		m_Items : new Array(),

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_ItemID = data[1];
			this.m_NewEnhanceLv = data[2];
			s_GetSubDataArray(data, 3, this.m_Items, function(){return new DeleteItemInfo();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_ItemID);
			data.push(this.m_NewEnhanceLv);
			s_AddSubDataArray(data, this.m_Items);
			return data;
		},
	};
}

function Msg_LC_RequestExpeditionResult(){

	return {
		m_Guid : 0,
		m_ServerIp : "",
		m_ServerPort : 0,
		m_Key : 0,
		m_CampId : 0,
		m_SceneType : 0,
		m_ActiveTollgate : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
			this.m_ServerIp = data[1];
			this.m_ServerPort = data[2];
			this.m_Key = data[3];
			this.m_CampId = data[4];
			this.m_SceneType = data[5];
			this.m_ActiveTollgate = data[6];
			this.m_Result = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			data.push(this.m_ServerIp);
			data.push(this.m_ServerPort);
			data.push(this.m_Key);
			data.push(this.m_CampId);
			data.push(this.m_SceneType);
			data.push(this.m_ActiveTollgate);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_RequestGowPrizeResult(){

	function AwardItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	Msg_LC_RequestGowPrizeResult.AwardItemInfo = AwardItemInfo;

	return {
		m_Money : 0,
		m_Gold : 0,
		m_Items : new Array(),
		m_IsAcquirePrize : false,
		m_Result : 0,

		fromJson : function(data){
			this.m_Money = data[0];
			this.m_Gold = data[1];
			s_GetSubDataArray(data, 2, this.m_Items, function(){return new AwardItemInfo();});
			this.m_IsAcquirePrize = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Money);
			data.push(this.m_Gold);
			s_AddSubDataArray(data, this.m_Items);
			data.push(this.m_IsAcquirePrize);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_RequestInviteResult(){

	return {
		m_ResultCode : 0,
		m_RewardID : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_RewardID = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_RewardID);
			return data;
		},
	};
}

function Msg_LC_RequestInviteRewardResult(){

	return {
		m_ResultCode : 0,
		m_RewardId : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_RewardId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_RewardId);
			return data;
		},
	};
}

function Msg_LC_RequestItemUseResult(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_RequestJoinGroupResult(){

	return {
		m_Result : 0,
		m_Nick : "",

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Nick = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Nick);
			return data;
		},
	};
}

function Msg_LC_RequestUserPositionResult(){

	return {
		m_User : 0,
		m_X : 0,
		m_Z : 0,
		m_FaceDir : 0,

		fromJson : function(data){
			this.m_User = data[0];
			this.m_X = data[1];
			this.m_Z = data[2];
			this.m_FaceDir = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_User);
			data.push(this.m_X);
			data.push(this.m_Z);
			data.push(this.m_FaceDir);
			return data;
		},
	};
}

function Msg_LC_RequestUsersResult(){

	function UserInfo(){

		return {
			m_Guid : 0,
			m_HeroId : 0,
			m_Nick : "",
			m_X : 0,
			m_Z : 0,
			m_FaceDir : 0,
			m_XSoulItemId : 0,
			m_XSoulLevel : 0,
			m_XSoulExp : 0,
			m_XSoulShowLevel : 0,
			m_WingItemId : 0,
			m_WingLevel : 0,
			m_DressedFashionClothId : 0,
			m_DressedFashionWingId : 0,
			m_DressedFashionWeaponId : 0,

			fromJson : function(data){
				this.m_Guid = data[0];
				this.m_HeroId = data[1];
				this.m_Nick = data[2];
				this.m_X = data[3];
				this.m_Z = data[4];
				this.m_FaceDir = data[5];
				this.m_XSoulItemId = data[6];
				this.m_XSoulLevel = data[7];
				this.m_XSoulExp = data[8];
				this.m_XSoulShowLevel = data[9];
				this.m_WingItemId = data[10];
				this.m_WingLevel = data[11];
				this.m_DressedFashionClothId = data[12];
				this.m_DressedFashionWingId = data[13];
				this.m_DressedFashionWeaponId = data[14];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Guid);
				data.push(this.m_HeroId);
				data.push(this.m_Nick);
				data.push(this.m_X);
				data.push(this.m_Z);
				data.push(this.m_FaceDir);
				data.push(this.m_XSoulItemId);
				data.push(this.m_XSoulLevel);
				data.push(this.m_XSoulExp);
				data.push(this.m_XSoulShowLevel);
				data.push(this.m_WingItemId);
				data.push(this.m_WingLevel);
				data.push(this.m_DressedFashionClothId);
				data.push(this.m_DressedFashionWingId);
				data.push(this.m_DressedFashionWeaponId);
				return data;
			},
		};
	}

	Msg_LC_RequestUsersResult.UserInfo = UserInfo;

	return {
		m_Users : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Users, function(){return new UserInfo();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Users);
			return data;
		},
	};
}

function Msg_LC_ResetConsumeGoodsCount(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_ResetCorpsSignIn(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_ResetDailyMissions(){

	return {
		m_Missions : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Missions, function(){return new MissionInfoForSync();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Missions);
			return data;
		},
	};
}

function Msg_LC_ResetOnlineTimeRewardData(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_ResetWeeklyLoginRewardData(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_RoleEnterResult(){

	function ChapterAwardMsg(){

		return {
			ChapterId : 0,
			AwardValue : 0,

			fromJson : function(data){
				this.ChapterId = data[0];
				this.AwardValue = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.ChapterId);
				data.push(this.AwardValue);
				return data;
			},
		};
	}

	function DareData(){

		return {
			CharpterId : 0,
			CurDareCount : 0,

			fromJson : function(data){
				this.CharpterId = data[0];
				this.CurDareCount = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.CharpterId);
				data.push(this.CurDareCount);
				return data;
			},
		};
	}

	function ExchangeGoodsMsg(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	function ExchangeRefreshMsg(){

		return {
			m_CurrencyId : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_CurrencyId = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_CurrencyId);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	function GoodsPurchasedMsg(){

		return {
			m_GoodsId : "",
			m_GoodsCount : 0,

			fromJson : function(data){
				this.m_GoodsId = data[0];
				this.m_GoodsCount = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_GoodsId);
				data.push(this.m_GoodsCount);
				return data;
			},
		};
	}

	function LotteryInfo(){

		return {
			m_Id : 0,
			m_CurFreeCount : 0,
			m_LastDrawTime : "",

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_CurFreeCount = data[1];
				this.m_LastDrawTime = data[2];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_CurFreeCount);
				data.push(this.m_LastDrawTime);
				return data;
			},
		};
	}

	function MissionInfoForSync(){

		return {
			m_MissionId : 0,
			m_IsCompleted : false,
			m_Progress : "",

			fromJson : function(data){
				this.m_MissionId = data[0];
				this.m_IsCompleted = data[1];
				this.m_Progress = data[2];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_MissionId);
				data.push(this.m_IsCompleted);
				data.push(this.m_Progress);
				return data;
			},
		};
	}

	function ResetEliteCountInfo(){

		return {
			SceneId : 0,
			ResetCount : 0,

			fromJson : function(data){
				this.SceneId = data[0];
				this.ResetCount = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.SceneId);
				data.push(this.ResetCount);
				return data;
			},
		};
	}

	function SceneDataMsg(){

		return {
			m_SceneId : 0,
			m_Grade : 0,

			fromJson : function(data){
				this.m_SceneId = data[0];
				this.m_Grade = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_SceneId);
				data.push(this.m_Grade);
				return data;
			},
		};
	}

	function ScenesCompletedCountDataMsg(){

		return {
			m_SceneId : 0,
			m_Count : 0,

			fromJson : function(data){
				this.m_SceneId = data[0];
				this.m_Count = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_SceneId);
				data.push(this.m_Count);
				return data;
			},
		};
	}

	Msg_LC_RoleEnterResult.ChapterAwardMsg = ChapterAwardMsg;
	Msg_LC_RoleEnterResult.DareData = DareData;
	Msg_LC_RoleEnterResult.ExchangeGoodsMsg = ExchangeGoodsMsg;
	Msg_LC_RoleEnterResult.ExchangeRefreshMsg = ExchangeRefreshMsg;
	Msg_LC_RoleEnterResult.GoodsPurchasedMsg = GoodsPurchasedMsg;
	Msg_LC_RoleEnterResult.LotteryInfo = LotteryInfo;
	Msg_LC_RoleEnterResult.MissionInfoForSync = MissionInfoForSync;
	Msg_LC_RoleEnterResult.ResetEliteCountInfo = ResetEliteCountInfo;
	Msg_LC_RoleEnterResult.SceneDataMsg = SceneDataMsg;
	Msg_LC_RoleEnterResult.ScenesCompletedCountDataMsg = ScenesCompletedCountDataMsg;

	return {
		m_Result : 0,
		m_Money : 0,
		m_Gold : 0,
		m_GoldCash : 0,
		m_Stamina : 0,
		m_Exp : 0,
		m_Level : 0,
		m_CitySceneId : 0,
		m_BuyStaminaCount : 0,
		m_BuyMoneyCount : 0,
		m_CurSellItemGoldIncome : 0,
		m_Vip : 0,
		m_NewbieGuideScene : 0,
		m_Gow : c_Null/*GowDataMsg*/,
		m_NewbieGuides : new Array(),
		m_BagItems : new Array(),
		m_Equipments : new Array(),
		m_SkillInfo : new Array(),
		m_Missions : new Array(),
		m_Legacys : new Array(),
		m_SceneData : new Array(),
		m_SceneCompletedCountData : new Array(),
		m_Friends : new Array(),
		m_Partners : new Array(),
		m_ActivePartners : new Array(),
		m_XSouls : new Array(),
		m_Exchanges : new Array(),
		m_WorldId : 0,
		m_Vigor : 0,
		m_SignInCountCurMonth : 0,
		m_RestSignInCountCurDay : 0,
		m_NewbieFlag : new Array(),
		m_RefreshExchangeNum : new Array(),
		m_NewbieActionFlag : new Array(),
		m_IsGetWeeklyReward : false,
		m_WeeklyRewardRecord : new Array(),
		m_OnlineTimeRewardedIndex : new Array(),
		m_OnlineDuration : 0,
		m_GuideFlag : 0,
		m_EquipTalents : new Array(),
		m_CorpsId : 0,
		m_PaymentRebates : new Array(),
		m_MpveInfo : new Array(),
		m_GoodsInfo : new Array(),
		m_PurchasedDiamonds : 0,
		m_RewardedVipLevel : new Array(),
		m_IsRewardedFirstBuy : false,
		m_FashionInfo : new Array(),
		m_Lottery : new Array(),
		m_Vitality : 0,
		m_CorpsSignInState : false,
		m_PartnerCombatInfo : c_Null/*Msg_LC_PartnerCombatInfo*/,
		m_CorpsInfo : c_Null/*Msg_LC_SyncCorpsInfo*/,
		m_SecretAreaFought : 0,
		m_SecretAreaSegments : new Array(),
		m_SecretAreaFightNum : new Array(),
		m_SecretAreaHp : new Array(),
		m_SecretAreaMp : new Array(),
		m_MorrowRewardInfo : c_Null/*MorrowRewardInfo*/,
		m_FashionHide : c_Null/*FashionHideMsg*/,
		m_RecentLoginState : 0,
		m_SumLoginDayCount : 0,
		m_UsedLoginDrawLotteryCount : 0,
		m_UserAccountId : "",
		m_ResetEliteCount : new Array(),
		m_ExtraDiamondBox : new Array(),
		m_GrowthFundValue : 0,
		m_CorpsDareData : new Array(),
		m_ServerStartTime : "",
		m_ChapterAwardData : new Array(),
		m_SecretAreaPartnerHp : new Array(),

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Money = data[1];
			this.m_Gold = data[2];
			this.m_GoldCash = data[3];
			this.m_Stamina = data[4];
			this.m_Exp = data[5];
			this.m_Level = data[6];
			this.m_CitySceneId = data[7];
			this.m_BuyStaminaCount = data[8];
			this.m_BuyMoneyCount = data[9];
			this.m_CurSellItemGoldIncome = data[10];
			this.m_Vip = data[11];
			this.m_NewbieGuideScene = data[12];
			this.m_Gow = new GowDataMsg();
			s_GetSubData(data, 13, this.m_Gow);
			this.m_NewbieGuides = data[14];
			s_GetSubDataArray(data, 15, this.m_BagItems, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 16, this.m_Equipments, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 17, this.m_SkillInfo, function(){return new SkillDataInfo();});
			s_GetSubDataArray(data, 18, this.m_Missions, function(){return new MissionInfoForSync();});
			s_GetSubDataArray(data, 19, this.m_Legacys, function(){return new LegacyDataMsg();});
			s_GetSubDataArray(data, 20, this.m_SceneData, function(){return new SceneDataMsg();});
			s_GetSubDataArray(data, 21, this.m_SceneCompletedCountData, function(){return new ScenesCompletedCountDataMsg();});
			s_GetSubDataArray(data, 22, this.m_Friends, function(){return new FriendInfoForMsg();});
			s_GetSubDataArray(data, 23, this.m_Partners, function(){return new PartnerDataMsg();});
			this.m_ActivePartners = data[24];
			s_GetSubDataArray(data, 25, this.m_XSouls, function(){return new XSoulDataMsg();});
			s_GetSubDataArray(data, 26, this.m_Exchanges, function(){return new ExchangeGoodsMsg();});
			this.m_WorldId = data[27];
			this.m_Vigor = data[28];
			this.m_SignInCountCurMonth = data[29];
			this.m_RestSignInCountCurDay = data[30];
			this.m_NewbieFlag = data[31];
			s_GetSubDataArray(data, 32, this.m_RefreshExchangeNum, function(){return new ExchangeRefreshMsg();});
			this.m_NewbieActionFlag = data[33];
			this.m_IsGetWeeklyReward = data[34];
			this.m_WeeklyRewardRecord = data[35];
			this.m_OnlineTimeRewardedIndex = data[36];
			this.m_OnlineDuration = data[37];
			this.m_GuideFlag = data[38];
			s_GetSubDataArray(data, 39, this.m_EquipTalents, function(){return new JsonTalentDataMsg();});
			this.m_CorpsId = data[40];
			s_GetSubDataArray(data, 41, this.m_PaymentRebates, function(){return new PaymentRebateDataMsg();});
			s_GetSubDataArray(data, 42, this.m_MpveInfo, function(){return new Msg_LC_SyncMpveInfo();});
			s_GetSubDataArray(data, 43, this.m_GoodsInfo, function(){return new GoodsPurchasedMsg();});
			this.m_PurchasedDiamonds = data[44];
			this.m_RewardedVipLevel = data[45];
			this.m_IsRewardedFirstBuy = data[46];
			s_GetSubDataArray(data, 47, this.m_FashionInfo, function(){return new FashionMsg();});
			s_GetSubDataArray(data, 48, this.m_Lottery, function(){return new LotteryInfo();});
			this.m_Vitality = data[49];
			this.m_CorpsSignInState = data[50];
			this.m_PartnerCombatInfo = new Msg_LC_PartnerCombatInfo();
			s_GetSubData(data, 51, this.m_PartnerCombatInfo);
			this.m_CorpsInfo = new Msg_LC_SyncCorpsInfo();
			s_GetSubData(data, 52, this.m_CorpsInfo);
			this.m_SecretAreaFought = data[53];
			this.m_SecretAreaSegments = data[54];
			this.m_SecretAreaFightNum = data[55];
			this.m_SecretAreaHp = data[56];
			this.m_SecretAreaMp = data[57];
			this.m_MorrowRewardInfo = new MorrowRewardInfo();
			s_GetSubData(data, 58, this.m_MorrowRewardInfo);
			this.m_FashionHide = new FashionHideMsg();
			s_GetSubData(data, 59, this.m_FashionHide);
			this.m_RecentLoginState = data[60];
			this.m_SumLoginDayCount = data[61];
			this.m_UsedLoginDrawLotteryCount = data[62];
			this.m_UserAccountId = data[63];
			s_GetSubDataArray(data, 64, this.m_ResetEliteCount, function(){return new ResetEliteCountInfo();});
			this.m_ExtraDiamondBox = data[65];
			this.m_GrowthFundValue = data[66];
			s_GetSubDataArray(data, 67, this.m_CorpsDareData, function(){return new DareData();});
			this.m_ServerStartTime = data[68];
			s_GetSubDataArray(data, 69, this.m_ChapterAwardData, function(){return new ChapterAwardMsg();});
			this.m_SecretAreaPartnerHp = data[70];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			data.push(this.m_Stamina);
			data.push(this.m_Exp);
			data.push(this.m_Level);
			data.push(this.m_CitySceneId);
			data.push(this.m_BuyStaminaCount);
			data.push(this.m_BuyMoneyCount);
			data.push(this.m_CurSellItemGoldIncome);
			data.push(this.m_Vip);
			data.push(this.m_NewbieGuideScene);
			s_AddSubData(data, this.m_Gow);
			data.push(this.m_NewbieGuides);
			s_AddSubDataArray(data, this.m_BagItems);
			s_AddSubDataArray(data, this.m_Equipments);
			s_AddSubDataArray(data, this.m_SkillInfo);
			s_AddSubDataArray(data, this.m_Missions);
			s_AddSubDataArray(data, this.m_Legacys);
			s_AddSubDataArray(data, this.m_SceneData);
			s_AddSubDataArray(data, this.m_SceneCompletedCountData);
			s_AddSubDataArray(data, this.m_Friends);
			s_AddSubDataArray(data, this.m_Partners);
			data.push(this.m_ActivePartners);
			s_AddSubDataArray(data, this.m_XSouls);
			s_AddSubDataArray(data, this.m_Exchanges);
			data.push(this.m_WorldId);
			data.push(this.m_Vigor);
			data.push(this.m_SignInCountCurMonth);
			data.push(this.m_RestSignInCountCurDay);
			data.push(this.m_NewbieFlag);
			s_AddSubDataArray(data, this.m_RefreshExchangeNum);
			data.push(this.m_NewbieActionFlag);
			data.push(this.m_IsGetWeeklyReward);
			data.push(this.m_WeeklyRewardRecord);
			data.push(this.m_OnlineTimeRewardedIndex);
			data.push(this.m_OnlineDuration);
			data.push(this.m_GuideFlag);
			s_AddSubDataArray(data, this.m_EquipTalents);
			data.push(this.m_CorpsId);
			s_AddSubDataArray(data, this.m_PaymentRebates);
			s_AddSubDataArray(data, this.m_MpveInfo);
			s_AddSubDataArray(data, this.m_GoodsInfo);
			data.push(this.m_PurchasedDiamonds);
			data.push(this.m_RewardedVipLevel);
			data.push(this.m_IsRewardedFirstBuy);
			s_AddSubDataArray(data, this.m_FashionInfo);
			s_AddSubDataArray(data, this.m_Lottery);
			data.push(this.m_Vitality);
			data.push(this.m_CorpsSignInState);
			s_AddSubData(data, this.m_PartnerCombatInfo);
			s_AddSubData(data, this.m_CorpsInfo);
			data.push(this.m_SecretAreaFought);
			data.push(this.m_SecretAreaSegments);
			data.push(this.m_SecretAreaFightNum);
			data.push(this.m_SecretAreaHp);
			data.push(this.m_SecretAreaMp);
			s_AddSubData(data, this.m_MorrowRewardInfo);
			s_AddSubData(data, this.m_FashionHide);
			data.push(this.m_RecentLoginState);
			data.push(this.m_SumLoginDayCount);
			data.push(this.m_UsedLoginDrawLotteryCount);
			data.push(this.m_UserAccountId);
			s_AddSubDataArray(data, this.m_ResetEliteCount);
			data.push(this.m_ExtraDiamondBox);
			data.push(this.m_GrowthFundValue);
			s_AddSubDataArray(data, this.m_CorpsDareData);
			data.push(this.m_ServerStartTime);
			s_AddSubDataArray(data, this.m_ChapterAwardData);
			data.push(this.m_SecretAreaPartnerHp);
			return data;
		},
	};
}

function Msg_LC_RoleListResult(){

	function UserInfoForMessage(){

		return {
			m_Nickname : "",
			m_HeroId : 0,
			m_Level : 0,
			m_UserGuid : 0,

			fromJson : function(data){
				this.m_Nickname = data[0];
				this.m_HeroId = data[1];
				this.m_Level = data[2];
				this.m_UserGuid = data[3];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Nickname);
				data.push(this.m_HeroId);
				data.push(this.m_Level);
				data.push(this.m_UserGuid);
				return data;
			},
		};
	}

	Msg_LC_RoleListResult.UserInfoForMessage = UserInfoForMessage;

	return {
		m_Result : 0,
		m_UserInfoCount : 0,
		m_UserInfos : new Array(),

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_UserInfoCount = data[1];
			s_GetSubDataArray(data, 2, this.m_UserInfos, function(){return new UserInfoForMessage();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_UserInfoCount);
			s_AddSubDataArray(data, this.m_UserInfos);
			return data;
		},
	};
}

function Msg_LC_SecretAreaTrialAward(){

	return {
		m_AwardId : 0,
		m_Finish : false,

		fromJson : function(data){
			this.m_AwardId = data[0];
			this.m_Finish = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_AwardId);
			data.push(this.m_Finish);
			return data;
		},
	};
}

function Msg_LC_SecretAreaTrialResult(){

	return {
		m_Difficulty : 0,
		m_Result : 0,
		m_AlreadyFightNum : 0,
		m_Segments : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_AlreadyFought : 0,
		m_Sweept : false,
		m_Refresh : false,
		m_PartnerHp : 0,
		m_Prime : 0,
		m_CheckNumbers : new Array(),

		fromJson : function(data){
			this.m_Difficulty = data[0];
			this.m_Result = data[1];
			this.m_AlreadyFightNum = data[2];
			this.m_Segments = data[3];
			this.m_Hp = data[4];
			this.m_Mp = data[5];
			this.m_AlreadyFought = data[6];
			this.m_Sweept = data[7];
			this.m_Refresh = data[8];
			this.m_PartnerHp = data[9];
			this.m_Prime = data[10];
			this.m_CheckNumbers = data[11];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Difficulty);
			data.push(this.m_Result);
			data.push(this.m_AlreadyFightNum);
			data.push(this.m_Segments);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_AlreadyFought);
			data.push(this.m_Sweept);
			data.push(this.m_Refresh);
			data.push(this.m_PartnerHp);
			data.push(this.m_Prime);
			data.push(this.m_CheckNumbers);
			return data;
		},
	};
}

function Msg_LC_SelectPartnerResult(){

	return {
		m_ResultCode : 0,
		m_Partners : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_Partners = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_Partners);
			return data;
		},
	};
}

function Msg_LC_SellItemResult(){

	return {
		ItemGuid : 0,
		ItemNum : 0,
		Result : 0,
		Diamand : 0,
		Money : 0,
		TotalIncome : 0,
		GoldCash : 0,

		fromJson : function(data){
			this.ItemGuid = data[0];
			this.ItemNum = data[1];
			this.Result = data[2];
			this.Diamand = data[3];
			this.Money = data[4];
			this.TotalIncome = data[5];
			this.GoldCash = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemGuid);
			data.push(this.ItemNum);
			data.push(this.Result);
			data.push(this.Diamand);
			data.push(this.Money);
			data.push(this.TotalIncome);
			data.push(this.GoldCash);
			return data;
		},
	};
}

function Msg_LC_SendScreenTip(){

	return {
		m_Content : "",
		m_Align : 0,

		fromJson : function(data){
			this.m_Content = data[0];
			this.m_Align = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			data.push(this.m_Align);
			return data;
		},
	};
}

function Msg_LC_ServerShutdown(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_SetFashionShowResult(){

	return {
		m_Result : 0,
		m_FashionPartType : 0,
		m_IsHide : false,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_FashionPartType = data[1];
			this.m_IsHide = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_FashionPartType);
			data.push(this.m_IsHide);
			return data;
		},
	};
}

function Msg_LC_SignInAndGetRewardResult(){

	return {
		m_ResultCode : 0,
		m_RewardId : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_RewardId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_RewardId);
			return data;
		},
	};
}

function Msg_LC_StageClearResult(){

	function MissionInfoForSync(){

		return {
			m_MissionId : 0,
			m_IsCompleted : false,
			m_Progress : "",

			fromJson : function(data){
				this.m_MissionId = data[0];
				this.m_IsCompleted = data[1];
				this.m_Progress = data[2];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_MissionId);
				data.push(this.m_IsCompleted);
				data.push(this.m_Progress);
				return data;
			},
		};
	}

	function Teammate(){

		return {
			m_Nick : "",
			m_ResId : 0,
			m_Money : 0,
			m_TotalDamage : 0,
			m_ReliveTime : 0,
			m_HitCount : 0,
			m_Level : 0,
			m_HitCountFit : false,
			m_TimeFit : false,

			fromJson : function(data){
				this.m_Nick = data[0];
				this.m_ResId = data[1];
				this.m_Money = data[2];
				this.m_TotalDamage = data[3];
				this.m_ReliveTime = data[4];
				this.m_HitCount = data[5];
				this.m_Level = data[6];
				this.m_HitCountFit = data[7];
				this.m_TimeFit = data[8];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Nick);
				data.push(this.m_ResId);
				data.push(this.m_Money);
				data.push(this.m_TotalDamage);
				data.push(this.m_ReliveTime);
				data.push(this.m_HitCount);
				data.push(this.m_Level);
				data.push(this.m_HitCountFit);
				data.push(this.m_TimeFit);
				return data;
			},
		};
	}

	Msg_LC_StageClearResult.MissionInfoForSync = MissionInfoForSync;
	Msg_LC_StageClearResult.Teammate = Teammate;

	return {
		m_SceneId : 0,
		m_HitCount : 0,
		m_MaxMultHitCount : 0,
		m_Duration : 0,
		m_ItemId : 0,
		m_ItemCount : 0,
		m_ExpPoint : 0,
		m_Hp : 0,
		m_Mp : 0,
		m_Gold : 0,
		m_DeadCount : 0,
		m_CompletedRewardId : 0,
		m_SceneStarNum : 0,
		m_Missions : new Array(),
		m_KillNpcCount : 0,
		m_ResultCode : 0,
		m_Teammate : new Array(),
		m_Level : 0,
		m_BattleResult : 0,
		m_StageScore : 0,
		m_RewardItemIdList : new Array(),
		m_RewardItemNumList : new Array(),

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_HitCount = data[1];
			this.m_MaxMultHitCount = data[2];
			this.m_Duration = data[3];
			this.m_ItemId = data[4];
			this.m_ItemCount = data[5];
			this.m_ExpPoint = data[6];
			this.m_Hp = data[7];
			this.m_Mp = data[8];
			this.m_Gold = data[9];
			this.m_DeadCount = data[10];
			this.m_CompletedRewardId = data[11];
			this.m_SceneStarNum = data[12];
			s_GetSubDataArray(data, 13, this.m_Missions, function(){return new MissionInfoForSync();});
			this.m_KillNpcCount = data[14];
			this.m_ResultCode = data[15];
			s_GetSubDataArray(data, 16, this.m_Teammate, function(){return new Teammate();});
			this.m_Level = data[17];
			this.m_BattleResult = data[18];
			this.m_StageScore = data[19];
			this.m_RewardItemIdList = data[20];
			this.m_RewardItemNumList = data[21];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_HitCount);
			data.push(this.m_MaxMultHitCount);
			data.push(this.m_Duration);
			data.push(this.m_ItemId);
			data.push(this.m_ItemCount);
			data.push(this.m_ExpPoint);
			data.push(this.m_Hp);
			data.push(this.m_Mp);
			data.push(this.m_Gold);
			data.push(this.m_DeadCount);
			data.push(this.m_CompletedRewardId);
			data.push(this.m_SceneStarNum);
			s_AddSubDataArray(data, this.m_Missions);
			data.push(this.m_KillNpcCount);
			data.push(this.m_ResultCode);
			s_AddSubDataArray(data, this.m_Teammate);
			data.push(this.m_Level);
			data.push(this.m_BattleResult);
			data.push(this.m_StageScore);
			data.push(this.m_RewardItemIdList);
			data.push(this.m_RewardItemNumList);
			return data;
		},
	};
}

function Msg_LC_StartGameResult(){

	return {
		server_ip : "",
		server_port : 0,
		key : 0,
		camp_id : 0,
		scene_type : 0,
		match_key : 0,
		result : 0,
		prime : 0,

		fromJson : function(data){
			this.server_ip = data[0];
			this.server_port = data[1];
			this.key = data[2];
			this.camp_id = data[3];
			this.scene_type = data[4];
			this.match_key = data[5];
			this.result = data[6];
			this.prime = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.server_ip);
			data.push(this.server_port);
			data.push(this.key);
			data.push(this.camp_id);
			data.push(this.scene_type);
			data.push(this.match_key);
			data.push(this.result);
			data.push(this.prime);
			return data;
		},
	};
}

function Msg_LC_StartLootResult(){

	return {
		m_TargetGuid : 0,
		m_Sign : 0,
		m_ResultCode : 0,
		m_Prime : 0,

		fromJson : function(data){
			this.m_TargetGuid = data[0];
			this.m_Sign = data[1];
			this.m_ResultCode = data[2];
			this.m_Prime = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_TargetGuid);
			data.push(this.m_Sign);
			data.push(this.m_ResultCode);
			data.push(this.m_Prime);
			return data;
		},
	};
}

function Msg_LC_StartPartnerBattleResult(){

	return {
		m_ResultCode : 0,
		m_MatchKey : 0,
		m_HideAttrKey : 0,
		m_Index : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_MatchKey = data[1];
			this.m_HideAttrKey = data[2];
			this.m_Index = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_MatchKey);
			data.push(this.m_HideAttrKey);
			data.push(this.m_Index);
			return data;
		},
	};
}

function Msg_LC_StartTDChallengeResult(){

	return {
		Result : 0,
		IsFull : false,
		LeftFightCount : 0,
		QueryCount : 0,
		BuyFightCount : 0,
		Sign : 0,
		TargetInfo : c_Null/*ArenaInfoMsg*/,

		fromJson : function(data){
			this.Result = data[0];
			this.IsFull = data[1];
			this.LeftFightCount = data[2];
			this.QueryCount = data[3];
			this.BuyFightCount = data[4];
			this.Sign = data[5];
			this.TargetInfo = new ArenaInfoMsg();
			s_GetSubData(data, 6, this.TargetInfo);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			data.push(this.IsFull);
			data.push(this.LeftFightCount);
			data.push(this.QueryCount);
			data.push(this.BuyFightCount);
			data.push(this.Sign);
			s_AddSubData(data, this.TargetInfo);
			return data;
		},
	};
}

function Msg_LC_SwapSkillResult(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_SourcePos : 0,
		m_TargetPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_SourcePos = data[2];
			this.m_TargetPos = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_SourcePos);
			data.push(this.m_TargetPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_SweepStageResult(){

	return {
		m_SceneId : 0,
		m_ResultCode : 0,
		m_ItemInfo : new Array(),
		m_Exp : 0,
		m_Gold : 0,
		m_SweepItemCost : 0,

		fromJson : function(data){
			this.m_SceneId = data[0];
			this.m_ResultCode = data[1];
			s_GetSubDataArray(data, 2, this.m_ItemInfo, function(){return new JsonItemDataMsg();});
			this.m_Exp = data[3];
			this.m_Gold = data[4];
			this.m_SweepItemCost = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SceneId);
			data.push(this.m_ResultCode);
			s_AddSubDataArray(data, this.m_ItemInfo);
			data.push(this.m_Exp);
			data.push(this.m_Gold);
			data.push(this.m_SweepItemCost);
			return data;
		},
	};
}

function Msg_LC_SyncCombatData(){

	return {
		m_Legacys : new Array(),
		m_Skills : new Array(),
		m_XSouls : new Array(),
		m_Equipments : new Array(),
		m_PartnerDatas : new Array(),
		m_Fashions : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Legacys, function(){return new LegacyDataMsg();});
			s_GetSubDataArray(data, 1, this.m_Skills, function(){return new SkillDataInfo();});
			s_GetSubDataArray(data, 2, this.m_XSouls, function(){return new XSoulDataMsg();});
			s_GetSubDataArray(data, 3, this.m_Equipments, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 4, this.m_PartnerDatas, function(){return new SelectedPartnerDataMsg();});
			this.m_Fashions = data[5];
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Legacys);
			s_AddSubDataArray(data, this.m_Skills);
			s_AddSubDataArray(data, this.m_XSouls);
			s_AddSubDataArray(data, this.m_Equipments);
			s_AddSubDataArray(data, this.m_PartnerDatas);
			data.push(this.m_Fashions);
			return data;
		},
	};
}

function Msg_LC_SyncCorpsDareCount(){

	function DareData(){

		return {
			CharpterId : 0,
			CurDareCount : 0,

			fromJson : function(data){
				this.CharpterId = data[0];
				this.CurDareCount = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.CharpterId);
				data.push(this.CurDareCount);
				return data;
			},
		};
	}

	Msg_LC_SyncCorpsDareCount.DareData = DareData;

	return {
		Data : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.Data, function(){return new DareData();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.Data);
			return data;
		},
	};
}

function Msg_LC_SyncCorpsInfo(){

	function Charpter(){

		function BattleInfo(){

			return {
				SceneId : 0,
				IsFinish : false,
				Monsters : new Array(),
				Hps : new Array(),
				IsFighting : false,

				fromJson : function(data){
					this.SceneId = data[0];
					this.IsFinish = data[1];
					this.Monsters = data[2];
					this.Hps = data[3];
					this.IsFighting = data[4];
				},
				toJson : function(){
					var data = new Array();
					data.push(this.SceneId);
					data.push(this.IsFinish);
					data.push(this.Monsters);
					data.push(this.Hps);
					data.push(this.IsFighting);
					return data;
				},
			};
		}

		function TopInfo(){

			return {
				Guid : 0,
				HeroId : 0,
				Name : "",
				Level : 0,
				Damage : 0,

				fromJson : function(data){
					this.Guid = data[0];
					this.HeroId = data[1];
					this.Name = data[2];
					this.Level = data[3];
					this.Damage = data[4];
				},
				toJson : function(){
					var data = new Array();
					data.push(this.Guid);
					data.push(this.HeroId);
					data.push(this.Name);
					data.push(this.Level);
					data.push(this.Damage);
					return data;
				},
			};
		}

		Charpter.BattleInfo = BattleInfo;
		Charpter.TopInfo = TopInfo;

		return {
			Index : 0,
			IsOpen : false,
			Top : new Array(),
			Tollgates : new Array(),

			fromJson : function(data){
				this.Index = data[0];
				this.IsOpen = data[1];
				s_GetSubDataArray(data, 2, this.Top, function(){return new TopInfo();});
				s_GetSubDataArray(data, 3, this.Tollgates, function(){return new BattleInfo();});
			},
			toJson : function(){
				var data = new Array();
				data.push(this.Index);
				data.push(this.IsOpen);
				s_AddSubDataArray(data, this.Top);
				s_AddSubDataArray(data, this.Tollgates);
				return data;
			},
		};
	}

	function CorpsClaimer(){

		return {
			m_Guid : 0,
			m_HeroId : 0,
			m_Nickname : "",
			m_Level : 0,
			m_FightScore : 0,
			m_Time : "",
			m_CorpsId : 0,

			fromJson : function(data){
				this.m_Guid = data[0];
				this.m_HeroId = data[1];
				this.m_Nickname = data[2];
				this.m_Level = data[3];
				this.m_FightScore = data[4];
				this.m_Time = data[5];
				this.m_CorpsId = data[6];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Guid);
				data.push(this.m_HeroId);
				data.push(this.m_Nickname);
				data.push(this.m_Level);
				data.push(this.m_FightScore);
				data.push(this.m_Time);
				data.push(this.m_CorpsId);
				return data;
			},
		};
	}

	function CorpsMember(){

		return {
			m_Guid : 0,
			m_HeroId : 0,
			m_Nickname : "",
			m_Level : 0,
			m_FightScore : 0,
			m_Title : 0,
			m_DayActiveness : 0,
			m_WeekActiveness : 0,
			m_LastLoginTime : 0,
			m_IsOnline : false,

			fromJson : function(data){
				this.m_Guid = data[0];
				this.m_HeroId = data[1];
				this.m_Nickname = data[2];
				this.m_Level = data[3];
				this.m_FightScore = data[4];
				this.m_Title = data[5];
				this.m_DayActiveness = data[6];
				this.m_WeekActiveness = data[7];
				this.m_LastLoginTime = data[8];
				this.m_IsOnline = data[9];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Guid);
				data.push(this.m_HeroId);
				data.push(this.m_Nickname);
				data.push(this.m_Level);
				data.push(this.m_FightScore);
				data.push(this.m_Title);
				data.push(this.m_DayActiveness);
				data.push(this.m_WeekActiveness);
				data.push(this.m_LastLoginTime);
				data.push(this.m_IsOnline);
				return data;
			},
		};
	}

	Msg_LC_SyncCorpsInfo.Charpter = Charpter;
	Msg_LC_SyncCorpsInfo.CorpsClaimer = CorpsClaimer;
	Msg_LC_SyncCorpsInfo.CorpsMember = CorpsMember;

	return {
		m_Guid : 0,
		m_Name : "",
		m_Level : 0,
		m_Score : 0,
		m_Rank : 0,
		m_Members : new Array(),
		m_Confirms : new Array(),
		m_Notice : "",
		m_CreateTime : "",
		m_Activeness : 0,
		Charpters : new Array(),

		fromJson : function(data){
			this.m_Guid = data[0];
			this.m_Name = data[1];
			this.m_Level = data[2];
			this.m_Score = data[3];
			this.m_Rank = data[4];
			s_GetSubDataArray(data, 5, this.m_Members, function(){return new CorpsMember();});
			s_GetSubDataArray(data, 6, this.m_Confirms, function(){return new CorpsClaimer();});
			this.m_Notice = data[7];
			this.m_CreateTime = data[8];
			this.m_Activeness = data[9];
			s_GetSubDataArray(data, 10, this.Charpters, function(){return new Charpter();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			data.push(this.m_Name);
			data.push(this.m_Level);
			data.push(this.m_Score);
			data.push(this.m_Rank);
			s_AddSubDataArray(data, this.m_Members);
			s_AddSubDataArray(data, this.m_Confirms);
			data.push(this.m_Notice);
			data.push(this.m_CreateTime);
			data.push(this.m_Activeness);
			s_AddSubDataArray(data, this.Charpters);
			return data;
		},
	};
}

function Msg_LC_SyncCorpsOpResult(){

	return {
		m_Result : 0,
		m_Nickname : "",
		m_Gold : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Nickname = data[1];
			this.m_Gold = data[2];
			this.m_GoldCash = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Nickname);
			data.push(this.m_Gold);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_SyncCorpsStar(){

	return {
		m_Start : 0,
		m_Count : 0,
		m_Star : new Array(),

		fromJson : function(data){
			this.m_Start = data[0];
			this.m_Count = data[1];
			s_GetSubDataArray(data, 2, this.m_Star, function(){return new Msg_LC_SyncCorpsInfo();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Start);
			data.push(this.m_Count);
			s_AddSubDataArray(data, this.m_Star);
			return data;
		},
	};
}

function Msg_LC_SyncFightingScore(){

	return {
		Guid : 0,
		Score : 0,

		fromJson : function(data){
			this.Guid = data[0];
			this.Score = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.Score);
			return data;
		},
	};
}

function Msg_LC_SyncFriendList(){

	return {
		m_FriendInfo : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_FriendInfo, function(){return new FriendInfoForMsg();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_FriendInfo);
			return data;
		},
	};
}

function Msg_LC_SyncGoldTollgateInfo(){

	return {
		m_GoldCurAcceptedCount : 0,

		fromJson : function(data){
			this.m_GoldCurAcceptedCount = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_GoldCurAcceptedCount);
			return data;
		},
	};
}

function Msg_LC_SyncGowBattleResult(){

	return {
		m_Result : 0,
		m_OldGowElo : 0,
		m_GowElo : 0,
		m_OldPoint : 0,
		m_Point : 0,
		m_OldRankId : 0,
		m_RankId : 0,
		m_MaxMultiHitCount : 0,
		m_TotalDamage : 0,
		m_EnemyNick : "",
		m_EnemyHeroId : 0,
		m_EnemyOldGowElo : 0,
		m_EnemyGowElo : 0,
		m_EnemyOldPoint : 0,
		m_EnemyPoint : 0,
		m_EnemyMaxMultiHitCount : 0,
		m_EnemyTotalDamage : 0,
		m_SceneType : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_OldGowElo = data[1];
			this.m_GowElo = data[2];
			this.m_OldPoint = data[3];
			this.m_Point = data[4];
			this.m_OldRankId = data[5];
			this.m_RankId = data[6];
			this.m_MaxMultiHitCount = data[7];
			this.m_TotalDamage = data[8];
			this.m_EnemyNick = data[9];
			this.m_EnemyHeroId = data[10];
			this.m_EnemyOldGowElo = data[11];
			this.m_EnemyGowElo = data[12];
			this.m_EnemyOldPoint = data[13];
			this.m_EnemyPoint = data[14];
			this.m_EnemyMaxMultiHitCount = data[15];
			this.m_EnemyTotalDamage = data[16];
			this.m_SceneType = data[17];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_OldGowElo);
			data.push(this.m_GowElo);
			data.push(this.m_OldPoint);
			data.push(this.m_Point);
			data.push(this.m_OldRankId);
			data.push(this.m_RankId);
			data.push(this.m_MaxMultiHitCount);
			data.push(this.m_TotalDamage);
			data.push(this.m_EnemyNick);
			data.push(this.m_EnemyHeroId);
			data.push(this.m_EnemyOldGowElo);
			data.push(this.m_EnemyGowElo);
			data.push(this.m_EnemyOldPoint);
			data.push(this.m_EnemyPoint);
			data.push(this.m_EnemyMaxMultiHitCount);
			data.push(this.m_EnemyTotalDamage);
			data.push(this.m_SceneType);
			return data;
		},
	};
}

function Msg_LC_SyncGowOtherInfo(){

	return {
		m_WinNum : 0,
		m_LoseNum : 0,

		fromJson : function(data){
			this.m_WinNum = data[0];
			this.m_LoseNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_WinNum);
			data.push(this.m_LoseNum);
			return data;
		},
	};
}

function Msg_LC_SyncGowRankInfo(){

	return {
		m_Gow : c_Null/*GowDataMsg*/,

		fromJson : function(data){
			this.m_Gow = new GowDataMsg();
			s_GetSubData(data, 0, this.m_Gow);
		},
		toJson : function(){
			var data = new Array();
			s_AddSubData(data, this.m_Gow);
			return data;
		},
	};
}

function Msg_LC_SyncGowStarList(){

	function GowStarInfoForMessage(){

		return {
			m_Guid : 0,
			m_GowElo : 0,
			m_Nick : "",
			m_HeroId : 0,
			m_Level : 0,
			m_FightingScore : 0,
			m_RankId : 0,
			m_Point : 0,
			m_CriticalTotalMatches : 0,
			m_CriticalAmassWinMatches : 0,
			m_CriticalAmassLossMatches : 0,

			fromJson : function(data){
				this.m_Guid = data[0];
				this.m_GowElo = data[1];
				this.m_Nick = data[2];
				this.m_HeroId = data[3];
				this.m_Level = data[4];
				this.m_FightingScore = data[5];
				this.m_RankId = data[6];
				this.m_Point = data[7];
				this.m_CriticalTotalMatches = data[8];
				this.m_CriticalAmassWinMatches = data[9];
				this.m_CriticalAmassLossMatches = data[10];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Guid);
				data.push(this.m_GowElo);
				data.push(this.m_Nick);
				data.push(this.m_HeroId);
				data.push(this.m_Level);
				data.push(this.m_FightingScore);
				data.push(this.m_RankId);
				data.push(this.m_Point);
				data.push(this.m_CriticalTotalMatches);
				data.push(this.m_CriticalAmassWinMatches);
				data.push(this.m_CriticalAmassLossMatches);
				return data;
			},
		};
	}

	Msg_LC_SyncGowStarList.GowStarInfoForMessage = GowStarInfoForMessage;

	return {
		m_Stars : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Stars, function(){return new GowStarInfoForMessage();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Stars);
			return data;
		},
	};
}

function Msg_LC_SyncGroupUsers(){

	function UserInfoForGroup(){

		return {
			m_Guid : 0,
			m_HeroId : 0,
			m_Nick : "",
			m_Level : 0,
			m_FightingScore : 0,
			m_Status : 0,

			fromJson : function(data){
				this.m_Guid = data[0];
				this.m_HeroId = data[1];
				this.m_Nick = data[2];
				this.m_Level = data[3];
				this.m_FightingScore = data[4];
				this.m_Status = data[5];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Guid);
				data.push(this.m_HeroId);
				data.push(this.m_Nick);
				data.push(this.m_Level);
				data.push(this.m_FightingScore);
				data.push(this.m_Status);
				return data;
			},
		};
	}

	Msg_LC_SyncGroupUsers.UserInfoForGroup = UserInfoForGroup;

	return {
		m_Creator : 0,
		m_Count : 0,
		m_Members : new Array(),
		m_Confirms : new Array(),

		fromJson : function(data){
			this.m_Creator = data[0];
			this.m_Count = data[1];
			s_GetSubDataArray(data, 2, this.m_Members, function(){return new UserInfoForGroup();});
			s_GetSubDataArray(data, 3, this.m_Confirms, function(){return new UserInfoForGroup();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Creator);
			data.push(this.m_Count);
			s_AddSubDataArray(data, this.m_Members);
			s_AddSubDataArray(data, this.m_Confirms);
			return data;
		},
	};
}

function Msg_LC_SyncGuideFlag(){

	return {
		m_Flag : 0,

		fromJson : function(data){
			this.m_Flag = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Flag);
			return data;
		},
	};
}

function Msg_LC_SyncHomeNotice(){

	return {
		m_Content : "",

		fromJson : function(data){
			this.m_Content = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_LC_SyncLeaveGroup(){

	return {
		m_Result : 0,
		m_GroupNick : "",
		m_NeedTip : false,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_GroupNick = data[1];
			this.m_NeedTip = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_GroupNick);
			data.push(this.m_NeedTip);
			return data;
		},
	};
}

function Msg_LC_SyncLootAward(){

	return {
		ItemId : 0,
		ItemNum : 0,
		Result : 0,

		fromJson : function(data){
			this.ItemId = data[0];
			this.ItemNum = data[1];
			this.Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemId);
			data.push(this.ItemNum);
			data.push(this.Result);
			return data;
		},
	};
}

function Msg_LC_SyncLootHistory(){

	return {
		History : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.History, function(){return new LootHistoryData();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.History);
			return data;
		},
	};
}

function Msg_LC_SyncLootInfo(){

	return {
		Info : c_Null/*LootInfoMsg*/,

		fromJson : function(data){
			this.Info = new LootInfoMsg();
			s_GetSubData(data, 0, this.Info);
		},
		toJson : function(){
			var data = new Array();
			s_AddSubData(data, this.Info);
			return data;
		},
	};
}

function Msg_LC_SyncLotteryInfo(){

	return {
		m_Id : 0,
		m_CurFreeCount : 0,

		fromJson : function(data){
			this.m_Id = data[0];
			this.m_CurFreeCount = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Id);
			data.push(this.m_CurFreeCount);
			return data;
		},
	};
}

function Msg_LC_SyncMailList(){

	return {
		m_Mails : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Mails, function(){return new MailInfoForMessage();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Mails);
			return data;
		},
	};
}

function Msg_LC_SyncMpveInfo(){

	return {
		m_Type : 0,
		m_Difficulty : new Array(),
		m_DareCount : 0,
		m_AwardCount : 0,
		m_AwardIndex : new Array(),
		m_IsGet : new Array(),

		fromJson : function(data){
			this.m_Type = data[0];
			this.m_Difficulty = data[1];
			this.m_DareCount = data[2];
			this.m_AwardCount = data[3];
			this.m_AwardIndex = data[4];
			this.m_IsGet = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Type);
			data.push(this.m_Difficulty);
			data.push(this.m_DareCount);
			data.push(this.m_AwardCount);
			data.push(this.m_AwardIndex);
			data.push(this.m_IsGet);
			return data;
		},
	};
}

function Msg_LC_SyncNewbieActionFlag(){

	return {
		m_NewbieActionFlag : new Array(),

		fromJson : function(data){
			this.m_NewbieActionFlag = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_NewbieActionFlag);
			return data;
		},
	};
}

function Msg_LC_SyncNewbieFlag(){

	return {
		m_NewbieFlag : new Array(),

		fromJson : function(data){
			this.m_NewbieFlag = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_NewbieFlag);
			return data;
		},
	};
}

function Msg_LC_SyncNoticeContent(){

	return {
		m_Content : "",
		m_RollNum : 0,

		fromJson : function(data){
			this.m_Content = data[0];
			this.m_RollNum = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			data.push(this.m_RollNum);
			return data;
		},
	};
}

function Msg_LC_SyncPinviteTeam(){

	return {
		m_LeaderNick : "",
		m_Sponsor : "",

		fromJson : function(data){
			this.m_LeaderNick = data[0];
			this.m_Sponsor = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_LeaderNick);
			data.push(this.m_Sponsor);
			return data;
		},
	};
}

function Msg_LC_SyncPlayerDetail(){

	return {
		m_Nick : "",
		m_Level : 0,
		m_Score : 0,
		m_CorpsId : 0,
		m_VipLv : 0,
		EquipInfo : new Array(),
		Partners : new Array(),
		Talents : new Array(),
		m_HeroId : 0,
		FashonInfo : new Array(),
		FashionHide : c_Null/*FashionHideMsg*/,
		m_PartnerScore : 0,

		fromJson : function(data){
			this.m_Nick = data[0];
			this.m_Level = data[1];
			this.m_Score = data[2];
			this.m_CorpsId = data[3];
			this.m_VipLv = data[4];
			s_GetSubDataArray(data, 5, this.EquipInfo, function(){return new JsonItemDataMsg();});
			s_GetSubDataArray(data, 6, this.Partners, function(){return new PartnerDataMsg();});
			s_GetSubDataArray(data, 7, this.Talents, function(){return new JsonTalentDataMsg();});
			this.m_HeroId = data[8];
			s_GetSubDataArray(data, 9, this.FashonInfo, function(){return new FashionMsg();});
			this.FashionHide = new FashionHideMsg();
			s_GetSubData(data, 10, this.FashionHide);
			this.m_PartnerScore = data[11];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nick);
			data.push(this.m_Level);
			data.push(this.m_Score);
			data.push(this.m_CorpsId);
			data.push(this.m_VipLv);
			s_AddSubDataArray(data, this.EquipInfo);
			s_AddSubDataArray(data, this.Partners);
			s_AddSubDataArray(data, this.Talents);
			data.push(this.m_HeroId);
			s_AddSubDataArray(data, this.FashonInfo);
			s_AddSubData(data, this.FashionHide);
			data.push(this.m_PartnerScore);
			return data;
		},
	};
}

function Msg_LC_SyncPlayerInfo(){

	return {
		m_Nick : "",
		m_Level : 0,
		m_Score : 0,
		m_CorpsId : 0,

		fromJson : function(data){
			this.m_Nick = data[0];
			this.m_Level = data[1];
			this.m_Score = data[2];
			this.m_CorpsId = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Nick);
			data.push(this.m_Level);
			data.push(this.m_Score);
			data.push(this.m_CorpsId);
			return data;
		},
	};
}

function Msg_LC_SyncQuitRoom(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_SyncRecentLoginState(){

	return {
		m_RecentLoginState : 0,
		m_SumLoginDayCount : 0,

		fromJson : function(data){
			this.m_RecentLoginState = data[0];
			this.m_SumLoginDayCount = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_RecentLoginState);
			data.push(this.m_SumLoginDayCount);
			return data;
		},
	};
}

function Msg_LC_SyncResetGowPrize(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_LC_SyncSignInCount(){

	return {
		m_SignInCountCurMonth : 0,
		m_RestSignInCountCurDay : 0,

		fromJson : function(data){
			this.m_SignInCountCurMonth = data[0];
			this.m_RestSignInCountCurDay = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_SignInCountCurMonth);
			data.push(this.m_RestSignInCountCurDay);
			return data;
		},
	};
}

function Msg_LC_SyncSkillInfos(){

	return {
		m_Skills : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.m_Skills, function(){return new SkillDataInfo();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.m_Skills);
			return data;
		},
	};
}

function Msg_LC_SyncStamina(){

	return {
		m_Stamina : 0,

		fromJson : function(data){
			this.m_Stamina = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Stamina);
			return data;
		},
	};
}

function Msg_LC_SyncValidCorpsList(){

	return {
		m_Start : 0,
		m_Count : 0,
		m_List : new Array(),

		fromJson : function(data){
			this.m_Start = data[0];
			this.m_Count = data[1];
			s_GetSubDataArray(data, 2, this.m_List, function(){return new Msg_LC_SyncCorpsInfo();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Start);
			data.push(this.m_Count);
			s_AddSubDataArray(data, this.m_List);
			return data;
		},
	};
}

function Msg_LC_SyncVigor(){

	return {
		m_Vigor : 0,

		fromJson : function(data){
			this.m_Vigor = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Vigor);
			return data;
		},
	};
}

function Msg_LC_SyncVitality(){

	return {
		m_Vitality : 0,

		fromJson : function(data){
			this.m_Vitality = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Vitality);
			return data;
		},
	};
}

function Msg_LC_SystemChatWorldResult(){

	return {
		m_Content : "",

		fromJson : function(data){
			this.m_Content = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Content);
			return data;
		},
	};
}

function Msg_LC_TDChallengeResult(){

	return {
		Result : 0,
		ChallengeInfo : c_Null/*ChallengeInfoData*/,

		fromJson : function(data){
			this.Result = data[0];
			this.ChallengeInfo = new ChallengeInfoData();
			s_GetSubData(data, 1, this.ChallengeInfo);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Result);
			s_AddSubData(data, this.ChallengeInfo);
			return data;
		},
	};
}

function Msg_LC_UnlockLegacyResult(){

	return {
		m_Index : 0,
		m_ItemID : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_Index = data[0];
			this.m_ItemID = data[1];
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Index);
			data.push(this.m_ItemID);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_UnlockSkillResult(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_UserLevel : 0,
		m_SlotPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_UserLevel = data[2];
			this.m_SlotPos = data[3];
			this.m_Result = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_UserLevel);
			data.push(this.m_SlotPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_UnmountEquipmentResult(){

	return {
		m_EquipPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_EquipPos = data[0];
			this.m_Result = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_EquipPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_UnmountFashionResult(){

	return {
		m_Result : 0,
		m_FashionID : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_FashionID = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_FashionID);
			return data;
		},
	};
}

function Msg_LC_UnmountSkillResult(){

	return {
		m_PresetIndex : 0,
		m_SlotPos : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SlotPos = data[1];
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SlotPos);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_UpdateInviteInfo(){

	return {
		m_InviteeCount : 0,
		m_OverLv30Count : 0,
		m_OverLv50Count : 0,

		fromJson : function(data){
			this.m_InviteeCount = data[0];
			this.m_OverLv30Count = data[1];
			this.m_OverLv50Count = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_InviteeCount);
			data.push(this.m_OverLv30Count);
			data.push(this.m_OverLv50Count);
			return data;
		},
	};
}

function Msg_LC_UpgradeEquipBatch(){

	return {
		m_Result : 0,
		m_Money : 0,
		m_Gold : 0,
		m_Guids : new Array(),
		m_Level : new Array(),
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_Money = data[1];
			this.m_Gold = data[2];
			this.m_Guids = data[3];
			this.m_Level = data[4];
			this.m_GoldCash = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Guids);
			data.push(this.m_Level);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_UpgradeItemResult(){

	return {
		m_Position : 0,
		m_Money : 0,
		m_Gold : 0,
		m_Result : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_Position = data[0];
			this.m_Money = data[1];
			this.m_Gold = data[2];
			this.m_Result = data[3];
			this.m_GoldCash = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Position);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Result);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_UpgradeLegacyResult(){

	return {
		m_Index : 0,
		m_ItemID : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_Index = data[0];
			this.m_ItemID = data[1];
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Index);
			data.push(this.m_ItemID);
			data.push(this.m_Result);
			return data;
		},
	};
}

function Msg_LC_UpgradeParnerStageResult(){

	return {
		m_ResultCode : 0,
		m_PartnerId : 0,
		m_CurStage : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_PartnerId = data[1];
			this.m_CurStage = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_PartnerId);
			data.push(this.m_CurStage);
			return data;
		},
	};
}

function Msg_LC_UpgradePartnerLevelResult(){

	return {
		m_ResultCode : 0,
		m_PartnerId : 0,
		m_CurLevel : 0,
		m_PartnerEquipState : new Array(),

		fromJson : function(data){
			this.m_ResultCode = data[0];
			this.m_PartnerId = data[1];
			this.m_CurLevel = data[2];
			this.m_PartnerEquipState = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			data.push(this.m_PartnerId);
			data.push(this.m_CurLevel);
			data.push(this.m_PartnerEquipState);
			return data;
		},
	};
}

function Msg_LC_UpgradeSkillResult(){

	return {
		m_PresetIndex : 0,
		m_SkillID : 0,
		m_AllowCostGold : false,
		m_Money : 0,
		m_Gold : 0,
		m_Vigor : 0,
		m_Result : 0,
		m_GoldCash : 0,

		fromJson : function(data){
			this.m_PresetIndex = data[0];
			this.m_SkillID = data[1];
			this.m_AllowCostGold = data[2];
			this.m_Money = data[3];
			this.m_Gold = data[4];
			this.m_Vigor = data[5];
			this.m_Result = data[6];
			this.m_GoldCash = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_PresetIndex);
			data.push(this.m_SkillID);
			data.push(this.m_AllowCostGold);
			data.push(this.m_Money);
			data.push(this.m_Gold);
			data.push(this.m_Vigor);
			data.push(this.m_Result);
			data.push(this.m_GoldCash);
			return data;
		},
	};
}

function Msg_LC_UserLevelup(){

	return {
		m_UserId : 0,
		m_UserLevel : 0,

		fromJson : function(data){
			this.m_UserId = data[0];
			this.m_UserLevel = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_UserId);
			data.push(this.m_UserLevel);
			return data;
		},
	};
}

function Msg_LC_WeeklyLoginRewardResult(){

	return {
		m_ResultCode : 0,

		fromJson : function(data){
			this.m_ResultCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_ResultCode);
			return data;
		},
	};
}

function Msg_LC_XSoulChangeShowModelResult(){

	return {
		m_XSoulPart : 0,
		m_ModelLevel : 0,
		m_Result : 0,

		fromJson : function(data){
			this.m_XSoulPart = data[0];
			this.m_ModelLevel = data[1];
			this.m_Result = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_XSoulPart);
			data.push(this.m_ModelLevel);
			data.push(this.m_Result);
			return data;
		},
	};
}

function NodeMessageWithAccount(){

	return {
		m_Account : "",

		fromJson : function(data){
			this.m_Account = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Account);
			return data;
		},
	};
}

function NodeMessageWithAccountAndGuid(){

	return {
		m_Account : "",
		m_Guid : 0,

		fromJson : function(data){
			this.m_Account = data[0];
			this.m_Guid = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Account);
			data.push(this.m_Guid);
			return data;
		},
	};
}

function NodeMessageWithAccountAndLogicServerId(){

	return {
		m_Account : "",
		m_LogicServerId : 0,

		fromJson : function(data){
			this.m_Account = data[0];
			this.m_LogicServerId = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Account);
			data.push(this.m_LogicServerId);
			return data;
		},
	};
}

function NodeMessageWithGuid(){

	return {
		m_Guid : 0,

		fromJson : function(data){
			this.m_Guid = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Guid);
			return data;
		},
	};
}

function NodeMessageWithLogicServerId(){

	return {
		m_LogicServerId : 0,

		fromJson : function(data){
			this.m_LogicServerId = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_LogicServerId);
			return data;
		},
	};
}

function NodeRegister(){

	return {
		m_Name : "",

		fromJson : function(data){
			this.m_Name = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Name);
			return data;
		},
	};
}

function NodeRegisterResult(){

	return {
		m_IsOk : false,

		fromJson : function(data){
			this.m_IsOk = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_IsOk);
			return data;
		},
	};
}

function PartnerDataMsg(){

	return {
		Id : 0,
		AdditionLevel : 0,
		SkillStage : 0,
		EquipState : new Array(),

		fromJson : function(data){
			this.Id = data[0];
			this.AdditionLevel = data[1];
			this.SkillStage = data[2];
			this.EquipState = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Id);
			data.push(this.AdditionLevel);
			data.push(this.SkillStage);
			data.push(this.EquipState);
			return data;
		},
	};
}

function PaymentRebateDataMsg(){

	function AwardItemInfo(){

		return {
			m_Id : 0,
			m_Num : 0,

			fromJson : function(data){
				this.m_Id = data[0];
				this.m_Num = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.m_Id);
				data.push(this.m_Num);
				return data;
			},
		};
	}

	PaymentRebateDataMsg.AwardItemInfo = AwardItemInfo;

	return {
		Id : 0,
		Group : 0,
		Describe : "",
		AnnounceTime : "",
		StartTime : "",
		EndTime : "",
		TotalDiamond : 0,
		Diamond : 0,
		Gold : 0,
		Exp : 0,
		AwardItems : new Array(),
		DiamondAlready : 0,

		fromJson : function(data){
			this.Id = data[0];
			this.Group = data[1];
			this.Describe = data[2];
			this.AnnounceTime = data[3];
			this.StartTime = data[4];
			this.EndTime = data[5];
			this.TotalDiamond = data[6];
			this.Diamond = data[7];
			this.Gold = data[8];
			this.Exp = data[9];
			s_GetSubDataArray(data, 10, this.AwardItems, function(){return new AwardItemInfo();});
			this.DiamondAlready = data[11];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Id);
			data.push(this.Group);
			data.push(this.Describe);
			data.push(this.AnnounceTime);
			data.push(this.StartTime);
			data.push(this.EndTime);
			data.push(this.TotalDiamond);
			data.push(this.Diamond);
			data.push(this.Gold);
			data.push(this.Exp);
			s_AddSubDataArray(data, this.AwardItems);
			data.push(this.DiamondAlready);
			return data;
		},
	};
}

function SelectedPartnerDataMsg(){

	return {
		m_Id : 0,
		m_AdditionLevel : 0,
		m_SkillStage : 0,

		fromJson : function(data){
			this.m_Id = data[0];
			this.m_AdditionLevel = data[1];
			this.m_SkillStage = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Id);
			data.push(this.m_AdditionLevel);
			data.push(this.m_SkillStage);
			return data;
		},
	};
}

function SkillDataInfo(){

	return {
		ID : 0,
		Level : 0,
		Postions : 0,

		fromJson : function(data){
			this.ID = data[0];
			this.Level = data[1];
			this.Postions = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ID);
			data.push(this.Level);
			data.push(this.Postions);
			return data;
		},
	};
}

function TDMatchInfoMsg(){

	return {
		Guid : 0,
		NickName : "",
		Level : 0,
		FightScore : 0,
		HeroId : 0,
		DropId : 0,

		fromJson : function(data){
			this.Guid = data[0];
			this.NickName = data[1];
			this.Level = data[2];
			this.FightScore = data[3];
			this.HeroId = data[4];
			this.DropId = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.Guid);
			data.push(this.NickName);
			data.push(this.Level);
			data.push(this.FightScore);
			data.push(this.HeroId);
			data.push(this.DropId);
			return data;
		},
	};
}

function TooManyOperations(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function UserHeartbeat(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function VersionVerify(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function VersionVerifyResult(){

	return {
		m_Result : 0,
		m_EnableLog : 0,
		m_ShopMask : 0,

		fromJson : function(data){
			this.m_Result = data[0];
			this.m_EnableLog = data[1];
			this.m_ShopMask = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_Result);
			data.push(this.m_EnableLog);
			data.push(this.m_ShopMask);
			return data;
		},
	};
}

function XSoulDataMsg(){

	return {
		ItemId : 0,
		Level : 0,
		ModelLevel : 0,
		Experience : 0,

		fromJson : function(data){
			this.ItemId = data[0];
			this.Level = data[1];
			this.ModelLevel = data[2];
			this.Experience = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ItemId);
			data.push(this.Level);
			data.push(this.ModelLevel);
			data.push(this.Experience);
			return data;
		},
	};
}

var JsonMessageDefine = {
	NodeRegister : 1,
	NodeRegisterResult : 2,
	VersionVerify : 3,
	VersionVerifyResult : 4,
	DirectLogin : 5,
	AccountLogout : 6,
	Logout : 7,
	GetQueueingCount : 8,
	UserHeartbeat : 9,
	KickUser : 10,
	TooManyOperations : 11,
	Msg_CL_StartGame : 12,
	Msg_CL_FriendList : 13,
	Msg_CL_RoleList : 14,
	Msg_CL_CreateNickname : 15,
	Msg_CL_BuyStamina : 16,
	Msg_CL_BuyLife : 17,
	Msg_CL_GetMailList : 18,
	Msg_CL_QueryExpeditionInfo : 19,
	Msg_CL_ExpeditionFailure : 20,
	Msg_CL_MidasTouch : 21,
	Msg_CL_RequestGroupInfo : 22,
	Msg_CL_QuitPve : 23,
	Msg_CL_RequestVigor : 24,
	Msg_CL_WeeklyLoginReward : 25,
	Msg_CL_GetQueueingCount : 26,
	Msg_CL_RequestGowPrize : 27,
	Msg_CL_GainFirstPayReward : 28,
	Msg_CL_CorpsSignIn : 29,
	Msg_CL_BuyPartnerCombatTicket : 30,
	Msg_CL_RefreshPartnerCombat : 31,
	Msg_CL_GetLootHistory : 32,
	Msg_CL_GetMorrowReward : 33,
	Msg_CL_GetLoginLottery : 34,
	Msg_CL_QuerySkillInfos : 35,
	Msg_CL_CorpsClearRequest : 36,
	Msg_CL_ExpeditionSweep : 37,
	Msg_CL_GMResetDailyMissions : 38,
	Msg_LC_ResetCorpsSignIn : 39,
	Msg_LC_SyncQuitRoom : 40,
	Msg_LC_NoticeQuitGroup : 41,
	Msg_LC_NotifyNewMail : 42,
	Msg_LC_RequestItemUseResult : 43,
	Msg_LC_ServerShutdown : 44,
	Msg_LC_SyncResetGowPrize : 45,
	Msg_LC_ResetConsumeGoodsCount : 46,
	Msg_LC_NotifyMorrowReward : 47,
	Msg_LC_ResetOnlineTimeRewardData : 48,
	Msg_LC_ResetWeeklyLoginRewardData : 49,
	Msg_LC_GmCode : 50,
	Msg_CL_AccountLogin : 51,
	Msg_LC_AccountLoginResult : 52,
	Msg_LC_CreateNicnameResult : 53,
	Msg_CL_CreateRole : 54,
	Msg_LC_CreateRoleResult : 55,
	Msg_CL_ActivateAccount : 56,
	Msg_LC_ActivateAccountResult : 57,
	MailItemForMessage : 58,
	MailInfoForMessage : 59,
	Msg_LC_SyncMailList : 60,
	MissionInfoForSync : 61,
	Msg_LC_ResetDailyMissions : 62,
	Msg_CL_SinglePVE : 63,
	Msg_CL_SaveSkillPreset : 64,
	Msg_LC_StartGameResult : 65,
	Msg_CL_UpdateFightingScore : 66,
	Msg_CL_RoleEnter : 67,
	JsonItemDataMsg : 68,
	SkillDataInfo : 69,
	LegacyDataMsg : 70,
	XSoulDataMsg : 71,
	JsonTalentDataMsg : 72,
	PaymentRebateDataMsg : 73,
	GowDataMsg : 74,
	FashionMsg : 75,
	FashionHideMsg : 76,
	FashionSynMsg : 77,
	Msg_LC_RoleEnterResult : 78,
	MorrowRewardInfo : 79,
	Msg_CL_ExpeditionReset : 80,
	Msg_LC_ExpeditionResetResult : 81,
	Msg_LC_RoleListResult : 82,
	Msg_CL_RequestExpedition : 83,
	Msg_LC_RequestExpeditionResult : 84,
	Msg_CL_FinishExpedition : 85,
	Msg_LC_FinishExpeditionResult : 86,
	Msg_LC_ExpeditionSweepResult : 87,
	Msg_CL_DiscardItem : 88,
	Msg_LC_DiscardItemResult : 89,
	Msg_CL_SellItem : 90,
	Msg_LC_SellItemResult : 91,
	Msg_CL_MountEquipment : 92,
	Msg_LC_MountEquipmentResult : 93,
	Msg_CL_MountSkill : 94,
	Msg_LC_MountSkillResult : 95,
	Msg_CL_UnmountSkill : 96,
	Msg_LC_UnmountSkillResult : 97,
	Msg_CL_UpgradeSkill : 98,
	Msg_LC_UpgradeSkillResult : 99,
	Msg_CL_UnlockSkill : 100,
	Msg_LC_UnlockSkillResult : 101,
	Msg_CL_LiftSkill : 102,
	Msg_LC_LiftSkillResult : 103,
	Msg_CL_UpgradeItem : 104,
	Msg_LC_UpgradeItemResult : 105,
	Msg_CL_SwapSkill : 106,
	Msg_LC_SwapSkillResult : 107,
	Msg_LC_BuyStaminaResult : 108,
	Msg_CL_FinishMission : 109,
	Msg_LC_FinishMissionResult : 110,
	Msg_LC_BuyLifeResult : 111,
	Msg_LC_UnlockLegacyResult : 112,
	Msg_CL_UpgradeLegacy : 113,
	Msg_LC_UpgradeLegacyResult : 114,
	Msg_CL_AddXSoulExperience : 115,
	Msg_LC_AddXSoulExperienceResult : 116,
	Msg_CL_XSoulChangeShowModel : 117,
	Msg_LC_XSoulChangeShowModelResult : 118,
	Msg_CL_ReadMail : 119,
	Msg_CL_ReceiveMail : 120,
	Msg_CL_ExpeditionAward : 121,
	Msg_LC_ExpeditionAwardResult : 122,
	Msg_LC_MidasTouchResult : 123,
	Msg_CL_UnmountEquipment : 124,
	Msg_LC_UnmountEquipmentResult : 125,
	Msg_LC_UserLevelup : 126,
	Msg_LC_SyncStamina : 127,
	Msg_CL_StageClear : 128,
	Msg_LC_StageClearResult : 129,
	Msg_CL_AddAssets : 130,
	Msg_LC_AddAssetsResult : 131,
	Msg_CL_AddItem : 132,
	Msg_LC_SyncGowStarList : 133,
	Msg_CL_RequestGowBattleResult : 134,
	Msg_LC_SyncGowBattleResult : 135,
	Msg_CL_AddFriend : 136,
	FriendInfoForMsg : 137,
	Msg_LC_AddFriendResult : 138,
	Msg_LC_SyncFriendList : 139,
	Msg_CL_DeleteFriend : 140,
	Msg_LC_DelFriendResult : 141,
	Msg_CL_ConfirmFriend : 142,
	Msg_CL_QueryFriendInfo : 143,
	Msg_LC_QueryFriendInfoResult : 144,
	Msg_LC_MissionCompleted : 145,
	Msg_LC_SyncNoticeContent : 146,
	Msg_LC_FriendOnline : 147,
	Msg_LC_FriendOffline : 148,
	Msg_LC_SyncGroupUsers : 149,
	Msg_LC_RequestJoinGroupResult : 150,
	Msg_LC_ConfirmJoinGroupResult : 151,
	Msg_CL_PinviteTeam : 152,
	Msg_CL_RequestJoinGroup : 153,
	Msg_CL_ConfirmJoinGroup : 154,
	Msg_CL_QuitGroup : 155,
	Msg_LC_SyncPinviteTeam : 156,
	Msg_LC_AddItemResult : 157,
	Msg_LC_AddItemsResult : 158,
	Msg_CL_GetGowStarList : 159,
	Msg_LC_SyncLeaveGroup : 160,
	Msg_CL_RefuseGroupRequest : 161,
	Msg_CL_SelectPartner : 162,
	Msg_CL_CancelSelectPartner : 163,
	Msg_LC_CancelSelectPartnerResult : 164,
	Msg_LC_SelectPartnerResult : 165,
	Msg_CL_UpgradePartnerLevel : 166,
	Msg_LC_UpgradePartnerLevelResult : 167,
	Msg_CL_UpgradePartnerStage : 168,
	Msg_LC_UpgradeParnerStageResult : 169,
	Msg_CL_PartnerEquip : 170,
	Msg_LC_PartnerEquipResult : 171,
	Msg_LC_GetPartner : 172,
	Msg_LC_ChangeCaptain : 173,
	Msg_CL_RequestMatch : 174,
	Msg_CL_CancelMatch : 175,
	Msg_LC_MatchResult : 176,
	Msg_LC_MpveGeneralResult : 177,
	Msg_CL_StartMpve : 178,
	Msg_LC_MpveAwardResult : 179,
	Msg_CL_UpdatePosition : 180,
	Msg_CL_RequestUsers : 181,
	Msg_LC_RequestUsersResult : 182,
	Msg_CL_RequestUserPosition : 183,
	Msg_LC_RequestUserPositionResult : 184,
	Msg_CL_ChangeScene : 185,
	Msg_CL_ChangeCityRoom : 186,
	Msg_CL_CompoundPartner : 187,
	Msg_LC_CompoundPartnerResult : 188,
	Msg_CL_SweepStage : 189,
	Msg_LC_SweepStageResult : 190,
	Msg_CL_RequestPlayerInfo : 191,
	Msg_LC_SyncPlayerInfo : 192,
	Msg_CL_UpdateActivityUnlock : 193,
	Msg_CL_RequestPlayerDetail : 194,
	Msg_LC_SyncPlayerDetail : 195,
	Msg_LC_NoticePlayerOffline : 196,
	Msg_CL_ExchangeGoods : 197,
	Msg_LC_ExchangeGoodsResult : 198,
	Msg_CL_SecretAreaTrial : 199,
	Msg_LC_SecretAreaTrialResult : 200,
	Msg_CL_SecretAreaFightingInfo : 201,
	Msg_LC_SecretAreaTrialAward : 202,
	Msg_CL_QuitRoom : 203,
	Msg_LC_SyncVigor : 204,
	Msg_CL_SignInAndGetReward : 205,
	Msg_LC_SignInAndGetRewardResult : 206,
	Msg_LC_SyncSignInCount : 207,
	Msg_LC_SyncMpveInfo : 208,
	Msg_CL_SetNewbieFlag : 209,
	Msg_LC_SyncNewbieFlag : 210,
	PartnerDataMsg : 211,
	ArenaInfoMsg : 212,
	Msg_CL_QueryArenaInfo : 213,
	Msg_LC_ArenaInfoResult : 214,
	Msg_CL_QueryArenaMatchGroup : 215,
	Msg_LC_ArenaMatchGroupResult : 216,
	Msg_LC_SyncGoldTollgateInfo : 217,
	Msg_CL_RequestRefreshExchange : 218,
	Msg_LC_RefreshExchangeResult : 219,
	Msg_CL_DiamondExtraBuyBox : 220,
	Msg_LC_DiamondExtraBuyBoxResult : 221,
	Msg_CL_ArenaStartChallenge : 222,
	Msg_LC_ArenaStartCallengeResult : 223,
	Msg_CL_ArenaChallengeOver : 224,
	Msg_LC_ArenaChallengeResult : 225,
	DamageInfoData : 226,
	ChallengeEntityData : 227,
	ChallengeInfoData : 228,
	Msg_CL_ArenaQueryRank : 229,
	Msg_LC_ArenaQueryRankResult : 230,
	Msg_CL_ArenaQueryHistory : 231,
	Msg_LC_ArenaQueryHistoryResult : 232,
	Msg_CL_ArenaChangePartner : 233,
	Msg_LC_ArenaChangePartnerResult : 234,
	Msg_CL_ArenaBuyFightCount : 235,
	Msg_LC_ArenaBuyFightCountResult : 236,
	Msg_CL_ArenaBeginFight : 237,
	Msg_CL_ExchangeGift : 238,
	Msg_LC_ExchangeGiftResult : 239,
	Msg_CL_CompoundEquip : 240,
	Msg_LC_CompoundEquipResult : 241,
	Msg_CL_SetNewbieActionFlag : 242,
	Msg_LC_SyncNewbieActionFlag : 243,
	Msg_LC_WeeklyLoginRewardResult : 244,
	Msg_LC_QueueingCountResult : 245,
	Msg_CL_GetOnlineTimeReward : 246,
	Msg_LC_GetOnlineTimeRewardResult : 247,
	Msg_CL_RecordNewbieFlag : 248,
	SelectedPartnerDataMsg : 249,
	Msg_CL_RequestSkillInfos : 250,
	Msg_LC_SyncSkillInfos : 251,
	Msg_LC_SyncCombatData : 252,
	Msg_CL_UploadFPS : 253,
	Msg_LC_SyncGuideFlag : 254,
	Msg_CL_RequestDare : 255,
	Msg_LC_RequestDare : 256,
	Msg_CL_RequestDareByGuid : 257,
	Msg_LC_RequestDareResult : 258,
	Msg_CL_AcceptedDare : 259,
	Msg_LC_RequestGowPrizeResult : 260,
	Msg_CL_RequestEnhanceEquipmentStar : 261,
	Msg_LC_RequestEnhanceEquipmentStar : 262,
	Msg_LC_SyncCorpsOpResult : 263,
	Msg_CL_CorpsCreate : 264,
	Msg_CL_CorpsJoin : 265,
	Msg_CL_CorpsQuit : 266,
	Msg_CL_CorpsAgreeClaimer : 267,
	Msg_CL_CorpsRefuseClaimer : 268,
	Msg_CL_CorpsKickout : 269,
	Msg_CL_CorpsAppoint : 270,
	Msg_CL_QueryCorpsInfo : 271,
	Msg_LC_SyncCorpsInfo : 272,
	Msg_CL_QueryCorpsStar : 273,
	Msg_LC_SyncCorpsStar : 274,
	Msg_CL_CorpsDissolve : 275,
	Msg_CL_EquipTalentCard : 276,
	Msg_LC_EquipTalentCardResult : 277,
	Msg_CLC_UnequipTalentCard : 278,
	Msg_CL_AddTalentExperience : 279,
	ItemLeftMsg : 280,
	Msg_LC_AddTalentExperienceResult : 281,
	Msg_CL_UpgradeEquipBatch : 282,
	Msg_LC_UpgradeEquipBatch : 283,
	Msg_CL_RequestMpveAward : 284,
	Msg_CL_SendChat : 285,
	Msg_LC_ChatStatus : 286,
	Msg_LC_ChatResult : 287,
	Msg_LC_ChatWorldResult : 288,
	Msg_LC_SystemChatWorldResult : 289,
	Msg_CL_ChatAddShield : 290,
	Msg_CL_ChatAddShieldByName : 291,
	Msg_LC_ChatAddShieldResult : 292,
	Msg_CL_ChatDelShield : 293,
	Msg_LC_ChatDelShieldResult : 294,
	Msg_CL_RequireChatRoleInfo : 295,
	Msg_LC_ChatRoleInfoReturn : 296,
	Msg_CL_RequireChatEquipInfo : 297,
	Msg_LC_ChatEquipInfoReturn : 298,
	Msg_CL_RequireChatShieldList : 299,
	ChatShieldInfoForMsg : 300,
	Msg_LC_ChatShieldListReturn : 301,
	Msg_LC_SendScreenTip : 302,
	Msg_CL_SetCorpsNotice : 303,
	Msg_CL_GmPay : 304,
	Msg_LC_GmPayResult : 305,
	Msg_CL_GainVipReward : 306,
	Msg_LC_GainVipRewardResult : 307,
	Msg_CL_QueryValidCorpsList : 308,
	Msg_LC_SyncValidCorpsList : 309,
	Msg_LC_SyncGowRankInfo : 310,
	Msg_LC_GainFirstPayRewardResult : 311,
	Msg_CL_BuyFashion : 312,
	Msg_LC_BuyFashionResult : 313,
	Msg_LC_AwardFashionResult : 314,
	Msg_CL_MountFashion : 315,
	Msg_LC_MountFashionResult : 316,
	Msg_CL_UnmountFashion : 317,
	Msg_LC_UnmountFashionResult : 318,
	Msg_LC_NoticeFashionOverdueSoon : 319,
	Msg_LC_NoticeFashionOverdue : 320,
	Msg_LC_BuyGoodsSucceed : 321,
	Msg_LC_DrawRewardResult : 322,
	Msg_CL_DrawReward : 323,
	Msg_CL_CombinTalentCard : 324,
	Msg_CL_CombinTalentCardResult : 325,
	Msg_CL_BuyEliteCount : 326,
	Msg_LC_BuyEliteCountResult : 327,
	Msg_CL_StartPartnerBattle : 328,
	Msg_LC_StartPartnerBattleResult : 329,
	Msg_CL_EndPartnerBattle : 330,
	Msg_LC_EndPartnerBattleResult : 331,
	Msg_LC_BuyPartnerCombatTicketResult : 332,
	Msg_CL_CorpsIndirectJoin : 333,
	TDMatchInfoMsg : 334,
	Msg_CL_QueryTDMatchGroup : 335,
	Msg_LC_QueryTDMatchGroupResult : 336,
	Msg_CL_StartTDChallenge : 337,
	Msg_LC_StartTDChallengeResult : 338,
	Msg_CL_TDBeginFight : 339,
	Msg_CL_TDChallengeOver : 340,
	Msg_LC_TDChallengeResult : 341,
	Msg_CL_BuyTDFightCount : 342,
	Msg_LC_BuyTDFightCountResult : 343,
	Msg_LC_SyncVitality : 344,
	Msg_LC_CorpsSignIn : 345,
	Msg_LC_PartnerCombatInfo : 346,
	FightingScoreEntityMsg : 347,
	Msg_CLC_QueryFightingScoreRank : 348,
	Msg_CL_OpenDomain : 349,
	Msg_LC_LootOpResult : 350,
	LootInfoMsg : 351,
	Msg_LC_SyncLootInfo : 352,
	Msg_LC_LootMatchResult : 353,
	Msg_LC_LootChangeDefenseOrder : 354,
	Msg_CL_StartLoot : 355,
	Msg_LC_StartLootResult : 356,
	Msg_CL_OverLoot : 357,
	LootEntityData : 358,
	Msg_LC_OverLootResult : 359,
	LootHistoryData : 360,
	Msg_LC_SyncLootHistory : 361,
	Msg_LC_SyncLootAward : 362,
	Msg_CL_RefusedDare : 363,
	Msg_LC_SyncLotteryInfo : 364,
	Msg_CL_EquipmentStrength : 365,
	Msg_LC_EquipmentStrengthResult : 366,
	Msg_CL_SetFashionShow : 367,
	Msg_LC_SetFashionShowResult : 368,
	Msg_LC_MorrowRewardActive : 369,
	Msg_LC_GetMorrowRewardResult : 370,
	Msg_LC_SyncHomeNotice : 371,
	Msg_LC_LootCostVitality : 372,
	Msg_CL_QueryLootInfo : 373,
	Msg_CL_CostVitality : 374,
	Msg_CL_LootMatchTarget : 375,
	Msg_CL_GetLootAward : 376,
	Msg_LC_SyncRecentLoginState : 377,
	Msg_LC_SyncFightingScore : 378,
	Msg_LC_GetLoginLotteryResult : 379,
	Msg_LC_RefreshPartnerCombatResult : 380,
	Msg_LC_LootChangeLootOrder : 381,
	Msg_LC_ItemsGiveBack : 382,
	Msg_CL_QueryCorpsByName : 383,
	Msg_LC_QueryCorpsByName : 384,
	Msg_CLC_StoryMessage : 385,
	Msg_CL_OpenCharpter : 386,
	Msg_CL_ResetCharpter : 387,
	Msg_CL_StartCorpsTollgate : 388,
	Msg_LC_InviteInfoAfterRoleEnter : 389,
	Msg_CL_RequestInvite : 390,
	Msg_LC_RequestInviteResult : 391,
	Msg_LC_UpdateInviteInfo : 392,
	Msg_CL_RequestInviteReward : 393,
	Msg_LC_RequestInviteRewardResult : 394,
	Msg_CLC_CollectGrowthFund : 395,
	Msg_LC_SyncCorpsDareCount : 396,
	Msg_CLC_CollectChapterAward : 397,
	Msg_CL_InteractivePrize : 398,
	Msg_LC_InteractivePrize : 399,
	Msg_CL_PushDelayData : 400,
	Msg_CL_EnterField : 401,
	Msg_CL_ChangeFieldRoom : 402,
	Msg_LC_LackOfSpace : 403,
	Msg_LC_SyncGowOtherInfo : 404,
	ItemInfo_UseItem : 405,
	Msg_CL_ItemUseRequest : 406,
	Msg_LC_ItemUseResult : 407,
	Msg_CL_AuctionQuery : 408,
	Msg_CL_AuctionSell : 409,
	Msg_CL_AuctionBuy : 410,
	Msg_CL_AuctionUnshelve : 411,
	Msg_CL_AuctionReceive : 412,
	Msg_CL_AuctionSelfAuction : 413,
	AuctionInfo : 414,
	Msg_LC_AuctionSelfAuction : 415,
	Msg_LC_AuctionAuction : 416,
	Msg_LC_AuctionOpResult : 417,
};

function JsonMessageDefine2Object(){
	var dict = new Object();
	dict[JsonMessageDefine.AccountLogout] = function(){return new AccountLogout();};
	dict[JsonMessageDefine.ArenaInfoMsg] = function(){return new ArenaInfoMsg();};
	dict[JsonMessageDefine.AuctionInfo] = function(){return new AuctionInfo();};
	dict[JsonMessageDefine.ChallengeEntityData] = function(){return new ChallengeEntityData();};
	dict[JsonMessageDefine.ChallengeInfoData] = function(){return new ChallengeInfoData();};
	dict[JsonMessageDefine.ChatShieldInfoForMsg] = function(){return new ChatShieldInfoForMsg();};
	dict[JsonMessageDefine.DamageInfoData] = function(){return new DamageInfoData();};
	dict[JsonMessageDefine.DirectLogin] = function(){return new DirectLogin();};
	dict[JsonMessageDefine.FashionHideMsg] = function(){return new FashionHideMsg();};
	dict[JsonMessageDefine.FashionMsg] = function(){return new FashionMsg();};
	dict[JsonMessageDefine.FashionSynMsg] = function(){return new FashionSynMsg();};
	dict[JsonMessageDefine.FightingScoreEntityMsg] = function(){return new FightingScoreEntityMsg();};
	dict[JsonMessageDefine.FriendInfoForMsg] = function(){return new FriendInfoForMsg();};
	dict[JsonMessageDefine.GetQueueingCount] = function(){return new GetQueueingCount();};
	dict[JsonMessageDefine.GowDataMsg] = function(){return new GowDataMsg();};
	dict[JsonMessageDefine.ItemInfo_UseItem] = function(){return new ItemInfo_UseItem();};
	dict[JsonMessageDefine.ItemLeftMsg] = function(){return new ItemLeftMsg();};
	dict[JsonMessageDefine.JsonItemDataMsg] = function(){return new JsonItemDataMsg();};
	dict[JsonMessageDefine.JsonTalentDataMsg] = function(){return new JsonTalentDataMsg();};
	dict[JsonMessageDefine.KickUser] = function(){return new KickUser();};
	dict[JsonMessageDefine.LegacyDataMsg] = function(){return new LegacyDataMsg();};
	dict[JsonMessageDefine.Logout] = function(){return new Logout();};
	dict[JsonMessageDefine.LootEntityData] = function(){return new LootEntityData();};
	dict[JsonMessageDefine.LootHistoryData] = function(){return new LootHistoryData();};
	dict[JsonMessageDefine.LootInfoMsg] = function(){return new LootInfoMsg();};
	dict[JsonMessageDefine.MailInfoForMessage] = function(){return new MailInfoForMessage();};
	dict[JsonMessageDefine.MailItemForMessage] = function(){return new MailItemForMessage();};
	dict[JsonMessageDefine.MissionInfoForSync] = function(){return new MissionInfoForSync();};
	dict[JsonMessageDefine.MorrowRewardInfo] = function(){return new MorrowRewardInfo();};
	dict[JsonMessageDefine.Msg_CL_AcceptedDare] = function(){return new Msg_CL_AcceptedDare();};
	dict[JsonMessageDefine.Msg_CL_AccountLogin] = function(){return new Msg_CL_AccountLogin();};
	dict[JsonMessageDefine.Msg_CL_ActivateAccount] = function(){return new Msg_CL_ActivateAccount();};
	dict[JsonMessageDefine.Msg_CL_AddAssets] = function(){return new Msg_CL_AddAssets();};
	dict[JsonMessageDefine.Msg_CL_AddFriend] = function(){return new Msg_CL_AddFriend();};
	dict[JsonMessageDefine.Msg_CL_AddItem] = function(){return new Msg_CL_AddItem();};
	dict[JsonMessageDefine.Msg_CL_AddTalentExperience] = function(){return new Msg_CL_AddTalentExperience();};
	dict[JsonMessageDefine.Msg_CL_AddXSoulExperience] = function(){return new Msg_CL_AddXSoulExperience();};
	dict[JsonMessageDefine.Msg_CL_ArenaBeginFight] = function(){return new Msg_CL_ArenaBeginFight();};
	dict[JsonMessageDefine.Msg_CL_ArenaBuyFightCount] = function(){return new Msg_CL_ArenaBuyFightCount();};
	dict[JsonMessageDefine.Msg_CL_ArenaChallengeOver] = function(){return new Msg_CL_ArenaChallengeOver();};
	dict[JsonMessageDefine.Msg_CL_ArenaChangePartner] = function(){return new Msg_CL_ArenaChangePartner();};
	dict[JsonMessageDefine.Msg_CL_ArenaQueryHistory] = function(){return new Msg_CL_ArenaQueryHistory();};
	dict[JsonMessageDefine.Msg_CL_ArenaQueryRank] = function(){return new Msg_CL_ArenaQueryRank();};
	dict[JsonMessageDefine.Msg_CL_ArenaStartChallenge] = function(){return new Msg_CL_ArenaStartChallenge();};
	dict[JsonMessageDefine.Msg_CL_AuctionBuy] = function(){return new Msg_CL_AuctionBuy();};
	dict[JsonMessageDefine.Msg_CL_AuctionQuery] = function(){return new Msg_CL_AuctionQuery();};
	dict[JsonMessageDefine.Msg_CL_AuctionReceive] = function(){return new Msg_CL_AuctionReceive();};
	dict[JsonMessageDefine.Msg_CL_AuctionSelfAuction] = function(){return new Msg_CL_AuctionSelfAuction();};
	dict[JsonMessageDefine.Msg_CL_AuctionSell] = function(){return new Msg_CL_AuctionSell();};
	dict[JsonMessageDefine.Msg_CL_AuctionUnshelve] = function(){return new Msg_CL_AuctionUnshelve();};
	dict[JsonMessageDefine.Msg_CL_BuyEliteCount] = function(){return new Msg_CL_BuyEliteCount();};
	dict[JsonMessageDefine.Msg_CL_BuyFashion] = function(){return new Msg_CL_BuyFashion();};
	dict[JsonMessageDefine.Msg_CL_BuyLife] = function(){return new Msg_CL_BuyLife();};
	dict[JsonMessageDefine.Msg_CL_BuyPartnerCombatTicket] = function(){return new Msg_CL_BuyPartnerCombatTicket();};
	dict[JsonMessageDefine.Msg_CL_BuyStamina] = function(){return new Msg_CL_BuyStamina();};
	dict[JsonMessageDefine.Msg_CL_BuyTDFightCount] = function(){return new Msg_CL_BuyTDFightCount();};
	dict[JsonMessageDefine.Msg_CL_CancelMatch] = function(){return new Msg_CL_CancelMatch();};
	dict[JsonMessageDefine.Msg_CL_CancelSelectPartner] = function(){return new Msg_CL_CancelSelectPartner();};
	dict[JsonMessageDefine.Msg_CL_ChangeCityRoom] = function(){return new Msg_CL_ChangeCityRoom();};
	dict[JsonMessageDefine.Msg_CL_ChangeFieldRoom] = function(){return new Msg_CL_ChangeFieldRoom();};
	dict[JsonMessageDefine.Msg_CL_ChangeScene] = function(){return new Msg_CL_ChangeScene();};
	dict[JsonMessageDefine.Msg_CL_ChatAddShield] = function(){return new Msg_CL_ChatAddShield();};
	dict[JsonMessageDefine.Msg_CL_ChatAddShieldByName] = function(){return new Msg_CL_ChatAddShieldByName();};
	dict[JsonMessageDefine.Msg_CL_ChatDelShield] = function(){return new Msg_CL_ChatDelShield();};
	dict[JsonMessageDefine.Msg_CL_CombinTalentCard] = function(){return new Msg_CL_CombinTalentCard();};
	dict[JsonMessageDefine.Msg_CL_CombinTalentCardResult] = function(){return new Msg_CL_CombinTalentCardResult();};
	dict[JsonMessageDefine.Msg_CL_CompoundEquip] = function(){return new Msg_CL_CompoundEquip();};
	dict[JsonMessageDefine.Msg_CL_CompoundPartner] = function(){return new Msg_CL_CompoundPartner();};
	dict[JsonMessageDefine.Msg_CL_ConfirmFriend] = function(){return new Msg_CL_ConfirmFriend();};
	dict[JsonMessageDefine.Msg_CL_ConfirmJoinGroup] = function(){return new Msg_CL_ConfirmJoinGroup();};
	dict[JsonMessageDefine.Msg_CL_CorpsAgreeClaimer] = function(){return new Msg_CL_CorpsAgreeClaimer();};
	dict[JsonMessageDefine.Msg_CL_CorpsAppoint] = function(){return new Msg_CL_CorpsAppoint();};
	dict[JsonMessageDefine.Msg_CL_CorpsClearRequest] = function(){return new Msg_CL_CorpsClearRequest();};
	dict[JsonMessageDefine.Msg_CL_CorpsCreate] = function(){return new Msg_CL_CorpsCreate();};
	dict[JsonMessageDefine.Msg_CL_CorpsDissolve] = function(){return new Msg_CL_CorpsDissolve();};
	dict[JsonMessageDefine.Msg_CL_CorpsIndirectJoin] = function(){return new Msg_CL_CorpsIndirectJoin();};
	dict[JsonMessageDefine.Msg_CL_CorpsJoin] = function(){return new Msg_CL_CorpsJoin();};
	dict[JsonMessageDefine.Msg_CL_CorpsKickout] = function(){return new Msg_CL_CorpsKickout();};
	dict[JsonMessageDefine.Msg_CL_CorpsQuit] = function(){return new Msg_CL_CorpsQuit();};
	dict[JsonMessageDefine.Msg_CL_CorpsRefuseClaimer] = function(){return new Msg_CL_CorpsRefuseClaimer();};
	dict[JsonMessageDefine.Msg_CL_CorpsSignIn] = function(){return new Msg_CL_CorpsSignIn();};
	dict[JsonMessageDefine.Msg_CL_CostVitality] = function(){return new Msg_CL_CostVitality();};
	dict[JsonMessageDefine.Msg_CL_CreateNickname] = function(){return new Msg_CL_CreateNickname();};
	dict[JsonMessageDefine.Msg_CL_CreateRole] = function(){return new Msg_CL_CreateRole();};
	dict[JsonMessageDefine.Msg_CL_DeleteFriend] = function(){return new Msg_CL_DeleteFriend();};
	dict[JsonMessageDefine.Msg_CL_DiamondExtraBuyBox] = function(){return new Msg_CL_DiamondExtraBuyBox();};
	dict[JsonMessageDefine.Msg_CL_DiscardItem] = function(){return new Msg_CL_DiscardItem();};
	dict[JsonMessageDefine.Msg_CL_DrawReward] = function(){return new Msg_CL_DrawReward();};
	dict[JsonMessageDefine.Msg_CL_EndPartnerBattle] = function(){return new Msg_CL_EndPartnerBattle();};
	dict[JsonMessageDefine.Msg_CL_EnterField] = function(){return new Msg_CL_EnterField();};
	dict[JsonMessageDefine.Msg_CL_EquipmentStrength] = function(){return new Msg_CL_EquipmentStrength();};
	dict[JsonMessageDefine.Msg_CL_EquipTalentCard] = function(){return new Msg_CL_EquipTalentCard();};
	dict[JsonMessageDefine.Msg_CL_ExchangeGift] = function(){return new Msg_CL_ExchangeGift();};
	dict[JsonMessageDefine.Msg_CL_ExchangeGoods] = function(){return new Msg_CL_ExchangeGoods();};
	dict[JsonMessageDefine.Msg_CL_ExpeditionAward] = function(){return new Msg_CL_ExpeditionAward();};
	dict[JsonMessageDefine.Msg_CL_ExpeditionFailure] = function(){return new Msg_CL_ExpeditionFailure();};
	dict[JsonMessageDefine.Msg_CL_ExpeditionReset] = function(){return new Msg_CL_ExpeditionReset();};
	dict[JsonMessageDefine.Msg_CL_ExpeditionSweep] = function(){return new Msg_CL_ExpeditionSweep();};
	dict[JsonMessageDefine.Msg_CL_FinishExpedition] = function(){return new Msg_CL_FinishExpedition();};
	dict[JsonMessageDefine.Msg_CL_FinishMission] = function(){return new Msg_CL_FinishMission();};
	dict[JsonMessageDefine.Msg_CL_FriendList] = function(){return new Msg_CL_FriendList();};
	dict[JsonMessageDefine.Msg_CL_GainFirstPayReward] = function(){return new Msg_CL_GainFirstPayReward();};
	dict[JsonMessageDefine.Msg_CL_GainVipReward] = function(){return new Msg_CL_GainVipReward();};
	dict[JsonMessageDefine.Msg_CL_GetGowStarList] = function(){return new Msg_CL_GetGowStarList();};
	dict[JsonMessageDefine.Msg_CL_GetLoginLottery] = function(){return new Msg_CL_GetLoginLottery();};
	dict[JsonMessageDefine.Msg_CL_GetLootAward] = function(){return new Msg_CL_GetLootAward();};
	dict[JsonMessageDefine.Msg_CL_GetLootHistory] = function(){return new Msg_CL_GetLootHistory();};
	dict[JsonMessageDefine.Msg_CL_GetMailList] = function(){return new Msg_CL_GetMailList();};
	dict[JsonMessageDefine.Msg_CL_GetMorrowReward] = function(){return new Msg_CL_GetMorrowReward();};
	dict[JsonMessageDefine.Msg_CL_GetOnlineTimeReward] = function(){return new Msg_CL_GetOnlineTimeReward();};
	dict[JsonMessageDefine.Msg_CL_GetQueueingCount] = function(){return new Msg_CL_GetQueueingCount();};
	dict[JsonMessageDefine.Msg_CL_GmPay] = function(){return new Msg_CL_GmPay();};
	dict[JsonMessageDefine.Msg_CL_GMResetDailyMissions] = function(){return new Msg_CL_GMResetDailyMissions();};
	dict[JsonMessageDefine.Msg_CL_InteractivePrize] = function(){return new Msg_CL_InteractivePrize();};
	dict[JsonMessageDefine.Msg_CL_ItemUseRequest] = function(){return new Msg_CL_ItemUseRequest();};
	dict[JsonMessageDefine.Msg_CL_LiftSkill] = function(){return new Msg_CL_LiftSkill();};
	dict[JsonMessageDefine.Msg_CL_LootMatchTarget] = function(){return new Msg_CL_LootMatchTarget();};
	dict[JsonMessageDefine.Msg_CL_MidasTouch] = function(){return new Msg_CL_MidasTouch();};
	dict[JsonMessageDefine.Msg_CL_MountEquipment] = function(){return new Msg_CL_MountEquipment();};
	dict[JsonMessageDefine.Msg_CL_MountFashion] = function(){return new Msg_CL_MountFashion();};
	dict[JsonMessageDefine.Msg_CL_MountSkill] = function(){return new Msg_CL_MountSkill();};
	dict[JsonMessageDefine.Msg_CL_OpenCharpter] = function(){return new Msg_CL_OpenCharpter();};
	dict[JsonMessageDefine.Msg_CL_OpenDomain] = function(){return new Msg_CL_OpenDomain();};
	dict[JsonMessageDefine.Msg_CL_OverLoot] = function(){return new Msg_CL_OverLoot();};
	dict[JsonMessageDefine.Msg_CL_PartnerEquip] = function(){return new Msg_CL_PartnerEquip();};
	dict[JsonMessageDefine.Msg_CL_PinviteTeam] = function(){return new Msg_CL_PinviteTeam();};
	dict[JsonMessageDefine.Msg_CL_PushDelayData] = function(){return new Msg_CL_PushDelayData();};
	dict[JsonMessageDefine.Msg_CL_QueryArenaInfo] = function(){return new Msg_CL_QueryArenaInfo();};
	dict[JsonMessageDefine.Msg_CL_QueryArenaMatchGroup] = function(){return new Msg_CL_QueryArenaMatchGroup();};
	dict[JsonMessageDefine.Msg_CL_QueryCorpsByName] = function(){return new Msg_CL_QueryCorpsByName();};
	dict[JsonMessageDefine.Msg_CL_QueryCorpsInfo] = function(){return new Msg_CL_QueryCorpsInfo();};
	dict[JsonMessageDefine.Msg_CL_QueryCorpsStar] = function(){return new Msg_CL_QueryCorpsStar();};
	dict[JsonMessageDefine.Msg_CL_QueryExpeditionInfo] = function(){return new Msg_CL_QueryExpeditionInfo();};
	dict[JsonMessageDefine.Msg_CL_QueryFriendInfo] = function(){return new Msg_CL_QueryFriendInfo();};
	dict[JsonMessageDefine.Msg_CL_QueryLootInfo] = function(){return new Msg_CL_QueryLootInfo();};
	dict[JsonMessageDefine.Msg_CL_QuerySkillInfos] = function(){return new Msg_CL_QuerySkillInfos();};
	dict[JsonMessageDefine.Msg_CL_QueryTDMatchGroup] = function(){return new Msg_CL_QueryTDMatchGroup();};
	dict[JsonMessageDefine.Msg_CL_QueryValidCorpsList] = function(){return new Msg_CL_QueryValidCorpsList();};
	dict[JsonMessageDefine.Msg_CL_QuitGroup] = function(){return new Msg_CL_QuitGroup();};
	dict[JsonMessageDefine.Msg_CL_QuitPve] = function(){return new Msg_CL_QuitPve();};
	dict[JsonMessageDefine.Msg_CL_QuitRoom] = function(){return new Msg_CL_QuitRoom();};
	dict[JsonMessageDefine.Msg_CL_ReadMail] = function(){return new Msg_CL_ReadMail();};
	dict[JsonMessageDefine.Msg_CL_ReceiveMail] = function(){return new Msg_CL_ReceiveMail();};
	dict[JsonMessageDefine.Msg_CL_RecordNewbieFlag] = function(){return new Msg_CL_RecordNewbieFlag();};
	dict[JsonMessageDefine.Msg_CL_RefreshPartnerCombat] = function(){return new Msg_CL_RefreshPartnerCombat();};
	dict[JsonMessageDefine.Msg_CL_RefusedDare] = function(){return new Msg_CL_RefusedDare();};
	dict[JsonMessageDefine.Msg_CL_RefuseGroupRequest] = function(){return new Msg_CL_RefuseGroupRequest();};
	dict[JsonMessageDefine.Msg_CL_RequestDare] = function(){return new Msg_CL_RequestDare();};
	dict[JsonMessageDefine.Msg_CL_RequestDareByGuid] = function(){return new Msg_CL_RequestDareByGuid();};
	dict[JsonMessageDefine.Msg_CL_RequestEnhanceEquipmentStar] = function(){return new Msg_CL_RequestEnhanceEquipmentStar();};
	dict[JsonMessageDefine.Msg_CL_RequestExpedition] = function(){return new Msg_CL_RequestExpedition();};
	dict[JsonMessageDefine.Msg_CL_RequestGowBattleResult] = function(){return new Msg_CL_RequestGowBattleResult();};
	dict[JsonMessageDefine.Msg_CL_RequestGowPrize] = function(){return new Msg_CL_RequestGowPrize();};
	dict[JsonMessageDefine.Msg_CL_RequestGroupInfo] = function(){return new Msg_CL_RequestGroupInfo();};
	dict[JsonMessageDefine.Msg_CL_RequestInvite] = function(){return new Msg_CL_RequestInvite();};
	dict[JsonMessageDefine.Msg_CL_RequestInviteReward] = function(){return new Msg_CL_RequestInviteReward();};
	dict[JsonMessageDefine.Msg_CL_RequestJoinGroup] = function(){return new Msg_CL_RequestJoinGroup();};
	dict[JsonMessageDefine.Msg_CL_RequestMatch] = function(){return new Msg_CL_RequestMatch();};
	dict[JsonMessageDefine.Msg_CL_RequestMpveAward] = function(){return new Msg_CL_RequestMpveAward();};
	dict[JsonMessageDefine.Msg_CL_RequestPlayerDetail] = function(){return new Msg_CL_RequestPlayerDetail();};
	dict[JsonMessageDefine.Msg_CL_RequestPlayerInfo] = function(){return new Msg_CL_RequestPlayerInfo();};
	dict[JsonMessageDefine.Msg_CL_RequestRefreshExchange] = function(){return new Msg_CL_RequestRefreshExchange();};
	dict[JsonMessageDefine.Msg_CL_RequestSkillInfos] = function(){return new Msg_CL_RequestSkillInfos();};
	dict[JsonMessageDefine.Msg_CL_RequestUserPosition] = function(){return new Msg_CL_RequestUserPosition();};
	dict[JsonMessageDefine.Msg_CL_RequestUsers] = function(){return new Msg_CL_RequestUsers();};
	dict[JsonMessageDefine.Msg_CL_RequestVigor] = function(){return new Msg_CL_RequestVigor();};
	dict[JsonMessageDefine.Msg_CL_RequireChatEquipInfo] = function(){return new Msg_CL_RequireChatEquipInfo();};
	dict[JsonMessageDefine.Msg_CL_RequireChatRoleInfo] = function(){return new Msg_CL_RequireChatRoleInfo();};
	dict[JsonMessageDefine.Msg_CL_RequireChatShieldList] = function(){return new Msg_CL_RequireChatShieldList();};
	dict[JsonMessageDefine.Msg_CL_ResetCharpter] = function(){return new Msg_CL_ResetCharpter();};
	dict[JsonMessageDefine.Msg_CL_RoleEnter] = function(){return new Msg_CL_RoleEnter();};
	dict[JsonMessageDefine.Msg_CL_RoleList] = function(){return new Msg_CL_RoleList();};
	dict[JsonMessageDefine.Msg_CL_SaveSkillPreset] = function(){return new Msg_CL_SaveSkillPreset();};
	dict[JsonMessageDefine.Msg_CL_SecretAreaFightingInfo] = function(){return new Msg_CL_SecretAreaFightingInfo();};
	dict[JsonMessageDefine.Msg_CL_SecretAreaTrial] = function(){return new Msg_CL_SecretAreaTrial();};
	dict[JsonMessageDefine.Msg_CL_SelectPartner] = function(){return new Msg_CL_SelectPartner();};
	dict[JsonMessageDefine.Msg_CL_SellItem] = function(){return new Msg_CL_SellItem();};
	dict[JsonMessageDefine.Msg_CL_SendChat] = function(){return new Msg_CL_SendChat();};
	dict[JsonMessageDefine.Msg_CL_SetCorpsNotice] = function(){return new Msg_CL_SetCorpsNotice();};
	dict[JsonMessageDefine.Msg_CL_SetFashionShow] = function(){return new Msg_CL_SetFashionShow();};
	dict[JsonMessageDefine.Msg_CL_SetNewbieActionFlag] = function(){return new Msg_CL_SetNewbieActionFlag();};
	dict[JsonMessageDefine.Msg_CL_SetNewbieFlag] = function(){return new Msg_CL_SetNewbieFlag();};
	dict[JsonMessageDefine.Msg_CL_SignInAndGetReward] = function(){return new Msg_CL_SignInAndGetReward();};
	dict[JsonMessageDefine.Msg_CL_SinglePVE] = function(){return new Msg_CL_SinglePVE();};
	dict[JsonMessageDefine.Msg_CL_StageClear] = function(){return new Msg_CL_StageClear();};
	dict[JsonMessageDefine.Msg_CL_StartCorpsTollgate] = function(){return new Msg_CL_StartCorpsTollgate();};
	dict[JsonMessageDefine.Msg_CL_StartGame] = function(){return new Msg_CL_StartGame();};
	dict[JsonMessageDefine.Msg_CL_StartLoot] = function(){return new Msg_CL_StartLoot();};
	dict[JsonMessageDefine.Msg_CL_StartMpve] = function(){return new Msg_CL_StartMpve();};
	dict[JsonMessageDefine.Msg_CL_StartPartnerBattle] = function(){return new Msg_CL_StartPartnerBattle();};
	dict[JsonMessageDefine.Msg_CL_StartTDChallenge] = function(){return new Msg_CL_StartTDChallenge();};
	dict[JsonMessageDefine.Msg_CL_SwapSkill] = function(){return new Msg_CL_SwapSkill();};
	dict[JsonMessageDefine.Msg_CL_SweepStage] = function(){return new Msg_CL_SweepStage();};
	dict[JsonMessageDefine.Msg_CL_TDBeginFight] = function(){return new Msg_CL_TDBeginFight();};
	dict[JsonMessageDefine.Msg_CL_TDChallengeOver] = function(){return new Msg_CL_TDChallengeOver();};
	dict[JsonMessageDefine.Msg_CL_UnlockSkill] = function(){return new Msg_CL_UnlockSkill();};
	dict[JsonMessageDefine.Msg_CL_UnmountEquipment] = function(){return new Msg_CL_UnmountEquipment();};
	dict[JsonMessageDefine.Msg_CL_UnmountFashion] = function(){return new Msg_CL_UnmountFashion();};
	dict[JsonMessageDefine.Msg_CL_UnmountSkill] = function(){return new Msg_CL_UnmountSkill();};
	dict[JsonMessageDefine.Msg_CL_UpdateActivityUnlock] = function(){return new Msg_CL_UpdateActivityUnlock();};
	dict[JsonMessageDefine.Msg_CL_UpdateFightingScore] = function(){return new Msg_CL_UpdateFightingScore();};
	dict[JsonMessageDefine.Msg_CL_UpdatePosition] = function(){return new Msg_CL_UpdatePosition();};
	dict[JsonMessageDefine.Msg_CL_UpgradeEquipBatch] = function(){return new Msg_CL_UpgradeEquipBatch();};
	dict[JsonMessageDefine.Msg_CL_UpgradeItem] = function(){return new Msg_CL_UpgradeItem();};
	dict[JsonMessageDefine.Msg_CL_UpgradeLegacy] = function(){return new Msg_CL_UpgradeLegacy();};
	dict[JsonMessageDefine.Msg_CL_UpgradePartnerLevel] = function(){return new Msg_CL_UpgradePartnerLevel();};
	dict[JsonMessageDefine.Msg_CL_UpgradePartnerStage] = function(){return new Msg_CL_UpgradePartnerStage();};
	dict[JsonMessageDefine.Msg_CL_UpgradeSkill] = function(){return new Msg_CL_UpgradeSkill();};
	dict[JsonMessageDefine.Msg_CL_UploadFPS] = function(){return new Msg_CL_UploadFPS();};
	dict[JsonMessageDefine.Msg_CL_WeeklyLoginReward] = function(){return new Msg_CL_WeeklyLoginReward();};
	dict[JsonMessageDefine.Msg_CL_XSoulChangeShowModel] = function(){return new Msg_CL_XSoulChangeShowModel();};
	dict[JsonMessageDefine.Msg_CLC_CollectChapterAward] = function(){return new Msg_CLC_CollectChapterAward();};
	dict[JsonMessageDefine.Msg_CLC_CollectGrowthFund] = function(){return new Msg_CLC_CollectGrowthFund();};
	dict[JsonMessageDefine.Msg_CLC_QueryFightingScoreRank] = function(){return new Msg_CLC_QueryFightingScoreRank();};
	dict[JsonMessageDefine.Msg_CLC_StoryMessage] = function(){return new Msg_CLC_StoryMessage();};
	dict[JsonMessageDefine.Msg_CLC_UnequipTalentCard] = function(){return new Msg_CLC_UnequipTalentCard();};
	dict[JsonMessageDefine.Msg_LC_AccountLoginResult] = function(){return new Msg_LC_AccountLoginResult();};
	dict[JsonMessageDefine.Msg_LC_ActivateAccountResult] = function(){return new Msg_LC_ActivateAccountResult();};
	dict[JsonMessageDefine.Msg_LC_AddAssetsResult] = function(){return new Msg_LC_AddAssetsResult();};
	dict[JsonMessageDefine.Msg_LC_AddFriendResult] = function(){return new Msg_LC_AddFriendResult();};
	dict[JsonMessageDefine.Msg_LC_AddItemResult] = function(){return new Msg_LC_AddItemResult();};
	dict[JsonMessageDefine.Msg_LC_AddItemsResult] = function(){return new Msg_LC_AddItemsResult();};
	dict[JsonMessageDefine.Msg_LC_AddTalentExperienceResult] = function(){return new Msg_LC_AddTalentExperienceResult();};
	dict[JsonMessageDefine.Msg_LC_AddXSoulExperienceResult] = function(){return new Msg_LC_AddXSoulExperienceResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaBuyFightCountResult] = function(){return new Msg_LC_ArenaBuyFightCountResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaChallengeResult] = function(){return new Msg_LC_ArenaChallengeResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaChangePartnerResult] = function(){return new Msg_LC_ArenaChangePartnerResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaInfoResult] = function(){return new Msg_LC_ArenaInfoResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaMatchGroupResult] = function(){return new Msg_LC_ArenaMatchGroupResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaQueryHistoryResult] = function(){return new Msg_LC_ArenaQueryHistoryResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaQueryRankResult] = function(){return new Msg_LC_ArenaQueryRankResult();};
	dict[JsonMessageDefine.Msg_LC_ArenaStartCallengeResult] = function(){return new Msg_LC_ArenaStartCallengeResult();};
	dict[JsonMessageDefine.Msg_LC_AuctionAuction] = function(){return new Msg_LC_AuctionAuction();};
	dict[JsonMessageDefine.Msg_LC_AuctionOpResult] = function(){return new Msg_LC_AuctionOpResult();};
	dict[JsonMessageDefine.Msg_LC_AuctionSelfAuction] = function(){return new Msg_LC_AuctionSelfAuction();};
	dict[JsonMessageDefine.Msg_LC_AwardFashionResult] = function(){return new Msg_LC_AwardFashionResult();};
	dict[JsonMessageDefine.Msg_LC_BuyEliteCountResult] = function(){return new Msg_LC_BuyEliteCountResult();};
	dict[JsonMessageDefine.Msg_LC_BuyFashionResult] = function(){return new Msg_LC_BuyFashionResult();};
	dict[JsonMessageDefine.Msg_LC_BuyGoodsSucceed] = function(){return new Msg_LC_BuyGoodsSucceed();};
	dict[JsonMessageDefine.Msg_LC_BuyLifeResult] = function(){return new Msg_LC_BuyLifeResult();};
	dict[JsonMessageDefine.Msg_LC_BuyPartnerCombatTicketResult] = function(){return new Msg_LC_BuyPartnerCombatTicketResult();};
	dict[JsonMessageDefine.Msg_LC_BuyStaminaResult] = function(){return new Msg_LC_BuyStaminaResult();};
	dict[JsonMessageDefine.Msg_LC_BuyTDFightCountResult] = function(){return new Msg_LC_BuyTDFightCountResult();};
	dict[JsonMessageDefine.Msg_LC_CancelSelectPartnerResult] = function(){return new Msg_LC_CancelSelectPartnerResult();};
	dict[JsonMessageDefine.Msg_LC_ChangeCaptain] = function(){return new Msg_LC_ChangeCaptain();};
	dict[JsonMessageDefine.Msg_LC_ChatAddShieldResult] = function(){return new Msg_LC_ChatAddShieldResult();};
	dict[JsonMessageDefine.Msg_LC_ChatDelShieldResult] = function(){return new Msg_LC_ChatDelShieldResult();};
	dict[JsonMessageDefine.Msg_LC_ChatEquipInfoReturn] = function(){return new Msg_LC_ChatEquipInfoReturn();};
	dict[JsonMessageDefine.Msg_LC_ChatResult] = function(){return new Msg_LC_ChatResult();};
	dict[JsonMessageDefine.Msg_LC_ChatRoleInfoReturn] = function(){return new Msg_LC_ChatRoleInfoReturn();};
	dict[JsonMessageDefine.Msg_LC_ChatShieldListReturn] = function(){return new Msg_LC_ChatShieldListReturn();};
	dict[JsonMessageDefine.Msg_LC_ChatStatus] = function(){return new Msg_LC_ChatStatus();};
	dict[JsonMessageDefine.Msg_LC_ChatWorldResult] = function(){return new Msg_LC_ChatWorldResult();};
	dict[JsonMessageDefine.Msg_LC_CompoundEquipResult] = function(){return new Msg_LC_CompoundEquipResult();};
	dict[JsonMessageDefine.Msg_LC_CompoundPartnerResult] = function(){return new Msg_LC_CompoundPartnerResult();};
	dict[JsonMessageDefine.Msg_LC_ConfirmJoinGroupResult] = function(){return new Msg_LC_ConfirmJoinGroupResult();};
	dict[JsonMessageDefine.Msg_LC_CorpsSignIn] = function(){return new Msg_LC_CorpsSignIn();};
	dict[JsonMessageDefine.Msg_LC_CreateNicnameResult] = function(){return new Msg_LC_CreateNicnameResult();};
	dict[JsonMessageDefine.Msg_LC_CreateRoleResult] = function(){return new Msg_LC_CreateRoleResult();};
	dict[JsonMessageDefine.Msg_LC_DelFriendResult] = function(){return new Msg_LC_DelFriendResult();};
	dict[JsonMessageDefine.Msg_LC_DiamondExtraBuyBoxResult] = function(){return new Msg_LC_DiamondExtraBuyBoxResult();};
	dict[JsonMessageDefine.Msg_LC_DiscardItemResult] = function(){return new Msg_LC_DiscardItemResult();};
	dict[JsonMessageDefine.Msg_LC_DrawRewardResult] = function(){return new Msg_LC_DrawRewardResult();};
	dict[JsonMessageDefine.Msg_LC_EndPartnerBattleResult] = function(){return new Msg_LC_EndPartnerBattleResult();};
	dict[JsonMessageDefine.Msg_LC_EquipmentStrengthResult] = function(){return new Msg_LC_EquipmentStrengthResult();};
	dict[JsonMessageDefine.Msg_LC_EquipTalentCardResult] = function(){return new Msg_LC_EquipTalentCardResult();};
	dict[JsonMessageDefine.Msg_LC_ExchangeGiftResult] = function(){return new Msg_LC_ExchangeGiftResult();};
	dict[JsonMessageDefine.Msg_LC_ExchangeGoodsResult] = function(){return new Msg_LC_ExchangeGoodsResult();};
	dict[JsonMessageDefine.Msg_LC_ExpeditionAwardResult] = function(){return new Msg_LC_ExpeditionAwardResult();};
	dict[JsonMessageDefine.Msg_LC_ExpeditionResetResult] = function(){return new Msg_LC_ExpeditionResetResult();};
	dict[JsonMessageDefine.Msg_LC_ExpeditionSweepResult] = function(){return new Msg_LC_ExpeditionSweepResult();};
	dict[JsonMessageDefine.Msg_LC_FinishExpeditionResult] = function(){return new Msg_LC_FinishExpeditionResult();};
	dict[JsonMessageDefine.Msg_LC_FinishMissionResult] = function(){return new Msg_LC_FinishMissionResult();};
	dict[JsonMessageDefine.Msg_LC_FriendOffline] = function(){return new Msg_LC_FriendOffline();};
	dict[JsonMessageDefine.Msg_LC_FriendOnline] = function(){return new Msg_LC_FriendOnline();};
	dict[JsonMessageDefine.Msg_LC_GainFirstPayRewardResult] = function(){return new Msg_LC_GainFirstPayRewardResult();};
	dict[JsonMessageDefine.Msg_LC_GainVipRewardResult] = function(){return new Msg_LC_GainVipRewardResult();};
	dict[JsonMessageDefine.Msg_LC_GetLoginLotteryResult] = function(){return new Msg_LC_GetLoginLotteryResult();};
	dict[JsonMessageDefine.Msg_LC_GetMorrowRewardResult] = function(){return new Msg_LC_GetMorrowRewardResult();};
	dict[JsonMessageDefine.Msg_LC_GetOnlineTimeRewardResult] = function(){return new Msg_LC_GetOnlineTimeRewardResult();};
	dict[JsonMessageDefine.Msg_LC_GetPartner] = function(){return new Msg_LC_GetPartner();};
	dict[JsonMessageDefine.Msg_LC_GmCode] = function(){return new Msg_LC_GmCode();};
	dict[JsonMessageDefine.Msg_LC_GmPayResult] = function(){return new Msg_LC_GmPayResult();};
	dict[JsonMessageDefine.Msg_LC_InteractivePrize] = function(){return new Msg_LC_InteractivePrize();};
	dict[JsonMessageDefine.Msg_LC_InviteInfoAfterRoleEnter] = function(){return new Msg_LC_InviteInfoAfterRoleEnter();};
	dict[JsonMessageDefine.Msg_LC_ItemsGiveBack] = function(){return new Msg_LC_ItemsGiveBack();};
	dict[JsonMessageDefine.Msg_LC_ItemUseResult] = function(){return new Msg_LC_ItemUseResult();};
	dict[JsonMessageDefine.Msg_LC_LackOfSpace] = function(){return new Msg_LC_LackOfSpace();};
	dict[JsonMessageDefine.Msg_LC_LiftSkillResult] = function(){return new Msg_LC_LiftSkillResult();};
	dict[JsonMessageDefine.Msg_LC_LootChangeDefenseOrder] = function(){return new Msg_LC_LootChangeDefenseOrder();};
	dict[JsonMessageDefine.Msg_LC_LootChangeLootOrder] = function(){return new Msg_LC_LootChangeLootOrder();};
	dict[JsonMessageDefine.Msg_LC_LootCostVitality] = function(){return new Msg_LC_LootCostVitality();};
	dict[JsonMessageDefine.Msg_LC_LootMatchResult] = function(){return new Msg_LC_LootMatchResult();};
	dict[JsonMessageDefine.Msg_LC_LootOpResult] = function(){return new Msg_LC_LootOpResult();};
	dict[JsonMessageDefine.Msg_LC_MatchResult] = function(){return new Msg_LC_MatchResult();};
	dict[JsonMessageDefine.Msg_LC_MidasTouchResult] = function(){return new Msg_LC_MidasTouchResult();};
	dict[JsonMessageDefine.Msg_LC_MissionCompleted] = function(){return new Msg_LC_MissionCompleted();};
	dict[JsonMessageDefine.Msg_LC_MorrowRewardActive] = function(){return new Msg_LC_MorrowRewardActive();};
	dict[JsonMessageDefine.Msg_LC_MountEquipmentResult] = function(){return new Msg_LC_MountEquipmentResult();};
	dict[JsonMessageDefine.Msg_LC_MountFashionResult] = function(){return new Msg_LC_MountFashionResult();};
	dict[JsonMessageDefine.Msg_LC_MountSkillResult] = function(){return new Msg_LC_MountSkillResult();};
	dict[JsonMessageDefine.Msg_LC_MpveAwardResult] = function(){return new Msg_LC_MpveAwardResult();};
	dict[JsonMessageDefine.Msg_LC_MpveGeneralResult] = function(){return new Msg_LC_MpveGeneralResult();};
	dict[JsonMessageDefine.Msg_LC_NoticeFashionOverdue] = function(){return new Msg_LC_NoticeFashionOverdue();};
	dict[JsonMessageDefine.Msg_LC_NoticeFashionOverdueSoon] = function(){return new Msg_LC_NoticeFashionOverdueSoon();};
	dict[JsonMessageDefine.Msg_LC_NoticePlayerOffline] = function(){return new Msg_LC_NoticePlayerOffline();};
	dict[JsonMessageDefine.Msg_LC_NoticeQuitGroup] = function(){return new Msg_LC_NoticeQuitGroup();};
	dict[JsonMessageDefine.Msg_LC_NotifyMorrowReward] = function(){return new Msg_LC_NotifyMorrowReward();};
	dict[JsonMessageDefine.Msg_LC_NotifyNewMail] = function(){return new Msg_LC_NotifyNewMail();};
	dict[JsonMessageDefine.Msg_LC_OverLootResult] = function(){return new Msg_LC_OverLootResult();};
	dict[JsonMessageDefine.Msg_LC_PartnerCombatInfo] = function(){return new Msg_LC_PartnerCombatInfo();};
	dict[JsonMessageDefine.Msg_LC_PartnerEquipResult] = function(){return new Msg_LC_PartnerEquipResult();};
	dict[JsonMessageDefine.Msg_LC_QueryCorpsByName] = function(){return new Msg_LC_QueryCorpsByName();};
	dict[JsonMessageDefine.Msg_LC_QueryFriendInfoResult] = function(){return new Msg_LC_QueryFriendInfoResult();};
	dict[JsonMessageDefine.Msg_LC_QueryTDMatchGroupResult] = function(){return new Msg_LC_QueryTDMatchGroupResult();};
	dict[JsonMessageDefine.Msg_LC_QueueingCountResult] = function(){return new Msg_LC_QueueingCountResult();};
	dict[JsonMessageDefine.Msg_LC_RefreshExchangeResult] = function(){return new Msg_LC_RefreshExchangeResult();};
	dict[JsonMessageDefine.Msg_LC_RefreshPartnerCombatResult] = function(){return new Msg_LC_RefreshPartnerCombatResult();};
	dict[JsonMessageDefine.Msg_LC_RequestDare] = function(){return new Msg_LC_RequestDare();};
	dict[JsonMessageDefine.Msg_LC_RequestDareResult] = function(){return new Msg_LC_RequestDareResult();};
	dict[JsonMessageDefine.Msg_LC_RequestEnhanceEquipmentStar] = function(){return new Msg_LC_RequestEnhanceEquipmentStar();};
	dict[JsonMessageDefine.Msg_LC_RequestExpeditionResult] = function(){return new Msg_LC_RequestExpeditionResult();};
	dict[JsonMessageDefine.Msg_LC_RequestGowPrizeResult] = function(){return new Msg_LC_RequestGowPrizeResult();};
	dict[JsonMessageDefine.Msg_LC_RequestInviteResult] = function(){return new Msg_LC_RequestInviteResult();};
	dict[JsonMessageDefine.Msg_LC_RequestInviteRewardResult] = function(){return new Msg_LC_RequestInviteRewardResult();};
	dict[JsonMessageDefine.Msg_LC_RequestItemUseResult] = function(){return new Msg_LC_RequestItemUseResult();};
	dict[JsonMessageDefine.Msg_LC_RequestJoinGroupResult] = function(){return new Msg_LC_RequestJoinGroupResult();};
	dict[JsonMessageDefine.Msg_LC_RequestUserPositionResult] = function(){return new Msg_LC_RequestUserPositionResult();};
	dict[JsonMessageDefine.Msg_LC_RequestUsersResult] = function(){return new Msg_LC_RequestUsersResult();};
	dict[JsonMessageDefine.Msg_LC_ResetConsumeGoodsCount] = function(){return new Msg_LC_ResetConsumeGoodsCount();};
	dict[JsonMessageDefine.Msg_LC_ResetCorpsSignIn] = function(){return new Msg_LC_ResetCorpsSignIn();};
	dict[JsonMessageDefine.Msg_LC_ResetDailyMissions] = function(){return new Msg_LC_ResetDailyMissions();};
	dict[JsonMessageDefine.Msg_LC_ResetOnlineTimeRewardData] = function(){return new Msg_LC_ResetOnlineTimeRewardData();};
	dict[JsonMessageDefine.Msg_LC_ResetWeeklyLoginRewardData] = function(){return new Msg_LC_ResetWeeklyLoginRewardData();};
	dict[JsonMessageDefine.Msg_LC_RoleEnterResult] = function(){return new Msg_LC_RoleEnterResult();};
	dict[JsonMessageDefine.Msg_LC_RoleListResult] = function(){return new Msg_LC_RoleListResult();};
	dict[JsonMessageDefine.Msg_LC_SecretAreaTrialAward] = function(){return new Msg_LC_SecretAreaTrialAward();};
	dict[JsonMessageDefine.Msg_LC_SecretAreaTrialResult] = function(){return new Msg_LC_SecretAreaTrialResult();};
	dict[JsonMessageDefine.Msg_LC_SelectPartnerResult] = function(){return new Msg_LC_SelectPartnerResult();};
	dict[JsonMessageDefine.Msg_LC_SellItemResult] = function(){return new Msg_LC_SellItemResult();};
	dict[JsonMessageDefine.Msg_LC_SendScreenTip] = function(){return new Msg_LC_SendScreenTip();};
	dict[JsonMessageDefine.Msg_LC_ServerShutdown] = function(){return new Msg_LC_ServerShutdown();};
	dict[JsonMessageDefine.Msg_LC_SetFashionShowResult] = function(){return new Msg_LC_SetFashionShowResult();};
	dict[JsonMessageDefine.Msg_LC_SignInAndGetRewardResult] = function(){return new Msg_LC_SignInAndGetRewardResult();};
	dict[JsonMessageDefine.Msg_LC_StageClearResult] = function(){return new Msg_LC_StageClearResult();};
	dict[JsonMessageDefine.Msg_LC_StartGameResult] = function(){return new Msg_LC_StartGameResult();};
	dict[JsonMessageDefine.Msg_LC_StartLootResult] = function(){return new Msg_LC_StartLootResult();};
	dict[JsonMessageDefine.Msg_LC_StartPartnerBattleResult] = function(){return new Msg_LC_StartPartnerBattleResult();};
	dict[JsonMessageDefine.Msg_LC_StartTDChallengeResult] = function(){return new Msg_LC_StartTDChallengeResult();};
	dict[JsonMessageDefine.Msg_LC_SwapSkillResult] = function(){return new Msg_LC_SwapSkillResult();};
	dict[JsonMessageDefine.Msg_LC_SweepStageResult] = function(){return new Msg_LC_SweepStageResult();};
	dict[JsonMessageDefine.Msg_LC_SyncCombatData] = function(){return new Msg_LC_SyncCombatData();};
	dict[JsonMessageDefine.Msg_LC_SyncCorpsDareCount] = function(){return new Msg_LC_SyncCorpsDareCount();};
	dict[JsonMessageDefine.Msg_LC_SyncCorpsInfo] = function(){return new Msg_LC_SyncCorpsInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncCorpsOpResult] = function(){return new Msg_LC_SyncCorpsOpResult();};
	dict[JsonMessageDefine.Msg_LC_SyncCorpsStar] = function(){return new Msg_LC_SyncCorpsStar();};
	dict[JsonMessageDefine.Msg_LC_SyncFightingScore] = function(){return new Msg_LC_SyncFightingScore();};
	dict[JsonMessageDefine.Msg_LC_SyncFriendList] = function(){return new Msg_LC_SyncFriendList();};
	dict[JsonMessageDefine.Msg_LC_SyncGoldTollgateInfo] = function(){return new Msg_LC_SyncGoldTollgateInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncGowBattleResult] = function(){return new Msg_LC_SyncGowBattleResult();};
	dict[JsonMessageDefine.Msg_LC_SyncGowOtherInfo] = function(){return new Msg_LC_SyncGowOtherInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncGowRankInfo] = function(){return new Msg_LC_SyncGowRankInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncGowStarList] = function(){return new Msg_LC_SyncGowStarList();};
	dict[JsonMessageDefine.Msg_LC_SyncGroupUsers] = function(){return new Msg_LC_SyncGroupUsers();};
	dict[JsonMessageDefine.Msg_LC_SyncGuideFlag] = function(){return new Msg_LC_SyncGuideFlag();};
	dict[JsonMessageDefine.Msg_LC_SyncHomeNotice] = function(){return new Msg_LC_SyncHomeNotice();};
	dict[JsonMessageDefine.Msg_LC_SyncLeaveGroup] = function(){return new Msg_LC_SyncLeaveGroup();};
	dict[JsonMessageDefine.Msg_LC_SyncLootAward] = function(){return new Msg_LC_SyncLootAward();};
	dict[JsonMessageDefine.Msg_LC_SyncLootHistory] = function(){return new Msg_LC_SyncLootHistory();};
	dict[JsonMessageDefine.Msg_LC_SyncLootInfo] = function(){return new Msg_LC_SyncLootInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncLotteryInfo] = function(){return new Msg_LC_SyncLotteryInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncMailList] = function(){return new Msg_LC_SyncMailList();};
	dict[JsonMessageDefine.Msg_LC_SyncMpveInfo] = function(){return new Msg_LC_SyncMpveInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncNewbieActionFlag] = function(){return new Msg_LC_SyncNewbieActionFlag();};
	dict[JsonMessageDefine.Msg_LC_SyncNewbieFlag] = function(){return new Msg_LC_SyncNewbieFlag();};
	dict[JsonMessageDefine.Msg_LC_SyncNoticeContent] = function(){return new Msg_LC_SyncNoticeContent();};
	dict[JsonMessageDefine.Msg_LC_SyncPinviteTeam] = function(){return new Msg_LC_SyncPinviteTeam();};
	dict[JsonMessageDefine.Msg_LC_SyncPlayerDetail] = function(){return new Msg_LC_SyncPlayerDetail();};
	dict[JsonMessageDefine.Msg_LC_SyncPlayerInfo] = function(){return new Msg_LC_SyncPlayerInfo();};
	dict[JsonMessageDefine.Msg_LC_SyncQuitRoom] = function(){return new Msg_LC_SyncQuitRoom();};
	dict[JsonMessageDefine.Msg_LC_SyncRecentLoginState] = function(){return new Msg_LC_SyncRecentLoginState();};
	dict[JsonMessageDefine.Msg_LC_SyncResetGowPrize] = function(){return new Msg_LC_SyncResetGowPrize();};
	dict[JsonMessageDefine.Msg_LC_SyncSignInCount] = function(){return new Msg_LC_SyncSignInCount();};
	dict[JsonMessageDefine.Msg_LC_SyncSkillInfos] = function(){return new Msg_LC_SyncSkillInfos();};
	dict[JsonMessageDefine.Msg_LC_SyncStamina] = function(){return new Msg_LC_SyncStamina();};
	dict[JsonMessageDefine.Msg_LC_SyncValidCorpsList] = function(){return new Msg_LC_SyncValidCorpsList();};
	dict[JsonMessageDefine.Msg_LC_SyncVigor] = function(){return new Msg_LC_SyncVigor();};
	dict[JsonMessageDefine.Msg_LC_SyncVitality] = function(){return new Msg_LC_SyncVitality();};
	dict[JsonMessageDefine.Msg_LC_SystemChatWorldResult] = function(){return new Msg_LC_SystemChatWorldResult();};
	dict[JsonMessageDefine.Msg_LC_TDChallengeResult] = function(){return new Msg_LC_TDChallengeResult();};
	dict[JsonMessageDefine.Msg_LC_UnlockLegacyResult] = function(){return new Msg_LC_UnlockLegacyResult();};
	dict[JsonMessageDefine.Msg_LC_UnlockSkillResult] = function(){return new Msg_LC_UnlockSkillResult();};
	dict[JsonMessageDefine.Msg_LC_UnmountEquipmentResult] = function(){return new Msg_LC_UnmountEquipmentResult();};
	dict[JsonMessageDefine.Msg_LC_UnmountFashionResult] = function(){return new Msg_LC_UnmountFashionResult();};
	dict[JsonMessageDefine.Msg_LC_UnmountSkillResult] = function(){return new Msg_LC_UnmountSkillResult();};
	dict[JsonMessageDefine.Msg_LC_UpdateInviteInfo] = function(){return new Msg_LC_UpdateInviteInfo();};
	dict[JsonMessageDefine.Msg_LC_UpgradeEquipBatch] = function(){return new Msg_LC_UpgradeEquipBatch();};
	dict[JsonMessageDefine.Msg_LC_UpgradeItemResult] = function(){return new Msg_LC_UpgradeItemResult();};
	dict[JsonMessageDefine.Msg_LC_UpgradeLegacyResult] = function(){return new Msg_LC_UpgradeLegacyResult();};
	dict[JsonMessageDefine.Msg_LC_UpgradeParnerStageResult] = function(){return new Msg_LC_UpgradeParnerStageResult();};
	dict[JsonMessageDefine.Msg_LC_UpgradePartnerLevelResult] = function(){return new Msg_LC_UpgradePartnerLevelResult();};
	dict[JsonMessageDefine.Msg_LC_UpgradeSkillResult] = function(){return new Msg_LC_UpgradeSkillResult();};
	dict[JsonMessageDefine.Msg_LC_UserLevelup] = function(){return new Msg_LC_UserLevelup();};
	dict[JsonMessageDefine.Msg_LC_WeeklyLoginRewardResult] = function(){return new Msg_LC_WeeklyLoginRewardResult();};
	dict[JsonMessageDefine.Msg_LC_XSoulChangeShowModelResult] = function(){return new Msg_LC_XSoulChangeShowModelResult();};
	dict[JsonMessageDefine.NodeRegister] = function(){return new NodeRegister();};
	dict[JsonMessageDefine.NodeRegisterResult] = function(){return new NodeRegisterResult();};
	dict[JsonMessageDefine.PartnerDataMsg] = function(){return new PartnerDataMsg();};
	dict[JsonMessageDefine.PaymentRebateDataMsg] = function(){return new PaymentRebateDataMsg();};
	dict[JsonMessageDefine.SelectedPartnerDataMsg] = function(){return new SelectedPartnerDataMsg();};
	dict[JsonMessageDefine.SkillDataInfo] = function(){return new SkillDataInfo();};
	dict[JsonMessageDefine.TDMatchInfoMsg] = function(){return new TDMatchInfoMsg();};
	dict[JsonMessageDefine.TooManyOperations] = function(){return new TooManyOperations();};
	dict[JsonMessageDefine.UserHeartbeat] = function(){return new UserHeartbeat();};
	dict[JsonMessageDefine.VersionVerify] = function(){return new VersionVerify();};
	dict[JsonMessageDefine.VersionVerifyResult] = function(){return new VersionVerifyResult();};
	dict[JsonMessageDefine.XSoulDataMsg] = function(){return new XSoulDataMsg();};

	function newObject(id){
	  var factory = dict[id];
	  if(factory){
	    return factory();
	  }
	  return null;
	}
	  
	return {
	  newObject : newObject
	};
}

exports.GameFrameworkMessage = {

	JsonMessageDefine : JsonMessageDefine,
	JsonMessageDefine2Object : new JsonMessageDefine2Object(),

	AccountLogout : AccountLogout,
	ArenaInfoMsg : ArenaInfoMsg,
	AuctionInfo : AuctionInfo,
	ChallengeEntityData : ChallengeEntityData,
	ChallengeInfoData : ChallengeInfoData,
	ChatShieldInfoForMsg : ChatShieldInfoForMsg,
	DamageInfoData : DamageInfoData,
	DirectLogin : DirectLogin,
	FashionHideMsg : FashionHideMsg,
	FashionMsg : FashionMsg,
	FashionSynMsg : FashionSynMsg,
	FightingScoreEntityMsg : FightingScoreEntityMsg,
	FriendInfoForMsg : FriendInfoForMsg,
	GetQueueingCount : GetQueueingCount,
	GowDataMsg : GowDataMsg,
	ItemInfo_UseItem : ItemInfo_UseItem,
	ItemLeftMsg : ItemLeftMsg,
	JsonItemDataMsg : JsonItemDataMsg,
	JsonTalentDataMsg : JsonTalentDataMsg,
	KickUser : KickUser,
	LegacyDataMsg : LegacyDataMsg,
	Logout : Logout,
	LootEntityData : LootEntityData,
	LootHistoryData : LootHistoryData,
	LootInfoMsg : LootInfoMsg,
	MailInfoForMessage : MailInfoForMessage,
	MailItemForMessage : MailItemForMessage,
	MissionInfoForSync : MissionInfoForSync,
	MorrowRewardInfo : MorrowRewardInfo,
	Msg_CL_AcceptedDare : Msg_CL_AcceptedDare,
	Msg_CL_AccountLogin : Msg_CL_AccountLogin,
	Msg_CL_ActivateAccount : Msg_CL_ActivateAccount,
	Msg_CL_AddAssets : Msg_CL_AddAssets,
	Msg_CL_AddFriend : Msg_CL_AddFriend,
	Msg_CL_AddItem : Msg_CL_AddItem,
	Msg_CL_AddTalentExperience : Msg_CL_AddTalentExperience,
	Msg_CL_AddXSoulExperience : Msg_CL_AddXSoulExperience,
	Msg_CL_ArenaBeginFight : Msg_CL_ArenaBeginFight,
	Msg_CL_ArenaBuyFightCount : Msg_CL_ArenaBuyFightCount,
	Msg_CL_ArenaChallengeOver : Msg_CL_ArenaChallengeOver,
	Msg_CL_ArenaChangePartner : Msg_CL_ArenaChangePartner,
	Msg_CL_ArenaQueryHistory : Msg_CL_ArenaQueryHistory,
	Msg_CL_ArenaQueryRank : Msg_CL_ArenaQueryRank,
	Msg_CL_ArenaStartChallenge : Msg_CL_ArenaStartChallenge,
	Msg_CL_AuctionBuy : Msg_CL_AuctionBuy,
	Msg_CL_AuctionQuery : Msg_CL_AuctionQuery,
	Msg_CL_AuctionReceive : Msg_CL_AuctionReceive,
	Msg_CL_AuctionSelfAuction : Msg_CL_AuctionSelfAuction,
	Msg_CL_AuctionSell : Msg_CL_AuctionSell,
	Msg_CL_AuctionUnshelve : Msg_CL_AuctionUnshelve,
	Msg_CL_BuyEliteCount : Msg_CL_BuyEliteCount,
	Msg_CL_BuyFashion : Msg_CL_BuyFashion,
	Msg_CL_BuyLife : Msg_CL_BuyLife,
	Msg_CL_BuyPartnerCombatTicket : Msg_CL_BuyPartnerCombatTicket,
	Msg_CL_BuyStamina : Msg_CL_BuyStamina,
	Msg_CL_BuyTDFightCount : Msg_CL_BuyTDFightCount,
	Msg_CL_CancelMatch : Msg_CL_CancelMatch,
	Msg_CL_CancelSelectPartner : Msg_CL_CancelSelectPartner,
	Msg_CL_ChangeCityRoom : Msg_CL_ChangeCityRoom,
	Msg_CL_ChangeFieldRoom : Msg_CL_ChangeFieldRoom,
	Msg_CL_ChangeScene : Msg_CL_ChangeScene,
	Msg_CL_ChatAddShield : Msg_CL_ChatAddShield,
	Msg_CL_ChatAddShieldByName : Msg_CL_ChatAddShieldByName,
	Msg_CL_ChatDelShield : Msg_CL_ChatDelShield,
	Msg_CL_CombinTalentCard : Msg_CL_CombinTalentCard,
	Msg_CL_CombinTalentCardResult : Msg_CL_CombinTalentCardResult,
	Msg_CL_CompoundEquip : Msg_CL_CompoundEquip,
	Msg_CL_CompoundPartner : Msg_CL_CompoundPartner,
	Msg_CL_ConfirmFriend : Msg_CL_ConfirmFriend,
	Msg_CL_ConfirmJoinGroup : Msg_CL_ConfirmJoinGroup,
	Msg_CL_CorpsAgreeClaimer : Msg_CL_CorpsAgreeClaimer,
	Msg_CL_CorpsAppoint : Msg_CL_CorpsAppoint,
	Msg_CL_CorpsClearRequest : Msg_CL_CorpsClearRequest,
	Msg_CL_CorpsCreate : Msg_CL_CorpsCreate,
	Msg_CL_CorpsDissolve : Msg_CL_CorpsDissolve,
	Msg_CL_CorpsIndirectJoin : Msg_CL_CorpsIndirectJoin,
	Msg_CL_CorpsJoin : Msg_CL_CorpsJoin,
	Msg_CL_CorpsKickout : Msg_CL_CorpsKickout,
	Msg_CL_CorpsQuit : Msg_CL_CorpsQuit,
	Msg_CL_CorpsRefuseClaimer : Msg_CL_CorpsRefuseClaimer,
	Msg_CL_CorpsSignIn : Msg_CL_CorpsSignIn,
	Msg_CL_CostVitality : Msg_CL_CostVitality,
	Msg_CL_CreateNickname : Msg_CL_CreateNickname,
	Msg_CL_CreateRole : Msg_CL_CreateRole,
	Msg_CL_DeleteFriend : Msg_CL_DeleteFriend,
	Msg_CL_DiamondExtraBuyBox : Msg_CL_DiamondExtraBuyBox,
	Msg_CL_DiscardItem : Msg_CL_DiscardItem,
	Msg_CL_DrawReward : Msg_CL_DrawReward,
	Msg_CL_EndPartnerBattle : Msg_CL_EndPartnerBattle,
	Msg_CL_EnterField : Msg_CL_EnterField,
	Msg_CL_EquipmentStrength : Msg_CL_EquipmentStrength,
	Msg_CL_EquipTalentCard : Msg_CL_EquipTalentCard,
	Msg_CL_ExchangeGift : Msg_CL_ExchangeGift,
	Msg_CL_ExchangeGoods : Msg_CL_ExchangeGoods,
	Msg_CL_ExpeditionAward : Msg_CL_ExpeditionAward,
	Msg_CL_ExpeditionFailure : Msg_CL_ExpeditionFailure,
	Msg_CL_ExpeditionReset : Msg_CL_ExpeditionReset,
	Msg_CL_ExpeditionSweep : Msg_CL_ExpeditionSweep,
	Msg_CL_FinishExpedition : Msg_CL_FinishExpedition,
	Msg_CL_FinishMission : Msg_CL_FinishMission,
	Msg_CL_FriendList : Msg_CL_FriendList,
	Msg_CL_GainFirstPayReward : Msg_CL_GainFirstPayReward,
	Msg_CL_GainVipReward : Msg_CL_GainVipReward,
	Msg_CL_GetGowStarList : Msg_CL_GetGowStarList,
	Msg_CL_GetLoginLottery : Msg_CL_GetLoginLottery,
	Msg_CL_GetLootAward : Msg_CL_GetLootAward,
	Msg_CL_GetLootHistory : Msg_CL_GetLootHistory,
	Msg_CL_GetMailList : Msg_CL_GetMailList,
	Msg_CL_GetMorrowReward : Msg_CL_GetMorrowReward,
	Msg_CL_GetOnlineTimeReward : Msg_CL_GetOnlineTimeReward,
	Msg_CL_GetQueueingCount : Msg_CL_GetQueueingCount,
	Msg_CL_GmPay : Msg_CL_GmPay,
	Msg_CL_GMResetDailyMissions : Msg_CL_GMResetDailyMissions,
	Msg_CL_InteractivePrize : Msg_CL_InteractivePrize,
	Msg_CL_ItemUseRequest : Msg_CL_ItemUseRequest,
	Msg_CL_LiftSkill : Msg_CL_LiftSkill,
	Msg_CL_LootMatchTarget : Msg_CL_LootMatchTarget,
	Msg_CL_MidasTouch : Msg_CL_MidasTouch,
	Msg_CL_MountEquipment : Msg_CL_MountEquipment,
	Msg_CL_MountFashion : Msg_CL_MountFashion,
	Msg_CL_MountSkill : Msg_CL_MountSkill,
	Msg_CL_OpenCharpter : Msg_CL_OpenCharpter,
	Msg_CL_OpenDomain : Msg_CL_OpenDomain,
	Msg_CL_OverLoot : Msg_CL_OverLoot,
	Msg_CL_PartnerEquip : Msg_CL_PartnerEquip,
	Msg_CL_PinviteTeam : Msg_CL_PinviteTeam,
	Msg_CL_PushDelayData : Msg_CL_PushDelayData,
	Msg_CL_QueryArenaInfo : Msg_CL_QueryArenaInfo,
	Msg_CL_QueryArenaMatchGroup : Msg_CL_QueryArenaMatchGroup,
	Msg_CL_QueryCorpsByName : Msg_CL_QueryCorpsByName,
	Msg_CL_QueryCorpsInfo : Msg_CL_QueryCorpsInfo,
	Msg_CL_QueryCorpsStar : Msg_CL_QueryCorpsStar,
	Msg_CL_QueryExpeditionInfo : Msg_CL_QueryExpeditionInfo,
	Msg_CL_QueryFriendInfo : Msg_CL_QueryFriendInfo,
	Msg_CL_QueryLootInfo : Msg_CL_QueryLootInfo,
	Msg_CL_QuerySkillInfos : Msg_CL_QuerySkillInfos,
	Msg_CL_QueryTDMatchGroup : Msg_CL_QueryTDMatchGroup,
	Msg_CL_QueryValidCorpsList : Msg_CL_QueryValidCorpsList,
	Msg_CL_QuitGroup : Msg_CL_QuitGroup,
	Msg_CL_QuitPve : Msg_CL_QuitPve,
	Msg_CL_QuitRoom : Msg_CL_QuitRoom,
	Msg_CL_ReadMail : Msg_CL_ReadMail,
	Msg_CL_ReceiveMail : Msg_CL_ReceiveMail,
	Msg_CL_RecordNewbieFlag : Msg_CL_RecordNewbieFlag,
	Msg_CL_RefreshPartnerCombat : Msg_CL_RefreshPartnerCombat,
	Msg_CL_RefusedDare : Msg_CL_RefusedDare,
	Msg_CL_RefuseGroupRequest : Msg_CL_RefuseGroupRequest,
	Msg_CL_RequestDare : Msg_CL_RequestDare,
	Msg_CL_RequestDareByGuid : Msg_CL_RequestDareByGuid,
	Msg_CL_RequestEnhanceEquipmentStar : Msg_CL_RequestEnhanceEquipmentStar,
	Msg_CL_RequestExpedition : Msg_CL_RequestExpedition,
	Msg_CL_RequestGowBattleResult : Msg_CL_RequestGowBattleResult,
	Msg_CL_RequestGowPrize : Msg_CL_RequestGowPrize,
	Msg_CL_RequestGroupInfo : Msg_CL_RequestGroupInfo,
	Msg_CL_RequestInvite : Msg_CL_RequestInvite,
	Msg_CL_RequestInviteReward : Msg_CL_RequestInviteReward,
	Msg_CL_RequestJoinGroup : Msg_CL_RequestJoinGroup,
	Msg_CL_RequestMatch : Msg_CL_RequestMatch,
	Msg_CL_RequestMpveAward : Msg_CL_RequestMpveAward,
	Msg_CL_RequestPlayerDetail : Msg_CL_RequestPlayerDetail,
	Msg_CL_RequestPlayerInfo : Msg_CL_RequestPlayerInfo,
	Msg_CL_RequestRefreshExchange : Msg_CL_RequestRefreshExchange,
	Msg_CL_RequestSkillInfos : Msg_CL_RequestSkillInfos,
	Msg_CL_RequestUserPosition : Msg_CL_RequestUserPosition,
	Msg_CL_RequestUsers : Msg_CL_RequestUsers,
	Msg_CL_RequestVigor : Msg_CL_RequestVigor,
	Msg_CL_RequireChatEquipInfo : Msg_CL_RequireChatEquipInfo,
	Msg_CL_RequireChatRoleInfo : Msg_CL_RequireChatRoleInfo,
	Msg_CL_RequireChatShieldList : Msg_CL_RequireChatShieldList,
	Msg_CL_ResetCharpter : Msg_CL_ResetCharpter,
	Msg_CL_RoleEnter : Msg_CL_RoleEnter,
	Msg_CL_RoleList : Msg_CL_RoleList,
	Msg_CL_SaveSkillPreset : Msg_CL_SaveSkillPreset,
	Msg_CL_SecretAreaFightingInfo : Msg_CL_SecretAreaFightingInfo,
	Msg_CL_SecretAreaTrial : Msg_CL_SecretAreaTrial,
	Msg_CL_SelectPartner : Msg_CL_SelectPartner,
	Msg_CL_SellItem : Msg_CL_SellItem,
	Msg_CL_SendChat : Msg_CL_SendChat,
	Msg_CL_SetCorpsNotice : Msg_CL_SetCorpsNotice,
	Msg_CL_SetFashionShow : Msg_CL_SetFashionShow,
	Msg_CL_SetNewbieActionFlag : Msg_CL_SetNewbieActionFlag,
	Msg_CL_SetNewbieFlag : Msg_CL_SetNewbieFlag,
	Msg_CL_SignInAndGetReward : Msg_CL_SignInAndGetReward,
	Msg_CL_SinglePVE : Msg_CL_SinglePVE,
	Msg_CL_StageClear : Msg_CL_StageClear,
	Msg_CL_StartCorpsTollgate : Msg_CL_StartCorpsTollgate,
	Msg_CL_StartGame : Msg_CL_StartGame,
	Msg_CL_StartLoot : Msg_CL_StartLoot,
	Msg_CL_StartMpve : Msg_CL_StartMpve,
	Msg_CL_StartPartnerBattle : Msg_CL_StartPartnerBattle,
	Msg_CL_StartTDChallenge : Msg_CL_StartTDChallenge,
	Msg_CL_SwapSkill : Msg_CL_SwapSkill,
	Msg_CL_SweepStage : Msg_CL_SweepStage,
	Msg_CL_TDBeginFight : Msg_CL_TDBeginFight,
	Msg_CL_TDChallengeOver : Msg_CL_TDChallengeOver,
	Msg_CL_UnlockSkill : Msg_CL_UnlockSkill,
	Msg_CL_UnmountEquipment : Msg_CL_UnmountEquipment,
	Msg_CL_UnmountFashion : Msg_CL_UnmountFashion,
	Msg_CL_UnmountSkill : Msg_CL_UnmountSkill,
	Msg_CL_UpdateActivityUnlock : Msg_CL_UpdateActivityUnlock,
	Msg_CL_UpdateFightingScore : Msg_CL_UpdateFightingScore,
	Msg_CL_UpdatePosition : Msg_CL_UpdatePosition,
	Msg_CL_UpgradeEquipBatch : Msg_CL_UpgradeEquipBatch,
	Msg_CL_UpgradeItem : Msg_CL_UpgradeItem,
	Msg_CL_UpgradeLegacy : Msg_CL_UpgradeLegacy,
	Msg_CL_UpgradePartnerLevel : Msg_CL_UpgradePartnerLevel,
	Msg_CL_UpgradePartnerStage : Msg_CL_UpgradePartnerStage,
	Msg_CL_UpgradeSkill : Msg_CL_UpgradeSkill,
	Msg_CL_UploadFPS : Msg_CL_UploadFPS,
	Msg_CL_WeeklyLoginReward : Msg_CL_WeeklyLoginReward,
	Msg_CL_XSoulChangeShowModel : Msg_CL_XSoulChangeShowModel,
	Msg_CLC_CollectChapterAward : Msg_CLC_CollectChapterAward,
	Msg_CLC_CollectGrowthFund : Msg_CLC_CollectGrowthFund,
	Msg_CLC_QueryFightingScoreRank : Msg_CLC_QueryFightingScoreRank,
	Msg_CLC_StoryMessage : Msg_CLC_StoryMessage,
	Msg_CLC_UnequipTalentCard : Msg_CLC_UnequipTalentCard,
	Msg_LC_AccountLoginResult : Msg_LC_AccountLoginResult,
	Msg_LC_ActivateAccountResult : Msg_LC_ActivateAccountResult,
	Msg_LC_AddAssetsResult : Msg_LC_AddAssetsResult,
	Msg_LC_AddFriendResult : Msg_LC_AddFriendResult,
	Msg_LC_AddItemResult : Msg_LC_AddItemResult,
	Msg_LC_AddItemsResult : Msg_LC_AddItemsResult,
	Msg_LC_AddTalentExperienceResult : Msg_LC_AddTalentExperienceResult,
	Msg_LC_AddXSoulExperienceResult : Msg_LC_AddXSoulExperienceResult,
	Msg_LC_ArenaBuyFightCountResult : Msg_LC_ArenaBuyFightCountResult,
	Msg_LC_ArenaChallengeResult : Msg_LC_ArenaChallengeResult,
	Msg_LC_ArenaChangePartnerResult : Msg_LC_ArenaChangePartnerResult,
	Msg_LC_ArenaInfoResult : Msg_LC_ArenaInfoResult,
	Msg_LC_ArenaMatchGroupResult : Msg_LC_ArenaMatchGroupResult,
	Msg_LC_ArenaQueryHistoryResult : Msg_LC_ArenaQueryHistoryResult,
	Msg_LC_ArenaQueryRankResult : Msg_LC_ArenaQueryRankResult,
	Msg_LC_ArenaStartCallengeResult : Msg_LC_ArenaStartCallengeResult,
	Msg_LC_AuctionAuction : Msg_LC_AuctionAuction,
	Msg_LC_AuctionOpResult : Msg_LC_AuctionOpResult,
	Msg_LC_AuctionSelfAuction : Msg_LC_AuctionSelfAuction,
	Msg_LC_AwardFashionResult : Msg_LC_AwardFashionResult,
	Msg_LC_BuyEliteCountResult : Msg_LC_BuyEliteCountResult,
	Msg_LC_BuyFashionResult : Msg_LC_BuyFashionResult,
	Msg_LC_BuyGoodsSucceed : Msg_LC_BuyGoodsSucceed,
	Msg_LC_BuyLifeResult : Msg_LC_BuyLifeResult,
	Msg_LC_BuyPartnerCombatTicketResult : Msg_LC_BuyPartnerCombatTicketResult,
	Msg_LC_BuyStaminaResult : Msg_LC_BuyStaminaResult,
	Msg_LC_BuyTDFightCountResult : Msg_LC_BuyTDFightCountResult,
	Msg_LC_CancelSelectPartnerResult : Msg_LC_CancelSelectPartnerResult,
	Msg_LC_ChangeCaptain : Msg_LC_ChangeCaptain,
	Msg_LC_ChatAddShieldResult : Msg_LC_ChatAddShieldResult,
	Msg_LC_ChatDelShieldResult : Msg_LC_ChatDelShieldResult,
	Msg_LC_ChatEquipInfoReturn : Msg_LC_ChatEquipInfoReturn,
	Msg_LC_ChatResult : Msg_LC_ChatResult,
	Msg_LC_ChatRoleInfoReturn : Msg_LC_ChatRoleInfoReturn,
	Msg_LC_ChatShieldListReturn : Msg_LC_ChatShieldListReturn,
	Msg_LC_ChatStatus : Msg_LC_ChatStatus,
	Msg_LC_ChatWorldResult : Msg_LC_ChatWorldResult,
	Msg_LC_CompoundEquipResult : Msg_LC_CompoundEquipResult,
	Msg_LC_CompoundPartnerResult : Msg_LC_CompoundPartnerResult,
	Msg_LC_ConfirmJoinGroupResult : Msg_LC_ConfirmJoinGroupResult,
	Msg_LC_CorpsSignIn : Msg_LC_CorpsSignIn,
	Msg_LC_CreateNicnameResult : Msg_LC_CreateNicnameResult,
	Msg_LC_CreateRoleResult : Msg_LC_CreateRoleResult,
	Msg_LC_DelFriendResult : Msg_LC_DelFriendResult,
	Msg_LC_DiamondExtraBuyBoxResult : Msg_LC_DiamondExtraBuyBoxResult,
	Msg_LC_DiscardItemResult : Msg_LC_DiscardItemResult,
	Msg_LC_DrawRewardResult : Msg_LC_DrawRewardResult,
	Msg_LC_EndPartnerBattleResult : Msg_LC_EndPartnerBattleResult,
	Msg_LC_EquipmentStrengthResult : Msg_LC_EquipmentStrengthResult,
	Msg_LC_EquipTalentCardResult : Msg_LC_EquipTalentCardResult,
	Msg_LC_ExchangeGiftResult : Msg_LC_ExchangeGiftResult,
	Msg_LC_ExchangeGoodsResult : Msg_LC_ExchangeGoodsResult,
	Msg_LC_ExpeditionAwardResult : Msg_LC_ExpeditionAwardResult,
	Msg_LC_ExpeditionResetResult : Msg_LC_ExpeditionResetResult,
	Msg_LC_ExpeditionSweepResult : Msg_LC_ExpeditionSweepResult,
	Msg_LC_FinishExpeditionResult : Msg_LC_FinishExpeditionResult,
	Msg_LC_FinishMissionResult : Msg_LC_FinishMissionResult,
	Msg_LC_FriendOffline : Msg_LC_FriendOffline,
	Msg_LC_FriendOnline : Msg_LC_FriendOnline,
	Msg_LC_GainFirstPayRewardResult : Msg_LC_GainFirstPayRewardResult,
	Msg_LC_GainVipRewardResult : Msg_LC_GainVipRewardResult,
	Msg_LC_GetLoginLotteryResult : Msg_LC_GetLoginLotteryResult,
	Msg_LC_GetMorrowRewardResult : Msg_LC_GetMorrowRewardResult,
	Msg_LC_GetOnlineTimeRewardResult : Msg_LC_GetOnlineTimeRewardResult,
	Msg_LC_GetPartner : Msg_LC_GetPartner,
	Msg_LC_GmCode : Msg_LC_GmCode,
	Msg_LC_GmPayResult : Msg_LC_GmPayResult,
	Msg_LC_InteractivePrize : Msg_LC_InteractivePrize,
	Msg_LC_InviteInfoAfterRoleEnter : Msg_LC_InviteInfoAfterRoleEnter,
	Msg_LC_ItemsGiveBack : Msg_LC_ItemsGiveBack,
	Msg_LC_ItemUseResult : Msg_LC_ItemUseResult,
	Msg_LC_LackOfSpace : Msg_LC_LackOfSpace,
	Msg_LC_LiftSkillResult : Msg_LC_LiftSkillResult,
	Msg_LC_LootChangeDefenseOrder : Msg_LC_LootChangeDefenseOrder,
	Msg_LC_LootChangeLootOrder : Msg_LC_LootChangeLootOrder,
	Msg_LC_LootCostVitality : Msg_LC_LootCostVitality,
	Msg_LC_LootMatchResult : Msg_LC_LootMatchResult,
	Msg_LC_LootOpResult : Msg_LC_LootOpResult,
	Msg_LC_MatchResult : Msg_LC_MatchResult,
	Msg_LC_MidasTouchResult : Msg_LC_MidasTouchResult,
	Msg_LC_MissionCompleted : Msg_LC_MissionCompleted,
	Msg_LC_MorrowRewardActive : Msg_LC_MorrowRewardActive,
	Msg_LC_MountEquipmentResult : Msg_LC_MountEquipmentResult,
	Msg_LC_MountFashionResult : Msg_LC_MountFashionResult,
	Msg_LC_MountSkillResult : Msg_LC_MountSkillResult,
	Msg_LC_MpveAwardResult : Msg_LC_MpveAwardResult,
	Msg_LC_MpveGeneralResult : Msg_LC_MpveGeneralResult,
	Msg_LC_NoticeFashionOverdue : Msg_LC_NoticeFashionOverdue,
	Msg_LC_NoticeFashionOverdueSoon : Msg_LC_NoticeFashionOverdueSoon,
	Msg_LC_NoticePlayerOffline : Msg_LC_NoticePlayerOffline,
	Msg_LC_NoticeQuitGroup : Msg_LC_NoticeQuitGroup,
	Msg_LC_NotifyMorrowReward : Msg_LC_NotifyMorrowReward,
	Msg_LC_NotifyNewMail : Msg_LC_NotifyNewMail,
	Msg_LC_OverLootResult : Msg_LC_OverLootResult,
	Msg_LC_PartnerCombatInfo : Msg_LC_PartnerCombatInfo,
	Msg_LC_PartnerEquipResult : Msg_LC_PartnerEquipResult,
	Msg_LC_QueryCorpsByName : Msg_LC_QueryCorpsByName,
	Msg_LC_QueryFriendInfoResult : Msg_LC_QueryFriendInfoResult,
	Msg_LC_QueryTDMatchGroupResult : Msg_LC_QueryTDMatchGroupResult,
	Msg_LC_QueueingCountResult : Msg_LC_QueueingCountResult,
	Msg_LC_RefreshExchangeResult : Msg_LC_RefreshExchangeResult,
	Msg_LC_RefreshPartnerCombatResult : Msg_LC_RefreshPartnerCombatResult,
	Msg_LC_RequestDare : Msg_LC_RequestDare,
	Msg_LC_RequestDareResult : Msg_LC_RequestDareResult,
	Msg_LC_RequestEnhanceEquipmentStar : Msg_LC_RequestEnhanceEquipmentStar,
	Msg_LC_RequestExpeditionResult : Msg_LC_RequestExpeditionResult,
	Msg_LC_RequestGowPrizeResult : Msg_LC_RequestGowPrizeResult,
	Msg_LC_RequestInviteResult : Msg_LC_RequestInviteResult,
	Msg_LC_RequestInviteRewardResult : Msg_LC_RequestInviteRewardResult,
	Msg_LC_RequestItemUseResult : Msg_LC_RequestItemUseResult,
	Msg_LC_RequestJoinGroupResult : Msg_LC_RequestJoinGroupResult,
	Msg_LC_RequestUserPositionResult : Msg_LC_RequestUserPositionResult,
	Msg_LC_RequestUsersResult : Msg_LC_RequestUsersResult,
	Msg_LC_ResetConsumeGoodsCount : Msg_LC_ResetConsumeGoodsCount,
	Msg_LC_ResetCorpsSignIn : Msg_LC_ResetCorpsSignIn,
	Msg_LC_ResetDailyMissions : Msg_LC_ResetDailyMissions,
	Msg_LC_ResetOnlineTimeRewardData : Msg_LC_ResetOnlineTimeRewardData,
	Msg_LC_ResetWeeklyLoginRewardData : Msg_LC_ResetWeeklyLoginRewardData,
	Msg_LC_RoleEnterResult : Msg_LC_RoleEnterResult,
	Msg_LC_RoleListResult : Msg_LC_RoleListResult,
	Msg_LC_SecretAreaTrialAward : Msg_LC_SecretAreaTrialAward,
	Msg_LC_SecretAreaTrialResult : Msg_LC_SecretAreaTrialResult,
	Msg_LC_SelectPartnerResult : Msg_LC_SelectPartnerResult,
	Msg_LC_SellItemResult : Msg_LC_SellItemResult,
	Msg_LC_SendScreenTip : Msg_LC_SendScreenTip,
	Msg_LC_ServerShutdown : Msg_LC_ServerShutdown,
	Msg_LC_SetFashionShowResult : Msg_LC_SetFashionShowResult,
	Msg_LC_SignInAndGetRewardResult : Msg_LC_SignInAndGetRewardResult,
	Msg_LC_StageClearResult : Msg_LC_StageClearResult,
	Msg_LC_StartGameResult : Msg_LC_StartGameResult,
	Msg_LC_StartLootResult : Msg_LC_StartLootResult,
	Msg_LC_StartPartnerBattleResult : Msg_LC_StartPartnerBattleResult,
	Msg_LC_StartTDChallengeResult : Msg_LC_StartTDChallengeResult,
	Msg_LC_SwapSkillResult : Msg_LC_SwapSkillResult,
	Msg_LC_SweepStageResult : Msg_LC_SweepStageResult,
	Msg_LC_SyncCombatData : Msg_LC_SyncCombatData,
	Msg_LC_SyncCorpsDareCount : Msg_LC_SyncCorpsDareCount,
	Msg_LC_SyncCorpsInfo : Msg_LC_SyncCorpsInfo,
	Msg_LC_SyncCorpsOpResult : Msg_LC_SyncCorpsOpResult,
	Msg_LC_SyncCorpsStar : Msg_LC_SyncCorpsStar,
	Msg_LC_SyncFightingScore : Msg_LC_SyncFightingScore,
	Msg_LC_SyncFriendList : Msg_LC_SyncFriendList,
	Msg_LC_SyncGoldTollgateInfo : Msg_LC_SyncGoldTollgateInfo,
	Msg_LC_SyncGowBattleResult : Msg_LC_SyncGowBattleResult,
	Msg_LC_SyncGowOtherInfo : Msg_LC_SyncGowOtherInfo,
	Msg_LC_SyncGowRankInfo : Msg_LC_SyncGowRankInfo,
	Msg_LC_SyncGowStarList : Msg_LC_SyncGowStarList,
	Msg_LC_SyncGroupUsers : Msg_LC_SyncGroupUsers,
	Msg_LC_SyncGuideFlag : Msg_LC_SyncGuideFlag,
	Msg_LC_SyncHomeNotice : Msg_LC_SyncHomeNotice,
	Msg_LC_SyncLeaveGroup : Msg_LC_SyncLeaveGroup,
	Msg_LC_SyncLootAward : Msg_LC_SyncLootAward,
	Msg_LC_SyncLootHistory : Msg_LC_SyncLootHistory,
	Msg_LC_SyncLootInfo : Msg_LC_SyncLootInfo,
	Msg_LC_SyncLotteryInfo : Msg_LC_SyncLotteryInfo,
	Msg_LC_SyncMailList : Msg_LC_SyncMailList,
	Msg_LC_SyncMpveInfo : Msg_LC_SyncMpveInfo,
	Msg_LC_SyncNewbieActionFlag : Msg_LC_SyncNewbieActionFlag,
	Msg_LC_SyncNewbieFlag : Msg_LC_SyncNewbieFlag,
	Msg_LC_SyncNoticeContent : Msg_LC_SyncNoticeContent,
	Msg_LC_SyncPinviteTeam : Msg_LC_SyncPinviteTeam,
	Msg_LC_SyncPlayerDetail : Msg_LC_SyncPlayerDetail,
	Msg_LC_SyncPlayerInfo : Msg_LC_SyncPlayerInfo,
	Msg_LC_SyncQuitRoom : Msg_LC_SyncQuitRoom,
	Msg_LC_SyncRecentLoginState : Msg_LC_SyncRecentLoginState,
	Msg_LC_SyncResetGowPrize : Msg_LC_SyncResetGowPrize,
	Msg_LC_SyncSignInCount : Msg_LC_SyncSignInCount,
	Msg_LC_SyncSkillInfos : Msg_LC_SyncSkillInfos,
	Msg_LC_SyncStamina : Msg_LC_SyncStamina,
	Msg_LC_SyncValidCorpsList : Msg_LC_SyncValidCorpsList,
	Msg_LC_SyncVigor : Msg_LC_SyncVigor,
	Msg_LC_SyncVitality : Msg_LC_SyncVitality,
	Msg_LC_SystemChatWorldResult : Msg_LC_SystemChatWorldResult,
	Msg_LC_TDChallengeResult : Msg_LC_TDChallengeResult,
	Msg_LC_UnlockLegacyResult : Msg_LC_UnlockLegacyResult,
	Msg_LC_UnlockSkillResult : Msg_LC_UnlockSkillResult,
	Msg_LC_UnmountEquipmentResult : Msg_LC_UnmountEquipmentResult,
	Msg_LC_UnmountFashionResult : Msg_LC_UnmountFashionResult,
	Msg_LC_UnmountSkillResult : Msg_LC_UnmountSkillResult,
	Msg_LC_UpdateInviteInfo : Msg_LC_UpdateInviteInfo,
	Msg_LC_UpgradeEquipBatch : Msg_LC_UpgradeEquipBatch,
	Msg_LC_UpgradeItemResult : Msg_LC_UpgradeItemResult,
	Msg_LC_UpgradeLegacyResult : Msg_LC_UpgradeLegacyResult,
	Msg_LC_UpgradeParnerStageResult : Msg_LC_UpgradeParnerStageResult,
	Msg_LC_UpgradePartnerLevelResult : Msg_LC_UpgradePartnerLevelResult,
	Msg_LC_UpgradeSkillResult : Msg_LC_UpgradeSkillResult,
	Msg_LC_UserLevelup : Msg_LC_UserLevelup,
	Msg_LC_WeeklyLoginRewardResult : Msg_LC_WeeklyLoginRewardResult,
	Msg_LC_XSoulChangeShowModelResult : Msg_LC_XSoulChangeShowModelResult,
	NodeMessageWithAccount : NodeMessageWithAccount,
	NodeMessageWithAccountAndGuid : NodeMessageWithAccountAndGuid,
	NodeMessageWithAccountAndLogicServerId : NodeMessageWithAccountAndLogicServerId,
	NodeMessageWithGuid : NodeMessageWithGuid,
	NodeMessageWithLogicServerId : NodeMessageWithLogicServerId,
	NodeRegister : NodeRegister,
	NodeRegisterResult : NodeRegisterResult,
	PartnerDataMsg : PartnerDataMsg,
	PaymentRebateDataMsg : PaymentRebateDataMsg,
	SelectedPartnerDataMsg : SelectedPartnerDataMsg,
	SkillDataInfo : SkillDataInfo,
	TDMatchInfoMsg : TDMatchInfoMsg,
	TooManyOperations : TooManyOperations,
	UserHeartbeat : UserHeartbeat,
	VersionVerify : VersionVerify,
	VersionVerifyResult : VersionVerifyResult,
	XSoulDataMsg : XSoulDataMsg,
};
