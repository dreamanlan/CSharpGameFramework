using System.Collections.Generic;
using System.IO;
using UnityEngine;
using StoryScript;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This file handles all GM-related UI logic. Considering that one project may be used for
/// experiments with multiple functions, the UI is configurable, but the logic is all written
/// here.
/// We provide a loadui ("ui configuration resource") command to load resources, but generally
/// you can load the resources you want to use for the current experiment directly in Start.
/// If you don't want to re-output the package, you can use the command to switch ui.
/// The ui configuration uses MetaDSL syntax, but the script interpreter is not considered here,
/// because the functional codes of the experiment are hard-coded in the project, and using
/// scripts is of little significance and may complicate things.
/// </summary>
[UnityEngine.Scripting.Preserve]
public class UiHandler : MonoBehaviour
{
    public GameObject LabelTemplate;
    public GameObject InputTemplate;
    public GameObject ButtonTemplate;
    public GameObject DropDownTemplate;
    public GameObject ToggleTemplate;
    public GameObject ToggleGroupTemplate;
    public GameObject SliderTemplate;

    private void Start()
    {
        m_DebugUi.LabelTemplate = LabelTemplate;
        m_DebugUi.InputTemplate = InputTemplate;
        m_DebugUi.ButtonTemplate = ButtonTemplate;
        m_DebugUi.DropDownTemplate = DropDownTemplate;
        m_DebugUi.ToggleTemplate = ToggleTemplate;
        m_DebugUi.ToggleGroupTemplate = ToggleGroupTemplate;
        m_DebugUi.SliderTemplate = SliderTemplate;

        m_DebugUi.ApiObject = this;
        m_DebugUi.OnLoadUi = LoadUi;
        m_DebugUi.OnShowUi = ShowUi;
        m_DebugUi.OnInitUi = OnInitUi;
        m_DebugUi.Init();

        //The UI of the current experiment is loaded here. Only one is the current one.
        LoadUi(c_TestUI);
    }
    private void OnDestroy()
    {
        ReleaseSkinSprites();
    }
    public void ShowUi()
    {
        m_DebugUi.ShowRootUi();
        OnUiShow(m_DebugUi.CurUiRes);
    }
    public void HideUi()
    {
        m_DebugUi.HideRootUi();
    }
    public void LoadUi(string res)
    {
        var ta = Resources.Load<TextAsset>(res);
        if (TabularDebugUI.IsNullObject(ta)) {
            string path = GmRootScript.ScriptPath + res + ".txt";
            if (File.Exists(path)) {
                string text = File.ReadAllText(path);
                m_DebugUi.LoadUi(res, text);
            }
        }
        else {
            m_DebugUi.LoadUi(ta.name, ta.text);
        }
    }
    public void LoadAndShowIndexUi()
    {
        m_DebugUi.LoadAndShowIndexUi();
    }

    //UI configuration is a .txt file in the Resources directory
    //After the UI is loaded, initialization processing is performed, mainly initial value setting.
    private void OnInitUi(string res)
    {
        if (res == c_TestUI) {
            InitTestUi();
        }
    }
    //Values on the UI can be reset or synchronized while the UI is displayed
    private void OnUiShow(string res)
    {
        m_DebugUi.CheckTmpFont();
        if (res == c_TestUI) {
            InitTestUi();
        }
    }

    //Event handling corresponding to TestUI configuration
    private void InitTestUi()
    {
    }
    private void OnButton()
    {
        LogSystem.Warn("OnButton");
    }
    private void OnValueChanged(int val)
    {
        LogSystem.Warn("OnValueChanged {0}", val);
    }
    private void OnCheckedChanged(bool val)
    {
        LogSystem.Warn("OnCheckedChanged {0}", val);
    }
    private void OnOneChanged(bool val)
    {
        LogSystem.Warn("OnOneChanged {0}", val);
    }
    private void OnTwoChanged(bool val)
    {
        LogSystem.Warn("OnTwoChanged {0}", val);
    }
    private void OnSliderChanged(float val)
    {
        LogSystem.Warn("OnSliderChanged {0}", val);
    }


    private const string c_CanvasNode = "Canvas";
    // GmScript.prefab: Canvas.m_AdditionalShaderChannelsFlag
    private const int c_CanvasShaderChannels = 27;
    // edge length of the generated ui skin sprites
    private const int c_SkinSize = 32;

    private const string c_TestUI = "TestUI";

    private TabularDebugUI m_DebugUi = new TabularDebugUI(new List<KeyValuePair<string, string>>
        {
            KeyValuePair.Create(c_TestUI, c_TestUI),
        });

    public static UiHandler TryInstall(GameObject prefab)
    {
        // null check, inactive assert and idempotent install are handled by the utility
        var uiHandler = StoryScriptUtility.TryInstallComponent<UiHandler>(prefab);
        if (null == uiHandler) {
            return null;
        }

        // when GmScript.prefab fails to load, GmRootScript falls back to a bare
        // GameObject without any children; rebuild the prefab ui hierarchy at
        // runtime so the templates below can be bound
        TryRebuildUiHierarchy(prefab);

        // collect child transforms by name (shallowest wins on conflicts),
        // including inactive ones
        var map = StoryScriptUtility.CollectChildrenByName(prefab.transform);

        uiHandler.LabelTemplate = TryFindTemplate(map, "TextTmpl");
        uiHandler.InputTemplate = TryFindTemplate(map, "InputTmpl");
        uiHandler.ButtonTemplate = TryFindTemplate(map, "ButtonTmpl");
        uiHandler.DropDownTemplate = TryFindTemplate(map, "DropdownTmpl");
        uiHandler.ToggleTemplate = TryFindTemplate(map, "ToggleTmpl");
        uiHandler.ToggleGroupTemplate = TryFindTemplate(map, "ToggleGroupTmpl");
        uiHandler.SliderTemplate = TryFindTemplate(map, "SliderTmpl");

        // bind the two navigation buttons on the prefab
        TryBindButtonClick(map, "HideBtn", uiHandler.HideUi);
        TryBindButtonClick(map, "IndexUI", uiHandler.LoadAndShowIndexUi);

        // missing templates are tolerated (fields stay null), but log once
        // to expose unexpected prefab structure changes early
        if (null == uiHandler.LabelTemplate || null == uiHandler.InputTemplate || null == uiHandler.ButtonTemplate
            || null == uiHandler.DropDownTemplate || null == uiHandler.ToggleTemplate
            || null == uiHandler.ToggleGroupTemplate || null == uiHandler.SliderTemplate) {
            UnityEngine.Debug.LogWarning("UiHandler.TryInstall: some templates missing under " + prefab.name);
        }
        return uiHandler;
    }
    private static GameObject TryFindTemplate(Dictionary<string, Transform> map, string name)
    {
        return map.TryGetValue(name, out var t) ? t.gameObject : null;
    }
    private static void TryBindButtonClick(Dictionary<string, Transform> map, string name, UnityAction handler)
    {
        if (map.TryGetValue(name, out var t)) {
            var button = t.GetComponent<Button>();
            if (null != button) {
                // remove runtime listeners first, so repeated TryInstall calls never
                // stack duplicate handlers (persistent listeners are not affected)
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(handler);
                return;
            }
        }
        UnityEngine.Debug.LogWarning("UiHandler.TryInstall: button " + name + " not found");
    }

    // Rebuild the GmScript ui hierarchy at runtime, mirroring
    // Assets/Release/Res/MoreFunTech/Resources/GmScript.prefab exactly (node
    // names, anchors, component values all come from the prefab).
    // The built-in ui skin sprites referenced by the prefab are not reachable
    // at runtime, so images are left sprite-less (solid color) and rendered as
    // Simple instead of Sliced; layouts and interactions are unaffected.
    private static void TryRebuildUiHierarchy(GameObject root)
    {
        if (null != root.transform.Find(c_CanvasNode)) {
            return;
        }
        UnityEngine.Debug.LogWarning("UiHandler.TryInstall: rebuilding GmScript ui hierarchy at runtime");
        int uiLayer = 5;
        var centerPivot = new Vector2(0.5f, 0.5f);
        var centerAnchor = new Vector2(0.5f, 0.5f);
        var zero = Vector2.zero;
        var one = Vector2.one;

        var canvasRt = CreateChild(c_CanvasNode, root.transform, zero, zero, zero, zero, zero, uiLayer);
        var canvas = canvasRt.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // stay above every game window canvas (the ui framework assigns them
        // incrementally from 1), but BELOW the TMP dropdown popup/blocker
        // canvases (hardcoded 30000/29999 in TMP_Dropdown): if the gm canvas
        // sat above them, the expanded list would render and raycast behind
        // the gm panel and the blocker could never win a click, so an open
        // dropdown would never close
        canvas.sortingOrder = 29998;
        // mirrors the prefab's m_AdditionalShaderChannelsFlag value (27)
        canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)c_CanvasShaderChannels;
        var scaler = canvasRt.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1024, 768);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.0f;
        canvasRt.gameObject.AddComponent<GraphicRaycaster>();

        var rootPanel = CreateChild("RootPanel", canvasRt, zero, one, zero, zero, centerPivot, uiLayer);
        var panelImg = AddImage(rootPanel, new Color(1f, 1f, 1f, 0f), GetBackgroundSkin(), true);
        panelImg.raycastTarget = false;

        // HideBtn: top-right corner button "Hide UI"
        var hideBtn = CreateChild("HideBtn", rootPanel, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-40f, -10f), new Vector2(60f, 22f), centerPivot, uiLayer);
        BuildSimpleButton(hideBtn, "Hide UI");

        // IndexUI: top-right button "Index UI", left of HideBtn
        var indexBtn = CreateChild("IndexUI", rootPanel, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-160f, -10f), new Vector2(60f, 22f), centerPivot, uiLayer);
        BuildSimpleButton(indexBtn, "Index UI");

        // TextTmpl: inactive label template
        var textTmpl = CreateChild("TextTmpl", rootPanel, centerAnchor, centerAnchor, zero, new Vector2(200f, 50f), centerPivot, uiLayer);
        var textTmp = AddTmpText(textTmpl, "New Text", new Color(1f, 0.95554936f, 0f, 1f), 36f, TMPro.TextAlignmentOptions.TopRight, true);
        textTmp.enableAutoSizing = true;
        textTmp.fontSizeMin = 10;
        textTmp.fontSizeMax = 72;
        textTmpl.gameObject.SetActive(false);

        // InputTmpl: inactive input field template
        var inputTmpl = CreateChild("InputTmpl", rootPanel, centerAnchor, centerAnchor, zero, new Vector2(160f, 30f), centerPivot, uiLayer);
        var inputImg = AddImage(inputTmpl, new Color(0f, 1f, 1f, 1f), GetInputSkin(), true);
        var input = inputTmpl.gameObject.AddComponent<TMPro.TMP_InputField>();
        input.targetGraphic = inputImg;
        input.scrollSensitivity = 1f;
        input.caretBlinkRate = 0.85f;
        var textArea = CreateChild("Text Area", inputTmpl, zero, one, new Vector2(0f, -0.5f), new Vector2(-20f, -13f), centerPivot, uiLayer);
        var textAreaMask = textArea.gameObject.AddComponent<RectMask2D>();
        textAreaMask.padding = new Vector4(-8f, -5f, -8f, -5f);
        var placeholder = CreateChild("Placeholder", textArea, zero, one, zero, zero, centerPivot, uiLayer);
        var placeholderTmp = AddTmpText(placeholder, "Enter text...", new Color(1f, 0.97164285f, 0f, 0.5f), 14f, TMPro.TextAlignmentOptions.TopLeft, false);
        placeholderTmp.fontStyle = TMPro.FontStyles.Italic;
        var placeholderLe = placeholder.gameObject.AddComponent<LayoutElement>();
        placeholderLe.ignoreLayout = true;
        var inputText = CreateChild("Text", textArea, zero, one, zero, zero, centerPivot, uiLayer);
        var inputTmp = AddTmpText(inputText, "\u200B", new Color(1f, 0.92332065f, 0f, 1f), 14f, TMPro.TextAlignmentOptions.TopLeft, false);
        input.textViewport = textArea;
        input.textComponent = inputTmp;
        input.placeholder = placeholderTmp;
        inputTmpl.gameObject.SetActive(false);

        // ButtonTmpl: inactive button template
        var buttonTmpl = CreateChild("ButtonTmpl", rootPanel, centerAnchor, centerAnchor, zero, new Vector2(160f, 30f), centerPivot, uiLayer);
        var buttonImg = AddImage(buttonTmpl, new Color(0f, 0.952054f, 1f, 1f), GetStdSkin(), true);
        var button = buttonTmpl.gameObject.AddComponent<Button>();
        button.targetGraphic = buttonImg;
        AddFillChildTmpText(buttonTmpl, "Text (TMP)", "Button", new Color(1f, 0.9027058f, 0f, 1f), uiLayer);
        buttonTmpl.gameObject.SetActive(false);

        // DropdownTmpl: inactive dropdown template
        var dropdownTmpl = CreateChild("DropdownTmpl", rootPanel, centerAnchor, centerAnchor, new Vector2(10.4f, 38f), new Vector2(160f, 30f), centerPivot, uiLayer);
        var dropdownImg = AddImage(dropdownTmpl, new Color(0f, 0.86666703f, 1f, 1f), GetStdSkin(), true);
        var dropdownLabel = CreateChild("Label", dropdownTmpl, zero, one, new Vector2(-7.5f, -0.5f), new Vector2(-35f, -13f), centerPivot, uiLayer);
        var dropdownLabelTmp = AddTmpText(dropdownLabel, string.Empty, new Color(1f, 0.93621373f, 0.06132078f, 1f), 14f, TMPro.TextAlignmentOptions.Left, false);
        var arrow = CreateChild("Arrow", dropdownTmpl, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-15f, 0f), new Vector2(20f, 20f), centerPivot, uiLayer);
        AddImage(arrow, new Color(0.9716981f, 0.89733535f, 0.0779192f, 1f), GetArrowSkin(), false);
        var template = CreateChild("Template", dropdownTmpl, zero, new Vector2(1f, 0f), new Vector2(0f, 2f), new Vector2(0f, 150f), new Vector2(0.5f, 1f), uiLayer);
        AddImage(template, new Color(0.0509078f, 0.90611094f, 0.9811321f, 1f), GetStdSkin(), true);
        var scrollRect = template.gameObject.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        var viewport = CreateChild("Viewport", template, zero, one, zero, new Vector2(-18f, 0f), new Vector2(0f, 1f), uiLayer);
        AddImage(viewport, new Color(0f, 0.93359566f, 1f, 1f), GetMaskSkin(), true);
        var viewportMask = viewport.gameObject.AddComponent<Mask>();
        viewportMask.showMaskGraphic = false;
        var content = CreateChild("Content", viewport, new Vector2(0f, 1f), new Vector2(1f, 1f), zero, new Vector2(0f, 28f), new Vector2(0.5f, 1f), uiLayer);
        var item = CreateChild("Item", content, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), zero, new Vector2(0f, 20f), centerPivot, uiLayer);
        var itemBackground = CreateChild("Item Background", item, zero, one, zero, zero, centerPivot, uiLayer);
        // translucent so the option rows do not read as opaque rectangles
        // covering the rounded template window
        var itemBackgroundImg = AddImage(itemBackground, new Color(0f, 0.95917225f, 1f, 0.35f));
        var itemCheckmark = CreateChild("Item Checkmark", item, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(10f, 0f), new Vector2(20f, 20f), centerPivot, uiLayer);
        var itemCheckmarkImg = AddImage(itemCheckmark, new Color(0.9433962f, 0.85919625f, 0.06674972f, 1f), GetCheckmarkSkin(), false);
        var itemLabel = CreateChild("Item Label", item, zero, one, new Vector2(5f, -0.5f), new Vector2(-30f, -3f), centerPivot, uiLayer);
        var itemLabelTmp = AddTmpText(itemLabel, "Option A", new Color(0.9716981f, 0.90682757f, 0.050418302f, 1f), 14f, TMPro.TextAlignmentOptions.Left, false);
        var itemToggle = item.gameObject.AddComponent<Toggle>();
        itemToggle.targetGraphic = itemBackgroundImg;
        itemToggle.graphic = itemCheckmarkImg;
        itemToggle.isOn = true;
        var scrollbar = CreateChild("Scrollbar", template, new Vector2(1f, 0f), new Vector2(1f, 1f), zero, new Vector2(20f, 0f), new Vector2(1f, 1f), uiLayer);
        var scrollbarImg = AddImage(scrollbar, new Color(0f, 0.91935134f, 1f, 1f), GetBackgroundSkin(), true);
        var scrollbarComp = scrollbar.gameObject.AddComponent<Scrollbar>();
        scrollbarComp.targetGraphic = scrollbarImg;
        var slidingArea = CreateChild("Sliding Area", scrollbar, zero, one, zero, new Vector2(-20f, -20f), centerPivot, uiLayer);
        var scrollbarHandle = CreateChild("Handle", slidingArea, zero, new Vector2(1f, 0.2f), zero, new Vector2(20f, 20f), centerPivot, uiLayer);
        scrollbarComp.handleRect = scrollbarHandle;
        scrollbarComp.direction = Scrollbar.Direction.BottomToTop;
        scrollbarComp.size = 0.2f;
        scrollbarComp.numberOfSteps = 0;
        AddImage(scrollbarHandle, new Color(0.9622642f, 0.05900679f, 0.9602558f, 1f), GetStdSkin(), true);
        scrollRect.content = content;
        scrollRect.viewport = viewport;
        scrollRect.verticalScrollbar = scrollbarComp;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        scrollRect.verticalScrollbarSpacing = -3f;
        var dropdown = dropdownTmpl.gameObject.AddComponent<GmDropdown>();
        dropdown.targetGraphic = dropdownImg;
        dropdown.captionText = dropdownLabelTmp;
        dropdown.itemText = itemLabelTmp;
        dropdown.template = template;
        template.gameObject.SetActive(false);
        dropdownTmpl.gameObject.SetActive(false);

        // ToggleTmpl: inactive toggle template, label is a legacy ugui Text
        // (TabularDebugUI reads it via SetToggleLabel, TMP first)
        var toggleTmpl = CreateChild("ToggleTmpl", rootPanel, centerAnchor, centerAnchor, zero, new Vector2(160f, 20f), centerPivot, uiLayer);
        // prefab pins a 20x20 box to the top-left corner, which reads as half
        // height once a cell stretches the template root, so stretch the box
        // vertically with a small padding instead
        var toggleBackground = CreateChild("Background", toggleTmpl, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(10f, 0f), new Vector2(22f, -14f), new Vector2(0f, 0.5f), uiLayer);
        var toggleBackgroundImg = AddImage(toggleBackground, new Color(0.032707386f, 0.87623054f, 0.990566f, 1f), GetStdSkin(), true);
        var checkmark = CreateChild("Checkmark", toggleBackground, zero, one, zero, new Vector2(-6f, -6f), centerPivot, uiLayer);
        var checkmarkImg = AddImage(checkmark, new Color(0.9622642f, 0.89677244f, 0.040850822f, 1f), GetCheckmarkSkin(), false);
        // prefab uses a legacy ugui Text here, rebuilt as TMP (font size 14,
        // top-left aligned, same color) so no legacy font asset is required
        var toggleLabel = CreateChild("Label", toggleTmpl, zero, one, new Vector2(9f, -0.5f), new Vector2(-28f, -3f), centerPivot, uiLayer);
        // shift the label right so it clears the stretched checkbox
        toggleLabel.offsetMin = new Vector2(36f, toggleLabel.offsetMin.y);
        AddTmpText(toggleLabel, "Toggle", new Color(0.9716981f, 0.8922082f, 0.050418302f, 1f), 14f, TMPro.TextAlignmentOptions.TopLeft, true);
        var toggle = toggleTmpl.gameObject.AddComponent<Toggle>();
        toggle.targetGraphic = toggleBackgroundImg;
        toggle.graphic = checkmarkImg;
        toggle.isOn = false;
        toggleTmpl.gameObject.SetActive(false);

        // ToggleGroupTmpl: inactive toggle group template
        var toggleGroupTmpl = CreateChild("ToggleGroupTmpl", rootPanel, zero, one, zero, zero, centerPivot, uiLayer);
        var toggleGroup = toggleGroupTmpl.gameObject.AddComponent<ToggleGroup>();
        toggleGroup.allowSwitchOff = false;
        toggleGroupTmpl.gameObject.SetActive(false);

        // SliderTmpl: inactive slider template
        var sliderTmpl = CreateChild("SliderTmpl", rootPanel, centerAnchor, centerAnchor, new Vector2(142.9f, 0f), new Vector2(160f, 20f), centerPivot, uiLayer);
        var sliderBackground = CreateChild("Background", sliderTmpl, new Vector2(0f, 0.25f), new Vector2(1f, 0.75f), zero, zero, centerPivot, uiLayer);
        var sliderBackgroundImg = AddImage(sliderBackground, new Color(0.07008721f, 0.908824f, 0.990566f, 1f), GetBackgroundSkin(), true);
        var fillArea = CreateChild("Fill Area", sliderTmpl, new Vector2(0f, 0.25f), new Vector2(1f, 0.75f), new Vector2(-5f, 0f), new Vector2(-20f, 0f), centerPivot, uiLayer);
        var fill = CreateChild("Fill", fillArea, zero, new Vector2(0f, 1f), zero, new Vector2(10f, 0f), centerPivot, uiLayer);
        AddImage(fill, new Color(0.9433962f, 0.85135114f, 0.06674972f, 1f), GetStdSkin(), true);
        var handleSlideArea = CreateChild("Handle Slide Area", sliderTmpl, zero, one, zero, new Vector2(-20f, 0f), centerPivot, uiLayer);
        var sliderHandle = CreateChild("Handle", handleSlideArea, zero, new Vector2(0f, 1f), zero, new Vector2(20f, 0f), centerPivot, uiLayer);
        AddImage(sliderHandle, new Color(0.94131213f, 0f, 0.9716981f, 1f), GetKnobSkin(), false);
        var slider = sliderTmpl.gameObject.AddComponent<Slider>();
        slider.targetGraphic = sliderBackgroundImg;
        slider.fillRect = fill;
        slider.handleRect = sliderHandle;
        slider.direction = Slider.Direction.LeftToRight;
        slider.wholeNumbers = true;
        sliderTmpl.gameObject.SetActive(false);
    }
    private static RectTransform CreateChild(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 anchoredPos, Vector2 sizeDelta, Vector2 pivot, int layer)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.layer = layer;
        var rt = (RectTransform)go.transform;
        rt.SetParent(parent, false);
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        rt.pivot = pivot;
        rt.localScale = Vector3.one;
        return rt;
    }
    private static Image AddImage(RectTransform rt, Color color, Sprite sprite, bool sliced)
    {
        var img = rt.gameObject.AddComponent<Image>();
        img.sprite = sprite;
        img.type = sliced ? Image.Type.Sliced : Image.Type.Simple;
        img.color = color;
        img.raycastTarget = true;
        return img;
    }
    private static Image AddImage(RectTransform rt, Color color)
    {
        // no sprite (matches the few prefab images without one), plain solid quad
        return AddImage(rt, color, null, false);
    }
    private static TMPro.TextMeshProUGUI AddTmpText(RectTransform rt, string text, Color color, float fontSize,
        TMPro.TextAlignmentOptions alignment, bool wordWrapping)
    {
        var tmp = rt.gameObject.AddComponent<TMPro.TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = color;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.enableWordWrapping = wordWrapping;
        return tmp;
    }
    private static void AddFillChildTmpText(RectTransform parent, string name, string text, Color color, int layer)
    {
        var rt = CreateChild(name, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Vector2(0.5f, 0.5f), layer);
        AddTmpText(rt, text, color, 14f, TMPro.TextAlignmentOptions.Center, true);
    }
    private static void BuildSimpleButton(RectTransform rt, string caption)
    {
        var img = AddImage(rt, new Color(0f, 0.8666668f, 1f, 1f), GetStdSkin(), true);
        var button = rt.gameObject.AddComponent<Button>();
        button.targetGraphic = img;
        AddFillChildTmpText(rt, "Text (TMP)", caption, new Color(1f, 0.95554936f, 0f, 1f), rt.gameObject.layer);
    }

    private sealed class GmDropdown : TMPro.TMP_Dropdown
    {
        protected override GameObject CreateBlocker(Canvas rootCanvas)
        {
            var blocker = base.CreateBlocker(rootCanvas);
            blocker.GetComponent<Image>().raycastTarget = true;
            return blocker;
        }
    }

    // ---- procedural ui skin sprites ---------------------------------------
    // The prefab tints white builtin-extra sprites (UISprite.psd, Knob.psd, ...)
    // with per-node colors. Those sprites cannot be fetched by name at runtime
    // (builtin extra resources only have a name lookup in the editor), so white
    // look-alikes are generated once here and tinted through Image.color the
    // same way, with the same Sliced/Simple usage as in the prefab.
    private static Sprite s_StdSkin;
    private static Sprite s_BackgroundSkin;
    private static Sprite s_InputSkin;
    private static Sprite s_MaskSkin;
    private static Sprite s_KnobSkin;
    private static Sprite s_CheckmarkSkin;
    private static Sprite s_ArrowSkin;

    // the cached skin textures/sprites use HideFlags.HideAndDontSave and are
    // never unloaded with the scene, so release them explicitly when the gm
    // object goes away; the lazy getters above recreate them on demand
    private static void ReleaseSkinSprites()
    {
        DestroySkinSprite(ref s_StdSkin);
        DestroySkinSprite(ref s_BackgroundSkin);
        DestroySkinSprite(ref s_InputSkin);
        DestroySkinSprite(ref s_MaskSkin);
        DestroySkinSprite(ref s_KnobSkin);
        DestroySkinSprite(ref s_CheckmarkSkin);
        DestroySkinSprite(ref s_ArrowSkin);
    }
    private static void DestroySkinSprite(ref Sprite sprite)
    {
        if (null != sprite) {
            var tex = sprite.texture;
            StoryScriptUtility.DestroyObject(sprite);
            if (null != tex) {
                StoryScriptUtility.DestroyObject(tex);
            }
            sprite = null;
        }
    }

    // like UISprite.psd: bright rounded border over a nearly solid fill
    private static Sprite GetStdSkin()
    {
        if (null == s_StdSkin) {
            s_StdSkin = CreateSkinSprite(StdSkinShape, new Vector4(6.0f, 6.0f, 6.0f, 6.0f));
        }
        return s_StdSkin;
    }
    // like Background.psd: solid rounded block
    private static Sprite GetBackgroundSkin()
    {
        if (null == s_BackgroundSkin) {
            s_BackgroundSkin = CreateSkinSprite(BackgroundSkinShape, new Vector4(6.0f, 6.0f, 6.0f, 6.0f));
        }
        return s_BackgroundSkin;
    }
    // like InputFieldBackground.psd: bright rounded border over a faint fill
    private static Sprite GetInputSkin()
    {
        if (null == s_InputSkin) {
            s_InputSkin = CreateSkinSprite(InputSkinShape, new Vector4(6.0f, 6.0f, 6.0f, 6.0f));
        }
        return s_InputSkin;
    }
    // like UIMask.psd: plain white quad for the dropdown viewport mask
    private static Sprite GetMaskSkin()
    {
        if (null == s_MaskSkin) {
            s_MaskSkin = CreateSkinSprite(MaskSkinShape, Vector4.zero);
        }
        return s_MaskSkin;
    }
    // like Knob.psd: filled circle for the slider handle
    private static Sprite GetKnobSkin()
    {
        if (null == s_KnobSkin) {
            s_KnobSkin = CreateSkinSprite(KnobSkinShape, Vector4.zero);
        }
        return s_KnobSkin;
    }
    // like Checkmark.psd: white check mark drawn as two thick strokes
    // (texture space is y-up: dip to the bottom center, then rise to the top right)
    private static Sprite GetCheckmarkSkin()
    {
        if (null == s_CheckmarkSkin) {
            s_CheckmarkSkin = CreateSkinSprite(CheckmarkSkinShape, Vector4.zero);
        }
        return s_CheckmarkSkin;
    }
    // like DropdownArrow.psd: small triangle pointing down
    // (texture space is y-up: wide edge on top, apex at the bottom)
    private static Sprite GetArrowSkin()
    {
        if (null == s_ArrowSkin) {
            s_ArrowSkin = CreateSkinSprite(ArrowSkinShape, Vector4.zero);
        }
        return s_ArrowSkin;
    }
    // shape functions passed to CreateSkinSprite as named methods
    // (project rule: no anonymous functions in code files)
    private static float StdSkinShape(float x, float y, float size)
    {
        return RoundedRectShape(x, y, size, 1.0f, 0.88f);
    }
    private static float BackgroundSkinShape(float x, float y, float size)
    {
        return RoundedRectShape(x, y, size, 1.0f, 1.0f);
    }
    private static float InputSkinShape(float x, float y, float size)
    {
        return RoundedRectShape(x, y, size, 1.0f, 0.25f);
    }
    private static float MaskSkinShape(float x, float y, float size)
    {
        return 1.0f;
    }
    private static float KnobSkinShape(float x, float y, float size)
    {
        float dx = x - size * 0.5f;
        float dy = y - size * 0.5f;
        float d = Mathf.Sqrt(dx * dx + dy * dy) - (size * 0.5f - 2.0f);
        return Mathf.Clamp(0.5f - d, 0.0f, 1.0f);
    }
    private static float CheckmarkSkinShape(float x, float y, float size)
    {
        float d = Mathf.Min(
            SegmentDistance(x, y, size * 0.25f, size * 0.40f, size * 0.42f, size * 0.22f),
            SegmentDistance(x, y, size * 0.42f, size * 0.22f, size * 0.76f, size * 0.62f));
        return Mathf.Clamp(3.2f - d, 0.0f, 1.0f);
    }
    private static float ArrowSkinShape(float x, float y, float size)
    {
        float apex = size * 0.30f;
        float top = size * 0.70f;
        if (y < apex - 0.5f || y > top + 0.5f) {
            return 0.0f;
        }
        float t = Mathf.Clamp01((y - apex) / (top - apex));
        float halfW = size * 0.26f * t;
        float d = Mathf.Abs(x - size * 0.5f) - halfW;
        return Mathf.Clamp(0.5f - d, 0.0f, 1.0f);
    }
    // rounded rect with a bright border, sliced with the corners kept intact
    // (small radius: a large one makes little controls like the 20px toggle
    // checkbox read as a circle instead of a box)
    private static float RoundedRectShape(float x, float y, float size, float borderAlpha, float fillAlpha)
    {
        float c = size * 0.5f;
        float radius = size * 0.19f;
        const float border = 2.0f;
        float qx = Mathf.Abs(x - c) - (c - 1.0f - radius);
        float qy = Mathf.Abs(y - c) - (c - 1.0f - radius);
        float ax = Mathf.Max(qx, 0.0f);
        float ay = Mathf.Max(qy, 0.0f);
        float dist = Mathf.Sqrt(ax * ax + ay * ay) + Mathf.Min(Mathf.Max(qx, qy), 0.0f) - radius;
        float cover = Mathf.Clamp(0.5f - dist, 0.0f, 1.0f);
        if (cover <= 0.0f) {
            return 0.0f;
        }
        float alpha = (-dist) < border ? borderAlpha : fillAlpha;
        return cover * alpha;
    }
    // rasterize a coverage function (0..1 per pixel) into a white sprite
    private static Sprite CreateSkinSprite(System.Func<float, float, float, float> shape, Vector4 border)
    {
        int size = c_SkinSize;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        tex.hideFlags = HideFlags.HideAndDontSave;
        var cols = new Color32[size * size];
        for (int y = 0; y < size; ++y) {
            for (int x = 0; x < size; ++x) {
                float a = Mathf.Clamp01(shape(x + 0.5f, y + 0.5f, size));
                cols[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255.0f));
            }
        }
        tex.SetPixels32(cols);
        tex.Apply(false, false);
        var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, size, size), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect, border);
        sprite.hideFlags = HideFlags.HideAndDontSave;
        return sprite;
    }
    private static float SegmentDistance(float x, float y, float x0, float y0, float x1, float y1)
    {
        float dx = x1 - x0;
        float dy = y1 - y0;
        float len2 = dx * dx + dy * dy;
        float t = len2 > 0.0f ? Mathf.Clamp01(((x - x0) * dx + (y - y0) * dy) / len2) : 0.0f;
        float px = x0 + t * dx - x;
        float py = y0 + t * dy - y;
        return Mathf.Sqrt(px * px + py * py);
    }
}
