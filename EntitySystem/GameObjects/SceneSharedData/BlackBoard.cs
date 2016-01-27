using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
  public class BlackBoard
  {
    public TypedDataCollection BlackBoardDatas
    {
      get { return m_BlackBoardDatas; }
    }
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
    public void Reset()
    {
      m_BlackBoardDatas.Clear();
    }
    public bool IsAutoOperate
    {
        get { return m_IsAutoOperate; }
        set { m_IsAutoOperate = value; }
    }
    public bool IsGameOver
    {
        get { return m_IsGameOver; }
        set { m_IsGameOver = value; }
    }
    public int FormationId
    {
        get { return m_FormationId; }
        set { m_FormationId = value; }
    }

    private bool m_IsAutoOperate = false;
    private bool m_IsGameOver = false;
    private int m_FormationId = 1;
    private TypedDataCollection m_BlackBoardDatas = new TypedDataCollection();
    private Dictionary<string, object> m_BlackBoardVariables = new Dictionary<string, object>();
  }
}
