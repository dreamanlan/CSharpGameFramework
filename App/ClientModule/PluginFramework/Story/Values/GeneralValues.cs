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
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_AttrName.InitFromDsl(callData.GetParam(0));
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(1));
                }
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
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_AttrName.Evaluate(instance, iterator, args);
            }
            if (m_ParamNum > 1) {
                m_DefaultValue.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_AttrName.HaveValue) {
                string name = m_AttrName.Value;
                m_HaveValue = true;
                if (!GfxStorySystem.Instance.GfxDatas.TryGetValue(name, out m_Value)) {
                    if (m_ParamNum > 1) {
                        m_Value = m_DefaultValue.Value;
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GfxTimeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gfxtime") {
            }
        }
        public IStoryValue<object> Clone()
        {
            GfxTimeValue val = new GfxTimeValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
        
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = UnityEngine.Time.time;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GfxTimeScaleValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gfxtimescale") {
            }
        }
        public IStoryValue<object> Clone()
        {
            GfxTimeScaleValue val = new GfxTimeScaleValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
        
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = UnityEngine.Time.timeScale;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsActiveValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isactive") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            IsActiveValue val = new IsActiveValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                object o = m_ObjPath.Value;
                string objPath = o as string;
                if (null != objPath) {
                    UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = obj.activeSelf ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    try {
                        int objId = (int)o;
                        UnityEngine.GameObject obj = EntityController.Instance.GetGameObject(objId);
                        if (null != obj) {
                            m_Value = obj.activeSelf ? 1 : 0;
                        } else {
                            m_Value = 0;
                        }
                    } catch {
                        m_Value = null;
                    }
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsReallyActiveValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isreallyactive") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            IsReallyActiveValue val = new IsReallyActiveValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                object o = m_ObjPath.Value;
                string objPath = o as string;
                if (null != objPath) {
                    UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = obj.activeInHierarchy ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    try {
                        int objId = (int)o;
                        UnityEngine.GameObject obj = EntityController.Instance.GetGameObject(objId);
                        if (null != obj) {
                            m_Value = obj.activeInHierarchy ? 1 : 0;
                        } else {
                            m_Value = 0;
                        }
                    } catch {
                        m_Value = null;
                    }
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetComponentValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getcomponent") {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetComponentValue val = new GetComponentValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, iterator, args);
            m_ComponentType.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                object objPath = m_ObjPath.Value;
                object componentType = m_ComponentType.Value;
                UnityEngine.GameObject obj = objPath as UnityEngine.GameObject;
                if (null == obj) {
                    string path = objPath as string;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                }
                if (null != obj) {
                    Type t = componentType as Type;
                    if (null != t) {
                        UnityEngine.Component component = obj.GetComponent(t);
                        m_Value = component;
                    } else {
                        string name = componentType as string;
                        if (null != name) {
                            UnityEngine.Component component = obj.GetComponent(name);
                            m_Value = component;
                        }
                    }
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue<object>();
        private IStoryValue<object> m_ComponentType = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetGameObjectValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getgameobject") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetGameObjectValue val = new GetGameObjectValue();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                object o = m_ObjPath.Value;
                string objPath = o as string;
                if (null != objPath) {
                    UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = obj;
                    } else {
                        m_Value = null;
                    }
                } else {
                    try {
                        int objId = (int)o;
                        m_Value = EntityController.Instance.GetGameObject(objId);
                    } catch {
                        m_Value = null;
                    }
                }
            }
        }

        private IStoryValue<object> m_ObjPath = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetUnityTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getunitytype") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUnityTypeValue val = new GetUnityTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                if (null != typeName) {
                    if (!typeName.StartsWith("UnityEngine.")) {
                        typeName = string.Format("UnityEngine.{0},UnityEngine", typeName);
                    }
                    Type t = Type.GetType(typeName);
                    if (null != t) {
                        m_Value = t;
                    } else {
                        m_Value = null;
                    }
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetUnityUiTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getunityuitype") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUnityUiTypeValue val = new GetUnityUiTypeValue();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                if (null != typeName) {
                    if (!typeName.StartsWith("UnityEngine.UI.")) {
                        typeName = string.Format("UnityEngine.UI.{0},UnityEngine", typeName);
                    }
                    Type t = Type.GetType(typeName);
                    if (null != t) {
                        m_Value = t;
                    } else {
                        m_Value = null;
                    }
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<string> m_TypeName = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetEntityInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getentityinfo" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetEntityInfoValue val = new GetEntityInfoValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjId.HaveValue) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                m_Value = PluginFramework.Instance.GetEntityById(objId);
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetEntityViewValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getentityview" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetEntityViewValue val = new GetEntityViewValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjId.HaveValue) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                m_Value = EntityController.Instance.GetEntityViewById(objId);
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
