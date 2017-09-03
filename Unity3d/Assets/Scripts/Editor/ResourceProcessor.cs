using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.MemoryProfiler;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;
using GameFramework;

public sealed class ResourceEditWindow : EditorWindow
{
    [MenuItem("工具/资源处理")]
    internal static void InitWindow()
    {
        ResourceEditWindow window = (ResourceEditWindow)EditorWindow.GetWindow(typeof(ResourceEditWindow));
        window.Init();
        window.Show();
        EditorUtility.ClearProgressBar();
    }

    private void Init()
    {
        if (!m_SnapshotRegistered) {
            MemorySnapshot.OnSnapshotReceived += IncomingSnapshot;
            m_SnapshotRegistered = true;
        }
    }

    private void OnGUI()
    {
        bool oldRichText = GUI.skin.button.richText;
        GUI.skin.button.richText = true;
        string[] textColors;
        if (EditorGUIUtility.isProSkin) {
            textColors = new string[] { "orange", "fuchsia", "fuchsia", "lime", "lightblue" };
        } else {
            textColors = new string[] { "green", "fuchsia", "fuchsia", "maroon", "darkblue" };
        }
        bool skipToNextFrame = false;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("资源依赖:", GUILayout.Width(60));
        if (GUILayout.Button(string.Format("<color={0}>分析</color>", textColors[0]))) {
            AnalyseAssets();
        }
        if (GUILayout.Button(string.Format("<color={0}>保存</color>", textColors[0]))) {
            SaveDependencies();
        }
        if (GUILayout.Button(string.Format("<color={0}>加载</color>", textColors[0]))) {
            LoadDependencies();
        }
        EditorGUILayout.LabelField("内存:", GUILayout.Width(40));
        if (GUILayout.Button(string.Format("<color={0}>捕获</color>", textColors[1]))) {
            if (ProfilerDriver.connectedProfiler != -1) {
                string id = ProfilerDriver.GetConnectionIdentifier(ProfilerDriver.connectedProfiler);
                m_ActiveProfilerIsEditor = id == "Editor";
            } else {
                m_ActiveProfilerIsEditor = true;
            }
            MemorySnapshot.RequestNewSnapshot();
        }
        if (GUILayout.Button(string.Format("<color={0}>保存</color>", textColors[1]))) {
            SaveMemoryInfo();
        }
        if (GUILayout.Button(string.Format("<color={0}>加载</color>", textColors[1]))) {
            LoadMemoryInfo();
        }
        EditorGUILayout.LabelField("耗时:", GUILayout.Width(40));
        if (GUILayout.Button(string.Format("<color={0}>清空</color>", textColors[2]))) {
            ClearInstrumentInfo();
        }
        if (m_Record) {
            if (GUILayout.Button(string.Format("<color={0}>停止</color>", textColors[2]))) {
                m_Record = false;
            }
        } else {
            if (GUILayout.Button(string.Format("<color={0}>记录</color>", textColors[2]))) {
                m_Record = true;
            }
        }
        if (GUILayout.Button(string.Format("<color={0}>保存</color>", textColors[2]))) {
            SaveInstrumentInfo();
        }
        if (GUILayout.Button(string.Format("<color={0}>加载</color>", textColors[2]))) {
            LoadInstrumentInfo();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(string.Format("<color={0}>选择处理脚本</color>", textColors[3]))) {
            SelectDsl();
        }
        if (GUILayout.Button(string.Format("<color={0}>收集资源</color>", textColors[3]))) {
            Collect();
        }
        if (GUILayout.Button(string.Format("<color={0}>处理选中资源</color>", textColors[3]))) {
            Process();
        }
        if (GUILayout.Button(string.Format("<color={0}>同步选择u3d资源或场景</color>", textColors[4]))) {
            SelectAssetsOrObjects();
        }
        if (GUILayout.Button(string.Format("<color={0}>生成资源后处理代码</color>", textColors[4]))) {
            Generate();
        }
        if (GUILayout.Button(string.Format("<color={0}>生成场景</color>", textColors[4]))) {
            GenerateScene();
            skipToNextFrame = true;
        }
        EditorGUILayout.EndHorizontal();

        if (m_NeedAnalyseSnapshot) {
            m_NeedAnalyseSnapshot = false;
            AnalyseSnapshot();
            CalcTotalValue();
            skipToNextFrame = true;
        }
        if (m_Record) {
            int firstIndex = ProfilerDriver.firstFrameIndex;
            int lastIndex = ProfilerDriver.lastFrameIndex;

            if (lastIndex >= firstIndex && lastIndex >= 0) {
                float[] batches = new float[lastIndex - firstIndex + 1];
                float[] triangles = new float[lastIndex - firstIndex + 1];
                var labels = ProfilerDriver.GetGraphStatisticsPropertiesForArea(ProfilerArea.Rendering);
                foreach (string l in labels) {
                    var id = ProfilerDriver.GetStatisticsIdentifier(l);
                    var lowerLabel = l.ToLower();
                    if (lowerLabel == "batches") {
                        float maxVal;
                        ProfilerDriver.GetStatisticsValues(id, firstIndex, 1.0f, batches, out maxVal);
                    } else if (lowerLabel == "triangles") {
                        float maxVal;
                        ProfilerDriver.GetStatisticsValues(id, firstIndex, 1.0f, triangles, out maxVal);
                    }
                }

                for (int index = firstIndex > m_LastFrame ? firstIndex : m_LastFrame + 1; index <= lastIndex; ++index) {
                    int ix = index - firstIndex;
                    float triangle = 0;
                    if (ix >= 0 && ix < triangles.Length) {
                        triangle = triangles[ix];
                    }
                    float batch = 0;
                    if (ix >= 0 && ix < batches.Length) {
                        batch = batches[ix];
                    }
                    if (RecordInstrument(m_RecordIndex, index, ProfilerColumn.FunctionName, ProfilerViewType.Hierarchy, triangle, batch)) {
                        ++m_RecordIndex;
                    }
                }
                m_LastFrame = lastIndex;

                if (lastIndex >= 0) {
                    ProfilerProperty prop = new ProfilerProperty();
                    prop.SetRoot(lastIndex, ProfilerColumn.FunctionName, ProfilerViewType.Hierarchy);
                    prop.onlyShowGPUSamples = false;

                    int ix = lastIndex - firstIndex;
                    float triangle = 0;
                    if (ix >= 0 && ix < triangles.Length) {
                        triangle = triangles[ix];
                    }
                    float batch = 0;
                    if (ix >= 0 && ix < batches.Length) {
                        batch = batches[ix];
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("depth:{0}\tfps:{1}\tcpu time:{2}\tgpu time:{3} \triangles:{4} \tbatches:{5}", prop.depth, prop.frameFPS, prop.frameTime, prop.frameGpuTime, triangle, batch);
                    sb.AppendLine();
                    while (prop.Next(true)) {
                        sb.AppendFormat("{0}:{1}->{2}\t{3}\t{4}\t{5}\t{6}", prop.depth, prop.propertyName, prop.propertyPath, prop.GetColumn(ProfilerColumn.Calls), prop.GetColumn(ProfilerColumn.GCMemory), prop.GetColumn(ProfilerColumn.SelfPercent), prop.GetColumn(ProfilerColumn.SelfTime));
                        sb.AppendLine();
                    }
                    m_Text = sb.ToString();
                }
            }
        }
        if (skipToNextFrame)
            return;

        if (m_ParamNames.Count > 0) {
            foreach (var name in m_ParamNames) {
                ResourceEditUtility.ParamInfo info;
                if (m_Params.TryGetValue(name, out info)) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(info.Name, GUILayout.Width(160));
                    string oldVal = info.Value.ToString();
                    string newVal = oldVal;
                    if (info.Options.Count > 0) {
                        if (info.OptionStyle == "toggle") {
                            bool changed = false;
                            foreach (var key in info.OptionNames) {
                                string val;
                                if (info.Options.TryGetValue(key, out val)) {
                                    if (changed) {
                                        EditorGUILayout.Toggle(key, false);
                                    } else {
                                        bool toggle = val == oldVal;
                                        if (EditorGUILayout.Toggle(key, toggle)) {
                                            if (!toggle) {
                                                changed = true;
                                                newVal = val;
                                            }
                                        }
                                    }
                                }
                            }
                        } else if (info.OptionStyle == "multiple") {
                            var oldVals = oldVal.Split('|');
                            List<string> newVals = new List<string>();
                            foreach (var key in info.OptionNames) {
                                string val;
                                if (info.Options.TryGetValue(key, out val)) {
                                    bool toggle = Array.IndexOf(oldVals, val) >= 0;
                                    if (EditorGUILayout.Toggle(key, toggle)) {
                                        newVals.Add(val);
                                    }
                                }
                            }
                            newVal = string.Join("|", newVals.ToArray());
                        } else {
                            int ix = 0;
                            string[] keys = info.OptionNames.ToArray();
                            foreach (var key in info.OptionNames) {
                                string val;
                                if (info.Options.TryGetValue(key, out val)) {
                                    if (val == oldVal) {
                                        break;
                                    }
                                }
                                ++ix;
                            }
                            if (ix >= info.Options.Count)
                                ix = 0;
                            int newIx = ix;
                            newIx = EditorGUILayout.Popup(ix, keys);
                            if (newIx != ix) {
                                newVal = info.Options[keys[newIx]];
                            }
                        }
                    } else if (info.Type == typeof(bool)) {
                        bool v = EditorGUILayout.Toggle((bool)info.Value);
                        newVal = v ? "true" : "false";
                    } else if (info.Type == typeof(int)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            int min = (int)info.MinValue;
                            int max = (int)info.MaxValue;
                            int v = EditorGUILayout.IntSlider((int)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        } else {
                            int v = EditorGUILayout.IntField((int)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    } else if (info.Type == typeof(float)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            float min = (float)info.MinValue;
                            float max = (float)info.MaxValue;
                            float v = EditorGUILayout.Slider((float)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        } else {
                            float v = EditorGUILayout.FloatField((float)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    } else {
                        newVal = EditorGUILayout.TextField(oldVal, GUILayout.MaxWidth(1024));
                    }
                    EditorGUILayout.EndHorizontal();
                    if (newVal != oldVal) {
                        m_EditedParams.Add(name, newVal);
                    }
                }
            }
            if (m_EditedParams.Count > 0) {
                foreach (var pair in m_EditedParams) {
                    ResourceEditUtility.ParamInfo val;
                    if (m_Params.TryGetValue(pair.Key, out val)) {
                        if (val.Type == typeof(int)) {
                            int v = int.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                int min = (int)val.MinValue;
                                int max = (int)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        } else if (val.Type == typeof(float)) {
                            float v = float.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                float min = (float)val.MinValue;
                                float max = (float)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        } else if (val.Type == typeof(string)) {
                            string v = pair.Value;
                            if (null != val.MinValue && null != val.MaxValue) {
                                string min = (string)val.MinValue;
                                string max = (string)val.MaxValue;
                                if (v.CompareTo(min) < 0) v = min;
                                if (v.CompareTo(min) > 0) v = max;
                            }
                            val.Value = v;
                        } else if (val.Type == typeof(bool)) {
                            bool v = bool.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                bool min = (bool)val.MinValue;
                                bool max = (bool)val.MaxValue;
                                if (v.CompareTo(min) < 0) v = min;
                                if (v.CompareTo(min) > 0) v = max;
                            }
                            val.Value = v;
                        }
                    }
                }
                m_EditedParams.Clear();
            }
        }
        if (m_ItemList.Count <= 0 && string.IsNullOrEmpty(m_CollectPath)) {
            if (!string.IsNullOrEmpty(m_Text)) {
                m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true);
                EditorGUILayout.TextArea(m_Text);
                EditorGUILayout.EndScrollView();
            }
        } else {
            if (!string.IsNullOrEmpty(m_Text)) {
                EditorGUILayout.TextArea(m_Text, GUILayout.MaxHeight(70));
            }
            ListItem();
        }
        GUI.skin.button.richText = oldRichText;
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
                    m_SearchSource = string.Empty;
                    m_PostProcessClass = string.Empty;
                    m_PostProcessMethod = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_ParamNames.Clear();
                    m_Params.Clear();

                    foreach (var info in m_DslFile.DslInfos) {
                        var first = info.First;
                        var input = first.Call;
                        foreach (var param in input.Params) {
                            string ext = param.GetId();
                            if (ext.Length > 0 && ext[0] == '*') {
                                ext = ext.Substring(1);
                            }
                            m_TypeOrExtList.Add(ext);
                        }
                        foreach (var comp in first.Statements) {
                            var callData = comp as Dsl.CallData;
                            if (null != callData) {
                                ParseCallData(callData);
                            } else {
                                var funcData = comp as Dsl.FunctionData;
                                ParseFunctionData(funcData);
                            }
                        }
                    }
                    m_Text = File.ReadAllText(path);
                } else {
                    m_DslFile = null;
                    m_SearchSource = string.Empty;
                    m_PostProcessClass = string.Empty;
                    m_PostProcessMethod = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_ParamNames.Clear();
                    m_Params.Clear();
                    m_Text = null;
                    m_NextFilterIndex = 0;
                    m_NextProcessIndex = 0;
                    m_FilterCalculator = null;
                    m_ProcessCalculator = null;
                }
            } else {
                m_DslFile = null;
                m_SearchSource = string.Empty;
                m_PostProcessClass = string.Empty;
                m_PostProcessMethod = string.Empty;
                m_TypeOrExtList.Clear();
                m_TypeList.Clear();
                m_ParamNames.Clear();
                m_Params.Clear();
                m_Text = null;
                m_NextFilterIndex = 0;
                m_NextProcessIndex = 0;
                m_FilterCalculator = null;
                m_ProcessCalculator = null;
            }
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
        }
    }
    private string ParseCallData(Dsl.CallData callData)
    {
        string id = callData.GetId();
        string key = callData.GetParamId(0);
        string val = callData.GetParamId(1);
        if (id == "int") {
            //int(name, val);
            int v = int.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(int), Value = v };
            m_ParamNames.Add(key);
        } else if (id == "float") {
            //float(name, val);
            float v = float.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(float), Value = v };
            m_ParamNames.Add(key);
        } else if (id == "string") {
            //string(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(string), Value = v };
            m_ParamNames.Add(key);
        } else if (id == "bool") {
            //bool(name, val);
            bool v = bool.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(bool), Value = v };
            m_ParamNames.Add(key);
        } else if (id == "feature") {
            if (key == "source") {
                //feature("source", "sceneobjects" or "sceneassets" or "allassets" or "unusedassets");
                m_SearchSource = val;
            } else if (key == "postprocessclass") {
                //feature("postprocessclass", "PostProcessDataOfIos|PostProcessDataOfAndroid");
                m_PostProcessClass = val;
            } else if (key == "postprocessmethod") {
                //feature("postprocessmethod", "GetAnimaticFbxSet|GetUnanimaticFbxSet|GetTextureSet");
                m_PostProcessMethod = val;
            }
        }
        return key;
    }
    private void ParseFunctionData(Dsl.FunctionData funcData)
    {
        var callData = funcData.Call;
        string name = ParseCallData(callData);
        ResourceEditUtility.ParamInfo info;
        if (m_Params.TryGetValue(name, out info)) {
            foreach (var comp in funcData.Statements) {
                var id = comp.GetId();
                var cd = comp as Dsl.CallData;
                if (null != cd) {
                    if (id == "range") {
                        int num = cd.GetParamNum();
                        var p1 = cd.GetParam(0);
                        var p2 = cd.GetParam(1);
                        var pvd1 = p1 as Dsl.ValueData;
                        var pvd2 = p2 as Dsl.ValueData;
                        if (null != pvd1 && null != pvd2) {
                            //range(min,max);
                            string min = pvd1.GetId();
                            string max = pvd2.GetId();
                            if (info.Type == typeof(int)) {
                                info.MinValue = int.Parse(min);
                                info.MaxValue = int.Parse(max);
                            } else if (info.Type == typeof(float)) {
                                info.MinValue = float.Parse(min);
                                info.MaxValue = float.Parse(max);
                            } else if (info.Type == typeof(string)) {
                                info.MinValue = min;
                                info.MaxValue = max;
                            } else if (info.Type == typeof(bool)) {
                                info.MinValue = bool.Parse(min);
                                info.MaxValue = bool.Parse(max);
                            }
                        }
                    } else if (id == "popup" || id == "toggle" || id == "multiple") {
                        if (string.IsNullOrEmpty(info.OptionStyle)) {
                            info.OptionStyle = id;
                        } else if (info.OptionStyle != id) {
                            EditorUtility.DisplayDialog("错误", string.Format("param's option must use same style, {0} will use {1} style (dont use {2}) !", info.Name, info.OptionStyle, id), "ok");
                        }

                        int num = cd.GetParamNum();
                        var p1 = cd.GetParam(0);
                        var p2 = cd.GetParam(1);
                        var pvd1 = p1 as Dsl.ValueData;
                        var pvd2 = p2 as Dsl.ValueData;
                        if (null != pvd1 && null != pvd2) {
                            //option(key,val);
                            string key = pvd1.GetId();
                            string val = pvd2.GetId();
                            info.Options[key] = val;
                            info.OptionNames.Add(key);
                        } else {
                            var pcd1 = p1 as Dsl.CallData;
                            var pcd2 = p2 as Dsl.CallData;
                            if (1 == num && null != pcd1) {
                                if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_OPERATOR && pcd1.GetId() == "..") {
                                    //option(min..max);
                                    int min = int.Parse(pcd1.GetParamId(0));
                                    int max = int.Parse(pcd1.GetParamId(1));
                                    for (int v = min; v <= max; ++v) {
                                        string kStr = v.ToString("D3");
                                        string vStr = v.ToString();
                                        info.Options[kStr] = vStr;
                                        info.OptionNames.Add(kStr);
                                    }
                                } else if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd1.HaveId()) {
                                    //option([v1,v2,v3,v4,v5]);
                                    for (int i = 0; i < pcd1.GetParamNum(); ++i) {
                                        var vStr = pcd1.GetParamId(i);
                                        info.Options[vStr] = vStr;
                                        info.OptionNames.Add(vStr);
                                    }
                                }
                            } else if (2 == num) {
                                if (null != pcd1 && null != pcd2) {
                                    if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd1.HaveId() && pcd2.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd2.HaveId()) {
                                        //option([k1,k2,k3,k4,k5],[v1,v2,v3,v4,v5]);
                                        int num1 = pcd1.GetParamNum();
                                        int num2 = pcd2.GetParamNum();
                                        int n = num1 <= num2 ? num1 : num2;
                                        for (int i = 0; i < n; ++i) {
                                            var kStr = pcd1.GetParamId(i);
                                            var vStr = pcd2.GetParamId(i);
                                            info.Options[kStr] = vStr;
                                            info.OptionNames.Add(kStr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                } else {
                    var vd = comp as Dsl.ValueData;
                    if (null != vd) {
                        //用于支持标签属性
                    }
                }
            }
        }
    }

    private void Collect()
    {
        m_CollectPath = string.Empty;
        Refresh();
    }

    private void Refresh()
    {
        if (m_SearchSource == "sceneobjects") {
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = CountSceneObjects();
            if (m_TotalSearchCount > 0) {
                SearchSceneObjects();
                EditorUtility.ClearProgressBar();
            }
        } else if (m_SearchSource == "sceneareas") {
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountSceneAreas();
            if (m_TotalSearchCount > 0) {
                SearchSceneAreas();
                EditorUtility.ClearProgressBar();
            }
        } else if (m_SearchSource == "sceneassets") {
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountSceneAssets();
            if (m_TotalSearchCount > 0) {
                SearchSceneAssets();
                EditorUtility.ClearProgressBar();
            }
        } else if (m_SearchSource == "allassets") {
            if (m_ReferenceAssets.Count <= 0 && m_ReferenceByAssets.Count <= 0 && m_UnusedAssets.Count <= 0) {
                EditorUtility.DisplayDialog("错误", "未找到资源依赖信息，请先执行资源依赖‘分析’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountAllAssets();
            if (m_TotalSearchCount > 0) {
                SearchAllAssets();
                EditorUtility.ClearProgressBar();
            }
        } else if (m_SearchSource == "unusedassets") {
            if (m_ReferenceAssets.Count <= 0 && m_ReferenceByAssets.Count <= 0 && m_UnusedAssets.Count <= 0) {
                EditorUtility.DisplayDialog("错误", "未找到资源依赖信息，请先执行资源依赖‘分析’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountUnusedAssets();
            if (m_TotalSearchCount > 0) {
                SearchUnusedAssets();
                EditorUtility.ClearProgressBar();
            }
        } else if (m_SearchSource == "snapshot") {
            if (m_ClassifiedMemoryInfos.Count <= 0) {
                EditorUtility.DisplayDialog("错误", "未找到内存对象信息，请先执行内存‘捕获’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            SearchSnapshot();
        } else if (m_SearchSource == "instruments") {
            if (m_InstrumentInfos.Count <= 0) {
                EditorUtility.DisplayDialog("错误", "未找到耗时信息，请先执行耗时‘记录’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_Page = 1;
            m_SelectedAssetPath = string.Empty;
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            SearchInstruments();
        } else {
            if (string.IsNullOrEmpty(m_CollectPath)) {
                string path = EditorUtility.OpenFolderPanel("请选择要收集资源的根目录", Application.dataPath, string.Empty);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
                    m_CollectPath = path;
                }
            }
            if (!string.IsNullOrEmpty(m_CollectPath)) {
                m_ItemList.Clear();
                m_Page = 1;
                m_SelectedAssetPath = string.Empty;
                m_CurSearchCount = 0;
                m_TotalSearchCount = 0;
                CountFiles(m_CollectPath);
                if (m_TotalSearchCount > 0) {
                    SearchFiles(m_CollectPath);
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        CalcTotalValue();
    }

    private void CalcTotalValue()
    {
        m_TotalItemValue = 0;
        foreach (var item in m_ItemList) {
            m_TotalItemValue += item.Value;
        }
    }

    private void Process()
    {
        if (null == m_DslFile) {
            EditorUtility.DisplayDialog("错误", "请先选择dsl !", "ok");
            return;
        }
        if (null == m_ProcessCalculator || m_NextProcessIndex <= 0) {
            EditorUtility.DisplayDialog("错误", "当前dsl没有配置process !\n【input(exts){...}filter{...}process{...};】", "ok");
            return;
        }
        int totalSelectedCount = 0;
        int index = 0;
        foreach (var item in m_ItemList) {
            if (item.Selected) {
                ++totalSelectedCount;
            }
        }
        foreach (var item in m_ItemList) {
            if (item.Selected) {
                ResourceEditUtility.Process(false, item, m_ProcessCalculator, m_NextProcessIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
                ++index;
                DisplayProgressBar("处理进度", index, totalSelectedCount);
            }
        }
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("提示", "处理完成", "ok");
    }

    private void SelectAssetsOrObjects()
    {
        var list = new List<UnityEngine.Object>();
        foreach (var item in m_ItemList) {
            if (item.Selected) {
                if (m_SearchSource == "sceneobjects" || m_SearchSource == "sceneareas") {
                    if (null != item.Object)
                        list.Add(item.Object);
                } else {
                    if (null != item.Object) {
                        list.Add(item.Object);
                    } else {
                        var obj = AssetDatabase.LoadMainAssetAtPath(item.AssetPath);
                        if (null != obj) {
                            list.Add(obj);
                        }
                    }
                }
            }
        }
        Selection.objects = list.ToArray();
    }

    private void Generate()
    {
        ResourceEditUtility.ParamInfo paramInfo;
        if (string.IsNullOrEmpty(m_PostProcessClass) || string.IsNullOrEmpty(m_PostProcessMethod) || !m_Params.TryGetValue("postprocessindex", out paramInfo)) {
            EditorUtility.DisplayDialog("错误", "当前dsl没有配置后处理类、方法名或索引！\n【feature(\"postprocessclass\",\"PostProcessDataOfIos|PostProcessDataOfAndroid\"); feature(\"postprocessmethod\",\"GetAnimaticFbxSet|GetUnanimaticFbxSet|GetTextureSet\"); and int(\"postprocessindex\", 1..16);】", "ok");
            return;
        }
        string path = AssetPathToPath("Assets/Editor/PostProcess/");
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
                ResourceEditUtility.ParamInfo v;
                if (m_Params.TryGetValue("maxSize", out v) && v.Type == typeof(int)) {
                    sw.WriteLine("\t\tmaxSize = {0};", (int)v.Value);
                }
                foreach (var item in m_ItemList) {
                    if (item.Selected && !string.IsNullOrEmpty(item.AssetPath)) {
                        sw.WriteLine("\t\tlist.Add(\"{0}\");", item.AssetPath);
                    }
                }
                sw.WriteLine("\t}");
            } else {
                sw.WriteLine("\tstatic partial void {0}(HashSet<string> list)", m_PostProcessMethod);
                sw.WriteLine("\t{");
                foreach (var item in m_ItemList) {
                    if (item.Selected && !string.IsNullOrEmpty(item.AssetPath)) {
                        sw.WriteLine("\t\tlist.Add(\"{0}\");", item.AssetPath);
                    }
                }
                sw.WriteLine("\t}");
            }
            sw.WriteLine("}");
            sw.Close();
        }
    }

    private void GenerateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        int size = 10;
        int cx, cy;
        var terrain = CreateTerrain(size, out cx, out cy);
        int ix = 1;
        int iy = 1;
        foreach (var item in m_ItemList) {
            if (item.Selected) {
                var obj = AssetDatabase.LoadMainAssetAtPath(item.AssetPath) as GameObject;
                if (null != obj) {
                    var newObj = PrefabUtility.InstantiatePrefab(obj, scene) as GameObject;
                    if (null != newObj) {
                        newObj.transform.position = new Vector3(ix * size, 0, iy * size);
                        ++ix;
                        if (ix >= cy) {
                            ix = 1;
                            ++iy;
                        }
                    }
                }
            }
        }
    }

    private void SaveDependencies()
    {
        string path = EditorUtility.SaveFilePanel("请指定要保存依赖信息的文件", string.Empty, "dependencies", "txt");
        if (!string.IsNullOrEmpty(path)) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine("asset1\tasset2");
                int curCount = 0;
                int totalCount = 0;
                foreach (var pair in m_ReferenceAssets) {
                    totalCount += pair.Value.Count;
                }
                totalCount += m_UnusedAssets.Count;
                foreach (var pair in m_ReferenceAssets) {
                    var asset1 = pair.Key;
                    foreach (var asset2 in pair.Value) {
                        sw.WriteLine("{0}\t{1}", asset1, asset2);
                        ++curCount;
                        DisplayProgressBar("保存进度", curCount, totalCount);
                    }
                }
                foreach (var asset in m_UnusedAssets) {
                    sw.WriteLine("unused_asset_tag\t{0}", asset);
                    ++curCount;
                    DisplayProgressBar("保存进度", curCount, totalCount);
                }
                sw.Close();
                EditorUtility.ClearProgressBar();
            }
        }
    }
    private void LoadDependencies()
    {
        string path = EditorUtility.OpenFilePanel("请指定要加载依赖信息的文件", string.Empty, "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            int i = 0;
            try {
                var txt = File.ReadAllText(path);
                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                m_ReferenceAssets.Clear();
                m_ReferenceByAssets.Clear();
                m_UnusedAssets.Clear();
                int curCount = 1;
                int totalCount = lines.Length;
                for (i = 1; i < lines.Length; ++i) {
                    var fields = lines[i].Split('\t');
                    var one = fields[0];
                    var two = fields[1];

                    if (one == "unused_asset_tag") {
                        m_UnusedAssets.Add(two);
                    } else {
                        HashSet<string> refSet;
                        if (!m_ReferenceAssets.TryGetValue(one, out refSet)) {
                            refSet = new HashSet<string>();
                            m_ReferenceAssets.Add(one, refSet);
                        }
                        if (!refSet.Contains(two))
                            refSet.Add(two);
                        HashSet<string> refBySet;
                        if (!m_ReferenceByAssets.TryGetValue(two, out refBySet)) {
                            refBySet = new HashSet<string>();
                            m_ReferenceByAssets.Add(two, refBySet);
                        }
                        if (!refBySet.Contains(one))
                            refBySet.Add(one);
                    }

                    ++curCount;
                    DisplayProgressBar("加载进度", curCount, totalCount);
                }
            } catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }

    private void SaveMemoryInfo()
    {
        if (m_ClassifiedMemoryInfos.Count > 0) {
            string path = EditorUtility.SaveFilePanel("请指定要保存内存信息的文件", string.Empty, "memory", "txt");
            if (!string.IsNullOrEmpty(path)) {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path)) {
                    sw.WriteLine("id\tasset\tpath\tname\tclass\tsize\tis_manager\tis_persistent\tis_dont_destroy_on_load\taddress");
                    int curCount = 0;
                    int totalCount = 0;
                    foreach (var pair in m_ClassifiedMemoryInfos) {
                        totalCount += pair.Value.Count;
                    }
                    foreach (var pair in m_ClassifiedMemoryInfos) {
                        foreach (var memory in pair.Value) {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t0x{9:X}", (uint)memory.instanceId, memory.assetPath, memory.scenePath, memory.name, memory.className, memory.size, memory.isManager, memory.isPersistent, memory.isDontDestroyOnLoad, memory.nativeObjectAddress);
                            ++curCount;
                            DisplayProgressBar("保存进度", curCount, totalCount);
                        }
                    }
                    sw.Close();
                    EditorUtility.ClearProgressBar();
                }
            }
        } else {
            EditorUtility.DisplayDialog("错误", "没有内存快照，请先捕获内存快照！", "ok");
        }
    }
    private void LoadMemoryInfo()
    {
        string path = EditorUtility.OpenFilePanel("请指定要加载内存信息的文件", string.Empty, "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            int i = 0;
            try {
                var txt = File.ReadAllText(path);
                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                m_ClassifiedMemoryInfos.Clear();
                int curCount = 1;
                int totalCount = lines.Length;
                for (i = 1; i < lines.Length; ++i) {
                    var fields = lines[i].Split('\t');
                    var info = new ResourceEditUtility.MemoryInfo();
                    info.instanceId = (int)uint.Parse(fields[0]);
                    info.assetPath = fields[1];
                    info.scenePath = fields[2];
                    info.name = fields[3];
                    info.className = fields[4];
                    info.size = int.Parse(fields[5]);
                    info.isManager = bool.Parse(fields[6]);
                    info.isPersistent = bool.Parse(fields[7]);
                    info.isDontDestroyOnLoad = bool.Parse(fields[8]);
                    info.nativeObjectAddress = long.Parse(fields[9].Substring(2), System.Globalization.NumberStyles.HexNumber);

                    List<ResourceEditUtility.MemoryInfo> list;
                    if (!m_ClassifiedMemoryInfos.TryGetValue(info.className, out list)) {
                        list = new List<ResourceEditUtility.MemoryInfo>();
                        m_ClassifiedMemoryInfos.Add(info.className, list);
                    }
                    list.Add(info);

                    ++curCount;
                    DisplayProgressBar("加载进度", curCount, totalCount);
                }
            } catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }

    private void ClearInstrumentInfo()
    {
        m_InstrumentInfos.Clear();
        m_RecordIndex = 0;
        m_LastFrame = -1;
    }
    private bool RecordInstrument(int index, int frame, ProfilerColumn sortType, ProfilerViewType viewType, float triangle, float batch)
    {
        ProfilerProperty prop = new ProfilerProperty();
        prop.SetRoot(frame, sortType, viewType);
        prop.onlyShowGPUSamples = false;

        if (!prop.frameDataReady)
            return false;
        
        var info = new ResourceEditUtility.InstrumentInfo();
        info.index = index;
        info.frame = frame + 1;
        info.sortType = (int)sortType;
        info.viewType = (int)viewType;
        info.triangle = triangle;
        info.batch = batch;
        info.totalCpuTime = InstrumentString2Float(prop.frameTime);
        info.totalGpuTime = InstrumentString2Float(prop.frameGpuTime);
        info.fps = InstrumentString2Float(prop.frameFPS);
        info.totalCalls = 0;
        info.totalGcMemory = 0;

        while (prop.Next(true)) {
            var data = new ResourceEditUtility.InstrumentRecord();
            data.depth = prop.depth;
            data.fps = info.fps;
            data.calls = int.Parse(prop.GetColumn(ProfilerColumn.Calls));
            data.gcMemory = InstrumentString2Float(prop.GetColumn(ProfilerColumn.GCMemory));
            data.name = prop.propertyName;
            data.layerPath = prop.propertyPath;
            data.totalTime = InstrumentString2Float(prop.GetColumn(ProfilerColumn.TotalTime));
            data.totalPercent = InstrumentString2Float(prop.GetColumn(ProfilerColumn.TotalPercent));
            data.selfTime = InstrumentString2Float(prop.GetColumn(ProfilerColumn.SelfTime));
            data.selfPercent = InstrumentString2Float(prop.GetColumn(ProfilerColumn.SelfPercent));
            data.totalGpuTime = InstrumentString2Float(prop.GetColumn(ProfilerColumn.TotalGPUTime));
            data.totalGpuPercent = InstrumentString2Float(prop.GetColumn(ProfilerColumn.TotalGPUPercent));
            data.selfGpuTime = InstrumentString2Float(prop.GetColumn(ProfilerColumn.SelfGPUTime));
            data.selfGpuPercent = InstrumentString2Float(prop.GetColumn(ProfilerColumn.SelfGPUPercent));

            info.totalCalls += data.calls;
            info.totalGcMemory += data.gcMemory;
            info.records.Add(data);
        }

        var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Instrument = info, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
        var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
            m_InstrumentInfos[index] = info;
            return true;
        }
        return false;
    }
    private float InstrumentString2Float(string val)
    {
        try {
            if (val == "N/A") {
                return 0;
            }
            int ix = val.IndexOf('%');
            if (ix > 0) {
                return float.Parse(val.Substring(0, ix).Trim());
            }
            ix = val.IndexOf(" MB");
            if (ix > 0) {
                return float.Parse(val.Substring(0, ix).Trim()) * 1024.0f;
            }
            ix = val.IndexOf(" KB");
            if (ix > 0) {
                return float.Parse(val.Substring(0, ix).Trim());
            }
            ix = val.IndexOf(" B");
            if (ix > 0) {
                return float.Parse(val.Substring(0, ix).Trim()) / 1024.0f;
            }
            return float.Parse(val.Trim());
        } catch(Exception ex) {
            Debug.LogErrorFormat("InstrumentString2Float {0} throw exception:{1}\n{2}", val, ex.Message, ex.StackTrace);
            return 0;
        }
    }
    private void SaveInstrumentInfo()
    {
        if (m_InstrumentInfos.Count > 0) {
            string path = EditorUtility.SaveFilePanel("请指定要保存耗时信息的文件", string.Empty, "instrument", "txt");
            if (!string.IsNullOrEmpty(path)) {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path)) {
                    sw.WriteLine("index\tframe\tdepth\tname\tpath\tfps\tcalls\tgc\ttotal_time\ttotal_percent\tself_time\tself_percent\ttotal_gpu_time\ttotal_gpu_percent\tself_gpu_time\tself_gpu_percent");
                    int curCount = 0;
                    int totalCount = 0;
                    foreach (var pair in m_InstrumentInfos) {
                        totalCount += pair.Value.records.Count;
                    }
                    foreach (var pair in m_InstrumentInfos) {
                        var info = pair.Value;
                        sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}",
                            info.index, info.frame, 0, "frame_stat_tag", "frame_stat_tag",
                            info.fps, info.totalCalls, info.totalGcMemory,
                            info.totalCpuTime, 100, info.sortType, info.viewType,
                            info.totalGpuTime, 100, info.triangle, info.batch);
                        ++curCount;
                        DisplayProgressBar("保存进度", curCount, totalCount);
                        foreach (var record in info.records) {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}",
                                info.index, info.frame, record.depth, record.name, record.layerPath,
                                info.fps, record.calls, record.gcMemory,
                                record.totalTime, record.totalPercent, record.selfTime, record.selfPercent,
                                record.totalGpuTime, record.totalGpuPercent, record.selfGpuTime, record.selfGpuPercent);
                            ++curCount;
                            DisplayProgressBar("保存进度", curCount, totalCount);
                        }
                    }
                    sw.Close();
                    EditorUtility.ClearProgressBar();
                }
            }
        } else {
            EditorUtility.DisplayDialog("错误", "没有记录耗时信息，请先记录！", "ok");
        }
    }
    private void LoadInstrumentInfo()
    {
        string path = EditorUtility.OpenFilePanel("请指定要加载耗时信息的文件", string.Empty, "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            int i = 0;
            try {
                var txt = File.ReadAllText(path);
                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                m_InstrumentInfos.Clear();
                m_RecordIndex = 0;
                m_LastFrame = -1;
                int curCount = 1;
                int totalCount = lines.Length;
                for (i = 1; i < lines.Length; ++i) {
                    var fields = lines[i].Split('\t');
                    var index = int.Parse(fields[0]);
                    var frame = int.Parse(fields[1]);
                    var depth = int.Parse(fields[2]);
                    var name = fields[3];
                    var layerPath = fields[4];
                    var fps = float.Parse(fields[5]);
                    var calls = int.Parse(fields[6]);
                    var gc = float.Parse(fields[7]);
                    var totalTime = float.Parse(fields[8]);
                    var totalPercent = float.Parse(fields[9]);
                    var selfTime = float.Parse(fields[10]);
                    var selfPercent = float.Parse(fields[11]);
                    var totalGpuTime = float.Parse(fields[12]);
                    var totalGpuPercent = float.Parse(fields[13]);
                    var selfGpuTime = float.Parse(fields[14]);
                    var selfGpuPercent = float.Parse(fields[15]);

                    if (name == "frame_stat_tag" && layerPath == "frame_stat_tag") {
                        var info = new ResourceEditUtility.InstrumentInfo();
                        info.index = index;
                        info.frame = frame;
                        info.fps = fps;
                        info.totalCalls = calls;
                        info.totalGcMemory = gc;
                        info.totalCpuTime = totalTime;
                        info.totalGpuTime = totalGpuTime;
                        info.sortType = (int)selfTime;
                        info.viewType = (int)selfPercent;
                        info.triangle = selfGpuTime;
                        info.batch = selfGpuPercent;

                        m_InstrumentInfos[index] = info;
                        m_LastFrame = frame;
                        m_RecordIndex = index + 1;
                    } else {
                        ResourceEditUtility.InstrumentInfo info;
                        if (m_InstrumentInfos.TryGetValue(index, out info)) {
                            var record = new ResourceEditUtility.InstrumentRecord();
                            record.depth = depth;
                            record.name = name;
                            record.layerPath = layerPath;
                            record.fps = fps;
                            record.calls = calls;
                            record.gcMemory = gc;
                            record.totalTime = totalTime;
                            record.totalPercent = totalPercent;
                            record.selfTime = selfTime;
                            record.selfPercent = selfPercent;
                            record.totalGpuTime = totalGpuTime;
                            record.totalGpuPercent = totalGpuPercent;
                            record.selfGpuTime = selfGpuTime;
                            record.selfGpuPercent = selfGpuPercent;
                        }
                    }

                    ++curCount;
                    DisplayProgressBar("加载进度", curCount, totalCount);
                }
            } catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }

    private GameObject CreateTerrain(int areaSize, out int cx, out int cy)
    {
        const int c_size = 512;
        const int c_height = 128;
        cx = cy = (int)Mathf.Ceil(c_size * 1.0f / areaSize);
        TerrainData terrainData = new TerrainData();
        terrainData.size = new Vector3(c_size, c_height, c_size);
        terrainData.heightmapResolution = c_size * 2 + 1;
        terrainData.baseMapResolution = c_size * 2;
        terrainData.SetDetailResolution(c_size * 2, 8);
        string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling(null, "Terrain");
        GameObject gameObject = Terrain.CreateTerrainGameObject(terrainData);
        gameObject.name = uniqueNameForSibling;
        GameObjectUtility.SetParentAndAlign(gameObject, null);
        return gameObject;
    }

    private void Sort(bool asc)
    {
        m_ItemList.Sort((a, b) => {
            int v;
            if (a.Order < b.Order)
                v = -1;
            else if (a.Order > b.Order)
                v = 1;
            else
                v = 0;
            if (!asc)
                v = -v;
            return v;
        });
    }

    private void ListItem()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (var item in m_ItemList) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (var item in m_ItemList) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Total count ({0})", m_ItemList.Count), GUILayout.Width(120));
        GUILayout.Label(string.Format("Go to page ({0})", m_ItemList.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = EditorGUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        if (GUILayout.Button("Refresh", GUILayout.Width(60))) {
            Refresh();
        }
        if (GUILayout.Button("升序", GUILayout.Width(60))) {
            Sort(true);
        }
        if (GUILayout.Button("降序", GUILayout.Width(60))) {
            Sort(false);
        }
        GUILayout.Label(string.Format("Total value ({0})", m_TotalItemValue));
        EditorGUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_ItemList.Count / c_ItemsPerPage + 1, m_Page));
        bool showReferences = false;
        float rightWidth = 0;
        if (!string.IsNullOrEmpty(m_SelectedAssetPath) && (m_ReferenceAssets.Count > 0 || m_ReferenceByAssets.Count > 0)) {
            showReferences = true;
            rightWidth = m_RightWidth;
        }
        float windowWidth = position.width;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true, GUILayout.Width(windowWidth - rightWidth));
        int index = 0;
        int totalShown = 0;
        foreach (var item in m_ItemList) {
            ++index;
            if (index <= (m_Page - 1) * c_ItemsPerPage)
                continue;
            ++totalShown;
            if (totalShown > c_ItemsPerPage)
                break;
            EditorGUILayout.BeginHorizontal();
            item.Selected = GUILayout.Toggle(item.Selected, index + ".", GUILayout.Width(60));
            string buttonName = string.Format("{0},{1}", item.AssetPath, item.ScenePath);
            if (m_SearchSource == "sceneobjects" || m_SearchSource == "sceneareas") {
                var oldAlignment = GUI.skin.button.alignment;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (GUILayout.Button(new GUIContent(buttonName, buttonName), GUILayout.MinWidth(80), GUILayout.MaxWidth(windowWidth - rightWidth))) {
                    SelectObject(item.Object);
                    m_SelectedAssetPath = string.Empty;
                }
                GUI.skin.button.alignment = oldAlignment;
            } else {
                Texture icon = AssetDatabase.GetCachedIcon(item.AssetPath);
                var oldAlignment = GUI.skin.button.alignment;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (GUILayout.Button(new GUIContent(buttonName, icon, buttonName), GUILayout.MinWidth(80), GUILayout.MaxWidth(windowWidth - rightWidth))) {
                    if (null != item.Object)
                        SelectObject(item.Object);
                    else
                        SelectObject(item.AssetPath);
                    m_SelectedAssetPath = item.AssetPath;
                }
                GUI.skin.button.alignment = oldAlignment;
            }
            EditorGUILayout.TextArea(item.Info, GUILayout.MaxHeight(72), GUILayout.MinWidth(80), GUILayout.MaxWidth(windowWidth - rightWidth));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        if (showReferences) {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_RightWidth - 200);
            m_RightWidth = EditorGUILayout.Slider(m_RightWidth, windowWidth - 200, 200, GUILayout.Width(160));
            EditorGUILayout.EndHorizontal();
            m_PanelPosRight = EditorGUILayout.BeginScrollView(m_PanelPosRight, true, true, GUILayout.Width(rightWidth));
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("References:");
            EditorGUILayout.EndHorizontal();
            HashSet<string> refSet;
            if (m_ReferenceAssets.TryGetValue(m_SelectedAssetPath, out refSet)) {
                foreach (string assetPath in refSet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        SelectObject(assetPath);
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("References by:");
            EditorGUILayout.EndHorizontal();
            HashSet<string> refBySet;
            if (m_ReferenceByAssets.TryGetValue(m_SelectedAssetPath, out refBySet)) {
                foreach (string assetPath in refBySet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        SelectObject(assetPath);
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SearchFiles(string dir)
    {
        string dirName = Path.GetFileName(dir).ToLower();
        if (s_IgnoredDirs.Contains(dirName))
            return;
        foreach (string ext in m_TypeOrExtList) {
            SearchFilesRecursively(dir, ext.ToLower());
        }
    }

    private void SearchFilesRecursively(string dir, string ext)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            string ext0 = Path.GetExtension(file).ToLower();
            return ext0 == ext;
        }).ToArray();
        foreach (string file in files) {
            ++m_CurSearchCount;
            string assetPath = PathToAssetPath(file);
            var importer = AssetImporter.GetAtPath(assetPath);
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.Add(item);
            }
            DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        }
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            string dirName = Path.GetFileName(subDir).ToLower();
            if (s_IgnoredDirs.Contains(dirName))
                continue;
            SearchFilesRecursively(subDir, ext);
        }
    }

    private void CountFiles(string dir)
    {
        string dirName = Path.GetFileName(dir).ToLower();
        if (s_IgnoredDirs.Contains(dirName))
            return;
        foreach (string ext in m_TypeOrExtList) {
            CountFilesRecursively(dir, ext.ToLower());
        }
    }

    private void CountFilesRecursively(string dir, string ext)
    {
        string[] files = Directory.GetFiles(dir).Where((string file) => {
            string ext0 = Path.GetExtension(file).ToLower();
            return ext0 == ext;
        }).ToArray();
        m_TotalSearchCount += files.Length;

        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            string dirName = Path.GetFileName(subDir).ToLower();
            if (s_IgnoredDirs.Contains(dirName))
                continue;
            CountFilesRecursively(subDir, ext);
        }
    }

    private void AnalyseAssets()
    {
        m_ReferenceAssets.Clear();
        m_ReferenceByAssets.Clear();
        m_UnusedAssets.Clear();
        var guids = AssetDatabase.FindAssets(string.Empty);
        var allFiles = new HashSet<string>();
        var depFiles = new HashSet<string>();
        for (int i = 0; i < guids.Length; ++i) {
            string file = AssetDatabase.GUIDToAssetPath(guids[i]);
            string fpath = AssetPathToPath(file);
            if (!File.Exists(fpath) || IsIgnoreDir(file))
                continue;
            if (!allFiles.Contains(file)) {
                allFiles.Add(file);

                var deps = AssetDatabase.GetDependencies(file);
                HashSet<string> refSet;
                if (!m_ReferenceAssets.TryGetValue(file, out refSet)) {
                    refSet = new HashSet<string>();
                    m_ReferenceAssets.Add(file, refSet);
                }
                foreach (var dep in deps) {
                    if (dep == file)
                        continue;
                    if (!depFiles.Contains(dep))
                        depFiles.Add(dep);
                    if (!refSet.Contains(dep)) {
                        refSet.Add(dep);
                    }
                    HashSet<string> refBySet;
                    if (!m_ReferenceByAssets.TryGetValue(dep, out refBySet)) {
                        refBySet = new HashSet<string>();
                        m_ReferenceByAssets.Add(dep, refBySet);
                    }
                    if (!refBySet.Contains(file)) {
                        refBySet.Add(file);
                    }
                }
            }
            int ct = i + 1;
            if (DisplayCancelableProgressBar("依赖分析进度", depFiles.Count, ct, guids.Length, false)) {
                m_ReferenceAssets.Clear();
                m_ReferenceByAssets.Clear();
                m_UnusedAssets.Clear();
                EditorUtility.ClearProgressBar();
                return;
            }
        }
        foreach (string file in allFiles) {
            if (!depFiles.Contains(file)) {
                m_UnusedAssets.Add(file);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private void AnalyseSceneObjects()
    {
        m_SceneAreaInfo.areas.Clear();
        var terrain = Terrain.activeTerrain;
        var size = terrain.terrainData.size;
        m_SceneAreaInfo.Init(size);

        int totalCount = CountSceneObjects();
        int curCount = 0;

        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                AnalyseChildObjectsRecursively(string.Empty, obj, totalCount, ref curCount);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private void AnalyseChildObjectsRecursively(string path, GameObject obj, int totalCount, ref int curCount)
    {
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        } else {
            path = path + "/" + obj.name;
        }

        m_SceneAreaInfo.AddObject(path, obj);
        ++curCount;
        DisplayProgressBar("物件分析进度", curCount, totalCount);

        var trans = obj.transform;
        int ct = trans.childCount;
        for (int i = 0; i < ct; ++i) {
            var t = trans.GetChild(i);
            AnalyseChildObjectsRecursively(path, t.gameObject, totalCount, ref curCount);
        }
    }

    private void SearchUnusedAssets()
    {
        if (m_UnusedAssets.Count <= 0)
            return;

        foreach (string ext in m_TypeOrExtList) {
            string lowerExt = ext.ToLower();
            var files = m_UnusedAssets.Where((string file) => {
                string ext0 = Path.GetExtension(file).ToLower();
                return ext0 == lowerExt;
            }).ToArray();

            foreach (string file in files) {
                ++m_CurSearchCount;
                string assetPath = PathToAssetPath(file);
                var importer = AssetImporter.GetAtPath(assetPath);
                var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
                if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                    m_ItemList.Add(item);
                }
                DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
            }
        }
    }

    private void CountUnusedAssets()
    {
        foreach (string ext in m_TypeOrExtList) {
            string lowerExt = ext.ToLower();
            var files = m_UnusedAssets.Where((string file) => {
                string ext0 = Path.GetExtension(file).ToLower();
                return ext0 == lowerExt;
            }).ToArray();
            m_TotalSearchCount += files.Length;
        }
    }

    private void SearchAllAssets()
    {
        string filter = string.Join(" ", m_TypeOrExtList.ToArray());
        var guids = AssetDatabase.FindAssets(filter);
        var allFiles = new HashSet<string>();
        for (int i = 0; i < guids.Length; ++i) {
            string file = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (!allFiles.Contains(file)) {
                allFiles.Add(file);
            }
        }

        var files = allFiles.ToArray();
        foreach (string file in files) {
            ++m_CurSearchCount;
            string assetPath = PathToAssetPath(file);
            var importer = AssetImporter.GetAtPath(assetPath);
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.Add(item);
            }
            DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        }
    }

    private void CountAllAssets()
    {
        string filter = string.Join(" ", m_TypeOrExtList.ToArray());
        var guids = AssetDatabase.FindAssets(filter);
        m_TotalSearchCount = guids.Length;
    }

    private void SearchSceneAssets()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var assets = GetDependencies(scene.path);

            foreach (string ext in m_TypeOrExtList) {
                string lowerExt = ext.ToLower();
                var files = assets.Where((string file) => {
                    string ext0 = Path.GetExtension(file).ToLower();
                    return ext0 == lowerExt;
                }).ToArray();

                foreach (string file in files) {
                    ++m_CurSearchCount;
                    string assetPath = PathToAssetPath(file);
                    var importer = AssetImporter.GetAtPath(assetPath);
                    var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                    var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
                    if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                        m_ItemList.Add(item);
                    }
                    DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
                }
            }
        }
    }

    private void CountSceneAssets()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var assets = GetDependencies(scene.path);

            foreach (string ext in m_TypeOrExtList) {
                string lowerExt = ext.ToLower();
                var files = assets.Where((string file) => {
                    string ext0 = Path.GetExtension(file).ToLower();
                    return ext0 == lowerExt;
                }).ToArray();
                m_TotalSearchCount += files.Length;
            }
        }
    }

    private void SearchSceneAreas()
    {
        foreach (var area in m_SceneAreaInfo.areas) {
            ++m_CurSearchCount;
            string assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(area.maxTriangleObject));
            AssetImporter importer = null;
            if (string.IsNullOrEmpty(assetPath)) {
                assetPath = string.Empty;
            } else {
                importer = AssetImporter.GetAtPath(assetPath);
            }
            string scenePath = string.Format("area:{0},{1}", area.rowIndex, area.colIndex);
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = area.maxTriangleObject, Area = area, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.Add(item);
            }
            DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        }
    }

    private void CountSceneAreas()
    {
        ResourceEditUtility.ParamInfo paramInfo;
        if (m_Params.TryGetValue("areasize", out paramInfo) && paramInfo.Type == typeof(int)) {
            int v = (int)paramInfo.Value;
            if (ResourceEditUtility.SceneAreaInfo.s_AreaSize != v) {
                ResourceEditUtility.SceneAreaInfo.s_AreaSize = v;
                m_SceneAreaInfo.Clear();
            }
        }
        if (m_SceneAreaInfo.areas.Count <= 0) {
            AnalyseSceneObjects();
        }
        m_TotalSearchCount = m_SceneAreaInfo.areas.Count;
    }

    private void SearchSceneObjects()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                SearchChildObjectsRecursively(string.Empty, obj);
            }
        }
    }

    private void SearchChildObjectsRecursively(string path, GameObject obj)
    {
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        } else {
            path = path + "/" + obj.name;
        }
        ++m_CurSearchCount;
        if (IsMatchedObject(obj)) {
            string assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
            AssetImporter importer = null;
            if (string.IsNullOrEmpty(assetPath)) {
                assetPath = string.Empty;
            } else {
                importer = AssetImporter.GetAtPath(assetPath);
            }
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = path, Importer = importer, Object = obj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.Add(item);
            }
        }
        DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);

        var trans = obj.transform;
        int ct = trans.childCount;
        for (int i = 0; i < ct; ++i) {
            var t = trans.GetChild(i);
            SearchChildObjectsRecursively(path, t.gameObject);
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

    private int CountSceneObjects()
    {
        int totalCount = 0;
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            totalCount += objs.Length;

            foreach (var obj in objs) {
                totalCount += CountChildObjectsRecursively(obj);
            }
        }
        return totalCount;
    }

    private int CountChildObjectsRecursively(GameObject obj)
    {
        int totalCount = 0;
        var trans = obj.transform;
        int ct = trans.childCount;
        totalCount += ct;
        for (int i = 0; i < ct; ++i) {
            var t = trans.GetChild(i);
            totalCount += CountChildObjectsRecursively(t.gameObject);
        }
        return totalCount;
    }

    private void SearchSnapshot()
    {
        string type = string.Empty;
        ResourceEditUtility.ParamInfo paramInfo;
        if (m_Params.TryGetValue("type", out paramInfo)) {
            type = paramInfo.Value as string;
        }
        int curCount = 0;
        int totalCount = 0;
        bool handled = false;
        if (!string.IsNullOrEmpty(type)) {
            List<ResourceEditUtility.MemoryInfo> list;
            if (m_ClassifiedMemoryInfos.TryGetValue(type, out list)) {
                totalCount = list.Count;
                foreach (var memory in list) {
                    DoFilterMemoryInfo(memory);
                    ++curCount;
                    DisplayProgressBar("采集进度", m_ItemList.Count, curCount, totalCount);
                }
                handled = true;
            }
        }
        if (!handled) {
            totalCount = m_Snapshot.nativeObjects.Length;
            foreach (var pair in m_ClassifiedMemoryInfos) {
                foreach (var memory in pair.Value) {
                    DoFilterMemoryInfo(memory);
                    ++curCount;
                    DisplayProgressBar("采集进度", m_ItemList.Count, curCount, totalCount);
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private void DoFilterMemoryInfo(ResourceEditUtility.MemoryInfo memory)
    {
        string assetPath = memory.assetPath;
        string scenePath = memory.scenePath;
        UnityEngine.Object assetObj = memory.Object;
        AssetImporter importer = null;
        if (null == assetObj && !string.IsNullOrEmpty(scenePath)) {
            assetObj = GameObject.Find(scenePath);
        }
        if (!string.IsNullOrEmpty(assetPath)) {
            importer = AssetImporter.GetAtPath(assetPath);
            if (null == assetObj)
                assetObj = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }
        var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = assetObj, Memory = memory, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
        var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
            m_ItemList.Add(item);
        }
    }

    private void SearchInstruments()
    {
        if (m_InstrumentInfos.Count <= 0)
            return;

        m_TotalSearchCount = m_InstrumentInfos.Count;
        foreach(var pair in m_InstrumentInfos) {
            var info = pair.Value;
            ++m_CurSearchCount;
            var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Instrument = info, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Process(true, item, m_FilterCalculator, m_NextFilterIndex, m_Params, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.Add(item);
            }
            DisplayProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        }
        EditorUtility.ClearProgressBar();
    }

    private void IncomingSnapshot(PackedMemorySnapshot snapshot)
    {
        if (EditorWindow.focusedWindow != this)
            return;

        m_Snapshot = snapshot;
        m_NeedAnalyseSnapshot = true;
    }

    private void AnalyseSnapshot()
    {
        m_ClassifiedMemoryInfos.Clear();
        int curCount = 0;
        int totalCount = m_Snapshot.nativeObjects.Length;
        foreach (var obj in m_Snapshot.nativeObjects) {
            var typeInfo = m_Snapshot.nativeTypes[obj.nativeTypeArrayIndex];
            string assetPath = string.Empty;
            string scenePath = string.Empty;
            UnityEngine.Object assetObj = null;
            bool handled = false;
            if (m_ActiveProfilerIsEditor) {
                var runtimeObj = InternalEditorUtility.GetObjectFromInstanceID(obj.instanceId);
                if (null != runtimeObj) {
                    assetPath = AssetDatabase.GetAssetPath(runtimeObj);
                    var go = runtimeObj as UnityEngine.GameObject;
                    if (null == go) {
                        var comp = runtimeObj as UnityEngine.Component;
                        if (null != comp) {
                            go = comp.gameObject;
                        }
                    }
                    if (null != go) {
                        var tran = go.transform;
                        List<string> paths = new List<string>();
                        while (null != tran) {
                            paths.Insert(0, tran.name);
                            tran = tran.parent;
                        }
                        scenePath = string.Join("/", paths.ToArray());
                        if (string.IsNullOrEmpty(assetPath)) {
                            assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(go));
                        }
                    }
                    assetObj = runtimeObj;
                    handled = true;
                }
            }
            if (!handled) {
                handled = FindSceneObject(obj.name, typeInfo.name, ref assetPath, ref scenePath, ref assetObj);
            }
            if (!handled && !string.IsNullOrEmpty(obj.name) && !string.IsNullOrEmpty(typeInfo.name)) {
                var guids = AssetDatabase.FindAssets(obj.name);
                foreach (var guid in guids) {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (Path.GetExtension(path).ToLower() == ".unity")
                        continue;
                    string osPath = AssetPathToPath(path);
                    if (File.Exists(osPath)) {
                        bool find = false;
                        var objs = AssetDatabase.LoadAllAssetsAtPath(path);
                        foreach (var assetObject in objs) {
                            if (null != assetObject && assetObject.GetType().Name.EndsWith(typeInfo.name)) {
                                assetPath = path;
                                assetObj = assetObject;
                                find = true;
                                break;
                            }
                        }
                        if (find)
                            break;
                    }
                }
            }
            var memory = new ResourceEditUtility.MemoryInfo();
            memory.instanceId = obj.instanceId;
            memory.name = obj.name;
            memory.className = typeInfo.name;
            memory.size = obj.size;
            memory.isManager = obj.isManager;
            memory.isPersistent = obj.isPersistent;
            memory.isDontDestroyOnLoad = obj.isDontDestroyOnLoad;
            memory.nativeObjectAddress = obj.nativeObjectAddress;
            memory.assetPath = assetPath;
            memory.scenePath = scenePath;
            memory.Object = assetObj;

            List<ResourceEditUtility.MemoryInfo> list = null;
            if (!m_ClassifiedMemoryInfos.TryGetValue(memory.className, out list)) {
                list = new List<ResourceEditUtility.MemoryInfo>();
                m_ClassifiedMemoryInfos.Add(memory.className, list);
            }
            list.Add(memory);

            ++curCount;
            if (DisplayCancelableProgressBar("内存信息分类进度", curCount, totalCount, false)) {
                m_ClassifiedMemoryInfos.Clear();
                EditorUtility.ClearProgressBar();
                return;
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private bool FindSceneObject(string name, string type, ref string assetPath, ref string scenePath, ref UnityEngine.Object sceneObj)
    {
        bool ret = false;
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                ret = FindChildObjectsRecursively(string.Empty, obj, name, type, ref assetPath, ref scenePath, ref sceneObj);
                if (ret)
                    break;
            }
            if (ret)
                break;
        }
        return ret;
    }

    private bool FindChildObjectsRecursively(string path, GameObject obj, string name, string type, ref string assetPath, ref string scenePath, ref UnityEngine.Object sceneObj)
    {
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        } else {
            path = path + "/" + obj.name;
        }
        bool ret = false;
        if (obj.name == name) {
            if (type == "GameObject") {
                ret = true;
            } else if (type == "ParticleSystem") {
                var comp = obj.GetComponent<ParticleSystem>();
                if (null != comp) {
                    ret = true;
                }
            }
            var prefabObj = PrefabUtility.GetPrefabObject(obj);
            var prefabPath = AssetDatabase.GetAssetPath(prefabObj);
            if (!ret) {
                var objs = AssetDatabase.LoadAllAssetsAtPath(prefabPath);
                foreach (var assetObject in objs) {
                    if (null != assetObject && assetObject.GetType().Name.EndsWith(type)) {
                        ret = true;
                        break;
                    }
                }
            }
            if (ret) {
                assetPath = prefabPath;
                if (string.IsNullOrEmpty(assetPath)) {
                    assetPath = string.Empty;
                }
                scenePath = path;
                sceneObj = obj;
            }
        }
        if (!ret) {
            var trans = obj.transform;
            int ct = trans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = trans.GetChild(i);
                ret = FindChildObjectsRecursively(path, t.gameObject, name, type, ref assetPath, ref scenePath, ref sceneObj);
                if (ret)
                    break;
            }
        }
        return ret;
    }

    private void DisplayProgressBar(string title, int resultCount, int curCount, int totalCount)
    {
        DisplayProgressBar(title, resultCount, curCount, totalCount, true);
    }
    private void DisplayProgressBar(string title, int resultCount, int curCount, int totalCount, bool batch)
    {
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                EditorUtility.DisplayProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
            }
        } else {
            EditorUtility.DisplayProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
        }
    }

    private void DisplayProgressBar(string title, int curCount, int totalCount)
    {
        DisplayProgressBar(title, curCount, totalCount, true);
    }
    private void DisplayProgressBar(string title, int curCount, int totalCount, bool batch)
    {
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
            }
        } else {
            EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
        }
    }

    private bool DisplayCancelableProgressBar(string title, int resultCount, int curCount, int totalCount)
    {
        return DisplayCancelableProgressBar(title, resultCount, curCount, totalCount, true);
    }
    private bool DisplayCancelableProgressBar(string title, int resultCount, int curCount, int totalCount, bool batch)
    {
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
            }
        } else {
            return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
        }
        return false;
    }

    private bool DisplayCancelableProgressBar(string title, int curCount, int totalCount)
    {
        return DisplayCancelableProgressBar(title, curCount, totalCount, true);
    }
    private bool DisplayCancelableProgressBar(string title, int curCount, int totalCount, bool batch)
    {
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
            }
        } else {
            return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
        }
        return false;
    }


    private IEnumerable<string> GetDependencies(string path)
    {
        HashSet<string> list;
        if (m_ReferenceAssets.TryGetValue(path, out list)) {
            return list;
        } else {
            return AssetDatabase.GetDependencies(path);
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

    private bool IsIgnoreDir(string dir)
    {
        string path = dir.ToLower();
        foreach (string key in s_IgnoreDirKeys) {
            if (path.Contains(key)) {
                return true;
            }
        }
        return false;
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

    private static void SelectObject(UnityEngine.Object obj)
    {
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(Selection.activeObject);
        //SceneView.lastActiveSceneView.FrameSelected(true);
        SceneView.FrameLastActiveSceneView();
    }

    private string m_SearchSource = string.Empty;
    private string m_PostProcessClass = string.Empty;
    private string m_PostProcessMethod = string.Empty;
    private List<string> m_TypeOrExtList = new List<string>();
    private List<Type> m_TypeList = new List<Type>();
    private List<string> m_ParamNames = new List<string>();
    private Dictionary<string, ResourceEditUtility.ParamInfo> m_Params = new Dictionary<string, ResourceEditUtility.ParamInfo>();
    private Dictionary<string, HashSet<string>> m_ReferenceAssets = new Dictionary<string, HashSet<string>>();
    private Dictionary<string, HashSet<string>> m_ReferenceByAssets = new Dictionary<string, HashSet<string>>();
    private List<string> m_UnusedAssets = new List<string>();
    private Dictionary<string, List<ResourceEditUtility.MemoryInfo>> m_ClassifiedMemoryInfos = new Dictionary<string, List<ResourceEditUtility.MemoryInfo>>();
    private ResourceEditUtility.SceneAreaInfo m_SceneAreaInfo = new ResourceEditUtility.SceneAreaInfo();
    private List<ResourceEditUtility.ItemInfo> m_ItemList = new List<ResourceEditUtility.ItemInfo>();
    private double m_TotalItemValue = 0;
    private Dsl.DslFile m_DslFile = null;
    private string m_CollectPath = string.Empty;

    private bool m_Record = false;
    private SortedList<int, ResourceEditUtility.InstrumentInfo> m_InstrumentInfos = new SortedList<int, ResourceEditUtility.InstrumentInfo>();
    private int m_RecordIndex = 0;
    private int m_LastFrame = -1;

    private Expression.DslCalculator m_FilterCalculator = null;
    private Expression.DslCalculator m_ProcessCalculator = null;
    private int m_NextFilterIndex = 0;
    private int m_NextProcessIndex = 0;

    private Vector2 m_PanelPos = Vector2.zero;
    private Vector2 m_PanelPosRight = Vector2.zero;
    private float m_RightWidth = 240;
    private string m_Text = string.Empty;
    private Dictionary<string, string> m_EditedParams = new Dictionary<string, string>();
    private int m_Page = 1;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;
    private string m_SelectedAssetPath = string.Empty;

    [NonSerialized]
    private bool m_SnapshotRegistered = false;
    [NonSerialized]
    private bool m_ActiveProfilerIsEditor = false;
    [NonSerialized]
    PackedMemorySnapshot m_Snapshot;
    [NonSerialized]
    private bool m_NeedAnalyseSnapshot = false;

    private const int c_ItemsPerPage = 50;
    private static readonly HashSet<string> s_IgnoredDirs = new HashSet<string> { "plugins", "streamingassets" };
    private static readonly List<string> s_IgnoreDirKeys = new List<string> { "assets/plugins/", "assets/streamingassets/", "/editor default resources/", "assets/thirdparty/" };
}

internal static class ResourceEditUtility
{
    internal class ParamInfo
    {
        internal string Name;
        internal Type Type;
        internal object Value;
        internal object MinValue;
        internal object MaxValue;
        internal List<string> OptionNames = new List<string>();
        internal Dictionary<string, string> Options = new Dictionary<string, string>();
        internal string OptionStyle = string.Empty;
    }
    internal class ItemInfo
    {
        internal string AssetPath;
        internal string ScenePath;
        internal AssetImporter Importer;
        internal UnityEngine.Object Object;
        internal AreaInfo Area;
        internal MemoryInfo Memory;
        internal InstrumentInfo Instrument;
        internal string Info;
        internal int Order;
        internal double Value;
        internal bool Selected;
    }
    internal class AreaInfo
    {
        internal int rowIndex;
        internal int colIndex;
        internal int vertexCount;
        internal int triangleCount;
        internal int materialCount;
        internal int boneCount;
        internal int differentMaterialCount;
        internal Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
        internal GameObject maxTriangleObject;
        internal int maxTriangleCount;
        internal GameObject maxMaterialObject;
        internal int maxMaterialCount;
        internal GameObject maxBoneObject;
        internal int maxBoneCount;
        internal GameObject maxBatchObject;
        internal int maxBatchCount;
    }
    internal class SceneAreaInfo
    {
        internal static int s_AreaSize = 10;
        internal int rowCount;
        internal int colCount;
        internal List<AreaInfo> areas = new List<AreaInfo>();

        internal void Init(Vector3 size)
        {
            colCount = (int)Mathf.Ceil(size.x / s_AreaSize);
            rowCount = (int)Mathf.Ceil(size.z / s_AreaSize);
            areas.Clear();
            for (int i = 0; i < rowCount; ++i) {
                for (int j = 0; j < colCount; ++j) {
                    var info = new AreaInfo();
                    info.rowIndex = i;
                    info.colIndex = j;
                    areas.Add(info);
                }
            }
        }
        internal void Clear()
        {
            rowCount = 0;
            colCount = 0;
            areas.Clear();
        }
        internal int CalcIndex(float x, float z)
        {
            int col = (int)Mathf.Floor(x / s_AreaSize);
            int row = (int)Mathf.Floor(z / s_AreaSize);
            if (row >= 0 && col >= 0 && row < rowCount && col < colCount)
                return row * colCount + col;
            else
                return -1;
        }
        internal void AddObject(string path, GameObject obj)
        {
            var tran = obj.transform;
            int index = CalcIndex(tran.position.x, tran.position.z);
            if (index >= 0 && index < areas.Count) {
                var info = areas[index];
                int vc, tc, mc, bc, dmc;
                CollectInfo(obj, out vc, out tc, out mc, out bc, out dmc);
                info.vertexCount += vc;
                info.triangleCount += tc;
                info.materialCount += mc;
                info.boneCount += bc;
                info.differentMaterialCount += dmc;
                if (info.maxTriangleCount < tc) {
                    info.maxTriangleObject = obj;
                    info.maxTriangleCount = tc;
                }
                if (info.maxMaterialCount < mc) {
                    info.maxMaterialObject = obj;
                    info.maxMaterialCount = mc;
                }
                if (info.maxBoneCount < bc) {
                    info.maxBoneObject = obj;
                    info.maxBoneCount = bc;
                }
                if (info.maxBatchCount < dmc) {
                    info.maxBatchObject = obj;
                    info.maxBatchCount = dmc;
                }
                info.objects[path] = obj;
            }
        }
        internal AreaInfo FindArea(string path)
        {
            GameObject obj = GameObject.Find(path);
            if (null != obj) {
                var tran = obj.transform;
                return FindArea(tran.position.x, tran.position.z);
            }
            return null;
        }
        internal AreaInfo FindArea(GameObject obj)
        {
            if (null != obj) {
                var tran = obj.transform;
                return FindArea(tran.position.x, tran.position.z);
            }
            return null;
        }
        internal AreaInfo FindArea(float x, float z)
        {
            int index = CalcIndex(x, z);
            if (index >= 0 && index < areas.Count) {
                var info = areas[index];
                return info;
            }
            return null;
        }

        internal static void CollectInfo(GameObject obj, out int vertexCount, out int triangleCount, out int materialCount, out int boneCount, out int differentMaterialCount)
        {
            int vc = 0;
            int tc = 0;
            int mc = 0;
            int bc = 0;
            HashSet<string> mats = new HashSet<string>();
            var skinnedrenderers = obj.GetComponents<SkinnedMeshRenderer>();
            foreach (var renderer in skinnedrenderers) {
                if (null != renderer.sharedMesh) {
                    vc += renderer.sharedMesh.vertexCount;
                    tc += renderer.sharedMesh.triangles.Length / 3;
                }
                mc += renderer.sharedMaterials.Length;
                bc += renderer.bones.Length;

                foreach (var mat in renderer.sharedMaterials) {
                    string path = AssetDatabase.GetAssetPath(mat);
                    if (!string.IsNullOrEmpty(path) && !mats.Contains(path)) {
                        mats.Add(path);
                    }
                }
            }
            var filters = obj.GetComponents<MeshFilter>();
            foreach (var filter in filters) {
                if (null != filter.sharedMesh) {
                    vc += filter.sharedMesh.vertexCount;
                    tc += filter.sharedMesh.triangles.Length / 3;
                }
            }
            var meshrenderers = obj.GetComponents<MeshRenderer>();
            foreach (var renderer in meshrenderers) {
                mc += renderer.sharedMaterials.Length;

                foreach (var mat in renderer.sharedMaterials) {
                    string path = AssetDatabase.GetAssetPath(mat);
                    if (!string.IsNullOrEmpty(path) && !mats.Contains(path)) {
                        mats.Add(path);
                    }
                }
            }
            vertexCount = vc;
            triangleCount = tc;
            materialCount = mc;
            boneCount = bc;
            differentMaterialCount = mats.Count;
        }
    }
    internal class MemoryInfo
    {
        internal int instanceId;
        internal string name;
        internal string className;
        internal int size;
        internal bool isManager;
        internal bool isPersistent;
        internal bool isDontDestroyOnLoad;
        internal long nativeObjectAddress;
        internal string assetPath;
        internal string scenePath;
        internal UnityEngine.Object Object;
    }
    internal class InstrumentRecord
    {
        internal int depth;
        internal string name;
        internal string layerPath;
        internal float totalTime;
        internal float totalPercent;
        internal float selfTime;
        internal float selfPercent;
        internal float totalGpuTime;
        internal float totalGpuPercent;
        internal float selfGpuTime;
        internal float selfGpuPercent;
        internal float fps;
        internal int calls;
        internal float gcMemory;
    }
    internal class InstrumentInfo
    {
        internal int index;
        internal int frame;
        internal float fps;
        internal float totalGcMemory;
        internal int totalCalls;
        internal float totalCpuTime;
        internal float totalGpuTime;
        internal int sortType;
        internal int viewType;
        internal float batch;
        internal float triangle;
        internal List<InstrumentRecord> records = new List<InstrumentRecord>();
    }
    internal static void InitCalculator(Expression.DslCalculator calc)
    {
        calc.Init();
        calc.Register("getreferenceassets", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceAssetsExp>());
        calc.Register("getreferencebyassets", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceByAssetsExp>());
        calc.Register("saveandreimport", new Expression.ExpressionFactoryHelper<ResourceEditApi.SaveAndReimportExp>());
        calc.Register("getdefaulttexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetDefaultTextureSettingExp>());
        calc.Register("gettexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetTextureSettingExp>());
        calc.Register("settexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetTextureSettingExp>());
        calc.Register("gettexturecompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetTextureCompressionExp>());
        calc.Register("settexturecompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetTextureCompressionExp>());
        calc.Register("getmeshcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetMeshCompressionExp>());
        calc.Register("setmeshcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetMeshCompressionExp>());
        calc.Register("closemeshanimationifnoanimation", new Expression.ExpressionFactoryHelper<ResourceEditApi.CloseMeshAnimationIfNoAnimationExp>());
        calc.Register("collectmeshinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.CollectMeshInfoExp>());
        calc.Register("collectanimatorcontrollerinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.CollectAnimatorControllerInfoExp>());
        calc.Register("getanimationclipinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationClipInfoExp>());
        calc.Register("getanimationcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationCompressionExp>());
        calc.Register("setanimationcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationCompressionExp>());
        calc.Register("getanimationtype", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationTypeExp>());
        calc.Register("setanimationtype", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationTypeExp>());
        calc.Register("clearanimationscalecurve", new Expression.ExpressionFactoryHelper<ResourceEditApi.ClearAnimationScaleCurveExp>());
        calc.Register("getaudiosetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAudioSettingExp>());
        calc.Register("setaudiosetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAudioSettingExp>());
    }
    internal static object Process(bool isFilter, ItemInfo item, Expression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
    {
        try {
            object ret = null;
            if (null != calc) {
                calc.NamedVariables.Clear();
                calc.NamedVariables.Add("assetpath", item.AssetPath);
                calc.NamedVariables.Add("scenepath", item.ScenePath);
                calc.NamedVariables.Add("importer", item.Importer);
                calc.NamedVariables.Add("object", item.Object);
                calc.NamedVariables.Add("area", item.Area);
                calc.NamedVariables.Add("memory", item.Memory);
                calc.NamedVariables.Add("instrument", item.Instrument);
                calc.NamedVariables.Add("refdict", refDict);
                calc.NamedVariables.Add("refbydict", refByDict);
                foreach (var pair in args) {
                    var p = pair.Value;
                    calc.NamedVariables.Add(p.Name, p.Value);
                }

                for (int i = 0; i < indexCount; ++i) {
                    ret = calc.Calc(i.ToString());
                }

                if (isFilter) {
                    object v;
                    if (calc.NamedVariables.TryGetValue("assetpath", out v)) {
                        var path = v as string;
                        if (!string.IsNullOrEmpty(path))
                            item.AssetPath = path;
                    }
                    if (calc.NamedVariables.TryGetValue("scenepath", out v)) {
                        var path = v as string;
                        if (!string.IsNullOrEmpty(path))
                            item.ScenePath = path;
                    }
                    if (calc.NamedVariables.TryGetValue("importer", out v)) {
                        if (null != v) {
                            item.Importer = v as AssetImporter;
                        }
                    }
                    if (calc.NamedVariables.TryGetValue("object", out v)) {
                        if (null != v) {
                            item.Object = v as UnityEngine.Object;
                        }
                    }
                    if (calc.NamedVariables.TryGetValue("info", out v)) {
                        item.Info = v as string;
                        if (null == item.Info)
                            item.Info = string.Empty;
                    }
                    if (calc.NamedVariables.TryGetValue("order", out v)) {
                        if (null != v) {
                            item.Order = (int)Convert.ChangeType(v, typeof(int));
                        }
                    }
                    if (calc.NamedVariables.TryGetValue("value", out v)) {
                        if (null != v) {
                            item.Value = (double)Convert.ChangeType(v, typeof(double));
                        }
                    }
                }
            }
            return ret;
        } catch (Exception ex) {
            Debug.LogErrorFormat("process {0} exception:{1}\n{2}", item.AssetPath, ex.Message, ex.StackTrace);
            return null;
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
namespace ResourceEditApi
{
    public class MeshInfo
    {
        public int skinnedMeshCount;
        public int meshFilterCount;
        public int vertexCount;
        public int triangleCount;
        public int boneCount;
        public int materialCount;
        public int clipCount;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public string maxKeyFrameClipName = string.Empty;
        public List<AnimationClipInfo> clips = new List<AnimationClipInfo>();
    }
    public class KeyFrameCurveInfo
    {
        public string curveName = string.Empty;
        public string curvePath = string.Empty;
        public int keyFrameCount;
    }
    public class AnimationClipInfo
    {
        public string clipName = string.Empty;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public List<KeyFrameCurveInfo> curves = new List<KeyFrameCurveInfo>();
    }
    public class AnimatorControllerInfo
    {
        public int layerCount;
        public int paramCount;
        public int stateCount;
        public int subStateMachineCount;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public string maxKeyFrameClipName = string.Empty;
        public List<AnimationClipInfo> clips = new List<AnimationClipInfo>();
    }
    internal class GetReferenceAssetsExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = Calculator.NamedVariables["refdict"] as Dictionary<string, HashSet<string>>;
                var path = operands[0] as string;
                if (null != dict && !string.IsNullOrEmpty(path)) {
                    HashSet<string> refbyset;
                    if (dict.TryGetValue(path, out refbyset)) {
                        r = refbyset.ToArray();
                    } else {
                        r = new List<string>();
                    }
                }
            }
            return r;
        }
    }
    internal class GetReferenceByAssetsExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = Calculator.NamedVariables["refbydict"] as Dictionary<string, HashSet<string>>;
                var path = operands[0] as string;
                if (null != dict && !string.IsNullOrEmpty(path)) {
                    HashSet<string> refbyset;
                    if (dict.TryGetValue(path, out refbyset)) {
                        r = refbyset.ToArray();
                    } else {
                        r = new List<string>();
                    }
                }
            }
            return r;
        }
    }
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
    internal class CollectMeshInfoExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.GameObject;
                ModelImporter importer = null;
                if (operands.Count >= 2)
                    importer = operands[1] as ModelImporter;
                if (null != obj) {
                    var info = new MeshInfo();
                    int vc = 0;
                    int tc = 0;
                    int bc = 0;
                    int mc = 0;
                    var skinnedrenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    info.skinnedMeshCount = skinnedrenderers.Length;
                    foreach (var renderer in skinnedrenderers) {
                        if (null != renderer.sharedMesh) {
                            vc += renderer.sharedMesh.vertexCount;
                            tc += renderer.sharedMesh.triangles.Length / 3;
                        }
                        bc += renderer.bones.Length;
                        mc += renderer.sharedMaterials.Length;
                    }
                    var filters = obj.GetComponentsInChildren<MeshFilter>();
                    info.meshFilterCount = filters.Length;
                    foreach (var filter in filters) {
                        if (null != filter.sharedMesh) {
                            vc += filter.sharedMesh.vertexCount;
                            tc += filter.sharedMesh.triangles.Length / 3;
                        }
                    }
                    var meshrenderers = obj.GetComponentsInChildren<MeshRenderer>();
                    foreach (var renderer in meshrenderers) {
                        mc += renderer.sharedMaterials.Length;
                    }
                    info.vertexCount = vc;
                    info.triangleCount = tc;
                    info.boneCount = bc;
                    info.materialCount = mc;
                    var animator = obj.GetComponentInChildren<Animator>();
                    if (null != animator) {
                        var ctrl = animator.runtimeAnimatorController;
                        if (null != ctrl) {
                            info.clipCount = ctrl.animationClips.Length;
                            int gMaxKfc = 0;
                            string gCurveName = string.Empty;
                            string gClipName = string.Empty;
                            foreach (var clip in ctrl.animationClips) {
                                var clipInfo = new AnimationClipInfo();
                                clipInfo.clipName = clip.name;
                                var bindings = AnimationUtility.GetCurveBindings(clip);
                                int maxKfc = 0;
                                string curveName = string.Empty;
                                foreach (var binding in bindings) {
                                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                                    int kfc = curve.keys.Length;
                                    clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                                    if (maxKfc < kfc) {
                                        maxKfc = kfc;
                                        curveName = binding.path + "/" + binding.propertyName;
                                    }
                                }
                                clipInfo.maxKeyFrameCount = maxKfc;
                                clipInfo.maxKeyFrameCurveName = curveName;
                                info.clips.Add(clipInfo);

                                if (gMaxKfc < maxKfc) {
                                    gMaxKfc = maxKfc;
                                    gCurveName = curveName;
                                    gClipName = clip.name;
                                }
                            }
                            info.maxKeyFrameCount = gMaxKfc;
                            info.maxKeyFrameCurveName = gCurveName;
                            info.maxKeyFrameClipName = gClipName;
                        }
                    }
                    if (null != importer && info.clipCount <= 0) {
                        info.clipCount = importer.clipAnimations.Length;
                        if (info.clipCount <= 0)
                            info.clipCount = importer.defaultClipAnimations.Length;
                        int gMaxKfc = 0;
                        string gCurveName = string.Empty;
                        string gClipName = string.Empty;
                        var objs = AssetDatabase.LoadAllAssetsAtPath(importer.assetPath);
                        foreach (var clipObj in objs) {
                            var clip = clipObj as AnimationClip;
                            if (null != clip) {
                                if (importer.clipAnimations.Length > 0) {
                                    bool isDefault = false;
                                    foreach (var ci in importer.defaultClipAnimations) {
                                        if (ci.name == clip.name) {
                                            isDefault = true;
                                            break;
                                        }
                                    }
                                    if (isDefault)
                                        continue;
                                }
                                var clipInfo = new AnimationClipInfo();
                                clipInfo.clipName = clip.name;
                                var bindings = AnimationUtility.GetCurveBindings(clip);
                                int maxKfc = 0;
                                string curveName = string.Empty;
                                foreach (var binding in bindings) {
                                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                                    int kfc = curve.keys.Length;
                                    clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                                    if (maxKfc < kfc) {
                                        maxKfc = kfc;
                                        curveName = binding.path + "/" + binding.propertyName;
                                    }
                                }
                                clipInfo.maxKeyFrameCount = maxKfc;
                                clipInfo.maxKeyFrameCurveName = curveName;
                                info.clips.Add(clipInfo);

                                if (gMaxKfc < maxKfc) {
                                    gMaxKfc = maxKfc;
                                    gCurveName = curveName;
                                    gClipName = clip.name;
                                }
                            }
                        }
                        info.maxKeyFrameCount = gMaxKfc;
                        info.maxKeyFrameCurveName = gCurveName;
                        info.maxKeyFrameClipName = gClipName;
                    }
                    r = info;
                }
            }
            return r;
        }
    }
    internal class CollectAnimatorControllerInfoExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var ctrl = operands[0] as RuntimeAnimatorController;
                if (null != ctrl) {
                    var info = new AnimatorControllerInfo();
                    var editorCtrl = ctrl as UnityEditor.Animations.AnimatorController;
                    if (null == editorCtrl) {
                        var overrideCtrl = ctrl as AnimatorOverrideController;
                        if (null != overrideCtrl) {
                            editorCtrl = overrideCtrl.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
                        }
                    }
                    if (null != editorCtrl) {
                        info.layerCount = editorCtrl.layers.Length;
                        info.paramCount = editorCtrl.parameters.Length;
                        int sc = 0;
                        int smc = 0;
                        foreach (var layer in editorCtrl.layers) {
                            sc += CalcStateCount(layer.stateMachine);
                            smc += CalcSubStateMachineCount(layer.stateMachine);
                        }
                        info.stateCount = sc;
                        info.subStateMachineCount = smc;
                    }
                    int gMaxKfc = 0;
                    string gCurveName = string.Empty;
                    string gClipName = string.Empty;
                    foreach (var clip in ctrl.animationClips) {
                        var clipInfo = new AnimationClipInfo();
                        clipInfo.clipName = clip.name;
                        var bindings = AnimationUtility.GetCurveBindings(clip);
                        int maxKfc = 0;
                        string curveName = string.Empty;
                        foreach (var binding in bindings) {
                            var curve = AnimationUtility.GetEditorCurve(clip, binding);
                            int kfc = curve.keys.Length;
                            clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                            if (maxKfc < kfc) {
                                maxKfc = kfc;
                                curveName = binding.path + "/" + binding.propertyName;
                            }
                        }
                        clipInfo.maxKeyFrameCount = maxKfc;
                        clipInfo.maxKeyFrameCurveName = curveName;
                        info.clips.Add(clipInfo);

                        if (gMaxKfc < maxKfc) {
                            gMaxKfc = maxKfc;
                            gCurveName = curveName;
                            gClipName = clip.name;
                        }
                    }
                    info.maxKeyFrameCount = gMaxKfc;
                    info.maxKeyFrameCurveName = gCurveName;
                    info.maxKeyFrameClipName = gClipName;
                    r = info;
                }
            }
            return r;
        }
        private static int CalcStateCount(AnimatorStateMachine sm)
        {
            int ct = 0;
            ct += sm.states.Length;
            foreach (var ssm in sm.stateMachines) {
                ct += CalcStateCount(ssm.stateMachine);
            }
            return ct;
        }
        private static int CalcSubStateMachineCount(AnimatorStateMachine sm)
        {
            int ct = 0;
            ct += sm.stateMachines.Length;
            foreach (var ssm in sm.stateMachines) {
                ct += CalcSubStateMachineCount(ssm.stateMachine);
            }
            return ct;
        }
    }
    internal class GetAnimationClipInfoExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 0) {
                string assetPath = Calculator.NamedVariables["assetpath"] as string;
                if (!string.IsNullOrEmpty(assetPath)) {
                    var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    if (null != obj) {
                        var clip = obj as AnimationClip;
                        if (null != clip) {
                            var clipInfo = new AnimationClipInfo();
                            clipInfo.clipName = clip.name;
                            var bindings = AnimationUtility.GetCurveBindings(clip);
                            int maxKfc = 0;
                            string curveName = string.Empty;
                            foreach (var binding in bindings) {
                                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                                int kfc = curve.keys.Length;
                                clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                                if (maxKfc < kfc) {
                                    maxKfc = kfc;
                                    curveName = binding.path + "/" + binding.propertyName;
                                }
                            }
                            clipInfo.maxKeyFrameCount = maxKfc;
                            clipInfo.maxKeyFrameCurveName = curveName;
                            r = clipInfo;
                        }
                        Resources.UnloadAsset(obj);
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
        protected static float ToFloat(object v)
        {
            return (float)Convert.ChangeType(v, typeof(float));
        }
        protected static int ToInt(object v)
        {
            return (int)Convert.ChangeType(v, typeof(int));
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
            object v = ToLong(v1) << ToInt(v2);
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
            object v = ToLong(v1) >> ToInt(v2);
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
            object v = ToLong(v1) != 0 ? v2 = m_Op2.Calc(args) : v3 = m_Op3.Calc(args);
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
                    if (ToLong(condVal) != 0) {
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
                if (ToLong(condVal) != 0) {
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
    internal sealed class FormatExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            string fmt = string.Empty;
            ArrayList al = new ArrayList();
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc(args);
                if (ix == 0)
                    fmt = v as string;
                else
                    al.Add(v);
            }
            v = Utility.Format(fmt, al.ToArray());
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
                    ret = Type.GetType("UnityEngine." + type + ", UnityEngine");
                    if (null == ret) {
                        ret = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                    }
                    if (null == ret) {
                        ret = Type.GetType(type + ", Assembly-CSharp");
                    }
                    if (null == ret) {
                        ret = Type.GetType(type);
                    }
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
                        Type t = Type.GetType("UnityEngine." + type + ", UnityEngine");
                        if (null == t) {
                            t = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
                        }
                        if (null == t) {
                            t = Type.GetType(type + ", Assembly-CSharp");
                        }
                        if (null == t) {
                            t = Type.GetType(type);
                        }
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
    internal sealed class LinqExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            object list = m_List.Calc(args);
            string method = m_Method.Calc(args) as string;
            IEnumerable obj = list as IEnumerable;
            if (null != obj && !string.IsNullOrEmpty(method)) {
                if (method == "orderby" || method == "orderbydesc") {
                    bool desc = method == "orderbydesc";
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;
                        results.Add(val);
                    }
                    results.Sort((object o1, object o2) => {
                        Calculator.NamedVariables["$$"] = o1;
                        object r1 = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r1 = m_Expressions[index].Calc(args);
                        }
                        Calculator.NamedVariables["$$"] = o2;
                        object r2 = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r2 = m_Expressions[index].Calc(args);
                        }
                        string rs1 = r1 as string;
                        string rs2 = r2 as string;
                        int r = 0;
                        if (null != rs1 && null != rs2) {
                            r = rs1.CompareTo(rs2);
                        } else {
                            double rd1 = ToDouble(r1);
                            double rd2 = ToDouble(r2);
                            r = rd1.CompareTo(rd2);
                        }
                        if (desc)
                            r = -r;
                        return r;
                    });
                    v = results;
                } else if (method == "where") {
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;

                        Calculator.NamedVariables["$$"] = val;
                        object r = null;
                        for (int index = 0; index < m_Expressions.Count; ++index) {
                            r = m_Expressions[index].Calc(args);
                        }
                        if (ToLong(r) != 0) {
                            results.Add(val);
                        }
                    }
                    v = results;
                } else if (method == "top") {
                    object r = null;
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        r = m_Expressions[index].Calc(args);
                    }
                    long ct = ToLong(r);
                    List<object> results = new List<object>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        object val = enumer.Current;
                        if (ct > 0) {
                            results.Add(val);
                            --ct;
                        }
                    }
                    v = results;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.ISyntaxComponent list = callData.GetParam(0);
            m_List = Calculator.Load(list);
            Dsl.ISyntaxComponent method = callData.GetParam(1);
            m_Method = Calculator.Load(method);
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private IExpression m_List;
        private IExpression m_Method;
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
    internal class GetComponentsExp : SimpleExpressionBase
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
                        r = obj.GetComponents(t);
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
    internal class NewStringBuilderExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 0) {
                r = new StringBuilder();
            }
            return r;
        }
    }
    internal class AppendFormatExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var sb = operands[0] as StringBuilder;
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i] as string;
                    else
                        al.Add(operands[i]);
                }
                if (null != sb && !string.IsNullOrEmpty(fmt)) {
                    Utility.AppendFormat(sb, fmt, al.ToArray());
                    r = sb;
                }
            }
            return r;
        }
    }
    internal class AppendLineFormatExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var sb = operands[0] as StringBuilder;
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i] as string;
                    else
                        al.Add(operands[i]);
                }
                if (null != sb) {
                    if (string.IsNullOrEmpty(fmt)) {
                        sb.AppendLine();
                    } else {
                        Utility.AppendFormat(sb, fmt, al.ToArray());
                        sb.AppendLine();
                    }
                    r = sb;
                }
            }
            return r;
        }
    }
    internal class StringBuilderToStringExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var sb = operands[0] as StringBuilder;
                if (null != sb) {
                    r = sb.ToString();
                }
            }
            return r;
        }
    }
    internal class StringJoinExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var sep = operands[0] as string;
                var list = operands[1] as IList;
                if (null != sep && null != list) {
                    string[] strs = new string[list.Count];
                    for (int i = 0; i < list.Count; ++i) {
                        strs[i] = list[i].ToString();
                    }
                    r = string.Join(sep, strs);
                }
            }
            return r;
        }
    }
    internal class StringSplitExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var str = operands[0] as string;
                var seps = operands[1] as IList;
                if (!string.IsNullOrEmpty(str) && null != seps) {
                    char[] cs = new char[seps.Count];
                    for (int i = 0; i < seps.Count; ++i) {
                        string sep = seps[i].ToString();
                        if (sep.Length > 0) {
                            cs[i] = sep[0];
                        } else {
                            cs[i] = '\0';
                        }
                    }
                    str.Split(cs);
                }
            }
            return r;
        }
    }
    internal class ListSizeExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 1) {
                var list = operands[0] as IList;
                if (null != list) {
                    r = list.Count;
                }
            }
            return r;
        }
    }
    internal class ListExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            ArrayList al = new ArrayList();
            for (int i = 0; i < operands.Count; ++i) {
                al.Add(operands[i]);
            }
            r = al;
            return r;
        }
    }
    internal class ListGetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object defVal = null;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        r = list[index];
                    } else {
                        r = defVal;
                    }
                }
            }
            return r;
        }
    }
    internal class ListSetExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 3) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object val = operands[2];
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        list[index] = val;
                    }
                }
            }
            return r;
        }
    }
    internal class ListIndexOfExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    r = list.IndexOf(val);
                }
            }
            return r;
        }
    }
    internal class ListAddExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    list.Add(val);
                }
            }
            return r;
        }
    }
    internal class ListRemoveExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                object val = operands[1];
                if (null != list) {
                    list.Remove(val);
                }
            }
            return r;
        }
    }
    internal class ListInsertExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 3) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                object val = operands[2];
                if (null != list) {
                    list.Insert(index, val);
                }
            }
            return r;
        }
    }
    internal class ListRemoveAtExp : SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands, object[] args)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as IList;
                var index = ToInt(operands[1]);
                if (null != list) {
                    list.RemoveAt(index);
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
            Register("format", new ExpressionFactoryHelper<FormatExp>());
            Register("gettype", new ExpressionFactoryHelper<GetTypeExp>());
            Register("changetype", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("dotnetcall", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnetset", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnetget", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("linq", new ExpressionFactoryHelper<LinqExp>());
            Register("isnull", new ExpressionFactoryHelper<IsNullExp>());
            Register("loadasset", new ExpressionFactoryHelper<LoadAssetExp>());
            Register("unloadasset", new ExpressionFactoryHelper<UnloadAssetExp>());
            Register("getcomponent", new ExpressionFactoryHelper<GetComponentExp>());
            Register("getcomponents", new ExpressionFactoryHelper<GetComponentsExp>());
            Register("getcomponentinchildren", new ExpressionFactoryHelper<GetComponentInChildrenExp>());
            Register("getcomponentsinchildren", new ExpressionFactoryHelper<GetComponentsInChildrenExp>());
            Register("newstringbuilder", new ExpressionFactoryHelper<NewStringBuilderExp>());
            Register("appendformat", new ExpressionFactoryHelper<AppendFormatExp>());
            Register("appendlineformat", new ExpressionFactoryHelper<AppendLineFormatExp>());
            Register("stringbuildertostring", new ExpressionFactoryHelper<StringBuilderToStringExp>());
            Register("stringjoin", new ExpressionFactoryHelper<StringJoinExp>());
            Register("stringsplit", new ExpressionFactoryHelper<StringSplitExp>());
            Register("listsize", new ExpressionFactoryHelper<ListSizeExp>());
            Register("list", new ExpressionFactoryHelper<ListExp>());
            Register("listget", new ExpressionFactoryHelper<ListGetExp>());
            Register("listset", new ExpressionFactoryHelper<ListSetExp>());
            Register("listindexof", new ExpressionFactoryHelper<ListIndexOfExp>());
            Register("listadd", new ExpressionFactoryHelper<ListAddExp>());
            Register("listremove", new ExpressionFactoryHelper<ListRemoveExp>());
            Register("listinsert", new ExpressionFactoryHelper<ListInsertExp>());
            Register("listremoveat", new ExpressionFactoryHelper<ListRemoveAtExp>());
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

        public Dictionary<string, object> NamedVariables {
            get { return m_NamedVariables; }
        }
        public Dictionary<int, object> Variables {
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
                                    string apiName;
                                    string member = innerCall.GetParamId(0);
                                    if (member == "orderby" || member == "orderbydesc" || member == "where" || member == "top") {
                                        apiName = "linq";
                                    } else {
                                        apiName = "dotnetcall";
                                    }
                                    Dsl.CallData newCall = new Dsl.CallData();
                                    newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
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

                                    if (apiName == "dotnetcall") {
                                        var callExp = new DotnetCallExp();
                                        callExp.Load(newCall, this);
                                        return callExp;
                                    } else {
                                        var callExp = new LinqExp();
                                        callExp.Load(newCall, this);
                                        return callExp;
                                    }
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