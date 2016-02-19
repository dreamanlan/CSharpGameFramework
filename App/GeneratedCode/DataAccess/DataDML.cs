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
				case (int)DataEnum.TableFriendInfo:
					SaveTableFriendInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGlobalParam:
					SaveTableGlobalParam(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableGuid:
					SaveTableGuid(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableItemInfo:
					SaveTableItemInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMailInfo:
					SaveTableMailInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMailStateInfo:
					SaveTableMailStateInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableMemberInfo:
					SaveTableMemberInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableNicknameInfo:
					SaveTableNicknameInfo(isValid, dataVersion, data);
					break;
				case (int)DataEnum.TableUserInfo:
					SaveTableUserInfo(isValid, dataVersion, data);
					break;
			}
		}
		internal static int BatchSave(int msgId, List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			int count = 0;
			switch(msgId){
				case (int)DataEnum.TableAccount:
					count = BatchSaveTableAccount(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableActivationCode:
					count = BatchSaveTableActivationCode(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableFriendInfo:
					count = BatchSaveTableFriendInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableGlobalParam:
					count = BatchSaveTableGlobalParam(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableGuid:
					count = BatchSaveTableGuid(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableItemInfo:
					count = BatchSaveTableItemInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableMailInfo:
					count = BatchSaveTableMailInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableMailStateInfo:
					count = BatchSaveTableMailStateInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableMemberInfo:
					count = BatchSaveTableMemberInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableNicknameInfo:
					count = BatchSaveTableNicknameInfo(validList, dataList, dataVersion);
					break;
				case (int)DataEnum.TableUserInfo:
					count = BatchSaveTableUserInfo(validList, dataList, dataVersion);
					break;
			}
			return count;
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
				case (int)DataEnum.TableFriendInfo:
					return LoadAllTableFriendInfo(start, count);
				case (int)DataEnum.TableGlobalParam:
					return LoadAllTableGlobalParam(start, count);
				case (int)DataEnum.TableGuid:
					return LoadAllTableGuid(start, count);
				case (int)DataEnum.TableItemInfo:
					return LoadAllTableItemInfo(start, count);
				case (int)DataEnum.TableMailInfo:
					return LoadAllTableMailInfo(start, count);
				case (int)DataEnum.TableMailStateInfo:
					return LoadAllTableMailStateInfo(start, count);
				case (int)DataEnum.TableMemberInfo:
					return LoadAllTableMemberInfo(start, count);
				case (int)DataEnum.TableNicknameInfo:
					return LoadAllTableNicknameInfo(start, count);
				case (int)DataEnum.TableUserInfo:
					return LoadAllTableUserInfo(start, count);
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
				case (int)DataEnum.TableFriendInfo:
					return LoadSingleTableFriendInfo(primaryKeys);
				case (int)DataEnum.TableGlobalParam:
					return LoadSingleTableGlobalParam(primaryKeys);
				case (int)DataEnum.TableGuid:
					return LoadSingleTableGuid(primaryKeys);
				case (int)DataEnum.TableItemInfo:
					return LoadSingleTableItemInfo(primaryKeys);
				case (int)DataEnum.TableMailInfo:
					return LoadSingleTableMailInfo(primaryKeys);
				case (int)DataEnum.TableMailStateInfo:
					return LoadSingleTableMailStateInfo(primaryKeys);
				case (int)DataEnum.TableMemberInfo:
					return LoadSingleTableMemberInfo(primaryKeys);
				case (int)DataEnum.TableNicknameInfo:
					return LoadSingleTableNicknameInfo(primaryKeys);
				case (int)DataEnum.TableUserInfo:
					return LoadSingleTableUserInfo(primaryKeys);
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
				case (int)DataEnum.TableFriendInfo:
					return LoadMultiTableFriendInfo(foreignKeys);
				case (int)DataEnum.TableGlobalParam:
					return LoadMultiTableGlobalParam(foreignKeys);
				case (int)DataEnum.TableGuid:
					return LoadMultiTableGuid(foreignKeys);
				case (int)DataEnum.TableItemInfo:
					return LoadMultiTableItemInfo(foreignKeys);
				case (int)DataEnum.TableMailInfo:
					return LoadMultiTableMailInfo(foreignKeys);
				case (int)DataEnum.TableMailStateInfo:
					return LoadMultiTableMailStateInfo(foreignKeys);
				case (int)DataEnum.TableMemberInfo:
					return LoadMultiTableMemberInfo(foreignKeys);
				case (int)DataEnum.TableNicknameInfo:
					return LoadMultiTableNicknameInfo(foreignKeys);
				case (int)DataEnum.TableUserInfo:
					return LoadMultiTableUserInfo(foreignKeys);
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
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["IsBanned"];
			        msg.IsBanned = (bool)val;
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


		private static GeneralRecordData LoadSingleTableAccount(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableAccount";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = primaryKeys[0];
			    inputParam.Size = 64;
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableAccount msg = new TableAccount();
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["IsBanned"];
			        msg.IsBanned = (bool)val;
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
				    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.AccountId;
				    inputParam.Size = 64;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_IsBanned", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.IsBanned;
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


		private static int BatchSaveTableAccount(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableAccount ", 4096); 
			sbSql.Append("(IsValid,DataVersion,AccountId,IsBanned,UserGuid)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableAccount), out _msg)) {
			    TableAccount msg = _msg as TableAccount;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.AccountId);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsBanned);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" AccountId = if(DataVersion < {0}, values(AccountId), AccountId),", dataVersion);
			sbSql.AppendFormat(" IsBanned = if(DataVersion < {0}, values(IsBanned), IsBanned),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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


		private static int BatchSaveTableActivationCode(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableActivationCode ", 4096); 
			sbSql.Append("(IsValid,DataVersion,ActivationCode,IsActivated,AccountId)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableActivationCode), out _msg)) {
			    TableActivationCode msg = _msg as TableActivationCode;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ActivationCode);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsActivated);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.AccountId);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" ActivationCode = if(DataVersion < {0}, values(ActivationCode), ActivationCode),", dataVersion);
			sbSql.AppendFormat(" IsActivated = if(DataVersion < {0}, values(IsActivated), IsActivated),", dataVersion);
			sbSql.AppendFormat(" AccountId = if(DataVersion < {0}, values(AccountId), AccountId),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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
			        val = reader["QQ"];
			        msg.QQ = (long)val;
			        val = reader["weixin"];
			        msg.weixin = (long)val;
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
			        val = reader["QQ"];
			        msg.QQ = (long)val;
			        val = reader["weixin"];
			        msg.weixin = (long)val;
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
			        val = reader["QQ"];
			        msg.QQ = (long)val;
			        val = reader["weixin"];
			        msg.weixin = (long)val;
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
				    inputParam = new MySqlParameter("@_QQ", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.QQ;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_weixin", MySqlDbType.Int64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.weixin;
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


		private static int BatchSaveTableFriendInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableFriendInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,Guid,UserGuid,FriendGuid,FriendNickname,QQ,weixin,IsBlack)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableFriendInfo), out _msg)) {
			    TableFriendInfo msg = _msg as TableFriendInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Guid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.FriendGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.FriendNickname);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.QQ);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.weixin);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsBlack);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" Guid = if(DataVersion < {0}, values(Guid), Guid),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" FriendGuid = if(DataVersion < {0}, values(FriendGuid), FriendGuid),", dataVersion);
			sbSql.AppendFormat(" FriendNickname = if(DataVersion < {0}, values(FriendNickname), FriendNickname),", dataVersion);
			sbSql.AppendFormat(" QQ = if(DataVersion < {0}, values(QQ), QQ),", dataVersion);
			sbSql.AppendFormat(" weixin = if(DataVersion < {0}, values(weixin), weixin),", dataVersion);
			sbSql.AppendFormat(" IsBlack = if(DataVersion < {0}, values(IsBlack), IsBlack),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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


		private static int BatchSaveTableGlobalParam(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableGlobalParam ", 4096); 
			sbSql.Append("(IsValid,DataVersion,ParamType,ParamValue)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableGlobalParam), out _msg)) {
			    TableGlobalParam msg = _msg as TableGlobalParam;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ParamType);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ParamValue);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" ParamType = if(DataVersion < {0}, values(ParamType), ParamType),", dataVersion);
			sbSql.AppendFormat(" ParamValue = if(DataVersion < {0}, values(ParamValue), ParamValue),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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


		private static int BatchSaveTableGuid(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableGuid ", 4096); 
			sbSql.Append("(IsValid,DataVersion,GuidType,GuidValue)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableGuid), out _msg)) {
			    TableGuid msg = _msg as TableGuid;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.GuidType);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.GuidValue);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" GuidType = if(DataVersion < {0}, values(GuidType), GuidType),", dataVersion);
			sbSql.AppendFormat(" GuidValue = if(DataVersion < {0}, values(GuidValue), GuidValue),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static int BatchSaveTableItemInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableItemInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,ItemGuid,UserGuid,ItemId,ItemNum)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableItemInfo), out _msg)) {
			    TableItemInfo msg = _msg as TableItemInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ItemGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ItemId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ItemNum);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" ItemGuid = if(DataVersion < {0}, values(ItemGuid), ItemGuid),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" ItemId = if(DataVersion < {0}, values(ItemId), ItemId),", dataVersion);
			sbSql.AppendFormat(" ItemNum = if(DataVersion < {0}, values(ItemNum), ItemNum),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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


		private static int BatchSaveTableMailInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableMailInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,Guid,ModuleTypeId,Sender,Receiver,SendDate,ExpiryDate,Title,Text,Money,Gold,ItemIds,ItemNumbers,LevelDemand,IsRead)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableMailInfo), out _msg)) {
			    TableMailInfo msg = _msg as TableMailInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Guid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ModuleTypeId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Sender);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Receiver);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.SendDate);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ExpiryDate);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Title);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Text);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Money);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Gold);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ItemIds);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ItemNumbers);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.LevelDemand);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsRead);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" Guid = if(DataVersion < {0}, values(Guid), Guid),", dataVersion);
			sbSql.AppendFormat(" ModuleTypeId = if(DataVersion < {0}, values(ModuleTypeId), ModuleTypeId),", dataVersion);
			sbSql.AppendFormat(" Sender = if(DataVersion < {0}, values(Sender), Sender),", dataVersion);
			sbSql.AppendFormat(" Receiver = if(DataVersion < {0}, values(Receiver), Receiver),", dataVersion);
			sbSql.AppendFormat(" SendDate = if(DataVersion < {0}, values(SendDate), SendDate),", dataVersion);
			sbSql.AppendFormat(" ExpiryDate = if(DataVersion < {0}, values(ExpiryDate), ExpiryDate),", dataVersion);
			sbSql.AppendFormat(" Title = if(DataVersion < {0}, values(Title), Title),", dataVersion);
			sbSql.AppendFormat(" Text = if(DataVersion < {0}, values(Text), Text),", dataVersion);
			sbSql.AppendFormat(" Money = if(DataVersion < {0}, values(Money), Money),", dataVersion);
			sbSql.AppendFormat(" Gold = if(DataVersion < {0}, values(Gold), Gold),", dataVersion);
			sbSql.AppendFormat(" ItemIds = if(DataVersion < {0}, values(ItemIds), ItemIds),", dataVersion);
			sbSql.AppendFormat(" ItemNumbers = if(DataVersion < {0}, values(ItemNumbers), ItemNumbers),", dataVersion);
			sbSql.AppendFormat(" LevelDemand = if(DataVersion < {0}, values(LevelDemand), LevelDemand),", dataVersion);
			sbSql.AppendFormat(" IsRead = if(DataVersion < {0}, values(IsRead), IsRead),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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


		private static int BatchSaveTableMailStateInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableMailStateInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,Guid,UserGuid,MailGuid,IsRead,IsReceived,ExpiryDate)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableMailStateInfo), out _msg)) {
			    TableMailStateInfo msg = _msg as TableMailStateInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Guid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.MailGuid);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsRead);
			    sbValue.Append(',');
			    sbValue.Append(msg.IsReceived);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ExpiryDate);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" Guid = if(DataVersion < {0}, values(Guid), Guid),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" MailGuid = if(DataVersion < {0}, values(MailGuid), MailGuid),", dataVersion);
			sbSql.AppendFormat(" IsRead = if(DataVersion < {0}, values(IsRead), IsRead),", dataVersion);
			sbSql.AppendFormat(" IsReceived = if(DataVersion < {0}, values(IsReceived), IsReceived),", dataVersion);
			sbSql.AppendFormat(" ExpiryDate = if(DataVersion < {0}, values(ExpiryDate), ExpiryDate),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
		}


		private static List<GeneralRecordData> LoadAllTableMemberInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableMemberInfo";
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
			        TableMemberInfo msg = new TableMemberInfo();
			        val = reader["MemberGuid"];
			        msg.MemberGuid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
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


		private static GeneralRecordData LoadSingleTableMemberInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableMemberInfo";
			    if(primaryKeys.Count != 1)
				    throw new Exception("primary key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_MemberGuid", MySqlDbType.UInt64);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = (ulong)Convert.ChangeType(primaryKeys[0],typeof(ulong));
			    cmd.Parameters.Add(inputParam);
			    using (DbDataReader reader = cmd.ExecuteReader()) {
			      if (reader.Read()) {
			        ret = new GeneralRecordData();
			        object val;
			        TableMemberInfo msg = new TableMemberInfo();
			        val = reader["MemberGuid"];
			        msg.MemberGuid = (ulong)val;
			        ret.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
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


		private static List<GeneralRecordData> LoadMultiTableMemberInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadMultiTableMemberInfo";
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
			        TableMemberInfo msg = new TableMemberInfo();
			        val = reader["MemberGuid"];
			        msg.MemberGuid = (ulong)val;
			        record.PrimaryKeys.Add(val.ToString());
			        val = reader["UserGuid"];
			        msg.UserGuid = (ulong)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
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


		private static void SaveTableMemberInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableMemberInfo), out _msg)){
				TableMemberInfo msg = _msg as TableMemberInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableMemberInfo";
				    MySqlParameter inputParam;
				    inputParam = new MySqlParameter("@_IsValid", MySqlDbType.Bit);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = isValid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_DataVersion", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = dataVersion;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_MemberGuid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.MemberGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_UserGuid", MySqlDbType.UInt64);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.UserGuid;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_HeroId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.HeroId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static int BatchSaveTableMemberInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableMemberInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,MemberGuid,UserGuid,HeroId,Level)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableMemberInfo), out _msg)) {
			    TableMemberInfo msg = _msg as TableMemberInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.MemberGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.HeroId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Level);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" MemberGuid = if(DataVersion < {0}, values(MemberGuid), MemberGuid),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" HeroId = if(DataVersion < {0}, values(HeroId), HeroId),", dataVersion);
			sbSql.AppendFormat(" Level = if(DataVersion < {0}, values(Level), Level),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
		}


		private static List<GeneralRecordData> LoadAllTableNicknameInfo(int start, int count)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadAllTableNicknameInfo";
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
			        TableNicknameInfo msg = new TableNicknameInfo();
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


		private static GeneralRecordData LoadSingleTableNicknameInfo(List<string> primaryKeys)
		{
			GeneralRecordData ret = null;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.StoredProcedure;
			    cmd.CommandText = "LoadSingleTableNicknameInfo";
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
			        TableNicknameInfo msg = new TableNicknameInfo();
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


		private static List<GeneralRecordData> LoadMultiTableNicknameInfo(List<string> foreignKeys)
		{
			List<GeneralRecordData> ret = new List<GeneralRecordData>();
			return ret;
		}


		private static void SaveTableNicknameInfo(bool isValid, int dataVersion, byte[] data)
		{
			object _msg;
			if(DbDataSerializer.Decode(data, typeof(TableNicknameInfo), out _msg)){
				TableNicknameInfo msg = _msg as TableNicknameInfo;
				try {
				  using (MySqlCommand cmd = new MySqlCommand()) {
				    cmd.Connection = DBConn.MySqlConn;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "SaveTableNicknameInfo";
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


		private static int BatchSaveTableNicknameInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableNicknameInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,Nickname,UserGuid)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableNicknameInfo), out _msg)) {
			    TableNicknameInfo msg = _msg as TableNicknameInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Nickname);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.UserGuid);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" Nickname = if(DataVersion < {0}, values(Nickname), Nickname),", dataVersion);
			sbSql.AppendFormat(" UserGuid = if(DataVersion < {0}, values(UserGuid), UserGuid),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
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
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["SceneId"];
			        msg.SceneId = (int)val;
			        val = reader["PositionX"];
			        msg.PositionX = (float)val;
			        val = reader["PositionZ"];
			        msg.PositionZ = (float)val;
			        val = reader["FaceDir"];
			        msg.FaceDir = (float)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
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
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        ret.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["SceneId"];
			        msg.SceneId = (int)val;
			        val = reader["PositionX"];
			        msg.PositionX = (float)val;
			        val = reader["PositionZ"];
			        msg.PositionZ = (float)val;
			        val = reader["FaceDir"];
			        msg.FaceDir = (float)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
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
			    if(foreignKeys.Count != 1)
				    throw new Exception("foreign key number don't match !!!");
			    MySqlParameter inputParam;
			    inputParam = new MySqlParameter("@_AccountId", MySqlDbType.VarChar);
			    inputParam.Direction = ParameterDirection.Input;
			    inputParam.Value = foreignKeys[0];
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
			        val = reader["AccountId"];
			        msg.AccountId = (string)val;
			        record.ForeignKeys.Add(val.ToString());
			        val = reader["Nickname"];
			        msg.Nickname = (string)val;
			        val = reader["HeroId"];
			        msg.HeroId = (int)val;
			        val = reader["CreateTime"];
			        msg.CreateTime = (string)val;
			        val = reader["LastLogoutTime"];
			        msg.LastLogoutTime = (string)val;
			        val = reader["Level"];
			        msg.Level = (int)val;
			        val = reader["ExpPoints"];
			        msg.ExpPoints = (int)val;
			        val = reader["SceneId"];
			        msg.SceneId = (int)val;
			        val = reader["PositionX"];
			        msg.PositionX = (float)val;
			        val = reader["PositionZ"];
			        msg.PositionZ = (float)val;
			        val = reader["FaceDir"];
			        msg.FaceDir = (float)val;
			        val = reader["Money"];
			        msg.Money = (int)val;
			        val = reader["Gold"];
			        msg.Gold = (int)val;
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
				    inputParam = new MySqlParameter("@_LastLogoutTime", MySqlDbType.VarChar);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.LastLogoutTime;
				    inputParam.Size = 24;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Level", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Level;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_ExpPoints", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.ExpPoints;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_SceneId", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.SceneId;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PositionX", MySqlDbType.Float);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PositionX;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_PositionZ", MySqlDbType.Float);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.PositionZ;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_FaceDir", MySqlDbType.Float);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.FaceDir;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Money", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Money;
				    cmd.Parameters.Add(inputParam);
				    inputParam = new MySqlParameter("@_Gold", MySqlDbType.Int32);
				    inputParam.Direction = ParameterDirection.Input;
				    inputParam.Value = msg.Gold;
				    cmd.Parameters.Add(inputParam);
				    cmd.ExecuteNonQuery();
				  }
				} catch (Exception ex) {
				  DBConn.Close();
				  throw ex;
				}
			}
		}


		private static int BatchSaveTableUserInfo(List<bool> validList, List<byte[]> dataList, int dataVersion)
		{
			if (dataList.Count <= 0) {
			  return 0;
			}
			StringBuilder sbSql = new StringBuilder("insert into TableUserInfo ", 4096); 
			sbSql.Append("(IsValid,DataVersion,Guid,AccountId,Nickname,HeroId,CreateTime,LastLogoutTime,Level,ExpPoints,SceneId,PositionX,PositionZ,FaceDir,Money,Gold)");
			sbSql.Append(" values ");
			for (int i = 0; i < validList.Count; ++i) {
			  Byte valid = 1;
			  if (validList[i] == false) {
			    valid = 0;
			  }
			  StringBuilder sbValue = new StringBuilder();
			  sbValue.AppendFormat("({0},{1}", valid, dataVersion);
			  object _msg;
			  if (DbDataSerializer.Decode(dataList[i], typeof(TableUserInfo), out _msg)) {
			    TableUserInfo msg = _msg as TableUserInfo;
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Guid);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.AccountId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Nickname);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.HeroId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.CreateTime);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.LastLogoutTime);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Level);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.ExpPoints);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.SceneId);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.PositionX);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.PositionZ);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.FaceDir);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Money);
			    sbValue.Append(',');
			    sbValue.AppendFormat("'{0}'", msg.Gold);
			    sbValue.Append(')');
			    sbSql.Append(sbValue.ToString());
			    sbSql.Append(',');
			  }
			}
			sbSql.Remove(sbSql.Length - 1, 1);
			sbSql.Append(" on duplicate key update ");
			sbSql.AppendFormat(" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),", dataVersion);
			sbSql.AppendFormat(" Guid = if(DataVersion < {0}, values(Guid), Guid),", dataVersion);
			sbSql.AppendFormat(" AccountId = if(DataVersion < {0}, values(AccountId), AccountId),", dataVersion);
			sbSql.AppendFormat(" Nickname = if(DataVersion < {0}, values(Nickname), Nickname),", dataVersion);
			sbSql.AppendFormat(" HeroId = if(DataVersion < {0}, values(HeroId), HeroId),", dataVersion);
			sbSql.AppendFormat(" CreateTime = if(DataVersion < {0}, values(CreateTime), CreateTime),", dataVersion);
			sbSql.AppendFormat(" LastLogoutTime = if(DataVersion < {0}, values(LastLogoutTime), LastLogoutTime),", dataVersion);
			sbSql.AppendFormat(" Level = if(DataVersion < {0}, values(Level), Level),", dataVersion);
			sbSql.AppendFormat(" ExpPoints = if(DataVersion < {0}, values(ExpPoints), ExpPoints),", dataVersion);
			sbSql.AppendFormat(" SceneId = if(DataVersion < {0}, values(SceneId), SceneId),", dataVersion);
			sbSql.AppendFormat(" PositionX = if(DataVersion < {0}, values(PositionX), PositionX),", dataVersion);
			sbSql.AppendFormat(" PositionZ = if(DataVersion < {0}, values(PositionZ), PositionZ),", dataVersion);
			sbSql.AppendFormat(" FaceDir = if(DataVersion < {0}, values(FaceDir), FaceDir),", dataVersion);
			sbSql.AppendFormat(" Money = if(DataVersion < {0}, values(Money), Money),", dataVersion);
			sbSql.AppendFormat(" Gold = if(DataVersion < {0}, values(Gold), Gold),", dataVersion);
			sbSql.AppendFormat(" DataVersion = if(DataVersion < {0}, {0}, DataVersion),", dataVersion);
			sbSql.Remove(sbSql.Length - 1, 1);
			string statement = sbSql.ToString();
			int count = 0;
			try {
			  using (MySqlCommand cmd = new MySqlCommand()) {
			    cmd.Connection = DBConn.MySqlConn;
			    cmd.CommandType = CommandType.Text;
			    cmd.CommandText = statement;
			    count = cmd.ExecuteNonQuery();
			  }
			} catch (Exception ex) {
			  if (dataList.Count < 200) {
			    LogSys.Log(LOG_TYPE.ERROR, "Error Sql statement:{0}", statement);
			  }
			  DBConn.Close();
			  throw ex;
			}
			return count;
		}


	}
}
