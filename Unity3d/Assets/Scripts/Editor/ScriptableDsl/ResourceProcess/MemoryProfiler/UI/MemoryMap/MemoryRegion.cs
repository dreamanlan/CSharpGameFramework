using System.Collections.Generic;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI.MemoryMap
{
    public enum RegionType
    {
        Native,
        VirtualMemory,
        Managed,
        ManagedStack,
    };

    internal class MemoryRegion
    {
        public int Group { get; set; }
        public readonly string Name;
        public readonly ulong AddressBegin;
        public readonly ulong Size;
        public readonly RegionType Type;
        public Color32 ColorRegion { get; set; }
        public ulong AddressEnd
        {
            get { return AddressBegin + Size; }
        }

        public MemoryRegion(RegionType type,  ulong begin, ulong size, string displayName)
        {
            AddressBegin = begin;
            Size = size;
            Name = displayName;
            Type = type;
        }
    }
}
