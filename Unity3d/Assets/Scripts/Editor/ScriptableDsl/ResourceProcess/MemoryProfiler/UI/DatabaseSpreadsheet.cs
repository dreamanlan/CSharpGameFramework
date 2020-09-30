using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Filter = Unity.MemoryProfilerForExtension.Editor.Database.Operation.Filter;
using Unity.MemoryProfilerForExtension.Editor.Database.Operation;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    /// <summary>
    /// A spreadsheet containing data from a Database.Table
    /// </summary>
    internal class DatabaseSpreadsheet : TextSpreadsheet
    {
        protected Database.Table m_TableSource;
        protected Database.Table m_TableDisplay;
        protected FormattingOptions m_FormattingOptions;

        const string k_DisplayWidthPrefKeyBase = "Unity.MemoryProfilerForExtension.Editor.Database.DisplayWidth";
        string[] m_DisplayWidthPrefKeysPerColumn;

        public int GetDisplayWidth(int columnIndex, int defaultDisplayWidth)
        {
            return EditorPrefs.GetInt(m_DisplayWidthPrefKeysPerColumn[columnIndex], defaultDisplayWidth);
        }

        public void SetDisplayWidth(int columnIndex, int value)
        {
            value = Mathf.Max(value, UI.SplitterStateEx.MinSplitterSize);
            EditorPrefs.SetInt(m_DisplayWidthPrefKeysPerColumn[columnIndex], value);
        }

        public Database.Table SourceTable
        {
            get
            {
                return m_TableSource;
            }
        }

        public Database.Table DisplayTable
        {
            get
            {
                return m_TableDisplay;
            }
        }

        public long RowCount
        {
            get
            {
                return m_TableDisplay.GetRowCount();
            }
        }

        public long SelectedRow
        {
            get
            {
                return m_GUIState.SelectedRow;
            }

            set
            {
                m_GUIState.SelectedRow = value;
            }
        }

        //keep the state of each column's desired filters
        //does not contain order in which the filters should be applied
        //in order of m_TableSource columns
        protected Filter.ColumnState[] m_ColumnState;

        Filter.Multi m_Filters = new Filter.Multi();
        Filter.Sort m_AllLevelSortFilter = new Filter.Sort();
        //filter.DefaultSort allLevelDefaultSort = new filter.DefaultSort();

        public struct State
        {
            public Database.Operation.Filter.Filter Filter;
            public Database.CellLink SelectedCell;
            public Vector2 ScrollPosition;
            public List<Database.CellPosition> ExpandedCells;

            public long SelectedRow;
            public long FirstVisibleRow;
            public float FirstVisibleRowY;
            public long FirstVisibleRowIndex;//sequential index assigned to all visible row. Differ from row index if there are invisible rows
            public double HeightBeforeFirstVisibleRow;//using double since this value will be maintained by offseting it.
        }

        public State CurrentState
        {
            get
            {
                State state = new State();

                state.SelectedCell = GetLinkToCurrentSelection();

                state.Filter = GetCurrentFilterCopy();
                state.ScrollPosition = m_GUIState.ScrollPosition;
                state.SelectedRow = m_GUIState.SelectedRow;
                state.FirstVisibleRow = m_GUIState.FirstVisibleRow;
                state.FirstVisibleRowIndex = m_GUIState.FirstVisibleRowIndex;
                state.FirstVisibleRowY = m_GUIState.FirstVisibleRowY;
                state.HeightBeforeFirstVisibleRow = m_GUIState.HeightBeforeFirstVisibleRow;

                state.ExpandedCells = new List<Database.CellPosition>();
                var rowCount = DisplayTable.GetRowCount();
                var columnCount = DisplayTable.GetMetaData().GetColumnCount();
                for (long row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < columnCount; col++)
                    {
                        var expendedState = DisplayTable.GetCellExpandState(row, col);
                        if (expendedState.isColumnExpandable && expendedState.isExpanded)
                        {
                            state.ExpandedCells.Add(new Database.CellPosition(row, col));
                        }
                    }
                }

                return state;
            }
            set
            {
                InitFilter(value.Filter, value.ExpandedCells);

                m_GUIState.ScrollPosition = value.ScrollPosition;
                m_GUIState.SelectedRow = value.SelectedRow;
                m_GUIState.FirstVisibleRow = value.FirstVisibleRow;
                m_GUIState.FirstVisibleRowIndex = value.FirstVisibleRowIndex;
                m_GUIState.FirstVisibleRowY = value.FirstVisibleRowY;
                m_GUIState.HeightBeforeFirstVisibleRow = value.HeightBeforeFirstVisibleRow;
            }
        }


        public DatabaseSpreadsheet(FormattingOptions formattingOptions, Database.Table table, IViewEventListener listener, State state)
            : base(listener)
        {
            m_TableSource = table;
            m_TableDisplay = table;
            m_FormattingOptions = formattingOptions;

            InitSplitter();
            CurrentState = state;
        }

        public DatabaseSpreadsheet(FormattingOptions formattingOptions, Database.Table table, IViewEventListener listener)
            : base(listener)
        {
            m_TableSource = table;
            m_TableDisplay = table;
            m_FormattingOptions = formattingOptions;

            InitSplitter();
            InitDefaultTableFilter();
        }

        private void InitSplitter()
        {
            var meta = m_TableSource.GetMetaData();
            int colCount = meta.GetColumnCount();
            m_ColumnState = new Filter.ColumnState[colCount];
            int[] colSizes = new int[colCount];

            string basePrefKey = k_DisplayWidthPrefKeyBase /*+ DisplayTable.GetName()*/;
            m_DisplayWidthPrefKeysPerColumn = new string[colCount];
            for (int i = 0; i != colCount; ++i)
            {
                var column = meta.GetColumnByIndex(i);
                m_DisplayWidthPrefKeysPerColumn[i] = basePrefKey + column.Name;
                colSizes[i] = GetDisplayWidth(i, column.DefaultDisplayWidth);
                m_ColumnState[i] = new Filter.ColumnState();
            }
            m_Splitter = new SplitterStateEx(colSizes);
            m_Splitter.RealSizeChanged += SetDisplayWidth;
        }

        private void InitEmptyFilter(List<Database.CellPosition> expandedCells = null)
        {
            m_Filters = new Filter.Multi();
            var ds = new Database.Operation.Filter.DefaultSort(m_AllLevelSortFilter, null);
            m_Filters.filters.Add(ds);
            UpdateDisplayTable(expandedCells);
        }

        protected void InitFilter(Database.Operation.Filter.Filter filter, List<Database.CellPosition> expandedCells = null)
        {
            Database.Operation.Filter.FilterCloning fc = new Database.Operation.Filter.FilterCloning();

            var deffilter = filter.Clone(fc);
            if (deffilter != null)
            {
                m_Filters = new Filter.Multi();

                m_Filters.filters.Add(deffilter);

                m_AllLevelSortFilter = fc.GetFirstUniqueOf<Filter.Sort>();
                if (m_AllLevelSortFilter == null)
                {
                    m_AllLevelSortFilter = new Filter.Sort();
                    var ds = new Database.Operation.Filter.DefaultSort(m_AllLevelSortFilter, null);
                    m_Filters.filters.Add(ds);
                }
                bool bDirty = false;
                m_Filters.Simplify(ref bDirty);
                UpdateDisplayTable(expandedCells);
            }
            else
            {
                InitEmptyFilter(expandedCells);
            }
        }

        protected void InitDefaultTableFilter()
        {
            var meta = m_TableSource.GetMetaData();
            if (meta.defaultFilter == null)
            {
                InitEmptyFilter();
                return;
            }
            InitFilter(meta.defaultFilter);
        }

        public Database.CellLink GetLinkToCurrentSelection()
        {
            if (m_GUIState.SelectedRow >= 0)
            {
                return m_TableDisplay.GetLinkTo(new Database.CellPosition(m_GUIState.SelectedRow, 0));
            }
            return null;
        }

        public Database.CellLink GetLinkToFirstVisible()
        {
            if (m_GUIState.FirstVisibleRow >= 0)
            {
                return m_TableDisplay.GetLinkTo(new Database.CellPosition(m_GUIState.FirstVisibleRow, 0));
            }
            return null;
        }

        public Database.Operation.Filter.Filter GetCurrentFilterCopy()
        {
            Database.Operation.Filter.FilterCloning fc = new Database.Operation.Filter.FilterCloning();
            return m_Filters.Clone(fc);
        }

        public void Goto(Database.CellLink cl)
        {
            Goto(cl.Apply(m_TableDisplay));
        }

        protected override long GetFirstRow()
        {
            long c = m_TableDisplay.GetRowCount();
            if (c <= 0) return -1;
            return 0;
        }

        protected override long GetNextVisibleRow(long row)
        {
            row += 1;
            if (row >= m_TableDisplay.GetRowCount())
            {
                return -1;
            }
            return row;
        }

        protected override long GetPreviousVisibleRow(long row)
        {
            return row - 1;
        }

        protected override DirtyRowRange SetCellExpanded(long row, long col, bool expanded)
        {
            DirtyRowRange o;
            o.Range = m_TableDisplay.ExpandCell(row, (int)col, expanded);
            o.HeightOffset = k_RowHeight * o.Range.Length;
            return o;
        }

        protected override bool GetCellExpanded(long row, long col)
        {
            return m_TableDisplay.GetCellExpandState(row, (int)col).isExpanded;
        }

        protected override void DrawHeader(long col, Rect r, ref GUIPipelineState pipe)
        {
            var colState = m_ColumnState[col];

            string str = m_FormattingOptions.ObjectDataFormatter.ShowPrettyNames
                ? m_TableDisplay.GetMetaData().GetColumnByIndex((int)col).DisplayName
                : m_TableDisplay.GetMetaData().GetColumnByIndex((int)col).Name;
            if (colState.Grouped)
            {
                str = "[" + str + "]";
            }
            var sorted = colState.Sorted != SortOrder.None ? colState.Sorted : colState.DefaultSorted;
            var sortName = Filter.Sort.GetSortName(sorted);
            str = sortName + str;

            if (!GUI.Button(r, str, Styles.General.Header))
                return;

            var meta = m_TableSource.GetMetaData();
            var metaCol = meta.GetColumnByIndex((int)col);
            bool canGroup = false;
            if (metaCol != null)
            {
                if (metaCol.DefaultGroupAlgorithm != null)
                {
                    canGroup = true;
                }
            }

            var menu = new GenericMenu();
            const string strGroup = "Group";
            const string strSortAsc = "Sort Ascending";
            const string strSortDsc = "Sort Descending";
            const string strMatch = "Match...";
            if (canGroup)
            {
                menu.AddItem(new GUIContent(strGroup), colState.Grouped, () =>
                {
                    if (colState.Grouped)
                        RemoveSubGroupFilter((int)col);
                    else
                        AddSubGroupFilter((int)col);
                });
            }

            menu.AddItem(new GUIContent(strSortAsc), sorted == SortOrder.Ascending, () =>
            {
                if (sorted == SortOrder.Ascending)
                    RemoveDefaultSortFilter();
                else
                    SetDefaultSortFilter((int)col, SortOrder.Ascending);
            });

            menu.AddItem(new GUIContent(strSortDsc), sorted == SortOrder.Descending, () =>
            {
                if (sorted == SortOrder.Descending)
                    RemoveDefaultSortFilter();
                else
                    SetDefaultSortFilter((int)col, SortOrder.Descending);
            });

            menu.AddItem(new GUIContent(strMatch), false, () =>
            {
                AddMatchFilter((int)col);
            });
            menu.DropDown(r);
        }

        List<MemoryProfilerAnalytics.Filter> m_FilterBuffer = new List<MemoryProfilerAnalytics.Filter>();

        void ReportFilterChanges()
        {
            m_FilterBuffer.Clear();
            foreach (var filter in m_Filters.filters)
            {
                if (filter is Filter.Sort)
                {
                    var sortFilter = (filter as Filter.Sort);
                    var level = (sortFilter.SortLevel != null && sortFilter.SortLevel.Count > 0) ? sortFilter.SortLevel[0] : null;
                    if (level == null)
                        continue;
                    string columnName = GetColumnName(level);
                    m_FilterBuffer.Add(new MemoryProfilerAnalytics.Filter() {column = columnName, filterName = "Sort", filterInput = level.Order.ToString() });
                }
                else if (filter is Filter.DefaultSort)
                {
                    var sortFilter = (filter as Filter.DefaultSort);
                    Filter.Sort.Level level = null;
                    if (sortFilter.SortOverride != null && sortFilter.SortOverride.SortLevel != null && sortFilter.SortOverride.SortLevel.Count > 0)
                        level = sortFilter.SortOverride.SortLevel[0];
                    if ((level == null || level.Order == SortOrder.None) && sortFilter.SortDefault != null && sortFilter.SortDefault.SortLevel != null && sortFilter.SortDefault.SortLevel.Count > 0)
                        level = sortFilter.SortDefault.SortLevel[0];
                    if (level == null)
                        continue;
                    string columnName = GetColumnName(level);
                    m_FilterBuffer.Add(new MemoryProfilerAnalytics.Filter() { column = columnName, filterName = "Sort", filterInput = level != null ? level.Order.ToString() : null });
                }
                else if (filter is Filter.Group)
                {
                    m_FilterBuffer.Add(new MemoryProfilerAnalytics.Filter() { column = (filter as Filter.Group).GetColumnName(m_TableDisplay), filterName = "Group"});
                }
                else if (filter is Filter.Match)
                {
                    var matchFilter = (filter as Filter.Match);
                    m_FilterBuffer.Add(new MemoryProfilerAnalytics.Filter() { column = matchFilter.GetColumnName(m_TableDisplay), filterName = "Match", filterInput = matchFilter.MatchString });
                }
            }
            MemoryProfilerAnalytics.FiltersChanged(m_TableDisplay.GetName(),  m_FilterBuffer);
        }

        string GetColumnName(Filter.Sort.Level sortLevel)
        {
            if (sortLevel is Filter.Sort.LevelByIndex)
            {
                return m_TableDisplay.GetMetaData().GetColumnByIndex((sortLevel as Filter.Sort.LevelByIndex).ColumnIndex).Name;
            }
            else if (sortLevel is Filter.Sort.LevelByName)
            {
                return (sortLevel as Filter.Sort.LevelByName).ColumnName;
            }
            return "";
        }

        public void UpdateTable()
        {
            var updater = m_TableDisplay.BeginUpdate();
            if (updater != null)
            {
                long sel = updater.OldToNewRow(m_GUIState.SelectedRow);

                //find the row that is still the first visible or the previous one that still exist after the uptate
                long fvr = -1;
                long fvr_before = m_GUIState.FirstVisibleRow;
                do
                {
                    fvr = updater.OldToNewRow(fvr_before);
                    --fvr_before;
                }
                while (fvr < 0 && fvr_before >= 0);

                //if did not find any valid first visible row, use selected row
                if (fvr < 0)
                {
                    fvr = sel;
                }

                m_TableSource.EndUpdate(updater);

                if (fvr >= 0)
                {
                    long nextRow;
                    long fvrIndex = 0;
                    float fvrY = GetCumulativeHeight(GetFirstRow(), fvr, out nextRow, ref fvrIndex);
                    long lastIndex = fvrIndex;
                    float totalh = fvrY + GetCumulativeHeight(nextRow, long.MaxValue, out nextRow, ref lastIndex);


                    m_GUIState.ScrollPosition.y = fvrY;
                    m_GUIState.FirstVisibleRowY = fvrY;
                    m_GUIState.FirstVisibleRow = fvr;
                    m_GUIState.FirstVisibleRowIndex = fvrIndex;
                    m_GUIState.HeightBeforeFirstVisibleRow = fvrY;
                    m_TotalDataHeight = totalh;
                }
                else
                {
                    m_GUIState.ScrollPosition = Vector2.zero;
                    m_GUIState.FirstVisibleRowY = 0;
                    m_GUIState.FirstVisibleRow = GetFirstRow();
                    m_GUIState.FirstVisibleRowIndex = 0;
                    m_GUIState.HeightBeforeFirstVisibleRow = 0;
                    long nextRow;
                    long lastIndex = 0;
                    m_TotalDataHeight = GetCumulativeHeight(GetFirstRow(), long.MaxValue, out nextRow, ref lastIndex);
                }

                m_GUIState.SelectedRow = sel;
                //m_Listener.OnRepaint();
            }
            else
            {
                m_TableDisplay.EndUpdate(updater);
            }
            //UpdateDataState();
            //ResetGUIState();
        }

        public void UpdateDisplayTable(List<Database.CellPosition> expandedCells = null)
        {
            UpdateColumnState();
            m_TableDisplay = m_Filters.CreateFilter(m_TableSource);

            UpdateExpandedState(expandedCells);
            UpdateDataState();
            ResetGUIState();
        }

        void UpdateExpandedState(List<Database.CellPosition> expandedCells)
        {
            if (expandedCells == null)
                return;
            foreach (var cell in expandedCells)
            {
                m_TableDisplay.ExpandCell(cell.row, cell.col, true);
            }
        }

        protected override void DrawCell(long row, long col, Rect r, long index, bool selected, ref GUIPipelineState pipe)
        {
            var s = m_TableDisplay.GetCellExpandState(row, (int)col);

            if (s.isColumnExpandable)
            {
                int indent = s.expandDepth * 16;
                r.x += indent;
                r.width -= indent;
                if (s.isExpandable)
                {
                    Rect rToggle = new Rect(r.x, r.y, Styles.General.FoldoutWidth, r.height);
                    bool newExpanded = GUI.Toggle(rToggle, s.isExpanded, GUIContent.none, Styles.General.Foldout);
                    if (newExpanded != s.isExpanded)
                    {
                        pipe.processMouseClick = false;
                        SetCellExpandedState(row, col, newExpanded);
                    }
                }
                r.x += 16;
                r.width -= 16;
            }

            Database.LinkRequest link = null;
            if (onClickLink != null)
            {
                link = m_TableDisplay.GetCellLink(new Database.CellPosition(row, (int)col));
            }
            if (Event.current.type == EventType.Repaint)
            {
                var column = m_TableDisplay.GetColumnByIndex((int)col);
                var metaColumn = m_TableDisplay.GetMetaData().GetColumnByIndex((int)col);
                if (column != null)
                {
                    var str = column.GetRowValueString(row, m_FormattingOptions.GetFormatter(metaColumn.FormatName));
                    DrawTextEllipsis(str, r,
                        link == null ? Styles.General.NumberLabel : Styles.General.ClickableLabel
                        , EllipsisStyleMetricData, selected);
                }
            }
            if (link != null)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
                }
            }
        }

        protected override void OnGUI_CellMouseMove(Database.CellPosition pos)
        {
        }

        protected override void OnGUI_CellMouseDown(Database.CellPosition pos)
        {
            //UnityEngine.Debug.Log("MouseDown at (" + Event.current.mousePosition.x + ", " + Event.current.mousePosition.y + " row:" + row + " col:" + col);
        }

        protected override void OnGUI_CellMouseUp(Database.CellPosition pos)
        {
            if (onClickLink != null)
            {
                var link = m_TableDisplay.GetCellLink(pos);
                if (link != null)
                {
                    onClickLink(this, link, pos);
                }
            }
        }

        public delegate void OnClickLink(DatabaseSpreadsheet sheet, Database.LinkRequest link, Database.CellPosition pos);
        public OnClickLink onClickLink;


        // update m_ColumnState from filters
        protected void UpdateColumnState()
        {
            long colCount = m_TableSource.GetMetaData().GetColumnCount();
            for (long i = 0; i != colCount; ++i)
            {
                m_ColumnState[i] = new Filter.ColumnState();
            }

            m_Filters.UpdateColumnState(m_TableSource, m_ColumnState);
        }

        public bool RemoveSubSortFilter(int colIndex, bool update = true)
        {
            if (m_AllLevelSortFilter.SortLevel.RemoveAll(x => x.GetColumnIndex(m_TableSource) == colIndex) > 0)
            {
                bool dirty = false;
                m_Filters.Simplify(ref dirty);
                if (update)
                {
                    UpdateDisplayTable();
                }
                return true;
            }
            ReportFilterChanges();
            return false;
        }

        // return if something change
        public bool AddSubSortFilter(int colIndex, SortOrder ss, bool update = true)
        {
            Filter.Sort.Level sl = new Filter.Sort.LevelByIndex(colIndex, ss);
            int index = m_AllLevelSortFilter.SortLevel.FindIndex(x => x.GetColumnIndex(m_TableSource) == colIndex);
            if (index >= 0)
            {
                if (m_AllLevelSortFilter.SortLevel[index].Equals(sl)) return false;
                m_AllLevelSortFilter.SortLevel[index] = sl;
            }
            else
            {
                m_AllLevelSortFilter.SortLevel.Add(sl);
            }
            if (update)
            {
                UpdateDisplayTable();
            }
            ReportFilterChanges();
            return true;
        }

        // return if something change
        public bool RemoveDefaultSortFilter(bool update = true)
        {
            bool changed = m_AllLevelSortFilter.SortLevel.Count > 0;
            m_AllLevelSortFilter.SortLevel.Clear();
            if (changed && update)
            {
                UpdateDisplayTable();
            }
            if (changed)
                ReportFilterChanges();
            return changed;
        }

        public bool SetDefaultSortFilter(int colIndex, SortOrder ss, bool update = true)
        {
            m_AllLevelSortFilter.SortLevel.Clear();

            if (ss != SortOrder.None)
            {
                Filter.Sort.Level sl = new Filter.Sort.LevelByIndex(colIndex, ss);
                m_AllLevelSortFilter.SortLevel.Add(sl);
            }
            if (update)
            {
                UpdateDisplayTable();
            }
            ReportFilterChanges();
            return true;
        }

        // return if something change
        public bool AddSubGroupFilter(int colIndex, bool update = true)
        {
            var newFilter = new Filter.GroupByColumnIndex(colIndex, SortOrder.Ascending);


            var ds = new Database.Operation.Filter.DefaultSort(m_AllLevelSortFilter, null);

            var gfp = GetDeepestGroupFilter(m_Filters);
            if (gfp.child != null)
            {
                //add the new group with the default sort filter
                var newMulti = new Filter.Multi();
                newMulti.filters.Add(newFilter);
                newMulti.filters.Add(ds);
                var subf = gfp.child.SubGroupFilter;
                gfp.child.SubGroupFilter = newMulti;
                newFilter.SubGroupFilter = subf;
            }
            else
            {
                //add it to top, already has te default sort filter there
                newFilter.SubGroupFilter = ds;
                m_Filters.filters.Insert(0, newFilter);
            }

            if (update)
            {
                UpdateDisplayTable();
            }
            ReportFilterChanges();
            return true;
        }

        // return if something change
        public bool RemoveSubGroupFilter(long colIndex, bool update = true)
        {
            FilterParenthood<Filter.Filter, Filter.Group> fpToRemove = new FilterParenthood<Filter.Filter, Filter.Group>();

            foreach (var fp in VisitAllSubGroupFilters(m_Filters))
            {
                if (fp.child.GetColumnIndex(m_TableSource) == colIndex)
                {
                    fpToRemove = fp;
                    break;
                }
            }

            if (fpToRemove.child != null)
            {
                if (Filter.Filter.RemoveFilter(fpToRemove.parent, fpToRemove.child))
                {
                    bool dirty = false;
                    m_Filters.Simplify(ref dirty);
                    if (update)
                    {
                        UpdateDisplayTable();
                    }
                    ReportFilterChanges();
                    return true;
                }
            }

            return false;
        }

        public bool AddMatchFilter(int colIndex, string matchString = "", bool update = true)
        {
            var newFilter = new Filter.Match(colIndex, matchString);

            m_Filters.filters.Insert(0, newFilter);

            if (update)
            {
                UpdateDisplayTable();
            }
            ReportFilterChanges();
            return true;
        }

        protected struct FilterParenthood<PFilter, CFilter> where PFilter : Filter.Filter where CFilter : Filter.Filter
        {
            public FilterParenthood(PFilter parent, CFilter child)
            {
                this.parent = parent;
                this.child = child;
            }

            public static implicit operator FilterParenthood<Filter.Filter, Filter.Filter>(FilterParenthood<PFilter, CFilter> a)
            {
                FilterParenthood<Filter.Filter, Filter.Filter> o = new FilterParenthood<Filter.Filter, Filter.Filter>();
                o.parent = a.parent;
                o.child = a.child;
                return o;
            }
            public PFilter parent;
            public CFilter child;
        }

        protected IEnumerable<FilterParenthood<Filter.Filter, Filter.Group>> VisitAllSubGroupFilters(Filter.Filter filter)
        {
            foreach (var f in filter.SubFilters())
            {
                if (f is Filter.Group)
                {
                    Filter.Group gf = (Filter.Group)f;
                    yield return new FilterParenthood<Filter.Filter, Filter.Group>(filter, gf);
                }
                foreach (var f2 in VisitAllSubGroupFilters(f))
                {
                    yield return f2;
                }
            }
        }

        protected FilterParenthood<Filter.Filter, Filter.Group> GetFirstSubGroupFilter(Filter.Filter filter)
        {
            var e = VisitAllSubGroupFilters(filter).GetEnumerator();
            if (e.MoveNext()) return e.Current;
            return new FilterParenthood<Filter.Filter, Filter.Group>();
        }

        protected FilterParenthood<Filter.Filter, Filter.Group> GetDeepestGroupFilter(Filter.Filter filter)
        {
            foreach (var f in filter.SubFilters())
            {
                var sgf = GetDeepestGroupFilter(f);
                if (sgf.child != null) return sgf;

                if (f is Filter.Group)
                {
                    Filter.Group gf = (Filter.Group)f;
                    return new FilterParenthood<Filter.Filter, Filter.Group>(filter, gf);
                }
            }

            return new FilterParenthood<Filter.Filter, Filter.Group>();
        }

        public void OnGui_Filters()
        {
            bool dirty = false;

            EditorGUILayout.BeginVertical();
            m_Filters.OnGui(m_TableDisplay, ref dirty);
            m_AllLevelSortFilter.OnGui(m_TableDisplay, ref dirty);
            EditorGUILayout.EndVertical();
            if (dirty)
            {
                UpdateDisplayTable();
                ReportFilterChanges();
            }
        }
    }
}
