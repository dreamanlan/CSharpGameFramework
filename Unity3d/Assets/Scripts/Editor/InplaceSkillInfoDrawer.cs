using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework;
using GameFramework.Skill;
using SkillSystem;

[CustomEditor(typeof(InplaceSkillInfo))]
public class InplaceSkillInfoInspector : Editor
{
    void OnEnable()
    {
        EditorApplication.update += this.OnUpdate;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("拷贝到剪贴板")) {
            InplaceSkillInfo info = target as InplaceSkillInfo;
            if (null != info) {
                var props = info.Properties;
                StringBuilder sb = new StringBuilder();
                foreach (var pair in props) {
                    int skillId = pair.Key;
                    var group = pair.Value;
                    sb.AppendFormat("========================dsl:{0}========================", skillId);
                    sb.AppendLine();
                    if (null != group.PropertyList) {
                        string lastGroup = string.Empty;
                        foreach (var prop in group.PropertyList) {
                            if (!string.IsNullOrEmpty(lastGroup) && lastGroup != prop.Group) {
                                sb.AppendLine();
                            }
                            if (null != prop.Property && null != prop.Property.Value && !(prop.Property.Value is AnimationCurve)) {
                                sb.AppendFormat("{0}:{1}\t{2}", prop.Group, prop.Key, prop.Property.Value.ToString());
                                sb.AppendLine();
                            } else {
                                sb.AppendFormat("{0}:{1}", prop.Group, prop.Key);
                                sb.AppendLine();
                            }
                            lastGroup = prop.Group;
                        }
                    }
                    sb.AppendLine();
                    TableConfig.Skill skillCfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != skillCfg) {
                        sb.AppendLine("=====================SkillResources=====================");
                        foreach (var resPair in skillCfg.resources) {
                            sb.AppendFormat("{0}\t{1}\t{2}", skillId, resPair.Key, resPair.Value);
                            sb.AppendLine();
                        }
                        sb.AppendLine("====================EndSkillResources===================");
                    }
                }
                string text = sb.ToString();
                TextEditor editor = new TextEditor();
                editor.text = text;
                //editor.content = new GUIContent(text);
                editor.OnFocus();
                editor.Copy();
            }
        }
    }

    private void OnUpdate()
    {
        Repaint();
    }
}

[CustomPropertyDrawer(typeof(InplaceSkillInfo.CustomInfo))]
public class InplaceSkillInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, 80, c_FieldHeight), property.isExpanded, label);
        if (property.isExpanded) {
            float w = position.width;
            position.width = c_LabelWidth;
            position.height = c_FieldHeight;
            float x = position.x;
            float y = position.y;
            EditorGUI.LabelField(position, label);

            InplaceSkillInfo info = property.serializedObject.targetObject as InplaceSkillInfo;
            Dictionary<int, InplaceSkillInfo.InplaceSkillPropertyInfoGroup> dict = info.Properties;
            foreach (var pair in dict) {
                int skillId = pair.Key;
                var group = pair.Value;
                position.x = x;
                position.y += c_FieldHeight;
                group.IsFoldOut = EditorGUI.Foldout(position, group.IsFoldOut, "============<" + skillId + ">============");
                if (group.IsFoldOut) {
                    //Skill visual editing features section
                    if (null != group.PropertyList) {
                        string lastGroup = string.Empty;
                        foreach (var p in group.PropertyList) {
                            position.x = x;
                            position.y += c_FieldHeight;
                            position.width = c_LabelWidth;
                            if (lastGroup == p.Group) {
                                EditorGUI.LabelField(position, string.Empty);
                            } else {
                                EditorGUI.LabelField(position, GetIndentLabel(p.Group));
                            }

                            position.x += c_LabelWidth;
                            position.width = 160;
                            EditorGUI.LabelField(position, null == p.Key ? string.Empty : p.Key);

                            var prop = p.Property;
                            if (null != prop && prop.Value is AnimationCurve) {
                                float tx = position.x;
                                position.x += 160;
                                EditorGUI.BeginChangeCheck();
                                object v = EditorGUI.CurveField(position, prop.Value as AnimationCurve);
                                if (EditorGUI.EndChangeCheck()) {
                                    prop.Value = v;
                                }
                                position.x = tx + 120;
                                position.width = 40;
                                if (GUI.Button(position, "拷贝")) {
                                    AnimationCurve curve = v as AnimationCurve;
                                    if (null != curve) {
                                        StringBuilder sb = new StringBuilder();
                                        for (int i = 0; i < curve.keys.Length; ++i) {
                                            Keyframe key = curve.keys[i];
                                            sb.AppendFormat("keyframe({0},{1},{2},{3});", key.time, key.value, key.inTangent, key.outTangent);
                                            sb.AppendLine();
                                        }
                                        string text = sb.ToString();
                                        TextEditor editor = new TextEditor();
                                        editor.text = text;
                                        //editor.content = new GUIContent(text);
                                        editor.OnFocus();
                                        editor.Copy();
                                    }
                                }
                            } else {
                                position.x += 160;
                                EditProperty(position, p.Property);
                            }

                            lastGroup = p.Group;
                        }
                    }
                    //Skill resources (new resources are not allowed here, only modifications can be made)
                    TableConfig.Skill skillCfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != skillCfg) {
                        position.x = x;
                        position.y += c_FieldHeight;
                        position.width = c_LabelWidth;
                        EditorGUI.LabelField(position, "==================");

                        position.x += c_LabelWidth;
                        position.width = 160;
                        EditorGUI.LabelField(position, "SkillResources");

                        position.x += 160;
                        EditorGUI.LabelField(position, "============");

                        bool first = true;
                        Dictionary<string, string> modifiedResources = new Dictionary<string, string>();
                        foreach (var resPair in skillCfg.resources) {
                            string key = resPair.Key;
                            string path = resPair.Value;
                            position.x = x;
                            position.y += c_FieldHeight;
                            position.width = c_LabelWidth - 80;
                            if (first) {
                                EditorGUI.LabelField(position, ">>>resource");
                            } else {
                                EditorGUI.LabelField(position, string.Empty);
                            }
                            first = false;

                            position.width = 80;
                            position.x += c_LabelWidth - 80;
                            EditorGUI.LabelField(position, key);

                            position.width = 160;
                            position.x += 80; 
                            string[] guids = AssetDatabase.FindAssets("t:Prefab," + Path.GetFileName(path));
                            Object v = null;
                            for (int i = 0; i < guids.Length; ++i) {
                                string p = AssetDatabase.GUIDToAssetPath(guids[i]);
                                if (p.Contains("Resources/" + path)) {
                                    v = AssetDatabase.LoadAssetAtPath<GameObject>(p);
                                    break;
                                }
                            }
                            EditorGUI.BeginChangeCheck();
                            v = EditorGUI.ObjectField(position, v, typeof(GameObject), false);
                            if (EditorGUI.EndChangeCheck()) {
                                string respath = AssetDatabase.GetAssetPath(v.GetInstanceID());
                                int startIndex = respath.IndexOf(c_Resources);
                                if (startIndex >= 0) {
                                    path = respath.Substring(startIndex + c_Resources.Length, respath.Length - startIndex - c_Resources.Length - c_Ext.Length);
                                } else {
                                    path = respath;
                                }
                                modifiedResources.Add(key, path);
                            }

                            position.width = w - c_LabelWidth - 160;
                            position.x += 160;
                            EditorGUI.LabelField(position, path);
                        }
                        foreach (var resPair in modifiedResources) {
                            skillCfg.resources[resPair.Key] = resPair.Value;
                        }
                    }
                }
            }
        }
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded) {
            float height = c_FieldHeight;
            InplaceSkillInfo info = property.serializedObject.targetObject as InplaceSkillInfo;
            Dictionary<int, InplaceSkillInfo.InplaceSkillPropertyInfoGroup> dict = info.Properties;
            foreach (var pair in dict) {
                int skillId = pair.Key;
                var group = pair.Value;
                height += c_FieldHeight;
                if (group.IsFoldOut) {
                    if (null != group.PropertyList) {
                        foreach (var p in group.PropertyList) {
                            height += c_FieldHeight;
                        }
                    }
                    TableConfig.Skill skillCfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != skillCfg) {
                        height += c_FieldHeight * skillCfg.resources.Count + c_FieldHeight;
                    }
                }
            }
            return height;
        } else {
            return c_FieldHeight;
        }
    }
    private void EditProperty(Rect position, IProperty p)
    {
        if (null == p) {
            EditorGUI.LabelField(position, "============");
        } else {
            object val = p.Value;
            if (null == val) {
                EditorGUI.BeginChangeCheck();
                string nval = EditorGUI.TextField(position, string.Empty);
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            } else if (val is float) {
                EditorGUI.BeginChangeCheck();
                float nval = EditorGUI.FloatField(position, (float)val);
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            } else if (val is double) {
                EditorGUI.BeginChangeCheck();
                double nval = EditorGUI.DoubleField(position, (double)val);
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            } else if (val is int || val is uint) {
                EditorGUI.BeginChangeCheck();
                int nval = EditorGUI.IntField(position, (int)val);
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            } else if (val is long || val is ulong) {
                EditorGUI.BeginChangeCheck();
                long nval = EditorGUI.LongField(position, (long)val);
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            } else {
                EditorGUI.BeginChangeCheck();
                string nval = EditorGUI.TextField(position, val.ToString());
                if (EditorGUI.EndChangeCheck()) {
                    p.Value = nval;
                }
            }
        }
    }

    private static string GetIndentLabel(string group)
    {
        if (group.EndsWith("SkillInstance")) {
            return "==================";
        } else if (group.EndsWith("SkillSection") || group.EndsWith("SkillMessageHandler")) {
            return "==================";
        } else {
            return ">>>" + group;
        }
    }

    private const string c_Resources = "Resources/";
    private const string c_Ext = ".prefab";
    private const float c_LabelWidth = 180;
    private const float c_FieldHeight = 16;
}

[CustomEditor(typeof(PrintCurve))]
public class PrintCurveInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("拷贝到剪贴板")) {
            PrintCurve p = target as PrintCurve;
            AnimationCurve curve = p.Curve as AnimationCurve;
            if (null != curve) {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < curve.keys.Length; ++i) {
                    Keyframe key = curve.keys[i];
                    sb.AppendFormat("keyframe({0},{1},{2},{3});", key.time, key.value, key.inTangent, key.outTangent);
                    sb.AppendLine();
                }
                string text = sb.ToString();
                TextEditor editor = new TextEditor();
                editor.text = text;
                //editor.content = new GUIContent(text);
                editor.OnFocus();
                editor.Copy();
            }
        }
    }
}