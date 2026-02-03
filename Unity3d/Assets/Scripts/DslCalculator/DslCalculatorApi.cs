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
using ScriptableFramework;
using ScriptableFramework.GmCommands;
using StoryScript;

#region interpreter
#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace StoryScript.DslExpression
{
    internal class GetObjectByIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string[] ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i)
                {
                    var str = operands[i].AsString;
                    if (null != str)
                    {
                        list.Add(str);
                    }
                    else
                    {
                        var strList = operands[i].As<IList>();
                        if (null != strList)
                        {
                            foreach (var strObj in strList)
                            {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    list.Add(tempStr);
                            }
                        }
                    }
                }
                if (list.Count == 1)
                {
                    ret = AssetDatabase.GetDependencies(list[0], false);
                }
                else if (list.Count > 1)
                {
                    ret = AssetDatabase.GetDependencies(list.ToArray(), false);
                }
            }
#endif
            return BoxedValue.FromObject(ret);
        }
    }
    internal class GetDependenciesGraphExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            string[] ret = null;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var list = new List<string>();
                for (int i = 0; i < operands.Count; ++i)
                {
                    var str = operands[i].AsString;
                    if (null != str)
                    {
                        list.Add(str);
                    }
                    else
                    {
                        var strList = operands[i].As<IList>();
                        if (null != strList)
                        {
                            foreach (var strObj in strList)
                            {
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
                foreach (var str in list)
                {
                    queue.Enqueue(Tuple.Create(str, string.Empty));
                }
                while (queue.Count > 0)
                {
                    var tuple = queue.Dequeue();
                    string asset = tuple.Item1;
                    string path = tuple.Item2 + "->" + tuple.Item1;
                    if (accessed.Contains(asset))
                    {
                        results.Add(path);
                    }
                    else
                    {
                        accessed.Add(asset);
                        var deps = AssetDatabase.GetDependencies(asset, false);
                        if (deps.Length > 0)
                        {
                            foreach (var dep in deps)
                            {
                                queue.Enqueue(Tuple.Create(dep, path));
                            }
                        }
                        else
                        {
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
    internal class UnloadAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count >= 1) {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj) {
                    var t = obj.GetType();
                    if (typeof(GameObject).IsAssignableFrom(t) || typeof(Component).IsAssignableFrom(t) || typeof(AssetBundle).IsAssignableFrom(t) || typeof(ScriptableObject).IsAssignableFrom(t)) {
#if UNITY_EDITOR
                        if (!PrefabUtility.IsPartOfPrefabAsset(obj)) {
                            DestroyObject(obj);
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
        private static void DestroyObject(UnityEngine.Object obj)
        {
            if (obj != null) {
#if UNITY_EDITOR
                if (Application.isPlaying && !UnityEditor.EditorApplication.isPaused)
                    UnityEngine.Object.Destroy(obj);
                else
                    UnityEngine.Object.DestroyImmediate(obj, false);
#else
                UnityEngine.Object.DestroyImmediate(obj);
#endif
            }
        }
    }
    internal class GetPrefabTypeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2)
            {
                var obj = operands[0].As<UnityEngine.Object>();
                var assetPath = operands[1].AsString;
                if (null != obj && !string.IsNullOrEmpty(assetPath))
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetCorrespondingObjectFromSourceAtPath(obj, assetPath));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabParentFromOriginExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetNearstPrefabRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetNearestPrefabInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetNearstPrefabRootAssetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetOutermostPrefabRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.Object>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetOutermostPrefabInstanceRoot(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetOriginalPrefabRootWhereAddedExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetOriginalSourceRootWhereGameObjectIsAdded(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class GetPrefabOverridesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.GameObject>();
                bool includeDefaultOverrides = false;
                if(operands.Count >= 2)
                {
                    includeDefaultOverrides = operands[1].GetBool();
                }
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.GetObjectOverrides(obj, includeDefaultOverrides));
                }
            }
#endif
            return r;
        }
    }
    internal class FindPrefabInstancesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj)
                {
                    r = BoxedValue.FromObject(PrefabUtility.FindAllInstancesOfPrefab(obj));
                }
            }
#endif
            return r;
        }
    }
    internal class FindPrefabInstancesInSceneExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.GameObject>();
                var scene = operands[1].CastTo< UnityEngine.SceneManagement.Scene>();
                if (null != obj && scene.IsValid())
                {
                    r = BoxedValue.FromObject(PrefabUtility.FindAllInstancesOfPrefab(obj, scene));
                }
            }
#endif
            return r;
        }
    }
    internal class ScanPrefabExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 1)
            {
                var obj = operands[0].As<UnityEngine.GameObject>();
                if (null != obj)
                {
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
            if (!prefab)
            {
                prefab = obj;
                isInst = false;
            }
            if (null != prefab)
            {
                if (isInst)
                {
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
                if (rootNearst)
                {
                    sb.AppendFormat(" root:{0}", rootNearst.name);
                }
                if (rootOutermost)
                {
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
                if (isInst)
                {
                    sb.Append(" modifications:");
                    var modifications = PrefabUtility.GetPropertyModifications(prefab);
                    if (null != modifications)
                    {
                        foreach (var modification in modifications)
                        {
                            sb.AppendFormat(" {0}", modification.propertyPath);
                        }
                    }
                    sb.Append(" added_objs:");
                    var addObjs = PrefabUtility.GetAddedGameObjects(prefab);
                    if (null != addObjs)
                    {
                        foreach (var mobj in addObjs)
                        {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" removed_objs:");
                    var removeObjs = PrefabUtility.GetRemovedGameObjects(prefab);
                    if (null != removeObjs)
                    {
                        foreach (var mobj in removeObjs)
                        {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" added_comps:");
                    var addComps = PrefabUtility.GetAddedComponents(prefab);
                    if (null != addComps)
                    {
                        foreach (var mobj in addComps)
                        {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                    sb.Append(" removed_comps:");
                    var removeComps = PrefabUtility.GetRemovedComponents(prefab);
                    if (null != removeComps)
                    {
                        foreach (var mobj in removeObjs)
                        {
                            sb.AppendFormat(" {0}", mobj);
                        }
                    }
                }
                sb.Append(" comps:");
                foreach (var comp in obj.GetComponents<Component>())
                {
                    sb.AppendFormat(" {0}({1})", comp.name, comp.GetType().Name);
                }
                sb.AppendLine();
                for (int ix = 0; ix < obj.transform.childCount; ++ix)
                {
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2)
            {
                var prefabA = operands[0].AsString;
                var prefabB = operands[1].AsString;
                if (!string.IsNullOrEmpty(prefabA) && !string.IsNullOrEmpty(prefabB))
                {
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

            if (sourcePrefab == null || targetDependency == null)
            {
                sb.AppendLine("The path is incorrect; please check the path in the script.");
                return;
            }

            sb.AppendLine($"[Start] Checking if '{sourcePrefab.name}' depends on '{targetDependency.name}'...");

            GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(sourcePrefab);
            bool foundDependency = false;

            try
            {
                // PHASE 1: Hierarchy
                Transform[] allTransforms = instance.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in allTransforms)
                {
                    GameObject sourceAsset = PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject);

                    // Use Shared Utility
                    if (sourceAsset != null && IsAssetDerivedFrom(sourceAsset, targetPath))
                    {
                        sb.AppendLine();
                        sb.AppendLine($"    [Hierarchy Dependency] Node: '{GetHierarchyPath(t)}' is instance/variant of target.");
                        foundDependency = true;
                    }
                }

                // PHASE 2: Properties
                Component[] allComponents = instance.GetComponentsInChildren<Component>(true);
                foreach (Component comp in allComponents)
                {
                    if (comp == null)
                    {
                        continue;
                    }
                    SerializedObject so = new SerializedObject(comp);
                    SerializedProperty sp = so.GetIterator();

                    while (sp.Next(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            var refObj = sp.objectReferenceValue;
                            if (refObj == null)
                            {
                                continue;
                            }
                            if (IsSelfReference(refObj, instance.transform))
                            {
                                continue;
                            }
                            string refPath = AssetDatabase.GetAssetPath(refObj);
                            if (refPath == sourcePath)
                            {
                                continue;
                            }
                            // Use Shared Utility
                            if (IsObjectDerivedFrom(refObj, targetPath))
                            {
                                sb.AppendLine();
                                sb.AppendLine($"    [Property Dependency] Source: '{GetHierarchyPath(comp.transform)}' -> Property: '{sp.propertyPath}' refers to target.");
                                foundDependency = true;
                            }
                        }
                    }
                }

                sb.AppendLine();
                if (!foundDependency)
                {
                    sb.AppendLine($"    [Result] No dependency found.");
                }
                else
                {
                    sb.AppendLine($"    [Result] Dependencies found!");
                }
            }
            finally
            {
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
            if (obj is GameObject go)
            {
                return go.transform.IsChildOf(rootTransform);
            }
            else if (obj is Component comp)
            {
                return comp.transform.IsChildOf(rootTransform);
            }

            // Assets like Materials, Textures, etc., are not part of the hierarchy
            return false;
        }
        // Helper: Check if an object (Component or GameObject) belongs to an asset
        // that is (or inherits from) the target path.
        internal static bool IsObjectDerivedFrom(UnityEngine.Object obj, string targetPath)
        {
            if (obj == null)
            {
                return false;
            }

            // Get the path of the asset this object belongs to
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(objPath))
            {
                return false;
            }

            // 1. Direct path match (Fastest)
            if (objPath == targetPath)
            {
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
            if (assetToCheck == null)
            {
                return false;
            }

            string currentPath = AssetDatabase.GetAssetPath(assetToCheck);
            if (currentPath == targetPath)
            {
                return true;
            }

            GameObject basePrefab = PrefabUtility.GetCorrespondingObjectFromSource(assetToCheck);

            if (basePrefab == null)
            {
                return false;
            }
            if (basePrefab == assetToCheck)
            {
                return false;
            }
            return IsAssetDerivedFrom(basePrefab, targetPath);
        }
        // Helper: Check if a GameObject is the main root of its own asset file
        internal static bool IsRootOfItsAsset(GameObject go)
        {
            if (go == null)
            {
                return false;
            }
            string assetPath = AssetDatabase.GetAssetPath(go);
            GameObject mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as GameObject;
            return go == mainAsset;
        }
        internal static string GetHierarchyPath(Transform t)
        {
            if (t == null)
            {
                return "";
            }
            string path = t.name;
            while (t.parent != null && t.parent.parent != null)
            {
                t = t.parent;
                path = t.name + "/" + path;
            }
            return path;
        }
#endif
    }
    internal class CheckInternalDependencyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var r = BoxedValue.NullObject;
#if UNITY_EDITOR
            if (operands.Count >= 2)
            {
                var prefabA = operands[0].AsString;
                var prefabB = operands[1].AsString;
                bool checkRootObjComp = false;
                bool checkChildGameObjects = false;
                if (operands.Count >= 3)
                {
                    checkRootObjComp = operands[2].GetBool();
                }
                if (operands.Count >= 4)
                {
                    checkChildGameObjects = operands[3].GetBool();
                }
                if (!string.IsNullOrEmpty(prefabA) && !string.IsNullOrEmpty(prefabB))
                {
                    var sb = new StringBuilder();
                    bool foundIssue = CheckInternalDependency(prefabA, prefabB, checkRootObjComp, checkChildGameObjects, sb);
                    if (foundIssue)
                    {
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

            if (sourcePrefab == null || targetDependency == null)
            {
                return false;
            }

            // Load the Main Asset (Root GameObject) of B to distinguish between Root and Child
            GameObject targetRoot = AssetDatabase.LoadMainAssetAtPath(targetPath) as GameObject;
            if (targetRoot == null)
            {
                return false;
            }

            GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(sourcePrefab);
            bool foundIssue = false;

            try
            {
                Component[] allComponents = instance.GetComponentsInChildren<Component>(true);

                foreach (Component comp in allComponents)
                {
                    if (comp == null)
                    {
                        continue;
                    }

                    SerializedObject so = null;
                    try
                    {
                        so = new SerializedObject(comp);
                    }
                    catch(Exception ex)
                    {
                        Debug.LogErrorFormat("source:{0} target:{1} checkRoot:{2} checkChild:{3} comp:{4} exception:{5}\n{6}", sourcePath, targetPath, checkRootObjComp, checkChildGameObjects, ex.Message, ex.StackTrace);
                        so = null;
                    }
                    if (null == so)
                    {
                        continue;
                    }

                    SerializedProperty sp = so.GetIterator();

                    while (sp.Next(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            var refObj = sp.objectReferenceValue;

                            if (refObj == null)
                            {
                                continue;
                            }
                            if (ScanDependencyExp.IsSelfReference(refObj, instance.transform))
                            {
                                continue;
                            }
                            string refPath = AssetDatabase.GetAssetPath(refObj);
                            if (refPath == sourcePath)
                            {
                                continue;
                            }
                            // 1. Check if the referenced object is (or inherits from) the target Prefab
                            if (ScanDependencyExp.IsObjectDerivedFrom(refObj, targetPath))
                            {
                                bool shouldReport = false;
                                string referenceType = "";

                                // 2. Analyze the type of reference
                                if (refObj is Component refComp)
                                {
                                    if (ScanDependencyExp.IsRootOfItsAsset(refComp.gameObject)) {
                                        if (checkRootObjComp) {
                                            // Case A: Reference to a Component (Script, Transform, etc.)
                                            // Always report.
                                            shouldReport = true;
                                            referenceType = $"Component({refObj.GetType().Name})";
                                        }
                                    }
                                    else
                                    {
                                        // Case A: Reference to a Component (Script, Transform, etc.)
                                        // Always report.
                                        shouldReport = true;
                                        referenceType = $"Component({refObj.GetType().Name})";
                                    }
                                }
                                else if (refObj is GameObject refGo)
                                {
                                    // Case B: Reference to a GameObject
                                    // We need to check if it's the Root or a Child.

                                    // To do this accurately for Variants, we check if the referenced GO
                                    // corresponds to the Root of the asset it lives in.
                                    if (ScanDependencyExp.IsRootOfItsAsset(refGo))
                                    {
                                        // It is a Root GameObject (Safe dependency)
                                        // (Logic: pass)
                                    }
                                    else
                                    {
                                        // It is a Child GameObject (Sub-Asset)
                                        if (checkChildGameObjects)
                                        {
                                            shouldReport = true;
                                            referenceType = "Child_GameObject";
                                        }
                                    }
                                }

                                // 3. Log
                                if (shouldReport)
                                {
                                    if (foundIssue)
                                    {
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
            finally
            {
                GameObject.DestroyImmediate(instance);
            }
        }
#endif
    }
    internal class DisplayProgressBarExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            return true;
        }
    }
    internal class OpenWithDefaultAppExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
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
    internal class StoryFunctionExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = ClientGmStorySystem.Instance.GetStory("main");
            if (null == instance) {
                string txt = "script(main){onmessage(\"start\"){};};";
                ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                ClientGmStorySystem.Instance.StartStory("main");
                instance = ClientGmStorySystem.Instance.GetStory("main");
            }
            var handler = instance.GetMessageHandler("start");
            object ret = null;
            foreach (var exp in m_Values) {
                exp.Evaluate(instance, handler, BoxedValue.NullObject, null);
                if (exp.HaveValue) {
                    ret = exp.Value.GetObject();
                }
            }
            return BoxedValue.FromObject(ret);
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent param = callData.GetParam(ix);
                var exp = DotnetStoryScript.StoryFunctionManager.Instance.CreateFunction(param);
                m_Values.Add(exp);
            }
            return true;
        }

        private List<DotnetStoryScript.IStoryFunction> m_Values = new List<DotnetStoryScript.IStoryFunction>();
    }
    internal class StoryCommandExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = ClientGmStorySystem.Instance.GetStory("main");
            if (null == instance) {
                string txt = "script(main){onmessage(\"start\"){};};";
                ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes(txt));
                instance = ClientGmStorySystem.Instance.GetStory("main");
            }
            var handler = instance.GetMessageHandler("start");
            foreach (var cmd in m_Commands) {
                cmd.Reset();
            }
            foreach (var cmd in m_Commands) {
                cmd.Execute(instance, handler, 0, BoxedValue.NullObject, null);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int ix = 0; ix < num; ++ix) {
                Dsl.ISyntaxComponent param = callData.GetParam(ix);
                var cmd = DotnetStoryScript.StoryCommandManager.Instance.CreateCommand(param);
                m_Commands.Add(cmd);
            }
            return true;
        }

        private List<DotnetStoryScript.IStoryCommand> m_Commands = new List<DotnetStoryScript.IStoryCommand>();
    }
#endif
    public sealed class UnityEditorApi
    {
        public static void Register(DslCalculator calculator)
        {
            calculator.Register("getobjectbyid", "getobjectbyid(instance_id) api", new ExpressionFactoryHelper<GetObjectByIdExp>());
            calculator.Register("getobjectid", "getobjectid(obj) api", new ExpressionFactoryHelper<GetObjectIdExp>());
            calculator.Register("assetpath2guid", "assetpath2guid(asset_path) api", new ExpressionFactoryHelper<AssetPath2GUIDExp>());
            calculator.Register("guid2assetpath", "guid2assetpath(guid) api", new ExpressionFactoryHelper<GUID2AssetPathExp>());
            calculator.Register("getassetpath", "getassetpath(obj) api", new ExpressionFactoryHelper<GetAssetPathExp>());
            calculator.Register("getguidandfileid", "getguidandfileid(obj) api, return KeyValuaPair<string,long>", new ExpressionFactoryHelper<GetGuidAndLocalFileIdentifierExp>());
            calculator.Register("getdependencies", "getdependencies(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDependenciesExp>());
            calculator.Register("getdirectdependencies", "getdirectdependencies(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDirectDependenciesExp>());
            calculator.Register("getdependenciesgraph", "getdependenciesgraph(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDependenciesGraphExp>());
            calculator.Register("getassetimporter", "getassetimporter(path) api", new ExpressionFactoryHelper<GetAssetImporterExp>());
            calculator.Register("loadasset", "loadasset(asset_path) api", new ExpressionFactoryHelper<LoadAssetExp>());
            calculator.Register("unloadasset", "unloadasset(obj) api", new ExpressionFactoryHelper<UnloadAssetExp>());
            calculator.Register("getprefabtype", "getprefabtype(obj) api", new ExpressionFactoryHelper<GetPrefabTypeExp>());
            calculator.Register("getprefabstatus", "getprefabstatus(obj) api", new ExpressionFactoryHelper<GetPrefabStatusExp>());
            calculator.Register("getprefabobject", "getprefabobject(obj) api", new ExpressionFactoryHelper<GetPrefabObjectExp>());
            calculator.Register("getprefabparent", "getprefabparent(obj) api", new ExpressionFactoryHelper<GetPrefabParentExp>());
            calculator.Register("getprefabparentatpath", "getprefabparentatpath(obj) api", new ExpressionFactoryHelper<GetPrefabParentAtPathExp>());
            calculator.Register("getprefabparentfromorigin", "getprefabparentfromorigin(obj) api", new ExpressionFactoryHelper<GetPrefabParentFromOriginExp>());
            calculator.Register("getnearstprefabroot", "getnearstprefabroot(obj) api", new ExpressionFactoryHelper<GetNearstPrefabRootExp>());
            calculator.Register("getnearstprefabrootasset", "getnearstprefabrootasset(obj) api", new ExpressionFactoryHelper<GetNearstPrefabRootAssetExp>());
            calculator.Register("getoutermostprefabroot", "getoutermostprefabroot(obj) api", new ExpressionFactoryHelper<GetOutermostPrefabRootExp>());
            calculator.Register("getoriginalprefabroot", "getoriginalprefabroot(obj) api", new ExpressionFactoryHelper<GetOriginalPrefabRootWhereAddedExp>());
            calculator.Register("getprefaboverrides", "getprefaboverrides(obj,include_def_overrides) api", new ExpressionFactoryHelper<GetPrefabOverridesExp>());
            calculator.Register("findprefabinstances", "findprefabinstances(obj) api", new ExpressionFactoryHelper<FindPrefabInstancesExp>());
            calculator.Register("findprefabinstancesinscene", "findprefabinstancesinscene(obj,scene) api", new ExpressionFactoryHelper<FindPrefabInstancesInSceneExp>());
            calculator.Register("scanprefab", "scanprefab(obj) api", new ExpressionFactoryHelper<ScanPrefabExp>());
            calculator.Register("scandependency", "scandependency(prefab_a,prefab_b) api", new ExpressionFactoryHelper<ScanDependencyExp>());
            calculator.Register("checkinternaldependency", "checkinternaldependency(prefab_a,prefab_b[include_root_comp,include_child_gobj]) api", new ExpressionFactoryHelper<CheckInternalDependencyExp>());
            calculator.Register("displayprogressbar", "displayprogressbar(title,text,progress) api", new ExpressionFactoryHelper<DisplayProgressBarExp>());
            calculator.Register("displaycancelableprogressbar", "displaycancelableprogressbar(title,text,progress) api", new ExpressionFactoryHelper<DisplayCancelableProgressBarExp>());
            calculator.Register("clearprogressbar", "clearprogressbar() api", new ExpressionFactoryHelper<ClearProgressBarExp>());
            calculator.Register("openwithdefaultapp", "openwithdefaultapp(file) api", new ExpressionFactoryHelper<OpenWithDefaultAppExp>());
            calculator.Register("openfilepanel", "openfilepanel(title,dir,ext) api", new ExpressionFactoryHelper<OpenFilePanelExp>());
            calculator.Register("openfilepanelwithfilters", "openfilepanelwithfilters(title,dir,filter1,filter2,...) api", new ExpressionFactoryHelper<OpenFilePanelWithFiltersExp>());
            calculator.Register("openfolderpanel", "openfolderpanel(title,dir,def) api", new ExpressionFactoryHelper<OpenFolderPanelExp>());
            calculator.Register("savefilepanel", "savefilepanel(title,dir,def,ext) api", new ExpressionFactoryHelper<SaveFilePanelExp>());
            calculator.Register("savefilepanelinproject", "savefilepanelinproject(title,def,ext,msg[,path]) api", new ExpressionFactoryHelper<SaveFilePanelInProjectExp>());
            calculator.Register("savefolderpanel", "savefolderpanel(title,dir,def) api", new ExpressionFactoryHelper<SaveFolderPanelExp>());
            calculator.Register("displaydialog", "displaydialog(title,msg,ok[,cancel[,alt]]) api", new ExpressionFactoryHelper<DisplayDialogExp>());
            calculator.Register("hascmdarg", "hascmdarg(name) api, return (bool,string)", new ExpressionFactoryHelper<HasCmdArgExp>());
            calculator.Register("strlen", "strlen(str) api", new ExpressionFactoryHelper<StrLenExp>());
#if USE_GM_STORY
            calculator.Register("storyvar", "storyvar(name,val) or storyvar(name) api", new ExpressionFactoryHelper<StoryVarExp>());
            calculator.Register("storyfunction", "storyfunction(code1,code2,...) api", new ExpressionFactoryHelper<StoryFunctionExp>());
            calculator.Register("storycommand", "storycommand(code1,code2,...) api", new ExpressionFactoryHelper<StoryCommandExp>());
#endif
        }
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
#endregion
