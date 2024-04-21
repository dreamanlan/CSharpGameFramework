using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace GameFramework
{
    public sealed class BinaryTable
    {
        public const int c_Identity = 0x414e4942;//BINA
        public const int c_Version = 0x00000001;

        [StructLayout(LayoutKind.Auto, Pack = 1, Size = 32)]
        public struct FileHeader
        {
            public int m_Identity;
            public int m_Version;
            public int m_RecordNum;
            public int m_RecordSize;
            public int m_StringOffset;
            public int m_IntListOffset;
            public int m_FloatListOffset;
            public int m_StrListOffset;

            public FileHeader(int recordNum, int recordSize)
            {
                m_Identity = c_Identity;
                m_Version = c_Version;
                m_RecordNum = recordNum;
                m_RecordSize = recordSize;
                m_StringOffset = 0;
                m_IntListOffset = 0;
                m_FloatListOffset = 0;
                m_StrListOffset = 0;
            }
        }

        public List<byte[]> Records
        {
            get { return m_Records; }
        }
        public List<string> StringList
        {
            get { return m_StringList; }
        }
        public List<int> IntLists
        {
            get { return m_IntLists; }
        }
        public List<float> FloatLists
        {
            get { return m_FloatLists; }
        }
        public List<int> StrLists
        {
            get { return m_StrLists; }
        }

        public void Reset()
        {
            m_Records.Clear();
            m_StringList.Clear();
            m_IntLists.Clear();
            m_FloatLists.Clear();
            m_StrLists.Clear();
            m_Strings = null;
            m_Ints = null;
            m_Floats = null;
            m_Strs = null;
        }
        public void Update()
        {
            m_Strings = m_StringList.ToArray();
            m_Ints = m_IntLists.ToArray();
            m_Floats = m_FloatLists.ToArray();
            m_Strs = m_StrLists.ToArray();
        }
        public int AddString(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return -1;
            }
            int ret = m_StringList.IndexOf(str);
            if (ret < 0) {
                m_StringList.Add(str);
                ret = m_StringList.Count - 1;
            }
            return ret;
        }
        public int AddIntList(int[] vals)
        {
            if (null == vals || vals.Length == 0)
                return -1;
            int ct = vals.Length;
            int start = m_IntLists.Count;
            m_IntLists.AddRange(vals);
            return (start << 8) + ct;
        }
        public int AddFloatList(float[] vals)
        {
            if (null == vals || vals.Length == 0)
                return -1;
            int ct = vals.Length;
            int start = m_FloatLists.Count;
            m_FloatLists.AddRange(vals);
            return (start << 8) + ct;
        }
        public int AddStrList(string[] vals)
        {
            if (null == vals || vals.Length == 0)
                return -1;
            int ct = vals.Length;
            int start = m_StrLists.Count;
            int[] indexes = new int[ct];
            for (int i = 0; i < ct; ++i) {
                indexes[i] = AddString(vals[i]);
            }
            m_StrLists.AddRange(indexes);
            return (start << 8) + ct;
        }
        public string GetString(int val)
        {
            if (val >= 0 && val < m_StringList.Count) {
                return m_StringList[val];
            } else {
                return "";
            }
        }
        public int[] GetIntList(int val)
        {
            if (val < 0 || null == m_Ints || 0 == m_Ints.Length) {
                return null;
            }
            long ct = (val & 0x000000ff);
            long start = ((val & 0xffffff00) >> 8);
            int[] vals = new int[ct];
            Array.Copy(m_Ints, start, vals, 0, ct);
            return vals;
        }
        public float[] GetFloatList(int val)
        {
            if (val < 0 || null == m_Floats || 0 == m_Floats.Length) {
                return null;
            }
            long ct = (val & 0x000000ff);
            long start = ((val & 0xffffff00) >> 8);
            float[] vals = new float[ct];
            Array.Copy(m_Floats, start, vals, 0, ct);
            return vals;
        }
        public string[] GetStrList(int val)
        {
            if (val < 0 || null == m_Strs || 0 == m_Strs.Length) {
                return null;
            }
            long ct = (val & 0x000000ff);
            long start = ((val & 0xffffff00) >> 8);
            int[] indexes = new int[ct];
            string[] vals = new string[ct];
            Array.Copy(m_Strs, start, indexes, 0, ct);
            for (int i = 0; i < ct; ++i) {
                vals[i] = GetString(indexes[i]);
            }
            return vals;
        }

        public void Load(string file)
        {
            try {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    FileHeader header = ReadFileHeader(fs);
                    if (header.m_Identity == c_Identity && header.m_Version == c_Version) {
                        Reset();

                        for (int i = 0; i < header.m_RecordNum && fs.Position < fs.Length; ++i) {
                            byte[] bytes = new byte[header.m_RecordSize];
                            fs.Read(bytes, 0, header.m_RecordSize);
                            m_Records.Add(bytes);
                        }

                        if (m_Records.Count > 0) {
                            fs.Position = header.m_StringOffset;
                            m_StringList = new List<string>(ReadStrArray(fs));

                            fs.Position = header.m_IntListOffset;
                            m_IntLists = new List<int>(ReadIntArray(fs));

                            fs.Position = header.m_FloatListOffset;
                            m_FloatLists = new List<float>(ReadFloatArray(fs));

                            fs.Position = header.m_StrListOffset;
                            m_StrLists = new List<int>(ReadIntArray(fs));

                            Update();

                            bool haveError = false;
                            int recordSize = m_Records[0].Length;
                            int recordNum = m_Records.Count;
                            for (int i = 1; i < recordNum; ++i) {
                                if (m_Records[i].Length != recordSize) {
                                    LogSystem.Error("Record Size not equal, {0}!={1}({2})", recordSize, m_Records[i].Length, i);
                                    haveError = true;
                                }
                            }
                            if (haveError) {
                                Reset();
                            }
                        }
                    } else {
                        LogSystem.Error("Unknown binary file tag {0} or error version {1}", header.m_Identity, header.m_Version);
                    }
                    fs.Close();
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        public void Save(string file)
        {
            try {
                if (m_Records.Count > 0) {
                    int recordSize = m_Records[0].Length;
                    int recordNum = m_Records.Count;
                    for (int i = 1; i < recordNum; ++i) {
                        if (m_Records[i].Length != recordSize) {
                            LogSystem.Error("Record Size not equal, {0}!={1}({2})", recordSize, m_Records[i].Length, i);
                            return;
                        }
                    }
                    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        FileHeader header = new FileHeader(recordNum, recordSize);
                        WriteFileHeader(fs, header);

                        for (int i = 0; i < recordNum; ++i) {
                            fs.Write(m_Records[i], 0, recordSize);
                        }

                        header.m_StringOffset = (int)fs.Position;
                        WriteStrArray(fs, m_StringList.ToArray());

                        header.m_IntListOffset = (int)fs.Position;
                        WriteIntArray(fs, m_IntLists.ToArray());

                        header.m_FloatListOffset = (int)fs.Position;
                        WriteFloatArray(fs, m_FloatLists.ToArray());

                        header.m_StrListOffset = (int)fs.Position;
                        WriteIntArray(fs, m_StrLists.ToArray());

                        //Rewrite file header
                        fs.Position = 0;
                        WriteFileHeader(fs, header);

                        fs.Close();
                    }
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private List<byte[]> m_Records = new List<byte[]>();
        private List<string> m_StringList = new List<string>();
        private List<int> m_IntLists = new List<int>();
        private List<float> m_FloatLists = new List<float>();
        private List<int> m_StrLists = new List<int>();

        private string[] m_Strings = null;
        private int[] m_Ints = null;
        private float[] m_Floats = null;
        private int[] m_Strs = null;

        /*****************************************************************************************************
        //----------------------------------------------------------------------------------------------------
        *****************************************************************************************************/
        public static bool IsValid(string file)
        {
            bool ret = false;
            try {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    FileHeader header = ReadFileHeader(fs);
                    if (header.m_Identity == c_Identity && header.m_Version == c_Version) {
                        ret = true;
                    }
                    fs.Close();
                }
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return ret;
        }

        private static unsafe FileHeader ReadFileHeader(FileStream fs)
        {
            FileHeader header;
            byte[] bytes = new byte[sizeof(FileHeader)];
            fs.Read(bytes, 0, sizeof(FileHeader));
            fixed (byte* p = bytes) {
                header = *(FileHeader*)p;
            }
            return header;
        }
        private static unsafe void WriteFileHeader(FileStream fs, FileHeader header)
        {
            byte[] data = new byte[sizeof(FileHeader)];
            fixed (byte* pData = data) {
                FileHeader* temp = (FileHeader*)pData;
                *temp = header;
            }
            fs.Write(data, 0, sizeof(FileHeader));
        }
        private static int ReadShort(FileStream fs)
        {
            int i1 = fs.ReadByte();
            int i2 = fs.ReadByte();
            return (i1 + (i2 << 8));
        }
        private static void WriteShort(FileStream fs, short val)
        {
            fs.WriteByte((byte)(val & 0x00ff));
            fs.WriteByte((byte)((val & 0x0ff00) >> 8));
        }
        private static int ReadInt(FileStream fs)
        {
            int i1 = fs.ReadByte();
            int i2 = fs.ReadByte();
            int i3 = fs.ReadByte();
            int i4 = fs.ReadByte();
            return i1 + (i2 << 8) + (i3 << 16) + (i4 << 24);
        }
        private static void WriteInt(FileStream fs, int val)
        {
            fs.WriteByte((byte)(val & 0x00ff));
            fs.WriteByte((byte)((val & 0x0ff00) >> 8));
            fs.WriteByte((byte)((val & 0x0ff0000) >> 16));
            fs.WriteByte((byte)((val & 0x0ff000000) >> 24));
        }
        private static string ReadString(FileStream fs)
        {
            int len = ReadShort(fs);
            byte[] bytes = new byte[len];
            fs.Read(bytes, 0, len);
            return Encoding.UTF8.GetString(bytes);
        }
        private static void WriteString(FileStream fs, string val)
        {
            int len = Encoding.UTF8.GetByteCount(val);
            WriteShort(fs, (short)len);
            byte[] bytes = Encoding.UTF8.GetBytes(val);
            fs.Write(bytes, 0, bytes.Length);
        }
        private static unsafe int[] ReadIntArray(FileStream fs)
        {
            int count = ReadInt(fs);
            byte[] data = new byte[count * sizeof(int)];
            fs.Read(data, 0, count * sizeof(int));
            int[] vals = new int[count];
            fixed (byte* pData = data) {
                int* ptr = (int*)pData;
                for (int i = 0; i < count; ++i) {
                    vals[i] = *ptr;
                    ptr++;
                }
            }
            return vals;
        }
        private static unsafe void WriteIntArray(FileStream fs, int[] vals)
        {
            int count = vals.Length;
            WriteInt(fs, count);
            byte[] data = new byte[count * sizeof(int)];
            fixed (byte* pData = data) {
                int* ptr = (int*)pData;
                for (int i = 0; i < count; ++i) {
                    *ptr = vals[i];
                    ptr++;
                }
            }
            fs.Write(data, 0, count * sizeof(int));
        }
        private static unsafe float[] ReadFloatArray(FileStream fs)
        {
            int count = ReadInt(fs);
            byte[] data = new byte[count * sizeof(float)];
            fs.Read(data, 0, count * sizeof(float));
            float[] vals = new float[count];
            fixed (byte* pData = data) {
                float* ptr = (float*)pData;
                for (int i = 0; i < count; ++i) {
                    vals[i] = *(float*)ptr;
                    ptr++;
                }
            }
            return vals;
        }
        private static unsafe void WriteFloatArray(FileStream fs, float[] vals)
        {
            int count = vals.Length;
            WriteInt(fs, count);
            byte[] data = new byte[count * sizeof(float)];
            fixed (byte* pData = data) {
                float* ptr = (float*)pData;
                for (int i = 0; i < count; ++i) {
                    *ptr = vals[i];
                    ptr++;
                }
            }
            fs.Write(data, 0, count * sizeof(float));
        }
        private static unsafe string[] ReadStrArray(FileStream fs)
        {
            int count = ReadInt(fs);
            string[] vals = new string[count];
            for (int i = 0; i < count; ++i) {
                vals[i] = ReadString(fs);
            }
            return vals;
        }
        private static unsafe void WriteStrArray(FileStream fs, string[] vals)
        {
            int count = vals.Length;
            WriteInt(fs, count);
            for (int i = 0; i < count; ++i) {
                WriteString(fs, vals[i]);
            }
        }
    }

    public static class Txt2Binary
    {
        public static bool Convert(string file, string outFile, Encoding encoding)
        {
            try {
                string[] lines = File.ReadAllLines(file, encoding);
                if (lines.Length >= 2) {
                    string[] types = lines[0].Split('\t');
                    string[] fields = lines[1].Split('\t');

                    string dirName = Path.GetDirectoryName(file);
                    string fileName = Path.GetFileName(file);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    int fieldCount = 0;
                    int excelColumnCount = types.Length;
                    if (fields.Length != excelColumnCount) {
                        LogSystem.Error("[line:2] “{0}” field count != {1}", lines[1], excelColumnCount);
                        return false;
                    }
                    for (int ix = 0; ix < excelColumnCount; ++ix) {
                        if (string.IsNullOrEmpty(types[ix]) || string.IsNullOrEmpty(fields[ix]))
                            continue;
                        ++fieldCount;
                    }
                    BinaryTable table = new BinaryTable();
                    for (int rowIndex = 2; rowIndex < lines.Length; ++rowIndex) {
                        if (lines[rowIndex].StartsWith("#") || lines[rowIndex].StartsWith("//"))
                            continue;
                        int colIndex = 0;
                        string[] fieldValues = lines[rowIndex].Split('\t');
                        if (fieldValues.Length != excelColumnCount) {
                            LogSystem.Error("[line:{0}] “{1}” field count != {2}", rowIndex + 1, lines[rowIndex], excelColumnCount);
                            continue;
                        }
                        byte[] record = new byte[fieldCount * sizeof(int)];
                        table.Records.Add(record);
                        for (int ix = 0; ix < excelColumnCount; ++ix) {
                            if (string.IsNullOrEmpty(fields[ix]) || string.IsNullOrEmpty(types[ix]))
                                continue;
                            string type = types[ix].Trim();
                            string val = fieldValues[ix].Trim();
                            try {
                                if (0 == type.CompareTo("int") || 0 == type.CompareTo("int32") || 0 == type.CompareTo("long") || 0 == type.CompareTo("int64")) {
                                    int v = 0;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = int.Parse(val);
                                    }
                                    WriteIndex(record, colIndex, v);
                                } else if (0 == type.CompareTo("float")) {
                                    float v = 0;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = float.Parse(val);
                                    }
                                    WriteFloat(record, colIndex, v);
                                } else if (0 == type.CompareTo("bool")) {
                                    bool v = false;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = (val == "true" || val == "1");
                                    }
                                    WriteIndex(record, colIndex, v ? 1 : 0);
                                } else if (0 == type.CompareTo("string")) {
                                    int index = table.AddString(val);
                                    WriteIndex(record, colIndex, index);
                                } else if (0 == type.CompareTo("int[]") || 0 == type.CompareTo("int32[]") || 0 == type.CompareTo("long[]") || 0 == type.CompareTo("int64[]")) {
                                    int index = -1;
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        int[] vals = new int[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = int.Parse(v[i]);
                                        }
                                        index = table.AddIntList(vals);
                                    }
                                    WriteIndex(record, colIndex, index);
                                } else if (0 == type.CompareTo("float[]")) {
                                    int index = -1;
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        float[] vals = new float[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = float.Parse(v[i]);
                                        }
                                        index = table.AddFloatList(vals);
                                    }
                                    WriteIndex(record, colIndex, index);
                                } else if (0 == type.CompareTo("bool[]")) {
                                    int index = -1;
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        int[] vals = new int[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = (v[i] == "true" || v[i] == "1") ? 1 : 0;
                                        }
                                        index = table.AddIntList(vals);
                                    }
                                    WriteIndex(record, colIndex, index);
                                } else if (0 == type.CompareTo("string[]")) {
                                    int index = -1;
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] vals = val.Split(',', ';', '|', ' ');
                                        index = table.AddStrList(vals);
                                    }
                                    WriteIndex(record, colIndex, index);
                                }
                            } catch (Exception ex) {
                                LogSystem.Error("[line:{0} col:{1}] “{2}”, exception:{3}\n{4}", rowIndex + 1, colIndex + 1, lines[rowIndex], ex.Message, ex.StackTrace);
                            }
                            ++colIndex;
                        }
                    }
                    table.Save(outFile);
                }
                return true;
            } catch (Exception ex) {
                LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }
        private unsafe static void WriteIndex(byte[] record, int colIndex, int index)
        {
            fixed (byte* p = record) {
                int* pInts = (int*)p;
                *(pInts + colIndex) = index;
            }
        }
        private unsafe static void WriteFloat(byte[] record, int colIndex, float val)
        {
            fixed (byte* p = record) {
                float* pInts = (float*)p;
                *(pInts + colIndex) = val;
            }
        }
    }
}
