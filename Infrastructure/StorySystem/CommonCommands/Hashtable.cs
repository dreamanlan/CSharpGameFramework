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
        public override IStoryCommand Clone()
        {
            WriteAllLinesCommand cmd = new WriteAllLinesCommand();
            cmd.m_File = m_File.Clone();
            cmd.m_Val = m_Val.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_File.Evaluate(instance, iterator, args);
            m_Val.Evaluate(instance, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_File.HaveValue && m_Val.HaveValue) {
                string file = m_File.Value;
                IList vals = m_Val.Value as IList;
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
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_File.InitFromDsl(callData.GetParam(0));
                m_Val.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<string> m_File = new StoryValue<string>();
        private IStoryValue<object> m_Val = new StoryValue();
    }
    /// <summary>
    /// writefile(file, val);
    /// </summary>
    internal sealed class WriteFileCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WriteFileCommand cmd = new WriteFileCommand();
            cmd.m_File = m_File.Clone();
            cmd.m_Val = m_Val.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_File.Evaluate(instance, iterator, args);
            m_Val.Evaluate(instance, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_File.HaveValue && m_Val.HaveValue) {
                string file = m_File.Value;
                var val = m_Val.Value;
                if (!string.IsNullOrEmpty(file) && null != val) {
                    File.WriteAllText(file, val.ToString());
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_File.InitFromDsl(callData.GetParam(0));
                m_Val.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<string> m_File = new StoryValue<string>();
        private IStoryValue<object> m_Val = new StoryValue();
    }
    /// <summary>
    /// hashtableadd(hashtable, key, val);
    /// </summary>
    internal sealed class HashtableAddCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HashtableAddCommand cmd = new HashtableAddCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Key.Evaluate(instance, iterator, args);
            m_Value.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value;
                object key = m_Key.Value;
                object val = m_Value.Value;
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict.Add(key, val);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// hashtableset(hashtable,key,val);
    /// </summary>
    internal sealed class HashtableSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HashtableSetCommand cmd = new HashtableSetCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Key.Evaluate(instance, iterator, args);
            m_Value.Evaluate(instance, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value;
                object key = m_Key.Value;
                object val = m_Value.Value;
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict[key] = val;
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// hashtableremove(hashtable,key);
    /// </summary>
    internal sealed class HashtableRemoveCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HashtableRemoveCommand cmd = new HashtableRemoveCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Key.Evaluate(instance, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue) {
                object obj = m_Var.Value;
                object key = m_Key.Value;
                var dict = obj as IDictionary;
                if (null != dict && null != key) {
                    dict.Remove(key);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
    }
    /// <summary>
    /// hashtableclear(hashtable);
    /// </summary>
    internal sealed class HashtableClearCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HashtableClearCommand cmd = new HashtableClearCommand();
            cmd.m_Var = m_Var.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue) {
                object obj = m_Var.Value;
                var dict = obj as IDictionary;
                if (null != dict) {
                    dict.Clear();
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Var.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<object> m_Var = new StoryValue();
    }
}
