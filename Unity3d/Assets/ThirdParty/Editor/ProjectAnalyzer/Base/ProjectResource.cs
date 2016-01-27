using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    class ProjectResource
    {
        public List<AnimationClip> animClips = new List<AnimationClip>();
        public List<MaterialInfo> materials = new List<MaterialInfo>();
        public List<Mesh> meshes = new List<Mesh>();
        public List<ModelInfo> models = new List<ModelInfo>();
        public List<TextureInfo> textures = new List<TextureInfo>();
        public List<ParticleInfo> particles = new List<ParticleInfo>();


        private volatile static ProjectResource _instance = new ProjectResource();
        private static readonly object lockHelper = new object();


        static public ProjectResource Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new ProjectResource();
                    }
                }
                return _instance;
            }
        }

        public void Init()
        {
            Reset();
            CreateProejectInfo();

            SortTexture(TextureInfo.SortType.Name);
            SortModel(ModelInfo.SortType.Name);
            SortMaterial(MaterialInfo.SortType.Name);
        }

        public void CreateProejectInfo()
        {
            string[] paths = AssetDatabase.GetAllAssetPaths();

            for (int i = 0; i < paths.Length; i++)
            {
                //if (i > 1000)
                //    break;
                UpdateProgress(i, paths.Length);

                string path = paths[i];
                UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset is Material)
                {
                    materials.Add(new MaterialInfo(asset as Material, path));
                    continue;
                }
                else if (asset is Texture)
                {
                    textures.Add(new TextureInfo(asset as Texture, path));
                    continue;
                }
                else if (asset is AudioClip)
                {
                    continue;
                }
                ModelImporter mi = AssetImporter.GetAtPath(path) as ModelImporter;
                if (mi != null)
                {
                    models.Add(new ModelInfo(mi, path));
                    continue;
                }

            }
            EditorUtility.ClearProgressBar();
        }



        public void SortModel(ModelInfo.SortType sortType)
        {
            switch (sortType)
            {
                case ModelInfo.SortType.Name:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo1.name.CompareTo(tInfo2.name); });
                    break;
                case ModelInfo.SortType.Scale:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.scale.CompareTo(tInfo1.scale); });
                    break;
                case ModelInfo.SortType.MeshCompression:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.meshCompression.CompareTo(tInfo1.meshCompression); });
                    break;
                case ModelInfo.SortType.AnimCompression:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.animCompression.CompareTo(tInfo1.animCompression); });
                    break;
                case ModelInfo.SortType.IsRW:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.isRW.CompareTo(tInfo1.isRW); });
                    break;
                case ModelInfo.SortType.Collider:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.isAddCollider.CompareTo(tInfo1.isAddCollider); });
                    break;
                case ModelInfo.SortType.NormalImportMode:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.normalImportMode.CompareTo(tInfo1.normalImportMode); });
                    break;
                case ModelInfo.SortType.TangentImportMode:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.tangentImportMode.CompareTo(tInfo1.tangentImportMode); });
                    break;
                case ModelInfo.SortType.BakeIK:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.isBakeIK.CompareTo(tInfo1.isBakeIK); });
                    break;
                case ModelInfo.SortType.FileSize:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.fileSize.CompareTo(tInfo1.fileSize); });
                    break;
                //case ModelInfo.SortType.Animation:
                //    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.isGenerateAnimation.CompareTo(tInfo1.isGenerateAnimation); });
                //    break;
                case ModelInfo.SortType.SkinnedMeshCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.skinnedMeshCount.CompareTo(tInfo1.skinnedMeshCount); });
                    break;
                case ModelInfo.SortType.MeshFilterCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.meshFilterCount.CompareTo(tInfo1.meshFilterCount); });
                    break;
                case ModelInfo.SortType.AnimationClipCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.animationClipCount.CompareTo(tInfo1.animationClipCount); });
                    break;
                case ModelInfo.SortType.VertexCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.vertexCount.CompareTo(tInfo1.vertexCount); });
                    break;
                case ModelInfo.SortType.TriangleCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.triangleCount.CompareTo(tInfo1.triangleCount); });
                    break;
                case ModelInfo.SortType.BoneCount:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.boneCount.CompareTo(tInfo1.boneCount); });
                    break;
                case ModelInfo.SortType.Propose:
                    models.Sort(delegate(ModelInfo tInfo1, ModelInfo tInfo2) { return tInfo2.proposeTipCount.CompareTo(tInfo1.proposeTipCount); });
                    break;

            }
        }
        public void SortTexture(TextureInfo.SortType sortType)
        {
            switch (sortType)
            {
                case TextureInfo.SortType.Name:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo1.name.CompareTo(tInfo2.name); });
                    break;
                case TextureInfo.SortType.MemorySize:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.size.CompareTo(tInfo1.size); });
                    break;
                case TextureInfo.SortType.PixWidth:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.width.CompareTo(tInfo1.width); });
                    break;
                case TextureInfo.SortType.PixHeigh:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.height.CompareTo(tInfo1.height); });
                    break;
                case TextureInfo.SortType.IsRW:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.isRW.CompareTo(tInfo1.isRW); });
                    break;
                case TextureInfo.SortType.Mipmap:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.isMipmap.CompareTo(tInfo1.isMipmap); });
                    break;
                case TextureInfo.SortType.IsLightmap:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.isLightmap.CompareTo(tInfo1.isLightmap); });
                    break;
                case TextureInfo.SortType.AnisoLevel:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.anisoLevel.CompareTo(tInfo1.anisoLevel); });
                    break;
                case TextureInfo.SortType.Propose:
                    textures.Sort(delegate(TextureInfo tInfo1, TextureInfo tInfo2) { return tInfo2.proposeTipCount.CompareTo(tInfo1.proposeTipCount); });
                    break;
            }
        }
        public void SortMaterial(MaterialInfo.SortType sortType)
        {
            switch (sortType)
            {
                case MaterialInfo.SortType.Name:
                    materials.Sort(delegate(MaterialInfo tInfo1, MaterialInfo tInfo2) { return tInfo1.name.CompareTo(tInfo2.name); });
                    break;
                case MaterialInfo.SortType.ShaderName:
                    materials.Sort(delegate(MaterialInfo tInfo1, MaterialInfo tInfo2) { return tInfo1.shaderName.CompareTo(tInfo2.shaderName); });
                    break;
                case MaterialInfo.SortType.Propose:
                    materials.Sort(delegate(MaterialInfo tInfo1, MaterialInfo tInfo2) { return tInfo2.proposeTipCount.CompareTo(tInfo1.proposeTipCount); });
                    break;
            }
        }
        public void SortParticle(ParticleInfo.SortType sortType)
        {
            switch (sortType)
            {
                case ParticleInfo.SortType.Name:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo1.name.CompareTo(tInfo2.name); });
                    break;
                case ParticleInfo.SortType.ParentIsParticle:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.parentIsParticle.CompareTo(tInfo1.parentIsParticle); });
                    break;
                case ParticleInfo.SortType.ParticleSysCnt:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.particleSysCnt.CompareTo(tInfo1.particleSysCnt); });
                    break;
                case ParticleInfo.SortType.Duration:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.duration.CompareTo(tInfo1.duration); });
                    break;
                case ParticleInfo.SortType.MaxParticles:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.maxParticles.CompareTo(tInfo1.maxParticles); });
                    break;
                case ParticleInfo.SortType.MinParticles:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.minParticles.CompareTo(tInfo1.minParticles); });
                    break;
                case ParticleInfo.SortType.Conllier:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.conllier.CompareTo(tInfo1.conllier); });
                    break;
                case ParticleInfo.SortType.Propose:
                    particles.Sort(delegate(ParticleInfo tInfo1, ParticleInfo tInfo2) { return tInfo2.proposeTipCount.CompareTo(tInfo1.proposeTipCount); });
                    break;
                
            }
        }
        
        public void ReCheckModels()
        {
            if (models == null)
            {
                return;
            }
            for (int i = 0; i < models.Count; i++)
            {
                models[i].CheckValid();
            }
        }
        public void SetModelsMeshCompress()
        {
            if (models == null)
            {
                return;
            }

            string format = "更新：{0}/{1}";
            EditorUtility.DisplayProgressBar("Progress", string.Format(format, 0, models.Count), 0);

            for (int i = 0; i < models.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, i, models.Count), (float)i / (float)models.Count);
                models[i].SetMeshCompress();
            }
            EditorUtility.ClearProgressBar();


        }
        public void SetModelsAnimCompress()
        {
            if (models == null)
            {
                return;
            }
            string format = "更新：{0}/{1}";
            EditorUtility.DisplayProgressBar("Progress", string.Format(format, 0, models.Count), 0);

            for (int i = 0; i < models.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, i, models.Count), (float)i / (float)models.Count);
                models[i].SetAnimCompress();
            }
            EditorUtility.ClearProgressBar();
        }
        public void SetModelsWriteReadClose()
        {
            if (models == null)
            {
                return;
            }
            string format = "更新：{0}/{1}";
            EditorUtility.DisplayProgressBar("Progress", string.Format(format, 0, models.Count), 0);

            for (int i = 0; i < models.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, i, models.Count), (float)i / (float)models.Count);
                models[i].SetReadWriteClose();
            }
            EditorUtility.ClearProgressBar();
        }

        public void SetModelDefault()
        {
            if (models == null)
            {
                return;
            }
            string format = "更新：{0}/{1}";
            EditorUtility.DisplayProgressBar("Progress", string.Format(format, 0, models.Count), 0);

            for (int i = 0; i < models.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, i, models.Count), (float)i / (float)models.Count);
                models[i].SetModelDefault();
            }
            EditorUtility.ClearProgressBar();
        }

        public void SetTextureNoMinmap()
        {
            if (textures == null)
            {
                return;
            }
            string format = "更新：{0}/{1}";
            EditorUtility.DisplayProgressBar("Progress", string.Format(format, 0, textures.Count), 0);

            for (int i = 0; i < textures.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, i, textures.Count), (float)i / (float)textures.Count);
                textures[i].SetTextureNoMinmap();
            }
            EditorUtility.ClearProgressBar();
        } 

        public void ReCheckTextures()
        {
            if (textures == null)
            {
                return;
            }
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i].CheckValid();
            }
        }

        public string ToString()
        {
            string log = "textures size {0}";
            return string.Format(log, textures.Count);
        }


        private void UpdateProgress(int cur, int total)
        {
            if (cur % 10 == 0)
            {
                float progress = (float)cur / (float)total;
                string format = "读取资源：{0}%";
                EditorUtility.DisplayProgressBar("Progress", string.Format(format, (int)(progress * 100)), progress);
            }
        }


        public void Reset()
        {
            animClips.Clear();
            materials.Clear();
            meshes.Clear();
            textures.Clear();
            models.Clear();
            particles.Clear();
        }

        public void CollectParticles()
        {
            particles.Clear();
            string[] paths = AssetDatabase.GetAllAssetPaths();

            for (int i = 0; i < paths.Length; i++)
            {
                UpdateProgress(i, paths.Length);

                string path = paths[i];
                string extName = Path.GetExtension(path).ToLower();
                if (path.Contains("UI"))
                {
                    continue;
                }
                if (extName != ".prefab")
                {
                    continue;
                }
                
                GameObject prefab = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;

                if (prefab == null)
                    continue;
                GameObject go = GameObject.Instantiate(prefab) as GameObject;
                ParticleSystem[] pses = go.GetComponentsInChildren<ParticleSystem>();                
                
                if (pses.Length <= 0)
                {                   
                    GameObject.DestroyImmediate(go, false);                
                    prefab = null;
                    go = null;
                    continue;
                }
                particles.Add(new ParticleInfo(go,path));
                GameObject.DestroyImmediate(go, false);
                prefab = null;
                go = null;
            }
            SortParticle(ParticleInfo.SortType.ParticleSysCnt);

            EditorUtility.ClearProgressBar();
         

        }
        public Transform[] GetTransforms(GameObject parentGameObject)
        {
            if (parentGameObject != null)
            {
                List<Component> components = new List<Component>(parentGameObject.GetComponentsInChildren(typeof(Transform)));
                List<Transform> transforms = components.ConvertAll(c => (Transform)c);

                transforms.Remove(parentGameObject.transform);
                transforms.Sort(delegate(Transform a, Transform b)
                {
                    return a.name.CompareTo(b.name);
                });

                return transforms.ToArray();
            }

            return null;
        }

    }


}
