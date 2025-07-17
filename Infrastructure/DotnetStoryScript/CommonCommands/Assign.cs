using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.CommonCommands
{
    using TupleValue1 = Tuple<BoxedValue>;
    using TupleValue2 = Tuple<BoxedValue, BoxedValue>;
    using TupleValue3 = Tuple<BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue4 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue5 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue6 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue7 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue>;
    using TupleValue8 = Tuple<BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, BoxedValue, Tuple<BoxedValue>>;

    public sealed class TupleSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            TupleSetCommand cmd = new TupleSetCommand();
            cmd.m_Line = m_Line;
            cmd.m_VarIds.AddRange(m_VarIds);
            foreach(var v in m_EmbeddedVars) {
                var vs = new List<ValueTuple<string, int>>();
                vs.AddRange(v);
                cmd.m_EmbeddedVars.Add(vs);
            }
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Value.HaveValue) {
                var val = m_Value.Value;
                bool success = true;
                var setVars = new Dictionary<string, BoxedValue>();
                MatchRecursively(ref success, setVars, val, m_VarIds, 0);
                if (success) {
                    foreach (var pair in setVars) {
                        instance.SetVariable(pair.Key, pair.Value);
                    }
                }
                else {
                    LogSystem.Warn("Tuple doesnt match. line:{0}", m_Line);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                Dsl.ISyntaxComponent param1 = callData.GetParam(0);
                var vars = param1 as Dsl.FunctionData;
                if (null != vars) {
                    int pnum = vars.GetParamNum();
                    LoadRecursively(vars, m_VarIds);
                }
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private void LoadRecursively(Dsl.FunctionData vars, List<ValueTuple<string, int>> varIds)
        {
            m_Line = vars.GetLine();
            int num = vars.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var p = vars.GetParam(i);
                var pvd = p as Dsl.ValueData;
                var pfd = p as Dsl.FunctionData;
                if (null != pvd) {
                    varIds.Add(ValueTuple.Create(pvd.GetId(), -1));
                }
                else if (null != pfd && !pfd.HaveId()) {
                    m_EmbeddedVars.Add(new List<ValueTuple<string, int>>());
                    int index = m_EmbeddedVars.Count - 1;
                    varIds.Add(ValueTuple.Create(string.Empty, index));
                    LoadRecursively(pfd, m_EmbeddedVars[index]);
                }
                else {
                    LogSystem.Warn("invalid tuple member {0}. code:{1} line:{2}", i, p.ToScriptString(false, Dsl.DelimiterInfo.Default), p.GetLine());
                }
            }
        }
        private void MatchRecursively(ref bool success, Dictionary<string, BoxedValue> setVars, BoxedValue val, List<ValueTuple<string, int>> varIds, int start)
        {
            int num = varIds.Count - start;
            if (num == 1) {
                //tuple1 may be a single value, in order to use it both for tuple1 and for normal parentheses usage
                if (val.IsTuple) {
                    var tuple1 = val.GetTuple1();
                    if (null != tuple1) {
                        val = tuple1.Item1;
                    }
                }
                MatchItem(ref success, setVars, varIds[start], val);
            }
            else {
                switch (val.Type) {
                    case BoxedValue.c_Tuple2Type:
                        if (num == 2) {
                            var tuple = val.GetTuple2();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple3Type:
                        if (num == 3) {
                            var tuple = val.GetTuple3();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple4Type:
                        if (num == 4) {
                            var tuple = val.GetTuple4();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple5Type:
                        if (num == 5) {
                            var tuple = val.GetTuple5();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple6Type:
                        if (num == 6) {
                            var tuple = val.GetTuple6();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple7Type:
                        if (num == 7) {
                            var tuple = val.GetTuple7();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                            MatchItem(ref success, setVars, varIds[start + 6], tuple.Item7);
                        }
                        else {
                            success = false;
                        }
                        break;
                    case BoxedValue.c_Tuple8Type:
                        if (num >= 8) {
                            var tuple = val.GetTuple8();
                            MatchItem(ref success, setVars, varIds[start + 0], tuple.Item1);
                            MatchItem(ref success, setVars, varIds[start + 1], tuple.Item2);
                            MatchItem(ref success, setVars, varIds[start + 2], tuple.Item3);
                            MatchItem(ref success, setVars, varIds[start + 3], tuple.Item4);
                            MatchItem(ref success, setVars, varIds[start + 4], tuple.Item5);
                            MatchItem(ref success, setVars, varIds[start + 5], tuple.Item6);
                            MatchItem(ref success, setVars, varIds[start + 6], tuple.Item7);
                            MatchRecursively(ref success, setVars, tuple.Rest.Item1, varIds, start + 7);
                        }
                        else {
                            success = false;
                        }
                        break;
                }
            }
        }
        private void MatchItem(ref bool success, Dictionary<string, BoxedValue> setVars, ValueTuple<string, int> var, BoxedValue val)
        {
            string varId = var.Item1;
            if (string.IsNullOrEmpty(varId)) {
                int index = var.Item2;
                if (index >= 0 && index < m_EmbeddedVars.Count) {
                    var newVarIds = m_EmbeddedVars[index];
                    MatchRecursively(ref success, setVars, val, newVarIds, 0);
                }
                else {
                    success = false;
                }
            }
            else {
                setVars[varId] = val;
            }
        }

        private int m_Line = 0;
        private List<ValueTuple<string, int>> m_VarIds = new List<ValueTuple<string, int>>();
        private List<List<ValueTuple<string, int>>> m_EmbeddedVars = new List<List<ValueTuple<string, int>>>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// assign(@local,value);
    /// or
    /// assign(@@global,value);
    /// or
    /// assign($var,value);
    /// </summary>
    public sealed class AssignCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AssignCommand cmd = new AssignCommand();
            cmd.m_VarName = m_VarName;
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Value.HaveValue) {
                instance.SetVariable(m_VarName, m_Value.Value);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_VarName = callData.GetParamId(0);
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private string m_VarName = null;
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// inc(@local,value);
    /// or
    /// inc(@@global,value);
    /// or
    /// inc($var,value);
    /// </summary>
    public sealed class IncCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            IncCommand cmd = new IncCommand();
            cmd.m_VarName = m_VarName;
            cmd.m_Value = m_Value.Clone();
            cmd.m_ParamNum = m_ParamNum;
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_VarName.StartsWith("@@")) {
                if (null != instance.GlobalVariables) {
                    if (instance.GlobalVariables.ContainsKey(m_VarName)) {
                        var oval = instance.GlobalVariables[m_VarName];
                        if (oval.IsInteger) {
                            int ov = oval.GetInt();
                            if (m_ParamNum > 1 && m_Value.HaveValue) {
                                int v = m_Value.Value;
                                ov += v;
                                instance.GlobalVariables[m_VarName] = ov;
                            } else {
                                ++ov;
                                instance.GlobalVariables[m_VarName] = ov;
                            }
                        } else {
                            float ov = oval.GetFloat();
                            if (m_ParamNum > 1 && m_Value.HaveValue) {
                                float v = m_Value.Value;
                                ov += v;
                                instance.GlobalVariables[m_VarName] = ov;
                            } else {
                                ++ov;
                                instance.GlobalVariables[m_VarName] = ov;
                            }
                        }
                    }
                }
            } else if (m_VarName.StartsWith("@")) {
                if (instance.LocalVariables.ContainsKey(m_VarName)) {
                    var oval = instance.LocalVariables[m_VarName];
                    if (oval.IsInteger) {
                        int ov = oval.GetInt();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            int v = m_Value.Value;
                            ov += v;
                            instance.LocalVariables[m_VarName] = ov;
                        } else {
                            ++ov;
                            instance.LocalVariables[m_VarName] = ov;
                        }
                    } else {
                        float ov = oval.GetFloat();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            float v = m_Value.Value;
                            ov += v;
                            instance.LocalVariables[m_VarName] = ov;
                        } else {
                            ++ov;
                            instance.LocalVariables[m_VarName] = ov;
                        }
                    }
                }
            } else if (m_VarName.StartsWith("$")) {
                if (instance.StackVariables.ContainsKey(m_VarName)) {
                    var oval = instance.StackVariables[m_VarName];
                    if (oval.IsInteger) {
                        int ov = oval.GetInt();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            int v = m_Value.Value;
                            ov += v;
                            instance.StackVariables[m_VarName] = ov;
                        } else {
                            ++ov;
                            instance.StackVariables[m_VarName] = ov;
                        }
                    } else {
                        float ov = oval.GetFloat();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            float v = m_Value.Value;
                            ov += v;
                            instance.StackVariables[m_VarName] = ov;
                        } else {
                            ++ov;
                            instance.StackVariables[m_VarName] = ov;
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            m_ParamNum = num;
            if (num > 0) {
                m_VarName = callData.GetParamId(0);
            }
            if (num > 1) {
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private int m_ParamNum = 0;
        private string m_VarName = null;
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// dec(@local,value);
    /// or
    /// dec(@@global,value);
    /// or
    /// dec($var,value);
    /// </summary>
    public sealed class DecCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DecCommand cmd = new DecCommand();
            cmd.m_VarName = m_VarName;
            cmd.m_Value = m_Value.Clone();
            cmd.m_ParamNum = m_ParamNum;
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_VarName.StartsWith("@@")) {
                if (null != instance.GlobalVariables) {
                    if (instance.GlobalVariables.ContainsKey(m_VarName)) {
                        var oval = instance.GlobalVariables[m_VarName];
                        if (oval.IsInteger) {
                            int ov = oval.GetInt();
                            if (m_ParamNum > 1 && m_Value.HaveValue) {
                                int v = m_Value.Value;
                                ov -= v;
                                instance.GlobalVariables[m_VarName] = ov;
                            } else {
                                --ov;
                                instance.GlobalVariables[m_VarName] = ov;
                            }
                        } else {
                            float ov = oval.GetFloat();
                            if (m_ParamNum > 1 && m_Value.HaveValue) {
                                float v = m_Value.Value;
                                ov -= v;
                                instance.GlobalVariables[m_VarName] = ov;
                            } else {
                                --ov;
                                instance.GlobalVariables[m_VarName] = ov;
                            }
                        }
                    }
                }
            } else if (m_VarName.StartsWith("@")) {
                if (instance.LocalVariables.ContainsKey(m_VarName)) {
                    var oval = instance.LocalVariables[m_VarName];
                    if (oval.IsInteger) {
                        int ov = oval.GetInt();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            int v = m_Value.Value;
                            ov -= v;
                            instance.LocalVariables[m_VarName] = ov;
                        } else {
                            --ov;
                            instance.LocalVariables[m_VarName] = ov;
                        }
                    } else {
                        float ov = oval.GetFloat();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            float v = m_Value.Value;
                            ov -= v;
                            instance.LocalVariables[m_VarName] = ov;
                        } else {
                            --ov;
                            instance.LocalVariables[m_VarName] = ov;
                        }
                    }
                }
            } else if (m_VarName.StartsWith("$")) {
                if (instance.StackVariables.ContainsKey(m_VarName)) {
                    var oval = instance.StackVariables[m_VarName];
                    if (oval.IsInteger) {
                        int ov = oval.GetInt();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            int v = m_Value.Value;
                            ov -= v;
                            instance.StackVariables[m_VarName] = ov;
                        } else {
                            --ov;
                            instance.StackVariables[m_VarName] = ov;
                        }
                    } else {
                        float ov = oval.GetFloat();
                        if (m_ParamNum > 1 && m_Value.HaveValue) {
                            float v = m_Value.Value;
                            ov -= v;
                            instance.StackVariables[m_VarName] = ov;
                        } else {
                            --ov;
                            instance.StackVariables[m_VarName] = ov;
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            m_ParamNum = num;
            if (num > 0) {
                m_VarName = callData.GetParamId(0);
            }
            if (num > 1) {
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private int m_ParamNum = 0;
        private string m_VarName = null;
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// propset(name,value);
    /// </summary>
    public sealed class PropSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PropSetCommand cmd = new PropSetCommand();
            cmd.m_VarName = m_VarName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_VarName.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_VarName.HaveValue && m_Value.HaveValue) {
                string varName = m_VarName.Value;
                instance.SetVariable(varName, m_Value.Value);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_VarName.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryFunction<string> m_VarName = new StoryFunction<string>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// listset(list,index,value);
    /// </summary>
    public sealed class ListSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListSetCommand cmd = new ListSetCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            cmd.m_IndexValue = m_IndexValue.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
            m_IndexValue.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue && m_IndexValue.HaveValue && m_Value.HaveValue) {
                IList listValue = m_ListValue.Value;
                int index = m_IndexValue.Value;
                if (listValue is List<BoxedValue> bvList) {
                    var val = m_Value.Value;
                    int ct = listValue.Count;
                    if (index >= 0 && index < ct) {
                        bvList[index] = val;
                    }
                    else {
                        bvList.Add(val);
                    }
                }
                else {
                    object val = m_Value.Value.GetObject();
                    int ct = listValue.Count;
                    if (index >= 0 && index < ct) {
                        listValue[index] = val;
                    }
                    else {
                        listValue.Add(val);
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
                m_IndexValue.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }
        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryFunction<int>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// listadd(list,value);
    /// </summary>
    public sealed class ListAddCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListAddCommand cmd = new ListAddCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue && m_Value.HaveValue) {
                IList listValue = m_ListValue.Value;
                if (listValue is List<BoxedValue> bvList) {
                    var val = m_Value.Value;
                    bvList.Add(val);
                }
                else {
                    object val = m_Value.Value.GetObject();
                    listValue.Add(val);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// listremove(list,value);
    /// </summary>
    public sealed class ListRemoveCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListRemoveCommand cmd = new ListRemoveCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue && m_Value.HaveValue) {
                IList listValue = m_ListValue.Value;
                if (listValue is List<BoxedValue> bvList) {
                    var val = m_Value.Value;
                    bvList.Remove(val);
                }
                else {
                    object val = m_Value.Value.GetObject();
                    listValue.Remove(val);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// listinsert(list,index,value);
    /// </summary>
    public sealed class ListInsertCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListInsertCommand cmd = new ListInsertCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            cmd.m_IndexValue = m_IndexValue.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
            m_IndexValue.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue && m_IndexValue.HaveValue && m_Value.HaveValue) {
                IList listValue = m_ListValue.Value;
                int index = m_IndexValue.Value;
                if (listValue is List<BoxedValue> bvList) {
                    var val = m_Value.Value;
                    bvList.Insert(index, val);
                }
                else {
                    object val = m_Value.Value.GetObject();
                    listValue.Insert(index, val);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
                m_IndexValue.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }
        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryFunction<int>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// listremoveat(list,index);
    /// </summary>
    public sealed class ListRemoveAtCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListRemoveAtCommand cmd = new ListRemoveAtCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            cmd.m_IndexValue = m_IndexValue.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
            m_IndexValue.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue && m_IndexValue.HaveValue) {
                IList listValue = m_ListValue.Value;
                int index = m_IndexValue.Value;
                listValue.RemoveAt(index);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
                m_IndexValue.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryFunction<int>();
    }
    /// <summary>
    /// listclear(list);
    /// </summary>
    public sealed class ListClearCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ListClearCommand cmd = new ListClearCommand();
            cmd.m_ListValue = m_ListValue.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ListValue.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_ListValue.HaveValue) {
                IList listValue = m_ListValue.Value;
                listValue.Clear();
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ListValue.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<IList> m_ListValue = new StoryFunction<IList>();
    }
}
