using System;
using System.Collections;
using System.Collections.Generic;
using Dsl;
using StoryScript;
using StoryScript.DslExpression;

namespace GmCommands
{
    /// <summary>
    /// sendmessage(objname,msg,arg1,arg2,...);
    /// </summary>
    internal class SendMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2) {
                throw new Exception("Expected: sendmessage(objname,msg,...)");
            }
            string objname = operands[0].AsString;
            string msg = operands[1].AsString;
            ArrayList arglist = new ArrayList();
            for (int i = 2; i < operands.Count; ++i) {
                arglist.Add(operands[i].GetObject());
            }
            object[] args = arglist.ToArray();
            if (args.Length == 0)
                StoryScriptUtility.SendMessage(objname, msg, null);
            else if (args.Length == 1)
                StoryScriptUtility.SendMessage(objname, msg, args[0]);
            else
                StoryScriptUtility.SendMessage(objname, msg, args);
            return false;
        }
    }
    /// <summary>
    /// sendmessagewithtag(tagname,msg,arg1,arg2,...);
    /// </summary>
    internal class SendMessageWithTagCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                string objtag = operands[0].AsString;
                string msg = operands[1].AsString;
                ArrayList arglist = new ArrayList();
                for (int i = 2; i < operands.Count; ++i) {
                    arglist.Add(operands[i].GetObject());
                }
                object[] args = arglist.ToArray();
                if (args.Length == 0)
                    StoryScriptUtility.SendMessageWithTag(objtag, msg, null);
                else if (args.Length == 1)
                    StoryScriptUtility.SendMessageWithTag(objtag, msg, args[0]);
                else
                    StoryScriptUtility.SendMessageWithTag(objtag, msg, args);
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// sendmessagewithgameobject(gameobject,msg,arg1,arg2,...);
    /// </summary>
    internal class SendMessageWithGameObjectCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                var objVal = operands[0];
                UnityEngine.GameObject uobj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
                if (null == uobj) {
                    try {
                        int objId = objVal.IsInteger ? objVal.GetInt() : -1;
                        uobj = null;
                    }
                    catch {
                        uobj = null;
                    }
                }
                if (null != uobj) {
                    string msg = operands[1].AsString;
                    ArrayList arglist = new ArrayList();
                    for (int i = 2; i < operands.Count; ++i) {
                        arglist.Add(operands[i].GetObject());
                    }
                    object[] args = arglist.ToArray();
                    if (args.Length == 0)
                        uobj.SendMessage(msg, UnityEngine.SendMessageOptions.DontRequireReceiver);
                    else if (args.Length == 1)
                        uobj.SendMessage(msg, args[0], UnityEngine.SendMessageOptions.DontRequireReceiver);
                    else
                        uobj.SendMessage(msg, args, UnityEngine.SendMessageOptions.DontRequireReceiver);
                }
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// creategameobject(name, prefab[, parent])[obj("varname")]{
    ///     position(vector3(x,y,z));
    ///     rotation(vector3(x,y,z));
    ///     scale(vector3(x,y,z));
    ///     loadtimeout(1000);
    ///     disable("typename", "typename", ...);
    ///     remove("typename", "typename", ...);
    /// };
    /// </summary>
    internal sealed class CreateGameObjectCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var name = m_Name.Calc(calculator).ToString();
            var prefab = m_Prefab.Calc(calculator).ToString();
            var uo = UnityEngine.Resources.Load(prefab);
            var obj = UnityEngine.GameObject.Instantiate(uo) as UnityEngine.GameObject;
            if (null != obj) {
                if (null != m_DisableComponents) {
                    foreach (var dc in m_DisableComponents) {
                        string disable = dc.Calc(calculator).ToString();
                        var type = StoryScriptUtility.GetType(disable);
                        if (null != type) {
                            var comps = obj.GetComponentsInChildren(type);
                            for (int i = 0; i < comps.Length; ++i) {
                                var t = comps[i].GetType();
                                t.InvokeMember("enabled", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, null, comps[i], new object[] { false });
                            }
                        }
                    }
                }
                if (null != m_RemoveComponents) {
                    foreach (var rc in m_RemoveComponents) {
                        string remove = rc.Calc(calculator).ToString();
                        var type = StoryScriptUtility.GetType(remove);
                        if (null != type) {
                            var comps = obj.GetComponentsInChildren(type);
                            for (int i = 0; i < comps.Length; ++i) {
                                StoryScriptUtility.DestroyObject(comps[i]);
                            }
                        }
                    }
                }
                obj.name = name;
                if (m_HaveParent) {
                    var parentVal = m_Parent.Calc(calculator);
                    string path = parentVal.IsString ? parentVal.StringVal : null;
                    if (null != path) {
                        var pobj = UnityEngine.GameObject.Find(path);
                        if (null != pobj) {
                            obj.transform.SetParent(pobj.transform, false);
                        }
                    }
                    else {
                        UnityEngine.GameObject pobj = parentVal.IsObject ? parentVal.ObjectVal as UnityEngine.GameObject : null;
                        if (null != pobj) {
                            obj.transform.SetParent(pobj.transform, false);
                        }
                    }
                }
                if (null != m_Position) {
                    var v = m_Position.Calc(calculator);
                    var pos = (UnityEngine.Vector3)v.GetObject();
                    obj.transform.localPosition = pos;
                }
                if (null != m_Rotation) {
                    var v = m_Rotation.Calc(calculator);
                    var rot = (UnityEngine.Vector3)v.GetObject();
                    obj.transform.localEulerAngles = rot;
                }
                if (null != m_Scale) {
                    var v = m_Scale.Calc(calculator);
                    var scl = (UnityEngine.Vector3)v.GetObject();
                    obj.transform.localScale = scl;
                }
                if (m_HaveObj) {
                    string varName = m_ObjVarName.Calc(calculator).ToString();
                    var storyInst = calculator.GetFuncContext<StoryInstance>();
                    if (null != storyInst) {
                        storyInst.SetVariable(varName, obj);
                    }
                }
                return BoxedValue.FromObject(obj);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder) {
                callData = funcData.LowerOrderFunction;
            }
            if (null != callData && callData.HaveParam()) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_Name = CodeStructure.Load(callData.GetParam(0));
                    m_Prefab = CodeStructure.Load(callData.GetParam(1));
                    if (num > 2) {
                        m_HaveParent = true;
                        m_Parent = CodeStructure.Load(callData.GetParam(2));
                    }
                }
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.IsHighOrder) {
                        LoadVarName(second.LowerOrderFunction);
                    }
                    else if (second.HaveParam()) {
                        LoadVarName(second);
                    }
                }
                if (null != second && second.HaveStatement()) {
                    for (int i = 0; i < second.GetParamNum(); ++i) {
                        var cd = second.GetParam(i) as Dsl.FunctionData;
                        if (null != cd) {
                            LoadOptional(cd);
                        }
                    }
                }
            }
            return true;
        }
        private void LoadVarName(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "obj" && callData.GetParamNum() == 1) {
                m_ObjVarName = CodeStructure.Load(callData.GetParam(0));
                m_HaveObj = true;
            }
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Position = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Rotation = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "scale") {
                if (num == 3)
                    m_Scale = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Scale = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "disable") {
                if (m_DisableComponents == null)
                    m_DisableComponents = new List<IExpression>();
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    m_DisableComponents.Add(CodeStructure.Load(callData.GetParam(i)));
                }
            }
            else if (id == "remove") {
                if (m_RemoveComponents == null)
                    m_RemoveComponents = new List<IExpression>();
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    m_RemoveComponents.Add(CodeStructure.Load(callData.GetParam(i)));
                }
            }
        }

        private IExpression m_Name;
        private IExpression m_Prefab;
        private IExpression m_Parent;
        private bool m_HaveParent = false;
        private bool m_HaveObj = false;
        private IExpression m_ObjVarName;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
        private List<IExpression> m_DisableComponents;
        private List<IExpression> m_RemoveComponents;
    }

    /// <summary>
    /// settransform(name, world_or_local){
    ///     position(vector3(x,y,z));
    ///     rotation(vector3(x,y,z));
    ///     scale(vector3(x,y,z));
    /// };
    /// </summary>
    internal sealed class SetTransformCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var objVal = m_ObjPath.Calc(calculator);
            int local0OrWorld1 = m_LocalOrWorld.Calc(calculator).GetInt();
            UnityEngine.GameObject obj = objVal.IsString ? UnityEngine.GameObject.Find(objVal.StringVal) : objVal.ObjectVal as UnityEngine.GameObject;
            if (null == obj) return BoxedValue.NullObject;
            if (null != m_Position) {
                var v = m_Position.Calc(calculator).As<Vector3Obj>().Value;
                if (0 == local0OrWorld1)
                    obj.transform.localPosition = v;
                else
                    obj.transform.position = v;
            }
            if (null != m_Rotation) {
                var v = m_Rotation.Calc(calculator).As<Vector3Obj>().Value;
                if (0 == local0OrWorld1)
                    obj.transform.localEulerAngles = v;
                else
                    obj.transform.eulerAngles = v;
            }
            if (null != m_Scale) {
                var v = m_Scale.Calc(calculator).As<Vector3Obj>().Value;
                obj.transform.localScale = v;
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder)
                callData = funcData.LowerOrderFunction;
            if (null != callData && callData.GetParamNum() > 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                m_LocalOrWorld = CodeStructure.Load(callData.GetParam(1));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Position = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Rotation = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "scale") {
                if (num == 3)
                    m_Scale = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Scale = CodeStructure.Load(callData.GetParam(0));
            }
        }
        private IExpression m_ObjPath;
        private IExpression m_LocalOrWorld;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
    }
    /// <summary>
    /// addtransform(name, world_or_local){
    ///     position(vector3(x,y,z));
    ///     rotation(vector3(x,y,z));
    ///     scale(vector3(x,y,z));
    /// };
    /// </summary>
    internal sealed class AddTransformCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var objVal = m_ObjPath.Calc(calculator);
            int local0OrWorld1 = m_LocalOrWorld.Calc(calculator).GetInt();
            UnityEngine.GameObject obj = objVal.IsString ? UnityEngine.GameObject.Find(objVal.StringVal) : objVal.ObjectVal as UnityEngine.GameObject;
            if (null == obj) return BoxedValue.NullObject;
            if (null != m_Position) {
                var v = m_Position.Calc(calculator).As<Vector3Obj>().Value;
                if (0 == local0OrWorld1)
                    obj.transform.localPosition += v;
                else
                    obj.transform.position += v;
            }
            if (null != m_Rotation) {
                var v = m_Rotation.Calc(calculator).As<Vector3Obj>().Value;
                if (0 == local0OrWorld1)
                    obj.transform.localEulerAngles += v;
                else
                    obj.transform.eulerAngles += v;
            }
            if (null != m_Scale) {
                var v = m_Scale.Calc(calculator).As<Vector3Obj>().Value;
                obj.transform.localScale += v;
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder)
                callData = funcData.LowerOrderFunction;
            if (null != callData && callData.GetParamNum() > 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                m_LocalOrWorld = CodeStructure.Load(callData.GetParam(1));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd) {
                        LoadOptional(cd);
                    }
                }
            }
            return true;
        }
        private void LoadOptional(Dsl.FunctionData callData)
        {
            string id = callData.GetId();
            int num = callData.GetParamNum();
            if (id == "position") {
                if (num == 3)
                    m_Position = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Position = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "rotation") {
                if (num == 3)
                    m_Rotation = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Rotation = CodeStructure.Load(callData.GetParam(0));
            }
            else if (id == "scale") {
                if (num == 3)
                    m_Scale = CodeStructure.Load(callData);
                else if (num > 0)
                    m_Scale = CodeStructure.Load(callData.GetParam(0));
            }
        }
        private IExpression m_ObjPath;
        private IExpression m_LocalOrWorld;
        private IExpression m_Position;
        private IExpression m_Rotation;
        private IExpression m_Scale;
    }
    /// <summary>
    /// destroygameobject(path);
    /// </summary>
    internal class DestroyGameObjectCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var pathVal = operands[0];
            string path = pathVal.IsString ? pathVal.StringVal : null;
            if (null != path) {
                var obj = UnityEngine.GameObject.Find(path);
                if (null != obj) {
                    obj.transform.SetParent(null);
                }
            }
            else {
                var obj = pathVal.IsObject ? pathVal.ObjectVal as UnityEngine.GameObject : null;
                if (null != obj) {
                    obj.transform.SetParent(null);
                    StoryScriptUtility.DestroyObject(obj);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setparent(objpath,parent,stay_world_pos);
    /// </summary>
    internal sealed class SetParentCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            var parentVal = operands[1];
            int stayWorldPos = operands.Count > 2 ? operands[2].GetInt() : 0;
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null != obj) {
                UnityEngine.GameObject pobj = parentVal.IsObject ? parentVal.ObjectVal as UnityEngine.GameObject : null;
                if (null == pobj) {
                    string parentPath = parentVal.IsString ? parentVal.StringVal : null;
                    if (null != parentPath && !string.IsNullOrEmpty(parentPath)) {
                        pobj = UnityEngine.GameObject.Find(parentPath);
                    }
                }
                obj.transform.SetParent(null != pobj ? pobj.transform : null, stayWorldPos != 0);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setactive(objpath,1_or_0);
    /// </summary>
    internal sealed class SetActiveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            int active = operands[1].GetInt();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null != obj) {
                obj.SetActive(active != 0);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setvisible(objpath,1_or_0);
    /// </summary>
    internal sealed class SetVisibleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            var objVal = operands[0];
            int visible = operands[1].GetInt();
            UnityEngine.GameObject obj = objVal.IsObject ? objVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objVal.IsString ? objVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null != obj) {
                var renderers = obj.GetComponentsInChildren<UnityEngine.Renderer>();
                if (null != renderers) {
                    for (int i = 0; i < renderers.Length; ++i) {
                        renderers[i].enabled = visible != 0;
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// addcomponent(objpath,type)[obj("varname")];
    /// </summary>
    internal sealed class AddComponentCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var objPathVal = m_ObjPath.Calc(calculator);
            var componentType = m_ComponentType.Calc(calculator);
            UnityEngine.GameObject obj = objPathVal.IsObject ? objPathVal.ObjectVal as UnityEngine.GameObject : null;
            if (null == obj) {
                string objPath = objPathVal.IsString ? objPathVal.StringVal : null;
                if (null != objPath) {
                    obj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null == obj) return BoxedValue.NullObject;
            UnityEngine.Component component = null;
            Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
            if (null == t) {
                string name = componentType.IsString ? componentType.StringVal : null;
                if (null != name) {
                    t = StoryScriptUtility.GetType(name);
                }
            }
            if (null != t) {
                component = obj.AddComponent(t);
            }
            if (m_HaveObj && null != component) {
                string varName = m_ObjVarName.Calc(calculator).ToString();
                var storyInst = calculator.GetFuncContext<StoryInstance>();
                if (null != storyInst) {
                    storyInst.SetVariable(varName, component);
                }
            }
            return component != null ? BoxedValue.FromObject(component) : BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder)
                callData = funcData.LowerOrderFunction;
            if (null != callData && callData.GetParamNum() > 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                m_ComponentType = CodeStructure.Load(callData.GetParam(1));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var cd = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != cd) {
                        LoadVarName(cd);
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadVarName(second);
                }
            }
            return true;
        }
        private void LoadVarName(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "obj" && callData.GetParamNum() == 1) {
                m_ObjVarName = CodeStructure.Load(callData.GetParam(0));
                m_HaveObj = true;
            }
        }
        private IExpression m_ObjPath;
        private IExpression m_ComponentType;
        private bool m_HaveObj = false;
        private IExpression m_ObjVarName;
    }
    /// <summary>
    /// removecomponent(objpath,type);
    /// </summary>
    internal class RemoveComponentCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                var objPathVal = operands[0];
                var componentType = operands[1];
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
                    Type t = componentType.IsObject ? componentType.ObjectVal as Type : null;
                    if (null != t) {
                        var comp = obj.GetComponent(t);
                        StoryScriptUtility.DestroyObject(comp);
                    }
                    else {
                        string name = componentType.IsString ? componentType.StringVal : null;
                        if (null != name) {
                            t = StoryScriptUtility.GetType(name);
                            var comp = obj.GetComponent(t);
                            StoryScriptUtility.DestroyObject(comp);
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// openurl(url);
    /// </summary>
    internal class OpenUrlCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                string url = operands[0].AsString;
                UnityEngine.Application.OpenURL(url);
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// quit();
    /// </summary>
    internal class QuitCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            UnityEngine.Application.Quit();
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// gameobjectanimation(obj, anim[, normalized_time]);
    /// </summary>
    internal class GameObjectAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1) {
                var o = operands[0];
                string objPath = o.IsString ? o.StringVal : null;
                UnityEngine.GameObject uobj = o.IsObject ? o.ObjectVal as UnityEngine.GameObject : null;
                if (null == uobj) {
                    if (null != objPath) {
                        uobj = UnityEngine.GameObject.Find(objPath);
                    }
                    else {
                        try {
                            int objId = o.GetInt();
                            uobj = null;
                        }
                        catch {
                            uobj = null;
                        }
                    }
                }
                if (null != uobj) {
                    string anim = operands[1].AsString;
                    var animators = uobj.GetComponentsInChildren<UnityEngine.Animator>();
                    if (null != animators) {
                        for (int i = 0; i < animators.Length; ++i) {
                            animators[i].Play(anim);
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// gameobjectanimationparam(obj)
    /// {
    ///     float(name,val);
    ///     int(name,val);
    ///     bool(name,val);
    ///     trigger(name,val);
    /// };
    /// </summary>
    internal sealed class GameObjectAnimationParamCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var o = m_ObjPath.Calc(calculator);
            UnityEngine.GameObject obj = o.IsString ? UnityEngine.GameObject.Find(o.StringVal) : o.ObjectVal as UnityEngine.GameObject;
            if (null == obj) return BoxedValue.NullObject;
            UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
            if (null == animator) return BoxedValue.NullObject;
            for (int i = 0; i < m_Params.Count; ++i) {
                var param = m_Params[i];
                string type = param.Type;
                string key = param.Key.Calc(calculator).ToString();
                var val = param.Value.Calc(calculator);
                if (type == "int") {
                    animator.SetInteger(key, val.GetInt());
                }
                else if (type == "float") {
                    animator.SetFloat(key, val.GetFloat());
                }
                else if (type == "bool") {
                    animator.SetBool(key, val.GetBool());
                }
                else if (type == "trigger") {
                    string v = val.ToString();
                    if (v == "false") {
                        animator.ResetTrigger(key);
                    }
                    else {
                        animator.SetTrigger(key);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.FunctionData callData = funcData;
            if (funcData.IsHighOrder)
                callData = funcData.LowerOrderFunction;
            if (null != callData && callData.GetParamNum() >= 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent statement = funcData.GetParam(i);
                    Dsl.FunctionData stCall = statement as Dsl.FunctionData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        ParamInfo param = new ParamInfo();
                        param.Type = id;
                        param.Key = CodeStructure.Load(stCall.GetParam(0));
                        param.Value = CodeStructure.Load(stCall.GetParam(1));
                        m_Params.Add(param);
                    }
                }
            }
            return true;
        }
        private class ParamInfo
        {
            internal string Type;
            internal IExpression Key;
            internal IExpression Value;
            internal ParamInfo()
            {
                Type = string.Empty;
            }
        }
        private IExpression m_ObjPath;
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
}
