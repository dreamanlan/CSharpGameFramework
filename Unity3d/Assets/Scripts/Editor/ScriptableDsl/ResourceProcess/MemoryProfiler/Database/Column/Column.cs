using System;
using Unity.MemoryProfilerForExtension.Editor.Database.Operation;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    /// <summary>
    /// Represents a column in a table having a set of values indexed sequentially using a `row` parameter
    /// </summary>
    internal abstract class Column
    {
        public Type type;

        /// <summary>
        /// Tests if the column is correctly setup. useful for debug and tests
        /// </summary>
        /// <param name="log">
        /// true: will log any errors or warnings
        /// false: will only output if the table is valid
        /// </param>
        /// <param name="metaColumn">
        /// MetaColumn this column was created with
        /// </param>
        /// <returns>
        /// true: the table can be used. Call update before accessing the data
        /// false: the table is not correctly setup
        /// </returns>
        public virtual bool Validate(bool log, MetaColumn metaColumn)
        {
            bool valid = true;
            if (type != metaColumn.Type)
            {
                valid = false;
                Debug.Log("Column must have the same value type as its MetaColumn");
            }
            return valid;
        }

        public abstract long[] GetSortIndex(SortOrder order, ArrayRange indices, bool relativeIndex);

        // equality is an array the same size as the returned index. the value is the index that it's equal to.
        // so that R[x] == R[equality[x]]
        // If equality[x] == x then it is unique
        // if equality == null then all entries are considered equal
        public virtual long[] GetSortIndexAndEquality(SortOrder order, ArrayRange indices, bool relativeIndex, out long[] equality)
        {
            equality = null;
            return GetSortIndex(order, indices, relativeIndex);
        }

        //call this to get a displayable value
        public abstract string GetRowValueString(long row, IDataFormatter formatter);
        public abstract int CompareRow(long rowA, long rowB);
        public abstract int Compare(long rowLhs, Operation.Expression exp, long rowRhs);
        //returning indice array is always in ascending index order
        public abstract long[] GetMatchIndex(ArrayRange rowRange, Operation.Operator op, Operation.Expression exp, long expRowFirst, bool rowToRow);
        public abstract long[] GetMatchIndex(ArrayRange indices, Operation.Matcher matcher);
        public abstract long GetFirstMatchIndex(Operation.Operator op, Operation.Expression exp, long expRowFirst);

        public virtual LinkRequest GetRowLink(long row) { return null; }

        // will return -1 if the underlying data has not been computed yet.
        // ComputeRowCount or Update should be called at least once before getting accurate row count
        public abstract long GetRowCount();

        // Update is provided to offset the load of computing the table's data outside the table's construction
        // return if anything changed
        public virtual bool Update() { return false; }

        // ComputeRowCount is provided to offset the load of computing the table's data outside the table's construction
        // return if new row count was computed
        public virtual bool ComputeRowCount() { return false; }
    }

    internal interface IColumnDecorator
    {
        Column GetBaseColumn();
    }
}
