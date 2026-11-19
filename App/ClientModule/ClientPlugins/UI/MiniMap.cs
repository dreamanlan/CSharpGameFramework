using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using ScriptableFramework;

class MiniMap : ITickPlugin
{
    private const float c_DotSize = 9f;
    private const float c_RefreshInterval = 0.1f;
    private static readonly Color c_PlayerColor = new Color(1f, 0.9f, 0.1f);
    private static readonly Color c_FriendColor = new Color(0.2f, 0.9f, 0.25f);
    private static readonly Color c_EnemyColor = new Color(0.95f, 0.2f, 0.2f);

    public void Init(GameObject obj, MonoBehaviourProxy behaviour)
    {
        var rectTrans = obj.transform as RectTransform;
        m_Root = obj.transform;
        m_RawImage = obj.GetComponent<RawImage>();
        m_MapPlayer = obj.transform.Find("Player");
        m_MapWidth = (int)rectTrans.rect.width;
        m_MapHeight = (int)rectTrans.rect.height;
        var terrain = Terrain.activeTerrain;
        if (null != terrain) {
            m_TerrainWidth = (int)terrain.terrainData.size.x;
            m_TerrainHeight = (int)terrain.terrainData.size.z;
        }
        if (null == s_DotSprite) s_DotSprite = CreateDotSprite();
        // The prefab already has a Player marker; recolour it yellow and only
        // supply a round sprite when it has none, so a designed icon survives.
        if (null != m_MapPlayer) {
            var img = m_MapPlayer.GetComponent<Image>();
            if (null != img) {
                img.color = c_PlayerColor;
                if (null == img.sprite) img.sprite = s_DotSprite;
            }
        }
        m_NextRefresh = 0f;
    }
    public void Update()
    {
        if (null == m_Root) return;
        int leaderId = PluginFramework.Instance.LeaderId;
        if (m_LeaderId != leaderId || null == m_GamePlayer) {
            m_LeaderId = leaderId;
            m_GamePlayer = PluginFramework.Instance.GetGameObject(leaderId);
        }
        if (null != m_GamePlayer && null != m_MapPlayer) {
            PlaceDot(m_MapPlayer as RectTransform, m_GamePlayer.transform.position);
        }
        // Entity dots are throttled: the plugin ticks every frame and the
        // entity list can hold dozens of actors.
        if (Time.time < m_NextRefresh) return;
        m_NextRefresh = Time.time + c_RefreshInterval;
        RefreshEntityDots(leaderId);
    }
    public void Call(string name, params object[] args)
    {
        if (name == "SetImage") {
            string res = args[0] as string;
            var obj = UiResourceSystem.Instance.GetUiResource(res) as Texture2D;
            if (null != obj) {
                m_RawImage.texture = obj;
            }
        }
    }

    private void RefreshEntityDots(int leaderId)
    {
        var alive = new HashSet<int>();
        var leader = PluginFramework.Instance.GetLeaderEntityInfo();
        int camp = null != leader ? leader.GetCampId() : PluginFramework.Instance.CampId;
        var mgr = PluginFramework.Instance.EntityManager;
        if (null == mgr) return;
        foreach (EntityInfo info in mgr.Entities.Values) {
            if (null == info) continue;
            int id = info.GetId();
            if (id == leaderId) continue;
            if (info.IsDead() || !info.IsCombatNpc()) continue;
            CharacterRelation rel = EntityInfo.GetRelation(camp, info.GetCampId());
            if (rel != CharacterRelation.RELATION_FRIEND && rel != CharacterRelation.RELATION_ENEMY) continue;
            var go = PluginFramework.Instance.GetGameObject(id);
            if (null == go) continue;
            alive.Add(id);
            Color color = rel == CharacterRelation.RELATION_FRIEND ? c_FriendColor : c_EnemyColor;
            PlaceDot(EnsureDot(id, color), go.transform.position);
        }
        var stale = new List<int>();
        foreach (var kv in m_Dots) {
            if (!alive.Contains(kv.Key)) stale.Add(kv.Key);
        }
        foreach (int id in stale) {
            var dot = m_Dots[id];
            if (null != dot) UnityEngine.Object.Destroy(dot.gameObject);
            m_Dots.Remove(id);
        }
    }

    private RectTransform EnsureDot(int id, Color color)
    {
        RectTransform rt;
        if (m_Dots.TryGetValue(id, out rt) && null != rt) {
            var image = rt.GetComponent<Image>();
            if (null != image) image.color = color;
            return rt;
        }
        // Init can run more than once (UI reload). Reuse an existing child dot
        // rather than piling up duplicates under the RawImage.
        string dotName = "Dot_" + id;
        var child = m_Root.Find(dotName);
        GameObject go;
        if (null != child) {
            go = child.gameObject;
        } else {
            go = new GameObject(dotName, typeof(RectTransform), typeof(Image));
            var fresh = go.transform as RectTransform;
            fresh.SetParent(m_Root, false);
            fresh.anchorMin = new Vector2(0.5f, 0.5f);
            fresh.anchorMax = new Vector2(0.5f, 0.5f);
            fresh.pivot = new Vector2(0.5f, 0.5f);
            fresh.sizeDelta = new Vector2(c_DotSize, c_DotSize);
            fresh.localScale = Vector3.one;
        }
        rt = go.transform as RectTransform;
        var img = go.GetComponent<Image>();
        if (null != img) {
            img.sprite = s_DotSprite;
            img.color = color;
            img.raycastTarget = false;
        }
        m_Dots[id] = rt;
        return rt;
    }

    // Same mapping as the original player marker: world (x,z) -> minimap
    // localPosition, with the map origin at the RawImage centre. No Y flip.
    private void PlaceDot(RectTransform rt, Vector3 worldPos)
    {
        if (null == rt) return;
        float x = worldPos.x * m_MapWidth / m_TerrainWidth - m_MapWidth / 2;
        float y = worldPos.z * m_MapHeight / m_TerrainHeight - m_MapHeight / 2;
        rt.localPosition = new Vector3(x, y, 0);
    }

    private static Sprite CreateDotSprite()
    {
        const int size = 16;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        var cols = new Color32[size * size];
        float radius = size * 0.5f - 0.5f;
        for (int y = 0; y < size; ++y) {
            for (int x = 0; x < size; ++x) {
                float dx = x - radius, dy = y - radius;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                byte a = (byte)(Mathf.Clamp01(radius + 0.5f - dist) * 255f);
                cols[y * size + x] = new Color32(255, 255, 255, a);
            }
        }
        tex.SetPixels32(cols);
        tex.Apply(false, false);
        var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        sprite.hideFlags = HideFlags.HideAndDontSave;
        return sprite;
    }

    private static Sprite s_DotSprite = null;

    private RawImage m_RawImage = null;
    private Transform m_Root = null;
    private int m_MapWidth = 100;
    private int m_MapHeight = 100;
    private int m_TerrainWidth = 512;
    private int m_TerrainHeight = 512;
    private Transform m_MapPlayer = null;
    private GameObject m_GamePlayer = null;
    private int m_LeaderId = 0;
    private float m_NextRefresh = 0f;
    private readonly Dictionary<int, RectTransform> m_Dots = new Dictionary<int, RectTransform>();
}
