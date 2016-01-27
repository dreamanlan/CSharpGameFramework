using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    class EditTools
    {
        #region 工程定位文件
        public static void PingAssetInProject(string file)
        {
            if (!file.StartsWith("Assets/"))
            {
                return;
            }
            UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(file);
            if (asset != null)
            {
                GUI.skin = null;
                //EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(file, typeof(Object)));
                EditorGUIUtility.PingObject(asset);
                Selection.activeObject = asset;
            }
        }
        #endregion 
    }
}
