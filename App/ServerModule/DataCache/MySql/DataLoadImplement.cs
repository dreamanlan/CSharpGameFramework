using System;
using System.Data;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataCache;
using MySql.Data.MySqlClient;
using System.Data.Common;
using GameFrameworkMessage;
using GameFrameworkData;

internal static class DataLoadImplement
{
    internal static void Load(Msg_LD_Load msg, MyAction<Msg_DL_LoadResult> callback)
    {
        Msg_DL_LoadResult ret = new Msg_DL_LoadResult();
        ret.MsgId = msg.MsgId;
        ret.PrimaryKeys.AddRange(msg.PrimaryKeys);
        ret.SerialNo = msg.SerialNo; ;
        ret.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.Success;
        if (DataCacheConfig.IsPersistent) {
            try {
                for (int i = 0; i < msg.LoadRequests.Count; ++i) {
                    Msg_LD_SingleLoadRequest req = msg.LoadRequests[i];
                    switch (req.LoadType) {
                        case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll: {
                                int start = req.Start;
                                int count = req.Count;
                                List<GeneralRecordData> datas = DataDML.LoadAll(req.MsgId, start, count);
                                foreach (GeneralRecordData data in datas) {
                                    Msg_DL_SingleRowResult result = new Msg_DL_SingleRowResult();
                                    result.MsgId = req.MsgId;
                                    result.PrimaryKeys.AddRange(data.PrimaryKeys);
                                    result.ForeignKeys.AddRange(data.ForeignKeys);
                                    result.DataVersion = data.DataVersion;
                                    result.Data = data.Data;
                                    ret.Results.Add(result);
                                }
                            }
                            break;
                        case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle: {
                                GeneralRecordData data = DataDML.LoadSingle(req.MsgId, req.Keys);
                                Msg_DL_SingleRowResult result = new Msg_DL_SingleRowResult();
                                result.MsgId = req.MsgId;

                                if (data != null) {
                                    result.PrimaryKeys.AddRange(data.PrimaryKeys);
                                    result.ForeignKeys.AddRange(data.ForeignKeys);
                                    result.DataVersion = data.DataVersion;
                                    result.Data = data.Data;
                                } else {
                                    result.PrimaryKeys.AddRange(msg.PrimaryKeys);
                                    result.ForeignKeys.Clear();
                                    result.DataVersion = -1;
                                    result.Data = null;
                                    ret.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.NotFound;
                                }
                                ret.Results.Add(result);
                            }
                            break;
                        case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti: {
                                List<GeneralRecordData> dataList = DataDML.LoadMulti(req.MsgId, req.Keys);
                                foreach (GeneralRecordData data in dataList) {
                                    Msg_DL_SingleRowResult result = new Msg_DL_SingleRowResult();
                                    result.MsgId = req.MsgId;
                                    result.PrimaryKeys.AddRange(data.PrimaryKeys);
                                    result.ForeignKeys.AddRange(data.ForeignKeys);
                                    result.DataVersion = data.DataVersion;
                                    result.Data = data.Data;
                                    ret.Results.Add(result);
                                }
                            }
                            break;
                    }
                }
            } catch (Exception ex) {
                ret.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.PostError;
                ret.ErrorInfo = ex.Message;
                LogSys.Log(ServerLogType.ERROR, ConsoleColor.Red, "Load data from mysql ERROR. MsgId:{0}, Key:{1}\nErrorMessage:{2}\nStackTrace:{3}",
                              msg.MsgId, msg.PrimaryKeys, ex.Message, ex.StackTrace);
            } finally {
                DataCacheSystem.Instance.QueueAction(callback, ret);
            }
        } else {
            ret.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.NotFound;
            ret.ErrorInfo = string.Empty;
            DataCacheSystem.Instance.QueueAction(callback, ret);
        }
    }
}