using System.Xml;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    // Defines what data to select and where it comes from
    internal class Select
    {
        public string name;
        public Database.Table sourceTable;
        public WhereUnion where;

        // Will yield a result of no more than this amount of rows. -1 is unlimited
        public int MaxRow = -1;

        //if the select statement has any where statements with multiple row conditions
        public bool isManyToMany = false;

        public bool HasWhereCondition()
        {
            return where != null && where.Count > 0;
        }

        // useful when isManyToMany == false as it does not require a row parameter
        // if isManyToMany == true, return the result only for matching where expression row 0
        public long[] GetMatchingIndices()
        {
            return GetMatchingIndices(0);
        }

        // useful when isManyToMany = false as it does not need require a row parameter
        public bool ComputeRowCount()
        {
            if (!isManyToMany && cacheMatchingIndices == null)
            {
                GetMatchingIndices();
                return true;
            }
            return false;
        }

        // Cache for querying matching indices for a specific source row
        private long[] cacheMatchingIndices;
        private long cacheMatchingIndicesRow = -1;

        public long[] GetMatchingIndices(long row)
        {
            if (cacheMatchingIndicesRow != row)
            {
                if (HasWhereCondition())
                {
                    cacheMatchingIndices = where.GetMatchingIndices(row);
                    cacheMatchingIndicesRow = row;
                    if (MaxRow >= 0)
                    {
                        System.Array.Resize(ref cacheMatchingIndices, MaxRow);
                    }
                }
            }
            return cacheMatchingIndices;
        }

        // Get row count if known without heavy computation
        public long GetRowCount()
        {
            if (isManyToMany)
            {
                // requires to computes all results for all rows. too heavy
                return -1;
            }
            else
            {
                if (cacheMatchingIndices != null)
                {
                    // we've computed the result already
                    return cacheMatchingIndices.Length;
                }
                else if (!HasWhereCondition())
                {
                    // we are selecting all rows from source table
                    return sourceTable.GetRowCount();
                }
            }

            return -1;
        }

        //get the first matching index for each row between [rowFirst, rowLast) of expressions used in where statements
        public void GetIndexFirstMatches(bool[] aIsComputed, long[] aIndex, long rowFirst, long rowLast)
        {
            if (where == null)
            {
                long[] result = new long[rowLast - rowFirst];
                for (int i = 0; i != result.Length; ++i)
                {
                    aIsComputed[i] = true;
                    aIndex[i] = i;
                }
            }
            else
            {
                for (long i = rowFirst; i < rowLast; ++i)
                {
                    if (!aIsComputed[i])
                    {
                        aIsComputed[i] = true;
                        aIndex[i] = where.GetIndexFirstMatch(i);
                    }
                }
            }
        }

        //get the first matching index for each row between [rowFirst, rowLast) of expressions used in where statements
        public long[] GetIndexFirstMatches(long rowFirst, long rowLast)
        {
            if (where == null)
            {
                long[] result = new long[rowLast - rowFirst];
                for (int i = 0; i != result.Length; ++i)
                {
                    result[i] = i;
                }
                return result;
            }
            return where.GetIndexFirstMatches(rowFirst, rowLast);
        }

        //get the first matching index for a specific row of expressions used in where statements
        public long GetIndexFirstMatch(long row)
        {
            if (where == null) return row;
            return where.GetIndexFirstMatch(row);
        }

        public long GetIndexFirstMatchRowCount()
        {
            if (where == null) return 0;
            long rowMin = long.MaxValue;
            foreach (var w in where.where)
            {
                long rowCount = w.Comparison.value.RowCount();
                if (rowCount < rowMin)
                {
                    rowMin = rowCount;
                }
            }
            if (rowMin == long.MaxValue)
            {
                return 0;
            }
            return rowMin;
        }

        public class Builder
        {
            public string name;
            public string sourceTableName;
            public int MaxRow = -1;
            public Where.Builder AddWhere(string column, Operation.Operator op, Operation.Expression.MetaExpression value)
            {
                Where.Builder w = new Where.Builder(column, op, value);
                where.Add(w);
                return w;
            }

            protected System.Collections.Generic.List<Where.Builder> where = new System.Collections.Generic.List<Where.Builder>();
            public Select Create(ViewSchema vs, Database.Schema baseSchema, Table table)
            {
                Select sel = new Select();
                sel.name = name;
                if (baseSchema != null) sel.sourceTable = baseSchema.GetTableByName(sourceTableName);
                sel.MaxRow = MaxRow;
                if (sel.sourceTable == null)
                {
                    Debug.LogError("No table named '" + (sourceTableName == null ? "null" : sourceTableName) + "'");
                    return null;
                }

                return sel;
            }

            public void Build(ViewSchema vs, ViewTable vTable, SelectSet selectSet, Select sel, Database.Schema baseSchema, Operation.ExpressionParsingContext expressionParsingContext)
            {
                if (where.Count > 0)
                {
                    sel.where = new WhereUnion();
                    foreach (var w in where)
                    {
                        var w2 = w.Build(vs, vTable, selectSet, sel, baseSchema, sel.sourceTable, expressionParsingContext);
                        if (w2.Comparison.IsManyToMany())
                        {
                            sel.isManyToMany = true;
                        }
                        sel.where.Add(w2);
                    }
                }
            }

            public static Builder LoadFromXML(XmlElement root)
            {
                Builder b = new Builder();
                b.name = root.GetAttribute("name");
                b.sourceTableName = root.GetAttribute("table");
                string strMaxRow = root.GetAttribute("maxRow");
                if (string.IsNullOrEmpty(strMaxRow) || !int.TryParse(strMaxRow, out b.MaxRow))
                {
                    b.MaxRow = -1;
                }
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement e = (XmlElement)node;
                        switch (e.Name)
                        {
                            case "Where":
                                {
                                    var w = Where.Builder.LoadFromXML(e);
                                    if (w != null)
                                    {
                                        b.where.Add(w);
                                    }
                                    break;
                                }
                            default:
                                //DebugUtility.LogInvalidXmlChild(root, e);
                                break;
                        }
                    }
                }
                return b;
            }
        }
    }


    // A collection of select statements
    // the first Select in the list is the "Main" select.
    // TryGetMainSelect, GetMainRows and GetMainRowCount will return data from the main select regardless of whether there is a condition set or not.
    //
    internal class SelectSet
    {
        public List<Select> select = new List<Select>();
        public Dictionary<string, Select> selectByName = new Dictionary<string, Select>();

        // when true, m_MainIndices needs to be computed
        bool m_MainIndicesDirty = true;

        // Array of row index into the main Select's source table that are selected
        // if null, all rows from the main Select's source table are selected
        long[] m_MainIndices;

        // Applies a condition on the main select result rows. Will add row indices that pass the condition into m_MainConditionalIndices.
        // When null, all rows are valid.
        public Operation.ExpComparison Condition;

        // when true, m_MainConditionalIndices needs to be computed
        bool m_MainConditionalIndicesDirty = true;

        // array of index into m_MainIndices that passes the condition.
        // if null, all indices in m_MainIndices passes the condition.
        long[] m_MainConditionalIndices;

        // Retrieve the main Select (the first Select added to the SelectSet)
        public bool TryGetMainSelect(out Select select)
        {
            if (this.select.Count > 0)
            {
                select = this.select[0];
                return true;
            }
            select = null;
            return false;
        }

        void UpdateMainIndices()
        {
            if (m_MainIndicesDirty)
            {
                m_MainIndicesDirty = false;
                Select mainSelect;
                if (TryGetMainSelect(out mainSelect))
                {
                    m_MainIndices = mainSelect.GetMatchingIndices();
                }
            }
        }

        // Returns the index of the resulting rows from the main select.
        // Will return null if all rows from the main Select's source table are selected.
        public long[] GetMainRows()
        {
            UpdateMainIndices();
            return m_MainIndices;
        }

        public long GetMainRowCount()
        {
            UpdateMainIndices();
            if (m_MainIndices != null) return m_MainIndices.LongLength;

            // when m_MainIndices is null and not dirty, all rows from main Select's source table are selected.
            // therefore, we return the source table rows count.
            Select mainSelect;
            if (TryGetMainSelect(out mainSelect))
            {
                long sourceTableRowCount = mainSelect.sourceTable.GetRowCount();
                if (sourceTableRowCount < 0)
                {
                    // Force computing row count
                    mainSelect.sourceTable.ComputeRowCount();
                    return mainSelect.sourceTable.GetRowCount();
                }
                return sourceTableRowCount;
            }

            // No main select set.
            throw new System.InvalidOperationException("Cannot call 'SelectSet.GetMainRowCount' when it has no main select. Call 'SelectSet.Add' at least once before calling 'SelectSet.GetMainRowCount'");
        }

        void UpdateMainConditionalIndices()
        {
            if (m_MainConditionalIndicesDirty)
            {
                m_MainConditionalIndicesDirty = false;
                if (Condition != null)
                {
                    long[] rows = GetMainRows();
                    if (rows == null)
                    {
                        m_MainConditionalIndices = null;
                        return;
                    }
                    var conditionalIndices = new List<long>();
                    for (int i = 0; i != rows.Length; ++i)
                    {
                        if (Condition.GetValue(i))
                        {
                            conditionalIndices.Add(i);
                        }
                    }
                    m_MainConditionalIndices = conditionalIndices.ToArray();
                }
                else
                {
                    m_MainConditionalIndices = null;
                }
            }
        }

        // Returns an array of indices into GetMainRows() of rows that passes the SelectSet condition.
        // If it returns null, all entries in GetMainRowIndices() pass the condition
        public long[] GetConditionalRowIndices()
        {
            UpdateMainConditionalIndices();
            return m_MainConditionalIndices;
        }

        public bool IsManyToMany()
        {
            if (select.Count > 0)
            {
                return select[0].isManyToMany;
            }
            return false;
        }

        public void Add(Select newSelect)
        {
            if (select.Count == 0)
            {
                //setting main select.
                m_MainIndicesDirty = true;
            }
            m_MainConditionalIndicesDirty = true;
            select.Add(newSelect);
            selectByName.Add(newSelect.name, newSelect);
        }

        public bool TryGetSelect(string name, out Select select)
        {
            return selectByName.TryGetValue(name, out select);
        }

        public class Builder
        {
            public System.Collections.Generic.List<Select.Builder> select = new System.Collections.Generic.List<Select.Builder>();
            public Operation.MetaExpComparison Condition;

            public SelectSet Build(ViewTable viewTable, ViewSchema viewSchema, Database.Schema baseSchema)
            {
                if (select.Count == 0) return null;

                SelectSet selectSet = new SelectSet();

                // Create select statements (first pass)
                foreach (var iSelect in select)
                {
                    Select s = iSelect.Create(viewTable.ViewSchema, baseSchema, viewTable);
                    if (s != null)
                    {
                        selectSet.Add(s);
                    }
                    else
                    {
                        Debug.LogError("Could not create Select named '" + iSelect.name + "'");
                    }
                }

                // add current set to the expression parsing hierarchy
                var expressionParsingContext = new Operation.ExpressionParsingContext(viewTable.ExpressionParsingContext, selectSet);

                // Build select statements (second pass)
                var eSelBuilder = select.GetEnumerator();
                var eSelList = selectSet.select.GetEnumerator();
                while (eSelBuilder.MoveNext())
                {
                    eSelList.MoveNext();
                    eSelBuilder.Current.Build(viewSchema, viewTable, selectSet, eSelList.Current, baseSchema, expressionParsingContext);
                }

                if (Condition != null)
                {
                    Operation.Expression.ParseIdentifierOption parseOpt = new Operation.Expression.ParseIdentifierOption(viewSchema, viewTable, true, false, null, expressionParsingContext);
                    parseOpt.BypassSelectSetCondition = selectSet;
                    selectSet.Condition = Condition.Build(parseOpt);
                }

                return selectSet;
            }

            public static Builder LoadFromXML(XmlElement root)
            {
                Builder b = new Builder();
                foreach (XmlNode xNode in root.ChildNodes)
                {
                    if (xNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement e = (XmlElement)xNode;
                        switch (e.Name)
                        {
                            case "Select":
                                {
                                    var s = Select.Builder.LoadFromXML(e);
                                    if (s != null)
                                    {
                                        b.select.Add(s);
                                    }
                                    break;
                                }
                            case "Condition":
                                {
                                    b.Condition = Operation.MetaExpComparison.LoadFromXML(e);
                                    break;
                                }
                            default:
                                //DebugUtility.LogInvalidXmlChild(root, e);
                                break;
                        }
                    }
                }
                return b;
            }
        }
    }
}
