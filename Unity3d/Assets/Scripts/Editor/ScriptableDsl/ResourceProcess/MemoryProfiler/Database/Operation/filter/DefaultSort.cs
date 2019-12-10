using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    namespace Operation
    {
        namespace Filter
        {
            /// <summary>
            /// Filter that sort entries using a default Sort filter and may be overridden with a different Sort filter
            /// </summary>
            internal class DefaultSort : Filter
            {
                public Sort SortDefault;
                public Sort SortOverride;

                public DefaultSort() {}
                public DefaultSort(Sort sortDefault, Sort sortOverride)
                {
                    SortDefault = sortDefault;
                    SortOverride = sortOverride;
                }

                public override Filter Clone(FilterCloning fc)
                {
                    DefaultSort o = new DefaultSort();
                    o.SortDefault = (Sort)fc.CloneUnique(SortDefault);
                    if (SortOverride != null) o.SortOverride = (Sort)SortOverride.Clone(fc);
                    return o;
                }

                public override Database.Table CreateFilter(Database.Table tableIn)
                {
                    if (SortOverride != null) return SortOverride.CreateFilter(tableIn);
                    if (SortDefault != null) return SortDefault.CreateFilter(tableIn);
                    return tableIn;
                }

                public override Database.Table CreateFilter(Database.Table tableIn, ArrayRange range)
                {
                    if (SortOverride != null) return SortOverride.CreateFilter(tableIn, range);
                    if (SortDefault != null) return SortDefault.CreateFilter(tableIn, range);
                    return new Database.Operation.IndexedTable(tableIn, range);
                }

                public override IEnumerable<Filter> SubFilters()
                {
                    if (SortOverride != null)
                    {
                        yield return SortOverride;
                    }
                    else
                    {
                        yield return SortDefault;
                    }
                }

                public override bool RemoveSubFilters(Filter f)
                {
                    if (f == SortOverride)
                    {
                        SortOverride = null;
                        return true;
                    }
                    return false;
                }

                public override bool ReplaceSubFilters(Filter replaced, Filter with)
                {
                    if (replaced == SortOverride)
                    {
                        if (with is Sort)
                        {
                            SortOverride = (Sort)with;
                            return true;
                        }
                    }
                    return false;
                }

                //Filter to replace with when it gets removed
                public override Filter GetSurrogate() { return null; }

                //return if the filter must be deleted
                public override bool OnGui(Database.Table sourceTable, ref bool dirty)
                {
                    return false;
                }

                public override void UpdateColumnState(Database.Table sourceTable, ColumnState[] colState)
                {
                    if (SortOverride != null)
                    {
                        SortOverride.UpdateColumnState(sourceTable, colState);
                    }
                    else if (SortDefault != null)
                    {
                        foreach (var l in SortDefault.SortLevel)
                        {
                            colState[l.GetColumnIndex(sourceTable)].DefaultSorted = l.Order;
                        }
                    }
                }

                public override bool Simplify(ref bool dirty)
                {
                    if (SortOverride != null)
                    {
                        SortOverride.Simplify(ref dirty);
                    }
                    return false;
                }
            }
        }
    }
}
