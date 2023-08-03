using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace GameFramework
{
  internal class NodeMessageInfo
  {
    internal bool IsGmTool;
    internal uint Seq;
    internal ulong SourceHandle;
    internal ulong DestHandle;
    internal byte[] Data;
  }
}
