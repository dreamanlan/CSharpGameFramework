using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using GameFramework;
using GameFramework.Skill;
using GameFrameworkMessage;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// preload(objresid1,objresid2,...);
    /// </summary>
    internal class PreloadCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PreloadCommand cmd = new PreloadCommand();
            for (int i = 0; i < m_ObjResIds.Count; i++) {
                cmd.m_ObjResIds.Add(m_ObjResIds[i].Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_ObjResIds.Count; i++) {
                m_ObjResIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_ObjResIds.Count; i++) {
                m_ObjResIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                List<int> resIds = new List<int>();
                for (int i = 0; i < m_ObjResIds.Count; i++) {
                    int resId = m_ObjResIds[i].Value;
                    resIds.Add(resId);
                    PreloadActorAndSkills(scene, resId);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<int> val = new StoryValue<int>();
                val.InitFromDsl(callData.GetParam(i));
                m_ObjResIds.Add(val);
            }
        }

        private void PreloadActorAndSkills(Scene scene, int resId)
        {
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(resId);
            if (null != cfg) {
                int[] skillIds = new int[] { cfg.skill0, cfg.skill1, cfg.skill2, cfg.skill3, cfg.skill4, cfg.skill5, cfg.skill6, cfg.skill7, cfg.skill8, cfg.bornskill, cfg.deadskill };
                for (int ix = 0; ix < skillIds.Length; ++ix) {
                    int skillId = skillIds[ix];
                    if (skillId > 0) {
                        scene.SkillSystem.PreloadSkillInstance(skillId);
                    }
                }
            }
        }

        private List<IStoryValue<int>> m_ObjResIds = new List<IStoryValue<int>>();
    }
    /// <summary>
    /// startstory(story_id);
    /// </summary>
    internal class StartStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            StartStoryCommand cmd = new StartStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                scene.DelayActionProcessor.QueueAction(scene.StorySystem.StartStory, m_StoryId.Value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<string> m_StoryId = new StoryValue<string>();
    }
    /// <summary>
    /// stopstory(story_id);
    /// </summary>
    internal class StopStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            StopStoryCommand cmd = new StopStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                scene.StorySystem.MarkStoryTerminated(m_StoryId.Value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<string> m_StoryId = new StoryValue<string>();
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitStoryCommand cmd = new WaitStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
            m_CurTime = 0;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            bool ret = false;
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    ct += scene.StorySystem.CountStory(m_StoryIds[i].Value);
                }
                if (ct <= 0) {
                    string varName = m_SetVar.Value;
                    object varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int timeout = m_TimeoutVal.Value;
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (timeout <= 0 || curTime <= timeout) {
                        ret = true;
                    } else {
                        string varName = m_TimeoutSetVar.Value;
                        object varVal = m_TimeoutSetVal.Value;
                        instance.SetVariable(varName, varVal);
                    }
                }
            }
            return ret;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }

        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }

        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pausestory(storyid1,storyid2,...);
    /// </summary>
    internal class PauseStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PauseStoryCommand cmd = new PauseStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    scene.StorySystem.PauseStory(m_StoryIds[i].Value, true);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
        }

        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// resumestory(storyid1,storyid2,...);
    /// </summary>
    internal class ResumeStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ResumeStoryCommand cmd = new ResumeStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    scene.StorySystem.PauseStory(m_StoryIds[i].Value, false);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
        }

        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    internal class FireMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            FireMessageCommand cmd = new FireMessageCommand();
            cmd.m_MsgId = m_MsgId.Clone();
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                cmd.m_MsgArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_MsgId.Substitute(iterator, args);
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_MsgId.Evaluate(instance);
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string msgId = m_MsgId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_MsgArgs.Count; ++i) {
                    IStoryValue<object> val = m_MsgArgs[i];
                    arglist.Add(val.Value);
                }
                object[] args = arglist.ToArray();
                scene.StorySystem.SendMessage(msgId, args);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_MsgId.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgArgs.Add(val);
            }
        }

        private IStoryValue<string> m_MsgId = new StoryValue<string>();
        private List<IStoryValue<object>> m_MsgArgs = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitAllMessageCommand cmd = new WaitAllMessageCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
            m_CurTime = 0;
            m_StartTime = 0;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            bool ret = false;
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_StartTime <= 0) {
                    long startTime = GameFramework.TimeUtility.GetLocalMilliseconds();
                    m_StartTime = startTime;
                }
                bool triggered = false;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    long time = instance.GetMessageTriggerTime(m_MsgIds[i].Value);
                    if (time > m_StartTime) {
                        triggered = true;
                        break;
                    }
                }
                if (triggered) {
                    string varName = m_SetVar.Value;
                    object varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int timeout = m_TimeoutVal.Value;
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (timeout <= 0 || curTime <= timeout) {
                        ret = true;
                    } else {
                        string varName = m_TimeoutSetVar.Value;
                        object varVal = m_TimeoutSetVal.Value;
                        instance.SetVariable(varName, varVal);
                    }
                }
            }
            return ret;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }

        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitAllMessageHandlerCommand cmd = new WaitAllMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
            m_CurTime = 0;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            bool ret = false;
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int ct = 0;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += scene.StorySystem.CountMessage(m_MsgIds[i].Value);
                }
                if (ct <= 0) {
                    string varName = m_SetVar.Value;
                    object varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int timeout = m_TimeoutVal.Value;
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (timeout <= 0 || curTime <= timeout) {
                        ret = true;
                    } else {
                        string varName = m_TimeoutSetVar.Value;
                        object varVal = m_TimeoutSetVal.Value;
                        instance.SetVariable(varName, varVal);
                    }
                }
            }
            return ret;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }

        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pauseallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal class PauseAllMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PauseAllMessageHandlerCommand cmd = new PauseAllMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    scene.StorySystem.PauseMessageHandler(m_MsgIds[i].Value, true);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// resumeallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal class ResumeAllMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ResumeAllMessageHandlerCommand cmd = new ResumeAllMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    scene.StorySystem.PauseMessageHandler(m_MsgIds[i].Value, false);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendServerStoryMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendServerStoryMessageCommand cmd = new SendServerStoryMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string _msg = m_Msg.Value;

                Msg_LRL_StoryMessage msg = new Msg_LRL_StoryMessage();
                msg.MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.NULL;
                        arg.str_val = "";
                        msg.Args.Add(arg);
                    } else if (v is int) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.Args.Add(arg);
                    } else if (v is float) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
                        msg.Args.Add(arg);
                    } else {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.Args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            msg.UserGuid = us.Guid;
                            scene.GetRoom().SendServerMessage(msg);
                        }
                    }
                } else {
                    scene.GetRoom().SendServerMessage(msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendClientStoryMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendClientStoryMessageCommand cmd = new SendClientStoryMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string _msg = m_Msg.Value;

                Msg_CRC_StoryMessage msg = new Msg_CRC_StoryMessage();
                msg.m_MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.m_Args.Add(arg);
                    } else if (v is int) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.m_Args.Add(arg);
                    } else if (v is float) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
                        msg.m_Args.Add(arg);
                    } else {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.m_Args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class PublishGfxEventCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PublishGfxEventCommand cmd = new PublishGfxEventCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_Group = m_Group.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Substitute(iterator, args);
            m_EventName.Substitute(iterator, args);
            m_Group.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance);
            m_EventName.Evaluate(instance);
            m_Group.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string evname = m_EventName.Value;
                string group = m_Group.Value;

                Msg_RC_PublishEvent msg = new Msg_RC_PublishEvent();
                msg.group = group;
                msg.ev_name = evname;
                msg.is_logic_event = false;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_PublishEvent, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_PublishEvent, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_EventName.InitFromDsl(callData.GetParam(0));
                m_Group.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<string> m_EventName = new StoryValue<string>();
        private IStoryValue<string> m_Group = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendGfxMessageCommand cmd = new SendGfxMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_ObjName = m_ObjName.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Substitute(iterator, args);
            m_ObjName.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance);
            m_ObjName.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string objname = m_ObjName.Value;
                string _msg = m_Msg.Value;

                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = false;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjName.InitFromDsl(callData.GetParam(0));
                m_Msg.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<string> m_ObjName = new StoryValue<string>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageWithTagCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendGfxMessageWithTagCommand cmd = new SendGfxMessageWithTagCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_ObjTag = m_ObjTag.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Substitute(iterator, args);
            m_ObjTag.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance);
            m_ObjTag.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string objname = m_ObjTag.Value;
                string _msg = m_Msg.Value;

                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = true;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjTag.InitFromDsl(callData.GetParam(0));
                m_Msg.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<string> m_ObjTag = new StoryValue<string>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendskillmessage(actorid,skillid,seq,msg,arg1,arg2,...);
    /// </summary>
    internal class SendSkillMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendSkillMessageCommand cmd = new SendSkillMessageCommand();
            cmd.m_ActorId = m_ActorId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            cmd.m_Seq = m_Seq.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ActorId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
            m_Seq.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ActorId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
            m_Seq.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int actorId = m_ActorId.Value;
                int skillId = m_SkillId.Value;
                int seq = m_Seq.Value;
                string msg = m_Msg.Value;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Value as string;
                    object val = m_Args[i + 1].Value;
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                scene.SkillSystem.SendMessage(actorId, skillId, seq, msg, locals);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 3) {
                m_ActorId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
                m_Seq.InitFromDsl(callData.GetParam(2));
                m_Msg.InitFromDsl(callData.GetParam(3));
            }
            for (int i = 4; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        private IStoryValue<int> m_ActorId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
        private IStoryValue<int> m_Seq = new StoryValue<int>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// gfxclear();
    /// </summary>
    internal class GfxClearCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            GfxClearCommand cmd = new GfxClearCommand();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
        }

        protected override void Evaluate(StoryInstance instance)
        {
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                scene.StorySystem.GfxDatas.Clear();
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
        }
    }
    /// <summary>
    /// gfxset(name,value);
    /// </summary>
    internal class GfxSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            GfxSetCommand cmd = new GfxSetCommand();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_AttrName.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_AttrName.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string name = m_AttrName.Value;
                object value = m_Value.Value;
                if (scene.StorySystem.GfxDatas.ContainsKey(name)) {
                    scene.StorySystem.GfxDatas[name] = value;
                } else {
                    scene.StorySystem.GfxDatas.Add(name, value);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_AttrName.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// activescene(target_scene_id, obj_id);
    /// </summary>
    internal class ActiveSceneCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ActiveSceneCommand cmd = new ActiveSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_TargetSceneId.Substitute(iterator, args);
            m_ObjId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_TargetSceneId.Evaluate(instance);
            m_ObjId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int sceneId = m_TargetSceneId.Value;
                object idObj = m_ObjId.Value;
                if (idObj is int) {
                    int objId = (int)idObj;
                    EntityInfo entity = scene.GetEntityById(objId);
                    if (null != entity) {
                        User us = entity.CustomData as User;
                        if (null != us) {
                            RoomServer.RoomServer.Instance.PlayerRequestActiveRoom(sceneId, us.Guid);
                        }
                    }
                } else {
                    IList<int> list = idObj as IList<int>;
                    if (null != list) {
                        List<ulong> guids = new List<ulong>();
                        for (int i = 0; i < list.Count; ++i) {
                            EntityInfo entity = scene.GetEntityById(list[i]);
                            if (null != entity) {
                                User us = entity.CustomData as User;
                                if (null != us) {
                                    guids.Add(us.Guid);
                                }
                            }
                        }
                        if (guids.Count > 0) {
                            RoomServer.RoomServer.Instance.PlayerRequestActiveRoom(sceneId, guids.ToArray());
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
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
                m_ObjId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_TargetSceneId = new StoryValue<int>();
        private IStoryValue<object> m_ObjId = new StoryValue();
    }
    /// <summary>
    /// changescene(target_scene_id, obj_id);
    /// </summary>
    internal class ChangeSceneCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ChangeSceneCommand cmd = new ChangeSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_TargetSceneId.Substitute(iterator, args);
            m_ObjId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_TargetSceneId.Evaluate(instance);
            m_ObjId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int sceneId = m_TargetSceneId.Value;
                object idObj = m_ObjId.Value;
                if (idObj is int) {
                    int objId = (int)idObj;
                    EntityInfo entity = scene.GetEntityById(objId);
                    if (null != entity) {
                        User us = entity.CustomData as User;
                        if (null != us) {
                            RoomServer.RoomServer.Instance.PlayerRequestChangeRoom(sceneId, us.Guid);
                        }
                    }
                } else {
                    IList<int> list = idObj as IList<int>;
                    if (null != list) {
                        List<ulong> guids = new List<ulong>();
                        for (int i = 0; i < list.Count; ++i) {
                            EntityInfo entity = scene.GetEntityById(list[i]);
                            if (null != entity) {
                                User us = entity.CustomData as User;
                                if (null != us) {
                                    guids.Add(us.Guid);
                                }
                            }
                        }
                        if (guids.Count > 0) {
                            RoomServer.RoomServer.Instance.PlayerRequestChangeRoom(sceneId, guids.ToArray());
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
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
                m_ObjId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_TargetSceneId = new StoryValue<int>();
        private IStoryValue<object> m_ObjId = new StoryValue();
    }
    /// <summary>
    /// changeroomscene(target_scene_id);
    /// </summary>
    internal class ChangeRoomSceneCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ChangeRoomSceneCommand cmd = new ChangeRoomSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_TargetSceneId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_TargetSceneId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int targetSceneId = m_TargetSceneId.Value;
                RoomServer.RoomServer.Instance.ChangeRoomScene(scene.GetRoom().RoomID, targetSceneId);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_TargetSceneId = new StoryValue<int>();
    }
    /// <summary>
    /// notifychangescene(target_scene_id);
    /// </summary>
    internal class NotifyChangeSceneCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NotifyChangeSceneCommand cmd = new NotifyChangeSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_TargetSceneId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_TargetSceneId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int targetSceneId = m_TargetSceneId.Value;
                Msg_RC_ChangeScene msg = new Msg_RC_ChangeScene();
                msg.target_scene_id = targetSceneId;
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_ChangeScene, msg);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_TargetSceneId = new StoryValue<int>();
    }
    /// <summary>
    /// createscenelogic(config_id,logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    internal class CreateSceneLogicCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CreateSceneLogicCommand cmd = new CreateSceneLogicCommand();
            cmd.m_ConfigId = m_ConfigId.Clone();
            cmd.m_Logic = m_Logic.Clone();
            cmd.m_Params = m_Params.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ConfigId.Substitute(iterator, args);
            m_Logic.Substitute(iterator, args);
            m_Params.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ConfigId.Evaluate(instance);
            m_Logic.Evaluate(instance);
            m_Params.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int configId = m_ConfigId.Value;
                int logicId = m_Logic.Value;
                IEnumerable args = m_Params.Value;
                List<string> list = new List<string>();
                foreach (string arg in args) {
                    list.Add(arg);
                }
                int id = scene.CreateSceneLogic(configId, logicId, list.ToArray());
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ConfigId.InitFromDsl(callData.GetParam(0));
                m_Logic.InitFromDsl(callData.GetParam(1));
                m_Params.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_ConfigId = new StoryValue<int>();
        private IStoryValue<int> m_Logic = new StoryValue<int>();
        private IStoryValue<IEnumerable> m_Params = new StoryValue<IEnumerable>();
    }
    /// <summary>
    /// destroyscenelogic(config_id);
    /// </summary>
    internal class DestroySceneLogicCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DestroySceneLogicCommand cmd = new DestroySceneLogicCommand();
            cmd.m_ConfigId = m_ConfigId.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ConfigId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ConfigId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int configId = m_ConfigId.Value;
                scene.DestroySceneLogicByConfigId(configId);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ConfigId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_ConfigId = new StoryValue<int>();
    }
    /// <summary>
    /// pausescenelogic(scene_logic_config_id,true_or_false);
    /// </summary>
    internal class PauseSceneLogicCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PauseSceneLogicCommand cmd = new PauseSceneLogicCommand();
            cmd.m_SceneLogicConfigId = m_SceneLogicConfigId.Clone();
            cmd.m_Enabled = m_Enabled.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_SceneLogicConfigId.Substitute(iterator, args);
            m_Enabled.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_SceneLogicConfigId.Evaluate(instance);
            m_Enabled.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int cfgId = m_SceneLogicConfigId.Value;
                string enabled = m_Enabled.Value;
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    info.IsLogicPaused = (0 == string.Compare(enabled, "true"));
                } else {
                    LogSystem.Error("pausescenelogic can't find scenelogic {0}", cfgId);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_SceneLogicConfigId.InitFromDsl(callData.GetParam(0));
                m_Enabled.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_SceneLogicConfigId = new StoryValue<int>();
        private IStoryValue<string> m_Enabled = new StoryValue<string>();
    }
    /// <summary>
    /// restarttimeout(scene_logic_config_id[,timeout]);
    /// </summary>
    internal class RestartTimeoutCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            RestartTimeoutCommand cmd = new RestartTimeoutCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_SceneLogicConfigId = m_SceneLogicConfigId.Clone();
            cmd.m_Timeout = m_Timeout.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_SceneLogicConfigId.Substitute(iterator, args);
            if (m_ParamNum > 1)
                m_Timeout.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_SceneLogicConfigId.Evaluate(instance);
            if (m_ParamNum > 1)
                m_Timeout.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int cfgId = m_SceneLogicConfigId.Value;
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    TimeoutLogicInfo data = info.LogicDatas.GetData<TimeoutLogicInfo>();
                    if (null != data) {
                        data.m_IsTriggered = false;
                        data.m_CurTime = 0;
                        if (m_ParamNum > 1) {
                            data.m_Timeout = m_Timeout.Value;
                        }
                    } else {
                        LogSystem.Warn("restarttimeout scenelogic {0} dosen't start, add wait command !", cfgId);
                    }
                } else {
                    LogSystem.Error("restarttimeout can't find scenelogic {0}", cfgId);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 0) {
                m_SceneLogicConfigId.InitFromDsl(callData.GetParam(0));
            }
            if (m_ParamNum > 1) {
                m_Timeout.InitFromDsl(callData.GetParam(1));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_SceneLogicConfigId = new StoryValue<int>();
        private IStoryValue<int> m_Timeout = new StoryValue<int>();
    }
    /// <summary>
    /// highlightprompt(objid,dictid,arg1,arg2,...);
    /// </summary>
    internal class HighlightPromptCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HighlightPromptCommand cmd = new HighlightPromptCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                cmd.m_DictArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_DictId.Substitute(iterator, args);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_DictId.Evaluate(instance);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_DictArgs.Count; ++i) {
                    IStoryValue<object> val = m_DictArgs[i];
                    arglist.Add(val.Value);
                }
                object[] args = arglist.ToArray();
                scene.SceneContext.HighlightPrompt(objId, dictId, args);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_DictId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_DictArgs.Add(val);
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_DictId = new StoryValue<string>();
        private List<IStoryValue<object>> m_DictArgs = new List<IStoryValue<object>>();
    }
}
