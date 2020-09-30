using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.MemoryProfilerForExtension.Editor.NativeArrayExtensions;
using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI.MemoryMap
{
    internal class MemoryMapDiff : MemoryMapBase
    {
        [Flags]
        public enum DisplayElements
        {
            RegionDiff = 1 << 0,
            AllocationsDiff = 1 << 1,
            ManagedObjectsDiff = 1 << 2,
            NativeObjectsDiff = 1 << 3,
            VirtualMemory = 1 << 4
        }


        [Flags]
        public enum PresenceInSnapshots
        {
            None = 0,
            First = 1 << 0,
            Second = 1 << 1,
            Both = First | Second
        }

        public struct ViewState
        {
            public ulong BytesInRow;
            public ulong HighlightedAddrMin;
            public ulong HighlightedAddrMax;
            public int DisplayElements;
            public ColorScheme ColorScheme;
            public Vector2 ScrollArea;
        };

        public ViewState CurrentViewState
        {
            get
            {
                ViewState state;
                state.BytesInRow = m_BytesInRow;
                state.HighlightedAddrMin = m_HighlightedAddrMin;
                state.HighlightedAddrMax = m_HighlightedAddrMax;
                state.DisplayElements = m_DisplayElements;
                state.ColorScheme = ActiveColorScheme;
                state.ScrollArea = m_ScrollArea;
                return state;
            }

            set
            {
                m_BytesInRow = value.BytesInRow;
                m_HighlightedAddrMin = value.HighlightedAddrMin;
                m_HighlightedAddrMax = value.HighlightedAddrMax;
                m_DisplayElements = value.DisplayElements;
                m_ColorScheme = value.ColorScheme;
                m_ScrollArea = value.ScrollArea;
            }
        }

        int m_DisplayElements = int.MaxValue & (~(int)DisplayElements.NativeObjectsDiff);

        public int DisplayElement
        {
            get
            {
                return m_DisplayElements;
            }
            set
            {
                m_DisplayElements = value;
                SetupView(m_BytesInRow);
                m_ForceReselect = true;
                ForceRepaint = true;

                UnityEditor.EditorPrefs.SetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMapDiff.DisplayElements", m_DisplayElements);
            }
        }

        public bool GetDisplayElement(DisplayElements element)
        {
            return (m_DisplayElements & (int)element) != 0;
        }

        public void SetDisplayElement(DisplayElements element, bool state)
        {
            if (state)
                DisplayElement = DisplayElement | (int)element;
            else
                DisplayElement = DisplayElement & (~(int)element);
        }

        public void ToggleDisplayElement(DisplayElements element)
        {
            SetDisplayElement(element, !GetDisplayElement(element));
        }

        public enum ColorScheme
        {
            Normal,
            Allocated,
            Deallocated
        };

        ColorScheme m_ColorScheme = ColorScheme.Normal;

        public ColorScheme ActiveColorScheme
        {
            get
            {
                return m_ColorScheme;
            }

            set
            {
                m_ColorScheme = value;
                SetupView(m_BytesInRow);
                m_ForceReselect = true;
                ForceRepaint = true;

                UnityEditor.EditorPrefs.SetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMapDiff.ColorScheme", (int)m_ColorScheme);
            }
        }

        Color[] m_colorNotModified = new Color[3] { new Color(0.30f , 0.30f , 0.40f), new Color(0.28f , 0.28f , 0.28f), new Color(0.28f , 0.28f , 0.28f) };
        Color[] m_colorDeallocated = new Color[3] { new Color(0.60f , 0.00f , 0.00f), new Color(0.34f , 0.28f , 0.28f), new Color(0.60f , 0.00f , 0.00f) };
        Color[] m_colorModified = new Color[3] { new Color(0.60f , 0.60f , 0.00f), new Color(0.60f , 0.60f , 0.00f), new Color(0.60f , 0.60f , 0.00f) };
        Color[] m_colorAllocated = new Color[3] { new Color(0.00f , 0.60f , 0.00f), new Color(0.00f , 0.60f , 0.00f), new Color(0.28f , 0.34f , 0.28f) };

        bool m_ForceReselect = false;

        public void Reselect()
        {
            m_ForceReselect = true;
        }

        public class DiffMemoryRegion : MemoryRegion
        {
            public PresenceInSnapshots m_Snapshot;

            public DiffMemoryRegion(RegionType type, ulong begin, ulong size, string displayName)
                : base(type, begin, size, displayName)
            {
                m_Snapshot = PresenceInSnapshots.None;
            }
        }


        public ulong BytesInRow
        {
            get { return m_BytesInRow; }

            set
            {
                SetupView(value);
                ForceRepaint = true;
                UnityEditor.EditorPrefs.SetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMap.BytesInRow", (int)m_BytesInRow);
            }
        }

        public event Action<ulong, ulong> RegionSelected;

        CachedSnapshot[]    m_Snapshots = new CachedSnapshot[2];
        DiffMemoryRegion[]  m_SnapshotMemoryRegion;
        List<EntryRange>[]  m_GroupsMangedObj = new List<EntryRange>[2] { new List<EntryRange>(), new List<EntryRange>() };
        List<EntryRange>[]  m_GroupsNativeAlloc = new List<EntryRange>[2] { new List<EntryRange>(), new List<EntryRange>() };
        List<EntryRange>[]  m_GroupsNativeObj = new List<EntryRange>[2] { new List<EntryRange>(), new List<EntryRange>() };
        Vector2 m_ScrollArea;
        ulong m_MouseDragStartAddr = 0;

        public MemoryMapDiff()
            : base(1, "Resources/MemoryMapDiff")
        {
            m_BytesInRow = (ulong)UnityEditor.EditorPrefs.GetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMapDiff.BytesInRow", (int)m_BytesInRow);
            m_DisplayElements = UnityEditor.EditorPrefs.GetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMapDiff.DisplayElements", m_DisplayElements);
            m_ColorScheme = (ColorScheme)UnityEditor.EditorPrefs.GetInt("Unity.MemoryProfilerForExtension.Editor.UI.MemoryMapDiff.ColorScheme", (int)m_ColorScheme);
        }

        void SetupSortedData()
        {
            PrepareSortedData(new CachedSnapshot.ISortedEntriesCache[]
            {
                m_Snapshots[0].SortedManagedObjects,
                m_Snapshots[0].SortedNativeAllocations,
                m_Snapshots[0].SortedNativeObjects,
                m_Snapshots[0].SortedNativeRegionsEntries,
                m_Snapshots[0].SortedManagedHeapEntries,
                m_Snapshots[0].SortedManagedStacksEntries,

                m_Snapshots[1].SortedManagedObjects,
                m_Snapshots[1].SortedNativeAllocations,
                m_Snapshots[1].SortedNativeObjects,
                m_Snapshots[1].SortedNativeRegionsEntries,
                m_Snapshots[1].SortedManagedHeapEntries,
                m_Snapshots[1].SortedManagedStacksEntries
            });
        }

        DiffMemoryRegion CreateNativeRegion(int regionIndex, CachedSnapshot snapshot, int i)
        {
            ulong start = snapshot.SortedNativeRegionsEntries.Address(i);
            ulong size = (ulong)snapshot.SortedNativeRegionsEntries.Size(i);
            string name = snapshot.SortedNativeRegionsEntries.Name(i);

            DiffMemoryRegion region;

            if (name.Contains("Virtual Memory"))
            {
                region = new DiffMemoryRegion(RegionType.VirtualMemory, start, size, name);
                region.ColorRegion = m_ColorNative[(int)EntryColors.VirtualMemory];
            }
            else
            {
                region = new DiffMemoryRegion(RegionType.Native, start, size, name);
                region.ColorRegion = m_ColorNative[(int)EntryColors.Region];
            }

            region.ColorRegion = new Color32(region.ColorRegion.r, region.ColorRegion.g, region.ColorRegion.b, (byte)(1 + regionIndex % 255));
            return region;
        }

        DiffMemoryRegion CreateManagedHeapRegion(int regionIndex, CachedSnapshot snapshot, int i)
        {
            ulong start = snapshot.SortedManagedHeapEntries.Address(i);
            ulong size = (ulong)snapshot.SortedManagedHeapEntries.Bytes(i).Length;
            string name = string.Format("Heap Sections {0}", i);

            DiffMemoryRegion region = new DiffMemoryRegion(RegionType.Managed, start, size, name);
            region.ColorRegion = new Color32(0, 0, 0, (byte)(1 + regionIndex % 255));
            return region;
        }

        DiffMemoryRegion CreateManagedStackRegion(int regionIndex, CachedSnapshot snapshot, int i)
        {
            ulong start = snapshot.SortedManagedStacksEntries.Address(i);
            ulong size = (ulong)snapshot.SortedManagedStacksEntries.Bytes(i).Length;
            string name = string.Format("Stack Sections {0}", i);

            DiffMemoryRegion region = new DiffMemoryRegion(RegionType.ManagedStack, start, size, name);
            region.ColorRegion = new Color32(0, 0, 0, (byte)(1 + regionIndex % 255));
            return region;
        }

        void SetupRegions()
        {
            ProgressBarDisplay.UpdateProgress(0.0f, "Flushing regions ...");

            long regionCount = 0;

            for (int snapshotIdx = 0; snapshotIdx < m_Snapshots.Length; ++snapshotIdx)
                regionCount += m_Snapshots[snapshotIdx].nativeMemoryRegions.Count + m_Snapshots[snapshotIdx].managedHeapSections.Count + m_Snapshots[snapshotIdx].managedStacks.Count;

            m_SnapshotMemoryRegion = new DiffMemoryRegion[regionCount];

            int regionIndex = 0;

            int offset = 0;

            uint processed = 0;

            for (int i = 0; i != m_Snapshots[0].SortedNativeRegionsEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                DiffMemoryRegion region = CreateNativeRegion(regionIndex, m_Snapshots[0], i);
                region.m_Snapshot = PresenceInSnapshots.First;
                m_SnapshotMemoryRegion[regionIndex++] = region;
            }

            int offsetMax = regionIndex;

            for (int i = 0; i != m_Snapshots[1].SortedNativeRegionsEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                ulong  addr = m_Snapshots[1].SortedNativeRegionsEntries.Address(i);
                ulong  size = (ulong)m_Snapshots[1].SortedNativeRegionsEntries.Size(i);
                string name = m_Snapshots[1].SortedNativeRegionsEntries.Name(i);

                while (offset < offsetMax && m_SnapshotMemoryRegion[offset].AddressBegin < addr) offset++;

                for (int j = offset; j < offsetMax && m_SnapshotMemoryRegion[j].AddressBegin == addr; ++j)
                {
                    if (m_SnapshotMemoryRegion[j].Name == name && m_SnapshotMemoryRegion[j].Size == size)
                    {
                        m_SnapshotMemoryRegion[j].m_Snapshot |= PresenceInSnapshots.Second;
                        name = null;
                        break;
                    }
                }

                if (name != null)
                {
                    DiffMemoryRegion region = CreateNativeRegion(regionIndex, m_Snapshots[1], i);
                    region.m_Snapshot = PresenceInSnapshots.Second;
                    m_SnapshotMemoryRegion[regionIndex++] = region;
                }
            }

            offset = regionIndex;

            for (int i = 0; i != m_Snapshots[0].SortedManagedHeapEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                DiffMemoryRegion region = CreateManagedHeapRegion(regionIndex, m_Snapshots[0], i);
                region.m_Snapshot = PresenceInSnapshots.First;
                m_SnapshotMemoryRegion[regionIndex++] = region;
            }

            offsetMax = regionIndex;

            for (int i = 0; i != m_Snapshots[1].SortedManagedHeapEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                ulong  addr = m_Snapshots[1].SortedManagedHeapEntries.Address(i);
                ulong  size = (ulong)m_Snapshots[1].SortedManagedHeapEntries.Size(i);

                while (offset < offsetMax && m_SnapshotMemoryRegion[offset].AddressBegin < addr) offset++;

                for (int j = offset; j < offsetMax && m_SnapshotMemoryRegion[j].AddressBegin == addr; ++j)
                {
                    if (m_SnapshotMemoryRegion[j].Size == size)
                    {
                        m_SnapshotMemoryRegion[j].m_Snapshot |= PresenceInSnapshots.Second;
                        addr = size = 0;
                        break;
                    }
                }

                if (addr != 0)
                {
                    DiffMemoryRegion region = CreateManagedHeapRegion(regionIndex, m_Snapshots[1], i);
                    region.m_Snapshot = PresenceInSnapshots.Second;
                    m_SnapshotMemoryRegion[regionIndex++] = region;
                }
            }


            offset = regionIndex;

            for (int i = 0; i != m_Snapshots[0].SortedManagedStacksEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                DiffMemoryRegion region = CreateManagedStackRegion(regionIndex, m_Snapshots[0], i);
                region.m_Snapshot = PresenceInSnapshots.First;
                m_SnapshotMemoryRegion[regionIndex++] = region;
            }

            offsetMax = regionIndex;

            for (int i = 0; i != m_Snapshots[1].SortedManagedStacksEntries.Count; ++i)
            {
                if (processed++ % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)processed / (float)m_SnapshotMemoryRegion.Length);

                ulong  addr = m_Snapshots[1].SortedManagedStacksEntries.Address(i);
                ulong  size = (ulong)m_Snapshots[1].SortedManagedStacksEntries.Size(i);

                while (offset < offsetMax && m_SnapshotMemoryRegion[offset].AddressBegin < addr) offset++;

                for (int j = offset; j < offsetMax && m_SnapshotMemoryRegion[j].AddressBegin == addr; ++j)
                {
                    if (m_SnapshotMemoryRegion[j].Size == size)
                    {
                        m_SnapshotMemoryRegion[j].m_Snapshot |= PresenceInSnapshots.Second;
                        addr = size = 0;
                        break;
                    }
                }

                if (addr != 0)
                {
                    DiffMemoryRegion region = CreateManagedStackRegion(regionIndex, m_Snapshots[1], i);
                    region.m_Snapshot = PresenceInSnapshots.Second;
                    m_SnapshotMemoryRegion[regionIndex++] = region;
                }
            }


            ProgressBarDisplay.UpdateProgress(0.0f, "Sorting regions ..");

            Array.Resize(ref m_SnapshotMemoryRegion, regionIndex);

            Array.Sort(m_SnapshotMemoryRegion, delegate(MemoryRegion a, MemoryRegion b)
            {
                int result = a.AddressBegin.CompareTo(b.AddressBegin);

                if (result == 0)
                    result = -a.AddressEnd.CompareTo(b.AddressEnd);

                return result;
            }
            );
        }

        void CreateGroups()
        {
            m_Groups.Clear();

            ProgressBarDisplay.UpdateProgress(0.0f, "Create groups ...");

            int metaRegions = 0;

            while (m_SnapshotMemoryRegion[metaRegions].AddressBegin == 0 && m_SnapshotMemoryRegion[metaRegions].AddressBegin == m_SnapshotMemoryRegion[metaRegions].AddressEnd)
            {
                metaRegions++;
            }

            int   groupIdx = 0;
            ulong groupAddressBegin = m_SnapshotMemoryRegion[metaRegions].AddressBegin;
            ulong groupAddressEnd = groupAddressBegin;

            for (int i = metaRegions; i < m_SnapshotMemoryRegion.Length; ++i)
            {
                if (i % 10000 == 0) ProgressBarDisplay.UpdateProgress((float)i / (float)m_SnapshotMemoryRegion.Length);

                if (m_SnapshotMemoryRegion[i].Type == RegionType.VirtualMemory && !GetDisplayElement(DisplayElements.VirtualMemory))
                    continue;

                ulong addressBegin = m_SnapshotMemoryRegion[i].AddressBegin;
                ulong addressEnd   = m_SnapshotMemoryRegion[i].AddressEnd;

                if ((addressBegin > groupAddressEnd) && (addressBegin / m_BytesInRow) > (groupAddressEnd / m_BytesInRow) + 1)
                {
                    AddGroup(groupAddressBegin, groupAddressEnd);
                    groupAddressBegin = addressBegin;
                    groupAddressEnd = addressEnd;
                    groupIdx++;
                }
                else
                {
                    groupAddressEnd = Math.Max(groupAddressEnd, addressEnd);
                }

                m_SnapshotMemoryRegion[i].Group = groupIdx;
            }

            AddGroup(groupAddressBegin, groupAddressEnd);

            ProgressBarDisplay.UpdateProgress(1.0f);
        }

        void SetupGroups()
        {
            ProgressBarDisplay.UpdateProgress(0.0f, "Setup groups ...");

            for (Int32 snapshotIdx = 0; snapshotIdx < 2; ++snapshotIdx)
            {
                CachedSnapshot snapshot = m_Snapshots[snapshotIdx];

                int managedObjectsOffset = 0;
                int managedObjectsCount = snapshot.SortedManagedObjects.Count;

                int nativeAllocationsOffset = 0;
                int nativeAllocationsCount = snapshot.SortedNativeAllocations.Count;


                int nativeObjectsOffset = 0;
                int nativeObjectsCount = snapshot.SortedNativeObjects.Count;

                m_GroupsMangedObj[snapshotIdx].Clear();
                m_GroupsNativeAlloc[snapshotIdx].Clear();
                m_GroupsNativeObj[snapshotIdx].Clear();

                EntryRange range;

                for (int i = 0; i < m_Groups.Count; ++i)
                {
                    if (i % 1000 == 0) ProgressBarDisplay.UpdateProgress((float)i / (float)m_Groups.Count);

                    // Assigning Managed Objects Range
                    while (managedObjectsOffset < managedObjectsCount && m_Groups[i].AddressBegin > snapshot.SortedManagedObjects.Address(managedObjectsOffset))
                        managedObjectsOffset++;

                    range.Begin = managedObjectsOffset;

                    while (managedObjectsOffset < managedObjectsCount && snapshot.SortedManagedObjects.Address(managedObjectsOffset) < m_Groups[i].AddressEnd)
                        managedObjectsOffset++;

                    range.End = managedObjectsOffset;

                    m_GroupsMangedObj[snapshotIdx].Add(range);

                    // Assigning Native Allocation Range
                    while (nativeAllocationsOffset < nativeAllocationsCount && m_Groups[i].AddressBegin > snapshot.SortedNativeAllocations.Address(nativeAllocationsOffset))
                        nativeAllocationsOffset++;

                    range.Begin = nativeAllocationsOffset;

                    while (nativeAllocationsOffset < nativeAllocationsCount && snapshot.SortedNativeAllocations.Address(nativeAllocationsOffset) < m_Groups[i].AddressEnd)
                        nativeAllocationsOffset++;

                    range.End = nativeAllocationsOffset;

                    m_GroupsNativeAlloc[snapshotIdx].Add(range);

                    // Assigning Native Objects Range
                    while (nativeObjectsOffset < nativeObjectsCount && m_Groups[i].AddressBegin > snapshot.SortedNativeObjects.Address(nativeObjectsOffset))
                        nativeObjectsOffset++;

                    range.Begin = nativeObjectsOffset;

                    while (nativeObjectsOffset < nativeObjectsCount && snapshot.SortedNativeObjects.Address(nativeObjectsOffset) < m_Groups[i].AddressEnd)
                        nativeObjectsOffset++;

                    range.End = nativeObjectsOffset;

                    m_GroupsNativeObj[snapshotIdx].Add(range);
                }
            }

            ProgressBarDisplay.UpdateProgress(1.0f);
        }

        public void SetupView(ulong rowMemorySize)
        {
            ProgressBarDisplay.ShowBar("Setup memory map diff view ");

            m_BytesInRow = rowMemorySize;

            CreateGroups();

            SetupGroups();

            ProgressBarDisplay.ClearBar();
        }

        public void Setup(CachedSnapshot snapshotA, CachedSnapshot snapshotB)
        {
            m_Snapshots[0] = snapshotA;
            m_Snapshots[1] = snapshotB;

            ProgressBarDisplay.ShowBar("Setup memory map diff");

            SetupSortedData();

            SetupRegions();

            CreateGroups();

            SetupGroups();

            ProgressBarDisplay.ClearBar();
        }

        void RenderRegions(ulong addressMin, ulong addressMax)
        {
            for (int i = 0; i < m_SnapshotMemoryRegion.Length; ++i)
            {
                if (m_SnapshotMemoryRegion[i].Type == RegionType.VirtualMemory && !GetDisplayElement(DisplayElements.VirtualMemory))
                    continue;

                ulong stripGroupAddrBegin = m_SnapshotMemoryRegion[i].AddressBegin.Clamp(addressMin, addressMax);
                ulong stripGroupAddrEnd   = m_SnapshotMemoryRegion[i].AddressEnd.Clamp(addressMin, addressMax);

                if (stripGroupAddrBegin == stripGroupAddrEnd)
                    continue;

                MemoryGroup group = m_Groups[m_SnapshotMemoryRegion[i].Group];

                //RenderStrip(group, stripGroupAddrBegin, stripGroupAddrEnd, m_SnapshotMemoryRegion[i].ColorRegion);

                RenderStrip(group, stripGroupAddrBegin, stripGroupAddrEnd, (Color32 c) =>
                    Max(c, new Color32(0x00, 0x00, 0x00, m_SnapshotMemoryRegion[i].ColorRegion.a))
                );
            }
        }

        void RenderRegionsDiff(ulong addressMin, ulong addressMax)
        {
            for (int i = 0; i < m_SnapshotMemoryRegion.Length; ++i)
            {
                if (m_SnapshotMemoryRegion[i].Type == RegionType.VirtualMemory && !GetDisplayElement(DisplayElements.VirtualMemory))
                    continue;

                ulong stripGroupAddrBegin = m_SnapshotMemoryRegion[i].AddressBegin.Clamp(addressMin, addressMax);
                ulong stripGroupAddrEnd   = m_SnapshotMemoryRegion[i].AddressEnd.Clamp(addressMin, addressMax);

                if (stripGroupAddrBegin == stripGroupAddrEnd)
                    continue;

                PresenceInSnapshots entryMask = m_SnapshotMemoryRegion[i].m_Snapshot;

                MemoryGroup group = m_Groups[m_SnapshotMemoryRegion[i].Group];

                RenderStrip(group, stripGroupAddrBegin, stripGroupAddrEnd, (Color32 c) =>
                    Max(c, new Color32((byte)(entryMask == PresenceInSnapshots.First ? 0xFF : 0x00), (byte)(entryMask == PresenceInSnapshots.Second ? 0xFF : 0x00), (byte)(entryMask == PresenceInSnapshots.Both ? 0xFF : 0x00), m_SnapshotMemoryRegion[i].ColorRegion.a))
                );
            }
        }

        public override void OnRenderMap(ulong addressMin, ulong addressMax, int slot)
        {
            if (GetDisplayElement(DisplayElements.RegionDiff))
            {
                RenderRegionsDiff(addressMin, addressMax);
            }
            else
            {
                RenderRegions(addressMin, addressMax);
            }

            int groupsBegin = int.MaxValue;
            int groupsEnd = int.MinValue;

            for (int i = 0; i < m_Groups.Count; ++i)
            {
                ulong stripGroupAddrBegin = m_Groups[i].AddressBegin.Clamp(addressMin, addressMax);
                ulong stripGroupAddrEnd   = m_Groups[i].AddressEnd.Clamp(addressMin, addressMax);

                if (stripGroupAddrBegin == stripGroupAddrEnd)
                    continue;

                groupsBegin = Math.Min(groupsBegin, i);
                groupsEnd = Math.Max(groupsEnd, i + 1);
            }


            for (int i = groupsBegin; i < groupsEnd; ++i)
            {
                if (GetDisplayElement(DisplayElements.AllocationsDiff))
                {
                    RenderDiff(m_Snapshots[0].SortedNativeAllocations, m_GroupsNativeAlloc[0], m_Snapshots[1].SortedNativeAllocations, m_GroupsNativeAlloc[1], i, addressMin, addressMax);
                }

                if (GetDisplayElement(DisplayElements.NativeObjectsDiff))
                {
                    RenderDiff(m_Snapshots[0].SortedNativeObjects, m_GroupsNativeObj[0], m_Snapshots[1].SortedNativeObjects, m_GroupsNativeObj[1], i, addressMin, addressMax);
                }

                if (GetDisplayElement(DisplayElements.ManagedObjectsDiff))
                {
                    RenderDiff(m_Snapshots[0].SortedManagedObjects, m_GroupsMangedObj[0], m_Snapshots[1].SortedManagedObjects, m_GroupsMangedObj[1], i, addressMin, addressMax);
                }
            }
        }

        void OnGUIView(Rect r, Rect viewRect)
        {
            GUI.BeginGroup(r);

            m_ScrollArea = GUI.BeginScrollView(new Rect(0, 0, r.width, r.height), m_ScrollArea, new Rect(0, 0, viewRect.width - Styles.MemoryMap.VScrollBarWidth, viewRect.height), false, true);

            if (m_ScrollArea.y + r.height > viewRect.height)
                m_ScrollArea.y = Math.Max(0, viewRect.height - r.height);

            FlushTextures(m_ScrollArea.y, m_ScrollArea.y + r.height);

            float viewTop    = m_ScrollArea.y;
            float viewBottom = m_ScrollArea.y + r.height;

            HandleMouseClick(r);

            if (Event.current.type == EventType.Repaint)
            {
                Material mat = BindDefaultMaterial();

                mat.SetColor("_ColorNotModified", m_colorNotModified[(int)m_ColorScheme]);
                mat.SetColor("_ColorDeallocated", m_colorDeallocated[(int)m_ColorScheme]);
                mat.SetColor("_ColorModified",    m_colorModified[(int)m_ColorScheme]);
                mat.SetColor("_ColorAllocated",   m_colorAllocated[(int)m_ColorScheme]);

                RenderGroups(viewTop, viewBottom);
            }

            RenderGroupLabels(viewTop, viewBottom);

            GUI.EndScrollView();

            GUI.EndGroup();
        }

        void OnGUILegend(Rect r)
        {
            Color oldColor = GUI.backgroundColor;

            r.xMin += Styles.MemoryMap.HeaderWidth;
            GUILayout.BeginArea(r);
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();

            GUI.backgroundColor = m_colorNotModified[(int)m_ColorScheme];
            GUILayout.Toggle(true, "Not modified", Styles.MemoryMap.SeriesLabel);
            GUILayout.Space(Styles.MemoryMap.LegendSpacerWidth);

            GUI.backgroundColor = m_colorDeallocated[(int)m_ColorScheme];
            GUILayout.Toggle(true, "Deallocated", Styles.MemoryMap.SeriesLabel);
            GUILayout.Space(Styles.MemoryMap.LegendSpacerWidth);

            GUI.backgroundColor = m_colorModified[(int)m_ColorScheme];
            GUILayout.Toggle(true, "Modified", Styles.MemoryMap.SeriesLabel);
            GUILayout.Space(Styles.MemoryMap.LegendSpacerWidth);

            GUI.backgroundColor = m_colorAllocated[(int)m_ColorScheme];
            GUILayout.Toggle(true, "New Allocations", Styles.MemoryMap.SeriesLabel);
            GUILayout.Space(Styles.MemoryMap.LegendSpacerWidth);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUI.backgroundColor = oldColor;
        }

        public void OnGUI(Rect rect)
        {
            Rect r = new Rect(rect);
            r.y      += Styles.MemoryMap.LegendHeight;
            r.height -= Styles.MemoryMap.LegendHeight;

            Rect viewRect = new Rect(0, 0, r.width, m_Groups[m_Groups.Count - 1].MaxY + Styles.MemoryMap.RowPixelHeight);

            MemoryMapRect = new Rect(
                viewRect.x + Styles.MemoryMap.HeaderWidth,
                viewRect.y,
                viewRect.width - Styles.MemoryMap.HeaderWidth - Styles.MemoryMap.VScrollBarWidth,
                viewRect.height);

            if (MemoryMapRect.width <= 0 || MemoryMapRect.height <= 0)
                return;

            OnGUILegend(new Rect(r.x, rect.y, r.width, Styles.MemoryMap.LegendHeight));

            OnGUIView(r, viewRect);

            if (m_ForceReselect)
            {
                RegionSelected(m_HighlightedAddrMin, m_HighlightedAddrMax);
                m_ForceReselect = false;
            }
        }

        int AddressToRegion(ulong addr)
        {
            int select = -1;
            for (int i = 0; i < m_SnapshotMemoryRegion.Length; ++i)
            {
                MemoryRegion region = m_SnapshotMemoryRegion[i];

                if (addr < region.AddressBegin)
                {
                    break;
                }

                if (region.AddressBegin <= addr && addr < region.AddressEnd)
                {
                    select = i;
                }
            }
            return select;
        }

        void HandleMouseClick(Rect r)
        {
            ulong pixelDragLimit = 2 * m_BytesInRow / (ulong)MemoryMapRect.width;

            if (Event.current.mousePosition.y - m_ScrollArea.y >= r.height)
                return;

            if (Event.current.type == EventType.MouseDown)
            {
                m_MouseDragStartAddr = MouseToAddress(Event.current.mousePosition);
                m_HighlightedAddrMin = m_MouseDragStartAddr;
                m_HighlightedAddrMax = m_MouseDragStartAddr;
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                ulong addr = MouseToAddress(Event.current.mousePosition);;
                m_HighlightedAddrMin = (addr < m_MouseDragStartAddr) ? addr : m_MouseDragStartAddr;
                m_HighlightedAddrMax = (addr < m_MouseDragStartAddr) ? m_MouseDragStartAddr : addr;

                if (m_HighlightedAddrMax - m_HighlightedAddrMin > pixelDragLimit)
                {
                    Event.current.Use();
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                if (m_HighlightedAddrMax - m_HighlightedAddrMin <= pixelDragLimit)
                {
                    if (Event.current.mousePosition.x < Styles.MemoryMap.HeaderWidth)
                    {
                        for (int i = 0; i < m_Groups.Count; ++i)
                        {
                            if (m_Groups[i].Labels[0].TextRect.Contains(Event.current.mousePosition))
                            {
                                m_HighlightedAddrMin = m_Groups[i].AddressBegin;
                                m_HighlightedAddrMax = m_Groups[i].AddressEnd;
                                break;
                            }
                        }
                    }
                    else
                    {
                        m_HighlightedAddrMax = m_HighlightedAddrMin = ulong.MaxValue;
                        int reg = AddressToRegion(m_MouseDragStartAddr);
                        if (reg >= 0)
                        {
                            m_HighlightedAddrMin = m_SnapshotMemoryRegion[reg].AddressBegin;
                            m_HighlightedAddrMax = m_SnapshotMemoryRegion[reg].AddressEnd;
                        }
                    }
                }

                RegionSelected(m_HighlightedAddrMin, m_HighlightedAddrMax);
                Event.current.Use();
            }
        }
    }
}
