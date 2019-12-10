using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal class IndexedTable : Table
    {
        public Table m_SourceTable;
        public long[] indices;
        public override long GetRowCount()
        {
            return indices.Length;
        }

        public IndexedTable(Database.Table sourceTable)
            : base(sourceTable.Schema)
        {
            m_SourceTable = sourceTable;
            m_Meta = m_SourceTable.GetMetaData();
            CreateColumn();
        }

        public IndexedTable(Database.Table sourceTable, ArrayRange indices)
            : base(sourceTable.Schema)
        {
            this.indices = indices.ToArray();
            m_SourceTable = sourceTable;
            m_Meta = m_SourceTable.GetMetaData();
            CreateColumn();
        }

        protected IndexedTable(Schema schema)
            : base(schema)
        {
        }

        protected void CreateColumn()
        {
            m_Columns = new System.Collections.Generic.List<Column>(m_Meta.GetColumnCount());
            for (int i = 0; i != m_Meta.GetColumnCount(); ++i)
            {
                var metaCol = m_Meta.GetColumnByIndex(i);
                IIndexedColumn newCol = (IIndexedColumn)ColumnCreator.CreateColumn(typeof(IndexedColumnTyped<>), metaCol.Type);

                newCol.Initialize(this, m_SourceTable.GetColumnByIndex(i));
                m_Columns.Add((Column)newCol);
            }
        }

        public override ExpandableCellState GetCellExpandState(long row, int col)
        {
            return m_SourceTable.GetCellExpandState(indices[row], col);
        }

        //return the range or new/deleted row. length is positive for added, negative for removed
        public override Range ExpandCell(long row, int col, bool expanded)
        {
            long subRow = indices[row];
            var dirtyRange = m_SourceTable.ExpandCell(subRow, col, expanded);
            //if( dirtyRange.length > 0)
            //{
            //new rows inserted
            long newCount = indices.Length + dirtyRange.Length;
            long[] newIndices = new long[newCount];

            //find in our index where the created gap corespond to
            long gapFirst = 0;
            if (dirtyRange.First > 0)
            {
                gapFirst = System.Array.IndexOf(indices, dirtyRange.First - 1) + 1;
            }
            long gapLast = gapFirst + dirtyRange.Length;

            long gapMin = gapFirst;
            if (gapLast < gapMin) gapMin = gapLast;
            //fixup indices before gap
            for (long i = 0; i < gapMin; ++i)
            {
                long offset = 0;
                if (indices[i] >= dirtyRange.First)
                {
                    //index was after gap, need offset
                    offset = dirtyRange.Length;
                }
                newIndices[i] = indices[i] + offset;
            }

            //fill in the gap
            for (long i = 0; i < dirtyRange.Length; ++i)
            {
                newIndices[gapFirst + i] = dirtyRange.First + i;
            }

            //offset and fixup following indices
            long remainingIndices = newCount - gapLast;
            for (long i = 0; i < remainingIndices; ++i)
            {
                //gapFirst + dirtyRange.length
                long offset = 0;
                if (indices[gapFirst + i] >= dirtyRange.First)
                {
                    offset = dirtyRange.Length;
                }
                newIndices[gapLast + i] = indices[gapFirst + i] + offset;
            }
            indices = newIndices;
            return Range.FirstToLast(gapFirst, gapLast);
        }

        class LinkIndex : CellLink
        {
            public CellLink subLink;
            public override CellPosition Apply(Table t)
            {
                if (t is IndexedTable)
                {
                    IndexedTable it = (IndexedTable)t;
                    var pos = subLink.Apply(it.m_SourceTable);
                    var index = System.Array.FindIndex(it.indices, x => x == pos.row);
                    return new CellPosition(index, pos.col);
                }
                return CellPosition.invalid;
            }

            public override string ToString()
            {
                if (subLink != null)
                {
                    return "Index(" + subLink.ToString() + ")";
                }
                return "Index()";
            }
        }
        public override Database.CellLink GetLinkTo(CellPosition pos)
        {
            if (indices.Length == 0 || pos.row > indices.Length)
                return null;
            LinkIndex li = new LinkIndex();
            var i = indices[pos.row];
            li.subLink = m_SourceTable.GetLinkTo(new CellPosition(i, pos.col));

            return li;
        }

        protected class Updater : IUpdater
        {
            public IUpdater m_SourceTableUpdater;
            public IndexedTable m_Table;
            public long[] m_OldToNewRow;
            public long[] m_NewIndices;

            long IUpdater.OldToNewRow(long a)
            {
                if (m_OldToNewRow == null) return a;
                if (a < 0 || a >= m_OldToNewRow.LongLength) return -1;
                return m_OldToNewRow[a];
            }

            long IUpdater.GetRowCount()
            {
                return m_NewIndices.LongLength;
            }
        }
        public override IUpdater BeginUpdate()
        {
            var updater = new Updater();
            updater.m_SourceTableUpdater = m_SourceTable.BeginUpdate();
            updater.m_Table = this;
            if (updater.m_SourceTableUpdater != null)
            {
                var newIndices = new List<long>();
                var oldToNewRow = new List<long>();
                for (int i = 0; i != indices.Length; ++i)
                {
                    var newTarget = updater.m_SourceTableUpdater.OldToNewRow(indices[i]);
                    if (newTarget >= 0)
                    {
                        oldToNewRow.Add(newIndices.Count);
                        newIndices.Add(newTarget);
                    }
                    else
                    {
                        oldToNewRow.Add(-1);
                    }
                }
                updater.m_OldToNewRow = oldToNewRow.ToArray();
                updater.m_NewIndices = newIndices.ToArray();
            }
            else
            {
                updater.m_NewIndices = indices;
            }
            return updater;
        }

        public override void EndUpdate(IUpdater updater)
        {
            if (updater is Updater)
            {
                var u = (Updater)updater;
                indices = u.m_NewIndices;
            }
        }
    }

    internal class SortedTable : IndexedTable
    {
        public int[] m_SortColumn;
        public SortOrder[] m_SortOrder;
        public int m_ColumnIndexFirst; //index into m_SortColumn
        public SortedTable(Database.Table table, int[] sortColumn, SortOrder[] sortOrder, int columnIndexFirst, ArrayRange indices)
            : base(table.Schema)
        {
            m_SourceTable = table;
            m_Meta = m_SourceTable.GetMetaData();
            m_SortColumn = sortColumn;
            m_SortOrder = sortOrder;
            m_ColumnIndexFirst = columnIndexFirst;

            this.indices = SortRange(indices, sortColumn, sortOrder, m_ColumnIndexFirst);

            //create columns
            CreateColumn();
        }

        private long[] SortRange(ArrayRange indices, int[] colToSort, SortOrder[] order, int depth)
        {
            var col = m_SourceTable.GetColumnByIndex(colToSort[depth]);
            var sortedIndices = col.GetSortIndex(order[depth], indices, false);
            ++depth;
            if (depth < colToSort.Length)
            {
                long iGroupFirst = 0;
                for (long i = 1, j = 0; i < sortedIndices.Length; j = i++)
                {
                    if (col.CompareRow(sortedIndices[j], sortedIndices[i]) != 0)
                    {
                        if (i - iGroupFirst > 1)
                        {
                            //sort sub range
                            long[] subIndices = SortRange(new ArrayRange(sortedIndices, iGroupFirst, i), colToSort, order, depth);
                            //copy sorted sub range
                            System.Array.Copy(subIndices, 0, sortedIndices, iGroupFirst, i - iGroupFirst);
                        }
                        iGroupFirst = i;
                    }
                }

                //sort last sub range
                long[] subIndicesLast = SortRange(new ArrayRange(sortedIndices, iGroupFirst, sortedIndices.Length), colToSort, order, depth);
                //copy last sorted sub range
                System.Array.Copy(subIndicesLast, 0, sortedIndices, iGroupFirst, sortedIndices.Length - iGroupFirst);
            }

            return sortedIndices;
        }
    }
}
