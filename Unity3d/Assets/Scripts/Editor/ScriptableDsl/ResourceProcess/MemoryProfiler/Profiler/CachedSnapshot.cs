using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MemoryProfilerForExtension.Editor.Database.Soa;
using Unity.Profiling;
using Unity.MemoryProfilerForExtension.Editor.Format;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal static class VMTools
    {
        //supported archs
        public const int x64ArchPtrSize = 8;
        public const int x86ArchPtrSize = 4;

        public static bool ValidateVirtualMachineInfo(VirtualMachineInformation vmInfo)
        {
            bool ok = vmInfo.pointerSize == x64ArchPtrSize || vmInfo.pointerSize == x86ArchPtrSize;
            if (ok)
            {
                //partial checks to validate computations based on pointer size
                int expectedObjHeaderSize = 2 * vmInfo.pointerSize;
                ok |= expectedObjHeaderSize == vmInfo.objectHeaderSize;
                ok |= expectedObjHeaderSize == vmInfo.allocationGranularity;
            }

            return ok;
        }
    }
    internal static class TypeTools
    {
        public enum FieldFindOptions
        {
            OnlyInstance,
            OnlyStatic
        }

        static void RecurseCrawlFields(ref List<int> fieldsBuffer, int ITypeArrayIndex, CachedSnapshot data, FieldFindOptions fieldFindOptions, bool crawlBase)
        {
            bool isValueType = data.typeDescriptions.HasFlag(ITypeArrayIndex, TypeFlags.kValueType);
            if (crawlBase)
            {
                int baseTypeIndex = data.typeDescriptions.baseOrElementTypeIndex[ITypeArrayIndex];
                if (crawlBase && baseTypeIndex != -1 && !isValueType)
                {
                    int baseArrayIndex = data.typeDescriptions.TypeIndex2ArrayIndex(baseTypeIndex);
                    RecurseCrawlFields(ref fieldsBuffer, baseArrayIndex, data, fieldFindOptions, true);
                }
            }


            int iTypeIndex = data.typeDescriptions.typeIndex[ITypeArrayIndex];
            var fieldIndices = data.typeDescriptions.fieldIndices[ITypeArrayIndex];
            for (int i = 0; i < fieldIndices.Length; ++i)
            {
                var iField = fieldIndices[i];

                if (!FieldMatchesOptions(iField, data, fieldFindOptions))
                    continue;

                if (data.fieldDescriptions.typeIndex[iField] == iTypeIndex && isValueType)
                {
                    // this happens in primitive types like System.Single, which is a weird type that has a field of its own type.
                    continue;
                }

                if (data.fieldDescriptions.offset[iField] == -1) //TODO: verify this assumption
                {
                    // this is how we encode TLS fields. We don't support TLS fields yet.
                    continue;
                }

                fieldsBuffer.Add(iField);
            }
        }
       
        public static void AllFieldArrayIndexOf(ref List<int> fieldsBuffer, int ITypeArrayIndex, CachedSnapshot data, FieldFindOptions findOptions, bool includeBase)
        {
            //make sure we clear before we start crawling
            fieldsBuffer.Clear();
            RecurseCrawlFields(ref fieldsBuffer, ITypeArrayIndex, data, findOptions, includeBase);
        }

        static bool FieldMatchesOptions(int fieldIndex, CachedSnapshot data, FieldFindOptions options)
        {
            if (options == FieldFindOptions.OnlyStatic)
            {
                return data.fieldDescriptions.isStatic[fieldIndex];
            }
            if (options == FieldFindOptions.OnlyInstance)
            {
                return !data.fieldDescriptions.isStatic[fieldIndex];
            }
            return false;
        }
    }

    internal class CachedSnapshot
    {
        const uint kSnapshotVersionWithConnectionChanges = 10;
        uint m_SnapshotVersion;
        public static int kCacheEntrySize = 4 * 1024;

        public bool HasConnectionOverhaul
        {
            get { return m_SnapshotVersion >= kSnapshotVersionWithConnectionChanges; }
        }

        public ManagedData CrawledData { internal set; get; }
        public QueriedMemorySnapshot packedMemorySnapshot { private set; get; }

        public class NativeAllocationSiteEntriesCache
        {
            public long Count;
            public DataArray.Cache<long> id;
            public DataArray.Cache<int> memoryLabelIndex;
            public DataArray.Cache<ulong[]> callstackSymbols;
            public SoaDataSet dataSet;
            public NativeAllocationSiteEntriesCache(NativeAllocationSiteEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                id = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.id));
                memoryLabelIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.memoryLabelIndex));
                callstackSymbols = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.callstackSymbols));
            }

            public string GetReadableCallstackForId(NativeCallstackSymbolEntriesCache symbols, long id)
            {
                int entryIdx = this.id.FindIndex(x => x == id);

                return entryIdx < 0 ? "" : GetReadableCallstack(symbols, entryIdx);
            }

            public string GetReadableCallstack(NativeCallstackSymbolEntriesCache symbols, long idx)
            {
                string readableStackTrace = "";

                ulong[] callstackSymbols = this.callstackSymbols[idx];

                for (int i = 0; i < callstackSymbols.Length; ++i)
                {
                    int symbolIdx = symbols.symbol.FindIndex(x => x == callstackSymbols[i]);

                    if (symbolIdx < 0)
                        readableStackTrace += "<unknown>\n";
                    else
                        readableStackTrace += symbols.readableStackTrace[symbolIdx];
                }

                return readableStackTrace;
            }
        }

        public class TypeDescriptionEntriesCache
        {
            public long Count;
            public DataArray.Cache<TypeFlags> flags;
            public DataArray.Cache<string> typeDescriptionName;
            public DataArray.Cache<string> assembly;
            public DataArray.Cache<int[]> fieldIndices;
            public DataArray.Cache<byte[]> staticFieldBytes;
            public DataArray.Cache<int> baseOrElementTypeIndex;
            public DataArray.Cache<int> size;
            public DataArray.Cache<ulong> typeInfoAddress;
            public DataArray.Cache<int> typeIndex;
            public SoaDataSet dataSet;


            const int k_DefaultFieldProcessingBufferSize = 64;
            const string k_SystemObjectTypeName = "System.ValueType";
            const string k_SystemValueTypeName = "System.Object";
            const string k_SystemEnumTypeName = "System.Enum";

            public TypeDescriptionEntriesCache(TypeDescriptionEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                flags                   = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.flags));
                typeDescriptionName     = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.typeDescriptionName));
                assembly                = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.assembly));
                fieldIndices            = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.fieldIndices));
                staticFieldBytes        = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.staticFieldBytes));
                baseOrElementTypeIndex  = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.baseOrElementTypeIndex));
                size                    = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.size));
                typeInfoAddress         = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.typeInfoAddress));
                typeIndex               = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.typeIndex));
            }

            public int[][] fieldIndices_instance;//includes all bases' instance fields
            public int[][] fieldIndices_static;  //includes all bases' static fields
            public int[][] fieldIndicesOwned_static;  //includes only type's static fields
            public bool[] m_HasStaticFields;
            public int iType_ValueType;
            public int iType_Object;
            public int iType_Enum;

            public Dictionary<UInt64, int> typeInfoToArrayIndex;
            public Dictionary<int, int> typeIndexToArrayIndex;

            // Check all bases' fields
            public bool HasAnyField(int iType)
            {
                return fieldIndices_instance[iType].Length > 0 || fieldIndices_static[iType].Length > 0;
            }

            // Check all bases' fields
            public bool HasAnyStaticField(int iType)
            {
                return fieldIndices_static[iType].Length > 0;
            }

            // Check only the type's fields
            public bool HasStaticField(int iType)
            {
                return m_HasStaticFields[iType];
            }

            public bool HasFlag(int arrayIndex, TypeFlags flag)
            {
                return (flags[arrayIndex] & flag) == flag;
            }

            public int GetRank(int arrayIndex)
            {
                int r = (int)(flags[arrayIndex] & TypeFlags.kArrayRankMask) >> 16;
                return r;
            }

            public int TypeIndex2ArrayIndex(int typeIndex)
            {
                int i;
                if (!typeIndexToArrayIndex.TryGetValue(typeIndex, out i))
                {
                    throw new Exception("typeIndex not found");
                }
                return i;
            }

            public int TypeInfo2ArrayIndex(UInt64 aTypeInfoAddress)
            {
                int i;

                if (!typeInfoToArrayIndex.TryGetValue(aTypeInfoAddress, out i))
                {
                    return -1;
                }
                return i;
            }

            static ProfilerMarker typeFieldArraysBuild = new ProfilerMarker("MemoryProfilerForExtension.TypeFields.TypeFieldArrayBuilding");
            public void InitSecondaryItems(CachedSnapshot cs)
            {
                typeInfoToArrayIndex = Enumerable.Range(0, (int)typeInfoAddress.Length).ToDictionary(x => typeInfoAddress[x], x => x);
                typeIndexToArrayIndex = Enumerable.Range(0, (int)typeIndex.Length).ToDictionary(x => typeIndex[x], x => x);

                using (typeFieldArraysBuild.Auto())
                {
                    m_HasStaticFields = new bool[Count];
                    fieldIndices_instance = new int[Count][];
                    fieldIndices_static = new int[Count][];
                    fieldIndicesOwned_static = new int[Count][];
                    List<int> fieldProcessingBuffer = new List<int>(k_DefaultFieldProcessingBufferSize);

                    for (int i = 0; i < Count; ++i)
                    {
                        m_HasStaticFields[i] = false;
                        foreach (var iField in fieldIndices[i])
                        {
                            if (cs.fieldDescriptions.isStatic[iField])
                            {
                                m_HasStaticFields[i] = true;
                                break;
                            }
                        }

                        TypeTools.AllFieldArrayIndexOf(ref fieldProcessingBuffer, i, cs, TypeTools.FieldFindOptions.OnlyInstance, true);
                        fieldIndices_instance[i] = fieldProcessingBuffer.ToArray();

                        TypeTools.AllFieldArrayIndexOf(ref fieldProcessingBuffer, i, cs, TypeTools.FieldFindOptions.OnlyStatic, true);
                        fieldIndices_static[i] = fieldProcessingBuffer.ToArray();

                        TypeTools.AllFieldArrayIndexOf(ref fieldProcessingBuffer, i, cs, TypeTools.FieldFindOptions.OnlyStatic, false);
                        fieldIndicesOwned_static[i] = fieldProcessingBuffer.ToArray();
                    }
                }

                iType_ValueType = typeDescriptionName.FindIndex(x => x == k_SystemValueTypeName);
                iType_Object = typeDescriptionName.FindIndex(x => x == k_SystemObjectTypeName);
                iType_Enum = typeDescriptionName.FindIndex(x => x == k_SystemEnumTypeName);

            }
        }

        public class NativeTypeEntriesCache
        {
            public long Count;
            public DataArray.Cache<string> typeName;
            public DataArray.Cache<int> nativeBaseTypeArrayIndex;
            public SoaDataSet dataSet;
            public NativeTypeEntriesCache(NativeTypeEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                typeName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.typeName));
                nativeBaseTypeArrayIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.nativeBaseTypeArrayIndex));
            }
        }

        public class NativeRootReferenceEntriesCache
        {
            public long Count;
            public DataArray.Cache<long> id;
            public DataArray.Cache<string> areaName;
            public DataArray.Cache<string> objectName;
            public DataArray.Cache<ulong> accumulatedSize;
            public SoaDataSet dataSet;
            public NativeRootReferenceEntriesCache(NativeRootReferenceEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                id = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.id));
                areaName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.areaName));
                objectName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.objectName));
                accumulatedSize = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.accumulatedSize));
            }
        }

        public class NativeObjectEntriesCache
        {
            public long Count;
            public DataArray.Cache<string> objectName;
            public DataArray.Cache<int> instanceId;
            public DataArray.Cache<ulong> size;
            public DataArray.Cache<int> nativeTypeArrayIndex;
            public DataArray.Cache<HideFlags> hideFlags;
            public DataArray.Cache<ObjectFlags> flags;
            public DataArray.Cache<ulong> nativeObjectAddress;
            public Dictionary<ulong, int> nativeObjectAddressToInstanceId { private set; get; }
            public DataArray.Cache<long> rootReferenceId;
            public int[] refcount;
            public int[] managedObjectIndex;
            public SoaDataSet dataSet;
            public NativeObjectEntriesCache(NativeObjectEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                objectName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.objectName));
                instanceId = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.instanceId));
                size = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.size));
                nativeTypeArrayIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.nativeTypeArrayIndex));
                hideFlags = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.hideFlags));
                flags = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.flags));
                nativeObjectAddress = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.nativeObjectAddress));
                rootReferenceId = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.rootReferenceId));
                nativeObjectAddressToInstanceId = new Dictionary<ulong, int>((int)nativeObjectAddress.Length);
                for(int i = 0; i < nativeObjectAddress.Length; ++i)
                {
                    nativeObjectAddressToInstanceId.Add(nativeObjectAddress[i], instanceId[i]);
                }
            }

            public const int k_InstanceIDNone = 0;
            public SortedDictionary<int, int> instanceId2Index;
            public void InitSecondaryItems()
            {
                instanceId2Index = new SortedDictionary<int, int>();
                for (int i = 0; i != Count; ++i)
                {
                    var id = instanceId[i];
                    instanceId2Index[id] = i;
                }
            }

            public void InitSecondaryItems(CachedSnapshot snapshot)
            {
                refcount = new int[Count];
                managedObjectIndex = new int[Count];

                for (int i = 0; i != Count; ++i)
                {
                    managedObjectIndex[i] = -1;
                }
            }
        }

        public class NativeMemoryRegionEntriesCache
        {
            public long Count;
            public DataArray.Cache<string> memoryRegionName;
            public DataArray.Cache<int> parentIndex;
            public DataArray.Cache<ulong> addressBase;
            public DataArray.Cache<ulong> addressSize;
            public DataArray.Cache<int> firstAllocationIndex;
            public DataArray.Cache<int> numAllocations;
            public SoaDataSet dataSet;
            public NativeMemoryRegionEntriesCache(NativeMemoryRegionEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                memoryRegionName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.memoryRegionName));
                parentIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.parentIndex));
                addressBase = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.addressBase));
                addressSize = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.addressSize));
                firstAllocationIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.firstAllocationIndex));
                numAllocations = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.numAllocations));
            }
        }

        public class NativeMemoryLabelEntriesCache
        {
            public long Count;
            public DataArray.Cache<string> memoryLabelName;
            public SoaDataSet dataSet;
            public NativeMemoryLabelEntriesCache(NativeMemoryLabelEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                memoryLabelName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.memoryLabelName));
            }
        }

        public class NativeCallstackSymbolEntriesCache
        {
            public long Count;
            public DataArray.Cache<ulong> symbol;
            public DataArray.Cache<string> readableStackTrace;
            public SoaDataSet dataSet;
            public NativeCallstackSymbolEntriesCache(NativeCallstackSymbolEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                symbol = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.symbol));
                readableStackTrace = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.readableStackTrace));
            }
        }

        public class NativeAllocationEntriesCache
        {
            public long Count;
            public DataArray.Cache<int> memoryRegionIndex;
            public DataArray.Cache<long> rootReferenceId;
            public DataArray.Cache<long> allocationSiteId;
            public DataArray.Cache<ulong> address;
            public DataArray.Cache<ulong> size;
            public DataArray.Cache<int> overheadSize;
            public DataArray.Cache<int> paddingSize;
            public SoaDataSet dataSet;
            public NativeAllocationEntriesCache(NativeAllocationEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                memoryRegionIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.memoryRegionIndex));
                rootReferenceId = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.rootReferenceId));
                allocationSiteId = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.allocationSiteId));
                address = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.address));
                size = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.size));
                overheadSize = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.overheadSize));
                paddingSize = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.paddingSize));
            }
        }

        public class ManagedMemorySectionEntriesCache
        {
            ProfilerMarker cacheFind = new ProfilerMarker("ManagedMemorySectionEntriesCache.Find");
            public long Count;
            public byte[][] bytes;
            public ulong[] startAddress;
            ulong minAddress;
            ulong maxAddress;

            public BytesAndOffset Find(ulong address, VirtualMachineInformation virtualMachineInformation)
            {
                using (cacheFind.Auto())
                {
                    var bytesAndOffset = new BytesAndOffset();

                    if (address != 0 && address >= minAddress && address < maxAddress)
                    {
                        int idx = Array.BinarySearch(startAddress, address);
                        if (idx < 0)
                        {
                            idx = ~idx - 1;
                        }

                        if (address >= startAddress[idx] && address < (startAddress[idx] + (ulong)bytes[idx].Length))
                        {
                            bytesAndOffset.bytes = bytes[idx];
                            bytesAndOffset.offset = (int)(address - startAddress[idx]);
                            bytesAndOffset.pointerSize = virtualMachineInformation.pointerSize;
                        }
                    }

                    return bytesAndOffset;
                }
            }

            static void SortSectionEntries(ref ulong[] startAddresses, ref byte[][] associatedByteArrays)
            {
                var sortMapping = new int[startAddresses.Length];

                for (int i = 0; i < sortMapping.Length; ++i)
                    sortMapping[i] = i;

                var startAddrs = startAddresses;
                Array.Sort(sortMapping, (x, y) => startAddrs[x].CompareTo(startAddrs[y]));

                var newSortedAddresses = new ulong[startAddresses.Length];
                var newSortedByteArrays = new byte[startAddresses.Length][];

                for(int i = 0; i < startAddresses.Length; ++i)
                {
                    long idx = sortMapping[i];
                    newSortedAddresses[i] = startAddresses[idx];
                    newSortedByteArrays[i] = associatedByteArrays[idx];
                }

                startAddresses = newSortedAddresses;
                associatedByteArrays = newSortedByteArrays;
            }

            public ManagedMemorySectionEntriesCache(ManagedMemorySectionEntries sectionEntries)
            {
                Count = sectionEntries.GetNumEntries();
                if (Count > 0)
                {
                    startAddress = new ulong[Count];
                    //workaround using GetNumEntries instead of count due to limitations of internal API
                    sectionEntries.startAddress.GetEntries(0, sectionEntries.GetNumEntries(), ref startAddress);
                    bytes = new byte[Count][];
                    var cacheBytes = new byte[1][];
                    for (uint i = 0; i < Count; ++i)
                    {
                        sectionEntries.bytes.GetEntries(i, 1, ref cacheBytes);
                        bytes[i] = cacheBytes[0];
                    }

                    SortSectionEntries(ref startAddress, ref bytes);

                    minAddress = startAddress[0];
                    maxAddress = startAddress[Count - 1] + (ulong)bytes[Count - 1].LongLength;
                }
            }
        }

        public class GCHandleEntriesCache
        {
            public long Count;
            public DataArray.Cache<ulong> target;
            public SoaDataSet dataSet;
            public GCHandleEntriesCache(GCHandleEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                target = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.target));
            }
        }

        public class FieldDescriptionEntriesCache
        {
            public long Count;
            public DataArray.Cache<string> fieldDescriptionName;
            public DataArray.Cache<int> offset;
            public DataArray.Cache<int> typeIndex;
            public DataArray.Cache<bool> isStatic;
            public SoaDataSet dataSet;
            public FieldDescriptionEntriesCache(FieldDescriptionEntries ss)
            {
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
                fieldDescriptionName = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.fieldDescriptionName));
                offset = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.offset));
                typeIndex = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.typeIndex));
                isStatic = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.isStatic));
            }
        }

        public class ConnectionEntriesCache
        {
            public long Count;
            public DataArray.Cache<int> from { private set; get; }
            public DataArray.Cache<int> to { private set; get; }
            public SoaDataSet dataSet;
            public ConnectionEntriesCache(QueriedMemorySnapshot snap, bool connectionsNeedRemaping)
            {
                var ss = snap.connections;
                Count = ss.GetNumEntries();
                dataSet = new SoaDataSet(Count, kCacheEntrySize);
#if UNITY_2019_3_OR_NEWER
                if (connectionsNeedRemaping)
                {
                    var fromAPIArr = new int[Count];
                    ss.from.GetEntries(0, (uint)Count, ref fromAPIArr);
                    var toAPIArr = new int[Count];
                    ss.to.GetEntries(0, (uint)Count, ref toAPIArr);
                    var instanceIDArr = new int[snap.nativeObjects.instanceId.GetNumEntries()];
                    snap.nativeObjects.instanceId.GetEntries(0, snap.nativeObjects.instanceId.GetNumEntries(), ref instanceIDArr);
                    var gchandlesIndexArr = new int[snap.nativeObjects.gcHandleIndex.GetNumEntries()];
                    snap.nativeObjects.gcHandleIndex.GetEntries(0, snap.nativeObjects.gcHandleIndex.GetNumEntries(), ref gchandlesIndexArr);

                    Dictionary<int, int> instanceIDToIndex = new Dictionary<int, int>();
                    Dictionary<int, int> instanceIDToGcHandleIndex = new Dictionary<int, int>();

                    for (int i = 0; i < instanceIDArr.Length; ++i)
                    {
                        if(gchandlesIndexArr[i] != -1)
                        {
                            instanceIDToGcHandleIndex.Add(instanceIDArr[i], gchandlesIndexArr[i]);
                        }
                        instanceIDToIndex.Add(instanceIDArr[i], i);
                    }

                    var gcHandlesCount = snap.gcHandles.GetNumEntries();
                    int[] fromIndices = new int[Count + instanceIDToGcHandleIndex.Count];
                    int[] toIndices = new int[fromIndices.Length];

                    for (long i = 0; i < Count; ++i)
                    {
                        fromIndices[i] = (int)(gcHandlesCount + instanceIDToIndex[fromAPIArr[i]]);

                        //some native objects might have references to other native objects that are currently getting deleted or have been deleted
                        int instanceIDIDX = -1;
                        if(!instanceIDToIndex.TryGetValue(toAPIArr[i], out instanceIDIDX))
                        {
                            toIndices[i] = instanceIDIDX;
                        }
                        else
                            toIndices[i] = (int)(gcHandlesCount + instanceIDIDX);
                    }


                    var enumerator = instanceIDToGcHandleIndex.GetEnumerator();
                    for (long i = Count; i < fromIndices.Length; ++i)
                    {
                        enumerator.MoveNext();
                        fromIndices[i] = (int)(gcHandlesCount + instanceIDToIndex[enumerator.Current.Key]);
                        toIndices[i] = enumerator.Current.Value;
                    }

                    from = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(fromIndices));
                    to = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(toIndices));
                }
                else
#endif
                {
                    from = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.from));
                    to = DataArray.MakeCache(dataSet, DataSourceFromAPI.ApiToDatabase(ss.to));
                }

            }
        }
        
        public VirtualMachineInformation virtualMachineInformation { get; private set; }
        public NativeAllocationSiteEntriesCache nativeAllocationSites;
        public TypeDescriptionEntriesCache typeDescriptions;
        public NativeTypeEntriesCache nativeTypes;
        public NativeRootReferenceEntriesCache nativeRootReferences;
        public NativeObjectEntriesCache nativeObjects;
        public NativeMemoryRegionEntriesCache nativeMemoryRegions;
        public NativeMemoryLabelEntriesCache nativeMemoryLabels;
        public NativeCallstackSymbolEntriesCache nativeCallstackSymbols;
        public NativeAllocationEntriesCache nativeAllocations;
        public ManagedMemorySectionEntriesCache managedStacks;
        public ManagedMemorySectionEntriesCache managedHeapSections;
        public GCHandleEntriesCache gcHandles;
        public FieldDescriptionEntriesCache fieldDescriptions;
        public ConnectionEntriesCache connections;

        public SortedNativeMemoryRegionEntriesCache SortedNativeRegionsEntries;
        public SortedManagedMemorySectionEntriesCache SortedManagedStacksEntries;
        public SortedManagedMemorySectionEntriesCache SortedManagedHeapEntries;
        public SortedManagedObjectsCache SortedManagedObjects;
        public SortedNativeAllocationsCache SortedNativeAllocations;
        public SortedNativeObjectsCache SortedNativeObjects;
        
        public CachedSnapshot(QueriedMemorySnapshot s)
        {

            var vmInfo = s.virtualMachineInformation;
            if(!VMTools.ValidateVirtualMachineInfo(vmInfo))
            {
                throw new UnityException("Invalid VM info. Snapshot file is corrupted.");
            }

            virtualMachineInformation = vmInfo;
            packedMemorySnapshot = s;
            m_SnapshotVersion = s.version;
            nativeAllocationSites   = new NativeAllocationSiteEntriesCache(s.nativeAllocationSites);
            typeDescriptions        = new TypeDescriptionEntriesCache(s.typeDescriptions);
            nativeTypes             = new NativeTypeEntriesCache(s.nativeTypes);
            nativeRootReferences    = new NativeRootReferenceEntriesCache(s.nativeRootReferences);
            nativeObjects           = new NativeObjectEntriesCache(s.nativeObjects);
            nativeMemoryRegions     = new NativeMemoryRegionEntriesCache(s.nativeMemoryRegions);
            nativeMemoryLabels      = new NativeMemoryLabelEntriesCache(s.nativeMemoryLabels);
            nativeCallstackSymbols  = new NativeCallstackSymbolEntriesCache(s.nativeCallstackSymbols);
            nativeAllocations       = new NativeAllocationEntriesCache(s.nativeAllocations);
            managedStacks           = new ManagedMemorySectionEntriesCache(s.managedStacks);
            managedHeapSections     = new ManagedMemorySectionEntriesCache(s.managedHeapSections);
            gcHandles               = new GCHandleEntriesCache(s.gcHandles);
            fieldDescriptions       = new FieldDescriptionEntriesCache(s.fieldDescriptions);
            connections             = new ConnectionEntriesCache(s, HasConnectionOverhaul);

            SortedNativeRegionsEntries = new SortedNativeMemoryRegionEntriesCache(this);
            SortedManagedStacksEntries = new SortedManagedMemorySectionEntriesCache(managedStacks);
            SortedManagedHeapEntries   = new SortedManagedMemorySectionEntriesCache(managedHeapSections);

            SortedManagedObjects    = new SortedManagedObjectsCache(this);
            SortedNativeAllocations = new SortedNativeAllocationsCache(this);
            SortedNativeObjects     = new SortedNativeObjectsCache(this);

            CrawledData = new ManagedData(gcHandles.Count, connections.Count);

            typeDescriptions.InitSecondaryItems(this);
            nativeObjects.InitSecondaryItems();
            nativeObjects.InitSecondaryItems(this);
        }

        //Unified Object index are in that order: gcHandle, native object, crawled objects
        public int ManagedObjectIndexToUnifiedObjectIndex(int i)
        {
            if (i < 0) return -1;
            if (i < gcHandles.Count) return i;
            if (i < CrawledData.ManagedObjects.Count) return i + (int)nativeObjects.Count;
            return -1;
        }

        public int NativeObjectIndexToUnifiedObjectIndex(int i)
        {
            if (i < 0) return -1;
            if (i < nativeObjects.Count) return i + (int)gcHandles.Count;
            return -1;
        }

        public int UnifiedObjectIndexToManagedObjectIndex(int i)
        {
            if (i < 0) return -1;
            if (i < gcHandles.Count) return i;
            int firstCrawled = (int)(gcHandles.Count + nativeObjects.Count);
            int lastCrawled = (int)(nativeObjects.Count + CrawledData.ManagedObjects.Count);
            if (i >= firstCrawled && i < lastCrawled) return i - (int)nativeObjects.Count;
            return -1;
        }

        public int UnifiedObjectIndexToNativeObjectIndex(int i)
        {
            if (i < gcHandles.Count) return -1;
            int firstCrawled = (int)(gcHandles.Count + nativeObjects.Count);
            if (i < firstCrawled) return i - (int)gcHandles.Count;
            return -1;
        }

        public interface ISortedEntriesCache
        {
            void Preload();
            int Count { get; }
            ulong Address(int index);
            ulong Size(int index);
        }

        public class SortedNativeMemoryRegionEntriesCache : ISortedEntriesCache
        {
            CachedSnapshot m_Snapshot;
            int[] m_Sorting;

            public SortedNativeMemoryRegionEntriesCache(CachedSnapshot snapshot)
            {
                m_Snapshot = snapshot;
            }

            public void Preload()
            {
                if (m_Sorting == null)
                {
                    m_Sorting = new int[m_Snapshot.nativeMemoryRegions.Count];

                    for (int i = 0; i < m_Sorting.Length; ++i)
                        m_Sorting[i] = i;

                    Array.Sort(m_Sorting, (x, y) => m_Snapshot.nativeMemoryRegions.addressBase[x].CompareTo(m_Snapshot.nativeMemoryRegions.addressBase[y]));
                }
            }

            int this[int index]
            {
                get
                {
                    Preload();
                    return m_Sorting[index];
                }
            }

            public int  Count { get { return (int)m_Snapshot.nativeMemoryRegions.Count; } }
            public ulong  Address(int index) { return m_Snapshot.nativeMemoryRegions.addressBase[this[index]]; }
            public ulong  Size(int index) { return m_Snapshot.nativeMemoryRegions.addressSize[this[index]]; }

            public string Name(int index) { return m_Snapshot.nativeMemoryRegions.memoryRegionName[this[index]]; }
            public int    UnsortedParentRegionIndex(int index) { return m_Snapshot.nativeMemoryRegions.parentIndex[this[index]]; }
            public int    UnsortedFirstAllocationIndex(int index) { return m_Snapshot.nativeMemoryRegions.firstAllocationIndex[this[index]]; }
            public int    UnsortedNumAllocations(int index) { return m_Snapshot.nativeMemoryRegions.numAllocations[this[index]]; }
        }

        //TODO: unify with the other old section entries as those are sorted by default now
        public class SortedManagedMemorySectionEntriesCache : ISortedEntriesCache
        {
            ManagedMemorySectionEntriesCache m_Entries;

            public SortedManagedMemorySectionEntriesCache(ManagedMemorySectionEntriesCache entries)
            {
                m_Entries = entries;
            }

            public void Preload()
            {
                //Dummy for the interface
            }

            public int  Count { get { return (int)m_Entries.Count; } }
            public ulong Address(int index) { return m_Entries.startAddress[index]; }
            public ulong Size(int index) { return (ulong)m_Entries.bytes[index].Length; }
            public byte[] Bytes(int index) { return m_Entries.bytes[index]; }
        }

        public class SortedManagedObjectsCache : ISortedEntriesCache
        {
            CachedSnapshot m_Snapshot;
            int[] m_Sorting;

            public SortedManagedObjectsCache(CachedSnapshot snapshot)
            {
                m_Snapshot = snapshot;
            }

            public void Preload()
            {
                if (m_Sorting == null)
                {
                    m_Sorting = new int[m_Snapshot.CrawledData.ManagedObjects.Count];

                    for (int i = 0; i < m_Sorting.Length; ++i)
                        m_Sorting[i] = i;

                    Array.Sort(m_Sorting, (x, y) => m_Snapshot.CrawledData.ManagedObjects[x].PtrObject.CompareTo(m_Snapshot.CrawledData.ManagedObjects[y].PtrObject));
                }
            }

            ManagedObjectInfo this[int index]
            {
                get
                {
                    Preload();
                    return m_Snapshot.CrawledData.ManagedObjects[m_Sorting[index]];
                }
            }

            public int  Count { get { return (int)m_Snapshot.CrawledData.ManagedObjects.Count; } }

            public ulong Address(int index) { return this[index].PtrObject; }
            public ulong Size(int index) { return (ulong)this[index].Size; }
        }

        public class SortedNativeAllocationsCache : ISortedEntriesCache
        {
            CachedSnapshot m_Snapshot;
            int[] m_Sorting;

            public SortedNativeAllocationsCache(CachedSnapshot snapshot)
            {
                m_Snapshot = snapshot;
            }

            public void Preload()
            {
                if (m_Sorting == null)
                {
                    m_Sorting = new int[m_Snapshot.nativeAllocations.address.Length];

                    for (int i = 0; i < m_Sorting.Length; ++i)
                        m_Sorting[i] = i;

                    Array.Sort(m_Sorting, (x, y) => m_Snapshot.nativeAllocations.address[x].CompareTo(m_Snapshot.nativeAllocations.address[y]));
                }
            }

            int this[int index]
            {
                get
                {
                    Preload();
                    return m_Sorting[index];
                }
            }

            public int  Count { get { return (int)m_Snapshot.nativeAllocations.Count; } }
            public ulong Address(int index) { return m_Snapshot.nativeAllocations.address[this[index]]; }
            public ulong Size(int index) { return m_Snapshot.nativeAllocations.size[this[index]]; }
            public int MemoryRegionIndex(int index) { return m_Snapshot.nativeAllocations.memoryRegionIndex[this[index]]; }
            public long RootReferenceId(int index) { return m_Snapshot.nativeAllocations.rootReferenceId[this[index]]; }
            public long AllocationSiteId(int index) { return m_Snapshot.nativeAllocations.allocationSiteId[this[index]]; }
            public int OverheadSize(int index) { return m_Snapshot.nativeAllocations.overheadSize[this[index]]; }
            public int PaddingSize(int index) { return m_Snapshot.nativeAllocations.paddingSize[this[index]]; }
        }

        public class SortedNativeObjectsCache : ISortedEntriesCache
        {
            CachedSnapshot m_Snapshot;
            int[] m_Sorting;

            public SortedNativeObjectsCache(CachedSnapshot snapshot)
            {
                m_Snapshot = snapshot;
            }

            public void Preload()
            {
                if (m_Sorting == null)
                {
                    m_Sorting = new int[m_Snapshot.nativeObjects.nativeObjectAddress.Length];

                    for (int i = 0; i < m_Sorting.Length; ++i)
                        m_Sorting[i] = i;

                    Array.Sort(m_Sorting, (x, y) => m_Snapshot.nativeObjects.nativeObjectAddress[x].CompareTo(m_Snapshot.nativeObjects.nativeObjectAddress[y]));
                }
            }

            int this[int index]
            {
                get
                {
                    Preload();
                    return m_Sorting[index];
                }
            }

            public int  Count { get { return (int)m_Snapshot.nativeObjects.Count; } }
            public ulong Address(int index) { return m_Snapshot.nativeObjects.nativeObjectAddress[this[index]]; }
            public ulong Size(int index) { return m_Snapshot.nativeObjects.size[this[index]]; }
            public string Name(int index) { return m_Snapshot.nativeObjects.objectName[this[index]]; }
            public int InstanceId(int index) { return m_Snapshot.nativeObjects.instanceId[this[index]]; }
            public int NativeTypeArrayIndex(int index) { return m_Snapshot.nativeObjects.nativeTypeArrayIndex[this[index]]; }
            public HideFlags HideFlags(int index) { return m_Snapshot.nativeObjects.hideFlags[this[index]]; }
            public ObjectFlags Flags(int index) { return m_Snapshot.nativeObjects.flags[this[index]]; }
            public long RootReferenceId(int index) { return m_Snapshot.nativeObjects.rootReferenceId[this[index]]; }
            public int Refcount(int index) { return m_Snapshot.nativeObjects.refcount[this[index]]; }
            public int ManagedObjectIndex(int index) { return m_Snapshot.nativeObjects.managedObjectIndex[this[index]]; }
        }
    }
}
