using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using ScriptableFramework;

class MiniMap : ITickPlugin
{
    public void Init(GameObject obj, MonoBehaviourProxy behaviour)
    {
        var rectTrans = obj.transform as RectTransform;
        m_RawImage = obj.GetComponent<RawImage>();
        m_MapPlayer = obj.transform.Find("Player");
        m_MapWidth = (int)rectTrans.rect.width;
        m_MapHeight = (int)rectTrans.rect.height;
        m_TerrainWidth = (int)Terrain.activeTerrain.terrainData.size.x;
        m_TerrainHeight = (int)Terrain.activeTerrain.terrainData.size.z;
    }
    public void Update()
    {
        if (null == m_GamePlayer) {
            m_GamePlayer = PluginFramework.Instance.GetGameObject(PluginFramework.Instance.LeaderId);
        } else {
            var pos = m_GamePlayer.transform.position;
            float x = pos.x * m_MapWidth / m_TerrainWidth - m_MapWidth / 2;
            float y = pos.z * m_MapHeight / m_TerrainHeight - m_MapHeight / 2;

            var rect = m_MapPlayer.transform as RectTransform;
            rect.localPosition = new Vector3(x, y, 0);
        }
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

    private RawImage m_RawImage = null;
    private int m_MapWidth = 100;
    private int m_MapHeight = 100;
    private int m_TerrainWidth = 512;
    private int m_TerrainHeight = 512;
    private Transform m_MapPlayer = null;
    private GameObject m_GamePlayer = null;
}
