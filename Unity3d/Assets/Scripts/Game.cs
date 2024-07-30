using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using ScriptableFramework;
using ScriptableFramework.Story;
using ScriptableFramework.Skill;
using System.IO;
using UnityEngine.SceneManagement;

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
    private int m_LevelId;
    private CameraController m_CameraController;
    private GameObject m_MainCamera = null;
    private RectTransform m_MainUiRectTransform = null;
    private Camera m_UiCamera = null;

    void Awake()
    {
        //load plugin
        PluginAssembly.Instance.Init();
        SceneManager.sceneLoaded += this.OnLevelLoaded;
    }
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

#if DEVELOPMENT_BUILD
            GlobalVariables.Instance.IsDevelopment = true;
#else
            GlobalVariables.Instance.IsDevelopment = false;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
	        GameControler.Init(tempPath, persistentDataPath);
            GlobalVariables.Instance.IsDevice = true;
#elif UNITY_IPHONE && !UNITY_EDITOR
            GameControler.Init(tempPath, persistentDataPath);
            GlobalVariables.Instance.IsDevice = true;
#else
            GlobalVariables.Instance.IsDevice = false;
            if (Application.isEditor)
                GameControler.Init(".", streamingAssetsPath);
            else
                GameControler.Init(dataPath, streamingAssetsPath);
#endif

            GlobalVariables.Instance.IsClient = true;
            StartCoroutine(CheckAndUpdate());
        }
        catch (System.Exception ex) {
            Debug.LogErrorFormat("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    IEnumerator CheckAndUpdate()
    {
        //1、Update check
        //2、Extract resources（AssetBundle and loose files）
#if UNITY_ANDROID || UNITY_IPHONE
        yield return StartCoroutine(ExtractDataFile());
#endif
        //3、Init game logic
        GameControler.InitGame(true);
        yield return null;
        //4、Change to the first scene
        SpriteManager.Init();
        LoadingManager.Instance.Init();
        PluginFramework.Instance.ChangeScene(1);
    }

    void Update()
    {
        HighlightPromptManager.Instance.Update();
        GameControler.TickGame();

        if (Input.GetMouseButtonDown((int)MouseButton.LEFT)) {
            GameObject storyDlg = GameObject.Find("StoryDlg");
            if (null == m_MainUiRectTransform) {
                GameObject mainUi = GameObject.Find("MainUI");
                if (null != mainUi) {
                    m_UiCamera = mainUi.GetComponent<Canvas>().worldCamera;
                }
                GameObject mainPanel = GameObject.Find("MainUI/Panel");
                if (null != mainPanel) {
                    m_MainUiRectTransform = mainPanel.transform as RectTransform;
                }
            }
            if (null == storyDlg || !storyDlg.activeSelf) {
                Vector3 screenPos = Input.mousePosition;
                if (!IsPointOnMainUI(screenPos)) {
                    Ray ray = Camera.main.ScreenPointToRay(screenPos);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Default")))) {
                        int objId = PluginFramework.Instance.GetGameObjectId(hit.collider.gameObject);
                        if (objId > 0) {
                            PluginFramework.Instance.ClickNpc(objId);
                        }
                        else {
                            Vector3 pos = hit.point;
                            PluginFramework.Instance.MoveTo(pos.x, pos.y, pos.z);
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        //Pause
        if (Input.GetKeyDown(KeyCode.Pause))
            Debug.Break();
#endif
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
        LoadingManager.Instance.Release();
        GameControler.Release();
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {   
        if (scene.buildIndex == 2) {
            HighlightPromptManager.Instance.Init();
        }
        else if (scene.buildIndex > 2) {
            GameObject cameraObj = ResourceSystem.Instance.NewObject("UI/UiCamera") as GameObject;
            if (null != cameraObj) {
                cameraObj.transform.parent = Camera.main.transform;
            }
            TopMenuManager.Instance.Init();
            HighlightPromptManager.Instance.Init();
            OverHeadBarManager.Instance.Init();
        }
        m_CameraController = new CameraController(Camera.main);
    }

    private IEnumerator LoadScene(TableConfig.Level lvl)
    {
        LoadingManager.Instance.Show();
        TopMenuManager.Instance.Release();
        HighlightPromptManager.Instance.Release();
        OverHeadBarManager.Instance.Release();
        m_CameraController = null;
        yield return null;
        LoadingManager.Instance.SetProgress(0.1f);
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Loading");
        LoadingManager.Instance.SetProgress(0.2f);
        yield return Resources.UnloadUnusedAssets();
        LoadingManager.Instance.SetProgress(0.3f);
        var asyncOper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(lvl.prefab);
        LoadingManager.Instance.SetAsync(asyncOper, 0.3f, 0.7f);
        yield return asyncOper;
        PluginFramework.Instance.OnSceneLoaded(lvl);
    }

    private IEnumerator LoadBattle(TableConfig.Level lvl)
    {
        LoadingManager.Instance.Show();
        yield return null;
        LoadingManager.Instance.SetProgress(0.1f);
        yield return Resources.UnloadUnusedAssets();
        LoadingManager.Instance.SetProgress(0.2f);
        var asyncOper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(lvl.prefab, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        LoadingManager.Instance.SetAsync(asyncOper, 0.3f, 0.7f);
        yield return asyncOper;
        PluginFramework.Instance.OnBattleLoaded(lvl);
    }

    private IEnumerator UnloadBattle(TableConfig.Level lvl)
    {
        LoadingManager.Instance.Show();
        yield return null;
        LoadingManager.Instance.SetProgress(0.1f);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(lvl.prefab);
        LoadingManager.Instance.SetProgress(0.9f);
        yield return Resources.UnloadUnusedAssets();
        LoadingManager.Instance.SetProgress(1.0f);
        PluginFramework.Instance.OnBattleUnloaded(lvl);
    }

    private void OnLoadMainUiComplete(int levelId)
    {
        this.m_LevelId = levelId;

        LoadUi(levelId);
    }

    private void OnLoadSceneComplete(int levelId)
    {
        this.m_LevelId = levelId;

        GameObject obj = new GameObject();
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (m_MainCamera) {
            Camera cameraMain = m_MainCamera.GetComponent<Camera>();
            int uiLayer = LayerMask.NameToLayer("UI");
            cameraMain.cullingMask &= ~(1 << uiLayer);
        }

        LoadUi(levelId);
    }

    private void OnLoadBattleComplete(int levelId)
    {
    }

    private IEnumerator LoadAssetAsync(object[] args)
    {
        if (args.Length == 3) {
            string path = args[0] as string;
            string name = args[1] as string;
            var callback = args[2] as ResourceSystem.ResourceLoadDelegation;

            var req = AssetBundle.LoadFromFileAsync(path);
            yield return req;
            if (null != req.assetBundle) {
                var req2 = req.assetBundle.LoadAssetAsync(name);
                yield return req2;
                if (null != callback) {
                    callback(req2.asset);
                }
            }
        }
    }
    private IEnumerator LoadResourceAsync(object[] args)
    {
        if (args.Length == 2) {
            string path = args[0] as string;
            var callback = args[1] as ResourceSystem.ResourceLoadDelegation;

            var req = Resources.LoadAsync(path);
            yield return req;
            if (null != callback) {
                callback(req.asset);
            }
        }
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
                                ScriptableFramework.Story.UiStoryInitializer initer = uiObj.GetComponent<ScriptableFramework.Story.UiStoryInitializer>();
                                if (null == initer) {
                                    initer = uiObj.AddComponent<ScriptableFramework.Story.UiStoryInitializer>();
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

    private bool IsPointOnMainUI(Vector3 pos)
    {
        if (null != m_MainUiRectTransform) {
            if (RectTransformUtility.RectangleContainsScreenPoint(m_MainUiRectTransform, new Vector2(pos.x, pos.y), m_UiCamera))
                return true;
        }
        return false;
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

    private void OnCastSkill(object[] fargs)
    {
        if (null != fargs && fargs.Length == 2) {
            int objId = (int)fargs[0];
            int skillId = (int)fargs[1];

            bool bSuccess = PluginFramework.Instance.CastSkill(objId, skillId);
            if (bSuccess) {
            }
        }
    }

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
