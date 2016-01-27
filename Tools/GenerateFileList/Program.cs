using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GenerateFileList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> result = new List<string>();
            string path = @"Unity3d\Assets\StreamingAssets";
            string targetFile = path + "\\list.txt";

            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files) {
                if (!file.EndsWith("list.txt")) {
                    string s = file.Substring(path.Length + 1);
                    s = s.Replace("\\", "/");
                    result.Add(s);
                }
            }
            FileStream fs = new FileStream(targetFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("{0}", result.Count);
            foreach (string s in result) {
                sw.WriteLine(s);
            }
            sw.Close();
            fs.Close();

            Console.WriteLine("write file to " + targetFile);

            Console.WriteLine("press any key to exit...");
            Console.ReadKey();
        }
    }
}
