using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StoryScript;

[UnityEngine.Scripting.Preserve]
public class TabularDebugUI
{
    public delegate void LoadUiDelegation(string uiRes);
    public delegate void ShowUiDelegation();
    public delegate void InitUiDelegation(string uiRes);

    public GameObject LabelTemplate;
    public GameObject InputTemplate;
    public GameObject ButtonTemplate;
    public GameObject DropDownTemplate;
    public GameObject ToggleTemplate;
    public GameObject ToggleGroupTemplate;
    public GameObject SliderTemplate;

    public System.Object ApiObject;
    public LoadUiDelegation OnLoadUi;
    public ShowUiDelegation OnShowUi;
    public InitUiDelegation OnInitUi;

    public Dictionary<string, UIBehaviour> UiControls = new Dictionary<string, UIBehaviour>();
    public string CurUiRes = string.Empty;
    public bool UiLoaded = false;
    public bool UiInited = false;

    public TabularDebugUI(List<KeyValuePair<string, string>> debugUis)
    {
        m_DebugUis = debugUis;
    }
    public void Init()
    {
        m_RootUi = GameObject.Find("RootPanel");
        if (IsNullObject(m_RootUi)) {
            return;
        }
        InitUiCells();
        HideRootUi();
    }
    public void ShowRootUi()
    {
        if (IsNullObject(m_RootUi)) {
            return;
        }
        m_RootUi.SetActive(true);
    }
    public void HideRootUi()
    {
        if (IsNullObject(m_RootUi)) {
            return;
        }
        m_RootUi.SetActive(false);
    }
    public void LoadUi(string name, string text)
    {
        if (IsNullObject(m_RootUi)) {
            return;
        }
        ClearCells();
        UiLoaded = false;
        UiInited = false;

        if (!string.IsNullOrEmpty(text)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.LoadFromString(text, LogSystem.Log)) {
                foreach (var info in file.DslInfos) {
                    string type = info.GetId();
                    if (type == "label") {
                        BuildLabel(info);
                    }
                    else if (type == "input") {
                        BuildInput(info);
                    }
                    else if (type == "button") {
                        BuildButton(info);
                    }
                    else if (type == "dropdown") {
                        BuildDropdown(info);
                    }
                    else if (type == "toggle") {
                        BuildToggle(info);
                    }
                    else if (type == "toggle_group") {
                        BuildToggleGroup(info);
                    }
                    else if (type == "slider") {
                        BuildSlider(info);
                    }
                }
            }

            CurUiRes = name;
            UiLoaded = true;
            CallInitUi(CurUiRes);
            UiInited = true;
        }
    }
    public void LoadAndShowIndexUi()
    {
        if (IsNullObject(m_RootUi)) {
            return;
        }
        const string c_IndexUiRes = "index";
        const int c_NumPerRow = c_CellColNum - 2;
        const int c_StartRow = 1;
        const int c_StartCol = 1;

        ClearCells();
        UiLoaded = false;
        UiInited = false;

        int totalCt = m_DebugUis.Count;
        int ix = 0;
        for (int row = c_StartRow; row < c_CellRowNum && ix < totalCt; ++row) {
            for (int col = c_StartCol; col < c_StartCol + c_NumPerRow && ix < totalCt; ++col) {
                string id = GenAutoId();
                var uiInfo = m_DebugUis[ix];
                ++ix;

                var buttonObj = AddToCell(ButtonTemplate, id, row, col);
                var button = buttonObj.GetComponent<UnityEngine.UI.Button>();
                var labelObj = buttonObj.transform.Find("Text (TMP)");
                if (null != labelObj) {
                    var label = labelObj.GetComponent<TMPro.TextMeshProUGUI>();
                    label.text = uiInfo.Key;

                    AddUiControl(id + "|Text", label);
                }
                var handler = new IndexButtonHandler(this, uiInfo.Value);
                button.onClick.AddListener(handler.OnClick);

                AddUiControl(id, button);
            }
        }

        CurUiRes = c_IndexUiRes;
        UiLoaded = true;
        CallInitUi(CurUiRes);
        UiInited = true;
    }
    public void CheckTmpFont()
    {
        if (IsNullObject(m_RootUi)) {
            return;
        }
        if (!m_Font) {
            var comps = GameObject.FindObjectsOfType<TMPro.TextMeshProUGUI>(true);
            foreach (var comp in comps) {
                if (comp.font && comp.font.name.Contains(c_FontKey)) {
                    m_Font = comp.font;
                    break;
                }
            }
        }
        if (m_Font) {
            var comps = m_RootUi.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
            foreach (var comp in comps) {
                if (!comp.font) {
                    comp.font = m_Font;
                }
            }
        }
    }

    private void CallLoadUi(string uiRes)
    {
        if (null != OnLoadUi) {
            OnLoadUi(uiRes);
        }
    }
    private void CallShowUi()
    {
        if (null != OnShowUi) {
            OnShowUi();
        }
    }
    private void CallInitUi(string uiRes)
    {
        if (null != OnInitUi) {
            OnInitUi(uiRes);
        }
    }

    private void InitUiCells()
    {
        float ratioRow = 1.0f / c_CellRowNum;
        float ratioCol = 1.0f / c_CellColNum;
        m_Cells = new RectTransform[c_CellRowNum, c_CellColNum];
        for (int i = 0; i < c_CellRowNum; ++i) {
            for (int j = 0; j < c_CellColNum; ++j) {
                var cellObj = new GameObject("cell" + i + "" + j);
                cellObj.transform.SetParent(m_RootUi.transform);
                var cell = cellObj.AddComponent<RectTransform>();
                cell.pivot = new Vector2(0.5f, 0.5f);
                cell.anchorMin = new Vector2(ratioCol * j, ratioRow * (c_CellRowNum - i - 1));
                cell.anchorMax = new Vector2(ratioCol * (j + 1), ratioRow * (c_CellRowNum - i));
                cell.offsetMin = Vector2.zero;
                cell.offsetMax = Vector2.zero;
                cell.localScale = Vector3.one;
                m_Cells[i, j] = cell;
            }
        }
    }
    private void ClearCells()
    {
        foreach (var cell in m_Cells) {
            var list = new List<GameObject>();
            for (int ix = 0; ix < cell.childCount; ++ix) {
                var gobj = cell.GetChild(ix).gameObject;
                list.Add(gobj);
            }
            cell.DetachChildren();
            foreach (var gobj in list) {
                StoryScriptUtility.DestroyObject(gobj);
            }
        }

        UiControls.Clear();
    }

    private void BuildLabel(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd) {
            var id = fd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = fd.GetParamId(1);
            var colStr = fd.GetParamId(2);
            var caption = fd.GetParamId(3);
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                AddLabel(id, row, col, caption);
            }
        }
    }
    private void BuildInput(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd) {
            var id = fd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = fd.GetParamId(1);
            var colStr = fd.GetParamId(2);
            var dataType = fd.GetParamId(3);
            var method = fd.GetParamId(4);
            bool hasDef = fd.GetParamNum() > 5;
            var defStr = hasDef ? fd.GetParamId(5) : string.Empty;
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                AddInput(id, row, col, dataType, method, hasDef, defStr);
            }
        }
    }
    private void BuildButton(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd) {
            var id = fd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = fd.GetParamId(1);
            var colStr = fd.GetParamId(2);
            var caption = fd.GetParamId(3);
            var method = fd.GetParamId(4);
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                AddButton(id, row, col, caption, method);
            }
        }
    }
    private void BuildDropdown(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd && fd.IsHighOrder && fd.HaveStatement()) {
            var cd = fd.LowerOrderFunction;
            var id = cd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = cd.GetParamId(1);
            var colStr = cd.GetParamId(2);
            var method = cd.GetParamId(3);
            bool hasDef = cd.GetParamNum() > 4;
            var selStr = hasDef ? cd.GetParamId(4) : string.Empty;
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                var dropdown = AddDropdown(id, row, col, method);
                var list = new List<string>();
                foreach (var p in fd.Params) {
                    list.Add(p.GetId());
                }
                dropdown.AddOptions(list);
                if (hasDef && int.TryParse(selStr, out var select)) {
                    dropdown.value = select;
                }
            }
        }
    }
    private void BuildToggle(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd) {
            var id = fd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = fd.GetParamId(1);
            var colStr = fd.GetParamId(2);
            var caption = fd.GetParamId(3);
            var method = fd.GetParamId(4);
            bool hasDef = fd.GetParamNum() > 5;
            var checkStr = hasDef ? fd.GetParamId(5) : string.Empty;
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                AddToggle(id, row, col, caption, method, hasDef, checkStr == "true" || checkStr == "True");
            }
        }
    }
    private void BuildToggleGroup(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd && fd.IsHighOrder && fd.HaveStatement()) {
            var cd = fd.LowerOrderFunction;
            var id = cd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = cd.GetParamId(1);
            var colStr = cd.GetParamId(2);
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)) {
                var group = AddToggleGroup(id, row, col);
                foreach (var p in fd.Params) {
                    var pfd = p as Dsl.FunctionData;
                    if (null != pfd && pfd.GetId() == "toggle") {
                        var pid = pfd.GetParamId(0);
                        pid = ResolveId(pid);
                        var pcap = pfd.GetParamId(1);
                        var method = pfd.GetParamId(2);
                        bool hasDef = pfd.GetParamNum() > 3;
                        var pchecked = hasDef ? pfd.GetParamId(3) : string.Empty;
                        AddToToggleGroup(group, pid, row, col++, pcap, method, hasDef, pchecked == "true" || pchecked == "True");
                    }
                }
            }
        }
    }
    private void BuildSlider(Dsl.ISyntaxComponent info)
    {
        var fd = info as Dsl.FunctionData;
        if (null != fd) {
            var id = fd.GetParamId(0);
            id = ResolveId(id);
            var rowStr = fd.GetParamId(1);
            var colStr = fd.GetParamId(2);
            var method = fd.GetParamId(3);
            var minValStr = fd.GetParamId(4);
            var maxValStr = fd.GetParamId(5);
            bool hasDef = fd.GetParamNum() > 6;
            var defValStr = hasDef ? fd.GetParamId(6) : string.Empty;
            if (int.TryParse(rowStr, out var row) && int.TryParse(colStr, out var col)
                && float.TryParse(minValStr, out var minVal) && float.TryParse(maxValStr, out var maxVal) && float.TryParse(defValStr, out var defVal)) {
                AddSlider(id, row, col, method, minVal, maxVal, hasDef, defVal);
            }
        }
    }

    private void AddLabel(string id, int row, int col, string caption)
    {
        var labelObj = AddToCell(LabelTemplate, id, row, col);
        var label = labelObj.GetComponent<TMPro.TextMeshProUGUI>();
        label.text = caption;

        AddUiControl(id, label);
    }
    private void AddInput(string id, int row, int col, string dataType, string method, bool hasDef, string defStr)
    {
        var inputObj = AddToCell(InputTemplate, id, row, col);
        var input = inputObj.GetComponent<TMPro.TMP_InputField>();
        input.inputType = TMPro.TMP_InputField.InputType.Standard;
        if (dataType == "int")
            input.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
        else if (dataType == "float")
            input.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        else
            input.contentType = TMPro.TMP_InputField.ContentType.Standard;
        if (hasDef) {
            input.text = defStr;
        }

        var mobj = GetEventHandler<string>(method);
        if (null != mobj) {
            input.onValueChanged.AddListener(mobj);
        }

        AddUiControl(id, input);
    }
    private void AddButton(string id, int row, int col, string caption, string method)
    {
        var buttonObj = AddToCell(ButtonTemplate, id, row, col);
        var button = buttonObj.GetComponent<UnityEngine.UI.Button>();
        var labelObj = buttonObj.transform.Find("Text (TMP)");
        if (null != labelObj) {
            var label = labelObj.GetComponent<TMPro.TextMeshProUGUI>();
            label.text = caption;

            AddUiControl(id + "|Text", label);
        }
        var mobj = GetEventHandler(method);
        if (null != mobj) {
            button.onClick.AddListener(mobj);
        }

        AddUiControl(id, button);
    }
    private TMPro.TMP_Dropdown AddDropdown(string id, int row, int col, string method)
    {
        var dropdownObj = AddToCell(DropDownTemplate, id, row, col);
        var dropdown = dropdownObj.GetComponent<TMPro.TMP_Dropdown>();
        var mobj = GetEventHandler<int>(method);
        if (null != mobj) {
            dropdown.onValueChanged.AddListener(mobj);
        }

        AddUiControl(id, dropdown);
        return dropdown;
    }
    private void AddToggle(string id, int row, int col, string caption, string method, bool hasDef, bool isChecked)
    {
        var toggleObj = AddToCell(ToggleTemplate, id, row, col);
        var toggle = toggleObj.GetComponent<UnityEngine.UI.Toggle>();
        SetToggleLabel(toggleObj.transform, id, caption);
        if (hasDef) {
            toggle.isOn = isChecked;
        }
        toggle.group = null;
        var mobj = GetEventHandler<bool>(method);
        if (null != mobj) {
            toggle.onValueChanged.AddListener(mobj);
        }

        AddUiControl(id, toggle);
    }
    private UnityEngine.UI.ToggleGroup AddToggleGroup(string id, int row, int col)
    {
        var toggleObj = AddToCell(ToggleGroupTemplate, id, row, col);
        var toggle = toggleObj.GetComponent<UnityEngine.UI.ToggleGroup>();

        AddUiControl(id, toggle);
        return toggle;
    }
    private void AddToToggleGroup(UnityEngine.UI.ToggleGroup group, string id, int row, int col, string caption, string method, bool hasDef, bool isChecked)
    {
        var toggleObj = AddToCell(ToggleTemplate, id, row, col);
        var toggle = toggleObj.GetComponent<UnityEngine.UI.Toggle>();
        SetToggleLabel(toggleObj.transform, id, caption);
        if (hasDef) {
            toggle.isOn = isChecked;
        }
        toggle.group = group;
        var mobj = GetEventHandler<bool>(method);
        if (null != mobj) {
            toggle.onValueChanged.AddListener(mobj);
        }

        AddUiControl(id, toggle);
    }
    // the rebuilt hierarchy carries a TMP label, while the prefab label is a
    // legacy ugui Text; both are supported so neither path loses the caption
    private void SetToggleLabel(Transform toggleObj, string id, string caption)
    {
        var labelObj = toggleObj.Find("Label");
        if (null == labelObj) {
            return;
        }
        var tmp = labelObj.GetComponent<TMPro.TextMeshProUGUI>();
        if (null != tmp) {
            tmp.text = caption;

            AddUiControl(id + "|Label", tmp);
            return;
        }
        var label = labelObj.GetComponent<Text>();
        if (null == label) {
            label = labelObj.gameObject.AddComponent<Text>();
        }
        label.text = caption;

        AddUiControl(id + "|Label", label);
    }
    private void AddSlider(string id, int row, int col, string method, float minVal, float maxVal, bool hasDef, float defVal)
    {
        var sliderObj = AddToCell(SliderTemplate, id, row, col);
        var slider = sliderObj.GetComponent<UnityEngine.UI.Slider>();
        slider.minValue = minVal;
        slider.maxValue = maxVal;
        slider.wholeNumbers = false;
        if (hasDef) {
            slider.value = defVal;
        }
        var mobj = GetEventHandler<float>(method);
        if (null != mobj) {
            slider.onValueChanged.AddListener(mobj);
        }

        AddUiControl(id, slider);
    }

    private GameObject AddToCell(GameObject template, string id, int row, int col)
    {
        var uiObj = Object.Instantiate<GameObject>(template, m_Cells[row, col]);
        uiObj.SetActive(true);
        uiObj.name = id;
        var rt = uiObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
        return uiObj;
    }
    private void AddUiControl(string id, UIBehaviour ui)
    {
        UiControls[id] = ui;
    }
    private string ResolveId(string id)
    {
        if (id == c_AutoIdKeyword)
            return GenAutoId();
        else
            return id;
    }
    private string GenAutoId()
    {
        ++m_CurAutoId;
        return c_AutoIdKeyword + "_" + m_CurAutoId.ToString();
    }

    //These two methods are used to associate the event processing method in the UI configuration
    //to the corresponding C# method of UiHandler (the actual method is later in this document)
    private UnityAction GetEventHandler(string method)
    {
        var t = ApiObject.GetType();
        var mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (null != mi) {
            var delegation = System.Delegate.CreateDelegate(typeof(UnityAction), ApiObject, mi, false);
            if (null != delegation)
                return (UnityAction)delegation;
            else
                return new ReflectionInvoker0(mi, ApiObject).Invoke;
        }
        else {
            mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (null != mi) {
                var delegation = System.Delegate.CreateDelegate(typeof(UnityAction), mi, false);
                if (null != delegation)
                    return (UnityAction)delegation;
                else
                    return new ReflectionInvoker0(mi, null).Invoke;
            }
        }
        return null;
    }
    private UnityAction<T> GetEventHandler<T>(string method)
    {
        var t = ApiObject.GetType();
        var mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (null != mi) {
            var delegation = System.Delegate.CreateDelegate(typeof(UnityAction<T>), ApiObject, mi, false);
            if (null != delegation)
                return (UnityAction<T>)delegation;
            else
                return new ReflectionInvoker1<T>(mi, ApiObject).Invoke;
        }
        else {
            mi = t.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (null != mi) {
                var delegation = System.Delegate.CreateDelegate(typeof(UnityAction<T>), mi, false);
                if (null != delegation)
                    return (UnityAction<T>)delegation;
                else
                    return new ReflectionInvoker1<T>(mi, null).Invoke;
            }
        }
        return null;
    }

    private sealed class IndexButtonHandler
    {
        private readonly TabularDebugUI m_Owner;
        private readonly string m_UiRes;

        public IndexButtonHandler(TabularDebugUI owner, string uiRes)
        {
            m_Owner = owner;
            m_UiRes = uiRes;
        }
        public void OnClick()
        {
            m_Owner.CallLoadUi(m_UiRes);
            m_Owner.CallShowUi();
        }
    }
    private sealed class ReflectionInvoker0
    {
        private readonly System.Reflection.MethodInfo m_Method;
        private readonly object m_Target;

        public ReflectionInvoker0(System.Reflection.MethodInfo method, object target)
        {
            m_Method = method;
            m_Target = target;
        }
        public void Invoke()
        {
            m_Method.Invoke(m_Target, null);
        }
    }
    private sealed class ReflectionInvoker1<T>
    {
        private readonly System.Reflection.MethodInfo m_Method;
        private readonly object m_Target;
        private readonly object[] m_Args = new object[1];

        public ReflectionInvoker1(System.Reflection.MethodInfo method, object target)
        {
            m_Method = method;
            m_Target = target;
        }
        public void Invoke(T val)
        {
            m_Args[0] = val;
            m_Method.Invoke(m_Target, m_Args);
        }
    }

    private List<KeyValuePair<string, string>> m_DebugUis;
    private GameObject m_RootUi;
    private RectTransform[,] m_Cells;
    private int m_CurAutoId = 0;
    private TMPro.TMP_FontAsset m_Font;
    private const string c_FontKey = "FZSHK_SDF";

    private const string c_AutoIdKeyword = "@auto";
    private const int c_CellRowNum = 20;
    private const int c_CellColNum = 6;

    public static bool IsNullObject(Object obj)
    {
        return null == obj;
    }
}
