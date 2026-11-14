using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
//using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using Unity.Profiling;
using StoryScript;

public class EditorGmCommandWindow : EditorWindow
{
    public static void RunOnEditorStartup()
    {
        const string path = "./tools/dsl_scripts/editor_startup.dsl";
        if (File.Exists(path)) {
            EditorDslProcessorUtility.ResetCommandCalculator();
            var calculator = EditorDslProcessorUtility.GetCommandCalculator();
            string scriptName = string.Empty;
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                bool haveError = false;
                foreach (var syntaxComponent in file.DslInfos) {
                    bool check = false;
                    var func = syntaxComponent as Dsl.FunctionData;
                    var info = syntaxComponent as Dsl.StatementData;
                    Dsl.FunctionData func2 = null;
                    if (null == func && null != info) {
                        func = info.First.AsFunction;
                        func2 = info.Second.AsFunction;
                    }
                    int num = null != info ? info.GetFunctionNum() : 1;
                    if (num == 1) {
                        var id = func.GetId();
                        if (id == "script") {
                            check = true;
                            scriptName = func.LowerOrderFunction.GetParamId(0);
                            calculator.LoadDsl(func);
                        }
                    }
                    else if (num == 2) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        if (firstId == "script" && secondId == "args") {
                            check = true;
                            scriptName = func.GetParamId(0);
                            calculator.LoadDsl(info);
                        }
                    }
                    if (!check) {
                        EditorUtility.DisplayDialog("错误", string.Format("error script:{0}, must be script(name){{...}}; or script(name)args(...){{...}};", info.GetLine()), "ok");
                        haveError = true;
                    }
                }
                if (!haveError) {
                    var r = calculator.Calc("main");
                    Debug.LogFormat("editor startup script result:{0}", r.ToString());
                }
            }
        }
    }
    internal static void InitWindow(EditorDslEditWindow resEdit, string content, BoxedValue obj, BoxedValue item)
    {
        EditorGmCommandWindow window = (EditorGmCommandWindow)EditorWindow.GetWindow(typeof(EditorGmCommandWindow));
        window.Init(resEdit, content, obj, item);
        window.Show();
    }

    private void Init(EditorDslEditWindow resEdit, string content, BoxedValue obj, BoxedValue item)
    {
        m_ResourceEditWindow = resEdit;
        m_Content = content;
        m_Object = obj;
        m_Item = item;
        m_IsReady = true;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea(m_Content, EditorStyles.textArea, GUILayout.MinHeight(160), GUILayout.MaxHeight(this.position.height - 60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("刷新列表", EditorStyles.toolbarButton, GUILayout.Width(60))) {
            m_Menus = null;
            m_Files = null;
            m_SelectedIndex = 0;
            DeferAction(obj => { EditorDslProcessor.Instance.ClearDsl(); });
        }
        if (null == m_Menus || m_Menus.Length <= 0) {
            SortedDictionary<string, string> dslFiles = new SortedDictionary<string, string>();
            dslFiles.Add("0.Empty", string.Empty);
            var files = Directory.GetFiles("./tools/dsl_scripts/CommandDsl", "*.dsl", SearchOption.AllDirectories);
            foreach (var file in files) {
                string name, desc;
                if (ReadMenuAndDescription(file, out name, out desc)) {
                    try {
                        dslFiles.Add(string.Format("{0}", name), file);
                    }
                    catch (Exception ex) {
                        Debug.LogFormat("Add 'Find' menu {0} desc {1} exception:{2}", name, desc, ex.Message);
                    }
                }
            }
            m_Menus = dslFiles.Keys.ToArray();
            m_Files = dslFiles.Values.ToArray();
        }
        int newIndex = EditorGUILayout.Popup(m_SelectedIndex, m_Menus, EditorStyles.toolbarPopup, GUILayout.Width(320));
        if (newIndex != m_SelectedIndex) {
            m_SelectedIndex = newIndex;
            string file = m_Files[m_SelectedIndex];
            if (!string.IsNullOrEmpty(file)) {
                DeferAction(obj => { SelectDsl(file); });
            }
            else {
                DeferAction(obj => { ClearDsl(); });
            }
        }
        EditorGUILayout.EndHorizontal();

        var rt = EditorGUILayout.BeginHorizontal();
        m_Command = EditorGUILayout.TextField(m_Command, EditorStyles.toolbarTextField, GUILayout.MinWidth(200), GUILayout.MaxWidth(this.position.width - 52));
        if (GUILayout.Button("run", EditorStyles.toolbarButton, GUILayout.Width(40))) {
            DeferAction(w => Run());
        }
        EditorGUILayout.EndHorizontal();
        if (m_IsReady) {
            m_Pos = EditorGUILayout.BeginScrollView(m_Pos);
            while (m_Results.Count > 20) {
                m_Results.Dequeue();
            }
            foreach (var info in m_Results) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextArea(info, EditorStyles.textArea, GUILayout.MinWidth(200), GUILayout.MaxWidth(this.position.width), GUILayout.MinHeight(16), GUILayout.MaxHeight(32));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        ExecuteDeferredActions();
    }
    private void Run()
    {
        m_ScriptableInfo.Content = m_Content;
        m_ScriptableInfo.Items = EditorDslProcessor.Instance.ItemList;
        m_ScriptableInfo.Groups = EditorDslProcessor.Instance.GroupList;
        m_ScriptableInfo.Results = m_Results;
        m_ScriptableInfo.EditorDslEditWindow = m_ResourceEditWindow;
        m_ScriptableInfo.EditorDslProcessor = EditorDslProcessor.Instance;
        m_ScriptableInfo.ResourceEditWindowType = typeof(EditorDslEditWindow);
        m_ScriptableInfo.ResourceProcessorType = typeof(EditorDslProcessor);
        m_ScriptableInfo.ResourceEditUtilityType = typeof(EditorDslProcessorUtility);

        if (string.IsNullOrEmpty(m_Command)) {
            var r = EditorDslProcessorUtility.RunCommandScript(m_ScriptName, m_Object, m_Item, EditorDslProcessor.Instance.Params, new Dictionary<string, BoxedValue> { { "@context", BoxedValue.FromObject(m_ScriptableInfo) } });
            if (!r.IsNullObject) {
                m_Results.Enqueue(string.Format("script:{0} result:{1}", m_ScriptName, r.ToString()));
            }
            else {
                m_Results.Enqueue(string.Format("script:{0} result:null", m_ScriptName));
            }
        }
        else {
            if (EditorDslProcessorUtility.LoadCommand(m_Command, EditorDslProcessor.Instance.Params, new Dictionary<string, BoxedValue> { { "@context", BoxedValue.FromObject(m_ScriptableInfo) } })) {
                var r = EditorDslProcessorUtility.EvalCommand(m_Object, m_Item);
                if (!r.IsNullObject) {
                    m_Results.Enqueue(string.Format("cmd:{0} result:{1}", m_Command, r.ToString()));
                }
                else {
                    m_Results.Enqueue(string.Format("cmd:{0} result:null", m_Command));
                }
            }
        }
        m_Content = m_ScriptableInfo.Content;
    }

    private void ExecuteDeferredActions()
    {
        if (m_InActions)
            return;
        try {
            m_InActions = true;
            while (m_Actions.Count > 0) {
                var action = m_Actions.Dequeue();
                if (null != action) {
                    action(this);
                }
            }
        }
        finally {
            m_InActions = false;
        }
    }
    private void DeferAction(Action<EditorGmCommandWindow> action)
    {
        m_Actions.Enqueue(action);
    }

    private void ClearDsl()
    {
        m_Command = string.Empty;
        m_ScriptName = string.Empty;
        EditorDslProcessorUtility.ResetCommandCalculator();
    }
    internal void SelectDsl(string path)
    {
        ClearDsl();
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            var calculator = EditorDslProcessorUtility.GetCommandCalculator();
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                bool haveError = false;
                foreach (var syntaxComponent in file.DslInfos) {
                    bool check = false;
                    var func = syntaxComponent as Dsl.FunctionData;
                    var info = syntaxComponent as Dsl.StatementData;
                    Dsl.FunctionData func2 = null;
                    if (null == func && null != info) {
                        func = info.First.AsFunction;
                        func2 = info.Second.AsFunction;
                    }
                    int num = null != info ? info.GetFunctionNum() : 1;
                    if (num == 1) {
                        var id = func.GetId();
                        if (id == "script") {
                            check = true;
                            m_ScriptName = func.LowerOrderFunction.GetParamId(0);
                            calculator.LoadDsl(func);
                        }
                    }
                    else if (num == 2) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        if (firstId == "script" && secondId == "args") {
                            check = true;
                            m_ScriptName = func.GetParamId(0);
                            calculator.LoadDsl(info);
                        }
                        else if (firstId == "features" && secondId == "script") {
                            check = true;
                            Debug.Assert(null != func2);
                            m_ScriptName = func2.LowerOrderFunction.GetParamId(0);
                            calculator.LoadDsl(info.Second);
                        }
                    }
                    else if (num == 3) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        string thirdId = info.Last.GetId();
                        if (firstId == "features" && secondId == "script" && thirdId == "args") {
                            check = true;
                            Debug.Assert(null != func2);
                            m_ScriptName = func2.GetParamId(0);
                            info.Functions.RemoveAt(0);
                            calculator.LoadDsl(info);
                        }
                    }
                    if (!check) {
                        EditorUtility.DisplayDialog("错误", string.Format("error script:{0}, must be features{{...}};script(name){{...}}; or features{{...}};script(name)args(...){{...}}; or features{{...}}script(name){{...}}; or features{{...}}script(name)args(...){{...}};", info.GetLine()), "ok");
                        haveError = true;
                    }
                }
                if (!haveError) {
                    m_Content = File.ReadAllText(path);
                }
                else {
                    m_Content = string.Empty;
                    m_ScriptName = string.Empty;
                }
            }
        }
    }

    internal class ScriptableInfo
    {
        internal string Content;
        internal List<EditorDslProcessorUtility.ItemInfo> Items;
        internal List<EditorDslProcessorUtility.GroupInfo> Groups;
        internal Queue<string> Results;
        internal EditorDslEditWindow EditorDslEditWindow;
        internal EditorDslProcessor EditorDslProcessor;
        internal Type ResourceEditWindowType;
        internal Type ResourceProcessorType;
        internal Type ResourceEditUtilityType;
    }
    private ScriptableInfo m_ScriptableInfo = new ScriptableInfo();

    private bool m_IsReady = false;
    private bool m_InActions = false;
    private Queue<Action<EditorGmCommandWindow>> m_Actions = new Queue<Action<EditorGmCommandWindow>>();

    private string[] m_Menus = null;
    private string[] m_Files = null;
    private int m_SelectedIndex = 0;
    private string m_ScriptName = string.Empty;

    private string m_Content = string.Empty;
    private Queue<string> m_Results = new Queue<string>();
    private string m_Command = "@context.Content=check_pipeline_asset_guid()+'\\n'+check_pipeline_assets();";
    private BoxedValue m_Object = BoxedValue.NullObject;
    private BoxedValue m_Item = BoxedValue.NullObject;
    private EditorDslEditWindow m_ResourceEditWindow = null;
    private Vector2 m_Pos = Vector2.zero;

    private static bool ReadMenuAndDescription(string path, out string menu, out string desc)
    {
        bool ret = false;
        bool readMenu = false;
        bool readDesc = false;
        menu = string.Empty;
        desc = string.Empty;
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                foreach (var info in file.DslInfos) {
                    var func = info as Dsl.FunctionData;
                    var stData = info as Dsl.StatementData;
                    if (null == func && null != stData) {
                        func = stData.First.AsFunction;
                    }
                    if (null != func && func.GetId() == "features") {
                        foreach (var comp in func.Params) {
                            var callData = comp as Dsl.FunctionData;
                            if (null != callData && callData.GetId() == "feature") {
                                string key = callData.GetParamId(0);
                                string val = callData.GetParamId(1);
                                if (key == "menu") {
                                    menu = val;
                                    readMenu = true;
                                }
                                else if (key == "description") {
                                    desc = val;
                                    readDesc = true;
                                }
                                if (readMenu && readDesc) {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (file.DslInfos.Count == 0) {
                    Debug.LogErrorFormat("'{0}' no any DSL info !", path);
                }
            }
            if (string.IsNullOrEmpty(menu)) {
                menu = string.Format("{0}", Path.GetFileNameWithoutExtension(path));
            }
            if (string.IsNullOrEmpty(desc)) {
                desc = path;
            }
            ret = true;
        }
        return ret;
    }
}
