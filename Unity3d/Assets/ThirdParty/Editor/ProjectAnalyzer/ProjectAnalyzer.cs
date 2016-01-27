
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
namespace ProjectAnalyzer
{
    public class ProjectAnalyzer : EditorWindow
    {
        private TexturesAnalyzer texturesAnalyzer = new TexturesAnalyzer();
        private MaterialsAnalyzer materialsAnalyzer = new MaterialsAnalyzer();
        private ModelsAnalyzer modelsAnalyzer = new ModelsAnalyzer();
        private ParticleAnalyzer particleAnalyzer = new ParticleAnalyzer();


        private string[] toolbarStrings = {LanguageCfg.HOME,LanguageCfg.TEXTURES,LanguageCfg.MODELS,LanguageCfg.MATERIALS,LanguageCfg.PARTICLES };
        private ActiveType activeType;      
        

        #region 插件有关的函数
        [MenuItem("ProjectAnalyzer/ProjectAnalyzer")]
        static void ProjectAnalyzerStart()
        {
            ProjectAnalyzer window = (ProjectAnalyzer)EditorWindow.GetWindow(typeof(ProjectAnalyzer));
            window.title = "Analyzer";
            window.minSize = new Vector2(1000, 500);           
        }


        [MenuItem("Assets/AssetView")]
        public static void ProjectAnalyzerView()
        {
            UnityEngine.Object asset = Selection.activeGameObject;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            ResInfo refInfo = null;
            if (asset is Texture)
            {
                refInfo = new TextureInfo(asset as Texture, assetPath);
            }
            else
            {
                ModelImporter mi = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (mi != null)
                {
                    refInfo = new ModelInfo(mi, assetPath);
                }
            }

            if (refInfo != null)
            {
                EditorUtility.DisplayDialog("Tips", refInfo.GetResInfoDetails(), "OK");
            }
        }

        public void Init()
        {
            SettingCfg.Apply(false);
            ProjectResource.Instance.Init();
            Repaint();
        }

        void OnGUI()
        {
            if(GUILayout.Button(LanguageCfg.REFRESH,GUILayout.Width(200)))
            {
                Init();
            }
            activeType = (ActiveType)GUILayout.Toolbar((int)activeType, toolbarStrings);

            switch (activeType)
            {
                case ActiveType.Home:
                    DrawHome();
                    break;
                case ActiveType.Textures:
                    texturesAnalyzer.DrawTextures();
                    break;
                case ActiveType.Models:
                    modelsAnalyzer.DrawModels();
                    break;
                case ActiveType.Materials:
                    materialsAnalyzer.DrawMaterials();
                    break;
                case ActiveType.Particles:
                    particleAnalyzer.DrawParticles();
                    break;
                    
            }
        }


     

        #endregion


        #region 总览
        private Vector2 scrollPosHome = new Vector2(0, 0);
        void DrawHome()
        {
            scrollPosHome = EditorGUILayout.BeginScrollView(scrollPosHome);

            GUILayout.Label(LanguageCfg.REFRESH_TIP, GUILayout.Width(400));
            GUILayout.Space(20);
            GUILayout.Label(LanguageCfg.PROJECT_LIST, GUILayout.Width(400));
            GUILayout.Space(10);
            GUILayout.Label(string.Format(LanguageCfg.PROJECT_TEXTURES_LIST, ProjectResource.Instance.textures.Count), GUILayout.MinWidth(100));
            GUILayout.Label(string.Format(LanguageCfg.PROJECT_MODELS_LIST, ProjectResource.Instance.models.Count), GUILayout.MinWidth(100));
            GUILayout.Label(string.Format(LanguageCfg.PROJECT_MATERIALS_LIST, ProjectResource.Instance.materials.Count), GUILayout.MinWidth(100));

            GUILayout.Space(20);
            GUILayout.Label(LanguageCfg.BALABALALBALA, GUILayout.MinWidth(100));

            EditorGUILayout.EndScrollView();
        }
        #endregion

    }
     
}
