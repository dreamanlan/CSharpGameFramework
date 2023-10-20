using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Messenger
{
    public static class PBUtility
    {
        public static byte[] Encode(object msg)
        {
            DataStream.SetLength(0);
            ProtoBuf.Serializer.Serialize(DataStream, msg);
            return DataStream.ToArray();
        }

        public static bool Decode(byte[] data, Type t, out object msg)
        {
            bool ret = false;
            DataStream.SetLength(0);
            DataStream.Write(data, 0, data.Length);
            DataStream.Position = 0;
            try {
                msg = ProtoBuf.Serializer.Deserialize(t, DataStream);
                if (msg == null) {
                    ret = false;
                }
                else {
                    ret = true;
                }
            }
            catch {
                msg = null;
                throw;
            }
            return ret;
        }

        private static MemoryStream DataStream {
            get {
                if (null == s_Stream)
                    s_Stream = new MemoryStream(4096);
                return s_Stream;
            }
        }

        [ThreadStatic]
        private static MemoryStream s_Stream = null;
    }
}
