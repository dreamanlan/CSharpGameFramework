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
        internal List<MailInfo> TotalMailList
        {
            get
            {
                List<MailInfo> totalMailList = new List<MailInfo>(m_WholeMails);
                foreach (KeyValuePair<ulong, List<MailInfo>> pair in m_UserMails) {
                    var userMailList = pair.Value;
                    totalMailList.AddRange(userMailList);
                }
                return totalMailList;
            }
        }
        internal void InitMailData(List<MailInfo> mailList)
        {
            foreach (var mail in mailList) {
                if (mail.m_Receiver == 0) {
                    m_WholeMails.Add(mail);
                } else {
                    List<MailInfo> userMailList = null;
                    if (!m_UserMails.TryGetValue(mail.m_Receiver, out userMailList)) {
                        userMailList = new List<MailInfo>();
                        m_UserMails.Add(mail.m_Receiver, userMailList);
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
                List<MailInfo> mails;
                if (m_UserMails.TryGetValue(userGuid, out mails)) {
                    int ct = mails.Count;
                    for (int ix = 0; ix < ct; ++ix) {
                        MailInfo mailInfo = mails[ix];
                        if (mailInfo.m_ExpiryDate >= DateTime.Now) {
                            MailInfoForMessage mailInfoForMsg = new MailInfoForMessage();
                            mailInfoForMsg.m_AlreadyRead = mailInfo.m_AlreadyRead;
                            mailInfoForMsg.m_MailGuid = mailInfo.m_MailGuid;
                            mailInfoForMsg.m_Title = mailInfo.m_Title;
                            mailInfoForMsg.m_Sender = mailInfo.m_Sender;
                            mailInfoForMsg.m_SendTime = mailInfo.m_SendTime.ToString("yyyyMMddHHmmss");
                            mailInfoForMsg.m_Text = mailInfo.m_Text;
                            mailInfoForMsg.m_Money = mailInfo.m_Money;
                            mailInfoForMsg.m_Gold = mailInfo.m_Gold;
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
                    MailInfo mailInfo = m_WholeMails[ix];
                    if (mailInfo.m_LevelDemand <= user.Level
                      && mailInfo.m_SendTime >= user.CreateTime
                      && mailInfo.m_ExpiryDate >= DateTime.Now
                      && !mailStateInfo.IsAlreadyReceived(mailInfo.m_MailGuid)) {
                        if (!mailStateInfo.HaveMail(mailInfo.m_MailGuid)) {
                            mailStateInfo.AddMail(mailInfo.m_MailGuid, mailInfo.m_ExpiryDate);
                        }
                        MailInfoForMessage mailInfoForMsg = new MailInfoForMessage();
                        mailInfoForMsg.m_AlreadyRead = mailStateInfo.IsAlreadyRead(mailInfo.m_MailGuid);
                        mailInfoForMsg.m_MailGuid = mailInfo.m_MailGuid;
                        mailInfoForMsg.m_Title = mailInfo.m_Title;
                        mailInfoForMsg.m_Sender = mailInfo.m_Sender;
                        mailInfoForMsg.m_SendTime = mailInfo.m_SendTime.ToString("yyyyMMddHHmmss");
                        mailInfoForMsg.m_Text = mailInfo.m_Text;
                        mailInfoForMsg.m_Money = mailInfo.m_Money;
                        mailInfoForMsg.m_Gold = mailInfo.m_Gold;
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
        internal void SendUserMail(MailInfo userMail, int validityPeriod)
        {
            userMail.m_MailGuid = GenMailGuid();
            userMail.m_SendTime = DateTime.Now;
            userMail.m_ExpiryDate = userMail.m_SendTime.AddDays(validityPeriod);
            List<MailInfo> mails = null;
            if (!m_UserMails.TryGetValue(userMail.m_Receiver, out mails)) {
                mails = new List<MailInfo>();
                m_UserMails.Add(userMail.m_Receiver, mails);
            }
            mails.Add(userMail);
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userMail.m_Receiver);
            if (null != user && user.CurrentState != UserState.DropOrOffline) {
                NodeMessage newMailMsg = new NodeMessage(LobbyMessageDefine.Msg_LC_NotifyNewMail, userMail.m_Receiver);
                NodeMessageDispatcher.SendNodeMessage(user.NodeName, newMailMsg);
            }
        }
        internal void SendWholeMail(MailInfo wholeMail, int validityPeriod)
        {
            wholeMail.m_MailGuid = GenMailGuid();
            wholeMail.m_SendTime = DateTime.Now;
            wholeMail.m_ExpiryDate = wholeMail.m_SendTime.AddDays(validityPeriod);
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
                List<MailInfo> mails;
                if (m_UserMails.TryGetValue(userGuid, out mails)) {
                    if (null != mails) {
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].m_MailGuid == mailGuid) {
                                MailInfo info = mails[index];
                                info.m_AlreadyRead = true;
                                break;
                            }
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                int wholeCt = m_WholeMails.Count;
                for (int index = 0; index < wholeCt; ++index) {
                    MailInfo info = m_WholeMails[index];
                    if (info.m_MailGuid == mailGuid) {
                        mailStateInfo.ReadMail(mailGuid);
                        break;
                    }
                }
            }
        }
        private bool CheckBagCapacity(UserInfo user, MailInfo info)
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
            protoData.m_MailGuid = info.m_MailGuid;
            opMsg.m_ProtoData = protoData;
            NodeMessageDispatcher.SendNodeMessage(user.NodeName, opMsg);
            return result;
        }
        internal void ReceiveMail(ulong userGuid, ulong mailGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(userGuid);
            if (null != user) {
                List<MailInfo> mails;
                if (m_UserMails.TryGetValue(userGuid, out mails)) {
                    if (null != mails) {
                        MailInfo info = null;
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].m_MailGuid == mailGuid) {
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
                        MailInfo info = m_WholeMails[index];
                        if (info.m_MailGuid == mailGuid) {
                            if (CheckBagCapacity(user, info)) {
                                mailStateInfo.ReceiveMail(mailGuid);
                                if (info.m_LevelDemand <= user.Level && info.m_SendTime >= user.CreateTime && info.m_ExpiryDate >= DateTime.Now) {
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
                List<MailInfo> mails;
                if (m_UserMails.TryGetValue(userGuid, out mails)) {
                    if (null != mails) {
                        MailInfo info = null;
                        int ct = mails.Count;
                        int index = 0;
                        for (; index < ct; ++index) {
                            if (mails[index].m_MailGuid == mailGuid) {
                                info = mails[index];
                                break;
                            }
                        }
                        if (null != info) {
                            mails.RemoveAt(index);
                        }
                    }
                }
                MailStateInfo mailStateInfo = user.MailStateInfo;
                if (!mailStateInfo.IsAlreadyReceived(mailGuid)) {
                    int wholeCt = m_WholeMails.Count;
                    for (int index = 0; index < wholeCt; ++index) {
                        MailInfo info = m_WholeMails[index];
                        if (info.m_MailGuid == mailGuid) {
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
                    MailInfo mailInfo = m_WholeMails[index];
                    if (null != mailInfo) {
                        if (mailInfo.m_ExpiryDate < DateTime.Now) {
                            m_WholeMails.RemoveAt(index);
                        }
                    }
                }
                foreach (KeyValuePair<ulong, List<MailInfo>> pair in m_UserMails) {
                    var mails = pair.Value;
                    int mailCt = mails.Count;
                    for (int index = mailCt - 1; index >= 0; --index) {
                        MailInfo mailInfo = mails[index];
                        if (null != mailInfo) {
                            if (mailInfo.m_ExpiryDate < DateTime.Now) {
                                mails.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }

        private void ExtractMailAttachment(MailInfo info, ulong userGuid)
        {
            UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
            UserThread userThread = dataProcess.GetUserThread(userGuid);
            userThread.QueueAction(userThread.AddAssets, userGuid, info.m_Money, info.m_Gold);

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

        private Dictionary<ulong, List<MailInfo>> m_UserMails = new Dictionary<ulong, List<MailInfo>>();
        private List<MailInfo> m_WholeMails = new List<MailInfo>();

        private long m_LastTickTime = 0;
        private const long c_TickInterval = 60000;

        private bool m_IsDataLoaded = false;
    }
}
