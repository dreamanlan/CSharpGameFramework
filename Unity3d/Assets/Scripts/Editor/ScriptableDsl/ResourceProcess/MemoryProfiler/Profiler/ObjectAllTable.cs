namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class ObjectAllManagedTable : ObjectListTable
    {
        public new const string TableName = "AllManagedObjects";
        public new const string TableDisplayName = "All Managed Objects";
        private ObjectData[] m_cache;
        public ObjectAllManagedTable(Database.Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            InitObjectList();
        }

        public override string GetName()
        {
            return TableName;
        }

        public override string GetDisplayName()
        {
            return TableDisplayName;
        }

        public override long GetObjectCount()
        {
            return CrawledData.ManagedObjects.Count;
        }

        public override ObjectData GetObjectData(long row)
        {
            if (m_cache == null)
            {
                m_cache = new ObjectData[CrawledData.ManagedObjects.Count];
            }

            if (row < 0 || row >= CrawledData.ManagedObjects.Count)
            {
                UnityEngine.Debug.Log("GetObjectData out of range");
            }
            if (!m_cache[row].IsValid)
            {
                var mo = CrawledData.ManagedObjects[(int)row];
                m_cache[row] = ObjectData.FromManagedPointer(Snapshot, mo.PtrObject);
            }
            return m_cache[row];
        }

        public override bool GetObjectStatic(long row)
        {
            return false;
        }

        public override void EndUpdate(IUpdater updater)
        {
            base.EndUpdate(updater);
            m_cache = null;
        }
    }
    internal class ObjectAllNativeTable : ObjectListTable
    {
        public new const string TableName = "AllNativeObjects";
        public new const string TableDisplayName = "All Native Objects";
        private ObjectData[] m_cache;
        public ObjectAllNativeTable(Database.Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            InitObjectList();
        }

        public override string GetName()
        {
            return TableName;
        }

        public override string GetDisplayName()
        {
            return TableDisplayName;
        }

        public override long GetObjectCount()
        {
            return Snapshot.nativeObjects.Count;
        }

        public override ObjectData GetObjectData(long row)
        {
            if (m_cache == null)
            {
                m_cache = new ObjectData[Snapshot.nativeObjects.Count];
            }
            if (!m_cache[row].IsValid)
            {
                m_cache[row] = ObjectData.FromNativeObjectIndex(Snapshot, (int)row);
            }
            return m_cache[row];
        }

        public override bool GetObjectStatic(long row)
        {
            return false;
        }

        public override void EndUpdate(IUpdater updater)
        {
            base.EndUpdate(updater);
            m_cache = null;
        }
    }
    internal class ObjectAllTable : ObjectListTable
    {
        public new const string TableName = "AllObjects";
        public new const string TableDisplayName = "All Objects";
        private ObjectData[] m_cache;
        public ObjectAllTable(Database.Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            InitObjectList();
        }

        public override string GetName()
        {
            return TableName;
        }

        public override string GetDisplayName()
        {
            return TableDisplayName;
        }

        public override long GetObjectCount()
        {
            return Snapshot.nativeObjects.Count + CrawledData.ManagedObjects.Count;
        }

        public override ObjectData GetObjectData(long row)
        {
            if (m_cache == null)
            {
                m_cache = new ObjectData[Snapshot.nativeObjects.Count + CrawledData.ManagedObjects.Count];
            }
            if (!m_cache[row].IsValid)
            {
                var iNative = Snapshot.UnifiedObjectIndexToNativeObjectIndex((int)row);
                if (iNative >= 0)
                {
                    m_cache[row] = ObjectData.FromNativeObjectIndex(Snapshot, iNative);
                }
                var iManaged = Snapshot.UnifiedObjectIndexToManagedObjectIndex((int)row);
                if (iManaged >= 0)
                {
                    m_cache[row] = ObjectData.FromManagedObjectIndex(Snapshot, iManaged);
                }
            }
            return m_cache[row];
        }

        public override bool GetObjectStatic(long row)
        {
            return false;
        }

        public override void EndUpdate(IUpdater updater)
        {
            base.EndUpdate(updater);
            m_cache = null;
        }

        //public override int GetObjectType(long row)
        //{
        //    var mo = crawledData.managedObjects[row];
        //    return mo.iTypeDescription;
        //}
    }
}
