using System;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    internal class ViewColumnNode
    {
        public ViewTable viewTable;
        public MetaColumn metaColumn;
        public Operation.ExpressionParsingContext ParsingContext;
        public ViewColumnNode(ViewTable viewTable, MetaColumn metaColumn, Operation.ExpressionParsingContext parsingContext)
        {
            this.viewTable = viewTable;
            this.metaColumn = metaColumn;
            ParsingContext = parsingContext;
        }

        public interface IViewColumnNode
        {
            void SetColumn(ViewColumnNode vc);
            Database.Column GetColumn();
            void SetEntry(long row, Operation.Expression exp, TableLink Link);
        }
    }

    // List of expression set by a view when the data type is "node"
    internal class ViewColumnNodeTyped<DataT> : Database.ColumnTyped<DataT>, ViewColumnNode.IViewColumnNode where DataT : IComparable
    {
        public Operation.TypedExpression<DataT>[] entries;
        public TableLink[] linkEntries;
        private ViewColumnNode m_ViewColumn;

        Operation.ValueStringArrayCache<DataT> m_Cache;

        public ViewColumnNodeTyped()
        {
            type = typeof(DataT);
        }

        void ViewColumnNode.IViewColumnNode.SetColumn(ViewColumnNode vc)
        {
            if (m_ViewColumn != null)
            {
                throw new InvalidOperationException("Cannot call 'ViewColumn.IViewColumn.SetColumn' once already set");
            }
            if (vc.viewTable.node.data.type != ViewTable.Builder.Node.Data.DataType.Node)
            {
                throw new Exception("Cannot set a ViewColumnNodeTyped column on a non-Node data type view table");
            }
            m_ViewColumn = vc;
            entries = new Operation.TypedExpression<DataT>[vc.viewTable.GetNodeChildCount()];
            m_Cache.InitDirect(entries.Length);
            linkEntries = new TableLink[vc.viewTable.GetNodeChildCount()];
        }

        Database.Column ViewColumnNode.IViewColumnNode.GetColumn()
        {
            return this;
        }

        void ViewColumnNode.IViewColumnNode.SetEntry(long row, Operation.Expression exp, TableLink link)
        {
            m_Cache.SetEntryDirty((int)row);
            entries[(int)row] = exp as Operation.TypedExpression<DataT>;
            linkEntries[(int)row] = link;
        }

        public override long GetRowCount()
        {
            return entries.Length;
        }

        public override LinkRequest GetRowLink(long row)
        {
            return LinkRequestTable.MakeLinkRequest(linkEntries[(int)row], m_ViewColumn.viewTable, this, row, m_ViewColumn.ParsingContext);
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            return m_Cache.GetValueStringFromExpression((int)row, entries[row], 0, formatter);
        }

        public override DataT GetRowValue(long row)
        {
            return m_Cache.GetValueFromExpression((int)row, entries[row], 0);
        }

        public override void SetRowValue(long row, DataT value)
        {
            throw new InvalidOperationException("Cannot call 'SetRowValue' on 'ViewColumnNodeTyped'");
        }
    }
}
