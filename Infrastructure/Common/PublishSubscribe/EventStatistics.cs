using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework
{
    public class EventStatistics
    {
        static bool[] m_EventPublishFlag = null;
        static List<int> m_IgnoreEventList = new List<int>();
        static public void AddIgnoreEvent(int evt)
        {
            m_IgnoreEventList.Add(evt);
        }
        static bool IsIgnoreEvent(int evt)
        {
            return m_IgnoreEventList.Contains(evt);
        }

        static public void InitEventPublishFlag(int evtCount)
        {
            m_EventPublishFlag = new bool[evtCount];
        }
        static public bool IsEventPublished(int evt)
        {
            if (m_EventPublishFlag == null)
            {
                return false;
            }
            if (IsIgnoreEvent(evt))
            {
                return false;
            }
            if (evt > 0 && evt < m_EventPublishFlag.Length)
            {
                return m_EventPublishFlag[evt];
            }
            return false;
        }
        static public void SetEventPublished(int evt, bool bFlag)
        {
            if (m_EventPublishFlag == null)
            {
                return ;
            }
            if (evt > 0 && evt < m_EventPublishFlag.Length)
            {
                m_EventPublishFlag[evt] = bFlag;
            }
        }
    }
}
