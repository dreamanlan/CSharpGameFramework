using System;
using System.Collections.Generic;
using CSharpCenterClient;

namespace Messenger
{
  /// <summary>
  /// 消息channel, 即表示了某一类dcore的消息类型的消息
  /// </summary>
  /// <remarks>
  /// 注意：这个类实例需要支持在多个线程调用，除初始化外，所有方法都应该是多线程安全的！！！
  /// </remarks>
  public sealed class PBChannel
  {
    public delegate int MsgIdQuery(Type t);
    public delegate Type MsgTypeQuery(int id);
    public delegate void DefaultMessageHandler(object msg, PBChannel channel, int src,uint session);

    /// <summary>
    /// 内部初始化channel, 比较重要的是(id<->type)的相互查询函数
    /// </summary>
    /// <param name="msgid_query">id->type查询函数</param>
    /// <param name="msgtype_query">type->id查询函数</param>
    public PBChannel(MsgIdQuery msgid_query, MsgTypeQuery msgtype_query)
    {
      world_id_ = 0;
      msgid_query_ = msgid_query;
      msgtype_query_ = msgtype_query;
    }

    public void OnUpdateNameHandle(bool addOrUpdate, string name, int handle)
    {
      if (name == default_service_name_) {
        if (addOrUpdate)
          default_service_handle_ = handle;
        else
          default_service_handle_ = 0;
      }
    }
    /// <summary>
    /// 这个特性的名字代表的涵义比较特别，指定worldid表明此Channel使用CenterHubApi进行通讯，设为0表明使用CenterClientApi进行通讯。
    /// </summary>
    public int WorldId
    {
      get { return world_id_; }
      set { world_id_ = value; }
    }
    /// <summary>
    /// 指定缺省目标名限定了此Channel只与指定目标通讯，如果想要同时与多个不同目标通讯，不用指定目标名。
    /// </summary>
    public string DefaultServiceName
    {
      get { return default_service_name_; }
      set { default_service_name_ = value; }
    }
    public int DefaultServiceHandle
    {
      get
      {
        if (default_service_handle_ == 0 && !string.IsNullOrEmpty(default_service_name_)) {
          if (world_id_ == 0) {
            default_service_handle_ = CenterClientApi.TargetHandle(default_service_name_);
          } else {
            default_service_handle_ = CenterHubApi.TargetHandle(world_id_, default_service_name_);
          }
        }
        return default_service_handle_;
      }
    }

    /// <summary>
    /// 注册对于类型是MsgType的消息的处理函数, 例如:
    /// 对于MsgA
    /// c.Register<MsgA>(Handler);
    /// Handler的定义是
    /// void Handler(MsgA msg, PBChannel channel, int src,uint session) { ... }
    /// </summary>
    /// <typeparam name="MsgType">protobuf的消息类型</typeparam>
    /// <param name="f">消息处理函数</param>
    public void Register<MsgType>(PBHandler<MsgType>.F f)
    {
      int id = msgid_query_(typeof(MsgType));
      if (id > 0)
        handlers_[id] = new PBHandler<MsgType>(f);
    }

    /// <summary>
    /// 默认的消息处理函数, 凡是没有注册处理函数的消息, 都会调用到这个函数
    /// </summary>
    /// <param name="h">消息处理函数</param>
    public void RegisterDefaultHandler(DefaultMessageHandler h)
    {
      default_handler_ = h;
    }

    public void Dispatch(int from_handle, uint seq, byte[] data)
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
      int handle = DefaultServiceHandle;
      if (handle > 0) {
        ret = Send(handle, msg);
      }
      return ret;
    }

    public bool Send(string dest_name, object msg)
    {
      bool ret = false;
      if (null != dest_name) {
        if (world_id_ == 0) {
          int handle = CenterClientApi.TargetHandle(dest_name);
          if (handle != 0) {
            ret = Send(handle, msg);
          }
        } else {
          int handle = CenterHubApi.TargetHandle(world_id_, dest_name);
          if (handle != 0) {
            ret = Send(handle, msg);
          }
        }
      }
      return ret;
    }

    public bool Send(int dest_handle, object msg)
    {
      byte[] data;
      bool ret = Build(msg, out data);
      if (ret) {
        if (null != data) {
          if (world_id_ == 0) {
            ret = CenterClientApi.SendByHandle(dest_handle, data, data.Length);
          } else {
            ret = CenterHubApi.SendByHandle(world_id_, dest_handle, data, data.Length);
          }
        } else {
          ret = false;
        }
      }
      
      return ret;
    }

    public bool Forward(byte[] data)
    {
      bool ret = false;
      string name = DefaultServiceName;
      if (!string.IsNullOrEmpty(name)) {
        ret = Forward(name, data);
      }
      return ret;
    }

    public bool Forward(string dest_name, byte[] data)
    {
      bool ret = false;
      if (null != data) {
        if (world_id_ == 0) {
          ret = CenterClientApi.SendByName(dest_name, data, data.Length);
        } else {
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
    private int default_service_handle_;
    private DefaultMessageHandler default_handler_;
    private Dictionary<int, IPBHandler> handlers_ = new Dictionary<int, IPBHandler>();
  }
}