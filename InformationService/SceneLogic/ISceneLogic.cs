using System;
using System.Collections.Generic;

namespace GameFramework
{
  public delegate void SceneLogicSendStoryMessageDelegation(SceneLogicInfo info, string msgId, object[] args);
  public interface ISceneLogic
  {
    void Execute(SceneLogicInfo info, long deltaTime);
  }
  public abstract class AbstractSceneLogic : ISceneLogic
  {
    public static SceneLogicSendStoryMessageDelegation OnSceneLogicSendStoryMessage;
    public abstract void Execute(SceneLogicInfo info, long deltaTime);

    protected void SceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, params object[] args)
    {
      if (null != OnSceneLogicSendStoryMessage) {
        OnSceneLogicSendStoryMessage(info, msgId, args);
      }
    }
  }
}
