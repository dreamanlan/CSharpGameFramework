//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkLobbyMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using LitJson;
using GameFramework;

namespace GameFrameworkMessage
{

	public class AccountLogout : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class ArenaInfoMsg : IJsonMessage
	{
		public int Rank;
		public ulong Guid;
		public int HeroId;
		public string NickName;
		public int Level;
		public int FightScore;
		public List<PartnerDataMsg> ActivePartners = new List<PartnerDataMsg>();
		public List<JsonItemDataMsg> EquipInfo = new List<JsonItemDataMsg>();
		public List<SkillDataInfo> ActiveSkills = new List<SkillDataInfo>();
		public List<PartnerDataMsg> FightParters = new List<PartnerDataMsg>();
		public List<LegacyDataMsg> LegacyAttr = new List<LegacyDataMsg>();
		public List<XSoulDataMsg> XSouls = new List<XSoulDataMsg>();
		public List<JsonTalentDataMsg> Talents = new List<JsonTalentDataMsg>();
		public List<FashionSynMsg> Fashions = new List<FashionSynMsg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Rank);
			data.Get(1, ref Guid);
			data.Get(2, ref HeroId);
			data.Get(3, ref NickName);
			data.Get(4, ref Level);
			data.Get(5, ref FightScore);
			JsonMessageUtility.GetSubDataArray(data, 6, ref ActivePartners);
			JsonMessageUtility.GetSubDataArray(data, 7, ref EquipInfo);
			JsonMessageUtility.GetSubDataArray(data, 8, ref ActiveSkills);
			JsonMessageUtility.GetSubDataArray(data, 9, ref FightParters);
			JsonMessageUtility.GetSubDataArray(data, 10, ref LegacyAttr);
			JsonMessageUtility.GetSubDataArray(data, 11, ref XSouls);
			JsonMessageUtility.GetSubDataArray(data, 12, ref Talents);
			JsonMessageUtility.GetSubDataArray(data, 13, ref Fashions);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Rank);
			data.ArrayAdd(Guid);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(NickName);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightScore);
			JsonMessageUtility.AddSubDataArray(data, ActivePartners);
			JsonMessageUtility.AddSubDataArray(data, EquipInfo);
			JsonMessageUtility.AddSubDataArray(data, ActiveSkills);
			JsonMessageUtility.AddSubDataArray(data, FightParters);
			JsonMessageUtility.AddSubDataArray(data, LegacyAttr);
			JsonMessageUtility.AddSubDataArray(data, XSouls);
			JsonMessageUtility.AddSubDataArray(data, Talents);
			JsonMessageUtility.AddSubDataArray(data, Fashions);
			return data;
		}
	}

	public class AuctionInfo : IJsonMessage
	{
		public ulong auctionGuid;
		public ulong userGuid;
		public string userNickname;
		public int itemStatus;
		public long statusLeftTime;
		public int itemType;
		public int itemNum;
		public JsonItemDataMsg itemInfo;
		public int price;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref auctionGuid);
			data.Get(1, ref userGuid);
			data.Get(2, ref userNickname);
			data.Get(3, ref itemStatus);
			data.Get(4, ref statusLeftTime);
			data.Get(5, ref itemType);
			data.Get(6, ref itemNum);
			JsonMessageUtility.GetSubData(data, 7, out itemInfo);
			data.Get(8, ref price);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(auctionGuid);
			data.ArrayAdd(userGuid);
			data.ArrayAdd(userNickname);
			data.ArrayAdd(itemStatus);
			data.ArrayAdd(statusLeftTime);
			data.ArrayAdd(itemType);
			data.ArrayAdd(itemNum);
			JsonMessageUtility.AddSubData(data, itemInfo);
			data.ArrayAdd(price);
			return data;
		}
	}

	public class ChallengeEntityData : IJsonMessage
	{
		public ulong Guid;
		public int HeroId;
		public int Level;
		public int Rank;
		public int FightScore;
		public string NickName;
		public int UserDamage;
		public List<DamageInfoData> PartnerDamage = new List<DamageInfoData>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref HeroId);
			data.Get(2, ref Level);
			data.Get(3, ref Rank);
			data.Get(4, ref FightScore);
			data.Get(5, ref NickName);
			data.Get(6, ref UserDamage);
			JsonMessageUtility.GetSubDataArray(data, 7, ref PartnerDamage);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(Level);
			data.ArrayAdd(Rank);
			data.ArrayAdd(FightScore);
			data.ArrayAdd(NickName);
			data.ArrayAdd(UserDamage);
			JsonMessageUtility.AddSubDataArray(data, PartnerDamage);
			return data;
		}
	}

	public class ChallengeInfoData : IJsonMessage
	{
		public ChallengeEntityData Challenger;
		public ChallengeEntityData Target;
		public bool IsChallengeSuccess;
		public long EndTime;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubData(data, 0, out Challenger);
			JsonMessageUtility.GetSubData(data, 1, out Target);
			data.Get(2, ref IsChallengeSuccess);
			data.Get(3, ref EndTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubData(data, Challenger);
			JsonMessageUtility.AddSubData(data, Target);
			data.ArrayAdd(IsChallengeSuccess);
			data.ArrayAdd(EndTime);
			return data;
		}
	}

	public class ChatShieldInfoForMsg : IJsonMessage
	{
		public ulong Guid;
		public string Nickname;
		public int HeroId;
		public int Level;
		public int FightingScore;
		public bool IsOnline;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref Nickname);
			data.Get(2, ref HeroId);
			data.Get(3, ref Level);
			data.Get(4, ref FightingScore);
			data.Get(5, ref IsOnline);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(Nickname);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightingScore);
			data.ArrayAdd(IsOnline);
			return data;
		}
	}

	public class DamageInfoData : IJsonMessage
	{
		public int OwnerId;
		public int Damage;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref OwnerId);
			data.Get(1, ref Damage);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(OwnerId);
			data.ArrayAdd(Damage);
			return data;
		}
	}

	public class DirectLogin : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class FashionHideMsg : IJsonMessage
	{
		public bool m_IsWingHide;
		public bool m_IsWeaponHide;
		public bool m_IsClothHide;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_IsWingHide);
			data.Get(1, ref m_IsWeaponHide);
			data.Get(2, ref m_IsClothHide);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_IsWingHide);
			data.ArrayAdd(m_IsWeaponHide);
			data.ArrayAdd(m_IsClothHide);
			return data;
		}
	}

	public class FashionMsg : IJsonMessage
	{
		public int m_FsnId;
		public bool m_UseForever;
		public int m_DeadlineSeconds;
		public bool m_DressOn;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_FsnId);
			data.Get(1, ref m_UseForever);
			data.Get(2, ref m_DeadlineSeconds);
			data.Get(3, ref m_DressOn);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_FsnId);
			data.ArrayAdd(m_UseForever);
			data.ArrayAdd(m_DeadlineSeconds);
			data.ArrayAdd(m_DressOn);
			return data;
		}
	}

	public class FashionSynMsg : IJsonMessage
	{
		public int FashionId;
		public bool IsHide;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref FashionId);
			data.Get(1, ref IsHide);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(FashionId);
			data.ArrayAdd(IsHide);
			return data;
		}
	}

	public class FightingScoreEntityMsg : IJsonMessage
	{
		public ulong Guid;
		public int HeroId;
		public string NickName;
		public int Level;
		public int FightingScore;
		public int Rank;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref HeroId);
			data.Get(2, ref NickName);
			data.Get(3, ref Level);
			data.Get(4, ref FightingScore);
			data.Get(5, ref Rank);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(NickName);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightingScore);
			data.ArrayAdd(Rank);
			return data;
		}
	}

	public class FriendInfoForMsg : IJsonMessage
	{
		public ulong Guid;
		public string Nickname;
		public int HeroId;
		public int Level;
		public int FightingScore;
		public bool IsOnline;
		public bool IsBlack;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref Nickname);
			data.Get(2, ref HeroId);
			data.Get(3, ref Level);
			data.Get(4, ref FightingScore);
			data.Get(5, ref IsOnline);
			data.Get(6, ref IsBlack);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(Nickname);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightingScore);
			data.ArrayAdd(IsOnline);
			data.ArrayAdd(IsBlack);
			return data;
		}
	}

	public class GetQueueingCount : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class GowDataMsg : IJsonMessage
	{
		public int GowElo;
		public int GowMatches;
		public int GowWinMatches;
		public int LeftMatchCount;
		public int RankId;
		public int Point;
		public int CriticalTotalMatches;
		public int CriticalAmassWinMatches;
		public int CriticalAmassLossMatches;
		public bool IsAcquirePrize;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref GowElo);
			data.Get(1, ref GowMatches);
			data.Get(2, ref GowWinMatches);
			data.Get(3, ref LeftMatchCount);
			data.Get(4, ref RankId);
			data.Get(5, ref Point);
			data.Get(6, ref CriticalTotalMatches);
			data.Get(7, ref CriticalAmassWinMatches);
			data.Get(8, ref CriticalAmassLossMatches);
			data.Get(9, ref IsAcquirePrize);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(GowElo);
			data.ArrayAdd(GowMatches);
			data.ArrayAdd(GowWinMatches);
			data.ArrayAdd(LeftMatchCount);
			data.ArrayAdd(RankId);
			data.ArrayAdd(Point);
			data.ArrayAdd(CriticalTotalMatches);
			data.ArrayAdd(CriticalAmassWinMatches);
			data.ArrayAdd(CriticalAmassLossMatches);
			data.ArrayAdd(IsAcquirePrize);
			return data;
		}
	}

	public class ItemInfo_UseItem : IJsonMessage
	{
		public int ItemID;
		public ulong ItemGuid;
		public int Num;
		public int RandomProperty;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemID);
			data.Get(1, ref ItemGuid);
			data.Get(2, ref Num);
			data.Get(3, ref RandomProperty);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemID);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(Num);
			data.ArrayAdd(RandomProperty);
			return data;
		}
	}

	public class ItemLeftMsg : IJsonMessage
	{
		public ulong ItemGuid;
		public int ItemNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
			data.Get(1, ref ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemNum);
			return data;
		}
	}

	public class JsonItemDataMsg : IJsonMessage
	{
		public ulong Guid;
		public int ItemId;
		public int Level;
		public int Experience;
		public int Num;
		public int AppendProperty;
		public int EnhanceStarLevel;
		public int StrengthLevel;
		public int StrengthFailCount;
		public bool IsCanTrade;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref ItemId);
			data.Get(2, ref Level);
			data.Get(3, ref Experience);
			data.Get(4, ref Num);
			data.Get(5, ref AppendProperty);
			data.Get(6, ref EnhanceStarLevel);
			data.Get(7, ref StrengthLevel);
			data.Get(8, ref StrengthFailCount);
			data.Get(9, ref IsCanTrade);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(ItemId);
			data.ArrayAdd(Level);
			data.ArrayAdd(Experience);
			data.ArrayAdd(Num);
			data.ArrayAdd(AppendProperty);
			data.ArrayAdd(EnhanceStarLevel);
			data.ArrayAdd(StrengthLevel);
			data.ArrayAdd(StrengthFailCount);
			data.ArrayAdd(IsCanTrade);
			return data;
		}
	}

	public class JsonTalentDataMsg : IJsonMessage
	{
		public int Slot;
		public ulong ItemGuid;
		public int ItemId;
		public int Level;
		public int Experience;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Slot);
			data.Get(1, ref ItemGuid);
			data.Get(2, ref ItemId);
			data.Get(3, ref Level);
			data.Get(4, ref Experience);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Slot);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemId);
			data.ArrayAdd(Level);
			data.ArrayAdd(Experience);
			return data;
		}
	}

	public class KickUser : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class LegacyDataMsg : IJsonMessage
	{
		public int ItemId;
		public int Level;
		public int AppendProperty;
		public bool IsUnlock;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemId);
			data.Get(1, ref Level);
			data.Get(2, ref AppendProperty);
			data.Get(3, ref IsUnlock);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemId);
			data.ArrayAdd(Level);
			data.ArrayAdd(AppendProperty);
			data.ArrayAdd(IsUnlock);
			return data;
		}
	}

	public class Logout : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class LootEntityData : IJsonMessage
	{
		public ulong Key;
		public ulong Guid;
		public int HeroId;
		public int Level;
		public int FightScore;
		public string NickName;
		public int UserDamage;
		public List<int> DefenseOrder = new List<int>();
		public List<int> LootOrder = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Key);
			data.Get(1, ref Guid);
			data.Get(2, ref HeroId);
			data.Get(3, ref Level);
			data.Get(4, ref FightScore);
			data.Get(5, ref NickName);
			data.Get(6, ref UserDamage);
			JsonMessageUtility.GetSimpleArray(data, 7, ref DefenseOrder);
			JsonMessageUtility.GetSimpleArray(data, 8, ref LootOrder);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Key);
			data.ArrayAdd(Guid);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightScore);
			data.ArrayAdd(NickName);
			data.ArrayAdd(UserDamage);
			JsonMessageUtility.AddSimpleArray(data, DefenseOrder);
			JsonMessageUtility.AddSimpleArray(data, LootOrder);
			return data;
		}
	}

	public class LootHistoryData : IJsonMessage
	{
		public int DomainType;
		public int Booty;
		public LootEntityData Looter;
		public bool IsLootSuccess;
		public string BeginTime;
		public string EndTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref DomainType);
			data.Get(1, ref Booty);
			JsonMessageUtility.GetSubData(data, 2, out Looter);
			data.Get(3, ref IsLootSuccess);
			data.Get(4, ref BeginTime);
			data.Get(5, ref EndTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(DomainType);
			data.ArrayAdd(Booty);
			JsonMessageUtility.AddSubData(data, Looter);
			data.ArrayAdd(IsLootSuccess);
			data.ArrayAdd(BeginTime);
			data.ArrayAdd(EndTime);
			return data;
		}
	}

	public class LootInfoMsg : IJsonMessage
	{
		public ulong Key;
		public ulong Guid;
		public int HeroId;
		public string NickName;
		public int Level;
		public int FightScore;
		public bool IsOpen;
		public bool IsGetAward;
		public int DomainType;
		public string SessionStartTime;
		public string SessionEndTime;
		public List<JsonItemDataMsg> EquipInfo = new List<JsonItemDataMsg>();
		public List<SkillDataInfo> ActiveSkills = new List<SkillDataInfo>();
		public List<PartnerDataMsg> FightParters = new List<PartnerDataMsg>();
		public List<LegacyDataMsg> LegacyAttr = new List<LegacyDataMsg>();
		public List<XSoulDataMsg> XSouls = new List<XSoulDataMsg>();
		public List<JsonTalentDataMsg> Talents = new List<JsonTalentDataMsg>();
		public List<int> FightOrder = new List<int>();
		public List<int> LootOrder = new List<int>();
		public List<FashionSynMsg> FashionInfo = new List<FashionSynMsg>();
		public int Income;
		public int Loss;
		public int LootType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Key);
			data.Get(1, ref Guid);
			data.Get(2, ref HeroId);
			data.Get(3, ref NickName);
			data.Get(4, ref Level);
			data.Get(5, ref FightScore);
			data.Get(6, ref IsOpen);
			data.Get(7, ref IsGetAward);
			data.Get(8, ref DomainType);
			data.Get(9, ref SessionStartTime);
			data.Get(10, ref SessionEndTime);
			JsonMessageUtility.GetSubDataArray(data, 11, ref EquipInfo);
			JsonMessageUtility.GetSubDataArray(data, 12, ref ActiveSkills);
			JsonMessageUtility.GetSubDataArray(data, 13, ref FightParters);
			JsonMessageUtility.GetSubDataArray(data, 14, ref LegacyAttr);
			JsonMessageUtility.GetSubDataArray(data, 15, ref XSouls);
			JsonMessageUtility.GetSubDataArray(data, 16, ref Talents);
			JsonMessageUtility.GetSimpleArray(data, 17, ref FightOrder);
			JsonMessageUtility.GetSimpleArray(data, 18, ref LootOrder);
			JsonMessageUtility.GetSubDataArray(data, 19, ref FashionInfo);
			data.Get(20, ref Income);
			data.Get(21, ref Loss);
			data.Get(22, ref LootType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Key);
			data.ArrayAdd(Guid);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(NickName);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightScore);
			data.ArrayAdd(IsOpen);
			data.ArrayAdd(IsGetAward);
			data.ArrayAdd(DomainType);
			data.ArrayAdd(SessionStartTime);
			data.ArrayAdd(SessionEndTime);
			JsonMessageUtility.AddSubDataArray(data, EquipInfo);
			JsonMessageUtility.AddSubDataArray(data, ActiveSkills);
			JsonMessageUtility.AddSubDataArray(data, FightParters);
			JsonMessageUtility.AddSubDataArray(data, LegacyAttr);
			JsonMessageUtility.AddSubDataArray(data, XSouls);
			JsonMessageUtility.AddSubDataArray(data, Talents);
			JsonMessageUtility.AddSimpleArray(data, FightOrder);
			JsonMessageUtility.AddSimpleArray(data, LootOrder);
			JsonMessageUtility.AddSubDataArray(data, FashionInfo);
			data.ArrayAdd(Income);
			data.ArrayAdd(Loss);
			data.ArrayAdd(LootType);
			return data;
		}
	}

	public class MailInfoForMessage : IJsonMessage
	{
		public bool m_AlreadyRead;
		public ulong m_MailGuid;
		public int m_Module;
		public string m_Title;
		public string m_Sender;
		public string m_SendTime;
		public string m_Text;
		public List<MailItemForMessage> m_Items = new List<MailItemForMessage>();
		public int m_Money;
		public int m_Gold;
		public int m_Stamina;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_AlreadyRead);
			data.Get(1, ref m_MailGuid);
			data.Get(2, ref m_Module);
			data.Get(3, ref m_Title);
			data.Get(4, ref m_Sender);
			data.Get(5, ref m_SendTime);
			data.Get(6, ref m_Text);
			JsonMessageUtility.GetSubDataArray(data, 7, ref m_Items);
			data.Get(8, ref m_Money);
			data.Get(9, ref m_Gold);
			data.Get(10, ref m_Stamina);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_AlreadyRead);
			data.ArrayAdd(m_MailGuid);
			data.ArrayAdd(m_Module);
			data.ArrayAdd(m_Title);
			data.ArrayAdd(m_Sender);
			data.ArrayAdd(m_SendTime);
			data.ArrayAdd(m_Text);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Stamina);
			return data;
		}
	}

	public class MailItemForMessage : IJsonMessage
	{
		public int m_ItemId;
		public int m_ItemNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemId);
			data.Get(1, ref m_ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemId);
			data.ArrayAdd(m_ItemNum);
			return data;
		}
	}

	public class MissionInfoForSync : IJsonMessage
	{
		public int m_MissionId;
		public bool m_IsCompleted;
		public string m_Progress;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MissionId);
			data.Get(1, ref m_IsCompleted);
			data.Get(2, ref m_Progress);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MissionId);
			data.ArrayAdd(m_IsCompleted);
			data.ArrayAdd(m_Progress);
			return data;
		}
	}

	public class MorrowRewardInfo : IJsonMessage
	{
		public int m_ActiveId;
		public bool m_IsActive;
		public int m_LeftSeconds;
		public bool m_CanGetReward;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ActiveId);
			data.Get(1, ref m_IsActive);
			data.Get(2, ref m_LeftSeconds);
			data.Get(3, ref m_CanGetReward);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ActiveId);
			data.ArrayAdd(m_IsActive);
			data.ArrayAdd(m_LeftSeconds);
			data.ArrayAdd(m_CanGetReward);
			return data;
		}
	}

	public class Msg_CL_AcceptedDare : IJsonMessage
	{
		public string m_ChallengerNickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ChallengerNickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ChallengerNickname);
			return data;
		}
	}

	public class Msg_CL_AccountLogin : IJsonMessage
	{
		public int m_OpCode;
		public int m_ChannelId;
		public string m_Data;
		public string m_ClientGameVersion;
		public string m_ClientLoginIp;
		public string m_GameChannelId;
		public string m_UniqueIdentifier;
		public string m_System;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_OpCode);
			data.Get(1, ref m_ChannelId);
			data.Get(2, ref m_Data);
			data.Get(3, ref m_ClientGameVersion);
			data.Get(4, ref m_ClientLoginIp);
			data.Get(5, ref m_GameChannelId);
			data.Get(6, ref m_UniqueIdentifier);
			data.Get(7, ref m_System);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_OpCode);
			data.ArrayAdd(m_ChannelId);
			data.ArrayAdd(m_Data);
			data.ArrayAdd(m_ClientGameVersion);
			data.ArrayAdd(m_ClientLoginIp);
			data.ArrayAdd(m_GameChannelId);
			data.ArrayAdd(m_UniqueIdentifier);
			data.ArrayAdd(m_System);
			return data;
		}
	}

	public class Msg_CL_ActivateAccount : IJsonMessage
	{
		public string m_ActivationCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ActivationCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ActivationCode);
			return data;
		}
	}

	public class Msg_CL_AddAssets : IJsonMessage
	{
		public int m_Money;
		public int m_Gold;
		public int m_Exp;
		public int m_Stamina;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Money);
			data.Get(1, ref m_Gold);
			data.Get(2, ref m_Exp);
			data.Get(3, ref m_Stamina);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_Stamina);
			return data;
		}
	}

	public class Msg_CL_AddFriend : IJsonMessage
	{
		public string m_TargetNick;
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetNick);
			data.Get(1, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetNick);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_AddItem : IJsonMessage
	{
		public int m_ItemId;
		public int m_ItemNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemId);
			data.Get(1, ref m_ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemId);
			data.ArrayAdd(m_ItemNum);
			return data;
		}
	}

	public class Msg_CL_AddTalentExperience : IJsonMessage
	{
		public int Slot;
		public List<ulong> ItemGuid = new List<ulong>();
		public int Result;
		public int Experience;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Slot);
			JsonMessageUtility.GetSimpleArray(data, 1, ref ItemGuid);
			data.Get(2, ref Result);
			data.Get(3, ref Experience);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Slot);
			JsonMessageUtility.AddSimpleArray(data, ItemGuid);
			data.ArrayAdd(Result);
			data.ArrayAdd(Experience);
			return data;
		}
	}

	public class Msg_CL_AddXSoulExperience : IJsonMessage
	{
		public int m_XSoulPart;
		public ulong m_UseItemId;
		public int m_ItemNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_XSoulPart);
			data.Get(1, ref m_UseItemId);
			data.Get(2, ref m_ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_XSoulPart);
			data.ArrayAdd(m_UseItemId);
			data.ArrayAdd(m_ItemNum);
			return data;
		}
	}

	public class Msg_CL_ArenaBeginFight : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ArenaBuyFightCount : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ArenaChallengeOver : IJsonMessage
	{
		public bool IsSuccess;
		public int ChallengerDamage;
		public int TargetDamage;
		public List<DamageInfoData> ChallengerPartnerDamage = new List<DamageInfoData>();
		public List<DamageInfoData> TargetPartnerDamage = new List<DamageInfoData>();
		public int Sign;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref IsSuccess);
			data.Get(1, ref ChallengerDamage);
			data.Get(2, ref TargetDamage);
			JsonMessageUtility.GetSubDataArray(data, 3, ref ChallengerPartnerDamage);
			JsonMessageUtility.GetSubDataArray(data, 4, ref TargetPartnerDamage);
			data.Get(5, ref Sign);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(IsSuccess);
			data.ArrayAdd(ChallengerDamage);
			data.ArrayAdd(TargetDamage);
			JsonMessageUtility.AddSubDataArray(data, ChallengerPartnerDamage);
			JsonMessageUtility.AddSubDataArray(data, TargetPartnerDamage);
			data.ArrayAdd(Sign);
			return data;
		}
	}

	public class Msg_CL_ArenaChangePartner : IJsonMessage
	{
		public List<int> Partners = new List<int>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref Partners);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, Partners);
			return data;
		}
	}

	public class Msg_CL_ArenaQueryHistory : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ArenaQueryRank : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ArenaStartChallenge : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_AuctionBuy : IJsonMessage
	{
		public ulong AuctionGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref AuctionGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(AuctionGuid);
			return data;
		}
	}

	public class Msg_CL_AuctionQuery : IJsonMessage
	{
		public int Category1;
		public int Category2;
		public bool AscPrice;
		public string ItemName;
		public int PageNo;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Category1);
			data.Get(1, ref Category2);
			data.Get(2, ref AscPrice);
			data.Get(3, ref ItemName);
			data.Get(4, ref PageNo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Category1);
			data.ArrayAdd(Category2);
			data.ArrayAdd(AscPrice);
			data.ArrayAdd(ItemName);
			data.ArrayAdd(PageNo);
			return data;
		}
	}

	public class Msg_CL_AuctionReceive : IJsonMessage
	{
		public ulong AuctionGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref AuctionGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(AuctionGuid);
			return data;
		}
	}

	public class Msg_CL_AuctionSelfAuction : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_AuctionSell : IJsonMessage
	{
		public ulong ItemGuid;
		public int ItemNum;
		public int ItemType;
		public int Price;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
			data.Get(1, ref ItemNum);
			data.Get(2, ref ItemType);
			data.Get(3, ref Price);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemNum);
			data.ArrayAdd(ItemType);
			data.ArrayAdd(Price);
			return data;
		}
	}

	public class Msg_CL_AuctionUnshelve : IJsonMessage
	{
		public ulong AuctionGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref AuctionGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(AuctionGuid);
			return data;
		}
	}

	public class Msg_CL_BuyEliteCount : IJsonMessage
	{
		public int m_EliteId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_EliteId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_EliteId);
			return data;
		}
	}

	public class Msg_CL_BuyFashion : IJsonMessage
	{
		public int m_ItemID;
		public int m_TimeType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
			data.Get(1, ref m_TimeType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_TimeType);
			return data;
		}
	}

	public class Msg_CL_BuyLife : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_BuyPartnerCombatTicket : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_BuyStamina : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_BuyTDFightCount : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_CancelMatch : IJsonMessage
	{
		public int m_SceneType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneType);
			return data;
		}
	}

	public class Msg_CL_CancelSelectPartner : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_CL_ChangeCityRoom : IJsonMessage
	{
		public int m_SceneId;
		public int m_RoomId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_RoomId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_RoomId);
			return data;
		}
	}

	public class Msg_CL_ChangeFieldRoom : IJsonMessage
	{
		public int m_SceneId;
		public int m_RoomId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_RoomId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_RoomId);
			return data;
		}
	}

	public class Msg_CL_ChangeScene : IJsonMessage
	{
		public int m_SceneId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			return data;
		}
	}

	public class Msg_CL_ChatAddShield : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_ChatAddShieldByName : IJsonMessage
	{
		public string m_TargetNickName;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetNickName);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetNickName);
			return data;
		}
	}

	public class Msg_CL_ChatDelShield : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_CombinTalentCard : IJsonMessage
	{
		public List<ulong> m_PartGuid = new List<ulong>();
		public List<int> m_ItemNum = new List<int>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_PartGuid);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_PartGuid);
			JsonMessageUtility.AddSimpleArray(data, m_ItemNum);
			return data;
		}
	}

	public class Msg_CL_CombinTalentCardResult : IJsonMessage
	{
		public List<ulong> m_PartGuid = new List<ulong>();
		public List<int> m_ItemNum = new List<int>();
		public int m_Result;
		public int m_gainItemId;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_PartGuid);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_ItemNum);
			data.Get(2, ref m_Result);
			data.Get(3, ref m_gainItemId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_PartGuid);
			JsonMessageUtility.AddSimpleArray(data, m_ItemNum);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_gainItemId);
			return data;
		}
	}

	public class Msg_CL_CompoundEquip : IJsonMessage
	{
		public int m_PartId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartId);
			return data;
		}
	}

	public class Msg_CL_CompoundPartner : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_CL_ConfirmFriend : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_ConfirmJoinGroup : IJsonMessage
	{
		public ulong m_InviteeGuid;
		public string m_GroupNick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_InviteeGuid);
			data.Get(1, ref m_GroupNick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_InviteeGuid);
			data.ArrayAdd(m_GroupNick);
			return data;
		}
	}

	public class Msg_CL_CorpsAgreeClaimer : IJsonMessage
	{
		public ulong m_ClaimerGuid;
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ClaimerGuid);
			data.Get(1, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ClaimerGuid);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsAppoint : IJsonMessage
	{
		public ulong m_TargetGuid;
		public int m_Title;
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_Title);
			data.Get(2, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_Title);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsClearRequest : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_CorpsCreate : IJsonMessage
	{
		public string m_CorpsName;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CorpsName);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CorpsName);
			return data;
		}
	}

	public class Msg_CL_CorpsDissolve : IJsonMessage
	{
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsIndirectJoin : IJsonMessage
	{
		public ulong m_UserGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_UserGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_UserGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsJoin : IJsonMessage
	{
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsKickout : IJsonMessage
	{
		public ulong m_TargetGuid;
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsQuit : IJsonMessage
	{
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsRefuseClaimer : IJsonMessage
	{
		public ulong m_ClaimerGuid;
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ClaimerGuid);
			data.Get(1, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ClaimerGuid);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_CorpsSignIn : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_CostVitality : IJsonMessage
	{
		public ulong m_Key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Key);
			return data;
		}
	}

	public class Msg_CL_CreateNickname : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_CreateRole : IJsonMessage
	{
		public int m_HeroId;
		public string m_Nickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_HeroId);
			data.Get(1, ref m_Nickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_HeroId);
			data.ArrayAdd(m_Nickname);
			return data;
		}
	}

	public class Msg_CL_DeleteFriend : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_DiamondExtraBuyBox : IJsonMessage
	{
		public int m_BoxPlace;
		public int m_SceneId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_BoxPlace);
			data.Get(1, ref m_SceneId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_BoxPlace);
			data.ArrayAdd(m_SceneId);
			return data;
		}
	}

	public class Msg_CL_DiscardItem : IJsonMessage
	{
		public List<ulong> m_ItemGuid = new List<ulong>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_ItemGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_ItemGuid);
			return data;
		}
	}

	public class Msg_CL_DrawReward : IJsonMessage
	{
		public int m_RewardType;
		public int m_LotteryType;
		public long m_Time;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RewardType);
			data.Get(1, ref m_LotteryType);
			data.Get(2, ref m_Time);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RewardType);
			data.ArrayAdd(m_LotteryType);
			data.ArrayAdd(m_Time);
			return data;
		}
	}

	public class Msg_CL_EndPartnerBattle : IJsonMessage
	{
		public int m_BattleResult;
		public int m_MatchKey;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_BattleResult);
			data.Get(1, ref m_MatchKey);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_BattleResult);
			data.ArrayAdd(m_MatchKey);
			return data;
		}
	}

	public class Msg_CL_EnterField : IJsonMessage
	{
		public int m_SceneId;
		public int m_RoomId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_RoomId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_RoomId);
			return data;
		}
	}

	public class Msg_CL_EquipmentStrength : IJsonMessage
	{
		public int m_ItemID;
		public bool m_IsProtected;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
			data.Get(1, ref m_IsProtected);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_IsProtected);
			return data;
		}
	}

	public class Msg_CL_EquipTalentCard : IJsonMessage
	{
		public ulong ItemGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			return data;
		}
	}

	public class Msg_CL_ExchangeGift : IJsonMessage
	{
		public string m_GiftCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_GiftCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_GiftCode);
			return data;
		}
	}

	public class Msg_CL_ExchangeGoods : IJsonMessage
	{
		public int m_ExchangeId;
		public int m_NpcId;
		public bool m_RequestRefresh;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ExchangeId);
			data.Get(1, ref m_NpcId);
			data.Get(2, ref m_RequestRefresh);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ExchangeId);
			data.ArrayAdd(m_NpcId);
			data.ArrayAdd(m_RequestRefresh);
			return data;
		}
	}

	public class Msg_CL_ExpeditionAward : IJsonMessage
	{
		public int m_TollgateNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TollgateNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TollgateNum);
			return data;
		}
	}

	public class Msg_CL_ExpeditionFailure : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ExpeditionReset : IJsonMessage
	{
		public int m_Hp;
		public int m_Mp;
		public int m_Rage;
		public int m_RequestNum;
		public bool m_IsReset;
		public bool m_AllowCostGold;
		public long m_Timestamp;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Hp);
			data.Get(1, ref m_Mp);
			data.Get(2, ref m_Rage);
			data.Get(3, ref m_RequestNum);
			data.Get(4, ref m_IsReset);
			data.Get(5, ref m_AllowCostGold);
			data.Get(6, ref m_Timestamp);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Rage);
			data.ArrayAdd(m_RequestNum);
			data.ArrayAdd(m_IsReset);
			data.ArrayAdd(m_AllowCostGold);
			data.ArrayAdd(m_Timestamp);
			return data;
		}
	}

	public class Msg_CL_ExpeditionSweep : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_FinishExpedition : IJsonMessage
	{
		public int m_SceneId;
		public int m_TollgateNum;
		public int m_Hp;
		public int m_Mp;
		public int m_Rage;
		public int m_PartnerId;
		public int m_PartnerHpPer;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_TollgateNum);
			data.Get(2, ref m_Hp);
			data.Get(3, ref m_Mp);
			data.Get(4, ref m_Rage);
			data.Get(5, ref m_PartnerId);
			data.Get(6, ref m_PartnerHpPer);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_TollgateNum);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Rage);
			data.ArrayAdd(m_PartnerId);
			data.ArrayAdd(m_PartnerHpPer);
			return data;
		}
	}

	public class Msg_CL_FinishMission : IJsonMessage
	{
		public int m_MissionId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MissionId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MissionId);
			return data;
		}
	}

	public class Msg_CL_FriendList : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GainFirstPayReward : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GainVipReward : IJsonMessage
	{
		public int m_VipLevel;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_VipLevel);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_VipLevel);
			return data;
		}
	}

	public class Msg_CL_GetGowStarList : IJsonMessage
	{
		public int m_Start;
		public int m_Count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Start);
			data.Get(1, ref m_Count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Start);
			data.ArrayAdd(m_Count);
			return data;
		}
	}

	public class Msg_CL_GetLoginLottery : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GetLootAward : IJsonMessage
	{
		public ulong m_Key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Key);
			return data;
		}
	}

	public class Msg_CL_GetLootHistory : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GetMailList : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GetMorrowReward : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GetOnlineTimeReward : IJsonMessage
	{
		public int m_Index;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Index);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Index);
			return data;
		}
	}

	public class Msg_CL_GetQueueingCount : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_GmPay : IJsonMessage
	{
		public int m_Diamonds;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Diamonds);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Diamonds);
			return data;
		}
	}

	public class Msg_CL_GMResetDailyMissions : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_InteractivePrize : IJsonMessage
	{
		public int m_ActorId;
		public int m_LinkId;
		public int m_StoryId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ActorId);
			data.Get(1, ref m_LinkId);
			data.Get(2, ref m_StoryId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ActorId);
			data.ArrayAdd(m_LinkId);
			data.ArrayAdd(m_StoryId);
			return data;
		}
	}

	public class Msg_CL_ItemUseRequest : IJsonMessage
	{
		public ulong m_ItemGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemGuid);
			return data;
		}
	}

	public class Msg_CL_LiftSkill : IJsonMessage
	{
		public int m_SkillId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SkillId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SkillId);
			return data;
		}
	}

	public class Msg_CL_LootMatchTarget : IJsonMessage
	{
		public ulong m_Key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Key);
			return data;
		}
	}

	public class Msg_CL_MidasTouch : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_MountEquipment : IJsonMessage
	{
		public ulong m_EquipGuid;
		public int m_EquipPos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_EquipGuid);
			data.Get(1, ref m_EquipPos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_EquipGuid);
			data.ArrayAdd(m_EquipPos);
			return data;
		}
	}

	public class Msg_CL_MountFashion : IJsonMessage
	{
		public int m_FashionID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_FashionID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_FashionID);
			return data;
		}
	}

	public class Msg_CL_MountSkill : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public int m_SlotPos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_SlotPos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_SlotPos);
			return data;
		}
	}

	public class Msg_CL_OpenCharpter : IJsonMessage
	{
		public int CharpterId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref CharpterId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(CharpterId);
			return data;
		}
	}

	public class Msg_CL_OpenDomain : IJsonMessage
	{
		public int DomainType;
		public ulong Key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref DomainType);
			data.Get(1, ref Key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(DomainType);
			data.ArrayAdd(Key);
			return data;
		}
	}

	public class Msg_CL_OverLoot : IJsonMessage
	{
		public bool IsSuccess;
		public int LooterDamage;
		public int DefenderDamage;
		public int Sign;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref IsSuccess);
			data.Get(1, ref LooterDamage);
			data.Get(2, ref DefenderDamage);
			data.Get(3, ref Sign);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(IsSuccess);
			data.ArrayAdd(LooterDamage);
			data.ArrayAdd(DefenderDamage);
			data.ArrayAdd(Sign);
			return data;
		}
	}

	public class Msg_CL_PartnerEquip : IJsonMessage
	{
		public int m_PartnerId;
		public ulong m_ItemGuid;
		public int m_EquipIndex;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
			data.Get(1, ref m_ItemGuid);
			data.Get(2, ref m_EquipIndex);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			data.ArrayAdd(m_ItemGuid);
			data.ArrayAdd(m_EquipIndex);
			return data;
		}
	}

	public class Msg_CL_PinviteTeam : IJsonMessage
	{
		public string m_FirstNick;
		public string m_SecondNick;
		public ulong m_FirstGuid;
		public ulong m_SecondGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_FirstNick);
			data.Get(1, ref m_SecondNick);
			data.Get(2, ref m_FirstGuid);
			data.Get(3, ref m_SecondGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_FirstNick);
			data.ArrayAdd(m_SecondNick);
			data.ArrayAdd(m_FirstGuid);
			data.ArrayAdd(m_SecondGuid);
			return data;
		}
	}

	public class Msg_CL_PushDelayData : IJsonMessage
	{
		public long m_RoundtripTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RoundtripTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RoundtripTime);
			return data;
		}
	}

	public class Msg_CL_QueryArenaInfo : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_QueryArenaMatchGroup : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_QueryCorpsByName : IJsonMessage
	{
		public string DimName;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref DimName);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(DimName);
			return data;
		}
	}

	public class Msg_CL_QueryCorpsInfo : IJsonMessage
	{
		public ulong m_CorpsGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CorpsGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CorpsGuid);
			return data;
		}
	}

	public class Msg_CL_QueryCorpsStar : IJsonMessage
	{
		public int m_Start;
		public int m_Count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Start);
			data.Get(1, ref m_Count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Start);
			data.ArrayAdd(m_Count);
			return data;
		}
	}

	public class Msg_CL_QueryExpeditionInfo : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_QueryFriendInfo : IJsonMessage
	{
		public int m_QueryType;
		public string m_TargetName;
		public int m_TargetLevel;
		public int m_TargetScore;
		public int m_TargetFortune;
		public int m_TargetGender;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_QueryType);
			data.Get(1, ref m_TargetName);
			data.Get(2, ref m_TargetLevel);
			data.Get(3, ref m_TargetScore);
			data.Get(4, ref m_TargetFortune);
			data.Get(5, ref m_TargetGender);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_QueryType);
			data.ArrayAdd(m_TargetName);
			data.ArrayAdd(m_TargetLevel);
			data.ArrayAdd(m_TargetScore);
			data.ArrayAdd(m_TargetFortune);
			data.ArrayAdd(m_TargetGender);
			return data;
		}
	}

	public class Msg_CL_QueryLootInfo : IJsonMessage
	{
		public ulong m_Key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Key);
			return data;
		}
	}

	public class Msg_CL_QuerySkillInfos : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_QueryTDMatchGroup : IJsonMessage
	{
		public bool IsNext;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref IsNext);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(IsNext);
			return data;
		}
	}

	public class Msg_CL_QueryValidCorpsList : IJsonMessage
	{
		public int m_Start;
		public int m_Count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Start);
			data.Get(1, ref m_Count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Start);
			data.ArrayAdd(m_Count);
			return data;
		}
	}

	public class Msg_CL_QuitGroup : IJsonMessage
	{
		public string m_DropoutNick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_DropoutNick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_DropoutNick);
			return data;
		}
	}

	public class Msg_CL_QuitPve : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_QuitRoom : IJsonMessage
	{
		public bool m_IsQuitRoom;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_IsQuitRoom);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_IsQuitRoom);
			return data;
		}
	}

	public class Msg_CL_ReadMail : IJsonMessage
	{
		public ulong m_MailGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MailGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MailGuid);
			return data;
		}
	}

	public class Msg_CL_ReceiveMail : IJsonMessage
	{
		public ulong m_MailGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MailGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MailGuid);
			return data;
		}
	}

	public class Msg_CL_RecordNewbieFlag : IJsonMessage
	{
		public int m_Bit;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Bit);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Bit);
			return data;
		}
	}

	public class Msg_CL_RefreshPartnerCombat : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RefusedDare : IJsonMessage
	{
		public string m_ChallengerNickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ChallengerNickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ChallengerNickname);
			return data;
		}
	}

	public class Msg_CL_RefuseGroupRequest : IJsonMessage
	{
		public ulong m_RequesterGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RequesterGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RequesterGuid);
			return data;
		}
	}

	public class Msg_CL_RequestDare : IJsonMessage
	{
		public string m_TargetNickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetNickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetNickname);
			return data;
		}
	}

	public class Msg_CL_RequestDareByGuid : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_RequestEnhanceEquipmentStar : IJsonMessage
	{
		public int m_ItemID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			return data;
		}
	}

	public class Msg_CL_RequestExpedition : IJsonMessage
	{
		public int m_SceneId;
		public int m_TollgateNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_TollgateNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_TollgateNum);
			return data;
		}
	}

	public class Msg_CL_RequestGowBattleResult : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RequestGowPrize : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RequestGroupInfo : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RequestInvite : IJsonMessage
	{
		public string m_InviteCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_InviteCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_InviteCode);
			return data;
		}
	}

	public class Msg_CL_RequestInviteReward : IJsonMessage
	{
		public int m_RewardId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RewardId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RewardId);
			return data;
		}
	}

	public class Msg_CL_RequestJoinGroup : IJsonMessage
	{
		public ulong m_InviteeGuid;
		public string m_GroupNick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_InviteeGuid);
			data.Get(1, ref m_GroupNick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_InviteeGuid);
			data.ArrayAdd(m_GroupNick);
			return data;
		}
	}

	public class Msg_CL_RequestMatch : IJsonMessage
	{
		public int m_SceneType;
		public int m_SceneDifficulty;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneType);
			data.Get(1, ref m_SceneDifficulty);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneType);
			data.ArrayAdd(m_SceneDifficulty);
			return data;
		}
	}

	public class Msg_CL_RequestMpveAward : IJsonMessage
	{
		public int m_Type;
		public int m_Index;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Type);
			data.Get(1, ref m_Index);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Type);
			data.ArrayAdd(m_Index);
			return data;
		}
	}

	public class Msg_CL_RequestPlayerDetail : IJsonMessage
	{
		public string m_Nick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Nick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Nick);
			return data;
		}
	}

	public class Msg_CL_RequestPlayerInfo : IJsonMessage
	{
		public string m_Nick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Nick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Nick);
			return data;
		}
	}

	public class Msg_CL_RequestRefreshExchange : IJsonMessage
	{
		public bool m_RequestRefresh;
		public int m_CurrencyId;
		public int m_NpcId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RequestRefresh);
			data.Get(1, ref m_CurrencyId);
			data.Get(2, ref m_NpcId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RequestRefresh);
			data.ArrayAdd(m_CurrencyId);
			data.ArrayAdd(m_NpcId);
			return data;
		}
	}

	public class Msg_CL_RequestSkillInfos : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RequestUserPosition : IJsonMessage
	{
		public ulong m_User;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_User);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_User);
			return data;
		}
	}

	public class Msg_CL_RequestUsers : IJsonMessage
	{
		public int m_Count;
		public List<ulong> m_AlreadyExists = new List<ulong>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Count);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_AlreadyExists);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Count);
			JsonMessageUtility.AddSimpleArray(data, m_AlreadyExists);
			return data;
		}
	}

	public class Msg_CL_RequestVigor : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_RequireChatEquipInfo : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_RequireChatRoleInfo : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_CL_RequireChatShieldList : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_ResetCharpter : IJsonMessage
	{
		public int CharpterId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref CharpterId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(CharpterId);
			return data;
		}
	}

	public class Msg_CL_RoleEnter : IJsonMessage
	{
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class Msg_CL_RoleList : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_SaveSkillPreset : IJsonMessage
	{
		public int m_SelectedPresetIndex;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SelectedPresetIndex);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SelectedPresetIndex);
			return data;
		}
	}

	public class Msg_CL_SecretAreaFightingInfo : IJsonMessage
	{
		public int m_Difficulty;
		public int m_Segment;
		public int m_Hp;
		public int m_Mp;
		public bool m_Finish;
		public int m_PartnerHp;
		public int m_CheckNumber;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Difficulty);
			data.Get(1, ref m_Segment);
			data.Get(2, ref m_Hp);
			data.Get(3, ref m_Mp);
			data.Get(4, ref m_Finish);
			data.Get(5, ref m_PartnerHp);
			data.Get(6, ref m_CheckNumber);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Difficulty);
			data.ArrayAdd(m_Segment);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Finish);
			data.ArrayAdd(m_PartnerHp);
			data.ArrayAdd(m_CheckNumber);
			return data;
		}
	}

	public class Msg_CL_SecretAreaTrial : IJsonMessage
	{
		public int m_Difficulty;
		public bool m_Sweept;
		public bool m_Refresh;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Difficulty);
			data.Get(1, ref m_Sweept);
			data.Get(2, ref m_Refresh);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Difficulty);
			data.ArrayAdd(m_Sweept);
			data.ArrayAdd(m_Refresh);
			return data;
		}
	}

	public class Msg_CL_SelectPartner : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_CL_SellItem : IJsonMessage
	{
		public ulong ItemGuid;
		public int ItemNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
			data.Get(1, ref ItemNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemNum);
			return data;
		}
	}

	public class Msg_CL_SendChat : IJsonMessage
	{
		public int m_Type;
		public string m_TargetNickName;
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Type);
			data.Get(1, ref m_TargetNickName);
			data.Get(2, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Type);
			data.ArrayAdd(m_TargetNickName);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_CL_SetCorpsNotice : IJsonMessage
	{
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_CL_SetFashionShow : IJsonMessage
	{
		public int m_FashionPartType;
		public bool m_IsHide;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_FashionPartType);
			data.Get(1, ref m_IsHide);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_FashionPartType);
			data.ArrayAdd(m_IsHide);
			return data;
		}
	}

	public class Msg_CL_SetNewbieActionFlag : IJsonMessage
	{
		public int m_Bit;
		public int m_Num;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Bit);
			data.Get(1, ref m_Num);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Bit);
			data.ArrayAdd(m_Num);
			return data;
		}
	}

	public class Msg_CL_SetNewbieFlag : IJsonMessage
	{
		public int m_Bit;
		public int m_Num;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Bit);
			data.Get(1, ref m_Num);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Bit);
			data.ArrayAdd(m_Num);
			return data;
		}
	}

	public class Msg_CL_SignInAndGetReward : IJsonMessage
	{
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class Msg_CL_SinglePVE : IJsonMessage
	{
		public int m_SceneType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneType);
			return data;
		}
	}

	public class Msg_CL_StageClear : IJsonMessage
	{
		public int m_HitCount;
		public int m_KillNpcCount;
		public int m_MaxMultHitCount;
		public int m_Hp;
		public int m_Mp;
		public int m_Gold;
		public int m_MatchKey;
		public bool m_IsFitTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_HitCount);
			data.Get(1, ref m_KillNpcCount);
			data.Get(2, ref m_MaxMultHitCount);
			data.Get(3, ref m_Hp);
			data.Get(4, ref m_Mp);
			data.Get(5, ref m_Gold);
			data.Get(6, ref m_MatchKey);
			data.Get(7, ref m_IsFitTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_HitCount);
			data.ArrayAdd(m_KillNpcCount);
			data.ArrayAdd(m_MaxMultHitCount);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_MatchKey);
			data.ArrayAdd(m_IsFitTime);
			return data;
		}
	}

	public class Msg_CL_StartCorpsTollgate : IJsonMessage
	{
		public int CarpterId;
		public int SceneId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref CarpterId);
			data.Get(1, ref SceneId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(CarpterId);
			data.ArrayAdd(SceneId);
			return data;
		}
	}

	public class Msg_CL_StartGame : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_StartLoot : IJsonMessage
	{
		public ulong TargetKey;
		public ulong SelfKey;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref TargetKey);
			data.Get(1, ref SelfKey);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(TargetKey);
			data.ArrayAdd(SelfKey);
			return data;
		}
	}

	public class Msg_CL_StartMpve : IJsonMessage
	{
		public int m_SceneType;
		public int m_SceneDifficulty;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneType);
			data.Get(1, ref m_SceneDifficulty);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneType);
			data.ArrayAdd(m_SceneDifficulty);
			return data;
		}
	}

	public class Msg_CL_StartPartnerBattle : IJsonMessage
	{
		public int m_IndexId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_IndexId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_IndexId);
			return data;
		}
	}

	public class Msg_CL_StartTDChallenge : IJsonMessage
	{
		public ulong Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			return data;
		}
	}

	public class Msg_CL_SwapSkill : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public int m_SourcePos;
		public int m_TargetPos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_SourcePos);
			data.Get(3, ref m_TargetPos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_SourcePos);
			data.ArrayAdd(m_TargetPos);
			return data;
		}
	}

	public class Msg_CL_SweepStage : IJsonMessage
	{
		public int m_SceneId;
		public int m_SweepTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_SweepTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_SweepTime);
			return data;
		}
	}

	public class Msg_CL_TDBeginFight : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_TDChallengeOver : IJsonMessage
	{
		public bool IsSuccess;
		public int ChallengerDamage;
		public int TargetDamage;
		public List<DamageInfoData> ChallengerPartnerDamage = new List<DamageInfoData>();
		public List<DamageInfoData> TargetPartnerDamage = new List<DamageInfoData>();
		public int Sign;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref IsSuccess);
			data.Get(1, ref ChallengerDamage);
			data.Get(2, ref TargetDamage);
			JsonMessageUtility.GetSubDataArray(data, 3, ref ChallengerPartnerDamage);
			JsonMessageUtility.GetSubDataArray(data, 4, ref TargetPartnerDamage);
			data.Get(5, ref Sign);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(IsSuccess);
			data.ArrayAdd(ChallengerDamage);
			data.ArrayAdd(TargetDamage);
			JsonMessageUtility.AddSubDataArray(data, ChallengerPartnerDamage);
			JsonMessageUtility.AddSubDataArray(data, TargetPartnerDamage);
			data.ArrayAdd(Sign);
			return data;
		}
	}

	public class Msg_CL_UnlockSkill : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			return data;
		}
	}

	public class Msg_CL_UnmountEquipment : IJsonMessage
	{
		public int m_EquipPos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_EquipPos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_EquipPos);
			return data;
		}
	}

	public class Msg_CL_UnmountFashion : IJsonMessage
	{
		public int m_FashionID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_FashionID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_FashionID);
			return data;
		}
	}

	public class Msg_CL_UnmountSkill : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SlotPos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SlotPos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SlotPos);
			return data;
		}
	}

	public class Msg_CL_UpdateActivityUnlock : IJsonMessage
	{
		public int m_SceneId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			return data;
		}
	}

	public class Msg_CL_UpdateFightingScore : IJsonMessage
	{
		public int score;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref score);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(score);
			return data;
		}
	}

	public class Msg_CL_UpdatePosition : IJsonMessage
	{
		public float m_X;
		public float m_Z;
		public float m_FaceDir;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_X);
			data.Get(1, ref m_Z);
			data.Get(2, ref m_FaceDir);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_X);
			data.ArrayAdd(m_Z);
			data.ArrayAdd(m_FaceDir);
			return data;
		}
	}

	public class Msg_CL_UpgradeEquipBatch : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_UpgradeItem : IJsonMessage
	{
		public int m_Position;
		public int m_ItemId;
		public bool m_AllowCostGold;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Position);
			data.Get(1, ref m_ItemId);
			data.Get(2, ref m_AllowCostGold);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Position);
			data.ArrayAdd(m_ItemId);
			data.ArrayAdd(m_AllowCostGold);
			return data;
		}
	}

	public class Msg_CL_UpgradeLegacy : IJsonMessage
	{
		public int m_Index;
		public int m_ItemID;
		public bool m_AllowCostGold;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Index);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_AllowCostGold);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Index);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_AllowCostGold);
			return data;
		}
	}

	public class Msg_CL_UpgradePartnerLevel : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_CL_UpgradePartnerStage : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_CL_UpgradeSkill : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public bool m_AllowCostGold;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_AllowCostGold);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_AllowCostGold);
			return data;
		}
	}

	public class Msg_CL_UploadFPS : IJsonMessage
	{
		public string m_Fps;
		public string m_Nickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Fps);
			data.Get(1, ref m_Nickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Fps);
			data.ArrayAdd(m_Nickname);
			return data;
		}
	}

	public class Msg_CL_WeeklyLoginReward : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CL_XSoulChangeShowModel : IJsonMessage
	{
		public int m_XSoulPart;
		public int m_ModelLevel;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_XSoulPart);
			data.Get(1, ref m_ModelLevel);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_XSoulPart);
			data.ArrayAdd(m_ModelLevel);
			return data;
		}
	}

	public class Msg_CLC_CollectChapterAward : IJsonMessage
	{
		public int Chapter;
		public int OrderId;
		public int AwardSign;
		public int Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Chapter);
			data.Get(1, ref OrderId);
			data.Get(2, ref AwardSign);
			data.Get(3, ref Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Chapter);
			data.ArrayAdd(OrderId);
			data.ArrayAdd(AwardSign);
			data.ArrayAdd(Result);
			return data;
		}
	}

	public class Msg_CLC_CollectGrowthFund : IJsonMessage
	{
		public int LevelIndex;
		public int GrowthFundValue;
		public int Result;
		public int Diamond;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref LevelIndex);
			data.Get(1, ref GrowthFundValue);
			data.Get(2, ref Result);
			data.Get(3, ref Diamond);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(LevelIndex);
			data.ArrayAdd(GrowthFundValue);
			data.ArrayAdd(Result);
			data.ArrayAdd(Diamond);
			return data;
		}
	}

	public class Msg_CLC_QueryFightingScoreRank : IJsonMessage
	{
		public List<FightingScoreEntityMsg> RankEntities = new List<FightingScoreEntityMsg>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref RankEntities);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, RankEntities);
			return data;
		}
	}

	public class Msg_CLC_StoryMessage : IJsonMessage
	{

		public class MessageArg : IJsonMessage
		{
			public int val_type;
			public string str_val;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref val_type);
				data.Get(1, ref str_val);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(val_type);
				data.ArrayAdd(str_val);
				return data;
			}
		}
		public string m_MsgId;
		public List<MessageArg> m_Args = new List<MessageArg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MsgId);
			JsonMessageUtility.GetSubDataArray(data, 1, ref m_Args);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MsgId);
			JsonMessageUtility.AddSubDataArray(data, m_Args);
			return data;
		}
	}

	public class Msg_CLC_UnequipTalentCard : IJsonMessage
	{
		public int Slot;
		public int Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Slot);
			data.Get(1, ref Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Slot);
			data.ArrayAdd(Result);
			return data;
		}
	}

	public class Msg_LC_AccountLoginResult : IJsonMessage
	{
		public string m_AccountId;
		public string m_Oid;
		public string m_Token;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_AccountId);
			data.Get(1, ref m_Oid);
			data.Get(2, ref m_Token);
			data.Get(3, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_AccountId);
			data.ArrayAdd(m_Oid);
			data.ArrayAdd(m_Token);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_ActivateAccountResult : IJsonMessage
	{
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_AddAssetsResult : IJsonMessage
	{
		public int m_Money;
		public int m_Gold;
		public int m_Exp;
		public int m_Stamina;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Money);
			data.Get(1, ref m_Gold);
			data.Get(2, ref m_Exp);
			data.Get(3, ref m_Stamina);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_Stamina);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_AddFriendResult : IJsonMessage
	{
		public string m_TargetNick;
		public FriendInfoForMsg m_FriendInfo;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetNick);
			JsonMessageUtility.GetSubData(data, 1, out m_FriendInfo);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetNick);
			JsonMessageUtility.AddSubData(data, m_FriendInfo);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_AddItemResult : IJsonMessage
	{
		public ulong m_ItemGuid;
		public int m_ItemId;
		public int m_RandomProperty;
		public int m_Result;
		public int m_ItemCount;
		public int m_Exp;
		public bool m_IsCanTrade;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemGuid);
			data.Get(1, ref m_ItemId);
			data.Get(2, ref m_RandomProperty);
			data.Get(3, ref m_Result);
			data.Get(4, ref m_ItemCount);
			data.Get(5, ref m_Exp);
			data.Get(6, ref m_IsCanTrade);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemGuid);
			data.ArrayAdd(m_ItemId);
			data.ArrayAdd(m_RandomProperty);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_ItemCount);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_IsCanTrade);
			return data;
		}
	}

	public class Msg_LC_AddItemsResult : IJsonMessage
	{

		public class ItemInstance : IJsonMessage
		{
			public ulong m_ItemGuid;
			public int m_ItemId;
			public int m_RandomProperty;
			public int m_ItemCount;
			public int m_Exp;
			public bool m_IsCanTrade;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_ItemGuid);
				data.Get(1, ref m_ItemId);
				data.Get(2, ref m_RandomProperty);
				data.Get(3, ref m_ItemCount);
				data.Get(4, ref m_Exp);
				data.Get(5, ref m_IsCanTrade);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_ItemGuid);
				data.ArrayAdd(m_ItemId);
				data.ArrayAdd(m_RandomProperty);
				data.ArrayAdd(m_ItemCount);
				data.ArrayAdd(m_Exp);
				data.ArrayAdd(m_IsCanTrade);
				return data;
			}
		}
		public List<ItemInstance> Items = new List<ItemInstance>();
		public int m_Result;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref Items);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, Items);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_AddTalentExperienceResult : IJsonMessage
	{
		public int Slot;
		public List<ItemLeftMsg> ItemLefts = new List<ItemLeftMsg>();
		public int Result;
		public int Experience;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Slot);
			JsonMessageUtility.GetSubDataArray(data, 1, ref ItemLefts);
			data.Get(2, ref Result);
			data.Get(3, ref Experience);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Slot);
			JsonMessageUtility.AddSubDataArray(data, ItemLefts);
			data.ArrayAdd(Result);
			data.ArrayAdd(Experience);
			return data;
		}
	}

	public class Msg_LC_AddXSoulExperienceResult : IJsonMessage
	{
		public int m_XSoulPart;
		public ulong m_UseItemId;
		public int m_ItemNum;
		public int m_Result;
		public int m_Experience;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_XSoulPart);
			data.Get(1, ref m_UseItemId);
			data.Get(2, ref m_ItemNum);
			data.Get(3, ref m_Result);
			data.Get(4, ref m_Experience);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_XSoulPart);
			data.ArrayAdd(m_UseItemId);
			data.ArrayAdd(m_ItemNum);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Experience);
			return data;
		}
	}

	public class Msg_LC_ArenaBuyFightCountResult : IJsonMessage
	{
		public int Result;
		public int CurFightCount;
		public int CurBuyTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			data.Get(1, ref CurFightCount);
			data.Get(2, ref CurBuyTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			data.ArrayAdd(CurFightCount);
			data.ArrayAdd(CurBuyTime);
			return data;
		}
	}

	public class Msg_LC_ArenaChallengeResult : IJsonMessage
	{
		public ChallengeInfoData m_ChallengeInfo;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubData(data, 0, out m_ChallengeInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubData(data, m_ChallengeInfo);
			return data;
		}
	}

	public class Msg_LC_ArenaChangePartnerResult : IJsonMessage
	{
		public int Result;
		public List<int> Partners = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			JsonMessageUtility.GetSimpleArray(data, 1, ref Partners);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			JsonMessageUtility.AddSimpleArray(data, Partners);
			return data;
		}
	}

	public class Msg_LC_ArenaInfoResult : IJsonMessage
	{
		public ArenaInfoMsg m_ArenaInfo;
		public int m_LeftBattleCount;
		public int m_CurFightCountByTime;
		public long m_BattleLeftCDTime;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubData(data, 0, out m_ArenaInfo);
			data.Get(1, ref m_LeftBattleCount);
			data.Get(2, ref m_CurFightCountByTime);
			data.Get(3, ref m_BattleLeftCDTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubData(data, m_ArenaInfo);
			data.ArrayAdd(m_LeftBattleCount);
			data.ArrayAdd(m_CurFightCountByTime);
			data.ArrayAdd(m_BattleLeftCDTime);
			return data;
		}
	}

	public class Msg_LC_ArenaMatchGroupResult : IJsonMessage
	{

		public class MatchGroupData : IJsonMessage
		{
			public ArenaInfoMsg One;
			public ArenaInfoMsg Two;
			public ArenaInfoMsg Three;

			public void FromJson(JsonData data)
			{
				JsonMessageUtility.GetSubData(data, 0, out One);
				JsonMessageUtility.GetSubData(data, 1, out Two);
				JsonMessageUtility.GetSubData(data, 2, out Three);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				JsonMessageUtility.AddSubData(data, One);
				JsonMessageUtility.AddSubData(data, Two);
				JsonMessageUtility.AddSubData(data, Three);
				return data;
			}
		}
		public List<MatchGroupData> m_MatchGroups = new List<MatchGroupData>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_MatchGroups);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_MatchGroups);
			return data;
		}
	}

	public class Msg_LC_ArenaQueryHistoryResult : IJsonMessage
	{
		public List<ChallengeInfoData> ChallengeHistory = new List<ChallengeInfoData>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref ChallengeHistory);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, ChallengeHistory);
			return data;
		}
	}

	public class Msg_LC_ArenaQueryRankResult : IJsonMessage
	{
		public List<ArenaInfoMsg> RankMsg = new List<ArenaInfoMsg>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref RankMsg);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, RankMsg);
			return data;
		}
	}

	public class Msg_LC_ArenaStartCallengeResult : IJsonMessage
	{
		public ulong m_TargetGuid;
		public int m_Sign;
		public int m_ResultCode;
		public int m_Prime;
		public ArenaInfoMsg TargetInfo;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_Sign);
			data.Get(2, ref m_ResultCode);
			data.Get(3, ref m_Prime);
			JsonMessageUtility.GetSubData(data, 4, out TargetInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_Sign);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_Prime);
			JsonMessageUtility.AddSubData(data, TargetInfo);
			return data;
		}
	}

	public class Msg_LC_AuctionAuction : IJsonMessage
	{
		public List<AuctionInfo> auctionInfo = new List<AuctionInfo>();
		public bool IsLastPage;
		public int PageNo;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref auctionInfo);
			data.Get(1, ref IsLastPage);
			data.Get(2, ref PageNo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, auctionInfo);
			data.ArrayAdd(IsLastPage);
			data.ArrayAdd(PageNo);
			return data;
		}
	}

	public class Msg_LC_AuctionOpResult : IJsonMessage
	{
		public int result;
		public ulong ItemGuid;
		public int ItemNum;
		public int ItemType;
		public int Price;
		public JsonItemDataMsg itemInfo;
		public int Cost;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref result);
			data.Get(1, ref ItemGuid);
			data.Get(2, ref ItemNum);
			data.Get(3, ref ItemType);
			data.Get(4, ref Price);
			JsonMessageUtility.GetSubData(data, 5, out itemInfo);
			data.Get(6, ref Cost);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(result);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemNum);
			data.ArrayAdd(ItemType);
			data.ArrayAdd(Price);
			JsonMessageUtility.AddSubData(data, itemInfo);
			data.ArrayAdd(Cost);
			return data;
		}
	}

	public class Msg_LC_AuctionSelfAuction : IJsonMessage
	{
		public List<AuctionInfo> auctionInfo = new List<AuctionInfo>();
		public bool IsAuctionOpen;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref auctionInfo);
			data.Get(1, ref IsAuctionOpen);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, auctionInfo);
			data.ArrayAdd(IsAuctionOpen);
			return data;
		}
	}

	public class Msg_LC_AwardFashionResult : IJsonMessage
	{
		public int m_ItemID;
		public int m_DeadlineSeconds;
		public bool m_DressOn;
		public int m_UseDays;
		public bool m_IsForever;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
			data.Get(1, ref m_DeadlineSeconds);
			data.Get(2, ref m_DressOn);
			data.Get(3, ref m_UseDays);
			data.Get(4, ref m_IsForever);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_DeadlineSeconds);
			data.ArrayAdd(m_DressOn);
			data.ArrayAdd(m_UseDays);
			data.ArrayAdd(m_IsForever);
			return data;
		}
	}

	public class Msg_LC_BuyEliteCountResult : IJsonMessage
	{
		public int m_EliteId;
		public int m_ResultCode;
		public int m_DiamondCount;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_EliteId);
			data.Get(1, ref m_ResultCode);
			data.Get(2, ref m_DiamondCount);
			data.Get(3, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_EliteId);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_DiamondCount);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_BuyFashionResult : IJsonMessage
	{
		public int m_Result;
		public int m_ItemID;
		public int m_TimeType;
		public int m_DeadlineSeconds;
		public bool m_DressOn;
		public int m_Money;
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_TimeType);
			data.Get(3, ref m_DeadlineSeconds);
			data.Get(4, ref m_DressOn);
			data.Get(5, ref m_Money);
			data.Get(6, ref m_Gold);
			data.Get(7, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_TimeType);
			data.ArrayAdd(m_DeadlineSeconds);
			data.ArrayAdd(m_DressOn);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_BuyGoodsSucceed : IJsonMessage
	{
		public string m_GoodsId;
		public bool m_IsFirstPay;
		public bool m_IsFirstBuyThis;
		public int m_VipLevel;
		public string m_AccountId;
		public string m_OrderId;
		public string m_PayType;
		public int m_GoodsNum;
		public string m_CurrencyType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_GoodsId);
			data.Get(1, ref m_IsFirstPay);
			data.Get(2, ref m_IsFirstBuyThis);
			data.Get(3, ref m_VipLevel);
			data.Get(4, ref m_AccountId);
			data.Get(5, ref m_OrderId);
			data.Get(6, ref m_PayType);
			data.Get(7, ref m_GoodsNum);
			data.Get(8, ref m_CurrencyType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_GoodsId);
			data.ArrayAdd(m_IsFirstPay);
			data.ArrayAdd(m_IsFirstBuyThis);
			data.ArrayAdd(m_VipLevel);
			data.ArrayAdd(m_AccountId);
			data.ArrayAdd(m_OrderId);
			data.ArrayAdd(m_PayType);
			data.ArrayAdd(m_GoodsNum);
			data.ArrayAdd(m_CurrencyType);
			return data;
		}
	}

	public class Msg_LC_BuyLifeResult : IJsonMessage
	{
		public bool m_Succeed;
		public int m_CurDiamond;
		public int m_CostReliveStone;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Succeed);
			data.Get(1, ref m_CurDiamond);
			data.Get(2, ref m_CostReliveStone);
			data.Get(3, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Succeed);
			data.ArrayAdd(m_CurDiamond);
			data.ArrayAdd(m_CostReliveStone);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_BuyPartnerCombatTicketResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_CurDiamond;
		public int m_RemainCount;
		public int m_BuyCount;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_CurDiamond);
			data.Get(2, ref m_RemainCount);
			data.Get(3, ref m_BuyCount);
			data.Get(4, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_CurDiamond);
			data.ArrayAdd(m_RemainCount);
			data.ArrayAdd(m_BuyCount);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_BuyStaminaResult : IJsonMessage
	{
		public int m_Result;
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Gold);
			data.Get(2, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_BuyTDFightCountResult : IJsonMessage
	{
		public int Result;
		public int BuyCount;
		public int CurFightCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			data.Get(1, ref BuyCount);
			data.Get(2, ref CurFightCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			data.ArrayAdd(BuyCount);
			data.ArrayAdd(CurFightCount);
			return data;
		}
	}

	public class Msg_LC_CancelSelectPartnerResult : IJsonMessage
	{
		public int m_ResultCode;
		public List<int> m_Partners = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_Partners);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSimpleArray(data, m_Partners);
			return data;
		}
	}

	public class Msg_LC_ChangeCaptain : IJsonMessage
	{
		public ulong m_CreatorGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_CreatorGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_CreatorGuid);
			return data;
		}
	}

	public class Msg_LC_ChatAddShieldResult : IJsonMessage
	{
		public int m_Result;
		public ulong m_TargetGuid;
		public string m_TargetNickName;
		public ChatShieldInfoForMsg m_ShieldInfo;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_TargetGuid);
			data.Get(2, ref m_TargetNickName);
			JsonMessageUtility.GetSubData(data, 3, out m_ShieldInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_TargetNickName);
			JsonMessageUtility.AddSubData(data, m_ShieldInfo);
			return data;
		}
	}

	public class Msg_LC_ChatDelShieldResult : IJsonMessage
	{
		public ulong m_TargetGuid;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_ChatEquipInfoReturn : IJsonMessage
	{
		public ulong m_TargetGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			return data;
		}
	}

	public class Msg_LC_ChatResult : IJsonMessage
	{
		public int m_Type;
		public ulong m_SenderGuid;
		public string m_SenderNickName;
		public int m_SenderHeroId;
		public ulong m_TargetGuid;
		public string m_TargetNickName;
		public int m_TargetHeroId;
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Type);
			data.Get(1, ref m_SenderGuid);
			data.Get(2, ref m_SenderNickName);
			data.Get(3, ref m_SenderHeroId);
			data.Get(4, ref m_TargetGuid);
			data.Get(5, ref m_TargetNickName);
			data.Get(6, ref m_TargetHeroId);
			data.Get(7, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Type);
			data.ArrayAdd(m_SenderGuid);
			data.ArrayAdd(m_SenderNickName);
			data.ArrayAdd(m_SenderHeroId);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_TargetNickName);
			data.ArrayAdd(m_TargetHeroId);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_LC_ChatRoleInfoReturn : IJsonMessage
	{
		public ulong m_TargetGuid;
		public string m_TargetNickName;
		public int m_TargetLevel;
		public int m_TargetPower;
		public bool m_IsShield;
		public bool m_IsOnline;
		public int m_HeroId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_TargetNickName);
			data.Get(2, ref m_TargetLevel);
			data.Get(3, ref m_TargetPower);
			data.Get(4, ref m_IsShield);
			data.Get(5, ref m_IsOnline);
			data.Get(6, ref m_HeroId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_TargetNickName);
			data.ArrayAdd(m_TargetLevel);
			data.ArrayAdd(m_TargetPower);
			data.ArrayAdd(m_IsShield);
			data.ArrayAdd(m_IsOnline);
			data.ArrayAdd(m_HeroId);
			return data;
		}
	}

	public class Msg_LC_ChatShieldListReturn : IJsonMessage
	{
		public List<ChatShieldInfoForMsg> m_ShieldInfoList = new List<ChatShieldInfoForMsg>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_ShieldInfoList);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_ShieldInfoList);
			return data;
		}
	}

	public class Msg_LC_ChatStatus : IJsonMessage
	{
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_ChatWorldResult : IJsonMessage
	{
		public ulong m_SenderGuid;
		public string m_SenderNickName;
		public int m_SenderHeroId;
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SenderGuid);
			data.Get(1, ref m_SenderNickName);
			data.Get(2, ref m_SenderHeroId);
			data.Get(3, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SenderGuid);
			data.ArrayAdd(m_SenderNickName);
			data.ArrayAdd(m_SenderHeroId);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_LC_CompoundEquipResult : IJsonMessage
	{
		public int m_Result;
		public int m_PartId;
		public int m_ItemId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_PartId);
			data.Get(2, ref m_ItemId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_PartId);
			data.ArrayAdd(m_ItemId);
			return data;
		}
	}

	public class Msg_LC_CompoundPartnerResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_LC_ConfirmJoinGroupResult : IJsonMessage
	{
		public int m_Result;
		public string m_Nick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Nick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Nick);
			return data;
		}
	}

	public class Msg_LC_CorpsSignIn : IJsonMessage
	{
		public int m_Stamina;
		public bool m_CorpsSignInState;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Stamina);
			data.Get(1, ref m_CorpsSignInState);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Stamina);
			data.ArrayAdd(m_CorpsSignInState);
			return data;
		}
	}

	public class Msg_LC_CreateNicnameResult : IJsonMessage
	{
		public List<string> m_Nicknames = new List<string>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_Nicknames);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_Nicknames);
			return data;
		}
	}

	public class Msg_LC_CreateRoleResult : IJsonMessage
	{
		public int m_Result;
		public string m_Nickname;
		public int m_HeroId;
		public int m_Level;
		public ulong m_UserGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Nickname);
			data.Get(2, ref m_HeroId);
			data.Get(3, ref m_Level);
			data.Get(4, ref m_UserGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Nickname);
			data.ArrayAdd(m_HeroId);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_UserGuid);
			return data;
		}
	}

	public class Msg_LC_DelFriendResult : IJsonMessage
	{
		public ulong m_TargetGuid;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_DiamondExtraBuyBoxResult : IJsonMessage
	{

		public class AwardItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_BoxPlace;
		public int m_Result;
		public bool m_Fresh;
		public int m_AddMoney;
		public int m_AddGold;
		public List<AwardItemInfo> m_Items = new List<AwardItemInfo>();
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_BoxPlace);
			data.Get(1, ref m_Result);
			data.Get(2, ref m_Fresh);
			data.Get(3, ref m_AddMoney);
			data.Get(4, ref m_AddGold);
			JsonMessageUtility.GetSubDataArray(data, 5, ref m_Items);
			data.Get(6, ref m_Gold);
			data.Get(7, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_BoxPlace);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Fresh);
			data.ArrayAdd(m_AddMoney);
			data.ArrayAdd(m_AddGold);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_DiscardItemResult : IJsonMessage
	{
		public List<ulong> m_ItemGuids = new List<ulong>();
		public int m_TotalIncome;
		public int m_Gold;
		public int m_Money;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_ItemGuids);
			data.Get(1, ref m_TotalIncome);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_Money);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_ItemGuids);
			data.ArrayAdd(m_TotalIncome);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_DrawRewardResult : IJsonMessage
	{

		public class LotteryInfo : IJsonMessage
		{
			public int m_Id;
			public int m_CurFreeCount;
			public string m_LastDrawTime;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_CurFreeCount);
				data.Get(2, ref m_LastDrawTime);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_CurFreeCount);
				data.ArrayAdd(m_LastDrawTime);
				return data;
			}
		}
		public int m_Result;
		public int m_Money;
		public int m_Diamond;
		public List<int> m_RewardId = new List<int>();
		public List<LotteryInfo> m_Lottery = new List<LotteryInfo>();
		public int m_LotteryType;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Money);
			data.Get(2, ref m_Diamond);
			JsonMessageUtility.GetSimpleArray(data, 3, ref m_RewardId);
			JsonMessageUtility.GetSubDataArray(data, 4, ref m_Lottery);
			data.Get(5, ref m_LotteryType);
			data.Get(6, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Diamond);
			JsonMessageUtility.AddSimpleArray(data, m_RewardId);
			JsonMessageUtility.AddSubDataArray(data, m_Lottery);
			data.ArrayAdd(m_LotteryType);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_EndPartnerBattleResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_RemainCount;
		public int m_FinishedCount;
		public int m_BattleResult;
		public int m_RewardItemId;
		public int m_RewardItemNum;
		public List<int> m_Partners = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_RemainCount);
			data.Get(2, ref m_FinishedCount);
			data.Get(3, ref m_BattleResult);
			data.Get(4, ref m_RewardItemId);
			data.Get(5, ref m_RewardItemNum);
			JsonMessageUtility.GetSimpleArray(data, 6, ref m_Partners);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_RemainCount);
			data.ArrayAdd(m_FinishedCount);
			data.ArrayAdd(m_BattleResult);
			data.ArrayAdd(m_RewardItemId);
			data.ArrayAdd(m_RewardItemNum);
			JsonMessageUtility.AddSimpleArray(data, m_Partners);
			return data;
		}
	}

	public class Msg_LC_EquipmentStrengthResult : IJsonMessage
	{

		public class DeleteItemInfo : IJsonMessage
		{
			public ulong m_ItemGuid;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_ItemGuid);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_ItemGuid);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_Result;
		public int m_ItemID;
		public int m_NewStrengthLv;
		public int m_OldStrengthLv;
		public bool m_DeductItem;
		public bool m_IsProtected;
		public List<DeleteItemInfo> m_Items = new List<DeleteItemInfo>();
		public int m_StrengthFailCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_NewStrengthLv);
			data.Get(3, ref m_OldStrengthLv);
			data.Get(4, ref m_DeductItem);
			data.Get(5, ref m_IsProtected);
			JsonMessageUtility.GetSubDataArray(data, 6, ref m_Items);
			data.Get(7, ref m_StrengthFailCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_NewStrengthLv);
			data.ArrayAdd(m_OldStrengthLv);
			data.ArrayAdd(m_DeductItem);
			data.ArrayAdd(m_IsProtected);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_StrengthFailCount);
			return data;
		}
	}

	public class Msg_LC_EquipTalentCardResult : IJsonMessage
	{
		public ulong ItemGuid;
		public ulong OldItemGuid;
		public int Result;
		public int Slot;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
			data.Get(1, ref OldItemGuid);
			data.Get(2, ref Result);
			data.Get(3, ref Slot);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(OldItemGuid);
			data.ArrayAdd(Result);
			data.ArrayAdd(Slot);
			return data;
		}
	}

	public class Msg_LC_ExchangeGiftResult : IJsonMessage
	{

		public class GiftItemInfo : IJsonMessage
		{
			public int m_ItemId;
			public int m_ItemNumber;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_ItemId);
				data.Get(1, ref m_ItemNumber);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_ItemId);
				data.ArrayAdd(m_ItemNumber);
				return data;
			}
		}
		public int m_GiftId;
		public int m_Result;
		public string m_GiftName;
		public string m_GiftDesc;
		public List<GiftItemInfo> m_GiftItems = new List<GiftItemInfo>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_GiftId);
			data.Get(1, ref m_Result);
			data.Get(2, ref m_GiftName);
			data.Get(3, ref m_GiftDesc);
			JsonMessageUtility.GetSubDataArray(data, 4, ref m_GiftItems);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_GiftId);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_GiftName);
			data.ArrayAdd(m_GiftDesc);
			JsonMessageUtility.AddSubDataArray(data, m_GiftItems);
			return data;
		}
	}

	public class Msg_LC_ExchangeGoodsResult : IJsonMessage
	{
		public int m_ExchangeId;
		public int m_NpcId;
		public int m_ExchangeNum;
		public int m_Result;
		public bool m_Refresh;
		public int m_Money;
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ExchangeId);
			data.Get(1, ref m_NpcId);
			data.Get(2, ref m_ExchangeNum);
			data.Get(3, ref m_Result);
			data.Get(4, ref m_Refresh);
			data.Get(5, ref m_Money);
			data.Get(6, ref m_Gold);
			data.Get(7, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ExchangeId);
			data.ArrayAdd(m_NpcId);
			data.ArrayAdd(m_ExchangeNum);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Refresh);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_ExpeditionAwardResult : IJsonMessage
	{

		public class AwardItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_TollgateNum;
		public List<AwardItemInfo> m_Items = new List<AwardItemInfo>();
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TollgateNum);
			JsonMessageUtility.GetSubDataArray(data, 1, ref m_Items);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TollgateNum);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_ExpeditionResetResult : IJsonMessage
	{

		public class ExpeditionPartner : IJsonMessage
		{
			public int Id;
			public int Hp;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref Id);
				data.Get(1, ref Hp);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(Id);
				data.ArrayAdd(Hp);
				return data;
			}
		}

		public class ImageDataMsg : IJsonMessage
		{
			public ulong Guid;
			public int HeroId;
			public string Nickname;
			public int Level;
			public int FightingScore;
			public List<JsonItemDataMsg> EquipInfo = new List<JsonItemDataMsg>();
			public List<SkillDataInfo> SkillInfo = new List<SkillDataInfo>();
			public List<LegacyDataMsg> LegacyInfo = new List<LegacyDataMsg>();
			public List<PartnerDataMsg> Partners = new List<PartnerDataMsg>();
			public List<XSoulDataMsg> XSouls = new List<XSoulDataMsg>();
			public List<JsonTalentDataMsg> Talents = new List<JsonTalentDataMsg>();
			public List<FashionSynMsg> Fashions = new List<FashionSynMsg>();

			public void FromJson(JsonData data)
			{
				data.Get(0, ref Guid);
				data.Get(1, ref HeroId);
				data.Get(2, ref Nickname);
				data.Get(3, ref Level);
				data.Get(4, ref FightingScore);
				JsonMessageUtility.GetSubDataArray(data, 5, ref EquipInfo);
				JsonMessageUtility.GetSubDataArray(data, 6, ref SkillInfo);
				JsonMessageUtility.GetSubDataArray(data, 7, ref LegacyInfo);
				JsonMessageUtility.GetSubDataArray(data, 8, ref Partners);
				JsonMessageUtility.GetSubDataArray(data, 9, ref XSouls);
				JsonMessageUtility.GetSubDataArray(data, 10, ref Talents);
				JsonMessageUtility.GetSubDataArray(data, 11, ref Fashions);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(Guid);
				data.ArrayAdd(HeroId);
				data.ArrayAdd(Nickname);
				data.ArrayAdd(Level);
				data.ArrayAdd(FightingScore);
				JsonMessageUtility.AddSubDataArray(data, EquipInfo);
				JsonMessageUtility.AddSubDataArray(data, SkillInfo);
				JsonMessageUtility.AddSubDataArray(data, LegacyInfo);
				JsonMessageUtility.AddSubDataArray(data, Partners);
				JsonMessageUtility.AddSubDataArray(data, XSouls);
				JsonMessageUtility.AddSubDataArray(data, Talents);
				JsonMessageUtility.AddSubDataArray(data, Fashions);
				return data;
			}
		}

		public class TollgateDataForMsg : IJsonMessage
		{
			public int Type;
			public bool IsFinish;
			public List<bool> IsAcceptedAward = new List<bool>();
			public List<int> EnemyArray = new List<int>();
			public List<int> EnemyAttrArray = new List<int>();
			public List<ImageDataMsg> UserImageArray = new List<ImageDataMsg>();

			public void FromJson(JsonData data)
			{
				data.Get(0, ref Type);
				data.Get(1, ref IsFinish);
				JsonMessageUtility.GetSimpleArray(data, 2, ref IsAcceptedAward);
				JsonMessageUtility.GetSimpleArray(data, 3, ref EnemyArray);
				JsonMessageUtility.GetSimpleArray(data, 4, ref EnemyAttrArray);
				JsonMessageUtility.GetSubDataArray(data, 5, ref UserImageArray);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(Type);
				data.ArrayAdd(IsFinish);
				JsonMessageUtility.AddSimpleArray(data, IsAcceptedAward);
				JsonMessageUtility.AddSimpleArray(data, EnemyArray);
				JsonMessageUtility.AddSimpleArray(data, EnemyAttrArray);
				JsonMessageUtility.AddSubDataArray(data, UserImageArray);
				return data;
			}
		}
		public int m_Hp;
		public int m_Mp;
		public int m_Rage;
		public int m_Schedule;
		public int m_CurResetCount;
		public TollgateDataForMsg Tollgates;
		public bool m_AllowCostGold;
		public bool m_IsUnlock;
		public int m_Result;
		public List<ExpeditionPartner> Partners = new List<ExpeditionPartner>();
		public int LastAchievedSchedule;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Hp);
			data.Get(1, ref m_Mp);
			data.Get(2, ref m_Rage);
			data.Get(3, ref m_Schedule);
			data.Get(4, ref m_CurResetCount);
			JsonMessageUtility.GetSubData(data, 5, out Tollgates);
			data.Get(6, ref m_AllowCostGold);
			data.Get(7, ref m_IsUnlock);
			data.Get(8, ref m_Result);
			JsonMessageUtility.GetSubDataArray(data, 9, ref Partners);
			data.Get(10, ref LastAchievedSchedule);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Rage);
			data.ArrayAdd(m_Schedule);
			data.ArrayAdd(m_CurResetCount);
			JsonMessageUtility.AddSubData(data, Tollgates);
			data.ArrayAdd(m_AllowCostGold);
			data.ArrayAdd(m_IsUnlock);
			data.ArrayAdd(m_Result);
			JsonMessageUtility.AddSubDataArray(data, Partners);
			data.ArrayAdd(LastAchievedSchedule);
			return data;
		}
	}

	public class Msg_LC_ExpeditionSweepResult : IJsonMessage
	{
		public int m_TollgateNum;
		public int m_Hp;
		public int m_Mp;
		public int m_Rage;
		public int m_Diamond;
		public int m_Result;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TollgateNum);
			data.Get(1, ref m_Hp);
			data.Get(2, ref m_Mp);
			data.Get(3, ref m_Rage);
			data.Get(4, ref m_Diamond);
			data.Get(5, ref m_Result);
			data.Get(6, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TollgateNum);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Rage);
			data.ArrayAdd(m_Diamond);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_FinishExpeditionResult : IJsonMessage
	{
		public int m_SceneId;
		public int m_TollgateNum;
		public int m_Hp;
		public int m_Mp;
		public int m_Rage;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_TollgateNum);
			data.Get(2, ref m_Hp);
			data.Get(3, ref m_Mp);
			data.Get(4, ref m_Rage);
			data.Get(5, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_TollgateNum);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Rage);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_FinishMissionResult : IJsonMessage
	{

		public class MissionInfoForSync : IJsonMessage
		{
			public int m_MissionId;
			public bool m_IsCompleted;
			public string m_Progress;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_MissionId);
				data.Get(1, ref m_IsCompleted);
				data.Get(2, ref m_Progress);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_MissionId);
				data.ArrayAdd(m_IsCompleted);
				data.ArrayAdd(m_Progress);
				return data;
			}
		}
		public int m_ResultCode;
		public int m_FinishMissionId;
		public int m_Gold;
		public int m_Exp;
		public int m_Diamond;
		public List<MissionInfoForSync> m_UnlockMissions = new List<MissionInfoForSync>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_FinishMissionId);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_Exp);
			data.Get(4, ref m_Diamond);
			JsonMessageUtility.GetSubDataArray(data, 5, ref m_UnlockMissions);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_FinishMissionId);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_Diamond);
			JsonMessageUtility.AddSubDataArray(data, m_UnlockMissions);
			return data;
		}
	}

	public class Msg_LC_FriendOffline : IJsonMessage
	{
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class Msg_LC_FriendOnline : IJsonMessage
	{
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class Msg_LC_GainFirstPayRewardResult : IJsonMessage
	{
		public int m_ResultCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			return data;
		}
	}

	public class Msg_LC_GainVipRewardResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_VipLevel;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_VipLevel);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_VipLevel);
			return data;
		}
	}

	public class Msg_LC_GetLoginLotteryResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_RewardId;
		public int m_UsedLoginDrawLotteryCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_RewardId);
			data.Get(2, ref m_UsedLoginDrawLotteryCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_RewardId);
			data.ArrayAdd(m_UsedLoginDrawLotteryCount);
			return data;
		}
	}

	public class Msg_LC_GetMorrowRewardResult : IJsonMessage
	{
		public int m_ResultCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			return data;
		}
	}

	public class Msg_LC_GetOnlineTimeRewardResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_OnlineTime;
		public int m_Index;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_OnlineTime);
			data.Get(2, ref m_Index);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_OnlineTime);
			data.ArrayAdd(m_Index);
			return data;
		}
	}

	public class Msg_LC_GetPartner : IJsonMessage
	{
		public int m_PartnerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PartnerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PartnerId);
			return data;
		}
	}

	public class Msg_LC_GmCode : IJsonMessage
	{
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_LC_GmPayResult : IJsonMessage
	{
		public int m_Vip;
		public int m_Diamonds;
		public string m_DateTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Vip);
			data.Get(1, ref m_Diamonds);
			data.Get(2, ref m_DateTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Vip);
			data.ArrayAdd(m_Diamonds);
			data.ArrayAdd(m_DateTime);
			return data;
		}
	}

	public class Msg_LC_InteractivePrize : IJsonMessage
	{
		public int m_ActorId;
		public int m_LinkId;
		public int m_StoryId;
		public bool m_IsValid;
		public List<int> m_Ids = new List<int>();
		public List<int> m_Nums = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ActorId);
			data.Get(1, ref m_LinkId);
			data.Get(2, ref m_StoryId);
			data.Get(3, ref m_IsValid);
			JsonMessageUtility.GetSimpleArray(data, 4, ref m_Ids);
			JsonMessageUtility.GetSimpleArray(data, 5, ref m_Nums);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ActorId);
			data.ArrayAdd(m_LinkId);
			data.ArrayAdd(m_StoryId);
			data.ArrayAdd(m_IsValid);
			JsonMessageUtility.AddSimpleArray(data, m_Ids);
			JsonMessageUtility.AddSimpleArray(data, m_Nums);
			return data;
		}
	}

	public class Msg_LC_InviteInfoAfterRoleEnter : IJsonMessage
	{
		public string m_InviteCode;
		public bool m_IsInvited;
		public int m_InviteeCount;
		public int m_OverLv30Count;
		public int m_OverLv50Count;
		public List<int> m_RewardsHaveRecived = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_InviteCode);
			data.Get(1, ref m_IsInvited);
			data.Get(2, ref m_InviteeCount);
			data.Get(3, ref m_OverLv30Count);
			data.Get(4, ref m_OverLv50Count);
			JsonMessageUtility.GetSimpleArray(data, 5, ref m_RewardsHaveRecived);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_InviteCode);
			data.ArrayAdd(m_IsInvited);
			data.ArrayAdd(m_InviteeCount);
			data.ArrayAdd(m_OverLv30Count);
			data.ArrayAdd(m_OverLv50Count);
			JsonMessageUtility.AddSimpleArray(data, m_RewardsHaveRecived);
			return data;
		}
	}

	public class Msg_LC_ItemsGiveBack : IJsonMessage
	{

		public class ItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public List<ItemInfo> m_Items = new List<ItemInfo>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Items);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			return data;
		}
	}

	public class Msg_LC_ItemUseResult : IJsonMessage
	{
		public int m_ResultCode;
		public List<ulong> m_ItemGuids = new List<ulong>();
		public List<int> m_nums = new List<int>();
		public List<ItemInfo_UseItem> m_Items = new List<ItemInfo_UseItem>();
		public int m_arg;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_ItemGuids);
			JsonMessageUtility.GetSimpleArray(data, 2, ref m_nums);
			JsonMessageUtility.GetSubDataArray(data, 3, ref m_Items);
			data.Get(4, ref m_arg);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSimpleArray(data, m_ItemGuids);
			JsonMessageUtility.AddSimpleArray(data, m_nums);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_arg);
			return data;
		}
	}

	public class Msg_LC_LackOfSpace : IJsonMessage
	{
		public bool m_Succeed;
		public int m_Type;
		public int m_ReceiveNum;
		public int m_FreeNum;
		public ulong m_MailGuid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Succeed);
			data.Get(1, ref m_Type);
			data.Get(2, ref m_ReceiveNum);
			data.Get(3, ref m_FreeNum);
			data.Get(4, ref m_MailGuid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Succeed);
			data.ArrayAdd(m_Type);
			data.ArrayAdd(m_ReceiveNum);
			data.ArrayAdd(m_FreeNum);
			data.ArrayAdd(m_MailGuid);
			return data;
		}
	}

	public class Msg_LC_LiftSkillResult : IJsonMessage
	{
		public int m_SkillID;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SkillID);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_LootChangeDefenseOrder : IJsonMessage
	{
		public ulong Key;
		public List<int> Order = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Key);
			JsonMessageUtility.GetSimpleArray(data, 1, ref Order);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Key);
			JsonMessageUtility.AddSimpleArray(data, Order);
			return data;
		}
	}

	public class Msg_LC_LootChangeLootOrder : IJsonMessage
	{
		public ulong Key;
		public List<int> Order = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Key);
			JsonMessageUtility.GetSimpleArray(data, 1, ref Order);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Key);
			JsonMessageUtility.AddSimpleArray(data, Order);
			return data;
		}
	}

	public class Msg_LC_LootCostVitality : IJsonMessage
	{
		public int m_Result;
		public int m_Vitality;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Vitality);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Vitality);
			return data;
		}
	}

	public class Msg_LC_LootMatchResult : IJsonMessage
	{
		public int Result;
		public int Money;
		public int Income;
		public int LootType;
		public bool IsShow;
		public LootInfoMsg Target;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			data.Get(1, ref Money);
			data.Get(2, ref Income);
			data.Get(3, ref LootType);
			data.Get(4, ref IsShow);
			JsonMessageUtility.GetSubData(data, 5, out Target);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			data.ArrayAdd(Money);
			data.ArrayAdd(Income);
			data.ArrayAdd(LootType);
			data.ArrayAdd(IsShow);
			JsonMessageUtility.AddSubData(data, Target);
			return data;
		}
	}

	public class Msg_LC_LootOpResult : IJsonMessage
	{
		public int Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			return data;
		}
	}

	public class Msg_LC_MatchResult : IJsonMessage
	{
		public int m_Result;
		public List<int> m_UserLevel = new List<int>();
		public List<string> m_UserName = new List<string>();
		public List<int> m_UserHeroId = new List<int>();
		public List<int> m_LogicServerId = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_UserLevel);
			JsonMessageUtility.GetSimpleArray(data, 2, ref m_UserName);
			JsonMessageUtility.GetSimpleArray(data, 3, ref m_UserHeroId);
			JsonMessageUtility.GetSimpleArray(data, 4, ref m_LogicServerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			JsonMessageUtility.AddSimpleArray(data, m_UserLevel);
			JsonMessageUtility.AddSimpleArray(data, m_UserName);
			JsonMessageUtility.AddSimpleArray(data, m_UserHeroId);
			JsonMessageUtility.AddSimpleArray(data, m_LogicServerId);
			return data;
		}
	}

	public class Msg_LC_MidasTouchResult : IJsonMessage
	{
		public int m_Count;
		public int m_Money;
		public int m_Gold;
		public int m_GoldCash;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Count);
			data.Get(1, ref m_Money);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_GoldCash);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Count);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_MissionCompleted : IJsonMessage
	{
		public int m_MissionId;
		public string m_Progress;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MissionId);
			data.Get(1, ref m_Progress);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MissionId);
			data.ArrayAdd(m_Progress);
			return data;
		}
	}

	public class Msg_LC_MorrowRewardActive : IJsonMessage
	{
		public int m_ActiveIndex;
		public int m_RemainTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ActiveIndex);
			data.Get(1, ref m_RemainTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ActiveIndex);
			data.ArrayAdd(m_RemainTime);
			return data;
		}
	}

	public class Msg_LC_MountEquipmentResult : IJsonMessage
	{
		public ulong m_ItemGuid;
		public int m_EquipPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemGuid);
			data.Get(1, ref m_EquipPos);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemGuid);
			data.ArrayAdd(m_EquipPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_MountFashionResult : IJsonMessage
	{
		public int m_Result;
		public int m_FashionID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_FashionID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_FashionID);
			return data;
		}
	}

	public class Msg_LC_MountSkillResult : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public int m_SlotPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_SlotPos);
			data.Get(3, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_SlotPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_MpveAwardResult : IJsonMessage
	{

		public class AwardItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_SceneType;
		public int m_Result;
		public int m_AwardIndex;
		public int m_AddMoney;
		public int m_AddGold;
		public List<AwardItemInfo> m_Items = new List<AwardItemInfo>();
		public int m_DareCount;
		public int m_AwardCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneType);
			data.Get(1, ref m_Result);
			data.Get(2, ref m_AwardIndex);
			data.Get(3, ref m_AddMoney);
			data.Get(4, ref m_AddGold);
			JsonMessageUtility.GetSubDataArray(data, 5, ref m_Items);
			data.Get(6, ref m_DareCount);
			data.Get(7, ref m_AwardCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneType);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_AwardIndex);
			data.ArrayAdd(m_AddMoney);
			data.ArrayAdd(m_AddGold);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_DareCount);
			data.ArrayAdd(m_AwardCount);
			return data;
		}
	}

	public class Msg_LC_MpveGeneralResult : IJsonMessage
	{
		public int m_Result;
		public string m_Nick;
		public int m_Type;
		public int m_Difficulty;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Nick);
			data.Get(2, ref m_Type);
			data.Get(3, ref m_Difficulty);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Nick);
			data.ArrayAdd(m_Type);
			data.ArrayAdd(m_Difficulty);
			return data;
		}
	}

	public class Msg_LC_NoticeFashionOverdue : IJsonMessage
	{
		public int m_ItemID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			return data;
		}
	}

	public class Msg_LC_NoticeFashionOverdueSoon : IJsonMessage
	{
		public int m_ItemID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ItemID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ItemID);
			return data;
		}
	}

	public class Msg_LC_NoticePlayerOffline : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_NoticeQuitGroup : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_NotifyMorrowReward : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_NotifyNewMail : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_OverLootResult : IJsonMessage
	{
		public int DomainType;
		public int Booty;
		public LootEntityData Looter;
		public LootEntityData Defender;
		public bool IsLootSuccess;
		public string EndTime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref DomainType);
			data.Get(1, ref Booty);
			JsonMessageUtility.GetSubData(data, 2, out Looter);
			JsonMessageUtility.GetSubData(data, 3, out Defender);
			data.Get(4, ref IsLootSuccess);
			data.Get(5, ref EndTime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(DomainType);
			data.ArrayAdd(Booty);
			JsonMessageUtility.AddSubData(data, Looter);
			JsonMessageUtility.AddSubData(data, Defender);
			data.ArrayAdd(IsLootSuccess);
			data.ArrayAdd(EndTime);
			return data;
		}
	}

	public class Msg_LC_PartnerCombatInfo : IJsonMessage
	{
		public List<int> m_Partners = new List<int>();
		public int m_RemainCount;
		public int m_FinishedCount;
		public int m_BuyCount;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_Partners);
			data.Get(1, ref m_RemainCount);
			data.Get(2, ref m_FinishedCount);
			data.Get(3, ref m_BuyCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_Partners);
			data.ArrayAdd(m_RemainCount);
			data.ArrayAdd(m_FinishedCount);
			data.ArrayAdd(m_BuyCount);
			return data;
		}
	}

	public class Msg_LC_PartnerEquipResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_PartnerId;
		public ulong m_ItemGuid;
		public List<bool> m_PartnerEquipState = new List<bool>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_PartnerId);
			data.Get(2, ref m_ItemGuid);
			JsonMessageUtility.GetSimpleArray(data, 3, ref m_PartnerEquipState);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_PartnerId);
			data.ArrayAdd(m_ItemGuid);
			JsonMessageUtility.AddSimpleArray(data, m_PartnerEquipState);
			return data;
		}
	}

	public class Msg_LC_QueryCorpsByName : IJsonMessage
	{
		public int m_Count;
		public List<Msg_LC_SyncCorpsInfo> m_List = new List<Msg_LC_SyncCorpsInfo>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Count);
			JsonMessageUtility.GetSubDataArray(data, 1, ref m_List);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Count);
			JsonMessageUtility.AddSubDataArray(data, m_List);
			return data;
		}
	}

	public class Msg_LC_QueryFriendInfoResult : IJsonMessage
	{
		public List<FriendInfoForMsg> m_Friends = new List<FriendInfoForMsg>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Friends);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Friends);
			return data;
		}
	}

	public class Msg_LC_QueryTDMatchGroupResult : IJsonMessage
	{
		public int Result;
		public bool IsFull;
		public int LeftFightCount;
		public int QueryCount;
		public int BuyFightCount;
		public List<TDMatchInfoMsg> MatchGroup = new List<TDMatchInfoMsg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			data.Get(1, ref IsFull);
			data.Get(2, ref LeftFightCount);
			data.Get(3, ref QueryCount);
			data.Get(4, ref BuyFightCount);
			JsonMessageUtility.GetSubDataArray(data, 5, ref MatchGroup);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			data.ArrayAdd(IsFull);
			data.ArrayAdd(LeftFightCount);
			data.ArrayAdd(QueryCount);
			data.ArrayAdd(BuyFightCount);
			JsonMessageUtility.AddSubDataArray(data, MatchGroup);
			return data;
		}
	}

	public class Msg_LC_QueueingCountResult : IJsonMessage
	{
		public int m_QueueingCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_QueueingCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_QueueingCount);
			return data;
		}
	}

	public class Msg_LC_RefreshExchangeResult : IJsonMessage
	{
		public int m_RequestRefreshResult;
		public int m_RefreshNum;
		public int m_CurrencyId;
		public int m_NpcId;
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RequestRefreshResult);
			data.Get(1, ref m_RefreshNum);
			data.Get(2, ref m_CurrencyId);
			data.Get(3, ref m_NpcId);
			data.Get(4, ref m_Gold);
			data.Get(5, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RequestRefreshResult);
			data.ArrayAdd(m_RefreshNum);
			data.ArrayAdd(m_CurrencyId);
			data.ArrayAdd(m_NpcId);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_RefreshPartnerCombatResult : IJsonMessage
	{
		public int m_ResultCode;
		public List<int> m_Partners = new List<int>();
		public int m_Gold;
		public int m_Diamond;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_Partners);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_Diamond);
			data.Get(4, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSimpleArray(data, m_Partners);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Diamond);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_RequestDare : IJsonMessage
	{
		public string m_ChallengerNickname;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ChallengerNickname);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ChallengerNickname);
			return data;
		}
	}

	public class Msg_LC_RequestDareResult : IJsonMessage
	{
		public string m_Nickname;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Nickname);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Nickname);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_RequestEnhanceEquipmentStar : IJsonMessage
	{

		public class DeleteItemInfo : IJsonMessage
		{
			public ulong m_ItemGuid;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_ItemGuid);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_ItemGuid);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_Result;
		public int m_ItemID;
		public int m_NewEnhanceLv;
		public List<DeleteItemInfo> m_Items = new List<DeleteItemInfo>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_NewEnhanceLv);
			JsonMessageUtility.GetSubDataArray(data, 3, ref m_Items);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_NewEnhanceLv);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			return data;
		}
	}

	public class Msg_LC_RequestExpeditionResult : IJsonMessage
	{
		public ulong m_Guid;
		public string m_ServerIp;
		public uint m_ServerPort;
		public uint m_Key;
		public int m_CampId;
		public int m_SceneType;
		public int m_ActiveTollgate;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
			data.Get(1, ref m_ServerIp);
			data.Get(2, ref m_ServerPort);
			data.Get(3, ref m_Key);
			data.Get(4, ref m_CampId);
			data.Get(5, ref m_SceneType);
			data.Get(6, ref m_ActiveTollgate);
			data.Get(7, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			data.ArrayAdd(m_ServerIp);
			data.ArrayAdd(m_ServerPort);
			data.ArrayAdd(m_Key);
			data.ArrayAdd(m_CampId);
			data.ArrayAdd(m_SceneType);
			data.ArrayAdd(m_ActiveTollgate);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_RequestGowPrizeResult : IJsonMessage
	{

		public class AwardItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int m_Money;
		public int m_Gold;
		public List<AwardItemInfo> m_Items = new List<AwardItemInfo>();
		public bool m_IsAcquirePrize;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Money);
			data.Get(1, ref m_Gold);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_Items);
			data.Get(3, ref m_IsAcquirePrize);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			JsonMessageUtility.AddSubDataArray(data, m_Items);
			data.ArrayAdd(m_IsAcquirePrize);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_RequestInviteResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_RewardID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_RewardID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_RewardID);
			return data;
		}
	}

	public class Msg_LC_RequestInviteRewardResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_RewardId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_RewardId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_RewardId);
			return data;
		}
	}

	public class Msg_LC_RequestItemUseResult : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_RequestJoinGroupResult : IJsonMessage
	{
		public int m_Result;
		public string m_Nick;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Nick);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Nick);
			return data;
		}
	}

	public class Msg_LC_RequestUserPositionResult : IJsonMessage
	{
		public ulong m_User;
		public float m_X;
		public float m_Z;
		public float m_FaceDir;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_User);
			data.Get(1, ref m_X);
			data.Get(2, ref m_Z);
			data.Get(3, ref m_FaceDir);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_User);
			data.ArrayAdd(m_X);
			data.ArrayAdd(m_Z);
			data.ArrayAdd(m_FaceDir);
			return data;
		}
	}

	public class Msg_LC_RequestUsersResult : IJsonMessage
	{

		public class UserInfo : IJsonMessage
		{
			public ulong m_Guid;
			public int m_HeroId;
			public string m_Nick;
			public float m_X;
			public float m_Z;
			public float m_FaceDir;
			public int m_XSoulItemId;
			public int m_XSoulLevel;
			public int m_XSoulExp;
			public int m_XSoulShowLevel;
			public int m_WingItemId;
			public int m_WingLevel;
			public int m_DressedFashionClothId;
			public int m_DressedFashionWingId;
			public int m_DressedFashionWeaponId;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Guid);
				data.Get(1, ref m_HeroId);
				data.Get(2, ref m_Nick);
				data.Get(3, ref m_X);
				data.Get(4, ref m_Z);
				data.Get(5, ref m_FaceDir);
				data.Get(6, ref m_XSoulItemId);
				data.Get(7, ref m_XSoulLevel);
				data.Get(8, ref m_XSoulExp);
				data.Get(9, ref m_XSoulShowLevel);
				data.Get(10, ref m_WingItemId);
				data.Get(11, ref m_WingLevel);
				data.Get(12, ref m_DressedFashionClothId);
				data.Get(13, ref m_DressedFashionWingId);
				data.Get(14, ref m_DressedFashionWeaponId);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Guid);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Nick);
				data.ArrayAdd(m_X);
				data.ArrayAdd(m_Z);
				data.ArrayAdd(m_FaceDir);
				data.ArrayAdd(m_XSoulItemId);
				data.ArrayAdd(m_XSoulLevel);
				data.ArrayAdd(m_XSoulExp);
				data.ArrayAdd(m_XSoulShowLevel);
				data.ArrayAdd(m_WingItemId);
				data.ArrayAdd(m_WingLevel);
				data.ArrayAdd(m_DressedFashionClothId);
				data.ArrayAdd(m_DressedFashionWingId);
				data.ArrayAdd(m_DressedFashionWeaponId);
				return data;
			}
		}
		public List<UserInfo> m_Users = new List<UserInfo>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Users);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Users);
			return data;
		}
	}

	public class Msg_LC_ResetConsumeGoodsCount : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_ResetCorpsSignIn : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_ResetDailyMissions : IJsonMessage
	{
		public List<MissionInfoForSync> m_Missions = new List<MissionInfoForSync>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Missions);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Missions);
			return data;
		}
	}

	public class Msg_LC_ResetOnlineTimeRewardData : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_ResetWeeklyLoginRewardData : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_RoleEnterResult : IJsonMessage
	{

		public class ChapterAwardMsg : IJsonMessage
		{
			public int ChapterId;
			public int AwardValue;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref ChapterId);
				data.Get(1, ref AwardValue);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(ChapterId);
				data.ArrayAdd(AwardValue);
				return data;
			}
		}

		public class DareData : IJsonMessage
		{
			public int CharpterId;
			public int CurDareCount;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref CharpterId);
				data.Get(1, ref CurDareCount);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(CharpterId);
				data.ArrayAdd(CurDareCount);
				return data;
			}
		}

		public class ExchangeGoodsMsg : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}

		public class ExchangeRefreshMsg : IJsonMessage
		{
			public int m_CurrencyId;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_CurrencyId);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_CurrencyId);
				data.ArrayAdd(m_Num);
				return data;
			}
		}

		public class GoodsPurchasedMsg : IJsonMessage
		{
			public string m_GoodsId;
			public int m_GoodsCount;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_GoodsId);
				data.Get(1, ref m_GoodsCount);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_GoodsId);
				data.ArrayAdd(m_GoodsCount);
				return data;
			}
		}

		public class LotteryInfo : IJsonMessage
		{
			public int m_Id;
			public int m_CurFreeCount;
			public string m_LastDrawTime;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_CurFreeCount);
				data.Get(2, ref m_LastDrawTime);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_CurFreeCount);
				data.ArrayAdd(m_LastDrawTime);
				return data;
			}
		}

		public class MissionInfoForSync : IJsonMessage
		{
			public int m_MissionId;
			public bool m_IsCompleted;
			public string m_Progress;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_MissionId);
				data.Get(1, ref m_IsCompleted);
				data.Get(2, ref m_Progress);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_MissionId);
				data.ArrayAdd(m_IsCompleted);
				data.ArrayAdd(m_Progress);
				return data;
			}
		}

		public class ResetEliteCountInfo : IJsonMessage
		{
			public int SceneId;
			public int ResetCount;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref SceneId);
				data.Get(1, ref ResetCount);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(SceneId);
				data.ArrayAdd(ResetCount);
				return data;
			}
		}

		public class SceneDataMsg : IJsonMessage
		{
			public int m_SceneId;
			public int m_Grade;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_SceneId);
				data.Get(1, ref m_Grade);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_SceneId);
				data.ArrayAdd(m_Grade);
				return data;
			}
		}

		public class ScenesCompletedCountDataMsg : IJsonMessage
		{
			public int m_SceneId;
			public int m_Count;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_SceneId);
				data.Get(1, ref m_Count);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_SceneId);
				data.ArrayAdd(m_Count);
				return data;
			}
		}
		public int m_Result;
		public int m_Money;
		public int m_Gold;
		public int m_GoldCash;
		public int m_Stamina;
		public int m_Exp;
		public int m_Level;
		public int m_CitySceneId;
		public int m_BuyStaminaCount;
		public int m_BuyMoneyCount;
		public int m_CurSellItemGoldIncome;
		public int m_Vip;
		public int m_NewbieGuideScene;
		public GowDataMsg m_Gow;
		public List<int> m_NewbieGuides = new List<int>();
		public List<JsonItemDataMsg> m_BagItems = new List<JsonItemDataMsg>();
		public List<JsonItemDataMsg> m_Equipments = new List<JsonItemDataMsg>();
		public List<SkillDataInfo> m_SkillInfo = new List<SkillDataInfo>();
		public List<MissionInfoForSync> m_Missions = new List<MissionInfoForSync>();
		public List<LegacyDataMsg> m_Legacys = new List<LegacyDataMsg>();
		public List<SceneDataMsg> m_SceneData = new List<SceneDataMsg>();
		public List<ScenesCompletedCountDataMsg> m_SceneCompletedCountData = new List<ScenesCompletedCountDataMsg>();
		public List<FriendInfoForMsg> m_Friends = new List<FriendInfoForMsg>();
		public List<PartnerDataMsg> m_Partners = new List<PartnerDataMsg>();
		public List<int> m_ActivePartners = new List<int>();
		public List<XSoulDataMsg> m_XSouls = new List<XSoulDataMsg>();
		public List<ExchangeGoodsMsg> m_Exchanges = new List<ExchangeGoodsMsg>();
		public int m_WorldId;
		public int m_Vigor;
		public int m_SignInCountCurMonth;
		public int m_RestSignInCountCurDay;
		public List<long> m_NewbieFlag = new List<long>();
		public List<ExchangeRefreshMsg> m_RefreshExchangeNum = new List<ExchangeRefreshMsg>();
		public List<long> m_NewbieActionFlag = new List<long>();
		public bool m_IsGetWeeklyReward;
		public List<int> m_WeeklyRewardRecord = new List<int>();
		public List<int> m_OnlineTimeRewardedIndex = new List<int>();
		public int m_OnlineDuration;
		public long m_GuideFlag;
		public List<JsonTalentDataMsg> m_EquipTalents = new List<JsonTalentDataMsg>();
		public ulong m_CorpsId;
		public List<PaymentRebateDataMsg> m_PaymentRebates = new List<PaymentRebateDataMsg>();
		public List<Msg_LC_SyncMpveInfo> m_MpveInfo = new List<Msg_LC_SyncMpveInfo>();
		public List<GoodsPurchasedMsg> m_GoodsInfo = new List<GoodsPurchasedMsg>();
		public int m_PurchasedDiamonds;
		public List<int> m_RewardedVipLevel = new List<int>();
		public bool m_IsRewardedFirstBuy;
		public List<FashionMsg> m_FashionInfo = new List<FashionMsg>();
		public List<LotteryInfo> m_Lottery = new List<LotteryInfo>();
		public int m_Vitality;
		public bool m_CorpsSignInState;
		public Msg_LC_PartnerCombatInfo m_PartnerCombatInfo;
		public Msg_LC_SyncCorpsInfo m_CorpsInfo;
		public int m_SecretAreaFought;
		public List<int> m_SecretAreaSegments = new List<int>();
		public List<int> m_SecretAreaFightNum = new List<int>();
		public List<int> m_SecretAreaHp = new List<int>();
		public List<int> m_SecretAreaMp = new List<int>();
		public MorrowRewardInfo m_MorrowRewardInfo;
		public FashionHideMsg m_FashionHide;
		public uint m_RecentLoginState;
		public int m_SumLoginDayCount;
		public int m_UsedLoginDrawLotteryCount;
		public string m_UserAccountId;
		public List<ResetEliteCountInfo> m_ResetEliteCount = new List<ResetEliteCountInfo>();
		public List<bool> m_ExtraDiamondBox = new List<bool>();
		public int m_GrowthFundValue;
		public List<DareData> m_CorpsDareData = new List<DareData>();
		public string m_ServerStartTime;
		public List<ChapterAwardMsg> m_ChapterAwardData = new List<ChapterAwardMsg>();
		public List<int> m_SecretAreaPartnerHp = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Money);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_GoldCash);
			data.Get(4, ref m_Stamina);
			data.Get(5, ref m_Exp);
			data.Get(6, ref m_Level);
			data.Get(7, ref m_CitySceneId);
			data.Get(8, ref m_BuyStaminaCount);
			data.Get(9, ref m_BuyMoneyCount);
			data.Get(10, ref m_CurSellItemGoldIncome);
			data.Get(11, ref m_Vip);
			data.Get(12, ref m_NewbieGuideScene);
			JsonMessageUtility.GetSubData(data, 13, out m_Gow);
			JsonMessageUtility.GetSimpleArray(data, 14, ref m_NewbieGuides);
			JsonMessageUtility.GetSubDataArray(data, 15, ref m_BagItems);
			JsonMessageUtility.GetSubDataArray(data, 16, ref m_Equipments);
			JsonMessageUtility.GetSubDataArray(data, 17, ref m_SkillInfo);
			JsonMessageUtility.GetSubDataArray(data, 18, ref m_Missions);
			JsonMessageUtility.GetSubDataArray(data, 19, ref m_Legacys);
			JsonMessageUtility.GetSubDataArray(data, 20, ref m_SceneData);
			JsonMessageUtility.GetSubDataArray(data, 21, ref m_SceneCompletedCountData);
			JsonMessageUtility.GetSubDataArray(data, 22, ref m_Friends);
			JsonMessageUtility.GetSubDataArray(data, 23, ref m_Partners);
			JsonMessageUtility.GetSimpleArray(data, 24, ref m_ActivePartners);
			JsonMessageUtility.GetSubDataArray(data, 25, ref m_XSouls);
			JsonMessageUtility.GetSubDataArray(data, 26, ref m_Exchanges);
			data.Get(27, ref m_WorldId);
			data.Get(28, ref m_Vigor);
			data.Get(29, ref m_SignInCountCurMonth);
			data.Get(30, ref m_RestSignInCountCurDay);
			JsonMessageUtility.GetSimpleArray(data, 31, ref m_NewbieFlag);
			JsonMessageUtility.GetSubDataArray(data, 32, ref m_RefreshExchangeNum);
			JsonMessageUtility.GetSimpleArray(data, 33, ref m_NewbieActionFlag);
			data.Get(34, ref m_IsGetWeeklyReward);
			JsonMessageUtility.GetSimpleArray(data, 35, ref m_WeeklyRewardRecord);
			JsonMessageUtility.GetSimpleArray(data, 36, ref m_OnlineTimeRewardedIndex);
			data.Get(37, ref m_OnlineDuration);
			data.Get(38, ref m_GuideFlag);
			JsonMessageUtility.GetSubDataArray(data, 39, ref m_EquipTalents);
			data.Get(40, ref m_CorpsId);
			JsonMessageUtility.GetSubDataArray(data, 41, ref m_PaymentRebates);
			JsonMessageUtility.GetSubDataArray(data, 42, ref m_MpveInfo);
			JsonMessageUtility.GetSubDataArray(data, 43, ref m_GoodsInfo);
			data.Get(44, ref m_PurchasedDiamonds);
			JsonMessageUtility.GetSimpleArray(data, 45, ref m_RewardedVipLevel);
			data.Get(46, ref m_IsRewardedFirstBuy);
			JsonMessageUtility.GetSubDataArray(data, 47, ref m_FashionInfo);
			JsonMessageUtility.GetSubDataArray(data, 48, ref m_Lottery);
			data.Get(49, ref m_Vitality);
			data.Get(50, ref m_CorpsSignInState);
			JsonMessageUtility.GetSubData(data, 51, out m_PartnerCombatInfo);
			JsonMessageUtility.GetSubData(data, 52, out m_CorpsInfo);
			data.Get(53, ref m_SecretAreaFought);
			JsonMessageUtility.GetSimpleArray(data, 54, ref m_SecretAreaSegments);
			JsonMessageUtility.GetSimpleArray(data, 55, ref m_SecretAreaFightNum);
			JsonMessageUtility.GetSimpleArray(data, 56, ref m_SecretAreaHp);
			JsonMessageUtility.GetSimpleArray(data, 57, ref m_SecretAreaMp);
			JsonMessageUtility.GetSubData(data, 58, out m_MorrowRewardInfo);
			JsonMessageUtility.GetSubData(data, 59, out m_FashionHide);
			data.Get(60, ref m_RecentLoginState);
			data.Get(61, ref m_SumLoginDayCount);
			data.Get(62, ref m_UsedLoginDrawLotteryCount);
			data.Get(63, ref m_UserAccountId);
			JsonMessageUtility.GetSubDataArray(data, 64, ref m_ResetEliteCount);
			JsonMessageUtility.GetSimpleArray(data, 65, ref m_ExtraDiamondBox);
			data.Get(66, ref m_GrowthFundValue);
			JsonMessageUtility.GetSubDataArray(data, 67, ref m_CorpsDareData);
			data.Get(68, ref m_ServerStartTime);
			JsonMessageUtility.GetSubDataArray(data, 69, ref m_ChapterAwardData);
			JsonMessageUtility.GetSimpleArray(data, 70, ref m_SecretAreaPartnerHp);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			data.ArrayAdd(m_Stamina);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_CitySceneId);
			data.ArrayAdd(m_BuyStaminaCount);
			data.ArrayAdd(m_BuyMoneyCount);
			data.ArrayAdd(m_CurSellItemGoldIncome);
			data.ArrayAdd(m_Vip);
			data.ArrayAdd(m_NewbieGuideScene);
			JsonMessageUtility.AddSubData(data, m_Gow);
			JsonMessageUtility.AddSimpleArray(data, m_NewbieGuides);
			JsonMessageUtility.AddSubDataArray(data, m_BagItems);
			JsonMessageUtility.AddSubDataArray(data, m_Equipments);
			JsonMessageUtility.AddSubDataArray(data, m_SkillInfo);
			JsonMessageUtility.AddSubDataArray(data, m_Missions);
			JsonMessageUtility.AddSubDataArray(data, m_Legacys);
			JsonMessageUtility.AddSubDataArray(data, m_SceneData);
			JsonMessageUtility.AddSubDataArray(data, m_SceneCompletedCountData);
			JsonMessageUtility.AddSubDataArray(data, m_Friends);
			JsonMessageUtility.AddSubDataArray(data, m_Partners);
			JsonMessageUtility.AddSimpleArray(data, m_ActivePartners);
			JsonMessageUtility.AddSubDataArray(data, m_XSouls);
			JsonMessageUtility.AddSubDataArray(data, m_Exchanges);
			data.ArrayAdd(m_WorldId);
			data.ArrayAdd(m_Vigor);
			data.ArrayAdd(m_SignInCountCurMonth);
			data.ArrayAdd(m_RestSignInCountCurDay);
			JsonMessageUtility.AddSimpleArray(data, m_NewbieFlag);
			JsonMessageUtility.AddSubDataArray(data, m_RefreshExchangeNum);
			JsonMessageUtility.AddSimpleArray(data, m_NewbieActionFlag);
			data.ArrayAdd(m_IsGetWeeklyReward);
			JsonMessageUtility.AddSimpleArray(data, m_WeeklyRewardRecord);
			JsonMessageUtility.AddSimpleArray(data, m_OnlineTimeRewardedIndex);
			data.ArrayAdd(m_OnlineDuration);
			data.ArrayAdd(m_GuideFlag);
			JsonMessageUtility.AddSubDataArray(data, m_EquipTalents);
			data.ArrayAdd(m_CorpsId);
			JsonMessageUtility.AddSubDataArray(data, m_PaymentRebates);
			JsonMessageUtility.AddSubDataArray(data, m_MpveInfo);
			JsonMessageUtility.AddSubDataArray(data, m_GoodsInfo);
			data.ArrayAdd(m_PurchasedDiamonds);
			JsonMessageUtility.AddSimpleArray(data, m_RewardedVipLevel);
			data.ArrayAdd(m_IsRewardedFirstBuy);
			JsonMessageUtility.AddSubDataArray(data, m_FashionInfo);
			JsonMessageUtility.AddSubDataArray(data, m_Lottery);
			data.ArrayAdd(m_Vitality);
			data.ArrayAdd(m_CorpsSignInState);
			JsonMessageUtility.AddSubData(data, m_PartnerCombatInfo);
			JsonMessageUtility.AddSubData(data, m_CorpsInfo);
			data.ArrayAdd(m_SecretAreaFought);
			JsonMessageUtility.AddSimpleArray(data, m_SecretAreaSegments);
			JsonMessageUtility.AddSimpleArray(data, m_SecretAreaFightNum);
			JsonMessageUtility.AddSimpleArray(data, m_SecretAreaHp);
			JsonMessageUtility.AddSimpleArray(data, m_SecretAreaMp);
			JsonMessageUtility.AddSubData(data, m_MorrowRewardInfo);
			JsonMessageUtility.AddSubData(data, m_FashionHide);
			data.ArrayAdd(m_RecentLoginState);
			data.ArrayAdd(m_SumLoginDayCount);
			data.ArrayAdd(m_UsedLoginDrawLotteryCount);
			data.ArrayAdd(m_UserAccountId);
			JsonMessageUtility.AddSubDataArray(data, m_ResetEliteCount);
			JsonMessageUtility.AddSimpleArray(data, m_ExtraDiamondBox);
			data.ArrayAdd(m_GrowthFundValue);
			JsonMessageUtility.AddSubDataArray(data, m_CorpsDareData);
			data.ArrayAdd(m_ServerStartTime);
			JsonMessageUtility.AddSubDataArray(data, m_ChapterAwardData);
			JsonMessageUtility.AddSimpleArray(data, m_SecretAreaPartnerHp);
			return data;
		}
	}

	public class Msg_LC_RoleListResult : IJsonMessage
	{

		public class UserInfoForMessage : IJsonMessage
		{
			public string m_Nickname;
			public int m_HeroId;
			public int m_Level;
			public ulong m_UserGuid;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Nickname);
				data.Get(1, ref m_HeroId);
				data.Get(2, ref m_Level);
				data.Get(3, ref m_UserGuid);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Nickname);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_UserGuid);
				return data;
			}
		}
		public int m_Result;
		public int m_UserInfoCount;
		public List<UserInfoForMessage> m_UserInfos = new List<UserInfoForMessage>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_UserInfoCount);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_UserInfos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_UserInfoCount);
			JsonMessageUtility.AddSubDataArray(data, m_UserInfos);
			return data;
		}
	}

	public class Msg_LC_SecretAreaTrialAward : IJsonMessage
	{
		public int m_AwardId;
		public bool m_Finish;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_AwardId);
			data.Get(1, ref m_Finish);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_AwardId);
			data.ArrayAdd(m_Finish);
			return data;
		}
	}

	public class Msg_LC_SecretAreaTrialResult : IJsonMessage
	{
		public int m_Difficulty;
		public int m_Result;
		public int m_AlreadyFightNum;
		public int m_Segments;
		public int m_Hp;
		public int m_Mp;
		public int m_AlreadyFought;
		public bool m_Sweept;
		public bool m_Refresh;
		public int m_PartnerHp;
		public int m_Prime;
		public List<int> m_CheckNumbers = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Difficulty);
			data.Get(1, ref m_Result);
			data.Get(2, ref m_AlreadyFightNum);
			data.Get(3, ref m_Segments);
			data.Get(4, ref m_Hp);
			data.Get(5, ref m_Mp);
			data.Get(6, ref m_AlreadyFought);
			data.Get(7, ref m_Sweept);
			data.Get(8, ref m_Refresh);
			data.Get(9, ref m_PartnerHp);
			data.Get(10, ref m_Prime);
			JsonMessageUtility.GetSimpleArray(data, 11, ref m_CheckNumbers);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Difficulty);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_AlreadyFightNum);
			data.ArrayAdd(m_Segments);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_AlreadyFought);
			data.ArrayAdd(m_Sweept);
			data.ArrayAdd(m_Refresh);
			data.ArrayAdd(m_PartnerHp);
			data.ArrayAdd(m_Prime);
			JsonMessageUtility.AddSimpleArray(data, m_CheckNumbers);
			return data;
		}
	}

	public class Msg_LC_SelectPartnerResult : IJsonMessage
	{
		public int m_ResultCode;
		public List<int> m_Partners = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_Partners);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSimpleArray(data, m_Partners);
			return data;
		}
	}

	public class Msg_LC_SellItemResult : IJsonMessage
	{
		public ulong ItemGuid;
		public int ItemNum;
		public int Result;
		public int Diamand;
		public int Money;
		public int TotalIncome;
		public int GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemGuid);
			data.Get(1, ref ItemNum);
			data.Get(2, ref Result);
			data.Get(3, ref Diamand);
			data.Get(4, ref Money);
			data.Get(5, ref TotalIncome);
			data.Get(6, ref GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemGuid);
			data.ArrayAdd(ItemNum);
			data.ArrayAdd(Result);
			data.ArrayAdd(Diamand);
			data.ArrayAdd(Money);
			data.ArrayAdd(TotalIncome);
			data.ArrayAdd(GoldCash);
			return data;
		}
	}

	public class Msg_LC_SendScreenTip : IJsonMessage
	{
		public string m_Content;
		public int m_Align;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
			data.Get(1, ref m_Align);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			data.ArrayAdd(m_Align);
			return data;
		}
	}

	public class Msg_LC_ServerShutdown : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_SetFashionShowResult : IJsonMessage
	{
		public int m_Result;
		public int m_FashionPartType;
		public bool m_IsHide;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_FashionPartType);
			data.Get(2, ref m_IsHide);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_FashionPartType);
			data.ArrayAdd(m_IsHide);
			return data;
		}
	}

	public class Msg_LC_SignInAndGetRewardResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_RewardId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_RewardId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_RewardId);
			return data;
		}
	}

	public class Msg_LC_StageClearResult : IJsonMessage
	{

		public class MissionInfoForSync : IJsonMessage
		{
			public int m_MissionId;
			public bool m_IsCompleted;
			public string m_Progress;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_MissionId);
				data.Get(1, ref m_IsCompleted);
				data.Get(2, ref m_Progress);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_MissionId);
				data.ArrayAdd(m_IsCompleted);
				data.ArrayAdd(m_Progress);
				return data;
			}
		}

		public class Teammate : IJsonMessage
		{
			public string m_Nick;
			public int m_ResId;
			public int m_Money;
			public float m_TotalDamage;
			public int m_ReliveTime;
			public int m_HitCount;
			public int m_Level;
			public bool m_HitCountFit;
			public bool m_TimeFit;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Nick);
				data.Get(1, ref m_ResId);
				data.Get(2, ref m_Money);
				data.Get(3, ref m_TotalDamage);
				data.Get(4, ref m_ReliveTime);
				data.Get(5, ref m_HitCount);
				data.Get(6, ref m_Level);
				data.Get(7, ref m_HitCountFit);
				data.Get(8, ref m_TimeFit);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Nick);
				data.ArrayAdd(m_ResId);
				data.ArrayAdd(m_Money);
				data.ArrayAdd(m_TotalDamage);
				data.ArrayAdd(m_ReliveTime);
				data.ArrayAdd(m_HitCount);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_HitCountFit);
				data.ArrayAdd(m_TimeFit);
				return data;
			}
		}
		public int m_SceneId;
		public int m_HitCount;
		public int m_MaxMultHitCount;
		public long m_Duration;
		public int m_ItemId;
		public int m_ItemCount;
		public int m_ExpPoint;
		public int m_Hp;
		public int m_Mp;
		public int m_Gold;
		public int m_DeadCount;
		public int m_CompletedRewardId;
		public int m_SceneStarNum;
		public List<MissionInfoForSync> m_Missions = new List<MissionInfoForSync>();
		public int m_KillNpcCount;
		public int m_ResultCode;
		public List<Teammate> m_Teammate = new List<Teammate>();
		public int m_Level;
		public int m_BattleResult;
		public int m_StageScore;
		public List<int> m_RewardItemIdList = new List<int>();
		public List<int> m_RewardItemNumList = new List<int>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_HitCount);
			data.Get(2, ref m_MaxMultHitCount);
			data.Get(3, ref m_Duration);
			data.Get(4, ref m_ItemId);
			data.Get(5, ref m_ItemCount);
			data.Get(6, ref m_ExpPoint);
			data.Get(7, ref m_Hp);
			data.Get(8, ref m_Mp);
			data.Get(9, ref m_Gold);
			data.Get(10, ref m_DeadCount);
			data.Get(11, ref m_CompletedRewardId);
			data.Get(12, ref m_SceneStarNum);
			JsonMessageUtility.GetSubDataArray(data, 13, ref m_Missions);
			data.Get(14, ref m_KillNpcCount);
			data.Get(15, ref m_ResultCode);
			JsonMessageUtility.GetSubDataArray(data, 16, ref m_Teammate);
			data.Get(17, ref m_Level);
			data.Get(18, ref m_BattleResult);
			data.Get(19, ref m_StageScore);
			JsonMessageUtility.GetSimpleArray(data, 20, ref m_RewardItemIdList);
			JsonMessageUtility.GetSimpleArray(data, 21, ref m_RewardItemNumList);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_HitCount);
			data.ArrayAdd(m_MaxMultHitCount);
			data.ArrayAdd(m_Duration);
			data.ArrayAdd(m_ItemId);
			data.ArrayAdd(m_ItemCount);
			data.ArrayAdd(m_ExpPoint);
			data.ArrayAdd(m_Hp);
			data.ArrayAdd(m_Mp);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_DeadCount);
			data.ArrayAdd(m_CompletedRewardId);
			data.ArrayAdd(m_SceneStarNum);
			JsonMessageUtility.AddSubDataArray(data, m_Missions);
			data.ArrayAdd(m_KillNpcCount);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSubDataArray(data, m_Teammate);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_BattleResult);
			data.ArrayAdd(m_StageScore);
			JsonMessageUtility.AddSimpleArray(data, m_RewardItemIdList);
			JsonMessageUtility.AddSimpleArray(data, m_RewardItemNumList);
			return data;
		}
	}

	public class Msg_LC_StartGameResult : IJsonMessage
	{
		public string server_ip;
		public uint server_port;
		public uint key;
		public int camp_id;
		public int scene_type;
		public int match_key;
		public int result;
		public int prime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref server_ip);
			data.Get(1, ref server_port);
			data.Get(2, ref key);
			data.Get(3, ref camp_id);
			data.Get(4, ref scene_type);
			data.Get(5, ref match_key);
			data.Get(6, ref result);
			data.Get(7, ref prime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(server_ip);
			data.ArrayAdd(server_port);
			data.ArrayAdd(key);
			data.ArrayAdd(camp_id);
			data.ArrayAdd(scene_type);
			data.ArrayAdd(match_key);
			data.ArrayAdd(result);
			data.ArrayAdd(prime);
			return data;
		}
	}

	public class Msg_LC_StartLootResult : IJsonMessage
	{
		public ulong m_TargetGuid;
		public int m_Sign;
		public int m_ResultCode;
		public int m_Prime;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_TargetGuid);
			data.Get(1, ref m_Sign);
			data.Get(2, ref m_ResultCode);
			data.Get(3, ref m_Prime);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_TargetGuid);
			data.ArrayAdd(m_Sign);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_Prime);
			return data;
		}
	}

	public class Msg_LC_StartPartnerBattleResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_MatchKey;
		public int m_HideAttrKey;
		public int m_Index;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_MatchKey);
			data.Get(2, ref m_HideAttrKey);
			data.Get(3, ref m_Index);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_MatchKey);
			data.ArrayAdd(m_HideAttrKey);
			data.ArrayAdd(m_Index);
			return data;
		}
	}

	public class Msg_LC_StartTDChallengeResult : IJsonMessage
	{
		public int Result;
		public bool IsFull;
		public int LeftFightCount;
		public int QueryCount;
		public int BuyFightCount;
		public int Sign;
		public ArenaInfoMsg TargetInfo;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			data.Get(1, ref IsFull);
			data.Get(2, ref LeftFightCount);
			data.Get(3, ref QueryCount);
			data.Get(4, ref BuyFightCount);
			data.Get(5, ref Sign);
			JsonMessageUtility.GetSubData(data, 6, out TargetInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			data.ArrayAdd(IsFull);
			data.ArrayAdd(LeftFightCount);
			data.ArrayAdd(QueryCount);
			data.ArrayAdd(BuyFightCount);
			data.ArrayAdd(Sign);
			JsonMessageUtility.AddSubData(data, TargetInfo);
			return data;
		}
	}

	public class Msg_LC_SwapSkillResult : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public int m_SourcePos;
		public int m_TargetPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_SourcePos);
			data.Get(3, ref m_TargetPos);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_SourcePos);
			data.ArrayAdd(m_TargetPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_SweepStageResult : IJsonMessage
	{
		public int m_SceneId;
		public int m_ResultCode;
		public List<JsonItemDataMsg> m_ItemInfo = new List<JsonItemDataMsg>();
		public int m_Exp;
		public int m_Gold;
		public int m_SweepItemCost;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SceneId);
			data.Get(1, ref m_ResultCode);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_ItemInfo);
			data.Get(3, ref m_Exp);
			data.Get(4, ref m_Gold);
			data.Get(5, ref m_SweepItemCost);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SceneId);
			data.ArrayAdd(m_ResultCode);
			JsonMessageUtility.AddSubDataArray(data, m_ItemInfo);
			data.ArrayAdd(m_Exp);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_SweepItemCost);
			return data;
		}
	}

	public class Msg_LC_SyncCombatData : IJsonMessage
	{
		public List<LegacyDataMsg> m_Legacys = new List<LegacyDataMsg>();
		public List<SkillDataInfo> m_Skills = new List<SkillDataInfo>();
		public List<XSoulDataMsg> m_XSouls = new List<XSoulDataMsg>();
		public List<JsonItemDataMsg> m_Equipments = new List<JsonItemDataMsg>();
		public List<SelectedPartnerDataMsg> m_PartnerDatas = new List<SelectedPartnerDataMsg>();
		public List<int> m_Fashions = new List<int>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Legacys);
			JsonMessageUtility.GetSubDataArray(data, 1, ref m_Skills);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_XSouls);
			JsonMessageUtility.GetSubDataArray(data, 3, ref m_Equipments);
			JsonMessageUtility.GetSubDataArray(data, 4, ref m_PartnerDatas);
			JsonMessageUtility.GetSimpleArray(data, 5, ref m_Fashions);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Legacys);
			JsonMessageUtility.AddSubDataArray(data, m_Skills);
			JsonMessageUtility.AddSubDataArray(data, m_XSouls);
			JsonMessageUtility.AddSubDataArray(data, m_Equipments);
			JsonMessageUtility.AddSubDataArray(data, m_PartnerDatas);
			JsonMessageUtility.AddSimpleArray(data, m_Fashions);
			return data;
		}
	}

	public class Msg_LC_SyncCorpsDareCount : IJsonMessage
	{

		public class DareData : IJsonMessage
		{
			public int CharpterId;
			public int CurDareCount;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref CharpterId);
				data.Get(1, ref CurDareCount);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(CharpterId);
				data.ArrayAdd(CurDareCount);
				return data;
			}
		}
		public List<DareData> Data = new List<DareData>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref Data);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, Data);
			return data;
		}
	}

	public class Msg_LC_SyncCorpsInfo : IJsonMessage
	{

		public class Charpter : IJsonMessage
		{

			public class BattleInfo : IJsonMessage
			{
				public int SceneId;
				public bool IsFinish;
				public List<int> Monsters = new List<int>();
				public List<int> Hps = new List<int>();
				public bool IsFighting;

				public void FromJson(JsonData data)
				{
					data.Get(0, ref SceneId);
					data.Get(1, ref IsFinish);
					JsonMessageUtility.GetSimpleArray(data, 2, ref Monsters);
					JsonMessageUtility.GetSimpleArray(data, 3, ref Hps);
					data.Get(4, ref IsFighting);
				}
				public JsonData ToJson()
				{
					JsonData data = new JsonData();
					data.SetJsonType(JsonType.Array);
					data.ArrayAdd(SceneId);
					data.ArrayAdd(IsFinish);
					JsonMessageUtility.AddSimpleArray(data, Monsters);
					JsonMessageUtility.AddSimpleArray(data, Hps);
					data.ArrayAdd(IsFighting);
					return data;
				}
			}

			public class TopInfo : IJsonMessage
			{
				public ulong Guid;
				public int HeroId;
				public string Name;
				public int Level;
				public long Damage;

				public void FromJson(JsonData data)
				{
					data.Get(0, ref Guid);
					data.Get(1, ref HeroId);
					data.Get(2, ref Name);
					data.Get(3, ref Level);
					data.Get(4, ref Damage);
				}
				public JsonData ToJson()
				{
					JsonData data = new JsonData();
					data.SetJsonType(JsonType.Array);
					data.ArrayAdd(Guid);
					data.ArrayAdd(HeroId);
					data.ArrayAdd(Name);
					data.ArrayAdd(Level);
					data.ArrayAdd(Damage);
					return data;
				}
			}
			public int Index;
			public bool IsOpen;
			public List<TopInfo> Top = new List<TopInfo>();
			public List<BattleInfo> Tollgates = new List<BattleInfo>();

			public void FromJson(JsonData data)
			{
				data.Get(0, ref Index);
				data.Get(1, ref IsOpen);
				JsonMessageUtility.GetSubDataArray(data, 2, ref Top);
				JsonMessageUtility.GetSubDataArray(data, 3, ref Tollgates);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(Index);
				data.ArrayAdd(IsOpen);
				JsonMessageUtility.AddSubDataArray(data, Top);
				JsonMessageUtility.AddSubDataArray(data, Tollgates);
				return data;
			}
		}

		public class CorpsClaimer : IJsonMessage
		{
			public ulong m_Guid;
			public int m_HeroId;
			public string m_Nickname;
			public int m_Level;
			public int m_FightScore;
			public string m_Time;
			public ulong m_CorpsId;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Guid);
				data.Get(1, ref m_HeroId);
				data.Get(2, ref m_Nickname);
				data.Get(3, ref m_Level);
				data.Get(4, ref m_FightScore);
				data.Get(5, ref m_Time);
				data.Get(6, ref m_CorpsId);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Guid);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Nickname);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_FightScore);
				data.ArrayAdd(m_Time);
				data.ArrayAdd(m_CorpsId);
				return data;
			}
		}

		public class CorpsMember : IJsonMessage
		{
			public ulong m_Guid;
			public int m_HeroId;
			public string m_Nickname;
			public int m_Level;
			public int m_FightScore;
			public int m_Title;
			public int m_DayActiveness;
			public int m_WeekActiveness;
			public long m_LastLoginTime;
			public bool m_IsOnline;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Guid);
				data.Get(1, ref m_HeroId);
				data.Get(2, ref m_Nickname);
				data.Get(3, ref m_Level);
				data.Get(4, ref m_FightScore);
				data.Get(5, ref m_Title);
				data.Get(6, ref m_DayActiveness);
				data.Get(7, ref m_WeekActiveness);
				data.Get(8, ref m_LastLoginTime);
				data.Get(9, ref m_IsOnline);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Guid);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Nickname);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_FightScore);
				data.ArrayAdd(m_Title);
				data.ArrayAdd(m_DayActiveness);
				data.ArrayAdd(m_WeekActiveness);
				data.ArrayAdd(m_LastLoginTime);
				data.ArrayAdd(m_IsOnline);
				return data;
			}
		}
		public ulong m_Guid;
		public string m_Name;
		public int m_Level;
		public int m_Score;
		public int m_Rank;
		public List<CorpsMember> m_Members = new List<CorpsMember>();
		public List<CorpsClaimer> m_Confirms = new List<CorpsClaimer>();
		public string m_Notice;
		public string m_CreateTime;
		public int m_Activeness;
		public List<Charpter> Charpters = new List<Charpter>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
			data.Get(1, ref m_Name);
			data.Get(2, ref m_Level);
			data.Get(3, ref m_Score);
			data.Get(4, ref m_Rank);
			JsonMessageUtility.GetSubDataArray(data, 5, ref m_Members);
			JsonMessageUtility.GetSubDataArray(data, 6, ref m_Confirms);
			data.Get(7, ref m_Notice);
			data.Get(8, ref m_CreateTime);
			data.Get(9, ref m_Activeness);
			JsonMessageUtility.GetSubDataArray(data, 10, ref Charpters);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			data.ArrayAdd(m_Name);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_Score);
			data.ArrayAdd(m_Rank);
			JsonMessageUtility.AddSubDataArray(data, m_Members);
			JsonMessageUtility.AddSubDataArray(data, m_Confirms);
			data.ArrayAdd(m_Notice);
			data.ArrayAdd(m_CreateTime);
			data.ArrayAdd(m_Activeness);
			JsonMessageUtility.AddSubDataArray(data, Charpters);
			return data;
		}
	}

	public class Msg_LC_SyncCorpsOpResult : IJsonMessage
	{
		public int m_Result;
		public string m_Nickname;
		public int m_Gold;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Nickname);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Nickname);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_SyncCorpsStar : IJsonMessage
	{
		public int m_Start;
		public int m_Count;
		public List<Msg_LC_SyncCorpsInfo> m_Star = new List<Msg_LC_SyncCorpsInfo>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Start);
			data.Get(1, ref m_Count);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_Star);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Start);
			data.ArrayAdd(m_Count);
			JsonMessageUtility.AddSubDataArray(data, m_Star);
			return data;
		}
	}

	public class Msg_LC_SyncFightingScore : IJsonMessage
	{
		public ulong Guid;
		public int Score;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref Score);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(Score);
			return data;
		}
	}

	public class Msg_LC_SyncFriendList : IJsonMessage
	{
		public List<FriendInfoForMsg> m_FriendInfo = new List<FriendInfoForMsg>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_FriendInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_FriendInfo);
			return data;
		}
	}

	public class Msg_LC_SyncGoldTollgateInfo : IJsonMessage
	{
		public int m_GoldCurAcceptedCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_GoldCurAcceptedCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_GoldCurAcceptedCount);
			return data;
		}
	}

	public class Msg_LC_SyncGowBattleResult : IJsonMessage
	{
		public int m_Result;
		public int m_OldGowElo;
		public int m_GowElo;
		public int m_OldPoint;
		public int m_Point;
		public int m_OldRankId;
		public int m_RankId;
		public int m_MaxMultiHitCount;
		public int m_TotalDamage;
		public string m_EnemyNick;
		public int m_EnemyHeroId;
		public int m_EnemyOldGowElo;
		public int m_EnemyGowElo;
		public int m_EnemyOldPoint;
		public int m_EnemyPoint;
		public int m_EnemyMaxMultiHitCount;
		public int m_EnemyTotalDamage;
		public int m_SceneType;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_OldGowElo);
			data.Get(2, ref m_GowElo);
			data.Get(3, ref m_OldPoint);
			data.Get(4, ref m_Point);
			data.Get(5, ref m_OldRankId);
			data.Get(6, ref m_RankId);
			data.Get(7, ref m_MaxMultiHitCount);
			data.Get(8, ref m_TotalDamage);
			data.Get(9, ref m_EnemyNick);
			data.Get(10, ref m_EnemyHeroId);
			data.Get(11, ref m_EnemyOldGowElo);
			data.Get(12, ref m_EnemyGowElo);
			data.Get(13, ref m_EnemyOldPoint);
			data.Get(14, ref m_EnemyPoint);
			data.Get(15, ref m_EnemyMaxMultiHitCount);
			data.Get(16, ref m_EnemyTotalDamage);
			data.Get(17, ref m_SceneType);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_OldGowElo);
			data.ArrayAdd(m_GowElo);
			data.ArrayAdd(m_OldPoint);
			data.ArrayAdd(m_Point);
			data.ArrayAdd(m_OldRankId);
			data.ArrayAdd(m_RankId);
			data.ArrayAdd(m_MaxMultiHitCount);
			data.ArrayAdd(m_TotalDamage);
			data.ArrayAdd(m_EnemyNick);
			data.ArrayAdd(m_EnemyHeroId);
			data.ArrayAdd(m_EnemyOldGowElo);
			data.ArrayAdd(m_EnemyGowElo);
			data.ArrayAdd(m_EnemyOldPoint);
			data.ArrayAdd(m_EnemyPoint);
			data.ArrayAdd(m_EnemyMaxMultiHitCount);
			data.ArrayAdd(m_EnemyTotalDamage);
			data.ArrayAdd(m_SceneType);
			return data;
		}
	}

	public class Msg_LC_SyncGowOtherInfo : IJsonMessage
	{
		public int m_WinNum;
		public int m_LoseNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_WinNum);
			data.Get(1, ref m_LoseNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_WinNum);
			data.ArrayAdd(m_LoseNum);
			return data;
		}
	}

	public class Msg_LC_SyncGowRankInfo : IJsonMessage
	{
		public GowDataMsg m_Gow;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubData(data, 0, out m_Gow);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubData(data, m_Gow);
			return data;
		}
	}

	public class Msg_LC_SyncGowStarList : IJsonMessage
	{

		public class GowStarInfoForMessage : IJsonMessage
		{
			public ulong m_Guid;
			public int m_GowElo;
			public string m_Nick;
			public int m_HeroId;
			public int m_Level;
			public int m_FightingScore;
			public int m_RankId;
			public int m_Point;
			public int m_CriticalTotalMatches;
			public int m_CriticalAmassWinMatches;
			public int m_CriticalAmassLossMatches;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Guid);
				data.Get(1, ref m_GowElo);
				data.Get(2, ref m_Nick);
				data.Get(3, ref m_HeroId);
				data.Get(4, ref m_Level);
				data.Get(5, ref m_FightingScore);
				data.Get(6, ref m_RankId);
				data.Get(7, ref m_Point);
				data.Get(8, ref m_CriticalTotalMatches);
				data.Get(9, ref m_CriticalAmassWinMatches);
				data.Get(10, ref m_CriticalAmassLossMatches);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Guid);
				data.ArrayAdd(m_GowElo);
				data.ArrayAdd(m_Nick);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_FightingScore);
				data.ArrayAdd(m_RankId);
				data.ArrayAdd(m_Point);
				data.ArrayAdd(m_CriticalTotalMatches);
				data.ArrayAdd(m_CriticalAmassWinMatches);
				data.ArrayAdd(m_CriticalAmassLossMatches);
				return data;
			}
		}
		public List<GowStarInfoForMessage> m_Stars = new List<GowStarInfoForMessage>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Stars);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Stars);
			return data;
		}
	}

	public class Msg_LC_SyncGroupUsers : IJsonMessage
	{

		public class UserInfoForGroup : IJsonMessage
		{
			public ulong m_Guid;
			public int m_HeroId;
			public string m_Nick;
			public int m_Level;
			public int m_FightingScore;
			public int m_Status;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Guid);
				data.Get(1, ref m_HeroId);
				data.Get(2, ref m_Nick);
				data.Get(3, ref m_Level);
				data.Get(4, ref m_FightingScore);
				data.Get(5, ref m_Status);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Guid);
				data.ArrayAdd(m_HeroId);
				data.ArrayAdd(m_Nick);
				data.ArrayAdd(m_Level);
				data.ArrayAdd(m_FightingScore);
				data.ArrayAdd(m_Status);
				return data;
			}
		}
		public ulong m_Creator;
		public int m_Count;
		public List<UserInfoForGroup> m_Members = new List<UserInfoForGroup>();
		public List<UserInfoForGroup> m_Confirms = new List<UserInfoForGroup>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Creator);
			data.Get(1, ref m_Count);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_Members);
			JsonMessageUtility.GetSubDataArray(data, 3, ref m_Confirms);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Creator);
			data.ArrayAdd(m_Count);
			JsonMessageUtility.AddSubDataArray(data, m_Members);
			JsonMessageUtility.AddSubDataArray(data, m_Confirms);
			return data;
		}
	}

	public class Msg_LC_SyncGuideFlag : IJsonMessage
	{
		public long m_Flag;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Flag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Flag);
			return data;
		}
	}

	public class Msg_LC_SyncHomeNotice : IJsonMessage
	{
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_LC_SyncLeaveGroup : IJsonMessage
	{
		public int m_Result;
		public string m_GroupNick;
		public bool m_NeedTip;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_GroupNick);
			data.Get(2, ref m_NeedTip);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_GroupNick);
			data.ArrayAdd(m_NeedTip);
			return data;
		}
	}

	public class Msg_LC_SyncLootAward : IJsonMessage
	{
		public int ItemId;
		public int ItemNum;
		public int Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemId);
			data.Get(1, ref ItemNum);
			data.Get(2, ref Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemId);
			data.ArrayAdd(ItemNum);
			data.ArrayAdd(Result);
			return data;
		}
	}

	public class Msg_LC_SyncLootHistory : IJsonMessage
	{
		public List<LootHistoryData> History = new List<LootHistoryData>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref History);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, History);
			return data;
		}
	}

	public class Msg_LC_SyncLootInfo : IJsonMessage
	{
		public LootInfoMsg Info;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubData(data, 0, out Info);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubData(data, Info);
			return data;
		}
	}

	public class Msg_LC_SyncLotteryInfo : IJsonMessage
	{
		public int m_Id;
		public int m_CurFreeCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Id);
			data.Get(1, ref m_CurFreeCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Id);
			data.ArrayAdd(m_CurFreeCount);
			return data;
		}
	}

	public class Msg_LC_SyncMailList : IJsonMessage
	{
		public List<MailInfoForMessage> m_Mails = new List<MailInfoForMessage>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Mails);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Mails);
			return data;
		}
	}

	public class Msg_LC_SyncMpveInfo : IJsonMessage
	{
		public int m_Type;
		public List<int> m_Difficulty = new List<int>();
		public int m_DareCount;
		public int m_AwardCount;
		public List<int> m_AwardIndex = new List<int>();
		public List<bool> m_IsGet = new List<bool>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Type);
			JsonMessageUtility.GetSimpleArray(data, 1, ref m_Difficulty);
			data.Get(2, ref m_DareCount);
			data.Get(3, ref m_AwardCount);
			JsonMessageUtility.GetSimpleArray(data, 4, ref m_AwardIndex);
			JsonMessageUtility.GetSimpleArray(data, 5, ref m_IsGet);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Type);
			JsonMessageUtility.AddSimpleArray(data, m_Difficulty);
			data.ArrayAdd(m_DareCount);
			data.ArrayAdd(m_AwardCount);
			JsonMessageUtility.AddSimpleArray(data, m_AwardIndex);
			JsonMessageUtility.AddSimpleArray(data, m_IsGet);
			return data;
		}
	}

	public class Msg_LC_SyncNewbieActionFlag : IJsonMessage
	{
		public List<long> m_NewbieActionFlag = new List<long>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_NewbieActionFlag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_NewbieActionFlag);
			return data;
		}
	}

	public class Msg_LC_SyncNewbieFlag : IJsonMessage
	{
		public List<long> m_NewbieFlag = new List<long>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSimpleArray(data, 0, ref m_NewbieFlag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSimpleArray(data, m_NewbieFlag);
			return data;
		}
	}

	public class Msg_LC_SyncNoticeContent : IJsonMessage
	{
		public string m_Content;
		public int m_RollNum;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
			data.Get(1, ref m_RollNum);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			data.ArrayAdd(m_RollNum);
			return data;
		}
	}

	public class Msg_LC_SyncPinviteTeam : IJsonMessage
	{
		public string m_LeaderNick;
		public string m_Sponsor;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_LeaderNick);
			data.Get(1, ref m_Sponsor);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_LeaderNick);
			data.ArrayAdd(m_Sponsor);
			return data;
		}
	}

	public class Msg_LC_SyncPlayerDetail : IJsonMessage
	{
		public string m_Nick;
		public int m_Level;
		public int m_Score;
		public int m_CorpsId;
		public int m_VipLv;
		public List<JsonItemDataMsg> EquipInfo = new List<JsonItemDataMsg>();
		public List<PartnerDataMsg> Partners = new List<PartnerDataMsg>();
		public List<JsonTalentDataMsg> Talents = new List<JsonTalentDataMsg>();
		public int m_HeroId;
		public List<FashionMsg> FashonInfo = new List<FashionMsg>();
		public FashionHideMsg FashionHide;
		public int m_PartnerScore;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Nick);
			data.Get(1, ref m_Level);
			data.Get(2, ref m_Score);
			data.Get(3, ref m_CorpsId);
			data.Get(4, ref m_VipLv);
			JsonMessageUtility.GetSubDataArray(data, 5, ref EquipInfo);
			JsonMessageUtility.GetSubDataArray(data, 6, ref Partners);
			JsonMessageUtility.GetSubDataArray(data, 7, ref Talents);
			data.Get(8, ref m_HeroId);
			JsonMessageUtility.GetSubDataArray(data, 9, ref FashonInfo);
			JsonMessageUtility.GetSubData(data, 10, out FashionHide);
			data.Get(11, ref m_PartnerScore);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Nick);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_Score);
			data.ArrayAdd(m_CorpsId);
			data.ArrayAdd(m_VipLv);
			JsonMessageUtility.AddSubDataArray(data, EquipInfo);
			JsonMessageUtility.AddSubDataArray(data, Partners);
			JsonMessageUtility.AddSubDataArray(data, Talents);
			data.ArrayAdd(m_HeroId);
			JsonMessageUtility.AddSubDataArray(data, FashonInfo);
			JsonMessageUtility.AddSubData(data, FashionHide);
			data.ArrayAdd(m_PartnerScore);
			return data;
		}
	}

	public class Msg_LC_SyncPlayerInfo : IJsonMessage
	{
		public string m_Nick;
		public int m_Level;
		public int m_Score;
		public ulong m_CorpsId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Nick);
			data.Get(1, ref m_Level);
			data.Get(2, ref m_Score);
			data.Get(3, ref m_CorpsId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Nick);
			data.ArrayAdd(m_Level);
			data.ArrayAdd(m_Score);
			data.ArrayAdd(m_CorpsId);
			return data;
		}
	}

	public class Msg_LC_SyncQuitRoom : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_SyncRecentLoginState : IJsonMessage
	{
		public uint m_RecentLoginState;
		public int m_SumLoginDayCount;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_RecentLoginState);
			data.Get(1, ref m_SumLoginDayCount);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_RecentLoginState);
			data.ArrayAdd(m_SumLoginDayCount);
			return data;
		}
	}

	public class Msg_LC_SyncResetGowPrize : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_LC_SyncSignInCount : IJsonMessage
	{
		public int m_SignInCountCurMonth;
		public int m_RestSignInCountCurDay;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_SignInCountCurMonth);
			data.Get(1, ref m_RestSignInCountCurDay);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_SignInCountCurMonth);
			data.ArrayAdd(m_RestSignInCountCurDay);
			return data;
		}
	}

	public class Msg_LC_SyncSkillInfos : IJsonMessage
	{
		public List<SkillDataInfo> m_Skills = new List<SkillDataInfo>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref m_Skills);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, m_Skills);
			return data;
		}
	}

	public class Msg_LC_SyncStamina : IJsonMessage
	{
		public int m_Stamina;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Stamina);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Stamina);
			return data;
		}
	}

	public class Msg_LC_SyncValidCorpsList : IJsonMessage
	{
		public int m_Start;
		public int m_Count;
		public List<Msg_LC_SyncCorpsInfo> m_List = new List<Msg_LC_SyncCorpsInfo>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Start);
			data.Get(1, ref m_Count);
			JsonMessageUtility.GetSubDataArray(data, 2, ref m_List);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Start);
			data.ArrayAdd(m_Count);
			JsonMessageUtility.AddSubDataArray(data, m_List);
			return data;
		}
	}

	public class Msg_LC_SyncVigor : IJsonMessage
	{
		public int m_Vigor;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Vigor);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Vigor);
			return data;
		}
	}

	public class Msg_LC_SyncVitality : IJsonMessage
	{
		public int m_Vitality;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Vitality);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Vitality);
			return data;
		}
	}

	public class Msg_LC_SystemChatWorldResult : IJsonMessage
	{
		public string m_Content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Content);
			return data;
		}
	}

	public class Msg_LC_TDChallengeResult : IJsonMessage
	{
		public int Result;
		public ChallengeInfoData ChallengeInfo;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Result);
			JsonMessageUtility.GetSubData(data, 1, out ChallengeInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Result);
			JsonMessageUtility.AddSubData(data, ChallengeInfo);
			return data;
		}
	}

	public class Msg_LC_UnlockLegacyResult : IJsonMessage
	{
		public int m_Index;
		public int m_ItemID;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Index);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Index);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_UnlockSkillResult : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public int m_UserLevel;
		public int m_SlotPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_UserLevel);
			data.Get(3, ref m_SlotPos);
			data.Get(4, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_UserLevel);
			data.ArrayAdd(m_SlotPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_UnmountEquipmentResult : IJsonMessage
	{
		public int m_EquipPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_EquipPos);
			data.Get(1, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_EquipPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_UnmountFashionResult : IJsonMessage
	{
		public int m_Result;
		public int m_FashionID;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_FashionID);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_FashionID);
			return data;
		}
	}

	public class Msg_LC_UnmountSkillResult : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SlotPos;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SlotPos);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SlotPos);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_UpdateInviteInfo : IJsonMessage
	{
		public int m_InviteeCount;
		public int m_OverLv30Count;
		public int m_OverLv50Count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_InviteeCount);
			data.Get(1, ref m_OverLv30Count);
			data.Get(2, ref m_OverLv50Count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_InviteeCount);
			data.ArrayAdd(m_OverLv30Count);
			data.ArrayAdd(m_OverLv50Count);
			return data;
		}
	}

	public class Msg_LC_UpgradeEquipBatch : IJsonMessage
	{
		public int m_Result;
		public int m_Money;
		public int m_Gold;
		public List<ulong> m_Guids = new List<ulong>();
		public List<int> m_Level = new List<int>();
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_Money);
			data.Get(2, ref m_Gold);
			JsonMessageUtility.GetSimpleArray(data, 3, ref m_Guids);
			JsonMessageUtility.GetSimpleArray(data, 4, ref m_Level);
			data.Get(5, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			JsonMessageUtility.AddSimpleArray(data, m_Guids);
			JsonMessageUtility.AddSimpleArray(data, m_Level);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_UpgradeItemResult : IJsonMessage
	{
		public int m_Position;
		public int m_Money;
		public int m_Gold;
		public int m_Result;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Position);
			data.Get(1, ref m_Money);
			data.Get(2, ref m_Gold);
			data.Get(3, ref m_Result);
			data.Get(4, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Position);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_UpgradeLegacyResult : IJsonMessage
	{
		public int m_Index;
		public int m_ItemID;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Index);
			data.Get(1, ref m_ItemID);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Index);
			data.ArrayAdd(m_ItemID);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class Msg_LC_UpgradeParnerStageResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_PartnerId;
		public int m_CurStage;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_PartnerId);
			data.Get(2, ref m_CurStage);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_PartnerId);
			data.ArrayAdd(m_CurStage);
			return data;
		}
	}

	public class Msg_LC_UpgradePartnerLevelResult : IJsonMessage
	{
		public int m_ResultCode;
		public int m_PartnerId;
		public int m_CurLevel;
		public List<bool> m_PartnerEquipState = new List<bool>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
			data.Get(1, ref m_PartnerId);
			data.Get(2, ref m_CurLevel);
			JsonMessageUtility.GetSimpleArray(data, 3, ref m_PartnerEquipState);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			data.ArrayAdd(m_PartnerId);
			data.ArrayAdd(m_CurLevel);
			JsonMessageUtility.AddSimpleArray(data, m_PartnerEquipState);
			return data;
		}
	}

	public class Msg_LC_UpgradeSkillResult : IJsonMessage
	{
		public int m_PresetIndex;
		public int m_SkillID;
		public bool m_AllowCostGold;
		public int m_Money;
		public int m_Gold;
		public int m_Vigor;
		public int m_Result;
		public int m_GoldCash;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_PresetIndex);
			data.Get(1, ref m_SkillID);
			data.Get(2, ref m_AllowCostGold);
			data.Get(3, ref m_Money);
			data.Get(4, ref m_Gold);
			data.Get(5, ref m_Vigor);
			data.Get(6, ref m_Result);
			data.Get(7, ref m_GoldCash);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_PresetIndex);
			data.ArrayAdd(m_SkillID);
			data.ArrayAdd(m_AllowCostGold);
			data.ArrayAdd(m_Money);
			data.ArrayAdd(m_Gold);
			data.ArrayAdd(m_Vigor);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_GoldCash);
			return data;
		}
	}

	public class Msg_LC_UserLevelup : IJsonMessage
	{
		public int m_UserId;
		public int m_UserLevel;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_UserId);
			data.Get(1, ref m_UserLevel);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_UserId);
			data.ArrayAdd(m_UserLevel);
			return data;
		}
	}

	public class Msg_LC_WeeklyLoginRewardResult : IJsonMessage
	{
		public int m_ResultCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_ResultCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_ResultCode);
			return data;
		}
	}

	public class Msg_LC_XSoulChangeShowModelResult : IJsonMessage
	{
		public int m_XSoulPart;
		public int m_ModelLevel;
		public int m_Result;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_XSoulPart);
			data.Get(1, ref m_ModelLevel);
			data.Get(2, ref m_Result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_XSoulPart);
			data.ArrayAdd(m_ModelLevel);
			data.ArrayAdd(m_Result);
			return data;
		}
	}

	public class NodeMessageWithAccount : IJsonMessage
	{
		public string m_Account;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Account);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Account);
			return data;
		}
	}

	public class NodeMessageWithAccountAndGuid : IJsonMessage
	{
		public string m_Account;
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Account);
			data.Get(1, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Account);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class NodeMessageWithAccountAndLogicServerId : IJsonMessage
	{
		public string m_Account;
		public int m_LogicServerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Account);
			data.Get(1, ref m_LogicServerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Account);
			data.ArrayAdd(m_LogicServerId);
			return data;
		}
	}

	public class NodeMessageWithGuid : IJsonMessage
	{
		public ulong m_Guid;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Guid);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Guid);
			return data;
		}
	}

	public class NodeMessageWithLogicServerId : IJsonMessage
	{
		public int m_LogicServerId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_LogicServerId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_LogicServerId);
			return data;
		}
	}

	public class NodeRegister : IJsonMessage
	{
		public string m_Name;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Name);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Name);
			return data;
		}
	}

	public class NodeRegisterResult : IJsonMessage
	{
		public bool m_IsOk;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_IsOk);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_IsOk);
			return data;
		}
	}

	public class PartnerDataMsg : IJsonMessage
	{
		public int Id;
		public int AdditionLevel;
		public int SkillStage;
		public List<bool> EquipState = new List<bool>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Id);
			data.Get(1, ref AdditionLevel);
			data.Get(2, ref SkillStage);
			JsonMessageUtility.GetSimpleArray(data, 3, ref EquipState);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Id);
			data.ArrayAdd(AdditionLevel);
			data.ArrayAdd(SkillStage);
			JsonMessageUtility.AddSimpleArray(data, EquipState);
			return data;
		}
	}

	public class PaymentRebateDataMsg : IJsonMessage
	{

		public class AwardItemInfo : IJsonMessage
		{
			public int m_Id;
			public int m_Num;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref m_Id);
				data.Get(1, ref m_Num);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(m_Id);
				data.ArrayAdd(m_Num);
				return data;
			}
		}
		public int Id;
		public int Group;
		public string Describe;
		public string AnnounceTime;
		public string StartTime;
		public string EndTime;
		public int TotalDiamond;
		public int Diamond;
		public int Gold;
		public int Exp;
		public List<AwardItemInfo> AwardItems = new List<AwardItemInfo>();
		public int DiamondAlready;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Id);
			data.Get(1, ref Group);
			data.Get(2, ref Describe);
			data.Get(3, ref AnnounceTime);
			data.Get(4, ref StartTime);
			data.Get(5, ref EndTime);
			data.Get(6, ref TotalDiamond);
			data.Get(7, ref Diamond);
			data.Get(8, ref Gold);
			data.Get(9, ref Exp);
			JsonMessageUtility.GetSubDataArray(data, 10, ref AwardItems);
			data.Get(11, ref DiamondAlready);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Id);
			data.ArrayAdd(Group);
			data.ArrayAdd(Describe);
			data.ArrayAdd(AnnounceTime);
			data.ArrayAdd(StartTime);
			data.ArrayAdd(EndTime);
			data.ArrayAdd(TotalDiamond);
			data.ArrayAdd(Diamond);
			data.ArrayAdd(Gold);
			data.ArrayAdd(Exp);
			JsonMessageUtility.AddSubDataArray(data, AwardItems);
			data.ArrayAdd(DiamondAlready);
			return data;
		}
	}

	public class SelectedPartnerDataMsg : IJsonMessage
	{
		public int m_Id;
		public int m_AdditionLevel;
		public int m_SkillStage;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Id);
			data.Get(1, ref m_AdditionLevel);
			data.Get(2, ref m_SkillStage);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Id);
			data.ArrayAdd(m_AdditionLevel);
			data.ArrayAdd(m_SkillStage);
			return data;
		}
	}

	public class SkillDataInfo : IJsonMessage
	{
		public int ID;
		public int Level;
		public int Postions;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ID);
			data.Get(1, ref Level);
			data.Get(2, ref Postions);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ID);
			data.ArrayAdd(Level);
			data.ArrayAdd(Postions);
			return data;
		}
	}

	public class TDMatchInfoMsg : IJsonMessage
	{
		public ulong Guid;
		public string NickName;
		public int Level;
		public int FightScore;
		public int HeroId;
		public int DropId;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref Guid);
			data.Get(1, ref NickName);
			data.Get(2, ref Level);
			data.Get(3, ref FightScore);
			data.Get(4, ref HeroId);
			data.Get(5, ref DropId);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(Guid);
			data.ArrayAdd(NickName);
			data.ArrayAdd(Level);
			data.ArrayAdd(FightScore);
			data.ArrayAdd(HeroId);
			data.ArrayAdd(DropId);
			return data;
		}
	}

	public class TooManyOperations : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class UserHeartbeat : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class VersionVerify : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class VersionVerifyResult : IJsonMessage
	{
		public int m_Result;
		public int m_EnableLog;
		public uint m_ShopMask;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_Result);
			data.Get(1, ref m_EnableLog);
			data.Get(2, ref m_ShopMask);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_Result);
			data.ArrayAdd(m_EnableLog);
			data.ArrayAdd(m_ShopMask);
			return data;
		}
	}

	public class XSoulDataMsg : IJsonMessage
	{
		public int ItemId;
		public int Level;
		public int ModelLevel;
		public int Experience;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ItemId);
			data.Get(1, ref Level);
			data.Get(2, ref ModelLevel);
			data.Get(3, ref Experience);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ItemId);
			data.ArrayAdd(Level);
			data.ArrayAdd(ModelLevel);
			data.ArrayAdd(Experience);
			return data;
		}
	}
}
