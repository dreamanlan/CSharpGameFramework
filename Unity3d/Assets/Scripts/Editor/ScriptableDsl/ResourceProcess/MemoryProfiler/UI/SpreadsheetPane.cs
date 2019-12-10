using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal class SpreadsheetPane : ViewPane
    {
        public string TableDisplayName
        {
            get
            {
                return m_Spreadsheet.SourceTable.GetDisplayName();
            }
        }

        UI.DatabaseSpreadsheet m_Spreadsheet;
        Database.TableReference m_CurrentTableLink;

        public int CurrentTableIndex { get; private set; }

        bool m_NeedRefresh = false;

        internal class History : HistoryEvent
        {
            readonly Database.TableReference m_Table;
            readonly DatabaseSpreadsheet.State m_SpreadsheetState;

            public History(SpreadsheetPane spreadsheetPane, UIState.BaseMode mode, Database.CellLink cell)
            {
                Mode = mode;
                m_Table = spreadsheetPane.m_CurrentTableLink;
                m_SpreadsheetState = spreadsheetPane.m_Spreadsheet.CurrentState;
            }

            public void Restore(SpreadsheetPane pane)
            {
                var table = pane.m_UIState.CurrentMode.GetSchema().GetTableByReference(m_Table);
                if (table == null)
                {
                    Debug.LogError("No table named '" + m_Table.Name + "' found.");
                    return;
                }
                pane.m_CurrentTableLink = m_Table;
                pane.CurrentTableIndex = pane.m_UIState.CurrentMode.GetTableIndex(table);
                pane.m_Spreadsheet = new UI.DatabaseSpreadsheet(pane.m_UIState.FormattingOptions, table, pane, m_SpreadsheetState);
                pane.m_Spreadsheet.onClickLink += pane.OnSpreadsheetClick;
                pane.m_EventListener.OnRepaint();
            }

            public override string ToString()
            {
                string s = Mode.GetSchema().GetDisplayName() + seperator + m_Table.Name;
                if (m_Table.Param != null)
                {
                    s += "(";
                    string sp = "";
                    foreach (var p in m_Table.Param.AllParameters)
                    {
                        if (sp != "")
                        {
                            sp += ", ";
                        }
                        sp += p.Key;
                        sp += "=";
                        sp += p.Value.GetValueString(0, Database.DefaultDataFormatter.Instance);
                    }
                    s += sp + ")";
                }
                return s;
            }
        }

        public SpreadsheetPane(UIState s, IViewPaneEventListener l)
            : base(s, l)
        {
        }

        protected void CloseCurrentTable()
        {
            if (m_Spreadsheet != null)
            {
                if (m_Spreadsheet.SourceTable is Database.ExpandTable)
                {
                    (m_Spreadsheet.SourceTable as Database.ExpandTable).ResetAllGroup();
                }
            }
        }

        public void OpenLinkRequest(Database.LinkRequestTable link)
        {
            var tableRef = new Database.TableReference(link.LinkToOpen.TableName, link.Parameters);
            var table = m_UIState.CurrentMode.GetSchema().GetTableByReference(tableRef);
            if (table == null)
            {
                UnityEngine.Debug.LogError("No table named '" + link.LinkToOpen.TableName + "' found.");
                return;
            }
            OpenLinkRequest(link, tableRef, table);
        }

        public bool OpenLinkRequest(Database.LinkRequestTable link, Database.TableReference tableLink, Database.Table table)
        {
            if (link.LinkToOpen.RowWhere != null && link.LinkToOpen.RowWhere.Count > 0)
            {
                Database.Table filteredTable = table;
                if (table.GetMetaData().defaultFilter != null)
                {
                    filteredTable = table.GetMetaData().defaultFilter.CreateFilter(table);
                }
                var whereUnion = new Database.View.WhereUnion(link.LinkToOpen.RowWhere, null, null, null, null, m_UIState.CurrentMode.GetSchema(), filteredTable, link.SourceView == null ? null : link.SourceView.ExpressionParsingContext);
                long rowToSelect = whereUnion.GetIndexFirstMatch(link.SourceRow);
                if (rowToSelect < 0)
                {
                    Debug.LogWarning("Could not find entry in target table '" + link.LinkToOpen.TableName + "'");
                    return false;
                }

                OpenTable(tableLink, table, new Database.CellPosition(rowToSelect, 0));
            }
            else
            {
                OpenTable(tableLink, table, new Database.CellPosition(0, 0));
            }
            return true;
        }

        void OnSpreadsheetClick(UI.DatabaseSpreadsheet sheet, Database.LinkRequest link, Database.CellPosition pos)
        {
            var hEvent = new History(this, m_UIState.CurrentMode, sheet.DisplayTable.GetLinkTo(pos));
            m_UIState.history.AddEvent(hEvent);
            m_EventListener.OnOpenLink(link);
        }

        public void OpenTable(Database.TableReference tableRef, Database.Table table)
        {
            CloseCurrentTable();
            m_CurrentTableLink = tableRef;
            CurrentTableIndex = m_UIState.CurrentMode.GetTableIndex(table);
            m_Spreadsheet = new UI.DatabaseSpreadsheet(m_UIState.FormattingOptions, table, this);
            m_Spreadsheet.onClickLink += OnSpreadsheetClick;
            m_EventListener.OnRepaint();
        }

        public void OpenTable(Database.TableReference tableRef, Database.Table table, Database.CellPosition pos)
        {
            CloseCurrentTable();
            m_CurrentTableLink = tableRef;
            CurrentTableIndex = m_UIState.CurrentMode.GetTableIndex(table);
            m_Spreadsheet = new UI.DatabaseSpreadsheet(m_UIState.FormattingOptions, table, this);
            m_Spreadsheet.onClickLink += OnSpreadsheetClick;
            m_Spreadsheet.Goto(pos);
            m_EventListener.OnRepaint();
        }

        public void OpenHistoryEvent(History e)
        {
            if (e == null) return;
            e.Restore(this);
        }

        public override UI.HistoryEvent GetCurrentHistoryEvent()
        {
            if (m_Spreadsheet != null && m_CurrentTableLink != null)
            {
                var c = m_Spreadsheet.GetLinkToCurrentSelection();
                if (c == null)
                {
                    c = m_Spreadsheet.GetLinkToFirstVisible();
                }
                if (c != null)
                {
                    var hEvent = new History(this, m_UIState.CurrentMode, c);
                    return hEvent;
                }
            }
            return null;
        }

        private void OnGUI_OptionBar()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var ff = GUILayout.Toggle(m_UIState.FormattingOptions.ObjectDataFormatter.flattenFields, "Flatten Fields");
            if (m_UIState.FormattingOptions.ObjectDataFormatter.flattenFields != ff)
            {
                m_UIState.FormattingOptions.ObjectDataFormatter.flattenFields = ff;
                if (m_Spreadsheet != null)
                {
                    m_NeedRefresh = true;
                }
            }
            var fsf = GUILayout.Toggle(m_UIState.FormattingOptions.ObjectDataFormatter.flattenStaticFields, "Flatten Static Fields");
            if (m_UIState.FormattingOptions.ObjectDataFormatter.flattenStaticFields != fsf)
            {
                m_UIState.FormattingOptions.ObjectDataFormatter.flattenStaticFields = fsf;
                if (m_Spreadsheet != null)
                {
                    m_NeedRefresh = true;
                }
            }
            var spn = GUILayout.Toggle(m_UIState.FormattingOptions.ObjectDataFormatter.ShowPrettyNames, "Pretty Name");
            if (m_UIState.FormattingOptions.ObjectDataFormatter.ShowPrettyNames != spn)
            {
                m_UIState.FormattingOptions.ObjectDataFormatter.ShowPrettyNames = spn;
                m_EventListener.OnRepaint();
            }
            EditorGUILayout.EndHorizontal();
        }

        public override void OnGUI(Rect r)
        {
            if (Event.current.type == EventType.Layout)
            {
                if (m_NeedRefresh)
                {
                    m_Spreadsheet.UpdateTable();
                    m_NeedRefresh = false;
                }
            }
            m_UIState.FormattingOptions.ObjectDataFormatter.forceLinkAllObject = false;
            if (m_Spreadsheet != null)
            {
                GUILayout.BeginArea(r);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Filters:");

                m_Spreadsheet.OnGui_Filters();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(2);
                m_Spreadsheet.OnGUI(r.width);
                GUILayout.Space(2);
                EditorGUILayout.EndHorizontal();

                OnGUI_OptionBar();
                GUILayout.Space(2);
                EditorGUILayout.EndVertical();
                GUILayout.EndArea();
                if (m_NeedRefresh)
                {
                    m_EventListener.OnRepaint();
                }
            }
        }

        public override void OnClose()
        {
            MemoryProfilerAnalytics.SendPendingFilterChanges();
            CloseCurrentTable();
            m_Spreadsheet = null;
        }
    }
}
