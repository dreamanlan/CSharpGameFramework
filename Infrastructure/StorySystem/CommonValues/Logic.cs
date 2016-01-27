using System;
using System.Collections.Generic;
using StorySystem;

namespace StorySystem.CommonValues
{
  internal sealed class AndOperator : IStoryValue<object>
  {
    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
      Dsl.CallData callData = param as Dsl.CallData;
      if (null != callData && callData.GetId() == "&&") {
        if (callData.GetParamNum() == 2) {
          m_X.InitFromDsl(callData.GetParam(0));
          m_Y.InitFromDsl(callData.GetParam(1));
          m_Flag = m_X.Flag | m_Y.Flag;
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      AndOperator val = new AndOperator();
      val.m_X = m_X.Clone();
      val.m_Y = m_Y.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      val.m_Flag = m_Flag;
      return val;
    }
    public void Substitute(object iterator, object[] args)
    {
      m_HaveValue = false;
      if (StoryValueHelper.HaveArg(Flag)) {
        m_X.Substitute(iterator, args);
        m_Y.Substitute(iterator, args);
      }
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      m_Y.Evaluate(instance);
      TryUpdateValue();
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }
    public int Flag
    {
      get
      {
        return m_Flag;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue && m_Y.HaveValue) {
        m_HaveValue = true;
        int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
        int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
        m_Value = ((x != 0 && y != 0) ? 1 : 0);
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private IStoryValue<object> m_Y = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
    private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
  }
  internal sealed class OrOperator : IStoryValue<object>
  {
    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
      Dsl.CallData callData = param as Dsl.CallData;
      if (null != callData && callData.GetId() == "||") {
        if (callData.GetParamNum() == 2) {
          m_X.InitFromDsl(callData.GetParam(0));
          m_Y.InitFromDsl(callData.GetParam(1));
          m_Flag = m_X.Flag | m_Y.Flag;
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      OrOperator val = new OrOperator();
      val.m_X = m_X.Clone();
      val.m_Y = m_Y.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      val.m_Flag = m_Flag;
      return val;
    }
    public void Substitute(object iterator, object[] args)
    {
      m_HaveValue = false;
      if (StoryValueHelper.HaveArg(Flag)) {
        m_X.Substitute(iterator, args);
        m_Y.Substitute(iterator, args);
      }
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      m_Y.Evaluate(instance);
      TryUpdateValue();
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }
    public int Flag
    {
      get
      {
        return m_Flag;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue && m_Y.HaveValue) {
        m_HaveValue = true;
        int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
        int y = (int)Convert.ChangeType(m_Y.Value, typeof(int));
        m_Value = ((x != 0 || y != 0) ? 1 : 0);
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private IStoryValue<object> m_Y = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
    private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
  }
  internal sealed class NotOperator : IStoryValue<object>
  {
    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
      Dsl.CallData callData = param as Dsl.CallData;
      if (null != callData && callData.GetId() == "!") {
        if (callData.GetParamNum() > 0) {
          m_X.InitFromDsl(callData.GetParam(0));
          m_Flag = m_X.Flag;
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      NotOperator val = new NotOperator();
      val.m_X = m_X.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      val.m_Flag = m_Flag;
      return val;
    }
    public void Substitute(object iterator, object[] args)
    {
      m_HaveValue = false;
      if (StoryValueHelper.HaveArg(Flag)) {
        m_X.Substitute(iterator, args);
      }
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      TryUpdateValue();
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }
    public int Flag
    {
      get
      {
        return m_Flag;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue) {
        m_HaveValue = true;
        int x = (int)Convert.ChangeType(m_X.Value, typeof(int));
        m_Value = (x == 0 ? 1 : 0);
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
    private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
  }
}
