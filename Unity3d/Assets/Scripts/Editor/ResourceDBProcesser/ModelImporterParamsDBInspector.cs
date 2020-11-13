using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModelImporterParamsDB))]
public class FBXImporterParamsDBInspector : Editor
{
    bool show = false;
    const string EMPTY = "无数据";
    const string SELECT = "选择";
    const string UPDATERES = "更新资源";
    const string UPDATEDB = "更新参数库";
    const string VALID = "无效数据";
    const string DELETE = "删除";
    const string ADD = "添加新资源";

    const string NOT_FOUND_TITLE = "找不到资源";
    const string NOT_FOUND_CONTEXT = "{0}已经找不到，是否删除该数据？";
    const string OK = "删除";
    const string NO = "忽略";

    UnityEngine.Object objToAdd = null;
    List<bool> foldout = new List<bool>();
    List<string> toDel = new List<string>();
    public override void OnInspectorGUI()
    {
        bool saveDB = false;
        toDel.Clear();
        ModelImporterParamsDB db = target as ModelImporterParamsDB;
        int count = db.Data.Count;
        if (count == 0) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EMPTY);
            GUILayout.EndHorizontal();
        }
        else {
            for (int i = 0; i < count; ++i) {
                string guid = db.Data.GetResourceKey(i);
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ResourceParams param = db.Data.GetResourceParams(i);
                bool valid = !string.IsNullOrEmpty(path) && param != null;
                if (!valid) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(VALID);
                    if (GUILayout.Button(DELETE)) {
                        toDel.Add(guid);
                        saveDB = true;
                    }
                    GUILayout.EndHorizontal();
                }
                else {
                    if (foldout.Count < i + 1)
                        foldout.Add(false);
                    var pathArr = path.Split('/');
                    if (EditorGUILayout.Foldout(foldout[i], i + ":" + pathArr[pathArr.Length - 1].ToString())) {
                        foldout[i] = true;
                        for (int ix = 0; ix < param.Count; ++ix) {
                            var key = param.GetKey(ix);
                            var val = param.GetValue(ix);
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(string.Format("{0}:{1}", key, val));
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(SELECT)) {
                            var asset = AssetDatabase.LoadMainAssetAtPath(path);
                            if (asset == null) {
                                if (EditorUtility.DisplayDialog(NOT_FOUND_TITLE, string.Format(NOT_FOUND_CONTEXT, path), OK, NO)) {
                                    toDel.Add(guid);
                                    saveDB = true;
                                }
                            }
                            else {
                                Selection.activeObject = asset;
                            }
                        }
                        if (GUILayout.Button(UPDATERES)) {
                            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
                            if (null != importer) {
                                ModelImporterParamsDB.UpdateModel(path, importer, false);
                                AssetDatabase.ImportAsset(path);
                            }
                        }
                        if (GUILayout.Button(UPDATEDB)) {
                            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
                            if (null != importer) {
                                ModelImporterParamsDB.UpdateDB(path, importer);
                                saveDB = true;
                            }
                        }
                        if (GUILayout.Button(DELETE)) {
                            toDel.Add(guid);
                            saveDB = true;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else {
                        foldout[i] = false;
                    }
                }
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ModelImporterParamsDB.SYNC_ALL)) {
                ResourceEditUtility.ResetResourceParamsCalculator();
                ModelImporterParamsDB.UpdateAllModels();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        objToAdd = EditorGUILayout.ObjectField(objToAdd, typeof(UnityEngine.GameObject), false);
        if (GUILayout.Button(ADD) && null != objToAdd) {
            var path = AssetDatabase.GetAssetPath(objToAdd);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (null != importer) {
                ModelImporterParamsDB.UpdateDB(path, importer);
                saveDB = true;
            }
        }
        GUILayout.EndHorizontal();

        foreach (var del in toDel)
            db.Data.Remove(del);

        if (saveDB) {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ModelImporterParamsDB.IMPORT)) {
            string file = EditorUtility.OpenFilePanel(ModelImporterParamsDB.IMPORT_DIALOG, string.Empty, "json");
            if (!string.IsNullOrEmpty(file)) {
                ModelImporterParamsDB.Import(file);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ModelImporterParamsDB.EXPORT)) {
            string file = EditorUtility.SaveFilePanel(ModelImporterParamsDB.EXPORT_DIALOG, string.Empty, "modeldata", "json");
            if (!string.IsNullOrEmpty(file)) {
                ModelImporterParamsDB.Export(file);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ModelImporterParamsDB.FETCH)) {
            ModelImporterParamsDB.Fetch();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ModelImporterParamsDB.COMMIT)) {
            ModelImporterParamsDB.Commit();
        }
        GUILayout.EndHorizontal();
    }
}
