using UnityEngine;
using System.Collections.Generic;
using Unity.MemoryProfilerForExtension.Editor.Database;
using Unity.MemoryProfilerForExtension.Editor.Database.Operation;
using Unity.MemoryProfilerForExtension.Editor.Database.Soa;
using Unity.MemoryProfilerForExtension.Editor.Database.Aos;
using Unity.Profiling;
using Unity.MemoryProfilerForExtension.Editor.Format;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class RawSchema : Schema
    {
        public static string kPrefixTableName = "Raw";
        public static string kPrefixTableDisplayName = "Raw ";

        public CachedSnapshot m_Snapshot;
        public SnapshotObjectDataFormatter formatter;
        APITable[] m_Tables;
        Table[] m_ExtraTable;

        public class TypeBase
        {
            public int[] TypeIndex;
            public int[] BaseIndex;
            public TypeBase(CachedSnapshot snapshot)
            {
                ComputeTypeBases(snapshot);
            }

            public void ComputeTypeBases(CachedSnapshot snapshot)
            {
                var typeIndex = new List<int>();
                var baseIndex = new List<int>();
                for (int i = 0; i != snapshot.nativeTypes.Count; ++i)
                {
                    var currentBase = snapshot.nativeTypes.nativeBaseTypeArrayIndex[i];
                    while (currentBase >= 0)
                    {
                        typeIndex.Add(i);
                        baseIndex.Add(currentBase);
                        currentBase = snapshot.nativeTypes.nativeBaseTypeArrayIndex[currentBase];
                    }
                }
                TypeIndex = typeIndex.ToArray();
                BaseIndex = baseIndex.ToArray();
            }
        };
        TypeBase m_TypeBase;

        Dictionary<string, Table> m_TablesByName = new Dictionary<string, Table>();

        static ProfilerMarker s_CreateSnapshotSchema = new ProfilerMarker("CreateSnapshotSchema");

        public void SetupSchema(CachedSnapshot snapshot, ObjectDataFormatter objectDataFormatter)
        {
            using (s_CreateSnapshotSchema.Auto())
            {
                m_Snapshot = snapshot;
                formatter = new SnapshotObjectDataFormatter(objectDataFormatter, m_Snapshot);
                CreateTables(m_Snapshot.CrawledData);
            }
        }

        private void CreateTables(ManagedData crawledData)
        {
            List<APITable> tables = new List<APITable>();
            CreateTable_RootReferences(tables);
            CreateTable_NativeAllocations(tables);
            CreateTable_NativeAllocationSites(tables);
            CreateTable_NativeCallstackSymbols(tables);
            CreateTable_NativeMemoryLabels(tables);
            CreateTable_NativeMemoryRegions(tables);
            CreateTable_NativeObjects(tables);
            CreateTable_NativeTypes(tables);
            CreateNativeTable_NativeTypeBase(tables);
            CreateTable_NativeConnections(tables);
            CreateTable_TypeDescriptions(tables);
            m_Tables = tables.ToArray();


            List<Table> extraTable = new List<Table>();

            extraTable.Add(new ObjectAllManagedTable(this, formatter, m_Snapshot, crawledData, ObjectTable.ObjectMetaType.Managed));
            extraTable.Add(new ObjectAllNativeTable(this, formatter, m_Snapshot, crawledData, ObjectTable.ObjectMetaType.Native));
            extraTable.Add(new ObjectAllTable(this, formatter, m_Snapshot, crawledData, ObjectTable.ObjectMetaType.All));

            m_ExtraTable = extraTable.ToArray();
            foreach (var t in m_ExtraTable)
            {
                m_TablesByName.Add(t.GetName(), t);
            }
        }

        public void Clear()
        {
            m_Snapshot = null;
            formatter.Clear();
        }

        public override string GetDisplayName()
        {
            return m_Snapshot.packedMemorySnapshot.recordDate.ToString();
        }

        public override bool OwnsTable(Table table)
        {
            if (table.Schema == this) return true;
            if (System.Array.IndexOf(m_Tables, table) >= 0) return true;
            if (System.Array.IndexOf(m_ExtraTable, table) >= 0) return true;
            return false;
        }

        public override long GetTableCount()
        {
            return m_Tables.Length + m_ExtraTable.Length;
        }

        public override Table GetTableByName(string name)
        {
            Table t;
            if (m_TablesByName.TryGetValue(name, out t))
            {
                if (t is ExpandTable)
                {
                    var et = (ExpandTable)t;
                    et.ResetAllGroup();
                }
                return t;
            }
            return null;
        }

        public override Table GetTableByIndex(long index)
        {
            if (index < 0) return null;
            Table t = null;
            if (index >= m_Tables.Length)
            {
                index -= m_Tables.Length;
                if (index >= m_ExtraTable.Length) return null;
                t = m_ExtraTable[index];
            }
            else
            {
                t = m_Tables[index];
            }

            if (t is ExpandTable)
            {
                var et = (ExpandTable)t;
                et.ResetAllGroup();
            }
            return t;
        }

        private bool TryGetParam(ParameterSet param, string name, out ulong value)
        {
            if (param == null)
            {
                value = default(ulong);
                return false;
            }
            Expression expObj;
            param.TryGet(ObjectTable.ObjParamName, out expObj);
            if (expObj == null)
            {
                value = 0;
                return false;
            }
            if (expObj is TypedExpression<ulong>)
            {
                TypedExpression<ulong> e = (TypedExpression<ulong>)expObj;
                value = e.GetValue(0);
            }
            else
            {
                if (!ulong.TryParse(expObj.GetValueString(0, DefaultDataFormatter.Instance), out value))
                {
                    return false;
                }
            }
            return true;
        }

        private bool TryGetParam(ParameterSet param, string name, out int value)
        {
            if (param == null)
            {
                value = default(int);
                return false;
            }
            Expression expObj;
            param.TryGet(ObjectTable.ObjParamName, out expObj);
            if (expObj == null)
            {
                value = 0;
                return false;
            }
            if (expObj is TypedExpression<int>)
            {
                TypedExpression<int> e = (TypedExpression<int>)expObj;
                value = e.GetValue(0);
            }
            else
            {
                if (!int.TryParse(expObj.GetValueString(0, DefaultDataFormatter.Instance), out value))
                {
                    return false;
                }
            }
            return true;
        }

        public override Table GetTableByName(string name, ParameterSet param)
        {
            if (name == ObjectTable.TableName)
            {
                ulong obj;
                if (!TryGetParam(param, ObjectTable.ObjParamName, out obj))
                {
                    return null;
                }
                int iType;
                if (!TryGetParam(param, ObjectTable.TypeParamName, out iType))
                {
                    iType = -1;
                }

                ObjectData od = ObjectData.FromManagedPointer(m_Snapshot, obj, iType);
                var table = new ObjectSingleTable(this, formatter, m_Snapshot, m_Snapshot.CrawledData, od, od.isNative ? ObjectTable.ObjectMetaType.Native : ObjectTable.ObjectMetaType.Managed);
                return table;
            }
            else if (name == ObjectReferenceTable.kObjectReferenceTableName)
            {
                int objUnifiedIndex;
                if (!TryGetParam(param, ObjectTable.ObjParamName, out objUnifiedIndex))
                {
                    return null;
                }
                var od = ObjectData.FromUnifiedObjectIndex(m_Snapshot, objUnifiedIndex);
                var table = new ObjectReferenceTable(this, formatter, m_Snapshot, m_Snapshot.CrawledData, od, ObjectTable.ObjectMetaType.All); //, od.isNative ? ObjectTable.ObjectMetaType.Native : ObjectTable.ObjectMetaType.Managed);
                return table;
                //ObjectReferenceTable
            }
            else
            {
                return GetTableByName(name);
            }
        }

        private bool HasBit(ObjectFlags bitfield, ObjectFlags bit)
        {
            return (bitfield & bit) == bit;
        }

        private void SetBit(ref ObjectFlags bitfield, ObjectFlags bit, bool value)
        {
            bitfield = bitfield & ~bit | (value ? bit : 0);
        }

        private bool HasBit(HideFlags bitfield, HideFlags bit)
        {
            return (bitfield & bit) == bit;
        }

        private void SetBit(ref HideFlags bitfield, HideFlags bit, bool value)
        {
            bitfield = bitfield & ~bit | (value ? bit : 0);
        }

        private bool HasBit(TypeFlags bitfield, TypeFlags bit)
        {
            return (bitfield & bit) == bit;
        }

        private void SetBit(ref TypeFlags bitfield, TypeFlags bit, bool value)
        {
            bitfield = bitfield & ~bit | (value ? bit : 0);
        }

        private int GetBits(TypeFlags bitfield, TypeFlags mask, int shift)
        {
            return (int)(bitfield & mask) >> shift;
        }

        private void SetBits(ref TypeFlags bitfield, TypeFlags mask, int shift, int value)
        {
            bitfield = bitfield & ~mask | (TypeFlags)((value << shift) & (int)mask);
        }

        private void CreateTable_RootReferences(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeRootReferences.dataSet);
            table.AddColumn(
                new MetaColumn("id", "id", typeof(long), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeRootReferences.id)
            );
            table.AddColumn(
                new MetaColumn("areaName", "areaName", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeRootReferences.areaName)
            );
            table.AddColumn(
                new MetaColumn("objectName", "objectName", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeRootReferences.objectName)
            );
            table.AddColumn(
                new MetaColumn("accumulatedSize", "accumulatedSize", typeof(ulong), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(ulong)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeRootReferences.accumulatedSize)
            );
            table.CreateTable(kPrefixTableName + "RootReference", kPrefixTableDisplayName + "Root Reference");
            AddTable(table, tables);
        }

        private void CreateTable_NativeAllocationSites(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeAllocationSites.dataSet);
            table.AddColumn(
                new MetaColumn("id", "id", typeof(long), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocationSites.id)
            );
            table.AddColumn(
                new MetaColumn("memoryLabelIndex", "memoryLabelIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocationSites.memoryLabelIndex)
            );

            table.CreateTable(kPrefixTableName + "NativeAllocationSite", kPrefixTableDisplayName + "Native Allocation Site");
            AddTable(table, tables);
        }

        private void CreateTable_NativeCallstackSymbols(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeCallstackSymbols.dataSet);
            table.AddColumn(
                new MetaColumn("symbol", "symbol", typeof(ulong), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeCallstackSymbols.symbol)
            );
            table.AddColumn(
                new MetaColumn("readableStackTrace", "readableStackTrace", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeCallstackSymbols.readableStackTrace)
            );
            table.CreateTable(kPrefixTableName + "NativeCallstackSymbol", kPrefixTableDisplayName + "Native Callstack Symbol");
            AddTable(table, tables);
        }

        private void CreateTable_NativeMemoryLabels(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeMemoryLabels.dataSet);
            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryLabels.memoryLabelName)
            );
            table.CreateTable(kPrefixTableName + "NativeMemoryLabel", kPrefixTableDisplayName + "Native Memory Label");
            AddTable(table, tables);
        }

        private void CreateTable_NativeMemoryRegions(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeMemoryRegions.dataSet);
            table.AddColumn(
                new MetaColumn("parentIndex", "parentIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.parentIndex)
            );
            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.memoryRegionName)
            );
            table.AddColumn(
                new MetaColumn("addressBase", "addressBase", typeof(ulong), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.addressBase)
            );
            table.AddColumn(
                new MetaColumn("addressSize", "addressSize", typeof(ulong), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(ulong)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.addressSize)
            );
            table.AddColumn(
                new MetaColumn("firstAllocationIndex", "firstAllocationIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.firstAllocationIndex)
            );
            table.AddColumn(
                new MetaColumn("numAllocations", "numAllocations", typeof(int), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(int)))
                , DataArray.MakeColumn(m_Snapshot.nativeMemoryRegions.numAllocations)
            );
            table.CreateTable(kPrefixTableName + "NativeMemoryRegion", kPrefixTableDisplayName + "Native Memory Region");
            AddTable(table, tables);
        }

        private void CreateTable_NativeObjects(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeObjects.dataSet);
            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.objectName)
            );
            table.AddColumn(
                new MetaColumn("instanceId", "instanceId", typeof(int), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.instanceId)
            );
            table.AddColumn(
                new MetaColumn("size", "size", typeof(ulong), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(ulong)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.size)
            );

            table.AddColumn(
                new MetaColumn("nativeObjectAddress", "nativeObjectAddress", typeof(ulong), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.nativeObjectAddress)
            );
            table.AddColumn(
                new MetaColumn("rootReferenceId", "rootReferenceId", typeof(long), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.rootReferenceId)
            );

            table.AddColumn(
                new MetaColumn("nativeTypeArrayIndex", "nativeTypeArrayIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeObjects.nativeTypeArrayIndex)
            );

            table.AddColumn(
                new MetaColumn("isPersistent", "isPersistent", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.flags, (a) => HasBit(a, ObjectFlags.IsPersistent) , (ref ObjectFlags o, bool v) => SetBit(ref o, ObjectFlags.IsPersistent, v))
            );
            table.AddColumn(
                new MetaColumn("isDontDestroyOnLoad", "isDontDestroyOnLoad", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.flags, (a) => HasBit(a, ObjectFlags.IsDontDestroyOnLoad), (ref ObjectFlags o, bool v) => SetBit(ref o, ObjectFlags.IsDontDestroyOnLoad, v))
            );
            table.AddColumn(
                new MetaColumn("isManager", "isManager", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.flags, (a) => HasBit(a, ObjectFlags.IsManager), (ref ObjectFlags o, bool v) => SetBit(ref o, ObjectFlags.IsManager, v))
            );

            table.AddColumn(
                new MetaColumn("HideInHierarchy", "HideInHierarchy", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.HideInHierarchy), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.HideInHierarchy, v))
            );
            table.AddColumn(
                new MetaColumn("HideInInspector", "HideInInspector", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.HideInInspector), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.HideInInspector, v))
            );
            table.AddColumn(
                new MetaColumn("DontSaveInEditor", "DontSaveInEditor", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.DontSaveInEditor), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.DontSaveInEditor, v))
            );
            table.AddColumn(
                new MetaColumn("NotEditable", "NotEditable", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.NotEditable), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.NotEditable, v))
            );
            table.AddColumn(
                new MetaColumn("DontSaveInBuild", "DontSaveInBuild", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.DontSaveInBuild), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.DontSaveInBuild, v))
            );
            table.AddColumn(
                new MetaColumn("DontUnloadUnusedAsset", "DontUnloadUnusedAsset", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.nativeObjects.hideFlags, (a) => HasBit(a, HideFlags.DontUnloadUnusedAsset), (ref HideFlags o, bool v) => SetBit(ref o, HideFlags.DontUnloadUnusedAsset, v))
            );

            table.CreateTable(kPrefixTableName + "NativeObject", kPrefixTableDisplayName + "Native Object");
            AddTable(table, tables);
        }

        private void CreateTable_NativeTypes(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeTypes.dataSet);
            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeTypes.typeName)
            );
            table.AddColumn(
                new MetaColumn("nativeBaseTypeArrayIndex", "nativeBaseTypeArrayIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeTypes.nativeBaseTypeArrayIndex)
            );
            table.CreateTable(kPrefixTableName + "NativeType", kPrefixTableDisplayName + "Native Type");
            AddTable(table, tables);
        }

        private void CreateTable_NativeAllocations(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.nativeAllocations.dataSet);
            table.AddColumn(
                new MetaColumn("rootReferenceId", "Root Reference Id", typeof(long), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.rootReferenceId)
            );
            table.AddColumn(
                new MetaColumn("memoryRegionIndex", "Memory Region Index", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.memoryRegionIndex)
            );
            table.AddColumn(
                new MetaColumn("allocationSiteId", "Allocation Site Id", typeof(long), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.allocationSiteId)
            );
            table.AddColumn(
                new MetaColumn("address", "Address", typeof(ulong), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.address)
            );
            table.AddColumn(
                new MetaColumn("size", "Size", typeof(ulong), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(ulong)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.size)
            );
            table.AddColumn(
                new MetaColumn("overheadSize", "Overhead Size", typeof(int), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(int)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.overheadSize)
            );
            table.AddColumn(
                new MetaColumn("paddingSize", "Padding Size", typeof(int), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(int)), "size")
                , DataArray.MakeColumn(m_Snapshot.nativeAllocations.paddingSize)
            );
            table.CreateTable(kPrefixTableName + "NativeAllocation", kPrefixTableDisplayName + "Native Allocation");
            AddTable(table, tables);
        }

        private void CreateTable_NativeConnections(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.connections.dataSet);
            table.AddColumn(
                new MetaColumn("from", "from", typeof(int), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.connections.from)
            );
            table.AddColumn(
                new MetaColumn("to", "to", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.connections.to)
            );
            table.CreateTable(kPrefixTableName + "NativeConnection", kPrefixTableDisplayName + "Native Connection");
            AddTable(table, tables);
        }

        private void CreateTable_TypeDescriptions(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.typeDescriptions.dataSet);


            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.typeDescriptionName)
            );
            table.AddColumn(
                new MetaColumn("assembly", "assembly", typeof(string), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.assembly)
            );

            table.AddColumn(
                new MetaColumn("isValueType", "isValueType", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.typeDescriptions.flags, (a) => HasBit(a, TypeFlags.kValueType), (ref TypeFlags o, bool v) => SetBit(ref o, TypeFlags.kValueType, v))
            );
            table.AddColumn(
                new MetaColumn("isArray", "isArray", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.typeDescriptions.flags, (a) => HasBit(a, TypeFlags.kArray), (ref TypeFlags o, bool v) => SetBit(ref o, TypeFlags.kArray, v))
            );

            table.AddColumn(
                new MetaColumn("arrayRank", "arrayRank", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn_Transform(m_Snapshot.typeDescriptions.flags, (a) => GetBits(a, TypeFlags.kArrayRankMask, 16), (ref TypeFlags o, int v) => SetBits(ref o, TypeFlags.kArrayRankMask, 16, v))
            );
            table.AddColumn(
                new MetaColumn("baseOrElementTypeIndex", "baseOrElementTypeIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.baseOrElementTypeIndex)
            );
            table.AddColumn(
                new MetaColumn("size", "size", typeof(int), false, Grouping.groupByDuplicate
                    , Grouping.GetMergeAlgo(Grouping.MergeAlgo.sum, typeof(int)), "size")
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.size)
            );
            table.AddColumn(
                new MetaColumn("typeInfoAddress", "typeInfoAddress", typeof(ulong), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.typeInfoAddress)
            );
            table.AddColumn(
                new MetaColumn("typeIndex", "typeIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.typeDescriptions.typeIndex)
            );
            table.CreateTable(kPrefixTableName + "ManagedType", kPrefixTableDisplayName + "Managed Type");
            AddTable(table, tables);
        }

        private void CreateTable_Field(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.fieldDescriptions.dataSet);

            table.AddColumn(
                new MetaColumn("name", "name", typeof(string), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.fieldDescriptions.fieldDescriptionName)
            );
            table.AddColumn(
                new MetaColumn("typeIndex", "typeIndex", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.fieldDescriptions.typeIndex)
            );
            table.AddColumn(
                new MetaColumn("offset", "offset", typeof(int), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.fieldDescriptions.offset)
            );
            table.AddColumn(
                new MetaColumn("isStatic", "isStatic", typeof(bool), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.fieldDescriptions.isStatic)
            );

            table.CreateTable(kPrefixTableName + "ManagedTypeField", kPrefixTableDisplayName + "Managed Type Field");
            AddTable(table, tables);
        }

        private void CreateTable_GCHandles(List<APITable> tables)
        {
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.gcHandles.dataSet);

            table.AddColumn(
                new MetaColumn("target", "target", typeof(ulong), false, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_Snapshot.gcHandles.target)
            );
            table.CreateTable(kPrefixTableName + "GCHandles", kPrefixTableDisplayName + "GCHandles");
            AddTable(table, tables);
        }

        private void CreateTable_ManagedHeapSections(List<APITable> tables)
        {
            SoaDataSet dataSet = new SoaDataSet(m_Snapshot.managedHeapSections.Count, 1024);
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.managedHeapSections.Count);
            table.AddColumn(
                new MetaColumn("startAddress", "startAddress", typeof(ulong), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(dataSet, DataSourceFromAPI.ApiToDatabase(m_Snapshot.packedMemorySnapshot.managedHeapSections.startAddress))
            );
            table.CreateTable(kPrefixTableName + "ManagedHeapSections", kPrefixTableDisplayName + "ManagedHeapSections");
            AddTable(table, tables);
        }

        private void CreateTable_ManagedStacks(List<APITable> tables)
        {
            SoaDataSet dataSet = new SoaDataSet(m_Snapshot.managedHeapSections.Count, 1024);
            APITable table = new APITable(this, m_Snapshot, m_Snapshot.managedHeapSections.Count);
            table.AddColumn(
                new MetaColumn("startAddress", "startAddress", typeof(ulong), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(dataSet, DataSourceFromAPI.ApiToDatabase(m_Snapshot.packedMemorySnapshot.managedHeapSections.startAddress))
            );
            table.CreateTable(kPrefixTableName + "ManagedStacks", kPrefixTableDisplayName + "ManagedStacks");
            AddTable(table, tables);
        }

        private void CreateManageTable_Connections(List<APITable> tables)
        {
            var crawledData = m_Snapshot.CrawledData;
            APITable table = new APITable(this, m_Snapshot, crawledData.Connections.Count);

            table.AddColumn(
                new MetaColumn("from", "from", typeof(int), true, Grouping.groupByDuplicate, null)
                , Data.MakeColumn(crawledData.Connections, n => n.GetUnifiedIndexFrom(m_Snapshot))
            );
            table.AddColumn(
                new MetaColumn("to", "to", typeof(int), true, Grouping.groupByDuplicate, null)
                , Data.MakeColumn(crawledData.Connections, n => n.GetUnifiedIndexTo(m_Snapshot))
            );
            table.AddColumn(
                new MetaColumn("type", "type", typeof(ManagedConnection.ConnectionType), true, Grouping.groupByDuplicate, null)
                , Data.MakeColumn(crawledData.Connections, n => n.connectionType)
            );
            table.CreateTable(kPrefixTableName + "ObjectConnection", kPrefixTableDisplayName + "Object Connection");
            AddTable(table, tables);
        }

        private void CreateNativeTable_NativeTypeBase(List<APITable> tables)
        {
            m_TypeBase = new TypeBase(m_Snapshot);
            APITable table = new APITable(this, m_Snapshot, m_TypeBase.TypeIndex.LongLength);

            table.AddColumn(
                new MetaColumn("typeIndex", "typeIndex", typeof(int), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_TypeBase.TypeIndex)
            );
            table.AddColumn(
                new MetaColumn("baseIndex", "baseIndex", typeof(int), true, Grouping.groupByDuplicate, null)
                , DataArray.MakeColumn(m_TypeBase.BaseIndex)
            );
            table.CreateTable(kPrefixTableName + "NativeTypeBase", kPrefixTableDisplayName + "Native Type Base");
            AddTable(table, tables);
        }

        private void AddTable(APITable t, List<APITable> tables)
        {
            m_TablesByName.Add(t.GetName(), t);
            tables.Add(t);
        }
    }
}
