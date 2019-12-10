using System;
using System.Collections;
using Unity.MemoryProfilerForExtension.Editor.EnumerationUtilities;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Legacy.LegacyFormats
{
    internal static class LegacyPackedMemorySnapshotConverter
    {
        const uint kCurrentVersion = 8;
        const int kCurrentConversionStepCount = 8;

        public static IEnumerator Convert(LegacyPackedMemorySnapshot snapshot, string writePath)
        {
            if (snapshot == null)
            {
                Debug.LogError("Parameter 'snapshot' was null.");
                yield break;
            }

            //using fully qualified name in advance to us moving both writer/reader to package
            UnityEditorInternal.Profiling.Memory.Experimental.MemorySnapshotFileWriter writer = null;
            try
            {
                writer = new UnityEditorInternal.Profiling.Memory.Experimental.MemorySnapshotFileWriter(writePath);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to create snapshot file: " + e.Message);
                yield break;
            }

            var status = new EnumerationStatus(kCurrentConversionStepCount);

            status.StepStatus = "Writing metadata";
            yield return status;

            //snapshot version will always be the current one for conversion operations
            writer.WriteEntry(EntryType.Metadata_Version, kCurrentVersion);

            //timestamp with conversion date
            writer.WriteEntry(EntryType.Metadata_RecordDate, (ulong)DateTime.Now.Ticks);

            //prepare metadata
            string platform = "Unknown";
            string content = "Converted Snapshot";
            int stringDataLength = (platform.Length + content.Length) * sizeof(char);
            byte[] metaDataBytes = new byte[stringDataLength + (sizeof(int) * 3)]; // add space for serializing the lengths of the strings

            int offset = 0;
            offset = WriteIntToByteArray(metaDataBytes, offset, content.Length);
            offset = WriteStringToByteArray(metaDataBytes, offset, content);
            offset = WriteIntToByteArray(metaDataBytes, offset, platform.Length);
            offset = WriteStringToByteArray(metaDataBytes, offset, platform);
            offset = WriteIntToByteArray(metaDataBytes, offset, 0);

            // Write metadata
            writer.WriteEntryArray(EntryType.Metadata_UserMetadata, metaDataBytes);

            writer.WriteEntry(EntryType.Metadata_CaptureFlags, (UInt32)UnityEngine.Profiling.Memory.Experimental.CaptureFlags.ManagedObjects); //capture just managed

            // Write managed heap sections
            status.IncrementStep();
            status.StepStatus = "Writing managed heap sections.";
            yield return status;
            for (int i = 0; i < snapshot.m_ManagedHeapSections.Length; ++i)
            {
                var heapSection = snapshot.m_ManagedHeapSections[i];
                writer.WriteEntry(EntryType.ManagedHeapSections_StartAddress, heapSection.m_StartAddress);
                writer.WriteEntryArray(EntryType.ManagedHeapSections_Bytes, heapSection.m_Bytes);
            }

            // Write managed types
            int fieldDescriptionCount = 0;
            status.IncrementStep();
            status.StepStatus = string.Format("Writing managed type descriptions.");
            yield return status;
            for (int i = 0; i < snapshot.m_TypeDescriptions.Length; ++i)
            {
                var type = snapshot.m_TypeDescriptions[i];
                writer.WriteEntry(EntryType.TypeDescriptions_Flags, (UInt32)type.m_Flags);
                writer.WriteEntry(EntryType.TypeDescriptions_BaseOrElementTypeIndex, type.m_BaseOrElementTypeIndex);
                writer.WriteEntry(EntryType.TypeDescriptions_TypeIndex, type.m_TypeIndex);

                if (!type.IsArray)
                {
                    var typeFields = type.m_Fields;
                    int[] fieldIndices = new int[typeFields.Length];

                    for (int j = 0; j < typeFields.Length; ++j)
                    {
                        var field = typeFields[j];
                        fieldIndices[j] = fieldDescriptionCount;
                        ++fieldDescriptionCount;

                        writer.WriteEntry(EntryType.FieldDescriptions_Offset, field.m_Offset);
                        writer.WriteEntry(EntryType.FieldDescriptions_TypeIndex, field.m_TypeIndex);
                        writer.WriteEntry(EntryType.FieldDescriptions_Name, field.m_Name);
                        writer.WriteEntry(EntryType.FieldDescriptions_IsStatic, field.m_IsStatic);
                    }

                    writer.WriteEntryArray(EntryType.TypeDescriptions_FieldIndices, fieldIndices);
                    writer.WriteEntryArray(EntryType.TypeDescriptions_StaticFieldBytes, type.m_StaticFieldBytes);
                }
                else
                {
                    writer.WriteEntryArray(EntryType.TypeDescriptions_FieldIndices, new int[0]);
                    writer.WriteEntryArray(EntryType.TypeDescriptions_StaticFieldBytes, new byte[0]);
                }

                writer.WriteEntry(EntryType.TypeDescriptions_Name, type.m_Name);
                writer.WriteEntry(EntryType.TypeDescriptions_Assembly, type.m_Assembly);
                writer.WriteEntry(EntryType.TypeDescriptions_TypeInfoAddress, type.m_TypeInfoAddress);
                writer.WriteEntry(EntryType.TypeDescriptions_Size, type.m_Size);
            }

            //write managed GC handles
            status.IncrementStep();
            status.StepStatus = "Writing managed object handles.";
            yield return status;
            for (int i = 0; i < snapshot.m_GCHandles.Length; ++i)
            {
                var handle = snapshot.m_GCHandles[i];
                writer.WriteEntry(EntryType.GCHandles_Target, handle.m_Target);
            }

            //write managed connections
            status.IncrementStep();
            status.StepStatus = "Writing connections.";
            yield return status;
            for (int i = 0; i < snapshot.m_Connections.Length; ++i)
            {
                var connection = snapshot.m_Connections[i];
                writer.WriteEntry(EntryType.Connections_From, connection.m_From);
                writer.WriteEntry(EntryType.Connections_To, connection.m_To);
            }

            //write native types
            status.IncrementStep();
            status.StepStatus = "Writing native type descriptions.";
            yield return status;
            for (int i = 0; i < snapshot.m_NativeTypes.Length; ++i)
            {
                var nativeType = snapshot.m_NativeTypes[i];
                writer.WriteEntry(EntryType.NativeTypes_NativeBaseTypeArrayIndex, nativeType.m_NativeBaseTypeArrayIndex);
                writer.WriteEntry(EntryType.NativeTypes_Name, nativeType.m_Name);
            }

            //write stub root reference for all native objects to point to, as the old format did not capture these
            writer.WriteEntry(EntryType.NativeRootReferences_AreaName, "Invalid Root");
            writer.WriteEntry(EntryType.NativeRootReferences_AccumulatedSize, (ulong)0);
            writer.WriteEntry(EntryType.NativeRootReferences_Id, (long)0);
            writer.WriteEntry(EntryType.NativeRootReferences_ObjectName, "Invalid Root Object");

            //write native objects
            status.IncrementStep();
            status.StepStatus = "Writing native object descriptions.";
            yield return status;
            for (int i = 0; i < snapshot.m_NativeObjects.Length; ++i)
            {
                var nativeObject = snapshot.m_NativeObjects[i];
                writer.WriteEntry(EntryType.NativeObjects_Name, nativeObject.m_Name);
                writer.WriteEntry(EntryType.NativeObjects_InstanceId, nativeObject.m_InstanceId);
                writer.WriteEntry(EntryType.NativeObjects_Size, (ulong)nativeObject.m_Size);
                writer.WriteEntry(EntryType.NativeObjects_NativeTypeArrayIndex, nativeObject.m_NativeTypeArrayIndex);
                writer.WriteEntry(EntryType.NativeObjects_HideFlags, (UInt32)nativeObject.m_HideFlags);
                writer.WriteEntry(EntryType.NativeObjects_Flags, (UInt32)nativeObject.m_Flags);
                writer.WriteEntry(EntryType.NativeObjects_NativeObjectAddress, (ulong)nativeObject.m_NativeObjectAddress);
                writer.WriteEntry(EntryType.NativeObjects_RootReferenceId, (long)0);
            }

            //write virtual machine information
            status.IncrementStep();
            status.StepStatus = "Writing virtual machine information.";
            yield return status;
            writer.WriteEntry(EntryType.Metadata_VirtualMachineInformation, snapshot.m_VirtualMachineInformation);

            writer.Dispose();
        }

        private static int WriteIntToByteArray(byte[] array, int offset, int value)
        {
            unsafe
            {
                byte* pi = (byte*)&value;
                array[offset++] = pi[0];
                array[offset++] = pi[1];
                array[offset++] = pi[2];
                array[offset++] = pi[3];
            }

            return offset;
        }

        private static int WriteStringToByteArray(byte[] array, int offset, string value)
        {
            if (value.Length != 0)
            {
                unsafe
                {
                    fixed(char* p = value)
                    {
                        char* begin = p;
                        char* end = p + value.Length;

                        while (begin != end)
                        {
                            for (int i = 0; i < sizeof(char); ++i)
                            {
                                array[offset++] = ((byte*)begin)[i];
                            }

                            begin++;
                        }
                    }
                }
            }

            return offset;
        }
    }

    static class JsonFormatTokenChanges
    {
        public class JsonTokenPair
        {
            public JsonTokenPair(string oldFieldName, string newFieldName)
            {
                OldField = oldFieldName;
                NewField = newFieldName;
            }

            public string OldField { private set; get; }
            public string NewField { private set; get; }
        }

        public static readonly JsonTokenPair kGcHandles = new JsonTokenPair("m_GcHandles", "m_GCHandles");
    }

    [Serializable]
    internal class LegacyPackedMemorySnapshot
    {
        public PackedNativeType[] m_NativeTypes = null;
        public PackedNativeUnityEngineObject[] m_NativeObjects = null;
        public PackedGCHandle[] m_GCHandles = null;
        public Connection[] m_Connections = null;
        public MemorySection[] m_ManagedHeapSections = null;
        public MemorySection[] m_ManagedStacks = null;
        public TypeDescription[] m_TypeDescriptions = null;
        public VirtualMachineInformation m_VirtualMachineInformation = default(VirtualMachineInformation);
    }

    [Serializable]
    internal struct PackedNativeType
    {
        public string m_Name;
        public int m_NativeBaseTypeArrayIndex;

        public PackedNativeType(string name, int nativeBaseTypeArrayIndex)
        {
            m_Name = name;
            m_NativeBaseTypeArrayIndex = nativeBaseTypeArrayIndex;
        }

        public static explicit operator PackedNativeType(UnityEditor.MemoryProfiler.PackedNativeType type)
        {
            return new PackedNativeType(type.name, type.nativeBaseTypeArrayIndex);
        }
    }


    [Serializable]
    internal struct PackedNativeUnityEngineObject
    {
        public string m_Name;
        public int m_InstanceId;
        public int m_Size;
        public int m_NativeTypeArrayIndex;
        public UnityEngine.HideFlags m_HideFlags;
        public ObjectFlags m_Flags;
        public long m_NativeObjectAddress;

        public PackedNativeUnityEngineObject(string name, int instanceId, int size, int nativeTypeArrayIndex, UnityEngine.HideFlags hideFlags, ObjectFlags flags, long nativeObjectAddress)
        {
            m_Name = name;
            m_InstanceId = instanceId;
            m_Size = size;
            m_NativeTypeArrayIndex = nativeTypeArrayIndex;
            m_HideFlags = hideFlags;
            m_Flags = flags;
            m_NativeObjectAddress = nativeObjectAddress;
        }

        public static explicit operator PackedNativeUnityEngineObject(UnityEditor.MemoryProfiler.PackedNativeUnityEngineObject packedObject)
        {
            ObjectFlags flags = 0;
            if (packedObject.isDontDestroyOnLoad)
            {
                flags |= ObjectFlags.IsDontDestroyOnLoad;
            }
            if (packedObject.isPersistent)
            {
                flags |= ObjectFlags.IsPersistent;
            }
            if (packedObject.isManager)
            {
                flags |= ObjectFlags.IsManager;
            }
            return new PackedNativeUnityEngineObject(
                packedObject.name,
                packedObject.instanceId,
                packedObject.size,
                packedObject.nativeTypeArrayIndex,
                packedObject.hideFlags,
                flags,
                packedObject.nativeObjectAddress);
        }

        public enum ObjectFlags
        {
            IsDontDestroyOnLoad = 0x1,
            IsPersistent = 0x2,
            IsManager = 0x4,
        }
    }

    [Serializable]
    internal struct PackedGCHandle
    {
        public UInt64 m_Target;

        public PackedGCHandle(UInt64 target)
        {
            m_Target = target;
        }

        public static explicit operator PackedGCHandle(UnityEditor.MemoryProfiler.PackedGCHandle handle)
        {
            return new PackedGCHandle(handle.target);
        }
    }

    [Serializable]
    internal struct Connection
    {
        public int m_From;
        public int m_To;

        public Connection(int from, int to)
        {
            m_From = from;
            m_To = to;
        }

        public static explicit operator Connection(UnityEditor.MemoryProfiler.Connection connection)
        {
            return new Connection(connection.from, connection.to);
        }
    }

    [Serializable]
    internal struct MemorySection
    {
        public byte[] m_Bytes;
        public UInt64 m_StartAddress;

        public MemorySection(byte[] bytes, UInt64 startAddress)
        {
            m_Bytes = bytes;
            m_StartAddress = startAddress;
        }

        public static explicit operator MemorySection(UnityEditor.MemoryProfiler.MemorySection memorySection)
        {
            return new MemorySection(memorySection.bytes, memorySection.startAddress);
        }
    }

    [Serializable]
    internal struct TypeDescription
    {
        public string m_Name;
        public string m_Assembly;
        public FieldDescription[] m_Fields;
        public byte[] m_StaticFieldBytes;
        public int m_BaseOrElementTypeIndex;
        public int m_Size;
        public UInt64 m_TypeInfoAddress;
        public int m_TypeIndex;
        public TypeFlags m_Flags;

        public TypeDescription(string name, string assembly, FieldDescription[] fields, byte[] staticFieldBytes, int baseOrElementTypeIndes, int size, UInt64 typeInfoAddress, int typeIndex, TypeFlags flags)
        {
            m_Name = name;
            m_Assembly = assembly;
            m_Fields = fields;
            m_StaticFieldBytes = staticFieldBytes;
            m_BaseOrElementTypeIndex = baseOrElementTypeIndes;
            m_Size = size;
            m_TypeInfoAddress = typeInfoAddress;
            m_TypeIndex = typeIndex;
            m_Flags = flags;
        }

        public static explicit operator TypeDescription(UnityEditor.MemoryProfiler.TypeDescription typeDescription)
        {
            var fields = new FieldDescription[typeDescription.fields.Length];
            for (int i = 0; i < fields.Length; ++i)
            {
                fields[i] = (FieldDescription)typeDescription.fields[i];
            }
            TypeFlags flags = 0;

            if (typeDescription.isValueType)
            {
                flags = TypeFlags.kValueType;
            }
            else if (typeDescription.isArray)
            {
                flags = TypeFlags.kArray | (TypeFlags.kArrayRankMask & (TypeFlags)(typeDescription.arrayRank << 16));
            }
            else
            {
                flags = TypeFlags.kNone;
            }

            return new TypeDescription(
                typeDescription.name,
                typeDescription.assembly,
                fields,
                typeDescription.staticFieldBytes,
                typeDescription.baseOrElementTypeIndex,
                typeDescription.size,
                typeDescription.typeInfoAddress,
                typeDescription.typeIndex,
                flags);
        }

        public bool IsArray
        {
            get { return (m_Flags & TypeFlags.kArray) != 0; }
        }

        public enum TypeFlags
        {
            kNone = 0,
            kValueType = 1 << 0,
            kArray = 1 << 1,
            kArrayRankMask = unchecked((int)0xFFFF0000)
        };
    }

    [Serializable]
    internal struct FieldDescription
    {
        public string m_Name;
        public int m_Offset;
        public int m_TypeIndex;
        public bool m_IsStatic;

        public FieldDescription(string name, int offset, int typeIndex, bool isStatic)
        {
            m_Name = name;
            m_Offset = offset;
            m_TypeIndex = typeIndex;
            m_IsStatic = isStatic;
        }

        public static explicit operator FieldDescription(UnityEditor.MemoryProfiler.FieldDescription fieldDescription)
        {
            return new FieldDescription(
                fieldDescription.name,
                fieldDescription.offset,
                fieldDescription.typeIndex,
                fieldDescription.isStatic);
        }
    }

    [Serializable]
    internal struct VirtualMachineInformation
    {
        public int m_PointerSize;
        public int m_ObjectHeaderSize;
        public int m_ArrayHeaderSize;
        public int m_ArrayBoundsOffsetInHeader;
        public int m_ArraySizeOffsetInHeader;
        public int m_AllocationGranularity;

        public static explicit operator VirtualMachineInformation(UnityEditor.MemoryProfiler.VirtualMachineInformation VMInfo)
        {
            return new VirtualMachineInformation
            {
                m_AllocationGranularity = VMInfo.allocationGranularity,
                m_ObjectHeaderSize = VMInfo.objectHeaderSize,
                m_ArrayHeaderSize = VMInfo.arrayHeaderSize,
                m_ArrayBoundsOffsetInHeader = VMInfo.arrayBoundsOffsetInHeader,
                m_ArraySizeOffsetInHeader = VMInfo.arraySizeOffsetInHeader,
                m_PointerSize = VMInfo.pointerSize
            };
        }
    };
}
