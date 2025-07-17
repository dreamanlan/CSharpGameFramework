using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;

namespace DotnetStoryScript.CommonCommands
{
    /// <summary>
    /// appendformat(sb, fmt, args);
    /// </summary>
    public sealed class AppendFormatCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AppendFormatCommand cmd = new AppendFormatCommand();
            cmd.m_StringBuilder = m_StringBuilder.Clone();
            cmd.m_Format = m_Format.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                cmd.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_StringBuilder.Evaluate(instance, handler, iterator, args);
            m_Format.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            var sb = m_StringBuilder.Value;
            var formatObj = m_Format.Value;
            string format = formatObj.IsString ? formatObj.StringVal : null;
            if (!string.IsNullOrEmpty(format) && m_FormatArgs.Count > 0) {
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    arglist.Add(m_FormatArgs[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                sb.AppendFormat(format, args);
            }
            else {
                sb.AppendFormat("{0}", formatObj.ToString());
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_StringBuilder.InitFromDsl(callData.GetParam(0));
                m_Format.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryFunction val = new StoryFunction();
                val.InitFromDsl(callData.GetParam(i));
                m_FormatArgs.Add(val);
            }
            return true;
        }

        private IStoryFunction<StringBuilder> m_StringBuilder = new StoryFunction<StringBuilder>();
        private IStoryFunction m_Format = new StoryFunction();
        private List<IStoryFunction> m_FormatArgs = new List<IStoryFunction>();
    }
    /// <summary>
    /// appendformatline(sb, fmt, args);
    /// </summary>
    public sealed class AppendFormatLineCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            AppendFormatLineCommand cmd = new AppendFormatLineCommand();
            cmd.m_StringBuilder = m_StringBuilder.Clone();
            cmd.m_Format = m_Format.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                cmd.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_StringBuilder.Evaluate(instance, handler, iterator, args);
            m_Format.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            var sb = m_StringBuilder.Value;
            var formatObj = m_Format.Value;
            string format = formatObj.IsString ? formatObj.StringVal : null;
            if (!string.IsNullOrEmpty(format) && m_FormatArgs.Count > 0) {
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    arglist.Add(m_FormatArgs[i].Value.GetObject());
                }
                object[] args = arglist.ToArray();
                sb.AppendFormat(format, args);
                sb.AppendLine();
            }
            else {
                sb.AppendFormat("{0}", formatObj.ToString());
                sb.AppendLine();
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_StringBuilder.InitFromDsl(callData.GetParam(0));
                m_Format.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryFunction val = new StoryFunction();
                val.InitFromDsl(callData.GetParam(i));
                m_FormatArgs.Add(val);
            }
            return true;
        }

        private IStoryFunction<StringBuilder> m_StringBuilder = new StoryFunction<StringBuilder>();
        private IStoryFunction m_Format = new StoryFunction();
        private List<IStoryFunction> m_FormatArgs = new List<IStoryFunction>();
    }
    /// <summary>
    /// writealllines(file, val);
    /// </summary>
    public sealed class WriteAllLinesCommand : AbstractStoryCommand
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
        private IStoryFunction<string> m_File = new StoryFunction<string>();
        private IStoryFunction m_Val = new StoryFunction();
    }
    /// <summary>
    /// writefile(file, val);
    /// </summary>
    public sealed class WriteFileCommand : AbstractStoryCommand
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
        private IStoryFunction<string> m_File = new StoryFunction<string>();
        private IStoryFunction m_Val = new StoryFunction();
    }
    /// <summary>
    /// hashtableadd(hashtable, key, val);
    /// </summary>
    public sealed class HashtableAddCommand : AbstractStoryCommand
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
                object obj = m_Var.Value.GetObject();
                var key = m_Key.Value;
                var val = m_Value.Value;
                var dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict.Add(key, val);
                }
                else if (null != dict && null != key.GetObject()) {
                    dict.Add(key.GetObject(), val.GetObject());
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
        private IStoryFunction m_Var = new StoryFunction();
        private IStoryFunction m_Key = new StoryFunction();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// hashtableset(hashtable,key,val);
    /// </summary>
    public sealed class HashtableSetCommand : AbstractStoryCommand
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
                object obj = m_Var.Value.GetObject();
                var key = m_Key.Value;
                var val = m_Value.Value;
                var dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict[key] = val;
                }
                else if (null != dict && null != key.GetObject()) {
                    dict[key.GetObject()] = val.GetObject();
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

        private IStoryFunction m_Var = new StoryFunction();
        private IStoryFunction m_Key = new StoryFunction();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// hashtableremove(hashtable,key);
    /// </summary>
    public sealed class HashtableRemoveCommand : AbstractStoryCommand
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
                object obj = m_Var.Value.GetObject();
                var key = m_Key.Value;
                var dict = obj as IDictionary;
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict.Remove(key);
                }
                else if (null != dict && null != key.GetObject()) {
                    dict.Remove(key.GetObject());
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
        private IStoryFunction m_Var = new StoryFunction();
        private IStoryFunction m_Key = new StoryFunction();
    }
    /// <summary>
    /// hashtableclear(hashtable);
    /// </summary>
    public sealed class HashtableClearCommand : AbstractStoryCommand
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
                object obj = m_Var.Value.GetObject();
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

        private IStoryFunction m_Var = new StoryFunction();
    }
}
