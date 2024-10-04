using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript
{
    public static class StoryFunctionHelper
    {
        public const int c_MaxWaitCommandTime = 3600000;
        public static T CastTo<T>(object obj)
        {
            return Converter.CastTo<T>(obj);
        }
        public static object CastTo(Type t, object obj)
        {
            return Converter.CastTo(t, obj);
        }
    }
    public sealed class StoryFunctionResult
    {
        public StoryFunctionResult Clone()
        {
            StoryFunctionResult val = new StoryFunctionResult();
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
    public interface IStoryFunctionParam
    {
        Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments);
        IStoryFunctionParam Clone();
        void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args);
        bool HaveValue { get; }
    }
    public sealed class StoryFunctionParam : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            if (enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                    }
                }
            }
            return ret;
        }
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam val = new StoryFunctionParam();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        { }
        public bool HaveValue
        {
            get { return true; }
        }
    }
    public sealed class StoryFunctionParam<P1> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1> val = new StoryFunctionParam<P1>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
    }
    public sealed class StoryFunctionParam<P1, P2> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2> val = new StoryFunctionParam<P1, P2>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3> val = new StoryFunctionParam<P1, P2, P3>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3, P4> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3, P4> val = new StoryFunctionParam<P1, P2, P3, P4>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
        private IStoryFunction<P4> m_P4 = new StoryFunction<P4>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3, P4, P5> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3, P4, P5> val = new StoryFunctionParam<P1, P2, P3, P4, P5>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
        private IStoryFunction<P4> m_P4 = new StoryFunction<P4>();
        private IStoryFunction<P5> m_P5 = new StoryFunction<P5>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3, P4, P5, P6> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3, P4, P5, P6> val = new StoryFunctionParam<P1, P2, P3, P4, P5, P6>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
        private IStoryFunction<P4> m_P4 = new StoryFunction<P4>();
        private IStoryFunction<P5> m_P5 = new StoryFunction<P5>();
        private IStoryFunction<P6> m_P6 = new StoryFunction<P6>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7> val = new StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
        private IStoryFunction<P4> m_P4 = new StoryFunction<P4>();
        private IStoryFunction<P5> m_P5 = new StoryFunction<P5>();
        private IStoryFunction<P6> m_P6 = new StoryFunction<P6>();
        private IStoryFunction<P7> m_P7 = new StoryFunction<P7>();
    }
    public sealed class StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7, P8> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7, P8> val = new StoryFunctionParam<P1, P2, P3, P4, P5, P6, P7, P8>();
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
        private IStoryFunction<P1> m_P1 = new StoryFunction<P1>();
        private IStoryFunction<P2> m_P2 = new StoryFunction<P2>();
        private IStoryFunction<P3> m_P3 = new StoryFunction<P3>();
        private IStoryFunction<P4> m_P4 = new StoryFunction<P4>();
        private IStoryFunction<P5> m_P5 = new StoryFunction<P5>();
        private IStoryFunction<P6> m_P6 = new StoryFunction<P6>();
        private IStoryFunction<P7> m_P7 = new StoryFunction<P7>();
        private IStoryFunction<P8> m_P8 = new StoryFunction<P8>();
    }
    public sealed class StoryFunctionParams<P> : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryFunction<P> val = new StoryFunction<P>();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(default(P));
                }
            }
            return ret;
        }
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParams<P> val = new StoryFunctionParams<P>();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction<P> arg = m_Args[i];
                val.m_Args.Add(arg.Clone());
                m_Values.Add(default(P));
            }
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction<P> val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }

            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction<P> val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get {
                bool ret = true;
                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction<P> val = m_Args[i];
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
        private List<IStoryFunction<P>> m_Args = new List<IStoryFunction<P>>();
        private List<P> m_Values = new List<P>();
    }
    public sealed class StoryFunctionParams : IStoryFunctionParam
    {
        public Dsl.FunctionData InitFromDsl(Dsl.ISyntaxComponent param, int startIndex, bool enableComments)
        {
            Dsl.FunctionData ret = null;
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null == callData && enableComments) {
                var statementData = param as Dsl.StatementData;
                if (null != statementData && statementData.GetFunctionNum() == 2) {
                    var first = statementData.First.AsFunction;
                    var last = statementData.Last.AsFunction;
                    if (!first.HaveStatement() && (last.GetId() == "comment" || last.GetId() == "comments")) {
                        ret = last;
                        statementData.Functions.RemoveAt(1);
                        callData = first;
                    }
                }
            }
            if (null != callData) {
                for (int i = startIndex; i < callData.GetParamNum(); ++i) {
                    StoryFunction val = new StoryFunction();
                    val.InitFromDsl(callData.GetParam(i));
                    m_Args.Add(val);
                    m_Values.Add(BoxedValue.NullObject);
                }
            }
            return ret;
        }
        public IStoryFunctionParam Clone()
        {
            StoryFunctionParams val = new StoryFunctionParams();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction arg = m_Args[i];
                val.m_Args.Add(arg.Clone());
                val.m_Values.Add(BoxedValue.NullObject);
            }
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }

            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                m_Values[i] = val.Value;
            }
        }
        public bool HaveValue
        {
            get {
                bool ret = true;
                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
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
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private BoxedValueList m_Values = new BoxedValueList();
    }
}
