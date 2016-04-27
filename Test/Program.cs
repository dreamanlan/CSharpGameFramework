#if DEBUG
#define GENERATE_CODE
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DemoCommon;

namespace Test
{
    class Program
    {
#if GENERATE_CODE
        static void Main(string[] args)
        {
            LogSystem.OnOutput = (Log_Type type, string msg) => {
                Console.WriteLine(msg);
            };

            HomePath.InitHomePath();
            TableReaderGenerator.TableReaderGenerator.Generate(true);
            //TableReaderGenerator.TableReaderGenerator.Generate(false);
        }
#else
        static void Main(string[] args)
        {
            LogSystem.OnOutput = (Log_Type type, string msg) => {
                Console.WriteLine(msg);
            };
            HomePath.InitHomePath();
            TableReaderGenerator.TableReaderGenerator.Generate(false);
            float avg1 = TestBinary();
            float avg2 = TestProtoBuf();

            Console.WriteLine("binary avg:{0}ms", avg1 / 1000.0f);
            Console.WriteLine("protobuf avg:{0}ms", avg2 / 1000.0f);
        }

        private static float TestBinary()
        {
            long t1 = GetTime();
            for (int ct = 0; ct < 100; ++ct) {
                LoadBinary();
            }
            long t2 = GetTime();
            return (t2 - t1) * 1.0f;
        }

        private static float TestProtoBuf()
        {
            long t1 = GetTime();
            for (int ct = 0; ct < 100; ++ct) {
                LoadProtoBuf();
            }
            long t2 = GetTime();
            return (t2 - t1) * 1.0f;
        }

        private static void LoadBinary()
        {
            BinaryTableConfig.SkillProvider.Instance.Clear();
            BinaryTableConfig.SkillProvider.Instance.LoadForClient();
            LogSystem.Info("record num: {0}", BinaryTableConfig.SkillProvider.Instance.GetSkillCount());
        }

        private static void LoadProtoBuf()
        {
            long t1 = GetTime();
            List<TableConfig.Skill> list = LoadConfig<TableConfig.SkillList>(FilePathDefine_Client.C_ProtoSkill).items;
            long t2 = GetTime();
            long t3 = GetTime();
            ToDictionary<int, TableConfig.Skill>(list);
            long t4 = GetTime();
            LogSystem.Info("protobuf load {0} parse {1}, file {2}", t2 - t1, t4 - t3, FilePathDefine_Client.C_ProtoSkill);
            LogSystem.Info("record num: {0}", list.Count);
        }

        private static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(List<TValue> items)
        {
            Dictionary<TKey, TValue> rlt = new Dictionary<TKey, TValue>();
            for (int i = 0; i < items.Count; i++) {
                object obj = items[i];
                Type t = obj.GetType();
                object idObj = t.InvokeMember("id", System.Reflection.BindingFlags.GetProperty, null, obj, null);
                TKey id = (TKey)idObj;
                rlt[id] = items[i];
            }
            return rlt;
        }

        private static T LoadConfig<T>(string path)
        {
            System.IO.FileStream stream = System.IO.File.OpenRead(path);
            object obj = ParseConfig(stream, typeof(T));
            return (T)obj;
        }

        private static object ParseConfig(Stream stream, Type type)
        {
            return s_Serializer.Deserialize(stream, null, type);
        }

        private static long GetTime()
        {
            return TimeUtility.GetElapsedTimeUs();
        }

        private static TableConfigSerializer s_Serializer = new TableConfigSerializer();
#endif
    }
}
