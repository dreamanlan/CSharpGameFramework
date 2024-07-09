using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
    public delegate BoxedValueList SceneLogicNewBoxedValueListDelegation(SceneLogicInfo info);
    public delegate void SceneLogicSendStoryMessageDelegation(SceneLogicInfo info, string msgId, BoxedValueList args);
    public interface ISceneLogic
    {
        void Execute(SceneLogicInfo info, long deltaTime);
    }
    public abstract class AbstractSceneLogic : ISceneLogic
    {
        public static SceneLogicNewBoxedValueListDelegation OnSceneLogicNewBoxedValueList;
        public static SceneLogicSendStoryMessageDelegation OnSceneLogicSendStoryMessage;
        public abstract void Execute(SceneLogicInfo info, long deltaTime);

        protected BoxedValueList SceneLogicNewBoxedValueList(SceneLogicInfo info)
        {
            if (null != OnSceneLogicNewBoxedValueList) {
                return OnSceneLogicNewBoxedValueList(info);
            }
            return null;
        }
        protected void SceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValue arg1)
        {
            if (null != OnSceneLogicSendStoryMessage) {
                var args = SceneLogicNewBoxedValueList(info);
                args.Add(arg1);
                OnSceneLogicSendStoryMessage(info, msgId, args);
            }
        }
        protected void SceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            if (null != OnSceneLogicSendStoryMessage) {
                var args = SceneLogicNewBoxedValueList(info);
                args.Add(arg1);
                args.Add(arg2);
                OnSceneLogicSendStoryMessage(info, msgId, args);
            }
        }
        protected void SceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            if (null != OnSceneLogicSendStoryMessage) {
                var args = SceneLogicNewBoxedValueList(info);
                args.Add(arg1);
                args.Add(arg2);
                args.Add(arg3);
                OnSceneLogicSendStoryMessage(info, msgId, args);
            }
        }
        protected void SceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, BoxedValueList args)
        {
            if (null != OnSceneLogicSendStoryMessage) {
                OnSceneLogicSendStoryMessage(info, msgId, args);
            }
        }
    }
}
