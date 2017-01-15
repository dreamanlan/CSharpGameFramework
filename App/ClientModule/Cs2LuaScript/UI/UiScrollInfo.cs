using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

class UiScrollInfo : ITickPlugin
{
    public void Init(GameObject obj, MonoBehaviour behaviour)
    {
        m_GameObject = obj;
        m_Item = Resources.Load("UI/Image");
        m_RectTransform = m_GameObject.transform as RectTransform;
        var viewObj = m_RectTransform.parent.parent.gameObject;
        m_ScrollRect = viewObj.GetComponent<ScrollRect>();
        var rectTrans = m_RectTransform.parent.parent as RectTransform;
        m_Top = 0;
        m_Width = rectTrans.rect.width;
        m_Height = m_RectTransform.rect.height;
    }
    public void Update()
    {
        float time=Time.time;
        if (m_ScrollToEndTime + 1 >= time) {
            m_ScrollRect.verticalScrollbar.value = 0;
        }
    }
    public void Call(string name, params object[] args)
    {
        if (name == "PushInfo") {
            string info = args[0] as string;
            if (null != info) {
                PushInfo(info);
            }
        }
    }
    private void PushInfo(string text)
    {
        if (null != m_Item) {
            var item = GameObject.Instantiate(m_Item) as GameObject;
            var t = item.transform.Find("Text");
            if (null != t) {
                item.transform.SetParent(m_GameObject.transform, false);

                var bkg = item.GetComponent<Image>();
                var rectTrans = bkg.transform as RectTransform;
                var content = t.gameObject.GetComponent<Text>();
                var rectTrans2 = t.gameObject.transform as RectTransform;
                float dw = 16;
                float dh = 6;
                if (null != content) {
                    float w, h;
                    CalcBounds(text, content, new Vector2(rectTrans.rect.width, rectTrans.rect.height), m_Width - dw, out w, out h);
                    content.text = text;
                    rectTrans.localPosition = new Vector3(0, m_Top, 0);
                    rectTrans.sizeDelta = new Vector2(m_Width - dw, h + dh);
                    m_Top -= h + dh;

                    m_RectTransform.localPosition = new Vector3(0, 0, 0);
                    m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, dh - m_Top);                    
                }
            }
        }
        m_ScrollRect.verticalScrollbar.value = 0;
        m_ScrollToEndTime = Time.time;
    }

    private bool CalcBounds(string text, Text content, Vector2 size, float maxWidth, out float width, out float height)
    {
        bool ret = false;
        size.x = maxWidth;
        width = 0;
        height = 0;
        var lines = text.Split('\n');
        int lineCt = lines.Length;
        for (int i = 0; i < lineCt; ++i) {
            string txt = lines[i].Trim('\r');
            var textSetting = content.GetGenerationSettings(size);
            textSetting.verticalOverflow = VerticalWrapMode.Overflow;
            textSetting.horizontalOverflow = HorizontalWrapMode.Wrap;
            textSetting.updateBounds = true;
            var generator = new TextGenerator();
            float pw = generator.GetPreferredWidth(txt, textSetting);
            if (generator.Populate(txt, textSetting)) {
                float w = generator.rectExtents.width;
                float h = generator.rectExtents.height;

                if (w > pw) {
                    w = pw;
                }

                if (width < w)
                    width = w;

                height += h;
                ret = true;
            }
        }
        return ret;
    }

    private Object m_Item = null;
    private GameObject m_GameObject = null;
    private RectTransform m_RectTransform = null;
    private ScrollRect m_ScrollRect = null;
    private float m_Top = 0;
    private float m_Width = 0;
    private float m_Height = 2048;
    private float m_ScrollToEndTime = 0;
}
