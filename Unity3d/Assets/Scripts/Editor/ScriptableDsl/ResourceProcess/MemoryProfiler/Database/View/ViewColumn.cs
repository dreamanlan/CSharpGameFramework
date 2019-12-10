using System;
using System.Xml;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.Database.View
{
    internal class ViewColumn
    {
        public Select select;
        public Operation.ExpressionParsingContext ParsingContext;
        public ViewTable viewTable;
        public bool m_IsDisplayMergedOnly = false;
        public TableLink m_MetaLink;

        public interface IViewColumn
        {
            void SetColumn(ViewColumn vc, Database.Column col);
            void SetConstValue(string value); //used for const value column only
            Database.Column GetColumn();
        }
        public class Builder
        {
            public string name;
            public Database.Operation.Grouping.IGroupAlgorithm groupAlgo;

            public Operation.Expression.MetaExpression value;

            public int displayDefaultWidth = 100;
            public Operation.Grouping.MergeAlgo mergeAlgoE;
            public TableLink m_MetaLink;
            public bool isPrimaryKey = false;
            public string FormatName;

            public Builder() {}
            public Builder(string name, Operation.Expression.MetaExpression value)
            {
                this.name = name;
                this.value = value;
            }

            private string FormatErrorContextInfo(ViewSchema vs, ViewTable vTable)
            {
                string str = "Error while building view column '" + name + "'";
                if (vs != null) str += " schema '" + vs.name + "'";
                if (vTable != null) str += " view table '" + vTable.GetName() + "'";
                return str;
            }

            private Operation.Grouping.IMergeAlgorithm BuildOrGetMergeAlgo(ViewColumn vc, Type columnValueType, Database.MetaColumn metaColumn)
            {
                Operation.Grouping.IMergeAlgorithm mergeAlgo = null;
                if (mergeAlgoE != Operation.Grouping.MergeAlgo.none)
                {
                    mergeAlgo = Operation.Grouping.GetMergeAlgo(mergeAlgoE, columnValueType);
                    if (metaColumn != null)
                    {
                        metaColumn.DefaultMergeAlgorithm = mergeAlgo;
                    }
                }
                else
                {
                    if (metaColumn != null)
                    {
                        mergeAlgo = metaColumn.DefaultMergeAlgorithm;
                    }
                }

                if (vc != null && mergeAlgo != null && mergeAlgo.IsDisplayMergedRowsOnly())
                {
                    vc.m_IsDisplayMergedOnly = true;
                }
                return mergeAlgo;
            }

            private MetaColumn BuildOrUpdateMetaColumn(ViewTable.Builder.BuildingData buildingData, ref MetaColumn metaColumn, Type columnValueType, Operation.Grouping.IMergeAlgorithm mergeAlgo)
            {
                if (metaColumn == null)
                {
                    metaColumn = new MetaColumn(name, name, columnValueType, isPrimaryKey, groupAlgo, mergeAlgo, FormatName, displayDefaultWidth);
                    buildingData.MetaTable.AddColumn(metaColumn);
                }
                else
                {
                    if (metaColumn.Type == null)
                    {
                        metaColumn.Type = columnValueType;
                    }
                    else if (columnValueType != null && metaColumn.Type != columnValueType)
                    {
                        Debug.LogError("Cannot redefine column type as '" + columnValueType + "'. Was already defined as '" + metaColumn.Type + "'");
                    }
                    if (!String.IsNullOrEmpty(FormatName))
                    {
                        if (String.IsNullOrEmpty(metaColumn.FormatName) || metaColumn.FormatName == FormatName)
                        {
                            metaColumn.FormatName = FormatName;
                        }
                        else
                        {
                            Debug.LogWarning("Format already defined as '" + metaColumn.FormatName + "'. Trying to redefined it as '" + FormatName + "'");
                        }
                    }
                }
                return metaColumn;
            }

            // a column declaration only creates or adds to the column meta data. it does not create the actual column.
            public void BuildOrUpdateDeclaration(ViewTable.Builder.BuildingData buildingData, ViewTable vTable, ref MetaColumn metaColumn, Operation.ExpressionParsingContext expressionParsingContext, Type aOverrideType = null)
            {
                Type finalType = aOverrideType != null
                    ? aOverrideType
                    : (value == null
                        ? null
                        : value.type
                    );

                //Build meta data
                var mergeAlgo = BuildOrGetMergeAlgo(null, finalType, metaColumn);
                BuildOrUpdateMetaColumn(buildingData, ref metaColumn, finalType, mergeAlgo);


                if (metaColumn != null && metaColumn.Type == null && finalType == null && !buildingData.FallbackColumnType.ContainsKey(metaColumn))
                {
                    Operation.Expression.ParseIdentifierOption parseOpt = new Operation.Expression.ParseIdentifierOption(buildingData.Schema, vTable, true, false, null, expressionParsingContext);
                    var fallbackType = Operation.Expression.ResolveTypeOf(value, parseOpt);
                    if (fallbackType != null)
                    {
                        buildingData.FallbackColumnType.Add(metaColumn, fallbackType);
                    }
                }
            }

            //Create a column that merge the result of all sub nodes
            static public Column BuildColumnNodeMerge(ViewTable vTable, Database.MetaColumn metaColumn, Operation.ExpressionParsingContext expressionParsingContext)
            {
                var columnNode = (ViewColumnNode.IViewColumnNode)Operation.ColumnCreator.CreateColumn(typeof(ViewColumnNodeMergeTyped<>), metaColumn.Type);
                ViewColumnNode viewColumnNode = new ViewColumnNode(vTable, metaColumn, expressionParsingContext);
                columnNode.SetColumn(viewColumnNode);
                return columnNode.GetColumn();
            }

            static ViewColumnNode.IViewColumnNode BuildOrGetColumnNode(ViewTable vTable, Database.MetaColumn metaColumn, string columnName, Operation.ExpressionParsingContext expressionParsingContext)
            {
                var column = vTable.GetColumnByName(columnName);
                if (column == null)
                {
                    var columnNode = (ViewColumnNode.IViewColumnNode)Operation.ColumnCreator.CreateColumn(typeof(ViewColumnNodeTyped<>), metaColumn.Type);
                    ViewColumnNode viewColumnNode = new ViewColumnNode(vTable, metaColumn, expressionParsingContext);

                    columnNode.SetColumn(viewColumnNode);
                    vTable.SetColumn(metaColumn, columnNode.GetColumn());
                    return columnNode;
                }

                // View table use expand column to decorate the IViewColumnNode with required functionality for expanding rows.
                // So we need to get the underlying column, which is the IViewColumnNode we want.
                if (column is IColumnDecorator)
                {
                    column = (column as IColumnDecorator).GetBaseColumn();
                }

                if (column is ViewColumnNode.IViewColumnNode)
                {
                    return (ViewColumnNode.IViewColumnNode)column;
                }
                else
                {
                    throw new Exception("Expecting column  '" + vTable.GetName() + "." + metaColumn.Name + "' to be a from a node data type (ViewColumnNode) but is of type '" + column.GetType().Name + "'");
                }
            }

            //A column under a <Node> (or <View>) element is either a declaration (build meta data only) or defines an entry in the parent's ViewColumnNode
            public void BuildNodeValue(ViewTable.Builder.BuildingData buildingData, ViewTable vTable, ViewTable.Builder.Node node, long row, ViewTable parentViewTable, Operation.ExpressionParsingContext expressionParsingContext, ref Database.MetaColumn metaColum)
            {
                BuildOrUpdateDeclaration(buildingData, vTable, ref metaColum, expressionParsingContext);

                //If the parent's node data type is Node
                if (node.parent != null && node.parent.data.type == ViewTable.Builder.Node.Data.DataType.Node)
                {
                    // this column is an entry in the parent's column
                    var option = new Operation.Expression.ParseIdentifierOption(buildingData.Schema, parentViewTable, true, true, metaColum != null ? metaColum.Type : null, expressionParsingContext);
                    option.formatError = (string s, Operation.Expression.ParseIdentifierOption opt) =>
                    {
                        return FormatErrorContextInfo(buildingData.Schema, parentViewTable) + " : " + s;
                    };
                    Operation.Expression expression = Operation.Expression.ParseIdentifier(value, option);

                    //if the meta column does not have a type defined yet, define it as the expression's type.
                    if (metaColum.Type == null)
                    {
                        metaColum.Type = expression.type;
                    }
                    ViewColumnNode.IViewColumnNode column = BuildOrGetColumnNode(parentViewTable, metaColum, name, expressionParsingContext);
                    column.SetEntry(row, expression, m_MetaLink);
                }
            }

            // Set a Node value to the merged result of it's sub entries
            public static void BuildNodeValueDefault(ViewTable.Builder.BuildingData buildingData, ViewTable.Builder.Node node, long row, ViewTable parentViewTable, Operation.ExpressionParsingContext expressionParsingContext, Database.MetaColumn metaColumn)
            {
                //set the entry for merge column
                if (metaColumn.DefaultMergeAlgorithm != null && metaColumn.Type != null)
                {
                    ViewColumnNode.IViewColumnNode column = BuildOrGetColumnNode(parentViewTable, metaColumn, metaColumn.Name, expressionParsingContext);
                    Operation.Expression expression = Operation.ColumnCreator.CreateTypedExpressionColumnMerge(metaColumn.Type, parentViewTable, row, column.GetColumn(), metaColumn);
                    column.SetEntry(row, expression, null);
                }
            }

            public IViewColumn Build(ViewTable.Builder.BuildingData buildingData, ViewTable.Builder.Node node, ViewTable vTable, Operation.ExpressionParsingContext expressionParsingContext, ref Database.MetaColumn metaColumn)
            {
                // Check if we have a type mismatch
                Type columnValueType = metaColumn != null ? metaColumn.Type : null;
                if (value != null && value.type != null)
                {
                    if (columnValueType != null && columnValueType != value.type)
                    {
                        Debug.LogWarning("While building column '" + name + "' : "
                            + "Cannot override type from '" + columnValueType.Name
                            + "' to '" + value.type.Name + "'");
                    }
                    columnValueType = value.type;
                }

                // Parse expression value
                Operation.Expression.ParseIdentifierOption parseOpt = new Operation.Expression.ParseIdentifierOption(buildingData.Schema, vTable, true, false, columnValueType, expressionParsingContext);
                parseOpt.formatError = (string s, Operation.Expression.ParseIdentifierOption opt) => {
                    return FormatErrorContextInfo(buildingData.Schema, vTable) + " : " + s;
                };

                Operation.Expression expression = Operation.Expression.ParseIdentifier(value, parseOpt);


                // Build declaration with the type we've just parsed
                BuildOrUpdateDeclaration(buildingData, vTable, ref metaColumn, expressionParsingContext, expression.type);

                IViewColumn result = (IViewColumn)Operation.ColumnCreator.CreateViewColumnExpression(expression);
                ViewColumn vc = new ViewColumn();
                vc.m_MetaLink = m_MetaLink;
                vc.viewTable = vTable;
                vc.ParsingContext = expressionParsingContext;
                result.SetColumn(vc, null);
                return result;
            }

            private static System.Collections.Generic.SortedDictionary<string, Operation.Grouping.MergeAlgo> _m_StringToMergeAlgo;
            protected static System.Collections.Generic.SortedDictionary<string, Operation.Grouping.MergeAlgo> m_StringToMergeAlgo
            {
                get
                {
                    if (_m_StringToMergeAlgo == null)
                    {
                        _m_StringToMergeAlgo = new System.Collections.Generic.SortedDictionary<string, Operation.Grouping.MergeAlgo>();
                        _m_StringToMergeAlgo.Add("first", Operation.Grouping.MergeAlgo.first);
                        _m_StringToMergeAlgo.Add("sum", Operation.Grouping.MergeAlgo.sum);
                        _m_StringToMergeAlgo.Add("min", Operation.Grouping.MergeAlgo.min);
                        _m_StringToMergeAlgo.Add("max", Operation.Grouping.MergeAlgo.max);
                        _m_StringToMergeAlgo.Add("average", Operation.Grouping.MergeAlgo.average);
                        _m_StringToMergeAlgo.Add("deviation", Operation.Grouping.MergeAlgo.deviation);
                        _m_StringToMergeAlgo.Add("median", Operation.Grouping.MergeAlgo.median);
                        _m_StringToMergeAlgo.Add("sumpositive", Operation.Grouping.MergeAlgo.sumpositive);
                        _m_StringToMergeAlgo.Add("count", Operation.Grouping.MergeAlgo.count);
                    }
                    return _m_StringToMergeAlgo;
                }
            }

            private static void LoadLinkFromXML(Builder b, XmlElement root)
            {
                b.m_MetaLink = new TableLink();
                DebugUtility.TryGetMandatoryXmlAttribute(root, "view", out b.m_MetaLink.TableName);
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement e = (XmlElement)node;
                        if (e.Name == "Where")
                        {
                            var w = Where.Builder.LoadFromXML(e);
                            if (b.m_MetaLink.RowWhere == null)
                            {
                                b.m_MetaLink.RowWhere = new System.Collections.Generic.List<Where.Builder>();
                            }
                            b.m_MetaLink.RowWhere.Add(w);
                        }
                        else if (e.Name == "Param")
                        {
                            string paramName, paramValue;
                            if (DebugUtility.TryGetMandatoryXmlAttribute(e, "name", out paramName)
                                && DebugUtility.TryGetMandatoryXmlAttribute(e, "value", out paramValue))
                            {
                                b.m_MetaLink.SetParameter(paramName, paramValue);
                            }
                        }
                    }
                }
            }

            private static void LoadParentGroupFromXML(Builder b, XmlElement root)
            {
            }

            public static Builder LoadFromXML(XmlElement root)
            {
                Builder b = new Builder();
                DebugUtility.TryGetMandatoryXmlAttribute(root, "name", out b.name);
                b.value = Operation.Expression.MetaExpression.LoadFromXML(root);
                if (!int.TryParse(root.GetAttribute("width"), out b.displayDefaultWidth))
                {
                    b.displayDefaultWidth = 100;
                }

                string strisKey = root.GetAttribute("isKey");
                if (strisKey != null && strisKey != "")
                {
                    if (!bool.TryParse(strisKey, out b.isPrimaryKey))
                    {
                        UnityEngine.Debug.LogError("Error parsing bool value '" + strisKey + "', default to false");
                        b.isPrimaryKey = false;
                    }
                }
                string strGroupable = root.GetAttribute("groupable");
                if (strGroupable == "duplicate")
                {
                    b.groupAlgo = Database.Operation.Grouping.groupByDuplicate;
                }
                string strOp = root.GetAttribute("merged");
                m_StringToMergeAlgo.TryGetValue(strOp, out b.mergeAlgoE);

                b.FormatName = root.GetAttribute("format");

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement e = (XmlElement)node;
                        if (e.Name == "Link")
                        {
                            LoadLinkFromXML(b, e);
                        }
                        else if (e.Name == "ParentGroup")
                        {
                            LoadParentGroupFromXML(b, e);
                        }
                    }
                }

                return b;
            }
        }
    }
}
