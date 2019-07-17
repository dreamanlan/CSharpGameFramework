using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryProfilerWindowForExtension;

namespace UnityEditor.MemoryProfiler2
{
    public class ProfilerNodeObjectInfo
    {
        public string text;
        public int id;
        public Rect myAreaRect;
        public ThingInMemory memObject;
    }
}
