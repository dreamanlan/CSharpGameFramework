using System;
using System.Text;
using CSharpCenterClient;

/// <summary>
/// mysql connection configuration
/// </summary>
internal class DataCacheConfig
{
    internal static bool IsPersistent
    {
        get { return s_Instance.m_IsPersistent; }
    }
    internal static string DataBase
    {
        get { return s_Instance.m_Database; }
    }
    internal static uint PersistentInterval
    {
        get { return s_Instance.m_PersistentInterval; }
    }

    internal static string MySqlConnectString
    {
        get
        {
            string mysqlConnString = string.Format("SERVER={0};UID={1};PWD={2};DATABASE={3};CHARSET=utf8",
                                        s_Instance.m_Server, s_Instance.m_User, s_Instance.m_Password, s_Instance.m_Database);
            return mysqlConnString;
        }
    }
    internal static int LoadThreadNum
    {
        get { return s_Instance.m_LoadThreadNum; }
    }
    internal static int SaveThreadNum
    {
        get { return s_Instance.m_SaveThreadNum; }
    }
    internal static int TablePieceCapacity
    {
        get { return s_Instance.m_TablePieceCapacity; }
    }

    internal static void Init()
    {
        try {
            StringBuilder sb = new StringBuilder(256);
            if (CenterClientApi.GetConfig("IsPersistent", sb, 256)) {
                string str = sb.ToString();
                s_Instance.m_IsPersistent = int.Parse(str) != 0;
            }
            if (CenterClientApi.GetConfig("Server", sb, 256)) {
                string str = sb.ToString();
                if (null == s_Instance.m_Server || 0 != s_Instance.m_Server.CompareTo(str))
                    s_Instance.m_Server = str;
            }
            if (CenterClientApi.GetConfig("User", sb, 256)) {
                string str = sb.ToString();
                if (null == s_Instance.m_User || 0 != s_Instance.m_User.CompareTo(str))
                    s_Instance.m_User = str;
            }
            if (CenterClientApi.GetConfig("Password", sb, 256)) {
                string str = sb.ToString();
                if (null == s_Instance.m_Password || 0 != s_Instance.m_Password.CompareTo(str))
                    s_Instance.m_Password = str;
            }
            if (CenterClientApi.GetConfig("Database", sb, 256)) {
                string str = sb.ToString();
                if (null == s_Instance.m_Database || 0 != s_Instance.m_Database.CompareTo(str))
                    s_Instance.m_Database = str;
            }
            if (CenterClientApi.GetConfig("PersistentInterval", sb, 256)) {
                uint val = uint.Parse(sb.ToString());
                if (s_Instance.m_PersistentInterval != val)
                    s_Instance.m_PersistentInterval = val;
            }
            if (CenterClientApi.GetConfig("LoadThreadNum", sb, 256)) {
                int val = int.Parse(sb.ToString());
                if (s_Instance.m_LoadThreadNum != val)
                    s_Instance.m_LoadThreadNum = val;
            }
            if (CenterClientApi.GetConfig("SaveThreadNum", sb, 256)) {
                int val = int.Parse(sb.ToString());
                if (s_Instance.m_SaveThreadNum != val)
                    s_Instance.m_SaveThreadNum = val;
            }
            if (CenterClientApi.GetConfig("TablePieceCapacity", sb, 256)) {
                int val = int.Parse(sb.ToString());
                if (s_Instance.m_TablePieceCapacity != val)
                    s_Instance.m_TablePieceCapacity = val;
            }
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "DataStoreConfig.Init throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private bool m_IsPersistent = false;
    private string m_Server = "127.0.0.1";
    private string m_User = "dfds";
    private string m_Password = "dfds";
    private string m_Database = "dsnode";
    private uint m_PersistentInterval = 120000;
    private int m_LoadThreadNum = 40;
    private int m_SaveThreadNum = 40;
    private int m_TablePieceCapacity = 10000;

    private static DataCacheConfig s_Instance = new DataCacheConfig();
}