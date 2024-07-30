//#define EMBED_ONGUI

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
#define MOBILE
#endif

using ScriptableFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugConsole : MonoBehaviour
{
    public delegate object DebugCommand(params string[] args);

    public int MaxLinesForDisplay = 500;

    public Color DefaultColor = Message.DefaultColor;
    public Color WarningColor = Message.WarningColor;
    public Color ErrorColor = Message.ErrorColor;
    public Color SystemColor = Message.SystemColor;
    public Color InputColor = Message.InputColor;
    public Color OutputColor = Message.OutputColor;

    public enum MessageType
    {
        NORMAL,
        WARNING,
        ERROR,
        SYSTEM,
        INPUT,
        OUTPUT
    }

    struct Message
    {
        public Color Color
        {
            get {
                return m_Color;
            }
        }

        public static Color DefaultColor = Color.white;
        public static Color WarningColor = Color.yellow;
        public static Color ErrorColor = Color.red;
        public static Color SystemColor = Color.green;
        public static Color InputColor = Color.green;
        public static Color OutputColor = Color.cyan;

        public Message(object messageObject)
            : this(messageObject, MessageType.NORMAL, Message.DefaultColor)
        {
        }

        public Message(object messageObject, Color displayColor)
            : this(messageObject, MessageType.NORMAL, displayColor)
        {
        }

        public Message(object messageObject, MessageType messageType)
            : this(messageObject, messageType, Message.DefaultColor)
        {
            switch (messageType) {
                case MessageType.ERROR:
                    m_Color = ErrorColor;
                    break;
                case MessageType.SYSTEM:
                    m_Color = SystemColor;
                    break;
                case MessageType.WARNING:
                    m_Color = WarningColor;
                    break;
                case MessageType.OUTPUT:
                    m_Color = OutputColor;
                    break;
                case MessageType.INPUT:
                    m_Color = InputColor;
                    break;
            }
        }

        public Message(object messageObject, MessageType messageType, Color displayColor)
        {
            m_Text = (messageObject == null ? "<null>" : messageObject.ToString());
            m_Formatted = string.Empty;
            m_Type = messageType;
            m_Color = displayColor;
        }

        public static Message Log(object message)
        {
            return new Message(message, MessageType.NORMAL, DefaultColor);
        }

        public static Message System(object message)
        {
            return new Message(message, MessageType.SYSTEM, SystemColor);
        }

        public static Message Warning(object message)
        {
            return new Message(message, MessageType.WARNING, WarningColor);
        }

        public static Message Error(object message)
        {
            return new Message(message, MessageType.ERROR, ErrorColor);
        }

        public static Message Output(object message)
        {
            return new Message(message, MessageType.OUTPUT, OutputColor);
        }

        public static Message Input(object message)
        {
            return new Message(message, MessageType.INPUT, InputColor);
        }

        public override string ToString()
        {
            switch (m_Type) {
                case MessageType.ERROR:
                    return string.Format("[{0}] {1}", m_Type, m_Text);
                case MessageType.WARNING:
                    return string.Format("[{0}] {1}", m_Type, m_Text);
                default:
                    return ToGUIString();
            }
        }

        public string ToGUIString()
        {
            if (!string.IsNullOrEmpty(m_Formatted)) {
                return m_Formatted;
            }

            switch (m_Type) {
                case MessageType.INPUT:
                    m_Formatted = string.Format(">>> {0}", m_Text);
                    break;
                case MessageType.OUTPUT:
                    var lines = m_Text.Trim('\n').Split('\n');
                    var output = new StringBuilder();

                    foreach (var line in lines) {
                        output.AppendFormat("= {0}\n", line);
                    }

                    m_Formatted = output.ToString();
                    break;
                case MessageType.SYSTEM:
                    m_Formatted = string.Format("# {0}", m_Text);
                    break;
                case MessageType.WARNING:
                    m_Formatted = string.Format("* {0}", m_Text);
                    break;
                case MessageType.ERROR:
                    m_Formatted = string.Format("** {0}", m_Text);
                    break;
                default:
                    m_Formatted = m_Text;
                    break;
            }

            return m_Formatted;
        }

        private string m_Text;
        private string m_Formatted;
        private MessageType m_Type;
        private Color m_Color;
    }

    class History
    {
        public void Add(string item)
        {
            DoRemoveAll(item);
            m_History.Add(item);
            m_Index = 0;
        }

        public string Fetch(string current, bool next, string filter = "")
        {
            List<string> list = (filter == null || filter == string.Empty || filter == "`" || filter == "~") ? m_History : DoFilter(filter);
            if (m_Index == 0) {
                m_Current = current;
            }

            if (list.Count == 0) {
                return current;
            }

            m_Index += next ? -1 : 1;

            if (list.Count + m_Index < 0 || list.Count + m_Index > list.Count - 1) {
                m_Index = 0;
                return m_Current;
            }

            var result = list[list.Count + m_Index];

            return result;
        }

        public override string ToString()
        {
            string rlt = "";
            for (int i = 0; i < m_History.Count; i++) {
                if (i > m_History.Count - 500) {
                    rlt += m_History[i];
                    rlt += i < m_History.Count - 1 ? "\n" : "";
                }
            }
            return rlt;
        }
        public void FromString(string s)
        {
            m_History = new List<string>(s.Split('\n'));
        }

        private void DoRemoveAll(string item)
        {
            var deletes = new Stack<int>();
            for (int ix = 0; ix < m_History.Count; ++ix) {
                var str = m_History[ix];
                if (str == item) {
                    deletes.Push(ix);
                }
            }
            while (deletes.Count > 0) {
                int ix = deletes.Pop();
                m_History.RemoveAt(ix);
            }
        }
        private List<string> DoFilter(string filter)
        {
            var list = new List<string>();
            foreach (var msg in m_History) {
                if (msg.StartsWith(filter)) {
                    list.Add(msg);
                }
            }
            return list;
        }

        private List<string> m_History = new List<string>();
        private int m_Index = 0;
        private string m_Current = string.Empty;
    }

    void Awake()
    {
        if (s_Instance != null && s_Instance != this) {
            DestroyImmediate(this, true);
            return;
        }

        s_Instance = this;
    }

    void OnEnable()
    {
        m_InputFilter = string.Empty;
        m_Filter = string.Empty;
        m_History.FromString(PlayerPrefs.GetString("debug_console_history"));
#if !EMBED_ONGUI
        m_InitialScreenWidth = Screen.width;

        m_WindowMethods = new GUI.WindowFunction[] { LogWindow, CopyLogWindow };

        Message.DefaultColor = DefaultColor;
        Message.WarningColor = WarningColor;
        Message.ErrorColor = ErrorColor;
        Message.SystemColor = SystemColor;
        Message.InputColor = InputColor;
        Message.OutputColor = OutputColor;
#if MOBILE
        this.useGUILayout = false;
        float left = 5.0f;
        float top = 5.0f;
        float width = Screen.width - left * 2;
        float height = Screen.height - top * 2;
        m_WindowRect = new Rect(left, top, width, height);
        m_FakeWindowRect = new Rect(0.0f, 0.0f, m_WindowRect.width, m_WindowRect.height);
        m_FakeDragRect = new Rect(0.0f, 0.0f, m_WindowRect.width - 32, 24);
        m_ScrollRect = new Rect(10, 20, width - 20, height - 110);
        m_InputRect = new Rect(10, height - 80, width - 110, 30);
        m_EnterRect = new Rect(width - 90, height - 80, 80, 30);
        m_ToolbarRect = new Rect(16, height - 40, width - 32, 30);
        m_MessageLine = new Rect(4, 0, width - 8, 30);
#else
        float left = 30.0f;
        float top = 30.0f;
        float width = Screen.width - left * 2;
        float height = Screen.height - top * 2;
        m_WindowRect = new Rect(left, top, width, height);
        m_ScrollRect = new Rect(10, 20, width - 20, height - 110);
        m_InputRect = new Rect(10, height - 80, width - 110, 30);
        m_EnterRect = new Rect(width - 90, height - 80, 80, 30);
        m_ToolbarRect = new Rect(16, height - 40, width - 32, 30);
        m_MessageLine = new Rect(4, 0, width - 8, 30);
#endif

#endif //EMBED_ONGUI

        LogMessage(Message.System(string.Format(" DebugConsole version {0}", VERSION)));
        LogMessage(Message.System(" Copyright 2008-2010 Jeremy Hollingsworth "));
        LogMessage(Message.System(" Ennanzus-Interactive.com "));
        LogMessage(Message.System(" type '/?' for available commands."));
        LogMessage(Message.System(" type '/? filter' for available story commands and script apis."));
        LogMessage(Message.Log(""));

        this.RegisterCommandCallback("open", CMDOpen);
        this.RegisterCommandCallback("close", CMDClose);
        this.RegisterCommandCallback("clear", CMDClear);
        this.RegisterCommandCallback("sys", CMDSystemInfo);
        this.RegisterCommandCallback("quality", CMDQuality);
        this.RegisterCommandCallback("vsync", CMDVSync);
        this.RegisterCommandCallback("resetdsl", CMDResetDsl);
        this.RegisterCommandCallback("script", CMDScript);
        this.RegisterCommandCallback("scp", CMDScript);
        this.RegisterCommandCallback("command", CMDCommand);
        this.RegisterCommandCallback("cmd", CMDCommand);
        this.RegisterCommandCallback("gm", CMDGm);
        this.RegisterCommandCallback("filter", CMDFilter);
        this.RegisterCommandCallback("/?", CMDHelp);
    }

    private void ShowImpl()
    {
        m_IsOpen = true;
        m_Filter = string.Empty;
        m_InputFilter = string.Empty;
    }

    private void HideImpl()
    {
        m_IsOpen = false;
    }

#if EMBED_ONGUI
    void OnEmbedGUI()
    {
        var evt = Event.current;

        // Create a custom GUI skin
        GUISkin customSkin = GUI.skin;

        // Set the width of the scroll bar
        customSkin.verticalScrollbar.fixedWidth = 30;
        customSkin.horizontalScrollbar.fixedHeight = 30;

        // Set the size of the scroll bar slider
        customSkin.verticalScrollbarThumb.fixedWidth = 30;
        customSkin.horizontalScrollbarThumb.fixedHeight = 30;

        // Apply a custom skin
        GUI.skin = customSkin;

        GUILayout.BeginHorizontal();
        m_ViewScrollPos = GUILayout.BeginScrollView(m_ViewScrollPos, true, true);

        if (m_Messages.Count > 0)
        {
            Color oldColor = GUI.contentColor;
            foreach (Message m in m_Messages)
            {
                string txt = m.ToString();
                if (!txt.Contains(m_Filter))
                    continue;

                GUI.contentColor = m.Color;
                string text = m.ToGUIString();
                GUILayout.Label(text);
            }
            GUI.contentColor = oldColor;
        }

        GUILayout.EndScrollView();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUI.SetNextControlName(ENTRYFIELD);
        m_InputText = GUILayout.TextField(m_InputText);
        if (GUILayout.Button("Run", GUILayout.Width(80)))
        {
            EvalInputString(m_InputText);
        }

        GUILayout.EndHorizontal();

        if (GUI.GetNameOfFocusedControl() == ENTRYFIELD)
        {
            if (evt.isKey && evt.type == EventType.KeyUp)
            {
                if (evt.keyCode == KeyCode.Return)
                {
                    EvalInputString(m_InputText);
                    m_InputText = string.Empty;
                    m_InputFilter = string.Empty;
                }
                else if (evt.keyCode == KeyCode.UpArrow)
                {
                    m_InputText = m_History.Fetch(m_InputText, true, m_InputFilter);
                }
                else if (evt.keyCode == KeyCode.DownArrow)
                {
                    m_InputText = m_History.Fetch(m_InputText, false, m_InputFilter);
                }
                else
                {
                    m_InputFilter = m_InputText;
                }
            }
        }

        GUI.FocusControl(ENTRYFIELD);
    }
#else
    [Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    void OnGUI()
    {
        var evt = Event.current;

        // Create a custom GUI skin
        GUISkin customSkin = GUI.skin;

        customSkin.label.fontSize = 20;
        customSkin.textField.fontSize = 20;
        customSkin.textArea.fontSize = 20;
        customSkin.textField.fixedHeight = 30;

        // Set button size
        customSkin.button.fixedHeight = 30;
        customSkin.button.fontSize = 20;

        // Set the width of the scroll bar
        customSkin.verticalScrollbar.fixedWidth = 20;
        customSkin.horizontalScrollbar.fixedHeight = 20;

        // Set the size of the scroll bar slider
        customSkin.verticalScrollbarThumb.fixedWidth = 20;
        customSkin.horizontalScrollbarThumb.fixedHeight = 20;

        // Apply a custom skin
        GUI.skin = customSkin;

        var scale = Screen.width / m_InitialScreenWidth;
        bool scaled = false;
        var guiScale = Vector3.one;
        if (scale != 1.0f) {
            scaled = true;
            guiScale = new Vector3(scale, scale, scale);
        }
        if (scaled) {
            m_RestoreMatrix = GUI.matrix;

            GUI.matrix = GUI.matrix * Matrix4x4.Scale(guiScale);
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR || MOBILE
        // Toggle key shows the console
        if (evt.keyCode == ToggleKey && evt.type == EventType.KeyUp)
            m_IsOpen = !m_IsOpen;
#endif
#if MOBILE
    if (Input.touchCount == 1) {
      var touch = Input.GetTouch(0);

      // Triple Tap shows/hides the console in iOS/Android dev builds.
      if (evt.type == EventType.Repaint && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && touch.tapCount == 3/* && !_isLastHitUi*/) {
          Vector2 pos = touch.position;
          if (pos.x >= Screen.width * 1 / 3 && pos.x <= Screen.width * 2 / 3 && pos.y >= Screen.height * 2 / 3 && touch.deltaPosition.sqrMagnitude <= 25) {
              m_IsOpen = !m_IsOpen;
          }
      }
      if (m_IsOpen) {
        var pos = touch.position;
        pos.y = Screen.height - pos.y;

        if (m_Dragging && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)) {
          m_Dragging = false;
        }
        else if (!m_Dragging && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)) {
          var dragRect = m_FakeDragRect;

          dragRect.x = m_WindowRect.x * guiScale.x;
          dragRect.y = m_WindowRect.y * guiScale.y;
          dragRect.width *= guiScale.x;
          dragRect.height *= guiScale.y;

          // check to see if the touch is inside the dragRect.
          if (dragRect.Contains(pos)) {
            m_Dragging = true;
          }
        }

        if (m_Dragging && evt.type == EventType.Repaint) {
#if UNITY_ANDROID && !UNITY_EDITOR
          var delta = touch.deltaPosition * 2.0f;
#elif UNITY_IOS
          var delta = touch.deltaPosition;
          delta.x /= guiScale.x;
          delta.y /= guiScale.y;
#endif
          delta.y = -delta.y;

          m_WindowRect.center += delta;
        }
        else {
          var tapRect = m_ScrollRect;
          tapRect.x += m_WindowRect.x * guiScale.x;
          tapRect.y += m_WindowRect.y * guiScale.y;
          tapRect.width -= 32;
          tapRect.width *= guiScale.x;
          tapRect.height *= guiScale.y;

          if (tapRect.Contains(pos)) {
            var scrollY = (tapRect.center.y - pos.y) / guiScale.y;

            switch (m_ToolbarIndex) {
            case 0:
              m_LogScrollPos.y -= scrollY;
              break;
            case 1:
              m_ViewScrollPos.y -= scrollY;
              break;
            }
          }
        }
      }
    }
    else if (m_Dragging && Input.touchCount == 0) {
      m_Dragging = false;
    }
#endif
        if (m_WindowStyle == null) {
            m_WindowStyle = new GUIStyle(GUI.skin.window);
            m_WindowOnStyle = new GUIStyle(GUI.skin.window);
            m_WindowOnStyle.normal.background = GUI.skin.window.onNormal.background;
            m_WindowStyle.fontSize = 20;
            m_WindowOnStyle.fontSize = 20;
        }
        if (m_LabelStyle == null) {
            m_LabelStyle = new GUIStyle(GUI.skin.label);
            m_LabelStyle.fontSize = 20;
        }
        if (m_TextareaStyle == null) {
            m_TextareaStyle = new GUIStyle(GUI.skin.textArea);
            m_TextareaStyle.fontSize = 20;
        }
        if (m_TextfieldStyle == null) {
            m_TextfieldStyle = new GUIStyle(GUI.skin.textField);
            m_TextfieldStyle.fontSize = 20;
        }
        if (!m_IsOpen) {
            return;
        }

        m_InnerRect.width = m_MessageLine.width;
#if !MOBILE
        m_WindowRect = GUI.Window(-1111, m_WindowRect, m_WindowMethods[m_ToolbarIndex], "");
        GUI.BringWindowToFront(-1111);
#else
    GUI.BeginGroup(m_WindowRect);
#if UNITY_EDITOR
    if (GUI.RepeatButton(m_FakeDragRect, string.Empty, GUIStyle.none)) {
      Vector2 delta = (Vector2) Input.mousePosition - m_PrevMousePos;
      delta.y = -delta.y;

      m_WindowRect.center += delta;
      m_Dragging = true;
    }

    if (evt.type == EventType.Repaint) {
      m_PrevMousePos = Input.mousePosition;
    }
#endif
    GUI.Box(m_FakeWindowRect, "", m_Dragging ? m_WindowOnStyle : m_WindowStyle);
    m_WindowMethods[m_ToolbarIndex](0);
    GUI.EndGroup();
#endif

        if (GUI.GetNameOfFocusedControl() == ENTRYFIELD) {
            if (evt.isKey && evt.type == EventType.KeyUp) {
                if (evt.keyCode == KeyCode.Return) {
                    EvalInputString(m_InputText);
                    m_InputText = string.Empty;
                    m_InputFilter = string.Empty;
                }
                else if (evt.keyCode == KeyCode.UpArrow) {
                    m_InputText = m_History.Fetch(m_InputText, true, m_InputFilter);
                }
                else if (evt.keyCode == KeyCode.DownArrow) {
                    m_InputText = m_History.Fetch(m_InputText, false, m_InputFilter);
                }
                else if (evt.keyCode == ToggleKey) {

                }
                else {
                    m_InputFilter = m_InputText;
                }
            }
        }

        if (scaled) {
            GUI.matrix = m_RestoreMatrix;
        }

        if (m_Dirty && evt.type == EventType.Repaint) {
            m_LogScrollPos.y = 50000.0f;
            m_ViewScrollPos.y = 50000.0f;

            BuildDisplayString();
            m_Dirty = false;
        }
    }

    private void DrawBottomControls()
    {
        GUI.SetNextControlName(ENTRYFIELD);
        m_InputText = GUI.TextField(m_InputRect, m_InputText, m_TextfieldStyle);

        if (GUI.Button(m_EnterRect, "Enter")) {
            EvalInputString(m_InputText);
            m_InputText = string.Empty;
        }

        var index = GUI.Toolbar(m_ToolbarRect, m_ToolbarIndex, m_Tabs);

        if (index != m_ToolbarIndex) {
            m_ToolbarIndex = index;

            if (index > 0) {
                m_Dirty = true;
            }
        }
#if !MOBILE
        GUI.DragWindow();
#endif
    }
    private void LogWindow(int windowID)
    {
        GUI.Box(m_ScrollRect, string.Empty);

        m_InnerRect.height = m_InnerHeight < m_ScrollRect.height ? m_ScrollRect.height : m_InnerHeight;

        m_LogScrollPos = GUI.BeginScrollView(m_ScrollRect, m_LogScrollPos, m_InnerRect, false, true);

        if (m_Messages.Count > 0) {
            Color oldColor = GUI.contentColor;

            m_MessageLine.y = 0;

            foreach (Message m in m_Messages) {
                string txt = m.ToString();
                if (!txt.Contains(m_Filter))
                    continue;

                GUI.contentColor = m.Color;

                m_GuiContent.text = m.ToGUIString();

                m_MessageLine.height = m_LabelStyle.CalcHeight(m_GuiContent, m_MessageLine.width);

                GUI.Label(m_MessageLine, m_GuiContent, m_LabelStyle);

                m_MessageLine.y += (m_MessageLine.height + m_LineOffset);

                m_InnerHeight = m_MessageLine.y > m_ScrollRect.height ? (int)m_MessageLine.y : (int)m_ScrollRect.height;
            }
            GUI.contentColor = oldColor;
        }

        GUI.EndScrollView();

        DrawBottomControls();

        GUI.FocusControl(ENTRYFIELD);
    }
    private string GetDisplayString()
    {
        return m_DisplayString.ToString();
    }
    private void BuildDisplayString()
    {
        m_DisplayString.Length = 0;

        foreach (Message m in m_Messages) {
            string txt = m.ToString();
            if (!txt.Contains(m_Filter))
                continue;
            m_DisplayString.AppendLine(m.ToString());
        }
    }
    private void CopyLogWindow(int windowID)
    {
        m_GuiContent.text = GetDisplayString();

        var calcHeight = GUI.skin.textArea.CalcHeight(m_GuiContent, m_MessageLine.width);

        m_InnerRect.height = calcHeight < m_ScrollRect.height ? m_ScrollRect.height : calcHeight;

        m_ViewScrollPos = GUI.BeginScrollView(m_ScrollRect, m_ViewScrollPos, m_InnerRect, false, true);

        GUI.TextArea(m_InnerRect, m_GuiContent.text, m_TextareaStyle);

        GUI.EndScrollView();

        DrawBottomControls();
    }
#endif //EMBED_ONGUI

    #region Console commands

    //==== Built-in example DebugCommand handlers ====
    private object CMDOpen(params string[] args)
    {
        ShowImpl();

        return "opened";
    }

    private object CMDClose(params string[] args)
    {
        HideImpl();

        return "closed";
    }

    private object CMDClear(params string[] args)
    {
        this.ClearLog();

        return "clear";
    }

    private object CMDHelp(params string[] args)
    {
        string filter = string.Empty;
        if (args.Length > 1) {
            filter = args[1];
        }
        var output = new StringBuilder();

        if (string.IsNullOrEmpty(filter)) {
            output.AppendLine(":: Command List ::");

            foreach (string key in m_CmdTable.Keys) {
                output.AppendLine(key);
            }

            output.AppendLine(" ");
        }
        else {
            output.AppendLine(":: Story Command List ::");

            foreach (var pair in PluginFramework.Instance.CommandDocs) {
                if (pair.Key.Contains(filter) || pair.Value.Contains(filter)) {
                    output.Append("[");
                    output.Append(pair.Key);
                    output.Append("]:");
                    output.AppendLine(pair.Value);
                }
            }

            output.AppendLine(" ");
            output.AppendLine(":: Story Function List ::");

            foreach (var pair in PluginFramework.Instance.FunctionDocs) {
                if (pair.Key.Contains(filter) || pair.Value.Contains(filter)) {
                    output.Append("[");
                    output.Append(pair.Key);
                    output.Append("]:");
                    output.AppendLine(pair.Value);
                }
            }

            output.AppendLine(" ");
        }

        return output.ToString();
    }

    private object CMDSystemInfo(params string[] args)
    {
        var info = new StringBuilder();

        info.AppendFormat("Unity Ver: {0}\n", Application.unityVersion);
        info.AppendFormat("Platform: {0} Language: {1}\n", Application.platform, Application.systemLanguage);
        info.AppendFormat("Screen:({0},{1}) DPI:{2} Target:{3}fps\n", Screen.width, Screen.height, Screen.dpi, Application.targetFrameRate);
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.IsValid()) {
            info.AppendFormat("Level: {0} ({1} of {2})\n", scene.name, scene.buildIndex, UnityEngine.SceneManagement.SceneManager.sceneCount);
        }
        info.AppendFormat("Quality: {0}\n", QualitySettings.names[QualitySettings.GetQualityLevel()]);
        info.AppendLine();
        info.AppendFormat("Data Path: {0}\n", Application.dataPath);
        info.AppendFormat("Cache Path: {0}\n", Application.temporaryCachePath);
        info.AppendFormat("Persistent Path: {0}\n", Application.persistentDataPath);
        info.AppendFormat("Streaming Path: {0}\n", Application.streamingAssetsPath);
#if MOBILE
	    info.AppendLine();
	    info.AppendFormat("Net Reachability: {0}\n", Application.internetReachability);
	    info.AppendFormat("Multitouch: {0}\n", Input.multiTouchEnabled);
#endif
#if UNITY_EDITOR
        info.AppendLine();
        info.AppendFormat("editorApp: {0}\n", UnityEditor.EditorApplication.applicationPath);
        info.AppendFormat("editorAppContents: {0}\n", UnityEditor.EditorApplication.applicationContentsPath);
        var editorScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        if (null != editorScene) {
            info.AppendFormat("scene: {0} ({1})\n", editorScene.name, editorScene.buildIndex);
        }
#endif
        info.AppendLine();

        return info.ToString();
    }

    private object CMDQuality(params string[] args)
    {
        int quality = 0;
        if (args.Length == 2) {
            quality = int.Parse(args[1]);
            QualitySettings.SetQualityLevel(quality, true);
        }
        quality = QualitySettings.GetQualityLevel();
        string name = QualitySettings.names[quality];
        return string.Format("{0}({1})", name, quality);
    }

    private object CMDVSync(params string[] args)
    {
        int count = 1;
        if (args.Length == 2) {
            count = int.Parse(args[1]);
            if (count == 2)
                Application.targetFrameRate = 30;
            else
                Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = count;
        }
        int frameRate = Application.targetFrameRate;
        count = QualitySettings.vSyncCount;
        return string.Format("frame rate:{0} vsync count:{1}", frameRate, count);
    }

    private object CMDResetDsl(params string[] args)
    {
        GameObject obj = GameObject.Find("GameRoot");
        if (null != obj) {
            obj.SendMessage("OnResetDsl", null);
        }
        return "resetdsl finish.";
    }

    private object CMDScript(params string[] args)
    {
        if (args.Length == 2) {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecScript", args[1]);
            }
            return "script " + args[1];
        }
        else {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecScript", "");
            }
            return "script";
        }
    }

    private object CMDCommand(params string[] args)
    {
        if (args.Length == 2) {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecCommand", args[1]);
            }
            return "command " + args[1];
        }
        else {
            return "cmd need argument command.";
        }
    }

    private object CMDGm(params string[] args)
    {
        if (args.Length == 2) {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecCommand", "gm(\"" + args[1] + "\");");
            }
            return "gm " + args[1];
        }
        else {
            return "gm need argument command.";
        }
    }

    private object CMDFilter(params string[] args)
    {
        if (args.Length == 2) {
            m_Filter = args[1];
#if !EMBED_ONGUI
            BuildDisplayString();
#endif
            return "filter set to " + args[1];
        }
        else {
            m_Filter = string.Empty;
#if !EMBED_ONGUI
            BuildDisplayString();
#endif
            return "filter set to empty.";
        }
    }

    #endregion

    private void LogMessage(Message msg)
    {
        while (m_Messages.Count > MaxLinesForDisplay) {
            if (m_Messages.Count > MaxLinesForDisplay + 1000)
                m_Messages.Clear();
            else
                m_Messages.RemoveAt(0);
        }
        m_Messages.Add(msg);
        m_LogScrollPos.y = 50000.0f;
    }
    //--- Local version. Use the static version above instead.
    private void ClearLog()
    {
        m_Messages.Clear();
        m_LogScrollPos.y = 50000.0f;
    }
    //--- Local version. Use the static version above instead.
    private void RegisterCommandCallback(string commandString, DebugCommand commandCallback)
    {
#if !UNITY_FLASH
        m_CmdTable[commandString.ToLower()] = new DebugCommand(commandCallback);
#endif
    }
    //--- Local version. Use the static version above instead.
    private void UnRegisterCommandCallback(string commandString)
    {
        m_CmdTable.Remove(commandString.ToLower());
    }

    private void EvalInputString(string inputString)
    {
        inputString = inputString.Trim();
        if (string.IsNullOrEmpty(inputString)) {
            LogMessage(Message.Input(string.Empty));
            return;
        }
        m_History.Add(inputString);

        string stringToSave = m_History.ToString();
        PlayerPrefs.SetString("debug_console_history", stringToSave);

        LogMessage(Message.Input(inputString));

        inputString = inputString.Trim();
        List<string> input = new List<string>();
        string cmd;
        int startIx = inputString.IndexOf(' ');
        if (startIx > 0) {
            cmd = inputString.Substring(0, startIx).Trim().ToLower();
            string leftCmd = inputString.Substring(startIx + 1).Trim();
            input.Add(cmd);
            if (0 == cmd.CompareTo("command") || 0 == cmd.CompareTo("cmd") || 0 == cmd.CompareTo("script") || 0 == cmd.CompareTo("scp") || 0 == cmd.CompareTo("gm")) {
                input.Add(leftCmd);
            }
            else {
                input.AddRange(leftCmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
            }
        }
        else {
            cmd = inputString.ToLower();
            input.Add(inputString);
        }
        if (m_CmdTable.ContainsKey(cmd)) {
            Log(m_CmdTable[cmd](input.ToArray()), MessageType.OUTPUT);
        }
        else {
            input.Clear();
            input.Add("cmd");
            input.Add(inputString);
            Log(m_CmdTable["cmd"](input.ToArray()), MessageType.OUTPUT);
            //LogMessage(Message.Output(string.Format("*** Unknown Command: {0} ***", cmd)));
        }
    }

    private Dictionary<string, DebugCommand> m_CmdTable = new Dictionary<string, DebugCommand>();
    private List<Message> m_Messages = new List<Message>();
    private History m_History = new History();
    private string m_Filter = string.Empty;
    private string m_InputFilter = string.Empty;
    private string m_InputText = string.Empty;
    private bool m_IsOpen;

#if EMBED_ONGUI
    private Vector2 m_ViewScrollPos = Vector2.zero;
#else
    private Vector2 m_ViewScrollPos = Vector2.zero;
    private Vector2 m_LogScrollPos = Vector2.zero;
    private Rect m_WindowRect;
    private GUIStyle m_WindowOnStyle;
    private GUIStyle m_WindowStyle;
    private GUIStyle m_LabelStyle;
    private GUIStyle m_TextareaStyle;
    private GUIStyle m_TextfieldStyle;
#if MOBILE
    private Rect m_FakeWindowRect;
    private Rect m_FakeDragRect;
    private bool m_Dragging = false;
#if UNITY_EDITOR
    private Vector2 m_PrevMousePos;
#endif
#endif

    private float m_InitialScreenWidth = 1.0f;
    private Matrix4x4 m_RestoreMatrix = Matrix4x4.identity;
    private StringBuilder m_DisplayString = new StringBuilder();
    private bool m_Dirty;

    private Rect m_ScrollRect = new Rect(0, 0, 0, 0);
    private Rect m_InputRect = new Rect(0, 0, 0, 0);
    private Rect m_EnterRect = new Rect(0, 0, 0, 0);
    private Rect m_ToolbarRect = new Rect(0, 0, 0, 0);
    private Rect m_MessageLine = new Rect(0, 0, 0, 0);

    private int m_LineOffset = -4;
    private string[] m_Tabs = new string[] { "Log", "View" };

    // Keep these private, their values are generated automatically
    private Rect m_InnerRect = new Rect(0, 0, 0, 0);
    private int m_InnerHeight = 0;
    private int m_ToolbarIndex = 0;
    private GUIContent m_GuiContent = new GUIContent();
    private GUI.WindowFunction[] m_WindowMethods;
#endif //EMBED_ONGUI

    #region StaticAccessors

    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object Log(object message)
    {
        s_Instance?.LogMessage(Message.Log(message));

        return message;
    }
    public static object LogFormat(string format, params object[] args)
    {
        return Log(string.Format(format, args));
    }
    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="messageType">The MessageType of the message. Used to provide
    /// formatting in order to distinguish between message types.</param>
    public static object Log(object message, MessageType messageType)
    {
        s_Instance?.LogMessage(new Message(message, messageType));

        return message;
    }
    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="displayColor">The text color to use when displaying the message.</param>
    public static object Log(object message, Color displayColor)
    {
        s_Instance?.LogMessage(new Message(message, displayColor));

        return message;
    }
    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Messate to print.</param>
    /// <param name="messageType">The MessageType of the message. Used to provide
    /// formatting in order to distinguish between message types.</param>
    /// <param name="displayColor">The color to use when displaying the message.</param>
    /// <param name="useCustomColor">Flag indicating if the displayColor value should be used or
    /// if the default color for the message type should be used instead.</param>
    public static object Log(object message, MessageType messageType, Color displayColor)
    {
        s_Instance?.LogMessage(new Message(message, messageType, displayColor));

        return message;
    }
    /// <summary>
    /// Prints a message string to the console using the "Warning" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogWarning(object message)
    {
        s_Instance?.LogMessage(Message.Warning(message));

        return message;
    }
    /// <summary>
    /// Prints a message string to the console using the "Error" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogError(object message)
    {
        s_Instance?.LogMessage(Message.Error(message));

        return message;
    }

    /// <summary>
    /// Clears all console output.
    /// </summary>
    public static void Clear()
    {
        s_Instance?.ClearLog();
    }
    /// <summary>
    /// Execute a console command directly from code.
    /// </summary>
    /// <param name="commandString">The command line you want to execute. For example: "sys"</param>
    public static void Execute(string commandString)
    {
        s_Instance?.EvalInputString(commandString);
    }
    /// <summary>
    /// Registers a debug command that is "fired" when the specified command string is entered.
    /// </summary>
    /// <param name="commandString">The string that represents the command. For example: "FOV"</param>
    /// <param name="commandCallback">The method/function to call with the commandString is entered.
    /// For example: "SetFOV"</param>
    public static void RegisterCommand(string commandString, DebugCommand commandCallback)
    {
        s_Instance?.RegisterCommandCallback(commandString, commandCallback);
    }
    /// <summary>
    /// Removes a previously-registered debug command.
    /// </summary>
    /// <param name="commandString">The string that represents the command.</param>
    public static void UnRegisterCommand(string commandString)
    {
        s_Instance?.UnRegisterCommandCallback(commandString);
    }

    public static bool IsOpen
    {
        get {
            bool result = (bool)(s_Instance?.m_IsOpen);
            return result;
        }
    }
    public static void Show()
    {
        s_Instance?.ShowImpl();
    }
    public static void Hide()
    {
        s_Instance?.HideImpl();
    }
    public static void DoEmbedGUI()
    {
#if EMBED_ONGUI
        s_Instance?.OnEmbedGUI();
#endif
    }

    /// <summary>
    /// Key to press to toggle the visibility of the console.
    /// </summary>
    public static KeyCode ToggleKey = KeyCode.BackQuote;

    private static DebugConsole s_Instance;

    private const string VERSION = "3.0";
    private const string ENTRYFIELD = "DebugConsoleEntryField";

    #endregion
}
