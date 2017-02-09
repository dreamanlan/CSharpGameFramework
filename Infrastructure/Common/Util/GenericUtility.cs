using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IntList : List<int>
{
    public IntList() { }
    public IntList(IEnumerable<int> coll) : base(coll) { }
}
public class StrList : List<string>
{
    public StrList() { }
    public StrList(IEnumerable<string> coll) : base(coll) { }
}
public class IntIntDict : Dictionary<int, int>
{
    public IntIntDict() { }
    public IntIntDict(IDictionary<int, int> dict) : base(dict) { }
}
public class IntStrDict : Dictionary<int, string>
{
    public IntStrDict() { }
    public IntStrDict(IDictionary<int, string> dict) : base(dict) { }
}
public class IntObjDict : Dictionary<int, object>
{
    public IntObjDict() { }
    public IntObjDict(IDictionary<int, object> dict) : base(dict) { }
}
public class StrIntDict : Dictionary<string, int>
{
    public StrIntDict() { }
    public StrIntDict(IDictionary<string, int> dict) : base(dict) { }
}
public class StrStrDict : Dictionary<string, string>
{
    public StrStrDict() { }
    public StrStrDict(IDictionary<string, string> dict) : base(dict) { }
}
public class StrObjDict : Dictionary<string, object>
{
    public StrObjDict() { }
    public StrObjDict(IDictionary<string, object> dict) : base(dict) { }
}
public class ObjIntDict : Dictionary<object, int>
{
    public ObjIntDict() { }
    public ObjIntDict(IDictionary<object, int> dict) : base(dict) { }
}
public class ObjStrDict : Dictionary<object, string>
{
    public ObjStrDict() { }
    public ObjStrDict(IDictionary<object, string> dict) : base(dict) { }
}
public class ObjObjDict : Dictionary<object, object>
{
    public ObjObjDict() { }
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
public class ObjQueue : Queue<object>
{
    public ObjQueue() { }
    public ObjQueue(IEnumerable<object> coll) : base(coll) { }
}
public class StrQueue : Queue<string>
{
    public StrQueue() { }
    public StrQueue(IEnumerable<string> coll) : base(coll) { }
}
public class ObjStack : Stack<object>
{
    public ObjStack() { }
    public ObjStack(IEnumerable<object> coll) : base(coll) { }
}
public class StrStack : Stack<string>
{
    public StrStack() { }
    public StrStack(IEnumerable<string> coll) : base(coll) { }
}