using System;
using System.IO;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace GameFramework.Network
{
    internal static class Serialize
    {
        internal static byte[] Encode(object msg, int id)
        {
#if DEBUG
            int rightId = RoomMessageDefine2Type.Query(msg.GetType());
            if (rightId > 0) {
                if (rightId != id) {
                    LogSystem.Error("msg {0} id {1} not match {2} !", msg.ToString(), id, rightId);
                    System.Diagnostics.Debug.Assert(false);
                }
            } else {
                LogSystem.Error("msg {0} id {1} can't find, maybe forget register !", msg.ToString(), id);
                System.Diagnostics.Debug.Assert(false);
            }
#endif
            DataStream.SetLength(0);
            if (id >= 128) {
                DataStream.WriteByte((byte)((id & 0x0000007f) | 0x00000080));
                DataStream.WriteByte((byte)(id >> 7));
            } else {
                DataStream.WriteByte((byte)id);
            }
            Serializer.Serialize(DataStream, msg);
            return DataStream.ToArray();
        }
        internal static object Decode(byte[] msgbuf, out int id)
        {
            int idLen = 1;
            byte first = msgbuf[0];
            if ((first & 0x80) == 0x80) {
                byte second = msgbuf[1];
                id = (int)(((int)first & 0x0000007f) | ((int)second << 7));
                idLen = 2;
            } else {
                id = (int)first;
            }
            if (id < 0) {
                LogSystem.Error("decode message error:id({0}) len({1}) error !!!", id, msgbuf.Length - idLen);
                return null;
            }

            Type t = RoomMessageDefine2Type.Query(id);
            if (null != t) {
                DataStream.SetLength(0);
                DataStream.Write(msgbuf, idLen, msgbuf.Length - idLen);
                DataStream.Position = 0;
                try {
                    object msg = Serializer.Deserialize(DataStream, null, t);
                    if (msg == null) {
                        LogSystem.Error("decode message error:can't find id {0} len({1}) !!!", id, msgbuf.Length - idLen);
                        return null;
                    }
                    //LogSystem.Info("decode message:id {0} len({1})[{2}]", id, msgbuf.Length - idLen, msg);
                    return msg;
                } catch (Exception ex) {
                    LogSystem.Error("decode message error:id({0}) len({1}) {2}\n{3}\nData:\n{4}", id, msgbuf.Length - idLen, ex.Message, ex.StackTrace, Helper.BinToHex(msgbuf, idLen));
                    throw ex;
                }
            }
            return null;
        }

        private static MemoryStream DataStream
        {
            get
            {
                if (null == s_Stream)
                    s_Stream = new MemoryStream(4096);
                return s_Stream;
            }
        }
        private static ProtobufSerializer Serializer
        {
            get
            {
                if (null == s_Serializer)
                    s_Serializer = new ProtobufSerializer();
                return s_Serializer;
            }
        }

        [ThreadStatic]
        private static MemoryStream s_Stream = null;
        [ThreadStatic]
        private static ProtobufSerializer s_Serializer = null;
    }
}
