using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameFramework;

public class TopMenu : MonoBehaviour
{
    public Text txtTime;
    public delegate void OnClickButton();
    public OnClickButton onTimeOver;

    public void SetTotalTime(int seconds)
    {
        gameObject.SetActive(true);
        timeToEnd = Time.time + seconds;
        txtTime.rectTransform.localScale = Vector3.one;
        isWatchStoped = false;
    }
    public void StopWatch()
    {
        isWatchStoped = true;
        txtTime.rectTransform.localScale = Vector3.one;
    }

    internal void Start()
    {}
    internal void Update()
    {
        if (!isWatchStoped) {
            if (timeToEnd > Time.time) {
                float timeRemain = timeToEnd - Time.time;
                int num = (int)timeRemain;
                if (num >= 60) {
                    txtTime.text = string.Format("{0}:{1:00}", num / 60, num % 60);
                }
                else {
                    txtTime.text = ((int)timeRemain).ToString();
                }

                if (timeRemain < 10) {
                    float t = timeRemain - ((int)timeRemain);
                    txtTime.rectTransform.localScale = Vector3.one * Mathf.Lerp(1, 2, t * t * t);
                }
            }
            else {
                if (onTimeOver != null) {
                    onTimeOver();
                    StopWatch();
                }
            }
        }
    }
    
    private object subscriber;
    private float timeToEnd;
    private bool isWatchStoped;
}