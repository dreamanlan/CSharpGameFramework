using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ProcessModelPost : AssetPostprocessor
{
  
  void OnPostprocessModel (GameObject g) {
    Apply(g);
  }
  
  void Apply(GameObject g)
  {
    
    ModelImporter mi = assetImporter as ModelImporter;
    if (mi == null)
      return;
    if (mi.animationType == ModelImporterAnimationType.None)
      return;
//    if (mi.animationType != ModelImporterAnimationType.Legacy) {
//      Debug.Log("xxxxxxxxxxxx"+g.name );
//      return;
//    }
    List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
    foreach (AnimationClip theAnimation in animationClipList)
    {
      
      foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
      {
        string name = theCurveBinding.propertyName.ToLower();
        if (name.Contains("scale"))
        {
          AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
        }
      } 
    }
//    EditorUtility.SetDirty(g);
//    AssetDatabase.SaveAssets();
  }
}