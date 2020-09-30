using System;
using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Format
{
    [Flags]
    public enum CaptureFlags : uint
    {
        ManagedObjects = 1 << 0,
        NativeObjects = 1 << 1,
        NativeAllocations = 1 << 2,
        NativeAllocationSites = 1 << 3,
        NativeStackTraces = 1 << 4,
    }

    public class MetaData
    {
        [NonSerialized]
        public string content;
        [NonSerialized]
        public string platform;
        #if !UNITY_2020_1_OR_NEWER
        public Texture2D screenshot;
        #endif
    }

    // !!!!! NOTE: Keep in sync with native unity code-base
    public class QueriedMemorySnapshot : IDisposable
    {
        const uint kMinSupportedVersion = 8;
        const uint kCurrentVersion = 10;

        public static QueriedMemorySnapshot Load(string path)
        {
            MemorySnapshotFileReader reader = new MemorySnapshotFileReader(path);

            uint ver = reader.GetDataSingle(EntryType.Metadata_Version, ConversionFunctions.ToUInt32);

            if (ver < kMinSupportedVersion)
            {
                throw new Exception(string.Format("Memory snapshot at {0}, is using an older format version: {1}", new object[] { path, ver.ToString() }));
            }

            return new QueriedMemorySnapshot(reader);
        }

        public static void Save(QueriedMemorySnapshot snapshot, string writePath)
        {
            string path = snapshot.GetReader().FilePath;

            if (!System.IO.File.Exists(path))
            {
                throw new UnityException("Failed to save snapshot. Snapshot file: " + path + " is missing.");
            }

            FileUtil.CopyFileOrDirectory(path, writePath);
        }

        MemorySnapshotFileReader m_Reader = null;

        public ConnectionEntries connections { get; internal set; }
        public FieldDescriptionEntries fieldDescriptions { get; internal set; }
        public GCHandleEntries gcHandles { get; internal set; }
        public ManagedMemorySectionEntries managedHeapSections { get; internal set; }
        public ManagedMemorySectionEntries managedStacks { get; internal set; }
        public NativeAllocationEntries nativeAllocations { get; internal set; }
        public NativeAllocationSiteEntries nativeAllocationSites { get; internal set; }
        public NativeCallstackSymbolEntries nativeCallstackSymbols { get; internal set; }
        public NativeMemoryLabelEntries nativeMemoryLabels { get; internal set; }
        public NativeMemoryRegionEntries nativeMemoryRegions { get; internal set; }
        public NativeObjectEntries nativeObjects { get; internal set; }
        public NativeRootReferenceEntries nativeRootReferences { get; internal set; }
        public NativeTypeEntries nativeTypes { get; internal set; }
        public TypeDescriptionEntries typeDescriptions { get; internal set; }

        internal QueriedMemorySnapshot(MemorySnapshotFileReader reader)
        {
            m_Reader = reader;
            BuildEntries();
        }

        internal void BuildEntries()
        {
            connections = new ConnectionEntries(m_Reader);
            fieldDescriptions = new FieldDescriptionEntries(m_Reader);
            gcHandles = new GCHandleEntries(m_Reader);
            managedHeapSections = new ManagedMemorySectionEntries(m_Reader, EntryType.ManagedHeapSections_StartAddress);
            managedStacks = new ManagedMemorySectionEntries(m_Reader, EntryType.ManagedStacks_StartAddress);
            nativeAllocations = new NativeAllocationEntries(m_Reader);
            nativeAllocationSites = new NativeAllocationSiteEntries(m_Reader);
            nativeCallstackSymbols = new NativeCallstackSymbolEntries(m_Reader);
            nativeMemoryLabels = new NativeMemoryLabelEntries(m_Reader);
            nativeMemoryRegions = new NativeMemoryRegionEntries(m_Reader);
            nativeObjects = new NativeObjectEntries(m_Reader, version == kCurrentVersion);
            nativeRootReferences = new NativeRootReferenceEntries(m_Reader);
            nativeTypes = new NativeTypeEntries(m_Reader);
            typeDescriptions = new TypeDescriptionEntries(m_Reader);
        }

        internal MemorySnapshotFileReader GetReader()
        {
            return m_Reader;
        }

        public uint version
        {
            get
            {
                return m_Reader.GetDataSingle(EntryType.Metadata_Version, ConversionFunctions.ToUInt32);
            }
        }
        public MetaData metadata
        {
            get
            {
                byte[] array = m_Reader.GetDataSingle(EntryType.Metadata_UserMetadata, ConversionFunctions.ToByteArray);
                // decoded as
                //   content_data_length
                //   content_data
                //   platform_data_length
                //   platform_data
                var data = new MetaData();

                if (array.Length == 0)
                {
                    data.content = "";
                    data.platform = "";
                    return data;
                }

                int offset = 0;
                int dataLength = 0;
                offset = ReadIntFromByteArray(array, offset, out dataLength);
                offset = ReadStringFromByteArray(array, offset, dataLength, out data.content);
                offset = ReadIntFromByteArray(array, offset, out dataLength);
                offset = ReadStringFromByteArray(array, offset, dataLength, out data.platform);

                if (offset + sizeof(int) < array.Length)
                {
                    offset = ReadIntFromByteArray(array, offset, out dataLength);
                    if (dataLength > 0)
                    {
                        int width;
                        int height;
                        int format;
                        byte[] screenshot = new byte[dataLength];

                        Array.Copy(array, offset, screenshot, 0, dataLength);

                        offset += dataLength;
                        offset = ReadIntFromByteArray(array, offset, out width);
                        offset = ReadIntFromByteArray(array, offset, out height);
                        offset = ReadIntFromByteArray(array, offset, out format);

                        //Suppress compiler warning about member
#if !UNITY_2020_1_OR_NEWER
                    data.screenshot = new Texture2D(width, height, (TextureFormat)format, false);
                    data.screenshot.LoadRawTextureData(screenshot);
                    data.screenshot.Apply();
#endif
                    }
                }

                return data;
            }
        }

        private static int ReadIntFromByteArray(byte[] array, int offset, out int value)
        {
            unsafe
            {
                int lValue = 0;
                byte* pi = (byte*)&lValue;
                pi[0] = array[offset++];
                pi[1] = array[offset++];
                pi[2] = array[offset++];
                pi[3] = array[offset++];

                value = lValue;
            }

            return offset;
        }

        private static int ReadStringFromByteArray(byte[] array, int offset, int stringLength, out string value)
        {
            if (stringLength == 0)
            {
                value = "";
                return offset;
            }

            unsafe
            {
                value = new string('\0', stringLength);
                fixed(char* p = value)
                {
                    char* begin = p;
                    char* end = p + value.Length;

                    while (begin != end)
                    {
                        for (int i = 0; i < sizeof(char); ++i)
                        {
                            ((byte*)begin)[i] = array[offset++];
                        }

                        begin++;
                    }
                }
            }

            return offset;
        }

        public DateTime recordDate
        {
            get
            {
                long ticks = m_Reader.GetDataSingle(EntryType.Metadata_RecordDate, ConversionFunctions.ToInt64);
                return new DateTime(ticks);
            }
        }

        public CaptureFlags captureFlags
        {
            get
            {
                return (CaptureFlags)m_Reader.GetDataSingle(EntryType.Metadata_CaptureFlags, ConversionFunctions.ToUInt32);
            }
        }

        public VirtualMachineInformation virtualMachineInformation
        {
            get
            {
                return m_Reader.GetDataSingle(EntryType.Metadata_VirtualMachineInformation, ConversionFunctions.ToVirtualMachineInformation);
            }
        }

        ~QueriedMemorySnapshot()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_Reader == null)
                {
                    return;
                }

                m_Reader.Dispose();
                m_Reader = null;
            }
        }
    }

    [Flags]
    public enum ObjectFlags
    {
        IsDontDestroyOnLoad = 0x1,
        IsPersistent = 0x2,
        IsManager = 0x4,
    }

    public static class ObjectFlagsExtensions
    {
        public static bool IsDontDestroyOnLoad(this ObjectFlags flags)
        {
            return (flags & ObjectFlags.IsDontDestroyOnLoad) != 0;
        }

        public static bool IsPersistent(this ObjectFlags flags)
        {
            return (flags & ObjectFlags.IsPersistent) != 0;
        }

        public static bool IsManager(this ObjectFlags flags)
        {
            return (flags & ObjectFlags.IsManager) != 0;
        }
    }

    [Flags]
    public enum TypeFlags
    {
        kNone = 0,
        kValueType = 1 << 0,
        kArray = 1 << 1,
        kArrayRankMask = unchecked((int)0xFFFF0000)
    }

    public static class TypeFlagsExtensions
    {
        public static bool IsValueType(this TypeFlags flags)
        {
            return (flags & TypeFlags.kValueType) != 0;
        }

        public static bool IsArray(this TypeFlags flags)
        {
            return (flags & TypeFlags.kArray) != 0;
        }

        public static int ArrayRank(this TypeFlags flags)
        {
            return (int)(flags & TypeFlags.kArrayRankMask) >> 16;
        }
    }

    public struct VirtualMachineInformation
    {
        public int pointerSize { get; internal set; }
        public int objectHeaderSize { get; internal set; }
        public int arrayHeaderSize { get; internal set; }
        public int arrayBoundsOffsetInHeader { get; internal set; }
        public int arraySizeOffsetInHeader { get; internal set; }
        public int allocationGranularity { get; internal set; }
    }
}
