using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//导出给lua使用的常用集合类型
public class IntList : List<int> { }
public class StrList : List<string> { }
public class IntIntDict : Dictionary<int, int> { }
public class IntStrDict : Dictionary<int, string> { }
public class IntObjDict : Dictionary<int, object> { }
public class StrIntDict : Dictionary<string, int> { }
public class StrStrDict : Dictionary<string, string> { }
public class StrObjDict : Dictionary<int, object> { }
public class ObjIntDict : Dictionary<object, int> { }
public class ObjStrDict : Dictionary<object, string> { }
public class ObjObjDict : Dictionary<object, object> { }
public class IntHashSet : HashSet<int> { }
public class StrHashSet : HashSet<int> { }
public class ObjHashSet : HashSet<object> { }
public class ObjQueue : Queue<object> { }
public class ObjStack : Stack<object> { }

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