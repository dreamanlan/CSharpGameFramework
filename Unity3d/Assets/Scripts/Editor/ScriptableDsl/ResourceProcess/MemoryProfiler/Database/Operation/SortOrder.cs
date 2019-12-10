namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal enum SortOrder
    {
        None,
        Ascending,
        Descending,
    }
    internal class SortOrderString
    {
        public static SortOrder StringToSortOrder(string s, SortOrder defaultOrder = SortOrder.None)
        {
            if (s == null) return defaultOrder;
            switch (s.ToLower())
            {
                case "ascending":
                case "asc":
                    return SortOrder.Ascending;
                case "descending":
                case "des":
                    return SortOrder.Descending;
            }
            return defaultOrder;
        }
    }
}
