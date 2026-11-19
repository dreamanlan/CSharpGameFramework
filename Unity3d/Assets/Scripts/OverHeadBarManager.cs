using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Skill;

public sealed class OverHeadBarManager
{
    public void Init()
    {
        // sceneLoaded fires for Additive battle scenes too. Subscribing again
        // would duplicate every handler (one AddActor call per subscription)
        // while views still holds the previous battle's bars.
        if (subscribes.Count > 0) return;
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

    private void DestroyBar(OverHeadBar view)
    {
        if (null == view) return;
        // ResourceSystem tracks pooled objects by the GameObject's instance id
        // (AddToUsedResources), so the component itself can never be recycled.
        GameObject go = view.gameObject;
        if (!ResourceSystem.Instance.RecycleObject(go)) {
            // The pool ledger may already have been cleared by ChangeScene(),
            // or the bar came from FindChildObject on an actor prefab. Either
            // way it must not survive as an orphan across a scene switch.
            GameObject.Destroy(go);
        }
    }

    public void Release()
    {
        foreach (OverHeadBar view in views.Values) {
            DestroyBar(view);
        }
        views.Clear();
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
        subscribes.Clear();
    }

    /// <summary>
    /// Drop bars whose owning entity is gone. Additive battle unloading does
    /// not destroy them: actors live in the active/base scene and pooled bars
    /// live under the DontDestroyOnLoad ResPool root. Called on battle unload
    /// so bars from a finished battle do not pile up in the next one.
    /// </summary>
    public int PurgeOrphans()
    {
        var dead = new List<int>();
        foreach (var pair in views) {
            OverHeadBar view = pair.Value;
            if (null == view || PluginFramework.Instance.GetGameObject(pair.Key) == null)
                dead.Add(pair.Key);
        }
        foreach (int id in dead) {
            OverHeadBar view;
            if (views.TryGetValue(id, out view)) DestroyBar(view);
            views.Remove(id);
        }
        return dead.Count;
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
        if (obj == null) return;

        // Avatar prefabs come from AssetBundles and do not carry the legacy
        // "bloodbar" anchor. Previously a missing anchor silently skipped the
        // whole HP bar, so nobody (players or mobs) had any health display.
        // Derive a head-top anchor from the renderers instead.
        GameObject headObj = Utility.FindChildObject(obj, "bloodbar");
        Vector3 anchor;
        if (headObj != null) {
            anchor = headObj.transform.position;
        } else {
            Renderer[] rs = obj.GetComponentsInChildren<Renderer>(true);
            Bounds bounds = default(Bounds);
            bool hasBounds = false;
            foreach (Renderer r in rs) {
                if (r == null) continue;
                if (!hasBounds) { bounds = r.bounds; hasBounds = true; }
                else bounds.Encapsulate(r.bounds);
            }
            if (hasBounds) {
                Vector3 p = obj.transform.position;
                anchor = new Vector3(p.x, bounds.max.y + 0.4f, p.z);
            } else {
                anchor = obj.transform.position + Vector3.up * 2f;
            }
        }

        {
            GameObject overheadBar = Utility.FindChildObject(obj, "OverHeadBar(Clone)");
            if (overheadBar == null) {
                overheadBar = (GameObject)ResourceSystem.Instance.NewObject("UI/OverHeadBar");
            }
            if (overheadBar == null) return;

            overheadBar.SetActive(true);
            overheadBar.transform.SetParent(obj.transform);
            overheadBar.transform.position = anchor;
            OverHeadBar view = overheadBar.GetComponent<OverHeadBar>();
            if (view == null) return;
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
            DestroyBar(views[actorID]);
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
