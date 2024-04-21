using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Messenger;
using GameFramework;
using GameFrameworkMessage;
using GameFrameworkData;

namespace GameFramework
{
    internal sealed partial class DataCacheThread
    {
        //--------------------------------------------------------------------------------------------------------------------------
        //For methods called externally through QueueAction, the actual execution thread is DataCacheThread.
        //--------------------------------------------------------------------------------------------------------------------------
        internal void LoadAccount(string accountId)
        {
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableAccount;
            msg.PrimaryKeys.Add(accountId);
            Msg_LD_SingleLoadRequest reqAccount = new Msg_LD_SingleLoadRequest();
            reqAccount.MsgId = (int)DataEnum.TableAccount;
            reqAccount.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqAccount.Keys.Add(accountId);
            msg.LoadRequests.Add(reqAccount);
            RequestLoad(msg, (ret) => {
                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "DataCache Load Success: Msg:{0}, Key:{1}", "Account", primaryKey);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "Account", primaryKey);
                } else {
                    LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "Account", primaryKey, ret.ErrorInfo);
                }
                UserProcessScheduler dataProcessScheduler = UserServer.Instance.UserProcessScheduler;
                dataProcessScheduler.DefaultUserThread.QueueAction(dataProcessScheduler.LoadAccountCallback, accountId, ret);
            });
        }
        internal void LoadUser(ulong userGuid, string accountId, string nickname)
        {
            string key = userGuid.ToString();
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableUserInfo;
            msg.PrimaryKeys.Add(key);
            Msg_LD_SingleLoadRequest reqUser = new Msg_LD_SingleLoadRequest();
            reqUser.MsgId = (int)DataEnum.TableUserInfo;
            reqUser.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqUser.Keys.Add(key);
            msg.LoadRequests.Add(reqUser);
            Msg_LD_SingleLoadRequest reqMember = new Msg_LD_SingleLoadRequest();
            reqMember.MsgId = (int)DataEnum.TableMemberInfo;
            reqMember.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti;
            reqMember.Keys.Add(key);
            msg.LoadRequests.Add(reqMember);
            Msg_LD_SingleLoadRequest reqItem = new Msg_LD_SingleLoadRequest();
            reqItem.MsgId = (int)DataEnum.TableItemInfo;
            reqItem.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti;
            reqItem.Keys.Add(key);
            msg.LoadRequests.Add(reqItem);
            Msg_LD_SingleLoadRequest reqFriend = new Msg_LD_SingleLoadRequest();
            reqFriend.MsgId = (int)DataEnum.TableFriendInfo;
            reqFriend.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti;
            reqFriend.Keys.Add(key);
            msg.LoadRequests.Add(reqFriend);
            RequestLoad(msg, (ret) => {
                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, "DataCache Load Success: Msg:{0}, Key:{1}", "UserInfo", primaryKey);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "UserInfo", primaryKey);
                } else {
                    LogSys.Log(ServerLogType.ERROR, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "UserInfo", primaryKey, ret.ErrorInfo);
                }
                UserProcessScheduler dataProcessScheduler = UserServer.Instance.UserProcessScheduler;
                dataProcessScheduler.DefaultUserThread.QueueAction(dataProcessScheduler.LoadUserCallback, ret, accountId, nickname);
            });
        }
        //
        internal void GMLoadAccount(string gmAccount, string accountId, ulong nodeHandle)
        {
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableAccount;
            msg.PrimaryKeys.Add(accountId);
            Msg_LD_SingleLoadRequest reqAccount = new Msg_LD_SingleLoadRequest();
            reqAccount.MsgId = (int)DataEnum.TableAccount;
            reqAccount.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqAccount.Keys.Add(accountId);
            msg.LoadRequests.Add(reqAccount);
            RequestLoad(msg, (ret) => {
                JsonMessage resultMsg = new JsonMessage(LobbyGmMessageDefine.Msg_CL_GmQueryAccount, gmAccount);
                GameFrameworkMessage.Msg_LC_GmQueryAccount protoData = new GameFrameworkMessage.Msg_LC_GmQueryAccount();
                protoData.m_Result = GameFrameworkMessage.GmResultEnum.Failed;
                protoData.m_QueryAccount = accountId;
                protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Offline;
                resultMsg.m_ProtoData = protoData;

                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "DataCache Load Success: Msg:{0}, Key:{1}", "GmAccount", primaryKey);
                    TableAccount dataAccount = null;
                    foreach (var result in ret.Results) {
                        object _msg;
                        if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                            DataEnum msgEnum = (DataEnum)result.MsgId;
                            switch (msgEnum) {
                                case DataEnum.TableAccount:
                                    dataAccount = _msg as TableAccount;
                                    break;
                                default:
                                    LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Decode account data ERROR. Wrong message id. Account:{0}, WrongId:{1}", result.PrimaryKeys[0], msgEnum);
                                    break;
                            }
                        }
                    }
                    protoData.m_Result = GameFrameworkMessage.GmResultEnum.Success;
                    protoData.m_QueryAccount = dataAccount.AccountId;
                    protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Online;
                    if (dataAccount.IsBanned) {
                        protoData.m_AccountState = GameFrameworkMessage.GmStateEnum.Banned;
                    }
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "GmAccount", primaryKey);
                } else {
                    LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "GmAccount", primaryKey, ret.ErrorInfo);
                }
                resultMsg.m_ProtoData = protoData;
                JsonGmMessageDispatcher.SendNodeMessage(nodeHandle, resultMsg);
            });
        }
        internal void GMLoadUser(string gmAccount, ulong userGuid, LobbyGmMessageDefine jsonMsgId, ulong nodeHandle)
        {
            string key = userGuid.ToString();
            Msg_LD_Load msg = new Msg_LD_Load();
            msg.MsgId = (int)DataEnum.TableUserInfo;
            msg.PrimaryKeys.Add(key);
            Msg_LD_SingleLoadRequest reqUser = new Msg_LD_SingleLoadRequest();
            reqUser.MsgId = (int)DataEnum.TableUserInfo;
            reqUser.LoadType = Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle;
            reqUser.Keys.Add(key);
            msg.LoadRequests.Add(reqUser);
            RequestLoad(msg, (ret) => {
                JsonMessage resultMsg = new JsonMessage(jsonMsgId, gmAccount);
                GameFrameworkMessage.Msg_LC_GmQueryUser protoData = new GameFrameworkMessage.Msg_LC_GmQueryUser();
                protoData.m_Result = GameFrameworkMessage.GmResultEnum.Failed;
                protoData.m_Info = null;

                KeyString primaryKey = KeyString.Wrap(ret.PrimaryKeys);
                if (Msg_DL_LoadResult.ErrorNoEnum.Success == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, "DataCache Load Success: Msg:{0}, Key:{1}", "GmUser", primaryKey);
                    TableUserInfo dataUser = null;
                    foreach (var result in ret.Results) {
                        object _msg;
                        if (DbDataSerializer.Decode(result.Data, DataEnum2Type.Query(result.MsgId), out _msg)) {
                            DataEnum msgEnum = (DataEnum)result.MsgId;
                            switch (msgEnum) {
                                case DataEnum.TableUserInfo:
                                    dataUser = _msg as TableUserInfo;
                                    break;
                                default:
                                    LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Decode user data ERROR. Wrong message id. UserGuid:{0}, WrongId:{1}", userGuid, msgEnum);
                                    break;
                            }
                        }
                    }
                    protoData.m_Result = GameFrameworkMessage.GmResultEnum.Success;
                    protoData.m_Info = CreateGmUserInfo(dataUser);
                } else if (Msg_DL_LoadResult.ErrorNoEnum.NotFound == ret.ErrorNo) {
                    LogSys.Log(ServerLogType.INFO, ConsoleColor.Yellow, "DataCache Load NotFound: Msg:{0}, Key:{1}", "GmUser", primaryKey);
                } else {
                    LogSys.Log(ServerLogType.ERROR, "DataCache Load Failed: Msg:{0}, Key:{1}, ERROR:{2}", "GmUser", primaryKey, ret.ErrorInfo);
                }
                resultMsg.m_ProtoData = protoData;
                JsonGmMessageDispatcher.SendNodeMessage(nodeHandle, resultMsg);
            });
        }
        private GameFrameworkMessage.GmUserInfo CreateGmUserInfo(TableUserInfo user)
        {
            if (null == user)
                return null;
            GameFrameworkMessage.GmUserInfo gmUserInfo = new GameFrameworkMessage.GmUserInfo();
            gmUserInfo.m_Guid = (ulong)user.Guid;
            gmUserInfo.m_AccountId = user.AccountId;
            gmUserInfo.m_Nickname = user.Nickname;
            gmUserInfo.m_UserState = GameFrameworkMessage.GmStateEnum.Offline;
            //basic information
            GameFrameworkMessage.GmUserBasic gmUserBasic = new GameFrameworkMessage.GmUserBasic();
            gmUserBasic.m_HeroId = user.HeroId;
            gmUserBasic.m_Level = user.Level;
            gmUserBasic.m_Money = user.Money;
            gmUserBasic.m_Gold = user.Gold;
            gmUserBasic.m_CreateTime = user.CreateTime;
            gmUserBasic.m_LastLogoutTime = user.LastLogoutTime;
            gmUserInfo.m_UserBasic = gmUserBasic;
            return gmUserInfo;
        }

        private void LoadGlobalData()
        {
            InitGuidData();
            InitMailData();
            InitNicknameData();
            InitGlobalData();
        }
        private void InitGuidData()
        {
            if (m_GuidInitStatus == DataInitStatus.Unload) {
                if (UserServerConfig.DataStoreAvailable == true) {
                    m_GuidInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableGuid;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableGuid;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<GuidInfo> guidList = new List<GuidInfo>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableGuid dataGuid = _msg as TableGuid;
                                    if (null != dataGuid) {
                                        GuidInfo guidinfo = new GuidInfo();
                                        guidinfo.GuidType = dataGuid.GuidType;
                                        guidinfo.NextGuid = (long)dataGuid.GuidValue;
                                        guidList.Add(guidinfo);
                                    }
                                }
                            }
                            UserServer.Instance.GlobalProcessThread.InitGuidData(guidList);
                            m_GuidInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableGuid");
                        } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                            //Temporarily initialized to an empty list
                            List<GuidInfo> guidList = new List<GuidInfo>();
                            UserServer.Instance.GlobalProcessThread.InitGuidData(guidList);
                            m_GuidInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableGuid (empty)");
                        } else {
                            m_GuidInitStatus = DataInitStatus.Unload;
                            LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "TableGuid");
                        }
                    }
                   ));
                } else {
                    m_GuidInitStatus = DataInitStatus.Done;
                    LogSys.Log(ServerLogType.INFO, "init guid done!");
                }
            }
        }
        private void InitMailData()
        {
            if (m_MailInitStatus == DataInitStatus.Unload) {
                if (UserServerConfig.DataStoreAvailable == true) {
                    m_MailInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableMailInfo;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableMailInfo;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<TableMailInfoWrap> mailList = new List<TableMailInfoWrap>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableMailInfo dataMail = _msg as TableMailInfo;
                                    if (null != dataMail) {
                                        TableMailInfoWrap mailInfo = new TableMailInfoWrap();
                                        mailInfo.FromProto(dataMail);
                                        List<int> itemIds = Converter.ConvertNumericList<int>(dataMail.ItemIds);
                                        List<int> itemNums = Converter.ConvertNumericList<int>(dataMail.ItemNumbers);
                                        for (int i = 0; i < itemIds.Count && i < itemNums.Count; ++i) {
                                            var item = new MailItem();
                                            item.m_ItemId = itemIds[i];
                                            item.m_ItemNum = itemNums[i];
                                            mailInfo.m_Items.Add(item);
                                        }
                                        mailList.Add(mailInfo);
                                    }
                                }
                            }
                            UserServer.Instance.GlobalProcessThread.InitMailData(mailList);
                            m_MailInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableMailInfo");
                        } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                            //Temporarily initialized to an empty list
                            List<TableMailInfoWrap> mailList = new List<TableMailInfoWrap>();
                            UserServer.Instance.GlobalProcessThread.InitMailData(mailList);
                            m_MailInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableMailInfo (empty)");
                        } else {
                            m_MailInitStatus = DataInitStatus.Unload;
                            LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "TableMailInfo");
                        }
                    }
                   ));
                } else {
                    m_MailInitStatus = DataInitStatus.Done;
                    LogSys.Log(ServerLogType.INFO, "init mail done!");
                }
            }
        }
        private void InitNicknameData()
        {
            if (m_NicknameInitStatus == DataInitStatus.Unload) {
                if (m_DataStoreAvailable) {
                    m_NicknameInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableNicknameInfo;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableNicknameInfo;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<TableNicknameInfo> nickList = new List<TableNicknameInfo>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableNicknameInfo dataNickname = _msg as TableNicknameInfo;
                                    if (null != dataNickname) {
                                        nickList.Add(dataNickname);
                                    }
                                }
                            }
                            UserServer.Instance.UserProcessScheduler.InitNicknameData(nickList);
                            m_NicknameInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableNickname");
                        } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                            //Temporarily initialized to a fixed list
                            List<string> nicks = new List<string> { "整容的柠檬", "温柔的椰枣", "霸气的研究生", "爱斗嘴的贵妇", "顽皮的姐姐", "爱唱歌的女汉子", "小心的木星人", "爱画画的贵妇", "妖娆的小灰灰", "可爱的菠菜", "呆萌的小老鼠", "卖大米的包子", "搞怪的火星人", "可气的数据线", "妖娆的无花果", "非主流的闪电", "没头发的伙计", "大眼睛的王老五", "没头发的夜莺", "傻萌的大象", "高调的小豆豆", "甜蜜的小瓶子", "爱网购的男神", "坚强的海龟", "手残的石榴", "坚强的公主", "爱刷屏的二锅头", "搞笑的香蕉", "妩媚的大雁", "不穿鞋的雪姨", "抑郁的海鸥", "爱网购的螺丝钉", "没牙的燕子", "爱装酷的毛毛虫", "爱哭的风扇", "纯纯的棉花糖", "温柔的母老虎", "积极的巧克力", "红果果的话梅", "安静的糍粑", "大眼睛的橘子", "顽皮的土豆", "搞笑的数据线", "不加糖的大雁", "高贵的大象", "爱刷屏的彩虹糖", "可气的螺丝钉", "爱网购的吃货", "不加糖的桑葚", "吃不饱的熊孩子", "高贵的蜡笔", "尊贵的大象", "不吐皮的少爷", "开心的椰奶", "悲愤的梨子", "高调的大叔", "狂热的天鹅", "搞笑的雪莲", "温柔的变色龙", "装失忆的斑竹", "爱装酷的壁虎", "残酷的古琴", "爱画画的大象", "冷酷的榛子", "苦恼的鸵鸟", "搞怪的喜羊羊", "欢喜的柿子", "爱唱歌的粉丝", "不说话的画笔", "爱斗嘴的红毛丹", "安静的鲸鱼", "非主流的小灰灰", "爱装酷的红太狼", "打酱油的灰太狼", "装失忆的汉堡包", "抑郁的熊孩子", "欢喜的海鸥", "温柔的榛子", "悲伤的壁虎", "悲愤的猫咪", "脑残的桑葚", "欢喜的空心菜", "爱斗嘴的牛蛙", "孤独的布丁", "大眼睛的水星人", "没牙的河马", "高调的粉丝", "坚强的大叔", "残酷的鲸鱼", "火星来的大妈", "着急的菠菜", "可气的玻璃杯", "孤独的大叔", "打酱油的蝴蝶", "积极的蚂蚁", "尊贵的珍珠", "不吐皮的少女", "爱踢球的砖家", "清纯的雪姨", "威武的大雁", "爱哭的数据线", "吃骨头的王老五", "低调的蝴蝶", "会弹琴的糯米", "吹口哨的金桔", "悲愤的杨梅", "搞笑的美眉", "听话的圣斗士", "着急的空心菜", "呆萌的袋鼠", "爱笑的胡桃", "胖嘟嘟的小豆豆", "呆萌的柑橘", "听话的喵妹", "不死的桃子", "爱笑的红毛丹", "火星来的椰枣", "悲愤的鸵鸟", "大眼睛的花生", "爱笑的蹄子", "高兴的雪莲", "爱睡觉的MM豆", "抑郁的土星人", "丑萌的蹄子", "爱哭的少爷", "甜美的男神", "不加糖的博士生", "爱斗嘴的男神", "脑残的玻璃杯", "闪瞎眼的蝴蝶", "爱笑的母老虎", "古怪的槟榔", "搞笑的土豆", "厚脸皮的大象", "尊贵的小媳妇", "英俊的珍珠", "非主流的桑葚", "傻萌的羚羊", "吃不饱的蜗牛", "伤心的小灰灰", "大眼睛的金星人", "爱装酷的灰太狼", "卖大米的鹦鹉", "冷艳的夜莺", "酷到爆的贵公子", "妖娆的田鼠", "青春的猴子", "残酷的米老鼠", "高大上的蚂蚁", "爱斗嘴的梯子", "古怪的画眉", "羞涩的女老板", "捣蛋的龙虾", "高兴的大熊", "虚荣的香瓜", "高调的鸭梨", "温柔的龙珠", "不加糖的小清新", "积极的柠檬", "爱斗嘴的信天翁", "压抑的椰奶", "狂热的杨梅", "纯纯的少爷", "厚脸皮的大学生", "搞笑的女博士", "甜蜜的龙眼", "清纯的骑士", "坚强的扇贝", "不加糖的河马", "高兴的母老虎", "威武的土星人", "爱画画的金龟子", "爱哭的贝勒爷", "没牙的汪星人", "残暴的彩虹", "高贵的毛毛虫", "可怜的土豪", "轻蔑的八哥", "傻萌的橘子", "妩媚的大叔", "甜蜜的樱桃", "低调的蛇皮果", "英俊的猫咪", "呆萌的石榴", "残暴的西红柿", "不穿鞋的便当", "装失忆的橄榄", "积极的凤梨", "没头发的椰奶", "开心的土豪", "小心的白狐", "残酷的橘子", "打酱油的小蓝瓶", "冷酷的画笔", "正义的伙计", "闪瞎眼的北极星", "爱哭的表情帝", "搞怪的姐姐", "冷艳的薯条", "听话的苹果" };
                            List<TableNicknameInfo> nickList = new List<TableNicknameInfo>();
                            foreach (string nick in nicks) {
                                TableNicknameInfo info = new TableNicknameInfo();
                                info.Nickname = nick;
                                info.UserGuid = 0;
                                nickList.Add(info);
                            }
                            UserServer.Instance.UserProcessScheduler.InitNicknameData(nickList);
                            m_NicknameInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableNickname (empty)");
                        } else {
                            m_NicknameInitStatus = DataInitStatus.Unload;
                            LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "TableNickname");
                        }
                    }
                   ));
                } else {
                    List<TableNicknameInfo> nicknameList = new List<TableNicknameInfo>();
                    UserServer.Instance.UserProcessScheduler.InitNicknameData(nicknameList);
                    m_NicknameInitStatus = DataInitStatus.Done;
                    LogSys.Log(ServerLogType.INFO, "load NickName done!");
                }
            }
        }
        private void InitGlobalData()
        {
            if (m_GlobalInitStatus == DataInitStatus.Unload) {
                if (m_DataStoreAvailable) {
                    m_GlobalInitStatus = DataInitStatus.Loading;
                    Msg_LD_Load msg = new Msg_LD_Load();
                    msg.MsgId = (int)DataEnum.TableGlobalData;
                    GameFrameworkMessage.Msg_LD_SingleLoadRequest slr = new GameFrameworkMessage.Msg_LD_SingleLoadRequest();
                    slr.MsgId = (int)GameFrameworkData.DataEnum.TableGlobalData;
                    slr.LoadType = GameFrameworkMessage.Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll;
                    slr.Keys.Clear();
                    msg.LoadRequests.Add(slr);
                    RequestLoad(msg, ((ret) => {
                        if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                            List<TableGlobalData> datas = new List<TableGlobalData>();
                            foreach (var singlerow in ret.Results) {
                                object _msg;
                                if (DbDataSerializer.Decode(singlerow.Data, DataEnum2Type.Query(slr.MsgId), out _msg)) {
                                    TableGlobalData data = _msg as TableGlobalData;
                                    if (null != data) {
                                        datas.Add(data);
                                    }
                                }
                            }
                            GlobalData.Instance.Init(datas);
                            m_GlobalInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableGlobalData");
                        } else if (ret.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.NotFound) {
                            GlobalData.Instance.Clear();
                            m_GlobalInitStatus = DataInitStatus.Done;
                            LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "Load DataCache global data success. Table:TableGlobalData (empty)");
                        } else {
                            m_GlobalInitStatus = DataInitStatus.Unload;
                            LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Load DataCache global data failed. Table: {0}", "TableGlobalData");
                        }
                    }
                   ));
                } else {
                    List<TableNicknameInfo> nicknameList = new List<TableNicknameInfo>();
                    UserServer.Instance.UserProcessScheduler.InitNicknameData(nicknameList);
                    m_GlobalInitStatus = DataInitStatus.Done;
                    LogSys.Log(ServerLogType.INFO, "load GlobalData done!");
                }
            }
        }
        private bool CheckGlobalDataInitFinish()
        {
            if ((m_GuidInitStatus == DataInitStatus.Done) && (m_MailInitStatus == DataInitStatus.Done) && (m_NicknameInitStatus == DataInitStatus.Done) && (m_GlobalInitStatus == DataInitStatus.Done)) {
                LogSys.Log(ServerLogType.INFO, ConsoleColor.Green, "GlobalDataInitFinish ...");
                return true;
            } else {
                return false;
            }
        }

        private DataInitStatus m_GuidInitStatus = DataInitStatus.Unload;
        private DataInitStatus m_MailInitStatus = DataInitStatus.Unload;
        private DataInitStatus m_GlobalInitStatus = DataInitStatus.Unload;
        private DataInitStatus m_NicknameInitStatus = DataInitStatus.Unload;
    }
}
