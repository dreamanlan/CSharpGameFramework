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
            if (obj is BoxedValue) {
                return ((BoxedValue)obj).Get<T>();
            }
            else if (obj is T) {
                return (T)obj;
            }
            else {
                try {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                catch {
                    return default(T);
                }
            }
        }
        public static object CastTo(Type t, object obj)
        {
            if (null == obj)
                return null;
            Type st = obj.GetType();
            if (obj is BoxedValue) {
                return ((BoxedValue)obj).Get(t);
            }
            else if (t.IsAssignableFrom(st) || st.IsSubclassOf(t)) {
                return obj;
            }
            else {
                try {
                    return Convert.ChangeType(obj, t);
                }
                catch {
                    return null;
                }
            }
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
            get {
                return m_HaveValue;
            }
            set {
                m_HaveValue = value;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
            set {
                m_HaveValue = true;
                m_Value = value;
            }
        }
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public interface IStoryValueParam
    {
        Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments);
        IStoryValueParam Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);
        bool HaveValue { get; }
    }
    public sealed class StoryValueParam : IStoryValueParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            if (enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                    }
                }
            }
            return ret;
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam val = new StoryValueParam();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        { }
        public bool HaveValue
        {
            get { return true; }
        }
    }
    public sealed class StoryValueParam<P1> : IStoryValueParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 1) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
            }
            return ret;
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1> val = new StoryValueParam<P1>();
            val.m_P1 = m_P1.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 2) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
            }
            return ret;
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2> val = new StoryValueParam<P1, P2>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 3) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
            }
            return ret;
        }
        public IStoryValueParam Clone()
        {
            StoryValueParam<P1, P2, P3> val = new StoryValueParam<P1, P2, P3>();
            val.m_P1 = m_P1.Clone();
            val.m_P2 = m_P2.Clone();
            val.m_P3 = m_P3.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 4) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);
            m_P4.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 5) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);
            m_P4.Evaluate(instance, handler, iterator, args);
            m_P5.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 6) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);
            m_P4.Evaluate(instance, handler, iterator, args);
            m_P5.Evaluate(instance, handler, iterator, args);
            m_P6.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 7) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
                m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);
            m_P4.Evaluate(instance, handler, iterator, args);
            m_P5.Evaluate(instance, handler, iterator, args);
            m_P6.Evaluate(instance, handler, iterator, args);
            m_P7.Evaluate(instance, handler, iterator, args);

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
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData && callData.GetParamNum() >= 8) {
                m_P1.InitFromDsl(callData.GetParam(startIndex));
                m_P2.InitFromDsl(callData.GetParam(startIndex + 1));
                m_P3.InitFromDsl(callData.GetParam(startIndex + 2));
                m_P4.InitFromDsl(callData.GetParam(startIndex + 3));
                m_P5.InitFromDsl(callData.GetParam(startIndex + 4));
                m_P6.InitFromDsl(callData.GetParam(startIndex + 5));
                m_P7.InitFromDsl(callData.GetParam(startIndex + 6));
                m_P8.InitFromDsl(callData.GetParam(startIndex + 7));
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_P1.Evaluate(instance, handler, iterator, args);
            m_P2.Evaluate(instance, handler, iterator, args);
            m_P3.Evaluate(instance, handler, iterator, args);
            m_P4.Evaluate(instance, handler, iterator, args);
            m_P5.Evaluate(instance, handler, iterator, args);
            m_P6.Evaluate(instance, handler, iterator, args);
            m_P7.Evaluate(instance, handler, iterator, args);
            m_P8.Evaluate(instance, handler, iterator, args);

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
    public sealed class StoryValueParams<P> : IStoryValueParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryValue<P> val = new StoryValue<P>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(default(P));
                }
            }
            return ret;
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
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<P> val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }

            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<P> val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get {
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
            get {
                return m_Values;
            }
        }
        private List<IStoryValue<P>> m_Args = new List<IStoryValue<P>>();
        private List<P> m_Values = new List<P>();
    }
    public sealed class StoryValueParams : IStoryValueParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First;
                    var last = statementData.Last;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(BoxedValue.NullObject);
                }
            }
            return ret;
        }
        public IStoryValueParam Clone()
        {
            StoryValueParams val = new StoryValueParams();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue arg = m_Args[i];
                val.m_Args.Add(arg.Clone());
                val.m_Values.Add(BoxedValue.NullObject);
            }
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }

            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get {
                bool ret = true;
                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    if (!val.HaveValue) {
                        ret = false;
                        break;
                    }
                }
                return ret;
            }
        }
        public BoxedValueList Values
        {
            get {
                return m_Values;
            }
        }
        private List<IStoryValue> m_Args = new List<IStoryValue>();
        private BoxedValueList m_Values = new BoxedValueList();
    }
}
