namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    // TableInstanceRef reference to a table with a set of parameter defined.
    // A Schema can translate a 'TableInstanceRef' to an actual table using the GetTableByReference method
    internal class TableReference
    {
        public TableReference(string name)
        {
            Name = name;
        }

        public TableReference(string name, ParameterSet param)
        {
            Name = name;
            Param = param;
        }

        public string Name;
        public ParameterSet Param;
    }

    internal abstract class Schema
    {
        public abstract long GetTableCount();
        public abstract Table GetTableByIndex(long index);
        public abstract Table GetTableByName(string name);
        public virtual Table GetTableByName(string name, ParameterSet param)
        {
            return GetTableByName(name);
        }

        public virtual Table GetTableByReference(TableReference tableRef)
        {
            return GetTableByName(tableRef.Name, tableRef.Param);
        }

        public abstract string GetDisplayName();
        public abstract bool OwnsTable(Table table);
    }
}
