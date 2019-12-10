using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    // Used when displaying a constant value for all rows
    internal class ViewColumRowIndex : ColumnTyped<long>, ViewColumn.IViewColumn
    {
        private ViewColumn vc;
        public ViewColumRowIndex(ViewColumn vc)
        {
            this.vc = vc;
            type = typeof(long);
        }

        void ViewColumn.IViewColumn.SetColumn(ViewColumn vc, Column col)
        {
            this.vc = vc;
        }

        void ViewColumn.IViewColumn.SetConstValue(string value)
        {
            var metaColumn = vc.viewTable.GetMetaColumnByColumn(this);
            string extraInfo = "";
            if (metaColumn != null)
            {
                extraInfo += " column '" + metaColumn.Name + "'";
            }
            Debug.LogError("Cannot set a const value on a RowIndex view column. Table '" + vc.viewTable.GetName() + "'" + extraInfo);
        }

        Database.Column ViewColumn.IViewColumn.GetColumn()
        {
            return this;
        }

        public override long GetRowCount()
        {
            return vc.viewTable.GetRowCount();
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(row);
        }

        public override long GetRowValue(long row)
        {
            return row;
        }

        public override void SetRowValue(long row, long value)
        {
        }

        public override LinkRequest GetRowLink(long row)
        {
            return LinkRequestTable.MakeLinkRequest(vc.m_MetaLink, vc.viewTable, this, row, vc.ParsingContext);
        }
    }
}
