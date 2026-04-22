using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFrameworkMessage;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFramework.Skill;
using ScriptableFramework.Story;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// Dummy command that does nothing. Accepts any dsl syntax so it can be used
    /// as a placeholder for unimplemented apis.
    /// </summary>
    internal sealed class DummyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.ValueData valData) { return true; }
        protected override bool Load(Dsl.FunctionData funcData) { return true; }
        protected override bool Load(Dsl.StatementData statementData) { return true; }
    }
    /// <summary>
    /// preload(resId1,resId2,...);
    /// </summary>
    internal sealed class PreloadCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            for (int i = 0; i < operands.Count; i++) {
                int resId = operands[i].GetInt();
                PreloadActorAndSkills(resId);
            }
            return BoxedValue.NullObject;
        }
        private void PreloadActorAndSkills(int resId)
        {
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(resId);
            if (null != cfg) {
                ResourceSystem.Instance.PreloadObject(cfg.avatar);
                int[] skillIds = new int[] { cfg.skill0, cfg.skill1, cfg.skill2, cfg.skill3, cfg.skill4, cfg.skill5, cfg.skill6, cfg.skill7, cfg.skill8, cfg.bornskill, cfg.deadskill };
                for (int ix = 0; ix < skillIds.Length; ++ix) {
                    int skillId = skillIds[ix];
                    if (skillId > 0) {
                        GfxSkillSystem.Instance.PreloadSkillInstance(skillId);
                        TableConfig.Skill skillCfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                        if (null != skillCfg) {
                            foreach (var pair in skillCfg.resources) {
                                ResourceSystem.Instance.PreloadObject(pair.Value);
                            }
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// startstory(storyId[,multiple]);
    /// </summary>
    internal sealed class StartStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string storyId = operands[0].GetString();
            int multiple = operands.Count > 1 ? operands[1].GetInt() : 0;
            PluginFramework.Instance.QueueAction(() => {
                if (multiple == 0)
                    GfxStorySystem.Instance.StartStory(storyId);
                else
                    GfxStorySystem.Instance.StartStories(storyId);
            });
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// stopstory(storyId[,multiple]);
    /// </summary>
    internal sealed class StopStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string storyId = operands[0].GetString();
            int multiple = operands.Count > 1 ? operands[1].GetInt() : 0;
            if (multiple == 0)
                GfxStorySystem.Instance.MarkStoryTerminated(storyId);
            else
                GfxStorySystem.Instance.MarkStoriesTerminated(storyId);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// waitstory(storyId1,storyId2,...)[set(var,val)timeoutset(timeout,var,val)][multiple(n)];
    /// </summary>
    internal sealed class WaitStoryCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var inst = Calculator.GetFuncContext<StoryInstance>();
            if (null == inst) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            int multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
            int ct = 0;
            for (int i = 0; i < m_StoryIds.Count; i++) {
                ct += GfxStorySystem.Instance.CountStory(m_StoryIds[i].Calc().GetString());
            }
            if (ct <= 0) {
                if (m_HaveSet) {
                    string varName = m_SetVar.Calc().GetString();
                    var varVal = m_SetVal.Calc();
                    inst.SetVariable(varName, varVal);
                }
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            while (true) {
                yield return null;
                ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    ct += GfxStorySystem.Instance.CountStory(m_StoryIds[i].Calc().GetString());
                }
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().GetString();
                        var varVal = m_SetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
                int curTime = (int)(ScriptableFramework.TimeUtility.GetLocalMilliseconds() - startTime);
                if (StoryConfigManager.Instance.IsStorySkipped || (timeout > 0 && curTime > timeout)) {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().GetString();
                        var varVal = m_TimeoutSetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            } else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            int ct = statementData.Functions.Count;
            if (ct >= 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (ct == 2) {
                    m_HaveMultiple = true;
                    LoadMultiple(second);
                } else if (ct == 3) {
                    Dsl.FunctionData third = statementData.Third.AsFunction;
                    if (null != first && null != second && null != third) {
                        m_HaveSet = true;
                        Load(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                    }
                } else if (ct == 4) {
                    Dsl.FunctionData third = statementData.Third.AsFunction;
                    Dsl.FunctionData last = statementData.Last.AsFunction;
                    if (null != first && null != second && null != third && null != last) {
                        m_HaveSet = true;
                        Load(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                        m_HaveMultiple = true;
                        LoadMultiple(last);
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            if (id == "set") {
                LoadSet(callData);
            } else if (id == "timeoutset") {
                LoadTimeoutSet(callData);
            } else if (id == "multiple") {
                LoadMultiple(callData);
            }
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2 && callData.GetId() == "set") {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
                m_HaveSet = true;
            }
        }
        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3 && callData.GetId() == "timeoutset") {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }
        private void LoadMultiple(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 1 && callData.GetId() == "multiple") {
                m_Multiple = Calculator.Load(callData.GetParam(0));
                m_HaveMultiple = true;
            }
        }
        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// pausestory(storyId1,storyId2,...[,multiple]);
    /// </summary>
    internal sealed class PauseStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int multiple = operands.Count > 0 ? operands[operands.Count - 1].GetInt() : 0;
            if (multiple == 0) {
                for (int i = 0; i < operands.Count; i++) {
                    string storyId = operands[i].GetString();
                    GfxStorySystem.Instance.PauseStory(storyId, true);
                }
            } else {
                for (int i = 0; i < operands.Count - 1; i++) {
                    string storyId = operands[i].GetString();
                    GfxStorySystem.Instance.PauseStories(storyId, true);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// resumestory(storyId1,storyId2,...[,multiple]);
    /// </summary>
    internal sealed class ResumeStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            int multiple = operands.Count > 0 ? operands[operands.Count - 1].GetInt() : 0;
            if (multiple == 0) {
                for (int i = 0; i < operands.Count; i++) {
                    string storyId = operands[i].GetString();
                    GfxStorySystem.Instance.PauseStory(storyId, false);
                }
            } else {
                for (int i = 0; i < operands.Count - 1; i++) {
                    string storyId = operands[i].GetString();
                    GfxStorySystem.Instance.PauseStories(storyId, false);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setparent(objpath,parentpath);
    /// </summary>
    internal sealed class SetParentCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objPathVal = operands[0];
            var parentPathVal = operands[1];
            string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
            string parentPath = parentPathVal.IsString ? parentPathVal.StringVal : null;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            UnityEngine.GameObject parent = parentPathVal.IsObject ? parentPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objPathVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null == parent) {
                if (null != parentPath) {
                    parent = UnityEngine.GameObject.Find(parentPath);
                } else {
                    try {
                        int parentId = parentPathVal.GetInt();
                        parent = PluginFramework.Instance.GetGameObject(parentId);
                    } catch {
                        parent = null;
                    }
                }
            }
            if (null != obj && null != parent) {
                obj.transform.SetParent(parent.transform);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setactive(objpath,active);
    /// </summary>
    internal sealed class SetActiveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objPathVal = operands[0];
            int active = operands[1].GetInt();
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objPathVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                obj.SetActive(active != 0);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// quit();
    /// </summary>
    internal sealed class QuitCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            UnityEngine.Application.Quit();
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// firemessage(msgId,arg1,arg2,...);
    /// </summary>
    internal class FireMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string msgId = operands[0].GetString();
            var args = GfxStorySystem.Instance.NewBoxedValueList();
            for (int i = 1; i < operands.Count; ++i) {
                args.Add(operands[i]);
            }
            GfxStorySystem.Instance.SendMessage(msgId, args);
            const string c_DialogOverPrefix = "dialog_over:";
            if (msgId.StartsWith(c_DialogOverPrefix)) {
                if (!PluginFramework.Instance.IsBattleState) {
                    ScriptableFrameworkMessage.Msg_CR_DlgClosed msg = new ScriptableFrameworkMessage.Msg_CR_DlgClosed();
                    msg.dialog_id = int.Parse(msgId.Substring(c_DialogOverPrefix.Length).Trim());
                    Network.NetworkSystem.Instance.SendMessage(ScriptableFrameworkMessage.RoomMessageDefine.Msg_CR_DlgClosed, msg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// fireconcurrentmessage(msgId,arg1,arg2,...);
    /// </summary>
    internal class FireConcurrentMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string msgId = operands[0].GetString();
            var args = GfxStorySystem.Instance.NewBoxedValueList();
            for (int i = 1; i < operands.Count; ++i) {
                args.Add(operands[i]);
            }
            GfxStorySystem.Instance.SendConcurrentMessage(msgId, args);
            const string c_DialogOverPrefix = "dialog_over:";
            if (msgId.StartsWith(c_DialogOverPrefix)) {
                if (!PluginFramework.Instance.IsBattleState) {
                    ScriptableFrameworkMessage.Msg_CR_DlgClosed msg = new ScriptableFrameworkMessage.Msg_CR_DlgClosed();
                    msg.dialog_id = int.Parse(msgId.Substring(c_DialogOverPrefix.Length).Trim());
                    Network.NetworkSystem.Instance.SendMessage(ScriptableFrameworkMessage.RoomMessageDefine.Msg_CR_DlgClosed, msg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var inst = Calculator.GetFuncContext<StoryInstance>();
            if (null == inst) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
            bool triggered = false;
            for (int i = 0; i < m_MsgIds.Count; i++) {
                long time = inst.GetMessageTriggerTime(m_MsgIds[i].Calc().GetString());
                if (time > startTime) {
                    triggered = true;
                    break;
                }
            }
            if (triggered) {
                if (m_HaveSet) {
                    string varName = m_SetVar.Calc().GetString();
                    var varVal = m_SetVal.Calc();
                    inst.SetVariable(varName, varVal);
                }
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            while (true) {
                yield return null;
                triggered = false;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    long time = inst.GetMessageTriggerTime(m_MsgIds[i].Calc().GetString());
                    if (time > 0) {
                        triggered = true;
                        break;
                    }
                }
                if (triggered) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().GetString();
                        var varVal = m_SetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
                int curTime = (int)(ScriptableFramework.TimeUtility.GetLocalMilliseconds() - startTime);
                if (StoryConfigManager.Instance.IsStorySkipped || (timeout > 0 && curTime > timeout)) {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().GetString();
                        var varVal = m_TimeoutSetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                Dsl.FunctionData third = statementData.Third.AsFunction;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }
        private List<IExpression> m_MsgIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageHandlerCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var inst = Calculator.GetFuncContext<StoryInstance>();
            if (null == inst) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
            int ct = 0;
            for (int i = 0; i < m_MsgIds.Count; i++) {
                ct += GfxStorySystem.Instance.CountMessage(m_MsgIds[i].Calc().GetString());
            }
            if (ct <= 0) {
                if (m_HaveSet) {
                    string varName = m_SetVar.Calc().GetString();
                    var varVal = m_SetVal.Calc();
                    inst.SetVariable(varName, varVal);
                }
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            while (true) {
                yield return null;
                ct = 0;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += GfxStorySystem.Instance.CountMessage(m_MsgIds[i].Calc().GetString());
                }
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().GetString();
                        var varVal = m_SetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
                int curTime = (int)(ScriptableFramework.TimeUtility.GetLocalMilliseconds() - startTime);
                if (StoryConfigManager.Instance.IsStorySkipped || (timeout > 0 && curTime > timeout)) {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().GetString();
                        var varVal = m_TimeoutSetVal.Calc();
                        inst.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                Dsl.FunctionData third = statementData.Third.AsFunction;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }
        private List<IExpression> m_MsgIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// suspendallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal class SuspendAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            for (int i = 0; i < operands.Count; i++) {
                GfxStorySystem.Instance.SuspendMessageHandler(operands[i].GetString(), true);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// resumeallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal class ResumeAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            for (int i = 0; i < operands.Count; i++) {
                GfxStorySystem.Instance.SuspendMessageHandler(operands[i].GetString(), false);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sendroomstorymessage(msg,arg1,arg2,...);
    /// </summary>
    internal class SendRoomStoryMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string _msg = operands[0].GetString();
            Msg_CRC_StoryMessage msg = new Msg_CRC_StoryMessage();
            msg.m_MsgId = _msg;
            for (int i = 1; i < operands.Count; ++i) {
                object v = operands[i].GetObject();
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
            Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...);
    /// </summary>
    internal class SendServerStoryMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            string _msg = m_Msg.Calc().GetString();
            Msg_CLC_StoryMessage protoData = new Msg_CLC_StoryMessage();
            protoData.m_MsgId = _msg;
            for (int i = 0; i < m_Args.Count; ++i) {
                object v = m_Args[i].Calc().GetObject();
                if (null == v) {
                    Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                    arg.val_type = LobbyArgType.NULL;
                    arg.str_val = "";
                    protoData.m_Args.Add(arg);
                } else if (v is int) {
                    Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                    arg.val_type = LobbyArgType.INT;
                    arg.str_val = ((int)v).ToString();
                    protoData.m_Args.Add(arg);
                } else if (v is float) {
                    Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                    arg.val_type = LobbyArgType.FLOAT;
                    arg.str_val = ((float)v).ToString();
                    protoData.m_Args.Add(arg);
                } else {
                    Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                    arg.val_type = LobbyArgType.STRING;
                    arg.str_val = v.ToString();
                    protoData.m_Args.Add(arg);
                }
            }
            try {
                Network.NodeMessage msg = new Network.NodeMessage(LobbyMessageDefine.Msg_CLC_StoryMessage, Network.UserNetworkSystem.Instance.Guid);
                msg.m_ProtoData = protoData;
                Network.NodeMessageDispatcher.SendMessage(msg);
            } catch (Exception ex) {
                LogSystem.Error("LobbyNetworkSystem.SendMessage throw Exception: {0}\n{1}", ex.Message, ex.StackTrace);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg = Calculator.Load(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...);
    /// </summary>
    internal class PublishGfxEventCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            string evName = m_EventName.Calc().GetString();
            string group = m_Group.Calc().GetString();
            List<object> arglist = new List<object>();
            for (int i = 0; i < m_Args.Count; ++i) {
                arglist.Add(m_Args[i].Calc().GetObject());
            }
            Utility.EventSystem.Publish(evName, group, arglist.ToArray());
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_EventName = Calculator.Load(callData.GetParam(0));
                m_Group = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    // Client side ignores 'touser(...)' qualifier; execution is local.
                }
            }
            return true;
        }
        private IExpression m_EventName;
        private IExpression m_Group;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessage(objpath,funcname,arg1,arg2,...);
    /// </summary>
    internal class SendGfxMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            string objPath = m_ObjPath.Calc().GetString();
            string funcName = m_FuncName.Calc().GetString();
            object[] args = new object[m_Args.Count];
            for (int i = 0; i < m_Args.Count; ++i) {
                args[i] = m_Args[i].Calc().GetObject();
            }
            Utility.SendMessage(objPath, funcName, args);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = Calculator.Load(callData.GetParam(0));
                m_FuncName = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    // Client side ignores 'touser(...)' qualifier; execution is local.
                }
            }
            return true;
        }
        private IExpression m_ObjPath;
        private IExpression m_FuncName;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tag,funcname,arg1,arg2,...);
    /// </summary>
    internal class SendGfxMessageWithTagCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            string tag = m_Tag.Calc().GetString();
            string funcName = m_FuncName.Calc().GetString();
            object[] args = new object[m_Args.Count];
            for (int i = 0; i < m_Args.Count; ++i) {
                args[i] = m_Args[i].Calc().GetObject();
            }
            Utility.SendMessageWithTag(tag, funcName, args);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Tag = Calculator.Load(callData.GetParam(0));
                m_FuncName = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    // Client side ignores 'touser(...)' qualifier; execution is local.
                }
            }
            return true;
        }
        private IExpression m_Tag;
        private IExpression m_FuncName;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessagewithgameobject(obj,funcname,arg1,arg2,...);
    /// </summary>
    internal class SendGfxMessageWithGameObjectCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            string funcName = operands[1].GetString();
            object[] args = new object[operands.Count - 2];
            for (int i = 2; i < operands.Count; ++i) {
                args[i - 2] = operands[i].GetObject();
            }
            if (null != obj) {
                obj.SendMessage(funcName, args);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sendskillmessage(objid,skillid,selector,arg1,arg2,...);
    /// </summary>
    internal class SendSkillMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 4)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int skillId = operands[1].GetInt();
            int selector = operands[2].GetInt();
            string msgId = operands.Count > 3 ? operands[3].GetString() : "";
            Dictionary<string, object> locals = null;
            if (operands.Count > 4) {
                locals = new Dictionary<string, object>();
                for (int i = 4; i < operands.Count; i += 2) {
                    if (i + 1 < operands.Count) {
                        string key = operands[i].GetString();
                        object val = operands[i + 1].GetObject();
                        locals[key] = val;
                    }
                }
            }
            GfxSkillSystem.Instance.SendMessage(objId, skillId, selector, msgId, locals);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// creategameobject(name,prefab[,parent]){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));}(obj(varname));
    /// </summary>
    internal class CreateGameObjectCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            string name = m_Name.Calc().GetString();
            string prefab = m_Prefab.Calc().GetString();
            UnityEngine.GameObject obj = ResourceSystem.Instance.NewObject(prefab) as UnityEngine.GameObject;
            if (null != obj) {
                obj.name = name;
                if (m_HaveParent) {
                    object parent = m_Parent.Calc().GetObject();
                    string path = parent as string;
                    if (null != path) {
                        var pobj = UnityEngine.GameObject.Find(path);
                        if (null != pobj) {
                            obj.transform.SetParent(pobj.transform, false);
                        }
                    } else {
                        var pobj = parent as UnityEngine.GameObject;
                        if (null != pobj) {
                            obj.transform.SetParent(pobj.transform, false);
                        }
                    }
                }
                if (m_HavePosition) {
                    var vObj = m_Position.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        obj.transform.localPosition = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveRotation) {
                    var vObj = m_Rotation.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        obj.transform.localEulerAngles = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveScale) {
                    var vObj = m_Scale.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        obj.transform.localScale = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveObj) {
                    string varName = m_ObjVarName.Calc().GetString();
                    var inst = Calculator.GetFuncContext<StoryInstance>();
                    if (null != inst) {
                        inst.SetVariable(varName, BoxedValue.FromObject(obj));
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.IsHighOrder) {
                        LoadVarName(second.LowerOrderFunction);
                    }
                    else {
                        LoadVarName(second);
                    }
                }
                if (null != second && second.HaveStatement()) {
                    foreach (var comp in second.Params) {
                        var cd = comp as Dsl.FunctionData;
                        if (null != cd) {
                            LoadOptional(cd);
                        }
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Name = Calculator.Load(callData.GetParam(0));
                m_Prefab = Calculator.Load(callData.GetParam(1));
                if (num > 2) {
                    m_HaveParent = true;
                    m_Parent = Calculator.Load(callData.GetParam(2));
                }
            }
        }
        private void LoadVarName(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "obj" && callData.GetParamNum() == 1) {
                m_ObjVarName = Calculator.Load(callData.GetParam(0));
                m_HaveObj = true;
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = Calculator.Load(callData);
                else
                    m_Position = Calculator.Load(callData.GetParam(0));
                m_HavePosition = true;
            }
            else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = Calculator.Load(callData);
                else
                    m_Rotation = Calculator.Load(callData.GetParam(0));
                m_HaveRotation = true;
            }
            else if (id == "scale") {
                if (num == 3)
                    m_Scale = Calculator.Load(callData);
                else
                    m_Scale = Calculator.Load(callData.GetParam(0));
                m_HaveScale = true;
            }
        }

        private IExpression m_Name;
        private IExpression m_Prefab;
        private IExpression m_Parent;
        private bool m_HaveParent = false;
        private bool m_HaveObj = false;
        private IExpression m_ObjVarName;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
        private bool m_HavePosition = false;
        private bool m_HaveRotation = false;
        private bool m_HaveScale = false;
    }
    /// <summary>
    /// settransform(name, local_or_world){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));};
    /// </summary>
    internal class SetTransformCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            object objVal = m_ObjPath.Calc().GetObject();
            int localOrWorld = m_LocalOrWorld.Calc().GetInt();
            string objPath = objVal as string;
            UnityEngine.GameObject obj = null;
            if (null != objPath) {
                obj = UnityEngine.GameObject.Find(objPath);
            } else {
                obj = objVal as UnityEngine.GameObject;
                if (null == obj) {
                    try {
                        int id = (int)objVal;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                if (m_HavePosition) {
                    var vObj = m_Position.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        if (0 == localOrWorld)
                            obj.transform.localPosition = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                        else
                            obj.transform.position = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveRotation) {
                    var vObj = m_Rotation.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        if (0 == localOrWorld)
                            obj.transform.localEulerAngles = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                        else
                            obj.transform.eulerAngles = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveScale) {
                    var vObj = m_Scale.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        obj.transform.localScale = new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = Calculator.Load(callData.GetParam(0));
                m_LocalOrWorld = Calculator.Load(callData.GetParam(1));
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = Calculator.Load(callData);
                else
                    m_Position = Calculator.Load(callData.GetParam(0));
                m_HavePosition = true;
            } else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = Calculator.Load(callData);
                else
                    m_Rotation = Calculator.Load(callData.GetParam(0));
                m_HaveRotation = true;
            } else if (id == "scale") {
                if (num == 3)
                    m_Scale = Calculator.Load(callData);
                else
                    m_Scale = Calculator.Load(callData.GetParam(0));
                m_HaveScale = true;
            }
        }

        private IExpression m_ObjPath;
        private IExpression m_LocalOrWorld;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
        private bool m_HavePosition = false;
        private bool m_HaveRotation = false;
        private bool m_HaveScale = false;
    }
    /// <summary>
    /// addtransform(name, local_or_world){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));};
    /// </summary>
    internal class AddTransformCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            object objVal = m_ObjPath.Calc().GetObject();
            int localOrWorld = m_LocalOrWorld.Calc().GetInt();
            string objPath = objVal as string;
            UnityEngine.GameObject obj = null;
            if (null != objPath) {
                obj = UnityEngine.GameObject.Find(objPath);
            } else {
                obj = objVal as UnityEngine.GameObject;
                if (null == obj) {
                    try {
                        int id = (int)objVal;
                        obj = PluginFramework.Instance.GetGameObject(id);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                if (m_HavePosition) {
                    var vObj = m_Position.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        if (0 == localOrWorld)
                            obj.transform.localPosition += new UnityEngine.Vector3(v.X, v.Y, v.Z);
                        else
                            obj.transform.position += new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveRotation) {
                    var vObj = m_Rotation.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        if (0 == localOrWorld)
                            obj.transform.localEulerAngles += new UnityEngine.Vector3(v.X, v.Y, v.Z);
                        else
                            obj.transform.eulerAngles += new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
                if (m_HaveScale) {
                    var vObj = m_Scale.Calc().GetObject() as Vector3Obj;
                    if (null != vObj) {
                        var v = vObj.Value;
                        obj.transform.localScale += new UnityEngine.Vector3(v.X, v.Y, v.Z);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = Calculator.Load(callData.GetParam(0));
                m_LocalOrWorld = Calculator.Load(callData.GetParam(1));
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = Calculator.Load(callData);
                else
                    m_Position = Calculator.Load(callData.GetParam(0));
                m_HavePosition = true;
            }
            else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = Calculator.Load(callData);
                else
                    m_Rotation = Calculator.Load(callData.GetParam(0));
                m_HaveRotation = true;
            }
            else if (id == "scale") {
                if (num == 3)
                    m_Scale = Calculator.Load(callData);
                else
                    m_Scale = Calculator.Load(callData.GetParam(0));
                m_HaveScale = true;
            }
        }

        private IExpression m_ObjPath;
        private IExpression m_LocalOrWorld;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
        private bool m_HavePosition = false;
        private bool m_HaveRotation = false;
        private bool m_HaveScale = false;
    }
    /// <summary>
    /// destroygameobject(objpath);
    /// </summary>
    internal class DestroyGameObjectCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            var pathVal = operands[0];
            string path = pathVal.IsString ? pathVal.StringVal : null;
            if (null != path) {
                var obj = UnityEngine.GameObject.Find(path);
                if (null != obj) {
                    obj.transform.SetParent(null);
                    if (!ResourceSystem.Instance.RecycleObject(obj)) {
                        UnityEngine.GameObject.Destroy(obj);
                    }
                }
            }
            else {
                var obj = pathVal.IsObject ? pathVal.ObjectVal as UnityEngine.GameObject : null;
                if (null != obj) {
                    obj.transform.SetParent(null);
                    if (!ResourceSystem.Instance.RecycleObject(obj)) {
                        UnityEngine.GameObject.Destroy(obj);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setvisible(objpath,visible);
    /// </summary>
    internal class SetVisibleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            int visible = operands[1].GetInt();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                ResourceSystem.Instance.SetVisible(obj, visible != 0);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// putonground(objpath);
    /// </summary>
    internal class PutOnGroundCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                var pos = obj.transform.position;
                Skill.Trigers.TriggerUtil.GetRayCastPosInNavMesh(pos + UnityEngine.Vector3.up * 500, pos + UnityEngine.Vector3.down * 500, ref pos);
                obj.transform.position = pos;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setnavmeshagentenable(objpath,enable);
    /// </summary>
    internal class SetNavMeshAgentEnabledCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            int enable = operands[1].GetInt();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (null != agent) {
                    agent.enabled = enable != 0;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// addcomponent(objpath,componenttype)[obj(varname)];
    /// </summary>
    internal class AddComponentCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            var objVal = m_ObjPath.Calc();
            string componentType = m_ComponentType.Calc().GetString();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                // TODO: implement AddComponent by type name - Utility.AddComponent(obj, componentType);
                var type = System.Type.GetType(componentType);
                var comp = null != type ? obj.AddComponent(type) : null;
                if (m_HaveObj && null != instance && null != comp) {
                    string varName = m_ObjVarName.Calc().GetString();
                    instance.SetVariable(varName, BoxedValue.FromObject(comp));
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = Calculator.Load(callData.GetParam(0));
                m_ComponentType = Calculator.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadVarName(second);
                }
            }
            return true;
        }
        private void LoadVarName(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "obj" && callData.GetParamNum() == 1) {
                m_ObjVarName = Calculator.Load(callData.GetParam(0));
                m_HaveObj = true;
            }
        }
        private IExpression m_ObjPath;
        private IExpression m_ComponentType;
        private bool m_HaveObj = false;
        private IExpression m_ObjVarName;
    }
    /// <summary>
    /// removecomponent(objpath,componenttype);
    /// </summary>
    internal class RemoveComponentCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            string componentType = operands[1].GetString();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                var type = System.Type.GetType(componentType);
                if (null != type) {
                    var comp = obj.GetComponent(type);
                    if (null != comp) {
                        UnityEngine.Object.Destroy(comp);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// installplugin(objpath,pluginname,dslfile,storyid);
    /// </summary>
    internal class InstallPluginCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string objPath = operands[0].AsString;
            string pluginClass = operands[1].AsString;
            int isTickPlugin = operands[2].GetInt();
            int useScriptPlugin = operands[3].GetInt();
            if (useScriptPlugin != 0) {
                if (isTickPlugin != 0)
                    PluginProxy.ScriptProxy.InstallTickPlugin(objPath, pluginClass);
                else
                    PluginProxy.ScriptProxy.InstallStartupPlugin(objPath, pluginClass);
            }
            else {
                if (isTickPlugin != 0)
                    PluginProxy.NativeProxy.InstallTickPlugin(objPath, pluginClass);
                else
                    PluginProxy.NativeProxy.InstallStartupPlugin(objPath, pluginClass);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// removeplugin(objpath,pluginname,dslfile,storyid);
    /// </summary>
    internal class RemovePluginCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string objPath = operands[0].AsString;
            string pluginClass = operands[1].AsString;
            int isTickPlugin = operands[2].GetInt();
            int useScriptPlugin = operands[3].GetInt();
            if (useScriptPlugin != 0) {
                if (isTickPlugin != 0)
                    PluginProxy.ScriptProxy.RemoveTickPlugin(objPath, pluginClass);
                else
                    PluginProxy.ScriptProxy.RemoveStartupPlugin(objPath, pluginClass);
            }
            else {
                if (isTickPlugin != 0)
                    PluginProxy.NativeProxy.RemoveTickPlugin(objPath, pluginClass);
                else
                    PluginProxy.NativeProxy.RemoveStartupPlugin(objPath, pluginClass);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// openurl(url);
    /// </summary>
    internal class OpenUrlCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string url = operands[0].GetString();
            UnityEngine.Application.OpenURL(url);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// changescene(sceneId);
    /// </summary>
    internal class ChangeSceneCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            int sceneId = operands[0].GetInt();
            PluginFramework.Instance.DelayChangeScene(sceneId);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// openbattle(sceneId);
    /// </summary>
    internal class OpenBattleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            int sceneId = operands[0].GetInt();
            PluginFramework.Instance.LoadBattle(sceneId);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// closebattle();
    /// </summary>
    internal class CloseBattleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            int sceneId = operands[0].GetInt();
            PluginFramework.Instance.UnloadBattle(sceneId);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// createscenelogic(scenelogicid,dslfile,storyid);
    /// </summary>
    internal class CreateSceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                return BoxedValue.NullObject;
            int configId = operands[0].GetInt();
            int logicId = operands[1].GetInt();
            var argsVal = operands[2];
            IEnumerable args = argsVal.IsObject ? argsVal.ObjectVal as IEnumerable : null;
            List<string> list = new List<string>();
            if (null != args) {
                foreach (string arg in args) {
                    list.Add(arg);
                }
            }
            PluginFramework.Instance.CreateSceneLogic(configId, logicId, list.ToArray());
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// destroyscenelogic(scenelogicid);
    /// </summary>
    internal class DestroySceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            int configId = operands[0].GetInt();
            PluginFramework.Instance.DestroySceneLogicByConfigId(configId);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// pausescenelogic(scenelogicid,pause);
    /// </summary>
    internal class PauseSceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            int cfgId = operands[0].GetInt();
            string enabled = operands[1].GetString();
            SceneLogicInfo info = PluginFramework.Instance.GetSceneLogicInfoByConfigId(cfgId);
            if (null != info) {
                info.IsLogicPaused = (0 == string.Compare(enabled, "true"));
            } else {
                LogSystem.Error("pausescenelogic can't find scenelogic {0}", cfgId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// restarttimeout(scenelogicid[,storyid,dslfile]);
    /// </summary>
    internal class RestartTimeoutCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            int cfgId = operands[0].GetInt();
            SceneLogicInfo info = PluginFramework.Instance.GetSceneLogicInfoByConfigId(cfgId);
            if (null != info) {
                TimeoutLogicInfo data = info.LogicDatas.GetData<TimeoutLogicInfo>();
                if (null != data) {
                    data.m_IsTriggered = false;
                    data.m_CurTime = 0;
                    if (operands.Count > 1) {
                        data.m_Timeout = operands[1].GetLong();
                    }
                } else {
                    LogSystem.Warn("restarttimeout scenelogic {0} dosen't start, add wait command !", cfgId);
                }
            } else {
                LogSystem.Error("restarttimeout can't find scenelogic {0}", cfgId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// highlightpromptwithdict(key,arg1,arg2,...);
    /// </summary>
    internal class HighlightPromptWithDictCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string key = operands[0].GetString();
            object[] args = new object[operands.Count - 1];
            for (int i = 1; i < operands.Count; ++i) {
                args[i - 1] = operands[i].GetObject();
            }
            PluginFramework.Instance.HighlightPromptWithDict(key, args);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// highlightprompt(prompt);
    /// </summary>
    internal class HighlightPromptCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            string prompt = operands[0].GetString();
            PluginFramework.Instance.HighlightPrompt(prompt);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setactorscale(objpath,scale);
    /// </summary>
    internal class SetActorScaleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            var scaleVal = operands[1];
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                Vector3Obj scaleObj = scaleVal.ObjectVal as Vector3Obj;
                if (null != scaleObj) {
                    obj.transform.localScale = new UnityEngine.Vector3(scaleObj.Value.X, scaleObj.Value.Y, scaleObj.Value.Z);
                } else {
                    float scale = scaleVal.GetFloat();
                    obj.transform.localScale = new UnityEngine.Vector3(scale, scale, scale);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// gameobjectanimation(objpath,animName[,crossFadeTime]);
    /// </summary>
    internal class GameObjectAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            string animName = operands[1].GetString();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                float crossFadeTime = operands.Count > 2 ? operands[2].GetFloat() : 0;
                EntityViewModel view = EntityController.Instance.GetEntityView(obj);
                UnityEngine.Animator animator = null != view ? view.Animator : null;
                if (null == animator) {
                    animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                }
                if (null != animator) {
                    int layerIndex = 0;
                    if (crossFadeTime > 0) {
                        animator.CrossFade(animName, crossFadeTime, layerIndex);
                    } else {
                        animator.Play(animName, layerIndex);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// gameobjectanimationparam(objpath,type(key,val),type(key,val),...);
    /// </summary>
    internal class GameObjectAnimationParamCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var objVal = m_ObjPath.Calc();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    for (int i = 0; i < m_Params.Count; ++i) {
                        var param = m_Params[i];
                        string type = param.Type;
                        string key = param.Key.Calc().GetString();
                        var val = param.Value.Calc();
                        if (type == "int") {
                            int v = val.GetInt();
                            animator.SetInteger(key, v);
                        } else if (type == "float") {
                            float v = val.GetFloat();
                            animator.SetFloat(key, v);
                        } else if (type == "bool") {
                            bool v = val.GetBool();
                            animator.SetBool(key, v);
                        } else if (type == "trigger") {
                            string v = val.GetString();
                            if (v == "false") {
                                animator.ResetTrigger(key);
                            } else {
                                animator.SetTrigger(key);
                            }
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            if (callData.IsHighOrder) {
                LoadCall(callData.LowerOrderFunction);
            } else if (callData.HaveParam()) {
                LoadCall(callData);
            }
            if (callData.HaveStatement()) {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent statement = callData.GetParam(i);
                    Dsl.FunctionData stCall = statement as Dsl.FunctionData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        ParamInfo param = new ParamInfo(Calculator, id, stCall.GetParam(0), stCall.GetParam(1));
                        m_Params.Add(param);
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadParam(second);
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                m_ObjPath = Calculator.Load(callData.GetParam(0));
            }
        }
        private void LoadParam(Dsl.FunctionData callData)
        {
            if (callData.IsHighOrder) {
                LoadCall(callData.LowerOrderFunction);
            } else if (callData.HaveParam()) {
                LoadCall(callData);
            }
            if (callData.HaveStatement()) {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent statement = callData.GetParam(i);
                    Dsl.FunctionData stCall = statement as Dsl.FunctionData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        ParamInfo param = new ParamInfo(Calculator, id, stCall.GetParam(0), stCall.GetParam(1));
                        m_Params.Add(param);
                    }
                }
            }
        }
        private class ParamInfo
        {
            internal string Type;
            internal IExpression Key;
            internal IExpression Value;
            internal ParamInfo(DslCalculator calc, string type, Dsl.ISyntaxComponent keyDsl, Dsl.ISyntaxComponent valDsl)
            {
                Type = type;
                Key = calc.Load(keyDsl);
                Value = calc.Load(valDsl);
            }
        }
        private IExpression m_ObjPath;
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
    /// <summary>
    /// gameobjectcastskill(objpath,skillid,...);
    /// </summary>
    internal class GameObjectCastSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            int skillId = operands[1].GetInt();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                EntityViewModel view = EntityController.Instance.GetEntityView(obj);
                if (null != view) {
                    int objId = view.ObjId;
                    TableConfig.Skill skillData = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != skillData) {
                        GfxSkillSystem.Instance.StartSkill(objId, skillData, 0);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// gameobjectstopskill(objpath);
    /// </summary>
    internal class GameObjectStopSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                } else {
                    try {
                        int objId = objVal.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                EntityViewModel view = EntityController.Instance.GetEntityView(obj);
                if (null != view) {
                    GfxSkillSystem.Instance.StopAllSkillWithGameObject(obj, true);
                }
            }
            return BoxedValue.NullObject;
        }
    }
}
