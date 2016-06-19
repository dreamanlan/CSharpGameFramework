using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using GameFramework;
using GameFramework.Story;
using GameFramework.Skill;
using System.IO;
using System.Text;
#if UNITY_EDITOR_WIN
using UnityEditor;

[InitializeOnLoad]
#endif
public class SkillViewer : MonoBehaviour
{
    public enum MouseButton
    {
        LEFT = 0,
        MIDDLE,
        RIGHT,
    }
    public EventSystem eventSystem;

#if UNITY_EDITOR_WIN

    private SkillViewerCameraController m_CameraController = new SkillViewerCameraController();
    private GameObject m_MainCamera = null;
    private static bool s_IsStepPlay = false;
    private static bool s_NeedStep = false;

    static SkillViewer()
    {
        EditorApplication.update += EditorUpdate;
    }

    static void EditorUpdate()
    {
        if (s_NeedStep) {
            if (!EditorApplication.isPaused) {
                EditorApplication.Step();
            }
            s_NeedStep = false;
        }
    }

    void Awake()
    {
    }
    
    void Start()
    {
        Input.multiTouchEnabled = true;

        InitFilePath();

        string tempPath = Application.temporaryCachePath;
        string streamingAssetsPath = Application.streamingAssetsPath;
        GlobalVariables.Instance.IsDevice = false;
        GameControler.Init(tempPath, streamingAssetsPath);        
        GameControler.InitGame(false);
        SpriteManager.Init();
        ClientModule.Instance.ChangeScene(4);
    }

    void OnDestroy()
    {
    }

    public static void InitFilePath()
    {
        string persistentDataPath = Application.persistentDataPath + "/DataFile";
        string streamingAssetsPath = Application.streamingAssetsPath;
        HomePath.CurHomePath = streamingAssetsPath;
        Debug.Log("persistentDataPath=" + persistentDataPath);
        Debug.Log("streamingAssetsPath = " + streamingAssetsPath);
    }

    void Update()
    {
        HighlightPromptManager.Instance.Update();
        GameControler.TickGame();

        if (Input.GetMouseButtonDown((int)MouseButton.LEFT)) {
            GameObject storyDlg = GameObject.Find("StoryDlg");
            if (null == storyDlg || !storyDlg.activeSelf) {
                Vector3 screenPos = Input.mousePosition;
                if (!BattleTopMenuManager.Instance.IsOn(screenPos) && !SkillBarManager.Instance.IsOn(screenPos)) {
                    Ray ray = Camera.main.ScreenPointToRay(screenPos);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Default")))) {
                        int objId = ClientModule.Instance.GetGameObjectId(hit.collider.gameObject);
                        if (objId > 0) {
                            ClientModule.Instance.ClickNpc(objId);
                        } else {
                            Vector3 pos = hit.point;
                            ClientModule.Instance.MoveTo(pos.x, pos.y, pos.z);
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        //Pause按键暂停
        if (Input.GetKeyDown(KeyCode.Pause))
            Debug.Break();
#endif
    }

    void LateUpdate()
    {
        if (null != m_CameraController) {
            m_CameraController.Update();
        }
    }

    void OnBecameVisible()
    {
    }
        
    void OnLevelWasLoaded(int level)
    {
    }

    private IEnumerator LoadLevel(TableConfig.Level lvl)
    {
        ClientModule.Instance.OnSceneLoaded(lvl);
        yield return null;
    }

    private IEnumerator PreloadObjects(IList<int> objLinkIds)
    {
        yield return null;
        GfxStorySystem.Instance.SendMessage("load");
    }

    private void OnLoadMainUiComplete(int levelId)
    {
        LoadUi(levelId);
    }

    //装载结束后加入BattleManager脚本
    private void OnLoadBattleComplete(int levelId)
    {
        GameObject obj = new GameObject();
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (m_MainCamera) {
            Camera cameraMain = m_MainCamera.GetComponent<Camera>();
            cameraMain.cullingMask &= ~(1 << 5);
        }

        m_CameraController.OnLevelWasLoaded(null);
        GameObject cameraObj = ResourceSystem.Instance.NewObject("UI/UiCamera") as GameObject;
        if (null != cameraObj) {
            cameraObj.transform.parent = Camera.main.transform;
        }
        SkillBarManager.Instance.Init(cameraObj);
        OverHeadBarManager.Instance.Init();
        LoadUi(levelId);
    }

    private void LoadUi(int levelId)
    {
        TableConfig.Level level = TableConfig.LevelProvider.Instance.GetLevel(levelId);
        if (null != level) {
            int ct = level.SceneUi.Count;
            for (int i = 0; i < ct; ++i) {
                int uiId = level.SceneUi[i];
                TableConfig.UI ui = TableConfig.UIProvider.Instance.GetUI(uiId);
                if (null != ui) {
                    GameObject asset = UiResourceSystem.Instance.GetUiResource(ui.path) as GameObject;
                    if (null != asset) {
                        GameObject uiObj = GameObject.Instantiate(asset);
                        if (null != uiObj) {
                            uiObj.name = ui.name;
                            if (!string.IsNullOrEmpty(ui.dsl)) {
                                GameFramework.Story.UiStoryInitializer initer = uiObj.GetComponent<GameFramework.Story.UiStoryInitializer>();
                                if (null == initer) {
                                    initer = uiObj.AddComponent<GameFramework.Story.UiStoryInitializer>();
                                }
                                if (null != initer) {
                                    initer.WindowName = ui.name;
                                    initer.Init();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void OnStepChanged(int val)
    {
        s_IsStepPlay = val != 0;
    }

    private void OnCastSkill(object[] fargs)
    {
        if (null != fargs && fargs.Length == 2) {
            int objId = (int)fargs[0];
            int skillId = (int)fargs[1];
            
            bool bSuccess = ClientModule.Instance.CastSkill(objId, skillId);
            if (bSuccess) {
                if (s_IsStepPlay) {
                    s_NeedStep = true;
                }
            }
        }
    }

    private void LoadViewedSkills(object[] fargs)
    {
        if (null != fargs && fargs.Length == 2) {
            int actorId = (int)fargs[0];
            int targetId = (int)fargs[1];

            PlayerPrefs.SetInt("ActorId", actorId);
            PlayerPrefs.SetInt("TargetId", targetId);
            PlayerPrefs.Save();

            if (UnityEditor.EditorUtility.DisplayDialog("关键信息", "从文件加载数据将同时覆盖正在编辑的数据，继续吗？（如果之前的数据没有保存到表格文件里，请利用剪贴板拷到表格文件！）", "我确定要继续", "不要继续，我还没保存呢")) {
                SaveEditedSkills(HomePath.GetAbsolutePath("../../../edit_skills_bak.txt"));
                CopyTableAndDslFiles();
                m_CameraController.OnLevelWasLoaded(null);
                ClientModule.LoadTableConfig();       
                PredefinedSkill.Instance.ReBuild();
                GfxSkillSystem.Instance.Reset();
                GfxSkillSystem.Instance.ClearSkillInstancePool();
                SkillSystem.SkillConfigManager.Instance.Clear();

                GameObject actor = GameObject.Find("Editor_ActorRecord");
                GameObject skills = GameObject.Find("Editor_SkillRecords");
                if (null != actor && null != skills) {
                    ActorRecord actorRecord = actor.GetComponent<ActorRecord>();
                    SkillRecords skillRecords = skills.GetComponent<SkillRecords>();
                    if (null != actorRecord && null != skillRecords) {
                        TableConfig.Actor actorInfo = TableConfig.ActorProvider.Instance.GetActor(actorId);
                        if (null != actorInfo) {
                            List<int> args = new List<int>();
                            if (actorInfo.skill0 > 0)
                                args.Add(actorInfo.skill0);
                            if (actorInfo.skill1 > 0)
                                args.Add(actorInfo.skill1);
                            if (actorInfo.skill2 > 0)
                                args.Add(actorInfo.skill2);
                            if (actorInfo.skill3 > 0)
                                args.Add(actorInfo.skill3);
                            if (actorInfo.skill4 > 0)
                                args.Add(actorInfo.skill4);
                            if (actorInfo.skill5 > 0)
                                args.Add(actorInfo.skill5);
                            if (actorInfo.skill6 > 0)
                                args.Add(actorInfo.skill6);
                            if (actorInfo.skill7 > 0)
                                args.Add(actorInfo.skill7);
                            if (actorInfo.skill8 > 0)
                                args.Add(actorInfo.skill8);

                            RebuildVisualSkillInfo(actorId);

                            actorRecord.CopyFrom(actorInfo);

                            skillRecords.records.Clear();
                            foreach (int skillId in args) {
                                AddViewedSkill(skillId, skillRecords.records);
                            }

                            GfxStorySystem.Instance.SendMessage("reload", actorId, targetId, args);
                        }
                    }
                }

                UnityEditor.EditorUtility.DisplayDialog("提示", "从文件加载英雄与技能完毕", "ok");
            }
        }
    }
    private void NewEditedSkills()
    {
        if (UnityEditor.EditorUtility.DisplayDialog("关键信息", "加载或创建英雄技能数据将覆盖正在编辑的数据，继续吗？（如果之前的数据没有保存到表格文件里，请利用剪贴板拷到表格文件！）", "我确定要继续", "不要继续，我还没保存呢")) {
            SaveEditedSkills(HomePath.GetAbsolutePath("../../../edit_skills_bak.txt"));

            ClientModule.LoadTableConfig();
            PredefinedSkill.Instance.ReBuild();
            GfxSkillSystem.Instance.Reset();
            GfxSkillSystem.Instance.ClearSkillInstancePool();
            SkillSystem.SkillConfigManager.Instance.Clear();

            GameObject actor = GameObject.Find("Editor_ActorRecord");
            GameObject skills = GameObject.Find("Editor_SkillRecords");
            if (null != actor && null != skills) {
                ActorRecord actorRecord = actor.GetComponent<ActorRecord>();
                SkillRecords skillRecords = skills.GetComponent<SkillRecords>();
                if (null != actorRecord && null != skillRecords) {
                    int skillCount = 0;
                    List<int> skillIds = new List<int>();
                    if (actorRecord.skill0 > 0) { ++skillCount; skillIds.Add(actorRecord.skill0); }
                    if (actorRecord.skill1 > 0) { ++skillCount; skillIds.Add(actorRecord.skill1); }
                    if (actorRecord.skill2 > 0) { ++skillCount; skillIds.Add(actorRecord.skill2); }
                    if (actorRecord.skill3 > 0) { ++skillCount; skillIds.Add(actorRecord.skill3); }
                    if (actorRecord.skill4 > 0) { ++skillCount; skillIds.Add(actorRecord.skill4); }
                    if (actorRecord.skill5 > 0) { ++skillCount; skillIds.Add(actorRecord.skill5); }
                    if (actorRecord.skill6 > 0) { ++skillCount; skillIds.Add(actorRecord.skill6); }
                    if (actorRecord.skill7 > 0) { ++skillCount; skillIds.Add(actorRecord.skill7); }
                    if (actorRecord.skill8 > 0) { ++skillCount; skillIds.Add(actorRecord.skill8); }

                    RebuildVisualSkillInfo(actorRecord.id);

                    skillRecords.records.Clear();
                    for (int i = 0; i < skillCount; ++i) {
                        AddViewedSkill(skillIds[i], skillRecords.records);
                    }
                }
            }
        }

        UnityEditor.EditorUtility.DisplayDialog("提示", "技能数据加载/创建完毕", "ok");
    }
    private void LoadEditedSkills(int targetId)
    {
        PlayerPrefs.SetInt("TargetId", targetId);
        PlayerPrefs.Save();

        SaveEditedSkills(HomePath.GetAbsolutePath("../../../edit_skills_bak.txt"));        
        CopyTableAndDslFiles();

        ClientModule.LoadTableConfig();
        PredefinedSkill.Instance.ReBuild();
        GfxSkillSystem.Instance.Reset();
        GfxSkillSystem.Instance.ClearSkillInstancePool();
        SkillSystem.SkillConfigManager.Instance.Clear();

        GameObject actor = GameObject.Find("Editor_ActorRecord");
        GameObject skills = GameObject.Find("Editor_SkillRecords");
        if (null != actor && null != skills) {
            ActorRecord actorRecord = actor.GetComponent<ActorRecord>();
            SkillRecords skillRecords = skills.GetComponent<SkillRecords>();
            if (null != actorRecord && null != skillRecords) {
                List<int> args = new List<int>();
                int actorId = actorRecord.id;
                if (actorRecord.skill0 > 0)
                    args.Add(actorRecord.skill0);
                if (actorRecord.skill1 > 0)
                    args.Add(actorRecord.skill1);
                if (actorRecord.skill2 > 0)
                    args.Add(actorRecord.skill2);
                if (actorRecord.skill3 > 0)
                    args.Add(actorRecord.skill3);
                if (actorRecord.skill4 > 0)
                    args.Add(actorRecord.skill4);
                if (actorRecord.skill5 > 0)
                    args.Add(actorRecord.skill5);
                if (actorRecord.skill6 > 0)
                    args.Add(actorRecord.skill6);
                if (actorRecord.skill7 > 0)
                    args.Add(actorRecord.skill7);
                if (actorRecord.skill8 > 0)
                    args.Add(actorRecord.skill8);

                RebuildVisualSkillInfo(actorId);

                bool isValid = true;
                if (string.IsNullOrEmpty(actorRecord.avatar)) {
                    Debug.LogErrorFormat("actor avatar is empty !!!");
                    isValid = false;
                }
                foreach (int skillId in args) {
                    CheckEditedSkill(skillId, skillRecords.records, ref isValid);
                }
                if (isValid) {
                    TableConfig.Actor actorInfo = TableConfig.ActorProvider.Instance.GetActor(actorId);
                    if (null == actorInfo) {
                        actorInfo = new TableConfig.Actor();
                        actorInfo.id = actorId;
                        TableConfig.ActorProvider.Instance.ActorMgr.GetData().Add(actorId, actorInfo);
                    }
                    actorRecord.CopyTo(actorInfo);

                    foreach (SkillRecords.SkillRecord record in skillRecords.records) {
                        TableConfig.Skill skillInfo = TableConfig.SkillProvider.Instance.GetSkill(record.id);
                        if (null == skillInfo) {
                            skillInfo = new TableConfig.Skill();
                            skillInfo.id = record.id;
                            TableConfig.SkillProvider.Instance.SkillMgr.GetData().Add(record.id, skillInfo);
                        }
                        record.CopyTo(skillInfo);
                    }

                    if (args.Count > 1) {
                        GfxStorySystem.Instance.SendMessage("reload", actorId, targetId, args);
                    }
                }
            }

            UnityEditor.EditorUtility.DisplayDialog("提示", "编辑英雄与技能加载完毕", "ok");
        }
    }
    private void CopyEditedSkillsToClipboard()
    {
        SaveEditedSkills(HomePath.GetAbsolutePath("../../../edit_skills_bak.txt"));

        string text = GetEditedSkillsText();
        TextEditor editor = new TextEditor();
        editor.text = text;
        //editor.content = new GUIContent(text);
        editor.OnFocus();
        editor.Copy();
    }

    private void RebuildVisualSkillInfo(int actorId)
    {
        GameObject vsi = GameObject.Find("Editor_InplaceSkillInfo");
        if (null != vsi) {
            InplaceSkillInfo info = vsi.GetComponent<InplaceSkillInfo>();
            if (null != info) {
                info.Rebuild(actorId);
            }
        }
    }

    private static void CopyTableAndDslFiles()
    {
        string path = HomePath.GetAbsolutePath("..\\..\\..\\dslcopy.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug False", path));
        p.WaitForExit();
    }

    private static void SaveEditedSkills(string path)
    {
        string text = GetEditedSkillsText();
        File.WriteAllText(path, text);
    }

    private static string GetEditedSkillsText()
    {
        string ret = string.Empty;
        GameObject actor = GameObject.Find("Editor_ActorRecord");
        GameObject skills = GameObject.Find("Editor_SkillRecords");
        if (null != actor && null != skills) {
            ActorRecord actorRecord = actor.GetComponent<ActorRecord>();
            SkillRecords skillRecords = skills.GetComponent<SkillRecords>();
            if (null != actorRecord && null != skillRecords) {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//========actor========");
                sb.AppendLine(actorRecord.GetClipboardText());
                sb.AppendLine("//========skills========");
                foreach (SkillRecords.SkillRecord record in skillRecords.records) {
                    sb.AppendLine(record.GetSkillClipboardText());
                }
                sb.AppendLine("//========skillDsls========");
                foreach (SkillRecords.SkillRecord record in skillRecords.records) {
                    sb.AppendLine(record.GetSkillDslClipboardText());
                }
                sb.AppendLine("//========skillResources========");
                foreach (SkillRecords.SkillRecord record in skillRecords.records) {
                    sb.AppendLine(record.GetSkillResourcesClipboardText());
                }
                ret = sb.ToString();
            }
        }
        return ret;
    }

    private static void AddViewedSkill(int skillId, List<SkillRecords.SkillRecord> records)
    {
        foreach (var r in records) {
            if (r.id == skillId) {
                if (r.impact > 0) {
                    AddViewedSkill(r.impact, records);
                }
                return;
            }
        }
        SkillRecords.SkillRecord record = new SkillRecords.SkillRecord();
        records.Add(record);

        record.id = skillId;
        record.type = (int)SkillOrImpactType.Skill;
        record.dslSkillId = record.id;
        record.dslFile = string.Empty;

        TableConfig.Skill skillInfo = TableConfig.SkillProvider.Instance.GetSkill(skillId);
        if (null != skillInfo) {
            record.CopyFrom(skillInfo);

            if (skillInfo.impact > 0) {
                AddViewedSkill(skillInfo.impact, records);
            }
        }
    }

    private static void CheckEditedSkill(int skillId, List<SkillRecords.SkillRecord> records, ref bool isValid)
    {
        bool find = false;
        if (null != TableConfig.SkillProvider.Instance.GetSkill(skillId)) {
            find = true;
        } else {
            foreach (SkillRecords.SkillRecord record in records) {
                if (skillId == record.id) {
                    find = true;
                    if (record.impact > 0) {
                        CheckEditedSkill(record.impact, records, ref isValid);
                    }
                    break;
                }
            }
        }
        if (!find) {
            isValid = find;
            Debug.LogErrorFormat("Can't find skill config: {0}", skillId);
        }
    }
    private void ShowHideForSkill(object[] args)
    {
        if (null != args && args.Length >= 2) {
            GfxSkillSenderInfo info = args[0] as GfxSkillSenderInfo;
            string v = args[1] as string;
            if (null != info && null != v && null != info.GfxObj) {
                info.GfxObj.SetActive(v != "0");
            }
        }
    }
    private void CameraShakeForSkill(object[] args)
    {
        if (null != args && args.Length >= 4) {
            GfxSkillSenderInfo info = args[0] as GfxSkillSenderInfo;
            if (null != info) {
                if (ClientModule.Instance.IsLocalSkillEffect(info)) {
                    m_CameraController.Shake(float.Parse(args[1] as string), float.Parse(args[2] as string), float.Parse(args[3] as string));
                }
            }
        }
    }
    private void CameraDark(object[] args)//float darkdegree,bool froceEnd=false)
    {
        if (null != args && args.Length >= 2) {
            float darkdegree = (float)args[0];
            bool froceEnd = (bool)args[1];
            if (m_MainCamera == null) {
                m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            if (m_MainCamera != null) {
                m_MainCamera.GetComponent<Camera>().cullingMask &= ~(1 << 5);
                /*
                GrayCamera gcam = mMainCamera.GetComponent<GrayCamera>();
                if (null == gcam) {
                    mMainCamera.AddComponent<GrayCamera>();
                    gcam = mMainCamera.GetComponent<GrayCamera>();
                }
                if (null != gcam.darkMask) {
                    if (darkdegree != 0.0f) {
                        cameraDrakCount++;
                        gcam.intensity = darkdegree;
                        gcam.darkMask.gameObject.SetActive(true);
                    } else {
                        cameraDrakCount--;
                        if (cameraDrakCount < 0) cameraDrakCount = 0;
                        if (froceEnd == true) {
                            cameraDrakCount = 0;
                        }
                        if (cameraDrakCount == 0) {
                            gcam.intensity = 0.0f;
                            gcam.darkMask.gameObject.SetActive(false);
                        }
                    }
                }
                */
            }
        }
    }
    private void CameraShake(object[] args)
    {
        if (null != args && args.Length >= 3) {            
            m_CameraController.Shake((float)args[0], (float)args[1], (float)args[2]);
        }
    }
    private void CameraLook(object[] poses)
    {
        if (null != poses && poses.Length >= 3) {
            m_CameraController.ClearFollow();
            m_CameraController.MoveToLookat(new Vector3((float)poses[0], (float)poses[1], (float)poses[2]), true);
        }
    }

    private void CameraFollow(object actorId)
    {
        m_CameraController.Follow((int)actorId);
    }

    private void CameraPosition(object[] args)
    {
        if (null != args && args.Length >= 3) {
            Vector3 pos = (Vector3)args[0];
            Quaternion q = (Quaternion)args[1];
            float dist = (float)args[2];

            m_CameraController.ClearFollow();
            m_CameraController.MoveToFixedPosition(pos, q, true);
        }
    }
    private void CameraLookLast()
    {
        m_CameraController.ClearFollow();
        m_CameraController.ToLastLookat();
    }
    private void CameraTrack()
    {
        m_CameraController.ClearFollow();
        m_CameraController.ToTrack();
    }
    private void CameraEnable(object[] args)
    {
        if (null != args[0] && null != args[1]) {
            string cameraName = args[0] as string;
            int enable = (int)args[1];

            if (null != cameraName) {
                GameObject cameraObj = GameObject.Find(cameraName);
                if (cameraObj != null) {
                    Camera camera = cameraObj.GetComponent<Camera>();
                    if (null != camera) {
                        camera.enabled = enable != 0;
                    }
                }
            }
        }
    }
    private void LogToConsole(string msg)
    {
        DebugConsole.Log(msg);
    }
    private void OnResetDsl()
    {
        Utility.EventSystem.Publish("gm_resetdsl", "gm");
    }

    private void OnExecScript(string script)
    {
        Utility.EventSystem.Publish("gm_execscript", "gm", script);
    }

    private void OnExecCommand(string command)
    {
        Utility.EventSystem.Publish("gm_execcommand", "gm", command);
    }

#endif //UNITY_EDITOR
}
