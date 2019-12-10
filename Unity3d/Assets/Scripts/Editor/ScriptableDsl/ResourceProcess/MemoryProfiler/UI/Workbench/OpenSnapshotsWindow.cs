using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using UnityEditor.UIElements;
#else
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#endif
using System;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class OpenSnapshotsWindow : VisualElement
    {
        public event Action SwapOpenSnapshots = delegate {};
        public event Action ShowDiffOfOpenSnapshots = delegate {};
        public event Action ShowFirstOpenSnapshot = delegate {};
        public event Action ShowSecondOpenSnapshot = delegate {};

        class OpenSnapshotItemUI
        {
            public VisualElement Item;
            public Image Image;
            public Image PlatformIcon;
            public Image EditorPlatformIcon;
            public Label NoData;
            public Label Name;
            public Label Date;
            public Label Age;
            public DateTime UtcDateTime;
        }
        OpenSnapshotItemUI m_OpenSnapshotItemUIFirst = new OpenSnapshotItemUI();
        OpenSnapshotItemUI m_OpenSnapshotItemUISecond = new OpenSnapshotItemUI();
        Button m_DiffButton;

        VisualElement m_FirstSnapshotHolder;
        VisualElement m_SecondSnapshotHolder;
        VisualElement m_DiffButtonHolder;

        const string k_UxmlPath = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/UXML/OpenSnapshotsWindow.uxml";
        const string k_CommonStyleSheetPath = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/StyleSheets/OpenSnapshotsWindow_style.uss";
        const string k_OpenSnapshotItemUxmlPath = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/UXML/OpenSnapshotItem.uxml";
        const string k_SelectedSnapshotClassName = "selectedSnapshot";

        static readonly bool k_SplitWidthEvenly = true;

        bool firstIsOpen;
        bool secondIsOpen;

        public OpenSnapshotsWindow(float initialWidth)
        {
            var windowTree = AssetDatabase.LoadAssetAtPath(k_UxmlPath, typeof(VisualTreeAsset)) as VisualTreeAsset;
#if UNITY_2019_1_OR_NEWER
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_CommonStyleSheetPath));
            windowTree.CloneTree(this);
#else
            AddStyleSheetPath(k_CommonStyleSheetPath);
            var windowSlots = new Dictionary<string, VisualElement>();
            windowTree.CloneTree(this, windowSlots);
#endif

            // setup the open snapshots panel
            this.Q<ToolbarButton>("swapOpenSnapshots").clickable.clicked += () => SwapOpenSnapshots();
            m_DiffButton = this.Q<Button>("diffOpenSnapshots");
            m_DiffButton.clickable.clicked += () => ShowDiffOfOpenSnapshots();
            m_DiffButton.SetEnabled(false);

            var openSnapshotItemTree = AssetDatabase.LoadAssetAtPath(k_OpenSnapshotItemUxmlPath, typeof(VisualTreeAsset)) as VisualTreeAsset;

            var openSnapshotItemSlots = new Dictionary<string, VisualElement>();
            m_FirstSnapshotHolder = this.Q("openSnapshotsWindowFirstSnapshotHolder");
            m_SecondSnapshotHolder = this.Q("openSnapshotsWindowSecondSnapshotHolder");
            m_DiffButtonHolder = this.Q("openSnapshotsWindowDiffSnapshotSpace");

            InitializeOpenSnapshotItem(openSnapshotItemTree, openSnapshotItemSlots, m_FirstSnapshotHolder, ref m_OpenSnapshotItemUIFirst, () => ShowFirstOpenSnapshot());
            InitializeOpenSnapshotItem(openSnapshotItemTree, openSnapshotItemSlots, m_SecondSnapshotHolder, ref m_OpenSnapshotItemUISecond, () => ShowSecondOpenSnapshot());

            UpdateWidth(initialWidth);
        }

        void InitializeOpenSnapshotItem(VisualTreeAsset openSnapshotItemTree, Dictionary<string, VisualElement> openSnapshotItemSlots, VisualElement snapshotHolder, ref OpenSnapshotItemUI openSnapshotItemUI, Action openSnapshotHandler)
        {
#if UNITY_2019_1_OR_NEWER
            var item = openSnapshotItemTree.CloneTree();
#else
            openSnapshotItemSlots.Clear();
            var item = openSnapshotItemTree.CloneTree(openSnapshotItemSlots);
#endif
            item.style.flexGrow = 1;
            snapshotHolder.Add(item);
            item.AddManipulator(new Clickable(openSnapshotHandler));
            openSnapshotItemUI.Item = item;
            openSnapshotItemUI.Image = item.Q<Image>("previewImage", "previewImage");
            openSnapshotItemUI.Image.scaleMode = ScaleMode.ScaleToFit;
            openSnapshotItemUI.PlatformIcon = item.Q<Image>("platformIcon", "platformIcon");
            openSnapshotItemUI.EditorPlatformIcon = item.Q<Image>("editorIcon", "platformIcon");
            openSnapshotItemUI.NoData = item.Q<Label>("noDataLoaded");
            openSnapshotItemUI.Name = item.Q<Label>("snapshotName");
            openSnapshotItemUI.Date = item.Q<Label>("snapshotDate");
            openSnapshotItemUI.Age = item.Q<Label>("snapshotAge");
            UIElementsHelper.SetVisibility(openSnapshotItemUI.PlatformIcon, false);
            UIElementsHelper.SetVisibility(openSnapshotItemUI.EditorPlatformIcon, false);
            UIElementsHelper.SetVisibility(openSnapshotItemUI.NoData, true);
            UIElementsHelper.SetVisibility(openSnapshotItemUI.Name, false);
        }

        public void SetSnapshotUIData(bool first, SnapshotFileGUIData snapshotGUIData, bool isInView)
        {
            OpenSnapshotItemUI itemUI = m_OpenSnapshotItemUIFirst;
            if (!first)
            {
                itemUI = m_OpenSnapshotItemUISecond;
            }
            if (snapshotGUIData == null)
            {
                UIElementsHelper.SwitchVisibility(itemUI.NoData, itemUI.Name);
                itemUI.Name.text = "";
                itemUI.Date.text = "";
                itemUI.Age.text = "";
                itemUI.Image.image = null;
                UIElementsHelper.SetVisibility(itemUI.PlatformIcon, false);
                UIElementsHelper.SetVisibility(itemUI.EditorPlatformIcon, false);
                itemUI.UtcDateTime = default(DateTime);
                // one of both snapshots is not open so there is no Age comparison between them.
                m_OpenSnapshotItemUIFirst.Age.text = "";
                m_OpenSnapshotItemUISecond.Age.text = "";
            }
            else
            {
                UIElementsHelper.SwitchVisibility(itemUI.Name, itemUI.NoData);
                itemUI.Name.text = snapshotGUIData.Name.text;
                itemUI.Date.text = snapshotGUIData.SnapshotDate.text;
                itemUI.Image.image = snapshotGUIData.MetaScreenshot;
                UIElementsHelper.SetVisibility(itemUI.PlatformIcon, true);
                MemoryProfilerWindow.SetPlatformIcons(itemUI.Item, snapshotGUIData);
                UIElementsHelper.SwitchVisibility(snapshotGUIData.dynamicVisualElements.closeButton, snapshotGUIData.dynamicVisualElements.openButton, true);
                itemUI.UtcDateTime = snapshotGUIData.UtcDateTime;
                if (first && secondIsOpen || !first && firstIsOpen)
                {
                    m_OpenSnapshotItemUIFirst.Age.text = GetAgeDifference(m_OpenSnapshotItemUIFirst.UtcDateTime, m_OpenSnapshotItemUISecond.UtcDateTime);
                    m_OpenSnapshotItemUISecond.Age.text = GetAgeDifference(m_OpenSnapshotItemUISecond.UtcDateTime, m_OpenSnapshotItemUIFirst.UtcDateTime);
                }
            }
            if (first)
                firstIsOpen = snapshotGUIData != null;
            else
                secondIsOpen = snapshotGUIData != null;

            if (isInView)
            {
                SetFocusFirst(first);
                SetFocusSecond(!first);
                SetFocusDiff(false);
            }
            else
            {
                if (first)
                    SetFocusFirst(false);
                else
                    SetFocusSecond(false);
            }

            m_DiffButton.SetEnabled(firstIsOpen && secondIsOpen);

            UpdateWidth(layout.width);
        }

        string GetAgeDifference(DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime.CompareTo(otherDateTime) < 0 ? "Old" : "New";
        }

        struct LabelWidthData
        {
            public readonly float Padding;
            public readonly float Margin;
            public readonly float TextWidth;

            float m_SnapshotMinWidth;
            Label m_Label;

            public float ExtraWidthNeeded
            {
                get
                {
                    return TextWidth + Padding + Margin - m_SnapshotMinWidth;
                }
            }

            public LabelWidthData(Label label, float snapshotMinWidth)
            {
                Padding = label.GetPaddingLeftFromStyle() + label.GetPaddingRightFromStyle();
                Margin = label.GetMarginLeftFromStyle() + label.GetMarginRightFromStyle();
                TextWidth = label.MeasureTextSize(label.text, label.GetWidthFromStyle(), MeasureMode.Undefined, 20, MeasureMode.Exactly).x;
                m_SnapshotMinWidth = snapshotMinWidth;
                m_Label = label;
            }

            public void SetMaxWidth(float maxWidth)
            {
                maxWidth -= Margin;
                m_Label.style.maxWidth = maxWidth;
                m_Label.style.minWidth = maxWidth;
                m_Label.style.unityTextAlign = (maxWidth >= ExtraWidthNeeded + m_SnapshotMinWidth) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
            }
        }

        struct SnapshotUIWidthData
        {
            public readonly LabelWidthData NameData;
            public readonly LabelWidthData DateData;

            public float ExtraWidthNeeded
            {
                get
                {
                    return Mathf.Max(Mathf.Max(NameData.ExtraWidthNeeded, DateData.ExtraWidthNeeded), 0);
                }
            }

            public SnapshotUIWidthData(OpenSnapshotItemUI uiItem, float snapshotMinWidth)
            {
                NameData = new LabelWidthData(string.IsNullOrEmpty(uiItem.Name.text) ? uiItem.NoData : uiItem.Name, snapshotMinWidth);
                DateData = new LabelWidthData(uiItem.Date, snapshotMinWidth);
            }

            public void SetMaxWidth(float maxWidth)
            {
                NameData.SetMaxWidth(maxWidth);
                DateData.SetMaxWidth(maxWidth);
            }
        }

        public void UpdateWidth(float width)
        {
            float snapshotMinWidth = m_FirstSnapshotHolder.GetMinWidthFromStyle();

            var distributableWidth = width - (2 * snapshotMinWidth + m_DiffButtonHolder.GetMinWidthFromStyle());

            var firstSnapshotWidthData = new SnapshotUIWidthData(m_OpenSnapshotItemUIFirst, snapshotMinWidth);
            var secondSnapshotWidthData = new SnapshotUIWidthData(m_OpenSnapshotItemUISecond, snapshotMinWidth);

            if (k_SplitWidthEvenly)
            {
                firstSnapshotWidthData.SetMaxWidth(distributableWidth / 2f + snapshotMinWidth);
                secondSnapshotWidthData.SetMaxWidth(distributableWidth / 2f + snapshotMinWidth);
            }
            else
            {
                var superfluousWidth = distributableWidth - (firstSnapshotWidthData.ExtraWidthNeeded + secondSnapshotWidthData.ExtraWidthNeeded);
                if (superfluousWidth >= 0)
                {
                    m_DiffButtonHolder.style.maxWidth = -1;

                    return;
                }
                else
                {
                    m_DiffButtonHolder.style.maxWidth = m_DiffButtonHolder.style.minWidth;

                    var totalNeeded = firstSnapshotWidthData.ExtraWidthNeeded + secondSnapshotWidthData.ExtraWidthNeeded;
                    var firstSnapshotRatio = firstSnapshotWidthData.ExtraWidthNeeded / totalNeeded;
                    var secondSnapshotRatio = secondSnapshotWidthData.ExtraWidthNeeded / totalNeeded;
                    var firstSnapshotExtraWidth = firstSnapshotRatio * distributableWidth;
                    var secondSnapshotExtraWidth = secondSnapshotRatio * distributableWidth;

                    firstSnapshotWidthData.SetMaxWidth(firstSnapshotExtraWidth + snapshotMinWidth);
                    secondSnapshotWidthData.SetMaxWidth(secondSnapshotExtraWidth + snapshotMinWidth);
                }
            }
        }

        public void FocusDiff()
        {
            SetFocusFirst(false);
            SetFocusDiff(true);
            SetFocusSecond(false);
        }

        public void FocusFirst()
        {
            SetFocusFirst(true);
            SetFocusDiff(false);
            SetFocusSecond(false);
        }

        public void FocusSecond()
        {
            SetFocusFirst(false);
            SetFocusDiff(false);
            SetFocusSecond(true);
        }

        void SetFocusFirst(bool on)
        {
            if (on)
            {
                m_OpenSnapshotItemUIFirst.Image.AddToClassList(k_SelectedSnapshotClassName);
                m_FirstSnapshotHolder.AddToClassList(k_SelectedSnapshotClassName);
            }
            else
            {
                m_OpenSnapshotItemUIFirst.Image.RemoveFromClassList(k_SelectedSnapshotClassName);
                m_FirstSnapshotHolder.RemoveFromClassList(k_SelectedSnapshotClassName);
            }
        }

        internal void RefreshScreenshots(SnapshotFileGUIData guiDataFirst, SnapshotFileGUIData guiDataSecond)
        {
            if (guiDataFirst != null && m_OpenSnapshotItemUIFirst != null && m_OpenSnapshotItemUIFirst.Image != null)
                m_OpenSnapshotItemUIFirst.Image.image  = guiDataFirst.texture;
            if (guiDataSecond != null && m_OpenSnapshotItemUISecond != null && m_OpenSnapshotItemUISecond.Image != null)
                m_OpenSnapshotItemUISecond.Image.image = guiDataSecond.texture;
        }

        void SetFocusSecond(bool on)
        {
            if (on)
            {
                m_OpenSnapshotItemUISecond.Image.AddToClassList(k_SelectedSnapshotClassName);
                m_SecondSnapshotHolder.AddToClassList(k_SelectedSnapshotClassName);
            }
            else
            {
                m_OpenSnapshotItemUISecond.Image.RemoveFromClassList(k_SelectedSnapshotClassName);
                m_SecondSnapshotHolder.RemoveFromClassList(k_SelectedSnapshotClassName);
            }
        }

        void SetFocusDiff(bool on)
        {
            if (on)
            {
                m_DiffButtonHolder.AddToClassList(k_SelectedSnapshotClassName);
                m_DiffButton.AddToClassList(k_SelectedSnapshotClassName);
            }
            else
            {
                m_DiffButtonHolder.RemoveFromClassList(k_SelectedSnapshotClassName);
                m_DiffButton.RemoveFromClassList(k_SelectedSnapshotClassName);
            }
        }
    }
}
