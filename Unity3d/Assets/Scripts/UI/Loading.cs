using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image imgBackground;
    public Slider sldProgress;
    
    public void SetBackground(string path)
    {
        imgBackground.sprite = Resources.Load<Sprite>(path);
    }
    public void SetProgress(float value)
    {
        sldProgress.value = value;
    }
    public void SetAsync(AsyncOperation async, float start, float scale)
    {
        m_Async = async;
        m_Start = start;
        m_Scale = scale;
    }
    internal void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    internal void Update()
    {
        if (m_Async != null) {
            sldProgress.value = m_Start + m_Async.progress * m_Scale;
        }
    }


    private AsyncOperation m_Async;
    private float m_Start;
    private float m_Scale;
}
