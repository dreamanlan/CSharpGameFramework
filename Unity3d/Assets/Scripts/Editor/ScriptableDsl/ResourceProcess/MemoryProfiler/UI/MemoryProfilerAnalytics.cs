using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Analytics;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal static class MemoryProfilerAnalytics
    {
        static bool s_EnableAnalytics = false;

        static Dictionary<Type, double> s_PendingEvents = new Dictionary<Type, double>();

        static Dictionary<Type, List<int>> s_MetaDataForPendingEvents = new Dictionary<Type, List<int>>();

        static List<Filter> s_PendingFilterChanges = new List<Filter>();
        static string s_TableNameOfPendingFilterChanges = "";

        const int k_MaxEventsPerHour = 100;
        const int k_MaxEventItems = 1000;
        const string k_VendorKey = "unity.memoryprofiler";

        public interface IMemoryProfilerAnalyticsEvent
        {
            void SetTime(int ts, float duration);
        }

        public interface IMemoryProfilerAnalyticsEventWithMetaData : IMemoryProfilerAnalyticsEvent
        {
            int[] Data { set; }
        }


        [Serializable]
        internal struct CapturedSnapshotEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "captureSnapshot";
            }

            // camelCase since these events get serialized to Json and naming convention in analytics is camelCase
            public string subtype;
            public int ts;
            public float duration;
            public bool success;
        }

        [Serializable]
        internal struct LoadedSnapshotEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "loadSnapshot";
            }

            public string subtype;
            public int ts;
            public float duration;
        }

        [Serializable]
        internal struct ImportedSnapshotEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "importSnapshot";
            }

            public string subtype;
            public int ts;
            public float duration;
        }

        [Serializable]
        internal struct DiffedSnapshotEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "diffSnapshot";
            }

            public string subtype;
            public int ts;
            public float duration;
        }

        [Serializable]
        internal struct DiffToggledEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "diffToggle";
            }

            public string subtype;
            public int ts;
            public float duration;
            public enum ShowSnapshot
            {
                Both, // 0
                First, // 1
                Second, // 2
            }
            public int shown;
            public int show;
            public string viewName;
        }

        [Serializable]
        public struct OpenedViewEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "openView";
            }

            public string subtype;
            public int ts;
            public float duration;
            public string viewName;
        }

        [Serializable]
        public struct LoadViewXMLEvent : IMemoryProfilerAnalyticsEventWithMetaData
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "loadViewXML";
            }

            public string subtype;
            public int ts;
            public float duration;

            public int[] Data { set { data = value; } }

            public int[] data;

            public bool success;
            public string fileName;
        }

        [Serializable]
        public struct Filter
        {
            public string filterName;
            public string column;
            public string filterInput;
            public int key;
        }

        [Serializable]
        public struct TableFilteredEvent : IMemoryProfilerAnalyticsEvent
        {
            public void SetTime(int ts, float duration)
            {
                this.ts = ts;
                this.duration = duration;
                subtype = "filterTable";
            }

            public string subtype;
            public int ts;
            public float duration;
            public string viewName;
            public Filter filter;
        }

        const string k_EventTopicName = "memoryProfiler";

        public static void EnableAnalytics()
        {
            s_EnableAnalytics = true;
            EditorAnalytics.RegisterEventWithLimit(k_EventTopicName, k_MaxEventsPerHour, k_MaxEventItems, k_VendorKey);
        }

        public static void SendEvent<T>(T eventData) where T : IMemoryProfilerAnalyticsEvent
        {
            if (s_EnableAnalytics)
                EditorAnalytics.SendEventWithLimit(k_EventTopicName, eventData);
        }

        public static void StartEvent<T>() where T : IMemoryProfilerAnalyticsEvent
        {
            if (s_EnableAnalytics)
                s_PendingEvents[typeof(T)] = EditorApplication.timeSinceStartup;
        }

        public static void CancelEvent<T>() where T : IMemoryProfilerAnalyticsEvent
        {
            if (s_EnableAnalytics && s_PendingEvents.ContainsKey(typeof(T)))
            {
                s_PendingEvents[typeof(T)] = -1;
            }
        }

        public static void EndEvent<T>(T eventData) where T : IMemoryProfilerAnalyticsEvent
        {
            if (s_EnableAnalytics)
            {
                if (s_PendingEvents.ContainsKey(typeof(T)) && s_PendingEvents[typeof(T)] >= 0)
                {
                    int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    eventData.SetTime(unixTimestamp, (float)(EditorApplication.timeSinceStartup - s_PendingEvents[typeof(T)]));
                    s_PendingEvents[typeof(T)] = -1;
                    SendEvent(eventData);
                }
                else
                {
                    Debug.LogError("Ending analytics event before it even started: " + eventData);
                }
            }
        }

        public static void StartEventWithMetaData<T>() where T : IMemoryProfilerAnalyticsEventWithMetaData
        {
            if (s_EnableAnalytics)
            {
                StartEvent<T>();
                if (s_MetaDataForPendingEvents.ContainsKey(typeof(T)))
                {
                    s_MetaDataForPendingEvents[typeof(T)].Clear();
                }
                else
                {
                    s_MetaDataForPendingEvents[typeof(T)] = new List<int>();
                }
            }
        }

        public static void EndEventWithMetadata<T>(T eventData) where T : IMemoryProfilerAnalyticsEventWithMetaData
        {
            if (s_EnableAnalytics)
            {
                if (s_EnableAnalytics && s_PendingEvents.ContainsKey(typeof(T)) && s_PendingEvents[typeof(T)] >= 0)
                {
                    eventData.Data = s_MetaDataForPendingEvents[typeof(T)].ToArray();
                    s_MetaDataForPendingEvents[typeof(T)].Clear();
                }
                EndEvent<T>(eventData);
            }
        }

        public static void AddMetaDatatoEvent<T>(byte data) where T : IMemoryProfilerAnalyticsEventWithMetaData
        {
            if (s_EnableAnalytics && s_PendingEvents.ContainsKey(typeof(T)) && s_PendingEvents[typeof(T)] >= 0)
                s_MetaDataForPendingEvents[typeof(T)].Add(data);
        }

        public static void FiltersChanged(string tableName, List<Filter> filters)
        {
            if (s_EnableAnalytics)
            {
                bool changesOccured = false;
                if (s_PendingFilterChanges.Count == filters.Count)
                {
                    for (int i = 0; i < filters.Count; i++)
                    {
                        changesOccured = s_PendingFilterChanges[i].column != filters[i].column || s_PendingFilterChanges[i].filterName != filters[i].filterName || s_PendingFilterChanges[i].filterInput != filters[i].filterInput;
                        if (changesOccured)
                            break;
                    }
                }
                else
                {
                    changesOccured = true;
                }
                if (!changesOccured)
                    return;

                if (!s_PendingEvents.ContainsKey(typeof(TableFilteredEvent)) || s_PendingEvents[typeof(TableFilteredEvent)] < 0)
                {
                    StartEvent<TableFilteredEvent>();
                }
                s_TableNameOfPendingFilterChanges = tableName;
                s_PendingFilterChanges.Clear();

                foreach (var item in filters)
                {
                    s_PendingFilterChanges.Add(item);
                }
            }
        }

        public static void SendPendingFilterChanges()
        {
            //TODO: Send off 20seconds after the last change
            if (s_PendingFilterChanges.Count > 0)
            {
                if (s_PendingEvents.ContainsKey(typeof(TableFilteredEvent)) && s_PendingEvents[typeof(TableFilteredEvent)] >= 0)
                {
                    int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    foreach (var filter in s_PendingFilterChanges)
                    {
                        var filterToSend = filter;
                        filterToSend.key = unixTimestamp;
                        var eventData = new TableFilteredEvent() { viewName = s_TableNameOfPendingFilterChanges, filter = filterToSend };
                        eventData.SetTime(unixTimestamp, (float)(EditorApplication.timeSinceStartup - s_PendingEvents[typeof(TableFilteredEvent)]));
                        SendEvent(eventData);
                    }
                    s_PendingEvents[typeof(TableFilteredEvent)] = -1;
                }
                //TODO substract the time waited until sending from the time spend filtering
                s_PendingFilterChanges.Clear();
                s_TableNameOfPendingFilterChanges = "";
            }
        }
    }
}
