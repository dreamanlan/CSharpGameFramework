using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.Net;
using NUnit.Framework.Constraints;
using UnityEditor.MemoryProfiler;
using Object = UnityEngine.Object;
using System.IO;
using MemoryProfilerWindowForExtension;
using UnityEditor.MemoryProfiler2;
using System.Collections;

namespace UnityEditor.MemoryProfiler2
{
    public class ProfilerNodeView
    {
        public List<ProfilerNode> nodes = new List<ProfilerNode>();
        private EditorWindow myParentWindow;
        //Zoom and Pan Navigation vars
        private const float kZoomMin = 0.1f;
        private const float kZoomMax = 10.0f;
        private float _zoom = 1.0f;
        private Vector2 _zoomCoordsOrigin = Vector2.zero;
        private float panX = 0;
        private float panY = 0;
        //Actual Focused Window
        int _focusedWindowId = 0;
        CrawledMemorySnapshot _unpackedCrawl;
        GUIStyle memHeapStyle;
        GUIStyle memHeapStyle2;
        GUIStyle memHeapBarBGStyle;
        GUIStyle memHeapStyleText;
        GUIStyle memTitleStyleText;
        GUIStyle tmpStyle;
        MemoryHeapUsageComparer memComparer1;
        ManageObjectComparer memComparer2;
        public bool bShowMemHeap = false;
        float offsetDialogPosY = 0;
        float tmpPosY = 0;
        GUIStyle colorHelpStyle = new GUIStyle();
        public Vector2 scrollPosition = Vector2.zero;

        private int m_Page = 1;
        private const int c_ItemsPerPage = 8;

        private ulong heapSpace = 0;
        private ulong heapSize = 0;
        private long sized = 0;
        private List<MemoryHeapUsage> memUsageSectors = new List<MemoryHeapUsage>();
        private List<long> memoryValues = new List<long>();
        private List<string> memoryLabelValues = new List<string>();

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public ProfilerNodeView(EditorWindow window)
        {
            myParentWindow = window;

            GUISkin skin = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_DialogGUISkin.guiskin") as GUISkin;
            memHeapStyle2 = new GUIStyle(skin.box);

            GUISkin skin2 = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_MemHeapGUISkin.guiskin") as GUISkin;
            memHeapBarBGStyle = new GUIStyle(skin2.box);

            GUISkin skin3 = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_memSegmentGUISkin.guiskin") as GUISkin;
            memHeapStyle = new GUIStyle(skin3.box);

            GUISkin skin4 = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_DialogGUISkin.guiskin") as GUISkin;
            memTitleStyleText = new GUIStyle(skin4.label);

            GUISkin skin5 = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_MemHeapGUISkin.guiskin") as GUISkin;
            memHeapStyleText = new GUIStyle(skin5.label);

            memComparer1 = new MemoryHeapUsageComparer();
            memComparer2 = new ManageObjectComparer();

            memoryValues = new List<long>();
            for (int i = 0; i < 5; i++) {
                memoryValues.Add(0);
            }
            memoryLabelValues = new List<string>();
            memoryLabelValues.Add("  Total size of native objects found: ");
            memoryLabelValues.Add("  Total size of managed objects found: ");
            memoryLabelValues.Add("  Total size of gc handles found: ");
            memoryLabelValues.Add("  Total size of static fields found: ");
            memoryLabelValues.Add("  Total size of objects found: ");
        }

        public void DrawColorHelp(Rect rect)
        {
            if (_unpackedCrawl != null) {
                /*Rect heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (10), 150, 20);
                colorHelpStyle.normal.textColor = Color.white;
                GUI.Box(new Rect(rect.width, rect.y + (20), 20, 20), GUIContent.none, colorHelpStyle);
                GUI.Label(heapMemBorder, " Color Help: ", colorHelpStyle);*/

                Rect heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (5), 150, 20);
                colorHelpStyle.normal.textColor = Color.white;
                GUI.Box(new Rect(rect.width, rect.y + (20), 20, 20), GUIContent.none, colorHelpStyle);
                GUI.Label(heapMemBorder, " References. ", colorHelpStyle);

                heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (25), 150, 20);
                colorHelpStyle.normal.textColor = new Color(0, 0.7372f, 0.8313f);
                GUI.Label(heapMemBorder, " Referenced By. ", colorHelpStyle);

                heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (45), 150, 20);
                colorHelpStyle.normal.textColor = new Color(0.9568f, 0.2627f, 0.2117f);
                GUI.Label(heapMemBorder, " Native Objects. ", colorHelpStyle);

                heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (65), 150, 20);
                colorHelpStyle.normal.textColor = new Color(0.1294f, 0.5882f, 0.9529f);
                GUI.Label(heapMemBorder, " Managed Objects. ", colorHelpStyle);

                heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (85), 150, 20);
                colorHelpStyle.normal.textColor = new Color(0.5411f, 0.7607f, 0.2862f);
                GUI.Label(heapMemBorder, " GC Handles. ", colorHelpStyle);

                heapMemBorder = new Rect(rect.x + rect.width - 130, rect.y + (105), 150, 20);
                colorHelpStyle.normal.textColor = new Color(1, 0.9215f, 0.2313f);
                GUI.Label(heapMemBorder, " Static Fields. ", colorHelpStyle);
            }
        }


        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000) {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords, Rect rect)
        {
            return (screenCoords - rect.TopLeft()) / _zoom + _zoomCoordsOrigin;
        }

        private void HandleEvents(Rect rect)
        {
            // Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
            // as the zoom center instead of the top left corner of the zoom area. This is achieved by
            // maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
            if (Event.current.type == EventType.ScrollWheel) {
                Vector2 screenCoordsMousePos = Event.current.mousePosition;
                Vector2 delta = Event.current.delta;
                Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos, rect);
                float zoomDelta = -delta.y / 150.0f;
                float oldZoom = _zoom;
                _zoom += zoomDelta;
                _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
                _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

                Event.current.Use();
            }

            // Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
            // with the left mouse button with Alt pressed.
            if (Event.current.type == EventType.MouseDrag &&
                (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt)) {
                Vector2 delta = Event.current.delta;
                delta /= _zoom;
                _zoomCoordsOrigin += delta;

                Event.current.Use();
            }

            if (Event.current.button == 1 && Event.current.type == EventType.MouseDown) {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Clear All"), false, ContextCallback, "deleteNode");
                menu.ShowAsContext();
                Event.current.Use();
            }



            if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag) {
                panX = Event.current.delta.x;
                panY = Event.current.delta.y;
                for (int i = 0; i < nodes.Count; i++) {
                    nodes[i].SetPannedRect(panX, panY);
                }
            }
        }

        public void ClearNodeView()
        {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i] = null;
            }
            nodes.Clear();
        }

        public void DrawProfilerNodeView(Rect rect)
        {
            DrawBackground(rect);
            HandleEvents(rect);
            ProfilerNodeZoomArea.Begin(_zoom, rect, panX, panY);
            DrawProfilerNodes();
            ProfilerNodeZoomArea.End(panX, panY);
            DrawTotalMemory(rect);
            if (bShowMemHeap) {
                DrawHeapMemory(rect);
            } else {
                DrawColorHelp(rect);
            }
        }

        public void DrawBackground(Rect rect)
        {
            GUI.DrawTextureWithTexCoords(
               new Rect(rect),
               Texture2D.blackTexture,
            new Rect(0f, 0f, 20, 20));
        }

        public void DrawProfilerNodes()
        {
            myParentWindow.BeginWindows();
            for (int i = 0; i < nodes.Count; i++) {
                GUI.skin.window = nodes[i].nodeStyle;

                //nodes[i].SetPannedRect(panX,panY);
                nodes[i].DrawCurves();
                nodes[i].windowRect = GUI.Window(i, nodes[i].windowRect, DrawNodeWindow, nodes[i].nodeTitle);

            }
            myParentWindow.EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            GUI.SetNextControlName(id + "");
            nodes[id].DrawNode();
            GUI.DragWindow();
            if (Event.current.GetTypeForControl(id) == EventType.Used) {
                for (int i = 0; i < nodes.Count; i++) {
                    nodes[i].bIsSelected = false;
                }
                _focusedWindowId = id;
                nodes[id].bIsSelected = true;

            }

        }

        void ContextCallback(object obj)
        {
            string clb = obj.ToString();

            if (clb.Equals("deleteNode")) {
                //ProfilerNode selNode = nodes[_focusedWindowId];
                //nodes.RemoveAt(_focusedWindowId);
                ClearNodeView();
            }
        }

        public void DrawTotalMemory(Rect rect)
        {
            if (_unpackedCrawl != null) {
                Rect heapMemBorder = new Rect(rect.x, rect.y + (20), rect.width, 20);
                //GUI.Box(heapMemBorder, GUIContent.none, memHeapStyle2);

                for (int i = 0; i < memoryValues.Count; i++) {
                    memoryValues[i] = 0;
                    switch (i) {
                        case 0:
                            for (int j = 0; j < _unpackedCrawl.nativeObjects.Length; j++) {
                                memoryValues[i] += _unpackedCrawl.nativeObjects[j].size;
                            }
                            break;
                        case 1:
                            for (int j = 0; j < _unpackedCrawl.managedObjects.Length; j++) {
                                memoryValues[i] += _unpackedCrawl.managedObjects[j].size;
                            }
                            break;
                        case 2:
                            for (int j = 0; j < _unpackedCrawl.gcHandles.Length; j++) {
                                memoryValues[i] += _unpackedCrawl.gcHandles[j].size;
                            }
                            break;
                        case 3:
                            for (int j = 0; j < _unpackedCrawl.staticFields.Length; j++) {
                                memoryValues[i] += _unpackedCrawl.staticFields[j].size;
                            }
                            break;
                        case 4:
                            for (int j = 0; j < _unpackedCrawl.allObjects.Length; j++) {
                                memoryValues[i] += _unpackedCrawl.allObjects[j].size;
                            }
                            break;
                    }

                    heapMemBorder = new Rect(rect.x, rect.y + (20 * i), rect.width, 20);
                    if (i == 4) {
                        GUI.Label(heapMemBorder, memoryLabelValues[i] + SizeSuffix(memoryValues[i]), memHeapStyleText);
                    } else {
                        GUI.Label(heapMemBorder, memoryLabelValues[i] + memoryValues[i] + " bytes.", memHeapStyleText);
                    }
                }
            }
        }

        public void DrawHeapMemory(Rect rect)
        {
            if (_unpackedCrawl != null) {
                if (_unpackedCrawl.managedHeap.Length <= 0) {
                    Rect heapMemBorder2 = new Rect(rect.x, (rect.height - ((memUsageSectors.Count + 1.0f) * 2) * 22) + offsetDialogPosY, rect.width, rect.height);
                    GUI.Label(heapMemBorder2, "No Managed Objects found", memTitleStyleText);
                    return;
                }
                if (memUsageSectors.Count <= 0) {
                    heapSpace = 0;
                    heapSize = 0;
                    sized = 0;

                    foreach (var segment in _unpackedCrawl.managedHeap) {
                        MemoryHeapUsage memSector = new MemoryHeapUsage();
                        memSector.section = segment;
                        memUsageSectors.Add(memSector);

                        heapSize += (ulong)segment.bytes.Length;
                    }

                    for (int k = 0; k < memUsageSectors.Count; k++) {
                        foreach (var manage in _unpackedCrawl.managedObjects) {
                            if (manage.address >= memUsageSectors[k].section.startAddress && manage.address <= (memUsageSectors[k].section.startAddress + (ulong)memUsageSectors[k].section.bytes.Length)) {
                                memUsageSectors[k].sectors.Add(manage);
                                memUsageSectors[k].totalSizeOfElements += manage.size;
                            }
                        }
                        if (EditorUtility.DisplayCancelableProgressBar("BuildData", string.Format("{0}/{1}", k, memUsageSectors.Count), k * 1.0f / memUsageSectors.Count))
                            break;
                    }

                    foreach (var manage in _unpackedCrawl.managedObjects) {
                        sized += manage.size;
                    }

                    EditorUtility.ClearProgressBar();

                    memUsageSectors.Sort(memComparer1);
                    for (int h = 0; h < memUsageSectors.Count; h++) {
                        memUsageSectors[h].sectors.Sort(memComparer2);
                    }
                    heapSpace = ((memUsageSectors[memUsageSectors.Count - 1].section.startAddress + (ulong)memUsageSectors[memUsageSectors.Count - 1].section.bytes.Length)) - memUsageSectors[0].section.startAddress;
                }
                
                Rect rt = new Rect(rect.x, 160, 120, 22);
                GUI.Label(rt, string.Format("Total count ({0})", memUsageSectors.Count));
                rt.x += 120;
                rt.width = 100;
                GUI.Label(rt, string.Format("Go to page ({0})", memUsageSectors.Count / c_ItemsPerPage + 1));
                rt.x += 100;
                rt.width = 40;
                string strPage = GUI.TextField(rt, m_Page.ToString());
                rt.x += 40;
                rt.width = 80;
                int.TryParse(strPage, out m_Page);
                if (GUI.Button(rt, "Prev")) {
                    m_Page--;
                }
                rt.x += 80;
                if (GUI.Button(rt, "Next")) {
                    m_Page++;
                }
                m_Page = Mathf.Max(1, Mathf.Min(memUsageSectors.Count / c_ItemsPerPage + 1, m_Page));

                Rect heapMemBorder = new Rect(rect.x, 200, rect.width, 22);
                GUI.Box(heapMemBorder, GUIContent.none, memHeapStyle2);
                GUI.Label(heapMemBorder, string.Format("  Mono Heap (Memory Sections found:{0}) heapSpace:{1} heapSize:{2} realSize:{3}", _unpackedCrawl.managedHeap.Length, heapSpace, heapSize, sized), memTitleStyleText);
                
                int altura = (((c_ItemsPerPage + 1) * 2) * 22);
                scrollPosition = GUI.BeginScrollView(new Rect(0, 220, rect.width, rect.height - 200), scrollPosition, new Rect(0, 0, rect.width - 20, altura));

                for (int h = (m_Page - 1) * c_ItemsPerPage; h < m_Page * c_ItemsPerPage && h < memUsageSectors.Count; h++) {
                    tmpStyle = memHeapStyle;

                    int h0 = h - (m_Page - 1) * c_ItemsPerPage;

                    GUI.Box(new Rect(rect.x, 42 + (22 * h0) + (h0 * 20), rect.width, 15), GUIContent.none, memHeapBarBGStyle);

                    Rect labelPos = new Rect(rect.x, 22 + (22 * h0) + (h0 * 20), rect.width, 30);
                    GUI.Label(labelPos, " Memory sector " + h + ": " + memUsageSectors[h].section.startAddress + " - Size " + SizeSuffix(memUsageSectors[h].section.bytes.Length, 2) + " - Elements Size: " + memUsageSectors[h].totalSizeOfElements + " - # elements in this section: " + memUsageSectors[h].sectors.Count, memHeapStyleText);

                    for (int r = 0; r < memUsageSectors[h].sectors.Count; r++) {
                        float zoomed = ((float)memUsageSectors[h].section.bytes.Length) / (rect.width - 20);
                        Rect position = new Rect(rect.x + ((memUsageSectors[h].sectors[r].address - memUsageSectors[h].section.startAddress) / zoomed), 22 + 20 + (22 * h0) + (h0 * 20), memUsageSectors[h].sectors[r].size / zoomed, 15);
                        GUI.Box(position, GUIContent.none, tmpStyle);
                    }
                }

                GUI.EndScrollView();
            }
        }

        public static void DrawNodeCurve(Rect start, Rect end, Color elColor)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 256;
            Vector3 endTan = endPos + Vector3.left * 256;

            Handles.DrawBezier(startPos, endPos, startTan, endTan, elColor, null, 5);
        }

        public void CreateObjectInfoNode(ProfilerNode objectInfoNode, float nodeRefPosX = 0, float nodeRefPosY = 0, bool bCreateInGrid = true)
        {
            if (nodes.Count > 0) {
                float instancingPosX = nodes[nodes.Count - 1].windowRect.x;
                float instancingPosY = nodes[nodes.Count - 1].windowRect.y;
                if (bCreateInGrid) {
                    if (nodes.Count % 4 == 0) {
                        instancingPosX = nodes[0].windowRect.x;
                        instancingPosY = nodes[nodes.Count - 1].windowRect.y + 460;
                    }
                } else {
                    instancingPosX = nodeRefPosX - 60;
                    instancingPosY = nodeRefPosY - 100;
                }
                objectInfoNode.windowRect = new Rect(instancingPosX + 460, instancingPosY, 360, 360);
                objectInfoNode.originalPosX = instancingPosX + 460;
                objectInfoNode.originalPosY = instancingPosY;
            } else {
                objectInfoNode.windowRect = new Rect(460, 360, 360, 360);
                objectInfoNode.originalPosX = 460;
                objectInfoNode.originalPosY = 360;
            }
            nodes.Add(objectInfoNode);
        }

        public void SetSelectedNode(int id)
        {
            GUI.FocusControl(id + "");
        }

        public int FindExistingObjectByInstanceID(int id)
        {
            for (int i = 0; i < nodes.Count; i++) {
                var nativeObject = nodes[i].myInfo.memObject as NativeUnityEngineObject;
                if (nativeObject != null) {
                    if (nodes[i].instanceID == id) {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int FindExistingObjectByAddress(UInt64 id)
        {
            for (int i = 0; i < nodes.Count; i++) {
                var manageObject = nodes[i].myInfo.memObject as ManagedObject;
                if (manageObject != null) {
                    if (manageObject.address == id) {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void CreateNode(ThingInMemory memData, CrawledMemorySnapshot unpackedCrawl)
        {
            if (_unpackedCrawl == null) {
                _unpackedCrawl = unpackedCrawl;
            }
            ProfilerNodeObjectInfo info = new ProfilerNodeObjectInfo();
            info.memObject = memData;
            ProfilerNode objectInfoNode = new ProfilerNode(info, _unpackedCrawl, this, true, null);
            CreateObjectInfoNode(objectInfoNode);
        }

        public void CreateTreelessView(CrawledMemorySnapshot unpackedCrawl)
        {
            _unpackedCrawl = unpackedCrawl;
            bShowMemHeap = true;
            memUsageSectors.Clear();
        }
    }
}

public class MemoryHeapUsage
{
    public MemorySection section;
    public List<ManagedObject> sectors = new List<ManagedObject>();
    public long totalSizeOfElements;
}

public class MemoryHeapUsageComparer : IComparer<MemoryHeapUsage>
{
    public int Compare(MemoryHeapUsage x, MemoryHeapUsage y)
    {
        if (x.section.startAddress > y.section.startAddress) {
            return 1;
        }
        if (x.section.startAddress < y.section.startAddress) {
            return -1;
        }
        return 0;
    }
}

public class ManageObjectComparer : IComparer<ManagedObject>
{
    public int Compare(ManagedObject x, ManagedObject y)
    {
        if (x.address > y.address) {
            return 1;
        }
        if (x.address < y.address) {
            return -1;
        }
        return 0;
    }
}