using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace GameFramework
{
    public class UiResourceSystem
    {
        public UnityEngine.Object GetUiResource(string res)
        {
            UnityEngine.Object obj = null;
            if (string.IsNullOrEmpty(res)) {
                return obj;
            }
            if (!m_LoadedPrefabs.TryGetValue(res, out obj)) {
                if (GlobalVariables.Instance.IsPublish && Contains(res)) {
                    obj = Load(res);
                }
                if (obj == null) {
                    obj = Resources.Load(res);
                }
                if (obj != null) {
                    m_LoadedPrefabs.Add(res, obj);
                } else {
                    UnityEngine.Debug.Log("LoadAsset failed:" + res);
                }
            }
            return obj;
        }
        public void UnloadUiResource(string res)
        {
            AssetBundle ab = null;
            if (m_AssetBundles.TryGetValue(res, out ab)) {
                ab.Unload(true);
                m_AssetBundles.Remove(res);
            }
        }
        public void CleanupAllUiResources()
        {
            m_LoadedPrefabs.Clear();
            foreach (var pair in m_AssetBundles) {
                AssetBundle ab = pair.Value;
                ab.Unload(true);
            }
        }

        private bool Contains(string res)
        {
            string path = GetAssetBundleFile(res);
            if (File.Exists(path))
                return true;
            return false;
        }
        private UnityEngine.Object Load(string res)
        {
            UnityEngine.Object obj = null;
            AssetBundle ab = null;
            if (!m_AssetBundles.TryGetValue(res, out ab)) {
                string path = GetAssetBundleFile(res);
                ab = AssetBundle.LoadFromFile(path);
                //Debug.LogWarningFormat("load {0}({1}) from assetbundle ({2}).", res, path, ab.name);
            }
            if (null != ab) {
                string name = GetAssetName(res);
                obj = ab.LoadAsset(name);
            }
            return obj;
        }

        private string GetAssetName(string res)
        {
            string name = Path.GetFileNameWithoutExtension(res);
            return name.ToLower();
        }
        private string GetAssetBundleFile(string res)
        {
            string path = HomePath.GetAbsolutePath(res);
            path = Path.ChangeExtension(path, ".ab");
            return path.ToLower();
        }

        private MyDictionary<string, UnityEngine.Object> m_LoadedPrefabs = new MyDictionary<string, UnityEngine.Object>();
        private MyDictionary<string, AssetBundle> m_AssetBundles = new MyDictionary<string, AssetBundle>();

        public static UiResourceSystem Instance
        {
            get { return s_Instance; }
        }
        private static UiResourceSystem s_Instance = new UiResourceSystem();
    }
}
