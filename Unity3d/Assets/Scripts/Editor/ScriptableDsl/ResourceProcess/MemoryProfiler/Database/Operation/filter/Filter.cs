using UnityEngine;
using System;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation.Filter
{
    internal class ColumnState
    {
        public SortOrder Sorted = SortOrder.None;
        public SortOrder DefaultSorted = SortOrder.None;
        public bool Grouped = false;
    }

    /// <summary>
    /// Information needed for cloning a filter
    /// </summary>
    internal class FilterCloning
    {
        Dictionary<Filter, Filter> m_Uniques = new Dictionary<Filter, Filter>();
        public Filter CloneUnique(Filter source)
        {
            Filter c;
            if (m_Uniques.TryGetValue(source, out c))
            {
                return c;
            }
            c = source.Clone(this);
            m_Uniques[source] = c;
            return c;
        }

        public Filter GetUnique(Filter source)
        {
            Filter c;
            if (m_Uniques.TryGetValue(source, out c))
            {
                return c;
            }
            return null;
        }

        public T GetFirstUniqueOf<T>() where T : Filter
        {
            foreach (var f in m_Uniques.Values)
            {
                if (f is T)
                {
                    return (T)f;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Represent a filter that takes in a table, apply some transformation and output a new table.
    /// The source table will not be modified, however a filter may return the source table if nothing was needed to do.
    /// TODO OnGui should be moved to UI namespace.
    /// </summary>
    internal abstract class Filter
    {
        public abstract Filter Clone(FilterCloning fc);
        public abstract Database.Table CreateFilter(Database.Table tableIn);
        public abstract Database.Table CreateFilter(Database.Table tableIn, ArrayRange range);

        public abstract IEnumerable<Filter> SubFilters();

        public virtual bool RemoveSubFilters(Filter f) { return false; }
        public virtual bool ReplaceSubFilters(Filter replaced, Filter with) { return false; }
        //Filter to replace with when it gets removed
        public virtual Filter GetSurrogate() { return null; }

        //return if the filter must be deleted
        public abstract bool OnGui(Database.Table sourceTable, ref bool dirty);

        public abstract void UpdateColumnState(Database.Table sourceTable, ColumnState[] colState);

        //return if the filter must be deleted
        public virtual bool Simplify(ref bool dirty) { return false; }

        public static bool RemoveFilter(Filter parent, Filter childToRemove)
        {
            Filter surrogate = childToRemove.GetSurrogate();
            if (surrogate != null)
            {
                return parent.ReplaceSubFilters(childToRemove, surrogate);
            }
            return parent.RemoveSubFilters(childToRemove);
        }

        public static string GetSortName(SortOrder order)
        {
            switch (order)
            {
                case SortOrder.None:
                    return "";
                case SortOrder.Ascending:
                    return "▲";
                case SortOrder.Descending:
                    return "▼";
            }
            throw new Exception("Bad Sort Order");
        }

        public static bool OnGui_RemoveButton()
        {
            if (GUILayout.Button("X"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
