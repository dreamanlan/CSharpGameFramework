#define USE_GM_STORY

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using GmCommands;

#region interpreter
#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace StoryScript.DslExpression
{
    internal class GetObjectByIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            UnityEngine.Object r = null;
            if (operands.Count >= 1) {
                var instId = operands[0].GetInt();
                if (instId != 0) {
                    var o = typeof(UnityEngine.Object).InvokeMember("FindObjectFromInstanceID", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[] { instId });
                    r = o as UnityEngine.Object;
                }
            }
            return r;
        }
    }
    internal class GetObjectIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int r = 0;
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = obj.GetInstanceID();
                }
            }
            return r;
        }
    }
    internal class AssetPath2GUIDExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string r = string.Empty;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var assetPath = operands[0].AsString;
                if (null != assetPath) {
                    r = AssetDatabase.AssetPathToGUID(assetPath);
                }
            }
#endif
            return r;
        }
    }
    internal class GUID2AssetPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string r = string.Empty;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var guid = operands[0].AsString;
                if (null != guid) {
                    r = AssetDatabase.GUIDToAssetPath(guid);
                }
            }
#endif
            return r;
        }
    }
    internal class GetAssetPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string r = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    var pobj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                    if (null != pobj)
                        r = AssetDatabase.GetAssetPath(pobj);
                    else
                        r = AssetDatabase.GetAssetPath(obj);
                }
            }
#endif
            return r;
        }
    }
    internal class GetGuidAndLocalFileIdentifierExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    var pobj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                    if (null == pobj)
                        pobj = obj;
                    string guid = string.Empty;
                    long localId = 0;
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(pobj, out guid, out localId)) {
                        r = BoxedValue.FromObject(new KeyValuePair<string, long>(guid, localId));
                    }
                }
            }
#endif
            return r;
        }
    }
    internal class GetDependenciesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string[] ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        list.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    list.Add(tempStr);
                            }
                        }
                    }
                }
                if (list.Count == 1) {
                    ret = AssetDatabase.GetDependencies(list[0]);
                }
                else if (list.Count > 1) {
                    ret = AssetDatabase.GetDependencies(list.ToArray());
                }
            }
#endif
            return BoxedValue.FromObject(ret);
        }
    }
    internal class GetDirectDependenciesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string[] ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        list.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    list.Add(tempStr);
                            }
                        }
                    }
                }
                if (list.Count == 1) {
                    ret = AssetDatabase.GetDependencies(list[0], false);
                }
                else if (list.Count > 1) {
                    ret = AssetDatabase.GetDependencies(list.ToArray(), false);
                }
            }
#endif
            return BoxedValue.FromObject(ret);
        }
    }
    internal class GetDependenciesGraphExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string[] ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        list.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    list.Add(tempStr);
                            }
                        }
                    }
                }
                List<string> results = new List<string>();
                HashSet<string> accessed = new HashSet<string>();
                Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();
                foreach (var str in list) {
                    queue.Enqueue(Tuple.Create(str, string.Empty));
                }
                while (queue.Count > 0) {
                    var tuple = queue.Dequeue();
                    string asset = tuple.Item1;
                    string path = tuple.Item2 + "->" + tuple.Item1;
                    if (accessed.Contains(asset)) {
                        results.Add(path);
                    }
                    else {
                        accessed.Add(asset);
                        var deps = AssetDatabase.GetDependencies(asset, false);
                        if (deps.Length > 0) {
                            foreach (var dep in deps) {
                                queue.Enqueue(Tuple.Create(dep, path));
                            }
                        }
                        else {
                            results.Add(path);
                        }
                    }
                }
                ret = results.ToArray();
            }
#endif
            return BoxedValue.FromObject(ret);
        }
    }
    internal class GetAssetImporterExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    r = BoxedValue.FromObject(AssetImporter.GetAtPath(path));
                }
            }
#endif
            return r;
        }
    }
    internal class LoadAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    r = BoxedValue.FromObject(AssetDatabase.LoadMainAssetAtPath(path));
                }
            }
#endif
            return r;
        }
    }
    internal class LoadAssetsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    r = BoxedValue.FromObject(AssetDatabase.LoadAllAssetsAtPath(path));
                }
            }
#endif
            return r;
        }
    }
    internal class UnloadAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    var t = obj.GetType();
                    if (typeof(GameObject).IsAssignableFrom(t) || typeof(Component).IsAssignableFrom(t) || typeof(AssetBundle).IsAssignableFrom(t) || typeof(ScriptableObject).IsAssignableFrom(t)) {
#if UNITY_EDITOR
                        if (!PrefabUtility.IsPartOfPrefabAsset(obj)) {
                            StoryScriptUtility.DestroyObject(obj);
                            //EditorUtility.UnloadUnusedAssetsImmediate();
                        }
#endif
                    }
                    else {
                        Resources.UnloadAsset(obj);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal class AddObjectToAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = false;
#if UNITY_EDITOR
            if (operands.Count >= 2) {
                var obj = operands[0].As<UnityEngine.Object>();
                var path = operands[1].AsString;
                var assetObj = operands[1].As<UnityEngine.Object>();
                if (null != obj && !string.IsNullOrEmpty(path)) {
                    AssetDatabase.AddObjectToAsset(obj, path);
                    r = true;
                }
                else if (null != obj && null != assetObj) {
                    AssetDatabase.AddObjectToAsset(obj, assetObj);
                    r = true;
                }
            }
#endif
            return r;
        }
    }
    internal class RemoveObjectFromAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = false;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    AssetDatabase.RemoveObjectFromAsset(obj);
                    r = true;
                }
            }
#endif
            return r;
        }
    }
    internal class FromJsonOverwriteExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = false;
#if UNITY_EDITOR
            if (operands.Count >= 2) {
                var json = operands[0].AsString;
                var obj = operands[1].As<UnityEngine.Object>();
                if (!string.IsNullOrEmpty(json) && null != obj) {
                    EditorJsonUtility.FromJsonOverwrite(json, obj);
                    r = true;
                }
            }
#endif
            return r;
        }
    }
    internal class ToJsonExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.EmptyString;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                var pretty = operands.Count > 1 ? operands[1].GetBool() : false;
                if (null != obj) {
                    r = BoxedValue.FromString(EditorJsonUtility.ToJson(obj, pretty));
                }
            }
#endif
            return r;
        }
    }
    internal sealed class GetStringInLengthExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3) {
                throw new Exception("Expected: get_string_in_length(str, len[, get_the_end_def_false])");
            }
            string str = operands[0].AsString;
            int len = operands[1].GetInt();
            bool getTheEnd = operands.Count > 2 ? operands[2].GetBool() : false;
            if (!string.IsNullOrEmpty(str)) {
                if (getTheEnd) {
                    return BoxedValue.From(str.Length > len ? "..." + str.Substring(str.Length - len, len) : str);
                }
                else {
                    return BoxedValue.From(str.Length > len ? str.Substring(0, len) + "..." : str);
                }
            }
            return BoxedValue.EmptyString;
        }
    }
    internal class GetPrefabTypeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetPrefabAssetType(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabStatusExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetPrefabInstanceStatus(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabObjectExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetPrefabInstanceHandle(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabParentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetCorrespondingObjectFromSource(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabParentAtPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2) {
                var obj = operands[0].As<UnityEngine.Object>();
                var assetPath = operands[1].AsString;
                if (null != obj && !string.IsNullOrEmpty(assetPath)) {
                    r = BoxedValue.FromObject(PrefabUtility.GetCorrespondingObjectFromSourceAtPath(obj, assetPath));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabParentFromOriginExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetNearstPrefabRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetNearestPrefabInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetNearstPrefabRootAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetOutermostPrefabRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetOutermostPrefabInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetOriginalPrefabRootWhereAddedExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetOriginalSourceRootWhereGameObjectIsAdded(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabOverridesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.GameObject>();
                bool includeDefaultOverrides = false;
                if (operands.Count >= 2) {
                    includeDefaultOverrides = operands[1].GetBool();
                }
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.GetObjectOverrides(obj, includeDefaultOverrides));
                }
            }
#endif
            return r;
        }
    }
    internal class FindPrefabInstancesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj) {
                    r = BoxedValue.FromObject(PrefabUtility.FindAllInstancesOfPrefab(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class FindPrefabInstancesInSceneExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.GameObject>();
                var scene = operands[1].CastTo<UnityEngine.SceneManagement.Scene>();
                if (null != obj && scene.IsValid()) {
                    r = BoxedValue.FromObject(PrefabUtility.FindAllInstancesOfPrefab(obj, scene));
                }
            }
#endif
            return r;
        }
    }
    internal class ScanPrefabExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj) {
                    var sb = new StringBuilder();
                    CheckPrefabRecursively(sb, obj, 0);
                    return sb.ToString();
                }
            }
#endif
            return r;
        }
#if UNITY_EDITOR
        private static void CheckPrefabRecursively(StringBuilder sb, UnityEngine.GameObject obj, int indent)
        {
            var objFromSource = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            var originObj = PrefabUtility.GetOriginalSourceRootWhereGameObjectIsAdded(obj);
            var objFromOrigin = originObj ? PrefabUtility.GetCorrespondingObjectFromOriginalSource(originObj) : null;
            sb.Append(s_IndentString.Substring(0, indent * 4));
            sb.AppendFormat("obj:{0} objFromSource:{1} origin:{2} objFromOrigin:{3}", obj, objFromSource, originObj, objFromOrigin);
            var prefab = PrefabUtility.GetPrefabInstanceHandle(obj) as UnityEngine.GameObject;
            bool isInst = true;
            if (!prefab) {
                prefab = obj;
                isInst = false;
            }
            if (null != prefab) {
                if (isInst) {
                    var objFromSourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                    var originObjPrefab = PrefabUtility.GetOriginalSourceRootWhereGameObjectIsAdded(prefab);
                    var objFromOriginPrefab = originObjPrefab ? PrefabUtility.GetCorrespondingObjectFromOriginalSource(originObjPrefab) : null;
                    sb.AppendFormat(" prefab:{0} prefab_objFromSource:{1} prefab_origin:{2} prefab_objFromOrigin:{3}", prefab, objFromSourcePrefab, originObjPrefab, objFromOriginPrefab);
                }
                var assetType = PrefabUtility.GetPrefabAssetType(prefab);
                var status = PrefabUtility.GetPrefabInstanceStatus(prefab);
                bool missing = PrefabUtility.HasManagedReferencesWithMissingTypes(prefab);
                string asset = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
                var rootNearst = PrefabUtility.GetNearestPrefabInstanceRoot(prefab);
                var rootOutermost = PrefabUtility.GetOutermostPrefabInstanceRoot(prefab);
                bool overrides = PrefabUtility.HasPrefabInstanceAnyOverrides(prefab, true);
                bool overridesNearst = rootNearst ? PrefabUtility.HasPrefabInstanceAnyOverrides(rootNearst, true) : false;
                bool overridesOuter = rootOutermost ? PrefabUtility.HasPrefabInstanceAnyOverrides(rootOutermost, true) : false;
                sb.AppendFormat(" type:{0} status:{1} missing:{2} overrides:{3} overridesNearst:{4} overridesOutermost:{5} asset:{6}", assetType, status, missing, overrides, overridesNearst, overridesOuter, asset);
                if (rootNearst) {
                    sb.AppendFormat(" root:{0}", rootNearst.name);
                }
                if (rootOutermost) {
                    sb.AppendFormat(" rootOuter:{0}", rootOutermost.name);
                }
                sb.AppendFormat(" added_comp_overrides:{0} added_obj_overrides:{1} anyroot:{2} outermost:{3}", PrefabUtility.IsAddedComponentOverride(prefab)
                    , PrefabUtility.IsAddedGameObjectOverride(prefab)
                    , PrefabUtility.IsAnyPrefabInstanceRoot(prefab)
                    , PrefabUtility.IsOutermostPrefabInstanceRoot(prefab));
                sb.AppendFormat(" part_of_any:{0} part_of_immutable:{1} part_of_model:{2} part_of_nonasset:{3} part_of_asset:{4} part_of_instance:{5} part_of_canbeapply:{6} part_of_regular:{7} part_of_variant:{8} missing:{9}"
                    , PrefabUtility.IsPartOfAnyPrefab(prefab)
                    , PrefabUtility.IsPartOfImmutablePrefab(prefab)
                    , PrefabUtility.IsPartOfModelPrefab(prefab)
                    , PrefabUtility.IsPartOfNonAssetPrefabInstance(prefab)
                    , PrefabUtility.IsPartOfPrefabAsset(prefab)
                    , PrefabUtility.IsPartOfPrefabInstance(prefab)
                    , PrefabUtility.IsPartOfPrefabThatCanBeAppliedTo(prefab)
                    , PrefabUtility.IsPartOfRegularPrefab(prefab)
                    , PrefabUtility.IsPartOfVariantPrefab(prefab)
                    , PrefabUtility.IsPrefabAssetMissing(prefab));
                if (isInst) {
                    sb.Append(" modifications:");
                    var modifications = PrefabUtility.GetPropertyModifications(prefab);
                    if (null != modifications) {
                        foreach (var modification in modifications) {
                            sb.AppendFormat(" {0}", modification.propertyPath);
                        }
                    }
                    sb.Append(" added_objs:");
                    var addObjs = PrefabUtility.GetAddedGameObjects(prefab);
                    if (null != addObjs) {
                        foreach (var mobj in addObjs) {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" removed_objs:");
                    var removeObjs = PrefabUtility.GetRemovedGameObjects(prefab);
                    if (null != removeObjs) {
                        foreach (var mobj in removeObjs) {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" added_comps:");
                    var addComps = PrefabUtility.GetAddedComponents(prefab);
                    if (null != addComps) {
                        foreach (var mobj in addComps) {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" removed_comps:");
                    var removeComps = PrefabUtility.GetRemovedComponents(prefab);
                    if (null != removeComps) {
                        foreach (var mobj in removeComps) {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                }
                sb.Append(" comps:");
                foreach (var comp in obj.GetComponents<Component>()) {
                    sb.AppendFormat(" {0}({1})", comp.name, comp.GetType().Name);
                }
                sb.AppendLine();
                for (int ix = 0; ix < obj.transform.childCount; ++ix) {
                    var tr = obj.transform.GetChild(ix);
                    CheckPrefabRecursively(sb, tr.gameObject, indent + 1);
                }
            }
        }
        private static string s_IndentString = "                                                                                                                                                                                                                                                                ";
#endif
    }
    internal class ScanDependencyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2) {
                var prefabA = operands[0].AsString;
                var prefabB = operands[1].AsString;
                if (!string.IsNullOrEmpty(prefabA) && !string.IsNullOrEmpty(prefabB)) {
                    var sb = new StringBuilder();
                    ScanForDependency(prefabA, prefabB, sb);
                    return sb.ToString();
                }
            }
#endif
            return r;
        }
#if UNITY_EDITOR
        private static void ScanForDependency(string sourcePath, string targetPath, StringBuilder sb)
        {
            GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            GameObject targetDependency = AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);

            if (sourcePrefab == null || targetDependency == null) {
                sb.AppendLine("The path is incorrect; please check the path in the script.");
                return;
            }

            sb.AppendLine($"[Start] Checking if '{sourcePrefab.name}' depends on '{targetDependency.name}'...");

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(sourcePrefab);
            bool foundDependency = false;

            try {
                // PHASE 1: Hierarchy
                Transform[] allTransforms = instance.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in allTransforms) {
                    GameObject sourceAsset = PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject);

                    // Use Shared Utility
                    if (sourceAsset != null && IsAssetDerivedFrom(sourceAsset, targetPath)) {
                        sb.AppendLine();
                        sb.AppendLine($"    [Hierarchy Dependency] Node: '{GetHierarchyPath(t)}' is instance/variant of target.");
                        foundDependency = true;
                    }
                }

                // PHASE 2: Properties
                Component[] allComponents = instance.GetComponentsInChildren<Component>(true);
                foreach (Component comp in allComponents) {
                    if (comp == null) {
                        continue;
                    }
                    SerializedObject so = new SerializedObject(comp);
                    SerializedProperty sp = so.GetIterator();

                    while (sp.Next(true)) {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference) {
                            var refObj = sp.objectReferenceValue;
                            if (refObj == null) {
                                continue;
                            }
                            if (IsSelfReference(refObj, instance.transform)) {
                                continue;
                            }
                            string refPath = AssetDatabase.GetAssetPath(refObj);
                            if (refPath == sourcePath) {
                                continue;
                            }
                            // Use Shared Utility
                            if (IsObjectDerivedFrom(refObj, targetPath)) {
                                sb.AppendLine();
                                sb.AppendLine($"    [Property Dependency] Source: '{GetHierarchyPath(comp.transform)}' -> Property: '{sp.propertyPath}' refers to target.");
                                foundDependency = true;
                            }
                        }
                    }
                }

                sb.AppendLine();
                if (!foundDependency) {
                    sb.AppendLine($"    [Result] No dependency found.");
                }
                else {
                    sb.AppendLine($"    [Result] Dependencies found!");
                }
            }
            finally {
                GameObject.DestroyImmediate(instance);
            }
        }
        /// Checks if the object is part of the current hierarchy (Self-Reference).
        /// </summary>
        /// <param name="obj">The object to check (GameObject or Component).</param>
        /// <param name="rootTransform">The root of the instantiated hierarchy.</param>
        /// <returns>True if the object is a child of the root or the root itself.</returns>
        internal static bool IsSelfReference(UnityEngine.Object obj, Transform rootTransform)
        {
            if (obj is GameObject go) {
                return go.transform.IsChildOf(rootTransform);
            }
            else if (obj is Component comp) {
                return comp.transform.IsChildOf(rootTransform);
            }

            // Assets like Materials, Textures, etc., are not part of the hierarchy
            return false;
        }
        // Helper: Check if an object (Component or GameObject) belongs to an asset
        // that is (or inherits from) the target path.
        internal static bool IsObjectDerivedFrom(UnityEngine.Object obj, string targetPath)
        {
            if (obj == null) {
                return false;
            }

            // Get the path of the asset this object belongs to
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(objPath)) {
                return false;
            }

            // 1. Direct path match (Fastest)
            if (objPath == targetPath) {
                return true;
            }

            // 2. Variant check
            // Load the Main Asset at the path to check the Prefab chain
            GameObject assetRoot = AssetDatabase.LoadMainAssetAtPath(objPath) as GameObject;
            return IsAssetDerivedFrom(assetRoot, targetPath);
        }
        // Recursive check for Prefab Variants
        internal static bool IsAssetDerivedFrom(GameObject assetToCheck, string targetPath)
        {
            if (assetToCheck == null) {
                return false;
            }

            string currentPath = AssetDatabase.GetAssetPath(assetToCheck);
            if (currentPath == targetPath) {
                return true;
            }

            GameObject basePrefab = PrefabUtility.GetCorrespondingObjectFromSource(assetToCheck);

            if (basePrefab == null) {
                return false;
            }
            if (basePrefab == assetToCheck) {
                return false;
            }
            return IsAssetDerivedFrom(basePrefab, targetPath);
        }
        // Helper: Check if a GameObject is the main root of its own asset file
        internal static bool IsRootOfItsAsset(GameObject go)
        {
            if (go == null) {
                return false;
            }
            string assetPath = AssetDatabase.GetAssetPath(go);
            GameObject mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as GameObject;
            return go == mainAsset;
        }
        internal static string GetHierarchyPath(Transform t)
        {
            if (t == null) {
                return "";
            }
            string path = t.name;
            while (t.parent != null && t.parent.parent != null) {
                t = t.parent;
                path = t.name + "/" + path;
            }
            return path;
        }
#endif
    }
    internal class CheckInternalDependencyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2) {
                var prefabA = operands[0].AsString;
                var prefabB = operands[1].AsString;
                bool checkRootObjComp = false;
                bool checkChildGameObjects = false;
                if (operands.Count >= 3) {
                    checkRootObjComp = operands[2].GetBool();
                }
                if (operands.Count >= 4) {
                    checkChildGameObjects = operands[3].GetBool();
                }
                if (!string.IsNullOrEmpty(prefabA) && !string.IsNullOrEmpty(prefabB)) {
                    var sb = new StringBuilder();
                    bool foundIssue = CheckInternalDependency(prefabA, prefabB, checkRootObjComp, checkChildGameObjects, sb);
                    if (foundIssue) {
                        return sb.ToString();
                    }
                    return string.Empty;
                }
            }
#endif
            return r;
        }
#if UNITY_EDITOR
        private static bool CheckInternalDependency(string sourcePath, string targetPath, bool checkRootObjComp, bool checkChildGameObjects, StringBuilder sb)
        {
            GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            GameObject targetDependency = AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);

            if (sourcePrefab == null || targetDependency == null) {
                Debug.LogErrorFormat("source:{0} target:{1} checkRoot:{2} checkChild:{3} sourcePrefab:{4} targetDependency:{5}", sourcePath, targetPath, checkRootObjComp, checkChildGameObjects, sourcePrefab, targetDependency);
                return false;
            }

            // Load the Main Asset (Root GameObject) of B to distinguish between Root and Child
            GameObject targetRoot = AssetDatabase.LoadMainAssetAtPath(targetPath) as GameObject;
            if (targetRoot == null) {
                Debug.LogErrorFormat("source:{0} target:{1} checkRoot:{2} checkChild:{3} targetRoot is null", sourcePath, targetPath, checkRootObjComp, checkChildGameObjects);
                return false;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(sourcePrefab);
            bool foundIssue = false;

            try {
                Component[] allComponents = instance.GetComponentsInChildren<Component>(true);

                foreach (Component comp in allComponents) {
                    if (comp == null) {
                        continue;
                    }

                    SerializedObject so = null;
                    try {
                        so = new SerializedObject(comp);
                    }
                    catch (Exception ex) {
                        Debug.LogErrorFormat("source:{0} target:{1} checkRoot:{2} checkChild:{3} comp:{4} exception:{5}\n{6}", sourcePath, targetPath, checkRootObjComp, checkChildGameObjects, comp, ex.Message, ex.StackTrace);
                        so = null;
                    }
                    if (null == so) {
                        Debug.LogErrorFormat("source:{0} target:{1} checkRoot:{2} checkChild:{3} comp:{4} so is null", sourcePath, targetPath, checkRootObjComp, checkChildGameObjects, comp);
                        continue;
                    }

                    SerializedProperty sp = so.GetIterator();

                    while (sp.Next(true)) {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference) {
                            var refObj = sp.objectReferenceValue;

                            if (refObj == null) {
                                continue;
                            }
                            if (ScanDependencyExp.IsSelfReference(refObj, instance.transform)) {
                                continue;
                            }
                            string refPath = AssetDatabase.GetAssetPath(refObj);
                            if (refPath == sourcePath) {
                                continue;
                            }
                            // 1. Check if the referenced object is (or inherits from) the target Prefab
                            if (ScanDependencyExp.IsObjectDerivedFrom(refObj, targetPath)) {
                                bool shouldReport = false;
                                string referenceType = "";

                                // 2. Analyze the type of reference
                                if (refObj is Component refComp) {
                                    if (ScanDependencyExp.IsRootOfItsAsset(refComp.gameObject)) {
                                        if (checkRootObjComp) {
                                            // Case A: Reference to a Component (Script, Transform, etc.)
                                            // Always report.
                                            shouldReport = true;
                                            referenceType = $"Component({refObj.GetType().Name})";
                                        }
                                    }
                                    else {
                                        // Case A: Reference to a Component (Script, Transform, etc.)
                                        // Always report.
                                        shouldReport = true;
                                        referenceType = $"Component({refObj.GetType().Name})";
                                    }
                                }
                                else if (refObj is GameObject refGo) {
                                    // Case B: Reference to a GameObject
                                    // We need to check if it's the Root or a Child.

                                    // To do this accurately for Variants, we check if the referenced GO
                                    // corresponds to the Root of the asset it lives in.
                                    if (ScanDependencyExp.IsRootOfItsAsset(refGo)) {
                                        // It is a Root GameObject (Safe dependency)
                                        // (Logic: pass)
                                    }
                                    else {
                                        // It is a Child GameObject (Sub-Asset)
                                        if (checkChildGameObjects) {
                                            shouldReport = true;
                                            referenceType = "Child_GameObject";
                                        }
                                    }
                                }

                                // 3. Log
                                if (shouldReport) {
                                    if (foundIssue) {
                                        sb.Append(" ");
                                    }
                                    foundIssue = true;
                                    sb.Append($"[INTERNAL_DEPENDENCY]");
                                    sb.Append($" Source({ScanDependencyExp.GetHierarchyPath(comp.transform)})");
                                    sb.Append($" Property({sp.propertyPath})");
                                    sb.Append($" Refers_to({referenceType})");
                                    sb.Append($" Target_Object({refObj.name}) Asset({AssetDatabase.GetAssetPath(refObj)})");
                                }
                            }
                        }
                    }
                }

                return foundIssue;
            }
            finally {
                GameObject.DestroyImmediate(instance);
            }
        }
#endif
    }
    internal class ConvertDumpBinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count >= 2) {
                var binFile = operands[0].AsString;
                var txtFile = operands[1].AsString;
                if (!string.IsNullOrEmpty(binFile) && !string.IsNullOrEmpty(txtFile)) {
                    Convert(binFile, txtFile);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Convert the custom binary file to a formatted text file.
        /// Binary format:
        ///   [start_addr (8 bytes, Int64)]
        ///   [size (8 bytes, Int64)]
        ///   [size_truncated_to_max (8 bytes, Int64)]
        ///   [data (size_truncated_to_max bytes)]
        ///
        /// Text format:
        ///   Line 1: start_addr, size, size_truncated_to_max in hex (each as 16 hex digits)
        ///   From line 2:
        ///     - First line may be a partial line for alignment:
        ///         address = start_addr aligned down to 8-byte boundary,
        ///         bytes before real start_addr shown as spaces.
        ///     - Then normal lines:
        ///         1) Address in hex, 16 characters
        ///         2) A space
        ///         3) 16 bytes as 2-digit hex values, separated by spaces
        ///         4) A space
        ///         5) 16 ASCII chars (non-printable and >=127 shown as '.')
        ///            for positions with no data (before start_addr or after end),
        ///            spaces are printed.
        /// </summary>
        public static void Convert(string binPath, string txtPath)
        {
            using FileStream fs = new FileStream(binPath, FileMode.Open, FileAccess.Read);
            using BinaryReader br = new BinaryReader(fs);
            using StreamWriter sw = new StreamWriter(txtPath, false, Encoding.ASCII);

            // Read header: 3 x 64-bit values (assuming little-endian platform)
            long startAddr = br.ReadInt64();
            long size = br.ReadInt64();
            long sizeTruncated = br.ReadInt64();

            // Helper to format a 64-bit integer as 16-digit uppercase hex
            static string ToHex64(long value) => value.ToString("x16");

            // Line 1: three 64-bit values in hex, width 16
            sw.WriteLine($"{ToHex64(startAddr)} {ToHex64(size)} {ToHex64(sizeTruncated)}");

            // Read data region
            byte[] data = br.ReadBytes(checked((int)sizeTruncated));

            const int bytesPerLine = 16;

            // Align start address down to 8-byte boundary
            long alignedAddr = startAddr & ~0x7L;

            long addr = alignedAddr;
            int offset = 0;

            // Handle possible first partial line before startAddr
            long delta = startAddr - alignedAddr; // how many bytes before the first real byte
            if (delta > 0) {
                int preBytes = (int)Math.Min(bytesPerLine, delta);
                int lineBytes = Math.Min(bytesPerLine, preBytes + data.Length);

                // 1) Address field
                sw.Write(ToHex64(addr));
                sw.Write(' ');

                // 2) Hex bytes: positions before start_addr are spaces
                for (int i = 0; i < bytesPerLine; i++) {
                    if (i < preBytes) {
                        // 2 hex chars + 1 space => 3 spaces
                        sw.Write("   ");
                    }
                    else if (i < lineBytes) {
                        int dataIndex = i - preBytes;
                        sw.Write(data[dataIndex].ToString("x2"));
                        sw.Write(i == bytesPerLine - 1 ? ' ' : ' ');
                        if (i < bytesPerLine - 1)
                            sw.Write(' '); // keep one extra space as separator
                    }
                    else {
                        // no more data on this line; fill hex area with spaces
                        sw.Write("   ");
                    }
                }

                sw.Write(' ');

                // 3) ASCII: spaces for positions before start_addr or without data
                for (int i = 0; i < bytesPerLine; i++) {
                    if (i < preBytes) {
                        sw.Write(' ');
                    }
                    else if (i < lineBytes) {
                        int dataIndex = i - preBytes;
                        byte b = data[dataIndex];
                        char c = (b >= 32 && b < 127) ? (char)b : '.';
                        sw.Write(c);
                    }
                    else {
                        sw.Write(' ');
                    }
                }

                sw.WriteLine();

                // Advance offset and addr
                int consumed = lineBytes - preBytes;
                offset += consumed;
                addr += bytesPerLine;
            }

            // Normal full/partial lines after the first aligned line
            while (offset < data.Length) {
                int count = Math.Min(bytesPerLine, data.Length - offset);

                // 1) Address field (16 hex digits)
                sw.Write(ToHex64(addr));
                sw.Write(' ');

                // 2) Hex bytes (up to 16, separated by spaces; pad to 16-byte width)
                for (int i = 0; i < bytesPerLine; i++) {
                    if (i < count) {
                        sw.Write(data[offset + i].ToString("x2"));
                        if (i < bytesPerLine - 1) {
                            sw.Write(' ');
                        }
                    }
                    else {
                        // pad hex area for missing bytes
                        sw.Write("   ");
                    }
                }

                sw.Write(' ');

                // 3) ASCII representation (pad with spaces for missing bytes)
                for (int i = 0; i < bytesPerLine; i++) {
                    if (i < count) {
                        byte b = data[offset + i];
                        char c = (b >= 32 && b < 127) ? (char)b : '.';
                        sw.Write(c);
                    }
                    else {
                        sw.Write(' ');
                    }
                }

                sw.WriteLine();

                offset += count;
                addr += bytesPerLine;
            }
        }
        /// <summary>
        /// Align the given 64-bit value to the next 8-byte boundary (if not already aligned).
        /// </summary>
        private static long AlignTo8(long value)
        {
            const long alignment = 8;
            long remainder = value & (alignment - 1);
            if (remainder == 0) {
                return value;
            }
            return value + (alignment - remainder);
        }
    }
    internal class DisplayProgressBarExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            if (operands.Count >= 3) {
                var title = operands[0].AsString;
                var text = operands[1].AsString;
                var progress = operands[2].GetFloat();
                if (null != title && null != text) {
                    EditorUtility.DisplayProgressBar(title, text, progress);
                }
            }
#endif
            return true;
        }
    }
    internal class DisplayCancelableProgressBarExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool ret = false;
#if UNITY_EDITOR
            if (operands.Count >= 3) {
                var title = operands[0].AsString;
                var text = operands[1].AsString;
                var progress = operands[2].GetFloat();
                if (null != title && null != text) {
                    ret = EditorUtility.DisplayCancelableProgressBar(title, text, progress);
                }
            }
#endif
            return ret;
        }
    }
    internal class ClearProgressBarExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            return true;
        }
    }
    internal class OpenWithDefaultAppExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            if (operands.Count >= 1) {
                string file = operands[0].AsString;
                if (!string.IsNullOrEmpty(file)) {
                    EditorUtility.OpenWithDefaultApp(file);
                }
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal class OpenFolderPanelExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0].AsString;
                string dir = operands[1].AsString;
                string def = operands[2].AsString;
                if (null != title && null != dir && null != def) {
                    ret = EditorUtility.OpenFolderPanel(title, dir, def);
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class OpenFilePanelExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0].AsString;
                string dir = operands[1].AsString;
                string ext = operands[2].AsString;
                if (null != title && null != dir && null != ext) {
                    ret = EditorUtility.OpenFilePanel(title, dir, ext);
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class OpenFilePanelWithFiltersExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0].AsString;
                string dir = operands[1].AsString;
                List<string> filters = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    string filter = operands[i].AsString;
                    if (!string.IsNullOrEmpty(filter)) {
                        filters.Add(filter);
                    }
                }
                if (null != title && null != dir) {
                    ret = EditorUtility.OpenFilePanelWithFilters(title, dir, filters.ToArray());
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class SaveFilePanelExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 4) {
                string title = operands[0].AsString;
                string dir = operands[1].AsString;
                string def = operands[2].AsString;
                string ext = operands[3].AsString;
                if (null != title && null != dir && null != def && null != ext) {
                    ret = EditorUtility.SaveFilePanel(title, dir, def, ext);
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class SaveFilePanelInProjectExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 4) {
                string title = operands[0].AsString;
                string def = operands[1].AsString;
                string ext = operands[2].AsString;
                string msg = operands[3].AsString;
                string path = string.Empty;
                if (operands.Count >= 5) {
                    path = operands[4].AsString;
                }
                if (null != title && null != def && null != ext && null != msg) {
                    if (!string.IsNullOrEmpty(path)) {
                        ret = EditorUtility.SaveFilePanelInProject(title, def, ext, msg, path);
                    }
                    else {
                        ret = EditorUtility.SaveFilePanelInProject(title, def, ext, msg);
                    }
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class SaveFolderPanelExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            string ret = null;
            if (operands.Count >= 3) {
                string title = operands[0].AsString;
                string dir = operands[1].AsString;
                string def = operands[2].AsString;
                if (null != title && null != dir && null != def) {
                    ret = EditorUtility.SaveFolderPanel(title, dir, def);
                }
            }
            return ret;
#else
            return BoxedValue.NullObject;
#endif
        }
    }
    internal class DisplayDialogExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            int ret = -1;
            if (operands.Count >= 3) {
                string title = operands[0].AsString;
                string msg = operands[1].AsString;
                string ok = operands[2].AsString;
                if (null != title && null != msg && null != ok) {
                    if (operands.Count >= 4) {
                        string cancel = operands[3].AsString;
                        if (operands.Count >= 5) {
                            string alt = operands[4].AsString;
                            ret = EditorUtility.DisplayDialogComplex(title, msg, ok, cancel, alt);
                        }
                        else {
                            ret = EditorUtility.DisplayDialog(title, msg, ok, cancel) ? 1 : 0;
                        }
                    }
                    else {
                        ret = EditorUtility.DisplayDialog(title, msg, ok) ? 1 : 0;
                    }
                }
            }
            return ret;
#else
            return -1;
#endif
        }
    }
    internal class HasCmdArgExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                var name = operands[0].AsString;
                bool r = HasCommandArg(name, out var val);
                return BoxedValue.From(Tuple.Create(BoxedValue.FromBool(r), BoxedValue.FromString(val)));
            }
            return BoxedValue.From(Tuple.Create(BoxedValue.FromBool(false), BoxedValue.FromString(string.Empty)));
        }
        private static bool HasCommandArg(string name, out string val)
        {
            val = string.Empty;
            try {
                var args = Environment.GetCommandLineArgs();
                for (var i = 0; i < args.Length; i++) {
                    if (string.Equals(args[i], name, StringComparison.CurrentCultureIgnoreCase)) {
                        if (args.Length > i + 1) {
                            var result = args[i + 1];
                            if (result.Length > 0 && result[0] != '-') {
                                val = result;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception e) {
                LogSystem.Error(e.ToString());
            }
            return false;
        }
    }
    internal class StrLenExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                string str = operands[0].GetString();
                return string.IsNullOrEmpty(str) ? 0 : str.Length;
            }
            return 0;
        }
    }
#if USE_GM_STORY
    internal class StoryVarExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                string name = operands[0].AsString;
                if (operands.Count >= 2) {
                    var val = operands[1];
                    if (!string.IsNullOrEmpty(name)) {
                        var instance = ClientGmStorySystem.Instance.GetStory("main");
                        if (null == instance) {
                            string txt = "script(main){onmessage(\"start\"){};};";
                            ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                            instance = ClientGmStorySystem.Instance.GetStory("main");
                        }
                        instance.SetVariable(name, BoxedValue.FromObject(val.GetObject()));
                        ret = val;
                    }
                }
                else {
                    var instance = ClientGmStorySystem.Instance.GetStory("main");
                    if (null == instance) {
                        string txt = "script(main){onmessage(\"start\"){};};";
                        ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                        instance = ClientGmStorySystem.Instance.GetStory("main");
                    }
                    BoxedValue bv;
                    instance.TryGetVariable(name, out bv);
                    ret = BoxedValue.FromObject(bv.GetObject());
                }
            }
            return ret;
        }
    }
    /// <summary>
    /// call_story("msgId", arg1, arg2, ...);
    /// or
    /// call_story(apiOrMsgId(arg1, arg2, ...));
    /// </summary>
    internal sealed class CallStoryExp : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            if (null != m_Api) {
                return m_Api.Calc(calculator);
            }
            else {
                string msgId = m_MsgId;
                if (string.IsNullOrEmpty(m_MsgId))
                    msgId = null != m_Func ? m_Func.Calc(calculator).ToString() : string.Empty;
                var args = ClientGmStorySystem.Instance.NewBoxedValueList();
                for (int i = 0; i < m_Args.Count; ++i) {
                    args.Add(m_Args[i].Calc(calculator));
                }
                ClientGmStorySystem.Instance.SendMessage(msgId, args);
                return BoxedValue.NullObject;
            }
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                var p0 = callData.GetParam(0);
                var func = p0 as Dsl.FunctionData;
                if (null != func) {
                    var f = func.GetId();
                    if (DslCalculatorHost.GetSharedApiRegistry().TryGetFactory(f, out var factory)) {
                        m_Api = factory.Create();
                        m_Api.Load(p0, CodeStructure);
                    }
                    else {
                        m_MsgId = f;
                        for (int i = 0; i < func.GetParamNum(); ++i) {
                            m_Args.Add(CodeStructure.Load(func.GetParam(i)));
                        }
                    }
                }
                else {
                    m_Func = CodeStructure.Load(p0);
                    for (int i = 1; i < callData.GetParamNum(); ++i) {
                        m_Args.Add(CodeStructure.Load(callData.GetParam(i)));
                    }
                }
            }
            return true;
        }

        private IExpression m_Api;

        private string m_MsgId = string.Empty;
        private IExpression m_Func;
        private List<IExpression> m_Args = new List<IExpression>();
    }
#endif
#if UNITY_EDITOR
    // Play mode control for the GM socket / MCP editor channel. Entering Play triggers a
    // domain reload that tears down the socket server, so the state change is deferred to
    // the next editor update: the RPC returns normally before the reload happens.
    internal class BuildTargetInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            var group = UnityEditor.BuildPipeline.GetBuildTargetGroup(target);
            bool iosSupported = UnityEditor.BuildPipeline.IsBuildTargetSupported(UnityEditor.BuildTargetGroup.iOS, UnityEditor.BuildTarget.iOS);
            return BoxedValue.FromString(string.Format("active={0} group={1} iosSupported={2} playing={3} compiling={4} updating={5}",
                target, group, iosSupported, UnityEditor.EditorApplication.isPlaying,
                UnityEditor.EditorApplication.isCompiling, UnityEditor.EditorApplication.isUpdating));
        }
    }
    internal class SwitchBuildTargetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.FromString("ERROR: switch_build_target(name) - name: iOS | StandaloneOSX");
            string name = operands[0].GetString();
            UnityEditor.BuildTargetGroup group;
            UnityEditor.BuildTarget target;
            switch (name.ToLowerInvariant())
            {
                case "ios":
                case "iphone":
                    group = UnityEditor.BuildTargetGroup.iOS;
                    target = UnityEditor.BuildTarget.iOS;
                    break;
                case "osx":
                case "standaloneosx":
                case "standalone":
                    group = UnityEditor.BuildTargetGroup.Standalone;
                    target = UnityEditor.BuildTarget.StandaloneOSX;
                    break;
                default:
                    return BoxedValue.FromString("ERROR: unsupported target '" + name + "'; use iOS | StandaloneOSX");
            }
            if (UnityEditor.EditorApplication.isPlaying)
                return BoxedValue.FromString("ERROR: exit Play mode before switching build target");
            if (UnityEditor.EditorApplication.isCompiling || UnityEditor.EditorApplication.isUpdating)
                return BoxedValue.FromString("ERROR: editor is compiling/importing; retry later");
            if (!UnityEditor.BuildPipeline.IsBuildTargetSupported(group, target))
                return BoxedValue.FromString("ERROR: playback engine not installed for " + target);
            var g = group;
            var t = target;
            UnityEditor.EditorApplication.delayCall += () =>
            {
                bool ok = UnityEditor.EditorUserBuildSettings.SwitchActiveBuildTarget(g, t);
                Debug.Log("[GM] switch_build_target(" + t + ") ok=" + ok);
            };
            return BoxedValue.FromString("SCHEDULED: switch_build_target(" + target + ") - editor will reload/reimport; poll build_target() after reconnect");
        }
    }
    internal class EditorPlayExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool play = true;
            if (operands.Count >= 1)
                play = operands[0].GetBool();
            bool target = play;
            bool current = UnityEditor.EditorApplication.isPlaying;
            if (current != target)
            {
                UnityEditor.EditorApplication.delayCall += () => { UnityEditor.EditorApplication.isPlaying = target; };
            }
            return BoxedValue.FromBool(current);
        }
    }
    internal class EditorIsPlayingExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return BoxedValue.FromBool(UnityEditor.EditorApplication.isPlaying);
        }
    }
    internal class EditorPlayNowExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool current = UnityEditor.EditorApplication.isPlaying;
            if (!current)
                UnityEditor.EditorApplication.isPlaying = true;
            return BoxedValue.FromBool(current);
        }
    }
    internal class EditorStopNowExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool current = UnityEditor.EditorApplication.isPlaying;
            if (current)
                UnityEditor.EditorApplication.isPlaying = false;
            return BoxedValue.FromBool(current);
        }
    }
    internal class GfxFormatProbeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var b10 = UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32;
            var rgba16f = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat;
            var loadStore = UnityEngine.Experimental.Rendering.FormatUsage.LoadStore;
            var render = UnityEngine.Experimental.Rendering.FormatUsage.Render;
            var sb = new StringBuilder();
            sb.AppendLine("device=" + SystemInfo.graphicsDeviceType);
            sb.AppendLine("defaultHDR=" + SystemInfo.GetGraphicsFormat(UnityEngine.Experimental.Rendering.DefaultFormat.HDR));
            sb.AppendLine("defaultLDR=" + SystemInfo.GetGraphicsFormat(UnityEngine.Experimental.Rendering.DefaultFormat.LDR));
            sb.AppendLine("B10G11R11 Render=" + SystemInfo.IsFormatSupported(b10, render));
            sb.AppendLine("B10G11R11 LoadStore=" + SystemInfo.IsFormatSupported(b10, loadStore));
            sb.AppendLine("RGBA16F Render=" + SystemInfo.IsFormatSupported(rgba16f, render));
            sb.AppendLine("RGBA16F LoadStore=" + SystemInfo.IsFormatSupported(rgba16f, loadStore));
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                var t = asm.GetType("UnityEngine.Rendering.Universal.UniversalRenderPipeline");
                if (t == null) continue;
                foreach (var name in new string[] { "s_hdrFormat", "s_defaultFormat" }) {
                    var fi = t.GetField(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    sb.AppendLine(name + "=" + (fi == null ? "missing" : fi.GetValue(null).ToString()));
                }
                var fr = t.GetField("s_defaultResc", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (fr != null && fr.GetValue(null) is RenderTextureDescriptor rd)
                    sb.AppendLine("s_defaultResc fmt=" + rd.graphicsFormat + " rw=" + rd.enableRandomWrite);
                break;
            }
            int n = 0;
            foreach (var rt in Resources.FindObjectsOfTypeAll<RenderTexture>()) {
                if (rt.graphicsFormat != b10) continue;
                sb.AppendLine("rt name=" + rt.name + " rw=" + rt.enableRandomWrite + " size=" + rt.width + "x" + rt.height);
                if (++n >= 20) break;
            }
            sb.AppendLine("B10G11R11 RT count=" + n);
            return BoxedValue.FromString(sb.ToString());
        }
    }

    // ---------- Generic SceneView camera control for automated tests ----------
    // Unity-editor-only and project-agnostic: drives the active SceneView via pivot /
    // rotation / size. Play-mode camera control lives in the project side (GmCommands.cs)
    // because it must know about the project's runtime camera controller.
    internal static class SceneCamUtil
    {
        public static string Fmt(UnityEngine.Vector3 v)
        {
            return string.Format("({0:F2},{1:F2},{2:F2})", v.x, v.y, v.z);
        }
        public static float F(IList<BoxedValue> o, int i, float def)
        {
            return o.Count > i ? o[i].GetFloat() : def;
        }
        public static bool GetPose(out UnityEngine.Vector3 pos, out UnityEngine.Vector3 euler, out string source)
        {
            pos = UnityEngine.Vector3.zero;
            euler = UnityEngine.Vector3.zero;
            source = "none";
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            if (sv == null) { source = "no active SceneView"; return false; }
            source = "SceneView.pivot/rotation";
            pos = sv.pivot;
            euler = sv.rotation.eulerAngles;
            return true;
        }
        public static string Apply(UnityEngine.Vector3 pos, UnityEngine.Vector3 euler)
        {
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            if (sv == null) return "ERROR: no active SceneView";
            sv.pivot = pos;
            sv.rotation = UnityEngine.Quaternion.Euler(euler);
            sv.Repaint();
            return string.Format("edit:SceneView pivot={0} rotationEuler={1}", Fmt(pos), Fmt(euler));
        }

        private static bool s_SweepOn;
        private static double s_Last;
        private static float s_Speed, s_Radius, s_Angle;
        private static UnityEngine.Vector3 s_Center, s_BaseEuler;

        public static string Sweep(bool enable, float speed, float radius, UnityEngine.Vector3 center)
        {
            UnityEditor.EditorApplication.update -= SweepTick;
            s_SweepOn = false;
            if (!enable) return "scene_cam_sweep: stopped";
            UnityEngine.Vector3 pos, euler;
            string src;
            if (!GetPose(out pos, out euler, out src)) return "ERROR: " + src;
            s_Speed = speed;
            s_Radius = radius;
            s_Center = center;
            s_BaseEuler = euler;
            s_Angle = 0f;
            s_Last = UnityEditor.EditorApplication.timeSinceStartup;
            s_SweepOn = true;
            UnityEditor.EditorApplication.update += SweepTick;
            return string.Format("scene_cam_sweep: ON speed={0:F1}deg/s radius={1:F1} center={2}",
                speed, radius, Fmt(center));
        }

        private static void SweepTick()
        {
            if (!s_SweepOn) { UnityEditor.EditorApplication.update -= SweepTick; return; }
            double now = UnityEditor.EditorApplication.timeSinceStartup;
            float dt = (float)(now - s_Last);
            s_Last = now;
            if (dt <= 0f || dt > 1f) dt = 1f / 60f;
            s_Angle += s_Speed * dt;
            if (s_Angle > 360f) s_Angle -= 360f;
            UnityEngine.Vector3 pos, euler;
            string src;
            if (!GetPose(out pos, out euler, out src)) return;
            if (s_Radius > 0f)
            {
                float a = s_Angle * UnityEngine.Mathf.Deg2Rad;
                var target = new UnityEngine.Vector3(
                    s_Center.x + UnityEngine.Mathf.Sin(a) * s_Radius,
                    pos.y,
                    s_Center.z + UnityEngine.Mathf.Cos(a) * s_Radius);
                var look = s_Center - target;
                var rot = look.sqrMagnitude > 0.0001f
                    ? UnityEngine.Quaternion.LookRotation(look.normalized, UnityEngine.Vector3.up)
                    : UnityEngine.Quaternion.Euler(s_BaseEuler);
                Apply(target, rot.eulerAngles);
            }
            else
            {
                Apply(pos, new UnityEngine.Vector3(s_BaseEuler.x,
                    s_BaseEuler.y + s_Angle, s_BaseEuler.z));
            }
        }
    }

    internal class SceneCamInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            sb.AppendLine("mode=" + (UnityEditor.EditorApplication.isPlaying ? "play (use cam_* runtime APIs)" : "edit"));
            sb.AppendLine("sceneView=" + (sv == null ? "NONE" :
                "active size=" + sv.size.ToString("F2") + " orthographic=" + sv.orthographic));
            UnityEngine.Vector3 pos, euler;
            string src;
            if (SceneCamUtil.GetPose(out pos, out euler, out src))
                sb.AppendLine(string.Format("pose({0}) pivot={1} rotationEuler={2}",
                    src, SceneCamUtil.Fmt(pos), SceneCamUtil.Fmt(euler)));
            else
                sb.AppendLine("pose=UNAVAILABLE (" + src + ")");
            return BoxedValue.FromString(sb.ToString());
        }
    }

    internal class SceneCamSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: scene_cam_set(x,y,z[,pitch,yaw,roll])");
            var pos = new UnityEngine.Vector3(SceneCamUtil.F(operands, 0, 0),
                SceneCamUtil.F(operands, 1, 0), SceneCamUtil.F(operands, 2, 0));
            UnityEngine.Vector3 curPos, curEuler;
            string src;
            SceneCamUtil.GetPose(out curPos, out curEuler, out src);
            var euler = new UnityEngine.Vector3(SceneCamUtil.F(operands, 3, curEuler.x),
                SceneCamUtil.F(operands, 4, curEuler.y), SceneCamUtil.F(operands, 5, curEuler.z));
            return BoxedValue.FromString(SceneCamUtil.Apply(pos, euler));
        }
    }

    internal class SceneCamMoveExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: scene_cam_move(dx,dy,dz[,local])");
            var d = new UnityEngine.Vector3(SceneCamUtil.F(operands, 0, 0),
                SceneCamUtil.F(operands, 1, 0), SceneCamUtil.F(operands, 2, 0));
            bool local = operands.Count > 3 && operands[3].GetBool();
            UnityEngine.Vector3 pos, euler;
            string src;
            if (!SceneCamUtil.GetPose(out pos, out euler, out src)) return BoxedValue.FromString("ERROR: " + src);
            if (local) d = UnityEngine.Quaternion.Euler(euler) * d;
            return BoxedValue.FromString(SceneCamUtil.Apply(pos + d, euler));
        }
    }

    internal class SceneCamLookExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: scene_cam_look(x,y,z)");
            var target = new UnityEngine.Vector3(SceneCamUtil.F(operands, 0, 0),
                SceneCamUtil.F(operands, 1, 0), SceneCamUtil.F(operands, 2, 0));
            UnityEngine.Vector3 pos, euler;
            string src;
            if (!SceneCamUtil.GetPose(out pos, out euler, out src)) return BoxedValue.FromString("ERROR: " + src);
            var dir = target - pos;
            if (dir.sqrMagnitude < 0.0001f) return BoxedValue.FromString("ERROR: target == camera position");
            return BoxedValue.FromString(SceneCamUtil.Apply(pos,
                UnityEngine.Quaternion.LookRotation(dir.normalized, UnityEngine.Vector3.up).eulerAngles));
        }
    }

    internal class SceneCamSweepExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool enable = operands.Count < 1 || operands[0].GetBool();
            var center = new UnityEngine.Vector3(SceneCamUtil.F(operands, 3, 0),
                SceneCamUtil.F(operands, 4, 0), SceneCamUtil.F(operands, 5, 0));
            return BoxedValue.FromString(SceneCamUtil.Sweep(enable,
                SceneCamUtil.F(operands, 1, 12f), SceneCamUtil.F(operands, 2, 0f), center));
        }
    }

    // Generic edit-mode scene inspection / loading. Leaving Play mode restores a
    // Temp/__Backupscenes/0.backup scene; being able to reopen the real project scene
    // from the DSL keeps automated runs from silently testing the wrong content.
    internal class SceneInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            sb.AppendLine(string.Format("active='{0}' path='{1}' loaded={2} dirty={3}",
                scene.name, scene.path, scene.isLoaded, scene.isDirty));
            sb.AppendLine("playing=" + UnityEditor.EditorApplication.isPlaying);
            int n = UnityEditor.SceneManagement.EditorSceneManager.sceneCount;
            for (int i = 0; i < n; ++i)
            {
                var s = UnityEditor.SceneManagement.EditorSceneManager.GetSceneAt(i);
                sb.AppendLine(string.Format("  [{0}] '{1}' path='{2}'", i, s.name, s.path));
            }
            return BoxedValue.FromString(sb.ToString());
        }
    }

    internal class SceneOpenExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.FromString("ERROR: scene_open(asset_path[,single])");
            string path = operands[0].GetString();
            if (string.IsNullOrEmpty(path))
                return BoxedValue.FromString("ERROR: empty scene path");
            if (UnityEditor.EditorApplication.isPlaying)
                return BoxedValue.FromString("ERROR: exit Play mode before opening a scene");
            if (System.IO.File.Exists(path) == false)
                return BoxedValue.FromString("ERROR: scene file not found: " + path);
            bool single = operands.Count < 2 || operands[1].GetBool();
            var mode = single
                ? UnityEditor.SceneManagement.OpenSceneMode.Single
                : UnityEditor.SceneManagement.OpenSceneMode.Additive;
            var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path, mode);
            return BoxedValue.FromString(string.Format("scene_open: '{0}' path='{1}' mode={2} roots={3}",
                scene.name, scene.path, mode, scene.rootCount));
        }
    }

    // ---------- Generic editor window activation + SceneView/Game camera sync ----------
    internal static class EditorWindowCamUtil
    {
        public static System.Type EditorType(string shortName)
        {
            var asm = typeof(UnityEditor.EditorWindow).Assembly;
            return asm.GetType("UnityEditor." + shortName, false, true);
        }

        // Resolve the window type for a friendly name. GameView / SceneView / InspectorWindow
        // are internal in UnityEditor, so this uses reflection by name (no project types).
        public static System.Type ResolveWindowType(string name, out string resolved)
        {
            resolved = name;
            switch (name.ToLowerInvariant())
            {
                case "game": resolved = "GameView"; break;
                case "scene": resolved = "SceneView"; break;
                case "inspector": resolved = "InspectorWindow"; break;
                case "project": resolved = "ProjectBrowser"; break;
                case "hierarchy": resolved = "SceneHierarchyWindow"; break;
                case "console": resolved = "ConsoleWindow"; break;
                case "profiler": resolved = "ProfilerWindow"; break;
                default: break;
            }
            return EditorType(resolved);
        }

        public static UnityEngine.Camera FindGameCamera(out string source)
        {
            source = "none";
            var cam = UnityEngine.Camera.main;
            if (cam != null) { source = "Camera.main"; return cam; }
            foreach (var c in UnityEngine.Camera.allCameras)
            {
                if (c != null && c.cameraType == UnityEngine.CameraType.Game && c.isActiveAndEnabled)
                {
                    source = "allCameras:" + c.name;
                    return c;
                }
            }
            var go = UnityEngine.GameObject.Find("Camera");
            if (go != null)
            {
                var c2 = go.GetComponent<UnityEngine.Camera>();
                if (c2 != null) { source = "GameObject.Find('Camera')"; return c2; }
            }
            // Edit mode fallback: a scene camera may be disabled, so scan the scene too.
            foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Camera>(
                UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
            {
                if (c != null && c.cameraType == UnityEngine.CameraType.Game)
                {
                    source = "scene:" + c.name;
                    return c;
                }
            }
            return null;
        }

        public static string Fmt(UnityEngine.Vector3 v)
        {
            return string.Format("({0:F2},{1:F2},{2:F2})", v.x, v.y, v.z);
        }

        // SceneView keeps (pivot, rotation, size), while its actual camera sits at
        // pivot + rotation * (0,0,-cameraDistance). Deriving from sv.camera.transform is
        // unreliable before the view has rendered (it can still be (0,0,0)); this uses the
        // public cameraDistance property and is the exact inverse of LookAt.
        public static UnityEngine.Vector3 SceneViewCameraPose(UnityEditor.SceneView sv)
        {
            return sv.pivot + sv.rotation * new UnityEngine.Vector3(0, 0, -sv.cameraDistance);
        }

        // In play mode the runtime camera may be owned by a project-side controller whose
        // Update() overwrites transform. If that controller exposes the Automation API
        // (ApplyPose/ReadPose), call it by reflection so the sync is not immediately undone.
        // This stays generic: no reference to any project type.
        static System.Reflection.MethodInfo FindApplyPose(UnityEngine.Camera cam, out object target)
        {
            target = null;
            if (cam == null) return null;
            foreach (var mb in cam.GetComponents<UnityEngine.MonoBehaviour>())
            {
                if (mb == null) continue;
                var m = mb.GetType().GetMethod("ApplyPose",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                    null, new[] { typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3) }, null);
                if (m != null) { target = mb; return m; }
            }
            return null;
        }

        public static string SetGamePose(UnityEngine.Vector3 pos, UnityEngine.Vector3 euler, out UnityEngine.Camera cam)
        {
            string source;
            cam = FindGameCamera(out source);
            if (cam == null) return "ERROR: no game camera (" + source + ")";
            // Only a live (play-mode) controller should receive ApplyPose; in edit mode
            // its CameraState is uninitialized and the plain transform is authoritative.
            if (UnityEditor.EditorApplication.isPlaying)
            {
                object ctrl;
                var apply = FindApplyPose(cam, out ctrl);
                if (apply != null)
                {
                    apply.Invoke(ctrl, new object[] { pos, euler });
                    return string.Format("game camera '{0}' via controller.ApplyPose pos={1} euler={2}",
                        cam.name, Fmt(pos), Fmt(euler));
                }
            }
            cam.transform.position = pos;
            cam.transform.eulerAngles = euler;
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(cam.gameObject.scene);
            return string.Format("game camera '{0}' transform pos={1} euler={2} (edit mode, scene marked dirty)",
                cam.name, Fmt(pos), Fmt(euler));
        }

        public static string ReadGamePose(out UnityEngine.Vector3 pos, out UnityEngine.Vector3 euler, out UnityEngine.Camera cam)
        {
            string source;
            cam = FindGameCamera(out source);
            pos = UnityEngine.Vector3.zero;
            euler = UnityEngine.Vector3.zero;
            if (cam == null) return "ERROR: no game camera (" + source + ")";
            // In play mode the controller's target state is authoritative (transform may
            // lag behind due to interpolation). In EDIT mode the controller is not enabled,
            // so its CameraState has never been initialized from the transform and still
            // holds (0,0,0)/(0,0,0); always read transform there.
            if (UnityEditor.EditorApplication.isPlaying && cam.GetComponent<UnityEngine.MonoBehaviour>() != null)
            {
                foreach (var mb in cam.GetComponents<UnityEngine.MonoBehaviour>())
                {
                    if (mb == null || !mb.enabled) continue;
                    var read = mb.GetType().GetMethod("ReadPose",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (read == null) continue;
                    var pars = read.GetParameters();
                    if (pars.Length != 2) continue;
                    var args = new object[] { UnityEngine.Vector3.zero, UnityEngine.Vector3.zero };
                    read.Invoke(mb, args);
                    pos = (UnityEngine.Vector3)args[0];
                    euler = (UnityEngine.Vector3)args[1];
                    return "controller.ReadPose on " + cam.name;
                }
            }
            pos = cam.transform.position;
            euler = cam.transform.eulerAngles;
            return "transform on " + cam.name + " (" + source + ")";
        }
    }

    internal class WindowInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            var focused = UnityEditor.EditorWindow.focusedWindow;
            sb.AppendLine("focused=" + (focused == null ? "NONE" : focused.GetType().FullName + " '" + focused.titleContent.text + "'"));
            var mouseOver = UnityEditor.EditorWindow.mouseOverWindow;
            sb.AppendLine("mouseOver=" + (mouseOver == null ? "NONE" : mouseOver.GetType().FullName));
            sb.AppendLine("playing=" + UnityEditor.EditorApplication.isPlaying);
            foreach (var w in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>())
            {
                if (w == null) continue;
                sb.AppendLine("  " + w.GetType().FullName + " '" + w.titleContent.text + "'");
            }
            return BoxedValue.FromString(sb.ToString());
        }
    }

    internal class WindowFocusExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.FromString("ERROR: window_focus(name) - game/scene/inspector/project/hierarchy/console/profiler or a UnityEditor window type name");
            string name = operands[0].GetString();
            string resolved;
            var type = EditorWindowCamUtil.ResolveWindowType(name, out resolved);
            if (type == null)
                return BoxedValue.FromString("ERROR: unknown window type 'UnityEditor." + resolved + "'");
            var win = UnityEditor.EditorWindow.GetWindow(type);
            if (win == null)
                return BoxedValue.FromString("ERROR: GetWindow failed for " + type.FullName);
            win.Focus();
            win.Repaint();
            return BoxedValue.FromString(string.Format("window_focus: '{0}' -> {1} focused={2}",
                name, type.FullName, UnityEditor.EditorWindow.focusedWindow == win));
        }
    }

    internal class CamSyncInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            sb.AppendLine("playing=" + UnityEditor.EditorApplication.isPlaying);
            if (sv == null)
                sb.AppendLine("sceneView=NONE");
            else
                sb.AppendLine(string.Format("sceneView pivot={0} rotationEuler={1} size={2:F2}",
                    EditorWindowCamUtil.Fmt(sv.pivot), EditorWindowCamUtil.Fmt(sv.rotation.eulerAngles), sv.size));
            UnityEngine.Vector3 pos, euler;
            UnityEngine.Camera cam;
            string how = EditorWindowCamUtil.ReadGamePose(out pos, out euler, out cam);
            sb.AppendLine(cam == null ? "gameCamera=NONE (" + how + ")"
                : string.Format("gameCamera '{0}' pos={1} euler={2} via {3}",
                    cam.name, EditorWindowCamUtil.Fmt(pos), EditorWindowCamUtil.Fmt(euler), how));
            if (sv != null)
            {
                var sp = EditorWindowCamUtil.SceneViewCameraPose(sv);
                sb.AppendLine(string.Format("sceneCam computed pos={0} euler={1} (from pivot/rotation/size)",
                    EditorWindowCamUtil.Fmt(sp), EditorWindowCamUtil.Fmt(sv.rotation.eulerAngles)));
                if (sv.camera != null)
                    sb.AppendLine(string.Format("sceneCam raw transform pos={0} euler={1}",
                        EditorWindowCamUtil.Fmt(sv.camera.transform.position),
                        EditorWindowCamUtil.Fmt(sv.camera.transform.eulerAngles)));
            }
            return BoxedValue.FromString(sb.ToString());
        }
    }

    // SceneView <- Game camera. SceneView's pose is (pivot, rotation, size): the actual
    // camera sits behind the pivot. Keep the pivot in front of the game camera by the
    // current view size, and copy its rotation, so both look at the same thing.
    internal class SceneCamFromGameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            if (sv == null) return BoxedValue.FromString("ERROR: no active SceneView");
            UnityEngine.Vector3 pos, euler;
            UnityEngine.Camera cam;
            string how = EditorWindowCamUtil.ReadGamePose(out pos, out euler, out cam);
            if (cam == null) return BoxedValue.FromString("ERROR: " + how);
            var rot = UnityEngine.Quaternion.Euler(euler);
            float size = sv.size;
            // Put the pivot in front of the game camera by the SceneView's own camera
            // distance, so the SceneView camera lands exactly at `pos`.
            // Set pivot/rotation/size DIRECTLY: LookAt() writes the animated target while
            // the pivot getter returns the current value, so the sync would lag a frame
            // (or several) and report the stale pose.
            var pivot = pos + rot * UnityEngine.Vector3.forward * sv.cameraDistance;
            sv.pivot = pivot;
            sv.rotation = rot;
            sv.size = size;
            sv.Repaint();
            return BoxedValue.FromString(string.Format("scene_cam_from_game: source={0} game pos={1} euler={2} -> pivot={3} size={4:F2}",
                how, EditorWindowCamUtil.Fmt(pos), EditorWindowCamUtil.Fmt(euler),
                EditorWindowCamUtil.Fmt(sv.pivot), sv.size));
        }
    }

    // Game camera <- SceneView. Use the SceneView camera's actual transform so the game
    // camera lands exactly where the editor view currently is.
    internal class GameCamFromSceneExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sv = UnityEditor.SceneView.lastActiveSceneView;
            if (sv == null) return BoxedValue.FromString("ERROR: no active SceneView");
            var pos = EditorWindowCamUtil.SceneViewCameraPose(sv);
            var euler = sv.rotation.eulerAngles;
            UnityEngine.Camera cam;
            string result = EditorWindowCamUtil.SetGamePose(pos, euler, out cam);
            return BoxedValue.FromString("game_cam_from_scene: " + result);
        }
    }

    // ---------- Focus BOTH views on a named scene object ----------
    internal static class CamFocusUtil
    {
        public static UnityEngine.GameObject Find(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            var go = UnityEngine.GameObject.Find(name);
            if (go != null) return go;
            // GameObject.Find only sees active objects and needs a full path for children.
            // Fall back to a name match over all transforms (including inactive).
            foreach (var t in UnityEngine.Object.FindObjectsByType<UnityEngine.Transform>(
                UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
            {
                if (t != null && t.name == name) return t.gameObject;
            }
            return null;
        }

        public static UnityEngine.Bounds BoundsOf(UnityEngine.GameObject go, out int rendererCount)
        {
            rendererCount = 0;
            UnityEngine.Bounds b = new UnityEngine.Bounds();
            bool has = false;
            foreach (var r in go.GetComponentsInChildren<UnityEngine.Renderer>(true))
            {
                if (r == null) continue;
                if (!has) { b = r.bounds; has = true; }
                else b.Encapsulate(r.bounds);
                ++rendererCount;
            }
            if (!has)
                b = new UnityEngine.Bounds(go.transform.position, UnityEngine.Vector3.one);
            return b;
        }

        public static string Focus(string name, float padding, bool sceneOnly, out string report)
        {
            var sb = new StringBuilder();
            var go = Find(name);
            if (go == null)
            {
                report = "ERROR: object '" + name + "' not found in the open scene";
                return report;
            }
            int n;
            var bounds = BoundsOf(go, out n);
            sb.AppendLine(string.Format("target='{0}' path={1} renderers={2} center={3} size={4}",
                go.name, PathOf(go.transform), n, EditorWindowCamUtil.Fmt(bounds.center),
                EditorWindowCamUtil.Fmt(bounds.size)));

            var sv = UnityEditor.SceneView.lastActiveSceneView;
            if (sv != null)
            {
                // Frame() accounts for the SceneView's FOV/aspect and picks the right size.
                var padded = new UnityEngine.Bounds(bounds.center, bounds.size * (padding > 0 ? padding : 1.2f));
                sv.Frame(padded, true);
                sv.Repaint();
                sb.AppendLine(string.Format("sceneView: pivot={0} euler={1} size={2:F2}",
                    EditorWindowCamUtil.Fmt(sv.pivot), EditorWindowCamUtil.Fmt(sv.rotation.eulerAngles), sv.size));
            }
            else sb.AppendLine("sceneView: NONE");

            if (!sceneOnly)
            {
                if (sv == null)
                    sb.AppendLine("gameCamera: skipped (no SceneView to derive pose from)");
                else
                {
                    UnityEngine.Camera cam;
                    string r = EditorWindowCamUtil.SetGamePose(
                        EditorWindowCamUtil.SceneViewCameraPose(sv), sv.rotation.eulerAngles, out cam);
                    sb.AppendLine("gameCamera: " + r);
                }
            }
            else sb.AppendLine("gameCamera: untouched (sceneOnly=true)");
            report = sb.ToString();
            return report;
        }

        public static string PathOf(UnityEngine.Transform t)
        {
            var parts = new List<string>();
            while (t != null) { parts.Add(t.name); t = t.parent; }
            parts.Reverse();
            return string.Join("/", parts.ToArray());
        }
    }

    internal class CamFocusInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1) return BoxedValue.FromString("ERROR: cam_focus_info(object_name)");
            var go = CamFocusUtil.Find(operands[0].GetString());
            if (go == null) return BoxedValue.FromString("ERROR: object '" + operands[0].GetString() + "' not found");
            int n;
            var b = CamFocusUtil.BoundsOf(go, out n);
            return BoxedValue.FromString(string.Format(
                "target='{0}' path={1} renderers={2} center={3} size={4} active={5}",
                go.name, CamFocusUtil.PathOf(go.transform), n,
                EditorWindowCamUtil.Fmt(b.center), EditorWindowCamUtil.Fmt(b.size), go.activeInHierarchy));
        }
    }

    internal class CamFocusObjectExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.FromString("ERROR: cam_focus_object(object_name[,padding,sceneOnly])");
            float padding = operands.Count > 1 ? operands[1].GetFloat() : 1.2f;
            bool sceneOnly = operands.Count > 2 && operands[2].GetBool();
            string report;
            CamFocusUtil.Focus(operands[0].GetString(), padding, sceneOnly, out report);
            return BoxedValue.FromString(report);
        }
    }

    // Report the APIs registered on the CURRENT interpreter, mirroring BatchCommand's
    // HelpAllExp (BatchScript.ApiDocs => Calculator.ApiDocs). AbstractExpression exposes a
    // protected `Calculator` property, so no reflection and no shared/global registry is
    // needed - and, unlike DslCalculatorHost.GetSharedApiRegistry(), this is guaranteed to
    // be the exact Calculator that is executing this call (the editor channel builds its
    // own registry via NewApiRegistry()).
    // api_list(pattern, ...) - regex search over all api docs, like helpall().
    internal class ApiListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var regexes = new List<System.Text.RegularExpressions.Regex>();
            foreach (var op in operands)
            {
                string pattern = op.ToString();
                if (!string.IsNullOrEmpty(pattern))
                    regexes.Add(new System.Text.RegularExpressions.Regex(pattern,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            }
            var docs = calculator.ApiDocs;
            var sb = new StringBuilder();
            int total = docs == null ? 0 : docs.Count;
            int shown = 0;
            sb.AppendLine(string.Format("api_list: channel=editor source=Calculator.ApiDocs total={0} patterns={1}",
                total, regexes.Count));
            if (docs != null)
            {
                foreach (var pair in docs)
                {
                    bool match = regexes.Count == 0;
                    string info = string.Format("{0}: {1}", pair.Key, pair.Value);
                    foreach (var regex in regexes)
                    {
                        if (regex.IsMatch(info)) { match = true; break; }
                    }
                    if (match)
                    {
                        sb.AppendLine(info);
                        ++shown;
                    }
                }
            }
            sb.AppendLine("shown=" + shown);
            return BoxedValue.FromString(sb.ToString());
        }
    }

    // ===========================================================================
    // Editor-channel component add/remove.
    //
    // The runtime GM registry already has editor_add_component/remove_component
    // (ClientGmStorySystem.cs / GeneralCommands.cs), but those call GameObject.AddComponent
    // and Object.Destroy directly: no Undo, no dry run, and they are unavailable on the
    // editor channel (which has its own Calculator). Editing scene objects needs reversible
    // changes, so these editor-side versions use Undo.AddComponent /
    // Undo.DestroyObjectImmediate and default to a dry run.
    // ===========================================================================
    internal static class EditorComponentUtil
    {
        // Kept local so this generic package layer does not depend on the project-specific
        // ray-tracing diagnostics helper (RtMaterialUtil lives in UCEditorDslApi.cs).
        public static string PathOf(UnityEngine.Transform t)
        {
            var parts = new List<string>();
            while (t != null) { parts.Add(t.name); t = t.parent; }
            parts.Reverse();
            return string.Join("/", parts.ToArray());
        }

        public static System.Type ResolveType(BoxedValue op, out string label)
        {
            label = "";
            try
            {
                var o = op.ObjectVal;
                var t = o as System.Type;
                if (t != null) { label = t.FullName; return t; }
            }
            catch (System.Exception) { }
            string name = "";
            try { name = op.ToString(); } catch (System.Exception) { }
            if (string.IsNullOrEmpty(name)) return null;
            label = name;

            // Try the name as given first (handles fully qualified names such as
            // "UnityEngine.BoxCollider" without prepending a namespace twice), then the
            // common short-name assemblies, then a full AppDomain scan for project/package
            // and editor types.
            var direct = System.Type.GetType(name);
            if (direct != null) return direct;
            foreach (var asmName in new[] { "UnityEngine", "UnityEngine.UI", "UnityEngine.CoreModule",
                                            "Assembly-CSharp", "Assembly-CSharp-Editor" })
            {
                var t = System.Type.GetType(name + ", " + asmName);
                if (t != null) return t;
                if (!name.Contains("."))
                {
                    t = System.Type.GetType((asmName.StartsWith("UnityEngine.UI") ? "UnityEngine.UI." : "UnityEngine.") + name + ", " + asmName);
                    if (t != null) return t;
                }
            }
            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Type[] types;
                try { types = a.GetTypes(); } catch (System.Reflection.ReflectionTypeLoadException e) { types = e.Types; }
                catch (System.Exception) { continue; }
                if (types == null) continue;
                foreach (var t in types)
                {
                    if (t == null) continue;
                    if (t.FullName == name || t.Name == name) return t;
                }
            }
            return null;
        }

        public static UnityEngine.GameObject ResolveGameObject(BoxedValue op, out string path)
        {
            path = "";
            try
            {
                var o = op.ObjectVal;
                var go = o as UnityEngine.GameObject;
                if (go != null) { path = EditorComponentUtil.PathOf(go.transform); return go; }
                var comp = o as UnityEngine.Component;
                if (comp != null) { path = EditorComponentUtil.PathOf(comp.transform); return comp.gameObject; }
            }
            catch (System.Exception) { }
            string s = "";
            try { s = op.ToString(); } catch (System.Exception) { }
            if (string.IsNullOrEmpty(s)) return null;
            var found = UnityEngine.GameObject.Find(s);
            if (found != null) { path = EditorComponentUtil.PathOf(found.transform); return found; }
            // GameObject.Find misses inactive objects and children addressed by name only.
            foreach (var t in UnityEngine.Object.FindObjectsByType<UnityEngine.Transform>(
                UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
            {
                if (t == null) continue;
                string p = EditorComponentUtil.PathOf(t);
                if (t.name == s || p == s || p.EndsWith("/" + s, System.StringComparison.Ordinal))
                { path = p; return t.gameObject; }
            }
            return null;
        }

        public static string ListComponents(UnityEngine.GameObject go)
        {
            var sb = new StringBuilder();
            var comps = go.GetComponents<UnityEngine.Component>();
            sb.AppendLine(string.Format("components on '{0}': {1}", EditorComponentUtil.PathOf(go.transform), comps.Length));
            for (int i = 0; i < comps.Length; ++i)
            {
                var c = comps[i];
                sb.AppendLine(string.Format("  [{0}] {1} enabled={2}", i,
                    c == null ? "<missing script>" : c.GetType().FullName,
                    c == null ? "n/a" : (c is UnityEngine.Behaviour ? ((UnityEngine.Behaviour)c).enabled.ToString() : "n/a")));
            }
            return sb.ToString();
        }

        public static string Add(BoxedValue objOp, BoxedValue typeOp, bool dryRun)
        {
            string path;
            var go = ResolveGameObject(objOp, out path);
            if (go == null) return "ERROR: GameObject not found";
            string typeLabel;
            var t = ResolveType(typeOp, out typeLabel);
            if (t == null) return "ERROR: type not found: " + typeLabel;
            if (!typeof(UnityEngine.Component).IsAssignableFrom(t))
                return "ERROR: '" + t.FullName + "' is not a UnityEngine.Component";
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("editor_add_component: '{0}' type='{1}' dryRun={2}", path, t.FullName, dryRun));
            var existing = go.GetComponents(t);
            sb.AppendLine("  existing instances of this type: " + existing.Length);
            if (dryRun)
            {
                sb.AppendLine(ListComponents(go));
                return sb.ToString() + "  (DRY RUN - nothing written; pass dryRun=false to apply)";
            }
            var comp = UnityEditor.Undo.AddComponent(go, t);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
            sb.AppendLine(comp == null ? "  FAILED to add component" : "  ADDED " + comp.GetType().FullName + " (Undo-recorded, NOT saved)");
            return sb.ToString();
        }

        public static string Remove(BoxedValue objOp, BoxedValue typeOp, bool dryRun, bool all)
        {
            string path;
            var go = ResolveGameObject(objOp, out path);
            if (go == null) return "ERROR: GameObject not found";
            string typeLabel;
            var t = ResolveType(typeOp, out typeLabel);
            if (t == null) return "ERROR: type not found: " + typeLabel;
            var comps = go.GetComponents(t);
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("remove_component: '{0}' type='{1}' matches={2} all={3} dryRun={4}",
                path, t.FullName, comps.Length, all, dryRun));
            if (comps.Length == 0) return sb.ToString() + "  nothing to remove";
            // Transform is not removable; guard against destroying the object's core.
            var targets = new List<UnityEngine.Component>();
            foreach (var c in comps)
            {
                if (c == null || c is UnityEngine.Transform) continue;
                targets.Add(c);
                if (!all) break;
            }
            sb.AppendLine(ListComponents(go));
            if (dryRun)
                return sb.ToString() + string.Format("  would remove {0} component(s) (DRY RUN - nothing written)", targets.Count);
            foreach (var c in targets)
            {
                sb.AppendLine("  REMOVED " + c.GetType().FullName);
                UnityEditor.Undo.DestroyObjectImmediate(c);
            }
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
            sb.AppendLine("  Undo-recorded; NOT saved to assets");
            return sb.ToString();
        }
    }

    internal class EditorListComponentsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1) return BoxedValue.FromString("ERROR: editor_list_components(obj_or_path)");
            string path;
            var go = EditorComponentUtil.ResolveGameObject(operands[0], out path);
            if (go == null) return BoxedValue.FromString("ERROR: GameObject not found");
            return BoxedValue.FromString(EditorComponentUtil.ListComponents(go));
        }
    }

    internal class EditorAddComponentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.FromString("ERROR: editor_add_component(obj_or_path,type_or_str[,dryRun])");
            bool dry = operands.Count < 3 || operands[2].GetBool();
            return BoxedValue.FromString(EditorComponentUtil.Add(operands[0], operands[1], dry));
        }
    }

    internal class EditorRemoveComponentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.FromString("ERROR: remove_component(obj_or_path,type_or_str[,dryRun,all])");
            bool dry = operands.Count < 3 || operands[2].GetBool();
            bool all = operands.Count > 3 && operands[3].GetBool();
            return BoxedValue.FromString(EditorComponentUtil.Remove(operands[0], operands[1], dry, all));
        }
    }

    // Generic player build support. Project-specific preparation can be injected via
    // prepareHook (see uc_local_build_player on the project side); the generic package
    // layer itself only uses UnityEditor.BuildPipeline and knows nothing about UC.
    [Serializable]
    public sealed class BuildStateData
    {
        public string status = "idle";
        public string platform = "";
        public string output = "";
        public bool development = false;
        public bool append = false;
        public string scenes = "";
        public string startedAt = "";
        public string finishedAt = "";
        public string result = "";
        public string error = "";
    }

    public static class BuildPlayerUtil
    {
        private static string StatePath
        {
            get { return Path.GetFullPath(Path.Combine(UnityEngine.Application.dataPath, "../Library/build_state.json")); }
        }

        public static BuildStateData ReadState()
        {
            try
            {
                if (File.Exists(StatePath))
                {
                    var st = JsonUtility.FromJson<BuildStateData>(File.ReadAllText(StatePath));
                    if (st != null)
                        return st;
                }
            }
            catch (Exception) { }
            return new BuildStateData();
        }

        public static void WriteState(BuildStateData st)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(StatePath));
                File.WriteAllText(StatePath, JsonUtility.ToJson(st, true));
            }
            catch (Exception e)
            {
                Debug.LogError("[build_player] failed to persist build state: " + e.Message);
            }
        }

        // A queued build cannot survive a domain reload (platform switch or recompile).
        // Persisted pending/running states are marked interrupted on the next load.
        [InitializeOnLoadMethod]
        private static void MarkInterruptedOnReload()
        {
            var st = ReadState();
            if (st.status == "pending" || st.status == "running")
            {
                st.status = "interrupted";
                st.error = "editor domain reload/restart during build; re-issue build_player()";
                st.finishedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WriteState(st);
            }
        }

        public static bool ResolveTarget(string platform, out BuildTarget target, out BuildTargetGroup group, out string label)
        {
            target = BuildTarget.StandaloneOSX;
            group = BuildTargetGroup.Standalone;
            label = "";
            switch ((platform ?? "").ToLowerInvariant().Replace("-", "").Replace("_", ""))
            {
                case "mac":
                case "macos":
                case "osx":
                case "standaloneosx":
                    target = BuildTarget.StandaloneOSX;
                    group = BuildTargetGroup.Standalone;
                    label = "mac";
                    return true;
                case "ios":
                case "iphone":
                    target = BuildTarget.iOS;
                    group = BuildTargetGroup.iOS;
                    label = "ios";
                    return true;
                default:
                    return false;
            }
        }

        public static string DefaultOutput(string label)
        {
            var rel = label == "mac" ? "Build/Mac/UCRayTracing.app" : "Build/iOS";
            return Path.GetFullPath(Path.Combine(UnityEngine.Application.dataPath, "..", rel));
        }

        public static void Schedule(BuildStateData st, BuildTarget target, BuildOptions options)
        {
            st.status = "pending";
            st.startedAt = "";
            st.finishedAt = "";
            st.result = "";
            st.error = "";
            WriteState(st);
            EditorApplication.delayCall += () => RunBuild(target, options);
        }

        private static void RunBuild(BuildTarget target, BuildOptions options)
        {
            var st = ReadState();
            st.status = "running";
            st.startedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WriteState(st);
            try
            {
                var scenes = (st.scenes ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Directory.CreateDirectory(Path.GetDirectoryName(st.output));
                var bp = new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = st.output,
                    target = target,
                    options = options
                };
                var report = BuildPipeline.BuildPlayer(bp);
                st.result = report.summary.result.ToString();
                st.status = st.result == "Succeeded" ? "succeeded" : "failed";
                st.error = report.summary.totalErrors > 0
                    ? string.Format("errors={0} warnings={1} size={2}", report.summary.totalErrors, report.summary.totalWarnings, report.summary.totalSize)
                    : "";
            }
            catch (Exception e)
            {
                st.status = "failed";
                st.error = e.GetType().Name + ": " + e.Message;
            }
            st.finishedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WriteState(st);
            Debug.Log("[build_player] " + st.status + " -> " + st.output + " " + st.error);
        }

        // Shared plan/validation/scheduling for all player build commands.
        // prepareHook(dryRun) is optional and supplied by project-specific commands.
        public static string Request(string platform, string outDir, bool development, bool dryRun, string scenesArg, Func<bool, string> prepareHook, bool append)
        {
            BuildTarget target;
            BuildTargetGroup group;
            string label;
            if (!ResolveTarget(platform, out target, out group, out label))
                return "ERROR: unknown platform '" + platform + "'; use \"mac\" or \"ios\".";

            var active = EditorUserBuildSettings.activeBuildTarget;
            bool supported = BuildPipeline.IsBuildTargetSupported(group, target);
            var output = string.IsNullOrEmpty(outDir) ? DefaultOutput(label) : Path.GetFullPath(outDir);

            var scenes = new List<string>();
            string scenesFrom;
            if (!string.IsNullOrEmpty(scenesArg))
            {
                foreach (var s in scenesArg.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    scenes.Add(s.Trim());
                scenesFrom = "argument";
            }
            else
            {
                foreach (var s in EditorBuildSettings.scenes)
                    if (s.enabled) scenes.Add(s.path);
                scenesFrom = "EditorBuildSettings(enabled)";
            }

            var sb = new StringBuilder();
            sb.AppendFormat("dryRun={0} requested={1} target={2} active={3} supported={4} development={5} append={6}\n",
                dryRun, platform, target, active, supported, development, append);
            sb.AppendFormat("output={0}\nscenes({1}, from={2}): {3}\n",
                output, scenes.Count, scenesFrom, string.Join(", ", scenes.ToArray()));
            if (prepareHook != null)
                sb.Append(prepareHook(dryRun)).Append("\n");
            if (append)
                sb.AppendFormat("append=True (BuildOptions.AcceptExternalModificationsToPlayer): keeps manual edits in the existing Xcode project; requires an appendable target at {0}.\n", output);

            if (!supported)
                sb.Append("BLOCKED: playback engine for this target is not installed.\n");
            if (active != target)
                sb.AppendFormat("BLOCKED: active target is {0}; call switch_build_target(\"{1}\") first, wait for the domain reload, then re-issue this command.\n",
                    active, label == "mac" ? "StandaloneOSX" : "iOS");
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                sb.Append("BLOCKED: exit Play mode before building.\n");
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                sb.Append("BLOCKED: editor is compiling/importing; retry when idle.\n");
            if (scenes.Count == 0)
                sb.Append("BLOCKED: no enabled scenes; enable scenes in Build Settings or pass the scenes argument.\n");
            var existing = ReadState();
            if (existing.status == "pending" || existing.status == "running")
                sb.Append("BLOCKED: another build is pending/running; poll build_state().\n");

            if (dryRun || sb.ToString().Contains("BLOCKED:"))
            {
                sb.Append("NOTE: dryRun defaults to true; pass dryRun=false to queue the build. Poll build_state().");
                return sb.ToString();
            }

            var options = BuildOptions.None;
            if (development)
                options |= BuildOptions.Development | BuildOptions.AllowDebugging;
            if (append)
                options |= BuildOptions.AcceptExternalModificationsToPlayer;
            EditorUserBuildSettings.development = development;
            var state = new BuildStateData
            {
                platform = label,
                output = output,
                development = development,
                append = append,
                scenes = string.Join(",", scenes.ToArray())
            };
            Schedule(state, target, options);
            sb.Append("SCHEDULED: BuildPlayer queued via EditorApplication.delayCall.\n");
            sb.Append("The build blocks the editor main thread; the GM socket may be unreachable until it finishes.\n");
            sb.Append("Result is persisted in Library/build_state.json - poll build_state() after reconnecting.");
            return sb.ToString();
        }
    }

    internal class BuildPlayerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.FromString("ERROR: build_player(platform[,outDir[,development[,dryRun[,scenes[,append]]]]]) platform: \"mac\" or \"ios\"");
            var platform = operands[0].GetString();
            var outDir = operands.Count > 1 ? operands[1].GetString() : "";
            bool development = operands.Count > 2 && operands[2].GetBool();
            bool dryRun = operands.Count < 4 || operands[3].GetBool();
            var scenes = operands.Count > 4 ? operands[4].GetString() : "";
            bool append = operands.Count > 5 && operands[5].GetBool();
            return BoxedValue.FromString(BuildPlayerUtil.Request(platform, outDir, development, dryRun, scenes, null, append));
        }
    }

    internal class BuildStateExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var st = BuildPlayerUtil.ReadState();
            var sb = new StringBuilder();
            sb.AppendFormat("status={0} platform={1} development={2} append={3}\n", st.status, st.platform, st.development, st.append);
            sb.AppendFormat("output={0}\nscenes={1}\n", st.output, st.scenes);
            sb.AppendFormat("started={0} finished={1} result={2}\n", st.startedAt, st.finishedAt, st.result);
            if (!string.IsNullOrEmpty(st.error))
                sb.AppendFormat("error={0}\n", st.error);
            sb.Append("stateFile=Library/build_state.json (persists across domain reloads)");
            return BoxedValue.FromString(sb.ToString());
        }
    }

#endif
    public sealed class EditorDslApi
    {
        public static void Register(DslCalculator calculator)
        {
            calculator.Register("get_object_by_id", "get_object_by_id(instance_id) api", new ExpressionFactoryHelper<GetObjectByIdExp>());
            calculator.Register("get_object_id", "get_object_id(obj) api", new ExpressionFactoryHelper<GetObjectIdExp>());
            calculator.Register("asset_path_2_guid", "asset_path_2_guid(asset_path) api", new ExpressionFactoryHelper<AssetPath2GUIDExp>());
            calculator.Register("guid_2_asset_path", "guid_2_asset_path(guid) api", new ExpressionFactoryHelper<GUID2AssetPathExp>());
            calculator.Register("get_asset_path", "get_asset_path(obj) api", new ExpressionFactoryHelper<GetAssetPathExp>());
            calculator.Register("get_guid_and_file_id", "get_guid_and_file_id(obj) api, return KeyValuaPair<string,long>", new ExpressionFactoryHelper<GetGuidAndLocalFileIdentifierExp>());
            calculator.Register("get_dependencies", "get_dependencies(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDependenciesExp>());
            calculator.Register("get_direct_dependencies", "get_direct_dependencies(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDirectDependenciesExp>());
            calculator.Register("get_dependencies_graph", "get_dependencies_graph(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDependenciesGraphExp>());
            calculator.Register("get_asset_importer", "get_asset_importer(path) api", new ExpressionFactoryHelper<GetAssetImporterExp>());
            calculator.Register("load_asset", "load_asset(asset_path) api", new ExpressionFactoryHelper<LoadAssetExp>());
            calculator.Register("load_assets", "load_assets(asset_path) api", new ExpressionFactoryHelper<LoadAssetsExp>());
            calculator.Register("unload_asset", "unload_asset(obj) api", new ExpressionFactoryHelper<UnloadAssetExp>());
            calculator.Register("add_object_to_asset", "add_object_to_asset(obj) api", new ExpressionFactoryHelper<AddObjectToAssetExp>());
            calculator.Register("remove_object_from_asset", "remove_object_from_asset(obj) api", new ExpressionFactoryHelper<RemoveObjectFromAssetExp>());
            calculator.Register("from_json_overwrite", "from_json_overwrite(json,obj) api", new ExpressionFactoryHelper<FromJsonOverwriteExp>());
            calculator.Register("to_json", "to_json(obj[,pretty]) api", new ExpressionFactoryHelper<ToJsonExp>());
            calculator.Register("get_string_in_length", "get_string_in_length(str,len[,get_the_end]) api", new ExpressionFactoryHelper<GetStringInLengthExp>());
            calculator.Register("get_prefab_type", "get_prefab_type(obj) api", new ExpressionFactoryHelper<GetPrefabTypeExp>());
            calculator.Register("get_prefab_status", "get_prefab_status(obj) api", new ExpressionFactoryHelper<GetPrefabStatusExp>());
            calculator.Register("get_prefab_object", "get_prefab_object(obj) api", new ExpressionFactoryHelper<GetPrefabObjectExp>());
            calculator.Register("get_prefab_parent", "get_prefab_parent(obj) api", new ExpressionFactoryHelper<GetPrefabParentExp>());
            calculator.Register("get_prefab_parent_at_path", "get_prefab_parent_at_path(obj) api", new ExpressionFactoryHelper<GetPrefabParentAtPathExp>());
            calculator.Register("get_prefab_parent_from_origin", "get_prefab_parent_from_origin(obj) api", new ExpressionFactoryHelper<GetPrefabParentFromOriginExp>());
            calculator.Register("get_nearst_prefab_root", "get_nearst_prefab_root(obj) api", new ExpressionFactoryHelper<GetNearstPrefabRootExp>());
            calculator.Register("get_nearst_prefab_root_asset", "get_nearst_prefab_root_asset(obj) api", new ExpressionFactoryHelper<GetNearstPrefabRootAssetExp>());
            calculator.Register("get_outermost_prefab_root", "get_outermost_prefab_root(obj) api", new ExpressionFactoryHelper<GetOutermostPrefabRootExp>());
            calculator.Register("get_original_prefab_root", "get_original_prefab_root(obj) api", new ExpressionFactoryHelper<GetOriginalPrefabRootWhereAddedExp>());
            calculator.Register("get_prefab_overrides", "get_prefab_overrides(obj,include_def_overrides) api", new ExpressionFactoryHelper<GetPrefabOverridesExp>());
            calculator.Register("find_prefab_instances", "find_prefab_instances(obj) api", new ExpressionFactoryHelper<FindPrefabInstancesExp>());
            calculator.Register("find_prefab_instances_in_scene", "find_prefab_instances_in_scene(obj,scene) api", new ExpressionFactoryHelper<FindPrefabInstancesInSceneExp>());
            calculator.Register("scan_prefab", "scan_prefab(obj) api", new ExpressionFactoryHelper<ScanPrefabExp>());
            calculator.Register("scan_dependency", "scan_dependency(prefab_a,prefab_b) api", new ExpressionFactoryHelper<ScanDependencyExp>());
            calculator.Register("check_internal_dependency", "check_internal_dependency(prefab_a,prefab_b[include_root_comp,include_child_gobj]) api", new ExpressionFactoryHelper<CheckInternalDependencyExp>());
            calculator.Register("convert_dump_bin", "convert_dump_bin(bin_file,txt_file) api", new ExpressionFactoryHelper<ConvertDumpBinExp>());
            calculator.Register("display_progress_bar", "display_progress_bar(title,text,progress) api", new ExpressionFactoryHelper<DisplayProgressBarExp>());
            calculator.Register("display_cancelable_progress_bar", "display_cancelable_progress_bar(title,text,progress) api", new ExpressionFactoryHelper<DisplayCancelableProgressBarExp>());
            calculator.Register("clear_progress_bar", "clear_progress_bar() api", new ExpressionFactoryHelper<ClearProgressBarExp>());
            calculator.Register("open_with_default_app", "open_with_default_app(file) api", new ExpressionFactoryHelper<OpenWithDefaultAppExp>());
            calculator.Register("open_file_panel", "open_file_panel(title,dir,ext) api", new ExpressionFactoryHelper<OpenFilePanelExp>());
            calculator.Register("open_file_panel_with_filters", "open_file_panel_with_filters(title,dir,filter1,filter2,...) api", new ExpressionFactoryHelper<OpenFilePanelWithFiltersExp>());
            calculator.Register("open_folder_panel", "open_folder_panel(title,dir,def) api", new ExpressionFactoryHelper<OpenFolderPanelExp>());
            calculator.Register("save_file_panel", "save_file_panel(title,dir,def,ext) api", new ExpressionFactoryHelper<SaveFilePanelExp>());
            calculator.Register("save_file_panel_in_project", "save_file_panel_in_project(title,def,ext,msg[,path]) api", new ExpressionFactoryHelper<SaveFilePanelInProjectExp>());
            calculator.Register("save_folder_panel", "save_folder_panel(title,dir,def) api", new ExpressionFactoryHelper<SaveFolderPanelExp>());
            calculator.Register("display_dialog", "display_dialog(title,msg,ok[,cancel[,alt]]) api", new ExpressionFactoryHelper<DisplayDialogExp>());
            calculator.Register("has_cmd_arg", "has_cmd_arg(name) api, return (bool,string)", new ExpressionFactoryHelper<HasCmdArgExp>());
            calculator.Register("str_len", "str_len(str) api", new ExpressionFactoryHelper<StrLenExp>());
			calculator.Register("strlen", "strlen(str) api", new ExpressionFactoryHelper<StrLenExp>());
			
#if UNITY_EDITOR
            calculator.Register("build_target", "build_target() - report active build target/group, iOS playback engine availability and editor state (read-only)", new ExpressionFactoryHelper<BuildTargetInfoExp>());
            calculator.Register("switch_build_target", "switch_build_target(name) - switch to iOS | StandaloneOSX on the next editor update (edit mode only, async; triggers domain reload / reimport)", new ExpressionFactoryHelper<SwitchBuildTargetExp>());
            calculator.Register("editor_play", "editor_play(play) - enter/exit Play mode on the next editor update; returns the state before the change", new ExpressionFactoryHelper<EditorPlayExp>());
            calculator.Register("editor_isplaying", "editor_isplaying() - current Play mode state", new ExpressionFactoryHelper<EditorIsPlayingExp>());
            calculator.Register("gfx_format_probe", "gfx_format_probe() - report graphics format capabilities, URP HDR format and B10G11R11 render textures", new ExpressionFactoryHelper<GfxFormatProbeExp>());
            calculator.Register("editor_play_now", "editor_play_now() - enter Play mode immediately without delayCall; returns previous state", new ExpressionFactoryHelper<EditorPlayNowExp>());
            calculator.Register("editor_stop_now", "editor_stop_now() - exit Play mode immediately without delayCall; returns previous state", new ExpressionFactoryHelper<EditorStopNowExp>());
            calculator.Register("scene_cam_info", "scene_cam_info() - generic edit-mode SceneView camera pose/size (read-only)", new ExpressionFactoryHelper<SceneCamInfoExp>());
            calculator.Register("scene_cam_set", "scene_cam_set(x,y,z[,pitch,yaw,roll]) - set SceneView pivot/rotation", new ExpressionFactoryHelper<SceneCamSetExp>());
            calculator.Register("scene_cam_move", "scene_cam_move(dx,dy,dz[,local]) - translate SceneView pivot", new ExpressionFactoryHelper<SceneCamMoveExp>());
            calculator.Register("scene_cam_look", "scene_cam_look(x,y,z) - aim SceneView at a world point", new ExpressionFactoryHelper<SceneCamLookExp>());
            calculator.Register("scene_cam_sweep", "scene_cam_sweep(enable[,degPerSec,radius,cx,cy,cz]) - continuous non-blocking SceneView sweep", new ExpressionFactoryHelper<SceneCamSweepExp>());
            calculator.Register("scene_info", "scene_info() - report active/open scenes and Play state (read-only)", new ExpressionFactoryHelper<SceneInfoExp>());
            calculator.Register("scene_open", "scene_open(asset_path[,single]) - open a scene asset in edit mode; refuses while playing", new ExpressionFactoryHelper<SceneOpenExp>());
            calculator.Register("window_info", "window_info() - list open editor windows and the focused window (read-only)", new ExpressionFactoryHelper<WindowInfoExp>());
            calculator.Register("window_focus", "window_focus(name) - activate game/scene/inspector/project/hierarchy/console/profiler or a UnityEditor window type", new ExpressionFactoryHelper<WindowFocusExp>());
            calculator.Register("cam_sync_info", "cam_sync_info() - report SceneView and game-camera poses side by side (read-only)", new ExpressionFactoryHelper<CamSyncInfoExp>());
            calculator.Register("scene_cam_from_game", "scene_cam_from_game() - align SceneView to the game camera", new ExpressionFactoryHelper<SceneCamFromGameExp>());
            calculator.Register("game_cam_from_scene", "game_cam_from_scene() - align game camera to the SceneView (uses controller.ApplyPose in play mode)", new ExpressionFactoryHelper<GameCamFromSceneExp>());
            calculator.Register("cam_focus_info", "cam_focus_info(object_name) - report a scene object's path and renderer bounds (read-only)", new ExpressionFactoryHelper<CamFocusInfoExp>());
            calculator.Register("cam_focus_object", "cam_focus_object(object_name[,padding,sceneOnly]) - frame an object in SceneView and align the game camera to the same view", new ExpressionFactoryHelper<CamFocusObjectExp>());
            calculator.Register("api_list", "api_list([regex,...]) - regex search over all APIs/docs registered on the current editor interpreter (like helpall)", new ExpressionFactoryHelper<ApiListExp>());
            calculator.Register("editor_list_components", "editor_list_components(obj_or_path) - list components on a scene GameObject, including inactive objects (read-only)", new ExpressionFactoryHelper<EditorListComponentsExp>());
            calculator.Register("editor_add_component", "editor_add_component(obj_or_path,type_or_str[,dryRun]) - add a component in edit mode; defaults to DRY RUN, uses Undo.AddComponent, never saves the scene", new ExpressionFactoryHelper<EditorAddComponentExp>());
            calculator.Register("editor_remove_component", "editor_remove_component(obj_or_path,type_or_str[,dryRun,all]) - remove component(s) in edit mode; defaults to DRY RUN, uses Undo.DestroyObjectImmediate, never saves the scene", new ExpressionFactoryHelper<EditorRemoveComponentExp>());
            calculator.Register("build_player", "build_player(platform[,outDir[,development[,dryRun[,scenes[,append]]]]]) - build a mac or iOS player with Unity BuildPipeline; platform: \"mac\"|\"ios\"; append=1 keeps manual edits in an existing Xcode project (AcceptExternalModificationsToPlayer); dryRun defaults to true; real build is queued asynchronously, poll build_state()", new ExpressionFactoryHelper<BuildPlayerExp>());
            calculator.Register("build_state", "build_state() - report the last/queued player build (persisted in Library/build_state.json, survives domain reloads)", new ExpressionFactoryHelper<BuildStateExp>());
#endif
#if USE_GM_STORY
            calculator.Register("story_var", "story_var(name,val) or story_var(name) api", new ExpressionFactoryHelper<StoryVarExp>());
            calculator.Register("call_story", "call_story(msgId...) or call_story(apiOrMsgId(...)) api", new ExpressionFactoryHelper<CallStoryExp>());
#endif
        }
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
#endregion
