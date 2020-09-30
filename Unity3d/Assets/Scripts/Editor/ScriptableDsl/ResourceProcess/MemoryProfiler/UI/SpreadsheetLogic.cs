using UnityEngine;
using UnityEditor;
using System;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal interface IViewEventListener
    {
        void OnRepaint();
    }

    /// <summary>
    /// Defines the logic behind a spreadsheet.
    /// Handles scrolling, moving to a specific cell, compute where to draw cells
    /// The actual drawing of the cell's content is left abstract and must be defined in a sub-class
    /// </summary>
    internal abstract class SpreadsheetLogic
    {
        //used to catch infinite loop caused by implementation
        const long k_MaxRow = 100000000;

        protected const float k_SmallMargin = 4;
        WeakReference m_Listener;

        protected SplitterStateEx m_Splitter;

        //total height of all data without scroll region. used to optimize layout calls
        protected double m_TotalDataHeight = -1; //using double since this value will be maintained by offseting it.
        protected class GUIState
        {
            //rect of the scroll region for displaying data. relative to current window.
            public Rect RectHeader;
            public Rect RectData;
            public bool HasRectData;
            public Vector2 ScrollPosition;

            public long FirstVisibleRow;
            public float FirstVisibleRowY;
            public long FirstVisibleRowIndex;//sequential index assigned to all visible row. Differ from row index if there are invisible rows
            public double HeightBeforeFirstVisibleRow;//using double since this value will be maintained by offseting it.
            public long SelectedRow = -1;

            public GUIState() {}

            public GUIState(GUIState copy)
            {
                RectHeader = copy.RectHeader;
                RectData = copy.RectData;
                HasRectData = copy.HasRectData;
                ScrollPosition = copy.ScrollPosition;
                FirstVisibleRow = copy.FirstVisibleRow;
                FirstVisibleRowY = copy.FirstVisibleRowY;
                FirstVisibleRowIndex = copy.FirstVisibleRowIndex;
                HeightBeforeFirstVisibleRow = copy.HeightBeforeFirstVisibleRow;
                SelectedRow = copy.SelectedRow;
            }
        };
        protected GUIState m_GUIState = new GUIState();
        protected GUIState m_DelayedUpdateGUIState = null;
        public SpreadsheetLogic(IViewEventListener listener) : this(null, listener) {}

        public SpreadsheetLogic(SplitterStateEx splitter, IViewEventListener listener)
        {
            m_Splitter = splitter;
            m_Listener = new WeakReference(listener);
        }

        protected abstract float GetRowHeight(long row);

        public struct DirtyRowRange
        {
            public Range Range;
            public float HeightOffset;
            public static DirtyRowRange NonDirty
            {
                get
                {
                    DirtyRowRange o;
                    o.Range = Range.None;
                    o.HeightOffset = 0;
                    return o;
                }
            }
        }
        protected abstract DirtyRowRange SetCellExpanded(long row, long col, bool expanded);
        protected abstract bool GetCellExpanded(long row, long col);

        //return -1 when reach the end.
        protected abstract long GetFirstRow();
        protected abstract long GetNextVisibleRow(long row);
        protected abstract long GetPreviousVisibleRow(long row);

        protected class GUIPipelineState
        {
            public bool processMouseClick = true;
        }

        protected abstract void DrawHeader(long col, Rect r, ref GUIPipelineState pipe);
        protected abstract void DrawRow(long row, Rect r, long index, bool selected, ref GUIPipelineState pipe);
        protected abstract void DrawCell(long row, long col, Rect r, long index, bool abSelected, ref GUIPipelineState pipe);

        protected float GetCumulativeHeight(long rowMin, long rowMaxExclusive, out long outNextRow, ref long rowMinIndex)
        {
            float h = 0;
            long i = rowMin;
            for (; i >= 0 && i < rowMaxExclusive && i >= 0; i = GetNextVisibleRow(i))
            {
                h += GetRowHeight(i);
                ++rowMinIndex;
            }
            outNextRow = i;
            return h;
        }

        protected void ResetGUIState()
        {
            m_GUIState = new GUIState();
        }

        protected void UpdateDataState()
        {
            m_TotalDataHeight = 0;
            long iRowCount = 0;
            float h = 0;

            long i = 0;
            for (; i >= 0 && iRowCount < k_MaxRow; i = GetNextVisibleRow(i), ++iRowCount)
            {
                h += GetRowHeight(i);
            }
            UnityEngine.Debug.Assert(iRowCount < k_MaxRow, "GridSheet.UpdateDataState Reached " + k_MaxRow + " rows while computing data state, make sure GetNextTopLevelRow() eventually returns -1");


            m_TotalDataHeight = h;
        }

        public void UpdateDirtyRowRange(DirtyRowRange d)
        {
            m_TotalDataHeight += d.HeightOffset;
        }

        public void SetCellExpandedState(long row, long col, bool expanded)
        {
            DirtyRowRange dirtyRange = SetCellExpanded(row, col, expanded);
            UpdateDirtyRowRange(dirtyRange);
        }

        public void Goto(Database.CellPosition pos)
        {
            long rowIndex = 0;
            long nextRow = 0;
            var y = GetCumulativeHeight(0, pos.row, out nextRow, ref rowIndex);
            m_GUIState.ScrollPosition = new Vector2(0, y);
            m_GUIState.SelectedRow = pos.row;
            m_GUIState.FirstVisibleRow = pos.row < 0 ? 0 : pos.row;
            m_GUIState.FirstVisibleRowIndex = rowIndex;
            m_GUIState.FirstVisibleRowY = y;
            m_GUIState.HeightBeforeFirstVisibleRow = y;
        }

        protected virtual void OnGUI_CellMouseMove(Database.CellPosition pos)
        {
        }

        protected virtual void OnGUI_CellMouseDown(Database.CellPosition pos)
        {
        }

        protected virtual void OnGUI_CellMouseUp(Database.CellPosition pos)
        {
        }

        protected long GetColAtPosition(Vector2 pos)
        {
            var xLocal = pos.x - m_Splitter.m_TopLeft.x;
            long col = 0;
            while (col < m_Splitter.realSizes.Length)
            {
                xLocal -= m_Splitter.realSizes[col];
                if (xLocal < 0) return col;
                ++col;
            }
            return -1;
        }

        protected long GetRowAtPosition(Vector2 pos)
        {
            if (!m_GUIState.HasRectData) return -1;
            if (pos.x < m_GUIState.RectData.x || pos.y < m_GUIState.RectData.y) return -1;
            if (pos.x >= m_GUIState.RectData.xMax || pos.y >= m_GUIState.RectData.yMax) return -1;
            var vInData = pos - m_GUIState.RectData.position;
            //var yWorld = m_GUIState.m_ScrollPosition + vInData.y;
            var y = m_GUIState.ScrollPosition.y + vInData.y - m_GUIState.FirstVisibleRowY;

            float h = 0;
            long i = m_GUIState.FirstVisibleRowIndex;
            int iMax = 0;
            while (i >= 0 && iMax < k_MaxRow)
            {
                h += GetRowHeight(i);
                if (h >= y) break;
                i = GetNextVisibleRow(i);
                ++iMax;
            }
            return i;
        }

        public void OnGUI(Rect r)
        {
            GUI.BeginGroup(r);
            OnGUI(float.PositiveInfinity);
            GUI.EndGroup();
        }

        public void OnGUI(float maxWidth, params GUILayoutOption[] opt)
        {
            GUIPipelineState pipe = new GUIPipelineState();

            if (m_TotalDataHeight == -1)
            {
                UpdateDataState();
            }

            GUILayout.BeginVertical(opt);

            m_Splitter.BeginHorizontalSplit(-m_GUIState.ScrollPosition.x);
            Rect rectHeader = new Rect(m_Splitter.m_TopLeft.x, m_Splitter.m_TopLeft.y, 0, Styles.General.Header.fixedHeight);

            float totalHeaderWidth = 0;
            for (int k = 0; k < m_Splitter.realSizes.Length; ++k)
            {
                rectHeader.width = m_Splitter.realSizes[k];
                totalHeaderWidth += rectHeader.width;

                DrawHeader(k, rectHeader, ref pipe);

                rectHeader.x += rectHeader.width;
            }
            GUILayout.Space(rectHeader.width < maxWidth ? rectHeader.width : maxWidth);

            m_Splitter.EndHorizontalSplit();


            GUILayout.Space(rectHeader.height + 1);

            var rectHeader2 = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint)
            {
                m_GUIState.RectHeader = rectHeader2;
            }

            //Rect rr = GUILayoutUtility.GetRect(10, 10000, 10, 10000, Styles.styles.background);
            //Debug.Log("rr = " + rr.xMin + ", " + rr.yMin + " - " + rr.xMax + ", " + rr.yMax);

            Vector2 scrollBefore = m_GUIState.ScrollPosition;
            m_GUIState.ScrollPosition = GUILayout.BeginScrollView(scrollBefore);

            EditorGUILayout.BeginHorizontal();
            for (int k = 0; k < m_Splitter.realSizes.Length; ++k)
            {
                GUILayout.Space(m_Splitter.realSizes[k]);
            }
            EditorGUILayout.EndHorizontal();

            if (scrollBefore.y < m_GUIState.ScrollPosition.y)
            {
                //moved down
                double curYMin = m_GUIState.FirstVisibleRowY;
                double rowH = 0;
                double curYMax = m_GUIState.FirstVisibleRowY;
                double offsetY = 0;
                long curRow = 0;
                long nextRow = m_GUIState.FirstVisibleRow;
                long i = 0;
                do
                {
                    curRow = nextRow;
                    curYMin = curYMax;
                    offsetY += rowH;

                    rowH = GetRowHeight(curRow);
                    curYMax = curYMin + rowH;

                    nextRow = GetNextVisibleRow(nextRow);
                    ++i;
                }
                while (curYMax < m_GUIState.ScrollPosition.y && nextRow >= 0 && i < k_MaxRow);

                UnityEngine.Debug.Assert(i < k_MaxRow, "Reached " + k_MaxRow + " iteration while updating data state. make sure GetRowHeight does not always return 0 or GetNextVisibleRow eventually returns -1");

                var guiState = m_GUIState;
                if (Event.current.type == EventType.Repaint)
                {
                    // don't change the guiState during repaint
                    guiState = m_DelayedUpdateGUIState = new GUIState(m_GUIState);
                }
                guiState.FirstVisibleRow = curRow;
                guiState.FirstVisibleRowY = (float)curYMin;
                guiState.FirstVisibleRowIndex += i - 1;
                guiState.HeightBeforeFirstVisibleRow += offsetY;
            }
            else if (scrollBefore.y > m_GUIState.ScrollPosition.y)
            {
                //moved up
                float curYMin = m_GUIState.FirstVisibleRowY;
                float rowH = 0;
                long curRow = m_GUIState.FirstVisibleRow;
                long i = 0;
                double offsetY = 0;
                while (curYMin > m_GUIState.ScrollPosition.y && curRow >= 0 && i < k_MaxRow)
                {
                    var prevRow = GetPreviousVisibleRow(curRow);
                    if (prevRow < 0)
                    {
                        //can't move up any further. set all data to top of the list.
                        curYMin = 0;
                        offsetY = m_GUIState.HeightBeforeFirstVisibleRow;
                        break;
                    }
                    curRow = prevRow;
                    rowH = GetRowHeight(curRow);
                    offsetY += rowH;
                    curYMin = curYMin - rowH;
                    ++i;
                }
                UnityEngine.Debug.Assert(i < k_MaxRow, "Reached " + k_MaxRow + " iteration while updating data state. make sure GetRowHeight does not always return 0 or GetNextVisibleRow eventually returns -1");

                var guiState = m_GUIState;
                if (Event.current.type == EventType.Repaint)
                {
                    // don't change the guiState during repaint
                    guiState = m_DelayedUpdateGUIState = new GUIState(m_GUIState);
                }
                guiState.FirstVisibleRow = curRow;
                guiState.FirstVisibleRowY = curYMin;
                guiState.FirstVisibleRowIndex -= i;
                guiState.HeightBeforeFirstVisibleRow -= offsetY;
            }


            GUILayout.Space((float)m_GUIState.HeightBeforeFirstVisibleRow);

            double visibleRowTotalHeight = 0;

            float yMax = m_GUIState.ScrollPosition.y + m_GUIState.RectData.height;
            Rect r = new Rect(0
                , m_GUIState.FirstVisibleRowY
                , 0
                , 0);
            long firstRow = GetFirstRow();
            if (firstRow >= 0)
            {
                firstRow = m_GUIState.FirstVisibleRow;
            }
            for (long i = firstRow, j = 0; r.y < yMax && i < k_MaxRow && i >= 0; i = GetNextVisibleRow(i), ++j)
            {
                float h = GetRowHeight(i);
                r.height = h;
                visibleRowTotalHeight += h;
                r.x = 0;
                r.width = m_GUIState.RectData.width;
                Rect rRow = new Rect(m_GUIState.ScrollPosition.x, r.y, r.width, r.height);

                DrawRow(i, rRow, m_GUIState.FirstVisibleRowIndex + j, i == m_GUIState.SelectedRow, ref pipe);
                for (long k = 0; k < m_Splitter.realSizes.Length; ++k)
                {
                    if (k == 0)
                    {
                        r.xMax = m_Splitter.realSizes[k];
                    }
                    else
                    {
                        r.width = m_Splitter.realSizes[k] - k_SmallMargin;
                    }
                    if (m_Splitter.realSizes[k] > 0)
                    {
                        DrawCell(i, k, r, m_GUIState.FirstVisibleRowIndex + j, i == m_GUIState.SelectedRow, ref pipe);
                    }

                    r.x += m_Splitter.realSizes[k];
                }

                r.y += h;
            }

            double heightAfterVisibleRow = m_TotalDataHeight - (m_GUIState.HeightBeforeFirstVisibleRow + visibleRowTotalHeight);

            GUILayout.Space((float)heightAfterVisibleRow);

            GUILayout.EndScrollView();

           
            if (Event.current.type == EventType.Repaint)
            {
                m_GUIState.RectData = GUILayoutUtility.GetLastRect();
                if (m_GUIState.HasRectData == false)
                {
                    m_GUIState.HasRectData = true;

                    if (m_Listener != null && m_Listener.IsAlive)
                    {
                        ((IViewEventListener)m_Listener.Target).OnRepaint();
                    }
                }
            }


            GUILayout.EndVertical();


            if (pipe.processMouseClick)
            {
                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                    {
                        var row = GetRowAtPosition(Event.current.mousePosition);
                        if (row >= 0)
                        {
                            m_GUIState.SelectedRow = row;
                            if (m_Listener != null && m_Listener.IsAlive)
                            {
                                ((IViewEventListener)m_Listener.Target).OnRepaint();
                            }

                            OnGUI_CellMouseDown(new Database.CellPosition(row, (int)GetColAtPosition(Event.current.mousePosition)));
                        }
                        break;
                    }
                    case EventType.MouseUp:
                    {
                        var row = GetRowAtPosition(Event.current.mousePosition);
                        if (row >= 0)
                        {
                            var col = (int)GetColAtPosition(Event.current.mousePosition);
                            if (col >= 0)
                            {
                                OnGUI_CellMouseUp(new Database.CellPosition(row, col));
                            }
                        }
                        break;
                    }
                    case EventType.MouseMove:
                    {
                        var row = GetRowAtPosition(Event.current.mousePosition);
                        if (row >= 0)
                        {
                            var col = (int)GetColAtPosition(Event.current.mousePosition);
                            if (col >= 0)
                            {
                                OnGUI_CellMouseUp(new Database.CellPosition(row, col));
                            }
                        }
                        break;
                    }
                }
            }

            if (m_DelayedUpdateGUIState != null && Event.current.type == EventType.Repaint)
            {
                m_GUIState = m_DelayedUpdateGUIState;
                m_DelayedUpdateGUIState = null;

                if (m_Listener != null && m_Listener.IsAlive)
                {
                    ((IViewEventListener)m_Listener.Target).OnRepaint();
                }
            }
        }
    }
}
