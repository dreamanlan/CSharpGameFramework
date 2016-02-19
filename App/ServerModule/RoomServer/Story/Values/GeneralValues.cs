using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class GfxGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gfxget") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_AttrName.InitFromDsl(callData.GetParam(0));
                    flag |= m_AttrName.Flag;
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(1));
                    flag |= m_DefaultValue.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GfxGetValue val = new GfxGetValue();
            val.m_ParamNum = m_ParamNum;
            val.m_AttrName = m_AttrName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                if (m_ParamNum > 0) {
                    m_AttrName.Substitute(iterator, args);
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.Substitute(iterator, args);
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0) {
                m_AttrName.Evaluate(instance);
            }
            if (m_ParamNum > 1) {
                m_DefaultValue.Evaluate(instance);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_AttrName.HaveValue) {
                    string name = m_AttrName.Value;
                    m_HaveValue = true;
                    if (!scene.StorySystem.GfxDatas.TryGetValue(name, out m_Value)) {
                        if (m_ParamNum > 1) {
                            m_Value = m_DefaultValue.Value;
                        }
                    }
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private int m_ParamNum = 0;
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GfxTimeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gfxtime") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            GfxTimeValue val = new GfxTimeValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
        }
        public void Evaluate(StoryInstance instance)
        {
            TryUpdateValue(instance);
        }
        public void Analyze(StoryInstance instance)
        {
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                m_HaveValue = true;
                m_Value = 0;
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GfxTimeScaleValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gfxtimescale") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            GfxTimeScaleValue val = new GfxTimeScaleValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
        }
        public void Evaluate(StoryInstance instance)
        {
            TryUpdateValue(instance);
        }
        public void Analyze(StoryInstance instance)
        {
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                m_HaveValue = true;
                m_Value = 1;
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class IsActiveValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isactive") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    flag |= m_ObjPath.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            IsActiveValue val = new IsActiveValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjPath.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjPath.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjPath.HaveValue) {
                    m_HaveValue = true;
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class IsReallyActiveValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isreallyactive") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    flag |= m_ObjPath.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            IsReallyActiveValue val = new IsReallyActiveValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjPath.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjPath.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjPath.HaveValue) {
                    m_HaveValue = true;
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetComponentValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getcomponent") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                    flag |= m_ObjPath.Flag;
                    flag |= m_ComponentType.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetComponentValue val = new GetComponentValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjPath.Substitute(iterator, args);
                m_ComponentType.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjPath.Evaluate(instance);
            m_ComponentType.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                    m_HaveValue = true;
                    object objPath = m_ObjPath.Value;
                    object componentType = m_ComponentType.Value;
                    m_Value = null;
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private IStoryValue<object> m_ComponentType = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetGameObjectValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getgameobject") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    flag |= m_ObjPath.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetGameObjectValue val = new GetGameObjectValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjPath.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjPath.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjPath.HaveValue) {
                    m_HaveValue = true;
                    m_Value = null;
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetUnityTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getunitytype") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                    flag |= m_TypeName.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUnityTypeValue val = new GetUnityTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_TypeName.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_TypeName.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_TypeName.HaveValue) {
                    m_HaveValue = true;
                    string typeName = m_TypeName.Value;
                    m_Value = null;
                }
            }
        }

        private IStoryValue<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetUnityUiTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getunityuitype") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                    flag |= m_TypeName.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUnityUiTypeValue val = new GetUnityUiTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_TypeName.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_TypeName.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_TypeName.HaveValue) {
                    m_HaveValue = true;
                    string typeName = m_TypeName.Value;
                    m_Value = null;
                }
            }
        }

        private IStoryValue<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetEntityInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getentityinfo" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_ObjId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetEntityInfoValue val = new GetEntityInfoValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    m_Value = scene.SceneContext.GetEntityById(objId);
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetEntityViewValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getentityview" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_ObjId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetEntityViewValue val = new GetEntityViewValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    m_Value = null;
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
