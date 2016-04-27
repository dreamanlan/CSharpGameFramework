using System;
using System.Collections.Generic;

namespace DemoCommon
{
    public sealed partial class DataRecordUtility
    {
        public static int ExtractInt(BinaryTable table, int recordVal, int defaultVal)
        {
            return recordVal;
        }
        public static long ExtractLong(BinaryTable table, int recordVal, long defaultVal)
        {
            return recordVal;
        }
        public static float ExtractFloat(BinaryTable table, float recordVal, float defaultVal)
        {
            return recordVal;
        }
        public static bool ExtractBool(BinaryTable table, int recordVal, bool defaultVal)
        {
            return recordVal != 0;
        }
        public static string ExtractString(BinaryTable table, int recordVal, string defaultVal)
        {
            string ret = table.GetString(recordVal);
            if (string.IsNullOrEmpty(ret)) {
                ret = defaultVal;
            }
            return ret;
        }
        public static List<int> ExtractIntList(BinaryTable table, int recordVal, int[] defaultVal)
        {
            int[] vals = ExtractIntArray(table, recordVal, defaultVal);
            if (null != vals)
                return new List<int>(vals);
            else
                return new List<int>();
        }
        public static int[] ExtractIntArray(BinaryTable table, int recordVal, int[] defaultVal)
        {
            int[] ret = table.GetIntList(recordVal);
            if (null == ret) {
                ret = defaultVal;
            }
            return ret;
        }
        public static List<float> ExtractFloatList(BinaryTable table, int recordVal, float[] defaultVal)
        {
            float[] vals = ExtractFloatArray(table, recordVal, defaultVal);
            if (null != vals)
                return new List<float>(vals);
            else
                return new List<float>();
        }
        public static float[] ExtractFloatArray(BinaryTable table, int recordVal, float[] defaultVal)
        {
            float[] ret = table.GetFloatList(recordVal);
            if (null == ret) {
                ret = defaultVal;
            }
            return ret;
        }
        public static List<string> ExtractStringList(BinaryTable table, int recordVal, string[] defaultVal)
        {
            string[] vals = ExtractStringArray(table, recordVal, defaultVal);
            if (null != vals)
                return new List<string>(vals);
            else
                return new List<string>();
        }
        public static string[] ExtractStringArray(BinaryTable table, int recordVal, string[] defaultVal)
        {
            string[] ret = table.GetStrList(recordVal);
            if (null == ret) {
                ret = defaultVal;
            }
            return ret;
        }

        public static int SetValue(BinaryTable table, bool val, bool defaultVal)
        {
            return val ? 1 : 0;
        }
        public static int SetValue(BinaryTable table, int val, int defaultVal)
        {
            return val;
        }
        public static int SetValue(BinaryTable table, long val, long defaultVal)
        {
            return (int)val;
        }
        public static float SetValue(BinaryTable table, float val, float defaultVal)
        {
            return val;
        }
        public static int SetValue(BinaryTable table, string val, string defaultVal)
        {
            if (0 == val.CompareTo(defaultVal)) {
                return -1;
            }
            return table.AddString(val);
        }
        public static int SetValue(BinaryTable table, int[] vals, int[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddIntList(vals);
        }
        public static int SetValue(BinaryTable table, List<int> vals, int[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddIntList(vals.ToArray());
        }
        public static int SetValue(BinaryTable table, float[] vals, float[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddFloatList(vals);
        }
        public static int SetValue(BinaryTable table, List<float> vals, float[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddFloatList(vals.ToArray());
        }
        public static int SetValue(BinaryTable table, string[] vals, string[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddStrList(vals);
        }
        public static int SetValue(BinaryTable table, List<string> vals, string[] defaultVal)
        {
            if (IsEqual(vals, defaultVal)) {
                return -1;
            }
            return table.AddStrList(vals.ToArray());
        }

        public static void ExtractValues<T>(IList<T> list, int ct, params T[] vals)
        {
            for (int i = 0; (i < ct) && (i < vals.Length); i++) {
                list.Add(vals[i]);
            }
        }

        private static bool IsEqual(IList<int> a, IList<int> b)
        {
            if (null == a && null != b || null != a && null == b)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; ++i) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        private static bool IsEqual(IList<float> a, IList<float> b)
        {
            if (null == a && null != b || null != a && null == b)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; ++i) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        private static bool IsEqual(IList<string> a, IList<string> b)
        {
            if (null == a && null != b || null != a && null == b)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; ++i) {
                if (0 != a[i].CompareTo(b[i]))
                    return false;
            }
            return true;
        }
    }
}
