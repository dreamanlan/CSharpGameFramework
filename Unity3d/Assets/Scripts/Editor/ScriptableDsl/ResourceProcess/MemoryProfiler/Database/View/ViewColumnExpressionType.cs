using System;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    // Column that yield the value of an expression
    internal class ViewColumnExpressionType<DataT> : Database.ColumnTyped<DataT>, ViewColumn.IViewColumn where DataT : IComparable
    {
        ViewColumn m_SourceViewColumn;
        Operation.TypedExpression<DataT> m_SourceExpression;

        Operation.ValueStringArrayCache<DataT> m_Cache;

        public ViewColumnExpressionType(Operation.TypedExpression<DataT> expression)
        {
            type = typeof(DataT);
            m_SourceExpression = expression;
        }

        bool HasFixedRow { get { return m_SourceViewColumn.ParsingContext != null && m_SourceViewColumn.ParsingContext.fixedRow >= 0; } }
        int FixedRow { get { return (int)m_SourceViewColumn.ParsingContext.fixedRow; } }

        bool UpdateCache()
        {
            if (!m_Cache.Initialized)
            {
                m_Cache.InitForExpression(m_SourceExpression);
                return true;
            }
            return false;
        }

        int EntryIndexToCacheIndex(long index)
        {
            return HasFixedRow ? 0 : (int)index;
        }

        int EntryIndexToSourceRow(long index)
        {
            return HasFixedRow ? FixedRow : (int)index;
        }

        void ViewColumn.IViewColumn.SetColumn(ViewColumn vc, Database.Column col)
        {
            m_SourceViewColumn = vc;
            UpdateCache();
        }

        void ViewColumn.IViewColumn.SetConstValue(string value)
        {
            throw new InvalidOperationException("Cannot call 'ViewColumn.IViewColumn.SetConstValue' on 'ViewColumnExpressionType'");
        }

        Database.Column ViewColumn.IViewColumn.GetColumn()
        {
            return this;
        }

        public override long GetRowCount()
        {
            return m_SourceExpression.RowCount();
        }

        public override bool ComputeRowCount()
        {
            return m_SourceViewColumn.viewTable.ComputeRowCount();
        }

        public override LinkRequest GetRowLink(long row)
        {
            if (m_SourceViewColumn.ParsingContext != null && m_SourceViewColumn.ParsingContext.fixedRow >= 0)
            {
                return LinkRequestTable.MakeLinkRequest(m_SourceViewColumn.m_MetaLink, m_SourceViewColumn.viewTable, this, m_SourceViewColumn.ParsingContext.fixedRow, m_SourceViewColumn.ParsingContext);
            }
            else
            {
                return LinkRequestTable.MakeLinkRequest(m_SourceViewColumn.m_MetaLink, m_SourceViewColumn.viewTable, this, row, m_SourceViewColumn.ParsingContext);
            }
        }

        public override string GetRowValueString(long row, IDataFormatter formatter)
        {
            if (m_SourceViewColumn.m_IsDisplayMergedOnly)
            {
                return "";
            }
            UpdateCache();
            return m_Cache.GetValueStringFromExpression((int)row, m_SourceExpression, row, formatter);
        }

        public override DataT GetRowValue(long row)
        {
            UpdateCache();
            return m_Cache.GetValueFromExpression((int)row, m_SourceExpression, row);
        }

        public override void SetRowValue(long row, DataT value)
        {
            throw new InvalidOperationException("Cannot call 'SetRowValue' on 'ViewColumnExpressionType'");
        }
    }
}
