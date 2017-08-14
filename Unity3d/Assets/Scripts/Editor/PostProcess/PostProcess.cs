using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GeneralPostProcessor : AssetPostprocessor
{
    void OnPostprocessModel(GameObject g)
    {
        if (!m_DataInited) {
#if UNITY_ANDROID
            PostProcessDataOfAndroid.GetAnimaticFbxSet(m_AnimaticFbxSet);
            PostProcessDataOfAndroid.GetUnanimaticFbxSet(m_UnanimaticFbxSet);
            PostProcessDataOfAndroid.GetTextureSet(m_TextureSet);
#endif

#if UNITY_IOS
            PostProcessDataOfIos.GetAnimaticFbxSet(m_AnimaticFbxSet);
            PostProcessDataOfIos.GetUnanimaticFbxSet(m_UnanimaticFbxSet);
            PostProcessDataOfIos.GetTextureSet(m_TextureSet);
#endif
        }
        Apply(g);
    }

    void Apply(GameObject g)
    {
        ModelImporter mi = assetImporter as ModelImporter;
        TextureImporter ti = assetImporter as TextureImporter;
        if (null != mi) {
            if (IsAnimaticFbx(assetPath)) {
                if (mi.animationType != ModelImporterAnimationType.None) {
                    List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
                    foreach (AnimationClip theAnimation in animationClipList) {
                        foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation)) {
                            string name = theCurveBinding.propertyName.ToLower();
                            if (name.Contains("scale")) {
                                AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                            }
                        }
                    }
                }

                mi.isReadable = false;
                mi.meshCompression = ModelImporterMeshCompression.High;
                mi.animationCompression = ModelImporterAnimationCompression.Optimal;
            } else if (IsUnanimaticFbx(assetPath)) {
                mi.isReadable = false;
                mi.meshCompression = ModelImporterMeshCompression.High;
                mi.animationType = ModelImporterAnimationType.None;
            }
        } else if (null != ti) {
            int maxSize;
            if (IsTexture(assetPath, out maxSize)) {
                ti.maxTextureSize = maxSize;
                ti.textureCompression = TextureImporterCompression.Compressed;
                ti.isReadable = false;
            }
        }
    }

    private bool IsAnimaticFbx(string path)
    {
        return m_AnimaticFbxSet.Contains(path);
    }
    private bool IsUnanimaticFbx(string path)
    {
        return m_UnanimaticFbxSet.Contains(path);
    }
    private bool IsTexture(string path, out int maxSize)
    {
        return m_TextureSet.TryGetValue(path, out maxSize);
    }

    private bool m_DataInited = false;
    private HashSet<string> m_AnimaticFbxSet = new HashSet<string>();
    private HashSet<string> m_UnanimaticFbxSet = new HashSet<string>();
    private Dictionary<string, int> m_TextureSet = new Dictionary<string, int>();
}

public static partial class PostProcessDataOfAndroid
{
    public static void GetAnimaticFbxSet(HashSet<string> list)
    {
        GetAnimaticFbxSet1(list);
        GetAnimaticFbxSet2(list);
        GetAnimaticFbxSet3(list);
        GetAnimaticFbxSet4(list);
        GetAnimaticFbxSet5(list);
        GetAnimaticFbxSet6(list);
        GetAnimaticFbxSet7(list);
        GetAnimaticFbxSet8(list);
        GetAnimaticFbxSet9(list);
        GetAnimaticFbxSet10(list);
        GetAnimaticFbxSet11(list);
        GetAnimaticFbxSet12(list);
        GetAnimaticFbxSet13(list);
        GetAnimaticFbxSet14(list);
        GetAnimaticFbxSet15(list);
        GetAnimaticFbxSet16(list);
    }
    public static void GetUnanimaticFbxSet(HashSet<string> list)
    {
        GetUnanimaticFbxSet1(list);
        GetUnanimaticFbxSet2(list);
        GetUnanimaticFbxSet3(list);
        GetUnanimaticFbxSet4(list);
        GetUnanimaticFbxSet5(list);
        GetUnanimaticFbxSet6(list);
        GetUnanimaticFbxSet7(list);
        GetUnanimaticFbxSet8(list);
        GetUnanimaticFbxSet9(list);
        GetUnanimaticFbxSet10(list);
        GetUnanimaticFbxSet11(list);
        GetUnanimaticFbxSet12(list);
        GetUnanimaticFbxSet13(list);
        GetUnanimaticFbxSet14(list);
        GetUnanimaticFbxSet15(list);
        GetUnanimaticFbxSet16(list);
    }
    public static void GetTextureSet(Dictionary<string, int> dict)
    {
        int maxSize = 512;
        HashSet<string> list = new HashSet<string>();
        GetTextureSet1(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet2(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet3(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet4(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet5(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet6(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet7(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet8(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet9(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet10(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet11(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet12(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet13(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet14(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet15(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet16(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);
    }
    private static void MergeTextureSet(Dictionary<string, int> dict, HashSet<string> list, int maxSize)
    {
        foreach (string path in list) {
            dict[path] = maxSize;
        }
    }

    static partial void GetAnimaticFbxSet1(HashSet<string> list);
    static partial void GetAnimaticFbxSet2(HashSet<string> list);
    static partial void GetAnimaticFbxSet3(HashSet<string> list);
    static partial void GetAnimaticFbxSet4(HashSet<string> list);
    static partial void GetAnimaticFbxSet5(HashSet<string> list);
    static partial void GetAnimaticFbxSet6(HashSet<string> list);
    static partial void GetAnimaticFbxSet7(HashSet<string> list);
    static partial void GetAnimaticFbxSet8(HashSet<string> list);
    static partial void GetAnimaticFbxSet9(HashSet<string> list);
    static partial void GetAnimaticFbxSet10(HashSet<string> list);
    static partial void GetAnimaticFbxSet11(HashSet<string> list);
    static partial void GetAnimaticFbxSet12(HashSet<string> list);
    static partial void GetAnimaticFbxSet13(HashSet<string> list);
    static partial void GetAnimaticFbxSet14(HashSet<string> list);
    static partial void GetAnimaticFbxSet15(HashSet<string> list);
    static partial void GetAnimaticFbxSet16(HashSet<string> list);

    static partial void GetUnanimaticFbxSet1(HashSet<string> list);
    static partial void GetUnanimaticFbxSet2(HashSet<string> list);
    static partial void GetUnanimaticFbxSet3(HashSet<string> list);
    static partial void GetUnanimaticFbxSet4(HashSet<string> list);
    static partial void GetUnanimaticFbxSet5(HashSet<string> list);
    static partial void GetUnanimaticFbxSet6(HashSet<string> list);
    static partial void GetUnanimaticFbxSet7(HashSet<string> list);
    static partial void GetUnanimaticFbxSet8(HashSet<string> list);
    static partial void GetUnanimaticFbxSet9(HashSet<string> list);
    static partial void GetUnanimaticFbxSet10(HashSet<string> list);
    static partial void GetUnanimaticFbxSet11(HashSet<string> list);
    static partial void GetUnanimaticFbxSet12(HashSet<string> list);
    static partial void GetUnanimaticFbxSet13(HashSet<string> list);
    static partial void GetUnanimaticFbxSet14(HashSet<string> list);
    static partial void GetUnanimaticFbxSet15(HashSet<string> list);
    static partial void GetUnanimaticFbxSet16(HashSet<string> list);

    static partial void GetTextureSet1(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet2(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet3(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet4(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet5(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet6(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet7(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet8(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet9(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet10(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet11(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet12(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet13(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet14(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet15(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet16(HashSet<string> list, ref int maxSize);
}
public static partial class PostProcessDataOfIos
{
    public static void GetAnimaticFbxSet(HashSet<string> list)
    {
        GetAnimaticFbxSet1(list);
        GetAnimaticFbxSet2(list);
        GetAnimaticFbxSet3(list);
        GetAnimaticFbxSet4(list);
        GetAnimaticFbxSet5(list);
        GetAnimaticFbxSet6(list);
        GetAnimaticFbxSet7(list);
        GetAnimaticFbxSet8(list);
        GetAnimaticFbxSet9(list);
        GetAnimaticFbxSet10(list);
        GetAnimaticFbxSet11(list);
        GetAnimaticFbxSet12(list);
        GetAnimaticFbxSet13(list);
        GetAnimaticFbxSet14(list);
        GetAnimaticFbxSet15(list);
        GetAnimaticFbxSet16(list);
    }
    public static void GetUnanimaticFbxSet(HashSet<string> list)
    {
        GetUnanimaticFbxSet1(list);
        GetUnanimaticFbxSet2(list);
        GetUnanimaticFbxSet3(list);
        GetUnanimaticFbxSet4(list);
        GetUnanimaticFbxSet5(list);
        GetUnanimaticFbxSet6(list);
        GetUnanimaticFbxSet7(list);
        GetUnanimaticFbxSet8(list);
        GetUnanimaticFbxSet9(list);
        GetUnanimaticFbxSet10(list);
        GetUnanimaticFbxSet11(list);
        GetUnanimaticFbxSet12(list);
        GetUnanimaticFbxSet13(list);
        GetUnanimaticFbxSet14(list);
        GetUnanimaticFbxSet15(list);
        GetUnanimaticFbxSet16(list);
    }
    public static void GetTextureSet(Dictionary<string, int> dict)
    {
        int maxSize = 512;
        HashSet<string> list = new HashSet<string>();
        GetTextureSet1(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet2(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet3(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet4(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet5(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet6(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet7(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet8(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet9(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet10(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet11(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet12(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet13(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet14(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet15(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);

        list = new HashSet<string>();
        GetTextureSet16(list, ref maxSize);
        MergeTextureSet(dict, list, maxSize);
    }
    private static void MergeTextureSet(Dictionary<string, int> dict, HashSet<string> list, int maxSize)
    {
        foreach (string path in list) {
            dict[path] = maxSize;
        }
    }

    static partial void GetAnimaticFbxSet1(HashSet<string> list);
    static partial void GetAnimaticFbxSet2(HashSet<string> list);
    static partial void GetAnimaticFbxSet3(HashSet<string> list);
    static partial void GetAnimaticFbxSet4(HashSet<string> list);
    static partial void GetAnimaticFbxSet5(HashSet<string> list);
    static partial void GetAnimaticFbxSet6(HashSet<string> list);
    static partial void GetAnimaticFbxSet7(HashSet<string> list);
    static partial void GetAnimaticFbxSet8(HashSet<string> list);
    static partial void GetAnimaticFbxSet9(HashSet<string> list);
    static partial void GetAnimaticFbxSet10(HashSet<string> list);
    static partial void GetAnimaticFbxSet11(HashSet<string> list);
    static partial void GetAnimaticFbxSet12(HashSet<string> list);
    static partial void GetAnimaticFbxSet13(HashSet<string> list);
    static partial void GetAnimaticFbxSet14(HashSet<string> list);
    static partial void GetAnimaticFbxSet15(HashSet<string> list);
    static partial void GetAnimaticFbxSet16(HashSet<string> list);

    static partial void GetUnanimaticFbxSet1(HashSet<string> list);
    static partial void GetUnanimaticFbxSet2(HashSet<string> list);
    static partial void GetUnanimaticFbxSet3(HashSet<string> list);
    static partial void GetUnanimaticFbxSet4(HashSet<string> list);
    static partial void GetUnanimaticFbxSet5(HashSet<string> list);
    static partial void GetUnanimaticFbxSet6(HashSet<string> list);
    static partial void GetUnanimaticFbxSet7(HashSet<string> list);
    static partial void GetUnanimaticFbxSet8(HashSet<string> list);
    static partial void GetUnanimaticFbxSet9(HashSet<string> list);
    static partial void GetUnanimaticFbxSet10(HashSet<string> list);
    static partial void GetUnanimaticFbxSet11(HashSet<string> list);
    static partial void GetUnanimaticFbxSet12(HashSet<string> list);
    static partial void GetUnanimaticFbxSet13(HashSet<string> list);
    static partial void GetUnanimaticFbxSet14(HashSet<string> list);
    static partial void GetUnanimaticFbxSet15(HashSet<string> list);
    static partial void GetUnanimaticFbxSet16(HashSet<string> list);

    static partial void GetTextureSet1(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet2(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet3(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet4(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet5(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet6(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet7(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet8(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet9(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet10(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet11(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet12(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet13(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet14(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet15(HashSet<string> list, ref int maxSize);
    static partial void GetTextureSet16(HashSet<string> list, ref int maxSize);
}