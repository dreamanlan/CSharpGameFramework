// Resource Checker
// (c) 2012 Simon Oliver / HandCircus / hello@handcircus.com
// Public domain, do with whatever you like, commercial or not
// This comes with no warranty, use at your own risk!
// https://github.com/handcircus/Unity-Resource-Checker

using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemInSceneInfo
{
  public string path;
  public int usedCount;
  public int usedCount1;

  public List<int> FoundInRenderersHierarchy = new List<int>();
  public List<string> FoundInMeshFilters = new List<string>();
  public List<string> FoundInSkinnedMeshRenderer = new List<string>();
}

public class ItemInScene
{
  public List<ItemInSceneInfo> itemInfo = new List<ItemInSceneInfo>();

  public Texture texture;
  public bool isCubeMap;
  public int memSizeKB;
  public TextureFormat format;
  public int mipMapCount;
  public List<Object> FoundInMaterials = new List<Object>();

  public Material material;
  public Mesh mesh;
}

public class TextureDetails
{
  public bool isCubeMap;
  public int memSizeKB;
  public Texture texture;
  public TextureFormat format;
  public int mipMapCount;
  public List<Object> FoundInMaterials = new List<Object>();
  public List<Object> FoundInRenderers = new List<Object>();

  public TextureDetails()
  {
  }
};

public class MaterialDetails
{
  public Material material;
  public List<Renderer> FoundInRenderers = new List<Renderer>();

  public MaterialDetails()
  {
  }
};

public class MeshDetails
{
  public Mesh mesh;
  public List<MeshFilter> FoundInMeshFilters = new List<MeshFilter>();
  public List<SkinnedMeshRenderer> FoundInSkinnedMeshRenderer = new List<SkinnedMeshRenderer>();

  public MeshDetails()
  {
  }
};

public class ResourceChecker : EditorWindow
{

  string[] inspectToolbarStrings = { "Textures", "Materials", "Meshes" };

  enum InspectType
  {
    Textures, Materials, Meshes
  };

  InspectType ActiveInspectType = InspectType.Textures;

  float ThumbnailWidth = 40;
  float ThumbnailHeight = 40;

  List<TextureDetails> ActiveTextures = new List<TextureDetails>();
  List<MaterialDetails> ActiveMaterials = new List<MaterialDetails>();
  List<MeshDetails> ActiveMeshDetails = new List<MeshDetails>();

  List<ItemInScene> SceneTextures = new List<ItemInScene>();
  List<ItemInScene> SceneMaterials = new List<ItemInScene>();
  List<ItemInScene> SceneMeshes = new List<ItemInScene>();

  Vector2 textureListScrollPos = new Vector2(0, 0);
  Vector2 materialListScrollPos = new Vector2(0, 0);
  Vector2 meshListScrollPos = new Vector2(0, 0);

  int TotalTextureMemory = 0;
  int TotalMeshVertices = 0;
  bool ctrlPressed = false;

  static int MinWidth = 455;
  static bool allScene = false;

  [MenuItem("ProjectAnalyzer/Used Resource Checker")]
  static void Init()
  {
    ResourceChecker window = (ResourceChecker)EditorWindow.GetWindow(typeof(ResourceChecker));
    window.CheckCurrentSceneResources();
    window.minSize = new Vector2(MinWidth, 300);
  }

  void OnGUI()
  {
    #region refresh resources
    GUILayout.BeginHorizontal();
    allScene = GUILayout.Toggle(allScene, "All Scene");
    if (GUILayout.Button("Refresh")) {
      if (allScene) {
        CheckAllSceneResources();
      } else {
        CheckCurrentSceneResources();
      }
    }
    GUILayout.EndHorizontal();
    #endregion

    #region show resources

    ctrlPressed = Event.current.control || Event.current.command;

    GUILayout.BeginHorizontal();
    GUILayout.Label("Materials " + ActiveMaterials.Count);
    GUILayout.Label("Textures " + ActiveTextures.Count + " - " + FormatSizeString(TotalTextureMemory));
    GUILayout.Label("Meshes " + ActiveMeshDetails.Count + " - " + TotalMeshVertices + " verts");
    GUILayout.EndHorizontal();

    ActiveInspectType = (InspectType)GUILayout.Toolbar((int)ActiveInspectType, inspectToolbarStrings);

    switch (ActiveInspectType) {
      case InspectType.Textures:
        if (allScene) {
          ListAllSceneTextures();
        } else {
          ListCurrentSceneTextures();
        }
        break;
      case InspectType.Materials:
        if (allScene) {
          ListAllSceneMaterials();
        } else {
          ListCurrentSceneMaterials();
        }
        break;
      case InspectType.Meshes:
        if (allScene) {
          ListAllSceneMeshes();
        } else {
          ListCurrentSceneMeshes();
        }
        break;
    }
    #endregion
  }

  List<Texture> GetSelectedTexture()
  {
    List<Texture> textures = new List<Texture>();

    if (Selection.objects != null && Selection.objects.Length > 0) {
      Object[] objects = EditorUtility.CollectDependencies(Selection.objects);

      foreach (Object o in objects) {
        Texture tex = o as Texture;
        if (tex == null) continue;
        textures.Add(tex); break;
      }
    }
    return textures;
  }

  void CheckCurrentSceneResources()
  {
    ActiveTextures.Clear();
    ActiveMaterials.Clear();
    ActiveMeshDetails.Clear();

    Renderer[] renderers = (Renderer[])FindObjectsOfType(typeof(Renderer));
    //Debug.Log("Total renderers "+renderers.Length);
    foreach (Renderer renderer in renderers) {
      //Debug.Log("Renderer is "+renderer.name);
      foreach (Material material in renderer.sharedMaterials) {
        if (material == null) {
          continue;
        }
        MaterialDetails tMaterialDetails = FindMaterialDetails(material);
        if (tMaterialDetails == null) {
          tMaterialDetails = new MaterialDetails();
          tMaterialDetails.material = material;
          ActiveMaterials.Add(tMaterialDetails);
        }
        tMaterialDetails.FoundInRenderers.Add(renderer);
      }
    }

    foreach (MaterialDetails tMaterialDetails in ActiveMaterials) {
      Material tMaterial = tMaterialDetails.material;
      var dependencies = EditorUtility.CollectDependencies(new UnityEngine.Object[] { tMaterial });
      foreach (Object obj in dependencies) {
        if (obj is Texture) {
          Texture tTexture = obj as Texture;
          string texPath = AssetDatabase.GetAssetPath(tTexture);
          if (!texPath.Contains("Brush") && !texPath.Contains("MapData_")) {
            var tTextureDetail = GetTextureDetail(tTexture, tMaterial, tMaterialDetails);
            if (!ActiveTextures.Contains(tTextureDetail))
              ActiveTextures.Add(tTextureDetail);
          }
        }
      }

      //if the texture was downloaded, it won't be included in the editor dependencies
      if (tMaterial.mainTexture != null && !dependencies.Contains(tMaterial.mainTexture)) {
        var tTextureDetail = GetTextureDetail(tMaterial.mainTexture, tMaterial, tMaterialDetails);
        ActiveTextures.Add(tTextureDetail);
      }
    }


    MeshFilter[] meshFilters = (MeshFilter[])FindObjectsOfType(typeof(MeshFilter));

    foreach (MeshFilter tMeshFilter in meshFilters) {
      Mesh tMesh = tMeshFilter.sharedMesh;
      if (tMesh != null) {
        MeshDetails tMeshDetails = FindMeshDetails(tMesh);
        if (tMeshDetails == null) {
          tMeshDetails = new MeshDetails();
          tMeshDetails.mesh = tMesh;
          ActiveMeshDetails.Add(tMeshDetails);
        }
        tMeshDetails.FoundInMeshFilters.Add(tMeshFilter);
      }
    }

    SkinnedMeshRenderer[] skinnedMeshRenderers = (SkinnedMeshRenderer[])FindObjectsOfType(typeof(SkinnedMeshRenderer));

    foreach (SkinnedMeshRenderer tSkinnedMeshRenderer in skinnedMeshRenderers) {
      Mesh tMesh = tSkinnedMeshRenderer.sharedMesh;
      if (tMesh != null) {
        MeshDetails tMeshDetails = FindMeshDetails(tMesh);
        if (tMeshDetails == null) {
          tMeshDetails = new MeshDetails();
          tMeshDetails.mesh = tMesh;
          ActiveMeshDetails.Add(tMeshDetails);
        }
        tMeshDetails.FoundInSkinnedMeshRenderer.Add(tSkinnedMeshRenderer);
      }
    }


    TotalTextureMemory = 0;
    foreach (TextureDetails tTextureDetails in ActiveTextures) TotalTextureMemory += tTextureDetails.memSizeKB;

    TotalMeshVertices = 0;
    foreach (MeshDetails tMeshDetails in ActiveMeshDetails) TotalMeshVertices += tMeshDetails.mesh.vertexCount;

    // Sort by size, descending
    ActiveTextures.Sort(delegate(TextureDetails details1, TextureDetails details2) { return details2.memSizeKB - details1.memSizeKB; });
    ActiveMeshDetails.Sort(delegate(MeshDetails details1, MeshDetails details2) { return details2.mesh.vertexCount - details1.mesh.vertexCount; });

  }

  int selCurrentSceneTextureGridInt = -1;
  string[] selStrings = new string[] { "Use", "Name" };
  void ListCurrentSceneTextures()
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label("Sort by", GUILayout.Width(100));
    int oldGridInt = selCurrentSceneTextureGridInt;
    selCurrentSceneTextureGridInt = GUILayout.SelectionGrid(selCurrentSceneTextureGridInt, selStrings, selStrings.Length, GUILayout.Width(100));
    if (oldGridInt != selCurrentSceneTextureGridInt) {
      oldGridInt = selCurrentSceneTextureGridInt;
      switch (selCurrentSceneTextureGridInt) {
        case 0: {
            ActiveTextures.Sort(delegate(TextureDetails info1, TextureDetails info2) { return info2.FoundInRenderers.Count - info1.FoundInRenderers.Count; });
          } break;
        case 1: {
            ActiveTextures.Sort(delegate(TextureDetails info1, TextureDetails info2) { return info1.texture.name.CompareTo(info2.texture.name); });
          } break;
      }
    }

    GUILayout.EndHorizontal();

    textureListScrollPos = EditorGUILayout.BeginScrollView(textureListScrollPos);

    foreach (TextureDetails tDetails in ActiveTextures) {

      GUILayout.BeginHorizontal();
      GUILayout.Box(tDetails.texture, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));

      if (GUILayout.Button(tDetails.texture.name, GUILayout.Width(200))) {
        SelectObject(tDetails.texture, ctrlPressed);
      }

      string sizeLabel = "" + tDetails.texture.width + "x" + tDetails.texture.height;
      if (tDetails.isCubeMap) sizeLabel += "x6";
      sizeLabel += " - " + tDetails.mipMapCount + "mip";
      sizeLabel += "\n" + FormatSizeString(tDetails.memSizeKB) + " - " + tDetails.format + "";

      GUILayout.Label(sizeLabel, GUILayout.Width(120));

      if (GUILayout.Button(tDetails.FoundInMaterials.Count + " Mat", GUILayout.Width(50))) {
        SelectObjects(tDetails.FoundInMaterials, ctrlPressed);
      }

      if (GUILayout.Button(tDetails.FoundInRenderers.Count + " GO", GUILayout.Width(50))) {
        List<Object> FoundObjects = new List<Object>();
        foreach (Renderer renderer in tDetails.FoundInRenderers) FoundObjects.Add(renderer.gameObject);
        SelectObjects(FoundObjects, ctrlPressed);
      }

      GUILayout.EndHorizontal();
    }
    if (ActiveTextures.Count > 0) {
      GUILayout.BeginHorizontal();
      GUILayout.Box(" ", GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));

      if (GUILayout.Button("Select All", GUILayout.Width(150))) {
        List<Object> AllTextures = new List<Object>();
        foreach (TextureDetails tDetails in ActiveTextures) AllTextures.Add(tDetails.texture);
        SelectObjects(AllTextures, ctrlPressed);
      }
      EditorGUILayout.EndHorizontal();
    }
    EditorGUILayout.EndScrollView();
  }

  int selCurrentSceneMaterialGridInt = -1;
  void ListCurrentSceneMaterials()
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label("Sort by", GUILayout.Width(100));
    int oldGridInt = selCurrentSceneMaterialGridInt;
    selCurrentSceneMaterialGridInt = GUILayout.SelectionGrid(selCurrentSceneMaterialGridInt, selStrings, selStrings.Length, GUILayout.Width(100));
    if (oldGridInt != selCurrentSceneMaterialGridInt) {
      oldGridInt = selCurrentSceneMaterialGridInt;
      switch (selCurrentSceneMaterialGridInt) {
        case 0: {
            ActiveMaterials.Sort(delegate(MaterialDetails info1, MaterialDetails info2) { return info2.FoundInRenderers.Count - info1.FoundInRenderers.Count; });
          } break;
        case 1: {
            ActiveMaterials.Sort(delegate(MaterialDetails info1, MaterialDetails info2) { return info1.material.name.CompareTo(info2.material.name); });
          } break;
      }
    }
    EditorGUILayout.EndHorizontal();

    materialListScrollPos = EditorGUILayout.BeginScrollView(materialListScrollPos);

    foreach (MaterialDetails tDetails in ActiveMaterials) {
      if (tDetails.material != null) {
        GUILayout.BeginHorizontal();

        if (tDetails.material.mainTexture != null) GUILayout.Box(tDetails.material.mainTexture, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
        else {
          GUILayout.Box("n/a", GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
        }

        if (GUILayout.Button(tDetails.material.name, GUILayout.Width(150))) {
          SelectObject(tDetails.material, ctrlPressed);
        }

        string shaderLabel = tDetails.material.shader != null ? tDetails.material.shader.name : "no shader";
        GUILayout.Label(shaderLabel, GUILayout.Width(200));

        if (GUILayout.Button(tDetails.FoundInRenderers.Count + " GO", GUILayout.Width(50))) {
          List<Object> FoundObjects = new List<Object>();
          foreach (Renderer renderer in tDetails.FoundInRenderers) FoundObjects.Add(renderer.gameObject);
          SelectObjects(FoundObjects, ctrlPressed);
        }


        GUILayout.EndHorizontal();
      }
    }
    EditorGUILayout.EndScrollView();
  }

  int selCurrentSceneMeshGridInt = -1;
  void ListCurrentSceneMeshes()
  {

    GUILayout.BeginHorizontal();
    GUILayout.Label("Sort by", GUILayout.Width(100));
    int oldGridInt = selCurrentSceneMeshGridInt;
    selCurrentSceneMeshGridInt = GUILayout.SelectionGrid(selCurrentSceneMeshGridInt, selStrings, selStrings.Length, GUILayout.Width(100));
    if (oldGridInt != selCurrentSceneMeshGridInt) {
      oldGridInt = selCurrentSceneMeshGridInt;
      switch (selCurrentSceneMeshGridInt) {
        case 0: {
            ActiveMeshDetails.Sort(delegate(MeshDetails info1, MeshDetails info2) { return info2.FoundInMeshFilters.Count - info1.FoundInMeshFilters.Count; });
          } break;
        case 1: {
            ActiveMeshDetails.Sort(delegate(MeshDetails info1, MeshDetails info2) { return info1.mesh.name.CompareTo(info2.mesh.name); });
          } break;
      }
    }
    EditorGUILayout.EndHorizontal();

    meshListScrollPos = EditorGUILayout.BeginScrollView(meshListScrollPos);

    foreach (MeshDetails tDetails in ActiveMeshDetails) {
      if (tDetails.mesh != null) {
        GUILayout.BeginHorizontal();
        /*
        if (tDetails.material.mainTexture!=null) GUILayout.Box(tDetails.material.mainTexture, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
        else	
        {
            GUILayout.Box("n/a",GUILayout.Width(ThumbnailWidth),GUILayout.Height(ThumbnailHeight));
        }
        */

        if (GUILayout.Button(tDetails.mesh.name, GUILayout.Width(150))) {
          SelectObject(tDetails.mesh, ctrlPressed);
        }
        string sizeLabel = "" + tDetails.mesh.vertexCount + " vert";

        GUILayout.Label(sizeLabel, GUILayout.Width(100));


        if (GUILayout.Button(tDetails.FoundInMeshFilters.Count + " GO", GUILayout.Width(50))) {
          List<Object> FoundObjects = new List<Object>();
          foreach (MeshFilter meshFilter in tDetails.FoundInMeshFilters) FoundObjects.Add(meshFilter.gameObject);
          SelectObjects(FoundObjects, ctrlPressed);
        }

        if (GUILayout.Button(tDetails.FoundInSkinnedMeshRenderer.Count + " GO", GUILayout.Width(50))) {
          List<Object> FoundObjects = new List<Object>();
          foreach (SkinnedMeshRenderer skinnedMeshRenderer in tDetails.FoundInSkinnedMeshRenderer) FoundObjects.Add(skinnedMeshRenderer.gameObject);
          SelectObjects(FoundObjects, ctrlPressed);
        }


        GUILayout.EndHorizontal();
      }
    }
    EditorGUILayout.EndScrollView();
  }

  TextureDetails FindTextureDetails(Texture tTexture)
  {
    foreach (TextureDetails tTextureDetails in ActiveTextures) {
      if (tTextureDetails.texture == tTexture) return tTextureDetails;
    }
    return null;

  }

  MaterialDetails FindMaterialDetails(Material tMaterial)
  {
    foreach (MaterialDetails tMaterialDetails in ActiveMaterials) {
      if (tMaterialDetails.material == tMaterial) return tMaterialDetails;
    }
    return null;

  }

  MeshDetails FindMeshDetails(Mesh tMesh)
  {
    foreach (MeshDetails tMeshDetails in ActiveMeshDetails) {
      if (tMeshDetails.mesh == tMesh) return tMeshDetails;
    }
    return null;

  }

  void CheckAllSceneResources()
  {
    SceneTextures.Clear();
    SceneMaterials.Clear();
    SceneMeshes.Clear();

    foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes) {
      if (scene.enabled) {
        EditorApplication.OpenScene(scene.path);
        CheckCurrentSceneResources();

        foreach (var tex in ActiveTextures) {
          string texPath = AssetDatabase.GetAssetPath(tex.texture);
          if (!texPath.Contains("Brush") && !texPath.Contains("MapData_")) {
            var tTextureInScene = FindTextureInScene(tex.texture);
            if (tTextureInScene == null) {
              tTextureInScene = new ItemInScene();
              SceneTextures.Add(tTextureInScene);

              tTextureInScene.isCubeMap = tex.isCubeMap;
              tTextureInScene.format = tex.format;
              tTextureInScene.memSizeKB = tex.memSizeKB;
              tTextureInScene.mipMapCount = tex.mipMapCount;
              tTextureInScene.texture = tex.texture;
            }
            var itemInfo = FindItemInSceneInfo(tTextureInScene, scene.path);
            if (itemInfo == null) {
              itemInfo = new ItemInSceneInfo();
              itemInfo.path = scene.path;
              tTextureInScene.itemInfo.Add(itemInfo);
            }
            itemInfo.usedCount += tex.FoundInRenderers.Count;

            foreach (var mat in tex.FoundInMaterials) {
              if (!tTextureInScene.FoundInMaterials.Contains(mat)) {
                tTextureInScene.FoundInMaterials.Add(mat);
              }
            }
          }
        }

        foreach (var ma in ActiveMaterials) {
          var tMaterialInScene = FindMaterialInScene(ma.material);
          if (tMaterialInScene == null) {
            tMaterialInScene = new ItemInScene();
            SceneMaterials.Add(tMaterialInScene);
            tMaterialInScene.material = ma.material;
          }
          var itemInfo = FindItemInSceneInfo(tMaterialInScene, scene.path);
          if (itemInfo == null) {
            itemInfo = new ItemInSceneInfo();
            itemInfo.path = scene.path;
            tMaterialInScene.itemInfo.Add(itemInfo);
          }
          itemInfo.usedCount += ma.FoundInRenderers.Count;
        }

        foreach (var me in ActiveMeshDetails) {
          var tMeshInScene = FindMeshInScene(me.mesh);
          if (tMeshInScene == null) {
            tMeshInScene = new ItemInScene();
            SceneMeshes.Add(tMeshInScene);
            tMeshInScene.mesh = me.mesh;
          }
          var itemInfo = FindItemInSceneInfo(tMeshInScene, scene.path);
          if (itemInfo == null) {
            itemInfo = new ItemInSceneInfo();
            itemInfo.path = scene.path;
            tMeshInScene.itemInfo.Add(itemInfo);
          }
          itemInfo.usedCount += me.FoundInMeshFilters.Count;
          itemInfo.usedCount1 += me.FoundInSkinnedMeshRenderer.Count;
        }
      }
    }
    SceneMaterials.Sort(delegate(ItemInScene info1, ItemInScene info2) { return info2.itemInfo.Count - info1.itemInfo.Count; });
    SceneMeshes.Sort(delegate(ItemInScene info1, ItemInScene info2) { return info2.itemInfo.Count - info1.itemInfo.Count; });
  }

  private ItemInSceneInfo FindItemInSceneInfo(ItemInScene itemInScene, string scenePath)
  {
    foreach (var info in itemInScene.itemInfo) {
      if (info.path == scenePath) {
        return info;
      }
    }
    return null;
  }



  private ItemInScene currentInfo;

  int selAllSceneTextureGridInt = -1;

  string[] selTexturesStrings = new string[] { "Use", "Name", "Format" };
  private void ListAllSceneTextures()
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label("Sort by", GUILayout.Width(100));
    int oldGridInt = selAllSceneTextureGridInt;
    selAllSceneTextureGridInt = GUILayout.SelectionGrid(selAllSceneTextureGridInt, selTexturesStrings, selStrings.Length, GUILayout.Width(100));
    if (oldGridInt != selAllSceneTextureGridInt) {
      oldGridInt = selAllSceneTextureGridInt;
      switch (selAllSceneTextureGridInt) {
        case 0: {
            SceneTextures.Sort(delegate(ItemInScene info1, ItemInScene info2) {
              if (info2.FoundInMaterials.Count != info1.FoundInMaterials.Count)
                return info2.FoundInMaterials.Count - info1.FoundInMaterials.Count;
              else {
                int info1Go = 0;
                foreach (var it in info1.itemInfo) {
                  info1Go += it.usedCount;
                }
                int info2Go = 0;
                foreach (var it in info2.itemInfo) {
                  info2Go += it.usedCount;
                }

                return info2Go - info1Go;
              }
            });
          } break;
        case 1: {
            SceneTextures.Sort(delegate(ItemInScene info1, ItemInScene info2) { return info1.texture.name.CompareTo(info2.texture.name); });
          } break;
        case 2: {
            SceneTextures.Sort(delegate(ItemInScene info1, ItemInScene info2) { return info1.format.CompareTo(info2.format); });
          } break;
      }
    }
    EditorGUILayout.EndHorizontal();

    textureListScrollPos = EditorGUILayout.BeginScrollView(textureListScrollPos);

    foreach (var sceneTexture in SceneTextures) {
      GUILayout.BeginHorizontal();
      GUILayout.Box(sceneTexture.texture, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));

      if (GUILayout.Button(sceneTexture.texture.name, GUILayout.Width(150))) {
        SelectObject(sceneTexture.texture, ctrlPressed);
      }

      string sizeLabel = "" + sceneTexture.texture.width + "x" + sceneTexture.texture.height;
      if (sceneTexture.isCubeMap) sizeLabel += "x6";
      sizeLabel += " - " + sceneTexture.mipMapCount + "mip";
      sizeLabel += "\n" + FormatSizeString(sceneTexture.memSizeKB) + " - " + sceneTexture.format + "";

      GUILayout.Label(sizeLabel, GUILayout.Width(120));
      GUILayout.Label(sceneTexture.itemInfo.Count + " Sce", GUILayout.Width(50));

      if (GUILayout.Button(sceneTexture.FoundInMaterials.Count + " Mat", GUILayout.Width(50))) {
        SelectObjects(sceneTexture.FoundInMaterials, ctrlPressed);
      }

      GUILayout.BeginVertical();
      foreach (var info in sceneTexture.itemInfo) {
        GUILayout.BeginHorizontal();

        ItemInSceneInfo itemInSceneInfo = sceneTexture.itemInfo.Find(delegate(ItemInSceneInfo obj) { return obj.path == info.path; });
        if (GUILayout.Button(itemInSceneInfo.usedCount + " GO", GUILayout.Width(50))) {
          EditorApplication.OpenScene(info.path);
          CheckCurrentSceneResources();

          foreach (var tDetails in ActiveTextures) {
            if (tDetails.texture == sceneTexture.texture) {
              List<Object> FoundObjects = new List<Object>();
              foreach (Renderer renderer in tDetails.FoundInRenderers) FoundObjects.Add(renderer.gameObject);
              SelectObjects(FoundObjects, ctrlPressed);
            }
          }
        }
        GUILayout.Label(info.path);

        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();

      GUILayout.EndHorizontal();
    }

    EditorGUILayout.EndScrollView();
  }

  private void ListAllSceneMaterials()
  {
    materialListScrollPos = EditorGUILayout.BeginScrollView(materialListScrollPos);

    foreach (var sceneMaterial in SceneMaterials) {
      if (sceneMaterial.material != null) {
        GUILayout.BeginHorizontal();

        if (sceneMaterial.material.mainTexture != null) GUILayout.Box(sceneMaterial.material.mainTexture, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
        else {
          GUILayout.Box("n/a", GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
        }

        if (GUILayout.Button(sceneMaterial.material.name, GUILayout.Width(150))) {
          SelectObject(sceneMaterial.material, ctrlPressed);
        }

        string shaderLabel = sceneMaterial.material.shader != null ? sceneMaterial.material.shader.name : "no shader";
        GUILayout.Label(shaderLabel, GUILayout.Width(200));

        GUILayout.Label(sceneMaterial.itemInfo.Count + " Sce", GUILayout.Width(50));

        GUILayout.BeginVertical();
        foreach (var info in sceneMaterial.itemInfo) {
          GUILayout.BeginHorizontal();

          ItemInSceneInfo itemInSceneInfo = sceneMaterial.itemInfo.Find(delegate(ItemInSceneInfo obj) { return obj.path == info.path; });
          if (GUILayout.Button(itemInSceneInfo.usedCount + " GO", GUILayout.Width(50))) {
            EditorApplication.OpenScene(info.path);
            CheckCurrentSceneResources();

            foreach (var tDetails in ActiveMaterials) {
              if (tDetails.material == sceneMaterial.material) {
                List<Object> FoundObjects = new List<Object>();
                foreach (Renderer renderer in tDetails.FoundInRenderers) FoundObjects.Add(renderer.gameObject);
                SelectObjects(FoundObjects, ctrlPressed);
              }
            }
          }
          GUILayout.Label(info.path);

          GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
      }
    }
    EditorGUILayout.EndScrollView();
  }

  private void ListAllSceneMeshes()
  {
    meshListScrollPos = EditorGUILayout.BeginScrollView(meshListScrollPos);

    foreach (var sceneMesh in SceneMeshes) {
      if (sceneMesh.mesh != null) {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(sceneMesh.mesh.name, GUILayout.Width(150))) {
          SelectObject(sceneMesh.mesh, ctrlPressed);
        }
        string sizeLabel = "" + sceneMesh.mesh.vertexCount + " vert";

        GUILayout.Label(sizeLabel, GUILayout.Width(100));

        if (GUILayout.Button(sceneMesh.itemInfo.Count + " Sce", GUILayout.Width(50))) {
          if (currentInfo != null) {
            currentInfo = null;
          } else {
            currentInfo = sceneMesh;
          }
        }
        GUILayout.BeginVertical();
        foreach (var info in sceneMesh.itemInfo) {
          GUILayout.BeginHorizontal();

          ItemInSceneInfo itemInSceneInfo = sceneMesh.itemInfo.Find(delegate(ItemInSceneInfo obj) { return obj.path == info.path; });
          if (GUILayout.Button(itemInSceneInfo.usedCount + " GO", GUILayout.Width(50))) {
            EditorApplication.OpenScene(info.path);
            CheckCurrentSceneResources();

            foreach (var tDetails in ActiveMeshDetails) {
              if (tDetails.mesh == sceneMesh.mesh) {
                List<Object> FoundObjects = new List<Object>();
                foreach (MeshFilter meshFilter in tDetails.FoundInMeshFilters) FoundObjects.Add(meshFilter.gameObject);
                SelectObjects(FoundObjects, ctrlPressed);
              }
            }
          }

          if (GUILayout.Button(itemInSceneInfo.usedCount1 + " GO", GUILayout.Width(50))) {
            EditorApplication.OpenScene(info.path);
            CheckCurrentSceneResources();

            foreach (var tDetails in ActiveMeshDetails) {
              if (tDetails.mesh == sceneMesh.mesh) {
                List<Object> FoundObjects = new List<Object>();
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in tDetails.FoundInSkinnedMeshRenderer) FoundObjects.Add(skinnedMeshRenderer.gameObject);
                SelectObjects(FoundObjects, ctrlPressed);
              }
            }
          }
          GUILayout.Label(info.path);

          GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
      }
    }
    EditorGUILayout.EndScrollView();
  }

  #region function
  ItemInScene FindTextureInScene(Texture tTexture)
  {
    foreach (var tTextureDetails in SceneTextures) {
      if (tTextureDetails.texture == tTexture) return tTextureDetails;
    }
    return null;
  }

  ItemInScene FindMaterialInScene(Material tMaterial)
  {
    foreach (var tMaterialDetails in SceneMaterials) {
      if (tMaterialDetails.material == tMaterial) return tMaterialDetails;
    }
    return null;
  }

  ItemInScene FindMeshInScene(Mesh tMesh)
  {
    foreach (var tMeshDetails in SceneMeshes) {
      if (tMeshDetails.mesh == tMesh) return tMeshDetails;
    }
    return null;
  }

  private TextureDetails GetTextureDetail(Texture tTexture, Material tMaterial, MaterialDetails tMaterialDetails)
  {
    TextureDetails tTextureDetails = FindTextureDetails(tTexture);
    if (tTextureDetails == null) {
      tTextureDetails = new TextureDetails();
      tTextureDetails.texture = tTexture;
      tTextureDetails.isCubeMap = tTexture is Cubemap;

      int memSize = CalculateTextureSizeBytes(tTexture);

      tTextureDetails.memSizeKB = memSize / 1024;
      TextureFormat tFormat = TextureFormat.RGBA32;
      int tMipMapCount = 1;
      if (tTexture is Texture2D) {
        tFormat = (tTexture as Texture2D).format;
        tMipMapCount = (tTexture as Texture2D).mipmapCount;
      }
      if (tTexture is Cubemap) {
        tFormat = (tTexture as Cubemap).format;
      }

      tTextureDetails.format = tFormat;
      tTextureDetails.mipMapCount = tMipMapCount;

    }
    tTextureDetails.FoundInMaterials.Add(tMaterial);
    foreach (Renderer renderer in tMaterialDetails.FoundInRenderers) {
      if (!tTextureDetails.FoundInRenderers.Contains(renderer)) tTextureDetails.FoundInRenderers.Add(renderer);
    }
    return tTextureDetails;
  }

  int GetBitsPerPixel(TextureFormat format)
  {
    switch (format) {
      case TextureFormat.Alpha8: //	 Alpha-only texture format.
        return 8;
      case TextureFormat.ARGB4444: //	 A 16 bits/pixel texture format. Texture stores color with an alpha channel.
        return 16;
      case TextureFormat.RGBA4444: //	 A 16 bits/pixel texture format.
        return 16;
      case TextureFormat.RGB24:	// A color texture format.
        return 24;
      case TextureFormat.RGBA32:	//Color with an alpha channel texture format.
        return 32;
      case TextureFormat.ARGB32:	//Color with an alpha channel texture format.
        return 32;
      case TextureFormat.RGB565:	//	 A 16 bit color texture format.
        return 16;
      case TextureFormat.DXT1:	// Compressed color texture format.
        return 4;
      case TextureFormat.DXT5:	// Compressed color with alpha channel texture format.
        return 8;
      /*
      case TextureFormat.WiiI4:	// Wii texture format.
      case TextureFormat.WiiI8:	// Wii texture format. Intensity 8 bit.
      case TextureFormat.WiiIA4:	// Wii texture format. Intensity + Alpha 8 bit (4 + 4).
      case TextureFormat.WiiIA8:	// Wii texture format. Intensity + Alpha 16 bit (8 + 8).
      case TextureFormat.WiiRGB565:	// Wii texture format. RGB 16 bit (565).
      case TextureFormat.WiiRGB5A3:	// Wii texture format. RGBA 16 bit (4443).
      case TextureFormat.WiiRGBA8:	// Wii texture format. RGBA 32 bit (8888).
      case TextureFormat.WiiCMPR:	//	 Compressed Wii texture format. 4 bits/texel, ~RGB8A1 (Outline alpha is not currently supported).
          return 0;  //Not supported yet
      */
      case TextureFormat.PVRTC_RGB2://	 PowerVR (iOS) 2 bits/pixel compressed color texture format.
        return 2;
      case TextureFormat.PVRTC_RGBA2://	 PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
        return 2;
      case TextureFormat.PVRTC_RGB4://	 PowerVR (iOS) 4 bits/pixel compressed color texture format.
        return 4;
      case TextureFormat.PVRTC_RGBA4://	 PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
        return 4;
      case TextureFormat.ETC_RGB4://	 ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        return 4;
      case TextureFormat.ATC_RGB4://	 ATC (ATITC) 4 bits/pixel compressed RGB texture format.
        return 4;
      case TextureFormat.ATC_RGBA8://	 ATC (ATITC) 8 bits/pixel compressed RGB texture format.
        return 8;
      case TextureFormat.BGRA32://	 Format returned by iPhone camera
        return 32;
    }
    return 0;
  }

  int CalculateTextureSizeBytes(Texture tTexture)
  {

    int tWidth = tTexture.width;
    int tHeight = tTexture.height;
    if (tTexture is Texture2D) {
      Texture2D tTex2D = tTexture as Texture2D;
      int bitsPerPixel = GetBitsPerPixel(tTex2D.format);
      int mipMapCount = tTex2D.mipmapCount;
      int mipLevel = 1;
      int tSize = 0;
      while (mipLevel <= mipMapCount) {
        tSize += tWidth * tHeight * bitsPerPixel / 8;
        tWidth = tWidth / 2;
        tHeight = tHeight / 2;
        mipLevel++;
      }
      return tSize;
    }

    if (tTexture is Cubemap) {
      Cubemap tCubemap = tTexture as Cubemap;
      int bitsPerPixel = GetBitsPerPixel(tCubemap.format);
      return tWidth * tHeight * 6 * bitsPerPixel / 8;
    }
    return 0;
  }


  void SelectObject(Object selectedObject, bool append)
  {
    if (append) {
      List<Object> currentSelection = new List<Object>(Selection.objects);
      // Allow toggle selection
      if (currentSelection.Contains(selectedObject)) currentSelection.Remove(selectedObject);
      else currentSelection.Add(selectedObject);

      Selection.objects = currentSelection.ToArray();
    } else Selection.activeObject = selectedObject;
  }

  void SelectObjects(List<Object> selectedObjects, bool append)
  {
    if (append) {
      List<Object> currentSelection = new List<Object>(Selection.objects);
      currentSelection.AddRange(selectedObjects);
      Selection.objects = currentSelection.ToArray();
    } else Selection.objects = selectedObjects.ToArray();
  }

  string FormatSizeString(int memSizeKB)
  {
    if (memSizeKB < 1024) return "" + memSizeKB + "k";
    else {
      float memSizeMB = ((float)memSizeKB) / 1024.0f;
      return memSizeMB.ToString("0.00") + "Mb";
    }
  }

  #endregion
}