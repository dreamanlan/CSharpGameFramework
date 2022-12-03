using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public sealed class AnimationClipEditWindow : EditorWindow
{
    [MenuItem("工具/AnimationClip切割")]
    internal static void InitWindow()
    {
        AnimationClipEditWindow window = (AnimationClipEditWindow)EditorWindow.GetWindow(typeof(AnimationClipEditWindow));
        window.minSize = new Vector2(640, 480);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("收集")) {
            Collect();
        }
        if (GUILayout.Button("导入")) {
            ImportAll();
        }
        if (GUILayout.Button("导出")) {
            ExportAll();
        }
        GUILayout.Space(120);
        if (GUILayout.Button("导入到选中fbx")) {
            ImportToSelected();
        }
        if (GUILayout.Button("导出选中fbx")) {
            ExportFromSelected();
        }
        EditorGUILayout.EndHorizontal();

        m_Pos = EditorGUILayout.BeginScrollView(m_Pos, true, true);

        ListItem();

        EditorGUILayout.EndScrollView();
    }

    private void Collect()
    {
        string path = EditorUtility.OpenFolderPanel("请选择要收集FBX的根目录", Application.dataPath, string.Empty);
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
            m_FbxList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountFileRecursively(path);
            if (m_TotalSearchCount > 0) {
                SearchFileRecursively(path);
                EditorUtility.ClearProgressBar();
            }
        }
    }
    
    private void ImportAll()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            foreach (ItemInfo item in m_FbxList) {
                if (item.Selected) {
                    var importer = AssetImporter.GetAtPath(item.Path) as ModelImporter;
                    if (null != importer) {
                        AnimationClipUtility.Import(path, Path.GetFileNameWithoutExtension(item.Path), importer);
                        Debug.Log("ImportAll " + item.Path);
                    }
                }
            }
			EditorUtility.DisplayDialog ("提示", "处理完成", "ok");
        }
    }

    private void ExportAll()
    {
        string path = EditorUtility.SaveFilePanel("请选择要导出的dsl文件", string.Empty, string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path)) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
            foreach (ItemInfo item in m_FbxList) {
                if (item.Selected) {
                    var importer = AssetImporter.GetAtPath(item.Path) as ModelImporter;
                    if (null != importer) {
                        AnimationClipUtility.Export(path, Path.GetFileNameWithoutExtension(item.Path), importer);
                        Debug.Log("ExportAll " + item.Path);
                    }
                }
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }

    private void ImportToSelected()
    {
        string path = EditorUtility.OpenFilePanel("请选择要导入的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            var objs = Selection.objects;
            foreach (var obj in objs) {
                var itemPath = AssetDatabase.GetAssetPath(obj);
                var importer = AssetImporter.GetAtPath(itemPath) as ModelImporter;
                if (null != importer) {
                    AnimationClipUtility.Import(path, Path.GetFileNameWithoutExtension(itemPath), importer);
                    Debug.Log("ImportSelected " + itemPath);
                }
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }

    private void ExportFromSelected()
    {
        string path = EditorUtility.SaveFilePanel("请选择要导出的dsl文件", string.Empty, string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path)) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
            var objs = Selection.objects;
            foreach (var obj in objs) {
                var itemPath = AssetDatabase.GetAssetPath(obj);
                var importer = AssetImporter.GetAtPath(itemPath) as ModelImporter;
                if (null != importer) {
                    AnimationClipUtility.Export(path, Path.GetFileNameWithoutExtension(itemPath), importer);
                    Debug.Log("ExportSelected " + itemPath);
                }
            }
            EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }

    private void ListItem()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (ItemInfo item in m_FbxList) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (ItemInfo item in m_FbxList) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Go to page ({0}) : ", m_FbxList.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = GUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        GUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_FbxList.Count / c_ItemsPerPage + 1, m_Page));
        GUILayout.EndVertical();
        int index = 0;
        int totalShown = 0;
        foreach (ItemInfo item in m_FbxList) {
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
            return Path.GetExtension(file).ToLower() == ".fbx";
        }).ToArray();
        foreach (string file in files) {
            m_FbxList.Add(new ItemInfo { Path = PathToAssetPath(file), Selected = false });
            ++m_CurSearchCount;
            EditorUtility.DisplayProgressBar("采集进度", string.Format("{0} in {1}/{2}", m_FbxList.Count, m_CurSearchCount, m_TotalSearchCount), m_CurSearchCount * 1.0f / m_TotalSearchCount);
        }        
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            SearchFileRecursively(subDir);
        }
    }

    private void CountFileRecursively(string dir)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            return Path.GetExtension(file).ToLower() == ".fbx";
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

    private List<ItemInfo> m_FbxList = new List<ItemInfo>();
    private Vector2 m_Pos = Vector2.zero;
    private int m_Page = 1;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;

    private const int c_ItemsPerPage = 50;
}

public static class AnimationClipUtility
{
    public static void Import(string path, string name, ModelImporter importer)
    {
        Dsl.DslFile file = new Dsl.DslFile();
        if (file.Load(path, (string msg) => { Debug.Log(msg); })) {
            Dsl.ISyntaxComponent dslInfo = null;
            Dsl.FunctionData funcData = null;
            if (file.DslInfos.Count == 1) {
                dslInfo = file.DslInfos[0];
                funcData = dslInfo as Dsl.FunctionData;
                var stData = dslInfo as Dsl.StatementData;
                if (null == funcData && null!=stData) {
                    funcData = stData.First.AsFunction;
                }
            } else {
                foreach (var info in file.DslInfos) {
                    if (info.GetId() == "model") {
                        funcData = info as Dsl.FunctionData;
                        var stData = info as Dsl.StatementData;
                        if (null == funcData && null != stData) {
                            funcData = stData.First.AsFunction;
                        }
                        if (null != funcData) {
                            var callData = funcData;
                            if (funcData.IsHighOrder)
                                callData = funcData.LowerOrderFunction;
                            if (callData.GetParamId(0) == name) {
                                dslInfo = info;
                                break;
                            }
                        }
                    }
                }
            }
            if (null != dslInfo) {
                if (dslInfo.GetId() == "model") {
                    List<ModelImporterClipAnimation> list = new List<ModelImporterClipAnimation>();

                    foreach (Dsl.ISyntaxComponent clipInfo in funcData.Params) {
                        var fd = clipInfo as Dsl.FunctionData;
                        if (null != fd) {
                            var cd = fd;
                            if (fd.IsHighOrder)
                                cd = fd.LowerOrderFunction;
                            string id = cd.GetId();
                            if (id == "setting") {
                                foreach (var settingInfo in fd.Params) {
                                    var setting = settingInfo as Dsl.FunctionData;
                                    if (null != setting) {
                                        string key = setting.GetId();
                                        if (key == "import_animation") {
                                            importer.importAnimation = bool.Parse(setting.GetParamId(0));
                                        } else if (key == "resample_curves") {
                                            importer.resampleCurves = bool.Parse(setting.GetParamId(0));
                                        } else if (key == "animation_compression") {
                                            importer.animationCompression = GameFramework.Converter.ConvertStrToEnum<UnityEditor.ModelImporterAnimationCompression>(setting.GetParamId(0));
                                        } else if (key == "rotation_error") {
                                            importer.animationRotationError = float.Parse(setting.GetParamId(0));
                                        } else if (key == "position_error") {
                                            importer.animationPositionError = float.Parse(setting.GetParamId(0));
                                        } else if (key == "scale_error") {
                                            importer.animationScaleError = float.Parse(setting.GetParamId(0));
                                        }
                                    }
                                }
                            } else if (id == "clip_animation") {
                                ModelImporterClipAnimation clip = new ModelImporterClipAnimation();
                                clip.name = cd.GetParamId(0);
                                foreach (var settingInfo in fd.Params) {
                                    var setting = settingInfo as Dsl.FunctionData;
                                    if (null != setting) {
                                        string key = setting.GetId();
                                        if (key == "take_name") {
                                            clip.takeName = setting.GetParamId(0);
                                        } else if (key == "first_frame") {
                                            clip.firstFrame = float.Parse(setting.GetParamId(0));
                                        } else if (key == "last_frame") {
                                            clip.lastFrame = float.Parse(setting.GetParamId(0));
                                        } else if (key == "loop_time") {
                                            clip.loopTime = bool.Parse(setting.GetParamId(0));
                                        } else if (key == "loop_pose") {
                                            clip.loopPose = bool.Parse(setting.GetParamId(0));
                                        } else if (key == "cycle_offset") {
                                            clip.cycleOffset = float.Parse(setting.GetParamId(0));
                                        } else if (key == "additive_reference_pose") {
                                            clip.hasAdditiveReferencePose = bool.Parse(setting.GetParamId(0));
                                        } else if (key == "additive_reference_pose_frame") {
                                            clip.additiveReferencePoseFrame = float.Parse(setting.GetParamId(0));
                                        }
                                    }
                                }
                                list.Add(clip);
                            }
                        }
                    }

                    importer.clipAnimations = list.ToArray();
                    importer.SaveAndReimport();
                }
            } else {
                EditorUtility.DisplayDialog("提示", "找不到对应名字的数据，如果需要不同名导入，导入数据里只能包含一个model", "ok");
            }
        }
    }
    public static void Export(string path, string name, ModelImporter importer)
    {
        int indent = 0;
        using (StreamWriter sw = new StreamWriter(path, true)) {
            sw.WriteLine("{0}model(\"{1}\")", GetIndent(indent), name);
            sw.WriteLine("{0}{1}", GetIndent(indent), "{");
            ++indent;
            {
                sw.WriteLine("{0}setting", GetIndent(indent));
                sw.WriteLine("{0}{1}", GetIndent(indent), "{");
                ++indent;
                sw.WriteLine("{0}import_animation({1});", GetIndent(indent), importer.importAnimation);
                sw.WriteLine("{0}resample_curves({1});", GetIndent(indent), importer.resampleCurves);
                sw.WriteLine("{0}animation_compression(\"{1}\");", GetIndent(indent), GameFramework.Converter.ConvertEnumToStr<UnityEditor.ModelImporterAnimationCompression>(importer.animationCompression));
                sw.WriteLine("{0}rotation_error({1});", GetIndent(indent), importer.animationRotationError);
                sw.WriteLine("{0}position_error({1});", GetIndent(indent), importer.animationPositionError);
                sw.WriteLine("{0}scale_error({1});", GetIndent(indent), importer.animationScaleError);
                --indent;
                sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            }
            foreach (var takeInfo in importer.importedTakeInfos) {
                sw.WriteLine("{0}imported_take_info(\"{1}\")", GetIndent(indent), takeInfo.name);
                sw.WriteLine("{0}{1}", GetIndent(indent), "{");
                ++indent;
                sw.WriteLine("{0}default_clip_name(\"{1}\");", GetIndent(indent), takeInfo.defaultClipName);
                sw.WriteLine("{0}sample_rate({1});", GetIndent(indent), takeInfo.sampleRate);
                sw.WriteLine("{0}start_time({1});", GetIndent(indent), takeInfo.startTime);
                sw.WriteLine("{0}stop_time({1});", GetIndent(indent), takeInfo.stopTime);
                sw.WriteLine("{0}bake_start_time({1});", GetIndent(indent), takeInfo.bakeStartTime);
                sw.WriteLine("{0}bake_stop_time({1});", GetIndent(indent), takeInfo.bakeStopTime);
                --indent;
                sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            }
            foreach (var clip in importer.defaultClipAnimations) {
                sw.WriteLine("{0}default_clip_animation(\"{1}\")", GetIndent(indent), clip.name);
                sw.WriteLine("{0}{1}", GetIndent(indent), "{");
                ++indent;
                sw.WriteLine("{0}take_name(\"{1}\");", GetIndent(indent), clip.takeName);
                sw.WriteLine("{0}first_frame({1});", GetIndent(indent), clip.firstFrame);
                sw.WriteLine("{0}last_frame({1});", GetIndent(indent), clip.lastFrame);
                sw.WriteLine("{0}loop_time({1});", GetIndent(indent), clip.loopTime);
                sw.WriteLine("{0}loop_pose({1});", GetIndent(indent), clip.loopPose);
                sw.WriteLine("{0}cycle_offset({1});", GetIndent(indent), clip.cycleOffset);
                sw.WriteLine("{0}additive_reference_pose({1});", GetIndent(indent), clip.hasAdditiveReferencePose);
                sw.WriteLine("{0}additive_reference_pose_frame({1});", GetIndent(indent), clip.additiveReferencePoseFrame);
                var takeInfo = FindTakeInfo(importer, clip.takeName);
                float sampleRate = 30.0f;
                if (null != takeInfo.name) {
                    sampleRate = takeInfo.sampleRate;
                }
                sw.WriteLine("{0}length({1});", GetIndent(indent), (clip.lastFrame - clip.firstFrame) / sampleRate);
                --indent;
                sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            }
            foreach (var clip in importer.clipAnimations) {
                sw.WriteLine("{0}clip_animation(\"{1}\")", GetIndent(indent), clip.name);
                sw.WriteLine("{0}{1}", GetIndent(indent), "{");
                ++indent;
                sw.WriteLine("{0}take_name(\"{1}\");", GetIndent(indent), clip.takeName);
                sw.WriteLine("{0}first_frame({1});", GetIndent(indent), clip.firstFrame);
                sw.WriteLine("{0}last_frame({1});", GetIndent(indent), clip.lastFrame);
                sw.WriteLine("{0}loop_time({1});", GetIndent(indent), clip.loopTime);
                sw.WriteLine("{0}loop_pose({1});", GetIndent(indent), clip.loopPose);
                sw.WriteLine("{0}cycle_offset({1});", GetIndent(indent), clip.cycleOffset);
                sw.WriteLine("{0}additive_reference_pose({1});", GetIndent(indent), clip.hasAdditiveReferencePose);
                sw.WriteLine("{0}additive_reference_pose_frame({1});", GetIndent(indent), clip.additiveReferencePoseFrame);
                var takeInfo = FindTakeInfo(importer, clip.takeName);
                float sampleRate = 30.0f;
                if(null!=takeInfo.name){
                    sampleRate = takeInfo.sampleRate;
                }
                sw.WriteLine("{0}length({1});", GetIndent(indent), (clip.lastFrame - clip.firstFrame) / sampleRate);
                --indent;
                sw.WriteLine("{0}{1}", GetIndent(indent), "};");
            }
            --indent;
            sw.WriteLine("{0}{1}", GetIndent(indent), "};");
        }
    }

    private static TakeInfo FindTakeInfo(ModelImporter importer, string name)
    {
        foreach (var info in importer.importedTakeInfos) {
            if (info.name == name)
                return info;
        }
        return new TakeInfo();
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
