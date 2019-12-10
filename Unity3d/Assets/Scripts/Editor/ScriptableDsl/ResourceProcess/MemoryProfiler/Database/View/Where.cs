using System.Collections.Generic;
using System.Xml;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    internal class Where
    {
        public Operation.ColumnComparison m_Comparison;
        public Operation.ColumnComparison Comparison { get { return m_Comparison; } }

        public long GetFirstMatchIndex(long row)
        {
            return m_Comparison.GetFirstMatchIndex(row);
        }

        public long[] GetMatchIndex(long row)
        {
            return m_Comparison.GetMatchIndex(row);
        }

        public long[] GetMatchIndex(long row, ArrayRange range)
        {
            return m_Comparison.GetMatchIndex(row, range);
        }

        public class Builder
        {
            Operation.MetaColumnComparison comparison;

            public Builder()
            {
            }

            public Builder(string column, Operation.Operator op, Operation.Expression.MetaExpression value)
            {
                comparison = new Operation.MetaColumnComparison(column, op, value);
            }

            private string FormatErrorContextInfo(ViewSchema vs, ViewTable vTable, Select select)
            {
                string str = "";
                if (vs != null && vTable != null) str += "Error while building view '" + vs.name + "' table '" + vTable.GetName() + "'";
                if (select != null) str += " select '" + select.name + "'";
                if (str != "") str += ". ";
                return str;
            }

            public Where Build(ViewSchema vs, ViewTable vTable, SelectSet selectSet, Select select, Schema baseSchema, Table table, Operation.ExpressionParsingContext expressionParsingContext)
            {
                Where w = new Where();
                var option = new Operation.Expression.ParseIdentifierOption(vs, table, true, false, null, expressionParsingContext);
                option.formatError = (msg, y) =>
                {
                    return FormatErrorContextInfo(vs, vTable, select) + msg;
                };
                option.BypassSelectSetCondition = selectSet;
                w.m_Comparison = comparison.Build(option);
                return w;
            }

            public static Builder LoadFromXML(XmlElement root)
            {
                Builder b = new Builder();
                b.comparison = Operation.MetaColumnComparison.LoadFromXML(root);
                return b;
            }
        }
    }
    internal class WhereUnion
    {
        public List<Where> where = new List<Where>();
        public WhereUnion()
        {
        }

        public WhereUnion(List<Where.Builder> builders, ViewSchema vs, ViewTable vTable, SelectSet selectSet, Select select, Schema baseSchema, Table table, Operation.ExpressionParsingContext expressionParsingContext)
        {
            foreach (var b in builders)
            {
                Add(b.Build(vs, vTable, selectSet, select, baseSchema, table, expressionParsingContext));
            }
        }

        public int Count
        {
            get
            {
                return where.Count;
            }
        }

        public void Add(Where w)
        {
            if (w != null) where.Add(w);
        }

        //get the first matching index for each row between [rowFirst, rowLast) of expressions used in where statements
        public long[] GetIndexFirstMatches(long rowFirst, long rowLast)
        {
            long count = rowLast - rowFirst;
            long[] r = new long[count];
            for (long i = 0; i < count; ++i)
            {
                r[i] = GetIndexFirstMatch(rowFirst + i);
            }
            return r;
        }

        //get the first matching index for a specific row of expressions used in where statements
        public long GetIndexFirstMatch(long row)
        {
            //No where statements, seting index to null will bypass directly to the underlying talbe data
            if (where == null || where.Count == 0)
            {
                return row;
            }

            //only 1 where statement, use its result directly
            if (where.Count == 1)
            {
                return where[0].GetFirstMatchIndex(row);
            }

            // many where statements
            long[] index = null;
            foreach (var w in where)
            {
                if (index != null)
                {
                    index = w.GetMatchIndex(row, ArrayRange.IndexArray(index));
                }
                else
                {
                    index = w.GetMatchIndex(row);
                }
                if (index.Length == 0)
                {
                    break;
                }
            }
            if (index == null || index.Length == 0)
            {
                return -1;
            }
            else
            {
                return index[0];
            }
        }

        /// <summary>
        /// get all matching indices for a specific row of expressions used in where statements
        /// </summary>
        public long[] GetMatchingIndices(long row)
        {
            //No where statements, setting index to null will bypass directly to the underlying table data
            if (where == null || where.Count == 0)
            {
                long[] r = new long[1];
                r[0] = row;
                return r;
            }

            //only 1 where statement, use its result directly
            if (where.Count == 1)
            {
                return where[0].GetMatchIndex(row);
            }

            // many where statements
            long[] index = null;
            foreach (var w in where)
            {
                if (index != null)
                {
                    index = w.GetMatchIndex(row, ArrayRange.IndexArray(index));
                }
                else
                {
                    index = w.GetMatchIndex(row);
                }
                if (index.Length == 0)
                {
                    break;
                }
            }
            return index;
        }
    }
}
