using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public IntHashSet(IEnumerable<int> coll) : base(coll) { }
}
public class StrHashSet : HashSet<string>
{
    public StrHashSet() { }
    public StrHashSet(IEnumerable<string> coll) : base(coll) { }
}
public class ObjHashSet : HashSet<object>
{
    public ObjHashSet() { }
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
    public IntObjSortedDict(IDictionary<int, object> dict) : base(dict) { }
}
public class Vector2List : List<ScriptRuntime.Vector2>
{
    public Vector2List() { }
    public Vector2List(int capacity) : base(capacity) { }
    public Vector2List(ICollection<ScriptRuntime.Vector2> coll) : base(coll) { }
}
public class Vector3List : List<ScriptRuntime.Vector3>
{
    public Vector3List() { }
    public Vector3List(int capacity) : base(capacity) { }
    public Vector3List(ICollection<ScriptRuntime.Vector3> coll) : base(coll) { }
}
