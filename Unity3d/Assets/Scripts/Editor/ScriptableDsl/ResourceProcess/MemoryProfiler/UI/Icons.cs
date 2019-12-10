using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal static class Icons
    {
        public const string IconFolder = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/Icons/";
        public static Texture2D MemoryProfilerWindowTabIcon { get { return LoadIcon(IconFolder + "Memory Profiler.png"); } }

        static Texture2D LoadIcon(string resourcePath, bool autoScale = false)
        {
            if (string.IsNullOrEmpty(resourcePath))
                return null;

            if (EditorGUIUtility.isProSkin)
            {
                var dirName = Path.GetDirectoryName(resourcePath).Replace('\\', '/');
                var fileName = Path.GetFileName(resourcePath);
                var darkSkinVariation = string.Format("{0}/d_{1}", dirName, fileName);
                if (File.Exists(darkSkinVariation))
                {
                    resourcePath = darkSkinVariation;
                }
            }

            float systemScale = EditorGUIUtility.pixelsPerPoint;
            if (autoScale && systemScale > 1f)
            {
                int scale = Mathf.RoundToInt(systemScale);
                string dirName = Path.GetDirectoryName(resourcePath).Replace('\\', '/');
                string fileName = Path.GetFileNameWithoutExtension(resourcePath);
                string fileExt = Path.GetExtension(resourcePath);
                for (int s = scale; scale > 1; --scale)
                {
                    string scaledResourcePath = string.Format("{0}/{1}@{2}x{3}", dirName, fileName, s, fileExt);
                    var scaledResource = AssetDatabase.LoadAssetAtPath<Texture2D>(scaledResourcePath);// EditorResources.Load<Texture2D>(scaledResourcePath, Unsupported.IsDeveloperMode());
                    if (scaledResource != null)
                        return scaledResource;
                    else if (Unsupported.IsDeveloperMode())
                        Debug.LogWarningFormat("Couldn't load scaled Icon asset at path: {0}", scaledResourcePath);
                }
            }
            var resource = AssetDatabase.LoadAssetAtPath<Texture2D>(resourcePath);//EditorResources.Load<Texture2D>(resourcePath, Unsupported.IsDeveloperMode()); if (Unsupported.IsDeveloperMode())
            if (Unsupported.IsDeveloperMode() && resource == null)
                Debug.LogErrorFormat("Couldn't load Icon asset at path: {0}", resourcePath);
            return resource;
        }
    }
}
