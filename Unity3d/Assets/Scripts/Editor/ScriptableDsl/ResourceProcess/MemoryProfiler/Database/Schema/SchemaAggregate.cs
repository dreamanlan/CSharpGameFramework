namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    internal class SchemaAggregate : Schema
    {
        public string name = "<unknown>";
        public System.Collections.Generic.List<Table> tables = new System.Collections.Generic.List<Table>();
        public System.Collections.Generic.Dictionary<string, Table> tablesByName = new System.Collections.Generic.Dictionary<string, Table>();

        public override string GetDisplayName()
        {
            return name;
        }

        public override bool OwnsTable(Table table)
        {
            if (table.Schema == this) return true;
            return tables.Contains(table);
        }

        public override long GetTableCount()
        {
            return tables.Count;
        }

        public override Table GetTableByIndex(long index)
        {
            return tables[(int)index];
        }

        public override Table GetTableByName(string name)
        {
            Table vt;
            if (tablesByName.TryGetValue(name, out vt))
            {
                return vt;
            }
            return null;
        }

        public void AddTable(Table t)
        {
            string name = t.GetName();
            var existingTable = GetTableByName(name);
            if (existingTable != null)
            {
                tables.Remove(existingTable);
                tablesByName.Remove(name);
            }
            tables.Add(t);
            tablesByName.Add(name, t);
        }

        public void ClearTable()
        {
            tables.Clear();
            tablesByName.Clear();
        }
    }
}
