using System;

namespace Messenger
{
  internal interface IPBHandler
  {
    void Execute(object msg, PBChannel channel, ulong src, uint seq);
  }

  public sealed class PBHandler<MsgType> : IPBHandler
  {
    public delegate void F(MsgType msg, PBChannel channel, ulong src, uint seq);

    public PBHandler(F f)
    {
      f_ = f;
    }

    public void Execute(object msg, PBChannel channel, ulong src, uint seq)
    {
      if (null != f_)
        f_((MsgType)msg, channel, src, seq);
    }

    private F f_;
  }
}