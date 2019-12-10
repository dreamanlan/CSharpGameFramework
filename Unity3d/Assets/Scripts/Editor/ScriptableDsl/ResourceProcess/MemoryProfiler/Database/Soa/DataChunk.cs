namespace Unity.MemoryProfilerForExtension.Editor.Database.Soa
{
    /// <summary>
    /// Represent a chunk of data with a specific length.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class DataChunk<DataT>
    {
        public DataChunk(long size)
        {
            m_Data = new DataT[size];
        }

        public DataT[] m_Data;
    }
}
