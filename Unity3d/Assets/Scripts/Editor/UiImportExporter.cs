using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public sealed class UiEditWindow : EditorWindow
{
    [MenuItem("工具/UI布局与挂点导入导出")]
    internal static void InitWindow()
    {
        UiEditWindow window = (UiEditWindow)EditorWindow.GetWindow(typeof(UiEditWindow));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("收集所有UI预制件")) {
            Collect();
        }
        if (GUILayout.Button("导出选中预制件")) {
            ExportSelectedPrefabs();
        }
        if (GUILayout.Button("导出选中场景结点")) {
            ExportSelectedSceneNodes();
        }
        if (GUILayout.Button("导入UI/GameObject")) {
            ImportAll();
        }
        if (GUILayout.Button("导入挂点到选中结点")) {
            ImportAttachPointToSelectedSceneNodes();
        }
        EditorGUILayout.EndHorizontal();

        m_Pos = EditorGUILayout.BeginScrollView(m_Pos, true, true);

        ListItem();

        EditorGUILayout.EndScrollView();
    }

    private void Collect()
    {
        string path = EditorUtility.OpenFolderPanel("请选择要收集UI预制件的根目录", Application.dataPath, string.Empty);
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
            m_Prefabs.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountFileRecursively(path);
            if (m_TotalSearchCount > 0) {
                SearchFileRecursively(path);
                EditorUtility.ClearProgressBar();
            }
        }
    }

    private void ExportSelectedPrefabs()
    {
        string path = EditorUtility.SaveFilePanel("请选择要导出的dsl文件", string.Empty, string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path)) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
            foreach (ItemInfo item in m_Prefabs) {
                if (item.Selected) {                    
                    if (null != item.Prefab) {
                        UiEditUtility.Export(path, item.Prefab);
                        Debug.Log("BatchExportAll " + item.Path);
                    }
                }
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }

    private void ExportSelectedSceneNodes()
    {
        string path = EditorUtility.SaveFilePanel("请选择要导出的dsl文件", string.Empty, string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path)) {
            if (File.Exists(path)) {
                File.Delete(path);
            }            
            foreach (GameObject obj in Selection.gameObjects) {
                UiEditUtility.Export(path, obj);
                Debug.Log("BatchExportAll " + obj.name);
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }
    
    private void ImportAttachPointToSelectedSceneNodes()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            foreach (GameObject obj in Selection.gameObjects) {
                UiEditUtility.ImportAttachPoint(path, obj);
                Debug.Log("BatchExportAll " + obj.name);
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }

    private void ImportAll()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            var root = GameObject.Find("UiImportCanvas");
            if (null == root) {
                GameObject obj = new GameObject("UiImportCanvas");
                var canvas = obj.AddComponent<Canvas>();
                var scaler = obj.AddComponent<CanvasScaler>();
                obj.AddComponent<GraphicRaycaster>();

                canvas.planeDistance = 800.0f;
                canvas.gameObject.layer = 5;
                canvas.pixelPerfect = false;
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.referenceResolution = new Vector2(1136, 640);
                if (Screen.width / Screen.height <= scaler.referenceResolution.x / scaler.referenceResolution.y) {
                    scaler.matchWidthOrHeight = 0;
                } else {
                    scaler.matchWidthOrHeight = 1;
                }

                Camera uiCamera;
                GameObject objUICamera = GameObject.FindGameObjectWithTag("UICamera");
                if (objUICamera == null) {
                    GameObject uiCamObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/UICamera"));
                    uiCamera = uiCamObj.GetComponent<Camera>();
                    GameObject.DontDestroyOnLoad(uiCamera.gameObject);
                } else {
                    uiCamera = objUICamera.GetComponent<Camera>();
                }

                uiCamera.ResetAspect();
                float canvasScalerFator = 1 / ((Screen.width / scaler.referenceResolution.x) * (1 - scaler.matchWidthOrHeight) + (Screen.height / scaler.referenceResolution.y) * scaler.matchWidthOrHeight);
                uiCamera.orthographicSize = (Screen.height * canvasScalerFator) / 2;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = uiCamera;

                root = obj;
            }
            UiEditUtility.Import(path, root);
			EditorUtility.DisplayDialog ("提示", "处理完成", "ok");
        }
    }

    private void ListItem()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (ItemInfo item in m_Prefabs) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (ItemInfo item in m_Prefabs) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Go to page ({0}) : ", m_Prefabs.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = GUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        GUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_Prefabs.Count / c_ItemsPerPage + 1, m_Page));
        GUILayout.EndVertical();
        int index = 0;
        int totalShown = 0;
        foreach (ItemInfo item in m_Prefabs) {
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
            return Path.GetExtension(file).ToLower() == ".prefab";
        }).ToArray();
        foreach (string file in files) {
            var path = PathToAssetPath(file);
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var rectTrans = obj.GetComponent<RectTransform>();
            if (null != rectTrans) {
                m_Prefabs.Add(new ItemInfo { Path = path, Selected = false, Prefab = obj });
                ++m_CurSearchCount;
                EditorUtility.DisplayProgressBar("采集进度", string.Format("{0} in {1}/{2}", m_Prefabs.Count, m_CurSearchCount, m_TotalSearchCount), m_CurSearchCount * 1.0f / m_TotalSearchCount);
            }
        }        
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            SearchFileRecursively(subDir);
        }
    }

    private void CountFileRecursively(string dir)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            return Path.GetExtension(file).ToLower() == ".prefab";
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
        internal GameObject Prefab;
    }

    private List<ItemInfo> m_Prefabs = new List<ItemInfo>();
    private Vector2 m_Pos = Vector2.zero;
    private int m_Page = 1;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;

    private const int c_ItemsPerPage = 50;
}

internal static class UiEditUtility
{
    internal static void Import(string path, GameObject root)
    {
        Dsl.DslFile file = new Dsl.DslFile();
        if (file.Load(path, (string msg) => { Debug.Log(msg); })) {
            foreach (var info in file.DslInfos) {
                var func = info as Dsl.FunctionData;
                var stData = info as Dsl.StatementData;
                if(null==func && null != stData) {
                    func = stData.First.AsFunction;
                }
                if (null == func)
                    continue;
                ReadObject(func, root.transform);
            }
        }
    }

    internal static void ImportAttachPoint(string path, GameObject root)
    {
        Dsl.DslFile file = new Dsl.DslFile();
        if (file.Load(path, (string msg) => { Debug.Log(msg); })) {
            foreach (var info in file.DslInfos) {
                var func = info as Dsl.FunctionData;
                var stData = info as Dsl.StatementData;
                if (null == func && null != stData) {
                    func = stData.First.AsFunction;
                }
                if (null == func)
                    continue;
                foreach (var comp in func.Params) {
                    var funcData = comp as Dsl.FunctionData;
                    if (null != funcData && funcData.GetId() == "object") {
                        ReadAttachPoint(funcData, root.transform);
                    }
                }
            }
        }
    }

    internal static void Export(string path, GameObject prefab)
    {
        var trans = prefab.GetComponent<Transform>();
        if (null != trans) {
            using (var sw = new StreamWriter(path, true)) {
                WriteObject(sw, 0, trans);
                sw.Close();
            }
        }
    }

    private static void ReadObject(Dsl.FunctionData funcData, Transform trans)
    {
        var funcHeader = funcData;
        if (funcData.IsHighOrder)
            funcHeader = funcData.LowerOrderFunction;
        if (null == funcData)
            return;
        GameObject prefab = null;
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "asset") {
                var call = statement as Dsl.FunctionData;
                if (null != call) {
                    string path = call.GetParamId(0);
                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }
            }
        }
        GameObject obj;
        if (null == prefab) {
            obj = new GameObject(funcHeader.GetParamId(0));
        } else {
            obj = GameObject.Instantiate(prefab);
            obj.name = funcHeader.GetParamId(0);
        }
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "recttransform") {
                ReadRectTransform(statement, obj);
            } else if (id == "transform") {
                ReadTransform(statement, obj);
            }
        }
        obj.transform.SetParent(trans, false);
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "object") {
                ReadObject(statement as Dsl.FunctionData, obj.transform);
            }
        }
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "component") {
                ReadComponent(statement, obj);
            }
        }
    }
    private static void ReadRectTransform(Dsl.ISyntaxComponent comp, GameObject obj)
    {
        var funcData = comp as Dsl.FunctionData;
        if (null != funcData) {
            var funcHeader = funcData;
            if (funcData.IsHighOrder)
                funcHeader = funcData.LowerOrderFunction;
            var rect = obj.AddComponent<RectTransform>();
            if (null != rect) {
                float x = float.Parse(funcHeader.GetParamId(0));
                float y = float.Parse(funcHeader.GetParamId(1));
                float z = float.Parse(funcHeader.GetParamId(2));
                float w = float.Parse(funcHeader.GetParamId(3));
                float h = float.Parse(funcHeader.GetParamId(4));
                rect.anchoredPosition3D = new Vector3(x, y, z);
                rect.sizeDelta = new Vector2(w, h);
                foreach (var statement in funcData.Params) {
                    string id = statement.GetId();
                    if (id == "anchor") {
                        var call = statement as Dsl.FunctionData;
                        if (null != call) {
                            float a1 = float.Parse(call.GetParamId(0));
                            float a2 = float.Parse(call.GetParamId(1));
                            float a3 = float.Parse(call.GetParamId(2));
                            float a4 = float.Parse(call.GetParamId(3));
                            rect.anchorMin = new Vector2(a1, a2);
                            rect.anchorMax = new Vector2(a3, a4);
                        }
                    } else if (id == "pivot") {
                        var call = statement as Dsl.FunctionData;
                        if (null != call) {
                            float p1 = float.Parse(call.GetParamId(0));
                            float p2 = float.Parse(call.GetParamId(1));
                            rect.pivot = new Vector2(p1, p2);
                        }
                    } else if (id == "rotation") {
                        var call = statement as Dsl.FunctionData;
                        if (null != call) {
                            float r1 = float.Parse(call.GetParamId(0));
                            float r2 = float.Parse(call.GetParamId(1));
                            float r3 = float.Parse(call.GetParamId(2));
                            rect.localEulerAngles = new Vector3(r1, r2, r3);
                        }
                    } else if (id == "scale") {
                        var call = statement as Dsl.FunctionData;
                        if (null != call) {
                            float s1 = float.Parse(call.GetParamId(0));
                            float s2 = float.Parse(call.GetParamId(1));
                            float s3 = float.Parse(call.GetParamId(2));
                            rect.localScale = new Vector3(s1, s2, s3);
                        }
                    }
                }
            }
        }
    }
    private static void ReadTransform(Dsl.ISyntaxComponent comp, GameObject obj)
    {
        var funcData = comp as Dsl.FunctionData;
        if (null != funcData) {
            foreach (var statement in funcData.Params) {
                string id = statement.GetId();
                if (id == "position") {
                    var call = statement as Dsl.FunctionData;
                    if (null != call) {
                        float x = float.Parse(call.GetParamId(0));
                        float y = float.Parse(call.GetParamId(1));
                        float z = float.Parse(call.GetParamId(2));
                        obj.transform.localPosition = new Vector3(x, y, z);
                    }
                } else if (id == "rotation") {
                    var call = statement as Dsl.FunctionData;
                    if (null != call) {
                        float x = float.Parse(call.GetParamId(0));
                        float y = float.Parse(call.GetParamId(1));
                        float z = float.Parse(call.GetParamId(2));
                        obj.transform.localEulerAngles = new Vector3(x, y, z);
                    }
                } else if (id == "scale") {
                    var call = statement as Dsl.FunctionData;
                    if (null != call) {
                        float x = float.Parse(call.GetParamId(0));
                        float y = float.Parse(call.GetParamId(1));
                        float z = float.Parse(call.GetParamId(2));
                        obj.transform.localScale = new Vector3(x, y, z);
                    }
                }
            }
        }
    }
    private static void ReadComponent(Dsl.ISyntaxComponent comp, GameObject obj)
    {
        var funcData = comp as Dsl.FunctionData;
        if (null != funcData) {
            var callData = funcData;
            if (funcData.IsHighOrder)
                callData = funcData.LowerOrderFunction;
            int paramNum = callData.GetParamNum();
            string name = callData.GetParamId(0);
            System.Type type;
            if (paramNum > 1) {
                string fullName = callData.GetParamId(1);
                type = System.Type.GetType(fullName);
            } else {
                string fullName = string.Format("UnityEngine.UI.{0}, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", name);
                type = System.Type.GetType(fullName);
                if (null == type) {
                    fullName = string.Format("UnityEngine.{0}, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", name);
                    type = System.Type.GetType(fullName);
                }
                if (null == type) {
                    fullName = string.Format("{0}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", name);
                    type = System.Type.GetType(fullName);
                }
                if (null == type) {
                    fullName = string.Format("{0}, PluginFramework, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", name);
                    type = System.Type.GetType(fullName);
                }
                if (null == type) {
                    fullName = string.Format("ScriptableFramework.{0}, PluginFramework, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", name);
                    type = System.Type.GetType(fullName);
                }
            }
            if (null != type) {
                var cobj = obj.AddComponent(type);
                var img = cobj as Image;
                if (null != img) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "color") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float r = float.Parse(call.GetParamId(0));
                                float g = float.Parse(call.GetParamId(1));
                                float b = float.Parse(call.GetParamId(2));
                                float a = float.Parse(call.GetParamId(3));
                                img.color = new Color(r, g, b, a);
                            }
                        } else if (id == "sprite") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string sprite = call.GetParamId(0);
                                img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(sprite);
                            }
                        }
                    }
                    return;
                }
                var text = cobj as Text;
                if (null != text) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "color") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float r = float.Parse(call.GetParamId(0));
                                float g = float.Parse(call.GetParamId(1));
                                float b = float.Parse(call.GetParamId(2));
                                float a = float.Parse(call.GetParamId(3));
                                text.color = new Color(r, g, b, a);
                            }
                        } else if (id == "text") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string txt = call.GetParamId(0);
                                text.text = txt;
                            }
                        } else if (id == "align") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string alignEnum = call.GetParamId(0);
                                string boolVal = call.GetParamId(1);
                                text.alignment = ScriptableFramework.Converter.ConvertStrToEnum<TextAnchor>(alignEnum);
                                text.alignByGeometry = boolVal == "True";
                            }
                        } else if (id == "font") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string fontName = call.GetParamId(0);
                                string fontPath = call.GetParamId(1);
                                int size = int.Parse(call.GetParamId(2));
                                float lineSpacing = float.Parse(call.GetParamId(3));
                                string fontStyle = call.GetParamId(4);
                                var font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
                                if (null == font) {
                                    font = Font.CreateDynamicFontFromOSFont(fontName, size);
                                }
                                text.font = font;
                                text.fontSize = size;
                                text.lineSpacing = lineSpacing;
                                text.fontStyle = ScriptableFramework.Converter.ConvertStrToEnum<FontStyle>(fontStyle);
                            }
                        }
                    }
                    return;
                }
                var gridLayout = cobj as GridLayoutGroup;
                if (null != gridLayout) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "cellsize") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float x = float.Parse(call.GetParamId(0));
                                float y = float.Parse(call.GetParamId(1));
                                gridLayout.cellSize = new Vector2(x, y);
                            }
                        } else if (id == "spacing") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float x = float.Parse(call.GetParamId(0));
                                float y = float.Parse(call.GetParamId(1));
                                gridLayout.spacing = new Vector2(x, y);
                            }
                        } else if (id == "padding") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                int l = int.Parse(call.GetParamId(0));
                                int t = int.Parse(call.GetParamId(1));
                                int r = int.Parse(call.GetParamId(2));
                                int b = int.Parse(call.GetParamId(3));
                                gridLayout.padding = new RectOffset(l, r, t, b);
                            }
                        } else if (id == "options") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string e1 = call.GetParamId(0);
                                string e2 = call.GetParamId(1);
                                string e3 = call.GetParamId(2);
                                string e4 = call.GetParamId(3);
                                gridLayout.startCorner = ScriptableFramework.Converter.ConvertStrToEnum<GridLayoutGroup.Corner>(e1);
                                gridLayout.startAxis = ScriptableFramework.Converter.ConvertStrToEnum<GridLayoutGroup.Axis>(e2);
                                gridLayout.childAlignment = ScriptableFramework.Converter.ConvertStrToEnum<TextAnchor>(e3);
                                gridLayout.constraint = ScriptableFramework.Converter.ConvertStrToEnum<GridLayoutGroup.Constraint>(e4);
                            }
                        }
                    }
                    return;
                }
                var horizontalLayout = cobj as HorizontalLayoutGroup;
                if (null != horizontalLayout) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "spacing") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float x = float.Parse(call.GetParamId(0));
                                horizontalLayout.spacing = x;
                            }
                        } else if (id == "padding") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                int l = int.Parse(call.GetParamId(0));
                                int t = int.Parse(call.GetParamId(1));
                                int r = int.Parse(call.GetParamId(2));
                                int b = int.Parse(call.GetParamId(3));
                                horizontalLayout.padding = new RectOffset(l, r, t, b);
                            }
                        } else if (id == "options") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string o1 = call.GetParamId(0);
                                bool o2 = call.GetParamId(1) == "True";
                                bool o3 = call.GetParamId(2) == "True";
                                bool o4 = call.GetParamId(3) == "True";
                                bool o5 = call.GetParamId(4) == "True";
                                horizontalLayout.childAlignment = ScriptableFramework.Converter.ConvertStrToEnum<TextAnchor>(o1);
                                horizontalLayout.childControlWidth = o2;
                                horizontalLayout.childControlHeight = o3;
                                horizontalLayout.childForceExpandWidth = o4;
                                horizontalLayout.childForceExpandHeight = o5;
                            }
                        }
                    }
                    return;
                }
                var verticalLayout = cobj as VerticalLayoutGroup;
                if (null != verticalLayout) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "spacing") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                float x = float.Parse(call.GetParamId(0));
                                verticalLayout.spacing = x;
                            }
                        } else if (id == "padding") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                int l = int.Parse(call.GetParamId(0));
                                int t = int.Parse(call.GetParamId(1));
                                int r = int.Parse(call.GetParamId(2));
                                int b = int.Parse(call.GetParamId(3));
                                verticalLayout.padding = new RectOffset(l, r, t, b);
                            }
                        } else if (id == "options") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string o1 = call.GetParamId(0);
                                bool o2 = call.GetParamId(1) == "True";
                                bool o3 = call.GetParamId(2) == "True";
                                bool o4 = call.GetParamId(3) == "True";
                                bool o5 = call.GetParamId(4) == "True";
                                verticalLayout.childAlignment = ScriptableFramework.Converter.ConvertStrToEnum<TextAnchor>(o1);
                                verticalLayout.childControlWidth = o2;
                                verticalLayout.childControlHeight = o3;
                                verticalLayout.childForceExpandWidth = o4;
                                verticalLayout.childForceExpandHeight = o5;
                            }
                        }
                    }
                    return;
                }
                //---------------------------------------------------------------------------
                var animator = cobj as Animator;
                if (null != animator) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "controller") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string path = call.GetParamId(0);
                                animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
                            }
                        } else if (id == "avatar") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string path = call.GetParamId(0);
                                animator.avatar = AssetDatabase.LoadAssetAtPath<Avatar>(path);
                            }
                        } else if (id == "options") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                bool v1 = call.GetParamId(0) == "True";
                                string v2 = call.GetParamId(1);
                                string v3 = call.GetParamId(2);
                                animator.applyRootMotion = v1;
                                animator.updateMode = ScriptableFramework.Converter.ConvertStrToEnum<AnimatorUpdateMode>(v2);
                                animator.cullingMode = ScriptableFramework.Converter.ConvertStrToEnum<AnimatorCullingMode>(v3);
                            }
                        }
                    }
                }
                var meshFilter = cobj as MeshFilter;
                if (null != meshFilter) {
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "mesh") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string meshName = call.GetParamId(0);
                                string path = call.GetParamId(1);
                                meshFilter.sharedMesh = LoadAssetByPathAndName(path, meshName) as Mesh;
                            }
                        }
                    }
                }
                var meshRenderer = cobj as MeshRenderer;
                if (null != meshRenderer) {
                    List<Material> mats = new List<Material>();
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "material") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string matName = call.GetParamId(0);
                                string path = call.GetParamId(1);
                                mats.Add(AssetDatabase.LoadAssetAtPath<Material>(path));
                            }
                        }
                    }
                    if (mats.Count > 0) {
                        meshRenderer.sharedMaterials = mats.ToArray();
                    }
                }
                var skinnedMeshRenderer = cobj as SkinnedMeshRenderer;
                if (null != skinnedMeshRenderer) {
                    List<Material> mats = new List<Material>();
                    foreach (var statement in funcData.Params) {
                        string id = statement.GetId();
                        if (id == "material") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string matName = call.GetParamId(0);
                                string path = call.GetParamId(1);
                                mats.Add(AssetDatabase.LoadAssetAtPath<Material>(path));
                            }
                        } else if (id == "mesh") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string meshName = call.GetParamId(0);
                                string path = call.GetParamId(1);
                                skinnedMeshRenderer.sharedMesh = LoadAssetByPathAndName(path, meshName) as Mesh;
                            }
                        } else if (id == "rootbone") {
                            var call = statement as Dsl.FunctionData;
                            if (null != call) {
                                string rootBone = call.GetParamId(0);
                                var root = FindRoot(obj);
                                var boneObj = ScriptableFramework.Utility.FindChildObject(root, rootBone);
                                if (null != boneObj) {
                                    skinnedMeshRenderer.rootBone = boneObj.transform;
                                }
                            }
                        }
                    }
                    if (mats.Count > 0) {
                        skinnedMeshRenderer.sharedMaterials = mats.ToArray();
                    }
                }
            }
        } else {
            var callData = comp as Dsl.FunctionData;
            string name = callData.GetParamId(0);
            string fullName = callData.GetParamId(1);
            var type = System.Type.GetType(fullName);
            if (null != type) {
                obj.AddComponent(type);
            }
        }
    }

    private static void ReadAttachPoint(Dsl.FunctionData funcData, Transform trans)
    {
        if (null == funcData)
            return;
        //Skip non-skeletal nodes
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "asset" || id == "component") {
                return;
            }
        }
        var cd = funcData;
        if (funcData.IsHighOrder)
            cd = funcData.LowerOrderFunction;
        string name = cd.GetParamId(0);
        GameObject obj = null;
        var t = trans.Find(name);
        if (null != t) {
            obj = t.gameObject;
        } else {
            obj = new GameObject(name);
            obj.transform.SetParent(trans, false);
        }
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "recttransform") {
                ReadRectTransform(statement, obj);
            } else if (id == "transform") {
                ReadTransform(statement, obj);
            }
        }
        foreach (var statement in funcData.Params) {
            string id = statement.GetId();
            if (id == "object") {
                ReadAttachPoint(statement as Dsl.FunctionData, obj.transform);
            }
        }
    }

    private static void WriteObject(StreamWriter sw, int indent, Transform trans)
    {
        var obj = trans.gameObject;
        sw.WriteLine("{0}object(\"{1}\")", GetIndent(indent), obj.name);
        sw.WriteLine("{0}{{", GetIndent(indent));
        ++indent;
        var rectTrans = trans as RectTransform;
        if (null != rectTrans) {
            sw.WriteLine("{0}recttransform({1:f3}, {2:f3}, {3:f3}, {4:f3}, {5:f3})", GetIndent(indent), rectTrans.anchoredPosition3D.x, rectTrans.anchoredPosition3D.y, rectTrans.anchoredPosition3D.z, rectTrans.sizeDelta.x, rectTrans.sizeDelta.y);
            sw.WriteLine("{0}{{", GetIndent(indent));
            ++indent;
            sw.WriteLine("{0}anchor({1:f3}, {2:f3}, {3:f3}, {4:f3});", GetIndent(indent), rectTrans.anchorMin.x, rectTrans.anchorMin.y, rectTrans.anchorMax.x, rectTrans.anchorMax.y);
            sw.WriteLine("{0}pivot({1:f3}, {2:f3});", GetIndent(indent), rectTrans.pivot.x, rectTrans.pivot.y);
            sw.WriteLine("{0}rotation({1:f3}, {2:f3}, {3:f3});", GetIndent(indent), rectTrans.localEulerAngles.x, rectTrans.localEulerAngles.y, rectTrans.localEulerAngles.z);
            sw.WriteLine("{0}scale({1:f3}, {2:f3}, {3:f3});", GetIndent(indent), rectTrans.localScale.x, rectTrans.localScale.y, rectTrans.localScale.z);
            sw.WriteLine("{0}offset({1:f3}, {2:f3}, {3:f3}, {4:f3});", GetIndent(indent), rectTrans.offsetMin.x, rectTrans.offsetMin.y, rectTrans.offsetMax.x, rectTrans.offsetMax.y);
            --indent;
            sw.WriteLine("{0}}};", GetIndent(indent));

            WriteUiComponents(sw, indent, trans);

            int ct = rectTrans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = rectTrans.GetChild(i);
                WriteObject(sw, indent, t);
            }
        } else {
            sw.WriteLine("{0}transform", GetIndent(indent));
            sw.WriteLine("{0}{{", GetIndent(indent));
            ++indent;
            sw.WriteLine("{0}position({1:f3}, {2:f3}, {3:f3});", GetIndent(indent), trans.localPosition.x, trans.localPosition.y, trans.localPosition.z);
            sw.WriteLine("{0}rotation({1:f3}, {2:f3}, {3:f3});", GetIndent(indent), trans.localEulerAngles.x, trans.localEulerAngles.y, trans.localEulerAngles.z);
            sw.WriteLine("{0}scale({1:f3}, {2:f3}, {3:f3});", GetIndent(indent), trans.localScale.x, trans.localScale.y, trans.localScale.z);
            --indent;
            sw.WriteLine("{0}}};", GetIndent(indent));

            WriteComponents(sw, indent, trans);

            int ct = trans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = trans.GetChild(i);
                WriteObject(sw, indent, t);
            }
        }
        --indent;
        sw.WriteLine("{0}}};", GetIndent(indent));
    }
    private static void WriteUiComponents(StreamWriter sw, int indent, Transform trans)
    {
        var control = trans.gameObject;
        var behaviours = control.GetComponents<UnityEngine.EventSystems.UIBehaviour>();
        for (int i = 0; i < behaviours.Length; ++i) {
            bool handled = false;
            var comp = behaviours[i];
            var img = comp as UnityEngine.UI.Image;
            if (null != img) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                var path = AssetDatabase.GetAssetPath(img.sprite);
                sw.WriteLine("{0}sprite(\"{1}\");", GetIndent(indent), path);
                sw.WriteLine("{0}color({1:f3}, {2:f3}, {3:f3}, {4:f3});", GetIndent(indent), img.color.r, img.color.g, img.color.b, img.color.a);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var text = comp as UnityEngine.UI.Text;
            if (null != text) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                sw.WriteLine("{0}text(\"{1}\");", GetIndent(indent), text.text);
                sw.WriteLine("{0}color({1:f3}, {2:f3}, {3:f3}, {4:f3});", GetIndent(indent), text.color.r, text.color.g, text.color.b, text.color.a);
                sw.WriteLine("{0}font(\"{1}\", \"{2}\", {3}, {4}, \"{5}\");", GetIndent(indent), text.font.name, AssetDatabase.GetAssetPath(text.font), text.fontSize, text.lineSpacing, text.fontStyle);
                sw.WriteLine("{0}align(\"{1}\", {2});", GetIndent(indent), text.alignment, text.alignByGeometry);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var gridLayout = comp as UnityEngine.UI.GridLayoutGroup;
            if (null != gridLayout) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                sw.WriteLine("{0}cellsize({1:f3}, {2:f3});", GetIndent(indent), gridLayout.cellSize.x, gridLayout.cellSize.y);
                sw.WriteLine("{0}spacing({1:f3}, {2:f3});", GetIndent(indent), gridLayout.spacing.x, gridLayout.spacing.y);
                sw.WriteLine("{0}padding({1}, {2}, {3}, {4});", GetIndent(indent), gridLayout.padding.left, gridLayout.padding.top, gridLayout.padding.right, gridLayout.padding.bottom);
                sw.WriteLine("{0}options(\"{1}\", \"{2}\", \"{3}\", \"{4}\");", GetIndent(indent), gridLayout.startCorner, gridLayout.startAxis, gridLayout.childAlignment, gridLayout.constraint);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var horizontalLayout = comp as UnityEngine.UI.HorizontalLayoutGroup;
            if (null != horizontalLayout) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                sw.WriteLine("{0}spacing({1:f3});", GetIndent(indent), horizontalLayout.spacing);
                sw.WriteLine("{0}padding({1}, {2}, {3}, {4});", GetIndent(indent), horizontalLayout.padding.left, horizontalLayout.padding.top, horizontalLayout.padding.right, horizontalLayout.padding.bottom);
                sw.WriteLine("{0}options(\"{1}\", {2}, {3}, {4}, {5});", GetIndent(indent), horizontalLayout.childAlignment, horizontalLayout.childControlWidth, horizontalLayout.childControlHeight, horizontalLayout.childForceExpandWidth, horizontalLayout.childForceExpandHeight);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var verticalLayout = comp as UnityEngine.UI.VerticalLayoutGroup;
            if (null != verticalLayout) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                sw.WriteLine("{0}spacing({1:f3});", GetIndent(indent), verticalLayout.spacing);
                sw.WriteLine("{0}padding({1}, {2}, {3}, {4});", GetIndent(indent), verticalLayout.padding.left, verticalLayout.padding.top, verticalLayout.padding.right, verticalLayout.padding.bottom);
                sw.WriteLine("{0}options(\"{1}\", {2}, {3}, {4}, {5});", GetIndent(indent), verticalLayout.childAlignment, verticalLayout.childControlWidth, verticalLayout.childControlHeight, verticalLayout.childForceExpandWidth, verticalLayout.childForceExpandHeight);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            if (!handled) {
                sw.WriteLine("{0}component(\"{1}\", \"{2}\");", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
            }
        }
    }
    private static void WriteComponents(StreamWriter sw, int indent, Transform trans)
    {
        var control = trans.gameObject;
        var components = control.GetComponents<Component>();
        for (int i = 0; i < components.Length; ++i) {
            bool handled = false;
            var comp = components[i];
            if (null == comp)
                continue;
            var t = comp as Transform;
            if (null != t)
                continue;
            var animator = comp as Animator;
            if (null != animator) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                var path = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
                sw.WriteLine("{0}controller(\"{1}\");", GetIndent(indent), path);
                sw.WriteLine("{0}avatar(\"{1}\");", GetIndent(indent), AssetDatabase.GetAssetPath(animator.avatar));
                sw.WriteLine("{0}options({1}, \"{2}\", \"{3}\");", GetIndent(indent), animator.applyRootMotion, animator.updateMode, animator.cullingMode);
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var meshFilter = comp as MeshFilter;
            if (null != meshFilter) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                if (null != meshFilter.sharedMesh) {
                    sw.WriteLine("{0}mesh(\"{1}\", \"{2}\");", GetIndent(indent), meshFilter.sharedMesh.name, AssetDatabase.GetAssetPath(meshFilter.sharedMesh));
                }
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var meshRenderer = comp as MeshRenderer;
            if (null != meshRenderer) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                foreach (var mat in meshRenderer.sharedMaterials) {
                    if (null == mat)
                        continue;
                    sw.WriteLine("{0}material(\"{1}\", \"{2}\");", GetIndent(indent), mat.name, AssetDatabase.GetAssetPath(mat));
                }
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            var skinnedMeshRenderer = comp as SkinnedMeshRenderer;
            if (null != skinnedMeshRenderer) {
                handled = true;
                sw.WriteLine("{0}component(\"{1}\", \"{2}\")", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
                sw.WriteLine("{0}{{", GetIndent(indent));
                ++indent;
                if (null != skinnedMeshRenderer.sharedMesh) {
                    sw.WriteLine("{0}mesh(\"{1}\", \"{2}\");", GetIndent(indent), skinnedMeshRenderer.sharedMesh.name, AssetDatabase.GetAssetPath(skinnedMeshRenderer.sharedMesh));
                }
                if (null != skinnedMeshRenderer.rootBone) {
                    sw.WriteLine("{0}rootbone(\"{1}\");", GetIndent(indent), skinnedMeshRenderer.rootBone.name);
                }
                foreach (var mat in skinnedMeshRenderer.sharedMaterials) {
                    if (null == mat)
                        continue;
                    sw.WriteLine("{0}material(\"{1}\", \"{2}\");", GetIndent(indent), mat.name, AssetDatabase.GetAssetPath(mat));
                }
                --indent;
                sw.WriteLine("{0}}};", GetIndent(indent));
            }
            if (!handled) {
                sw.WriteLine("{0}component(\"{1}\", \"{2}\");", GetIndent(indent), comp.GetType().Name, comp.GetType().AssemblyQualifiedName);
            }
        }
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
    private static Object LoadAssetByPathAndName(string path, string name)
    {
        var objs = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (var obj in objs) {
            if (obj.name == name)
                return obj;
        }
        return null;
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
