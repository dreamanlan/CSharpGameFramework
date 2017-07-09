using System;
using System.Collections;
using System.Collections.Generic;
namespace StorySystem
{
    public static class StoryValueHelper
    {
        public const int c_MaxWaitCommandTime = 3600000;
        public static T CastTo<T>(object obj)
        {
            if (obj is T) {
                return (T)obj;
            } else {
                try {
                    return (T)Convert.ChangeType(obj, typeof(T));
                } catch {
                    return default(T);
                }
            }
        }
        public static IStoryValue<object> AdaptFrom<T>(IStoryValue<T> original)
        {
            return new StoryValueAdapter<T>(original);
        }
    }
    public sealed class StoryValueResult
    {
        public StoryValueResult Clone()
        {
            StoryValueResult val = new StoryValueResult();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
            set
            {
                m_HaveValue = value;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_HaveValue = true;
                m_Value = value;
            }
        }
        private bool m_HaveValue;
        private object m_Value;
    }
    public interface IStoryValueParam
    {
        void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex);
        IStoryValueParam Clone();
        void Evaluate(StoryInstance instance, object iterator, object[] args);
        bool HaveValue { get; }
    }
    public sealed class StoryValueParam : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        { }
        public IStoryValueParam Clone()
        {
            StoryValueParam val = new StoryValueParam();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {  }
        public bool HaveValue
        {
            get { return true; }
        }
    }
    public sealed class StoryValueParam<P1> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 1) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1> val = new StoryValueParam<P1>();
            val.m_P1 = m_P1.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
    }
    public sealed class StoryValueParam<P1, P2> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 2) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2> val = new StoryValueParam<P1, P2>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
    }
    public sealed class StoryValueParam<P1, P2, P3> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 3) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3> val = new StoryValueParam<P1, P2, P3>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 4) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3, P4> val = new StoryValueParam<P1, P2, P3, P4>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            val.m_P4 = m_P4.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
            m_P4.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        public P4 Param4Value
        {
            get { return m_P4.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
        private IStoryValue<P4> m_P4 = new StoryValue<P4>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 5) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3, P4, P5> val = new StoryValueParam<P1, P2, P3, P4, P5>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            val.m_P4 = m_P4.Clone();
            val.m_P5 = m_P5.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
            m_P4.Evaluate(instance, iterator, args);
            m_P5.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        public P4 Param4Value
        {
            get { return m_P4.Value; }
        }
        public P5 Param5Value
        {
            get { return m_P5.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
        private IStoryValue<P4> m_P4 = new StoryValue<P4>();
        private IStoryValue<P5> m_P5 = new StoryValue<P5>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 6) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3, P4, P5, P6> val = new StoryValueParam<P1, P2, P3, P4, P5, P6>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            val.m_P4 = m_P4.Clone();
            val.m_P5 = m_P5.Clone();
            val.m_P6 = m_P6.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
            m_P4.Evaluate(instance, iterator, args);
            m_P5.Evaluate(instance, iterator, args);
            m_P6.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        public P4 Param4Value
        {
            get { return m_P4.Value; }
        }
        public P5 Param5Value
        {
            get { return m_P5.Value; }
        }
        public P6 Param6Value
        {
            get { return m_P6.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
        private IStoryValue<P4> m_P4 = new StoryValue<P4>();
        private IStoryValue<P5> m_P5 = new StoryValue<P5>();
        private IStoryValue<P6> m_P6 = new StoryValue<P6>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 7) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
                m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3, P4, P5, P6, P7> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            val.m_P4 = m_P4.Clone();
            val.m_P5 = m_P5.Clone();
            val.m_P6 = m_P6.Clone();
            val.m_P7 = m_P7.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
            m_P4.Evaluate(instance, iterator, args);
            m_P5.Evaluate(instance, iterator, args);
            m_P6.Evaluate(instance, iterator, args);
            m_P7.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        public P4 Param4Value
        {
            get { return m_P4.Value; }
        }
        public P5 Param5Value
        {
            get { return m_P5.Value; }
        }
        public P6 Param6Value
        {
            get { return m_P6.Value; }
        }
        public P7 Param7Value
        {
            get { return m_P7.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
        private IStoryValue<P4> m_P4 = new StoryValue<P4>();
        private IStoryValue<P5> m_P5 = new StoryValue<P5>();
        private IStoryValue<P6> m_P6 = new StoryValue<P6>();
        private IStoryValue<P7> m_P7 = new StoryValue<P7>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetParamNum() >= 8) {
                m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
                m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
                m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            val.m_P4 = m_P4.Clone();
            val.m_P5 = m_P5.Clone();
            val.m_P6 = m_P6.Clone();
            val.m_P7 = m_P7.Clone();
            val.m_P8 = m_P8.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_P1.Evaluate(instance, iterator, args);
            m_P2.Evaluate(instance, iterator, args);
            m_P3.Evaluate(instance, iterator, args);
            m_P4.Evaluate(instance, iterator, args);
            m_P5.Evaluate(instance, iterator, args);
            m_P6.Evaluate(instance, iterator, args);
            m_P7.Evaluate(instance, iterator, args);
            m_P8.Evaluate(instance, iterator, args);
        
        }
        public bool HaveValue
        {
            get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue; }
        }
        public P1 Param1Value
        {
            get { return m_P1.Value; }
        }
        public P2 Param2Value
        {
            get { return m_P2.Value; }
        }
        public P3 Param3Value
        {
            get { return m_P3.Value; }
        }
        public P4 Param4Value
        {
            get { return m_P4.Value; }
        }
        public P5 Param5Value
        {
            get { return m_P5.Value; }
        }
        public P6 Param6Value
        {
            get { return m_P6.Value; }
        }
        public P7 Param7Value
        {
            get { return m_P7.Value; }
        }
        public P8 Param8Value
        {
            get { return m_P8.Value; }
        }
        private IStoryValue<P1> m_P1 = new StoryValue<P1>();
        private IStoryValue<P2> m_P2 = new StoryValue<P2>();
        private IStoryValue<P3> m_P3 = new StoryValue<P3>();
        private IStoryValue<P4> m_P4 = new StoryValue<P4>();
        private IStoryValue<P5> m_P5 = new StoryValue<P5>();
        private IStoryValue<P6> m_P6 = new StoryValue<P6>();
        private IStoryValue<P7> m_P7 = new StoryValue<P7>();
        private IStoryValue<P8> m_P8 = new StoryValue<P8>();
    }
    /*
    //更多参数的版本在ios上aot编译失败（内部错误，原因不明），先注掉。实际需要超过8个参数的StoryValue或StoryCommand时，直接实现IStoryValue接口与继承AbstractStoryCommand。
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 9) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 10) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 11) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 12) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
          m_P12.InitFromDsl(callData.GetParam(startIndex + 11));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        val.m_P12 = m_P12.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
        m_P12.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue && m_P12.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      public P12 Param12Value
      {
        get { return m_P12.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
      private IStoryValue<P12> m_P12 = new StoryValue<P12>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 13) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
          m_P12.InitFromDsl(callData.GetParam(startIndex + 11));
          m_P13.InitFromDsl(callData.GetParam(startIndex + 12));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        val.m_P12 = m_P12.Clone();
        val.m_P13 = m_P13.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
        m_P12.Evaluate(instance, iterator, args);
        m_P13.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue && m_P12.HaveValue && m_P13.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      public P12 Param12Value
      {
        get { return m_P12.Value; }
      }
      public P13 Param13Value
      {
        get { return m_P13.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
      private IStoryValue<P12> m_P12 = new StoryValue<P12>();
      private IStoryValue<P13> m_P13 = new StoryValue<P13>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 14) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
          m_P12.InitFromDsl(callData.GetParam(startIndex + 11));
          m_P13.InitFromDsl(callData.GetParam(startIndex + 12));
          m_P14.InitFromDsl(callData.GetParam(startIndex + 13));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        val.m_P12 = m_P12.Clone();
        val.m_P13 = m_P13.Clone();
        val.m_P14 = m_P14.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
        m_P12.Evaluate(instance, iterator, args);
        m_P13.Evaluate(instance, iterator, args);
        m_P14.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue && m_P12.HaveValue && m_P13.HaveValue && m_P14.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      public P12 Param12Value
      {
        get { return m_P12.Value; }
      }
      public P13 Param13Value
      {
        get { return m_P13.Value; }
      }
      public P14 Param14Value
      {
        get { return m_P14.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
      private IStoryValue<P12> m_P12 = new StoryValue<P12>();
      private IStoryValue<P13> m_P13 = new StoryValue<P13>();
      private IStoryValue<P14> m_P14 = new StoryValue<P14>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 15) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
          m_P12.InitFromDsl(callData.GetParam(startIndex + 11));
          m_P13.InitFromDsl(callData.GetParam(startIndex + 12));
          m_P14.InitFromDsl(callData.GetParam(startIndex + 13));
          m_P15.InitFromDsl(callData.GetParam(startIndex + 14));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        val.m_P12 = m_P12.Clone();
        val.m_P13 = m_P13.Clone();
        val.m_P14 = m_P14.Clone();
        val.m_P15 = m_P15.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
        m_P12.Evaluate(instance, iterator, args);
        m_P13.Evaluate(instance, iterator, args);
        m_P14.Evaluate(instance, iterator, args);
        m_P15.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue && m_P12.HaveValue && m_P13.HaveValue && m_P14.HaveValue && m_P15.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      public P12 Param12Value
      {
        get { return m_P12.Value; }
      }
      public P13 Param13Value
      {
        get { return m_P13.Value; }
      }
      public P14 Param14Value
      {
        get { return m_P14.Value; }
      }
      public P15 Param15Value
      {
        get { return m_P15.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
      private IStoryValue<P12> m_P12 = new StoryValue<P12>();
      private IStoryValue<P13> m_P13 = new StoryValue<P13>();
      private IStoryValue<P14> m_P14 = new StoryValue<P14>();
      private IStoryValue<P15> m_P15 = new StoryValue<P15>();
    }
    public sealed class StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> : IStoryValueParam
    {
      public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
      {
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData && callData.GetParamNum() >= 16) {
          m_P1.InitFromDsl(callData.GetParam(startIndex + 0));
          m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
          m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
          m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
          m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
          m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
          m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
          m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
          m_P9.InitFromDsl(callData.GetParam(startIndex + 8));
          m_P10.InitFromDsl(callData.GetParam(startIndex + 9));
          m_P11.InitFromDsl(callData.GetParam(startIndex + 10));
          m_P12.InitFromDsl(callData.GetParam(startIndex + 11));
          m_P13.InitFromDsl(callData.GetParam(startIndex + 12));
          m_P14.InitFromDsl(callData.GetParam(startIndex + 13));
          m_P15.InitFromDsl(callData.GetParam(startIndex + 14));
          m_P16.InitFromDsl(callData.GetParam(startIndex + 15));
        }
      }
      public IStoryValueParam Clone()
      {
        StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> val = new StoryValueParam<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>();
        val.m_P1 = m_P1.Clone();
        val.m_P2 = m_P2.Clone();
        val.m_P3 = m_P3.Clone();
        val.m_P4 = m_P4.Clone();
        val.m_P5 = m_P5.Clone();
        val.m_P6 = m_P6.Clone();
        val.m_P7 = m_P7.Clone();
        val.m_P8 = m_P8.Clone();
        val.m_P9 = m_P9.Clone();
        val.m_P10 = m_P10.Clone();
        val.m_P11 = m_P11.Clone();
        val.m_P12 = m_P12.Clone();
        val.m_P13 = m_P13.Clone();
        val.m_P14 = m_P14.Clone();
        val.m_P15 = m_P15.Clone();
        val.m_P16 = m_P16.Clone();
        return val;
      }
      public void Evaluate(StoryInstance instance, object iterator, object[] args)
          {
        m_P1.Evaluate(instance, iterator, args);
        m_P2.Evaluate(instance, iterator, args);
        m_P3.Evaluate(instance, iterator, args);
        m_P4.Evaluate(instance, iterator, args);
        m_P5.Evaluate(instance, iterator, args);
        m_P6.Evaluate(instance, iterator, args);
        m_P7.Evaluate(instance, iterator, args);
        m_P8.Evaluate(instance, iterator, args);
        m_P9.Evaluate(instance, iterator, args);
        m_P10.Evaluate(instance, iterator, args);
        m_P11.Evaluate(instance, iterator, args);
        m_P12.Evaluate(instance, iterator, args);
        m_P13.Evaluate(instance, iterator, args);
        m_P14.Evaluate(instance, iterator, args);
        m_P15.Evaluate(instance, iterator, args);
        m_P16.Evaluate(instance, iterator, args);
      
      }
      public bool HaveValue
      {
        get { return m_P1.HaveValue && m_P2.HaveValue && m_P3.HaveValue && m_P4.HaveValue && m_P5.HaveValue && m_P6.HaveValue && m_P7.HaveValue && m_P8.HaveValue && m_P9.HaveValue && m_P10.HaveValue && m_P11.HaveValue && m_P12.HaveValue && m_P13.HaveValue && m_P14.HaveValue && m_P15.HaveValue && m_P16.HaveValue; }
      }
      public P1 Param1Value
      {
        get { return m_P1.Value; }
      }
      public P2 Param2Value
      {
        get { return m_P2.Value; }
      }
      public P3 Param3Value
      {
        get { return m_P3.Value; }
      }
      public P4 Param4Value
      {
        get { return m_P4.Value; }
      }
      public P5 Param5Value
      {
        get { return m_P5.Value; }
      }
      public P6 Param6Value
      {
        get { return m_P6.Value; }
      }
      public P7 Param7Value
      {
        get { return m_P7.Value; }
      }
      public P8 Param8Value
      {
        get { return m_P8.Value; }
      }
      public P9 Param9Value
      {
        get { return m_P9.Value; }
      }
      public P10 Param10Value
      {
        get { return m_P10.Value; }
      }
      public P11 Param11Value
      {
        get { return m_P11.Value; }
      }
      public P12 Param12Value
      {
        get { return m_P12.Value; }
      }
      public P13 Param13Value
      {
        get { return m_P13.Value; }
      }
      public P14 Param14Value
      {
        get { return m_P14.Value; }
      }
      public P15 Param15Value
      {
        get { return m_P15.Value; }
      }
      public P16 Param16Value
      {
        get { return m_P16.Value; }
      }
      private IStoryValue<P1> m_P1 = new StoryValue<P1>();
      private IStoryValue<P2> m_P2 = new StoryValue<P2>();
      private IStoryValue<P3> m_P3 = new StoryValue<P3>();
      private IStoryValue<P4> m_P4 = new StoryValue<P4>();
      private IStoryValue<P5> m_P5 = new StoryValue<P5>();
      private IStoryValue<P6> m_P6 = new StoryValue<P6>();
      private IStoryValue<P7> m_P7 = new StoryValue<P7>();
      private IStoryValue<P8> m_P8 = new StoryValue<P8>();
      private IStoryValue<P9> m_P9 = new StoryValue<P9>();
      private IStoryValue<P10> m_P10 = new StoryValue<P10>();
      private IStoryValue<P11> m_P11 = new StoryValue<P11>();
      private IStoryValue<P12> m_P12 = new StoryValue<P12>();
      private IStoryValue<P13> m_P13 = new StoryValue<P13>();
      private IStoryValue<P14> m_P14 = new StoryValue<P14>();
      private IStoryValue<P15> m_P15 = new StoryValue<P15>();
      private IStoryValue<P16> m_P16 = new StoryValue<P16>();
    }
    */
    public sealed class StoryValueParams<P> : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryValue<P> val = new StoryValue<P>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(default(P));
                }
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParams<P> val = new StoryValueParams<P>();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<P> arg = m_Args[i];
                val.m_Args.Add(arg.Clone());
                m_Values.Add(default(P));
            }
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<P> val = m_Args[i];
                val.Evaluate(instance, iterator, args);
            }
        
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<P> val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get
            {
                bool ret = true;
                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<P> val = m_Args[i];
                    if (!val.HaveValue) {
                        ret = false;
                        break;
                    }
                }
                return ret;
            }
        }
        public List<P> Values
        {
            get
            {
                return m_Values;
            }
        }
        private List<IStoryValue<P>> m_Args = new List<IStoryValue<P>>();
        private List<P> m_Values = new List<P>();
    }
    public sealed class StoryValueParams : IStoryValueParam
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param, int startIndex)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(null);
                }
            }
        }
        public IStoryValueParam Clone()
        {
            StoryValueParams val = new StoryValueParams();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> arg = m_Args[i];
                val.m_Args.Add(arg.Clone());
                val.m_Values.Add(null);
            }
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance, iterator, args);
            }

            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get
            {
                bool ret = true;
                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    if (!val.HaveValue) {
                        ret = false;
                        break;
                    }
                }
                return ret;
            }
        }
        public ArrayList Values
        {
            get
            {
                return m_Values;
            }
        }
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private ArrayList m_Values = new ArrayList();
    }
}
