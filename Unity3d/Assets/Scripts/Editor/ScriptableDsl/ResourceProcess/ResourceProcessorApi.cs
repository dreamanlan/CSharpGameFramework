using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.MemoryProfiler;
using UnityEditorInternal;
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
        internal string Encoding;
        internal object MinValue;
        internal object MaxValue;
        internal List<string> OptionNames = new List<string>();
        internal Dictionary<string, string> Options = new Dictionary<string, string>();
        internal string OptionStyle = string.Empty;
        internal string FileExts = string.Empty;
        internal string FileInitDir = string.Empty;
        //temp
        internal string[] PopupOptionNames = null;
        internal IList<string> MultipleOldValues = null;
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
        internal IList<string> ExtraList = null;
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
        internal IList<string> ExtraList = null;
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
        internal int refOtherCount;
        internal MemoryProfilerWindowForExtension.ThingInMemory memoryObject;
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
    internal static void InitCalculator(Expression.DslCalculator calc)
    {
        calc.Init();
        calc.Register("callscript", new Expression.ExpressionFactoryHelper<ResourceEditApi.CallScriptExp>());
        calc.Register("setredirect", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetRedirectExp>());
        calc.Register("newitem", new Expression.ExpressionFactoryHelper<ResourceEditApi.NewItemExp>());
        calc.Register("getreferenceassets", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceAssetsExp>());
        calc.Register("getreferencebyassets", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetReferenceByAssetsExp>());
        calc.Register("calcrefcount", new Expression.ExpressionFactoryHelper<ResourceEditApi.CalcRefCountExp>());
        calc.Register("calcrefbycount", new Expression.ExpressionFactoryHelper<ResourceEditApi.CalcRefByCountExp>());
        calc.Register("findasset", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindAssetExp>());
        calc.Register("findshortestpathtoroot", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindShortestPathToRootExp>());
        calc.Register("saveandreimport", new Expression.ExpressionFactoryHelper<ResourceEditApi.SaveAndReimportExp>());
        calc.Register("setdirty", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetDirtyExp>());
        calc.Register("getdefaulttexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetDefaultTextureSettingExp>());
        calc.Register("gettexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetTextureSettingExp>());
        calc.Register("settexturesetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetTextureSettingExp>());
        calc.Register("isastctexture", new Expression.ExpressionFactoryHelper<ResourceEditApi.IsAstcTextureExp>());
        calc.Register("setastctexture", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAstcTextureExp>());
        calc.Register("istexturenoalphasource", new Expression.ExpressionFactoryHelper<ResourceEditApi.IsTextureNoAlphaSourceExp>());
        calc.Register("doestexturehavealpha", new Expression.ExpressionFactoryHelper<ResourceEditApi.DoesTextureHaveAlphaExp>());
        calc.Register("correctnonealphatexture", new Expression.ExpressionFactoryHelper<ResourceEditApi.CorrectNoneAlphaTextureExp>());
        calc.Register("setnonealphatexture", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetNoneAlphaTextureExp>());
        calc.Register("gettexturecompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetTextureCompressionExp>());
        calc.Register("settexturecompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetTextureCompressionExp>());
        calc.Register("getmeshcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetMeshCompressionExp>());
        calc.Register("setmeshcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetMeshCompressionExp>());
        calc.Register("setmeshimportexternalmaterials", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetMeshImportExternalMaterialsExp>());
        calc.Register("setmeshimportinprefabmaterials", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetMeshImportInPrefabMaterialsExp>());
        calc.Register("closemeshanimationifnoanimation", new Expression.ExpressionFactoryHelper<ResourceEditApi.CloseMeshAnimationIfNoAnimationExp>());
        calc.Register("collectmeshinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.CollectMeshInfoExp>());
        calc.Register("collectanimatorcontrollerinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.CollectAnimatorControllerInfoExp>());
        calc.Register("collectprefabinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.CollectPrefabInfoExp>());
        calc.Register("getanimationclipinfo", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationClipInfoExp>());
        calc.Register("getanimationcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationCompressionExp>());
        calc.Register("setanimationcompression", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationCompressionExp>());
        calc.Register("getanimationtype", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAnimationTypeExp>());
        calc.Register("setanimationtype", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAnimationTypeExp>());
        calc.Register("setExtraExposedTransformPaths", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetExtraExposedTransformPathsExp>());
        calc.Register("clearanimationscalecurve", new Expression.ExpressionFactoryHelper<ResourceEditApi.ClearAnimationScaleCurveExp>());
        calc.Register("getaudiosetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetAudioSettingExp>());
        calc.Register("setaudiosetting", new Expression.ExpressionFactoryHelper<ResourceEditApi.SetAudioSettingExp>());
        calc.Register("calcmeshtexratio", new Expression.ExpressionFactoryHelper<ResourceEditApi.CalcMeshTexRatioExp>());
        calc.Register("calcassetmd5", new Expression.ExpressionFactoryHelper<ResourceEditApi.CalcAssetMd5Exp>());
        calc.Register("calcassetsize", new Expression.ExpressionFactoryHelper<ResourceEditApi.CalcAssetSizeExp>());
        calc.Register("deleteasset", new Expression.ExpressionFactoryHelper<ResourceEditApi.DeleteAssetExp>());
        calc.Register("getshaderutil", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetShaderUtilExp>());
        calc.Register("getshaderpropertycount", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetShaderPropertyCountExp>());
        calc.Register("getshaderpropertynames", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetShaderPropertyNamesExp>());
        calc.Register("getshadervariants", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetShaderVariantsExp>());
        calc.Register("addshadertocollection", new Expression.ExpressionFactoryHelper<ResourceEditApi.AddShaderToCollectionExp>());
        calc.Register("findrowindex", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindRowIndexExp>());
        calc.Register("findrowindexes", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindRowIndexesExp>());
        calc.Register("findcellindex", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindCellIndexExp>());
        calc.Register("findcellindexes", new Expression.ExpressionFactoryHelper<ResourceEditApi.FindCellIndexesExp>());
        calc.Register("getcellvalue", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetCellValueExp>());
        calc.Register("getcellstring", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetCellStringExp>());
        calc.Register("getcellnumeric", new Expression.ExpressionFactoryHelper<ResourceEditApi.GetCellNumericExp>());
        calc.Register("rowtoline", new Expression.ExpressionFactoryHelper<ResourceEditApi.RowToLineExp>());
    }
    internal static object Filter(ItemInfo item, Dictionary<string, object> addVars, List<ItemInfo> results, Expression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
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
                        var strList = new List<string>();
                        foreach (var str in list) {
                            strList.Add(str.ToString());
                        }
                        item.ExtraList = strList;
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
    internal static object Process(ItemInfo item, Expression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
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
    internal static object Group(GroupInfo item, Expression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
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
                        var strList = new List<string>();
                        foreach (var str in list) {
                            strList.Add(str.ToString());
                        }
                        item.ExtraList = strList;
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
    internal static object GroupProcess(GroupInfo item, Expression.DslCalculator calc, int indexCount, Dictionary<string, ParamInfo> args, SceneDepInfo sceneDeps, Dictionary<string, HashSet<string>> refDict, Dictionary<string, HashSet<string>> refByDict)
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
    private static Expression.DslCalculator GetResourceParamsCalculator()
    {
        if (null == s_ResourceParamsCalculator && File.Exists(s_ResourceParamsDslFile)) {
            s_ResourceParamsCalculator = new Expression.DslCalculator();
            InitCalculator(s_ResourceParamsCalculator);
            s_ResourceParamsCalculator.LoadDsl(s_ResourceParamsDslFile);
        }
        return s_ResourceParamsCalculator;
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

    private static Expression.DslCalculator s_ResourceParamsCalculator = null;
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
    internal class CallScriptExp : Expression.SimpleExpressionBase
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
    internal class SetRedirectExp : Expression.SimpleExpressionBase
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
    internal class NewItemExp : Expression.SimpleExpressionBase
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
    internal class GetReferenceAssetsExp : Expression.SimpleExpressionBase
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
    internal class GetReferenceByAssetsExp : Expression.SimpleExpressionBase
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
    internal class CalcRefCountExp : Expression.SimpleExpressionBase
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
    internal class CalcRefByCountExp : Expression.SimpleExpressionBase
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
    internal class FindAssetExp : Expression.SimpleExpressionBase
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
    internal class FindShortestPathToRootExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = string.Empty;
            if (operands.Count >= 1) {
                var obj = operands[0] as MemoryProfilerWindowForExtension.ThingInMemory;
                if (null != obj) {
                    r = ResourceProcessor.Instance.FindShortestPathToRoot(obj);
                }
            }
            return r;
        }
    }
    internal class SaveAndReimportExp : Expression.SimpleExpressionBase
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
    internal class SetDirtyExp : Expression.SimpleExpressionBase
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
    internal class GetDefaultTextureSettingExp : Expression.SimpleExpressionBase
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
    internal class GetTextureSettingExp : Expression.SimpleExpressionBase
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
    internal class SetTextureSettingExp : Expression.SimpleExpressionBase
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
    internal class IsAstcTextureExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = 0;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 6;
                if (operands.Count >= 2) {
                    sizeNoAlpha = (int)Convert.ChangeType(operands[1], typeof(int));//4、5、6、8、10、12
                    sizeAlpha = sizeNoAlpha;
                }
                if (operands.Count >= 3) {
                    sizeAlpha = (int)Convert.ChangeType(operands[2], typeof(int));//4、5、6、8、10、12
                }
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
    internal class SetAstcTextureExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = null;
            if (operands.Count >= 1) {
                var importer = Calculator.GetVariable("importer") as TextureImporter;
                var setting = operands[0] as TextureImporterPlatformSettings;
                var sizeNoAlpha = 8;
                var sizeAlpha = 6;
                if (operands.Count >= 2) {
                    sizeNoAlpha = (int)Convert.ChangeType(operands[1], typeof(int));//4、5、6、8、10、12
                    sizeAlpha = sizeNoAlpha;
                }
                if (operands.Count >= 3) {
                    sizeAlpha = (int)Convert.ChangeType(operands[2], typeof(int));//4、5、6、8、10、12
                }
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
    }
    internal class IsTextureNoAlphaSourceExp : Expression.SimpleExpressionBase
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
    internal class DoesTextureHaveAlphaExp : Expression.SimpleExpressionBase
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
    internal class CorrectNoneAlphaTextureExp : Expression.SimpleExpressionBase
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
    internal class SetNoneAlphaTextureExp : Expression.SimpleExpressionBase
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
    internal class GetTextureCompressionExp : Expression.SimpleExpressionBase
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
    internal class SetTextureCompressionExp : Expression.SimpleExpressionBase
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
    internal class GetMeshCompressionExp : Expression.SimpleExpressionBase
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
    internal class SetMeshCompressionExp : Expression.SimpleExpressionBase
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
    internal class SetMeshImportExternalMaterialsExp : Expression.SimpleExpressionBase
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
    internal class SetMeshImportInPrefabMaterialsExp : Expression.SimpleExpressionBase
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
    internal class CloseMeshAnimationIfNoAnimationExp : Expression.SimpleExpressionBase
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
    internal class CollectMeshInfoExp : Expression.SimpleExpressionBase
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
    internal class CollectAnimatorControllerInfoExp : Expression.SimpleExpressionBase
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
    internal class CollectPrefabInfoExp : Expression.SimpleExpressionBase
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
    internal class GetAnimationClipInfoExp : Expression.SimpleExpressionBase
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
    internal class GetAnimationCompressionExp : Expression.SimpleExpressionBase
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
    internal class SetAnimationCompressionExp : Expression.SimpleExpressionBase
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
    internal class GetAnimationTypeExp : Expression.SimpleExpressionBase
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
    internal class SetAnimationTypeExp : Expression.SimpleExpressionBase
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
    internal class SetExtraExposedTransformPathsExp : Expression.SimpleExpressionBase
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
    internal class ClearAnimationScaleCurveExp : Expression.SimpleExpressionBase
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
    internal class GetAudioSettingExp : Expression.SimpleExpressionBase
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
    internal class SetAudioSettingExp : Expression.SimpleExpressionBase
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
    internal class CalcMeshTexRatioExp : Expression.SimpleExpressionBase
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
    internal class CalcAssetMd5Exp : Expression.SimpleExpressionBase
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
    internal class CalcAssetSizeExp : Expression.SimpleExpressionBase
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
    internal class DeleteAssetExp : Expression.SimpleExpressionBase
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
    internal class GetShaderUtilExp : Expression.SimpleExpressionBase
    {
        protected override object OnCalc(IList<object> operands)
        {
            object r = typeof(ShaderUtil);
            return r;
        }
    }
    internal class GetShaderPropertyCountExp : Expression.SimpleExpressionBase
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
    internal class GetShaderPropertyNamesExp : Expression.SimpleExpressionBase
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
    internal class GetShaderVariantsExp : Expression.SimpleExpressionBase
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
    internal class AddShaderToCollectionExp : Expression.SimpleExpressionBase
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
    internal class FindRowIndexExp : Expression.SimpleExpressionBase
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
    internal class FindRowIndexesExp : Expression.SimpleExpressionBase
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
    internal class FindCellIndexExp : Expression.SimpleExpressionBase
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
    internal class FindCellIndexesExp : Expression.SimpleExpressionBase
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
    internal class GetCellValueExp : Expression.SimpleExpressionBase
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
    internal class GetCellStringExp : Expression.SimpleExpressionBase
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
    internal class GetCellNumericExp : Expression.SimpleExpressionBase
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
    internal class RowToLineExp : Expression.SimpleExpressionBase
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
internal class MemoryObjectDrawer
{
    internal MemoryObjectDrawer(MemoryProfilerWindowForExtension.CrawledMemorySnapshot unpackedCrawl)
    {
        s_UnpackedCrawl = unpackedCrawl;
        s_PrimitiveValueReader = new MemoryProfilerWindowForExtension.PrimitiveValueReader(s_UnpackedCrawl.virtualMachineInformation, s_UnpackedCrawl.managedHeap);
    }
    internal string DrawThing(MemoryProfilerWindowForExtension.ThingInMemory thing)
    {
        StringBuilder sb = new StringBuilder();
        var managedObject = thing as MemoryProfilerWindowForExtension.ManagedObject;
        if (managedObject != null) {
            sb.AppendLine("[ManagedObject]");
            if (managedObject.typeDescription.name == "System.String") {
                sb.AppendLine("--string--");
                sb.AppendLine(ReadString(managedObject.address));
            }
            else if (managedObject.typeDescription.isArray) {
                sb.AppendLine("--array--");
                sb.AppendLine(DrawArray(managedObject));
            }
            else {
                sb.AppendLine("--fields--");
                sb.AppendLine(DrawFields(managedObject));
            }
        }

        if (thing is MemoryProfilerWindowForExtension.GCHandle) {
            sb.AppendLine("[GCHandle]");
            sb.Append("size: ");
            sb.AppendLine(thing.size.ToString());
        }

        var staticFields = thing as MemoryProfilerWindowForExtension.StaticFields;
        if (staticFields != null) {
            sb.Append("[Static Fields Of type: ");
            sb.Append(staticFields.typeDescription.name);
            sb.AppendLine("]");
            sb.Append("size: ");
            sb.AppendLine(staticFields.size.ToString());

            sb.AppendLine();
            sb.AppendLine(DrawFields(staticFields.typeDescription, new MemoryProfilerWindowForExtension.BytesAndOffset() { bytes = staticFields.typeDescription.staticFieldBytes, offset = 0, pointerSize = s_UnpackedCrawl.virtualMachineInformation.pointerSize }, true));
        }
        return sb.ToString();
    }
    internal MemoryProfilerWindowForExtension.ThingInMemory GetThingAt(ulong address)
    {
        if (!s_ObjectCache.ContainsKey(address)) {
            s_ObjectCache[address] = s_UnpackedCrawl.allObjects.OfType<MemoryProfilerWindowForExtension.ManagedObject>().FirstOrDefault(mo => mo.address == address);
        }

        return s_ObjectCache[address];
    }
    internal string ReadString(ulong address)
    {
        return MemoryProfilerWindowForExtension.StringTools.ReadString(MemoryProfilerWindowForExtension.ManagedHeapExtensions.Find(s_UnpackedCrawl.managedHeap, address, s_UnpackedCrawl.virtualMachineInformation), s_UnpackedCrawl.virtualMachineInformation);
    }
    private string DrawArray(MemoryProfilerWindowForExtension.ManagedObject managedObject)
    {
        StringBuilder sb = new StringBuilder();
        var typeDescription = managedObject.typeDescription;
        int elementCount = MemoryProfilerWindowForExtension.ArrayTools.ReadArrayLength(s_UnpackedCrawl.managedHeap, managedObject.address, typeDescription, s_UnpackedCrawl.virtualMachineInformation);
        sb.AppendLine("element count: " + elementCount);
        int rank = typeDescription.arrayRank;
        sb.AppendLine("arrayRank: " + rank);
        if (s_UnpackedCrawl.typeDescriptions[typeDescription.baseOrElementTypeIndex].isValueType) {
            sb.AppendLine("Cannot yet display elements of value type arrays");
            return sb.ToString();
        }
        if (rank != 1) {
            sb.AppendLine("Cannot display non rank=1 arrays yet.");
            return sb.ToString();
        }

        var pointers = new List<UInt64>();
        for (int i = 0; i != elementCount; i++) {
            pointers.Add(s_PrimitiveValueReader.ReadPointer(managedObject.address + (UInt64)s_UnpackedCrawl.virtualMachineInformation.arrayHeaderSize + (UInt64)(i * s_UnpackedCrawl.virtualMachineInformation.pointerSize)));
        }
        sb.AppendLine("elements: [");
        DrawLinks(sb, pointers);
        sb.AppendLine("]");
        return sb.ToString();
    }
    private string DrawFields(MemoryProfilerWindowForExtension.ManagedObject managedObject)
    {
        StringBuilder sb = new StringBuilder();
        if (managedObject.typeDescription.isArray)
            return string.Empty;
        sb.AppendLine(DrawFields(managedObject.typeDescription, MemoryProfilerWindowForExtension.ManagedHeapExtensions.Find(s_UnpackedCrawl.managedHeap, managedObject.address, s_UnpackedCrawl.virtualMachineInformation)));
        return sb.ToString();
    }
    private string DrawFields(TypeDescription typeDescription, MemoryProfilerWindowForExtension.BytesAndOffset bytesAndOffset, bool useStatics = false)
    {
        StringBuilder sb = new StringBuilder();
        int counter = 0;
        foreach (var field in MemoryProfilerWindowForExtension.TypeTools.AllFieldsOf(typeDescription, s_UnpackedCrawl.typeDescriptions, useStatics ? MemoryProfilerWindowForExtension.TypeTools.FieldFindOptions.OnlyStatic : MemoryProfilerWindowForExtension.TypeTools.FieldFindOptions.OnlyInstance)) {
            counter++;
            sb.AppendLine(DrawValueFor(field, bytesAndOffset.Add(field.offset), field.name));
        }
        return sb.ToString();
    }
    private string DrawValueFor(FieldDescription field, MemoryProfilerWindowForExtension.BytesAndOffset bytesAndOffset, string name)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(name);
        sb.Append(": ");
        var typeDescription = s_UnpackedCrawl.typeDescriptions[field.typeIndex];
        try {
            switch (typeDescription.name) {
                case "System.Int32":
                    sb.Append(s_PrimitiveValueReader.ReadInt32(bytesAndOffset).ToString());
                    break;
                case "System.Int64":
                    sb.Append(s_PrimitiveValueReader.ReadInt64(bytesAndOffset).ToString());
                    break;
                case "System.UInt32":
                    sb.Append(s_PrimitiveValueReader.ReadUInt32(bytesAndOffset).ToString());
                    break;
                case "System.UInt64":
                    sb.Append(s_PrimitiveValueReader.ReadUInt64(bytesAndOffset).ToString());
                    break;
                case "System.Int16":
                    sb.Append(s_PrimitiveValueReader.ReadInt16(bytesAndOffset).ToString());
                    break;
                case "System.UInt16":
                    sb.Append(s_PrimitiveValueReader.ReadUInt16(bytesAndOffset).ToString());
                    break;
                case "System.Byte":
                    sb.Append(s_PrimitiveValueReader.ReadByte(bytesAndOffset).ToString());
                    break;
                case "System.SByte":
                    sb.Append(s_PrimitiveValueReader.ReadSByte(bytesAndOffset).ToString());
                    break;
                case "System.Char":
                    sb.Append(s_PrimitiveValueReader.ReadChar(bytesAndOffset).ToString());
                    break;
                case "System.Boolean":
                    sb.Append(s_PrimitiveValueReader.ReadBool(bytesAndOffset).ToString());
                    break;
                case "System.Single":
                    sb.Append(s_PrimitiveValueReader.ReadSingle(bytesAndOffset).ToString());
                    break;
                case "System.Double":
                    sb.Append(s_PrimitiveValueReader.ReadDouble(bytesAndOffset).ToString());
                    break;
                case "System.IntPtr":
                    sb.Append(s_PrimitiveValueReader.ReadPointer(bytesAndOffset).ToString("X"));
                    break;
                default:
                    if (!typeDescription.isValueType) {
                        MemoryProfilerWindowForExtension.ThingInMemory item = GetThingAt(bytesAndOffset.ReadPointer());
                        var mobj = item as MemoryProfilerWindowForExtension.ManagedObject;
                        if (null != mobj && mobj.typeDescription.name == "System.String") {
                            sb.Append(ReadString(mobj.address));
                        }
                        else {
                            sb.Append(item.caption);
                        }
                    }
                    else {
                        sb.AppendLine("{");
                        sb.AppendLine(DrawFields(typeDescription, bytesAndOffset));
                        sb.Append("}");
                    }
                    break;
            }
        }
        catch (Exception ex) {
            Debug.LogFormat("<bad_entry> type: {0}, len: {1}, offset: {2}, ex: {3}", typeDescription.name, bytesAndOffset.bytes.Length, bytesAndOffset.offset, ex.GetType().Name);
        }
        return sb.ToString();
    }
    private void DrawLinks(StringBuilder sb, IEnumerable<UInt64> pointers)
    {
        DrawLinks(sb, pointers.Select(p => GetThingAt(p)));
    }
    private void DrawLinks(StringBuilder sb, IEnumerable<MemoryProfilerWindowForExtension.ThingInMemory> thingInMemories)
    {
        foreach (var rb in thingInMemories) {
            var caption = rb == null ? "null" : rb.caption;
            var managedObject = rb as MemoryProfilerWindowForExtension.ManagedObject;
            if (managedObject != null && managedObject.typeDescription.name == "System.String")
                caption = ReadString(managedObject.address);
            sb.AppendLine(caption);
        }
    }

    private MemoryProfilerWindowForExtension.CrawledMemorySnapshot s_UnpackedCrawl;
    private MemoryProfilerWindowForExtension.PrimitiveValueReader s_PrimitiveValueReader;
    private Dictionary<ulong, MemoryProfilerWindowForExtension.ThingInMemory> s_ObjectCache = new Dictionary<ulong, MemoryProfilerWindowForExtension.ThingInMemory>();
}
#endregion