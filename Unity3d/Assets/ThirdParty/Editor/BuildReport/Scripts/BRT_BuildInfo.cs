using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BuildReportTool
{

[System.Serializable]
public class SizePart
{
	public string Name; // file with path, but relative to project's Assets folder
	public string Size;
	public double Percentage = 0;
	public int SizeBytes = -1;
	public double DerivedSize = 0; // in cases where we don't have exact values of file size (we just got it from editor log converted to readable format already)

	public double UsableSize
	{
		get
		{
			if (DerivedSize > 0)
				return DerivedSize;
			return SizeBytes;
		}
	}

	public bool IsTotal { get{ return Name == "Complete size"; } }
}

[System.Serializable]
public class AssetList
{
	[SerializeField]
	BuildReportTool.SizePart[] _all;

	int[] _viewOffsets;

	[SerializeField]
	BuildReportTool.SizePart[][] _perCategory;

	public List<BuildReportTool.SizePart> GetAllAsList()
	{
		return _all.ToList();
	}

	public int AllCount
	{
		get
		{
			return _all.Length;
		}
	}

	public double GetTotalSizeInBytes()
	{
		double total = 0;

		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			if (_all[n].UsableSize > 0)
			{
				total += _all[n].UsableSize;
			}
		}
		return total;
	}

	public void UnescapeAssetNames()
	{
		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			_all[n].Name = BuildReportTool.Util.MyHtmlDecode(_all[n].Name);
		}
	}

	public int GetViewOffsetForDisplayedList(FileFilterGroup fileFilters)
	{
		if (fileFilters.SelectedFilterIdx == -1)
		{
			return _viewOffsets[0];
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			return _viewOffsets[fileFilters.SelectedFilterIdx+1];
		}
		return 0;
	}

	public void SetViewOffsetForDisplayedList(FileFilterGroup fileFilters, int newVal)
	{
		if (fileFilters.SelectedFilterIdx == -1)
		{
			_viewOffsets[0] = newVal;
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			_viewOffsets[fileFilters.SelectedFilterIdx+1] = newVal;
		}
	}


	public BuildReportTool.SizePart[] GetListToDisplay(FileFilterGroup fileFilters)
	{
		BuildReportTool.SizePart[] ret = null;
		if (fileFilters.SelectedFilterIdx == -1)
		{
			ret = All;
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			ret = PerCategory[fileFilters.SelectedFilterIdx];
		}
		return ret;
	}

	[SerializeField]
	string[] _labels;

	public void Init(BuildReportTool.SizePart[] all, BuildReportTool.SizePart[][] perCategory, FileFilterGroup fileFilters)
	{
		_all = all;
		_perCategory = perCategory;

		_viewOffsets = new int[1 + PerCategory.Length];

		RefreshFilterLabels(fileFilters);
	}

	public void RefreshFilterLabels(FileFilterGroup fileFiltersToUse)
	{
		_labels = new string[1 + PerCategory.Length];
		_labels[0] = "All (" + All.Length + ")";
		for (int n = 0, len = fileFiltersToUse.Count; n < len; ++n)
		{
			_labels[n+1] = fileFiltersToUse[n].Label + " (" + PerCategory[n].Length + ")";
		}
		_labels[_labels.Length-1] = "Unknown (" + PerCategory[PerCategory.Length-1].Length + ")";
	}

	public BuildReportTool.SizePart[] All { get{ return _all; } set{ _all = value; } }
	public BuildReportTool.SizePart[][] PerCategory { get{ return _perCategory; } }

	public void AssignPerCategoryList(BuildReportTool.SizePart[][] perCategory)
	{
		_perCategory = perCategory;
		_viewOffsets = new int[1 + _perCategory.Length];
	}
	public void Reinit(BuildReportTool.SizePart[] all, BuildReportTool.SizePart[][] perCategory)
	{
		_all = all;
		_perCategory = perCategory;
	}

	public string[] Labels { get{ return _labels; } set{ _labels = value; } }





	[SerializeField]
	Dictionary <string, BuildReportTool.SizePart> _selectedForSum = new Dictionary<string, BuildReportTool.SizePart>();

	public void ToggleSumSelection(BuildReportTool.SizePart b)
	{
		if (InSumSelection(b))
		{
			RemoveFromSumSelection(b);
		}
		else
		{
			AddToSumSelection(b);
		}
	}

	public bool InSumSelection(BuildReportTool.SizePart b)
	{
		return _selectedForSum.ContainsKey(b.Name);
	}

	public void RemoveFromSumSelection(BuildReportTool.SizePart b)
	{
		_selectedForSum.Remove(b.Name);
	}

	public void AddToSumSelection(BuildReportTool.SizePart b)
	{
		_selectedForSum.Add(b.Name, b);
	}

	public void AddDisplayedRangeToSumSelection(FileFilterGroup fileFilters, int offset, int range)
	{
		BuildReportTool.SizePart[] listForSelection = GetListToDisplay(fileFilters);

		for (int n = offset; n < offset+range; ++n)
		{
			if (!InSumSelection(listForSelection[n]))
			{
				AddToSumSelection(listForSelection[n]);
			}
		}
	}

	public void AddAllDisplayedToSumSelection(FileFilterGroup fileFilters)
	{
		BuildReportTool.SizePart[] listForSelection = GetListToDisplay(fileFilters);

		for (int n = 0; n < listForSelection.Length; ++n)
		{
			if (!InSumSelection(listForSelection[n]))
			{
				AddToSumSelection(listForSelection[n]);
			}
		}
	}

	double GetSizeOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			if (pair.Value.UsableSize > 0)
			{
				total += pair.Value.UsableSize;
			}
		}
		return total;
	}

	public double GetPercentageOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			if (pair.Value.Percentage > 0)
			{
				if (pair.Value.Percentage > 0)
				{
					total += pair.Value.Percentage;
				}
			}
		}
		return total;
	}

	public string GetReadableSizeOfSumSelection()
	{
		return BuildReportTool.Util.GetBytesReadable( GetSizeOfSumSelection() );
	}

	public bool AtLeastOneSelectedForSum
	{
		get
		{
			return _selectedForSum.Count > 0;
		}
	}

	public bool IsNothingSelected
	{
		get
		{
			return _selectedForSum.Count <= 0;
		}
	}

	public int GetSelectedCount()
	{
		return _selectedForSum.Count;
	}

	public void ClearSelection()
	{
		_selectedForSum.Clear();
	}
}

[System.Serializable]
public class BuildInfo
{
	public string ProjectName;
	public string BuildType;

	public DateTime TimeGot;
	public string TimeGotReadable;

	public string GetTimeReadable()
	{
		if (!string.IsNullOrEmpty(TimeGotReadable))
		{
			return TimeGotReadable;
		}
		return TimeGot.ToString(BuildReportTool.ReportManager.TIME_OF_BUILD_FORMAT);
	}

	public BuildReportTool.SizePart[] BuildSizes;



	// total sizes
	// ==================================================================================

	// old size values were only TotalBuildSize and CompressedBuildSize

	public bool HasOldSizeValues
	{
		get
		{
			return
				string.IsNullOrEmpty(UnusedTotalSize) &&
				string.IsNullOrEmpty(UsedTotalSize) &&
				string.IsNullOrEmpty(StreamingAssetsSize) &&
				string.IsNullOrEmpty(WebFileBuildSize) &&
				string.IsNullOrEmpty(AndroidApkFileBuildSize) &&
				string.IsNullOrEmpty(AndroidObbFileBuildSize);
		}
	}

	public string CompressedBuildSize = ""; // not used anymore

	public string UnusedTotalSize = "";
	public string UsedTotalSize = "";
	public string StreamingAssetsSize = "";

	public string TotalBuildSize = "";


	// per-platform sizes
	public string WebFileBuildSize = "";
	public string AndroidApkFileBuildSize = "";
	public string AndroidObbFileBuildSize = "";





	// file entries
	// ==================================================================================

	public BuildReportTool.SizePart[] MonoDLLs;
	public BuildReportTool.SizePart[] ScriptDLLs;

	public FileFilterGroup FileFilters = null;

	public AssetList UsedAssets = null;
	public AssetList UnusedAssets = null;

	public bool HasUsedAssets
	{
		get
		{
			return UsedAssets != null;
		}
	}
	public bool HasUnusedAssets
	{
		get
		{
			return UnusedAssets != null;
		}
	}



	// unity/os environment values at time of build report creation
	// ==================================================================================

	public string UnityVersion = "";
	public string EditorAppContentsPath = "";
	public string ProjectAssetsPath = "";
	public string BuildFilePath = "";
	public string[] ScenesIncludedInProject;
	public string[] PrefabsUsedInScenes;
	public StrippingLevel CodeStrippingLevel;
	public ApiCompatibilityLevel MonoLevel;
	public bool AndroidUseAPKExpansionFiles = false;
	public bool AndroidCreateProject = false;


	// build report tool options values at time of building
	// ==================================================================================

	public bool IncludedSvnInUnused;
	public bool IncludedGitInUnused;

	public bool UsedAssetsIncludedInCreation;
	public bool UnusedAssetsIncludedInCreation;
	public bool UnusedPrefabsIncludedInCreation;

	public int UnusedAssetsEntriesPerBatch;





	// temp variables that are not serialized into the XML file
	// ==================================================================================

	// only used while generating the build report or opening one

	int _unusedAssetsBatchNum = 0;

	public int UnusedAssetsBatchNum { get{ return _unusedAssetsBatchNum; } }
	public void MoveUnusedAssetsBatchNumToNext()
	{
		++_unusedAssetsBatchNum;
	}
	public void MoveUnusedAssetsBatchNumToPrev()
	{
		if (_unusedAssetsBatchNum == 0)
		{
			return;
		}
		--_unusedAssetsBatchNum;
	}

	string _savedPath;

	public string SavedPath { get{ return _savedPath; } }
	public void SetSavedPath(string val)
	{
		_savedPath = val;
	}



	BuildTarget _buildTarget;

	public BuildTarget BuildTargetUsed { get{ return _buildTarget; } }
	public void SetBuildTargetUsed(BuildTarget val)
	{
		_buildTarget = val;
	}



	public string GetDefaultFilename()
	{
		return ProjectName + "-" + BuildType + TimeGot.ToString("-yyyyMMMdd-HHmmss") + ".xml";
	}


	bool _refreshRequest;

	public void FlagOkToRefresh()
	{
		_refreshRequest = true;
	}

	public void FlagFinishedRefreshing()
	{
		_refreshRequest = false;
	}

	public bool RequestedToRefresh { get{ return _refreshRequest; } }





	// ==================================================================================


	public bool HasContents
	{
		get
		{
			return !string.IsNullOrEmpty(ProjectName);
		}
	}

	public void UnescapeAssetNames()
	{
		if (UsedAssets != null)
		{
			UsedAssets.UnescapeAssetNames();
		}

		if (UnusedAssets != null)
		{
			UnusedAssets.UnescapeAssetNames();
		}
	}

	public void RecategorizeAssetLists()
	{
		FileFilterGroup fileFiltersToUse = FileFilters;

		if (BuildReportTool.Options.ShouldUseConfiguredFileFilters())
		{
			fileFiltersToUse = BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUse();
			//Debug.Log("going to use configured file filters instead... loaded: " + (fileFiltersToUse != null));
		}

		if (UsedAssets != null)
		{
			UsedAssets.AssignPerCategoryList( BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(UsedAssets.All, fileFiltersToUse) );

			UsedAssets.RefreshFilterLabels(fileFiltersToUse);
		}

		if (UnusedAssets != null)
		{
			UnusedAssets.AssignPerCategoryList( BuildReportTool.ReportManager.SegregateAssetSizesPerCategory(UnusedAssets.All, fileFiltersToUse) );

			UnusedAssets.RefreshFilterLabels(fileFiltersToUse);
		}
	}

	void CalculateUsedAssetsDerivedSizes()
	{
		if (UsedAssets != null)
		{
			for (int n = 0, len = UsedAssets.All.Length; n < len; ++n)
			{
				UsedAssets.All[n].DerivedSize = BuildReportTool.Util.GetApproxSizeFromString(UsedAssets.All[n].Size);
			}
		}
	}

	public void OnDeserialize()
	{
		if (HasContents)
		{
			CalculateUsedAssetsDerivedSizes();
			UnescapeAssetNames();
			RecategorizeAssetLists();
		}
	}
}

} // namespace BuildReportTool
