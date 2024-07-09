using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
  public class KeyString
  {
    public List<string> Keys
    {
      get { return m_Keys; }
      set { m_Keys = value; }
    }
    public override bool Equals(object obj)
    {
      return Equals(this, obj as KeyString);
    }
    public override int GetHashCode()
    {
      int ret = 0;
      for (int ix = 0; ix < m_Keys.Count; ++ix) {
        ret += m_Keys[ix].GetHashCode();
      }
      return ret;
    }
    public override string ToString()
    {
      return string.Join(",", m_Keys.ToArray());
    }
    public static bool operator ==(KeyString a, KeyString b) 
    {
      return Equals(a, b);
    }
    public static bool operator !=(KeyString a, KeyString b)
    {
      return !Equals(a, b);
    }
    public static bool IsNullOrEmpty(KeyString str)
    {
      return str == null || str.m_Keys.Count == 0;
    }    
    public static KeyString Wrap(List<string> strs)
    {
      KeyString obj = new KeyString();
      obj.m_Keys = strs;
      return obj;
    }
    public static KeyString Wrap(params string[] strs)
    {
      KeyString obj = new KeyString();
      for (int i = 0; i < strs.Length; ++i) {
        obj.m_Keys.Add(strs[i]);
      }
      return obj;
    }

    private static bool Equals(KeyString a, KeyString b)
    {
      bool ret = false;
      if (object.ReferenceEquals(a, b)) {
        ret = true;
      } else if (object.ReferenceEquals(a, null)) {
        ret = false;
      } else if (object.ReferenceEquals(b, null)) {
        ret = false;
      } else {
        int ct = a.m_Keys.Count;
        if (ct == b.m_Keys.Count) {
          ret = true;
          for (int ix = 0; ix < ct; ++ix) {
            if (a.m_Keys[ix] != b.m_Keys[ix]) {
              ret = false;
              break;
            }
          }
        }
      }
      return ret;
    }

    private List<string> m_Keys = new List<string>();
  }
}
