using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using UnityEditor.UIElements;
#else
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#endif

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal static class UIElementsHelper
    {
        public static void SwitchVisibility(VisualElement first, VisualElement second, bool showFirst = true)
        {
            SetVisibility(first, showFirst);
            SetVisibility(second, !showFirst);
        }

        public static void SetVisibility(VisualElement element, bool visible)
        {
            element.visible = visible;
#if UNITY_2019_1_OR_NEWER
            element.style.position = visible ? Position.Relative : Position.Absolute;
#else
            element.style.positionType = visible ? PositionType.Relative : PositionType.Absolute;
#endif
        }

#if !UNITY_2019_1_OR_NEWER
        public static void RegisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
        {
            control.OnValueChanged(callback);
        }

#endif
    }
}
