using System;
using System.Collections.Generic;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// terminate();
    /// </summary>
    internal sealed class TerminateCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            TerminateCommand cmd = new TerminateCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            instance.IsTerminated = true;
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        { }
    }
    /// <summary>
    /// pause();
    /// </summary>
    internal sealed class PauseCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PauseCommand cmd = new PauseCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            instance.IsPaused = true;
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        { }
    }
}
