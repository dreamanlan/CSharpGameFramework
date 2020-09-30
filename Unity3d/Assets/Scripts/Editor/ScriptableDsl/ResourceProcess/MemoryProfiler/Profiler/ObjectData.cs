using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.MemoryProfilerForExtension.Editor.Format;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal enum ObjectDataType
    {
        Unknown,
        Global,
        Value,
        Object,
        Array,
        BoxedValue,
        ReferenceObject,
        ReferenceArray,
        Type,
        NativeObject,
        NativeObjectReference,
    }

    internal enum CodeType
    {
        Native,
        Managed,
        Unknown,
        Count,
    }

    internal class ObjectDataParent
    {
        public ObjectData obj;
        public int iField;
        public int arrayIndex;
        public bool expandToTarget;//true means it should display the value/target of the field. False means it should display the owning object
        public ObjectDataParent(ObjectData obj, int iField, int arrayIndex, bool expandToTarget)
        {
            this.obj = obj;
            this.iField = iField;
            this.arrayIndex = arrayIndex;
            this.expandToTarget = expandToTarget;
        }
    }
    internal struct ObjectData
    {
        private void SetManagedType(CachedSnapshot snapshot, int iType)
        {
            m_data.managed.iType = iType;
        }

        public static int InvalidInstanceID
        {
            get
            {
                return CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone;
            }
        }
        private ObjectDataType m_dataType;
        public ObjectDataParent m_Parent;//used for reference object/array and value to hold the owning object.
        public ObjectData displayObject
        {
            get
            {
                if (m_Parent != null && !m_Parent.expandToTarget)
                {
                    return m_Parent.obj;
                }
                return this;
            }
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Data
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Managed
            {
                public ulong objectPtr;
                public int iType;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct Native
            {
                public int index;
            }
            [FieldOffset(0)] public Managed managed;
            [FieldOffset(0)] public Native native;
        }
        private Data m_data;
        public int managedTypeIndex
        {
            get
            {
                switch (m_dataType)
                {
                    case ObjectDataType.Array:
                    case ObjectDataType.BoxedValue:
                    case ObjectDataType.Object:
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.Value:
                    case ObjectDataType.Type:
                        return m_data.managed.iType;
                }

                return -1;
            }
        }
        public BytesAndOffset managedObjectData;

        public ObjectDataType dataType
        {
            get
            {
                return m_dataType;
            }
        }
        public int nativeObjectIndex
        {
            get
            {
                if (m_dataType == ObjectDataType.NativeObject)
                {
                    return m_data.native.index;
                }
                return -1;
            }
        }
        public ulong hostManagedObjectPtr
        {
            get
            {
                switch (m_dataType)
                {
                    case ObjectDataType.Array:
                    case ObjectDataType.BoxedValue:
                    case ObjectDataType.Object:
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.Value:
                        return m_data.managed.objectPtr;
                }
                return 0;
            }
        }

        public int fieldIndex
        {
            get
            {
                switch (m_dataType)
                {
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.Value:
                        if (m_Parent != null)
                        {
                            return m_Parent.iField;
                        }
                        break;
                }
                return -1;
            }
        }
        public int arrayIndex
        {
            get
            {
                switch (m_dataType)
                {
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.Value:
                        if (m_Parent != null)
                        {
                            return m_Parent.arrayIndex;
                        }
                        break;
                }
                return 0;
            }
        }
        public bool dataIncludeObjectHeader
        {
            get
            {
                switch (m_dataType)
                {
                    case ObjectDataType.Unknown:
                    case ObjectDataType.Global:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.Value:
                    case ObjectDataType.Type:
                        return false;
                    case ObjectDataType.Array:
                    case ObjectDataType.Object:
                    case ObjectDataType.BoxedValue:
                        return true;
                }
                throw new Exception("Bad datatype");
            }
        }
        public bool IsValid
        {
            get
            {
                return m_dataType != ObjectDataType.Unknown;//return data.IsValid;
            }
        }
        public bool TryGetObjectPointer(out ulong ptr)
        {
            switch (dataType)
            {
                case ObjectDataType.ReferenceArray:
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.Object:
                case ObjectDataType.Array:
                case ObjectDataType.BoxedValue:
                case ObjectDataType.Value:
                    ptr = hostManagedObjectPtr;
                    return true;
                default:
                    ptr = 0;
                    return false;
            }
        }

        public ulong GetObjectPointer(CachedSnapshot snapshot, bool logError = true)
        {
            switch (dataType)
            {
                case ObjectDataType.ReferenceArray:
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.Object:
                case ObjectDataType.Array:
                case ObjectDataType.BoxedValue:
                case ObjectDataType.Value:
                    return hostManagedObjectPtr;
                case ObjectDataType.NativeObject:
                    return snapshot.nativeObjects.nativeObjectAddress[nativeObjectIndex];
                default:
                    if (logError) UnityEngine.Debug.LogError("Requesting an object pointer on an invalid data type");
                    return 0;
            }
        }

        public ulong GetReferencePointer()
        {
            switch (m_dataType)
            {
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.ReferenceArray:
                    ulong ptr;
                    managedObjectData.TryReadPointer(out ptr);
                    return ptr;
                default:
                    UnityEngine.Debug.LogError("Requesting a reference pointer on an invalid data type");
                    return 0;
            }
        }

        public ObjectData GetBoxedValue(CachedSnapshot snapshot, bool expandToTarget)
        {
            switch (m_dataType)
            {
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                    break;
                default:
                    UnityEngine.Debug.LogError("Requesting a boxed value on an invalid data type");
                    return invalid;
            }
            ObjectData od = this;
            od.m_Parent = new ObjectDataParent(this, -1, -1, expandToTarget);
            od.m_dataType = ObjectDataType.Value;
            od.managedObjectData = od.managedObjectData.Add(snapshot.virtualMachineInformation.objectHeaderSize);
            return od;
        }

        public ArrayInfo GetArrayInfo(CachedSnapshot snapshot)
        {
            if (m_dataType != ObjectDataType.Array)
            {
                UnityEngine.Debug.LogError("Requesting an ArrayInfo on an invalid data type");
                return null;
            }
            return ArrayTools.GetArrayInfo(snapshot, managedObjectData, m_data.managed.iType);
        }

        public ObjectData GetArrayElement(CachedSnapshot snapshot, int index, bool expandToTarget)
        {
            return GetArrayElement(snapshot, GetArrayInfo(snapshot), index, expandToTarget);
        }

        public ObjectData GetArrayElement(CachedSnapshot snapshot, ArrayInfo ai, int index, bool expandToTarget)
        {
            switch (m_dataType)
            {
                case ObjectDataType.Array:
                case ObjectDataType.ReferenceArray:
                    break;
                default:
                    Debug.Log("Requesting an array element on an invalid data type");
                    return invalid;
            }
            ObjectData o = new ObjectData();
            o.m_Parent = new ObjectDataParent(this, -1, index, expandToTarget);
            o.SetManagedType(snapshot, ai.elementTypeDescription);
            o.m_data.managed.objectPtr = m_data.managed.objectPtr;
            o.m_dataType = TypeToSubDataType(snapshot, ai.elementTypeDescription);
            o.managedObjectData = ai.GetArrayElement(index);
            return o;
        }

        public static ObjectDataType TypeToSubDataType(CachedSnapshot snapshot, int iType)
        {
            if (iType < 0) return ObjectDataType.Unknown;
            if (snapshot.typeDescriptions.HasFlag(iType, TypeFlags.kArray)) return ObjectDataType.ReferenceArray;
            else if (snapshot.typeDescriptions.HasFlag(iType, TypeFlags.kValueType)) return ObjectDataType.Value;
            else return ObjectDataType.ReferenceObject;
        }

        public static ObjectDataType TypeToDataType(CachedSnapshot snapshot, int iType)
        {
            if (iType < 0) return ObjectDataType.Unknown;
            if (snapshot.typeDescriptions.HasFlag(iType, TypeFlags.kArray)) return ObjectDataType.Array;
            else if (snapshot.typeDescriptions.HasFlag(iType, TypeFlags.kValueType)) return ObjectDataType.BoxedValue;
            else return ObjectDataType.Object;
        }

        // ObjectData is pointing to an object's field
        public bool IsField()
        {
            return m_Parent != null && m_Parent.iField >= 0;
        }

        // ObjectData is pointing to an item in an array
        public bool IsArrayItem()
        {
            return m_Parent != null && m_Parent.obj.dataType == ObjectDataType.Array;
        }

        // Returns the name of the field this ObjectData is pointing at.
        // should be called only when IsField() return true
        public string GetFieldName(CachedSnapshot snapshot)
        {
            return snapshot.fieldDescriptions.fieldDescriptionName[m_Parent.iField];
        }

        // Returns the number of fields the object (that this ObjectData is currently pointing at) has
        public int GetInstanceFieldCount(CachedSnapshot snapshot)
        {
            switch (m_dataType)
            {
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.Value:
                    if (managedTypeIndex < 0 || managedTypeIndex >= snapshot.typeDescriptions.fieldIndices_instance.Length) return 0;
                    return snapshot.typeDescriptions.fieldIndices_instance[managedTypeIndex].Length;
                default:
                    return 0;
            }
        }

        // Returns a new ObjectData pointing to the object's (that this ObjectData is currently pointing at) field
        // using the field index from [0, GetInstanceFieldCount()[
        public ObjectData GetInstanceFieldByIndex(CachedSnapshot snapshot, int i)
        {
            int iField = snapshot.typeDescriptions.fieldIndices_instance[managedTypeIndex][i];
            return GetInstanceFieldBySnapshotFieldIndex(snapshot, iField, true);
        }

        // Returns a new ObjectData pointing to the object's (that this ObjectData is currently pointing at) field
        // using a field index from snapshot.fieldDescriptions
        public ObjectData GetInstanceFieldBySnapshotFieldIndex(CachedSnapshot snapshot, int iField, bool expandToTarget)
        {
            ObjectData obj;
            ulong objectPtr;
            switch (m_dataType)
            {
                case ObjectDataType.ReferenceObject:
                    objectPtr = GetReferencePointer();
                    obj = FromManagedPointer(snapshot, objectPtr);
                    break;
                case ObjectDataType.BoxedValue:
                case ObjectDataType.Object:
                case ObjectDataType.Value:
                    objectPtr = m_data.managed.objectPtr;
                    obj = this;
                    break;
                //case ObjectDataType.ReferenceArray:
                default:
                    //TODO: add proper handling for missing types
                    //DebugUtility.LogError("Requesting a field on an invalid data type");
                    return new ObjectData();
            }
            var fieldOffset = snapshot.fieldDescriptions.offset[iField];
            var fieldType = snapshot.fieldDescriptions.typeIndex[iField];
            bool isStatic = snapshot.fieldDescriptions.isStatic[iField];
            switch (m_dataType)
            {
                case ObjectDataType.Value:
                    if (!isStatic) fieldOffset -= snapshot.virtualMachineInformation.objectHeaderSize;
                    break;
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                    break;
                case ObjectDataType.Type:
                    if (!isStatic)
                    {
                        Debug.LogError("Requesting a non-static field on a type");
                        return invalid;
                    }
                    break;
                default:
                    break;
            }

            ObjectData o = new ObjectData();
            o.m_Parent = new ObjectDataParent(obj, iField, -1, expandToTarget);
            o.SetManagedType(snapshot, fieldType);
            o.m_dataType = TypeToSubDataType(snapshot, fieldType);

            if (isStatic)
            {
                //the field requested might come from a base class. make sure we are using the right staticFieldBytes.
                var iOwningType = obj.m_data.managed.iType;
                while (iOwningType >= 0)
                {
                    var fieldIndex = System.Array.FindIndex(snapshot.typeDescriptions.fieldIndicesOwned_static[iOwningType], x => x == iField);
                    if (fieldIndex >= 0)
                    {
                        //field iField is owned by type iCurrentBase
                        break;
                    }
                    iOwningType = snapshot.typeDescriptions.baseOrElementTypeIndex[iOwningType];
                }
                if (iOwningType < 0)
                {
                    Debug.LogError("Field requested is not owned by the type not any of its bases");
                    return invalid;
                }

                o.m_data.managed.objectPtr = 0;
                var typeStaticData = new BytesAndOffset(snapshot.typeDescriptions.staticFieldBytes[iOwningType], snapshot.virtualMachineInformation.pointerSize);
                o.managedObjectData = typeStaticData.Add(fieldOffset);
            }
            else
            {
                o.m_data.managed.objectPtr = objectPtr;// m_data.managed.objectPtr;
                o.managedObjectData = obj.managedObjectData.Add(fieldOffset);
            }
            return o;
        }

        public int GetInstanceID(CachedSnapshot snapshot)
        {
            int nativeIndex = nativeObjectIndex;
            if (nativeIndex < 0)
            {
                int managedIndex = GetManagedObjectIndex(snapshot);
                if (managedIndex >= 0)
                {
                    nativeIndex = snapshot.CrawledData.ManagedObjects[managedIndex].NativeObjectIndex;
                }
            }

            if (nativeIndex >= 0)
            {
                return snapshot.nativeObjects.instanceId[nativeIndex];
            }
            return CachedSnapshot.NativeObjectEntriesCache.k_InstanceIDNone;
        }

        public ObjectData GetBase(CachedSnapshot snapshot)
        {
            switch (m_dataType)
            {
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.Object:
                case ObjectDataType.Type:
                    break;
                case ObjectDataType.Value:
                    return invalid;
                default:
                    UnityEngine.Debug.LogError("Requesting a base on an invalid data type");
                    return invalid;
            }

            var b = snapshot.typeDescriptions.baseOrElementTypeIndex[m_data.managed.iType];
            if (b == snapshot.typeDescriptions.iType_ValueType) return invalid;
            if (b == snapshot.typeDescriptions.iType_Object) return invalid;
            if (b == snapshot.typeDescriptions.iType_Enum) return invalid;
            ObjectData o = this;
            o.SetManagedType(snapshot, b);
            return o;
        }

        public int GetUnifiedObjectIndex(CachedSnapshot snapshot)
        {
            switch (dataType)
            {
                case ObjectDataType.Array:
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                {
                    int idx;
                    if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(m_data.managed.objectPtr, out idx))
                    {
                        return snapshot.ManagedObjectIndexToUnifiedObjectIndex(idx);
                    }
                    break;
                }
                case ObjectDataType.NativeObject:
                    return snapshot.NativeObjectIndexToUnifiedObjectIndex(m_data.native.index);
            }

            return -1;
        }

        public ManagedObjectInfo GetManagedObject(CachedSnapshot snapshot)
        {
            switch (dataType)
            {
                case ObjectDataType.Array:
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                    {
                        int idx;
                        if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(m_data.managed.objectPtr, out idx))
                        {
                            return snapshot.CrawledData.ManagedObjects[idx];
                        }
                        throw new Exception("Invalid object pointer used to query object list.");
                    }
                case ObjectDataType.ReferenceObject:
                case ObjectDataType.ReferenceArray:
                    {
                        int idx;
                        ulong refPtr = GetReferencePointer();
                        if (refPtr == 0)
                            return default(ManagedObjectInfo);
                        if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(GetReferencePointer(), out idx))
                        {
                            return snapshot.CrawledData.ManagedObjects[idx];
                        }
                        throw new Exception("Invalid object reference pointer used to query object list.");
                    }
                default:
                    throw new Exception("GetManagedObjectSize was called on a instance of ObjectData which does not contain an managed object.");
            }
        }

        public int GetManagedObjectIndex(CachedSnapshot snapshot)
        {
            switch (dataType)
            {
                case ObjectDataType.Array:
                case ObjectDataType.Object:
                case ObjectDataType.BoxedValue:
                {
                    int idx;
                    if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(m_data.managed.objectPtr, out idx))
                    {
                        return idx;
                    }


                    break;
                }
            }

            return -1;
        }

        public int GetNativeObjectIndex(CachedSnapshot snapshot)
        {
            switch (dataType)
            {
                case ObjectDataType.NativeObject:
                    return m_data.native.index;
            }

            return -1;
        }

        private ObjectData(ObjectDataType t)
        {
            m_dataType = t;
            m_data = new Data();
            m_data.managed.objectPtr = 0;
            managedObjectData = new BytesAndOffset();
            m_data.managed.iType = -1;
            m_Parent = null;
        }

        public static ObjectData invalid
        {
            get
            {
                return new ObjectData();
            }
        }
        public static ObjectData global
        {
            get
            {
                return new ObjectData(ObjectDataType.Global);
            }
        }
        public static ObjectData FromManagedType(CachedSnapshot snapshot, int iType)
        {
            ObjectData o = new ObjectData();
            o.SetManagedType(snapshot, iType);
            o.m_dataType = ObjectDataType.Type;
            o.managedObjectData = new BytesAndOffset { bytes = snapshot.typeDescriptions.staticFieldBytes[iType], offset = 0, pointerSize = snapshot.virtualMachineInformation.pointerSize };
            return o;
        }

        //index from an imaginary array composed of native objects followed by managed objects.
        public static ObjectData FromUnifiedObjectIndex(CachedSnapshot snapshot, int index)
        {
            int iNative = snapshot.UnifiedObjectIndexToNativeObjectIndex(index);
            if (iNative >= 0)
            {
                return FromNativeObjectIndex(snapshot, iNative);
            }

            int iManaged = snapshot.UnifiedObjectIndexToManagedObjectIndex(index);
            if (iManaged >= 0)
            {
                return FromManagedObjectIndex(snapshot, iManaged);
            }

            return ObjectData.invalid;
        }

        public static ObjectData FromNativeObjectIndex(CachedSnapshot snapshot, int index)
        {
            if (index < 0 || index >= snapshot.nativeObjects.Count) return ObjectData.invalid;
            ObjectData o = new ObjectData();
            o.m_dataType = ObjectDataType.NativeObject;
            o.m_data.native.index = index;
            return o;
        }

        public static ObjectData FromManagedObjectInfo(CachedSnapshot snapshot, ManagedObjectInfo moi)
        {
            if (moi.ITypeDescription < 0) return ObjectData.invalid;
            ObjectData o = new ObjectData();
            o.m_dataType = TypeToDataType(snapshot, moi.ITypeDescription);// ObjectDataType.Object;
            o.m_data.managed.objectPtr = moi.PtrObject;
            o.SetManagedType(snapshot, moi.ITypeDescription);
            o.managedObjectData = moi.data;
            return o;
        }

        public static ObjectData FromManagedObjectIndex(CachedSnapshot snapshot, int index)
        {
            if (index < 0 || index >= snapshot.CrawledData.ManagedObjects.Count) return ObjectData.invalid;
            var moi = snapshot.CrawledData.ManagedObjects[index];

            if (index < snapshot.gcHandles.Count)
            {
                //When snapshotting we might end up getting some handle targets as they are about to be collected
                //we do restart the world temporarily this can cause us to end up with targets that are not present in the dumped heaps
                if (moi.PtrObject == 0)
                    return ObjectData.invalid;

                if (moi.PtrObject != snapshot.gcHandles.target[index])
                {
                    throw new Exception("bad object");
                }
            }

            return FromManagedObjectInfo(snapshot, moi);
        }

        public static ObjectData FromManagedPointer(CachedSnapshot snapshot, ulong ptr, int asTypeIndex = -1)
        {
            if (ptr == 0) return ObjectData.invalid;
            int idx;
            if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(ptr, out idx))
            {
                return FromManagedObjectInfo(snapshot, snapshot.CrawledData.ManagedObjects[idx]);
            }
            else
            {
                ObjectData o = new ObjectData();
                o.m_data.managed.objectPtr = ptr;
                o.managedObjectData = snapshot.managedHeapSections.Find(ptr, snapshot.virtualMachineInformation);
                if (o.managedObjectData.IsValid)
                {
                    var info = Crawler.ParseObjectHeader(snapshot, o.managedObjectData, false);
                    if (asTypeIndex >= 0)
                    {
                        o.SetManagedType(snapshot, asTypeIndex);
                    }
                    else
                    {
                        o.SetManagedType(snapshot, info.ITypeDescription);
                    }

                    o.m_dataType = TypeToDataType(snapshot, info.ITypeDescription);
                    return o;
                }
            }
            return invalid;
        }

        public bool isNative
        {
            get
            {
                switch (dataType)
                {
                    case ObjectDataType.NativeObject: return true;
                }
                return false;
            }
        }
        public bool isManaged
        {
            get
            {
                switch (dataType)
                {
                    case ObjectDataType.Global:
                    case ObjectDataType.Value:
                    case ObjectDataType.Object:
                    case ObjectDataType.Array:
                    case ObjectDataType.BoxedValue:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.Type:
                        return true;
                }
                return false;
            }
        }
        public CodeType codeType
        {
            get
            {
                switch (dataType)
                {
                    case ObjectDataType.Global:
                    case ObjectDataType.Value:
                    case ObjectDataType.Object:
                    case ObjectDataType.Array:
                    case ObjectDataType.BoxedValue:
                    case ObjectDataType.ReferenceObject:
                    case ObjectDataType.ReferenceArray:
                    case ObjectDataType.Type:
                        return CodeType.Managed;
                    case ObjectDataType.NativeObject:
                        return CodeType.Native;
                    default:
                        return CodeType.Unknown;
                }
            }
        }
    }

    internal struct ObjectConnection
    {
        public static ObjectData[] GetAllObjectConnectingTo(CachedSnapshot snapshot, ObjectData obj)
        {
            var o = new List<ObjectData>();
            int objIndex = -1;
            switch (obj.dataType)
            {
                case ObjectDataType.Array:
                case ObjectDataType.BoxedValue:
                case ObjectDataType.Object:
                {
                    int idx;
                    if (snapshot.CrawledData.MangedObjectIndexByAddress.TryGetValue(obj.hostManagedObjectPtr, out idx))
                    {
                        objIndex = snapshot.ManagedObjectIndexToUnifiedObjectIndex(idx);

                        //add crawled connections
                        for (int i = 0; i != snapshot.CrawledData.Connections.Count; ++i)
                        {
                            var c = snapshot.CrawledData.Connections[i];
                            switch (c.connectionType)
                            {
                                case ManagedConnection.ConnectionType.Global_To_ManagedObject:
                                    if (c.toManagedObjectIndex == idx)
                                    {
                                        o.Add(ObjectData.global);
                                    }
                                    break;
                                case ManagedConnection.ConnectionType.ManagedObject_To_ManagedObject:
                                    if (c.toManagedObjectIndex == idx)
                                    {
                                        var objParent = ObjectData.FromManagedObjectIndex(snapshot, c.fromManagedObjectIndex);
                                        if (c.fieldFrom >= 0)
                                        {
                                            o.Add(objParent.GetInstanceFieldBySnapshotFieldIndex(snapshot, c.fieldFrom, false));
                                        }
                                        else if (c.arrayIndexFrom >= 0)
                                        {
                                            o.Add(objParent.GetArrayElement(snapshot, c.arrayIndexFrom, false));
                                        }
                                        else
                                        {
                                            o.Add(objParent);
                                        }
                                    }
                                    break;
                                case ManagedConnection.ConnectionType.ManagedType_To_ManagedObject:
                                    if (c.toManagedObjectIndex == idx)
                                    {
                                        var objType = ObjectData.FromManagedType(snapshot, c.fromManagedType);
                                        if (c.fieldFrom >= 0)
                                        {
                                            o.Add(objType.GetInstanceFieldBySnapshotFieldIndex(snapshot, c.fieldFrom, false));
                                        }
                                        else if (c.arrayIndexFrom >= 0)
                                        {
                                            o.Add(objType.GetArrayElement(snapshot, c.arrayIndexFrom, false));
                                        }
                                        else
                                        {
                                            o.Add(objType);
                                        }
                                    }
                                    break;
                                case ManagedConnection.ConnectionType.UnityEngineObject:
                                    if (c.UnityEngineManagedObjectIndex == idx)
                                    {
                                        o.Add(ObjectData.FromNativeObjectIndex(snapshot, c.UnityEngineNativeObjectIndex));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                }
                case ObjectDataType.NativeObject:
                    objIndex = snapshot.NativeObjectIndexToUnifiedObjectIndex(obj.nativeObjectIndex);

                    //add crawled connection
                    for (int i = 0; i != snapshot.CrawledData.Connections.Count; ++i)
                    {
                        switch (snapshot.CrawledData.Connections[i].connectionType)
                        {
                            case ManagedConnection.ConnectionType.Global_To_ManagedObject:
                            case ManagedConnection.ConnectionType.ManagedObject_To_ManagedObject:
                            case ManagedConnection.ConnectionType.ManagedType_To_ManagedObject:
                                break;
                            case ManagedConnection.ConnectionType.UnityEngineObject:
                                if (snapshot.CrawledData.Connections[i].UnityEngineNativeObjectIndex == obj.nativeObjectIndex)
                                {
                                    o.Add(ObjectData.FromManagedObjectIndex(snapshot, snapshot.CrawledData.Connections[i].UnityEngineManagedObjectIndex));
                                }
                                break;
                        }
                    }
                    break;
                default:
                    return null;
            }
            //add connections from the raw snapshot
            if (objIndex >= 0)
            {
                for (int i = 0; i != snapshot.connections.Count; ++i)
                {
                    if (snapshot.connections.to[i] == objIndex)
                    {
                        o.Add(ObjectData.FromUnifiedObjectIndex(snapshot, snapshot.connections.from[i]));
                    }
                }
            }
            return o.ToArray();
        }
    }
}
