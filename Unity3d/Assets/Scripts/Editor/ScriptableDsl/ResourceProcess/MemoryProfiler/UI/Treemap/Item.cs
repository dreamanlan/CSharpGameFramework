using System;
using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI.Treemap
{
    internal enum ObjectMetricType : byte
    {
        None = 0,
        Managed,
        Native
    }

    internal struct ObjectMetric : IEquatable<ObjectMetric>
    {
        const string k_UnknownNativeType = "<invalid native type>";
        const string k_UnknownManagedType = "<invalid managed type>";

        public ObjectMetricType MetricType { private set; get; }
        public int ObjectIndex { private set; get; }
        CachedSnapshot m_CachedSnapshot;

        public ObjectMetric(int objectIndex, CachedSnapshot cachedSnapshot, ObjectMetricType metricType)
        {
            MetricType = metricType;
            ObjectIndex = objectIndex;
            m_CachedSnapshot = cachedSnapshot;
        }

        public string GetTypeName()
        {
            switch (MetricType)
            {
                case ObjectMetricType.Managed:
                    var ITypeDesc = m_CachedSnapshot.CrawledData.ManagedObjects[ObjectIndex].ITypeDescription;
                    if (ITypeDesc >= 0)
                    {
                        return m_CachedSnapshot.typeDescriptions.typeDescriptionName[ITypeDesc];
                    }
                    return k_UnknownManagedType;
                case ObjectMetricType.Native:
                    var INatTypeDesc = m_CachedSnapshot.nativeObjects.nativeTypeArrayIndex[ObjectIndex];
                    if (INatTypeDesc > 0)
                    {
                        return m_CachedSnapshot.nativeTypes.typeName[INatTypeDesc];
                    }
                    return k_UnknownNativeType;
                default:
                    return null;
            }
        }

        public string GetName()
        {
            switch (MetricType)
            {
                case ObjectMetricType.Managed:
                    var managedObj = m_CachedSnapshot.CrawledData.ManagedObjects[ObjectIndex];
                    if (managedObj.NativeObjectIndex >= 0)
                    {
                        string objName = m_CachedSnapshot.nativeObjects.objectName[managedObj.NativeObjectIndex];
                        if (objName.Length > 0)
                        {
                            return " \"" + objName + "\" <" + GetTypeName() + ">";
                        }
                    }
                    return string.Format("[0x{0:x16}]", managedObj.PtrObject) + " < " + GetTypeName() + " > ";
                case ObjectMetricType.Native:
                    string objectName = m_CachedSnapshot.nativeObjects.objectName[ObjectIndex];
                    if (objectName.Length > 0)
                    {
                        return " \"" + objectName + "\" <" + GetTypeName() + ">";
                    }
                    return GetTypeName();
                default:
                    return null;
            }
        }

        public int GetObjectUID()
        {
            switch (MetricType)
            {
                case ObjectMetricType.Managed:
                    return m_CachedSnapshot.ManagedObjectIndexToUnifiedObjectIndex(ObjectIndex);
                case ObjectMetricType.Native:
                    return m_CachedSnapshot.NativeObjectIndexToUnifiedObjectIndex(ObjectIndex);
                default:
                    return -1;
            }
        }

        public long GetValue()
        {
            switch (MetricType)
            {
                case ObjectMetricType.Managed:
                    return m_CachedSnapshot.CrawledData.ManagedObjects[ObjectIndex].Size;
                case ObjectMetricType.Native:
                    return (long)m_CachedSnapshot.nativeObjects.size[ObjectIndex];
                default:
                    return -1;
            }
        }

        public bool Equals(ObjectMetric other)
        {
            if (MetricType == other.MetricType)
            {
                return ObjectIndex == other.ObjectIndex;
            }

            return false;
        }
    }

    internal class Item : IComparable<Item>
    {
        public Group Group { private set; get; }
        public Rect Position;
        //public int _index;

        public ObjectMetric Metric { private set; get; }

        public string Label { private set; get; }
        public long Value { get { return Metric.GetValue(); } }
        public Color Color { get { return Group.Color; } }

        public Item(ObjectMetric metric, Group group)
        {
            Metric = metric;
            Group = group;
            Label = Metric.GetName() + "\n" + EditorUtility.FormatBytes(Value);
        }

        public int CompareTo(Item other)
        {
            return (int)(Group != other.Group ? other.Group.TotalValue - Group.TotalValue : other.Value - Value);
        }
    }
}
