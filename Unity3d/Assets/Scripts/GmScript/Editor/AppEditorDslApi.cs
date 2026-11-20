using UnityEngine;
//using UnityEngine.UI;
using UnityEditor;
//using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.MemoryProfiler;
using UnityEditorInternal;
using UnityEditor.Profiling;
using UnityEditor.Profiling.Memory.Experimental;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using StoryScript;
using StoryScript.DslExpression;

public sealed class AppEditorDslApi
{
    [InitializeOnLoadMethod]
    public static void RunOnEditorStartup()
    {
        EditorDslProcessorUtility.OnRegisterApi = Register;
        EditorGmCommandWindow.RunOnEditorStartup();
    }
    private static void Register(DslCalculator calc)
    {
        calc.Register("scene_audit", "scene_audit([asset_path]) - audit editor tool scenes / levels for missing scripts; empty = all", new ExpressionFactoryHelper<SceneAuditExp>());
        calc.Register("scene_terrain_info", "scene_terrain_info(asset_path) - list terrain tree prototypes and missing prefabs", new ExpressionFactoryHelper<SceneTerrainInfoExp>());
        calc.Register("scene_repair", "scene_repair(asset_path[,tree_prefab_path]) - save scene to drop stale components and fix missing tree prototypes", new ExpressionFactoryHelper<SceneRepairExp>());
        calc.Register("res_probe", "res_probe([resource_path]) - verify Resources.Load prefab and report renderers/particles/animators", new ExpressionFactoryHelper<ResProbeExp>());
        calc.Register("scene_build", "scene_build() - generate ten themed battle scenes and register them in BuildSettings", new ExpressionFactoryHelper<SceneBuildExp>());
        calc.Register("actor_probe", "actor_probe([resource_path]) - report actor prefab size and animator/controller/clip state", new ExpressionFactoryHelper<ActorProbeExp>());
        calc.Register("actor_fix", "actor_fix([target_height]) - loop model clips, set default Idle, scale actor prefabs and bind avatars", new ExpressionFactoryHelper<ActorFixExp>());
        calc.Register("prop_scale", "prop_scale([apply]) - measure and normalize ThirdParty scene props to a shared scale spec", new ExpressionFactoryHelper<PropScaleExp>());
        calc.Register("anim_fix", "anim_fix() - bind Generic avatars and repair NavMeshAgent step offsets on actor prefabs", new ExpressionFactoryHelper<AnimFixExp>());
        calc.Register("asset_polish", "asset_polish() - fix scaled CharacterController steps and seat battle scene props on terrain", new ExpressionFactoryHelper<AssetPolishExp>());
        calc.Register("nav_bake", "nav_bake() - bake NavMesh for generated battle scenes and remove duplicate audio listeners", new ExpressionFactoryHelper<NavBakeExp>());
        calc.Register("anim_runtime_probe", "anim_runtime_probe() - inspect live Animators (avatar validity, controller, state time, speed) in Play mode", new ExpressionFactoryHelper<AnimRuntimeProbeExp>());
        calc.Register("controller_fix", "controller_fix() - re-map controller states to matching model clips (handles prefixed clip names)", new ExpressionFactoryHelper<ControllerFixExp>());
        calc.Register("anim_binding_probe", "anim_binding_probe([resource_path]) - compare clip curve binding paths with prefab bone paths", new ExpressionFactoryHelper<AnimBindingProbeExp>());
        calc.Register("animator_host_fix", "animator_host_fix() - move Animator to prefab root so clip bone paths resolve", new ExpressionFactoryHelper<AnimatorHostFixExp>());
        calc.Register("anim_sample_probe", "anim_sample_probe([resource_path[,clip_name]]) - sample a clip and report bone rotation delta", new ExpressionFactoryHelper<AnimSampleProbeExp>());
        calc.Register("scene_catalog", "scene_catalog([battle_number]) - per-scene inventory of terrain, props, fx, navmesh, camera and light", new ExpressionFactoryHelper<SceneCatalogExp>());
        calc.Register("scene_shot", "scene_shot([battle_number]) - render each battle scene camera to e:/tmp/scene_previews/BattleN.png", new ExpressionFactoryHelper<SceneShotExp>());
        calc.Register("scene_fix", "scene_fix([battle_number]) - apply the battle-scene standard (camera/atmosphere/props/navmesh)", new ExpressionFactoryHelper<SceneFixExp>());
        calc.Register("scene_verify", "scene_verify([battle_number]) - audit each battle scene against the standard", new ExpressionFactoryHelper<SceneVerifyExp>());
        calc.Register("map_build", "map_build([tile_id]) - build 64 world tiles + 640 magnified battlefields (design doc 35)", new ExpressionFactoryHelper<MapBuildExp>());
        calc.Register("map_verify", "map_verify([tile_id]) - audit scene/terrain/areas/minimap/story and real relief for all tiles", new ExpressionFactoryHelper<MapVerifyExp>());
        calc.Register("map_shot", "map_shot([tile_id]) - render tile scene cameras to e:/tmp/map_previews", new ExpressionFactoryHelper<MapShotExp>());
        calc.Register("map_nav", "map_nav([tile_id]) - bake missing NavMesh for battlefield scenes (resumable)", new ExpressionFactoryHelper<MapNavExp>());
        calc.Register("map_nav_force", "map_nav_force([tile_id]) - re-bake ALL battlefield NavMeshes in place after a terrain change", new ExpressionFactoryHelper<MapNavForceExp>());
        calc.Register("map_camera_fix", "map_camera_fix() - patch all 704 scene cameras (64 world + 640 battlefields) without rebuilding NavMesh", new ExpressionFactoryHelper<MapCameraFixExp>());
        calc.Register("map_world_build", "map_world_build([tile_id]) - rebuild world-map scenes with Config waypoints, leaving battlefield scenes untouched", new ExpressionFactoryHelper<MapWorldBuildExp>());
        calc.Register("map_battle_origin", "map_battle_origin([tile_id]) - move existing battlefield scenes and their baked NavMesh to the fixed battlefield origin (idempotent, no re-bake)", new ExpressionFactoryHelper<MapBattleOriginExp>());
        calc.Register("map_terrain_patch", "map_terrain_patch([tile_id]) - refresh all 704 TerrainData assets from the design PNGs in place (no scene rebuild, no NavMesh re-bake)", new ExpressionFactoryHelper<MapTerrainPatchExp>());
        calc.Register("map_terrain_materials", "map_terrain_materials() - attach generated normal maps and tighten TerrainLayer tiling on the 4 shared terrain layers", new ExpressionFactoryHelper<MapTerrainMaterialsExp>());
        calc.Register("map_skybox", "map_skybox() - build a six-sided skybox material under Assets/Resources/Materials from the Bluesky textures", new ExpressionFactoryHelper<MapSkyboxExp>());
        calc.Register("map_worldmap_texture", "map_worldmap_texture() - set the 4096 treasure map importer resolution", new ExpressionFactoryHelper<MapWorldMapTextureExp>());
        calc.Register("map_world_nav", "map_world_nav([tile_id]) - bake NavMesh for world-map scenes (resumable)", new ExpressionFactoryHelper<MapWorldNavExp>());
        calc.Register("camera_probe", "camera_probe() - report live camera pose and distance to the party leader", new ExpressionFactoryHelper<CameraProbeExp>());
        calc.Register("combat_probe", "combat_probe() - dump live entity AI state/home/target ranges", new ExpressionFactoryHelper<CombatProbeExp>());
        calc.Register("explore_probe", "explore_probe() - dispatch cast_explore in Play mode to verify the server explore round trip", new ExpressionFactoryHelper<ExploreProbeExp>());
        calc.Register("fix_select_prefab", "fix_select_prefab() - remove colliders/rigidbodies from the selection VFX prefab", new ExpressionFactoryHelper<SelectPrefabFixExp>());
    }
}

// Audit legacy editor tool scenes after the combat system refactor.
public static class SceneAudit
{
    private const string ReportPath = "e:/tmp/editor_scenes_audit.txt";
    private static readonly StringBuilder Sb = new StringBuilder();

    private static readonly string[] Targets =
    {
        "Assets/Scenes/EditNpc.unity",
        "Assets/Scenes/EditUI.unity",
        "Assets/Scenes/SkillViewer.unity",
        "Assets/Scenes/Start.unity",
        "Assets/Scenes/Loading.unity",
        "Assets/Scenes/Levels/Main.unity",
        "Assets/Scenes/Levels/Story1.unity",
        "Assets/Scenes/Levels/Story2.unity",
        "Assets/Scenes/Levels/Battle1.unity",
        "Assets/Scenes/Levels/Battle2.unity"
    };

    private static void L(string s)
    {
        Sb.AppendLine(s);
        Debug.Log("[SceneAudit] " + s);
    }

    private static void Flush()
    {
        File.WriteAllText(ReportPath, Sb.ToString(), new UTF8Encoding(false));
    }

    [MenuItem("Tools/GameRef/Audit Editor Tool Scenes")]
    public static void Run()
    {
        RunToString(string.Empty);
    }

    public static string RunToString(string path)
    {
        Sb.Clear();
        L("AUDIT START Unity=" + Application.unityVersion);
        try
        {
            L("BUILDSCENES " + string.Join(", ", EditorBuildSettings.scenes.Select(s => Path.GetFileNameWithoutExtension(s.path))));
        }
        catch (Exception e) { L("BUILDSCENES_FAIL " + e.Message); }
        Flush();
        if (string.IsNullOrEmpty(path))
        {
            foreach (string p in Targets) AuditOne(p);
        }
        else AuditOne(path);
        L("AUDIT DONE");
        Flush();
        return Sb.ToString();
    }

    private static void AuditOne(string path)
    {
        if (!File.Exists(path)) { L("SCENE MISSING " + path); Flush(); return; }
        Scene sc;
        try { sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single); }
        catch (Exception e) { L("SCENE OPEN_FAIL " + path + " :: " + e.Message); Flush(); return; }

        int missing = 0, objects = 0, components = 0, nullComponents = 0;
        var compTypes = new Dictionary<string, int>();
        var samples = new List<string>();
        foreach (GameObject root in sc.GetRootGameObjects())
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                objects++;
                int ms = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
                if (ms > 0)
                {
                    missing += ms;
                    if (samples.Count < 8) samples.Add(t.name + " = " + ms);
                }
                foreach (Component c in t.GetComponents<Component>())
                {
                    if (c == null) { nullComponents++; continue; }
                    components++;
                    string n = c.GetType().FullName;
                    int cnt;
                    compTypes.TryGetValue(n, out cnt);
                    compTypes[n] = cnt + 1;
                }
            }
        }
        L("SCENE " + path + " roots=" + sc.rootCount + " objects=" + objects +
          " components=" + components + " missingScripts=" + missing + " nullComponents=" + nullComponents);
        foreach (string s in samples) L("  MISSING_SCRIPT " + s);
        foreach (KeyValuePair<string, int> kv in compTypes
            .Where(k => k.Key.Contains("SkillViewer") || k.Key.Contains("NpcConfig") ||
                        k.Key.Contains("ActorRecord") || k.Key.Contains("SkillRecord") ||
                        k.Key.Contains("Game") || k.Key.Contains("Camera") || k.Key.Contains("Terrain"))
            .OrderByDescending(k => k.Value).Take(12))
            L("  COMPONENT " + kv.Key + " x" + kv.Value);
        Flush();
    }
}

public sealed class SceneAuditExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string path = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(SceneAudit.RunToString(path));
    }
}

// Diagnose and repair stale scene components / missing terrain tree prototypes.
public static class SceneRepair
{
    private static Terrain[] Terrains()
    {
        return UnityEngine.Object.FindObjectsByType<Terrain>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public static string Info(string path)
    {
        string current = SceneManager.GetActiveScene().path;
        var sb = new StringBuilder();
        try
        {
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            sb.AppendLine("TERRAIN_INFO " + path);
            foreach (Terrain t in Terrains())
            {
                TerrainData td = t.terrainData;
                if (td == null) continue;
                sb.AppendLine("  TERRAIN " + t.name + " data=" + AssetDatabase.GetAssetPath(td) +
                              " prototypes=" + td.treePrototypes.Length + " instances=" + td.treeInstanceCount);
                for (int i = 0; i < td.treePrototypes.Length; i++)
                    sb.AppendLine("    [" + i + "] " + (td.treePrototypes[i].prefab == null ? "NULL" : AssetDatabase.GetAssetPath(td.treePrototypes[i].prefab)));
            }
        }
        catch (Exception e) { sb.AppendLine("INFO_FAIL " + e.Message); }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }

    public static string Repair(string path, string treePrefabPath)
    {
        string current = SceneManager.GetActiveScene().path;
        var sb = new StringBuilder();
        try
        {
            Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            GameObject tree = string.IsNullOrEmpty(treePrefabPath) ? null : AssetDatabase.LoadAssetAtPath<GameObject>(treePrefabPath);
            int fixedTrees = 0;
            foreach (Terrain t in Terrains())
            {
                TerrainData td = t.terrainData;
                if (td == null) continue;
                TreePrototype[] tps = td.treePrototypes;
                bool changed = false;
                for (int i = 0; i < tps.Length; i++)
                {
                    if (tps[i].prefab == null && tree != null)
                    {
                        tps[i].prefab = tree;
                        changed = true;
                        fixedTrees++;
                    }
                }
                if (changed) { td.treePrototypes = tps; EditorUtility.SetDirty(td); }
            }
            EditorSceneManager.MarkSceneDirty(sc);
            bool saved = EditorSceneManager.SaveScene(sc);
            AssetDatabase.SaveAssets();
            sb.AppendLine("REPAIRED " + path + " saved=" + saved + " treePrototypesFixed=" + fixedTrees +
                          (tree == null ? " (no tree prefab given)" : " tree=" + treePrefabPath));
        }
        catch (Exception e) { sb.AppendLine("REPAIR_FAIL " + e.Message); }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }
}

public sealed class SceneTerrainInfoExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(SceneRepair.Info(operands[0].GetString()));
    }
}

public sealed class SceneRepairExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string path = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        string tree = operands.Count > 1 ? operands[1].GetString() : string.Empty;
        return BoxedValue.FromString(SceneRepair.Repair(path, tree));
    }
}

// Verify that a prefab is reachable through Resources.Load and has usable renderers/particles.
public sealed class ResProbeExp : SimpleExpressionBase
{
    private static readonly string[] Samples =
    {
        "SceneProps/Building_a", "SceneProps/Well", "SceneProps/PineTree001", "SceneProps/Pyramid",
        "Effects/GameRef/7_Guangyun", "Effects/GameRef/7_Huo_03",
        "Monster/Campaign_Dungeon/01_Zombie/5_Mon_Zombie_01",
        "Monster/Campaign_Dungeon/02_Skeleton/5_Mon_SkeletonAxe_01",
        "Monster/Boss/5_BOSS_NiuTou_01",
        "Monster/NonCombat/5_NPC_LaoBanNiang_01",
        "Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_02"
    };

    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        var sb = new StringBuilder();
        string p = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        if (string.IsNullOrEmpty(p))
        {
            foreach (string s in Samples) Probe(s, sb);
        }
        else Probe(p, sb);
        return BoxedValue.FromString(sb.ToString());
    }

    private static void Probe(string p, StringBuilder sb)
    {
        GameObject go = Resources.Load<GameObject>(p);
        if (go == null) { sb.AppendLine("MISS " + p); return; }
        sb.AppendLine("OK " + p + " renderers=" + go.GetComponentsInChildren<Renderer>(true).Length +
                      " particles=" + go.GetComponentsInChildren<ParticleSystem>(true).Length +
                      " animators=" + go.GetComponentsInChildren<Animator>(true).Length);
    }
}

// Generate ten themed battle scenes from ThirdParty props and GameRef effects.
public static class SceneBuild
{
    private static readonly string[][] Themes =
    {
        new[] { "Building_a", "Well", "Tent", "Fence_a", "Barrel" },
        new[] { "DeadTree001", "Rock_a", "Sign_a", "Pyramid", "Cliff_Desert_01" },
        new[] { "PineTree001", "LargeTree001", "Tree4_B_Rot0", "Rock_a", "DeadTree001" },
        new[] { "Pyramid", "Cliff_Desert_01", "Rock_a", "Rock_f", "Sign_a" },
        new[] { "Building_a", "Building_f", "Lantern_post", "Barrel", "Cart_Covered1" },
        new[] { "Tent", "Barrel", "Lantern_post", "Sign_a", "Well" },
        new[] { "Building_f", "Rock_f", "Stairway", "Mill", "Fence_a" },
        new[] { "Cliff_Desert_01", "Rock_a", "DeadTree001", "Sewers_entrance" },
        new[] { "Tent", "Cart_Covered1", "Barrel", "Well", "Baker_house" },
        new[] { "Pyramid", "Lantern_post", "Rock_f", "Sign_a", "House" }
    };
    private static readonly string[] FxNames =
    {
        "7_Guangyun", "7_YingHuoChong_01", "7_Huo_03", "7_LuoYe_01", "7_Yan_01",
        "7_Guangyun_01", "7_DanRuZheZhao_3s_01", "7_Huaban", "7_Scene_Dot_01", "7_Scene_Target_02"
    };

    public static string Build()
    {
        var sb = new StringBuilder();
        string current = SceneManager.GetActiveScene().path;
        var buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        var known = new HashSet<string>(buildScenes.Select(s => s.path));
        try
        {
            for (int i = 0; i < Themes.Length; i++)
            {
                string name = "Battle" + (i + 3);
                string scenePath = "Assets/Scenes/Levels/" + name + ".unity";
                Scene sc = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                GameObject cam = new GameObject("Main Camera");
                cam.tag = "MainCamera";
                cam.AddComponent<Camera>();
                cam.AddComponent<AudioListener>();
                cam.transform.position = new Vector3(60, 35, 60);
                cam.transform.rotation = Quaternion.Euler(40, 45, 0);

                GameObject lightGo = new GameObject("Directional Light");
                Light light = lightGo.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.1f;
                lightGo.transform.rotation = Quaternion.Euler(50, -30 - i * 12, 0);

                TerrainData td = AssetDatabase.LoadAssetAtPath<TerrainData>("Assets/Terrain/Level" + (i % 4 + 1) + ".asset");
                Terrain terrain = null;
                if (td != null)
                {
                    GameObject tgo = Terrain.CreateTerrainGameObject(td);
                    terrain = tgo != null ? tgo.GetComponent<Terrain>() : null;
                }

                var rnd = new System.Random(1000 + i);
                int placed = 0, missing = 0;
                foreach (string prop in Themes[i])
                {
                    GameObject prefab = Resources.Load<GameObject>("SceneProps/" + prop);
                    if (prefab == null) { missing++; continue; }
                    for (int k = 0; k < 2; k++)
                    {
                        float x = 18f + (float)rnd.NextDouble() * 84f;
                        float z = 18f + (float)rnd.NextDouble() * 84f;
                        float y = terrain != null ? terrain.SampleHeight(new Vector3(x, 0, z)) : 0f;
                        GameObject inst = PrefabUtility.InstantiatePrefab(prefab, sc) as GameObject;
                        if (inst == null) continue;
                        inst.transform.position = new Vector3(x, y, z);
                        inst.transform.rotation = Quaternion.Euler(0, (float)rnd.NextDouble() * 360f, 0);
                        placed++;
                    }
                }

                GameObject fxPrefab = Resources.Load<GameObject>("Effects/GameRef/" + FxNames[i]);
                if (fxPrefab != null)
                {
                    GameObject fx = PrefabUtility.InstantiatePrefab(fxPrefab, sc) as GameObject;
                    if (fx != null) fx.transform.position = new Vector3(60, 2, 60);
                }

                EditorSceneManager.SaveScene(sc, scenePath);
                if (!known.Contains(scenePath))
                {
                    buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    known.Add(scenePath);
                }
                sb.AppendLine("BUILT " + scenePath + " props=" + placed + " missingPrefabs=" + missing +
                              " terrain=" + (td != null ? td.name : "none"));
            }
            EditorBuildSettings.scenes = buildScenes.ToArray();
            AssetDatabase.SaveAssets();
        }
        catch (Exception e) { sb.AppendLine("BUILD_FAIL " + e); }
        finally
        {
            if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single);
        }
        return sb.ToString();
    }
}

public sealed class SceneBuildExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(SceneBuild.Build());
    }
}

// Diagnose actor prefab size and animator state (pose / no animation issues).
public static class ActorProbe
{
    private static readonly string[] Samples =
    {
        "Monster/Boss/5_IP_YiNa_01",
        "Monster/Boss/5_IP_AiDa_01",
        "Monster/Campaign_Dungeon/01_Zombie/5_Mon_Zombie_01",
        "Monster/Campaign_Dungeon/02_Skeleton/5_Mon_SkeletonAxe_01",
        "Monster/Boss/5_BOSS_NiuTou_01",
        "Monster/NonCombat/5_NPC_LaoBanNiang_01"
    };

    public static string Run(string path)
    {
        var sb = new StringBuilder();
        if (string.IsNullOrEmpty(path))
        {
            foreach (string s in Samples) Probe(s, sb);
        }
        else Probe(path, sb);
        return sb.ToString();
    }

    private static void Probe(string p, StringBuilder sb)
    {
        GameObject prefab = Resources.Load<GameObject>(p);
        if (prefab == null) { sb.AppendLine("MISS " + p); return; }
        GameObject go = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(prefab));
        try
        {
            Renderer[] rs = go.GetComponentsInChildren<Renderer>(true);
            float maxY = 0f;
            foreach (Renderer r in rs)
            {
                if (r == null) continue;
                float h = r.bounds.size.y;
                if (h > maxY) maxY = h;
            }
            sb.AppendLine("ACTOR " + p + " scale=" + go.transform.localScale + " renderers=" + rs.Length + " boundsY=" + maxY.ToString("F2"));
            Animator an = go.GetComponentInChildren<Animator>(true);
            if (an == null) { sb.AppendLine("  ANIMATOR=NULL"); return; }
            UnityEditor.Animations.AnimatorController ctl = an.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            sb.AppendLine("  animator enabled=" + an.enabled + " culling=" + an.cullingMode +
                          " controller=" + (ctl != null ? ctl.name : "null") +
                          " avatar=" + (an.avatar != null ? an.avatar.name : "null"));
            if (ctl == null || ctl.layers.Length == 0) return;
            AnimatorStateMachine sm = ctl.layers[0].stateMachine;
            AnimatorState def = sm.defaultState;
            AnimationClip defClip = def != null ? def.motion as AnimationClip : null;
            sb.AppendLine("  default=" + (def != null ? def.name : "null") +
                          " clip=" + (defClip != null ? defClip.name : "null") +
                          " len=" + (defClip != null ? defClip.length.ToString("F2") : "0") +
                          " loop=" + (defClip != null && defClip.isLooping));
            foreach (ChildAnimatorState st in sm.states)
            {
                if (st.state.name != "Idle" && st.state.name != "Run" && st.state.name != "Attack" && st.state.name != "Death") continue;
                AnimationClip c = st.state.motion as AnimationClip;
                sb.AppendLine("  STATE " + st.state.name + " clip=" + (c != null ? c.name : "null") +
                              " len=" + (c != null ? c.length.ToString("F2") : "0") +
                              " loop=" + (c != null && c.isLooping));
            }
        }
        finally { PrefabUtility.UnloadPrefabContents(go); }
    }
}

public sealed class ActorProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string p = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(ActorProbe.Run(p));
    }
}

// Fix GameRef actor prefabs: looping clips, default Idle state, visual scale and valid avatars.
public static class ActorFix
{
    public static string Run(float targetHeight)
    {
        var sb = new StringBuilder();

        string[] modelGuids = AssetDatabase.FindAssets("t:Model", new[] { "Assets/ThirdParty/GameRef/Character/Monster" });
        int clipFixed = 0, models = 0, noClips = 0;
        foreach (string g in modelGuids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            if (p.Contains("_w_")) continue;
            ModelImporter imp = AssetImporter.GetAtPath(p) as ModelImporter;
            if (imp == null) continue;
            ModelImporterClipAnimation[] clips = imp.clipAnimations;
            if (clips == null || clips.Length == 0) { noClips++; continue; }
            bool changed = false;
            for (int i = 0; i < clips.Length; i++)
            {
                if (!clips[i].loopTime) { clips[i].loopTime = true; changed = true; clipFixed++; }
            }
            if (changed) { imp.clipAnimations = clips; imp.SaveAndReimport(); models++; }
        }
        sb.AppendLine("CLIPS loopSet=" + clipFixed + " models=" + models + " modelsWithoutClips=" + noClips);

        string[] ctlGuids = AssetDatabase.FindAssets("t:AnimatorController", new[] { "Assets/ThirdParty/GameRef/Animators" });
        int ctlFixed = 0;
        foreach (string g in ctlGuids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            UnityEditor.Animations.AnimatorController ctl = AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(p);
            if (ctl == null || ctl.layers.Length == 0) continue;
            UnityEditor.Animations.AnimatorStateMachine sm = ctl.layers[0].stateMachine;
            UnityEditor.Animations.ChildAnimatorState idle = sm.states.FirstOrDefault(s => s.state.name == "Idle");
            if (idle.state != null) { sm.defaultState = idle.state; EditorUtility.SetDirty(ctl); ctlFixed++; }
        }
        AssetDatabase.SaveAssets();
        sb.AppendLine("CONTROLLERS defaultIdle=" + ctlFixed);

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ThirdParty/GameRef/Resources/Monster" });
        int scaled = 0, avatars = 0, animators = 0, avatarMiss = 0;
        foreach (string g in prefabGuids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            GameObject root = PrefabUtility.LoadPrefabContents(p);
            try
            {
                Renderer[] rs = root.GetComponentsInChildren<Renderer>(true);
                float maxY = 0f;
                foreach (Renderer r in rs) if (r != null) maxY = Mathf.Max(maxY, r.bounds.size.y);
                if (maxY > 0.05f)
                {
                    float f = Mathf.Clamp(targetHeight / maxY, 0.5f, 12f);
                    root.transform.localScale = new Vector3(f, f, f);
                    scaled++;
                }
                Avatar av = null;
                foreach (SkinnedMeshRenderer smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if (smr == null || smr.sharedMesh == null) continue;
                    string meshPath = AssetDatabase.GetAssetPath(smr.sharedMesh);
                    if (string.IsNullOrEmpty(meshPath)) continue;
                    av = AssetDatabase.LoadAllAssetsAtPath(meshPath).OfType<Avatar>().FirstOrDefault();
                    if (av != null) break;
                }
                if (av == null) avatarMiss++;
                foreach (Animator an in root.GetComponentsInChildren<Animator>(true))
                {
                    if (an == null) continue;
                    an.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    an.applyRootMotion = false;
                    if (an.avatar == null && av != null) { an.avatar = av; avatars++; }
                    animators++;
                }
                PrefabUtility.SaveAsPrefabAsset(root, p);
            }
            catch (Exception e) { sb.AppendLine("  PREFAB_FAIL " + p + " :: " + e.Message); }
            finally { PrefabUtility.UnloadPrefabContents(root); }
        }
        sb.AppendLine("PREFABS scaled=" + scaled + " avatarsSet=" + avatars + " avatarMissing=" + avatarMiss + " animators=" + animators);
        return sb.ToString();
    }
}

public sealed class ActorFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        float h = operands.Count > 0 ? operands[0].GetFloat() : 8f;
        return BoxedValue.FromString(ActorFix.Run(h));
    }
}

// Normalize ThirdParty scene props to one coherent scale spec (max dimension target).
public static class PropScale
{
    private static readonly Dictionary<string, float> Targets = new Dictionary<string, float>
    {
        { "Building_a", 9f }, { "Building_f", 9f }, { "House", 9f },
        { "Baker_house", 9f }, { "Mill", 9f }, { "Pyramid", 12f },
        { "Cliff_Desert_01", 10f }, { "Tent", 5f }, { "Stairway", 5f },
        { "Sewers_entrance", 4f }, { "Cart_Covered1", 3f }, { "Well", 2.5f },
        { "PineTree001", 8f }, { "LargeTree001", 8f }, { "DeadTree001", 8f },
        { "Tree4_B_Rot0", 8f }, { "Rock_a", 3f }, { "Rock_f", 3f },
        { "Barrel", 1.5f }, { "Fence_a", 2f }, { "Lantern_post", 3f },
        { "Sign_a", 2.5f }
    };

    public static string Run(bool apply)
    {
        var sb = new StringBuilder();
        string dir = "Assets/Resources/SceneProps";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { dir });
        int done = 0, missing = 0;
        foreach (string g in guids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            string name = Path.GetFileNameWithoutExtension(p);
            float target;
            if (!Targets.TryGetValue(name, out target)) { missing++; sb.AppendLine("  NO_SPEC " + name); continue; }
            GameObject root = PrefabUtility.LoadPrefabContents(p);
            try
            {
                Vector3 oldScale = root.transform.localScale;
                root.transform.localScale = Vector3.one;
                Renderer[] rs = root.GetComponentsInChildren<Renderer>(true);
                Bounds b = default(Bounds);
                bool has = false;
                foreach (Renderer r in rs)
                {
                    if (r == null) continue;
                    if (!has) { b = r.bounds; has = true; }
                    else b.Encapsulate(r.bounds);
                }
                float maxDim = has ? Mathf.Max(b.size.x, Mathf.Max(b.size.y, b.size.z)) : 0f;
                float factor = maxDim > 0.01f ? target / maxDim : 1f;
                sb.AppendLine(string.Format("  {0,-20} beforeMax={1:F2} factor={2:F2} target={3:F1} oldScale={4}", name, maxDim, factor, target, oldScale));
                if (apply && maxDim > 0.01f)
                {
                    root.transform.localScale = new Vector3(factor, factor, factor);
                    PrefabUtility.SaveAsPrefabAsset(root, p);
                    done++;
                }
                else root.transform.localScale = oldScale;
            }
            finally { PrefabUtility.UnloadPrefabContents(root); }
        }
        sb.Insert(0, "PROP_SCALE apply=" + apply + " prefabs=" + guids.Length + " changed=" + done + " noSpec=" + missing + "\n");
        return sb.ToString();
    }
}

public sealed class PropScaleExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        bool apply = operands.Count == 0 || operands[0].GetBool();
        return BoxedValue.FromString(PropScale.Run(apply));
    }
}

// Bind a valid Generic avatar to every actor animator and fix scaled NavMeshAgent steps.
public static class AnimFix
{
    public static string Run()
    {
        var sb = new StringBuilder();
        string avatarDir = "Assets/ThirdParty/GameRef/Animators";
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ThirdParty/GameRef/Resources/Monster" });
        int fromFbx = 0, built = 0, reused = 0, stepFixed = 0, legacyClips = 0, noAnimator = 0, failed = 0;
        foreach (string g in prefabGuids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            string name = Path.GetFileNameWithoutExtension(p);
            GameObject root = PrefabUtility.LoadPrefabContents(p);
            try
            {
                Animator[] animators = root.GetComponentsInChildren<Animator>(true);
                if (animators.Length == 0) { noAnimator++; continue; }

                string meshPath = string.Empty;
                foreach (SkinnedMeshRenderer smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if (smr == null || smr.sharedMesh == null) continue;
                    meshPath = AssetDatabase.GetAssetPath(smr.sharedMesh);
                    if (!string.IsNullOrEmpty(meshPath)) break;
                }
                Avatar av = null;
                if (!string.IsNullOrEmpty(meshPath))
                    av = AssetDatabase.LoadAllAssetsAtPath(meshPath).OfType<Avatar>().FirstOrDefault();
                if (av != null) fromFbx++;
                else
                {
                    string avPath = avatarDir + "/" + name + "_Avatar.asset";
                    av = AssetDatabase.LoadAssetAtPath<Avatar>(avPath);
                    if (av != null) reused++;
                    else
                    {
                        try
                        {
                            Avatar fresh = AvatarBuilder.BuildGenericAvatar(root, string.Empty);
                            if (fresh != null)
                            {
                                AssetDatabase.CreateAsset(fresh, avPath);
                                av = AssetDatabase.LoadAssetAtPath<Avatar>(avPath);
                                built++;
                            }
                        }
                        catch (Exception e) { sb.AppendLine("  AVATAR_FAIL " + name + " :: " + e.Message); failed++; }
                    }
                }
                foreach (Animator an in animators)
                {
                    if (an == null) continue;
                    if (an.avatar == null && av != null) an.avatar = av;
                    RuntimeAnimatorController ctl = an.runtimeAnimatorController;
                    if (ctl != null)
                        foreach (AnimationClip clip in ctl.animationClips)
                            if (clip != null && clip.legacy) legacyClips++;
                }
                foreach (UnityEngine.AI.NavMeshAgent agent in root.GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>(true))
                {
                    if (agent == null) continue;
                    UnityEngine.Object.DestroyImmediate(agent, true);
                    stepFixed++;
                }
                PrefabUtility.SaveAsPrefabAsset(root, p);
            }
            finally { PrefabUtility.UnloadPrefabContents(root); }
        }
        AssetDatabase.SaveAssets();
        sb.Insert(0, "ANIM_FIX prefabs=" + prefabGuids.Length + " fromFbxAvatar=" + fromFbx +
                     " builtAvatar=" + built + " reused=" + reused + " legacyClips=" + legacyClips +
                     " stepFixed=" + stepFixed + " noAnimator=" + noAnimator + " failed=" + failed + "\n");
        return sb.ToString();
    }
}

public sealed class AnimFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(AnimFix.Run());
    }
}

// Fix scaled CharacterController steps and seat scene props on the terrain surface.
public static class AssetPolish
{
    public static string Run()
    {
        var sb = new StringBuilder();
        int ccTotal = 0, ccFixed = 0;
        foreach (string g in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ThirdParty/GameRef/Resources/Monster" }))
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            GameObject root = PrefabUtility.LoadPrefabContents(p);
            try
            {
                bool changed = false;
                foreach (CharacterController cc in root.GetComponentsInChildren<CharacterController>(true))
                {
                    if (cc == null) continue;
                    ccTotal++;
                    float maxStep = cc.height * 0.5f;
                    if (cc.stepOffset > maxStep)
                    {
                        cc.stepOffset = Mathf.Max(0f, cc.height * 0.3f);
                        changed = true;
                        ccFixed++;
                    }
                }
                if (changed) PrefabUtility.SaveAsPrefabAsset(root, p);
            }
            finally { PrefabUtility.UnloadPrefabContents(root); }
        }
        sb.AppendLine("CHARACTER_CONTROLLER total=" + ccTotal + " stepFixed=" + ccFixed);

        string current = SceneManager.GetActiveScene().path;
        int scenes = 0, props = 0, adjusted = 0;
        float maxBefore = 0f, maxAfter = 0f;
        float minScale = float.MaxValue, maxScale = 0f;
        try
        {
            for (int i = 3; i <= 12; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) continue;
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Terrain terrain = Terrain.activeTerrain;
                if (terrain == null) continue;
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    if (go == null || go.GetComponent<Terrain>() != null) continue;
                    if (go.GetComponentInChildren<ParticleSystem>(true) != null) continue;
                    Renderer[] rs = go.GetComponentsInChildren<Renderer>(true);
                    if (rs == null || rs.Length == 0) continue;
                    Bounds b = default(Bounds);
                    bool has = false;
                    foreach (Renderer r in rs)
                    {
                        if (r == null) continue;
                        if (!has) { b = r.bounds; has = true; }
                        else b.Encapsulate(r.bounds);
                    }
                    if (!has) continue;
                    minScale = Mathf.Min(minScale, go.transform.localScale.x);
                    maxScale = Mathf.Max(maxScale, go.transform.localScale.x);
                    float ground = terrain.SampleHeight(new Vector3(b.center.x, 0, b.center.z)) + terrain.transform.position.y;
                    float err = b.min.y - ground;
                    maxBefore = Mathf.Max(maxBefore, Mathf.Abs(err));
                    props++;
                    if (Mathf.Abs(err) > 0.5f)
                    {
                        go.transform.position += new Vector3(0, -err, 0);
                        adjusted++;
                        Bounds b2 = default(Bounds);
                        bool has2 = false;
                        foreach (Renderer r in rs)
                        {
                            if (r == null) continue;
                            if (!has2) { b2 = r.bounds; has2 = true; }
                            else b2.Encapsulate(r.bounds);
                        }
                        if (has2) maxAfter = Mathf.Max(maxAfter, Mathf.Abs(b2.min.y - ground));
                    }
                }
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                scenes++;
            }
        }
        catch (Exception e) { sb.AppendLine("GROUND_FAIL " + e.Message); }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        sb.AppendLine("GROUND scenes=" + scenes + " props=" + props + " adjusted=" + adjusted +
                      " maxOffsetBefore=" + maxBefore.ToString("F2") + " maxOffsetAfter=" + maxAfter.ToString("F2") +
                      " instanceScale=" + (minScale == float.MaxValue ? 0f : minScale).ToString("F2") + ".." + maxScale.ToString("F2"));
        return sb.ToString();
    }
}

public sealed class AssetPolishExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(AssetPolish.Run());
    }
}

// Bake legacy NavMesh data for generated battle scenes and drop duplicate audio listeners.
public static class NavBake
{
    public static string Run()
    {
        var sb = new StringBuilder();
        string current = SceneManager.GetActiveScene().path;
        int baked = 0, listeners = 0;
        try
        {
            for (int i = 3; i <= 12; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) continue;
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    if (go == null || go.GetComponent<Camera>() != null || go.GetComponent<Light>() != null) continue;
                    foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                        GameObjectUtility.SetStaticEditorFlags(t.gameObject, StaticEditorFlags.NavigationStatic);
                }
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    foreach (AudioListener al in go.GetComponentsInChildren<AudioListener>(true))
                    {
                        UnityEngine.Object.DestroyImmediate(al, true);
                        listeners++;
                    }
                }
                UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                baked++;
                sb.AppendLine("BAKED " + path);
            }
        }
        catch (Exception e) { sb.AppendLine("BAKE_FAIL " + e.Message); }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        sb.Insert(0, "NAV_BAKE scenes=" + baked + " listenersRemoved=" + listeners + "\n");
        return sb.ToString();
    }
}

public sealed class NavBakeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(NavBake.Run());
    }
}

// Inspect live Animators in Play mode to diagnose T-pose / frozen animation.
public static class AnimRuntimeProbe
{
    public static string Run()
    {
        var sb = new StringBuilder();
        Animator[] all = UnityEngine.Object.FindObjectsOfType<Animator>(false);
        sb.AppendLine("RUNTIME animators=" + all.Length);
        sb.AppendLine("SCENES count=" + SceneManager.sceneCount);
        for (int si = 0; si < SceneManager.sceneCount; ++si)
        {
            Scene s = SceneManager.GetSceneAt(si);
            int camOn = 0, camOff = 0, terrOn = 0, terrOff = 0, rendOn = 0, rendOff = 0;
            foreach (GameObject go in s.GetRootGameObjects())
            {
                if (go == null) continue;
                foreach (Camera c in go.GetComponentsInChildren<Camera>(true)) { if (c.enabled) camOn++; else camOff++; }
                foreach (Terrain t in go.GetComponentsInChildren<Terrain>(true)) { if (t.enabled) terrOn++; else terrOff++; }
                foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true)) { if (r.enabled) rendOn++; else rendOff++; }
            }
            sb.AppendLine("  " + s.name + " camera(on/off)=" + camOn + "/" + camOff +
                          " terrain=" + terrOn + "/" + terrOff +
                          " renderer=" + rendOn + "/" + rendOff);
        }
        int shown = 0;
        foreach (Animator an in all)
        {
            if (an == null || shown >= 14) continue;
            shown++;
            Avatar av = an.avatar;
            RuntimeAnimatorController ctl = an.runtimeAnimatorController;
            string st = "n/a";
            if (an.isInitialized)
            {
                AnimatorStateInfo si = an.GetCurrentAnimatorStateInfo(0);
                st = "hash=" + si.shortNameHash + " time=" + si.normalizedTime.ToString("F2") +
                     " len=" + si.length.ToString("F2");
            }
            sb.AppendLine("  " + an.gameObject.name +
                          " active=" + an.gameObject.activeInHierarchy +
                          " enabled=" + an.enabled +
                          " speed=" + an.speed.ToString("F2") +
                          " init=" + an.isInitialized +
                          " avatar=" + (av != null ? av.name + "/valid=" + av.isValid : "null") +
                          " ctl=" + (ctl != null ? ctl.name : "null") +
                          " state=" + st +
                          " legacyAnim=" + (an.GetComponent<Animation>() != null));
        }
        return sb.ToString();
    }
}

public sealed class AnimRuntimeProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(AnimRuntimeProbe.Run());
    }
}

// Some GameRef models name clips with a model prefix (YiNa_Run, YiNa_Dead ...).
// The original generic-name matcher missed those, so every controller state ended
// up pointing at the first clip (a frozen pose). Re-map such states to the proper clip.
public static class ControllerFix
{
    private static readonly Dictionary<string, string[]> Candidates = new Dictionary<string, string[]>
    {
        { "Idle",   new[] { "Idle_01", "Idle_02", "Stand", "Walk" } },
        { "Stand",  new[] { "Stand", "Idle_01" } },
        { "Run",    new[] { "Run", "Walk" } },
        { "Walk",   new[] { "Walk", "Run" } },
        { "Attack", new[] { "Attack_01", "Skill_01", "Skill01", "Skill_02", "Hit" } },
        { "Death",  new[] { "Dead", "DeadPose" } },
        { "Hit",    new[] { "Hurt_M", "Hurt_L", "Hurt_R", "Hit", "Hurt" } }
    };

    public static string Run()
    {
        var sb = new StringBuilder();
        string[] ctlGuids = AssetDatabase.FindAssets("t:AnimatorController", new[] { "Assets/ThirdParty/GameRef/Animators" });
        int ctlFixed = 0, stateFixed = 0, skipped = 0;
        foreach (string g in ctlGuids)
        {
            string ctlPath = AssetDatabase.GUIDToAssetPath(g);
            string name = Path.GetFileNameWithoutExtension(ctlPath);
            var ctl = AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(ctlPath);
            if (ctl == null || ctl.layers.Length == 0) { skipped++; continue; }
            string fbx = FindModel(name);
            if (string.IsNullOrEmpty(fbx)) { skipped++; continue; }
            List<AnimationClip> clips = AssetDatabase.LoadAllAssetsAtPath(fbx).OfType<AnimationClip>()
                .Where(c => c != null && !c.name.StartsWith("__preview__")).ToList();
            if (clips.Count == 0) { skipped++; continue; }

            var sm = ctl.layers[0].stateMachine;
            var usage = new Dictionary<AnimationClip, int>();
            foreach (var s in sm.states)
            {
                AnimationClip c = s.state.motion as AnimationClip;
                if (c == null) continue;
                int n; usage.TryGetValue(c, out n); usage[c] = n + 1;
            }
            bool changed = false;
            foreach (var st in sm.states)
            {
                string stateName = st.state.name;
                string[] cand;
                if (!Candidates.TryGetValue(stateName, out cand)) continue;
                AnimationClip cur = st.state.motion as AnimationClip;
                int used = 0;
                if (cur != null) usage.TryGetValue(cur, out used);
                if (cur != null && used <= 1) continue;
                AnimationClip pick = Pick(stateName, cand, clips);
                if (pick == null || pick == cur) continue;
                st.state.motion = pick;
                stateFixed++;
                changed = true;
            }
            if (changed) { EditorUtility.SetDirty(ctl); ctlFixed++; }
        }
        AssetDatabase.SaveAssets();
        sb.AppendLine("CONTROLLER_FIX controllers=" + ctlGuids.Length + " changed=" + ctlFixed +
                      " states=" + stateFixed + " skipped=" + skipped);
        return sb.ToString();
    }

    private static string FindModel(string name)
    {
        foreach (string g in AssetDatabase.FindAssets(name + " t:Model", new[] { "Assets/ThirdParty/GameRef/Character/Monster" }))
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            if (Path.GetFileNameWithoutExtension(p) == name &&
                p.EndsWith(".FBX", StringComparison.OrdinalIgnoreCase)) return p;
        }
        return null;
    }

    private static AnimationClip Pick(string state, string[] cand, List<AnimationClip> clips)
    {
        foreach (string key in cand)
        {
            AnimationClip c = clips.FirstOrDefault(x => x.name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (c != null) return c;
            c = clips.FirstOrDefault(x => x.name.EndsWith("_" + key, StringComparison.OrdinalIgnoreCase));
            if (c != null) return c;
        }
        foreach (string key in cand)
        {
            AnimationClip c = clips.FirstOrDefault(x => x.name.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0);
            if (c != null) return c;
        }
        return null;
    }
}

public sealed class ControllerFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(ControllerFix.Run());
    }
}

// A clip can be sampled while the model stays in bind pose when its curve binding
// paths do not match the transform hierarchy under the Animator. Report both sides.
public static class AnimBindingProbe
{
    public static string Run(string resourcePath)
    {
        var sb = new StringBuilder();
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null) return "MISS " + resourcePath;
        string assetPath = AssetDatabase.GetAssetPath(prefab);
        string baseName = Path.GetFileNameWithoutExtension(assetPath);
        GameObject root = PrefabUtility.LoadPrefabContents(assetPath);
        try
        {
            var paths = new HashSet<string>();
            CollectPaths(root.transform, "", paths);
            sb.AppendLine("PREFAB " + resourcePath + " transforms=" + paths.Count);
            int n = 0;
            foreach (string p in paths) { if (n++ >= 8) break; sb.AppendLine("  T [" + p + "]"); }

            Animator an = root.GetComponentInChildren<Animator>(true);
            sb.AppendLine("ANIMATOR " + (an != null ? an.gameObject.name : "null") +
                          " avatar=" + (an != null && an.avatar != null ? an.avatar.name : "null"));
            RuntimeAnimatorController ctl = an != null ? an.runtimeAnimatorController : null;
            if (ctl == null) { sb.AppendLine("CONTROLLER null"); return sb.ToString(); }
            int shown = 0;
            foreach (AnimationClip c in ctl.animationClips)
            {
                if (c == null || shown++ >= 6) continue;
                EditorCurveBinding[] binds = AnimationUtility.GetCurveBindings(c);
                int matched = binds.Count(b => paths.Contains(b.path));
                string[] sample = binds.Select(b => b.path).Distinct().Take(3).ToArray();
                sb.AppendLine("CLIP " + c.name + " curves=" + binds.Length +
                              " legacy=" + c.legacy + " matched=" + matched +
                              " paths=[" + string.Join(" | ", sample) + "]");
            }
            foreach (string g in AssetDatabase.FindAssets(baseName + " t:Model", new[] { "Assets/ThirdParty/GameRef/Character/Monster" }))
            {
                string p = AssetDatabase.GUIDToAssetPath(g);
                if (Path.GetFileNameWithoutExtension(p) != baseName) continue;
                UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(p);
                sb.AppendLine("FBX " + p + " avatars=" + assets.OfType<Avatar>().Count() +
                              " clips=" + assets.OfType<AnimationClip>().Count());
                foreach (Avatar av in assets.OfType<Avatar>())
                    sb.AppendLine("  FBX_AVATAR " + av.name + " valid=" + av.isValid + " human=" + av.isHuman);
                break;
            }
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
        return sb.ToString();
    }

    private static void CollectPaths(Transform t, string prefix, HashSet<string> set)
    {
        foreach (Transform child in t)
        {
            string path = prefix.Length == 0 ? child.name : prefix + "/" + child.name;
            set.Add(path);
            CollectPaths(child, path, set);
        }
    }
}

public sealed class AnimBindingProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string p = operands.Count > 0 ? operands[0].GetString() : "Monster/Boss/5_IP_YiNa_01";
        return BoxedValue.FromString(AnimBindingProbe.Run(p));
    }
}

// Clip binding paths are resolved from the GameObject that owns the Animator.
// GameRef prefabs put the Animator on a weapon child, so the curves (Bone_Root/...)
// never reach the body skeleton and the model stays in bind pose. Host the
// Animator on the prefab root instead.
public static class AnimatorHostFix
{
    public static string Run()
    {
        var sb = new StringBuilder();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ThirdParty/GameRef/Resources/Monster" });
        int fixedCount = 0, already = 0, skipped = 0;
        foreach (string g in guids)
        {
            string p = AssetDatabase.GUIDToAssetPath(g);
            GameObject root = PrefabUtility.LoadPrefabContents(p);
            try
            {
                Animator[] all = root.GetComponentsInChildren<Animator>(true);
                Animator src = all.FirstOrDefault(a => a != null && a.runtimeAnimatorController != null);
                if (src == null) { skipped++; continue; }
                AnimationClip clip = src.runtimeAnimatorController.animationClips.FirstOrDefault(c => c != null);
                if (clip == null) { skipped++; continue; }
                string sample = string.Empty;
                foreach (EditorCurveBinding b in AnimationUtility.GetCurveBindings(clip))
                {
                    if (!string.IsNullOrEmpty(b.path)) { sample = b.path; break; }
                }
                if (string.IsNullOrEmpty(sample)) { skipped++; continue; }
                if (src.transform.Find(sample) != null) { already++; continue; }

                Animator host = root.GetComponent<Animator>();
                if (host == null) host = root.AddComponent<Animator>();
                if (host.transform.Find(sample) == null)
                {
                    sb.AppendLine("  NO_MATCH " + Path.GetFileNameWithoutExtension(p) + " sample=" + sample);
                    skipped++;
                    continue;
                }
                host.runtimeAnimatorController = src.runtimeAnimatorController;
                host.avatar = src.avatar;
                host.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                host.applyRootMotion = false;
                host.enabled = true;
                foreach (Animator a in all)
                {
                    if (a == null || a == host) continue;
                    a.runtimeAnimatorController = null;
                    a.enabled = false;
                }
                PrefabUtility.SaveAsPrefabAsset(root, p);
                fixedCount++;
                if (fixedCount <= 12)
                    sb.AppendLine("  FIX " + Path.GetFileNameWithoutExtension(p) +
                                  " from=" + PathOf(src.transform, root.transform) + " sample=" + sample);
            }
            finally { PrefabUtility.UnloadPrefabContents(root); }
        }
        AssetDatabase.SaveAssets();
        sb.Insert(0, "ANIMATOR_HOST_FIX prefabs=" + guids.Length + " fixed=" + fixedCount +
                     " alreadyOk=" + already + " skipped=" + skipped + "\n");
        return sb.ToString();
    }

    private static string PathOf(Transform t, Transform root)
    {
        var names = new List<string>();
        while (t != null && t != root) { names.Add(t.name); t = t.parent; }
        names.Reverse();
        return string.Join("/", names.ToArray());
    }
}

public sealed class AnimatorHostFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        return BoxedValue.FromString(AnimatorHostFix.Run());
    }
}

// Deterministic proof that a clip actually drives the prefab skeleton: sample the
// clip at mid-time and compare a curved bone's transform before/after.
public static class AnimSampleProbe
{
    public static string Run(string resourcePath, string clipName)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null) return "MISS " + resourcePath;
        string assetPath = AssetDatabase.GetAssetPath(prefab);
        GameObject root = PrefabUtility.LoadPrefabContents(assetPath);
        try
        {
            Animator an = root.GetComponent<Animator>();
            if (an == null) an = root.GetComponentInChildren<Animator>(true);
            if (an == null || an.runtimeAnimatorController == null) return "NO_CONTROLLER";
            AnimationClip clip = an.runtimeAnimatorController.animationClips
                .FirstOrDefault(c => c != null && (string.IsNullOrEmpty(clipName) || c.name == clipName));
            if (clip == null) return "NO_CLIP " + clipName;
            string bonePath = AnimationUtility.GetCurveBindings(clip)
                .Select(b => b.path)
                .FirstOrDefault(x => !string.IsNullOrEmpty(x) && root.transform.Find(x) != null &&
                                     x.Contains("UpperArm"));
            if (bonePath == null)
                bonePath = AnimationUtility.GetCurveBindings(clip)
                    .Select(b => b.path)
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x) && root.transform.Find(x) != null);
            if (bonePath == null) return "NO_RESOLVABLE_BONE clip=" + clip.name;
            Transform bone = root.transform.Find(bonePath);
            Quaternion q0 = bone.localRotation;
            Vector3 p0 = bone.localPosition;
            AnimationMode.StartAnimationMode();
            try
            {
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(root, clip, clip.length * 0.5f);
                AnimationMode.EndSampling();
            }
            finally { AnimationMode.StopAnimationMode(); }
            return "SAMPLE " + Path.GetFileNameWithoutExtension(assetPath) +
                   " clip=" + clip.name + " bone=[" + bonePath + "]" +
                   " rotDelta=" + Quaternion.Angle(q0, bone.localRotation).ToString("F2") +
                   " posDelta=" + Vector3.Distance(p0, bone.localPosition).ToString("F3");
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
    }
}

public sealed class AnimSampleProbeExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string p = operands.Count > 0 ? operands[0].GetString() : "Monster/Boss/5_IP_YiNa_01";
        string c = operands.Count > 1 ? operands[1].GetString() : string.Empty;
        return BoxedValue.FromString(AnimSampleProbe.Run(p, c));
    }
}

// Structured inventory of the generated battle scenes, for side-by-side review.
public static class SceneCatalog
{
    public static string Run(string which)
    {
        var sb = new StringBuilder();
        var buildPaths = new HashSet<string>(EditorBuildSettings.scenes.Select(s => s.path));
        string current = SceneManager.GetActiveScene().path;
        int from = 3, to = 12;
        int only;
        if (int.TryParse(which, out only)) { from = only; to = only; }
        try
        {
            for (int i = from; i <= to; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) { sb.AppendLine("MISS " + path); continue; }
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Terrain terrain = null;
                Camera cam = null;
                Light light = null;
                GameObject fx = null;
                var props = new List<GameObject>();
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    if (go == null) continue;
                    if (go.GetComponent<Terrain>() != null) { terrain = go.GetComponent<Terrain>(); continue; }
                    if (go.GetComponent<Camera>() != null) { cam = go.GetComponent<Camera>(); continue; }
                    if (go.GetComponent<Light>() != null) { light = go.GetComponent<Light>(); continue; }
                    if (go.GetComponentInChildren<ParticleSystem>(true) != null) { fx = go; continue; }
                    props.Add(go);
                }
                TerrainData td = terrain != null ? terrain.terrainData : null;
                var names = new List<string>();
                float minScale = float.MaxValue, maxScale = 0f, minY = float.MaxValue, maxY = float.MinValue;
                foreach (GameObject go in props)
                {
                    string src = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                    names.Add(string.IsNullOrEmpty(src) ? go.name : Path.GetFileNameWithoutExtension(src));
                    minScale = Mathf.Min(minScale, go.transform.localScale.x);
                    maxScale = Mathf.Max(maxScale, go.transform.localScale.x);
                    minY = Mathf.Min(minY, go.transform.position.y);
                    maxY = Mathf.Max(maxY, go.transform.position.y);
                }
                var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
                sb.AppendLine("SCENE Battle" + i +
                    " | terrain=" + (td != null ? td.name : "none") +
                    " | props=" + props.Count +
                    " | scale=" + (props.Count > 0 ? minScale.ToString("F2") + ".." + maxScale.ToString("F2") : "-") +
                    " | y=" + (props.Count > 0 ? minY.ToString("F1") + ".." + maxY.ToString("F1") : "-") +
                    " | fx=" + (fx != null ? fx.name : "none") +
                    " | navVerts=" + tri.vertices.Length +
                    " | inBuild=" + buildPaths.Contains(path));
                sb.AppendLine("   props: " + string.Join(", ", names.Distinct().ToArray()));
                sb.AppendLine("   cam=" + (cam != null ? cam.transform.position + " euler=" + cam.transform.eulerAngles + " fov=" + cam.fieldOfView : "none") +
                    " | light=" + (light != null ? light.color + " i=" + light.intensity.ToString("F2") + " euler=" + light.transform.eulerAngles : "none") +
                    " | fog=" + RenderSettings.fog + " ambient=" + RenderSettings.ambientLight);
            }
        }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }
}

public sealed class SceneCatalogExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(SceneCatalog.Run(w));
    }
}

// Render each battle scene from its own camera into a PNG for visual review.
public static class SceneShot
{
    public static string Run(string which)
    {
        var sb = new StringBuilder();
        string current = SceneManager.GetActiveScene().path;
        int from = 3, to = 12;
        int only;
        if (int.TryParse(which, out only)) { from = only; to = only; }
        string dir = "e:/tmp/scene_previews";
        Directory.CreateDirectory(dir);
        try
        {
            for (int i = from; i <= to; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) continue;
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Camera cam = null;
                foreach (GameObject go in sc.GetRootGameObjects())
                {
                    Camera c = go.GetComponent<Camera>();
                    if (c != null) { cam = c; break; }
                }
                if (cam == null) { sb.AppendLine("NO_CAM " + path); continue; }
                int w = 640, h = 360;
                var rt = new RenderTexture(w, h, 24);
                var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
                RenderTexture oldTarget = cam.targetTexture;
                RenderTexture prevActive = RenderTexture.active;
                try
                {
                    cam.targetTexture = rt;
                    cam.Render();
                    RenderTexture.active = rt;
                    tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
                    tex.Apply();
                    string outPath = Path.Combine(dir, "Battle" + i + ".png");
                    File.WriteAllBytes(outPath, tex.EncodeToPNG());
                    sb.AppendLine("SHOT " + outPath);
                }
                finally
                {
                    cam.targetTexture = oldTarget;
                    RenderTexture.active = prevActive;
                    UnityEngine.Object.DestroyImmediate(rt);
                    UnityEngine.Object.DestroyImmediate(tex);
                }
            }
        }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }
}

public sealed class SceneShotExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(SceneShot.Run(w));
    }
}

// Reusable battle-scene standard, encoded so future scenes can be built and
// audited without repeating the same investigation.
//   camera above terrain top, framed on field center; solid themed background
//   + fog (no black void); distinct per-scene tint; >=16 perimeter props with
//   the combat centre kept clear; navmesh re-baked after props change.
// Per-scene terrain recipes. The repository bitmaps are the actual relief
// source; a radial mask carves a walkable platform around the combat point
// (60,70) while the surrounding terrain keeps its shape.
public static class TerrainDesign
{
    private struct Recipe
    {
        public string Bitmap; public int Channel; public bool Invert;
        public float Amp; public float Rim; public float Scale; public float Rotate;
        public float InnerR; public float OuterR; public bool Steps;
        public int LayerA; public int LayerB; public string Label;
        public Recipe(string b, int ch, bool inv, float amp, float rim, float sc, float rot,
                      float inner, float outer, bool steps, int la, int lb, string label)
        {
            Bitmap = b; Channel = ch; Invert = inv; Amp = amp; Rim = rim; Scale = sc;
            Rotate = rot; InnerR = inner; OuterR = outer; Steps = steps;
            LayerA = la; LayerB = lb; Label = label;
        }
    }

    private static readonly Recipe[] Recipes =
    {
        new Recipe("world",  3, false, 0.55f, 0.30f, 1.0f, 0.0f, 30f, 80f,  false, 0, 1, "高地平台"),
        new Recipe("circle", 0, false, 0.42f, 0.38f, 1.4f, 0.4f, 32f, 78f,  false, 1, 2, "环形沙丘"),
        new Recipe("spiral", 1, false, 0.58f, 0.22f, 1.0f, 0.0f, 28f, 75f,  false, 0, 2, "螺旋林谷"),
        new Recipe("chess",  2, true,  0.46f, 0.18f, 2.0f, 0.0f, 31f, 77f,  true,  2, 3, "棋盘台地"),
        new Recipe("world",  0, true,  0.68f, 0.42f, 1.6f, 0.9f, 30f, 72f,  false, 1, 3, "裂谷"),
        new Recipe("circle", 3, false, 0.34f, 0.26f, 2.2f, 0.2f, 33f, 85f,  false, 0, 1, "缓丘营地"),
        new Recipe("chess",  3, false, 0.50f, 0.30f, 1.2f, 0.6f, 30f, 78f,  true,  2, 0, "遗迹阶梯"),
        new Recipe("spiral", 0, true,  0.30f, 0.16f, 1.8f, 0.0f, 34f, 90f,  false, 3, 1, "荒原浅纹"),
        new Recipe("chess",  1, false, 0.40f, 0.20f, 2.6f, 0.3f, 31f, 80f,  true,  0, 2, "集市台地"),
        new Recipe("circle", 2, true,  0.36f, 0.52f, 1.0f, 0.0f, 35f, 88f,  false, 3, 2, "环形竞技场")
    };

    private static Recipe Get(int i) { return Recipes[Mathf.Clamp(i - 3, 0, Recipes.Length - 1)]; }
    public static string Label(int i) { return Get(i).Label; }

    // Ten distinct landforms. The bitmaps are colour/pattern maps, not height
    // maps, so they modulate these shapes instead of being the relief itself.
    private static float Shape(int s, float wx, float wz, float d, float ang, float noise)
    {
        switch (s)
        {
            case 0: return Mathf.Exp(-Mathf.Pow((d - 95f) / 38f, 2f));                       // 环形山脊
            case 1: return 0.5f + 0.5f * Mathf.Sin(wx * 0.045f + wz * 0.021f + noise);      // 沙丘带
            case 2: return 0.5f + 0.5f * Mathf.Sin(d * 0.055f - ang * 2.5f);                // 螺旋丘
            case 3: return Mathf.Floor((0.5f + 0.5f * Mathf.Sin(wx * 0.05f)) * 5f) / 5f;    // 棋盘台地
            case 4: return 1f - Mathf.Exp(-Mathf.Pow((wz - 150f) / 55f, 2f));               // 横向裂谷
            case 5: return Mathf.PerlinNoise(wx * 0.011f, wz * 0.011f);                     // 缓丘
            case 6: return Mathf.Clamp01(Mathf.Floor(d / 26f) / 7f);                        // 同心阶梯
            case 7: return 0.25f + 0.75f * Mathf.Pow(0.5f + 0.5f * Mathf.Cos(ang * 4f), 2f);// 花瓣丘
            case 8: return Mathf.Abs(Mathf.Sin(d * 0.032f));                                // 波纹荒原
            default: return d < 100f ? 0.05f : Mathf.Clamp01((d - 100f) / 45f);             // 竞技场看台
        }
    }

    public static string Apply(int i, TerrainData td, Terrain terrain)
    {
        Recipe r = Get(i);
        string root = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
        string file = Path.Combine(root, r.Bitmap + ".png");
        if (!File.Exists(file)) return "NO_BITMAP " + file;
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!ImageConversion.LoadImage(tex, File.ReadAllBytes(file), false))
        {
            UnityEngine.Object.DestroyImmediate(tex);
            return "LOAD_FAIL " + file;
        }
        int bw = tex.width, bh = tex.height;
        Color32[] px = tex.GetPixels32();

        int res = td.heightmapResolution;
        float sx = td.size.x, sz = td.size.z;
        td.size = new Vector3(sx, 70f, sz);
        float[,] h = new float[res, res];
        float cx = sx * 0.5f, cz = sz * 0.5f;
        float ca = Mathf.Cos(r.Rotate), sa = Mathf.Sin(r.Rotate);
        float minH = 1f, maxH = 0f;
        for (int z = 0; z < res; ++z)
        {
            for (int x = 0; x < res; ++x)
            {
                float wx = (float)x / (res - 1) * sx;
                float wz = (float)z / (res - 1) * sz;
                float rx = (wx - cx) * ca - (wz - cz) * sa;
                float rz = (wx - cx) * sa + (wz - cz) * ca;
                float u = rx / sx * r.Scale + 0.5f;
                float v = rz / sz * r.Scale + 0.5f;
                u -= Mathf.Floor(u); v -= Mathf.Floor(v);
                int ix = Mathf.Clamp((int)(u * bw), 0, bw - 1);
                int iy = Mathf.Clamp((int)(v * bh), 0, bh - 1);
                Color32 c = px[iy * bw + ix];
                float b = r.Channel == 0 ? c.r / 255f : r.Channel == 1 ? c.g / 255f :
                          r.Channel == 2 ? c.b / 255f :
                          (c.r * 0.299f + c.g * 0.587f + c.b * 0.114f) / 255f;
                if (r.Invert) b = 1f - b;
                if (r.Steps) b = Mathf.Floor(b * 6f) / 6f;
                float dx = wx - 60f, dz = wz - 70f;
                float d = Mathf.Sqrt(dx * dx + dz * dz);
                float ang = Mathf.Atan2(dz, dx);
                float noise = Mathf.PerlinNoise(wx * 0.018f + i * 7.3f, wz * 0.018f + i * 3.1f);
                float ridge = Shape(i - 3, wx, wz, d, ang, noise);
                float mask = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((d - r.InnerR) / (r.OuterR - r.InnerR)));
                float edge = Mathf.Min(Mathf.Min(wx, sx - wx), Mathf.Min(wz, sz - wz));
                float rim = r.Rim * mask * (1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(edge / 120f)));
                float val = Mathf.Min(0.60f, Mathf.Clamp01(mask * r.Amp * ridge * (0.6f + 0.8f * b) +
                                          rim * (0.6f + 0.4f * noise) + 0.004f * noise));
                h[z, x] = val;
                if (val < minH) minH = val;
                if (val > maxH) maxH = val;
            }
        }
        td.SetHeights(0, 0, h);

        int ares = td.alphamapResolution, layers = td.alphamapLayers;
        if (layers > 0 && ares > 0)
        {
            float[,,] m = new float[ares, ares, layers];
            int la = Mathf.Clamp(r.LayerA, 0, layers - 1);
            int lb = Mathf.Clamp(r.LayerB, 0, layers - 1);
            for (int z = 0; z < ares; ++z)
            {
                for (int x = 0; x < ares; ++x)
                {
                    int hx = Mathf.Clamp(x * res / ares, 0, res - 1);
                    int hz = Mathf.Clamp(z * res / ares, 0, res - 1);
                    float u = (float)x / ares * r.Scale; u -= Mathf.Floor(u);
                    float v = (float)z / ares * r.Scale; v -= Mathf.Floor(v);
                    Color32 bc = px[Mathf.Clamp((int)(v * bh), 0, bh - 1) * bw + Mathf.Clamp((int)(u * bw), 0, bw - 1)];
                    float tone = (bc.r + bc.g + bc.b) / 765f;
                    float t = Mathf.Clamp01(h[hz, hx] * 2.2f + tone * 0.5f);
                    m[z, x, la] = 1f - t;
                    m[z, x, lb] = t;
                }
            }
            td.SetAlphamaps(0, 0, m);
        }
        // Drop the terrain trees inherited from the story levels inside the
        // combat arena, otherwise hundreds of trees hide the fight.
        TreeInstance[] trees = td.treeInstances;
        if (trees != null && trees.Length > 0)
        {
            var keep = new List<TreeInstance>();
            foreach (TreeInstance ti in trees)
            {
                float dx = ti.position.x * sx - 60f, dz = ti.position.z * sz - 70f;
                if (dx * dx + dz * dz < 95f * 95f) continue;
                keep.Add(ti);
            }
            if (keep.Count != trees.Length) td.treeInstances = keep.ToArray();
        }
        UnityEngine.Object.DestroyImmediate(tex);
        EditorUtility.SetDirty(td);
        AssetDatabase.SaveAssets();
        return r.Label + " src=" + r.Bitmap + ".png ch=" + r.Channel +
               " relief=" + (minH * 70f).ToString("F1") + ".." + (maxH * 70f).ToString("F1");
    }
}

public static class SceneStandard
{
    private static readonly Color[] Tints =
    {
        new Color(0.72f, 0.78f, 0.92f), new Color(0.92f, 0.82f, 0.62f),
        new Color(0.68f, 0.82f, 0.68f), new Color(0.86f, 0.74f, 0.72f),
        new Color(0.62f, 0.72f, 0.78f), new Color(0.90f, 0.66f, 0.48f),
        new Color(0.76f, 0.70f, 0.88f), new Color(0.80f, 0.80f, 0.76f),
        new Color(0.88f, 0.76f, 0.58f), new Color(0.66f, 0.60f, 0.74f)
    };

    private static Color Tint(int i) { return Tints[Mathf.Clamp(i - 3, 0, Tints.Length - 1)]; }

    private struct Parts
    {
        public Terrain Terrain;
        public Camera Cam;
        public Light Light;
        public GameObject Fx;
        public List<GameObject> Props;
    }

    private static Parts Collect(Scene sc)
    {
        var p = new Parts { Props = new List<GameObject>() };
        foreach (GameObject go in sc.GetRootGameObjects())
        {
            if (go == null) continue;
            if (go.GetComponent<Terrain>() != null) { p.Terrain = go.GetComponent<Terrain>(); continue; }
            if (go.GetComponent<Camera>() != null) { p.Cam = go.GetComponent<Camera>(); continue; }
            if (go.GetComponent<Light>() != null) { p.Light = go.GetComponent<Light>(); continue; }
            if (go.GetComponentInChildren<ParticleSystem>(true) != null) { p.Fx = go; continue; }
            p.Props.Add(go);
        }
        return p;
    }

    // Terrain assets are 512x512, but combat happens around (60,70) per the
    // battle story scripts. Frame and dress that area, not the terrain centre.
    private static Vector3 FieldCenter(Terrain t)
    {
        float x = 60f, z = 70f;
        float y = t.SampleHeight(new Vector3(x, 0, z)) + t.transform.position.y;
        return new Vector3(x, y, z);
    }

    private static float TerrainTop(Terrain t) { return t.transform.position.y + t.terrainData.size.y; }

    public static string Fix(string which)
    {
        var sb = new StringBuilder();
        string current = SceneManager.GetActiveScene().path;
        int from = 3, to = 12, only;
        if (int.TryParse(which, out only)) { from = only; to = only; }
        try
        {
            for (int i = from; i <= to; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) { sb.AppendLine("MISS " + path); continue; }
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Parts p = Collect(sc);
                if (p.Terrain == null || p.Cam == null) { sb.AppendLine("SKIP Battle" + i); continue; }
                TerrainData td = p.Terrain.terrainData;
                // Clone the shared TerrainData so the story scenes' Level1~4 stay
                // untouched, then generate this battle's bitmap-driven relief.
                string tdDir = "Assets/Scenes/Levels/Battle" + i;
                Directory.CreateDirectory(tdDir);
                string tdPath = tdDir + "/Ground.asset";
                TerrainData flat = AssetDatabase.LoadAssetAtPath<TerrainData>(tdPath);
                if (flat == null)
                {
                    flat = UnityEngine.Object.Instantiate(td);
                    AssetDatabase.CreateAsset(flat, tdPath);
                }
                p.Terrain.terrainData = flat;
                td = flat;
                string design = TerrainDesign.Apply(i, flat, p.Terrain);
                Vector3 center = FieldCenter(p.Terrain);
                float span = Mathf.Max(td.size.x, td.size.z);
                float top = TerrainTop(p.Terrain);
                Color tint = Tint(i);

                // Sit inside the terrain (NE of the arena) so the map corner is
                // never in frame; a shallower angle shows the surrounding relief.
                Vector3 camXZ = new Vector3(center.x + 68f, 0, center.z + 68f);
                float localGround = p.Terrain.SampleHeight(camXZ) + p.Terrain.transform.position.y;
                float camY = Mathf.Max(center.y, localGround) + 44f;
                p.Cam.transform.position = new Vector3(camXZ.x, camY, camXZ.z);
                p.Cam.transform.LookAt(center + Vector3.up * 8f);
                p.Cam.fieldOfView = 52f;
                p.Cam.clearFlags = CameraClearFlags.SolidColor;
                p.Cam.backgroundColor = tint * 0.62f;
                p.Cam.farClipPlane = 900f;

                if (p.Light != null)
                {
                    p.Light.color = Color.Lerp(Color.white, tint, 0.35f);
                    p.Light.intensity = 1.25f;
                    p.Light.transform.rotation = Quaternion.Euler(48f, 20f + (i - 3) * 28f, 0f);
                }
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = tint * 0.72f;
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.ExponentialSquared;
                RenderSettings.fogColor = tint * 0.66f;
                RenderSettings.fogDensity = 0.0012f;

                var sources = new List<string>();
                foreach (GameObject go in p.Props)
                {
                    string s = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                    if (!string.IsNullOrEmpty(s) && !sources.Contains(s)) sources.Add(s);
                    UnityEngine.Object.DestroyImmediate(go);
                }
                int placed = 0;
                var rnd = new System.Random(9100 + i);
                foreach (string src in sources)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(src);
                    if (prefab == null) continue;
                    for (int k = 0; k < 4; k++)
                    {
                        float ang = (float)rnd.NextDouble() * Mathf.PI * 2f;
                        float rad = 20f + (float)rnd.NextDouble() * 36f;
                        float x = Mathf.Clamp(center.x + Mathf.Cos(ang) * rad, 5f, td.size.x - 5f);
                        float z = Mathf.Clamp(center.z + Mathf.Sin(ang) * rad, 5f, td.size.z - 5f);
                        float ground = p.Terrain.SampleHeight(new Vector3(x, 0, z)) + p.Terrain.transform.position.y;
                        GameObject inst = PrefabUtility.InstantiatePrefab(prefab, sc) as GameObject;
                        if (inst == null) continue;
                        inst.transform.position = new Vector3(x, ground, z);
                        inst.transform.rotation = Quaternion.Euler(0, (float)rnd.NextDouble() * 360f, 0);
                        foreach (Transform t in inst.GetComponentsInChildren<Transform>(true))
                            GameObjectUtility.SetStaticEditorFlags(t.gameObject, StaticEditorFlags.NavigationStatic);
                        Renderer[] rs = inst.GetComponentsInChildren<Renderer>(true);
                        Bounds b = default(Bounds);
                        bool has = false;
                        foreach (Renderer r in rs)
                        {
                            if (r == null) continue;
                            if (!has) { b = r.bounds; has = true; } else b.Encapsulate(r.bounds);
                        }
                        if (has) inst.transform.position += new Vector3(0, ground - b.min.y, 0);
                        placed++;
                    }
                }
                if (p.Fx != null) p.Fx.transform.position = new Vector3(center.x, center.y + 2f, center.z);

                UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
                sb.AppendLine("FIX Battle" + i + " [" + design + "]" +
                              " top=" + top.ToString("F1") + " camY=" + camY.ToString("F1") +
                              " props=" + placed + " tint=#" + ColorUtility.ToHtmlStringRGB(tint));
            }
        }
        catch (Exception e) { sb.AppendLine("FIX_FAIL " + e); }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }

    public static string Verify(string which)
    {
        var sb = new StringBuilder();
        string current = SceneManager.GetActiveScene().path;
        int from = 3, to = 12, only;
        if (int.TryParse(which, out only)) { from = only; to = only; }
        try
        {
            for (int i = from; i <= to; i++)
            {
                string path = "Assets/Scenes/Levels/Battle" + i + ".unity";
                if (!File.Exists(path)) continue;
                Scene sc = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Parts p = Collect(sc);
                if (p.Terrain == null || p.Cam == null) { sb.AppendLine("FAIL Battle" + i + " missing terrain/cam"); continue; }
                Vector3 center = FieldCenter(p.Terrain);
                bool camAbove = p.Cam.transform.position.y > center.y + 20f;
                bool noBlack = p.Cam.clearFlags != CameraClearFlags.Skybox;
                int centerProps = 0;
                foreach (GameObject go in p.Props)
                    if (new Vector2(go.transform.position.x - center.x, go.transform.position.z - center.z).magnitude < 18f) centerProps++;
                // A flat heightmap must fail the standard: sample the whole
                // heightmap for relief, and a small ring for platform flatness.
                TerrainData vd = p.Terrain.terrainData;
                float gmin = float.MaxValue, gmax = float.MinValue;
                for (int gz = 0; gz <= 32; ++gz)
                    for (int gx = 0; gx <= 32; ++gx)
                    {
                        float hv = vd.GetInterpolatedHeight((float)gx / 32f, (float)gz / 32f);
                        gmin = Mathf.Min(gmin, hv); gmax = Mathf.Max(gmax, hv);
                    }
                float relief = gmax - gmin;
                float pmin = float.MaxValue, pmax = float.MinValue;
                for (int a = 0; a < 24; ++a)
                {
                    float ang = a * Mathf.PI * 2f / 24f;
                    float px = 60f + Mathf.Cos(ang) * 20f;
                    float pz = 70f + Mathf.Sin(ang) * 20f;
                    float hv = vd.GetInterpolatedHeight(Mathf.Clamp01(px / vd.size.x), Mathf.Clamp01(pz / vd.size.z));
                    pmin = Mathf.Min(pmin, hv); pmax = Mathf.Max(pmax, hv);
                }
                float platformDelta = pmax - pmin;
                var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
                sb.AppendLine("VERIFY Battle" + i +
                    " camAbove=" + (camAbove ? "PASS" : "FAIL") +
                    " noBlackVoid=" + (noBlack ? "PASS" : "FAIL") +
                    " props=" + (p.Props.Count >= 16 ? "PASS" : "FAIL") + "(" + p.Props.Count + ")" +
                    " centerClear=" + (centerProps == 0 ? "PASS" : "FAIL") + "(" + centerProps + ")" +
                    " platformFlat=" + (platformDelta < 3f ? "PASS" : "FAIL") + "(" + platformDelta.ToString("F1") + ")" +
                    " relief=" + (relief > 8f ? "PASS" : "FAIL") + "(" + relief.ToString("F1") + ")" +
                    " fog=" + (RenderSettings.fog ? "PASS" : "FAIL") +
                    " tint=#" + ColorUtility.ToHtmlStringRGB(p.Cam.backgroundColor) +
                    " nav=" + (tri.vertices.Length > 0 ? "PASS" : "FAIL") + "(" + tri.vertices.Length + ")");
            }
        }
        finally { if (!string.IsNullOrEmpty(current)) EditorSceneManager.OpenScene(current, OpenSceneMode.Single); }
        return sb.ToString();
    }
}

public sealed class SceneFixExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(SceneStandard.Fix(w));
    }
}

public sealed class SceneVerifyExp : SimpleExpressionBase
{
    protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
    {
        string w = operands.Count > 0 ? operands[0].GetString() : string.Empty;
        return BoxedValue.FromString(SceneStandard.Verify(w));
    }
}
