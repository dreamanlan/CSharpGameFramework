using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using System.IO;

using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEditorInternal;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using UnityEditor.UIElements;
#else
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
#endif
#if UNITY_2019_3_OR_NEWER
using UnityEngine.Profiling.Experimental;
#endif

#if UNITY_2020_1_OR_NEWER
using UnityEditor.Networking.PlayerConnection;
using UnityEngine.Networking.PlayerConnection;
#else
using ConnectionUtility = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUIUtility;
using ConnectionGUI = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUI;
using UnityEngine.Experimental.Networking.PlayerConnection;
#endif


using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.MemoryProfilerForExtension.Editor;
using Unity.MemoryProfilerForExtension.Editor.UI;
using Unity.MemoryProfilerForExtension.Editor.Legacy;
using Unity.MemoryProfilerForExtension.Editor.Legacy.LegacyFormats;
using Unity.MemoryProfilerForExtension.Editor.EnumerationUtilities;

[assembly: InternalsVisibleTo("Unity.MemoryProfilerForExtension.Editor.Tests")]
namespace Unity.MemoryProfilerForExtension.Editor
{
    using QueryMemoryProfiler = UnityEngine.Profiling.Memory.Experimental.MemoryProfiler;
    internal class MemoryProfilerWindow : EditorWindow, UI.IViewPaneEventListener
    {
        static class Content
        {
            public static readonly GUIContent NoneView = new GUIContent("None", "");
            public static readonly GUIContent MemoryMapView = new GUIContent("Memory Map", "Show Snapshot as Memory Map");
            public static readonly GUIContent MemoryMapViewDiff = new GUIContent("Memory Map Diff", "Show Snapshot Diff as Memory Map");
            public static readonly GUIContent TreeMapView = new GUIContent("Tree Map", "Show Snapshot as Memory Tree");
            public static readonly GUIContent TableMapViewRoot = new GUIContent("Table/", "");
            public static readonly GUIContent RawDataTableMapViewRoot = new GUIContent("Raw Data/", "");
            public static readonly GUIContent DiffRawDataTableMapViewRoot = new GUIContent("Diff Raw Data/", "");
            public static readonly GUIContent SnapshotOptionMenuItemDelete = new GUIContent("Delete", "Deletes the snapshot file from disk.");
            public static readonly GUIContent SnapshotOptionMenuItemRename = new GUIContent("Rename", "Renames the snapshot file on disk.");
            public static readonly GUIContent SnapshotOptionMenuItemBrowse = new GUIContent("Open Folder", "Opens the folder where the snapshot file is located on disk.");

            public static GUIContent Title
            {
                get
                {
                    s_Title.image = Icons.MemoryProfilerWindowTabIcon;
                    return s_Title;
                }
            }
            static GUIContent s_Title = new GUIContent("Memory Profiler");

            public static GUIContent AttachToPlayerCachedTargetName = new GUIContent();

            public const string ImportSnapshotWindowTitle = "Import snapshot file";
            public const string DeleteSnapshotDialogTitle = "Delete Snapshot";
            public const string DeleteSnapshotDialogMessage = "Are you sure you want to delete the snapshot?";
            public const string DeleteSnapshotDialogAccept = "OK";
            public const string DeleteSnapshotDialogCancel = "Cancel";

            public const string RenameSnapshotDialogTitle = "Rename Open Snapshot";
            public const string RenameSnapshotDialogMessage = "Renaming an open snapshot will close it. Are you sure you want to close the snapshot?";
            public const string RenameSnapshotDialogAccept = "OK";
            public const string RenameSnapshotDialogCancel = "Cancel";

            public const string HeapWarningWindowTitle = "Warning!";
            public const string HeapWarningWindowContent = "Memory snapshots contain all memory in the managed heap of your Unity Player or Editor as raw data at the moment of capture. " +
                "This might include passwords, server keys, access tokens and other personally identifying data. " +
                "Please use special caution when sharing snapshots. For more information on this, please visit the Memory Profiler Documentation.";
            public const string HeapWarningWindowOK = "Take Snapshot";

            public static readonly string[] MemorySnapshotImportWindowFileExtensions = new string[] { "MemorySnapshot", "snap", "Bitbucket MemorySnapshot", "memsnap,memsnap2,memsnap3" };
        }


        static MemoryProfilerWindow s_MemoryProfilerWindow = null;
        static Dictionary<BuildTarget, string> s_PlatformIconClasses = new Dictionary<BuildTarget, string>();

        const string k_SnapshotFileNamePart = "Snapshot-";
        const string k_SnapshotTempFileName = "temp.tmpsnap";
#if UNITY_2019_3_OR_NEWER
        const string k_SnapshotTempScreenshotFileExtension = ".tmppng";
        const string k_SnapshotScreenshotFileExtension = ".png";
#endif
        internal const string k_SnapshotFileExtension = ".snap";
        internal const string k_ConvertedSnapshotTempFileName = "ConvertedSnaphot.tmpsnap";
        const string k_ViewFileExtension = "xml";
        const string k_RawCategoryName = "Raw";
        const string k_DiffRawCategoryName = "Diff Raw";

        const string k_PackageResourcesPath = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/";

        const string k_UxmlFilesPath = k_PackageResourcesPath + "UXML/";
        const string k_WindowUxmlPath = k_UxmlFilesPath + "MemoryProfilerWindow.uxml";
        const string k_SnapshotListItemUxmlPath = k_UxmlFilesPath + "SnapshotListItem.uxml";

        const string k_StyleSheetsPath = k_PackageResourcesPath + "StyleSheets/";
        const string k_WindowCommonStyleSheetPath = k_StyleSheetsPath + "MemoryProfilerWindow_style.uss";
        const string k_WindowLightStyleSheetPath = k_StyleSheetsPath + "MemoryProfilerWindow_style_light.uss";
        const string k_WindowDarkStyleSheetPath = k_StyleSheetsPath + "MemoryProfilerWindow_style_dark.uss";
        const string k_WindowNewThemingStyleSheetPath = k_StyleSheetsPath + "MemoryProfilerWindow_style_newTheming.uss";

        const string k_SnapshotButtonClassName = "snapshotButton";
        const string k_SnapshotMetaDataTextClassName = "snapshotMetaDataText";
        const string k_EvenRowStyleClass = "evenRow";
        const string k_OddRowStyleClass = "oddRow";

        [NonSerialized]
        SnapshotCollection m_MemorySnapshotsCollection;
        [NonSerialized]
        LegacyReader m_LegacyReader;

        [NonSerialized]
        bool m_PrevApplicationFocusState;

        VisualElement m_ToolbarExtension;

        [SerializeField]
        Vector2 m_ViewDropdownSize;

        bool m_WindowInitialized = false;

        [MenuItem("Dsl资源工具/Memory Profiler", false, 200)]
        public static void ShowWindow()
        {
            var win = GetWindow<MemoryProfilerWindow>(Content.Title.text);
        }
        public static void OpenTableLink(Database.LinkRequest link)
        {
            var win = GetWindow<MemoryProfilerWindow>(Content.Title.text);
            win.OpenLink(link);
        }
        public static CachedSnapshot GetCurCachedSnapshot()
        {
            if (null != s_MemoryProfilerWindow) {
                return s_MemoryProfilerWindow.GetCurCachedSnapshotImpl();
            }
            else {
                return null;
            }
        }
        public static UIState.BaseMode GetCurMode()
        {
            if (null != s_MemoryProfilerWindow) {
                return s_MemoryProfilerWindow.GetCurModeImpl();
            }
            else {
                return null;
            }
        }

        private CachedSnapshot GetCurCachedSnapshotImpl()
        {
            var curMode = UIState.CurrentMode;
            var snapshotMode = curMode as UIState.SnapshotMode;
            var diffMode = curMode as UIState.DiffMode;
            if (null != snapshotMode) {
                return snapshotMode.snapshot;
            }
            else if (null != diffMode) {
                var firstSnapshotMode = diffMode.modeSecond as UIState.SnapshotMode;
                var secondSnapshotMode = diffMode.modeSecond as UIState.SnapshotMode;
                if (null != secondSnapshotMode && null != secondSnapshotMode.snapshot) {
                    return secondSnapshotMode.snapshot;
                }
                else if (null != firstSnapshotMode && null != firstSnapshotMode.snapshot) {
                    return firstSnapshotMode.snapshot;
                }
                else {
                    return null;
                }
            }
            else {
                return null;
            }
        }
        private UIState.BaseMode GetCurModeImpl()
        {
            var curMode = UIState.CurrentMode;
            return curMode;
        }

        CaptureFlags m_CaptureFlags = CaptureFlags.ManagedObjects
            | CaptureFlags.NativeObjects
            | CaptureFlags.NativeAllocations
            | CaptureFlags.NativeAllocationSites
            | CaptureFlags.NativeStackTraces;


        Button m_BackwardsInHistoryButton;
        Button m_ForwardsInHistoryButton;
        ToolbarButton m_InportButton;
        VisualElement m_ViewSelectorMenu;

        VisualElement m_AttachToPlayerDropdownHolder;
        Button m_CaptureButton;

        //storing the fixed width of the VisualElements of the left part of the toolbar (i.e. excluding the AttachToPlayerDropdown)
        float m_LeftPaneToolbarWidthTakenByUIElements;
        float m_LeftPaneToolbarHeight;

        VisualElement m_LeftPane;
        VisualElement m_MainViewPanel;
        VisualTreeAsset m_SnapshotListItemTree;
        VisualElement m_EmptyWorkbenchText;
        VisualElement m_SnapshotList;
        bool m_ShowEmptySnapshotListHint = true;

        IConnectionState m_PlayerConnectionState = null;

        StringBuilder m_TabelNameStringBuilder = new StringBuilder();
        Dictionary<string, GUIContent> m_UIFriendlyViewOptionNamesWithFullPath = new Dictionary<string, GUIContent>();
        Dictionary<string, GUIContent> m_UIFriendlyViewOptionNames = new Dictionary<string, GUIContent>();

        OpenSnapshotsManager m_OpenSnapshots = new OpenSnapshotsManager();

        public event Action<UIState> UIStateChanged = delegate {};

        public event Action<float> SidebarWidthChanged = delegate {};

        public UI.UIState UIState { get; private set; }

        private UI.ViewPane currentViewPane
        {
            get
            {
                if (UIState.CurrentMode == null) return null;
                return UIState.CurrentMode.CurrentViewPane;
            }
        }

        void Init()
        {
            s_MemoryProfilerWindow = this;
            m_WindowInitialized = true;

            UIState = new UI.UIState();
            Styles.Initialize();
            EditorCoroutineUtility.StartCoroutine(UpdateTitle(), this);
            MemoryProfilerAnalytics.EnableAnalytics();
            m_MemorySnapshotsCollection = new SnapshotCollection(MemoryProfilerSettings.AbsoluteMemorySnapshotStoragePath);
            m_PrevApplicationFocusState = InternalEditorUtility.isApplicationActive;
            EditorApplication.update += PollForApplicationFocus;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            m_LegacyReader = new LegacyReader();

#if UNITY_2019_1_OR_NEWER
            var root = this.rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_WindowCommonStyleSheetPath));
            if (EditorGUIUtility.isProSkin)
            {
                root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_WindowDarkStyleSheetPath));
            }
            else
            {
                root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_WindowLightStyleSheetPath));
            }
#if UNITY_2019_3_OR_NEWER
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(k_WindowNewThemingStyleSheetPath));
#endif // UNITY_2019_3_OR_NEWER
            // Import UXML
            var windowTree = AssetDatabase.LoadAssetAtPath(k_WindowUxmlPath, typeof(VisualTreeAsset)) as VisualTreeAsset;
            windowTree.CloneTree(root);
#else // UNITY_2019_1_OR_NEWER -> !UNITY_2019_1_OR_NEWER
            var root = this.GetRootVisualContainer();
            root.AddStyleSheetPath(k_WindowCommonStyleSheetPath);
            root.AddStyleSheetPath(k_WindowCommonStyleSheetPath);
            if (EditorGUIUtility.isProSkin)
            {
                root.AddStyleSheetPath(k_WindowDarkStyleSheetPath);
            }
            else
            {
                root.AddStyleSheetPath(k_WindowLightStyleSheetPath);
            }
            // Import UXML
            var windowTree = AssetDatabase.LoadAssetAtPath(k_WindowUxmlPath, typeof(VisualTreeAsset)) as VisualTreeAsset;
            var slots = new Dictionary<string, VisualElement>();
            windowTree.CloneTree(root, slots);
#endif // !UNITY_2019_1_OR_NEWER

            // Add toolbar functionality
#if UNITY_2020_1_OR_NEWER
            m_PlayerConnectionState = PlayerConnectionGUIUtility.GetConnectionState(this);
#else
            m_PlayerConnectionState = ConnectionUtility.GetAttachToPlayerState(this);
#endif

            m_AttachToPlayerDropdownHolder = root.Q("attachToPlayerMenuPlaceholder");
            m_AttachToPlayerDropdownHolder.Add(new IMGUIContainer(DrawAttachToPlayerDropdown) { name = "attachToPlayerMenu" });

            if (root.Q<ToolbarButton>("captureButton") == null)
                return; // in case the construction of the Window went wrong, exit here
            m_CaptureButton = root.Q<ToolbarButton>("captureButton");
            m_CaptureButton.clickable.clicked += TakeCapture;

            m_InportButton = root.Q<ToolbarButton>("importButton");
            m_InportButton.clickable.clicked += ImportCapture;

            EditorApplication.playModeStateChanged += PlaymodeStateChanged;
            PlaymodeStateChanged(Application.isPlaying ? PlayModeStateChange.EnteredPlayMode : PlayModeStateChange.EnteredEditMode);

            m_ViewSelectorMenu = new IMGUIContainer(DrawTableSelection);
            root.Q("viewSelectorMenuPlaceholder").Add(m_ViewSelectorMenu);

            m_MainViewPanel = root.Q("mainWindowContent");
            RecreateMainView();

            m_BackwardsInHistoryButton = root.Q<ToolbarButton>("backwardsInHistoryButton", "iconButton");
            m_BackwardsInHistoryButton.clickable.clicked += StepBackwardsInHistory;
            m_BackwardsInHistoryButton.SetEnabled(false);
            m_ForwardsInHistoryButton = root.Q<ToolbarButton>("forwardsInHistoryButton", "iconButton");
            m_ForwardsInHistoryButton.clickable.clicked += StepForwardsInHistory;
            m_ForwardsInHistoryButton.SetEnabled(false);

            m_ToolbarExtension = root.Query<Toolbar>("ToolbarExtension").First();

            // setup the references for the snapshot list to be used for initial filling of the snapshot items as well as new captures and imports
            m_EmptyWorkbenchText = root.Q("emptyWorkbenchText");
            m_SnapshotList = root.Q("snapshotListSidebar");
            (m_SnapshotList as ScrollView).showVertical = false;
            m_SnapshotListItemTree = AssetDatabase.LoadAssetAtPath(k_SnapshotListItemUxmlPath, typeof(VisualTreeAsset)) as VisualTreeAsset;
            // fill snapshot List
            RefreshSnapshotList();
            m_MemorySnapshotsCollection.collectionRefresh += RefreshSnapshotList;

            // setup the splitter between sidebar and mainWindow area
            m_LeftPane = root.Q("sidebar");
            var right = root.Q("mainWindow");

            var splitter = new WorkbenchSplitter(Styles.General.InitialWorkbenchWidth);
            splitter.LeftPane.Add(m_LeftPane);
            splitter.RightPane.Add(right);

            root.Add(splitter);

            // setup the open snapshots panel
            var openSnapshotsPanel = m_OpenSnapshots.InitializeOpenSnapshotsWindow(Styles.General.InitialWorkbenchWidth);
            SidebarWidthChanged += openSnapshotsPanel.UpdateWidth;
            root.Q("sidebar").Add(openSnapshotsPanel);

            splitter.LeftPaneWidthChanged += (f) => EditorCoroutineUtility.StartCoroutine(UpdateOpenSnapshotsPaneAfterLayout(), this);

            // during on enabled, none of the layout is filled with useful numbers (most is just 0 or NaN).
            EditorCoroutineUtility.StartCoroutine(UpdateOpenSnapshotsPaneAfterLayout(), this);
            CompilationPipeline.assemblyCompilationStarted += StartedCompilationCallback;
            CompilationPipeline.assemblyCompilationFinished += FinishedCompilationCallback;

            UIStateChanged += OnUIStateChanged;
            UIStateChanged += m_OpenSnapshots.RegisterUIState;
            UIStateChanged(UIState);
        }

        void OnGUI()
        {
            if (m_WindowInitialized)
                return;

            Init();
        }

        void StartedCompilationCallback(string msg)
        {
            //Disable the capture button during compilation.
            m_CaptureButton.SetEnabled(false);
        }

        void FinishedCompilationCallback(string msg, UnityEditor.Compilation.CompilerMessage[] compilerMsg)
        {
            m_CaptureButton.SetEnabled(true);
        }

        IEnumerator UpdateTitle()
        {
            yield return null;
            titleContent = Content.Title;
        }

        IEnumerator UpdateOpenSnapshotsPaneAfterLayout()
        {
            if (m_LeftPane == null)
                yield break;
            while (float.IsNaN(m_LeftPane.layout.width))
            {
                yield return null;
            }
            if (float.IsNaN(m_LeftPaneToolbarWidthTakenByUIElements) || m_LeftPaneToolbarWidthTakenByUIElements <= 0)
            {
#if UNITY_2019_1_OR_NEWER
                var root = this.rootVisualElement;
#else
                var root = this.GetRootVisualContainer();
#endif
                var captureButton = root.Q<ToolbarButton>("captureButton");
                var importButton = root.Q<ToolbarButton>("importButton");
                var toolbarSpacer = root.Q<ToolbarSpacer>();
                m_LeftPaneToolbarHeight = toolbarSpacer.layout.height;
                m_LeftPaneToolbarWidthTakenByUIElements = captureButton.layout.width + importButton.layout.width + toolbarSpacer.layout.width * 3;
            }
            SidebarWidthChanged(m_LeftPane.layout.width);
        }

        void RefreshSnapshotList()
        {
            m_SnapshotList.Clear();
            m_ShowEmptySnapshotListHint = true;
            using (var snapshots = m_MemorySnapshotsCollection.GetEnumerator())
            {
                while (snapshots.MoveNext())
                {
                    AddSnapshotToUI(snapshots.Current);
                }
            }
            if (m_ShowEmptySnapshotListHint)
            {
                ShowWorkBenchHintText();
            }
        }

        void ShowWorkBenchHintText()
        {
            if (m_SnapshotList.childCount <= 0 && m_ShowEmptySnapshotListHint)
            {
                UIElementsHelper.SwitchVisibility(m_EmptyWorkbenchText, m_SnapshotList);
            }
        }

        void OnUIStateChanged(UIState newState)
        {
            newState.ModeChanged += OnModeChanged;
            OnModeChanged(newState.CurrentMode, newState.CurrentViewMode);
            newState.history.historyChanged += HistoryChanged;
        }

        void OnModeChanged(UIState.BaseMode newMode, UIState.ViewMode newViewMode)
        {
            if (newMode != null)
            {
                newMode.ViewPaneChanged -= OnViewPaneChanged;
                newMode.ViewPaneChanged += OnViewPaneChanged;
            }
            if (newViewMode == UIState.ViewMode.ShowNone)
            {
                ShowNothing();
            }
            if (UIState.CurrentMode != null && UIState.CurrentMode.CurrentViewPane == null)
            {
                TransitPane(UIState.CurrentMode.GetDefaultView(UIState, this));
            }

            RecreateMainView();
        }

        void OnViewPaneChanged(ViewPane newPane)
        {
            RecreateMainView();
        }

        void RecreateMainView()
        {
            if (m_MainViewPanel == null)
                return;
            if (UIState.CurrentMode == null || UIState.CurrentMode.CurrentViewPane == null)
            {
                m_MainViewPanel.Clear();
            }
            else
            {
                m_MainViewPanel.Clear();
                foreach (var element in UIState.CurrentMode.CurrentViewPane.VisualElements)
                {
                    m_MainViewPanel.Add(element);
                }
            }
        }

        void SetRowStyleOnSnapshotListItem(VisualElement snapshotListItem, int row)
        {
            var snapshotListItemContainer = snapshotListItem.Q("snapshotListItem");
            var snapshotButtons = snapshotListItem.Q("snapshotButtons");
            if (row % 2 == 0)
            {
                snapshotListItemContainer.AddToClassList(k_EvenRowStyleClass);
                snapshotButtons.AddToClassList(k_EvenRowStyleClass);

                snapshotListItemContainer.RemoveFromClassList(k_OddRowStyleClass);
                snapshotButtons.RemoveFromClassList(k_OddRowStyleClass);
            }
            else
            {
                snapshotListItemContainer.AddToClassList(k_OddRowStyleClass);
                snapshotButtons.AddToClassList(k_OddRowStyleClass);

                snapshotListItemContainer.RemoveFromClassList(k_EvenRowStyleClass);
                snapshotButtons.RemoveFromClassList(k_EvenRowStyleClass);
            }
        }

        void AddSnapshotToUI(SnapshotFileData snapshot)
        {
            if (m_SnapshotList == null || m_SnapshotListItemTree == null)
                return;

            if (m_ShowEmptySnapshotListHint)
            {
                // take out the empty-snapshot-list-please-take-a-capture-hint text
                UIElementsHelper.SwitchVisibility(m_SnapshotList, m_EmptyWorkbenchText);
                m_ShowEmptySnapshotListHint = false;
            }

#if UNITY_2019_1_OR_NEWER
            var snapshotListItem = m_SnapshotListItemTree.CloneTree();
#else
            var slots = new Dictionary<string, VisualElement>();
            var snapshotListItem = m_SnapshotListItemTree.CloneTree(slots);
#endif
            SetRowStyleOnSnapshotListItem(snapshotListItem, m_SnapshotList.childCount);

            m_SnapshotList.Add(snapshotListItem);
            if (snapshot.GuiData != null)
            {
                Image image = null;
                if (snapshot.GuiData.MetaScreenshot)
                {
                    image = snapshotListItem.Q<Image>("previewImage", "previewImage");
                    image.image = snapshot.GuiData.MetaScreenshot;
                    image.scaleMode = ScaleMode.ScaleToFit;
                    image.tooltip = snapshot.GuiData.MetaPlatform.text;
                }
                SetPlatformIcons(snapshotListItem, snapshot.GuiData);

                var firstLabel = snapshotListItem.Query<Label>("snapshotName", k_SnapshotMetaDataTextClassName).First();
                firstLabel.text = snapshot.GuiData.Name.text;
                firstLabel.tooltip = snapshot.FileInfo.FullName;
                firstLabel.AddManipulator(new Clickable(() => RenameCapture(snapshot)));
                var renameField = snapshotListItem.Q<TextField>("snapshotName");
                renameField.SetValueWithoutNotify(firstLabel.text);
                renameField.isDelayed = true;
                var secondLabel = snapshotListItem.Query<Label>("secondRowMetaData", k_SnapshotMetaDataTextClassName).First();
                secondLabel.text = snapshot.GuiData.SnapshotDate.text;
                secondLabel.tooltip = snapshot.GuiData.MetaContent.text;

                //store the references to dynamic elements for later
                snapshot.GuiData.dynamicVisualElements = new SnapshotFileGUIData.DynamicVisualElements
                {
                    screenshot = image,
                    snapshotNameLabel = firstLabel,
                    snapshotDateLabel = secondLabel,
                    snapshotRenameField = renameField,
                    snapshotListItem = snapshotListItem,
                    openButton = snapshotListItem.Q<Button>("openSnapshotButton", k_SnapshotButtonClassName),
                    optionDropdownButton = snapshotListItem.Q<Image>("optionButton"),
                    closeButton = snapshotListItem.Q<Button>("closeSnapshotButton", k_SnapshotButtonClassName),
                };
                snapshot.GuiData.dynamicVisualElements.openButton.clickable.clicked += () => OpenCapture(snapshot);
                snapshot.GuiData.dynamicVisualElements.optionDropdownButton.AddManipulator(new Clickable(() => OpenSnapshotOptionMenu(snapshot)));
                snapshot.GuiData.dynamicVisualElements.closeButton.clickable.clicked += () => m_OpenSnapshots.CloseCapture(snapshot);

                snapshot.GuiData.dynamicVisualElements.snapshotRenameField.RegisterCallback<ChangeEvent<string>>((evt) =>
                {
                    if (evt.newValue != evt.previousValue)
                    {
                        m_MemorySnapshotsCollection.RenameSnapshot(snapshot, snapshot.GuiData.dynamicVisualElements.snapshotRenameField.text);
                    }
                });

                snapshot.GuiData.dynamicVisualElements.snapshotRenameField.RegisterCallback<KeyDownEvent>((evt) =>
                {
                    if (evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return)
                    {
                        m_MemorySnapshotsCollection.RenameSnapshot(snapshot, snapshot.GuiData.dynamicVisualElements.snapshotRenameField.text);
                    }
                    if (evt.keyCode == KeyCode.Escape)
                    {
                        // abort the name change
                        snapshot.GuiData.RenamingFieldVisible = false;
                        m_MemorySnapshotsCollection.RenameSnapshot(snapshot, snapshot.GuiData.Name.text);
                    }
                });
                snapshot.GuiData.dynamicVisualElements.snapshotRenameField.RegisterCallback<BlurEvent>((evt) =>
                {
                    m_MemorySnapshotsCollection.RenameSnapshot(snapshot, snapshot.GuiData.dynamicVisualElements.snapshotRenameField.text);
                });

                UIElementsHelper.SetVisibility(snapshot.GuiData.dynamicVisualElements.closeButton, false);
            }
        }

        public static void SetPlatformIcons(VisualElement snapshotItem, SnapshotFileGUIData snapshotGUIData)
        {
            Image platformIcon = snapshotItem.Q<Image>("platformIcon", "platformIcon");
            Image editorIcon = snapshotItem.Q<Image>("editorIcon", "platformIcon");
            platformIcon.ClearClassList();
            platformIcon.AddToClassList("platformIcon");
            var platformIconClass = GetPlatformIconClass(snapshotGUIData.runtimePlatform);
            if (!string.IsNullOrEmpty(platformIconClass))
                platformIcon.AddToClassList(platformIconClass);
            if (snapshotGUIData.MetaPlatform.text.Contains("Editor"))
            {
                UIElementsHelper.SetVisibility(editorIcon, true);
                platformIcon.AddToClassList("Editor");
            }
            else
                UIElementsHelper.SetVisibility(editorIcon, false);
        }

        static string GetPlatformIconClass(RuntimePlatform platform)
        {
            BuildTarget buildTarget = BuildTarget.NoTarget;
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    buildTarget = BuildTarget.StandaloneWindows;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    buildTarget = BuildTarget.iOS;
                    break;
                case RuntimePlatform.Android:
                    buildTarget = BuildTarget.Android;
                    break;
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    buildTarget = BuildTarget.WebGL;
                    break;
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerARM:
                    buildTarget = BuildTarget.WSAPlayer;
                    break;
                case RuntimePlatform.PS4:
                    buildTarget = BuildTarget.PS4;
                    break;
                case RuntimePlatform.XboxOne:
                    buildTarget = BuildTarget.XboxOne;
                    break;
                case RuntimePlatform.tvOS:
                    buildTarget = BuildTarget.tvOS;
                    break;
                case RuntimePlatform.Switch:
                    buildTarget = BuildTarget.Switch;
                    break;
                case RuntimePlatform.Lumin:
                    buildTarget = BuildTarget.Lumin;
                    break;
                default:
                    // Unknown target
                    return null;
            }

            if (!s_PlatformIconClasses.ContainsKey(buildTarget))
            {
                s_PlatformIconClasses[buildTarget] = buildTarget.ToString();
            }
            return s_PlatformIconClasses[buildTarget];
        }

        private void OpenSnapshotOptionMenu(SnapshotFileData snapshot)
        {
            var menu = new GenericMenu();
            menu.AddItem(Content.SnapshotOptionMenuItemDelete, false, () => DeleteCapture(snapshot));
            menu.AddItem(Content.SnapshotOptionMenuItemRename, false, () => RenameCapture(snapshot));
            menu.AddItem(Content.SnapshotOptionMenuItemBrowse, false, () => BrowseCaptureFolder(snapshot));
            menu.ShowAsContext();
        }

        void OnSceneChanged(Scene sceneA, Scene sceneB)
        {
            m_MemorySnapshotsCollection.RefreshScreenshots();
            m_OpenSnapshots.RefreshScreenshots();
        }

        void PollForApplicationFocus()
        {
            if (m_PrevApplicationFocusState != InternalEditorUtility.isApplicationActive)
            {
                if (!m_PrevApplicationFocusState)
                {
                    m_MemorySnapshotsCollection.RefreshCollection();
                }
                m_PrevApplicationFocusState = InternalEditorUtility.isApplicationActive;
            }
        }

        void OnDisable()
        {
            m_WindowInitialized = false;
            Styles.Cleanup();
            ProgressBarDisplay.ClearBar();
            UIStateChanged = delegate {};
            UIState.ClearAllOpenModes();
            SidebarWidthChanged = delegate {};
            if (m_PlayerConnectionState != null)
            {
                m_PlayerConnectionState.Dispose();
                m_PlayerConnectionState = null;
                EditorApplication.playModeStateChanged -= PlaymodeStateChanged;
            }

            CompilationPipeline.assemblyCompilationStarted -= StartedCompilationCallback;
            CompilationPipeline.assemblyCompilationFinished -= FinishedCompilationCallback;
            EditorApplication.update -= PollForApplicationFocus;
            EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
        }

        void UI.IViewPaneEventListener.OnRepaint()
        {
            Repaint();
        }

        void UI.IViewPaneEventListener.OnOpenLink(Database.LinkRequest link)
        {
            OpenLink(link);
        }

        void UI.IViewPaneEventListener.OnOpenLink(Database.LinkRequest link, UIState.SnapshotMode mode)
        {
            var tableLinkRequest = link as Database.LinkRequestTable;
            if (tableLinkRequest != null)
            {
                var tableRef = new Database.TableReference(tableLinkRequest.LinkToOpen.TableName, link.Parameters);
                var table = mode.GetSchema().GetTableByReference(tableRef);

                var pane = new UI.SpreadsheetPane(UIState, this);
                if (pane.OpenLinkRequest(tableLinkRequest, tableRef, table))
                {
                    UIState.TransitModeToOwningTable(table);
                    TransitPane(pane);
                }
            }
        }

        void UI.IViewPaneEventListener.OnOpenMemoryMap()
        {
            if (UIState.CurrentMode is UI.UIState.SnapshotMode)
            {
                OpenMemoryMap(null);
            }
            else
            {
                OpenMemoryMapDiff(null);
            }
        }

        void UI.IViewPaneEventListener.OnOpenTreeMap()
        {
            OpenTreeMap(null);
        }

        void HistoryChanged()
        {
            m_BackwardsInHistoryButton.SetEnabled(UIState.history.hasPast);
            m_ForwardsInHistoryButton.SetEnabled(UIState.history.hasFuture);
        }

        void TransitPane(UI.ViewPane newPane)
        {
            UIState.CurrentMode.TransitPane(newPane);
        }

        void ShowNothing()
        {
            TransitPane(null);
        }

        void StepBackwardsInHistory()
        {
            var history = UIState.history;
            if (history.hasPast)
            {
                if (!history.hasPresentEvent)
                {
                    if (UIState.CurrentMode != null)
                    {
                        history.SetPresentEvent(UIState.CurrentMode.GetCurrentHistoryEvent());
                    }
                }
                EditorCoroutineUtility.StartCoroutine(DelayedHistoryEvent(history.Backward()), this);
                Repaint();
            }
        }

        void StepForwardsInHistory()
        {
            var evt = UIState.history.Forward();
            if (evt != null)
            {
                EditorCoroutineUtility.StartCoroutine(DelayedHistoryEvent(evt), this);
                Repaint();
            }
        }

        void AddCurrentHistoryEvent()
        {
            if (currentViewPane != null)
            {
                UIState.AddHistoryEvent(currentViewPane.GetCurrentHistoryEvent());
            }
        }

        void OpenLink(Database.LinkRequest link)
        {
            var tableLinkRequest = link as Database.LinkRequestTable;
            if (tableLinkRequest != null)
            {
                try
                {
                    ProgressBarDisplay.ShowBar("Resolving Link...");
                    var tableRef = new Database.TableReference(tableLinkRequest.LinkToOpen.TableName, link.Parameters);
                    var table = UIState.CurrentMode.GetSchema().GetTableByReference(tableRef);

                    ProgressBarDisplay.UpdateProgress(0.0f, "Updating Table...");
                    if (table.Update())
                    {
                        UIState.CurrentMode.UpdateTableSelectionNames();
                    }

                    ProgressBarDisplay.UpdateProgress(0.75f, "Opening Table...");
                    var pane = new UI.SpreadsheetPane(UIState, this);
                    if (pane.OpenLinkRequest(tableLinkRequest, tableRef, table))
                    {
                        UIState.TransitModeToOwningTable(table);
                        TransitPane(pane);
                    }
                }
                finally
                {
                    ProgressBarDisplay.ClearBar();
                }
            }
            else
                Debug.LogWarning("Cannot open unknown link '" + link.ToString() + "'");
        }

        void OpenTable(Database.TableReference tableRef, Database.Table table)
        {
            UIState.TransitModeToOwningTable(table);
            var pane = new UI.SpreadsheetPane(UIState, this);
            pane.OpenTable(tableRef, table);
            TransitPane(pane);
        }

        void OpenTable(Database.TableReference tableRef, Database.Table table, Database.CellPosition pos)
        {
            UIState.TransitModeToOwningTable(table);
            var pane = new UI.SpreadsheetPane(UIState, this);
            pane.OpenTable(tableRef, table, pos);
            TransitPane(pane);
        }

        void OpenHistoryEvent(UI.HistoryEvent evt)
        {
            if (evt == null) return;

            UIState.TransitMode(evt.Mode);

            if (evt is SpreadsheetPane.History)
            {
                OpenTable(evt);
            }
            else if (evt is MemoryMapPane.History)
            {
                OpenMemoryMap(evt);
            }
            else if (evt is MemoryMapDiffPane.History)
            {
                OpenMemoryMapDiff(evt);
            }
            else if (evt is TreeMapPane.History)
            {
                OpenTreeMap(evt);
            }
        }

        void OpenTable(UI.HistoryEvent history)
        {
            var pane = new UI.SpreadsheetPane(UIState, this);

            if (history != null)
                pane.OpenHistoryEvent(history as SpreadsheetPane.History);

            TransitPane(pane);
        }

        void OpenMemoryMap(UI.HistoryEvent history)
        {
            var pane = new UI.MemoryMapPane(UIState, this, m_ToolbarExtension);

            if (history != null)
                pane.RestoreHistoryEvent(history);

            TransitPane(pane);
        }

        void OpenMemoryMapDiff(UI.HistoryEvent history)
        {
            var pane = new UI.MemoryMapDiffPane(UIState, this, m_ToolbarExtension);

            if (history != null)
                pane.RestoreHistoryEvent(history);

            TransitPane(pane);
        }

        void OpenTreeMap(UI.HistoryEvent history)
        {
            TreeMapPane.History evt = history as TreeMapPane.History;

            if (currentViewPane is UI.TreeMapPane)
            {
                if (evt != null)
                {
                    (currentViewPane as UI.TreeMapPane).OpenHistoryEvent(evt);
                    return;
                }
            }
            var pane = new UI.TreeMapPane(UIState, this);
            if (evt != null) pane.OpenHistoryEvent(evt);
            TransitPane(pane);
        }

        void CopyDataToTexture(Texture2D tex, NativeArray<byte> byteArray)
        {
            unsafe
            {
                void* srcPtr = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(byteArray);
                void* dstPtr = tex.GetRawTextureData<byte>().GetUnsafeReadOnlyPtr();
                UnsafeUtility.MemCpy(dstPtr, srcPtr, byteArray.Length * sizeof(byte));
            }
        }

        bool m_SnapshotInProgress;
        bool m_ScreenshotInProgress;
#if UNITY_2019_3_OR_NEWER
        double m_TimestamprOfLastSnapshotReceived;
        const double k_TimeOutForScreenshots = 3;
#endif

        IEnumerator DelayedSnapshotRoutine()
        {
            if (m_SnapshotInProgress || m_ScreenshotInProgress)
            {
                yield break;
            }

            ProgressBarDisplay.ShowBar("Memory capture");
            ProgressBarDisplay.UpdateProgress(0.0f, "Taking capture...");

            yield return null; //skip one frame so we can draw the progress bar

            m_SnapshotInProgress = true;
#if UNITY_2019_3_OR_NEWER
            m_ScreenshotInProgress = true;
#endif
            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.CapturedSnapshotEvent>();
            m_OpenSnapshots.CloseAllOpenSnapshots();
            UIState.ClearAllOpenModes();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            string basePath = Path.Combine(MemoryProfilerSettings.AbsoluteMemorySnapshotStoragePath, k_SnapshotTempFileName);

            bool snapshotCaptureResult = false;
            string capturePath = "";

#if UNITY_2019_3_OR_NEWER
            bool screenshotCaptureResult = false;
            Action<string, bool, DebugScreenCapture> screenshotCaptureFunc = (string path, bool result, DebugScreenCapture screenCapture) =>
            {
                m_ScreenshotInProgress = false;
                screenshotCaptureResult = result;
                if (!screenshotCaptureResult)
                    return;

                ProgressBarDisplay.UpdateProgress(0.8f, "Processing Screenshot");

                if (Path.HasExtension(path))
                {
                    path = Path.ChangeExtension(path, k_SnapshotTempScreenshotFileExtension);
                }

                Texture2D tex = new Texture2D(screenCapture.width, screenCapture.height, screenCapture.imageFormat, false);
                CopyDataToTexture(tex, screenCapture.rawImageDataReference);
                File.WriteAllBytes(path, tex.EncodeToPNG());
                if (Application.isPlaying)
                    Destroy(tex);
                else
                    DestroyImmediate(tex);
            };
#endif
            Action<string, bool> snapshotCaptureFunc = (string path, bool result) =>
            {
                m_SnapshotInProgress = false;
#if UNITY_2019_3_OR_NEWER
                m_TimestamprOfLastSnapshotReceived = EditorApplication.timeSinceStartup;
#endif
                snapshotCaptureResult = result;
                capturePath = path;
            };

#if UNITY_2019_3_OR_NEWER
            if (m_PlayerConnectionState.connectedToTarget == ConnectionTarget.Player || Application.isPlaying)

            {
                QueryMemoryProfiler.TakeSnapshot(basePath, snapshotCaptureFunc, screenshotCaptureFunc, m_CaptureFlags);
            }
            else
#endif
            {
                QueryMemoryProfiler.TakeSnapshot(basePath, snapshotCaptureFunc, m_CaptureFlags);
                m_ScreenshotInProgress = false; //screenshot is not in progress
            }

            ProgressBarDisplay.UpdateProgress(1.0f);

            int framesToSkip = 1;
            //wait for snapshotting operation to finish and skip one frame to update loading bar
            while (framesToSkip > 0)
            {
                if (!m_SnapshotInProgress
#if UNITY_2019_3_OR_NEWER
                    && (!m_ScreenshotInProgress || EditorApplication.timeSinceStartup - m_TimestamprOfLastSnapshotReceived >= k_TimeOutForScreenshots)
#endif
                )
                {
                    --framesToSkip;
                }
                yield return null;
            }

            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.CapturedSnapshotEvent() { success = snapshotCaptureResult });

            if (snapshotCaptureResult)
            {
                string snapshotPath = Path.Combine(MemoryProfilerSettings.AbsoluteMemorySnapshotStoragePath, k_SnapshotFileNamePart + DateTime.Now.Ticks + k_SnapshotFileExtension);
                File.Move(capturePath, snapshotPath);
#if UNITY_2019_3_OR_NEWER
                if (screenshotCaptureResult)
                {
                    capturePath = Path.ChangeExtension(capturePath, k_SnapshotTempScreenshotFileExtension);
                    string screenshotPath = Path.ChangeExtension(snapshotPath, k_SnapshotScreenshotFileExtension);
                    File.Move(capturePath, screenshotPath);
                }
#endif
                var snapshot = m_MemorySnapshotsCollection.AddSnapshotToCollection(snapshotPath);
                AddSnapshotToUI(snapshot);
            }

            ProgressBarDisplay.ClearBar();
        }

        void TakeCapture()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("Unable to snapshot while compilation is ongoing");
                return;
            }

            if (EditorUtilityCompatibilityHelper.DisplayDialog(Content.HeapWarningWindowTitle, Content.HeapWarningWindowContent, Content.HeapWarningWindowOK, EditorUtilityCompatibilityHelper.DialogOptOutDecisionType.ForThisMachine, MemoryProfilerSettings.HeapWarningWindowOptOutKey))
            {
                this.StartCoroutine(DelayedSnapshotRoutine());
            }
            EditorGUIUtility.ExitGUI();
        }

        IEnumerator ImportCaptureRoutine(string path)
        {
            m_InportButton.SetEnabled(false);

            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.ImportedSnapshotEvent>();
            string targetPath = null;
            ProgressBarDisplay.ShowBar("Importing snapshot.");
            yield return null;
            bool legacy = m_LegacyReader.IsLegacyFileFormat(path);
            if (legacy)
            {
                float initalProgress = 0.15f;
                ProgressBarDisplay.UpdateProgress(initalProgress, "Reading legacy format file.");
                var oldSnapshot = m_LegacyReader.ReadFromFile(path);
                targetPath = Path.Combine(Application.temporaryCachePath, k_ConvertedSnapshotTempFileName);

                ProgressBarDisplay.UpdateProgress(0.25f, "Converting to current snapshot format.");

                var conversion = LegacyPackedMemorySnapshotConverter.Convert(oldSnapshot, targetPath);
                conversion.MoveNext(); //start execution

                if (conversion.Current == null)
                {
                    ProgressBarDisplay.ClearBar();
                    MemoryProfilerAnalytics.CancelEvent<MemoryProfilerAnalytics.ImportedSnapshotEvent>();
                    m_InportButton.SetEnabled(true);
                    yield break;
                }

                var status = conversion.Current as EnumerationStatus;
                float progressPerStep = (1.0f - initalProgress) / status.StepCount;
                while (conversion.MoveNext())
                {
                    ProgressBarDisplay.UpdateProgress(initalProgress + status.CurrentStep * progressPerStep, status.StepStatus);
                }
            }
            else
            {
                targetPath = path;
            }

            var snapshot = m_MemorySnapshotsCollection.AddSnapshotToCollection(targetPath, legacy ? ImportMode.Move : ImportMode.Copy);
            AddSnapshotToUI(snapshot);

            ProgressBarDisplay.ClearBar();
            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.ImportedSnapshotEvent());

            m_InportButton.SetEnabled(true);
            m_InportButton.MarkDirtyRepaint();
        }

        void ImportCapture()
        {
            string path = EditorUtility.OpenFilePanelWithFilters(Content.ImportSnapshotWindowTitle, MemoryProfilerSettings.LastImportPath, Content.MemorySnapshotImportWindowFileExtensions);
            if (path.Length == 0)
            {
                EditorGUIUtility.ExitGUI();
                return;
            }

            MemoryProfilerSettings.LastImportPath = path;

            this.StartCoroutine(ImportCaptureRoutine(path));
            EditorGUIUtility.ExitGUI();
        }

        void DeleteCapture(SnapshotFileData snapshot)
        {
            if (!EditorUtility.DisplayDialog(Content.DeleteSnapshotDialogTitle, Content.DeleteSnapshotDialogMessage, Content.DeleteSnapshotDialogAccept, Content.DeleteSnapshotDialogCancel))
                return;

            m_OpenSnapshots.CloseCapture(snapshot);
            m_MemorySnapshotsCollection.RemoveSnapshotFromCollection(snapshot);
            m_SnapshotList.Remove(snapshot.GuiData.dynamicVisualElements.snapshotListItem);
            if (m_SnapshotList.childCount <= 0)
            {
                m_ShowEmptySnapshotListHint = true;
                ShowWorkBenchHintText();
            }
            else
            {
                // Even or odd styling is now off, and needs to be recalculated
                for (int i = 0; i < m_SnapshotList.childCount; i++)
                {
                    SetRowStyleOnSnapshotListItem(m_SnapshotList.ElementAt(i), i);
                }
            }
        }

        void RenameCapture(SnapshotFileData snapshot)
        {
            if (CanRenameSnaphot(snapshot))
            {
                snapshot.GuiData.RenamingFieldVisible = true;
#if UNITY_2019_1_OR_NEWER
                EditorApplication.delayCall += () => { snapshot.GuiData.dynamicVisualElements.snapshotRenameField.Q("unity-text-input").Focus(); };
#else
                snapshot.GuiData.dynamicVisualElements.snapshotRenameField.Focus();
#endif
            }
        }

        void BrowseCaptureFolder(SnapshotFileData snapshot)
        {
            EditorUtility.RevealInFinder(snapshot.FileInfo.FullName);
        }

        bool CanRenameSnaphot(SnapshotFileData snapshot)
        {
            if (m_OpenSnapshots.IsSnapshotOpen(snapshot))
            {
                bool close = EditorUtility.DisplayDialog(Content.RenameSnapshotDialogTitle, Content.RenameSnapshotDialogMessage, Content.RenameSnapshotDialogAccept, Content.RenameSnapshotDialogCancel);
                if (close)
                    m_OpenSnapshots.CloseCapture(snapshot);
                return close;
            }
            return true;
        }

        void OpenCapture(SnapshotFileData snapshot)
        {
            try
            {
                MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.LoadedSnapshotEvent>();

                m_OpenSnapshots.OpenSnapshot(snapshot);
                MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.LoadedSnapshotEvent());
            }
            catch (Exception)
            {
                throw;
            }
        }

        void PlaymodeStateChanged(PlayModeStateChange playModeState)
        {
            switch (playModeState)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (m_PlayerConnectionState.connectionName == "Editor")
                        m_CaptureButton.text = "Capture Editor";
                    else
                        m_CaptureButton.text = "Capture Player";
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    m_CaptureButton.text = "Capture Player";
                    break;
                default:
                    break;
            }
        }

        void DrawAttachToPlayerDropdown()
        {
            if (float.IsNaN(m_LeftPane.layout.width) || m_LeftPaneToolbarWidthTakenByUIElements <= 0)
                return;
            float width = m_LeftPane.layout.width - m_LeftPaneToolbarWidthTakenByUIElements;
            if (Content.AttachToPlayerCachedTargetName.text != m_PlayerConnectionState.connectionName)
            {
                PlaymodeStateChanged(Application.isPlaying ? PlayModeStateChange.EnteredPlayMode : PlayModeStateChange.EnteredEditMode);
                Content.AttachToPlayerCachedTargetName.text = m_PlayerConnectionState.connectionName;
            }
            width = Mathf.Min(width, EditorStyles.toolbarDropDown.CalcSize(Content.AttachToPlayerCachedTargetName).x);
            m_AttachToPlayerDropdownHolder.style.minWidth = width;
            m_AttachToPlayerDropdownHolder.style.maxWidth = width;
            var rect = GUILayoutUtility.GetRect(width, m_LeftPaneToolbarHeight);
            rect.x--;
#if UNITY_2020_1_OR_NEWER
            PlayerConnectionGUI.ConnectionTargetSelectionDropdown(rect, m_PlayerConnectionState, EditorStyles.toolbarDropDown);
#else
            ConnectionGUI.AttachToPlayerDropdown(rect, m_PlayerConnectionState, EditorStyles.toolbarDropDown);
#endif
        }

        void DrawTableSelection()
        {
            using (new EditorGUI.DisabledGroupScope(UIState.CurrentMode == null || UIState.CurrentMode.CurrentViewPane == null))
            {
                var dropdownContent = Content.NoneView;
                if (UIState.CurrentMode != null && UIState.CurrentMode.CurrentViewPane != null)
                {
                    var currentViewPane = UIState.CurrentMode.CurrentViewPane;
                    if (currentViewPane is UI.TreeMapPane)
                    {
                        dropdownContent = Content.TreeMapView;
                    }
                    else if (currentViewPane is UI.MemoryMapPane)
                    {
                        dropdownContent = Content.MemoryMapView;
                    }
                    else if (currentViewPane is UI.MemoryMapDiffPane)
                    {
                        dropdownContent = Content.MemoryMapViewDiff;
                    }
                    else if (currentViewPane is UI.SpreadsheetPane)
                    {
                        dropdownContent = ConvertTableNameForUI((currentViewPane as UI.SpreadsheetPane).TableDisplayName, false);
                    }
                }

                var minSize = EditorStyles.toolbarDropDown.CalcSize(dropdownContent);
                minSize.y = Mathf.Min(minSize.y, EditorGUIUtility.singleLineHeight);
                minSize = new Vector2(Mathf.Max(minSize.x, m_ViewDropdownSize.x), Mathf.Max(minSize.y, m_ViewDropdownSize.y));
                Rect viewDropdownRect = GUILayoutUtility.GetRect(minSize.x, minSize.y, Styles.General.ToolbarPopup);
                viewDropdownRect.x--;
#if UNITY_2019_3_OR_NEWER // Hotfixing theming issues...
                viewDropdownRect.y--;
#endif
                if (m_ViewSelectorMenu != null && Event.current.type == EventType.Repaint)
                {
                    m_ViewSelectorMenu.style.width = minSize.x;
                    m_ViewSelectorMenu.style.height = minSize.y;
                }
                if (EditorGUI.DropdownButton(viewDropdownRect, dropdownContent, FocusType.Passive, Styles.General.ToolbarPopup))
                {
                    int curTableIndex = -1;
                    if (currentViewPane is UI.SpreadsheetPane)
                    {
                        UI.SpreadsheetPane pane = (UI.SpreadsheetPane)currentViewPane;
                        curTableIndex = pane.CurrentTableIndex;
                    }

                    GenericMenu menu = new GenericMenu();

                    if (UIState.CurrentMode is UI.UIState.SnapshotMode)
                    {
                        menu.AddItem(Content.TreeMapView, UIState.CurrentMode.CurrentViewPane is UI.TreeMapPane, () =>
                        {
                            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.OpenedViewEvent>();
                            AddCurrentHistoryEvent();
                            OpenTreeMap(null);
                            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.OpenedViewEvent() { viewName = "TreeMap" });
                        });
                        menu.AddItem(Content.MemoryMapView, UIState.CurrentMode.CurrentViewPane is UI.MemoryMapPane, () =>
                        {
                            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.OpenedViewEvent>();
                            AddCurrentHistoryEvent();
                            OpenMemoryMap(null);
                            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.OpenedViewEvent() { viewName = "MemoryMap" });
                        });
                    }
                    else
                    {
                        menu.AddDisabledItem(Content.TreeMapView);
                        menu.AddItem(Content.MemoryMapViewDiff, UIState.CurrentMode.CurrentViewPane is UI.MemoryMapDiffPane, () =>
                        {
                            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.OpenedViewEvent>();
                            AddCurrentHistoryEvent();
                            OpenMemoryMapDiff(null);
                            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.OpenedViewEvent() { viewName = "MemoryMapDiff" });
                        });
                    }

                    if (UIState.CurrentMode != null)
                    {
                        // skip "none"
                        int numberOfTabelsToSkip = 1;

                        for (int i = numberOfTabelsToSkip; i < UIState.CurrentMode.TableNames.Length; i++)
                        {
                            int newTableIndex = i;
                            menu.AddItem(ConvertTableNameForUI(UIState.CurrentMode.TableNames[i]), newTableIndex == curTableIndex, () =>
                            {
                                ProgressBarDisplay.ShowBar("Opening Table...");
                                try
                                {
                                    MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.OpenedViewEvent>();
                                    var tab = UIState.CurrentMode.GetTableByIndex(newTableIndex - numberOfTabelsToSkip);

                                    ProgressBarDisplay.UpdateProgress(0.0f, "Updating Table...");
                                    if (tab.Update())
                                    {
                                        UIState.CurrentMode.UpdateTableSelectionNames();
                                    }
                                    AddCurrentHistoryEvent();

                                    ProgressBarDisplay.UpdateProgress(0.75f, "Opening Table...");
                                    OpenTable(new Database.TableReference(tab.GetName()), tab);

                                    MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.OpenedViewEvent() { viewName = tab.GetDisplayName() });
                                    ProgressBarDisplay.UpdateProgress(1.0f, "");
                                }
                                finally
                                {
                                    ProgressBarDisplay.ClearBar();
                                }
                            });
                        }
                    }

                    menu.DropDown(viewDropdownRect);
                }
            }
        }

        private GUIContent ConvertTableNameForUI(string tableName, bool fullPath = true)
        {
            if (!m_UIFriendlyViewOptionNames.ContainsKey(tableName))
            {
                m_TabelNameStringBuilder.Length = 0;
                m_TabelNameStringBuilder.Append(Content.TableMapViewRoot.text);
                m_TabelNameStringBuilder.Append(tableName);
                if (tableName.StartsWith(k_DiffRawCategoryName))
                    m_TabelNameStringBuilder.Replace(k_DiffRawCategoryName, Content.DiffRawDataTableMapViewRoot.text, Content.TableMapViewRoot.text.Length, k_DiffRawCategoryName.Length);
                else
                    m_TabelNameStringBuilder.Replace(k_RawCategoryName, Content.RawDataTableMapViewRoot.text, Content.TableMapViewRoot.text.Length, k_RawCategoryName.Length);
                string name = m_TabelNameStringBuilder.ToString();
                m_UIFriendlyViewOptionNamesWithFullPath[tableName] = new GUIContent(name);

                int lastSlash = name.LastIndexOf('/');
                if (lastSlash >= 0 && lastSlash + 1 < name.Length)
                    name = name.Substring(lastSlash + 1);
                m_UIFriendlyViewOptionNames[tableName] = new GUIContent(name);

                Vector2 potentialViewDropdownSize = Styles.General.ToolbarPopup.CalcSize(m_UIFriendlyViewOptionNames[tableName]);
                potentialViewDropdownSize.x = Mathf.Clamp(potentialViewDropdownSize.x, 100, 300);
                if (m_ViewDropdownSize.x < potentialViewDropdownSize.x)
                {
                    m_ViewDropdownSize = potentialViewDropdownSize;
                }
            }

            return fullPath ? m_UIFriendlyViewOptionNamesWithFullPath[tableName] : m_UIFriendlyViewOptionNames[tableName];
        }

        void ClearCurrentlyOpenedCapturesUIData()
        {
        }

        IEnumerator DelayedHistoryEvent(HistoryEvent eventToOpen)
        {
            yield return null;
            try
            {
                if (eventToOpen != null)
                {
                    OpenHistoryEvent(eventToOpen);
                    eventToOpen = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
