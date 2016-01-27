using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

public sealed class HighlightPromptManager
{
    public void Init()
    {
        m_Canvas = ResourceSystem.Instance.NewObject("UI/HighlightPrompt") as GameObject;

        m_Subscriber = Utility.EventSystem.Subscribe<string>("ui_highlight_prompt", "ui", ShowPrompt);
    }
    public void Release()
    {
        if (null != m_Canvas) {
            ResourceSystem.Instance.RecycleObject(m_Canvas);
            m_Canvas = null;
        }
        if (null != m_Subscriber) {
            Utility.EventSystem.Unsubscribe(m_Subscriber);
        }
    }
    public void Update()
    {
        if (null != m_Canvas && m_Messages.Count > 0) {
            float curTime = Time.time;
            if (m_LastShowTime + 0.1f < curTime) {
                m_LastShowTime = curTime;

                GameObject obj = ResourceSystem.Instance.NewObject("UI/HighlightText", 3.0f) as GameObject;
                Text ui = obj.GetComponent<Text>();
                if (null != ui) {
                    string text = m_Messages.Dequeue();
                    ui.text = text;
                    ui.color = new Color(1, 1, 0, 1);
                    obj.transform.SetParent(m_Canvas.transform, false);
                    RectTransform rect = ui.transform as RectTransform;
                    if (null != rect) {
                        rect.anchoredPosition = new Vector2(0, 150);
                    }                 
                }
                Animation anim = obj.GetComponent<Animation>();
                if (null != anim) {
                    //anim.Rewind();
                    anim.Play();
                }
            }
        }
    }
    private void ShowPrompt(string text)
    {
        m_Messages.Enqueue(text);
    }

    private GameObject m_Canvas = null;
    private object m_Subscriber = null;
    private Queue<string> m_Messages = new Queue<string>();
    private float m_LastShowTime = 0;

    public static HighlightPromptManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static HighlightPromptManager s_Instance = new HighlightPromptManager();
}
