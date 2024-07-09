using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Messenger;
using ScriptableFramework;
using GameFrameworkMessage;
using GameFrameworkData;

namespace ScriptableFramework
{
    internal sealed partial class DataCacheThread
    {
        //--------------------------------------------------------------------------------------------------------------------------
        // Methods for external direct calls, the actual execution thread is the calling thread.
        //--------------------------------------------------------------------------------------------------------------------------
        internal void SaveCreateUser(AccountInfo ai, string nickname, ulong userGuid)
        {
            try {
                TableAccount dataAccount = ai.ToProto();
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableAccount;
                msg.PrimaryKeys.Add(dataAccount.AccountId);
                msg.Data = DbDataSerializer.Encode(dataAccount);
                DispatchAction(SaveInternal, msg);

                TableNicknameInfo dataNickname = new TableNicknameInfo();
                dataNickname.Nickname = nickname;
                dataNickname.UserGuid = userGuid;
                Msg_LD_Save msgNickname = new Msg_LD_Save();
                msgNickname.MsgId = (int)DataEnum.TableNicknameInfo;
                msgNickname.PrimaryKeys.Add(dataNickname.Nickname);
                msgNickname.Data = DbDataSerializer.Encode(dataNickname);
                DispatchAction(SaveInternal, msgNickname);
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR. Msg:CreateUser, Key:{0}, Error:{1},\nStacktrace:{2}", ai.AccountId, e.Message, e.StackTrace);
            }
        }
        internal void SaveUser(UserInfo ui, int saveCount)
        {
            try {
                ulong userGuid = ui.Guid;
                string key = userGuid.ToString();
                if (ui.Modified) {
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableUserInfo;
                    msg.PrimaryKeys.AddRange(ui.PrimaryKeys);
                    msg.ForeignKeys.AddRange(ui.ForeignKeys);
                    msg.Data = DbDataSerializer.Encode(ui.ToProto());
                    DispatchAction(SaveInternal, msg);
                    ui.Modified = false;
                }
                foreach (var pair in ui.MailStateInfo.WholeMailStates) {
                    var mi = pair.Value;
                    if (mi.Modified) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableMailStateInfo;
                        msg.PrimaryKeys.AddRange(mi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(mi.ForeignKeys);
                        msg.Data = DbDataSerializer.Encode(mi.ToProto());
                        DispatchAction(SaveInternal, msg);
                        mi.Modified = false;
                    }
                }
                foreach (var mi in ui.MailStateInfo.DeletedWholeMailStates) {
                    if (mi.Deleted) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableMailStateInfo;
                        msg.PrimaryKeys.AddRange(mi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(mi.ForeignKeys);
                        msg.Data = null;
                        DispatchAction(SaveInternal, msg);
                        mi.Deleted = false;
                    }
                }
                ui.MailStateInfo.DeletedWholeMailStates.Clear();
                foreach (var mi in ui.MemberInfos) {
                    if (mi.Modified) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableMemberInfo;
                        msg.PrimaryKeys.AddRange(mi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(mi.ForeignKeys);
                        msg.Data = DbDataSerializer.Encode(mi.ToProto());
                        DispatchAction(SaveInternal, msg);
                        mi.Modified = false;
                    }
                }
                foreach (var mi in ui.DeletedMemberInfos) {
                    if (mi.Deleted) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableMemberInfo;
                        msg.PrimaryKeys.AddRange(mi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(mi.ForeignKeys);
                        msg.Data = null;
                        DispatchAction(SaveInternal, msg);
                        mi.Deleted = false;
                    }
                }
                ui.DeletedMemberInfos.Clear();
                foreach (var ii in ui.ItemBag.ItemInfos) {
                    if (ii.Modified) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableItemInfo;
                        msg.PrimaryKeys.AddRange(ii.PrimaryKeys);
                        msg.ForeignKeys.AddRange(ii.ForeignKeys);
                        msg.Data = DbDataSerializer.Encode(ii.ToProto());
                        DispatchAction(SaveInternal, msg);
                        ii.Modified = false;
                    }
                }
                foreach (var ii in ui.ItemBag.DeletedItemInfos) {
                    if (ii.Deleted) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableItemInfo;
                        msg.PrimaryKeys.AddRange(ii.PrimaryKeys);
                        msg.ForeignKeys.AddRange(ii.ForeignKeys);
                        msg.Data = null;
                        DispatchAction(SaveInternal, msg);
                        ii.Deleted = false;
                    }
                }
                ui.ItemBag.DeletedItemInfos.Clear();
                foreach (var fi in ui.FriendInfos) {
                    if (fi.Modified) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableFriendInfo;
                        msg.PrimaryKeys.AddRange(fi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(fi.ForeignKeys);
                        msg.Data = DbDataSerializer.Encode(fi.ToProto());
                        DispatchAction(SaveInternal, msg);
                        fi.Modified = false;
                    }
                }
                foreach (var fi in ui.DeletedFriendInfos) {
                    if (fi.Deleted) {
                        Msg_LD_Save msg = new Msg_LD_Save();
                        msg.MsgId = (int)DataEnum.TableFriendInfo;
                        msg.PrimaryKeys.AddRange(fi.PrimaryKeys);
                        msg.ForeignKeys.AddRange(fi.ForeignKeys);
                        msg.Data = DbDataSerializer.Encode(fi.ToProto());
                        DispatchAction(SaveInternal, msg);
                        fi.Deleted = false;
                    }
                }
                ui.DeletedFriendInfos.Clear();
                ui.CurrentUserSaveCount = saveCount;
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR. Msg:SaveUser, Key:{0}, SaveCount:{1}, Error:{2},\nStacktrace:{3}", ui.Guid, saveCount, e.Message, e.StackTrace);
            }
        }
        internal void SaveGuid(string guidType, ulong guidValue)
        {
            try {
                TableGuid dataGuid = new TableGuid();
                dataGuid.GuidType = guidType;
                dataGuid.GuidValue = guidValue;
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableGuid;
                msg.PrimaryKeys.Add(dataGuid.GuidType);
                msg.Data = DbDataSerializer.Encode(dataGuid);
                DispatchAction(SaveInternal, msg);
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void SaveGuid(List<GuidInfo> guidList, int saveCount)
        {
            try {
                foreach (var guidinfo in guidList) {
                    TableGuid dataGuid = new TableGuid();
                    dataGuid.GuidType = guidinfo.GuidType;
                    dataGuid.GuidValue = (ulong)guidinfo.NextGuid;
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableGuid;
                    msg.PrimaryKeys.Add(dataGuid.GuidType);
                    msg.Data = DbDataSerializer.Encode(dataGuid);
                    DispatchAction(SaveInternal, msg);
                }
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void SaveMail(List<TableMailInfoWrap> mailList, int saveCount)
        {
            try {
                foreach (var mailInfo in mailList) {
                    if (!mailInfo.Modified)
                        continue;
                    TableMailInfo dataMail = mailInfo.ToProto();
                    List<int> itemIds = new List<int>();
                    List<int> itemNums = new List<int>();
                    foreach (var item in mailInfo.m_Items) {
                        itemIds.Add(item.m_ItemId);
                        itemNums.Add(item.m_ItemNum);
                    }
                    dataMail.ItemIds = Converter.IntList2String(itemIds);
                    dataMail.ItemNumbers = Converter.IntList2String(itemNums);
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableMailInfo;
                    msg.PrimaryKeys.Add(dataMail.Guid.ToString());
                    msg.Data = DbDataSerializer.Encode(dataMail);
                    DispatchAction(SaveInternal, msg);
                }
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal void SaveDeletedMail(List<TableMailInfoWrap> mailList, int saveCount)
        {
            try {
                foreach (var mailInfo in mailList) {
                    if (!mailInfo.Deleted)
                        continue;
                    Msg_LD_Save msg = new Msg_LD_Save();
                    msg.MsgId = (int)DataEnum.TableMailInfo;
                    msg.PrimaryKeys.Add(mailInfo.Guid.ToString());
                    msg.Data = null;
                    DispatchAction(SaveInternal, msg);
                }
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        internal int SaveNickname(TableNicknameInfo nick)
        {
            try {
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableNicknameInfo;
                msg.PrimaryKeys.Add(nick.Nickname);
                msg.ForeignKeys.Clear();
                msg.Data = DbDataSerializer.Encode(nick);
                RequestSave(msg);
                return 0;
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
                return 0;
            }
        }
        internal int SaveGlobalData(TableGlobalData data)
        {
            try {
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableGlobalData;
                msg.PrimaryKeys.Add(data.Key);
                msg.ForeignKeys.Clear();
                msg.Data = DbDataSerializer.Encode(data);
                RequestSave(msg);
                return 0;
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
                return 0;
            }
        }
        internal int DeleteGlobalData(string key)
        {
            try {
                Msg_LD_Save msg = new Msg_LD_Save();
                msg.MsgId = (int)DataEnum.TableGlobalData;
                msg.PrimaryKeys.Add(key);
                msg.ForeignKeys.Clear();
                msg.Data = null;
                RequestSave(msg);
                return 0;
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
                return 0;
            }
        }
    }
}
