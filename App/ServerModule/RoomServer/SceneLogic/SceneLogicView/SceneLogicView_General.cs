using GameFrameworkMessage;
using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class SceneLogicView_General
    {
        public SceneLogicView_General()
        {
            AbstractSceneLogic.OnSceneLogicNewBoxedValueList += this.OnSceneLogicNewBoxedValueList;
            AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
        }

        public BoxedValueList OnSceneLogicNewBoxedValueList(SceneLogicInfo info)
        {
            Scene scene = info.SceneContext.CustomData as Scene;
            if (null != scene) {
                return scene.StorySystem.NewBoxedValueList();
            }
            return null;
        }
        public void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValueList args)
        {
            Scene scene = info.SceneContext.CustomData as Scene;
            if (null != scene) {
                scene.StorySystem.SendMessage(msgId, args);
            }
        }
    }
}