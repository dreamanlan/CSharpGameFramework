using UnityEditor;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation.Filter
{
    /// <summary>
    /// A collection of filters applied sequentially
    /// </summary>
    internal class Multi : Filter
    {
        public List<Filter> filters = new List<Filter>();


        public override Filter Clone(FilterCloning fc)
        {
            Multi o = new Multi();
            o.filters.Capacity = filters.Count;
            foreach (var f in filters)
            {
                o.filters.Add(f.Clone(fc));
            }
            return o;
        }

        public override Database.Table CreateFilter(Database.Table tableIn)
        {
            Database.Table t = tableIn;
            foreach (var f in filters)
            {
                t = f.CreateFilter(t);
            }
            return t;
        }

        public override Database.Table CreateFilter(Database.Table tableIn, ArrayRange range)
        {
            Database.Operation.IndexedTable subTable = new Database.Operation.IndexedTable(tableIn, range);
            return CreateFilter(subTable);
        }

        public override IEnumerable<Filter> SubFilters()
        {
            foreach (var f in filters)
            {
                yield return f;
            }
        }

        public override bool RemoveSubFilters(Filter f)
        {
            return filters.Remove(f);
        }

        public override bool ReplaceSubFilters(Filter replaced, Filter with)
        {
            int index = filters.IndexOf(replaced);
            if (index >= 0)
            {
                filters[index] = with;
                return true;
            }
            return false;
        }

        public override bool OnGui(Database.Table sourceTable, ref bool dirty)
        {
            Filter filterToRemove = null;
            EditorGUILayout.BeginVertical();
            foreach (var f in filters)
            {
                bool bRemove = f.OnGui(sourceTable, ref dirty);
                if (bRemove)
                {
                    filterToRemove = f;
                    dirty = true;
                }
            }
            EditorGUILayout.EndVertical();
            if (filterToRemove != null)
            {
                RemoveFilter(this, filterToRemove);
            }
            //remove this filter if it's empty
            return filters.Count == 0;
        }

        public override void UpdateColumnState(Database.Table sourceTable, ColumnState[] colState)
        {
            foreach (var f in filters)
            {
                f.UpdateColumnState(sourceTable, colState);
            }
        }

        public override bool Simplify(ref bool dirty)
        {
            Filter previous = null;
            int i = 0;
            DefaultSort ds = null;
            while (i != filters.Count)
            {
                var f = filters[i];
                if (f is Multi)
                {
                    //merge sub multifilters into ourself
                    dirty = true;
                    filters.RemoveAt(i);
                    var mf = (Multi)f;
                    filters.InsertRange(i, mf.filters);
                    mf.filters.Clear();
                }
                else if (f.Simplify(ref dirty))
                {
                    //must remove it
                    dirty = true;
                    filters.RemoveAt(i);
                }
                else if (f == previous)
                {
                    //remove duplicated
                    dirty = true;
                    filters.RemoveAt(i);
                }
                else if (f is DefaultSort)
                {
                    //there should only be 1 DefaultSort and must be at the end
                    //if not last, remove it
                    if (i < filters.Count - 1)
                    {
                        ds = (DefaultSort)f;
                        filters.RemoveAt(i);
                    }
                    else
                    {
                        previous = f;
                        ++i;
                    }
                }
                else
                {
                    previous = f;
                    ++i;
                }
            }
            if (ds != null)
            {
                if (!(filters[filters.Count - 1] is DefaultSort))
                {
                    filters.Add(ds);
                }
            }

            return filters.Count == 0;
        }
    }
}
