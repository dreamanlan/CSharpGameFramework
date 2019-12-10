namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    /// <summary>
    /// Column that output "Error" for all its rows
    /// </summary>
    internal class ColumnError : Database.ColumnTyped<string>
    {
        private Table m_Table;
        public ColumnError(Table table)
        {
            m_Table = table;
        }

        public override long GetRowCount()
        {
            return m_Table.GetRowCount();
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            return "Error";
        }

        public override string GetRowValue(long row)
        {
            return "Error";
        }

        public override void SetRowValue(long row, string value)
        {
        }

        public override LinkRequest GetRowLink(long row)
        {
            return null;
        }
    }
}
