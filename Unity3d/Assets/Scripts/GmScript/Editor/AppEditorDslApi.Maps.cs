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
    private const string RootDir = @"D:\GitHub\CSharpGameFramework";
    private const string ROOT_DSL = RootDir + @"\Resource\DslFile";
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

    // One battlefield per (tile, slot). Coordinates are the event centre in
    // the tile's 0..512 world space; the 32x32 neighbourhood around it has
    // already been magnified x16 into its own terrain PNG by
    // build_battlefield_art.py, so battlefields never share TerrainData.
    private struct Slot
    {
        public int LevelId, SlotIndex;
        public string TileId, GridKey, Era, Kind;
        public float X, Z;
    }

    private const int SlotsPerTile = 10;
    private const float Neighbourhood = 32f;

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

    private static List<Slot> LoadSlots()
    {
        var list = new List<Slot>();
        string path = Path.Combine(DesignRoot, "battle_slots.txt");
        if (!File.Exists(path)) return list;
        foreach (string line in File.ReadAllLines(path))
        {
            if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;
            string[] p = line.Split('\t');
            if (p.Length < 9) continue;
            list.Add(new Slot
            {
                LevelId = int.Parse(p[0]), TileId = p[1], SlotIndex = int.Parse(p[2]),
                GridKey = p[3], Era = p[4], Kind = p[5],
                X = float.Parse(p[6], System.Globalization.CultureInfo.InvariantCulture),
                Z = float.Parse(p[7], System.Globalization.CultureInfo.InvariantCulture)
            });
        }
        return list;
    }

    private static string SlotSceneName(Slot s)
    {
        return "Battle_" + s.TileId + "_" + s.Era + "_s" + s.SlotIndex.ToString("00");
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

    /// <summary>
    /// Bilinear sample of a design PNG channel. The old RoundToInt lookup
    /// quantised the 256px design into a 129px heightmap in visible steps.
    /// </summary>
    private static Vector2 SampleBilinearRG(Color32[] px, int w, int h, float u, float v)
    {
        float fx = Mathf.Clamp(u, 0f, 1f) * (w - 1);
        float fy = Mathf.Clamp(v, 0f, 1f) * (h - 1);
        int x0 = Mathf.FloorToInt(fx), y0 = Mathf.FloorToInt(fy);
        int x1 = Mathf.Min(x0 + 1, w - 1), y1 = Mathf.Min(y0 + 1, h - 1);
        float tx = fx - x0, ty = fy - y0;
        Color32 a = px[y0 * w + x0], b = px[y0 * w + x1];
        Color32 c = px[y1 * w + x0], d = px[y1 * w + x1];
        float r = Mathf.Lerp(Mathf.Lerp(a.r, b.r, tx), Mathf.Lerp(c.r, d.r, tx), ty);
        float g = Mathf.Lerp(Mathf.Lerp(a.g, b.g, tx), Mathf.Lerp(c.g, d.g, tx), ty);
        return new Vector2(r, g);
    }

    /// <summary>
    /// Write the R/G channels of a design PNG into an EXISTING TerrainData.
    /// Shared by BuildTerrain() and map_terrain_patch() so that a full art
    /// regeneration never has to rebuild the 704 scenes or re-bake NavMesh
    /// references held by those scenes.
    ///
    /// The material band is a CONTINUOUS weight across adjacent layers (with a
    /// slope bias towards rock) instead of the old one-hot four-step lookup,
    /// which was the main cause of the hard-edged "colouring book" look.
    /// </summary>
    private static void ApplyTerrainFromPng(TerrainData td, string png, out float minH, out float maxH)
    {
        if (!File.Exists(png)) throw new Exception("missing terrain PNG " + png);
        int tw, th;
        Color32[] tp = ReadPng(png, out tw, out th);
        int res = td.heightmapResolution;
        td.size = new Vector3(WorldSize, TerrainHeight, WorldSize);
        float[,] heights = new float[res, res];
        minH = 1f; maxH = 0f;
        for (int z = 0; z < res; ++z)
        {
            for (int x = 0; x < res; ++x)
            {
                float u = (float)x / (res - 1), v = (float)z / (res - 1);
                float val = SampleBilinearRG(tp, tw, th, u, v).x / 255f;
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
                    Vector2 rg = SampleBilinearRG(tp, tw, th, u, v);
                    // local terrain slope, sampled from the design height
                    float du = 1f / Mathf.Max(1, tw - 1);
                    float dv = 1f / Mathf.Max(1, th - 1);
                    float hx = SampleBilinearRG(tp, tw, th, u + du, v).x -
                               SampleBilinearRG(tp, tw, th, u - du, v).x;
                    float hz = SampleBilinearRG(tp, tw, th, u, v + dv).x -
                               SampleBilinearRG(tp, tw, th, u, v - dv).x;
                    float slope = Mathf.Sqrt(hx * hx + hz * hz) / 255f;
                    // G in 0..255 -> continuous position across the 4 layers;
                    // steep ground is biased towards the rock/snow end.
                    float t = Mathf.Clamp(rg.y / 64f + Mathf.Clamp(slope * 3.2f, 0f, 1f) * 0.85f,
                                          0f, layers - 1 + 0.001f);
                    int la = Mathf.Clamp(Mathf.FloorToInt(t), 0, layers - 1);
                    int lb = Mathf.Min(la + 1, layers - 1);
                    float f = Mathf.Clamp01(t - la);
                    alpha[z, x, la] = 1f - f;
                    alpha[z, x, lb] += f;
                }
            }
            td.SetAlphamaps(0, 0, alpha);
        }
        // Design PNGs carry no tree layer; drop trees inherited from Level2.
        td.treeInstances = new TreeInstance[0];
        td.treePrototypes = new TreePrototype[0];
        EditorUtility.SetDirty(td);
    }

    private static TerrainData BuildTerrain(Tile t, string assetPath, string pngName, out float minH, out float maxH)
    {
        TerrainData td = AssetDatabase.LoadAssetAtPath<TerrainData>(assetPath);
        if (td == null)
        {
            TerrainData src = AssetDatabase.LoadAssetAtPath<TerrainData>(SourceTerrain);
            if (src == null) throw new Exception("missing base terrain " + SourceTerrain);
            td = UnityEngine.Object.Instantiate(src);
            AssetDatabase.CreateAsset(td, assetPath);
        }
        ApplyTerrainFromPng(td, Path.Combine(TileDir, pngName), out minH, out maxH);
        return td;
    }

    /// <summary>
    /// Build a six-sided skybox material from the Bluesky textures already in
    /// the repo and place it under Resources so Game.cs can apply it at runtime
    /// (no need to edit 704 scene files).
    /// </summary>
    public static string BuildSkybox()
    {
        const string dir = "Assets/Resources/Materials";
        const string path = dir + "/BattleSkybox.mat";
        Directory.CreateDirectory(dir);
        var shader = Shader.Find("Skybox/6 Sided");
        if (null == shader) return "SKYBOX_FAIL missing shader Skybox/6 Sided";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (null == mat)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }
        mat.shader = shader;
        string baseDir = "Assets/Content/Textures/Skybox";
        var slots = new[] {
            new KeyValuePair<string,string>("Front", "_FrontTex"),
            new KeyValuePair<string,string>("Back", "_BackTex"),
            new KeyValuePair<string,string>("Left", "_LeftTex"),
            new KeyValuePair<string,string>("Right", "_RightTex"),
            new KeyValuePair<string,string>("Up", "_UpTex"),
            new KeyValuePair<string,string>("Down", "_DownTex"),
        };
        int ok = 0;
        foreach (var kv in slots)
        {
            string texPath = baseDir + "/Bluesky " + kv.Key + ".jpg";
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            if (null == tex) continue;
            mat.SetTexture(kv.Value, tex);
            ok++;
        }
        mat.SetFloat("_Exposure", 1.05f);
        EditorUtility.SetDirty(mat);
        AssetDatabase.SaveAssets();
        return "MAP_SKYBOX textures=" + ok + " path=" + path;
    }

    /// <summary>
    /// Configure the 4096 treasure map so Unity does not downscale it to the
    /// project default 2048, and keep mipmaps on for the minimap zoom levels.
    /// </summary>
    public static string SetupWorldMapTexture()
    {
        const string path = "Assets/Resources/UITexture/WorldMap.png";
        if (!File.Exists(path)) return "WORLDMAP_FAIL missing " + path;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        var imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (null == imp) return "WORLDMAP_FAIL no TextureImporter";
        imp.textureType = TextureImporterType.Default;
        imp.maxTextureSize = 4096;
        imp.mipmapEnabled = true;
        imp.textureCompression = TextureImporterCompression.Compressed;
        imp.sRGBTexture = true;
        imp.alphaIsTransparency = false;
        imp.SaveAndReimport();
        return "MAP_WORLDMAP_TEXTURE path=" + path + " max=4096";
    }

    /// <summary>
    /// Upgrade the four SHARED TerrainLayer assets with normal maps, a tighter
    /// tile size and mild specular response. All 704 TerrainData assets point
    /// at these layers, so one pass upgrades every scene without touching a
    /// single .unity file or NavMesh reference.
    /// </summary>
    public static string PatchTerrainMaterials()
    {
        var sb = new StringBuilder();
        var names = new[] {
            "LvZhouCaoDi_01", "ChengZhen_DiZhuan_02",
            "LvZhouSha_01", "YiZhan_HuangTuDi_02",
        };
        int layersDone = 0, normalsFound = 0;
        try
        {
            // Import the generated normal maps with the right texture type.
            foreach (string n in names)
            {
                string path = "Assets/Resources/TerrainNormals/" + n + "_normal.png";
                if (!File.Exists(path)) { sb.AppendLine("MISS_NORMAL " + n); continue; }
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
                var imp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (null != imp)
                {
                    if (imp.textureType != TextureImporterType.NormalMap || imp.mipmapEnabled == false)
                    {
                        imp.textureType = TextureImporterType.NormalMap;
                        imp.mipmapEnabled = true;
                        imp.SaveAndReimport();
                    }
                }
                normalsFound++;
            }

            var td = AssetDatabase.LoadAssetAtPath<TerrainData>(
                DataDir + "/r0c0_s00_Ground.asset");
            if (null == td) throw new Exception("missing sample TerrainData");
            var layers = td.terrainLayers;
            for (int i = 0; i < layers.Length && i < names.Length; ++i)
            {
                var layer = layers[i];
                if (null == layer) continue;
                string npath = "Assets/Resources/TerrainNormals/" + names[i] + "_normal.png";
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(npath);
                if (null != tex)
                {
                    layer.normalMapTexture = tex;
                    layer.normalScale = 0.85f;
                }
                // 15m tiles made the 512m terrain read as one flat smear;
                // 6m tiles keep visible surface grain at gameplay camera distance.
                layer.tileSize = new Vector2(6f, 6f);
                layer.smoothness = 0.12f;
                layer.metallic = 0f;
                EditorUtility.SetDirty(layer);
                layersDone++;
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception e) { sb.AppendLine("MATERIAL_FAIL " + e); }
        sb.Insert(0, string.Format("MAP_TERRAIN_MATERIALS layers={0} normals={1}\n",
            layersDone, normalsFound));
        return sb.ToString();
    }

    /// <summary>
    /// In-place refresh of all 704 TerrainData assets from the design PNGs.
    /// Does NOT touch any .unity scene, so the baked NavMesh references stay
    /// intact. Optional tile id limits the refresh to one tile's 11 assets.
    /// </summary>
    public static string PatchTerrains(string which = "")
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
        {
            tiles = tiles.Where(t => t.Id == which).ToList();
            slots = slots.Where(s => s.TileId == which).ToList();
        }
        int done = 0, missing = 0;
        try
        {
            var jobs = new List<KeyValuePair<string, string>>();
            foreach (Tile t in tiles)
                jobs.Add(new KeyValuePair<string, string>(
                    t.Id + "_Ground.asset", t.Id + "_terrain.png"));
            foreach (Slot s in slots)
                jobs.Add(new KeyValuePair<string, string>(
                    s.GridKey + "_Ground.asset", s.GridKey + "_terrain.png"));
            foreach (var job in jobs)
            {
                string assetPath = DataDir + "/" + job.Key;
                var td = AssetDatabase.LoadAssetAtPath<TerrainData>(assetPath);
                string png = Path.Combine(TileDir, job.Value);
                if (null == td || !File.Exists(png)) { missing++; sb.AppendLine("MISS " + job.Key); continue; }
                float minH, maxH;
                ApplyTerrainFromPng(td, png, out minH, out maxH);
                done++;
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception e) { sb.AppendLine("PATCH_FAIL " + e); }
        sb.Insert(0, string.Format("MAP_TERRAIN_PATCH jobs={0} updated={1} missing={2}\n",
            tiles.Count + slots.Count, done, missing));
        return sb.ToString();
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
        sb.AppendLine(string.Format("    create_npc(1000,vector3({0:F0},0,{1:F0}),0,3,1,\"ai_player\",string_list(\"Ai/ailogic_player.dsl\"));",
            portal.X, portal.Z));
        sb.AppendLine("    set_leader_id(unit_id_2_obj_id(1000));");
        int uid = 1006;
        foreach (Area a in battles)
        {
            sb.AppendLine(string.Format("    loop(6){{ create_npc({0}+$$,vector3({1:F0}+rnd_int(0,8),0,{2:F0}+rnd_int(0,8)),0,4,$$+6,\"ai_normal\",string_list(\"Ai/ailogic_normal.dsl\")); }};",
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

    // Spectator-style view: a high, pulled-back oblique camera. distance=104
    // (with up=0) gives camDist=104; at FOV 50 the ground width is ~176 units
    // (~1/3 of a 512 tile), double the previous 52 view.
    // Keep FOV at 50: OverHeadBar scales itself from Camera.main.fieldOfView.
    // Mirror CameraSetting.BattleDistance/Up/Pitch (runtime override). Kept as
    // literals because CameraSetting lives under the special Gizmos folder.
    // Up must stay 0 or the player slides toward the bottom hint area.
    private const float BattleCamDistance = 104f;
    private const float BattleCamUp = 0f;
    private const float BattleCamPitch = 46f;

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
    private static void ApplyBattleCamera(Camera cam, TerrainData td, Vector3 origin)
    {
        Quaternion rot = Quaternion.Euler(BattleCamPitch, 0f, 0f);
        Vector3 anchor = new Vector3(60f, 0f, 70f);
        if (td != null)
            anchor.y = td.GetInterpolatedHeight(60f / WorldSize, 70f / WorldSize);
        cam.transform.position = anchor + origin + Vector3.up * BattleCamUp - rot * Vector3.forward * BattleCamDistance;
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
        List<Slot> slots = LoadSlots();
        string current = SceneManager.GetActiveScene().path;
        int fixedCount = 0;
        var scenes = new List<KeyValuePair<string, Vector3>>();
        foreach (Tile t in tiles)
            scenes.Add(new KeyValuePair<string, Vector3>("Battle_" + t.Id, Vector3.zero));
        foreach (Slot s in slots)
            scenes.Add(new KeyValuePair<string, Vector3>(SlotSceneName(s),
                ScriptableFramework.SceneOrigin.BattleOffset));
        try
        {
            foreach (var entry in scenes)
            {
                string name = entry.Key;
                Vector3 origin = entry.Value;
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
                    foreach (GameObject go in sc.GetRootGameObjects())
                        if (go != null && go.name.StartsWith("Terrain_")) go.transform.position = origin;
                    ApplyBattleCamera(cam, td, origin);
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

    // One .unity per world tile or battlefield. Each battlefield owns its own
    // TerrainData (magnified neighbourhood); no two scenes share terrain.
    // World scenes additionally get Config/1..N waypoint nodes: autorun.dsl
    // calls get_child_count("Config") and get_position("Config/"+index), so
    // the children must be active GameObjects named with those indices.
    // ---- Stage 3 set dressing (design doc 43) -----------------------------
    // Runtime props are authored INTO the scene so every battlefield has a
    // stable, deterministic silhouette. They are deliberately NOT marked
    // NavigationStatic: baked NavMesh must stay terrain-only, otherwise the
    // two-wave encounter can be blocked by a tree or a house.

    private const string PropDir = "Assets/Resources/SceneProps/";
    private const float WaterR = 17f / 255f;   // matches terrain_synth water_level

    private static GameObject LoadProp(string name)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>(PropDir + name + ".prefab");
    }

    private static Material s_FallbackPropMat = null;

    private static Material FallbackPropMat()
    {
        if (null == s_FallbackPropMat)
        {
            var sh = Shader.Find("Standard");
            if (null != sh)
            {
                s_FallbackPropMat = new Material(sh);
                s_FallbackPropMat.name = "SceneryFallback";
                s_FallbackPropMat.color = new Color(0.24f, 0.36f, 0.20f);
                s_FallbackPropMat.SetFloat("_Glossiness", 0.12f);
            }
        }
        return s_FallbackPropMat;
    }

    /// <summary>
    /// Legacy tree prefabs use a bright pink foliage atlas. Standard shader
    /// multiplies m.color over the texture, so a deep green tint neutralises
    /// the pink while keeping all leaf detail. One cloned material per source
    /// material is created as a scene-local asset and shared by every prop.
    /// </summary>
    private static readonly Dictionary<int, Material> s_TintedMats =
        new Dictionary<int, Material>();

    private static void TintFoliage(GameObject go)
    {
        foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
        {
            if (null == r) continue;
            Material[] src = r.sharedMaterials;
            if (null == src || src.Length == 0) continue;
            var dst = new Material[src.Length];
            for (int i = 0; i < src.Length; ++i)
            {
                Material m = src[i];
                if (null == m) { dst[i] = FallbackPropMat(); continue; }
                Material tinted;
                if (!s_TintedMats.TryGetValue(m.GetInstanceID(), out tinted))
                {
                    tinted = new Material(m);
                    tinted.name = m.name + "_Foliage";
                    tinted.color = new Color(0.34f, 0.52f, 0.30f, m.color.a);
                    string p = "Assets/Resources/Materials/Foliage_" +
                               Mathf.Abs(m.GetInstanceID()) + ".mat";
                    var existing = AssetDatabase.LoadAssetAtPath<Material>(p);
                    if (null != existing) AssetDatabase.DeleteAsset(p);
                    AssetDatabase.CreateAsset(tinted, p);
                    s_TintedMats[m.GetInstanceID()] = tinted;
                }
                dst[i] = tinted;
            }
            r.sharedMaterials = dst;
        }
    }

    private static void PlaceProp(GameObject prefab, Transform parent, Vector3 pos,
                                  float yaw, float scale, string name, bool tintFoliage = false)
    {
        if (null == prefab) return;
        GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (null == go) return;
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        go.transform.localScale = Vector3.one * scale;
        // Scenery is decorative only: a collider would intercept the click-to-
        // move Raycast in Game.cs and turn a tree into a movement target.
        foreach (Collider c in go.GetComponentsInChildren<Collider>(true))
            if (null != c) UnityEngine.Object.DestroyImmediate(c);
        // Some legacy prop materials reference a missing shader and render
        // bright magenta. Replace only broken ones; healthy ones keep their art.
        foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
        {
            if (null == r) continue;
            Material[] mats = r.sharedMaterials;
            if (null == mats) continue;
            bool changed = false;
            for (int i = 0; i < mats.Length; ++i)
            {
                Material m = mats[i];
                bool bad = (null == m) || (null == m.shader) ||
                           m.shader.name.Contains("InternalError") ||
                           m.shader.name.Contains("Hidden/");
                if (bad) { mats[i] = FallbackPropMat(); changed = true; }
            }
            if (changed) r.sharedMaterials = mats;
        }
        if (tintFoliage) TintFoliage(go);
    }

    private static void DressScene(Scene sc, TerrainData td, Vector3 origin,
                                   string label, bool isEra, Color mood, List<Slot> waypoints)
    {
        var root = new GameObject("Scenery");
        root.transform.position = origin;
        SceneManager.MoveGameObjectToScene(root, sc);

        // ---- water plane: only the low-lying carved areas show through -----
        float waterY = TerrainHeight * WaterR;
        var waterGo = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterGo.name = "Water";
        waterGo.transform.SetParent(root.transform, false);
        waterGo.transform.position = origin + new Vector3(WorldSize * 0.5f, waterY + 0.12f, WorldSize * 0.5f);
        // Slightly inside the tile so the plane never extends past the terrain
        // edge into the sky, and only low-lying carved areas show water.
        waterGo.transform.localScale = new Vector3(WorldSize * 0.094f, 1f, WorldSize * 0.094f);
        var waterCol = waterGo.GetComponent<Collider>();
        if (null != waterCol) UnityEngine.Object.DestroyImmediate(waterCol);
        // The legacy Assets/Content/Materials/water.mat points at a missing
        // shader (Hidden/InternalErrorShader) and renders bright magenta.
        // Always build a self-contained Standard transparent material instead.
        const string waterPath = "Assets/Resources/Materials/BattleWater.mat";
        Material waterMat = AssetDatabase.LoadAssetAtPath<Material>(waterPath);
        if (null != waterMat && Shader.Find("Standard") != waterMat.shader)
        {
            AssetDatabase.DeleteAsset(waterPath);
            waterMat = null;
        }
        if (null == waterMat)
        {
            var sh = Shader.Find("Standard");
            if (null != sh)
            {
                waterMat = new Material(sh);
                waterMat.name = "BattleWater";
                AssetDatabase.CreateAsset(waterMat, waterPath);
            }
        }
        if (null != waterMat)
        {
            waterMat.color = new Color(0.16f, 0.34f, 0.46f, 0.80f);
            waterMat.SetFloat("_Mode", 3f);
            waterMat.SetFloat("_Glossiness", 0.92f);
            waterMat.SetFloat("_Metallic", 0.05f);
            waterMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            waterMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            waterMat.SetInt("_ZWrite", 0);
            waterMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            waterMat.renderQueue = 3000;
            EditorUtility.SetDirty(waterMat);
            waterGo.GetComponent<Renderer>().sharedMaterial = waterMat;
        }

        // ---- deterministic scatter ----------------------------------------
        var seed = 2166136261u;
        foreach (char ch in label) { seed ^= ch; seed *= 16777619u; }
        var rnd = new System.Random((int)(seed & 0x7FFFFFFF));
        var tree = LoadProp("PineTree001");
        var tree2 = LoadProp("LargeTree001");
        var dead = LoadProp("DeadTree001");
        var rock = LoadProp("Rock_a");
        var house = LoadProp("House");
        var hall = LoadProp("Building_a");

        // Keep the spawn plateau (world 60,70) and the wave-2 advance clear.
        const float spawnClear = 42f;
        int placed = 0;
        for (int i = 0; i < 150 && placed < 46; ++i)
        {
            float wx = (float)rnd.NextDouble() * WorldSize;
            float wz = (float)rnd.NextDouble() * WorldSize;
            if (Vector2.Distance(new Vector2(wx, wz), new Vector2(60f, 70f)) < spawnClear) continue;
            float u = Mathf.Clamp01(wx / WorldSize), v = Mathf.Clamp01(wz / WorldSize);
            float h = td.GetInterpolatedHeight(u, v);
            if (h < waterY + 0.6f) continue;                 // underwater
            float slope = td.GetSteepness(u, v);
            if (slope > 34f) continue;                        // too steep
            float wy = h;
            GameObject prefab;
            float scale;
            bool isTree;
            double pick = rnd.NextDouble();
            if (slope > 22f || pick < 0.18) { prefab = rock; scale = 0.8f + (float)rnd.NextDouble() * 1.5f; isTree = false; }
            else if (pick < 0.48) { prefab = tree; scale = 0.7f + (float)rnd.NextDouble() * 0.8f; isTree = true; }
            else if (pick < 0.70) { prefab = tree2; scale = 0.7f + (float)rnd.NextDouble() * 0.7f; isTree = true; }
            else { prefab = dead; scale = 0.8f + (float)rnd.NextDouble() * 0.7f; isTree = true; }
            if (null == prefab) continue;
            PlaceProp(prefab, root.transform,
                      origin + new Vector3(wx, wy, wz),
                      (float)rnd.NextDouble() * 360f, scale, "Prop_" + placed, isTree);
            placed++;
        }

        // ---- a small ruin cluster, away from the fighting ground -----------
        if (isEra)
        {
            float bx = 300f + (float)rnd.NextDouble() * 120f;
            float bz = 300f + (float)rnd.NextDouble() * 120f;
            float bu = Mathf.Clamp01(bx / WorldSize), bv = Mathf.Clamp01(bz / WorldSize);
            float by = td.GetInterpolatedHeight(bu, bv);
            if (by > waterY + 1.5f && td.GetSteepness(bu, bv) < 20f)
            {
                PlaceProp(house, root.transform, origin + new Vector3(bx, by, bz),
                          (float)rnd.NextDouble() * 360f, 1.0f, "Ruin_House");
                PlaceProp(hall, root.transform, origin + new Vector3(bx + 26f, by, bz + 8f),
                          (float)rnd.NextDouble() * 360f, 0.9f, "Ruin_Hall");
                PlaceProp(rock, root.transform, origin + new Vector3(bx - 18f, by, bz + 20f),
                          0f, 1.2f, "Ruin_Rock");
            }
        }

        // ---- event beacons from the slot waypoints -------------------------
        if (null != waypoints)
        {
            var beamMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/Materials/Beacon.mat");
            if (null == beamMat)
            {
                var sh = Shader.Find("Standard");
                if (null != sh)
                {
                    beamMat = new Material(sh);
                    beamMat.color = new Color(0.95f, 0.78f, 0.32f, 0.55f);
                    beamMat.SetFloat("_Mode", 3f);
                    beamMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    beamMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    beamMat.SetInt("_ZWrite", 0);
                    beamMat.EnableKeyword("_EMISSION");
                    beamMat.SetColor("_EmissionColor", new Color(1.0f, 0.72f, 0.24f) * 1.4f);
                    beamMat.renderQueue = 3000;
                    AssetDatabase.CreateAsset(beamMat, "Assets/Resources/Materials/Beacon.mat");
                }
            }
            for (int i = 0; i < waypoints.Count; ++i)
            {
                Slot s = waypoints[i];
                float u = Mathf.Clamp01(s.X / WorldSize), v = Mathf.Clamp01(s.Z / WorldSize);
                float h = td.GetInterpolatedHeight(u, v);
                var beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                beam.name = "Beacon_" + (i + 1);
                beam.transform.SetParent(root.transform, false);
                beam.transform.position = origin + new Vector3(s.X, h + 5.0f, s.Z);
                beam.transform.localScale = new Vector3(1.1f, 5.0f, 1.1f);
                var col = beam.GetComponent<Collider>();
                if (null != col) UnityEngine.Object.DestroyImmediate(col);
                if (null != beamMat) beam.GetComponent<Renderer>().sharedMaterial = beamMat;
            }
        }
    }

    private static void MakeScene(Tile t, TerrainData td, Color tint, string path, bool isEra, string label,
                                  List<Slot> waypoints = null)
    {
        Scene sc = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        // Battlefield scenes are authored at a fixed far-away origin so the
        // world tile (server-owned coordinates) and the battlefield can both
        // stay loaded. World-tile scenes keep the zero origin.
        Vector3 origin = isEra ? ScriptableFramework.SceneOrigin.BattleOffset : Vector3.zero;
        GameObject terrainGo = Terrain.CreateTerrainGameObject(td);
        terrainGo.name = "Terrain_" + label;
        terrainGo.transform.position = origin;
        foreach (Transform tr in terrainGo.GetComponentsInChildren<Transform>(true))
            GameObjectUtility.SetStaticEditorFlags(tr.gameObject, StaticEditorFlags.NavigationStatic);

        if (null != waypoints && waypoints.Count > 0)
        {
            var config = new GameObject("Config");
            for (int i = 0; i < waypoints.Count; ++i)
            {
                Slot s = waypoints[i];
                var node = new GameObject((i + 1).ToString());
                node.transform.SetParent(config.transform, false);
                float u = Mathf.Clamp01(s.X / WorldSize);
                float v = Mathf.Clamp01(s.Z / WorldSize);
                float h = td.GetInterpolatedHeight(u, v);
                node.transform.position = new Vector3(s.X, h + 1.0f, s.Z) + origin;
            }
        }

        Color mood = isEra
            ? new Color(tint.r * 0.72f, tint.g * 0.70f, tint.b * 0.80f)
            : tint;
        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        Camera cam = camGo.AddComponent<Camera>();
        // Gameplay pose: a close third-person view anchored on the party spawn
        // point (60,70). CameraController only rewrites position while it has a
        // follow target, so the saved pose must already be playable.
        ApplyBattleCamera(cam, td, origin);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = mood * 0.66f;
        cam.farClipPlane = 1200f;

        var lightGo = new GameObject("Sun");
        Light sun = lightGo.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.color = Color.Lerp(Color.white, mood, 0.35f);
        sun.intensity = isEra ? 1.05f : 1.25f;
        lightGo.transform.position = origin;
        lightGo.transform.rotation = Quaternion.Euler(48f, 20f + (t.Seq % 8) * 28f + (isEra ? 15f : 0f), 0f);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = mood * (isEra ? 0.72f : 0.85f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = mood * 0.66f;
        RenderSettings.fogDensity = isEra ? 0.0012f : 0.0008f;

        var marker = new GameObject(isEra ? "EraLayer" : "ModernLayer");
        marker.tag = "EditorOnly";
        marker.transform.position = origin;

        Directory.CreateDirectory("Assets/Resources/Materials");
        DressScene(sc, td, origin, label, isEra, mood, waypoints);
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

    // Design doc 35: 64 world tiles + 64x10 battlefields, each battlefield
    // with its own magnified 32x32 neighbourhood terrain. Story skeletons for
    // battlefields are generated by build_story.py (stage 3), not here.
    public static string Build(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
        {
            tiles = tiles.Where(t => t.Id == which).ToList();
            slots = slots.Where(s => s.TileId == which).ToList();
        }
        Directory.CreateDirectory(DataDir);
        Directory.CreateDirectory("Assets/Resources/Minimap");
        Directory.CreateDirectory("Assets/Resources/MapData");
        Directory.CreateDirectory(Path.Combine(DesignRoot, "stories"));
        string current = SceneManager.GetActiveScene().path;
        int okWorld = 0, okBattle = 0;
        var scenePaths = new List<string>();
        try
        {
            // ---- world / modern survey layer: one full-tile terrain per tile ----
            foreach (Tile t in tiles)
            {
                string terrainPng = t.Id + "_terrain.png";
                if (!File.Exists(Path.Combine(TileDir, terrainPng)))
                { sb.AppendLine("MISS_WORLD_TERRAIN " + t.Id); continue; }
                float minH, maxH;
                TerrainData td = BuildTerrain(t, DataDir + "/" + t.Id + "_Ground.asset",
                                              terrainPng, out minH, out maxH);
                string src = Path.Combine(TileDir, t.Id + "_minimap.png");
                if (File.Exists(src))
                {
                    string dst = "Assets/Resources/Minimap/" + t.Id + "_minimap.png";
                    File.Copy(src, dst, true);
                    AssetDatabase.ImportAsset(dst, ImportAssetOptions.ForceSynchronousImport);
                }
                Color tint = Tints.ContainsKey(t.Code) ? Tints[t.Code] : Color.white;
                string path = SceneDir + "/Battle_" + t.Id + ".unity";
                List<Slot> tileSlots = slots.Where(s => s.TileId == t.Id).OrderBy(s => s.SlotIndex).ToList();
                MakeScene(t, td, tint, path, false, t.Id, tileSlots);
                scenePaths.Add(path);
                okWorld++;
            }

            // ---- 640 battlefields: independent magnified terrain per slot ----
            var tileBy = tiles.ToDictionary(t => t.Id, t => t);
            foreach (Slot s in slots)
            {
                if (!tileBy.ContainsKey(s.TileId)) { sb.AppendLine("NO_TILE " + s.GridKey); continue; }
                Tile t = tileBy[s.TileId];
                string terrainPng = s.GridKey + "_terrain.png";
                if (!File.Exists(Path.Combine(TileDir, terrainPng)))
                { sb.AppendLine("MISS_BATTLE_TERRAIN " + s.GridKey); continue; }
                float minH, maxH;
                TerrainData td = BuildTerrain(t, DataDir + "/" + s.GridKey + "_Ground.asset",
                                              terrainPng, out minH, out maxH);
                string src = Path.Combine(TileDir, s.GridKey + "_minimap.png");
                if (File.Exists(src))
                {
                    string dst = "Assets/Resources/Minimap/" + s.GridKey + "_minimap.png";
                    File.Copy(src, dst, true);
                    AssetDatabase.ImportAsset(dst, ImportAssetOptions.ForceSynchronousImport);
                }
                Color tint = Tints.ContainsKey(t.Code) ? Tints[t.Code] : Color.white;
                string path = SceneDir + "/" + SlotSceneName(s) + ".unity";
                MakeScene(t, td, tint, path, true, s.GridKey);
                scenePaths.Add(path);
                okBattle++;
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
        sb.Insert(0, string.Format("MAP_BUILD tiles={0} world_scenes={1} battlefields={2} scenes={3} slots_source={4}\n",
            tiles.Count, okWorld, okBattle, okWorld + okBattle, slots.Count));
        return sb.ToString();
    }

    // Stage 1 of the world-layer cutover (design doc 36): rebuild ONLY the 64
    // world-map scenes. Never touch battlefield .unity files here -- those hold
    // the 640 baked NavMesh references from map_nav().
    public static string BuildWorld(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
        {
            tiles = tiles.Where(t => t.Id == which).ToList();
            slots = slots.Where(s => s.TileId == which).ToList();
        }
        Directory.CreateDirectory(DataDir);
        Directory.CreateDirectory("Assets/Resources/Minimap");
        string current = SceneManager.GetActiveScene().path;
        int ok = 0;
        var scenePaths = new List<string>();
        try
        {
            foreach (Tile t in tiles)
            {
                string png = t.Id + "_terrain.png";
                if (!File.Exists(Path.Combine(TileDir, png)))
                { sb.AppendLine("MISS_WORLD_TERRAIN " + t.Id); continue; }
                float minH, maxH;
                TerrainData td = BuildTerrain(t, DataDir + "/" + t.Id + "_Ground.asset",
                                              png, out minH, out maxH);
                string src = Path.Combine(TileDir, t.Id + "_minimap.png");
                if (File.Exists(src))
                {
                    string dst = "Assets/Resources/Minimap/" + t.Id + "_minimap.png";
                    File.Copy(src, dst, true);
                    AssetDatabase.ImportAsset(dst, ImportAssetOptions.ForceSynchronousImport);
                }
                Color tint = Tints.ContainsKey(t.Code) ? Tints[t.Code] : Color.white;
                string path = SceneDir + "/Battle_" + t.Id + ".unity";
                List<Slot> tileSlots = slots.Where(s => s.TileId == t.Id)
                                            .OrderBy(s => s.SlotIndex).ToList();
                MakeScene(t, td, tint, path, false, t.Id, tileSlots);
                scenePaths.Add(path);
                ok++;
                sb.AppendLine(string.Format("WORLD {0} waypoints={1}", t.Id, tileSlots.Count));
            }
        }
        catch (Exception e) { sb.AppendLine("WORLD_BUILD_FAIL " + e); }
        finally
        {
            AssetDatabase.SaveAssets();
            int added = RegisterBuildScenes(scenePaths);
            sb.AppendLine("BUILD_SETTINGS added=" + added + " total=" + EditorBuildSettings.scenes.Length);
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, "MAP_WORLD_BUILD tiles=" + tiles.Count + " built=" + ok + "\n");
        return sb.ToString();
    }

    // Bake NavMesh for world-map scenes only (autorun.dsl obj_move needs it).
    public static string BakeWorldNav(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        if (!string.IsNullOrEmpty(which)) tiles = tiles.Where(t => t.Id == which).ToList();
        string current = SceneManager.GetActiveScene().path;
        int baked = 0;
        try
        {
            foreach (Tile t in tiles)
            {
                string sceneName = "Battle_" + t.Id;
                string path = SceneDir + "/" + sceneName + ".unity";
                if (!File.Exists(path)) continue;
                if (File.Exists(SceneDir + "/" + sceneName + "/NavMesh.asset")) continue;
                Directory.CreateDirectory(SceneDir + "/" + sceneName);
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                List<GameObject> scenery = new List<GameObject>();
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    if (go == null) continue;
                    if (go.name == "Scenery")
                    {
                        scenery.Add(go);
                        go.SetActive(false);
                        continue;
                    }
                    foreach (Transform tr in go.GetComponentsInChildren<Transform>(true))
                        GameObjectUtility.SetStaticEditorFlags(tr.gameObject, StaticEditorFlags.NavigationStatic);
                }
                UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
                foreach (GameObject go in scenery)
                    if (null != go) go.SetActive(true);
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                baked++;
            }
        }
        catch (Exception e) { sb.AppendLine("WORLD_NAV_FAIL " + e); }
        finally
        {
            AssetDatabase.SaveAssets();
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, "MAP_WORLD_NAV tiles=" + tiles.Count + " baked=" + baked + "\n");
        return sb.ToString();
    }

    public static string Verify(string which)
    {
        var sb = new StringBuilder();
        List<Tile> tiles = LoadTiles();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
        {
            tiles = tiles.Where(t => t.Id == which).ToList();
            slots = slots.Where(s => s.TileId == which).ToList();
        }
        int pass = 0, total = 0;
        // World tiles: scene + full-tile terrain + minimap.
        foreach (Tile t in tiles)
        {
            total++;
            bool sceneOk = File.Exists(SceneDir + "/Battle_" + t.Id + ".unity");
            bool groundOk = File.Exists(DataDir + "/" + t.Id + "_Ground.asset");
            bool miniOk = File.Exists("Assets/Resources/Minimap/" + t.Id + "_minimap.png");
            bool ok = sceneOk && groundOk && miniOk;
            if (ok) pass++;
            sb.AppendLine(string.Format("VERIFY_WORLD {0} scene={1} ground={2} mini={3} => {4}",
                t.Id, sceneOk, groundOk, miniOk, ok ? "PASS" : "FAIL"));
        }
        // Battlefields: scene + independent magnified terrain + minimap + areas + story.
        foreach (Slot s in slots)
        {
            total++;
            string scene = SceneDir + "/" + SlotSceneName(s) + ".unity";
            string ground = DataDir + "/" + s.GridKey + "_Ground.asset";
            string mini = "Assets/Resources/Minimap/" + s.GridKey + "_minimap.png";
            string areas = "Assets/Resources/MapData/" + s.GridKey + "_areas.txt";
            string story = Path.Combine(ROOT_DSL, "Story", SlotSceneName(s), "story.dsl");
            bool ok = File.Exists(scene) && File.Exists(ground) && File.Exists(mini) &&
                      File.Exists(areas) && File.Exists(story);
            if (ok) pass++;
            sb.AppendLine(string.Format("VERIFY_BATTLE {0} scene={1} ground={2} mini={3} areas={4} story={5} => {6}",
                s.GridKey, File.Exists(scene), File.Exists(ground), File.Exists(mini),
                File.Exists(areas), File.Exists(story), ok ? "PASS" : "FAIL"));
        }
        sb.Insert(0, "MAP_VERIFY total=" + total + " pass=" + pass + " fail=" + (total - pass) + "\n");
        return sb.ToString();
    }

    /// <summary>
    /// Move already-built battlefield scenes (and their baked NavMesh data) to
    /// the fixed battlefield origin. Re-baking 640 scenes takes hours; a baked
    /// NavMeshData is a self-contained object with its own position/rotation,
    /// so shifting it by the same delta keeps terrain, physics and NavMesh in
    /// one frame without a rebuild. Idempotent: a scene already at the origin
    /// is skipped. World-tile scenes are never touched.
    /// </summary>
    public static string ApplyBattleOrigin(string which = "")
    {
        var sb = new StringBuilder();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
            slots = slots.Where(s => s.TileId == which).ToList();
        string current = SceneManager.GetActiveScene().path;
        Vector3 origin = ScriptableFramework.SceneOrigin.BattleOffset;
        int moved = 0, skipped = 0, navShifted = 0, navMissed = 0;
        try
        {
            foreach (Slot s in slots)
            {
                string sceneName = SlotSceneName(s);
                string path = SceneDir + "/" + sceneName + ".unity";
                if (!File.Exists(path)) { skipped++; continue; }
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                GameObject terrainGo = null;
                foreach (GameObject go in sc.GetRootGameObjects())
                    if (go != null && go.name.StartsWith("Terrain_")) { terrainGo = go; break; }
                if (null == terrainGo) { skipped++; continue; }
                Vector3 delta = origin - terrainGo.transform.position;
                if (delta.sqrMagnitude < 0.01f) { skipped++; continue; }
                foreach (GameObject go in sc.GetRootGameObjects())
                    if (go != null) go.transform.position += delta;
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                moved++;

                string navPath = SceneDir + "/" + sceneName + "/NavMesh.asset";
                var data = AssetDatabase.LoadAssetAtPath<UnityEngine.AI.NavMeshData>(navPath);
                if (null == data) { navMissed++; continue; }
                try
                {
                    var so = new SerializedObject(data);
                    var pos = so.FindProperty("m_Position");
                    if (null == pos) { navMissed++; continue; }
                    pos.vector3Value += delta;
                    so.ApplyModifiedPropertiesWithoutUndo();
                    EditorUtility.SetDirty(data);
                    navShifted++;
                }
                catch (Exception e) { navMissed++; sb.AppendLine("NAV_SHIFT_FAIL " + sceneName + ": " + e.Message); }
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception e) { sb.AppendLine("ORIGIN_FAIL " + e); }
        finally
        {
            if (!string.IsNullOrEmpty(current) && File.Exists(current))
                EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        sb.Insert(0, string.Format("MAP_BATTLE_ORIGIN slots={0} moved={1} skipped={2} navShifted={3} navMissed={4}\n",
            slots.Count, moved, skipped, navShifted, navMissed));
        return sb.ToString();
    }

    /// <summary>
    /// Bake exactly one battlefield scene. Public so the sentinel-driven
    /// bootstrap can spread 640 bakes across editor frames (keeping Unity
    /// responsive) without going through the GM channel.
    /// </summary>
    public static bool BakeOneBattleScene(string sceneName)
    {
        string path = SceneDir + "/" + sceneName + ".unity";
        if (!File.Exists(path)) return false;
        Directory.CreateDirectory(SceneDir + "/" + sceneName);
        Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        var scenery = new List<GameObject>();
        foreach (GameObject go in sc.GetRootGameObjects())
        {
            if (go == null) continue;
            // Classic BuildNavMesh() may collect every Mesh regardless of
            // static flags; hide decorative props while baking.
            if (go.name == "Scenery") { scenery.Add(go); go.SetActive(false); continue; }
            foreach (Transform tr in go.GetComponentsInChildren<Transform>(true))
                GameObjectUtility.SetStaticEditorFlags(tr.gameObject, StaticEditorFlags.NavigationStatic);
        }
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        foreach (GameObject go in scenery)
            if (null != go) go.SetActive(true);
        EditorSceneManager.MarkSceneDirty(sc);
        EditorSceneManager.SaveScene(sc);
        return true;
    }

    // Legacy NavMesh data is stored in a folder named after each scene.
    // force=true re-bakes scenes that already have a NavMesh.asset (needed
    // after the terrain is regenerated); the default keeps the resumable
    // "skip existing" behaviour for interrupted runs.
    public static string BakeNav(string which = "", bool force = false)
    {
        var sb = new StringBuilder();
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
            slots = slots.Where(s => s.TileId == which).ToList();
        string current = SceneManager.GetActiveScene().path;
        int baked = 0;
        try
        {
            foreach (Slot s in slots)
            {
                string sceneName = SlotSceneName(s);
                string path = SceneDir + "/" + sceneName + ".unity";
                if (!File.Exists(path)) continue;
                // Baking 640 scenes takes hours; support resuming after a
                // cancelled run instead of rebuilding everything from scratch.
                // force overrides this for a full refresh after terrain changes.
                // Do NOT delete the existing asset: the scene keeps a guid
                // reference to it, and BuildNavMesh() overwrites its content in
                // place. Deleting would break that link for every scene.
                if (!force && File.Exists(SceneDir + "/" + sceneName + "/NavMesh.asset")) continue;
                if (BakeOneBattleScene(sceneName)) baked++;
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
        List<Slot> slots = LoadSlots();
        if (!string.IsNullOrEmpty(which))
        {
            tiles = tiles.Where(t => t.Id == which).ToList();
            slots = slots.Where(s => s.TileId == which).ToList();
        }
        var variants = new List<KeyValuePair<string, string>>();
        foreach (Tile t in tiles)
            variants.Add(new KeyValuePair<string, string>(
                SceneDir + "/Battle_" + t.Id + ".unity", t.Id));
        foreach (Slot s in slots)
            variants.Add(new KeyValuePair<string, string>(
                SceneDir + "/" + SlotSceneName(s) + ".unity", s.GridKey));
        string current = SceneManager.GetActiveScene().path;
        try
        {
            foreach (var v in variants)
            {
                string variantPath = v.Key, variantName = v.Value;
                if (!File.Exists(variantPath)) continue;
                {
                    Scene sc = EditorSceneManager.OpenScene(variantPath, OpenSceneMode.Single);
                    Camera cam = null;
                    foreach (GameObject go in sc.GetRootGameObjects())
                    {
                        cam = go.GetComponent<Camera>();
                        if (cam != null) break;
                    }
                    if (cam == null) { sb.AppendLine("NO_CAM " + variantName); continue; }
                    var rt = new RenderTexture(640, 360, 24);
                    var tex = new Texture2D(640, 360, TextureFormat.RGB24, false);
                    RenderTexture oldTarget = cam.targetTexture, prev = RenderTexture.active;
                    Vector3 savedPos = cam.transform.position;
                    Quaternion savedRot = cam.transform.rotation;
                    float savedFov = cam.fieldOfView;
                    try
                    {
                        // Whole-tile oblique preview only; the saved scene keeps
                        // its close gameplay camera pose. Battlefield scenes
                        // are authored at a fixed offset, so both the camera
                        // and its look-at target must use that same origin.
                        Vector3 previewOrigin = variantName.IndexOf("_s") >= 0
                            ? ScriptableFramework.SceneOrigin.BattleOffset : Vector3.zero;
                        cam.transform.position = new Vector3(20f, 300f, 20f) + previewOrigin;
                        cam.transform.LookAt(new Vector3(256f, 0f, 256f) + previewOrigin);
                        cam.fieldOfView = 60f;
                        cam.targetTexture = rt; cam.Render();
                        RenderTexture.active = rt;
                        tex.ReadPixels(new Rect(0, 0, 640, 360), 0, 0); tex.Apply();
                        string outPath = Path.Combine(dir, variantName + ".png");
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
  string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
  return BoxedValue.FromString(MapBuild.BakeNav(w));
  }
  }

  public sealed class MapNavForceExp : SimpleExpressionBase
  {
  protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
  {
  string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
  return BoxedValue.FromString(MapBuild.BakeNav(w, true));
  }
  }

public sealed class MapWorldBuildExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(MapBuild.BuildWorld(w));
    }
}

public sealed class MapWorldNavExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(MapBuild.BakeWorldNav(w));
    }
}

public sealed class MapCameraFixExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    return BoxedValue.FromString(MapBuild.FixCameras());
    }
    }

    public sealed class MapBattleOriginExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
    return BoxedValue.FromString(MapBuild.ApplyBattleOrigin(w));
    }
    }

    public sealed class MapTerrainPatchExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
    return BoxedValue.FromString(MapBuild.PatchTerrains(w));
    }
    }

    public sealed class MapTerrainMaterialsExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    return BoxedValue.FromString(MapBuild.PatchTerrainMaterials());
    }
    }

    public sealed class MapSkyboxExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    return BoxedValue.FromString(MapBuild.BuildSkybox());
    }
    }

    public sealed class MapWorldMapTextureExp : SimpleExpressionBase
    {
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
    return BoxedValue.FromString(MapBuild.SetupWorldMapTexture());
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
    private const float c_Rad2Deg = 180.0f / (float)Math.PI;

    // Shortest angular distance in degrees, always in [0, 180].
    private static float AngleError(float a, float b)
    {
        float d = (a - b) % 360.0f;
        if (d < 0.0f) d += 360.0f;
        return d > 180.0f ? 360.0f - d : d;
    }

    public static string Run()
    {
        var sb = new StringBuilder();
        // Entity ids are global and monotonically increasing, so scanning a
        // fixed id window misses later battles. Walk from the live HP bars back
        // to their owning actors instead: this also proves which actors got one.
        var bars = UnityEngine.Object.FindObjectsOfType<OverHeadBar>();
        int n = 0;
        int orphans = 0;      // bar exists but its owning entity is gone
        int noParent = 0;     // bar detached from any actor
        int faceCount = 0;
        float errSum = 0.0f, errMax = 0.0f;
        foreach (var bar in bars)
        {
            if (bar == null || bar.transform.parent == null) { noParent++; continue; }
            GameObject owner = bar.transform.parent.gameObject;
            int id = ScriptableFramework.PluginFramework.Instance.GetGameObjectId(owner);
            var e = ScriptableFramework.PluginFramework.Instance.GetEntityById(id);
            if (e == null)
            {
                orphans++;
                sb.AppendLine("E id=" + id + " obj=" + owner.name + " (no entity)");
                continue;
            }
            var ai = e.GetAiStateInfo();
            var ms = e.GetMovementStateInfo();
            float faceDeg = ms.GetFaceDir() * c_Rad2Deg;
            float wantedDeg = ms.GetWantedFaceDir() * c_Rad2Deg;
            sb.Append("E id=" + id +
                      " pos=" + ms.GetPosition3D() +
                      " state=" + ai.CurState +
                      " target=" + ai.Target +
                      " home=" + ai.HomePos +
                      " gohome=" + e.GohomeRange +
                      " view=" + e.ViewRange +
                      " moving=" + ms.IsMoving +
                      " tgtPos=" + ms.TargetPosition +
                      " face=" + faceDeg.ToString("F1") +
                      " wanted=" + wantedDeg.ToString("F1") +
                      " faceErr=" + AngleError(faceDeg, wantedDeg).ToString("F1"));
            // Expected heading: 0 deg = +z, same convention as Geometry.GetYRadian.
            if (ai.Target > 0) {
                var target = ScriptableFramework.PluginFramework.Instance.GetEntityById(ai.Target);
                if (target != null) {
                    var p = ms.GetPosition3D();
                    var t = target.GetMovementStateInfo().GetPosition3D();
                    float expectDeg = (float)Math.Atan2(t.X - p.X, t.Z - p.Z) * c_Rad2Deg;
                    float err = AngleError(faceDeg, expectDeg);
                    sb.Append(" expect=" + expectDeg.ToString("F1") +
                              " expectErr=" + err.ToString("F1"));
                    errSum += err;
                    if (err > errMax) errMax = err;
                    faceCount++;
                }
            }
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
                  " orphans=" + orphans + " noParent=" + noParent +
                  " lockedTarget=" + (sel == null ? 0 : sel.TargetId) +
                  " facing=" + faceCount +
                  " expectErrAvg=" + (faceCount == 0 ? 0.0f : errSum / faceCount).ToString("F1") +
                  " expectErrMax=" + errMax.ToString("F1") + "\n");
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
