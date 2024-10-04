﻿using System;
using System.Collections.Generic;
using DotnetStoryScript;
using GameFrameworkMessage;

namespace ScriptableFramework.GmCommands
{
    internal class EnableCalculatorLogCommand : SimpleStoryCommandBase<EnableCalculatorLogCommand, StoryFunctionParam<int, int, int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int, int, int> _params, long delta)
        {
            int val1 = _params.Param1Value;
            int val2 = _params.Param2Value;
            int val3 = _params.Param3Value;
            GlobalVariables.s_EnableCalculatorLog = val1 != 0;
            GlobalVariables.s_EnableCalculatorDetailLog = val2 != 0;
            GlobalVariables.s_EnableCalculatorOperatorLog = val3 != 0;
            return false;
        }
    }
    //---------------------------------------------------------------------------------------------------------------
    internal class DoResetDslCommand : SimpleStoryCommandBase<DoResetDslCommand, StoryFunctionParam<string>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<string> _params, long delta)
        {
            string val = _params.Param1Value;
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 0;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return false;
        }
    }
    internal class DoScpCommand : SimpleStoryCommandBase<DoScpCommand, StoryFunctionParam<string>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<string> _params, long delta)
        {
            string val = _params.Param1Value;
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 1;
                cmdMsg.content = val;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return false;
        }
    }
    internal class DoGmCommand : SimpleStoryCommandBase<DoGmCommand, StoryFunctionParam<string>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<string> _params, long delta)
        {
            string val = _params.Param1Value;
            //ChatMessageSender.SendGmCommand(val);
            if (Network.NetworkSystem.Instance.CanSendMessage) {
                Msg_CR_GmCommand cmdMsg = new Msg_CR_GmCommand();
                cmdMsg.type = 2;
                cmdMsg.content = val;
                Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_GmCommand, cmdMsg);
            }
            return false;
        }
    }
    internal class SetDebugCommand : SimpleStoryCommandBase<SetDebugCommand, StoryFunctionParam<int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
        {
            int val = _params.Param1Value;
            GlobalVariables.Instance.IsDebug = val != 0;
            Msg_CR_SwitchDebug msg = new Msg_CR_SwitchDebug();
            msg.is_debug = val != 0;
            Network.NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_SwitchDebug, msg);
            return false;
        }
    }
  internal class AllocMemoryCommand : SimpleStoryCommandBase<AllocMemoryCommand, StoryFunctionParam<string,int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<string,int> _params, long delta)
    {
      string key = _params.Param1Value;
      int size = _params.Param2Value;
      byte[] m = new byte[size];
      if (instance.GlobalVariables.ContainsKey(key)) {
        instance.GlobalVariables[key] = m;
      } else {
        instance.GlobalVariables.Add(key, m);
      }
      return false;
    }
  }
  internal class FreeMemoryCommand : SimpleStoryCommandBase<FreeMemoryCommand, StoryFunctionParam<string>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<string> _params, long delta)
    {
      string key = _params.Param1Value;
      if (instance.GlobalVariables.ContainsKey(key)) {
        instance.GlobalVariables.Remove(key);
        GC.Collect();
      } else {
        GC.Collect();
      }
      return false;
    }
  }
  internal class ConsumeCpuCommand : SimpleStoryCommandBase<ConsumeCpuCommand, StoryFunctionParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
    {
      int time = _params.Param1Value;
      long startTime = TimeUtility.GetElapsedTimeUs();
      while (startTime + time > TimeUtility.GetElapsedTimeUs()) {
      }
      return false;
    }
  }
  //---------------------------------------------------------------------------------------------------------------------------------
}
