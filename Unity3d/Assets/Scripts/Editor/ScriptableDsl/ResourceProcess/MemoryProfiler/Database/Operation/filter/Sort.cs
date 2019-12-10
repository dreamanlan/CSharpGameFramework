using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation.Filter
{
    /// <summary>
    /// A filter that will sort entries using a column and a SortOrder (ascending/descending)
    /// It can sort data on several depth level. When 2 entries have an equal value while sorting, it will sort them using a deeper level of sort
    /// </summary>
    internal class Sort : Filter
    {
        public abstract class Level
        {
            public Level(SortOrder order)
            {
                this.Order = order;
            }

            public abstract int GetColumnIndex(Database.Table tableIn);
            public abstract string GetColumnName(Database.Table tableIn);
            //public long columnIndex;
            public SortOrder Order;
        }
        internal class LevelByIndex : Level
        {
            public LevelByIndex(int columnIndex, SortOrder order)
                : base(order)
            {
                this.ColumnIndex = columnIndex;
            }

            public override int GetColumnIndex(Database.Table tableIn)
            {
                return ColumnIndex;
            }

            public override string GetColumnName(Database.Table tableIn)
            {
                return tableIn.GetMetaData().GetColumnByIndex(ColumnIndex).Name;
            }

            public int ColumnIndex;
        }
        internal class LevelByName : Level
        {
            public LevelByName(string columnName, SortOrder order)
                : base(order)
            {
                this.ColumnName = columnName;
            }

            public override int GetColumnIndex(Database.Table tableIn)
            {
                return tableIn.GetMetaData().GetColumnByName(ColumnName).Index;
            }

            public override string GetColumnName(Database.Table tableIn)
            {
                return ColumnName;
            }

            public string ColumnName;
        }
        public List<Level> SortLevel = new List<Level>();

        public override Filter Clone(FilterCloning fc)
        {
            Sort o = new Sort();

            o.SortLevel.Capacity = SortLevel.Count;
            foreach (var l in SortLevel)
            {
                o.SortLevel.Add(l);
            }

            return o;
        }

        public override Database.Table CreateFilter(Database.Table tableIn)
        {
            if (SortLevel.Count == 0)
            {
                return tableIn;
            }

            // make sure we can get an accurate row count
            tableIn.ComputeRowCount();

            // This is a temporary fix to avoid sorting sub tables entries with top level entries.
            // the real fix involve sorting sub tables entries as part of the group head
            if (tableIn is ExpandTable)
            {
                var et = (ExpandTable)tableIn;
                et.ResetAllGroup();
            }

            return CreateFilter(tableIn, new ArrayRange(0, tableIn.GetRowCount()));
        }

        public override Database.Table CreateFilter(Database.Table tableIn, ArrayRange range)
        {
            if (SortLevel.Count == 0)
            {
                return new Database.Operation.IndexedTable(tableIn, range);
            }


            int[] columnIndex = new int[SortLevel.Count];
            SortOrder[] order = new SortOrder[SortLevel.Count];
            for (int i = 0; i != SortLevel.Count; ++i)
            {
                columnIndex[i] = SortLevel[i].GetColumnIndex(tableIn);
                order[i] = SortLevel[i].Order;
            }
            return new Database.Operation.SortedTable(tableIn, columnIndex, order, 0, range);
        }

        public override IEnumerable<Filter> SubFilters()
        {
            yield break;
        }

        public override bool OnGui(Database.Table sourceTable, ref bool dirty)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            int iLevelToRemove = -1;
            for (int i = 0; i != SortLevel.Count; ++i)
            {
                string sortName = GetSortName(SortLevel[i].Order);
                string colName = SortLevel[i].GetColumnName(sourceTable);

                EditorGUILayout.BeginHorizontal();
                if (OnGui_RemoveButton())
                {
                    iLevelToRemove = i;
                }
                GUILayout.Label("Sort" + sortName + " '" + colName + "'");
                EditorGUILayout.EndHorizontal();
            }
            if (iLevelToRemove >= 0)
            {
                dirty = true;
                SortLevel.RemoveAt(iLevelToRemove);
            }

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //remove this filter if it's empty
            return SortLevel.Count == 0;
        }

        public override void UpdateColumnState(Database.Table sourceTable, ColumnState[] colState)
        {
            foreach (var l in SortLevel)
            {
                colState[l.GetColumnIndex(sourceTable)].Sorted = l.Order;
            }
        }

        public override bool Simplify(ref bool dirty)
        {
            return SortLevel.Count == 0;
        }
    }
}
