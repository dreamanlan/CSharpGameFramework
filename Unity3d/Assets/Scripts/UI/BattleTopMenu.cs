using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleTopMenu : MonoBehaviour
{
    public Toggle toggleAuto;
    public Button btnQuit;
    public Text txtTime;
    public delegate void OnSelectAuto(bool bAuto);
    public OnSelectAuto onSelectAuto;
    public delegate void OnClickButton();
    public OnClickButton onClickQuit;
    public OnClickButton onTimeOver;

    private float timeToEnd;
    private bool isWatchStoped;
    public void Start()
    {
        UIEventTrigger.Get(toggleAuto).onClick = () => {
            if (onSelectAuto != null) {
                onSelectAuto(toggleAuto.isOn);
            }
        };
        UIEventTrigger.Get(btnQuit).onClick = () =>{
            if(onClickQuit != null){
                onClickQuit();
            }
        };
    }
    public void SetTotalTime(int seconds)
    {
        timeToEnd = Time.time + seconds;
        txtTime.rectTransform.localScale = Vector3.one;
        isWatchStoped = false;
    }

    public void Update()
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
    public void StopWatch()
    {
        isWatchStoped = true;
        txtTime.rectTransform.localScale = Vector3.one;
    }
}