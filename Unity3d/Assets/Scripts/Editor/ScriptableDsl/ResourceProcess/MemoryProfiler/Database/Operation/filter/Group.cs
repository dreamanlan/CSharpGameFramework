using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation.Filter
{
    /// <summary>
    /// Base filter class for creating grouped table
    /// it will apply a sub filter for entries upon expansion of a group
    /// </summary>
    internal abstract class Group : Filter
    {
        protected readonly SortOrder m_Order;//default ordering
        public Filter SubGroupFilter;
        public Group(SortOrder order)
        {
            this.m_Order = order;
        }

        public abstract int GetColumnIndex(Database.Table tableIn);
        public abstract string GetColumnName(Database.Table tableIn);
        public override Database.Table CreateFilter(Database.Table tableIn)
        {
            Database.Operation.GroupedTable tableOut = new Database.Operation.GroupedTable(tableIn, new ArrayRange(0, tableIn.GetRowCount()), GetColumnIndex(tableIn), m_Order, CreateGroupTable);
            return tableOut;
        }

        public override Database.Table CreateFilter(Database.Table tableIn, ArrayRange range)
        {
            Database.Operation.GroupedTable tableOut = new Database.Operation.GroupedTable(tableIn, range, GetColumnIndex(tableIn), m_Order, CreateGroupTable);
            return tableOut;
        }

        protected Database.Table CreateGroupTable(Database.Operation.GroupedTable table, Database.Operation.GroupedTable.Group g, long groupIndex)
        {
            if (SubGroupFilter != null)
            {
                return SubGroupFilter.CreateFilter(table.m_table, g.m_GroupIndice);
            }
            Database.Operation.IndexedTable subTable = new Database.Operation.IndexedTable(table.m_table, g.m_GroupIndice);
            return subTable;
        }

        public override IEnumerable<Filter> SubFilters()
        {
            if (SubGroupFilter != null)
            {
                yield return SubGroupFilter;
            }
        }

        public override bool RemoveSubFilters(Filter f)
        {
            if (SubGroupFilter == f)
            {
                SubGroupFilter = null;
                return true;
            }
            return false;
        }

        public override bool ReplaceSubFilters(Filter replaced, Filter with)
        {
            if (SubGroupFilter == replaced)
            {
                SubGroupFilter = with;
                return true;
            }
            return false;
        }

        public override Filter GetSurrogate()
        {
            return SubGroupFilter;
        }

        public override bool OnGui(Database.Table sourceTable, ref bool dirty)
        {
            string colName = GetColumnName(sourceTable);
            string sortName = GetSortName(m_Order);
            EditorGUILayout.BeginHorizontal();
            bool bRemove = OnGui_RemoveButton();
            GUILayout.Label("Group" + sortName + " '" + colName + "'");
            if (SubGroupFilter != null)
            {
                if (SubGroupFilter.OnGui(sourceTable, ref dirty))
                {
                    dirty = true;
                    RemoveFilter(this, SubGroupFilter);
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            return bRemove;
        }

        public override void UpdateColumnState(Database.Table sourceTable, ColumnState[] colState)
        {
            long columnIndex = GetColumnIndex(sourceTable);
            colState[columnIndex].Grouped = true;
            //if(colState[columnIndex].sorted == database.SortOrder.None)
            //{
            //    colState[columnIndex].sorted = order;
            //}
            if (SubGroupFilter != null)
            {
                SubGroupFilter.UpdateColumnState(sourceTable, colState);
            }
        }

        public override bool Simplify(ref bool dirty)
        {
            if (SubGroupFilter != null)
            {
                SubGroupFilter.Simplify(ref dirty);
            }
            return false;
        }
    }

    /// <summary>
    /// Group a table using the name of a column
    /// </summary>
    internal class GroupByColumnName : Group
    {
        readonly string m_ColumnName;
        public GroupByColumnName(string columnName, SortOrder order)
            : base(order)
        {
            this.m_ColumnName = columnName;
        }

        public override Filter Clone(FilterCloning fc)
        {
            GroupByColumnName o = new GroupByColumnName(m_ColumnName, m_Order);
            if (SubGroupFilter != null)
            {
                o.SubGroupFilter = SubGroupFilter.Clone(fc);
            }
            return o;
        }

        public override int GetColumnIndex(Database.Table tableIn)
        {
            return tableIn.GetMetaData().GetColumnByName(m_ColumnName).Index;
        }

        public override string GetColumnName(Table tableIn)
        {
            return m_ColumnName;
        }
    }
    //sourceTable.GetMetaData().GetColumnByIndex(columnIndex).name;
    internal class GroupByColumnIndex : Group
    {
        readonly int m_ColumnIndex;
        public GroupByColumnIndex(int columnIndex, SortOrder order)
            : base(order)
        {
            this.m_ColumnIndex = columnIndex;
        }

        public override Filter Clone(FilterCloning fc)
        {
            GroupByColumnIndex o = new GroupByColumnIndex(m_ColumnIndex, m_Order);
            if (SubGroupFilter != null)
            {
                o.SubGroupFilter = SubGroupFilter.Clone(fc);
            }
            return o;
        }

        public override int GetColumnIndex(Database.Table tableIn)
        {
            return m_ColumnIndex;
        }

        public override string GetColumnName(Database.Table tableIn)
        {
            return tableIn.GetMetaData().GetColumnByIndex(m_ColumnIndex).Name;
        }
    }
}
