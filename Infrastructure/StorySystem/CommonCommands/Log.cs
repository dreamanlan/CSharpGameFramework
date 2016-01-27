using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

namespace StorySystem.CommonCommands
{
    /// <summary>
    /// log(format,arg1,arg2,...);
    /// </summary>
    internal class LogCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            LogCommand cmd = new LogCommand();
            cmd.m_Format = m_Format.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                cmd.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Format.Substitute(iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Format.Evaluate(instance);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
        private List<IStoryValue<object>> m_FormatArgs = new List<IStoryValue<object>>();
    }
}
