using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameFrameworkData;
using GameFramework;
using System.Diagnostics;

namespace GameFramework
{
  internal sealed class NicknameSystem
  {
    internal enum CheckNicknameResult
    {
      Success = 0,
      AlreadyUsed = 1,
      Error = 2
    }
    //创建昵称数据
    internal void CreateData()
    {
      string prefix = "Temp_";
      int index = 0;      
      while (true) {
        string name = string.Format("{0}{1}", prefix, index);
        NicknameInfo data;
        if (!m_UnusedNicknames.TryGetValue(name, out data)) {
          data = new NicknameInfo();
          data.Nickname = name;
          data.UserGuid = 0;
          m_UnusedNicknames.TryAdd(name, data);
          index++;
        }        
        if (index >= c_NumberForCreate) {
          break;
        }
      }      
    }
    //初始化昵称数据，程序启动时从数据库中加载
    internal void InitNicknameData(List<TableNicknameInfo> nicknameList)
    {
      if (nicknameList.Count > 0) {
        foreach (var dataNickname in nicknameList) {
          NicknameInfo wrap = new NicknameInfo();
          wrap.FromProto(dataNickname);
          if (dataNickname.UserGuid > 0) {
            m_UsedNicknames.TryAdd(dataNickname.Nickname, wrap);
          } else {
            m_UnusedNicknames.TryAdd(dataNickname.Nickname, wrap);
          }
        }        
      } else {
        CreateData();  
      }    
      //m_IsDataLoaded = true;
    }
    //返回一定数量的可用昵称   
    internal List<string> RequestNicknames(string accountId)
    {
      const int c_MaxRandomCount = 1000;
      lock (m_Lock) {
        List<string> nicknameList = new List<string>();
        RevertAccountNicknames(accountId);
        if (m_UnusedNicknames.Count > c_NumberForOneRequest) {
          int leftCount = m_UnusedNicknames.Count - c_NumberForOneRequest;
          int randomStart = m_Random.Next(0, leftCount > c_MaxRandomCount ? c_MaxRandomCount : leftCount);
          int index = 0;
          foreach (KeyValuePair<string, NicknameInfo> pair in m_UnusedNicknames) {
            string key = pair.Key;
            if (null != key) {
              if (index < randomStart) {
                index++;
              } else if (index < randomStart + c_NumberForOneRequest) {
                nicknameList.Add(key);
                index++;
              } else {
                break;
              }
            }
          }
        } else {
          foreach (KeyValuePair<string, NicknameInfo> pair in m_UnusedNicknames) {
            string key = pair.Key;
            if (null != key) {
              nicknameList.Add(key);
            }
          }
        }
        if (nicknameList.Count > 0) {
          NicknameInfo outValue;
          for (int i = 0; i < nicknameList.Count; ++i) {
            if (m_UnusedNicknames.TryRemove(nicknameList[i], out outValue)) {
              m_UsedNicknames.AddOrUpdate(nicknameList[i], outValue, (g, u) => u);
            }
          }
          m_AccountReqNicknames.AddOrUpdate(accountId, nicknameList, (g, u) => nicknameList);          
        } else {
          //可用的昵称为空时,向客户端传送已使用的昵称
          if (m_UsedNicknames.Count > c_NumberForOneRequest) {
            int leftCount = m_UsedNicknames.Count - c_NumberForOneRequest;
            int randomStart = m_Random.Next(0, leftCount > c_MaxRandomCount ? c_MaxRandomCount : leftCount);
            int index = 0;
            foreach (KeyValuePair<string, NicknameInfo> pair in m_UsedNicknames) {
              string key = pair.Key;
              if (null != key) {
                if (index < randomStart) {
                  index++;
                } else if (index < randomStart + c_NumberForOneRequest) {
                  nicknameList.Add(key);
                  index++;
                } else {
                  break;
                }
              }
            }
          } else {
            foreach (KeyValuePair<string, NicknameInfo> pair in m_UnusedNicknames) {
              string key = pair.Key;
              if (null != key) {
                nicknameList.Add(key);
              }
            }
          }
        }
        return nicknameList;
      }
    }
    //昵称是否可用，可用返回true，不可用返回false   
    internal CheckNicknameResult CheckNickname(string accoountKey, string nickname)
    {
      lock (m_Lock) {
        CheckNicknameResult ret = CheckNicknameResult.Error;
        //验证昵称是否合法
        if (VerifyNickname(nickname)) {
          RevertAccountNicknames(accoountKey);
          NicknameInfo wrap;
          //验证昵称是否已存在
          if (!m_UsedNicknames.TryGetValue(nickname, out wrap)) {
            if (!m_UnusedNicknames.TryRemove(nickname, out wrap)) {
              wrap = new NicknameInfo();
              wrap.Nickname = nickname;
              wrap.UserGuid = 1;
            }
            m_UsedNicknames.TryAdd(nickname, wrap);
            ret = CheckNicknameResult.Success;
          } else {
            ret = CheckNicknameResult.AlreadyUsed;
          }
        }
        return ret;
      }
    }
    //返还账号申请的昵称
    internal void RevertAccountNicknames(string accountId)
    {
      List<string> abortNicknameList = null;
      if (m_AccountReqNicknames.TryGetValue(accountId, out abortNicknameList)) {
        foreach (string nickname in abortNicknameList) {
          NicknameInfo wrap;
          if (!m_UsedNicknames.TryRemove(nickname, out wrap)) {
            wrap = new NicknameInfo();
            wrap.Nickname = nickname;
          }
          wrap.UserGuid = 0;
          m_UnusedNicknames.AddOrUpdate(nickname, wrap, (g, u) => { u.UserGuid = 0; return u; });
        }
        m_AccountReqNicknames.TryRemove(accountId, out abortNicknameList);
      }
    }
    //根据Nickname查找UserGuid
    internal ulong FindUserGuidByNickname(string nickname)
    {
      ulong guid = 0;
      NicknameInfo wrap;
      if (m_UsedNicknames.TryGetValue(nickname, out wrap))
        guid = wrap.UserGuid;
      return guid;
    }
    //根据Nickname模糊查找UserGuid
    internal List<ulong> FindUserGuidByFuzzyNickname(string fuzzyName)
    {
      List<ulong> guidList = new List<ulong>();
      foreach (var usedNickname in m_UsedNicknames) {
        if (usedNickname.Key.Contains(fuzzyName)) {
          guidList.Add(usedNickname.Value.UserGuid);
        }
      }
      return guidList;
    }
    internal void UpdateUsedNickname(string nickname, ulong userGuid)
    {
      NicknameInfo wrap = new NicknameInfo();
      wrap.Nickname = nickname;
      wrap.UserGuid = userGuid;
      m_UsedNicknames.AddOrUpdate(nickname, wrap, (n, g) => { g.UserGuid = userGuid; return g; });
    }
    internal void Tick()
    {
      long curTime = TimeUtility.GetLocalMilliseconds();
      if (m_LastTickTime + c_TickInterval <= curTime || UserServer.Instance.WaitQuit) {
        m_LastTickTime = curTime;

        foreach (var pair in m_UnusedNicknames) {
          if (pair.Value.Modified) {
            TableNicknameInfo record = pair.Value.ToProto();
            UserServer.Instance.DataCacheThread.SaveNickname(record);
            pair.Value.Modified = false;
          }
        }
        foreach (var pair in m_UsedNicknames) {
          if (pair.Value.Modified) {
            TableNicknameInfo record = pair.Value.ToProto();
            UserServer.Instance.DataCacheThread.SaveNickname(record);
            pair.Value.Modified = false;
          }
        }
      }
    }
    internal static bool VerifyNickname(string nickname)
    {
      //服务器验证昵称是否合法
      if (!string.IsNullOrEmpty(nickname.Trim())) {
        char[] charArray = nickname.ToCharArray();
        bool ret = true;
        foreach (var ch in charArray) {
          if (!IsValidateCharacter(ch)){
            ret = false;
            break;
          }
        }
        return ret;
      } else {
        return false;
      }
    }
    private static bool IsValidateCharacter(char ch)
    {
      //增加汉字输入
      if (ch >= 0x4e00 && ch <= 0x9fa5) return true;
      // Lowercase and numbers
      if (ch >= 'A' && ch <= 'Z') return true;
      if (ch >= 'a' && ch <= 'z') return true;
      if (ch >= '0' && ch <= '9') return true;
      //增加特殊字符判段
      if (ch == '.') return true;
      if (ch == '_') return true;
      return false;
    }

    private ConcurrentDictionary<string, NicknameInfo> m_UnusedNicknames = new ConcurrentDictionary<string, NicknameInfo>();  //未使用的昵称
    private ConcurrentDictionary<string, NicknameInfo> m_UsedNicknames = new ConcurrentDictionary<string, NicknameInfo>();    //已使用的昵称   
    private ConcurrentDictionary<string, List<string>> m_AccountReqNicknames = new ConcurrentDictionary<string, List<string>>();    //客户端申请的昵称
    private const int c_NumberForOneRequest = 20;       //客户端一次申请获得的昵称数目  
    private const int c_NumberForCreate = 1000;         //初始创建的昵称数目
    private Random m_Random = new Random();
    private object m_Lock = new object();
    //private bool m_IsDataLoaded = false;

    private const long c_TickInterval = 10000;
    private long m_LastTickTime = 0;
  }
}
