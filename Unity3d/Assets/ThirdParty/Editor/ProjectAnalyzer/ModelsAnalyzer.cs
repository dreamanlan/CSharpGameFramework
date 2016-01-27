using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    /**
     * 工程模型部分相关
     */
    class ModelsAnalyzer : BaseAnalyzer
    {
        private ActiveSubType actSubTypeModel = ActiveSubType.Details;

        private string[] modelToolStrings = { LanguageCfg.DETAILS, LanguageCfg.HELPS, LanguageCfg.SETTINGS };


        private Vector2 scrollPosModel = new Vector2(0, 0);
        public void DrawModels()
        {

            GUILayout.Space(10);
            actSubTypeModel = (ActiveSubType)GUILayout.Toolbar((int)actSubTypeModel, modelToolStrings, GUILayout.MaxWidth(200));
            GUILayout.Space(10);


            scrollPosModel = EditorGUILayout.BeginScrollView(scrollPosModel);
            if (actSubTypeModel == ActiveSubType.Details)
            {
                GUILayout.BeginHorizontal();
                string info = "Model counts1  {0}";
                GUILayout.Label(string.Format(info, ProjectResource.Instance.models.Count), GUILayout.Width(200));

                if (GUILayout.Button(LanguageCfg.SettingModelDefault, GUILayout.Width(120)))
                {
                    ProjectResource.Instance.SetModelDefault();
                }

                if (GUILayout.Button(LanguageCfg.SettingMeshCompress, GUILayout.Width(120)))
                {
                    ProjectResource.Instance.SetModelsMeshCompress();
                }
                if (GUILayout.Button(LanguageCfg.SettingAnimCompress, GUILayout.Width(120)))
                {
                    ProjectResource.Instance.SetModelsAnimCompress();
                }

                if (GUILayout.Button(LanguageCfg.SettingWriteClose, GUILayout.Width(120)))
                {
                    ProjectResource.Instance.SetModelsWriteReadClose();
                }

                DrawPageCnt(ProjectResource.Instance.models.Count);

                GUILayout.EndHorizontal();
                GUILayout.Space(10);



                //绘制title
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(LanguageCfg.NAME, GUILayout.Width(150)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.Name);
                }
                if (GUILayout.Button(LanguageCfg.Scale, GUILayout.Width(50)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.Scale);
                }
                if (GUILayout.Button(LanguageCfg.MeshCompress, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.MeshCompression);
                }
                if (GUILayout.Button(LanguageCfg.AnimCompress, GUILayout.Width(180)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.AnimCompression);
                }
                if (GUILayout.Button(LanguageCfg.AnimCnt, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.AnimationClipCount);
                }
                if (GUILayout.Button(LanguageCfg.IsRW, GUILayout.Width(50)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.IsRW);
                }
                if (GUILayout.Button(LanguageCfg.Collider, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.Collider);
                }
                if (GUILayout.Button(LanguageCfg.NormalMode, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.NormalImportMode);
                }
                if (GUILayout.Button(LanguageCfg.TangentMode, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.TangentImportMode);
                }
                if (GUILayout.Button(LanguageCfg.BakeIK, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.BakeIK);
                }
                if (GUILayout.Button(LanguageCfg.FileSize, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.FileSize);
                }
                if (GUILayout.Button(LanguageCfg.SkinnedMeshCnt, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.SkinnedMeshCount);
                }

                if (GUILayout.Button(LanguageCfg.MeshFilterCnt, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.MeshFilterCount);
                }

                if (GUILayout.Button(LanguageCfg.VertexCnt, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.VertexCount);
                }
                if (GUILayout.Button(LanguageCfg.TriangleCnt, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.TriangleCount);
                }
                if (GUILayout.Button(LanguageCfg.BoneCnt, GUILayout.Width(80)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.BoneCount);
                }

                if (GUILayout.Button(LanguageCfg.PROPOSE, GUILayout.Width(100)))
                {
                    mCurPage = 0;
                    ProjectResource.Instance.SortModel(ModelInfo.SortType.Propose);
                }

                GUILayout.EndHorizontal();

                 int start = mPageCnt * mCurPage;
                 int end = mPageCnt * (mCurPage + 1);
                 end = end >= ProjectResource.Instance.models.Count ? ProjectResource.Instance.models.Count : end;
                 for (int i = start; i < end; i++)
                 {
                    ModelInfo modelInfo = ProjectResource.Instance.models[i];
                    GUILayout.BeginHorizontal();


                    if (GUILayout.Button(modelInfo.name, GUILayout.Width(150)))
                    {
                        EditTools.PingAssetInProject(modelInfo.path);
                    }
                    GUILayout.Space(10);
                    GUILayout.Label(modelInfo.scale.ToString(), GUILayout.MaxWidth(50));
                    GUILayout.Label(modelInfo.meshCompression.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.animCompression.ToString(), GUILayout.MaxWidth(180));
                    GUILayout.Label(modelInfo.animationClipCount.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.isRW.ToString(), GUILayout.MaxWidth(50));
                    GUILayout.Label(modelInfo.isAddCollider.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.normalImportMode.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.tangentImportMode.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.isBakeIK.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.GetFileLenth(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.skinnedMeshCount.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.meshFilterCount.ToString(), GUILayout.MaxWidth(100));

                    GUILayout.Label(modelInfo.vertexCount.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.triangleCount.ToString(), GUILayout.MaxWidth(80));
                    GUILayout.Label(modelInfo.boneCount.ToString(), GUILayout.MaxWidth(80));

                    DrawProposeTips(modelInfo);
                    GUILayout.EndHorizontal();
                }
            }
            else if (actSubTypeModel == ActiveSubType.Settings)
            {
                GUILayout.BeginHorizontal();
                SettingCfgUI.modelCheckScale = GUILayout.Toggle(SettingCfgUI.modelCheckScale, "检查Scale属性", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                SettingCfgUI.modelCheckMeshCompression = GUILayout.Toggle(SettingCfgUI.modelCheckMeshCompression, "检查Mesh压缩", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.modelCheckAnimCompression = GUILayout.Toggle(SettingCfgUI.modelCheckAnimCompression, "检查动画压缩", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.modelCheckMeshIsRW = GUILayout.Toggle(SettingCfgUI.modelCheckMeshIsRW, "检查可读写", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                SettingCfgUI.modelCheckCollider = GUILayout.Toggle(SettingCfgUI.modelCheckCollider, "检查是否生成碰撞器", GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal();
                //SettingCfgUI.modelCheckNormals = GUILayout.Toggle(SettingCfgUI.modelCheckNormals, "检查是否存在法线", GUILayout.MaxWidth(100));
                //GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal();
                //SettingCfgUI.modelCheckTangents = GUILayout.Toggle(SettingCfgUI.modelCheckTangents, "检查是否存在切线", GUILayout.MaxWidth(100));
                //GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal();
                //SettingCfgUI.modelCheckFileSize = GUILayout.Toggle(SettingCfgUI.modelCheckFileSize, "检查文件大小", GUILayout.MaxWidth(100));
                //SettingCfgUI.modelCheckFileSizeValue = GUILayout.TextField(SettingCfgUI.modelCheckFileSizeValue, GUILayout.MaxWidth(80));
                //GUILayout.Label("kb", GUILayout.MaxWidth(20));
                //GUILayout.EndHorizontal();
                if (GUILayout.Button("应用", GUILayout.MaxWidth(100)))
                {
                    SettingCfg.Apply(true);
                    ProjectResource.Instance.ReCheckModels();
                }
            }
            else
            {
                DrawHelpTips(LanguageCfg.HELP_MODEL);
            }

            EditorGUILayout.EndScrollView();
        }

    }
}
