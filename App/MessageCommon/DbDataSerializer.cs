using System;
using System.IO;
using System.Collections.Generic;
using ScriptableFramework;

namespace GameFrameworkData
{
    internal static class DbDataSerializer
    {
        internal static byte[] Encode(object msg)
        {
            if (null == msg)
                return null;
            DataStream.SetLength(0);
            ProtoBuf.Serializer.Serialize(DataStream, msg);
            return DataStream.ToArray();
        }
        internal static bool Decode(byte[] msgbuf, Type t, out object msg)
        {
            if (null != t) {
                DataStream.SetLength(0);
                DataStream.Write(msgbuf, 0, msgbuf.Length);
                DataStream.Position = 0;
                try {
                    msg = ProtoBuf.Serializer.Deserialize(t, DataStream);
                    if (msg == null) {
                        return false;
                    }
                    return true;
                } catch (Exception ex) {
                    LogSystem.Error("decode message error:{0}\n{1}\nData:\n{2}", ex.Message, ex.StackTrace, Helper.BinToHex(msgbuf));
                    msg = null;
                    return false;
                }
            } else {
                msg = null;
                return false;
            }
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

        [ThreadStatic]
        private static MemoryStream s_Stream = null;
    }
}
