using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using GameFramework;

public sealed class ResourceEditWindow : EditorWindow
{
    [MenuItem("工具/资源处理")]
    internal static void InitWindow()
    {
        ResourceEditWindow window = (ResourceEditWindow)EditorWindow.GetWindow(typeof(ResourceEditWindow));
        window.Init();
        window.Show();
    }

    private void Init()
    {
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("选择处理脚本")) {
            SelectDsl();
        }
        if (GUILayout.Button("收集资源")) {
            Collect();
        }
        if (GUILayout.Button("处理选中资源")) {
            Process();
        }
        if (GUILayout.Button("生成后处理资源代码")) {
            Generate();
        }
        EditorGUILayout.EndHorizontal();

        if (m_Params.Count > 0) {
            foreach (var pair in m_Params) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(pair.Key, GUILayout.Width(60));
                string oldVal = pair.Value.ToString();
                string newVal = GUILayout.TextField(oldVal, 1024);
                EditorGUILayout.EndHorizontal();
                if (newVal != oldVal) {
                    m_EditedParams.Add(pair.Key, newVal);
                }
            }
            if (m_EditedParams.Count > 0) {
                foreach (var pair in m_EditedParams) {
                    object val;
                    if (m_Params.TryGetValue(pair.Key, out val)) {
                        if (val is int) {
                            m_Params[pair.Key] = int.Parse(pair.Value);
                        } else if (val is float) {
                            m_Params[pair.Key] = float.Parse(pair.Value);
                        } else if (val is string) {
                            m_Params[pair.Key] = pair.Value;
                        }
                    }
                }
                m_EditedParams.Clear();
            }
        }
        if (m_ResourceList.Count <= 0) {
            if (!string.IsNullOrEmpty(m_Text)) {
                m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true);
                EditorGUILayout.TextArea(m_Text);
                EditorGUILayout.EndScrollView();
            }
        } else {
            if (!string.IsNullOrEmpty(m_Text)) {
                EditorGUILayout.TextArea(m_Text, GUILayout.MaxHeight(128));
            }
            m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true);
            ListItem();
            EditorGUILayout.EndScrollView();
        }
    }

    private void SelectDsl()
    {
        string path = EditorUtility.OpenFilePanel("请选择要执行的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                m_DslFile = file;
                m_NextFilterIndex = 0;
                m_NextProcessIndex = 0;
                m_FilterCalculator = new Expression.DslCalculator();
                m_ProcessCalculator = new Expression.DslCalculator();
                ResourceEditUtility.InitCalculator(m_FilterCalculator);
                ResourceEditUtility.InitCalculator(m_ProcessCalculator);

                bool haveError = false;
                foreach (var info in file.DslInfos) {
                    bool check = false;
                    int num = info.GetFunctionNum();
                    if (num == 2) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        if (firstId == "input" && (secondId == "filter" || secondId == "process")) {
                            check = true;

                            if (secondId == "filter") {
                                m_FilterCalculator.Load(m_NextFilterIndex.ToString(), info.Second);
                                ++m_NextFilterIndex;
                            } else {
                                m_ProcessCalculator.Load(m_NextProcessIndex.ToString(), info.Second);
                                ++m_NextProcessIndex;
                            }
                        }
                    } else if (num == 3) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        string thirdId = info.Last.GetId();
                        if (firstId == "input" && secondId == "filter" && thirdId == "process") {
                            check = true;

                            m_FilterCalculator.Load(m_NextFilterIndex.ToString(), info.Second);
                            ++m_NextFilterIndex;
                            m_ProcessCalculator.Load(m_NextProcessIndex.ToString(), info.Last);
                            ++m_NextProcessIndex;
                        }
                    }
                    if (!check) {
                        EditorUtility.DisplayDialog("错误", string.Format("error script:{0}, must be input(exts){{...}}filter{{...}}process{{...}};", info.GetLine()), "ok");
                        haveError = true;
                    }
                }
                if (!haveError) {
                    m_IsSearchInScene = false;
                    m_PostProcessClass = string.Empty;
                    m_PostProcessMethod = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_Params.Clear();

                    foreach (var info in m_DslFile.DslInfos) {
                        var first = info.First;
                        var input = first.Call;
                        foreach (var param in input.Params) {
                            m_TypeOrExtList.Add(param.GetId());
                        }
                        foreach (var comp in first.Statements) {
                            var callData = comp as Dsl.CallData;
                            string id = callData.GetId();
                            string key = callData.GetParamId(0);
                            string val = callData.GetParamId(1);
                            if (id == "int") {
                                m_Params[key] = int.Parse(val);
                            } else if (id == "float") {
                                m_Params[key] = float.Parse(val);
                            } else if (id == "string") {
                                m_Params[key] = val;
                            } else if (id == "option") {
                                if(key=="source"){
                                    m_IsSearchInScene = val == "scene";
                                } else if (key == "postprocessclass") {
                                    m_PostProcessClass = val;
                                } else if (key == "postprocessmethod") {
                                    m_PostProcessMethod = val;
                                }
                            }
                        }
                    }
                    m_Text = File.ReadAllText(path);
                } else {
                    m_DslFile = null;
                    m_IsSearchInScene = false;
                    m_PostProcessClass = string.Empty;
                    m_PostProcessMethod = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_Params.Clear();
                    m_Text = null;
                    m_NextFilterIndex = 0;
                    m_NextProcessIndex = 0;
                    m_FilterCalculator = null;
                    m_ProcessCalculator = null;
                }
            } else {
                m_DslFile = null;
                m_IsSearchInScene = false;
                m_PostProcessClass = string.Empty;
                m_PostProcessMethod = string.Empty;
                m_TypeOrExtList.Clear();
                m_TypeList.Clear();
                m_Params.Clear();
                m_Text = null;
                m_NextFilterIndex = 0;
                m_NextProcessIndex = 0;
                m_FilterCalculator = null;
                m_ProcessCalculator = null;
            }
            m_ResourceList.Clear();
        }
    }

    private void Collect()
    {
        if (m_IsSearchInScene) {
            m_ResourceList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountSceneObjectRecursively();
            if (m_TotalSearchCount > 0) {
                SearchSceneObjectRecursively();
                EditorUtility.ClearProgressBar();
            }
        } else {
            string path = EditorUtility.OpenFolderPanel("请选择要收集资源的根目录", Application.dataPath, string.Empty);
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
                m_ResourceList.Clear();
                m_CurSearchCount = 0;
                m_TotalSearchCount = 0;
                CountFileRecursively(path);
                if (m_TotalSearchCount > 0) {
                    SearchFileRecursively(path);
                    EditorUtility.ClearProgressBar();
                }
            }
        }
    }

    private void Process()
    {
        if (null == m_DslFile) {
            EditorUtility.DisplayDialog("错误", "请先选择dsl !", "ok");
            return;
        }
        int totalSelectedCount = 0;
        int index = 0;
        foreach (var item in m_ResourceList) {
            if (item.Selected) {
                ++totalSelectedCount;
            }
        }
        foreach (var item in m_ResourceList) {
            if (item.Selected) {
                ResourceEditUtility.Process(item.Path, item.Importer, item.Object, m_ProcessCalculator, m_NextProcessIndex, m_Params);
                ++index;
                EditorUtility.DisplayProgressBar("处理进度", string.Format("{0}/{1}", index, totalSelectedCount), index * 1.0f / totalSelectedCount);
            }
        }
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("提示", "处理完成", "ok");
    }
    
    private void Generate()
    {
        if (string.IsNullOrEmpty(m_PostProcessClass) || string.IsNullOrEmpty(m_PostProcessMethod)) {
            EditorUtility.DisplayDialog("错误", "当前dsl没有配置后处理类与方法名 !", "ok");
            return;
        }
        string path = AssetPathToPath("Assets/Scripts/Editor/PostProcess/");
        string file = Path.Combine(path, string.Format("{0}_{1}.cs", m_PostProcessClass, m_PostProcessMethod));
        using (StreamWriter sw = new StreamWriter(file)) {
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using UnityEditor;");
            sw.WriteLine("using System.Collections;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine();
            sw.WriteLine("public static partial class {0}", m_PostProcessClass);
            sw.WriteLine("{");
            if (m_PostProcessMethod.Contains("Texture")) {
                sw.WriteLine("\tstatic partial void {0}(HashSet<string> list, ref int maxSize)", m_PostProcessMethod);
                sw.WriteLine("\t{");
                object v;
                if (m_Params.TryGetValue("maxSize", out v) && v is int) {
                    sw.WriteLine("\t\tmaxSize = {0};", (int)v);
                }
                foreach (var item in m_ResourceList) {
                    if (item.Selected && !string.IsNullOrEmpty(item.Path)) {
                        sw.WriteLine("\t\tlist.Add(\"{0}\");", item.Path);
                    }
                }
                sw.WriteLine("\t}");
            } else {
                sw.WriteLine("\tstatic partial void {0}(HashSet<string> list)", m_PostProcessMethod);
                sw.WriteLine("\t{");
                foreach (var item in m_ResourceList) {
                    if (item.Selected && !string.IsNullOrEmpty(item.Path)) {
                        sw.WriteLine("\t\tlist.Add(\"{0}\");", item.Path);
                    }
                }
                sw.WriteLine("\t}");
            }
            sw.WriteLine("}");
            sw.Close();
        }
    }

    private void ListItem()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (ItemInfo item in m_ResourceList) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (ItemInfo item in m_ResourceList) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Go to page ({0}) : ", m_ResourceList.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = GUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        GUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_ResourceList.Count / c_ItemsPerPage + 1, m_Page));
        GUILayout.EndVertical();
        int index = 0;
        int totalShown = 0;
        foreach (ItemInfo item in m_ResourceList) {
            ++index;
            if (index <= (m_Page - 1) * c_ItemsPerPage)
                continue;
            ++totalShown;
            if (totalShown > c_ItemsPerPage)
                break;
            EditorGUILayout.BeginHorizontal();
            item.Selected = GUILayout.Toggle(item.Selected, index + ".", GUILayout.Width(40));
            if (m_IsSearchInScene) {
                var oldAlignment = GUI.skin.button.alignment;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (GUILayout.Button(new GUIContent(item.Path))) {
                    Selection.activeObject = item.Object;
                }
                GUI.skin.button.alignment = oldAlignment;
            } else {
                Texture icon = AssetDatabase.GetCachedIcon(item.Path);
                var oldAlignment = GUI.skin.button.alignment;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (GUILayout.Button(new GUIContent(item.Path, icon, item.Path))) {
                    if (null != item.Object)
                        Selection.activeObject = item.Object;
                    else
                        SelectObject(item.Path);
                }
                GUI.skin.button.alignment = oldAlignment;
            }
            GUILayout.Label(item.Info);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void SearchFileRecursively(string dir)
    {
        foreach (string ext in m_TypeOrExtList) {
            string[] files = Directory.GetFiles(dir, ext);
            foreach (string file in files) {
                ++m_CurSearchCount;
                string assetPath = PathToAssetPath(file);
                var importer = AssetImporter.GetAtPath(assetPath);
                UnityEngine.Object obj = null;
                var ret = ResourceEditUtility.Process(assetPath, importer, obj, m_FilterCalculator, m_NextFilterIndex, m_Params);
                if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                    string info = string.Empty;
                    if (null != m_FilterCalculator) {
                        object v;
                        if (m_FilterCalculator.NamedVariables.TryGetValue("object", out v)) {
                            if (null != v) {
                                obj = v as UnityEngine.Object;
                            }
                        }
                        if (m_FilterCalculator.NamedVariables.TryGetValue("info", out v)) {
                            info = v as string;
                            if (null == info)
                                info = string.Empty;
                        }
                    }
                    m_ResourceList.Add(new ItemInfo { Path = assetPath, Importer = importer, Object = obj, Info = info, Selected = false });
                }
                EditorUtility.DisplayProgressBar("采集进度", string.Format("{0} in {1}/{2}", m_ResourceList.Count, m_CurSearchCount, m_TotalSearchCount), m_CurSearchCount * 1.0f / m_TotalSearchCount);
            }
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string subDir in dirs) {
                SearchFileRecursively(subDir);
            }
        }
    }

    private void CountFileRecursively(string dir)
    {
        foreach (string ext in m_TypeOrExtList) {
            string[] files = Directory.GetFiles(dir, ext);
            m_TotalSearchCount += files.Length;

            string[] dirs = Directory.GetDirectories(dir);
            foreach (string subDir in dirs) {
                CountFileRecursively(subDir);
            }
        }
    }

    private void SearchSceneObjectRecursively()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                SearchChildObjectRecursively(string.Empty, obj);
            }
        }
    }

    private void SearchChildObjectRecursively(string path, GameObject obj)
    {
        if (!IsMatchedObject(obj))
            return;
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        } else {
            path = path + "/" + obj.name;
        }
        ++m_CurSearchCount;
        AssetImporter importer = null;
        var ret = ResourceEditUtility.Process(path, importer, obj, m_FilterCalculator, m_NextFilterIndex, m_Params);
        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
            string info = string.Empty;
            if (null != m_FilterCalculator) {
                object v;
                if (m_FilterCalculator.NamedVariables.TryGetValue("importer", out v)) {
                    if (null != v) {
                        importer = v as AssetImporter;
                    }
                }
                if (m_FilterCalculator.NamedVariables.TryGetValue("info", out v)) {
                    info = v as string;
                    if (null == info)
                        info = string.Empty;
                }
            }
            m_ResourceList.Add(new ItemInfo { Path = path, Importer = importer, Object = obj, Info = info, Selected = false });
        }
        EditorUtility.DisplayProgressBar("采集进度", string.Format("{0} in {1}/{2}", m_ResourceList.Count, m_CurSearchCount, m_TotalSearchCount), m_CurSearchCount * 1.0f / m_TotalSearchCount);

        var trans = obj.transform;
        int ct = trans.childCount;
        for (int i = 0; i < ct; ++i) {
            var t = trans.GetChild(i);
            SearchChildObjectRecursively(path, t.gameObject);
        }
    }

    private bool IsMatchedObject(GameObject obj)
    {
        if (m_TypeList.Count <= 0 && m_TypeOrExtList.Count > 0) {
            foreach (string type in m_TypeOrExtList) {
                Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                if (null == t) {
                    t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                }
                if (null == t) {
                    t = Type.GetType(type + ", Assembly-CSharp");
                }
                if (null != t) {
                    m_TypeList.Add(t);
                }
            }
        }
        foreach (Type type in m_TypeList) {
            var com = obj.GetComponent(type);
            if (null != com)
                return true;
        }
        return false;
    }

    private void CountSceneObjectRecursively()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            m_TotalSearchCount += objs.Length;

            foreach (var obj in objs) {
                CountChildObjectRecursively(obj);
            }
        }
    }

    private void CountChildObjectRecursively(GameObject obj)
    {
        var trans = obj.transform;
        int ct = trans.childCount;
        m_TotalSearchCount += ct;
        for (int i = 0; i < ct; ++i) {
            var t = trans.GetChild(i);
            CountChildObjectRecursively(t.gameObject);
        }
    }

    private string PathToAssetPath(string path)
    {
        string rootPath = Application.dataPath.Replace('\\', '/');
        path = path.Replace('\\', '/');
        if (path.StartsWith(rootPath)) {
            return "Assets" + path.Substring(rootPath.Length);
        } else {
            return path;
        }
    }

    private string AssetPathToPath(string assetPath)
    {
        string rootPath = Application.dataPath.Replace('\\', '/');
        return Path.Combine(rootPath, assetPath.Substring("Assets/".Length));
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
        internal AssetImporter Importer;
        internal UnityEngine.Object Object;
        internal string Info;
        internal bool Selected;
    }

    private bool m_IsSearchInScene = false;
    private string m_PostProcessClass = string.Empty;
    private string m_PostProcessMethod = string.Empty;
    private List<string> m_TypeOrExtList = new List<string>();
    private List<Type> m_TypeList = new List<Type>();
    private Dictionary<string, object> m_Params = new Dictionary<string, object>();
    private List<ItemInfo> m_ResourceList = new List<ItemInfo>();
    private Dsl.DslFile m_DslFile = null;

    private Expression.DslCalculator m_FilterCalculator = null;
    private Expression.DslCalculator m_ProcessCalculator = null;
    private int m_NextFilterIndex = 0;
    private int m_NextProcessIndex = 0;

    private Vector2 m_PanelPos = Vector2.zero;
    private string m_Text = string.Empty;
    private Dictionary<string, string> m_EditedParams = new Dictionary<string, string>();
    private int m_Page = 1;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;

    private const int c_ItemsPerPage = 50;
}

internal static class ResourceEditUtility
{
    internal static void InitCalculator(Expression.DslCalculator calc)
    {
        calc.Init();
        calc.Register("saveandreimport", new Expression.ExpressionFactoryHelper<SaveAndReimportExp>());
        calc.Register("getdefaulttexturesetting", new Expression.ExpressionFactoryHelper<GetDefaultTextureSettingExp>());
        calc.Register("gettexturesetting", new Expression.ExpressionFactoryHelper<GetTextureSettingExp>());
        calc.Register("settexturesetting", new Expression.ExpressionFactoryHelper<SetTextureSettingExp>());
        calc.Register("gettexturecompression", new Expression.ExpressionFactoryHelper<GetTextureCompressionExp>());
        calc.Register("settexturecompression", new Expression.ExpressionFactoryHelper<SetTextureCompressionExp>());
        calc.Register("getmeshcompression", new Expression.ExpressionFactoryHelper<GetMeshCompressionExp>());
        calc.Register("setmeshcompression", new Expression.ExpressionFactoryHelper<SetMeshCompressionExp>());
        calc.Register("closemeshanimationifnoanimation", new Expression.ExpressionFactoryHelper<CloseMeshAnimationIfNoAnimationExp>());
        calc.Register("getanimationcompression", new Expression.ExpressionFactoryHelper<GetAnimationCompressionExp>());
        calc.Register("setanimationcompression", new Expression.ExpressionFactoryHelper<SetAnimationCompressionExp>());
        calc.Register("getanimationtype", new Expression.ExpressionFactoryHelper<GetAnimationTypeExp>());
        calc.Register("setanimationtype", new Expression.ExpressionFactoryHelper<SetAnimationTypeExp>());
        calc.Register("clearanimationscalecurve", new Expression.ExpressionFactoryHelper<ClearAnimationScaleCurveExp>());
        calc.Register("getaudiosetting", new Expression.ExpressionFactoryHelper<GetAudioSettingExp>());
        calc.Register("setaudiosetting", new Expression.ExpressionFactoryHelper<SetAudioSettingExp>());
    }
    internal static object Process(string path, object importer, object obj, Expression.DslCalculator calc, int indexCount, Dictionary<string, object> args)
    {
        object ret = null;
        if (null != importer && null != calc) {
            calc.NamedVariables.Clear();
            calc.NamedVariables.Add("assetpath", path);
            calc.NamedVariables.Add("importer", importer);
            calc.NamedVariables.Add("object", obj);
            foreach (var pair in args) {
                calc.NamedVariables.Add(pair.Key, pair.Value);
            }

            for (int i = 0; i < indexCount; ++i) {
                ret = calc.Calc(i.ToString());
            }
        }
        return ret;
    }

    private static GameObject FindRoot(GameObject obj)
    {
        GameObject ret = null;
        var trans = obj.transform;
        while (null != trans && !(trans is RectTransform)) {
            ret = trans.gameObject;
            trans = trans.parent;
        }
        return ret;
    }
    private static UnityEngine.Object LoadAssetByPathAndName(string path, string name)
    {
        var objs = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (var obj in objs) {
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    private static void AppendLine(StringBuilder sb, string format, params object[] args)
    {
        sb.AppendFormat(format, args);
        sb.AppendLine();
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

    private const string c_IndentString = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
}

#region API
internal class SaveAndReimportExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as TextureImporter;
            if (null != importer) {
                //importer.SaveAndReimport();
                try {
                    AssetDatabase.StartAssetEditing();
                    AssetDatabase.ImportAsset(importer.assetPath);
                } finally {
                    AssetDatabase.StopAssetEditing();
                }
            }
        }
        return r;
    }
}
internal class GetDefaultTextureSettingExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as TextureImporter;
            if (null != importer) {
                r = importer.GetDefaultPlatformTextureSettings();
            }
        }
        return r;
    }
}
internal class GetTextureSettingExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as TextureImporter;
            var platform = operands[0] as string;
            if (null != importer) {
                r = importer.GetPlatformTextureSettings(platform);
            }
        }
        return r;
    }
}
internal class SetTextureSettingExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as TextureImporter;
            var setting = operands[0] as TextureImporterPlatformSettings;
            if (null != importer && null != setting) {
                importer.SetPlatformTextureSettings(setting);
                r = setting;
            }
        }
        return r;
    }
}
internal class GetTextureCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var setting = operands[0] as TextureImporterPlatformSettings;
            if (null != setting) {
                switch (setting.textureCompression) {
                    case TextureImporterCompression.Uncompressed:
                        r = "none";
                        break;
                    case TextureImporterCompression.CompressedLQ:
                        r = "lowquality";
                        break;
                    case TextureImporterCompression.Compressed:
                        r = "normal";
                        break;
                    case TextureImporterCompression.CompressedHQ:
                        r = "highquality";
                        break;
                }
            }
        }
        return r;
    }
}
internal class SetTextureCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 2) {
            var setting = operands[0] as TextureImporterPlatformSettings;
            var type = operands[1] as string;
            if (null != setting && null != type) {
                r = type;
                if (type == "none")
                    setting.textureCompression = TextureImporterCompression.Uncompressed;
                else if (type == "lowquality")
                    setting.textureCompression = TextureImporterCompression.CompressedLQ;
                else if (type == "normal")
                    setting.textureCompression = TextureImporterCompression.Compressed;
                else if (type == "highquality")
                    setting.textureCompression = TextureImporterCompression.CompressedHQ;
            }
        }
        return r;
    }
}
internal class GetMeshCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            if (null != importer) {
                switch (importer.meshCompression) {
                    case ModelImporterMeshCompression.Off:
                        r = "off";
                        break;
                    case ModelImporterMeshCompression.Low:
                        r = "low";
                        break;
                    case ModelImporterMeshCompression.Medium:
                        r = "medium";
                        break;
                    case ModelImporterMeshCompression.High:
                        r = "high";
                        break;
                }
            }
        }
        return r;
    }
}
internal class SetMeshCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            var type = operands[0] as string;
            if (null != importer && null != type) {
                r = type;
                if (type == "off")
                    importer.meshCompression = ModelImporterMeshCompression.Off;
                else if (type == "low")
                    importer.meshCompression = ModelImporterMeshCompression.Low;
                else if (type == "medium")
                    importer.meshCompression = ModelImporterMeshCompression.Medium;
                else if (type == "high")
                    importer.meshCompression = ModelImporterMeshCompression.High;
            }
        }
        return r;
    }
}
internal class CloseMeshAnimationIfNoAnimationExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            if (null != importer) {
                if (importer.importedTakeInfos.Length <= 0 && importer.defaultClipAnimations.Length <= 0 && importer.clipAnimations.Length <= 0) {
                    importer.animationType = ModelImporterAnimationType.None;
                }
            }
        }
        return r;
    }
}
internal class GetAnimationCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            if (null != importer) {
                switch (importer.animationCompression) {
                    case ModelImporterAnimationCompression.Off:
                        r = "off";
                        break;
                    case ModelImporterAnimationCompression.KeyframeReduction:
                        r = "keyframe";
                        break;
                    case ModelImporterAnimationCompression.KeyframeReductionAndCompression:
                        r = "keyframeandcompression";
                        break;
                    case ModelImporterAnimationCompression.Optimal:
                        r = "optimal";
                        break;
                }
            }
        }
        return r;
    }
}
internal class SetAnimationCompressionExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            var type = operands[0] as string;
            if (null != importer && null != type) {
                r = type;
                if (type == "off")
                    importer.animationCompression = ModelImporterAnimationCompression.Off;
                else if (type == "keyframe")
                    importer.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
                else if (type == "keyframeandcompression")
                    importer.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
                else if (type == "optimal")
                    importer.animationCompression = ModelImporterAnimationCompression.Optimal;
            }
        }
        return r;
    }
}
internal class GetAnimationTypeExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            if (null != importer) {
                switch (importer.animationType) {
                    case ModelImporterAnimationType.None:
                        r = "none";
                        break;
                    case ModelImporterAnimationType.Legacy:
                        r = "legacy";
                        break;
                    case ModelImporterAnimationType.Generic:
                        r = "generic";
                        break;
                    case ModelImporterAnimationType.Human:
                        r = "humanoid";
                        break;
                }
            }
        }
        return r;
    }
}
internal class SetAnimationTypeExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as ModelImporter;
            var type = operands[0] as string;
            if (null != importer && null != type) {
                r = type;
                if (type == "none")
                    importer.animationType = ModelImporterAnimationType.None;
                else if (type == "legacy")
                    importer.animationType = ModelImporterAnimationType.Legacy;
                else if (type == "generic")
                    importer.animationType = ModelImporterAnimationType.Generic;
                else if (type == "humanoid")
                    importer.animationType = ModelImporterAnimationType.Human;
            }
        }
        return r;
    }
}
internal class ClearAnimationScaleCurveExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 0) {
            var path = Calculator.NamedVariables["assetpath"] as string;
            if (null != path) {
                GameObject obj = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
                List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(obj));
                foreach (AnimationClip theAnimation in animationClipList) {
                    foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation)) {
                        string name = theCurveBinding.propertyName.ToLower();
                        if (name.Contains("scale")) {
                            AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                        }
                    }
                }
            }
        }
        return r;
    }
}
internal class GetAudioSettingExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var importer = Calculator.NamedVariables["importer"] as AudioImporter;
            var platform = operands[0] as string;
            if (null != importer) {
                r = importer.GetOverrideSampleSettings(platform);
            }
        }
        return r;
    }
}
internal class SetAudioSettingExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 2) {
            var importer = Calculator.NamedVariables["importer"] as AudioImporter;
            var platform = operands[0] as string;
            var setting = (AudioImporterSampleSettings)operands[1];
            if (null != importer) {
                importer.SetOverrideSampleSettings(platform, setting);
                r = setting;
            }
        }
        return r;
    }
}
#endregion

#region 解释器
namespace Expression
{
    public interface IExpression
    {
        object Calc(object[] args);
        bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator);
    }
    public interface IExpressionFactory
    {
        IExpression Create();
    }
    public sealed class ExpressionFactoryHelper<T> : IExpressionFactory where T : IExpression, new()
    {
        public IExpression Create()
        {
            return new T();
        }
    }
    public abstract class AbstractExpression : IExpression
    {
        public abstract object Calc(object[] args);
        public bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator)
        {
            m_Calculator = calculator;
            Dsl.ValueData valueData = dsl as Dsl.ValueData;
            if (null != valueData) {
                return Load(valueData);
            } else {
                Dsl.CallData callData = dsl as Dsl.CallData;
                if (null != callData) {
                    bool ret = Load(callData);
                    if (!ret) {
                        int num = callData.GetParamNum();
                        List<IExpression> args = new List<IExpression>();
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent param = callData.GetParam(ix);
                            args.Add(calculator.Load(param));
                        }
                        return Load(args);
                    }
                    return ret;
                } else {
                    Dsl.FunctionData funcData = dsl as Dsl.FunctionData;
                    if (null != funcData) {
                        return Load(funcData);
                    } else {
                        Dsl.StatementData statementData = dsl as Dsl.StatementData;
                        if (null != statementData) {
                            return Load(statementData);
                        }
                    }
                }
            }
            return false;
        }
        protected virtual bool Load(Dsl.ValueData valData) { return false; }
        protected virtual bool Load(Dsl.CallData callData) { return false; }
        protected virtual bool Load(IList<IExpression> exps) { return false; }
        protected virtual bool Load(Dsl.FunctionData funcData) { return false; }
        protected virtual bool Load(Dsl.StatementData statementData) { return false; }

        protected DslCalculator Calculator {
            get { return m_Calculator; }
        }

        private DslCalculator m_Calculator = null;

        protected static double ToDouble(object v)
        {
            return (double)Convert.ChangeType(v, typeof(double));
        }
        protected static long ToLong(object v)
        {
            return (long)Convert.ChangeType(v, typeof(long));
        }
        protected static string ToString(object v)
        {
            return v.ToString();
        }
        protected static T CastTo<T>(object v)
        {
            return (T)Convert.ChangeType(v, typeof(T));
        }
    }
    public abstract class SimpleExpressionBase : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            List<object> operands = new List<object>();
            for (int i = 0; i < m_Exps.Count; ++i) {
                object v = m_Exps[i].Calc(args);
                operands.Add(v);
            }
            return OnCalc(operands, args);
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Exps = exps;
            return true;
        }
        protected abstract object OnCalc(IList<object> operands, object[] args);

        private IList<IExpression> m_Exps = null;
    }
    internal sealed class VarSet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Op.Calc(args);
            m_Variables[m_VarId] = v;
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.CallData param1 = callData.GetParam(0) as Dsl.CallData;
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(param1.GetParamId(0));
            m_Op = Calculator.Load(param2);
            return true;
        }

        private Dictionary<int, object> m_Variables;
        private int m_VarId;
        private IExpression m_Op;
    }
    internal sealed class VarGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = 0;
            m_Variables.TryGetValue(m_VarId, out ret);
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(callData.GetParamId(0));
            return true;
        }

        private Dictionary<int, object> m_Variables;
        private int m_VarId;
    }
    internal sealed class NamedVarSet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Op.Calc(args);
            m_Variables[m_VarId] = v;
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Variables = Calculator.NamedVariables;
            m_VarId = param1.GetId();
            m_Op = Calculator.Load(param2);
            return true;
        }

        private Dictionary<string, object> m_Variables;
        private string m_VarId;
        private IExpression m_Op;
    }
    internal sealed class NamedVarGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = 0;
            m_Variables.TryGetValue(m_VarId, out ret);
            return ret;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_Variables = Calculator.NamedVariables;
            m_VarId = valData.GetId();
            return true;
        }

        private Dictionary<string, object> m_Variables;
        private string m_VarId;
    }
    internal sealed class ArgGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            if (m_Index >= 0 && m_Index < args.Length) {
                v = args[m_Index];
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_Index = int.Parse(callData.GetParamId(0));
            return true;
        }

        private int m_Index;
    }
    internal sealed class ConstGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Val;
            return v;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            string id = valData.GetId();
            int idType = valData.GetIdType();
            if (idType == Dsl.ValueData.NUM_TOKEN) {
                if (id.StartsWith("0x")) {
                    long v = long.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    if (v >= int.MinValue && v <= int.MaxValue) {
                        m_Val = (int)v;
                    } else {
                        m_Val = v;
                    }
                } else if (id.IndexOf('.') < 0) {
                    long v = long.Parse(id);
                    if (v >= int.MinValue && v <= int.MaxValue) {
                        m_Val = (int)v;
                    } else {
                        m_Val = v;
                    }
                } else {
                    double v = double.Parse(id);
                    if (v >= float.MinValue && v <= float.MaxValue) {
                        m_Val = (float)v;
                    } else {
                        m_Val = v;
                    }
                }
            } else if (idType == Dsl.ValueData.BOOL_TOKEN) {
                m_Val = id == "true";
            } else {
                m_Val = id;
            }
            return true;
        }

        private object m_Val;
    }
    internal sealed class AddExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v;
            if (v1 is string || v2 is string) {
                v = ToString(v1) + ToString(v2);
            } else {
                v = ToDouble(v1) + ToDouble(v2);
            }
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SubExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) - ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MulExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) * ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class DivExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) / ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class ModExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) % ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitAndExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) & ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitOrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) | ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitXorExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) ^ ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitNotExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v = ~ToLong(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LShiftExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) << (int)ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class RShiftExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) >> (int)ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MaxExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 >= v2 ? v1 : v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 <= v2 ? v1 : v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class AbsExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = v1 >= 0 ? v1 : -v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class SinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Sin((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CosExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Cos((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Tan((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AsinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Asin((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AcosExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Acos((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AtanExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Atan((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class Atan2Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)Mathf.Atan2((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SinhExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Sinh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CoshExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Cosh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanhExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Tanh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class RndIntExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = ToLong(m_Op2.Calc(args));
            object v = (long)UnityEngine.Random.Range((int)v1, (int)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class RndFloatExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)UnityEngine.Random.Range((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class PowExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)Mathf.Pow((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SqrtExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Sqrt((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LogExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Log((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class Log10Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Log10((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FloorExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Floor((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CeilExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Ceil((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LerpExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.Lerp((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpUnclampedExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.LerpUnclamped((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpAngleExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.LerpAngle((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class Clamp01Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Clamp01((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class ClampExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v;
            if (v3 < v1)
                v = v1;
            else if (v3 > v2)
                v = v2;
            else
                v = v3;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class DistExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            float x1 = (float)ToDouble(m_Op1.Calc(args));
            float y1 = (float)ToDouble(m_Op2.Calc(args));
            float x2 = (float)ToDouble(m_Op3.Calc(args));
            float y2 = (float)ToDouble(m_Op4.Calc(args));
            object v = Geometry.Distance(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class DistSqrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            float x1 = (float)ToDouble(m_Op1.Calc(args));
            float y1 = (float)ToDouble(m_Op2.Calc(args));
            float x2 = (float)ToDouble(m_Op3.Calc(args));
            float y2 = (float)ToDouble(m_Op4.Calc(args));
            object v = Geometry.DistanceSquare(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class GreatExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 > v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class GreatEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 >= v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class LessExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 < v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class LessEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 <= v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class EqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToString(v1) == ToString(v2) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class NotEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToString(v1) != ToString(v2) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class AndExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = 0;
            object v = v1 != 0 && (v2 = ToLong(m_Op2.Calc(args))) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class OrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = 0;
            object v = v1 != 0 || (v2 = ToLong(m_Op2.Calc(args))) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class NotExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long val = ToLong(m_Op.Calc(args));
            object v = val == 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CondExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = null;
            object v3 = null;
            object v = ToDouble(v1) != 0 ? v2 = m_Op2.Calc(args) : v3 = m_Op3.Calc(args);
            return v;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            Dsl.FunctionData funcData1 = statementData.First;
            Dsl.FunctionData funcData2 = statementData.Second;
            if (funcData2.GetId() == ":") {
                Dsl.ISyntaxComponent cond = funcData1.Call.GetParam(0);
                Dsl.ISyntaxComponent op1 = funcData1.GetStatement(0);
                Dsl.ISyntaxComponent op2 = funcData2.GetStatement(0);
                m_Op1 = Calculator.Load(cond);
                m_Op2 = Calculator.Load(op1);
                m_Op3 = Calculator.Load(op2);
            } else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", statementData.ToScriptString(false), statementData.GetLine());
            }
            return true;
        }

        private IExpression m_Op1 = null;
        private IExpression m_Op2 = null;
        private IExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    object condVal = clause.Condition.Calc(args);
                    if (ToDouble(condVal) != 0) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            v = clause.Expressions[index].Calc(args);
                        }
                        break;
                    }
                } else if (ix == m_Clauses.Count - 1) {
                    for (int index = 0; index < clause.Expressions.Count; ++index) {
                        v = clause.Expressions[index].Calc(args);
                    }
                    break;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            IfExp.Clause item = new IfExp.Clause();
            item.Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                item.Expressions.Add(subExp);
            }
            m_Clauses.Add(item);
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            foreach (var fData in statementData.Functions) {
                if (fData.GetId() == "if" || fData.GetId() == "elseif") {
                    IfExp.Clause item = new IfExp.Clause();
                    if (fData.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = fData.Call.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    } else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    }
                    for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                        IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                        item.Expressions.Add(subExp);
                    }
                    m_Clauses.Add(item);
                } else if (fData.GetId() == "else") {
                    if (fData != statementData.Last) {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    } else {
                        IfExp.Clause item = new IfExp.Clause();
                        for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                            IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                            item.Expressions.Add(subExp);
                        }
                        m_Clauses.Add(item);
                    }
                } else {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                }
            }
            return true;
        }

        private sealed class Clause
        {
            internal IExpression Condition;
            internal List<IExpression> Expressions = new List<IExpression>();
        }

        private List<Clause> m_Clauses = new List<Clause>();
    }
    internal sealed class WhileExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (;;) {
                object condVal = m_Condition.Calc(args);
                if (ToDouble(condVal) != 0) {
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc(args);
                    }
                } else {
                    break;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            m_Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_Condition;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            object count = m_Count.Calc(args);
            long ct = ToLong(count);
            for (int i = 0; i < ct; ++i) {
                Calculator.NamedVariables["$$"] = i;
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc(args);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent count = funcData.Call.GetParam(0);
            m_Count = Calculator.Load(count);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_Count;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopListExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            object list = m_List.Calc(args);
            IEnumerable obj = list as IEnumerable;
            if (null != obj) {
                IEnumerator enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    object val = enumer.Current;
                    Calculator.NamedVariables["$$"] = val;
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc(args);
                    }
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent list = funcData.Call.GetParam(0);
            m_List = Calculator.Load(list);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_List;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ForeachExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            List<object> list = new List<object>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                object val = m_Elements[ix].Calc(args);
                list.Add(val);
            }
            IEnumerator enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                object val = enumer.Current;
                Calculator.NamedVariables["$$"] = val;
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc(args);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.CallData callData = funcData.Call;
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent cond = funcData.Call.GetParam(ix);
                m_Elements.Add(Calculator.Load(cond));
            }
            int fnum = funcData.GetStatementNum();
            for (int ix = 0; ix < fnum; ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private List<IExpression> m_Elements = new List<IExpression>();
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ParenthesisExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc(args);
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class GetTypeExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                string type = m_Expressions[0].Calc(args) as string;
                try {
                    ret = Type.GetType(type);
                    if (null == ret) {
                        Debug.LogWarningFormat("null == Type.GetType({0})", type);
                    }
                } catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ChangeTypeExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            if (m_Expressions.Count >= 2) {
                object obj = m_Expressions[0].Calc(args);
                string type = m_Expressions[1].Calc(args) as string;
                try {
                    if (0 == type.CompareTo("sbyte")) {
                        ret = CastTo<sbyte>(obj);
                    } else if (0 == type.CompareTo("byte")) {
                        ret = CastTo<byte>(obj);
                    } else if (0 == type.CompareTo("short")) {
                        ret = CastTo<short>(obj);
                    } else if (0 == type.CompareTo("ushort")) {
                        ret = CastTo<ushort>(obj);
                    } else if (0 == type.CompareTo("int")) {
                        ret = CastTo<int>(obj);
                    } else if (0 == type.CompareTo("uint")) {
                        ret = CastTo<uint>(obj);
                    } else if (0 == type.CompareTo("long")) {
                        ret = CastTo<long>(obj);
                    } else if (0 == type.CompareTo("ulong")) {
                        ret = CastTo<ulong>(obj);
                    } else if (0 == type.CompareTo("float")) {
                        ret = CastTo<float>(obj);
                    } else if (0 == type.CompareTo("double")) {
                        ret = CastTo<double>(obj);
                    } else if (0 == type.CompareTo("string")) {
                        ret = CastTo<string>(obj);
                    } else if (0 == type.CompareTo("bool")) {
                        ret = CastTo<bool>(obj);
                    } else {
                        Type t = Type.GetType(type);
                        if (null != t) {
                            ret = Convert.ChangeType(obj, t);
                        } else {
                            Debug.LogWarningFormat("null == Type.GetType({0})", type);
                        }
                    }
                } catch (Exception ex) {
                    Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetCallExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            object obj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc(args);
                if (ix == 0)
                    obj = v;
                else if (ix == 1)
                    method = v as string;
                else
                    arglist.Add(v);
            }
            object[] _args = arglist.ToArray();
            if (null != obj && !string.IsNullOrEmpty(method)) {
                Type t = obj as Type;
                if (null != t) {
                    try {
                        BindingFlags flags = BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                        Converter.CastArgsForCall(t, method, flags, _args);
                        ret = t.InvokeMember(method, flags, null, null, _args);
                    } catch (Exception ex) {
                        Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                } else {
                    t = obj.GetType();
                    if (null != t) {
                        try {
                            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
                            Converter.CastArgsForCall(t, method, flags, _args);
                            ret = t.InvokeMember(method, flags, null, obj, _args);
                        } catch (Exception ex) {
                            Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetSetExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            object obj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc(args);
                if (ix == 0)
                    obj = v;
                else if (ix == 1)
                    method = v as string;
                else
                    arglist.Add(v);
            }
            object[] _args = arglist.ToArray();
            if (null != obj && !string.IsNullOrEmpty(method)) {
                Type t = obj as Type;
                if (null != t) {
                    try {
                        BindingFlags flags = BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                        Converter.CastArgsForSet(t, method, flags, _args);
                        ret = t.InvokeMember(method, flags, null, null, _args);
                    } catch (Exception ex) {
                        Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                } else {
                    t = obj.GetType();
                    if (null != t) {
                        try {
                            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                            Converter.CastArgsForSet(t, method, flags, _args);
                            ret = t.InvokeMember(method, flags, null, obj, _args);
                        } catch (Exception ex) {
                            Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class DotnetGetExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            object obj = null;
            string method = null;
            ArrayList arglist = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                var v = exp.Calc(args);
                if (ix == 0)
                    obj = v;
                else if (ix == 1)
                    method = v as string;
                else
                    arglist.Add(v);
            }
            object[] _args = arglist.ToArray();
            if (null != obj && !string.IsNullOrEmpty(method)) {
                Type t = obj as Type;
                if (null != t) {
                    try {
                        BindingFlags flags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                        Converter.CastArgsForGet(t, method, flags, _args);
                        ret = t.InvokeMember(method, flags, null, null, _args);
                    } catch (Exception ex) {
                        Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                } else {
                    t = obj.GetType();
                    if (null != t) {
                        try {
                            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
                            Converter.CastArgsForGet(t, method, flags, _args);
                            ret = t.InvokeMember(method, flags, null, obj, _args);
                        } catch (Exception ex) {
                            Debug.LogWarningFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class IsNullExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = null;
            if (m_Expressions.Count >= 1) {
                var obj = m_Expressions[0].Calc(args);
                UnityEngine.Object uo = obj as UnityEngine.Object;
                if (!System.Object.ReferenceEquals(null, uo))
                    ret = null == uo;
                else
                    ret = null == obj;
            }
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal class LoadAssetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (null != path) {
                    r = AssetDatabase.LoadMainAssetAtPath(path);
                }
            }
            return r;
        }
    }
    internal class UnloadAssetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    Resources.UnloadAsset(obj);
                }
            }
            return r;
        }
    }
    internal class GetComponentExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponent(t);
                    }
                }
            }
            return r;
        }
    }
    internal class GetComponentInChildrenExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponentInChildren(t);
                    }
                }
            }
            return r;
        }
    }
    internal class GetComponentsInChildrenExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var obj = operands[0] as GameObject;
                var type = operands[1] as string;
                if (null != obj && null != type) {
                    Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == t) {
                        t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == t) {
                        t = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null != t) {
                        r = obj.GetComponentsInChildren(t);
                    }
                }
            }
            return r;
        }
    }
    public sealed class DslCalculator
    {
        public void Init()
        {
            Register("arg", new ExpressionFactoryHelper<ArgGet>());
            Register("var", new ExpressionFactoryHelper<VarGet>());
            Register("+", new ExpressionFactoryHelper<AddExp>());
            Register("-", new ExpressionFactoryHelper<SubExp>());
            Register("*", new ExpressionFactoryHelper<MulExp>());
            Register("/", new ExpressionFactoryHelper<DivExp>());
            Register("%", new ExpressionFactoryHelper<ModExp>());
            Register("&", new ExpressionFactoryHelper<BitAndExp>());
            Register("|", new ExpressionFactoryHelper<BitOrExp>());
            Register("^", new ExpressionFactoryHelper<BitXorExp>());
            Register("~", new ExpressionFactoryHelper<BitNotExp>());
            Register("<<", new ExpressionFactoryHelper<LShiftExp>());
            Register(">>", new ExpressionFactoryHelper<RShiftExp>());
            Register("max", new ExpressionFactoryHelper<MaxExp>());
            Register("min", new ExpressionFactoryHelper<MinExp>());
            Register("abs", new ExpressionFactoryHelper<AbsExp>());
            Register("sin", new ExpressionFactoryHelper<SinExp>());
            Register("cos", new ExpressionFactoryHelper<CosExp>());
            Register("tan", new ExpressionFactoryHelper<TanExp>());
            Register("asin", new ExpressionFactoryHelper<AsinExp>());
            Register("acos", new ExpressionFactoryHelper<AcosExp>());
            Register("atan", new ExpressionFactoryHelper<AtanExp>());
            Register("atan2", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", new ExpressionFactoryHelper<TanhExp>());
            Register("rndint", new ExpressionFactoryHelper<RndIntExp>());
            Register("rndfloat", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", new ExpressionFactoryHelper<SqrtExp>());
            Register("log", new ExpressionFactoryHelper<LogExp>());
            Register("log10", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", new ExpressionFactoryHelper<FloorExp>());
            Register("ceil", new ExpressionFactoryHelper<CeilExp>());
            Register("lerp", new ExpressionFactoryHelper<LerpExp>());
            Register("lerpunclamped", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerpangle", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("clamp01", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", new ExpressionFactoryHelper<ClampExp>());
            Register("dist", new ExpressionFactoryHelper<DistExp>());
            Register("distsqr", new ExpressionFactoryHelper<DistSqrExp>());
            Register(">", new ExpressionFactoryHelper<GreatExp>());
            Register(">=", new ExpressionFactoryHelper<GreatEqualExp>());
            Register("<", new ExpressionFactoryHelper<LessExp>());
            Register("<=", new ExpressionFactoryHelper<LessEqualExp>());
            Register("==", new ExpressionFactoryHelper<EqualExp>());
            Register("!=", new ExpressionFactoryHelper<NotEqualExp>());
            Register("&&", new ExpressionFactoryHelper<AndExp>());
            Register("||", new ExpressionFactoryHelper<OrExp>());
            Register("!", new ExpressionFactoryHelper<NotExp>());
            Register("?", new ExpressionFactoryHelper<CondExp>());
            Register("if", new ExpressionFactoryHelper<IfExp>());
            Register("while", new ExpressionFactoryHelper<WhileExp>());
            Register("loop", new ExpressionFactoryHelper<LoopExp>());
            Register("looplist", new ExpressionFactoryHelper<LoopListExp>());
            Register("foreach", new ExpressionFactoryHelper<ForeachExp>());
            Register("gettype", new ExpressionFactoryHelper<GetTypeExp>());
            Register("changetype", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("dotnetcall", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnetset", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnetget", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("isnull", new ExpressionFactoryHelper<IsNullExp>());
            Register("loadasset", new ExpressionFactoryHelper<LoadAssetExp>());
            Register("unloadasset", new ExpressionFactoryHelper<UnloadAssetExp>());
            Register("getcomponent", new ExpressionFactoryHelper<GetComponentExp>());
            Register("getcomponentinchildren", new ExpressionFactoryHelper<GetComponentInChildrenExp>());
            Register("getcomponentsinchildren", new ExpressionFactoryHelper<GetComponentsInChildrenExp>());
        }
        public void Register(string name, IExpressionFactory factory)
        {
            if (!m_ExpressionFactories.ContainsKey(name)) {
                m_ExpressionFactories.Add(name, factory);
            } else {
                m_ExpressionFactories[name] = factory;
            }
        }
        public void Load(string proc, Dsl.FunctionData func)
        {
            List<IExpression> list;
            if (!m_Procs.TryGetValue(proc, out list)) {
                list = new List<IExpression>();
                m_Procs.Add(proc, list);
            }
            foreach (Dsl.ISyntaxComponent comp in func.Statements) {
                var exp = Load(comp);
                if (null != exp) {
                    list.Add(exp);
                }
            }
        }
        public object Calc(string proc, params object[] args)
        {
            object ret = 0;
            m_Variables.Clear();
            List<IExpression> exps;
            if (m_Procs.TryGetValue(proc, out exps)) {
                for (int i = 0; i < exps.Count; ++i) {
                    ret = exps[i].Calc(args);
                }
            }
            return ret;
        }

        public Dictionary<string, object> NamedVariables 
		{
            get { return m_NamedVariables; }
        }
        public Dictionary<int, object> Variables
        {
            get { return m_Variables; }
        }
        internal IExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            if (null != valueData) {
                int idType = valueData.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    NamedVarGet varExp = new NamedVarGet();
                    varExp.Load(comp, this);
                    return varExp;
                } else {
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                }
            } else {
                Dsl.CallData callData = comp as Dsl.CallData;
                if (null != callData) {
                    if (!callData.HaveId()) {
                        int num = callData.GetParamNum();
                        if (num == 1) {
                            Dsl.ISyntaxComponent param = callData.GetParam(0);
                            return Load(param);
                        } else {
                            ParenthesisExp exp = new ParenthesisExp();
                            exp.Load(comp, this);
                            return exp;
                        }
                    } else {
                        int paramClass = callData.GetParamClass();
                        string op = callData.GetId();
                        if (op == "=") {//赋值
                            Dsl.CallData innerCall = callData.GetParam(0) as Dsl.CallData;
                            if (null != innerCall) {
                                //obj.property = val -> dotnetset(obj, property, val)
                                int innerParamClass = innerCall.GetParamClass();
                                if (innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                                  innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                                    Dsl.CallData newCall = new Dsl.CallData();
                                    newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                    if (innerCall.IsHighOrder) {
                                        newCall.Params.Add(innerCall.Call);
                                        newCall.Params.Add(innerCall.GetParam(0));
                                        newCall.Params.Add(callData.GetParam(1));
                                    } else {
                                        newCall.Params.Add(innerCall.Name);
                                        newCall.Params.Add(innerCall.GetParam(0));
                                        newCall.Params.Add(callData.GetParam(1));
                                    }

                                    var setExp = new DotnetSetExp();
                                    setExp.Load(newCall, this);
                                    return setExp;
                                }
                            }
                            IExpression exp = null;
                            string name = callData.GetParamId(0);
                            if (name == "var") {
                                exp = new VarSet();
                            } else {
                                exp = new NamedVarSet();
                            }
                            if (null != exp) {
                                exp.Load(comp, this);
                            } else {
                                //error
                                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", callData.ToScriptString(false), callData.GetLine());
                            }
                            return exp;
                        } else {
                            if (callData.IsHighOrder) {
                                Dsl.CallData innerCall = callData.Call;
                                int innerParamClass = innerCall.GetParamClass();
                                if (paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS && (
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                                    innerParamClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS)) {
                                    //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                    Dsl.CallData newCall = new Dsl.CallData();
                                    newCall.Name = new Dsl.ValueData("dotnetcall", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                    if (innerCall.IsHighOrder) {
                                        newCall.Params.Add(innerCall.Call);
                                        newCall.Params.Add(innerCall.GetParam(0));
                                        for (int i = 0; i < callData.GetParamNum(); ++i) {
                                            Dsl.ISyntaxComponent p = callData.Params[i];
                                            newCall.Params.Add(p);
                                        }
                                    } else {
                                        newCall.Params.Add(innerCall.Name);
                                        newCall.Params.Add(innerCall.GetParam(0));
                                        for (int i = 0; i < callData.GetParamNum(); ++i) {
                                            Dsl.ISyntaxComponent p = callData.Params[i];
                                            newCall.Params.Add(p);
                                        }
                                    }

                                    var callExp = new DotnetCallExp();
                                    callExp.Load(newCall, this);
                                    return callExp;
                                }
                            }
                            if (paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                              paramClass == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                                //obj.property or obj[property] or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                Dsl.CallData newCall = new Dsl.CallData();
                                newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                if (callData.IsHighOrder) {
                                    newCall.Params.Add(callData.Call);
                                    newCall.Params.Add(callData.GetParam(0));
                                } else {
                                    newCall.Params.Add(callData.Name);
                                    newCall.Params.Add(callData.GetParam(0));
                                }

                                var getExp = new DotnetGetExp();
                                getExp.Load(newCall, this);
                                return getExp;
                            }
                        }
                    }
                }
            }
            IExpression ret = Create(comp.GetId());
            if (null != ret) {
                if (!ret.Load(comp, this)) {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                }
            } else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
            }
            return ret;
        }

        private IExpression Create(string name)
        {
            IExpression ret = null;
            IExpressionFactory factory;
            if (m_ExpressionFactories.TryGetValue(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }

        private Dictionary<string, List<IExpression>> m_Procs = new Dictionary<string, List<IExpression>>();
        private Dictionary<int, object> m_Variables = new Dictionary<int, object>();
        private Dictionary<string, object> m_NamedVariables = new Dictionary<string, object>();
        private Dictionary<string, IExpressionFactory> m_ExpressionFactories = new Dictionary<string, IExpressionFactory>();
    }
}
#endregion