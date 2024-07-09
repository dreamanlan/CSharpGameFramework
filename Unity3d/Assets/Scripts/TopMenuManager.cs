using UnityEngine;
using System.Collections;
using ScriptableFramework;
using ScriptableFramework.Story;
using System.Collections.Generic;

public sealed class TopMenuManager
{
    public void Init()
    {
        GameObject obj = ResourceSystem.Instance.NewObject("UI/TopMenu") as GameObject;
        view = obj.GetComponent<TopMenu>();

        view.onTimeOver = () => {
            GfxStorySystem.Instance.SendMessage("time_over");
        };
        subscribes.Add(Utility.EventSystem.Subscribe<int>("ui_start_timer", "ui", time => view.SetTotalTime(time)));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_stop_timer", "ui", () => view.StopWatch()));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_show_timer", "ui", Show));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_hide_timer", "ui", Hide));
        Hide();
    }
    public void Release()
    {
        if (null != view) {
            ResourceSystem.Instance.RecycleObject(view.gameObject);
        }
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
        subscribes.Clear();
    }
    public void Show()
    {
        view.gameObject.SetActive(true);
    }
    public void Hide()
    {
        view.gameObject.SetActive(false);
    }

    private TopMenu view;
    private List<object> subscribes = new List<object>();

    public static TopMenuManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static TopMenuManager s_Instance = new TopMenuManager();
}
