using UnityEngine;
using UnityEditor;
using System;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal static class Styles
    {
        public static GeneralStyles General { get; private set; }
        public static MemoryMapStyles MemoryMap { get; private set; }

        public static void Initialize()
        {
            if(General == null)
            {
                General = new GeneralStyles();
                General.Init();
            }

            if(MemoryMap == null)
            {
                MemoryMap = new MemoryMapStyles();
                MemoryMap.Init();
            }
        }

        public static void Cleanup()
        {
            General = null;
            MemoryMap = null;
        }
    }


    public abstract class StyleSet
    {
        protected bool m_Inited = false;

        public abstract void Init();
    }

    public class GeneralStyles : StyleSet
    {
        public readonly Color SelectedColor = new Color(62f / 255f, 95f / 255f, 150f / 255f);
        public readonly int FoldoutWidth = 16;
        public readonly int ViewPaneMargin = 2;
        public readonly int InitialWorkbenchWidth = 200;

        public GUIStyle Background { get; private set; }
        public GUIStyle Tooltip { get; private set; }
        public GUIStyle TooltipArrow { get; private set; }
        public GUIStyle Bar { get; private set; }
        public GUIStyle Header { get; private set; }
        public GUIStyle LeftPane { get; private set; }
        public GUIStyle RightPane { get; private set; }
        public GUIStyle EntryEven { get; private set; }
        public GUIStyle EntryOdd { get; private set; }
        public GUIStyle EntrySelected { get; private set; }
        public GUIStyle NumberLabel { get; private set; }
        public GUIStyle ClickableLabel { get; private set; }
        public GUIStyle Foldout { get; private set; }
        public GUIStyle ProfilerGraphBackground { get; private set; }
        public GUIStyle ToolbarPopup { get; private set; }

        public override void Init()
        {
            if (m_Inited)
                return;

            m_Inited = true;
            Background = "OL Box";
            Tooltip = "AnimationEventTooltip";
            TooltipArrow = "AnimationEventTooltipArrow";
            Bar = "ProfilerTimelineBar";
            Header = "OL title";
            LeftPane = "ProfilerTimelineLeftPane";
            RightPane = "ProfilerRightPane";
            EntryEven = "OL EntryBackEven";
            EntryOdd = "OL EntryBackOdd";
            EntrySelected = "TV Selection";
            NumberLabel = "OL Label";
            Foldout = "ProfilerTimelineFoldout";
            ProfilerGraphBackground = new GUIStyle("ProfilerScrollviewBackground");
            ToolbarPopup = new GUIStyle(EditorStyles.toolbarPopup);

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
    
    public class MemoryMapStyles : StyleSet
    {

        public GUIStyle TimelineBar { get; private set; }
        public GUIStyle AddressSub { get; private set; }
        public GUIStyle SeriesLabel { get; private set; }
        public GUIStyle ContentToolbar { get; private set; }

        public readonly ulong SubAddressStepInRows = 10;
        public readonly int LegendHeight = (int)EditorGUIUtility.singleLineHeight;
        public readonly int LegendSpacerWidth = 10;
        public readonly int VScrollBarWidth = 15;
        public readonly float RowPixelHeight = 24.0f;
        public readonly float HeaderHeight = 24.0f;
        public readonly float HeaderWidth = 128.0f;
        public readonly Color LineColor = new Color(0.33f, 0.33f, 0.33f);
        public readonly Color SplitLineColor = new Color(0.0f, 0.0f, 0.0f);
        public readonly Color MemBackground = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        public override void Init()
        {
            if (m_Inited)
                return;

            m_Inited = true;

            TimelineBar = new GUIStyle("AnimationEventTooltip");
            AddressSub = new GUIStyle("OL Label");
            // Hard coded font sizes for now because the text will otherwise spill over
            TimelineBar.fontSize = 11;
            AddressSub.fontSize = 11;
            SeriesLabel = "ProfilerPaneSubLabel";
            ContentToolbar =
#if UNITY_2019_3_OR_NEWER
                "ContentToolbar";
#else
                EditorStyles.toolbar;
#endif
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
