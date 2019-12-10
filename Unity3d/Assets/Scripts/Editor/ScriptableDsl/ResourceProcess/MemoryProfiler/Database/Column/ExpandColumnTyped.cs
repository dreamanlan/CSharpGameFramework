using System;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    /// <summary>
    /// A column which entries may expand or collapse into/from several rows
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpandColumnTyped<DataT> : ColumnTyped<DataT>, IExpandColumn, IColumnDecorator where DataT : System.IComparable
    {
        protected ExpandTable m_Table;
        protected ColumnTyped<DataT> m_Column;
        protected int m_ColumnIndex;


        public ExpandColumnTyped()
        {
            type = typeof(DataT);
        }

        Column IColumnDecorator.GetBaseColumn()
        {
            return m_Column;
        }

        public void Initialize(ExpandTable table, Column column, int columnIndex)
        {
            m_Table = table;
            m_Column = (ColumnTyped<DataT>)column;
            m_ColumnIndex = columnIndex;
        }

        public override bool Update()
        {
            bool changed = base.Update();
            if (m_Column != null) changed |= m_Column.Update();
            return changed;
        }

        public override long GetRowCount()
        {
            return m_Table.GetRowCount();
        }

        public override LinkRequest GetRowLink(long row)
        {
            var tableEntry = m_Table.m_RowData[row];
            if (tableEntry.isGroupHead())
            {
                if (m_Column == null) return null;
                return m_Column.GetRowLink(tableEntry.groupIndex);
            }
            else
            {
                var subTable = m_Table.GetGroupSubTable(tableEntry.groupIndex);
                if (subTable != null)
                {
                    return subTable.GetCellLink(new CellPosition(tableEntry.subGroupIndex, m_ColumnIndex));
                }
            }
            return null;
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            var e = m_Table.m_RowData[row];
            if (e.isGroupHead())
            {
                if (m_Column == null) return (string)Convert.ChangeType(default(DataT), typeof(string));
                return m_Column.GetRowValueString(e.groupIndex, formatter);
            }
            else
            {
                var subTable = m_Table.GetGroupSubTable(e.groupIndex);
                if (subTable != null)
                {
                    var col = subTable.GetColumnByIndex(m_ColumnIndex);
                    return col.GetRowValueString(e.subGroupIndex, formatter);
                }
            }
            return "";
        }

        public override DataT GetRowValue(long row)
        {
            var e = m_Table.m_RowData[row];
            if (e.isGroupHead())
            {
                if (m_Column == null) return default(DataT);
                return m_Column.GetRowValue(e.groupIndex);
            }
            else
            {
                var subTable = m_Table.GetGroupSubTable(e.groupIndex);
                if (subTable != null)
                {
                    var col = subTable.GetColumnByIndex(m_ColumnIndex);
                    if (col is ColumnTyped<DataT>)
                    {
                        ColumnTyped<DataT> ct = (ColumnTyped<DataT>)col;
                        return ct.GetRowValue(e.subGroupIndex);
                    }
                    else
                    {
                        throw new Exception("Expand column type mismatch");
                    }
                }
            }
            return default(DataT);
        }

        public override void SetRowValue(long row, DataT value)
        {
            var e = m_Table.m_RowData[row];
            if (e.isGroupHead())
            {
                if (m_Column == null) return;
                m_Column.SetRowValue(e.groupIndex, value);
            }
            else
            {
                var subTable = m_Table.GetGroupSubTable(e.groupIndex);
                if (subTable != null)
                {
                    var col = subTable.GetColumnByIndex(m_ColumnIndex);
                    if (col is ColumnTyped<DataT>)
                    {
                        ColumnTyped<DataT> ct = (ColumnTyped<DataT>)col;
                        ct.SetRowValue(e.subGroupIndex, value);
                    }
                    else
                    {
                        throw new Exception("Expand column type mismatch");
                    }
                }
            }
        }

        public override long[] GetMatchIndex(ArrayRange indices, Operation.Matcher matcher)
        {
            return GetMatchIndex(indices, matcher, new Operation.ExpColumn<DataT>(m_Column));
        }

        protected long[] GetMatchIndex(ArrayRange indices, Operation.Matcher matcher, Operation.Expression exp)
        {
            //translate group matches to row matches
            if (m_Table.m_RowData.Length == m_Table.m_GroupRowDataRange.Length)
            {
                //all group are closed, group matches are == to row matches
                return matcher.GetMatchIndex(exp, indices);
            }
            else
            {
                //some group are expanded
                bool matchAllData = indices.IsSequence && indices.Count == m_Table.m_RowData.Length;
                long[] groupMatches;
                if (matchAllData)
                {
                    //when asking to test all data, test all group
                    groupMatches = matcher.GetMatchIndex(exp, new ArrayRange(0, m_Column.GetRowCount()));
                }
                else
                {
                    //when asking to test only a subset of the data, test only the groups that fall in the indices range
                    var l = new System.Collections.Generic.List<long>((int)indices.Count);
                    for (int i = 0; i != indices.Count; ++i)
                    {
                        var row = indices[i];
                        if (m_Table.m_RowData[row].isGroupHead())
                        {
                            l.Add(m_Table.m_RowData[row].groupIndex);
                        }
                    }

                    groupMatches = matcher.GetMatchIndex(exp, new ArrayRange(l.ToArray()));
                }

                // translate from a list of group index to a list of index including the group's subdata
                var matches = new System.Collections.Generic.List<long>(m_Table.m_RowData.Length);
                for (int i = 0; i != groupMatches.Length; ++i)
                {
                    var groupIndex = groupMatches[i];
                    var groupRange = m_Table.m_GroupRowDataRange[groupIndex];
                    for (long j = groupRange.First; j != groupRange.Last; ++j)
                    {
                        matches.Add(j);
                    }
                }
                return matches.ToArray();
            }
        }
    }
}
