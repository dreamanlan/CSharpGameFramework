using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.MemoryProfilerForExtension.Editor.Database.View;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Operation
{
    internal interface IColumnFactory
    {
    }

    internal abstract class TypedColumnFactory : IColumnFactory
    {
        public abstract Column CreateTypedColumn();
    }

    internal abstract class ViewColumnExpressionFactory : IColumnFactory
    {
        public abstract Column CreateViewColumnExpression(Expression expression);
    }

    internal class ViewColumnExpressionTypeFactory<T> : ViewColumnExpressionFactory where T : IComparable
    {
        public override Column CreateViewColumnExpression(Expression expression)
        {
            return new ViewColumnExpressionType<T>((TypedExpression<T>)expression);
        }
    }

    internal abstract class ExpressionFactory : IColumnFactory
    {
        public abstract Expression CreateTypedExpressionColumn(Column source);
        public abstract Expression CreateTypedExpressionFixedRow(Expression baseExp, long row);
        public abstract Expression CreateTypedExpressionConst(string aConstValue);
        public abstract Expression CreateTypedExpressionTypeChange(Expression sourceExp);
        public abstract Expression CreateTypedExpressionSelectSetConditional(SelectSet selectSet, Expression subExpression);
        public abstract Expression CreateTypedExpressionSelect(Select selection, Column source);
        public abstract Expression CreateTypedExpressionSelectFirstMatch(Select selection, Column source);
        public abstract Expression CreateTypedExpressionColumnMerge(View.ViewTable parentViewTable, long parentGroupIndex, Column parentColumn, MetaColumn metaColumnToMerge);
        public abstract Expression CreateTypedExpressionDefaultOnError(Expression baseExp, IComparable defaultValue);
        public abstract Expression CreateTypedExpressionDataBreakPoint(Expression baseExp, Expression value);
    }

    internal abstract class MatcherFactory : IColumnFactory
    {
        protected interface IParser {}
        protected class Parser<T> : IParser
        {
            public delegate bool ParseDelegate(string s, out T result);
            public readonly ParseDelegate tryParse;

            public Parser(ParseDelegate p)
            {
                tryParse = p;
            }
        }
        private static bool tryParseEnum<T>(string s, out T result)
        {
            result = (T)Enum.Parse(typeof(T), s);
            return true;
        }

        protected static readonly Dictionary<Type, IParser> kParseTypes = new Dictionary<Type, IParser>()
        {
            {typeof(bool), new Parser<bool>((string s, out bool x) => bool.TryParse(s, out x))},
            {typeof(double), new Parser<double>((string s, out double x) => double.TryParse(s, out x))},
            {typeof(float), new Parser<float>((string s, out float x) => float.TryParse(s, out x))},
            {typeof(int), new Parser<int>((string s, out int x) => int.TryParse(s, out x))},
            {typeof(long), new Parser<long>((string s, out long x) => long.TryParse(s, out x))},
            {typeof(short), new Parser<short>((string s, out short x) => short.TryParse(s, out x))},
            {typeof(uint), new Parser<uint>((string s, out uint x) => uint.TryParse(s, out x))},
            {typeof(ulong), new Parser<ulong>((string s, out ulong x) => ulong.TryParse(s, out x))},
            {typeof(ushort), new Parser<ushort>((string s, out ushort x) => ushort.TryParse(s, out x))},
            {typeof(DiffTable.DiffResult), new Parser<DiffTable.DiffResult>(tryParseEnum<DiffTable.DiffResult>)}
        };
        public abstract Matcher CreateTypedMatcher(string matchString);
    }

    internal class ColumnFactory<ColumnT> : TypedColumnFactory where ColumnT : Column, new()
    {
        public override Column CreateTypedColumn()
        {
            return new ColumnT();
        }
    }
    internal class ViewFirstMatchColumnFactory<T> : TypedColumnFactory where T : IComparable
    {
        public override Column CreateTypedColumn()
        {
            return new ViewColumnFirstMatch<T>();
        }
    }

    internal class ExpressionColumnFactory<T> : ExpressionFactory where T : IComparable
    {
        public override Expression CreateTypedExpressionColumn(Column source)
        {
            return new ExpColumn<T>(source);
        }

        public override Expression CreateTypedExpressionFixedRow(Expression baseExp, long row)
        {
            return new ExpFixedRow<T>((TypedExpression<T>)baseExp, row);
        }

        public override Expression CreateTypedExpressionConst(string aConstValue)
        {
            return new ExpConst<T>(aConstValue);
        }

        public override Expression CreateTypedExpressionTypeChange(Expression aSourceExp)
        {
            return new ExpTypeChange<T>(aSourceExp);
        }

        public override Expression CreateTypedExpressionSelectSetConditional(SelectSet selectSet, Expression subExpression)
        {
            return new ExpSelectSetConditional<T>(selectSet, subExpression as TypedExpression<T>);
        }

        public override Expression CreateTypedExpressionSelect(Select selection, Column source)
        {
            return new ExpSelect<T>(selection, source);
        }

        public override Expression CreateTypedExpressionSelectFirstMatch(Select selection, Column source)
        {
            return new ExpFirstMatchSelect<T>(selection, source);
        }

        public override Expression CreateTypedExpressionColumnMerge(View.ViewTable parentViewTable, long parentGroupIndex, Column parentColumn, MetaColumn metaColumnToMerge)
        {
            return new ExpColumnMerge<T>(parentViewTable, parentGroupIndex, parentColumn, metaColumnToMerge);
        }

        public override Expression CreateTypedExpressionDefaultOnError(Expression baseExp, IComparable defaultValue)
        {
            T v = (T)Convert.ChangeType(defaultValue, typeof(T));
            return new ExpDefaultOnError<T>((TypedExpression<T>)baseExp, v);
        }

        public override Expression CreateTypedExpressionDataBreakPoint(Expression baseExp, Expression value)
        {
            return new ExpDataBreakPoint<T>((TypedExpression<T>)baseExp, (TypedExpression<T>)value);
        }
    }

    internal class ConstMatcherFactory<T> : MatcherFactory where T : IComparable
    {
        public override Matcher CreateTypedMatcher(string matchString)
        {
            IParser p;
            if (!kParseTypes.TryGetValue(typeof(T), out p))
            {
                throw new UnityException("Parser for specified type does not exist");
            }
            T constVal;
            if (!((Parser<T>)p).tryParse(matchString, out constVal))
                return null;
            return new ConstMatcher<T>(constVal);
        }
    }

    internal struct TypePair
    {
        public Type Key, Value;
    };

    internal static class ColumnCreator
    {
        private static Dictionary<Type, IColumnFactory> kExpressionFactory = new Dictionary<Type, IColumnFactory>()
        {
            {typeof(bool), new ExpressionColumnFactory<bool>() },
            {typeof(double), new ExpressionColumnFactory<double>() },
            {typeof(float), new ExpressionColumnFactory<float>() },
            {typeof(int), new ExpressionColumnFactory<int>() },
            {typeof(long), new ExpressionColumnFactory<long>() },
            {typeof(short), new ExpressionColumnFactory<short>() },
            {typeof(string), new ExpressionColumnFactory<string>() },
            {typeof(uint), new ExpressionColumnFactory<uint>() },
            {typeof(ulong), new ExpressionColumnFactory<ulong>() },
            {typeof(ushort), new ExpressionColumnFactory<ushort>() },
            {typeof(DiffTable.DiffResult), new ExpressionColumnFactory<DiffTable.DiffResult>() },
            {typeof(ManagedConnection.ConnectionType), new ExpressionColumnFactory<ManagedConnection.ConnectionType>() },
        };
        private static Dictionary<Type, ViewColumnExpressionFactory> kViewColumnExpressionTypeFactory = new Dictionary<Type, ViewColumnExpressionFactory>()
        {
            {typeof(bool), new ViewColumnExpressionTypeFactory<bool>() },
            {typeof(double), new ViewColumnExpressionTypeFactory<double>() },
            {typeof(float), new ViewColumnExpressionTypeFactory<float>() },
            {typeof(int), new ViewColumnExpressionTypeFactory<int>() },
            {typeof(long), new ViewColumnExpressionTypeFactory<long>() },
            {typeof(short), new ViewColumnExpressionTypeFactory<short>() },
            {typeof(string), new ViewColumnExpressionTypeFactory<string>() },
            {typeof(uint), new ViewColumnExpressionTypeFactory<uint>() },
            {typeof(ulong), new ViewColumnExpressionTypeFactory<ulong>() },
            {typeof(ushort), new ViewColumnExpressionTypeFactory<ushort>() },
            {typeof(DiffTable.DiffResult), new ViewColumnExpressionTypeFactory<DiffTable.DiffResult>() },
            {typeof(ManagedConnection.ConnectionType), new ViewColumnExpressionTypeFactory<ManagedConnection.ConnectionType>() },
        };


        private static Dictionary<TypePair, IColumnFactory> kFactories = new Dictionary<TypePair, IColumnFactory>()
        {
            //Typed column - Expanded
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(bool) }, new ColumnFactory<ExpandColumnTyped<bool>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(double) }, new ColumnFactory<ExpandColumnTyped<double>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(float) }, new ColumnFactory<ExpandColumnTyped<float>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(int) }, new ColumnFactory<ExpandColumnTyped<int>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(long) }, new ColumnFactory<ExpandColumnTyped<long>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(short) }, new ColumnFactory<ExpandColumnTyped<short>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(string) }, new ColumnFactory<ExpandColumnTyped<string>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(uint) }, new ColumnFactory<ExpandColumnTyped<uint>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(ulong) }, new ColumnFactory<ExpandColumnTyped<ulong>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(ushort) }, new ColumnFactory<ExpandColumnTyped<ushort>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ExpandColumnTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ExpandColumnTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ExpandColumnTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - diff
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(bool) }, new ColumnFactory<DiffColumnTyped<bool>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(double) }, new ColumnFactory<DiffColumnTyped<double>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(float) }, new ColumnFactory<DiffColumnTyped<float>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(int) }, new ColumnFactory<DiffColumnTyped<int>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(long) }, new ColumnFactory<DiffColumnTyped<long>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(short) }, new ColumnFactory<DiffColumnTyped<short>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(string) }, new ColumnFactory<DiffColumnTyped<string>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(uint) }, new ColumnFactory<DiffColumnTyped<uint>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(ulong) }, new ColumnFactory<DiffColumnTyped<ulong>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(ushort) }, new ColumnFactory<DiffColumnTyped<ushort>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<DiffColumnTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(DiffColumnTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<DiffColumnTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - grouped
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(bool) }, new ColumnFactory<GroupedColumnTyped<bool>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(double) }, new ColumnFactory<GroupedColumnTyped<double>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(float) }, new ColumnFactory<GroupedColumnTyped<float>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(int) }, new ColumnFactory<GroupedColumnTyped<int>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(long) }, new ColumnFactory<GroupedColumnTyped<long>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(short) }, new ColumnFactory<GroupedColumnTyped<short>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(string) }, new ColumnFactory<GroupedColumnTyped<string>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(uint) }, new ColumnFactory<GroupedColumnTyped<uint>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(ulong) }, new ColumnFactory<GroupedColumnTyped<ulong>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(ushort) }, new ColumnFactory<GroupedColumnTyped<ushort>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<GroupedColumnTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(GroupedColumnTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<GroupedColumnTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - indexed
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(bool) }, new ColumnFactory<IndexedColumnTyped<bool>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(double) }, new ColumnFactory<IndexedColumnTyped<double>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(float) }, new ColumnFactory<IndexedColumnTyped<float>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(int) }, new ColumnFactory<IndexedColumnTyped<int>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(long) }, new ColumnFactory<IndexedColumnTyped<long>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(short) }, new ColumnFactory<IndexedColumnTyped<short>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(string) }, new ColumnFactory<IndexedColumnTyped<string>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(uint) }, new ColumnFactory<IndexedColumnTyped<uint>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(ulong) }, new ColumnFactory<IndexedColumnTyped<ulong>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(ushort) }, new ColumnFactory<IndexedColumnTyped<ushort>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<IndexedColumnTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(IndexedColumnTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<IndexedColumnTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - view
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(bool) }, new ColumnFactory<ViewColumnTyped<bool>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(double) }, new ColumnFactory<ViewColumnTyped<double>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(float) }, new ColumnFactory<ViewColumnTyped<float>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(int) }, new ColumnFactory<ViewColumnTyped<int>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(long) }, new ColumnFactory<ViewColumnTyped<long>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(short) }, new ColumnFactory<ViewColumnTyped<short>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(string) }, new ColumnFactory<ViewColumnTyped<string>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(uint) }, new ColumnFactory<ViewColumnTyped<uint>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(ulong) }, new ColumnFactory<ViewColumnTyped<ulong>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(ushort) }, new ColumnFactory<ViewColumnTyped<ushort>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ViewColumnTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ViewColumnTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ViewColumnTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - view
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(bool) }, new ColumnFactory<ViewColumnNodeTyped<bool>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(double) }, new ColumnFactory<ViewColumnNodeTyped<double>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(float) }, new ColumnFactory<ViewColumnNodeTyped<float>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(int) }, new ColumnFactory<ViewColumnNodeTyped<int>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(long) }, new ColumnFactory<ViewColumnNodeTyped<long>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(short) }, new ColumnFactory<ViewColumnNodeTyped<short>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(string) }, new ColumnFactory<ViewColumnNodeTyped<string>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(uint) }, new ColumnFactory<ViewColumnNodeTyped<uint>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(ulong) }, new ColumnFactory<ViewColumnNodeTyped<ulong>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(ushort) }, new ColumnFactory<ViewColumnNodeTyped<ushort>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ViewColumnNodeTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ViewColumnNodeTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ViewColumnNodeTyped<ManagedConnection.ConnectionType>>()},

            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(bool) }, new ColumnFactory<ViewColumnNodeMergeTyped<bool>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(double) }, new ColumnFactory<ViewColumnNodeMergeTyped<double>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(float) }, new ColumnFactory<ViewColumnNodeMergeTyped<float>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(int) }, new ColumnFactory<ViewColumnNodeMergeTyped<int>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(long) }, new ColumnFactory<ViewColumnNodeMergeTyped<long>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(short) }, new ColumnFactory<ViewColumnNodeMergeTyped<short>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(string) }, new ColumnFactory<ViewColumnNodeMergeTyped<string>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(uint) }, new ColumnFactory<ViewColumnNodeMergeTyped<uint>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(ulong) }, new ColumnFactory<ViewColumnNodeMergeTyped<ulong>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(ushort) }, new ColumnFactory<ViewColumnNodeMergeTyped<ushort>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ViewColumnNodeMergeTyped<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ViewColumnNodeMergeTyped<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ViewColumnNodeMergeTyped<ManagedConnection.ConnectionType>>()},

            //Typed column - view first match
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(bool) }, new ColumnFactory<ViewColumnFirstMatch<bool>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(double) }, new ColumnFactory<ViewColumnFirstMatch<double>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(float) }, new ColumnFactory<ViewColumnFirstMatch<float>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(int) }, new ColumnFactory<ViewColumnFirstMatch<int>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(long) }, new ColumnFactory<ViewColumnFirstMatch<long>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(short) }, new ColumnFactory<ViewColumnFirstMatch<short>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(string) }, new ColumnFactory<ViewColumnFirstMatch<string>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(uint) }, new ColumnFactory<ViewColumnFirstMatch<uint>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(ulong) }, new ColumnFactory<ViewColumnFirstMatch<ulong>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(ushort) }, new ColumnFactory<ViewColumnFirstMatch<ushort>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ViewColumnFirstMatch<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ViewColumnFirstMatch<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ViewColumnFirstMatch<ManagedConnection.ConnectionType>>()},

            //ConstMatcher string to - X
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(bool) }, new ConstMatcherFactory<bool>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(double) }, new ConstMatcherFactory<double>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(float) }, new ConstMatcherFactory<float>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(int) }, new ConstMatcherFactory<int>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(long) }, new ConstMatcherFactory<long>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(short) }, new ConstMatcherFactory<short>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(uint) }, new ConstMatcherFactory<uint>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(ulong) }, new ConstMatcherFactory<ulong>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(ushort) }, new ConstMatcherFactory<ushort>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(DiffTable.DiffResult) }, new ConstMatcherFactory<DiffTable.DiffResult>()},
            { new TypePair { Key = typeof(ConstMatcher<>), Value = typeof(ManagedConnection.ConnectionType) }, new ConstMatcherFactory<ManagedConnection.ConnectionType>()},

            //Const column
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(bool) }, new ColumnFactory<ViewColumnConst<bool>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(double) }, new ColumnFactory<ViewColumnConst<double>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(float) }, new ColumnFactory<ViewColumnConst<float>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(int) }, new ColumnFactory<ViewColumnConst<int>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(long) }, new ColumnFactory<ViewColumnConst<long>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(short) }, new ColumnFactory<ViewColumnConst<short>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(string) }, new ColumnFactory<ViewColumnConst<string>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(uint) }, new ColumnFactory<ViewColumnConst<uint>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(ulong) }, new ColumnFactory<ViewColumnConst<ulong>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(ushort) }, new ColumnFactory<ViewColumnConst<ushort>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(DiffTable.DiffResult) }, new ColumnFactory<ViewColumnConst<DiffTable.DiffResult>>()},
            { new TypePair { Key = typeof(ViewColumnConst<>), Value = typeof(ManagedConnection.ConnectionType) }, new ColumnFactory<ViewColumnConst<ManagedConnection.ConnectionType>>()},
        };

        private static IColumnFactory GetFactory(Type columnType, Type type)
        {
            IColumnFactory factory;
            if (!kFactories.TryGetValue(new TypePair { Key = columnType, Value = type }, out factory))
            {
                throw new UnityException("Column / Value type combination is invalid. Provided column type: " +
                    columnType.Name + "can not consume data type: " + type == null ? "null" : type.Name);
            }
            return factory;
        }

        private static ExpressionFactory GetExpressionFactory(Type type)
        {
            IColumnFactory factory;
            if (!kExpressionFactory.TryGetValue(type, out factory))
            {
                throw new UnityException("Unsupported expression value type. Provided type: " +
                    type == null ? "null" : type.Name);
            }
            return (ExpressionFactory)factory;
        }

        private static ViewColumnExpressionFactory GetViewColumnExpressionFactory(Type type)
        {
            ViewColumnExpressionFactory factory;
            if (!kViewColumnExpressionTypeFactory.TryGetValue(type, out factory))
            {
                throw new UnityException("Unsupported column value type. Provided type: " +
                    type == null ? "null" : type.Name);
            }
            return factory;
        }

        public static Column CreateColumn(Type columnType, Type type)
        {
            return ((TypedColumnFactory)GetFactory(columnType, type)).CreateTypedColumn();
        }

        public static Matcher CreateConstMatcher(Type type, string matchString)
        {
            return ((MatcherFactory)GetFactory(typeof(ConstMatcher<>), type)).CreateTypedMatcher(matchString);
        }

        public static Column CreateViewColumnExpression(Expression expression)
        {
            return GetViewColumnExpressionFactory(expression.type).CreateViewColumnExpression(expression);
        }

        public static Expression CreateTypedExpressionColumn(Type type, Column source)
        {
            return GetExpressionFactory(type).CreateTypedExpressionColumn(source);
        }

        public static Expression CreateTypedExpressionFixedRow(Expression baseExp, long row)
        {
            return GetExpressionFactory(baseExp.type).CreateTypedExpressionFixedRow(baseExp, row);
        }

        public static Expression CreateTypedExpressionConst(Type type, string aConstValue)
        {
            return GetExpressionFactory(type).CreateTypedExpressionConst(aConstValue);
        }

        public static Expression CreateTypedExpressionTypeChange(Type type, Expression sourceExp)
        {
            return GetExpressionFactory(type).CreateTypedExpressionTypeChange(sourceExp);
        }

        public static Expression CreateTypedExpressionSelectSetConditional(Type type, SelectSet selectSet, Expression subExpression)
        {
            return GetExpressionFactory(type).CreateTypedExpressionSelectSetConditional(selectSet, subExpression);
        }

        public static Expression CreateTypedExpressionSelect(Type type, Select selection, Column source)
        {
            return GetExpressionFactory(type).CreateTypedExpressionSelect(selection, source);
        }

        public static Expression CreateTypedExpressionSelectFirstMatch(Type type, Select selection, Column source)
        {
            return GetExpressionFactory(type).CreateTypedExpressionSelectFirstMatch(selection, source);
        }

        public static Expression CreateTypedExpressionColumnMerge(Type type, View.ViewTable parentViewTable, long parentGroupIndex, Column parentColumn, MetaColumn metaColumnToMerge)
        {
            return GetExpressionFactory(type).CreateTypedExpressionColumnMerge(parentViewTable, parentGroupIndex, parentColumn, metaColumnToMerge);
        }

        public static Expression CreateTypedExpressionDefaultOnError(Expression baseExp, IComparable defaultValue)
        {
            return GetExpressionFactory(baseExp.type).CreateTypedExpressionDefaultOnError(baseExp, defaultValue);
        }

        public static Expression CreateTypedExpressionDataBreakPoint(Expression baseExp, Expression value)
        {
            return GetExpressionFactory(baseExp.type).CreateTypedExpressionDataBreakPoint(baseExp, value);
        }
    }
}
