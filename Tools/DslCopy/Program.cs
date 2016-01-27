using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dsl;

namespace DslCopy
{
  class Program
  {
    static void Main(string[] args)
    {
      int defaultCodePage = Encoding.Default.CodePage;
      Console.WriteLine("defaultCodePage:{0}", defaultCodePage);
      if (args.Length != 2 && args.Length != 3) {
        Console.WriteLine("[Usage:] dslcopy source_dir target_dir");
        Console.WriteLine(" or");
        Console.WriteLine(" dslcopy source_dir target_dir isserver");
      } else {
        string sourceDir = args[0];
        string targetDir = args[1];
        bool isServer = false;
        if (args.Length == 3) {
          isServer = (0 == string.Compare(args[2], "isserver", true));
        }
        try {
          CopyFolder(sourceDir, targetDir, isServer);
          Console.WriteLine("dsl files copy finished");
          EncodeDsl(targetDir);
          Console.WriteLine("dsl encode finished");
        } catch (Exception ex) {
          Console.WriteLine(ex);
        }
      }
    }

    private static void CopyFolder(string from, string to, bool isServer)
    {
      if (!Directory.Exists(to))
        Directory.CreateDirectory(to);
      string clientPath = Path.Combine(from, "Client");
      string serverPath = Path.Combine(from, "Server");
      // 子文件夹
      foreach (string sub in Directory.GetDirectories(from)) {
        if (isServer) {
          if (0 == string.Compare(sub, clientPath, true))
            continue;
        } else {
          if (0 == string.Compare(sub, serverPath, true))
            continue;
        }
        CopyFolder(sub, Path.Combine(to, Path.GetFileName(sub)));
      }
      // 文件
      foreach (string file in Directory.GetFiles(from))
        File.Copy(file, Path.Combine(to, Path.GetFileName(file)), true);
    }

    private static void CopyFolder(string from, string to)
    {
      if (!Directory.Exists(to))
        Directory.CreateDirectory(to);
      // 子文件夹
      foreach (string sub in Directory.GetDirectories(from)) {
        CopyFolder(sub, Path.Combine(to, Path.GetFileName(sub)));
      }
      // 文件
      foreach (string file in Directory.GetFiles(from))
        File.Copy(file, Path.Combine(to, Path.GetFileName(file)), true);
    }

    private static void EncodeDsl(string dir)
    {
#if DEBUG
      List<string> destfiles = new List<string>();
      foreach (string filter in dslfilters.Split(',')) {
        foreach (var eachfileinfo in new DirectoryInfo(dir).GetFiles(filter, SearchOption.AllDirectories)) {
          destfiles.Add(eachfileinfo.FullName);
        }
      }
      foreach (string destfile in destfiles) {
        string code = File.ReadAllText(destfile);
        DslFile file = new DslFile();
        code = file.GenerateBinaryCode(code, GameFramework.GlobalVariables.Instance.EncodeTable, (string msg) => GameFramework.LogSystem.Warn("{0}", msg));
        if (string.IsNullOrEmpty(code)) {
          Console.WriteLine("encode dsl:{0} failed.", destfile);
        } else {
          try {
            File.WriteAllText(destfile, code);
          } catch (Exception ex) {
            Console.WriteLine("EncodeDsl {0} exception:{1}\n{2}", destfile, ex.Message, ex.StackTrace);
          }
        }
      }
#endif
    }

    private const string dslfilters = "*.dsl";
  }
}
