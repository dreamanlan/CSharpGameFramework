using Unity.MemoryProfilerForExtension.Editor.Database;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class ObjectReferenceTable : ObjectListTable
    {
        public static string kObjectReferenceTableName = "ManagedObjectReference";
        //not used //static string k_ObjectReferenceTableDisplayName = "Managed Object Reference";
        //not used //public ManagedConnection[] managedReference;

        ObjectData m_Object;
        ObjectData[] m_References;

        public ObjectReferenceTable(Database.Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectData obj, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            m_Object = obj;
            m_References = ObjectConnection.GetAllObjectConnectingTo(snapshot, obj);
            InitObjectList();
        }

        public override string GetName()
        {
            var str = Formatter.Format(m_Object, DefaultDataFormatter.Instance); //string.Format("0x{0:X16}", ptr);
            return kObjectReferenceTableName + "(" + str + ")";
        }

        public override string GetDisplayName() { return GetName(); }


        public override long GetObjectCount()
        {
            return m_References.LongLength;
        }

        public override string GetObjectName(long row)
        {
            if (m_References[row].isManaged)
            {
                if (m_References[row].m_Parent != null)
                {
                    var typeIndex = m_References[row].m_Parent.obj.managedTypeIndex;
                    var typeName = Snapshot.typeDescriptions.typeDescriptionName[typeIndex];
                    var iField = m_References[row].m_Parent.iField;
                    var arrayIndex = m_References[row].m_Parent.arrayIndex;
                    if (iField >= 0)
                    {
                        var fieldName = Snapshot.fieldDescriptions.fieldDescriptionName[iField];
                        return typeName + "." + fieldName;
                    }
                    else if (arrayIndex >= 0)
                    {
                        if (typeName.EndsWith("[]"))
                        {
                            return typeName.Substring(0, typeName.Length - 2) + "[" + arrayIndex + "]";
                        }
                        else
                        {
                            return typeName + "[" + arrayIndex + "]";
                        }
                    }
                }
            }
            return "[" + row + "]";
        }

        public override ObjectData GetObjectData(long row)
        {
            return m_References[row];
        }

        public override bool GetObjectStatic(long row)
        {
            return false;
        }
    }
}
