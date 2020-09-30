using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.MemoryProfilerForExtension.Editor.NativeArrayExtensions;
using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI.MemoryMap
{
    internal abstract class MemoryMapBase
    {        
        public const int k_RowCacheSize = 512;       // Height of texture that store visible rows + extra data to preload data.
        protected ulong m_BytesInRow = 8 * 1024 * 1024;
        protected ulong m_HighlightedAddrMin = ulong.MinValue;
        protected ulong m_HighlightedAddrMax = ulong.MaxValue;

        Material m_Material;
        Texture2D m_Texture;
        public Rect MemoryMapRect = new Rect();

        long m_MinCachedRow = 0;
        long m_MaxCachedRow = 0;
        int m_MinCachedGroup = 0;
        int m_MaxCachedGroup = 0;

        public bool ForceRepaint { get; set; }

        NativeArray<Color32> m_RawTexture;

        protected List<MemoryGroup> m_Groups = new List<MemoryGroup>();


        public struct AddressLabel
        {
            public string Text;
            public Rect TextRect;
        }

        public struct EntryRange
        {
            public int Begin;
            public int End;
        }

        protected enum EntryColors
        {
            Region = 0,
            Allocation = 1,
            Object = 2,
            VirtualMemory = 3
        }


        protected Color[] m_ColorNative;
        protected Color[] m_ColorManaged;
        protected Color[] m_ColorManagedStack;

        public struct MemoryGroup
        {
            public ulong AddressBegin;
            public ulong AddressEnd;
            public float RowsOffsetY;
            public ulong RowsOffset;
            public ulong RowsCount;
            public AddressLabel[] Labels;
            public float MinY { get { return RowsOffsetY; } }
            public float MaxY { get { return RowsOffsetY + (float)RowsCount * Styles.MemoryMap.RowPixelHeight; } }
            public long RowsStart { get { return (long)RowsOffset; } }
            public long RowsEnd { get { return (long)(RowsOffset + RowsCount); } }
        }

        string m_MaterialName;

        Texture2D[] m_TextureSlots;

        public MemoryMapBase(int textureSlots = 1, string materialName = "Resources/MemoryMap")
        {
            ForceRepaint = false;

            m_MaterialName = materialName;

            m_TextureSlots = new Texture2D[textureSlots];

            m_ColorNative = new Color[Enum.GetNames(typeof(EntryColors)).Length];
            m_ColorNative[(int)EntryColors.Allocation] = ProfilerColors.currentColors[(int)RegionType.Native];
            m_ColorNative[(int)EntryColors.Region] = new Color(m_ColorNative[(int)EntryColors.Allocation].r * 0.6f, m_ColorNative[(int)EntryColors.Allocation].g * 0.6f, m_ColorNative[(int)EntryColors.Allocation].b * 0.6f, 1.0f);
            m_ColorNative[(int)EntryColors.Object] = new Color(1, 1, 0, 1.0f);
            m_ColorNative[(int)EntryColors.VirtualMemory] = new Color(m_ColorNative[(int)EntryColors.Allocation].r * 0.3f, m_ColorNative[(int)EntryColors.Allocation].g * 0.3f, m_ColorNative[(int)EntryColors.Allocation].b * 0.3f, 1.0f);

            m_ColorManaged = new Color[Enum.GetNames(typeof(EntryColors)).Length];
            m_ColorManaged[(int)EntryColors.Allocation] = new Color(0.05f, 0.40f, 0.55f, 1.0f);
            m_ColorManaged[(int)EntryColors.Region] = new Color(0.05f, 0.40f, 0.55f, 1.0f);
            m_ColorManaged[(int)EntryColors.Object] = new Color(0.45f, 0.80f, 0.95f, 1.0f);//ProfilerColors.currentColors[(int)RegionType.Managed];
            m_ColorManaged[(int)EntryColors.VirtualMemory] = new Color(m_ColorManaged[(int)EntryColors.Allocation].r * 0.3f, m_ColorManaged[(int)EntryColors.Allocation].g * 0.3f, m_ColorManaged[(int)EntryColors.Allocation].b * 0.3f, 1.0f);

            m_ColorManagedStack = new Color[Enum.GetNames(typeof(EntryColors)).Length];
            m_ColorManagedStack[(int)EntryColors.Allocation] = ProfilerColors.currentColors[(int)RegionType.ManagedStack];
            m_ColorManagedStack[(int)EntryColors.Region] = ProfilerColors.currentColors[(int)RegionType.ManagedStack];
            m_ColorManagedStack[(int)EntryColors.Object] = new Color(0.87f, 0.29f, 0.68f, 1.0f);
            m_ColorManagedStack[(int)EntryColors.VirtualMemory] = new Color(m_ColorManagedStack[(int)EntryColors.Allocation].r * 0.3f, m_ColorManagedStack[(int)EntryColors.Allocation].g * 0.3f, m_ColorManagedStack[(int)EntryColors.Allocation].b * 0.3f, 1.0f);
        }

        protected void PrepareSortedData(CachedSnapshot.ISortedEntriesCache[] caches)
        {
            ProgressBarDisplay.UpdateProgress(0.0f, "Sorting data ...");

            long entriesCount = 0;
            long entriesProcessed = 0;

            for (int i = 0; i < caches.Length; ++i)
            {
                entriesCount += caches[i].Count;
            }

            for (int i = 0; i < caches.Length; ++i)
            {
                CachedSnapshot.ISortedEntriesCache cache = caches[i];
                cache.Preload();
                entriesProcessed += cache.Count;
                ProgressBarDisplay.UpdateProgress((float)((100 * entriesProcessed) / entriesCount) / 100.0f);
            }
        }

        public void AddGroup(ulong beginRow, ulong endRow)
        {
            float       prevGroupSpace = 1.0f;

            MemoryGroup prevGroup;

            if (m_Groups.Count == 0)
            {
                prevGroup.RowsOffsetY = 0;
                prevGroup.RowsOffset = 0;
                prevGroup.RowsCount = 0;
                prevGroupSpace = 0.25f;
            }
            else
            {
                prevGroup = m_Groups[m_Groups.Count - 1];
            }

            MemoryGroup group;
            group.AddressBegin  = (beginRow / m_BytesInRow) * m_BytesInRow;
            group.AddressEnd    = ((endRow + m_BytesInRow - 1) / m_BytesInRow) * m_BytesInRow;
            group.RowsOffsetY   = prevGroup.RowsOffsetY + prevGroup.RowsCount * Styles.MemoryMap.RowPixelHeight +  prevGroupSpace * Styles.MemoryMap.HeaderHeight;
            group.RowsOffset    = prevGroup.RowsOffset + prevGroup.RowsCount;
            group.RowsCount     = (group.AddressEnd - group.AddressBegin) / m_BytesInRow;

            group.Labels = new AddressLabel[1 + (group.RowsCount - 1) / Styles.MemoryMap.SubAddressStepInRows];
            group.Labels[0].TextRect = new Rect(16.0f, group.RowsOffsetY - 0.25f * Styles.MemoryMap.HeaderHeight, Styles.MemoryMap.HeaderWidth, Styles.MemoryMap.HeaderHeight);
            group.Labels[0].Text = String.Format("0x{0:X15}", group.AddressBegin);

            for (int i = 1; i < group.Labels.Length; ++i)
            {
                group.Labels[i].TextRect = new Rect(4.0f, group.RowsOffsetY - 0.90f * Styles.MemoryMap.HeaderHeight + (float)i * Styles.MemoryMap.SubAddressStepInRows * Styles.MemoryMap.RowPixelHeight, Styles.MemoryMap.HeaderWidth, Styles.MemoryMap.HeaderHeight);
                group.Labels[i].Text = String.Format("0x{0:X15}", group.AddressBegin + (ulong)i * Styles.MemoryMap.SubAddressStepInRows * m_BytesInRow);
            }

            m_Groups.Add(group);
        }

        int FindFirstGroup(float yMin, int startGroup)
        {
            int minGroup = startGroup;

            while (minGroup < m_Groups.Count && m_Groups[minGroup].RowsOffsetY + m_Groups[minGroup].RowsCount * Styles.MemoryMap.RowPixelHeight < yMin)
            {
                minGroup++;
            }

            return minGroup;
        }

        public Material BindDefaultMaterial()
        {
            if (m_Material == null)
            {
                m_Material = new Material(Shader.Find(m_MaterialName));
                m_Material.hideFlags = HideFlags.HideAndDontSave;
                m_Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m_Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m_Material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                m_Material.SetInt("_ZWrite", 0);
            }

            m_Material.SetPass(0);

            for (int i = 0; i < m_TextureSlots.Length; ++i)
                m_Material.SetTexture("_Input" + i, m_TextureSlots[i]);

            float selectMin = 0;
            float selectMax = 0;

            for (int i = 0; i < m_Groups.Count; ++i)
            {
                if (m_Groups[i].AddressBegin <= m_HighlightedAddrMin && m_HighlightedAddrMin <= m_Groups[i].AddressEnd)
                {
                    selectMin = m_Groups[i].RowsOffset + (float)(m_HighlightedAddrMin - m_Groups[i].AddressBegin) / (float)m_BytesInRow;
                }

                if (m_Groups[i].AddressBegin <= m_HighlightedAddrMax && m_HighlightedAddrMax <= m_Groups[i].AddressEnd)
                {
                    selectMax = m_Groups[i].RowsOffset + (float)(m_HighlightedAddrMax - m_Groups[i].AddressBegin) / (float)m_BytesInRow;
                }
            }

            m_Material.SetFloat("SelectBegin", selectMin);
            m_Material.SetFloat("SelectEnd", selectMax);

            return m_Material;
        }

        public abstract void OnRenderMap(ulong addressMin, ulong addressMax, int slot);


        public void FlushTextures(float yMin, float yMax)
        {
            bool flushTexture = false;

            long rowsCount = (long)(m_Groups[m_Groups.Count - 1].RowsOffset + m_Groups[m_Groups.Count - 1].RowsCount);

            for (int slot = 0; slot < m_TextureSlots.Length; ++slot)
                flushTexture |= (m_TextureSlots[slot] == null || m_TextureSlots[slot].width != (int)MemoryMapRect.width);

            long visibleRows = (long)((yMax - yMin) / Styles.MemoryMap.RowPixelHeight);

            int minGroup = FindFirstGroup(yMin, 0);
            int minGroupRowOffset = (int)((yMin - m_Groups[minGroup].RowsOffsetY) / Styles.MemoryMap.RowPixelHeight);

            long minRow = (long)m_Groups[minGroup].RowsOffset + minGroupRowOffset;
            long maxRow = Math.Min(minRow + visibleRows, rowsCount);

            // If rows are already in cache do nothing ...
            if (!flushTexture && m_MinCachedRow <= minRow && maxRow <= m_MaxCachedRow && !ForceRepaint)
                return;

            for (int slot = 0; slot < m_TextureSlots.LongLength; ++slot)
            {
                m_Texture = m_TextureSlots[slot];

                // ... otherwise rebuild cache.
                long cacheSize = Math.Max(0, (long)k_RowCacheSize - visibleRows) / 2;
                m_MinCachedRow = Math.Max(0, minRow - cacheSize / 2);
                m_MaxCachedRow = Math.Min(m_MinCachedRow + k_RowCacheSize, rowsCount);

                m_MinCachedGroup = minGroup;
                while (m_MinCachedGroup > 0 && m_MinCachedRow < m_Groups[m_MinCachedGroup].RowsStart)
                    m_MinCachedGroup--;

                m_MaxCachedGroup = minGroup;
                while (m_MaxCachedGroup + 1 < m_Groups.Count && m_Groups[m_MaxCachedGroup + 1].RowsStart < m_MaxCachedRow)
                    m_MaxCachedGroup++;

                ulong addressMin = m_Groups[m_MinCachedGroup].AddressBegin + ((ulong)m_MinCachedRow - m_Groups[m_MinCachedGroup].RowsOffset) * m_BytesInRow;
                ulong addressMax = m_Groups[m_MaxCachedGroup].AddressBegin + ((ulong)m_MaxCachedRow - m_Groups[m_MaxCachedGroup].RowsOffset) * m_BytesInRow;

                if (flushTexture)
                {
                    m_Texture = m_TextureSlots[slot] = new Texture2D((int)MemoryMapRect.width, k_RowCacheSize, TextureFormat.RGBA32, false);
                    m_Texture.wrapMode = TextureWrapMode.Clamp;
                    m_Texture.filterMode = FilterMode.Point;
                }

                m_RawTexture = m_Texture.GetRawTextureData<Color32>();

                m_RawTexture.MemClear();

                OnRenderMap(addressMin, addressMax, slot);

                // Sacrificing first pixel of row for information about group and it position in it.
                for (int i = 0; i < m_Groups.Count; ++i)
                {
                    if (m_MinCachedRow <= m_Groups[i].RowsEnd && m_Groups[i].RowsStart <= m_MaxCachedRow)
                    {
                        long begin = Math.Max(m_MinCachedRow, m_Groups[i].RowsStart);
                        long end   = Math.Min(m_MaxCachedRow, m_Groups[i].RowsEnd);

                        for (long j = begin; j < end; j++)
                        {
                            Color c = new Color(j, j == begin ? 1.0f : 0 , j + 1 == end ? 1.0f : 0 , 1.0f);

                            m_RawTexture[(int)((j % m_Texture.height) * m_Texture.width)] = c;
                        }
                    }
                }

                m_Texture.Apply(false);
            }

            ForceRepaint = false;
        }

        public void RenderStrip(MemoryGroup group, ulong addrBegin, ulong addrEnd, Color32 color)
        {
            ulong diffBegin = (addrBegin - group.AddressBegin);

            ulong rowBegin = diffBegin / m_BytesInRow;

            int   rowDelta = (int)((MemoryMapRect.width * (addrEnd - addrBegin)) / m_BytesInRow);
            float  x0 = MemoryMapRect.width * (((float)(diffBegin % m_BytesInRow)) / ((float)m_BytesInRow));

            int texelX0 = (int)x0;
            int texelX1 = texelX0 + Math.Max(1, rowDelta);
            int texelRow = (int)MemoryMapRect.width;

            int texelBegin = (int)(group.RowsOffset + rowBegin) * m_Texture.width + texelX0;
            int texelEnd = (int)(group.RowsOffset + rowBegin) * m_Texture.width + texelX1;

            for (int x = texelBegin; x < texelEnd; x++)
            {
                m_RawTexture[x % m_RawTexture.Length] = color;
            }
        }

        public void RenderStrip(MemoryGroup group, ulong addrBegin, ulong addrEnd, Func<Color32, Color32> func)
        {
            ulong diffBegin = (addrBegin - group.AddressBegin);

            ulong rowBegin = diffBegin / m_BytesInRow;

            int rowDelta = (int)((MemoryMapRect.width * (addrEnd - addrBegin)) / m_BytesInRow);
            float  x0 = MemoryMapRect.width * (((float)(diffBegin % m_BytesInRow)) / ((float)m_BytesInRow));

            int texelX0 = (int)x0;
            int texelX1 = texelX0 + Math.Max(1, rowDelta);
            int texelRow = (int)MemoryMapRect.width;

            int texelBegin = (int)(group.RowsOffset + rowBegin) * m_Texture.width + texelX0;
            int texelEnd = (int)(group.RowsOffset + rowBegin) * m_Texture.width + texelX1;

            for (int x = texelBegin; x < texelEnd; x++)
            {
                m_RawTexture[x % m_RawTexture.Length] = func(m_RawTexture[x % m_RawTexture.Length]);
            }
        }

        public void Render<T>(T data, List<EntryRange> ranges, int i, ulong addressMin, ulong addressMax, Color32 color) where T : CachedSnapshot.ISortedEntriesCache
        {
            for (int j = ranges[i].Begin; j < ranges[i].End; ++j)
            {
                ulong addr = (ulong)data.Address(j);
                ulong size = (ulong)data.Size(j);

                ulong stripAddrBegin = addr.Clamp(addressMin, addressMax);
                ulong stripAddrEnd   = (addr + size).Clamp(addressMin, addressMax);

                if (stripAddrBegin != stripAddrEnd)
                {
                    RenderStrip(m_Groups[i], stripAddrBegin, stripAddrEnd, color);
                }
            }
        }

        public void Render<T>(T data, List<EntryRange> ranges, int i, ulong addressMin, ulong addressMax, Func<Color32, Color32> func) where T : CachedSnapshot.ISortedEntriesCache
        {
            for (int j = ranges[i].Begin; j < ranges[i].End; ++j)
            {
                ulong addr = (ulong)data.Address(j);
                ulong size = (ulong)data.Size(j);

                ulong stripAddrBegin = addr.Clamp(addressMin,  addressMax);
                ulong stripAddrEnd   = (addr + size).Clamp(addressMin,  addressMax);

                if (stripAddrBegin != stripAddrEnd)
                {
                    RenderStrip(m_Groups[i], stripAddrBegin, stripAddrEnd, func);
                }
            }
        }

        public static Color32 Add(Color32 c1, Color32 c2)
        {
            return new Color32((byte)(c1.r  + c2.r), (byte)(c1.g  + c2.g), (byte)(c1.b  + c2.b), (byte)(c1.a + c2.a));
        }

        public static Color32 Max(Color32 c1, Color32 c2)
        {
            return new Color32(Math.Max(c1.r, c2.r), Math.Max(c1.g, c2.g), Math.Max(c1.b, c2.b), Math.Max(c1.a, c2.a));
        }

        public void RenderDiff<T>(T data0, List<EntryRange> ranges0, T data1, List<EntryRange> ranges1, int i, ulong addressMin, ulong addressMax) where T : CachedSnapshot.ISortedEntriesCache
        {
            MemoryGroup group = m_Groups[i];

            int index0 = ranges0[i].Begin;
            int lastIndex0 = ranges0[i].End;

            while (index0 < lastIndex0 && data0.Address(index0) + data0.Size(index0) <= addressMin) index0++;

            int index1 = ranges1[i].Begin;
            int lastIndex1 = ranges1[i].End;

            while (index1 < lastIndex1 && data1.Address(index1) + data1.Size(index1) <= addressMin) index1++;

            while (index0 < lastIndex0 || index1 < lastIndex1)
            {
                ulong addr0 = index0 < lastIndex0 ? data0.Address(index0) : addressMax;
                ulong addr1 = index1 < lastIndex1 ? data1.Address(index1) : addressMax;

                if (addr0 >= addressMax && addr1 >= addressMax)
                {
                    // There is no need to process entries anymore because I assuming they are sorted.
                    return;
                }

                ulong size0 = index0 < lastIndex0 ? data0.Size(index0) : 0;
                ulong size1 = index1 < lastIndex1 ? data1.Size(index1) : 0;

                if (addr0 == addr1 && size0 == size1)
                {
                    ulong stripAddrBegin = addr0.Clamp(addressMin, addressMax);
                    ulong stripAddrEnd   = (addr0 + size0).Clamp(addressMin, addressMax);

                    if (stripAddrBegin != stripAddrEnd)
                    {
                        RenderStrip(group, stripAddrBegin, stripAddrEnd, (Color32 c) => Add(c, new Color32(0x00, 0x00, 0x0F, 0x00)));
                    }

                    index0++;
                    index1++;
                }
                else
                {
                    if (addr0 < addr1 || (addr0 == addr1 && size0 != size1))
                    {
                        ulong stripAddrBegin = addr0.Clamp(addressMin, addressMax);
                        ulong stripAddrEnd   = (addr0 + size0).Clamp(addressMin, addressMax);

                        if (stripAddrBegin != stripAddrEnd)
                        {
                            RenderStrip(group, stripAddrBegin, stripAddrEnd, (Color32 c) => Add(c, new Color32(0x0F, 0x00, 0x00, 0x00)));
                        }

                        index0++;
                    }

                    if (addr0 > addr1 || (addr0 == addr1 && size0 != size1))
                    {
                        ulong stripAddrBegin = addr1.Clamp(addressMin, addressMax);
                        ulong stripAddrEnd   = (addr1 + size1).Clamp(addressMin, addressMax);

                        if (stripAddrBegin != stripAddrEnd)
                        {
                            RenderStrip(group, stripAddrBegin, stripAddrEnd, (Color32 c) => Add(c, new Color32(0x00, 0x0F, 0x00, 0x00)));
                        }

                        index1++;
                    }
                }
            }
        }

        public void RenderGroups(float yMin, float yMax)
        {
            float u0 = 0.0f;
            float u1 = m_Texture.width;

            GL.Begin(GL.QUADS);
            for (int i = 0; i < m_Groups.Count; ++i)
            {
                if (yMin < m_Groups[i].RowsOffsetY + m_Groups[i].RowsCount * Styles.MemoryMap.RowPixelHeight && m_Groups[i].RowsOffsetY < yMax)
                {
                    float v0 = m_Groups[i].RowsStart;
                    float v1 = m_Groups[i].RowsEnd;
                    float s =  m_Groups[i].RowsCount;

                    GL.TexCoord3(u1, v0, 0); GL.Vertex3(MemoryMapRect.xMax, m_Groups[i].MinY, 0f);
                    GL.TexCoord3(u0, v0, 0); GL.Vertex3(Styles.MemoryMap.HeaderWidth, m_Groups[i].MinY,  0f);
                    GL.TexCoord3(u0, v1, s); GL.Vertex3(Styles.MemoryMap.HeaderWidth, m_Groups[i].MaxY, 0f);
                    GL.TexCoord3(u1, v1, s); GL.Vertex3(MemoryMapRect.xMax,       m_Groups[i].MaxY, 0f);
                }
            }
            GL.End();
        }

        public void RenderGroupLabels(float yMin, float yMax)
        {
            for (int i = 0; i < m_Groups.Count; ++i)
            {
                if (m_Groups[i].RowsOffsetY > yMax)
                {
                    return; // Groups are sorted so there is no need to process anymore
                }

                if (yMin < m_Groups[i].RowsOffsetY + m_Groups[i].RowsCount * Styles.MemoryMap.RowPixelHeight)
                {
                    GUI.Label(m_Groups[i].Labels[0].TextRect, m_Groups[i].Labels[0].Text, Styles.MemoryMap.TimelineBar);

                    for (int j = 1; j < m_Groups[i].Labels.Length && m_Groups[i].Labels[j].TextRect.yMax < yMax; ++j)
                    {
                        GUI.Label(m_Groups[i].Labels[j].TextRect, m_Groups[i].Labels[j].Text, Styles.MemoryMap.AddressSub);
                    }
                }
            }
        }

        public ulong MouseToAddress(Vector2 vCursor)
        {
            int i = m_MinCachedGroup;

            while (i <= m_MaxCachedGroup && vCursor.y > m_Groups[i].MaxY)
            {
                i++;
            }

            if (i <= m_MaxCachedGroup)
            {
                if (vCursor.y >= m_Groups[i].MinY)
                {
                    ulong Addr = m_Groups[i].AddressBegin;
                    Addr += (ulong)(Math.Floor((vCursor.y - m_Groups[i].MinY) / Styles.MemoryMap.RowPixelHeight) * m_BytesInRow);

                    if (vCursor.x >= MemoryMapRect.x)
                        Addr += (ulong)(((vCursor.x - MemoryMapRect.x) * m_BytesInRow) / MemoryMapRect.width);

                    return Addr;
                }

                return m_Groups[i].AddressBegin;
            }

            return 0;
        }
    }
}
