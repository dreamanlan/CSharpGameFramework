using System;
using Unity.MemoryProfilerForExtension.Editor.Database.Operation;
using Unity.Profiling;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    /// <summary>
    /// A partial implementation of a column that holds value of a specific type `DataT`
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal abstract class ColumnTyped<DataT> : Column where DataT : System.IComparable
    {
        protected long[] m_SortIndexAsc;
        protected bool m_IsDataNullable;
        public ColumnTyped()
        {
            var type = typeof(DataT);
            m_IsDataNullable = Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }

        //Call this to get a value to do computation with. May differ from GetRowValueString
        public abstract DataT GetRowValue(long row);
        public abstract void SetRowValue(long row, DataT value);

        public virtual System.Collections.Generic.IEnumerable<DataT> VisitRows(ArrayRange ar)
        {
            for (long i = 0; i != ar.Count; ++i)
            {
                yield return GetRowValue(ar[i]);
            }
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            if (row >= GetRowCount())
            {
                return "Out of Range";
            }
            return formatter.Format(GetRowValue(row));
        }

        public override int CompareRow(long rowA, long rowB)
        {
            var a = GetRowValue(rowA);
            var b = GetRowValue(rowB);
            if (m_IsDataNullable)
            {
                if (a != null && b == null)
                    return 1;
                else if (a == null && b != null)
                    return -1;
                else if (a == null && b == null)
                    return 0;
            }
            return a.CompareTo(b);
        }

        protected long[] GetSortIndexAsc()
        {
            if (m_SortIndexAsc == null)
            {
                m_SortIndexAsc = GetSortIndex(Operation.Comparer.Ascending<DataT>(), new ArrayRange(0, GetRowCount()), false);
            }
            return m_SortIndexAsc;
        }

        public override long[] GetSortIndex(SortOrder order, ArrayRange indices, bool relativeIndex)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    return GetSortIndex(Operation.Comparer.Ascending<DataT>(), indices, relativeIndex);
                case SortOrder.Descending:
                    return GetSortIndex(Operation.Comparer.Descending<DataT>(), indices, relativeIndex);
            }
            throw new System.Exception("Bad SordOrder");
        }

        protected virtual long[] GetSortIndex(System.Collections.Generic.IComparer<DataT> comparer, ArrayRange indices, bool relativeIndex)
        {
            Update();
            long count = indices.Count;
            DataT[] keys = new DataT[count];


            //create index array
            long[] index = new long[count];
            if (relativeIndex)
            {
                for (long i = 0; i != count; ++i)
                {
                    index[i] = i;
                    keys[i] = GetRowValue(indices[i]);
                }
            }
            else
            {
                for (long i = 0; i != count; ++i)
                {
                    index[i] = indices[i];
                    keys[i] = GetRowValue(indices[i]);
                }
            }
            System.Array.Sort(keys, index, comparer);

            return index;
        }


        public override long[] GetMatchIndex(ArrayRange rowRange, Operation.Operator operation, Operation.Expression expression, long expressionRowFirst, bool rowToRow)
        {
            Update();
            long count = rowRange.Count;
            long[] matchedIndices = new long[count];
            long indexOflastMatchedIndex = 0;
            Operation.Operation.ComparableComparator comparator = Operation.Operation.GetComparator(type, expression.type);
            if (rowToRow)
            {
                for (long i = 0; i != count; ++i)
                {
                    var leftValue = GetRowValue(rowRange[i]);
                    if (Operation.Operation.Match(operation, comparator, leftValue, expression, expressionRowFirst + i))
                    {
                        matchedIndices[indexOflastMatchedIndex] = rowRange[i];
                        ++indexOflastMatchedIndex;
                    }
                }
            }
            else
            {
                if (Operation.Operation.IsOperatorOneToMany(operation))
                {
                    for (int i = 0; i != count; ++i)
                    {
                        var leftValue = GetRowValue(rowRange[i]);
                        if (Operation.Operation.Match(operation, comparator, leftValue, expression, expressionRowFirst))
                        {
                            matchedIndices[indexOflastMatchedIndex] = rowRange[i];
                            ++indexOflastMatchedIndex;
                        }
                    }
                }
                else
                {
                    var valueRight = expression.GetComparableValue(expressionRowFirst);
                    for (int i = 0; i != count; ++i)
                    {
                        var leftValue = GetRowValue(rowRange[i]);
                        if (Operation.Operation.Match(operation, comparator, leftValue, valueRight))
                        {
                            matchedIndices[indexOflastMatchedIndex] = rowRange[i];
                            ++indexOflastMatchedIndex;
                        }
                    }
                }
            }

            if (indexOflastMatchedIndex != count)
            {
                System.Array.Resize(ref matchedIndices, (int)indexOflastMatchedIndex);
            }

            return matchedIndices;
        }

        public override long[] GetMatchIndex(ArrayRange indices, Operation.Matcher matcher)
        {
            var expression = new Operation.ExpColumn<DataT>(this);
            var result = matcher.GetMatchIndex(expression, indices);
            return result;
        }

        private long LowerBound(IComparable key, Operation.Operation.ComparableComparator comparator)
        {
            long[] sortedIndex = GetSortIndexAsc();
            long step;
            long first = 0;
            long count = sortedIndex.Length;
            while (count > 0)
            {
                long it = first;
                step = count / 2;
                it += step;
                var val = GetRowValue(sortedIndex[it]);
                int result = comparator(val, key);
                if (result < 0)
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

        private long UpperBound(IComparable key, Operation.Operation.ComparableComparator comparator)
        {
            long[] sortedIndex = GetSortIndexAsc();
            long first = 0;
            long count = sortedIndex.Length;
            while (count > 0)
            {
                long it = first;
                long step = count / 2;
                it += step;

                var value = GetRowValue(sortedIndex[it]);
                int result = comparator(key, value);
                if (result >= 0)
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


        public override long GetFirstMatchIndex(Operation.Operator operation, Operation.Expression expression, long expRowFirst)
        {
            Update();
            long[] sortedIndex = GetSortIndexAsc();

            Operation.Operation.ComparableComparator comparator = Operation.Operation.GetComparator(type, expression.type);
            var val2 = expression.GetComparableValue(expRowFirst);

            long firstMatchIndex = -1;
            switch (operation)
            {
                case Operation.Operator.Less:
                    {
                        long iFirst = sortedIndex[0];
                        var val1 = GetRowValue(iFirst);
                        int result = comparator(val1, val2);
                        if (result < 0)
                        {
                            firstMatchIndex = iFirst;
                        }
                        break;
                    }
                case Operation.Operator.LessEqual:
                    {
                        long iFirst = sortedIndex[0];
                        var val1 = GetRowValue(iFirst);
                        int result = comparator(val1, val2);
                        if (result <= 0)
                        {
                            firstMatchIndex = iFirst;
                        }
                        break;
                    }
                case Operation.Operator.Equal:
                    {
                        long iFirstGreaterEqual = LowerBound(val2, comparator);
                        if (iFirstGreaterEqual < sortedIndex.Length)
                        {
                            long index = sortedIndex[iFirstGreaterEqual];
                            var val1 = GetRowValue(index);
                            int comparisonResult = comparator(val1, val2);
                            if (comparisonResult == 0)
                            {
                                firstMatchIndex = index;
                            }
                        }
                        break;
                    }
                case Operation.Operator.GreaterEqual:
                    {
                        long iFirstGreaterEqual = LowerBound(val2, comparator);
                        if (iFirstGreaterEqual < sortedIndex.Length)
                        {
                            firstMatchIndex = sortedIndex[iFirstGreaterEqual];
                        }
                        break;
                    }
                case Operation.Operator.Greater:
                    {
                        long iFirstGreater = UpperBound(val2, comparator);
                        if (iFirstGreater < sortedIndex.Length)
                        {
                            firstMatchIndex = sortedIndex[iFirstGreater];
                        }
                        break;
                    }
            }
            return firstMatchIndex;
        }

        public override int Compare(long rowLhs, Operation.Expression expression, long rowRhs)
        {
            Operation.Operation.ComparableComparator comparator = Operation.Operation.GetComparator(type, expression.type);
            var val1 = GetRowValue(rowLhs);
            var val2 = expression.GetComparableValue(rowRhs);
            int result = comparator(val1, val2);
            return result;
        }
    }
}
