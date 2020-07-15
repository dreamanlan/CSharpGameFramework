using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

[CustomEditor(typeof(AnimatorController))]
public class AnimatorControllerImportExporter : Editor
{
    public override void OnInspectorGUI()
    {
        AnimatorController ctrl = (AnimatorController)target;
        if (GUILayout.Button("导入全部")) {
            string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
            if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
                AnimatorControllerUtility.ImportAll(path, ctrl);
            }
        }
        if (GUILayout.Button("导入部分（不处理any/entry/exit状态）")) {
            string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
            if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
                AnimatorControllerUtility.ImportPartial(path, ctrl);
            }
        }
        if (GUILayout.Button("导出全部")) {
            string path = EditorUtility.SaveFilePanel("请指定导出的dsl文件", string.Empty, string.Empty, "dsl");
            if (!string.IsNullOrEmpty(path) && Directory.Exists(Path.GetDirectoryName(path))) {
                AnimatorControllerUtility.ExportAll(path, ctrl);
            }
        }
    }
}

public sealed class AnimatorControllerEditWindow : EditorWindow
{
    [MenuItem("工具/AnimatorController批量导入")]
    internal static void InitWindow()
    {
        AnimatorControllerEditWindow window = (AnimatorControllerEditWindow)EditorWindow.GetWindow(typeof(AnimatorControllerEditWindow));
        window.minSize = new Vector2(640, 480);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("收集所有动画控制器")) {
            Collect();
        }
        if (GUILayout.Button("批量导入部分（不处理any/entry/exit状态）")) {
            BatchImportPartial();
        }
        if (GUILayout.Button("批量导入全部")) {
            BatchImportAll();
        }
        if (GUILayout.Button("清零ExitTime")) {
            CleanupExitTime();
        }
        EditorGUILayout.EndHorizontal();

        m_Pos = EditorGUILayout.BeginScrollView(m_Pos, true, true);

        ListItem();

        EditorGUILayout.EndScrollView();
    }

    private void Collect()
    {
        string path = EditorUtility.OpenFolderPanel("请选择要收集动画控制器的根目录", Application.dataPath, string.Empty);
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
            m_Controllers.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountFileRecursively(path);
            if (m_TotalSearchCount > 0) {
                SearchFileRecursively(path);
                EditorUtility.ClearProgressBar();
            }
        }
    }

    private void BatchImportPartial()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            foreach (ItemInfo item in m_Controllers) {
                if (item.Selected) {
                    AnimatorController ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(item.Path);
                    if (null != ctrl) {
                        AnimatorControllerUtility.ImportPartial(path, ctrl);
                        Debug.Log("BatchImportPartial " + item.Path);
                    }
                }
            }
			EditorUtility.DisplayDialog ("提示", "处理完成", "ok");
        }
    }

    private void BatchImportAll()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            foreach (ItemInfo item in m_Controllers) {
                if (item.Selected) {
                    AnimatorController ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(item.Path);
                    if (null != ctrl) {
                        AnimatorControllerUtility.ImportAll(path, ctrl);
                        Debug.Log("BatchImportAll " + item.Path);
                    }
                }
            }
			EditorUtility.DisplayDialog ("提示", "处理完成", "ok");
        }
    }

    private void CleanupExitTime()
    {
        var objs = Selection.objects;
        foreach (var obj in objs) {
            var path = AssetDatabase.GetAssetPath(obj);            
            AnimatorController ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (null != ctrl) {
                foreach (var layer in ctrl.layers) {
                    var sm = layer.stateMachine;
                    AnimatorControllerUtility.CleanupStateMachineExitTime(sm);
                }
                AssetDatabase.SaveAssets();
            }
        }
    }


    private void ListItem()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (ItemInfo item in m_Controllers) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (ItemInfo item in m_Controllers) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Go to page ({0}) : ", m_Controllers.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = GUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        GUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_Controllers.Count / c_ItemsPerPage + 1, m_Page));
        GUILayout.EndVertical();
        int index = 0;
        int totalShown = 0;
        foreach (ItemInfo item in m_Controllers) {
            ++index;
            if (index <= (m_Page - 1) * c_ItemsPerPage)
                continue;
            ++totalShown;
            if (totalShown > c_ItemsPerPage)
                break;
            EditorGUILayout.BeginHorizontal();
            item.Selected = GUILayout.Toggle(item.Selected, index + ".", GUILayout.Width(40));
            Texture icon = AssetDatabase.GetCachedIcon(item.Path);
            var oldAlignment = GUI.skin.button.alignment;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            if (GUILayout.Button(new GUIContent(item.Path, icon, item.Path))) {
                SelectObject(item.Path);
            }
            GUI.skin.button.alignment = oldAlignment;
            EditorGUILayout.EndHorizontal();
        }
    }

    private void SearchFileRecursively(string dir)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            return Path.GetExtension(file).ToLower() == ".controller";
        }).ToArray();
        foreach (string file in files) {
            m_Controllers.Add(new ItemInfo { Path = PathToAssetPath(file), Selected = false });
            ++m_CurSearchCount;
            EditorUtility.DisplayProgressBar("采集进度", string.Format("{0} in {1}/{2}", m_Controllers.Count, m_CurSearchCount, m_TotalSearchCount), m_CurSearchCount * 1.0f / m_TotalSearchCount);
        }        
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            SearchFileRecursively(subDir);
        }
    }

    private void CountFileRecursively(string dir)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            return Path.GetExtension(file).ToLower() == ".controller";
        }).ToArray();
        m_TotalSearchCount += files.Length;

        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            CountFileRecursively(subDir);
        }
    }

    private string PathToAssetPath(string path)
    {
        string rootPath = Application.dataPath.Replace('\\','/');
        path = path.Replace('\\', '/');
        if (path.StartsWith(rootPath)) {
            return "Assets" + path.Substring(rootPath.Length);
        } else {
            return path;
        }
    }

    private static void SelectObject(string assetPath)
    {
        var objLoaded = AssetDatabase.LoadMainAssetAtPath(assetPath);
        if (objLoaded != null) {
            if (Selection.activeObject != null && !(Selection.activeObject is GameObject)) {
                Resources.UnloadAsset(Selection.activeObject);
                Selection.activeObject = null;
            }
            Selection.activeObject = objLoaded;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
    }

    private class ItemInfo
    {
        internal string Path;
        internal bool Selected;
    }

    private List<ItemInfo> m_Controllers = new List<ItemInfo>();
    private Vector2 m_Pos = Vector2.zero;
    private int m_Page = 1;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;

    private const int c_ItemsPerPage = 50;
}

public static class AnimatorControllerUtility
{
    public static void ImportAll(string path, AnimatorController ctrl)
    {
        s_ImportAll = true;
        Import(path, ctrl);
    }
    public static void ImportPartial(string path, AnimatorController ctrl)
    {
        s_ImportAll = false;
        Import(path, ctrl);
    }
    public static void ExportAll(string path, AnimatorController ctrl)
    {
        int indent = 0;
        using (StreamWriter sw = new StreamWriter(path)) {
            sw.WriteLine("{0}animatorcontroller(\"{1}\")", GetIndent(indent), ctrl.name);
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            sw.WriteLine("{0}parameters", GetIndent(indent));
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            foreach (var parameter in ctrl.parameters) {
                switch (parameter.type) {
                    case AnimatorControllerParameterType.Trigger:
                        sw.WriteLine("{0}trigger(\"{1}\");", GetIndent(indent), parameter.name);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        sw.WriteLine("{0}bool(\"{1}\",{2});", GetIndent(indent), parameter.name, parameter.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Float:
                        sw.WriteLine("{0}float(\"{1}\",{2});", GetIndent(indent), parameter.name, parameter.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Int:
                        sw.WriteLine("{0}float(\"{1}\",{2});", GetIndent(indent), parameter.name, parameter.defaultInt);
                        break;
                }
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            sw.WriteLine();
            foreach (var layer in ctrl.layers) {
                sw.WriteLine("{0}layer(\"{1}\")", GetIndent(indent), layer.name);
                sw.WriteLine("{0}{1}", GetIndent(indent), "{");
                ++indent;
                sw.WriteLine("{0}weight({1});", GetIndent(indent), layer.defaultWeight);
                sw.WriteLine("{0}blendingmode(\"{1}\");", GetIndent(indent), layer.blendingMode);
                sw.WriteLine();
                WriteAnimatorStateMachine(sw, indent, layer.stateMachine, Vector3.zero, layer);
                --indent;
                sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
    }

    public static void CleanupStateMachineExitTime(AnimatorStateMachine stateMachine)
    {
        foreach (var state in stateMachine.states) {
            foreach (var trans in state.state.transitions) {
                trans.exitTime = 1;
                trans.duration = 0;
                trans.offset = 0;
            }
        }
        foreach (var sm in stateMachine.stateMachines) {
            CleanupStateMachineExitTime(sm.stateMachine);            
        }
    }

    private static void Import(string path, AnimatorController ctrl)
    {
        Dsl.DslFile file = new Dsl.DslFile();
        if (file.Load(path, (string msg) => { Debug.Log(msg); })) {
            if (file.DslInfos.Count == 1) {
                Dsl.ISyntaxComponent dslInfo = file.DslInfos[0];
                if (dslInfo.GetId() == "animatorcontroller") {
                    var func = dslInfo as Dsl.FunctionData;
                    var stData = dslInfo as Dsl.StatementData;
                    if (null == func && null != stData) {
                        func = stData.First;
                    }
                    if (null != func) {
                        foreach (Dsl.ISyntaxComponent comp in func.Params) {
                            if (comp.GetId() == "parameters") {
                                ReadAnimatorParameter(comp as Dsl.FunctionData, ctrl);
                            }
                            else if (comp.GetId() == "layer") {
                                ReadAnimatorLayer(comp as Dsl.FunctionData, ctrl);
                            }
                        }
                    }
                }
            }
        }
    }

    private static void ReadAnimatorParameter(Dsl.FunctionData funcData, AnimatorController ctrl)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                string name = callData.GetParamId(0);
                AnimatorControllerParameter param = FindAnimatorParameter(ctrl, name);
                if (callData.GetId() == "trigger") {
                    if (null != param) {
                        param.type = AnimatorControllerParameterType.Trigger;
                    } else {
                        ctrl.AddParameter(name, AnimatorControllerParameterType.Trigger);
                    }
                } else if (callData.GetId() == "bool") {
                    bool val = bool.Parse(callData.GetParamId(1));
                    if (null != param) {
                        param.type = AnimatorControllerParameterType.Bool;
                        param.defaultBool = val;
                    } else {
                        param = new AnimatorControllerParameter();
                        param.name = name;
                        param.type = AnimatorControllerParameterType.Bool;
                        param.defaultBool = val;
                        ctrl.AddParameter(param);
                    }
                } else if (callData.GetId() == "float") {
                    float val = float.Parse(callData.GetParamId(1));
                    if (null != param) {
                        param.type = AnimatorControllerParameterType.Float;
                        param.defaultFloat = val;
                    } else {
                        param = new AnimatorControllerParameter();
                        param.name = name;
                        param.type = AnimatorControllerParameterType.Float;
                        param.defaultFloat = val;
                        ctrl.AddParameter(param);
                    }
                } else if (callData.GetId() == "int") {
                    int val = int.Parse(callData.GetParamId(1));
                    if (null != param) {
                        param.type = AnimatorControllerParameterType.Int;
                        param.defaultInt = val;
                    } else {
                        param = new AnimatorControllerParameter();
                        param.name = name;
                        param.type = AnimatorControllerParameterType.Int;
                        param.defaultInt = val;
                        ctrl.AddParameter(param);
                    }
                }
            }
        }
    }
    private static void ReadAnimatorLayer(Dsl.FunctionData funcData, AnimatorController ctrl)
    {
        var funcHeader = funcData;
        if (funcData.IsHighOrder)
            funcHeader = funcData.LowerOrderFunction;
        string name = funcHeader.GetParamId(0);
        if (!string.IsNullOrEmpty(name)) {
            int layerIndex;
            AnimatorControllerLayer layer = FindAnimatorLayer(ctrl, name, out layerIndex);
            if (null == layer) {
                ctrl.AddLayer(name);
                layer = FindAnimatorLayer(ctrl, name, out layerIndex);
            }
            if (null != layer) {
                foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
                    Dsl.FunctionData callData = comp as Dsl.FunctionData;
                    if (null != callData) {
                        string id = callData.GetId();
                        if (id == "weight") {
                            float val = float.Parse(callData.GetParamId(0));
                            layer.defaultWeight = val;
                        } else if (id == "blendingmode") {
                            string m = callData.GetParamId(0);
                            AnimatorLayerBlendingMode mode = GameFramework.Converter.ConvertStrToEnum<AnimatorLayerBlendingMode>(m);
                            layer.blendingMode = mode;
                        }
                    }
                }
                HashSet<string> stateMachines = new HashSet<string>();
                HashSet<string> states = new HashSet<string>();
                ReadAnimatorStateMachine1(funcData, layer.stateMachine, layer, layerIndex, ctrl, stateMachines, states);
                CleanOldTransitions(layer.stateMachine, stateMachines, states);
                ReadAnimatorStateMachine2(funcData, layer.stateMachine, layer, layerIndex, ctrl);
            }
        }
    }
    private static void ReadAnimatorState(Dsl.FunctionData funcData, AnimatorState state, AnimatorStateMachine stateMachine, AnimatorControllerLayer layer, int layerIndex, AnimatorController ctrl)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            string id = comp.GetId();
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                string val = callData.GetParamId(0);
                if (id == "tag") {
                    state.tag = val;
                } else if (id == "layerdefaultstate" && s_ImportAll) {
                    bool v = bool.Parse(val);
                    if (v) {
                        layer.stateMachine.defaultState = state;
                    }
                } else if (id == "defaultstate" && s_ImportAll) {
                    bool v = bool.Parse(val);
                    if (v) {
                        stateMachine.defaultState = state;
                    }
                } else if (id == "speed") {
                    state.speed = float.Parse(val);
                } else if (id == "speedparameter") {
                    state.speedParameterActive = true;
                    state.speedParameter = val;
                } else if (id == "cycleoffset") {
                    state.cycleOffset = float.Parse(val);
                } else if (id == "cycleoffsetparameter") {
                    state.cycleOffsetParameterActive = true;
                    state.cycleOffsetParameter = val;
                } else if (id == "mirror") {
                    state.mirror = bool.Parse(val);
                } else if (id == "mirrorparameter") {
                    state.mirrorParameterActive = true;
                    state.mirrorParameter = val;
                } else if (id == "footik") {
                    state.iKOnFeet = bool.Parse(val);
                } else if (id == "writedefault") {
                    state.writeDefaultValues = bool.Parse(val);
                }
            } else {
                Dsl.FunctionData subFuncData = comp as Dsl.FunctionData;
                if (null != subFuncData) {
                    if (id == "blendtree") {
                        BlendTree blendTree = state.motion as BlendTree;
                        if (null != blendTree) {
                            ReadAnimatorBlendTree(subFuncData, blendTree);
                        }
                    } else if (id == "transition") {
                        Dsl.FunctionData callData2 = subFuncData;
                        if (subFuncData.IsHighOrder)
                            callData2 = subFuncData.LowerOrderFunction;
                        AnimatorStateTransition tran = null;
                        int paramNum = callData2.GetParamNum();
                        if (paramNum >= 1) {
                            string destName = callData2.GetParamId(0);
                            if (paramNum >= 2) {
                                tran = FindAnimatorStateTransition2(state.transitions, destName);
                                if (null == tran) {
                                    AnimatorStateMachine outerStateMachine = FindAnimatorStateMachineRecurively(layer.stateMachine.stateMachines, destName);
                                    if (null != outerStateMachine) {
                                        tran = state.AddTransition(outerStateMachine);
                                    }
                                }
                            } else {
                                tran = FindAnimatorStateTransition(state.transitions, destName);
                                if (null == tran) {
                                    AnimatorState outerState = FindAnimatorStateRecurively(layer.stateMachine, destName);
                                    if (null != outerState) {
                                        tran = state.AddTransition(outerState);
                                    }
                                }
                            }
                        } else if(s_ImportAll) {
                            tran = FindAnimatorStateExitTransition(state.transitions);
                            if (null == tran) {
                                tran = state.AddExitTransition();
                            }
                        }
                        if (null != tran) {
                            ReadAnimatorStateTransition(subFuncData, tran);
                        }
                    }
                }
            }
        }
    }
    private static void ReadAnimatorBlendTree(Dsl.FunctionData funcData, BlendTree tree)
    {
        int motionIndex = 0;
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            string id = comp.GetId();
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                string val = callData.GetParamId(0);
                if (id == "type") {
                    tree.blendType = GameFramework.Converter.ConvertStrToEnum<BlendTreeType>(val);
                } else if (id == "parameter") {
                    tree.blendParameter = val;
                } else if (id == "parametery") {
                    tree.blendParameterY = val;
                } else if (id == "automatethreshold") {
                    tree.useAutomaticThresholds = bool.Parse(val);
                }
            } else {
                Dsl.FunctionData subFuncData = comp as Dsl.FunctionData;
                if (null != subFuncData) {
                    if (id == "motion") {                        
                        ReadAnimatorMotion(subFuncData, tree, false, motionIndex);
                        ++motionIndex;
                    } else if (id == "blendtreemotion") {
                        ReadAnimatorMotion(subFuncData, tree, true, motionIndex);
                        ++motionIndex;
                    }
                }
            }
        }
        /*
        if (motionIndex < tree.children.Length) {
            for (int i = tree.children.Length - 1; i >= motionIndex; --i) {
                tree.RemoveChild(i);
            }
        }
        */
    }
    private static void ReadAnimatorMotion(Dsl.FunctionData funcData, BlendTree tree, bool isBlendTreeMotion, int motionIndex)
    {
        BlendTree blendTree = null;
        ChildMotion motion;
        if (motionIndex < tree.children.Length) {
            motion = tree.children[motionIndex];
            if (isBlendTreeMotion) {
                blendTree = motion.motion as BlendTree;
                if (null == blendTree) {
                    blendTree = new BlendTree();
                    blendTree.name = "BlendTree";
                    blendTree.hideFlags = HideFlags.HideInHierarchy;
                    if (AssetDatabase.GetAssetPath(tree) != string.Empty) {
                        AssetDatabase.AddObjectToAsset(blendTree, AssetDatabase.GetAssetPath(tree));
                    }
                    motion.motion = blendTree;
                }
            }
        } else {
            if (isBlendTreeMotion) {
                blendTree = tree.CreateBlendTreeChild(0);
            } else {
                tree.AddChild(null);
            }
            motion = tree.children[tree.children.Length - 1];
        }
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            string id = comp.GetId();
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                string val = callData.GetParamId(0);
                if (id == "mirror") {
                    motion.mirror = bool.Parse(val);
                } else if (id == "timescale") {
                    motion.timeScale = float.Parse(val);
                } else if (id == "threshold") {
                    motion.threshold = float.Parse(val);
                } else if (id == "parameter") {
                    motion.directBlendParameter = val;
                } else if (id == "cycleoffset") {
                    motion.cycleOffset = float.Parse(val);
                } else if (id == "position") {
                    float x = float.Parse(val);
                    float y = float.Parse(callData.GetParamId(1));
                    motion.position = new Vector2(x, y);
                }
            } else {
                Dsl.FunctionData subFuncData = comp as Dsl.FunctionData;
                if (null != subFuncData) {
                    if (id == "blendtree" && null != blendTree) {
                        ReadAnimatorBlendTree(subFuncData, blendTree);
                    }
                }
            }
        }
    }
    private static void ReadAnimatorStateMachine1(Dsl.FunctionData funcData, AnimatorStateMachine stateMachine, AnimatorControllerLayer layer, int layerIndex, AnimatorController ctrl, HashSet<string> stateMachines, HashSet<string> states)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            Dsl.FunctionData subFuncData = comp as Dsl.FunctionData;
            if (null != subFuncData) {
                var funcHeader = subFuncData;
                if (subFuncData.IsHighOrder)
                    funcHeader = subFuncData.LowerOrderFunction;
                string id = subFuncData.GetId();
                string name = funcHeader.GetParamId(0);
                if (id == "state") {
                    states.Add(name);
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    AnimatorState state = FindAnimatorState(stateMachine, name);
                    if (null == state) {
                        if (pos.sqrMagnitude < GameFramework.Geometry.c_FloatPrecision)
                            state = stateMachine.AddState(name);
                        else
                            state = stateMachine.AddState(name, pos);
                    }
                } else if (id == "blendtreestate") {
                    states.Add(name);
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    BlendTree tree;
                    AnimatorState state = FindAnimatorState(stateMachine, name);
                    if (null == state) {
                        state = ctrl.CreateBlendTreeInController(name, out tree, layerIndex);
                        if (pos.sqrMagnitude >= GameFramework.Geometry.c_FloatPrecision) {
                            int len = stateMachine.states.Length - 1;
                            stateMachine.states[len].position = pos;
                        }
                    } else {
                        tree = state.motion as BlendTree;
                        if (null == tree) {
                            state = ctrl.CreateBlendTreeInController(name, out tree, layerIndex);
                            if (pos.sqrMagnitude >= GameFramework.Geometry.c_FloatPrecision) {
                                int len = stateMachine.states.Length - 1;
                                stateMachine.states[len].position = pos;
                            }
                        }
                    }
                } else if (id == "statemachine") {
                    stateMachines.Add(name);
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    AnimatorStateMachine subStateMachine = FindAnimatorStateMachine(stateMachine, name);
                    if (null == subStateMachine) {
                        if (pos.sqrMagnitude < GameFramework.Geometry.c_FloatPrecision)
                            subStateMachine = stateMachine.AddStateMachine(name);
                        else
                            subStateMachine = stateMachine.AddStateMachine(name, pos);
                    }
                    ReadAnimatorStateMachine1(subFuncData, subStateMachine, layer, layerIndex, ctrl, stateMachines, states);
                }
            }
        }
    }
    private static void CleanOldTransitions(AnimatorStateMachine stateMachine, HashSet<string> stateMachines, HashSet<string> states)
    {
        foreach (var st in stateMachine.states) {
            var state = st.state;
            if (states.Contains(st.state.name)) {
                bool needContinue=true;
                while (needContinue) {
                    needContinue = false;
                    for (int ix = state.transitions.Length - 1; ix >= 0; --ix) {
                        AnimatorStateTransition tran = state.transitions[ix];
                        if (null != tran.destinationState && states.Contains(tran.destinationState.name)) {
                            state.RemoveTransition(tran);
                            needContinue = true;
                            break;
                        }
                        if (null != tran.destinationStateMachine && stateMachines.Contains(tran.destinationStateMachine.name)) {
                            state.RemoveTransition(tran);
                            needContinue = true;
                            break;
                        }
                        if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit && s_ImportAll) {
                            state.RemoveTransition(tran);
                            needContinue = true;
                            break;
                        }
                    }
                }
            }
        }
        if (s_ImportAll) {
            bool needContinue = true;
            while (needContinue) {
                needContinue = false;
                for (int ix = stateMachine.anyStateTransitions.Length - 1; ix >= 0; --ix) {
                    AnimatorStateTransition tran = stateMachine.anyStateTransitions[ix];
                    if (null != tran.destinationState && states.Contains(tran.destinationState.name)) {
                        stateMachine.RemoveAnyStateTransition(tran);
                        needContinue = true;
                        break;
                    }
                    if (null != tran.destinationStateMachine && stateMachines.Contains(tran.destinationStateMachine.name)) {
                        stateMachine.RemoveAnyStateTransition(tran);
                        needContinue = true;
                        break;
                    }
                    if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit && s_ImportAll) {
                        stateMachine.RemoveAnyStateTransition(tran);
                        needContinue = true;
                        break;
                    }
                }
            }
            needContinue = true;
            while (needContinue) {
                needContinue = false;
                for (int ix = stateMachine.entryTransitions.Length - 1; ix >= 0; --ix) {
                    AnimatorTransition tran = stateMachine.entryTransitions[ix];
                    if (null != tran.destinationState && states.Contains(tran.destinationState.name)) {
                        stateMachine.RemoveEntryTransition(tran);
                        needContinue = true;
                        break;
                    }
                    if (null != tran.destinationStateMachine && stateMachines.Contains(tran.destinationStateMachine.name)) {
                        stateMachine.RemoveEntryTransition(tran);
                        needContinue = true;
                        break;
                    }
                    if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit && s_ImportAll) {
                        stateMachine.RemoveEntryTransition(tran);
                        needContinue = true;
                        break;
                    }
                }
            }
        }
        bool needCont = true;
        while (needCont) {
            needCont = false;
            var trans = stateMachine.GetStateMachineTransitions(stateMachine);
            for (int ix = trans.Length - 1; ix >= 0; --ix) {
                AnimatorTransition tran = trans[ix];
                if (null != tran.destinationState && states.Contains(tran.destinationState.name)) {
                    stateMachine.RemoveStateMachineTransition(stateMachine, tran);
                    needCont = true;
                    break;
                }
                if (null != tran.destinationStateMachine && stateMachines.Contains(tran.destinationStateMachine.name)) {
                    stateMachine.RemoveStateMachineTransition(stateMachine, tran);
                    needCont = true;
                    break;
                }
                if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit && s_ImportAll) {
                    stateMachine.RemoveStateMachineTransition(stateMachine, tran);
                    needCont = true;
                    break;
                }
            }
        }
        foreach (var sm in stateMachine.stateMachines) {
            var subStateMachine = sm.stateMachine;
            if (stateMachines.Contains(subStateMachine.name)) {
                CleanOldTransitions(subStateMachine, stateMachines, states);
            }
        }
    }
    private static void ReadAnimatorStateMachine2(Dsl.FunctionData funcData, AnimatorStateMachine stateMachine, AnimatorControllerLayer layer, int layerIndex, AnimatorController ctrl)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            Dsl.FunctionData subFuncData = comp as Dsl.FunctionData;
            if (null != subFuncData) {
                var funcHeader = subFuncData;
                if (subFuncData.IsHighOrder)
                    funcHeader = subFuncData.LowerOrderFunction;
                string id = subFuncData.GetId();
                string name = funcHeader.GetParamId(0);
                if (id == "state") {
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    AnimatorState state = FindAnimatorState(stateMachine, name);
                    if (null == state) {
                        if (pos.sqrMagnitude < GameFramework.Geometry.c_FloatPrecision)
                            state = stateMachine.AddState(name);
                        else
                            state = stateMachine.AddState(name, pos);
                    }
                    ReadAnimatorState(subFuncData, state, stateMachine, layer, layerIndex, ctrl);
                } else if (id == "blendtreestate") {
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    BlendTree tree;
                    AnimatorState state = FindAnimatorState(stateMachine, name);
                    if (null == state) {
                        state = ctrl.CreateBlendTreeInController(name, out tree, layerIndex);
                        if (pos.sqrMagnitude >= GameFramework.Geometry.c_FloatPrecision) {
                            int len = stateMachine.states.Length - 1;
                            stateMachine.states[len].position = pos;
                        }
                    } else {
                        tree = state.motion as BlendTree;
                        if (null == tree) {
                            state = ctrl.CreateBlendTreeInController(name, out tree, layerIndex);
                            if (pos.sqrMagnitude >= GameFramework.Geometry.c_FloatPrecision) {
                                int len = stateMachine.states.Length - 1;
                                stateMachine.states[len].position = pos;
                            }
                        }
                    }
                    ReadAnimatorState(subFuncData, state, stateMachine, layer, layerIndex, ctrl);
                } else if (id == "statemachine") {
                    Vector3 pos = Vector3.zero;
                    if (funcHeader.GetParamNum() >= 2) {
                        Dsl.FunctionData posDsl = funcHeader.GetParam(1) as Dsl.FunctionData;
                        pos = ReadPosition(posDsl);
                    }
                    AnimatorStateMachine subStateMachine = FindAnimatorStateMachine(stateMachine, name);
                    if (null == subStateMachine) {
                        if (pos.sqrMagnitude < GameFramework.Geometry.c_FloatPrecision)
                            subStateMachine = stateMachine.AddStateMachine(name);
                        else
                            subStateMachine = stateMachine.AddStateMachine(name, pos);
                    }
                    ReadAnimatorStateMachine2(subFuncData, subStateMachine, layer, layerIndex, ctrl);
                } else if (id == "anytransition" && s_ImportAll) {
                    Dsl.FunctionData callData = funcHeader;
                    string destName = callData.GetParamId(0);
                    AnimatorStateTransition tran = null;
                    int paramNum = callData.GetParamNum();
                    if (paramNum >= 2) {
                        tran = FindAnimatorStateTransition2(stateMachine.anyStateTransitions, destName);
                        if (null == tran) {
                            AnimatorStateMachine outerStateMachine = FindAnimatorStateMachineRecurively(layer.stateMachine.stateMachines, destName);
                            if (null != outerStateMachine) {
                                tran = stateMachine.AddAnyStateTransition(outerStateMachine);
                            }
                        }
                    } else {
                        tran = FindAnimatorStateTransition(stateMachine.anyStateTransitions, destName);
                        if (null == tran) {
                            AnimatorState outerState = FindAnimatorStateRecurively(layer.stateMachine, destName);
                            if (null != outerState) {
                                tran = stateMachine.AddAnyStateTransition(outerState);
                            }
                        }
                    }
                    if (null != tran) {
                        ReadAnimatorStateTransition(subFuncData, tran);
                    }
                } else if (id == "entrytransition" && s_ImportAll) {
                    Dsl.FunctionData callData = funcHeader;
                    string destName = callData.GetParamId(0);
                    AnimatorTransition tran = null;
                    int paramNum = callData.GetParamNum();
                    if (paramNum >= 2) {
                        tran = FindAnimatorTransition2(stateMachine.entryTransitions, destName);
                        if (null == tran) {
                            AnimatorStateMachine outerStateMachine = FindAnimatorStateMachineRecurively(layer.stateMachine.stateMachines, destName);
                            if (null != outerStateMachine) {
                                tran = stateMachine.AddEntryTransition(outerStateMachine);
                            }
                        }
                    } else {
                        tran = FindAnimatorTransition(stateMachine.entryTransitions, destName);
                        if (null == tran) {
                            AnimatorState outerState = FindAnimatorStateRecurively(layer.stateMachine, destName);
                            if (null != outerState) {
                                tran = stateMachine.AddEntryTransition(outerState);
                            }
                        }
                    }
                    if (null != tran) {
                        ReadAnimatorTransition(subFuncData, tran);
                    }
                } else if (id == "transition") {
                    Dsl.FunctionData callData = funcHeader;
                    AnimatorTransition tran = null;
                    int paramNum = callData.GetParamNum();
                    if (paramNum >= 1) {
                        string destName = callData.GetParamId(0);
                        if (paramNum >= 2) {
                            tran = FindAnimatorTransition2(stateMachine.GetStateMachineTransitions(stateMachine), destName);
                            if (null == tran) {
                                AnimatorStateMachine outerStateMachine = FindAnimatorStateMachineRecurively(layer.stateMachine.stateMachines, destName);
                                if (null != outerStateMachine) {
                                    tran = stateMachine.AddStateMachineTransition(stateMachine, outerStateMachine);
                                }
                            }
                        } else {
                            tran = FindAnimatorTransition(stateMachine.GetStateMachineTransitions(stateMachine), destName);
                            if (null == tran) {
                                AnimatorState outerState = FindAnimatorStateRecurively(layer.stateMachine, destName);
                                if (null != outerState) {
                                    tran = stateMachine.AddStateMachineTransition(stateMachine, outerState);
                                }
                            }
                        }
                    } else if(s_ImportAll) {
                        tran = FindAnimatorExitTransition(stateMachine.GetStateMachineTransitions(stateMachine));
                        if (null == tran) {
                            tran = stateMachine.AddStateMachineExitTransition(stateMachine);
                        }
                    }
                    if (null != tran) {
                        ReadAnimatorTransition(subFuncData, tran);
                    }
                }
            }
        }
    }
    private static void ReadAnimatorStateTransition(Dsl.FunctionData funcData, AnimatorStateTransition tran)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                string id = callData.GetId();
                if (id == "hasexittime") {
                    bool val = bool.Parse(callData.GetParamId(0));
                    tran.hasExitTime = val;
                } else if (id == "condition") {
                    string param = callData.GetParamId(0);
                    string mode = callData.GetParamId(1);
                    float val = float.Parse(callData.GetParamId(2));
                    AnimatorConditionMode modeEnum = GameFramework.Converter.ConvertStrToEnum<AnimatorConditionMode>(mode);
                    if (!TryUpdateAnimatorCondition(tran.conditions, param, val, modeEnum)) {
                        tran.AddCondition(modeEnum, val, param);
                    }
                }
            }
        }
    }
    private static void ReadAnimatorTransition(Dsl.FunctionData funcData, AnimatorTransition tran)
    {
        foreach (Dsl.ISyntaxComponent comp in funcData.Params) {
            Dsl.FunctionData callData = comp as Dsl.FunctionData;
            if (null != callData) {
                if (callData.GetId() == "condition") {
                    string param = callData.GetParamId(0);
                    string mode = callData.GetParamId(1);
                    float val = float.Parse(callData.GetParamId(2));
                    AnimatorConditionMode modeEnum = GameFramework.Converter.ConvertStrToEnum<AnimatorConditionMode>(mode);
                    if (!TryUpdateAnimatorCondition(tran.conditions, param, val, modeEnum)) {
                        tran.AddCondition(modeEnum, val, param);
                    }
                }
            }
        }
    }
    private static Vector3 ReadPosition(Dsl.FunctionData posDsl)
    {
        if (posDsl.GetId() == "vector3") {
            float x = float.Parse(posDsl.GetParamId(0));
            float y = float.Parse(posDsl.GetParamId(1));
            float z = float.Parse(posDsl.GetParamId(2));
            return new Vector3(x, y, z);
        } else {
            return Vector3.zero;
        }
    }
    private static AnimatorControllerParameter FindAnimatorParameter(AnimatorController ctrl, string name)
    {
        foreach (AnimatorControllerParameter param in ctrl.parameters) {
            if (param.name == name) {
                return param;
            }
        }
        return null;
    }
    private static AnimatorControllerLayer FindAnimatorLayer(AnimatorController ctrl, string name, out int layerIndex)
    {
        layerIndex = 0;
        foreach (AnimatorControllerLayer layer in ctrl.layers) {
            if (layer.name == name) {
                return layer;
            }
            ++layerIndex;
        }
        return null;
    }
    private static AnimatorState FindAnimatorStateRecurively(AnimatorStateMachine topStateMachine, string name)
    {
        var ret = FindAnimatorState(topStateMachine, name);
        if (null != ret) {
            return ret;
        }
        ret = FindAnimatorStateRecurively(topStateMachine.stateMachines, name);
        return ret;
    }
    private static AnimatorState FindAnimatorStateRecurively(ChildAnimatorStateMachine[] stateMachines, string name)
    {
        foreach (var sm in stateMachines) {
            bool find = false;
            foreach (var state in sm.stateMachine.states) {
                if (state.state.name == name) {
                    return state.state;
                }
            }
            if(!find){
                var ret = FindAnimatorStateRecurively(sm.stateMachine.stateMachines, name);
                if (null != ret) {
                    return ret;
                }
            }
        }
        return null;
    }
    private static AnimatorStateMachine FindAnimatorStateMachineRecurively(ChildAnimatorStateMachine[] stateMachines, string name)
    {
        foreach (var sm in stateMachines) {
            if (sm.stateMachine.name == name) {
                return sm.stateMachine;
            } else {
                var ret = FindAnimatorStateMachineRecurively(sm.stateMachine.stateMachines, name);
                if (null != ret) {
                    return ret;
                }
            }
        }
        return null;
    }
    private static AnimatorState FindAnimatorState(AnimatorStateMachine stateMachine, string name)
    {
        foreach (ChildAnimatorState state in stateMachine.states) {
            if (state.state.name == name) {
                return state.state;
            }
        }
        return null;
    }
    private static AnimatorStateMachine FindAnimatorStateMachine(AnimatorStateMachine stateMachine, string name)
    {
        foreach (ChildAnimatorStateMachine sm in stateMachine.stateMachines) {
            if (sm.stateMachine.name == name) {
                return sm.stateMachine;
            }
        }
        return null;
    }
    private static AnimatorTransition FindAnimatorExitTransition(AnimatorTransition[] trans)
    {
        foreach (AnimatorTransition tran in trans) {
            if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit) {
                return tran;
            }
        }
        return null;
    }
    private static AnimatorTransition FindAnimatorTransition(AnimatorTransition[] trans, string name)
    {
        foreach (AnimatorTransition tran in trans) {
            if (tran.destinationState.name == name && null == tran.destinationStateMachine) {
                return tran;
            }
        }
        return null;
    }
    private static AnimatorTransition FindAnimatorTransition2(AnimatorTransition[] trans, string name)
    {
        foreach (AnimatorTransition tran in trans) {
            if (null != tran.destinationStateMachine && tran.destinationStateMachine.name == name) {
                return tran;
            }
        }
        return null;
    }
    private static AnimatorStateTransition FindAnimatorStateExitTransition(AnimatorStateTransition[] trans)
    {
        foreach (AnimatorStateTransition tran in trans) {
            if (null == tran.destinationState && null == tran.destinationStateMachine && tran.isExit) {
                return tran;
            }
        }
        return null;
    }
    private static AnimatorStateTransition FindAnimatorStateTransition(AnimatorStateTransition[] trans, string name)
    {
        foreach (AnimatorStateTransition tran in trans) {
            if (null != tran.destinationState && tran.destinationState.name == name) {
                return tran;
            }
        }
        return null;
    }
    private static AnimatorStateTransition FindAnimatorStateTransition2(AnimatorStateTransition[] trans, string name)
    {
        foreach (AnimatorStateTransition tran in trans) {
            if (null != tran.destinationStateMachine && tran.destinationStateMachine.name == name) {
                return tran;
            }
        }
        return null;
    }
    private static bool TryUpdateAnimatorCondition(AnimatorCondition[] conds, string name, float val, AnimatorConditionMode mode)
    {
        for (int i = 0; i < conds.Length;++i ) {
            if (conds[i].parameter == name) {
                conds[i].mode = mode;
                conds[i].threshold = val;
                return true;
            }
        }
        return false;
    }
    private static void WriteAnimatorStateMachine(StreamWriter sw, int indent, AnimatorStateMachine stateMachine, Vector3 pos, AnimatorControllerLayer layer)
    {
        foreach (var st in stateMachine.states) {
            WriteAnimatorState(sw, indent, st.state, st.position, stateMachine, layer);
        }
        sw.WriteLine();
        foreach (var sm in stateMachine.stateMachines) {
            sw.WriteLine("{0}statemachine(\"{1}\",vector3({2},{3},{4}))", GetIndent(indent), sm.stateMachine.name, pos.x, pos.y, pos.z);
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            WriteAnimatorStateMachine(sw, indent, sm.stateMachine, sm.position, layer);
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
        if (stateMachine.stateMachines.Length > 0)
            sw.WriteLine();
        var trans1 = stateMachine.anyStateTransitions;
        WriteAnimatorStateTransitions(sw, indent, trans1, "anytransition");
        if (trans1.Length > 0)
            sw.WriteLine();
        var trans2 = stateMachine.entryTransitions;
        WriteAnimatorTransitions(sw, indent, trans2, "entrytransition");
        if (trans2.Length > 0)
            sw.WriteLine();
        trans2 = stateMachine.GetStateMachineTransitions(stateMachine);
        WriteAnimatorTransitions(sw, indent, trans2, "transition");
        if (trans2.Length > 0)
            sw.WriteLine();
    }
    private static void WriteAnimatorState(StreamWriter sw, int indent, AnimatorState state, Vector3 pos, AnimatorStateMachine stateMachine, AnimatorControllerLayer layer)
    {
        BlendTree blendTree = state.motion as BlendTree;
        if (null != blendTree) {
            sw.WriteLine("{0}blendtreestate(\"{1}\",vector3({2},{3},{4}))", GetIndent(indent), state.name, pos.x, pos.y, pos.z);
        } else {
            sw.WriteLine("{0}state(\"{1}\",vector3({2},{3},{4}))", GetIndent(indent), state.name, pos.x, pos.y, pos.z);
        }
        sw.WriteLine("{0}{1}", GetIndent(indent), "{");
        ++indent;
        bool exist = false;
        if (!string.IsNullOrEmpty(state.tag)) {
            sw.WriteLine("{0}tag(\"{1}\");", GetIndent(indent), state.tag);
            exist = true;
        }
        if (layer.stateMachine.defaultState == state) {
            sw.WriteLine("{0}layerdefaultstate(True);", GetIndent(indent));
            exist = true;
        } else if (stateMachine.defaultState == state) {
            sw.WriteLine("{0}defaultstate(True);", GetIndent(indent));
            exist = true;
        }
        if (state.speed != 1.0f) {
            sw.WriteLine("{0}speed({1});", GetIndent(indent), state.speed);
            exist = true;
        }
        if (state.speedParameterActive) {
            sw.WriteLine("{0}speedparameter(\"{1}\");", GetIndent(indent), state.speedParameter);
            exist = true;
        }
        if (state.cycleOffset != 0.0f) {
            sw.WriteLine("{0}cycleoffset({1});", GetIndent(indent), state.cycleOffset);
            exist = true;
        }
        if (state.cycleOffsetParameterActive) {
            sw.WriteLine("{0}cycleoffsetparameter(\"{1}\");", GetIndent(indent), state.cycleOffsetParameter);
            exist = true;
        }
        if (state.mirror) {
            sw.WriteLine("{0}mirror({1});", GetIndent(indent), state.mirror);
            exist = true;
        }
        if (state.mirrorParameterActive) {
            sw.WriteLine("{0}mirrorparameter(\"{1}\");", GetIndent(indent), state.mirrorParameter);
            exist = true;
        }
        if (state.iKOnFeet) {
            sw.WriteLine("{0}footik({1});", GetIndent(indent), state.iKOnFeet);
            exist = true;
        }
        if (!state.writeDefaultValues) {
            sw.WriteLine("{0}writedefault({1});", GetIndent(indent), state.writeDefaultValues);
            exist = true;
        }
        if (exist)
            sw.WriteLine();
        if (null != blendTree) {
            WriteAnimatorBlendTree(sw, indent, blendTree);
            sw.WriteLine();
        }
        var trans = state.transitions;
        WriteAnimatorStateTransitions(sw, indent, trans, "transition");
        --indent;
        sw.WriteLine("{0}{1}", GetIndent(indent), "};");
    }
    private static void WriteAnimatorBlendTree(StreamWriter sw, int indent, BlendTree tree)
    {
        sw.WriteLine("{0}blendtree", GetIndent(indent));
        sw.WriteLine("{0}{1}", GetIndent(indent), "{");
        ++indent;
        sw.WriteLine("{0}type(\"{1}\");", GetIndent(indent), tree.blendType);
        if (tree.blendType != BlendTreeType.Direct) {
            sw.WriteLine("{0}parameter(\"{1}\");", GetIndent(indent), tree.blendParameter);
        }
        if (tree.blendType != BlendTreeType.Simple1D && tree.blendType != BlendTreeType.Direct) {
            sw.WriteLine("{0}parametery(\"{1}\");", GetIndent(indent), tree.blendParameterY);
        }
        if (tree.useAutomaticThresholds) {
            sw.WriteLine("{0}automatethreshold({1});", GetIndent(indent), tree.useAutomaticThresholds);
        }
        sw.WriteLine();
        foreach (var motion in tree.children) {
            BlendTree blendTree = motion.motion as BlendTree;
            if (null != blendTree) {
                sw.WriteLine("{0}blendtreemotion", GetIndent(indent));
            } else {
                sw.WriteLine("{0}motion", GetIndent(indent));
            }
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            if (motion.mirror) {
                sw.WriteLine("{0}mirror({1});", GetIndent(indent), motion.mirror);
            }
            if (motion.timeScale != 1.0f) {
                sw.WriteLine("{0}timescale({1});", GetIndent(indent), motion.timeScale);
            }
            if (tree.blendType == BlendTreeType.Simple1D) {
                sw.WriteLine("{0}threshold({1});", GetIndent(indent), motion.threshold);
            } else if (tree.blendType == BlendTreeType.Direct) {
                sw.WriteLine("{0}parameter(\"{1}\");", GetIndent(indent), motion.directBlendParameter);
                if (motion.cycleOffset > 0.0f) {
                    sw.WriteLine("{0}cycleoffset({1});", GetIndent(indent), motion.cycleOffset);
                }
            } else {
                sw.WriteLine("{0}position({1},{2});", GetIndent(indent), motion.position.x, motion.position.y);
            }

            if (null != blendTree) {
                sw.WriteLine();
                WriteAnimatorBlendTree(sw, indent, blendTree);
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
        --indent;
        sw.WriteLine("{0}{1}", GetIndent(indent), "};");
    }
    private static void WriteAnimatorStateTransitions(StreamWriter sw, int indent, AnimatorStateTransition[] trans, string type)
    {
        foreach (var tran in trans) {
            if (null != tran.destinationState) {
                sw.WriteLine("{0}{1}(\"{2}\")", GetIndent(indent), type, tran.destinationState.name);
            } else if (null != tran.destinationStateMachine) {
                sw.WriteLine("{0}{1}(\"{2}\",\"statemachine\")", GetIndent(indent), type, tran.destinationStateMachine.name);
            } else {
                sw.WriteLine("{0}{1}()", GetIndent(indent), type);
            }
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            if (tran.hasExitTime) {
                sw.WriteLine("{0}hasexittime({1});", GetIndent(indent), tran.hasExitTime);
            }
            foreach (var cond in tran.conditions) {
                sw.WriteLine("{0}condition(\"{1}\",\"{2}\",{3});", GetIndent(indent), cond.parameter, cond.mode, cond.threshold);
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
    }
    private static void WriteAnimatorTransitions(StreamWriter sw, int indent, AnimatorTransition[] trans, string type)
    {
        foreach (var tran in trans) {
            if (null != tran.destinationState) {
                sw.WriteLine("{0}{1}(\"{2}\")", GetIndent(indent), type, tran.destinationState.name);
            } else if (null != tran.destinationStateMachine) {
                sw.WriteLine("{0}{1}(\"{2}\",\"statemachine\")", GetIndent(indent), type, tran.destinationStateMachine.name);
            } else {
                sw.WriteLine("{0}{1}()", GetIndent(indent), type);
            }
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            foreach (var cond in tran.conditions) {
                sw.WriteLine("{0}condition(\"{1}\",\"{2}\",{3});", GetIndent(indent), cond.parameter, cond.mode, cond.threshold);
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
    }

    private static string GetIndent(int indent)
    {
        return c_IndentString.Substring(0, indent);
    }
    private static string IndentScript(string indent, string scp)
    {
        string[] lines = scp.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int ix = 0; ix < lines.Length; ++ix) {
            lines[ix] = string.Format("{0}{1}", indent, lines[ix]);
        }
        return string.Join("\r\n", lines);
    }

    private static bool s_ImportAll = false;
    private const string c_IndentString = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
}
