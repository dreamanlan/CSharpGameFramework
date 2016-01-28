//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using GameFrameworkData;
using GameFramework;

namespace GameFramework
{
	internal static class DataDML
	{

		internal static void Save(int msgId, bool isValid, int dataVersion, byte[] data)
		{
			switch(msgId){
				case (int)DataEnum.TableAccount:
					SaveTableAccount(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableActivationCode:
					SaveTableActivationCode(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableArenaInfo:
					SaveTableArenaInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableArenaRecord:
					SaveTableArenaRecord(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableCorpsInfo:
					SaveTableCorpsInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableCorpsMember:
					SaveTableCorpsMember(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableEquipInfo:
					SaveTableEquipInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableExpeditionInfo:
					SaveTableExpeditionInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableFashionInfo:
					SaveTableFashionInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableFightInfo:
					SaveTableFightInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableFriendInfo:
					SaveTableFriendInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGlobalParam:
					SaveTableGlobalParam(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGowInfo:
					SaveTableGowInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGowStar:
					SaveTableGowStar(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGuid:
					SaveTableGuid(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableHomeNotice:
					SaveTableHomeNotice(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableInviterInfo:
					SaveTableInviterInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableItemInfo:
					SaveTableItemInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLegacyInfo:
					SaveTableLegacyInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLevelInfo:
					SaveTableLevelInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLoginLotteryRecord:
					SaveTableLoginLotteryRecord(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLootInfo:
					SaveTableLootInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLootRecord:
					SaveTableLootRecord(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableLotteryInfo:
					SaveTableLotteryInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMailInfo:
					SaveTableMailInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMailStateInfo:
					SaveTableMailStateInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMissionInfo:
					SaveTableMissionInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMpveAwardInfo:
					SaveTableMpveAwardInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableNickname:
					SaveTableNickname(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TablePartnerInfo:
					SaveTablePartnerInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TablePaymentInfo:
					SaveTablePaymentInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableSkillInfo:
					SaveTableSkillInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableTalentInfo:
					SaveTableTalentInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUndonePayment:
					SaveTableUndonePayment(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUserBattleInfo:
					SaveTableUserBattleInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUserGeneralInfo:
					SaveTableUserGeneralInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUserInfo:
					SaveTableUserInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUserSpecialInfo:
					SaveTableUserSpecialInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableXSoulInfo:
					SaveTableXSoulInfo(isValid, dataVersion, data);
					break;
			}
		}
		internal static List<GeneralRecordData> LoadAll(int msgId, int start, int count)
		{
			if(start<0)
				start=0;
			if(count<=0)
				count=int.MaxValue;
			switch(msgId){
				case (int)DataEnum.TableAccount:
					return LoadAllTableAccount(start, count);
				case (int)DataEnum.TableActivationCode:
					return LoadAllTableActivationCode(start, count);
				case (int)DataEnum.TableArenaInfo:
					return LoadAllTableArenaInfo(start, count);
				case (int)DataEnum.TableArenaRecord:
					return LoadAllTableArenaRecord(start, count);
				case (int)DataEnum.TableCorpsInfo:
					return LoadAllTableCorpsInfo(start, count);
				case (int)DataEnum.TableCorpsMember:
					return LoadAllTableCorpsMember(start, count);
				case (int)DataEnum.TableEquipInfo:
					return LoadAllTableEquipInfo(start, count);
				case (int)DataEnum.TableExpeditionInfo:
					return LoadAllTableExpeditionInfo(start, count);
				case (int)DataEnum.TableFashionInfo:
					return LoadAllTableFashionInfo(start, count);
				case (int)DataEnum.TableFightInfo:
					return LoadAllTableFightInfo(start, count);
				case (int)DataEnum.TableFriendInfo:
					return LoadAllTableFriendInfo(start, count);
				case (int)DataEnum.TableGlobalParam:
					return LoadAllTableGlobalParam(start, count);
				case (int)DataEnum.TableGowInfo:
					return LoadAllTableGowInfo(start, count);
				case (int)DataEnum.TableGowStar:
					return LoadAllTableGowStar(start, count);
				case (int)DataEnum.TableGuid:
					return LoadAllTableGuid(start, count);
				case (int)DataEnum.TableHomeNotice:
					return LoadAllTableHomeNotice(start, count);
				case (int)DataEnum.TableInviterInfo:
					return LoadAllTableInviterInfo(start, count);
				case (int)DataEnum.TableItemInfo:
					return LoadAllTableItemInfo(start, count);
				case (int)DataEnum.TableLegacyInfo:
					return LoadAllTableLegacyInfo(start, count);
				case (int)DataEnum.TableLevelInfo:
					return LoadAllTableLevelInfo(start, count);
				case (int)DataEnum.TableLoginLotteryRecord:
					return LoadAllTableLoginLotteryRecord(start, count);
				case (int)DataEnum.TableLootInfo:
					return LoadAllTableLootInfo(start, count);
				case (int)DataEnum.TableLootRecord:
					return LoadAllTableLootRecord(start, count);
				case (int)DataEnum.TableLotteryInfo:
					return LoadAllTableLotteryInfo(start, count);
				case (int)DataEnum.TableMailInfo:
					return LoadAllTableMailInfo(start, count);
				case (int)DataEnum.TableMailStateInfo:
					return LoadAllTableMailStateInfo(start, count);
				case (int)DataEnum.TableMissionInfo:
					return LoadAllTableMissionInfo(start, count);
				case (int)DataEnum.TableMpveAwardInfo:
					return LoadAllTableMpveAwardInfo(start, count);
				case (int)DataEnum.TableNickname:
					return LoadAllTableNickname(start, count);
				case (int)DataEnum.TablePartnerInfo:
					return LoadAllTablePartnerInfo(start, count);
				case (int)DataEnum.TablePaymentInfo:
					return LoadAllTablePaymentInfo(start, count);
				case (int)DataEnum.TableSkillInfo:
					return LoadAllTableSkillInfo(start, count);
				case (int)DataEnum.TableTalentInfo:
					return LoadAllTableTalentInfo(start, count);
				case (int)DataEnum.TableUndonePayment:
					return LoadAllTableUndonePayment(start, count);
				case (int)DataEnum.TableUserBattleInfo:
					return LoadAllTableUserBattleInfo(start, count);
				case (int)DataEnum.TableUserGeneralInfo:
					return LoadAllTableUserGeneralInfo(start, count);
				case (int)DataEnum.TableUserInfo:
					return LoadAllTableUserInfo(start, count);
				case (int)DataEnum.TableUserSpecialInfo:
					return LoadAllTableUserSpecialInfo(start, count);
				case (int)DataEnum.TableXSoulInfo:
					return LoadAllTableXSoulInfo(start, count);
			}
			return null;
		}
		internal static GeneralRecordData LoadSingle(int msgId, List<string> primaryKeys)
		{
			switch(msgId){
				case (int)DataEnum.TableAccount:
					return LoadSingleTableAccount(primaryKeys);
				case (int)DataEnum.TableActivationCode:
					return LoadSingleTableActivationCode(primaryKeys);
				case (int)DataEnum.TableArenaInfo:
					return LoadSingleTableArenaInfo(primaryKeys);
				case (int)DataEnum.TableArenaRecord:
					return LoadSingleTableArenaRecord(primaryKeys);
				case (int)DataEnum.TableCorpsInfo:
					return LoadSingleTableCorpsInfo(primaryKeys);
				case (int)DataEnum.TableCorpsMember:
					return LoadSingleTableCorpsMember(primaryKeys);
				case (int)DataEnum.TableEquipInfo:
					return LoadSingleTableEquipInfo(primaryKeys);
				case (int)DataEnum.TableExpeditionInfo:
					return LoadSingleTableExpeditionInfo(primaryKeys);
				case (int)DataEnum.TableFashionInfo:
					return LoadSingleTableFashionInfo(primaryKeys);
				case (int)DataEnum.TableFightInfo:
					return LoadSingleTableFightInfo(primaryKeys);
				case (int)DataEnum.TableFriendInfo:
					return LoadSingleTableFriendInfo(primaryKeys);
				case (int)DataEnum.TableGlobalParam:
					return LoadSingleTableGlobalParam(primaryKeys);
				case (int)DataEnum.TableGowInfo:
					return LoadSingleTableGowInfo(primaryKeys);
				case (int)DataEnum.TableGowStar:
					return LoadSingleTableGowStar(primaryKeys);
				case (int)DataEnum.TableGuid:
					return LoadSingleTableGuid(primaryKeys);
				case (int)DataEnum.TableHomeNotice:
					return LoadSingleTableHomeNotice(primaryKeys);
				case (int)DataEnum.TableInviterInfo:
					return LoadSingleTableInviterInfo(primaryKeys);
				case (int)DataEnum.TableItemInfo:
					return LoadSingleTableItemInfo(primaryKeys);
				case (int)DataEnum.TableLegacyInfo:
					return LoadSingleTableLegacyInfo(primaryKeys);
				case (int)DataEnum.TableLevelInfo:
					return LoadSingleTableLevelInfo(primaryKeys);
				case (int)DataEnum.TableLoginLotteryRecord:
					return LoadSingleTableLoginLotteryRecord(primaryKeys);
				case (int)DataEnum.TableLootInfo:
					return LoadSingleTableLootInfo(primaryKeys);
				case (int)DataEnum.TableLootRecord:
					return LoadSingleTableLootRecord(primaryKeys);
				case (int)DataEnum.TableLotteryInfo:
					return LoadSingleTableLotteryInfo(primaryKeys);
				case (int)DataEnum.TableMailInfo:
					return LoadSingleTableMailInfo(primaryKeys);
				case (int)DataEnum.TableMailStateInfo:
					return LoadSingleTableMailStateInfo(primaryKeys);
				case (int)DataEnum.TableMissionInfo:
					return LoadSingleTableMissionInfo(primaryKeys);
				case (int)DataEnum.TableMpveAwardInfo:
					return LoadSingleTableMpveAwardInfo(primaryKeys);
				case (int)DataEnum.TableNickname:
					return LoadSingleTableNickname(primaryKeys);
				case (int)DataEnum.TablePartnerInfo:
					return LoadSingleTablePartnerInfo(primaryKeys);
				case (int)DataEnum.TablePaymentInfo:
					return LoadSingleTablePaymentInfo(primaryKeys);
				case (int)DataEnum.TableSkillInfo:
					return LoadSingleTableSkillInfo(primaryKeys);
				case (int)DataEnum.TableTalentInfo:
					return LoadSingleTableTalentInfo(primaryKeys);
				case (int)DataEnum.TableUndonePayment:
					return LoadSingleTableUndonePayment(primaryKeys);
				case (int)DataEnum.TableUserBattleInfo:
					return LoadSingleTableUserBattleInfo(primaryKeys);
				case (int)DataEnum.TableUserGeneralInfo:
					return LoadSingleTableUserGeneralInfo(primaryKeys);
				case (int)DataEnum.TableUserInfo:
					return LoadSingleTableUserInfo(primaryKeys);
				case (int)DataEnum.TableUserSpecialInfo:
					return LoadSingleTableUserSpecialInfo(primaryKeys);
				case (int)DataEnum.TableXSoulInfo:
					return LoadSingleTableXSoulInfo(primaryKeys);
			}
			return null;
		}
		internal static List<GeneralRecordData> LoadMulti(int msgId, List<string> foreignKeys)
		{
			switch(msgId){
				case (int)DataEnum.TableAccount:
					return LoadMultiTableAccount(foreignKeys);
				case (int)DataEnum.TableActivationCode:
					return LoadMultiTableActivationCode(foreignKeys);
				case (int)DataEnum.TableArenaInfo:
					return LoadMultiTableArenaInfo(foreignKeys);
				case (int)DataEnum.TableArenaRecord:
					return LoadMultiTableArenaRecord(foreignKeys);
				case (int)DataEnum.TableCorpsInfo:
					return LoadMultiTableCorpsInfo(foreignKeys);
				case (int)DataEnum.TableCorpsMember:
					return LoadMultiTableCorpsMember(foreignKeys);
				case (int)DataEnum.TableEquipInfo:
					return LoadMultiTableEquipInfo(foreignKeys);
				case (int)DataEnum.TableExpeditionInfo:
					return LoadMultiTableExpeditionInfo(foreignKeys);
				case (int)DataEnum.TableFashionInfo:
					return LoadMultiTableFashionInfo(foreignKeys);
				case (int)DataEnum.TableFightInfo:
					return LoadMultiTableFightInfo(foreignKeys);
				case (int)DataEnum.TableFriendInfo:
					return LoadMultiTableFriendInfo(foreignKeys);
				case (int)DataEnum.TableGlobalParam:
					return LoadMultiTableGlobalParam(foreignKeys);
				case (int)DataEnum.TableGowInfo:
					return LoadMultiTableGowInfo(foreignKeys);
				case (int)DataEnum.TableGowStar:
					return LoadMultiTableGowStar(foreignKeys);
				case (int)DataEnum.TableGuid:
					return LoadMultiTableGuid(foreignKeys);
				case (int)DataEnum.TableHomeNotice:
					return LoadMultiTableHomeNotice(foreignKeys);
				case (int)DataEnum.TableInviterInfo:
					return LoadMultiTableInviterInfo(foreignKeys);
				case (int)DataEnum.TableItemInfo:
					return LoadMultiTableItemInfo(foreignKeys);
				case (int)DataEnum.TableLegacyInfo:
					return LoadMultiTableLegacyInfo(foreignKeys);
				case (int)DataEnum.TableLevelInfo:
					return LoadMultiTableLevelInfo(foreignKeys);
				case (int)DataEnum.TableLoginLotteryRecord:
					return LoadMultiTableLoginLotteryRecord(foreignKeys);
				case (int)DataEnum.TableLootInfo:
					return LoadMultiTableLootInfo(foreignKeys);
				case (int)DataEnum.TableLootRecord:
					return LoadMultiTableLootRecord(foreignKeys);
				case (int)DataEnum.TableLotteryInfo:
					return LoadMultiTableLotteryInfo(foreignKeys);
				case (int)DataEnum.TableMailInfo:
					return LoadMultiTableMailInfo(foreignKeys);
				case (int)DataEnum.TableMailStateInfo:
					return LoadMultiTableMailStateInfo(foreignKeys);
				case (int)DataEnum.TableMissionInfo:
					return LoadMultiTableMissionInfo(foreignKeys);
				case (int)DataEnum.TableMpveAwardInfo:
					return LoadMultiTableMpveAwardInfo(foreignKeys);
				case (int)DataEnum.TableNickname:
					return LoadMultiTableNickname(foreignKeys);
				case (int)DataEnum.TablePartnerInfo:
					return LoadMultiTablePartnerInfo(foreignKeys);
				case (int)DataEnum.TablePaymentInfo:
					return LoadMultiTablePaymentInfo(foreignKeys);
				case (int)DataEnum.TableSkillInfo:
					return LoadMultiTableSkillInfo(foreignKeys);
				case (int)DataEnum.TableTalentInfo:
					return LoadMultiTableTalentInfo(foreignKeys);
				case (int)DataEnum.TableUndonePayment:
					return LoadMultiTableUndonePayment(foreignKeys);
				case (int)DataEnum.TableUserBattleInfo:
					return LoadMultiTableUserBattleInfo(foreignKeys);
				case (int)DataEnum.TableUserGeneralInfo:
					return LoadMultiTableUserGeneralInfo(foreignKeys);
				case (int)DataEnum.TableUserInfo:
					return LoadMultiTableUserInfo(foreignKeys);
				case (int)DataEnum.TableUserSpecialInfo:
					return LoadMultiTableUserSpecialInfo(foreignKeys);
				case (int)DataEnum.TableXSoulInfo:
					return LoadMultiTableXSoulInfo(foreignKeys);
			}
			return null;
		}

		private static List<GeneralRecordData> LoadAllTableAccount(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableAccount";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableAccount msg = new TableAccount();
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["IsBanned"];
			        msg.IsBanned = (bool)val;
			        val = reader["UserGuid1"];
			        msg.UserGuid1 = (ulong)val;
			        val = reader["UserGuid2"];
			        msg.UserGuid2 = (ulong)val;
			        val = reader["UserGuid3"];
			        msg.UserGuid3 = (ulong)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableAccount(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableAccount";
			    if(primaryKeys.Count != 2)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (int)Convert.ChangeType(primaryKeys[0],typeof(int));
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[1];
			    inputParam.Size = 64;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableAccount msg = new TableAccount();
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["IsBanned"];
			        msg.IsBanned = (bool)val;
			        val = reader["UserGuid1"];
			        msg.UserGuid1 = (ulong)val;
			        val = reader["UserGuid2"];
			        msg.UserGuid2 = (ulong)val;
			        val = reader["UserGuid3"];
			        msg.UserGuid3 = (ulong)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableAccount(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableAccount(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableAccount), out _msg)){
				TableAccount msg = _msg as TableAccount;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableAccount";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AccountId;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsBanned", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsBanned;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid1", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid1;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid2", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid2;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid3", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid3;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableActivationCode(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableActivationCode";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableActivationCode msg = new TableActivationCode();
			        val = reader["ActivationCode"];
			        msg.ActivationCode = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["IsActivated"];
			        msg.IsActivated = (bool)val;
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableActivationCode(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableActivationCode";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_ActivationCode", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 32;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableActivationCode msg = new TableActivationCode();
			        val = reader["ActivationCode"];
			        msg.ActivationCode = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["IsActivated"];
			        msg.IsActivated = (bool)val;
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableActivationCode(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableActivationCode(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableActivationCode), out _msg)){
				TableActivationCode msg = _msg as TableActivationCode;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableActivationCode";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActivationCode", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActivationCode;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsActivated", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsActivated;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AccountId;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableArenaInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableArenaInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableArenaInfo msg = new TableArenaInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["IsRobot"];
			        msg.IsRobot = (bool)val;
			        val = reader["ArenaBytes"];
			        msg.ArenaBytes = (byte[])val;
			        val = reader["LastBattleTime"];
			        msg.LastBattleTime = (string)val;
			        val = reader["LeftFightCount"];
			        msg.LeftFightCount = (int)val;
			        val = reader["BuyFightCount"];
			        msg.BuyFightCount = (int)val;
			        val = reader["LastResetFightCountTime"];
			        msg.LastResetFightCountTime = (string)val;
			        val = reader["ArenaHistroyTimeList"];
			        msg.ArenaHistroyTimeList = (string)val;
			        val = reader["ArenaHistroyRankList"];
			        msg.ArenaHistroyRankList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableArenaInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableArenaInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableArenaInfo msg = new TableArenaInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["IsRobot"];
			        msg.IsRobot = (bool)val;
			        val = reader["ArenaBytes"];
			        msg.ArenaBytes = (byte[])val;
			        val = reader["LastBattleTime"];
			        msg.LastBattleTime = (string)val;
			        val = reader["LeftFightCount"];
			        msg.LeftFightCount = (int)val;
			        val = reader["BuyFightCount"];
			        msg.BuyFightCount = (int)val;
			        val = reader["LastResetFightCountTime"];
			        msg.LastResetFightCountTime = (string)val;
			        val = reader["ArenaHistroyTimeList"];
			        msg.ArenaHistroyTimeList = (string)val;
			        val = reader["ArenaHistroyRankList"];
			        msg.ArenaHistroyRankList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableArenaInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableArenaInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableArenaInfo), out _msg)){
				TableArenaInfo msg = _msg as TableArenaInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableArenaInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsRobot", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsRobot;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ArenaBytes", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ArenaBytes;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastBattleTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastBattleTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LeftFightCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LeftFightCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BuyFightCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BuyFightCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetFightCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetFightCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ArenaHistroyTimeList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ArenaHistroyTimeList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ArenaHistroyRankList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ArenaHistroyRankList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableArenaRecord(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableArenaRecord";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableArenaRecord msg = new TableArenaRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["IsChallengerSuccess"];
			        msg.IsChallengerSuccess = (bool)val;
			        val = reader["BeginTime"];
			        msg.BeginTime = (string)val;
			        val = reader["EndTime"];
			        msg.EndTime = (string)val;
			        val = reader["CGuid"];
			        msg.CGuid = (long)val;
			        val = reader["CHeroId"];
			        msg.CHeroId = (int)val;
			        val = reader["CLevel"];
			        msg.CLevel = (int)val;
			        val = reader["CFightScore"];
			        msg.CFightScore = (int)val;
			        val = reader["CNickname"];
			        msg.CNickname = (string)val;
			        val = reader["CRank"];
			        msg.CRank = (int)val;
			        val = reader["CUserDamage"];
			        msg.CUserDamage = (int)val;
			        val = reader["CPartnerId1"];
			        msg.CPartnerId1 = (int)val;
			        val = reader["CPartnerDamage1"];
			        msg.CPartnerDamage1 = (int)val;
			        val = reader["CPartnerId2"];
			        msg.CPartnerId2 = (int)val;
			        val = reader["CPartnerDamage2"];
			        msg.CPartnerDamage2 = (int)val;
			        val = reader["CPartnerId3"];
			        msg.CPartnerId3 = (int)val;
			        val = reader["CPartnerDamage3"];
			        msg.CPartnerDamage3 = (int)val;
			        val = reader["TGuid"];
			        msg.TGuid = (long)val;
			        val = reader["THeroId"];
			        msg.THeroId = (int)val;
			        val = reader["TLevel"];
			        msg.TLevel = (int)val;
			        val = reader["TFightScore"];
			        msg.TFightScore = (int)val;
			        val = reader["TNickname"];
			        msg.TNickname = (string)val;
			        val = reader["TRank"];
			        msg.TRank = (int)val;
			        val = reader["TUserDamage"];
			        msg.TUserDamage = (int)val;
			        val = reader["TPartnerId1"];
			        msg.TPartnerId1 = (int)val;
			        val = reader["TPartnerDamage1"];
			        msg.TPartnerDamage1 = (int)val;
			        val = reader["TPartnerId2"];
			        msg.TPartnerId2 = (int)val;
			        val = reader["TPartnerDamage2"];
			        msg.TPartnerDamage2 = (int)val;
			        val = reader["TPartnerId3"];
			        msg.TPartnerId3 = (int)val;
			        val = reader["TPartnerDamage3"];
			        msg.TPartnerDamage3 = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableArenaRecord(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableArenaRecord";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableArenaRecord msg = new TableArenaRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["IsChallengerSuccess"];
			        msg.IsChallengerSuccess = (bool)val;
			        val = reader["BeginTime"];
			        msg.BeginTime = (string)val;
			        val = reader["EndTime"];
			        msg.EndTime = (string)val;
			        val = reader["CGuid"];
			        msg.CGuid = (long)val;
			        val = reader["CHeroId"];
			        msg.CHeroId = (int)val;
			        val = reader["CLevel"];
			        msg.CLevel = (int)val;
			        val = reader["CFightScore"];
			        msg.CFightScore = (int)val;
			        val = reader["CNickname"];
			        msg.CNickname = (string)val;
			        val = reader["CRank"];
			        msg.CRank = (int)val;
			        val = reader["CUserDamage"];
			        msg.CUserDamage = (int)val;
			        val = reader["CPartnerId1"];
			        msg.CPartnerId1 = (int)val;
			        val = reader["CPartnerDamage1"];
			        msg.CPartnerDamage1 = (int)val;
			        val = reader["CPartnerId2"];
			        msg.CPartnerId2 = (int)val;
			        val = reader["CPartnerDamage2"];
			        msg.CPartnerDamage2 = (int)val;
			        val = reader["CPartnerId3"];
			        msg.CPartnerId3 = (int)val;
			        val = reader["CPartnerDamage3"];
			        msg.CPartnerDamage3 = (int)val;
			        val = reader["TGuid"];
			        msg.TGuid = (long)val;
			        val = reader["THeroId"];
			        msg.THeroId = (int)val;
			        val = reader["TLevel"];
			        msg.TLevel = (int)val;
			        val = reader["TFightScore"];
			        msg.TFightScore = (int)val;
			        val = reader["TNickname"];
			        msg.TNickname = (string)val;
			        val = reader["TRank"];
			        msg.TRank = (int)val;
			        val = reader["TUserDamage"];
			        msg.TUserDamage = (int)val;
			        val = reader["TPartnerId1"];
			        msg.TPartnerId1 = (int)val;
			        val = reader["TPartnerDamage1"];
			        msg.TPartnerDamage1 = (int)val;
			        val = reader["TPartnerId2"];
			        msg.TPartnerId2 = (int)val;
			        val = reader["TPartnerDamage2"];
			        msg.TPartnerDamage2 = (int)val;
			        val = reader["TPartnerId3"];
			        msg.TPartnerId3 = (int)val;
			        val = reader["TPartnerDamage3"];
			        msg.TPartnerDamage3 = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableArenaRecord(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableArenaRecord";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableArenaRecord msg = new TableArenaRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["IsChallengerSuccess"];
			        msg.IsChallengerSuccess = (bool)val;
			        val = reader["BeginTime"];
			        msg.BeginTime = (string)val;
			        val = reader["EndTime"];
			        msg.EndTime = (string)val;
			        val = reader["CGuid"];
			        msg.CGuid = (long)val;
			        val = reader["CHeroId"];
			        msg.CHeroId = (int)val;
			        val = reader["CLevel"];
			        msg.CLevel = (int)val;
			        val = reader["CFightScore"];
			        msg.CFightScore = (int)val;
			        val = reader["CNickname"];
			        msg.CNickname = (string)val;
			        val = reader["CRank"];
			        msg.CRank = (int)val;
			        val = reader["CUserDamage"];
			        msg.CUserDamage = (int)val;
			        val = reader["CPartnerId1"];
			        msg.CPartnerId1 = (int)val;
			        val = reader["CPartnerDamage1"];
			        msg.CPartnerDamage1 = (int)val;
			        val = reader["CPartnerId2"];
			        msg.CPartnerId2 = (int)val;
			        val = reader["CPartnerDamage2"];
			        msg.CPartnerDamage2 = (int)val;
			        val = reader["CPartnerId3"];
			        msg.CPartnerId3 = (int)val;
			        val = reader["CPartnerDamage3"];
			        msg.CPartnerDamage3 = (int)val;
			        val = reader["TGuid"];
			        msg.TGuid = (long)val;
			        val = reader["THeroId"];
			        msg.THeroId = (int)val;
			        val = reader["TLevel"];
			        msg.TLevel = (int)val;
			        val = reader["TFightScore"];
			        msg.TFightScore = (int)val;
			        val = reader["TNickname"];
			        msg.TNickname = (string)val;
			        val = reader["TRank"];
			        msg.TRank = (int)val;
			        val = reader["TUserDamage"];
			        msg.TUserDamage = (int)val;
			        val = reader["TPartnerId1"];
			        msg.TPartnerId1 = (int)val;
			        val = reader["TPartnerDamage1"];
			        msg.TPartnerDamage1 = (int)val;
			        val = reader["TPartnerId2"];
			        msg.TPartnerId2 = (int)val;
			        val = reader["TPartnerDamage2"];
			        msg.TPartnerDamage2 = (int)val;
			        val = reader["TPartnerId3"];
			        msg.TPartnerId3 = (int)val;
			        val = reader["TPartnerDamage3"];
			        msg.TPartnerDamage3 = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableArenaRecord(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableArenaRecord), out _msg)){
				TableArenaRecord msg = _msg as TableArenaRecord;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableArenaRecord";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsChallengerSuccess", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsChallengerSuccess;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BeginTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BeginTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EndTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EndTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CHeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CHeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CFightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CFightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CNickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CNickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CRank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CRank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CUserDamage", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CUserDamage;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerId1", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerId1;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerDamage1", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerDamage1;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerId2", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerId2;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerDamage2", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerDamage2;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerId3", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerId3;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CPartnerDamage3", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CPartnerDamage3;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_THeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.THeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TFightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TFightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TNickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TNickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TRank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TRank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TUserDamage", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TUserDamage;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerId1", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerId1;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerDamage1", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerDamage1;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerId2", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerId2;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerDamage2", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerDamage2;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerId3", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerId3;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TPartnerDamage3", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TPartnerDamage3;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableCorpsInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableCorpsInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableCorpsInfo msg = new TableCorpsInfo();
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["CorpsName"];
			        msg.CorpsName = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Score"];
			        msg.Score = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["Activeness"];
			        msg.Activeness = (int)val;
			        val = reader["Notice"];
			        msg.Notice = (string)val;
			        val = reader["LastResetActivenessTime"];
			        msg.LastResetActivenessTime = (string)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["TollgateBytes"];
			        msg.TollgateBytes = (byte[])val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableCorpsInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableCorpsInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_CorpsGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableCorpsInfo msg = new TableCorpsInfo();
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["CorpsName"];
			        msg.CorpsName = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Score"];
			        msg.Score = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["Activeness"];
			        msg.Activeness = (int)val;
			        val = reader["Notice"];
			        msg.Notice = (string)val;
			        val = reader["LastResetActivenessTime"];
			        msg.LastResetActivenessTime = (string)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["TollgateBytes"];
			        msg.TollgateBytes = (byte[])val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableCorpsInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableCorpsInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableCorpsInfo), out _msg)){
				TableCorpsInfo msg = _msg as TableCorpsInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableCorpsInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsName", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsName;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Score", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Score;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Activeness", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Activeness;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Notice", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Notice;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetActivenessTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetActivenessTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CreateTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CreateTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TollgateBytes", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TollgateBytes;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableCorpsMember(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableCorpsMember";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableCorpsMember msg = new TableCorpsMember();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Title"];
			        msg.Title = (int)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightScore"];
			        msg.FightScore = (int)val;
			        val = reader["DayActiveness"];
			        msg.DayActiveness = (int)val;
			        val = reader["WeekActiveness"];
			        msg.WeekActiveness = (int)val;
			        val = reader["ActivenessHistoryDate"];
			        msg.ActivenessHistoryDate = (string)val;
			        val = reader["ActivenessHistoryValue"];
			        msg.ActivenessHistoryValue = (string)val;
			        val = reader["LastLoginTime"];
			        msg.LastLoginTime = (long)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableCorpsMember(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableCorpsMember";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableCorpsMember msg = new TableCorpsMember();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Title"];
			        msg.Title = (int)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightScore"];
			        msg.FightScore = (int)val;
			        val = reader["DayActiveness"];
			        msg.DayActiveness = (int)val;
			        val = reader["WeekActiveness"];
			        msg.WeekActiveness = (int)val;
			        val = reader["ActivenessHistoryDate"];
			        msg.ActivenessHistoryDate = (string)val;
			        val = reader["ActivenessHistoryValue"];
			        msg.ActivenessHistoryValue = (string)val;
			        val = reader["LastLoginTime"];
			        msg.LastLoginTime = (long)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableCorpsMember(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableCorpsMember";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_CorpsGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableCorpsMember msg = new TableCorpsMember();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Title"];
			        msg.Title = (int)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightScore"];
			        msg.FightScore = (int)val;
			        val = reader["DayActiveness"];
			        msg.DayActiveness = (int)val;
			        val = reader["WeekActiveness"];
			        msg.WeekActiveness = (int)val;
			        val = reader["ActivenessHistoryDate"];
			        msg.ActivenessHistoryDate = (string)val;
			        val = reader["ActivenessHistoryValue"];
			        msg.ActivenessHistoryValue = (string)val;
			        val = reader["LastLoginTime"];
			        msg.LastLoginTime = (long)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableCorpsMember(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableCorpsMember), out _msg)){
				TableCorpsMember msg = _msg as TableCorpsMember;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableCorpsMember";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Title", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Title;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DayActiveness", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DayActiveness;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_WeekActiveness", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.WeekActiveness;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActivenessHistoryDate", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActivenessHistoryDate;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActivenessHistoryValue", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActivenessHistoryValue;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastLoginTime", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastLoginTime;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableEquipInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableEquipInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableEquipInfo msg = new TableEquipInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        val = reader["StrengthLevel"];
			        msg.StrengthLevel = (int)val;
			        val = reader["FailCount"];
			        msg.FailCount = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableEquipInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableEquipInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableEquipInfo msg = new TableEquipInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        val = reader["StrengthLevel"];
			        msg.StrengthLevel = (int)val;
			        val = reader["FailCount"];
			        msg.FailCount = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableEquipInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableEquipInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableEquipInfo msg = new TableEquipInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        val = reader["StrengthLevel"];
			        msg.StrengthLevel = (int)val;
			        val = reader["FailCount"];
			        msg.FailCount = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableEquipInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableEquipInfo), out _msg)){
				TableEquipInfo msg = _msg as TableEquipInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableEquipInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Position", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Position;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AppendProperty", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AppendProperty;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EnhanceStarLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EnhanceStarLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_StrengthLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.StrengthLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FailCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FailCount;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableExpeditionInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableExpeditionInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableExpeditionInfo msg = new TableExpeditionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["HP"];
			        msg.HP = (int)val;
			        val = reader["MP"];
			        msg.MP = (int)val;
			        val = reader["Rage"];
			        msg.Rage = (int)val;
			        val = reader["Schedule"];
			        msg.Schedule = (int)val;
			        val = reader["MonsterCount"];
			        msg.MonsterCount = (int)val;
			        val = reader["BossCount"];
			        msg.BossCount = (int)val;
			        val = reader["OnePlayerCount"];
			        msg.OnePlayerCount = (int)val;
			        val = reader["Unrewarded"];
			        msg.Unrewarded = (string)val;
			        val = reader["TollgateType"];
			        msg.TollgateType = (int)val;
			        val = reader["EnemyList"];
			        msg.EnemyList = (string)val;
			        val = reader["EnemyAttrList"];
			        msg.EnemyAttrList = (string)val;
			        val = reader["ImageA"];
			        msg.ImageA = (byte[])val;
			        val = reader["ImageB"];
			        msg.ImageB = (byte[])val;
			        val = reader["ResetCount"];
			        msg.ResetCount = (int)val;
			        val = reader["PartnerIdList"];
			        msg.PartnerIdList = (string)val;
			        val = reader["PartnerHpList"];
			        msg.PartnerHpList = (string)val;
			        val = reader["LastAchievedSchedule"];
			        msg.LastAchievedSchedule = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableExpeditionInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableExpeditionInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableExpeditionInfo msg = new TableExpeditionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["HP"];
			        msg.HP = (int)val;
			        val = reader["MP"];
			        msg.MP = (int)val;
			        val = reader["Rage"];
			        msg.Rage = (int)val;
			        val = reader["Schedule"];
			        msg.Schedule = (int)val;
			        val = reader["MonsterCount"];
			        msg.MonsterCount = (int)val;
			        val = reader["BossCount"];
			        msg.BossCount = (int)val;
			        val = reader["OnePlayerCount"];
			        msg.OnePlayerCount = (int)val;
			        val = reader["Unrewarded"];
			        msg.Unrewarded = (string)val;
			        val = reader["TollgateType"];
			        msg.TollgateType = (int)val;
			        val = reader["EnemyList"];
			        msg.EnemyList = (string)val;
			        val = reader["EnemyAttrList"];
			        msg.EnemyAttrList = (string)val;
			        val = reader["ImageA"];
			        msg.ImageA = (byte[])val;
			        val = reader["ImageB"];
			        msg.ImageB = (byte[])val;
			        val = reader["ResetCount"];
			        msg.ResetCount = (int)val;
			        val = reader["PartnerIdList"];
			        msg.PartnerIdList = (string)val;
			        val = reader["PartnerHpList"];
			        msg.PartnerHpList = (string)val;
			        val = reader["LastAchievedSchedule"];
			        msg.LastAchievedSchedule = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableExpeditionInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableExpeditionInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableExpeditionInfo msg = new TableExpeditionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["HP"];
			        msg.HP = (int)val;
			        val = reader["MP"];
			        msg.MP = (int)val;
			        val = reader["Rage"];
			        msg.Rage = (int)val;
			        val = reader["Schedule"];
			        msg.Schedule = (int)val;
			        val = reader["MonsterCount"];
			        msg.MonsterCount = (int)val;
			        val = reader["BossCount"];
			        msg.BossCount = (int)val;
			        val = reader["OnePlayerCount"];
			        msg.OnePlayerCount = (int)val;
			        val = reader["Unrewarded"];
			        msg.Unrewarded = (string)val;
			        val = reader["TollgateType"];
			        msg.TollgateType = (int)val;
			        val = reader["EnemyList"];
			        msg.EnemyList = (string)val;
			        val = reader["EnemyAttrList"];
			        msg.EnemyAttrList = (string)val;
			        val = reader["ImageA"];
			        msg.ImageA = (byte[])val;
			        val = reader["ImageB"];
			        msg.ImageB = (byte[])val;
			        val = reader["ResetCount"];
			        msg.ResetCount = (int)val;
			        val = reader["PartnerIdList"];
			        msg.PartnerIdList = (string)val;
			        val = reader["PartnerHpList"];
			        msg.PartnerHpList = (string)val;
			        val = reader["LastAchievedSchedule"];
			        msg.LastAchievedSchedule = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableExpeditionInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableExpeditionInfo), out _msg)){
				TableExpeditionInfo msg = _msg as TableExpeditionInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableExpeditionInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightingScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightingScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HP", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HP;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MP", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MP;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rage", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rage;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Schedule", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Schedule;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MonsterCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MonsterCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BossCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BossCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_OnePlayerCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.OnePlayerCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Unrewarded", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Unrewarded;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TollgateType", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TollgateType;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EnemyList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EnemyList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EnemyAttrList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EnemyAttrList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ImageA", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ImageA;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ImageB", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ImageB;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ResetCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ResetCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerIdList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerIdList;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerHpList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerHpList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastAchievedSchedule", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastAchievedSchedule;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableFashionInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableFashionInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableFashionInfo msg = new TableFashionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["IsForever"];
			        msg.IsForever = (bool)val;
			        val = reader["ExpirationTime"];
			        msg.ExpirationTime = (string)val;
			        val = reader["LastNoticeTime"];
			        msg.LastNoticeTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableFashionInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableFashionInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableFashionInfo msg = new TableFashionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["IsForever"];
			        msg.IsForever = (bool)val;
			        val = reader["ExpirationTime"];
			        msg.ExpirationTime = (string)val;
			        val = reader["LastNoticeTime"];
			        msg.LastNoticeTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableFashionInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableFashionInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableFashionInfo msg = new TableFashionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["IsForever"];
			        msg.IsForever = (bool)val;
			        val = reader["ExpirationTime"];
			        msg.ExpirationTime = (string)val;
			        val = reader["LastNoticeTime"];
			        msg.LastNoticeTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableFashionInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableFashionInfo), out _msg)){
				TableFashionInfo msg = _msg as TableFashionInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableFashionInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Position", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Position;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsForever", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsForever;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExpirationTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExpirationTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastNoticeTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastNoticeTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableFightInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableFightInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableFightInfo msg = new TableFightInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableFightInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableFightInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 16;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableFightInfo msg = new TableFightInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableFightInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableFightInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableFightInfo), out _msg)){
				TableFightInfo msg = _msg as TableFightInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableFightInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 16;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightingScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightingScore;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableFriendInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableFriendInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableFriendInfo msg = new TableFriendInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["FriendGuid"];
			        msg.FriendGuid = (long)val;
			        val = reader["FriendNickname"];
			        msg.FriendNickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["IsBlack"];
			        msg.IsBlack = (bool)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableFriendInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableFriendInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableFriendInfo msg = new TableFriendInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["FriendGuid"];
			        msg.FriendGuid = (long)val;
			        val = reader["FriendNickname"];
			        msg.FriendNickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["IsBlack"];
			        msg.IsBlack = (bool)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableFriendInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableFriendInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableFriendInfo msg = new TableFriendInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["FriendGuid"];
			        msg.FriendGuid = (long)val;
			        val = reader["FriendNickname"];
			        msg.FriendNickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["IsBlack"];
			        msg.IsBlack = (bool)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableFriendInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableFriendInfo), out _msg)){
				TableFriendInfo msg = _msg as TableFriendInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableFriendInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FriendGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FriendGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FriendNickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FriendNickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightingScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightingScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsBlack", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsBlack;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableGlobalParam(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableGlobalParam";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableGlobalParam msg = new TableGlobalParam();
			        val = reader["ParamType"];
			        msg.ParamType = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["ParamValue"];
			        msg.ParamValue = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableGlobalParam(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableGlobalParam";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_ParamType", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 32;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableGlobalParam msg = new TableGlobalParam();
			        val = reader["ParamType"];
			        msg.ParamType = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["ParamValue"];
			        msg.ParamValue = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableGlobalParam(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableGlobalParam(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableGlobalParam), out _msg)){
				TableGlobalParam msg = _msg as TableGlobalParam;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableGlobalParam";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ParamType", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ParamType;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ParamValue", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ParamValue;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableGowInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableGowInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableGowInfo msg = new TableGowInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["GowElo"];
			        msg.GowElo = (int)val;
			        val = reader["GowMatches"];
			        msg.GowMatches = (int)val;
			        val = reader["GowWinMatches"];
			        msg.GowWinMatches = (int)val;
			        val = reader["GowHistroyTimeList"];
			        msg.GowHistroyTimeList = (string)val;
			        val = reader["GowHistroyEloList"];
			        msg.GowHistroyEloList = (string)val;
			        val = reader["RankId"];
			        msg.RankId = (int)val;
			        val = reader["Point"];
			        msg.Point = (int)val;
			        val = reader["CriticalMatchCount"];
			        msg.CriticalMatchCount = (int)val;
			        val = reader["CriticalWinMatchCount"];
			        msg.CriticalWinMatchCount = (int)val;
			        val = reader["IsAcquirePrize"];
			        msg.IsAcquirePrize = (bool)val;
			        val = reader["LastTourneyDate"];
			        msg.LastTourneyDate = (string)val;
			        val = reader["LastResetGowPrizeTime"];
			        msg.LastResetGowPrizeTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableGowInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableGowInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableGowInfo msg = new TableGowInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["GowElo"];
			        msg.GowElo = (int)val;
			        val = reader["GowMatches"];
			        msg.GowMatches = (int)val;
			        val = reader["GowWinMatches"];
			        msg.GowWinMatches = (int)val;
			        val = reader["GowHistroyTimeList"];
			        msg.GowHistroyTimeList = (string)val;
			        val = reader["GowHistroyEloList"];
			        msg.GowHistroyEloList = (string)val;
			        val = reader["RankId"];
			        msg.RankId = (int)val;
			        val = reader["Point"];
			        msg.Point = (int)val;
			        val = reader["CriticalMatchCount"];
			        msg.CriticalMatchCount = (int)val;
			        val = reader["CriticalWinMatchCount"];
			        msg.CriticalWinMatchCount = (int)val;
			        val = reader["IsAcquirePrize"];
			        msg.IsAcquirePrize = (bool)val;
			        val = reader["LastTourneyDate"];
			        msg.LastTourneyDate = (string)val;
			        val = reader["LastResetGowPrizeTime"];
			        msg.LastResetGowPrizeTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableGowInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableGowInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableGowInfo msg = new TableGowInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["GowElo"];
			        msg.GowElo = (int)val;
			        val = reader["GowMatches"];
			        msg.GowMatches = (int)val;
			        val = reader["GowWinMatches"];
			        msg.GowWinMatches = (int)val;
			        val = reader["GowHistroyTimeList"];
			        msg.GowHistroyTimeList = (string)val;
			        val = reader["GowHistroyEloList"];
			        msg.GowHistroyEloList = (string)val;
			        val = reader["RankId"];
			        msg.RankId = (int)val;
			        val = reader["Point"];
			        msg.Point = (int)val;
			        val = reader["CriticalMatchCount"];
			        msg.CriticalMatchCount = (int)val;
			        val = reader["CriticalWinMatchCount"];
			        msg.CriticalWinMatchCount = (int)val;
			        val = reader["IsAcquirePrize"];
			        msg.IsAcquirePrize = (bool)val;
			        val = reader["LastTourneyDate"];
			        msg.LastTourneyDate = (string)val;
			        val = reader["LastResetGowPrizeTime"];
			        msg.LastResetGowPrizeTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableGowInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableGowInfo), out _msg)){
				TableGowInfo msg = _msg as TableGowInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableGowInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowElo", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowElo;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowMatches", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowMatches;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowWinMatches", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowWinMatches;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowHistroyTimeList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowHistroyTimeList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowHistroyEloList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowHistroyEloList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RankId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RankId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Point", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Point;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CriticalMatchCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CriticalMatchCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CriticalWinMatchCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CriticalWinMatchCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsAcquirePrize", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsAcquirePrize;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastTourneyDate", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastTourneyDate;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetGowPrizeTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetGowPrizeTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableGowStar(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableGowStar";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableGowStar msg = new TableGowStar();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["GowElo"];
			        msg.GowElo = (int)val;
			        val = reader["RankId"];
			        msg.RankId = (int)val;
			        val = reader["Point"];
			        msg.Point = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableGowStar(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableGowStar";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 16;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableGowStar msg = new TableGowStar();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["Rank"];
			        msg.Rank = (int)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightingScore"];
			        msg.FightingScore = (int)val;
			        val = reader["GowElo"];
			        msg.GowElo = (int)val;
			        val = reader["RankId"];
			        msg.RankId = (int)val;
			        val = reader["Point"];
			        msg.Point = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableGowStar(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableGowStar(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableGowStar), out _msg)){
				TableGowStar msg = _msg as TableGowStar;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableGowStar";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 16;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Rank", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Rank;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightingScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightingScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GowElo", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GowElo;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RankId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RankId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Point", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Point;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableGuid(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableGuid";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableGuid msg = new TableGuid();
			        val = reader["GuidType"];
			        msg.GuidType = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["GuidValue"];
			        msg.GuidValue = (ulong)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableGuid(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableGuid";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_GuidType", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableGuid msg = new TableGuid();
			        val = reader["GuidType"];
			        msg.GuidType = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["GuidValue"];
			        msg.GuidValue = (ulong)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableGuid(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableGuid(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableGuid), out _msg)){
				TableGuid msg = _msg as TableGuid;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableGuid";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GuidType", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GuidType;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GuidValue", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GuidValue;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableHomeNotice(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableHomeNotice";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableHomeNotice msg = new TableHomeNotice();
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["Content"];
			        msg.Content = (string)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableHomeNotice(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableHomeNotice";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (int)Convert.ChangeType(primaryKeys[0],typeof(int));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableHomeNotice msg = new TableHomeNotice();
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["Content"];
			        msg.Content = (string)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableHomeNotice(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableHomeNotice(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableHomeNotice), out _msg)){
				TableHomeNotice msg = _msg as TableHomeNotice;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableHomeNotice";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Content", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Content;
				    inputParam.Size = 2048;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CreateTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CreateTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableInviterInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableInviterInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableInviterInfo msg = new TableInviterInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["InviteCode"];
			        msg.InviteCode = (string)val;
			        val = reader["InviterGuid"];
			        msg.InviterGuid = (long)val;
			        val = reader["InviterLevel"];
			        msg.InviterLevel = (int)val;
			        val = reader["RewardedList"];
			        msg.RewardedList = (string)val;
			        val = reader["InviteeGuidList"];
			        msg.InviteeGuidList = (string)val;
			        val = reader["InviteeLevelList"];
			        msg.InviteeLevelList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableInviterInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableInviterInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableInviterInfo msg = new TableInviterInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["InviteCode"];
			        msg.InviteCode = (string)val;
			        val = reader["InviterGuid"];
			        msg.InviterGuid = (long)val;
			        val = reader["InviterLevel"];
			        msg.InviterLevel = (int)val;
			        val = reader["RewardedList"];
			        msg.RewardedList = (string)val;
			        val = reader["InviteeGuidList"];
			        msg.InviteeGuidList = (string)val;
			        val = reader["InviteeLevelList"];
			        msg.InviteeLevelList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableInviterInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableInviterInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableInviterInfo), out _msg)){
				TableInviterInfo msg = _msg as TableInviterInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableInviterInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_InviteCode", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.InviteCode;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_InviterGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.InviterGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_InviterLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.InviterLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RewardedList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RewardedList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_InviteeGuidList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.InviteeGuidList;
				    inputParam.Size = 1536;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_InviteeLevelList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.InviteeLevelList;
				    inputParam.Size = 512;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableItemInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableItemInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableItemInfo msg = new TableItemInfo();
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["ItemNum"];
			        msg.ItemNum = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableItemInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableItemInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_ItemGuid", MySqlDbType.UInt64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (ulong)Convert.ChangeType(primaryKeys[0],typeof(ulong));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableItemInfo msg = new TableItemInfo();
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (ulong)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["ItemNum"];
			        msg.ItemNum = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableItemInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableItemInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.UInt64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (ulong)Convert.ChangeType(foreignKeys[0],typeof(ulong));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableItemInfo msg = new TableItemInfo();
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["ItemNum"];
			        msg.ItemNum = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["EnhanceStarLevel"];
			        msg.EnhanceStarLevel = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableItemInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableItemInfo), out _msg)){
				TableItemInfo msg = _msg as TableItemInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableItemInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemGuid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemNum", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemNum;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Experience", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Experience;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AppendProperty", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AppendProperty;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EnhanceStarLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EnhanceStarLevel;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLegacyInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLegacyInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLegacyInfo msg = new TableLegacyInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["LegacyId"];
			        msg.LegacyId = (int)val;
			        val = reader["LegacyLevel"];
			        msg.LegacyLevel = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["IsUnlock"];
			        msg.IsUnlock = (bool)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLegacyInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLegacyInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLegacyInfo msg = new TableLegacyInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["LegacyId"];
			        msg.LegacyId = (int)val;
			        val = reader["LegacyLevel"];
			        msg.LegacyLevel = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["IsUnlock"];
			        msg.IsUnlock = (bool)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLegacyInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableLegacyInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLegacyInfo msg = new TableLegacyInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["LegacyId"];
			        msg.LegacyId = (int)val;
			        val = reader["LegacyLevel"];
			        msg.LegacyLevel = (int)val;
			        val = reader["AppendProperty"];
			        msg.AppendProperty = (int)val;
			        val = reader["IsUnlock"];
			        msg.IsUnlock = (bool)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableLegacyInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLegacyInfo), out _msg)){
				TableLegacyInfo msg = _msg as TableLegacyInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLegacyInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Position", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Position;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LegacyId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LegacyId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LegacyLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LegacyLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AppendProperty", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AppendProperty;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsUnlock", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsUnlock;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLevelInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLevelInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLevelInfo msg = new TableLevelInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["LevelId"];
			        msg.LevelId = (int)val;
			        val = reader["LevelRecord"];
			        msg.LevelRecord = (int)val;
			        val = reader["ResetEliteCount"];
			        msg.ResetEliteCount = (int)val;
			        val = reader["SceneDataBytes"];
			        msg.SceneDataBytes = (byte[])val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLevelInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLevelInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLevelInfo msg = new TableLevelInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["LevelId"];
			        msg.LevelId = (int)val;
			        val = reader["LevelRecord"];
			        msg.LevelRecord = (int)val;
			        val = reader["ResetEliteCount"];
			        msg.ResetEliteCount = (int)val;
			        val = reader["SceneDataBytes"];
			        msg.SceneDataBytes = (byte[])val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLevelInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableLevelInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLevelInfo msg = new TableLevelInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["LevelId"];
			        msg.LevelId = (int)val;
			        val = reader["LevelRecord"];
			        msg.LevelRecord = (int)val;
			        val = reader["ResetEliteCount"];
			        msg.ResetEliteCount = (int)val;
			        val = reader["SceneDataBytes"];
			        msg.SceneDataBytes = (byte[])val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableLevelInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLevelInfo), out _msg)){
				TableLevelInfo msg = _msg as TableLevelInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLevelInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LevelId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LevelId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LevelRecord", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LevelRecord;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ResetEliteCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ResetEliteCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SceneDataBytes", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SceneDataBytes;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLoginLotteryRecord(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLoginLotteryRecord";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLoginLotteryRecord msg = new TableLoginLotteryRecord();
			        val = reader["RecordId"];
			        msg.RecordId = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["RewardId"];
			        msg.RewardId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLoginLotteryRecord(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLoginLotteryRecord";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_RecordId", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLoginLotteryRecord msg = new TableLoginLotteryRecord();
			        val = reader["RecordId"];
			        msg.RecordId = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["RewardId"];
			        msg.RewardId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLoginLotteryRecord(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableLoginLotteryRecord(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLoginLotteryRecord), out _msg)){
				TableLoginLotteryRecord msg = _msg as TableLoginLotteryRecord;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLoginLotteryRecord";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RecordId", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RecordId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AccountId;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RewardId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RewardId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CreateTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CreateTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLootInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLootInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLootInfo msg = new TableLootInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["IsPool"];
			        msg.IsPool = (bool)val;
			        val = reader["IsVisible"];
			        msg.IsVisible = (bool)val;
			        val = reader["LootKey"];
			        msg.LootKey = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightScore"];
			        msg.FightScore = (int)val;
			        val = reader["LastBattleTime"];
			        msg.LastBattleTime = (string)val;
			        val = reader["DomainType"];
			        msg.DomainType = (int)val;
			        val = reader["BeginTime"];
			        msg.BeginTime = (string)val;
			        val = reader["IsOpen"];
			        msg.IsOpen = (bool)val;
			        val = reader["IsGetAward"];
			        msg.IsGetAward = (bool)val;
			        val = reader["LootAward"];
			        msg.LootAward = (float)val;
			        val = reader["TargetLootKey"];
			        msg.TargetLootKey = (long)val;
			        val = reader["FightOrderList"];
			        msg.FightOrderList = (string)val;
			        val = reader["LootOrderList"];
			        msg.LootOrderList = (string)val;
			        val = reader["LootIncome"];
			        msg.LootIncome = (int)val;
			        val = reader["LootLoss"];
			        msg.LootLoss = (int)val;
			        val = reader["LootType"];
			        msg.LootType = (int)val;
			        val = reader["LootBytes"];
			        msg.LootBytes = (byte[])val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLootInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLootInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLootInfo msg = new TableLootInfo();
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["IsPool"];
			        msg.IsPool = (bool)val;
			        val = reader["IsVisible"];
			        msg.IsVisible = (bool)val;
			        val = reader["LootKey"];
			        msg.LootKey = (long)val;
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["FightScore"];
			        msg.FightScore = (int)val;
			        val = reader["LastBattleTime"];
			        msg.LastBattleTime = (string)val;
			        val = reader["DomainType"];
			        msg.DomainType = (int)val;
			        val = reader["BeginTime"];
			        msg.BeginTime = (string)val;
			        val = reader["IsOpen"];
			        msg.IsOpen = (bool)val;
			        val = reader["IsGetAward"];
			        msg.IsGetAward = (bool)val;
			        val = reader["LootAward"];
			        msg.LootAward = (float)val;
			        val = reader["TargetLootKey"];
			        msg.TargetLootKey = (long)val;
			        val = reader["FightOrderList"];
			        msg.FightOrderList = (string)val;
			        val = reader["LootOrderList"];
			        msg.LootOrderList = (string)val;
			        val = reader["LootIncome"];
			        msg.LootIncome = (int)val;
			        val = reader["LootLoss"];
			        msg.LootLoss = (int)val;
			        val = reader["LootType"];
			        msg.LootType = (int)val;
			        val = reader["LootBytes"];
			        msg.LootBytes = (byte[])val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLootInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableLootInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLootInfo), out _msg)){
				TableLootInfo msg = _msg as TableLootInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLootInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsPool", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsPool;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsVisible", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsVisible;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootKey", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootKey;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastBattleTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastBattleTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DomainType", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DomainType;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BeginTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BeginTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsOpen", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsOpen;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsGetAward", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsGetAward;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootAward", MySqlDbType.Float);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootAward;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_TargetLootKey", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.TargetLootKey;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FightOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FightOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootIncome", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootIncome;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootLoss", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootLoss;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootType", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootType;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootBytes", MySqlDbType.Blob);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootBytes;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLootRecord(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLootRecord";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLootRecord msg = new TableLootRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["IsPool"];
			        msg.IsPool = (bool)val;
			        val = reader["IsLootSuccess"];
			        msg.IsLootSuccess = (bool)val;
			        val = reader["LootBeginTime"];
			        msg.LootBeginTime = (string)val;
			        val = reader["LootEndTime"];
			        msg.LootEndTime = (string)val;
			        val = reader["DomainType"];
			        msg.DomainType = (int)val;
			        val = reader["Booty"];
			        msg.Booty = (int)val;
			        val = reader["LGuid"];
			        msg.LGuid = (long)val;
			        val = reader["LHeroId"];
			        msg.LHeroId = (int)val;
			        val = reader["LLevel"];
			        msg.LLevel = (int)val;
			        val = reader["LFightScore"];
			        msg.LFightScore = (int)val;
			        val = reader["LNickname"];
			        msg.LNickname = (string)val;
			        val = reader["LUserDamage"];
			        msg.LUserDamage = (int)val;
			        val = reader["LDefenseOrderList"];
			        msg.LDefenseOrderList = (string)val;
			        val = reader["LLootOrderList"];
			        msg.LLootOrderList = (string)val;
			        val = reader["DGuid"];
			        msg.DGuid = (long)val;
			        val = reader["DHeroId"];
			        msg.DHeroId = (int)val;
			        val = reader["DLevel"];
			        msg.DLevel = (int)val;
			        val = reader["DFightScore"];
			        msg.DFightScore = (int)val;
			        val = reader["DNickname"];
			        msg.DNickname = (string)val;
			        val = reader["DUserDamage"];
			        msg.DUserDamage = (int)val;
			        val = reader["DDefenseOrderList"];
			        msg.DDefenseOrderList = (string)val;
			        val = reader["DLootOrderList"];
			        msg.DLootOrderList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLootRecord(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLootRecord";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLootRecord msg = new TableLootRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["IsPool"];
			        msg.IsPool = (bool)val;
			        val = reader["IsLootSuccess"];
			        msg.IsLootSuccess = (bool)val;
			        val = reader["LootBeginTime"];
			        msg.LootBeginTime = (string)val;
			        val = reader["LootEndTime"];
			        msg.LootEndTime = (string)val;
			        val = reader["DomainType"];
			        msg.DomainType = (int)val;
			        val = reader["Booty"];
			        msg.Booty = (int)val;
			        val = reader["LGuid"];
			        msg.LGuid = (long)val;
			        val = reader["LHeroId"];
			        msg.LHeroId = (int)val;
			        val = reader["LLevel"];
			        msg.LLevel = (int)val;
			        val = reader["LFightScore"];
			        msg.LFightScore = (int)val;
			        val = reader["LNickname"];
			        msg.LNickname = (string)val;
			        val = reader["LUserDamage"];
			        msg.LUserDamage = (int)val;
			        val = reader["LDefenseOrderList"];
			        msg.LDefenseOrderList = (string)val;
			        val = reader["LLootOrderList"];
			        msg.LLootOrderList = (string)val;
			        val = reader["DGuid"];
			        msg.DGuid = (long)val;
			        val = reader["DHeroId"];
			        msg.DHeroId = (int)val;
			        val = reader["DLevel"];
			        msg.DLevel = (int)val;
			        val = reader["DFightScore"];
			        msg.DFightScore = (int)val;
			        val = reader["DNickname"];
			        msg.DNickname = (string)val;
			        val = reader["DUserDamage"];
			        msg.DUserDamage = (int)val;
			        val = reader["DDefenseOrderList"];
			        msg.DDefenseOrderList = (string)val;
			        val = reader["DLootOrderList"];
			        msg.DLootOrderList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLootRecord(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableLootRecord";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLootRecord msg = new TableLootRecord();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["IsPool"];
			        msg.IsPool = (bool)val;
			        val = reader["IsLootSuccess"];
			        msg.IsLootSuccess = (bool)val;
			        val = reader["LootBeginTime"];
			        msg.LootBeginTime = (string)val;
			        val = reader["LootEndTime"];
			        msg.LootEndTime = (string)val;
			        val = reader["DomainType"];
			        msg.DomainType = (int)val;
			        val = reader["Booty"];
			        msg.Booty = (int)val;
			        val = reader["LGuid"];
			        msg.LGuid = (long)val;
			        val = reader["LHeroId"];
			        msg.LHeroId = (int)val;
			        val = reader["LLevel"];
			        msg.LLevel = (int)val;
			        val = reader["LFightScore"];
			        msg.LFightScore = (int)val;
			        val = reader["LNickname"];
			        msg.LNickname = (string)val;
			        val = reader["LUserDamage"];
			        msg.LUserDamage = (int)val;
			        val = reader["LDefenseOrderList"];
			        msg.LDefenseOrderList = (string)val;
			        val = reader["LLootOrderList"];
			        msg.LLootOrderList = (string)val;
			        val = reader["DGuid"];
			        msg.DGuid = (long)val;
			        val = reader["DHeroId"];
			        msg.DHeroId = (int)val;
			        val = reader["DLevel"];
			        msg.DLevel = (int)val;
			        val = reader["DFightScore"];
			        msg.DFightScore = (int)val;
			        val = reader["DNickname"];
			        msg.DNickname = (string)val;
			        val = reader["DUserDamage"];
			        msg.DUserDamage = (int)val;
			        val = reader["DDefenseOrderList"];
			        msg.DDefenseOrderList = (string)val;
			        val = reader["DLootOrderList"];
			        msg.DLootOrderList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableLootRecord(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLootRecord), out _msg)){
				TableLootRecord msg = _msg as TableLootRecord;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLootRecord";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsPool", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsPool;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsLootSuccess", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsLootSuccess;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootBeginTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootBeginTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LootEndTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LootEndTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DomainType", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DomainType;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Booty", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Booty;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LHeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LHeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LFightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LFightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LNickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LNickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LUserDamage", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LUserDamage;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LDefenseOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LDefenseOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LLootOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LLootOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DHeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DHeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DFightScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DFightScore;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DNickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DNickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DUserDamage", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DUserDamage;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DDefenseOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DDefenseOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DLootOrderList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DLootOrderList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableLotteryInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableLotteryInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLotteryInfo msg = new TableLotteryInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["LotteryId"];
			        msg.LotteryId = (int)val;
			        val = reader["IsFirstDraw"];
			        msg.IsFirstDraw = (bool)val;
			        val = reader["FreeCount"];
			        msg.FreeCount = (int)val;
			        val = reader["LastDrawTime"];
			        msg.LastDrawTime = (string)val;
			        val = reader["LastResetCountTime"];
			        msg.LastResetCountTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableLotteryInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableLotteryInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableLotteryInfo msg = new TableLotteryInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["LotteryId"];
			        msg.LotteryId = (int)val;
			        val = reader["IsFirstDraw"];
			        msg.IsFirstDraw = (bool)val;
			        val = reader["FreeCount"];
			        msg.FreeCount = (int)val;
			        val = reader["LastDrawTime"];
			        msg.LastDrawTime = (string)val;
			        val = reader["LastResetCountTime"];
			        msg.LastResetCountTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableLotteryInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableLotteryInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableLotteryInfo msg = new TableLotteryInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["LotteryId"];
			        msg.LotteryId = (int)val;
			        val = reader["IsFirstDraw"];
			        msg.IsFirstDraw = (bool)val;
			        val = reader["FreeCount"];
			        msg.FreeCount = (int)val;
			        val = reader["LastDrawTime"];
			        msg.LastDrawTime = (string)val;
			        val = reader["LastResetCountTime"];
			        msg.LastResetCountTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableLotteryInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableLotteryInfo), out _msg)){
				TableLotteryInfo msg = _msg as TableLotteryInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableLotteryInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LotteryId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LotteryId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsFirstDraw", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsFirstDraw;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FreeCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FreeCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastDrawTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastDrawTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableMailInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableMailInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMailInfo msg = new TableMailInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["ModuleTypeId"];
			        msg.ModuleTypeId = (int)val;
			        val = reader["Sender"];
			        msg.Sender = (string)val;
			        val = reader["Receiver"];
			        msg.Receiver = (long)val;
			        val = reader["SendDate"];
			        msg.SendDate = (string)val;
			        val = reader["ExpiryDate"];
			        msg.ExpiryDate = (string)val;
			        val = reader["Title"];
			        msg.Title = (string)val;
			        val = reader["Text"];
			        msg.Text = (string)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
			        val = reader["Stamina"];
			        msg.Stamina = (int)val;
			        val = reader["ItemIds"];
			        msg.ItemIds = (string)val;
			        val = reader["ItemNumbers"];
			        msg.ItemNumbers = (string)val;
			        val = reader["LevelDemand"];
			        msg.LevelDemand = (int)val;
			        val = reader["IsRead"];
			        msg.IsRead = (bool)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableMailInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableMailInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableMailInfo msg = new TableMailInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        val = reader["ModuleTypeId"];
			        msg.ModuleTypeId = (int)val;
			        val = reader["Sender"];
			        msg.Sender = (string)val;
			        val = reader["Receiver"];
			        msg.Receiver = (long)val;
			        val = reader["SendDate"];
			        msg.SendDate = (string)val;
			        val = reader["ExpiryDate"];
			        msg.ExpiryDate = (string)val;
			        val = reader["Title"];
			        msg.Title = (string)val;
			        val = reader["Text"];
			        msg.Text = (string)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
			        val = reader["Stamina"];
			        msg.Stamina = (int)val;
			        val = reader["ItemIds"];
			        msg.ItemIds = (string)val;
			        val = reader["ItemNumbers"];
			        msg.ItemNumbers = (string)val;
			        val = reader["LevelDemand"];
			        msg.LevelDemand = (int)val;
			        val = reader["IsRead"];
			        msg.IsRead = (bool)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableMailInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableMailInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableMailInfo), out _msg)){
				TableMailInfo msg = _msg as TableMailInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableMailInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ModuleTypeId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ModuleTypeId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Sender", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Sender;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Receiver", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Receiver;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SendDate", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SendDate;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExpiryDate", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExpiryDate;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Title", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Title;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Text", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Text;
				    inputParam.Size = 1024;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Money", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Money;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Gold", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Gold;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Stamina", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Stamina;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemIds", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemIds;
				    inputParam.Size = 128;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemNumbers", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemNumbers;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LevelDemand", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LevelDemand;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsRead", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsRead;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableMailStateInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableMailStateInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMailStateInfo msg = new TableMailStateInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MailGuid"];
			        msg.MailGuid = (long)val;
			        val = reader["IsRead"];
			        msg.IsRead = (bool)val;
			        val = reader["IsReceived"];
			        msg.IsReceived = (bool)val;
			        val = reader["ExpiryDate"];
			        msg.ExpiryDate = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableMailStateInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableMailStateInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableMailStateInfo msg = new TableMailStateInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["MailGuid"];
			        msg.MailGuid = (long)val;
			        val = reader["IsRead"];
			        msg.IsRead = (bool)val;
			        val = reader["IsReceived"];
			        msg.IsReceived = (bool)val;
			        val = reader["ExpiryDate"];
			        msg.ExpiryDate = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableMailStateInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableMailStateInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMailStateInfo msg = new TableMailStateInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MailGuid"];
			        msg.MailGuid = (long)val;
			        val = reader["IsRead"];
			        msg.IsRead = (bool)val;
			        val = reader["IsReceived"];
			        msg.IsReceived = (bool)val;
			        val = reader["ExpiryDate"];
			        msg.ExpiryDate = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableMailStateInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableMailStateInfo), out _msg)){
				TableMailStateInfo msg = _msg as TableMailStateInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableMailStateInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MailGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MailGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsRead", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsRead;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsReceived", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsReceived;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExpiryDate", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExpiryDate;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableMissionInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableMissionInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMissionInfo msg = new TableMissionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MissionId"];
			        msg.MissionId = (int)val;
			        val = reader["MissionValue"];
			        msg.MissionValue = (int)val;
			        val = reader["MissionState"];
			        msg.MissionState = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableMissionInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableMissionInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableMissionInfo msg = new TableMissionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["MissionId"];
			        msg.MissionId = (int)val;
			        val = reader["MissionValue"];
			        msg.MissionValue = (int)val;
			        val = reader["MissionState"];
			        msg.MissionState = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableMissionInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableMissionInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMissionInfo msg = new TableMissionInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MissionId"];
			        msg.MissionId = (int)val;
			        val = reader["MissionValue"];
			        msg.MissionValue = (int)val;
			        val = reader["MissionState"];
			        msg.MissionState = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableMissionInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableMissionInfo), out _msg)){
				TableMissionInfo msg = _msg as TableMissionInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableMissionInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MissionId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MissionId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MissionValue", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MissionValue;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MissionState", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MissionState;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableMpveAwardInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableMpveAwardInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMpveAwardInfo msg = new TableMpveAwardInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MpveSceneId"];
			        msg.MpveSceneId = (int)val;
			        val = reader["DareCount"];
			        msg.DareCount = (int)val;
			        val = reader["AwardCount"];
			        msg.AwardCount = (int)val;
			        val = reader["IsAwardedList"];
			        msg.IsAwardedList = (string)val;
			        val = reader["AwardIdList"];
			        msg.AwardIdList = (string)val;
			        val = reader["DifficultyList"];
			        msg.DifficultyList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableMpveAwardInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableMpveAwardInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableMpveAwardInfo msg = new TableMpveAwardInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["MpveSceneId"];
			        msg.MpveSceneId = (int)val;
			        val = reader["DareCount"];
			        msg.DareCount = (int)val;
			        val = reader["AwardCount"];
			        msg.AwardCount = (int)val;
			        val = reader["IsAwardedList"];
			        msg.IsAwardedList = (string)val;
			        val = reader["AwardIdList"];
			        msg.AwardIdList = (string)val;
			        val = reader["DifficultyList"];
			        msg.DifficultyList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableMpveAwardInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableMpveAwardInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableMpveAwardInfo msg = new TableMpveAwardInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["MpveSceneId"];
			        msg.MpveSceneId = (int)val;
			        val = reader["DareCount"];
			        msg.DareCount = (int)val;
			        val = reader["AwardCount"];
			        msg.AwardCount = (int)val;
			        val = reader["IsAwardedList"];
			        msg.IsAwardedList = (string)val;
			        val = reader["AwardIdList"];
			        msg.AwardIdList = (string)val;
			        val = reader["DifficultyList"];
			        msg.DifficultyList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableMpveAwardInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableMpveAwardInfo), out _msg)){
				TableMpveAwardInfo msg = _msg as TableMpveAwardInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableMpveAwardInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MpveSceneId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MpveSceneId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DareCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DareCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AwardCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AwardCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsAwardedList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsAwardedList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AwardIdList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AwardIdList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DifficultyList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DifficultyList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableNickname(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableNickname";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableNickname msg = new TableNickname();
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableNickname(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableNickname";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 32;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableNickname msg = new TableNickname();
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableNickname(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableNickname(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableNickname), out _msg)){
				TableNickname msg = _msg as TableNickname;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableNickname";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTablePartnerInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTablePartnerInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TablePartnerInfo msg = new TablePartnerInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["PartnerId"];
			        msg.PartnerId = (int)val;
			        val = reader["AdditionLevel"];
			        msg.AdditionLevel = (int)val;
			        val = reader["SkillLevel"];
			        msg.SkillLevel = (int)val;
			        val = reader["EquipList"];
			        msg.EquipList = (string)val;
			        val = reader["ActiveOrder"];
			        msg.ActiveOrder = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTablePartnerInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTablePartnerInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TablePartnerInfo msg = new TablePartnerInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["PartnerId"];
			        msg.PartnerId = (int)val;
			        val = reader["AdditionLevel"];
			        msg.AdditionLevel = (int)val;
			        val = reader["SkillLevel"];
			        msg.SkillLevel = (int)val;
			        val = reader["EquipList"];
			        msg.EquipList = (string)val;
			        val = reader["ActiveOrder"];
			        msg.ActiveOrder = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTablePartnerInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTablePartnerInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TablePartnerInfo msg = new TablePartnerInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["PartnerId"];
			        msg.PartnerId = (int)val;
			        val = reader["AdditionLevel"];
			        msg.AdditionLevel = (int)val;
			        val = reader["SkillLevel"];
			        msg.SkillLevel = (int)val;
			        val = reader["EquipList"];
			        msg.EquipList = (string)val;
			        val = reader["ActiveOrder"];
			        msg.ActiveOrder = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTablePartnerInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TablePartnerInfo), out _msg)){
				TablePartnerInfo msg = _msg as TablePartnerInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTablePartnerInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AdditionLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AdditionLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SkillLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SkillLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_EquipList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.EquipList;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActiveOrder", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActiveOrder;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTablePaymentInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTablePaymentInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TablePaymentInfo msg = new TablePaymentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["OrderId"];
			        msg.OrderId = (string)val;
			        val = reader["GoodsRegisterId"];
			        msg.GoodsRegisterId = (string)val;
			        val = reader["Diamond"];
			        msg.Diamond = (int)val;
			        val = reader["PaymentTime"];
			        msg.PaymentTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTablePaymentInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTablePaymentInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TablePaymentInfo msg = new TablePaymentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["OrderId"];
			        msg.OrderId = (string)val;
			        val = reader["GoodsRegisterId"];
			        msg.GoodsRegisterId = (string)val;
			        val = reader["Diamond"];
			        msg.Diamond = (int)val;
			        val = reader["PaymentTime"];
			        msg.PaymentTime = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTablePaymentInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTablePaymentInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TablePaymentInfo msg = new TablePaymentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["OrderId"];
			        msg.OrderId = (string)val;
			        val = reader["GoodsRegisterId"];
			        msg.GoodsRegisterId = (string)val;
			        val = reader["Diamond"];
			        msg.Diamond = (int)val;
			        val = reader["PaymentTime"];
			        msg.PaymentTime = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTablePaymentInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TablePaymentInfo), out _msg)){
				TablePaymentInfo msg = _msg as TablePaymentInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTablePaymentInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_OrderId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.OrderId;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GoodsRegisterId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GoodsRegisterId;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Diamond", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Diamond;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PaymentTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PaymentTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableSkillInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableSkillInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableSkillInfo msg = new TableSkillInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["SkillId"];
			        msg.SkillId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Preset"];
			        msg.Preset = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableSkillInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableSkillInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableSkillInfo msg = new TableSkillInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["SkillId"];
			        msg.SkillId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Preset"];
			        msg.Preset = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableSkillInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableSkillInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableSkillInfo msg = new TableSkillInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["SkillId"];
			        msg.SkillId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Preset"];
			        msg.Preset = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableSkillInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableSkillInfo), out _msg)){
				TableSkillInfo msg = _msg as TableSkillInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableSkillInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SkillId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SkillId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Preset", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Preset;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableTalentInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableTalentInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableTalentInfo msg = new TableTalentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableTalentInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableTalentInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableTalentInfo msg = new TableTalentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableTalentInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableTalentInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableTalentInfo msg = new TableTalentInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["ItemGuid"];
			        msg.ItemGuid = (long)val;
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["ItemId"];
			        msg.ItemId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Experience"];
			        msg.Experience = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableTalentInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableTalentInfo), out _msg)){
				TableTalentInfo msg = _msg as TableTalentInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableTalentInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Position", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Position;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ItemId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ItemId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Experience", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Experience;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableUndonePayment(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableUndonePayment";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUndonePayment msg = new TableUndonePayment();
			        val = reader["OrderId"];
			        msg.OrderId = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["GoodsRegisterId"];
			        msg.GoodsRegisterId = (string)val;
			        val = reader["GoodsNum"];
			        msg.GoodsNum = (int)val;
			        val = reader["GoodsPrice"];
			        msg.GoodsPrice = (float)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["ChannelId"];
			        msg.ChannelId = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableUndonePayment(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableUndonePayment";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_OrderId", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 48;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableUndonePayment msg = new TableUndonePayment();
			        val = reader["OrderId"];
			        msg.OrderId = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["GoodsRegisterId"];
			        msg.GoodsRegisterId = (string)val;
			        val = reader["GoodsNum"];
			        msg.GoodsNum = (int)val;
			        val = reader["GoodsPrice"];
			        msg.GoodsPrice = (float)val;
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        val = reader["ChannelId"];
			        msg.ChannelId = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableUndonePayment(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableUndonePayment(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableUndonePayment), out _msg)){
				TableUndonePayment msg = _msg as TableUndonePayment;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableUndonePayment";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_OrderId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.OrderId;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GoodsRegisterId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GoodsRegisterId;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GoodsNum", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GoodsNum;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GoodsPrice", MySqlDbType.Float);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GoodsPrice;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ChannelId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ChannelId;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableUserBattleInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableUserBattleInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUserBattleInfo msg = new TableUserBattleInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["SceneId"];
			        msg.SceneId = (int)val;
			        val = reader["StartTime"];
			        msg.StartTime = (long)val;
			        val = reader["SumGold"];
			        msg.SumGold = (int)val;
			        val = reader["Exp"];
			        msg.Exp = (int)val;
			        val = reader["RewardItemId"];
			        msg.RewardItemId = (int)val;
			        val = reader["RewardItemCount"];
			        msg.RewardItemCount = (int)val;
			        val = reader["DeadCount"];
			        msg.DeadCount = (int)val;
			        val = reader["ReliveCount"];
			        msg.ReliveCount = (int)val;
			        val = reader["IsClearing"];
			        msg.IsClearing = (bool)val;
			        val = reader["MatchKey"];
			        msg.MatchKey = (int)val;
			        val = reader["PartnerFinishedCount"];
			        msg.PartnerFinishedCount = (int)val;
			        val = reader["PartnerRemainCount"];
			        msg.PartnerRemainCount = (int)val;
			        val = reader["PartnerBuyCount"];
			        msg.PartnerBuyCount = (int)val;
			        val = reader["PartnerList"];
			        msg.PartnerList = (string)val;
			        val = reader["PartnerSelectIndex"];
			        msg.PartnerSelectIndex = (int)val;
			        val = reader["PartnerLastResetTime"];
			        msg.PartnerLastResetTime = (string)val;
			        val = reader["DungeonQueryCount"];
			        msg.DungeonQueryCount = (int)val;
			        val = reader["DungeonLeftFightCount"];
			        msg.DungeonLeftFightCount = (int)val;
			        val = reader["DungeonBuyFightCount"];
			        msg.DungeonBuyFightCount = (int)val;
			        val = reader["DungeonMatchTargetList"];
			        msg.DungeonMatchTargetList = (string)val;
			        val = reader["DungeonMatchDropList"];
			        msg.DungeonMatchDropList = (string)val;
			        val = reader["DungeonLastResetTime"];
			        msg.DungeonLastResetTime = (string)val;
			        val = reader["SecretCurrentFight"];
			        msg.SecretCurrentFight = (int)val;
			        val = reader["SecretHpRateList"];
			        msg.SecretHpRateList = (string)val;
			        val = reader["SecretMpRateList"];
			        msg.SecretMpRateList = (string)val;
			        val = reader["SecretSegmentList"];
			        msg.SecretSegmentList = (string)val;
			        val = reader["SecretFightCountList"];
			        msg.SecretFightCountList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableUserBattleInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableUserBattleInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableUserBattleInfo msg = new TableUserBattleInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["SceneId"];
			        msg.SceneId = (int)val;
			        val = reader["StartTime"];
			        msg.StartTime = (long)val;
			        val = reader["SumGold"];
			        msg.SumGold = (int)val;
			        val = reader["Exp"];
			        msg.Exp = (int)val;
			        val = reader["RewardItemId"];
			        msg.RewardItemId = (int)val;
			        val = reader["RewardItemCount"];
			        msg.RewardItemCount = (int)val;
			        val = reader["DeadCount"];
			        msg.DeadCount = (int)val;
			        val = reader["ReliveCount"];
			        msg.ReliveCount = (int)val;
			        val = reader["IsClearing"];
			        msg.IsClearing = (bool)val;
			        val = reader["MatchKey"];
			        msg.MatchKey = (int)val;
			        val = reader["PartnerFinishedCount"];
			        msg.PartnerFinishedCount = (int)val;
			        val = reader["PartnerRemainCount"];
			        msg.PartnerRemainCount = (int)val;
			        val = reader["PartnerBuyCount"];
			        msg.PartnerBuyCount = (int)val;
			        val = reader["PartnerList"];
			        msg.PartnerList = (string)val;
			        val = reader["PartnerSelectIndex"];
			        msg.PartnerSelectIndex = (int)val;
			        val = reader["PartnerLastResetTime"];
			        msg.PartnerLastResetTime = (string)val;
			        val = reader["DungeonQueryCount"];
			        msg.DungeonQueryCount = (int)val;
			        val = reader["DungeonLeftFightCount"];
			        msg.DungeonLeftFightCount = (int)val;
			        val = reader["DungeonBuyFightCount"];
			        msg.DungeonBuyFightCount = (int)val;
			        val = reader["DungeonMatchTargetList"];
			        msg.DungeonMatchTargetList = (string)val;
			        val = reader["DungeonMatchDropList"];
			        msg.DungeonMatchDropList = (string)val;
			        val = reader["DungeonLastResetTime"];
			        msg.DungeonLastResetTime = (string)val;
			        val = reader["SecretCurrentFight"];
			        msg.SecretCurrentFight = (int)val;
			        val = reader["SecretHpRateList"];
			        msg.SecretHpRateList = (string)val;
			        val = reader["SecretMpRateList"];
			        msg.SecretMpRateList = (string)val;
			        val = reader["SecretSegmentList"];
			        msg.SecretSegmentList = (string)val;
			        val = reader["SecretFightCountList"];
			        msg.SecretFightCountList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableUserBattleInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableUserBattleInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableUserBattleInfo), out _msg)){
				TableUserBattleInfo msg = _msg as TableUserBattleInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableUserBattleInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SceneId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SceneId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_StartTime", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.StartTime;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SumGold", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SumGold;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Exp", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Exp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RewardItemId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RewardItemId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RewardItemCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RewardItemCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DeadCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DeadCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ReliveCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ReliveCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsClearing", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsClearing;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MatchKey", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MatchKey;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerFinishedCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerFinishedCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerRemainCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerRemainCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerBuyCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerBuyCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerList;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerSelectIndex", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerSelectIndex;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PartnerLastResetTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PartnerLastResetTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonQueryCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonQueryCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonLeftFightCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonLeftFightCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonBuyFightCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonBuyFightCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonMatchTargetList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonMatchTargetList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonMatchDropList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonMatchDropList;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DungeonLastResetTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DungeonLastResetTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SecretCurrentFight", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SecretCurrentFight;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SecretHpRateList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SecretHpRateList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SecretMpRateList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SecretMpRateList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SecretSegmentList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SecretSegmentList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SecretFightCountList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SecretFightCountList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableUserGeneralInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableUserGeneralInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUserGeneralInfo msg = new TableUserGeneralInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["GuideFlag"];
			        msg.GuideFlag = (long)val;
			        val = reader["NewbieFlag"];
			        msg.NewbieFlag = (string)val;
			        val = reader["NewbieActionFlag"];
			        msg.NewbieActionFlag = (string)val;
			        val = reader["BuyMoneyCount"];
			        msg.BuyMoneyCount = (int)val;
			        val = reader["LastBuyMoneyTimestamp"];
			        msg.LastBuyMoneyTimestamp = (double)val;
			        val = reader["LastResetMidasTouchTime"];
			        msg.LastResetMidasTouchTime = (string)val;
			        val = reader["SellIncome"];
			        msg.SellIncome = (int)val;
			        val = reader["LastSellTimestamp"];
			        msg.LastSellTimestamp = (double)val;
			        val = reader["LastResetSellTime"];
			        msg.LastResetSellTime = (string)val;
			        val = reader["ExchangeGoodList"];
			        msg.ExchangeGoodList = (string)val;
			        val = reader["ExchangeGoodNumber"];
			        msg.ExchangeGoodNumber = (string)val;
			        val = reader["ExchangeGoodRefreshCount"];
			        msg.ExchangeGoodRefreshCount = (string)val;
			        val = reader["LastResetExchangeGoodTime"];
			        msg.LastResetExchangeGoodTime = (string)val;
			        val = reader["LastResetDaySignCountTime"];
			        msg.LastResetDaySignCountTime = (string)val;
			        val = reader["MonthSignCount"];
			        msg.MonthSignCount = (int)val;
			        val = reader["LastResetMonthSignCountTime"];
			        msg.LastResetMonthSignCountTime = (string)val;
			        val = reader["MonthCardExpireTime"];
			        msg.MonthCardExpireTime = (string)val;
			        val = reader["IsWeeklyLoginRewarded"];
			        msg.IsWeeklyLoginRewarded = (bool)val;
			        val = reader["WeeklyLoginRewardList"];
			        msg.WeeklyLoginRewardList = (string)val;
			        val = reader["LastResetWeeklyLoginRewardTime"];
			        msg.LastResetWeeklyLoginRewardTime = (string)val;
			        val = reader["DailyOnlineDuration"];
			        msg.DailyOnlineDuration = (int)val;
			        val = reader["DailyOnlineRewardList"];
			        msg.DailyOnlineRewardList = (string)val;
			        val = reader["LastResetDailyOnlineTime"];
			        msg.LastResetDailyOnlineTime = (string)val;
			        val = reader["IsFirstPaymentRewarded"];
			        msg.IsFirstPaymentRewarded = (bool)val;
			        val = reader["VipRewaredList"];
			        msg.VipRewaredList = (string)val;
			        val = reader["MorrowRewardId"];
			        msg.MorrowRewardId = (int)val;
			        val = reader["MorrowActiveTime"];
			        msg.MorrowActiveTime = (string)val;
			        val = reader["IsMorrowActive"];
			        msg.IsMorrowActive = (bool)val;
			        val = reader["LevelupCostTime"];
			        msg.LevelupCostTime = (double)val;
			        val = reader["IsChatForbidden"];
			        msg.IsChatForbidden = (bool)val;
			        val = reader["GrowthFund"];
			        msg.GrowthFund = (int)val;
			        val = reader["ChapterIdList"];
			        msg.ChapterIdList = (string)val;
			        val = reader["ChapterAwardList"];
			        msg.ChapterAwardList = (string)val;
			        val = reader["LastResetConsumeCountTime"];
			        msg.LastResetConsumeCountTime = (string)val;
			        val = reader["DayRestSignCount"];
			        msg.DayRestSignCount = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableUserGeneralInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableUserGeneralInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableUserGeneralInfo msg = new TableUserGeneralInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["GuideFlag"];
			        msg.GuideFlag = (long)val;
			        val = reader["NewbieFlag"];
			        msg.NewbieFlag = (string)val;
			        val = reader["NewbieActionFlag"];
			        msg.NewbieActionFlag = (string)val;
			        val = reader["BuyMoneyCount"];
			        msg.BuyMoneyCount = (int)val;
			        val = reader["LastBuyMoneyTimestamp"];
			        msg.LastBuyMoneyTimestamp = (double)val;
			        val = reader["LastResetMidasTouchTime"];
			        msg.LastResetMidasTouchTime = (string)val;
			        val = reader["SellIncome"];
			        msg.SellIncome = (int)val;
			        val = reader["LastSellTimestamp"];
			        msg.LastSellTimestamp = (double)val;
			        val = reader["LastResetSellTime"];
			        msg.LastResetSellTime = (string)val;
			        val = reader["ExchangeGoodList"];
			        msg.ExchangeGoodList = (string)val;
			        val = reader["ExchangeGoodNumber"];
			        msg.ExchangeGoodNumber = (string)val;
			        val = reader["ExchangeGoodRefreshCount"];
			        msg.ExchangeGoodRefreshCount = (string)val;
			        val = reader["LastResetExchangeGoodTime"];
			        msg.LastResetExchangeGoodTime = (string)val;
			        val = reader["LastResetDaySignCountTime"];
			        msg.LastResetDaySignCountTime = (string)val;
			        val = reader["MonthSignCount"];
			        msg.MonthSignCount = (int)val;
			        val = reader["LastResetMonthSignCountTime"];
			        msg.LastResetMonthSignCountTime = (string)val;
			        val = reader["MonthCardExpireTime"];
			        msg.MonthCardExpireTime = (string)val;
			        val = reader["IsWeeklyLoginRewarded"];
			        msg.IsWeeklyLoginRewarded = (bool)val;
			        val = reader["WeeklyLoginRewardList"];
			        msg.WeeklyLoginRewardList = (string)val;
			        val = reader["LastResetWeeklyLoginRewardTime"];
			        msg.LastResetWeeklyLoginRewardTime = (string)val;
			        val = reader["DailyOnlineDuration"];
			        msg.DailyOnlineDuration = (int)val;
			        val = reader["DailyOnlineRewardList"];
			        msg.DailyOnlineRewardList = (string)val;
			        val = reader["LastResetDailyOnlineTime"];
			        msg.LastResetDailyOnlineTime = (string)val;
			        val = reader["IsFirstPaymentRewarded"];
			        msg.IsFirstPaymentRewarded = (bool)val;
			        val = reader["VipRewaredList"];
			        msg.VipRewaredList = (string)val;
			        val = reader["MorrowRewardId"];
			        msg.MorrowRewardId = (int)val;
			        val = reader["MorrowActiveTime"];
			        msg.MorrowActiveTime = (string)val;
			        val = reader["IsMorrowActive"];
			        msg.IsMorrowActive = (bool)val;
			        val = reader["LevelupCostTime"];
			        msg.LevelupCostTime = (double)val;
			        val = reader["IsChatForbidden"];
			        msg.IsChatForbidden = (bool)val;
			        val = reader["GrowthFund"];
			        msg.GrowthFund = (int)val;
			        val = reader["ChapterIdList"];
			        msg.ChapterIdList = (string)val;
			        val = reader["ChapterAwardList"];
			        msg.ChapterAwardList = (string)val;
			        val = reader["LastResetConsumeCountTime"];
			        msg.LastResetConsumeCountTime = (string)val;
			        val = reader["DayRestSignCount"];
			        msg.DayRestSignCount = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableUserGeneralInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableUserGeneralInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableUserGeneralInfo), out _msg)){
				TableUserGeneralInfo msg = _msg as TableUserGeneralInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableUserGeneralInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GuideFlag", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GuideFlag;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_NewbieFlag", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.NewbieFlag;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_NewbieActionFlag", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.NewbieActionFlag;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BuyMoneyCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BuyMoneyCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastBuyMoneyTimestamp", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastBuyMoneyTimestamp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetMidasTouchTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetMidasTouchTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SellIncome", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SellIncome;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastSellTimestamp", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastSellTimestamp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetSellTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetSellTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExchangeGoodList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExchangeGoodList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExchangeGoodNumber", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExchangeGoodNumber;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExchangeGoodRefreshCount", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExchangeGoodRefreshCount;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetExchangeGoodTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetExchangeGoodTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetDaySignCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetDaySignCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MonthSignCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MonthSignCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetMonthSignCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetMonthSignCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MonthCardExpireTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MonthCardExpireTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsWeeklyLoginRewarded", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsWeeklyLoginRewarded;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_WeeklyLoginRewardList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.WeeklyLoginRewardList;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetWeeklyLoginRewardTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetWeeklyLoginRewardTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DailyOnlineDuration", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DailyOnlineDuration;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DailyOnlineRewardList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DailyOnlineRewardList;
				    inputParam.Size = 48;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetDailyOnlineTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetDailyOnlineTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsFirstPaymentRewarded", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsFirstPaymentRewarded;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_VipRewaredList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.VipRewaredList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MorrowRewardId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MorrowRewardId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MorrowActiveTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MorrowActiveTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsMorrowActive", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsMorrowActive;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LevelupCostTime", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LevelupCostTime;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsChatForbidden", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsChatForbidden;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_GrowthFund", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.GrowthFund;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ChapterIdList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ChapterIdList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ChapterAwardList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ChapterAwardList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetConsumeCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetConsumeCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DayRestSignCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DayRestSignCount;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableUserInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableUserInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUserInfo msg = new TableUserInfo();
			        val = reader["Guid"];
			        msg.Guid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["CitySceneId"];
			        msg.CitySceneId = (int)val;
			        val = reader["Vip"];
			        msg.Vip = (int)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["AchievedScore"];
			        msg.AchievedScore = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableUserInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableUserInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.UInt64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (ulong)Convert.ChangeType(primaryKeys[0],typeof(ulong));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableUserInfo msg = new TableUserInfo();
			        val = reader["Guid"];
			        msg.Guid = (ulong)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["CitySceneId"];
			        msg.CitySceneId = (int)val;
			        val = reader["Vip"];
			        msg.Vip = (int)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["AchievedScore"];
			        msg.AchievedScore = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableUserInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableUserInfo";
			    if(foreignKeys.Count != 2)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (int)Convert.ChangeType(foreignKeys[0],typeof(int));
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = foreignKeys[1];
			    inputParam.Size = 64;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUserInfo msg = new TableUserInfo();
			        val = reader["Guid"];
			        msg.Guid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["LogicServerId"];
			        msg.LogicServerId = (int)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["CitySceneId"];
			        msg.CitySceneId = (int)val;
			        val = reader["Vip"];
			        msg.Vip = (int)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["AchievedScore"];
			        msg.AchievedScore = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableUserInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableUserInfo), out _msg)){
				TableUserInfo msg = _msg as TableUserInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableUserInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LogicServerId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LogicServerId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AccountId;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Nickname", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Nickname;
				    inputParam.Size = 32;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CreateTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CreateTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Money", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Money;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Gold", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Gold;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExpPoints", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExpPoints;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CitySceneId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CitySceneId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Vip", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Vip;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastLogoutTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastLogoutTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_AchievedScore", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AchievedScore;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableUserSpecialInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableUserSpecialInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableUserSpecialInfo msg = new TableUserSpecialInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["Vigor"];
			        msg.Vigor = (int)val;
			        val = reader["LastAddVigorTimestamp"];
			        msg.LastAddVigorTimestamp = (double)val;
			        val = reader["Stamina"];
			        msg.Stamina = (int)val;
			        val = reader["BuyStaminaCount"];
			        msg.BuyStaminaCount = (int)val;
			        val = reader["LastAddStaminaTimestamp"];
			        msg.LastAddStaminaTimestamp = (double)val;
			        val = reader["UsedStamina"];
			        msg.UsedStamina = (int)val;
			        val = reader["LastResetStaminaTime"];
			        msg.LastResetStaminaTime = (string)val;
			        val = reader["CompleteSceneList"];
			        msg.CompleteSceneList = (string)val;
			        val = reader["CompleteSceneNumber"];
			        msg.CompleteSceneNumber = (string)val;
			        val = reader["LastResetSceneCountTime"];
			        msg.LastResetSceneCountTime = (string)val;
			        val = reader["LastResetDailyMissionTime"];
			        msg.LastResetDailyMissionTime = (string)val;
			        val = reader["ActiveFashionId"];
			        msg.ActiveFashionId = (int)val;
			        val = reader["IsFashionShow"];
			        msg.IsFashionShow = (bool)val;
			        val = reader["ActiveWingId"];
			        msg.ActiveWingId = (int)val;
			        val = reader["IsWingShow"];
			        msg.IsWingShow = (bool)val;
			        val = reader["ActiveWeaponId"];
			        msg.ActiveWeaponId = (int)val;
			        val = reader["IsWeaponShow"];
			        msg.IsWeaponShow = (bool)val;
			        val = reader["Vitality"];
			        msg.Vitality = (int)val;
			        val = reader["LastAddVitalityTimestamp"];
			        msg.LastAddVitalityTimestamp = (double)val;
			        val = reader["LastResetExpeditionTime"];
			        msg.LastResetExpeditionTime = (string)val;
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        val = reader["LastQuitCorpsTime"];
			        msg.LastQuitCorpsTime = (string)val;
			        val = reader["IsAcquireCorpsSignInPrize"];
			        msg.IsAcquireCorpsSignInPrize = (bool)val;
			        val = reader["LastResetCorpsSignInPrizeTime"];
			        msg.LastResetCorpsSignInPrizeTime = (string)val;
			        val = reader["CorpsChapterIdList"];
			        msg.CorpsChapterIdList = (string)val;
			        val = reader["CorpsChapterDareList"];
			        msg.CorpsChapterDareList = (string)val;
			        val = reader["LastResetSecretAreaTime"];
			        msg.LastResetSecretAreaTime = (string)val;
			        val = reader["RecentLoginState"];
			        msg.RecentLoginState = (long)val;
			        val = reader["SumLoginDayCount"];
			        msg.SumLoginDayCount = (int)val;
			        val = reader["UsedLoginLotteryDrawCount"];
			        msg.UsedLoginLotteryDrawCount = (int)val;
			        val = reader["LastSaveRecentLoginTime"];
			        msg.LastSaveRecentLoginTime = (string)val;
			        val = reader["DiamondBoxList"];
			        msg.DiamondBoxList = (string)val;
			        val = reader["LastResetMpveAwardTime"];
			        msg.LastResetMpveAwardTime = (string)val;
			        val = reader["FinishedActivityList"];
			        msg.FinishedActivityList = (string)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableUserSpecialInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableUserSpecialInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(primaryKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableUserSpecialInfo msg = new TableUserSpecialInfo();
			        val = reader["Guid"];
			        msg.Guid = (long)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["Vigor"];
			        msg.Vigor = (int)val;
			        val = reader["LastAddVigorTimestamp"];
			        msg.LastAddVigorTimestamp = (double)val;
			        val = reader["Stamina"];
			        msg.Stamina = (int)val;
			        val = reader["BuyStaminaCount"];
			        msg.BuyStaminaCount = (int)val;
			        val = reader["LastAddStaminaTimestamp"];
			        msg.LastAddStaminaTimestamp = (double)val;
			        val = reader["UsedStamina"];
			        msg.UsedStamina = (int)val;
			        val = reader["LastResetStaminaTime"];
			        msg.LastResetStaminaTime = (string)val;
			        val = reader["CompleteSceneList"];
			        msg.CompleteSceneList = (string)val;
			        val = reader["CompleteSceneNumber"];
			        msg.CompleteSceneNumber = (string)val;
			        val = reader["LastResetSceneCountTime"];
			        msg.LastResetSceneCountTime = (string)val;
			        val = reader["LastResetDailyMissionTime"];
			        msg.LastResetDailyMissionTime = (string)val;
			        val = reader["ActiveFashionId"];
			        msg.ActiveFashionId = (int)val;
			        val = reader["IsFashionShow"];
			        msg.IsFashionShow = (bool)val;
			        val = reader["ActiveWingId"];
			        msg.ActiveWingId = (int)val;
			        val = reader["IsWingShow"];
			        msg.IsWingShow = (bool)val;
			        val = reader["ActiveWeaponId"];
			        msg.ActiveWeaponId = (int)val;
			        val = reader["IsWeaponShow"];
			        msg.IsWeaponShow = (bool)val;
			        val = reader["Vitality"];
			        msg.Vitality = (int)val;
			        val = reader["LastAddVitalityTimestamp"];
			        msg.LastAddVitalityTimestamp = (double)val;
			        val = reader["LastResetExpeditionTime"];
			        msg.LastResetExpeditionTime = (string)val;
			        val = reader["CorpsGuid"];
			        msg.CorpsGuid = (long)val;
			        val = reader["LastQuitCorpsTime"];
			        msg.LastQuitCorpsTime = (string)val;
			        val = reader["IsAcquireCorpsSignInPrize"];
			        msg.IsAcquireCorpsSignInPrize = (bool)val;
			        val = reader["LastResetCorpsSignInPrizeTime"];
			        msg.LastResetCorpsSignInPrizeTime = (string)val;
			        val = reader["CorpsChapterIdList"];
			        msg.CorpsChapterIdList = (string)val;
			        val = reader["CorpsChapterDareList"];
			        msg.CorpsChapterDareList = (string)val;
			        val = reader["LastResetSecretAreaTime"];
			        msg.LastResetSecretAreaTime = (string)val;
			        val = reader["RecentLoginState"];
			        msg.RecentLoginState = (long)val;
			        val = reader["SumLoginDayCount"];
			        msg.SumLoginDayCount = (int)val;
			        val = reader["UsedLoginLotteryDrawCount"];
			        msg.UsedLoginLotteryDrawCount = (int)val;
			        val = reader["LastSaveRecentLoginTime"];
			        msg.LastSaveRecentLoginTime = (string)val;
			        val = reader["DiamondBoxList"];
			        msg.DiamondBoxList = (string)val;
			        val = reader["LastResetMpveAwardTime"];
			        msg.LastResetMpveAwardTime = (string)val;
			        val = reader["FinishedActivityList"];
			        msg.FinishedActivityList = (string)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableUserSpecialInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableUserSpecialInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableUserSpecialInfo), out _msg)){
				TableUserSpecialInfo msg = _msg as TableUserSpecialInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableUserSpecialInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Vigor", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Vigor;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastAddVigorTimestamp", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastAddVigorTimestamp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Stamina", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Stamina;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_BuyStaminaCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.BuyStaminaCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastAddStaminaTimestamp", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastAddStaminaTimestamp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UsedStamina", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UsedStamina;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetStaminaTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetStaminaTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CompleteSceneList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CompleteSceneList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CompleteSceneNumber", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CompleteSceneNumber;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetSceneCountTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetSceneCountTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetDailyMissionTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetDailyMissionTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActiveFashionId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActiveFashionId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsFashionShow", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsFashionShow;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActiveWingId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActiveWingId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsWingShow", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsWingShow;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ActiveWeaponId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ActiveWeaponId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsWeaponShow", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsWeaponShow;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Vitality", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Vitality;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastAddVitalityTimestamp", MySqlDbType.Double);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastAddVitalityTimestamp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetExpeditionTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetExpeditionTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastQuitCorpsTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastQuitCorpsTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsAcquireCorpsSignInPrize", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsAcquireCorpsSignInPrize;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetCorpsSignInPrizeTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetCorpsSignInPrizeTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsChapterIdList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsChapterIdList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_CorpsChapterDareList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.CorpsChapterDareList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetSecretAreaTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetSecretAreaTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_RecentLoginState", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.RecentLoginState;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SumLoginDayCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SumLoginDayCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UsedLoginLotteryDrawCount", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UsedLoginLotteryDrawCount;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastSaveRecentLoginTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastSaveRecentLoginTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DiamondBoxList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.DiamondBoxList;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_LastResetMpveAwardTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastResetMpveAwardTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FinishedActivityList", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FinishedActivityList;
				    inputParam.Size = 255;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static List<GeneralRecordData> LoadAllTableXSoulInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableXSoulInfo";
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Start", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = start;
			    cmd.Parameters.Add(inputParam);
			    inputParam = new MySqlParameter("@_Count", MySqlDbType.Int32);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = count;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableXSoulInfo msg = new TableXSoulInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["XSoulType"];
			        msg.XSoulType = (int)val;
			        val = reader["XSoulId"];
			        msg.XSoulId = (int)val;
			        val = reader["XSoulLevel"];
			        msg.XSoulLevel = (int)val;
			        val = reader["XSoulExp"];
			        msg.XSoulExp = (int)val;
			        val = reader["XSoulModelLevel"];
			        msg.XSoulModelLevel = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static GeneralRecordData LoadSingleTableXSoulInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableXSoulInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 24;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableXSoulInfo msg = new TableXSoulInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["XSoulType"];
			        msg.XSoulType = (int)val;
			        val = reader["XSoulId"];
			        msg.XSoulId = (int)val;
			        val = reader["XSoulLevel"];
			        msg.XSoulLevel = (int)val;
			        val = reader["XSoulExp"];
			        msg.XSoulExp = (int)val;
			        val = reader["XSoulModelLevel"];
			        msg.XSoulModelLevel = (int)val;
			        ret.DataVersion = (int)reader["DataVersion"];
			        ret.Data = DbDataSerializer.Encode(msg);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static List<GeneralRecordData> LoadMultiTableXSoulInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableXSoulInfo";
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (long)Convert.ChangeType(foreignKeys[0],typeof(long));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      while (reader.Read()) {
			        GeneralRecordData record = new GeneralRecordData();
			        object val;
			        TableXSoulInfo msg = new TableXSoulInfo();
			        val = reader["Guid"];
			        msg.Guid = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (long)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Position"];
			        msg.Position = (int)val;
			        val = reader["XSoulType"];
			        msg.XSoulType = (int)val;
			        val = reader["XSoulId"];
			        msg.XSoulId = (int)val;
			        val = reader["XSoulLevel"];
			        msg.XSoulLevel = (int)val;
			        val = reader["XSoulExp"];
			        msg.XSoulExp = (int)val;
			        val = reader["XSoulModelLevel"];
			        msg.XSoulModelLevel = (int)val;
			        record.DataVersion = (int)reader["DataVersion"];
			        record.Data = DbDataSerializer.Encode(msg);
			        ret.Add(record);
			      }
			    }
			  }
			} catch (Exception ex) {
			  DBConn.Close();
			  throw ex;
			}
			return ret;
		}


		private static void SaveTableXSoulInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableXSoulInfo), out _msg)){
				TableXSoulInfo msg = _msg as TableXSoulInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableXSoulInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Guid", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Guid;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Position", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Position;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_XSoulType", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.XSoulType;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_XSoulId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.XSoulId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_XSoulLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.XSoulLevel;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_XSoulExp", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.XSoulExp;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_XSoulModelLevel", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.XSoulModelLevel;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


	}
}
