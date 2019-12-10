namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal interface IIndexedColumn
    {
        void Initialize(IndexedTable table, Column sourceColumn);
    }
    internal class IndexedColumnTyped<DataT> : ColumnTyped<DataT>, IIndexedColumn, IColumnDecorator where DataT : System.IComparable
    {
        protected ColumnTyped<DataT> m_SourceColumn;
        protected IndexedTable m_IndexedTable;

        public IndexedColumnTyped()
        {
            type = typeof(DataT);
        }

        void IIndexedColumn.Initialize(IndexedTable table, Column sourceColumn)
        {
            m_IndexedTable = table;
            m_SourceColumn = (ColumnTyped<DataT>)sourceColumn;
        }

        Column IColumnDecorator.GetBaseColumn()
        {
            return m_SourceColumn;
        }

        public override long GetRowCount()
        {
            return m_IndexedTable.GetRowCount();
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            return m_SourceColumn.GetRowValueString(m_IndexedTable.indices[row], formatter);
        }

        public override DataT GetRowValue(long row)
        {
            return m_SourceColumn.GetRowValue(m_IndexedTable.indices[row]);
        }

        public override void SetRowValue(long row, DataT value)
        {
            m_SourceColumn.SetRowValue(m_IndexedTable.indices[row], value);
        }

        public override LinkRequest GetRowLink(long row)
        {
            return m_SourceColumn.GetRowLink(m_IndexedTable.indices[row]);
        }
    }
}
