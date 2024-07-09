using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ScriptableFramework
{
    public partial class EntityViewModel
    {
        public GameObject Actor
        {
            get { return m_Actor; }
        }

        public int ObjId
        {
            get { return m_ObjId; }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = UpdateVisible(value);
            }
        }

        protected void SetVisible(bool visible)
        {
            if (null != m_Actor) {
                ResourceSystem.Instance.SetVisible(m_Actor, true, null);
            }
        }

        protected void CreateActor(int objId, string model, float x, float y, float z, float dir, float scale, float radius, float speed)
        {
            m_ObjId = objId;
            m_Actor = ResourceSystem.Instance.NewObject(model) as GameObject;
            if (null != m_Actor) {
                m_Actor.transform.position = new Vector3(x, y, z);
                m_Actor.transform.localRotation = Quaternion.Euler(0, Utility.RadianToDegree(dir), 0);
                m_Actor.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        protected void DestroyActor()
        {
            if (null != m_Actor) {
                ResourceSystem.Instance.RecycleObject(m_Actor);
                m_Actor = null;
            }
        }

        public void ToggleMaskEffect(bool enable) 
        {
            GameObject obj = m_Actor;
            if (m_Materials.Count <= 0)
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                if (null != renderers)
                {
                    for (int i = 0; i < renderers.Length; ++i)
                    {
                        Material mat = renderers[i].material;
                        m_Shaders.Add(mat.shader);
                        if (mat.HasProperty("_Color"))
                        {
                            m_Colors.Add(mat.color);
                        }
                        else
                        {
                            m_Colors.Add(Color.white);
                        }
                        m_Materials.Add(mat);
                    }
                }
            }

            if (m_Materials.Count > 0)
            {
                for (int i = 0; i < m_Materials.Count; ++i)//
                //Now that the particles are tied to the bones, all materials cannot be modified directly.
                //Otherwise, when hit, the invisible state will be entered, and the texture of the hit
                //particles will also be changed, forming a black patch.
                {
                    Material mat = m_Materials[i];
                    if (mat.shader.name == "LOLL/Toon/toonwithStencil")
                    {
                        if (enable)
                        {
                            mat.SetInt("_StencilMask", 2);
                        }
                        else 
                        {
                            mat.SetInt("_StencilMask", 0);
                        }
                       
                    }
                }
            }
        }

        /// <summary>
        /// Invisibility effect switch
        /// </summary>
        public void Hide(bool hide,bool bNPC = false)
        {
            if ((m_isHidden && hide) || (!m_isHidden && !hide))
                return;
            GameObject obj = m_Actor;
            //obj.layer = 1 << LayerMask.NameToLayer("DeActive"); //(!hide);

            if (!hide)
            {
                
                int ct = m_Materials.Count;
                if (m_Shaders.Count != ct || m_Colors.Count != ct)
                {
                    ScriptableFramework.LogSystem.Error("replaceshaderandfadecolor materials num is not equal shaders num !");
                    return;
                }
                for (int i = 0; i < ct; ++i)
                {
                    m_Materials[i].shader = m_Shaders[i];
                    if (m_Materials[i].HasProperty("_Color"))
                    {
                        m_Materials[i].color = m_Colors[i];
                    }
                }
                m_Materials.Clear();
                m_Shaders.Clear();
                m_Colors.Clear();
                m_isHidden = false;
            }
            else
            {
                if (m_Materials.Count <= 0)
                {
                    Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                    if (null != renderers)
                    {
                        for (int i = 0; i < renderers.Length; ++i)
                        {
                            Material mat = renderers[i].material;
                            m_Shaders.Add(mat.shader);
                            if (mat.HasProperty("_Color"))
                            {
                                m_Colors.Add(mat.color);
                            }
                            else
                            {
                                m_Colors.Add(Color.white);
                            }
                            m_Materials.Add(mat);
                        }
                    }
                }
                if (m_Materials.Count > 0)
                {
                    for (int i = 0; i <m_Materials.Count ; ++i)//
                    //Now that the particles are tied to the bones, all materials cannot be modified directly.
                    //Otherwise, when hit, the invisible state will be entered, and the texture of the hit
                    //particles will also be changed, forming a black patch.
                    {
                        Material mat = m_Materials[i];
                        if (mat.shader.name == "Tut/Shader/Toon/toon" || mat.shader.name == "Mobile/Diffuse"||mat.shader.name == "LOLL/Toon/toonwithStencil") 
                        { }
                        else 
                        {
                            continue;
                        }

                        mat.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                        if (bNPC) 
                        {
                            mat.color = new Color(1, 1, 1, 0.0f);
                        }
                        else 
                        {
                            mat.color = new Color(1, 1, 1, 0.3f);
                        }
                       
                    }
                }
                m_isHidden = true;
            }
        }


        public void SetRedEdge(float duration)
        {
            startRedEdgeColorTime = Time.time;
            timeToStopRedEdge = Time.time + duration;
            isRedEdgeShowing = true;
        }

        public void UpdateEdgeColor()
        {
            if (isRedEdgeShowing) {
                float t = (Time.time - startRedEdgeColorTime)/(timeToStopRedEdge - startRedEdgeColorTime);
                if (t >= 1) {
                    t = 1;
                    isRedEdgeShowing = false;
                }
                GameObject obj = m_Actor;
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                if (null != renderers) {
                    for (int i = 0; i < renderers.Length; ++i) {
                        Material mat = renderers[i].material;
                        //if (mat.HasProperty("_Color")) {
                        //    mat.SetColor("_Color", Color.Lerp(redLineColor, defaultEdgeColor, t));
                        //}

                        if (mat.HasProperty("_RimColor")) {
                            mat.SetColor("_RimColor", Color.Lerp(redRimColor, defaultEdgeColor, t));
                        }
                        if (mat.HasProperty("_LineColor")) {
                            mat.SetColor("_LineColor", Color.Lerp(redLineColor, defaultEdgeColor, t));
                        }
                    }
                }
            } 
        }

        protected int m_ObjId = 0;
        protected bool m_Visible = true;
        protected GameObject m_Actor = null;
        private List<Material> m_Materials = new List<Material>();
        private List<Shader> m_Shaders = new List<Shader>();
        private List<Color> m_Colors = new List<Color>();
        private bool m_isHidden = false;

        private readonly Color defaultEdgeColor = new Color(0,0,0);
        private readonly Color redRimColor = new Color(255.0f / 255, 0 / 255, 0 / 255);
        private readonly Color redLineColor = new Color(169.0f / 255, 0 / 255, 0 / 255);
        private float startRedEdgeColorTime;
        private float timeToStopRedEdge;
        private bool isRedEdgeShowing;
    }
}

