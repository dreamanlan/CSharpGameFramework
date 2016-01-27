#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BuildReportTool
{

public class FiltersUsed
{
	static FileFilterGroup _defaultFileFilters = new FileFilterGroup(CreateDefaultFileFilters());

	static FileFilterGroup GetDefaultFileFilterGroup()
	{
		return _defaultFileFilters;
	}

	static FileFilters[] CreateDefaultFileFilters()
	{
		return new FileFilters[]
		{
			new FileFilters("Textures",
				new string[]{
				".psd",
				".jpg",
				".jpeg",
				".gif",
				".png",
				".tiff",
				".tif",
				".tga",
				".bmp",
				".dds",
				".exr",
				".iff",
				".pict",
				"Built-in Texture2D:",
			}),
			new FileFilters("Models",
				new string[]{
				".fbx",
				".dae",
				".mb",
				".ma",
				".max",
				".blend",
				".obj",
				".3ds",
				".dxf",
			}),
			new FileFilters("Prefabs",
				new string[]{
				".prefab",
			}),
			new FileFilters("Animation",
				new string[]{
				".anim",
				".controller",
				".mask",
			}),
			new FileFilters("Movies",
				new string[]{
				".mov",
				".mpg",
				".mpeg",
				".mp4",
				".avi",
				".asf",
			}),
			new FileFilters("Materials",
				new string[]{
				".mat",
				".sbsar",
				".cubemap",
				".flare",
				"Built-in Material:",
			}),
			new FileFilters("Shaders",
				new string[]{
				".shader",
				".compute",
				".cginc",
				"Built-in Shader:",
			}),
			new FileFilters("GUI",
				new string[]{
				".guiskin",
				".fontsettings",
				".ttf",
				".dfont",
				".otf",
			}),
			new FileFilters("Sounds",
				new string[]{
				".wav",
				".mp3",
				".ogg",
				".aif",
				".xm",
				".mod",
				".it",
				".s3m",
			}),
			new FileFilters("Scripts",
				new string[]{
				".cs",
				".js",
				".boo",
			}),
			new FileFilters("Plugins",
				new string[]{
				".dll", // Windows
				".bundle", // Mac
				".so", // Android (C++) or Linux
				".jar", // Android (Java)
				".a", // iOS
				".m", // iOS
				".mm", // iOS
				".c", // iOS
				".cpp", // iOS
			}),
			new FileFilters("Text",
				new string[]{
				".txt",
				".bytes",
				".html",
				".htm",
				".xml",
				".log",
			}),
			new FileFilters("Misc",
				new string[]{
				".asset",
				".physicmaterial",
				".unity",
			}),
			new FileFilters("Standard Assets",
				new string[]{
				"/Standard Assets/",
			}),
			new FileFilters("Streaming Assets",
				new string[]{
				"Assets/StreamingAssets/",
			}),
			new FileFilters("Editor",
				new string[]{
				"/Editor/",
			}),
			new FileFilters("Version Control",
				new string[]{
				"/.svn/",
				"/.git/",
				"/.cvs/",
			}),
			new FileFilters("Built-in Assets",
				new string[]{
				"Built-in",
			}),
			new FileFilters("Useless Files",
				new string[]{
				"\"Thumbs.db\"",
				"\".DS_Store\"",
				"\"._.DS_Store\"",
			}),
		};

	}

	static void SaveFileFilterGroupToFile(string saveFilePath, FileFilterGroup filterGroup)
	{
		XmlSerializer x = new XmlSerializer( typeof(FileFilterGroup) );
		
		saveFilePath = saveFilePath.Replace("\\", "/");
		
		TextWriter writer = new StreamWriter(saveFilePath);
		x.Serialize(writer, filterGroup);
		writer.Close();

		Debug.Log("Build Report Tool: Saved File Filter Group at \"" + saveFilePath + "\"");
	}

	static FileFilterGroup AttemptLoadFileFiltersFromFile(string filePath)
	{
		FileFilterGroup ret = null;

		XmlSerializer x = new XmlSerializer( typeof(FileFilterGroup) );

		using(FileStream fs = new FileStream(filePath, FileMode.Open))
		{
			XmlReader reader = new XmlTextReader(fs);
			ret = (FileFilterGroup)x.Deserialize(reader);
			fs.Close();
		}

		return ret;
	}

	const string FILE_FILTERS_USED_FILENAME = "FileFiltersUsed.xml";

	public static string GetProperFileFilterGroupToUseFilePath()
	{
		return GetProperFileFilterGroupToUseFilePath(BuildReportTool.Options.BuildReportSavePath);
	}

	public static string GetProperFileFilterGroupToUseFilePath(string userFileFilterSavePath)
	{
		// attempt to get from Assets/BuildReport/Config/FileFiltersUsed.xml
		// if none, attempt to get from ~/UnityBuildReports/FileFiltersUsed.xml
		// if no dice, create a new FileFiltersUsed.xml in ~/UnityBuildReports/ and use that
		
		// attempt to get from default Build Report Tool folder: Assets/BuildReport/Config/FileFiltersUsed.xml

		string fileFilterGroupAtDefaultAssetsPath = BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_PATH + "/" + FILE_FILTERS_USED_FILENAME;

		if (File.Exists(fileFilterGroupAtDefaultAssetsPath))
		{
			return fileFilterGroupAtDefaultAssetsPath;
		}


		// search for Build Report Tool folder in all subfolders of Assets folder and look for file there
		// maybe shouldn't do this? it's recursive and could be slow on project with hundreds of folders...
/*
		string assetFolderPath = BuildReportTool.Util.FindAssetFolder(Application.dataPath, BuildReportTool.Config.BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);
		if (!string.IsNullOrEmpty(assetFolderPath))
		{
			string fileFilterGroupAtFoundAssetsPath = assetFolderPath + "/" + FILE_FILTERS_USED_FILENAME;

			if (File.Exists(fileFilterGroupAtFoundAssetsPath))
			{
				return fileFilterGroupAtFoundAssetsPath;
			}
		}
*/

		string fileFilterGroupAtUserPersonalFolder = userFileFilterSavePath + "/" + FILE_FILTERS_USED_FILENAME;
		if (File.Exists(fileFilterGroupAtUserPersonalFolder))
		{
			//Debug.Log("will use file filter from user folder: " + fileFilterGroupAtUserPersonalFolder);
			return fileFilterGroupAtUserPersonalFolder;
		}

		string fileFilterGroupAtUserPersonalFolderDefaultName = BuildReportTool.Util.GetUserHomeFolder() + "/" + BuildReportTool.Options.BUILD_REPORTS_DEFAULT_FOLDER_NAME + "/" + FILE_FILTERS_USED_FILENAME;
		if (File.Exists(fileFilterGroupAtUserPersonalFolderDefaultName))
		{
			//Debug.Log("will use file filter from default user folder: " + fileFilterGroupAtUserPersonalFolderDefaultName);
			return fileFilterGroupAtUserPersonalFolderDefaultName;
		}

		// no dice. create a file filter group xml file at user personal folder
		if (!Directory.Exists(userFileFilterSavePath))
		{
			Debug.Log("Created a new Build Report File Filter Config XML File at " + userFileFilterSavePath);
			Directory.CreateDirectory(userFileFilterSavePath);
		}
		SaveFileFilterGroupToFile(fileFilterGroupAtUserPersonalFolder, _defaultFileFilters);
		return fileFilterGroupAtUserPersonalFolder;
	}




	public static FileFilterGroup GetProperFileFilterGroupToUse()
	{
		return GetProperFileFilterGroupToUse(BuildReportTool.Options.BuildReportSavePath);
	}

	public static FileFilterGroup GetProperFileFilterGroupToUse(string userFileFilterSavePath)
	{
		string fileFilterGroupPath = GetProperFileFilterGroupToUseFilePath(userFileFilterSavePath);

		//Debug.Log("fileFilterGroupPath: " + fileFilterGroupPath);

		FileFilterGroup ret = AttemptLoadFileFiltersFromFile(fileFilterGroupPath);

		if (ret != null)
		{
			return ret;
		}

		Debug.LogError("Build Report Tool: Could not find proper File Filter Group to use.");
		return null;
	}
}

} // namespace BuildReportTool

#endif