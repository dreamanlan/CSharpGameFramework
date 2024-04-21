using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//Common collection types exported to Lua
public class IntUobjDict : Dictionary<int, UnityEngine.Object>
{
    public IntUobjDict() { }
    public IntUobjDict(IDictionary<int, UnityEngine.Object> dict) : base(dict) { }
}
public class StrUobjDict : Dictionary<string, UnityEngine.Object>
{
    public StrUobjDict() { }
    public StrUobjDict(IDictionary<string, UnityEngine.Object> dict) : base(dict) { }
}
public class ObjUobjDict : Dictionary<object, UnityEngine.Object>
{
    public ObjUobjDict() { }
    public ObjUobjDict(IDictionary<object, UnityEngine.Object> dict) : base(dict) { }
}
public class UobjIntDict : Dictionary<UnityEngine.Object, int>
{
    public UobjIntDict() { }
    public UobjIntDict(IDictionary<UnityEngine.Object, int> dict) : base(dict) { }
}
public class UobjStrDict : Dictionary<UnityEngine.Object, string>
{
    public UobjStrDict() { }
    public UobjStrDict(IDictionary<UnityEngine.Object, string> dict) : base(dict) { }
}
public class UobjObjDict : Dictionary<UnityEngine.Object, object>
{
    public UobjObjDict() { }
    public UobjObjDict(IDictionary<UnityEngine.Object, object> dict) : base(dict) { }
}
public class UobjUobjDict : Dictionary<UnityEngine.Object, int>
{
    public UobjUobjDict() { }
    public UobjUobjDict(IDictionary<UnityEngine.Object, int> dict) : base(dict) { }
}
public class UobjHashSet : HashSet<UnityEngine.Object>
{
    public UobjHashSet() { }
    public UobjHashSet(IEnumerable<UnityEngine.Object> coll) : base(coll) { }
}
public class UobjQueue : Queue<UnityEngine.Object>
{
    public UobjQueue() { }
    public UobjQueue(IEnumerable<UnityEngine.Object> coll) : base(coll) { }
}
public class UobjStack : Stack<UnityEngine.Object>
{
    public UobjStack() { }
    public UobjStack(IEnumerable<UnityEngine.Object> coll) : base(coll) { }
}

//AiQuery's sorting requires the implementation of an interface, but the Lua implementation
//interface cannot be provided to the C# class and needs to be defined and exported here.
public class AiQueryComparer : IComparer
{
    public int Compare(object x, object y)
    {
        var ax = x as ArrayList;
        var ay = y as ArrayList;

        for (int i = 1; i <= m_Count; ++i) {
            int v = CompareObject(ax[i], ay[i]);
            if (v != 0) {
                return v;
            }
        }
        return 0;
    }
    public AiQueryComparer(bool desc, int count)
    {
        m_Desc = desc;
        m_Count = count;
    }
    private int CompareObject(object x, object y)
    {
        if (x is string && y is string) {
            var xs = x as string;
            var ys = y as string;
            if (m_Desc) {
                return -xs.CompareTo(ys);
            } else {
                return xs.CompareTo(ys);
            }
        } else {
            var fx = (float)System.Convert.ChangeType(x, typeof(float));
            var fy = (float)System.Convert.ChangeType(y, typeof(float));
            if (m_Desc) {
                return -fx.CompareTo(fy);
            } else {
                return fx.CompareTo(fy);
            }
        }
    }

    private bool m_Desc = false;
    private int m_Count = 0;
}

public sealed class MonoBehaviourProxy
{
    public MonoBehaviourProxy(UnityEngine.MonoBehaviour monoBehavior)
    {
        m_MonoBehaviour = monoBehavior;
    }
    public void StartCoroutine(IEnumerator routine)
    {
        m_MonoBehaviour.StartCoroutine(routine);
    }
    public void StopAllCoroutines()
    {
        m_MonoBehaviour.StopAllCoroutines();
    }

    public Coroutine StartOneCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    private UnityEngine.MonoBehaviour m_MonoBehaviour = null;
}