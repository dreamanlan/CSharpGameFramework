using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    public class ModelInfo : ResInfo
    {
        public enum SortType
        {
            Name,
            Scale,
            MeshCompression,
            AnimCompression,
            IsRW,
            Collider,
            NormalImportMode,
            TangentImportMode,
            BakeIK,
            FileSize,
            Animation,
            SkinnedMeshCount,
            MeshFilterCount,
            AnimationClipCount,
            VertexCount,
            TriangleCount,
            BoneCount,
            Propose
        }

        public float scale;
        public ModelImporterMeshCompression meshCompression;
        public ModelImporterAnimationCompression animCompression;
        public bool isRW;
        public bool isAddCollider;
        public bool swapUVChannels;
        public ModelImporterTangentSpaceMode normalImportMode;
        public ModelImporterTangentSpaceMode tangentImportMode;
        public bool isBakeIK;
        public string filePath;

        public int skinnedMeshCount;
        public int meshFilterCount;
        public int animationClipCount;
        public int vertexCount;
        public int triangleCount;

        public int boneCount;
        private ModelImporter mi;
        public ModelInfo(ModelImporter mi, string path)
        {
            //获取模型数据
            this.mi = mi;

            this.path = path;
            AnalyzerModel();

        }
        private void AnalyzerModel()
        {
            GameObject asset = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
            this.name = asset.name;
            this.scale = mi.globalScale;
            this.meshCompression = mi.meshCompression;
            this.isRW = mi.isReadable;
            this.isAddCollider = mi.addCollider;
            this.swapUVChannels = mi.swapUVChannels;
            this.normalImportMode = mi.normalImportMode;
            this.tangentImportMode = mi.tangentImportMode;
            this.isBakeIK = mi.bakeIK;
            this.animCompression = mi.animationCompression;

            if (!mi.importAnimation || mi.animationType == ModelImporterAnimationType.None)
            {
                this.animationClipCount = 0;
            }
            else
            {
                this.animationClipCount = mi.clipAnimations.Length;
            }

            CollectMeshInfo(asset);

            CheckValid();
        }

        override
        public string GetRefInfos()
        {
            string info = "";
            info += LanguageCfg.Scale + ":" + this.scale.ToString() + "\n";
            info += LanguageCfg.MeshCompress + ":" + this.meshCompression.ToString() + "\n";
            info += LanguageCfg.AnimCompress + ":" + this.animCompression.ToString() + "\n";
            info += LanguageCfg.IsRW + ":" + this.isRW.ToString() + "\n";
            info += LanguageCfg.Collider + ":" + this.isAddCollider.ToString() + "\n";
            info += LanguageCfg.NormalMode + ":" + this.normalImportMode.ToString() + "\n";
            info += LanguageCfg.TangentMode + ":" + this.tangentImportMode.ToString() + "\n";
            info += LanguageCfg.BakeIK + ":" + this.isBakeIK.ToString() + "\n";
            info += LanguageCfg.SkinnedMeshCnt + ":" + this.skinnedMeshCount.ToString() + "\n";
            info += LanguageCfg.AnimCnt + ":" + this.animationClipCount.ToString() + "\n";
            info += LanguageCfg.VertexCnt + ":" + this.vertexCount.ToString() + "\n";
            info += LanguageCfg.TriangleCnt + ":" + this.triangleCount.ToString() + "\n";
            info += LanguageCfg.BoneCnt + ":" + this.boneCount.ToString() + "\n\n\n";
            return info;

        }

        public void SetMeshCompress()
        {
            if (this.mi.meshCompression != ModelImporterMeshCompression.Medium)
            {
                this.mi.meshCompression = ModelImporterMeshCompression.Medium;
                AssetDatabase.ImportAsset(path);
                AnalyzerModel();
            }
        }
        public void SetAnimCompress()
        {
            if (this.mi.animationCompression != ModelImporterAnimationCompression.KeyframeReductionAndCompression)
            {
                this.mi.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
                AssetDatabase.ImportAsset(path);
                AnalyzerModel();
            }           
        }
        public void SetReadWriteClose()
        {
            if (this.mi.isReadable != false)
            {
                mi.isReadable = false;
                AssetDatabase.ImportAsset(path);
                AnalyzerModel();
            }
        }

        public void SetModelDefault()
        {
            bool flag = false;
            if (this.mi.meshCompression != ModelImporterMeshCompression.Medium)
            {
                this.mi.meshCompression = ModelImporterMeshCompression.Medium;
                flag = true;
              
            }
            if (this.mi.animationCompression != ModelImporterAnimationCompression.KeyframeReductionAndCompression)
            {
                this.mi.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
                flag = true;
            }    
            if (this.mi.isReadable != false)
            {
                mi.isReadable = false;
                flag = true;
            }
            if (flag)
            {
                AssetDatabase.ImportAsset(path);
                AnalyzerModel();
            }
        }


        public void CheckValid()
        {
            ResetProposeTip();
            //检测模型
            SettingCfg setting = SettingCfg.instance;
            if (setting.modelCheckScale)
            {
                if (this.scale != 1)
                {
                    AddProposeTip("建议模型的scale为1");
                }
            }
            if (setting.modelCheckMeshCompression)
            {
                if (this.meshCompression == ModelImporterMeshCompression.Off)
                {
                    AddProposeTip("建议模型采用压缩格式");
                }
            }

            if (setting.modelCheckAnimCompression)
            {
                if (this.animCompression == ModelImporterAnimationCompression.Off ||
                    this.animCompression == ModelImporterAnimationCompression.KeyframeReduction)
                {
                    if (this.animationClipCount > 0)
                    {
                        AddProposeTip("建议动画采用压缩KeyframeReductionAndCompression格式");
                    }
                }
            }


            if (setting.modelCheckMeshIsRW)
            {
                if (this.isRW)
                {
                    AddProposeTip("建议将非可读写的模型读写操作关掉");
                }
            }
            if (setting.modelCheckCollider)
            {
                if (this.isAddCollider)
                {
                    AddProposeTip("建议检查下当前模型确实需要导入Collider");
                }
            }
            //if (this.animationClipCount > 0)
            //{
            //    AddProposeTip("建议检查当前模型是否需要导入animation");
            //}
        }
        private void CollectAnimationInfo(GameObject modelObject)
        {
            Animation legacyAnimation = null;
            legacyAnimation = modelObject.GetComponent<Animation>();
            if (legacyAnimation == null)
            {
                return;
            }
            animationClipCount = legacyAnimation.GetClipCount();
        }

        private void CollectMeshInfo(GameObject modelObject)
        {
            List<SkinnedMeshRenderer> _skinnedMeshes = FindAllSkinnedMesh(modelObject);
            List<MeshFilter> _meshFilters = FindAllMeshFilter(modelObject);

            foreach (SkinnedMeshRenderer _skin in _skinnedMeshes)
            {
                vertexCount += _skin.sharedMesh.vertexCount;
                triangleCount += _skin.sharedMesh.triangles.Length / 3;
                Transform[] bones = _skin.bones;
                boneCount += bones.Length;
            }
            foreach (MeshFilter _filter in _meshFilters)
            {
                vertexCount += _filter.sharedMesh.vertexCount;
                triangleCount += _filter.sharedMesh.triangles.Length / 3;
            }
            skinnedMeshCount = _skinnedMeshes.Count;
            meshFilterCount = _meshFilters.Count;
        }


        public static List<SkinnedMeshRenderer> FindAllSkinnedMesh(GameObject modelObject)
        {
            List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();
            SkinnedMeshRenderer skin = null;

            skin = modelObject.GetComponent<SkinnedMeshRenderer>();
            if (skin != null)
            {
                skinnedMeshes.Add(skin);
            }
            for (int i = 0; i < modelObject.transform.childCount; i++)
            {
                skin = modelObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>();
                if (skin != null)
                {
                    skinnedMeshes.Add(skin);
                }
            }
            return skinnedMeshes;
        }

        public static List<MeshFilter> FindAllMeshFilter(GameObject modelObject)
        {
            List<MeshFilter> meshFilters = new List<MeshFilter>();
            MeshFilter filter = null;

            filter = modelObject.GetComponent<MeshFilter>();
            if (filter != null)
            {
                meshFilters.Add(filter);
            }
            for (int i = 0; i < modelObject.transform.childCount; i++)
            {
                filter = modelObject.transform.GetChild(i).GetComponent<MeshFilter>();
                if (filter != null)
                {
                    meshFilters.Add(filter);
                }
            }
            return meshFilters;
        }

    }
}
