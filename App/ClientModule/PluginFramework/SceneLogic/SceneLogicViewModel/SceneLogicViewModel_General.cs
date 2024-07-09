using System;
using System.Collections.Generic;
using ScriptableFramework.Story;

namespace ScriptableFramework
{
    internal class SceneLogicViewModel_General
    {
        internal SceneLogicViewModel_General()
        {
            AbstractSceneLogic.OnSceneLogicNewBoxedValueList += this.OnSceneLogicNewBoxedValueList;
            AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
        }

        internal BoxedValueList OnSceneLogicNewBoxedValueList(SceneLogicInfo info)
        {
            return GfxStorySystem.Instance.NewBoxedValueList();
        }
        internal void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValueList args)
        {
            GfxStorySystem.Instance.SendMessage(msgId, args);
        }
    }
}
