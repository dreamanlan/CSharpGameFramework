using System;
using System.Text;
using CSharpCenterClient;
using System.Collections.Generic;

internal class LobbyConfig
{
  internal static bool IsDebug
  {
    get { return s_Instance.m_Debug; }
  }

  internal static int WorldId
  {
    get { return s_Instance.m_WorldId; }
  }

  internal static void Init()
  {
    StringBuilder sb = new StringBuilder(256);

    if (CenterClientApi.GetConfig("Debug", sb, 256)) {
      string debug = sb.ToString();
      s_Instance.m_Debug = (int.Parse(debug) != 0 ? true : false);

      if (s_Instance.m_Debug) {
        ScriptableFramework.GlobalVariables.Instance.IsDebug = true;
      }
    }

    if (CenterClientApi.GetConfig("worldid", sb, 256)) {
      string worldid = sb.ToString();
      int val = int.Parse(worldid);
      if (s_Instance.m_WorldId != val)
        s_Instance.m_WorldId = val;
    }
  }

  private bool m_Debug = false;
  private int m_WorldId = -1;

  private object m_Lock = new object();

  private static LobbyConfig s_Instance = new LobbyConfig();
}
