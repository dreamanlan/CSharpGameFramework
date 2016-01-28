//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using GameFrameworkData;
using GameFramework;

namespace GameFramework
{
	public sealed class TableAccountWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public int LogicServerId
		{
			get{return m_TableAccount.LogicServerId;}
			set
			{
				m_TableAccount.LogicServerId = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string AccountId
		{
			get{return m_TableAccount.AccountId;}
			set
			{
				m_TableAccount.AccountId = value;
				OnPrimaryKeyUpdated();
			}
		}
		public bool IsBanned
		{
			get{return m_TableAccount.IsBanned;}
			set
			{
				m_TableAccount.IsBanned = value;
				OnFieldUpdated();
			}
		}
		public ulong UserGuid1
		{
			get{return m_TableAccount.UserGuid1;}
			set
			{
				m_TableAccount.UserGuid1 = value;
				OnFieldUpdated();
			}
		}
		public ulong UserGuid2
		{
			get{return m_TableAccount.UserGuid2;}
			set
			{
				m_TableAccount.UserGuid2 = value;
				OnFieldUpdated();
			}
		}
		public ulong UserGuid3
		{
			get{return m_TableAccount.UserGuid3;}
			set
			{
				m_TableAccount.UserGuid3 = value;
				OnFieldUpdated();
			}
		}

		public TableAccount ToProto()
		{
			return m_TableAccount;
		}
		public void FromProto(TableAccount proto)
		{
			m_TableAccount = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableAccount.LogicServerId.ToString());
			m_PrimaryKeys.Add(m_TableAccount.AccountId.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableAccount m_TableAccount = new TableAccount();

	}
}

namespace GameFramework
{
	public sealed class TableActivationCodeWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string ActivationCode
		{
			get{return m_TableActivationCode.ActivationCode;}
			set
			{
				m_TableActivationCode.ActivationCode = value;
				OnPrimaryKeyUpdated();
			}
		}
		public bool IsActivated
		{
			get{return m_TableActivationCode.IsActivated;}
			set
			{
				m_TableActivationCode.IsActivated = value;
				OnFieldUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableActivationCode.LogicServerId;}
			set
			{
				m_TableActivationCode.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public string AccountId
		{
			get{return m_TableActivationCode.AccountId;}
			set
			{
				m_TableActivationCode.AccountId = value;
				OnFieldUpdated();
			}
		}

		public TableActivationCode ToProto()
		{
			return m_TableActivationCode;
		}
		public void FromProto(TableActivationCode proto)
		{
			m_TableActivationCode = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableActivationCode.ActivationCode.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableActivationCode m_TableActivationCode = new TableActivationCode();

	}
}

namespace GameFramework
{
	public sealed class TableArenaInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long UserGuid
		{
			get{return m_TableArenaInfo.UserGuid;}
			set
			{
				m_TableArenaInfo.UserGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableArenaInfo.LogicServerId;}
			set
			{
				m_TableArenaInfo.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public int Rank
		{
			get{return m_TableArenaInfo.Rank;}
			set
			{
				m_TableArenaInfo.Rank = value;
				OnFieldUpdated();
			}
		}
		public bool IsRobot
		{
			get{return m_TableArenaInfo.IsRobot;}
			set
			{
				m_TableArenaInfo.IsRobot = value;
				OnFieldUpdated();
			}
		}
		public byte[] ArenaBytes
		{
			get{return m_TableArenaInfo.ArenaBytes;}
			set
			{
				m_TableArenaInfo.ArenaBytes = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastBattleTime
		{
			get{return m_LastBattleTime;}
			set
			{
				m_LastBattleTime = value;
				OnFieldUpdated();
			}
		}
		public int LeftFightCount
		{
			get{return m_TableArenaInfo.LeftFightCount;}
			set
			{
				m_TableArenaInfo.LeftFightCount = value;
				OnFieldUpdated();
			}
		}
		public int BuyFightCount
		{
			get{return m_TableArenaInfo.BuyFightCount;}
			set
			{
				m_TableArenaInfo.BuyFightCount = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetFightCountTime
		{
			get{return m_LastResetFightCountTime;}
			set
			{
				m_LastResetFightCountTime = value;
				OnFieldUpdated();
			}
		}
		public string ArenaHistroyTimeList
		{
			get{return m_TableArenaInfo.ArenaHistroyTimeList;}
			set
			{
				m_TableArenaInfo.ArenaHistroyTimeList = value;
				OnFieldUpdated();
			}
		}
		public int GetArenaHistroyRankListCount()
		{
		  return m_ArenaHistroyRankList.Count;
		}
		public void SetArenaHistroyRankList(int ix, int val)
		{
		  m_ArenaHistroyRankList[ix]=val;
		  OnFieldUpdated();
		}
		public int GetArenaHistroyRankList(int ix)
		{
		  return m_ArenaHistroyRankList[ix];
		}
		public void AddArenaHistroyRankList(int val)
		{
		  m_ArenaHistroyRankList.Add(val);
		  OnFieldUpdated();
		}
		public void DelArenaHistroyRankList(int ix)
		{
		  m_ArenaHistroyRankList.RemoveAt(ix);
		  OnFieldUpdated();
		}
		public void Visit(MyAction<int> visit)
		{
		  foreach(int v in m_ArenaHistroyRankList){
		    visit(v);
		  }
		}
		public int[] ToArray()
		{
		  return m_ArenaHistroyRankList.ToArray();
		}

		public TableArenaInfo ToProto()
		{
			m_TableArenaInfo.LastBattleTime = m_LastBattleTime.ToString("yyyyMMddHHmmss");
			m_TableArenaInfo.LastResetFightCountTime = m_LastResetFightCountTime.ToString("yyyyMMddHHmmss");
			m_TableArenaInfo.ArenaHistroyRankList = DataProtoUtility.JoinNumericList(",",m_ArenaHistroyRankList);
			return m_TableArenaInfo;
		}
		public void FromProto(TableArenaInfo proto)
		{
			m_TableArenaInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastBattleTime = DateTime.ParseExact(m_TableArenaInfo.LastBattleTime,"yyyyMMddHHmmss",null);
			m_LastResetFightCountTime = DateTime.ParseExact(m_TableArenaInfo.LastResetFightCountTime,"yyyyMMddHHmmss",null);
			m_ArenaHistroyRankList = DataProtoUtility.SplitNumericList<int>(new char[]{','}, m_TableArenaInfo.ArenaHistroyRankList);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableArenaInfo.UserGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableArenaInfo m_TableArenaInfo = new TableArenaInfo();
		private DateTime m_LastBattleTime = new DateTime();
		private DateTime m_LastResetFightCountTime = new DateTime();
		private List<int> m_ArenaHistroyRankList = new List<int>();

	}
}

namespace GameFramework
{
	public sealed class TableArenaRecordWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableArenaRecord.Guid;}
			set
			{
				m_TableArenaRecord.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableArenaRecord.UserGuid;}
			set
			{
				m_TableArenaRecord.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int Rank
		{
			get{return m_TableArenaRecord.Rank;}
			set
			{
				m_TableArenaRecord.Rank = value;
				OnFieldUpdated();
			}
		}
		public bool IsChallengerSuccess
		{
			get{return m_TableArenaRecord.IsChallengerSuccess;}
			set
			{
				m_TableArenaRecord.IsChallengerSuccess = value;
				OnFieldUpdated();
			}
		}
		public DateTime BeginTime
		{
			get{return m_BeginTime;}
			set
			{
				m_BeginTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime EndTime
		{
			get{return m_EndTime;}
			set
			{
				m_EndTime = value;
				OnFieldUpdated();
			}
		}
		public long CGuid
		{
			get{return m_TableArenaRecord.CGuid;}
			set
			{
				m_TableArenaRecord.CGuid = value;
				OnFieldUpdated();
			}
		}
		public int CHeroId
		{
			get{return m_TableArenaRecord.CHeroId;}
			set
			{
				m_TableArenaRecord.CHeroId = value;
				OnFieldUpdated();
			}
		}
		public int CLevel
		{
			get{return m_TableArenaRecord.CLevel;}
			set
			{
				m_TableArenaRecord.CLevel = value;
				OnFieldUpdated();
			}
		}
		public int CFightScore
		{
			get{return m_TableArenaRecord.CFightScore;}
			set
			{
				m_TableArenaRecord.CFightScore = value;
				OnFieldUpdated();
			}
		}
		public string CNickname
		{
			get{return m_TableArenaRecord.CNickname;}
			set
			{
				m_TableArenaRecord.CNickname = value;
				OnFieldUpdated();
			}
		}
		public int CRank
		{
			get{return m_TableArenaRecord.CRank;}
			set
			{
				m_TableArenaRecord.CRank = value;
				OnFieldUpdated();
			}
		}
		public int CUserDamage
		{
			get{return m_TableArenaRecord.CUserDamage;}
			set
			{
				m_TableArenaRecord.CUserDamage = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerId1
		{
			get{return m_TableArenaRecord.CPartnerId1;}
			set
			{
				m_TableArenaRecord.CPartnerId1 = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerDamage1
		{
			get{return m_TableArenaRecord.CPartnerDamage1;}
			set
			{
				m_TableArenaRecord.CPartnerDamage1 = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerId2
		{
			get{return m_TableArenaRecord.CPartnerId2;}
			set
			{
				m_TableArenaRecord.CPartnerId2 = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerDamage2
		{
			get{return m_TableArenaRecord.CPartnerDamage2;}
			set
			{
				m_TableArenaRecord.CPartnerDamage2 = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerId3
		{
			get{return m_TableArenaRecord.CPartnerId3;}
			set
			{
				m_TableArenaRecord.CPartnerId3 = value;
				OnFieldUpdated();
			}
		}
		public int CPartnerDamage3
		{
			get{return m_TableArenaRecord.CPartnerDamage3;}
			set
			{
				m_TableArenaRecord.CPartnerDamage3 = value;
				OnFieldUpdated();
			}
		}
		public long TGuid
		{
			get{return m_TableArenaRecord.TGuid;}
			set
			{
				m_TableArenaRecord.TGuid = value;
				OnFieldUpdated();
			}
		}
		public int THeroId
		{
			get{return m_TableArenaRecord.THeroId;}
			set
			{
				m_TableArenaRecord.THeroId = value;
				OnFieldUpdated();
			}
		}
		public int TLevel
		{
			get{return m_TableArenaRecord.TLevel;}
			set
			{
				m_TableArenaRecord.TLevel = value;
				OnFieldUpdated();
			}
		}
		public int TFightScore
		{
			get{return m_TableArenaRecord.TFightScore;}
			set
			{
				m_TableArenaRecord.TFightScore = value;
				OnFieldUpdated();
			}
		}
		public string TNickname
		{
			get{return m_TableArenaRecord.TNickname;}
			set
			{
				m_TableArenaRecord.TNickname = value;
				OnFieldUpdated();
			}
		}
		public int TRank
		{
			get{return m_TableArenaRecord.TRank;}
			set
			{
				m_TableArenaRecord.TRank = value;
				OnFieldUpdated();
			}
		}
		public int TUserDamage
		{
			get{return m_TableArenaRecord.TUserDamage;}
			set
			{
				m_TableArenaRecord.TUserDamage = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerId1
		{
			get{return m_TableArenaRecord.TPartnerId1;}
			set
			{
				m_TableArenaRecord.TPartnerId1 = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerDamage1
		{
			get{return m_TableArenaRecord.TPartnerDamage1;}
			set
			{
				m_TableArenaRecord.TPartnerDamage1 = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerId2
		{
			get{return m_TableArenaRecord.TPartnerId2;}
			set
			{
				m_TableArenaRecord.TPartnerId2 = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerDamage2
		{
			get{return m_TableArenaRecord.TPartnerDamage2;}
			set
			{
				m_TableArenaRecord.TPartnerDamage2 = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerId3
		{
			get{return m_TableArenaRecord.TPartnerId3;}
			set
			{
				m_TableArenaRecord.TPartnerId3 = value;
				OnFieldUpdated();
			}
		}
		public int TPartnerDamage3
		{
			get{return m_TableArenaRecord.TPartnerDamage3;}
			set
			{
				m_TableArenaRecord.TPartnerDamage3 = value;
				OnFieldUpdated();
			}
		}

		public TableArenaRecord ToProto()
		{
			m_TableArenaRecord.BeginTime = m_BeginTime.ToString("yyyyMMddHHmmss");
			m_TableArenaRecord.EndTime = m_EndTime.ToString("yyyyMMddHHmmss");
			return m_TableArenaRecord;
		}
		public void FromProto(TableArenaRecord proto)
		{
			m_TableArenaRecord = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_BeginTime = DateTime.ParseExact(m_TableArenaRecord.BeginTime,"yyyyMMddHHmmss",null);
			m_EndTime = DateTime.ParseExact(m_TableArenaRecord.EndTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableArenaRecord.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableArenaRecord.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableArenaRecord m_TableArenaRecord = new TableArenaRecord();
		private DateTime m_BeginTime = new DateTime();
		private DateTime m_EndTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableCorpsInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long CorpsGuid
		{
			get{return m_TableCorpsInfo.CorpsGuid;}
			set
			{
				m_TableCorpsInfo.CorpsGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableCorpsInfo.LogicServerId;}
			set
			{
				m_TableCorpsInfo.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public string CorpsName
		{
			get{return m_TableCorpsInfo.CorpsName;}
			set
			{
				m_TableCorpsInfo.CorpsName = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableCorpsInfo.Level;}
			set
			{
				m_TableCorpsInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int Score
		{
			get{return m_TableCorpsInfo.Score;}
			set
			{
				m_TableCorpsInfo.Score = value;
				OnFieldUpdated();
			}
		}
		public int Rank
		{
			get{return m_TableCorpsInfo.Rank;}
			set
			{
				m_TableCorpsInfo.Rank = value;
				OnFieldUpdated();
			}
		}
		public int Activeness
		{
			get{return m_TableCorpsInfo.Activeness;}
			set
			{
				m_TableCorpsInfo.Activeness = value;
				OnFieldUpdated();
			}
		}
		public string Notice
		{
			get{return m_TableCorpsInfo.Notice;}
			set
			{
				m_TableCorpsInfo.Notice = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetActivenessTime
		{
			get{return m_LastResetActivenessTime;}
			set
			{
				m_LastResetActivenessTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime CreateTime
		{
			get{return m_CreateTime;}
			set
			{
				m_CreateTime = value;
				OnFieldUpdated();
			}
		}
		public byte[] TollgateBytes
		{
			get{return m_TableCorpsInfo.TollgateBytes;}
			set
			{
				m_TableCorpsInfo.TollgateBytes = value;
				OnFieldUpdated();
			}
		}

		public TableCorpsInfo ToProto()
		{
			m_TableCorpsInfo.LastResetActivenessTime = m_LastResetActivenessTime.ToString("yyyyMMddHHmmss");
			m_TableCorpsInfo.CreateTime = m_CreateTime.ToString("yyyyMMddHHmmss");
			return m_TableCorpsInfo;
		}
		public void FromProto(TableCorpsInfo proto)
		{
			m_TableCorpsInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastResetActivenessTime = DateTime.ParseExact(m_TableCorpsInfo.LastResetActivenessTime,"yyyyMMddHHmmss",null);
			m_CreateTime = DateTime.ParseExact(m_TableCorpsInfo.CreateTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableCorpsInfo.CorpsGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableCorpsInfo m_TableCorpsInfo = new TableCorpsInfo();
		private DateTime m_LastResetActivenessTime = new DateTime();
		private DateTime m_CreateTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableCorpsMemberWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long UserGuid
		{
			get{return m_TableCorpsMember.UserGuid;}
			set
			{
				m_TableCorpsMember.UserGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long CorpsGuid
		{
			get{return m_TableCorpsMember.CorpsGuid;}
			set
			{
				m_TableCorpsMember.CorpsGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int Title
		{
			get{return m_TableCorpsMember.Title;}
			set
			{
				m_TableCorpsMember.Title = value;
				OnFieldUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableCorpsMember.Nickname;}
			set
			{
				m_TableCorpsMember.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableCorpsMember.HeroId;}
			set
			{
				m_TableCorpsMember.HeroId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableCorpsMember.Level;}
			set
			{
				m_TableCorpsMember.Level = value;
				OnFieldUpdated();
			}
		}
		public int FightScore
		{
			get{return m_TableCorpsMember.FightScore;}
			set
			{
				m_TableCorpsMember.FightScore = value;
				OnFieldUpdated();
			}
		}
		public int DayActiveness
		{
			get{return m_TableCorpsMember.DayActiveness;}
			set
			{
				m_TableCorpsMember.DayActiveness = value;
				OnFieldUpdated();
			}
		}
		public int WeekActiveness
		{
			get{return m_TableCorpsMember.WeekActiveness;}
			set
			{
				m_TableCorpsMember.WeekActiveness = value;
				OnFieldUpdated();
			}
		}
		public DateTime ActivenessHistoryDate
		{
			get{return m_ActivenessHistoryDate;}
			set
			{
				m_ActivenessHistoryDate = value;
				OnFieldUpdated();
			}
		}
		public string ActivenessHistoryValue
		{
			get{return m_TableCorpsMember.ActivenessHistoryValue;}
			set
			{
				m_TableCorpsMember.ActivenessHistoryValue = value;
				OnFieldUpdated();
			}
		}
		public long LastLoginTime
		{
			get{return m_TableCorpsMember.LastLoginTime;}
			set
			{
				m_TableCorpsMember.LastLoginTime = value;
				OnFieldUpdated();
			}
		}

		public TableCorpsMember ToProto()
		{
			m_TableCorpsMember.ActivenessHistoryDate = m_ActivenessHistoryDate.ToString("yyyyMMddHHmmss");
			return m_TableCorpsMember;
		}
		public void FromProto(TableCorpsMember proto)
		{
			m_TableCorpsMember = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_ActivenessHistoryDate = DateTime.ParseExact(m_TableCorpsMember.ActivenessHistoryDate,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableCorpsMember.UserGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableCorpsMember.CorpsGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableCorpsMember m_TableCorpsMember = new TableCorpsMember();
		private DateTime m_ActivenessHistoryDate = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableEquipInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableEquipInfo.Guid;}
			set
			{
				m_TableEquipInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableEquipInfo.UserGuid;}
			set
			{
				m_TableEquipInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public long ItemGuid
		{
			get{return m_TableEquipInfo.ItemGuid;}
			set
			{
				m_TableEquipInfo.ItemGuid = value;
				OnFieldUpdated();
			}
		}
		public int Position
		{
			get{return m_TableEquipInfo.Position;}
			set
			{
				m_TableEquipInfo.Position = value;
				OnFieldUpdated();
			}
		}
		public int ItemId
		{
			get{return m_TableEquipInfo.ItemId;}
			set
			{
				m_TableEquipInfo.ItemId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableEquipInfo.Level;}
			set
			{
				m_TableEquipInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int AppendProperty
		{
			get{return m_TableEquipInfo.AppendProperty;}
			set
			{
				m_TableEquipInfo.AppendProperty = value;
				OnFieldUpdated();
			}
		}
		public int EnhanceStarLevel
		{
			get{return m_TableEquipInfo.EnhanceStarLevel;}
			set
			{
				m_TableEquipInfo.EnhanceStarLevel = value;
				OnFieldUpdated();
			}
		}
		public int StrengthLevel
		{
			get{return m_TableEquipInfo.StrengthLevel;}
			set
			{
				m_TableEquipInfo.StrengthLevel = value;
				OnFieldUpdated();
			}
		}
		public int FailCount
		{
			get{return m_TableEquipInfo.FailCount;}
			set
			{
				m_TableEquipInfo.FailCount = value;
				OnFieldUpdated();
			}
		}

		public TableEquipInfo ToProto()
		{
			return m_TableEquipInfo;
		}
		public void FromProto(TableEquipInfo proto)
		{
			m_TableEquipInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableEquipInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableEquipInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableEquipInfo m_TableEquipInfo = new TableEquipInfo();

	}
}

namespace GameFramework
{
	public sealed class TableExpeditionInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableExpeditionInfo.Guid;}
			set
			{
				m_TableExpeditionInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableExpeditionInfo.UserGuid;}
			set
			{
				m_TableExpeditionInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int FightingScore
		{
			get{return m_TableExpeditionInfo.FightingScore;}
			set
			{
				m_TableExpeditionInfo.FightingScore = value;
				OnFieldUpdated();
			}
		}
		public int HP
		{
			get{return m_TableExpeditionInfo.HP;}
			set
			{
				m_TableExpeditionInfo.HP = value;
				OnFieldUpdated();
			}
		}
		public int MP
		{
			get{return m_TableExpeditionInfo.MP;}
			set
			{
				m_TableExpeditionInfo.MP = value;
				OnFieldUpdated();
			}
		}
		public int Rage
		{
			get{return m_TableExpeditionInfo.Rage;}
			set
			{
				m_TableExpeditionInfo.Rage = value;
				OnFieldUpdated();
			}
		}
		public int Schedule
		{
			get{return m_TableExpeditionInfo.Schedule;}
			set
			{
				m_TableExpeditionInfo.Schedule = value;
				OnFieldUpdated();
			}
		}
		public int MonsterCount
		{
			get{return m_TableExpeditionInfo.MonsterCount;}
			set
			{
				m_TableExpeditionInfo.MonsterCount = value;
				OnFieldUpdated();
			}
		}
		public int BossCount
		{
			get{return m_TableExpeditionInfo.BossCount;}
			set
			{
				m_TableExpeditionInfo.BossCount = value;
				OnFieldUpdated();
			}
		}
		public int OnePlayerCount
		{
			get{return m_TableExpeditionInfo.OnePlayerCount;}
			set
			{
				m_TableExpeditionInfo.OnePlayerCount = value;
				OnFieldUpdated();
			}
		}
		public string Unrewarded
		{
			get{return m_TableExpeditionInfo.Unrewarded;}
			set
			{
				m_TableExpeditionInfo.Unrewarded = value;
				OnFieldUpdated();
			}
		}
		public int TollgateType
		{
			get{return m_TableExpeditionInfo.TollgateType;}
			set
			{
				m_TableExpeditionInfo.TollgateType = value;
				OnFieldUpdated();
			}
		}
		public string EnemyList
		{
			get{return m_TableExpeditionInfo.EnemyList;}
			set
			{
				m_TableExpeditionInfo.EnemyList = value;
				OnFieldUpdated();
			}
		}
		public string EnemyAttrList
		{
			get{return m_TableExpeditionInfo.EnemyAttrList;}
			set
			{
				m_TableExpeditionInfo.EnemyAttrList = value;
				OnFieldUpdated();
			}
		}
		public byte[] ImageA
		{
			get{return m_TableExpeditionInfo.ImageA;}
			set
			{
				m_TableExpeditionInfo.ImageA = value;
				OnFieldUpdated();
			}
		}
		public byte[] ImageB
		{
			get{return m_TableExpeditionInfo.ImageB;}
			set
			{
				m_TableExpeditionInfo.ImageB = value;
				OnFieldUpdated();
			}
		}
		public int ResetCount
		{
			get{return m_TableExpeditionInfo.ResetCount;}
			set
			{
				m_TableExpeditionInfo.ResetCount = value;
				OnFieldUpdated();
			}
		}
		public string PartnerIdList
		{
			get{return m_TableExpeditionInfo.PartnerIdList;}
			set
			{
				m_TableExpeditionInfo.PartnerIdList = value;
				OnFieldUpdated();
			}
		}
		public string PartnerHpList
		{
			get{return m_TableExpeditionInfo.PartnerHpList;}
			set
			{
				m_TableExpeditionInfo.PartnerHpList = value;
				OnFieldUpdated();
			}
		}
		public int LastAchievedSchedule
		{
			get{return m_TableExpeditionInfo.LastAchievedSchedule;}
			set
			{
				m_TableExpeditionInfo.LastAchievedSchedule = value;
				OnFieldUpdated();
			}
		}

		public TableExpeditionInfo ToProto()
		{
			return m_TableExpeditionInfo;
		}
		public void FromProto(TableExpeditionInfo proto)
		{
			m_TableExpeditionInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableExpeditionInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableExpeditionInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableExpeditionInfo m_TableExpeditionInfo = new TableExpeditionInfo();

	}
}

namespace GameFramework
{
	public sealed class TableFashionInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableFashionInfo.Guid;}
			set
			{
				m_TableFashionInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableFashionInfo.UserGuid;}
			set
			{
				m_TableFashionInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public long ItemGuid
		{
			get{return m_TableFashionInfo.ItemGuid;}
			set
			{
				m_TableFashionInfo.ItemGuid = value;
				OnFieldUpdated();
			}
		}
		public int Position
		{
			get{return m_TableFashionInfo.Position;}
			set
			{
				m_TableFashionInfo.Position = value;
				OnFieldUpdated();
			}
		}
		public int ItemId
		{
			get{return m_TableFashionInfo.ItemId;}
			set
			{
				m_TableFashionInfo.ItemId = value;
				OnFieldUpdated();
			}
		}
		public bool IsForever
		{
			get{return m_TableFashionInfo.IsForever;}
			set
			{
				m_TableFashionInfo.IsForever = value;
				OnFieldUpdated();
			}
		}
		public DateTime ExpirationTime
		{
			get{return m_ExpirationTime;}
			set
			{
				m_ExpirationTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastNoticeTime
		{
			get{return m_LastNoticeTime;}
			set
			{
				m_LastNoticeTime = value;
				OnFieldUpdated();
			}
		}

		public TableFashionInfo ToProto()
		{
			m_TableFashionInfo.ExpirationTime = m_ExpirationTime.ToString("yyyyMMddHHmmss");
			m_TableFashionInfo.LastNoticeTime = m_LastNoticeTime.ToString("yyyyMMddHHmmss");
			return m_TableFashionInfo;
		}
		public void FromProto(TableFashionInfo proto)
		{
			m_TableFashionInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_ExpirationTime = DateTime.ParseExact(m_TableFashionInfo.ExpirationTime,"yyyyMMddHHmmss",null);
			m_LastNoticeTime = DateTime.ParseExact(m_TableFashionInfo.LastNoticeTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableFashionInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableFashionInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableFashionInfo m_TableFashionInfo = new TableFashionInfo();
		private DateTime m_ExpirationTime = new DateTime();
		private DateTime m_LastNoticeTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableFightInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableFightInfo.Guid;}
			set
			{
				m_TableFightInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableFightInfo.LogicServerId;}
			set
			{
				m_TableFightInfo.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public int Rank
		{
			get{return m_TableFightInfo.Rank;}
			set
			{
				m_TableFightInfo.Rank = value;
				OnFieldUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableFightInfo.UserGuid;}
			set
			{
				m_TableFightInfo.UserGuid = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableFightInfo.HeroId;}
			set
			{
				m_TableFightInfo.HeroId = value;
				OnFieldUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableFightInfo.Nickname;}
			set
			{
				m_TableFightInfo.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableFightInfo.Level;}
			set
			{
				m_TableFightInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int FightingScore
		{
			get{return m_TableFightInfo.FightingScore;}
			set
			{
				m_TableFightInfo.FightingScore = value;
				OnFieldUpdated();
			}
		}

		public TableFightInfo ToProto()
		{
			return m_TableFightInfo;
		}
		public void FromProto(TableFightInfo proto)
		{
			m_TableFightInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableFightInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableFightInfo m_TableFightInfo = new TableFightInfo();

	}
}

namespace GameFramework
{
	public sealed class TableFriendInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableFriendInfo.Guid;}
			set
			{
				m_TableFriendInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableFriendInfo.UserGuid;}
			set
			{
				m_TableFriendInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public long FriendGuid
		{
			get{return m_TableFriendInfo.FriendGuid;}
			set
			{
				m_TableFriendInfo.FriendGuid = value;
				OnFieldUpdated();
			}
		}
		public string FriendNickname
		{
			get{return m_TableFriendInfo.FriendNickname;}
			set
			{
				m_TableFriendInfo.FriendNickname = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableFriendInfo.HeroId;}
			set
			{
				m_TableFriendInfo.HeroId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableFriendInfo.Level;}
			set
			{
				m_TableFriendInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int FightingScore
		{
			get{return m_TableFriendInfo.FightingScore;}
			set
			{
				m_TableFriendInfo.FightingScore = value;
				OnFieldUpdated();
			}
		}
		public bool IsBlack
		{
			get{return m_TableFriendInfo.IsBlack;}
			set
			{
				m_TableFriendInfo.IsBlack = value;
				OnFieldUpdated();
			}
		}

		public TableFriendInfo ToProto()
		{
			return m_TableFriendInfo;
		}
		public void FromProto(TableFriendInfo proto)
		{
			m_TableFriendInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableFriendInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableFriendInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableFriendInfo m_TableFriendInfo = new TableFriendInfo();

	}
}

namespace GameFramework
{
	public sealed class TableGlobalParamWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string ParamType
		{
			get{return m_TableGlobalParam.ParamType;}
			set
			{
				m_TableGlobalParam.ParamType = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string ParamValue
		{
			get{return m_TableGlobalParam.ParamValue;}
			set
			{
				m_TableGlobalParam.ParamValue = value;
				OnFieldUpdated();
			}
		}

		public TableGlobalParam ToProto()
		{
			return m_TableGlobalParam;
		}
		public void FromProto(TableGlobalParam proto)
		{
			m_TableGlobalParam = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableGlobalParam.ParamType.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGlobalParam m_TableGlobalParam = new TableGlobalParam();

	}
}

namespace GameFramework
{
	public sealed class TableGowInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableGowInfo.Guid;}
			set
			{
				m_TableGowInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableGowInfo.UserGuid;}
			set
			{
				m_TableGowInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int GowElo
		{
			get{return m_TableGowInfo.GowElo;}
			set
			{
				m_TableGowInfo.GowElo = value;
				OnFieldUpdated();
			}
		}
		public int GowMatches
		{
			get{return m_TableGowInfo.GowMatches;}
			set
			{
				m_TableGowInfo.GowMatches = value;
				OnFieldUpdated();
			}
		}
		public int GowWinMatches
		{
			get{return m_TableGowInfo.GowWinMatches;}
			set
			{
				m_TableGowInfo.GowWinMatches = value;
				OnFieldUpdated();
			}
		}
		public string GowHistroyTimeList
		{
			get{return m_TableGowInfo.GowHistroyTimeList;}
			set
			{
				m_TableGowInfo.GowHistroyTimeList = value;
				OnFieldUpdated();
			}
		}
		public string GowHistroyEloList
		{
			get{return m_TableGowInfo.GowHistroyEloList;}
			set
			{
				m_TableGowInfo.GowHistroyEloList = value;
				OnFieldUpdated();
			}
		}
		public int RankId
		{
			get{return m_TableGowInfo.RankId;}
			set
			{
				m_TableGowInfo.RankId = value;
				OnFieldUpdated();
			}
		}
		public int Point
		{
			get{return m_TableGowInfo.Point;}
			set
			{
				m_TableGowInfo.Point = value;
				OnFieldUpdated();
			}
		}
		public int CriticalMatchCount
		{
			get{return m_TableGowInfo.CriticalMatchCount;}
			set
			{
				m_TableGowInfo.CriticalMatchCount = value;
				OnFieldUpdated();
			}
		}
		public int CriticalWinMatchCount
		{
			get{return m_TableGowInfo.CriticalWinMatchCount;}
			set
			{
				m_TableGowInfo.CriticalWinMatchCount = value;
				OnFieldUpdated();
			}
		}
		public bool IsAcquirePrize
		{
			get{return m_TableGowInfo.IsAcquirePrize;}
			set
			{
				m_TableGowInfo.IsAcquirePrize = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastTourneyDate
		{
			get{return m_LastTourneyDate;}
			set
			{
				m_LastTourneyDate = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetGowPrizeTime
		{
			get{return m_LastResetGowPrizeTime;}
			set
			{
				m_LastResetGowPrizeTime = value;
				OnFieldUpdated();
			}
		}

		public TableGowInfo ToProto()
		{
			m_TableGowInfo.LastTourneyDate = m_LastTourneyDate.ToString("yyyyMMddHHmmss");
			m_TableGowInfo.LastResetGowPrizeTime = m_LastResetGowPrizeTime.ToString("yyyyMMddHHmmss");
			return m_TableGowInfo;
		}
		public void FromProto(TableGowInfo proto)
		{
			m_TableGowInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastTourneyDate = DateTime.ParseExact(m_TableGowInfo.LastTourneyDate,"yyyyMMddHHmmss",null);
			m_LastResetGowPrizeTime = DateTime.ParseExact(m_TableGowInfo.LastResetGowPrizeTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableGowInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableGowInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGowInfo m_TableGowInfo = new TableGowInfo();
		private DateTime m_LastTourneyDate = new DateTime();
		private DateTime m_LastResetGowPrizeTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableGowStarWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableGowStar.Guid;}
			set
			{
				m_TableGowStar.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableGowStar.LogicServerId;}
			set
			{
				m_TableGowStar.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public int Rank
		{
			get{return m_TableGowStar.Rank;}
			set
			{
				m_TableGowStar.Rank = value;
				OnFieldUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableGowStar.UserGuid;}
			set
			{
				m_TableGowStar.UserGuid = value;
				OnFieldUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableGowStar.Nickname;}
			set
			{
				m_TableGowStar.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableGowStar.HeroId;}
			set
			{
				m_TableGowStar.HeroId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableGowStar.Level;}
			set
			{
				m_TableGowStar.Level = value;
				OnFieldUpdated();
			}
		}
		public int FightingScore
		{
			get{return m_TableGowStar.FightingScore;}
			set
			{
				m_TableGowStar.FightingScore = value;
				OnFieldUpdated();
			}
		}
		public int GowElo
		{
			get{return m_TableGowStar.GowElo;}
			set
			{
				m_TableGowStar.GowElo = value;
				OnFieldUpdated();
			}
		}
		public int RankId
		{
			get{return m_TableGowStar.RankId;}
			set
			{
				m_TableGowStar.RankId = value;
				OnFieldUpdated();
			}
		}
		public int Point
		{
			get{return m_TableGowStar.Point;}
			set
			{
				m_TableGowStar.Point = value;
				OnFieldUpdated();
			}
		}

		public TableGowStar ToProto()
		{
			return m_TableGowStar;
		}
		public void FromProto(TableGowStar proto)
		{
			m_TableGowStar = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableGowStar.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGowStar m_TableGowStar = new TableGowStar();

	}
}

namespace GameFramework
{
	public sealed class TableGuidWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string GuidType
		{
			get{return m_TableGuid.GuidType;}
			set
			{
				m_TableGuid.GuidType = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong GuidValue
		{
			get{return m_TableGuid.GuidValue;}
			set
			{
				m_TableGuid.GuidValue = value;
				OnFieldUpdated();
			}
		}

		public TableGuid ToProto()
		{
			return m_TableGuid;
		}
		public void FromProto(TableGuid proto)
		{
			m_TableGuid = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableGuid.GuidType.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGuid m_TableGuid = new TableGuid();

	}
}

namespace GameFramework
{
	public sealed class TableHomeNoticeWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public int LogicServerId
		{
			get{return m_TableHomeNotice.LogicServerId;}
			set
			{
				m_TableHomeNotice.LogicServerId = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string Content
		{
			get{return m_TableHomeNotice.Content;}
			set
			{
				m_TableHomeNotice.Content = value;
				OnFieldUpdated();
			}
		}
		public DateTime CreateTime
		{
			get{return m_CreateTime;}
			set
			{
				m_CreateTime = value;
				OnFieldUpdated();
			}
		}

		public TableHomeNotice ToProto()
		{
			m_TableHomeNotice.CreateTime = m_CreateTime.ToString("yyyyMMddHHmmss");
			return m_TableHomeNotice;
		}
		public void FromProto(TableHomeNotice proto)
		{
			m_TableHomeNotice = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_CreateTime = DateTime.ParseExact(m_TableHomeNotice.CreateTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableHomeNotice.LogicServerId.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableHomeNotice m_TableHomeNotice = new TableHomeNotice();
		private DateTime m_CreateTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableInviterInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long UserGuid
		{
			get{return m_TableInviterInfo.UserGuid;}
			set
			{
				m_TableInviterInfo.UserGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string InviteCode
		{
			get{return m_TableInviterInfo.InviteCode;}
			set
			{
				m_TableInviterInfo.InviteCode = value;
				OnFieldUpdated();
			}
		}
		public long InviterGuid
		{
			get{return m_TableInviterInfo.InviterGuid;}
			set
			{
				m_TableInviterInfo.InviterGuid = value;
				OnFieldUpdated();
			}
		}
		public int InviterLevel
		{
			get{return m_TableInviterInfo.InviterLevel;}
			set
			{
				m_TableInviterInfo.InviterLevel = value;
				OnFieldUpdated();
			}
		}
		public string RewardedList
		{
			get{return m_TableInviterInfo.RewardedList;}
			set
			{
				m_TableInviterInfo.RewardedList = value;
				OnFieldUpdated();
			}
		}
		public string InviteeGuidList
		{
			get{return m_TableInviterInfo.InviteeGuidList;}
			set
			{
				m_TableInviterInfo.InviteeGuidList = value;
				OnFieldUpdated();
			}
		}
		public string InviteeLevelList
		{
			get{return m_TableInviterInfo.InviteeLevelList;}
			set
			{
				m_TableInviterInfo.InviteeLevelList = value;
				OnFieldUpdated();
			}
		}

		public TableInviterInfo ToProto()
		{
			return m_TableInviterInfo;
		}
		public void FromProto(TableInviterInfo proto)
		{
			m_TableInviterInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableInviterInfo.UserGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableInviterInfo m_TableInviterInfo = new TableInviterInfo();

	}
}

namespace GameFramework
{
	public sealed class TableItemInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public ulong ItemGuid
		{
			get{return m_TableItemInfo.ItemGuid;}
			set
			{
				m_TableItemInfo.ItemGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableItemInfo.UserGuid;}
			set
			{
				m_TableItemInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int ItemId
		{
			get{return m_TableItemInfo.ItemId;}
			set
			{
				m_TableItemInfo.ItemId = value;
				OnFieldUpdated();
			}
		}
		public int ItemNum
		{
			get{return m_TableItemInfo.ItemNum;}
			set
			{
				m_TableItemInfo.ItemNum = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableItemInfo.Level;}
			set
			{
				m_TableItemInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int Experience
		{
			get{return m_TableItemInfo.Experience;}
			set
			{
				m_TableItemInfo.Experience = value;
				OnFieldUpdated();
			}
		}
		public int AppendProperty
		{
			get{return m_TableItemInfo.AppendProperty;}
			set
			{
				m_TableItemInfo.AppendProperty = value;
				OnFieldUpdated();
			}
		}
		public int EnhanceStarLevel
		{
			get{return m_TableItemInfo.EnhanceStarLevel;}
			set
			{
				m_TableItemInfo.EnhanceStarLevel = value;
				OnFieldUpdated();
			}
		}

		public TableItemInfo ToProto()
		{
			return m_TableItemInfo;
		}
		public void FromProto(TableItemInfo proto)
		{
			m_TableItemInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableItemInfo.ItemGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableItemInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableItemInfo m_TableItemInfo = new TableItemInfo();

	}
}

namespace GameFramework
{
	public sealed class TableLegacyInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableLegacyInfo.Guid;}
			set
			{
				m_TableLegacyInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableLegacyInfo.UserGuid;}
			set
			{
				m_TableLegacyInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int Position
		{
			get{return m_TableLegacyInfo.Position;}
			set
			{
				m_TableLegacyInfo.Position = value;
				OnFieldUpdated();
			}
		}
		public int LegacyId
		{
			get{return m_TableLegacyInfo.LegacyId;}
			set
			{
				m_TableLegacyInfo.LegacyId = value;
				OnFieldUpdated();
			}
		}
		public int LegacyLevel
		{
			get{return m_TableLegacyInfo.LegacyLevel;}
			set
			{
				m_TableLegacyInfo.LegacyLevel = value;
				OnFieldUpdated();
			}
		}
		public int AppendProperty
		{
			get{return m_TableLegacyInfo.AppendProperty;}
			set
			{
				m_TableLegacyInfo.AppendProperty = value;
				OnFieldUpdated();
			}
		}
		public bool IsUnlock
		{
			get{return m_TableLegacyInfo.IsUnlock;}
			set
			{
				m_TableLegacyInfo.IsUnlock = value;
				OnFieldUpdated();
			}
		}

		public TableLegacyInfo ToProto()
		{
			return m_TableLegacyInfo;
		}
		public void FromProto(TableLegacyInfo proto)
		{
			m_TableLegacyInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLegacyInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableLegacyInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLegacyInfo m_TableLegacyInfo = new TableLegacyInfo();

	}
}

namespace GameFramework
{
	public sealed class TableLevelInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableLevelInfo.Guid;}
			set
			{
				m_TableLevelInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableLevelInfo.UserGuid;}
			set
			{
				m_TableLevelInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int LevelId
		{
			get{return m_TableLevelInfo.LevelId;}
			set
			{
				m_TableLevelInfo.LevelId = value;
				OnFieldUpdated();
			}
		}
		public int LevelRecord
		{
			get{return m_TableLevelInfo.LevelRecord;}
			set
			{
				m_TableLevelInfo.LevelRecord = value;
				OnFieldUpdated();
			}
		}
		public int ResetEliteCount
		{
			get{return m_TableLevelInfo.ResetEliteCount;}
			set
			{
				m_TableLevelInfo.ResetEliteCount = value;
				OnFieldUpdated();
			}
		}
		public byte[] SceneDataBytes
		{
			get{return m_TableLevelInfo.SceneDataBytes;}
			set
			{
				m_TableLevelInfo.SceneDataBytes = value;
				OnFieldUpdated();
			}
		}

		public TableLevelInfo ToProto()
		{
			return m_TableLevelInfo;
		}
		public void FromProto(TableLevelInfo proto)
		{
			m_TableLevelInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLevelInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableLevelInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLevelInfo m_TableLevelInfo = new TableLevelInfo();

	}
}

namespace GameFramework
{
	public sealed class TableLoginLotteryRecordWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long RecordId
		{
			get{return m_TableLoginLotteryRecord.RecordId;}
			set
			{
				m_TableLoginLotteryRecord.RecordId = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string AccountId
		{
			get{return m_TableLoginLotteryRecord.AccountId;}
			set
			{
				m_TableLoginLotteryRecord.AccountId = value;
				OnFieldUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableLoginLotteryRecord.UserGuid;}
			set
			{
				m_TableLoginLotteryRecord.UserGuid = value;
				OnFieldUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableLoginLotteryRecord.Nickname;}
			set
			{
				m_TableLoginLotteryRecord.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int RewardId
		{
			get{return m_TableLoginLotteryRecord.RewardId;}
			set
			{
				m_TableLoginLotteryRecord.RewardId = value;
				OnFieldUpdated();
			}
		}
		public DateTime CreateTime
		{
			get{return m_CreateTime;}
			set
			{
				m_CreateTime = value;
				OnFieldUpdated();
			}
		}

		public TableLoginLotteryRecord ToProto()
		{
			m_TableLoginLotteryRecord.CreateTime = m_CreateTime.ToString("yyyyMMddHHmmss");
			return m_TableLoginLotteryRecord;
		}
		public void FromProto(TableLoginLotteryRecord proto)
		{
			m_TableLoginLotteryRecord = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_CreateTime = DateTime.ParseExact(m_TableLoginLotteryRecord.CreateTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLoginLotteryRecord.RecordId.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLoginLotteryRecord m_TableLoginLotteryRecord = new TableLoginLotteryRecord();
		private DateTime m_CreateTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableLootInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long UserGuid
		{
			get{return m_TableLootInfo.UserGuid;}
			set
			{
				m_TableLootInfo.UserGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public bool IsPool
		{
			get{return m_TableLootInfo.IsPool;}
			set
			{
				m_TableLootInfo.IsPool = value;
				OnFieldUpdated();
			}
		}
		public bool IsVisible
		{
			get{return m_TableLootInfo.IsVisible;}
			set
			{
				m_TableLootInfo.IsVisible = value;
				OnFieldUpdated();
			}
		}
		public long LootKey
		{
			get{return m_TableLootInfo.LootKey;}
			set
			{
				m_TableLootInfo.LootKey = value;
				OnFieldUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableLootInfo.Nickname;}
			set
			{
				m_TableLootInfo.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableLootInfo.HeroId;}
			set
			{
				m_TableLootInfo.HeroId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableLootInfo.Level;}
			set
			{
				m_TableLootInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int FightScore
		{
			get{return m_TableLootInfo.FightScore;}
			set
			{
				m_TableLootInfo.FightScore = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastBattleTime
		{
			get{return m_LastBattleTime;}
			set
			{
				m_LastBattleTime = value;
				OnFieldUpdated();
			}
		}
		public int DomainType
		{
			get{return m_TableLootInfo.DomainType;}
			set
			{
				m_TableLootInfo.DomainType = value;
				OnFieldUpdated();
			}
		}
		public DateTime BeginTime
		{
			get{return m_BeginTime;}
			set
			{
				m_BeginTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsOpen
		{
			get{return m_TableLootInfo.IsOpen;}
			set
			{
				m_TableLootInfo.IsOpen = value;
				OnFieldUpdated();
			}
		}
		public bool IsGetAward
		{
			get{return m_TableLootInfo.IsGetAward;}
			set
			{
				m_TableLootInfo.IsGetAward = value;
				OnFieldUpdated();
			}
		}
		public float LootAward
		{
			get{return m_TableLootInfo.LootAward;}
			set
			{
				m_TableLootInfo.LootAward = value;
				OnFieldUpdated();
			}
		}
		public long TargetLootKey
		{
			get{return m_TableLootInfo.TargetLootKey;}
			set
			{
				m_TableLootInfo.TargetLootKey = value;
				OnFieldUpdated();
			}
		}
		public string FightOrderList
		{
			get{return m_TableLootInfo.FightOrderList;}
			set
			{
				m_TableLootInfo.FightOrderList = value;
				OnFieldUpdated();
			}
		}
		public string LootOrderList
		{
			get{return m_TableLootInfo.LootOrderList;}
			set
			{
				m_TableLootInfo.LootOrderList = value;
				OnFieldUpdated();
			}
		}
		public int LootIncome
		{
			get{return m_TableLootInfo.LootIncome;}
			set
			{
				m_TableLootInfo.LootIncome = value;
				OnFieldUpdated();
			}
		}
		public int LootLoss
		{
			get{return m_TableLootInfo.LootLoss;}
			set
			{
				m_TableLootInfo.LootLoss = value;
				OnFieldUpdated();
			}
		}
		public int LootType
		{
			get{return m_TableLootInfo.LootType;}
			set
			{
				m_TableLootInfo.LootType = value;
				OnFieldUpdated();
			}
		}
		public byte[] LootBytes
		{
			get{return m_TableLootInfo.LootBytes;}
			set
			{
				m_TableLootInfo.LootBytes = value;
				OnFieldUpdated();
			}
		}

		public TableLootInfo ToProto()
		{
			m_TableLootInfo.LastBattleTime = m_LastBattleTime.ToString("yyyyMMddHHmmss");
			m_TableLootInfo.BeginTime = m_BeginTime.ToString("yyyyMMddHHmmss");
			return m_TableLootInfo;
		}
		public void FromProto(TableLootInfo proto)
		{
			m_TableLootInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastBattleTime = DateTime.ParseExact(m_TableLootInfo.LastBattleTime,"yyyyMMddHHmmss",null);
			m_BeginTime = DateTime.ParseExact(m_TableLootInfo.BeginTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLootInfo.UserGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLootInfo m_TableLootInfo = new TableLootInfo();
		private DateTime m_LastBattleTime = new DateTime();
		private DateTime m_BeginTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableLootRecordWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableLootRecord.Guid;}
			set
			{
				m_TableLootRecord.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableLootRecord.UserGuid;}
			set
			{
				m_TableLootRecord.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public bool IsPool
		{
			get{return m_TableLootRecord.IsPool;}
			set
			{
				m_TableLootRecord.IsPool = value;
				OnFieldUpdated();
			}
		}
		public bool IsLootSuccess
		{
			get{return m_TableLootRecord.IsLootSuccess;}
			set
			{
				m_TableLootRecord.IsLootSuccess = value;
				OnFieldUpdated();
			}
		}
		public DateTime LootBeginTime
		{
			get{return m_LootBeginTime;}
			set
			{
				m_LootBeginTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime LootEndTime
		{
			get{return m_LootEndTime;}
			set
			{
				m_LootEndTime = value;
				OnFieldUpdated();
			}
		}
		public int DomainType
		{
			get{return m_TableLootRecord.DomainType;}
			set
			{
				m_TableLootRecord.DomainType = value;
				OnFieldUpdated();
			}
		}
		public int Booty
		{
			get{return m_TableLootRecord.Booty;}
			set
			{
				m_TableLootRecord.Booty = value;
				OnFieldUpdated();
			}
		}
		public long LGuid
		{
			get{return m_TableLootRecord.LGuid;}
			set
			{
				m_TableLootRecord.LGuid = value;
				OnFieldUpdated();
			}
		}
		public int LHeroId
		{
			get{return m_TableLootRecord.LHeroId;}
			set
			{
				m_TableLootRecord.LHeroId = value;
				OnFieldUpdated();
			}
		}
		public int LLevel
		{
			get{return m_TableLootRecord.LLevel;}
			set
			{
				m_TableLootRecord.LLevel = value;
				OnFieldUpdated();
			}
		}
		public int LFightScore
		{
			get{return m_TableLootRecord.LFightScore;}
			set
			{
				m_TableLootRecord.LFightScore = value;
				OnFieldUpdated();
			}
		}
		public string LNickname
		{
			get{return m_TableLootRecord.LNickname;}
			set
			{
				m_TableLootRecord.LNickname = value;
				OnFieldUpdated();
			}
		}
		public int LUserDamage
		{
			get{return m_TableLootRecord.LUserDamage;}
			set
			{
				m_TableLootRecord.LUserDamage = value;
				OnFieldUpdated();
			}
		}
		public string LDefenseOrderList
		{
			get{return m_TableLootRecord.LDefenseOrderList;}
			set
			{
				m_TableLootRecord.LDefenseOrderList = value;
				OnFieldUpdated();
			}
		}
		public string LLootOrderList
		{
			get{return m_TableLootRecord.LLootOrderList;}
			set
			{
				m_TableLootRecord.LLootOrderList = value;
				OnFieldUpdated();
			}
		}
		public long DGuid
		{
			get{return m_TableLootRecord.DGuid;}
			set
			{
				m_TableLootRecord.DGuid = value;
				OnFieldUpdated();
			}
		}
		public int DHeroId
		{
			get{return m_TableLootRecord.DHeroId;}
			set
			{
				m_TableLootRecord.DHeroId = value;
				OnFieldUpdated();
			}
		}
		public int DLevel
		{
			get{return m_TableLootRecord.DLevel;}
			set
			{
				m_TableLootRecord.DLevel = value;
				OnFieldUpdated();
			}
		}
		public int DFightScore
		{
			get{return m_TableLootRecord.DFightScore;}
			set
			{
				m_TableLootRecord.DFightScore = value;
				OnFieldUpdated();
			}
		}
		public string DNickname
		{
			get{return m_TableLootRecord.DNickname;}
			set
			{
				m_TableLootRecord.DNickname = value;
				OnFieldUpdated();
			}
		}
		public int DUserDamage
		{
			get{return m_TableLootRecord.DUserDamage;}
			set
			{
				m_TableLootRecord.DUserDamage = value;
				OnFieldUpdated();
			}
		}
		public string DDefenseOrderList
		{
			get{return m_TableLootRecord.DDefenseOrderList;}
			set
			{
				m_TableLootRecord.DDefenseOrderList = value;
				OnFieldUpdated();
			}
		}
		public string DLootOrderList
		{
			get{return m_TableLootRecord.DLootOrderList;}
			set
			{
				m_TableLootRecord.DLootOrderList = value;
				OnFieldUpdated();
			}
		}

		public TableLootRecord ToProto()
		{
			m_TableLootRecord.LootBeginTime = m_LootBeginTime.ToString("yyyyMMddHHmmss");
			m_TableLootRecord.LootEndTime = m_LootEndTime.ToString("yyyyMMddHHmmss");
			return m_TableLootRecord;
		}
		public void FromProto(TableLootRecord proto)
		{
			m_TableLootRecord = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LootBeginTime = DateTime.ParseExact(m_TableLootRecord.LootBeginTime,"yyyyMMddHHmmss",null);
			m_LootEndTime = DateTime.ParseExact(m_TableLootRecord.LootEndTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLootRecord.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableLootRecord.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLootRecord m_TableLootRecord = new TableLootRecord();
		private DateTime m_LootBeginTime = new DateTime();
		private DateTime m_LootEndTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableLotteryInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableLotteryInfo.Guid;}
			set
			{
				m_TableLotteryInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableLotteryInfo.UserGuid;}
			set
			{
				m_TableLotteryInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int LotteryId
		{
			get{return m_TableLotteryInfo.LotteryId;}
			set
			{
				m_TableLotteryInfo.LotteryId = value;
				OnFieldUpdated();
			}
		}
		public bool IsFirstDraw
		{
			get{return m_TableLotteryInfo.IsFirstDraw;}
			set
			{
				m_TableLotteryInfo.IsFirstDraw = value;
				OnFieldUpdated();
			}
		}
		public int FreeCount
		{
			get{return m_TableLotteryInfo.FreeCount;}
			set
			{
				m_TableLotteryInfo.FreeCount = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastDrawTime
		{
			get{return m_LastDrawTime;}
			set
			{
				m_LastDrawTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetCountTime
		{
			get{return m_LastResetCountTime;}
			set
			{
				m_LastResetCountTime = value;
				OnFieldUpdated();
			}
		}

		public TableLotteryInfo ToProto()
		{
			m_TableLotteryInfo.LastDrawTime = m_LastDrawTime.ToString("yyyyMMddHHmmss");
			m_TableLotteryInfo.LastResetCountTime = m_LastResetCountTime.ToString("yyyyMMddHHmmss");
			return m_TableLotteryInfo;
		}
		public void FromProto(TableLotteryInfo proto)
		{
			m_TableLotteryInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastDrawTime = DateTime.ParseExact(m_TableLotteryInfo.LastDrawTime,"yyyyMMddHHmmss",null);
			m_LastResetCountTime = DateTime.ParseExact(m_TableLotteryInfo.LastResetCountTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableLotteryInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableLotteryInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableLotteryInfo m_TableLotteryInfo = new TableLotteryInfo();
		private DateTime m_LastDrawTime = new DateTime();
		private DateTime m_LastResetCountTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableMailInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long Guid
		{
			get{return m_TableMailInfo.Guid;}
			set
			{
				m_TableMailInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableMailInfo.LogicServerId;}
			set
			{
				m_TableMailInfo.LogicServerId = value;
				OnFieldUpdated();
			}
		}
		public int ModuleTypeId
		{
			get{return m_TableMailInfo.ModuleTypeId;}
			set
			{
				m_TableMailInfo.ModuleTypeId = value;
				OnFieldUpdated();
			}
		}
		public string Sender
		{
			get{return m_TableMailInfo.Sender;}
			set
			{
				m_TableMailInfo.Sender = value;
				OnFieldUpdated();
			}
		}
		public long Receiver
		{
			get{return m_TableMailInfo.Receiver;}
			set
			{
				m_TableMailInfo.Receiver = value;
				OnFieldUpdated();
			}
		}
		public DateTime SendDate
		{
			get{return m_SendDate;}
			set
			{
				m_SendDate = value;
				OnFieldUpdated();
			}
		}
		public DateTime ExpiryDate
		{
			get{return m_ExpiryDate;}
			set
			{
				m_ExpiryDate = value;
				OnFieldUpdated();
			}
		}
		public string Title
		{
			get{return m_TableMailInfo.Title;}
			set
			{
				m_TableMailInfo.Title = value;
				OnFieldUpdated();
			}
		}
		public string Text
		{
			get{return m_TableMailInfo.Text;}
			set
			{
				m_TableMailInfo.Text = value;
				OnFieldUpdated();
			}
		}
		public int Money
		{
			get{return m_TableMailInfo.Money;}
			set
			{
				m_TableMailInfo.Money = value;
				OnFieldUpdated();
			}
		}
		public int Gold
		{
			get{return m_TableMailInfo.Gold;}
			set
			{
				m_TableMailInfo.Gold = value;
				OnFieldUpdated();
			}
		}
		public int Stamina
		{
			get{return m_TableMailInfo.Stamina;}
			set
			{
				m_TableMailInfo.Stamina = value;
				OnFieldUpdated();
			}
		}
		public string ItemIds
		{
			get{return m_TableMailInfo.ItemIds;}
			set
			{
				m_TableMailInfo.ItemIds = value;
				OnFieldUpdated();
			}
		}
		public string ItemNumbers
		{
			get{return m_TableMailInfo.ItemNumbers;}
			set
			{
				m_TableMailInfo.ItemNumbers = value;
				OnFieldUpdated();
			}
		}
		public int LevelDemand
		{
			get{return m_TableMailInfo.LevelDemand;}
			set
			{
				m_TableMailInfo.LevelDemand = value;
				OnFieldUpdated();
			}
		}
		public bool IsRead
		{
			get{return m_TableMailInfo.IsRead;}
			set
			{
				m_TableMailInfo.IsRead = value;
				OnFieldUpdated();
			}
		}

		public TableMailInfo ToProto()
		{
			m_TableMailInfo.SendDate = m_SendDate.ToString("yyyyMMddHHmmss");
			m_TableMailInfo.ExpiryDate = m_ExpiryDate.ToString("yyyyMMddHHmmss");
			return m_TableMailInfo;
		}
		public void FromProto(TableMailInfo proto)
		{
			m_TableMailInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_SendDate = DateTime.ParseExact(m_TableMailInfo.SendDate,"yyyyMMddHHmmss",null);
			m_ExpiryDate = DateTime.ParseExact(m_TableMailInfo.ExpiryDate,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableMailInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMailInfo m_TableMailInfo = new TableMailInfo();
		private DateTime m_SendDate = new DateTime();
		private DateTime m_ExpiryDate = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableMailStateInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableMailStateInfo.Guid;}
			set
			{
				m_TableMailStateInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableMailStateInfo.UserGuid;}
			set
			{
				m_TableMailStateInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public long MailGuid
		{
			get{return m_TableMailStateInfo.MailGuid;}
			set
			{
				m_TableMailStateInfo.MailGuid = value;
				OnFieldUpdated();
			}
		}
		public bool IsRead
		{
			get{return m_TableMailStateInfo.IsRead;}
			set
			{
				m_TableMailStateInfo.IsRead = value;
				OnFieldUpdated();
			}
		}
		public bool IsReceived
		{
			get{return m_TableMailStateInfo.IsReceived;}
			set
			{
				m_TableMailStateInfo.IsReceived = value;
				OnFieldUpdated();
			}
		}
		public DateTime ExpiryDate
		{
			get{return m_ExpiryDate;}
			set
			{
				m_ExpiryDate = value;
				OnFieldUpdated();
			}
		}

		public TableMailStateInfo ToProto()
		{
			m_TableMailStateInfo.ExpiryDate = m_ExpiryDate.ToString("yyyyMMddHHmmss");
			return m_TableMailStateInfo;
		}
		public void FromProto(TableMailStateInfo proto)
		{
			m_TableMailStateInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_ExpiryDate = DateTime.ParseExact(m_TableMailStateInfo.ExpiryDate,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableMailStateInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableMailStateInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMailStateInfo m_TableMailStateInfo = new TableMailStateInfo();
		private DateTime m_ExpiryDate = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableMissionInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableMissionInfo.Guid;}
			set
			{
				m_TableMissionInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableMissionInfo.UserGuid;}
			set
			{
				m_TableMissionInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int MissionId
		{
			get{return m_TableMissionInfo.MissionId;}
			set
			{
				m_TableMissionInfo.MissionId = value;
				OnFieldUpdated();
			}
		}
		public int MissionValue
		{
			get{return m_TableMissionInfo.MissionValue;}
			set
			{
				m_TableMissionInfo.MissionValue = value;
				OnFieldUpdated();
			}
		}
		public int MissionState
		{
			get{return m_TableMissionInfo.MissionState;}
			set
			{
				m_TableMissionInfo.MissionState = value;
				OnFieldUpdated();
			}
		}

		public TableMissionInfo ToProto()
		{
			return m_TableMissionInfo;
		}
		public void FromProto(TableMissionInfo proto)
		{
			m_TableMissionInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableMissionInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableMissionInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMissionInfo m_TableMissionInfo = new TableMissionInfo();

	}
}

namespace GameFramework
{
	public sealed class TableMpveAwardInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableMpveAwardInfo.Guid;}
			set
			{
				m_TableMpveAwardInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableMpveAwardInfo.UserGuid;}
			set
			{
				m_TableMpveAwardInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int MpveSceneId
		{
			get{return m_TableMpveAwardInfo.MpveSceneId;}
			set
			{
				m_TableMpveAwardInfo.MpveSceneId = value;
				OnFieldUpdated();
			}
		}
		public int DareCount
		{
			get{return m_TableMpveAwardInfo.DareCount;}
			set
			{
				m_TableMpveAwardInfo.DareCount = value;
				OnFieldUpdated();
			}
		}
		public int AwardCount
		{
			get{return m_TableMpveAwardInfo.AwardCount;}
			set
			{
				m_TableMpveAwardInfo.AwardCount = value;
				OnFieldUpdated();
			}
		}
		public string IsAwardedList
		{
			get{return m_TableMpveAwardInfo.IsAwardedList;}
			set
			{
				m_TableMpveAwardInfo.IsAwardedList = value;
				OnFieldUpdated();
			}
		}
		public string AwardIdList
		{
			get{return m_TableMpveAwardInfo.AwardIdList;}
			set
			{
				m_TableMpveAwardInfo.AwardIdList = value;
				OnFieldUpdated();
			}
		}
		public string DifficultyList
		{
			get{return m_TableMpveAwardInfo.DifficultyList;}
			set
			{
				m_TableMpveAwardInfo.DifficultyList = value;
				OnFieldUpdated();
			}
		}

		public TableMpveAwardInfo ToProto()
		{
			return m_TableMpveAwardInfo;
		}
		public void FromProto(TableMpveAwardInfo proto)
		{
			m_TableMpveAwardInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableMpveAwardInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableMpveAwardInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMpveAwardInfo m_TableMpveAwardInfo = new TableMpveAwardInfo();

	}
}

namespace GameFramework
{
	public sealed class TableNicknameWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Nickname
		{
			get{return m_TableNickname.Nickname;}
			set
			{
				m_TableNickname.Nickname = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableNickname.UserGuid;}
			set
			{
				m_TableNickname.UserGuid = value;
				OnFieldUpdated();
			}
		}

		public TableNickname ToProto()
		{
			return m_TableNickname;
		}
		public void FromProto(TableNickname proto)
		{
			m_TableNickname = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableNickname.Nickname.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableNickname m_TableNickname = new TableNickname();

	}
}

namespace GameFramework
{
	public sealed class TablePartnerInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TablePartnerInfo.Guid;}
			set
			{
				m_TablePartnerInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TablePartnerInfo.UserGuid;}
			set
			{
				m_TablePartnerInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int PartnerId
		{
			get{return m_TablePartnerInfo.PartnerId;}
			set
			{
				m_TablePartnerInfo.PartnerId = value;
				OnFieldUpdated();
			}
		}
		public int AdditionLevel
		{
			get{return m_TablePartnerInfo.AdditionLevel;}
			set
			{
				m_TablePartnerInfo.AdditionLevel = value;
				OnFieldUpdated();
			}
		}
		public int SkillLevel
		{
			get{return m_TablePartnerInfo.SkillLevel;}
			set
			{
				m_TablePartnerInfo.SkillLevel = value;
				OnFieldUpdated();
			}
		}
		public string EquipList
		{
			get{return m_TablePartnerInfo.EquipList;}
			set
			{
				m_TablePartnerInfo.EquipList = value;
				OnFieldUpdated();
			}
		}
		public int ActiveOrder
		{
			get{return m_TablePartnerInfo.ActiveOrder;}
			set
			{
				m_TablePartnerInfo.ActiveOrder = value;
				OnFieldUpdated();
			}
		}

		public TablePartnerInfo ToProto()
		{
			return m_TablePartnerInfo;
		}
		public void FromProto(TablePartnerInfo proto)
		{
			m_TablePartnerInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TablePartnerInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TablePartnerInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TablePartnerInfo m_TablePartnerInfo = new TablePartnerInfo();

	}
}

namespace GameFramework
{
	public sealed class TablePaymentInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TablePaymentInfo.Guid;}
			set
			{
				m_TablePaymentInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TablePaymentInfo.UserGuid;}
			set
			{
				m_TablePaymentInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public string OrderId
		{
			get{return m_TablePaymentInfo.OrderId;}
			set
			{
				m_TablePaymentInfo.OrderId = value;
				OnFieldUpdated();
			}
		}
		public string GoodsRegisterId
		{
			get{return m_TablePaymentInfo.GoodsRegisterId;}
			set
			{
				m_TablePaymentInfo.GoodsRegisterId = value;
				OnFieldUpdated();
			}
		}
		public int Diamond
		{
			get{return m_TablePaymentInfo.Diamond;}
			set
			{
				m_TablePaymentInfo.Diamond = value;
				OnFieldUpdated();
			}
		}
		public DateTime PaymentTime
		{
			get{return m_PaymentTime;}
			set
			{
				m_PaymentTime = value;
				OnFieldUpdated();
			}
		}

		public TablePaymentInfo ToProto()
		{
			m_TablePaymentInfo.PaymentTime = m_PaymentTime.ToString("yyyyMMddHHmmss");
			return m_TablePaymentInfo;
		}
		public void FromProto(TablePaymentInfo proto)
		{
			m_TablePaymentInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_PaymentTime = DateTime.ParseExact(m_TablePaymentInfo.PaymentTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TablePaymentInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TablePaymentInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TablePaymentInfo m_TablePaymentInfo = new TablePaymentInfo();
		private DateTime m_PaymentTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableSkillInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableSkillInfo.Guid;}
			set
			{
				m_TableSkillInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableSkillInfo.UserGuid;}
			set
			{
				m_TableSkillInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int SkillId
		{
			get{return m_TableSkillInfo.SkillId;}
			set
			{
				m_TableSkillInfo.SkillId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableSkillInfo.Level;}
			set
			{
				m_TableSkillInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int Preset
		{
			get{return m_TableSkillInfo.Preset;}
			set
			{
				m_TableSkillInfo.Preset = value;
				OnFieldUpdated();
			}
		}

		public TableSkillInfo ToProto()
		{
			return m_TableSkillInfo;
		}
		public void FromProto(TableSkillInfo proto)
		{
			m_TableSkillInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableSkillInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableSkillInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableSkillInfo m_TableSkillInfo = new TableSkillInfo();

	}
}

namespace GameFramework
{
	public sealed class TableTalentInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableTalentInfo.Guid;}
			set
			{
				m_TableTalentInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableTalentInfo.UserGuid;}
			set
			{
				m_TableTalentInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public long ItemGuid
		{
			get{return m_TableTalentInfo.ItemGuid;}
			set
			{
				m_TableTalentInfo.ItemGuid = value;
				OnFieldUpdated();
			}
		}
		public int Position
		{
			get{return m_TableTalentInfo.Position;}
			set
			{
				m_TableTalentInfo.Position = value;
				OnFieldUpdated();
			}
		}
		public int ItemId
		{
			get{return m_TableTalentInfo.ItemId;}
			set
			{
				m_TableTalentInfo.ItemId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableTalentInfo.Level;}
			set
			{
				m_TableTalentInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int Experience
		{
			get{return m_TableTalentInfo.Experience;}
			set
			{
				m_TableTalentInfo.Experience = value;
				OnFieldUpdated();
			}
		}

		public TableTalentInfo ToProto()
		{
			return m_TableTalentInfo;
		}
		public void FromProto(TableTalentInfo proto)
		{
			m_TableTalentInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableTalentInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableTalentInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableTalentInfo m_TableTalentInfo = new TableTalentInfo();

	}
}

namespace GameFramework
{
	public sealed class TableUndonePaymentWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string OrderId
		{
			get{return m_TableUndonePayment.OrderId;}
			set
			{
				m_TableUndonePayment.OrderId = value;
				OnPrimaryKeyUpdated();
			}
		}
		public string GoodsRegisterId
		{
			get{return m_TableUndonePayment.GoodsRegisterId;}
			set
			{
				m_TableUndonePayment.GoodsRegisterId = value;
				OnFieldUpdated();
			}
		}
		public int GoodsNum
		{
			get{return m_TableUndonePayment.GoodsNum;}
			set
			{
				m_TableUndonePayment.GoodsNum = value;
				OnFieldUpdated();
			}
		}
		public float GoodsPrice
		{
			get{return m_TableUndonePayment.GoodsPrice;}
			set
			{
				m_TableUndonePayment.GoodsPrice = value;
				OnFieldUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableUndonePayment.UserGuid;}
			set
			{
				m_TableUndonePayment.UserGuid = value;
				OnFieldUpdated();
			}
		}
		public string ChannelId
		{
			get{return m_TableUndonePayment.ChannelId;}
			set
			{
				m_TableUndonePayment.ChannelId = value;
				OnFieldUpdated();
			}
		}

		public TableUndonePayment ToProto()
		{
			return m_TableUndonePayment;
		}
		public void FromProto(TableUndonePayment proto)
		{
			m_TableUndonePayment = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableUndonePayment.OrderId.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUndonePayment m_TableUndonePayment = new TableUndonePayment();

	}
}

namespace GameFramework
{
	public sealed class TableUserBattleInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long Guid
		{
			get{return m_TableUserBattleInfo.Guid;}
			set
			{
				m_TableUserBattleInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int SceneId
		{
			get{return m_TableUserBattleInfo.SceneId;}
			set
			{
				m_TableUserBattleInfo.SceneId = value;
				OnFieldUpdated();
			}
		}
		public long StartTime
		{
			get{return m_TableUserBattleInfo.StartTime;}
			set
			{
				m_TableUserBattleInfo.StartTime = value;
				OnFieldUpdated();
			}
		}
		public int SumGold
		{
			get{return m_TableUserBattleInfo.SumGold;}
			set
			{
				m_TableUserBattleInfo.SumGold = value;
				OnFieldUpdated();
			}
		}
		public int Exp
		{
			get{return m_TableUserBattleInfo.Exp;}
			set
			{
				m_TableUserBattleInfo.Exp = value;
				OnFieldUpdated();
			}
		}
		public int RewardItemId
		{
			get{return m_TableUserBattleInfo.RewardItemId;}
			set
			{
				m_TableUserBattleInfo.RewardItemId = value;
				OnFieldUpdated();
			}
		}
		public int RewardItemCount
		{
			get{return m_TableUserBattleInfo.RewardItemCount;}
			set
			{
				m_TableUserBattleInfo.RewardItemCount = value;
				OnFieldUpdated();
			}
		}
		public int DeadCount
		{
			get{return m_TableUserBattleInfo.DeadCount;}
			set
			{
				m_TableUserBattleInfo.DeadCount = value;
				OnFieldUpdated();
			}
		}
		public int ReliveCount
		{
			get{return m_TableUserBattleInfo.ReliveCount;}
			set
			{
				m_TableUserBattleInfo.ReliveCount = value;
				OnFieldUpdated();
			}
		}
		public bool IsClearing
		{
			get{return m_TableUserBattleInfo.IsClearing;}
			set
			{
				m_TableUserBattleInfo.IsClearing = value;
				OnFieldUpdated();
			}
		}
		public int MatchKey
		{
			get{return m_TableUserBattleInfo.MatchKey;}
			set
			{
				m_TableUserBattleInfo.MatchKey = value;
				OnFieldUpdated();
			}
		}
		public int PartnerFinishedCount
		{
			get{return m_TableUserBattleInfo.PartnerFinishedCount;}
			set
			{
				m_TableUserBattleInfo.PartnerFinishedCount = value;
				OnFieldUpdated();
			}
		}
		public int PartnerRemainCount
		{
			get{return m_TableUserBattleInfo.PartnerRemainCount;}
			set
			{
				m_TableUserBattleInfo.PartnerRemainCount = value;
				OnFieldUpdated();
			}
		}
		public int PartnerBuyCount
		{
			get{return m_TableUserBattleInfo.PartnerBuyCount;}
			set
			{
				m_TableUserBattleInfo.PartnerBuyCount = value;
				OnFieldUpdated();
			}
		}
		public string PartnerList
		{
			get{return m_TableUserBattleInfo.PartnerList;}
			set
			{
				m_TableUserBattleInfo.PartnerList = value;
				OnFieldUpdated();
			}
		}
		public int PartnerSelectIndex
		{
			get{return m_TableUserBattleInfo.PartnerSelectIndex;}
			set
			{
				m_TableUserBattleInfo.PartnerSelectIndex = value;
				OnFieldUpdated();
			}
		}
		public DateTime PartnerLastResetTime
		{
			get{return m_PartnerLastResetTime;}
			set
			{
				m_PartnerLastResetTime = value;
				OnFieldUpdated();
			}
		}
		public int DungeonQueryCount
		{
			get{return m_TableUserBattleInfo.DungeonQueryCount;}
			set
			{
				m_TableUserBattleInfo.DungeonQueryCount = value;
				OnFieldUpdated();
			}
		}
		public int DungeonLeftFightCount
		{
			get{return m_TableUserBattleInfo.DungeonLeftFightCount;}
			set
			{
				m_TableUserBattleInfo.DungeonLeftFightCount = value;
				OnFieldUpdated();
			}
		}
		public int DungeonBuyFightCount
		{
			get{return m_TableUserBattleInfo.DungeonBuyFightCount;}
			set
			{
				m_TableUserBattleInfo.DungeonBuyFightCount = value;
				OnFieldUpdated();
			}
		}
		public string DungeonMatchTargetList
		{
			get{return m_TableUserBattleInfo.DungeonMatchTargetList;}
			set
			{
				m_TableUserBattleInfo.DungeonMatchTargetList = value;
				OnFieldUpdated();
			}
		}
		public string DungeonMatchDropList
		{
			get{return m_TableUserBattleInfo.DungeonMatchDropList;}
			set
			{
				m_TableUserBattleInfo.DungeonMatchDropList = value;
				OnFieldUpdated();
			}
		}
		public DateTime DungeonLastResetTime
		{
			get{return m_DungeonLastResetTime;}
			set
			{
				m_DungeonLastResetTime = value;
				OnFieldUpdated();
			}
		}
		public int SecretCurrentFight
		{
			get{return m_TableUserBattleInfo.SecretCurrentFight;}
			set
			{
				m_TableUserBattleInfo.SecretCurrentFight = value;
				OnFieldUpdated();
			}
		}
		public string SecretHpRateList
		{
			get{return m_TableUserBattleInfo.SecretHpRateList;}
			set
			{
				m_TableUserBattleInfo.SecretHpRateList = value;
				OnFieldUpdated();
			}
		}
		public string SecretMpRateList
		{
			get{return m_TableUserBattleInfo.SecretMpRateList;}
			set
			{
				m_TableUserBattleInfo.SecretMpRateList = value;
				OnFieldUpdated();
			}
		}
		public string SecretSegmentList
		{
			get{return m_TableUserBattleInfo.SecretSegmentList;}
			set
			{
				m_TableUserBattleInfo.SecretSegmentList = value;
				OnFieldUpdated();
			}
		}
		public string SecretFightCountList
		{
			get{return m_TableUserBattleInfo.SecretFightCountList;}
			set
			{
				m_TableUserBattleInfo.SecretFightCountList = value;
				OnFieldUpdated();
			}
		}

		public TableUserBattleInfo ToProto()
		{
			m_TableUserBattleInfo.PartnerLastResetTime = m_PartnerLastResetTime.ToString("yyyyMMddHHmmss");
			m_TableUserBattleInfo.DungeonLastResetTime = m_DungeonLastResetTime.ToString("yyyyMMddHHmmss");
			return m_TableUserBattleInfo;
		}
		public void FromProto(TableUserBattleInfo proto)
		{
			m_TableUserBattleInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_PartnerLastResetTime = DateTime.ParseExact(m_TableUserBattleInfo.PartnerLastResetTime,"yyyyMMddHHmmss",null);
			m_DungeonLastResetTime = DateTime.ParseExact(m_TableUserBattleInfo.DungeonLastResetTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableUserBattleInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUserBattleInfo m_TableUserBattleInfo = new TableUserBattleInfo();
		private DateTime m_PartnerLastResetTime = new DateTime();
		private DateTime m_DungeonLastResetTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableUserGeneralInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long Guid
		{
			get{return m_TableUserGeneralInfo.Guid;}
			set
			{
				m_TableUserGeneralInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long GuideFlag
		{
			get{return m_TableUserGeneralInfo.GuideFlag;}
			set
			{
				m_TableUserGeneralInfo.GuideFlag = value;
				OnFieldUpdated();
			}
		}
		public string NewbieFlag
		{
			get{return m_TableUserGeneralInfo.NewbieFlag;}
			set
			{
				m_TableUserGeneralInfo.NewbieFlag = value;
				OnFieldUpdated();
			}
		}
		public string NewbieActionFlag
		{
			get{return m_TableUserGeneralInfo.NewbieActionFlag;}
			set
			{
				m_TableUserGeneralInfo.NewbieActionFlag = value;
				OnFieldUpdated();
			}
		}
		public int BuyMoneyCount
		{
			get{return m_TableUserGeneralInfo.BuyMoneyCount;}
			set
			{
				m_TableUserGeneralInfo.BuyMoneyCount = value;
				OnFieldUpdated();
			}
		}
		public double LastBuyMoneyTimestamp
		{
			get{return m_TableUserGeneralInfo.LastBuyMoneyTimestamp;}
			set
			{
				m_TableUserGeneralInfo.LastBuyMoneyTimestamp = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetMidasTouchTime
		{
			get{return m_LastResetMidasTouchTime;}
			set
			{
				m_LastResetMidasTouchTime = value;
				OnFieldUpdated();
			}
		}
		public int SellIncome
		{
			get{return m_TableUserGeneralInfo.SellIncome;}
			set
			{
				m_TableUserGeneralInfo.SellIncome = value;
				OnFieldUpdated();
			}
		}
		public double LastSellTimestamp
		{
			get{return m_TableUserGeneralInfo.LastSellTimestamp;}
			set
			{
				m_TableUserGeneralInfo.LastSellTimestamp = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetSellTime
		{
			get{return m_LastResetSellTime;}
			set
			{
				m_LastResetSellTime = value;
				OnFieldUpdated();
			}
		}
		public string ExchangeGoodList
		{
			get{return m_TableUserGeneralInfo.ExchangeGoodList;}
			set
			{
				m_TableUserGeneralInfo.ExchangeGoodList = value;
				OnFieldUpdated();
			}
		}
		public string ExchangeGoodNumber
		{
			get{return m_TableUserGeneralInfo.ExchangeGoodNumber;}
			set
			{
				m_TableUserGeneralInfo.ExchangeGoodNumber = value;
				OnFieldUpdated();
			}
		}
		public string ExchangeGoodRefreshCount
		{
			get{return m_TableUserGeneralInfo.ExchangeGoodRefreshCount;}
			set
			{
				m_TableUserGeneralInfo.ExchangeGoodRefreshCount = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetExchangeGoodTime
		{
			get{return m_LastResetExchangeGoodTime;}
			set
			{
				m_LastResetExchangeGoodTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetDaySignCountTime
		{
			get{return m_LastResetDaySignCountTime;}
			set
			{
				m_LastResetDaySignCountTime = value;
				OnFieldUpdated();
			}
		}
		public int MonthSignCount
		{
			get{return m_TableUserGeneralInfo.MonthSignCount;}
			set
			{
				m_TableUserGeneralInfo.MonthSignCount = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetMonthSignCountTime
		{
			get{return m_LastResetMonthSignCountTime;}
			set
			{
				m_LastResetMonthSignCountTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime MonthCardExpireTime
		{
			get{return m_MonthCardExpireTime;}
			set
			{
				m_MonthCardExpireTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsWeeklyLoginRewarded
		{
			get{return m_TableUserGeneralInfo.IsWeeklyLoginRewarded;}
			set
			{
				m_TableUserGeneralInfo.IsWeeklyLoginRewarded = value;
				OnFieldUpdated();
			}
		}
		public string WeeklyLoginRewardList
		{
			get{return m_TableUserGeneralInfo.WeeklyLoginRewardList;}
			set
			{
				m_TableUserGeneralInfo.WeeklyLoginRewardList = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetWeeklyLoginRewardTime
		{
			get{return m_LastResetWeeklyLoginRewardTime;}
			set
			{
				m_LastResetWeeklyLoginRewardTime = value;
				OnFieldUpdated();
			}
		}
		public int DailyOnlineDuration
		{
			get{return m_TableUserGeneralInfo.DailyOnlineDuration;}
			set
			{
				m_TableUserGeneralInfo.DailyOnlineDuration = value;
				OnFieldUpdated();
			}
		}
		public string DailyOnlineRewardList
		{
			get{return m_TableUserGeneralInfo.DailyOnlineRewardList;}
			set
			{
				m_TableUserGeneralInfo.DailyOnlineRewardList = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetDailyOnlineTime
		{
			get{return m_LastResetDailyOnlineTime;}
			set
			{
				m_LastResetDailyOnlineTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsFirstPaymentRewarded
		{
			get{return m_TableUserGeneralInfo.IsFirstPaymentRewarded;}
			set
			{
				m_TableUserGeneralInfo.IsFirstPaymentRewarded = value;
				OnFieldUpdated();
			}
		}
		public string VipRewaredList
		{
			get{return m_TableUserGeneralInfo.VipRewaredList;}
			set
			{
				m_TableUserGeneralInfo.VipRewaredList = value;
				OnFieldUpdated();
			}
		}
		public int MorrowRewardId
		{
			get{return m_TableUserGeneralInfo.MorrowRewardId;}
			set
			{
				m_TableUserGeneralInfo.MorrowRewardId = value;
				OnFieldUpdated();
			}
		}
		public DateTime MorrowActiveTime
		{
			get{return m_MorrowActiveTime;}
			set
			{
				m_MorrowActiveTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsMorrowActive
		{
			get{return m_TableUserGeneralInfo.IsMorrowActive;}
			set
			{
				m_TableUserGeneralInfo.IsMorrowActive = value;
				OnFieldUpdated();
			}
		}
		public double LevelupCostTime
		{
			get{return m_TableUserGeneralInfo.LevelupCostTime;}
			set
			{
				m_TableUserGeneralInfo.LevelupCostTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsChatForbidden
		{
			get{return m_TableUserGeneralInfo.IsChatForbidden;}
			set
			{
				m_TableUserGeneralInfo.IsChatForbidden = value;
				OnFieldUpdated();
			}
		}
		public int GrowthFund
		{
			get{return m_TableUserGeneralInfo.GrowthFund;}
			set
			{
				m_TableUserGeneralInfo.GrowthFund = value;
				OnFieldUpdated();
			}
		}
		public string ChapterIdList
		{
			get{return m_TableUserGeneralInfo.ChapterIdList;}
			set
			{
				m_TableUserGeneralInfo.ChapterIdList = value;
				OnFieldUpdated();
			}
		}
		public string ChapterAwardList
		{
			get{return m_TableUserGeneralInfo.ChapterAwardList;}
			set
			{
				m_TableUserGeneralInfo.ChapterAwardList = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetConsumeCountTime
		{
			get{return m_LastResetConsumeCountTime;}
			set
			{
				m_LastResetConsumeCountTime = value;
				OnFieldUpdated();
			}
		}
		public int DayRestSignCount
		{
			get{return m_TableUserGeneralInfo.DayRestSignCount;}
			set
			{
				m_TableUserGeneralInfo.DayRestSignCount = value;
				OnFieldUpdated();
			}
		}

		public TableUserGeneralInfo ToProto()
		{
			m_TableUserGeneralInfo.LastResetMidasTouchTime = m_LastResetMidasTouchTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetSellTime = m_LastResetSellTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetExchangeGoodTime = m_LastResetExchangeGoodTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetDaySignCountTime = m_LastResetDaySignCountTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetMonthSignCountTime = m_LastResetMonthSignCountTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.MonthCardExpireTime = m_MonthCardExpireTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetWeeklyLoginRewardTime = m_LastResetWeeklyLoginRewardTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetDailyOnlineTime = m_LastResetDailyOnlineTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.MorrowActiveTime = m_MorrowActiveTime.ToString("yyyyMMddHHmmss");
			m_TableUserGeneralInfo.LastResetConsumeCountTime = m_LastResetConsumeCountTime.ToString("yyyyMMddHHmmss");
			return m_TableUserGeneralInfo;
		}
		public void FromProto(TableUserGeneralInfo proto)
		{
			m_TableUserGeneralInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastResetMidasTouchTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetMidasTouchTime,"yyyyMMddHHmmss",null);
			m_LastResetSellTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetSellTime,"yyyyMMddHHmmss",null);
			m_LastResetExchangeGoodTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetExchangeGoodTime,"yyyyMMddHHmmss",null);
			m_LastResetDaySignCountTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetDaySignCountTime,"yyyyMMddHHmmss",null);
			m_LastResetMonthSignCountTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetMonthSignCountTime,"yyyyMMddHHmmss",null);
			m_MonthCardExpireTime = DateTime.ParseExact(m_TableUserGeneralInfo.MonthCardExpireTime,"yyyyMMddHHmmss",null);
			m_LastResetWeeklyLoginRewardTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetWeeklyLoginRewardTime,"yyyyMMddHHmmss",null);
			m_LastResetDailyOnlineTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetDailyOnlineTime,"yyyyMMddHHmmss",null);
			m_MorrowActiveTime = DateTime.ParseExact(m_TableUserGeneralInfo.MorrowActiveTime,"yyyyMMddHHmmss",null);
			m_LastResetConsumeCountTime = DateTime.ParseExact(m_TableUserGeneralInfo.LastResetConsumeCountTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableUserGeneralInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUserGeneralInfo m_TableUserGeneralInfo = new TableUserGeneralInfo();
		private DateTime m_LastResetMidasTouchTime = new DateTime();
		private DateTime m_LastResetSellTime = new DateTime();
		private DateTime m_LastResetExchangeGoodTime = new DateTime();
		private DateTime m_LastResetDaySignCountTime = new DateTime();
		private DateTime m_LastResetMonthSignCountTime = new DateTime();
		private DateTime m_MonthCardExpireTime = new DateTime();
		private DateTime m_LastResetWeeklyLoginRewardTime = new DateTime();
		private DateTime m_LastResetDailyOnlineTime = new DateTime();
		private DateTime m_MorrowActiveTime = new DateTime();
		private DateTime m_LastResetConsumeCountTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableUserInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public ulong Guid
		{
			get{return m_TableUserInfo.Guid;}
			set
			{
				m_TableUserInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int LogicServerId
		{
			get{return m_TableUserInfo.LogicServerId;}
			set
			{
				m_TableUserInfo.LogicServerId = value;
				OnForeignKeyUpdated();
			}
		}
		public string AccountId
		{
			get{return m_TableUserInfo.AccountId;}
			set
			{
				m_TableUserInfo.AccountId = value;
				OnForeignKeyUpdated();
			}
		}
		public string Nickname
		{
			get{return m_TableUserInfo.Nickname;}
			set
			{
				m_TableUserInfo.Nickname = value;
				OnFieldUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableUserInfo.HeroId;}
			set
			{
				m_TableUserInfo.HeroId = value;
				OnFieldUpdated();
			}
		}
		public DateTime CreateTime
		{
			get{return m_CreateTime;}
			set
			{
				m_CreateTime = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableUserInfo.Level;}
			set
			{
				m_TableUserInfo.Level = value;
				OnFieldUpdated();
			}
		}
		public int Money
		{
			get{return m_TableUserInfo.Money;}
			set
			{
				m_TableUserInfo.Money = value;
				OnFieldUpdated();
			}
		}
		public int Gold
		{
			get{return m_TableUserInfo.Gold;}
			set
			{
				m_TableUserInfo.Gold = value;
				OnFieldUpdated();
			}
		}
		public int ExpPoints
		{
			get{return m_TableUserInfo.ExpPoints;}
			set
			{
				m_TableUserInfo.ExpPoints = value;
				OnFieldUpdated();
			}
		}
		public int CitySceneId
		{
			get{return m_TableUserInfo.CitySceneId;}
			set
			{
				m_TableUserInfo.CitySceneId = value;
				OnFieldUpdated();
			}
		}
		public int Vip
		{
			get{return m_TableUserInfo.Vip;}
			set
			{
				m_TableUserInfo.Vip = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastLogoutTime
		{
			get{return m_LastLogoutTime;}
			set
			{
				m_LastLogoutTime = value;
				OnFieldUpdated();
			}
		}
		public int AchievedScore
		{
			get{return m_TableUserInfo.AchievedScore;}
			set
			{
				m_TableUserInfo.AchievedScore = value;
				OnFieldUpdated();
			}
		}

		public TableUserInfo ToProto()
		{
			m_TableUserInfo.CreateTime = m_CreateTime.ToString("yyyyMMddHHmmss");
			m_TableUserInfo.LastLogoutTime = m_LastLogoutTime.ToString("yyyyMMddHHmmss");
			return m_TableUserInfo;
		}
		public void FromProto(TableUserInfo proto)
		{
			m_TableUserInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_CreateTime = DateTime.ParseExact(m_TableUserInfo.CreateTime,"yyyyMMddHHmmss",null);
			m_LastLogoutTime = DateTime.ParseExact(m_TableUserInfo.LastLogoutTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableUserInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableUserInfo.LogicServerId.ToString());
			m_ForeignKeys.Add(m_TableUserInfo.AccountId.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUserInfo m_TableUserInfo = new TableUserInfo();
		private DateTime m_CreateTime = new DateTime();
		private DateTime m_LastLogoutTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableUserSpecialInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public long Guid
		{
			get{return m_TableUserSpecialInfo.Guid;}
			set
			{
				m_TableUserSpecialInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int Vigor
		{
			get{return m_TableUserSpecialInfo.Vigor;}
			set
			{
				m_TableUserSpecialInfo.Vigor = value;
				OnFieldUpdated();
			}
		}
		public double LastAddVigorTimestamp
		{
			get{return m_TableUserSpecialInfo.LastAddVigorTimestamp;}
			set
			{
				m_TableUserSpecialInfo.LastAddVigorTimestamp = value;
				OnFieldUpdated();
			}
		}
		public int Stamina
		{
			get{return m_TableUserSpecialInfo.Stamina;}
			set
			{
				m_TableUserSpecialInfo.Stamina = value;
				OnFieldUpdated();
			}
		}
		public int BuyStaminaCount
		{
			get{return m_TableUserSpecialInfo.BuyStaminaCount;}
			set
			{
				m_TableUserSpecialInfo.BuyStaminaCount = value;
				OnFieldUpdated();
			}
		}
		public double LastAddStaminaTimestamp
		{
			get{return m_TableUserSpecialInfo.LastAddStaminaTimestamp;}
			set
			{
				m_TableUserSpecialInfo.LastAddStaminaTimestamp = value;
				OnFieldUpdated();
			}
		}
		public int UsedStamina
		{
			get{return m_TableUserSpecialInfo.UsedStamina;}
			set
			{
				m_TableUserSpecialInfo.UsedStamina = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetStaminaTime
		{
			get{return m_LastResetStaminaTime;}
			set
			{
				m_LastResetStaminaTime = value;
				OnFieldUpdated();
			}
		}
		public string CompleteSceneList
		{
			get{return m_TableUserSpecialInfo.CompleteSceneList;}
			set
			{
				m_TableUserSpecialInfo.CompleteSceneList = value;
				OnFieldUpdated();
			}
		}
		public string CompleteSceneNumber
		{
			get{return m_TableUserSpecialInfo.CompleteSceneNumber;}
			set
			{
				m_TableUserSpecialInfo.CompleteSceneNumber = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetSceneCountTime
		{
			get{return m_LastResetSceneCountTime;}
			set
			{
				m_LastResetSceneCountTime = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetDailyMissionTime
		{
			get{return m_LastResetDailyMissionTime;}
			set
			{
				m_LastResetDailyMissionTime = value;
				OnFieldUpdated();
			}
		}
		public int ActiveFashionId
		{
			get{return m_TableUserSpecialInfo.ActiveFashionId;}
			set
			{
				m_TableUserSpecialInfo.ActiveFashionId = value;
				OnFieldUpdated();
			}
		}
		public bool IsFashionShow
		{
			get{return m_TableUserSpecialInfo.IsFashionShow;}
			set
			{
				m_TableUserSpecialInfo.IsFashionShow = value;
				OnFieldUpdated();
			}
		}
		public int ActiveWingId
		{
			get{return m_TableUserSpecialInfo.ActiveWingId;}
			set
			{
				m_TableUserSpecialInfo.ActiveWingId = value;
				OnFieldUpdated();
			}
		}
		public bool IsWingShow
		{
			get{return m_TableUserSpecialInfo.IsWingShow;}
			set
			{
				m_TableUserSpecialInfo.IsWingShow = value;
				OnFieldUpdated();
			}
		}
		public int ActiveWeaponId
		{
			get{return m_TableUserSpecialInfo.ActiveWeaponId;}
			set
			{
				m_TableUserSpecialInfo.ActiveWeaponId = value;
				OnFieldUpdated();
			}
		}
		public bool IsWeaponShow
		{
			get{return m_TableUserSpecialInfo.IsWeaponShow;}
			set
			{
				m_TableUserSpecialInfo.IsWeaponShow = value;
				OnFieldUpdated();
			}
		}
		public int Vitality
		{
			get{return m_TableUserSpecialInfo.Vitality;}
			set
			{
				m_TableUserSpecialInfo.Vitality = value;
				OnFieldUpdated();
			}
		}
		public double LastAddVitalityTimestamp
		{
			get{return m_TableUserSpecialInfo.LastAddVitalityTimestamp;}
			set
			{
				m_TableUserSpecialInfo.LastAddVitalityTimestamp = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetExpeditionTime
		{
			get{return m_LastResetExpeditionTime;}
			set
			{
				m_LastResetExpeditionTime = value;
				OnFieldUpdated();
			}
		}
		public long CorpsGuid
		{
			get{return m_TableUserSpecialInfo.CorpsGuid;}
			set
			{
				m_TableUserSpecialInfo.CorpsGuid = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastQuitCorpsTime
		{
			get{return m_LastQuitCorpsTime;}
			set
			{
				m_LastQuitCorpsTime = value;
				OnFieldUpdated();
			}
		}
		public bool IsAcquireCorpsSignInPrize
		{
			get{return m_TableUserSpecialInfo.IsAcquireCorpsSignInPrize;}
			set
			{
				m_TableUserSpecialInfo.IsAcquireCorpsSignInPrize = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetCorpsSignInPrizeTime
		{
			get{return m_LastResetCorpsSignInPrizeTime;}
			set
			{
				m_LastResetCorpsSignInPrizeTime = value;
				OnFieldUpdated();
			}
		}
		public string CorpsChapterIdList
		{
			get{return m_TableUserSpecialInfo.CorpsChapterIdList;}
			set
			{
				m_TableUserSpecialInfo.CorpsChapterIdList = value;
				OnFieldUpdated();
			}
		}
		public string CorpsChapterDareList
		{
			get{return m_TableUserSpecialInfo.CorpsChapterDareList;}
			set
			{
				m_TableUserSpecialInfo.CorpsChapterDareList = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetSecretAreaTime
		{
			get{return m_LastResetSecretAreaTime;}
			set
			{
				m_LastResetSecretAreaTime = value;
				OnFieldUpdated();
			}
		}
		public long RecentLoginState
		{
			get{return m_TableUserSpecialInfo.RecentLoginState;}
			set
			{
				m_TableUserSpecialInfo.RecentLoginState = value;
				OnFieldUpdated();
			}
		}
		public int SumLoginDayCount
		{
			get{return m_TableUserSpecialInfo.SumLoginDayCount;}
			set
			{
				m_TableUserSpecialInfo.SumLoginDayCount = value;
				OnFieldUpdated();
			}
		}
		public int UsedLoginLotteryDrawCount
		{
			get{return m_TableUserSpecialInfo.UsedLoginLotteryDrawCount;}
			set
			{
				m_TableUserSpecialInfo.UsedLoginLotteryDrawCount = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastSaveRecentLoginTime
		{
			get{return m_LastSaveRecentLoginTime;}
			set
			{
				m_LastSaveRecentLoginTime = value;
				OnFieldUpdated();
			}
		}
		public string DiamondBoxList
		{
			get{return m_TableUserSpecialInfo.DiamondBoxList;}
			set
			{
				m_TableUserSpecialInfo.DiamondBoxList = value;
				OnFieldUpdated();
			}
		}
		public DateTime LastResetMpveAwardTime
		{
			get{return m_LastResetMpveAwardTime;}
			set
			{
				m_LastResetMpveAwardTime = value;
				OnFieldUpdated();
			}
		}
		public string FinishedActivityList
		{
			get{return m_TableUserSpecialInfo.FinishedActivityList;}
			set
			{
				m_TableUserSpecialInfo.FinishedActivityList = value;
				OnFieldUpdated();
			}
		}

		public TableUserSpecialInfo ToProto()
		{
			m_TableUserSpecialInfo.LastResetStaminaTime = m_LastResetStaminaTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetSceneCountTime = m_LastResetSceneCountTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetDailyMissionTime = m_LastResetDailyMissionTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetExpeditionTime = m_LastResetExpeditionTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastQuitCorpsTime = m_LastQuitCorpsTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetCorpsSignInPrizeTime = m_LastResetCorpsSignInPrizeTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetSecretAreaTime = m_LastResetSecretAreaTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastSaveRecentLoginTime = m_LastSaveRecentLoginTime.ToString("yyyyMMddHHmmss");
			m_TableUserSpecialInfo.LastResetMpveAwardTime = m_LastResetMpveAwardTime.ToString("yyyyMMddHHmmss");
			return m_TableUserSpecialInfo;
		}
		public void FromProto(TableUserSpecialInfo proto)
		{
			m_TableUserSpecialInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_LastResetStaminaTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetStaminaTime,"yyyyMMddHHmmss",null);
			m_LastResetSceneCountTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetSceneCountTime,"yyyyMMddHHmmss",null);
			m_LastResetDailyMissionTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetDailyMissionTime,"yyyyMMddHHmmss",null);
			m_LastResetExpeditionTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetExpeditionTime,"yyyyMMddHHmmss",null);
			m_LastQuitCorpsTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastQuitCorpsTime,"yyyyMMddHHmmss",null);
			m_LastResetCorpsSignInPrizeTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetCorpsSignInPrizeTime,"yyyyMMddHHmmss",null);
			m_LastResetSecretAreaTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetSecretAreaTime,"yyyyMMddHHmmss",null);
			m_LastSaveRecentLoginTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastSaveRecentLoginTime,"yyyyMMddHHmmss",null);
			m_LastResetMpveAwardTime = DateTime.ParseExact(m_TableUserSpecialInfo.LastResetMpveAwardTime,"yyyyMMddHHmmss",null);
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableUserSpecialInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUserSpecialInfo m_TableUserSpecialInfo = new TableUserSpecialInfo();
		private DateTime m_LastResetStaminaTime = new DateTime();
		private DateTime m_LastResetSceneCountTime = new DateTime();
		private DateTime m_LastResetDailyMissionTime = new DateTime();
		private DateTime m_LastResetExpeditionTime = new DateTime();
		private DateTime m_LastQuitCorpsTime = new DateTime();
		private DateTime m_LastResetCorpsSignInPrizeTime = new DateTime();
		private DateTime m_LastResetSecretAreaTime = new DateTime();
		private DateTime m_LastSaveRecentLoginTime = new DateTime();
		private DateTime m_LastResetMpveAwardTime = new DateTime();

	}
}

namespace GameFramework
{
	public sealed class TableXSoulInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Guid
		{
			get{return m_TableXSoulInfo.Guid;}
			set
			{
				m_TableXSoulInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public long UserGuid
		{
			get{return m_TableXSoulInfo.UserGuid;}
			set
			{
				m_TableXSoulInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int Position
		{
			get{return m_TableXSoulInfo.Position;}
			set
			{
				m_TableXSoulInfo.Position = value;
				OnFieldUpdated();
			}
		}
		public int XSoulType
		{
			get{return m_TableXSoulInfo.XSoulType;}
			set
			{
				m_TableXSoulInfo.XSoulType = value;
				OnFieldUpdated();
			}
		}
		public int XSoulId
		{
			get{return m_TableXSoulInfo.XSoulId;}
			set
			{
				m_TableXSoulInfo.XSoulId = value;
				OnFieldUpdated();
			}
		}
		public int XSoulLevel
		{
			get{return m_TableXSoulInfo.XSoulLevel;}
			set
			{
				m_TableXSoulInfo.XSoulLevel = value;
				OnFieldUpdated();
			}
		}
		public int XSoulExp
		{
			get{return m_TableXSoulInfo.XSoulExp;}
			set
			{
				m_TableXSoulInfo.XSoulExp = value;
				OnFieldUpdated();
			}
		}
		public int XSoulModelLevel
		{
			get{return m_TableXSoulInfo.XSoulModelLevel;}
			set
			{
				m_TableXSoulInfo.XSoulModelLevel = value;
				OnFieldUpdated();
			}
		}

		public TableXSoulInfo ToProto()
		{
			return m_TableXSoulInfo;
		}
		public void FromProto(TableXSoulInfo proto)
		{
			m_TableXSoulInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
		}

		private void OnFieldUpdated()
		{
			m_Modified = true;
		}

		private void OnPrimaryKeyUpdated()
		{
			m_Modified = true;
			UpdatePrimaryKeys();
		}

		private void OnForeignKeyUpdated()
		{
			m_Modified = true;
			UpdateForeignKeys();
		}

		private void UpdatePrimaryKeys()
		{
			m_PrimaryKeys.Clear();
			m_PrimaryKeys.Add(m_TableXSoulInfo.Guid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableXSoulInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableXSoulInfo m_TableXSoulInfo = new TableXSoulInfo();

	}
}
