using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using StoryScript;
using StoryScript.DslExpression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// Stage 1: pure channel translation from the stage-0 design bitmaps.
// No shape functions, no invented layouts, no hardcoded spawn points.
//   terrain PNG : R -> heightmap, G -> alphamap material band, B -> walkability mask
//   layout PNG  : R -> area id, G -> intensity, B -> trigger type (baked to areas.txt)
public static class MapBuild
{
    private const string DesignRoot = @"D:\GitHub\CSharpGameFramework\Resource\MapDesign";
    private const string TileDir = DesignRoot + @"\tiles";
    private const string SceneDir = "Assets/Scenes/Levels";
    private const string DataDir = "Assets/Scenes/Levels/MapData";
    private const string SourceTerrain = "Assets/Terrain/Level2.asset";
    private const float WorldSize = 512f;
    private const float TerrainHeight = 70f;

    private struct Tile
    {
        public string Id, Code, Name, Era, Route;
        public int Diff, Seq;
    }

    private static readonly Dictionary<string, string> EraSuffix = new Dictionary<string, string>
    {
        { "ARR", "flood" }, { "ORB", "prediluvian" }, { "LEV", "sumerian" },
        { "MES", "sumer" }, { "EGY", "prediluvian" }, { "ABZ", "prediluvian" },
        { "AMR", "postdiluvian" }, { "PAC", "ancient" }
    };

    private static readonly Dictionary<string, Color> Tints = new Dictionary<string, Color>
    {
        { "ARR", new Color(0.74f, 0.81f, 0.88f) }, { "ORB", new Color(0.68f, 0.66f, 0.82f) },
        { "LEV", new Color(0.86f, 0.77f, 0.65f) }, { "MES", new Color(0.80f, 0.83f, 0.65f) },
        { "EGY", new Color(0.90f, 0.81f, 0.61f) }, { "ABZ", new Color(0.79f, 0.71f, 0.59f) },
        { "AMR", new Color(0.73f, 0.81f, 0.69f) }, { "PAC", new Color(0.71f, 0.79f, 0.83f) }
    };

    private static List<Tile> LoadTiles()
    {
        var list = new List<Tile>();
        foreach (string line in File.ReadAllLines(Path.Combine(DesignRoot, "regions.txt")))
        {
            if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;
            string[] p = line.Split('|').Select(s => s.Trim()).ToArray();
            if (p.Length < 9) continue;
            list.Add(new Tile
            {
                Id = p[0], Code = p[1], Name = p[2], Era = p[3], Route = p[4],
                Diff = int.Parse(p[7]), Seq = int.Parse(p[8])
            });
        }
        return list;
    }

    // Unity's GetPixels32 starts at bottom-left; the design PNGs start top-left.
    private static Color32[] ReadPng(string path, out int w, out int h)
    {
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!ImageConversion.LoadImage(tex, File.ReadAllBytes(path), false))
            throw new Exception("PNG load failed: " + path);
        w = tex.width; h = tex.height;
        Color32[] raw = tex.GetPixels32();
        var design = new Color32[w * h];
        for (int y = 0; y < h; ++y)
            for (int x = 0; x < w; ++x)
                design[y * w + x] = raw[(h - 1 - y) * w + x];
        UnityEngine.Object.DestroyImmediate(tex);
        return design;
    }

    private static float SampleR(Color32[] px, int w, int h, float u, float v)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt(u * (w - 1)), 0, w - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(v * (h - 1)), 0, h - 1);
        return px[y * w + x].r / 255f;
    }

    private static Color32 Sample(Color32[] px, int w, int h, float u, float v)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt(u * (w - 1)), 0, w - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(v * (h - 1)), 0, h - 1);
        return px[y * w + x];
    }

    private static TerrainData BuildTerrain(Tile t, string assetPath, out float minH, out float maxH)
    {
        TerrainData td = AssetDatabase.LoadAssetAtPath<TerrainData>(assetPath);
        if (td == null)
        {
            TerrainData src = AssetDatabase.LoadAssetAtPath<TerrainData>(SourceTerrain);
            if (src == null) throw new Exception("missing base terrain " + SourceTerrain);
            td = UnityEngine.Object.Instantiate(src);
            AssetDatabase.CreateAsset(td, assetPath);
        }
        int tw, th;
        Color32[] tp = ReadPng(Path.Combine(TileDir, t.Id + "_terrain.png"), out tw, out th);
        int res = td.heightmapResolution;
        td.size = new Vector3(WorldSize, TerrainHeight, WorldSize);
        float[,] heights = new float[res, res];
        minH = 1f; maxH = 0f;
        for (int z = 0; z < res; ++z)
        {
            for (int x = 0; x < res; ++x)
            {
                float u = (float)x / (res - 1), v = (float)z / (res - 1);
                float val = SampleR(tp, tw, th, u, v);
                heights[z, x] = val;
                minH = Mathf.Min(minH, val); maxH = Mathf.Max(maxH, val);
            }
        }
        td.SetHeights(0, 0, heights);

        int ares = td.alphamapResolution, layers = td.alphamapLayers;
        if (layers > 0 && ares > 0)
        {
            float[,,] alpha = new float[ares, ares, layers];
            for (int z = 0; z < ares; ++z)
            {
                for (int x = 0; x < ares; ++x)
                {
                    float u = (float)x / (ares - 1), v = (float)z / (ares - 1);
                    byte g = Sample(tp, tw, th, u, v).g;
                    int layer = g < 64 ? 0 : g < 128 ? 1 : g < 192 ? 2 : 3;
                    layer = Mathf.Clamp(layer, 0, layers - 1);
                    alpha[z, x, layer] = 1f;
                }
            }
            td.SetAlphamaps(0, 0, alpha);
        }
        // Stage-0 design PNGs carry no tree layer; drop the trees inherited from
        // the base Level2 terrain (they also spam the Pine_tree shader warning).
        td.treeInstances = new TreeInstance[0];
        td.treePrototypes = new TreePrototype[0];
        EditorUtility.SetDirty(td);
        return td;
    }

    private struct Area
    {
        public int Type; public float X, Z, Radius, G, B; public int Pixels;
    }

    // Connected components of equal R value: the layout PNG is the only source
    // of area positions. Nothing is invented here.
    private static List<Area> BakeLayout(string pngPath)
    {
        int w, h;
        Color32[] px = ReadPng(pngPath, out w, out h);
        var seen = new bool[w * h];
        var result = new List<Area>();
        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < w; ++x)
            {
                int idx = y * w + x;
                if (seen[idx]) continue;
                int type = px[idx].r;
                if (type == 0) { seen[idx] = true; continue; }
                var q = new Queue<int>();
                q.Enqueue(idx); seen[idx] = true;
                long sx = 0, sz = 0, sg = 0, sb = 0; int n = 0;
                while (q.Count > 0)
                {
                    int cur = q.Dequeue();
                    int cx = cur % w, cy = cur / w;
                    Color32 c = px[cur];
                    sx += cx; sz += cy; sg += c.g; sb += c.b; n++;
                    int[] nx = { cx + 1, cx - 1, cx, cx };
                    int[] ny = { cy, cy, cy + 1, cy - 1 };
                    for (int k = 0; k < 4; ++k)
                    {
                        int ax = nx[k], ay = ny[k];
                        if (ax < 0 || ax >= w || ay < 0 || ay >= h) continue;
                        int ni = ay * w + ax;
                        if (seen[ni] || px[ni].r != type) continue;
                        seen[ni] = true; q.Enqueue(ni);
                    }
                }
                float scale = WorldSize / w;   // 1 px = 2 world units
                result.Add(new Area
                {
                    Type = type,
                    X = (float)sx / n * scale, Z = (float)sz / n * scale,
                    Radius = Mathf.Sqrt((float)n / Mathf.PI) * scale,
                    G = (float)sg / n, B = (float)sb / n, Pixels = n
                });
            }
        }
        return result;
    }

    private static void WriteAreas(List<Area> areas, string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# area data baked from layout PNG (no runtime PNG reads)");
        sb.AppendLine("# type: 60/80/100=battle area, 200=road, 220=event, 240=reward, 250=portal");
        sb.AppendLine("# type | worldX | worldZ | radius | intensity(G) | trigger(B) | pixels");
        foreach (Area a in areas.OrderBy(a => a.Type).ThenByDescending(a => a.Pixels))
            sb.AppendLine(string.Format("{0} | {1:F1} | {2:F1} | {3:F1} | {4:F0} | {5:F0} | {6}",
                a.Type, a.X, a.Z, a.Radius, a.G, a.B, a.Pixels));
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }

    private static void WriteStorySkeleton(Tile t, List<Area> areas, int rank, string path)
    {
        var battles = areas.Where(a => a.Type >= 60 && a.Type <= 140).OrderBy(a => a.Type).ToList();
        var portal = areas.FirstOrDefault(a => a.Type == 250);
        var sb = new StringBuilder();
        sb.AppendLine("story(battle_main)");
        sb.AppendLine("{");
        sb.AppendLine("  onmessage(\"start\")");
        sb.AppendLine("  {");
        sb.AppendLine(string.Format("    create_npc(1000,vector_3({0:F0},0,{1:F0}),0,3,1,\"ai_player\",string_list(\"Ai/ailogic_player.dsl\"));",
            portal.X, portal.Z));
        sb.AppendLine("    set_leader_id(unit_id_2_obj_id(1000));");
        int uid = 1006;
        foreach (Area a in battles)
        {
            sb.AppendLine(string.Format("    loop(6){{ create_npc({0}+$$,vector_3({1:F0}+rnd_int(0,8),0,{2:F0}+rnd_int(0,8)),0,4,$$+6,\"ai_normal\",string_list(\"Ai/ailogic_normal.dsl\")); }};",
                uid, a.X, a.Z));
            uid += 10;
        }
        sb.AppendLine("    camera_follow(1000);");
        sb.AppendLine("    publish_gfx_event(\"loading_complete\", \"ui\");");
        sb.AppendLine("    start_story(\"auto_battle\");");
        sb.AppendLine("  };");
        sb.AppendLine("  onmessage(\"all_killed\"){ clear_message(\"show_dlg\"); fire_message(\"show_dlg\",998); };");
        sb.AppendLine("  onmessage(\"npc_killed:1000\"){ clear_message(\"show_dlg\"); fire_message(\"show_dlg\",999); };");
        sb.AppendLine("  onmessage(\"all_died\"){ clear_message(\"show_dlg\"); fire_message(\"show_dlg\",999); };");
        sb.AppendLine(string.Format("  onmessage(\"dialog_over\",998){{ terminate(); close_battle({0}); }};", 10000 + rank));
        sb.AppendLine(string.Format("  onmessage(\"dialog_over\",999){{ terminate(); close_battle({0}); }};", 10000 + rank));
        sb.AppendLine("};");
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }

    private const float BattleCamDistance = 28f;
    private const float BattleCamUp = 8f;
    private const float BattleCamPitch = 52f;

    private static System.Type CameraSettingType()
    {
        foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            var t = asm.GetType("CameraSetting");
            if (t != null) return t;
        }
        return null;
    }

    private static void SetFloatField(System.Type t, object obj, string name, float v)
    {
        var f = t.GetField(name);
        if (f != null) f.SetValue(obj, v);
    }

    public static System.Type CameraSettingTypePublic() { return CameraSettingType(); }

    public static float GetFloatFieldPublic(System.Type t, object obj, string name)
    {
        var f = t.GetField(name);
        return f != null ? (float)f.GetValue(obj) : 0f;
    }

    // CameraController only rewrites camera.position while followTargetId>0.
    // The saved scene pose therefore has to be a usable close gameplay view,
    // otherwise a missed camera_follow leaves the camera at a 300m preview pose.
    private static void ApplyBattleCamera(Camera cam, TerrainData td)
    {
        Quaternion rot = Quaternion.Euler(BattleCamPitch, 0f, 0f);
        Vector3 anchor = new Vector3(60f, 0f, 70f);
        if (td != null)
            anchor.y = td.GetInterpolatedHeight(60f / WorldSize, 70f / WorldSize);
        cam.transform.position = anchor + Vector3.up * BattleCamUp - rot * Vector3.forward * BattleCamDistance;
        cam.transform.rotation = rot;
        cam.fieldOfView = 50f;

        var t = CameraSettingType();
        if (t == null) return;
        Component setting = cam.gameObject.GetComponent(t);
        if (setting == null) setting = cam.gameObject.AddComponent(t);
        SetFloatField(t, setting, "distance", BattleCamDistance);
        SetFloatField(t, setting, "up", BattleCamUp);
        SetFloatField(t, setting, "pitch", BattleCamPitch);
        SetFloatField(t, setting, "yaw", 0f);
    }

    // Patch all already-built scenes in place. Rebuilding with map_build would
    // discard the baked NavMesh references, so cameras are fixed separately.
    public static string FixCameras()
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        string current = SceneManager.GetActiveScene().path;
        int fixedCount = 0;
        try
        {
            foreach (Tile t in tiles)
            {
                string suffix;
                if (!EraSuffix.TryGetValue(t.Code, out suffix)) suffix = "ancient";
                foreach (string name in new[] { "Battle_" + t.Id, "Battle_" + t.Id + "_" + suffix })
                {
                    string path = SceneDir + "/" + name + ".unity";
                    if (!File.Exists(path)) continue;
                    Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                    Camera cam = null;
                    TerrainData td = null;
                    foreach (GameObject go in sc.GetRootGameObjects())
                    {
                        if (go == null) continue;
                        if (cam == null) cam = go.GetComponent<Camera>();
                        if (td == null)
                        {
                            Terrain terrain = go.GetComponent<Terrain>();
                            if (terrain != null) td = terrain.terrainData;
                        }
                    }
                    if (cam == null) { sb.AppendLine("NO_CAM " + name); continue; }
                    ApplyBattleCamera(cam, td);
                    EditorSceneManager.MarkSceneDirty(sc);
                    EditorSceneManager.SaveScene(sc);
                    fixedCount++;
                }
            }
        }
        catch (Exception e) { sb.AppendLine("CAM_FIX_FAIL " + e); }
        finally
        {
            AssetDatabase.SaveAssets();
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, "MAP_CAMERA_FIX scenes=" + fixedCount + "\n");
        return sb.ToString();
    }

    // One .unity per era: the physical terrain data is shared, but the scene
    // (light, fog, mood, layout) is a separate asset so eras never stack in memory.
    private static void MakeScene(Tile t, TerrainData td, Color tint, string path, bool isEra)
    {
        Scene sc = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject terrainGo = Terrain.CreateTerrainGameObject(td);
        terrainGo.name = "Terrain_" + t.Id;
        foreach (Transform tr in terrainGo.GetComponentsInChildren<Transform>(true))
            GameObjectUtility.SetStaticEditorFlags(tr.gameObject, StaticEditorFlags.NavigationStatic);

        Color mood = isEra
            ? new Color(tint.r * 0.72f, tint.g * 0.70f, tint.b * 0.80f)
            : tint;
        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        Camera cam = camGo.AddComponent<Camera>();
        // Gameplay pose: a close third-person view anchored on the party spawn
        // point (60,70). CameraController only rewrites position while it has a
        // follow target, so the saved pose must already be playable.
        ApplyBattleCamera(cam, td);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = mood * 0.66f;
        cam.farClipPlane = 1200f;

        var lightGo = new GameObject("Sun");
        Light sun = lightGo.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.color = Color.Lerp(Color.white, mood, 0.35f);
        sun.intensity = isEra ? 1.05f : 1.25f;
        lightGo.transform.rotation = Quaternion.Euler(48f, 20f + (t.Seq % 8) * 28f + (isEra ? 15f : 0f), 0f);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = mood * (isEra ? 0.72f : 0.85f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = mood * 0.66f;
        RenderSettings.fogDensity = isEra ? 0.0012f : 0.0008f;

        var marker = new GameObject(isEra ? "EraLayer" : "ModernLayer");
        marker.tag = "EditorOnly";
        EditorSceneManager.SaveScene(sc, path);
    }

    private static int RegisterBuildScenes(IEnumerable<string> paths)
    {
        var known = new HashSet<string>(EditorBuildSettings.scenes.Select(s => s.path));
        var list = EditorBuildSettings.scenes.ToList();
        int added = 0;
        foreach (string p in paths)
        {
            if (!known.Add(p)) continue;
            list.Add(new EditorBuildSettingsScene(p, true));
            added++;
        }
        if (added > 0) EditorBuildSettings.scenes = list.ToArray();
        return added;
    }

    public static string Build(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        if (!string.IsNullOrEmpty(which)) tiles = tiles.Where(t => t.Id == which).ToList();
        Directory.CreateDirectory(DataDir);
        Directory.CreateDirectory("Assets/Resources/Minimap");
        Directory.CreateDirectory("Assets/Resources/MapData");
        Directory.CreateDirectory(Path.Combine(DesignRoot, "stories"));
        string current = SceneManager.GetActiveScene().path;
        int rank = 0, ok = 0;
        var scenePaths = new List<string>();
        try
        {
            foreach (Tile t in tiles)
            {
                rank = string.IsNullOrEmpty(which) ? rank + 1 : Mathf.Max(1, tiles[0].Seq);
                string suffix;
                if (!EraSuffix.TryGetValue(t.Code, out suffix)) suffix = "ancient";
                string terrainPath = Path.Combine(TileDir, t.Id + "_terrain.png");
                string layoutPath = Path.Combine(TileDir, t.Id + "_layout.png");
                string eraLayoutPath = Path.Combine(TileDir, t.Id + "_layout_" + suffix + ".png");
                if (!File.Exists(terrainPath) || !File.Exists(layoutPath))
                { sb.AppendLine("MISS_INPUT " + t.Id); continue; }

                float minH, maxH;
                string assetPath = DataDir + "/" + t.Id + "_Ground.asset";
                TerrainData td = BuildTerrain(t, assetPath, out minH, out maxH);

                List<Area> areas = BakeLayout(layoutPath);
                List<Area> eraAreas = File.Exists(eraLayoutPath) ? BakeLayout(eraLayoutPath) : new List<Area>();
                WriteAreas(areas, "Assets/Resources/MapData/" + t.Id + "_areas.txt");
                WriteAreas(eraAreas, "Assets/Resources/MapData/" + t.Id + "_areas_" + suffix + ".txt");

                foreach (string src in new[] { t.Id + "_minimap.png", t.Id + "_minimap_" + suffix + ".png" })
                {
                    string s = Path.Combine(TileDir, src);
                    if (!File.Exists(s)) continue;
                    string d = "Assets/Resources/Minimap/" + src;
                    File.Copy(s, d, true);
                    AssetDatabase.ImportAsset(d, ImportAssetOptions.ForceSynchronousImport);
                }
                WriteStorySkeleton(t, areas, rank, Path.Combine(DesignRoot, "stories", t.Id + "_story.dsl"));

                // Two independent scenes per tile: modern survey layer and the
                // tile's past-era battle layer. TerrainData is shared, scenes are not.
                Color tint = Tints.ContainsKey(t.Code) ? Tints[t.Code] : Color.white;
                string modernPath = SceneDir + "/Battle_" + t.Id + ".unity";
                string eraPath = SceneDir + "/Battle_" + t.Id + "_" + suffix + ".unity";
                MakeScene(t, td, tint, modernPath, false);
                MakeScene(t, td, tint, eraPath, true);
                scenePaths.Add(modernPath);
                scenePaths.Add(eraPath);
                ok++;
                int nBattle = areas.Count(a => a.Type >= 60 && a.Type <= 140);
                sb.AppendLine(string.Format("BUILD {0} [{1} {2}] modern+{3} height={4:F1}..{5:F1} areas={6} eraAreas={7} rank={8}",
                    t.Id, t.Code, t.Name, suffix,
                    minH * TerrainHeight, maxH * TerrainHeight, nBattle,
                    eraAreas.Count(a => a.Type >= 60 && a.Type <= 140), rank));
            }
        }
        catch (Exception e) { sb.AppendLine("BUILD_FAIL " + e); }
        finally
        {
            AssetDatabase.SaveAssets();
            int added = RegisterBuildScenes(scenePaths);
            sb.AppendLine("BUILD_SETTINGS added=" + added + " total=" + EditorBuildSettings.scenes.Length);
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, "MAP_BUILD tiles=" + tiles.Count + " built=" + ok + " scenes=" + (ok * 2) + "\n");
        return sb.ToString();
    }

    public static string Verify(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        if (!string.IsNullOrEmpty(which)) tiles = tiles.Where(t => t.Id == which).ToList();
        int pass = 0;
        foreach (Tile t in tiles)
        {
            string suffix;
            if (!EraSuffix.TryGetValue(t.Code, out suffix)) suffix = "ancient";
            string scene = SceneDir + "/Battle_" + t.Id + ".unity";
            string ground = DataDir + "/" + t.Id + "_Ground.asset";
            string areas = "Assets/Resources/MapData/" + t.Id + "_areas.txt";
            string mini = "Assets/Resources/Minimap/" + t.Id + "_minimap.png";
            string story = Path.Combine(DesignRoot, "stories", t.Id + "_story.dsl");
            bool sceneOk = File.Exists(scene), groundOk = File.Exists(ground);
            bool areasOk = File.Exists(areas), miniOk = File.Exists(mini), storyOk = File.Exists(story);
            int nBattle = 0, nEvent = 0, nPortal = 0, nReward = 0;
            float relief = 0f;
            if (areasOk)
            {
                foreach (string line in File.ReadAllLines(areas))
                {
                    if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;
                    string[] p = line.Split('|');
                    if (p.Length < 7) continue;
                    int type = int.Parse(p[0].Trim());
                    if (type >= 60 && type <= 140) nBattle++;
                    else if (type == 220) nEvent++;
                    else if (type == 240) nReward++;
                    else if (type == 250) nPortal++;
                }
            }
            TerrainData td = AssetDatabase.LoadAssetAtPath<TerrainData>(ground);
            if (td != null)
            {
                float lo = 1f, hi = 0f;
                for (int i = 0; i <= 16; ++i)
                    for (int j = 0; j <= 16; ++j)
                    {
                        float v = td.GetInterpolatedHeight(i / 16f, j / 16f) / td.size.y;
                        lo = Mathf.Min(lo, v); hi = Mathf.Max(hi, v);
                    }
                relief = (hi - lo) * td.size.y;
            }
            string eraScene = SceneDir + "/Battle_" + t.Id + "_" + suffix + ".unity";
            bool eraOk = File.Exists(eraScene);
            bool ok = sceneOk && eraOk && groundOk && areasOk && miniOk && storyOk &&
                      nBattle >= 2 && nEvent >= 1 && nPortal >= 1 && nReward >= 1 && relief > 8f;
            if (ok) pass++;
            sb.AppendLine(string.Format("VERIFY {0} modern={1} era={2} ground={3} areas={4} mini={5} story={6} " +
                "battle={7} event={8} reward={9} portal={10} relief={11:F1} => {12}",
                t.Id, sceneOk, eraOk, groundOk, areasOk, miniOk, storyOk,
                nBattle, nEvent, nReward, nPortal, relief, ok ? "PASS" : "FAIL"));
        }
        sb.Insert(0, "MAP_VERIFY total=" + tiles.Count + " pass=" + pass + " fail=" + (tiles.Count - pass) + "\n");
        return sb.ToString();
    }

    // Legacy NavMesh data is stored in a folder named after each scene.
    public static string BakeNav()
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        string current = SceneManager.GetActiveScene().path;
        int baked = 0;
        try
        {
            foreach (Tile t in tiles)
            {
                string suffix;
                if (!EraSuffix.TryGetValue(t.Code, out suffix)) suffix = "ancient";
                string sceneName = "Battle_" + t.Id + "_" + suffix;
                string path = SceneDir + "/" + sceneName + ".unity";
                if (!File.Exists(path)) continue;
                Directory.CreateDirectory(SceneDir + "/" + sceneName);
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    if (go == null) continue;
                    foreach (Transform tr in go.GetComponentsInChildren<Transform>(true))
                        GameObjectUtility.SetStaticEditorFlags(tr.gameObject, StaticEditorFlags.NavigationStatic);
                }
                UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                baked++;
            }
        }
        catch (Exception e) { sb.AppendLine("NAV_FAIL " + e); }
        finally
        {
            AssetDatabase.SaveAssets();
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, "MAP_NAV baked=" + baked + "\n");
        return sb.ToString();
    }

    public static string Shot(string which)
    {
        var sb = new StringBuilder();
        string dir = "e:/tmp/map_previews";
        Directory.CreateDirectory(dir);
        List<Tile> tiles = LoadTiles();
        if (!string.IsNullOrEmpty(which)) tiles = tiles.Where(t => t.Id == which).ToList();
        string current = SceneManager.GetActiveScene().path;
        try
        {
            foreach (Tile t in tiles)
            {
                string suffix;
                if (!EraSuffix.TryGetValue(t.Code, out suffix)) suffix = "ancient";
                var variants = new[] { new { Path = SceneDir + "/Battle_" + t.Id + ".unity", Name = t.Id },
                                       new { Path = SceneDir + "/Battle_" + t.Id + "_" + suffix + ".unity", Name = t.Id + "_" + suffix } };
                foreach (var v in variants)
                {
                    if (!File.Exists(v.Path)) continue;
                    Scene sc = EditorSceneManager.OpenScene(v.Path, OpenSceneMode.Single);
                    Camera cam = null;
                    foreach (GameObject go in sc.GetRootGameObjects())
                    {
                        cam = go.GetComponent<Camera>();
                        if (cam != null) break;
                    }
                    if (cam == null) { sb.AppendLine("NO_CAM " + v.Name); continue; }
                    var rt = new RenderTexture(640, 360, 24);
                    var tex = new Texture2D(640, 360, TextureFormat.RGB24, false);
                    RenderTexture oldTarget = cam.targetTexture, prev = RenderTexture.active;
                    Vector3 savedPos = cam.transform.position;
                    Quaternion savedRot = cam.transform.rotation;
                    float savedFov = cam.fieldOfView;
                    try
                    {
                        // Whole-tile oblique preview only; the saved scene keeps
                        // its close gameplay camera pose.
                        cam.transform.position = new Vector3(20f, 300f, 20f);
                        cam.transform.LookAt(new Vector3(256f, 0f, 256f));
                        cam.fieldOfView = 60f;
                        cam.targetTexture = rt; cam.Render();
                        RenderTexture.active = rt;
                        tex.ReadPixels(new Rect(0, 0, 640, 360), 0, 0); tex.Apply();
                        string outPath = Path.Combine(dir, v.Name + ".png");
                        File.WriteAllBytes(outPath, tex.EncodeToPNG());
                        sb.AppendLine("SHOT " + outPath);
                    }
                    finally
                    {
                        cam.targetTexture = oldTarget; RenderTexture.active = prev;
                        cam.transform.position = savedPos;
                        cam.transform.rotation = savedRot;
                        cam.fieldOfView = savedFov;
                        UnityEngine.Object.DestroyImmediate(rt);
                        UnityEngine.Object.DestroyImmediate(tex);
                    }
                }
            }
        }
        finally
        {
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        return sb.ToString();
    }
}

public sealed class MapBuildExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(MapBuild.Build(w));
    }
}

public sealed class MapVerifyExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(MapBuild.Verify(w));
    }
}

public sealed class MapShotExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(MapBuild.Shot(w));
    }
}

public sealed class MapNavExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(MapBuild.BakeNav());
    }
}

public sealed class MapCameraFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(MapBuild.FixCameras());
    }
}

// Runtime diagnostic: report the live camera and its distance to the party
// leader, so "follows / too far" can be measured instead of eyeballed.
public static class CameraProbe
{
    public static string Run()
    {
        Camera cam = Camera.main;
        if (cam == null) return "NO_MAIN_CAMERA";
        var sb = new StringBuilder();
        sb.Append("CAM name=" + cam.name + " pos=" + cam.transform.position.ToString("F1") +
                  " rot=" + cam.transform.eulerAngles.ToString("F1") +
                  " fov=" + cam.fieldOfView.ToString("F1"));
        var t = MapBuild.CameraSettingTypePublic();
        if (t != null)
        {
            Component setting = cam.gameObject.GetComponent(t);
            if (setting != null)
                sb.Append(" setting=[distance=" + MapBuild.GetFloatFieldPublic(t, setting, "distance") +
                          " up=" + MapBuild.GetFloatFieldPublic(t, setting, "up") +
                          " pitch=" + MapBuild.GetFloatFieldPublic(t, setting, "pitch") + "]");
        }
        sb.AppendLine();
        int leader = ScriptableFramework.PluginFramework.Instance.LeaderId;
        GameObject go = ScriptableFramework.PluginFramework.Instance.GetGameObject(leader);
        if (go != null)
            sb.AppendLine("LEADER id=" + leader + " pos=" + go.transform.position.ToString("F1") +
                          " camDist=" + Vector3.Distance(cam.transform.position, go.transform.position).ToString("F1"));
        else
            sb.AppendLine("LEADER id=" + leader + " obj=NULL");
        return sb.ToString();
    }
}

public sealed class CameraProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(CameraProbe.Run());
    }
}

// Runtime diagnostic: dump AI state / home / target / view ranges for every
// live entity, so "mobs wander like zombies" can be diagnosed from data.
public static class CombatProbe
{
    public static string Run()
    {
        var sb = new StringBuilder();
        // Entity ids are global and monotonically increasing, so scanning a
        // fixed id window misses later battles. Walk from the live HP bars back
        // to their owning actors instead: this also proves which actors got one.
        var bars = UnityEngine.Object.FindObjectsOfType<OverHeadBar>();
        int n = 0;
        foreach (var bar in bars)
        {
            if (bar == null || bar.transform.parent == null) continue;
            GameObject owner = bar.transform.parent.gameObject;
            int id = ScriptableFramework.PluginFramework.Instance.GetGameObjectId(owner);
            var e = ScriptableFramework.PluginFramework.Instance.GetEntityById(id);
            if (e == null)
            {
                sb.AppendLine("E id=" + id + " obj=" + owner.name + " (no entity)");
                continue;
            }
            var ai = e.GetAiStateInfo();
            var ms = e.GetMovementStateInfo();
            sb.Append("E id=" + id +
                      " pos=" + ms.GetPosition3D() +
                      " state=" + ai.CurState +
                      " target=" + ai.Target +
                      " home=" + ai.HomePos +
                      " gohome=" + e.GohomeRange +
                      " view=" + e.ViewRange +
                      " moving=" + ms.IsMoving +
                      " tgtPos=" + ms.TargetPosition);
            var view = ScriptableFramework.EntityViewModelManager.Instance.GetEntityViewById(id);
            if (view != null && view.Animator != null) {
                var st = view.Animator.GetCurrentAnimatorStateInfo(0);
                string anim = st.IsName("Run") ? "Run" : st.IsName("Idle") ? "Idle"
                            : st.IsName("Attack") ? "Attack" : st.IsName("Hit") ? "Hit"
                            : "#" + st.shortNameHash;
                sb.Append(" anim=" + anim + " moving=" + ms.IsMoving);
            }
            sb.AppendLine();
            n++;
        }
        var sel = ScriptableFramework.PluginFramework.Instance.SelectedTarget;
        sb.Insert(0, "COMBAT_PROBE entities=" + n + " overheadBars=" + bars.Length +
                  " lockedTarget=" + (sel == null ? 0 : sel.TargetId) + "\n");
        return sb.ToString();
    }
}

public sealed class CombatProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(CombatProbe.Run());
    }
}

// The selection ring is pure VFX. Its Plane carries a non-convex MeshCollider
// alongside a non-kinematic Rigidbody, which Unity 5+ rejects at runtime.
public static class SelectPrefabFix
{
    public static string Run()
    {
        const string path = "Assets/Resources/Effects/Select.prefab";
        if (!File.Exists(path)) return "MISS " + path;
        GameObject root = PrefabUtility.LoadPrefabContents(path);
        try
        {
            int removed = 0;
            foreach (Collider c in root.GetComponentsInChildren<Collider>(true))
            {
                if (c != null) { UnityEngine.Object.DestroyImmediate(c); removed++; }
            }
            foreach (Rigidbody rb in root.GetComponentsInChildren<Rigidbody>(true))
            {
                if (rb != null) { UnityEngine.Object.DestroyImmediate(rb); removed++; }
            }
            if (removed > 0) PrefabUtility.SaveAsPrefabAsset(root, path);
            return "SELECT_FIX path=" + path + " removed=" + removed;
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
    }
}

public sealed class SelectPrefabFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(SelectPrefabFix.Run());
    }
}

// Play-mode diagnostic: dispatch the same story message as the E key so the
// client -> server -> client explore round trip can be verified without
// simulated keyboard input. Not a gameplay entry point.
public static class ExploreProbe
{
    // mode 0: dispatch the E-key message at the player's real position.
    // mode 1: dispatch at an explicitly supplied position (test only).
    public static string Run(float x, float z, bool useCoord)
    {
        var sys = ScriptableFramework.Story.GfxStorySystem.Instance;
        if (!useCoord)
        {
            sys.SendMessage("cast_explore");
            return "cast_explore dispatched (expect explore_result from server)";
        }
        sys.SendMessage("cast_explore_at",
                        ScriptableFramework.BoxedValue.From(x),
                        ScriptableFramework.BoxedValue.From(z));
        return string.Format("cast_explore_at({0:F1},{1:F1}) dispatched", x, z);
    }
}

// Editor-only: read the server's current session grid from the traffic file,
// then read one event position from the server-only table. Both files live
// outside the client build, so this never leaks event data into the player.
public static class ExploreAutoProbe
{
    public static string Run()
    {
        string root = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
        string bin = Path.Combine(root, "App", "ServerModule", "ServerEnv", "bin");
        string traffic = Path.Combine(bin, "Data", "explore_traffic.txt");
        // Read the source project table (GBK). The runtime copy is now the
        // generated WorldEvent.dat consumed by TableConfig.WorldEventProvider.
        string table = Path.Combine(root, "Resource", "Tables", "Server", "WorldEvent.txt");
        if (!File.Exists(traffic) || !File.Exists(table))
            return "AUTO_FAIL missing server files";
        // Several sessions can coexist after a battle rotates. Role ids grow,
        // so the last active grid line is the live client's session.
        string grid = "";
        foreach (string line in File.ReadAllLines(traffic, Encoding.UTF8))
        {
            int i = line.IndexOf("grid=");
            if (i < 0) continue;
            string g = line.Substring(i + 5).Trim();
            if (g.Length > 0) grid = g;
        }
        if (grid.Length == 0) return "AUTO_FAIL no active grid in traffic";
        foreach (string line in File.ReadAllLines(table, Encoding.GetEncoding(936)))
        {
            string[] f = line.Split('\t');
            if (f.Length < 5 || f[1] != grid) continue;   // skips the 2 header rows
            float x = float.Parse(f[3], System.Globalization.CultureInfo.InvariantCulture);
            float z = float.Parse(f[4], System.Globalization.CultureInfo.InvariantCulture);
            var sys = ScriptableFramework.Story.GfxStorySystem.Instance;
            sys.SendMessage("cast_explore_at",
                            ScriptableFramework.BoxedValue.From(x),
                            ScriptableFramework.BoxedValue.From(z));
            return string.Format("AUTO_HIT grid={0} ({1:F1},{2:F1}) dispatched", grid, x, z);
        }
        return "AUTO_FAIL no event for grid " + grid;
    }
}

public sealed class ExploreProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        if (operands.Count == 1 && operands[0].GetString() == "auto")
            return BoxedValue.FromString(ExploreAutoProbe.Run());
        bool useCoord = operands.Count >= 2;
        float x = useCoord ? operands[0].GetFloat() : 0f;
        float z = useCoord ? operands[1].GetFloat() : 0f;
        return BoxedValue.FromString(ExploreProbe.Run(x, z, useCoord));
    }
}
