using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//导出给lua使用的常用集合类型
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

//AiQuery的排序需要实现接口，但lua实现接口无法提供给C#类，需要在这里定义并导出
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