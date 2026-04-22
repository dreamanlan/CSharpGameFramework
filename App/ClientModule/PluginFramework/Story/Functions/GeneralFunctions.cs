using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class GetTimeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return UnityEngine.Time.time;
        }
    }
    internal sealed class GetTimeScaleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return UnityEngine.Time.timeScale;
        }
    }
    internal sealed class IsActiveFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    return obj.activeSelf ? 1 : 0;
                }
                return 0;
            } else if (null != uobj) {
                return uobj.activeSelf ? 1 : 0;
            } else {
                try {
                    int objId = o.GetInt();
                    UnityEngine.GameObject obj = PluginFramework.Instance.GetGameObject(objId);
                    if (null != obj) {
                        return obj.activeSelf ? 1 : 0;
                    }
                    return 0;
                } catch {
                    return 0;
                }
            }
        }
    }
    internal sealed class IsReallyActiveFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    return obj.activeInHierarchy ? 1 : 0;
                }
                return 0;
            } else if (null != uobj) {
                return uobj.activeInHierarchy ? 1 : 0;
            } else {
                try {
                    int objId = o.GetInt();
                    UnityEngine.GameObject obj = PluginFramework.Instance.GetGameObject(objId);
                    if (null != obj) {
                        return obj.activeInHierarchy ? 1 : 0;
                    }
                    return 0;
                } catch {
                    return 0;
                }
            }
        }
    }
    internal sealed class IsVisibleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null == uobj) {
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
                    return renderer.isVisible ? 1 : 0;
                }
                return 0;
            }
            return 0;
        }
    }
    internal sealed class IsNavmeshAgentEnabledFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var o = operands[0];
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
                var agent = uobj.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (null != agent) {
                    return agent.enabled ? 1 : 0;
                }
                return 0;
            }
            return 0;
        }
    }
    internal sealed class GetComponentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
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
                    return BoxedValue.FromObject(component);
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        UnityEngine.Component component = obj.GetComponent(name);
                        return BoxedValue.FromObject(component);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentInParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = 1;
            if (operands.Count > 2) {
                includeInactive = operands[2].GetInt();
            }
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
                    UnityEngine.Component component = obj.GetComponentInParent(t, includeInactive != 0);
                    return BoxedValue.FromObject(component);
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = Utility.GetType(name);
                        if (null != t) {
                            UnityEngine.Component component = obj.GetComponentInParent(t, includeInactive != 0);
                            return BoxedValue.FromObject(component);
                        }
                        return BoxedValue.NullObject;
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentInChildrenFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = 1;
            if (operands.Count > 2) {
                includeInactive = operands[2].GetInt();
            }
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
                    UnityEngine.Component component = obj.GetComponentInChildren(t, includeInactive != 0);
                    return BoxedValue.FromObject(component);
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = Utility.GetType(name);
                        if (null != t) {
                            UnityEngine.Component component = obj.GetComponentInChildren(t, includeInactive != 0);
                            return BoxedValue.FromObject(component);
                        }
                        return BoxedValue.NullObject;
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
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
                    var comps = obj.GetComponents(t);
                    if (null != comps)
                        return comps;
                    else
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = Utility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponents(t);
                            if (null != comps)
                                return comps;
                            else
                                return BoxedValue.FromObject(new List<UnityEngine.Component>());
                        }
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentsInParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = 1;
            if (operands.Count > 2) {
                includeInactive = operands[2].GetInt();
            }
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                } else {
                    try {
                        int objId = (int)objPath;
                        obj = PluginFramework.Instance.GetGameObject(objId);
                    } catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                if (null != t) {
                    var comps = obj.GetComponentsInParent(t, includeInactive != 0);
                    if (null != comps)
                        return comps;
                    else
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = Utility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponentsInParent(t, includeInactive != 0);
                            if (null != comps)
                                return comps;
                            else
                                return BoxedValue.FromObject(new List<UnityEngine.Component>());
                        }
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentsInChildrenFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = 1;
            if (operands.Count > 2) {
                includeInactive = operands[2].GetInt();
            }
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
                    var comps = obj.GetComponentsInChildren(t, includeInactive != 0);
                    if (null != comps)
                        return comps;
                    else
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                } else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = Utility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponentsInChildren(t, includeInactive != 0);
                            if (null != comps)
                                return comps;
                            else
                                return BoxedValue.FromObject(new List<UnityEngine.Component>());
                        }
                        return BoxedValue.FromObject(new List<UnityEngine.Component>());
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetGameObjectFunction : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var o = m_ObjPath.Calc();
            string objPath = o.IsString ? o.StringVal : null;
            StrList disables = new StrList();
            for (int i = 0; i < m_DisableComponents.Count; ++i) {
                disables.Add(m_DisableComponents[i].Calc());
            }
            StrList removes = new StrList();
            for (int i = 0; i < m_RemoveComponents.Count; ++i) {
                removes.Add(m_RemoveComponents[i].Calc());
            }
            UnityEngine.GameObject obj = null;
            if (null != objPath) {
                obj = UnityEngine.GameObject.Find(objPath);
                if (null == obj)
                    return BoxedValue.NullObject;
            } else {
                try {
                    int objId = o.GetInt();
                    obj = PluginFramework.Instance.GetGameObject(objId);
                } catch {
                    return BoxedValue.NullObject;
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
                return BoxedValue.FromObject(obj);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            } else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "getgameobject") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath = Calculator.Load(callData.GetParam(0));
                }
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            if (id == "disable") {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    IExpression p = Calculator.Load(callData.GetParam(i));
                    m_DisableComponents.Add(p);
                }
            } else if (id == "remove") {
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    IExpression p = Calculator.Load(callData.GetParam(i));
                    m_RemoveComponents.Add(p);
                }
            }
        }
        private IExpression m_ObjPath;
        private List<IExpression> m_DisableComponents = new List<IExpression>();
        private List<IExpression> m_RemoveComponents = new List<IExpression>();
    }
    internal sealed class GetParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                var obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj && null != obj.transform.parent) {
                    return BoxedValue.FromObject(obj.transform.parent.gameObject);
                }
                return BoxedValue.NullObject;
            } else if (null != uobj) {
                if (null != uobj.transform.parent) {
                    return BoxedValue.FromObject(uobj.transform.parent.gameObject);
                }
                return BoxedValue.NullObject;
            } else {
                try {
                    int objId = o.GetInt();
                    var obj = PluginFramework.Instance.GetGameObject(objId);
                    if (null != obj && null != obj.transform.parent) {
                        return BoxedValue.FromObject(obj.transform.parent.gameObject);
                    }
                    return BoxedValue.NullObject;
                } catch {
                    return BoxedValue.NullObject;
                }
            }
        }
    }
    internal sealed class FindChildFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var o = operands[0];
            string childPath = operands[1].ToString();
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                var obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    var t = Utility.FindChildRecursive(obj.transform, childPath);
                    if (null != t) {
                        return BoxedValue.FromObject(t.gameObject);
                    }
                }
                return BoxedValue.NullObject;
            } else if (null != uobj) {
                var t = Utility.FindChildRecursive(uobj.transform, childPath);
                if (null != t) {
                    return BoxedValue.FromObject(t.gameObject);
                }
                return BoxedValue.NullObject;
            } else {
                try {
                    int objId = o.GetInt();
                    var obj = PluginFramework.Instance.GetGameObject(objId);
                    if (null != obj) {
                        var t = Utility.FindChildRecursive(obj.transform, childPath);
                        if (null != t) {
                            return BoxedValue.FromObject(t.gameObject);
                        }
                    }
                    return BoxedValue.NullObject;
                } catch {
                    return BoxedValue.NullObject;
                }
            }
        }
    }
    internal sealed class GetChildCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var o = operands[0];
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
                return uobj.transform.childCount;
            }
            return 0;
        }
    }
    internal sealed class GetChildFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var o = operands[0];
            int index = operands[1].GetInt();
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
                    return BoxedValue.FromObject(t.gameObject);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetUnityTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string typeName = operands[0].ToString();
            if (null != typeName) {
                if (!typeName.StartsWith("UnityEngine.")) {
                    typeName = string.Format("UnityEngine.{0},UnityEngine", typeName);
                }
                Type t = Type.GetType(typeName);
                if (null != t) {
                    return t;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetUnityUiTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string typeName = operands[0].ToString();
            if (null != typeName) {
                if (!typeName.StartsWith("UnityEngine.UI.")) {
                    typeName = string.Format("UnityEngine.UI.{0},UnityEngine.UI", typeName);
                }
                Type t = Type.GetType(typeName);
                if (null != t) {
                    return t;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetUserTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string typeName = operands[0].ToString();
            if (null != typeName) {
                typeName = string.Format("{0},Assembly-CSharp", typeName);
                Type t = Type.GetType(typeName);
                if (null != t) {
                    return t;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetEntityInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            return BoxedValue.FromObject(PluginFramework.Instance.GetEntityById(objId));
        }
    }
    internal sealed class GetEntityViewFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            return BoxedValue.FromObject(EntityController.Instance.GetEntityViewById(objId));
        }
    }
    internal sealed class GetGlobalFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(GlobalVariables.Instance);
        }
    }
    internal sealed class PluginFrameworkFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(PluginFramework.Instance);
        }
    }
}
