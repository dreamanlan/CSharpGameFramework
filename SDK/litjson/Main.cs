using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace litjsontest
{
#if false
  public class Program
  {
    [STAThread]
    public static void Main()
    {
      for (int i = 0; i < 10000; ++i) {
        LitJson.JsonData data = new LitJson.JsonData();
        data["Main"] = 1232131;
        Console.WriteLine("{0}", data.ToJson());
      }
    }
  }
#endif
}
