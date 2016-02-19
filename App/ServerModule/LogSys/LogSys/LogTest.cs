using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogTest
{
  class Program
  {
    static void Main(string[] args)
    {
      LogSystem.Init("./config/logconfig.xml");
      LogSystem.Log(2, "{0}", "just for test!");
      LogSystem.Log(1, "{0}", "just for test!");
      LogSystem.Log(0, "{0}", "just for test!");
    }
  }
}
