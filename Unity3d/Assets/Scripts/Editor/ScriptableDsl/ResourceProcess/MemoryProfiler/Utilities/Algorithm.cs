using System;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal static class Algorithm
    {
        public static int LowerBound<T>(T[] array, T v) where T : IComparable<T>
        {
            return LowerBound(array, 0, array.Length, v);
        }

        public static int LowerBound<T>(T[] array, int first, int count, T v) where T : IComparable<T>
        {
            while (count > 0)
            {
                int it = first;
                int step = count / 2;
                it += step;
                if (array[it].CompareTo(v) < 0)
                {
                    first = it + 1;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }

        public static int UpperBound<T>(T[] array, T v) where T : IComparable<T>
        {
            return UpperBound(array, 0, array.Length, v);
        }

        public static int UpperBound<T>(T[] array, int first, int count, T v) where T : IComparable<T>
        {
            while (count > 0)
            {
                int it = first;
                int step = count / 2;
                it += step;
                if (v.CompareTo(array[it]) >= 0)
                {
                    first = it + 1;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }
    }
}
