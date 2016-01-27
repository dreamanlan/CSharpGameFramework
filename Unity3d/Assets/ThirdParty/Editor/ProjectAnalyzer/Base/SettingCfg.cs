using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace ProjectAnalyzer
{
    //用于显示的
    public class SettingCfgUI
    {
        public static bool textureCheckMemSize = true;
        public static string textureCheckMemSizeValue = "512";
        public static bool textureCheckPix = true;
        public static string textureCheckPixW = "1024";
        public static string textureCheckPixH = "1024";
        public static bool textureCheckPix2Pow = true;
        public static bool textureCheckIsRW = true;
        public static bool textureCheckPlatSetting = true;
        public static bool textureCheckIsLightmap = true;


        public static bool modelCheckScale = true;
        public static bool modelCheckMeshCompression = true;
        public static bool modelCheckAnimCompression = true;
        public static bool modelCheckMeshIsRW = true;
        public static bool modelCheckCollider = true;
        //public static bool modelCheckNormals = true;
        //public static bool modelCheckTangents = true;
        //public static bool modelCheckBakeIK = true;
        //public static bool modelCheckFileSize = true;
        //public static string modelCheckFileSizeValue = 1024*1024 +""; 


        public static bool audioCheckCompression = true;
        public static bool audioCheckIs3D = true;
        public static bool audioCheckRate = true;
        public static string audioCheckRateValue = "128";


    }

    #region 用于检测的设置
    public class SettingCfg
    {
        public static SettingCfg instance = new SettingCfg();

        public bool textureCheckMemSize = true;
        public int textureCheckMemSizeValue = 0;
        public bool textureCheckPix = true;
        public int textureCheckPixW = 0;
        public int textureCheckPixH = 0;
        public bool textureCheckPix2Pow = true;
        public bool textureCheckIsRW = true;
        public bool textureCheckPlatSetting = true;
        public bool textureCheckIsLightmap = true;


        public bool modelCheckScale = true;
        public bool modelCheckMeshCompression = true;
        public bool modelCheckAnimCompression = true;
        public bool modelCheckMeshIsRW = true;
        public bool modelCheckCollider = true;
        //public bool modelCheckNormals = true;
        //public bool modelCheckTangents = true;
        //public bool modelCheckBakeIK = true;
        //public bool modelCheckFileSize = true;
        //public int modelCheckFileSizeValue = 0; 


        public bool audioCheckCompression = true;
        public bool audioCheckIs3D = true;
        public bool audioCheckRate = true;
        public int audioCheckRateValue = 0;

        public static void Apply(bool hasTip)
        {
            try
            {

                instance.textureCheckMemSize = SettingCfgUI.textureCheckMemSize;
                instance.textureCheckMemSizeValue = int.Parse(SettingCfgUI.textureCheckMemSizeValue);
                instance.textureCheckPix = SettingCfgUI.textureCheckPix;
                instance.textureCheckPixW = int.Parse(SettingCfgUI.textureCheckPixW);
                instance.textureCheckPixH = int.Parse(SettingCfgUI.textureCheckPixH);
                instance.textureCheckPix2Pow = SettingCfgUI.textureCheckPix2Pow;
                instance.textureCheckIsRW = SettingCfgUI.textureCheckIsRW;
                instance.textureCheckPlatSetting = SettingCfgUI.textureCheckPlatSetting;
                instance.textureCheckIsLightmap = SettingCfgUI.textureCheckIsLightmap;


                instance.modelCheckScale = SettingCfgUI.modelCheckScale;
                instance.modelCheckMeshCompression = SettingCfgUI.modelCheckMeshCompression;
                instance.modelCheckAnimCompression = SettingCfgUI.modelCheckAnimCompression;
                instance.modelCheckMeshIsRW = SettingCfgUI.modelCheckMeshIsRW;
                instance.modelCheckCollider = SettingCfgUI.modelCheckCollider;
                //tmpSetting.modelCheckNormals = ProjectCheckSettingUI.modelCheckNormals;
                //tmpSetting.modelCheckTangents = ProjectCheckSettingUI.modelCheckTangents;
                //tmpSetting.modelCheckBakeIK = ProjectCheckSettingUI.modelCheckBakeIK;
                //tmpSetting.modelCheckFileSize = ProjectCheckSettingUI.modelCheckFileSize;
                //tmpSetting.modelCheckFileSizeValue = int.Parse(ProjectCheckSettingUI.modelCheckFileSizeValue);


                instance.audioCheckCompression = SettingCfgUI.audioCheckCompression;
                instance.audioCheckIs3D = SettingCfgUI.audioCheckIs3D;
                instance.audioCheckRate = SettingCfgUI.audioCheckRate;
                instance.audioCheckRateValue = int.Parse(SettingCfgUI.audioCheckRateValue);


                if (hasTip)
                {
                    EditorUtility.DisplayDialog("Tips", "设置成功，请返回详情查看", "OK");
                }
            }
            catch (Exception e)
            {
                if (hasTip)
                {
                    EditorUtility.DisplayDialog("Tips", "设置失败，请关掉窗口重新打开 " + e.Message, "OK");
                }
            }


        }

    }

    #endregion
}
