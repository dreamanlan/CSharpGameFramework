using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class AssetBundleManager
    {
        public void Init()
        {

        }
        public bool Contains(string res)
        {
            return false;
        }
        public UnityEngine.Object Load(string res)
        {
            return null;
        }
        public void Unload(string res)
        {

        }

        public static AssetBundleManager Instance
        {
            get { return s_Instance; }
        }
        private static AssetBundleManager s_Instance = new AssetBundleManager();
    }
}
