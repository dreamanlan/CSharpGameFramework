using UnityEngine;
using System.Collections;
using GameFramework;
using GameFramework.Story;
using System.Collections.Generic;

public sealed class BattleTopMenuManager
{
    public void Init()
    {
        GameObject obj = ResourceSystem.Instance.NewObject("UI/BattleTopMenu") as GameObject;
        view = obj.GetComponent<BattleTopMenu>();  
     
        view.onSelectAuto = (bool bAuto) => {
            ClientModule.Instance.SetOperateType(bAuto);            
        };
        view.onClickQuit = () => {
            GfxStorySystem.Instance.SendMessage("quit");
        };
        view.onTimeOver = () => {
            GfxStorySystem.Instance.SendMessage("time_over");
        };
        subscribes.Add(Utility.EventSystem.Subscribe("ui_stop_watch", "ui", () => view.StopWatch()));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_show", "ui", Show));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_hide", "ui", Hide));        
        view.SetTotalTime(120);
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
    public bool IsOn(Vector3 pos)
    {
        if (null != view) {
            RectTransform rect = view.toggleAuto.transform as RectTransform;
            if (null != rect) {
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(pos.x, pos.y)))
                    return true;
            }
            RectTransform rect2 = view.btnQuit.transform as RectTransform;
            if (null != rect2) {
                if (RectTransformUtility.RectangleContainsScreenPoint(rect2, new Vector2(pos.x, pos.y)))
                    return true;
            }
        }
        return false;
    }

    private BattleTopMenu view;
    private List<object> subscribes = new List<object>();

    public static BattleTopMenuManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static BattleTopMenuManager s_Instance = new BattleTopMenuManager();
}
