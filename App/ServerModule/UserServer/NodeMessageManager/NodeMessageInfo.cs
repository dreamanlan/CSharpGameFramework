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
    internal int SourceHandle;
    internal int DestHandle;
    internal byte[] Data;
  }
}
