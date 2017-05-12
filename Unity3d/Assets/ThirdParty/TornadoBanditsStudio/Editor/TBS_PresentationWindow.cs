using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TornadoBanditsStudio
{
    public class TBS_PresentationWindow : EditorWindow
    {
        #region PACKAGE INFOS REGIONS
        private string packageName = "Low Poly Free Pack";
        private string packageDescription = "Firstly we would like to thank you for downloading our package. \nOur package contains the following:\n-25 different meshes\n-More than 25 ready to use prefabs\n-4 demo scenes containing lightning examples\n\nYou can check the documentation file in the project folder (LowPolyFreePack/Docs/Documentation.pdf) or check it online by clicking on the above button.\nDon't forget to stay updated by following our facebook page or visiting our website.\nAlso, if you have any problems or you would like to work with us don't hesitate to contact us.";
        #endregion

        #region LOGO_PATHS
        private const string logoPath = "Assets/TornadoBanditsStudio/Logo/TBSLogo.png";
        private const string facebookLogoPath = "Assets/TornadoBanditsStudio/Logo/facebook_icon.png";
        private const string mailLogoPath = "Assets/TornadoBanditsStudio/Logo/mail_icon.png";
        private const string siteLogoPath = "Assets/TornadoBanditsStudio/Logo/web_icon.png";
        private const string docLogoPath = "Assets/TornadoBanditsStudio/Logo/pdf_icon.png";
        #endregion

        #region LOGOS_TEXTURES
        private static Texture2D logo = null;
        private static Texture2D facebookLogo = null;
        private static Texture2D mailLogo = null;
        private static Texture2D siteLogo = null;
        private static Texture2D docLogo = null;
        #endregion

        #region URLS REGIONS
        private static string FACEBOOK_URL = "http://tinyurl.com/tornadobanditsstudio";
        private static string MAIL_URL = "mailto:tornadobanditsstudio@gmail.com";
        private static string SITE_URL = "www.tornadobanditsstudio.com";
		private static string DOC_URL = "https://tinyurl.com/lowpolyfreepack";
        #endregion

        [MenuItem ("Tornado Bandits Studio/Package Presentation")]
        static void InitializeWindow ()
        {
            // Get existing open window or if none, make a new one:
            TBS_PresentationWindow presentationWindow = (TBS_PresentationWindow)EditorWindow.GetWindowWithRect (typeof (TBS_PresentationWindow), new Rect (0, 0, 512, 600), true, "Package Presentation");
            presentationWindow.Show ();
        }

        void OnEnable ()
        {
            if (logo == null)
            {
                logo = (Texture2D)AssetDatabase.LoadAssetAtPath (logoPath, typeof (Texture2D));
                facebookLogo = (Texture2D)AssetDatabase.LoadAssetAtPath (facebookLogoPath, typeof (Texture2D));
                mailLogo = (Texture2D)AssetDatabase.LoadAssetAtPath (mailLogoPath, typeof (Texture2D));
                docLogo = (Texture2D)AssetDatabase.LoadAssetAtPath (docLogoPath, typeof (Texture2D));
                siteLogo = (Texture2D)AssetDatabase.LoadAssetAtPath (siteLogoPath, typeof (Texture2D));
            }
        }

        void OnGUI ()
        {
       
            //Set gui skins
            SetGUISkins ();

            //Apply a texture on our editor window and apply it
            Texture2D editorWindowTexture = new Texture2D (1, 1, TextureFormat.RGBA32, false);
            editorWindowTexture.SetPixel (0, 0, new Color (47/255f, 79f/255f, 79/255f, 1f));
            editorWindowTexture.Apply ();
            GUI.DrawTexture (new Rect (0, 0, maxSize.x, maxSize.y), editorWindowTexture, ScaleMode.StretchToFill);

            //Logo part
            GUILayout.BeginVertical ();
            GUILayout.Label (logo, logoGUISkin, GUILayout.Height (256f));
            GUILayout.EndVertical ();

            //Space
            GUILayout.Space (15);

            //Documentation part
            GUILayout.BeginVertical ();
            GUILayout.Label (packageName + ":", packageNameGUIStyle);
            GUILayout.Space (3);
            GUILayout.Label (packageDescription, packageDescriptionGUIStyle, GUILayout.MaxWidth (450));
            GUILayout.EndVertical ();

            //Space
            GUILayout.Space (3);

            GUI.backgroundColor = Color.clear;
            //Buttons part
            GUILayout.BeginHorizontal ();
            GUILayout.Space (90);
            if (GUILayout.Button (facebookLogo, GUILayout.Width (64), GUILayout.Height (64)))
                Application.OpenURL (FACEBOOK_URL);
            GUILayout.Space (15);
            if (GUILayout.Button (siteLogo, GUILayout.Width (64), GUILayout.Height (64)))
                Application.OpenURL (SITE_URL);
            GUILayout.Space (15);
            if (GUILayout.Button (mailLogo, GUILayout.Width (64), GUILayout.Height (64)))
                Application.OpenURL (MAIL_URL);
            GUILayout.Space (15);
            if (GUILayout.Button (docLogo, buttonGUIStyle, GUILayout.Width (64), GUILayout.Height (64)))
                Application.OpenURL (DOC_URL);
            GUILayout.EndHorizontal ();
        }

        #region GUI_SKINS
        private GUIStyle logoGUISkin;
        private GUIStyle packageNameGUIStyle;
        private GUIStyle packageDescriptionGUIStyle;
        private GUIStyle buttonGUIStyle;

        void SetGUISkins ()
        {
            //Logo gui style
            logoGUISkin = new GUIStyle (GUI.skin.label);
            logoGUISkin.imagePosition = ImagePosition.ImageOnly;
            logoGUISkin.alignment = TextAnchor.MiddleCenter;
            logoGUISkin.stretchHeight = true;
            logoGUISkin.stretchWidth = true;


            //Package name gui style
            packageNameGUIStyle = new GUIStyle (GUI.skin.label);
            packageNameGUIStyle.alignment = TextAnchor.MiddleLeft;
            packageNameGUIStyle.contentOffset = new Vector2 (25, 0);
            packageNameGUIStyle.fontSize = 15;
            packageNameGUIStyle.normal.textColor = Color.white;
            packageNameGUIStyle.fontStyle = FontStyle.Bold;
            packageNameGUIStyle.stretchWidth = true;

            //Package description
            packageDescriptionGUIStyle = new GUIStyle (GUI.skin.label);
            packageDescriptionGUIStyle.alignment = TextAnchor.MiddleLeft;
            packageDescriptionGUIStyle.contentOffset = new Vector2 (25, 0);
            packageDescriptionGUIStyle.fontSize = 13;
            packageDescriptionGUIStyle.fontStyle = FontStyle.Normal;
            packageDescriptionGUIStyle.normal.textColor = Color.white;
            packageDescriptionGUIStyle.wordWrap = true;

            //Button gui style
            buttonGUIStyle = new GUIStyle (GUI.skin.button);
        
        }
        #endregion
    }
}
