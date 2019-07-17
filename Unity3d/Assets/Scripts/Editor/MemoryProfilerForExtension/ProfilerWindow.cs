using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;
#if UNITY_5_6_OR_NEWER
using UnityEditor.IMGUI.Controls;
#endif
using UnityEngine;
using UnityEditor.TreeViewExamples;
using UnityEditor.MemoryProfiler;
using MemoryProfilerWindowForExtension;

namespace UnityEditor.MemoryProfiler2
{
    class ProfilerWindow : EditorWindow
    {
        private class ManagedGroupInfo
        {
            internal string Group;
            internal string Type;
            internal int Count;
            internal long Size;
        }
        private class NativeGroupInfo
        {
            internal string Type;
            internal int Count;
            internal long Size;
        }

        private static HashSet<string> s_ManagedGroupNames = new HashSet<string> { "CsLibrary", "PluginFramework", "SkillDisplayer", "DisplayerConfigInDll", "TableConfig", "MessageDefine", "StorySystem", "Dsl", "WeTest", "PigeonCoopToolkit", "SLua", "Mono", "Wup", "Cinemachine", "Apollo", "FairyGUI", "UnityEngine", "FMOD", "SevenZip", "System" };
        
        [NonSerialized]
        bool m_Initialized;
        string m_Status = "Profiling";
        bool bshowPlainData = false;

        [NonSerialized]
        UnityEditor.MemoryProfiler.PackedMemorySnapshot _snapshot;
        [SerializeField]
        PackedCrawlerData _packedCrawled;
        [SerializeField]
        private bool _packedCrawlGenerating = false;
        [NonSerialized]
        CrawledMemorySnapshot _unpackedCrawl;
        [NonSerialized]
        private bool _unpackedCrawlGenerating = false;
        [NonSerialized]
        private ProfilerNodeView m_nodeView;

        GUIStyle blueColorStyle;
        GUIStyle redColorStyle;
        GUIStyle greenColorStyle;
        private Vector2 scrollViewVector = Vector2.zero;

        private SortedDictionary<string, ManagedGroupInfo> m_ManagedGroups = new SortedDictionary<string, ManagedGroupInfo>();
        private SortedDictionary<string, NativeGroupInfo> m_NativeGroups = new SortedDictionary<string, NativeGroupInfo>();

        [MenuItem("Dsl资源工具/内存profiler数据")]
        public static ProfilerWindow GetWindow()
        {
            var window = GetWindow<ProfilerWindow>();
            window.titleContent = new GUIContent("MemoryProfiler");
            window.Focus();
            window.Repaint();
            return window;
        }

        Rect topToolbarRect
        {
            get { return new Rect(10f, 0f, position.width * .4f, 20f); }
        }

        Rect searchBarRect
        {
            get { return new Rect(10f, 22f, position.width * .4f, 20f); }
        }

        Rect multiColumnTreeViewRect
        {
            get { return new Rect(10f, 45f, position.width * .4f, position.height - 45f - 20f); }
        }

        public Rect canvasRect
        {
            get { return new Rect(.4f * position.width + 10f, 0f, position.width * .6f, position.height); }
        }

        public Rect fullCanvasRect
        {
            get { return new Rect(0, 20f, position.width, position.height - 20f); }
        }

        Rect bottomToolbarRect
        {
            get { return new Rect(10f, position.height - 20f, position.width * .4f, 16f); }
        }
        public void InitButonStyles()
        {
            blueColorStyle = new GUIStyle(GUI.skin.button);
            blueColorStyle.normal.textColor = Color.green;
            greenColorStyle = new GUIStyle(GUI.skin.button);
            greenColorStyle.normal.textColor = Color.red;
            redColorStyle = new GUIStyle(GUI.skin.button);
            redColorStyle.normal.textColor = Color.yellow;
        }

        void InitIfNeeded()
        {
            InitButonStyles();

            if (!m_Initialized) {
                m_nodeView = new ProfilerNodeView(this);
                // Register the callback for snapshots.
                UnityEditor.MemoryProfiler.MemorySnapshot.OnSnapshotReceived += IncomingSnapshot;

                m_Initialized = true;
            }
        }

        void Unpack()
        {
            _unpackedCrawl = CrawlDataUnpacker.Unpack(_packedCrawled);
            m_Status = "Loading snapshot in Grid .....";
#if !UNITY_5_6_OR_NEWER
            m_nodeView.ClearNodeView();
#endif
            m_nodeView.CreateTreelessView(_unpackedCrawl);
            m_nodeView.bShowMemHeap = false;
            Array.Sort(_unpackedCrawl.nativeObjects, new NativeUnityEngineObjectComparer());
            Array.Sort(_unpackedCrawl.managedObjects, new ManagedObjectComparer());
            m_Status = "Snapshot Loaded!";
        }

        void IncomingSnapshot(PackedMemorySnapshot snapshot)
        {
            m_nodeView.ClearNodeView();
            _snapshot = snapshot;
            _packedCrawled = null;
            _unpackedCrawl = null;
            _packedCrawlGenerating = false;
            _unpackedCrawlGenerating = false;
            m_Status = "Incoming s_unpackedCrawlGenerated = false;napshot.... OK.";
        }

        void OnGUI()
        {
            InitIfNeeded();
            if (null != _snapshot && null == _packedCrawled && !_packedCrawlGenerating) {
                _packedCrawlGenerating = true;
                _packedCrawled = new Crawler().Crawl(_snapshot);
                Unpack();
                m_Status = "Unpacking snapshot.... OK.";
            } else if (null != _packedCrawled && _packedCrawled.valid && null == _unpackedCrawl && !_unpackedCrawlGenerating) {
                _unpackedCrawlGenerating = true;
                Unpack();
                m_Status = "Unpacking snapshot.... OK.";
            }
            if (bshowPlainData) {
                DrawPlainData();
            } else {
                DoCanvasView(fullCanvasRect);
            }
            TopToolBar(topToolbarRect);
            BottomToolBar(bottomToolbarRect);
        }

        void DoCanvasView(Rect rect)
        {
            m_nodeView.DrawProfilerNodeView(rect);
            Repaint();
        }

        void TopToolBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope()) {
                var style = "miniButton";
                if (GUILayout.Button("Take Snapshot", style)) {
                    m_Status = "Taking snapshot.....";
                    UnityEditor.MemoryProfiler.MemorySnapshot.RequestNewSnapshot();
                }

                if (GUILayout.Button("Load Snapshot", style)) {
                    m_Status = "Loading snapshot.....";
                    PackedMemorySnapshot packedSnapshot = PackedMemorySnapshotUtility.LoadFromFile();
                    if (packedSnapshot != null)
                        IncomingSnapshot(packedSnapshot);
                }

                if (_snapshot != null) {
                    if (GUILayout.Button("Save Snapshot", style)) {
                        m_Status = "Saving snapshot.....";
                        PackedMemorySnapshotUtility.SaveToFile(_snapshot);
                    }
                }

                if (GUILayout.Button("Load CrawlerData", style)) {
                    m_Status = "Loading CrawlerData.....";
                    _packedCrawled = PackedMemorySnapshotUtility.LoadCrawlerDataFromFile();
                    if (null != _packedCrawled) {
                        m_nodeView.ClearNodeView();
                        _snapshot = _packedCrawled.packedMemorySnapshot;
                        _unpackedCrawl = null;
                        _packedCrawlGenerating = true;
                        _unpackedCrawlGenerating = false;
                    }
                }

                if (_packedCrawled != null) {
                    if (GUILayout.Button("Save CrawlerData", style)) {
                        m_Status = "Saving CrawlerData.....";
                        PackedMemorySnapshotUtility.SaveCrawlerDataToFile(_packedCrawled);
                    }
                }

                if (_unpackedCrawl != null) {
                    if (GUILayout.Button("Show Heap Usage", style)) {
                        bshowPlainData = false;
                        m_nodeView.ClearNodeView();
                        m_nodeView.CreateTreelessView(_unpackedCrawl);
                    }

                    if (GUILayout.Button("Show Plain Data", style)) {
                        bshowPlainData = true;
                    }
                }
            }
            GUILayout.EndArea();
        }

        void BottomToolBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope()) {
                GUILayout.Label(m_Status);
            }
            GUILayout.EndArea();
        }

        public void DrawPlainData()
        {
            if (_unpackedCrawl != null) {
                GUILayout.Label(" ");
                if (GUILayout.Button("Save managed heap data to an external .csv file", blueColorStyle)) {
                    string exportPath = EditorUtility.SaveFilePanel("Save Snapshot Info", Application.dataPath, "MANAGEDHEAP_SnapshotExport_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv", "csv");
                    if (!String.IsNullOrEmpty(exportPath)) {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(exportPath);
                        sw.WriteLine(" Managed Objects , Size , Address ");
                        for (int i = 0; i < _unpackedCrawl.managedHeap.Length; i++) {
                            MemorySection memorySection = _unpackedCrawl.managedHeap[i];
                            sw.WriteLine("Managed," + memorySection.bytes.Length + "," + memorySection.startAddress);
                        }
                        sw.Flush();
                        sw.Close();
                    }
                }
                if (GUILayout.Button("Save full list of MANAGED objects data to an external .csv file", blueColorStyle)) {
                    string exportPath = EditorUtility.SaveFilePanel("Save Snapshot Info", Application.dataPath, "MANAGED_SnapshotExport_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv", "csv");
                    if (!String.IsNullOrEmpty(exportPath)) {
                        m_ManagedGroups.Clear();
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(exportPath);
                        sw.WriteLine(" Managed Objects , Size , Caption , Type , Number of References (Total), Referenced By (Total), Address ");
                        for (int i = 0; i < _unpackedCrawl.managedObjects.Length; i++) {
                            ManagedObject managedObject = _unpackedCrawl.managedObjects[i];
                            sw.WriteLine("Managed," + managedObject.size + "," + CleanStrings(managedObject.caption) + "," + CleanStrings(managedObject.typeDescription.name) + "," + managedObject.references.Length + "," + managedObject.referencedBy.Length + "," + managedObject.address);

                            string type = managedObject.typeDescription.name;
                            long size = managedObject.size;
                            ManagedGroupInfo info;
                            if (m_ManagedGroups.TryGetValue(type, out info)) {
                                ++info.Count;
                                info.Size += size;
                            } else {
                                string g = string.Empty;
                                int si = type.IndexOf('.');
                                if (si > 0) {
                                    g = type.Substring(0, si);
                                    if (!s_ManagedGroupNames.Contains(g)) {
                                        g = string.Empty;
                                    }
                                }
                                info = new ManagedGroupInfo { Group = g, Type = type, Count = 1, Size = size };
                                m_ManagedGroups.Add(type, info);
                            }
                        }
                        sw.Flush();
                        sw.Close();

                        string dir = Path.GetDirectoryName(exportPath);
                        string fn = Path.GetFileNameWithoutExtension(exportPath);
                        string gpath = Path.Combine(dir, fn + "_groups.csv");
                        var lastGroup = "A000000";
                        using (var outsw = new StreamWriter(gpath)) {
                            outsw.WriteLine("group,type,count,size");
                            foreach (var pair in m_ManagedGroups) {
                                var info = pair.Value;
                                var g = info.Group;
                                if (!string.IsNullOrEmpty(lastGroup) && string.IsNullOrEmpty(info.Group))
                                    g = lastGroup + "__";
                                if (!string.IsNullOrEmpty(info.Group))
                                    lastGroup = info.Group;
                                outsw.WriteLine("{0},\"{1}\",{2},{3}", g, info.Type, info.Count, info.Size);
                            }
                        }
                        string gpath2 = Path.Combine(dir, fn + "_groups_forcmp.csv");
                        using (var outsw = new StreamWriter(gpath2)) {
                            outsw.WriteLine("type,count,size");
                            foreach (var pair in m_ManagedGroups) {
                                var info = pair.Value;
                                outsw.WriteLine("\"{0}\",{1},{2}", info.Type, info.Count, info.Size);
                            }
                        }
                    }
                }
                if (GUILayout.Button("Save full list of NATIVE objects data to an external .csv file", greenColorStyle)) {
                    string exportPath = EditorUtility.SaveFilePanel("Save Snapshot Info", Application.dataPath, "NATIVE_SnapshotExport_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv", "csv");
                    if (!String.IsNullOrEmpty(exportPath)) {
                        m_NativeGroups.Clear();
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(exportPath);
                        sw.WriteLine(" Native Objects , Size , Caption , Class Name , Name , Number of References (Total), Referenced By (Total), InstanceID ");
                        for (int i = 0; i < _unpackedCrawl.nativeObjects.Length; i++) {
                            NativeUnityEngineObject nativeObject = _unpackedCrawl.nativeObjects[i];
                            sw.WriteLine("Native," + nativeObject.size + "," + CleanStrings(nativeObject.caption) + "," + CleanStrings(nativeObject.className) + "," + CleanStrings(nativeObject.name) + "," + nativeObject.references.Length + "," + nativeObject.referencedBy.Length + "," + nativeObject.instanceID);

                            string type = nativeObject.className;
                            long size = nativeObject.size;
                            NativeGroupInfo info;
                            if (m_NativeGroups.TryGetValue(type, out info)) {
                                ++info.Count;
                                info.Size += size;
                            } else {
                                info = new NativeGroupInfo { Type = type, Count = 1, Size = size };
                                m_NativeGroups.Add(type, info);
                            }
                        }
                        sw.Flush();
                        sw.Close();

                        string dir = Path.GetDirectoryName(exportPath);
                        string fn = Path.GetFileNameWithoutExtension(exportPath);
                        string gpath = Path.Combine(dir, fn + "_groups.csv");
                        using (var outsw = new StreamWriter(gpath)) {
                            outsw.WriteLine("type,count,size");
                            foreach (var pair in m_NativeGroups) {
                                var info = pair.Value;
                                outsw.WriteLine("\"{0}\",{1},{2}", info.Type, info.Count, info.Size);
                            }
                        }
                    }
                }
                GUILayout.Label(" ");
                GUILayout.Label("Managed Objects (Total: " + _unpackedCrawl.managedObjects.Length + ") - First 10 Elements: ");
                GUILayout.Label(" ");
                for (int i = 0; i < _unpackedCrawl.managedObjects.Length && i < 10; i++) {
                    ManagedObject managedObject = _unpackedCrawl.managedObjects[i];
                    GUILayout.Label("Address: " + managedObject.address + ", Caption: " + managedObject.caption + ", Size: " + managedObject.size);
                }
                GUILayout.Label(" ");
                GUILayout.Label("Native Objects (Total: " + _unpackedCrawl.nativeObjects.Length + ") - First 10 Elements:");
                GUILayout.Label(" ");
                for (int i = 0; i < _unpackedCrawl.nativeObjects.Length && i < 10; i++) {
                    NativeUnityEngineObject nativeObject = _unpackedCrawl.nativeObjects[i];
                    GUILayout.Label("InstanceID: " + nativeObject.instanceID + ", Name: " + nativeObject.name + ", Size: " + nativeObject.size);
                }
            }
        }

        public string CleanStrings(string text)
        {
            return text.Replace(",", " ");
        }
    }
    
    public class NativeUnityEngineObjectComparer : System.Collections.Generic.IComparer<NativeUnityEngineObject>
    {
        public int Compare(NativeUnityEngineObject x, NativeUnityEngineObject y)
        {
            if (x.instanceID < y.instanceID) return -1;
            if (x.instanceID > y.instanceID) return 1;
            if (x.instanceID == y.instanceID) return 0;

            return 0;
        }
    }

    public class ManagedObjectComparer : System.Collections.Generic.IComparer<ManagedObject>
    {
        public int Compare(ManagedObject x, ManagedObject y)
        {
            if (x.address < y.address) return -1;
            if (x.address > y.address) return 1;
            if (x.address == y.address) return 0;

            return 0;
        }
    }
}
