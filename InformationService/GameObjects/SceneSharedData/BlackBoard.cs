using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
  public class BlackBoard
  {
    public void ClearVariables()
    {
        m_BlackBoardVariables.Clear();
    }
    public void SetVariable(string key, object val)
    {
        if (m_BlackBoardVariables.ContainsKey(key)) {
            m_BlackBoardVariables[key] = val;
        } else {
            m_BlackBoardVariables.Add(key, val);
        }
    }
    public bool TryGetVariable(string key, out object val)
    {
        return m_BlackBoardVariables.TryGetValue(key, out val);
    }

    private Dictionary<string, object> m_BlackBoardVariables = new Dictionary<string, object>();
  }
}
