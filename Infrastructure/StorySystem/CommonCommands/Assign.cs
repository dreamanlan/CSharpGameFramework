using System;
using System.Collections;
using System.Collections.Generic;
namespace StorySystem.CommonCommands
{
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
        private IStoryFunction m_Value = new StoryValue();
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
        private IStoryFunction m_Value = new StoryValue();
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
        private IStoryFunction m_Value = new StoryValue();
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
        private IStoryFunction<string> m_VarName = new StoryValue<string>();
        private IStoryFunction m_Value = new StoryValue();
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
                object val = m_Value.Value.GetObject();
                int ct = listValue.Count;
                if (index >= 0 && index < ct) {
                    listValue[index] = val;
                } else {
                    listValue.Add(val);
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
        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryValue<int>();
        private IStoryFunction m_Value = new StoryValue();
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
                object val = m_Value.Value.GetObject();
                listValue.Add(val);
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
        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
        private IStoryFunction m_Value = new StoryValue();
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
                object val = m_Value.Value.GetObject();
                listValue.Remove(val);
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
        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
        private IStoryFunction m_Value = new StoryValue();
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
                object val = m_Value.Value.GetObject();
                listValue.Insert(index, val);
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
        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryValue<int>();
        private IStoryFunction m_Value = new StoryValue();
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
        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
        private IStoryFunction<int> m_IndexValue = new StoryValue<int>();
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

        private IStoryFunction<IList> m_ListValue = new StoryValue<IList>();
    }
}
