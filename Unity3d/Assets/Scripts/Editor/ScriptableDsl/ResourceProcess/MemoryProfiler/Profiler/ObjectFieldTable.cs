using System.Collections.Generic;
using System.Linq;
using Unity.MemoryProfilerForExtension.Editor.Database;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class ObjectFieldTable : ObjectListTable
    {
        public static string kObjectFieldTableName = "ManagedObjectField";
        public class FieldsList
        {
            public int[] fieldIndices;
            public bool mbHasStaticGroup;
            public bool mbHasBaseGroup;
            public long GetGroupCount()
            {
                long total = fieldIndices.LongLength;
                if (mbHasBaseGroup) ++total;
                if (mbHasStaticGroup) ++total;
                return total;
            }

            public long GetBaseGroupRow()
            {
                if (mbHasBaseGroup) return 0;
                return -1;
            }

            public long GetStaticGroupRow()
            {
                long r = 0;
                if (mbHasBaseGroup) ++r;
                if (mbHasStaticGroup) return r;
                return -1;
            }

            public bool IsRowBaseGroup(long row)
            {
                //row 0 may be [base]
                //row 1 may be [static]
                if (mbHasBaseGroup)
                {
                    return row == 0;
                }
                return false;
            }

            public bool IsRowStaticGroup(long row)
            {
                if (mbHasStaticGroup)
                {
                    int staticIndex = 0;
                    if (mbHasBaseGroup)
                    {
                        //if there's a [base] group, the static group comes after
                        ++staticIndex;
                    }
                    return row == staticIndex;
                }
                return false;
            }

            public long RowToFieldIndex(long row)
            {
                if (mbHasBaseGroup)
                {
                    --row;
                }
                if (mbHasStaticGroup)
                {
                    --row;
                }
                return row;
            }

            public long FieldIndexToRow(long fieldIndex)
            {
                if (mbHasBaseGroup)
                {
                    ++fieldIndex;
                }
                if (mbHasStaticGroup)
                {
                    ++fieldIndex;
                }
                return fieldIndex;
            }
        }
        FieldsList m_Fields;
        public ObjectData obj;
        public ObjectData objBase;
        public ObjectFieldTable(Schema schema, SnapshotObjectDataFormatter formatter, CachedSnapshot snapshot, ManagedData crawledData, ObjectData obj, ObjectMetaType metaType)
            : base(schema, formatter, snapshot, crawledData, metaType)
        {
            this.obj = obj;
            objBase = obj.GetBase(snapshot);

            SetFieldsList(BuildFieldList());
            InitObjectList();
        }

        class FieldUpdater : IUpdater
        {
            public ObjectFieldTable m_Table;
            public FieldsList m_Fields;
            long IUpdater.OldToNewRow(long a)
            {
                if (a < 0) return 0;
                if (m_Table.m_Fields.IsRowBaseGroup(a))
                {
                    return m_Fields.GetBaseGroupRow();
                }
                if (m_Table.m_Fields.IsRowStaticGroup(a))
                {
                    return m_Fields.GetStaticGroupRow();
                }
                a = m_Table.m_Fields.RowToFieldIndex(a);
                var f = m_Table.m_Fields.fieldIndices[a];
                var newIndex = System.Array.FindIndex(m_Fields.fieldIndices, x => x == f);
                if (newIndex >= 0) return m_Fields.FieldIndexToRow(newIndex);
                return -1;
            }

            long IUpdater.GetRowCount()
            {
                return m_Fields.GetGroupCount();
            }
        }

        public override IUpdater BeginUpdate()
        {
            UpdateColumns();
            var newFL = BuildFieldList();
            if (HasDifferentFieldsList(newFL))
            {
                var u = new FieldUpdater();
                u.m_Table = this;
                u.m_Fields = newFL;
                return UpdateDataBegin(u);
            }
            return UpdateDataBegin(null);
        }

        public override void EndUpdate(IUpdater updater)
        {
            UpdateDataEnd(updater);
            if (updater is Updater)
            {
                var etUpdater = (Updater)updater;
                if (etUpdater.m_DataUpdater is FieldUpdater)
                {
                    SetFieldsList((etUpdater.m_DataUpdater as FieldUpdater).m_Fields);
                }
            }
        }

        private FieldsList BuildFieldList()
        {
            FieldsList fl = new FieldsList();
            List<int> fields = new List<int>();
            switch (obj.dataType)
            {
                case ObjectDataType.Type:
                    fl.mbHasStaticGroup = false;
                    fl.mbHasBaseGroup = false;
                    if (Formatter.flattenFields)
                    {
                        //take all static field
                        fl.fieldIndices = Snapshot.typeDescriptions.fieldIndices_static[obj.managedTypeIndex];
                        return fl;
                    }
                    else
                    {
                        var staticFields = Snapshot.typeDescriptions.fieldIndices[obj.managedTypeIndex].Where(x => Snapshot.fieldDescriptions.isStatic[x]);
                        fields.AddRange(staticFields);
                    }
                    break;
                case ObjectDataType.BoxedValue:
                case ObjectDataType.Object:
                case ObjectDataType.Value:
                case ObjectDataType.ReferenceObject:
                    if (Formatter.flattenFields)
                    {
                        fl.mbHasBaseGroup = false;
                        fields.AddRange(Snapshot.typeDescriptions.fieldIndices_instance[obj.managedTypeIndex]);
                        if (Formatter.flattenStaticFields)
                        {
                            fl.mbHasStaticGroup = false;
                            fields.AddRange(Snapshot.typeDescriptions.fieldIndices_static[obj.managedTypeIndex]);
                        }
                        else
                        {
                            fl.mbHasStaticGroup = true;
                        }
                    }
                    else
                    {
                        fl.mbHasBaseGroup = objBase.IsValid;
                        if (Formatter.flattenStaticFields)
                        {
                            fl.mbHasStaticGroup = false;
                            //already has instance and static fields in the same array
                            fl.fieldIndices = Snapshot.typeDescriptions.fieldIndices[obj.managedTypeIndex];
                            return fl;
                        }
                        else
                        {
                            var staticFields = Snapshot.typeDescriptions.fieldIndices[obj.managedTypeIndex].Where(x => Snapshot.fieldDescriptions.isStatic[x]);
                            fl.mbHasStaticGroup = (staticFields.Count() > 0);
                            var instanceFields = Snapshot.typeDescriptions.fieldIndices[obj.managedTypeIndex].Where(x => !Snapshot.fieldDescriptions.isStatic[x]);
                            fields.AddRange(instanceFields);
                        }
                    }
                    break;
            }
            fl.fieldIndices = fields.ToArray();
            return fl;
        }

        private bool HasDifferentFieldsList(FieldsList fl)
        {
            bool diff = false;

            if (m_Fields == null)
            {
                diff = true;
            }
            else if (m_Fields.fieldIndices == null)
            {
                diff = fl.fieldIndices != null;
            }
            else if (m_Fields.fieldIndices.Length != fl.fieldIndices.Length)
            {
                diff = true;
            }
            else
            {
                for (int i = 0; i != m_Fields.fieldIndices.Length; ++i)
                {
                    if (m_Fields.fieldIndices[i] != fl.fieldIndices[i])
                    {
                        diff = true;
                        break;
                    }
                }
            }
            return diff;
        }

        private void SetFieldsList(FieldsList fl)
        {
            m_Fields = fl;
        }

        public override string GetName()
        {
            var str = string.Format("0x{0:X16}", obj.hostManagedObjectPtr);
            return kObjectFieldTableName + "(" + str + ")";
        }

        public override string GetDisplayName() { return GetName(); }

        public override long GetObjectCount()
        {
            if (m_Fields == null) return 0;
            return m_Fields.GetGroupCount();
        }

        public override string GetObjectName(long row)
        {
            if (m_Fields.IsRowBaseGroup(row))
            {
                return "[base]";
            }
            if (m_Fields.IsRowStaticGroup(row))
            {
                return "[static]";
            }
            row = m_Fields.RowToFieldIndex(row);
            int fieldIndex = m_Fields.fieldIndices[row];
            return Snapshot.fieldDescriptions.fieldDescriptionName[fieldIndex];
        }

        public override ObjectData GetObjectData(long row)
        {
            if (m_Fields.IsRowBaseGroup(row))
            {
                return objBase;
            }
            if (m_Fields.IsRowStaticGroup(row))
            {
                return ObjectData.FromManagedType(Snapshot, obj.managedTypeIndex);
            }
            var row2 = m_Fields.RowToFieldIndex(row);
            if (row2 < 0 || row2 >= m_Fields.fieldIndices.Length)
            {
                return ObjectData.invalid;
            }
            return obj.GetInstanceFieldBySnapshotFieldIndex(Snapshot, m_Fields.fieldIndices[row2], true);
        }

        public override bool GetObjectStatic(long row)
        {
            if (m_Fields.IsRowBaseGroup(row))
            {
                return false;
            }
            if (m_Fields.IsRowStaticGroup(row))
            {
                return true;
            }
            row = m_Fields.RowToFieldIndex(row);
            var iField = m_Fields.fieldIndices[row];
            return Snapshot.fieldDescriptions.isStatic[iField];
        }
    }
}
