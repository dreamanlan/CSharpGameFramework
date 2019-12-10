using System;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    //Used with select statement that are Many-To-Many. Shows only the first match for each row.
    internal class ViewColumnFirstMatch<DataT> : Database.ColumnTyped<DataT>, ViewColumn.IViewColumn where DataT : IComparable
    {
        // Used as a cache when computing row on demand only
        private System.Collections.Generic.SortedDictionary<long, long> m_index = new System.Collections.Generic.SortedDictionary<long, long>();

        // Used when all row are computed
        private long[] m_rowIndex;

        private ViewColumn vc;
        public Database.ColumnTyped<DataT> column;

        public ViewColumnFirstMatch()
        {
            type = typeof(DataT);
        }

        void ViewColumn.IViewColumn.SetColumn(ViewColumn vc, Database.Column col)
        {
            this.vc = vc;
            column = (Database.ColumnTyped<DataT>)col;
        }

        void ViewColumn.IViewColumn.SetConstValue(string value)
        {
            var metaColumn = vc.viewTable.GetMetaColumnByColumn(this);
            string extraInfo = "";
            if (metaColumn != null)
            {
                extraInfo += " column '" + metaColumn.Name + "'";
            }
            Debug.LogError("Cannot set a const value on a first match view column. Table '" + vc.viewTable.GetName() + "'" + extraInfo);
        }

        Database.Column ViewColumn.IViewColumn.GetColumn()
        {
            return this;
        }

        public override LinkRequest GetRowLink(long row)
        {
            return LinkRequestTable.MakeLinkRequest(vc.m_MetaLink, vc.viewTable, this, row, vc.ParsingContext);
        }

        public override long GetRowCount()
        {
            return vc.viewTable.GetRowCount();
        }

        public override bool ComputeRowCount()
        {
            return vc.viewTable.ComputeRowCount();
        }

        public override bool Update()
        {
            if (m_rowIndex != null) return false;
            vc.viewTable.ComputeRowCount();
            long rowCount = vc.viewTable.GetRowCount();
            if (rowCount >= 0)
            {
                m_index = null;
                m_rowIndex = vc.select.GetIndexFirstMatches(0, rowCount);
            }
            return true;
        }

        private long GetRow(long row)
        {
            if (m_rowIndex == null)
            {
                long r;
                if (m_index.TryGetValue(row, out r))
                {
                    return r;
                }
                else
                {
                    r = vc.select.GetIndexFirstMatch(row);
                    m_index.Add(row, r);
                    return r;
                }
            }
            else
            {
                return m_rowIndex[row];
            }
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            if (vc.m_IsDisplayMergedOnly) return "";
            long r = GetRow(row);
            if (r >= 0)
            {
                return column.GetRowValueString(r, formatter);
            }
            else
            {
                return "N/A";
            }
        }

        public override DataT GetRowValue(long row)
        {
            long r = GetRow(row);
            if (r >= 0)
            {
                return column.GetRowValue(r);
            }
            else
            {
                return default(DataT);
            }
        }

        public override void SetRowValue(long row, DataT value)
        {
            long r = GetRow(row);
            if (r >= 0)
            {
                column.SetRowValue(r, value);
            }
        }
    }
}
