using System;
using System.IO;
using System.Collections.Generic;

namespace ScriptableFramework.Network
{
    internal sealed class ProtoNetEncoding
    {
        internal byte[] Encode(object msg)
        {
            m_DataStream.SetLength(0);
            ProtoBuf.Serializer.Serialize(m_DataStream, msg);
            byte[] ret = new byte[m_DataStream.Length];
            m_DataStream.Position = 0;
            m_DataStream.Read(ret, 0, ret.Length);

            //LogSystem.Info("encode message:id {0} len({1})[{2}]", id, ret.Length - 2, jsonData.GetType().Name);
            return ret;
        }
        internal byte[] Encode(int id, object header, object proto)
        {
            int headerSize = 0;
            m_DataStream.SetLength(0);
            m_DataStream.WriteByte((byte)(id & 0xff));
            m_DataStream.WriteByte((byte)((id & 0xff00) >> 8));
            m_DataStream.WriteByte(0);
            m_DataStream.WriteByte(0);
            if (null != header) {
                ProtoBuf.Serializer.Serialize(m_DataStream, header);
                headerSize = (int)m_DataStream.Length - 4;
            }
            if (null != proto) {
                ProtoBuf.Serializer.Serialize(m_DataStream, proto);
            }
            m_DataStream.Position = 2;
            if (headerSize > 0) {
                m_DataStream.WriteByte((byte)(headerSize & 0xff));
                m_DataStream.WriteByte((byte)((headerSize & 0xff00) >> 8));
            }
            return m_DataStream.ToArray();
        }
        internal object Decode(Type t, byte[] msgbuf)
        {
            return Decode(t, msgbuf, 0);
        }
        internal object Decode(Type t, byte[] msgbuf, int offset)
        {
            return Decode(t, msgbuf, offset, msgbuf.Length - offset);
        }
        internal object Decode(Type t, byte[] msgbuf, int offset, int count)
        {
            if (offset < 0 || offset + count > msgbuf.Length)
                return null;
            m_DataStream.SetLength(0);
            m_DataStream.Write(msgbuf, offset, count);
            m_DataStream.Position = 0;
            try {
                object msg = ProtoBuf.Serializer.Deserialize(t, m_DataStream);
                if (msg == null) {
                    LogSystem.Error("decode message error:can't find {0} len({1}) !!!", t.Name, msgbuf.Length);
                    return null;
                }
                //LogSystem.Debug("decode message:id {0} len({1})[{2}]", id, msgbuf.Length - 2, jsonData.GetType().Name);
                return msg;
            }
            catch (Exception ex) {
                LogSystem.Error("decode message error:{0} len({1}) {2}\n{3}\nData:\n{4}", t.Name, msgbuf.Length, ex.Message, ex.StackTrace, Helper.BinToHex(msgbuf));
                throw;
            }
        }

        private MemoryStream m_DataStream = new MemoryStream(4096);
    }
}
