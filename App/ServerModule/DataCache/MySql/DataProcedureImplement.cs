using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

internal static class DataProcedureImplement
{
    internal static string GetDSNodeVersion()
    {
        if (DataCacheConfig.IsPersistent) {
            string version = string.Empty;
            try {
                using (MySqlCommand cmd = new MySqlCommand()) {
                    cmd.Connection = DBConn.MySqlConn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDSNodeVersion";
                    cmd.Parameters.Add("@dsversion", MySqlDbType.String).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    version = (string)cmd.Parameters["@dsversion"].Value;
                }
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(LOG_TYPE.ERROR, "GetDSNodeVersion procedure ERROR:{0}\n Stacktrace:{1}", ex.Message, ex.StackTrace);
            }
            return version;
        } else {
            return DataCacheVersion.Version;
        }
    }

    internal static int GetGlobalDataVersion()
    {
        if (DataCacheConfig.IsPersistent) {
            int version = -1;
            try {
                using (MySqlCommand cmd = new MySqlCommand()) {
                    cmd.Connection = DBConn.MySqlConn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetGlobalDataVersion";
                    cmd.Parameters.Add("@dataVersion", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    version = (int)cmd.Parameters["@dataVersion"].Value;
                }
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(LOG_TYPE.ERROR, "GetGlobalDataVersion procedure ERROR:{0}\n Stacktrace:{1}", ex.Message, ex.StackTrace);
            }
            return version;
        } else {
            return 0;
        }
    }

    internal static void SetGlobalDataVersion(int dataVersion)
    {
        if (DataCacheConfig.IsPersistent) {
            int oldDataVersion = GetGlobalDataVersion();
            if (dataVersion > oldDataVersion) {
                //数据库中的GlobalDataVersion小
                try {
                    using (MySqlCommand cmd = new MySqlCommand()) {
                        cmd.Connection = DBConn.MySqlConn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SetGlobalDataVersion";
                        MySqlParameter inputParam = new MySqlParameter("@dataVersion", MySqlDbType.Int32);
                        inputParam.Direction = ParameterDirection.Input;
                        inputParam.Value = dataVersion;
                        cmd.Parameters.Add(inputParam);
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception ex) {
                    DBConn.Close();
                    LogSys.Log(LOG_TYPE.ERROR, "SetGlobalDataVersion procedure ERROR:{0}\n Stacktrace:{1}", ex.Message, ex.StackTrace);
                }
            } else {
                //数据库中的GlobalDataVersion大,异常情况!
                LogSys.Log(LOG_TYPE.ERROR, "SetGlobalDataVersion ERROR. Old GlobalDataVersion is bigger. OldGlobalDataVersion:{0}, NewGlobalDataVersion:{1}",
                  oldDataVersion, dataVersion);
            }
        }
    }
}

