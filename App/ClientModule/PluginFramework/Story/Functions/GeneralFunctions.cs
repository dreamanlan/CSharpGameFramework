using System;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class GetTimeFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            GetTimeFunction val = new GetTimeFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
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
        private BoxedValue m_Value;
    }
    internal sealed class GetTimeScaleFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            GetTimeScaleFunction val = new GetTimeScaleFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
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
        private BoxedValue m_Value;
    }
    internal sealed class IsActiveFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            IsActiveFunction val = new IsActiveFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != objPath) {
                    UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = obj.activeSelf ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else if (null != uobj) {
                    m_Value = uobj.activeSelf ? 1 : 0;
                } else {
                    try {
                        int objId = o.GetInt();
                        UnityEngine.GameObject obj = PluginFramework.Instance.GetGameObject(objId);
                        if (null != obj) {
                            m_Value = obj.activeSelf ? 1 : 0;
                        } else {
                            m_Value = 0;
                        }
                    } catch {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class IsReallyActiveFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            IsReallyActiveFunction val = new IsReallyActiveFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != objPath) {
                    UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = obj.activeInHierarchy ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else if (null != uobj) {
                    m_Value = uobj.activeInHierarchy ? 1 : 0;
                } else {
                    try {
                        int objId = o.GetInt();
                        UnityEngine.GameObject obj = PluginFramework.Instance.GetGameObject(objId);
                        if (null != obj) {
                            m_Value = obj.activeInHierarchy ? 1 : 0;
                        } else {
                            m_Value = 0;
                        }
                    } catch {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class IsVisibleFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            IsVisibleFunction val = new IsVisibleFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != uobj) {
                    if (null != objPath) {
                        uobj = UnityEngine.GameObject.Find(objPath);
                    } else {
                        try {
                            int objId = o.GetInt();
                            uobj = PluginFramework.Instance.GetGameObject(objId);
                        } catch {
                            uobj = null;
                        }
                    }
                }
                if (null != uobj) {
                    var renderer = uobj.GetComponentInChildren<UnityEngine.Renderer>();
                    if (null != renderer) {
                        m_Value = renderer.isVisible ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    m_Value = 0;
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class IsNavmeshAgentEnabledFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            IsNavmeshAgentEnabledFunction val = new IsNavmeshAgentEnabledFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                var uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != uobj) {
                    if (null != objPath) {
                        uobj = UnityEngine.GameObject.Find(objPath);
                    } else {
                        try {
                            int objId = (int)o;
                            uobj = PluginFramework.Instance.GetGameObject(objId);
                        } catch {
                            uobj = null;
                        }
                    }
                }
                if (null != uobj) {
                    var agent = uobj.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (null != agent) {
                        m_Value = agent.enabled ? 1 : 0;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    m_Value = 0;
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentFunction val = new GetComponentFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    } else {
                        try {
                            int objId = objPath.GetInt();
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        } catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        UnityEngine.Component component = obj.GetComponent(t);
                        m_Value = BoxedValue.FromObject(component);
                    } else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            UnityEngine.Component component = obj.GetComponent(name);
                            m_Value = BoxedValue.FromObject(component);
                        }
                    }
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentInParentFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_IncludeInactive.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentInParentFunction val = new GetComponentInParentFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_IncludeInactive = m_IncludeInactive.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2) {
                m_IncludeInactive.Evaluate(instance, handler, iterator, args);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                int includeInactive = 1;
                if (m_ParamNum > 2) {
                    includeInactive = m_IncludeInactive.Value;
                }
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                    else {
                        try {
                            int objId = objPath.GetInt();
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        }
                        catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        UnityEngine.Component component = obj.GetComponentInParent(t, includeInactive != 0);
                        m_Value = BoxedValue.FromObject(component);
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = Utility.GetType(name);
                            if (null != t) {
                                UnityEngine.Component component = obj.GetComponentInParent(t, includeInactive != 0);
                                m_Value = BoxedValue.FromObject(component);
                            }
                            else {
                                m_Value = BoxedValue.NullObject;
                            }
                        }
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private IStoryFunction<int> m_IncludeInactive = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentInChildrenFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_IncludeInactive.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentInChildrenFunction val = new GetComponentInChildrenFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_IncludeInactive = m_IncludeInactive.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2) {
                m_IncludeInactive.Evaluate(instance, handler, iterator, args);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                int includeInactive = 1;
                if (m_ParamNum > 2) {
                    includeInactive = m_IncludeInactive.Value;
                }
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                    else {
                        try {
                            int objId = objPath.GetInt();
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        }
                        catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        UnityEngine.Component component = obj.GetComponentInChildren(t, includeInactive != 0);
                        m_Value = BoxedValue.FromObject(component);
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = Utility.GetType(name);
                            if (null != t) {
                                UnityEngine.Component component = obj.GetComponentInChildren(t, includeInactive != 0);
                                m_Value = BoxedValue.FromObject(component);
                            } else {
                                m_Value = BoxedValue.NullObject;
                            }
                        }
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private IStoryFunction<int> m_IncludeInactive = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentsFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentsFunction val = new GetComponentsFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                    else {
                        try {
                            int objId = objPath.GetInt();
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        }
                        catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        var comps = obj.GetComponents(t);
                        if (null != comps)
                            m_Value = comps;
                        else
                            m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = Utility.GetType(name);
                            if (null != t) {
                                var comps = obj.GetComponents(t);
                                if (null != comps)
                                    m_Value = comps;
                                else
                                    m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            } else {
                                m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            }
                        }
                    }
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentsInParentFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_IncludeInactive.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentsInParentFunction val = new GetComponentsInParentFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_IncludeInactive = m_IncludeInactive.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2) {
                m_IncludeInactive.Evaluate(instance, handler, iterator, args);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                int includeInactive = 1;
                if (m_ParamNum > 2) {
                    includeInactive = m_IncludeInactive.Value;
                }
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                    else {
                        try {
                            int objId = (int)objPath;
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        }
                        catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        var comps = obj.GetComponentsInParent(t, includeInactive != 0);
                        if (null != comps)
                            m_Value = comps;
                        else
                            m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = Utility.GetType(name);
                            if (null != t) {
                                var comps = obj.GetComponentsInParent(t, includeInactive != 0);
                                if (null != comps)
                                    m_Value = comps;
                                else
                                    m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            }
                            else {
                                m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            }
                        }
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private IStoryFunction<int> m_IncludeInactive = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetComponentsInChildrenFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ComponentType.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_IncludeInactive.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetComponentsInChildrenFunction val = new GetComponentsInChildrenFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ComponentType = m_ComponentType.Clone();
            val.m_IncludeInactive = m_IncludeInactive.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ComponentType.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2) {
                m_IncludeInactive.Evaluate(instance, handler, iterator, args);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjPath.HaveValue && m_ComponentType.HaveValue) {
                m_HaveValue = true;
                var objPath = m_ObjPath.Value;
                var componentType = m_ComponentType.Value;
                int includeInactive = 1;
                if (m_ParamNum > 2) {
                    includeInactive = m_IncludeInactive.Value;
                }
                UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
                if (null == obj) {
                    string path = objPath.IsString ? objPath.StringVal : null;
                    if (null != path) {
                        obj = UnityEngine.GameObject.Find(path);
                    }
                    else {
                        try {
                            int objId = objPath.GetInt();
                            obj = PluginFramework.Instance.GetGameObject(objId);
                        }
                        catch {
                            obj = null;
                        }
                    }
                }
                if (null != obj) {
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        var comps = obj.GetComponentsInChildren(t, includeInactive != 0);
                        if (null != comps)
                            m_Value = comps;
                        else
                            m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = Utility.GetType(name);
                            if (null != t) {
                                var comps = obj.GetComponentsInChildren(t, includeInactive != 0);
                                if (null != comps)
                                    m_Value = comps;
                                else
                                    m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            }
                            else {
                                m_Value = BoxedValue.FromObject(new List<UnityEngine.Component>());
                            }
                        }
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction m_ComponentType = new StoryFunction();
        private IStoryFunction<int> m_IncludeInactive = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetGameObjectFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData funcData = param as Dsl.FunctionData;
            if (null != funcData) {
                Load(funcData);
            }
        }
        public IStoryFunction Clone()
        {
            GetGameObjectFunction val = new GetGameObjectFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            for (int i = 0; i < m_DisableComponents.Count; ++i) {
                val.m_DisableComponents.Add(m_DisableComponents[i].Clone());
            }
            for (int i = 0; i < m_RemoveComponents.Count; ++i) {
                val.m_RemoveComponents.Add(m_RemoveComponents[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_DisableComponents.Count; ++i) {
                m_DisableComponents[i].Evaluate(instance, handler, iterator, args);
            }
            for (int i = 0; i < m_RemoveComponents.Count; ++i) {
                m_RemoveComponents[i].Evaluate(instance, handler, iterator, args);
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
        public BoxedValue Value
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
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;

                StrList disables = new StrList();
                for (int i = 0; i < m_DisableComponents.Count; ++i) {
                    disables.Add(m_DisableComponents[i].Value);
                }
                StrList removes = new StrList();
                for (int i = 0; i < m_RemoveComponents.Count; ++i) {
                    removes.Add(m_RemoveComponents[i].Value);
                }
                UnityEngine.GameObject obj = null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        m_Value = BoxedValue.FromObject(obj);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    try {
                        int objId = o.GetInt();
                        obj = PluginFramework.Instance.GetGameObject(objId);
                        m_Value = BoxedValue.FromObject(obj);
                    } catch {
                        m_Value = BoxedValue.NullObject;
                    }
                }
                if (null != obj) {
                    foreach (string disable in disables) {
                        var type = Utility.GetType(disable);
                        if (null != type) {
                            var comps = obj.GetComponentsInChildren(type);
                            for (int i = 0; i < comps.Length; ++i) {
                                var t = comps[i].GetType();
                                t.InvokeMember("enabled", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, null, comps[i], new object[] { false });
                            }
                        }
                    }
                    foreach (string remove in removes) {
                        var type = Utility.GetType(remove);
                        if (null != type) {
                            var comps = obj.GetComponentsInChildren(type);
                            for (int i = 0; i < comps.Length; ++i) {
                                Utility.DestroyObject(comps[i]);
                            }
                        }
                    }
                }
            }
        }
        private void Load(Dsl.FunctionData funcData)
        {
            var callData = funcData.ThisOrLowerOrderCall;
            if (callData.IsValid()) {
                LoadCall(callData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "getgameobject") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            if (id == "disable") {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    var p = new StoryFunction<string>();
                    p.InitFromDsl(callData.GetParam(i));
                    m_DisableComponents.Add(p);
                }
            } else if (id == "remove") {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    var p = new StoryFunction<string>();
                    p.InitFromDsl(callData.GetParam(i));
                    m_RemoveComponents.Add(p);
                }
            }
        }

        private IStoryFunction m_ObjPath = new StoryFunction();
        private List<IStoryFunction<string>> m_DisableComponents = new List<IStoryFunction<string>>();
        private List<IStoryFunction<string>> m_RemoveComponents = new List<IStoryFunction<string>>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetParentFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetParentFunction val = new GetParentFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != objPath) {
                    var obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj && null != obj.transform.parent) {
                        m_Value = BoxedValue.FromObject(obj.transform.parent.gameObject);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else if (null != uobj) {
                    if (null != uobj.transform.parent) {
                        m_Value = BoxedValue.FromObject(uobj.transform.parent.gameObject);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    try {
                        int objId = o.GetInt();
                        var obj = PluginFramework.Instance.GetGameObject(objId);
                        if (null != obj && null != obj.transform.parent) {
                            m_Value = BoxedValue.FromObject(obj.transform.parent.gameObject);
                        } else {
                            m_Value = BoxedValue.NullObject;
                        }
                    } catch {
                        m_Value = BoxedValue.NullObject;
                    }
                }
            }
        }
        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class FindChildFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ChildPath.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryFunction Clone()
        {
            FindChildFunction val = new FindChildFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ChildPath = m_ChildPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ChildPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue && m_ChildPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string childPath = m_ChildPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null != objPath) {
                    var obj = UnityEngine.GameObject.Find(objPath);
                    if (null != obj) {
                        var t = Utility.FindChildRecursive(obj.transform, childPath);
                        if (null != t) {
                            m_Value = BoxedValue.FromObject(t.gameObject);
                        } else {
                            m_Value = BoxedValue.NullObject;
                        }
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else if (null != uobj) {
                    var t = Utility.FindChildRecursive(uobj.transform, childPath);
                    if (null != t) {
                        m_Value = BoxedValue.FromObject(t.gameObject);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    try {
                        int objId = o.GetInt();
                        var obj = PluginFramework.Instance.GetGameObject(objId);
                        if (null != obj) {
                            var t = Utility.FindChildRecursive(obj.transform, childPath);
                            if (null != t) {
                                m_Value = BoxedValue.FromObject(t.gameObject);
                            } else {
                                m_Value = BoxedValue.NullObject;
                            }
                        } else {
                            m_Value = BoxedValue.NullObject;
                        }
                    } catch {
                        m_Value = BoxedValue.NullObject;
                    }
                }
            }
        }

        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction<string> m_ChildPath = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetChildCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetChildCountFunction val = new GetChildCountFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                string objPath = o.IsString ? o.StringVal : null;
                var uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null == uobj) {
                    if (null != objPath) {
                        uobj = UnityEngine.GameObject.Find(objPath);
                    } else {
                        try {
                            int objId = (int)o;
                            uobj = PluginFramework.Instance.GetGameObject(objId);
                        } catch {
                            uobj = null;
                        }
                    }
                }
                if (null != uobj) {
                    m_Value = uobj.transform.childCount;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryFunction m_ObjPath = new StoryFunction();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetChildFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjPath.InitFromDsl(callData.GetParam(0));
                    m_ChildIndex.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetChildFunction val = new GetChildFunction();
            val.m_ObjPath = m_ObjPath.Clone();
            val.m_ChildIndex = m_ChildIndex.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjPath.Evaluate(instance, handler, iterator, args);
            m_ChildIndex.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjPath.HaveValue && m_ChildIndex.HaveValue) {
                m_HaveValue = true;
                var o = m_ObjPath.Value;
                int index = m_ChildIndex.Value;
                string objPath = o.IsString ? o.StringVal : null;
                var uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null == uobj) {
                    if (null != objPath) {
                        uobj = UnityEngine.GameObject.Find(objPath);
                    } else {
                        try {
                            int objId = (int)o;
                            uobj = PluginFramework.Instance.GetGameObject(objId);
                        } catch {
                            uobj = null;
                        }
                    }
                }
                if (null != uobj) {
                    var t = uobj.transform.GetChild(index);
                    if (null != t) {
                        m_Value = BoxedValue.FromObject(t.gameObject);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }

        private IStoryFunction m_ObjPath = new StoryFunction();
        private IStoryFunction<int> m_ChildIndex = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetUnityTypeFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetUnityTypeFunction val = new GetUnityTypeFunction();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, handler, iterator, args);
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
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }

        private IStoryFunction<string> m_TypeName = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetUnityUiTypeFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetUnityUiTypeFunction val = new GetUnityUiTypeFunction();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, handler, iterator, args);
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
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                if (null != typeName) {
                    if (!typeName.StartsWith("UnityEngine.UI.")) {
                        typeName = string.Format("UnityEngine.UI.{0},UnityEngine.UI", typeName);
                    }
                    Type t = Type.GetType(typeName);
                    if (null != t) {
                        m_Value = t;
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction<string> m_TypeName = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetUserTypeFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_TypeName.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            GetUserTypeFunction val = new GetUserTypeFunction();
            val.m_TypeName = m_TypeName.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_TypeName.Evaluate(instance, handler, iterator, args);
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
            if (m_TypeName.HaveValue) {
                m_HaveValue = true;
                string typeName = m_TypeName.Value;
                if (null != typeName) {
                    typeName = string.Format("{0},Assembly-CSharp", typeName);
                    Type t = Type.GetType(typeName);
                    if (null != t) {
                        m_Value = t;
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                } else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }
        private IStoryFunction<string> m_TypeName = new StoryFunction<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetEntityInfoFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetEntityInfoFunction val = new GetEntityInfoFunction();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjId.HaveValue) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                m_Value = BoxedValue.FromObject(PluginFramework.Instance.GetEntityById(objId));
            }
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetEntityViewFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            GetEntityViewFunction val = new GetEntityViewFunction();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, handler, iterator, args);
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
            if (m_ObjId.HaveValue) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                m_Value = BoxedValue.FromObject(EntityController.Instance.GetEntityViewById(objId));
            }
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetGlobalFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            GetGlobalFunction val = new GetGlobalFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
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
            m_HaveValue = true;
            m_Value = BoxedValue.FromObject(GlobalVariables.Instance);
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class PluginFrameworkFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            PluginFrameworkFunction val = new PluginFrameworkFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
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
            m_HaveValue = true;
            m_Value = BoxedValue.FromObject(PluginFramework.Instance);
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
