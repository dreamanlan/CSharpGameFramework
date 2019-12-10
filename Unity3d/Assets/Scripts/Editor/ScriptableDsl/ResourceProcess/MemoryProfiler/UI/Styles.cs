using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal static class Styles
    {
        public static readonly GUIStyle Background = "OL Box";
        public static readonly GUIStyle Tooltip = "AnimationEventTooltip";
        public static readonly GUIStyle TooltipArrow = "AnimationEventTooltipArrow";
        public static readonly GUIStyle Bar = "ProfilerTimelineBar";
        public static readonly GUIStyle Header = "OL title";
        public static readonly GUIStyle LeftPane = "ProfilerTimelineLeftPane";
        public static readonly GUIStyle RightPane = "ProfilerRightPane";
        public static readonly GUIStyle EntryEven = "OL EntryBackEven";
        public static readonly GUIStyle EntryOdd = "OL EntryBackOdd";
        public static readonly GUIStyle EntrySelected = "TV Selection";
        public static readonly GUIStyle NumberLabel = "OL Label";
        public static readonly GUIStyle ClickableLabel;// = "OL Label";
        public static readonly GUIStyle Border = new GUIStyle();
        public static readonly GUIStyle Foldout = "ProfilerTimelineFoldout";
        public static readonly GUIStyle ProfilerGraphBackground = new GUIStyle("ProfilerScrollviewBackground");
        public static readonly Color SelectedColor = new Color(62f / 255f, 95f / 255f, 150f / 255f);
        public const int FoldoutWidth = 16;

        static Styles()
        {
            Bar.normal.background = Bar.hover.background = Bar.active.background = EditorGUIUtility.whiteTexture;
            Bar.normal.textColor = Bar.hover.textColor = Bar.active.textColor = Color.black;
            LeftPane.padding.left = 15;
            ClickableLabel = new GUIStyle(NumberLabel);
            if (EditorGUIUtility.isProSkin)
                ClickableLabel.normal.textColor = new Color(33 / 255.0f, 150 / 255.0f, 243 / 255.0f, 1.0f);
            else
                ClickableLabel.normal.textColor = new Color(0, 0, 1, 1);
        }
    }
    internal class EllipsisStyleMetric
    {
        public GUIStyle style;
        public string ellipsisString = "...";
        public Vector2 pixelSize = Vector2.zero;
        public EllipsisStyleMetric(GUIStyle aStyle)
        {
            style = aStyle;
            pixelSize = aStyle.CalcSize(new GUIContent(ellipsisString));
        }
    }
}
