using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI.Treemap
{
    internal class Group : IComparable<Group>, ITreemapRenderable
    {
        public string _name;
        public Rect _position;
        public List<Item> _items;
        private long _totalValue = -1;

        public long totalValue
        {
            get
            {
                if (_totalValue != -1)
                    return _totalValue;

                long result = 0;
                foreach (Item item in _items)
                {
                    result += item.value;
                }
                _totalValue = result;
                return result;
            }
        }

        public float[] memorySizes
        {
            get
            {
                float[] result = new float[_items.Count];
                for (int i = 0; i < _items.Count; i++)
                {
                    result[i] = _items[i].value;
                }
                return result;
            }
        }

        public Color color
        {
            get { return Utility.GetColorForName(_name); }
        }

        public int CompareTo(Group other)
        {
            return (int)(other.totalValue - totalValue);
        }

        public Color GetColor()
        {
            return color;
        }

        public Rect GetPosition()
        {
            return _position;
        }

        public string GetLabel()
        {
            string row1 = string.Format("{0} ({1})", _name, _items != null ? _items.Count : 0);
            string row2 = EditorUtility.FormatBytes((long)totalValue);
            return row1 + "\n" + row2;
        }
    }
}
