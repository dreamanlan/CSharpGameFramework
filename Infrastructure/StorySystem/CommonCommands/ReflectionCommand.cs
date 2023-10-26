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
    public sealed class DotnetCallCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DotnetCallCommand cmd = new DotnetCallCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            object obj = m_Object.Value.GetObject();
            var methodObj = m_Method.Value;
            string method = methodObj.IsString ? methodObj.StringVal : null;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value.GetObject());
            }
            object[] args = arglist.ToArray();
            if (null != obj) {
                if (null != method) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && dict.Contains(method) && dict[method] is Delegate) {
                        var d = dict[method] as Delegate;
                        if (null != d) {
                            d.DynamicInvoke(args);
                        }
                    } else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                t.InvokeMember(method, flags, null, null, args);
                            } catch (Exception ex) {
                                GameFramework.LogSystem.Warn("DotnetExec {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                            }
                        } else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForCall(t, method, flags, args);
                                    t.InvokeMember(method, flags, null, obj, args);
                                } catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetExec {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
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
            return true;
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// dotnetset(obj,method,arg1,arg2,...);
    /// </summary>
    public sealed class DotnetSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DotnetSetCommand cmd = new DotnetSetCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            object obj = m_Object.Value.GetObject();
            var methodObj = m_Method.Value;
            string method = methodObj.IsString ? methodObj.StringVal : null;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value.GetObject());
            }
            object[] args = arglist.ToArray();
            if (null != obj) {
                if (null != method) {
                    IDictionary dict = obj as IDictionary;
                    if (null != dict && null == obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic)) {
                        dict[method] = args[0];
                    } else {
                        Type t = obj as Type;
                        if (null != t) {
                            try {
                                BindingFlags flags = BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                GameFramework.Converter.CastArgsForSet(t, method, flags, args);
                                t.InvokeMember(method, flags, null, null, args);
                            } catch (Exception ex) {
                                GameFramework.LogSystem.Warn("DotnetSet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                            }
                        } else {
                            t = obj.GetType();
                            if (null != t) {
                                try {
                                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                                    GameFramework.Converter.CastArgsForSet(t, method, flags, args);
                                    t.InvokeMember(method, flags, null, obj, args);
                                } catch (Exception ex) {
                                    GameFramework.LogSystem.Warn("DotnetSet {0}.{1} Exception:{2}\n{3}", t.Name, method, ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
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
            return true;
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// collectionexec(obj,method,arg1,arg2,...);
    /// </summary>
    public sealed class CollectionCallCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            CollectionCallCommand cmd = new CollectionCallCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            object obj = m_Object.Value.GetObject();
            var methodObj = m_Method.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value.GetObject());
            }
            object[] args = arglist.ToArray();
            if (null != obj) {
                IDictionary dict = obj as IDictionary;
                var mobj = methodObj.GetObject();
                if (null != dict && dict.Contains(mobj)) {
                    var d = dict[mobj] as Delegate;
                    if (null != d) {
                        d.DynamicInvoke(args);
                    }
                }
                else {
                    IEnumerable enumer = obj as IEnumerable;
                    if (null != enumer && methodObj.IsInteger) {
                        int index = methodObj.GetInt();
                        var e = enumer.GetEnumerator();
                        for (int i = 0; i <= index; ++i) {
                            e.MoveNext();
                        }
                        var d = e.Current as Delegate;
                        if (null != d) {
                            d.DynamicInvoke(args);
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
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
            return true;
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// collectionset(obj,method,arg1,arg2,...);
    /// </summary>
    public sealed class CollectionSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            CollectionSetCommand cmd = new CollectionSetCommand();
            cmd.m_Object = m_Object.Clone();
            cmd.m_Method = m_Method.Clone();
            for (int i = 0; i < m_Args.Count; i++) {
                cmd.m_Args.Add(m_Args[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Object.Evaluate(instance, handler, iterator, args);
            m_Method.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; i++) {
                m_Args[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            object obj = m_Object.Value.GetObject();
            var methodObj = m_Method.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_Args.Count; i++) {
                arglist.Add(m_Args[i].Value.GetObject());
            }
            object[] args = arglist.ToArray();
            if (null != obj) {
                IDictionary dict = obj as IDictionary;
                var mobj = methodObj.GetObject();
                if (null != dict && dict.Contains(mobj)) {
                    dict[mobj] = args[0];
                }
                else {
                    IList list = obj as IList;
                    if (null != list && methodObj.IsInteger) {
                        int index = methodObj.GetInt();
                        if (index >= 0 && index < list.Count) {
                            list[index] = args[0];
                        }
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
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
            return true;
        }
        private IStoryValue m_Object = new StoryValue();
        private IStoryValue m_Method = new StoryValue();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// system(file,args);
    /// </summary>
    public sealed class SystemCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SystemCommand cmd = new SystemCommand();
            cmd.m_FileName = m_FileName.Clone();
            cmd.m_Arguments = m_Arguments.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_FileName.Evaluate(instance, handler, iterator, args);
            m_Arguments.Evaluate(instance, handler, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            try {
                Process.Start(m_FileName.Value, m_Arguments.Value);
            } catch (Exception ex) {
                GameFramework.LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_FileName.InitFromDsl(callData.GetParam(0));
                m_Arguments.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue<string> m_FileName = new StoryValue<string>();
        private IStoryValue<string> m_Arguments = new StoryValue<string>();
    }
}
