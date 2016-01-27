using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BuildReportTool
{

public static class Options
{
	// constants
	public const string BUILD_REPORT_PACKAGE_MOVED_MSG = "BuildReport package seems to have been moved. Finding...";

	public const string BUILD_REPORT_PACKAGE_MISSING_MSG = "Unable to find BuildReport package folder! Cannot find suitable GUI Skin.\nTry editing the source code and change the value\nof `BUILD_REPORT_TOOL_DEFAULT_PATH` to what path the Build Report Tool is in.\nMake sure the folder is named \"BuildReport\".";

	public const string BUILD_REPORT_TOOL_DEFAULT_PATH = "Assets/BuildReport";
	public const string BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME = "BuildReport";

	public const string BUILD_REPORTS_DEFAULT_FOLDER_NAME = "UnityBuildReports";



	// user options

	public static string EditorLogOverridePath
	{
		get
		{
			return EditorPrefs.GetString("BRT_EditorLogOverridePath", "");
		}
		set
		{
			EditorPrefs.SetString("BRT_EditorLogOverridePath", value);
		}
	}

	public static bool IncludeSvnInUnused
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeSvnInUnused", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeSvnInUnused", value);
		}
	}

	public static bool IncludeGitInUnused
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeGitInUnused", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeGitInUnused", value);
		}
	}

	public static FileFilterDisplay GetOptionFileFilterDisplay()
	{
		switch (FileFilterDisplayInt)
		{
			case 0:
				return FileFilterDisplay.DropDown;
			case 1:
				return FileFilterDisplay.Buttons;
		}
		return FileFilterDisplay.DropDown;
	}

	public static int FileFilterDisplayInt
	{
		get
		{
			return EditorPrefs.GetInt("BRT_FileFilterDisplay", 0);
		}
		set
		{
			EditorPrefs.SetInt("BRT_FileFilterDisplay", value);
		}
	}

	public static bool AllowDeletingOfUsedAssets
	{
		get
		{
			return EditorPrefs.GetBool("BRT_AllowDeletingOfUsedAssets", false);
		}
		set
		{
			EditorPrefs.SetBool("BRT_AllowDeletingOfUsedAssets", value);
		}
	}


	public static bool CollectBuildInfo
	{
		get
		{
			return EditorPrefs.GetBool("BRT_CollectBuildInfo", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_CollectBuildInfo", value);
		}
	}

	/*public static int GetOptionEditorLogMegaByteSizeReadLimit()
	{
		return EditorPrefs.GetInt("BRT_EditorLogMegaByteSizeReadLimit", 100);
	}
	public static void SetOptionEditorLogMegaByteSizeReadLimit(int val)
	{
		EditorPrefs.SetInt("BRT_EditorLogMegaByteSizeReadLimit", val);
	}*/


	public static string BuildReportFolderName
	{
		get
		{
			return EditorPrefs.GetString("BRT_BuildReportFolderName", BUILD_REPORTS_DEFAULT_FOLDER_NAME);
		}
		set
		{
			EditorPrefs.SetString("BRT_BuildReportFolderName", value);
		}
	}


	public static string BuildReportSavePath
	{
		get
		{
			return EditorPrefs.GetString("BRT_SavePath", BuildReportTool.Util.GetUserHomeFolder() + "/" + BuildReportFolderName);
		}
		set
		{
			EditorPrefs.SetString("BRT_SavePath", value + "/" + BuildReportFolderName);
		}
	}




	public static int SaveType
	{
		get
		{
			return EditorPrefs.GetInt("BRT_SaveType", SAVE_TYPE_PERSONAL);
		}
		set
		{
			EditorPrefs.SetInt("BRT_SaveType", value);
		}
	}

	public enum FileFilterDisplay
	{
		DropDown = 0,
		Buttons = 1
	}

	public const int SAVE_TYPE_PERSONAL = 0;
	public const int SAVE_TYPE_PROJECT = 1;



	public enum FilterToUseType
	{
		UseConfiguredFile,
		UseEmbedded
	}

	public static FilterToUseType GetOptionFilterToUse()
	{
		switch (FilterToUseInt)
		{
			case 0:
				return FilterToUseType.UseConfiguredFile;
			case 1:
				return FilterToUseType.UseEmbedded;
		}
		return FilterToUseType.UseConfiguredFile;
	}

	public static bool ShouldUseConfiguredFileFilters()
	{
		//Debug.Log("GetOptionFilterToUse() " + GetOptionFilterToUse());
		return GetOptionFilterToUse() == FilterToUseType.UseConfiguredFile;
	}

	public static bool ShouldUseEmbeddedFileFilters()
	{
		return GetOptionFilterToUse() == FilterToUseType.UseEmbedded;
	}

	public static int FilterToUseInt
	{
		get
		{
			return EditorPrefs.GetInt("BRT_FilterToUse", 0);
		}
		set
		{
			EditorPrefs.SetInt("BRT_FilterToUse", value);
		}
	}



	public static int AssetListPaginationLength
	{
		get
		{
			return EditorPrefs.GetInt("BRT_AssetPaginationLength", 300);
		}
		set
		{
			EditorPrefs.SetInt("BRT_AssetPaginationLength", value);
		}
	}


	public static int UnusedAssetsEntriesPerBatch
	{
		get
		{
			return EditorPrefs.GetInt("BRT_UnusedAssetsEntriesPerBatch", 1000);
		}
		set
		{
			EditorPrefs.SetInt("BRT_UnusedAssetsEntriesPerBatch", value);
		}
	}


	// Build Report Calculation
	//  Full report
	//  No prefabs in unused assets calculation
	//  No unused assets calculation, but still has used assets list (won't collect prefabs in scene)
	//  No used assets and unused assets calculation (overview only)

	public static bool IncludeUsedAssetsInReportCreation
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeUsedAssetsInReportCreation", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeUsedAssetsInReportCreation", value);
		}
	}

	public static bool IncludeUnusedAssetsInReportCreation
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeUnusedAssetsInReportCreation", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeUnusedAssetsInReportCreation", value);
		}
	}

	public static bool IncludeUnusedPrefabsInReportCreation
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeUnusedPrefabsInReportCreation", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeUnusedPrefabsInReportCreation", value);
		}
	}


	public static bool IsCalculationLevelAtFull(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		return includeUsedAssets && includeUnusedAssets && includeUnusedPrefabs;
	}

	public static bool IsCalculationLevelAtNoUnusedPrefabs(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		return includeUsedAssets && includeUnusedAssets && !includeUnusedPrefabs;
	}

	public static bool IsCalculationLevelAtNoUnusedAssets(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		// unused prefabs are not checked. if unused assets are not calculated, it is understood that unused prefabs are not included
		return includeUsedAssets && !includeUnusedAssets;
	}

	public static bool IsCalculationLevelAtOverviewOnly(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		// if used assets not included, it is understood that unused assets are not included too.
		// if used assets are not included, there is no way to determing if an asset is unused.
		return !includeUsedAssets;
	}




	public static bool IsCurrentCalculationLevelAtFull
	{
		get
		{
			return IsCalculationLevelAtFull(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtNoUnusedPrefabs
	{
		get
		{
			return IsCalculationLevelAtNoUnusedPrefabs(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtNoUnusedAssets
	{
		get
		{
			return IsCalculationLevelAtNoUnusedAssets(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtOverviewOnly
	{
		get
		{
			return IsCalculationLevelAtOverviewOnly(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}


	public static void SetCalculationLevelToFull()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = true;
		IncludeUnusedPrefabsInReportCreation = true;
	}
	public static void SetCalculationLevelToNoUnusedPrefabs()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = true;
		IncludeUnusedPrefabsInReportCreation = false;
	}
	public static void SetCalculationLevelToNoUnusedAssets()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = false;
		IncludeUnusedPrefabsInReportCreation = false;
	}
	public static void SetCalculationLevelToOverviewOnly()
	{
		IncludeUsedAssetsInReportCreation = false;
		IncludeUnusedAssetsInReportCreation = false;
		IncludeUnusedPrefabsInReportCreation = false;
	}








	public static bool AutoShowWindowAfterNormalBuild
	{
		get
		{
			return EditorPrefs.GetBool("BRT_AutoShowWindowAfterNormalBuild", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_AutoShowWindowAfterNormalBuild", value);
		}
	}
	public static bool AutoShowWindowAfterBatchModeBuild
	{
		get
		{
			return EditorPrefs.GetBool("BRT_AutoShowWindowAfterBatchModeBuild", false);
		}
		set
		{
			EditorPrefs.SetBool("BRT_AutoShowWindowAfterBatchModeBuild", value);
		}
	}


	public static void SetAutoShowWindowTypeToNever()
	{
		AutoShowWindowAfterNormalBuild = false;
		AutoShowWindowAfterBatchModeBuild = false;
	}

	public static void SetAutoShowWindowTypeToAlways()
	{
		AutoShowWindowAfterNormalBuild = true;
		AutoShowWindowAfterBatchModeBuild = true;
	}

	public static void SetAutoShowWindowTypeToNotInBatchMode()
	{
		AutoShowWindowAfterNormalBuild = true;
		AutoShowWindowAfterBatchModeBuild = false;
	}



	public static bool IsAutoShowWindowTypeSetToNever
	{
		get
		{
			return
				!AutoShowWindowAfterNormalBuild &&
				!AutoShowWindowAfterBatchModeBuild;
		}
	}

	public static bool IsAutoShowWindowTypeSetToAlways
	{
		get
		{
			return
				AutoShowWindowAfterNormalBuild &&
				AutoShowWindowAfterBatchModeBuild;
		}
	}

	public static bool IsAutoShowWindowTypeSetToNotInBatchMode
	{
		get
		{
			return
				AutoShowWindowAfterNormalBuild &&
				!AutoShowWindowAfterBatchModeBuild;
		}
	}



	public static bool ShouldShowWindowAfterBuild
	{
		get
		{
			return
				(IsInBatchMode && AutoShowWindowAfterBatchModeBuild) ||
				(!IsInBatchMode && AutoShowWindowAfterNormalBuild);
		}
	}

	public static bool IsInBatchMode
	{
		get
		{
			return UnityEditorInternal.InternalEditorUtility.inBatchMode;


#if OTHER_BATCH_MODE_DETECTION_CODE
			// different ways to find out actually.
			// included here in case a new version of Unity
			// removes our current way of figuring out batchmode.

			// check the isHumanControllingUs bool
			return UnityEditorInternal.InternalEditorUtility.isHumanControllingUs;

			// check the command line args for "-batchmode"
			string[] arguments = Environment.GetCommandLineArgs();
			for (int n = 0, len = arguments.Length; n < len; ++n)
			{
				if (arguments[n] == "-batchmode")
				{
					return true;
				}
			}
			return false;
#endif
		}
	}
}

}
