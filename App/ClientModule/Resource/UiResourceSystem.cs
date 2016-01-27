using System;
using System.Collections.Generic;
using System.Text;
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

        private bool Contains(string res)
        {
            return false;
        }
        private UnityEngine.Object Load(string res)
        {
            return null;
        }
        private void Unload(string res)
        {

        }

        private Dictionary<string, UnityEngine.Object> m_LoadedPrefabs = new Dictionary<string, UnityEngine.Object>();
        public static UiResourceSystem Instance
        {
            get { return s_Instance; }
        }
        private static UiResourceSystem s_Instance = new UiResourceSystem();
    }
}
