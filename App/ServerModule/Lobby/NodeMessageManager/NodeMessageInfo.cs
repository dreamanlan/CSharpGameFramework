using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Lobby
{
  internal class NodeMessageInfo
  {
    internal uint Seq;
    internal int SourceHandle;
    internal int DestHandle;
    internal byte[] Data;
  }
}
