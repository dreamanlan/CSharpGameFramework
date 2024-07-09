using System;
using System.Collections.Generic;
using System.Threading;
using GameFrameworkData;
using ScriptableFramework;

namespace ScriptableFramework
{
  internal class GuidInfo
  {
    internal string GuidType
    { get; set; }
    internal long NextGuid
    { get; set; }
  }

  internal sealed class GuidSystem
  {   
    internal List<GuidInfo> GuidList
    {
      get
      {
        if (m_GuidList.Count == c_GuidCount) {
          m_GuidList[0].NextGuid = m_NextUserGuid;
          m_GuidList[1].NextGuid = m_NextMailGuid;
          m_GuidList[2].NextGuid = m_NextItemGuid;
          m_GuidList[3].NextGuid = m_NextMemberGuid;
        } else {
          LogSys.Log(ServerLogType.ERROR, "GlobalData GuidSystem ERROR. Get GuidList error. GuidList Count:{0}", m_GuidList.Count);
        }                
        return m_GuidList;
      }    
    }
    internal void InitGuidData(List<GuidInfo> guidList)
    {
      s_WorldId = (ulong)UserServerConfig.WorldId;
      foreach (var dataGuid in guidList) {        
        if (dataGuid.GuidType.Equals(s_UserGuidType)) {
          m_NextUserGuid = dataGuid.NextGuid;          
        }
      }
      GuidInfo userGuidInfo = new GuidInfo();
      userGuidInfo.GuidType = s_UserGuidType;
      userGuidInfo.NextGuid = m_NextUserGuid;
      m_GuidList.Add(userGuidInfo);

      foreach (var dataGuid in guidList) {
        if (dataGuid.GuidType.Equals(s_MailGuidType)) {
          m_NextMailGuid = dataGuid.NextGuid;         
        }
      }      
      GuidInfo mailGuidInfo = new GuidInfo();
      mailGuidInfo.GuidType = s_MailGuidType;
      mailGuidInfo.NextGuid = m_NextMailGuid;
      m_GuidList.Add(mailGuidInfo);

      foreach (var dataGuid in guidList) {
        if (dataGuid.GuidType.Equals(s_ItemGuidType)) {
          m_NextItemGuid = dataGuid.NextGuid;
        }
      }
      GuidInfo itemGuidInfo = new GuidInfo();
      itemGuidInfo.GuidType = s_ItemGuidType;
      itemGuidInfo.NextGuid = m_NextItemGuid;
      m_GuidList.Add(itemGuidInfo);

      foreach (var dataGuid in guidList) {
        if (dataGuid.GuidType.Equals(s_MemberGuidType)) {
          m_NextMemberGuid = dataGuid.NextGuid;
        }
      }
      GuidInfo memberGuidInfo = new GuidInfo();
      memberGuidInfo.GuidType = s_MemberGuidType;
      memberGuidInfo.NextGuid = m_NextMemberGuid;
      m_GuidList.Add(memberGuidInfo);
    }    
    internal ulong GenerateUserGuid()
    {      
      ulong serial = (ulong)Interlocked.Increment(ref m_NextUserGuid) - 1;
      if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
        UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_UserGuidType, serial);
      }
      return serial * 10000 + s_WorldId;
    }
    internal ulong GenerateMailGuid()
    {     
      ulong serial = (ulong)Interlocked.Increment(ref m_NextMailGuid) - 1;     
      if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
        UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_MailGuidType, serial);
      }
      return serial * 10000 + s_WorldId;
    }
    internal ulong GenerateItemGuid()
    {      
      ulong serial = (ulong)Interlocked.Increment(ref m_NextItemGuid) - 1;
      if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
        UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_ItemGuidType, serial);
      }
      return serial * 10000 + s_WorldId;
    }
    internal ulong GenerateMemberGuid()
    {     
      ulong serial = (ulong)Interlocked.Increment(ref m_NextMemberGuid) - 1;
      if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
        UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_MemberGuidType, serial);
      }
      return serial * 10000 + s_WorldId;
    }
    internal ulong GenerateFriendGuid()
    {
        ulong serial = (ulong)Interlocked.Increment(ref m_NextFriendGuid) - 1;
        if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
            UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_FriendGuidType, serial);
        }
        return serial * 10000 + s_WorldId;
    }
    internal ulong GenerateAuctionGuid()
    {
      ulong serial = (ulong)Interlocked.Increment(ref m_NextAuctionGuid) - 1;
      if (UserServer.Instance.DataCacheThread.DataStoreAvailable) {
        UserServer.Instance.DataCacheThread.SaveGuid(GuidSystem.s_AuctionGuidType, serial);
      }
      return serial * 10000 + s_WorldId;
    }
    internal static string s_UserGuidType = "UserGuid";    
    internal static string s_MailGuidType = "MailGuid";    
    internal static string s_ItemGuidType = "ItemGuid";
    internal static string s_MemberGuidType = "MemberGuid";
    internal static string s_FriendGuidType = "FriendGuid";
    internal static string s_AuctionGuidType = "AuctionGuid";

    private long m_NextUserGuid = 1;
    private long m_NextMailGuid = 1;
    private long m_NextItemGuid = 1;
    private long m_NextMemberGuid = 1;
    private long m_NextFriendGuid = 1;
    private long m_NextAuctionGuid = 1;
   
    private static ulong s_WorldId = 0;

    private Dictionary<string, GuidInfo> m_GuidDict = new Dictionary<string, GuidInfo>();

    private List<GuidInfo> m_GuidList = new List<GuidInfo>();
    private const int c_GuidCount = 4;
  }
}
