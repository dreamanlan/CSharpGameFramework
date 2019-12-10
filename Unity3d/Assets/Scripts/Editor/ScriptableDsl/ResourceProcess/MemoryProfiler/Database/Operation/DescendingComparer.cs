namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal class DescendingComparerValueType<DataT> : System.Collections.Generic.IComparer<DataT> where DataT : System.IComparable
    {
        public int Compare(DataT a, DataT b) { return b.CompareTo(a); }
    }
    internal class DescendingComparerReferenceType<DataT> : System.Collections.Generic.IComparer<DataT> where DataT : System.IComparable
    {
        public int Compare(DataT a, DataT b)
        {
            if (b == null)
            {
                if (a == null) return 0;
                return -1;
            }
            return b.CompareTo(a);
        }
    }
}
