using System;
using System.Collections.Generic;
using GameFramework.Story;

namespace GameFramework
{
    internal class SceneLogicViewModel_General
    {
        internal SceneLogicViewModel_General()
        {
            AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
        }

        internal void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, object[] args)
        {
            GfxStorySystem.Instance.SendMessage(msgId, args);
        }
    }
}
