using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ScriptRuntime;

namespace ScriptableFramework
{
    using TupleValue1 = Tuple<BoxedValue>;
    using TupleValue2 = Tuple<BoxedValue, BoxedValue>;
    using TupleValue3 = Tuple<BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue4 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue5 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue6 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue7 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue8 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, Tuple<BoxedValue>>;

    public class BoolList : List<bool>
    {
        public BoolList() { }
        public BoolList(int capacity) : base(capacity) { }
        public BoolList(IEnumerable<bool> coll) : base(coll) { }
    }
    public class CharList : List<char>
    {
        public CharList() { }
        public CharList(int capacity) : base(capacity) { }
        public CharList(IEnumerable<char> coll) : base(coll) { }
    }
    public class ByteList : List<byte>
    {
        public ByteList() { }
        public ByteList(int capacity) : base(capacity) { }
        public ByteList(IEnumerable<byte> coll) : base(coll) { }
    }
    public class SbyteList : List<sbyte>
    {
        public SbyteList() { }
        public SbyteList(int capacity) : base(capacity) { }
        public SbyteList(IEnumerable<sbyte> coll) : base(coll) { }
    }
    public class ShortList : List<short>
    {
        public ShortList() { }
        public ShortList(int capacity) : base(capacity) { }
        public ShortList(IEnumerable<short> coll) : base(coll) { }
    }
    public class UshortList : List<ushort>
    {
        public UshortList() { }
        public UshortList(int capacity) : base(capacity) { }
        public UshortList(IEnumerable<ushort> coll) : base(coll) { }
    }
    public class UintList : List<uint>
    {
        public UintList() { }
        public UintList(int capacity) : base(capacity) { }
        public UintList(IEnumerable<uint> coll) : base(coll) { }
    }
    public class LongList : List<long>
    {
        public LongList() { }
        public LongList(int capacity) : base(capacity) { }
        public LongList(IEnumerable<long> coll) : base(coll) { }
    }
    public class UlongList : List<ulong>
    {
        public UlongList() { }
        public UlongList(int capacity) : base(capacity) { }
        public UlongList(IEnumerable<ulong> coll) : base(coll) { }
    }
    public class DoubleList : List<double>
    {
        public DoubleList() { }
        public DoubleList(int capacity) : base(capacity) { }
        public DoubleList(IEnumerable<double> coll) : base(coll) { }
    }

    public class IntList : List<int>
    {
        public IntList() { }
        public IntList(int capacity) : base(capacity) { }
        public IntList(IEnumerable<int> coll) : base(coll) { }
    }
    public class FloatList : List<float>
    {
        public FloatList() { }
        public FloatList(int capacity) : base(capacity) { }
        public FloatList(IEnumerable<float> coll) : base(coll) { }
    }
    public class StrList : List<string>
    {
        public StrList() { }
        public StrList(int capacity) : base(capacity) { }
        public StrList(IEnumerable<string> coll) : base(coll) { }
    }
    public class ObjList : List<object>
    {
        public ObjList() { }
        public ObjList(int capacity) : base(capacity) { }
        public ObjList(IEnumerable<object> coll) : base(coll) { }
    }
    public class IntIntDict : Dictionary<int, int>
    {
        public IntIntDict() { }
        public IntIntDict(int capacity) : base(capacity) { }
        public IntIntDict(IDictionary<int, int> dict) : base(dict) { }
    }
    public class IntFloatDict : Dictionary<int, float>
    {
        public IntFloatDict() { }
        public IntFloatDict(int capacity) : base(capacity) { }
        public IntFloatDict(IDictionary<int, float> dict) : base(dict) { }
    }
    public class IntStrDict : Dictionary<int, string>
    {
        public IntStrDict() { }
        public IntStrDict(int capacity) : base(capacity) { }
        public IntStrDict(IDictionary<int, string> dict) : base(dict) { }
    }
    public class IntObjDict : Dictionary<int, object>
    {
        public IntObjDict() { }
        public IntObjDict(int capacity) : base(capacity) { }
        public IntObjDict(IDictionary<int, object> dict) : base(dict) { }
    }
    public class StrIntDict : Dictionary<string, int>
    {
        public StrIntDict() { }
        public StrIntDict(int capacity) : base(capacity) { }
        public StrIntDict(IDictionary<string, int> dict) : base(dict) { }
    }
    public class StrFloatDict : Dictionary<string, float>
    {
        public StrFloatDict() { }
        public StrFloatDict(int capacity) : base(capacity) { }
        public StrFloatDict(IDictionary<string, float> dict) : base(dict) { }
    }
    public class StrStrDict : Dictionary<string, string>
    {
        public StrStrDict() { }
        public StrStrDict(int capacity) : base(capacity) { }
        public StrStrDict(IDictionary<string, string> dict) : base(dict) { }
    }
    public class StrObjDict : Dictionary<string, object>
    {
        public StrObjDict() { }
        public StrObjDict(int capacity) : base(capacity) { }
        public StrObjDict(IDictionary<string, object> dict) : base(dict) { }
    }
    public class ObjIntDict : Dictionary<object, int>
    {
        public ObjIntDict() { }
        public ObjIntDict(int capacity) : base(capacity) { }
        public ObjIntDict(IDictionary<object, int> dict) : base(dict) { }
    }
    public class ObjFloatDict : Dictionary<object, float>
    {
        public ObjFloatDict() { }
        public ObjFloatDict(int capacity) : base(capacity) { }
        public ObjFloatDict(IDictionary<object, float> dict) : base(dict) { }
    }
    public class ObjStrDict : Dictionary<object, string>
    {
        public ObjStrDict() { }
        public ObjStrDict(int capacity) : base(capacity) { }
        public ObjStrDict(IDictionary<object, string> dict) : base(dict) { }
    }
    public class ObjObjDict : Dictionary<object, object>
    {
        public ObjObjDict() { }
        public ObjObjDict(int capacity) : base(capacity) { }
        public ObjObjDict(IDictionary<object, object> dict) : base(dict) { }
    }
    public class IntHashSet : HashSet<int>
    {
        public IntHashSet() { }
        public IntHashSet(int capacity) : base(capacity) { }
        public IntHashSet(IEnumerable<int> coll) : base(coll) { }
    }
    public class StrHashSet : HashSet<string>
    {
        public StrHashSet() { }
        public StrHashSet(int capacity) : base(capacity) { }
        public StrHashSet(IEnumerable<string> coll) : base(coll) { }
    }
    public class ObjHashSet : HashSet<object>
    {
        public ObjHashSet() { }
        public ObjHashSet(int capacity) : base(capacity) { }
        public ObjHashSet(IEnumerable<object> coll) : base(coll) { }
    }
    public class IntQueue : Queue<int>
    {
        public IntQueue() { }
        public IntQueue(int capacity) : base(capacity) { }
        public IntQueue(IEnumerable<int> coll) : base(coll) { }
    }
    public class ObjQueue : Queue<object>
    {
        public ObjQueue() { }
        public ObjQueue(int capacity) : base(capacity) { }
        public ObjQueue(IEnumerable<object> coll) : base(coll) { }
    }
    public class StrQueue : Queue<string>
    {
        public StrQueue() { }
        public StrQueue(int capacity) : base(capacity) { }
        public StrQueue(IEnumerable<string> coll) : base(coll) { }
    }
    public class IntStack : Stack<int>
    {
        public IntStack() { }
        public IntStack(int capacity) : base(capacity) { }
        public IntStack(IEnumerable<int> coll) : base(coll) { }
    }
    public class ObjStack : Stack<object>
    {
        public ObjStack() { }
        public ObjStack(int capacity) : base(capacity) { }
        public ObjStack(IEnumerable<object> coll) : base(coll) { }
    }
    public class StrStack : Stack<string>
    {
        public StrStack() { }
        public StrStack(int capacity) : base(capacity) { }
        public StrStack(IEnumerable<string> coll) : base(coll) { }
    }
    public class IntObjSortedDict : SortedDictionary<int, object>
    {
        public IntObjSortedDict() { }
        public IntObjSortedDict(int capacity) { }
        public IntObjSortedDict(IDictionary<int, object> dict) : base(dict) { }
    }
    public class Vector2List : List<Vector2>
    {
        public Vector2List() { }
        public Vector2List(int capacity) : base(capacity) { }
        public Vector2List(ICollection<Vector2> coll) : base(coll) { }
    }
    public class Vector3List : List<Vector3>
    {
        public Vector3List() { }
        public Vector3List(int capacity) : base(capacity) { }
        public Vector3List(ICollection<Vector3> coll) : base(coll) { }
    }

    public class Vector2Obj
    {
        public Vector2 Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Vector2Obj(Vector2 v) { var o = new Vector2Obj(); o.Value = v; return o; }
        public static implicit operator Vector2(Vector2Obj v) { return v.Value; }
    }
    public class Vector3Obj
    {
        public Vector3 Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Vector3Obj(Vector3 v) { var o = new Vector3Obj(); o.Value = v; return o; }
        public static implicit operator Vector3(Vector3Obj v) { return v.Value; }
    }
    public class Vector4Obj
    {
        public Vector4 Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Vector4Obj(Vector4 v) { var o = new Vector4Obj(); o.Value = v; return o; }
        public static implicit operator Vector4(Vector4Obj v) { return v.Value; }
    }
    public class QuaternionObj
    {
        public Quaternion Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator QuaternionObj(Quaternion v) { var o = new QuaternionObj(); o.Value = v; return o; }
        public static implicit operator Quaternion(QuaternionObj v) { return v.Value; }
    }
    public class ColorObj
    {
        public ColorF Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator ColorObj(ColorF v) { var o = new ColorObj(); o.Value = v; return o; }
        public static implicit operator ColorF(ColorObj v) { return v.Value; }
    }
    public class Color32Obj
    {
        public Color32 Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Color32Obj(Color32 v) { var o = new Color32Obj(); o.Value = v; return o; }
        public static implicit operator Color32(Color32Obj v) { return v.Value; }
    }
    public class Vector2IntObj
    {
        public Vector2Int Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Vector2IntObj(Vector2Int v) { var o = new Vector2IntObj(); o.Value = v; return o; }
        public static implicit operator Vector2Int(Vector2IntObj v) { return v.Value; }
    }
    public class Vector3IntObj
    {
        public Vector3Int Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Vector3IntObj(Vector3Int v) { var o = new Vector3IntObj(); o.Value = v; return o; }
        public static implicit operator Vector3Int(Vector3IntObj v) { return v.Value; }
    }
    public class Matrix4x4Obj
    {
        public Matrix44 Value;

        public override string ToString() { return Value.ToString(); }
        public static implicit operator Matrix4x4Obj(Matrix44 v) { var o = new Matrix4x4Obj(); o.Value = v; return o; }
        public static implicit operator Matrix44(Matrix4x4Obj v) { return v.Value; }
    }

    public static class GenericValueConverter
    {
        public static BoxedValue ToBoxedValue<T>(T v)
        {
            var from = s_FromBoxedValue as FromGenericDelegation<BoxedValue, T>;
            if (null != from)
                return from(v);
            return BoxedValue.NullObject;
        }
        public static object ToObject<T>(T v)
        {
            var from = s_FromObject as FromGenericDelegation<object, T>;
            if (null != from)
                return from(v);
            return null;
        }
        public static string ToString<T>(T v)
        {
            var from = s_FromString as FromGenericDelegation<string, T>;
            if (null != from)
                return from(v);
            return null;
        }
        public static bool ToBool<T>(T v)
        {
            var from = s_FromBool as FromGenericDelegation<bool, T>;
            if (null != from)
                return from(v);
            return false;
        }
        public static char ToChar<T>(T v)
        {
            var from = s_FromChar as FromGenericDelegation<char, T>;
            if (null != from)
                return from(v);
            return (char)0;
        }
        public static sbyte ToSByte<T>(T v)
        {
            var from = s_FromSByte as FromGenericDelegation<sbyte, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static short ToShort<T>(T v)
        {
            var from = s_FromShort as FromGenericDelegation<short, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static int ToInt<T>(T v)
        {
            var from = s_FromInt as FromGenericDelegation<int, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static long ToLong<T>(T v)
        {
            var from = s_FromLong as FromGenericDelegation<long, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static byte ToByte<T>(T v)
        {
            var from = s_FromByte as FromGenericDelegation<byte, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static ushort ToUShort<T>(T v)
        {
            var from = s_FromUShort as FromGenericDelegation<ushort, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static uint ToUInt<T>(T v)
        {
            var from = s_FromUInt as FromGenericDelegation<uint, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static ulong ToULong<T>(T v)
        {
            var from = s_FromULong as FromGenericDelegation<ulong, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static float ToFloat<T>(T v)
        {
            var from = s_FromFloat as FromGenericDelegation<float, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static double ToDouble<T>(T v)
        {
            var from = s_FromDouble as FromGenericDelegation<double, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static decimal ToDecimal<T>(T v)
        {
            var from = s_FromDecimal as FromGenericDelegation<decimal, T>;
            if (null != from)
                return from(v);
            return 0;
        }
        public static TupleValue1 ToTuple1<T>(T v)
        {
            var from = s_FromTuple1 as FromGenericDelegation<TupleValue1, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject);
        }
        public static TupleValue2 ToTuple2<T>(T v)
        {
            var from = s_FromTuple2 as FromGenericDelegation<TupleValue2, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue3 ToTuple3<T>(T v)
        {
            var from = s_FromTuple3 as FromGenericDelegation<TupleValue3, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue4 ToTuple4<T>(T v)
        {
            var from = s_FromTuple4 as FromGenericDelegation<TupleValue4, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue5 ToTuple5<T>(T v)
        {
            var from = s_FromTuple5 as FromGenericDelegation<TupleValue5, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue6 ToTuple6<T>(T v)
        {
            var from = s_FromTuple6 as FromGenericDelegation<TupleValue6, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue7 ToTuple7<T>(T v)
        {
            var from = s_FromTuple7 as FromGenericDelegation<TupleValue7, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static TupleValue8 ToTuple8<T>(T v)
        {
            var from = s_FromTuple8 as FromGenericDelegation<TupleValue8, T>;
            if (null != from)
                return from(v);
            return Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
        }
        public static Vector2 ToVector2<T>(T v)
        {
            var from = s_FromVector2 as FromGenericDelegation<Vector2, T>;
            if (null != from)
                return from(v);
            return Vector2.Zero;
        }
        public static Vector3 ToVector3<T>(T v)
        {
            var from = s_FromVector3 as FromGenericDelegation<Vector3, T>;
            if (null != from)
                return from(v);
            return Vector3.Zero;
        }
        public static Vector4 ToVector4<T>(T v)
        {
            var from = s_FromVector4 as FromGenericDelegation<Vector4, T>;
            if (null != from)
                return from(v);
            return Vector4.Zero;
        }
        public static Quaternion ToQuaternion<T>(T v)
        {
            var from = s_FromQuaternion as FromGenericDelegation<Quaternion, T>;
            if (null != from)
                return from(v);
            return Quaternion.Identity;
        }
        public static ColorF ToColor<T>(T v)
        {
            var from = s_FromColor as FromGenericDelegation<ColorF, T>;
            if (null != from)
                return from(v);
            return Color32.White.ToColorF();
        }
        public static Color32 ToColor32<T>(T v)
        {
            var from = s_FromColor32 as FromGenericDelegation<Color32, T>;
            if (null != from)
                return from(v);
            return Color32.White;
        }
        public static Vector2Int ToVector2Int<T>(T v)
        {
            var from = s_FromVector2Int as FromGenericDelegation<Vector2Int, T>;
            if (null != from)
                return from(v);
            return Vector2Int.zero;
        }
        public static Vector3Int ToVector3Int<T>(T v)
        {
            var from = s_FromVector3Int as FromGenericDelegation<Vector3Int, T>;
            if (null != from)
                return from(v);
            return Vector3Int.zero;
        }
        public static Matrix44 ToMatrix4x4<T>(T v)
        {
            var from = s_FromMatrix4x4 as FromGenericDelegation<Matrix44, T>;
            if (null != from)
                return from(v);
            return Matrix44.Identity;
        }

        public static T From<T>(BoxedValue v)
        {
            var from = s_FromBoxedValue as FromGenericDelegation<T, BoxedValue>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(object v)
        {
            var from = s_FromObject as FromGenericDelegation<T, object>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(string v)
        {
            var from = s_FromString as FromGenericDelegation<T, string>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(bool v)
        {
            var from = s_FromBool as FromGenericDelegation<T, bool>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(char v)
        {
            var from = s_FromChar as FromGenericDelegation<T, char>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(sbyte v)
        {
            var from = s_FromSByte as FromGenericDelegation<T, sbyte>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(short v)
        {
            var from = s_FromShort as FromGenericDelegation<T, short>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(int v)
        {
            var from = s_FromInt as FromGenericDelegation<T, int>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(long v)
        {
            var from = s_FromLong as FromGenericDelegation<T, long>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(byte v)
        {
            var from = s_FromByte as FromGenericDelegation<T, byte>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(ushort v)
        {
            var from = s_FromUShort as FromGenericDelegation<T, ushort>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(uint v)
        {
            var from = s_FromUInt as FromGenericDelegation<T, uint>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(ulong v)
        {
            var from = s_FromULong as FromGenericDelegation<T, ulong>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(float v)
        {
            var from = s_FromFloat as FromGenericDelegation<T, float>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(double v)
        {
            var from = s_FromDouble as FromGenericDelegation<T, double>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(decimal v)
        {
            var from = s_FromDecimal as FromGenericDelegation<T, decimal>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue1 v)
        {
            var from = s_FromTuple1 as FromGenericDelegation<T, TupleValue1>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue2 v)
        {
            var from = s_FromTuple2 as FromGenericDelegation<T, TupleValue2>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue3 v)
        {
            var from = s_FromTuple3 as FromGenericDelegation<T, TupleValue3>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue4 v)
        {
            var from = s_FromTuple4 as FromGenericDelegation<T, TupleValue4>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue5 v)
        {
            var from = s_FromTuple5 as FromGenericDelegation<T, TupleValue5>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue6 v)
        {
            var from = s_FromTuple6 as FromGenericDelegation<T, TupleValue6>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue7 v)
        {
            var from = s_FromTuple7 as FromGenericDelegation<T, TupleValue7>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(TupleValue8 v)
        {
            var from = s_FromTuple8 as FromGenericDelegation<T, TupleValue8>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Vector2 v)
        {
            var from = s_FromVector2 as FromGenericDelegation<T, Vector2>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Vector3 v)
        {
            var from = s_FromVector3 as FromGenericDelegation<T, Vector3>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Vector4 v)
        {
            var from = s_FromVector4 as FromGenericDelegation<T, Vector4>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Quaternion v)
        {
            var from = s_FromQuaternion as FromGenericDelegation<T, Quaternion>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(ColorF v)
        {
            var from = s_FromColor as FromGenericDelegation<T, ColorF>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Color32 v)
        {
            var from = s_FromColor32 as FromGenericDelegation<T, Color32>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Vector2Int v)
        {
            var from = s_FromVector2Int as FromGenericDelegation<T, Vector2Int>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Vector3Int v)
        {
            var from = s_FromVector3Int as FromGenericDelegation<T, Vector3Int>;
            if (null != from)
                return from(v);
            return default(T);
        }
        public static T From<T>(Matrix44 v)
        {
            var from = s_FromMatrix4x4 as FromGenericDelegation<T, Matrix44>;
            if (null != from)
                return from(v);
            return default(T);
        }

        public static BoxedValue ToBoxedValue(Type t, object o)
        {
            return CastTo<BoxedValue>(o);
        }
        public static bool ToBool(Type t, object o)
        {
            return CastTo<bool>(o);
        }
        public static char ToChar(Type t, object o)
        {
            return CastTo<char>(o);
        }
        public static sbyte ToSByte(Type t, object o)
        {
            return CastTo<sbyte>(o);
        }
        public static short ToShort(Type t, object o)
        {
            return CastTo<short>(o);
        }
        public static int ToInt(Type t, object o)
        {
            return CastTo<int>(o);
        }
        public static long ToLong(Type t, object o)
        {
            return CastTo<long>(o);
        }
        public static byte ToByte(Type t, object o)
        {
            return CastTo<byte>(o);
        }
        public static ushort ToUShort(Type t, object o)
        {
            return CastTo<ushort>(o);
        }
        public static uint ToUInt(Type t, object o)
        {
            return CastTo<uint>(o);
        }
        public static ulong ToULong(Type t, object o)
        {
            return CastTo<ulong>(o);
        }
        public static float ToFloat(Type t, object o)
        {
            return CastTo<float>(o);
        }
        public static double ToDouble(Type t, object o)
        {
            return CastTo<double>(o);
        }
        public static decimal ToDecimal(Type t, object o)
        {
            return CastTo<decimal>(o);
        }
        public static string ToString(Type t, object o)
        {
            return CastTo<string>(o);
        }
        public static object ToObject(Type t, object o)
        {
            return o;
        }
        public static TupleValue1 ToTuple1(Type t, object o)
        {
            return CastTo<TupleValue1>(o);
        }
        public static TupleValue2 ToTuple2(Type t, object o)
        {
            return CastTo<TupleValue2>(o);
        }
        public static TupleValue3 ToTuple3(Type t, object o)
        {
            return CastTo<TupleValue3>(o);
        }
        public static TupleValue4 ToTuple4(Type t, object o)
        {
            return CastTo<TupleValue4>(o);
        }
        public static TupleValue5 ToTuple5(Type t, object o)
        {
            return CastTo<TupleValue5>(o);
        }
        public static TupleValue6 ToTuple6(Type t, object o)
        {
            return CastTo<TupleValue6>(o);
        }
        public static TupleValue7 ToTuple7(Type t, object o)
        {
            return CastTo<TupleValue7>(o);
        }
        public static TupleValue8 ToTuple8(Type t, object o)
        {
            return CastTo<TupleValue8>(o);
        }
        public static Vector2 ToVector2(Type t, object o)
        {
            return CastTo<Vector2>(o);
        }
        public static Vector3 ToVector3(Type t, object o)
        {
            return CastTo<Vector3>(o);
        }
        public static Vector4 ToVector4(Type t, object o)
        {
            return CastTo<Vector4>(o);
        }
        public static Quaternion ToQuaternion(Type t, object o)
        {
            return CastTo<Quaternion>(o);
        }
        public static ColorF ToColor(Type t, object o)
        {
            return CastTo<ColorF>(o);
        }
        public static Color32 ToColor32(Type t, object o)
        {
            return CastTo<Color32>(o);
        }
        public static Vector2Int ToVector2Int(Type t, object o)
        {
            return CastTo<Vector2Int>(o);
        }
        public static Vector3Int ToVector3Int(Type t, object o)
        {
            return CastTo<Vector3Int>(o);
        }
        public static Matrix44 ToMatrix4x4(Type t, object o)
        {
            return CastTo<Matrix44>(o);
        }
        public static object From(Type t, object o)
        {
            return o;
        }

        internal static T CastTo<T>(object obj)
        {
            if (obj is T) {
                return (T)obj;
            }
            else {
                try {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                catch (OverflowException) {
                    return (T)Convert.ChangeType(obj.ToString(), typeof(T));
                }
                catch {
                    return default(T);
                }
            }
        }

        private delegate R FromGenericDelegation<R, T>(T v);
        private static FromGenericDelegation<BoxedValue, BoxedValue> s_FromBoxedValue = FromHelper<BoxedValue>;
        private static FromGenericDelegation<bool, bool> s_FromBool = FromHelper<bool>;
        private static FromGenericDelegation<char, char> s_FromChar = FromHelper<char>;
        private static FromGenericDelegation<sbyte, sbyte> s_FromSByte = FromHelper<sbyte>;
        private static FromGenericDelegation<short, short> s_FromShort = FromHelper<short>;
        private static FromGenericDelegation<int, int> s_FromInt = FromHelper<int>;
        private static FromGenericDelegation<long, long> s_FromLong = FromHelper<long>;
        private static FromGenericDelegation<byte, byte> s_FromByte = FromHelper<byte>;
        private static FromGenericDelegation<ushort, ushort> s_FromUShort = FromHelper<ushort>;
        private static FromGenericDelegation<uint, uint> s_FromUInt = FromHelper<uint>;
        private static FromGenericDelegation<ulong, ulong> s_FromULong = FromHelper<ulong>;
        private static FromGenericDelegation<float, float> s_FromFloat = FromHelper<float>;
        private static FromGenericDelegation<double, double> s_FromDouble = FromHelper<double>;
        private static FromGenericDelegation<decimal, decimal> s_FromDecimal = FromHelper<decimal>;
        private static FromGenericDelegation<string, string> s_FromString = FromHelper<string>;
        private static FromGenericDelegation<object, object> s_FromObject = FromHelper<object>;
        private static FromGenericDelegation<TupleValue1, TupleValue1> s_FromTuple1 = FromHelper<TupleValue1>;
        private static FromGenericDelegation<TupleValue2, TupleValue2> s_FromTuple2 = FromHelper<TupleValue2>;
        private static FromGenericDelegation<TupleValue3, TupleValue3> s_FromTuple3 = FromHelper<TupleValue3>;
        private static FromGenericDelegation<TupleValue4, TupleValue4> s_FromTuple4 = FromHelper<TupleValue4>;
        private static FromGenericDelegation<TupleValue5, TupleValue5> s_FromTuple5 = FromHelper<TupleValue5>;
        private static FromGenericDelegation<TupleValue6, TupleValue6> s_FromTuple6 = FromHelper<TupleValue6>;
        private static FromGenericDelegation<TupleValue7, TupleValue7> s_FromTuple7 = FromHelper<TupleValue7>;
        private static FromGenericDelegation<TupleValue8, TupleValue8> s_FromTuple8 = FromHelper<TupleValue8>;

        private static FromGenericDelegation<Vector2, Vector2> s_FromVector2 = FromHelper<Vector2>;
        private static FromGenericDelegation<Vector3, Vector3> s_FromVector3 = FromHelper<Vector3>;
        private static FromGenericDelegation<Vector4, Vector4> s_FromVector4 = FromHelper<Vector4>;
        private static FromGenericDelegation<Quaternion, Quaternion> s_FromQuaternion = FromHelper<Quaternion>;
        private static FromGenericDelegation<ColorF, ColorF> s_FromColor = FromHelper<ColorF>;
        private static FromGenericDelegation<Color32, Color32> s_FromColor32 = FromHelper<Color32>;
        private static FromGenericDelegation<Vector2Int, Vector2Int> s_FromVector2Int = FromHelper<Vector2Int>;
        private static FromGenericDelegation<Vector3Int, Vector3Int> s_FromVector3Int = FromHelper<Vector3Int>;
        private static FromGenericDelegation<Matrix44, Matrix44> s_FromMatrix4x4 = FromHelper<Matrix44>;

        private static T FromHelper<T>(T v)
        {
            return v;
        }
    }

    public struct BoxedValue : IEquatable<BoxedValue>
    {
        public const int c_ObjectType = 0;
        public const int c_StringType = 1;
        public const int c_BoolType = 2;
        public const int c_CharType = 3;
        public const int c_SByteType = 4;
        public const int c_ShortType = 5;
        public const int c_IntType = 6;
        public const int c_LongType = 7;
        public const int c_ByteType = 8;
        public const int c_UShortType = 9;
        public const int c_UIntType = 10;
        public const int c_ULongType = 11;
        public const int c_FloatType = 12;
        public const int c_DoubleType = 13;
        public const int c_DecimalType = 14;
        public const int c_Tuple1Type = 15;
        public const int c_Tuple2Type = 16;
        public const int c_Tuple3Type = 17;
        public const int c_Tuple4Type = 18;
        public const int c_Tuple5Type = 19;
        public const int c_Tuple6Type = 20;
        public const int c_Tuple7Type = 21;
        public const int c_Tuple8Type = 22;

        [StructLayout(LayoutKind.Explicit)]
        public struct UnionValue
        {
            [FieldOffset(0)]
            public bool BoolVal;
            [FieldOffset(0)]
            public char CharVal;
            [FieldOffset(0)]
            public sbyte SByteVal;
            [FieldOffset(0)]
            public short ShortVal;
            [FieldOffset(0)]
            public int IntVal;
            [FieldOffset(0)]
            public long LongVal;
            [FieldOffset(0)]
            public byte ByteVal;
            [FieldOffset(0)]
            public ushort UShortVal;
            [FieldOffset(0)]
            public uint UIntVal;
            [FieldOffset(0)]
            public ulong ULongVal;
            [FieldOffset(0)]
            public float FloatVal;
            [FieldOffset(0)]
            public double DoubleVal;
            [FieldOffset(0)]
            public decimal DecimalVal;
        }

        public TupleValue1 Tuple1Val
        {
            get { return ObjectVal as TupleValue1; }
            set { ObjectVal = value; }
        }
        public TupleValue2 Tuple2Val
        {
            get { return ObjectVal as TupleValue2; }
            set { ObjectVal = value; }
        }
        public TupleValue3 Tuple3Val
        {
            get { return ObjectVal as TupleValue3; }
            set { ObjectVal = value; }
        }
        public TupleValue4 Tuple4Val
        {
            get { return ObjectVal as TupleValue4; }
            set { ObjectVal = value; }
        }
        public TupleValue5 Tuple5Val
        {
            get { return ObjectVal as TupleValue5; }
            set { ObjectVal = value; }
        }
        public TupleValue6 Tuple6Val
        {
            get { return ObjectVal as TupleValue6; }
            set { ObjectVal = value; }
        }
        public TupleValue7 Tuple7Val
        {
            get { return ObjectVal as TupleValue7; }
            set { ObjectVal = value; }
        }
        public TupleValue8 Tuple8Val
        {
            get { return ObjectVal as TupleValue8; }
            set { ObjectVal = value; }
        }
        public string StringVal
        {
            get { return ObjectVal as string; }
            set { ObjectVal = value; }
        }
        public int Type;
        public object ObjectVal;
        public UnionValue Union;

        public static implicit operator BoxedValue(string v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator string(BoxedValue v)
        {
            return v.GetString();
        }
        public static implicit operator BoxedValue(bool v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator bool(BoxedValue v)
        {
            return v.GetBool();
        }
        public static implicit operator BoxedValue(char v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator char(BoxedValue v)
        {
            return v.GetChar();
        }
        public static implicit operator BoxedValue(sbyte v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator sbyte(BoxedValue v)
        {
            return v.GetSByte();
        }
        public static implicit operator BoxedValue(short v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator short(BoxedValue v)
        {
            return v.GetShort();
        }
        public static implicit operator BoxedValue(int v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator int(BoxedValue v)
        {
            return v.GetInt();
        }
        public static implicit operator BoxedValue(long v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator long(BoxedValue v)
        {
            return v.GetLong();
        }
        public static implicit operator BoxedValue(byte v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator byte(BoxedValue v)
        {
            return v.GetByte();
        }
        public static implicit operator BoxedValue(ushort v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator ushort(BoxedValue v)
        {
            return v.GetUShort();
        }
        public static implicit operator BoxedValue(uint v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator uint(BoxedValue v)
        {
            return v.GetUInt();
        }
        public static implicit operator BoxedValue(ulong v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator ulong(BoxedValue v)
        {
            return v.GetULong();
        }
        public static implicit operator BoxedValue(float v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator float(BoxedValue v)
        {
            return v.GetFloat();
        }
        public static implicit operator BoxedValue(double v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator double(BoxedValue v)
        {
            return v.GetDouble();
        }
        public static implicit operator BoxedValue(decimal v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator decimal(BoxedValue v)
        {
            return v.GetDecimal();
        }
        public static implicit operator BoxedValue(TupleValue1 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue1(BoxedValue v)
        {
            return v.GetTuple1();
        }
        public static implicit operator BoxedValue(TupleValue2 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue2(BoxedValue v)
        {
            return v.GetTuple2();
        }
        public static implicit operator BoxedValue(TupleValue3 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue3(BoxedValue v)
        {
            return v.GetTuple3();
        }
        public static implicit operator BoxedValue(TupleValue4 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue4(BoxedValue v)
        {
            return v.GetTuple4();
        }
        public static implicit operator BoxedValue(TupleValue5 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue5(BoxedValue v)
        {
            return v.GetTuple5();
        }
        public static implicit operator BoxedValue(TupleValue6 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue6(BoxedValue v)
        {
            return v.GetTuple6();
        }
        public static implicit operator BoxedValue(TupleValue7 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue7(BoxedValue v)
        {
            return v.GetTuple7();
        }
        public static implicit operator BoxedValue(TupleValue8 v)
        {
            return BoxedValue.From(v);
        }
        public static implicit operator TupleValue8(BoxedValue v)
        {
            return v.GetTuple8();
        }

        public static implicit operator BoxedValue(byte[] v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator byte[](BoxedValue v)
        {
            return v.ObjectVal as byte[];
        }
        public static implicit operator BoxedValue(object[] v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator object[](BoxedValue v)
        {
            return v.ObjectVal as object[];
        }
        public static implicit operator BoxedValue(Type v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Type(BoxedValue v)
        {
            return v.ObjectVal as Type;
        }
        public static implicit operator BoxedValue(ObjList v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator ObjList(BoxedValue v)
        {
            return v.ObjectVal as ObjList;
        }
        public static implicit operator BoxedValue(ArrayList v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator ArrayList(BoxedValue v)
        {
            return v.ObjectVal as ArrayList;
        }

        public static implicit operator BoxedValue(Vector2Obj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Vector2Obj(BoxedValue v)
        {
            return v.ObjectVal as Vector2Obj;
        }
        public static implicit operator BoxedValue(Vector3Obj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Vector3Obj(BoxedValue v)
        {
            return v.ObjectVal as Vector3Obj;
        }
        public static implicit operator BoxedValue(Vector4Obj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Vector4Obj(BoxedValue v)
        {
            return v.ObjectVal as Vector4Obj;
        }
        public static implicit operator BoxedValue(QuaternionObj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator QuaternionObj(BoxedValue v)
        {
            return v.ObjectVal as QuaternionObj;
        }
        public static implicit operator BoxedValue(ColorObj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator ColorObj(BoxedValue v)
        {
            return v.ObjectVal as ColorObj;
        }
        public static implicit operator BoxedValue(Color32Obj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Color32Obj(BoxedValue v)
        {
            return v.ObjectVal as Color32Obj;
        }
        public static implicit operator BoxedValue(Vector2IntObj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Vector2IntObj(BoxedValue v)
        {
            return v.ObjectVal as Vector2IntObj;
        }
        public static implicit operator BoxedValue(Vector3IntObj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Vector3IntObj(BoxedValue v)
        {
            return v.ObjectVal as Vector3IntObj;
        }
        public static implicit operator BoxedValue(Matrix4x4Obj v)
        {
            return BoxedValue.FromObject(v);
        }
        public static implicit operator Matrix4x4Obj(BoxedValue v)
        {
            return v.ObjectVal as Matrix4x4Obj;
        }

        public string GetTypeName()
        {
            switch (Type) {
                case c_ObjectType:
                    return "object";
                case c_StringType:
                    return "string";
                case c_BoolType:
                    return "bool";
                case c_CharType:
                    return "char";
                case c_SByteType:
                    return "sbyte";
                case c_ShortType:
                    return "short";
                case c_IntType:
                    return "int";
                case c_LongType:
                    return "long";
                case c_ByteType:
                    return "byte";
                case c_UShortType:
                    return "ushort";
                case c_UIntType:
                    return "uint";
                case c_ULongType:
                    return "ulong";
                case c_FloatType:
                    return "float";
                case c_DoubleType:
                    return "double";
                case c_DecimalType:
                    return "decimal";
                case c_Tuple1Type:
                    return "Tuple1";
                case c_Tuple2Type:
                    return "Tuple2";
                case c_Tuple3Type:
                    return "Tuple3";
                case c_Tuple4Type:
                    return "Tuple4";
                case c_Tuple5Type:
                    return "Tuple5";
                case c_Tuple6Type:
                    return "Tuple6";
                case c_Tuple7Type:
                    return "Tuple7";
                case c_Tuple8Type:
                    return "Tuple8";
                default:
                    return "Unknown";
            }
        }

        public bool IsNullObject
        {
            get { return Type == c_ObjectType && ObjectVal == null; }
        }
        public bool IsNullOrEmptyString
        {
            get { return Type == c_StringType && string.IsNullOrEmpty(StringVal); }
        }
        public bool IsObject
        {
            get {
                return Type == c_ObjectType;
            }
        }
        public bool IsString
        {
            get {
                return Type == c_StringType;
            }
        }
        public bool IsBoolean
        {
            get {
                return Type == c_BoolType;
            }
        }
        public bool IsChar
        {
            get {
                return Type == c_CharType;
            }
        }
        public bool IsSignedInteger
        {
            get {
                switch (Type) {
                    case c_SByteType:
                    case c_ShortType:
                    case c_IntType:
                    case c_LongType:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool IsUnsignedInteger
        {
            get {
                switch (Type) {
                    case c_ByteType:
                    case c_UShortType:
                    case c_UIntType:
                    case c_ULongType:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool IsInteger
        {
            get {
                switch (Type) {
                    case c_SByteType:
                    case c_ShortType:
                    case c_IntType:
                    case c_LongType:
                    case c_ByteType:
                    case c_UShortType:
                    case c_UIntType:
                    case c_ULongType:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool IsNumber
        {
            get {
                return Type == c_FloatType || Type == c_DoubleType || Type == c_DecimalType;
            }
        }
        public bool IsTuple
        {
            get {
                switch (Type) {
                    case c_Tuple1Type:
                    case c_Tuple2Type:
                    case c_Tuple3Type:
                    case c_Tuple4Type:
                    case c_Tuple5Type:
                    case c_Tuple6Type:
                    case c_Tuple7Type:
                    case c_Tuple8Type:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public string AsString
        {
            get {
                return IsString ? StringVal : (IsObject ? ObjectVal as string : null);
            }
        }
        public T As<T>() where T : class
        {
            return IsObject || IsString ? ObjectVal as T : null;
        }
        public object As(Type t)
        {
            if (null == ObjectVal) {
                return null;
            }
            else if (IsObject || IsString) {
                Type st = ObjectVal.GetType();
                if (t.IsAssignableFrom(st) || st.IsSubclassOf(t))
                    return ObjectVal;
                else
                    return null;
            }
            else {
                return null;
            }
        }

        public void SetNullObject()
        {
            Type = c_ObjectType;
            ObjectVal = null;
        }
        public void SetNullString()
        {
            Type = c_StringType;
            StringVal = null;
        }
        public void SetEmptyString()
        {
            Type = c_StringType;
            StringVal = string.Empty;
        }

        public void Set(bool v)
        {
            Type = c_BoolType;
            Union.BoolVal = v;
        }
        public void Set(char v)
        {
            Type = c_CharType;
            Union.CharVal = v;
        }
        public void Set(sbyte v)
        {
            Type = c_SByteType;
            Union.SByteVal = v;
        }
        public void Set(short v)
        {
            Type = c_ShortType;
            Union.ShortVal = v;
        }
        public void Set(int v)
        {
            Type = c_IntType;
            Union.IntVal = v;
        }
        public void Set(long v)
        {
            Type = c_LongType;
            Union.LongVal = v;
        }
        public void Set(byte v)
        {
            Type = c_ByteType;
            Union.ByteVal = v;
        }
        public void Set(ushort v)
        {
            Type = c_UShortType;
            Union.UShortVal = v;
        }
        public void Set(uint v)
        {
            Type = c_UIntType;
            Union.UIntVal = v;
        }
        public void Set(ulong v)
        {
            Type = c_ULongType;
            Union.ULongVal = v;
        }
        public void Set(float v)
        {
            Type = c_FloatType;
            Union.FloatVal = v;
        }
        public void Set(double v)
        {
            Type = c_DoubleType;
            Union.DoubleVal = v;
        }
        public void Set(decimal v)
        {
            Type = c_DecimalType;
            Union.DecimalVal = v;
        }
        public void Set(TupleValue1 v)
        {
            Type = c_Tuple1Type;
            Tuple1Val = v;
        }
        public void Set(TupleValue2 v)
        {
            Type = c_Tuple2Type;
            Tuple2Val = v;
        }
        public void Set(TupleValue3 v)
        {
            Type = c_Tuple3Type;
            Tuple3Val = v;
        }
        public void Set(TupleValue4 v)
        {
            Type = c_Tuple4Type;
            Tuple4Val = v;
        }
        public void Set(TupleValue5 v)
        {
            Type = c_Tuple5Type;
            Tuple5Val = v;
        }
        public void Set(TupleValue6 v)
        {
            Type = c_Tuple6Type;
            Tuple6Val = v;
        }
        public void Set(TupleValue7 v)
        {
            Type = c_Tuple7Type;
            Tuple7Val = v;
        }
        public void Set(TupleValue8 v)
        {
            Type = c_Tuple8Type;
            Tuple8Val = v;
        }
        public void Set(string v)
        {
            Type = c_StringType;
            StringVal = v;
        }
        public void SetObject(object val)
        {
            if (null == val) {
                SetWithObjectType(val);
                return;
            }
            Type t = val.GetType();
            if (t == typeof(string))
                Set((string)val);
            else if (t == typeof(bool))
                Set((bool)val);
            else if (t == typeof(char))
                Set((char)val);
            else if (t == typeof(sbyte))
                Set((sbyte)val);
            else if (t == typeof(short))
                Set((short)val);
            else if (t == typeof(int))
                Set((int)val);
            else if (t == typeof(long))
                Set((long)val);
            else if (t == typeof(byte))
                Set((byte)val);
            else if (t == typeof(ushort))
                Set((ushort)val);
            else if (t == typeof(uint))
                Set((uint)val);
            else if (t == typeof(ulong))
                Set((ulong)val);
            else if (t == typeof(float))
                Set((float)val);
            else if (t == typeof(double))
                Set((double)val);
            else if (t == typeof(decimal))
                Set((decimal)val);
            else if (t == typeof(TupleValue1))
                Set((TupleValue1)val);
            else if (t == typeof(TupleValue2))
                Set((TupleValue2)val);
            else if (t == typeof(TupleValue3))
                Set((TupleValue3)val);
            else if (t == typeof(TupleValue4))
                Set((TupleValue4)val);
            else if (t == typeof(TupleValue5))
                Set((TupleValue5)val);
            else if (t == typeof(TupleValue6))
                Set((TupleValue6)val);
            else if (t == typeof(TupleValue7))
                Set((TupleValue7)val);
            else if (t == typeof(TupleValue8))
                Set((TupleValue8)val);
            else if (t == typeof(BoxedValue))
                this = (BoxedValue)val;
            else
                SetWithObjectType(val);
        }
        public void SetWithObjectType(object val)
        {
            Type = c_ObjectType;
            ObjectVal = val;
        }

        public bool GetBool()
        {
            return ToBool();
        }
        public char GetChar()
        {
            return ToChar();
        }
        public sbyte GetSByte()
        {
            return ToSByte();
        }
        public short GetShort()
        {
            return ToShort();
        }
        public int GetInt()
        {
            return ToInt();
        }
        public long GetLong()
        {
            return ToLong();
        }
        public byte GetByte()
        {
            return ToByte();
        }
        public ushort GetUShort()
        {
            return ToUShort();
        }
        public uint GetUInt()
        {
            return ToUInt();
        }
        public ulong GetULong()
        {
            return ToULong();
        }
        public float GetFloat()
        {
            return ToFloat();
        }
        public double GetDouble()
        {
            return ToDouble();
        }
        public decimal GetDecimal()
        {
            return ToDecimal();
        }
        public TupleValue1 GetTuple1()
        {
            return ToTuple1();
        }
        public TupleValue2 GetTuple2()
        {
            return ToTuple2();
        }
        public TupleValue3 GetTuple3()
        {
            return ToTuple3();
        }
        public TupleValue4 GetTuple4()
        {
            return ToTuple4();
        }
        public TupleValue5 GetTuple5()
        {
            return ToTuple5();
        }
        public TupleValue6 GetTuple6()
        {
            return ToTuple6();
        }
        public TupleValue7 GetTuple7()
        {
            return ToTuple7();
        }
        public TupleValue8 GetTuple8()
        {
            return ToTuple8();
        }
        public string GetString()
        {
            return ToString();
        }
        public object GetObject()
        {
            return ToObject();
        }

        public T CastTo<T>()
        {
            Type t = typeof(T);
            if (t == typeof(string))
                return GenericValueConverter.From<T>(ToString());
            else if (t == typeof(bool))
                return GenericValueConverter.From<T>(ToBool());
            else if (t == typeof(char))
                return GenericValueConverter.From<T>(ToChar());
            else if (t == typeof(sbyte))
                return GenericValueConverter.From<T>(ToSByte());
            else if (t == typeof(short))
                return GenericValueConverter.From<T>(ToShort());
            else if (t == typeof(int))
                return GenericValueConverter.From<T>(ToInt());
            else if (t == typeof(long))
                return GenericValueConverter.From<T>(ToLong());
            else if (t == typeof(byte))
                return GenericValueConverter.From<T>(ToByte());
            else if (t == typeof(ushort))
                return GenericValueConverter.From<T>(ToUShort());
            else if (t == typeof(uint))
                return GenericValueConverter.From<T>(ToUInt());
            else if (t == typeof(ulong))
                return GenericValueConverter.From<T>(ToULong());
            else if (t == typeof(float))
                return GenericValueConverter.From<T>(ToFloat());
            else if (t == typeof(double))
                return GenericValueConverter.From<T>(ToDouble());
            else if (t == typeof(decimal))
                return GenericValueConverter.From<T>(ToDecimal());
            else if (t == typeof(TupleValue1))
                return GenericValueConverter.From<T>(ToTuple1());
            else if (t == typeof(TupleValue2))
                return GenericValueConverter.From<T>(ToTuple2());
            else if (t == typeof(TupleValue3))
                return GenericValueConverter.From<T>(ToTuple3());
            else if (t == typeof(TupleValue4))
                return GenericValueConverter.From<T>(ToTuple4());
            else if (t == typeof(TupleValue5))
                return GenericValueConverter.From<T>(ToTuple5());
            else if (t == typeof(TupleValue6))
                return GenericValueConverter.From<T>(ToTuple6());
            else if (t == typeof(TupleValue7))
                return GenericValueConverter.From<T>(ToTuple7());
            else if (t == typeof(TupleValue8))
                return GenericValueConverter.From<T>(ToTuple8());
            else if (t == typeof(BoxedValue))
                return GenericValueConverter.From<T>(this);
            else if (t == typeof(object))
                return GenericValueConverter.From<T>(ToObject());
            else if (IsObject && t == typeof(Vector2) && ObjectVal is Vector2Obj)
                return GenericValueConverter.From<T>((Vector2)(Vector2Obj)ObjectVal);
            else if (IsObject && t == typeof(Vector3) && ObjectVal is Vector3Obj)
                return GenericValueConverter.From<T>((Vector3)(Vector3Obj)ObjectVal);
            else if (IsObject && t == typeof(Vector4) && ObjectVal is Vector4Obj)
                return GenericValueConverter.From<T>((Vector4)(Vector4Obj)ObjectVal);
            else if (IsObject && t == typeof(Quaternion) && ObjectVal is QuaternionObj)
                return GenericValueConverter.From<T>((Quaternion)(QuaternionObj)ObjectVal);
            else if (IsObject && t == typeof(ColorF) && ObjectVal is ColorObj)
                return GenericValueConverter.From<T>((ColorF)(ColorObj)ObjectVal);
            else if (IsObject && t == typeof(Color32) && ObjectVal is Color32Obj)
                return GenericValueConverter.From<T>((Color32)(Color32Obj)ObjectVal);
            else if (IsObject && t == typeof(Vector2Int) && ObjectVal is Vector2IntObj)
                return GenericValueConverter.From<T>((Vector2Int)(Vector2IntObj)ObjectVal);
            else if (IsObject && t == typeof(Vector3Int) && ObjectVal is Vector3IntObj)
                return GenericValueConverter.From<T>((Vector3Int)(Vector3IntObj)ObjectVal);
            else if (IsObject && t == typeof(Matrix44) && ObjectVal is Matrix4x4Obj)
                return GenericValueConverter.From<T>((Matrix44)(Matrix4x4Obj)ObjectVal);
            else
                return GenericValueConverter.CastTo<T>(ToObject());
        }
        public object CastTo(Type t)
        {
            if (t == typeof(string))
                return ToString();
            else if (t == typeof(bool))
                return ToBool();
            else if (t == typeof(char))
                return ToChar();
            else if (t == typeof(sbyte))
                return ToSByte();
            else if (t == typeof(short))
                return ToShort();
            else if (t == typeof(int))
                return ToInt();
            else if (t == typeof(long))
                return ToLong();
            else if (t == typeof(byte))
                return ToByte();
            else if (t == typeof(ushort))
                return ToUShort();
            else if (t == typeof(uint))
                return ToUInt();
            else if (t == typeof(ulong))
                return ToULong();
            else if (t == typeof(float))
                return ToFloat();
            else if (t == typeof(double))
                return ToDouble();
            else if (t == typeof(decimal))
                return ToDecimal();
            else if (t == typeof(TupleValue1))
                return ToTuple1();
            else if (t == typeof(TupleValue2))
                return ToTuple2();
            else if (t == typeof(TupleValue3))
                return ToTuple3();
            else if (t == typeof(TupleValue4))
                return ToTuple4();
            else if (t == typeof(TupleValue5))
                return ToTuple5();
            else if (t == typeof(TupleValue6))
                return ToTuple6();
            else if (t == typeof(TupleValue7))
                return ToTuple7();
            else if (t == typeof(TupleValue8))
                return ToTuple8();
            else if (t == typeof(BoxedValue))
                return this;
            else if (t == typeof(object))
                return ToObject();
            else if (IsObject && t == typeof(Vector2) && ObjectVal is Vector2Obj)
                return (Vector2)(Vector2Obj)ObjectVal;
            else if (IsObject && t == typeof(Vector3) && ObjectVal is Vector3Obj)
                return (Vector3)(Vector3Obj)ObjectVal;
            else if (IsObject && t == typeof(Vector4) && ObjectVal is Vector4Obj)
                return (Vector4)(Vector4Obj)ObjectVal;
            else if (IsObject && t == typeof(Quaternion) && ObjectVal is QuaternionObj)
                return (Quaternion)(QuaternionObj)ObjectVal;
            else if (IsObject && t == typeof(ColorF) && ObjectVal is ColorObj)
                return (ColorF)(ColorObj)ObjectVal;
            else if (IsObject && t == typeof(Color32) && ObjectVal is Color32Obj)
                return (Color32)(Color32Obj)ObjectVal;
            else if (IsObject && t == typeof(Vector2Int) && ObjectVal is Vector2IntObj)
                return (Vector2Int)(Vector2IntObj)ObjectVal;
            else if (IsObject && t == typeof(Vector3Int) && ObjectVal is Vector3IntObj)
                return (Vector3Int)(Vector3IntObj)ObjectVal;
            else if (IsObject && t == typeof(Matrix44) && ObjectVal is Matrix4x4Obj)
                return (Matrix44)(Matrix4x4Obj)ObjectVal;
            else
                return Convert.ChangeType(ToObject(), t);
        }
        public void GenericSet<T>(T val)
        {
            Type t = typeof(T);
            if (t == typeof(string))
                Set(GenericValueConverter.ToString<T>(val));
            else if (t == typeof(bool))
                Set(GenericValueConverter.ToBool<T>(val));
            else if (t == typeof(char))
                Set(GenericValueConverter.ToChar<T>(val));
            else if (t == typeof(sbyte))
                Set(GenericValueConverter.ToSByte<T>(val));
            else if (t == typeof(short))
                Set(GenericValueConverter.ToShort<T>(val));
            else if (t == typeof(int))
                Set(GenericValueConverter.ToInt<T>(val));
            else if (t == typeof(long))
                Set(GenericValueConverter.ToLong<T>(val));
            else if (t == typeof(byte))
                Set(GenericValueConverter.ToByte<T>(val));
            else if (t == typeof(ushort))
                Set(GenericValueConverter.ToUShort<T>(val));
            else if (t == typeof(uint))
                Set(GenericValueConverter.ToUInt<T>(val));
            else if (t == typeof(ulong))
                Set(GenericValueConverter.ToULong<T>(val));
            else if (t == typeof(float))
                Set(GenericValueConverter.ToFloat<T>(val));
            else if (t == typeof(double))
                Set(GenericValueConverter.ToDouble<T>(val));
            else if (t == typeof(decimal))
                Set(GenericValueConverter.ToDecimal<T>(val));
            else if (t == typeof(TupleValue1))
                Set(GenericValueConverter.ToTuple1<T>(val));
            else if (t == typeof(TupleValue2))
                Set(GenericValueConverter.ToTuple2<T>(val));
            else if (t == typeof(TupleValue3))
                Set(GenericValueConverter.ToTuple3<T>(val));
            else if (t == typeof(TupleValue4))
                Set(GenericValueConverter.ToTuple4<T>(val));
            else if (t == typeof(TupleValue5))
                Set(GenericValueConverter.ToTuple5<T>(val));
            else if (t == typeof(TupleValue6))
                Set(GenericValueConverter.ToTuple6<T>(val));
            else if (t == typeof(TupleValue7))
                Set(GenericValueConverter.ToTuple7<T>(val));
            else if (t == typeof(TupleValue8))
                Set(GenericValueConverter.ToTuple8<T>(val));
            else if (t == typeof(BoxedValue))
                this = GenericValueConverter.ToBoxedValue<T>(val);
            else if (t == typeof(object))
                SetWithObjectType(GenericValueConverter.ToObject<T>(val));
            else
                SetWithObjectType(val);
        }
        public void GenericSet(Type t, object val)
        {
            if (null == val) {
                if (t == typeof(string))
                    Set((string)val);
                else
                    SetWithObjectType(val);
                return;
            }
            t = val.GetType();
            if (t == typeof(string))
                Set((string)val);
            else if (t == typeof(bool))
                Set((bool)val);
            else if (t == typeof(char))
                Set((char)val);
            else if (t == typeof(sbyte))
                Set((sbyte)val);
            else if (t == typeof(short))
                Set((short)val);
            else if (t == typeof(int))
                Set((int)val);
            else if (t == typeof(long))
                Set((long)val);
            else if (t == typeof(byte))
                Set((byte)val);
            else if (t == typeof(ushort))
                Set((ushort)val);
            else if (t == typeof(uint))
                Set((uint)val);
            else if (t == typeof(ulong))
                Set((ulong)val);
            else if (t == typeof(float))
                Set((float)val);
            else if (t == typeof(double))
                Set((double)val);
            else if (t == typeof(decimal))
                Set((decimal)val);
            else if (t == typeof(TupleValue1))
                Set((TupleValue1)val);
            else if (t == typeof(TupleValue2))
                Set((TupleValue2)val);
            else if (t == typeof(TupleValue3))
                Set((TupleValue3)val);
            else if (t == typeof(TupleValue4))
                Set((TupleValue4)val);
            else if (t == typeof(TupleValue5))
                Set((TupleValue5)val);
            else if (t == typeof(TupleValue6))
                Set((TupleValue6)val);
            else if (t == typeof(TupleValue7))
                Set((TupleValue7)val);
            else if (t == typeof(TupleValue8))
                Set((TupleValue8)val);
            else if (t == typeof(BoxedValue))
                this = (BoxedValue)val;
            else
                SetWithObjectType(val);
        }

        //Used by Lua or to prevent implicit conversion problems
        public void SetBool(bool v)
        {
            Set(v);
        }
        public void SetInteger(long v)
        {
            Set(v);
        }
        public void SetNumber(double v)
        {
            Set(v);
        }
        public void SetString(string v)
        {
            Set(v);
        }
        public long GetInteger()
        {
            return GetLong();
        }
        public double GetNumber()
        {
            return GetNumber();
        }

        public void CopyFrom(BoxedValue other)
        {
            Type = other.Type;
            switch (Type) {
                case c_ObjectType:
                    ObjectVal = other.ObjectVal;
                    break;
                case c_StringType:
                    StringVal = other.StringVal;
                    break;
                case c_BoolType:
                    Union.BoolVal = other.Union.BoolVal;
                    break;
                case c_CharType:
                    Union.CharVal = other.Union.CharVal;
                    break;
                case c_SByteType:
                    Union.SByteVal = other.Union.SByteVal;
                    break;
                case c_ShortType:
                    Union.ShortVal = other.Union.ShortVal;
                    break;
                case c_IntType:
                    Union.IntVal = other.Union.IntVal;
                    break;
                case c_LongType:
                    Union.LongVal = other.Union.LongVal;
                    break;
                case c_ByteType:
                    Union.ByteVal = other.Union.ByteVal;
                    break;
                case c_UShortType:
                    Union.UShortVal = other.Union.UShortVal;
                    break;
                case c_UIntType:
                    Union.UIntVal = other.Union.UIntVal;
                    break;
                case c_ULongType:
                    Union.ULongVal = other.Union.ULongVal;
                    break;
                case c_FloatType:
                    Union.FloatVal = other.Union.FloatVal;
                    break;
                case c_DoubleType:
                    Union.DoubleVal = other.Union.DoubleVal;
                    break;
                case c_DecimalType:
                    Union.DecimalVal = other.Union.DecimalVal;
                    break;
                case c_Tuple1Type:
                    Tuple1Val = other.Tuple1Val;
                    break;
                case c_Tuple2Type:
                    Tuple2Val = other.Tuple2Val;
                    break;
                case c_Tuple3Type:
                    Tuple3Val = other.Tuple3Val;
                    break;
                case c_Tuple4Type:
                    Tuple4Val = other.Tuple4Val;
                    break;
                case c_Tuple5Type:
                    Tuple5Val = other.Tuple5Val;
                    break;
                case c_Tuple6Type:
                    Tuple6Val = other.Tuple6Val;
                    break;
                case c_Tuple7Type:
                    Tuple7Val = other.Tuple7Val;
                    break;
                case c_Tuple8Type:
                    Tuple8Val = other.Tuple8Val;
                    break;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is BoxedValue other)
                return Equals(other);
            return false;
        }

        public bool Equals(BoxedValue other)
        {
            if (Type != other.Type)
                return false;
            switch (Type) {
                case c_ObjectType:
                    return null != ObjectVal ? ObjectVal == other.ObjectVal : true;
                case c_StringType:
                    return null != StringVal ? StringVal == other.StringVal : true;
                case c_BoolType:
                    return Union.BoolVal == other.Union.BoolVal;
                case c_CharType:
                    return Union.CharVal == other.Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal == other.Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal == other.Union.ShortVal;
                case c_IntType:
                    return Union.IntVal == other.Union.IntVal;
                case c_LongType:
                    return Union.LongVal == other.Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal == other.Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal == other.Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal == other.Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal == other.Union.ULongVal;
                case c_FloatType:
                    return Union.FloatVal == other.Union.FloatVal;
                case c_DoubleType:
                    return Union.DoubleVal == other.Union.DoubleVal;
                case c_DecimalType:
                    return Union.DecimalVal == other.Union.DecimalVal;
                case c_Tuple1Type:
                    return Tuple1Val == other.Tuple1Val;
                case c_Tuple2Type:
                    return Tuple2Val == other.Tuple2Val;
                case c_Tuple3Type:
                    return Tuple3Val == other.Tuple3Val;
                case c_Tuple4Type:
                    return Tuple4Val == other.Tuple4Val;
                case c_Tuple5Type:
                    return Tuple5Val == other.Tuple5Val;
                case c_Tuple6Type:
                    return Tuple6Val == other.Tuple6Val;
                case c_Tuple7Type:
                    return Tuple7Val == other.Tuple7Val;
                case c_Tuple8Type:
                    return Tuple8Val == other.Tuple8Val;
            }
            return false;
        }

        public override int GetHashCode()
        {
            switch (Type) {
                case c_ObjectType:
                    return null != ObjectVal ? ObjectVal.GetHashCode() : 0;
                case c_StringType:
                    return null != StringVal ? StringVal.GetHashCode() : 0;
                case c_BoolType:
                    return Union.BoolVal.GetHashCode();
                case c_CharType:
                    return Union.CharVal.GetHashCode();
                case c_SByteType:
                    return Union.SByteVal.GetHashCode();
                case c_ShortType:
                    return Union.ShortVal.GetHashCode();
                case c_IntType:
                    return Union.IntVal.GetHashCode();
                case c_LongType:
                    return Union.LongVal.GetHashCode();
                case c_ByteType:
                    return Union.ByteVal.GetHashCode();
                case c_UShortType:
                    return Union.UShortVal.GetHashCode();
                case c_UIntType:
                    return Union.UIntVal.GetHashCode();
                case c_ULongType:
                    return Union.ULongVal.GetHashCode();
                case c_FloatType:
                    return Union.FloatVal.GetHashCode();
                case c_DoubleType:
                    return Union.DoubleVal.GetHashCode();
                case c_DecimalType:
                    return Union.DecimalVal.GetHashCode();
                case c_Tuple1Type:
                    return Tuple1Val.GetHashCode();
                case c_Tuple2Type:
                    return Tuple2Val.GetHashCode();
                case c_Tuple3Type:
                    return Tuple3Val.GetHashCode();
                case c_Tuple4Type:
                    return Tuple4Val.GetHashCode();
                case c_Tuple5Type:
                    return Tuple5Val.GetHashCode();
                case c_Tuple6Type:
                    return Tuple6Val.GetHashCode();
                case c_Tuple7Type:
                    return Tuple7Val.GetHashCode();
                case c_Tuple8Type:
                    return Tuple8Val.GetHashCode();
            }
            return 0;
        }
        public override string ToString()
        {
            switch (Type) {
                case c_ObjectType:
                    return null != ObjectVal ? ObjectVal.ToString() : string.Empty;
                case c_StringType:
                    return null != StringVal ? StringVal : string.Empty;
                case c_BoolType:
                    return Union.BoolVal.ToString();
                case c_CharType:
                    return Union.CharVal.ToString();
                case c_SByteType:
                    return Union.SByteVal.ToString();
                case c_ShortType:
                    return Union.ShortVal.ToString();
                case c_IntType:
                    return Union.IntVal.ToString();
                case c_LongType:
                    return Union.LongVal.ToString();
                case c_ByteType:
                    return Union.ByteVal.ToString();
                case c_UShortType:
                    return Union.UShortVal.ToString();
                case c_UIntType:
                    return Union.UIntVal.ToString();
                case c_ULongType:
                    return Union.ULongVal.ToString();
                case c_FloatType:
                    return Union.FloatVal.ToString();
                case c_DoubleType:
                    return Union.DoubleVal.ToString();
                case c_DecimalType:
                    return Union.DecimalVal.ToString();
                case c_Tuple1Type:
                    return Tuple1Val.ToString();
                case c_Tuple2Type:
                    return Tuple2Val.ToString();
                case c_Tuple3Type:
                    return Tuple3Val.ToString();
                case c_Tuple4Type:
                    return Tuple4Val.ToString();
                case c_Tuple5Type:
                    return Tuple5Val.ToString();
                case c_Tuple6Type:
                    return Tuple6Val.ToString();
                case c_Tuple7Type:
                    return Tuple7Val.ToString();
                case c_Tuple8Type:
                    return Tuple8Val.ToString();
            }
            return string.Empty;
        }

        private object ToObject()
        {
            switch (Type) {
                case c_ObjectType:
                case c_StringType:
                    return ObjectVal;
                case c_BoolType:
                    return Union.BoolVal;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal;
                case c_FloatType:
                    return Union.FloatVal;
                case c_DoubleType:
                    return Union.DoubleVal;
                case c_DecimalType:
                    return Union.DecimalVal;
                case c_Tuple1Type:
                    return Tuple1Val;
                case c_Tuple2Type:
                    return Tuple2Val;
                case c_Tuple3Type:
                    return Tuple3Val;
                case c_Tuple4Type:
                    return Tuple4Val;
                case c_Tuple5Type:
                    return Tuple5Val;
                case c_Tuple6Type:
                    return Tuple6Val;
                case c_Tuple7Type:
                    return Tuple7Val;
                case c_Tuple8Type:
                    return Tuple8Val;
            }
            return null;
        }
        private bool ToBool()
        {
            switch (Type) {
                case c_StringType:
                    if (null != StringVal) {
                        long v;
                        long.TryParse(StringVal, out v);
                        return v != 0;
                    }
                    else {
                        return false;
                    }
                case c_ObjectType:
                    if (null != ObjectVal) {
                        if (ObjectVal is bool) {
                            return (bool)ObjectVal;
                        }
                        else {
                            long v = GenericValueConverter.CastTo<long>(ObjectVal);
                            return v != 0;
                        }
                    }
                    else {
                        return false;
                    }
                case c_BoolType:
                    return Union.BoolVal;
                case c_CharType:
                    return Union.CharVal != 0;
                case c_SByteType:
                    return Union.SByteVal != 0;
                case c_ShortType:
                    return Union.ShortVal != 0;
                case c_IntType:
                    return Union.IntVal != 0;
                case c_LongType:
                    return Union.LongVal != 0;
                case c_ByteType:
                    return Union.ByteVal != 0;
                case c_UShortType:
                    return Union.UShortVal != 0;
                case c_UIntType:
                    return Union.UIntVal != 0;
                case c_ULongType:
                    return Union.ULongVal != 0;
                case c_FloatType:
                    return Union.FloatVal != 0;
                case c_DoubleType:
                    return Union.DoubleVal != 0;
                case c_DecimalType:
                    return Union.DecimalVal != 0;
            }
            return false;
        }
        private char ToChar()
        {
            switch (Type) {
                case c_StringType:
                    if (null != StringVal && StringVal.Length > 0) {
                        return StringVal[0];
                    }
                    else {
                        return '\0';
                    }
                case c_ObjectType:
                    if (null != ObjectVal) {
                        if (ObjectVal is char) {
                            return (char)ObjectVal;
                        }
                        else {
                            char v = GenericValueConverter.CastTo<char>(ObjectVal);
                            return v;
                        }
                    }
                    else {
                        return '\0';
                    }
                case c_BoolType:
                    return Union.BoolVal ? '\x01' : '\0';
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return (char)Union.SByteVal;
                case c_ShortType:
                    return (char)Union.ShortVal;
                case c_IntType:
                    return (char)Union.IntVal;
                case c_LongType:
                    return (char)Union.LongVal;
                case c_ByteType:
                    return (char)Union.ByteVal;
                case c_UShortType:
                    return (char)Union.UShortVal;
                case c_UIntType:
                    return (char)Union.UIntVal;
                case c_ULongType:
                    return (char)Union.ULongVal;
                case c_FloatType:
                    return (char)(int)Union.FloatVal;
                case c_DoubleType:
                    return (char)(long)Union.DoubleVal;
                case c_DecimalType:
                    return (char)(int)Union.DecimalVal;
            }
            return '\0';
        }
        private sbyte ToSByte()
        {
            sbyte v = 0;
            switch (Type) {
                case c_BoolType:
                    return (sbyte)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return (sbyte)Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return (sbyte)Union.ShortVal;
                case c_IntType:
                    return (sbyte)Union.IntVal;
                case c_LongType:
                    return (sbyte)Union.LongVal;
                case c_ByteType:
                    return (sbyte)Union.ByteVal;
                case c_UShortType:
                    return (sbyte)Union.UShortVal;
                case c_UIntType:
                    return (sbyte)Union.UIntVal;
                case c_ULongType:
                    return (sbyte)Union.ULongVal;
                case c_FloatType:
                    return (sbyte)Union.FloatVal;
                case c_DoubleType:
                    return (sbyte)Union.DoubleVal;
                case c_DecimalType:
                    return (sbyte)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        sbyte.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<sbyte>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private short ToShort()
        {
            short v = 0;
            switch (Type) {
                case c_BoolType:
                    return (short)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return (short)Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return (short)Union.IntVal;
                case c_LongType:
                    return (short)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return (short)Union.UShortVal;
                case c_UIntType:
                    return (short)Union.UIntVal;
                case c_ULongType:
                    return (short)Union.ULongVal;
                case c_FloatType:
                    return (short)Union.FloatVal;
                case c_DoubleType:
                    return (short)Union.DoubleVal;
                case c_DecimalType:
                    return (short)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        short.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<short>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private int ToInt()
        {
            int v = 0;
            switch (Type) {
                case c_BoolType:
                    return Union.BoolVal ? 1 : 0;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return (int)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return (int)Union.UIntVal;
                case c_ULongType:
                    return (int)Union.ULongVal;
                case c_FloatType:
                    return (int)Union.FloatVal;
                case c_DoubleType:
                    return (int)Union.DoubleVal;
                case c_DecimalType:
                    return (int)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        int.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<int>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private long ToLong()
        {
            long v = 0;
            switch (Type) {
                case c_BoolType:
                    return Union.BoolVal ? 1 : 0;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return (long)Union.ULongVal;
                case c_FloatType:
                    return (long)Union.FloatVal;
                case c_DoubleType:
                    return (long)Union.DoubleVal;
                case c_DecimalType:
                    return (long)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        long.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<long>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private byte ToByte()
        {
            byte v = 0;
            switch (Type) {
                case c_BoolType:
                    return (byte)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return (byte)Union.CharVal;
                case c_SByteType:
                    return (byte)Union.SByteVal;
                case c_ShortType:
                    return (byte)Union.ShortVal;
                case c_IntType:
                    return (byte)Union.IntVal;
                case c_LongType:
                    return (byte)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return (byte)Union.UShortVal;
                case c_UIntType:
                    return (byte)Union.UIntVal;
                case c_ULongType:
                    return (byte)Union.ULongVal;
                case c_FloatType:
                    return (byte)Union.FloatVal;
                case c_DoubleType:
                    return (byte)Union.DoubleVal;
                case c_DecimalType:
                    return (byte)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        byte.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<byte>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private ushort ToUShort()
        {
            ushort v = 0;
            switch (Type) {
                case c_BoolType:
                    return (ushort)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return (ushort)Union.SByteVal;
                case c_ShortType:
                    return (ushort)Union.ShortVal;
                case c_IntType:
                    return (ushort)Union.IntVal;
                case c_LongType:
                    return (ushort)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return (ushort)Union.UIntVal;
                case c_ULongType:
                    return (ushort)Union.ULongVal;
                case c_FloatType:
                    return (ushort)Union.FloatVal;
                case c_DoubleType:
                    return (ushort)Union.DoubleVal;
                case c_DecimalType:
                    return (ushort)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        ushort.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<ushort>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private uint ToUInt()
        {
            uint v = 0;
            switch (Type) {
                case c_BoolType:
                    return (uint)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return (uint)Union.SByteVal;
                case c_ShortType:
                    return (uint)Union.ShortVal;
                case c_IntType:
                    return (uint)Union.IntVal;
                case c_LongType:
                    return (uint)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return (uint)Union.ULongVal;
                case c_FloatType:
                    return (uint)Union.FloatVal;
                case c_DoubleType:
                    return (uint)Union.DoubleVal;
                case c_DecimalType:
                    return (uint)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        uint.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<uint>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private ulong ToULong()
        {
            ulong v = 0;
            switch (Type) {
                case c_BoolType:
                    return (ulong)(Union.BoolVal ? 1 : 0);
                case c_CharType:
                    return (ulong)Union.CharVal;
                case c_SByteType:
                    return (ulong)Union.SByteVal;
                case c_ShortType:
                    return (ulong)Union.ShortVal;
                case c_IntType:
                    return (ulong)Union.IntVal;
                case c_LongType:
                    return (ulong)Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal;
                case c_FloatType:
                    return (ulong)Union.FloatVal;
                case c_DoubleType:
                    return (ulong)Union.DoubleVal;
                case c_DecimalType:
                    return (ulong)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        ulong.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<ulong>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private float ToFloat()
        {
            float v = 0;
            switch (Type) {
                case c_BoolType:
                    return Union.BoolVal ? 1 : 0;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal;
                case c_FloatType:
                    return Union.FloatVal;
                case c_DoubleType:
                    return (float)Union.DoubleVal;
                case c_DecimalType:
                    return (float)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        float.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<float>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private double ToDouble()
        {
            double v = 0;
            switch (Type) {
                case c_BoolType:
                    return Union.BoolVal ? 1 : 0;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal;
                case c_FloatType:
                    return Union.FloatVal;
                case c_DoubleType:
                    return Union.DoubleVal;
                case c_DecimalType:
                    return (double)Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        double.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<double>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private decimal ToDecimal()
        {
            decimal v = 0;
            switch (Type) {
                case c_BoolType:
                    return Union.BoolVal ? 1 : 0;
                case c_CharType:
                    return Union.CharVal;
                case c_SByteType:
                    return Union.SByteVal;
                case c_ShortType:
                    return Union.ShortVal;
                case c_IntType:
                    return Union.IntVal;
                case c_LongType:
                    return Union.LongVal;
                case c_ByteType:
                    return Union.ByteVal;
                case c_UShortType:
                    return Union.UShortVal;
                case c_UIntType:
                    return Union.UIntVal;
                case c_ULongType:
                    return Union.ULongVal;
                case c_FloatType:
                    return (decimal)Union.FloatVal;
                case c_DoubleType:
                    return (decimal)Union.DoubleVal;
                case c_DecimalType:
                    return Union.DecimalVal;
                case c_StringType:
                    if (null != StringVal) {
                        decimal.TryParse(StringVal, out v);
                    }
                    return v;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<decimal>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue1 ToTuple1()
        {
            TupleValue1 v = Tuple.Create(BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple1Type:
                    return Tuple1Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue1>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue2 ToTuple2()
        {
            TupleValue2 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple2Type:
                    return Tuple2Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue2>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue3 ToTuple3()
        {
            TupleValue3 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple3Type:
                    return Tuple3Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue3>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue4 ToTuple4()
        {
            TupleValue4 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple4Type:
                    return Tuple4Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue4>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue5 ToTuple5()
        {
            TupleValue5 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple5Type:
                    return Tuple5Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue5>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue6 ToTuple6()
        {
            TupleValue6 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple6Type:
                    return Tuple6Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue6>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue7 ToTuple7()
        {
            TupleValue7 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple7Type:
                    return Tuple7Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue7>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }
        private TupleValue8 ToTuple8()
        {
            TupleValue8 v = Tuple.Create(BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject, BoxedValue.NullObject);
            switch (Type) {
                case c_Tuple8Type:
                    return Tuple8Val;
                case c_ObjectType:
                    if (null != ObjectVal) {
                        v = GenericValueConverter.CastTo<TupleValue8>(ObjectVal);
                    }
                    return v;
            }
            return v;
        }

        public static BoxedValue From(bool v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(sbyte v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(short v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(int v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(long v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(byte v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(ushort v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(uint v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(ulong v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(float v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(double v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(decimal v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue1 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue2 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue3 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue4 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue5 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue6 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue7 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(TupleValue8 v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue From(string v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        //Generic From
        public static BoxedValue From<T>(T v)
        {
            BoxedValue bv = new BoxedValue();
            bv.GenericSet(v);
            return bv;
        }
        public static BoxedValue From(Type t, object o)
        {
            BoxedValue bv = new BoxedValue();
            bv.GenericSet(t, o);
            return bv;
        }
        //Used by Lua or to prevent implicit conversion problems
        public static BoxedValue FromBool(bool v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue FromNumber(double v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue FromString(string v)
        {
            BoxedValue bv = new BoxedValue();
            bv.Set(v);
            return bv;
        }
        public static BoxedValue FromObject(object v)
        {
            BoxedValue bv = new BoxedValue();
            bv.SetObject(v);
            return bv;
        }

        public static BoxedValue NullObject
        {
            get { return s_NullObject; }
        }
        public static BoxedValue EmptyString
        {
            get { return s_EmptyString; }
        }
        private static BoxedValue s_NullObject = BoxedValue.FromObject(null);
        private static BoxedValue s_EmptyString = BoxedValue.FromString(string.Empty);
    }

    public class BoxedValueList : List<BoxedValue>
    {
        public BoxedValueList() { }
        public BoxedValueList(int capacity) : base(capacity) { }
        public BoxedValueList(IEnumerable<BoxedValue> coll) : base(coll) { }
        public void AddBool(bool v)
        {
            Add(BoxedValue.FromBool(v));
        }
        public void AddNumber(double v)
        {
            Add(BoxedValue.FromNumber(v));
        }
        public void AddString(string v)
        {
            Add(BoxedValue.FromString(v));
        }
        public void AddObject(object v)
        {
            Add(BoxedValue.FromObject(v));
        }
    }

    public class StrBoxedValueDict : Dictionary<string, BoxedValue>
    {
        public StrBoxedValueDict() { }
        public StrBoxedValueDict(int capacity) : base(capacity) { }
        public StrBoxedValueDict(IDictionary<string, BoxedValue> dict) : base(dict) { }
        public void AddBool(string k, bool v)
        {
            Add(k, BoxedValue.FromBool(v));
        }
        public void AddNumber(string k, double v)
        {
            Add(k, BoxedValue.FromNumber(v));
        }
        public void AddString(string k, string v)
        {
            Add(k, BoxedValue.FromString(v));
        }
        public void AddObject(string k, object v)
        {
            Add(k, BoxedValue.FromObject(v));
        }
    }
}