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
using GameFramework.GmCommands;
using StorySystem;

#region interpreter
#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace DslExpression
{
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
                    Resources.UnloadAsset(obj);
                }
            }
            return BoxedValue.NullObject;
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
    internal class StoryValueExp : AbstractExpression
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
                var exp = StoryFunctionManager.Instance.CreateFunction(param);
                m_Values.Add(exp);
            }
            return true;
        }

        private List<IStoryFunction> m_Values = new List<IStoryFunction>();
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
                var cmd = StoryCommandManager.Instance.CreateCommand(param);
                m_Commands.Add(cmd);
            }
            return true;
        }

        private List<IStoryCommand> m_Commands = new List<IStoryCommand>();
    }
#endif
    public sealed class UnityEditorApi
    {
        public static void Register(DslCalculator calculator)
        {
            calculator.Register("assetpath2guid", "assetpath2guid(asset_path) api", new ExpressionFactoryHelper<AssetPath2GUIDExp>());
            calculator.Register("guid2assetpath", "guid2assetpath(guid) api", new ExpressionFactoryHelper<GUID2AssetPathExp>());
            calculator.Register("getassetpath", "getassetpath(obj) api", new ExpressionFactoryHelper<GetAssetPathExp>());
            calculator.Register("getguidandfileid", "getguidandfileid(obj) api, return KeyValuaPair<string,long>", new ExpressionFactoryHelper<GetGuidAndLocalFileIdentifierExp>());
            calculator.Register("getdependencies", "getdependencies(list_or_str1,list_or_str2,...) api, return string[]", new ExpressionFactoryHelper<GetDependenciesExp>());
            calculator.Register("getassetimporter", "getassetimporter(path) api", new ExpressionFactoryHelper<GetAssetImporterExp>());
            calculator.Register("loadasset", "loadasset(asset_path) api", new ExpressionFactoryHelper<LoadAssetExp>());
            calculator.Register("unloadasset", "unloadasset(obj) api", new ExpressionFactoryHelper<UnloadAssetExp>());
            calculator.Register("getprefabtype", "getprefabtype(obj) api", new ExpressionFactoryHelper<GetPrefabTypeExp>());
            calculator.Register("getprefabstatus", "getprefabstatus(obj) api", new ExpressionFactoryHelper<GetPrefabStatusExp>());
            calculator.Register("getprefabobject", "getprefabobject(obj) api", new ExpressionFactoryHelper<GetPrefabObjectExp>());
            calculator.Register("getprefabparent", "getprefabparent(obj) api", new ExpressionFactoryHelper<GetPrefabParentExp>());
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
#if USE_GM_STORY
            calculator.Register("storyvar", "storyvar(name,val) or storyvar(name) api", new ExpressionFactoryHelper<StoryVarExp>());
            calculator.Register("storyvalue", "storyvalue(code1,code2,...) api", new ExpressionFactoryHelper<StoryValueExp>());
            calculator.Register("storycommand", "storycommand(code1,code2,...) api", new ExpressionFactoryHelper<StoryCommandExp>());
#endif
        }
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
#endregion
