using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Dsl;
using StoryScript;
using StoryScript.DslExpression;

namespace GmCommands
{
    internal sealed class GetTimeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return UnityEngine.Time.time;
        }
    }
    internal sealed class GetTimeScaleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return UnityEngine.Time.timeScale;
        }
    }
    internal sealed class GetTimeSinceStartupValue : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return UnityEngine.Time.realtimeSinceStartup;
        }
    }
    internal sealed class IsActiveFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    return obj.activeSelf ? 1 : 0;
                }
            }
            else if (null != uobj) {
                return uobj.activeSelf ? 1 : 0;
            }
            else {
                try { int objId = o.GetInt(); } catch { }
            }
            return 0;
        }
    }
    internal sealed class IsReallyActiveFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                UnityEngine.GameObject obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    return obj.activeInHierarchy ? 1 : 0;
                }
            }
            else if (null != uobj) {
                return uobj.activeInHierarchy ? 1 : 0;
            }
            else {
                try { int objId = o.GetInt(); } catch { }
            }
            return 0;
        }
    }
    internal sealed class IsVisibleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null == uobj) {
                if (null != objPath) {
                    uobj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try { int objId = o.GetInt(); uobj = null; } catch { uobj = null; }
                }
            }
            if (null != uobj) {
                var renderer = uobj.GetComponentInChildren<UnityEngine.Renderer>();
                if (null != renderer) {
                    return renderer.isVisible ? 1 : 0;
                }
            }
            return 0;
        }
    }
    internal sealed class GetComponentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                if (null != t) {
                    return obj.GetComponent(t);
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        return obj.GetComponent(name);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentInParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = operands.Count > 2 ? operands[2].GetInt() : 1;
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                if (null != t) {
                    return obj.GetComponentInParent(t, includeInactive != 0);
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = StoryScriptUtility.GetType(name);
                        if (null != t) {
                            return obj.GetComponentInParent(t, includeInactive != 0);
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentInChildrenFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = operands.Count > 2 ? operands[2].GetInt() : 1;
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                if (null != t) {
                    return obj.GetComponentInChildren(t, includeInactive != 0);
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = StoryScriptUtility.GetType(name);
                        if (null != t) {
                            return obj.GetComponentInChildren(t, includeInactive != 0);
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetComponentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
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
                        return comps;
                    else
                        return new List<UnityEngine.Component>();
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = StoryScriptUtility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponents(t);
                            if (null != comps)
                                return comps;
                            else
                                return new List<UnityEngine.Component>();
                        }
                    }
                }
            }
            return new List<UnityEngine.Component>();
        }
    }
    internal sealed class GetComponentsInParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = operands.Count > 2 ? operands[2].GetInt() : 1;
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
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
                        return comps;
                    else
                        return new List<UnityEngine.Component>();
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = StoryScriptUtility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponentsInParent(t, includeInactive != 0);
                            if (null != comps)
                                return comps;
                            else
                                return new List<UnityEngine.Component>();
                        }
                    }
                }
            }
            return new List<UnityEngine.Component>();
        }
    }
    internal sealed class GetComponentsInChildrenFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPath = operands[0];
            var componentType = operands[1];
            int includeInactive = operands.Count > 2 ? operands[2].GetInt() : 1;
            UnityEngine.GameObject obj = objPath.IsObject ? objPath.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string path = objPath.IsString ? objPath.StringVal : null;
                if (null != path) {
                    obj = UnityEngine.GameObject.Find(path);
                }
                else {
                    try {
                        int objId = objPath.GetInt();
                        obj = null;
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
                        return comps;
                    else
                        return new List<UnityEngine.Component>();
                }
                else {
                    string name = componentType.IsString ? componentType.StringVal : null;
                    if (null != name) {
                        t = StoryScriptUtility.GetType(name);
                        if (null != t) {
                            var comps = obj.GetComponentsInChildren(t, includeInactive != 0);
                            if (null != comps)
                                return comps;
                            else
                                return new List<UnityEngine.Component>();
                        }
                    }
                }
            }
            return new List<UnityEngine.Component>();
        }
    }
    internal sealed class GetGameObjectFunction : AbstractExpression
    {
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder) {
                callData = funcData.LowerOrderFunction;
            }
            if (null != callData && callData.HaveParam()) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                }
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd) {
                        string id = cd.GetId();
                        if (id == "disable") {
                            for (int j = 0; j < cd.GetParamNum(); ++j) {
                                m_DisableComponents.Add(CodeStructure.Load(cd.GetParam(j)));
                            }
                        }
                        else if (id == "remove") {
                            for (int j = 0; j < cd.GetParamNum(); ++j) {
                                m_RemoveComponents.Add(CodeStructure.Load(cd.GetParam(j)));
                            }
                        }
                    }
                }
            }
            return true;
        }

        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var o = m_ObjPath.Calc(calculator);
            string objPath = o.IsString ? o.StringVal : null;
            StrList disables = new StrList();
            for (int i = 0; i < m_DisableComponents.Count; ++i) {
                disables.Add(m_DisableComponents[i].Calc(calculator).ToString());
            }
            StrList removes = new StrList();
            for (int i = 0; i < m_RemoveComponents.Count; ++i) {
                removes.Add(m_RemoveComponents[i].Calc(calculator).ToString());
            }
            UnityEngine.GameObject obj = null;
            if (null != objPath) {
                obj = UnityEngine.GameObject.Find(objPath);
            }
            else {
                try {
                    int objId = o.GetInt();
                    obj = null;
                }
                catch {
                }
            }
            if (null != obj) {
                foreach (string disable in disables) {
                    var type = StoryScriptUtility.GetType(disable);
                    if (null != type) {
                        var comps = obj.GetComponentsInChildren(type);
                        for (int i = 0; i < comps.Length; ++i) {
                            var t = comps[i].GetType();
                            t.InvokeMember("enabled", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, null, comps[i], new object[] { false });
                        }
                    }
                }
                foreach (string remove in removes) {
                    var type = StoryScriptUtility.GetType(remove);
                    if (null != type) {
                        var comps = obj.GetComponentsInChildren(type);
                        for (int i = 0; i < comps.Length; ++i) {
                            StoryScriptUtility.DestroyObject(comps[i]);
                        }
                    }
                }
                return obj;
            }
            return BoxedValue.NullObject;
        }
        private IExpression m_ObjPath;
        private List<IExpression> m_DisableComponents = new List<IExpression>();
        private List<IExpression> m_RemoveComponents = new List<IExpression>();
    }
    internal sealed class GetParentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var o = operands[0];
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                var obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj && null != obj.transform.parent) {
                    return obj.transform.parent.gameObject;
                }
            }
            else if (null != uobj) {
                if (null != uobj.transform.parent) {
                    return uobj.transform.parent.gameObject;
                }
            }
            else {
                try {
                    int objId = o.GetInt();
                }
                catch {
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetChildFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var o = operands[0];
            string childPath = operands[1].AsString;
            string objPath = o.IsString ? o.StringVal : null;
            UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
            if (null != objPath) {
                var obj = UnityEngine.GameObject.Find(objPath);
                if (null != obj) {
                    var t = StoryScriptUtility.FindChildRecursive(obj.transform, childPath);
                    if (null != t) {
                        return t.gameObject;
                    }
                }
            }
            else if (null != uobj) {
                var t = StoryScriptUtility.FindChildRecursive(uobj.transform, childPath);
                if (null != t) {
                    return t.gameObject;
                }
            }
            else {
                try {
                    int objId = o.GetInt();
                }
                catch {
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetUnityTypeFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string typeName = operands[0].AsString;
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string typeName = operands[0].AsString;
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string typeName = operands[0].AsString;
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
    internal sealed class GetPositionFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return (StoryScript.Vector3Obj)pt;
            }
            return (StoryScript.Vector3Obj)UnityEngine.Vector3.zero;
        }
    }
    internal sealed class GetPositionXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.x;
            }
            return 0.0f;
        }
    }
    internal sealed class GetPositionYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.y;
            }
            return 0.0f;
        }
    }
    internal sealed class GetPositionZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localPosition;
                else
                    pt = obj.transform.position;
                return pt.z;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return (StoryScript.Vector3Obj)pt;
            }
            return (StoryScript.Vector3Obj)UnityEngine.Vector3.zero;
        }
    }
    internal sealed class GetRotationXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.x;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.y;
            }
            return 0.0f;
        }
    }
    internal sealed class GetRotationZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            int local0OrWorld1 = operands.Count > 1 ? operands[1].GetInt() : 0;
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                UnityEngine.Vector3 pt;
                if (0 == local0OrWorld1)
                    pt = obj.transform.localEulerAngles;
                else
                    pt = obj.transform.eulerAngles;
                return pt.z;
            }
            return 0.0f;
        }
    }
    internal sealed class GetScaleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return (StoryScript.Vector3Obj)obj.transform.localScale;
            }
            return (StoryScript.Vector3Obj)new UnityEngine.Vector3(1, 1, 1);
        }
    }
    internal sealed class GetScaleXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.x;
            }
            return 1.0f;
        }
    }
    internal sealed class GetScaleYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.y;
            }
            return 1.0f;
        }
    }
    internal sealed class GetScaleZFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var objPathVal = operands[0];
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
                else {
                    try {
                        int id = objPathVal.IsInteger ? objPathVal.GetInt() : -1;
                        obj = null;
                    }
                    catch {
                        obj = null;
                    }
                }
            }
            if (null != obj) {
                return obj.transform.localScale.z;
            }
            return 1.0f;
        }
    }
    internal sealed class Vector3Function : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: vector3(x, y, z)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            Vector3Obj vecObj = new Vector3Obj { Value = new UnityEngine.Vector3(x, y, z) };
            return BoxedValue.FromObject(vecObj);
        }
    }
    internal sealed class Deg2RadFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float degree = operands[0].GetFloat();
            return degree * MathF.PI / 180.0f;
        }
    }
    internal sealed class Rad2DegFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float radian = operands[0].GetFloat();
            return radian * 180.0f / MathF.PI;
        }
    }
    internal sealed class GetFileNameFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string path = operands[0].AsString;
            return Path.GetFileName(path);
        }
    }
    internal sealed class GetDirectoryNameFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string path = operands[0].AsString;
            return Path.GetDirectoryName(path);
        }
    }
    internal sealed class GetExtensionFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string path = operands[0].AsString;
            return Path.GetExtension(path);
        }
    }
    internal sealed class CombinePathFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string path1 = operands[0].AsString;
            string path2 = operands[1].AsString;
            return Path.Combine(path1, path2);
        }
    }
    internal sealed class GetStreamingAssetsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return UnityEngine.Application.streamingAssetsPath;
        }
    }
    internal sealed class GetPersistentPathFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return UnityEngine.Application.persistentDataPath;
        }
    }
}
