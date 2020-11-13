using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Reflection;
using System.Linq.Expressions;
using System.IO;
using System.Text;

public enum AssetProcessorEnum
{
    CorrectNoneAlphaTexture,
    SetAndroidASTC,
	SetIPhoneASTC,
    SetAndroidSceneASTC,
    SetIPhoneSceneASTC,
    SetCompressed,
	SetUncompressed,
	SetReadableTrue,
	SetReadableFalse,
    SetMipMapTrue,
    SetMipMapFalse,
    SetMipMapFilterBox,
    SetMipMapFilterKaiser,
    SetStreamingMipMapsTrue,
    SetStreamingMipMapsFalse,
    SetMipMapBiasOne,
    SetMipMapBiasZero,
    SetLowDetailTrue,
    SetLowDetailFalse,
    SetAndroidMaxSize1024,
    SetIPhoneMaxSize1024,
    SetAndroidMaxSize512,
    SetIPhoneMaxSize512,
    SetFilterPoint,
    SetFilterBilinear,
    SetFilterTrilinear,
    SetAnisoLevel4,
    SetAnisoLevelN1,
    SetNpotScaleNearest,
    SetAndroidOverrideFalse,
    SetIPhoneOverrideFalse,
    SetAnimatorCullCompletely,
    SetAnimatorAlwaysAnimate,
    SetAnimationCompressOptimal,
    SetAnimationCompressDefault,
    SetMeshReadableTrue,
    SetMeshReadableFalse,
    SetMeshOptimizeGameObjectsTrue,
    SetMeshOptimizeGameObjectsFalse,
    SetMeshOptimizeTrue,
    SetMeshOptimizeFalse,
    SetMeshCompressMedium,
    SetMeshCompressOff,
    SetMeshImportMaterialsFalse,
    ClearAnimationScaleCurve,

    SetDirty,
    SaveAndReimport
}

[Serializable]
public class AssetProcessorInfo
{
    [SerializeField]
    public string ResPath;
    [SerializeField]
    public string DslPath;

    [NonSerialized]
    internal List<string> Filters = new List<string>();
    [NonSerialized]
    internal List<AssetProcessorEnum> Processes = new List<AssetProcessorEnum>();

    internal void PrepareFiltersAndProcesses()
    {
        if (Filters.Count <= 0 && Processes.Count <= 0) {
            List<string> processes = new List<string>();
            ResourceProcessor.ReadFiltersAndAssetProcessors(DslPath, Filters, processes);
            var enumNames = Enum.GetNames(typeof(AssetProcessorEnum));
            var enumValues = Enum.GetValues(typeof(AssetProcessorEnum));
            foreach (string processor in processes) {
                int ix = Array.IndexOf<string>(enumNames, processor);
                if (ix >= 0) {
                    var e = (AssetProcessorEnum)enumValues.GetValue(ix);
                    Processes.Add(e);
                }
            }
        }
    }
}

[Serializable]
public class AssetProcessorDB : ScriptableObject
{
	public static string PATH = "Assets/Editor/ResourceDBProcesser/DB/AssetProcessorDB.asset";
	public static string IPHONE = "iPhone";
	public static string ANDROID = "Android";
    
    public int Count
    {
        get { return m_Processers.Count; }
    }
    public AssetProcessorInfo this[int index]
    {
        get {
            if (index >= 0 && index < m_Processers.Count) {
                return m_Processers[index];
            } else {
                return null;
            }
        }
    }
    public List<AssetProcessorInfo> Processers
    {
        get { return m_Processers; }
    }
    public void Clear()
    {
        m_Processers.Clear();
    }
    public void Add(AssetProcessorInfo info)
    {
        m_Processers.Add(info);
    }
    public void Remove(AssetProcessorInfo info)
    {
        m_Processers.Remove(info);
    }
    public void RemoveAt(int index)
    {
        if (index >= 0 && index< m_Processers.Count) {
            m_Processers.RemoveAt(index);
        }
    }
    public void Save()
    {
        EditorUtility.SetDirty(DBInstance);
        AssetDatabase.SaveAssets();
    }

    [SerializeField]
    private List<AssetProcessorInfo> m_Processers = new List<AssetProcessorInfo>();

    [MenuItem("工具/AssetTool/CreateAssetImporterProcesserDB", false, 100)]
	public static void CreateAsset()
	{
		AssetProcessorDB param = ScriptableObject.CreateInstance<AssetProcessorDB>();
		ProjectWindowUtil.CreateAsset(param, PATH);
	}

	[MenuItem("工具/AssetTool/设置Asset导入属性", false, 200)]
	public static void SetAllAssetInDB()
    {
        int totalModifiedCount = 1;
        while (totalModifiedCount > 0) {
            //自动处理3次，还没完就询问是否继续
            for (int ct = 0; ct < 3; ++ct) {
                totalModifiedCount = SetAllAssetInDbOnce(0);
                if (totalModifiedCount <= 0)
                    break;
            }
            if (totalModifiedCount > 0) {
                if(!EditorUtility.DisplayDialog("确认", string.Format("处理了{0}个资源，有可能还需要继续处理，在点继续前可以保存一下工程，等一会再继续。", totalModifiedCount), "继续", "取消")) {
                    break;
                }
            }
        }
    }

    [MenuItem("工具/AssetTool/设置场景资源导入属性", false, 200)]
    public static void SetSceneAssetInDB()
    {
        int totalModifiedCount = 1;
        while (totalModifiedCount > 0) {
            //自动处理3次，还没完就询问是否继续
            for (int ct = 0; ct < 3; ++ct) {
                totalModifiedCount = SetAllAssetInDbOnce(1);
                if (totalModifiedCount <= 0)
                    break;
            }
            if (totalModifiedCount > 0) {
                if (!EditorUtility.DisplayDialog("确认", string.Format("处理了{0}个资源，有可能还需要继续处理，在点继续前可以保存一下工程，等一会再继续。", totalModifiedCount), "继续", "取消")) {
                    break;
                }
            }
        }
    }

    [MenuItem("工具/AssetTool/设置非场景资源导入属性", false, 200)]
    public static void SetOtherAssetInDB()
    {
        int totalModifiedCount = 1;
        while (totalModifiedCount > 0) {
            //自动处理3次，还没完就询问是否继续
            for (int ct = 0; ct < 3; ++ct) {
                totalModifiedCount = SetAllAssetInDbOnce(2);
                if (totalModifiedCount <= 0)
                    break;
            }
            if (totalModifiedCount > 0) {
                if (!EditorUtility.DisplayDialog("确认", string.Format("处理了{0}个资源，有可能还需要继续处理，在点继续前可以保存一下工程，等一会再继续。", totalModifiedCount), "继续", "取消")) {
                    break;
                }
            }
        }
    }

    [MenuItem("工具/AssetTool/奔跑吧兄弟", false, 300)]
    public static void RunningMan()
    {
        if (EditorUtility.DisplayDialog("确认", "你自愿为大家生成cache server资源，这将是个耗时的旅程，准备好了吗？", "奔跑吧", "我累了")) {
            RunningManGo();
        }
    }

    public static void RunningManGo()
    {
        SetAllAssetInDbOnce(0);
        try {
            AssetDatabase.StartAssetEditing();

            long seed = System.Diagnostics.Stopwatch.GetTimestamp();
            var r = new System.Random((int)seed);
            var assets = AssetDatabase.GetAllAssetPaths();
            int curCt = 0;
            int totalCt = assets.Length;
            for (int ct = 0; ct < 4; ++ct) {
                foreach (var asset in assets) {
                    if (r.Next(100) < 8) {
                        AssetDatabase.ImportAsset(asset, ImportAssetOptions.ForceUpdate);
                    }
                    ++curCt;
                    if (EditorUtility.DisplayCancelableProgressBar("奔跑进度", string.Format("{0}/{1}, {2}", curCt / 4, totalCt, asset), curCt * 0.25f / totalCt))
                        return;
                }
                EditorUtility.UnloadUnusedAssetsImmediate(true);
            }
        }
        finally {
            AssetDatabase.StopAssetEditing();

            EditorUtility.UnloadUnusedAssetsImmediate(true);
            EditorUtility.ClearProgressBar();
        }
    }

    public static void Fetch()
    {
        string res;
        if (ResourceEditGetPoster.HttpGet("AssetProcessorDB", out res)) {
            try {
                JsonUtility.FromJsonOverwrite(res, AssetProcessorDB.DBInstance);
                EditorUtility.SetDirty(DBInstance);
                AssetDatabase.SaveAssets();
                Debug.Log("Fetch ok.");
            } catch (Exception ex) {
                Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }

    public static void Commit()
    {
        var json = JsonUtility.ToJson(AssetProcessorDB.DBInstance, true);
        string r;
        if(ResourceEditGetPoster.HttpPost("AssetProcessorDB", json, out r)) {
            Debug.Log("Commit ok.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0--all 1--scene 2--other</param>
    /// <returns></returns>
    private static int SetAllAssetInDbOnce(int type)
    {
        int totalModifiedCount = 0;
        ResourceEditUtility.UseSpecificSettingDB = true;
        ResourceEditUtility.EnableSaveAndReimport = true;
        try {
            AssetDatabase.StartAssetEditing();
            var processedAssets = new HashSet<string>();

            int count = DBInstance.Count;
            for (int i = 0; i < count; ++i) {
                AssetProcessorInfo param = DBInstance[i];
                param.PrepareFiltersAndProcesses();
                if (type==1 && !param.ResPath.Contains("SceneRes")) {
                    continue;
                } else if(type==2 && param.ResPath.Contains("SceneRes")) {
                    continue;
                }

                ResourceProcessor.Instance.SelectDsl(param.DslPath);
                ResourceProcessor.Instance.CollectPath = param.ResPath;
                ResourceProcessor.Instance.OverridedProgressTitle = string.Format("{0}/{1}", i + 1, count);
                ResourceProcessor.Instance.Refresh(true);
                ResourceProcessor.Instance.OverridedProgressTitle = string.Format("{0}/{1}", i + 1, count);
                ResourceProcessor.Instance.SelectAll();
                int ct = ResourceProcessor.Instance.Process(true);
                if (ct > 0)
                    totalModifiedCount += ct;
            }
            TextureImporterParamsDB.UpdateAllTextures();
            ModelImporterParamsDB.UpdateAllModels();
            PrefabParamsDB.UpdateAllPrefabs();

            return totalModifiedCount;
        } finally {
            ResourceEditUtility.EnableSaveAndReimport = false;
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh(ImportAssetOptions.Default);
            EditorUtility.UnloadUnusedAssetsImmediate(true);
            EditorUtility.ClearProgressBar();
        }
    }

    public static string[] GetFiles(string path, IList<string> filters, SearchOption searchOption)
	{
		List<string> files = new List<string>();
        if (File.Exists(path)) {
            files.Add(path);
        } else {
            foreach (string sp in filters)
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
            files.Sort();
        }
		return files.ToArray();
	}

	public delegate bool AssetImporterProcesserHanlder(UnityEngine.Object ai, bool onlySetting, bool handled);

	public static List<AssetImporterProcesserHanlder> GetHandlers(AssetProcessorInfo param)
	{
		List<AssetImporterProcesserHanlder> handlers = new List<AssetImporterProcesserHanlder>();
        if (null != param.Processes) {
            for (int i = 0; i < param.Processes.Count; ++i) {
                handlers.Add(GetHandler(param.Processes[i]));
            }
        }
		return handlers;
	}

	public static AssetImporterProcesserHanlder GetHandler(AssetProcessorEnum p)
	{
		switch (p) {
            //---------------------------------------------------
            case AssetProcessorEnum.CorrectNoneAlphaTexture:
                return CorrectNoneAlphaTexture;
            case AssetProcessorEnum.SetAndroidASTC:
				return SetAndroidASTC;
			case AssetProcessorEnum.SetIPhoneASTC:
				return SetIPhoneASTC;
            case AssetProcessorEnum.SetAndroidSceneASTC:
                return SetAndroidSceneASTC;
            case AssetProcessorEnum.SetIPhoneSceneASTC:
                return SetIPhoneSceneASTC;
            case AssetProcessorEnum.SetCompressed:
				return SetCompressed;
			case AssetProcessorEnum.SetUncompressed:
				return SetUncompressed;
			case AssetProcessorEnum.SetReadableTrue:
				return SetReadableTrue;
			case AssetProcessorEnum.SetReadableFalse:
				return SetReadableFalse;
            case AssetProcessorEnum.SetMipMapTrue:
                return SetMipMapTrue;
            case AssetProcessorEnum.SetMipMapFalse:
                return SetMipMapFalse;
            case AssetProcessorEnum.SetMipMapFilterBox:
                return SetMipMapFilterBox;
            case AssetProcessorEnum.SetMipMapFilterKaiser:
                return SetMipMapFilterKaiser;
            case AssetProcessorEnum.SetStreamingMipMapsTrue:
                return SetStreamingMipMapsTrue;
            case AssetProcessorEnum.SetStreamingMipMapsFalse:
                return SetStreamingMipMapsFalse;
            case AssetProcessorEnum.SetMipMapBiasOne:
                return SetMipMapBiasOne;
            case AssetProcessorEnum.SetMipMapBiasZero:
                return SetMipMapBiasZero;
            case AssetProcessorEnum.SetLowDetailTrue:
                return SetLowDetailTrue;
            case AssetProcessorEnum.SetLowDetailFalse:
                return SetLowDetailFalse;
            case AssetProcessorEnum.SetAndroidMaxSize1024:
                return SetAndroidMaxSize1024;
            case AssetProcessorEnum.SetIPhoneMaxSize1024:
                return SetIPhoneMaxSize1024;
            case AssetProcessorEnum.SetAndroidMaxSize512:
                return SetAndroidMaxSize512;
            case AssetProcessorEnum.SetIPhoneMaxSize512:
                return SetIPhoneMaxSize512;
            case AssetProcessorEnum.SetFilterPoint:
                return SetFilterPoint;
            case AssetProcessorEnum.SetFilterBilinear:
                return SetFilterBilinear;
            case AssetProcessorEnum.SetFilterTrilinear:
                return SetFilterTrilinear;
            case AssetProcessorEnum.SetAnisoLevel4:
                return SetAnisoLevel4;
            case AssetProcessorEnum.SetAnisoLevelN1:
                return SetAnisoLevelN1;
            case AssetProcessorEnum.SetNpotScaleNearest:
                return SetNpotScaleNearest;
            case AssetProcessorEnum.SetAndroidOverrideFalse:
                return SetAndroidOverrideFalse;
            case AssetProcessorEnum.SetIPhoneOverrideFalse:
                return SetIPhoneOverrideFalse;
            case AssetProcessorEnum.SetAnimatorCullCompletely:
                return SetAnimatorCullCompletely;
            case AssetProcessorEnum.SetAnimatorAlwaysAnimate:
                return SetAnimatorAlwaysAnimate;
            case AssetProcessorEnum.SetAnimationCompressOptimal:
                return SetAnimationCompressOptimal;
            case AssetProcessorEnum.SetAnimationCompressDefault:
                return SetAnimationCompressDefault;
            case AssetProcessorEnum.SetMeshReadableTrue:
                return SetMeshReadableTrue;
            case AssetProcessorEnum.SetMeshReadableFalse:
                return SetMeshReadableFalse;
            case AssetProcessorEnum.SetMeshOptimizeGameObjectsTrue:
                return SetMeshOptimizeGameObjectsTrue;
            case AssetProcessorEnum.SetMeshOptimizeGameObjectsFalse:
                return SetMeshOptimizeGameObjectsFalse;
            case AssetProcessorEnum.SetMeshOptimizeTrue:
                return SetMeshOptimizeTrue;
            case AssetProcessorEnum.SetMeshOptimizeFalse:
                return SetMeshOptimizeFalse;
            case AssetProcessorEnum.SetMeshCompressMedium:
                return SetMeshCompressMedium;
            case AssetProcessorEnum.SetMeshCompressOff:
                return SetMeshCompressOff;
            case AssetProcessorEnum.SetMeshImportMaterialsFalse:
                return SetMeshImportMaterialsFalse;
            case AssetProcessorEnum.ClearAnimationScaleCurve:
                return ClearAnimationScaleCurve;
            //---------------------不要在下面添加新的---------------------
            case AssetProcessorEnum.SetDirty:
                return SetDirty;
            case AssetProcessorEnum.SaveAndReimport:
                return SaveAndReimport;
            default:
				return null;
		}
	}

    //---------------------------------------------------
    public static bool SetIPhoneOverrideTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetOverride(im, IPHONE, true);
	}

	public static bool SetIPhoneOverrideFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetOverride(im, IPHONE, false);
	}

	public static bool SetAndroidOverrideTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetOverride(im, ANDROID, true);
	}

	public static bool SetAndroidOverrideFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetOverride(im, ANDROID, false);
	}

    public static bool CorrectNoneAlphaTexture(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        if(im.alphaSource!=TextureImporterAlphaSource.None && !im.DoesSourceTextureHaveAlpha()) {
            handled = true;
            im.alphaSource = TextureImporterAlphaSource.None;
        }
        return handled;
    }

    public static bool SetAndroidASTC(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        if (im.textureType == TextureImporterType.Lightmap) {
            return handled || SetLightmapASTC(im, ANDROID);
        } else {
            return handled || SetASTC(im, ANDROID);
        }
	}

	public static bool SetIPhoneASTC(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        if (im.textureType == TextureImporterType.Lightmap) {
            return handled || SetLightmapASTC(im, IPHONE);
        } else {
            return handled || SetASTC(im, IPHONE);
        }
    }

    public static bool SetAndroidSceneASTC(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        if (im.textureType == TextureImporterType.Lightmap) {
            return handled || SetLightmapASTC(im, ANDROID);
        }
        else {
            return handled || SetSceneASTC(im, ANDROID);
        }
    }

    public static bool SetIPhoneSceneASTC(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        if (im.textureType == TextureImporterType.Lightmap) {
            return handled || SetLightmapASTC(im, IPHONE);
        }
        else {
            return handled || SetSceneASTC(im, IPHONE);
        }
    }

    public static bool SetCompressed(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        bool rt = handled;
		if (im.textureCompression != TextureImporterCompression.Compressed)
		{
			rt = true;
			im.textureCompression = TextureImporterCompression.Compressed;
			Debug.Log("SetCompressed succ:" + im.assetPath);
		}
		return rt;
	}

	public static bool SetUncompressed(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        bool rt = handled;
		if (im.textureCompression != TextureImporterCompression.Uncompressed)
		{
			rt = true;
			im.textureCompression = TextureImporterCompression.Compressed;
			Debug.Log("SetUncompressed succ:" + im.assetPath);
		}
		return rt;
	}

	public static bool SetReadableTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetReadable(im, true);
	}

	public static bool SetReadableFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetReadable(im, false);
	}

	public static bool SetMipMapTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmap(im, true);
	}

	public static bool SetMipMapFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmap(im, false);
    }

    public static bool SetMipMapFilterBox(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmapFilter(im, TextureImporterMipFilter.BoxFilter);
    }

    public static bool SetMipMapFilterKaiser(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmapFilter(im, TextureImporterMipFilter.KaiserFilter);
    }

    public static bool SetStreamingMipMapsTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetStreamingMipMaps(im, true);
    }

    public static bool SetStreamingMipMapsFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetStreamingMipMaps(im, false);
    }

    public static bool SetMipMapBiasOne(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmapBias(im, 1);
    }

    public static bool SetMipMapBiasZero(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMipmapBias(im, 0);
    }

    public static bool SetLowDetailTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetLowDetail(im, true);
    }

    public static bool SetLowDetailFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetLowDetail(im, false);
    }

    public static bool SetAndroidMaxSize1024(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMaxSize(im, ANDROID, 1024);
    }

    public static bool SetIPhoneMaxSize1024(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMaxSize(im, IPHONE, 1024);
    }

    public static bool SetAndroidMaxSize512(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMaxSize(im, ANDROID, 512);
    }

    public static bool SetIPhoneMaxSize512(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetMaxSize(im, IPHONE, 512);
    }

    public static bool SetFilterPoint(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetFilterMode(im, FilterMode.Point);
    }

    public static bool SetFilterBilinear(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetFilterMode(im, FilterMode.Bilinear);
    }

    public static bool SetFilterTrilinear(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetFilterMode(im, FilterMode.Trilinear);
    }

    public static bool SetAnisoLevel4(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetAnisoLevel(im, 4);
    }

    public static bool SetAnisoLevelN1(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetAnisoLevel(im, -1);
    }

    public static bool SetNpotScaleNearest(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var im = ai as TextureImporter;
        if (null == im)
            return handled;
        return handled || SetNpotScale(im, TextureImporterNPOTScale.ToNearest);
    }

    public static bool SetAnimatorCullCompletely(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var obj = ai as UnityEngine.GameObject;
        if (null == obj)
            return handled;
        return handled || SetAnimatorCullingMode(obj, AnimatorCullingMode.CullCompletely);
    }
    
    public static bool SetAnimatorAlwaysAnimate(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var obj = ai as UnityEngine.GameObject;
        if (null == obj)
            return handled;
        return handled || SetAnimatorCullingMode(obj, AnimatorCullingMode.AlwaysAnimate);
    }

    public static bool SetAnimationCompressOptimal(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetAnimationCompression(mi, ModelImporterAnimationCompression.Optimal);
    }

    public static bool SetAnimationCompressDefault(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetAnimationCompression(mi, ModelImporterAnimationCompression.KeyframeReduction);
    }

    public static bool SetMeshReadableTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshReadable(mi, true);
    }

    public static bool SetMeshReadableFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshReadable(mi, false);
    }

    public static bool SetMeshOptimizeGameObjectsTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshOptimizeGameObjects(mi, true);
    }

    public static bool SetMeshOptimizeGameObjectsFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshOptimizeGameObjects(mi, false);
    }

    public static bool SetMeshOptimizeTrue(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshOptimize(mi, true);
    }

    public static bool SetMeshOptimizeFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshOptimize(mi, false);
    }

    public static bool SetMeshCompressMedium(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshCompression(mi, ModelImporterMeshCompression.Medium);
    }

    public static bool SetMeshCompressOff(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshCompression(mi, ModelImporterMeshCompression.Off);
    }

    public static bool SetMeshImportMaterialsFalse(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var mi = ai as ModelImporter;
        if (null == mi)
            return handled;
        return handled || SetMeshImportMaterials(mi, false);
    }

    public static bool ClearAnimationScaleCurve(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        var obj = ai as UnityEngine.GameObject;
        if (null == obj)
            return handled;
        bool modified = handled;
        List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(obj));
        foreach (AnimationClip theAnimation in animationClipList) {
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation)) {
                string name = theCurveBinding.propertyName.ToLower();
                if (name.Contains("scale")) {
                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                    Debug.Log("ClearAnimationScaleCurve succ:" + theAnimation.name + " " + name);
                }
            }
        }
        return modified;
    }

    //---------------------------------------------------
    public static bool SetDirty(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        handled = handled || ResourceEditUtility.ForceSaveAndReimport;
        if (handled && !onlySetting) {
            EditorUtility.SetDirty(ai);
        }
        return handled;
    }

    public static bool SaveAndReimport(UnityEngine.Object ai, bool onlySetting, bool handled)
    {
        handled = handled || ResourceEditUtility.ForceSaveAndReimport;
        if (handled && !onlySetting && ResourceEditUtility.EnableSaveAndReimport) {
            var assetImporter = ai as UnityEditor.AssetImporter;
            if (null != assetImporter) {
                //assetImporter.SaveAndReimport();
                AssetDatabase.ImportAsset(assetImporter.assetPath);
            }
        }
        return handled;
    }

    //---------------------------------------------------
    private static bool SetOverride(TextureImporter im, string platform, bool value)
    {
        bool rt = false;
        var settings = im.GetPlatformTextureSettings(platform);
        if (settings == null) {
            Debug.LogError("no texturesettings for " + platform);
        } else if (settings.overridden != value) {
            rt = true;
            settings.overridden = value;
            Debug.Log("set override succ:" + im.assetPath + " " + platform + " " + value);
        }
        im.SetPlatformTextureSettings(settings);
        return rt;
    }

    private static bool SetASTC(TextureImporter im, string platform)
    {
        bool rt = false;
        var settings = im.GetPlatformTextureSettings(platform);
        if (settings == null) {
            Debug.LogError("no texturesettings for " + platform);
            return rt;
        }
        string fileName = Path.GetFileNameWithoutExtension(im.assetPath).ToLower();
        //no alpha use ASTC_RGB_8x8
        if (im.alphaSource == TextureImporterAlphaSource.None) {
            if (im.textureType == TextureImporterType.NormalMap || fileName.EndsWith("_nm")) {
                if (settings.format != TextureImporterFormat.ASTC_RGB_6x6) {
                    rt = true;
                    settings.format = TextureImporterFormat.ASTC_RGB_6x6;
                    Debug.Log("SetASTC succ:" + im.assetPath + " " + platform + " ASTC_RGB_6x6");
                }
            }
            else {
                if (settings.format != TextureImporterFormat.ASTC_RGB_8x8) {
                    rt = true;
                    settings.format = TextureImporterFormat.ASTC_RGB_8x8;
                    Debug.Log("SetASTC succ:" + im.assetPath + " " + platform + " ASTC_RGB_8x8");
                }
            }
        }
        //with alpha use ASTC_RGBA_6x6
        else {
            if (im.textureType == TextureImporterType.NormalMap || fileName.EndsWith("_nm")) {
                if (settings.format != TextureImporterFormat.ASTC_RGBA_6x6) {
                    rt = true;
                    settings.format = TextureImporterFormat.ASTC_RGBA_6x6;
                    Debug.Log("SetASTC succ:" + im.assetPath + " " + platform + " ASTC_RGBA_6x6");
                }
            } else {
                if (settings.format != TextureImporterFormat.ASTC_RGBA_8x8) {
                    rt = true;
                    settings.format = TextureImporterFormat.ASTC_RGBA_8x8;
                    Debug.Log("SetASTC succ:" + im.assetPath + " " + platform + " ASTC_RGBA_8x8");
                }
            }
        }
        im.SetPlatformTextureSettings(settings);
        rt = SetOverride(im, platform, true) || rt;
        return rt;
    }

    private static bool SetSceneASTC(TextureImporter im, string platform)
    {
        bool rt = false;
        var settings = im.GetPlatformTextureSettings(platform);
        if (settings == null) {
            Debug.LogError("no texturesettings for " + platform);
            return rt;
        }
        string fileName = Path.GetFileNameWithoutExtension(im.assetPath).ToLower();
        //no alpha use ASTC_RGB_8x8
        if (im.alphaSource == TextureImporterAlphaSource.None) {
            if (settings.format != TextureImporterFormat.ASTC_RGB_8x8) {
                rt = true;
                settings.format = TextureImporterFormat.ASTC_RGB_8x8;
                Debug.Log("SetSceneASTC succ:" + im.assetPath + " " + platform + " ASTC_RGB_8x8");
            }
        }
        //with alpha use ASTC_RGBA_8x8
        else {
            if (settings.format != TextureImporterFormat.ASTC_RGBA_8x8) {
                rt = true;
                settings.format = TextureImporterFormat.ASTC_RGBA_8x8;
                Debug.Log("SetSceneASTC succ:" + im.assetPath + " " + platform + " ASTC_RGBA_8x8");
            }
        }
        im.SetPlatformTextureSettings(settings);
        rt = SetOverride(im, platform, true) || rt;
        return rt;
    }

    private static bool SetLightmapASTC(TextureImporter im, string platform)
    {
        bool rt = false;
        var settings = im.GetPlatformTextureSettings(platform);
        if (settings == null) {
            Debug.LogError("no texturesettings for " + platform);
            return rt;
        }
        //lightmap use ASTC_RGB_8x8       
        if (im.textureType == TextureImporterType.Lightmap) {
            if (settings.format != TextureImporterFormat.ASTC_RGB_8x8) {
                rt = true;
                settings.format = TextureImporterFormat.ASTC_RGB_8x8;
                Debug.Log("SetLightmapASTC succ:" + im.assetPath + " " + platform + " ASTC_RGB_8x8");
            }
            im.SetPlatformTextureSettings(settings);
            rt = SetOverride(im, platform, true) || rt;
        } else {
            Debug.LogError("SetLightmapASTC failed:" + im.assetPath + " " + platform + " ASTC_RGB_8x8, texture type " + im.textureType + " is not lightmap");
            return rt;
        }
        return rt;
    }

    private static bool SetReadable(TextureImporter im, bool value)
    {
        bool rt = false;
        if (im.isReadable != value) {
            rt = true;
            im.isReadable = value;
            Debug.Log("SetReadable succ:" + im.assetPath + " " + value);
        }
        return rt;
    }

    private static bool SetMipmap(TextureImporter im, bool value)
    {
        bool rt = false;
        if (im.textureShape == TextureImporterShape.TextureCube) {
            if (!im.mipmapEnabled) {
                rt = true;
                im.mipmapEnabled = true;
            } else {
                rt = false;
            }
        } else {
            var assetName = Path.GetFileNameWithoutExtension(im.assetPath);
            if (assetName.StartsWith("UI_") || assetName.StartsWith("E_UI_")) {
                if (im.mipmapEnabled) {
                    rt = true;
                    im.mipmapEnabled = false;
                    Debug.Log("SetMipMap succ:" + im.assetPath + " false");
                }
            }
            else {
                if (im.mipmapEnabled != value) {
                    rt = true;
                    im.mipmapEnabled = value;
                    Debug.Log("SetMipMap succ:" + im.assetPath + " " + value);
                }
            }
        }
        return rt;
    }

    private static bool SetMipmapFilter(TextureImporter im, TextureImporterMipFilter value)
    {
        bool rt = false;
        if (im.mipmapFilter != value) {
            rt = true; 
            im.mipmapFilter = value;
            Debug.Log("SetMipmapFilter succ:" + im.assetPath + " " + value);
        }
        return rt;
    }

    private static bool SetStreamingMipMaps(TextureImporter im, bool value)
    {
        bool rt = false;
        var assetName = Path.GetFileNameWithoutExtension(im.assetPath);
        if (assetName.StartsWith("UI_") || assetName.StartsWith("E_UI_")) {
            if (im.streamingMipmaps) {
                rt = true;
                im.streamingMipmaps = false;
                Debug.Log("SetStreamingMipmaps succ:" + im.assetPath + " false");
            }
        }
        else {
            if (im.streamingMipmaps != value) {
                rt = true;
                im.streamingMipmaps = value;
                Debug.Log("SetStreamingMipmaps succ:" + im.assetPath + " " + value);
            }
        }
        return rt;
    }

    private static bool SetMipmapBias(TextureImporter im, float value)
    {
        bool rt = false;
        if (Mathf.Abs(im.mipMapBias - value) > Mathf.Epsilon) {
            rt = true;
            im.mipMapBias = value;
            Debug.Log("SetMipmapBias succ:" + im.assetPath + " " + value);
        }
        return rt;
    }

    private static bool SetLowDetail(TextureImporter im, bool value)
    {
        bool rt = false;
		int val = value ? 1 : 0;
#if UNITY_EDITOR_WIN
        //if (im.lowDetail != val) {
        //    rt = true;
        //    im.lowDetail = val;
        //    Debug.Log("SetLowDetail succ:" + im.assetPath + " " + value);
        //}
#endif
        return rt;
    }

    private static bool SetCustomFlag(TextureImporter im, int value)
    {
        bool rt = false;
#if UNITY_EDITOR_WIN
        if (im.customFlag != value) {
            rt = true;
            im.customFlag = value;
            Debug.Log("SetCustomFlag succ:" + im.assetPath + " " + value);
        }
#endif
        return rt;
    }

    private static bool SetCustomValue(TextureImporter im, int value)
    {
        bool rt = false;
#if UNITY_EDITOR_WIN
        if (im.customValue != value) {
            rt = true;
            im.customValue = value;
            Debug.Log("SetCustomValue succ:" + im.assetPath + " " + value);
        }
#endif
        return rt;
    }

    private static bool SetMaxSize(TextureImporter im, string platform, int maxSize)
    {
        bool rt = false;
        var settings = im.GetPlatformTextureSettings(platform);
        if (settings == null) {
            Debug.LogError("no texturesettings for " + platform);
            return rt;
        }
        if (settings.maxTextureSize != maxSize) {
            rt = true;
            settings.maxTextureSize = maxSize;
            Debug.Log("SetMaxSize succ:" + im.assetPath + " " + platform + " " + maxSize);
        }
        //string fileName = Path.GetFileNameWithoutExtension(im.assetPath).ToLower();
        im.SetPlatformTextureSettings(settings);
        rt = SetOverride(im, platform, true) || rt;
        return rt;
    }

    private static bool SetFilterMode(TextureImporter im, FilterMode value)
    {
        bool rt = false;
        if (im.filterMode != value) {
            rt = true;
            im.filterMode = value;
            Debug.Log("SetAnisoLevel succ:" + im.assetPath + " " + value);
        }
        return rt;
    }

    private static bool SetAnisoLevel(TextureImporter im, int value)
    {
        bool rt = false;
        if (im.anisoLevel != value) {
            rt = true;
            im.anisoLevel = value;
            Debug.Log("SetAnisoLevel succ:" + im.assetPath + " " + value);
        }
        return rt;
    }

    private static bool SetNpotScale(TextureImporter im, TextureImporterNPOTScale npotScale)
    {
        bool rt = false;
        if (im.npotScale != npotScale) {
            rt = true;
            im.npotScale = npotScale;
            Debug.Log("SetNpotScale succ:" + im.assetPath + " " + npotScale);
        }
        return rt;
    }

    private static bool SetAnimatorCullingMode(GameObject obj, AnimatorCullingMode mode)
    {
        var animator = obj.GetComponentInChildren<Animator>();
        if (null == animator)
            return false;
        bool rt = false;
        if (animator.cullingMode != mode) {
            rt = true;
            animator.cullingMode = mode;
            Debug.Log("SetAnimatorCullingMode succ:" + obj.name + " " + mode);
        }
        return rt;
    }

    private static bool SetAnimationCompression(ModelImporter mi, ModelImporterAnimationCompression compression)
    {
        bool rt = false;
        if (mi.animationCompression != compression) {
            rt = true;
            mi.animationCompression = compression;
            Debug.Log("SetAnimationCompression succ:" + mi.assetPath + " " + compression);
        }
        return rt;
    }

    private static bool SetMeshReadable(ModelImporter mi, bool readable)
    {
        bool rt = false;
        if (mi.isReadable != readable) {
            rt = true;
            mi.isReadable = readable;
            Debug.Log("SetMeshReadable succ:" + mi.assetPath + " " + readable);
        }
        return rt;
    }

    private static bool SetMeshOptimizeGameObjects(ModelImporter mi, bool optimize)
    {
        bool rt = false;
        if (mi.optimizeGameObjects != optimize) {
            rt = true;
            mi.optimizeGameObjects = optimize;
            Debug.Log("SetMeshOptimizeGameObjects succ:" + mi.assetPath + " " + optimize);
        }
        return rt;
    }

    private static bool SetMeshOptimize(ModelImporter mi, bool optimize)
    {
        bool rt = false;
        if (mi.optimizeMesh != optimize) {
            rt = true;
            mi.optimizeMesh = optimize;
            Debug.Log("SetMeshOptimize succ:" + mi.assetPath + " " + optimize);
        }
        return rt;
    }

    private static bool SetMeshCompression(ModelImporter mi, ModelImporterMeshCompression compression)
    {
        bool rt = false;
        if (mi.meshCompression != compression) {
            rt = true;
            mi.meshCompression = compression;
            Debug.Log("SetMeshCompression succ:" + mi.assetPath + " " + compression);
        }
        return rt;
    }

    private static bool SetMeshImportMaterials(ModelImporter mi, bool import)
    {
        bool rt = false;
        if (mi.importMaterials != import) {
            rt = true;
            mi.importMaterials = import;
            Debug.Log("SetMeshImportMaterials succ:" + mi.assetPath + " " + import);
        }
        return rt;
    }

    private static AssetProcessorDB m_DBInstance;
	public static AssetProcessorDB DBInstance
	{
		get
		{
			if (m_DBInstance == null)
				m_DBInstance = AssetDatabase.LoadAssetAtPath<AssetProcessorDB>(PATH);
			return m_DBInstance;
		}
	}
}
