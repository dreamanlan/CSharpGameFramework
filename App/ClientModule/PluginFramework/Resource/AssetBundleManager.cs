using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameFramework
{
    public class AssetBundleManager
    {
        public bool Contains(string res)
        {
            string path = GetAssetBundleFile(res);            
            if (File.Exists(path)) {
                return true;
            }
            return false;
        }
        public UnityEngine.Object Load(string res)
        {
            string name = GetAssetName(res);
            string path = GetAssetBundleFile(res);
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            //Debug.LogWarningFormat("load {0}({1}) from assetbundle ({2}).", res, path, ab.name);
            Object obj = ab.LoadAsset(name);
            ab.Unload(false);
            return obj;
        }
        public void LoadAsync(string res, ResourceSystem.ResourceLoadDelegation callback)
        {
            string name = GetAssetName(res);
            string path = GetAssetBundleFile(res);
            Utility.SendScriptMessage("LoadAssetAsync", new object[] { path, name, callback });
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
        
        public static AssetBundleManager Instance
        {
            get { return s_Instance; }
        }
        private static AssetBundleManager s_Instance = new AssetBundleManager();
    }
}
