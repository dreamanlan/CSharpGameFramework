using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using GameFramework;
using GameFramework.Story;
using GameFramework.Skill;
using System.IO;

#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine.iOS;
#endif

public class Game : MonoBehaviour
{
    public enum MouseButton
    {
        LEFT = 0,
        MIDDLE,
        RIGHT,
    }
    public EventSystem eventSystem;

    private bool m_IsInited;
    private int levelID;
    private CameraController m_CameraController;
    private GameObject mMainCamera = null;
    void Start()
    {
        try {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(eventSystem.gameObject);
            Input.multiTouchEnabled = true;
            Application.runInBackground = true;

            string dataPath = Application.dataPath;
            string persistentDataPath = Application.persistentDataPath + "/DataFile";
            string streamingAssetsPath = Application.streamingAssetsPath;
            string tempPath = Application.temporaryCachePath;

            Debug.Log("tempPath=" + tempPath);
            Debug.Log("dataPath=" + dataPath);
            Debug.Log("persistentDataPath=" + persistentDataPath);
            Debug.Log("streamingAssetsPath=" + streamingAssetsPath);

#if UNITY_ANDROID && !UNITY_EDITOR
	    GameControler.Init(tempPath, persistentDataPath);
        GlobalVariables.Instance.IsDevice = true;
#elif UNITY_IPHONE && !UNITY_EDITOR
        GameControler.Init(tempPath, persistentDataPath);
        GlobalVariables.Instance.IsDevice = true;
        if ((int)Device.generation <= (int)iPhoneGeneration.iPhone4S) {
            GlobalVariables.Instance.IsIphone4S = true;
        } else {
            GlobalVariables.Instance.IsIphone4S = false;
        }
#else
            GlobalVariables.Instance.IsDevice = false;
            if (Application.isEditor && !GlobalVariables.Instance.IsPublish)
                GameControler.Init(tempPath, streamingAssetsPath);
            else
                GameControler.Init(dataPath, streamingAssetsPath);
#endif
            StartCoroutine(CheckAndUpdate());
        } catch (System.Exception ex) {
            Debug.LogErrorFormat("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    IEnumerator CheckAndUpdate()
    {
        //1、检查更新
        //2、解压资源（AssetBundle与散文件）
#if UNITY_ANDROID || UNITY_IPHONE
        yield return StartCoroutine(ExtractDataFile());
#endif
        //3、启动游戏逻辑
        GameControler.InitGame(true);
        yield return null;
        //4、切换到第一个场景
        SpriteManager.Init();
        ClientModule.Instance.ChangeScene(1);
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
    }

    public void LateUpdate()
    {
        if (null != m_CameraController) {
            m_CameraController.Update();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        GameControler.PauseGame(pauseStatus);
        GameControler.PauseGameForeground(pauseStatus);
    }

    void OnApplicationQuit()
    {
        GameControler.Release();
    }
        
    void OnLevelWasLoaded(int level)
    {
        if (level == 2) {
            HighlightPromptManager.Instance.Init();
        } else if (level > 2) {
            HighlightPromptManager.Instance.Init();
            BattleTopMenuManager.Instance.Init();
            SkillBarManager.Instance.Init();
            OverHeadBarManager.Instance.Init();
        }
        m_CameraController = new CameraController(Camera.main);
    }

    private IEnumerator LoadLevel(TableConfig.Level lvl)
    {
        HighlightPromptManager.Instance.Release();
        BattleTopMenuManager.Instance.Release();
        SkillBarManager.Instance.Release();
        OverHeadBarManager.Instance.Release();
        m_CameraController = null;
        yield return null;
        yield return Application.LoadLevelAsync("Loading");
        yield return Resources.UnloadUnusedAssets();
        yield return Application.LoadLevelAsync(lvl.prefab);
        ClientModule.Instance.OnSceneLoaded(lvl);
        Utility.EventSystem.Publish("loading_complete","ui",null);
    }

    private void OnLoadMainUiComplete(int levelId)
    {
        this.levelID = levelId;

        LoadUi(levelId);
    }

    //装载结束后加入BattleManager脚本
    private void OnLoadBattleComplete(int levelId)
    {
        this.levelID = levelId;

        GameObject obj = new GameObject();
        mMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mMainCamera) {
            Camera cameraMain = mMainCamera.GetComponent<Camera>();
            cameraMain.cullingMask &= ~(1 << 5);
        }

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

#if UNITY_ANDROID || UNITY_IPHONE     
    IEnumerator ExtractDataFile()
    {
        string srcPath = Application.streamingAssetsPath;
        string destPath = Application.persistentDataPath + "/DataFile";
        Debug.Log(srcPath);
        Debug.Log(destPath);

        if (!srcPath.Contains("://"))
            srcPath = "file://" + srcPath;
        string listPath = srcPath + "/list.txt";
        WWW listData = new WWW(listPath);
        yield return listData;
        string listTxt = listData.text;
        if (null != listTxt) {
            using (StringReader sr = new StringReader(listTxt)) {
                string numStr = sr.ReadLine();
                float totalNum = 50;
                if (null != numStr) {
                    numStr = numStr.Trim();
                    totalNum = (float)int.Parse(numStr);
                    if (totalNum <= 0)
                        totalNum = 50;
                }
                for (float num = 1; ; num += 1) {
                    string path = sr.ReadLine();
                    if (null != path) {
                        path = path.Trim();
                        string url = srcPath + "/" + path;
                        //Debug.Log("extract " + url);
                        string filePath = Path.Combine(destPath, path);
                        string dir = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        WWW temp = new WWW(url);
                        yield return temp;
                        if (null != temp.bytes) {
                            try {
                                File.WriteAllBytes(filePath, temp.bytes);
#if UNITY_APPSTORE && UNITY_IPHONE
                                iPhone.SetNoBackupFlag(filePath);
#endif
                            }
                            catch (System.Exception ex) {
                                Debug.LogFormat("ExtractDataFileAndStartGame copy config failed. ex:{0} st:{1}",
                                ex.Message, ex.StackTrace);
                            }
                        }
                        else {
                            Debug.Log(path + " can't load");
                        }
                        temp = null;
                    }
                    else {
                        break;
                    }
                }
                sr.Close();
            }
            listData = null;
        }
        else {
            Debug.Log("Can't load list.txt");
        }
    }
#endif
    private void CameraLook(float[] poses)
    {
        if (null != poses && poses.Length >= 3) {
            m_CameraController.ClearFollow();
            m_CameraController.MoveFollowPath(new Vector3(poses[0], poses[1], poses[2]), true);
        }
    }
    
    private void CameraFollow(int actorId)
    {
        m_CameraController.Follow(actorId);
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

    private void CameraFollowPath()
    {
        m_CameraController.ClearFollow();
        m_CameraController.ToFollowPath();
        GfxStorySystem.Instance.SendMessage("camerafollowpath");
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
}
