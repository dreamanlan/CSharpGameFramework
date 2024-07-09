using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework;

[CustomEditor(typeof(NpcConfigs))]
public class NpcConfigInspedtor : Editor
{
    NpcConfigs configs;
    public void OnEnable()
    {
        configs = (NpcConfigs)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("拷贝到剪贴板")) {
            List<NpcConfig> bornPoints = new List<NpcConfig>();
            configs.transform.GetComponentsInChildren<NpcConfig>(true, bornPoints);

            StringBuilder sb = new StringBuilder();            
            for (int i = 0; i < bornPoints.Count; i++) {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", bornPoints[i].CampId, bornPoints[i].ActorId, bornPoints[i].transform.position.x, bornPoints[i].transform.position.z, bornPoints[i].transform.rotation.eulerAngles.y, bornPoints[i].Level, bornPoints[i].IsPassive);
                sb.AppendLine();
            }
            string text = sb.ToString();
            TextEditor editor = new TextEditor();
            //editor.text = text;
            editor.content = new GUIContent(text);
            editor.OnFocus();
            editor.Copy();
        }
        GUILayout.EndHorizontal();
    }
}
