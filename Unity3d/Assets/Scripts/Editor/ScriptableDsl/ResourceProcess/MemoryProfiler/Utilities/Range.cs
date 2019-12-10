using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal struct Range
    {
        public long First;
        /// <summary>
        /// past-the-end
        /// </summary>
        public long Last;

        public static readonly Range None = new Range(0, 0);
        public static readonly Range All = new Range(long.MinValue, long.MaxValue);

        public static Range FirstToLast(long first, long last)
        {
            return new Range(first, last);
        }

        public static Range FirstPlusLength(long first, long length)
        {
            return new Range(first, first + length);
        }

        public bool IsNone { get { return First == Last; } }
        public bool IsAll
        {
            get
            {
                return First == long.MinValue && Last == long.MaxValue;
            }
        }
        public long Length
        {
            get
            {
                return Last - First;
            }
        }
        public long this[long i]
        {
            get
            {
                return First + i;
            }
        }
        private Range(long first, long last)
        {
            this.First = first;
            this.Last = last;
        }

        public Range Union(long value)
        {
            Range o;
            o.First = Math.Min(First, value);
            o.Last = Math.Max(Last, value + 1);
            return o;
        }

        public Range Union(Range value)
        {
            Range o;
            o.First = Math.Min(First, value.First);
            o.Last = Math.Max(Last, value.Last);
            return o;
        }

        public Range Union(ArrayRange value)
        {
            if (value.IsSequence)
            {
                return Union(value.Sequence);
            }
            Range o = this;
            foreach (var v in value)
            {
                o = o.Union(v);
            }
            return o;
        }

        public Range Intersection(Range other)
        {
            Range o;
            o.First = Math.Max(First, other.First);
            o.Last = Math.Min(Last, other.Last);
            if (o.Length <= 0) return Range.None;
            return o;
        }

        /// <summary>
        /// Return a copy of this range with absolute value of its length
        /// </summary>
        public Range Abs
        {
            get
            {
                Range o;
                o.First = Math.Min(First, Last);
                o.Last = Math.Max(First, Last);
                return o;
            }
        }
        /// <summary>
        /// Test if this range can coalesce with another range, meaning that their union range includes only both ranges without any gap in between.
        /// </summary>
        /// <param name="other">The other range to test with</param>
        /// <returns>
        /// true if this range overlaps or touches the other range.
        /// false if there's a gap between the 2 ranges.
        /// </returns>
        public bool CanCoalesce(Range other)
        {
            if (IsNone || other.IsNone) return true;
            // based on minkowski difference
            long incLastA = Last + 1;
            long incLastB = other.Last + 1;
            bool b0 = First - other.First < 0;
            bool b1 = First - incLastB < 0;
            bool b2 = incLastA - other.First < 0;
            bool b3 = incLastA - incLastB < 0;
            return b0 != b1 || b0 != b2 || b0 != b3;
        }

        public long[] ToArray()
        {
            var o = new long[Length];
            for (long i = 0; i != Length; ++i)
            {
                o[i] = i + First;
            }
            return o;
        }

        public static Range operator+(Range r, long x)
        {
            return new Range(r.First + x, r.Last + x);
        }

        public static Range operator-(Range r, long x)
        {
            return new Range(r.First - x, r.Last - x);
        }
    }

    internal struct ArrayRange : IEnumerable<long>
    {
        public static ArrayRange Single(long index)
        {
            return new ArrayRange(index, index + 1);
        }

        public static ArrayRange FirstLast(long first, long last)
        {
            return new ArrayRange(first, last);
        }

        public static ArrayRange FirstLength(long first, long length)
        {
            return new ArrayRange(first, first + length);
        }

        public static ArrayRange IndexArray(long[] array)
        {
            return new ArrayRange(array);
        }

        public static ArrayRange FirstLastIndexArray(long[] array, long first, long last)
        {
            return new ArrayRange(array, first, last);
        }

        public static readonly ArrayRange All = new ArrayRange(Range.All);
        public static readonly ArrayRange None = new ArrayRange(0, 0);

        public ArrayRange(long indexFirst, long indexLast)
        {
            mArray = null;
            Sequence = Range.FirstToLast(indexFirst, indexLast);
        }

        public ArrayRange(long[] array, long indexFirst, long indexLast)
        {
            mArray = array;
            Sequence = Range.FirstToLast(indexFirst, indexLast);
        }

        public ArrayRange(Range range)
        {
            mArray = null;
            Sequence = range;
        }

        public ArrayRange(long[] array, Range range)
        {
            mArray = array;
            Sequence = range;
        }

        public ArrayRange(long[] array)
        {
            mArray = array;
            Sequence = Range.FirstPlusLength(0, array.Length);
        }

        public long[] ToArray()
        {
            if (IsAll) throw new InvalidOperationException("Cannot call ToArray on a 'All' ArrayRange");
            if (mArray != null)
            {
                if (Sequence.First == 0 && Sequence.Length == mArray.LongLength)
                {
                    return mArray;
                }
                var o = new long[Sequence.Length];
                System.Array.Copy(mArray, Sequence.First, o, 0, Sequence.Length);
                return o;
            }
            else
            {
                return Sequence.ToArray();
            }
        }

        private long[] mArray;

        /// <summary>
        /// Array of indices used when 'IsIndex' returns true
        /// </summary>
        public long[] Array { get { return mArray; } }

        public Range Sequence;

        public long Count
        {
            get
            {
                if (IsAll) throw new InvalidOperationException("IndexCount is unknown when using an 'All' ArrayRange");
                return Sequence.Length;
            }
        }
        public long this[long i]
        {
            get
            {
                if (IsAll) throw new InvalidOperationException("Cannot retrieve indices on a 'All' ArrayRange");
                if (mArray != null)
                {
                    return Array[Sequence.First + i];
                }
                return Sequence.First + i;
            }
        }
        public bool IsAll
        {
            get
            {
                return mArray == null && Sequence.IsAll;
            }
        }
        public bool IsNone
        {
            get
            {
                return Sequence.IsNone;
            }
        }

        /// <summary>
        /// if is a sequence between 2 numbers. output = [first, last[
        /// </summary>
        public bool IsSequence { get { return mArray == null; } }

        /// <summary>
        /// If is a sequence in an array of indices.  output = index([first, last[)
        /// </summary>
        public bool IsIndex { get { return mArray != null; } }

        public bool IsZeroRange { get { return Sequence.First == Sequence.Last; } }
        public bool IsPositiveRange { get { return Sequence.First > Sequence.Last; } }
        public bool IsNegativeRange { get { return Sequence.Last < Sequence.First; } }

        public ArrayRange Union(ArrayRange value)
        {
            ArrayRange o;
            if (IsAll || value.IsAll) return All;
            if (IsSequence && value.IsSequence)
            {
                // check if can coalesce both range
                if (Sequence.CanCoalesce(value.Sequence))
                {
                    o.mArray = null;
                    o.Sequence = Sequence.Union(value.Sequence);
                    return o;
                }
            }

            o.mArray = new long[Count + value.Count];
            o.Sequence.First = 0;
            o.Sequence.Last = Count + value.Count;

            if (value.IsIndex && IsIndex)
            {
                // join arrays
                System.Array.Copy(mArray, Sequence.First, o.mArray, 0, Count);
                System.Array.Copy(value.mArray, value.Sequence.First, o.mArray, Count, value.Count);
            }
            else
            {
                int i = 0;
                foreach (var v in this)
                {
                    o.mArray[i] = v;
                    ++i;
                }
                foreach (var v in value)
                {
                    o.mArray[i] = v;
                    ++i;
                }
            }
            return o;
        }

        public struct Enumerator : IEnumerator<long>
        {
            ArrayRange m_ArrayRange;
            int m_Cursor;
            public Enumerator(ArrayRange range)
            {
                m_ArrayRange = range;
                m_Cursor = -1;
            }

            /// <summary>
            /// left not implemented so to not create boxing operation using non-generic enumerators
            /// </summary>
            object IEnumerator.Current { get { throw new NotImplementedException(); } }
            public bool MoveNext()
            {
                ++m_Cursor;
                return m_Cursor < m_ArrayRange.Count;
            }

            public void Reset()
            {
                m_Cursor = 0;
            }

            public long Current { get { return m_ArrayRange[m_Cursor]; } }
            void IDisposable.Dispose() {}
        }
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<long> IEnumerable<long>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
