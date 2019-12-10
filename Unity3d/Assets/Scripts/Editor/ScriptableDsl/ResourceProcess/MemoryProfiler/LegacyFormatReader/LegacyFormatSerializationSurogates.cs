using System;
using System.Runtime.Serialization;
using LegacyMemoryProfiler = UnityEditor.MemoryProfiler;
namespace Unity.MemoryProfilerForExtension.Editor.Legacy.LegacyFormats.Serialization
{
    public class LegacyPackedMemorySnapshotSerializationSurrogate : ISerializationSurrogate
    {
        bool m_isPre2018_3_impl = false;
        public LegacyPackedMemorySnapshotSerializationSurrogate(bool isPre2018_3_implementation)
        {
            m_isPre2018_3_impl = isPre2018_3_implementation;
        }

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var legacySnapshot = new LegacyPackedMemorySnapshot();
            legacySnapshot.m_NativeTypes = Array.ConvertAll(info.GetValue("m_NativeTypes", typeof(LegacyMemoryProfiler.PackedNativeType[])) as LegacyMemoryProfiler.PackedNativeType[], x => (PackedNativeType)x);
            legacySnapshot.m_NativeObjects = Array.ConvertAll(info.GetValue("m_NativeObjects", typeof(LegacyMemoryProfiler.PackedNativeUnityEngineObject[])) as LegacyMemoryProfiler.PackedNativeUnityEngineObject[], x => (PackedNativeUnityEngineObject)x);
            if (m_isPre2018_3_impl)
            {
                legacySnapshot.m_GCHandles = Array.ConvertAll(info.GetValue(JsonFormatTokenChanges.kGcHandles.OldField, typeof(LegacyMemoryProfiler.PackedGCHandle[])) as LegacyMemoryProfiler.PackedGCHandle[], x => (PackedGCHandle)x);
            }
            else
            {
                legacySnapshot.m_GCHandles = Array.ConvertAll(info.GetValue(JsonFormatTokenChanges.kGcHandles.NewField, typeof(LegacyMemoryProfiler.PackedGCHandle[])) as LegacyMemoryProfiler.PackedGCHandle[], x => (PackedGCHandle)x);
            }
            legacySnapshot.m_Connections = Array.ConvertAll(info.GetValue("m_Connections", typeof(LegacyMemoryProfiler.Connection[])) as LegacyMemoryProfiler.Connection[], x => (Connection)x);
            legacySnapshot.m_ManagedHeapSections = Array.ConvertAll(info.GetValue("m_ManagedHeapSections", typeof(LegacyMemoryProfiler.MemorySection[])) as LegacyMemoryProfiler.MemorySection[], x => (MemorySection)x);
            legacySnapshot.m_ManagedStacks = Array.ConvertAll(info.GetValue("m_ManagedStacks", typeof(LegacyMemoryProfiler.MemorySection[])) as LegacyMemoryProfiler.MemorySection[], x => (MemorySection)x);
            legacySnapshot.m_TypeDescriptions = Array.ConvertAll(info.GetValue("m_TypeDescriptions", typeof(LegacyMemoryProfiler.TypeDescription[])) as LegacyMemoryProfiler.TypeDescription[], x => (TypeDescription)x);
            legacySnapshot.m_VirtualMachineInformation = (VirtualMachineInformation)(LegacyMemoryProfiler.VirtualMachineInformation)info.GetValue("m_VirtualMachineInformation", typeof(LegacyMemoryProfiler.VirtualMachineInformation));
            return legacySnapshot;
        }
    }
}
