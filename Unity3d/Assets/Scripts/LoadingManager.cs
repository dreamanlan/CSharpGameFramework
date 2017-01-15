using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;

public sealed class LoadingManager
{
    public void Init()
    {
        subscribes.Add(Utility.EventSystem.Subscribe("loading_complete", "ui", LoadComplete));
        GameObject obj = ResourceSystem.Instance.NewObject("UI/Loading") as GameObject;
        view = obj.GetComponent<Loading>();
        view.gameObject.SetActive(false);
    }
    public void Release()
    {
        if (null != view) {
            ResourceSystem.Instance.RecycleObject(view.gameObject);
            view = null;
        }
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
    }
    public void SetBackground(string path)
    {
        view.SetBackground(path);
    }
    public void SetProgress(float v)
    {
        view.SetProgress(v);
    }
    public void Show()
    {
        view.gameObject.SetActive(true);
        view.SetProgress(0);
    }
    public void SetAsync(AsyncOperation async, float start, float scale)
    {
        view.SetAsync(async, start, scale);
    }
    private void LoadComplete()
    {
        if (null != view) {
            view.SetAsync(null, 0, 0);
            view.gameObject.SetActive(false);
        }
    }

    private Loading view;
    private List<object> subscribes = new List<object>();

    public static LoadingManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static LoadingManager s_Instance = new LoadingManager();
}
