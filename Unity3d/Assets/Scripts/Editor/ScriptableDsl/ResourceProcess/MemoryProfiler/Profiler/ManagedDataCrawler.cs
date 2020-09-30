using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.MemoryProfilerForExtension.Editor.Containers;
using Unity.MemoryProfilerForExtension.Editor.Format;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal struct ManagedConnection
    {
        public enum ConnectionType
        {
            Global_To_ManagedObject,
            ManagedObject_To_ManagedObject,
            ManagedType_To_ManagedObject,
            UnityEngineObject,
        }
        public ManagedConnection(ConnectionType t, int from, int to, int fieldFrom, int arrayIndexFrom)
        {
            connectionType = t;
            index0 = from;
            index1 = to;
            this.fieldFrom = fieldFrom;
            this.arrayIndexFrom = arrayIndexFrom;
        }

        private int index0;
        private int index1;

        public int fieldFrom;
        public int arrayIndexFrom;

        public ConnectionType connectionType;
        public int GetUnifiedIndexFrom(CachedSnapshot snapshot)
        {
            switch (connectionType)
            {
                case ConnectionType.ManagedObject_To_ManagedObject:
                    return snapshot.ManagedObjectIndexToUnifiedObjectIndex(index0);
                case ConnectionType.ManagedType_To_ManagedObject:
                    return index0;
                case ConnectionType.UnityEngineObject:
                    return snapshot.NativeObjectIndexToUnifiedObjectIndex(index0);
                default:
                    return -1;
            }
        }

        public int GetUnifiedIndexTo(CachedSnapshot snapshot)
        {
            switch (connectionType)
            {
                case ConnectionType.Global_To_ManagedObject:
                case ConnectionType.ManagedObject_To_ManagedObject:
                case ConnectionType.ManagedType_To_ManagedObject:
                case ConnectionType.UnityEngineObject:
                    return snapshot.ManagedObjectIndexToUnifiedObjectIndex(index1);
                default:
                    return -1;
            }
        }

        public int fromManagedObjectIndex
        {
            get
            {
                switch (connectionType)
                {
                    case ConnectionType.Global_To_ManagedObject:
                    case ConnectionType.ManagedObject_To_ManagedObject:
                    case ConnectionType.ManagedType_To_ManagedObject:
                        return index0;
                }
                return -1;
            }
        }
        public int toManagedObjectIndex
        {
            get
            {
                switch (connectionType)
                {
                    case ConnectionType.Global_To_ManagedObject:
                    case ConnectionType.ManagedObject_To_ManagedObject:
                    case ConnectionType.ManagedType_To_ManagedObject:
                        return index1;
                }
                return -1;
            }
        }

        public int fromManagedType
        {
            get
            {
                if (connectionType == ConnectionType.ManagedType_To_ManagedObject)
                {
                    return index0;
                }
                return -1;
            }
        }
        public int UnityEngineNativeObjectIndex
        {
            get
            {
                if (connectionType == ConnectionType.UnityEngineObject)
                {
                    return index0;
                }
                return -1;
            }
        }
        public int UnityEngineManagedObjectIndex
        {
            get
            {
                if (connectionType == ConnectionType.UnityEngineObject)
                {
                    return index1;
                }
                return -1;
            }
        }
        public static ManagedConnection MakeUnityEngineObjectConnection(int NativeIndex, int ManagedIndex)
        {
            return new ManagedConnection(ConnectionType.UnityEngineObject, NativeIndex, ManagedIndex, 0, 0);
        }

        public static ManagedConnection MakeConnection(CachedSnapshot snapshot, int fromIndex, ulong fromPtr, int toIndex, ulong toPtr, int fromTypeIndex, int fromField, int fieldArrayIndexFrom)
        {
            if (fromIndex >= 0)
            {
                //from an object
#if DEBUG_VALIDATION
                if (fromField >= 0)
                {
                    if (snapshot.fieldDescriptions.isStatic[fromField])
                    {
                        Debug.LogError("Cannot make a connection from an object using a static field.");
                    }
                }
#endif
                return new ManagedConnection(ConnectionType.ManagedObject_To_ManagedObject, fromIndex, toIndex, fromField, fieldArrayIndexFrom);
            }
            else if (fromTypeIndex >= 0)
            {
                //from a type static data
#if DEBUG_VALIDATION
                if (fromField >= 0)
                {
                    if (!snapshot.fieldDescriptions.isStatic[fromField])
                    {
                        Debug.LogError("Cannot make a connection from a type using a non-static field.");
                    }
                }
#endif
                return new ManagedConnection(ConnectionType.ManagedType_To_ManagedObject, fromTypeIndex, toIndex, fromField, fieldArrayIndexFrom);
            }
            else
            {
                return new ManagedConnection(ConnectionType.Global_To_ManagedObject, fromIndex, toIndex, fromField, fieldArrayIndexFrom);
            }
        }
    }

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    internal struct ManagedObjectInfo
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public ulong PtrObject;
        public ulong PtrTypeInfo;
        public int NativeObjectIndex;
        public int ManagedObjectIndex;
        public int ITypeDescription;
        public int Size;
        public int RefCount;

        public bool IsKnownType()
        {
            return ITypeDescription >= 0;
        }

        public BytesAndOffset data;

        public bool IsValid()
        {
            return PtrObject != 0 && PtrTypeInfo != 0 && data.bytes != null;
        }

        public static bool operator==(ManagedObjectInfo lhs, ManagedObjectInfo rhs)
        {
            return lhs.PtrObject == rhs.PtrObject
                && lhs.PtrTypeInfo == rhs.PtrTypeInfo
                && lhs.NativeObjectIndex == rhs.NativeObjectIndex
                && lhs.ManagedObjectIndex == rhs.ManagedObjectIndex
                && lhs.ITypeDescription == rhs.ITypeDescription
                && lhs.Size == rhs.Size
                && lhs.RefCount == rhs.RefCount;
        }

        public static bool operator!=(ManagedObjectInfo lhs, ManagedObjectInfo rhs)
        {
            return !(lhs == rhs);
        }
    }

    internal class ManagedData
    {
        const int k_ManagedObjectBlockSize = 32768;
        const int k_ManagedConnectionsBlockSize = 65536;
        public BlockList<ManagedObjectInfo> ManagedObjects { private set; get; }
        public SortedDictionary<ulong, int> MangedObjectIndexByAddress { private set; get; }
        public BlockList<ManagedConnection> Connections { private set; get; }

        public ManagedData(long rawGcHandleCount, long rawConnectionsCount)
        {
            //compute initial block counts for larger snapshots
            ManagedObjects = new BlockList<ManagedObjectInfo>(k_ManagedObjectBlockSize, rawGcHandleCount);
            Connections = new BlockList<ManagedConnection>(k_ManagedConnectionsBlockSize, rawConnectionsCount);

            MangedObjectIndexByAddress = new SortedDictionary<ulong, int>();
        }
    }

    internal struct BytesAndOffset
    {
        public byte[] bytes;
        public int offset;
        public int pointerSize;
        public bool IsValid { get { return bytes != null; } }
        public BytesAndOffset(byte[] bytes, int pointerSize)
        {
            this.bytes = bytes;
            this.pointerSize = pointerSize;
            offset = 0;
        }

        public enum PtrReadError
        {
            Success,
            OutOfBounds,
            InvalidPtrSize
        }

        public PtrReadError TryReadPointer(out ulong ptr)
        {
            ptr = unchecked(0xffffffffffffffff);

            if (offset + pointerSize > bytes.Length)
                return PtrReadError.OutOfBounds;

            switch (pointerSize)
            {
                case VMTools.x64ArchPtrSize:
                    ptr = BitConverter.ToUInt64(bytes, offset);
                    return PtrReadError.Success;
                case VMTools.x86ArchPtrSize:
                    ptr = BitConverter.ToUInt32(bytes, offset);
                    return PtrReadError.Success;
                default: //should never happen
                    return PtrReadError.InvalidPtrSize;
            }
        }

        public byte ReadByte()
        {
            return bytes[offset];
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(bytes, offset);
        }

        public Int32 ReadInt32()
        {
            return BitConverter.ToInt32(bytes, offset);
        }

        public Int64 ReadInt64()
        {
            return BitConverter.ToInt64(bytes, offset);
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(bytes, offset);
        }

        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(bytes, offset);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(bytes, offset);
        }

        public bool ReadBoolean()
        {
            return BitConverter.ToBoolean(bytes, offset);
        }

        public char ReadChar()
        {
            return BitConverter.ToChar(bytes, offset);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(bytes, offset);
        }

        public float ReadSingle()
        {
            return BitConverter.ToSingle(bytes, offset);
        }

        public string ReadString()
        {
            int strLength = ReadInt32();
            if(offset + sizeof(int) + (strLength * 2) > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("Attempted to read outside of binary buffer.");
            }
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    string str = null;
                    char* begin = (char*)(ptr + (offset + sizeof(int)));
                    str = new string(begin, 0, strLength);

                    return str;
                }
            }
        }

        public BytesAndOffset Add(int add)
        {
            return new BytesAndOffset() { bytes = bytes, offset = offset + add, pointerSize = pointerSize };
        }

        public void WritePointer(UInt64 value)
        {
            for (int i = 0; i < pointerSize; i++)
            {
                bytes[i + offset] = (byte)value;
                value >>= 8;
            }
        }

        public BytesAndOffset NextPointer()
        {
            return Add(pointerSize);
        }
    }

    internal static class Crawler
    {
        struct StackCrawlData
        {
            public ulong ptr;
            public ulong ptrFrom;
            public int typeFrom;
            public int indexOfFrom;
            public int fieldFrom;
            public int fromArrayIndex;
        }

        class IntermediateCrawlData
        {
            public List<int> TypesWithStaticFields { private set; get; }
            public Stack<StackCrawlData> CrawlDataStack { private set; get; }
            public BlockList<ManagedObjectInfo> ManagedObjectInfos { get { return CachedMemorySnapshot.CrawledData.ManagedObjects; } }
            public BlockList<ManagedConnection> ManagedConnections { get { return CachedMemorySnapshot.CrawledData.Connections; } }
            public CachedSnapshot CachedMemorySnapshot { private set; get; }
            public Stack<int> DuplicatedGCHandleTargetsStack { private set; get; }
            const int kInitialStackSize = 256;
            public IntermediateCrawlData(CachedSnapshot snapshot)
            {
                DuplicatedGCHandleTargetsStack = new Stack<int>(kInitialStackSize);
                CachedMemorySnapshot = snapshot;
                CrawlDataStack = new Stack<StackCrawlData>();

                TypesWithStaticFields = new List<int>();
                for (long i = 0; i != snapshot.typeDescriptions.Count; ++i)
                {
                    if (snapshot.typeDescriptions.staticFieldBytes[i] != null
                        && snapshot.typeDescriptions.staticFieldBytes[i].Length > 0)
                    {
                        TypesWithStaticFields.Add(snapshot.typeDescriptions.typeIndex[i]);
                    }
                }
            }
        }

        static void GatherIntermediateCrawlData(CachedSnapshot snapshot, IntermediateCrawlData crawlData)
        {
            unsafe
            {
                var uniqueHandlesPtr = (ulong*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<ulong>() * snapshot.gcHandles.Count, UnsafeUtility.AlignOf<ulong>(), Collections.Allocator.Temp);

                ulong* uniqueHandlesBegin = uniqueHandlesPtr;
                int writtenRange = 0;

                // Parse all handles
                for (int i = 0; i != snapshot.gcHandles.Count; i++)
                {
                    var moi = new ManagedObjectInfo();
                    var target = snapshot.gcHandles.target[i];

                    moi.ManagedObjectIndex = i;

                    //this can only happen pre 19.3 scripting snapshot implementations where we dumped all handle targets but not the handles.
                    //Eg: multiple handles can have the same target. Future facing we need to start adding that as we move forward
                    if (snapshot.CrawledData.MangedObjectIndexByAddress.ContainsKey(target))
                    {
                        moi.PtrObject = target;
                        crawlData.DuplicatedGCHandleTargetsStack.Push(i);
                    }
                    else
                    {
                        snapshot.CrawledData.MangedObjectIndexByAddress.Add(target, moi.ManagedObjectIndex);
                        *(uniqueHandlesBegin++) = target;
                        ++writtenRange;
                    }

                    crawlData.ManagedObjectInfos.Add(moi);
                }
                uniqueHandlesBegin = uniqueHandlesPtr; //reset iterator
                ulong* uniqueHandlesEnd = uniqueHandlesPtr + writtenRange;
                //add handles for processing
                while (uniqueHandlesBegin != uniqueHandlesEnd)
                {
                    crawlData.CrawlDataStack.Push(new StackCrawlData { ptr = UnsafeUtility.ReadArrayElement<ulong>(uniqueHandlesBegin++, 0), ptrFrom = 0, typeFrom = -1, indexOfFrom = -1, fieldFrom = -1, fromArrayIndex = -1 });
                }
                UnsafeUtility.Free(uniqueHandlesPtr, Collections.Allocator.Temp);
            }
        }

        public static IEnumerator Crawl(CachedSnapshot snapshot)
        {
            const int stepCount = 5;
            var status = new EnumerationUtilities.EnumerationStatus(stepCount);

            IntermediateCrawlData crawlData = new IntermediateCrawlData(snapshot);

            //Gather handles and duplicates
            status.StepStatus = "Gathering snapshot managed data.";
            yield return status;
            GatherIntermediateCrawlData(snapshot, crawlData);

            //crawl handle data
            status.IncrementStep();
            status.StepStatus = "Crawling GC handles.";
            yield return status;
            while (crawlData.CrawlDataStack.Count > 0)
            {
                CrawlPointer(crawlData);
            }

            //crawl data pertaining to types with static fields and enqueue any heap objects
            status.IncrementStep();
            status.StepStatus = "Crawling data types with static fields";
            yield return status;
            for (int i = 0; i < crawlData.TypesWithStaticFields.Count; i++)
            {
                var iTypeDescription = crawlData.TypesWithStaticFields[i];
                var bytesOffset = new BytesAndOffset { bytes = snapshot.typeDescriptions.staticFieldBytes[iTypeDescription], offset = 0, pointerSize = snapshot.virtualMachineInformation.pointerSize };
                CrawlRawObjectData(crawlData, bytesOffset, iTypeDescription, true, 0, -1);
            }

            //crawl handles belonging to static instances
            status.IncrementStep();
            status.StepStatus = "Crawling static instances heap data.";
            yield return status;
            while (crawlData.CrawlDataStack.Count > 0)
            {
                CrawlPointer(crawlData);
            }

            //copy crawled object source data for duplicate objects
            foreach (var i in crawlData.DuplicatedGCHandleTargetsStack)
            {
                var ptr = snapshot.CrawledData.ManagedObjects[i].PtrObject;
                snapshot.CrawledData.ManagedObjects[i] = snapshot.CrawledData.ManagedObjects[snapshot.CrawledData.MangedObjectIndexByAddress[ptr]];
            }
            
            //crawl connection data
            status.IncrementStep();
            status.StepStatus = "Crawling connection data";
            yield return status;
            ConnectNativeToManageObject(crawlData);
            AddupRawRefCount(crawlData.CachedMemorySnapshot);
        }

        static void AddupRawRefCount(CachedSnapshot snapshot)
        {
            for (long i = 0; i != snapshot.connections.Count; ++i)
            {
                int iManagedTo = snapshot.UnifiedObjectIndexToManagedObjectIndex(snapshot.connections.to[i]);
                if (iManagedTo >= 0)
                {
                    var obj = snapshot.CrawledData.ManagedObjects[iManagedTo];
                    ++obj.RefCount;
                    snapshot.CrawledData.ManagedObjects[iManagedTo] = obj;
                    continue;
                }

                int iNativeTo = snapshot.UnifiedObjectIndexToNativeObjectIndex(snapshot.connections.to[i]);
                if (iNativeTo >= 0)
                {
                    ++snapshot.nativeObjects.refcount[iNativeTo];
                    continue;
                }
            }
        }

        static void ConnectNativeToManageObject(IntermediateCrawlData crawlData)
        {
            var snapshot = crawlData.CachedMemorySnapshot;
            var objectInfos = crawlData.ManagedObjectInfos;

            if (snapshot.typeDescriptions.Count == 0)
                return;

            // Get UnityEngine.Object
            int iTypeDescription_UnityEngineObject = snapshot.typeDescriptions.typeDescriptionName.FindIndex(x => x == "UnityEngine.Object");
#if DEBUG_VALIDATION //This shouldn't really happen
            if (iTypeDescription_UnityEngineObject < 0)
            {
               throw new Exception("Unable to find UnityEngine.Object");
            }
#endif
            int cachedPtrOffset = -1;
            int iField_UnityEngineObject_m_CachedPtr = Array.FindIndex(
                snapshot.typeDescriptions.fieldIndices[iTypeDescription_UnityEngineObject]
                , iField => snapshot.fieldDescriptions.fieldDescriptionName[iField] == "m_CachedPtr");

            if (iField_UnityEngineObject_m_CachedPtr >= 0)
            {
                cachedPtrOffset = snapshot.fieldDescriptions.offset[iField_UnityEngineObject_m_CachedPtr];
            }

#if DEBUG_VALIDATION
            if (cachedPtrOffset < 0)
            {
                Debug.LogWarning("Could not find unity object instance id field or m_CachedPtr");
                return;
            }
#endif

            for (int i = 0; i != objectInfos.Count; i++)
            {
                //Must derive of unity Object
                var objectInfo = objectInfos[i];
                objectInfo.NativeObjectIndex = -1;
                int instanceID = CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone;

                if (DerivesFrom(snapshot.typeDescriptions, objectInfo.ITypeDescription, iTypeDescription_UnityEngineObject))
                {
                    var heapSection = snapshot.managedHeapSections.Find(objectInfo.PtrObject + (ulong)cachedPtrOffset, snapshot.virtualMachineInformation);
                    if (!heapSection.IsValid)
                    {
                        Debug.LogWarning("Managed object (addr:" + objectInfo.PtrObject + ", index:" + objectInfo.ManagedObjectIndex + ") does not have data at cachedPtr offset(" + cachedPtrOffset + ")");
                    }
                    else
                    {
                        ulong cachedPtr;
                        heapSection.TryReadPointer(out cachedPtr);

                        if (!snapshot.nativeObjects.nativeObjectAddressToInstanceId.TryGetValue(cachedPtr, out instanceID))
                            instanceID = CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone;
                    }

                    if (instanceID != CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone && snapshot.nativeObjects.instanceId2Index.TryGetValue(instanceID, out objectInfo.NativeObjectIndex))
                    {
                        snapshot.nativeObjects.managedObjectIndex[objectInfo.NativeObjectIndex] = i;
                    }
                }

                objectInfos[i] = objectInfo;
                
                if (snapshot.HasConnectionOverhaul && instanceID != CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone)
                {
                    snapshot.CrawledData.Connections.Add(ManagedConnection.MakeUnityEngineObjectConnection(objectInfo.NativeObjectIndex, objectInfo.ManagedObjectIndex));
                    ++snapshot.nativeObjects.refcount[objectInfo.NativeObjectIndex];
                }
            }
        }

        static bool DerivesFrom(CachedSnapshot.TypeDescriptionEntriesCache typeDescriptions, int iTypeDescription, int potentialBase)
        {
            if (iTypeDescription < 0) return false;
            if (iTypeDescription == potentialBase)
                return true;

            var baseIndex = typeDescriptions.baseOrElementTypeIndex[iTypeDescription];

            if (baseIndex < 0)
                return false;
            var baseArrayIndex = typeDescriptions.TypeIndex2ArrayIndex(baseIndex);
            return DerivesFrom(typeDescriptions, baseArrayIndex, potentialBase);
        }

        static void CrawlRawObjectData(IntermediateCrawlData crawlData, BytesAndOffset bytesAndOffset, int iTypeDescription, bool useStaticFields, ulong ptrFrom, int indexOfFrom)
        {
            var snapshot = crawlData.CachedMemorySnapshot;

            var fields = useStaticFields ? snapshot.typeDescriptions.fieldIndicesOwned_static[iTypeDescription] : snapshot.typeDescriptions.fieldIndices_instance[iTypeDescription];
            foreach (var iField in fields)
            {
                int iField_TypeDescription_TypeIndex = snapshot.fieldDescriptions.typeIndex[iField];
                int iField_TypeDescription_ArrayIndex = snapshot.typeDescriptions.TypeIndex2ArrayIndex(iField_TypeDescription_TypeIndex);

                var fieldLocation = bytesAndOffset.Add(snapshot.fieldDescriptions.offset[iField] - (useStaticFields ? 0 : snapshot.virtualMachineInformation.objectHeaderSize));

                if (snapshot.typeDescriptions.HasFlag(iField_TypeDescription_ArrayIndex, TypeFlags.kValueType))
                {
                    CrawlRawObjectData(crawlData, fieldLocation, iField_TypeDescription_ArrayIndex, useStaticFields, ptrFrom, indexOfFrom);
                    continue;
                }


                ulong fieldAddr;
                if(fieldLocation.TryReadPointer(out fieldAddr) == BytesAndOffset.PtrReadError.Success)
                {
                    crawlData.CrawlDataStack.Push(new StackCrawlData() { ptr = fieldAddr, ptrFrom = ptrFrom, typeFrom = iTypeDescription, indexOfFrom = indexOfFrom, fieldFrom = iField, fromArrayIndex = -1 });
                }
            }
        }

        static bool CrawlPointer(IntermediateCrawlData dataStack)
        {
            UnityEngine.Debug.Assert(dataStack.CrawlDataStack.Count > 0);

            var snapshot = dataStack.CachedMemorySnapshot;
            var typeDescriptions = snapshot.typeDescriptions;
            var data = dataStack.CrawlDataStack.Pop();
            var virtualMachineInformation = snapshot.virtualMachineInformation;
            var managedHeapSections = snapshot.managedHeapSections;
            var byteOffset = managedHeapSections.Find(data.ptr, virtualMachineInformation);

            if (!byteOffset.IsValid)
            {
                return false;
            }

            ManagedObjectInfo obj;
            bool wasAlreadyCrawled;

            obj = ParseObjectHeader(snapshot, data.ptr, out wasAlreadyCrawled, false);
            ++obj.RefCount;

            snapshot.CrawledData.ManagedObjects[obj.ManagedObjectIndex] = obj;
            snapshot.CrawledData.MangedObjectIndexByAddress[obj.PtrObject] = obj.ManagedObjectIndex;

            dataStack.ManagedConnections.Add(ManagedConnection.MakeConnection(snapshot, data.indexOfFrom, data.ptrFrom, obj.ManagedObjectIndex, data.ptr, data.typeFrom, data.fieldFrom, data.fromArrayIndex));

            if (!obj.IsKnownType())
                return false;
            if (wasAlreadyCrawled)
                return true;

            if (!typeDescriptions.HasFlag(obj.ITypeDescription, TypeFlags.kArray))
            {
                CrawlRawObjectData(dataStack, byteOffset.Add(snapshot.virtualMachineInformation.objectHeaderSize), obj.ITypeDescription, false, data.ptr, obj.ManagedObjectIndex);
                return true;
            }

            var arrayLength = ArrayTools.ReadArrayLength(snapshot, data.ptr, obj.ITypeDescription);
            int iElementTypeDescription = typeDescriptions.baseOrElementTypeIndex[obj.ITypeDescription];
            if (iElementTypeDescription == -1)
            {
                return false; //do not crawl uninitialized object types, as we currently don't have proper handling for these
            }
            var arrayData = byteOffset.Add(virtualMachineInformation.arrayHeaderSize);
            for (int i = 0; i != arrayLength; i++)
            {
                if (typeDescriptions.HasFlag(iElementTypeDescription, TypeFlags.kValueType))
                {
                    CrawlRawObjectData(dataStack, arrayData, iElementTypeDescription, false, data.ptr, obj.ManagedObjectIndex);
                    arrayData = arrayData.Add(typeDescriptions.size[iElementTypeDescription]);
                }
                else
                {
                    ulong arrayDataPtr;
                    if (arrayData.TryReadPointer(out arrayDataPtr) != BytesAndOffset.PtrReadError.Success)
                        return false;

                    dataStack.CrawlDataStack.Push(new StackCrawlData() { ptr = arrayDataPtr, ptrFrom = data.ptr, typeFrom = obj.ITypeDescription, indexOfFrom = obj.ManagedObjectIndex, fieldFrom = -1, fromArrayIndex = i });
                    arrayData = arrayData.NextPointer();

                }
            }
            return true;
        }

        static int SizeOfObjectInBytes(CachedSnapshot snapshot, int iTypeDescription, BytesAndOffset bo, ulong address)
        {
            if (iTypeDescription < 0) return 0;

            if (snapshot.typeDescriptions.HasFlag(iTypeDescription, TypeFlags.kArray))
                return ArrayTools.ReadArrayObjectSizeInBytes(snapshot, address, iTypeDescription);

            if (snapshot.typeDescriptions.typeDescriptionName[iTypeDescription] == "System.String")
                return StringTools.ReadStringObjectSizeInBytes(bo, snapshot.virtualMachineInformation);

            //array and string are the only types that are special, all other types just have one size, which is stored in the type description
            return snapshot.typeDescriptions.size[iTypeDescription];
        }

        static int SizeOfObjectInBytes(CachedSnapshot snapshot, int iTypeDescription, BytesAndOffset byteOffset, CachedSnapshot.ManagedMemorySectionEntriesCache heap)
        {
            if (iTypeDescription < 0) return 0;

            if (snapshot.typeDescriptions.HasFlag(iTypeDescription, TypeFlags.kArray))
                return ArrayTools.ReadArrayObjectSizeInBytes(snapshot, byteOffset, iTypeDescription);

            if (snapshot.typeDescriptions.typeDescriptionName[iTypeDescription] == "System.String")
                return StringTools.ReadStringObjectSizeInBytes(byteOffset, snapshot.virtualMachineInformation);

            //array and string are the only types that are special, all other types just have one size, which is stored in the type description
            return snapshot.typeDescriptions.size[iTypeDescription];
        }

        static ManagedObjectInfo ParseObjectHeader(CachedSnapshot snapshot, ulong ptrObjectHeader, out bool wasAlreadyCrawled, bool ignoreBadHeaderError)
        {
            var objectList = snapshot.CrawledData.ManagedObjects;
            var objectsByAddress = snapshot.CrawledData.MangedObjectIndexByAddress;

            ManagedObjectInfo objectInfo = default(ManagedObjectInfo);
            int idx = 0;
            if (!snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(ptrObjectHeader, out idx))
            {
                objectInfo = ParseObjectHeader(snapshot, ptrObjectHeader, ignoreBadHeaderError);
                objectInfo.ManagedObjectIndex = (int)objectList.Count;
                objectList.Add(objectInfo);
                objectsByAddress.Add(ptrObjectHeader, objectInfo.ManagedObjectIndex);
                wasAlreadyCrawled = false;
                return objectInfo;
            }

            objectInfo = snapshot.CrawledData.ManagedObjects[idx];
            // this happens on objects from gcHandles, they are added before any other crawled object but have their ptr set to 0.
            if (objectInfo.PtrObject == 0)
            {
                idx = objectInfo.ManagedObjectIndex;
                objectInfo = ParseObjectHeader(snapshot, ptrObjectHeader, ignoreBadHeaderError);
                objectInfo.ManagedObjectIndex = idx;
                objectList[idx] = objectInfo;
                objectsByAddress[ptrObjectHeader] = idx;

                wasAlreadyCrawled = false;
                return objectInfo;
            }

            wasAlreadyCrawled = true;
            return objectInfo;
        }

        public static ManagedObjectInfo ParseObjectHeader(CachedSnapshot snapshot, ulong ptrObjectHeader, bool ignoreBadHeaderError)
        {
            var heap = snapshot.managedHeapSections;
            var boHeader = heap.Find(ptrObjectHeader, snapshot.virtualMachineInformation);
            var objectInfo = ParseObjectHeader(snapshot, boHeader, ignoreBadHeaderError);
            objectInfo.PtrObject = ptrObjectHeader;
            return objectInfo;
        }

        public static ManagedObjectInfo ParseObjectHeader(CachedSnapshot snapshot, BytesAndOffset byteOffset, bool ignoreBadHeaderError)
        {
            var heap = snapshot.managedHeapSections;
            ManagedObjectInfo objectInfo;

            objectInfo = new ManagedObjectInfo();
            objectInfo.PtrObject = 0;
            objectInfo.ManagedObjectIndex = -1;

            var boHeader = byteOffset;
            ulong ptrIdentity;
            boHeader.TryReadPointer(out ptrIdentity);
            objectInfo.PtrTypeInfo = ptrIdentity;
            objectInfo.ITypeDescription = snapshot.typeDescriptions.TypeInfo2ArrayIndex(objectInfo.PtrTypeInfo);
            bool error = false;
            if (objectInfo.ITypeDescription < 0)
            {
                var boIdentity = heap.Find(ptrIdentity, snapshot.virtualMachineInformation);
                if (boIdentity.IsValid)
                {
                    ulong ptrTypeInfo;
                    boIdentity.TryReadPointer(out ptrTypeInfo);
                    objectInfo.PtrTypeInfo = ptrTypeInfo;
                    objectInfo.ITypeDescription = snapshot.typeDescriptions.TypeInfo2ArrayIndex(objectInfo.PtrTypeInfo);
                    error = objectInfo.ITypeDescription < 0;
                }
                else
                {
                    error = true;
                }
            }
            if (!error)
            {
                objectInfo.Size = SizeOfObjectInBytes(snapshot, objectInfo.ITypeDescription, boHeader, heap);
                objectInfo.data = boHeader;
            }
            else
            {
                if (!ignoreBadHeaderError)
                {
#if DEBUG_VALIDATION
                    UnityEngine.Debug.LogError("Bad object header at address: "+ ptrIdentity);
#endif
                }

                objectInfo.PtrTypeInfo = 0;
                objectInfo.ITypeDescription = -1;
                objectInfo.Size = 0;
            }
            return objectInfo;
        }
    }

    internal static class StringTools
    {
        public static string ReadString(BytesAndOffset bo, VirtualMachineInformation virtualMachineInformation)
        {
            var lengthPointer = bo.Add(virtualMachineInformation.objectHeaderSize);
            var length = lengthPointer.ReadInt32();
            var firstChar = lengthPointer.Add(4);

            return System.Text.Encoding.Unicode.GetString(firstChar.bytes, firstChar.offset, length * 2);
        }

        public static int ReadStringObjectSizeInBytes(BytesAndOffset bo, VirtualMachineInformation virtualMachineInformation)
        {
            var lengthPointer = bo.Add(virtualMachineInformation.objectHeaderSize);
            var length = lengthPointer.ReadInt32();

            return virtualMachineInformation.objectHeaderSize + /*lengthfield*/ 1 + (length * /*utf16=2bytes per char*/ 2) + /*2 zero terminators*/ 2;
        }
    }
    internal class ArrayInfo
    {
        public ulong baseAddress;
        public int[] rank;
        public int length;
        public int elementSize;
        public int arrayTypeDescription;
        public int elementTypeDescription;
        public BytesAndOffset header;
        public BytesAndOffset data;
        public BytesAndOffset GetArrayElement(int index)
        {
            return data.Add(elementSize * index);
        }

        public ulong GetArrayElementAddress(int index)
        {
            return baseAddress + (ulong)(elementSize * index);
        }

        public string IndexToRankedString(int index)
        {
            return ArrayTools.ArrayRankIndexToString(rank, index);
        }

        public string ArrayRankToString()
        {
            return ArrayTools.ArrayRankToString(rank);
        }
    }
    internal static class ArrayTools
    {
        public static ArrayInfo GetArrayInfo(CachedSnapshot data, BytesAndOffset arrayData, int iTypeDescriptionArrayType)
        {
            var virtualMachineInformation = data.virtualMachineInformation;
            var arrayInfo = new ArrayInfo();
            arrayInfo.baseAddress = 0;
            arrayInfo.arrayTypeDescription = iTypeDescriptionArrayType;


            arrayInfo.header = arrayData;
            arrayInfo.data = arrayInfo.header.Add(virtualMachineInformation.arrayHeaderSize);
            ulong bounds;
            arrayInfo.header.Add(virtualMachineInformation.arrayBoundsOffsetInHeader).TryReadPointer(out bounds);

            if (bounds == 0)
            {
                arrayInfo.length = arrayInfo.header.Add(virtualMachineInformation.arraySizeOffsetInHeader).ReadInt32();
                arrayInfo.rank = new int[1] { arrayInfo.length };
            }
            else
            {
                var cursor = data.managedHeapSections.Find(bounds, virtualMachineInformation);
                int rank = data.typeDescriptions.GetRank(iTypeDescriptionArrayType);
                arrayInfo.rank = new int[rank];
                arrayInfo.length = 1;
                for (int i = 0; i != rank; i++)
                {
                    var l = cursor.ReadInt32();
                    arrayInfo.length *= l;
                    arrayInfo.rank[i] = l;
                    cursor = cursor.Add(8);
                }
            }

            arrayInfo.elementTypeDescription = data.typeDescriptions.baseOrElementTypeIndex[iTypeDescriptionArrayType];
            if (arrayInfo.elementTypeDescription == -1) //We currently do not handle uninitialized types as such override the type, making it return pointer size
            {
                arrayInfo.elementTypeDescription = iTypeDescriptionArrayType;
            }
            if (data.typeDescriptions.HasFlag(arrayInfo.elementTypeDescription, TypeFlags.kValueType))
            {
                arrayInfo.elementSize = data.typeDescriptions.size[arrayInfo.elementTypeDescription];
            }
            else
            {
                arrayInfo.elementSize = virtualMachineInformation.pointerSize;
            }
            return arrayInfo;
        }

        public static int GetArrayElementSize(CachedSnapshot data, int iTypeDescriptionArrayType)
        {
            int iElementTypeDescription = data.typeDescriptions.baseOrElementTypeIndex[iTypeDescriptionArrayType];
            if (data.typeDescriptions.HasFlag(iElementTypeDescription, TypeFlags.kValueType))
            {
                return data.typeDescriptions.size[iElementTypeDescription];
            }
            return data.virtualMachineInformation.pointerSize;
        }

        public static string ArrayRankToString(int[] rankLength)
        {
            string o = "";
            for (int i = 0; i < rankLength.Length; ++i)
            {
                if (o.Length > 0)
                {
                    o += ", ";
                }
                o += rankLength[i].ToString();
            }
            return o;
        }

        public static string ArrayRankIndexToString(int[] rankLength, int index)
        {
            string o = "";
            int remainder = index;
            for (int i = 1; i < rankLength.Length; ++i)
            {
                if (o.Length > 0)
                {
                    o += ", ";
                }
                var l = rankLength[i];
                int rankIndex = remainder / l;
                o += rankIndex.ToString();
                remainder = remainder - rankIndex * l;
            }
            if (o.Length > 0)
            {
                o += ", ";
            }
            o += remainder;
            return o;
        }

        public static int[] ReadArrayRankLength(CachedSnapshot data, CachedSnapshot.ManagedMemorySectionEntriesCache heap, UInt64 address, int iTypeDescriptionArrayType, VirtualMachineInformation virtualMachineInformation)
        {
            if (iTypeDescriptionArrayType < 0) return null;

            var bo = heap.Find(address, virtualMachineInformation);
            ulong bounds;
            bo.Add(virtualMachineInformation.arrayBoundsOffsetInHeader).TryReadPointer(out bounds);

            if (bounds == 0)
            {
                return new int[1] { bo.Add(virtualMachineInformation.arraySizeOffsetInHeader).ReadInt32() };
            }

            var cursor = heap.Find(bounds, virtualMachineInformation);
            int rank = data.typeDescriptions.GetRank(iTypeDescriptionArrayType);
            int[] l = new int[rank];
            for (int i = 0; i != rank; i++)
            {
                l[i] = cursor.ReadInt32();
                cursor = cursor.Add(8);
            }
            return l;
        }

        public static int ReadArrayLength(CachedSnapshot data, UInt64 address, int iTypeDescriptionArrayType)
        {
            if (iTypeDescriptionArrayType < 0)
            {
                return 0;
            }

            var heap = data.managedHeapSections;
            var bo = heap.Find(address, data.virtualMachineInformation);
            return ReadArrayLength(data, bo, iTypeDescriptionArrayType);
        }

        public static int ReadArrayLength(CachedSnapshot data, BytesAndOffset arrayData, int iTypeDescriptionArrayType)
        {
            if (iTypeDescriptionArrayType < 0) return 0;

            var virtualMachineInformation = data.virtualMachineInformation;
            var heap = data.managedHeapSections;
            var bo = arrayData;

            ulong bounds;
            bo.Add(virtualMachineInformation.arrayBoundsOffsetInHeader).TryReadPointer(out bounds);

            if (bounds == 0)
                return bo.Add(virtualMachineInformation.arraySizeOffsetInHeader).ReadInt32();

            var cursor = heap.Find(bounds, virtualMachineInformation);
            int length = 1;
            int rank = data.typeDescriptions.GetRank(iTypeDescriptionArrayType);
            for (int i = 0; i != rank; i++)
            {
                length *= cursor.ReadInt32();
                cursor = cursor.Add(8);
            }
            return length;
        }

        public static int ReadArrayObjectSizeInBytes(CachedSnapshot data, UInt64 address, int iTypeDescriptionArrayType)
        {
            var arrayLength = ReadArrayLength(data, address, iTypeDescriptionArrayType);

            var virtualMachineInformation = data.virtualMachineInformation;
            var ti = data.typeDescriptions.baseOrElementTypeIndex[iTypeDescriptionArrayType];
            var ai = data.typeDescriptions.TypeIndex2ArrayIndex(ti);
            var isValueType = data.typeDescriptions.HasFlag(ai, TypeFlags.kValueType);

            var elementSize = isValueType ? data.typeDescriptions.size[ai] : virtualMachineInformation.pointerSize;
            return virtualMachineInformation.arrayHeaderSize + elementSize * arrayLength;
        }

        public static int ReadArrayObjectSizeInBytes(CachedSnapshot data, BytesAndOffset arrayData, int iTypeDescriptionArrayType)
        {
            var arrayLength = ReadArrayLength(data, arrayData, iTypeDescriptionArrayType);
            var virtualMachineInformation = data.virtualMachineInformation;

            var ti = data.typeDescriptions.baseOrElementTypeIndex[iTypeDescriptionArrayType];
            if (ti == -1) // check added as element type index can be -1 if we are dealing with a class member (eg: Dictionary.Entry) whose type is uninitialized due to their generic data not getting inflated a.k.a unused types
            {
                ti = iTypeDescriptionArrayType;
            }

            var ai = data.typeDescriptions.TypeIndex2ArrayIndex(ti);
            var isValueType = data.typeDescriptions.HasFlag(ai, TypeFlags.kValueType);
            var elementSize = isValueType ? data.typeDescriptions.size[ai] : virtualMachineInformation.pointerSize;

            return virtualMachineInformation.arrayHeaderSize + elementSize * arrayLength;
        }
    }
}
