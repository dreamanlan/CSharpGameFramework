using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using UnityEditor.UIElements;
#else
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#endif

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal static class UIElementsStyleCompatibilityExtensions
    {
        internal static float GetMarginBottomFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.marginBottom;
#else
            return visualElement.style.marginBottom;
#endif
        }

        internal static float GetMarginTopFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.marginTop;
#else
            return visualElement.style.marginTop;
#endif
        }

        internal static float GetMarginRightFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.marginRight;
#else
            return visualElement.style.marginRight;
#endif
        }

        internal static float GetMarginLeftFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.marginLeft;
#else
            return visualElement.style.marginLeft;
#endif
        }

        internal static float GetPaddingBottomFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.paddingBottom;
#else
            return visualElement.style.paddingBottom;
#endif
        }

        internal static float GetPaddingTopFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.paddingTop;
#else
            return visualElement.style.paddingTop;
#endif
        }

        internal static float GetPaddingRightFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.paddingRight;
#else
            return visualElement.style.paddingRight;
#endif
        }

        internal static float GetPaddingLeftFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.paddingLeft;
#else
            return visualElement.style.paddingLeft;
#endif
        }

        internal static float GetWidthFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.width;
#else
            return visualElement.style.width;
#endif
        }

        internal static float GetMinWidthFromStyle(this VisualElement visualElement)
        {
#if UNITY_2019_1_OR_NEWER
            return visualElement.resolvedStyle.minWidth.value;
#else
            return visualElement.style.minWidth;
#endif
        }
    }
}
