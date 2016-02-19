using GameFrameworkMessage;
using System;
using System.Collections.Generic;

namespace GameFramework
{
  internal class SceneLogicView_General
  {
    internal SceneLogicView_General() {
      AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
    }

    internal void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, object[] args) {
      Scene scene = info.SceneContext.CustomData as Scene;
      if (null != scene) {
        scene.StorySystem.SendMessage(msgId, args);
      }
    }
  }
}