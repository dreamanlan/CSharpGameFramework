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
	public sealed partial class TableAccountWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
		}

		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
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
		public string Password
		{
			get{return m_TableAccount.Password;}
			set
			{
				m_TableAccount.Password = value;
				OnFieldUpdated();
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
		public ulong UserGuid
		{
			get{return m_TableAccount.UserGuid;}
			set
			{
				m_TableAccount.UserGuid = value;
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
			if (m_TableAccount.AccountId != null) {
				m_PrimaryKeys.Add(m_TableAccount.AccountId.ToString());
			}
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableAccount m_TableAccount = new TableAccount();

	}
}

namespace GameFramework
{
	public sealed partial class TableActivationCodeWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
			if (m_TableActivationCode.ActivationCode != null) {
				m_PrimaryKeys.Add(m_TableActivationCode.ActivationCode.ToString());
			}
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableActivationCode m_TableActivationCode = new TableActivationCode();

	}
}

namespace GameFramework
{
	public sealed partial class FriendInfo
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
			get{return m_TableFriendInfo.Guid;}
			set
			{
				m_TableFriendInfo.Guid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableFriendInfo.UserGuid;}
			set
			{
				m_TableFriendInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public ulong FriendGuid
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
		public long QQ
		{
			get{return m_TableFriendInfo.QQ;}
			set
			{
				m_TableFriendInfo.QQ = value;
				OnFieldUpdated();
			}
		}
		public long weixin
		{
			get{return m_TableFriendInfo.weixin;}
			set
			{
				m_TableFriendInfo.weixin = value;
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
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableFriendInfo m_TableFriendInfo = new TableFriendInfo();

	}
}

namespace GameFramework
{
	public sealed partial class TableGlobalDataWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
		}

		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public string Key
		{
			get{return m_TableGlobalData.Key;}
			set
			{
				m_TableGlobalData.Key = value;
				OnPrimaryKeyUpdated();
			}
		}
		public int IntValue
		{
			get{return m_TableGlobalData.IntValue;}
			set
			{
				m_TableGlobalData.IntValue = value;
				OnFieldUpdated();
			}
		}
		public float FloatValue
		{
			get{return m_TableGlobalData.FloatValue;}
			set
			{
				m_TableGlobalData.FloatValue = value;
				OnFieldUpdated();
			}
		}
		public string StrValue
		{
			get{return m_TableGlobalData.StrValue;}
			set
			{
				m_TableGlobalData.StrValue = value;
				OnFieldUpdated();
			}
		}

		public TableGlobalData ToProto()
		{
			return m_TableGlobalData;
		}
		public void FromProto(TableGlobalData proto)
		{
			m_TableGlobalData = proto;
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
			if (m_TableGlobalData.Key != null) {
				m_PrimaryKeys.Add(m_TableGlobalData.Key.ToString());
			}
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGlobalData m_TableGlobalData = new TableGlobalData();

	}
}

namespace GameFramework
{
	public sealed partial class TableGuidWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
			if (m_TableGuid.GuidType != null) {
				m_PrimaryKeys.Add(m_TableGuid.GuidType.ToString());
			}
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableGuid m_TableGuid = new TableGuid();

	}
}

namespace GameFramework
{
	public sealed partial class ItemInfo
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableItemInfo m_TableItemInfo = new TableItemInfo();

	}
}

namespace GameFramework
{
	public sealed partial class TableMailInfoWrap
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
			get{return m_TableMailInfo.Guid;}
			set
			{
				m_TableMailInfo.Guid = value;
				OnPrimaryKeyUpdated();
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
		public int GetItemIdsCount()
		{
		  return m_ItemIds.Count;
		}
		public void SetItemIds(int ix, int val)
		{
		  m_ItemIds[ix]=val;
		  OnFieldUpdated();
		}
		public int GetItemIds(int ix)
		{
		  return m_ItemIds[ix];
		}
		public void AddItemIds(int val)
		{
		  m_ItemIds.Add(val);
		  OnFieldUpdated();
		}
		public void DelItemIds(int ix)
		{
		  m_ItemIds.RemoveAt(ix);
		  OnFieldUpdated();
		}
		public void VisitItemIds(MyAction<int> visit)
		{
		  foreach(int v in m_ItemIds){
		    visit(v);
		  }
		}
		public int[] ItemIdsToArray()
		{
		  return m_ItemIds.ToArray();
		}
		public int GetItemNumbersCount()
		{
		  return m_ItemNumbers.Count;
		}
		public void SetItemNumbers(int ix, int val)
		{
		  m_ItemNumbers[ix]=val;
		  OnFieldUpdated();
		}
		public int GetItemNumbers(int ix)
		{
		  return m_ItemNumbers[ix];
		}
		public void AddItemNumbers(int val)
		{
		  m_ItemNumbers.Add(val);
		  OnFieldUpdated();
		}
		public void DelItemNumbers(int ix)
		{
		  m_ItemNumbers.RemoveAt(ix);
		  OnFieldUpdated();
		}
		public void VisitItemNumbers(MyAction<int> visit)
		{
		  foreach(int v in m_ItemNumbers){
		    visit(v);
		  }
		}
		public int[] ItemNumbersToArray()
		{
		  return m_ItemNumbers.ToArray();
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
			m_TableMailInfo.ItemIds = DataProtoUtility.JoinGeneralList(",",m_ItemIds);
			m_TableMailInfo.ItemNumbers = DataProtoUtility.JoinGeneralList(",",m_ItemNumbers);
			return m_TableMailInfo;
		}
		public void FromProto(TableMailInfo proto)
		{
			m_TableMailInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_SendDate = DateTime.ParseExact(m_TableMailInfo.SendDate,"yyyyMMddHHmmss",null);
			m_ExpiryDate = DateTime.ParseExact(m_TableMailInfo.ExpiryDate,"yyyyMMddHHmmss",null);
			m_ItemIds = DataProtoUtility.SplitGeneralList<int>(new char[]{','}, m_TableMailInfo.ItemIds);
			m_ItemNumbers = DataProtoUtility.SplitGeneralList<int>(new char[]{','}, m_TableMailInfo.ItemNumbers);
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
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMailInfo m_TableMailInfo = new TableMailInfo();
		private DateTime m_SendDate = new DateTime();
		private DateTime m_ExpiryDate = new DateTime();
		private List<int> m_ItemIds = new List<int>();
		private List<int> m_ItemNumbers = new List<int>();

	}
}

namespace GameFramework
{
	public sealed partial class MailState
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
		}

		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public ulong MailGuid
		{
			get{return m_TableMailStateInfo.MailGuid;}
			set
			{
				m_TableMailStateInfo.MailGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableMailStateInfo.UserGuid;}
			set
			{
				m_TableMailStateInfo.UserGuid = value;
				OnForeignKeyUpdated();
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
		public bool IsDeleted
		{
			get{return m_TableMailStateInfo.IsDeleted;}
			set
			{
				m_TableMailStateInfo.IsDeleted = value;
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
			m_PrimaryKeys.Add(m_TableMailStateInfo.MailGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableMailStateInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMailStateInfo m_TableMailStateInfo = new TableMailStateInfo();
		private DateTime m_ExpiryDate = new DateTime();

	}
}

namespace GameFramework
{
	public sealed partial class MemberInfo
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
		}

		public List<string> PrimaryKeys
		{
			get{ return m_PrimaryKeys;}
		}
		public List<string> ForeignKeys
		{
			get{ return m_ForeignKeys;}
		}

		public ulong MemberGuid
		{
			get{return m_TableMemberInfo.MemberGuid;}
			set
			{
				m_TableMemberInfo.MemberGuid = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableMemberInfo.UserGuid;}
			set
			{
				m_TableMemberInfo.UserGuid = value;
				OnForeignKeyUpdated();
			}
		}
		public int HeroId
		{
			get{return m_TableMemberInfo.HeroId;}
			set
			{
				m_TableMemberInfo.HeroId = value;
				OnFieldUpdated();
			}
		}
		public int Level
		{
			get{return m_TableMemberInfo.Level;}
			set
			{
				m_TableMemberInfo.Level = value;
				OnFieldUpdated();
			}
		}

		public TableMemberInfo ToProto()
		{
			return m_TableMemberInfo;
		}
		public void FromProto(TableMemberInfo proto)
		{
			m_TableMemberInfo = proto;
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
			m_PrimaryKeys.Add(m_TableMemberInfo.MemberGuid.ToString());
		}

		private void UpdateForeignKeys()
		{
			m_ForeignKeys.Clear();
			m_ForeignKeys.Add(m_TableMemberInfo.UserGuid.ToString());
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableMemberInfo m_TableMemberInfo = new TableMemberInfo();

	}
}

namespace GameFramework
{
	public sealed partial class NicknameInfo
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
			get{return m_TableNicknameInfo.Nickname;}
			set
			{
				m_TableNicknameInfo.Nickname = value;
				OnPrimaryKeyUpdated();
			}
		}
		public ulong UserGuid
		{
			get{return m_TableNicknameInfo.UserGuid;}
			set
			{
				m_TableNicknameInfo.UserGuid = value;
				OnFieldUpdated();
			}
		}

		public TableNicknameInfo ToProto()
		{
			return m_TableNicknameInfo;
		}
		public void FromProto(TableNicknameInfo proto)
		{
			m_TableNicknameInfo = proto;
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
			if (m_TableNicknameInfo.Nickname != null) {
				m_PrimaryKeys.Add(m_TableNicknameInfo.Nickname.ToString());
			}
		}

		private void UpdateForeignKeys()
		{
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableNicknameInfo m_TableNicknameInfo = new TableNicknameInfo();

	}
}

namespace GameFramework
{
	public sealed partial class UserInfo
	{

		public bool Modified
		{
			get{ return m_Modified;}
			set{ m_Modified = value;}
		}
		public bool Deleted
		{
			get{ return m_Deleted;}
			set{ m_Deleted = value;}
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
		public DateTime LastLogoutTime
		{
			get{return m_LastLogoutTime;}
			set
			{
				m_LastLogoutTime = value;
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
		public int ExpPoints
		{
			get{return m_TableUserInfo.ExpPoints;}
			set
			{
				m_TableUserInfo.ExpPoints = value;
				OnFieldUpdated();
			}
		}
		public int SceneId
		{
			get{return m_TableUserInfo.SceneId;}
			set
			{
				m_TableUserInfo.SceneId = value;
				OnFieldUpdated();
			}
		}
		public float PositionX
		{
			get{return m_TableUserInfo.PositionX;}
			set
			{
				m_TableUserInfo.PositionX = value;
				OnFieldUpdated();
			}
		}
		public float PositionZ
		{
			get{return m_TableUserInfo.PositionZ;}
			set
			{
				m_TableUserInfo.PositionZ = value;
				OnFieldUpdated();
			}
		}
		public float FaceDir
		{
			get{return m_TableUserInfo.FaceDir;}
			set
			{
				m_TableUserInfo.FaceDir = value;
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
		public int SummonerSkillId
		{
			get{return m_TableUserInfo.SummonerSkillId;}
			set
			{
				m_TableUserInfo.SummonerSkillId = value;
				OnFieldUpdated();
			}
		}
		public Dictionary<string,int> IntDatas
		{
			get{return m_IntDatas;}
			set
			{
				m_IntDatas = value;
				OnFieldUpdated();
			}
		}
		public Dictionary<string,float> FloatDatas
		{
			get{return m_FloatDatas;}
			set
			{
				m_FloatDatas = value;
				OnFieldUpdated();
			}
		}
		public Dictionary<string,string> StringDatas
		{
			get{return m_StringDatas;}
			set
			{
				m_StringDatas = value;
				OnFieldUpdated();
			}
		}

		public TableUserInfo ToProto()
		{
			m_TableUserInfo.CreateTime = m_CreateTime.ToString("yyyyMMddHHmmss");
			m_TableUserInfo.LastLogoutTime = m_LastLogoutTime.ToString("yyyyMMddHHmmss");
			m_TableUserInfo.IntDatas = DataProtoUtility.JoinGeneralDictionary(";",",",m_IntDatas);
			m_TableUserInfo.FloatDatas = DataProtoUtility.JoinGeneralDictionary(";",",",m_FloatDatas);
			m_TableUserInfo.StringDatas = DataProtoUtility.JoinGeneralDictionary(";",",",m_StringDatas);
			return m_TableUserInfo;
		}
		public void FromProto(TableUserInfo proto)
		{
			m_TableUserInfo = proto;
			UpdatePrimaryKeys();
			UpdateForeignKeys();
			m_CreateTime = DateTime.ParseExact(m_TableUserInfo.CreateTime,"yyyyMMddHHmmss",null);
			m_LastLogoutTime = DateTime.ParseExact(m_TableUserInfo.LastLogoutTime,"yyyyMMddHHmmss",null);
			m_IntDatas = DataProtoUtility.SplitGeneralDictionary<string,int>(new char[]{';'},new char[]{','}, m_TableUserInfo.IntDatas);
			m_FloatDatas = DataProtoUtility.SplitGeneralDictionary<string,float>(new char[]{';'},new char[]{','}, m_TableUserInfo.FloatDatas);
			m_StringDatas = DataProtoUtility.SplitGeneralDictionary<string,string>(new char[]{';'},new char[]{','}, m_TableUserInfo.StringDatas);
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
			if (m_TableUserInfo.AccountId != null) {
				m_ForeignKeys.Add(m_TableUserInfo.AccountId.ToString());
			}
		}

		private bool m_Modified = false;
		private bool m_Deleted = false;
		private List<string> m_PrimaryKeys = new List<string>();
		private List<string> m_ForeignKeys = new List<string>();
		private TableUserInfo m_TableUserInfo = new TableUserInfo();
		private DateTime m_CreateTime = new DateTime();
		private DateTime m_LastLogoutTime = new DateTime();
		private Dictionary<string,int> m_IntDatas = new Dictionary<string,int>();
		private Dictionary<string,float> m_FloatDatas = new Dictionary<string,float>();
		private Dictionary<string,string> m_StringDatas = new Dictionary<string,string>();

	}
}
