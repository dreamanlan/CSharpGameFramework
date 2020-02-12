using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.MemoryProfiler;
using UnityEditorInternal;
using UnityEditor.Profiling.Memory.Experimental;
using Unity.MemoryProfilerForExtension.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using GameFramework;

internal static class ResourceEditUtility
{
    internal class BatchProcessInfo
    {
        internal string ResPath = string.Empty;
        internal string DslPath = string.Empty;
    }
    internal class ParamInfo
    {
        internal string Name;
        internal Type Type;
        internal object Value;
        internal string StringValue = string.Empty;
        internal string Encoding;
        internal object MinValue;
        internal object MaxValue;
        internal List<string> OptionNames = new List<string>();
        internal Dictionary<string, string> Options = new Dictionary<string, string>();
        internal string OptionStyle = string.Empty;
        internal string FileExts = string.Empty;
        internal string FileInitDir = string.Empty;
        internal string Script = string.Empty;
        internal double NextRunScriptTime = 0;
        //temp
        internal string[] PopupOptionNames = null;
        internal List<string> MultipleOldValues = null;
        internal List<string> MultipleNewValues = null;
    }
    internal class ItemInfo
    {
        internal string AssetPath;
        internal string ScenePath;
        internal AssetImporter Importer;
        internal UnityEngine.Object Object;
        internal string Info;
        internal double Order;
        internal double Value;
        internal string Group;
        internal IList<KeyValuePair<string, object>> ExtraList = null;
        internal object ExtraObject = null;
        internal string ExtraListBuildScript = string.Empty;
        internal string ExtraListClickScript = string.Empty;
        internal string RedirectDsl = string.Empty;
        internal IDictionary<string, string> RedirectArgs = new Dictionary<string, string>();
        internal bool Selected;

        internal void PrepareShowInfo()
        {
            if (string.IsNullOrEmpty(Info)) {
                Info = string.Format("{0},{1}", AssetPath, ScenePath);
                Order = 0;
                Value = 0;
            }
        }
    }
    internal class GroupInfo
    {
        internal string Group;
        internal List<ItemInfo> Items = new List<ItemInfo>();
        internal double Sum;
        internal double Max;
        internal double Min;

        internal int Count;
        internal double Avg;
        internal double Order;
        internal double Value;

        internal string AssetPath;
        internal string ScenePath;
        internal string Info;
        internal IList<KeyValuePair<string, object>> ExtraList = null;
        internal object ExtraObject = null;
        internal string ExtraListBuildScript = string.Empty;
        internal string ExtraListClickScript = string.Empty;
        internal string RedirectDsl = string.Empty;
        internal IDictionary<string, string> RedirectArgs = null;
        internal bool Selected;

        internal void CopyFrom(GroupInfo other)
        {
            Group = other.Group;
            Items = other.Items;
            Sum = other.Sum;
            Max = other.Max;
            Min = other.Min;

            Count = other.Count;
            Avg = other.Avg;
            Order = other.Order;
            Value = other.Value;

            AssetPath = other.AssetPath;
            ScenePath = other.ScenePath;
            Info = other.Info;
            ExtraList = other.ExtraList;
            ExtraObject = other.ExtraObject;
            ExtraListBuildScript = other.ExtraListBuildScript;
            ExtraListClickScript = other.ExtraListClickScript;
            RedirectDsl = other.RedirectDsl;
            RedirectArgs = other.RedirectArgs;
        }
        internal void PrepareShowInfo()
        {
            if (string.IsNullOrEmpty(Info)) {
                Count = Items.Count;
                Avg = Sum / Count;

                var sb = new StringBuilder();
                sb.AppendFormat("{0}=>Sum:{1},Max:{2},Min:{3},Avg:{4},Count:{5}", Group, Sum, Max, Min, Avg, Count);
                for (int i = 0; i < Items.Count; ++i) {
                    sb.Append(",");
                    sb.Append(Items[i].Info);
                }
                Order = Items.Count;
                Value = Sum;
                AssetPath = Items[0].AssetPath;
                ScenePath = Items[0].ScenePath;
                Info = sb.ToString();
            }
        }
    }
    internal class SceneDepInfo
    {
        internal HashSet<string> deps = new HashSet<string>();
        internal Dictionary<string, string> name2paths = new Dictionary<string, string>();

        internal void Clear()
        {
            deps.Clear();
            name2paths.Clear();
        }
    }
    internal class MemoryInfo
    {
        internal ulong instanceId;
        internal string name;
        internal string className;
        internal long size;
        internal int refCount;
        internal ulong address;
        internal bool isManaged;
        internal int sortedObjectIndex;
        internal ObjectData objectData;
    }
    internal class MemoryGroupInfo
    {
        internal string group;
        internal int count = 0;
        internal long size = 0;
        internal List<MemoryInfo> memories = new List<MemoryInfo>();
    }
    internal class InstrumentRecord
    {
        internal int depth;
        internal string name;
        internal string layerPath;
        internal float totalTime;
        internal float totalPercent;
        internal float selfTime;
        internal float selfPercent;
        internal float totalGpuTime;
        internal float totalGpuPercent;
        internal float selfGpuTime;
        internal float selfGpuPercent;
        internal float fps;
        internal int calls;
        internal float gcMemory;
    }
    internal class InstrumentInfo
    {
        internal int index;
        internal int frame;
        internal float fps;
        internal float totalGcMemory;
        internal int totalCalls;
        internal float totalCpuTime;
        internal float totalGpuTime;
        internal int sortType;
        internal int viewType;
        internal float batch;
        internal float triangle;
        internal List<InstrumentRecord> records = new List<InstrumentRecord>();
    }
    internal class SectionInfo
    {
        internal ulong vm_start = 0;
        internal ulong vm_end = 0;
        internal ulong size = 0;
    }
    internal class MapsInfo
    {
        internal ulong vm_start = 0;
        internal ulong vm_end = 0;
        internal ulong size = 0;
        internal string flags = string.Empty;
        internal string offset = string.Empty;
        internal string file1 = string.Empty;
        internal string file2 = string.Empty;
        internal string module = string.Empty;
    }
    internal class SmapsInfo
    {
        internal ulong vm_start = 0;
        internal ulong vm_end = 0;
        internal ulong size = 0;
        internal string flags = string.Empty;
        internal string offset = string.Empty;
        internal string file1 = string.Empty;
        internal string file2 = string.Empty;
        internal string module = string.Empty;

        internal ulong sizeKB = 0;
        internal ulong rss = 0;
        internal ulong pss = 0;
        internal ulong shared_clean = 0;
        internal ulong shared_dirty = 0;
        internal ulong private_clean = 0;
        internal ulong private_dirty = 0;
        internal ulong referenced = 0;
        internal ulong anonymous = 0;
        internal ulong swap = 0;
        internal ulong swappss = 0;
    }
    internal class SymbolInfo
    {
        internal ulong Addr;
        internal string Name;
    }
    internal class DataRow
    {
        internal int RowIndex
        {
            get { return m_RowIndex; }
        }
        internal int CellCount
        {
            get { return m_Fields.Length; }
        }
        internal string this[int ix]
        {
            get {
                return GetCell(ix);
            }
        }
        internal string GetCell(int ix)
        {
            if (ix >= 0 && ix < m_Fields.Length)
                return m_Fields[ix];
            else
                return string.Empty;
        }
        internal string GetLine()
        {
            return GetLine(0);
        }
        internal string GetLine(int skipCols)
        {
            if (skipCols == m_SkipCols && null == m_ColIndexes) {
                return m_Line;
            }
            m_Line = string.Join(m_Delimiter, m_Fields, skipCols, m_Fields.Length - skipCols);
            m_SkipCols = skipCols;
            m_ColIndexes = null;
            return m_Line;
        }
        internal string GetLine(int skipCols, IList<int> colIndexes)
        {
            if (null == colIndexes)
                return GetLine(skipCols);
            if (skipCols == m_SkipCols && null != m_ColIndexes && colIndexes.Count == m_ColIndexes.Count) {
                bool same = true;
                for (int i = 0; i < colIndexes.Count && i < m_ColIndexes.Count; ++i) {
                    if (colIndexes[i] != m_ColIndexes[i]) {
                        same = false;
                        break;
                    }
                }
                if (same)
                    return m_Line;
            }
            var strs = new string[colIndexes.Count];
            int curIx = 0;
            foreach (var ix in colIndexes) {
                if (ix >= skipCols) {
                    strs[curIx++] = m_Fields[ix];
                }
            }
            m_Line = string.Join(m_Delimiter, strs);
            m_SkipCols = skipCols;
            m_ColIndexes = colIndexes;
            return m_Line;
        }
        internal DataRow(int rowIndex, string line, char delimiter)
        {
            m_RowIndex = rowIndex;
            m_Line = line;
            m_SkipCols = 0;
            m_ColIndexes = null;
            m_Delimiter = delimiter.ToString();
            m_Fields = line.Split(delimiter);
        }

        private int m_RowIndex = 0;
        private string m_Line = null;
        private int m_SkipCols = 0;
        private IList<int> m_ColIndexes = null;
        private string m_Delimiter = string.Empty;
        private string[] m_Fields = null;
    }
    internal class DataTable
    {
        internal int RowCount
        {
            get { return m_Rows.Length; }
        }
        internal DataRow this[int ix]
        {
            get {
                return GetRow(ix);
            }
        }
        internal DataRow GetRow(int ix)
        {
            if (ix >= 0 && ix < m_Rows.Length)
                return m_Rows[ix];
            else
                return null;
        }
        internal void Load(string path, Encoding encoding, char delimiter)
        {
            var lines = File.ReadAllLines(path, encoding);
            if (null != lines && lines.Length > 0) {
                m_Rows = new DataRow[lines.Length];
                for (int ix = 0; ix < lines.Length; ++ix) {
                    var line = lines[ix];
                    m_Rows[ix] = new DataRow(ix, line, delimiter);
                }
            }
        }

        private DataRow[] m_Rows = null;
    }
    internal static void InitCalculator(DslExpression.DslCalculator calc)
    {
        calc.Init();
        calc.Register("callscript", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CallScriptExp>());
        calc.Register("setredirect", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetRedirectExp>());
        calc.Register("newitem", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.NewItemExp>());
        calc.Register("newextralist", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.NewExtraListExp>());
        calc.Register("extralistadd", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ExtraListAddExp>());
        calc.Register("extralistclear", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ExtraListClearExp>());
        calc.Register("getreferenceassets", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceAssetsExp>());
        calc.Register("getreferencebyassets", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceByAssetsExp>());
        calc.Register("calcrefcount", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CalcRefCountExp>());
        calc.Register("calcrefbycount", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CalcRefByCountExp>());
        calc.Register("findasset", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindAssetExp>());
        calc.Register("findshortestpathtoroot", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindShortestPathToRootExp>());
        calc.Register("getobjdatarefbyhash", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetObjectDataRefByHashExp>());
        calc.Register("getobjdatarefbylist", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetObjectDataRefByListExp>());
        calc.Register("openlink", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.OpenLinkExp>());
        calc.Register("openreferencelink", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.OpenReferenceLinkExp>());
        calc.Register("openlinkincurrenttable", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.OpenLinkInCurrentTableExp>());
        calc.Register("getcurrenttablenames", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCurrentTableNamesExp>());
        calc.Register("getcurrenttablecount", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCurrentTableCountExp>());
        calc.Register("getcurrenttablename", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCurrentTableNameExp>());
        calc.Register("getcurrenttable", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCurrentTableExp>());
        calc.Register("objdatafromaddress", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ObjectDataFromAddressExp>());
        calc.Register("objdatafromunifiedindex", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ObjectDataFromUnifiedObjectIndexExp>());
        calc.Register("objdatafromnativeindex", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ObjectDataFromNativeObjectIndexExp>());
        calc.Register("objdatafrommanagedindex", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ObjectDataFromManagedObjectIndexExp>());
        calc.Register("loadidaprosymbols", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.LoadIdaproSymbolsExp>());
        calc.Register("mapbuglysymbols", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.MapBuglySymbolsExp>());
        calc.Register("mapmyhooksymbols", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.MapMyhookSymbolsExp>());
        calc.Register("grep", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GrepExp>());
        calc.Register("subst", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SubstExp>());
        calc.Register("setclipboard", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetClipboardExp>());
        calc.Register("getclipboard", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetClipboardExp>());
        calc.Register("selectobject", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SelectObjectExp>());
        calc.Register("selectprojectobject", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SelectProjectObjectExp>());
        calc.Register("selectsceneobject", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SelectSceneObjectExp>());
        calc.Register("saveandreimport", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SaveAndReimportExp>());
        calc.Register("setdirty", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetDirtyExp>());
        calc.Register("getdefaulttexturesetting", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetDefaultTextureSettingExp>());
        calc.Register("gettexturesetting", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetTextureSettingExp>());
        calc.Register("settexturesetting", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetTextureSettingExp>());
        calc.Register("isastctexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.IsAstcTextureExp>());
        calc.Register("setastctexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetAstcTextureExp>());
        calc.Register("issceneastctexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.IsSceneAstcTextureExp>());
        calc.Register("setsceneastctexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetSceneAstcTextureExp>());
        calc.Register("istexturenoalphasource", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.IsTextureNoAlphaSourceExp>());
        calc.Register("doestexturehavealpha", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.DoesTextureHaveAlphaExp>());
        calc.Register("correctnonealphatexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CorrectNoneAlphaTextureExp>());
        calc.Register("setnonealphatexture", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetNoneAlphaTextureExp>());
        calc.Register("gettexturecompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetTextureCompressionExp>());
        calc.Register("settexturecompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetTextureCompressionExp>());
        calc.Register("getmeshcompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetMeshCompressionExp>());
        calc.Register("setmeshcompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetMeshCompressionExp>());
        calc.Register("setmeshimportexternalmaterials", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetMeshImportExternalMaterialsExp>());
        calc.Register("setmeshimportinprefabmaterials", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetMeshImportInPrefabMaterialsExp>());
        calc.Register("closemeshanimationifnoanimation", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CloseMeshAnimationIfNoAnimationExp>());
        calc.Register("collectmeshes", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CollectMeshesExp>());
        calc.Register("collectmeshinfo", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CollectMeshInfoExp>());
        calc.Register("collectanimatorcontrollerinfo", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CollectAnimatorControllerInfoExp>());
        calc.Register("collectprefabinfo", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CollectPrefabInfoExp>());
        calc.Register("getanimationclipinfo", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationClipInfoExp>());
        calc.Register("getanimationcompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationCompressionExp>());
        calc.Register("setanimationcompression", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationCompressionExp>());
        calc.Register("getanimationtype", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationTypeExp>());
        calc.Register("setanimationtype", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationTypeExp>());
        calc.Register("setextraexposedtransformpaths", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetExtraExposedTransformPathsExp>());
        calc.Register("clearanimationscalecurve", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.ClearAnimationScaleCurveExp>());
        calc.Register("getaudiosetting", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetAudioSettingExp>());
        calc.Register("setaudiosetting", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.SetAudioSettingExp>());
        calc.Register("calcmeshtexratio", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CalcMeshTexRatioExp>());
        calc.Register("calcassetmd5", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CalcAssetMd5Exp>());
        calc.Register("calcassetsize", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.CalcAssetSizeExp>());
        calc.Register("deleteasset", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.DeleteAssetExp>());
        calc.Register("getshaderutil", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetShaderUtilExp>());
        calc.Register("getshaderpropertycount", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetShaderPropertyCountExp>());
        calc.Register("getshaderpropertynames", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetShaderPropertyNamesExp>());
        calc.Register("getshadervariants", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetShaderVariantsExp>());
        calc.Register("addshadertocollection", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.AddShaderToCollectionExp>());
        calc.Register("findrowindex", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindRowIndexExp>());
        calc.Register("findrowindexes", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindRowIndexesExp>());
        calc.Register("findcellindex", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindCellIndexExp>());
        calc.Register("findcellindexes", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindCellIndexesExp>());
        calc.Register("getcellvalue", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCellValueExp>());
        calc.Register("getcellstring", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCellStringExp>());
        calc.Register("getcellnumeric", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.GetCellNumericExp>());
        calc.Register("rowtoline", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.RowToLineExp>());
        calc.Register("tabletohashtable", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.TableToHashtableExp>());
        calc.Register("findrowfromhashtable", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindRowFromHashtableExp>());
        calc.Register("loadmanagedheaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.LoadManagedHeapsExp>());
        calc.Register("findmanagedheaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindManagedHeapsExp>());
        calc.Register("loadmaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.LoadMapsExp>());
        calc.Register("findmaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindMapsExp>());
        calc.Register("loadsmaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.LoadSmapsExp>());
        calc.Register("findsmaps", new DslExpression.ExpressionFactoryHelper<ResourceEditApi.FindSmapsExp>());
    }
    internal static object Filter(ItemInfo item, Dictionary<string, object> addVars, List<ItemInfo> results, DslExpression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
    {
        try {
            item.PrepareShowInfo();
            results.Clear();
            object ret = null;
            if (null != calc) {
                calc.SetGlobalVariable("assetpath", item.AssetPath);
                calc.SetGlobalVariable("scenepath", item.ScenePath);
                calc.SetGlobalVariable("importer", item.Importer);
                calc.SetGlobalVariable("object", item.Object);
                calc.SetGlobalVariable("info", item.Info);
                calc.SetGlobalVariable("order", item.Order);
                calc.SetGlobalVariable("value", item.Value);
                calc.SetGlobalVariable("results", results);
                calc.SetGlobalVariable("scenedeps", sceneDeps);
                calc.SetGlobalVariable("refdict", refDict);
                calc.SetGlobalVariable("refbydict", refByDict);
                if (null != addVars) {
                    foreach (var pair in addVars) {
                        calc.SetGlobalVariable(pair.Key, pair.Value);
                    }
                }
                calc.SetGlobalVariable("params", args);
                foreach (var pair in args) {
                    var p = pair.Value;
                    calc.SetGlobalVariable(p.Name, p.Value);
                }
                calc.RemoveGlobalVariable("group");
                calc.RemoveGlobalVariable("extralist");

                for (int i = 0; i < indexCount; ++i) {
                    ret = calc.Calc(i.ToString());
                }

                object v;
                if (calc.TryGetGlobalVariable("assetpath", out v)) {
                    var path = v as string;
                    if (!string.IsNullOrEmpty(path))
                        item.AssetPath = path;
                }
                if (calc.TryGetGlobalVariable("scenepath", out v)) {
                    var path = v as string;
                    if (!string.IsNullOrEmpty(path))
                        item.ScenePath = path;
                }
                if (calc.TryGetGlobalVariable("importer", out v)) {
                    if (null != v) {
                        item.Importer = v as AssetImporter;
                    }
                }
                if (calc.TryGetGlobalVariable("object", out v)) {
                    if (null != v) {
                        item.Object = v as UnityEngine.Object;
                    }
                }
                if (calc.TryGetGlobalVariable("info", out v)) {
                    item.Info = v as string;
                    if (null == item.Info) {
                        item.Info = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("order", out v)) {
                    if (null != v) {
                        item.Order = (double)Convert.ChangeType(v, typeof(double));
                    }
                }
                if (calc.TryGetGlobalVariable("value", out v)) {
                    if (null != v) {
                        item.Value = (double)Convert.ChangeType(v, typeof(double));
                    }
                }
                if (calc.TryGetGlobalVariable("group", out v)) {
                    item.Group = v as string;
                    if (null == item.Group) {
                        item.Group = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("extralist", out v)) {
                    var list = v as IList;
                    if (null != list) {
                        var pairList = new List<KeyValuePair<string, object>>();
                        foreach (var pair in list) {
                            var keyObj = (KeyValuePair<string, object>)pair;
                            pairList.Add(keyObj);
                        }
                        item.ExtraList = pairList;
                    }
                    else {
                        item.ExtraList = null;
                    }
                }
                if (calc.TryGetGlobalVariable("extraobject", out v)) {
                    item.ExtraObject = v;
                }
                if (calc.TryGetGlobalVariable("extralistbuild", out v)) {
                    string scp = v as string;
                    if (!string.IsNullOrEmpty(scp)) {
                        item.ExtraListBuildScript = scp;
                    }
                    else {
                        item.ExtraListBuildScript = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("extralistclick", out v)) {
                    string scp = v as string;
                    if (!string.IsNullOrEmpty(scp)) {
                        item.ExtraListClickScript = scp;
                    }
                    else {
                        item.ExtraListClickScript = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("redirectdsl", out v)) {
                    item.RedirectDsl = v as string;
                }
                if (calc.TryGetGlobalVariable("redirectargs", out v)) {
                    var dict = v as IDictionary;
                    if (null != dict) {
                        var strDict = new Dictionary<string, string>();
                        var enumer = dict.GetEnumerator();
                        while (enumer.MoveNext()) {
                            var key = enumer.Key as string;
                            var val = enumer.Value as string;
                            if (null != key) {
                                strDict.Add(key, val);
                            }
                        }
                        item.RedirectArgs = strDict;
                    }
                }

                if (results.Count <= 0) {
                    results.Add(item);
                }
                else {
                    ret = results.Count;
                }
            }
            return ret;
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("filter {0} exception:{1}\n{2}", item.AssetPath, ex.Message, ex.StackTrace);
            return null;
        }
    }
    internal static object Process(ItemInfo item, DslExpression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
    {
        try {
            item.PrepareShowInfo();
            object ret = null;
            if (null != calc) {
                calc.SetGlobalVariable("assetpath", item.AssetPath);
                calc.SetGlobalVariable("scenepath", item.ScenePath);
                calc.SetGlobalVariable("importer", item.Importer);
                calc.SetGlobalVariable("object", item.Object);
                calc.SetGlobalVariable("info", item.Info);
                calc.SetGlobalVariable("order", item.Order);
                calc.SetGlobalVariable("value", item.Value);
                calc.SetGlobalVariable("scenedeps", sceneDeps);
                calc.SetGlobalVariable("refdict", refDict);
                calc.SetGlobalVariable("refbydict", refByDict);
                calc.SetGlobalVariable("params", args);
                foreach (var pair in args) {
                    var p = pair.Value;
                    calc.SetGlobalVariable(p.Name, p.Value);
                }

                for (int i = 0; i < indexCount; ++i) {
                    ret = calc.Calc(i.ToString());
                }
            }
            return ret;
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("process {0} exception:{1}\n{2}", item.AssetPath, ex.Message, ex.StackTrace);
            return null;
        }
    }
    internal static object Group(GroupInfo item, DslExpression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
    {
        try {
            item.PrepareShowInfo();
            object ret = null;
            if (null != calc) {
                calc.SetGlobalVariable("group", item.Group);
                calc.SetGlobalVariable("items", item.Items);
                calc.SetGlobalVariable("sum", item.Sum);
                calc.SetGlobalVariable("max", item.Max);
                calc.SetGlobalVariable("min", item.Min);
                calc.SetGlobalVariable("count", item.Count);
                calc.SetGlobalVariable("avg", item.Avg);
                calc.SetGlobalVariable("assetpath", item.AssetPath);
                calc.SetGlobalVariable("scenepath", item.ScenePath);
                calc.SetGlobalVariable("info", item.Info);
                calc.SetGlobalVariable("order", item.Order);
                calc.SetGlobalVariable("value", item.Value);
                calc.SetGlobalVariable("scenedeps", sceneDeps);
                calc.SetGlobalVariable("refdict", refDict);
                calc.SetGlobalVariable("refbydict", refByDict);
                calc.SetGlobalVariable("params", args);
                foreach (var pair in args) {
                    var p = pair.Value;
                    calc.SetGlobalVariable(p.Name, p.Value);
                }
                calc.RemoveGlobalVariable("extralist");

                for (int i = 0; i < indexCount; ++i) {
                    ret = calc.Calc(i.ToString());
                }

                object v;
                if (calc.TryGetGlobalVariable("assetpath", out v)) {
                    var path = v as string;
                    if (!string.IsNullOrEmpty(path))
                        item.AssetPath = path;
                }
                if (calc.TryGetGlobalVariable("scenepath", out v)) {
                    var path = v as string;
                    if (!string.IsNullOrEmpty(path))
                        item.ScenePath = path;
                }
                if (calc.TryGetGlobalVariable("info", out v)) {
                    item.Info = v as string;
                    if (null == item.Info) {
                        item.Info = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("order", out v)) {
                    if (null != v) {
                        item.Order = (double)Convert.ChangeType(v, typeof(double));
                    }
                }
                if (calc.TryGetGlobalVariable("value", out v)) {
                    if (null != v) {
                        item.Value = (double)Convert.ChangeType(v, typeof(double));
                    }
                }
                if (calc.TryGetGlobalVariable("extralist", out v)) {
                    var list = v as IList;
                    if (null != list) {
                        var pairList = new List<KeyValuePair<string, object>>();
                        foreach (var pair in list) {
                            var keyObj = (KeyValuePair<string, object>)pair;
                            pairList.Add(keyObj);
                        }
                        item.ExtraList = pairList;
                    }
                    else {
                        item.ExtraList = null;
                    }
                }
                if (calc.TryGetGlobalVariable("extraobject", out v)) {
                    item.ExtraObject = v;
                }
                if (calc.TryGetGlobalVariable("extralistbuild", out v)) {
                    string scp = v as string;
                    if (!string.IsNullOrEmpty(scp)) {
                        item.ExtraListBuildScript = scp;
                    }
                    else {
                        item.ExtraListBuildScript = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("extralistclick", out v)) {
                    string scp = v as string;
                    if (!string.IsNullOrEmpty(scp)) {
                        item.ExtraListClickScript = scp;
                    }
                    else {
                        item.ExtraListClickScript = string.Empty;
                    }
                }
                if (calc.TryGetGlobalVariable("redirectdsl", out v)) {
                    item.RedirectDsl = v as string;
                }
                if (calc.TryGetGlobalVariable("redirectargs", out v)) {
                    var dict = v as IDictionary;
                    if (null != dict) {
                        var strDict = new Dictionary<string, string>();
                        var enumer = dict.GetEnumerator();
                        while (enumer.MoveNext()) {
                            var key = enumer.Key as string;
                            var val = enumer.Value as string;
                            if (null != key) {
                                strDict.Add(key, val);
                            }
                        }
                        item.RedirectArgs = strDict;
                    }
                }
            }
            return ret;
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("group {0} exception:{1}\n{2}", item.AssetPath, ex.Message, ex.StackTrace);
            return null;
        }
    }
    internal static object GroupProcess(GroupInfo item, DslExpression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
    {
        try {
            item.PrepareShowInfo();
            object ret = null;
            if (null != calc) {
                calc.SetGlobalVariable("group", item.Group);
                calc.SetGlobalVariable("items", item.Items);
                calc.SetGlobalVariable("sum", item.Sum);
                calc.SetGlobalVariable("max", item.Max);
                calc.SetGlobalVariable("min", item.Min);
                calc.SetGlobalVariable("count", item.Count);
                calc.SetGlobalVariable("avg", item.Avg);
                calc.SetGlobalVariable("assetpath", item.AssetPath);
                calc.SetGlobalVariable("scenepath", item.ScenePath);
                calc.SetGlobalVariable("info", item.Info);
                calc.SetGlobalVariable("order", item.Order);
                calc.SetGlobalVariable("value", item.Value);
                calc.SetGlobalVariable("scenedeps", sceneDeps);
                calc.SetGlobalVariable("refdict", refDict);
                calc.SetGlobalVariable("refbydict", refByDict);
                calc.SetGlobalVariable("params", args);
                foreach (var pair in args) {
                    var p = pair.Value;
                    calc.SetGlobalVariable(p.Name, p.Value);
                }

                for (int i = 0; i < indexCount; ++i) {
                    ret = calc.Calc(i.ToString());
                }
            }
            return ret;
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("group process {0} exception:{1}\n{2}", item.AssetPath, ex.Message, ex.StackTrace);
            return null;
        }
    }
    internal static void ResetCommandCalculator()
    {
        s_CommandCalculator = null;
    }
    internal static object EvalScript(string code, Dictionary<string, ParamInfo> args, object obj, object item, Dictionary<string, object> addVars)
    {
        object r = null;
        string procCode = string.Format("script{{ {0}; }};", code);
        var file = new Dsl.DslFile();
        if (file.LoadFromString(procCode, "command", msg => { Console.WriteLine("{0}", msg); })) {
            var calc = GetCommandCalculator();
            calc.SetGlobalVariable("params", args);
            foreach (var pair in args) {
                var p = pair.Value;
                calc.SetGlobalVariable(p.Name, p.Value);
            }
            if (null != addVars) {
                foreach (var pair in addVars) {
                    calc.SetGlobalVariable(pair.Key, pair.Value);
                }
            }
            calc.LoadDsl("main", new string[] { "$obj", "$item" }, file.DslInfos[0].First);
            r = calc.Calc("main", obj, item);
        }
        return r;
    }

    internal static bool FindSceneObject(string name, string type, ref string assetPath, ref string scenePath, ref UnityEngine.Object sceneObj)
    {
        bool ret = false;
        for (int i = 0; i < EditorSceneManager.sceneCount; ++i) {
            var scene = EditorSceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            foreach (var obj in objs) {
                ret = FindChildObjectsRecursively(string.Empty, obj, name, type, ref assetPath, ref scenePath, ref sceneObj);
                if (ret)
                    break;
            }
            if (ret)
                break;
        }
        return ret;
    }
    private static bool FindChildObjectsRecursively(string path, GameObject obj, string name, string type, ref string assetPath, ref string scenePath, ref UnityEngine.Object sceneObj)
    {
        if (string.IsNullOrEmpty(path)) {
            path = obj.name;
        }
        else {
            path = path + "/" + obj.name;
        }
        bool ret = false;
        if (obj.name == name) {
            if (type == "GameObject") {
                ret = true;
            }
            else if (type == "ParticleSystem") {
                var comp = obj.GetComponent<ParticleSystem>();
                if (null != comp) {
                    ret = true;
                }
            }
            var prefabObj = PrefabUtility.GetPrefabObject(obj);
            var prefabPath = AssetDatabase.GetAssetPath(prefabObj);
            if (!ret) {
                var objs = AssetDatabase.LoadAllAssetsAtPath(prefabPath);
                foreach (var assetObject in objs) {
                    if (null != assetObject && assetObject.GetType().Name.EndsWith(type)) {
                        ret = true;
                        break;
                    }
                }
            }
            if (ret) {
                assetPath = prefabPath;
                if (string.IsNullOrEmpty(assetPath)) {
                    assetPath = string.Empty;
                }
                scenePath = path;
                sceneObj = obj;
            }
        }
        if (!ret) {
            var trans = obj.transform;
            int ct = trans.childCount;
            for (int i = 0; i < ct; ++i) {
                var t = trans.GetChild(i);
                ret = FindChildObjectsRecursively(path, t.gameObject, name, type, ref assetPath, ref scenePath, ref sceneObj);
                if (ret)
                    break;
            }
        }
        return ret;
    }

    internal static Regex GetRegex(string str)
    {
        Regex regex;
        if (!s_Regexes.TryGetValue(str, out regex)) {
            regex = new Regex(str, RegexOptions.Compiled);
        }
        return regex;
    }
    internal static double CellToNumeric(NPOI.SS.UserModel.ICell cell)
    {
        double r = 0.0;
        if (null != cell) {
            switch (cell.CellType) {
                case NPOI.SS.UserModel.CellType.Boolean:
                    r = cell.BooleanCellValue ? 1.0 : 0.0;
                    break;
                case NPOI.SS.UserModel.CellType.Numeric:
                    r = cell.NumericCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    double.TryParse(cell.StringCellValue, out r);
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                    switch (cell.CachedFormulaResultType) {
                        case NPOI.SS.UserModel.CellType.Boolean:
                            r = cell.BooleanCellValue ? 1.0 : 0.0;
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            r = cell.NumericCellValue;
                            break;
                        case NPOI.SS.UserModel.CellType.String:
                            double.TryParse(cell.StringCellValue, out r);
                            break;
                        default:
                            r = 0.0;
                            break;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Blank:
                default:
                    r = 0.0;
                    break;
            }
        }
        return r;
    }
    internal static string CellToString(NPOI.SS.UserModel.ICell cell)
    {
        string r = string.Empty;
        if (null != cell) {
            switch (cell.CellType) {
                case NPOI.SS.UserModel.CellType.Boolean:
                    r = cell.BooleanCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.Numeric:
                    r = cell.NumericCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    r = cell.StringCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                    switch (cell.CachedFormulaResultType) {
                        case NPOI.SS.UserModel.CellType.Boolean:
                            r = cell.BooleanCellValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            r = cell.NumericCellValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.String:
                            r = cell.StringCellValue;
                            break;
                        default:
                            r = string.Empty;
                            break;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Blank:
                default:
                    r = string.Empty;
                    break;
            }
        }
        return r;
    }
    internal static Transform FindChildRecursive(Transform parent, string boneName)
    {
        if (parent == null)
            return null;

        Transform t = parent.Find(boneName);
        if (null != t) {
            return t;
        }
        else {
            int ct = parent.childCount;
            for (int i = 0; i < ct; ++i) {
                t = FindChildRecursive(parent.GetChild(i), boneName);
                if (null != t) {
                    return t;
                }
            }
        }
        return null;
    }
    internal static bool IsPathMatch(string path, string filter)
    {
        string ext = Path.GetExtension(path);
        if (ext == ".meta") {
            return false;
        }
        List<string> infos;
        if (!s_PathMatchInfos.TryGetValue(filter, out infos)) {
            string[] filters = filter.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            infos = new List<string>(filters);
            s_PathMatchInfos.Add(filter, infos);
        }
        string fileName = Path.GetFileName(path);
        bool match = true;
        int startIx = 0;
        for (int i = 0; i < infos.Count; ++i) {
            var info = infos[i];
            var ix = fileName.IndexOf(info, startIx, StringComparison.CurrentCultureIgnoreCase);
            if (ix >= 0) {
                startIx = ix + info.Length;
            }
            else {
                match = false;
                break;
            }
        }
        return match;
    }
    internal static bool IsAssetPath(string path)
    {
        string rootPath = Application.dataPath.Replace('\\', '/');
        path = path.Replace('\\', '/');
        if (path.StartsWith(rootPath)) {
            return true;
        }
        else {
            return false;
        }
    }
    internal static string PathToAssetPath(string path)
    {
        return FilePathToRelativePath(path);
    }
    internal static string AssetPathToPath(string assetPath)
    {
        return RelativePathToFilePath(assetPath);
    }
    internal static string FilePathToRelativePath(string path)
    {
        string rootPath = GetRootPath();
        path = path.Replace('\\', '/');
        if (path.StartsWith(rootPath)) {
            path = path.Substring(rootPath.Length);
        }
        return path;
    }
    internal static string RelativePathToFilePath(string path)
    {
        string rootPath = GetRootPath();
        path = path.Replace('\\', '/');
        return rootPath + path;
    }
    internal static string GetRootPath()
    {
        const string c_AssetsDir = "Assets";
        if (string.IsNullOrEmpty(s_RootPath)) {
            s_RootPath = Application.dataPath.Replace('\\', '/');
            if (s_RootPath.EndsWith(c_AssetsDir))
                s_RootPath = s_RootPath.Substring(0, s_RootPath.Length - c_AssetsDir.Length);
        }
        return s_RootPath;
    }
    internal static bool EnableSaveAndReimport
    {
        get { return s_EnableSaveAndReimport; }
        set { s_EnableSaveAndReimport = value; }
    }
    internal static bool ForceSaveAndReimport
    {
        get { return s_ForceSaveAndReimport; }
        set { s_ForceSaveAndReimport = value; }
    }
    internal static bool UseFastCrawler
    {
        get { return s_UseFastCrawler; }
        set { s_UseFastCrawler = value; }
    }
    
    internal static void SelectObject(UnityEngine.Object obj)
    {
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(Selection.activeObject);
        //SceneView.lastActiveSceneView.FrameSelected(true);
        SceneView.FrameLastActiveSceneView();
    }
    internal static void SelectProjectObject(string assetPath)
    {
        if (assetPath.IndexOfAny(new char[] { '/', '\\' }) < 0) {
            var guids = AssetDatabase.FindAssets(assetPath);
            if (guids.Length >= 1) {
                int ct = 0;
                for (int i = 0; i < guids.Length; ++i) {
                    var temp = AssetDatabase.GUIDToAssetPath(guids[0]);
                    var name = Path.GetFileNameWithoutExtension(temp);
                    if (string.Compare(name, assetPath, true) == 0) {
                        ++ct;
                        if (ct == 1) {
                            assetPath = temp;
                        }
                    }
                }
                if (ct > 1) {
                    EditorUtility.DisplayDialog("alert", string.Format("有{0}个同名的资源，仅选中其中一个:{1}", ct, assetPath), "ok");
                }
            }
        }
        var objLoaded = AssetDatabase.LoadMainAssetAtPath(assetPath);
        if (objLoaded != null) {
            if (Selection.activeObject != null && !(Selection.activeObject is GameObject)) {
                Resources.UnloadAsset(Selection.activeObject);
                Selection.activeObject = null;
            }
            Selection.activeObject = objLoaded;
            //EditorGUIUtility.PingObject(Selection.activeObject);
        }
    }
    internal static void SelectSceneObject(string scenePath)
    {
        var obj = GameObject.Find(scenePath);
        if (null != obj) {
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(Selection.activeObject);
            //SceneView.lastActiveSceneView.FrameSelected(true);
            SceneView.FrameLastActiveSceneView();
        }
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
    private static DslExpression.DslCalculator GetCommandCalculator()
    {
        if (null == s_CommandCalculator) {
            s_CommandCalculator = new DslExpression.DslCalculator();
            InitCalculator(s_CommandCalculator);
        }
        return s_CommandCalculator;
    }

    private static void AppendLine(StringBuilder sb, string format, params object[] args)
    {
        sb.AppendFormat(format, args);
        sb.AppendLine();
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

    private static bool s_EnableSaveAndReimport = true;
    private static bool s_ForceSaveAndReimport = false;
    private static bool s_UseFastCrawler = true;

    private static Dictionary<string, Regex> s_Regexes = new Dictionary<string, Regex>();
    private static Dictionary<string, List<string>> s_PathMatchInfos = new Dictionary<string, List<string>>();
    private static string s_RootPath = string.Empty;
    private const string c_IndentString = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";

    private static DslExpression.DslCalculator s_CommandCalculator = null;
    private static string s_ResourceParamsDslFile = "resourceparams.dsl";
}

#region API
namespace ResourceEditApi
{
    internal class MeshInfo
    {
        public int skinnedMeshCount;
        public int meshFilterCount;
        public int vertexCount;
        public int triangleCount;
        public int boneCount;
        public int materialCount;
        public int clipCount;
        public int maxTexWidth;
        public int maxTexHeight;
        public string maxTexName;
        public string maxTexPropName;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public string maxKeyFrameClipName = string.Empty;
        public int updateWhenOffscreenCount = 0;
        public int animatorCount = 0;
        public int alwaysAnimateCount = 0;
        public List<MaterialInfo> materials = new List<MaterialInfo>();
        public List<AnimationClipInfo> clips = new List<AnimationClipInfo>();

        public void CollectMaterials(IList<Material> mats)
        {
            foreach (var mat in mats) {
                if (null != mat) {
                    var matInfo = new MaterialInfo();
                    matInfo.name = mat.name;
                    matInfo.shaderName = null == mat.shader ? string.Empty : mat.shader.name;
                    matInfo.maxTexWidth = 0;
                    matInfo.maxTexHeight = 0;
                    matInfo.maxTexName = string.Empty;
                    matInfo.maxTexPropName = string.Empty;
                    foreach (var prop in mat.GetTexturePropertyNames()) {
                        var tex = mat.GetTexture(prop);
                        if (null != tex) {
                            var texInfo = new TextureInfo();
                            texInfo.propName = prop;
                            texInfo.texName = tex.name;
                            texInfo.width = tex.width;
                            texInfo.height = tex.height;
                            matInfo.texs.Add(texInfo);

                            if (texInfo.width * texInfo.height > matInfo.maxTexWidth * matInfo.maxTexHeight) {
                                matInfo.maxTexWidth = texInfo.width;
                                matInfo.maxTexHeight = texInfo.height;
                                matInfo.maxTexName = texInfo.texName;
                                matInfo.maxTexPropName = texInfo.propName;

                                if (texInfo.width * texInfo.height > maxTexWidth * maxTexHeight) {
                                    maxTexWidth = texInfo.width;
                                    maxTexHeight = texInfo.height;
                                    maxTexName = texInfo.texName;
                                    maxTexPropName = texInfo.propName;
                                }
                            }
                        }
                    }

                    materials.Add(matInfo);
                }
            }
        }
        public void CollectClip(AnimationClip clip)
        {
            var clipInfo = new AnimationClipInfo();
            clipInfo.clipName = clip.name;
            var bindings = AnimationUtility.GetCurveBindings(clip);
            int maxKfc = 0;
            string curveName = string.Empty;
            foreach (var binding in bindings) {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                int kfc = curve.keys.Length;
                clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                if (maxKfc < kfc) {
                    maxKfc = kfc;
                    curveName = binding.path + "/" + binding.propertyName;
                }
            }
            clipInfo.maxKeyFrameCount = maxKfc;
            clipInfo.maxKeyFrameCurveName = curveName;
            clips.Add(clipInfo);

            if (maxKeyFrameCount < maxKfc) {
                maxKeyFrameCount = maxKfc;
                maxKeyFrameCurveName = curveName;
                maxKeyFrameClipName = clip.name;
            }
        }
    }
    internal class TextureInfo
    {
        public string propName;
        public string texName;
        public int width;
        public int height;
    }
    internal class MaterialInfo
    {
        public string name;
        public string shaderName;
        public int maxTexWidth;
        public int maxTexHeight;
        public string maxTexName;
        public string maxTexPropName;
        public List<TextureInfo> texs = new List<TextureInfo>();
    }
    internal class KeyFrameCurveInfo
    {
        public string curveName = string.Empty;
        public string curvePath = string.Empty;
        public int keyFrameCount;
    }
    internal class AnimationClipInfo
    {
        public string clipName = string.Empty;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public List<KeyFrameCurveInfo> curves = new List<KeyFrameCurveInfo>();
    }
    internal class AnimatorControllerInfo
    {
        public int layerCount;
        public int paramCount;
        public int stateCount;
        public int subStateMachineCount;
        public int clipCount;
        public int maxKeyFrameCount;
        public string maxKeyFrameCurveName = string.Empty;
        public string maxKeyFrameClipName = string.Empty;
        public List<AnimationClipInfo> clips = new List<AnimationClipInfo>();

        public void CollectClip(AnimationClip clip)
        {
            var clipInfo = new AnimationClipInfo();
            clipInfo.clipName = clip.name;
            var bindings = AnimationUtility.GetCurveBindings(clip);
            int maxKfc = 0;
            string curveName = string.Empty;
            foreach (var binding in bindings) {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                int kfc = curve.keys.Length;
                clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                if (maxKfc < kfc) {
                    maxKfc = kfc;
                    curveName = binding.path + "/" + binding.propertyName;
                }
            }
            clipInfo.maxKeyFrameCount = maxKfc;
            clipInfo.maxKeyFrameCurveName = curveName;
            clips.Add(clipInfo);

            if (maxKeyFrameCount < maxKfc) {
                maxKeyFrameCount = maxKfc;
                maxKeyFrameCurveName = curveName;
                maxKeyFrameClipName = clip.name;
            }
        }
    }
    internal class CallScriptExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var proc = operands[0] as string;
                if (null != proc) {
                    ArrayList arrayList = new ArrayList();
                    for (int i = 1; i < operands.Count; ++i) {
                        arrayList.Add(operands[i]);
                    }
                    r = ResourceProcessor.Instance.CallScript(Calculator, proc, arrayList.ToArray());
                }
            }
            return r;
        }
    }
    internal class SetRedirectExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dsl = operands[0] as string;
                if (!string.IsNullOrEmpty(dsl)) {
                    var args = new Dictionary<string, string>();
                    for (int i = 1; i < operands.Count - 1; i += 2) {
                        var key = operands[i] as string;
                        var val = operands[i + 1] as string;
                        if (!string.IsNullOrEmpty(key)) {
                            args.Add(key, val);
                        }
                    }
                    Calculator.SetGlobalVariable("redirectdsl", dsl);
                    Calculator.SetGlobalVariable("redirectargs", args);
                }
            }
            return r;
        }
    }
    internal class NewItemExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var results = Calculator.GetVariable("results") as List<ResourceEditUtility.ItemInfo>;
                if (null != results) {
                    var item = new ResourceEditUtility.ItemInfo();
                    item.PrepareShowInfo();
                    results.Add(item);
                    r = item;
                }
            }
            return r;
        }
    }
    internal class NewExtraListExp : DslExpression.AbstractExpression
    {
        protected override object DoCalc()
        {
            object r = null;
            var list = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < m_Expressions.Count - 1; i += 2) {
                var key = m_Expressions[i].Calc() as string;
                var val = m_Expressions[i + 1].Calc();
                if (null != key) {
                    list.Add(new KeyValuePair<string, object>(key, val));
                }
            }
            r = list;
            return r;
        }
        protected override bool Load(Dsl.CallData callData)
        {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                Dsl.CallData paramCallData = callData.GetParam(i) as Dsl.CallData;
                if (null != paramCallData && paramCallData.GetParamNum() == 2) {
                    var expKey = Calculator.Load(paramCallData.GetParam(0));
                    m_Expressions.Add(expKey);
                    var expVal = Calculator.Load(paramCallData.GetParam(1));
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            for (int i = 0; i < funcData.GetStatementNum(); ++i) {
                Dsl.CallData callData = funcData.GetStatement(i) as Dsl.CallData;
                if (null != callData && callData.GetParamNum() == 2) {
                    var expKey = Calculator.Load(callData.GetParam(0));
                    m_Expressions.Add(expKey);
                    var expVal = Calculator.Load(callData.GetParam(1));
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }

        private List<DslExpression.IExpression> m_Expressions = new List<DslExpression.IExpression>();
    }
    internal class ExtraListAddExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var list = operands[0] as List<KeyValuePair<string, object>>;
                string key = operands[1] as string;
                object val = operands[2];
                if (null != list && null != key) {
                    list.Add(new KeyValuePair<string, object>(key, val));
                }
            }
            return r;
        }
    }
    internal class ExtraListClearExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var list = operands[0] as List<KeyValuePair<string, object>>;
                if (null != list) {
                    list.Clear();
                }
            }
            return r;
        }
    }
    internal class GetReferenceAssetsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = Calculator.GetVariable("refdict") as Dictionary<string, HashSet<string>>;
                var path = operands[0] as string;
                if (null != dict && !string.IsNullOrEmpty(path)) {
                    HashSet<string> refbyset;
                    if (dict.TryGetValue(path, out refbyset)) {
                        r = refbyset.ToArray();
                    }
                    else {
                        r = new List<string>();
                    }
                }
            }
            return r;
        }
    }
    internal class GetReferenceByAssetsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var dict = Calculator.GetVariable("refbydict") as Dictionary<string, HashSet<string>>;
                var path = operands[0] as string;
                if (null != dict && !string.IsNullOrEmpty(path)) {
                    HashSet<string> refbyset;
                    if (dict.TryGetValue(path, out refbyset)) {
                        r = refbyset.ToArray();
                    }
                    else {
                        r = new List<string>();
                    }
                }
            }
            return r;
        }
    }
    internal class CalcRefCountExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var refDict = Calculator.GetVariable("refdict") as Dictionary<string, HashSet<string>>;
                var file = operands[0] as string;
                if (null != file) {
                    HashSet<string> hash;
                    if (refDict.TryGetValue(file, out hash)) {
                        r = hash.Count;
                    }
                    else {
                        r = 0;
                    }
                }
            }
            return r;
        }
    }
    internal class CalcRefByCountExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var refByDict = Calculator.GetVariable("refbydict") as Dictionary<string, HashSet<string>>;
                var file = operands[0] as string;
                if (null != file) {
                    HashSet<string> hash;
                    if (refByDict.TryGetValue(file, out hash)) {
                        r = hash.Count;
                    }
                    else {
                        r = 0;
                    }
                }
            }
            return r;
        }
    }
    internal class FindAssetExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var sceneDeps = Calculator.GetVariable("scenedeps") as ResourceEditUtility.SceneDepInfo;
                var asset = operands[0] as string;
                var type = operands[1] as string;
                var assetPath = string.Empty;
                var scenePath = string.Empty;
                UnityEngine.Object sceneObj = null;
                if (!string.IsNullOrEmpty(asset) && !string.IsNullOrEmpty(type)) {
                    bool handled = false;
                    var guids = AssetDatabase.FindAssets(asset);
                    for (int i = 0; i < guids.Length; ++i) {
                        var temp = AssetDatabase.GUIDToAssetPath(guids[0]);
                        var name = Path.GetFileNameWithoutExtension(temp);
                        if (string.Compare(name, asset, true) == 0) {
                            assetPath = temp;
                            handled = true;
                            bool result = ResourceEditUtility.FindSceneObject(asset, type, ref assetPath, ref scenePath, ref sceneObj);
                        }
                    }
                    if (!handled) {
                        bool result = ResourceEditUtility.FindSceneObject(asset, type, ref assetPath, ref scenePath, ref sceneObj);
                        if (result) {
                        }
                        else if (type == "Texture2D") {
                            if (sceneDeps.name2paths.TryGetValue(asset + ".png", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".tga", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".jpg", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".exr", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".hdr", out assetPath)) {
                            }
                        }
                        else if (type == "Mesh") {
                            sceneDeps.name2paths.TryGetValue(asset + ".fbx", out assetPath);
                        }
                        else if (type == "AnimationClip") {
                            if (sceneDeps.name2paths.TryGetValue(asset + ".clip", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".fbx", out assetPath)) {
                            }
                        }
                        else if (type == "Material") {
                            sceneDeps.name2paths.TryGetValue(asset + ".mat", out assetPath);
                        }
                        else if (type == "Shader") {
                            sceneDeps.name2paths.TryGetValue(asset + ".shader", out assetPath);
                        }
                        else {
                            if (sceneDeps.name2paths.TryGetValue(asset + ".prefab", out assetPath)) {
                            }
                            else if (sceneDeps.name2paths.TryGetValue(asset + ".asset", out assetPath)) {
                            }
                        }
                    }
                }
                r = new object[] { assetPath, scenePath, sceneObj };
            }
            return r;
        }
    }
    internal class FindShortestPathToRootExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj is ObjectData) {
                    var data = (ObjectData)obj;
                    r = ResourceProcessor.Instance.FindShortestPathToRoot(data);
                }
                else if (null != obj) {
                    try {
                        ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                        r = ResourceProcessor.Instance.FindShortestPathToRoot(addr);
                    }
                    catch {
                    }
                }
            }
            return r;
        }
    }
    internal class GetObjectDataRefByHashExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj is ObjectData) {
                    var data = (ObjectData)obj;
                    r = ResourceProcessor.Instance.GetObjectDataRefByHash(data);
                }
                else if (null != obj) {
                    try {
                        ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                        r = ResourceProcessor.Instance.GetObjectDataRefByHash(addr);
                    }
                    catch {
                    }
                }
            }
            return r;
        }
    }
    internal class GetObjectDataRefByListExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj is ObjectData) {
                    var data = (ObjectData)obj;
                    r = ResourceProcessor.Instance.GetObjectDataRefByList(data);
                }
                else if (null != obj) {
                    try {
                        ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                        r = ResourceProcessor.Instance.GetObjectDataRefByList(addr);
                    }
                    catch {
                    }
                }
            }
            return r;
        }
    }
    internal class OpenLinkExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj is ObjectData) {
                    var data = (ObjectData)obj;
                    ResourceProcessor.Instance.OpenLink(data);
                }
                else if (null != obj) {
                    try {
                        ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                        ResourceProcessor.Instance.OpenLink(addr);
                    }
                    catch {
                    }
                }
            }
            return r;
        }
    }
    internal class OpenReferenceLinkExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj is ObjectData) {
                    var data = (ObjectData)obj;
                    ResourceProcessor.Instance.OpenReferenceLink(data);
                }
                else if (null != obj) {
                    try {
                        ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                        ResourceProcessor.Instance.OpenReferenceLink(addr);
                    }
                    catch {
                    }
                }
            }
            return r;
        }
    }
    internal class OpenLinkInCurrentTableExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var index = (int)Convert.ChangeType(operands[0], typeof(int));
                List<string> list = new List<string>();
                for(int i = 1; i < operands.Count; ++i) {
                    string str = operands[i] as string;
                    list.Add(str);
                }
                ResourceProcessor.Instance.OpenLinkInCurrentTable(index, list);
            }
            return r;
        }
    }
    internal class GetCurrentTableNamesExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            var sb = new StringBuilder();
            int ct = ResourceProcessor.Instance.GetCurrentTableCount();
            for(int i = 0; i < ct; ++i) {
                sb.Append(i);
                sb.Append(':');
                var name = ResourceProcessor.Instance.GetCurrentTableName(i);
                sb.Append(name);
                if (i < ct - 1)
                    sb.Append(',');
            }
            return sb.ToString();
        }
    }
    internal class GetCurrentTableCountExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = ResourceProcessor.Instance.GetCurrentTableCount();
            return r;
        }
    }
    internal class GetCurrentTableNameExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var index = (int)Convert.ChangeType(operands[0], typeof(int));
                r = ResourceProcessor.Instance.GetCurrentTableName(index);
            }
            return r;
        }
    }
    internal class GetCurrentTableExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var index = (int)Convert.ChangeType(operands[0], typeof(int));
                r = ResourceProcessor.Instance.GetCurrentTable(index);
            }
            return r;
        }
    }
    internal class ObjectDataFromAddressExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                ulong addr = (ulong)Convert.ChangeType(obj, typeof(ulong));
                r = ResourceProcessor.Instance.ObjectDataFromAddress(addr);
            }
            return r;
        }
    }
    internal class ObjectDataFromUnifiedObjectIndexExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                int index = (int)Convert.ChangeType(obj, typeof(int));
                r = ResourceProcessor.Instance.ObjectDataFromUnifiedObjectIndex(index);
            }
            return r;
        }
    }
    internal class ObjectDataFromNativeObjectIndexExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                int index = (int)Convert.ChangeType(obj, typeof(int));
                r = ResourceProcessor.Instance.ObjectDataFromNativeObjectIndex(index);
            }
            return r;
        }
    }
    internal class ObjectDataFromManagedObjectIndexExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0];
                int index = (int)Convert.ChangeType(obj, typeof(int));
                r = ResourceProcessor.Instance.ObjectDataFromManagedObjectIndex(index);
            }
            return r;
        }
    }
    internal class LoadIdaproSymbolsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                if (!string.IsNullOrEmpty(file)) {
                    var fileName = Path.GetFileName(file);
                    var symbols = new List<ResourceEditUtility.SymbolInfo>();
                    using (var sr = new StreamReader(file)) {
                        int curCt = 0;
                        int totalCt = 500000;
                        while (!sr.EndOfStream) {
                            var line = sr.ReadLine();
                            var m = s_Address.Match(line);
                            if (m.Success) {
                                var addr = m.Groups[1].Value;
                                var name = m.Groups[2].Value;
                                ulong v;
                                ulong.TryParse(addr, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v);
                                if (name.IndexOf("loc_") != 0 && name.IndexOf("off_") != 0 && name.IndexOf("dword_") != 0) {
                                    symbols.Add(new ResourceEditUtility.SymbolInfo { Addr = v, Name = name });
                                }
                            }
                            ++curCt;
                            if (curCt > totalCt)
                                totalCt += 10000;
                            if (curCt % 1000 == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("load symbols from " + fileName, curCt, totalCt)) {
                                break;
                            }
                        }
                    }
                    r = symbols;
                }
            }
            EditorUtility.ClearProgressBar();
            return r;
        }
        private static Regex s_Address = new Regex(@"^ [0-9a-fA-F]+:([0-9a-fA-F]+)       (.*)", RegexOptions.Compiled);
    }
    internal class MapBuglySymbolsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var lines = operands[0] as IList<string>;
                var symbols = operands[1] as IList<ResourceEditUtility.SymbolInfo>;
                var key = operands[2] as string;
                if (null!=lines && null != symbols && null != key) {
                    for (int i = 0; i < lines.Count; ++i) {
                        lines[i] = lines[i].TrimEnd();
                    }
                    int ct = lines.Count;
                    for (int ix = 0; ix < ct; ++ix) {
                        var line = lines[ix];
                        var m = s_Address.Match(line);
                        if (m.Success) {
                            var addr = m.Groups[1].Value;
                            var so = m.Groups[2].Value;
                            if (so.Contains(key)) {
                                ulong v;
                                if (ulong.TryParse(addr, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v)) {
                                    int lo = 0;
                                    int hi = symbols.Count - 2;
                                    for (; lo <= hi;) {
                                        int i = (lo + hi) / 2;
                                        var st = symbols[i].Addr;
                                        var ed = symbols[i + 1].Addr;
                                        var name = symbols[i].Name;
                                        if (st > v) {
                                            hi = i - 1;
                                        }
                                        else if (ed <= v) {
                                            lo = i + 1;
                                        }
                                        else {
                                            lines[ix] = s_Remove.Replace(line, string.Empty) + " " + name;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (ResourceProcessor.Instance.DisplayCancelableProgressBar("map symbols ...", ix, ct)) {
                            break;
                        }
                    }
                    r = lines;
                }
            }
            EditorUtility.ClearProgressBar();
            return r;
        }
        private static Regex s_Address = new Regex(@"^[0-9]+ #[0-9]+ pc ([0-9a-fA-F]+) (\S+)", RegexOptions.Compiled);
        private static Regex s_Remove = new Regex(@"/[^=]*==", RegexOptions.Compiled);
    }
    internal class MapMyhookSymbolsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 4) {
                var lines = operands[0] as IList<string>;
                var section_start = (ulong)Convert.ChangeType(operands[1], typeof(ulong));
                var section_end = (ulong)Convert.ChangeType(operands[2], typeof(ulong));
                var symbols = operands[3] as IList<ResourceEditUtility.SymbolInfo>;
                var key = string.Empty;
                if (operands.Count >= 5)
                    key = operands[4] as string;
                if (null != lines && null != symbols && null != key) {
                    for (int i = 0; i < lines.Count; ++i) {
                        lines[i] = lines[i].TrimEnd();
                    }
                    int ct = lines.Count;
                    int delta = 1;
                    if (ct > 100000)
                        delta = 1000;
                    else if (ct > 10000)
                        delta = 100;
                    else if (ct > 1000)
                        delta = 10;
                    else
                        delta = 1;
                    for (int ix = 0; ix < ct; ++ix) {
                        var line = lines[ix];
                        var m = s_Address1.Match(line);
                        if (m.Success) {
                            var addr = m.Groups[1].Value;
                            var raddr = m.Groups[2].Value;
                            var so = m.Groups[3].Value;
                            if (!string.IsNullOrEmpty(key) && so.Contains(key)) {
                                ulong v;
                                if (ulong.TryParse(raddr, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v)) {
                                    string name = FindSymbol(v, symbols);
                                    if (!string.IsNullOrEmpty(name)) {
                                        lines[ix] = s_Remove.Replace(line, string.Empty) + " " + name;
                                    }
                                }
                            }
                            else {
                                ulong v;
                                if (ulong.TryParse(addr, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v) && v >= section_start && v < section_end) {
                                    v -= section_start;
                                    string name = FindSymbol(v, symbols);
                                    if (!string.IsNullOrEmpty(name)) {
                                        lines[ix] = s_Remove.Replace(line, string.Empty) + " " + name;
                                    }
                                }
                            }
                        }
                        else {
                            m = s_Address2.Match(line);
                            if (m.Success) {
                                var addr = m.Groups[1].Value;
                                ulong v;
                                if (ulong.TryParse(addr, System.Globalization.NumberStyles.AllowHexSpecifier, null, out v) && v >= section_start && v < section_end) {
                                    v -= section_start;
                                    string name = FindSymbol(v, symbols);
                                    if (!string.IsNullOrEmpty(name)) {
                                        lines[ix] = s_Remove.Replace(line, string.Empty) + " " + name;
                                    }
                                }
                            }
                        }
                        if (ix % delta == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("map symbols ...", ix, ct)) {
                            break;
                        }
                    }
                    r = lines;
                }
            }
            EditorUtility.ClearProgressBar();
            return r;
        }
        private static string FindSymbol(ulong v, IList<ResourceEditUtility.SymbolInfo> symbols)
        {
            int lo = 0;
            int hi = symbols.Count - 2;
            for (; lo <= hi;) {
                int i = (lo + hi) / 2;
                var st = symbols[i].Addr;
                var ed = symbols[i + 1].Addr;
                var name = symbols[i].Name;
                if (st > v) {
                    hi = i - 1;
                }
                else if (ed <= v) {
                    lo = i + 1;
                }
                else {
                    return name;
                }
            }
            return string.Empty;
        }

        private static Regex s_Address1 = new Regex(@"#[0-9]+:0x([0-9a-fA-F]+) 0x([0-9a-fA-F]+) (.*)", RegexOptions.Compiled);
        private static Regex s_Address2 = new Regex(@"#[0-9]+:0x([0-9a-fA-F]+)", RegexOptions.Compiled);
        private static Regex s_Remove = new Regex(@"\|[^=]*==", RegexOptions.Compiled);
    }
    internal class GrepExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var lines = operands[0] as IList<string>;
                Regex regex = null;
                if (operands.Count >= 2)
                    regex = new Regex(operands[1] as string, RegexOptions.Compiled);
                var outLines = new List<string>();
                if (null != lines) {
                    int ct = lines.Count;
                    int delta = 1;
                    if (ct > 100000)
                        delta = 1000;
                    else if (ct > 10000)
                        delta = 100;
                    else if (ct > 1000)
                        delta = 10;
                    else
                        delta = 1;
                    for (int i = 0; i < ct; ++i) {
                        string lineStr = lines[i];
                        if (null != regex) {
                            if (regex.IsMatch(lineStr)) {
                                outLines.Add(lineStr);
                            }
                        }
                        else {
                            outLines.Add(lineStr);
                        }
                        if (i % delta == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("grep ...", i, ct)) {
                            break;
                        }
                    }
                    r = outLines;
                }
            }
            EditorUtility.ClearProgressBar();
            return r;
        }
    }
    internal class SubstExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var lines = operands[0] as IList<string>;
                Regex regex = new Regex(operands[1] as string, RegexOptions.Compiled);
                string subst = operands[2] as string;
                int count = -1;
                if (operands.Count >= 4)
                    count = (int)Convert.ChangeType(operands[3], typeof(int));
                var outLines = new List<string>();
                if (null != lines && null != regex && null != subst) {
                    int ct = lines.Count;
                    int delta = 1;
                    if (ct > 100000)
                        delta = 1000;
                    else if (ct > 10000)
                        delta = 100;
                    else if (ct > 1000)
                        delta = 10;
                    else
                        delta = 1;
                    for (int i = 0; i < ct; ++i) {
                        string lineStr = lines[i];
                        lineStr = regex.Replace(lineStr, subst, count);
                        outLines.Add(lineStr);
                        if (i % delta == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("subst ...", i, ct)) {
                            break;
                        }
                    }
                    r = outLines;
                }
            }
            EditorUtility.ClearProgressBar();
            return r;
        }
    }
    internal class SetClipboardExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var str = operands[0] as string;
                if (null != str) {
                    GUIUtility.systemCopyBuffer = str;
                    r = str;
                }
            }
            return r;
        }
    }
    internal class GetClipboardExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = GUIUtility.systemCopyBuffer;
            return r;
        }
    }
    internal class SelectObjectExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    ResourceEditUtility.SelectObject(obj);
                    r = true;
                }
            }
            return r;
        }
    }
    internal class SelectProjectObjectExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (!string.IsNullOrEmpty(path)) {
                    ResourceEditUtility.SelectProjectObject(path);
                    r = true;
                }
            }
            return r;
        }
    }
    internal class SelectSceneObjectExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 1) {
                var path = operands[0] as string;
                if (!string.IsNullOrEmpty(path)) {
                    ResourceEditUtility.SelectSceneObject(path);
                    r = true;
                }
            }
            return r;
        }
    }
    internal class SaveAndReimportExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as AssetImporter;
                if (null != importer && ResourceEditUtility.EnableSaveAndReimport) {
                    //importer.SaveAndReimport();
                    //ResourceProcessor::Process会统一执行StartAssetEditing/StopAssetEditing
                    AssetDatabase.ImportAsset(importer.assetPath);
                }
            }
            return r;
        }
    }
    internal class SetDirtyExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.Object;
                if (null != obj) {
                    EditorUtility.SetDirty(obj);
                }
            }
            return r;
        }
    }
    internal class GetDefaultTextureSettingExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                if (null != importer) {
                    r = importer.GetDefaultPlatformTextureSettings();
                }
            }
            return r;
        }
    }
    internal class GetTextureSettingExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var platform = operands[0] as string;
                if (null != importer) {
                    r = importer.GetPlatformTextureSettings(platform);
                }
            }
            return r;
        }
    }
    internal class SetTextureSettingExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                if (null != importer && null != setting) {
                    importer.SetPlatformTextureSettings(setting);
                    r = setting;
                }
            }
            return r;
        }
    }
    internal class IsAstcTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = 0;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 8;
                if (null != importer && null != setting) {
                    bool ret = false;
                    if (importer.textureType == TextureImporterType.NormalMap && importer.maxTextureSize > 512) {
                        sizeNoAlpha = 6;
                        sizeAlpha = 6;
                    }
                    if (importer.alphaSource == TextureImporterAlphaSource.None) {
                        switch (sizeNoAlpha) {
                            case 4:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_4x4;
                                break;
                            case 5:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_5x5;
                                break;
                            case 6:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_6x6;
                                break;
                            case 8:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                            case 10:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_10x10;
                                break;
                            case 12:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_12x12;
                                break;
                            default:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                        }
                    }
                    else {
                        switch (sizeAlpha) {
                            case 4:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_4x4;
                                break;
                            case 5:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_5x5;
                                break;
                            case 6:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                            case 8:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_8x8;
                                break;
                            case 10:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_10x10;
                                break;
                            case 12:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_12x12;
                                break;
                            default:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                        }
                    }
                    r = ret ? 1 : 0;
                }
            }
            return r;
        }
    }
    internal class SetAstcTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 8;
                if (null != importer && null != setting) {
                    if (importer.textureType == TextureImporterType.NormalMap && importer.maxTextureSize > 512) {
                        sizeNoAlpha = 6;
                        sizeAlpha = 6;
                    }
                    if (importer.alphaSource == TextureImporterAlphaSource.None) {
                        switch (sizeNoAlpha) {
                            case 4:
                                setting.format = TextureImporterFormat.ASTC_RGB_4x4;
                                break;
                            case 5:
                                setting.format = TextureImporterFormat.ASTC_RGB_5x5;
                                break;
                            case 6:
                                setting.format = TextureImporterFormat.ASTC_RGB_6x6;
                                break;
                            case 8:
                                setting.format = TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                            case 10:
                                setting.format = TextureImporterFormat.ASTC_RGB_10x10;
                                break;
                            case 12:
                                setting.format = TextureImporterFormat.ASTC_RGB_12x12;
                                break;
                            default:
                                setting.format = TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                        }
                    }
                    else {
                        switch (sizeAlpha) {
                            case 4:
                                setting.format = TextureImporterFormat.ASTC_RGBA_4x4;
                                break;
                            case 5:
                                setting.format = TextureImporterFormat.ASTC_RGBA_5x5;
                                break;
                            case 6:
                                setting.format = TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                            case 8:
                                setting.format = TextureImporterFormat.ASTC_RGBA_8x8;
                                break;
                            case 10:
                                setting.format = TextureImporterFormat.ASTC_RGBA_10x10;
                                break;
                            case 12:
                                setting.format = TextureImporterFormat.ASTC_RGBA_12x12;
                                break;
                            default:
                                setting.format = TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                        }
                    }
                }
            }
            return r;
        }

        private static int[] s_AstcSizes = new int[] { 4, 5, 6, 8, 10, 12 };
    }
    internal class IsSceneAstcTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = 0;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 8;
                if (null != importer && null != setting) {
                    bool ret = false;
                    if (importer.alphaSource == TextureImporterAlphaSource.None) {
                        switch (sizeNoAlpha) {
                            case 4:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_4x4;
                                break;
                            case 5:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_5x5;
                                break;
                            case 6:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_6x6;
                                break;
                            case 8:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                            case 10:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_10x10;
                                break;
                            case 12:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_12x12;
                                break;
                            default:
                                ret = setting.format == TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                        }
                    }
                    else {
                        switch (sizeAlpha) {
                            case 4:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_4x4;
                                break;
                            case 5:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_5x5;
                                break;
                            case 6:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                            case 8:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_8x8;
                                break;
                            case 10:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_10x10;
                                break;
                            case 12:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_12x12;
                                break;
                            default:
                                ret = setting.format == TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                        }
                    }
                    r = ret ? 1 : 0;
                }
            }
            return r;
        }
    }
    internal class SetSceneAstcTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 8;
                if (null != importer && null != setting) {
                    if (importer.alphaSource == TextureImporterAlphaSource.None) {
                        switch (sizeNoAlpha) {
                            case 4:
                                setting.format = TextureImporterFormat.ASTC_RGB_4x4;
                                break;
                            case 5:
                                setting.format = TextureImporterFormat.ASTC_RGB_5x5;
                                break;
                            case 6:
                                setting.format = TextureImporterFormat.ASTC_RGB_6x6;
                                break;
                            case 8:
                                setting.format = TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                            case 10:
                                setting.format = TextureImporterFormat.ASTC_RGB_10x10;
                                break;
                            case 12:
                                setting.format = TextureImporterFormat.ASTC_RGB_12x12;
                                break;
                            default:
                                setting.format = TextureImporterFormat.ASTC_RGB_8x8;
                                break;
                        }
                    }
                    else {
                        switch (sizeAlpha) {
                            case 4:
                                setting.format = TextureImporterFormat.ASTC_RGBA_4x4;
                                break;
                            case 5:
                                setting.format = TextureImporterFormat.ASTC_RGBA_5x5;
                                break;
                            case 6:
                                setting.format = TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                            case 8:
                                setting.format = TextureImporterFormat.ASTC_RGBA_8x8;
                                break;
                            case 10:
                                setting.format = TextureImporterFormat.ASTC_RGBA_10x10;
                                break;
                            case 12:
                                setting.format = TextureImporterFormat.ASTC_RGBA_12x12;
                                break;
                            default:
                                setting.format = TextureImporterFormat.ASTC_RGBA_6x6;
                                break;
                        }
                    }
                }
            }
            return r;
        }

        private static int[] s_AstcSizes = new int[] { 4, 5, 6, 8, 10, 12 };
    }
    internal class IsTextureNoAlphaSourceExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                if (null != importer) {
                    r = importer.alphaSource == TextureImporterAlphaSource.None;
                }
            }
            return r;
        }
    }
    internal class DoesTextureHaveAlphaExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                if (null != importer) {
                    r = importer.DoesSourceTextureHaveAlpha();
                }
            }
            return r;
        }
    }
    internal class CorrectNoneAlphaTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                if (null != importer && !importer.DoesSourceTextureHaveAlpha()) {
                    importer.alphaSource = TextureImporterAlphaSource.None;
                    r = true;
                }
            }
            return r;
        }
    }
    internal class SetNoneAlphaTextureExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = false;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                if (null != importer) {
                    importer.alphaSource = TextureImporterAlphaSource.None;
                    r = true;
                }
            }
            return r;
        }
    }
    internal class GetTextureCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var setting = operands[0] as TextureImporterPlatformSettings;
                if (null != setting) {
                    switch (setting.textureCompression) {
                        case TextureImporterCompression.Uncompressed:
                            r = "none";
                            break;
                        case TextureImporterCompression.CompressedLQ:
                            r = "lowquality";
                            break;
                        case TextureImporterCompression.Compressed:
                            r = "normal";
                            break;
                        case TextureImporterCompression.CompressedHQ:
                            r = "highquality";
                            break;
                    }
                }
            }
            return r;
        }
    }
    internal class SetTextureCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var setting = operands[0] as TextureImporterPlatformSettings;
                var type = operands[1] as string;
                if (null != setting && null != type) {
                    r = type;
                    if (type == "none")
                        setting.textureCompression = TextureImporterCompression.Uncompressed;
                    else if (type == "lowquality")
                        setting.textureCompression = TextureImporterCompression.CompressedLQ;
                    else if (type == "normal")
                        setting.textureCompression = TextureImporterCompression.Compressed;
                    else if (type == "highquality")
                        setting.textureCompression = TextureImporterCompression.CompressedHQ;
                }
            }
            return r;
        }
    }
    internal class GetMeshCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    switch (importer.meshCompression) {
                        case ModelImporterMeshCompression.Off:
                            r = "off";
                            break;
                        case ModelImporterMeshCompression.Low:
                            r = "low";
                            break;
                        case ModelImporterMeshCompression.Medium:
                            r = "medium";
                            break;
                        case ModelImporterMeshCompression.High:
                            r = "high";
                            break;
                    }
                }
            }
            return r;
        }
    }
    internal class SetMeshCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                var type = operands[0] as string;
                if (null != importer && null != type) {
                    r = type;
                    if (type == "off")
                        importer.meshCompression = ModelImporterMeshCompression.Off;
                    else if (type == "low")
                        importer.meshCompression = ModelImporterMeshCompression.Low;
                    else if (type == "medium")
                        importer.meshCompression = ModelImporterMeshCompression.Medium;
                    else if (type == "high")
                        importer.meshCompression = ModelImporterMeshCompression.High;
                }
            }
            return r;
        }
    }
    internal class SetMeshImportExternalMaterialsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    r = true;
                    importer.importMaterials = true;
                    importer.materialLocation = ModelImporterMaterialLocation.External;
                    importer.materialName = ModelImporterMaterialName.BasedOnTextureName;
                    importer.materialSearch = ModelImporterMaterialSearch.RecursiveUp;
                }
            }
            return r;
        }
    }
    internal class SetMeshImportInPrefabMaterialsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    r = true;
                    importer.importMaterials = true;
                    importer.materialLocation = ModelImporterMaterialLocation.InPrefab;
                }
            }
            return r;
        }
    }
    internal class CloseMeshAnimationIfNoAnimationExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    if (importer.importedTakeInfos.Length <= 0 && importer.defaultClipAnimations.Length <= 0 && importer.clipAnimations.Length <= 0) {
                        importer.animationType = ModelImporterAnimationType.None;
                    }
                }
            }
            return r;
        }
    }
    internal class CollectMeshesExp : DslExpression.SimpleExpressionBase
    {
        internal enum ScopeEnum : int
        {
            None = 0,
            NonParticle = 1,
            Particle = 2,
            All = 3,
        }
        protected override object OnCalc(IList<object> operands)
        {
            if (operands.Count >= 1) {
                var obj0 = operands[0] as GameObject;
                bool includeChildren = false;
                if (operands.Count >= 2) {
                    includeChildren = (bool)Convert.ChangeType(operands[1], typeof(bool));
                }
                int scope = (int)ScopeEnum.All; //0--none 1--non particle 2--particle 3--all
                if (operands.Count >= 3) {
                    scope = (int)Convert.ChangeType(operands[2], typeof(int));
                }
                if (null != obj0) {
                    List<GameObject> list = new List<GameObject>();
                    if (includeChildren) {
                        var comps = obj0.GetComponentsInChildren<Renderer>();
                        foreach (var comp in comps) {
                            list.Add(comp.gameObject);
                        }
                    }
                    else {
                        list.Add(obj0);
                    }
                    List<Mesh> results = new List<Mesh>();
                    foreach (var obj in list) {
                        string objName = obj.name;
                        if ((scope & (int)ScopeEnum.NonParticle) > 0) {
                            var filters = obj.GetComponents<MeshFilter>();
                            foreach (var filter in filters) {
                                if (null != filter && null != filter.sharedMesh) {
                                    var mesh = filter.sharedMesh;
                                    results.Add(mesh);
                                }
                            }
                            var renderers = obj.GetComponents<SkinnedMeshRenderer>();
                            foreach (var renderer in renderers) {
                                if (null != renderer && null != renderer.sharedMesh) {
                                    var mesh = renderer.sharedMesh;
                                    results.Add(mesh);
                                }
                            }
                        }
                        if ((scope & (int)ScopeEnum.Particle) > 0) {
                            var pss = obj.GetComponents<ParticleSystem>();
                            foreach (ParticleSystem ps in pss) {
                                if (null != ps) {
                                    ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
                                    if (null != renderer && renderer.renderMode == ParticleSystemRenderMode.Mesh) {
                                        if (null != renderer.mesh) {
                                            var mesh = renderer.mesh;
                                            results.Add(mesh);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return results;
                }
            }
            return new List<Mesh>();
        }
    }
    internal class CollectMeshInfoExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.GameObject;
                ModelImporter importer = null;
                if (operands.Count >= 2)
                    importer = operands[1] as ModelImporter;
                if (null != obj) {
                    var info = new MeshInfo();
                    info.maxTexWidth = 0;
                    info.maxTexHeight = 0;
                    info.maxTexName = string.Empty;
                    info.maxTexPropName = string.Empty;
                    info.maxKeyFrameCount = 0;
                    info.maxKeyFrameCurveName = string.Empty;
                    info.maxKeyFrameClipName = string.Empty;
                    int vc = 0;
                    int tc = 0;
                    int bc = 0;
                    int mc = 0;
                    int offscreenct = 0;
                    var skinnedrenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    info.skinnedMeshCount = skinnedrenderers.Length;
                    foreach (var renderer in skinnedrenderers) {
                        if (null != renderer.sharedMesh) {
                            vc += renderer.sharedMesh.vertexCount;
                            tc += renderer.sharedMesh.triangles.Length;
                        }
                        bc += renderer.bones.Length;
                        mc += renderer.sharedMaterials.Length;
                        offscreenct += renderer.updateWhenOffscreen ? 1 : 0;

                        info.CollectMaterials(renderer.sharedMaterials);
                    }
                    var filters = obj.GetComponentsInChildren<MeshFilter>();
                    info.meshFilterCount = filters.Length;
                    foreach (var filter in filters) {
                        if (null != filter.sharedMesh) {
                            vc += filter.sharedMesh.vertexCount;
                            tc += filter.sharedMesh.triangles.Length;
                        }
                    }
                    var meshrenderers = obj.GetComponentsInChildren<MeshRenderer>();
                    foreach (var renderer in meshrenderers) {
                        mc += renderer.sharedMaterials.Length;

                        info.CollectMaterials(renderer.sharedMaterials);
                    }
                    info.vertexCount = vc;
                    info.triangleCount = tc / 3;
                    info.boneCount = bc;
                    info.materialCount = mc;
                    info.updateWhenOffscreenCount = offscreenct;
                    int alwaysct = 0;
                    var animators = obj.GetComponentsInChildren<Animator>();
                    info.animatorCount = animators.Length;
                    foreach (var anim in animators) {
                        alwaysct += anim.cullingMode == AnimatorCullingMode.AlwaysAnimate ? 1 : 0;
                    }
                    info.alwaysAnimateCount = alwaysct;
                    if (null != importer && info.clipCount <= 0) {
                        info.clipCount = importer.clipAnimations.Length;
                        if (info.clipCount <= 0)
                            info.clipCount = importer.defaultClipAnimations.Length;
                        var objs = AssetDatabase.LoadAllAssetsAtPath(importer.assetPath);
                        foreach (var clipObj in objs) {
                            var clip = clipObj as AnimationClip;
                            if (null != clip) {
                                if (importer.clipAnimations.Length > 0) {
                                    bool isDefault = false;
                                    foreach (var ci in importer.defaultClipAnimations) {
                                        if (ci.name == clip.name) {
                                            isDefault = true;
                                            break;
                                        }
                                    }
                                    if (isDefault)
                                        continue;
                                }
                                info.CollectClip(clip);
                            }
                        }
                    }
                    r = info;
                }
            }
            return r;
        }
    }
    internal class CollectAnimatorControllerInfoExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var ctrl = operands[0] as RuntimeAnimatorController;
                if (null != ctrl) {
                    var info = new AnimatorControllerInfo();
                    info.maxKeyFrameCount = 0;
                    info.maxKeyFrameCurveName = string.Empty;
                    info.maxKeyFrameClipName = string.Empty;
                    var editorCtrl = ctrl as UnityEditor.Animations.AnimatorController;
                    if (null == editorCtrl) {
                        var overrideCtrl = ctrl as AnimatorOverrideController;
                        if (null != overrideCtrl) {
                            editorCtrl = overrideCtrl.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
                        }
                    }
                    if (null != editorCtrl) {
                        info.layerCount = editorCtrl.layers.Length;
                        info.paramCount = editorCtrl.parameters.Length;
                        int sc = 0;
                        int smc = 0;
                        foreach (var layer in editorCtrl.layers) {
                            sc += CalcStateCount(layer.stateMachine);
                            smc += CalcSubStateMachineCount(layer.stateMachine);
                        }
                        info.stateCount = sc;
                        info.subStateMachineCount = smc;
                    }
                    info.clipCount = ctrl.animationClips.Length;
                    foreach (var clip in ctrl.animationClips) {
                        if (null != clip) {
                            info.CollectClip(clip);
                        }
                    }
                    r = info;
                }
            }
            return r;
        }
        private static int CalcStateCount(AnimatorStateMachine sm)
        {
            int ct = 0;
            ct += sm.states.Length;
            foreach (var ssm in sm.stateMachines) {
                ct += CalcStateCount(ssm.stateMachine);
            }
            return ct;
        }
        private static int CalcSubStateMachineCount(AnimatorStateMachine sm)
        {
            int ct = 0;
            ct += sm.stateMachines.Length;
            foreach (var ssm in sm.stateMachines) {
                ct += CalcSubStateMachineCount(ssm.stateMachine);
            }
            return ct;
        }
    }
    internal class CollectPrefabInfoExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var obj = operands[0] as UnityEngine.GameObject;
                if (null != obj) {
                    var info = new MeshInfo();
                    info.maxTexWidth = 0;
                    info.maxTexHeight = 0;
                    info.maxTexName = string.Empty;
                    info.maxTexPropName = string.Empty;
                    info.maxKeyFrameCount = 0;
                    info.maxKeyFrameCurveName = string.Empty;
                    info.maxKeyFrameClipName = string.Empty;
                    int vc = 0;
                    int tc = 0;
                    int bc = 0;
                    int mc = 0;
                    int offscreenct = 0;
                    var skinnedrenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    info.skinnedMeshCount = skinnedrenderers.Length;
                    foreach (var renderer in skinnedrenderers) {
                        if (null != renderer.sharedMesh) {
                            vc += renderer.sharedMesh.vertexCount;
                            tc += renderer.sharedMesh.triangles.Length;
                        }
                        bc += renderer.bones.Length;
                        mc += renderer.sharedMaterials.Length;
                        offscreenct += renderer.updateWhenOffscreen ? 1 : 0;

                        info.CollectMaterials(renderer.sharedMaterials);
                    }
                    var filters = obj.GetComponentsInChildren<MeshFilter>();
                    info.meshFilterCount = filters.Length;
                    foreach (var filter in filters) {
                        if (null != filter.sharedMesh) {
                            vc += filter.sharedMesh.vertexCount;
                            tc += filter.sharedMesh.triangles.Length;
                        }
                    }
                    var meshrenderers = obj.GetComponentsInChildren<MeshRenderer>();
                    foreach (var renderer in meshrenderers) {
                        mc += renderer.sharedMaterials.Length;

                        info.CollectMaterials(renderer.sharedMaterials);
                    }
                    var pss = obj.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in pss) {
                        if (null != ps) {
                            int multiple = (int)(ps.emission.rateOverTime.constant * ps.main.startLifetime.constant);
                            multiple = Mathf.Clamp(multiple, 1, ps.main.maxParticles);
                            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
                            if (null != renderer && renderer.renderMode == ParticleSystemRenderMode.Mesh) {
                                if (null != renderer.mesh) {
                                    vc += multiple * renderer.mesh.vertexCount;
                                    tc += multiple * renderer.mesh.triangles.Length;
                                }
                                mc += renderer.sharedMaterials.Length;

                                info.CollectMaterials(renderer.sharedMaterials);
                            }
                        }
                    }
                    info.vertexCount = vc;
                    info.triangleCount = tc / 3;
                    info.boneCount = bc;
                    info.materialCount = mc;
                    info.updateWhenOffscreenCount = offscreenct;
                    int alwaysct = 0;
                    var animators = obj.GetComponentsInChildren<Animator>();
                    info.animatorCount = animators.Length;
                    foreach (var anim in animators) {
                        alwaysct += anim.cullingMode == AnimatorCullingMode.AlwaysAnimate ? 1 : 0;
                    }
                    info.alwaysAnimateCount = alwaysct;
                    int clipCount = 0;
                    foreach (var anim in animators) {
                        var ctrl = anim.runtimeAnimatorController;
                        if (null != ctrl) {
                            clipCount += ctrl.animationClips.Length;
                            foreach (var clip in ctrl.animationClips) {
                                if (null != clip) {
                                    info.CollectClip(clip);
                                }
                            }
                        }
                    }
                    info.clipCount = clipCount;
                    r = info;
                }
            }
            return r;
        }
    }
    internal class GetAnimationClipInfoExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                string assetPath = Calculator.GetVariable("assetpath") as string;
                if (!string.IsNullOrEmpty(assetPath)) {
                    var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    if (null != obj) {
                        var clip = obj as AnimationClip;
                        if (null != clip) {
                            var clipInfo = new AnimationClipInfo();
                            clipInfo.clipName = clip.name;
                            var bindings = AnimationUtility.GetCurveBindings(clip);
                            int maxKfc = 0;
                            string curveName = string.Empty;
                            foreach (var binding in bindings) {
                                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                                int kfc = curve.keys.Length;
                                clipInfo.curves.Add(new KeyFrameCurveInfo { curveName = binding.propertyName, curvePath = binding.path, keyFrameCount = kfc });
                                if (maxKfc < kfc) {
                                    maxKfc = kfc;
                                    curveName = binding.path + "/" + binding.propertyName;
                                }
                            }
                            clipInfo.maxKeyFrameCount = maxKfc;
                            clipInfo.maxKeyFrameCurveName = curveName;
                            r = clipInfo;
                        }
                        Resources.UnloadAsset(obj);
                    }
                }
            }
            return r;
        }
    }
    internal class GetAnimationCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    switch (importer.animationCompression) {
                        case ModelImporterAnimationCompression.Off:
                            r = "off";
                            break;
                        case ModelImporterAnimationCompression.KeyframeReduction:
                            r = "keyframe";
                            break;
                        case ModelImporterAnimationCompression.KeyframeReductionAndCompression:
                            r = "keyframeandcompression";
                            break;
                        case ModelImporterAnimationCompression.Optimal:
                            r = "optimal";
                            break;
                    }
                }
            }
            return r;
        }
    }
    internal class SetAnimationCompressionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                var type = operands[0] as string;
                if (null != importer && null != type) {
                    r = type;
                    if (type == "off")
                        importer.animationCompression = ModelImporterAnimationCompression.Off;
                    else if (type == "keyframe")
                        importer.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
                    else if (type == "keyframeandcompression")
                        importer.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
                    else if (type == "optimal")
                        importer.animationCompression = ModelImporterAnimationCompression.Optimal;
                }
            }
            return r;
        }
    }
    internal class GetAnimationTypeExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                if (null != importer) {
                    switch (importer.animationType) {
                        case ModelImporterAnimationType.None:
                            r = "none";
                            break;
                        case ModelImporterAnimationType.Legacy:
                            r = "legacy";
                            break;
                        case ModelImporterAnimationType.Generic:
                            r = "generic";
                            break;
                        case ModelImporterAnimationType.Human:
                            r = "humanoid";
                            break;
                    }
                }
            }
            return r;
        }
    }
    internal class SetAnimationTypeExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                var type = operands[0] as string;
                if (null != importer && null != type) {
                    r = type;
                    if (type == "none")
                        importer.animationType = ModelImporterAnimationType.None;
                    else if (type == "legacy")
                        importer.animationType = ModelImporterAnimationType.Legacy;
                    else if (type == "generic")
                        importer.animationType = ModelImporterAnimationType.Generic;
                    else if (type == "humanoid")
                        importer.animationType = ModelImporterAnimationType.Human;
                }
            }
            return r;
        }
    }
    internal class SetExtraExposedTransformPathsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var importer = Calculator.GetVariable("importer") as ModelImporter;
                var obj = operands[0] as UnityEngine.GameObject;
                if (null != importer && null != obj) {
                    List<string> paths = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var name = operands[i] as string;
                        if (!string.IsNullOrEmpty(name)) {
                            if (name == "*") {
                                var boneListPath = ResourceEditUtility.AssetPathToPath("Assets/StreamingAssets/BoneList.txt");
                                var lines = File.ReadAllLines(boneListPath);
                                foreach (var line in lines) {
                                    AddBonePath(paths, obj.transform, line.Trim());
                                }
                            }
                            else {
                                AddBonePath(paths, obj.transform, name);
                            }
                        }
                    }
                    importer.extraExposedTransformPaths = paths.ToArray();
                }
            }
            return r;
        }
        private static void AddBonePath(List<string> paths, Transform root, string name)
        {
            var t = ResourceEditUtility.FindChildRecursive(root, name);
            if (null != t) {
                var path = CalcPath(t);
                paths.Add(path);
            }
        }
        private static string CalcPath(Transform t)
        {
            List<string> names = new List<string>();
            while (null != t) {
                names.Insert(0, t.name);
                if (t.parent == t)
                    break;
                t = t.parent;
            }
            return string.Join("/", names.ToArray());
        }
    }
    internal class ClearAnimationScaleCurveExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 0) {
                var path = Calculator.GetVariable("assetpath") as string;
                if (null != path) {
                    GameObject obj = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
                    bool modified = false;
                    List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(obj));
                    foreach (AnimationClip theAnimation in animationClipList) {
                        foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation)) {
                            string name = theCurveBinding.propertyName.ToLower();
                            if (name.Contains("scale")) {
                                AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                                modified = true;
                            }
                        }
                    }
                    if (modified) {
                        EditorUtility.SetDirty(obj);
                    }
                }
            }
            return r;
        }
    }
    internal class GetAudioSettingExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as AudioImporter;
                var platform = operands[0] as string;
                if (null != importer) {
                    r = importer.GetOverrideSampleSettings(platform);
                }
            }
            return r;
        }
    }
    internal class SetAudioSettingExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var importer = Calculator.GetVariable("importer") as AudioImporter;
                var platform = operands[0] as string;
                var setting = (AudioImporterSampleSettings)operands[1];
                if (null != importer) {
                    importer.SetOverrideSampleSettings(platform, setting);
                    r = setting;
                }
            }
            return r;
        }
    }
    internal class CalcMeshTexRatioExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object ret = new object[] { string.Empty, 0 };
            if (operands.Count >= 1) {
                var obj0 = operands[0] as GameObject;
                bool includeChildren = false;
                if (operands.Count >= 2) {
                    includeChildren = (bool)Convert.ChangeType(operands[1], typeof(bool));
                }
                if (null != obj0) {
                    List<GameObject> list = new List<GameObject>();
                    if (includeChildren) {
                        var comps = obj0.GetComponentsInChildren<Renderer>();
                        foreach (var comp in comps) {
                            list.Add(comp.gameObject);
                        }
                    }
                    else {
                        list.Add(obj0);
                    }
                    List<string> uvRatios = new List<string>();
                    List<string> uvTiledRatios = new List<string>();
                    float maxVal = 0;
                    float firstRatio1 = 0;
                    float firstRatio2 = 0;
                    foreach (var obj in list) {
                        var areas = MeshAreaHelper.CalculateMeshAreaWorld(obj);
                        var uvs = MeshAreaHelper.CalculateMeshUVAreaObject(obj);
                        var tiledUvs = MeshAreaHelper.CalculateMeshUVAreaTile(obj);
                        var texSizes = MeshAreaHelper.TextureSize(obj);
                        if (areas.Count == uvs.Count) {
                            Vector2 lastSize = Vector2.one;
                            for (int i = 0; i < areas.Count; ++i) {
                                var area = areas[i];
                                if (area < Geometry.c_FloatPrecision) {
                                    uvRatios.Add("0");
                                }
                                else {
                                    float r = uvs[i] / areas[i];
                                    if (i < texSizes.Count) {
                                        var v = texSizes[i];
                                        r *= v.x * v.y;
                                        lastSize = v;
                                    }
                                    else {
                                        r *= lastSize.x * lastSize.y;
                                    }
                                    if (maxVal < r) {
                                        maxVal = r;
                                    }
                                    if (firstRatio1 * 1000 < Geometry.c_FloatPrecision) {
                                        firstRatio1 = r;
                                    }
                                    uvRatios.Add(r.ToString("f6"));
                                }
                            }
                        }
                        if (areas.Count == tiledUvs.Count) {
                            Vector2 lastSize = Vector2.one;
                            for (int i = 0; i < areas.Count; ++i) {
                                var area = areas[i];
                                if (area < Geometry.c_FloatPrecision) {
                                    uvTiledRatios.Add("0");
                                }
                                else {
                                    float r = tiledUvs[i] / areas[i];
                                    if (i < texSizes.Count) {
                                        var v = texSizes[i];
                                        r *= v.x * v.y;
                                        lastSize = v;
                                    }
                                    else {
                                        r *= lastSize.x * lastSize.y;
                                    }
                                    if (maxVal < r) {
                                        maxVal = r;
                                    }
                                    if (firstRatio2 * 1000 < Geometry.c_FloatPrecision) {
                                        firstRatio2 = r;
                                    }
                                    uvTiledRatios.Add(r.ToString("f6"));
                                }
                            }
                        }
                    }
                    var info = string.Format("uv/area:({0}) tiled uv/area:({1})", string.Join(", ", uvRatios.ToArray()), string.Join(", ", uvTiledRatios.ToArray()));
                    ret = new object[] { info, maxVal, firstRatio1, firstRatio2 };
                }
            }
            return ret;
        }
    }
    internal class CalcAssetMd5Exp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                if (null != file) {
                    file = ResourceEditUtility.AssetPathToPath(file);
                    byte[] buffer = File.ReadAllBytes(file);
                    MD5 mD = MD5.Create();
                    byte[] array = mD.ComputeHash(buffer);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < array.Length; i++) {
                        stringBuilder.Append(array[i].ToString("x2"));
                    }
                    r = stringBuilder.ToString();
                }
            }
            return r;
        }
    }
    internal class CalcAssetSizeExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                if (null != file) {
                    file = ResourceEditUtility.AssetPathToPath(file);
                    var fileInfo = new FileInfo(file);
                    r = fileInfo.Length;
                }
            }
            return r;
        }
    }
    internal class DeleteAssetExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                if (null != file) {
                    r = AssetDatabase.DeleteAsset(file);
                }
            }
            return r;
        }
    }
    internal class GetShaderUtilExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = typeof(ShaderUtil);
            return r;
        }
    }
    internal class GetShaderPropertyCountExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var shader = operands[0] as Shader;
                if (null != shader) {
                    if (operands.Count >= 2) {
                        int type = (int)Convert.ChangeType(operands[1], typeof(int));
                        int count = 0;
                        int ct = ShaderUtil.GetPropertyCount(shader);
                        for (int i = 0; i < ct; ++i) {
                            var t = ShaderUtil.GetPropertyType(shader, i);
                            if ((int)t == type) {
                                ++count;
                            }
                        }
                        r = count;
                    }
                    else {
                        r = ShaderUtil.GetPropertyCount(shader);
                    }
                }
            }
            return r;
        }
    }
    internal class GetShaderPropertyNamesExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var shader = operands[0] as Shader;
                int type = (int)ShaderUtil.ShaderPropertyType.TexEnv;
                if (operands.Count >= 2) {
                    type = (int)Convert.ChangeType(operands[1], typeof(int));
                }
                if (null != shader) {
                    List<string> list = new List<string>();
                    int ct = ShaderUtil.GetPropertyCount(shader);
                    for (int i = 0; i < ct; ++i) {
                        var t = ShaderUtil.GetPropertyType(shader, i);
                        if ((int)t == type) {
                            var name = ShaderUtil.GetPropertyName(shader, i);
                            list.Add(name);
                        }
                    }
                    r = list;
                }
            }
            return r;
        }
    }
    internal class GetShaderVariantsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var shader = operands[0] as Shader;
                if (null != shader) {
                    if (null == s_EmptyShaderVariants) {
                        s_EmptyShaderVariants = Resources.Load("EmptyShaderVariants") as ShaderVariantCollection;
                    }
                    if (null != s_EmptyShaderVariants) {
                        var t = typeof(ShaderUtil);
                        var args = new object[] { shader, s_EmptyShaderVariants, null, null };
                        t.InvokeMember("GetShaderVariantEntries", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, args);
                        var types = args[2] as int[];
                        var keywords = args[3] as string[];
                        r = new object[] { types, keywords };
                    }
                }
            }
            return r;
        }
        private static ShaderVariantCollection s_EmptyShaderVariants = null;
    }
    internal class AddShaderToCollectionExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var shader = operands[0] as Shader;
                ShaderVariantCollection coll = null;
                if (operands.Count >= 2) {
                    coll = operands[1] as ShaderVariantCollection;
                }
                else {
                    if (null == s_DefaultShaderVariants) {
                        s_DefaultShaderVariants = AssetDatabase.LoadAssetAtPath("Assets/ResourceAB/ShaderVariants/ShaderVariants.shadervariants", typeof(UnityEngine.Object)) as ShaderVariantCollection;
                    }
                    coll = s_DefaultShaderVariants;
                }
                if (null != shader && null != coll) {
                    var t = typeof(ShaderUtil);
                    var args = new object[] { shader, coll };
                    r = t.InvokeMember("AddNewShaderToCollection", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, args);
                    EditorUtility.SetDirty(coll);
                }
            }
            return r;
        }

        private static ShaderVariantCollection s_DefaultShaderVariants = null;
    }
    internal class FindRowIndexExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int r = -1;
            if (operands.Count >= 3) {
                var sheet = operands[0] as NPOI.SS.UserModel.ISheet;
                var dict = new Dictionary<int, string>();
                for (int ix = 1; ix < operands.Count - 1; ix += 2) {
                    var index = ToInt(operands[ix]);
                    var val = operands[ix + 1].ToString();
                    dict.Add(index, val);
                }
                if (null != sheet) {
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
                        var row = sheet.GetRow(i);
                        bool find = true;
                        foreach (var pair in dict) {
                            var ix = pair.Key;
                            var val = pair.Value;
                            var cell = row.GetCell(ix);
                            if (val != ResourceEditUtility.CellToString(cell).Trim()) {
                                find = false;
                                break;
                            }
                        }
                        if (find) {
                            r = i;
                            break;
                        }
                    }
                }
                else {
                    var tb = operands[0] as ResourceEditUtility.DataTable;
                    if (null != tb) {
                        for (int i = 0; i < tb.RowCount; ++i) {
                            var trow = tb.GetRow(i);
                            bool find = true;
                            foreach (var pair in dict) {
                                var ix = pair.Key;
                                var val = pair.Value;
                                if (val != trow.GetCell(ix).Trim()) {
                                    find = false;
                                    break;
                                }
                            }
                            if (find) {
                                r = i;
                                break;
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class FindRowIndexesExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            var list = new List<int>();
            if (operands.Count >= 3) {
                var sheet = operands[0] as NPOI.SS.UserModel.ISheet;
                var dict = new Dictionary<int, Regex>();
                for (int ix = 1; ix < operands.Count - 1; ix += 2) {
                    var index = ToInt(operands[ix]);
                    var val = ResourceEditUtility.GetRegex(operands[ix + 1].ToString());
                    dict.Add(index, val);
                }
                if (null != sheet) {
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
                        var row = sheet.GetRow(i);
                        bool find = true;
                        foreach (var pair in dict) {
                            var ix = pair.Key;
                            var val = pair.Value;
                            var cell = row.GetCell(ix);
                            if (val.IsMatch(ResourceEditUtility.CellToString(cell).Trim())) {
                                find = false;
                                break;
                            }
                        }
                        if (find) {
                            list.Add(i);
                        }
                    }
                }
                else {
                    var tb = operands[0] as ResourceEditUtility.DataTable;
                    if (null != tb) {
                        for (int i = 0; i < tb.RowCount; ++i) {
                            var trow = tb.GetRow(i);
                            bool find = true;
                            foreach (var pair in dict) {
                                var ix = pair.Key;
                                var val = pair.Value;
                                if (val.IsMatch(trow.GetCell(ix).Trim())) {
                                    find = false;
                                    break;
                                }
                            }
                            if (find) {
                                list.Add(i);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
    internal class FindCellIndexExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            int r = -1;
            if (operands.Count >= 2) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                var name = operands[1] as string;
                if (!string.IsNullOrEmpty(name)) {
                    if (null != row) {
                        for (int i = row.FirstCellNum; i <= row.LastCellNum; ++i) {
                            var cell = row.GetCell(i);
                            if (null != cell && ResourceEditUtility.CellToString(cell).Trim() == name) {
                                r = i;
                                break;
                            }
                        }
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            for (int i = 0; i < trow.CellCount; ++i) {
                                var val = trow.GetCell(i);
                                if (val == name) {
                                    r = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class FindCellIndexesExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            IList<int> r = null;
            if (operands.Count >= 2) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                var nameObjs = operands[1] as IList;
                var markChars = string.Empty;
                List<NPOI.SS.UserModel.IRow> markRows = null;
                if (operands.Count >= 4) {
                    markChars = operands[2] as string;
                    markRows = new List<NPOI.SS.UserModel.IRow>();
                    for (int ix = 3; ix < operands.Count; ++ix) {
                        var markRow = operands[ix] as NPOI.SS.UserModel.IRow;
                        if (null != markRow) {
                            markRows.Add(markRow);
                        }
                    }
                }
                List<string> names = null;
                if (null != nameObjs) {
                    names = new List<string>();
                    foreach (var nameObj in nameObjs) {
                        names.Add(nameObj.ToString());
                    }
                }
                if (null != names && names.Count > 0) {
                    if (null != row) {
                        r = new List<int>();
                        int curIx = 0;
                        for (int i = row.FirstCellNum; i <= row.LastCellNum; ++i) {
                            var cell = row.GetCell(i);
                            if (null != cell && ResourceEditUtility.CellToString(cell).Trim() == names[curIx] && IsValidColumn(i, markChars, markRows)) {
                                r.Add(i);
                                ++curIx;
                                if (curIx >= names.Count)
                                    break;
                            }
                        }
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            r = new List<int>();
                            int curIx = 0;
                            for (int i = 0; i < trow.CellCount; ++i) {
                                var val = trow.GetCell(i);
                                if (val == names[curIx]) {
                                    r.Add(i);
                                    ++curIx;
                                    if (curIx >= names.Count)
                                        break;
                                }
                            }
                        }
                    }
                }
                else {
                    //选择所有有效的列
                    if (null != row) {
                        r = new List<int>();
                        for (int i = row.FirstCellNum; i <= row.LastCellNum; ++i) {
                            var cell = row.GetCell(i);
                            if (null != cell && IsValidColumn(i, markChars, markRows)) {
                                r.Add(i);
                            }
                        }
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            r = new List<int>();
                            for (int i = 0; i < trow.CellCount; ++i) {
                                var val = trow.GetCell(i);
                                r.Add(i);
                            }
                        }
                    }
                }
            }
            if (null == r) {
                r = new List<int>();
            }
            return r;
        }
        private static bool IsValidColumn(int ix, string markChars, IList<NPOI.SS.UserModel.IRow> markRows)
        {
            bool r = false;
            if (null != markRows) {
                for (int i = 0; i < markRows.Count; ++i) {
                    var row = markRows[i];
                    if (i < markChars.Length) {
                        var cell = row.GetCell(ix);
                        var v = ResourceEditUtility.CellToString(cell).Trim();
                        if (v.Length == 1 && v[0] == markChars[i]) {
                            r = true;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class GetCellValueExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                var ix = CastTo<int>(operands[1]);
                if (ix >= 0) {
                    if (null != row) {
                        var cell = row.GetCell(ix);
                        if (null != cell) {
                            switch (cell.CellType) {
                                case NPOI.SS.UserModel.CellType.Boolean:
                                    r = cell.BooleanCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Numeric:
                                    r = cell.NumericCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.String:
                                    r = cell.StringCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Formula:
                                    switch (cell.CachedFormulaResultType) {
                                        case NPOI.SS.UserModel.CellType.Boolean:
                                            r = cell.BooleanCellValue;
                                            break;
                                        case NPOI.SS.UserModel.CellType.Numeric:
                                            r = cell.NumericCellValue;
                                            break;
                                        case NPOI.SS.UserModel.CellType.String:
                                            r = cell.StringCellValue;
                                            break;
                                        default:
                                            r = null;
                                            break;
                                    }
                                    break;
                                case NPOI.SS.UserModel.CellType.Blank:
                                    r = string.Empty;
                                    break;
                                default:
                                    r = null;
                                    break;
                            }
                        }
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            r = trow.GetCell(ix);
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class GetCellStringExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            string r = string.Empty;
            if (operands.Count >= 2) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                var ix = CastTo<int>(operands[1]);
                if (ix >= 0) {
                    if (null != row) {
                        var cell = row.GetCell(ix);
                        r = ResourceEditUtility.CellToString(cell);
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            r = trow.GetCell(ix);
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class GetCellNumericExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            double r = 0.0;
            if (operands.Count >= 2) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                var ix = CastTo<int>(operands[1]);
                if (ix >= 0) {
                    if (null != row) {
                        var cell = row.GetCell(ix);
                        r = ResourceEditUtility.CellToNumeric(cell);
                    }
                    else {
                        var trow = operands[0] as ResourceEditUtility.DataRow;
                        if (null != trow) {
                            var v = trow.GetCell(ix);
                            double.TryParse(v, out r);
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class RowToLineExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            string r = string.Empty;
            if (operands.Count >= 1) {
                var row = operands[0] as NPOI.SS.UserModel.IRow;
                int skipCols = 0;
                List<int> colIndexes = null;
                if (operands.Count >= 2) {
                    skipCols = ToInt(operands[1]);
                }
                if (operands.Count >= 3) {
                    var colObjs = operands[2] as IList;
                    if (null != colObjs) {
                        colIndexes = new List<int>();
                        foreach (var colObj in colObjs) {
                            colIndexes.Add(ToInt(colObj));
                        }
                    }
                }
                if (null != row) {
                    string[] cols = null;
                    if (null == colIndexes) {
                        if (skipCols < row.LastCellNum - row.FirstCellNum + 1) {
                            cols = new string[row.LastCellNum - row.FirstCellNum + 1 - skipCols];
                            for (int ix = row.FirstCellNum + skipCols; ix <= row.LastCellNum; ++ix) {
                                var cell = row.GetCell(ix);
                                cols[ix - row.FirstCellNum - skipCols] = ResourceEditUtility.CellToString(cell);
                            }
                            r = string.Join(",", cols);
                        }
                    }
                    else {
                        cols = new string[colIndexes.Count];
                        int i = 0;
                        foreach (var ix in colIndexes) {
                            var cell = row.GetCell(ix);
                            cols[i++] = ResourceEditUtility.CellToString(cell);
                        }
                        r = string.Join(",", cols);
                    }
                }
                else {
                    var trow = operands[0] as ResourceEditUtility.DataRow;
                    if (null != trow) {
                        r = trow.GetLine(skipCols, colIndexes);
                    }
                }
            }
            return r;
        }
    }
    internal class TableToHashtableExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 3) {
                var sheet = operands[0] as NPOI.SS.UserModel.ISheet;
                int skipRows = ToInt(operands[1]);
                List<int> colIndexes = new List<int>();
                var colObjs = operands[2] as IList;
                if (null != colObjs) {
                    foreach (var colObj in colObjs) {
                        colIndexes.Add(ToInt(colObj));
                    }
                }
                if (colIndexes.Count > 0) {
                    if (null != sheet) {
                        var dict = new Dictionary<string, object>();
                        for (int i = sheet.FirstRowNum + skipRows; i <= sheet.LastRowNum; ++i) {
                            var temp = dict;
                            var row = sheet.GetRow(i);
                            foreach (var ix in colIndexes) {
                                var cell = row.GetCell(ix);
                                var key = ResourceEditUtility.CellToString(cell);
                                object tempObj;
                                Dictionary<string, object> temp2 = null;
                                if (!temp.TryGetValue(key, out tempObj)) {
                                    temp2 = new Dictionary<string, object>();
                                    temp.Add(key, temp2);
                                }
                                else {
                                    temp2 = tempObj as Dictionary<string, object>;
                                }
                                temp = temp2;
                            }
                            temp.Add(i.ToString(), row);
                        }
                        r = dict;
                    }
                    else {
                        var table = operands[0] as ResourceEditUtility.DataTable;
                        if (null != table) {
                            var dict = new Dictionary<string, object>();
                            for (int i = skipRows; i < table.RowCount; ++i) {
                                var temp = dict;
                                var row = table.GetRow(i);
                                foreach (var ix in colIndexes) {
                                    var key = row.GetCell(ix);
                                    object tempObj;
                                    Dictionary<string, object> temp2 = null;
                                    if (!temp.TryGetValue(key, out tempObj)) {
                                        temp2 = new Dictionary<string, object>();
                                        temp.Add(key, temp2);
                                    }
                                    else {
                                        temp2 = tempObj as Dictionary<string, object>;
                                    }
                                    temp = temp2;
                                }
                                temp.Add(i.ToString(), row);
                            }
                            r = dict;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class FindRowFromHashtableExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var hash = operands[0] as Dictionary<string, object>;
                List<string> keys = new List<string>();
                var keyObjs = operands[1] as IList;
                if (null != keyObjs) {
                    foreach (var keyObj in keyObjs) {
                        var str = keyObj as string;
                        keys.Add(str);
                    }
                }
                if (null != hash && keys.Count > 0) {
                    var temp = hash;
                    foreach (var key in keys) {
                        object tempObj;
                        if (temp.TryGetValue(key, out tempObj)) {
                            temp = tempObj as Dictionary<string, object>;
                        }
                        else {
                            temp = null;
                            break;
                        }
                    }
                    if (null != temp) {
                        foreach (var pair in temp) {
                            r = pair.Value;
                            break;
                        }
                    }
                    else {
                        r = null;
                    }
                }
            }
            return r;
        }
    }
    internal class LoadManagedHeapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                var list = new List<ResourceEditUtility.SectionInfo>();
                var lines = File.ReadAllLines(file);
                foreach (var line in lines) {
                    var mapsInfo = new ResourceEditUtility.SectionInfo();
                    var fields = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields[0] == "Managed") {
                        ulong size = ulong.Parse(fields[1]);
                        ulong start = ulong.Parse(fields[2]);
                        mapsInfo.size = size;
                        mapsInfo.vm_start = start;
                        mapsInfo.vm_end = start + size;
                        list.Add(mapsInfo);
                    }
                }
                list.Sort((a, b) => {
                    if (a.vm_start < b.vm_start)
                        return -1;
                    else if (a.vm_start > b.vm_start)
                        return 1;
                    else if (a.vm_end < b.vm_end)
                        return -1;
                    else if (a.vm_end > b.vm_end)
                        return 1;
                    else
                        return 0;
                });
                r = list;
            }
            return r;
        }
    }
    internal class FindManagedHeapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as List<ResourceEditUtility.SectionInfo>;
                var addr = (ulong)Convert.ChangeType(operands[1], typeof(ulong));
                if (null != list && addr > 0) {
                    var low = 0;
                    var high = list.Count - 1;
                    while (low <= high) {
                        var cur = (low + high) / 2;
                        var vs = list[cur].vm_start;
                        var ve = list[cur].vm_end;
                        if (addr < vs)
                            high = cur - 1;
                        else if (addr >= ve)
                            low = cur + 1;
                        else {
                            r = list[cur];
                            break;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class LoadMapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                var list = new List<ResourceEditUtility.MapsInfo>();
                var lines = File.ReadAllLines(file);
                foreach (var line in lines) {
                    var mapsInfo = new ResourceEditUtility.MapsInfo();
                    var fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var se = fields[0].Split('-');
                    ulong start = ulong.Parse(se[0], System.Globalization.NumberStyles.AllowHexSpecifier);
                    ulong end = ulong.Parse(se[1], System.Globalization.NumberStyles.AllowHexSpecifier);
                    mapsInfo.vm_start = start;
                    mapsInfo.vm_end = end;
                    for (int i = 0; i < fields.Length; ++i) {
                        switch (i) {
                            case 1:
                                mapsInfo.flags = fields[i];
                                break;
                            case 2:
                                mapsInfo.offset = fields[i];
                                break;
                            case 3:
                                mapsInfo.file1 = fields[i];
                                break;
                            case 4:
                                mapsInfo.file2 = fields[i];
                                break;
                            case 5:
                                mapsInfo.module = fields[i];
                                break;
                        }
                    }
                    mapsInfo.size += mapsInfo.vm_end - mapsInfo.vm_start;
                    list.Add(mapsInfo);
                }
                list.Sort((a, b) => {
                    if (a.vm_start < b.vm_start)
                        return -1;
                    else if (a.vm_start > b.vm_start)
                        return 1;
                    else if (a.vm_end < b.vm_end)
                        return -1;
                    else if (a.vm_end > b.vm_end)
                        return 1;
                    else
                        return 0;
                });
                r = list;
            }
            return r;
        }
    }
    internal class FindMapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as List<ResourceEditUtility.MapsInfo>;
                var addr = (ulong)Convert.ChangeType(operands[1], typeof(ulong));
                if(null!=list && addr > 0) {
                    var low = 0;
                    var high = list.Count - 1;
                    while (low <= high) {
                        var cur = (low + high) / 2;
                        var vs = list[cur].vm_start;
                        var ve = list[cur].vm_end;
                        if (addr < vs)
                            high = cur - 1;
                        else if (addr >= ve)
                            low = cur + 1;
                        else {
                            r = list[cur];
                            break;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal class LoadSmapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var file = operands[0] as string;
                var list = new List<ResourceEditUtility.SmapsInfo>();
                ResourceEditUtility.SmapsInfo curInfo = null;
                var lines = File.ReadAllLines(file);
                foreach (var line in lines) {
                    var mapsInfo = new ResourceEditUtility.SmapsInfo();
                    var fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields[0].IndexOf('-') > 0) {
                        var se = fields[0].Split('-');
                        ulong start = ulong.Parse(se[0], System.Globalization.NumberStyles.AllowHexSpecifier);
                        ulong end = ulong.Parse(se[1], System.Globalization.NumberStyles.AllowHexSpecifier);
                        mapsInfo.vm_start = start;
                        mapsInfo.vm_end = end;
                        for (int i = 0; i < fields.Length; ++i) {
                            switch (i) {
                                case 1:
                                    mapsInfo.flags = fields[i];
                                    break;
                                case 2:
                                    mapsInfo.offset = fields[i];
                                    break;
                                case 3:
                                    mapsInfo.file1 = fields[i];
                                    break;
                                case 4:
                                    mapsInfo.file2 = fields[i];
                                    break;
                                case 5:
                                    mapsInfo.module = fields[i];
                                    break;
                            }
                        }
                        mapsInfo.size += mapsInfo.vm_end - mapsInfo.vm_start;

                        curInfo = mapsInfo;
                    }
                    else {
                        var key = fields[0];
                        var val = fields[1];
                        if (key == "Size:") {
                            curInfo.sizeKB = ulong.Parse(val);
                        }
                        else if (key == "Rss:") {
                            curInfo.rss = ulong.Parse(val);
                        }
                        else if (key == "Pss:") {
                            curInfo.pss = ulong.Parse(val);
                        }
                        else if (key == "Shared_Clean:") {
                            curInfo.shared_clean = ulong.Parse(val);
                        }
                        else if (key == "Shared_Dirty:") {
                            curInfo.shared_dirty = ulong.Parse(val);
                        }
                        else if (key == "Private_Clean:") {
                            curInfo.private_clean = ulong.Parse(val);
                        }
                        else if (key == "Private_Dirty:") {
                            curInfo.private_dirty = ulong.Parse(val);
                        }
                        else if (key == "Referenced:") {
                            curInfo.referenced = ulong.Parse(val);
                        }
                        else if (key == "Anonymous:") {
                            curInfo.anonymous = ulong.Parse(val);
                        }
                        else if (key == "Swap:") {
                            curInfo.swap = ulong.Parse(val);
                        }
                        else if (key == "SwapPss:") {
                            curInfo.swappss = ulong.Parse(val);
                        }
                    }
                    list.Add(mapsInfo);
                }
                list.Sort((a, b) => {
                    if (a.vm_start < b.vm_start)
                        return -1;
                    else if (a.vm_start > b.vm_start)
                        return 1;
                    else if (a.vm_end < b.vm_end)
                        return -1;
                    else if (a.vm_end > b.vm_end)
                        return 1;
                    else
                        return 0;
                });
                r = list;
            }
            return r;
        }
    }
    internal class FindSmapsExp : DslExpression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 2) {
                var list = operands[0] as List<ResourceEditUtility.SmapsInfo>;
                var addr = (ulong)Convert.ChangeType(operands[1], typeof(ulong));
                if (null != list && addr > 0) {
                    var low = 0;
                    var high = list.Count - 1;
                    while (low <= high) {
                        var cur = (low + high) / 2;
                        var vs = list[cur].vm_start;
                        var ve = list[cur].vm_end;
                        if (addr < vs)
                            high = cur - 1;
                        else if (addr >= ve)
                            low = cur + 1;
                        else {
                            r = list[cur];
                            break;
                        }
                    }
                }
            }
            return r;
        }
    }
}
#endregion

#region MeshArea
internal static class MeshAreaHelper
{
    internal static float Area(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 dirA = b - a;
        Vector3 dirB = c - a;
        return Vector3.Cross(dirA, dirB).magnitude / 2;
    }
    internal static float Area(Vector2 a, Vector2 b, Vector2 c)
    {
        Vector2 dirA = b - a;
        Vector2 dirB = c - a;
        return Vector3.Cross(new Vector3(dirA.x, dirA.y, 0), new Vector3(dirB.x, dirB.y, 0)).magnitude / 2;
    }

    internal static List<Vector2> TextureSize(GameObject go)
    {
        List<Vector2> textureSize = new List<Vector2>();

        Renderer r = go.GetComponent<Renderer>();
        if (r != null) {
            Material[] materials = r.sharedMaterials;
            if (materials != null) {
                for (int i = 0; i < materials.Length; i++) {
                    var m = materials[i];
                    if (null != m) {
                        var tex = m.mainTexture;
                        if (null != tex) {
                            textureSize.Add(new Vector2(tex.width, tex.height));
                        }
                        else {
                            textureSize.Add(Vector2.one);
                        }
                    }
                    else {
                        textureSize.Add(Vector2.one);
                    }
                }
            }
        }
        return textureSize;
    }
    internal static List<Vector2> GetLightMapSize(int index)
    {
        List<Vector2> lightMapSize = new List<Vector2>(3);
        lightMapSize[0] = Vector2.one;
        lightMapSize[1] = Vector2.one;
        lightMapSize[2] = Vector2.one;

        LightmapData[] datas = LightmapSettings.lightmaps;
        if (datas != null && datas[index] != null) {
            if (datas[index].lightmapColor != null) {
                var tex = datas[index].lightmapColor;
                lightMapSize[0] = new Vector2(tex.width, tex.height);
            }
            if (datas[index].lightmapDir != null) {
                var tex = datas[index].lightmapDir;
                lightMapSize[1] = new Vector2(tex.width, tex.height);
            }
            if (datas[index].shadowMask != null) {
                var tex = datas[index].shadowMask;
                lightMapSize[2] = new Vector2(tex.width, tex.height);
            }
        }

        return lightMapSize;
    }
    internal static List<float> CalculateMeshAreaWorld(GameObject go)
    {
        List<float> subMeshArea = new List<float>();
        Mesh mesh = GetMesh(go);
        if (mesh != null) {
            Vector3[] vertData = mesh.vertices;
            int submeshCount = mesh.subMeshCount;
            for (int i = 0; i < submeshCount; i++) {
                float subMeshArea_i = 0;
                int[] subIndex = mesh.GetIndices(i);
                for (int j = 0; j < subIndex.Length / 3; j++) {
                    int indexA = subIndex[j * 3 + 0];
                    int indexB = subIndex[j * 3 + 1];
                    int indexC = subIndex[j * 3 + 2];

                    Vector3 vertexA = go.transform.TransformPoint(vertData[indexA]);
                    Vector3 vertexB = go.transform.TransformPoint(vertData[indexB]);
                    Vector3 vertexC = go.transform.TransformPoint(vertData[indexC]);

                    subMeshArea_i += Area(vertexA, vertexB, vertexC);
                }
                subMeshArea.Add(subMeshArea_i);
            }
        }
        return subMeshArea;
    }
    internal static List<float> CalculateMeshUVAreaObject(GameObject go)
    {
        List<float> subMeshArea = new List<float>();
        Mesh mesh = GetMesh(go);
        if (mesh != null) {
            Vector2[] uvData = mesh.uv;
            int submeshCount = mesh.subMeshCount;
            for (int i = 0; i < submeshCount; i++) {
                float subMeshArea_i = 0;
                int[] subIndex = mesh.GetIndices(i);
                for (int j = 0; j < subIndex.Length / 3; j++) {
                    int indexA = subIndex[j * 3 + 0];
                    int indexB = subIndex[j * 3 + 1];
                    int indexC = subIndex[j * 3 + 2];

                    Vector2 vertexA = uvData[indexA];
                    Vector2 vertexB = uvData[indexB];
                    Vector2 vertexC = uvData[indexC];

                    subMeshArea_i += Area(vertexA, vertexB, vertexC);
                }
                subMeshArea.Add(subMeshArea_i);
            }
        }
        return subMeshArea;
    }
    internal static List<float> CalculateMeshUVAreaTile(GameObject go)
    {
        List<float> subMeshArea = new List<float>();
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer == null) return subMeshArea;
        Material[] materials = renderer.sharedMaterials;
        if (materials == null) return subMeshArea;
        Mesh mesh = GetMesh(go);
        if (mesh != null) {
            Vector2[] uvData = mesh.uv;
            int submeshCount = mesh.subMeshCount;
            Material m = null;
            for (int i = 0; i < submeshCount; i++) {
                if (i < materials.Length)
                    m = materials[i];
                float subMeshArea_i = 0;
                if (null != m) {
                    int[] subIndex = mesh.GetIndices(i);
                    for (int j = 0; j < subIndex.Length / 3; j++) {
                        int indexA = subIndex[j * 3 + 0];
                        int indexB = subIndex[j * 3 + 1];
                        int indexC = subIndex[j * 3 + 2];

                        Vector2 vertexA = new Vector2(uvData[indexA].x * m.mainTextureScale.x, uvData[indexA].y * m.mainTextureScale.y);
                        Vector2 vertexB = new Vector2(uvData[indexB].x * m.mainTextureScale.x, uvData[indexB].y * m.mainTextureScale.y); //uvData[indexB];
                        Vector2 vertexC = new Vector2(uvData[indexC].x * m.mainTextureScale.x, uvData[indexC].y * m.mainTextureScale.y); //uvData[indexC];

                        subMeshArea_i += Area(vertexA, vertexB, vertexC);
                    }
                }
                subMeshArea.Add(subMeshArea_i);
            }
        }
        return subMeshArea;
    }
    internal static List<float> CalculateMeshLightmapUVArea(GameObject go)
    {
        List<float> subMeshArea = new List<float>();
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer == null) { return subMeshArea; }
        Vector4 lightMapScaleOffset = renderer.lightmapScaleOffset;
        Mesh mesh = GetMesh(go);
        if (mesh != null) {
            Vector2[] uvData = mesh.uv2;
            int submeshCount = mesh.subMeshCount;
            for (int i = 0; i < submeshCount; i++) {
                float subMeshArea_i = 0;
                int[] subIndex = mesh.GetIndices(i);
                for (int j = 0; j < subIndex.Length / 3; j++) {
                    int indexA = subIndex[j * 3 + 0];
                    int indexB = subIndex[j * 3 + 1];
                    int indexC = subIndex[j * 3 + 2];

                    Vector2 vertexA = new Vector2(lightMapScaleOffset.x * uvData[indexA].x, lightMapScaleOffset.y * uvData[indexA].y);
                    Vector2 vertexB = new Vector2(lightMapScaleOffset.x * uvData[indexB].x, lightMapScaleOffset.y * uvData[indexB].y);
                    Vector2 vertexC = new Vector2(lightMapScaleOffset.x * uvData[indexC].x, lightMapScaleOffset.y * uvData[indexC].y);

                    subMeshArea_i += Area(vertexA, vertexB, vertexC);
                }
                subMeshArea.Add(subMeshArea_i);
            }
        }
        return subMeshArea;
    }

    private static Mesh GetMesh(GameObject go)
    {
        var mf = go.GetComponent<MeshFilter>();
        if (mf != null) {
            return mf.sharedMesh;
        }
        else {
            var skin = go.GetComponent<SkinnedMeshRenderer>();
            if (null != skin) {
                return skin.sharedMesh;
            }
            else {
                return null;
            }
        }
    }
}
#endregion

#region MemoryProfiler
class ShortestPathToRootObjectFinder
{
    private readonly CachedSnapshot _snapshot;
    private Dictionary<int, HashSet<ObjectData>> _refbydict;

    public ShortestPathToRootObjectFinder(CachedSnapshot snapshot)
    {
        _snapshot = snapshot;
        _refbydict = new Dictionary<int, HashSet<ObjectData>>();

        int ct = snapshot.CrawledData.Connections.Count;
        for (int i = 0; i < ct; ++i) {
            var c = snapshot.CrawledData.Connections[i];
            int objIndex = c.GetUnifiedIndexTo(snapshot);
            ObjectData objData = ObjectData.invalid;
            switch (c.connectionType) {
                case ManagedConnection.ConnectionType.Global_To_ManagedObject:
                    objData = ObjectData.global;
                    break;
                case ManagedConnection.ConnectionType.ManagedObject_To_ManagedObject:
                    var objParent = ObjectData.FromManagedObjectIndex(snapshot, c.fromManagedObjectIndex);
                    if (c.fieldFrom >= 0) {
                        if (objParent.dataType == ObjectDataType.Array || objParent.dataType == ObjectDataType.ReferenceArray) {
                            objData = objParent;
                        }
                        else {
                            objData = objParent.GetInstanceFieldBySnapshotFieldIndex(snapshot, c.fieldFrom, false);
                        }
                    }
                    else if (c.arrayIndexFrom >= 0) {
                        objData = objParent.GetArrayElement(snapshot, c.arrayIndexFrom, false);
                    }
                    else {
                        objData = objParent;
                    }
                    break;
                case ManagedConnection.ConnectionType.ManagedType_To_ManagedObject:
                    var objType = ObjectData.FromManagedType(snapshot, c.fromManagedType);
                    if (c.fieldFrom >= 0) {
                        objData = objType.GetInstanceFieldBySnapshotFieldIndex(snapshot, c.fieldFrom, false);
                    }
                    else if (c.arrayIndexFrom >= 0) {
                        objData = objType.GetArrayElement(snapshot, c.arrayIndexFrom, false);
                    }
                    else {
                        objData = objType;
                    }
                    break;
                case ManagedConnection.ConnectionType.UnityEngineObject:
                    objData = ObjectData.FromNativeObjectIndex(snapshot, c.UnityEngineNativeObjectIndex);
                    break;
            }
            TryAddRefBy(objIndex, ref objData);
            //考虑managed->native情形
            if(c.connectionType== ManagedConnection.ConnectionType.UnityEngineObject) {
                objIndex = _snapshot.NativeObjectIndexToUnifiedObjectIndex(c.UnityEngineNativeObjectIndex);
                objData = ObjectData.FromManagedObjectIndex(snapshot, c.UnityEngineManagedObjectIndex);
                TryAddRefBy(objIndex, ref objData);
            }
            if (i % 1000 == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("build reference by dictionary", i, ct)) {
                break;
            }
        }
        //从cache里检索一遍
        ct = (int)snapshot.connections.Count;
        for (int i = 0; i < ct; ++i) {
            int objIndex = snapshot.connections.to[i];
            var objData = ObjectData.FromUnifiedObjectIndex(snapshot, snapshot.connections.from[i]);
            TryAddRefBy(objIndex, ref objData);
            if (i % 1000 == 0 && ResourceProcessor.Instance.DisplayCancelableProgressBar("build reference by dictionary", i, ct)) {
                break;
            }
        }
        EditorUtility.ClearProgressBar();
    }

    public ObjectData[] FindFor(ObjectData data)
    {
        var seen = new HashSet<ObjectData>();
        var queue = new Queue<List<ObjectData>>();
        queue.Enqueue(new List<ObjectData> { data });

        ObjectData[] ret = null;
        while (queue.Any()) {
            var pop = queue.Dequeue();
            var obj = pop.Last();
            var subObj = obj.displayObject;

            string reason;
            if (IsRoot(obj, out reason)) {
                ret = pop.ToArray();
                break;
            }

            //var refBys = ObjectConnection.GetAllObjectConnectingTo(_snapshot, subObj);
            HashSet<ObjectData> refBys;
            if(_refbydict.TryGetValue(subObj.GetUnifiedObjectIndex(_snapshot), out refBys)) {
                foreach (var next in refBys) {
                    if (seen.Contains(next))
                        continue;
                    seen.Add(next);
                    var dupe = new List<ObjectData>(pop) { next };
                    queue.Enqueue(dupe);
                }
            }
            if (ResourceProcessor.Instance.DisplayCancelableProgressBar("find shortest path to root", queue.Count, 10000)) {
                ret = pop.ToArray();
                break;
            }
        }
        EditorUtility.ClearProgressBar();
        return ret;
    }
    public HashSet<ObjectData> GetReferenceByHash(ObjectData data)
    {
        HashSet<ObjectData> hash;
        int index = data.GetUnifiedObjectIndex(_snapshot);
        _refbydict.TryGetValue(index, out hash);
        return hash;
    }
    public ObjectData[] GetReferenceByList(ObjectData data)
    {
        return ObjectConnection.GetAllObjectConnectingTo(_snapshot, data);
    }
    public bool IsRoot(ObjectData data, out string reason)
    {
        reason = null;
        if (data.IsValid) {
            bool isStatic = false;
            if (data.IsField()) {
                if(data.fieldIndex>=0 && data.fieldIndex < _snapshot.fieldDescriptions.Count) {
                    isStatic = _snapshot.fieldDescriptions.isStatic[data.fieldIndex];
                }
            }
            if (isStatic || data.dataType == ObjectDataType.Global || data.dataType == ObjectDataType.Type) {
                reason = "Static fields are global variables. Anything they reference will not be unloaded.";
                return true;
            }
            if (data.isManaged)
                return false;

            var classID = _snapshot.nativeObjects.nativeTypeArrayIndex[data.nativeObjectIndex];
            var flags = _snapshot.nativeObjects.flags[data.nativeObjectIndex];
            var hideFlags = _snapshot.nativeObjects.hideFlags[data.nativeObjectIndex];

            if ((flags & ObjectFlags.IsPersistent) != 0)
                return false;
            if ((flags & ObjectFlags.IsManager) != 0) {
                reason = "this is an internal unity'manager' style object, which is a global object that will never be unloaded";
                return true;
            }
            if ((flags & ObjectFlags.IsDontDestroyOnLoad) != 0) {
                reason = "DontDestroyOnLoad() was called on this object, so it will never be unloaded";
                return true;
            }

            if ((hideFlags & HideFlags.DontUnloadUnusedAsset) != 0) {
                reason = "the DontUnloadUnusedAsset hideflag is set on this object. Unity's builtin resources set this flag. Users can also set the flag themselves";
                return true;
            }

            if (IsComponent(classID)) {
                reason = "this is a component, living on a gameobject, that is either part of the loaded scene, or was generated by script. It will be unloaded on next scene load.";
                return true;
            }
            if (IsGameObject(classID)) {
                reason = "this is a gameobject, that is either part of the loaded scene, or was generated by script. It will be unloaded on next scene load if nobody is referencing it";
                return true;
            }
            if (IsAssetBundle(classID)) {
                reason = "this object is an assetbundle, which is never unloaded automatically, but only through an explicit .Unload() call.";
                return true;
            }
        }
        reason = "This object is a root, but the memory profiler UI does not yet understand why";
        return true;
    }

    private bool IsGameObject(int classID)
    {
        return _snapshot.nativeTypes.typeName[classID] == "GameObject";
    }

    private bool IsAssetBundle(int classID)
    {
        return _snapshot.nativeTypes.typeName[classID] == "AssetBundle";
    }

    private bool IsComponent(int classID)
    {
        var typeName = _snapshot.nativeTypes.typeName[classID];
        var nativeBaseTypeArrayIndex = _snapshot.nativeTypes.nativeBaseTypeArrayIndex[classID];

        if (typeName == "Component")
            return true;

        var baseClassID = nativeBaseTypeArrayIndex;

        return baseClassID != -1 && IsComponent(baseClassID);
    }

    private void TryAddRefBy(int objIndex, ref ObjectData objData)
    {
        if (objIndex >= 0) {
            HashSet<ObjectData> hash;
            if (!_refbydict.TryGetValue(objIndex, out hash)) {
                hash = new HashSet<ObjectData>();
                _refbydict.Add(objIndex, hash);
            }
            if (!hash.Contains(objData)) {
                hash.Add(objData);
            }
        }
    }
}
#endregion