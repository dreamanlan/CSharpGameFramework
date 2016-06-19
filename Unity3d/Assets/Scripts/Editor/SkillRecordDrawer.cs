using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using GameFramework;

[CustomPropertyDrawer(typeof(SkillRecords.SkillRecord))]
public class SkillRecordDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, 80, c_FieldHeight), property.isExpanded, label);
        if (property.isExpanded) {

            SerializedProperty id = property.FindPropertyRelative("id");
            SerializedProperty desc = property.FindPropertyRelative("desc");
            SerializedProperty type = property.FindPropertyRelative("type");
            SerializedProperty icon = property.FindPropertyRelative("icon");
            SerializedProperty distance = property.FindPropertyRelative("distance");
            SerializedProperty duration = property.FindPropertyRelative("duration");
            SerializedProperty interval = property.FindPropertyRelative("interval");
            SerializedProperty canmove = property.FindPropertyRelative("canmove");
            SerializedProperty impact = property.FindPropertyRelative("impact");
            SerializedProperty interruptPriority = property.FindPropertyRelative("interruptPriority");
            SerializedProperty isInterrupt = property.FindPropertyRelative("isInterrupt");
            SerializedProperty targetType = property.FindPropertyRelative("targetType");
            SerializedProperty aoeType = property.FindPropertyRelative("aoeType");
            SerializedProperty aoeSize = property.FindPropertyRelative("aoeSize");
            SerializedProperty aoeAngleOrLength = property.FindPropertyRelative("aoeAngleOrLength");
            SerializedProperty maxAoeTargetCount = property.FindPropertyRelative("maxAoeTargetCount");
            SerializedProperty dslSkillId = property.FindPropertyRelative("dslSkillId");
            SerializedProperty dslFile = property.FindPropertyRelative("dslFile");
            SerializedProperty startupSkillId = property.FindPropertyRelative("startupSkillId");
            SerializedProperty flybackSkillId = property.FindPropertyRelative("flybackSkillId");
            SerializedProperty startupPositionType = property.FindPropertyRelative("startupPositionType");
            SerializedProperty subsequentSkills = property.FindPropertyRelative("subsequentSkills");
            SerializedProperty autoCast = property.FindPropertyRelative("autoCast");
            SerializedProperty needTarget = property.FindPropertyRelative("needTarget");
            SerializedProperty cooldown = property.FindPropertyRelative("cooldown");
            SerializedProperty damage = property.FindPropertyRelative("damage");
            SerializedProperty addhp = property.FindPropertyRelative("addhp");
            SerializedProperty addmp = property.FindPropertyRelative("addmp");
            SerializedProperty addattack = property.FindPropertyRelative("addattack");
            SerializedProperty adddefence = property.FindPropertyRelative("adddefence");
            SerializedProperty addshield = property.FindPropertyRelative("addshield");
            SerializedProperty addspeed = property.FindPropertyRelative("addspeed");
            SerializedProperty addcritical = property.FindPropertyRelative("addcritical");
            SerializedProperty addcriticalpow = property.FindPropertyRelative("addcriticalpow");
            SerializedProperty addrps = property.FindPropertyRelative("addrps");
            SerializedProperty skillres = property.FindPropertyRelative("resources");
                     
            int v = 0;
            float x = position.x;
            float y = position.y;
            EditorGUI.LabelField(position, label);

            position.width = position.width - c_LabelWidth;
            position.height = c_FieldHeight;
            float standardFieldWidth = position.width;
 
            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "技能类型");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            v = EditorGUI.IntPopup(position, type.intValue, new string[] { "0--技能", "1--效果", "2--BUFF" }, new int[] { 0, 1, 2 });
            if (EditorGUI.EndChangeCheck()) {
                type.intValue = v;
            }

            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "目标类型");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            v = EditorGUI.IntPopup(position, targetType.intValue, new string[] { "0--自己", "1--敌对", "2--友好", "3--随机敌人", "4--随机友好" }, new int[] { 0, 1, 2, 3, 4 });
            if (EditorGUI.EndChangeCheck()) {
                targetType.intValue = v;
            }

            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "AOE类型");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            v = EditorGUI.IntPopup(position, aoeType.intValue, new string[] { "0--非AOE", "1--圆形区域", "2--扇形区域", "3--胶囊区域", "4--矩形区域" }, new int[] { 0, 1, 2, 3, 4 });
            if (EditorGUI.EndChangeCheck()) {
                aoeType.intValue = v;
            }

            position.x = x;
            position.y += c_FieldHeight;
            label = new GUIContent("技能图标:" + icon.intValue);
            EditorGUI.LabelField(position, label);
            position.x += c_LabelWidth;
            //Debug.Log("icon:" + icon.intValue);
            Sprite sprite1 = SpriteManager.GetSkillIcon(icon.intValue);
            float w = 32;
            float h = 32;
            if (null != sprite1) {
                w = sprite1.rect.width;
                h = sprite1.rect.height;
                //Debug.Log("icon size:" + w + "," + h);
            }
            EditorGUI.BeginChangeCheck();
            sprite1 = EditorGUI.ObjectField(new Rect(position.x, position.y, w, h), sprite1, typeof(Sprite), false) as Sprite;
            if (EditorGUI.EndChangeCheck()) {
                icon.intValue = SpriteManager.GetSkillIconIndex(sprite1);
            }
            position.y += h - c_FieldHeight;

            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "id");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            int idVal = id.intValue;
            v = EditorGUI.IntField(position, idVal);
            if (EditorGUI.EndChangeCheck()) {
                id.intValue = v;
                dslSkillId.intValue = v;
            }

            SerializedProperty[] p = new SerializedProperty[] { desc, type, icon, distance, duration, interval, canmove, impact, interruptPriority, isInterrupt, targetType, aoeType, aoeSize, aoeAngleOrLength, maxAoeTargetCount, startupPositionType, autoCast, needTarget, cooldown, dslSkillId, startupSkillId, flybackSkillId, damage, addhp, addmp, addattack, adddefence, addshield, addspeed, addcritical, addcriticalpow, addrps };
            string[] titles = new string[] { "描述", "类型", "技能图标", "技能有效距离", "持续时间", "作用周期", "能否移动", "效果", "打断优先级", "自己能否打断", "目标类型", "AOE类型", "AOE半径", "AOE角度或矩形长度", "最大AOE目标数", "启动位置类型", "autoCast", "是否指向性技能", "冷却时间", "dslSkillId", "启动技能ID", "归位技能ID", "伤害", "加血", "加蓝", "加攻击", "加防御", "加盾", "加速", "加暴击概率", "加暴击率", "加攻速" };
            for (int i = 0; i < p.Length; ++i) {
                position.x = x;
                position.y += c_FieldHeight;
                EditorGUI.LabelField(position, titles[i]);
                position.x += c_LabelWidth;
                EditorGUI.PropertyField(position, p[i], GUIContent.none);
            }

            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "dslFile");
            position.x += c_LabelWidth;
            position.width = standardFieldWidth - 80;
            EditorGUI.PropertyField(position, dslFile, GUIContent.none);

            position.x += position.width;
            position.width = 40;
            if (GUI.Button(position, "打开")) {
                string filePath = dslFile.stringValue;
                OpenSkillDsl(filePath);
            }
            position.x += position.width;
            position.width = 40;
            if (GUI.Button(position, "创建")) {
                int dslId = dslSkillId.intValue;
                string filePath = dslFile.stringValue;
                ShowSkillDslTemplateMenu(dslId, filePath, position.x, position.y - c_FieldHeight * 3);
            }

            position.width = standardFieldWidth;

            int ctSkills = subsequentSkills.arraySize;
            position.x = x;
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "连续技所有后续技能");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            v = EditorGUI.IntField(position, ctSkills);
            if (EditorGUI.EndChangeCheck()) {
                for (int i = v; i < ctSkills; ++i) {
                    subsequentSkills.DeleteArrayElementAtIndex(v);
                }
                for (int i = ctSkills; i < v; ++i) {
                    subsequentSkills.InsertArrayElementAtIndex(ctSkills);
                }
            }
            
            for (int i = 0; i < v; ++i) {
                position.x = x + c_LabelWidth;
                position.y += c_FieldHeight;
                SerializedProperty skillId = subsequentSkills.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(position, skillId, GUIContent.none);
            }
                        
            int ct = skillres.arraySize;
            position.x = x; 
            position.y += c_FieldHeight;
            EditorGUI.LabelField(position, "特效资源数目");
            position.x += c_LabelWidth;
            EditorGUI.BeginChangeCheck();
            v = EditorGUI.IntField(position, ct);
            if (EditorGUI.EndChangeCheck()) {
                for (int i = v; i < ct; ++i) {
                    skillres.DeleteArrayElementAtIndex(v);
                }
                for (int i = ct; i < v; ++i) {
                    skillres.InsertArrayElementAtIndex(ct);
                }
            }

            for (int i = 0; i < v; ++i) {
                position.x = x;
                position.y += c_FieldHeight;
                SerializedProperty res = skillres.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(position, res, GUIContent.none);
            }
        }
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty skillIds = property.FindPropertyRelative("subsequentSkills");
        SerializedProperty skillres = property.FindPropertyRelative("resources");
        if (property.isExpanded) {
            return (property.CountInProperty() + 1 + skillIds.arraySize + 1 + skillres.arraySize + 1) * c_FieldHeight + 100;
        } else {
            return c_FieldHeight;
        }
    }

    private static void OpenSkillDsl(string filePath)
    {
        if (!s_DslEditorPathLoaded) {
            s_DslEditorPathLoaded = true;
            
            if (PlayerPrefs.HasKey("DslEditorPath")) {
                s_DslEditorPath = PlayerPrefs.GetString("DslEditorPath");
            }
        }
        if (!File.Exists(s_DslEditorPath)) {
            string editorPath = UnityEditor.EditorUtility.OpenFilePanelWithFilters("请选择用于编辑DSL的编辑器程序", Path.GetDirectoryName(s_DslEditorPath), new string[] { "可执行文件", "exe" });
            if (!File.Exists(editorPath)) {
                return;
            }
            s_DslEditorPath = editorPath;
            PlayerPrefs.SetString("DslEditorPath", s_DslEditorPath);
            PlayerPrefs.Save();
        }
        string path = GameFramework.HomePath.GetAbsolutePath("../../../Resource/DslFile/" + filePath).Replace('/', '\\');
        if (!File.Exists(path)) {
            UnityEditor.EditorUtility.DisplayDialog("关键信息", "技能dsl文件不存在，请先点击创建按钮创建文件！", "确定");
            return;
        }
        System.Diagnostics.Process p = System.Diagnostics.Process.Start(s_DslEditorPath, string.Format("\"{0}\"", path));
        p.Close();
    }
    private static void ShowSkillDslTemplateMenu(int dslSkillId, string filePath, float x, float y)
    {
        KeyValuePair<int, string> userData = new KeyValuePair<int, string>(dslSkillId, filePath);
        string path = GameFramework.HomePath.GetAbsolutePath("../../../Resource/DslTemplate/SkillTemplates.dsl");
        string txt = File.ReadAllText(path, Encoding.GetEncoding(936));
        Dsl.DslFile file = new Dsl.DslFile();
        if (file.LoadFromString(txt, path, GameFramework.LogSystem.Log)) {
            List<GUIContent> menus = new List<GUIContent>();
            menus.Add(new GUIContent("取消创建"));
            s_SkillDslTemplates.Clear();
            foreach (Dsl.DslInfo info in file.DslInfos) {
                Dsl.FunctionData funcData = info.First;
                if (funcData.GetId() == "skilltemplate") {
                    string key = funcData.Call.GetParamId(0);
                    string content = funcData.GetExternScript();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(content)) {
                        s_SkillDslTemplates.Add(key, content);
                        menus.Add(new GUIContent(key));
                    }
                }
            }
            UnityEditor.EditorUtility.DisplayCustomMenu(new Rect(x, y, c_LabelWidth, c_FieldHeight * menus.Count), menus.ToArray(), 0, OnCustomMenu, userData);
        } else {
            UnityEditor.EditorUtility.DisplayDialog("错误", "技能dsl模板文件存在语法错误！", "ok");
        }
    }
    private static void OnCustomMenu(object userData, string[] options, int selected)
    {
        KeyValuePair<int, string> data = (KeyValuePair<int, string>)userData;
        if (selected > 0) {
            string content;
            if (s_SkillDslTemplates.TryGetValue(options[selected], out content)) {
                CreateSkillDslFile(data.Key, data.Value, content);
            }
        }
    }
    private static void CreateSkillDslFile(int dslSkillId, string filePath, string content)
    {
        string path = GameFramework.HomePath.GetAbsolutePath("../../../Resource/DslFile/" + filePath);
        if (File.Exists(path)) {
            if (!UnityEditor.EditorUtility.DisplayDialog("关键信息", "技能dsl文件已存在，要覆盖么？", "我确定", "不覆盖")) {
                return;
            }
        }
        using (StreamWriter sw = new StreamWriter(path, false)) {
            sw.WriteLine("skill({0})", dslSkillId);
            sw.Write("{{{0}",content);
            sw.WriteLine("};");
            sw.Close();
        }
        UnityEditor.EditorUtility.DisplayDialog("提示", "dsl文件创建完毕", "ok");
    }

    private static Dictionary<string, string> s_SkillDslTemplates = new Dictionary<string, string>();

    private static string s_DslEditorPath = @"C:\Program Files (x86)\IDM Computer Solutions\UltraEdit\Uedit64.exe";
    private static bool s_DslEditorPathLoaded = false;

    private const string c_Resources = "Resources/";
    private const string c_Ext = ".prefab";
    private const float c_LabelWidth = 140;
    private const float c_FieldHeight = 16;
}

[CustomPropertyDrawer(typeof(SkillRecords.SkillRecord.SkillResource))]
public class SkillResourceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        SerializedProperty key = property.FindPropertyRelative("Key");
        SerializedProperty resource = property.FindPropertyRelative("Resource");
        SerializedProperty resourceFilter = property.FindPropertyRelative("ResourceFilter");

        float w = position.width;
        float x = position.x;
        
        position.width = c_LabelWidth;
        position.height = c_FieldHeight;
        string keyStr = key.stringValue;
        string[] list = new string[] { "selfEffect", "hitEffect", "emitEffect", "targetEffect", "selfEffect1", "selfEffect2", "selfEffect3", "hitEffect1", "hitEffect2", "hitEffect3", "emitEffect1", "emitEffect2", "emitEffect3", "targetEffect1", "targetEffect2", "targetEffect3" };
        int keyIndex = System.Array.IndexOf(list, keyStr);
        EditorGUI.BeginChangeCheck();
        keyIndex = EditorGUI.Popup(position, keyIndex, list);
        if (EditorGUI.EndChangeCheck()) {
            if (keyIndex >= 0)
                key.stringValue = list[keyIndex];
        }

        position.width = 240;
        position.x += c_LabelWidth;
        string[] guids = AssetDatabase.FindAssets("t:Prefab," + Path.GetFileName(resource.stringValue));
        Object v = null;
        for (int i = 0; i < guids.Length; ++i) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (path.Contains("Resources/" + resource.stringValue)) {
                v = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                break;
            }
        }
        EditorGUI.BeginChangeCheck();
        v = EditorGUI.ObjectField(position, v, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck()) {
            string respath = AssetDatabase.GetAssetPath(v.GetInstanceID());
            int startIndex = respath.IndexOf(c_Resources);
            if (startIndex >= 0) {
                resource.stringValue = respath.Substring(startIndex + c_Resources.Length, respath.Length - startIndex - c_Resources.Length - c_Ext.Length);
            } else {
                resource.stringValue = respath;
            }
        }

        position.width = w - 240;
        position.x += 240;
        EditorGUI.LabelField(position, resource.stringValue);

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return c_FieldHeight;
    }

    private const string c_Resources = "Resources/";
    private const string c_Ext = ".prefab";
    private const float c_LabelWidth = 140;
    private const float c_FieldHeight = 16;
}