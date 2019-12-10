namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class ObjectArrayTable : ObjectListTable
    {
        public new const string TableName = "ManagedObjectArray";
        public new const string TableDisplayName = "Managed Object Array";
        public ObjectData arrayData;
        public ArrayInfo arrayInfo;
        public ObjectArrayTable(Database.Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectData arrayData, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            this.arrayData = arrayData;

            if (arrayData.isManaged)
            {
                arrayInfo = ArrayTools.GetArrayInfo(snapshot, arrayData.managedObjectData, arrayData.managedTypeIndex);
            }

            InitObjectList();
        }

        public override string GetName()
        {
            if (arrayInfo == null) return TableName + "(null)";
            var str = string.Format("0x{0:X16}", arrayData.hostManagedObjectPtr);
            return TableName + "(" + str + ")";
        }

        public override string GetDisplayName()
        {
            if (arrayInfo == null) return TableDisplayName + "(null)";
            var str = string.Format("0x{0:X16}", arrayData.hostManagedObjectPtr);
            return TableDisplayName + "(" + str + ")";
        }

        public override long GetObjectCount()
        {
            if (arrayInfo == null) return 0;
            return arrayInfo.length;
        }

        public override string GetObjectName(long row)
        {
            var str = "[" + arrayInfo.IndexToRankedString((int)row) + "]";
            return str;
        }

        public override ObjectData GetObjectData(long row)
        {
            return arrayData.GetArrayElement(Snapshot, arrayInfo, (int)row, true);
        }

        public override bool GetObjectStatic(long row)
        {
            return false;
        }
    }
}
