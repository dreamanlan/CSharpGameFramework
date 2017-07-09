using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using RichTextParser;

[System.Serializable]
public class RichText : Text, IPointerClickHandler
{
    public class DrawSpriteInfo
    {
        public string id;
        public string name;
        public Vector3[] vertices = new Vector3[4];
        public bool alreadyDraw = false;
    }
    public class LinkInfo
    {
        public string Color = string.Empty;
        public string Name = string.Empty;
    }
    public class QuadInfo
    {
        public string Name = string.Empty;
        public int Size = RichText.defaultQuadSize;
        public int Count = 1;
        public Sprite Sprite = null;
        public string SpriteResource = string.Empty;
        public GameObject InstantiatedPrefab = null;
        public string PrefabResource = string.Empty;
    }
    public delegate bool BuildLinkOrGuadInfoDelegation(string id, string name, HyperText hyperText, out LinkInfo linkInfo, out QuadInfo quadInfo);
    [System.Serializable]
    public class HrefClickEvent : UnityEvent<HyperText> { }

    public string outputText
    {
        get { return m_OutputText; }
    }
    public IRichTextList parsedTexts {
        get {
            if (null != m_ParsedTexts) {
                return m_ParsedTexts;
            } else {
                return m_RichTextParser.Texts;
            }
        }
        set { m_ParsedTexts = value; }
    }
    public BuildLinkOrGuadInfoDelegation onBuildLinkOrQuadInfo;
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }
    public List<DrawSpriteInfo> drawSpriteInfos {
        get { return m_DrawSpriteInfos; }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out lp);

        foreach (var hrefInfo in m_HrefInfos) {
            if (!hrefInfo.isLink)
                continue;
            var boxes = hrefInfo.boxes; 
            for (var i = 0; i < boxes.Count; ++i) {
                if (boxes[i].Contains(lp)) {
                    m_OnHrefClick.Invoke(hrefInfo.text);
                    var attrs = hrefInfo.text.Attrs;
                    if (attrs.Count > 0) {
                        var attr = attrs[0];
                        Debug.LogFormat("OnClick href {0}:{1}", attr.Key, attr.Value);
                    }
                    return;
                }
            }
        }
        foreach (var spriteInfo in m_SpriteInfos) {
            if (!spriteInfo.isLink)
                continue;
            var boxes = spriteInfo.boxes;
            for (var i = 0; i < boxes.Count; ++i) {
                if (boxes[i].Contains(lp)) {
                    m_OnHrefClick.Invoke(spriteInfo.text);
                    var attrs = spriteInfo.text.Attrs;
                    if (attrs.Count > 0) {
                        var attr = attrs[0];
                        Debug.LogFormat("OnClick sprite {0}:{1}", attr.Key, attr.Value);
                    }
                    return;
                }
            }
        }
    }
    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();

        Analyze();
        m_OutputText = m_TextBuilder.ToString();

        Vector2 extents = rectTransform.rect.size;
    }
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        //base.OnPopulateMesh(toFill);
        //return;
        if (font == null)
            return;

        // We don't care if we the font Texture changes while we are doing our Update.
        // The end result of cachedTextGenerator will be valid for this instance.
        // Otherwise we can get issues like Case 619238.
        m_DisableFontTextureRebuiltCallback = true;

        Vector2 extents = rectTransform.rect.size;
        var settings = GetGenerationSettings(extents);
        cachedTextGenerator.Populate(m_OutputText, settings);
        toFill.Clear();

        CalcQuadTag(toFill);

        m_DisableFontTextureRebuiltCallback = false;

        CalcLinkTag(toFill);

        #region 处理超链接的下划线--拉伸实现
        /*
        TextGenerator _UnderlineText = new TextGenerator();
        _UnderlineText.Populate("_", settings);
        IList<UIVertex> _TUT = _UnderlineText.verts;

        foreach (var hrefInfo in m_HrefInfos)
        {
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            for (int i = 0; i < hrefInfo.boxes.Count; i++)
            {
                Vector3 _StartBoxPos = new Vector3(hrefInfo.boxes[i].x, hrefInfo.boxes[i].y, 0.0f);
                Vector3 _EndBoxPos = _StartBoxPos+ new Vector3(hrefInfo.boxes[i].width, 0.0f, 0.0f);
                AddUnderlineQuad(toFill, _TUT, _StartBoxPos, _EndBoxPos);
            }

        }
        */
        #endregion
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        alignByGeometry = true;
        SetVerticesDirty();
    }

    private void CalcQuadTag(VertexHelper toFill)
    {
        Rect inputRect = rectTransform.rect;

        // get the text alignment anchor point for the text in local space
        Vector2 textAnchorPivot = GetTextAnchorPivot(alignment);
        Vector2 refPoint = Vector2.zero;
        refPoint.x = (textAnchorPivot.x == 1 ? inputRect.xMax : inputRect.xMin);
        refPoint.y = (textAnchorPivot.y == 0 ? inputRect.yMin : inputRect.yMax);

        // Determine fraction of pixel to offset text mesh.
        Vector2 roundingOffset = PixelAdjustPoint(refPoint) - refPoint;

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        //Last 4 verts are always a new line...
        int vertCount = verts.Count - 4;

        //清除乱码
        for (int i = 0; i < m_SpriteInfos.Count; i++) {
            var indexes = m_SpriteInfos[i].indexes;
            for (int j = 0; j < indexes.Count; ++j) {
                //UGUIText不支持<quad/>标签，表现为乱码，这里将他的uv全设置为0,清除乱码
                for (int m = indexes[j] * 4; m < indexes[j] * 4 + 4; m++) {
                    UIVertex tempVertex = verts[m];
                    tempVertex.uv0 = Vector2.zero;
                    verts[m] = tempVertex;
                }
            }
        }

        if (roundingOffset != Vector2.zero) {
            for (int i = 0; i < vertCount; ++i) {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        } else {
            for (int i = 0; i < vertCount; ++i) {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }

        //计算标签 计算偏移值后 再计算标签的值
        List<UIVertex> vertsTemp = new List<UIVertex>();
        for (int i = 0; i < vertCount; i++) {
            UIVertex tempVer = new UIVertex();
            toFill.PopulateUIVertex(ref tempVer, i);
            vertsTemp.Add(tempVer);
        }
        for (int i = 0; i < m_SpriteInfos.Count; i++) {
            var spriteInfo = m_SpriteInfos[i];
            var textpos1 = vertsTemp[((spriteInfo.firstIndex + 1) * 4) - 1].position;
            var textpos2 = vertsTemp[((spriteInfo.lastIndex + 1) * 4) - 1].position;

            //------------------------------------------
            //  在quad的顶点list里，顶点序为
            //  0   1
            //  3   2
            //  也就是说每个quad的最后一个顶点是左下角的点，构造三角形时，以这个点为起点计算
            //  四个顶点，序列如下，其中0是textpos，对应quad的最后顶点即左下角的点
            //  3   1
            //  0   2
            //  三角形<0,1,2>与<1,0,3>
            //  对于多个quad占位的情形，需要用到第一个quad左下角的点算0、3点，用最后一个quad
            //  的左下角的点计算1、2点。
            //------------------------------------------
            var drawSpriteInfo = m_DrawSpriteInfos[i];
            drawSpriteInfo.vertices = new Vector3[4];
            drawSpriteInfo.vertices[0] = new Vector3(0, 0, 0) + textpos1;
            drawSpriteInfo.vertices[1] = new Vector3(spriteInfo.size.x, spriteInfo.size.y, 0) + textpos2;
            drawSpriteInfo.vertices[2] = new Vector3(spriteInfo.size.x, 0, 0) + textpos2;
            drawSpriteInfo.vertices[3] = new Vector3(0, spriteInfo.size.y, 0) + textpos1;
            drawSpriteInfo.alreadyDraw = false;

            var bounds = new Bounds(textpos1, Vector3.zero);
            bounds.Encapsulate(drawSpriteInfo.vertices[0]);
            bounds.Encapsulate(drawSpriteInfo.vertices[1]);
            bounds.Encapsulate(drawSpriteInfo.vertices[2]);
            bounds.Encapsulate(drawSpriteInfo.vertices[3]);
            spriteInfo.boxes.Clear();
            spriteInfo.boxes.Add(new Rect(bounds.min, bounds.size));

            if (null != spriteInfo.rectTransform) {
                drawSpriteInfo.alreadyDraw = true;

                var rt = spriteInfo.rectTransform;
                var size = rt.sizeDelta;
                rt.anchoredPosition = new Vector2(textpos1.x + size.x / 2, textpos1.y + size.y / 2);
            }
        }
    }
    private void CalcLinkTag(VertexHelper toFill)
    {
        // 处理超链接包围框
        UIVertex vert = new UIVertex();
        foreach (var hrefInfo in m_HrefInfos) {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount) {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++) {
                if (i >= toFill.currentVertCount) {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);

                pos = vert.position;
                if (pos.x < bounds.min.x) { // 换行重新添加包围框
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                } else {
                    bounds.Encapsulate(pos); // 扩展包围框
                }
            }
            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
        }
    }
    private void AddUnderlineQuad(VertexHelper _VToFill, IList<UIVertex> _VTUT, Vector3 _VStartPos, Vector3 _VEndPos)
    {
        Vector3[] _TUnderlinePos = new Vector3[4];
        _TUnderlinePos[0] = _VStartPos;
        _TUnderlinePos[1] = _VEndPos;
        _TUnderlinePos[2] = _VEndPos + new Vector3(0, fontSize * 0.2f, 0);
        _TUnderlinePos[3] = _VStartPos + new Vector3(0, fontSize * 0.2f, 0);

        for (int i = 0; i < 4; ++i) {
            int tempVertsIndex = i & 3;
            m_TempVerts[tempVertsIndex] = _VTUT[i % 4];
            m_TempVerts[tempVertsIndex].color = Color.blue;

            m_TempVerts[tempVertsIndex].position = _TUnderlinePos[i];

            if (tempVertsIndex == 3)
                _VToFill.AddUIVertexQuad(m_TempVerts);
        }
    }

    private void Analyze()
    {
        IRichTextList texts;
        if (null != m_ParsedTexts) {
            texts = m_ParsedTexts;
        } else {
            m_RichTextParser.Parse(text);
            texts = m_RichTextParser.Texts;
        }
        m_TextBuilder.Length = 0;
        
        for (int i = 0; i < m_SpriteInfos.Count; ++i) {
            var spriteInfo = m_SpriteInfos[i];
            if (null != spriteInfo && null != spriteInfo.rectTransform) {
                spriteInfo.rectTransform.SetParent(null);
                GameObject.Destroy(spriteInfo.rectTransform.gameObject);
            }
        }

        m_HrefInfos.Clear();
        m_SpriteInfos.Clear();
        m_DrawSpriteInfos.Clear();

        AnalyzeRichTexts(texts);
    }
    private void AnalyzeRichText(NormalText txt)
    {
        m_TextBuilder.Append(txt.Text);
    }
    private void AnalyzeRichText(HyperText txt)
    {
        if (txt.Attrs.Count >= 1) {
            string id = txt.Attrs[0].Key;
            string val = txt.Attrs[0].Value;
            if (s_SystemColors.Contains(id)) {
                //[{color_name}文本] 
                m_TextBuilder.AppendFormat("<color={0}>", id);
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</color>");
            } else if (id == "c") {
                //[{c}文本] 
                m_TextBuilder.AppendFormat("<color=#{0}>", val);
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</color>");
            } else if (id == "b") {
                //[{b}文本] 
                m_TextBuilder.Append("<b>");
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</b>");
            } else if (id == "i") {
                //[{i}文本] 
                m_TextBuilder.Append("<i>");
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</i>");
            } else if (id == "s") {
                //[{s}文本] 
                m_TextBuilder.AppendFormat("<size={0}>", val);
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</size>");
            } else if (id == "m") {
                //[{m}文本] 
                m_TextBuilder.AppendFormat("<material={0}>", val);
                AnalyzeRichTexts(txt.Texts);
                m_TextBuilder.Append("</material>");
            } else if (id == "#") {
                //[#id]
                BuildLinkOrQuadInfo(id, val, txt);
            } else if (id == "@") {
                //[@id{pos:x,y}]
                BuildLinkOrQuadInfo(id, val, txt);
            } else {
                //[!...]等，其中！还可以为$%&*?\^~-+=|:
                BuildLinkOrQuadInfo(id, val, txt);
            }
        } else {
            AnalyzeRichTexts(txt.Texts);
        }
    }
    private void AnalyzeRichTexts(IList<IRichText> txts)
    {
        for (int ix = 0; ix < txts.Count; ++ix) {
            var txt = txts[ix];
            var normalText = txt as NormalText;
            if (null != normalText) {
                AnalyzeRichText(normalText);
            } else {
                var hyperText = txt as HyperText;
                if (null != hyperText) {
                    AnalyzeRichText(hyperText);
                }
            }
        }
    }
    private void BuildLinkOrQuadInfo(string id, string name, HyperText text)
    {
        LinkInfo linkInfo;
        QuadInfo quadInfo;
        bool isLink = ExecOnBuildLinkOrQuadInfo(id, name, text, out linkInfo, out quadInfo);
        if (null != linkInfo) {
            BuildHrefInfo(text, linkInfo.Name, linkInfo.Color, isLink);
        } else if (null != quadInfo) {
            BuildQuadInfo(text, id, quadInfo.Name, quadInfo.Size, quadInfo.Count, isLink, quadInfo.Sprite, quadInfo.SpriteResource, quadInfo.InstantiatedPrefab, quadInfo.PrefabResource);
        } else {
            if (id == "#") {
                BuildQuadInfo(text, id, name, defaultQuadSize, isLink);
            } else {
                string color;
                if (!s_LinkColors.TryGetValue(id, out color)) {
                    color = "blue";
                }
                string bracketName = string.Format("[{0}]", name);
                BuildHrefInfo(text, bracketName, color, isLink);
            }
        }
    }
    private void BuildQuadInfo(HyperText text, string id, string name, int size, bool isLink)
    {
        BuildQuadInfo(text, id, name, size, 1, isLink, null, string.Empty, null, string.Empty);
    }
    private void BuildQuadInfo(HyperText text, string id, string name, int size, int count, bool isLink, Sprite sprite, string spriteResource, GameObject instantiatedPrefab, string prefabResource)
    {
        int firstIndex = m_TextBuilder.Length;
        int lastIndex = firstIndex;
        List<int> indexes = new List<int>();
        for (int i = 0; i < count; ++i) {
            lastIndex = m_TextBuilder.Length;
            indexes.Add(lastIndex);
            m_TextBuilder.AppendFormat("<quad name={0} size={1} x=0 y=0 width=1 height=1 />", name, size);
        }

        var spriteInfo = new SpriteInfo();
        spriteInfo.name = name;
        spriteInfo.firstIndex = firstIndex;
        spriteInfo.lastIndex = lastIndex;
        spriteInfo.indexes = indexes;
        spriteInfo.size = new Vector2(size, size);
        spriteInfo.text = text;
        spriteInfo.isLink = isLink;
        m_SpriteInfos.Add(spriteInfo);

        if (null != sprite || !string.IsNullOrEmpty(spriteResource)) {
            Image img = null;
            var resources = new DefaultControls.Resources();
            var go = DefaultControls.CreateImage(resources);
            go.layer = gameObject.layer;
            var nrt = go.transform as RectTransform;
            if (null != nrt) {
                nrt.SetParent(rectTransform);
                nrt.localPosition = Vector3.zero;
                nrt.localRotation = Quaternion.identity;
                nrt.localScale = Vector3.one;
                nrt.sizeDelta = new Vector2(size * count, size);
                nrt.anchorMin = new Vector2(0.5f, 0.5f);
                nrt.anchorMax = new Vector2(0.5f, 0.5f);
                nrt.pivot = new Vector2(0.5f, 0.5f);

                img = go.GetComponent<Image>();
                if (null != sprite) {
                    img.sprite = sprite;
                } else if (!string.IsNullOrEmpty(spriteResource)) {
                    img.sprite = Resources.Load<Sprite>(spriteResource);
                }
                img.enabled = true;

                spriteInfo.rectTransform = nrt;
            }
        }
        if (null != instantiatedPrefab || !string.IsNullOrEmpty(prefabResource)) {
            if (null == instantiatedPrefab) {
                var m = Resources.Load<GameObject>(prefabResource);
                instantiatedPrefab = GameObject.Instantiate<GameObject>(m);
            }
            if (null != instantiatedPrefab) {
                instantiatedPrefab.layer = gameObject.layer;
                var nrt = instantiatedPrefab.transform as RectTransform;
                if (null != nrt) {
                    nrt.SetParent(rectTransform);
                    nrt.localPosition = Vector3.zero;
                    nrt.localRotation = Quaternion.identity;
                    nrt.localScale = Vector3.one;
                    nrt.sizeDelta = new Vector2(size * count, size);
                    nrt.anchorMin = new Vector2(0.5f, 0.5f);
                    nrt.anchorMax = new Vector2(0.5f, 0.5f);
                    nrt.pivot = new Vector2(0.5f, 0.5f);

                    spriteInfo.rectTransform = nrt;
                }
            }
        }

        var drawSpriteInfo = new DrawSpriteInfo();
        drawSpriteInfo.id = id;
        drawSpriteInfo.name = name;        
        m_DrawSpriteInfos.Add(drawSpriteInfo);
    }
    private void BuildHrefInfo(HyperText text, string name, string color, bool isLink)
    {
        // 超链接颜色
        m_TextBuilder.AppendFormat("<color={0}>", color);
        var hrefInfo = new HrefInfo {
            startIndex = m_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
            endIndex = (m_TextBuilder.Length + name.Length - 1) * 4 + 3,
            text = text,
            isLink = isLink
        };
        m_HrefInfos.Add(hrefInfo);

        m_TextBuilder.Append(name);
        m_TextBuilder.Append("</color>");
    }
    private bool ExecOnBuildLinkOrQuadInfo(string id, string name, HyperText text, out LinkInfo linkInfo, out QuadInfo quadInfo)
    {
        bool isLink = true;
        if (null != onBuildLinkOrQuadInfo) {
            isLink = onBuildLinkOrQuadInfo(id, name, text, out linkInfo, out quadInfo);
        } else {
            linkInfo = null;
            quadInfo = null;
        }
        return isLink;
    }

    private class HrefInfo
    {
        internal int startIndex;
        internal int endIndex;
        internal List<Rect> boxes = new List<Rect>();
        internal HyperText text;
        internal bool isLink;
    }
    private class SpriteInfo
    {
        internal string name;
        internal int firstIndex;
        internal int lastIndex;
        internal List<int> indexes;
        internal Vector2 size;
        internal List<Rect> boxes = new List<Rect>();
        internal HyperText text;
        internal RectTransform rectTransform;
        internal bool isLink;
    }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    private List<HrefInfo> m_HrefInfos = new List<HrefInfo>();
    private List<SpriteInfo> m_SpriteInfos = new List<SpriteInfo>();
    private List<DrawSpriteInfo> m_DrawSpriteInfos = new List<DrawSpriteInfo>();

    private IRichTextList m_ParsedTexts = null;
    private RichTextParser.RichTextParser m_RichTextParser = new RichTextParser.RichTextParser();
    private StringBuilder m_TextBuilder = new StringBuilder();

    private string m_OutputText;
    private UIVertex[] m_TempVerts = new UIVertex[4];

    public static int defaultQuadSize
    {
        get { return s_DefaultQuadSize; }
        set { s_DefaultQuadSize = value; }
    }
    public static void SetLinkColor(string id, string color)
    {
        if (s_LinkColors.ContainsKey(id)) {
            s_LinkColors[id] = color;
        } else {
            s_LinkColors.Add(id, color);
        }
    }

    private static int s_DefaultQuadSize = 48;
    private static Dictionary<string, string> s_LinkColors = new Dictionary<string, string> {
        { "!", "green" },
        { "?", "cyan" },
        { "$", "lightblue" },
        { "@", "brown" },
        { "&", "orange" },
        { "%", "purple" },
        { "*", "red" },
        { "\\", "blue" }
    };
    private static readonly HashSet<string> s_SystemColors = new HashSet<string> { "aqua", "black", "blue", "brown", "cyan", "darkblue", "fuchsia", "green", "grey", "lightblue", "lime", "magenta", "maroon", "navy", "olive", "orange", "purple", "red", "silver", "teal", "white", "yellow" };
}