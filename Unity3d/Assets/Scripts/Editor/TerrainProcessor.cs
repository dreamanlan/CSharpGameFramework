using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using GameFramework;

public sealed class TerrainEditWindow : EditorWindow
{
    [MenuItem("工具/地形处理")]
    internal static void InitWindow()
    {
        TerrainEditWindow window = (TerrainEditWindow)EditorWindow.GetWindow(typeof(TerrainEditWindow));
        window.Init();
        window.Show();
    }

    private void Init()
    {
        GenerateTerrainsInfo();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("选中地形信息")) {
            GenerateTerrainsInfo();
        }
        if (GUILayout.Button("选择地形处理脚本")) {
            SelectDsl();
        }
        if (GUILayout.Button("处理选中场景地形")) {
            ProcessSelectedSceneNodes();
        }
        EditorGUILayout.EndHorizontal();

        if (m_Samplers.Count > 0) {
            foreach (var pair in m_Samplers) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(pair.Key, GUILayout.Width(60));
                GUILayout.TextField(pair.Value, 1024);
                if (GUILayout.Button("选择", GUILayout.Width(60))) {
                    SelectImage(pair.Key, pair.Value);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (m_EditedSamplers.Count > 0) {
                foreach (var pair in m_EditedSamplers) {
                    m_Samplers[pair.Key] = pair.Value;
                }
                m_EditedSamplers.Clear();
            }
        }

        m_Pos = EditorGUILayout.BeginScrollView(m_Pos, true, true);
        EditorGUILayout.TextArea(m_Text);
        EditorGUILayout.EndScrollView();
    }

    private void GenerateTerrainsInfo()
    {
        StringBuilder sb = new StringBuilder();
        foreach (GameObject obj in Selection.gameObjects) {
            TerrainEditUtility.GenerateInfo(sb, obj);
        }
        m_Text = sb.ToString();
    }

    private void SelectDsl()
    {
        string path = EditorUtility.OpenFilePanel("请选择要执行的dsl文件", string.Empty, "dsl");
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            Dsl.DslFile file = new Dsl.DslFile();
            if (file.Load(path, (string msg) => { Debug.Log(msg); })) {
                m_DslFile = file;

                m_Samplers.Clear();
                foreach(var info in m_DslFile.DslInfos) {
                    var first = info.First;
                    foreach(var comp in first.Statements) {
                        var callData = comp as Dsl.CallData;
                        string id = callData.GetId();
                        if (id == "sampler") {
                            string key = callData.GetParamId(0);
                            string val = callData.GetParamId(1);

                            if (File.Exists(val)) {
                                m_Samplers[key] = val;
                            } else {
                                m_Samplers[key] = string.Empty;
                            }
                        } else if (id == "cache") {
                            string key = callData.GetParamId(0);
                            int w = int.Parse(callData.GetParamId(1));
                            int h = int.Parse(callData.GetParamId(2));
                            m_Caches.Add(key, new Size(w, h));
                        }
                    }
                }
            } else {
                m_DslFile = null;
                m_Samplers.Clear();
            }
        }
    }

    private void SelectImage(string key, string val)
    {
        string path = EditorUtility.OpenFilePanelWithFilters("请选择要执行的dsl文件", val, new[] { "Image files", "png,jpg,jpeg,bmp,gif", "All files", "*" });
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            m_EditedSamplers[key] = path;
        }
    }

    private void ProcessSelectedSceneNodes()
    {
        Dictionary<string, Color32[,]> samplers = new Dictionary<string, Color32[,]>();
        foreach (var pair in m_Samplers) {
            var bitmap = System.Drawing.Image.FromFile(pair.Value) as Bitmap;
            if (null != bitmap) {
                var colors = new Color32[bitmap.Width, bitmap.Height];
                for (int i = 0; i < bitmap.Width; ++i) {
                    for (int j = 0; j < bitmap.Height; ++j) {
                        System.Drawing.Color c = bitmap.GetPixel(i, j);
                        colors[i, j] = new Color32(c.R, c.G, c.B, c.A);
                    }
                }
                samplers.Add(pair.Key, colors);
            }
        }
        foreach (GameObject obj in Selection.gameObjects) {
            TerrainEditUtility.Process(obj, m_DslFile, samplers, m_Caches);
            Debug.Log("handle " + obj.name);
        }
        EditorUtility.DisplayDialog("提示", "处理完成", "ok");
    }

    private Vector2 m_Pos = Vector2.zero;
    private string m_Text = string.Empty;
    private Dsl.DslFile m_DslFile = null;
    private Dictionary<string, string> m_Samplers = new Dictionary<string, string>();
    private Dictionary<string, string> m_EditedSamplers = new Dictionary<string, string>();
    private Dictionary<string, Size> m_Caches = new Dictionary<string, Size>();
}

internal class DetailInfo
{
    internal int X;
    internal int Y;
    internal int Val;
}

internal static class TerrainEditUtility
{
    internal static void GenerateInfo(StringBuilder sb, GameObject root)
    {
        var terrain = root.GetComponent<Terrain>();
        if (null != terrain) {
            WriteTerrainInfo(sb, 0, terrain);
        }
    }
    internal static void Process(GameObject root, Dsl.DslFile file, Dictionary<string, Color32[,]> samplers, Dictionary<string, Size> cacheInfos)
    {
        if (null != file) {
            List<TreeInstance> trees = new List<TreeInstance>();
            Dictionary<string, int[,]> caches = new Dictionary<string, int[,]>();
            foreach (var pair in cacheInfos) {
                caches.Add(pair.Key, new int[pair.Value.Width, pair.Value.Height]);
            }
            var terrain = root.GetComponent<Terrain>();
            var terrainData = terrain.terrainData;
            var datas = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
            var alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
            int alphanum = alphamaps.GetLength(2);
            int[] layers = terrainData.GetSupportedLayers(0, 0, terrainData.detailWidth, terrainData.detailHeight);
            Dictionary<int, int[,]> details = new Dictionary<int, int[,]>();
            foreach (int layer in layers) {
                var ds = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, layer);
                details.Add(layer, ds);
            }
            var calc = new Expression.DslCalculator();
            calc.Init();
            calc.Register("getheight", new Expression.ExpressionFactoryHelper<GetHeightExp>());
            calc.Register("getalphamap", new Expression.ExpressionFactoryHelper<GetAlphamapExp>());
            calc.Register("getalpha", new Expression.ExpressionFactoryHelper<GetAlphaExp>());
            calc.Register("setalpha", new Expression.ExpressionFactoryHelper<SetAlphaExp>());
            calc.Register("getdetail", new Expression.ExpressionFactoryHelper<GetDetailExp>());
            calc.Register("samplered", new Expression.ExpressionFactoryHelper<SampleRedExp>());
            calc.Register("samplegreen", new Expression.ExpressionFactoryHelper<SampleGreenExp>());
            calc.Register("sampleblue", new Expression.ExpressionFactoryHelper<SampleBlueExp>());
            calc.Register("samplealpha", new Expression.ExpressionFactoryHelper<SampleAlphaExp>());
            calc.Register("getcache", new Expression.ExpressionFactoryHelper<GetCacheExp>());
            calc.Register("setcache", new Expression.ExpressionFactoryHelper<SetCacheExp>());
            calc.Register("addtree", new Expression.ExpressionFactoryHelper<AddTreeExp>());
            calc.NamedVariables.Add("samplers", samplers);
            calc.NamedVariables.Add("caches", caches);
            calc.NamedVariables.Add("trees", trees);
            calc.NamedVariables.Add("heightscalex", terrainData.heightmapScale.x);
            calc.NamedVariables.Add("heightscaley", terrainData.heightmapScale.y);
            calc.NamedVariables.Add("heightscalez", terrainData.heightmapScale.z);
            calc.NamedVariables.Add("heights", datas);
            calc.NamedVariables.Add("alphamaps", alphamaps);
            calc.NamedVariables.Add("alphanum", alphanum);
            calc.NamedVariables.Add("details", details);
            calc.NamedVariables.Add("height", 0.0f);
            calc.NamedVariables.Add("alphas", new float[alphanum]);
            calc.NamedVariables.Add("detail", 0);
            bool resetTrees = false;
            bool canContinue = true;
            foreach (var info in file.DslInfos) {
                bool check=false;
                int num = info.GetFunctionNum();
                if (num >= 2) {
                    string firstId = info.First.GetId();
                    if(firstId=="input"){
                        check = true;
                        for (int i = 1; i < info.GetFunctionNum(); ++i) {
                            string id = info.GetFunctionId(i);
                            if (id == "height" || id == "alphamap" || id == "detail") {
                            } else {
                                check = false;
                                break;
                            }
                        }
                    }
                }
                if(!check){
                    canContinue = false;
                    Debug.LogErrorFormat("error script:{0}, {1}", info.GetLine(), info.ToScriptString(false));
                }
            }
            if (canContinue) {
                int ix = 0;
                foreach (var info in file.DslInfos) {
                    for (int i = 1; i < info.GetFunctionNum(); ++i) {
                        calc.Load(ix.ToString(), info.GetFunction(i));
                        ++ix;
                    }
                }
                int ix2 = 0;
                foreach (var info in file.DslInfos) {
                    for (int i = 1; i < info.GetFunctionNum(); ++i) {
                        ProcessWithDsl(info.First, info.GetFunctionId(i), datas, alphamaps, details, calc, ix2.ToString(), ref resetTrees);
                        ++ix2;
                    }
                }
            }
            terrainData.SetHeights(0, 0, datas);
            terrainData.SetAlphamaps(0, 0, alphamaps);
            foreach (var pair in details) {
                terrainData.SetDetailLayer(0, 0, pair.Key, pair.Value);
            }
            if (resetTrees) {
                terrainData.treeInstances = trees.ToArray();
            } else {
                trees.AddRange(terrainData.treeInstances);
                terrainData.treeInstances = trees.ToArray();
            }
        }
    }

    private static void ProcessWithDsl(Dsl.FunctionData funcData, string type, float[,] datas, float[, ,] alphamaps, Dictionary<int, int[,]> details, Expression.DslCalculator calc, string proc, ref bool resetTrees)
    {
        if (null == funcData)
            return;
        if (null != funcData) {
            if (type == "height") {
                foreach (var comp in funcData.Statements) {
                    var callData = comp as Dsl.CallData;
                    if (null != callData) {
                        string id = callData.GetId();
                        if (id == "resettrees") {
                            resetTrees = bool.Parse(callData.GetParamId(0));
                        } else if (id == "rect") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int w = int.Parse(callData.GetParamId(2));
                            int h = int.Parse(callData.GetParamId(3));
                            ProcessHeights(datas, calc, proc, x, y, w, h);
                        } else if (id == "circle") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int r = int.Parse(callData.GetParamId(2));
                            ProcessHeights(datas, calc, proc, x, y, r);
                        }
                    }
                }
            } else if (type == "alphamap") {
                int alphanum = alphamaps.GetLength(2);
                foreach (var comp in funcData.Statements) {
                    var callData = comp as Dsl.CallData;
                    if (null != callData) {
                        string id = callData.GetId();
                        if (id == "resettrees") {
                            resetTrees = bool.Parse(callData.GetParamId(0));
                        } else if (id == "rect") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int w = int.Parse(callData.GetParamId(2));
                            int h = int.Parse(callData.GetParamId(3));
                            ProcessAlphamaps(alphamaps, calc, proc, x, y, w, h);
                        } else if (id == "circle") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int r = int.Parse(callData.GetParamId(2));
                            ProcessAlphamaps(alphamaps, calc, proc, x, y, r);
                        }
                    }
                }
            } else if (type == "detail") {
                foreach (var comp in funcData.Statements) {
                    var callData = comp as Dsl.CallData;
                    if (null != callData) {
                        string id = callData.GetId();
                        if (id == "resettrees") {
                            resetTrees = bool.Parse(callData.GetParamId(0));
                        } else if (id == "rect") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int w = int.Parse(callData.GetParamId(2));
                            int h = int.Parse(callData.GetParamId(3));
                            ProcessDetails(details, calc, proc, x, y, w, h);
                        } else if (id == "circle") {
                            int x = int.Parse(callData.GetParamId(0));
                            int y = int.Parse(callData.GetParamId(1));
                            int r = int.Parse(callData.GetParamId(2));
                            ProcessDetails(details, calc, proc, x, y, r);
                        }
                    }
                }
            }
        }
    }

    private static void ProcessHeights(float[,] datas, Expression.DslCalculator calc, string proc, int x, int y, int w, int h)
    {
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                calc.NamedVariables["height"] = datas[yi, xi];
                calc.Calc(proc, xi, yi);
                datas[yi, xi] = (float)Convert.ChangeType(calc.NamedVariables["height"], typeof(float));
            }
        }
    }
    private static void ProcessHeights(float[,] datas, Expression.DslCalculator calc, string proc, int cx, int cy, int r)
    {
        int x = cx - r;
        int y = cy - r;
        int w = r * 2;
        int h = r * 2;
        int r2 = r * r;
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                int dx = xi - cx;
                int dy = yi - cy;
                if (dx * dx + dy * dy <= r2) {
                    calc.NamedVariables["height"] = datas[yi, xi];
                    calc.Calc(proc, xi, yi);
                    datas[yi, xi] = (float)Convert.ChangeType(calc.NamedVariables["height"], typeof(float));
                }
            }
        }
    }
    private static void ProcessAlphamaps(float[,,] alphamaps, Expression.DslCalculator calc, string proc, int x, int y, int w, int h)
    {
        int alphanum = alphamaps.GetLength(2);
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                float[] alphas = calc.NamedVariables["alphas"] as float[];
                for (int i = 0; i < alphanum; ++i) {
                    alphas[i] = alphamaps[xi, yi, i];
                }
                var v = calc.Calc(proc, xi, yi);
                for (int i = 0; i < alphanum; ++i) {
                    alphamaps[xi, yi, i] = alphas[i];
                }
            }
        }
    }
    private static void ProcessAlphamaps(float[,,] alphamaps, Expression.DslCalculator calc, string proc, int cx, int cy, int r)
    {
        int alphanum = alphamaps.GetLength(2);
        int x = cx - r;
        int y = cy - r;
        int w = r * 2;
        int h = r * 2;
        int r2 = r * r;
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                int dx = xi - cx;
                int dy = yi - cy;
                if (dx * dx + dy * dy <= r2) {
                    float[] alphas = calc.NamedVariables["alphas"] as float[];
                    for (int i = 0; i < alphanum; ++i) {
                        alphas[i] = alphamaps[xi, yi, i];
                    }
                    var v = calc.Calc(proc, xi, yi);
                    for (int i = 0; i < alphanum; ++i) {
                        alphamaps[xi, yi, i] = alphas[i];
                    }
                }
            }
        }
    }
    private static void ProcessDetails(Dictionary<int, int[,]> details, Expression.DslCalculator calc, string proc, int x, int y, int w, int h)
    {
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                foreach (var pair in details) {
                    int layer = pair.Key;
                    var detail = pair.Value[xi, yi];
                    calc.NamedVariables["detail"] = detail;
                    calc.Calc(proc, xi, yi, layer);
                    pair.Value[xi, yi] = (int)Convert.ChangeType(calc.NamedVariables["detail"], typeof(int));
                }
            }
        }
    }
    private static void ProcessDetails(Dictionary<int, int[,]> details, Expression.DslCalculator calc, string proc, int cx, int cy, int r)
    {
        int x = cx - r;
        int y = cy - r;
        int w = r * 2;
        int h = r * 2;
        int r2 = r * r;
        for (int ix = 0; ix < w; ++ix) {
            for (int iy = 0; iy < h; ++iy) {
                int xi = x + ix;
                int yi = y + iy;
                int dx = xi - cx;
                int dy = yi - cy;
                if (dx * dx + dy * dy <= r2) {
                    foreach (var pair in details) {
                        int layer = pair.Key;
                        var detail = pair.Value[xi, yi];
                        calc.NamedVariables["detail"] = detail;
                        calc.Calc(proc, xi, yi, layer);
                        pair.Value[xi, yi] = (int)Convert.ChangeType(calc.NamedVariables["detail"], typeof(int));
                    }
                }
            }
        }
    }

    private static void WriteTerrainInfo(StringBuilder sb, int indent, Terrain terrain)
    {
        var data = terrain.terrainData;
        AppendLine(sb, "{0}terrain(\"{1}/{2}\")", GetIndent(indent), terrain.name, data.name);
        AppendLine(sb, "{0}{{", GetIndent(indent));
        ++indent;

        AppendLine(sb, "{0}size{1};", GetIndent(indent), data.size);
        AppendLine(sb, "{0}thickness({1});", GetIndent(indent), data.thickness);
        AppendLine(sb, "{0}basemapresolution({1});", GetIndent(indent), data.baseMapResolution);

        AppendLine(sb, "{0}heightmap(size({1}, {2}), resolution({3}), scale{4});", GetIndent(indent), data.heightmapWidth, data.heightmapHeight, data.heightmapResolution, data.heightmapScale);

        AppendLine(sb, "{0}alphamap(size({1}, {2}), resolution({3}), layers({4}))", GetIndent(indent), data.alphamapWidth, data.alphamapHeight, data.alphamapResolution, data.alphamapLayers);
        AppendLine(sb, "{0}{{", GetIndent(indent));
        ++indent;
        foreach(var sp in data.splatPrototypes) {
            AppendLine(sb, "{0}splat(texture(\"{1}\", {2}, {3}), normalmap{4}, tilesize({5}), tileoffset({6}), specular({7}), metallic({8}), smoothness({9}));", GetIndent(indent), sp.texture.name, sp.texture.width, sp.texture.height, null != sp.normalMap ? string.Format("(\"{1}\", {2}, {3})", sp.normalMap.name, sp.normalMap.width, sp.normalMap.height) : "()", sp.tileSize, sp.tileOffset, sp.specular, sp.metallic, sp.smoothness);
        }
        --indent;
        AppendLine(sb, "{0}}};", GetIndent(indent));

        AppendLine(sb, "{0}detail(size({1}, {2}), resolution({3}))", GetIndent(indent), data.detailWidth, data.detailHeight, data.detailResolution);
        AppendLine(sb, "{0}{{", GetIndent(indent));
        ++indent;
        foreach(var dp in data.detailPrototypes) {
            AppendLine(sb, "{0}prototype(texture(\"{1}\", {2}, {3}), rendermode({4}), usemesh({5}), minsize({6}, {7}), maxsize({8}, {9}), noisespread({10}), bendfactor({11}), healthcolor({12}), drycolor({13}));", GetIndent(indent), dp.prototypeTexture.name, dp.prototypeTexture.width, dp.prototypeTexture.height, dp.renderMode, dp.usePrototypeMesh, dp.minWidth, dp.minHeight, dp.maxWidth, dp.maxHeight, dp.noiseSpread, dp.bendFactor, dp.healthyColor, dp.dryColor);
        }
        foreach (var layer in data.GetSupportedLayers(0, 0, data.detailWidth, data.detailHeight)) {
            AppendLine(sb, "{0}layer({1})", GetIndent(indent), layer);
            AppendLine(sb, "{0}{{", GetIndent(indent));
            ++indent;
            var details = data.GetDetailLayer(0, 0, data.detailWidth, data.detailHeight, layer);
            var infos = new List<DetailInfo>();
            for(int x = 0; x < data.detailWidth; ++x) {
                for(int y = 0; y < data.detailHeight; ++y) {
                    int v = details[x, y];
                    if (v != 0) {
                        infos.Add(new DetailInfo { X = x, Y = y, Val = v });
                    }
                }
            }
            for (int i = 0; i < 10; ++i) {
                int ix = UnityEngine.Random.Range(0, infos.Count);
                var v = infos[ix];
                AppendLine(sb, "{0}detail(x({1}), y({2}), val({3}));", GetIndent(indent), v.X, v.Y, v.Val);
            }
            --indent;
            AppendLine(sb, "{0}}};", GetIndent(indent));
        }
        --indent;
        AppendLine(sb, "{0}}};", GetIndent(indent));

        AppendLine(sb, "{0}tree({1})", GetIndent(indent), data.treeInstanceCount);
        AppendLine(sb, "{0}{{", GetIndent(indent));
        ++indent;
        foreach (var tp in data.treePrototypes) {
            AppendLine(sb, "{0}prototype(prefab(\"{1}\"), bendfactor({2}));", GetIndent(indent), tp.prefab, tp.bendFactor);
        }
        int totalNum = data.treeInstanceCount;
        for (int i = 0; i < 10 && i < totalNum; ++i) {
            int ix = UnityEngine.Random.Range(0, totalNum);
            var inst = data.GetTreeInstance(ix);
            AppendLine(sb, "{0}{1}:treeinstance(proto({2}), position({3}), rotation({4}), scale({5}, {6}), color({7}), lightmapcolor({8}));", GetIndent(indent), ix, inst.prototypeIndex, inst.position, inst.rotation, inst.widthScale, inst.heightScale, inst.color, inst.lightmapColor);
        }
        --indent;
        AppendLine(sb, "{0}}};", GetIndent(indent));

        AppendLine(sb, "{0}wavinggrass", GetIndent(indent));
        AppendLine(sb, "{0}{{", GetIndent(indent));
        ++indent;
        AppendLine(sb, "{0}amount({1});", GetIndent(indent), data.wavingGrassAmount);
        AppendLine(sb, "{0}speed({1});", GetIndent(indent), data.wavingGrassSpeed);
        AppendLine(sb, "{0}strength({1});", GetIndent(indent), data.wavingGrassStrength);
        AppendLine(sb, "{0}tint({1});", GetIndent(indent), data.wavingGrassTint);
        --indent;
        AppendLine(sb, "{0}}};", GetIndent(indent));

        --indent;
        AppendLine(sb, "{0}}};", GetIndent(indent));
    }
    private static void AppendLine(StringBuilder sb, string format, params object[] args)
    {
        sb.AppendFormat(format, args);
        sb.AppendLine();
    }

    private static GameObject FindRoot(GameObject obj)
    {
        GameObject ret = null;
        var trans = obj.transform;
        while (null != trans && !(trans is RectTransform)) {
            ret = trans.gameObject;
            trans = trans.parent;
        }
        return ret;
    }
    private static UnityEngine.Object LoadAssetByPathAndName(string path, string name)
    {
        var objs = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (var obj in objs) {
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    private static string GetIndent(int indent)
    {
        return c_IndentString.Substring(0, indent);
    }
    private static string IndentScript(string indent, string scp)
    {
        string[] lines = scp.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int ix = 0; ix < lines.Length; ++ix) {
            lines[ix] = string.Format("{0}{1}", indent, lines[ix]);
        }
        return string.Join("\r\n", lines);
    }

    private const string c_IndentString = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
}

internal class GetHeightExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 2) {
            var datas = Calculator.NamedVariables["heights"] as float[,];
            var x = ToLong(operands[0]);
            var y = ToLong(operands[1]);
            r = datas[y, x];
        }
        return r;
    }
}
internal class GetAlphamapExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["alphamaps"] as float[, ,];
            var x = ToLong(operands[0]);
            var y = ToLong(operands[1]);
            var ix = ToLong(operands[2]);
            r = datas[x, y, ix];
        }
        return r;
    }
}
internal class GetAlphaExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 1) {
            var datas = Calculator.NamedVariables["alphas"] as float[];
            var ix = ToLong(operands[0]);
            r = datas[ix];
        }
        return r;
    }
}
internal class SetAlphaExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 2) {
            var datas = Calculator.NamedVariables["alphas"] as float[];
            var ix = ToLong(operands[0]);
            var v = ToDouble(operands[1]);
            datas[ix] = (float)v;
            r = v;
        }
        return r;
    }
}
internal class GetDetailExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = null;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["details"] as Dictionary<int, int[,]>;
            var x = ToLong(operands[0]);
            var y = ToLong(operands[1]);
            var ix = ToLong(operands[2]);
            int[,] v;
            if (datas.TryGetValue((int)ix, out v)) {
                r = v[x, y];
            }
        }
        return r;
    }
}
internal class SampleRedExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["samplers"] as Dictionary<string, Color32[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            Color32[,] colors;
            if (datas.TryGetValue(key, out colors)) {
                if (x >= 0 && x < colors.GetLength(0)) {
                    if (y >= 0 && y < colors.GetLength(1)) {
                        r = (int)colors[x, y].r;
                    }
                }
            }
        }
        return r;
    }
}
internal class SampleGreenExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["samplers"] as Dictionary<string, Color32[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            Color32[,] colors;
            if (datas.TryGetValue(key, out colors)) {
                if (x >= 0 && x < colors.GetLength(0)) {
                    if (y >= 0 && y < colors.GetLength(1)) {
                        r = (int)colors[x, y].g;
                    }
                }
            }
        }
        return r;
    }
}
internal class SampleBlueExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["samplers"] as Dictionary<string, Color32[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            Color32[,] colors;
            if (datas.TryGetValue(key, out colors)) {
                if (x >= 0 && x < colors.GetLength(0)) {
                    if (y >= 0 && y < colors.GetLength(1)) {
                        r = (int)colors[x, y].b;
                    }
                }
            }
        }
        return r;
    }
}
internal class SampleAlphaExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["samplers"] as Dictionary<string, Color32[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            Color32[,] colors;
            if (datas.TryGetValue(key, out colors)) {
                if (x >= 0 && x < colors.GetLength(0)) {
                    if (y >= 0 && y < colors.GetLength(1)) {
                        r = (int)colors[x, y].a;
                    }
                }
            }
        }
        return r;
    }
}
internal class GetCacheExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 3) {
            var datas = Calculator.NamedVariables["caches"] as Dictionary<string, int[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            int[,] vals;
            if (datas.TryGetValue(key, out vals)) {
                if (x >= 0 && x < vals.GetLength(0)) {
                    if (y >= 0 && y < vals.GetLength(1)) {
                        r = vals[x, y];
                    }
                }
            }
        }
        return r;
    }
}
internal class SetCacheExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 4) {
            var datas = Calculator.NamedVariables["caches"] as Dictionary<string, int[,]>;
            var key = operands[0] as string;
            var x = ToLong(operands[1]);
            var y = ToLong(operands[2]);
            var v = ToLong(operands[3]);
            int[,] vals;
            if (datas.TryGetValue(key, out vals)) {
                if (x >= 0 && x < vals.GetLength(0)) {
                    if (y >= 0 && y < vals.GetLength(1)) {
                        vals[x, y] = (int)v;
                    }
                }
            }
            r = v;
        }
        return r;
    }
}
internal class AddTreeExp : Expression.SimpleExpressionBase
{
    protected override object OnCalc(IList<object> operands, object[] args)
    {
        object r = 0;
        if (operands.Count >= 9) {
            var trees = Calculator.NamedVariables["trees"] as List<TreeInstance>;
            var ix = ToLong(operands[0]);
            var x = ToDouble(operands[1]);
            var y = ToDouble(operands[2]);
            var z = ToDouble(operands[3]);
            var rot = ToDouble(operands[4]);
            var w_scale = ToDouble(operands[5]);
            var h_scale = ToDouble(operands[6]);
            var color = (uint)ToLong(operands[7]);
            var lightmap = (uint)ToLong(operands[8]);
            if (null != trees) {
                Color32 c = new Color32((byte)((color & 0xff000000) >> 24), (byte)((color & 0xff0000) >> 16), (byte)((color & 0xff00) >> 8), (byte)(color & 0xff));
                Color32 l = new Color32((byte)((lightmap & 0xff000000) >> 24), (byte)((lightmap & 0xff0000) >> 16), (byte)((lightmap & 0xff00) >> 8), (byte)(lightmap & 0xff));
                trees.Add(new TreeInstance { prototypeIndex = (int)ix, position = new Vector3((float)x, (float)y, (float)z), widthScale = (float)w_scale, heightScale = (float)h_scale, rotation = (float)rot, color = c, lightmapColor = l });
            }
            r = ix;
        }
        return r;
    }
}

namespace Expression
{
    public interface IExpression
    {
        object Calc(object[] args);
        bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator);
    }
    public interface IExpressionFactory
    {
        IExpression Create();
    }
    public sealed class ExpressionFactoryHelper<T> : IExpressionFactory where T : IExpression, new()
    {
        public IExpression Create()
        {
            return new T();
        }
    }
    public abstract class AbstractExpression : IExpression
    {
        public abstract object Calc(object[] args);
        public bool Load(Dsl.ISyntaxComponent dsl, DslCalculator calculator)
        {
            m_Calculator = calculator;
            Dsl.ValueData valueData = dsl as Dsl.ValueData;
            if (null != valueData) {
                return Load(valueData);
            } else {
                Dsl.CallData callData = dsl as Dsl.CallData;
                if (null != callData) {
                    bool ret = Load(callData);
                    if (!ret) {
                        int num = callData.GetParamNum();
                        List<IExpression> args = new List<IExpression>();
                        for (int ix = 0; ix < num; ++ix) {
                            Dsl.ISyntaxComponent param = callData.GetParam(ix);
                            args.Add(calculator.Load(param));
                        }
                        return Load(args);
                    }
                    return ret;
                } else {
                    Dsl.FunctionData funcData = dsl as Dsl.FunctionData;
                    if (null != funcData) {
                        return Load(funcData);
                    } else {
                        Dsl.StatementData statementData = dsl as Dsl.StatementData;
                        if (null != statementData) {
                            return Load(statementData);
                        }
                    }
                }
            }
            return false;
        }
        protected virtual bool Load(Dsl.ValueData valData) { return false; }
        protected virtual bool Load(Dsl.CallData callData) { return false; }
        protected virtual bool Load(IList<IExpression> exps) { return false; }
        protected virtual bool Load(Dsl.FunctionData funcData) { return false; }
        protected virtual bool Load(Dsl.StatementData statementData) { return false; }

        protected DslCalculator Calculator
        {
            get { return m_Calculator; }
        }

        private DslCalculator m_Calculator = null;

        protected static double ToDouble(object v)
        {
            return (double)Convert.ChangeType(v, typeof(double));
        }
        protected static long ToLong(object v)
        {
            return (long)Convert.ChangeType(v, typeof(long));
        }
        protected static string ToString(object v)
        {
            return v.ToString();
        }
        protected static T CastTo<T>(object v)
        {
            return (T)Convert.ChangeType(v, typeof(T));
        }
    }
    public abstract class SimpleExpressionBase : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            List<object> operands = new List<object>();
            for (int i = 0; i < m_Exps.Count; ++i) {
                object v = m_Exps[i].Calc(args);
                operands.Add(v);
            }
            return OnCalc(operands, args);
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Exps = exps;
            return true;
        }
        protected abstract object OnCalc(IList<object> operands, object[] args);

        private IList<IExpression> m_Exps = null;
    }
    internal sealed class VarSet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Op.Calc(args);
            m_Variables[m_VarId] = v;
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.CallData param1 = callData.GetParam(0) as Dsl.CallData;
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(param1.GetParamId(0));
            m_Op = Calculator.Load(param2);
            return true;
        }

        private Dictionary<int, object> m_Variables;
        private int m_VarId;
        private IExpression m_Op;
    }
    internal sealed class VarGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = 0;
            m_Variables.TryGetValue(m_VarId, out ret);
            return ret;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_Variables = Calculator.Variables;
            m_VarId = int.Parse(callData.GetParamId(0));
            return true;
        }

        private Dictionary<int, object> m_Variables;
        private int m_VarId;
    }
    internal sealed class NamedVarSet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Op.Calc(args);
            m_Variables[m_VarId] = v;
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
            m_Variables = Calculator.NamedVariables;
            m_VarId = param1.GetId();
            m_Op = Calculator.Load(param2);
            return true;
        }

        private Dictionary<string, object> m_Variables;
        private string m_VarId;
        private IExpression m_Op;
    }
    internal sealed class NamedVarGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object ret = 0;
            m_Variables.TryGetValue(m_VarId, out ret);
            return ret;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            m_Variables = Calculator.NamedVariables;
            m_VarId = valData.GetId();
            return true;
        }

        private Dictionary<string, object> m_Variables;
        private string m_VarId;
    }
    internal sealed class ArgGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            if (m_Index >= 0 && m_Index < args.Length) {
                v = args[m_Index];
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            m_Index = int.Parse(callData.GetParamId(0));
            return true;
        }

        private int m_Index;
    }
    internal sealed class ConstGet : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = m_Val;
            return v;
        }
        protected override bool Load(Dsl.ValueData valData)
        {
            string id = valData.GetId();
            if (valData.GetIdType() == Dsl.ValueData.NUM_TOKEN) {
                if (id.StartsWith("0x"))
                    m_Val = (double)long.Parse(id.Substring(2), System.Globalization.NumberStyles.HexNumber);
                else
                    m_Val = double.Parse(id);
            } else {
                m_Val = id;
            }
            return true;
        }

        private object m_Val;
    }
    internal sealed class AddExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v;
            if (v1 is string || v2 is string) {
                v = ToString(v1) + ToString(v2);
            } else {
                v = ToDouble(v1) + ToDouble(v2);
            }
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SubExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) - ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MulExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) * ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class DivExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) / ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class ModExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToDouble(v1) % ToDouble(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitAndExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) & ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitOrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) | ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitXorExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) ^ ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class BitNotExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v = ~ToLong(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LShiftExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) << (int)ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class RShiftExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToLong(v1) >> (int)ToLong(v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MaxExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 >= v2 ? v1 : v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class MinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 <= v2 ? v1 : v2;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class AbsExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = v1 >= 0 ? v1 : -v1;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class SinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Sin((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CosExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Cos((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Tan((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AsinExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Asin((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AcosExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Acos((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class AtanExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Atan((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class Atan2Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)Mathf.Atan2((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SinhExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Sinh(v1);
            return v;            
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CoshExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Cosh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class TanhExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = Math.Tanh(v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class RndIntExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = ToLong(m_Op2.Calc(args));
            object v = (long)UnityEngine.Random.Range((int)v1, (int)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class RndFloatExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)UnityEngine.Random.Range((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class PowExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = (double)Mathf.Pow((float)v1, (float)v2);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class SqrtExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Sqrt((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LogExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Log((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class Log10Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Log10((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class FloorExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Floor((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class CeilExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            object v = (double)Mathf.Ceil((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            return true;
        }

        private IExpression m_Op1;
    }
    internal sealed class LerpExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.Lerp((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpUnclampedExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.LerpUnclamped((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class LerpAngleExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v = (double)Mathf.LerpAngle((float)v1, (float)v2, (float)v3);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class Clamp01Exp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op.Calc(args));
            object v = (double)Mathf.Clamp01((float)v1);
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class ClampExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            double v3 = ToDouble(m_Op3.Calc(args));
            object v;
            if (v3 < v1)
                v = v1;
            else if (v3 > v2)
                v = v2;
            else
                v = v3;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
    }
    internal sealed class DistExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            float x1 = (float)ToDouble(m_Op1.Calc(args));
            float y1 = (float)ToDouble(m_Op2.Calc(args));
            float x2 = (float)ToDouble(m_Op3.Calc(args));
            float y2 = (float)ToDouble(m_Op4.Calc(args));
            object v = Geometry.Distance(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class DistSqrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            float x1 = (float)ToDouble(m_Op1.Calc(args));
            float y1 = (float)ToDouble(m_Op2.Calc(args));
            float x2 = (float)ToDouble(m_Op3.Calc(args));
            float y2 = (float)ToDouble(m_Op4.Calc(args));
            object v = Geometry.DistanceSquare(new ScriptRuntime.Vector2(x1, y1), new ScriptRuntime.Vector2(x2, y2));
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            m_Op3 = exps[2];
            m_Op4 = exps[3];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
        private IExpression m_Op3;
        private IExpression m_Op4;
    }
    internal sealed class GreatExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 > v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class GreatEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 >= v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class LessExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 < v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class LessEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            double v1 = ToDouble(m_Op1.Calc(args));
            double v2 = ToDouble(m_Op2.Calc(args));
            object v = v1 <= v2 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class EqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToString(v1) == ToString(v2) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class NotEqualExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = m_Op2.Calc(args);
            object v = ToString(v1) != ToString(v2) ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class AndExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = 0;
            object v = v1 != 0 && (v2 = ToLong(m_Op2.Calc(args))) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class OrExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long v1 = ToLong(m_Op1.Calc(args));
            long v2 = 0;
            object v = v1 != 0 || (v2 = ToLong(m_Op2.Calc(args))) != 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op1 = exps[0];
            m_Op2 = exps[1];
            return true;
        }

        private IExpression m_Op1;
        private IExpression m_Op2;
    }
    internal sealed class NotExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            long val = ToLong(m_Op.Calc(args));
            object v = val == 0 ? 1 : 0;
            return v;
        }
        protected override bool Load(IList<IExpression> exps)
        {
            m_Op = exps[0];
            return true;
        }

        private IExpression m_Op;
    }
    internal sealed class CondExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v1 = m_Op1.Calc(args);
            object v2 = null;
            object v3 = null;
            object v = ToDouble(v1) != 0 ? v2 = m_Op2.Calc(args) : v3 = m_Op3.Calc(args);
            return v;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            Dsl.FunctionData funcData1 = statementData.First;
            Dsl.FunctionData funcData2 = statementData.Second;
            if (funcData2.GetId() == ":") {
                Dsl.ISyntaxComponent cond = funcData1.Call.GetParam(0);
                Dsl.ISyntaxComponent op1 = funcData1.GetStatement(0);
                Dsl.ISyntaxComponent op2 = funcData2.GetStatement(0);
                m_Op1 = Calculator.Load(cond);
                m_Op2 = Calculator.Load(op1);
                m_Op3 = Calculator.Load(op2);
            } else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", statementData.ToScriptString(false), statementData.GetLine());
            }
            return true;
        }

        private IExpression m_Op1 = null;
        private IExpression m_Op2 = null;
        private IExpression m_Op3 = null;
    }
    internal sealed class IfExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (int ix = 0; ix < m_Clauses.Count; ++ix) {
                var clause = m_Clauses[ix];
                if (null != clause.Condition) {
                    object condVal = clause.Condition.Calc(args);
                    if (ToDouble(condVal) != 0) {
                        for (int index = 0; index < clause.Expressions.Count; ++index) {
                            v = clause.Expressions[index].Calc(args);
                        }
                        break;
                    }
                } else if (ix == m_Clauses.Count - 1) {
                    for (int index = 0; index < clause.Expressions.Count; ++index) {
                        v = clause.Expressions[index].Calc(args);
                    }
                    break;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            IfExp.Clause item = new IfExp.Clause();
            item.Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                item.Expressions.Add(subExp);
            }
            m_Clauses.Add(item);
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            foreach (var fData in statementData.Functions) {
                if (fData.GetId() == "if" || fData.GetId() == "elseif") {
                    IfExp.Clause item = new IfExp.Clause();
                    if (fData.Call.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent cond = fData.Call.GetParam(0);
                        item.Condition = Calculator.Load(cond);
                    } else {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    }
                    for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                        IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                        item.Expressions.Add(subExp);
                    }
                    m_Clauses.Add(item);
                } else if (fData.GetId() == "else") {
                    if (fData != statementData.Last) {
                        //error
                        Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                    } else {
                        IfExp.Clause item = new IfExp.Clause();
                        for (int ix = 0; ix < fData.GetStatementNum(); ++ix) {
                            IExpression subExp = Calculator.Load(fData.GetStatement(ix));
                            item.Expressions.Add(subExp);
                        }
                        m_Clauses.Add(item);
                    }
                } else {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", fData.ToScriptString(false), fData.GetLine());
                }
            }
            return true;
        }

        private sealed class Clause
        {
            internal IExpression Condition;
            internal List<IExpression> Expressions = new List<IExpression>();
        }

        private List<Clause> m_Clauses = new List<Clause>();
    }
    internal sealed class WhileExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (; ; ) {
                object condVal = m_Condition.Calc(args);
                if (ToDouble(condVal) != 0) {
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc(args);
                    }
                } else {
                    break;
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent cond = funcData.Call.GetParam(0);
            m_Condition = Calculator.Load(cond);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_Condition;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            object count = m_Count.Calc(args);
            long ct = ToLong(count);
            for (int i = 0; i < ct; ++i) {
                Calculator.NamedVariables["$$"] = i;
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc(args);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent count = funcData.Call.GetParam(0);
            m_Count = Calculator.Load(count);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_Count;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class LoopListExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            object list = m_List.Calc(args);
            IEnumerable obj = list as IEnumerable;
            if (null != obj) {
                IEnumerator enumer = obj.GetEnumerator();
                while (enumer.MoveNext()) {
                    object val = enumer.Current;
                    Calculator.NamedVariables["$$"] = val;
                    for (int index = 0; index < m_Expressions.Count; ++index) {
                        v = m_Expressions[index].Calc(args);
                    }
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.ISyntaxComponent list = funcData.Call.GetParam(0);
            m_List = Calculator.Load(list);
            for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private IExpression m_List;
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ForeachExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            List<object> list = new List<object>();
            for (int ix = 0; ix < m_Elements.Count; ++ix) {
                object val = m_Elements[ix].Calc(args);
                list.Add(val);
            }
            IEnumerator enumer = list.GetEnumerator();
            while (enumer.MoveNext()) {
                object val = enumer.Current;
                Calculator.NamedVariables["$$"] = val;
                for (int index = 0; index < m_Expressions.Count; ++index) {
                    v = m_Expressions[index].Calc(args);
                }
            }
            return v;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            Dsl.CallData callData = funcData.Call;
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent cond = funcData.Call.GetParam(ix);
                m_Elements.Add(Calculator.Load(cond));
            }
            int fnum = funcData.GetStatementNum();
            for (int ix = 0; ix < fnum; ++ix) {
                IExpression subExp = Calculator.Load(funcData.GetStatement(ix));
                m_Expressions.Add(subExp);
            }
            return true;
        }

        private List<IExpression> m_Elements = new List<IExpression>();
        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class ParenthesisExp : AbstractExpression
    {
        public override object Calc(object[] args)
        {
            object v = 0;
            for (int ix = 0; ix < m_Expressions.Count; ++ix) {
                var exp = m_Expressions[ix];
                v = exp.Calc(args);
            }
            return v;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.ISyntaxComponent param = callData.GetParam(i);
                m_Expressions.Add(Calculator.Load(param));
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    public sealed class DslCalculator
    {
        public void Init()
        {
            Register("arg", new ExpressionFactoryHelper<ArgGet>());
            Register("var", new ExpressionFactoryHelper<VarGet>());
            Register("+", new ExpressionFactoryHelper<AddExp>());
            Register("-", new ExpressionFactoryHelper<SubExp>());
            Register("*", new ExpressionFactoryHelper<MulExp>());
            Register("/", new ExpressionFactoryHelper<DivExp>());
            Register("%", new ExpressionFactoryHelper<ModExp>());
            Register("&", new ExpressionFactoryHelper<BitAndExp>());
            Register("|", new ExpressionFactoryHelper<BitOrExp>());
            Register("^", new ExpressionFactoryHelper<BitXorExp>());
            Register("~", new ExpressionFactoryHelper<BitNotExp>());
            Register("<<", new ExpressionFactoryHelper<LShiftExp>());
            Register(">>", new ExpressionFactoryHelper<RShiftExp>());
            Register("max", new ExpressionFactoryHelper<MaxExp>());
            Register("min", new ExpressionFactoryHelper<MinExp>());
            Register("abs", new ExpressionFactoryHelper<AbsExp>());
            Register("sin", new ExpressionFactoryHelper<SinExp>());
            Register("cos", new ExpressionFactoryHelper<CosExp>());
            Register("tan", new ExpressionFactoryHelper<TanExp>());
            Register("asin", new ExpressionFactoryHelper<AsinExp>());
            Register("acos", new ExpressionFactoryHelper<AcosExp>());
            Register("atan", new ExpressionFactoryHelper<AtanExp>());
            Register("atan2", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", new ExpressionFactoryHelper<TanhExp>());
            Register("rndint", new ExpressionFactoryHelper<RndIntExp>());
            Register("rndfloat", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", new ExpressionFactoryHelper<SqrtExp>());
            Register("log", new ExpressionFactoryHelper<LogExp>());
            Register("log10", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", new ExpressionFactoryHelper<FloorExp>());
            Register("ceil", new ExpressionFactoryHelper<CeilExp>());
            Register("lerp", new ExpressionFactoryHelper<LerpExp>());
            Register("lerpunclamped", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerpangle", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("clamp01", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", new ExpressionFactoryHelper<ClampExp>());
            Register("dist", new ExpressionFactoryHelper<DistExp>());
            Register("distsqr", new ExpressionFactoryHelper<DistSqrExp>());
            Register(">", new ExpressionFactoryHelper<GreatExp>());
            Register(">=", new ExpressionFactoryHelper<GreatEqualExp>());
            Register("<", new ExpressionFactoryHelper<LessExp>());
            Register("<=", new ExpressionFactoryHelper<LessEqualExp>());
            Register("==", new ExpressionFactoryHelper<EqualExp>());
            Register("!=", new ExpressionFactoryHelper<NotEqualExp>());
            Register("&&", new ExpressionFactoryHelper<AndExp>());
            Register("||", new ExpressionFactoryHelper<OrExp>());
            Register("!", new ExpressionFactoryHelper<NotExp>());
            Register("?", new ExpressionFactoryHelper<CondExp>());
            Register("if", new ExpressionFactoryHelper<IfExp>());
            Register("while", new ExpressionFactoryHelper<WhileExp>());
            Register("loop", new ExpressionFactoryHelper<LoopExp>());
            Register("looplist", new ExpressionFactoryHelper<LoopListExp>());
            Register("foreach", new ExpressionFactoryHelper<ForeachExp>());
        }
        public void Register(string name, IExpressionFactory factory)
        {
            if (!m_ExpressionFactories.ContainsKey(name)) {
                m_ExpressionFactories.Add(name, factory);
            } else {
                m_ExpressionFactories[name] = factory;
            }
        }
        public void Load(string proc, Dsl.FunctionData func)
        {
            List<IExpression> list;
            if (!m_Procs.TryGetValue(proc, out list)) {
                list = new List<IExpression>();
                m_Procs.Add(proc, list);
            }
            foreach (Dsl.ISyntaxComponent comp in func.Statements) {
                var exp = Load(comp);
                if (null != exp) {
                    list.Add(exp);
                }
            }            
        }
        public object Calc(string proc, params object[] args)
        {
            object ret = 0;
            m_Variables.Clear();
            List<IExpression> exps;
            if (m_Procs.TryGetValue(proc, out exps)) {
                for (int i = 0; i < exps.Count; ++i) {
                    ret = exps[i].Calc(args);
                }
            }
            return ret;
        }

        public Dictionary<int, object> Variables
        {
            get { return m_Variables; }
        }
        public Dictionary<string, object> NamedVariables
        {
            get { return m_NamedVariables; }
        }
        internal IExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            if (null != valueData) {
                int idType = valueData.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    NamedVarGet varExp = new NamedVarGet();
                    varExp.Load(comp, this);
                    return varExp;
                } else {
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                }
            } else {
                Dsl.CallData callData = comp as Dsl.CallData;
                if (null != callData) {
                    if (!callData.HaveId()) {
                        int num = callData.GetParamNum();
                        if (num == 1) {
                            Dsl.ISyntaxComponent param = callData.GetParam(0);
                            return Load(param);
                        } else {
                            ParenthesisExp exp = new ParenthesisExp();
                            exp.Load(comp, this);
                            return exp;
                        }
                    } else {
                        string op = callData.GetId();
                        if (op == "=") {//赋值
                            string name = callData.GetParamId(0);
                            IExpression exp = null;
                            if (name == "var") {
                                exp = new VarSet();
                            } else {
                                exp = new NamedVarSet();
                            }
                            if (null != exp) {
                                exp.Load(comp, this);
                            } else {
                                //error
                                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", callData.ToScriptString(false), callData.GetLine());
                            }
                            return exp;
                        }
                    }
                }
            }
            IExpression ret = Create(comp.GetId());
            if (null != ret) {
                if (!ret.Load(comp, this)) {
                    //error
                    Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
                }
            } else {
                //error
                Debug.LogErrorFormat("DslCalculator error, {0} line {1}", comp.ToScriptString(false), comp.GetLine());
            }
            return ret;
        }

        private IExpression Create(string name)
        {
            IExpression ret = null;
            IExpressionFactory factory;
            if (m_ExpressionFactories.TryGetValue(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }

        private Dictionary<string, List<IExpression>> m_Procs = new Dictionary<string, List<IExpression>>();
        private Dictionary<int, object> m_Variables = new Dictionary<int, object>();
        private Dictionary<string, object> m_NamedVariables = new Dictionary<string, object>();
        private Dictionary<string, IExpressionFactory> m_ExpressionFactories = new Dictionary<string, IExpressionFactory>();
    }
}