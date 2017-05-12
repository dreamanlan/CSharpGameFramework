using UnityEngine;
using System.Collections.Generic;
using GameFramework;

public class ScreenFading : MonoBehaviour
{
    //Color
    public Color TargetColor;
    public Color AddColor;
    public Shader MatShader;

    private Material m_Material;
    private GameObject m_Plane;
    private Mesh m_Mesh;

    private Vector3[] m_Vertices = new Vector3[4];

    private bool m_IsActive = false;
    private bool m_NeedDeactive = false;
    private float m_TotalTime = 0;
    private float m_StartTime = 0;
    private float m_ElapseTime = 0;
    private Color m_StartColor = Color.black;
    private Color m_EndColor = Color.black;
    private Color m_StartAddColor = Color.black;
    private Color m_EndAddColor = Color.black;
    //Properties
    protected Material material
    {
        get
        {
            if (m_Material == null) {
                m_Material = new Material(MatShader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }
    //Methods
    private void Update()
    {
        try {
            if (m_IsActive) {
                m_ElapseTime += Time.deltaTime / Time.timeScale;
                if (m_ElapseTime < m_TotalTime) {
                    TargetColor = Color.Lerp(m_StartColor, m_EndColor, m_ElapseTime / m_TotalTime);
                    AddColor = Color.Lerp(m_StartAddColor, m_EndAddColor, m_ElapseTime / m_TotalTime);
                } else {
                    m_NeedDeactive = true;
                    TargetColor = m_EndColor;
                }
            }
        } catch (System.Exception ex) {
            LogSystem.Error("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    protected void OnDisable()
    {
        if (m_Plane) {
            Destroy(m_Plane);
            m_Plane = null;
            m_Mesh = null;
        }
        if (m_Material) {
            DestroyImmediate(m_Material);
            m_Material = null;
        }
    }

    void OnEnable()
    {
        MatShader = Shader.Find("Hidden/Custom/FadeSimple");
        Camera fadeCamera = gameObject.GetComponent<Camera>();
        if (null == fadeCamera || !fadeCamera.enabled) {
            enabled = false;
            return;
        }
        if (null == m_Plane) {
            GameObject obj = new GameObject();
            obj.name = "Plane_" + fadeCamera.name;
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();

            m_Vertices[0] = fadeCamera.ViewportToWorldPoint(new Vector3(-0.2f, -0.2f, fadeCamera.nearClipPlane + 0.1f));
            m_Vertices[1] = fadeCamera.ViewportToWorldPoint(new Vector3(-0.2f, 1.2f, fadeCamera.nearClipPlane + 0.1f));
            m_Vertices[2] = fadeCamera.ViewportToWorldPoint(new Vector3(1.2f, 1.2f, fadeCamera.nearClipPlane + 0.1f));
            m_Vertices[3] = fadeCamera.ViewportToWorldPoint(new Vector3(1.2f, -0.2f, fadeCamera.nearClipPlane + 0.1f));
            ///*
            for (int i = 0; i < 4; ++i) {
                m_Vertices[i] = gameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(m_Vertices[i]);
            }
            //*/
            Vector2[] uvs = new Vector2[] { Vector2.zero, Vector2.zero };
            int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            mesh.vertices = m_Vertices;
            //mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.name = obj.name;

            meshFilter.mesh = mesh;
            renderer.sharedMaterial = material;
            material.SetColor("_Color", TargetColor);

            ///*
            Transform t = obj.transform;
            t.parent = fadeCamera.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            //*/

            obj.SetActive(true);

            m_Plane = obj;
            m_Mesh = mesh;
        }
    }

    void OnPreRender()
    {
        if (m_IsActive) {
            try {
                material.SetColor("_Color", TargetColor);
                material.SetColor("_AddColor", AddColor);
            } catch (System.Exception ex) {
                LogSystem.Error("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            if (m_NeedDeactive) {
                m_IsActive = false;
                m_NeedDeactive = false;
            }
        }
    }

    // message
    void DimScreen(long time)
    {
        m_IsActive = true;
        m_NeedDeactive = false;
        m_TotalTime = time / 1000.0f;
        m_StartTime = Time.time;
        m_StartColor = Color.white;
        m_EndColor = Color.black;
        m_ElapseTime = 0.0f;
    }
    void LightScreen(long time)
    {
        m_IsActive = true;
        m_NeedDeactive = false;
        m_TotalTime = time / 1000.0f;
        m_StartTime = Time.time;
        m_StartColor = Color.black;
        m_EndColor = Color.white;
        m_ElapseTime = 0.0f;
    }

    void LightTo(object[] args)
    {
        if (args.Length >= 5) {
            float[] param = new float[args.Length];
            for (int i = 0; i < args.Length; ++i) {
                param[i] = (float)System.Convert.ChangeType(args[i], typeof(float));
            }
            m_IsActive = true;
            m_NeedDeactive = false;
            m_TotalTime = param[0] / 1000.0f;
            m_StartTime = Time.time;
            m_StartColor = TargetColor;
            m_StartAddColor = AddColor;
            m_ElapseTime = 0.0f;

            float r = Mathf.Clamp01(param[1]);
            float g = Mathf.Clamp01(param[2]);
            float b = Mathf.Clamp01(param[3]);
            float a = Mathf.Clamp01(param[4]);
            float r2 = Mathf.Clamp01(param[1] - 1);
            float g2 = Mathf.Clamp01(param[2] - 1);
            float b2 = Mathf.Clamp01(param[3] - 1);
            float a2 = Mathf.Clamp01(param[4] - 1);
            m_EndColor = new Color(r, g, b, a);
            m_EndAddColor = new Color(r2, g2, b2, a2);
        }
    }
}

