using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI.Treemap
{
    internal class Group : IComparable<Group>
    {
        public string Name;
        public Rect Position;
        public List<Item> Items;
        private long m_TotalValue = -1;

        string m_Label = null;
        public string Label
        {
            get
            {
                if (m_Label == null)
                    m_Label = string.Format("{0} ({1})", Name, Items != null ? Items.Count : 0) + "\n" + EditorUtility.FormatBytes((long)TotalValue);

                return m_Label;
            }
        }

        public long TotalValue
        {
            get
            {
                if (m_TotalValue != -1)
                    return m_TotalValue;

                long result = 0;
                foreach (Item item in Items)
                {
                    result += item.Value;
                }
                m_TotalValue = result;
                return result;
            }
        }

        public float[] MemorySizes
        {
            get
            {
                float[] result = new float[Items.Count];
                for (int i = 0; i < Items.Count; i++)
                {
                    result[i] = Items[i].Value;
                }
                return result;
            }
        }

        public Color Color
        {
            get { return Utility.GetColorForName(Name); }
        }

        public int CompareTo(Group other)
        {
            return (int)(other.TotalValue - TotalValue);
        }
    }
}
