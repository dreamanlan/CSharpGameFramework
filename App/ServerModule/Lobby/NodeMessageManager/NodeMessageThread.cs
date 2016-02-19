using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace Lobby
{
  internal class NodeMessageThread
  {
    internal NodeMessageThread(int tickSleepTime, int actionNumPerTick)
    {
      m_TickSleepTime = tickSleepTime;
      m_ActionNumPerTick = actionNumPerTick;
      InitThread();
    }

    internal int TickSleepTime
    {
      get { return m_TickSleepTime; }
      set { m_TickSleepTime = value; }
    }

    internal int ActionNumPerTick
    {
      get
      {
        return m_ActionNumPerTick;
      }
      set
      {
        m_ActionNumPerTick = value;
      }
    }

    internal int CurActionNum
    {
      get
      {
        return m_MsgQueue.Count;
      }
    }

    internal int PoolCount
    {
      get
      {
        return m_MsgPool.Count;
      }
    }

    internal void Start()
    {
      m_IsRun = true;
      m_Thread.Start();
    }

    internal void Stop()
    {
      m_IsRun = false;
      m_Thread.Join();
    }

    internal Thread Thread
    {
      get
      {
        return m_Thread;
      }
    }

    internal void QueueMessage(uint seq, int sourceHandle, int destHandle, byte[] data)
    {
      try {
        NodeMessageInfo info;
        if (!m_MsgPool.TryDequeue(out info)) {
          info = new NodeMessageInfo();
        }
        info.Seq = seq;
        info.SourceHandle = sourceHandle;
        info.DestHandle = destHandle;
        info.Data = data;
        m_MsgQueue.Enqueue(info);
      } catch (Exception ex) {
        LogSys.Log(LOG_TYPE.ERROR, "NodeMessageThread.QueueMessage {0} {1} {2} throw exception:{3}\n{4}", seq, sourceHandle, destHandle, ex.Message, ex.StackTrace);
      }
    }

    private void HandleMessages(int maxCount)
    {
      try {
        for (int i = 0; i < maxCount; ++i) {
          if (m_MsgQueue.Count > 0) {
            NodeMessageInfo info = null;
            m_MsgQueue.TryDequeue(out info);
            if (null != info) {
              try {
                NodeMessageDispatcher.HandleNodeMessage(info.Seq, info.SourceHandle, info.DestHandle, info.Data);
                m_MsgPool.Enqueue(info);
              } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "NodeMessageThread NodeMessageDispatcher.HandleNodeMessage() throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
              }
            }
          } else {
            break;
          }
        }
      } catch (Exception ex) {
        LogSys.Log(LOG_TYPE.ERROR, "NodeMessageThread.HandleMessages throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
    }
    
    private void InitThread()
    {
      m_Thread = new Thread(this.Loop);
    }

    private void Loop()
    {
      try {
        while (m_IsRun) {
          HandleMessages(m_ActionNumPerTick);
          Thread.Sleep(m_TickSleepTime);
        }
      } catch (Exception ex) {
        LogSys.Log(LOG_TYPE.ERROR, "NodeMessageThread.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    private ConcurrentQueue<NodeMessageInfo> m_MsgQueue = new ConcurrentQueue<NodeMessageInfo>();
    private ConcurrentQueue<NodeMessageInfo> m_MsgPool = new ConcurrentQueue<NodeMessageInfo>();

    private Thread m_Thread = null;
    private bool m_IsRun = true;

    private int m_TickSleepTime = 10;
    private int m_ActionNumPerTick = 1024;
  }
}
