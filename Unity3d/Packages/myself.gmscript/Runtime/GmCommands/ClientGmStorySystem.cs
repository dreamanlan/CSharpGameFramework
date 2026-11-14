using System;
using System.Collections.Generic;
using StoryScript;
using StoryScript.DslExpression;

namespace GmCommands
{
    public delegate void RegisterGmCommandsAndFunctionsDelegation(DslCalculatorApiRegistry registry);
    public sealed class ClientGmStorySystem
    {
        public RegisterGmCommandsAndFunctionsDelegation OnRegisterGmCommandsAndFunctions;
        public void Init()
        {
            if (m_Inited) {
                // The editor may initialize first (project-side delegate not set yet); apply it later on demand.
                RegisterExtraCommands();
                return;
            }
            m_Inited = true;
            try {
                var registry = DslCalculatorHost.GetSharedApiRegistry();

                RegisterCommon(registry);

                //register Gm command
                registry.Register("logcodenum", "logcodenum() command", new ExpressionFactoryHelper<LogCodeNumCommand>());
                registry.Register("help", "help() command", new ExpressionFactoryHelper<HelpCommand>());
                registry.Register("setdebug", "setdebug(1_or_0) command", new ExpressionFactoryHelper<SetDebugCommand>());
                registry.Register("startactivity", "startactivity(package_name[[,class_name[,flags]],extra_list_or_dict]) command", new ExpressionFactoryHelper<StartActivityCommand>());
                registry.Register("finishactivity", "finishactivity() command", new ExpressionFactoryHelper<FinishActivityCommand>());
                registry.Register("listenclipboard", "listenclipboard(interval) command", new ExpressionFactoryHelper<ListenClipboardCommand>());
                registry.Register("listenandroid", "listenandroid() command", new ExpressionFactoryHelper<ListenAndroidCommand>());
				registry.Register("listensocket", "listensocket(port) command", new ExpressionFactoryHelper<ListenSocketCommand>());
                registry.Register("startservice", "startservice(srv_class, extra_name, extra_val) command", new ExpressionFactoryHelper<StartServiceCommand>());
                registry.Register("stopservice", "stopservice(srv_class) command", new ExpressionFactoryHelper<StopServiceCommand>());
                registry.Register("shell", "shell(cmd) command", new ExpressionFactoryHelper<ShellCommand>());
                registry.Register("shelltimeout", "shelltimeout(cmd,ms) command", new ExpressionFactoryHelper<ShellTimeoutCommand>());
                registry.Register("cleanupcompletedtasks", "cleanupcompletedtasks() command", new ExpressionFactoryHelper<CleanupCompletedTasksCommand>());
                registry.Register("usejavatask", "usejavatask(is_java) command", new ExpressionFactoryHelper<UseJavaTaskCommand>());
                registry.Register("setclipboard", "setclipboard(text) command", new ExpressionFactoryHelper<SetClipboardCommand>());
                registry.Register("editorbreak", "editorbreak() command", new ExpressionFactoryHelper<EditorBreakCommand>());
                registry.Register("debugbreak", "debugbreak() command", new ExpressionFactoryHelper<DebugBreakCommand>());
                registry.Register("settimescale", "settimescale(scale) command", new ExpressionFactoryHelper<SetTimeScaleCommand>());
                registry.Register("clearglobals", "clearglobals() command", new ExpressionFactoryHelper<ClearGlobalsCommand>());

                registry.Register("supportsgf", "supportsgf() command, print unsupported graphics format", new ExpressionFactoryHelper<SupportsGfxFormatCommand>());
                registry.Register("supportstex", "supportstex() command, print unsupported tex", new ExpressionFactoryHelper<SupportsTexCommand>());
                registry.Register("supportsrt", "supportsrt() command, print unsupported rt", new ExpressionFactoryHelper<SupportsRTCommand>());
                registry.Register("supportsva", "supportsva() command, print unsupported vertex attribute format", new ExpressionFactoryHelper<SupportsVertexAttributeFormatCommand>());
                registry.Register("devicesupports", "devicesupports() command, print unsupported feature", new ExpressionFactoryHelper<DeviceSupportsCommand>());
                registry.Register("logresolutions", "logresolutions() command, print supported resolutions", new ExpressionFactoryHelper<LogResolutionsCommand>());

                registry.Register("createdir", "createdir(dir) command", new ExpressionFactoryHelper<CreateDirectoryCommand>());
                registry.Register("deletedir", "deletedir(dir) command", new ExpressionFactoryHelper<DeleteDirectoryCommand>());
                registry.Register("copydir", "copydir(src,dest,filter1,new_ext1,...) command", new ExpressionFactoryHelper<CopyDirectoryCommand>());
                registry.Register("copyfile", "copyfile(src,dest) command", new ExpressionFactoryHelper<CopyFileCommand>());
                registry.Register("deletefile", "deletefile(file) command", new ExpressionFactoryHelper<DeleteFileCommand>());
                registry.Register("copyfiles", "copyfiles(src,dest,filter1,new_ext1,...) command", new ExpressionFactoryHelper<CopyFilesCommand>());
                registry.Register("deletefiles", "deletefiles(dir,filter1,filter2,...) command", new ExpressionFactoryHelper<DeleteFilesCommand>());
                registry.Register("deleteallfiles", "deleteallfiles(dir,filter1,filter2,...) command", new ExpressionFactoryHelper<DeleteAllFilesCommand>());
                registry.Register("cd", "cd(dir) command", new ExpressionFactoryHelper<SetCurrentDirectoryCommand>());

                registry.Register("allocmemory", "allocmemory(key,size) command", new ExpressionFactoryHelper<AllocMemoryCommand>());
                registry.Register("freememory", "freememory(key) command", new ExpressionFactoryHelper<FreeMemoryCommand>());
                registry.Register("consumecpu", "consumecpu(time) command", new ExpressionFactoryHelper<ConsumeCpuCommand>());
                registry.Register("gc", "gc() command, force Garbage Collect", new ExpressionFactoryHelper<GcCommand>());
                registry.Register("startprofiler", "startprofiler() command", new ExpressionFactoryHelper<StartProfilerCommand>());
                registry.Register("stopprofiler", "stopprofiler() command", new ExpressionFactoryHelper<StopProfilerCommand>());
                registry.Register("logprofiler", "logprofiler() command", new ExpressionFactoryHelper<LogProfilerCommand>());
                registry.Register("takesnapshot", "takesnapshot([clearmem,path,flags]) command", new ExpressionFactoryHelper<TakeSnapshotCommand>());

                registry.Register("cmd", "cmd(str) command", new ExpressionFactoryHelper<CmdCommand>());

                registry.Register("prefsetint", "prefsetint(key,val) command", new ExpressionFactoryHelper<PlayerPrefIntCommand>());
                registry.Register("prefsetfloat", "prefsetfloat(key,val) command", new ExpressionFactoryHelper<PlayerPrefFloatCommand>());
                registry.Register("prefsetstr", "prefsetstr(key,val) command", new ExpressionFactoryHelper<PlayerPrefStringCommand>());
                registry.Register("prefdel", "prefdel(key) command", new ExpressionFactoryHelper<PlayerPrefDeleteCommand>());
                registry.Register("prefdelall", "prefdelall() command", new ExpressionFactoryHelper<PlayerPrefDeleteAllCommand>());
                registry.Register("prefsetbyjava", "prefsetbyjava(key,val) command", new ExpressionFactoryHelper<PrefByJavaCommand>());
                registry.Register("wetesttouch", "wetesttouch(action,x,y) command, simulate touch event with WeTest", new ExpressionFactoryHelper<WeTestTouchCommand>());
                registry.Register("logmesh", "logmesh(mesh_or_path) command", new ExpressionFactoryHelper<LogMeshCommand>());
                registry.Register("setmat", "setmat(renderer_or_path,mat_or_path) or setmat(renderer_or_path,ix,mat_or_path) or setmat(renderer_or_path,ix,mats_or_path,ix) command", new ExpressionFactoryHelper<SetMaterialCommand>());
                registry.Register("setsmat", "setsmat(renderer_or_path,mat_or_path) or setsmat(renderer_or_path,ix,mat_or_path) or setsmat(renderer_or_path,ix,mats_or_path,ix) command", new ExpressionFactoryHelper<SetSharedMaterialCommand>());
                registry.Register("setmats", "setmats(mesh_or_path,mats_or_path) command", new ExpressionFactoryHelper<SetMaterialsCommand>());
                registry.Register("setsmats", "setsmats(mesh_or_path,mats_or_path) command", new ExpressionFactoryHelper<SetSharedMaterialsCommand>());
                registry.Register("matsetfloat", "matsetfloat(mat_or_path,key,val) command, Material.SetFloat", new ExpressionFactoryHelper<MaterialSetFloatCommand>());
                registry.Register("matsetint", "matsetint(mat_or_path,key,val) command, Material.SetInt", new ExpressionFactoryHelper<MaterialSetIntCommand>());
                registry.Register("matsetinteger", "matsetinteger(mat_or_path,key,val) command, Material.SetInteger", new ExpressionFactoryHelper<MaterialSetIntegerCommand>());
                registry.Register("matsetvector", "matsetvector(mat_or_path,key,val) command, Material.SetVector", new ExpressionFactoryHelper<MaterialSetVectorCommand>());
                registry.Register("matsetcolor", "matsetcolor(mat_or_path,key,val) command, Material.SetColor", new ExpressionFactoryHelper<MaterialSetColorCommand>());

                registry.Register("logobjs", "logobjs(name_key,type) command", new ExpressionFactoryHelper<LogObjectsCommand>());
                registry.Register("logcomps", "logcomps(root_name,[name1,name2,...],type,up_level,include_inactive) command", new ExpressionFactoryHelper<LogComponentsCommand>());
                registry.Register("logscenepath", "logscenepath([prefixs],obj,up_level) command", new ExpressionFactoryHelper<LogScenePathCommand>());
                registry.Register("clickui", "clickui(name1,name2,...) command", new ExpressionFactoryHelper<ClickUiCommand>());
                registry.Register("toggleon", "toggleon(name1,name2,...) command", new ExpressionFactoryHelper<ToggleOnCommand>());

                registry.Register("clickonptr", "clickonptr() command", new ExpressionFactoryHelper<ClickOnPointerCommand>());
                registry.Register("toggleonptr", "toggleonptr() command", new ExpressionFactoryHelper<ToggleOnPointerCommand>());
                registry.Register("clickonpos", "clickonpos(x,y) command", new ExpressionFactoryHelper<ClickOnPosCommand>());
                registry.Register("toggleonpos", "toggleonpos(x,y) command", new ExpressionFactoryHelper<ToggleOnPosCommand>());
                registry.Register("click", "click(uiobj) command", new ExpressionFactoryHelper<ClickCommand>());
                registry.Register("toggle", "toggle(uiobj) command", new ExpressionFactoryHelper<ToggleCommand>());

                registry.Register("runstartup", "runstartup(startup_dsl_file, rungm) command", new ExpressionFactoryHelper<RunStartupCommand>());
                registry.Register("import", "import(ns,assembly) command", new ExpressionFactoryHelper<ImportNamespaceCommand>());
                registry.Register("unimport", "unimport(ns,assembly) command", new ExpressionFactoryHelper<UnImportNamespaceCommand>());
                registry.Register("setviewer", "setviewer(key) or setviewer(key,rect) or setviewer(key,rect,interval) command", new ExpressionFactoryHelper<AddOrUpdateSceneViewerCommand>());
                registry.Register("viewerlookat", "viewerlookat(key) or viewerlookat(key,dist) or viewerlookat(key,dist,dir) or viewerlookat(key,dist,dir,target) command", new ExpressionFactoryHelper<SceneViewerLookAtCommand>());
                //register value or function
                registry.Register("existsdir", "existsdir(dir) function", new ExpressionFactoryHelper<ExistsDirectoryFunction>());
                registry.Register("existsfile", "existsfile(file) function", new ExpressionFactoryHelper<ExistsFileFunction>());
                registry.Register("expand", "expand(file) function", new ExpressionFactoryHelper<ExpandEnvironmentsFunction>());
                registry.Register("envs", "envs() function", new ExpressionFactoryHelper<EnvironmentsFunction>());
                registry.Register("pwd", "pwd() function", new ExpressionFactoryHelper<GetCurrentDirectoryFunction>());

                registry.Register("isdebug", "isdebug() function", new ExpressionFactoryHelper<IsDebugFunction>());
                registry.Register("isdev", "isdev() function", new ExpressionFactoryHelper<IsDevelopmentFunction>());
                registry.Register("typeof", "typeof(type) or typeof(type,assembly) function", new ExpressionFactoryHelper<TypeOfFunction>());
                registry.Register("mono", "mono() function, get total mono memory", new ExpressionFactoryHelper<GetMonoMemoryFunction>());
                registry.Register("native", "native() function, get used native memory", new ExpressionFactoryHelper<GetNativeMemoryFunction>());
                registry.Register("gfx", "gfx() function, get used gfx memory", new ExpressionFactoryHelper<GetGfxMemoryFunction>());
                registry.Register("unused", "unused() function, get unused reserved native memory", new ExpressionFactoryHelper<GetUnusedMemoryFunction>());
                registry.Register("total", "total() function, get total native memory", new ExpressionFactoryHelper<GetTotalMemoryFunction>());
                registry.Register("getobjectmemory", "getobjectmemory(obj) function, get object memory", new ExpressionFactoryHelper<GetObjectMemoryFunction>());
                registry.Register("deviceinfo", "deviceinfo() function, get device name/model and gpu model", new ExpressionFactoryHelper<DeviceInfoFunction>());
                registry.Register("getclipboard", "getclipboard() function, get system clipboard content", new ExpressionFactoryHelper<GetClipboardFunction>());
                registry.Register("getobjectbyid", "getobjectbyid(id) function, get unity object by instance id", new ExpressionFactoryHelper<GetObjectByIdFunction>());
                registry.Register("getobjectid", "getobjectid(obj) function, get unity object instance id", new ExpressionFactoryHelper<GetObjectIdFunction>());

                registry.Register("getandroidsdk", "getandroidsdk() function", new ExpressionFactoryHelper<GetAndroidSdkIntFunction>());
                registry.Register("getbool", "getbool(str) function", new ExpressionFactoryHelper<GetBoolFunction>());
                registry.Register("getboolarray", "getboolarray(str) function", new ExpressionFactoryHelper<GetBoolArrayFunction>());
                registry.Register("getstring", "getstring(str) function", new ExpressionFactoryHelper<GetStringFunction>());
                registry.Register("getstringarray", "getstringarray(str) function", new ExpressionFactoryHelper<GetStringArrayFunction>());
                registry.Register("getchar", "getchar(str) function", new ExpressionFactoryHelper<GetCharFunction>());
                registry.Register("getchararray", "getchararray(str) function", new ExpressionFactoryHelper<GetCharArrayFunction>());
                registry.Register("getbyte", "getbyte(str) function", new ExpressionFactoryHelper<GetByteFunction>());
                registry.Register("getbytearray", "getbytearray(str) function", new ExpressionFactoryHelper<GetByteArrayFunction>());
                registry.Register("getshort", "getshort(str) function", new ExpressionFactoryHelper<GetShortFunction>());
                registry.Register("getshortarray", "getshortarray(str) function", new ExpressionFactoryHelper<GetShortArrayFunction>());
                registry.Register("getint", "getint(str) function", new ExpressionFactoryHelper<GetIntFunction>());
                registry.Register("getintarray", "getintarray(str) function", new ExpressionFactoryHelper<GetIntArrayFunction>());
                registry.Register("getlong", "getlong(str) function", new ExpressionFactoryHelper<GetLongFunction>());
                registry.Register("getlongarray", "getlongarray(str) function", new ExpressionFactoryHelper<GetLongArrayFunction>());
                registry.Register("getfloat", "getfloat(str) function", new ExpressionFactoryHelper<GetFloatFunction>());
                registry.Register("getfloatarray", "getfloatarray(str) function", new ExpressionFactoryHelper<GetFloatArrayFunction>());
                registry.Register("getdouble", "getdouble(str) function", new ExpressionFactoryHelper<GetDoubleFunction>());
                registry.Register("getdoublearray", "getdoublearray(str) function", new ExpressionFactoryHelper<GetDoubleArrayFunction>());
                registry.Register("getactivityclass", "getactivityclass() function", new ExpressionFactoryHelper<GetActivityClassNameFunction>());

                registry.Register("prefgetint", "prefgetint(key,defval) function", new ExpressionFactoryHelper<PlayerPrefIntFunction>());
                registry.Register("prefgetfloat", "prefgetfloat(key,defval) function", new ExpressionFactoryHelper<PlayerPrefFloatFunction>());
                registry.Register("prefgetstr", "prefgetstr(key,defval) function", new ExpressionFactoryHelper<PlayerPrefStringFunction>());
                registry.Register("prefgetbyjava", "prefgetbyjava(key,defval) function", new ExpressionFactoryHelper<PrefByJavaFunction>());

                registry.Register("istexsupported", "istexsupported(fmt_str,usage_str) function", new ExpressionFactoryHelper<IsFormatSupportedFunction>());
                registry.Register("getcompatibleformat", "getcompatibleformat(fmt_str,usage_str) function", new ExpressionFactoryHelper<GetCompatibleFormatFunction>());
                registry.Register("getgraphicsformat", "getgraphicsformat(def_fmt) function", new ExpressionFactoryHelper<GetGraphicsFormatFunction>());
                registry.Register("getmsaasamplecount", "getmsaasamplecount(w,h,color_fmt,depth_bit,mip_ct) function", new ExpressionFactoryHelper<GetMSAASampleCountFunction>());
                registry.Register("sysinfo", "sysinfo() function, return typeof(SystemInfo)", new ExpressionFactoryHelper<SystemInfoFunction>());
                registry.Register("screen", "screen() function, return typeof(Screen)", new ExpressionFactoryHelper<ScreenFunction>());
                registry.Register("screenwidth", "screenwidth() function, return Screen.width", new ExpressionFactoryHelper<ScreenWidthFunction>());
                registry.Register("screenheight", "screenheight() function, return Screen.height", new ExpressionFactoryHelper<ScreenHeightFunction>());
                registry.Register("screendpi", "screendpi() function, return Screen.dpi", new ExpressionFactoryHelper<ScreenDPIFunction>());
                registry.Register("app", "app() function, return typeof(Application)", new ExpressionFactoryHelper<ApplicationFunction>());
                registry.Register("appid", "appid() function, return Application.identifier", new ExpressionFactoryHelper<AppIdFunction>());
                registry.Register("appname", "appname() function, return Application.productName", new ExpressionFactoryHelper<AppNameFunction>());
                registry.Register("platform", "platform() function, return Application.platform", new ExpressionFactoryHelper<PlatformFunction>());
                registry.Register("iseditor", "iseditor() function, return Application.isEditor", new ExpressionFactoryHelper<IsEditorFunction>());
                registry.Register("isconsole", "isconsole() function, return Application.isConsolePlatform", new ExpressionFactoryHelper<IsConsoleFunction>());
                registry.Register("ismobile", "ismobile() function, return Application.isMobilePlatform", new ExpressionFactoryHelper<IsMobileFunction>());
                registry.Register("isandroid", "isandroid() function, return Application.platform==Android", new ExpressionFactoryHelper<IsAndroidFunction>());
                registry.Register("isiphone", "isiphone() function, return Application.platform==IPhone", new ExpressionFactoryHelper<IsIPhoneFunction>());
                registry.Register("ispc", "ispc() function, return not mobile and not console", new ExpressionFactoryHelper<IsPCFunction>());
                registry.Register("getshell", "getshell(cmd) function, return string", new ExpressionFactoryHelper<ShellFunction>());
                registry.Register("getshelltimeout", "getshelltimeout(cmd,ms) function, return string", new ExpressionFactoryHelper<ShellTimeoutFunction>());
                registry.Register("isjavatask", "isjavatask() function, return int", new ExpressionFactoryHelper<IsJavaTaskFunction>());
                registry.Register("gettaskcount", "gettaskcount() function, return int", new ExpressionFactoryHelper<GetTaskCountFunction>());
                registry.Register("wetestx", "wetestx() function, WeTest GetX", new ExpressionFactoryHelper<WeTestGetXFunction>());
                registry.Register("wetesty", "wetesty() function, WeTest GetY", new ExpressionFactoryHelper<WeTestGetYFunction>());
                registry.Register("wetestwidth", "wetestwidth() function, WeTest GetWidth", new ExpressionFactoryHelper<WeTestGetWidthFunction>());
                registry.Register("wetestheight", "wetestheight() function, WeTest GetHeight", new ExpressionFactoryHelper<WeTestGetHeightFunction>());
                registry.Register("getpointer", "getpointer() function, return Input.mousePosition", new ExpressionFactoryHelper<GetPointerFunction>());
                registry.Register("getptruis", "getptruis() function, return List<UnityEngine.EventSystems.RaycastResult>", new ExpressionFactoryHelper<PointerRaycastUisFunction>());
                registry.Register("getptrcomps", "getptrcomps(type,include_inactive) function, return List<Component>", new ExpressionFactoryHelper<GetPointerComponentsFunction>());
                registry.Register("raycastuis", "raycastuis(x,y) function, return List<UnityEngine.EventSystems.RaycastResult>", new ExpressionFactoryHelper<RaycastUisFunction>());
                registry.Register("raycastcomps", "raycastcomps(x,y,type,include_inactive) function, return List<Component>", new ExpressionFactoryHelper<RaycastComponentsFunction>());
                registry.Register("getscenepath", "getscenepath([prefixs],obj,up_level) function, return partial scene path", new ExpressionFactoryHelper<GetScenePathFunction>());

                registry.Register("shader", "shader() function, return typeof(Shader)", new ExpressionFactoryHelper<ShaderFunction>());
                registry.Register("getmat", "getmat(mat_or_path[,index]) function, renderer.material[s[ix]]", new ExpressionFactoryHelper<GetMaterialFunction>());
                registry.Register("getsmat", "getsmat(mat_or_path[,index]) function, renderer.sharedMaterial[s[ix]]", new ExpressionFactoryHelper<GetSharedMaterialFunction>());
                registry.Register("matgetfloat", "matgetfloat(mat_or_path,key) function, Material.GetFloat", new ExpressionFactoryHelper<MaterialGetFloatFunction>());
                registry.Register("matgetint", "matgetint(mat_or_path,key) function, Material.GetInt", new ExpressionFactoryHelper<MaterialGetIntFunction>());
                registry.Register("matgetinteger", "matgetinteger(mat_or_path,key) function, Material.GetInteger", new ExpressionFactoryHelper<MaterialGetIntegerFunction>());
                registry.Register("matgetvector", "matgetvector(mat_or_path,key) function, Material.GetVector", new ExpressionFactoryHelper<MaterialGetVectorFunction>());
                registry.Register("matgetcolor", "matgetcolor(mat_or_path,key) function, Material.GetColor", new ExpressionFactoryHelper<MaterialGetColorFunction>());

                registry.Register("findrawimg", "findrawimg(name1,name2,...) function", new ExpressionFactoryHelper<FindRawImageFunction>());
                registry.Register("finduiimg", "finduiimg(name1,name2,...) function", new ExpressionFactoryHelper<FindUiImageFunction>());
                registry.Register("findbutton", "findbutton(name1,name2,...) function", new ExpressionFactoryHelper<FindUiButtonFunction>());
                registry.Register("findtoggle", "findtoggle(name1,name2,...) function", new ExpressionFactoryHelper<FindUiToggleFunction>());
                registry.Register("findslider", "findslider(name1,name2,...) function", new ExpressionFactoryHelper<FindUiSliderFunction>());
                registry.Register("findinput", "findinput(name1,name2,...) function", new ExpressionFactoryHelper<FindUiInputFunction>());
                registry.Register("findtmpinput", "findtmpinput(name1,name2,...) function", new ExpressionFactoryHelper<FindTmpInputFunction>());
                registry.Register("finddropdown", "finddropdown(name1,name2,...) function", new ExpressionFactoryHelper<FindUiDropdownFunction>());
                registry.Register("findtmpdropdown", "findtmpdropdown(name1,name2,...) function", new ExpressionFactoryHelper<FindTmpDropdownFunction>());

                registry.Register("findobj", "findobj(name_key,type) function", new ExpressionFactoryHelper<FindObjectFunction>());
                registry.Register("searchobjs", "searchobjs(name_key,type) function", new ExpressionFactoryHelper<SearchObjectsFunction>());
                registry.Register("findcomp", "findcomp(root_name,[name1,name2,...],type,include_inactive) function", new ExpressionFactoryHelper<FindComponentFunction>());
                registry.Register("searchcomps", "searchcomps(root_name,[name1,name2,...],type,include_inactive) function", new ExpressionFactoryHelper<SearchComponentsFunction>());

                registry.Register("raycastuis", "raycastuis(x,y) function, return List<UnityEngine.EventSystems.RaycastResult>", new ExpressionFactoryHelper<RaycastUisFunction>());
                registry.Register("raycastcomps", "raycastcomps(x,y,type,include_inactive) function, return List<Component>", new ExpressionFactoryHelper<RaycastComponentsFunction>());

                if (null != OnRegisterGmCommandsAndFunctions) {
                    OnRegisterGmCommandsAndFunctions(registry);
                    m_RegisteredHandler = OnRegisterGmCommandsAndFunctions;
                }

                //failback to call startup api
                StoryInstance.OnLoadFailback = this.OnLoadFailback;
            }
            catch (Exception ex) {
                LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void RegisterExtraCommands()
        {
            var handler = OnRegisterGmCommandsAndFunctions;
            if (null == handler || handler == m_RegisteredHandler) {
                return;
            }
            m_RegisteredHandler = handler;
            try {
                handler(DslCalculatorHost.GetSharedApiRegistry());
            }
            catch (Exception ex) {
                LogSystem.Error("register extra gm commands exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void RegisterCommon(DslCalculatorApiRegistry registry)
        {
            //register command
            registry.Register("sendmessage", "sendmessage(objname,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageCommand>());
            registry.Register("sendmessagewithtag", "sendmessagewithtag(tagname,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageWithTagCommand>());
            registry.Register("sendmessagewithgameobject", "sendmessagewithgameobject(gameobject,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageWithGameObjectCommand>());
            registry.Register("creategameobject", "creategameobject(name,prefab[,parent])[obj(\"varname\")]{position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));loadtimeout(1000);disable(\"typename\", \"typename\", ...);remove(\"typename\", \"typename\", ...);} command", new ExpressionFactoryHelper<GmCommands.CreateGameObjectCommand>());
            registry.Register("settransform", "settransform(name,local0_or_world1){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));} command", new ExpressionFactoryHelper<GmCommands.SetTransformCommand>());
            registry.Register("addtransform", "addtransform(name,local0_or_world1){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));} command", new ExpressionFactoryHelper<GmCommands.AddTransformCommand>());
            registry.Register("destroygameobject", "destroygameobject(path) command", new ExpressionFactoryHelper<GmCommands.DestroyGameObjectCommand>());
            registry.Register("setparent", "setparent(obj_or_path,parent,int_stay_world_pos) command", new ExpressionFactoryHelper<GmCommands.SetParentCommand>());
            registry.Register("setactive", "setactive(obj_or_path,1_or_0) command", new ExpressionFactoryHelper<GmCommands.SetActiveCommand>());
            registry.Register("setvisible", "setvisible(obj_or_path,1_or_0) command", new ExpressionFactoryHelper<GmCommands.SetVisibleCommand>());
            registry.Register("addcomponent", "addcomponent(obj_or_path,type_or_str)[obj(\"varname\")] command", new ExpressionFactoryHelper<GmCommands.AddComponentCommand>());
            registry.Register("removecomponent", "removecomponent(obj_or_path,type_or_str) command", new ExpressionFactoryHelper<GmCommands.RemoveComponentCommand>());
            registry.Register("openurl", "openurl(url) command", new ExpressionFactoryHelper<GmCommands.OpenUrlCommand>());
            registry.Register("quit", "quit() command", new ExpressionFactoryHelper<GmCommands.QuitCommand>());

            registry.Register("gameobjectanimation", "gameobjectanimation(obj, anim[, normalized_time]) command", new ExpressionFactoryHelper<GmCommands.GameObjectAnimationCommand>());
            registry.Register("gameobjectanimationparam", "gameobjectanimationparam(obj){float(name,val);int(name,val);bool(name,val);trigger(name,val);} command", new ExpressionFactoryHelper<GmCommands.GameObjectAnimationParamCommand>());

            //register value or function
            registry.Register("gettime", "gettime() function", new ExpressionFactoryHelper<GmCommands.GetTimeFunction>());
            registry.Register("gettimescale", "gettimescale() function", new ExpressionFactoryHelper<GmCommands.GetTimeScaleFunction>());
            registry.Register("isactive", "isactive(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsActiveFunction>());
            registry.Register("isreallyactive", "isreallyactive(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsReallyActiveFunction>());
            registry.Register("isvisible", "isvisible(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsVisibleFunction>());
            registry.Register("getcomponent", "getcomponent(obj_or_path,type_or_str) function", new ExpressionFactoryHelper<GmCommands.GetComponentFunction>());
            registry.Register("getcomponentinparent", "getcomponentinparent(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentInParentFunction>());
            registry.Register("api_list", "api_list([regex,...]) - regex search over all APIs/docs registered on the current runtime interpreter (like helpall)", new ExpressionFactoryHelper<GmCommands.ApiListCommand>());
            registry.Register("getcomponentinchildren", "getcomponentinchildren(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentInChildrenFunction>());
            registry.Register("getcomponents", "getcomponents(obj_or_path,type_or_str) function", new ExpressionFactoryHelper<GmCommands.GetComponentsFunction>());
            registry.Register("getcomponentsinparent", "getcomponentsinparent(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentsInParentFunction>());
            registry.Register("getcomponentsinchildren", "getcomponentsinchildren(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentsInChildrenFunction>());
            registry.Register("getgameobject", "getgameobject(obj_or_path) or getgameobject(obj_or_path){disable(comp1,comp2,...);remove(comp1,comp2,...);} function", new ExpressionFactoryHelper<GmCommands.GetGameObjectFunction>());
            registry.Register("getparent", "getparent(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetParentFunction>());
            registry.Register("getchild", "getchild(obj_or_path,child_path) function", new ExpressionFactoryHelper<GmCommands.GetChildFunction>());
            registry.Register("getunitytype", "getunitytype(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUnityTypeFunction>());
            registry.Register("getunityuitype", "getunityuitype(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUnityUiTypeFunction>());
            registry.Register("getusertype", "getusertype(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUserTypeFunction>());

            registry.Register("getposition", "getposition(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionFunction>());
            registry.Register("getpositionx", "getpositionx(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionXFunction>());
            registry.Register("getpositiony", "getpositiony(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionYFunction>());
            registry.Register("getpositionz", "getpositionz(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionZFunction>());
            registry.Register("getrotation", "getrotation(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationFunction>());
            registry.Register("getrotationx", "getrotationx(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationXFunction>());
            registry.Register("getrotationy", "getrotationy(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationYFunction>());
            registry.Register("getrotationz", "getrotationz(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationZFunction>());
            registry.Register("getscale", "getscale(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleFunction>());
            registry.Register("getscalex", "getscalex(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleXFunction>());
            registry.Register("getscaley", "getscaley(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleYFunction>());
            registry.Register("getscalez", "getscalez(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleZFunction>());

            registry.Register("position", "position(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());
            registry.Register("rotation", "rotation(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());
            registry.Register("scale", "scale(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());

            registry.Register("deg2rad", "deg2rad(deg) function", new ExpressionFactoryHelper<GmCommands.Deg2RadFunction>());
            registry.Register("rad2deg", "rad2deg(rad) function", new ExpressionFactoryHelper<GmCommands.Rad2DegFunction>());

            registry.Register("getfilename", "getfilename(path) function", new ExpressionFactoryHelper<GmCommands.GetFileNameFunction>());
            registry.Register("getdirname", "getdirname(path) function", new ExpressionFactoryHelper<GmCommands.GetDirectoryNameFunction>());
            registry.Register("getextension", "getextension(path) function", new ExpressionFactoryHelper<GmCommands.GetExtensionFunction>());
            registry.Register("combinepath", "combinepath(path1,path2) function", new ExpressionFactoryHelper<GmCommands.CombinePathFunction>());
            registry.Register("getstreamingassets", "getstreamingassets() function", new ExpressionFactoryHelper<GmCommands.GetStreamingAssetsFunction>());
            registry.Register("getpersistentpath", "getpersistentpath() function", new ExpressionFactoryHelper<GmCommands.GetPersistentPathFunction>());

        }
        private bool OnLoadFailback(Dsl.ISyntaxComponent comp, DslCalculatorCodeStructure code, out IExpression expression)
        {
            bool ret = false;
            expression = null;

            var funcData = comp as Dsl.FunctionData;
            if (null != funcData) {
                //all startup apis are in the form of function calls.
                if (funcData.HaveParam()) {
                    var callData = funcData;
                    string fn = callData.GetId();
                    if (StartupScript.ExistsApi(fn)) {
                        var exp = new StartupApiExp(fn);
                        if (exp.Load(funcData, code)) {
                            expression = exp;
                            ret = true;
                        }
                        expression = exp;
                        ret = true;
                    }
                }
                else {
                    var valData = comp as Dsl.ValueData;
                    if (null != valData && valData.IsId()) {
                        var typeName = valData.GetId();
                        var type = StoryScriptUtility.GetType(typeName);
                        if (null != type) {
                            var exp = new TypeHolderFunction();
                            exp.SetType(type);
                            exp.Load(valData, code);
                            expression = exp;
                            ret = true;
                        }
                        else if (StoryScriptUtility.IsNamespace(typeName, out var id)) {
                            var exp = new NamespaceHolderFunction();
                            exp.SetNamespace(typeName);
                            exp.Load(valData, code);
                            expression = exp;
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        public int ActiveStoryCount
        {
            get {
                return m_StoryLogicInfos.Count;
            }
        }
        public StrBoxedValueDict ContextVariables
        {
            get { return m_ContextVariables; }
        }
        public void ClearContextVariables()
        {
            m_ContextVariables.Clear();
        }
        public void Reset()
        {
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }
        public void LoadStory(string file)
        {
            m_StoryInstancePool.Clear();
            m_ConfigManager.Clear();
            m_ConfigManager.LoadStory(file, 0, string.Empty);
        }
        public void LoadStoryText(byte[] bytes)
        {
            m_StoryInstancePool.Clear();
            m_ConfigManager.Clear();
            m_ConfigManager.LoadStoryText(string.Empty, bytes, 0, string.Empty);
        }
        public StoryInstance GetStory(string storyId)
        {
            return GetStoryInstance(storyId);
        }
        public void StartStory(string storyId)
        {
            StoryInstance inst = NewStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = null;
                inst.ContextVariables = m_ContextVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            }
        }
        public void StopStory(string storyId)
        {
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public void Tick()
        {
            long time = TimeUtility.GetLocalMilliseconds();
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.Tick(time);
                if (info.IsTerminated) {
                    m_StoryLogicInfos.RemoveAt(ix);
                }
            }
        }
        public BoxedValueList NewBoxedValueList()
        {
            var args = m_BoxedValueListPool.Alloc();
            args.Clear();
            return args;
        }
        public void SendMessage(string msgId)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId);
            }
        }
        public void SendMessage(string msgId, BoxedValue arg1)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId, arg1);
            }
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId, arg1, arg2);
            }
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId, arg1, arg2, arg3);
            }
        }
        public void SendMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendMessage(msgId, newArgs);
            }
            m_BoxedValueListPool.Recycle(args);
        }

        private StoryInstance NewStoryInstance(string storyId)
        {
            StoryInstance instance = GetStoryInstance(storyId);
            if (null == instance) {
                instance = m_ConfigManager.NewStoryInstance(storyId, 0);
                if (instance == null) {
                    LogSystem.Error("Can't load story config, story:{0} !", storyId);
                    return null;
                }

                AddStoryInstance(storyId, instance);
                return instance;
            }
            else {
                return instance;
            }
        }
        private void AddStoryInstance(string storyId, StoryInstance info)
        {
            if (!m_StoryInstancePool.ContainsKey(storyId)) {
                m_StoryInstancePool.Add(storyId, info);
            }
            else {
                m_StoryInstancePool[storyId] = info;
            }
        }
        private StoryInstance GetStoryInstance(string storyId)
        {
            StoryInstance info;
            m_StoryInstancePool.TryGetValue(storyId, out info);
            return info;
        }

        private ClientGmStorySystem() { }

        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();
        private StrBoxedValueDict m_ContextVariables = new StrBoxedValueDict();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();

        private StoryConfigManager m_ConfigManager = StoryConfigManager.NewInstance();
        private bool m_Inited;
        private RegisterGmCommandsAndFunctionsDelegation m_RegisteredHandler;

        public static ClientGmStorySystem Instance
        {
            get {
                return s_Instance;
            }
        }
        private static ClientGmStorySystem s_Instance = new ClientGmStorySystem();
    }

    internal class TypeHolderFunction : SimpleExpressionBase
    {
        public void SetType(Type type)
        {
            m_Type = type;
        }
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(m_Type);
        }

        private Type m_Type;
    }
    internal class NamespaceObject : StoryScript.IObjectDispatch
    {
        public NamespaceObject(string ns)
        {
            m_Namespace = ns;
        }
        public int GetDispatchId(string name)
        {
            if (StoryScriptUtility.IsNamespace(name, out int id))
                return id;
            return -1;
        }
        public void SetProperty(int dispId, BoxedValue val)
        {
            throw new NotImplementedException();
        }
        public BoxedValue GetProperty(int dispId)
        {
            string name = StoryScriptUtility.GetNamespace(dispId);
            if (string.IsNullOrEmpty(name)) {
                return BoxedValue.NullObject;
            }
            else {
                string newName = m_Namespace + "." + name;
                var type = StoryScriptUtility.GetType(newName);
                if (null != type) {
                    return type;
                }
                else {
                    return BoxedValue.FromObject(new NamespaceObject(newName));
                }
            }
        }
        public BoxedValue InvokeMethod(int dispId, List<BoxedValue> args)
        {
            throw new NotImplementedException();
        }

        private string m_Namespace;
    }
    internal class NamespaceHolderFunction : SimpleExpressionBase
    {
        public void SetNamespace(string ns)
        {
            m_NamespaceObj = BoxedValue.FromObject(new NamespaceObject(ns));
        }
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(m_NamespaceObj);
        }

        private BoxedValue m_NamespaceObj;
    }
}
