using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ModelImporterParamsDB : ScriptableObject
{
    public ResourceParamsDB Data
    {
        get {
            return m_Data;
        }
    }
    [SerializeField]
    private ResourceParamsDB m_Data = new ResourceParamsDB();

	public static string PATH = "Assets/Editor/ResourceDBProcesser/DB/ModelImporterParamsDB.asset";
	public const string SYNC_ALL = "同步所有参数到资源";
    public const string IMPORT_DIALOG = "打开模型设置数据文件";
    public const string IMPORT = "导入";
    public const string EXPORT_DIALOG = "保存模型设置数据文件";
    public const string EXPORT = "导出";
    public const string FETCH = "从服务器更新";
    public const string COMMIT = "提交到服务器";

    [MenuItem("工具/AssetTool/CreateModelImportParams DB", false, 100)]
	public static void CreateAsset()
	{
		ModelImporterParamsDB param = ScriptableObject.CreateInstance<ModelImporterParamsDB>();
		ProjectWindowUtil.CreateAsset(param, PATH);
	}

	public static void UpdateAllModels()
	{
        try {
            var count = DBInstance.Data.Count;
            for (int i = 0; i < count; ++i) {
                string guid = DBInstance.Data.GetResourceKey(i);
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UpdateModel(assetPath, DBInstance.Data.GetResourceParams(i), false);
                if (EditorUtility.DisplayCancelableProgressBar("特殊模型处理进度", string.Format("{0}/{1}", i, count), i * 1.0f / count)) {
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
        var ret = ResourceEditGetPoster.Fetch("ModelImporterParamsDB", DBInstance.Data);
    }

    public static void Commit()
    {   
        var ret = ResourceEditGetPoster.Commit("ModelImporterParamsDB", DBInstance.Data);
    }

    public static bool UpdateModel(string assetPath, ResourceParams param, bool onlySetting)
    {
        ModelImporter modelImporter = ModelImporter.GetAtPath(assetPath) as ModelImporter;
        if (modelImporter == null) {
            Debug.LogErrorFormat("no model importer at {0}", assetPath);
            return false;
        }
        return UpdateModel(param, modelImporter, onlySetting);
    }

    public static bool UpdateModel(string path, ModelImporter modelImporter, bool onlySetting)
    {
        bool ret = false;
        ResourceParams val;
        if (DBInstance.Data.TryGetResourceParams(path, out val)) {
            ret = UpdateModel(val, modelImporter, onlySetting);
        }
        return ret;
    }

    private static bool UpdateModel(ResourceParams param, ModelImporter modelImporter, bool onlySetting)
    {
        bool rt = ResourceEditUtility.SetParamsToResource("setparamstomodel", param, modelImporter);
        rt = rt || ResourceEditUtility.ForceSaveAndReimport;
        if (!onlySetting && rt) {
            if (ResourceEditUtility.EnableSaveAndReimport) {
                //modelImporter.SaveAndReimport();
                AssetDatabase.ImportAsset(modelImporter.assetPath);
            }
        }
        return true;
    }

    public static bool UpdateDB(string assetPath, ModelImporter modelImporter)
    {
        ResourceParams param;
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        if (!DBInstance.Data.TryGetResourceParams(guid, out param)) {
            param = new ResourceParams();
            param.Resource = assetPath;
            DBInstance.Data.AddOrMerge(guid, param);
        } else {

        }
        bool rt = ResourceEditUtility.GetParamsFromResource("getparamsfrommodel", param, modelImporter);
        return rt;
    }

    public static ModelImporterParamsDB DBInstance
    {
        get {
            if (m_DBInstance == null)
                m_DBInstance = AssetDatabase.LoadAssetAtPath<ModelImporterParamsDB>(PATH);
            return m_DBInstance;
        }
    }
    private static ModelImporterParamsDB m_DBInstance;
}
