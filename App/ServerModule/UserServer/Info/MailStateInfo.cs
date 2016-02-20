using System;
using System.Collections.Generic;
using GameFramework;
using GameFrameworkData;
using GameFrameworkMessage;

namespace GameFramework
{
    public sealed class MailStateInfo
    {
        public object Lock
        {
            get { return m_Lock; }
        }
        //外部访问时先对Lock锁定！！！
        public Dictionary<ulong, MailState> WholeMailStates
        {
            get { return m_WholeMailStates; }
        }
        public int WholeMailCount
        {
            get
            {
                return m_WholeMailStates.Count;
            }
        }
        public bool HaveMail(ulong mailGuid)
        {
            bool ret = false;
            lock (m_Lock) {
                if (m_WholeMailStates.ContainsKey(mailGuid)) {
                    ret = true;
                }
            }
            return ret;
        }
        public void AddMail(ulong mailGuid, DateTime expiryDate)
        {
            lock (m_Lock) {
                MailState state = null;
                if (!m_WholeMailStates.ContainsKey(mailGuid)) {
                    state = new MailState();
                    state.MailGuid = mailGuid;
                    state.ExpiryDate = expiryDate;
                    state.IsRead = false;
                    state.IsReceived = false;
                    state.IsDeleted = false;
                    m_WholeMailStates.Add(mailGuid, state);
                }
            }
        }
        public void RemoveExpiredMails()
        {
            lock (m_Lock) {
                DateTime nowDate = DateTime.Now;
                foreach (KeyValuePair<ulong, MailState> pair in m_WholeMailStates) {
                    MailState state = pair.Value;
                    if (state.ExpiryDate < nowDate) {
                        m_ExpiredMails.Add(state.MailGuid);
                    }
                }
                foreach (ulong guid in m_ExpiredMails) {
                    m_WholeMailStates.Remove(guid);
                }
                m_ExpiredMails.Clear();
            }
        }
        public bool IsAlreadyRead(ulong mailGuid)
        {
            bool ret = true;
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    ret = state.IsRead;
                }
            }
            return ret;
        }
        public void ReadMail(ulong mailGuid)
        {
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    state.IsRead = true;
                }
            }
        }
        public bool IsAlreadyReceived(ulong mailGuid)
        {
            bool ret = false;
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    ret = state.IsReceived;
                }
            }
            return ret;
        }
        public void ReceiveMail(ulong mailGuid)
        {
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    state.IsReceived = true;
                }
            }
        }
        public bool IsAlreadyDeleted(ulong mailGuid)
        {
            bool ret = false;
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    ret = state.IsDeleted;
                }
            }
            return ret;
        }
        public void DeleteMail(ulong mailGuid)
        {
            lock (m_Lock) {
                MailState state;
                if (m_WholeMailStates.TryGetValue(mailGuid, out state)) {
                    state.IsDeleted = true;
                }
            }
        }
        public void Reset()
        {
            m_Lock = new object();
            m_WholeMailStates.Clear();
            m_ExpiredMails.Clear();
        }

        private object m_Lock = new object();
        private Dictionary<ulong, MailState> m_WholeMailStates = new Dictionary<ulong, MailState>();
        private List<ulong> m_ExpiredMails = new List<ulong>();
    }
}
