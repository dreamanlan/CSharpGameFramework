using System;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class GetUserInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetUserInfoFunction val = new GetUserInfoFunction();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    m_Value = BoxedValue.FromObject(UserServer.Instance.UserProcessScheduler.GetUserInfo(userGuid));
                }
            }
        }
        private IStoryFunction<ulong> m_UserGuid = new StoryFunction<ulong>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
