using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// localmessage(msgId, args...) - send a sequential message to the current StoryInstance
    /// </summary>
    internal sealed class LocalMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: localmessage(msgId, args...)");
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = operands[0].ToString();
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// localconcurrentmessage(msgId, args...) - send a concurrent message to the current StoryInstance
    /// </summary>
    internal sealed class LocalConcurrentMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: localconcurrentmessage(msgId, args...)");
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = operands[0].ToString();
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendConcurrentMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storylocalmessage(msgId, args...) - send message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryLocalMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: storylocalmessage(msgId, args...)");
            if (StoryConfigManager.Instance.IsStorySkipped)
                return BoxedValue.NullObject;
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = operands[0].ToString();
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storylocalconcurrentmessage(msgId, args...) - send concurrent message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryLocalConcurrentMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: storylocalconcurrentmessage(msgId, args...)");
            if (StoryConfigManager.Instance.IsStorySkipped)
                return BoxedValue.NullObject;
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = operands[0].ToString();
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendConcurrentMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// clearmessage(msgId1, msgId2, ...) - clear message queues for specified message IDs
    /// </summary>
    internal sealed class ClearMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string[] msgIds = new string[operands.Count];
            for (int i = 0; i < operands.Count; i++) {
                msgIds[i] = operands[i].ToString();
            }
            storyInst.ClearMessage(msgIds);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// waitlocalmessage(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for message trigger
    /// </summary>
    internal sealed class WaitLocalMessageExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            var msgIds = m_MsgIds;
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                bool triggered = false;
                foreach (var msgId in msgIds) {
                    long time = storyInst.GetMessageTriggerTime(msgId);
                    if (time > startTime) {
                        triggered = true;
                        break;
                    }
                }

                if (triggered) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (timeout <= 0 || curTime <= timeout) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) {
                    m_MsgIds.Add(param.GetId());
                }
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// waitlocalmessagehandler(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for message handler completion
    /// </summary>
    internal sealed class WaitLocalMessageHandlerExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                int ct = 0;
                foreach (var msgId in m_MsgIds) {
                    ct += storyInst.CountMessage(msgId);
                }

                if (ct <= 0) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (timeout <= 0 || curTime <= timeout) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// storywaitlocalmessage(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryWaitLocalMessageExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                bool triggered = false;
                foreach (var msgId in m_MsgIds) {
                    long time = storyInst.GetMessageTriggerTime(msgId);
                    if (time > startTime) {
                        triggered = true;
                        break;
                    }
                }

                if (triggered) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// storywaitlocalmessagehandler(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for handler, skipped if story is skipped
    /// </summary>
    internal sealed class StoryWaitLocalMessageHandlerExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                int ct = 0;
                foreach (var msgId in m_MsgIds) {
                    ct += storyInst.CountMessage(msgId);
                }

                if (ct <= 0) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// suspendlocalmessagehandler(msgId1, msgId2, ...) - suspend message handlers
    /// </summary>
    internal sealed class SuspendLocalMessageHandlerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            for (int i = 0; i < operands.Count; i++) {
                storyInst.SuspendMessageHandler(operands[i].ToString(), true);
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// resumelocalmessagehandler(msgId1, msgId2, ...) - resume message handlers
    /// </summary>
    internal sealed class ResumeLocalMessageHandlerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            for (int i = 0; i < operands.Count; i++) {
                storyInst.SuspendMessageHandler(operands[i].ToString(), false);
            }
            return BoxedValue.NullObject;
        }
    }

    // ========== Namespaced Message Expressions ==========

    /// <summary>
    /// localnamespacedmessage(msgId, args...) - send namespaced message
    /// </summary>
    internal sealed class LocalNamespacedMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: localnamespacedmessage(msgId, args...)");
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = MessageExpHelper.BuildNamespacedMessageId(storyInst, operands[0].ToString());
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// localconcurrentnamespacedmessage(msgId, args...) - send concurrent namespaced message
    /// </summary>
    internal sealed class LocalConcurrentNamespacedMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: localconcurrentnamespacedmessage(msgId, args...)");
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = MessageExpHelper.BuildNamespacedMessageId(storyInst, operands[0].ToString());
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendConcurrentMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storylocalnamespacedmessage(msgId, args...) - send namespaced message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryLocalNamespacedMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: storylocalnamespacedmessage(msgId, args...)");
            if (StoryConfigManager.Instance.IsStorySkipped)
                return BoxedValue.NullObject;
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = MessageExpHelper.BuildNamespacedMessageId(storyInst, operands[0].ToString());
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storylocalconcurrentnamespacedmessage(msgId, args...) - send concurrent namespaced message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryLocalConcurrentNamespacedMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: storylocalconcurrentnamespacedmessage(msgId, args...)");
            if (StoryConfigManager.Instance.IsStorySkipped)
                return BoxedValue.NullObject;
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string msgId = MessageExpHelper.BuildNamespacedMessageId(storyInst, operands[0].ToString());
            var args = new BoxedValueList();
            for (int i = 1; i < operands.Count; i++) {
                args.Add(operands[i]);
            }
            storyInst.SendConcurrentMessage(msgId, args);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// clearnamespacedmessage(msgId1, msgId2, ...) - clear namespaced message queues
    /// </summary>
    internal sealed class ClearNamespacedMessageExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string _namespace = storyInst.Namespace;
            string[] msgIds = new string[operands.Count];
            for (int i = 0; i < operands.Count; i++) {
                if (string.IsNullOrEmpty(_namespace)) {
                    msgIds[i] = operands[i].ToString();
                }
                else {
                    msgIds[i] = string.Format("{0}:{1}", _namespace, operands[i].ToString());
                }
            }
            storyInst.ClearMessage(msgIds);
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// waitlocalnamespacedmessage(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for namespaced message trigger
    /// </summary>
    internal sealed class WaitLocalNamespacedMessageExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            var nsMsgIds = MessageExpHelper.BuildNamespacedMessageIds(storyInst, m_MsgIds);
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                bool triggered = false;
                foreach (var msgId in nsMsgIds) {
                    long time = storyInst.GetMessageTriggerTime(msgId);
                    if (time > startTime) {
                        triggered = true;
                        break;
                    }
                }

                if (triggered) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (timeout <= 0 || curTime <= timeout) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// waitlocalnamespacedmessagehandler(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for namespaced message handler
    /// </summary>
    internal sealed class WaitLocalNamespacedMessageHandlerExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            var nsMsgIds = MessageExpHelper.BuildNamespacedMessageIds(storyInst, m_MsgIds);
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                int ct = 0;
                foreach (var msgId in nsMsgIds) {
                    ct += storyInst.CountMessage(msgId);
                }

                if (ct <= 0) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (timeout <= 0 || curTime <= timeout) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// storywaitlocalnamespacedmessage(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for namespaced message, skipped if story is skipped
    /// </summary>
    internal sealed class StoryWaitLocalNamespacedMessageExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            var nsMsgIds = MessageExpHelper.BuildNamespacedMessageIds(storyInst, m_MsgIds);
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                bool triggered = false;
                foreach (var msgId in nsMsgIds) {
                    long time = storyInst.GetMessageTriggerTime(msgId);
                    if (time > startTime) {
                        triggered = true;
                        break;
                    }
                }

                if (triggered) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// storywaitlocalnamespacedmessagehandler(msgId1, msgId2, ...)[set(var, val) timeoutset(timeout, var, val)] - wait for namespaced handler, skipped if story is skipped
    /// </summary>
    internal sealed class StoryWaitLocalNamespacedMessageHandlerExp : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            long startTime = TimeUtility.GetLocalMilliseconds();
            var nsMsgIds = MessageExpHelper.BuildNamespacedMessageIds(storyInst, m_MsgIds);
            int timeout = m_Timeout;
            string setVar = m_SetVar, setValExp = m_SetValExp, timeoutSetVar = m_TimeoutSetVar, timeoutSetValExp = m_TimeoutSetValExp;
            bool hasSet = m_HasSet;

            while (true) {
                int ct = 0;
                foreach (var msgId in nsMsgIds) {
                    ct += storyInst.CountMessage(msgId);
                }

                if (ct <= 0) {
                    if (hasSet && !string.IsNullOrEmpty(setVar)) {
                        storyInst.SetVariable(setVar, BoxedValue.FromObject(setValExp));
                    }
                    break;
                }

                int curTime = (int)(TimeUtility.GetLocalMilliseconds() - startTime);
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                }
                else {
                    if (hasSet && !string.IsNullOrEmpty(timeoutSetVar)) {
                        storyInst.SetVariable(timeoutSetVar, BoxedValue.FromObject(timeoutSetValExp));
                    }
                    break;
                }
            }

            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            var third = statementData.Third.AsFunction;
            if (first != null && second != null && third != null) {
                m_HasSet = true;
                LoadMsgIds(first);
                LoadSet(second);
                LoadTimeoutSet(third);
                return true;
            }
            return false;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            m_MsgIds = new List<string>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                var param = callData.GetParam(i);
                if (param != null) m_MsgIds.Add(param.GetId());
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar = callData.GetParamId(0);
                m_SetValExp = callData.GetParamId(1);
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_Timeout = int.Parse(callData.GetParamId(0));
                m_TimeoutSetVar = callData.GetParamId(1);
                m_TimeoutSetValExp = callData.GetParamId(2);
            }
        }

        private List<string> m_MsgIds = new List<string>();
        private int m_Timeout = 0;
        private string m_SetVar = null;
        private string m_SetValExp = null;
        private string m_TimeoutSetVar = null;
        private string m_TimeoutSetValExp = null;
        private bool m_HasSet = false;
    }

    /// <summary>
    /// suspendlocalnamespacedmessagehandler(msgId1, msgId2, ...) - suspend namespaced message handlers
    /// </summary>
    internal sealed class SuspendLocalNamespacedMessageHandlerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string _namespace = storyInst.Namespace;
            for (int i = 0; i < operands.Count; i++) {
                if (string.IsNullOrEmpty(_namespace)) {
                    storyInst.SuspendMessageHandler(operands[i].ToString(), true);
                }
                else {
                    storyInst.SuspendMessageHandler(string.Format("{0}:{1}", _namespace, operands[i].ToString()), true);
                }
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// resumelocalnamespacedmessagehandler(msgId1, msgId2, ...) - resume namespaced message handlers
    /// </summary>
    internal sealed class ResumeLocalNamespacedMessageHandlerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst == null)
                return BoxedValue.NullObject;
            string _namespace = storyInst.Namespace;
            for (int i = 0; i < operands.Count; i++) {
                if (string.IsNullOrEmpty(_namespace)) {
                    storyInst.SuspendMessageHandler(operands[i].ToString(), false);
                }
                else {
                    storyInst.SuspendMessageHandler(string.Format("{0}:{1}", _namespace, operands[i].ToString()), false);
                }
            }
            return BoxedValue.NullObject;
        }
    }

    // ========== Helper Methods ==========

    internal static class MessageExpHelper
    {
        public static string BuildNamespacedMessageId(StoryInstance storyInst, string msgId)
        {
            string _namespace = storyInst.Namespace;
            if (!string.IsNullOrEmpty(_namespace)) {
                return string.Format("{0}:{1}", _namespace, msgId);
            }
            return msgId;
        }

        public static List<string> BuildNamespacedMessageIds(StoryInstance storyInst, List<string> msgIds)
        {
            string _namespace = storyInst.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                return msgIds;
            }
            var result = new List<string>(msgIds.Count);
            foreach (var msgId in msgIds) {
                result.Add(string.Format("{0}:{1}", _namespace, msgId));
            }
            return result;
        }
    }
}
