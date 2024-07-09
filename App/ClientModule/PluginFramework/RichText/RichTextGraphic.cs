using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

public sealed class RichTextGraphic : MaskableGraphic
{
    public RichTextAsset SpriteAsset;
    public int AnimSpriteNum = 8;

    public override Texture mainTexture {
        get {
            if (SpriteAsset == null)
                return s_WhiteTexture;

            if (SpriteAsset.texSource == null) {
                return s_WhiteTexture;
            } else {
                return SpriteAsset.texSource;
            }
        }
    }
    public override Material material {
        get {
            if (Application.isPlaying) {
                return base.material;
            } else {
                if (null == s_EditModeMaterial) {
                    var mat = new Material(Shader.Find("UI/Unlit/Transparent"));
                    mat.color = new Color(0, 0, 0, 0);
                    s_EditModeMaterial = mat;
                }
                return s_EditModeMaterial;
            }
        }
        set {
            base.material = value;
        }
    }

	void ReleaseMat()
	{
		if (s_EditModeMaterial != null)
		{
			Utility.DestroyObjectFull(s_EditModeMaterial);
			s_EditModeMaterial = null;
		}
	}

	protected override void OnDestroy()
	{
		ReleaseMat();
		base.OnDestroy();
	}

	internal void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= 0.2f) {
            m_Time = 0.0f;
            //刷新一次 更新绘制图片的相关信息
            //Collect();

            for (int i = 0; i < m_AnimSpriteInfor.Count; i++) {
                if (m_Index >= m_AnimSpriteInfor[i].Length) {
                    m_ListSprite[i] = m_AnimSpriteInfor[i][0];
                } else {
                    m_ListSprite[i] = m_AnimSpriteInfor[i][m_Index];
                }
            }
            SetAllDirty();
            m_Index++;
            if (m_Index >= AnimSpriteNum) {
                m_Index = 0;
            }
        }
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Draw(vh);
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif

    public void RmoveSpriteInfo( )
    {

        m_ListSprite.Clear();
        m_AnimSpriteInfor.Clear();
    }
    public void CalulateUVforRichText(RichText richText)
    {
        bool find = false;
        for (int i = 0; i < m_AnimSpriteInfor.Count; i++)
        {
          
            if(m_AnimSpriteInfor[i].Length > 0 && m_AnimSpriteInfor[i][0].text == richText) //youhua

            {
                
                var drawSpriteInfos = richText.drawSpriteInfos;
                if (drawSpriteInfos != null)
                {
                    for (int j = 0; j < drawSpriteInfos.Count; j++)
                    {
                        var drawSpriteInfo = drawSpriteInfos[j];
                        if (drawSpriteInfo.alreadyDraw)
                            continue;

                        //var spriteInfos = new SpriteInfo[tempListName.Count];
                        for (int ix = 0; ix < m_AnimSpriteInfor[i].Length; ix++)
                        {
                            //spriteInfos[ix] = new SpriteInfo();

                            int vertCount = drawSpriteInfo.vertices.Length;
                            m_AnimSpriteInfor[i][ix].vertices = new Vector3[vertCount];
                            for (int vix = 0; vix < vertCount; ++vix)
                            {
                                m_AnimSpriteInfor[i][ix].vertices[vix] = drawSpriteInfo.vertices[vix] + transform.InverseTransformPoint(richText.transform.position);
                            }

                        }

                    }
                    
                }



                find = true;
                break;
                
            }


        }
        if(!find)
        {
            var drawSpriteInfos = richText.drawSpriteInfos;
            if (drawSpriteInfos != null)
            {
                for (int j = 0; j < drawSpriteInfos.Count; j++)
                {
                    var drawSpriteInfo = drawSpriteInfos[j];
                    if (drawSpriteInfo.alreadyDraw)
                        continue;
                    List<string> tempListName = new List<string>();
                    for (int k = 0; k < SpriteAsset.spriteList.Count; k++)
                    {
                        if (SpriteAsset.spriteList[k].name.Contains(drawSpriteInfo.name))
                        {
                            tempListName.Add(SpriteAsset.spriteList[k].name);
                        }
                    }
                    if (tempListName.Count > 0)
                    {
                        var spriteInfos = new SpriteInfo[tempListName.Count];
                        for (int ix = 0; ix < tempListName.Count; ix++)
                        {
                            spriteInfos[ix] = new SpriteInfo();

                            int vertCount = drawSpriteInfo.vertices.Length;
                            spriteInfos[ix].vertices = new Vector3[vertCount];
                            for (int vix = 0; vix < vertCount; ++vix)
                            {
                                spriteInfos[ix].vertices[vix] = drawSpriteInfo.vertices[vix] + transform.InverseTransformPoint(richText.transform.position);
                            }

                            //计算其uv
                            Rect newSpriteRect = SpriteAsset.spriteList[0].rect;
                            for (int m = 0; m < SpriteAsset.spriteList.Count; m++)
                            {
                                //通过标签的名称去索引spriteAsset里所对应的sprite的名称
                                if (tempListName[ix] == SpriteAsset.spriteList[m].name)
                                    newSpriteRect = SpriteAsset.spriteList[m].rect;
                            }
                            Vector2 newTexSize = new Vector2(SpriteAsset.texSource.width, SpriteAsset.texSource.height);

                            spriteInfos[ix].uv = new Vector2[4];
                            spriteInfos[ix].uv[0] = new Vector2(newSpriteRect.x / newTexSize.x, newSpriteRect.y / newTexSize.y);
                            spriteInfos[ix].uv[1] = new Vector2((newSpriteRect.x + newSpriteRect.width) / newTexSize.x, (newSpriteRect.y + newSpriteRect.height) / newTexSize.y);
                            spriteInfos[ix].uv[2] = new Vector2((newSpriteRect.x + newSpriteRect.width) / newTexSize.x, newSpriteRect.y / newTexSize.y);
                            spriteInfos[ix].uv[3] = new Vector2(newSpriteRect.x / newTexSize.x, (newSpriteRect.y + newSpriteRect.height) / newTexSize.y);

                            spriteInfos[ix].triangles = new int[6];
                            spriteInfos[ix].text = richText;
                        }
                        m_AnimSpriteInfor.Add(spriteInfos);
                        m_ListSprite.Add(spriteInfos[0]);
                    }
                }
            }

        }
        //SetAllDirty();

    }

//     private void Collect()
//     {
//         m_ListSprite.Clear();
//         m_AnimSpriteInfor.Clear();
// 
//         var parent = transform.parent;
//         while (parent.childCount == 1) {
//             parent = parent.parent;
//         }
// 
//         RichText[] allRichTexts = parent.GetComponentsInChildren<RichText>();
//         for (int i = 0; i < allRichTexts.Length; i++) {
//             var richText = allRichTexts[i];
//             if (!richText.isActiveAndEnabled)
//                 continue;
//             var drawSpriteInfos = richText.drawSpriteInfos;
//             if (drawSpriteInfos != null) {
//                 for (int j = 0; j < drawSpriteInfos.Count; j++) {
//                     var drawSpriteInfo = drawSpriteInfos[j];
//                     if (drawSpriteInfo.alreadyDraw)
//                         continue;
//                     List<string> tempListName = new List<string>();
//                     for (int k = 0; k < SpriteAsset.spriteList.Count; k++) {
//                         if (SpriteAsset.spriteList[k].name.Contains(drawSpriteInfo.name)) {
//                             tempListName.Add(SpriteAsset.spriteList[k].name);
//                         }
//                     }
//                     if (tempListName.Count > 0) {
//                         var spriteInfos = new SpriteInfo[tempListName.Count];
//                         for (int ix = 0; ix < tempListName.Count; ix++) {
//                             spriteInfos[ix] = new SpriteInfo();
// 
//                             int vertCount = drawSpriteInfo.vertices.Length;
//                             spriteInfos[ix].vertices = new Vector3[vertCount];
//                             for (int vix = 0; vix < vertCount; ++vix) {
//                                 spriteInfos[ix].vertices[vix] = drawSpriteInfo.vertices[vix] + transform.InverseTransformPoint(richText.transform.position);
//                             }
// 
//                             //计算其uv
//                             Rect newSpriteRect = SpriteAsset.spriteList[0].rect;
//                             for (int m = 0; m < SpriteAsset.spriteList.Count; m++) {
//                                 //通过标签的名称去索引spriteAsset里所对应的sprite的名称
//                                 if (tempListName[ix] == SpriteAsset.spriteList[m].name)
//                                     newSpriteRect = SpriteAsset.spriteList[m].rect;
//                             }
//                             Vector2 newTexSize = new Vector2(SpriteAsset.texSource.width, SpriteAsset.texSource.height);
// 
//                             spriteInfos[ix].uv = new Vector2[4];
//                             spriteInfos[ix].uv[0] = new Vector2(newSpriteRect.x / newTexSize.x, newSpriteRect.y / newTexSize.y);
//                             spriteInfos[ix].uv[1] = new Vector2((newSpriteRect.x + newSpriteRect.width) / newTexSize.x, (newSpriteRect.y + newSpriteRect.height) / newTexSize.y);
//                             spriteInfos[ix].uv[2] = new Vector2((newSpriteRect.x + newSpriteRect.width) / newTexSize.x, newSpriteRect.y / newTexSize.y);
//                             spriteInfos[ix].uv[3] = new Vector2(newSpriteRect.x / newTexSize.x, (newSpriteRect.y + newSpriteRect.height) / newTexSize.y);
// 
//                             spriteInfos[ix].triangles = new int[6];
//                         }
//                         m_AnimSpriteInfor.Add(spriteInfos);
//                         m_ListSprite.Add(spriteInfos[0]);
//                     }
//                 }
//             }
//         }
//     }
    private void Draw(VertexHelper vh)
    {
        vh.Clear();
        Color32 color = this.color;
        int triangleVertCount = 0;
        for (int i = 0; i < m_ListSprite.Count; i++) {
            for (int j = 0; j < m_ListSprite[i].vertices.Length && j < m_ListSprite[i].uv.Length; j++) {
                vh.AddVert(m_ListSprite[i].vertices[j], color, m_ListSprite[i].uv[j]);
            }
            for (int j = 0; j < m_ListSprite[i].triangles.Length; j++) {
                ++triangleVertCount;
            }
        }
        //计算顶点绘制顺序
        for (int i = 0; i < triangleVertCount; i++) {
            if (i % 6 == 0) {
                int num = i / 6;
                int t1 = 0 + 4 * num;
                int t2 = 1 + 4 * num;
                int t3 = 2 + 4 * num;

                int t4 = 1 + 4 * num;
                int t5 = 0 + 4 * num;
                int t6 = 3 + 4 * num;

                vh.AddTriangle(t1, t2, t3);
                vh.AddTriangle(t4, t5, t6);
            }
        }
    }

    public class SpriteInfo
    {
        // 4 顶点 
        internal Vector3[] vertices;
        //4 uv
        internal Vector2[] uv;
        //6 三角顶点顺序
        internal int[] triangles;

        public RichText text;
    }

    private float m_Time = 0.0f;
    private int m_Index = 0;
    private List<SpriteInfo> m_ListSprite = new List<SpriteInfo>();
    private List<SpriteInfo[]> m_AnimSpriteInfor = new List<SpriteInfo[]>();
    
    private static Material s_EditModeMaterial = null;
}
