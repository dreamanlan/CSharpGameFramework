using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace StorySystem.CommonCommands
{
    /// <summary>
    /// writealllines(file, val);
    /// </summary>
    internal sealed class WriteAllLinesCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            WriteAllLinesCommand cmd = new WriteAllLinesCommand();
            cmd.m_File = m_File.Clone();
            cmd.m_Val = m_Val.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_File.Evaluate(instance, handler, iterator, args);
            m_Val.Evaluate(instance, handler, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_File.HaveValue && m_Val.HaveValue) {
                string file = m_File.Value;
                IList vals = m_Val.Value.ObjectVal as IList;
                if (!string.IsNullOrEmpty(file) && null != vals) {
                    List<string> lines = new List<string>();
                    foreach (var obj in vals) {
                        if (null != obj) {
                            lines.Add(obj.ToString());
                        }
                    }
                    File.WriteAllLines(file, lines.ToArray());
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_File.InitFromDsl(callData.GetParam(0));
                m_Val.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue<string> m_File = new StoryValue<string>();
        private IStoryValue m_Val = new StoryValue();
    }
    /// <summary>
    /// writefile(file, val);
    /// </summary>
    internal sealed class WriteFileCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            WriteFileCommand cmd = new WriteFileCommand();
            cmd.m_File = m_File.Clone();
            cmd.m_Val = m_Val.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_File.Evaluate(instance, handler, iterator, args);
            m_Val.Evaluate(instance, handler, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_File.HaveValue && m_Val.HaveValue) {
                string file = m_File.Value;
                var val = m_Val.Value;
                if (!string.IsNullOrEmpty(file)) {
                    File.WriteAllText(file, val.ToString());
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_File.InitFromDsl(callData.GetParam(0));
                m_Val.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue<string> m_File = new StoryValue<string>();
        private IStoryValue m_Val = new StoryValue();
    }
    /// <summary>
    /// hashtableadd(hashtable, key, val);
    /// </summary>
    internal sealed class HashtableAddCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HashtableAddCommand cmd = new HashtableAddCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Var.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                object key = m_Key.Value.Get<object>();
                object val = m_Value.Value.Get<object>();
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict.Add(key, val);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }
        private IStoryValue m_Var = new StoryValue();
        private IStoryValue m_Key = new StoryValue();
        private IStoryValue m_Value = new StoryValue();
    }
    /// <summary>
    /// hashtableset(hashtable,key,val);
    /// </summary>
    internal sealed class HashtableSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HashtableSetCommand cmd = new HashtableSetCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Var.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                object key = m_Key.Value.Get<object>();
                object val = m_Value.Value.Get<object>();
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict[key] = val;
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryValue m_Var = new StoryValue();
        private IStoryValue m_Key = new StoryValue();
        private IStoryValue m_Value = new StoryValue();
    }
    /// <summary>
    /// hashtableremove(hashtable,key);
    /// </summary>
    internal sealed class HashtableRemoveCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HashtableRemoveCommand cmd = new HashtableRemoveCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Var.Evaluate(instance, handler, iterator, args);
            m_Key.Evaluate(instance, handler, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                object key = m_Key.Value.Get<object>();
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict.Remove(key);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue m_Var = new StoryValue();
        private IStoryValue m_Key = new StoryValue();
    }
    /// <summary>
    /// hashtableclear(hashtable);
    /// </summary>
    internal sealed class HashtableClearCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HashtableClearCommand cmd = new HashtableClearCommand();
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Var.Evaluate(instance, handler, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Var.HaveValue) {
                object obj = m_Var.Value.Get<object>();
                var dict = obj as IDictionary;
                if (null != dict) {
                    dict.Clear();
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Var.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryValue m_Var = new StoryValue();
    }
}
