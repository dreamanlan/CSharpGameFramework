using Unity.MemoryProfilerForExtension.Editor.Database;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal interface IObjectDataTypeFormatter
    {
        string GetTypeName();
        string Format(CachedSnapshot snapshot, ObjectData data, IDataFormatter formatter);
        bool Expandable();
    }
    internal class Int16TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Int16"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadInt16()); }
    }
    internal class Int32TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Int32"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadInt32()); }
    }
    internal class Int64TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Int64"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadInt64()); }
    }
    internal class UInt16TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.UInt16"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadUInt16()); }
    }
    internal class UInt32TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.UInt32"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadUInt32()); }
    }
    internal class UInt64TypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.UInt64"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadUInt64()); }
    }
    internal class BooleanTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Boolean"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadBoolean()); }
    }
    internal class CharTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Char"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadChar()); }
    }
    internal class DoubleTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Double"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadDouble()); }
    }
    internal class SingleTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Single"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadSingle()); }
    }
    internal class StringTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.String"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter)
        {
            if (od.dataIncludeObjectHeader)
            {
                od = od.GetBoxedValue(snapshot, true);
            }
            return formatter.Format(od.managedObjectData.ReadString());
        }
    }
    internal class IntPtrTypeDisplay : IObjectDataTypeFormatter
    {
        const string k_InvalidIntPtr = "Invalid IntPtr";
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.IntPtr"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter)
        {
            ulong ptr;
            if (od.managedObjectData.TryReadPointer(out ptr) != BytesAndOffset.PtrReadError.Success)
            {
                return k_InvalidIntPtr;  
            }

            return formatter.Format(ptr);
        }
    }
    internal class ByteTypeDisplay : IObjectDataTypeFormatter
    {
        bool IObjectDataTypeFormatter.Expandable() { return false; }
        string IObjectDataTypeFormatter.GetTypeName() { return "System.Byte"; }
        string IObjectDataTypeFormatter.Format(CachedSnapshot snapshot, ObjectData od, IDataFormatter formatter) { return formatter.Format(od.managedObjectData.ReadByte()); }
    }
}
