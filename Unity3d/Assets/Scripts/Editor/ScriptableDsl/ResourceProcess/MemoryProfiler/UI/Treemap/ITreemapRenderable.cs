using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI.Treemap
{
    interface ITreemapRenderable
    {
        Color GetColor();
        Rect GetPosition();
        string GetLabel();
    }
}
