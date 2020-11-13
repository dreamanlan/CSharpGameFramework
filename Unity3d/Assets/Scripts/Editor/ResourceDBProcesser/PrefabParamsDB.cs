using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class PrefabParamsDB : ScriptableObject
{
    public ResourceParamsDB Data
    {
        get {
            return m_Data;
        }
    }
    [SerializeField]
    private ResourceParamsDB m_Data = new ResourceParamsDB();

	public static string PATH = "Assets/Editor/ResourceDBProcesser/DB/PrefabParamsDB.asset";
	public const string SYNC_ALL = "同步所有参数到资源";
    public const string IMPORT_DIALOG = "打开Prefab设置数据文件";
    public const string IMPORT = "导入";
    public const string EXPORT_DIALOG = "保存Prefab设置数据文件";
    public const string EXPORT = "导出";
    public const string FETCH = "从服务器更新";
    public const string COMMIT = "提交到服务器";

    [MenuItem("工具/AssetTool/CreatePrefabParams DB", false, 100)]
	public static void CreateAsset()
	{
		PrefabParamsDB param = ScriptableObject.CreateInstance<PrefabParamsDB>();
		ProjectWindowUtil.CreateAsset(param, PATH);
	}

	public static void UpdateAllPrefabs()
	{
        try {
            var count = DBInstance.Data.Count;
            for (int i = 0; i < count; ++i) {
                string guid = DBInstance.Data.GetResourceKey(i);
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UpdatePrefab(assetPath, DBInstance.Data.GetResourceParams(i), false);
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
        var ret = ResourceEditGetPoster.Fetch("PrefabParamsDB", DBInstance.Data);
    }

    public static void Commit()
    {   
        var ret = ResourceEditGetPoster.Commit("PrefabParamsDB", DBInstance.Data);
    }

    public static bool UpdatePrefab(string assetPath, ResourceParams param, bool onlySetting)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(assetPath);
        if (prefab == null) {
            Debug.LogErrorFormat("can't load res at {0}", assetPath);
            return false;
        }
        return UpdatePrefab(param, prefab, onlySetting);
    }

    public static bool UpdatePrefab(string path, UnityEngine.GameObject prefab, bool onlySetting)
    {
        bool ret = false;
        ResourceParams val;
        if (DBInstance.Data.TryGetResourceParams(path, out val)) {
            ret = UpdatePrefab(val, prefab, onlySetting);
        }
        return ret;
    }

    private static bool UpdatePrefab(ResourceParams param, UnityEngine.GameObject prefab, bool onlySetting)
    {
        bool rt = ResourceEditUtility.SetParamsToResource("setparamstoprefab", param, prefab);
        rt = rt || ResourceEditUtility.ForceSaveAndReimport;
        if (!onlySetting && rt) {
            if (ResourceEditUtility.EnableSaveAndReimport) {
                EditorUtility.SetDirty(prefab);
            }
        }
        return true;
    }

    public static bool UpdateDB(string assetPath, UnityEngine.GameObject prefab)
    {
        ResourceParams param;
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        if (!DBInstance.Data.TryGetResourceParams(guid, out param)) {
            param = new ResourceParams();
            param.Resource = assetPath;
            DBInstance.Data.AddOrMerge(guid, param);
        } else {

        }
        bool rt = ResourceEditUtility.GetParamsFromResource("getparamsfromprefab", param, prefab);
        return rt;
    }

    public static PrefabParamsDB DBInstance
    {
        get {
            if (m_DBInstance == null)
                m_DBInstance = AssetDatabase.LoadAssetAtPath<PrefabParamsDB>(PATH);
            return m_DBInstance;
        }
    }
    private static PrefabParamsDB m_DBInstance;
}
