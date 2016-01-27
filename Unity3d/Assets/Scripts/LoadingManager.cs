using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;

public sealed class LoadingManager
{
    public void Init()
    {
        subscribes.Add(Utility.EventSystem.Subscribe("loading_complete", "ui", LoadComplete));
    }
    public void Release()
    {
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
    }
    public void Show()
    {
        GameObject obj = ResourceSystem.Instance.NewObject("UI/Loading") as GameObject;
        view = obj.GetComponent<Loading>();
        view.SetProgress(0);
    }
    public void SetAsync(AsyncOperation async)
    {
        this.async = async;
    }
    public void Update()
    {
        if (async != null) {
            view.SetProgress(async.progress);
        }
    }
    private void LoadComplete()
    {
        if (null != view) {
            ResourceSystem.Instance.RecycleObject(view.gameObject);
        }
        view = null;
        async = null;
    }

    private Loading view;
    private AsyncOperation async;
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
