using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    /// <summary>
    /// A spreadsheet with text rendering util methods
    /// </summary>
    internal abstract class TextSpreadsheet : SpreadsheetLogic
    {
        private EllipsisStyleMetric m_EllipsisStyleMetricData;
        private EllipsisStyleMetric m_EllipsisStyleMetricHeader;

        protected EllipsisStyleMetric EllipsisStyleMetricData
        {
            get
            {
                if (m_EllipsisStyleMetricData == null)
                    m_EllipsisStyleMetricData = new EllipsisStyleMetric(Styles.General.NumberLabel);

                return m_EllipsisStyleMetricData;
            }
        }
        protected EllipsisStyleMetric EllipsisStyleMetricHeader
        {
            get
            {
                if (m_EllipsisStyleMetricHeader == null)
                    m_EllipsisStyleMetricHeader = new EllipsisStyleMetric(Styles.General.EntryEven);

                return m_EllipsisStyleMetricHeader;
            }
        }

        protected const float k_RowHeight = 16;

        public TextSpreadsheet(SplitterStateEx splitter, IViewEventListener listener)
            : base(splitter, listener)
        {
        }

        public TextSpreadsheet(IViewEventListener listener)
            : base(listener)
        {
        }

        protected override float GetRowHeight(long row)
        {
            return k_RowHeight;
        }

        protected override void DrawRow(long row, Rect r, long index, bool selected, ref GUIPipelineState pipe)
        {
            if(Event.current.type == EventType.Layout)
                GUILayout.Space(r.height);

            if (Event.current.type == EventType.Repaint)
            {
                // TODO: clean this up when refactoring views to something more reliable when there are multiple MemoryProfilerWindow instances allowed.
                bool focused = EditorWindow.focusedWindow is MemoryProfilerWindow;
#if UNITY_2019_3_OR_NEWER
                if (selected)
                    Styles.General.EntrySelected.Draw(r, false, false, true, focused);
                else if(index % 2 == 0)
                    Styles.General.EntryEven.Draw(r, GUIContent.none, false, false, false, focused);

#else
                var background = (index % 2 == 0 ? Styles.General.EntryEven : Styles.General.EntryOdd);
                background.Draw(r, GUIContent.none, false, false, selected, focused);
#endif
            }
        }

        protected void DrawTextEllipsis(string text, Rect r, GUIStyle textStyle, EllipsisStyleMetric ellipsisStyle, bool selected)
        {
            Vector2 tSize = Styles.General.NumberLabel.CalcSize(new GUIContent(text));
            if (tSize.x > r.width)
            {
                Rect rclipped = new Rect(r.x, r.y, r.width - ellipsisStyle.pixelSize.x, r.height);
                textStyle.Draw(rclipped, text, false, false, false, false);
                Rect rEllipsis = new Rect(r.xMax - ellipsisStyle.pixelSize.x, r.y, ellipsisStyle.pixelSize.x, r.height);
                ellipsisStyle.style.Draw(rEllipsis, ellipsisStyle.ellipsisString, false, false, false, false);
            }
            else
            {
                textStyle.Draw(r, text, false, false, false, false);
            }
        }
    }


    class TestTextSpreadsheet : TextSpreadsheet
    {
        public TestTextSpreadsheet()
            : base(new SplitterStateEx(new[] { 100, 50, 50, 50 }), null)
        {
        }

        protected override long GetFirstRow()
        {
            return 0;
        }

        protected override long GetNextVisibleRow(long row)
        {
            if (row >= 1000)
            {
                return -1;
            }
            return row + 1;
        }

        protected override long GetPreviousVisibleRow(long row)
        {
            return row - 1;
        }

        protected override DirtyRowRange SetCellExpanded(long row, long col, bool expanded)
        {
            return DirtyRowRange.NonDirty;
        }

        protected override bool GetCellExpanded(long row, long col)
        {
            return false;
        }

        protected override void DrawCell(long row, long col, Rect r, long index, bool selected, ref GUIPipelineState pipe)
        {
            if (Event.current.type == EventType.Repaint)
            {
                string t = "R" + row + "C" + col + "Y" + r.y;

                DrawTextEllipsis(t, r, Styles.General.NumberLabel, EllipsisStyleMetricData, selected);
            }
        }

        protected override void DrawHeader(long col, Rect r, ref GUIPipelineState pipe)
        {
            Styles.General.Header.Draw(r, "Header" + col, false, false, false, false);
        }
    }
}
