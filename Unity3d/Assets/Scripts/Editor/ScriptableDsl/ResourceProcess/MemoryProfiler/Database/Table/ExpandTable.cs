using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    internal abstract class ExpandTable : Table
    {
        public class ExpandedGroup
        {
            public Table table;
        }
        public struct DataEntry
        {
            public long groupIndex;
            public long subGroupIndex;// <0 when is group head
            public bool isGroupHead() { return subGroupIndex < 0; }
        }
        public class Data
        {
            public System.Collections.Generic.SortedDictionary<long, ExpandedGroup> m_ExpandedGroup = new System.Collections.Generic.SortedDictionary<long, ExpandedGroup>();
            public Range[] m_GroupRowDataRange;
            public DataEntry[] m_RowData;
        }
        Data m_Data;
        public DataEntry[] m_RowData { get { return m_Data.m_RowData; } }
        public Range[] m_GroupRowDataRange { get { return m_Data.m_GroupRowDataRange; } }
        public System.Collections.Generic.SortedDictionary<long, ExpandedGroup> m_ExpandedGroup { get { return m_Data.m_ExpandedGroup; } }

        public ExpandTable(Schema schema)
            : base(schema)
        {
        }

        protected void InitGroup(long groupCount)
        {
            m_Data = new Data();
            m_Data.m_GroupRowDataRange = new Range[groupCount];

            ResetAllGroup();
        }

        protected bool IsGroupInitialized()
        {
            return m_Data != null;
        }

        public long GetGroupCount()
        {
            if (m_Data == null) return -1;
            return m_Data.m_GroupRowDataRange.LongLength;
        }

        public void ResetAllGroup()
        {
            if (m_Data == null) return;
            m_ExpandedGroup.Clear();
            //create data entries with group heads only
            m_Data.m_RowData = new DataEntry[m_GroupRowDataRange.Length];
            for (long i = 0; i != m_GroupRowDataRange.Length; ++i)
            {
                m_RowData[i].groupIndex = i;
                m_RowData[i].subGroupIndex = -1;
                m_GroupRowDataRange[i] = Range.FirstPlusLength(i, 1);
            }
        }

        protected void InitExpandColumn(List<Column> baseColumn)
        {
            //create columns
            m_Columns = new List<Column>(m_Meta.GetColumnCount());
            for (int i = 0; i != m_Meta.GetColumnCount(); ++i)
            {
                var metaCol = m_Meta.GetColumnByIndex(i);
                IExpandColumn newCol = (IExpandColumn)Database.Operation.ColumnCreator.CreateColumn(typeof(ExpandColumnTyped<>), metaCol.Type);

                newCol.Initialize(this, baseColumn[i], i);
                m_Columns.Add((Column)newCol);
            }
        }

        public void SetColumn(MetaColumn metaColumn, Column newColumn)
        {
            //add missing columns if the meta column index does not fit in table's column list
            int missingColumnCount = metaColumn.Index - m_Columns.Count + 1;
            if (missingColumnCount > 0)
            {
                m_Columns.AddRange(Enumerable.Range(0, missingColumnCount).Select(x => (Column)null));
            }
            if (metaColumn.Type != null)
            {
                IExpandColumn newExpandColumn = (IExpandColumn)Database.Operation.ColumnCreator.CreateColumn(typeof(ExpandColumnTyped<>), metaColumn.Type);
                newExpandColumn.Initialize(this, newColumn, metaColumn.Index);
                m_Columns[metaColumn.Index] = (Column)newExpandColumn;
            }
            else
            {
                m_Columns[metaColumn.Index] = new ColumnError(this);
                Debug.LogError("Cannot create column '" + GetName() + "." + metaColumn.Name + "'. Type is unknown (null)");
            }
        }

        public override long GetRowCount()
        {
            if (m_Data == null) return -1;
            return m_RowData.Length;
        }

        public struct Entry
        {
            public Table table;
            public long row;
            public long groupIndex;
        }
        public Entry GetEntryAt(long row)
        {
            Entry e = new Entry();
            if (m_RowData[row].isGroupHead())
            {
                e.table = this;
                e.groupIndex = m_RowData[row].groupIndex;
                e.row = row;
                return e;
            }
            var groupIndex = m_RowData[row].groupIndex;
            ExpandedGroup eg;
            if (m_ExpandedGroup.TryGetValue(groupIndex, out eg))
            {
                if (eg.table is ExpandTable)
                {
                    ExpandTable egSub = (ExpandTable)eg.table;
                    return egSub.GetEntryAt(m_RowData[row].subGroupIndex);
                }
                else
                {
                    e.table = eg.table;
                    e.groupIndex = m_RowData[row].subGroupIndex;
                    e.row = m_RowData[row].subGroupIndex;
                    return e;
                }
            }
            return e;
        }

        public Table GetGroupSubTable(long index)
        {
            ExpandedGroup eg = null;
            if (m_ExpandedGroup.TryGetValue(index, out eg))
            {
                return eg.table;
            }
            return null;
        }

        private Range FixGroupSubRow(long index, Range subRange)
        {
            if (subRange.Length < 0)
            {
                return RemoveGroupSubRow(index, subRange);
            }
            else
            {
                return InsertGroupSubRow(index, subRange);
            }
        }

        private Range InsertGroupSubRow(long index, Range subRange)
        {
            //convert subrange to level range
            long groupStart = m_GroupRowDataRange[index].First + 1;
            m_GroupRowDataRange[index].Last += subRange.Length;
            Range range = Range.FirstPlusLength(subRange.First + groupStart, subRange.Length);


            DataEntry[] newData = new DataEntry[m_RowData.Length + range.Length];

            //copy before gap
            System.Array.Copy(m_RowData, newData, range.First);

            //copy after gap
            if (range.First < m_RowData.Length)
            {
                long afterGapLength = m_RowData.Length - range.First;
                System.Array.Copy(m_RowData, range.First, newData, range.Last, afterGapLength);
            }

            //offset trailing goups' dataEntryIndexFirst
            for (long i = index + 1; i < m_GroupRowDataRange.Length; ++i)
            {
                m_GroupRowDataRange[i] += range.Length;
            }

            //fill the gap with new data
            for (long i = 0; i < range.Length; ++i)
            {
                newData[range.First + i].groupIndex = index;
                newData[range.First + i].subGroupIndex = i + subRange.First;
            }
            //update trailing subGroupIndex
            long traillingCount = m_GroupRowDataRange[index].Last - range.Last;
            for (long i = 0; i < traillingCount; ++i)
            {
                newData[range.Last + i].subGroupIndex = subRange.Last + i;
            }

            m_Data.m_RowData = newData;
            return range;
        }

        //subrange length must be negative and .end is > .start
        private Range RemoveGroupSubRow(long index, Range subRange)
        {
            long offset = subRange.Length;//length is negative
            //convert subrange to level range
            long groupStart = m_GroupRowDataRange[index].First + 1;
            Range range = Range.FirstPlusLength(subRange.Last + groupStart, -subRange.Length);


            DataEntry[] newData = new DataEntry[m_RowData.Length - range.Length];
            //copy before gap
            System.Array.Copy(m_RowData, newData, range.First);

            //copy after gap
            long rangeEnd = range.Last;
            if (rangeEnd < m_RowData.Length)
            {
                long afterGapLength = m_RowData.Length - rangeEnd;
                System.Array.Copy(m_RowData, rangeEnd, newData, range.First, afterGapLength);
            }

            //update trailing subGroupIndex. this happens when subrange does not include all the group last sub rows.
            long trailingSubIndexCount = m_GroupRowDataRange[index].Last - range.Last;

            for (long i = 0; i < trailingSubIndexCount; ++i)
            {
                newData[range.First + i].subGroupIndex += offset;
            }


            //offset trailing goups' dataEntryIndexFirst
            for (long i = index + 1; i < m_GroupRowDataRange.Length; ++i)
            {
                m_GroupRowDataRange[i] += offset;
            }

            m_GroupRowDataRange[index].Last += subRange.Length;

            m_Data.m_RowData = newData;

            //inverse range returned to mark it as removed
            return Range.FirstPlusLength(range.First + range.Length, -range.Length);
        }

        private Range ExpandGroupByIndex(long index, bool expand)
        {
            if (expand)
            {
                if (!m_ExpandedGroup.ContainsKey(index))
                {
                    var table = CreateGroupTable(index);
                    ExpandedGroup eg = new ExpandedGroup();
                    eg.table = table;
                    m_ExpandedGroup[index] = eg;
                    return InsertGroupSubRow(index, Range.FirstToLast(0, table.GetRowCount()));
                }
                return Range.None;
            }
            else
            {
                ExpandedGroup eg = null;
                if (m_ExpandedGroup.TryGetValue(index, out eg))
                {
                    m_ExpandedGroup.Remove(index);
                    long rowCount = eg.table.GetRowCount();
                    return RemoveGroupSubRow(index, Range.FirstPlusLength(rowCount, -rowCount));
                }
                return Range.None;
            }
        }

        public override ExpandableCellState GetCellExpandState(long row, int col)
        {
            long groupIndex = m_RowData[row].groupIndex;
            if (m_RowData[row].isGroupHead())
            {
                //is a group head row
                ExpandableCellState cellStateOut;
                cellStateOut.isRowExpandable = true;
                cellStateOut.isColumnExpandable = IsColumnExpandable(col);
                cellStateOut.expandDepth = 0;
                cellStateOut.isExpandable = IsGroupExpandable(groupIndex, col);
                cellStateOut.isExpanded = m_ExpandedGroup.ContainsKey(groupIndex);
                return cellStateOut;
            }
            else
            {
                //is a sub row
                long subRow = row - (m_GroupRowDataRange[groupIndex].First + 1);
                ExpandedGroup eg;
                if (m_ExpandedGroup.TryGetValue(groupIndex, out eg))
                {
                    var cellExpandStateOut = eg.table.GetCellExpandState(subRow, col);
                    cellExpandStateOut.expandDepth += 1;
                    cellExpandStateOut.isColumnExpandable |= IsColumnExpandable(col);
                    return cellExpandStateOut;
                }
                return new ExpandableCellState();
            }
        }

        public override Range ExpandCell(long row, int col, bool expanded)
        {
            long groupIndex = m_RowData[row].groupIndex;
            if (m_RowData[row].isGroupHead())
            {
                return ExpandGroupByIndex(groupIndex, expanded);
            }
            else
            {
                ExpandedGroup eg;
                if (m_ExpandedGroup.TryGetValue(groupIndex, out eg))
                {
                    long subRow = row - (m_GroupRowDataRange[groupIndex].First + 1);
                    var subDirtyRange = eg.table.ExpandCell(subRow, col, expanded);
                    return FixGroupSubRow(groupIndex, subDirtyRange);
                }
                return Range.None;
            }
        }

        class LinkOpenGroup : CellLink
        {
            public long groupIndex;
            public CellLink subLink;
            public override CellPosition Apply(Table t)
            {
                if (t is ExpandTable)
                {
                    ExpandTable gt = (ExpandTable)t;
                    gt.ExpandGroupByIndex(groupIndex, true);
                    if (subLink != null)
                    {
                        ExpandedGroup eg;
                        if (gt.m_ExpandedGroup.TryGetValue(groupIndex, out eg))
                        {
                            var subCellPos = subLink.Apply(eg.table);
                            long row = gt.m_GroupRowDataRange[groupIndex].First + 1 + subCellPos.row; //+1 to skip the group head
                            return new CellPosition(row, subCellPos.col);
                        }
                    }
                }
                return CellPosition.invalid;
            }

            public override string ToString()
            {
                if (subLink != null)
                {
                    return "Group(" + groupIndex + ", " + subLink.ToString() + ")";
                }
                return "Group(" + groupIndex + ")";
            }
        }
        public override Database.CellLink GetLinkTo(CellPosition pos)
        {
            if (pos.row < GetRowCount())
            {
                if (m_RowData[pos.row].isGroupHead())
                {
                    //add goto group index
                    LinkPosition lc = new LinkPosition(pos);
                    return lc;
                }
                else
                {
                    //add expand group index
                    var groupIndex = m_RowData[pos.row].groupIndex;
                    LinkOpenGroup l = new LinkOpenGroup();
                    l.groupIndex = groupIndex;

                    ExpandedGroup eg;
                    if (m_ExpandedGroup.TryGetValue(groupIndex, out eg))
                    {
                        var subPos = new CellPosition(m_RowData[pos.row].subGroupIndex, pos.col);
                        l.subLink = eg.table.GetLinkTo(subPos);
                    }
                    return l;
                }
            }
            return null;
        }

        public override bool Update()
        {
            var r = BeginUpdate();
            bool dirty = r != null;
            EndUpdate(r);
            return dirty;
        }

        protected class Updater : IUpdater
        {
            public IUpdater m_DataUpdater;
            public Data m_NewData = new Data();
            public System.Collections.Generic.SortedDictionary<long, IUpdater> m_NewExpandedGroupUpdater = new System.Collections.Generic.SortedDictionary<long, IUpdater>();
            public long[] m_OldToNewRow;
            long IUpdater.OldToNewRow(long a)
            {
                if (a < 0) return -1;
                return m_OldToNewRow[a];
            }

            long IUpdater.GetRowCount()
            {
                return m_NewData.m_RowData.LongLength;
            }
        }

        //dataUpdater is the update for the list of all group (no subgroup data)
        //returns an updater that includes subgroup data for group that were open before that are still present after the update
        protected Updater UpdateDataBegin(IUpdater dataUpdater)
        {
            if (dataUpdater == null && m_Data.m_ExpandedGroup.Count == 0)
            {
                //no change in data
                return null;
            }
            Updater u = new Updater();
            u.m_DataUpdater = dataUpdater;
            long newGroupCount = 0;
            if (dataUpdater != null)
            {
                newGroupCount = dataUpdater.GetRowCount();
                //convert open groups' row
                foreach (var e in m_Data.m_ExpandedGroup)
                {
                    var newRow = dataUpdater.OldToNewRow(e.Key);
                    if (newRow >= 0)
                    {
                        u.m_NewData.m_ExpandedGroup[newRow] = e.Value;
                    }
                }
            }
            else
            {
                newGroupCount = m_Data.m_GroupRowDataRange.LongLength;
                u.m_NewData.m_ExpandedGroup = m_Data.m_ExpandedGroup;
            }

            //update all expanded groups' table
            u.m_NewData.m_GroupRowDataRange = new Range[newGroupCount];
            long newCurrentRow = 0;
            long newCurrentGroup = 0;
            var newExpandedGroupEnum = u.m_NewData.m_ExpandedGroup.GetEnumerator();
            bool hasExpandedGroup = newExpandedGroupEnum.MoveNext();
            List<DataEntry> newRowDataList = new List<DataEntry>();
            while (newCurrentGroup < newGroupCount)
            {
                //add group to row data
                var deGroupHead = new DataEntry();
                deGroupHead.groupIndex = newCurrentGroup;
                deGroupHead.subGroupIndex = -1; // < 0 meaning the group head row
                newRowDataList.Add(deGroupHead);


                //check if this group is expanded
                if (hasExpandedGroup && newCurrentGroup == newExpandedGroupEnum.Current.Key)
                {
                    //update its sub table
                    var subGroupUpdater = newExpandedGroupEnum.Current.Value.table.BeginUpdate();
                    u.m_NewExpandedGroupUpdater[newCurrentGroup] = subGroupUpdater;
                    long subRowCount;
                    if (subGroupUpdater != null)
                    {
                        subRowCount = subGroupUpdater.GetRowCount();
                    }
                    else
                    {
                        subRowCount = newExpandedGroupEnum.Current.Value.table.GetRowCount();
                    }

                    //add subgroup rows into the row data
                    for (int i = 0; i != subRowCount; ++i)
                    {
                        var deSubRow = new DataEntry();
                        deSubRow.groupIndex = newCurrentGroup;
                        deSubRow.subGroupIndex = i;
                        newRowDataList.Add(deSubRow);
                    }

                    //set group data range to include head + sub rows
                    u.m_NewData.m_GroupRowDataRange[newCurrentGroup] = Range.FirstPlusLength(newCurrentRow, 1 + subRowCount);
                    newCurrentRow += subRowCount + 1;
                    hasExpandedGroup = newExpandedGroupEnum.MoveNext();
                }
                else
                {
                    //group use only a single row
                    u.m_NewData.m_GroupRowDataRange[newCurrentGroup] = Range.FirstPlusLength(newCurrentRow, 1);
                    ++newCurrentRow;
                }

                ++newCurrentGroup;
            }
            u.m_NewData.m_RowData = newRowDataList.ToArray();


            //compute old to new row conversion table
            var oldExpandedGroupEnum = m_Data.m_ExpandedGroup.GetEnumerator();
            hasExpandedGroup = oldExpandedGroupEnum.MoveNext();
            u.m_OldToNewRow = new long[m_Data.m_RowData.Length];
            newCurrentRow = 0;
            long oldCurrentRow = 0;
            //for (long i = 0; i < u.m_OldToNewRow.Length; ++i)
            while (oldCurrentRow < u.m_OldToNewRow.Length)
            {
                var oldRowGroup = m_Data.m_RowData[oldCurrentRow].groupIndex;
                long newRowGroup = -1;
                if (dataUpdater != null)
                {
                    newRowGroup = dataUpdater.OldToNewRow(oldRowGroup);
                }
                else
                {
                    newRowGroup = oldRowGroup;
                }
                if (newRowGroup >= 0)
                {
                    var newGroupHeadRow = u.m_NewData.m_GroupRowDataRange[newRowGroup].First;
                    u.m_OldToNewRow[oldCurrentRow] = newGroupHeadRow;
                    //check if this group was expanded
                    if (hasExpandedGroup && oldRowGroup == oldExpandedGroupEnum.Current.Key)
                    {
                        IUpdater subUpdater;
                        if (u.m_NewExpandedGroupUpdater.TryGetValue(newRowGroup, out subUpdater))
                        {
                            //fix all sub rows
                            if (subUpdater != null)
                            {
                                //sub table is dirty, update each sub row with the group's updater
                                for (int j = 0; j < m_Data.m_GroupRowDataRange[oldRowGroup].Length - 1; ++j)
                                {
                                    var subRowTopRow = oldCurrentRow + j + 1;//sub row index in the m_RowData array
                                    long newSubRow = subUpdater.OldToNewRow(j);
                                    if (newSubRow >= 0)
                                    {
                                        //set to new head row + new sub row
                                        u.m_OldToNewRow[subRowTopRow] = newGroupHeadRow + 1 + newSubRow;
                                    }
                                    else
                                    {
                                        //row no longer exist in sub table
                                        u.m_OldToNewRow[subRowTopRow] = -1;
                                    }
                                }
                            }
                            else
                            {
                                //sub table didn't change
                                for (int j = 0; j != m_Data.m_GroupRowDataRange[oldCurrentRow].Length - 1; ++j)
                                {
                                    var subRowTopRow = oldCurrentRow + 1 + j;//sub row index in the m_RowData array
                                    //set to new head row + new sub row
                                    u.m_OldToNewRow[subRowTopRow] = newGroupHeadRow + 1 + j;
                                }
                            }
                        }
                        hasExpandedGroup = oldExpandedGroupEnum.MoveNext();
                        //offset oldCurrentRow by the sub row we just fixed
                        oldCurrentRow += m_Data.m_GroupRowDataRange[oldCurrentRow].Length - 1;//-1 to exclude the group head
                    }
                }
                else
                {
                    //the group no longer exist
                    u.m_OldToNewRow[oldCurrentRow] = -1;
                }
                ++oldCurrentRow;
            }
            return u;
        }

        protected void UpdateDataEnd(IUpdater aUpdater)
        {
            if (aUpdater is Updater)
            {
                var updater = (Updater)aUpdater;
                foreach (var g in updater.m_NewExpandedGroupUpdater)
                {
                    var table = updater.m_NewData.m_ExpandedGroup[g.Key].table;
                    table.EndUpdate(g.Value);
                }
                m_Data = updater.m_NewData;
            }
        }

        protected class GroupUpdater : IUpdater
        {
            public ExpandTable m_Table;
            public GroupUpdater(ExpandTable t)
            {
                m_Table = t;
            }

            long IUpdater.OldToNewRow(long a)
            {
                return a;
            }

            long IUpdater.GetRowCount()
            {
                return m_Table.GetGroupCount();
            }
        }
        public override IUpdater BeginUpdate()
        {
            bool r = UpdateColumns();
            var updater = UpdateDataBegin(new GroupUpdater(this));
            if (updater == null && r)
            {
                return new DefaultDirtyUpdater(this);
            }
            return updater;
        }

        public override void EndUpdate(IUpdater updater)
        {
            UpdateDataEnd(updater);
        }

        public abstract Table CreateGroupTable(long groupIndex);
        public abstract bool IsGroupExpandable(long groupIndex, int col);
        public abstract bool IsColumnExpandable(int col);
    }
}
