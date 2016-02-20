using System;
using System.Collections.Generic;
using GameFramework;
using GameFrameworkData;
using GameFrameworkMessage;

namespace GameFramework
{
  internal sealed class MailItem
  {
    internal int m_ItemId;
    internal int m_ItemNum;
  }
  internal sealed class MailInfo
  {
    internal bool m_AlreadyRead = false;
    internal ulong m_MailGuid = 0;
    internal string m_Title = "";
    internal string m_Sender = "";
    internal DateTime m_SendTime = new DateTime();
    internal DateTime m_ExpiryDate = new DateTime();
    internal string m_Text = "";
    internal ulong m_Receiver = 0;
    internal int m_LevelDemand = 0;
    internal List<MailItem> m_Items = new List<MailItem>();
    internal int m_Money = 0;
    internal int m_Gold = 0;
  }
}
