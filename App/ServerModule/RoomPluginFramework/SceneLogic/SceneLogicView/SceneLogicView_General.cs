using GameFrameworkMessage;
using System;
using System.Collections.Generic;

namespace GameFramework
{
  public class SceneLogicView_General
  {
    public SceneLogicView_General() {
      AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
    }

    public void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, object[] args) {
      Scene scene = info.SceneContext.CustomData as Scene;
      if (null != scene) {
        scene.StorySystem.SendMessage(msgId, args);
      }
    }
  }
}