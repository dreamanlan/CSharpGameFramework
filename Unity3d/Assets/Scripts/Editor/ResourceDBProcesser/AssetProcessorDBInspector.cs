using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetProcessorDB))]
public class AssetProcessorDBInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var db = target as AssetProcessorDB;
        var list = db.Processers;

        var listProp = serializedObject.FindProperty("m_Processers");

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("从服务器更新")) {
            AssetProcessorDB.Fetch();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("提交到服务器")) {
            AssetProcessorDB.Commit();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("filter:");
        m_Filter = EditorGUILayout.TextField(m_Filter);
        if (GUILayout.Button("拷贝")) {
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; ++i) {
                var info = list[i];
                info.PrepareFiltersAndProcesses();
                if (info.ResPath.Contains(m_Filter)) {
                    sb.AppendFormat("Path:{0}", info.ResPath);
                    sb.AppendLine();
                    sb.AppendFormat("    (From Dsl:{0})", info.DslPath);
                    sb.AppendLine();
                    sb.AppendLine("    Filters:");
                    foreach (var filter in info.Filters) {
                        sb.AppendFormat("        {0}", filter);
                        sb.AppendLine();
                    }

                    sb.AppendLine("    Processes:");

                    foreach (var e in info.Processes) {
                        if (e != AssetProcessorEnum.SetDirty && e != AssetProcessorEnum.SaveAndReimport) {
                            sb.AppendFormat("        {0}", e);
                            sb.AppendLine();
                        }
                    }
                }
            }
            GUIUtility.systemCopyBuffer = sb.ToString();
        }
        EditorGUILayout.EndHorizontal();

        for(int i=0;i<list.Count;++i) {
            var info = list[i];
            info.PrepareFiltersAndProcesses();
            if (info.ResPath.Contains(m_Filter)) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Path:", GUILayout.Width(32));
                EditorGUILayout.LabelField(info.ResPath);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("    (From Dsl:" + info.DslPath + ")");
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("    Filters:");
                EditorGUILayout.EndHorizontal();

                foreach (var filter in info.Filters) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("        " + filter);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("    Processes:");
                EditorGUILayout.EndHorizontal();

                foreach (var e in info.Processes) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("        " + e);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    private string m_Filter = string.Empty;
}
