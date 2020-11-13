using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class TextureImporterParamsDB : ScriptableObject
{
    public ResourceParamsDB Data
    {
        get {
            return m_Data;
        }
    }
    [SerializeField]
    private ResourceParamsDB m_Data = new ResourceParamsDB();

    public static string PATH = "Assets/Editor/ResourceDBProcesser/DB/TextureImporterParamsDB.asset";
	public const string SYNC_ALL = "同步所有参数到资源";
    public const string IMPORT_DIALOG = "打开纹理设置数据文件";
    public const string IMPORT = "导入";
    public const string EXPORT_DIALOG = "保存纹理设置数据文件";
    public const string EXPORT = "导出";
    public const string FETCH = "从服务器更新";
    public const string COMMIT = "提交到服务器";

    [MenuItem("工具/AssetTool/CreateTextureImporterParams DB", false, 100)]
	public static void CreateAsset()
	{
		TextureImporterParamsDB param = ScriptableObject.CreateInstance<TextureImporterParamsDB>();
		ProjectWindowUtil.CreateAsset(param, PATH);
	}

	public static void UpdateAllTextures()
    {
        try {
            var count = DBInstance.Data.Count;
            for (int i = 0; i < count; ++i) {
                string guid = DBInstance.Data.GetResourceKey(i);
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UpdateTexture(assetPath, DBInstance.Data.GetResourceParams(i), false);
                if (EditorUtility.DisplayCancelableProgressBar("特殊纹理处理进度", string.Format("{0}/{1}", i, count), i * 1.0f / count)) {
                    return;
                }
            }
        } finally {
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
    }

    public static void Import(string file)
    {
        try {
            var json = File.ReadAllText(file);
            var db = JsonUtility.FromJson<ResourceParamsDB>(json);
            DBInstance.Data.Clear();
            DBInstance.Data.Merge(db);
            EditorUtility.SetDirty(DBInstance);
            AssetDatabase.SaveAssets();
            Debug.Log("Import ok.");
        } catch (Exception ex) {
            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public static void Export(string file)
    {
        try {
            DBInstance.Data.Upgrade();
            var json = JsonUtility.ToJson(DBInstance.Data, true);
            File.WriteAllText(file, json);
            Debug.Log("Export ok.");
        } catch (Exception ex) {
            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    public static void Fetch()
    {
        var ret = ResourceEditGetPoster.Fetch("TextureImporterParamsDB", DBInstance.Data);
    }

    public static void Commit()
    {
        var ret = ResourceEditGetPoster.Commit("TextureImporterParamsDB", DBInstance.Data);
    }

    public static bool UpdateTexture(string assetPath, ResourceParams param, bool onlySetting)
	{
		TextureImporter textureImporter = TextureImporter.GetAtPath(assetPath) as TextureImporter;
		if (textureImporter == null)
		{
			Debug.LogErrorFormat("no texture importer at {0}", assetPath);
			return false;
		}
        UpdateTexture(param, textureImporter, onlySetting);
		return true;
    }
    public static bool UpdateTexture(string path, TextureImporter textureImporter, bool onlySetting)
    {
        bool ret = false;
        ResourceParams param;
        if (DBInstance.Data.TryGetResourceParams(path, out param)) {
            ret = UpdateTexture(param, textureImporter, onlySetting);
        }
        return ret;
    }
    private static bool UpdateTexture(ResourceParams param, TextureImporter textureImporter, bool onlySetting)
    {
        bool rt = ResourceEditUtility.SetParamsToResource("setparamstotexture", param, textureImporter);
        rt = rt || ResourceEditUtility.ForceSaveAndReimport;
        if (!onlySetting && rt) {
            if (ResourceEditUtility.EnableSaveAndReimport) {
                //textureImporter.SaveAndReimport();
                AssetDatabase.ImportAsset(textureImporter.assetPath);
            }
        }
        return true;
    }

    public static bool UpdateDB(string assetPath, TextureImporter textureImporter)
    {
        ResourceParams param;
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        if (!DBInstance.Data.TryGetResourceParams(guid, out param)) {
            param = new ResourceParams();
            param.Resource = assetPath;
            DBInstance.Data.AddOrMerge(guid, param);
        } else {

        }
        bool rt = ResourceEditUtility.GetParamsFromResource("getparamsfromtexture", param, textureImporter);
        return rt;
    }

    public static TextureImporterParamsDB DBInstance
    {
        get {
            if (m_DBInstance == null)
                m_DBInstance = AssetDatabase.LoadAssetAtPath<TextureImporterParamsDB>(PATH);
            return m_DBInstance;
        }
    }
    private static TextureImporterParamsDB m_DBInstance;
}


