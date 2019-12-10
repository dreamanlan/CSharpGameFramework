namespace Unity.MemoryProfilerForExtension.Editor.Database.Soa
{
    /// <summary>
    /// Represent a source of data that can be requested in chunks
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal abstract class DataSource<DataT>
    {
        public abstract void Get(Range range, ref DataT[] dataOut);
    }
}
