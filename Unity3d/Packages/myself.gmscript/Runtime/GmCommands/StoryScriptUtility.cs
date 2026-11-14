using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using StoryScript;

public static partial class StoryScriptUtility
{
    public static Transform FindChildRecursive(Transform parent, string bonePath)
    {
        Transform t = parent.Find(bonePath);
        if (null != t) {
            return t;
        }
        else {
            int ct = parent.childCount;
            for (int i = 0; i < ct; ++i) {
                t = FindChildRecursive(parent.GetChild(i), bonePath);
                if (null != t) {
                    return t;
                }
            }
        }
        return null;
    }
    public static T TryInstallComponent<T>(GameObject prefab) where T : Component
    {
        if (null == prefab) {
            return null;
        }
        UnityEngine.Debug.Assert(!prefab.activeSelf);

        // idempotent: reuse an existing component instead of adding a duplicate,
        // so calling TryInstall twice never yields two instances
        var compObj = prefab.GetComponent<T>();
        if (null == compObj) {
            // prefab root is inactive, so Awake/OnEnable/Start have not run yet
            compObj = prefab.AddComponent<T>();
        }
        return compObj;
    }
    public static Dictionary<string, Transform> CollectChildrenByName(Transform root)
    {
        var map = new Dictionary<string, Transform>();
        if (null == root) {
            return map;
        }
        // GetComponentsInChildren<Transform>(true) also reaches inactive children
        var children = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < children.Length; ++i) {
            var t = children[i];
            if (t == root) {
                continue;
            }
            // on name conflict keep the shallowest node; on equal depth keep the
            // first visited, so the result does not depend on unique names
            if (map.TryGetValue(t.name, out var exist) && GetTransformDepth(t) >= GetTransformDepth(exist)) {
                continue;
            }
            map[t.name] = t;
        }
        return map;
    }
    private static int GetTransformDepth(Transform t)
    {
        int depth = 0;
        for (var p = t.parent; p != null; p = p.parent) {
            ++depth;
        }
        return depth;
    }
    public static bool IsPathMatch(UnityEngine.Transform tr, IList<string> names)
    {
        int ix = names.Count - 1;
        var p = tr;
        while (null != p && ix >= 0) {
            if (p.name == names[ix]) {
                --ix;
            }
            p = p.parent;
        }
        return ix < 0;
    }
    public static string GetScenePath(UnityEngine.Transform tr, int up_level)
    {
        var sb = new StringBuilder();
        var stack = new Stack<string>();
        int lvl = 0;
        while (null != tr && lvl <= up_level) {
            stack.Push(tr.name);
            tr = tr.parent;
            ++lvl;
        }
        while (stack.Count > 0) {
            sb.Append('/');
            sb.Append(stack.Pop());
        }
        return sb.ToString();
    }
    public static GameObject GetGameObjectArg(ref BoxedValue arg)
    {
        GameObject gobj = null;
        if (arg.IsObject) {
            var gcomp = arg.ObjectVal as Component;
            if (null != gcomp) {
                gobj = gcomp.gameObject;
            }
            else {
                gobj = arg.ObjectVal as GameObject;
            }
        }
        else if (arg.IsString) {
            gobj = GameObject.Find(arg.AsString);
        }
        return gobj;
    }
    public static Renderer GetRendererArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var r = arg.ObjectVal as Renderer;
            if (null != r)
                return r;
        }
        GameObject gobj = GetGameObjectArg(ref arg);
        if (null != gobj) {
            var r = gobj.GetComponent<Renderer>();
            if (null == r) {
                r = gobj.GetComponent<MeshRenderer>();
            }
            if (null == r) {
                r = gobj.GetComponent<SkinnedMeshRenderer>();
            }
            return r;
        }
        return null;
    }
    public static Mesh GetMeshArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var m = arg.ObjectVal as Mesh;
            if (null != m)
                return m;
        }
        GameObject gobj = GetGameObjectArg(ref arg);
        if (null != gobj) {
            var mf = gobj.GetComponent<MeshFilter>();
            if (null != mf) {
                var m = mf.sharedMesh;
                if (null != m) {
                    return m;
                }
                else {
                    return mf.mesh;
                }
            }
            else {
                var r = gobj.GetComponent<SkinnedMeshRenderer>();
                if (null != r) {
                    return r.sharedMesh;
                }
            }
        }
        return null;
    }
    public static Material GetMaterialArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var m = arg.ObjectVal as Material;
            if (null != m)
                return m;
        }
        var r = GetRendererArg(ref arg);
        if (null != r) {
            return r.material;
        }
        return null;
    }
    public static Material[] GetMaterialsArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var ms = arg.ObjectVal as Material[];
            if (null != ms)
                return ms;
        }
        var r = GetRendererArg(ref arg);
        if (null != r) {
            return r.materials;
        }
        return null;
    }
    public static Material GetSharedMaterialArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var m = arg.ObjectVal as Material;
            if (null != m)
                return m;
        }
        var r = GetRendererArg(ref arg);
        if (null != r) {
            return r.sharedMaterial;
        }
        return null;
    }
    public static Material[] GetSharedMaterialsArg(ref BoxedValue arg)
    {
        if (arg.IsObject) {
            var ms = arg.ObjectVal as Material[];
            if (null != ms)
                return ms;
        }
        var r = GetRendererArg(ref arg);
        if (null != r) {
            return r.sharedMaterials;
        }
        return null;
    }
    public static Color GetColor(string name)
    {
        var fi = typeof(Color).GetField(name);
        if (null != fi) {
            return (Color)fi.GetValue(null);
        }
        return Color.black;
    }

    public static void DrawGizmosCircle(Vector3 center, float radius, int step = 16)
    {
        for (int i = 1; i < step + 1; i++) {
            Vector3 pos0 = center + Quaternion.AngleAxis(360.0f * (i - 1) / step, Vector3.up) * Vector3.forward * radius;
            Vector3 pos1 = center + Quaternion.AngleAxis(360.0f * i / step, Vector3.up) * Vector3.forward * radius;
            Gizmos.DrawLine(pos0, pos1);
        }
    }
    public static void DrawGizmosArraw(Vector3 start, Vector3 end, float size = 0.5f)
    {
        Vector3 dir = (end - start).normalized;
        float length = (end - start).magnitude;
        Gizmos.DrawLine(start, end);
        Vector3 left = Quaternion.AngleAxis(45, Vector3.up) * (-dir);
        Gizmos.DrawLine(end, end + left * size);
        Vector3 right = Quaternion.AngleAxis(-45, Vector3.up) * (-dir);
        Gizmos.DrawLine(end, end + right * size);
    }
    public static void DestroyObject(UnityEngine.Object obj)
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

    public static void SendMessage(string objname, string msg, object arg)
    {
        SendMessage(objname, msg, arg, false);
    }
    public static void SendMessage(string objname, string msg, object arg, bool needReceiver)
    {
        SendMessageImpl(objname, msg, arg, needReceiver);
    }
    public static void SendMessageWithTag(string objtag, string msg, object arg)
    {
        SendMessageWithTag(objtag, msg, arg, false);
    }
    public static void SendMessageWithTag(string objtag, string msg, object arg, bool needReceiver)
    {
        SendMessageWithTagImpl(objtag, msg, arg, needReceiver);
    }

    public static bool IsNamespace(string ns, out int id)
    {
        if (s_Namespaces.Count == 0) {
            CollectNamespaces();
        }
        return s_Namespace2Ids.TryGetValue(ns, out id);
    }
    public static string GetNamespace(int id)
    {
        if (id >= 0 && id < s_Namespaces.Count) {
            return s_Namespaces[id];
        }
        return string.Empty;
    }
    public static void ImportNamespace(string ns, string assembly)
    {
        s_ImportedNamespaces.Add(new KeyValuePair<string, string>(ns, assembly));
    }
    public static void UnImportNamespace(string ns, string assembly)
    {
        for (int ix = s_ImportedNamespaces.Count - 1; ix >= 0; --ix) {
            var kv = s_ImportedNamespaces[ix];
            if (kv.Key == ns && kv.Value == assembly) {
                s_ImportedNamespaces.RemoveAt(ix);
            }
        }
    }
    public static Type GetType(string type)
    {
        Type ret = null;
        try {
            ret = Type.GetType("UnityEngine." + type + ", UnityEngine");
            if (null == ret) {
                ret = Type.GetType("UnityEngine.UI." + type + ", UnityEngine.UI");
            }
            if (null == ret) {
                ret = Type.GetType(type + ", Assembly-CSharp");
            }
            if (null == ret) {
                ret = Type.GetType(type);
            }
            if (null == ret) {
                foreach (var pair in s_ImportedNamespaces) {
                    string ns = pair.Key.Trim();
                    string assembly = pair.Value.Trim();
                    string prefix = string.IsNullOrEmpty(ns) ? string.Empty : ns + ".";
                    string suffix = string.IsNullOrEmpty(assembly) ? string.Empty : ", " + assembly;
                    ret = Type.GetType(prefix + type + suffix);
                    if (null != ret) {
                        break;
                    }
                }
            }
            if (null == ret) {
                Debug.LogWarningFormat("null == Type.GetType({0})", type);
            }
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
        return ret;
    }
    private static void CollectNamespaces()
    {
        int nextIndex = 0;
        var assems = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assem in assems) {
            if (assem.IsDynamic) {
                var sb = new StringBuilder();
                sb.AppendFormat("dynmaic assembly: {0}", assem);
                var dynatypes = assem.GetTypes();
                foreach (var dtype in dynatypes) {
                    sb.AppendLine();
                    sb.AppendFormat("\ttype: {0}", dtype);
                }
                LogSystem.Warn(sb.ToString());
                continue;
            }
            var types = assem.GetExportedTypes();
            foreach (var type in types) {
                string nss = type.Namespace;
                if (!string.IsNullOrEmpty(nss)) {
                    for (int ix = 0; ix < nss.Length;) {
                        int nix = nss.IndexOf('.', ix);
                        if (nix > ix) {
                            string ns = nss.Substring(ix, nix - ix);
                            if (s_Namespace2Ids.TryAdd(ns, nextIndex)) {
                                ++nextIndex;
                                s_Namespaces.Add(ns);
                            }
                            ix = nix + 1;
                        }
                        else {
                            string ns = nss.Substring(ix);
                            if (s_Namespace2Ids.TryAdd(ns, nextIndex)) {
                                ++nextIndex;
                                s_Namespaces.Add(ns);
                            }
                            break;
                        }
                    }
                }
                if (s_Namespace2Ids.TryAdd(type.Name, nextIndex)) {
                    ++nextIndex;
                    s_Namespaces.Add(type.Name);
                }
            }
        }
    }

    private static void SendMessageImpl(string objname, string msg, object arg, bool needReceiver)
    {
        GameObject obj = GameObject.Find(objname);
        if (null != obj) {
            try {
                obj.SendMessage(msg, arg, needReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver);
                if (msg.CompareTo("LogToConsole") != 0)
                    Debug.LogFormat("SendMessage {0} {1} {2} {3}", objname, msg, arg, needReceiver);
            }
            catch (Exception ex) {
                Debug.LogErrorFormat("SendMessage({0} {1} {2} {3}) Exception {4}\n{5}", objname, msg, arg, needReceiver, ex.Message, ex.StackTrace);
            }
        }
    }
    private static void SendMessageWithTagImpl(string objtag, string msg, object arg, bool needReceiver)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(objtag);
        if (null != objs) {
            for (int i = 0; i < objs.Length; i++) {
                try {
                    objs[i].SendMessage(msg, arg, needReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver);
                    if (msg.CompareTo("LogToConsole") != 0)
                        Debug.LogFormat("SendMessageWithTag {0} {1} {2} {3}", objtag, msg, arg, needReceiver);
                }
                catch (Exception ex) {
                    Debug.LogErrorFormat("SendMessageWithTag({0} {1} {2} {3}) Exception {4}\n{5}", objtag, msg, arg, needReceiver, ex.Message, ex.StackTrace);
                }
            }
        }
    }

    private static Dictionary<string, int> s_Namespace2Ids = new Dictionary<string, int>();
    private static List<string> s_Namespaces = new List<string>();
    private static List<KeyValuePair<string, string>> s_ImportedNamespaces = new List<KeyValuePair<string, string>>();
}

internal static class Literal
{
    internal static string GetIndentString(int indent)
    {
        PrepareIndent(indent);
        return s_IndentString.Substring(0, indent);
    }
    internal static string GetSpaceString(int indent)
    {
        PrepareSpace(indent * c_IndentSpaceString.Length);
        return s_SpaceString.Substring(0, indent * c_IndentSpaceString.Length);
    }

    private static void PrepareIndent(int indent)
    {
        if (s_IndentString.Length < indent) {
            int len = c_InitCount;
            while (len < indent) {
                len *= 2;
            }
            s_IndentString = new string('\t', indent);
        }
    }
    private static void PrepareSpace(int indent)
    {
        if (s_SpaceString.Length < indent) {
            int len = c_InitCount;
            while (len < indent) {
                len *= 2;
            }
            while (s_SpaceBuilder.Length < len) {
                s_SpaceBuilder.Append(c_IndentSpaceString);
            }
            s_SpaceString = s_SpaceBuilder.ToString();
        }
    }

    private static string s_IndentString = string.Empty;
    private static string s_SpaceString = string.Empty;
    private static StringBuilder s_SpaceBuilder = new StringBuilder();

    private const int c_InitCount = 256;
    private const string c_IndentSpaceString = "| ";
}
internal static class StringBuilderExtension
{
    public static void Append(this StringBuilder sb, string fmt, params object[] args)
    {
        if (args.Length == 0) {
            sb.Append(fmt);
        }
        else {
            sb.AppendFormat(fmt, args);
        }
    }
    public static void AppendLine(this StringBuilder sb, string fmt, params object[] args)
    {
        if (args.Length == 0) {
            sb.AppendLine(fmt);
        }
        else {
            sb.AppendFormat(fmt, args);
            sb.AppendLine();
        }
    }
}
