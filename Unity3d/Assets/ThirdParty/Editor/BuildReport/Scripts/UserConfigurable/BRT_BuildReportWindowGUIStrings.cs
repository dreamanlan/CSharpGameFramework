#if UNITY_EDITOR
using UnityEditor;

public partial class BRT_BuildReportWindow : EditorWindow
{
	// GUI messages, labels

	const string WAITING_FOR_BUILD_TO_COMPLETE_MSG = "Waiting for build to complete... Click this window if not in focus to refresh.";

	const string NO_BUILD_INFO_FOUND_MSG = "No Build Info.\n\nClick \"Get From Log\" to retrieve the last build info from the Editor log.\n\nClick Open to manually open a previously saved build report file.";



	const string MONO_DLLS_LABEL = "Included DLLs:";
	const string SCRIPT_DLLS_LABEL = "Script DLLs:";



	const string TIME_OF_BUILD_LABEL = "Time of Build:";



	const string UNUSED_TOTAL_SIZE_LABEL = "Total Unused\nAssets Size:";
	const string USED_TOTAL_SIZE_LABEL = "Total Used\nAssets Size:";
	const string STREAMING_ASSETS_TOTAL_SIZE_LABEL = "Streaming\nAssets Size:";
	const string BUILD_TOTAL_SIZE_LABEL = "Total Build\nSize:";

	const string WEB_UNITY3D_FILE_SIZE_LABEL = "Size of .unity3d File:";

	const string ANDROID_APK_FILE_SIZE_LABEL = "Size of .apk File:";
	const string ANDROID_OBB_FILE_SIZE_LABEL = "Size of .obb File:";


	const string UNUSED_TOTAL_SIZE_DESC = "Total size of project assets not included in the build.";
	const string USED_TOTAL_SIZE_DESC = "Total size of the used assets before being packed.\nAlso includes size of compiled Mono scripts.\nDoes not include size of files in StreamingAssets.";
	const string STREAMING_ASSETS_SIZE_DESC = "Total size of all files in the StreamingAssets folder.";


	const string BUILD_SIZE_STANDALONE_DESC = "File size of the executable file and the accompanying Data folder.";
	const string BUILD_SIZE_WINDOWS_DESC = "File size of the .exe file and the accompanying Data folder.";
	const string BUILD_SIZE_MACOSX_DESC = "File size of the .app file.";
	const string BUILD_SIZE_LINUX_UNIVERSAL_DESC = "File size of both 32-bit and 64-bit executables, plus the accompanying Data folder.";
	const string BUILD_SIZE_WEB_DESC = "File size of the whole web build folder.";

	const string BUILD_SIZE_IOS_DESC = "File size of the Xcode project folder.";

	const string BUILD_SIZE_ANDROID_DESC = "File size of resulting .apk file.";
	const string BUILD_SIZE_ANDROID_WITH_OBB_DESC = "File size of resulting .apk and .obb files.";
	const string BUILD_SIZE_ANDROID_WITH_PROJECT_DESC = "File size of generated Eclipse project folder.";


	const string OPEN_SERIALIZED_BUILD_INFO_TITLE = "Open Build Info XML File";

	const string TOTAL_SIZE_BREAKDOWN_LABEL = "Used Assets Size Breakdown:";

	const string TOTAL_SIZE_BREAKDOWN_MSG_PRE_BOLD = "Based on";
	const string TOTAL_SIZE_BREAKDOWN_MSG_BOLD = "uncompressed";
	const string TOTAL_SIZE_BREAKDOWN_MSG_POST_BOLD = "build size";


	const string ASSET_SIZE_BREAKDOWN_LABEL = "Asset Breakdown:";

	const string ASSET_SIZE_BREAKDOWN_MSG_PRE_BOLD = "Sorted by";
	const string ASSET_SIZE_BREAKDOWN_MSG_BOLD = "uncompressed";
	const string ASSET_SIZE_BREAKDOWN_MSG_POST_BOLD = "size. Click on name to select/ping it in the Project window. Click on checkbox to include it in size calculations or batch deletions.";

	const string NO_FILES_FOR_THIS_CATEGORY = "None";

	const string NON_APPLICABLE_PERCENTAGE = "N/A";

	const string OVERVIEW_CATEGORY_LABEL = "Overview";
	const string USED_ASSETS_CATEGORY_LABEL = "Used Assets";
	const string UNUSED_ASSETS_CATEGORY_LABEL = "Unused Assets";
	const string OPTIONS_CATEGORY_LABEL = "Options";
	const string HELP_CATEGORY_LABEL = "Help & Info";

	const string REFRESH_LABEL = "Get From Log";
	const string OPEN_LABEL = "Open";
	const string SAVE_LABEL = "Save";

	const string SAVE_MSG = "Save Build Info to XML";

	const string SELECT_ALL_LABEL = "Select All";
	const string SELECT_NONE_LABEL = "Select None";
	const string SELECTED_QTY_LABEL = "Selected: ";
	const string SELECTED_SIZE_LABEL = "Total size: ";
	const string SELECTED_PERCENT_LABEL = "Total percentage: ";

	const string BUILD_TYPE_PREFIX_MSG = "For ";
	const string BUILD_TYPE_SUFFIX_MSG = "";
	const string UNITY_VERSION_PREFIX_MSG = ", built in ";

	const string COLLECT_BUILD_INFO_LABEL = "Collect and save build info automatically after building (includes batchmode builds)";
	const string SHOW_AFTER_BUILD_LABEL = "Show Build Report Window automatically after building";
	const string INCLUDE_SVN_LABEL = "Include SVN metadata in unused assets scan";
	const string INCLUDE_GIT_LABEL = "Include Git metadata in unused assets scan";
	const string FILE_FILTER_DISPLAY_TYPE_LABEL = "Draw file filters as:";

	const string FILE_FILTER_DISPLAY_TYPE_DROP_DOWN_LABEL = "Dropdown box";
	const string FILE_FILTER_DISPLAY_TYPE_BUTTONS_LABEL = "Buttons";

	const string SAVE_PATH_LABEL = "Current Build Report save path: ";
	const string SAVE_FOLDER_NAME_LABEL = "Folder name for Build Reports:";
	const string SAVE_PATH_TYPE_LABEL = "Save build reports:";

	const string SAVE_PATH_TYPE_PERSONAL_DEFAULT_LABEL = "In user's personal folder";
	const string SAVE_PATH_TYPE_PERSONAL_WIN_LABEL = "In \"My Documents\" folder";
	const string SAVE_PATH_TYPE_PERSONAL_MAC_LABEL = "In Home folder";
	const string SAVE_PATH_TYPE_PROJECT_LABEL = "Beside project folder";

	const string EDITOR_LOG_LABEL = "Unity Editor.log path ";
	const string EDITOR_LOG_INVALID_MSG = "Invalid path. Please change the path by clicking \"Set Override Log\"";

	const string SET_OVERRIDE_LOG_LABEL = "Set Override Log";
	const string CLEAR_OVERRIDE_LOG_LABEL = "Clear Override Log";

	const string FILTER_GROUP_TO_USE_LABEL = "File Filter Group To Use:";
	const string FILTER_GROUP_FILE_PATH_LABEL = "Configured File Filter Group: ";

	const string FILTER_GROUP_TO_USE_CONFIGURED_LABEL = "Always use configured file filter group";
	const string FILTER_GROUP_TO_USE_EMBEDDED_LABEL = "Use file filter group embedded in file if available\n(when opening build report files)";

	const string OPEN_IN_FILE_BROWSER_DEFAULT_LABEL = "Open in file browser";
	const string OPEN_IN_FILE_BROWSER_WIN_LABEL = "Show in Explorer";
	const string OPEN_IN_FILE_BROWSER_MAC_LABEL = "Reveal in Finder";



	const string CALCULATION_LEVEL_FULL_NAME = "4 - Full Report (complete calculations)";
	const string CALCULATION_LEVEL_FULL_DESC = "Calculate everything. Will show size breakdown, \"Used Assets\", and \"Unused Assets\" list.\n\nThis can be slow if you have a large project with thousands of files or objects in scenes. If you get out of memory errors, try the lower calculation levels.";

	const string CALCULATION_LEVEL_NO_PREFAB_NAME = "3 - Do not calculate unused prefabs";
	const string CALCULATION_LEVEL_NO_PREFAB_DESC = "Will calculate everything, except that it will not determine whether a prefab is unused. It will still show which other assets are unused.\n\nIf you have scenes that use hundreds to thousands of prefabs and you get an out of memory error when generating a build report, try this setting.";

	const string CALCULATION_LEVEL_NO_UNUSED_NAME = "2 - Do not calculate unused assets";
	const string CALCULATION_LEVEL_NO_UNUSED_DESC = "Will display overview data and \"Used Assets\" list only. It will not determine which assets are unused.\n\nIt will not show Streaming Assets files in your Used Assets list, but their total size will still be shown in the Overview.";

	const string CALCULATION_LEVEL_MINIMAL_NAME = "1 - Overview only (minimum calculations)";
	const string CALCULATION_LEVEL_MINIMAL_DESC = "Will display overview data only. This is the fastest but also shows the least information.";
}

#endif
