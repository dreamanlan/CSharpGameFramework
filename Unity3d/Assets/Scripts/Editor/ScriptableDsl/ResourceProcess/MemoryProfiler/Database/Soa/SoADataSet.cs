namespace Unity.MemoryProfilerForExtension.Editor.Database.Soa
{
    /// <summary>
    /// Represent a `Struct-of-Array` structured data set.
    /// </summary>
    internal struct SoaDataSet
    {
        public SoaDataSet(long dataCount, long chunkSize)
        {
            DataCount = dataCount;
            ChunkSize = chunkSize;
        }

        public readonly long DataCount;
        public readonly long ChunkSize;
    }
}
