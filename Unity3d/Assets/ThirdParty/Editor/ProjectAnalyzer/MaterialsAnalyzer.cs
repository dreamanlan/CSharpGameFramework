using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    class MaterialsAnalyzer : BaseAnalyzer
    {
        #region 材质球部分

        private Vector2 scrollPosMaterial = new Vector2(0, 0);
        public void DrawMaterials()
        {
            scrollPosMaterial = EditorGUILayout.BeginScrollView(scrollPosMaterial);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            string info = "Materials counts  {0}";
            GUILayout.Label(string.Format(info, ProjectResource.Instance.materials.Count), GUILayout.Width(100));

            DrawPageCnt(ProjectResource.Instance.materials.Count);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
          
            //绘制title
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(LanguageCfg.NAME, GUILayout.Width(150)))
            {
                mCurPage = 0;
                ProjectResource.Instance.SortMaterial(MaterialInfo.SortType.Name);
            }
            if (GUILayout.Button(LanguageCfg.ShaderName, GUILayout.Width(500)))
            {
                mCurPage = 0;
                ProjectResource.Instance.SortMaterial(MaterialInfo.SortType.ShaderName);
            }
            if (GUILayout.Button(LanguageCfg.PROPOSE, GUILayout.Width(100)))
            {
                mCurPage = 0;
                ProjectResource.Instance.SortMaterial(MaterialInfo.SortType.Propose);
            }

            GUILayout.EndHorizontal();


            int start = mPageCnt * mCurPage;
            int end = mPageCnt * (mCurPage + 1);
            end = end >= ProjectResource.Instance.materials.Count ? ProjectResource.Instance.materials.Count : end;
            for (int i = start; i < end; i++)
            {
                MaterialInfo materialInfo = ProjectResource.Instance.materials[i];
                GUILayout.BeginHorizontal();


                if (GUILayout.Button(materialInfo.name, GUILayout.Width(150)))
                {
                    EditTools.PingAssetInProject(materialInfo.path);
                }
                GUILayout.Space(10);
                GUILayout.Label(materialInfo.shaderName.ToString(), GUILayout.MaxWidth(500));
                DrawProposeTips(materialInfo);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

        }
        #endregion

    }
}
