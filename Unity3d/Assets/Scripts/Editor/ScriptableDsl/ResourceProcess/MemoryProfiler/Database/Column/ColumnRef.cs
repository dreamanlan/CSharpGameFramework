namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    /// <summary>
    /// A name reference to a column in a table. Not currently used
    /// </summary>
    internal struct ColumnRef
    {
        public string tableName;
        public string columnName;

        public static ColumnRef kInvalidRef = new ColumnRef {tableName = "InvalidName", columnName = "InvalidName" };
    }
}
