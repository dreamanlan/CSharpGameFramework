using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{ 
    //纹理贴图相关
    class TexturesAnalyzer : BaseAnalyzer
    {
       
        private Vector2 scrollPosTexture = new Vector2(0, 0);
        private ActiveSubType actSubTypeTexture = ActiveSubType.Details;
        private string[] textureToolStrings = { LanguageCfg.DETAILS, LanguageCfg.HELPS, LanguageCfg.SETTINGS };



        public void DrawTextures()
        {
            GUILayout.Space(10);

            actSubTypeTexture = (ActiveSubType)GUILayout.Toolbar((int)actSubTypeTexture, textureToolStrings, GUILayout.MaxWidth(200));

            GUILayout.Space(10);

            scrollPosTexture = EditorGUILayout.BeginScrollView(scrollPosTexture);
            if (actSubTypeTexture == ActiveSubType.Details)
            {

                GUILayout.BeginHorizontal();
                
                GUILayout.Label(string.Format(LanguageCfg.PROJECT_TEXTURES_LIST, ProjectResource.Instance.textures.Count), GUILayout.Width(150));

                if (GUILayout.Button(LanguageCfg.TEXTURE_SETTING_NO_MINMAP, GUILayout.Width(120)))
                {

                    ProjectResource.Instance.SetTextureNoMinmap();
                }

                DrawPageCnt(ProjectResource.Instance.textures.Count);
                
                GUILayout.EndHorizontal();
                GUILayout.Space(10);


                //绘制title
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(LanguageCfg.NAME, GUILayout.Width(150)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.Name);
                   
                }
                if (GUILayout.Button(LanguageCfg.MemorySize, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.MemorySize);
                }
                if (GUILayout.Button(LanguageCfg.PIX_W, GUILayout.Width(50)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.PixWidth);
                }
                if (GUILayout.Button(LanguageCfg.PIX_H, GUILayout.Width(50)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.PixHeigh);
                }
                if (GUILayout.Button(LanguageCfg.IsRW, GUILayout.Width(50)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.IsRW);
                }
                if (GUILayout.Button(LanguageCfg.OverridePlat, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                }
                if (GUILayout.Button(LanguageCfg.Mipmap, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.Mipmap);
                }
                if (GUILayout.Button(LanguageCfg.IsLightmap, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.IsLightmap);
                }
                if (GUILayout.Button(LanguageCfg.AnisoLevel, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.AnisoLevel);
                }

                if (GUILayout.Button(LanguageCfg.PROPOSE, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortTexture(TextureInfo.SortType.Propose);
                }

                GUILayout.EndHorizontal();

                int start = mPageCnt * mCurPage;
                int end =mPageCnt * (mCurPage + 1);
                end = end >= ProjectResource.Instance.textures.Count ? ProjectResource.Instance.textures.Count : end;
                for (int i = start; i < end; i++)
                {   
                    TextureInfo textureInfo = ProjectResource.Instance.textures[i];
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(textureInfo.name, GUILayout.Width(150)))
                    {
                        EditTools.PingAssetInProject(textureInfo.path);
                        //Selection.activeObject = textureInfo.texture;
                    }
                    GUILayout.Space(10);
                    GUILayout.Label(textureInfo.GetSize(), GUILayout.MaxWidth(100));

                    GUILayout.Label(textureInfo.width + "x" + textureInfo.height, GUILayout.MaxWidth(100));
                    GUILayout.Label(textureInfo.isRW.ToString(), GUILayout.MaxWidth(50));

                    GUILayout.Label(textureInfo.IsOverridePlatform(), GUILayout.MaxWidth(100));
                    GUILayout.Label(textureInfo.isMipmap.ToString(), GUILayout.MaxWidth(100));
                    GUILayout.Label(textureInfo.isLightmap.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(textureInfo.anisoLevel.ToString(), GUILayout.MaxWidth(80));
                    DrawProposeTips(textureInfo);
                    GUILayout.EndHorizontal();
                }
            }
            else if (actSubTypeTexture == ActiveSubType.Settings)
            {
                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckMemSize = GUILayout.Toggle(SettingCfgUI.textureCheckMemSize, "检查内存大小", GUILayout.MaxWidth(100));
                SettingCfgUI.textureCheckMemSizeValue = GUILayout.TextField(SettingCfgUI.textureCheckMemSizeValue, GUILayout.MaxWidth(80));
                GUILayout.Label("kb", GUILayout.MaxWidth(20));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckPix = GUILayout.Toggle(SettingCfgUI.textureCheckPix, "检查分辨率", GUILayout.MaxWidth(100));
                SettingCfgUI.textureCheckPixW = GUILayout.TextField(SettingCfgUI.textureCheckPixW, GUILayout.MaxWidth(80));
                GUILayout.Label("x", GUILayout.MaxWidth(20));
                SettingCfgUI.textureCheckPixH = GUILayout.TextField(SettingCfgUI.textureCheckPixH, GUILayout.MaxWidth(80));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckPix2Pow = GUILayout.Toggle(SettingCfgUI.textureCheckPix2Pow, "检查2N次幂", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckIsRW = GUILayout.Toggle(SettingCfgUI.textureCheckIsRW, "检查可读写", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckPlatSetting = GUILayout.Toggle(SettingCfgUI.textureCheckPlatSetting, "检查平台设置", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.textureCheckIsLightmap = GUILayout.Toggle(SettingCfgUI.textureCheckIsLightmap, "检查lightmap格式", GUILayout.MaxWidth(150));
                GUILayout.EndHorizontal();

                if (GUILayout.Button("应用", GUILayout.MaxWidth(100)))
                {
                    SettingCfg.Apply(true);
                    ProjectResource.Instance.ReCheckTextures();
                }
            }
            else
            {
                DrawHelpTips(LanguageCfg.HELP_TEXTURE);
            }
            EditorGUILayout.EndScrollView();
        }

    }
}
