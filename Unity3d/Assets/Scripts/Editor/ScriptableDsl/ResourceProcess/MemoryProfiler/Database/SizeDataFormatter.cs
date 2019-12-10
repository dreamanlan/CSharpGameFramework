using System;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    internal class SizeDataFormatter : IDataFormatter
        , IDataFormatter<short>
        , IDataFormatter<int>
        , IDataFormatter<long>
        , IDataFormatter<ushort>
        , IDataFormatter<uint>
        , IDataFormatter<ulong>
        , IDataFormatter<float>
        , IDataFormatter<double>
    {
        public string FormatSize(ulong value, bool negative)
        {
            unchecked
            {
                return string.Format("{0}{1}", negative ? "-" : "", EditorUtility.FormatBytes((long)value));
            }
        }

        string IDataFormatter.Format(object obj)
        {
            if (obj is short) return Format((short)obj);
            if (obj is int) return Format((int)obj);
            if (obj is long) return Format((long)obj);
            if (obj is float) return Format((float)obj);
            if (obj is double) return Format((double)obj);

            return FormatSize((ulong)obj, false);
        }

        public string Format(short obj)
        {
            if (obj < 0)
            {
                return FormatSize((ulong)Math.Abs(obj), true);
            }
            else
            {
                return FormatSize((ulong)obj, false);
            }
        }

        public string Format(int obj)
        {
            if (obj < 0)
            {
                return FormatSize((ulong)Math.Abs(obj), true);
            }
            else
            {
                return FormatSize((ulong)obj, false);
            }
        }

        public string Format(long obj)
        {
            if (obj < 0)
            {
                return FormatSize((ulong)Math.Abs(obj), true);
            }
            else
            {
                return FormatSize((ulong)obj, false);
            }
        }

        public string Format(ushort obj)
        {
            return FormatSize(obj, false);
        }

        public string Format(uint obj)
        {
            return FormatSize(obj, false);
        }

        public string Format(ulong obj)
        {
            return FormatSize(obj, false);
        }

        public string Format(float obj)
        {
            if (obj < 0)
            {
                return FormatSize((ulong)Math.Abs(obj), true);
            }
            else
            {
                return FormatSize((ulong)obj, false);
            }
        }

        public string Format(double obj)
        {
            if (obj < 0)
            {
                return FormatSize((ulong)Math.Abs(obj), true);
            }
            else
            {
                return FormatSize((ulong)obj, false);
            }
        }
    }
}
