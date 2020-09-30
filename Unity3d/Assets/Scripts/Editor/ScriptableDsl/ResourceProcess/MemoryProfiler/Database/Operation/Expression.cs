using System;
using System.Xml;
using Unity.Profiling;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    /// <summary>
    /// Create a hierarchy of contexts used when parsing identifier in any expression
    /// </summary>
    internal class ExpressionParsingContext
    {
        public ExpressionParsingContext parent;
        public View.SelectSet selectSet;
        public ExpressionParsingContext(ExpressionParsingContext parent, View.SelectSet selectSet)
        {
            this.parent = parent;
            this.selectSet = selectSet;
        }

        public long fixedRow = -1;

        public bool TryGetSelect(string name, out ExpressionParsingContext context, out View.Select select)
        {
            if (!selectSet.TryGetSelect(name, out select))
            {
                if (parent != null) return parent.TryGetSelect(name, out context, out select);
                context = null;
                return false;
            }
            context = this;
            return true;
        }
    }

    /// <summary>
    /// Base class representing any expression that produce a single value or an array of values (using the row parameter)
    /// </summary>
    internal abstract class Expression
    {
        public Type type;
        public abstract string GetValueString(long row, IDataFormatter formatter);
        public abstract IComparable GetComparableValue(long row);
        public abstract bool HasMultipleRow();
        public abstract long RowCount();

        // Meta Expression are unresolved expression that must be parsed using ParseIdentifier to create an Expression
        internal class MetaExpression
        {
            public MetaExpression() {}
            public MetaExpression(string value, bool valueIsLiteral)
            {
                this.valueIsLiteral = valueIsLiteral;
                this.value = value;
            }

            public MetaExpression(string value, bool valueIsLiteral, Type type)
            {
                this.valueIsLiteral = valueIsLiteral;
                this.value = value;
                this.type = type;
            }

            // Can be a const value or an identifier. Identifier has the format "selectName.columnName"
            public string value;

            // force a fixed row when evaluating the value. Will collapse a Many-to-Many select to a Many-to-One when used on where clauses
            public long row = -1;

            // Value used when the value cannot be evaluated without errors
            public bool hasDefaultValue;
            public string valueDefault;

            // when false: value is parsed for identifiers
            // when true : value is used without parsing for identifiers. Will still interpret numbers as int or float
            public bool valueIsLiteral;

            // force the expression to be a specific type. If null, the type will be inferred from the value.
            public Type type;

            // Data breakpoint value will trigger a breakpoint if a debugger is attached. valueDataBreakpoint can be an identifier
            public bool hasDataBreakpointValue;
            public bool dataBreakpointValueIsLiteral;
            public string valueDataBreakpoint;

            public static MetaExpression LoadFromXML(XmlElement root)
            {
                MetaExpression metaExpression = new MetaExpression();
                metaExpression.value = root.GetAttribute("value");
                var literal = root.GetAttribute("literal");
                if (!String.IsNullOrEmpty(literal))
                {
                    metaExpression.valueIsLiteral = bool.Parse(literal);
                }
                var defaultValue = root.GetAttribute("default");
                if (!String.IsNullOrEmpty(defaultValue))
                {
                    metaExpression.valueDefault = defaultValue;
                    metaExpression.hasDefaultValue = true;
                }
                var breakOn = root.GetAttribute("breakOn");
                if (!String.IsNullOrEmpty(breakOn))
                {
                    metaExpression.valueDataBreakpoint = breakOn;
                    metaExpression.hasDataBreakpointValue = true;
                    var breakOnLiteral = root.GetAttribute("breakOnLiteral");
                    if (!String.IsNullOrEmpty(literal))
                    {
                        metaExpression.dataBreakpointValueIsLiteral = bool.Parse(breakOnLiteral);
                    }
                }
                string strRow = root.GetAttribute("row");
                if (!long.TryParse(strRow, out metaExpression.row))
                {
                    metaExpression.row = -1;
                }
                string strType = root.GetAttribute("type");
                if (!string.IsNullOrEmpty(strType))
                {
                    switch (strType)
                    {
                        case "bool": metaExpression.type = typeof(bool); break;
                        case "double": metaExpression.type = typeof(double); break;
                        case "float": metaExpression.type = typeof(float); break;
                        case "int": metaExpression.type = typeof(int); break;
                        case "long": metaExpression.type = typeof(long); break;
                        case "short": metaExpression.type = typeof(short); break;
                        case "uint": metaExpression.type = typeof(uint); break;
                        case "ulong": metaExpression.type = typeof(ulong); break;
                        case "ushort": metaExpression.type = typeof(ushort); break;
                        case "string": metaExpression.type = typeof(string); break;
                        case "DiffResult": metaExpression.type = typeof(DiffTable.DiffResult); break;
                        default:
                            metaExpression.type = null;
                            break;
                    }
                }
                else
                {
                    metaExpression.type = null;
                }
                return metaExpression;
            }
        }

        /// <summary>
        /// options used when parsing an identifier. provide information about context (ExpressionParsingContext) and other requirements
        /// </summary>
        internal class ParseIdentifierOption
        {
            public View.ViewSchema Schema;

            // the identifier can reference to this table either with the "tableName.columnName" format or the "columnName" format.
            public Table identifierColumn_table;

            // provide a way to add contextual information to parsing errors
            public Func<string, ParseIdentifierOption, string> formatError = (string s, ParseIdentifierOption opt) => { return s; };

            // will force the outputted expression to be of this type if the value in is a number
            public Type overrideValueType;

            //output a default expression even when an error happens
            public bool defaultOnError;

            // true: use the row parameter for the select source row. Useful for listing the first match for each source row. Only works if the select is many-to-many
            // false: use the row parameter for the matching result row. Useful for listing all match for a fixed source value
            public bool useFirstMatchSelect;

            //when set, it will not take into consideration this SelectSet's condition when referenced
            public View.SelectSet BypassSelectSetCondition;

            // context in which the identifier is parsed. will provide select sets in which to look for identifiers.
            public ExpressionParsingContext expressionParsingContext;

            /// <summary>
            /// Will not return an expression but only resolve the type of the expression.
            /// </summary>
            public bool OnlyResolveType;

            public ParseIdentifierOption(Database.View.ViewSchema schema, Table identifierColumn_table, bool useFirstMatchSelect, bool defaultOnError, Type overrideValueType, ExpressionParsingContext expressionParsingContext)
            {
                this.Schema = schema;
                this.identifierColumn_table = identifierColumn_table;
                this.useFirstMatchSelect = useFirstMatchSelect;
                this.defaultOnError = defaultOnError;
                this.overrideValueType = overrideValueType;
                this.expressionParsingContext = expressionParsingContext;
            }

            public ParseIdentifierOption(ExpressionParsingContext expressionParsingContext)
            {
                this.expressionParsingContext = expressionParsingContext;
            }
        }
        private static Expression ProcessDataBreakpointValue(Expression expression, MetaExpression value, ParseIdentifierOption opt)
        {
            if (value.hasDataBreakpointValue)
            {
                MetaExpression breakpointValue = new MetaExpression();
                breakpointValue.value = value.valueDataBreakpoint;
                breakpointValue.valueIsLiteral = value.dataBreakpointValueIsLiteral;
                var dataBreakpointExpression = ParseIdentifier(breakpointValue, opt);
                return ColumnCreator.CreateTypedExpressionDataBreakPoint(expression, dataBreakpointExpression);
            }
            return expression;
        }

        private static Type GetExplicitTypeOf(MetaExpression value, ParseIdentifierOption opt)
        {
            if (opt.overrideValueType != null) return opt.overrideValueType;
            else if (value.type != null) return value.type;
            return null;
        }

        public static Expression ParseAsDefault(MetaExpression value, ParseIdentifierOption opt, out Type resultExpressionType)
        {
            if (opt.defaultOnError)
            {
                resultExpressionType = typeof(string);
                if (opt.OnlyResolveType) return null;
                return new ExpConst<string>(value.value);
            }
            else
            {
                resultExpressionType = null;
                return null;
            }
        }

        public static bool TryParseAsIdentifier(MetaExpression value, ParseIdentifierOption opt, bool logError, out Expression expressionOut, ref Type resultExpressionType)
        {
            string[] identifier = value.value.Split('.');
            int colIdentifierIndex = 0;
            Table targetTable;
            View.Select sel = null;
            ExpressionParsingContext expressionParsingContext = opt.expressionParsingContext;
            if (identifier.Length == 2)
            {
                colIdentifierIndex = 1;
                if (expressionParsingContext != null && expressionParsingContext.TryGetSelect(identifier[0], out expressionParsingContext, out sel))
                {
                    targetTable = sel.sourceTable;
                }
                else if (opt.identifierColumn_table != null && identifier[0] == opt.identifierColumn_table.GetName())
                {
                    targetTable = opt.identifierColumn_table;
                }
                else
                {
                    targetTable = null;
                    if (logError)
                    {
                        Debug.LogError(opt.formatError("Unknown identifier '" + identifier[0] + "', must be a table or select name.", opt));

                    }
                }
            }
            else if (identifier.Length == 1)
            {
                colIdentifierIndex = 0;
                targetTable = opt.identifierColumn_table;
            }
            else
            {
                targetTable = null;
            }

            if (targetTable != null)
            {
                var col = targetTable.GetColumnByName(identifier[colIdentifierIndex]);
                var metaCol = targetTable.GetMetaData().GetColumnByName(identifier[colIdentifierIndex]);

                if (col != null && metaCol != null)
                {
                    Type columnValueType = metaCol.Type;

                    Type desiredValueType;
                    if (resultExpressionType != null)
                    {
                        desiredValueType = resultExpressionType;
                    }
                    else
                    {
                        desiredValueType = columnValueType;
                        resultExpressionType = desiredValueType;
                    }

                    if (opt.OnlyResolveType)
                    {
                        expressionOut = null;
                        return true;
                    }

                    Expression expression;
                    if (sel != null)
                    {
                        if (opt.useFirstMatchSelect && sel.isManyToMany)
                        {
                            expression = ColumnCreator.CreateTypedExpressionSelectFirstMatch(columnValueType, sel, col);
                        }
                        else
                        {
                            expression = ColumnCreator.CreateTypedExpressionSelect(columnValueType, sel, col);
                        }

                        if (expressionParsingContext.selectSet.Condition != null
                            && opt.BypassSelectSetCondition != expressionParsingContext.selectSet)
                        {
                            expression = ColumnCreator.CreateTypedExpressionSelectSetConditional(columnValueType, expressionParsingContext.selectSet, expression);
                        }
                    }
                    else
                    {
                        expression = ColumnCreator.CreateTypedExpressionColumn(columnValueType, col);
                    }

                    // Check if we need a fix row
                    if (value.row >= 0)
                    {
                        // options requires a fixed row
                        expression = ColumnCreator.CreateTypedExpressionFixedRow(expression, value.row);
                    }
                    else if (expressionParsingContext != null && expressionParsingContext.fixedRow >= 0)
                    {
                        // Parsing context requires a fixed row
                        expression = ColumnCreator.CreateTypedExpressionFixedRow(expression, expressionParsingContext.fixedRow);
                    }

                    // Check if we need a type change
                    if (desiredValueType != columnValueType)
                    {
                        //require type change
                        expression = ColumnCreator.CreateTypedExpressionTypeChange(desiredValueType, expression);
                    }

                    //Check for default value
                    if (value.hasDefaultValue)
                    {
                        expression = ColumnCreator.CreateTypedExpressionDefaultOnError(expression, value.valueDefault);
                    }
                    expressionOut = ProcessDataBreakpointValue(expression, value, opt);
                    return true;
                }
                else
                {
                    if (logError)
                    {
                        Debug.LogError(opt.formatError("Unknown identifier '" + identifier[colIdentifierIndex] + "', must be a column name.", opt));
                    }
                }
            }

            expressionOut = null;
            return false;
        }

        public static Expression ParseAsLiteralWithType(MetaExpression value, ParseIdentifierOption opt, Type type)
        {
            var expression = ColumnCreator.CreateTypedExpressionConst(type, value.value);
            return ProcessDataBreakpointValue(expression, value, opt);
        }

        /// <summary>
        /// Will try to deduce type of expression.
        /// int: starts with a number or a sign
        /// bool: is "true" or "false"
        /// returns null otherwise
        /// </summary>
        /// <returns></returns>
        public static Expression ParseAsLiteralWithTypeDeduction(MetaExpression value, ParseIdentifierOption opt, out Type resultExpressionType)
        {
            if (char.IsDigit(value.value[0]) || value.value[0] == '-' || value.value[0] == '+')
            {
                if (opt.overrideValueType != null) resultExpressionType = opt.overrideValueType;
                else if (value.type != null) resultExpressionType = value.type;
                else resultExpressionType = typeof(int);

                if (opt.OnlyResolveType) return null;

                var expression = ColumnCreator.CreateTypedExpressionConst(resultExpressionType, value.value);
                return ProcessDataBreakpointValue(expression, value, opt);
            }

            if (opt.overrideValueType == typeof(bool) || (opt.overrideValueType == null && value.type == typeof(bool)))
            {
                //could be a bool const value: "false" or "true"
                bool b;
                if (Boolean.TryParse(value.value, out b))
                {
                    resultExpressionType = typeof(bool);
                    if (opt.OnlyResolveType) return null;
                    var expression = new ExpConst<bool>(b);
                    return ProcessDataBreakpointValue(expression, value, opt);
                }
            }
            resultExpressionType = null;
            return null;
        }

        /// <summary>
        /// value can be interpreted as:
        /// a number: "0", "-1", "6.7", "+76"
        /// a bool: "true", "false"
        /// a table.column identifier: "tableName.columnName" (value.valueIsLitera must be false)
        /// a column identifier: "columnName" (value.valueIsLitera must be false)
        /// Will try to deduce type if it is not explicitly specified in either the value.type or opt.overrideValueType.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="opt">
        /// if opt.OnlyResolveType is true, will always return null
        /// if value.valueIsLiteral is true, will not parse for table/column identifiers
        /// </param>
        /// <param name="resultExpressionType"></param>
        /// <returns></returns>
        public static Expression ParseIdentifier(MetaExpression value, ParseIdentifierOption opt, out Type resultExpressionType)
        {
            if (value == null)
            {
                // nothing to parse
                return ParseAsDefault(value, opt, out resultExpressionType);
            }

            // Look for an explicit type
            resultExpressionType = GetExplicitTypeOf(value, opt);
            if (opt.OnlyResolveType && resultExpressionType != null) return null;

            Expression expression = null;

            // if no explicit type, try deducing it
            if (resultExpressionType == null)
            {
                //check for type deduction for non-string literals
                expression = ParseAsLiteralWithTypeDeduction(value, opt, out resultExpressionType);
                if (opt.OnlyResolveType && resultExpressionType != null) return null;
                if (expression != null) return expression;
            }

            // Type is string or could not deduce type.
            if (!value.valueIsLiteral)
            {
                // not forced as literal, check for identifier
                if (TryParseAsIdentifier(value, opt, false, out expression, ref resultExpressionType))
                {
                    if (opt.OnlyResolveType) return null;
                    return expression;
                }
            }

            // if no explicit type, could not deduce a type or is deducible as a string
            if (resultExpressionType == null)
            {
                // Force string
                resultExpressionType = typeof(string);
                if (opt.OnlyResolveType) return null;
                return ParseAsLiteralWithType(value, opt, resultExpressionType);
            }
            else
            {
                if (opt.OnlyResolveType) return null;
                return ParseAsLiteralWithType(value, opt, resultExpressionType);
            }
        }

        public static Database.Operation.Expression ParseIdentifier(MetaExpression value, ParseIdentifierOption opt)
        {
            Type resultExpressionType;
            return ParseIdentifier(value, opt, out resultExpressionType);
        }

        public static Type ResolveTypeOf(MetaExpression value, ParseIdentifierOption opt)
        {
            if (value == null) return null;
            opt.OnlyResolveType = true;
            Type resultExpressionType;
            ParseIdentifier(value, opt, out resultExpressionType);
            return resultExpressionType;
        }
    }

    /// <summary>
    /// Expression that yield a value of a specific type
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal abstract class TypedExpression<DataT> : Expression where DataT : IComparable
    {
        static public Type ExpressionType { get { return typeof(DataT); } }
        public abstract DataT GetValue(long row);
        // if GetValueString result may be different from GetValue().ToString()
        // When it is the same, we can avoid calling GetValue and GetValueString which may both be expensive
        public virtual bool StringValueDiffers { get { return true; } }
    }

    internal class ExpConst<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public ExpConst(DataT a)
        {
            value = a;
            type = typeof(DataT);
        }

        public ExpConst(string a)
        {
            value = (DataT)Convert.ChangeType(a, typeof(DataT));
            type = typeof(DataT);
        }

        public override DataT GetValue(long row)
        {
            return value;
        }

        public override bool StringValueDiffers { get { return false; } }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(value);
        }

        public override IComparable GetComparableValue(long row)
        {
            return value;
        }

        public override bool HasMultipleRow()
        {
            return false;
        }

        public override long RowCount()
        {
            return 1;
        }

        public DataT value;
    }

    /// <summary>
    /// Yield the value of a source column with same value type.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpColumn<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public Database.ColumnTyped<DataT> column;

        public ExpColumn(Database.Column c)
        {
            column = (Database.ColumnTyped<DataT>)c;
            type = typeof(DataT);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(GetValue(row));
        }

        public override DataT GetValue(long row)
        {
            return column.GetRowValue(row);

        }

        public override IComparable GetComparableValue(long row)
        {
            return column.GetRowValue(row);

        }

        public override bool HasMultipleRow()
        {
            return true;
        }

        public override long RowCount()
        {
            return column.GetRowCount();
        }
    }

    /// <summary>
    /// Output the row index for all entries of a given table
    /// </summary>
    internal class ExpTableRowIndex : TypedExpression<long>
    {
        public Table table;

        public ExpTableRowIndex(Table table)
        {
            this.table = table;
            type = typeof(long);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(row);
        }

        public override long GetValue(long row)
        {
            return row;
        }

        public override IComparable GetComparableValue(long row)
        {
            return row;
        }

        public override bool HasMultipleRow()
        {
            return true;
        }

        public override long RowCount()
        {
            return table.GetRowCount();
        }
    }

    /// <summary>
    /// Change the type of the input expression. will log an error if a value cannot change to the target type
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpTypeChange<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public Expression sourceExpression;

        public ExpTypeChange(Expression sourceExpression)
        {
            this.sourceExpression = sourceExpression;
            type = typeof(DataT);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(GetValue(row));
        }

        public override DataT GetValue(long row)
        {
            IComparable value = sourceExpression.GetComparableValue(row);
            try
            {
                return (DataT)Convert.ChangeType(value, typeof(DataT));
            }
            catch (Exception)
            {
                Debug.LogError("ExpTypeChange: Cannot type change value \"" + value.ToString()
                    + "\" from type '" + value.GetType().Name
                    + "' to type '" + typeof(DataT).Name
                    + "'"
                );
                return default(DataT);
            }
        }

        public override IComparable GetComparableValue(long row)
        {
            return GetValue(row);
        }

        public override bool HasMultipleRow()
        {
            return sourceExpression.HasMultipleRow();
        }

        public override long RowCount()
        {
            return sourceExpression.RowCount();
        }
    }

    /// <summary>
    /// Use the row parameter for the select result table
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpSelect<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public View.Select select;
        public Database.ColumnTyped<DataT> column;

        protected long[] m_rowIndex;
        public ExpSelect(View.Select sel, Database.Column c)
        {
            select = sel;
            column = (Database.ColumnTyped<DataT>)c;
            type = typeof(DataT);
        }

        private string GetOutOfRangeError(long row)
        {
            return "Out Of Range (" + row + " : [0," + m_rowIndex.Length + "[)";
        }

        public void Update()
        {
            if (m_rowIndex != null) return;
            m_rowIndex = select.GetMatchingIndices();
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            Update();
            if (m_rowIndex != null)
            {
                if (row < 0 & row >= m_rowIndex.Length)
                {
                    return GetOutOfRangeError(row);
                }
                return column.GetRowValueString(m_rowIndex[row], formatter);
            }
            else
            {
                //no indices means it uses the full table as is
                return column.GetRowValueString(row, formatter);
            }
        }

        public override DataT GetValue(long row)
        {
            Update();
            if (m_rowIndex != null)
            {
                return column.GetRowValue(m_rowIndex[row]);
            }
            else
            {
                //no indices means it uses the full table as is
                return column.GetRowValue(row);
            }
        }

        static readonly ProfilerMarker s_ExpSelectGetComparableValue = new ProfilerMarker("ExpSelect.GetComparableValue");
        public override IComparable GetComparableValue(long row)
        {
            using (s_ExpSelectGetComparableValue.Auto())
            {
                Update();
                if (m_rowIndex != null)
                {
                    if (row < 0 & row >= m_rowIndex.Length)
                    {
                        return GetOutOfRangeError(row);
                    }
                    return column.GetRowValue(m_rowIndex[row]);
                }
                else
                {
                    //no indices means it uses the full table as is
                    return column.GetRowValue(row);
                }
            }
        }

        public override bool HasMultipleRow()
        {
            return true;
        }

        public override long RowCount()
        {
            Update();
            if (m_rowIndex != null)
            {
                return m_rowIndex.LongLength;
            }
            return column.GetRowCount();
        }
    }

    /// <summary>
    /// Yield the result of a sub expression using the index of the main select from a SelectSet after passing the SelectSet's condition.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpSelectSetConditional<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public View.SelectSet SelectSet;
        public TypedExpression<DataT> SubExpression;
        public ExpSelectSetConditional(View.SelectSet selectSet, TypedExpression<DataT> subExpression)
        {
            SelectSet = selectSet;
            SubExpression = subExpression;
            type = typeof(DataT);
        }

        private long GetEffectiveRowCount()
        {
            long[] indices = SelectSet.GetConditionalRowIndices();
            if (indices == null) return SubExpression.RowCount();
            return indices.LongLength;
        }

        private long GetEffectiveRow(long rowIn)
        {
            long[] indices = SelectSet.GetConditionalRowIndices();
            if (indices == null) return rowIn;
            return indices[rowIn];
        }

        private string GetOutOfRangeError(long row)
        {
            return "Out Of Range (" + row + " : [0," + GetEffectiveRowCount() + "[)";
        }

        private bool IsInRange(long row)
        {
            return row >= 0 && row < GetEffectiveRowCount();
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            if (!IsInRange(row)) return GetOutOfRangeError(row);
            long effectiveRow = GetEffectiveRow(row);
            return SubExpression.GetValueString(effectiveRow, formatter);
        }

        public override DataT GetValue(long row)
        {
            long effectiveRow = GetEffectiveRow(row);
            return SubExpression.GetValue(effectiveRow);
        }

        public override IComparable GetComparableValue(long row)
        {
            long effectiveRow = GetEffectiveRow(row);
            return SubExpression.GetComparableValue(effectiveRow);
        }

        public override bool HasMultipleRow()
        {
            return SubExpression.HasMultipleRow();
        }

        public override long RowCount()
        {
            return GetEffectiveRowCount();
        }
    }

    /// <summary>
    /// Use the row parameter for the select where condition and only return the first match in each result
    /// The select used must be a many-to-many select, meaning it has a `row` parameter in its `where` statement and may yield 0,1 or several rows.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpFirstMatchSelect<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public View.Select select;
        public Database.ColumnTyped<DataT> column;
        public ExpFirstMatchSelect(View.Select sel, Database.Column c)
        {
            select = sel;
            column = (Database.ColumnTyped<DataT>)c;
            type = typeof(DataT);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            var index = select.GetIndexFirstMatch(row);
            if (index >= 0)
            {
                return column.GetRowValueString(index, formatter);
            }
            return "N/A";
        }

        public override DataT GetValue(long row)
        {
            var index = select.GetIndexFirstMatch(row);
            if (index >= 0)
            {
                return column.GetRowValue(index);
            }
            return default(DataT);
        }

        public override IComparable GetComparableValue(long row)
        {
            return GetValue(row);
        }

        public override bool HasMultipleRow()
        {
            return true;
        }

        public override long RowCount()
        {
            return select.GetIndexFirstMatchRowCount();
        }
    }

    /// <summary>
    /// Yield the result of a sub expression using a fixed row value for all requested rows.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpFixedRow<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public TypedExpression<DataT> exp;
        public long row;
        public ExpFixedRow(TypedExpression<DataT> exp, long row)
        {
            this.exp = exp;
            this.row = row;
            type = exp.type;
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return exp.GetValueString(this.row, formatter);
        }

        public override DataT GetValue(long row)
        {
            return exp.GetValue(this.row);
        }

        public override IComparable GetComparableValue(long row)
        {
            return exp.GetComparableValue(this.row);
        }

        public override bool HasMultipleRow()
        {
            return false;
        }

        public override long RowCount()
        {
            if (row < exp.RowCount()) return 1;
            return 0;
        }
    }

    /// <summary>
    /// Yield a default value if the input row is out of range of sub expression.
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpDefaultOnError<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public TypedExpression<DataT> SubExpression;
        public DataT DefaultValue;
        public ExpDefaultOnError(TypedExpression<DataT> exp, DataT defaultValue)
        {
            SubExpression = exp;
            DefaultValue = defaultValue;
            type = SubExpression.type;
        }

        private bool IsInRange(long row)
        {
            return row >= 0 && row < SubExpression.RowCount();
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            if (IsInRange(row)) return SubExpression.GetValueString(row, formatter);
            return formatter.Format(DefaultValue);
        }

        public override DataT GetValue(long row)
        {
            if (IsInRange(row)) return SubExpression.GetValue(row);
            return DefaultValue;
        }

        public override IComparable GetComparableValue(long row)
        {
            if (IsInRange(row)) return SubExpression.GetComparableValue(row);
            return DefaultValue;
        }

        public override bool HasMultipleRow()
        {
            return SubExpression.HasMultipleRow();
        }

        public override long RowCount()
        {
            return SubExpression.RowCount();
        }
    }

    /// <summary>
    /// Yields the merged value of a sub table from a viewtable's group index for a specific column.
    /// Ignores the row parameter
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpColumnMerge<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        private View.ViewTable m_ParentViewTable;
        private long m_ParentGroupIndex;
        private MetaColumn m_MetaColumnToMerge;
        public ExpColumnMerge(View.ViewTable parentViewTable, long parentGroupIndex, Column parentColumn, MetaColumn metaColumnToMerge)
        {
            m_ParentViewTable = parentViewTable;
            m_ParentGroupIndex = parentGroupIndex;
            m_MetaColumnToMerge = metaColumnToMerge;
            type = typeof(DataT);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(GetValue(row));

        }

        public override DataT GetValue(long row)
        {
            var subTable = m_ParentViewTable.CreateGroupTable(m_ParentGroupIndex);
            if (subTable != null)
            {
                var subColumn = subTable.GetColumnByIndex(m_MetaColumnToMerge.Index);
                while (subColumn is IColumnDecorator)
                {
                    subColumn = (subColumn as IColumnDecorator).GetBaseColumn();
                }
                return (DataT)m_MetaColumnToMerge.DefaultMergeAlgorithm.Merge(subColumn, new ArrayRange(0, subColumn.GetRowCount()));
            }
            return default(DataT);
        }

        public override bool StringValueDiffers { get { return false; } }

        public override IComparable GetComparableValue(long row)
        {
            var subTable = m_ParentViewTable.CreateGroupTable(m_ParentGroupIndex);
            if (subTable != null)
            {
                var subColumn = subTable.GetColumnByIndex(m_MetaColumnToMerge.Index);
                while (subColumn is IColumnDecorator)
                {
                    subColumn = (subColumn as IColumnDecorator).GetBaseColumn();
                }
                return m_MetaColumnToMerge.DefaultMergeAlgorithm.Merge(subColumn, new ArrayRange(0, subColumn.GetRowCount()));
            }
            return default(DataT);
        }

        public override bool HasMultipleRow()
        {
            return false;
        }

        public override long RowCount()
        {
            return 1;
        }
    }

    /// <summary>
    /// Triggers a breakpoint when the source expression yields a value equal the the break value expression. Useful for debugging only
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ExpDataBreakPoint<DataT> : TypedExpression<DataT> where DataT : IComparable
    {
        public TypedExpression<DataT> sourceExpression;
        public TypedExpression<DataT> BreakOnValue;
        public ExpDataBreakPoint(TypedExpression<DataT> sourceExpression, TypedExpression<DataT> breakOnValue)
        {
            this.sourceExpression = sourceExpression;
            type = typeof(DataT);
            BreakOnValue = breakOnValue;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void CheckBreakPoint(long row)
        {
            var rhv = sourceExpression.GetValue(row);
            var lhv = BreakOnValue.GetValue(row);
            if (lhv.CompareTo(rhv) == 0)
            {
                Debug.LogError("A user breakpoint event has been triggered. Attach a debugger and add a breakpoint on this line for debugging.");
            }
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            CheckBreakPoint(row);
            return sourceExpression.GetValueString(row, formatter);
        }

        public override DataT GetValue(long row)
        {
            CheckBreakPoint(row);
            return sourceExpression.GetValue(row);
        }

        public override IComparable GetComparableValue(long row)
        {
            CheckBreakPoint(row);
            return sourceExpression.GetComparableValue(row);
        }

        public override bool HasMultipleRow()
        {
            return sourceExpression.HasMultipleRow();
        }

        public override long RowCount()
        {
            return sourceExpression.RowCount();
        }
    }

    /// <summary>
    /// Base class providing a way to test and yield matching indices using an expression as input.
    /// </summary>
    internal abstract class Matcher
    {
        /// <summary>
        /// Test if the specified row in the expression pass the matching test
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract bool Match(Expression exp, long row);

        /// <summary>
        /// Returns matching indices from the expression parameter.
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public abstract long[] GetMatchIndex(Expression exp, ArrayRange indices);

        /// <summary>
        /// Returns matching indices from the expression parameter using a specified operator
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="indices"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public abstract long[] GetMatchIndex(Expression exp, ArrayRange indices, Operator operation);

        public Type type;
    }

    /// <summary>
    /// Matcher with a specific value type
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal abstract class TypedMatcher<DataT> : Matcher where DataT : IComparable
    {
        public abstract bool Match(DataT a);
    }

    /// <summary>
    /// Matcher that test the input expression with a constant value
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    internal class ConstMatcher<DataT> : TypedMatcher<DataT> where DataT : IComparable
    {
        public DataT value;
        public ConstMatcher(DataT c)
        {
            value = c;
            type = typeof(DataT);
        }

        public override bool Match(DataT a)
        {
            return value.CompareTo(a) == 0;
        }

        public override bool Match(Expression exp, long row)
        {
            var v = exp.GetComparableValue(row);
            return v.CompareTo(value) == 0;
        }

        public override long[] GetMatchIndex(Expression exp, ArrayRange indices)
        {
            long count = indices.Count;
            long[] o = new long[count];
            long lastO = 0;

            if (exp is TypedExpression<DataT>)
            {
                var e = (TypedExpression<DataT>)exp;

                for (int i = 0; i != count; ++i)
                {
                    long ii = indices[i];
                    var v = e.GetValue(ii);
                    if (v.CompareTo(value) == 0)
                    {
                        o[lastO] = ii;
                        ++lastO;
                    }
                }
            }
            else
            {
                for (int i = 0; i != count; ++i)
                {
                    long ii = indices[i];
                    var v = exp.GetComparableValue(ii);
                    if (v.CompareTo(value) == 0)
                    {
                        o[lastO] = ii;
                        ++lastO;
                    }
                }
            }
            if (lastO != count)
            {
                long[] trimmed = new long[lastO];
                System.Array.Copy(o, trimmed, lastO);
                return trimmed;
            }
            return o;
        }

        public override long[] GetMatchIndex(Expression exp, ArrayRange indices, Operator operation)
        {
            long count = indices.Count;
            long[] o = new long[count];
            long lastO = 0;

            if (exp is TypedExpression<DataT>)
            {
                var e = (TypedExpression<DataT>)exp;

                for (int i = 0; i != count; ++i)
                {
                    long ii = indices[i];
                    var v = e.GetValue(ii);
                    if (Operation.Match(operation, v.CompareTo(value)))
                    {
                        o[lastO] = ii;
                        ++lastO;
                    }
                }
            }
            else
            {
                for (int i = 0; i != count; ++i)
                {
                    long ii = indices[i];
                    var v = exp.GetComparableValue(ii);
                    if (Operation.Match(operation, v.CompareTo(value)))
                    {
                        o[lastO] = ii;
                        ++lastO;
                    }
                }
            }
            if (lastO != count)
            {
                long[] trimmed = new long[lastO];
                System.Array.Copy(o, trimmed, lastO);
                return trimmed;
            }
            return o;
        }
    }

    /// <summary>
    /// Matcher that test the input expression of value type `string` by searching for a constant sub-string value.
    /// </summary>
    internal class SubStringMatcher : TypedMatcher<string>
    {
        public string value;
        public IDataFormatter Formatter = DefaultDataFormatter.Instance;
        public override bool Match(string a)
        {
            return a.Contains(value);
        }

        public override bool Match(Expression exp, long row)
        {
            var v = exp.GetValueString(row, Formatter);
            return v.Contains(value);
        }

        public override long[] GetMatchIndex(Expression exp, ArrayRange indices)
        {
            var value2 = value.ToLower();
            long count = indices.Count;
            long[] o = new long[count];
            long lastO = 0;

            for (int i = 0; i != count; ++i)
            {
                long ii = indices[i];
                var v = exp.GetValueString(ii, Formatter);
                if (v.ToLower().Contains(value2))
                {
                    o[lastO] = ii;
                    ++lastO;
                }
            }

            if (lastO != count)
            {
                long[] trimmed = new long[lastO];
                System.Array.Copy(o, trimmed, lastO);
                return trimmed;
            }
            return o;
        }

        public override long[] GetMatchIndex(Expression exp, ArrayRange indices, Operator operation)
        {
            throw new InvalidOperationException("Do not use operators with string matcher");
        }
    }

    /// <summary>
    /// Yields the boolean result of comparing 2 expressions using a specified operator
    /// </summary>
    internal class ExpComparison : TypedExpression<bool>
    {
        public Expression valueLeft;
        public Operator operation;
        public Expression valueRight;
        public Operation.ComparableComparator comparator;
        public bool Evaluate(long rowLeft, long rowRight)
        {
            var valLeft = valueLeft.GetComparableValue(rowLeft);
            return Operation.Match(operation, comparator, valLeft, valueRight, rowRight);
        }

        public override bool GetValue(long row)
        {
            return Evaluate(row, row);
        }

        public override string GetValueString(long row, IDataFormatter formatter)
        {
            return formatter.Format(GetValue(row));
        }

        public override bool StringValueDiffers { get { return false; } }

        public override IComparable GetComparableValue(long row)
        {
            return GetValue(row);
        }

        public override bool HasMultipleRow()
        {
            return valueLeft.HasMultipleRow() || valueRight.HasMultipleRow();
        }

        public override long RowCount()
        {
            return Math.Min(valueLeft.RowCount(), valueRight.RowCount());
        }

        public bool IsManyToMany()
        {
            if (Operation.IsOperatorOneToMany(operation))
            {
                return false;
            }
            return valueLeft.HasMultipleRow() && valueRight.HasMultipleRow();
        }
    }

    /// <summary>
    /// Compares a column value to an expression using a specified operator
    /// </summary>
    internal class ColumnComparison
    {
        public Column column;
        public Operator operation;
        public Expression value;


        public long GetFirstMatchIndex(long row)
        {
            return column.GetFirstMatchIndex(operation, value, row);
        }

        public long[] GetMatchIndex(long row)
        {
            return column.GetMatchIndex(ArrayRange.FirstLast(0, column.GetRowCount()), operation, value, row, false);
        }

        public long[] GetMatchIndex(long row, ArrayRange range)
        {
            return column.GetMatchIndex(range, operation, value, row, false);
        }

        public bool IsManyToMany()
        {
            if (Operation.IsOperatorOneToMany(operation))
            {
                return false;
            }
            return value.HasMultipleRow();
        }
    }

    /// <summary>
    /// Base class for all MetaComparison. handles operator value to string translation
    /// </summary>
    internal class MetaComparisonBase
    {
        private static System.Collections.Generic.SortedDictionary<string, Operator> _m_StringToOp;
        protected static System.Collections.Generic.SortedDictionary<string, Operator> m_StringToOp
        {
            get
            {
                if (_m_StringToOp == null)
                {
                    _m_StringToOp = new System.Collections.Generic.SortedDictionary<string, Operator>();
                    _m_StringToOp.Add("equal", Operator.Equal);
                    _m_StringToOp.Add("greater", Operator.Greater);
                    _m_StringToOp.Add("greaterEqual", Operator.GreaterEqual);
                    _m_StringToOp.Add("less", Operator.Less);
                    _m_StringToOp.Add("lessEqual", Operator.LessEqual);
                    _m_StringToOp.Add("notEqual", Operator.NotEqual);
                    _m_StringToOp.Add("isIn", Operator.IsIn);
                    _m_StringToOp.Add("notIn", Operator.NotIn);

                    _m_StringToOp.Add("=", Operator.Equal);
                    _m_StringToOp.Add("==", Operator.Equal);
                    _m_StringToOp.Add(">", Operator.Greater);
                    _m_StringToOp.Add(">=", Operator.GreaterEqual);
                    _m_StringToOp.Add("<", Operator.Less);
                    _m_StringToOp.Add("<=", Operator.LessEqual);
                    _m_StringToOp.Add("!=", Operator.NotEqual);
                }
                return _m_StringToOp;
            }
        }
    }

    /// <summary>
    /// Represent an unresolved ColumnComparison. Can be resolved using a Expression.ParseIdentifierOption through the MetaColumnComparison.Build method
    /// Will be used to build `Where` statements in `Select` statements. See class Database.View.Where
    /// </summary>
    internal class MetaColumnComparison : MetaComparisonBase
    {
        public string columnName;
        public Operator operation;
        public Expression.MetaExpression value;
        public MetaColumnComparison() {}
        public MetaColumnComparison(string column, Operator operation, Expression.MetaExpression value)
        {
            columnName = column;
            this.operation = operation;
            this.value = value;
        }

        public ColumnComparison Build(Expression.ParseIdentifierOption option)
        {
            ColumnComparison comparison = new ColumnComparison();

            comparison.operation = operation;

            var metaColumn = option.identifierColumn_table.GetMetaData().GetColumnByName(columnName);
            if (metaColumn == null)
            {
                Debug.LogError(option.formatError("No column named '" + columnName + "' in table '" + option.identifierColumn_table.GetName() + "'", option));
                return null;
            }

            comparison.column = option.identifierColumn_table.GetColumnByName(columnName);
            if (comparison.column == null)
            {
                Debug.LogError(option.formatError("No column named '" + columnName + "' in table '" + option.identifierColumn_table.GetName() + "'", option));
                return null;
            }

            if (option.overrideValueType == null)
            {
                option.overrideValueType = metaColumn.Type;
            }

            if (Operation.IsOperatorOneToMany(operation))
            {
                option.useFirstMatchSelect = false;
            }
            comparison.value = Expression.ParseIdentifier(value, option);

            return comparison;
        }

        public static MetaColumnComparison LoadFromXML(System.Xml.XmlElement root)
        {
            MetaColumnComparison comparison = new MetaColumnComparison();
            comparison.columnName = root.GetAttribute("column");

            comparison.value = Expression.MetaExpression.LoadFromXML(root);
            string strOp = root.GetAttribute("op");
            if (!m_StringToOp.TryGetValue(strOp, out comparison.operation))
            {
                Debug.LogError("Unknown operator '" + strOp + "'.");
            }
            //Debug.LogAnyXmlChildAsInvalid(root);
            return comparison;
        }
    }

    /// <summary>
    /// Represent an unresolved ExpComparison. Can be resolved using a Expression.ParseIdentifierOption through the MetaExpComparison.Build method
    /// </summary>
    internal class MetaExpComparison : MetaComparisonBase
    {
        public Expression.MetaExpression valueLeft;
        public Operator operation;
        public Expression.MetaExpression valueRight;
        public ExpComparison Build(Expression.ParseIdentifierOption option)
        {
            ExpComparison comparison = new ExpComparison();
            comparison.operation = operation;
            comparison.valueLeft = Expression.ParseIdentifier(valueLeft, option);
            comparison.valueRight = Expression.ParseIdentifier(valueRight, option);
            comparison.comparator = Operation.GetComparator(comparison.valueLeft.type, comparison.valueRight.type);
            return comparison;
        }

        public static MetaExpComparison LoadFromXML(System.Xml.XmlElement root)
        {
            MetaExpComparison exp = new MetaExpComparison();

            string strOp = root.GetAttribute("op");
            if (!m_StringToOp.TryGetValue(strOp, out exp.operation))
            {
                Debug.LogError("Unknown operator '" + strOp + "'.");
            }

            foreach (XmlNode xNode in root.ChildNodes)
            {
                if (xNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement e = (XmlElement)xNode;
                    switch (e.Name)
                    {
                        case "Left":
                            exp.valueLeft = Expression.MetaExpression.LoadFromXML(e);
                            break;
                        case "Right":
                            exp.valueRight = Expression.MetaExpression.LoadFromXML(e);
                            break;
                        default:
                            //DebugUtility.LogInvalidXmlChild(root, e);
                            break;
                    }
                }
            }
            return exp;
        }
    }
}
