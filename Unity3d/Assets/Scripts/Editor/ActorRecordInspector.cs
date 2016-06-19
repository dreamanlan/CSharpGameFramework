using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using GameFramework;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
    }
}

[CustomEditor(typeof(ActorRecord))]
public class ActorRecordInspector : Editor
{
    public override void OnInspectorGUI()
    {
        const string c_Resources = "Resources/";
        const string c_Ext = ".prefab";
        ActorRecord record = (ActorRecord)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("类型");
        record.type = EditorGUILayout.IntPopup(record.type, new string[] { "0--普通", "1--塔", "2--英雄", "3--BOSS", "4--技能", "5--基地" }, new int[] { 0, 1, 2, 3, 4, 5 });
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("模型");
        Rect rect = GUILayoutUtility.GetLastRect();
        float w = rect.width;
        //Debug.LogFormat("rect:{0} {1} {2} {3}", rect.x, rect.y, rect.width, rect.height);
        
        rect.width = 60;
        rect.x += 40;
        record.avatarFilter = EditorGUI.TextField(rect, record.avatarFilter);

        string[] guids = AssetDatabase.FindAssets("t:Prefab," + record.avatarFilter);
        string[] paths = new string[guids.Length];
        for (int i = 0; i < guids.Length; ++i) {
            paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }
        int index = System.Array.FindIndex(paths, s => s.EndsWith(record.avatar + c_Ext));
        int ix = EditorGUILayout.Popup(index, paths);
        if (ix >= 0 && ix < paths.Length) {
            string avatar = paths[ix];
            int startIndex = avatar.IndexOf(c_Resources);
            if (startIndex >= 0) {
                record.avatar = avatar.Substring(startIndex + c_Resources.Length, avatar.Length - startIndex - c_Resources.Length - c_Ext.Length);
            } else {
                record.avatar = avatar;
            }
        }

        EditorGUILayout.LabelField(record.avatar);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("小图标:"+record.icon);
        Sprite sprite1 = SpriteManager.GetActorIcon(record.icon);
        Sprite sprite10 = EditorGUILayout.ObjectField(sprite1, typeof(Sprite), false) as Sprite;
        if (sprite10 != sprite1) {
            record.icon = SpriteManager.GetActorIconIndex(sprite10);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("大图标:" + record.bigIcon);
        Sprite sprite2 = SpriteManager.GetActorBigIcon(record.bigIcon);
        Sprite sprite20 = EditorGUILayout.ObjectField(sprite2, typeof(Sprite), false) as Sprite;
        if (sprite20 != sprite2) {
            record.bigIcon = SpriteManager.GetActorBigIconIndex(sprite20);
        }
        EditorGUILayout.EndHorizontal();        
        DrawDefaultInspector();
    }
}
