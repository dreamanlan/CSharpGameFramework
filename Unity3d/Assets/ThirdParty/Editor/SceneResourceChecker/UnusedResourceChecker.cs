using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//using YamlDotNet.RepresentationModel;

public enum ItemType
{
  Scene,
  Material,
  Prefab,
  Animation,
  Texture,
  Controller,
  Shader,
  Font,
  Media,
  Model,
  Folder,
  
  Unknown,
  Ignore,
}

public class ItemInfo
{
  public ItemType itemType;
  public string assetPath;
  public string assetGUID;
  public string assetMetaPath;
  public HashSet<string> dependencyAsset = new HashSet<string>();
  public HashSet<string> usedByAsset = new HashSet<string>();
}
/* TODO 
    unused scene
    unused texture
    unused ....
 */

public class UnusedResourceChecker : EditorWindow
{
  private const string guidString = "guid: ";
  private const string MetaExt = ".meta";
  private const int guidSize = 32;
  private const int itemPerPage = 50;
  
  private static Regex reg = new Regex(string.Format(@"(?<={0})([\s\S]{1})", guidString, "{"+guidSize+"}"));
  private static Regex ItemTypeReg;
  private static Regex FolderReg;
  
  /// unknown file ext
  private static HashSet<string> unkonwnExt = new HashSet<string>();
  /// item info, use guid as key
  private static Dictionary<string, ItemInfo> itemInfoDict = new Dictionary<string, ItemInfo>();
  /// scene file
  /// total meta file count
  private static int totalMetaFileCount = 0;
  private bool analyzeFinished = false;
  private bool analyzeContinue = true;
  private int page = 1;
  
  public delegate void ShowRule(ItemInfo itemInfo, ref bool bShow);
  private static Dictionary<ItemType, List<IFilter> > filterDictionary = new Dictionary<ItemType, List<IFilter> >();
  private static Dictionary<ItemType, IItemShow > itemShowDictionary = new Dictionary<ItemType, IItemShow>();
  
  // private static string[] InspectToolbarStrings = { "Scene", "Material", "Prefab", "Texture" };
  private static ItemType activeInspectType = ItemType.Unknown;
  private static Vector2 materialListScrollPos = new Vector2(0, 0);
  
  [MenuItem("ProjectAnalyzer/Unused Resource Checker")]
  static void Init()
  {
    UnusedResourceChecker window = (UnusedResourceChecker)EditorWindow.GetWindow(typeof(UnusedResourceChecker));
    window.CheckCurrentSceneResources();
    window.minSize = new Vector2(500, 300);
  }
  
  #region filter
  public interface IFilter
  {
    void ShowFilter();
    void CheckCanShow(ItemInfo itemInfo, ref bool bShow);
  }
  public class ItemFilter : IFilter
  {
    public virtual void ShowFilter() {}
    public virtual void CheckCanShow(ItemInfo itemInfo, ref bool bShow) {}
  }
  
  public class ItemFilterByName : ItemFilter
  {
    public string filterPath = string.Empty;
    public override void ShowFilter()
    {
      GUILayout.BeginVertical();
      GUILayout.Label("Search Name : ");
      filterPath = GUILayout.TextField(filterPath, GUILayout.Width(200));
      GUILayout.EndVertical();
    }
    public override void CheckCanShow(ItemInfo itemInfo, ref bool bShow)
    {
      if (string.IsNullOrEmpty(filterPath)) {
        return;
      }
      var paths = filterPath.Split(' ');
      foreach (var path in paths) {
        if (!itemInfo.assetPath.ToLower().Contains(path.ToLower())) {
          bShow = false;
          break;
        }
      }
    }
  }
  
  public static void ShowFilter(ItemType itemType)
  {
    List<IFilter> filterList;
    if (filterDictionary.TryGetValue(itemType, out filterList)) {
      foreach(var filter in filterList)
      {
        filter.ShowFilter();
      }
    }
  }
  
  public static bool CheckCanShow(ItemInfo itemInfo)
  {
    bool result = true;
    List<IFilter> filterList;
    if (filterDictionary.TryGetValue(itemInfo.itemType, out filterList)) {
      foreach(var filter in filterList)
      {
        filter.CheckCanShow(itemInfo, ref result);
        if (!result)
        {
          break;
        }
      }
    }
    return result;
  }
  
  private static void AddFilter(ItemType itemType, IFilter filter)
  {
    List<IFilter> filterList;
    if (!filterDictionary.TryGetValue(itemType, out filterList)) {
      filterList = new List<IFilter>();
      filterDictionary.Add(itemType, filterList);
    }
    filterList.Add(filter);
  }
  
  private static void DeleteFilter(ItemType itemType, IFilter filter)
  {
    List<IFilter> filterList;
    if (filterDictionary.TryGetValue(itemType, out filterList)) {
      filterList.Remove(filter);
    }
  }
  
  #endregion
  
  #region show single item
  public interface IItemShow
  {
    void ShowSingleItem(ItemInfo itemInfo);
  }
  
  public class ItemShow : IItemShow
  {
    public virtual void ShowSingleItem(ItemInfo itemInfo) {
      List<string> itemList;
      foreach (var type in Enum.GetNames(typeof(ItemType))) {
        itemList = itemInfo.usedByAsset.Where(
          asset => itemInfoDict [asset].itemType == (ItemType)Enum.Parse(typeof(ItemType), type)).ToList();
        if (itemList.Count > 0 && GUILayout.Button(string.Format("{0} x {1}", type, itemList.Count))) {
          SelectObjects(itemList);
        }
      }
    }
  }
  
  public class ItemShowTexture : ItemShow
  {
  }
  
  public class ItemShowShader : ItemShow
  {
  }
  
  private static void ShowSingleItem(ItemInfo itemInfo)
  {
    IItemShow itemShow;
    if (itemShowDictionary.TryGetValue(itemInfo.itemType, out itemShow)) {
      itemShow.ShowSingleItem(itemInfo);
    }
  }
  
  private static void AddItemShow(ItemType itemType, IItemShow itemShow)
  {
    itemShowDictionary[itemType] = itemShow;
  }
  
  private static void DeleteItemShow(ItemType itemType, IItemShow itemShow)
  {
    itemShowDictionary.Remove(itemType);
  }
  #endregion
  
  private void AddAllRules()
  {
    AddFilter(ItemType.Texture, new ItemFilterByName());
    AddItemShow(ItemType.Texture, new ItemShowTexture());
    AddItemShow(ItemType.Shader, new ItemShowShader());
  }
  
  private void OnGUI()
  {
    #region refresh resources
    GUILayout.BeginHorizontal();
    if (GUILayout.Button("Refresh")) {
      CheckCurrentSceneResources();
    }
    GUILayout.EndHorizontal();
    #endregion
    
    #region show resources
    
    activeInspectType = (ItemType)GUILayout.Toolbar((int)activeInspectType, Enum.GetNames(typeof(ItemType)));
    
    GUILayout.BeginHorizontal();
    
    ListItem(activeInspectType);
    
    GUILayout.EndHorizontal();
    #endregion
  }
  
  private void ListItem(ItemType itemType)
  {
    // TODO dont refresh every frame
    
    var items = itemInfoDict.Values.Where(itemInfo => itemInfo.itemType == itemType &&
                                          CheckCanShow(itemInfo));
    
    GUILayout.BeginVertical();
    
    GUILayout.BeginHorizontal();
    ShowFilter(itemType);
    
    #region page
    GUILayout.BeginVertical();
    GUILayout.Label(string.Format("Go to page ({0}) : ", items.Count()/itemPerPage + 1));
    string strPage = GUILayout.TextField(page.ToString());
    int.TryParse(strPage, out page);
    
    GUILayout.BeginHorizontal();
    if (GUILayout.Button("Prev")) {
      page --;
    }
    if (GUILayout.Button("Next")) {
      page ++;
    }
    GUILayout.EndHorizontal();
    page = Mathf.Max(1, Mathf.Min(items.Count()/itemPerPage + 1, page));
    GUILayout.EndVertical();
    #endregion
    
    GUILayout.EndHorizontal();
    
    materialListScrollPos = EditorGUILayout.BeginScrollView(materialListScrollPos);
    
    int index = 0;
    int totalShown = 0;
    foreach (var item in items) {
      ++index;
      if (index <= (page-1)*itemPerPage) continue;
      ++totalShown;
      if (totalShown > itemPerPage) break;
      GUILayout.BeginHorizontal();
      Texture icon = AssetDatabase.GetCachedIcon(item.assetPath);
      var oldAlignment = GUI.skin.button.alignment;
      GUI.skin.button.alignment = TextAnchor.MiddleLeft;
      if (GUILayout.Button(new GUIContent(index + ". " + item.assetPath, icon, item.assetPath), GUILayout.Width(500), GUILayout.Height(20))) {
        SelectObject(item.assetPath);
      }
      GUI.skin.button.alignment = oldAlignment;
      GUILayout.Label("Used Count " + item.usedByAsset.Count);
      
      ShowSingleItem(item);
      GUILayout.EndHorizontal();
    }
    EditorGUILayout.EndScrollView();
    
    GUILayout.EndVertical();
  }
  
  public void BuildReg()
  {
    /*   StringBuilder sb = new StringBuilder();
        foreach (var type in Enum.GetNames(typeof(ItemType))) {
            sb.Append(type).Append("|");
        }
        sb.Remove(sb.Length - 1, 1);
        ItemTypeReg = new Regex(string.Format(@"({0})", sb.ToString()));*/
    FolderReg = new Regex(string.Format(@"({0})", "folderAsset: yes"));
  }
  
  private void Reset()
  {
    totalMetaFileCount = 0;
    itemInfoDict.Clear();
    unkonwnExt.Clear();
    filterDictionary.Clear();
    itemShowDictionary.Clear();
    analyzeContinue = true;
    analyzeFinished = false;
  }
  
  public void CheckCurrentSceneResources()
  {
    Reset();
    BuildReg();
    AddAllRules();
    
    try
    {
      BuildBaseInfo(Application.dataPath);
      AnalyzeProject(Application.dataPath);
      foreach (var ext in unkonwnExt) {
        Debug.LogError(ext);
      }
      if (analyzeContinue) {
        AnalyzeDependencies();
      }
    }
    finally
    {
      EditorUtility.ClearProgressBar();
    }
  }
  
  private void BuildBaseInfo(string path)
  {
    if (!path.ToLower().Contains("editor")) {
      var files = Directory.GetFiles(path, "*.*")
        .Where(file => file.ToLower().EndsWith(MetaExt))
          .ToList();
      totalMetaFileCount += files.Count;
      var directorys = Directory.GetDirectories(path)
        .Where(dictionary => !dictionary.ToLower().Contains("editor"))
          .ToList();
      totalMetaFileCount += directorys.Count;
      foreach(var directory in directorys)
      {
        BuildBaseInfo(directory); 
      }
    }
  }
  
  private void AnalyzeProject(string path)
  {
    if (analyzeContinue && !path.ToLower().Contains("editor")) {
      var files = Directory.GetFiles(path, "*.*")
        .Where(file => file.ToLower().EndsWith(MetaExt))
          .ToList();
      foreach(var file in files)
      {
        var itemInfo = BuildItemInfoByMetaFile(file);
        if (itemInfo != null)
        {
          if (itemInfoDict.ContainsKey(itemInfo.assetGUID))
          {
            Debug.LogError("Dup " + itemInfo.assetMetaPath + " " + itemInfoDict[itemInfo.assetGUID].assetMetaPath);
          }
          else
          {
            itemInfoDict.Add(itemInfo.assetGUID, itemInfo);
          }
        }
        
        analyzeContinue = !EditorUtility.DisplayCancelableProgressBar("step 1", string.Format("waiting ...({0}/{1})", 
                                                                                              itemInfoDict.Count, totalMetaFileCount), itemInfoDict.Count*1f/totalMetaFileCount);
        // if not continue, set unfinish;
        analyzeFinished = analyzeContinue;
      }   
      var directorys = Directory.GetDirectories(path)
        .Where(dictionary => !dictionary.ToLower().Contains("editor"))
          .ToList();
      foreach(var directory in directorys)
      {
        AnalyzeProject(directory); 
      }
    }
  }
  
  private void AnalyzeDependencies()
  {
    // TODO
    // scene have prefab  texture material mesh shader media
    // prefab have texture shader media material
    var items = itemInfoDict.Values.Where( itemInfo => itemInfo.itemType == ItemType.Scene || 
                                          itemInfo.itemType == ItemType.Material ||
                                          itemInfo.itemType == ItemType.Prefab );
    
    // var items = itemInfoDict.Values.Where(itemInfo => itemInfo.itemType == ItemType.Material);
    int finishedCount = 0;
    ItemInfo useItem;
    
    foreach (var item in items) {
      var sr = new StreamReader(item.assetPath);
      var fileString = sr.ReadToEnd();
      var match = reg.Match(fileString);
      while (match.Success) {
        string useFile = match.Value;
        item.dependencyAsset.Add(useFile);
        if (itemInfoDict.TryGetValue(useFile, out useItem))
        {
          useItem.usedByAsset.Add(item.assetGUID);
        }
        match = match.NextMatch();
      }
      ++finishedCount;
      analyzeContinue = !EditorUtility.DisplayCancelableProgressBar(
        "step 2", string.Format("waiting ...({0}/{1})", finishedCount, items.Count()), finishedCount * 1f / items.Count());
      if (!analyzeContinue) {
        return;
      }
    }
    // build texture use, from material
    finishedCount = 0;
    items = itemInfoDict.Values.Where( itemInfo => itemInfo.itemType == ItemType.Material );
    foreach (var item in items) {
      foreach(var usedBy in item.usedByAsset)
      {
        ItemInfo mainAsset;
        if (itemInfoDict.TryGetValue(usedBy, out mainAsset))
        {
          foreach(var dependency in item.dependencyAsset)
          {
            ItemInfo dependcyAsset;
            if (itemInfoDict.TryGetValue(dependency, out dependcyAsset)){
              if (dependcyAsset.itemType == ItemType.Texture)
              {
                dependcyAsset.usedByAsset.Add(usedBy);
              }
            }
          }
        }
      }
      ++finishedCount;
      analyzeContinue = !EditorUtility.DisplayCancelableProgressBar(
        "step 3", string.Format("waiting ...({0}/{1})", finishedCount, items.Count()), finishedCount * 1f / items.Count());
      if (!analyzeContinue) {
        return;
      }
    }
  }
  
  private ItemInfo BuildItemInfoByMetaFile(string metaFile)
  {
    ItemInfo result = null;
    
    /*     YamlDotNet.Serialization.Deserializer d = new YamlDotNet.Serialization.Deserializer();
        var o = d.Deserialize(sr) as Dictionary<System.Object, System.Object>;

        if (o.ContainsKey("guid")) {
            result = new ItemInfo();
            result.assetGUID = o["guid"] as string;
            result.assetPath = AssetDatabase.GUIDToAssetPath(result.assetGUID);
            result.assetMetaPath = metaFile;
            bool isFolder = o.ContainsKey("folderAsset") && (o["folderAsset"] as string).Equals("yes");
            result.itemType = ItemTypeByExt(metaFile, isFolder);
            if (result.itemType == ItemType.Scene)
            {
                var srs = new StreamReader(metaFile.Replace(MetaExt, ".yaml"));
                try
                {
                    YamlDotNet.Serialization.Deserializer dd = new YamlDotNet.Serialization.Deserializer();
                    var oo = dd.Deserialize(srs) as Dictionary<System.Object, System.Object>;
                    if (oo != null)
                    {
                        oo = null;
                    }
                /*YamlStream ys = new YamlStream();

                ys.Load(srs);
                var mapping = (YamlMappingNode)ys.Documents[0].RootNode;
                
                foreach (var entry in mapping.Children)
                {
                    Debug.Log(((YamlScalarNode)entry.Key).Value);
                }
                }
                finally
                {
                    srs.Close();
                }
            }
            
        }*/
    
    var sr = new StreamReader(metaFile);
    try
    {
      var metaFileString = sr.ReadToEnd();
      var match = reg.Match(metaFileString);
      if (match.Success) {
        result = new ItemInfo();
        result.assetGUID = match.Value;
        result.assetPath = AssetDatabase.GUIDToAssetPath(result.assetGUID);
        result.assetMetaPath = metaFile;
        result.itemType = ItemTypeByExt(metaFile, metaFileString.Contains("folderAsset: yes"));
      } else {
        Debug.LogError("Error meta file "+metaFile);
      }
    }
    finally
    {
      sr.Close();
    }
    
    return result;
  }
  
  private ItemType ItemTypeByExt(string metaFile, bool isFolder)
  {
    // TODO
    ItemType itemType = ItemType.Unknown;
    string file = metaFile.Replace(MetaExt, "");
    string ext = Path.GetExtension(file).ToLower();
    switch (ext) {
    case ".png":
    case ".jpg":
    case ".git":
    case ".jpge":
    case ".tga":
    case ".psd":
    case ".exr":
    case ".tif":
    case ".rendertexture":
    {
      itemType = ItemType.Texture;
    }
      break;
    case ".obj":
    case ".fbx":
    {
      itemType = ItemType.Model;
    }
      break;
    case ".unity":
    {
      itemType = ItemType.Scene;
    }
      break;
    case ".prefab":
    {
      itemType = ItemType.Prefab;
    }
      break;
    case ".mat":
    {
      itemType = ItemType.Material;
    }
      break;
    case ".anim":
    {
      itemType = ItemType.Animation;
    }
      break;
    case ".controller":
    {
      itemType = ItemType.Controller;
    }
      break;
    case ".shader":
    {
      itemType = ItemType.Shader;
    }
      break;
    case ".ttf":
    {
      itemType = ItemType.Font;
    }
      break;
    case ".mp4":
    case ".mp3":
    case ".wav":
    {
      itemType = ItemType.Media;
    }
      break;
    case ".cs":
    case ".txt":
    case ".sh":
    case ".guiskin":
    case ".plist":
    case ".fnt":
    case ".mdb":
    case ".dll":
    case ".jar":
    case ".xml":
    case ".so":
    case ".properties":
    case ".bin":
    case ".js":
    case ".cginc":
    case ".physicmaterial":
    case ".dsl":
    case ".ini":
    case ".tmx":
    case ".map":
    case ".obs":
    case ".path":
    case ".asset":
    {
      itemType = ItemType.Ignore;
    }
      break;
    default:
    {
      if (isFolder) {
        itemType = ItemType.Folder;
      }
      else
      {
        unkonwnExt.Add(ext);
      }
    }
      break;
    }
    
    return itemType;
  }
  
  private static void SelectObject(string assetPath)
  {
    var objLoaded = AssetDatabase.LoadMainAssetAtPath(assetPath);
    if (objLoaded != null) {
      if (Selection.activeObject != null && !Selection.activeObject is GameObject)
      {
        Resources.UnloadAsset(Selection.activeObject);
        Selection.activeObject = null;
      }
      Selection.activeObject = objLoaded;
      EditorGUIUtility.PingObject(Selection.activeObject);
    }
  }
  
  private static void SelectObjects(List<string> selectedObjects, ItemType itemType)
  {
    var objList = new List<UnityEngine.Object>();
    for (int i=0; i<Selection.objects.Length; ++i) {
      var obj = Selection.objects[i];
      if (obj != null && !obj is GameObject)
      {
        Resources.UnloadAsset(obj);
      }
    }
    foreach (var obj in selectedObjects) {
      ItemInfo itemInfo;
      if (itemInfoDict.TryGetValue(obj, out itemInfo)){
        if (itemInfo.itemType == itemType)
        {
          var objLoaded = AssetDatabase.LoadMainAssetAtPath(itemInfo.assetPath);
          if (objLoaded != null)
          {
            objList.Add(objLoaded);
          }
        }
      }
    }
    Selection.objects = objList.ToArray();
  }
  
  private static void SelectObjects(List<string> selectedObjects)
  {
    var objList = new List<UnityEngine.Object>();
    for (int i=0; i<Selection.objects.Length; ++i) {
      var obj = Selection.objects[i];
      if (obj != null && !obj is GameObject)
      {
        Resources.UnloadAsset(obj);
      }
    }
    foreach (var obj in selectedObjects) {
      ItemInfo itemInfo;
      if (itemInfoDict.TryGetValue(obj, out itemInfo)){
        var objLoaded = AssetDatabase.LoadMainAssetAtPath(itemInfo.assetPath);
        if (objLoaded != null)
        {
          objList.Add(objLoaded);
        }
      }
    }
    Selection.objects = objList.ToArray();
  }
}