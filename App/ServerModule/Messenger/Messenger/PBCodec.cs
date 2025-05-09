﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;

namespace Messenger
{
    internal static class PBCodec
    {
        /// <summary>
        /// encode protobuf message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="msg">message object</param>
        /// <returns></returns>
        public static byte[] Encode(int id, object msg)
        {
            DataStream.SetLength(0);
            id = IPAddress.HostToNetworkOrder(id);
            DataStream.Write(BitConverter.GetBytes(id), 0, 4);
            ProtoBuf.Serializer.Serialize(DataStream, msg);
            return DataStream.ToArray();
        }

        /// <summary>
        /// decode protobuf message
        /// </summary>
        /// <param name="data">bytes data</param>
        /// <param name="t_query">message (id->type) query function</param>
        /// <param name="msg">message object for return</param>
        /// <param name="id">message id for return</param>
        /// <returns></returns>
        public static bool Decode(
            byte[] data, PBChannel.MsgTypeQuery t_query,
            out object msg, out int id)
        {
            bool ret = false;
            id = BitConverter.ToInt32(data, 0);
            id = IPAddress.NetworkToHostOrder(id);
            Type t = t_query(id);
            if (null != t) {
                DataStream.SetLength(0);
                DataStream.Write(data, 4, data.Length - 4);
                DataStream.Position = 0;
                try {
                    msg = ProtoBuf.Serializer.Deserialize(t, DataStream);
                    if (msg == null) {
                        ret = false;
                    }
                    else {
                        ret = true;
                    }
                } catch {
                    msg = null;
                    throw;
                }
            }
            else {
                msg = null;
            }
            return ret;
        }

        private static MemoryStream DataStream {
            get {
                if (null == s_Stream)
                    s_Stream = new MemoryStream(64 * 1024);
                return s_Stream;
            }
        }

        [ThreadStatic]
        private static MemoryStream s_Stream = null;
    }
}