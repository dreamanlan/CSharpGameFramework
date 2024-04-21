using System;
using System.Collections.Generic;
using CSharpCenterClient;

namespace Messenger
{
    /// <summary>
    /// Message channel, which represents a message type of a certain type of core
    /// </summary>
    /// <remarks>
    /// Note: This class instance needs to support calling from multiple threads.
    /// Except for initialization, all methods should be multi-thread safe! ! !
    /// </remarks>
    public sealed class PBChannel
    {
        public delegate int MsgIdQuery(Type t);
        public delegate Type MsgTypeQuery(int id);
        public delegate void DefaultMessageHandler(object msg, PBChannel channel, ulong src, uint session);

        /// <summary>
        /// Internal initialization channel, the more important thing is the mutual query function of (id<->type)
        /// </summary>
        /// <param name="msgid_query">id->type query function</param>
        /// <param name="msgtype_query">type->id query function</param>
        public PBChannel(MsgIdQuery msgid_query, MsgTypeQuery msgtype_query)
        {
            world_id_ = 0;
            msgid_query_ = msgid_query;
            msgtype_query_ = msgtype_query;
        }

        public void OnUpdateNameHandle(bool addOrUpdate, string name, ulong handle)
        {
            if (name == default_service_name_)
            {
                if (addOrUpdate)
                    default_service_handle_ = handle;
                else
                    default_service_handle_ = 0;
            }
        }
        /// <summary>
        /// The name of this feature has a special meaning. Specifying worldid indicates that this Channel uses
        /// CenterHubApi for communication, and setting it to 0 indicates that CenterClientApi is used
        /// for communication.
        /// </summary>
        public int WorldId
        {
            get { return world_id_; }
            set { world_id_ = value; }
        }
        /// <summary>
        /// Specifying a default target name limits this Channel to only communicate with the
        /// specified target. If you want to communicate with multiple different targets at
        /// the same time, you do not need to specify a target name.
        /// </summary>
        public string DefaultServiceName
        {
            get { return default_service_name_; }
            set { default_service_name_ = value; }
        }
        public ulong DefaultServiceHandle
        {
            get
            {
                if (default_service_handle_ == 0 && !string.IsNullOrEmpty(default_service_name_))
                {
                    if (world_id_ == 0)
                    {
                        default_service_handle_ = CenterClientApi.TargetHandle(default_service_name_);
                    }
                    else
                    {
                        default_service_handle_ = CenterHubApi.TargetHandle(world_id_, default_service_name_);
                    }
                }
                return default_service_handle_;
            }
        }

        /// <summary>
        /// Register a processing function for messages of type MsgType, for example:
        /// for MsgA
        /// c.Register<MsgA>(Handler);
        /// Handler's definition is
        /// void Handler(MsgA msg, PBChannel channel, int src,uint session) { ... }
        /// </summary>
        /// <typeparam name="MsgType">protobuf message type</typeparam>
        /// <param name="f">message handler</param>
        public void Register<MsgType>(PBHandler<MsgType>.F f)
        {
            int id = msgid_query_(typeof(MsgType));
            if (id > 0)
                handlers_[id] = new PBHandler<MsgType>(f);
        }

        /// <summary>
        /// The default message processing function. This function will be called for any message that does not have a registered processing function.
        /// </summary>
        /// <param name="h">message handler</param>
        public void RegisterDefaultHandler(DefaultMessageHandler h)
        {
            default_handler_ = h;
        }

        public void Dispatch(ulong from_handle, uint seq, byte[] data)
        {
            int id;
            object msg;
            if (PBCodec.Decode(data, msgtype_query_, out msg, out id))
            {
                IPBHandler h;
                if (handlers_.TryGetValue(id, out h))
                    h.Execute(msg, this, from_handle, seq);
                else if (null != default_handler_)
                    default_handler_(msg, this, from_handle, seq);
            }
        }

        public bool Send(object msg)
        {
            bool ret = false;
            ulong handle = DefaultServiceHandle;
            if (handle > 0)
            {
                ret = Send(handle, msg);
            }
            return ret;
        }

        public bool Send(string dest_name, object msg)
        {
            bool ret = false;
            if (null != dest_name)
            {
                if (world_id_ == 0)
                {
                    ulong handle = CenterClientApi.TargetHandle(dest_name);
                    if (handle != 0)
                    {
                        ret = Send(handle, msg);
                    }
                }
                else
                {
                    ulong handle = CenterHubApi.TargetHandle(world_id_, dest_name);
                    if (handle != 0)
                    {
                        ret = Send(handle, msg);
                    }
                }
            }
            return ret;
        }

        public bool Send(ulong dest_handle, object msg)
        {
            byte[] data;
            bool ret = Build(msg, out data);
            if (ret)
            {
                if (null != data)
                {
                    if (world_id_ == 0)
                    {
                        ret = CenterClientApi.SendByHandle(dest_handle, data, data.Length);
                    }
                    else
                    {
                        ret = CenterHubApi.SendByHandle(world_id_, dest_handle, data, data.Length);
                    }
                }
                else
                {
                    ret = false;
                }
            }

            return ret;
        }

        public bool Forward(byte[] data)
        {
            bool ret = false;
            string name = DefaultServiceName;
            if (!string.IsNullOrEmpty(name))
            {
                ret = Forward(name, data);
            }
            return ret;
        }

        public bool Forward(string dest_name, byte[] data)
        {
            bool ret = false;
            if (null != data)
            {
                if (world_id_ == 0)
                {
                    ret = CenterClientApi.SendByName(dest_name, data, data.Length);
                }
                else
                {
                    ret = CenterHubApi.SendByName(world_id_, dest_name, data, data.Length);
                }
            }
            return ret;
        }

        public byte[] Encode(object msg)
        {
            int id = msgid_query_(msg.GetType());
            return (id > 0) ?
              PBCodec.Encode(id, msg) :
              null;
        }

        public object Decode(byte[] data)
        {
            int id;
            object msg;
            PBCodec.Decode(data, msgtype_query_, out msg, out id);
            return msg;
        }

        private bool Build(object msg, out byte[] data)
        {
            int id = msgid_query_(msg.GetType());
            if (id > 0)
            {
                data = PBCodec.Encode(id, msg);
                return null != data;
            }
            else
            {
                data = null;
                return false;
            }
        }

        private int world_id_;
        private MsgIdQuery msgid_query_;
        private MsgTypeQuery msgtype_query_;
        private string default_service_name_;
        private ulong default_service_handle_;
        private DefaultMessageHandler default_handler_;
        private Dictionary<int, IPBHandler> handlers_ = new Dictionary<int, IPBHandler>();
    }
}