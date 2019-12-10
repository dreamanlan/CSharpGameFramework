namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal class AscendingComparerValueType<DataT> : System.Collections.Generic.IComparer<DataT> where DataT : System.IComparable
    {
        public int Compare(DataT a, DataT b) { return a.CompareTo(b); }
    }
    internal class AscendingComparerReferenceType<DataT> : System.Collections.Generic.IComparer<DataT> where DataT : System.IComparable
    {
        public int Compare(DataT a, DataT b)
        {
            if (a == null)
            {
                if (b == null) return 0;
                return -1;
            }
            return a.CompareTo(b);
        }
    }
}
