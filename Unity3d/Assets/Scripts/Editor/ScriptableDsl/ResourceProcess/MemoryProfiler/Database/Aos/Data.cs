using System;
using System.Collections.Generic;
using Unity.MemoryProfilerForExtension.Editor.Containers;

namespace Unity.MemoryProfilerForExtension.Editor.Database.Aos
{
    /// <summary>
    /// Provides a way to create columns using an `Array of Struct` data structure as input data
    /// </summary>
    internal class Data
    {
        public static Column<StructT, DataT> MakeColumn<StructT, DataT>(StructT[] array, Column<StructT, DataT>.Getter getter, Column<StructT, DataT>.Setter setter) where DataT : IComparable, new()
        {
            return new Column<StructT, DataT>(array, getter, setter);
        }

        public static Column<StructT, DataT> MakeColumn<StructT, DataT>(StructT[] array, Column<StructT, DataT>.Getter getter) where DataT : IComparable, new()
        {
            return new Column<StructT, DataT>(array, getter, (ref StructT o, DataT v) => { throw new Exception("Cannot set value on this column"); });
        }

        internal class Column<StructT, DataT> : Database.ColumnTyped<DataT> where DataT : System.IComparable, new()
        {
            public delegate void Setter(ref StructT s, DataT v);
            public delegate DataT Getter(StructT s);
            Setter setter;
            Getter getter;
            StructT[] array;
            public Column(StructT[] array, Getter getter, Setter setter)
            {
                type = typeof(DataT);
                this.array = array;
                this.getter = getter;
                this.setter = setter;
            }

            public override long GetRowCount()
            {
                return array.Length;
            }

            public override string GetRowValueString(long row, IDataFormatter formatter)
            {
                return formatter.Format(getter(array[row]));
            }

            public override DataT GetRowValue(long row)
            {
                return getter(array[row]);
            }

            public override void SetRowValue(long row, DataT value)
            {
                setter(ref array[row], value);
            }

            public override System.Collections.Generic.IEnumerable<DataT> VisitRows(ArrayRange indices)
            {
                for (long i = 0; i != indices.Count; ++i)
                {
                    yield return getter(array[indices[i]]);
                }
            }
        }

        public static ColumnList<StructT, DataT> MakeColumn<StructT, DataT>(BlockList<StructT> list, ColumnList<StructT, DataT>.Getter getter, ColumnList<StructT, DataT>.Setter setter) where DataT : IComparable, new()
        {
            return new ColumnList<StructT, DataT>(list, getter, setter);
        }

        public static ColumnList<StructT, DataT> MakeColumn<StructT, DataT>(BlockList<StructT> list, ColumnList<StructT, DataT>.Getter getter) where DataT : IComparable, new()
        {
            return new ColumnList<StructT, DataT>(list, getter, (BlockList<StructT> l, int index, DataT v) => { throw new Exception("Cannot set value on this column"); });
        }

        internal class ColumnList<StructT, DataT> : Database.ColumnTyped<DataT> where DataT : System.IComparable, new()
        {
            public delegate void Setter(BlockList<StructT> list, int index, DataT v);
            public delegate DataT Getter(StructT s);
            Setter setter;
            Getter getter;
            BlockList<StructT> list;
            public ColumnList(BlockList<StructT> list, Getter getter, Setter setter)
            {
                type = typeof(DataT);
                this.list = list;
                this.getter = getter;
                this.setter = setter;
            }

            public override long GetRowCount()
            {
                return list.Count;
            }

            public override string GetRowValueString(long row, IDataFormatter formatter)
            {
                return formatter.Format(getter(list[(int)row]));
            }

            public override DataT GetRowValue(long row)
            {
                return getter(list[(int)row]);
            }

            public override void SetRowValue(long row, DataT value)
            {
                setter(list, (int)row, value);
            }

            public override System.Collections.Generic.IEnumerable<DataT> VisitRows(ArrayRange indices)
            {
                for (long i = 0; i != indices.Count; ++i)
                {
                    yield return getter(list[(int)indices[i]]);
                }
            }
        }
    }
}
