using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// terminate();
  /// </summary>
  internal class TerminateCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      TerminateCommand cmd = new TerminateCommand();
      return cmd;
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      instance.IsTerminated = true;
      return false;
    }
    protected override void Load(Dsl.CallData callData)
    {}
  }
}
