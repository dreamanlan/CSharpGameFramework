using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using ScriptableFramework;
using ScriptableFramework.Story;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine.iOS;
#endif
using ExpressionAPI;
using Dsl;
using StoryScript;
using StoryScript.DslExpression;
using GmCommands;

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
    private float m_NextExploreTime = 0f;   // local UX cooldown for the explore skill

    // Battle scenes are loaded Additively on top of the base (world/story) scene.
    // Each entry keeps the component's ORIGINAL enabled state, so restoring after
    // unload never turns on something that was disabled to begin with (e.g. hidden
    // UI cameras). Entries are de-duplicated, so overlapping Additive loads cannot
    // drop a component from the restore list.
    // Renderer derives from Component, not Behaviour, so the common base here
    // is Component and enabled has to be read/written per concrete type.
    private readonly List<KeyValuePair<Component, bool>> m_HiddenVisuals =
        new List<KeyValuePair<Component, bool>>();

    private static bool VisualEnabled(Component c)
    {
        Behaviour b = c as Behaviour;
        if (null != b) return b.enabled;
        Renderer r = c as Renderer;
        return null != r && r.enabled;
    }

    private static void SetVisualEnabled(Component c, bool v)
    {
        Behaviour b = c as Behaviour;
        if (null != b) { b.enabled = v; return; }
        Renderer r = c as Renderer;
        if (null != r) r.enabled = v;
    }

    private void HideVisual(Component c)
    {
        if (null == c) return;
        for (int i = 0; i < m_HiddenVisuals.Count; ++i)
            if (m_HiddenVisuals[i].Key == c) return;
        m_HiddenVisuals.Add(new KeyValuePair<Component, bool>(c, VisualEnabled(c)));
        SetVisualEnabled(c, false);
    }

    void Awake()
    {
        StartupScript.InitLogger();
        StartupScript.OnExistsApi = ExistsApi;
        StartupScript.OnGetApi = GetApi;
        GmCommands.ClientGmStorySystem.Instance.OnRegisterGmCommandsAndFunctions = AppGmRegister.Register;
        //load plugin
        PluginAssembly.Instance.Init();
        SceneManager.sceneLoaded += this.OnLevelLoaded;
    }
    void Start()
    {
        try {
			StartupScript.TryInit();
        	StartupScript.Calculator.Register("set_clipboard", "set_clipboard(str) api", new ExpressionFactoryHelper<SetClipboardExp>());
        	StartupScript.Calculator.Register("get_clipboard", "get_clipboard() api", new ExpressionFactoryHelper<GetClipboardExp>());
			RunStartup(true);
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
    void OnDestroy()
    {
        StartupScript.ReleaseLogger();
    }
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type) {
            case LogType.Log:
                DebugConsole.Log(logString);
                break;
            case LogType.Warning:
                DebugConsole.LogWarning(logString);
                break;
            case LogType.Error:
                DebugConsole.LogError(string.Format("{0}{1}", logString, stackTrace));
                break;
            case LogType.Assert:
                DebugConsole.Log(string.Format("[Assert]:{0}{1}", logString, stackTrace), Color.blue);
                break;
            case LogType.Exception:
                DebugConsole.Log(string.Format("[Exception]:{0}{1}", logString, stackTrace), Color.magenta);
                break;
        }
    }


    public static bool ExistsApi(string method, out string className)
    {
        if (AppStartupApi.Instance.ExistsApi(method)) {
            className = "AppStartupApi";
            return true;
        }
        var t = typeof(Game);
        var mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (null != mi) {
            className = "Game";
            return true;
        }
        className = string.Empty;
        return false;
    }
    private static bool GetApi(string method, out StartupScript.ApiDelegation api)
    {
        api = AppStartupApi.Instance.GetApi(method);
        if (null != api) {
            return true;
        }
        var t = typeof(Game);
        var mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (null != mi) {
            var delegation = System.Delegate.CreateDelegate(typeof(StartupScript.ApiDelegation), mi, false);
            if (null != delegation)
                api = (StartupScript.ApiDelegation)delegation;
            else {
                if (!s_ApiCache.TryGetValue(method, out api)) {
                    var invoker = new StartupApi.ApiInvoker(mi, null);
                    api = new StartupScript.ApiDelegation(invoker.Invoke);
                    s_ApiCache[method] = api;
                }
            }
            return true;
        }
        return false;
    }

    public static void RunStartup(bool runGm)
    {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
        bool needRun = false;
        for (int i = 0; i < StartupApi.c_max_startup_cfgs; ++i) {
            string file = Path.Combine(StartupScript.ScriptPath, string.Format("startup{0}.dsl", i));
            if (File.Exists(file)) {
                if (!needRun) {
                    StartupScript.TryInit();
                    StartupScript.ClearStartups();
                    needRun = true;
                }
                StartupScript.LoadStartup(file);
            }
        }
        if (needRun) {
            StartupScript.RunStartup(out var gmtxt);
            if (runGm) {
                RunInitGM(gmtxt);
            }
        }
        else {
            //Open later on demand
            //StartupScript.RunStartupOnlyCsharp();
            if (runGm) {
                RunInitGM(string.Empty);
            }
        }
#endif
    }
    public static void RunInitGM(string gmtxt)
    {
        if (!string.IsNullOrEmpty(gmtxt)) {
            GmRootScript.TryInit();
            TryFixGM();
            GameObject obj = GmRootScript.GameObj;
            if (null != obj) {
                DebugConsole.Execute(gmtxt);
            }
        }
        else {
            string path = Application.persistentDataPath;
            if (Application.platform == RuntimePlatform.Android) {
                path = "/data/local/tmp";
            }
            string file = Path.Combine(path, "initgm.txt");
            if (File.Exists(file)) {
                GmRootScript.TryInit();
                TryFixGM();
                GameObject obj = GmRootScript.GameObj;
                if (null != obj) {
                    //DebugConsole.Execute will reset the dsl, so the commands must be concatenated into one command
                    var sb = new StringBuilder();
                    var lines = File.ReadAllLines(file);
                    foreach (string line in lines) {
                        string str = line.Trim();
                        if (!str.StartsWith("//") && !str.StartsWith('#')) {
                            if (str.IndexOf(';') < 0 && str.IndexOf('(') < 0 && str.IndexOf(')') < 0 && str.IndexOfAny(s_WhiteChars) > 0) {
                                sb.Append("cmd(\"");
                                sb.Append(str.Replace("\"", "\\\""));
                                sb.Append("\");");
                            }
                            else {
                                sb.Append(str);
                                if (!str.EndsWith(";"))
                                    sb.Append(";");
                            }
                        }
                    }
                    DebugConsole.Execute(sb.ToString());
                }
            }
            else {
#if UNITY_EDITOR
                GmRootScript.TryInit();
                TryFixGM();
#elif UNITY_STANDALONE
#if DEVELOPMENT_BUILD
            GmRootScript.TryInit();
            TryFixGM();
#endif
#elif UNITY_ANDROID
#if DEVELOPMENT_BUILD
            GmRootScript.TryInit();
            TryFixGM();
#endif
#endif
            }
        }
    }
    private static void TryFixGM()
    {
        var gmObj = GmRootScript.GameObj;
        if (null != gmObj) {
            var uiHandler = gmObj.GetComponent<UiHandler>();
            var debugConsole = gmObj.GetComponent<DebugConsole>();
            var gmScp = gmObj.GetComponent<GmRootScript>();
            if (null == uiHandler || null == debugConsole || null == gmScp) {
                // some components are missing: gmObj may carry missing-script placeholders,
                // clean them up before reinstalling
#if UNITY_EDITOR
                if (UnityEditor.GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gmObj) > 0) {
                    UnityEditor.GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gmObj);
                    Debug.Log("TryFixGM: removed missing scripts on " + gmObj.name);
                }
#else
                // no runtime api to remove missing scripts; count and log for diagnosis
                var comps = gmObj.GetComponents<Component>();
                int missingCount = 0;
                for (int i = 0; i < comps.Length; ++i) {
                    if (null == comps[i]) {
                        ++missingCount;
                    }
                }
                if (missingCount > 0) {
                    Debug.LogWarning("TryFixGM: " + missingCount + " missing scripts on " + gmObj.name + " (cannot remove at runtime)");
                }
#endif
                gmObj.SetActive(false);
                if (null == uiHandler)
                {
                    uiHandler = UiHandler.TryInstall(gmObj);
                }
                if (null == debugConsole)
                {
                    debugConsole = StoryScriptUtility.TryInstallComponent<DebugConsole>(gmObj);
                }
                if (null == gmScp)
                {
                    gmScp = StoryScriptUtility.TryInstallComponent<GmRootScript>(gmObj);
                }
                if (null == uiHandler || null == debugConsole || null == gmScp)
                {
                    // Fix failed.
                }
                else
                {
                    gmObj.SetActive(true);
                    GmRootScript.TryInit();
                }
            }
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
                    // Ray must reach the ground from the spectator camera: at
                    // distance=104 the centre ray is ~104 long and the top-edge
                    // ray ~209, so the old 100 limit made click-to-move miss.
                    if (Physics.Raycast(ray, out hit, 500, (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Default")))) {
                        int objId = PluginFramework.Instance.GetGameObjectId(hit.collider.gameObject);
                        if (objId > 0) {
                            PluginFramework.Instance.ClickNpc(objId);
                        }
                        else {
                            // Raycast hits world space; MoveTo and the battle
                            // story expect logical local coordinates.
                            Vector3 pos = SceneOrigin.ToLocal(hit.point, SceneOrigin.Current);
                            PluginFramework.Instance.MoveTo(pos.x, pos.y, pos.z);
                        }
                    }
                }
            }
        }

        // Active exploration: the player must press E at a visually hinted spot.
        // Local cooldown is UX only; the server enforces the authoritative gate.
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= m_NextExploreTime) {
            m_NextExploreTime = Time.time + 2.0f;
            GfxStorySystem.Instance.SendMessage("cast_explore");
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
        // An Additive battlefield is layered on top of an already loaded base
        // scene. Re-running the UI initialisation here used to create a second
        // UiCamera/TopMenu and subscribe every OverHeadBar handler again.
        // Still call OverHeadBarManager.Init() (now idempotent) because a base
        // scene with buildIndex==2 never initialises it.
        if (loadSceneMode == LoadSceneMode.Additive) {
            OverHeadBarManager.Instance.Init();
            m_CameraController = new CameraController(Camera.main);
            return;
        }
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
        // Capture the freshly loaded battlefield scene. From now on every
        // runtime object created through ResourceSystem.NewObject is moved
        // into this scene, so a single UnloadSceneAsync removes them all.
        ResourceSystem.Instance.SetBattleScene(FindBattleScene(lvl.prefab));
        PluginFramework.Instance.OnBattleLoaded(lvl);
    }

    private Scene FindBattleScene(string prefab)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i) {
            Scene sc = SceneManager.GetSceneAt(i);
            if (sc.isLoaded && sc.name == prefab)
                return sc;
        }
        return new Scene();
    }

    private IEnumerator UnloadBattle(TableConfig.Level lvl)
    {
        // The active controller holds the battle-scene camera, which is about
        // to be destroyed. Drop it before any LateUpdate can run.
        m_CameraController = null;
        RestoreMainSceneVisuals();

        // Recycle pooled objects that live in the battlefield scene BEFORE
        // unloading it. This keeps the ResourceSystem ledger valid (objects go
        // back to the DontDestroyOnLoad ResPool instead of being destroyed
        // behind its back). Anything not in the ledger is destroyed by the
        // scene unload itself.
        ResourceSystem.Instance.RecycleBattleObjects();
        OverHeadBarManager.Instance.ClearBattleViews();

        LoadingManager.Instance.Show();
        yield return null;
        LoadingManager.Instance.SetProgress(0.1f);
        // Wait for the unload to actually complete. Fire-and-forget used to let
        // on_battle_closed / the next battle start while the old scene was
        // still being torn down.
        var unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(lvl.prefab);
        if (null != unload)
            yield return unload;
        ResourceSystem.Instance.ClearBattleScene();
        LoadingManager.Instance.SetProgress(0.9f);
        yield return Resources.UnloadUnusedAssets();
        LoadingManager.Instance.SetProgress(1.0f);
        // Battle camera is gone and the story-scene camera is restored; bind
        // a fresh controller to the surviving Camera.main.
        m_CameraController = (Camera.main != null) ? new CameraController(Camera.main) : null;
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
        // A battlefield is loaded Additive on top of the base (world/story)
        // scene. Hide every other loaded scene's static visuals so only the
        // battlefield renders. Identify it by the level's prefab name rather
        // than the historical StartsWith("Battle") check: the 64 world-map
        // scenes are also named Battle_*, so that prefix used to leave two
        // terrains, two MainCameras and two suns enabled at the same time.
        string battleSceneName = string.Empty;
        TableConfig.Level battleLevel = TableConfig.LevelProvider.Instance.GetLevel(levelId);
        if (null != battleLevel) battleSceneName = battleLevel.prefab;
        m_HiddenVisuals.Clear();
        for (int i = 0; i < SceneManager.sceneCount; ++i) {
            Scene sc = SceneManager.GetSceneAt(i);
            bool isBattleScene = battleSceneName.Length > 0
                ? sc.name == battleSceneName
                : sc.name.StartsWith("Battle");
            if (!sc.isLoaded || isBattleScene) continue;
            foreach (GameObject go in sc.GetRootGameObjects()) {
                if (go == null) continue;
                foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true)) HideVisual(r);
                foreach (Camera c in go.GetComponentsInChildren<Camera>(true)) HideVisual(c);
                foreach (Light l in go.GetComponentsInChildren<Light>(true)) HideVisual(l);
                foreach (Terrain t in go.GetComponentsInChildren<Terrain>(true)) HideVisual(t);
            }
        }
        // sceneLoaded fires before the story-scene cameras are disabled, so the
        // CameraController created in OnLevelLoaded may be bound to the wrong
        // camera. Rebind now that only the battle camera is active.
        if (Camera.main != null)
            m_CameraController = new CameraController(Camera.main);
    }

    private void RestoreMainSceneVisuals()
    {
        foreach (var kv in m_HiddenVisuals)
            if (kv.Key != null) SetVisualEnabled(kv.Key, kv.Value);
        m_HiddenVisuals.Clear();
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
            // Targets come from MovementStateInfo (logical); the camera lives
            // in world space at the active scene origin.
            Vector3 target = SceneOrigin.ToWorld(new Vector3(poses[0], poses[1], poses[2]), SceneOrigin.Current);
            m_CameraController.ClearFollow();
            m_CameraController.MoveFollowPath(target, true);
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
        GfxStorySystem.Instance.SendMessage("camera_follow_path");
    }

    private void LogToConsole(string msg)
    {
        DebugConsole.Log(msg);
    }

    private void OnResetDsl()
    {
        ScriptableFramework.Utility.EventSystem.Publish("gm_resetdsl", "gm");
    }

    private void OnExecScript(string script)
    {
        ScriptableFramework.Utility.EventSystem.Publish("gm_execscript", "gm", script);
    }

    private void OnExecCommand(string command)
    {
        ScriptableFramework.Utility.EventSystem.Publish("gm_execcommand", "gm", command);
    }
	
	public static void LoadScript(string file)
    {
        var basePath = Application.persistentDataPath;
        if (!System.IO.Path.IsPathRooted(file)) {
            file = System.IO.Path.Combine(basePath, file);
        }
        StartupScript.Calculator.LoadDsl(file);
    }
    public static StoryScript.BoxedValue EvalAndRun(string code)
    {
        StoryScript.BoxedValue r = StoryScript.BoxedValue.EmptyString;
        var file = new Dsl.DslFile();
        if (file.LoadFromString(code, msg => Debug.LogWarning(msg))) {
            r = EvalAndRun(file.DslInfos);
        }
        return r;
    }
    public static StoryScript.BoxedValue EvalAndRun(params ISyntaxComponent[] expressions)
    {
        IList<ISyntaxComponent> exps = expressions;
        return EvalAndRun(exps);
    }
    public static StoryScript.BoxedValue EvalAndRun(IList<ISyntaxComponent> expressions)
    {
        StoryScript.BoxedValue r = StoryScript.BoxedValue.EmptyString;
        List<IExpression> exps = new List<IExpression>();
        StartupScript.Calculator.LoadDsl(expressions, exps);
        r = StartupScript.Calculator.CalcInCurrentContext(exps);
        return r;
    }
    public static void EvalAsFunc(string name, Dsl.FunctionData func, IList<string> argNames)
    {
        Debug.Assert(null != func);
        StartupScript.Calculator.LoadDsl(name, argNames, func);
    }
    public static bool TryGetApiFactory(string name, out IExpressionFactory factory)
    {
        return StartupScript.Calculator.CodeStructure.ApiRegistry.TryGetFactory(name, out factory);
    }
    public static object Call(string func, params object[] args)
    {
        if (null == s_Instance)
            return null;
        var vargs = StartupScript.Calculator.NewCalculatorValueList();
        foreach (var arg in args) {
            vargs.Add(StoryScript.BoxedValue.FromObject(arg));
        }
        var r = StartupScript.Calculator.Calc(func, vargs);
        StartupScript.Calculator.RecycleCalculatorValueList(vargs);
        return r.GetObject();
    }
    public static void RegisterGmApi(DslCalculatorApiRegistry registry)
    {
        registry.Register("load_scp", "load_scp(dsl_script_file) or load_scp(\"name\", func(param1, param2, ...)) or load_scp(name => func(param1, param2, ...)) command", new ExpressionFactoryHelper<LoadScriptCommand>());
        registry.Register("call_scp", "call_scp(\"func\", arg1, arg2, ...) or call_scp(apiOrFunc(arg1, arg2, ...)) command", new ExpressionFactoryHelper<CallScriptCommand>());
        registry.Register("eval_scp", "eval_scp(code_str) or eval_scp(exp1,exp2,...) command", new ExpressionFactoryHelper<EvalScriptCommand>());
        registry.Register("show_memory", "show_memory() command", new ExpressionFactoryHelper<ShowMemoryExp>());
        registry.Register("get_pss", "get_pss() function", new ExpressionFactoryHelper<GetPssExp>());
        registry.Register("get_vss", "get_vss() function", new ExpressionFactoryHelper<GetVssExp>());
        registry.Register("get_native", "get_native() function", new ExpressionFactoryHelper<GetNativeExp>());
        registry.Register("get_graphics", "get_graphics() function", new ExpressionFactoryHelper<GetGraphicsExp>());
        registry.Register("get_unknown", "get_unknown() function", new ExpressionFactoryHelper<GetUnknownExp>());
        registry.Register("get_java", "get_java() function", new ExpressionFactoryHelper<GetJavaExp>());
        registry.Register("get_code", "get_code() function", new ExpressionFactoryHelper<GetCodeExp>());
        registry.Register("get_stack", "get_stack() function", new ExpressionFactoryHelper<GetStackExp>());
        registry.Register("get_system", "get_system() function", new ExpressionFactoryHelper<GetSystemExp>());
        registry.Register("get_activity", "get_activity() function", new ExpressionFactoryHelper<GetActivityExp>());
        registry.Register("get_intent", "get_intent() function", new ExpressionFactoryHelper<GetIntentExp>());
    }

    private static Game s_Instance = null;
    private static char[] s_WhiteChars = new char[] { ' ', '\t' };
    private static readonly Dictionary<string, StartupScript.ApiDelegation> s_ApiCache = new Dictionary<string, StartupScript.ApiDelegation>();
}


internal static class MemoryInfo
{
    internal static float GetAppMemory()
    {
#if UNITY_ANDROID
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        float ret = s_MemoryActivity.CallStatic<int>("getTotalPss")/1024.0f;
        return ret;
#elif UNITY_IOS
        return ios_GetAppMemory();
#else
        return 0;
#endif
    }
    internal static float GetNativeMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.native-heap")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.native-heap")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetGraphicsMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.graphics")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.graphics")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetUnknownMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.private-other")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.private-other")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetJavaMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.java-heap")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.java-heap")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetCodeMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.code")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.code")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetStackMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.stack")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.stack")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetSystemMemory()
    {
#if UNITY_ANDROID
        /*
        if (null == s_DebugObj) {
            s_DebugObj = new AndroidJavaObject("android.os.Debug");
        }
        if (null == s_MemoryInfoObj) {
            s_MemoryInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }
        s_DebugObj.CallStatic("getMemoryInfo", s_MemoryInfoObj);
        return int.Parse(s_MemoryInfoObj.Call<string>("getMemoryStat", "summary.system")) / 1024.0f;
        */
        if (null == s_MemoryActivity) {
            s_MemoryActivity = new AndroidJavaClass("com.aaa.bbb.MainActivity");
        }
        var infos = s_MemoryActivity.CallStatic<AndroidJavaObject[]>("getMemoryInfo");
        float mem = 0;
        if (null != infos && infos.Length > 0) {
            mem = int.Parse(infos[0].Call<string>("getMemoryStat", "summary.system")) / 1024.0f;
        }
        return mem;
#else
        return 0;
#endif
    }
    internal static float GetVssMemory()
    {
        float vss = 0;
#if UNITY_ANDROID && !UNITY_EDITOR
        using (FileStream reader = new FileStream("/proc/self/status", FileMode.Open, FileAccess.Read, FileShare.Read, s_VssBuffer.Length, FileOptions.SequentialScan)) {
            int ct = reader.Read(s_VssBuffer, 0, s_VssBuffer.Length);
            int index = -1;
            int k = 0;
            for (int i = 0; i < ct && k < s_VmPeak.Length; ++i) {
                if (s_VssBuffer[i] == s_VmPeak[k]) {
                    ++k;
                    if (k == s_VmPeak.Length) {
                        index = i + 1;
                        break;
                    }
                }
                else {
                    k = 0;
                }
            }
            if (index >= 0) {
                for (; index < ct && (s_VssBuffer[index] == ' ' || s_VssBuffer[index] == '\t'); ++index) ;
                int vssKb = 0;
                for (; index < ct; ++index) {
                    int num = s_VssBuffer[index] - '0';
                    if (num < 0 || num > 9)
                        break;

                    vssKb = vssKb * 10 + num;
                }
                vss = vssKb / 1024.0f;
            }
            reader.Close();
        }
#endif
        return vss;
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private static byte[] s_VssBuffer = new byte[1024];
    private static byte[] s_VmPeak = new byte[] { (byte)'V', (byte)'m', (byte)'P', (byte)'e', (byte)'a', (byte)'k', (byte)':' };
#endif
#if UNITY_ANDROID
    private static AndroidJavaClass s_MemoryActivity = null;
    private static AndroidJavaObject s_DebugObj = null;
    private static AndroidJavaObject s_MemoryInfoObj = null;
#endif
#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern float ios_GetAppMemory();
#endif
}

namespace ExpressionAPI
{
    using BoxedValue = StoryScript.BoxedValue;
    /// <summary>
    /// load_scp(dsl_script_file);
    /// or
    /// load_scp("name", apiOrFunc(param1, param2, ...));
    /// or
    /// load_scp(name => apiOrFunc(param1, param2, ...));
    /// </summary>
    internal sealed class LoadScriptCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string f = null != m_FileOrId ? m_FileOrId.Calc(calculator).ToString() : string.Empty;
            if (m_ArgNum == 1) {
                if (string.IsNullOrEmpty(m_Name)) {
                    Game.LoadScript(f);
                }
                else {
                    f = m_Name;
                    var func = new Dsl.FunctionData();
                    func.AddParam(m_FuncCall);
                    Game.EvalAsFunc(f, func, m_Params);
                }
            }
            else if (m_ArgNum == 2 && null != m_FuncCall) {
                var func = new Dsl.FunctionData();
                func.AddParam(m_FuncCall);
                Game.EvalAsFunc(f, func, m_Params);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ArgNum = callData.GetParamNum();
            if (m_ArgNum > 0) {
                var p0 = callData.GetParam(0);
                var func = p0 as Dsl.FunctionData;
                if (null != func && func.IsOperatorParamClass() && func.GetId() == "=>") {
                    m_Name = func.GetParamId(0);
                    m_FuncCall = func.GetParam(1) as Dsl.FunctionData;
                    if (null != m_FuncCall) {
                        foreach (var p in m_FuncCall.Params) {
                            m_Params.Add(p.GetId());
                        }
                    }
                }
                else {
                    m_FileOrId = CodeStructure.Load(p0);
                    if (m_ArgNum > 1) {
                        m_FuncCall = callData.GetParam(1) as Dsl.FunctionData;
                        if (null != m_FuncCall) {
                            foreach (var p in m_FuncCall.Params) {
                                m_Params.Add(p.GetId());
                            }
                        }
                    }
                }
            }
            return true;
        }

        private int m_ArgNum = 0;
        private IExpression m_FileOrId;
        private string m_Name = string.Empty;
        private Dsl.FunctionData m_FuncCall;
        private List<string> m_Params = new List<string>();
    }
    /// <summary>
    /// call_scp("func", arg1, arg2, ...);
    /// or
    /// call_scp(apiOrFunc(arg1, arg2, ...));
    /// </summary>
    internal sealed class CallScriptCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            if (null != m_Api) {
                return m_Api.Calc(calculator);
            }
            else {
                string func = m_FuncName;
                if (string.IsNullOrEmpty(m_FuncName))
                    func = null != m_Func ? m_Func.Calc(calculator).ToString() : string.Empty;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_Args.Count; ++i) {
                    arglist.Add(m_Args[i].Calc(calculator).GetObject());
                }
                object[] args = arglist.ToArray();
                return BoxedValue.FromObject(Game.Call(func, args));
            }
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                var p0 = callData.GetParam(0);
                var func = p0 as Dsl.FunctionData;
                if (null != func) {
                    var f = func.GetId();
                    if (Game.TryGetApiFactory(f, out var factory)) {
                        m_Api = factory.Create();
                        m_Api.Load(p0, CodeStructure);
                    }
                    else {
                        m_FuncName = f;
                        for (int i = 0; i < func.GetParamNum(); ++i) {
                            m_Args.Add(CodeStructure.Load(func.GetParam(i)));
                        }
                    }
                }
                else {
                    m_Func = CodeStructure.Load(p0);
                    for (int i = 1; i < callData.GetParamNum(); ++i) {
                        m_Args.Add(CodeStructure.Load(callData.GetParam(i)));
                    }
                }
            }
            return true;
        }

        private IExpression m_Api;

        private string m_FuncName = string.Empty;
        private IExpression m_Func;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// eval_scp(code_string);
    /// or
    /// eval_scp(code_dsl);
    /// </summary>
    internal sealed class EvalScriptCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            if (m_IsString)
                return Game.EvalAndRun(m_Code.Calc(calculator).ToString());
            else
                return Game.EvalAndRun(m_Exps);
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                var vd = callData.GetParam(0) as Dsl.ValueData;
                if (null != vd && vd.IsString()) {
                    m_IsString = true;
                    m_Code = CodeStructure.Load(vd);
                }
                else {
                    m_IsString = false;
                    foreach (var p in callData.Params) {
                        m_Exps.Add(p);
                    }
                }
            }
            return true;
        }

        private bool m_IsString = false;
        private IExpression m_Code;
        private List<Dsl.ISyntaxComponent> m_Exps = new List<Dsl.ISyntaxComponent>();
    }
    internal sealed class ShowMemoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string info = string.Format("pss:{0} n:{1} g:{2} u:{3} j:{4} c:{5} t:{6} s:{7} vss:{8}", MemoryInfo.GetAppMemory(), MemoryInfo.GetNativeMemory(), MemoryInfo.GetGraphicsMemory(), MemoryInfo.GetUnknownMemory(), MemoryInfo.GetJavaMemory(), MemoryInfo.GetCodeMemory(), MemoryInfo.GetStackMemory(), MemoryInfo.GetSystemMemory(), MemoryInfo.GetVssMemory());
            Debug.LogFormat("{0}", info);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetPssExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetAppMemory();
        }
    }
    internal sealed class GetVssExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetVssMemory();
        }
    }
    internal sealed class GetNativeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetNativeMemory();
        }
    }
    internal sealed class GetGraphicsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetGraphicsMemory();
        }
    }
    internal sealed class GetUnknownExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetUnknownMemory();
        }
    }
    internal sealed class GetJavaExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetJavaMemory();
        }
    }
    internal sealed class GetCodeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetCodeMemory();
        }
    }
    internal sealed class GetStackExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetStackMemory();
        }
    }
    internal sealed class GetSystemExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return MemoryInfo.GetSystemMemory();
        }
    }
    internal sealed class GetActivityExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            return BoxedValue.FromObject(new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal sealed class GetIntentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            return BoxedValue.FromObject(act.Call<AndroidJavaObject>("getIntent"));
#else
            return BoxedValue.NullObject;
#endif
        }
    }

    internal sealed class SetClipboardExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
#if UNITY_EDITOR
                    GUIUtility.systemCopyBuffer = str;
#elif UNITY_IOS
                    SetClipboard(str);
#elif UNITY_ANDROID
                    AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");
                    cb.CallStatic ("setText", str);
#endif
                    r = str;
                }
            }
            return r;
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void SetClipboard(string str);
#endif
    }
    internal sealed class GetClipboardExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            r = GUIUtility.systemCopyBuffer;
#elif UNITY_IOS
            r = GetClipboard();
#elif UNITY_ANDROID
            AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");
            r = cb.CallStatic<string>("getText");
#endif
            return r;
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern string GetClipboard();
#endif
    }
}

