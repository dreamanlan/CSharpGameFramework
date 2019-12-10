using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Unity.MemoryProfilerForExtension.Editor.Legacy.LegacyFormats;

namespace Unity.MemoryProfilerForExtension.Editor.Legacy
{
    internal class LegacyReader
    {
        const string k_Memsnap = ".memsnap";
        const string k_Memsnap2 = ".memsnap2";
        const string k_Memsnap3 = ".memsnap3";

        public bool IsLegacyFileFormat(string path)
        {
            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case k_Memsnap:
                case k_Memsnap2:
                case k_Memsnap3:
                    return true;
                default:
                    return false;
            }
        }

        public LegacyPackedMemorySnapshot ReadFromFile(string path)
        {
            LegacyPackedMemorySnapshot snapshot = null;
            string json = null;

            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case k_Memsnap:
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    using (Stream stream = File.Open(path, FileMode.Open))
                    {
                        using (MemoryStream memStr = new MemoryStream())
                        {
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, (int)stream.Length);
                            memStr.Write(bytes, 0, (int)stream.Length);
                            memStr.Seek(0, SeekOrigin.Begin);
                            SurrogateSelector ss = new SurrogateSelector();

                            ss.AddSurrogate(typeof(UnityEditor.MemoryProfiler.PackedMemorySnapshot),
                                new StreamingContext(StreamingContextStates.All),
                                new LegacyFormats.Serialization.LegacyPackedMemorySnapshotSerializationSurrogate(true));
                            binaryFormatter.SurrogateSelector = ss;
                            object obj = null;
                            try
                            {
                                obj = binaryFormatter.Deserialize(memStr);
                            }
                            catch (SerializationException)
                            {
                                memStr.Seek(0, SeekOrigin.Begin);
                                ss = new SurrogateSelector();

                                ss.AddSurrogate(typeof(UnityEditor.MemoryProfiler.PackedMemorySnapshot),
                                    new StreamingContext(StreamingContextStates.All),
                                    new LegacyFormats.Serialization.LegacyPackedMemorySnapshotSerializationSurrogate(false));

                                binaryFormatter.SurrogateSelector = ss;
                                obj = binaryFormatter.Deserialize(memStr);
                            }
                            snapshot = obj as LegacyPackedMemorySnapshot;
                        }
                    }
                    break;
                case k_Memsnap2:
                    json = File.ReadAllText(path);
                    //fix binary compatibility for GCHandles
                    json = JsonUtil.JsonFindAndReplace(json, JsonFormatTokenChanges.kGcHandles.OldField, JsonFormatTokenChanges.kGcHandles.NewField);

                    snapshot = UnityEngine.JsonUtility.FromJson<LegacyPackedMemorySnapshot>(json);
                    break;
                case k_Memsnap3:
                    json = File.ReadAllText(path);
                    //fix binary compatibility for GCHandles
                    json = JsonUtil.JsonFindAndReplace(json, JsonFormatTokenChanges.kGcHandles.OldField, JsonFormatTokenChanges.kGcHandles.NewField);

                    JsonNetConverter converter = new JsonNetConverter();
                    json = converter.Convert(json);
                    snapshot = UnityEngine.JsonUtility.FromJson<LegacyPackedMemorySnapshot>(json);
                    break;
                default:
                    throw new System.Exception("Not a supported file format, provided extension was: " + extension);
            }

            return snapshot;
        }
    }
}
