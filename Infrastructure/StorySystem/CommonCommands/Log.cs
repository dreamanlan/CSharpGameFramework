using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// log(format,arg1,arg2,...);
    /// </summary>
    internal sealed class LogCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            LogCommand cmd = new LogCommand();
            cmd.m_Format = m_Format.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                cmd.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_Format.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            string format = m_Format.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                arglist.Add(m_FormatArgs[i].Value);
            }
            object[] args = arglist.ToArray();
            LogSystem.Warn(m_Format.Value, args);
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Format.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_FormatArgs.Add(val);
            }
        }
        private IStoryValue<string> m_Format = new StoryValue<string>();
        private List<IStoryValue> m_FormatArgs = new List<IStoryValue>();
    }
}
