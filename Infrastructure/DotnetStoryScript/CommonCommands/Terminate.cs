﻿using System;
using System.Collections.Generic;
namespace DotnetStoryScript.CommonCommands
{
    /// <summary>
    /// break();
    /// or
    /// break;
    /// </summary>
    public sealed class BreakCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            BreakCommand cmd = new BreakCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            handler.PeekRuntime().IsBreak = true;
            return false;
        }
    }
    /// <summary>
    /// continue();
    /// or
    /// continue;
    /// </summary>
    public sealed class ContinueCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ContinueCommand cmd = new ContinueCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            handler.PeekRuntime().IsContinue = true;
            return false;
        }
    }
    /// <summary>
    /// return();
    /// or
    /// return;
    /// </summary>
    public sealed class ReturnCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ReturnCommand cmd = new ReturnCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            handler.PeekRuntime().IsReturn = true;
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    /// <summary>
    /// suspend();
    /// </summary>
    public sealed class SuspendCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SuspendCommand cmd = new SuspendCommand();
            return cmd;
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            handler.IsSuspended = true;
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    /// <summary>
    /// terminate();
    /// </summary>
    public sealed class TerminateCommand : AbstractStoryCommand
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
    /// <summary>
    /// pause();
    /// </summary>
    public sealed class PauseCommand : AbstractStoryCommand
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
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
    }
}
