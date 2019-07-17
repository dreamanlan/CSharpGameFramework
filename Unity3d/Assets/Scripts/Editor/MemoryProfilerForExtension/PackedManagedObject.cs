using System;

namespace MemoryProfilerWindowForExtension
{
    [Serializable]
    public class PackedManagedObject
    {
        public UInt64 address;
        public int typeIndex;
        public int size;
    }
}
