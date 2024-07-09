using System;
using System.Text;
using System.Diagnostics;
using CSharpCenterClient;
using System.Collections.Generic;

internal class UserServerConfig
{
    internal static bool DataStoreAvailable
    {
        get { return s_Instance.m_DataStoreFlag; }
    }

    internal static bool GMServerAvailable
    {
        get { return s_Instance.m_GMServerFlag; }
    }

    internal static bool IsDebug
    {
        get { return s_Instance.m_Debug; }
    }

    internal static bool EnableDirectLogin
    {
        get { return s_Instance.m_EnableDirectLogin; }
        set { s_Instance.m_EnableDirectLogin = value; }
    }

    internal static long UserDSSaveInterval
    {
        get { return s_Instance.m_UserSaveInterval; }
    }

    internal static bool ActivateCodeAvailable
    {
        get { return s_Instance.m_ActivateCodeAvailable; }
    }
    internal static string StartServerTime
    {
        get { return s_Instance.m_StartServerTime; }
        set { s_Instance.m_StartServerTime = value; }
    }

    internal static int WorldId
    {
        get { return s_Instance.m_WorldId; }
    }

    internal static int WorldIdNum
    {
        get { return s_Instance.m_WorldIdNum; }
    }

    internal static int WorldId0
    {
        get { return s_Instance.m_WorldId0; }
    }

    internal static int WorldId1
    {
        get { return s_Instance.m_WorldId1; }
    }

    internal static uint DSRequestTimeout
    {
        get { return s_Instance.m_DSRequestTimeout; }
    }

    internal static void Init()
    {
        StringBuilder sb = new StringBuilder(256);
        if (CenterHubApi.GetConfig("DataStoreFlag", sb, 256)) {
            string dsflag = sb.ToString();
            s_Instance.m_DataStoreFlag = (int.Parse(dsflag) != 0 ? true : false);
        }

        if (CenterHubApi.GetConfig("GMServerFlag", sb, 256)) {
            string gsflag = sb.ToString();
            s_Instance.m_GMServerFlag = (int.Parse(gsflag) != 0 ? true : false);
        }

        if (CenterHubApi.GetConfig("Debug", sb, 256)) {
            string debug = sb.ToString();
            s_Instance.m_Debug = (int.Parse(debug) != 0 ? true : false);

            if (s_Instance.m_Debug) {
                ScriptableFramework.GlobalVariables.Instance.IsDebug = true;
            }
        }

        if (CenterHubApi.GetConfig("EnableDirectLogin", sb, 256)) {
            string val = sb.ToString();
            s_Instance.m_EnableDirectLogin = (int.Parse(val) != 0 ? true : false);
        }

        if (CenterHubApi.GetConfig("UserSaveInterval", sb, 256)) {
            string saveinterval = sb.ToString();
            int val = int.Parse(saveinterval);
            if (s_Instance.m_UserSaveInterval != val)
                s_Instance.m_UserSaveInterval = val;
        }

        if (CenterHubApi.GetConfig("ActivateCodeAvailable", sb, 256)) {
            string activatecode = sb.ToString();
            s_Instance.m_ActivateCodeAvailable = (int.Parse(activatecode) != 0 ? true : false);
        }

        if (CenterHubApi.GetConfig("StartServerTime", sb, 256)) {
            string time = sb.ToString();
            s_Instance.m_StartServerTime = time;
        }

        if (CenterHubApi.GetConfig("worldid", sb, 256)) {
            string worldid = sb.ToString();
            int val = int.Parse(worldid);
            if (s_Instance.m_WorldId != val)
                s_Instance.m_WorldId = val;
        }

        if (CenterHubApi.GetConfig("centernum", sb, 256)) {
            string worldidnum = sb.ToString();
            int val = int.Parse(worldidnum);
            if (s_Instance.m_WorldIdNum != val)
                s_Instance.m_WorldIdNum = val;
        }

        if (CenterHubApi.GetConfig("worldid0", sb, 256)) {
            string worldid = sb.ToString();
            int val = int.Parse(worldid);
            if (s_Instance.m_WorldId0 != val)
                s_Instance.m_WorldId0 = val;
        }

        if (CenterHubApi.GetConfig("worldid1", sb, 256)) {
            string worldid = sb.ToString();
            int val = int.Parse(worldid);
            if (s_Instance.m_WorldId1 != val)
                s_Instance.m_WorldId1 = val;
        }

        if (CenterHubApi.GetConfig("DSRequestTimeout", sb, 256)) {
            s_Instance.m_DSRequestTimeout = uint.Parse(sb.ToString());
        }
    }

    private bool m_DataStoreFlag = false;
    private bool m_GMServerFlag = false;
    private bool m_Debug = false;
    private bool m_EnableDirectLogin = true;
    private long m_UserSaveInterval = 180000;
    private bool m_ActivateCodeAvailable = false;
    private int m_WorldId = -1;
    private int m_WorldIdNum = 0;
    private int m_WorldId0 = -1;
    private int m_WorldId1 = -1;
    private uint m_DSRequestTimeout = 45000;
    private string m_StartServerTime = string.Empty;
    private object m_Lock = new object();

    private static UserServerConfig s_Instance = new UserServerConfig();
}
