using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// dotnetexec(obj,method,arg1,arg2,...);
    /// </summary>
    internal sealed class DotnetExecCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DotnetExecCommand cmd = new DotnetExecCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Object.Evaluate(instance, iterator, args);
            m_Method.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            object obj = m_Object.Value;
            string method = m_Method.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value);
            }
            object[] args = arglist.ToArray();
            if (null != obj && !string.IsNullOrEmpty(method)) {
                Type t = obj as Type;
                if (null != t) {
                    try {
                        t.InvokeMember(method, BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic, null, null, args);
                    } catch (Exception ex) {
                        GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                } else {                    
                    t = obj.GetType();
                    if (null != t) {
                        try {
                            t.InvokeMember(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic, null, obj, args);
                        } catch (Exception ex) {
                            GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Object.InitFromDsl(callData.GetParam(0));
                m_Method.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }
        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// dotnetset(obj,method,arg1,arg2,...);
    /// </summary>
    internal sealed class DotnetSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DotnetSetCommand cmd = new DotnetSetCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Object.Evaluate(instance, iterator, args);
            m_Method.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            object obj = m_Object.Value;
            string method = m_Method.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value);
            }
            object[] args = arglist.ToArray();
            if (null != obj && !string.IsNullOrEmpty(method)) {
                Type t = obj as Type;
                if (null != t) {
                    try {
                        t.InvokeMember(method, BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, null, args);
                    } catch (Exception ex) {
                        GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                } else {
                    t = obj.GetType();
                    if (null != t) {
                        try {
                            t.InvokeMember(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, obj, args);
                        } catch (Exception ex) {
                            GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Object.InitFromDsl(callData.GetParam(0));
                m_Method.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }
        private IStoryValue<object> m_Object = new StoryValue();
        private IStoryValue<string> m_Method = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// system(file,args);
    /// </summary>
    internal sealed class SystemCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SystemCommand cmd = new SystemCommand();
            cmd.m_FileName = m_FileName.Clone();
            cmd.m_Arguments = m_Arguments.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_FileName.Evaluate(instance, iterator, args);
            m_Arguments.Evaluate(instance, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            try {
                Process.Start(m_FileName.Value, m_Arguments.Value);
            } catch (Exception ex) {
                GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_FileName.InitFromDsl(callData.GetParam(0));
                m_Arguments.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<string> m_FileName = new StoryValue<string>();
        private IStoryValue<string> m_Arguments = new StoryValue<string>();
    }
}
