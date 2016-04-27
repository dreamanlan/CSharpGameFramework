#define DEBUG_CONSOLE

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
#define MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

#if DEBUG_CONSOLE
public class DebugConsole : MonoBehaviour
{
    readonly string VERSION = "3.0";
    readonly string ENTRYFIELD = "DebugConsoleEntryField";

    public delegate object DebugCommand(params string[] args);

    /// <summary>
    /// How many lines of text this console will display.
    /// </summary>
    public int maxLinesForDisplay = 500;

    /// <summary>
    /// Default color of the standard display text.
    /// </summary>
    public Color defaultColor = Message.defaultColor;
    public Color warningColor = Message.warningColor;
    public Color errorColor = Message.errorColor;
    public Color systemColor = Message.systemColor;
    public Color inputColor = Message.inputColor;
    public Color outputColor = Message.outputColor;

    /// <summary>
    /// Used to check (or toggle) the open state of the console.
    /// </summary>
    public static bool IsOpen
    {
        get { return DebugConsole.Instance._isOpen; }
        set { DebugConsole.Instance._isOpen = value; }
    }

    public static bool IsLastHitUi
    {
        get { return DebugConsole.Instance._isLastHitUi; }
        set { DebugConsole.Instance._isLastHitUi = value; }
    }

    /// <summary>
    /// Static instance of the console.
    ///
    /// When you want to access the console without a direct
    /// reference (which you do in mose cases), use DebugConsole.Instance and the required
    /// GameObject initialization will be done for you.
    /// </summary>
    static DebugConsole Instance
    {
        get
        {
            if (_instance == null) {
                _instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;

                if (_instance != null) {
                    return _instance;
                }

                GameObject console = new GameObject("__Debug Console__");
                _instance = console.AddComponent<DebugConsole>();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Key to press to toggle the visibility of the console.
    /// </summary>
    public static KeyCode toggleKey = KeyCode.BackQuote;
    static DebugConsole _instance;
    Dictionary<string, DebugCommand> _cmdTable = new Dictionary<string, DebugCommand>();
    string _inputString = string.Empty;
    Rect _windowRect;
    GUIStyle windowOnStyle;
    GUIStyle windowStyle;
    GUIStyle labelStyle;
    GUIStyle textareaStyle;
    GUIStyle textfieldStyle;
#if MOBILE
    Rect _fakeWindowRect;
    Rect _fakeDragRect;
    bool dragging = false;
#if UNITY_EDITOR
  Vector2 prevMousePos;
#endif
#endif

    Vector2 _logScrollPos = Vector2.zero;
    Vector2 _viewScrollPos = Vector2.zero;
    Vector3 _guiScale = Vector3.one;
    Matrix4x4 restoreMatrix = Matrix4x4.identity;
    bool _scaled = false;
    bool _isOpen;
    bool _isLastHitUi;
    StringBuilder _displayString = new StringBuilder();
    bool dirty;
    #region GUI position values
    // Make these values public if you want to adjust layout of console window
#if MOBILE
    readonly Rect scrollRect = new Rect(10, 20, 280, 360);
    readonly Rect inputRect = new Rect(10, 384, 228, 24);
    readonly Rect enterRect = new Rect(240, 384, 50, 24);
    readonly Rect toolbarRect = new Rect(16, 412, 266, 25);
    Rect messageLine = new Rect(4, 0, 264, 20);
#else
    readonly Rect scrollRect = new Rect(10, 20, 280, 360);
    readonly Rect inputRect = new Rect(10, 384, 228, 24);
    readonly Rect enterRect = new Rect(240, 384, 50, 24);
    readonly Rect toolbarRect = new Rect(16, 412, 266, 25);
    Rect messageLine = new Rect(4, 0, 264, 20);
#endif
    int lineOffset = -4;
    string[] tabs = new string[] { "Log", "View" };

    // Keep these private, their values are generated automatically
    Rect nameRect;
    Rect valueRect;
    Rect innerRect = new Rect(0, 0, 0, 0);
    int innerHeight = 0;
    int toolbarIndex = 0;
    GUIContent guiContent = new GUIContent();
    GUI.WindowFunction[] windowMethods;
    #endregion

    /// <summary>
    /// This Enum holds the message types used to easily control the formatting and display of a message.
    /// </summary>
    public enum MessageType
    {
        NORMAL,
        WARNING,
        ERROR,
        SYSTEM,
        INPUT,
        OUTPUT
    }

    /// <summary>
    /// Represents a single message, with formatting options.
    /// </summary>
    struct Message
    {
        string text;
        string formatted;
        MessageType type;
        Color color_;

        public Color color
        {
            get
            {
                return color_;
            }
        }

        public static Color defaultColor = Color.white;
        public static Color warningColor = Color.yellow;
        public static Color errorColor = Color.red;
        public static Color systemColor = Color.green;
        public static Color inputColor = Color.green;
        public static Color outputColor = Color.cyan;

        public Message(object messageObject)
            : this(messageObject, MessageType.NORMAL, Message.defaultColor)
        {
        }

        public Message(object messageObject, Color displayColor)
            : this(messageObject, MessageType.NORMAL, displayColor)
        {
        }

        public Message(object messageObject, MessageType messageType)
            : this(messageObject, messageType, Message.defaultColor)
        {
            switch (messageType) {
                case MessageType.ERROR:
                    color_ = errorColor;
                    break;
                case MessageType.SYSTEM:
                    color_ = systemColor;
                    break;
                case MessageType.WARNING:
                    color_ = warningColor;
                    break;
                case MessageType.OUTPUT:
                    color_ = outputColor;
                    break;
                case MessageType.INPUT:
                    color_ = inputColor;
                    break;
            }
        }

        public Message(object messageObject, MessageType messageType, Color displayColor)
        {
            text = (messageObject == null ? "<null>" : messageObject.ToString());
            formatted = string.Empty;
            type = messageType;
            color_ = displayColor;
        }

        public static Message Log(object message)
        {
            return new Message(message, MessageType.NORMAL, defaultColor);
        }

        public static Message System(object message)
        {
            return new Message(message, MessageType.SYSTEM, systemColor);
        }

        public static Message Warning(object message)
        {
            return new Message(message, MessageType.WARNING, warningColor);
        }

        public static Message Error(object message)
        {
            return new Message(message, MessageType.ERROR, errorColor);
        }

        public static Message Output(object message)
        {
            return new Message(message, MessageType.OUTPUT, outputColor);
        }

        public static Message Input(object message)
        {
            return new Message(message, MessageType.INPUT, inputColor);
        }

        public override string ToString()
        {
            switch (type) {
                case MessageType.ERROR:
                    return string.Format("[{0}] {1}", type, text);
                case MessageType.WARNING:
                    return string.Format("[{0}] {1}", type, text);
                default:
                    return ToGUIString();
            }
        }

        public string ToGUIString()
        {
            if (!string.IsNullOrEmpty(formatted)) {
                return formatted;
            }

            switch (type) {
                case MessageType.INPUT:
                    formatted = string.Format(">>> {0}", text);
                    break;
                case MessageType.OUTPUT:
                    var lines = text.Trim('\n').Split('\n');
                    var output = new StringBuilder();

                    foreach (var line in lines) {
                        output.AppendFormat("= {0}\n", line);
                    }

                    formatted = output.ToString();
                    break;
                case MessageType.SYSTEM:
                    formatted = string.Format("# {0}", text);
                    break;
                case MessageType.WARNING:
                    formatted = string.Format("* {0}", text);
                    break;
                case MessageType.ERROR:
                    formatted = string.Format("** {0}", text);
                    break;
                default:
                    formatted = text;
                    break;
            }

            return formatted;
        }
    }

    class History
    {
        List<string> history = new List<string>();
        int index = 0;

        public void Add(string item)
        {
            history.Add(item);
            index = 0;
        }

        string current;

        public string Fetch(string current, bool next)
        {
            if (index == 0) {
                this.current = current;
            }

            if (history.Count == 0) {
                return current;
            }

            index += next ? -1 : 1;

            if (history.Count + index < 0 || history.Count + index > history.Count - 1) {
                index = 0;
                return this.current;
            }

            var result = history[history.Count + index];

            return result;
        }
    }

    List<Message> _messages = new List<Message>();
    History _history = new History();

    void Awake()
    {
        if (_instance != null && _instance != this) {
            DestroyImmediate(this, true);
            return;
        }

        _instance = this;
    }

    void OnEnable()
    {
        var scale = Screen.dpi / 160.0f;

        if (scale != 0.0f && scale >= 1.1f) {
            _scaled = true;
            _guiScale.Set(scale, scale, scale);
        }

        windowMethods = new GUI.WindowFunction[] { LogWindow, CopyLogWindow };

        nameRect = messageLine;
        valueRect = messageLine;

        Message.defaultColor = defaultColor;
        Message.warningColor = warningColor;
        Message.errorColor = errorColor;
        Message.systemColor = systemColor;
        Message.inputColor = inputColor;
        Message.outputColor = outputColor;
#if MOBILE
        this.useGUILayout = false;
        _windowRect = new Rect(5.0f, 5.0f, 300.0f, 450.0f);
        //_windowRect = new Rect(5.0f, 5.0f, 300.0f, 280.0f);
        _fakeWindowRect = new Rect(0.0f, 0.0f, _windowRect.width, _windowRect.height);
        _fakeDragRect = new Rect(0.0f, 0.0f, _windowRect.width - 32, 24);
#else
        _windowRect = new Rect(30.0f, 30.0f, 300.0f, 450.0f);
        //_windowRect = new Rect(30.0f, 30.0f, 300.0f, 280.0f);
#endif

        LogMessage(Message.System(string.Format(" DebugConsole version {0}", VERSION)));
        LogMessage(Message.System(" Copyright 2008-2010 Jeremy Hollingsworth "));
        LogMessage(Message.System(" Ennanzus-Interactive.com "));
        LogMessage(Message.System(" type '/?' for available commands."));
        LogMessage(Message.Log(""));

        this.RegisterCommandCallback("close", CMDClose);
        this.RegisterCommandCallback("clear", CMDClear);
        this.RegisterCommandCallback("sys", CMDSystemInfo);
        this.RegisterCommandCallback("level", CMDLevel);
        this.RegisterCommandCallback("vsync", CMDVSync);
        this.RegisterCommandCallback("resetdsl", CMDResetDsl);
        this.RegisterCommandCallback("script", CMDScript);
        this.RegisterCommandCallback("scp", CMDScript);
        this.RegisterCommandCallback("command", CMDCommand);
        this.RegisterCommandCallback("cmd", CMDCommand);
        this.RegisterCommandCallback("/?", CMDHelp);
    }

    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    void OnGUI()
    {
        var evt = Event.current;

        if (_scaled) {
            restoreMatrix = GUI.matrix;

            GUI.matrix = GUI.matrix * Matrix4x4.Scale(_guiScale);
        }

        while (_messages.Count > maxLinesForDisplay) {
            _messages.RemoveAt(0);
        }
#if (!MOBILE) || UNITY_EDITOR
        // Toggle key shows the console in non-iOS dev builds
        if (evt.keyCode == toggleKey && evt.type == EventType.KeyUp)
            _isOpen = !_isOpen;
#endif
#if MOBILE
    if (Input.touchCount == 1) {
      var touch = Input.GetTouch(0);

      // Triple Tap shows/hides the console in iOS/Android dev builds.
      if (evt.type == EventType.Repaint && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && touch.tapCount == 3/* && !_isLastHitUi*/) {
          Vector2 pos = touch.position;
          if (pos.x >= Screen.width * 1 / 3 && pos.x <= Screen.width * 2 / 3 && pos.y >= Screen.height * 2 / 3 && touch.deltaPosition.sqrMagnitude <= 25) {
          _isOpen = !_isOpen;
          }
      }

      if (_isOpen) {
        var pos = touch.position;
        pos.y = Screen.height - pos.y;

        if (dragging && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)) {
          dragging = false;
        }
        else if (!dragging && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)) {
          var dragRect = _fakeDragRect;

          dragRect.x = _windowRect.x * _guiScale.x;
          dragRect.y = _windowRect.y * _guiScale.y;
          dragRect.width *= _guiScale.x;
          dragRect.height *= _guiScale.y;

          // check to see if the touch is inside the dragRect.
          if (dragRect.Contains(pos)) {
            dragging = true;
          }
        }

        if (dragging && evt.type == EventType.Repaint) {
#if UNITY_ANDROID && !UNITY_EDITOR
          var delta = touch.deltaPosition * 2.0f;
#elif UNITY_IOS
          var delta = touch.deltaPosition;
          delta.x /= _guiScale.x;
          delta.y /= _guiScale.y;
#endif
          delta.y = -delta.y;

          _windowRect.center += delta;
        }
        else {
          var tapRect = scrollRect;
          tapRect.x += _windowRect.x * _guiScale.x;
          tapRect.y += _windowRect.y * _guiScale.y;
          tapRect.width -= 32;
          tapRect.width *= _guiScale.x;
          tapRect.height *= _guiScale.y;

          if (tapRect.Contains(pos)) {
            var scrollY = (tapRect.center.y - pos.y) / _guiScale.y;

            switch (toolbarIndex) {
            case 0:
              _logScrollPos.y -= scrollY;
              break;
            case 1:
              _viewScrollPos.y -= scrollY;
              break;
            }
          }
        }
      }
    }
    else if (dragging && Input.touchCount == 0) {
      dragging = false;
    }
#endif
        if (windowStyle == null) {
            windowStyle = new GUIStyle(GUI.skin.window);
            windowOnStyle = new GUIStyle(GUI.skin.window);
            windowOnStyle.normal.background = GUI.skin.window.onNormal.background;
            windowStyle.fontSize = 14;
            windowOnStyle.fontSize = 14;
        }
        if (labelStyle == null) {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 14;
        }
        if (textareaStyle == null) {
            textareaStyle = new GUIStyle(GUI.skin.textArea);
            textareaStyle.fontSize = 14;
        }
        if (textfieldStyle == null) {
            textfieldStyle = new GUIStyle(GUI.skin.textField);
            textfieldStyle.fontSize = 14;
        }
        if (!_isOpen) {
            return;
        }

        innerRect.width = messageLine.width;
#if !MOBILE
        _windowRect = GUI.Window(-1111, _windowRect, windowMethods[toolbarIndex], string.Format("fps:{0:00.0} avgfps:{1:00.0}", GameFramework.TimeUtility.GfxFps, GameFramework.TimeUtility.GfxAvgFps));
        GUI.BringWindowToFront(-1111);
#else
    GUI.BeginGroup(_windowRect);
#if UNITY_EDITOR
    if (GUI.RepeatButton(_fakeDragRect, string.Empty, GUIStyle.none)) {
      Vector2 delta = (Vector2) Input.mousePosition - prevMousePos;
      delta.y = -delta.y;

      _windowRect.center += delta;
      dragging = true;
    }

    if (evt.type == EventType.Repaint) {
      prevMousePos = Input.mousePosition;
    }
#endif
    GUI.Box(_fakeWindowRect, string.Format("fps:{0:00.0} avgfps:{1:00.0}", GameFramework.TimeUtility.GfxFps, GameFramework.TimeUtility.GfxAvgFps), dragging ? windowOnStyle : windowStyle);
    windowMethods[toolbarIndex](0);
    GUI.EndGroup();
#endif

        if (GUI.GetNameOfFocusedControl() == ENTRYFIELD) {
            if (evt.isKey && evt.type == EventType.KeyUp) {
                if (evt.keyCode == KeyCode.Return) {
                    EvalInputString(_inputString);
                    _inputString = string.Empty;
                } else if (evt.keyCode == KeyCode.UpArrow) {
                    _inputString = _history.Fetch(_inputString, true);
                } else if (evt.keyCode == KeyCode.DownArrow) {
                    _inputString = _history.Fetch(_inputString, false);
                }
            }
        }

        if (_scaled) {
            GUI.matrix = restoreMatrix;
        }

        if (dirty && evt.type == EventType.Repaint) {
            _logScrollPos.y = 50000.0f;
            _viewScrollPos.y = 50000.0f;

            BuildDisplayString();
            dirty = false;
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
    #region StaticAccessors

    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object Log(object message)
    {
        DebugConsole.Instance.LogMessage(Message.Log(message));

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
        DebugConsole.Instance.LogMessage(new Message(message, messageType));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="displayColor">The text color to use when displaying the message.</param>
    public static object Log(object message, Color displayColor)
    {
        DebugConsole.Instance.LogMessage(new Message(message, displayColor));

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
        DebugConsole.Instance.LogMessage(new Message(message, messageType, displayColor));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console using the "Warning" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogWarning(object message)
    {
        DebugConsole.Instance.LogMessage(Message.Warning(message));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console using the "Error" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogError(object message)
    {
        DebugConsole.Instance.LogMessage(Message.Error(message));

        return message;
    }

    /// <summary>
    /// Clears all console output.
    /// </summary>
    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    public static void Clear()
    {
        DebugConsole.Instance.ClearLog();
    }

    /// <summary>
    /// Execute a console command directly from code.
    /// </summary>
    /// <param name="commandString">The command line you want to execute. For example: "sys"</param>
    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    public static void Execute(string commandString)
    {
        DebugConsole.Instance.EvalInputString(commandString);
    }

    /// <summary>
    /// Registers a debug command that is "fired" when the specified command string is entered.
    /// </summary>
    /// <param name="commandString">The string that represents the command. For example: "FOV"</param>
    /// <param name="commandCallback">The method/function to call with the commandString is entered.
    /// For example: "SetFOV"</param>
    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    public static void RegisterCommand(string commandString, DebugCommand commandCallback)
    {
        DebugConsole.Instance.RegisterCommandCallback(commandString, commandCallback);
    }

    /// <summary>
    /// Removes a previously-registered debug command.
    /// </summary>
    /// <param name="commandString">The string that represents the command.</param>
    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    public static void UnRegisterCommand(string commandString)
    {
        DebugConsole.Instance.UnRegisterCommandCallback(commandString);
    }

    #endregion
    #region Console commands

    //==== Built-in example DebugCommand handlers ====
    object CMDClose(params string[] args)
    {
        _isOpen = false;

        return "closed";
    }

    object CMDClear(params string[] args)
    {
        this.ClearLog();

        return "clear";
    }

    object CMDHelp(params string[] args)
    {
        var output = new StringBuilder();

        output.AppendLine(":: Command List ::");

        foreach (string key in _cmdTable.Keys) {
            output.AppendLine(key);
        }

        output.AppendLine(" ");

        return output.ToString();
    }

    object CMDSystemInfo(params string[] args)
    {
        var info = new StringBuilder();

        info.AppendFormat("Unity Ver: {0}\n", Application.unityVersion);
        info.AppendFormat("Platform: {0} Language: {1}\n", Application.platform, Application.systemLanguage);
        info.AppendFormat("Screen:({0},{1}) DPI:{2} Target:{3}fps\n", Screen.width, Screen.height, Screen.dpi, Application.targetFrameRate);
        info.AppendFormat("Level: {0} ({1} of {2})\n", Application.loadedLevelName, Application.loadedLevel, Application.levelCount);
        info.AppendFormat("Quality: {0}\n", QualitySettings.names[QualitySettings.GetQualityLevel()]);
        info.AppendLine();
        info.AppendFormat("Data Path: {0}\n", Application.dataPath);
        info.AppendFormat("Cache Path: {0}\n", Application.temporaryCachePath);
        info.AppendFormat("Persistent Path: {0}\n", Application.persistentDataPath);
        info.AppendFormat("Streaming Path: {0}\n", Application.streamingAssetsPath);
#if UNITY_WEBPLAYER
    info.AppendLine();
    info.AppendFormat("URL: {0}\n", Application.absoluteURL);
    info.AppendFormat("srcValue: {0}\n", Application.srcValue);
    info.AppendFormat("security URL: {0}\n", Application.webSecurityHostUrl);
#endif
#if MOBILE
    info.AppendLine();
    info.AppendFormat("Net Reachability: {0}\n", Application.internetReachability);
    info.AppendFormat("Multitouch: {0}\n", Input.multiTouchEnabled);
#endif
#if UNITY_EDITOR
        info.AppendLine();
        info.AppendFormat("editorApp: {0}\n", UnityEditor.EditorApplication.applicationPath);
        info.AppendFormat("editorAppContents: {0}\n", UnityEditor.EditorApplication.applicationContentsPath);
        info.AppendFormat("scene: {0}\n", UnityEditor.EditorApplication.currentScene);
#endif
        info.AppendLine();
        var devices = WebCamTexture.devices;
        if (devices.Length > 0) {
            info.AppendLine("Cameras: ");

            foreach (var device in devices) {
                info.AppendFormat("  {0} front:{1}\n", device.name, device.isFrontFacing);
            }
        }

        return info.ToString();
    }

    object CMDLevel(params string[] args)
    {
        int level = 0;
        if (args.Length == 2) {
            level = int.Parse(args[1]);
            QualitySettings.SetQualityLevel(level, true);
        }
        level = QualitySettings.GetQualityLevel();
        string name = QualitySettings.names[level];
        return string.Format("{0}({1})", name, level);
    }

    object CMDVSync(params string[] args)
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

    object CMDResetDsl(params string[] args)
    {
        GameObject obj = GameObject.Find("GameRoot");
        if (null != obj) {
            obj.SendMessage("OnResetDsl", null);
        }
        return "resetdsl finish.";
    }

    object CMDScript(params string[] args)
    {
        if (args.Length == 2) {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecScript", args[1]);
            }
            return "script " + args[1];
        } else {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecScript", "");
            }
            return "script";
        }
    }

    object CMDCommand(params string[] args)
    {
        if (args.Length == 2) {
            GameObject obj = GameObject.Find("GameRoot");
            if (null != obj) {
                obj.SendMessage("OnExecCommand", args[1]);
            }
            return "command " + args[1];
        } else {
            return "gm command need argument command.";
        }
    }

    #endregion
    #region GUI Window Methods

    void DrawBottomControls()
    {
        GUI.SetNextControlName(ENTRYFIELD);
        _inputString = GUI.TextField(inputRect, _inputString, textfieldStyle);

        if (GUI.Button(enterRect, "Enter")) {
            EvalInputString(_inputString);
            _inputString = string.Empty;
        }

        var index = GUI.Toolbar(toolbarRect, toolbarIndex, tabs);

        if (index != toolbarIndex) {
            toolbarIndex = index;

            if (index > 0) {
                dirty = true;
            }
        }
#if !MOBILE
        GUI.DragWindow();
#endif
    }

    void LogWindow(int windowID)
    {
        GUI.Box(scrollRect, string.Empty);

        innerRect.height = innerHeight < scrollRect.height ? scrollRect.height : innerHeight;

        _logScrollPos = GUI.BeginScrollView(scrollRect, _logScrollPos, innerRect, false, true);

        if (_messages != null || _messages.Count > 0) {
            Color oldColor = GUI.contentColor;

            messageLine.y = 0;

            foreach (Message m in _messages) {
                GUI.contentColor = m.color;

                guiContent.text = m.ToGUIString();

                messageLine.height = labelStyle.CalcHeight(guiContent, messageLine.width);

                GUI.Label(messageLine, guiContent, labelStyle);

                messageLine.y += (messageLine.height + lineOffset);

                innerHeight = messageLine.y > scrollRect.height ? (int)messageLine.y : (int)scrollRect.height;
            }
            GUI.contentColor = oldColor;
        }

        GUI.EndScrollView();

        DrawBottomControls();
    }

    string GetDisplayString()
    {
        if (_messages == null) {
            return string.Empty;
        }

        return _displayString.ToString();
    }

    void BuildDisplayString()
    {
        _displayString.Length = 0;

        foreach (Message m in _messages) {
            _displayString.AppendLine(m.ToString());
        }
    }

    void CopyLogWindow(int windowID)
    {

        guiContent.text = GetDisplayString();

        var calcHeight = GUI.skin.textArea.CalcHeight(guiContent, messageLine.width);

        innerRect.height = calcHeight < scrollRect.height ? scrollRect.height : calcHeight;

        _viewScrollPos = GUI.BeginScrollView(scrollRect, _viewScrollPos, innerRect, false, true);

        GUI.TextArea(innerRect, guiContent.text, textareaStyle);

        GUI.EndScrollView();

        DrawBottomControls();
    }

    #endregion
    #region InternalFunctionality
    [Conditional("DEBUG_CONSOLE"),
     Conditional("UNITY_EDITOR"),
     Conditional("DEVELOPMENT_BUILD")]
    void LogMessage(Message msg)
    {
        _messages.Add(msg);
    }

    //--- Local version. Use the static version above instead.
    void ClearLog()
    {
        _messages.Clear();
    }

    //--- Local version. Use the static version above instead.
    void RegisterCommandCallback(string commandString, DebugCommand commandCallback)
    {
#if !UNITY_FLASH
        _cmdTable[commandString.ToLower()] = new DebugCommand(commandCallback);
#endif
    }

    //--- Local version. Use the static version above instead.
    void UnRegisterCommandCallback(string commandString)
    {
        _cmdTable.Remove(commandString.ToLower());
    }

    void EvalInputString(string inputString)
    {
        inputString = inputString.Trim();
        if (string.IsNullOrEmpty(inputString)) {
            LogMessage(Message.Input(string.Empty));
            return;
        }
        _history.Add(inputString);

        LogMessage(Message.Input(inputString));

        inputString = inputString.Trim();
        List<string> input = new List<string>();
        string cmd;
        int startIx = inputString.IndexOf(' ');
        if (startIx > 0) {
            cmd = inputString.Substring(0, startIx).Trim().ToLower();
            string leftCmd = inputString.Substring(startIx + 1).Trim();
            input.Add(cmd);
            if (0 == cmd.CompareTo("command") || 0 == cmd.CompareTo("cmd") || 0 == cmd.CompareTo("script") || 0 == cmd.CompareTo("scp")) {
                input.Add(leftCmd);
            } else {
                input.AddRange(leftCmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
            }
        } else {
            cmd = inputString.ToLower();
            input.Add(inputString);
        }
        if (_cmdTable.ContainsKey(cmd)) {
            Log(_cmdTable[cmd](input.ToArray()), MessageType.OUTPUT);
        } else {
            LogMessage(Message.Output(string.Format("*** Unknown Command: {0} ***", cmd)));
        }
    }
    #endregion
}
#else
public static class DebugConsole {
  public static bool IsOpen;  
  public static KeyCode toggleKey;
  public delegate object DebugCommand(params string[] args);

  public static object Log(object message) {
    return message;
  }

  public static object LogFormat(string format, params object[] args) {
    return string.Format(format, args);
  }

  public static object LogWarning(object message) {
    return message;
  }

  public static object LogError(object message) {
    return message;
  }

  public static object Log(object message, object messageType) {
    return message;
  }

  public static object Log(object message, Color displayColor) {
    return message;
  }

  public static object Log(object message, object messageType, Color displayColor) {
    return message;
  }

  public static void Clear() {
  }

  public static void RegisterCommand(string commandString, DebugCommand commandCallback) {
  }

  public static void UnRegisterCommand(string commandString) {
  }

  public static void RegisterWatchVar(object watchVar) {
  }

  public static void UnRegisterWatchVar(string name) {
  }
}
#endif
