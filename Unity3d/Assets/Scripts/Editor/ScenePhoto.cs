using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ScenePhoto : EditorWindow {

  private static Vector2 m_WinMinSize = new Vector2(315.0f, 400.0f);
  private static Rect m_WinPosition = new Rect(100.0f, 100.0f, 315.0f, 400.0f);
  private static string DEFAULT_PHOTO_SAVE_PATH = ".";
  private static string DEFAULT_PHOTO_CAMERA_ASSET_PATH = "Assets/Editor/PhotoCamera.prefab";
  public string DebugInfo;

  private Vector3 SceneTopLeft;
  private Vector3 SceneBottomRight;
  private Vector2 PhotoSize;
  private Texture2D Photo;
  private string PhotoSavePath;

  private GameObject PhotoCameraAsset;
  private GameObject PhotoCameraObj;
  private Camera PhotoCamera;

  private bool IsAdvancedConfig = false;
  private bool IsHDR = true;

  [MenuItem("工具/ScenePhoto", false, 1000)]
  private static void Init() {
    ScenePhoto window = EditorWindow.GetWindow<ScenePhoto>("ScenePhoto", true, typeof(EditorWindow));
    window.position = m_WinPosition;
    window.minSize = m_WinMinSize;
    window.wantsMouseMove = true;
    window.Show();

    window.Initialize();
  }
  private void Initialize() {
    SceneTopLeft = Vector3.zero;
    SceneBottomRight = new Vector3(512, 0.0f, 512);
    PhotoSize = new Vector2(1024.0f, 1024.0f);
    PhotoSavePath = DEFAULT_PHOTO_SAVE_PATH + "/Scene.png";
    PhotoCamera = null;
    if (PhotoCameraObj != null) {
      GameObject.DestroyImmediate(PhotoCameraObj);
    }
    if (PhotoCameraAsset != null) {
      PhotoCameraAsset = null;
    }
    if (Photo != null) {
      GameObject.DestroyImmediate(Photo);
      Photo = null;
    }
    while (true) {
      GameObject remainObj = GameObject.Find("PhotoCamera");
      if (remainObj != null) {
        GameObject.DestroyImmediate(remainObj);
      } else {
        break;
      }
    }
  }
  private void OnDestroy() {
    Initialize();
  }
  private void OnGUI() {
    SceneTopLeft = EditorGUILayout.Vector3Field("SceneTopLeft", SceneTopLeft);
    SceneBottomRight = EditorGUILayout.Vector3Field("SceneBottomRight", SceneBottomRight);
    PhotoSize = EditorGUILayout.Vector2Field("PhotoSize", PhotoSize);

    EditorGUILayout.BeginHorizontal();
    PhotoSavePath = EditorGUILayout.TextField("SerizlizeFile:", PhotoSavePath);
    if (GUILayout.Button("Select", GUILayout.MaxWidth(50))) {
      PhotoSavePath = EditorUtility.SaveFilePanel(
        "Select Path to save photo",
        DEFAULT_PHOTO_SAVE_PATH,
        "Scene",
        "png");
    }
    EditorGUILayout.EndHorizontal();
    IsAdvancedConfig = EditorGUILayout.Foldout(IsAdvancedConfig, "AdvancedConfig");
    if (IsAdvancedConfig) {
      IsHDR = EditorGUILayout.Toggle("IsHDR", IsHDR);
    }
    EditorGUILayout.BeginHorizontal();
    if (GUILayout.Button("Take Photo", GUILayout.MaxWidth(80))) {
      TakePhoto();
    }
    EditorGUILayout.EndHorizontal();
    Rect winRect = this.position;
    float textureFildSize = Mathf.Min(winRect.width, winRect.height);
    Photo = EditorGUILayout.ObjectField(
      Photo,
      typeof(Texture),
      false,
      GUILayout.MaxWidth(textureFildSize),
      GUILayout.MaxHeight(textureFildSize)
      ) as Texture2D;
    this.Repaint();
  }
  private void TakePhoto() {
    List<GameObject> airWallMeshes = new List<GameObject>();
    try {
      if (PhotoCameraAsset == null) {
        PhotoCameraAsset = AssetDatabase.LoadAssetAtPath(DEFAULT_PHOTO_CAMERA_ASSET_PATH, typeof(GameObject)) as GameObject;
      }
      if (PhotoCameraObj == null && PhotoCameraAsset != null) {
        PhotoCameraObj = GameObject.Instantiate(PhotoCameraAsset) as GameObject;
        PhotoCamera = PhotoCameraObj.GetComponent<Camera>();
      }

      if (Photo != null) {
        GameObject.DestroyImmediate(Photo);
        Photo = null;
      }

      if (PhotoCamera == null) {
        EditorUtility.DisplayDialog(
          "Error",
          "Photo Camera Miss! Are you miss PhotoCamera.prefab?",
          "OK");
        return;
      }
      Vector3 tSceneSize = SceneBottomRight - SceneTopLeft;
      Vector3 tSceneCenter = (SceneTopLeft + SceneBottomRight) / 2;

      PhotoCamera.orthographic = true;
      PhotoCamera.aspect = 1.0f;
      PhotoCamera.allowHDR = IsHDR;
      PhotoCamera.orthographicSize = Mathf.Max(tSceneSize.x, tSceneSize.y) / 2;
      //PhotoCamera.rect = new Rect(0, 0, tSceneSize.x, tSceneSize.z);
      PhotoCamera.transform.position = tSceneCenter + new Vector3(0, 512.0f, 0);
      PhotoCamera.transform.LookAt(tSceneCenter);

      RenderTexture currentActiveRT = RenderTexture.active;

      RenderTexture tCameraRT = new RenderTexture((int)PhotoSize.x, (int)PhotoSize.y, 24);
      PhotoCamera.targetTexture = tCameraRT;
      RenderTexture.active = tCameraRT;
      PhotoCamera.Render();

      Photo = new Texture2D((int)PhotoSize.x, (int)PhotoSize.y, TextureFormat.RGB24, false);
      Photo.ReadPixels(new Rect(0, 0, (int)PhotoSize.x, (int)PhotoSize.y), 0, 0);
      Photo.Apply();

      RenderTexture.active = null;
      PhotoCamera.targetTexture = null;
      RenderTexture.active = currentActiveRT;

      byte[] bytes;
      bytes = Photo.EncodeToPNG();
      System.IO.File.WriteAllBytes(GetPhotoName(), bytes);
    } catch(Exception ex) {
      UnityEngine.Debug.Log("ScenePhoto.TackPhoto failed.ex:" + ex.Message);
    } finally {
      foreach (GameObject child in airWallMeshes) {
        child.transform.parent = null;
        child.SetActive(false);
        GameObject.DestroyImmediate(child);
      }
    }
  }
  private string GetPhotoName() {
  	return PhotoSavePath;
  	/*
    string tPhotoTimeFormat = "yyyyMMddHHmmss";
    return string.Format("{0}/{1}_{2}.png",
      PhotoSavePath,
      "Photo",
      DateTime.Now.ToString(tPhotoTimeFormat));
    */
  }
}