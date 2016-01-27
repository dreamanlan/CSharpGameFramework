//#define BRT_SHOW_MINOR_WARNINGS

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;


public partial class BRT_BuildReportWindow : EditorWindow
{
	public static string GetValueMessage { set; get; }

	public static bool LoadingValuesFromThread { get{ return !string.IsNullOrEmpty(GetValueMessage); } }


	[SerializeField]
	static BuildReportTool.BuildInfo _buildInfo;


	GUISkin _usedSkin = null;

	Vector2 _assetListScrollPos;

	public static bool IsOpen { get; set; }

	Vector2 _readmeScrollPos;
	string _readmeContents;
	float _readmeHeight;

	Vector2 _changelogScrollPos;
	string _changelogContents;
	float _changelogHeight;



	void RecategorizeDisplayedBuildInfo()
	{
		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			BuildReportTool.ReportManager.RecategorizeAssetList(_buildInfo);
		}
	}

	void OnDisable()
	{
		ForceStopFileLoadThread();
		IsOpen = false;
	}

	void OnFocus()
	{
		RefreshConfiguredFileFilters();

		// check if configured file filters changed and only then do we need to recategorize

		if (BuildReportTool.Options.ShouldUseConfiguredFileFilters())
		{
			RecategorizeDisplayedBuildInfo();
		}
	}

	void OnEnable()
	{
		//Debug.Log("BuildReportWindow.OnEnable() " + System.DateTime.Now);

		_saveTypeLabels = new string[] {SAVE_PATH_TYPE_PERSONAL_OS_SPECIFIC_LABEL, SAVE_PATH_TYPE_PROJECT_LABEL};

		_selectedAutoShowWindowIdx = GetAutoShowWindowTypeGuiIdxFromOptions();
		_selectedCalculationLevelIdx = GetCalculationLevelGuiIdxFromOptions();
		IsOpen = true;

		InitGUISkin();
		InitHelpContents();

		RefreshConfiguredFileFilters();

		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			//Debug.Log("recompiled " + _buildInfo.SavedPath);
			if (!string.IsNullOrEmpty(_buildInfo.SavedPath))
			{
				BuildReportTool.BuildInfo loadedBuild = BuildReportTool.Util.OpenSerializedBuildInfo(_buildInfo.SavedPath);
				if (BuildReportTool.Util.BuildInfoHasContents(loadedBuild))
				{
					_buildInfo = loadedBuild;
					RefreshConfiguredFileFilters();
				}
			}
			else
			{
				_buildInfo.UsedAssets.AssignPerCategoryList( BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(_buildInfo.UsedAssets.All, _buildInfo.FileFilters) );
				_buildInfo.UnusedAssets.AssignPerCategoryList( BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(_buildInfo.UnusedAssets.All, _buildInfo.FileFilters) );
			}
		}
	}

	void InitGUISkin()
	{
		string guiSkinToUse = DEFAULT_GUI_SKIN_FILENAME;
		if (EditorGUIUtility.isProSkin)
		{
			guiSkinToUse = DARK_GUI_SKIN_FILENAME;
		}

		// try default path
		_usedSkin = AssetDatabase.LoadAssetAtPath(BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_PATH + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;

		if (_usedSkin == null)
		{
#if BRT_SHOW_MINOR_WARNINGS
			Debug.LogWarning(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MOVED_MSG);
#endif

			string folderPath = BuildReportTool.Util.FindAssetFolder(Application.dataPath, BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);
			if (!string.IsNullOrEmpty(folderPath))
			{
				folderPath = folderPath.Replace('\\', '/');
				int assetsIdx = folderPath.IndexOf("/Assets/");
				if (assetsIdx != -1)
				{
					folderPath = folderPath.Substring(assetsIdx+8, folderPath.Length-assetsIdx-8);
				}
				//Debug.Log(folderPath);

				_usedSkin = AssetDatabase.LoadAssetAtPath("Assets/" + folderPath + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;
			}
			else
			{
				Debug.LogError(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MISSING_MSG);
			}
			//Debug.Log("_usedSkin " + (_usedSkin != null));
		}
	}


	const string HELP_CONTENT_GUI_STYLE = "label";
	const int HELP_CONTENT_WIDTH = 500;

	void InitHelpContents()
	{
		const string README_FILENAME = "README.txt";
		_readmeContents = BuildReportTool.Util.GetPackageFileContents(README_FILENAME);

		const string CHANGELOG_FILENAME = "VERSION.txt";
		_changelogContents = BuildReportTool.Util.GetPackageFileContents(CHANGELOG_FILENAME);
	}




	BuildReportTool.FileFilterGroup _configuredFileFilterGroup = null;

	void RefreshConfiguredFileFilters()
	{
		// reload used FileFilterGroup but save current used filter idx
		// to be re-set afterwards

		int tempIdx = 0;

		if (_configuredFileFilterGroup != null)
		{
			tempIdx = _configuredFileFilterGroup.GetSelectedFilterIdx();
		}

		_configuredFileFilterGroup = BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUse();

		_configuredFileFilterGroup.ForceSetSelectedFilterIdx(tempIdx);
	}

	BuildReportTool.FileFilterGroup FileFilterGroupToUse
	{
		get
		{
			if (BuildReportTool.Options.ShouldUseConfiguredFileFilters())
			{
				return _configuredFileFilterGroup;
			}
			return _buildInfo.FileFilters;
		}
	}

	public void Init(BuildReportTool.BuildInfo buildInfo)
	{
		_buildInfo = buildInfo;
	}

	void Refresh()
	{
		BuildReportTool.ReportManager.RefreshData(ref _buildInfo);
	}

	bool IsWaitingForBuildCompletionToGenerateBuildReport
	{
		get
		{
			return BuildReportTool.Util.ShouldGetBuildReportNow && EditorApplication.isCompiling;
		}
	}

	void OnFinishOpeningBuildReportFile()
	{
		_finishedOpeningFromThread = false;

		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			RefreshConfiguredFileFilters();
			_buildInfo.OnDeserialize();
			_buildInfo.SetSavedPath(_lastOpenedBuildInfoFilePath);
		}
		Repaint();
		GoToOverviewScreen();
	}

	void Update()
	{
		if (_buildInfo != null && BuildReportTool.ReportManager.IsFinishedGettingValuesFromThread)
		{
			BuildReportTool.ReportManager.OnFinishedGetValues(_buildInfo);
			_buildInfo.UnescapeAssetNames();
			GoToOverviewScreen();
		}

		if (BuildReportTool.Util.ShouldGetBuildReportNow && !BuildReportTool.ReportManager.IsGettingValuesFromThread && !EditorApplication.isCompiling)
		{
			//Debug.Log("BuildReportWindow getting build info right after the build... " + System.DateTime.Now);
			Refresh();
			GoToOverviewScreen();
		}

		if (_finishedOpeningFromThread)
		{
			OnFinishOpeningBuildReportFile();
		}

		if (_buildInfo != null)
		{
			if (_buildInfo.RequestedToRefresh)
			{
				Repaint();
				_buildInfo.FlagFinishedRefreshing();
			}
		}
	}


	void GoToOverviewScreen()
	{
		_selectedCategoryIdx = OVERVIEW_IDX;
	}


	void DrawNames(BuildReportTool.SizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildReportTool.SizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Name, styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}
	void DrawReadableSizes(BuildReportTool.SizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildReportTool.SizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Size, styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}
	void DrawPercentages(BuildReportTool.SizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildReportTool.SizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Percentage + "%", styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}


	void DrawLargeSizeDisplay(string label, string desc, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}

		GUILayout.BeginVertical();
			GUILayout.Label(label, INFO_TITLE_STYLE_NAME);
			GUILayout.Label(desc, TINY_HELP_STYLE_NAME);
			GUILayout.Label(value, BIG_NUMBER_STYLE_NAME);
		GUILayout.EndVertical();
	}


	void DrawTotalSize()
	{
		GUILayout.BeginVertical();

		GUILayout.Label(TIME_OF_BUILD_LABEL, INFO_TITLE_STYLE_NAME);
		GUILayout.Label(_buildInfo.GetTimeReadable(), INFO_SUBTITLE_STYLE_NAME);

		GUILayout.Space(40);


		if (_buildInfo.HasOldSizeValues)
		{
			// in old sizes:
			// TotalBuildSize is really the used assets size
			// CompressedBuildSize if present is the total build size

			DrawLargeSizeDisplay(USED_TOTAL_SIZE_LABEL, USED_TOTAL_SIZE_DESC, _buildInfo.TotalBuildSize);
			GUILayout.Space(40);
			DrawLargeSizeDisplay(BUILD_TOTAL_SIZE_LABEL, GetProperBuildSizeDesc(), _buildInfo.CompressedBuildSize);
		}
		else
		{

			// Unused Assets
			if (_buildInfo.UnusedAssetsIncludedInCreation)
			{
				DrawLargeSizeDisplay(UNUSED_TOTAL_SIZE_LABEL, UNUSED_TOTAL_SIZE_DESC, _buildInfo.UnusedTotalSize);
				GUILayout.Space(40);
			}


			// Used Assets
			DrawLargeSizeDisplay(USED_TOTAL_SIZE_LABEL, USED_TOTAL_SIZE_DESC, _buildInfo.UsedTotalSize);
			GUILayout.Space(40);


			// Streaming Assets
			if (_buildInfo.StreamingAssetsSize != "0 B")
			{
				DrawLargeSizeDisplay(STREAMING_ASSETS_TOTAL_SIZE_LABEL, STREAMING_ASSETS_SIZE_DESC, _buildInfo.StreamingAssetsSize);
				GUILayout.Space(40);
			}


			// Total Build Size
			DrawLargeSizeDisplay(BUILD_TOTAL_SIZE_LABEL, GetProperBuildSizeDesc(), _buildInfo.TotalBuildSize);


			DrawAuxiliaryBuildSizes();

		}

		GUILayout.EndVertical();
	}

	void DrawAuxiliaryBuildSizes()
	{
		BuildReportTool.BuildPlatform buildPlatform = BuildReportTool.ReportManager.GetBuildPlatformFromString(_buildInfo.BuildType, _buildInfo.BuildTargetUsed);

		if (buildPlatform == BuildReportTool.BuildPlatform.Web)
		{
			GUILayout.Space(20);
			GUILayout.BeginVertical();
				GUILayout.Label(WEB_UNITY3D_FILE_SIZE_LABEL, INFO_SUBTITLE_BOLD_STYLE_NAME);
				GUILayout.Label(_buildInfo.WebFileBuildSize, BIG_NUMBER_STYLE_NAME);
			GUILayout.EndVertical();
		}
		else if (buildPlatform == BuildReportTool.BuildPlatform.Android)
		{
			if (!_buildInfo.AndroidCreateProject && _buildInfo.AndroidUseAPKExpansionFiles)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
					GUILayout.Label(ANDROID_APK_FILE_SIZE_LABEL, INFO_SUBTITLE_BOLD_STYLE_NAME);
					GUILayout.Label(_buildInfo.AndroidApkFileBuildSize, INFO_TITLE_STYLE_NAME);
				GUILayout.EndVertical();

				GUILayout.Space(20);
				GUILayout.BeginVertical();
					GUILayout.Label(ANDROID_OBB_FILE_SIZE_LABEL, INFO_SUBTITLE_BOLD_STYLE_NAME);
					GUILayout.Label(_buildInfo.AndroidObbFileBuildSize, INFO_TITLE_STYLE_NAME);
				GUILayout.EndVertical();
			}
			else if (_buildInfo.AndroidCreateProject && _buildInfo.AndroidUseAPKExpansionFiles)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
					GUILayout.Label(ANDROID_OBB_FILE_SIZE_LABEL, INFO_SUBTITLE_BOLD_STYLE_NAME);
					GUILayout.Label(_buildInfo.AndroidObbFileBuildSize, INFO_TITLE_STYLE_NAME);
				GUILayout.EndVertical();
			}
		}
	}

	string GetProperBuildSizeDesc()
	{
		BuildReportTool.BuildPlatform buildPlatform = BuildReportTool.ReportManager.GetBuildPlatformFromString(_buildInfo.BuildType, _buildInfo.BuildTargetUsed);

		switch (buildPlatform)
		{
			case BuildReportTool.BuildPlatform.MacOSX32:
				return BUILD_SIZE_MACOSX_DESC;
			case BuildReportTool.BuildPlatform.MacOSX64:
				return BUILD_SIZE_MACOSX_DESC;
			case BuildReportTool.BuildPlatform.MacOSXUniversal:
				return BUILD_SIZE_MACOSX_DESC;

			case BuildReportTool.BuildPlatform.Windows32:
				return BUILD_SIZE_WINDOWS_DESC;
			case BuildReportTool.BuildPlatform.Windows64:
				return BUILD_SIZE_WINDOWS_DESC;

			case BuildReportTool.BuildPlatform.Linux32:
				return BUILD_SIZE_STANDALONE_DESC;
			case BuildReportTool.BuildPlatform.Linux64:
				return BUILD_SIZE_STANDALONE_DESC;
			case BuildReportTool.BuildPlatform.LinuxUniversal:
				return BUILD_SIZE_LINUX_UNIVERSAL_DESC;

			case BuildReportTool.BuildPlatform.Android:
				if (_buildInfo.AndroidCreateProject)
				{
					return BUILD_SIZE_ANDROID_WITH_PROJECT_DESC;
				}
				if (_buildInfo.AndroidUseAPKExpansionFiles)
				{
					return BUILD_SIZE_ANDROID_WITH_OBB_DESC;
				}
				return BUILD_SIZE_ANDROID_DESC;

			case BuildReportTool.BuildPlatform.iOS:
				return BUILD_SIZE_IOS_DESC;

			case BuildReportTool.BuildPlatform.Web:
				return BUILD_SIZE_WEB_DESC;
		}
		return "";
	}

	void DrawBuildSizes()
	{
		if (!string.IsNullOrEmpty(_buildInfo.CompressedBuildSize))
		{
			GUILayout.BeginVertical();
		}

		GUILayout.Label(TOTAL_SIZE_BREAKDOWN_LABEL, INFO_TITLE_STYLE_NAME);

		if (!string.IsNullOrEmpty(_buildInfo.CompressedBuildSize))
		{
			GUILayout.BeginHorizontal();
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_PRE_BOLD, INFO_SUBTITLE_STYLE_NAME);
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_BOLD, INFO_SUBTITLE_BOLD_STYLE_NAME);
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_POST_BOLD, INFO_SUBTITLE_STYLE_NAME);
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}

		if (_buildInfo.BuildSizes != null)
		{
			GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
			DrawNames(_buildInfo.BuildSizes);
			DrawReadableSizes(_buildInfo.BuildSizes);
			DrawPercentages(_buildInfo.BuildSizes);
			GUILayout.EndHorizontal();
		}
	}

	void DrawDLLList()
	{
		GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
				GUILayout.Label(MONO_DLLS_LABEL, INFO_TITLE_STYLE_NAME);
				{
					GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
						DrawNames(_buildInfo.MonoDLLs);
						DrawReadableSizes(_buildInfo.MonoDLLs);
					GUILayout.EndHorizontal();
				}
			GUILayout.EndVertical();

			GUILayout.Space(15);

			GUILayout.BeginVertical();
				GUILayout.Label(SCRIPT_DLLS_LABEL, INFO_TITLE_STYLE_NAME);
				{
					GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
						DrawNames(_buildInfo.ScriptDLLs);
						DrawReadableSizes(_buildInfo.ScriptDLLs);
					GUILayout.EndHorizontal();
				}
			GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}

	void PingAssetInProject(string file)
	{
		if (!file.StartsWith("Assets/"))
		{
			return;
		}

		// thanks to http://answers.unity3d.com/questions/37180/how-to-highlight-or-select-an-asset-in-project-win.html
		var asset = AssetDatabase.LoadMainAssetAtPath(file);
		if (asset != null)
		{
			GUI.skin = null;

			//EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(file, typeof(Object)));
			EditorGUIUtility.PingObject(asset);
			Selection.activeObject = asset;

			GUI.skin = _usedSkin;
		}
	}

	void DrawAssetList(BuildReportTool.AssetList list, BuildReportTool.FileFilterGroup filter, int length)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_PRE_BOLD, INFO_SUBTITLE_STYLE_NAME);
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_BOLD, INFO_SUBTITLE_BOLD_STYLE_NAME);
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_POST_BOLD, INFO_SUBTITLE_STYLE_NAME);
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		if (list != null)
		{
			BuildReportTool.SizePart[] assetListToUse = list.GetListToDisplay(filter);

			if (assetListToUse != null)
			{
				if (assetListToUse.Length == 0)
				{
					GUILayout.Label(NO_FILES_FOR_THIS_CATEGORY, INFO_TITLE_STYLE_NAME);
				}
				else
				{
					const int LIST_HEIGHT = 20;
					const int ICON_DISPLAY_SIZE = 15;
					EditorGUIUtility.SetIconSize(Vector2.one * ICON_DISPLAY_SIZE);
					bool useAlt = false;

					int viewOffset = list.GetViewOffsetForDisplayedList(filter);

					// if somehow view offset was out of bounds of the SizePart[] array
					// reset it to zero
					if (viewOffset >= assetListToUse.Length)
					{
						list.SetViewOffsetForDisplayedList(filter, 0);
						viewOffset = 0;
					}

					int len = Mathf.Min(viewOffset + length, assetListToUse.Length);

					GUILayout.BeginHorizontal();

					GUILayout.BeginVertical();
						useAlt = false;
						for (int n = viewOffset; n < len; ++n)
						{
							BuildReportTool.SizePart b = assetListToUse[n];

							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							bool inSumSelect = list.InSumSelection(b);
							if (inSumSelect)
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							GUILayout.BeginHorizontal();
								bool newInSumSelect = GUILayout.Toggle(inSumSelect, "");
								if (inSumSelect != newInSumSelect)
								{
									if (newInSumSelect)
									{
										list.AddToSumSelection(b);
									}
									else
									{
										list.RemoveFromSumSelection(b);
									}
								}

								Texture icon = AssetDatabase.GetCachedIcon(b.Name);
								if (GUILayout.Button(new GUIContent((n+1) + ". " + b.Name, icon), styleToUse, GUILayout.Height(LIST_HEIGHT)))
								{
									PingAssetInProject(b.Name);
								}
							GUILayout.EndHorizontal();

							useAlt = !useAlt;
						}
					GUILayout.EndVertical();

					GUILayout.BeginVertical();
						useAlt = false;
						for (int n = viewOffset; n < len; ++n)
						{
							BuildReportTool.SizePart b = assetListToUse[n];

							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							if (list.InSumSelection(b))
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							GUILayout.Label(b.Size, styleToUse, GUILayout.MinWidth(50), GUILayout.Height(LIST_HEIGHT));
							useAlt = !useAlt;
						}
					GUILayout.EndVertical();

					GUILayout.BeginVertical();
						useAlt = false;
						for (int n = viewOffset; n < len; ++n)
						{
							BuildReportTool.SizePart b = assetListToUse[n];

							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							if (list.InSumSelection(b))
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							string text = b.Percentage + "%";
							if (b.Percentage < 0)
							{
								text = NON_APPLICABLE_PERCENTAGE;
							}

							GUILayout.Label(text, styleToUse, GUILayout.MinWidth(30), GUILayout.Height(LIST_HEIGHT));
							useAlt = !useAlt;
						}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}
		}
	}


	void DrawOverviewScreen()
	{
		GUILayout.Space(10); // extra top padding

		GUILayout.BeginHorizontal();
			GUILayout.Space(10); // extra left padding
			DrawTotalSize();
			GUILayout.Space(CATEGORY_HORIZONTAL_SPACING);
			GUILayout.BeginVertical();
				DrawBuildSizes();
				GUILayout.Space(CATEGORY_VERTICAL_SPACING);
				DrawDLLList();
			GUILayout.EndVertical();
			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();
	}

	string[] _fileFilterDisplayTypeLabels = new string[] {FILE_FILTER_DISPLAY_TYPE_DROP_DOWN_LABEL, FILE_FILTER_DISPLAY_TYPE_BUTTONS_LABEL};

	string[] _saveTypeLabels = null;

	string[] _fileFilterToUseType = new string[] {FILTER_GROUP_TO_USE_CONFIGURED_LABEL, FILTER_GROUP_TO_USE_EMBEDDED_LABEL};

	string OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL
	{
		get
		{
			if (BuildReportTool.Util.IsInWinOS)
				return OPEN_IN_FILE_BROWSER_WIN_LABEL;
			if (BuildReportTool.Util.IsInMacOS)
				return OPEN_IN_FILE_BROWSER_MAC_LABEL;

			return OPEN_IN_FILE_BROWSER_DEFAULT_LABEL;
		}
	}

	string SAVE_PATH_TYPE_PERSONAL_OS_SPECIFIC_LABEL
	{
		get
		{
			if (BuildReportTool.Util.IsInWinOS)
				return SAVE_PATH_TYPE_PERSONAL_WIN_LABEL;
			if (BuildReportTool.Util.IsInMacOS)
				return SAVE_PATH_TYPE_PERSONAL_MAC_LABEL;

			return SAVE_PATH_TYPE_PERSONAL_DEFAULT_LABEL;
		}
	}




	string[] _calculationTypeLabels = new string[] {
		CALCULATION_LEVEL_FULL_NAME,
		CALCULATION_LEVEL_NO_PREFAB_NAME,
		CALCULATION_LEVEL_NO_UNUSED_NAME,
		CALCULATION_LEVEL_MINIMAL_NAME};

	int _selectedCalculationLevelIdx = 0;

	string CalculationLevelDescription
	{
		get
		{
			switch (_selectedCalculationLevelIdx)
			{
				case 0:
					return CALCULATION_LEVEL_FULL_DESC;
				case 1:
					return CALCULATION_LEVEL_NO_PREFAB_DESC;
				case 2:
					return CALCULATION_LEVEL_NO_UNUSED_DESC;
				case 3:
					return CALCULATION_LEVEL_MINIMAL_DESC;
			}
			return "";
		}
	}

	int GetCalculationLevelGuiIdxFromOptions()
	{
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtFull)
		{
			return 0;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtNoUnusedPrefabs)
		{
			return 1;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtNoUnusedAssets)
		{
			return 2;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtOverviewOnly)
		{
			return 3;
		}
		return 0;
	}

	void SetCalculationLevelFromGuiIdx(int selectedIdx)
	{
		switch (selectedIdx)
		{
			case 0:
				BuildReportTool.Options.SetCalculationLevelToFull();
				break;
			case 1:
				BuildReportTool.Options.SetCalculationLevelToNoUnusedPrefabs();
				break;
			case 2:
				BuildReportTool.Options.SetCalculationLevelToNoUnusedAssets();
				break;
			case 3:
				BuildReportTool.Options.SetCalculationLevelToOverviewOnly();
				break;
		}
	}






	string[] _autoShowWindowLabels = new string[] {
		"Never",
		"Always",
		"Yes, but not during batchmode"};

	int _selectedAutoShowWindowIdx = 2;


	int GetAutoShowWindowTypeGuiIdxFromOptions()
	{
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToNever)
		{
			return 0;
		}
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToAlways)
		{
			return 1;
		}
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToNotInBatchMode)
		{
			return 2;
		}
		return 2;
	}

	void SetAutoShowWindowTypeFromGuiIdx(int selectedIdx)
	{
		switch (selectedIdx)
		{
			case 0: // never
				BuildReportTool.Options.SetAutoShowWindowTypeToNever();
				break;
			case 1: // always
				BuildReportTool.Options.SetAutoShowWindowTypeToAlways();
				break;
			case 2: // yes, but not during batchmode
				BuildReportTool.Options.SetAutoShowWindowTypeToNotInBatchMode();
				break;
		}
	}





	int _fileFilterGroupToUseOnOpeningOptionsWindow = 0;
	int _fileFilterGroupToUseOnClosingOptionsWindow = 0;

	void DrawOptionsScreen()
	{
		GUILayout.Space(10); // extra top padding

		GUILayout.BeginHorizontal();
			GUILayout.Space(20); // extra left padding
			GUILayout.BeginVertical();

				// === Main Options ===

				GUILayout.Label("Main Options", INFO_TITLE_STYLE_NAME);

				BuildReportTool.Options.CollectBuildInfo = GUILayout.Toggle(BuildReportTool.Options.CollectBuildInfo, COLLECT_BUILD_INFO_LABEL);
				BuildReportTool.Options.AllowDeletingOfUsedAssets = GUILayout.Toggle(BuildReportTool.Options.AllowDeletingOfUsedAssets, "Allow deleting of Used Assets (practice caution!)");




				GUILayout.Space(10);

				GUILayout.BeginHorizontal();
					GUILayout.Label("Automatically show Build Report Window after building: ");

					GUILayout.BeginVertical();
						int newSelectedAutoShowWindowIdx = EditorGUILayout.Popup(_selectedAutoShowWindowIdx, _autoShowWindowLabels, "Popup", GUILayout.Width(300));
					GUILayout.EndVertical();

					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();


				if (newSelectedAutoShowWindowIdx != _selectedAutoShowWindowIdx)
				{
					_selectedAutoShowWindowIdx = newSelectedAutoShowWindowIdx;
					SetAutoShowWindowTypeFromGuiIdx(newSelectedAutoShowWindowIdx);
				}



				GUILayout.Space(10);

				GUILayout.BeginHorizontal();
					GUILayout.Label("Calculation Level: ");

					GUILayout.BeginVertical();
						int newSelectedCalculationLevelIdx = EditorGUILayout.Popup(_selectedCalculationLevelIdx, _calculationTypeLabels, "Popup", GUILayout.Width(300));
						GUILayout.BeginHorizontal();
							GUILayout.Space(20);
							GUILayout.Label(CalculationLevelDescription, GUILayout.MaxWidth(500), GUILayout.MinHeight(75));
						GUILayout.EndHorizontal();
					GUILayout.EndVertical();

					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);

				if (newSelectedCalculationLevelIdx != _selectedCalculationLevelIdx)
				{
					_selectedCalculationLevelIdx = newSelectedCalculationLevelIdx;
					SetCalculationLevelFromGuiIdx(newSelectedCalculationLevelIdx);
				}


				// === Editor Log File ===

				GUILayout.Label("Editor Log File", INFO_TITLE_STYLE_NAME);

				// which Editor.log is used
				GUILayout.BeginHorizontal();
					GUILayout.Label(EDITOR_LOG_LABEL + BuildReportTool.Util.EditorLogPathOverrideMessage + ": " + BuildReportTool.Util.UsedEditorLogPath);
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL) && BuildReportTool.Util.UsedEditorLogExists)
					{
						BuildReportTool.Util.OpenInFileBrowser(BuildReportTool.Util.UsedEditorLogPath);
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				if (!BuildReportTool.Util.UsedEditorLogExists)
				{
					GUILayout.Label(EDITOR_LOG_INVALID_MSG);
				}

				// override which log is opened
				GUILayout.BeginHorizontal();
					if (GUILayout.Button(SET_OVERRIDE_LOG_LABEL))
					{
						string filepath = EditorUtility.OpenFilePanel(
							"", // title
							"", // default path
							""); // file type (only one type allowed?)

						if (!string.IsNullOrEmpty(filepath))
						{
							BuildReportTool.Options.EditorLogOverridePath = filepath;
						}
					}
					if (GUILayout.Button(CLEAR_OVERRIDE_LOG_LABEL))
					{
						BuildReportTool.Options.EditorLogOverridePath = "";
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);




				// === Asset Lists ===

				GUILayout.Label("Asset Lists", INFO_TITLE_STYLE_NAME);

				BuildReportTool.Options.IncludeSvnInUnused = GUILayout.Toggle(BuildReportTool.Options.IncludeSvnInUnused, INCLUDE_SVN_LABEL);
				BuildReportTool.Options.IncludeGitInUnused = GUILayout.Toggle(BuildReportTool.Options.IncludeGitInUnused, INCLUDE_GIT_LABEL);

				GUILayout.Space(10);

				// pagination length
				GUILayout.BeginHorizontal();
					GUILayout.Label("View assets per groups of:");
					string pageInput = GUILayout.TextField(BuildReportTool.Options.AssetListPaginationLength.ToString(), GUILayout.MinWidth(100));
					pageInput = Regex.Replace(pageInput, @"[^0-9]", ""); // positive numbers only, no fractions
					if (string.IsNullOrEmpty(pageInput))
					{
						pageInput = "0";
					}
					BuildReportTool.Options.AssetListPaginationLength = int.Parse(pageInput);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(10);

				// unused assets entries per batch
				GUILayout.BeginHorizontal();
					GUILayout.Label("Process unused assets per batches of:");
					string entriesPerBatchInput = GUILayout.TextField(BuildReportTool.Options.UnusedAssetsEntriesPerBatch.ToString(), GUILayout.MinWidth(100));
					entriesPerBatchInput = Regex.Replace(entriesPerBatchInput, @"[^0-9]", ""); // positive numbers only, no fractions
					if (string.IsNullOrEmpty(entriesPerBatchInput))
					{
						entriesPerBatchInput = "0";
					}
					BuildReportTool.Options.UnusedAssetsEntriesPerBatch = int.Parse(entriesPerBatchInput);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(10);

				// file filter display type (dropdown or buttons)
				GUILayout.BeginHorizontal();
					GUILayout.Label(FILE_FILTER_DISPLAY_TYPE_LABEL);
					BuildReportTool.Options.FileFilterDisplayInt = GUILayout.SelectionGrid(BuildReportTool.Options.FileFilterDisplayInt, _fileFilterDisplayTypeLabels, _fileFilterDisplayTypeLabels.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(10);

				// choose which file filter group to use
				GUILayout.BeginHorizontal();
					GUILayout.Label(FILTER_GROUP_TO_USE_LABEL);
					BuildReportTool.Options.FilterToUseInt = GUILayout.SelectionGrid(BuildReportTool.Options.FilterToUseInt, _fileFilterToUseType, _fileFilterToUseType.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// display which file filter group is used
				GUILayout.BeginHorizontal();
					GUILayout.Label(FILTER_GROUP_FILE_PATH_LABEL + BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUseFilePath()); // display path to used file filter
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL))
					{
						BuildReportTool.Util.OpenInFileBrowser( BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUseFilePath() );
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);




				// === Build Report Files ===

				GUILayout.Label("Build Report Files", INFO_TITLE_STYLE_NAME);

				// build report files save path
				GUILayout.BeginHorizontal();
					GUILayout.Label(SAVE_PATH_LABEL + BuildReportTool.Options.BuildReportSavePath);
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL))
					{
						BuildReportTool.Util.OpenInFileBrowser( BuildReportTool.Options.BuildReportSavePath );
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// change name of build reports folder
				GUILayout.BeginHorizontal();
					GUILayout.Label(SAVE_FOLDER_NAME_LABEL);
					BuildReportTool.Options.BuildReportFolderName = GUILayout.TextField(BuildReportTool.Options.BuildReportFolderName, GUILayout.MinWidth(250));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// where to save build reports (my docs/home, or beside project)
				GUILayout.BeginHorizontal();
					GUILayout.Label(SAVE_PATH_TYPE_LABEL);
					BuildReportTool.Options.SaveType = GUILayout.SelectionGrid(BuildReportTool.Options.SaveType, _saveTypeLabels, _saveTypeLabels.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);


			GUILayout.EndVertical();
			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();

		if (BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PERSONAL)
		{
			// changed to user's personal folder
			BuildReportTool.ReportManager.ChangeSavePathToUserPersonalFolder();
		}
		else if (BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PROJECT)
		{
			// changed to project folder
			BuildReportTool.ReportManager.ChangeSavePathToProjectFolder();
		}
	}


	int _selectedHelpContentsIdx = 0;
	string[] _helpTypeLabels = new string[] {"Help (README)", "Version Changelog"};
	const int HELP_TYPE_README_IDX = 0;
	const int HELP_TYPE_CHANGELOG_IDX = 1;

	void DrawHelpScreen()
	{
		GUI.SetNextControlName("BRT_HelpUnfocuser");
		GUI.TextField(new Rect(-100, -100, 10, 10), "");

		GUILayout.Space(10); // extra top padding

		GUILayout.BeginHorizontal();
		int newSelectedHelpIdx = GUILayout.SelectionGrid(_selectedHelpContentsIdx, _helpTypeLabels, 1);

		if (newSelectedHelpIdx != _selectedHelpContentsIdx)
		{
			GUI.FocusControl("BRT_HelpUnfocuser");
		}

		_selectedHelpContentsIdx = newSelectedHelpIdx;

			//GUILayout.Space((position.width - HELP_CONTENT_WIDTH) * 0.5f);

				if (_selectedHelpContentsIdx == HELP_TYPE_README_IDX)
				{
					_readmeScrollPos = GUILayout.BeginScrollView(
						_readmeScrollPos);

						float readmeHeight = _usedSkin.GetStyle(HELP_CONTENT_GUI_STYLE).CalcHeight(new GUIContent(_readmeContents), HELP_CONTENT_WIDTH);

						EditorGUILayout.SelectableLabel(_readmeContents, HELP_CONTENT_GUI_STYLE, GUILayout.Width(HELP_CONTENT_WIDTH), GUILayout.Height(readmeHeight));

					GUILayout.EndScrollView();
				}
				else if (_selectedHelpContentsIdx == HELP_TYPE_CHANGELOG_IDX)
				{
					_changelogScrollPos = GUILayout.BeginScrollView(
						_changelogScrollPos);

						float changelogHeight = _usedSkin.GetStyle(HELP_CONTENT_GUI_STYLE).CalcHeight(new GUIContent(_changelogContents), HELP_CONTENT_WIDTH);

						EditorGUILayout.SelectableLabel(_changelogContents, HELP_CONTENT_GUI_STYLE, GUILayout.Width(HELP_CONTENT_WIDTH), GUILayout.Height(changelogHeight));

					GUILayout.EndScrollView();
				}

		GUILayout.EndHorizontal();
	}

	int _selectedCategoryIdx = 0;

	bool IsInOverviewCategory
	{
		get
		{
			return _selectedCategoryIdx == OVERVIEW_IDX;
		}
	}

	bool IsInUsedAssetsCategory
	{
		get
		{
			return _selectedCategoryIdx == USED_ASSETS_IDX;
		}
	}

	bool IsInUnusedAssetsCategory
	{
		get
		{
			return _selectedCategoryIdx == UNUSED_ASSETS_IDX;
		}
	}

	bool IsInOptionsCategory
	{
		get
		{
			return _selectedCategoryIdx == OPTIONS_IDX;
		}
	}

	bool IsInHelpCategory
	{
		get
		{
			return _selectedCategoryIdx == HELP_IDX;
		}
	}

	string[] _categories = new string[] {OVERVIEW_CATEGORY_LABEL, USED_ASSETS_CATEGORY_LABEL, UNUSED_ASSETS_CATEGORY_LABEL, OPTIONS_CATEGORY_LABEL, HELP_CATEGORY_LABEL};

	const int OVERVIEW_IDX = 0;
	const int USED_ASSETS_IDX = 1;
	const int UNUSED_ASSETS_IDX = 2;
	const int OPTIONS_IDX = 3;
	const int HELP_IDX = 4;


	bool _finishedOpeningFromThread = false;
	string _lastOpenedBuildInfoFilePath = "";

	void _OpenBuildInfo(string filepath)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return;
		}

		_finishedOpeningFromThread = false;
		GetValueMessage = "Opening...";
		BuildReportTool.BuildInfo loadedBuild = BuildReportTool.Util.OpenSerializedBuildInfo(filepath, false);


		if (BuildReportTool.Util.BuildInfoHasContents(loadedBuild))
		{
			_buildInfo = loadedBuild;
			_lastOpenedBuildInfoFilePath = filepath;
		}
		else
		{
			Debug.LogError("Build Report Tool: Invalid data in build info file: " + filepath);
		}

		_finishedOpeningFromThread = true;

		GetValueMessage = "";
	}


	Thread _currentBuildReportFileLoadThread = null;

	bool IsCurrentlyOpeningAFile
	{
		get { return _currentBuildReportFileLoadThread != null && _currentBuildReportFileLoadThread.ThreadState != ThreadState.Running; }
	}

	void ForceStopFileLoadThread()
	{
		if (IsCurrentlyOpeningAFile)
		{
			try { _currentBuildReportFileLoadThread.Abort(); }
			catch (ThreadStateException) { }
		}
	}

	void OpenBuildInfoAsync(string filepath)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return;
		}

		_currentBuildReportFileLoadThread = new Thread(() => _OpenBuildInfo(filepath));
		_currentBuildReportFileLoadThread.Start();
	}


	void DrawTopRowButtons()
	{
		if (GUI.Button(new Rect(5, 5, 100, 20), REFRESH_LABEL) && !LoadingValuesFromThread)
		{
			Refresh();
		}
		if (GUI.Button(new Rect(110, 5, 100, 20), OPEN_LABEL) && !LoadingValuesFromThread)
		{
			string filepath = EditorUtility.OpenFilePanel(
				OPEN_SERIALIZED_BUILD_INFO_TITLE,
				BuildReportTool.Options.BuildReportSavePath,
				"xml");

			OpenBuildInfoAsync(filepath);
		}
		if (GUI.Button(new Rect(215, 5, 100, 20), SAVE_LABEL) && BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			string filepath = EditorUtility.SaveFilePanel(
				SAVE_MSG,
				BuildReportTool.Options.BuildReportSavePath,
				_buildInfo.GetDefaultFilename(),
				"xml");

			if (!string.IsNullOrEmpty(filepath))
			{
				BuildReportTool.Util.SerializeBuildInfo(_buildInfo, filepath);
			}
		}
		if (!BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			if (GUI.Button(new Rect(320, 5, 100, 20), OPTIONS_CATEGORY_LABEL))
			{
				_selectedCategoryIdx = OPTIONS_IDX;
			}
			if (GUI.Button(new Rect(425, 5, 100, 20), HELP_CATEGORY_LABEL))
			{
				_selectedCategoryIdx = HELP_IDX;
			}
		}
	}

	void InitiateDeleteSelected()
	{
		if (IsInUnusedAssetsCategory)
		{
			InitiateDeleteSelectedUnused();
		}
		else if (IsInUsedAssetsCategory)
		{
			InitiateDeleteSelectedUsed();
		}
	}

	void InitiateDeleteSelectedUnused()
	{
		InitiateDeleteSelectedInAssetList(_buildInfo.UnusedAssets);
	}

	void InitiateDeleteSelectedUsed()
	{
		InitiateDeleteSelectedInAssetList(_buildInfo.UsedAssets);
	}

	void InitiateDeleteSelectedInAssetList(BuildReportTool.AssetList listToDeleteFrom)
	{
		if (listToDeleteFrom.IsNothingSelected)
		{
			return;
		}



		BuildReportTool.SizePart[] all = listToDeleteFrom.All;

		int numOfFilesRequestedToDelete = listToDeleteFrom.GetSelectedCount();
		int numOfFilesToDelete = numOfFilesRequestedToDelete;
		int systemDeletionFileCount = 0;
		int brtFilesSelectedForDelete = 0;


		// filter out files that shouldn't be deleted
		// and identify unrecoverable files
		for (int n = 0, len = all.Length; n < len; ++n)
		{
			BuildReportTool.SizePart b = all[n];
			bool isThisFileToBeDeleted = listToDeleteFrom.InSumSelection(b);

			if (isThisFileToBeDeleted)
			{
				if (BuildReportTool.Util.IsFileInBuildReportFolder(b.Name) && !BuildReportTool.Util.IsUselessFile(b.Name))
				{
					//Debug.Log("BRT file? " + b.Name);
					--numOfFilesToDelete;
					++brtFilesSelectedForDelete;
				}
				else if (BuildReportTool.Util.HaveToUseSystemForDelete(b.Name))
				{
					++systemDeletionFileCount;
				}
			}
		}

		if (numOfFilesToDelete <= 0)
		{
			if (brtFilesSelectedForDelete > 0)
			{
				EditorApplication.Beep();
				EditorUtility.DisplayDialog("Can't delete!", "Take note that for safety, Build Report Tool assets themselves will not be included for deletion.", "OK");
			}
			return;
		}


		// prepare warning message for user

		bool deletingSystemFilesOnly = (systemDeletionFileCount == numOfFilesToDelete);
		bool deleteIsRecoverable = !deletingSystemFilesOnly;

		string plural = "";
		if (numOfFilesToDelete > 1)
		{
			plural = "s";
		}

		string message = null;

		if (numOfFilesRequestedToDelete != numOfFilesToDelete)
		{
			message = "Among " + numOfFilesRequestedToDelete + " file" + plural + " requested to be deleted, only " + numOfFilesToDelete + " will be deleted.";
		}
		else
		{
			message = "This will delete " + numOfFilesToDelete + " asset" + plural + " in your project.";
		}

		// add warning about BRT files that are skipped
		if (brtFilesSelectedForDelete > 0)
		{
			message += "\n\nTake note that for safety, " + brtFilesSelectedForDelete + " file" + ((brtFilesSelectedForDelete > 1) ? "s" : "") + " found to be Build Report Tool assets are not included for deletion.";
		}

		// add warning about unrecoverable files
		if (systemDeletionFileCount > 0)
		{
			if (deletingSystemFilesOnly)
			{
				message += "\n\nThe deleted file" + plural + " will not be recoverable from the " + BuildReportTool.Util.NameOfOSTrashFolder + ", unless you have your own backup.";
			}
			else
			{
				message += "\n\nAmong the " + numOfFilesToDelete + " file" + plural + " for deletion, " + systemDeletionFileCount + " will not be recoverable from the " + BuildReportTool.Util.NameOfOSTrashFolder + ", unless you have your own backup.";
			}
			message += "\n\nThis is a limitation in Unity and .NET code. To ensure deleting will move the files to the " + BuildReportTool.Util.NameOfOSTrashFolder + " instead, delete your files the usual way using your project view.";
		}
		else
		{
			message += "\n\nThe deleted file" + plural + " can be recovered from your " + BuildReportTool.Util.NameOfOSTrashFolder + ".";
		}

		message += "\n\nDeleting a large number of files may take a long time as Unity will rebuild the project's Library folder.\n\nProceed with deleting?";

		EditorApplication.Beep();
		if (!EditorUtility.DisplayDialog("Delete?", message, "Yes", "No"))
		{
			return;
		}

		List<BuildReportTool.SizePart> allList = new List<BuildReportTool.SizePart>(all);
		List<BuildReportTool.SizePart> toRemove = new List<BuildReportTool.SizePart>(all.Length/4);

		// finally, delete the files
		int deletedCount = 0;
		for (int n = 0, len = allList.Count; n < len; ++n)
		{
			BuildReportTool.SizePart b = allList[n];


			bool okToDelete = BuildReportTool.Util.IsUselessFile(b.Name) || !BuildReportTool.Util.IsFileInBuildReportFolder(b.Name);

			if (listToDeleteFrom.InSumSelection(b) && okToDelete)
			{
				// delete this

				if (BuildReportTool.Util.ShowFileDeleteProgress(deletedCount, numOfFilesToDelete, b.Name, deleteIsRecoverable))
				{
					return;
				}

				BuildReportTool.Util.DeleteSizePartFile(b);
				toRemove.Add(b);
				++deletedCount;
			}
		}
		EditorUtility.ClearProgressBar();


		// refresh the asset lists
		allList.RemoveAll(i => toRemove.Contains(i));
		BuildReportTool.SizePart[] allWithRemoved = allList.ToArray();

		// recreate per category list (maybe just remove from existing per category lists instead?)
		BuildReportTool.SizePart[][] perCategoryOfList = BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(allWithRemoved, _buildInfo.FileFilters);

		listToDeleteFrom.Reinit(allWithRemoved, perCategoryOfList);
		listToDeleteFrom.ClearSelection();



		// print info about the delete operation to console
		string finalMessage = deletedCount + " file" + plural + " removed from your project.";
		if (deleteIsRecoverable)
		{
			finalMessage += " They can be recovered from your " + BuildReportTool.Util.NameOfOSTrashFolder + ".";
		}

		EditorApplication.Beep();
		EditorUtility.DisplayDialog("Delete successful", finalMessage, "OK");

		Debug.LogWarning(finalMessage);
	}


	void InitiateDeleteAllUnused()
	{
		BuildReportTool.AssetList list = _buildInfo.UnusedAssets;
		BuildReportTool.SizePart[] all = list.All;

		int filesToDeleteCount = 0;

		for (int n = 0, len = all.Length; n < len; ++n)
		{
			BuildReportTool.SizePart b = all[n];

			bool okToDelete = BuildReportTool.Util.IsFileOkForDeleteAllOperation(b.Name);

			if (okToDelete)
			{
				//Debug.Log("added " + b.Name + " for deletion");
				++filesToDeleteCount;
			}
		}

		if (filesToDeleteCount == 0)
		{
			const string nothingToDelete = "Take note that for safety, Build Report Tool assets, Unity editor assets, version control metadata, and Unix-style hidden files will not be included for deletion.\n\nYou can force deleting them by selecting them (via the checkbox) and using \"Delete selected\", or simply delete them the normal way in your project view.";

			EditorApplication.Beep();
			EditorUtility.DisplayDialog("Nothing to delete!", nothingToDelete, "Ok");
			return;
		}

		string plural = "";
		if (filesToDeleteCount > 1)
		{
			plural = "s";
		}

		EditorApplication.Beep();
		if (!EditorUtility.DisplayDialog("Delete?",
				"Among " + all.Length + " file" + plural + " in your project, " + filesToDeleteCount + " will be deleted.\n\nBuild Report Tool assets themselves, Unity editor assets, version control metadata, and Unix-style hidden files will not be included for deletion. You can force-delete those by selecting them (via the checkbox) and use \"Delete selected\", or simply delete them the normal way in your project view.\n\nDeleting a large number of files may take a long time as Unity will rebuild the project's Library folder.\n\nAre you sure about this?\n\nThe file" + plural + " can be recovered from your " + BuildReportTool.Util.NameOfOSTrashFolder + ".", "Yes", "No"))
		{
			return;
		}

		List<BuildReportTool.SizePart> newAll = new List<BuildReportTool.SizePart>();

		int deletedCount = 0;
		for (int n = 0, len = all.Length; n < len; ++n)
		{
			BuildReportTool.SizePart b = all[n];

			bool okToDelete = BuildReportTool.Util.IsFileOkForDeleteAllOperation(b.Name);

			if (okToDelete)
			{
				// delete this
				if (BuildReportTool.Util.ShowFileDeleteProgress(deletedCount, filesToDeleteCount, b.Name, true))
				{
					return;
				}

				BuildReportTool.Util.DeleteSizePartFile(b);
				++deletedCount;
			}
			else
			{
				//Debug.Log("added " + b.Name + " to new list");
				newAll.Add(b);
			}
		}
		EditorUtility.ClearProgressBar();

		BuildReportTool.SizePart[] newAllArr = newAll.ToArray();

		BuildReportTool.SizePart[][] perCategoryUnused = BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(newAllArr, _buildInfo.FileFilters);

		list.Reinit(newAllArr, perCategoryUnused);
		list.ClearSelection();


		string finalMessage = filesToDeleteCount + " file" + plural + " removed from your project. They can be recovered from your " + BuildReportTool.Util.NameOfOSTrashFolder + ".";
		Debug.LogWarning(finalMessage);

		EditorApplication.Beep();
		EditorUtility.DisplayDialog("Delete successful", finalMessage, "OK");
	}


	void DrawCentralMessage(string msg)
	{
		float w = 300;
		float h = 100;
		float x = (position.width - w) * 0.5f;
		float y = (position.height - h) * 0.25f;

		GUI.Label(new Rect(x, y, w, h), msg);
	}

	bool ShouldShowDeleteButtons()
	{
		return
			(IsInUnusedAssetsCategory && _buildInfo.HasUnusedAssets) ||
			(IsInUsedAssetsCategory && _buildInfo.HasUsedAssets && BuildReportTool.Options.AllowDeletingOfUsedAssets);
	}

	void OnGUI()
	{
		//GUI.Label(new Rect(5, 100, 800, 20), "BuildReportTool.Util.ShouldReload: " + BuildReportTool.Util.ShouldReload + " EditorApplication.isCompiling: " + EditorApplication.isCompiling);
		if (_usedSkin == null)
		{
			GUI.Label(new Rect(20, 20, 500, 100), BuildReportTool.Options.BUILD_REPORT_PACKAGE_MISSING_MSG);
			return;
		}

		GUI.skin = _usedSkin;

		DrawTopRowButtons();


		// loading message
		if (LoadingValuesFromThread)
		{
			DrawCentralMessage(GetValueMessage);
			return;
		}
		// content to show when there is no build report on display
		else if (!BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			if (IsInOptionsCategory)
			{
				GUILayout.Space(40);
				_assetListScrollPos = GUILayout.BeginScrollView(
					_assetListScrollPos);
					DrawOptionsScreen();
				GUILayout.EndScrollView();
			}
			else if (IsInHelpCategory)
			{
				GUILayout.Space(40);
				_assetListScrollPos = GUILayout.BeginScrollView(
					_assetListScrollPos);
					DrawHelpScreen();
				GUILayout.EndScrollView();
			}
			else if (IsWaitingForBuildCompletionToGenerateBuildReport)
			{
				DrawCentralMessage(WAITING_FOR_BUILD_TO_COMPLETE_MSG);
			}
			else
			{
				DrawCentralMessage(NO_BUILD_INFO_FOUND_MSG);
			}

			return;
		}



		GUILayout.Space(10); // top padding



		// report title
		GUILayout.BeginVertical();
			GUILayout.Space(20);
			GUILayout.Label(_buildInfo.ProjectName, MAIN_TITLE_STYLE_NAME);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(BUILD_TYPE_PREFIX_MSG + _buildInfo.BuildType + BUILD_TYPE_SUFFIX_MSG, MAIN_SUBTITLE_STYLE_NAME);
			if (!string.IsNullOrEmpty(_buildInfo.UnityVersion))
			{
				GUILayout.Label(UNITY_VERSION_PREFIX_MSG + _buildInfo.UnityVersion, MAIN_SUBTITLE_STYLE_NAME);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		GUILayout.EndVertical();



		// category buttons
		int newSelectedCategoryIdx = GUILayout.SelectionGrid(_selectedCategoryIdx, _categories, _categories.Length);

		if (_selectedCategoryIdx != OPTIONS_IDX && newSelectedCategoryIdx == OPTIONS_IDX)
		{
			// moving into the options screen
			_fileFilterGroupToUseOnOpeningOptionsWindow = BuildReportTool.Options.FilterToUseInt;
		}
		else if (IsInOptionsCategory && newSelectedCategoryIdx != OPTIONS_IDX)
		{
			// moving away from the options screen
			_fileFilterGroupToUseOnClosingOptionsWindow = BuildReportTool.Options.FilterToUseInt;

			if (_fileFilterGroupToUseOnOpeningOptionsWindow != _fileFilterGroupToUseOnClosingOptionsWindow)
			{
				RecategorizeDisplayedBuildInfo();
			}
		}
		_selectedCategoryIdx = newSelectedCategoryIdx;



		// additional controls specific to each screen
		if ((IsInUsedAssetsCategory && _buildInfo.HasUsedAssets) || (IsInUnusedAssetsCategory && _buildInfo.HasUnusedAssets))
		{
			GUILayout.Space(5);
			BuildReportTool.AssetList assetListToDisplay = IsInUsedAssetsCategory ? _buildInfo.UsedAssets : _buildInfo.UnusedAssets;


			//Debug.Log("file filter to draw: " + FileFilterGroupToUse);
			FileFilterGroupToUse.Draw(assetListToDisplay, position.width - 100);

			BuildReportTool.AssetList assetListUsed = IsInUsedAssetsCategory ? _buildInfo.UsedAssets : _buildInfo.UnusedAssets;
			BuildReportTool.SizePart[] assetListToUse = assetListUsed.GetListToDisplay(FileFilterGroupToUse);

			GUILayout.BeginHorizontal();
				//GUILayout.Space(20);

				float widthTry = 0;

				int viewOffset = assetListUsed.GetViewOffsetForDisplayedList(FileFilterGroupToUse);


				const string pagePrevLabel = "Previous";
				const string pageNextLabel = "Next";

				float prevW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(pagePrevLabel)).x;
				float nextW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(pageNextLabel)).x;


				// Unused Assets Batch
				if (IsInUnusedAssetsCategory && _buildInfo.HasUnusedAssets)
				{
					float batchW = 0;
					batchW += prevW;
					batchW += nextW;

					int batchNumber = _buildInfo.UnusedAssetsBatchNum + 1;

					string batchLabel = "Viewing batch " + batchNumber;

					float batchLabelW = GUI.skin.GetStyle("label").CalcSize(new GUIContent(batchLabel)).x;
					batchW += batchLabelW;


					if (GUILayout.Button(pagePrevLabel) && (batchNumber - 1 >= 1))
					{
						// move to previous batch
						BuildReportTool.ReportManager.MoveUnusedAssetsBatchToPrev(_buildInfo, FileFilterGroupToUse);
					}
					GUILayout.Label(batchLabel, GUILayout.Width(batchLabelW));

					if (GUILayout.Button(pageNextLabel))
					{
						// move to next batch
						BuildReportTool.ReportManager.MoveUnusedAssetsBatchToNext(_buildInfo, FileFilterGroupToUse);
					}


					// break to a new line or not
					if (widthTry + batchW > (position.width - 30))
					{
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();

						widthTry = batchW;
					}
					else
					{
						GUILayout.FlexibleSpace();

						widthTry += batchW;
					}
				}





				// Paginate Buttons

				float paginateW = 0;
				paginateW += prevW;


				if (GUILayout.Button(pagePrevLabel) && (viewOffset - BuildReportTool.Options.AssetListPaginationLength >= 0))
				{
					assetListUsed.SetViewOffsetForDisplayedList(FileFilterGroupToUse, viewOffset - BuildReportTool.Options.AssetListPaginationLength);
				}

				int len = Mathf.Min(viewOffset + BuildReportTool.Options.AssetListPaginationLength, assetListToUse.Length);
				int offsetNonZeroBased = viewOffset + (len > 0 ? 1 : 0);

				string paginateLabel = "Viewing page " + offsetNonZeroBased.ToString("N0") + " - " + len.ToString("N0") + " of " + assetListToUse.Length.ToString("N0");
				float paginateLabelW = GUI.skin.GetStyle("label").CalcSize(new GUIContent(paginateLabel)).x;
				GUILayout.Label(paginateLabel, GUILayout.Width(paginateLabelW));

				paginateW += paginateLabelW;

				paginateW += nextW;

				if (GUILayout.Button(pageNextLabel) && (viewOffset + BuildReportTool.Options.AssetListPaginationLength < assetListToUse.Length))
				{
					assetListUsed.SetViewOffsetForDisplayedList(FileFilterGroupToUse, viewOffset + BuildReportTool.Options.AssetListPaginationLength);
				}

				// break to a new line or not
				if (widthTry + paginateW > (position.width - 30))
				{
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();

					widthTry = paginateW;
				}
				else
				{
					GUILayout.FlexibleSpace();

					widthTry += paginateW;
				}



				// Selection Info

				float selectAllW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(SELECT_ALL_LABEL)).x;
				float selectNoneW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(SELECT_NONE_LABEL)).x;

				string selectedInfoLabel = SELECTED_QTY_LABEL + assetListUsed.GetSelectedCount().ToString("N0") + ". " + SELECTED_SIZE_LABEL + assetListUsed.GetReadableSizeOfSumSelection() + ". " + SELECTED_PERCENT_LABEL + assetListUsed.GetPercentageOfSumSelection().ToString("N") + "%";
				float selectedInfoLabelW = GUI.skin.GetStyle(INFO_SUBTITLE_STYLE_NAME).CalcSize(new GUIContent(selectedInfoLabel)).x;

				float selectionGroupW = selectAllW + selectNoneW + selectedInfoLabelW;

				// break to a new line or not
				if (widthTry + selectionGroupW > (position.width - 30))
				{
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();

					widthTry = selectionGroupW;
				}
				else
				{
					GUILayout.FlexibleSpace();

					widthTry += selectionGroupW;
				}


				if (GUILayout.Button(SELECT_ALL_LABEL))
				{
					//int rangeForSelection = BuildReportTool.Options.AssetListPaginationLength;
					//if (viewOffset + rangeForSelection > assetListToUse.Length)
					//{
					//	rangeForSelection = assetListToUse.Length - viewOffset;
					//}
					//assetListUsed.AddDisplayedRangeToSumSelection(FileFilterGroupToUse, viewOffset, rangeForSelection);

					assetListUsed.AddAllDisplayedToSumSelection(FileFilterGroupToUse);
				}
				if (GUILayout.Button(SELECT_NONE_LABEL))
				{
					assetListUsed.ClearSelection();
				}

				GUILayout.Label(selectedInfoLabel, INFO_SUBTITLE_STYLE_NAME, GUILayout.Width(selectedInfoLabelW));




				// Delete buttons

				string delSelectedLabel = "Delete ";
				if (assetListUsed.AtLeastOneSelectedForSum)
				{
					delSelectedLabel += assetListUsed.GetSelectedCount() + " ";
				}
				delSelectedLabel += "selected";

				float delSelectedLabelW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(delSelectedLabel)).x;


				const string deleteAllLabel = "Delete all";
				float deleteAllLabelW = GUI.skin.GetStyle("button").CalcSize(new GUIContent(deleteAllLabel)).x;


				float deletionGroupW = delSelectedLabelW + deleteAllLabelW;

				// break to a new line or not
				if (widthTry + deletionGroupW > (position.width - 30))
				{
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();

					widthTry = deletionGroupW;
				}
				else
				{
					GUILayout.FlexibleSpace();

					widthTry += deletionGroupW;
				}


				if (ShouldShowDeleteButtons())
				{
					GUI.backgroundColor = Color.red;
					if (GUILayout.Button(delSelectedLabel))
					{
						InitiateDeleteSelected();
					}

					// allow delete all only in the unused assets list
					if (IsInUnusedAssetsCategory && GUILayout.Button(deleteAllLabel))
					{
						InitiateDeleteAllUnused();
					}
					GUI.backgroundColor = Color.white;
				}

				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.Space(10);
		}


		// main content
		GUILayout.BeginHorizontal();
			GUILayout.Space(15); // left padding
			GUILayout.BeginVertical();


				_assetListScrollPos = GUILayout.BeginScrollView(
					_assetListScrollPos);


					if (IsInOverviewCategory)
					{
						DrawOverviewScreen();
					}
					else if (IsInUsedAssetsCategory)
					{
						if (_buildInfo.HasUsedAssets)
						{
							DrawAssetList(_buildInfo.UsedAssets, FileFilterGroupToUse, BuildReportTool.Options.AssetListPaginationLength);
						}
						else if (!_buildInfo.UsedAssetsIncludedInCreation)
						{
							DrawCentralMessage("No \"Used Assets\" included in this build report.");
						}
					}
					else if (IsInUnusedAssetsCategory)
					{
						if (_buildInfo.HasUnusedAssets)
						{
							DrawAssetList(_buildInfo.UnusedAssets, FileFilterGroupToUse, BuildReportTool.Options.AssetListPaginationLength);
						}
						else if (!_buildInfo.UnusedAssetsIncludedInCreation)
						{
							DrawCentralMessage("No \"Unused Assets\" included in this build report.");
						}
					}
					else if (IsInOptionsCategory)
					{
						DrawOptionsScreen();
					}
					else if (IsInHelpCategory)
					{
						DrawHelpScreen();
					}

					GUILayout.FlexibleSpace();
				GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(5); // right padding
		GUILayout.EndHorizontal();


		GUILayout.Space(10); // bottom padding
	}
}

#endif
