using System;
using System.Collections.Generic;
using GameFramework;
using GameFrameworkData;
using GameFrameworkMessage;

namespace GameFramework
{
    /// <summary>
    /// 本邮件系统支持的邮件都是由系统发送，分为2类邮件：全员邮件与个人邮件。
    /// 邮件都保存在邮件系统时，不保存到人身上。
    /// 全员邮件邮件系统为所有人保存一份，每个人记录已经收取过的全员邮件（邮件打开后点击收取按钮为收取，查看邮件不会收取，通常只有带附件的邮件可以收取）。
    /// </summary>
    /// <remarks>
    /// 除GlobalProcess处，不要直接调用本类的方法，邮件系统只在GlobalProcess线程进行处理，其它线程应调用GlobalProcess.QueueAction处理邮件。
    /// </remarks>
    internal sealed class MailSystem
    {
        //全部邮件列表，包含全局邮件、模块邮件和个人邮件
        internal List<TableMailInfoWrap> TotalMailList
        {
            get
            {
                List<TableMailInfoWrap> totalMailList = new List<TableMailInfoWrap>(m_WholeMails);
                foreach (KeyValuePair<long, List<TableMailInfoWrap>> pair in m_UserMails) {
                    var userMailList = pair.Value;
                    totalMailList.AddRange(userMailList);
                }
                return totalMailList;
            }
        }
        internal List<TableMailInfoWrap> TotalDeletedMailList
        {
            get
            {
                List<TableMailInfoWrap> totalMailList = new List<TableMailInfoWrap>(m_DeletedWholeMails);
                foreach (KeyValuePair<long, List<TableMailInfoWrap>> pair in m_DeletedUserMails) {
                    var userMailList = pair.Value;
                    totalMailList.AddRange(userMailList);
                }
                return totalMailList;
            }
        }
        internal void InitMailData(List<TableMailInfoWrap> mailList)
        {
            foreach (var mail in mailList) {
                if (mail.Receiver == 0) {
                    m_WholeMails.Add(mail);
                } else {
                    List<TableMailInfoWrap> userMailList = null;
                    if (!m_UserMails.TryGetValue(mail.Receiver, out userMailList)) {
                        userMailList = new List<TableMailInfoWrap>();
                        m_UserMails.Add(mail.Receiver, userMailList);
                    }
                    userMailList.Add(mail);
                }
            }
            m_IsDataLoaded = true;
        }

        internal void GetMailList(ulong userGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userGuid);
            if (null != user) {
                List<MailInfoForMessage> mailList = new List<MailInfoForMessage>();
                List<TableMailInfoWrap> mails;
                if (m_UserMails.TryGetValue((long)userGuid, out mails)) {
                    int ct = mails.Count;
                    for (int ix = 0; ix < ct; ++ix) {
                        TableMailInfoWrap mailInfo = mails[ix];
                        if (mailInfo.ExpiryDate >= DateTime.Now) {
                            MailInfoForMessage mailInfoForMsg = new MailInfoForMessage();
                            mailInfoForMsg.m_AlreadyRead = mailInfo.IsRead;
                            mailInfoForMsg.m_MailGuid = mailInfo.Guid;
                            mailInfoForMsg.m_Title = mailInfo.Title;
                            mailInfoForMsg.m_Sender = mailInfo.Sender;
                            mailInfoForMsg.m_SendTime = mailInfo.SendDate.ToString("yyyyMMddHHmmss");
                            mailInfoForMsg.m_Text = mailInfo.Text;
                            mailInfoForMsg.m_Money = mailInfo.Money;
                            mailInfoForMsg.m_Gold = mailInfo.Gold;
                            int itemCt = mailInfo.m_Items.Count;
                            if (itemCt > 0) {
                                for (int index = 0; index < itemCt; ++index) {
                                    MailItemForMessage mailItem = new MailItemForMessage();
                                    mailItem.m_ItemId = mailInfo.m_Items[index].m_ItemId;
                                    mailItem.m_ItemNum = mailInfo.m_Items[index].m_ItemNum;
                                    mailInfoForMsg.m_Items.Add(mailItem);
                                }
                            }
                            mailList.Add(mailInfoForMsg);
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                //这里不对用户数据加锁，因为用户的邮件状态的改变都在这个线程完成（除上线时的数据加载）
                int wholeMailCt = m_WholeMails.Count;
                for (int ix = 0; ix < wholeMailCt; ++ix) {
                    TableMailInfoWrap mailInfo = m_WholeMails[ix];
                    if (mailInfo.LevelDemand <= user.Level
                      && mailInfo.SendDate >= user.CreateTime
                      && mailInfo.ExpiryDate >= DateTime.Now
                      && !mailStateInfo.IsAlreadyReceived(mailInfo.Guid)) {
                        if (!mailStateInfo.HaveMail(mailInfo.Guid)) {
                            mailStateInfo.AddMail(mailInfo.Guid, mailInfo.ExpiryDate);
                        }
                        MailInfoForMessage mailInfoForMsg = new MailInfoForMessage();
                        mailInfoForMsg.m_AlreadyRead = mailStateInfo.IsAlreadyRead(mailInfo.Guid);
                        mailInfoForMsg.m_MailGuid = mailInfo.Guid;
                        mailInfoForMsg.m_Title = mailInfo.Title;
                        mailInfoForMsg.m_Sender = mailInfo.Sender;
                        mailInfoForMsg.m_SendTime = mailInfo.SendDate.ToString("yyyyMMddHHmmss");
                        mailInfoForMsg.m_Text = mailInfo.Text;
                        mailInfoForMsg.m_Money = mailInfo.Money;
                        mailInfoForMsg.m_Gold = mailInfo.Gold;
                        int itemCt = mailInfo.m_Items.Count;
                        if (itemCt > 0) {
                            for (int index = 0; index < itemCt; ++index) {
                                MailItemForMessage mailItem = new MailItemForMessage();
                                mailItem.m_ItemId = mailInfo.m_Items[index].m_ItemId;
                                mailItem.m_ItemNum = mailInfo.m_Items[index].m_ItemNum;
                                mailInfoForMsg.m_Items.Add(mailItem);
                            }
                        }
                        mailList.Add(mailInfoForMsg);
                    }
                }
                NodeMessage syncMailListMsg = new NodeMessage(LobbyMessageDefine.Msg_LC_SyncMailList, userGuid);
                GameFrameworkMessage.Msg_LC_SyncMailList protoMsg = new Msg_LC_SyncMailList();
                protoMsg.m_Mails.AddRange(mailList);
                syncMailListMsg.m_ProtoData = protoMsg;
                NodeMessageDispatcher.SendNodeMessage(user.NodeName, syncMailListMsg);
            }
        }
        internal void SendUserMail(TableMailInfoWrap userMail, int validityPeriod)
        {
            userMail.Guid = GenMailGuid();
            userMail.SendDate = DateTime.Now;
            userMail.ExpiryDate = userMail.SendDate.AddDays(validityPeriod);
            List<TableMailInfoWrap> mails = null;
            if (!m_UserMails.TryGetValue(userMail.Receiver, out mails)) {
                mails = new List<TableMailInfoWrap>();
                m_UserMails.Add(userMail.Receiver, mails);
            }
            mails.Add(userMail);
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo((ulong)userMail.Receiver);
            if (null != user && user.CurrentState != UserState.DropOrOffline) {
                NodeMessage newMailMsg = new NodeMessage(LobbyMessageDefine.Msg_LC_NotifyNewMail, (ulong)userMail.Receiver);
                NodeMessageDispatcher.SendNodeMessage(user.NodeName, newMailMsg);
            }
        }
        internal void SendWholeMail(TableMailInfoWrap wholeMail, int validityPeriod)
        {
            wholeMail.Guid = GenMailGuid();
            wholeMail.SendDate = DateTime.Now;
            wholeMail.ExpiryDate = wholeMail.SendDate.AddDays(validityPeriod);
            m_WholeMails.Add(wholeMail);
            NodeMessage newMailMsg = new NodeMessage(LobbyMessageDefine.Msg_LC_NotifyNewMail);
            NodeMessageWithGuid headerData = new NodeMessageWithGuid();
            newMailMsg.m_NodeHeader = headerData;
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            dataProcess.VisitUsers((UserInfo userInfo) => {
                headerData.m_Guid = userInfo.Guid;
                NodeMessageDispatcher.SendNodeMessage(userInfo.NodeName, newMailMsg);
            });
        }
        internal void ReadMail(ulong userGuid, ulong mailGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userGuid);
            if (null != user) {
                List<TableMailInfoWrap> mails;
                if (m_UserMails.TryGetValue((long)userGuid, out mails)) {
                    if (null != mails) {
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].Guid == mailGuid) {
                                TableMailInfoWrap info = mails[index];
                                info.IsRead = true;
                                break;
                            }
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                int wholeCt = m_WholeMails.Count;
                for (int index = 0; index < wholeCt; ++index) {
                    TableMailInfoWrap info = m_WholeMails[index];
                    if (info.Guid == mailGuid) {
                        mailStateInfo.ReadMail(mailGuid);
                        break;
                    }
                }
            }
        }
        private bool CheckBagCapacity(UserInfo user, TableMailInfoWrap info)
        {
            bool result = true;
            if (null == user || null == user.ItemBag
              || null == info || null == info.m_Items) {
                return result;
            }
            int ct = info.m_Items.Count;
            int free = user.ItemBag.GetFreeCount();
            if (ct > free) {
                result = false;
            }
            NodeMessage opMsg = new NodeMessage(LobbyMessageDefine.Msg_LC_LackOfSpace, user.Guid);
            Msg_LC_LackOfSpace protoData = new Msg_LC_LackOfSpace();
            protoData.m_Succeed = result;
            protoData.m_ReceiveNum = ct;
            protoData.m_FreeNum = free;
            protoData.m_MailGuid = info.Guid;
            opMsg.m_ProtoData = protoData;
            NodeMessageDispatcher.SendNodeMessage(user.NodeName, opMsg);
            return result;
        }
        internal void ReceiveMail(ulong userGuid, ulong mailGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userGuid);
            if (null != user) {
                List<TableMailInfoWrap> mails;
                if (m_UserMails.TryGetValue((long)userGuid, out mails)) {
                    if (null != mails) {
                        TableMailInfoWrap info = null;
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].Guid == mailGuid) {
                                info = mails[index];
                                break;
                            }
                        }
                        if (null != info && CheckBagCapacity(user, info)) {
                            mails.RemoveAt(index);
                            ExtractMailAttachment(info, userGuid);
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                if (!mailStateInfo.IsAlreadyReceived(mailGuid)) {
                    int wholeCt = m_WholeMails.Count;
                    for (int index = 0; index < wholeCt; ++index) {
                        TableMailInfoWrap info = m_WholeMails[index];
                        if (info.Guid == mailGuid) {
                            if (CheckBagCapacity(user, info)) {
                                mailStateInfo.ReceiveMail(mailGuid);
                                if (info.LevelDemand <= user.Level && info.SendDate >= user.CreateTime && info.ExpiryDate >= DateTime.Now) {
                                    ExtractMailAttachment(info, userGuid);
                                }
                            }
                        }
                    }
                }
            }
        }
        internal void DeleteMail(ulong userGuid, ulong mailGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userGuid);
            if (null != user) {
                List<TableMailInfoWrap> mails;
                if (m_UserMails.TryGetValue((long)userGuid, out mails)) {
                    if (null != mails) {
                        TableMailInfoWrap info = null;
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].Guid == mailGuid) {
                                info = mails[index];
                                break;
                            }
                        }
                        if (null != info) {
                            AddDeletedUserMail((long)userGuid, info);
                            mails.RemoveAt(index);
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                if (!mailStateInfo.IsAlreadyReceived(mailGuid)) {
                    int wholeCt = m_WholeMails.Count;
                    for (int index = 0; index < wholeCt; ++index) {
                        TableMailInfoWrap info = m_WholeMails[index];
                        if (info.Guid == mailGuid) {
                            mailStateInfo.DeleteMail(mailGuid);
                        }
                    }
                }
            }
        }
        internal void Tick()
        {
            var ds_thread = UserServer.Instance.DataCacheThread;
            if (ds_thread.DataStoreAvailable) {
                if (!m_IsDataLoaded) {
                    return;
                }
            }
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime + c_TickInterval < curTime) {
                m_LastTickTime = curTime;
                //清理过期邮件
                int ct = m_WholeMails.Count;
                for (int index = ct - 1; index >= 0; --index) {
                    TableMailInfoWrap mailInfo = m_WholeMails[index];
                    if (null != mailInfo) {
                        if (mailInfo.ExpiryDate < DateTime.Now) {
                            AddDeletedWholeMail(mailInfo);
                            m_WholeMails.RemoveAt(index);
                        }
                    }
                }
                foreach (KeyValuePair<long, List<TableMailInfoWrap>> pair in m_UserMails) {
                    var mails = pair.Value;
                    int mailCt = mails.Count;
                    for (int index = mailCt - 1; index >= 0; --index) {
                        TableMailInfoWrap mailInfo = mails[index];
                        if (null != mailInfo) {
                            if (mailInfo.ExpiryDate < DateTime.Now) {
                                AddDeletedUserMail(pair.Key, mailInfo);
                                mails.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }

        internal void ResetDeletedMailList()
        {
            m_DeletedWholeMails.Clear();
            m_DeletedUserMails.Clear();
        }
        
        private void AddDeletedWholeMail(TableMailInfoWrap info)
        {
            info.Deleted = true;
            m_DeletedWholeMails.Add(info);
        }

        private void AddDeletedUserMail(long userGuid, TableMailInfoWrap info)
        {
            List<TableMailInfoWrap> list;
            if (!m_DeletedUserMails.TryGetValue(userGuid, out list)) {
                list = new List<TableMailInfoWrap>();
                m_DeletedUserMails.Add(userGuid, list);
            }
            info.Deleted = true;
            list.Add(info);
        }

        private void ExtractMailAttachment(TableMailInfoWrap info, ulong userGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserThread userThread = dataProcess.GetUserThread(userGuid);
            userThread.QueueAction(userThread.AddAssets, userGuid, info.Money, info.Gold);

            int itemCt = info.m_Items.Count;
            for (int itemIx = 0; itemIx < itemCt; ++itemIx) {
                MailItem item = info.m_Items[itemIx];
                userThread.QueueAction(userThread.AddItem, userGuid, item.m_ItemId, item.m_ItemNum);
            }
        }
        private ulong GenMailGuid()
        {
            return UserServer.Instance.GlobalProcessThread.GenerateMailGuid();
        }

        private Dictionary<long, List<TableMailInfoWrap>> m_UserMails = new Dictionary<long, List<TableMailInfoWrap>>();
        private List<TableMailInfoWrap> m_WholeMails = new List<TableMailInfoWrap>();
        private Dictionary<long, List<TableMailInfoWrap>> m_DeletedUserMails = new Dictionary<long, List<TableMailInfoWrap>>();
        private List<TableMailInfoWrap> m_DeletedWholeMails = new List<TableMailInfoWrap>();

        private long m_LastTickTime = 0;
        private const long c_TickInterval = 60000;

        private bool m_IsDataLoaded = false;
    }
}
