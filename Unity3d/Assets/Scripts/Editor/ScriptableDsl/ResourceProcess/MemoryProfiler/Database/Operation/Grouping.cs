using System;
using Unity.Profiling;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal interface IArithmetic<DataT>
    {
        DataT Add(DataT a, DataT b);
        DataT Sub(DataT a, DataT b);
        DataT Div(DataT a, DataT b);
        DataT Min(DataT a, DataT b);
        DataT Max(DataT a, DataT b);
        DataT SubAbs(DataT a, DataT b);
        DataT Abs(DataT a);
        DataT Make(int a);
        DataT Make(float a);
        DataT Zero();
        DataT One();
        DataT MinValue();
        DataT MaxValue();
        int Compare(DataT a, DataT b);
    }

    internal class Arithmetic : IArithmetic<int>, IArithmetic<long>, IArithmetic<ulong>, IArithmetic<float>
    {
        int IArithmetic<int>.Add(int a, int b) { return a + b; }
        int IArithmetic<int>.Sub(int a, int b) { return a - b; }
        int IArithmetic<int>.Div(int a, int b) { return a / b; }
        int IArithmetic<int>.Min(int a, int b) { return a < b ? a : b; }
        int IArithmetic<int>.Max(int a, int b) { return a > b ? a : b; }
        int IArithmetic<int>.Abs(int a) { return Math.Abs(a); }
        int IArithmetic<int>.SubAbs(int a, int b) { return Math.Abs(a - b); }
        int IArithmetic<int>.Make(int a) { return (int)a; }
        int IArithmetic<int>.Make(float a) { return (int)a; }
        int IArithmetic<int>.Zero() { return 0; }
        int IArithmetic<int>.One() { return 1; }
        int IArithmetic<int>.MinValue() { return int.MinValue; }
        int IArithmetic<int>.MaxValue() { return int.MaxValue; }
        int IArithmetic<int>.Compare(int a, int b) { return a.CompareTo(b); }
        long IArithmetic<long>.Add(long a, long b) { return a + b; }
        long IArithmetic<long>.Sub(long a, long b) { return a - b; }
        long IArithmetic<long>.Div(long a, long b) { return a / b; }
        long IArithmetic<long>.Min(long a, long b) { return a < b ? a : b; }
        long IArithmetic<long>.Max(long a, long b) { return a > b ? a : b; }
        long IArithmetic<long>.SubAbs(long a, long b) { return Math.Abs(a - b); }
        long IArithmetic<long>.Abs(long a) { return Math.Abs(a); }
        long IArithmetic<long>.Make(int a) { return (long)a; }
        long IArithmetic<long>.Make(float a) { return (long)a; }
        long IArithmetic<long>.Zero() { return 0; }
        long IArithmetic<long>.One() { return 1; }
        long IArithmetic<long>.MinValue() { return long.MinValue; }
        long IArithmetic<long>.MaxValue() { return long.MaxValue; }
        int IArithmetic<long>.Compare(long a, long b) { return a.CompareTo(b); }
        ulong IArithmetic<ulong>.Add(ulong a, ulong b) { return a + b; }
        ulong IArithmetic<ulong>.Sub(ulong a, ulong b) { return a - b; }
        ulong IArithmetic<ulong>.Div(ulong a, ulong b) { return a / b; }
        ulong IArithmetic<ulong>.Min(ulong a, ulong b) { return a < b ? a : b; }
        ulong IArithmetic<ulong>.Max(ulong a, ulong b) { return a > b ? a : b; }
        ulong IArithmetic<ulong>.SubAbs(ulong a, ulong b) { return a > b ? a - b : b - a; }
        ulong IArithmetic<ulong>.Abs(ulong a) { return a; }
        ulong IArithmetic<ulong>.Make(int a) { return (ulong)a; }
        ulong IArithmetic<ulong>.Make(float a) { return (ulong)a; }
        ulong IArithmetic<ulong>.Zero() { return 0; }
        ulong IArithmetic<ulong>.One() { return 1; }
        ulong IArithmetic<ulong>.MinValue() { return ulong.MinValue; }
        ulong IArithmetic<ulong>.MaxValue() { return ulong.MaxValue; }
        int IArithmetic<ulong>.Compare(ulong a, ulong b) { return a.CompareTo(b); }
        float IArithmetic<float>.Add(float a, float b) { return a + b; }
        float IArithmetic<float>.Sub(float a, float b) { return a - b; }
        float IArithmetic<float>.Div(float a, float b) { return a / b; }
        float IArithmetic<float>.Min(float a, float b) { return a < b ? a : b; }
        float IArithmetic<float>.Max(float a, float b) { return a > b ? a : b; }
        float IArithmetic<float>.SubAbs(float a, float b) { return Math.Abs(a - b); }
        float IArithmetic<float>.Abs(float a) { return Math.Abs(a); }
        float IArithmetic<float>.Make(int a) { return (float)a; }
        float IArithmetic<float>.Make(float a) { return (float)a; }
        float IArithmetic<float>.Zero() { return 0; }
        float IArithmetic<float>.One() { return 1; }
        float IArithmetic<float>.MinValue() { return float.MinValue; }
        float IArithmetic<float>.MaxValue() { return float.MaxValue; }
        int IArithmetic<float>.Compare(float a, float b) { return a.CompareTo(b); }
    }

    internal class Grouping
    {
        public abstract class GroupCollection
        {
            public abstract long GetGroupCount();
            public abstract Group GetGroup(long index);
        }
        public abstract class Group
        {
            public abstract ArrayRange GetIndices(GroupCollection coll);
        }

        internal interface IGroupAlgorithm
        {
            GroupCollection Group(Column sourceColumn, ArrayRange range, SortOrder order);
        }
        internal interface IMergeAlgorithm
        {
            void Merge(Column destColumn, long destRow, Column sourceColumn, ArrayRange sourceIndices);
            IComparable Merge(Column sourceColumn, ArrayRange sourceIndices);
            LinkRequest GetLink(Column sourceColumn, ArrayRange sourceIndices);

            // Only display a value on merged rows. normal entries will display an empty string
            bool IsDisplayMergedRowsOnly();
        }

        public enum MergeAlgo
        {
            none,
            first,
            sum,
            average,
            median,
            deviation,
            max,
            min,
            count,
            sumpositive,
        }


        public class GroupByDuplicate : Grouping.IGroupAlgorithm
        {
            public class DupGroup : Grouping.Group
            {
                public DupGroup(Range range)
                {
                    this.range = range;
                }

                public Range range;
                public override ArrayRange GetIndices(Grouping.GroupCollection coll)
                {
                    DupCollection dcoll = (DupCollection)coll;
                    return new ArrayRange(dcoll.m_sortedIndex, range);
                }
            }
            public class DupCollection : Grouping.GroupCollection
            {
                public long[] m_sortedIndex;
                public DupGroup[] m_Groups;
                public override long GetGroupCount()
                {
                    return m_Groups.Length;
                }

                public override Grouping.Group GetGroup(long index)
                {
                    return m_Groups[index];
                }
            }

            Grouping.GroupCollection Grouping.IGroupAlgorithm.Group(Column sourceColumn, ArrayRange indices, SortOrder order)
            {
                DupCollection coll = new DupCollection();
                if (order == SortOrder.None)
                {
                    order = SortOrder.Ascending;
                }
                coll.m_sortedIndex = sourceColumn.GetSortIndex(order, indices, false);
                System.Collections.Generic.List<DupGroup> groups = new System.Collections.Generic.List<DupGroup>();
                int iGroupFirst = 0;
                for (int i = 1, j = 0; i < coll.m_sortedIndex.Length; j = i++)
                {
                    if (sourceColumn.CompareRow(coll.m_sortedIndex[j], coll.m_sortedIndex[i]) != 0)
                    {
                        //create group;
                        Range range = Range.FirstToLast(iGroupFirst, i);
                        groups.Add(new DupGroup(range));
                        iGroupFirst = i;
                    }
                }
                Range rangeLast = Range.FirstToLast(iGroupFirst, coll.m_sortedIndex.Length);
                if (rangeLast.Length > 0)
                {
                    groups.Add(new DupGroup(rangeLast));
                }
                coll.m_Groups = groups.ToArray();

                return coll;
            }
        }
        private static GroupByDuplicate _groupByDuplicate = null;
        public static GroupByDuplicate groupByDuplicate
        {
            get
            {
                if (_groupByDuplicate == null)
                {
                    _groupByDuplicate = new GroupByDuplicate();
                }
                return _groupByDuplicate;
            }
        }


        private class MergeAlgoKey : IComparable
        {
            int IComparable.CompareTo(object obj)
            {
                MergeAlgoKey o = (MergeAlgoKey)obj;
                if (o == null) return 1;
                int r1 = algo.CompareTo(o.algo);
                if (r1 == 0)
                {
                    return type.Name.CompareTo(o.type.Name);
                }
                return r1;
            }

            public MergeAlgoKey(MergeAlgo algo, Type ColumnDataType)
            {
                this.algo = algo;
                this.type = ColumnDataType;
            }

            public MergeAlgo algo;
            public Type type;
        }
        private static System.Collections.Generic.SortedDictionary<MergeAlgoKey, IMergeAlgorithm> _m_MergeAlgorithms;
        private static System.Collections.Generic.SortedDictionary<MergeAlgoKey, IMergeAlgorithm> m_MergeAlgorithms
        {
            get
            {
                if (_m_MergeAlgorithms == null)
                {
                    _m_MergeAlgorithms = new System.Collections.Generic.SortedDictionary<MergeAlgoKey, IMergeAlgorithm>();
                    Arithmetic ar = new Arithmetic();
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.first, typeof(int)), new First<int>());
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.first, typeof(long)), new First<long>());
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.first, typeof(ulong)), new First<ulong>());
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.first, typeof(float)), new First<float>());
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.first, typeof(bool)), new First<bool>());

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sum, typeof(int)), new Sum<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sum, typeof(long)), new Sum<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sum, typeof(ulong)), new Sum<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sum, typeof(float)), new Sum<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.max, typeof(int)), new Max<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.max, typeof(long)), new Max<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.max, typeof(ulong)), new Max<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.max, typeof(float)), new Max<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.min, typeof(int)), new Min<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.min, typeof(long)), new Min<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.min, typeof(ulong)), new Min<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.min, typeof(float)), new Min<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.average, typeof(int)), new Average<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.average, typeof(long)), new Average<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.average, typeof(ulong)), new Average<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.average, typeof(float)), new Average<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.median, typeof(int)), new Median<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.median, typeof(long)), new Median<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.median, typeof(ulong)), new Median<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.median, typeof(float)), new Median<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.deviation, typeof(int)), new Deviation<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.deviation, typeof(long)), new Deviation<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.deviation, typeof(ulong)), new Deviation<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.deviation, typeof(float)), new Deviation<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.count, typeof(int)), new Count<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.count, typeof(long)), new Count<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.count, typeof(ulong)), new Count<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.count, typeof(float)), new Count<float>(ar));

                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sumpositive, typeof(int)), new SumPositive<int>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sumpositive, typeof(long)), new SumPositive<long>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sumpositive, typeof(ulong)), new SumPositive<ulong>(ar));
                    _m_MergeAlgorithms.Add(new MergeAlgoKey(MergeAlgo.sumpositive, typeof(float)), new SumPositive<float>(ar));
                }
                return _m_MergeAlgorithms;
            }
        }
        public static IMergeAlgorithm GetMergeAlgo(MergeAlgo ma, Type type)
        {
            IMergeAlgorithm a;
            m_MergeAlgorithms.TryGetValue(new MergeAlgoKey(ma, type), out a);
            return a;
        }

        public abstract class TypedAlgorithm<DataT> : IMergeAlgorithm where DataT : System.IComparable
        {
            protected bool m_IsDisplayMergedOnly = false;
            public void Merge(Column destColumn, long destRow, Column sourceColumn, ArrayRange sourceIndices)
            {
                ColumnTyped<DataT> destColumnT = (ColumnTyped<DataT>)destColumn;
                ColumnTyped<DataT> sourceColumnT = (ColumnTyped<DataT>)sourceColumn;
                if (destColumnT == null || sourceColumnT == null)
                {
                    throw new System.Exception("Bad column type");
                }
                GroupTyped(destColumnT, destRow, sourceColumnT, sourceIndices);
            }

            public IComparable Merge(Column sourceColumn, ArrayRange sourceIndices)
            {
                return GroupTyped(sourceColumn as ColumnTyped<DataT>, sourceIndices);
            }

            LinkRequest IMergeAlgorithm.GetLink(Column sourceColumn, ArrayRange sourceIndices)
            {
                return this.GetLink(sourceColumn, sourceIndices);
            }

            protected virtual LinkRequest GetLink(Column sourceColumn, ArrayRange sourceIndices)
            {
                return null;
            }

            protected abstract void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices);
            protected abstract IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices);

            bool IMergeAlgorithm.IsDisplayMergedRowsOnly()
            {
                return m_IsDisplayMergedOnly;
            }

            // TODO, these should be used in the group table
            public virtual DataT GetRowValue(DataT mergedValue, ColumnTyped<DataT> sourceColumn, long row)
            {
                return sourceColumn.GetRowValue(row);
            }

            public virtual string GetRowValueString(DataT mergedValue, ColumnTyped<DataT> sourceColumn, long row, IDataFormatter formatter)
            {
                return sourceColumn.GetRowValueString(row, formatter);
            }
        }

        internal class First<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                return sourceColumn.GetRowValue(sourceIndices[0]);
            }

            protected override LinkRequest GetLink(Column sourceColumn, ArrayRange sourceIndices)
            {
                return sourceColumn.GetRowLink(sourceIndices[0]);
            }
        }
        public class Count<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Count(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                return math.Make(sourceIndices.Count);
            }
        }
        public class Sum<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Sum(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT result = math.Zero();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    result = math.Add(result, v);
                }
                return result;
            }
        }
        public class SumPositive<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public SumPositive(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT result = math.Zero();
                DataT z = math.Zero();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    if (math.Compare(v, z) >= 0)
                    {
                        result = math.Add(result, v);
                    }
                }
                return result;
            }
        }
        public class Min<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Min(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT result = math.MaxValue();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    result = math.Min(result, v);
                }
                return result;
            }
        }
        public class Max<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Max(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT result = math.MinValue();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    result = math.Max(result, v);
                }
                return result;
            }
        }
        public class Average<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Average(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT result = math.Zero();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    result = math.Add(result, v);
                }
                result = math.Div(result, math.Make(sourceIndices.Count));
                return result;
            }
        }

        public class Median<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Median(IArithmetic<DataT> math)
            {
                this.math = math;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                long count = sourceIndices.Count;
                DataT[] d = new DataT[count];
                int i = 0;
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    d[i] = v;
                    ++i;
                }
                Array.Sort(d, Comparer.Ascending<DataT>());
                DataT median;
                long mid = count / 2;
                if (count % 2 == 0)
                {
                    median = math.Div(math.Add(d[mid - 1], d[mid]), math.Make(2));
                }
                else
                {
                    median = d[mid];
                }
                return median;
            }
        }

        public class Deviation<DataT> : TypedAlgorithm<DataT> where DataT : System.IComparable
        {
            protected IArithmetic<DataT> math;
            public Deviation(IArithmetic<DataT> math)
            {
                this.math = math;
                m_IsDisplayMergedOnly = true;
            }

            protected override void GroupTyped(ColumnTyped<DataT> destColumn, long destRow, ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                destColumn.SetRowValue(destRow, (DataT)GroupTyped(sourceColumn, sourceIndices));
            }

            protected override IComparable GroupTyped(ColumnTyped<DataT> sourceColumn, ArrayRange sourceIndices)
            {
                DataT count = math.Make(sourceIndices.Count);
                DataT avg = math.Zero();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    avg = math.Add(avg, v);
                }
                avg = math.Div(avg, count);

                DataT dev = math.Zero();
                foreach (var v in sourceColumn.VisitRows(sourceIndices))
                {
                    dev = math.Add(dev, math.SubAbs(avg, v));
                }
                dev = math.Div(dev, count);
                return dev;
            }

            public override DataT GetRowValue(DataT mergedValue, ColumnTyped<DataT> sourceColumn, long row)
            {
                return sourceColumn.GetRowValue(row);
            }

            public override string GetRowValueString(DataT mergedValue, ColumnTyped<DataT> sourceColumn, long row, IDataFormatter formatter)
            {
                return sourceColumn.GetRowValueString(row, formatter);
            }
        }
    }
}
