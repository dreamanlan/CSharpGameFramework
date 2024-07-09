using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using ScriptableFramework;

namespace ScriptableFramework
{
  /// <summary>
  /// !!!Note that this class instance will be called in multiple threads.
  /// </summary>
  internal sealed class OperationMeasure
  {
    internal bool CheckOperation(ulong guid)
    {
      bool ret = true;
      long curTime = TimeUtility.GetLocalMilliseconds();
      OperationInfo opInfo;
      if (m_OperationInfos.TryGetValue(guid, out opInfo)) {
        if (opInfo.m_LastTime + c_MonitorInterval < curTime) {
          opInfo.m_Count = 0;
          opInfo.m_LastTime = curTime;
        } else {
          ++opInfo.m_Count;
          if (opInfo.m_Count > c_MaxOperationCount) {
            ret = false;
          }
        }
      } else {
        opInfo = new OperationInfo();
        opInfo.m_LastTime = curTime;
        m_OperationInfos.TryAdd(guid, opInfo);
      }
      return ret;
    }

    private class OperationInfo
    {
      internal long m_LastTime = 0;
      internal int m_Count = 0;
    }

    private ConcurrentDictionary<ulong, OperationInfo> m_OperationInfos = new ConcurrentDictionary<ulong, OperationInfo>();

    private const long c_MonitorInterval = 5000;
    private const int c_MaxOperationCount = 50;

    internal static OperationMeasure Instance
    {
      get { return s_Instance; }
    }
    private static OperationMeasure s_Instance = new OperationMeasure();
  }
}
