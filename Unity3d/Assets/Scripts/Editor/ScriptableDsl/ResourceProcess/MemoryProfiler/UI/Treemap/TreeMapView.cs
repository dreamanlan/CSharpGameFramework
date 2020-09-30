using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace Unity.MemoryProfilerForExtension.Editor.UI.Treemap
{
    internal class TreeMapView
    {
        public CachedSnapshot m_Snapshot;
        public ZoomArea m_ZoomArea;
        private Dictionary<string, Group> _groups = new Dictionary<string, Group>();
        private List<Item> _items = new List<Item>();
        private Mesh2D m_Mesh;
        private Mesh2D m_SelectionMesh;
        private Mesh2D m_SelectionGroupMesh;
        private Item _selectedItem;
        private Group _selectedGroup;
        private Item _mouseDownItem;

        private Vector2 mouseTreemapPosition { get { return m_ZoomArea.mousePositionInWorld; } } //_ZoomArea.ViewToDrawingTransformPoint(Event.current.mousePosition); } }

        public delegate void OnOpenItemDelegate(Item a);
        public OnOpenItemDelegate OnOpenItem;

        public delegate void OnClickItemDelegate(Item a);
        public OnClickItemDelegate OnClickItem;

        public delegate void OnClickGroupDelegate(Group a);
        public OnClickGroupDelegate OnClickGroup;

        public TreeMapView(CachedSnapshot snapshot)
        {
            m_Snapshot = snapshot;
        }

        public void Setup()//(MemoryProfilerWindow hostWindow, CrawledMemorySnapshot _unpackedCrawl)
        {
            m_ZoomArea = new ZoomArea();
            m_ZoomArea.resizeWorld(new Rect(-100, -100, 200, 200));
            //RefreshCaches();
            RefreshMesh();
        }

        public Item GetItemByObjectUID(int objectUID)
        {
            return _items.Find(x => x.Metric.GetObjectUID() == objectUID);
        }

        public Group FindGroup(string name)
        {
            Group group = null;
            if (!_groups.TryGetValue(name, out group))
                return null;
            return group;
        }

        public bool IsAnimated()
        {
            return m_ZoomArea.mbAnimated;
        }

        public void OnGUI(Rect r)
        {
            if (r != m_ZoomArea.m_ViewSpace)
            {
                m_ZoomArea.resizeView(r);
            }

            m_ZoomArea.BeginViewGUI();
            GUI.BeginGroup(r);
            var oldMatrix = Handles.matrix;
            Handles.matrix = m_ZoomArea.worldToViewMatrix;// _ZoomArea.drawingToViewMatrix;
            HandleMouseClick();
            RenderTreemap();
            Handles.matrix = oldMatrix;
            GUI.EndGroup();

            m_ZoomArea.EndViewGUI();
        }

        private void OnHoveredGroupChanged()
        {
            UpdateGroupRect();
        }

        private void HandleMouseClick()
        {
            if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) && Event.current.button == 0)
            {
                if (Event.current.mousePosition.x < m_ZoomArea.m_ViewSpace.width && Event.current.mousePosition.y < m_ZoomArea.m_ViewSpace.height)
                {
                    Group group = _groups.Values.FirstOrDefault(i => i.Position.Contains(mouseTreemapPosition));
                    Item item = _items.FirstOrDefault(i => i.Position.Contains(mouseTreemapPosition));

                    if (item != null && _selectedGroup == item.Group)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseDown:
                                _mouseDownItem = item;
                                break;

                            case EventType.MouseUp:
                                if (_mouseDownItem == item)
                                {
                                    if (_selectedItem == item)
                                    {
                                        if (OnOpenItem != null)
                                        {
                                            OnOpenItem(item);
                                        }
                                    }
                                    else
                                    {
                                        if (OnClickItem != null)
                                        {
                                            OnClickItem(item);
                                        }
                                    }
                                    //_hostWindow.SelectThing(item._thingInMemory);
                                    Event.current.Use();
                                }
                                break;
                        }
                    }
                    else if (group != null)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseUp:
                                if (OnClickGroup != null)
                                {
                                    OnClickGroup(group);
                                }
                                //_hostWindow.SelectGroup(group);
                                Event.current.Use();
                                break;
                        }
                    }
                }
            }
        }

        public void SelectThing(ObjectMetric thing)
        {
            var item = _items.First(i => i.Metric.Equals(thing));
            SelectItem(item);
        }

        public void SelectGroup(Group group)
        {
            _selectedItem = null;
            _selectedGroup = group;
            UpdateItemRectOfGroup(_selectedGroup);
            RefreshGroupMesh(_selectedGroup);

            if (group.Items.Count == 1)
            {
                if (OnClickItem != null)
                {
                    OnClickItem(group.Items[0]);
                }
            }
            RefreshSelectionMesh();
        }

        public void SelectItem(Item item)
        {
            _selectedItem = item;
            if (_selectedGroup != item.Group)
            {
                _selectedGroup = item.Group;
                UpdateItemRectOfGroup(_selectedGroup);
                RefreshGroupMesh(_selectedGroup);
            }
            RefreshSelectionMesh();
        }

        private void OnMidAnim()
        {
            if (itemToFocus != null)
            {
                SelectItem(itemToFocus);
                itemToFocus = null;
            }
        }

        Item itemToFocus;
        public void FocusOnItem(Item item, bool select)
        {
            if (_selectedGroup != item.Group)
            {
                UpdateItemRectOfGroup(item.Group);
            }
            if (select)
            {
                itemToFocus = item;
            }
            else
            {
                itemToFocus = null;
            }
            //float minRatioWanted = 0.2f;
            //float maxRatioWanted = 0.5f;
            var r = item.Position;
            var rWorld = m_ZoomArea.m_WorldSpace;
            var xRatio = rWorld.width / r.width;
            var yRatio = rWorld.height / r.height;
            var minRatio = Mathf.Min(xRatio, yRatio);
            //minRatioWanted / maxRatio;
            m_ZoomArea.FocusTo(minRatio * 0.2f, r.center, OnMidAnim, false);
            //r.width
        }

        public void FocusOnAll()
        {
            m_ZoomArea.FocusTo(1, Vector2.zero, null, true);
        }

        public Item SelectedItem
        {
            get
            {
                return _selectedItem;
            }
        }

        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
        }

        public void ClearMetric()
        {
            _items.Clear();
            _groups.Clear();
        }

        public bool HasMetric(ObjectMetric metric)
        {
            var i = _items.FindIndex(x => x.Metric.Equals(metric));
            return i >= 0;
        }

        public void AddMetric(ObjectMetric metric)
        {
            var groupName = metric.GetTypeName();
            if (!_groups.ContainsKey(groupName))
            {
                Group newGroup = new Group();
                newGroup.Name = groupName;
                newGroup.Items = new List<Item>();
                _groups.Add(groupName, newGroup);
            }
            Item item = new Item(metric, _groups[groupName]);
            _items.Add(item);
            _groups[groupName].Items.Add(item);
        }

        public void UpdateMetric()
        {
            foreach (Group group in _groups.Values)
            {
                group.Items.Sort();
            }

            _items.Sort();
            UpdateGroupRect();
        }

        private void UpdateGroupRect()
        {
            Rect space = new Rect(-100f, -100f, 200f, 200f);

            List<Group> groups = _groups.Values.ToList();
            groups.Sort();
            float[] groupTotalValues = new float[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                groupTotalValues[i] = groups.ElementAt(i).TotalValue;
            }

            Rect[] groupRects = Utility.GetTreemapRects(groupTotalValues, space);
            for (int groupIndex = 0; groupIndex < groupRects.Length; groupIndex++)
            {
                Group group = groups[groupIndex];
                group.Position = groupRects[groupIndex];
            }

            RefreshMesh();
        }

        private void UpdateItemRectOfGroup(Group g)
        {
            Rect[] rects = Utility.GetTreemapRects(g.MemorySizes, g.Position);
            for (int i = 0; i < rects.Length; i++)
            {
                g.Items[i].Position = rects[i];
            }
        }

        public void CleanupMeshes()
        {
            if (m_Mesh != null) m_Mesh.CleanupMeshes();
            m_Mesh = null;
            if (m_SelectionMesh != null) m_SelectionMesh.CleanupMeshes();
            m_SelectionMesh = null;
            if (m_SelectionGroupMesh != null) m_SelectionGroupMesh.CleanupMeshes();
            m_SelectionGroupMesh = null;
        }

        private void RefreshSelectionMesh()
        {
            if (m_SelectionMesh != null) m_SelectionMesh.CleanupMeshes();
            MeshBuilder mb = new MeshBuilder();
            if (_selectedItem != null)
            {
                mb.Add(0, new MeshBuilder.Rectangle(_selectedItem.Position, Color.white), false);
            }
            else if (_selectedGroup != null)
            {
                mb.Add(0, new MeshBuilder.Rectangle(_selectedGroup.Position, Color.white * 0.8f), false);
            }
            m_SelectionMesh = mb.CreateMesh();
        }

        private void RefreshGroupMesh(Group group)
        {
            if (m_SelectionGroupMesh != null) m_SelectionGroupMesh.CleanupMeshes();

            MeshBuilder mb = new MeshBuilder();

            foreach (Item item in group.Items)
            {
                Rect r = item.Position;
                var color = item.Color;
                mb.Add(0, new MeshBuilder.Rectangle(r, color, color * 0.75f, color * 0.5f, color * 0.75f), true);
            }

            m_SelectionGroupMesh = mb.CreateMesh();
        }

        private void RefreshMesh()
        {
            if (m_Mesh != null) m_Mesh.CleanupMeshes();

            MeshBuilder mb = new MeshBuilder();

            foreach (Group group in _groups.Values)
            {
                Rect r = group.Position;
                var color = group.Color;
                mb.Add(0, new MeshBuilder.Rectangle(r, color, color * 0.75f, color * 0.5f, color * 0.75f), true);
            }

            m_Mesh = mb.CreateMesh();
        }

        public void RenderTreemap()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            if (m_Mesh != null) m_Mesh.Render();
            if (m_SelectionGroupMesh != null) m_SelectionGroupMesh.Render();
            if (m_SelectionMesh != null) m_SelectionMesh.Render();


            RenderLabels();
        }

        private void RenderLabels()
        {
            if (_groups == null)
                return;

            GUI.color = Color.black;
            var mat = m_ZoomArea.worldToViewMatrix; //_ZoomArea.drawingToViewMatrix;

            foreach (var group in _groups.Values)
            {
                if (Utility.IsInside(group.Position, m_ZoomArea.ViewInWorldSpace)) //_ZoomArea.shownArea))
                {
                    if (_selectedGroup == group)
                    {
                        RenderGroupItems(group, ref mat);
                    }
                    else
                    {
                        RenderGroupLabel(group, ref mat);
                    }
                }
            }

            GUI.color = Color.white;
        }

        float k_MinWidthForLables = 30;
        float k_MinHeightForLables = 16;

        private void RenderGroupLabel(Group group, ref Matrix4x4 mat)
        {
            Vector3 p1 = mat.MultiplyPoint(new Vector3(group.Position.xMin, group.Position.yMin));
            Vector3 p2 = mat.MultiplyPoint(new Vector3(group.Position.xMax, group.Position.yMax));

            if (p2.x - p1.x > k_MinWidthForLables && p1.y - p2.y > k_MinHeightForLables)
            {
                Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                GUI.Label(rect, group.Label);
            }
        }

        private void RenderGroupItems(Group group, ref Matrix4x4 mat)
        {
            var viewInWorldSpace = m_ZoomArea.ViewInWorldSpace;
            foreach (var item in group.Items)
            {
                if (Utility.IsInside(item.Position, viewInWorldSpace))  //_ZoomArea.shownArea))
                {
                    Vector3 p1 = mat.MultiplyPoint(new Vector3(item.Position.xMin, item.Position.yMin));
                    Vector3 p2 = mat.MultiplyPoint(new Vector3(item.Position.xMax, item.Position.yMax));

                    if (p2.x - p1.x > k_MinWidthForLables && p1.y - p2.y > k_MinHeightForLables)
                    {
                        Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                        GUI.Label(rect, item.Label);
                    }
                }
            }
        }
    }
}
