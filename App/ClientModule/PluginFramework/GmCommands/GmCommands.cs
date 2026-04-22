using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFrameworkMessage;

namespace ScriptableFramework.GmCommands
{
    internal sealed class EnableCalculatorLogCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: enablecalculatorlog(val1, val2, val3)");

            int val1 = operands[0].GetInt();
            int val2 = operands[1].GetInt();
            int val3 = operands[2].GetInt();
            GlobalVariables.s_EnableCalculatorLog = val1 != 0;
            GlobalVariables.s_EnableCalculatorDetailLog = val2 != 0;
            GlobalVariables.s_EnableCalculatorOperatorLog = val3 != 0;
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------
    internal sealed class DoResetDslCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: resetdsl(val)");

            string val = operands[0].ToString();
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 0;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DoScpCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: scp(val)");

            string val = operands[0].ToString();
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 1;
                cmdMsg.content = val;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DoGmCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: gm(val)");

            string val = operands[0].ToString();
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 2;
                cmdMsg.content = val;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetDebugCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: setdebug(flag)");

            int val = operands[0].GetInt();
            GlobalVariables.Instance.IsDebug = val != 0;
            Msg_CR_SwitchDebug msg = new Msg_CR_SwitchDebug();
            msg.is_debug = val != 0;
            Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_SwitchDebug, msg);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class AllocMemoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: allocmemory(key, size)");

            string key = operands[0].ToString();
            int size = operands[1].GetInt();

            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null) {
                BoxedValue m = BoxedValue.FromObject(new byte[size]);
                var globals = storyInst.ContextVariables;
                if (globals.ContainsKey(key)) {
                    globals[key] = m;
                } else {
                    globals.Add(key, m);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FreeMemoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: freememory(key)");

            string key = operands[0].ToString();

            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null) {
                var globals = storyInst.ContextVariables;
                if (globals.ContainsKey(key)) {
                    globals.Remove(key);
                    GC.Collect();
                } else {
                    GC.Collect();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ConsumeCpuCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: consumecpu(time_us)");

            int time = operands[0].GetInt();
            long startTime = TimeUtility.GetElapsedTimeUs();
            while (startTime + (long)time > TimeUtility.GetElapsedTimeUs()) {
            }
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------
}
