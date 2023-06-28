using System;
using System.Collections.Generic;
using System.Threading;

namespace Lobby
{
  internal class NodeMessageManager
  {
    internal void Init(int threadNum, int tickSleepTime, int actionNumPerTick)
    {
      m_ThreadNum = threadNum;
      for (int i = 0; i < threadNum; ++i) {
        NodeMessageThread thread = new NodeMessageThread(tickSleepTime, actionNumPerTick);
        m_NodeMessageThreads.Add(thread);
      }
    }

    internal void Start()
    {
      int threadNum = m_NodeMessageThreads.Count;
      for (int i = 0; i < threadNum; ++i) {
        NodeMessageThread thread = m_NodeMessageThreads[i];
        thread.Start();
      }
    }

    internal void Stop()
    {
      int threadNum = m_NodeMessageThreads.Count;
      for (int i = 0; i < threadNum; ++i) {
        NodeMessageThread thread = m_NodeMessageThreads[i];
        thread.Stop();
      }
    }

    internal void DispatchMessage(uint seq, int sourceHandle, int destHandle, byte[] data)
    {
      try {
        int index = Interlocked.Increment(ref m_TurnIndex) % m_ThreadNum;
        NodeMessageThread thread = m_NodeMessageThreads[index];
        thread.QueueMessage(seq, sourceHandle, destHandle, data);
      } catch (Exception ex) {
        LogSys.Log(ServerLogType.ERROR, "NodeMessageManager.DispatchMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    internal void TickMonitor()
    {
      int threadNum = m_NodeMessageThreads.Count;
      for (int i = 0; i < threadNum; ++i) {
        NodeMessageThread thread = m_NodeMessageThreads[i];

        LogSys.Log(ServerLogType.INFO, "NodeMessageManager.ThreadMessagePool {0} buffered {1} messages", thread.Thread.ManagedThreadId, thread.PoolCount);

        LogSys.Log(ServerLogType.MONITOR, "NodeMessageManager.ThreadActionQueue ThreadActionCount {0} {1}", thread.Thread.ManagedThreadId, thread.CurActionNum);
      }
    }

    private List<NodeMessageThread> m_NodeMessageThreads = new List<NodeMessageThread>();
    private int m_ThreadNum = 0;
    private int m_TurnIndex = 0;
  }
}