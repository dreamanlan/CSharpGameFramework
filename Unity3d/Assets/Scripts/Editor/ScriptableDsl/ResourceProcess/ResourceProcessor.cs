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
#if UNITY_2018_3_OR_NEWER
using UnityEngine.Profiling;
using UnityEditorInternal.Profiling;
#endif
using UnityEditor.Profiling.Memory.Experimental;
using Unity.MemoryProfilerForExtension.Editor;
using Unity.MemoryProfilerForExtension.Editor.UI;
using Unity.MemoryProfilerForExtension.Editor.EnumerationUtilities;
using Unity.MemoryProfilerForExtension.Editor.Database;
using Unity.Profiling;
using GameFramework;

internal sealed class ResourceEditWindow : EditorWindow
{
    [MenuItem("Dsl资源工具/资源处理")]
    internal static void InitWindow()
    {
        ResourceEditWindow window = (ResourceEditWindow)EditorWindow.GetWindow(typeof(ResourceEditWindow));
        window.Init();
        window.Show();
        EditorUtility.ClearProgressBar();
    }

    internal void QueueProcessBegin()
    {
        m_BatchActions.Enqueue(w => w.Focus());
        m_BatchActions.Enqueue(w => w.ClearProcessedAssets());
    }
    internal void QueueProcess(string dslPath, string resPath, int index, int count)
    {
        m_BatchActions.Enqueue(w => BatchAction1(dslPath, resPath));
        m_BatchActions.Enqueue(w => BatchAction2(index, count));
        m_BatchActions.Enqueue(w => BatchAction3(index, count));
    }
    internal void QueueProcessEnd()
    {
        m_BatchActions.Enqueue(w => w.Focus());
    }
    internal void QueueProcess(string dslPath, IDictionary<string, string> args)
    {
        m_BatchActions.Enqueue(w => RedirectAction(dslPath, args));
    }

    private void Init()
    {
        s_CurrentWindow = this;
    }

    private void OnGUI()
    {
        bool oldRichText = GUI.skin.button.richText;
        GUI.skin.button.richText = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清理缓存", EditorStyles.toolbarButton, GUILayout.Width(60))) {
            DeferAction(obj => { ResourceProcessor.Instance.ClearCaches(); });
        }
        EditorGUILayout.LabelField("资源依赖:", EditorStyles.toolbarTextField, GUILayout.Width(60));
        if (GUILayout.Button("分析", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.AnalyseAssets(); });
        }
        if (GUILayout.Button("保存", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.SaveDependencies(); });
        }
        if (GUILayout.Button("加载", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.LoadDependencies(); });
        }
        EditorGUILayout.LabelField("内存:", EditorStyles.toolbarTextField, GUILayout.Width(40));
        if (GUILayout.Button("加载/刷新", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.LoadMemoryInfo(); });
        }
        if (GUILayout.Button("批量转换", EditorStyles.toolbarButton)) {
            DeferAction(obj => { BatchLoadWindow.InitWindow(); });
        }
        EditorGUILayout.LabelField("耗时:", EditorStyles.toolbarTextField, GUILayout.Width(40));
        if (GUILayout.Button("清空", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.ClearInstrumentInfo(); });
        }
        if (m_Record) {
            if (GUILayout.Button("停止", EditorStyles.toolbarButton)) {
                m_Record = false;
            }
        }
        else {
            if (GUILayout.Button("记录", EditorStyles.toolbarButton)) {
                m_Record = true;
            }
        }
        if (GUILayout.Button("保存", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.SaveInstrumentInfo(); });
        }
        if (GUILayout.Button("加载", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.LoadInstrumentInfo(); });
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("刷新列表", EditorStyles.toolbarButton, GUILayout.Width(60))) {
            m_Menus = null;
            m_Files = null;
            m_SelectedIndex = 0;
            DeferAction(obj => { ResourceProcessor.Instance.ClearDsl(); obj.CopyCollectResult(); });
        }

        if (null == m_Menus || m_Menus.Length <= 0) {
            SortedDictionary<string, string> dslFiles = new SortedDictionary<string, string>();
            dslFiles.Add("0.Empty", string.Empty);
            var files = Directory.GetFiles("./FindDsl", "*.dsl", SearchOption.AllDirectories);
            foreach (var file in files) {
                string name, desc;
                if (ResourceProcessor.ReadMenuAndDescription(file, out name, out desc)) {
                    try {
                        dslFiles.Add(string.Format("1.Find/{0}", name), file);
                    }
                    catch (Exception ex) {
                        Debug.LogFormat("Add 'Find' menu {0} desc {1} exception:{2}", name, desc, ex.Message);
                    }
                }
            }
            files = Directory.GetFiles("./ProcessDsl", "*.dsl", SearchOption.AllDirectories);
            foreach (var file in files) {
                string name, desc;
                if (ResourceProcessor.ReadMenuAndDescription(file, out name, out desc)) {
                    try {
                        dslFiles.Add(string.Format("2.Process/{0}", name), file);
                    }
                    catch (Exception ex) {
                        Debug.LogFormat("Add 'Process' menu {0} desc {1} exception:{2}", name, desc, ex.Message);
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
                DeferAction(obj => { ResourceProcessor.Instance.SelectDsl(file); obj.CopyCollectResult(); });
            }
            else {
                DeferAction(obj => { ResourceProcessor.Instance.ClearDsl(); obj.CopyCollectResult(); });
            }
        }

        if (GUILayout.Button("收集资源", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.Collect(); obj.CopyCollectResult(); });
        }
        if (GUILayout.Button("处理选中资源", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.Process(); });
        }
        if (GUILayout.Button("编辑器同步选中", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.SelectAssetsOrObjects(); });
        }
        if (GUILayout.Button("创建场景", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.GenerateScene(); });
        }
        GUILayout.Space(20);
        if (GUILayout.Button("保存", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.SaveResult(); });
        }
        if (GUILayout.Button("加载", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceProcessor.Instance.LoadResult(); obj.CopyCollectResult(); });
        }
        if (GUILayout.Button("拷贝", EditorStyles.toolbarButton)) {
            DeferAction(obj => { obj.CopyToClipboard(); });
        }
        if (GUILayout.Button("命令", EditorStyles.toolbarButton)) {
            DeferAction(obj => { ResourceCommandWindow.InitWindow(obj, string.Empty, null, null); });
        }
        GUILayout.Space(20);
        if (GUILayout.Button("批处理", EditorStyles.toolbarButton)) {
            DeferAction(obj => { BatchResourceProcessWindow.InitWindow(obj); });
        }
        EditorGUILayout.EndHorizontal();

        var paramNames = ResourceProcessor.Instance.ParamNames;
        var paramInfos = ResourceProcessor.Instance.Params;
        if (paramNames.Count > 0) {
            foreach (var name in paramNames) {
                ResourceEditUtility.ParamInfo info;
                if (paramInfos.TryGetValue(name, out info)) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(info.Name, GUILayout.Width(160));
                    string oldVal = info.StringValue;
                    string newVal = oldVal;
                    if (!string.IsNullOrEmpty(info.Script)) {
                        double curTime = EditorApplication.timeSinceStartup;
                        if (info.NextRunScriptTime <= curTime) {
                            var r = ResourceProcessor.Instance.CallScript(null, info.Script, info);
                            if (null != r) {
                                info.NextRunScriptTime = curTime + (double)Convert.ChangeType(r, typeof(double));
                            }
                        }
                    }
                    if (info.OptionStyle == "excel_sheets") {
                        DoPopup(info, oldVal, ref newVal);
                    }
                    else if (info.OptionStyle == "managed_memory_group") {
                        if (info.OptionNames.Count != ResourceProcessor.Instance.ClassifiedManagedMemoryInfos.Count) {
                            info.PopupOptionNames = null;
                            info.OptionNames.Clear();
                            info.Options.Clear();
                            foreach (var pair in ResourceProcessor.Instance.ClassifiedManagedMemoryInfos) {
                                info.OptionNames.Add(pair.Key);
                                info.Options.Add(pair.Key, pair.Key);
                            }
                        }
                        DoPopup(info, oldVal, ref newVal);
                    }
                    else if (info.OptionStyle == "native_memory_group") {
                        if (info.OptionNames.Count != ResourceProcessor.Instance.ClassifiedNativeMemoryInfos.Count) {
                            info.PopupOptionNames = null;
                            info.OptionNames.Clear();
                            info.Options.Clear();
                            foreach (var pair in ResourceProcessor.Instance.ClassifiedNativeMemoryInfos) {
                                info.OptionNames.Add(pair.Key);
                                info.Options.Add(pair.Key, pair.Key);
                            }
                        }
                        DoPopup(info, oldVal, ref newVal);
                    }
                    else if (info.Options.Count > 0) {
                        if (info.OptionStyle == "toggle") {
                            DoToggle(info, oldVal, ref newVal);
                        }
                        else if (info.OptionStyle == "multiple") {
                            DoMultiple(info, oldVal, ref newVal);
                        }
                        else {
                            DoPopup(info, oldVal, ref newVal);
                        }
                    }
                    else if (info.Type == typeof(bool)) {
                        bool v = EditorGUILayout.Toggle((bool)info.Value);
                        newVal = v ? "true" : "false";
                    }
                    else if (info.Type == typeof(int)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            int min = (int)info.MinValue;
                            int max = (int)info.MaxValue;
                            int v = EditorGUILayout.IntSlider((int)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            int v = EditorGUILayout.IntField((int)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(uint)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            int min = (int)info.MinValue;
                            int max = (int)info.MaxValue;
                            int v = EditorGUILayout.IntSlider((int)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            uint v = (uint)EditorGUILayout.IntField((int)(uint)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(long)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            int min = (int)info.MinValue;
                            int max = (int)info.MaxValue;
                            int v = EditorGUILayout.IntSlider((int)(long)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            long v = EditorGUILayout.LongField((long)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(ulong)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            int min = (int)info.MinValue;
                            int max = (int)info.MaxValue;
                            int v = EditorGUILayout.IntSlider((int)(long)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            ulong v = (ulong)EditorGUILayout.LongField((long)(ulong)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(float)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            float min = (float)info.MinValue;
                            float max = (float)info.MaxValue;
                            float v = EditorGUILayout.Slider((float)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            float v = EditorGUILayout.FloatField((float)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(double)) {
                        if (null != info.MinValue && null != info.MaxValue) {
                            float min = (float)info.MinValue;
                            float max = (float)info.MaxValue;
                            float v = EditorGUILayout.Slider((float)(double)info.Value, min, max, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                        else {
                            double v = EditorGUILayout.DoubleField((double)info.Value, GUILayout.MaxWidth(1024));
                            newVal = v.ToString();
                        }
                    }
                    else if (info.Type == typeof(UnityEngine.GUIElement)) {
                        if (GUILayout.Button(new GUIContent(string.Format("Return [{0}]", oldVal), oldVal))) {
                            var redirectDsl = oldVal;
                            var redirectArgs = info.Options;
                            QueueProcess(redirectDsl, redirectArgs);
                        }
                    }
                    else if (!string.IsNullOrEmpty(info.FileExts)) {
                        newVal = EditorGUILayout.TextField(oldVal, GUILayout.MaxWidth(1024));
                        if (GUILayout.Button("选择")) {
                            newVal = EditorUtility.OpenFilePanel("选择文件", info.FileInitDir, info.FileExts);
                        }
                    }
                    else {
                        newVal = EditorGUILayout.TextField(oldVal, GUILayout.MaxWidth(1024));
                    }
                    EditorGUILayout.EndHorizontal();
                    if (name == "excel") {
                        ResourceEditUtility.ParamInfo sheetNameInfo;
                        if (paramInfos.TryGetValue("sheetname", out sheetNameInfo)) {
                            if (newVal != oldVal || sheetNameInfo.OptionNames.Count == 0) {
                                sheetNameInfo.PopupOptionNames = null;
                                sheetNameInfo.OptionNames.Clear();
                                sheetNameInfo.Options.Clear();
                                var file = newVal;
                                var path = file;
                                if (!File.Exists(path)) {
                                    path = Path.Combine("../../Product/Excel", file);
                                }
                                if (File.Exists(path)) {
                                    var ext = Path.GetExtension(file);
                                    NPOI.SS.UserModel.IWorkbook book = null;
                                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                                        if (ext == ".xls") {
                                            book = new NPOI.HSSF.UserModel.HSSFWorkbook(stream);
                                        }
                                        else {
                                            book = new NPOI.XSSF.UserModel.XSSFWorkbook(stream);
                                        }
                                        for (int i = 0; i < book.NumberOfSheets; ++i) {
                                            var sheetName = book.GetSheetName(i);
                                            sheetNameInfo.OptionNames.Add(sheetName);
                                            sheetNameInfo.Options.Add(sheetName, sheetName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (newVal != oldVal) {
                        m_EditedParams.Add(name, newVal);
                    }
                }
            }
            if (m_EditedParams.Count > 0) {
                foreach (var pair in m_EditedParams) {
                    ResourceEditUtility.ParamInfo val;
                    if (paramInfos.TryGetValue(pair.Key, out val)) {
                        val.NextRunScriptTime = 0;
                        val.StringValue = pair.Value;
                        if (val.Type == typeof(int)) {
                            int v = int.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                int min = (int)val.MinValue;
                                int max = (int)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        if (val.Type == typeof(uint)) {
                            uint v = uint.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                uint min = (uint)(int)val.MinValue;
                                uint max = (uint)(int)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(long)) {
                            long v = long.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                int min = (int)val.MinValue;
                                int max = (int)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(ulong)) {
                            ulong v = ulong.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                ulong min = (ulong)(int)val.MinValue;
                                ulong max = (ulong)(int)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(float)) {
                            float v = float.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                float min = (float)val.MinValue;
                                float max = (float)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(double)) {
                            double v = double.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                float min = (float)val.MinValue;
                                float max = (float)val.MaxValue;
                                if (v < min) v = min;
                                if (v > max) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(string)) {
                            string v = pair.Value;
                            if (null != val.MinValue && null != val.MaxValue) {
                                string min = (string)val.MinValue;
                                string max = (string)val.MaxValue;
                                if (v.CompareTo(min) < 0) v = min;
                                if (v.CompareTo(min) > 0) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(bool)) {
                            bool v = bool.Parse(pair.Value);
                            if (null != val.MinValue && null != val.MaxValue) {
                                bool min = (bool)val.MinValue;
                                bool max = (bool)val.MaxValue;
                                if (v.CompareTo(min) < 0) v = min;
                                if (v.CompareTo(min) > 0) v = max;
                            }
                            val.Value = v;
                        }
                        else if (val.Type == typeof(List<int>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<int>();
                            foreach (var str in v) {
                                int iv;
                                int.TryParse(str, out iv);
                                list.Add(iv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<uint>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<uint>();
                            foreach (var str in v) {
                                uint iv;
                                uint.TryParse(str, out iv);
                                list.Add(iv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<long>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<long>();
                            foreach (var str in v) {
                                long iv;
                                long.TryParse(str, out iv);
                                list.Add(iv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<ulong>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<ulong>();
                            foreach (var str in v) {
                                ulong iv;
                                ulong.TryParse(str, out iv);
                                list.Add(iv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<float>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<float>();
                            foreach (var str in v) {
                                float fv;
                                float.TryParse(str, out fv);
                                list.Add(fv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<double>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            var list = new List<double>();
                            foreach (var str in v) {
                                double fv;
                                double.TryParse(str, out fv);
                                list.Add(fv);
                            }
                            val.Value = list;
                        }
                        else if (val.Type == typeof(List<string>)) {
                            var v = pair.Value.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                            val.Value = v;
                        }
                        else if (val.Type == typeof(ResourceEditUtility.DataTable)) {
                            val.Value = pair.Value;
                        }
                        else if (val.Type == typeof(NPOI.SS.UserModel.IWorkbook)) {
                            val.Value = pair.Value;
                        }
                        else if (val.Type == typeof(object)) {
                            val.Value = pair.Value;
                        }
                    }
                }
                m_EditedParams.Clear();
            }
        }
        var text = ResourceProcessor.Instance.Text;
        if (m_ItemList.Count <= 0 && !ResourceProcessor.Instance.CanRefresh) {
            if (!string.IsNullOrEmpty(text)) {
                m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true);
                EditorGUILayout.TextArea(text);
                EditorGUILayout.EndScrollView();
            }
        }
        else {
            if (!string.IsNullOrEmpty(text)) {
                EditorGUILayout.TextArea(text, GUILayout.MaxHeight(70));
            }
            if (m_IsReady) {
                if (m_ItemList.Count <= 0) {
                    if (GUILayout.Button("Refresh")) {
                        DeferAction(obj => { ResourceProcessor.Instance.Refresh(); obj.CopyCollectResult(); });
                    }
                }
                else {
                    ResourceEditUtility.ParamInfo val;
                    if (paramInfos.TryGetValue("pathwidth", out val)) {
                        m_PathWidth = (float)Convert.ChangeType(val.Value, typeof(float));
                    }
                    if (m_UnfilteredGroupCount <= 0) {
                        ListItem();
                    }
                    else {
                        ListGroupedItem();
                    }
                }
            }
        }
        GUI.skin.button.richText = oldRichText;

        if (m_Record) {
            ResourceProcessor.Instance.RecordInstrument();
        }

        ExecuteDeferredActions();
        ExecuteBatchActions();
    }
    private void DoToggle(ResourceEditUtility.ParamInfo info, string oldVal, ref string newVal)
    {
        bool changed = false;
        foreach (var key in info.OptionNames) {
            string val;
            if (info.Options.TryGetValue(key, out val)) {
                if (changed) {
                    EditorGUILayout.Toggle(key, false);
                }
                else {
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
    }
    private void DoMultiple(ResourceEditUtility.ParamInfo info, string oldVal, ref string newVal)
    {
        if (null == info.MultipleOldValues) {
            info.MultipleOldValues = new List<string>(oldVal.Split('|'));
            info.MultipleNewValues = new List<string>();
        }
        else {
            info.MultipleNewValues.Clear();
        }
        bool changed = false;
        foreach (var key in info.OptionNames) {
            string val;
            if (info.Options.TryGetValue(key, out val)) {
                bool toggle = info.MultipleOldValues.IndexOf(val) >= 0;
                if (EditorGUILayout.Toggle(key, toggle)) {
                    info.MultipleNewValues.Add(val);
                    if (!toggle)
                        changed = true;
                }
                else if (toggle) {
                    changed = true;
                }
            }
        }
        if (changed) {
            newVal = string.Join("|", info.MultipleNewValues.ToArray());
            info.MultipleOldValues.Clear();
            info.MultipleOldValues.AddRange(info.MultipleNewValues);
        }
    }
    private void DoPopup(ResourceEditUtility.ParamInfo info, string oldVal, ref string newVal)
    {
        int ix = 0;
        if (null == info.PopupOptionNames || info.PopupOptionNames.Length != info.OptionNames.Count) {
            info.PopupOptionNames = info.OptionNames.ToArray();
        }
        if (info.OptionNames.Count > 0) {
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
            newIx = EditorGUILayout.Popup(ix, info.PopupOptionNames);
            if (ix == 0 || newIx != ix) {
                newVal = info.Options[info.PopupOptionNames[newIx]];
            }
        }
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
    private void DeferAction(Action<ResourceEditWindow> action)
    {
        m_Actions.Enqueue(action);
    }
    private void ExecuteBatchActions()
    {
        if (m_InBatchActions)
            return;
        try {
            m_InBatchActions = true;
            if (m_BatchActions.Count > 0) {
                var action = m_BatchActions.Dequeue();
                if (null != action) {
                    action(this);
                }
            }
        }
        finally {
            m_InBatchActions = false;
        }
    }

    private void ClearProcessedAssets()
    {
        ResourceProcessor.Instance.ClearProcessedAssets();
    }
    private void BatchAction1(string dslPath, string resPath)
    {
        ResourceProcessor.Instance.SelectDsl(dslPath);
        ResourceProcessor.Instance.CollectPath = resPath;
    }
    private void BatchAction2(int index, int count)
    {
        ResourceProcessor.Instance.OverridedProgressTitle = string.Format("{0}/{1}", index + 1, count);
        ResourceProcessor.Instance.Refresh(true);
        CopyCollectResult();
    }
    private void BatchAction3(int index, int count)
    {
        ResourceProcessor.Instance.OverridedProgressTitle = string.Format("{0}/{1}", index + 1, count);
        ResourceProcessor.Instance.SelectAll();
        ResourceProcessor.Instance.Process(true);
    }
    private void RedirectAction(string dslPath, IDictionary<string, string> args)
    {
        ResourceProcessor.Instance.SelectDsl(dslPath);
        ResourceProcessor.Instance.CollectPath = string.Empty;
        var paramNames = ResourceProcessor.Instance.ParamNames;
        var paramInfos = ResourceProcessor.Instance.Params;
        foreach (var pair in args) {
            var name = pair.Key;
            var val = pair.Value;
            ResourceEditUtility.ParamInfo info;
            if (paramInfos.TryGetValue(name, out info)) {
                if (info.Type == typeof(bool)) {
                    info.Value = bool.Parse(val);
                }
                else if (info.Type == typeof(int)) {
                    info.Value = int.Parse(val);
                }
                else if (info.Type == typeof(float)) {
                    info.Value = float.Parse(val);
                }
                else {
                    info.Value = val;
                }
            }
        }
        ResourceProcessor.Instance.Refresh(true);
        CopyCollectResult();
    }

    private void CopyCollectResult()
    {
        m_IsReady = false;
        try {
            m_UnfilteredGroupCount = ResourceProcessor.Instance.UnfilteredGroupCount;
            m_ItemList.Clear();
            m_ItemList.AddRange(ResourceProcessor.Instance.ItemList);
            m_GroupList.Clear();
            m_GroupList.AddRange(ResourceProcessor.Instance.GroupList);
            m_TotalItemValue = ResourceProcessor.Instance.TotalItemValue;
        }
        finally {
            m_IsReady = true;
        }
    }
    private void CopyToClipboard()
    {
        var sb = new StringBuilder();
        if (m_GroupList.Count > 0) {
            sb.AppendLine("asset_path\tscene_path\tinfo\torder\tvalue");
            int curCount = 0;
            int totalCount = m_GroupList.Count;
            foreach (var item in m_GroupList) {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}", item.AssetPath, item.ScenePath, item.Info, item.Order, item.Value);
                sb.AppendLine();
                ++curCount;
                if (ResourceProcessor.Instance.DisplayCancelableProgressBar("拷贝进度", curCount, totalCount)) {
                    break;
                }
            }
        }
        else {
            sb.AppendLine("asset_path\tscene_path\tinfo\torder\tvalue");
            int curCount = 0;
            int totalCount = m_ItemList.Count;
            foreach (var item in m_ItemList) {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}", item.AssetPath, item.ScenePath, item.Info, item.Order, item.Value);
                sb.AppendLine();
                ++curCount;
                if (ResourceProcessor.Instance.DisplayCancelableProgressBar("拷贝进度", curCount, totalCount)) {
                    break;
                }
            }
        }
        EditorUtility.ClearProgressBar();

        GUIUtility.systemCopyBuffer = sb.ToString();
    }

    private void Sort(bool asc)
    {
        m_ItemList.Sort((a, b) => {
            int v;
            if (a.Order < b.Order)
                v = -1;
            else if (a.Order > b.Order)
                v = 1;
            else {
                if (!string.IsNullOrEmpty(a.AssetPath) && !string.IsNullOrEmpty(b.AssetPath)) {
                    v = string.CompareOrdinal(a.AssetPath, b.AssetPath);
                }
                else if (!string.IsNullOrEmpty(a.ScenePath) && !string.IsNullOrEmpty(b.ScenePath)) {
                    v = string.CompareOrdinal(a.ScenePath, b.ScenePath);
                }
                else if(!string.IsNullOrEmpty(a.Info) && !string.IsNullOrEmpty(b.Info)) {
                    v = string.CompareOrdinal(a.Info, b.Info);
                }
                else {
                    v = 0;
                }
            }
            if (!asc)
                v = -v;
            return v;
        });
    }
    private void GroupSort(bool asc)
    {
        m_GroupList.Sort((a, b) => {
            int v;
            if (a.Order < b.Order)
                v = -1;
            else if (a.Order > b.Order)
                v = 1;
            else {
                if (!string.IsNullOrEmpty(a.AssetPath) && !string.IsNullOrEmpty(b.AssetPath)) {
                    v = string.CompareOrdinal(a.AssetPath, b.AssetPath);
                }
                else if (!string.IsNullOrEmpty(a.ScenePath) && !string.IsNullOrEmpty(b.ScenePath)) {
                    v = string.CompareOrdinal(a.ScenePath, b.ScenePath);
                }
                else if (!string.IsNullOrEmpty(a.Info) && !string.IsNullOrEmpty(b.Info)) {
                    v = string.CompareOrdinal(a.Info, b.Info);
                }
                else {
                    v = 0;
                }
            }
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
            DeferAction(obj => { ResourceProcessor.Instance.Refresh(); obj.CopyCollectResult(); });
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
        if (null != m_SelectedItem && null == m_SelectedItem.ExtraList && !string.IsNullOrEmpty(m_SelectedItem.ExtraListBuildScript)) {
            m_SelectedItem.ExtraList = ResourceProcessor.Instance.CallScript(null, m_SelectedItem.ExtraListBuildScript, m_SelectedItem) as IList<KeyValuePair<string, object>>;
        }
        if (!string.IsNullOrEmpty(m_SelectedAssetPath) && (ResourceProcessor.Instance.ReferenceAssets.Count > 0 || ResourceProcessor.Instance.ReferenceByAssets.Count > 0) ||
            null != m_SelectedItem && null != m_SelectedItem.ExtraList) {
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
            bool assetNotEmpty = !string.IsNullOrEmpty(item.AssetPath);
            bool sceneNotEmpty = !string.IsNullOrEmpty(item.ScenePath);
            string buttonName = string.Empty;
            if (assetNotEmpty && sceneNotEmpty)
                buttonName = string.Format("{0},{1}", item.AssetPath, item.ScenePath);
            else if (assetNotEmpty)
                buttonName = item.AssetPath;
            else if (sceneNotEmpty)
                buttonName = item.ScenePath;
            if (!string.IsNullOrEmpty(item.RedirectDsl) && GUILayout.Button(new GUIContent("Go", item.RedirectDsl), GUILayout.MinWidth(30), GUILayout.MinWidth(30))) {
                QueueProcess(item.RedirectDsl, item.RedirectArgs);
            }
            if (!string.IsNullOrEmpty(buttonName)) {
                var minWidth = GUILayout.MinWidth(80);
                var maxWidth = GUILayout.MaxWidth(m_PathWidth);
                if (ResourceProcessor.Instance.SearchSource == "sceneobjects" || ResourceProcessor.Instance.SearchSource == "scenecomponents") {
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(buttonName, buttonName), minWidth, maxWidth)) {
                        DeferAction(obj => {
                            if (null != item.Object)
                                ResourceEditUtility.SelectObject(item.Object);
                            else
                                ResourceEditUtility.SelectSceneObject(item.ScenePath);
                            if (!string.IsNullOrEmpty(item.AssetPath)) {
                                ResourceEditUtility.SelectProjectObject(item.AssetPath);
                                obj.m_SelectedAssetPath = item.AssetPath;
                            }
                            else {
                                obj.m_SelectedAssetPath = string.Empty;
                            }
                            obj.m_SelectedItem = item;
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                }
                else {
                    Texture icon = AssetDatabase.GetCachedIcon(item.AssetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(buttonName, icon, buttonName), minWidth, maxWidth)) {
                        DeferAction(obj => {
                            if (null != item.Object)
                                ResourceEditUtility.SelectObject(item.Object);
                            else
                                ResourceEditUtility.SelectProjectObject(item.AssetPath);
                            obj.m_SelectedAssetPath = item.AssetPath;
                            obj.m_SelectedItem = item;
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                }
            }
            else {
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(0));
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
            HashSet<string> refSet;
            if (ResourceProcessor.Instance.ReferenceAssets.TryGetValue(m_SelectedAssetPath, out refSet)) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("References:");
                EditorGUILayout.EndHorizontal();
                foreach (string assetPath in refSet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectProjectObject(assetPath);
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            HashSet<string> refBySet;
            if (ResourceProcessor.Instance.ReferenceByAssets.TryGetValue(m_SelectedAssetPath, out refBySet)) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("References by:");
                EditorGUILayout.EndHorizontal();
                foreach (string assetPath in refBySet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectProjectObject(assetPath);
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (null != m_SelectedItem.ExtraList) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Extra List:");
                EditorGUILayout.EndHorizontal();
                foreach (var pair in m_SelectedItem.ExtraList) {
                    var info = pair.Key;
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(info);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(info, icon, info), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        if (!string.IsNullOrEmpty(m_SelectedItem.ExtraListClickScript)) {
                            ResourceProcessor.Instance.CallScript(null, m_SelectedItem.ExtraListClickScript, pair, m_SelectedItem);
                        }
                        else if (pair.Value is ObjectData) {
                            var data = (ObjectData)pair.Value;
                            ResourceProcessor.Instance.OpenLink(data);
                        }
                        else {
                            ResourceCommandWindow.InitWindow(this, info, pair, m_SelectedItem);
                        }
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
    private void ListGroupedItem()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(40))) {
            foreach (var item in m_GroupList) {
                item.Selected = true;
            }
        }
        if (GUILayout.Button("全不选", GUILayout.Width(60))) {
            foreach (var item in m_GroupList) {
                item.Selected = false;
            }
        }
        GUILayout.Label(string.Format("Total count ({0})", m_GroupList.Count), GUILayout.Width(120));
        GUILayout.Label(string.Format("Go to page ({0})", m_GroupList.Count / c_ItemsPerPage + 1), GUILayout.Width(100));
        string strPage = EditorGUILayout.TextField(m_Page.ToString(), GUILayout.Width(40));
        int.TryParse(strPage, out m_Page);
        if (GUILayout.Button("Prev", GUILayout.Width(80))) {
            m_Page--;
        }
        if (GUILayout.Button("Next", GUILayout.Width(80))) {
            m_Page++;
        }
        if (GUILayout.Button("Refresh", GUILayout.Width(60))) {
            DeferAction(obj => { ResourceProcessor.Instance.Refresh(); obj.CopyCollectResult(); });
        }
        if (GUILayout.Button("升序", GUILayout.Width(60))) {
            GroupSort(true);
        }
        if (GUILayout.Button("降序", GUILayout.Width(60))) {
            GroupSort(false);
        }
        GUILayout.Label(string.Format("Total value ({0})", m_TotalItemValue));
        EditorGUILayout.EndHorizontal();
        m_Page = Mathf.Max(1, Mathf.Min(m_GroupList.Count / c_ItemsPerPage + 1, m_Page));
        bool showReferences = false;
        float rightWidth = 0;
        if (null != m_SelectedGroup && null == m_SelectedGroup.ExtraList && !string.IsNullOrEmpty(m_SelectedGroup.ExtraListBuildScript)) {
            m_SelectedGroup.ExtraList = ResourceProcessor.Instance.CallScript(null, m_SelectedGroup.ExtraListBuildScript, m_SelectedGroup) as IList<KeyValuePair<string, object>>;
        }
        if (!string.IsNullOrEmpty(m_SelectedAssetPath) && (ResourceProcessor.Instance.ReferenceAssets.Count > 0 || ResourceProcessor.Instance.ReferenceByAssets.Count > 0) ||
            null != m_SelectedGroup && null != m_SelectedGroup.ExtraList) {
            showReferences = true;
            rightWidth = m_RightWidth;
        }
        float windowWidth = position.width;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        m_PanelPos = EditorGUILayout.BeginScrollView(m_PanelPos, true, true, GUILayout.Width(windowWidth - rightWidth));
        int index = 0;
        int totalShown = 0;
        foreach (var item in m_GroupList) {
            ++index;
            if (index <= (m_Page - 1) * c_ItemsPerPage)
                continue;
            ++totalShown;
            if (totalShown > c_ItemsPerPage)
                break;
            EditorGUILayout.BeginHorizontal();
            item.Selected = GUILayout.Toggle(item.Selected, index + ".", GUILayout.Width(60));
            bool assetNotEmpty = !string.IsNullOrEmpty(item.AssetPath);
            bool sceneNotEmpty = !string.IsNullOrEmpty(item.ScenePath);
            string buttonName = string.Empty;
            if (assetNotEmpty && sceneNotEmpty)
                buttonName = string.Format("{0},{1}", item.AssetPath, item.ScenePath);
            else if (assetNotEmpty)
                buttonName = item.AssetPath;
            else if (sceneNotEmpty)
                buttonName = item.ScenePath;
            if (!string.IsNullOrEmpty(item.RedirectDsl) && GUILayout.Button(new GUIContent("Go", item.RedirectDsl), GUILayout.MinWidth(30), GUILayout.MinWidth(30))) {
                QueueProcess(item.RedirectDsl, item.RedirectArgs);
            }
            if (!string.IsNullOrEmpty(buttonName)) {
                var minWidth = GUILayout.MinWidth(80);
                var maxWidth = GUILayout.MaxWidth(m_PathWidth);
                if (ResourceProcessor.Instance.SearchSource == "sceneobjects" || ResourceProcessor.Instance.SearchSource == "scenecomponents") {
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(buttonName, buttonName), minWidth, maxWidth)) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectSceneObject(item.ScenePath);
                            if (!string.IsNullOrEmpty(item.AssetPath)) {
                                ResourceEditUtility.SelectProjectObject(item.AssetPath);
                                obj.m_SelectedAssetPath = item.AssetPath;
                            }
                            else {
                                obj.m_SelectedAssetPath = string.Empty;
                            }
                            obj.m_SelectedGroup = item;
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                }
                else {
                    Texture icon = AssetDatabase.GetCachedIcon(item.AssetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(buttonName, icon, buttonName), minWidth, maxWidth)) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectProjectObject(item.AssetPath);
                            obj.m_SelectedAssetPath = item.AssetPath;
                            obj.m_SelectedGroup = item;
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                }
            }
            else {
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(0));
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
            if (ResourceProcessor.Instance.ReferenceAssets.TryGetValue(m_SelectedAssetPath, out refSet)) {
                foreach (string assetPath in refSet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectProjectObject(assetPath);
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("References by:");
            EditorGUILayout.EndHorizontal();
            HashSet<string> refBySet;
            if (ResourceProcessor.Instance.ReferenceByAssets.TryGetValue(m_SelectedAssetPath, out refBySet)) {
                foreach (string assetPath in refBySet) {
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(assetPath, icon, assetPath), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        DeferAction(obj => {
                            ResourceEditUtility.SelectProjectObject(assetPath);
                        });
                    }
                    GUI.skin.button.alignment = oldAlignment;
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (null != m_SelectedGroup.ExtraList) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Extra List:");
                EditorGUILayout.EndHorizontal();
                foreach (var pair in m_SelectedGroup.ExtraList) {
                    var info = pair.Key;
                    EditorGUILayout.BeginHorizontal();
                    Texture icon = AssetDatabase.GetCachedIcon(info);
                    var oldAlignment = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    if (GUILayout.Button(new GUIContent(info, icon, info), GUILayout.MinWidth(80), GUILayout.MaxWidth(rightWidth))) {
                        if (!string.IsNullOrEmpty(m_SelectedGroup.ExtraListClickScript)) {
                            ResourceProcessor.Instance.CallScript(null, m_SelectedGroup.ExtraListClickScript, pair, m_SelectedGroup);
                        }
                        else if (pair.Value is ObjectData) {
                            var data = (ObjectData)pair.Value;
                            ResourceProcessor.Instance.OpenLink(data);
                        }
                        else {
                            ResourceCommandWindow.InitWindow(this, info, pair, m_SelectedGroup);
                        }
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

    private string[] m_Menus = null;
    private string[] m_Files = null;
    private int m_SelectedIndex = 0;

    private List<ResourceEditUtility.ItemInfo> m_ItemList = new List<ResourceEditUtility.ItemInfo>();
    private List<ResourceEditUtility.GroupInfo> m_GroupList = new List<ResourceEditUtility.GroupInfo>();
    private int m_UnfilteredGroupCount = 0;
    private double m_TotalItemValue = 0;

    private Vector2 m_PanelPos = Vector2.zero;
    private Vector2 m_PanelPosRight = Vector2.zero;
    private float m_PathWidth = 240;
    private float m_RightWidth = 240;
    private Dictionary<string, string> m_EditedParams = new Dictionary<string, string>();
    private int m_Page = 1;
    private string m_SelectedAssetPath = string.Empty;
    private ResourceEditUtility.ItemInfo m_SelectedItem = null;
    private ResourceEditUtility.GroupInfo m_SelectedGroup = null;

    [NonSerialized]
    private bool m_Record = false;

    private bool m_IsReady = false;
    private bool m_InActions = false;
    private bool m_InBatchActions = false;
    private Queue<Action<ResourceEditWindow>> m_Actions = new Queue<Action<ResourceEditWindow>>();
    private Queue<Action<ResourceEditWindow>> m_BatchActions = new Queue<Action<ResourceEditWindow>>();

    private static ResourceEditWindow s_CurrentWindow = null;

    private const int c_ItemsPerPage = 50;
}

internal sealed class ResourceProcessor
{
    internal string OverridedProgressTitle
    {
        get { return m_OverridedProgressTitle; }
        set { m_OverridedProgressTitle = value; }
    }
    internal string DslMenu
    {
        get { return m_DslMenu; }
    }
    internal string DslDescription
    {
        get { return m_DslDescription; }
    }
    internal string SearchSource
    {
        get { return m_SearchSource; }
    }
    internal List<string> ParamNames
    {
        get { return m_ParamNames; }
    }
    internal Dictionary<string, ResourceEditUtility.ParamInfo> Params
    {
        get { return m_Params; }
    }
    internal bool CanRefresh
    {
        get { return m_CanRefresh; }
    }
    internal string Text
    {
        get { return m_Text; }
    }
    internal string CollectPath
    {
        get { return m_CollectPath; }
        set { m_CollectPath = value; }
    }
    internal List<ResourceEditUtility.ItemInfo> ItemList
    {
        get { return m_ItemList; }
    }
    internal List<ResourceEditUtility.GroupInfo> GroupList
    {
        get { return m_GroupList; }
    }
    internal int UnfilteredGroupCount
    {
        get { return m_UnfilteredGroupCount; }
    }
    internal double TotalItemValue
    {
        get { return m_TotalItemValue; }
    }
    internal IDictionary<string, HashSet<string>> ReferenceAssets
    {
        get { return m_ReferenceAssets; }
    }
    internal IDictionary<string, HashSet<string>> ReferenceByAssets
    {
        get { return m_ReferenceByAssets; }
    }
    internal IDictionary<string, ResourceEditUtility.MemoryGroupInfo> ClassifiedNativeMemoryInfos
    {
        get {
            return m_ClassifiedNativeMemoryInfos;
        }
    }
    internal IDictionary<string, ResourceEditUtility.MemoryGroupInfo> ClassifiedManagedMemoryInfos
    {
        get {
            return m_ClassifiedManagedMemoryInfos;
        }
    }
    internal IDictionary<int, ResourceEditUtility.InstrumentInfo> InstrumentInfos
    {
        get {
            return m_InstrumentInfos;
        }
    }
    internal void ClearProcessedAssets()
    {
        m_ProcessedAssets.Clear();
    }
    internal void SelectAssetsOrObjects()
    {
        var list = new List<UnityEngine.Object>();
        foreach (var item in m_ItemList) {
            if (item.Selected) {
                if (m_SearchSource == "sceneobjects" || m_SearchSource == "scenecomponents") {
                    if (null != item.Object)
                        list.Add(item.Object);
                }
                else {
                    if (null != item.Object) {
                        list.Add(item.Object);
                    }
                    else {
                        var obj = AssetDatabase.LoadMainAssetAtPath(item.AssetPath);
                        if (null != obj) {
                            list.Add(obj);
                        }
                    }
                }
            }
        }
        Selection.objects = list.ToArray();
        if (!(m_SearchSource == "sceneobjects" || m_SearchSource == "scenecomponents")) {
            Type.GetType("UnityEditor.ProjectBrowser,UnityEditor").InvokeMember("ShowSelectedObjectsInLastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, null);
        }
    }
    internal void GenerateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        int size = 10;
        int ct = 32;
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
                        if (ix >= ct) {
                            ix = 1;
                            ++iy;
                        }
                    }
                }
            }
        }
    }
    internal void AnalyseAssets()
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
    internal void SaveDependencies()
    {
        string fullpath = EditorPrefs.GetString(c_pref_key_save_dependencies);
        bool noPath = string.IsNullOrEmpty(fullpath);
        string dir = noPath ? Application.dataPath : Path.GetDirectoryName(fullpath);
        string name = noPath ? "dependencies" : Path.GetFileName(fullpath);
        string path = EditorUtility.SaveFilePanel("请指定要保存依赖信息的文件", dir, name, "txt");
        if (!string.IsNullOrEmpty(path)) {
            EditorPrefs.SetString(c_pref_key_save_dependencies, path);

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
                        if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                            goto L_EndSaveDep;
                        }
                    }
                }
                foreach (var asset in m_UnusedAssets) {
                    sw.WriteLine("unused_asset_tag\t{0}", asset);
                    ++curCount;
                    if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                        goto L_EndSaveDep;
                    }
                }
            L_EndSaveDep:
                sw.Close();
                EditorUtility.ClearProgressBar();
            }
        }
    }
    internal void LoadDependencies()
    {
        string file = EditorPrefs.GetString(c_pref_key_load_dependencies);
        string path = EditorUtility.OpenFilePanel("请指定要加载依赖信息的文件", string.IsNullOrEmpty(file) ? string.Empty : Path.GetDirectoryName(file), "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            EditorPrefs.SetString(c_pref_key_load_dependencies, path);

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
                    }
                    else {
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
                    if (DisplayCancelableProgressBar("加载进度", curCount, totalCount)) {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }
    internal void AnalyseSnapshot()
    {
        m_ClassifiedNativeMemoryInfos.Clear();
        m_ClassifiedManagedMemoryInfos.Clear();
        int curCount = 0;
        int totalCount = s_CachedSnapshot.SortedNativeObjects.Count;
        for (int i = 0; i < totalCount; ++i) {
            var size = s_CachedSnapshot.SortedNativeObjects.Size(i);
            var addr = s_CachedSnapshot.SortedNativeObjects.Address(i);
            var name = s_CachedSnapshot.SortedNativeObjects.Name(i);
            var refCount = s_CachedSnapshot.SortedNativeObjects.Refcount(i);
            var instanceId = s_CachedSnapshot.SortedNativeObjects.InstanceId(i);
            var nativeTypeIndex = s_CachedSnapshot.SortedNativeObjects.NativeTypeArrayIndex(i);
            string typeName = string.Empty;
            if (nativeTypeIndex >= 0 && nativeTypeIndex < s_CachedSnapshot.nativeTypes.Count) {
                typeName = s_CachedSnapshot.nativeTypes.typeName[nativeTypeIndex];
            }
            int managedObjectIndex = s_CachedSnapshot.SortedNativeObjects.ManagedObjectIndex(i);

            var memory = new ResourceEditUtility.MemoryInfo();
            memory.instanceId = (ulong)instanceId;
            memory.name = name;
            memory.className = typeName;
            memory.size = (long)size;
            memory.refCount = refCount;
            memory.address = addr;
            memory.isManaged = false;
            memory.sortedObjectIndex = i;
            int index;
            if(s_CachedSnapshot.nativeObjects.instanceId2Index.TryGetValue(instanceId, out index)) {
                memory.objectData = ObjectData.FromNativeObjectIndex(s_CachedSnapshot, index);
            }

            ResourceEditUtility.MemoryGroupInfo groupInfo = null;
            if (!m_ClassifiedNativeMemoryInfos.TryGetValue(memory.className, out groupInfo)) {
                groupInfo = new ResourceEditUtility.MemoryGroupInfo();
                groupInfo.group = memory.className;
                m_ClassifiedNativeMemoryInfos.Add(memory.className, groupInfo);
            }
            ++groupInfo.count;
            groupInfo.size += memory.size;
            groupInfo.memories.Add(memory);

            ++curCount;
            if (curCount % 100 == 0 && DisplayCancelableProgressBar("内存native信息分类进度", curCount, totalCount, false)) {
                m_ClassifiedNativeMemoryInfos.Clear();
                EditorUtility.ClearProgressBar();
                return;
            }
        }
        curCount = 0;
        totalCount = s_CachedSnapshot.SortedManagedObjects.Count;
        for (int i = 0; i < totalCount; ++i) {
            var size = s_CachedSnapshot.SortedManagedObjects.Size(i);
            var addr = s_CachedSnapshot.SortedManagedObjects.Address(i);
            ManagedObjectInfo objInfo;
            int refCount = 0;
            string typeName = string.Empty;
            if (s_CachedSnapshot.CrawledData.ManagedObjectByAddress.TryGetValue(addr, out objInfo)) {
                refCount = objInfo.RefCount;
                if (objInfo.ITypeDescription >= 0 && objInfo.ITypeDescription < s_CachedSnapshot.typeDescriptions.Count) {
                    typeName = s_CachedSnapshot.typeDescriptions.typeDescriptionName[objInfo.ITypeDescription];
                }
            }

            var memory = new ResourceEditUtility.MemoryInfo();
            memory.instanceId = addr;
            memory.name = typeName;
            memory.className = typeName;
            memory.size = (long)size;
            memory.address = addr;
            memory.refCount = refCount;
            memory.isManaged = true;
            memory.sortedObjectIndex = i;
            memory.objectData= ObjectData.FromManagedObjectInfo(s_CachedSnapshot, objInfo);            

            ResourceEditUtility.MemoryGroupInfo groupInfo = null;
            if (!m_ClassifiedManagedMemoryInfos.TryGetValue(memory.className, out groupInfo)) {
                groupInfo = new ResourceEditUtility.MemoryGroupInfo();
                groupInfo.group = memory.className;
                m_ClassifiedManagedMemoryInfos.Add(memory.className, groupInfo);
            }
            ++groupInfo.count;
            groupInfo.size += memory.size;
            groupInfo.memories.Add(memory);

            ++curCount;
            if (curCount % 1000 == 0 && DisplayCancelableProgressBar("内存managed信息分类进度", curCount, totalCount, false)) {
                m_ClassifiedManagedMemoryInfos.Clear();
                EditorUtility.ClearProgressBar();
                return;
            }
        }
        EditorUtility.ClearProgressBar();
    }
    internal IList<KeyValuePair<string, object>> FindShortestPathToRoot(ulong addr)
    {
        var data = ObjectDataFromAddress(addr);
        return FindShortestPathToRoot(data);
    }
    internal IList<KeyValuePair<string, object>> FindShortestPathToRoot(ObjectData obj)
    {
        var list = new List<KeyValuePair<string, object>>();
        if (null != s_ShortestPathToRootFinder) {
            var refbys = s_ShortestPathToRootFinder.FindFor(obj);
            if (null != refbys) {
                list.Add(new KeyValuePair<string, object>("=ShortestPathToRoot=", null));
                foreach (var data in refbys) {
                    if (data.IsField()) {
                        var parent = data.m_Parent.obj;
                        string name = string.Empty;
                        if (parent.managedTypeIndex >= 0 && parent.managedTypeIndex < s_CachedSnapshot.typeDescriptions.Count) {
                            name = s_CachedSnapshot.typeDescriptions.typeDescriptionName[parent.managedTypeIndex];
                        }
                        list.Add(new KeyValuePair<string, object>(string.Format("{0}.{1}", name, data.GetFieldName(s_CachedSnapshot)), data.displayObject));
                    }
                    else if (data.IsArrayItem()) {
                        var parent = data.m_Parent.obj;
                        var arrInfo = parent.GetArrayInfo(s_CachedSnapshot);
                        if (null != arrInfo) {
                            string type = string.Empty;
                            if (arrInfo.elementTypeDescription >= 0 && arrInfo.elementTypeDescription < s_CachedSnapshot.typeDescriptions.Count) {
                                type = s_CachedSnapshot.typeDescriptions.typeDescriptionName[arrInfo.elementTypeDescription];
                            }
                            string rank = arrInfo.ArrayRankToString();
                            var indexStr = arrInfo.IndexToRankedString(data.arrayIndex);
                            list.Add(new KeyValuePair<string, object>(string.Format("{0}(rank:{1})[{2}]", type, rank, indexStr), data.displayObject));
                        }
                        else {
                            string name = string.Empty;
                            if (data.managedTypeIndex >= 0 && data.managedTypeIndex < s_CachedSnapshot.typeDescriptions.Count) {
                                name = s_CachedSnapshot.typeDescriptions.typeDescriptionName[data.managedTypeIndex];
                            }
                            list.Add(new KeyValuePair<string, object>(string.Format("{0}[{1}]", name, parent.arrayIndex), data.displayObject));
                        }
                    }
                    else if(data.isManaged) {
                        string name = string.Empty;
                        if (data.managedTypeIndex >= 0 && data.managedTypeIndex < s_CachedSnapshot.typeDescriptions.Count) {
                            name = s_CachedSnapshot.typeDescriptions.typeDescriptionName[data.managedTypeIndex];
                        }
                        list.Add(new KeyValuePair<string, object>(name, data.displayObject));
                    }
                    else if (data.isNative) {
                        string name = string.Empty;
                        string type = string.Empty;
                        if (data.nativeObjectIndex >= 0 && data.nativeObjectIndex < s_CachedSnapshot.nativeObjects.Count) {
                            name = s_CachedSnapshot.nativeObjects.objectName[data.nativeObjectIndex];
                            int typeIndex = s_CachedSnapshot.nativeObjects.nativeTypeArrayIndex[data.nativeObjectIndex];
                            if (typeIndex >= 0 && typeIndex < s_CachedSnapshot.nativeTypes.Count) {
                                type = s_CachedSnapshot.nativeTypes.typeName[typeIndex];
                            }
                        }
                        list.Add(new KeyValuePair<string, object>(name + "(" + type + ")", data.displayObject));
                    }
                    else {
                        list.Add(new KeyValuePair<string, object>(data.ToString(), data.displayObject));
                    }
                }
                string reason;
                s_ShortestPathToRootFinder.IsRoot(refbys.Last(), out reason);
                list.Add(new KeyValuePair<string, object>("This is a root because:" + reason, null));
            }
            else {
                list.Add(new KeyValuePair<string, object>("No root is keeping this object alive.It will be collected next UnloadUnusedAssets() or scene load", null));
            }
        }
        list.Add(new KeyValuePair<string, object>(string.Empty, null));
        list.Add(new KeyValuePair<string, object>("[goto self]", obj.displayObject));
        return list;
    }
    internal HashSet<ObjectData> GetObjectDataRefByHash(ulong addr)
    {
        var data = ObjectDataFromAddress(addr);
        return GetObjectDataRefByHash(data);
    }
    internal HashSet<ObjectData> GetObjectDataRefByHash(ObjectData obj)
    {
        if (null != s_ShortestPathToRootFinder) {
            return s_ShortestPathToRootFinder.GetReferenceByHash(obj);
        }
        else {
            return s_EmptyObjectDataHash;
        }
    }
    internal IList<ObjectData> GetObjectDataRefByList(ulong addr)
    {
        var data = ObjectDataFromAddress(addr);
        return GetObjectDataRefByList(data);
    }
    internal IList<ObjectData> GetObjectDataRefByList(ObjectData obj)
    {
        if (null != s_ShortestPathToRootFinder) {
            return s_ShortestPathToRootFinder.GetReferenceByList(obj);
        }
        else {
            return s_EmptyObjectDataList;
        }
    }
    internal void OpenLink(ulong addr)
    {
        var data = ObjectDataFromAddress(addr);
        if (data.IsValid) {
            OpenLink(data);
        }
    }
    internal void OpenLink(ObjectData data)
    {
        if (null == s_CachedSnapshot)
            return;
        data = data.displayObject;
        if (data.isManaged) {
            int index = data.GetManagedObjectIndex(s_CachedSnapshot);
            if(index>=0 && index < s_CachedSnapshot.CrawledData.ManagedObjects.Count) {
                var obj = s_CachedSnapshot.CrawledData.ManagedObjects[index];

                var lr = new LinkRequestTable();
                lr.LinkToOpen = new TableLink();
                lr.LinkToOpen.TableName = ObjectTable.TableName;
                lr.SourceTable = null;
                lr.SourceColumn = null;
                lr.SourceRow = -1;
                lr.Parameters.AddValue(ObjectTable.ObjParamName, obj.PtrObject);
                lr.Parameters.AddValue(ObjectTable.TypeParamName, obj.ITypeDescription);

                Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.OpenTableLink(lr);
            }
        }
        else if (data.isNative) {
            int index = data.GetNativeObjectIndex(s_CachedSnapshot);
            if(index>=0 && index < s_CachedSnapshot.nativeObjects.Count) {
                int instanceId = s_CachedSnapshot.nativeObjects.instanceId[index];

                var lr = new LinkRequestTable();
                lr.LinkToOpen = new TableLink();
                lr.LinkToOpen.TableName = ObjectAllNativeTable.TableName;
                var exp = new Unity.MemoryProfilerForExtension.Editor.Database.View.Where.Builder("NativeInstanceId", 
                    Unity.MemoryProfilerForExtension.Editor.Database.Operation.Operator.Equal, 
                    new Unity.MemoryProfilerForExtension.Editor.Database.Operation.Expression.MetaExpression(instanceId.ToString(), true));
                lr.LinkToOpen.RowWhere = new List<Unity.MemoryProfilerForExtension.Editor.Database.View.Where.Builder>();
                lr.LinkToOpen.RowWhere.Add(exp);
                lr.SourceTable = null;
                lr.SourceColumn = null;
                lr.SourceRow = -1;

                Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.OpenTableLink(lr);
            }
        }
    }
    internal void OpenReferenceLink(ulong addr)
    {
        var data = ObjectDataFromAddress(addr);
        if (data.IsValid) {
            OpenReferenceLink(data);
        }
    }
    internal void OpenReferenceLink(ObjectData data)
    {
        if (null == s_CachedSnapshot)
            return;
        data = data.displayObject;
        int index = data.GetUnifiedObjectIndex(s_CachedSnapshot);
        if (index >= 0) {
            var lr = new LinkRequestTable();
            lr.LinkToOpen = new TableLink();
            lr.LinkToOpen.TableName = ObjectReferenceTable.kObjectReferenceTableName;
            lr.SourceTable = null;
            lr.SourceColumn = null;
            lr.SourceRow = -1;
            lr.Parameters.AddValue(ObjectTable.ObjParamName, index);

            Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.OpenTableLink(lr);
        }
    }
    internal void OpenLinkInCurrentTable(int tableIndex, IList<string> nameValues)
    {
        if (null == s_CachedSnapshot)
            return;
        var mode = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurMode();
        if (null != mode && tableIndex >= 0 && tableIndex < mode.GetSchema().GetTableCount()) {
            var table = mode.GetTableByIndex(tableIndex);
            if (null != table) {
                var lr = new LinkRequestTable();
                lr.LinkToOpen = new TableLink();
                lr.LinkToOpen.TableName = table.GetName();
                lr.LinkToOpen.RowWhere = BuildViewWheres(nameValues);
                lr.SourceTable = null;
                lr.SourceColumn = null;
                lr.SourceRow = -1;

                Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.OpenTableLink(lr);
            }
        }
    }
    internal int GetCurrentTableCount()
    {
        if (null == s_CachedSnapshot)
            return 0;
        var mode = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurMode();
        if (null != mode) {
            return (int)mode.GetSchema().GetTableCount();
        }
        else {
            return 0;
        }
    }
    internal string GetCurrentTableName(int tableIndex)
    {
        if (null == s_CachedSnapshot)
            return string.Empty;
        var mode = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurMode();
        if (null != mode && tableIndex >= 0 && tableIndex < mode.GetSchema().GetTableCount()) {
            return mode.GetTableByIndex(tableIndex).GetName();
        }
        else {
            return string.Empty;
        }
    }
    internal Table GetCurrentTable(int tableIndex)
    {
        if (null == s_CachedSnapshot)
            return null;
        var mode = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurMode();
        if (null != mode && tableIndex >= 0 && tableIndex < mode.GetSchema().GetTableCount()) {
            return mode.GetTableByIndex(tableIndex);
        }
        else {
            return null;
        }
    }
    internal ObjectData ObjectDataFromAddress(ulong addr)
    {
        if (null == s_CachedSnapshot)
            return ObjectData.invalid;
        var data = ObjectData.FromManagedPointer(s_CachedSnapshot, addr);
        if (!data.IsValid) {
            int low = 0;
            int high = s_CachedSnapshot.SortedNativeObjects.Count - 1;
            while (low <= high) {
                var ix = (low + high) / 2;
                var lowAddr = s_CachedSnapshot.SortedNativeObjects.Address(low);
                var highAddr = s_CachedSnapshot.SortedNativeObjects.Address(high);
                var vaddr = s_CachedSnapshot.SortedNativeObjects.Address(ix);
                if (vaddr < addr) {
                    low = ix + 1;
                }
                else if (vaddr > addr) {
                    high = ix - 1;
                }
                else {
                    var instanceId = s_CachedSnapshot.SortedNativeObjects.InstanceId(ix);
                    int nativeObjectIndex;
                    if (s_CachedSnapshot.nativeObjects.instanceId2Index.TryGetValue(instanceId, out nativeObjectIndex)) {
                        data = ObjectData.FromNativeObjectIndex(s_CachedSnapshot, nativeObjectIndex);
                    }
                    break;
                }
            }
        }
        return data;
    }
    internal ObjectData ObjectDataFromUnifiedObjectIndex(int index)
    {
        if (null == s_CachedSnapshot)
            return ObjectData.invalid;
        return ObjectData.FromUnifiedObjectIndex(s_CachedSnapshot, index);
    }
    internal ObjectData ObjectDataFromNativeObjectIndex(int index)
    {
        if (null == s_CachedSnapshot)
            return ObjectData.invalid;
        return ObjectData.FromNativeObjectIndex(s_CachedSnapshot, index);
    }
    internal ObjectData ObjectDataFromManagedObjectIndex(int index)
    {
        if (null == s_CachedSnapshot)
            return ObjectData.invalid;
        return ObjectData.FromManagedObjectIndex(s_CachedSnapshot, index);
    }
    internal void LoadMemoryInfo()
    {
        s_CachedSnapshot = null;
        m_ClassifiedNativeMemoryInfos.Clear();
        m_ClassifiedManagedMemoryInfos.Clear();
        Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.ShowWindow();
        s_CachedSnapshot = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurCachedSnapshot();
        if (null != s_CachedSnapshot) {
            s_ShortestPathToRootFinder = new ShortestPathToRootObjectFinder(s_CachedSnapshot);
            AnalyseSnapshot();
        }
    }
    internal void ClearInstrumentInfo()
    {
        m_InstrumentInfos.Clear();
        m_RecordIndex = 0;
        m_LastFrame = -1;
    }
    internal void RecordInstrument()
    {
        int firstIndex = ProfilerDriver.firstFrameIndex;
        int lastIndex = ProfilerDriver.lastFrameIndex;

        if (lastIndex >= firstIndex && lastIndex >= 0) {
            float[] batches = new float[lastIndex - firstIndex + 1];
            float[] triangles = new float[lastIndex - firstIndex + 1];
            var labels = ProfilerDriver.GetGraphStatisticsPropertiesForArea(ProfilerArea.Rendering);
            foreach (string l in labels) {
                var id = ProfilerDriver.GetStatisticsIdentifierForArea(ProfilerArea.Rendering, l);
                var lowerLabel = l.ToLower();
                if (lowerLabel == "batches") {
                    float maxVal;
                    ProfilerDriver.GetStatisticsValues(id, firstIndex, 1.0f, batches, out maxVal);
                }
                else if (lowerLabel == "triangles") {
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
                if (RecordInstrumentFrame(m_RecordIndex, index, ProfilerColumn.FunctionName, ProfilerViewType.Hierarchy, triangle, batch)) {
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
    internal void SaveInstrumentInfo()
    {
        if (m_InstrumentInfos.Count > 0) {
            string fullpath = EditorPrefs.GetString(c_pref_key_save_instrument);
            bool noPath = string.IsNullOrEmpty(fullpath);
            string dir = noPath ? Application.dataPath : Path.GetDirectoryName(fullpath);
            string name = noPath ? "instrument" : Path.GetFileName(fullpath);
            string path = EditorUtility.SaveFilePanel("请指定要保存耗时信息的文件", dir, name, "txt");
            if (!string.IsNullOrEmpty(path)) {
                EditorPrefs.SetString(c_pref_key_save_instrument, path);

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
                        if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                            goto L_EndSaveIns;
                        }
                        foreach (var record in info.records) {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}",
                                info.index, info.frame, record.depth, record.name, record.layerPath,
                                info.fps, record.calls, record.gcMemory,
                                record.totalTime, record.totalPercent, record.selfTime, record.selfPercent,
                                record.totalGpuTime, record.totalGpuPercent, record.selfGpuTime, record.selfGpuPercent);
                            ++curCount;
                            if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                                goto L_EndSaveIns;
                            }
                        }
                    }
                L_EndSaveIns:
                    sw.Close();
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        else {
            EditorUtility.DisplayDialog("错误", "没有记录耗时信息，请先记录！", "ok");
        }
    }
    internal void LoadInstrumentInfo()
    {
        string file = EditorPrefs.GetString(c_pref_key_load_instrument);
        string path = EditorUtility.OpenFilePanel("请指定要加载耗时信息的文件", string.IsNullOrEmpty(file) ? string.Empty : Path.GetDirectoryName(file), "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            EditorPrefs.SetString(c_pref_key_load_instrument, path);

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
                    }
                    else {
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
                    if (DisplayCancelableProgressBar("加载进度", curCount, totalCount)) {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }
    internal void SaveResult()
    {
        if (m_GroupList.Count > 0 || m_ItemList.Count > 0) {
            string fullpath = EditorPrefs.GetString(c_pref_key_save_result);
            bool noPath = string.IsNullOrEmpty(fullpath);
            string dir = noPath ? Application.dataPath : Path.GetDirectoryName(fullpath);
            string name = noPath ? "result" : Path.GetFileName(fullpath);
            string path = EditorUtility.SaveFilePanel("请指定要保存分析结果的文件", dir, name, "txt");
            if (!string.IsNullOrEmpty(path)) {
                EditorPrefs.SetString(c_pref_key_save_result, path);

                if (File.Exists(path)) {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path)) {
                    if (m_GroupList.Count > 0) {
                        sw.WriteLine("asset_path\tscene_path\tinfo\torder\tvalue");
                        int curCount = 0;
                        int totalCount = m_GroupList.Count;
                        foreach (var item in m_GroupList) {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", item.AssetPath, item.ScenePath, item.Info, item.Order, item.Value);
                            ++curCount;
                            if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                                break;
                            }
                        }
                        sw.Close();
                    }
                    else {
                        sw.WriteLine("asset_path\tscene_path\tinfo\torder\tvalue");
                        int curCount = 0;
                        int totalCount = m_ItemList.Count;
                        foreach (var item in m_ItemList) {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", item.AssetPath, item.ScenePath, item.Info, item.Order, item.Value);
                            ++curCount;
                            if (DisplayCancelableProgressBar("保存进度", curCount, totalCount)) {
                                break;
                            }
                        }
                        sw.Close();
                    }
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        else {
            EditorUtility.DisplayDialog("错误", "没有分析结果信息，请先采集分析结果！", "ok");
        }
    }
    internal void LoadResult()
    {
        string file = EditorPrefs.GetString(c_pref_key_load_result);
        string path = EditorUtility.OpenFilePanel("请指定要加载分析结果的文件", string.IsNullOrEmpty(file) ? string.Empty : Path.GetDirectoryName(file), "txt");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            EditorPrefs.SetString(c_pref_key_load_result, path);

            int i = 0;
            try {
                var txt = File.ReadAllText(path);
                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                m_ItemList.Clear();
                int curCount = 1;
                int totalCount = lines.Length;
                for (i = 1; i < lines.Length; ++i) {
                    var fields = lines[i].Split('\t');
                    var assetPath = fields[0];
                    var scenePath = fields[1];
                    var info = fields[2];
                    var order = int.Parse(fields[3]);
                    var value = double.Parse(fields[4]);

                    var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Info = info, Order = order, Value = value };
                    m_ItemList.Add(item);

                    ++curCount;
                    if (DisplayCancelableProgressBar("加载进度", curCount, totalCount)) {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                EditorUtility.DisplayDialog("异常", string.Format("line {0} exception {1}\n{2}", i, ex.Message, ex.StackTrace), "ok");
            }
            EditorUtility.ClearProgressBar();
        }
    }

    internal void ClearDsl()
    {
        CacheParams();

        m_CanRefresh = false;
        m_DslFile = null;
        m_SearchSource = string.Empty;
        m_TypeOrExtList.Clear();
        m_TypeList.Clear();
        m_ParamNames.Clear();
        m_Params.Clear();
        m_Text = null;
        m_NextFilterIndex = 0;
        m_NextGroupIndex = 0;
        m_NextProcessIndex = 0;
        m_FilterCalculator = null;
        m_GroupCalculator = null;
        m_ProcessCalculator = null;

        m_ItemList.Clear();
        m_GroupList.Clear();
        ResourceEditUtility.ResetCommandCalculator();
    }
    internal void SelectDsl(string path)
    {
        ClearDsl();
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                m_DslPath = path;
                m_DslFile = file;
                m_NextFilterIndex = 0;
                m_NextGroupIndex = 0;
                m_NextProcessIndex = 0;
                m_ScriptCalculator = new DslExpression.DslCalculator();
                m_FilterCalculator = new DslExpression.DslCalculator();
                m_GroupCalculator = new DslExpression.DslCalculator();
                m_ProcessCalculator = new DslExpression.DslCalculator();
                ResourceEditUtility.InitCalculator(m_ScriptCalculator);
                ResourceEditUtility.InitCalculator(m_FilterCalculator);
                ResourceEditUtility.InitCalculator(m_GroupCalculator);
                ResourceEditUtility.InitCalculator(m_ProcessCalculator);

                bool haveError = false;
                foreach (var info in file.DslInfos) {
                    bool check = false;
                    int num = info.GetFunctionNum();
                    if (num == 1) {
                        var func = info.First;
                        var id = func.GetId();
                        if (id == "script") {
                            check = true;
                            m_ScriptCalculator.LoadDsl(info);
                        }
                    }
                    else if (num == 2) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        if (firstId == "script" && secondId == "args") {
                            check = true;
                            m_ScriptCalculator.LoadDsl(info);
                        }
                        else if (firstId == "input" && (secondId == "filter" || secondId == "process")) {
                            check = true;

                            if (secondId == "filter") {
                                m_FilterCalculator.LoadDsl(m_NextFilterIndex.ToString(), info.Second);
                                ++m_NextFilterIndex;
                            }
                            else if (secondId == "process") {
                                m_ProcessCalculator.LoadDsl(m_NextProcessIndex.ToString(), info.Second);
                                ++m_NextProcessIndex;
                            }
                        }
                    }
                    else if (num == 3) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        string thirdId = info.Last.GetId();
                        if (firstId == "input" && secondId == "filter" && (thirdId == "group" || thirdId == "process")) {
                            check = true;

                            m_FilterCalculator.LoadDsl(m_NextFilterIndex.ToString(), info.Second);
                            ++m_NextFilterIndex;
                            if (thirdId == "group") {
                                m_GroupCalculator.LoadDsl(m_NextGroupIndex.ToString(), info.Last);
                                ++m_NextGroupIndex;
                            }
                            else if (thirdId == "process") {
                                m_ProcessCalculator.LoadDsl(m_NextProcessIndex.ToString(), info.Last);
                                ++m_NextProcessIndex;
                            }
                        }
                    }
                    else if (num == 4) {
                        string firstId = info.First.GetId();
                        string secondId = info.Second.GetId();
                        string thirdId = info.Functions[2].GetId();
                        string fourthId = info.Last.GetId();

                        if (firstId == "input" && secondId == "filter" && thirdId == "group" && fourthId == "process") {
                            check = true;

                            m_FilterCalculator.LoadDsl(m_NextFilterIndex.ToString(), info.Second);
                            ++m_NextFilterIndex;
                            m_GroupCalculator.LoadDsl(m_NextGroupIndex.ToString(), info.Functions[2]);
                            ++m_NextGroupIndex;
                            if (fourthId == "process") {
                                m_ProcessCalculator.LoadDsl(m_NextProcessIndex.ToString(), info.Last);
                                ++m_NextProcessIndex;
                            }
                        }
                    }
                    if (!check) {
                        EditorUtility.DisplayDialog("错误", string.Format("error script:{0}, must be input(exts){{...}}filter{{...}}; or input(exts){{...}}process{{...}}; or input(exts){{...}}filter{{...}}process{{...}}; or input(exts){{...}}filter{{...}}group{{...}}; or input(exts){{...}}filter{{...}}group{{...}}process{{...}}; or script(func){{...}};", info.GetLine()), "ok");
                        haveError = true;
                    }
                }
                if (!haveError) {
                    m_SearchSource = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_ParamNames.Clear();
                    m_Params.Clear();

                    foreach (var info in m_DslFile.DslInfos) {
                        int num = info.GetFunctionNum();
                        if (num == 1) {
                            var func = info.First;
                            var id = func.GetId();
                            if (id == "script") {
                                continue;
                            }
                        }
                        else if (num == 2) {
                            string firstId = info.First.GetId();
                            string secondId = info.Second.GetId();
                            if (firstId == "script" && secondId == "args") {
                                continue;
                            }
                        }
                        var first = info.First;
                        var input = first.Call;
                        foreach (var param in input.Params) {
                            string ext = param.GetId();
                            m_TypeOrExtList.Add(ext);
                        }
                        foreach (var comp in first.Statements) {
                            var callData = comp as Dsl.CallData;
                            if (null != callData) {
                                ParseCallData(callData);
                            }
                            else {
                                var funcData = comp as Dsl.FunctionData;
                                ParseFunctionData(funcData);
                            }
                        }
                    }
                    m_Text = File.ReadAllText(path);
                    RestoreParams();
                }
                else {
                    m_DslFile = null;
                    m_SearchSource = string.Empty;
                    m_TypeOrExtList.Clear();
                    m_TypeList.Clear();
                    m_ParamNames.Clear();
                    m_Params.Clear();
                    m_Text = null;
                    m_NextFilterIndex = 0;
                    m_NextGroupIndex = 0;
                    m_NextProcessIndex = 0;
                    m_FilterCalculator = null;
                    m_GroupCalculator = null;
                    m_ProcessCalculator = null;
                }
            }
        }
    }
    internal string ParseCallData(Dsl.CallData callData)
    {
        string id = callData.GetId();
        string key = callData.GetParamId(0);
        string val = callData.GetParamId(1);
        if (id == "int") {
            //int(name, val);
            int v = int.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(int), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "uint") {
            //uint(name, val);
            uint v = uint.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(uint), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "long") {
            //long(name, val);
            long v = long.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(long), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "ulong") {
            //ulong(name, val);
            ulong v = ulong.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(ulong), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "float") {
            //float(name, val);
            float v = float.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(float), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "double") {
            //double(name, val);
            double v = double.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(double), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "string") {
            //string(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(string), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "intlist") {
            //intlist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<int>();
            foreach (var str in v) {
                int iv;
                int.TryParse(str, out iv);
                list.Add(iv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<int>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "uintlist") {
            //uintlist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<uint>();
            foreach (var str in v) {
                uint iv;
                uint.TryParse(str, out iv);
                list.Add(iv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<uint>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "longlist") {
            //longlist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<long>();
            foreach (var str in v) {
                long iv;
                long.TryParse(str, out iv);
                list.Add(iv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<long>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "ulonglist") {
            //ulonglist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<ulong>();
            foreach (var str in v) {
                ulong iv;
                ulong.TryParse(str, out iv);
                list.Add(iv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<ulong>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "floatlist") {
            //floatlist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<float>();
            foreach (var str in v) {
                float fv;
                float.TryParse(str, out fv);
                list.Add(fv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<float>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "doublelist") {
            //doublelist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<double>();
            foreach (var str in v) {
                double fv;
                double.TryParse(str, out fv);
                list.Add(fv);
            }
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<double>), Value = list, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "stringlist") {
            //stringlist(name, val);
            var v = val.Split(new char[] { ';', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(List<string>), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "bool") {
            //bool(name, val);
            bool v = bool.Parse(val);
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(bool), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "button") {
            //button(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(UnityEngine.GUIElement), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "table") {
            //table(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(ResourceEditUtility.DataTable), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "excel") {
            //excel(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(NPOI.SS.UserModel.IWorkbook), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "script") {
            //script(name, val);
            string v = val;
            m_Params[key] = new ResourceEditUtility.ParamInfo { Name = key, Type = typeof(object), Value = v, StringValue = val };
            m_ParamNames.Add(key);
        }
        else if (id == "feature") {
            if (key == "menu") {
                m_DslMenu = val;
            }
            else if (key == "description") {
                m_DslDescription = val;
            }
            else if (key == "source") {
                //feature("source", "script" or "list" or "excel" or "table" or "project" or "sceneobjects" or "scenecomponents" or "sceneassets" or "allassets" or "unusedassets" or "assetbundle");
                m_SearchSource = val;
            }
        }
        return key;
    }
    internal void ParseFunctionData(Dsl.FunctionData funcData)
    {
        var callData = funcData.Call;
        string name = ParseCallData(callData);
        ResourceEditUtility.ParamInfo info;
        if (m_Params.TryGetValue(name, out info)) {
            foreach (var comp in funcData.Statements) {
                var id = comp.GetId();
                var cd = comp as Dsl.CallData;
                if (null != cd) {
                    if (id == "script") {
                        info.Script = cd.GetParamId(0);
                    }
                    else if (id == "file") {
                        int num = cd.GetParamNum();
                        if (num > 0) {
                            var p1 = cd.GetParamId(0);
                            info.FileExts = p1;
                        }
                        if (num > 1) {
                            var p2 = cd.GetParamId(0);
                            info.FileInitDir = p2;
                        }
                    }
                    else if (id == "encoding") {
                        info.Encoding = cd.GetParamId(0);
                    }
                    else if (id == "range") {
                        int num = cd.GetParamNum();
                        var p1 = cd.GetParam(0);
                        var p2 = cd.GetParam(1);
                        var pvd1 = p1 as Dsl.ValueData;
                        var pvd2 = p2 as Dsl.ValueData;
                        if (null != pvd1 && null != pvd2) {
                            //range(min,max);
                            string min = pvd1.GetId();
                            string max = pvd2.GetId();
                            if (info.Type == typeof(int) || info.Type == typeof(uint) || info.Type == typeof(long) || info.Type == typeof(ulong)) {
                                info.MinValue = int.Parse(min);
                                info.MaxValue = int.Parse(max);
                            }
                            else if (info.Type == typeof(float) || info.Type == typeof(double)) {
                                info.MinValue = float.Parse(min);
                                info.MaxValue = float.Parse(max);
                            }
                            else if (info.Type == typeof(string)) {
                                info.MinValue = min;
                                info.MaxValue = max;
                            }
                            else if (info.Type == typeof(bool)) {
                                info.MinValue = bool.Parse(min);
                                info.MaxValue = bool.Parse(max);
                            }
                        }
                    }
                    else if (id == "popup" || id == "toggle" || id == "multiple" || id == "param") {
                        if (string.IsNullOrEmpty(info.OptionStyle)) {
                            info.OptionStyle = id;
                        }
                        else if (info.OptionStyle != id) {
                            EditorUtility.DisplayDialog("错误", string.Format("param's option must use same style, {0} will use {1} style (dont use {2}) !", info.Name, info.OptionStyle, id), "ok");
                        }

                        int num = cd.GetParamNum();
                        var p1 = cd.GetParam(0);
                        var p2 = cd.GetParam(1);
                        var pvd1 = p1 as Dsl.ValueData;
                        var pvd2 = p2 as Dsl.ValueData;
                        if (null != pvd1 && null != pvd2) {
                            //xxx(key,val);
                            string key = pvd1.GetId();
                            string val = pvd2.GetId();
                            info.Options[key] = val;
                            info.OptionNames.Add(key);
                        }
                        else {
                            var pcd1 = p1 as Dsl.CallData;
                            var pcd2 = p2 as Dsl.CallData;
                            if (1 == num && null != pcd1) {
                                if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_OPERATOR && pcd1.GetId() == "..") {
                                    //xxx(min..max);
                                    int min = int.Parse(pcd1.GetParamId(0));
                                    int max = int.Parse(pcd1.GetParamId(1));
                                    for (int v = min; v <= max; ++v) {
                                        string kStr = v.ToString("D3");
                                        string vStr = v.ToString();
                                        info.Options[kStr] = vStr;
                                        info.OptionNames.Add(kStr);
                                    }
                                }
                                else if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd1.HaveId()) {
                                    //xxx([v1,v2,v3,v4,v5]);
                                    for (int i = 0; i < pcd1.GetParamNum(); ++i) {
                                        var vStr = pcd1.GetParamId(i);
                                        info.Options[vStr] = vStr;
                                        info.OptionNames.Add(vStr);
                                    }
                                }
                            }
                            else if (2 == num) {
                                if (null != pcd1 && null != pcd2) {
                                    if (pcd1.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd1.HaveId() && pcd2.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET && !pcd2.HaveId()) {
                                        //xxx([k1,k2,k3,k4,k5],[v1,v2,v3,v4,v5]);
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
                }
                else {
                    var vd = comp as Dsl.ValueData;
                    if (null != vd) {
                        //用于支持标签属性
                        info.OptionStyle = vd.GetId();
                    }
                }
            }
        }
    }
    internal object CallScript(DslExpression.DslCalculator calc, string name, params object[] args)
    {
        if (null != m_ScriptCalculator) {
            if (null != calc) {
                foreach (var pair in calc.NamedGlobalVariables) {
                    m_ScriptCalculator.SetGlobalVariable(pair.Key, pair.Value);
                }
            }
            else {
                m_ScriptCalculator.SetGlobalVariable("params", m_Params);
                foreach (var pair in m_Params) {
                    m_ScriptCalculator.SetGlobalVariable(pair.Key, pair.Value.Value);
                }
            }
            var ret = m_ScriptCalculator.Calc(name, args);
            if (null != calc) {
                foreach (var pair in m_ScriptCalculator.NamedGlobalVariables) {
                    calc.SetGlobalVariable(pair.Key, pair.Value);
                }
            }
            return ret;
        }
        else {
            return null;
        }
    }
    internal void Collect()
    {
        m_CollectPath = string.Empty;
        Refresh();
    }
    internal void Refresh()
    {
        m_CanRefresh = true;
        m_OverridedProgressTitle = string.Empty;
        Refresh(false);
    }
    internal void Refresh(bool isBatch)
    {
        foreach (var pair in m_Params) {
            var paramInfo = pair.Value;
            if (paramInfo.Type == typeof(ResourceEditUtility.DataTable) && paramInfo.Value is string) {
                var file = paramInfo.Value as string;
                var ext = Path.GetExtension(file);
                var table = new ResourceEditUtility.DataTable();
                table.Load(file, Encoding.GetEncoding(paramInfo.Encoding), ext == ".csv" ? ',' : '\t');
                paramInfo.Value = table;
            }
            else if (paramInfo.Type == typeof(NPOI.SS.UserModel.IWorkbook) && paramInfo.Value is string) {
                var file = paramInfo.Value as string;
                var ext = Path.GetExtension(file);
                NPOI.SS.UserModel.IWorkbook book = null;
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    if (ext == ".xls") {
                        book = new NPOI.HSSF.UserModel.HSSFWorkbook(stream);
                    }
                    else {
                        book = new NPOI.XSSF.UserModel.XSSFWorkbook(stream);
                    }
                }
                paramInfo.Value = book;
            }
            else if (paramInfo.Type == typeof(object)) {
                var funcName = paramInfo.StringValue;
                paramInfo.Value = CallScript(null, funcName, paramInfo);
            }
        }
        if (m_SearchSource == "script") {
            m_ItemList.Clear();
            SearchScriptResult();
            EditorUtility.ClearProgressBar();
        }
        else if (m_SearchSource == "list") {
            m_ItemList.Clear();
            if (m_TypeOrExtList.Count > 0) {
                SearchList();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "sceneobjects") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = CountSceneObjects();
            if (m_TotalSearchCount > 0) {
                SearchSceneObjects();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "scenecomponents") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = CountSceneObjects();
            if (m_TotalSearchCount > 0) {
                SearchSceneComponents();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "sceneassets") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountSceneAssets();
            if (m_TotalSearchCount > 0) {
                SearchSceneAssets();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "allassets") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountAllAssets();
            if (m_TotalSearchCount > 0) {
                SearchAllAssets();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "excel") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountExcelRecords();
            if (m_TotalSearchCount > 0) {
                SearchExcelRecords();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "table") {
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountTableRecords();
            if (m_TotalSearchCount > 0) {
                SearchTableRecords();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "unusedassets") {
            if (m_ReferenceAssets.Count <= 0 && m_ReferenceByAssets.Count <= 0 && m_UnusedAssets.Count <= 0) {
                if (!isBatch)
                    EditorUtility.DisplayDialog("错误", "未找到资源依赖信息，请先执行资源依赖‘分析’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            CountUnusedAssets();
            if (m_TotalSearchCount > 0) {
                SearchUnusedAssets();
                EditorUtility.ClearProgressBar();
            }
        }
        else if (m_SearchSource == "snapshot") {
            SearchSnapshot(isBatch);
            EditorUtility.ClearProgressBar();
        }
        else if (m_SearchSource == "instruments") {
            if (m_InstrumentInfos.Count <= 0) {
                if (!isBatch)
                    EditorUtility.DisplayDialog("错误", "未找到耗时信息，请先执行耗时‘记录’或‘加载’！", "ok");
                return;
            }
            m_ItemList.Clear();
            m_CurSearchCount = 0;
            m_TotalSearchCount = 0;
            SearchInstruments();
            EditorUtility.ClearProgressBar();
        }
        else {
            if (string.IsNullOrEmpty(m_CollectPath)) {
                string fullpath = EditorPrefs.GetString(c_pref_key_open_asset_folder);
                bool noPath = string.IsNullOrEmpty(fullpath);
                string dir = noPath ? Application.dataPath : Path.GetDirectoryName(fullpath);
                string name = noPath ? string.Empty : Path.GetFileName(fullpath);
                string path = EditorUtility.OpenFolderPanel("请选择要收集资源的根目录", dir, name);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
                    if (IsAssetPath(path)) {
                        EditorPrefs.SetString(c_pref_key_open_asset_folder, path);
                        m_CollectPath = path;
                    }
                    else {
                        if (!isBatch)
                            EditorUtility.DisplayDialog("错误", "必须选择本unity工程的资源路径！", "确定");
                    }
                }
            }
            if (!string.IsNullOrEmpty(m_CollectPath)) {
                m_ItemList.Clear();
                m_CurSearchCount = 0;
                m_TotalSearchCount = 0;
                CountFiles(m_CollectPath);
                if (m_TotalSearchCount > 0) {
                    SearchFiles(m_CollectPath);
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        CalcGroupValue();
        CalcTotalValue();
    }
    internal void CalcGroupValue()
    {
        m_IsItemListGroupStyle = false;
        ResourceEditUtility.ParamInfo paramInfo;
        if (m_Params.TryGetValue("style", out paramInfo)) {
            string str = paramInfo.Value as string;
            if (str == "itemlist") {
                m_IsItemListGroupStyle = true;
            }
            else if (str == "grouplist") {
                m_IsItemListGroupStyle = false;
            }
        }
        var groups = new Dictionary<string, ResourceEditUtility.GroupInfo>();
        foreach (var item in m_ItemList) {
            string group = item.Group;
            if (null != group) {
                ResourceEditUtility.GroupInfo groupInfo;
                if (groups.TryGetValue(group, out groupInfo)) {
                    groupInfo.Items.Add(item);
                    groupInfo.Sum += item.Value;
                    if (groupInfo.Max < item.Value)
                        groupInfo.Max = item.Value;
                    if (groupInfo.Min > item.Value)
                        groupInfo.Min = item.Value;
                }
                else {
                    groupInfo = new ResourceEditUtility.GroupInfo();
                    groupInfo.Group = group;
                    groupInfo.Items.Add(item);
                    groupInfo.Sum = item.Value;
                    groupInfo.Max = item.Value;
                    groupInfo.Min = item.Value;
                    groupInfo.Selected = false;
                    groups.Add(group, groupInfo);
                }
            }
            else {
                groups.Clear();
                break;
            }
        }
        m_UnfilteredGroupCount = groups.Count;
        m_GroupList.Clear();
        int curCt = 0;
        int totalCt = m_UnfilteredGroupCount;
        foreach (var pair in groups) {
            ++curCt;
            var group = pair.Value;
            if (m_IsItemListGroupStyle) {
                group.PrepareShowInfo();
                foreach (var item in group.Items) {
                    var itemGroup = new ResourceEditUtility.GroupInfo();
                    itemGroup.Items = group.Items;
                    itemGroup.CopyFrom(group);

                    itemGroup.AssetPath = item.AssetPath;
                    itemGroup.ScenePath = item.ScenePath;
                    itemGroup.Info = item.Info;

                    itemGroup.Order = item.Order;
                    itemGroup.Value = item.Value;
                    itemGroup.Selected = false;
                    var ret = ResourceEditUtility.Group(itemGroup, m_GroupCalculator, m_NextGroupIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                    if (m_NextGroupIndex <= 0 || null != ret && (int)ret > 0) {
                        m_GroupList.Add(itemGroup);
                    }
                }
            }
            else {
                var ret = ResourceEditUtility.Group(group, m_GroupCalculator, m_NextGroupIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                if (m_NextGroupIndex <= 0 || null != ret && (int)ret > 0) {
                    m_GroupList.Add(group);
                }
            }
            if (DisplayCancelableProgressBar("分组进度", m_GroupList.Count, curCt, totalCt)) {
                break;
            }
        }
        EditorUtility.ClearProgressBar();
    }
    internal void CalcTotalValue()
    {
        m_TotalItemValue = 0;
        if (m_UnfilteredGroupCount <= 0) {
            foreach (var item in m_ItemList) {
                m_TotalItemValue += item.Value;
            }
        }
        else {
            foreach (var item in m_GroupList) {
                m_TotalItemValue += item.Value;
            }
        }
    }
    internal void SelectAll()
    {
        if (m_UnfilteredGroupCount <= 0) {
            foreach (var item in m_ItemList) {
                item.Selected = true;
            }
        }
        else {
            foreach (var item in m_GroupList) {
                item.Selected = true;
            }
        }
    }
    internal void Process()
    {
        m_OverridedProgressTitle = string.Empty;
        Process(false);
    }
    internal void Process(bool isBatch)
    {
        if (null == m_DslFile) {
            if (!isBatch)
                EditorUtility.DisplayDialog("错误", "请先选择dsl !", "ok");
            return;
        }
        if (null == m_ProcessCalculator || m_NextProcessIndex <= 0) {
            if (!isBatch)
                EditorUtility.DisplayDialog("错误", "当前dsl没有配置process !", "ok");
            return;
        }
        if (!isBatch) {
            m_ProcessedAssets.Clear();
        }
        try {
            AssetDatabase.StartAssetEditing();

            int totalSelectedCount = 0;
            int index = 0;
            if (m_UnfilteredGroupCount <= 0) {
                foreach (var item in m_ItemList) {
                    if (item.Selected) {
                        ++totalSelectedCount;
                    }
                }
                foreach (var item in m_ItemList) {
                    if (item.Selected) {
                        if (!m_ProcessedAssets.Contains(item.AssetPath)) {
                            if (!string.IsNullOrEmpty(item.AssetPath))
                                m_ProcessedAssets.Add(item.AssetPath);
                            ResourceEditUtility.Process(item, m_ProcessCalculator, m_NextProcessIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                        }
                        ++index;
                        if (DisplayCancelableProgressBar("处理进度", index, totalSelectedCount)) {
                            break;
                        }
                    }
                }
            }
            else {
                foreach (var item in m_GroupList) {
                    if (item.Selected) {
                        ++totalSelectedCount;
                    }
                }
                foreach (var item in m_GroupList) {
                    if (item.Selected) {
                        if (!m_ProcessedAssets.Contains(item.AssetPath)) {
                            if (!string.IsNullOrEmpty(item.AssetPath))
                                m_ProcessedAssets.Add(item.AssetPath);
                            ResourceEditUtility.GroupProcess(item, m_ProcessCalculator, m_NextProcessIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                        }
                        ++index;
                        if (DisplayCancelableProgressBar("处理进度", index, totalSelectedCount)) {
                            break;
                        }
                    }
                }
            }
        }
        finally {
            AssetDatabase.StopAssetEditing();
            if (m_SearchSource == "sceneobjects" || m_SearchSource == "scenecomponents") {
                EditorSceneManager.SaveOpenScenes();
            }
            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh(ImportAssetOptions.Default);
            EditorUtility.UnloadUnusedAssetsImmediate(true);
            EditorUtility.ClearProgressBar();
            if (!isBatch)
                EditorUtility.DisplayDialog("提示", "处理完成", "ok");
        }
    }
    internal void ClearCaches()
    {
        m_DslParamCaches.Clear();
    }

    private List<Unity.MemoryProfilerForExtension.Editor.Database.View.Where.Builder> BuildViewWheres(IList<string> nameValues)
    {
        var list = new List<Unity.MemoryProfilerForExtension.Editor.Database.View.Where.Builder>();
        for (int i = 0; i < nameValues.Count - 4; i += 5) {
            var name = nameValues[i];
            var op = nameValues[i + 1];
            var val = nameValues[i + 2];
            var literal = nameValues[i + 3] == "literal";
            var typeName = nameValues[i + 4];
            var opEnum = (Unity.MemoryProfilerForExtension.Editor.Database.Operation.Operator)Enum.Parse(typeof(Unity.MemoryProfilerForExtension.Editor.Database.Operation.Operator), op);
            var type = Type.GetType(typeName);
            var exp = new Unity.MemoryProfilerForExtension.Editor.Database.View.Where.Builder(name,
                opEnum,
                new Unity.MemoryProfilerForExtension.Editor.Database.Operation.Expression.MetaExpression(val, literal, type));
            list.Add(exp);
        }
        return list;
    }

    private void CacheParams()
    {
        if (!string.IsNullOrEmpty(m_DslPath) && null != m_Params) {
            Dictionary<string, ResourceEditUtility.ParamInfo> paramInfos;
            if (!m_DslParamCaches.TryGetValue(m_DslPath, out paramInfos)) {
                paramInfos = new Dictionary<string, ResourceEditUtility.ParamInfo>();
                m_DslParamCaches.Add(m_DslPath, paramInfos);
            }
            else {
                paramInfos.Clear();
            }
            foreach (var pair in m_Params) {
                paramInfos.Add(pair.Key, pair.Value);
            }
        }
    }
    private void RestoreParams()
    {
        if (!string.IsNullOrEmpty(m_DslPath) && null != m_Params) {
            Dictionary<string, ResourceEditUtility.ParamInfo> paramInfos;
            if (m_DslParamCaches.TryGetValue(m_DslPath, out paramInfos)) {
                foreach (var pair in m_Params) {
                    var key = pair.Key;
                    var val = pair.Value;
                    ResourceEditUtility.ParamInfo info;
                    if (paramInfos.TryGetValue(key, out info) && val.Type == info.Type) {
                        val.Value = info.Value;
                    }
                }
            }
        }
    }

    private void SearchFiles(string dir)
    {
        string dirName = Path.GetFileName(dir).ToLower();
        if (s_IgnoredDirs.Contains(dirName))
            return;
        foreach (string ext in m_TypeOrExtList) {
            SearchFilesRecursively(dir, ext);
        }
    }
    private bool SearchFilesRecursively(string dir, string ext)
    {
        bool canceled = false;
        string[] files = Directory.GetFiles(dir);
        if (ext != "*.*") {
            files = files.Where((string file) => {
                return ResourceEditUtility.IsPathMatch(file, ext);
            }).ToArray();
        }
        foreach (string file in files) {
            ++m_CurSearchCount;
            string assetPath = PathToAssetPath(file);
            var importer = AssetImporter.GetAtPath(assetPath);
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.AddRange(m_Results);
            }
            canceled = DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
            if (canceled)
                return canceled;
        }
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            string dirName = Path.GetFileName(subDir).ToLower();
            if (s_IgnoredDirs.Contains(dirName))
                continue;
            canceled = SearchFilesRecursively(subDir, ext);
            if (canceled)
                return canceled;
        }
        return canceled;
    }
    private void CountFiles(string dir)
    {
        string dirName = Path.GetFileName(dir).ToLower();
        if (s_IgnoredDirs.Contains(dirName))
            return;
        foreach (string ext in m_TypeOrExtList) {
            CountFilesRecursively(dir, ext);
        }
    }
    private void CountFilesRecursively(string dir, string ext)
    {
        string[] files = Directory.GetFiles(dir);
        if (ext != "*.*") {
            files = files.Where((string file) => {
                return ResourceEditUtility.IsPathMatch(file, ext);
            }).ToArray();
        }
        m_TotalSearchCount += files.Length;

        string[] dirs = Directory.GetDirectories(dir);
        foreach (string subDir in dirs) {
            string dirName = Path.GetFileName(subDir).ToLower();
            if (s_IgnoredDirs.Contains(dirName))
                continue;
            CountFilesRecursively(subDir, ext);
        }
    }

    private void SearchUnusedAssets()
    {
        if (m_UnusedAssets.Count <= 0)
            return;

        foreach (string ext in m_TypeOrExtList) {
            IList<string> files = m_UnusedAssets;
            if (ext != "*.*") {
                files = m_UnusedAssets.Where((string file) => {
                    return ResourceEditUtility.IsPathMatch(file, ext);
                }).ToArray();
            }

            foreach (string file in files) {
                ++m_CurSearchCount;
                string assetPath = PathToAssetPath(file);
                var importer = AssetImporter.GetAtPath(assetPath);
                var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                    m_ItemList.AddRange(m_Results);
                }
                if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                    return;
                }
            }
        }
    }
    private void CountUnusedAssets()
    {
        foreach (string ext in m_TypeOrExtList) {
            IList<string> files = m_UnusedAssets;
            if (ext != "*.*") {
                files = m_UnusedAssets.Where((string file) => {
                    return ResourceEditUtility.IsPathMatch(file, ext);
                }).ToArray();
            }
            m_TotalSearchCount += files.Count;
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
            var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.AddRange(m_Results);
            }
            if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                break;
            }
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
                var files = assets;
                if (ext != "*.*") {
                    files = assets.Where((string file) => {
                        return ResourceEditUtility.IsPathMatch(file, ext);
                    }).ToArray();
                }

                foreach (string file in files) {
                    ++m_CurSearchCount;
                    string assetPath = PathToAssetPath(file);
                    var importer = AssetImporter.GetAtPath(assetPath);
                    var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = string.Empty, Importer = importer, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                    var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                    if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                        m_ItemList.AddRange(m_Results);
                    }
                    if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                        return;
                    }
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
                var files = assets;
                if (ext != "*.*") {
                    files = assets.Where((string file) => {
                        return ResourceEditUtility.IsPathMatch(file, ext);
                    }).ToArray();
                }
                m_TotalSearchCount += files.Count();
            }
        }
    }

    private void SearchSceneComponents()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                if (SearchChildComponentsRecursively(string.Empty, obj))
                    return;
            }
        }
    }
    private bool SearchChildComponentsRecursively(string path, GameObject obj)
    {
        bool canceled = false;
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        }
        else {
            path = path + "/" + obj.name;
        }
        ++m_CurSearchCount;
        if (IsMatchedObject(obj)) {
            var comps = obj.GetComponents<Component>();
            foreach (var comp in comps) {
                if (null != comp) {
                    var key = comp.GetType().Name;
                    string assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(obj));
                    AssetImporter importer = null;
                    if (string.IsNullOrEmpty(assetPath)) {
                        assetPath = string.Empty;
                    }
                    else {
                        importer = AssetImporter.GetAtPath(assetPath);
                    }
                    var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = path, Importer = importer, Object = comp, Info = key, Order = m_ItemList.Count, Group = key, Selected = false };
                    var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                    if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                        m_ItemList.AddRange(m_Results);
                    }
                }
            }
        }

        canceled = DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        if (!canceled) {
            var trans = obj.transform;
            int ct = trans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = trans.GetChild(i);
                canceled = SearchChildComponentsRecursively(path, t.gameObject);
                if (canceled)
                    return canceled;
            }
        }
        return canceled;
    }
    private void SearchSceneObjects()
    {
        RefreshSceneDeps();
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                if (SearchChildObjectsRecursively(string.Empty, obj))
                    return;
            }
        }
    }
    private bool SearchChildObjectsRecursively(string path, GameObject obj)
    {
        bool canceled = false;
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        }
        else {
            path = path + "/" + obj.name;
        }
        ++m_CurSearchCount;
        if (IsMatchedObject(obj)) {
            string assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(obj));
            AssetImporter importer = null;
            if (string.IsNullOrEmpty(assetPath)) {
                assetPath = string.Empty;
            }
            else {
                importer = AssetImporter.GetAtPath(assetPath);
            }
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = path, Importer = importer, Object = obj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.AddRange(m_Results);
            }
        }
        canceled = DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount);
        if (!canceled) {
            var trans = obj.transform;
            int ct = trans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = trans.GetChild(i);
                canceled = SearchChildObjectsRecursively(path, t.gameObject);
                if (canceled)
                    return canceled;
            }
        }
        return canceled;
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
        if (m_TypeList.Count > 0) {
            foreach (Type type in m_TypeList) {
                var com = obj.GetComponent(type);
                if (null != com)
                    return true;
            }
            return false;
        }
        else {
            return true;
        }
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

    private void CountExcelRecords()
    {
        m_TotalSearchCount = 0;
        ResourceEditUtility.ParamInfo info;
        if (m_Params.TryGetValue("excel", out info)) {
            string sheetName = string.Empty;
            ResourceEditUtility.ParamInfo sheetNameInfo;
            if (m_Params.TryGetValue("sheetname", out sheetNameInfo) && null != sheetNameInfo.Value) {
                sheetName = sheetNameInfo.Value.ToString();
            }
            if (string.IsNullOrEmpty(sheetName)) {
                sheetName = m_TypeOrExtList.Count > 0 ? m_TypeOrExtList[0] : string.Empty;
            }
            int skipRowNum = 5;
            ResourceEditUtility.ParamInfo skipInfo;
            if (m_Params.TryGetValue("skiprows", out skipInfo) && null != skipInfo.Value) {
                int.TryParse(skipInfo.Value.ToString(), out skipRowNum);
            }
            var file = info.Value as string;
            var path = file;
            if (!File.Exists(path)) {
                path = Path.Combine("../../Product/Excel", file);
            }
            if (File.Exists(path)) {
                var ext = Path.GetExtension(file);
                NPOI.SS.UserModel.IWorkbook book = null;
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    if (ext == ".xls") {
                        book = new NPOI.HSSF.UserModel.HSSFWorkbook(stream);
                    }
                    else {
                        book = new NPOI.XSSF.UserModel.XSSFWorkbook(stream);
                    }
                    int ct = 0;
                    var sheet = book.GetSheet(sheetName);
                    if (null != sheet) {
                        ct += sheet.LastRowNum - sheet.FirstRowNum - skipRowNum;
                    }
                    m_TotalSearchCount = ct;
                    m_WorkBook = book;
                }
            }
        }
    }
    private void SearchExcelRecords()
    {
        if (null != m_WorkBook) {
            string sheetName = string.Empty;
            ResourceEditUtility.ParamInfo sheetNameInfo;
            if (m_Params.TryGetValue("sheetname", out sheetNameInfo) && null != sheetNameInfo.Value) {
                sheetName = sheetNameInfo.Value.ToString();
            }
            if (string.IsNullOrEmpty(sheetName)) {
                sheetName = m_TypeOrExtList.Count > 0 ? m_TypeOrExtList[0] : string.Empty;
            }
            int skipRowNum = 5;
            ResourceEditUtility.ParamInfo skipInfo;
            if (m_Params.TryGetValue("skiprows", out skipInfo) && null != skipInfo.Value) {
                int.TryParse(skipInfo.Value.ToString(), out skipRowNum);
            }
            var book = m_WorkBook;
            if (null != book) {
                var sheet = book.GetSheet(sheetName);
                if (null != sheet) {
                    for (int i = sheet.FirstRowNum + skipRowNum; i <= sheet.LastRowNum; ++i) {
                        ++m_CurSearchCount;
                        var row = sheet.GetRow(i);
                        if (null != row) {
                            var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                            var ret = ResourceEditUtility.Filter(item, new Dictionary<string, object> { { "book", book }, { "sheet", sheet }, { "row", row } }, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                                m_ItemList.AddRange(m_Results);
                            }
                            if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                                goto L_EndExcel;
                            }
                        }
                    }
                }
            }
        }
    L_EndExcel:
        EditorUtility.ClearProgressBar();
    }
    private void CountTableRecords()
    {
        m_TotalSearchCount = 0;
        ResourceEditUtility.ParamInfo info;
        if (m_Params.TryGetValue("table", out info)) {
            string encoding = "utf-8";
            int skipRowNum = 1;
            ResourceEditUtility.ParamInfo encodingInfo;
            if (m_Params.TryGetValue("encoding", out encodingInfo) && null != encodingInfo.Value) {
                encoding = encodingInfo.Value.ToString();
            }
            ResourceEditUtility.ParamInfo skipInfo;
            if (m_Params.TryGetValue("skiprows", out skipInfo) && null != skipInfo.Value) {
                int.TryParse(skipInfo.Value.ToString(), out skipRowNum);
            }
            var file = info.Value as string;
            var path = file;
            if (!File.Exists(path)) {
                path = Path.Combine("../../Product/Table", file);
            }
            if (File.Exists(path)) {
                var ext = Path.GetExtension(file);
                var table = new ResourceEditUtility.DataTable();
                table.Load(path, Encoding.GetEncoding(encoding), ext == ".csv" ? ',' : '\t');
                m_TotalSearchCount = table.RowCount - skipRowNum;
                m_DataTable = table;
            }
        }
    }
    private void SearchTableRecords()
    {
        if (null != m_DataTable) {
            int skipRowNum = 5;
            ResourceEditUtility.ParamInfo skipInfo;
            if (m_Params.TryGetValue("skiprows", out skipInfo) && null != skipInfo.Value) {
                int.TryParse(skipInfo.Value.ToString(), out skipRowNum);
            }
            var table = m_DataTable;
            if (null != table) {
                for (int i = skipRowNum; i < table.RowCount; ++i) {
                    ++m_CurSearchCount;
                    var row = table[i];
                    if (null != row) {
                        var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                        var ret = ResourceEditUtility.Filter(item, new Dictionary<string, object> { { "sheet", table }, { "row", row } }, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                            m_ItemList.AddRange(m_Results);
                        }
                        if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                            goto L_EndTable;
                        }
                    }
                }
            }
        }
    L_EndTable:
        EditorUtility.ClearProgressBar();
    }
    private void SearchScriptResult()
    {
        m_TotalSearchCount = 0;
        ResourceEditUtility.ParamInfo info;
        if (m_Params.TryGetValue("script", out info)) {
            var funcName = info.Value as string;
            if (!string.IsNullOrEmpty(funcName)) {
                var list = CallScript(null, funcName, info) as IList;
                if (null != list) {
                    foreach (var pathObj in list) {
                        var path = pathObj as string;
                        if (!string.IsNullOrEmpty(path)) {
                            var obj = GameObject.Find(path);
                            string assetPath = string.Empty;
                            string scenePath = string.Empty;
                            AssetImporter importer = null;
                            if (null != obj) {
                                scenePath = path;
                                assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(obj));
                                if (string.IsNullOrEmpty(assetPath)) {
                                    assetPath = string.Empty;
                                }
                                else {
                                    importer = AssetImporter.GetAtPath(assetPath);
                                }
                            }
                            else {
                                assetPath = path;
                                importer = AssetImporter.GetAtPath(assetPath);
                            }
                            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = obj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
                            var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
                            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                                m_ItemList.AddRange(m_Results);
                            }
                        }
                    }
                }
            }
        }
    }
    private void SearchList()
    {
        foreach (var path in m_TypeOrExtList) {
            var obj = GameObject.Find(path);
            string assetPath = string.Empty;
            string scenePath = string.Empty;
            AssetImporter importer = null;
            if (null != obj) {
                scenePath = path;
                assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(obj));
                if (string.IsNullOrEmpty(assetPath)) {
                    assetPath = string.Empty;
                }
                else {
                    importer = AssetImporter.GetAtPath(assetPath);
                }
            }
            else {
                assetPath = path;
                importer = AssetImporter.GetAtPath(assetPath);
            }
            var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = obj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Filter(item, null, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.AddRange(m_Results);
            }
        }
    }

    private void SearchSnapshot(bool isBatch)
    {
        string category = string.Empty;
        ResourceEditUtility.ParamInfo categoryParamInfo;
        if (m_Params.TryGetValue("category", out categoryParamInfo)) {
            category = categoryParamInfo.Value as string;
        }
        string _class = string.Empty;
        ResourceEditUtility.ParamInfo classParamInfo;
        if (m_Params.TryGetValue("class", out classParamInfo)) {
            _class = classParamInfo.Value as string;
        }
        if (null == s_CachedSnapshot) {
            s_CachedSnapshot = Unity.MemoryProfilerForExtension.Editor.MemoryProfilerWindow.GetCurCachedSnapshot();
            if (null != s_CachedSnapshot) {
                s_ShortestPathToRootFinder = new ShortestPathToRootObjectFinder(s_CachedSnapshot);
            }
        }
        if ((category == "mgroup" || category == "managed") && m_ClassifiedManagedMemoryInfos.Count <= 0 ||
            (category == "ngroup" || category == "native") && m_ClassifiedNativeMemoryInfos.Count <= 0 ||
            m_ClassifiedManagedMemoryInfos.Count <= 0 && m_ClassifiedNativeMemoryInfos.Count <= 0) {
            if (null != s_CachedSnapshot) {
                AnalyseSnapshot();
            }
            else if (!isBatch) {
                EditorUtility.DisplayDialog("错误", "未找到内存对象信息，请先执行内存‘捕获’或‘加载’！", "ok");
                return;
            }
        }
        m_ItemList.Clear();
        m_CurSearchCount = 0;
        m_TotalSearchCount = 0;
        RefreshSceneDeps();
        if (category == "mgroup") {
            var infos = m_ClassifiedManagedMemoryInfos;
            int curCount = 0;
            int totalCount = 0;
            totalCount = infos.Count;
            foreach (var pair in infos) {
                DoFilterGroupMemoryInfo(pair.Value, infos);
                ++curCount;
                if (curCount % 1000 == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                    goto L_EndSnapshot;
                }
            }
        }
        else if (category == "ngroup") {
            var infos = m_ClassifiedNativeMemoryInfos;
            int curCount = 0;
            int totalCount = 0;
            totalCount = infos.Count;
            foreach (var pair in infos) {
                DoFilterGroupMemoryInfo(pair.Value, infos);
                ++curCount;
                if (curCount % 100 == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                    goto L_EndSnapshot;
                }
            }
        }
        else {
            IDictionary<string, ResourceEditUtility.MemoryGroupInfo> infos = null;
            int delta = 1000;
            if (category == "managed") {
                infos = m_ClassifiedManagedMemoryInfos;
                delta = 1000;
            }
            else if (category == "native") {
                infos = m_ClassifiedNativeMemoryInfos;
                delta = 100;
            }
            else
                infos = null;
            int curCount = 0;
            int totalCount = 0;
            bool handled = false;
            if (null != infos && !string.IsNullOrEmpty(_class)) {
                ResourceEditUtility.MemoryGroupInfo groupInfo;
                if (infos.TryGetValue(_class, out groupInfo)) {
                    totalCount = groupInfo.memories.Count;
                    foreach (var memory in groupInfo.memories) {
                        DoFilterMemoryInfo(memory, groupInfo, infos);
                        ++curCount;
                        if (curCount % delta == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                            goto L_EndSnapshot;
                        }
                    }
                    handled = true;
                }
            }
            if (!handled) {
                if (null != infos) {
                    totalCount = 0;
                    foreach (var pair in infos) {
                        totalCount += pair.Value.memories.Count;
                    }
                    foreach (var pair in infos) {
                        foreach (var memory in pair.Value.memories) {
                            DoFilterMemoryInfo(memory, pair.Value, infos);
                            ++curCount;
                            if (curCount % 1000 == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                                goto L_EndSnapshot;
                            }
                        }
                    }
                }
                else {
                    totalCount = 0;
                    infos = m_ClassifiedManagedMemoryInfos;
                    foreach (var pair in infos) {
                        totalCount += pair.Value.memories.Count;
                    }
                    infos = m_ClassifiedNativeMemoryInfos;
                    foreach (var pair in infos) {
                        totalCount += pair.Value.memories.Count;
                    }
                    infos = m_ClassifiedManagedMemoryInfos;
                    foreach (var pair in infos) {
                        foreach (var memory in pair.Value.memories) {
                            DoFilterMemoryInfo(memory, pair.Value, infos);
                            ++curCount;
                            if (curCount % 1000 == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                                goto L_EndSnapshot;
                            }
                        }
                    }
                    infos = m_ClassifiedNativeMemoryInfos;
                    foreach (var pair in infos) {
                        foreach (var memory in pair.Value.memories) {
                            DoFilterMemoryInfo(memory, pair.Value, infos);
                            ++curCount;
                            if (curCount % 1000 == 0 && DisplayCancelableProgressBar("采集进度", m_ItemList.Count, curCount, totalCount)) {
                                goto L_EndSnapshot;
                            }
                        }
                    }
                }
            }
        }
    L_EndSnapshot:
        EditorUtility.ClearProgressBar();
    }
    private void DoFilterMemoryInfo(ResourceEditUtility.MemoryInfo memory, ResourceEditUtility.MemoryGroupInfo groupInfo, IDictionary<string, ResourceEditUtility.MemoryGroupInfo> infos)
    {
        string assetPath = string.Empty;
        string scenePath = string.Empty;
        UnityEngine.Object assetObj = null;
        AssetImporter importer = null;
        if (null == assetObj && !string.IsNullOrEmpty(scenePath)) {
            assetObj = GameObject.Find(scenePath);
        }
        if (!string.IsNullOrEmpty(assetPath)) {
            importer = AssetImporter.GetAtPath(assetPath);
            if (null == assetObj)
                assetObj = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }
        var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = assetObj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
        var ret = ResourceEditUtility.Filter(item, new Dictionary<string, object> { { "memory", memory }, { "group_info", groupInfo }, { "all_groups", infos } }, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
            m_ItemList.AddRange(m_Results);
        }
    }
    private void DoFilterGroupMemoryInfo(ResourceEditUtility.MemoryGroupInfo groupInfo, IDictionary<string, ResourceEditUtility.MemoryGroupInfo> infos)
    {
        string assetPath = string.Empty;
        string scenePath = string.Empty;
        UnityEngine.Object assetObj = null;
        AssetImporter importer = null;
        if (null == assetObj && !string.IsNullOrEmpty(scenePath)) {
            assetObj = GameObject.Find(scenePath);
        }
        if (!string.IsNullOrEmpty(assetPath)) {
            importer = AssetImporter.GetAtPath(assetPath);
            if (null == assetObj)
                assetObj = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }
        var item = new ResourceEditUtility.ItemInfo { AssetPath = assetPath, ScenePath = scenePath, Importer = importer, Object = assetObj, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
        var ret = ResourceEditUtility.Filter(item, new Dictionary<string, object> { { "group_info", groupInfo }, { "all_groups", infos } }, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
        if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
            m_ItemList.AddRange(m_Results);
        }
    }
    private void SearchInstruments()
    {
        if (m_InstrumentInfos.Count <= 0)
            return;

        m_TotalSearchCount = m_InstrumentInfos.Count;
        foreach (var pair in m_InstrumentInfos) {
            var info = pair.Value;
            ++m_CurSearchCount;
            var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
            var ret = ResourceEditUtility.Filter(item, new Dictionary<string, object> { { "instrument", info } }, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
            if (m_NextFilterIndex <= 0 || null != ret && (int)ret > 0) {
                m_ItemList.AddRange(m_Results);
            }
            if (DisplayCancelableProgressBar("采集进度", m_ItemList.Count, m_CurSearchCount, m_TotalSearchCount)) {
                break;
            }
        }
    }

    private bool RecordInstrumentFrame(int index, int frame, ProfilerColumn sortType, ProfilerViewType viewType, float triangle, float batch)
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

        var item = new ResourceEditUtility.ItemInfo { AssetPath = string.Empty, ScenePath = string.Empty, Info = string.Empty, Order = m_ItemList.Count, Selected = false };
        var addVars = new Dictionary<string, object> { { "instrument", info } };
        var ret = ResourceEditUtility.Filter(item, addVars, m_Results, m_FilterCalculator, m_NextFilterIndex, m_Params, m_SceneDeps, m_ReferenceAssets, m_ReferenceByAssets);
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
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("InstrumentString2Float {0} throw exception:{1}\n{2}", val, ex.Message, ex.StackTrace);
            return 0;
        }
    }

    internal void DisplayProgressBar(string title, int resultCount, int curCount, int totalCount)
    {
        DisplayProgressBar(title, resultCount, curCount, totalCount, true);
    }
    internal void DisplayProgressBar(string title, int resultCount, int curCount, int totalCount, bool batch)
    {
        if (!string.IsNullOrEmpty(m_OverridedProgressTitle))
            title = m_OverridedProgressTitle;
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                EditorUtility.DisplayProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
            }
        }
        else {
            EditorUtility.DisplayProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
        }
    }

    internal void DisplayProgressBar(string title, int curCount, int totalCount)
    {
        DisplayProgressBar(title, curCount, totalCount, true);
    }
    internal void DisplayProgressBar(string title, int curCount, int totalCount, bool batch)
    {
        if (!string.IsNullOrEmpty(m_OverridedProgressTitle))
            title = m_OverridedProgressTitle;
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
            }
        }
        else {
            EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
        }
    }

    internal bool DisplayCancelableProgressBar(string title, int resultCount, int curCount, int totalCount)
    {
        return DisplayCancelableProgressBar(title, resultCount, curCount, totalCount, true);
    }
    internal bool DisplayCancelableProgressBar(string title, int resultCount, int curCount, int totalCount, bool batch)
    {
        if (!string.IsNullOrEmpty(m_OverridedProgressTitle))
            title = m_OverridedProgressTitle;
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
            }
        }
        else {
            return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0} in {1}/{2}", resultCount, curCount, totalCount), curCount * 1.0f / totalCount);
        }
        return false;
    }

    internal bool DisplayCancelableProgressBar(string title, int curCount, int totalCount)
    {
        return DisplayCancelableProgressBar(title, curCount, totalCount, true);
    }
    internal bool DisplayCancelableProgressBar(string title, int curCount, int totalCount, bool batch)
    {
        if (!string.IsNullOrEmpty(m_OverridedProgressTitle))
            title = m_OverridedProgressTitle;
        if (batch && totalCount > 1000) {
            if (curCount % 10 == 0) {
                return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
            }
        }
        else {
            return EditorUtility.DisplayCancelableProgressBar(title, string.Format("{0}/{1}", curCount, totalCount), curCount * 1.0f / totalCount);
        }
        return false;
    }

    private void RefreshSceneDeps()
    {
        m_SceneDeps.Clear();
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var assets = GetDependencies(scene.path);
            foreach (var asset in assets) {
                string name = Path.GetFileName(asset);
                if (!m_SceneDeps.deps.Contains(asset)) {
                    m_SceneDeps.deps.Add(asset);
                }
                if (!m_SceneDeps.name2paths.ContainsKey(name)) {
                    m_SceneDeps.name2paths.Add(name, asset);
                }
            }
        }
    }
    private IEnumerable<string> GetDependencies(string path)
    {
        HashSet<string> list;
        if (m_ReferenceAssets.TryGetValue(path, out list)) {
            return list;
        }
        else {
            return AssetDatabase.GetDependencies(path);
        }
    }

    private bool IsAssetPath(string path)
    {
        return ResourceEditUtility.IsAssetPath(path);
    }
    private string PathToAssetPath(string path)
    {
        return ResourceEditUtility.PathToAssetPath(path);
    }
    private string AssetPathToPath(string assetPath)
    {
        return ResourceEditUtility.AssetPathToPath(assetPath);
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

    private string m_OverridedProgressTitle = string.Empty;

    private string m_DslMenu = string.Empty;
    private string m_DslDescription = string.Empty;
    private string m_SearchSource = string.Empty;
    private List<string> m_TypeOrExtList = new List<string>();
    private List<Type> m_TypeList = new List<Type>();
    private List<string> m_ParamNames = new List<string>();
    private Dictionary<string, ResourceEditUtility.ParamInfo> m_Params = new Dictionary<string, ResourceEditUtility.ParamInfo>();

    private string m_DslPath = null;
    private Dsl.DslFile m_DslFile = null;
    private DslExpression.DslCalculator m_ScriptCalculator = null;
    private DslExpression.DslCalculator m_FilterCalculator = null;
    private DslExpression.DslCalculator m_GroupCalculator = null;
    private DslExpression.DslCalculator m_ProcessCalculator = null;
    private int m_NextFilterIndex = 0;
    private int m_NextGroupIndex = 0;
    private int m_NextProcessIndex = 0;

    private bool m_CanRefresh = false;
    private string m_Text = string.Empty;
    private string m_CollectPath = string.Empty;
    private int m_CurSearchCount = 0;
    private int m_TotalSearchCount = 0;
    private List<ResourceEditUtility.ItemInfo> m_Results = new List<ResourceEditUtility.ItemInfo>();
    private HashSet<string> m_ProcessedAssets = new HashSet<string>();

    private Dictionary<string, Dictionary<string, ResourceEditUtility.ParamInfo>> m_DslParamCaches = new Dictionary<string, Dictionary<string, ResourceEditUtility.ParamInfo>>();
    private NPOI.SS.UserModel.IWorkbook m_WorkBook = null;
    private ResourceEditUtility.DataTable m_DataTable = null;

    private bool m_IsItemListGroupStyle = false;
    private List<ResourceEditUtility.ItemInfo> m_ItemList = new List<ResourceEditUtility.ItemInfo>();
    private List<ResourceEditUtility.GroupInfo> m_GroupList = new List<ResourceEditUtility.GroupInfo>();
    private int m_UnfilteredGroupCount = 0;
    private double m_TotalItemValue = 0;

    private ResourceEditUtility.SceneDepInfo m_SceneDeps = new ResourceEditUtility.SceneDepInfo();
    private Dictionary<string, HashSet<string>> m_ReferenceAssets = new Dictionary<string, HashSet<string>>();
    private Dictionary<string, HashSet<string>> m_ReferenceByAssets = new Dictionary<string, HashSet<string>>();
    private List<string> m_UnusedAssets = new List<string>();
    private SortedDictionary<string, ResourceEditUtility.MemoryGroupInfo> m_ClassifiedNativeMemoryInfos = new SortedDictionary<string, ResourceEditUtility.MemoryGroupInfo>();
    private SortedDictionary<string, ResourceEditUtility.MemoryGroupInfo> m_ClassifiedManagedMemoryInfos = new SortedDictionary<string, ResourceEditUtility.MemoryGroupInfo>();

    private SortedList<int, ResourceEditUtility.InstrumentInfo> m_InstrumentInfos = new SortedList<int, ResourceEditUtility.InstrumentInfo>();
    private int m_RecordIndex = 0;
    private int m_LastFrame = -1;

    internal static bool ReadMenuAndDescription(string path, out string menu, out string desc)
    {
        bool ret = false;
        bool readSource = false;
        bool readMenu = false;
        bool readDesc = false;
        string source = string.Empty;
        menu = string.Empty;
        desc = string.Empty;
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                if (file.DslInfos.Count > 0) {
                    var info = file.DslInfos[0];
                    var first = info.First;
                    if (first.GetId() == "input") {
                        foreach (var comp in first.Statements) {
                            var callData = comp as Dsl.CallData;
                            if (null != callData && callData.GetId() == "feature") {
                                string key = callData.GetParamId(0);
                                string val = callData.GetParamId(1);
                                if (key == "source") {
                                    source = val;
                                    readSource = true;
                                }
                                else if (key == "menu") {
                                    menu = val;
                                    readMenu = true;
                                }
                                else if (key == "description") {
                                    desc = val;
                                    readDesc = true;
                                }
                                if (readSource && readMenu && readDesc) {
                                    break;
                                }
                            }
                        }
                    }
                }
                else {
                    Debug.LogErrorFormat("'{0}' no any DSL info !", path);
                }
            }
            if (string.IsNullOrEmpty(source)) {
                source = "project";
            }
            if (string.IsNullOrEmpty(menu)) {
                menu = string.Format("{0}/{1}", source, Path.GetFileNameWithoutExtension(path));
            }
            if (string.IsNullOrEmpty(desc)) {
                desc = path;
            }
            ret = true;
        }
        return ret;
    }
    internal static void ReadFiltersAndAssetProcessors(string path, List<string> filters, List<string> processors)
    {
        filters.Clear();
        processors.Clear();
        if (!string.IsNullOrEmpty(path)) {
            if (!File.Exists(path)) {
                path = ResourceEditUtility.RelativePathToFilePath(path);
            }
            if (File.Exists(path)) {
                Dsl.DslFile file = new Dsl.DslFile();
                if (file.Load(path, (string msg) => { Debug.LogError(msg); })) {
                    var info = file.DslInfos[0];
                    var first = info.First;
                    var last = info.Last;
                    if (first.GetId() == "input" && last.GetId() == "assetprocessor") {
                        foreach (var param in first.Call.Params) {
                            filters.Add(param.GetId());
                        }
                        foreach (var comp in last.Statements) {
                            var processor = comp.GetId();
                            processors.Add(processor);
                        }
                    }
                }
            }
        }
    }

    private static CachedSnapshot s_CachedSnapshot = null;
    private static ProfilerMarker s_CrawlManagedData = new ProfilerMarker("CrawlManagedData");
    private static ShortestPathToRootObjectFinder s_ShortestPathToRootFinder = null;
    private static readonly HashSet<ObjectData> s_EmptyObjectDataHash = new HashSet<ObjectData>();
    private static readonly List<ObjectData> s_EmptyObjectDataList = new List<ObjectData>();
    private static readonly HashSet<string> s_IgnoredDirs = new HashSet<string> { "plugins", "streamingassets" };
    private static readonly List<string> s_IgnoreDirKeys = new List<string> { "assets/fgui/", "assets/plugins/", "assets/streamingassets/", "/editor default resources/", "assets/thirdparty/" };

    private const string c_pref_key_open_asset_folder = "ResourceProcessor_OpenAssetFolder";
    private const string c_pref_key_load_dependencies = "ResourceProcessor_LoadDependencies";
    private const string c_pref_key_save_dependencies = "ResourceProcessor_SaveDependencies";
    private const string c_pref_key_load_memory = "ResourceProcessor_LoadMemory";
    private const string c_pref_key_save_memory = "ResourceProcessor_SaveMemory";
    private const string c_pref_key_load_instrument = "ResourceProcessor_LoadInstrument";
    private const string c_pref_key_save_instrument = "ResourceProcessor_SaveInstrument";
    private const string c_pref_key_load_result = "ResourceProcessor_LoadResult";
    private const string c_pref_key_save_result = "ResourceProcessor_SaveResult";

    internal static ResourceProcessor Instance
    {
        get { return s_Instance; }
    }
    private static ResourceProcessor s_Instance = new ResourceProcessor();
}