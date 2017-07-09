using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace RichTextParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int count = 100;
            Console.WriteLine("[usage]RichTextParser textfile count");
            Console.WriteLine();
            Console.WriteLine("Now run {0} parses with preset text ...", count);
            string text = c_TestText;
            if (args.Length > 0) {
                string file = args[0];
                if (File.Exists(file)) {
                    text = File.ReadAllText(file);
                } else {
                    Console.WriteLine("Can't read file '{0}'", file);
                    return;
                }
                if (args.Length > 1) {
                    try {
                        count = int.Parse(args[1]);
                    } catch {
                        Console.WriteLine("arg '{0}' error", args[1]);
                        return;
                    }
                }
            }
            var parser = new Parser();
            Stopwatch w = new Stopwatch();
            w.Start();
            for (int i = 0; i < count; ++i) {
                var result = parser.Parse(text);
            }
            double t = w.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            Console.WriteLine("elapsed {0}ms for {1} parses, {2}ms/parse", t, count, t / count);
        }

        #region test string
        internal const string c_TestText = @"清风徐来，水波不兴[这里无[开始]
[@时装1][!物品1][#装备1][$怪物3][^玩家5][~铸剑炉][`宠物][*红包][|副本][^某某某{pos:(123,455)}][-城镇{pos:(345,564)}][&活动{data:<345,564,4312,4312,4321>}][ \pos {x:123} {y:456} 某年某月][ @ %123%456 某年某月]水中[[]]花
[{ href : http://www.baidu.com }baidu]顶[ {size:30} Some 天山[ {color:yellow} 天山如] 天山]
[{href:http://www.baidu.com}baidu]层[{size:30}Some 天山[{color:yellow}天山如]天山]
[{href:http://www.baidu.com}baidu]文[{size:30}Some 天山[{color:yellow}天山如]天山]
[{href:http://www.baidu.com}baidu]字[{size:30}Some 天山[{color:yellow}天山如]天山]
[{href:http://www.baidu.com}baidu]设[{size:30}Some 天山[{color:yellow}天山如]天山]
[{href:http://www.baidu.com}baidu]定[{size:30}Some 天山[{color:yellow}天山如]天山]fdafds]]";
        #endregion
    }
}
