using System;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal class GroupedTable : ExpandTable
    {
        public Database.Table m_table;
        public ExpandTable m_expandTable;
        public Grouping.GroupCollection m_GroupCollection;


        public int m_GroupedColumnFirst;//index in m_ColumnOrder and m_SortOrder
        public int m_GroupedColumnLast;
        public int[] m_ColumnOrder;
        public SortOrder[] m_SortOrder;

        public struct Group
        {
            public ArrayRange m_GroupIndice;
            public Group(ArrayRange indices)
            {
                m_GroupIndice = indices;
            }
        }
        public Group[] m_Groups;
        public bool m_degenetareGroupOf1 = true;

        // colToGroup is an index from aNewColumnOrder.
        public GroupedTable(Database.Table table, ArrayRange range, int colToGroup, SortOrder sortOrder, FnCreateGroupTable subTable)
            : base(table.Schema)
        {
            m_table = table;
            if (m_table is ExpandTable) m_expandTable = (ExpandTable)m_table;
            m_Meta = m_table.GetMetaData();
            m_GroupedColumnFirst = 0;
            m_GroupedColumnLast = 0;
            m_ColumnOrder = new int[] { colToGroup };// colGroupOrder;
            m_SortOrder = new SortOrder[1] { sortOrder };
            m_createGroupTable = subTable;

            var col = m_table.GetColumnByIndex(colToGroup);
            var metaCol = m_Meta.GetColumnByIndex(colToGroup);
            if (metaCol.DefaultGroupAlgorithm != null)
            {
                m_GroupCollection = metaCol.DefaultGroupAlgorithm.Group(col, range, sortOrder);
            }
            else
            {
                throw new Exception("Trying to group a column without grouping algorithm. Column '" + metaCol.Name + "' from table '" + m_table.GetName() + "'");
            }
            InitializeFromGroupCollection(col, colToGroup);
        }

        public GroupedTable(Database.Table table, ArrayRange range, int colToGroupFirst, int colToGroupLast, int[] colGroupOrder, SortOrder[] sortOrder)
            : base(table.Schema)
        {
            m_table = table;
            if (m_table is ExpandTable) m_expandTable = (ExpandTable)m_table;
            m_Meta = m_table.GetMetaData();
            m_GroupedColumnFirst = colToGroupFirst;
            m_GroupedColumnLast = colToGroupLast;
            m_ColumnOrder = colGroupOrder;
            m_SortOrder = sortOrder;

            int sourceGroupedColumnIndex = m_ColumnOrder[colToGroupFirst];

            var col = m_table.GetColumnByIndex(sourceGroupedColumnIndex);
            var metaCol = m_Meta.GetColumnByIndex(sourceGroupedColumnIndex);
            if (metaCol.DefaultGroupAlgorithm != null)
            {
                m_GroupCollection = metaCol.DefaultGroupAlgorithm.Group(col, range, m_SortOrder[colToGroupFirst]);
            }
            else
            {
                throw new Exception("Trying to group a column without grouping algorithm. Column '" + metaCol.Name + "' from table '" + m_table.GetName() + "'");
            }
            InitializeFromGroupCollection(col, sourceGroupedColumnIndex);
        }

        private void InitializeFromGroupCollection(Database.Column col, long sourceColToGroup)
        {
            //create groups
            long groupCount = m_GroupCollection.GetGroupCount();
            m_Groups = new Group[groupCount];

            for (long i = 0; i < groupCount; ++i)
            {
                m_Groups[i] = new Group(m_GroupCollection.GetGroup(i).GetIndices(m_GroupCollection));
            }


            //create columns
            m_Columns = new System.Collections.Generic.List<Column>(m_Meta.GetColumnCount());
            for (int i = 0; i != m_Meta.GetColumnCount(); ++i)
            {
                var metaCol = m_Meta.GetColumnByIndex(i);
                IGroupedColumn newCol = (IGroupedColumn)ColumnCreator.CreateColumn(typeof(GroupedColumnTyped<>), metaCol.Type);

                newCol.Initialize(this, m_table.GetColumnByIndex(i), i, metaCol.DefaultMergeAlgorithm, i == sourceColToGroup);
                m_Columns.Add((Column)newCol);
            }
            InitGroup(groupCount);
        }

        //class Updater : IUpdater
        //{
        //    public GroupedTable m_Table;
        //    public Grouping.GroupCollection m_GroupCollection;
        //    long IUpdater.OldToNewGroupIndex(long a)
        //    {
        //        m_Table.m_GroupCollection.GetGroup(a).;
        //        m_GroupCollection.GetGroup(a);
        //        var f = m_Table.m_Fields.fieldIndices[a];
        //        var newIndex = System.Array.FindIndex(m_Fields.fieldIndices, x => x == a);
        //        return newIndex;
        //    }
        //}

        //public override IUpdater BeginUpdate()
        //{
        //    if (m_table.Update())
        //    {
        //        return true;
        //    }
        //    var u = new Updater();
        //    u.m_Table = this;
        //    u.m_Fields = BuildFieldList();
        //    return u;
        //}
        //public override void EndUpdate(IUpdater updater)
        //{
        //    SetFieldsList((updater as Updater).m_Fields);
        //}

        public override bool Update()
        {
            bool r = base.Update();
            if (m_table.Update())
            {
                return true;
            }
            return r;
        }

        public delegate Table FnCreateGroupTable(GroupedTable table, Group g, long groupIndex);
        protected FnCreateGroupTable m_createGroupTable;
        public void SetCreateGroupTableDelegate(FnCreateGroupTable fn)
        {
            m_createGroupTable = fn;
        }

        public override Table CreateGroupTable(long groupIndex)
        {
            if (IsGroupDegenerate(groupIndex))
            {
                // if sub table is an expand table, act as pass-through
                if (m_expandTable != null)
                {
                    var i = m_Groups[groupIndex].m_GroupIndice[0];
                    return m_expandTable.CreateGroupTable(i);
                }
            }
            if (m_createGroupTable != null)
            {
                return m_createGroupTable(this, m_Groups[groupIndex], groupIndex);
            }
            int subGroupColumn = m_GroupedColumnFirst + 1;
            if (subGroupColumn < m_GroupedColumnLast)
            {
                GroupedTable subTable = new GroupedTable(m_table, m_Groups[groupIndex].m_GroupIndice, subGroupColumn, m_GroupedColumnLast, m_ColumnOrder, m_SortOrder);
                return subTable;
            }
            else if (subGroupColumn < m_ColumnOrder.Length)
            {
                //create a sorted table only
                SortedTable subTable = new SortedTable(m_table, m_ColumnOrder, m_SortOrder, subGroupColumn, m_Groups[groupIndex].m_GroupIndice);
                return subTable;
            }
            else
            {
                //create indexed table
                IndexedTable subTable = new IndexedTable(m_table, m_Groups[groupIndex].m_GroupIndice);
                return subTable;
            }
        }

        public override bool IsGroupExpandable(long groupIndex, int col)
        {
            if (IsGroupDegenerate(groupIndex))
            {
                // if sub table is an expand table, act as pass-through
                if (m_expandTable != null)
                {
                    var i = m_Groups[groupIndex].m_GroupIndice[0];
                    return m_expandTable.IsGroupExpandable(i, col);
                }
                return false;
            }
            return m_ColumnOrder[m_GroupedColumnFirst] == col;
        }

        public override bool IsColumnExpandable(int col)
        {
            bool b = m_ColumnOrder[m_GroupedColumnFirst] == col;
            if (m_expandTable != null)
            {
                b |= m_expandTable.IsColumnExpandable(col);
            }
            return b;
        }

        // Degenerate group display their first entry as-is
        public bool IsGroupDegenerate(long groupIndex)
        {
            return m_degenetareGroupOf1 && m_Groups[groupIndex].m_GroupIndice.Count == 1;
        }
    }
}
