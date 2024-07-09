using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Skill;

public sealed class OverHeadBarManager
{
    public void Init()
    {
        subscribes.Add(Utility.EventSystem.Subscribe<int, bool, TableConfig.Actor>("ui_add_actor", "ui", AddActor));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float>("ui_actor_hp", "ui", ChangeHp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float, int>("ui_actor_mp", "ui", ChangeMp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, int>("ui_show_hp_num", "ui", ShowHpNum));
        subscribes.Add(Utility.EventSystem.Subscribe<int>("ui_remove_actor", "ui", RemoveActor));
        subscribes.Add(Utility.EventSystem.Subscribe<int, bool>("ui_actor_color", "ui", ChangeColor));
        subscribes.Add(Utility.EventSystem.Subscribe<int>("ui_drop", "ui", Drop));
        subscribes.Add(Utility.EventSystem.Subscribe<int, string>("ui_show_paopao", "ui", ShowPaoPao));
        subscribes.Add(Utility.EventSystem.Subscribe<int>("ui_hide_paopao", "ui", HidePaoPao));
    }
    public void Release()
    {
        foreach (OverHeadBar view in views.Values) {
            ResourceSystem.Instance.RecycleObject(view);
        }
        views.Clear();
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
        subscribes.Clear();
    }
    private void UpdateVisible(int actorID)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            bool bVisibleForHp = view.GetHealth() > 0.0001f;
            view.gameObject.SetActive(bVisibleForHp);
        }
    }
    private void AddActor(int actorID, bool isGreen, TableConfig.Actor cfg)
    {
        GameObject obj = PluginFramework.Instance.GetGameObject(actorID);
        GameObject headObj = Utility.FindChildObject(obj, "bloodbar");
        if (obj != null && headObj != null) {

            GameObject overheadBar = Utility.FindChildObject(obj, "OverHeadBar(Clone)");
            if (overheadBar == null) {
                overheadBar = (GameObject)ResourceSystem.Instance.NewObject("UI/OverHeadBar");
            }

            overheadBar.SetActive(true);
            overheadBar.transform.SetParent(obj.transform);
            //overheadBar.transform.localPosition = headObj.transform.position - obj.transform.position;
            overheadBar.transform.position = headObj.transform.position;
            OverHeadBar view = overheadBar.GetComponent<OverHeadBar>();
            view.Reset();

            Color green = new Color(35f / 255f, 192f / 255f, 25f / 255f);
            Color red = new Color(172f / 255f, 45f / 255f, 26f / 255f);

            view.SetHealthColor(isGreen);
            views[actorID] = view;
            view.SetHealth(1);
            float mp = PluginFramework.Instance.GetNpcMp(actorID);
            view.SetMp(mp);
        }
    }
    private void ChangeHp(int actorID, float hp)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            view.SetHealth(hp);
            if (hp == 0) {
                UpdateVisible(actorID);
            }
        }
    }
    private void ChangeMp(int actorID, float mp, int mpChange)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            view.SetMp(mp);
        }
    }
    private void ChangeColor(int actorID, bool isGreen)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            view.SetHealthColor(isGreen);
        }
    }
    private void ShowHpNum(int actorID, int num)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            if (num > 0) {
                view.ShowFloatNum(num, Color.green, 1.0f);
            } else {
                if (PluginFramework.Instance.GetCampId(actorID) == PluginFramework.Instance.CampId) {
                    view.ShowFloatNum(num, Color.red, 1.0f);
                } else {
                    view.ShowFloatNum(num, Color.white, 1.0f);
                }

            }
        }
    }
    private void Drop(int actorID)
    {
        // remove hp bar
        RemoveActor(actorID);
    }
    private void RemoveActor(int actorID)
    {
        if (views.ContainsKey(actorID)) {
            ResourceSystem.Instance.RecycleObject(views[actorID].gameObject);
            //GameObject.Destroy(views[actorID].gameObject);
            views.Remove(actorID);
        }
    }
    private void ShowPaoPao(int actorID, string text)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            view.ShowPaoPao(text);
        }
    }
    private void HidePaoPao(int actorID)
    {
        OverHeadBar view;
        if (views.TryGetValue(actorID, out view)) {
            view.HidePaoPao();
        }
    }

    private List<object> subscribes = new List<object>();
    private Dictionary<int, OverHeadBar> views = new Dictionary<int, OverHeadBar>();

    public static OverHeadBarManager Instance
    { 
        get 
        {
            return s_Instance;
        }
    }
    private static OverHeadBarManager s_Instance = new OverHeadBarManager();
}
