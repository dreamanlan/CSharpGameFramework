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
                registry.Register("log_code_num", "log_code_num() command", new ExpressionFactoryHelper<LogCodeNumCommand>());
                registry.Register("help", "help() command", new ExpressionFactoryHelper<HelpCommand>());
                registry.Register("set_debug", "set_debug(1_or_0) command", new ExpressionFactoryHelper<SetDebugCommand>());
                registry.Register("start_activity", "start_activity(package_name[[,class_name[,flags]],extra_list_or_dict]) command", new ExpressionFactoryHelper<StartActivityCommand>());
                registry.Register("finish_activity", "finish_activity() command", new ExpressionFactoryHelper<FinishActivityCommand>());
                registry.Register("listen_clipboard", "listen_clipboard(interval) command", new ExpressionFactoryHelper<ListenClipboardCommand>());
                registry.Register("listen_android", "listen_android() command", new ExpressionFactoryHelper<ListenAndroidCommand>());
				registry.Register("listen_socket", "listen_socket(port) command", new ExpressionFactoryHelper<ListenSocketCommand>());
                registry.Register("start_service", "start_service(srv_class, extra_name, extra_val) command", new ExpressionFactoryHelper<StartServiceCommand>());
                registry.Register("stop_service", "stop_service(srv_class) command", new ExpressionFactoryHelper<StopServiceCommand>());
                registry.Register("shell", "shell(cmd) command", new ExpressionFactoryHelper<ShellCommand>());
                registry.Register("shell_timeout", "shell_timeout(cmd,ms) command", new ExpressionFactoryHelper<ShellTimeoutCommand>());
                registry.Register("cleanup_completed_tasks", "cleanup_completed_tasks() command", new ExpressionFactoryHelper<CleanupCompletedTasksCommand>());
                registry.Register("use_java_task", "use_java_task(is_java) command", new ExpressionFactoryHelper<UseJavaTaskCommand>());
                registry.Register("set_clipboard", "set_clipboard(text) command", new ExpressionFactoryHelper<SetClipboardCommand>());
                registry.Register("editor_break", "editor_break() command", new ExpressionFactoryHelper<EditorBreakCommand>());
                registry.Register("debug_break", "debug_break() command", new ExpressionFactoryHelper<DebugBreakCommand>());
                registry.Register("set_time_scale", "set_time_scale(scale) command", new ExpressionFactoryHelper<SetTimeScaleCommand>());
                registry.Register("clear_globals", "clear_globals() command", new ExpressionFactoryHelper<ClearGlobalsCommand>());

                registry.Register("supports_gfx_format", "supports_gfx_format() command, print unsupported graphics format", new ExpressionFactoryHelper<SupportsGfxFormatCommand>());
                registry.Register("supports_tex", "supports_tex() command, print unsupported tex", new ExpressionFactoryHelper<SupportsTexCommand>());
                registry.Register("supports_rt", "supports_rt() command, print unsupported rt", new ExpressionFactoryHelper<SupportsRTCommand>());
                registry.Register("supports_vertex_attribute_format", "supports_vertex_attribute_format() command, print unsupported vertex attribute format", new ExpressionFactoryHelper<SupportsVertexAttributeFormatCommand>());
                registry.Register("device_supports", "device_supports() command, print unsupported feature", new ExpressionFactoryHelper<DeviceSupportsCommand>());
                registry.Register("log_resolutions", "log_resolutions() command, print supported resolutions", new ExpressionFactoryHelper<LogResolutionsCommand>());

                registry.Register("create_dir", "create_dir(dir) command", new ExpressionFactoryHelper<CreateDirectoryCommand>());
                registry.Register("delete_dir", "delete_dir(dir) command", new ExpressionFactoryHelper<DeleteDirectoryCommand>());
                registry.Register("copy_dir", "copy_dir(src,dest,filter1,new_ext1,...) command", new ExpressionFactoryHelper<CopyDirectoryCommand>());
                registry.Register("copy_file", "copy_file(src,dest) command", new ExpressionFactoryHelper<CopyFileCommand>());
                registry.Register("delete_file", "delete_file(file) command", new ExpressionFactoryHelper<DeleteFileCommand>());
                registry.Register("copy_files", "copy_files(src,dest,filter1,new_ext1,...) command", new ExpressionFactoryHelper<CopyFilesCommand>());
                registry.Register("delete_files", "delete_files(dir,filter1,filter2,...) command", new ExpressionFactoryHelper<DeleteFilesCommand>());
                registry.Register("delete_all_files", "delete_all_files(dir,filter1,filter2,...) command", new ExpressionFactoryHelper<DeleteAllFilesCommand>());
                registry.Register("cd", "cd(dir) command", new ExpressionFactoryHelper<SetCurrentDirectoryCommand>());

                registry.Register("alloc_memory", "alloc_memory(key,size) command", new ExpressionFactoryHelper<AllocMemoryCommand>());
                registry.Register("free_memory", "free_memory(key) command", new ExpressionFactoryHelper<FreeMemoryCommand>());
                registry.Register("consume_cpu", "consume_cpu(time) command", new ExpressionFactoryHelper<ConsumeCpuCommand>());
                registry.Register("gc", "gc() command, force Garbage Collect", new ExpressionFactoryHelper<GcCommand>());
                registry.Register("start_profiler", "start_profiler() command", new ExpressionFactoryHelper<StartProfilerCommand>());
                registry.Register("stop_profiler", "stop_profiler() command", new ExpressionFactoryHelper<StopProfilerCommand>());
                registry.Register("log_profiler", "log_profiler() command", new ExpressionFactoryHelper<LogProfilerCommand>());
                registry.Register("take_snapshot", "take_snapshot([clearmem,path,flags]) command", new ExpressionFactoryHelper<TakeSnapshotCommand>());

                registry.Register("cmd", "cmd(str) command", new ExpressionFactoryHelper<CmdCommand>());

                registry.Register("pref_set_int", "pref_set_int(key,val) command", new ExpressionFactoryHelper<PlayerPrefIntCommand>());
                registry.Register("pref_set_float", "pref_set_float(key,val) command", new ExpressionFactoryHelper<PlayerPrefFloatCommand>());
                registry.Register("pref_set_str", "pref_set_str(key,val) command", new ExpressionFactoryHelper<PlayerPrefStringCommand>());
                registry.Register("pref_del", "pref_del(key) command", new ExpressionFactoryHelper<PlayerPrefDeleteCommand>());
                registry.Register("pref_del_all", "pref_del_all() command", new ExpressionFactoryHelper<PlayerPrefDeleteAllCommand>());
                registry.Register("pref_set_by_java", "pref_set_by_java(key,val) command", new ExpressionFactoryHelper<PrefByJavaCommand>());
                registry.Register("wetest_touch", "wetest_touch(action,x,y) command, simulate touch event with WeTest", new ExpressionFactoryHelper<WeTestTouchCommand>());
                registry.Register("log_mesh", "log_mesh(mesh_or_path) command", new ExpressionFactoryHelper<LogMeshCommand>());
                registry.Register("set_mat", "set_mat(renderer_or_path,mat_or_path) or set_mat(renderer_or_path,ix,mat_or_path) or set_mat(renderer_or_path,ix,mats_or_path,ix) command", new ExpressionFactoryHelper<SetMaterialCommand>());
                registry.Register("set_smat", "set_smat(renderer_or_path,mat_or_path) or set_smat(renderer_or_path,ix,mat_or_path) or set_smat(renderer_or_path,ix,mats_or_path,ix) command", new ExpressionFactoryHelper<SetSharedMaterialCommand>());
                registry.Register("set_mats", "set_mats(mesh_or_path,mats_or_path) command", new ExpressionFactoryHelper<SetMaterialsCommand>());
                registry.Register("set_smats", "set_smats(mesh_or_path,mats_or_path) command", new ExpressionFactoryHelper<SetSharedMaterialsCommand>());
                registry.Register("mat_set_float", "mat_set_float(mat_or_path,key,val) command, Material.SetFloat", new ExpressionFactoryHelper<MaterialSetFloatCommand>());
                registry.Register("mat_set_int", "mat_set_int(mat_or_path,key,val) command, Material.SetInt", new ExpressionFactoryHelper<MaterialSetIntCommand>());
                registry.Register("mat_set_integer", "mat_set_integer(mat_or_path,key,val) command, Material.SetInteger", new ExpressionFactoryHelper<MaterialSetIntegerCommand>());
                registry.Register("mat_set_vector", "mat_set_vector(mat_or_path,key,val) command, Material.SetVector", new ExpressionFactoryHelper<MaterialSetVectorCommand>());
                registry.Register("mat_set_color", "mat_set_color(mat_or_path,key,val) command, Material.SetColor", new ExpressionFactoryHelper<MaterialSetColorCommand>());

                registry.Register("log_objs", "log_objs(name_key,type) command", new ExpressionFactoryHelper<LogObjectsCommand>());
                registry.Register("log_comps", "log_comps(root_name,[name1,name2,...],type,up_level,include_inactive) command", new ExpressionFactoryHelper<LogComponentsCommand>());
                registry.Register("log_scene_path", "log_scene_path([prefixs],obj,up_level) command", new ExpressionFactoryHelper<LogScenePathCommand>());
                registry.Register("click_ui", "click_ui(name1,name2,...) command", new ExpressionFactoryHelper<ClickUiCommand>());
                registry.Register("toggle_on", "toggle_on(name1,name2,...) command", new ExpressionFactoryHelper<ToggleOnCommand>());

                registry.Register("click_on_ptr", "click_on_ptr() command", new ExpressionFactoryHelper<ClickOnPointerCommand>());
                registry.Register("toggle_on_ptr", "toggle_on_ptr() command", new ExpressionFactoryHelper<ToggleOnPointerCommand>());
                registry.Register("click_on_pos", "click_on_pos(x,y) command", new ExpressionFactoryHelper<ClickOnPosCommand>());
                registry.Register("toggle_on_pos", "toggle_on_pos(x,y) command", new ExpressionFactoryHelper<ToggleOnPosCommand>());
                registry.Register("click", "click(uiobj) command", new ExpressionFactoryHelper<ClickCommand>());
                registry.Register("toggle", "toggle(uiobj) command", new ExpressionFactoryHelper<ToggleCommand>());

                registry.Register("run_startup", "run_startup(startup_dsl_file, rungm) command", new ExpressionFactoryHelper<RunStartupCommand>());
                registry.Register("import", "import(ns,assembly) command", new ExpressionFactoryHelper<ImportNamespaceCommand>());
                registry.Register("unimport", "unimport(ns,assembly) command", new ExpressionFactoryHelper<UnImportNamespaceCommand>());
                registry.Register("add_or_update_scene_viewer", "add_or_update_scene_viewer(key) or add_or_update_scene_viewer(key,rect) or add_or_update_scene_viewer(key,rect,interval) command", new ExpressionFactoryHelper<AddOrUpdateSceneViewerCommand>());
                registry.Register("scene_viewer_look_at", "scene_viewer_look_at(key) or scene_viewer_look_at(key,dist) or scene_viewer_look_at(key,dist,dir) or scene_viewer_look_at(key,dist,dir,target) command", new ExpressionFactoryHelper<SceneViewerLookAtCommand>());
                //register value or function
                registry.Register("exists_dir", "exists_dir(dir) function", new ExpressionFactoryHelper<ExistsDirectoryFunction>());
                registry.Register("exists_file", "exists_file(file) function", new ExpressionFactoryHelper<ExistsFileFunction>());
                registry.Register("expand", "expand(file) function", new ExpressionFactoryHelper<ExpandEnvironmentsFunction>());
                registry.Register("envs", "envs() function", new ExpressionFactoryHelper<EnvironmentsFunction>());
                registry.Register("pwd", "pwd() function", new ExpressionFactoryHelper<GetCurrentDirectoryFunction>());

                registry.Register("is_debug", "is_debug() function", new ExpressionFactoryHelper<IsDebugFunction>());
                registry.Register("is_dev", "is_dev() function", new ExpressionFactoryHelper<IsDevelopmentFunction>());
                registry.Register("typeof", "typeof(type) or typeof(type,assembly) function", new ExpressionFactoryHelper<TypeOfFunction>());
                registry.Register("mono", "mono() function, get total mono memory", new ExpressionFactoryHelper<GetMonoMemoryFunction>());
                registry.Register("native", "native() function, get used native memory", new ExpressionFactoryHelper<GetNativeMemoryFunction>());
                registry.Register("gfx", "gfx() function, get used gfx memory", new ExpressionFactoryHelper<GetGfxMemoryFunction>());
                registry.Register("unused", "unused() function, get unused reserved native memory", new ExpressionFactoryHelper<GetUnusedMemoryFunction>());
                registry.Register("total", "total() function, get total native memory", new ExpressionFactoryHelper<GetTotalMemoryFunction>());
                registry.Register("get_object_memory", "get_object_memory(obj) function, get object memory", new ExpressionFactoryHelper<GetObjectMemoryFunction>());
                registry.Register("device_info", "device_info() function, get device name/model and gpu model", new ExpressionFactoryHelper<DeviceInfoFunction>());
                registry.Register("get_clipboard", "get_clipboard() function, get system clipboard content", new ExpressionFactoryHelper<GetClipboardFunction>());
                registry.Register("get_object_by_id", "get_object_by_id(id) function, get unity object by instance id", new ExpressionFactoryHelper<GetObjectByIdFunction>());
                registry.Register("get_object_id", "get_object_id(obj) function, get unity object instance id", new ExpressionFactoryHelper<GetObjectIdFunction>());

                registry.Register("get_android_sdk_int", "get_android_sdk_int() function", new ExpressionFactoryHelper<GetAndroidSdkIntFunction>());
                registry.Register("get_bool", "get_bool(str) function", new ExpressionFactoryHelper<GetBoolFunction>());
                registry.Register("get_bool_array", "get_bool_array(str) function", new ExpressionFactoryHelper<GetBoolArrayFunction>());
                registry.Register("get_string", "get_string(str) function", new ExpressionFactoryHelper<GetStringFunction>());
                registry.Register("get_string_array", "get_string_array(str) function", new ExpressionFactoryHelper<GetStringArrayFunction>());
                registry.Register("get_char", "get_char(str) function", new ExpressionFactoryHelper<GetCharFunction>());
                registry.Register("get_char_array", "get_char_array(str) function", new ExpressionFactoryHelper<GetCharArrayFunction>());
                registry.Register("get_byte", "get_byte(str) function", new ExpressionFactoryHelper<GetByteFunction>());
                registry.Register("get_byte_array", "get_byte_array(str) function", new ExpressionFactoryHelper<GetByteArrayFunction>());
                registry.Register("get_short", "get_short(str) function", new ExpressionFactoryHelper<GetShortFunction>());
                registry.Register("get_short_array", "get_short_array(str) function", new ExpressionFactoryHelper<GetShortArrayFunction>());
                registry.Register("get_int", "get_int(str) function", new ExpressionFactoryHelper<GetIntFunction>());
                registry.Register("get_int_array", "get_int_array(str) function", new ExpressionFactoryHelper<GetIntArrayFunction>());
                registry.Register("get_long", "get_long(str) function", new ExpressionFactoryHelper<GetLongFunction>());
                registry.Register("get_long_array", "get_long_array(str) function", new ExpressionFactoryHelper<GetLongArrayFunction>());
                registry.Register("get_float", "get_float(str) function", new ExpressionFactoryHelper<GetFloatFunction>());
                registry.Register("get_float_array", "get_float_array(str) function", new ExpressionFactoryHelper<GetFloatArrayFunction>());
                registry.Register("get_double", "get_double(str) function", new ExpressionFactoryHelper<GetDoubleFunction>());
                registry.Register("get_double_array", "get_double_array(str) function", new ExpressionFactoryHelper<GetDoubleArrayFunction>());
                registry.Register("get_activity_class_name", "get_activity_class_name() function", new ExpressionFactoryHelper<GetActivityClassNameFunction>());

                registry.Register("pref_get_int", "pref_get_int(key,defval) function", new ExpressionFactoryHelper<PlayerPrefIntFunction>());
                registry.Register("pref_get_float", "pref_get_float(key,defval) function", new ExpressionFactoryHelper<PlayerPrefFloatFunction>());
                registry.Register("pref_get_str", "pref_get_str(key,defval) function", new ExpressionFactoryHelper<PlayerPrefStringFunction>());
                registry.Register("pref_get_by_java", "pref_get_by_java(key,defval) function", new ExpressionFactoryHelper<PrefByJavaFunction>());

                registry.Register("is_format_supported", "is_format_supported(fmt_str,usage_str) function", new ExpressionFactoryHelper<IsFormatSupportedFunction>());
                registry.Register("get_compatible_format", "get_compatible_format(fmt_str,usage_str) function", new ExpressionFactoryHelper<GetCompatibleFormatFunction>());
                registry.Register("get_graphics_format", "get_graphics_format(def_fmt) function", new ExpressionFactoryHelper<GetGraphicsFormatFunction>());
                registry.Register("get_msaa_sample_count", "get_msaa_sample_count(w,h,color_fmt,depth_bit,mip_ct) function", new ExpressionFactoryHelper<GetMSAASampleCountFunction>());
                registry.Register("sys_info", "sys_info() function, return typeof(SystemInfo)", new ExpressionFactoryHelper<SystemInfoFunction>());
                registry.Register("screen", "screen() function, return typeof(Screen)", new ExpressionFactoryHelper<ScreenFunction>());
                registry.Register("screen_width", "screen_width() function, return Screen.width", new ExpressionFactoryHelper<ScreenWidthFunction>());
                registry.Register("screen_height", "screen_height() function, return Screen.height", new ExpressionFactoryHelper<ScreenHeightFunction>());
                registry.Register("screen_dpi", "screen_dpi() function, return Screen.dpi", new ExpressionFactoryHelper<ScreenDPIFunction>());
                registry.Register("app", "app() function, return typeof(Application)", new ExpressionFactoryHelper<ApplicationFunction>());
                registry.Register("app_id", "app_id() function, return Application.identifier", new ExpressionFactoryHelper<AppIdFunction>());
                registry.Register("app_name", "app_name() function, return Application.productName", new ExpressionFactoryHelper<AppNameFunction>());
                registry.Register("platform", "platform() function, return Application.platform", new ExpressionFactoryHelper<PlatformFunction>());
                registry.Register("is_editor", "is_editor() function, return Application.isEditor", new ExpressionFactoryHelper<IsEditorFunction>());
                registry.Register("is_console", "is_console() function, return Application.isConsolePlatform", new ExpressionFactoryHelper<IsConsoleFunction>());
                registry.Register("is_mobile", "is_mobile() function, return Application.isMobilePlatform", new ExpressionFactoryHelper<IsMobileFunction>());
                registry.Register("is_android", "is_android() function, return Application.platform==Android", new ExpressionFactoryHelper<IsAndroidFunction>());
                registry.Register("is_iphone", "is_iphone() function, return Application.platform==IPhone", new ExpressionFactoryHelper<IsIPhoneFunction>());
                registry.Register("is_pc", "is_pc() function, return not mobile and not console", new ExpressionFactoryHelper<IsPCFunction>());
                registry.Register("get_shell", "get_shell(cmd) function, return string", new ExpressionFactoryHelper<ShellFunction>());
                registry.Register("get_shell_timeout", "get_shell_timeout(cmd,ms) function, return string", new ExpressionFactoryHelper<ShellTimeoutFunction>());
                registry.Register("is_java_task", "is_java_task() function, return int", new ExpressionFactoryHelper<IsJavaTaskFunction>());
                registry.Register("get_task_count", "get_task_count() function, return int", new ExpressionFactoryHelper<GetTaskCountFunction>());
                registry.Register("wetest_get_x", "wetest_get_x() function, WeTest GetX", new ExpressionFactoryHelper<WeTestGetXFunction>());
                registry.Register("wetest_get_y", "wetest_get_y() function, WeTest GetY", new ExpressionFactoryHelper<WeTestGetYFunction>());
                registry.Register("wetest_get_width", "wetest_get_width() function, WeTest GetWidth", new ExpressionFactoryHelper<WeTestGetWidthFunction>());
                registry.Register("wetest_get_height", "wetest_get_height() function, WeTest GetHeight", new ExpressionFactoryHelper<WeTestGetHeightFunction>());
                registry.Register("get_pointer", "get_pointer() function, return Input.mousePosition", new ExpressionFactoryHelper<GetPointerFunction>());
                registry.Register("get_ptr_uis", "get_ptr_uis() function, return List<UnityEngine.EventSystems.RaycastResult>", new ExpressionFactoryHelper<PointerRaycastUisFunction>());
                registry.Register("get_ptr_comps", "get_ptr_comps(type,include_inactive) function, return List<Component>", new ExpressionFactoryHelper<GetPointerComponentsFunction>());
                registry.Register("raycast_uis", "raycast_uis(x,y) function, return List<UnityEngine.EventSystems.RaycastResult>", new ExpressionFactoryHelper<RaycastUisFunction>());
                registry.Register("raycast_comps", "raycast_comps(x,y,type,include_inactive) function, return List<Component>", new ExpressionFactoryHelper<RaycastComponentsFunction>());
                registry.Register("get_scene_path", "get_scene_path([prefixs],obj,up_level) function, return partial scene path", new ExpressionFactoryHelper<GetScenePathFunction>());

                registry.Register("shader", "shader() function, return typeof(Shader)", new ExpressionFactoryHelper<ShaderFunction>());
                registry.Register("get_mat", "get_mat(mat_or_path[,index]) function, renderer.material[s[ix]]", new ExpressionFactoryHelper<GetMaterialFunction>());
                registry.Register("get_smat", "get_smat(mat_or_path[,index]) function, renderer.sharedMaterial[s[ix]]", new ExpressionFactoryHelper<GetSharedMaterialFunction>());
                registry.Register("mat_get_float", "mat_get_float(mat_or_path,key) function, Material.GetFloat", new ExpressionFactoryHelper<MaterialGetFloatFunction>());
                registry.Register("mat_get_int", "mat_get_int(mat_or_path,key) function, Material.GetInt", new ExpressionFactoryHelper<MaterialGetIntFunction>());
                registry.Register("mat_get_integer", "mat_get_integer(mat_or_path,key) function, Material.GetInteger", new ExpressionFactoryHelper<MaterialGetIntegerFunction>());
                registry.Register("mat_get_vector", "mat_get_vector(mat_or_path,key) function, Material.GetVector", new ExpressionFactoryHelper<MaterialGetVectorFunction>());
                registry.Register("mat_get_color", "mat_get_color(mat_or_path,key) function, Material.GetColor", new ExpressionFactoryHelper<MaterialGetColorFunction>());

                registry.Register("find_raw_img", "find_raw_img(name1,name2,...) function", new ExpressionFactoryHelper<FindRawImageFunction>());
                registry.Register("find_ui_img", "find_ui_img(name1,name2,...) function", new ExpressionFactoryHelper<FindUiImageFunction>());
                registry.Register("find_button", "find_button(name1,name2,...) function", new ExpressionFactoryHelper<FindUiButtonFunction>());
                registry.Register("find_toggle", "find_toggle(name1,name2,...) function", new ExpressionFactoryHelper<FindUiToggleFunction>());
                registry.Register("find_slider", "find_slider(name1,name2,...) function", new ExpressionFactoryHelper<FindUiSliderFunction>());
                registry.Register("find_input", "find_input(name1,name2,...) function", new ExpressionFactoryHelper<FindUiInputFunction>());
                registry.Register("find_tmp_input", "find_tmp_input(name1,name2,...) function", new ExpressionFactoryHelper<FindTmpInputFunction>());
                registry.Register("find_dropdown", "find_dropdown(name1,name2,...) function", new ExpressionFactoryHelper<FindUiDropdownFunction>());
                registry.Register("find_tmp_dropdown", "find_tmp_dropdown(name1,name2,...) function", new ExpressionFactoryHelper<FindTmpDropdownFunction>());

                registry.Register("find_obj", "find_obj(name_key,type) function", new ExpressionFactoryHelper<FindObjectFunction>());
                registry.Register("search_objs", "search_objs(name_key,type) function", new ExpressionFactoryHelper<SearchObjectsFunction>());
                registry.Register("find_comp", "find_comp(root_name,[name1,name2,...],type,include_inactive) function", new ExpressionFactoryHelper<FindComponentFunction>());
                registry.Register("search_comps", "search_comps(root_name,[name1,name2,...],type,include_inactive) function", new ExpressionFactoryHelper<SearchComponentsFunction>());


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
            registry.Register("send_message", "send_message(objname,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageCommand>());
            registry.Register("send_message_with_tag", "send_message_with_tag(tagname,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageWithTagCommand>());
            registry.Register("send_message_with_game_object", "send_message_with_game_object(gameobject,msg,arg1,arg2,...) command", new ExpressionFactoryHelper<GmCommands.SendMessageWithGameObjectCommand>());
            registry.Register("create_game_object", "create_game_object(name,prefab[,parent])[obj(\"varname\")]{position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));loadtimeout(1000);disable(\"typename\", \"typename\", ...);remove(\"typename\", \"typename\", ...);} command", new ExpressionFactoryHelper<GmCommands.CreateGameObjectCommand>());
            registry.Register("set_transform", "set_transform(name,local0_or_world1){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));} command", new ExpressionFactoryHelper<GmCommands.SetTransformCommand>());
            registry.Register("add_transform", "add_transform(name,local0_or_world1){position(vector3(x,y,z));rotation(vector3(x,y,z));scale(vector3(x,y,z));} command", new ExpressionFactoryHelper<GmCommands.AddTransformCommand>());
            registry.Register("destroy_game_object", "destroy_game_object(path) command", new ExpressionFactoryHelper<GmCommands.DestroyGameObjectCommand>());
            registry.Register("set_parent", "set_parent(obj_or_path,parent,int_stay_world_pos) command", new ExpressionFactoryHelper<GmCommands.SetParentCommand>());
            registry.Register("set_active", "set_active(obj_or_path,1_or_0) command", new ExpressionFactoryHelper<GmCommands.SetActiveCommand>());
            registry.Register("set_visible", "set_visible(obj_or_path,1_or_0) command", new ExpressionFactoryHelper<GmCommands.SetVisibleCommand>());
            registry.Register("add_component", "add_component(obj_or_path,type_or_str)[obj(\"varname\")] command", new ExpressionFactoryHelper<GmCommands.AddComponentCommand>());
            registry.Register("remove_component", "remove_component(obj_or_path,type_or_str) command", new ExpressionFactoryHelper<GmCommands.RemoveComponentCommand>());
            registry.Register("open_url", "open_url(url) command", new ExpressionFactoryHelper<GmCommands.OpenUrlCommand>());
            registry.Register("quit", "quit() command", new ExpressionFactoryHelper<GmCommands.QuitCommand>());

            registry.Register("game_object_animation", "game_object_animation(obj, anim[, normalized_time]) command", new ExpressionFactoryHelper<GmCommands.GameObjectAnimationCommand>());
            registry.Register("game_object_animation_param", "game_object_animation_param(obj){float(name,val);int(name,val);bool(name,val);trigger(name,val);} command", new ExpressionFactoryHelper<GmCommands.GameObjectAnimationParamCommand>());

            //register value or function
            registry.Register("get_time", "get_time() function", new ExpressionFactoryHelper<GmCommands.GetTimeFunction>());
            registry.Register("get_time_scale", "get_time_scale() function", new ExpressionFactoryHelper<GmCommands.GetTimeScaleFunction>());
            registry.Register("is_active", "is_active(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsActiveFunction>());
            registry.Register("is_really_active", "is_really_active(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsReallyActiveFunction>());
            registry.Register("is_visible", "is_visible(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.IsVisibleFunction>());
            registry.Register("get_component", "get_component(obj_or_path,type_or_str) function", new ExpressionFactoryHelper<GmCommands.GetComponentFunction>());
            registry.Register("get_component_in_parent", "get_component_in_parent(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentInParentFunction>());
            registry.Register("api_list", "api_list([regex,...]) - regex search over all APIs/docs registered on the current runtime interpreter (like helpall)", new ExpressionFactoryHelper<GmCommands.ApiListCommand>());
            registry.Register("get_component_in_children", "get_component_in_children(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentInChildrenFunction>());
            registry.Register("get_components", "get_components(obj_or_path,type_or_str) function", new ExpressionFactoryHelper<GmCommands.GetComponentsFunction>());
            registry.Register("get_components_in_parent", "get_components_in_parent(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentsInParentFunction>());
            registry.Register("get_components_in_children", "get_components_in_children(obj_or_path,type_or_str[,int_inc_inactive]) function", new ExpressionFactoryHelper<GmCommands.GetComponentsInChildrenFunction>());
            registry.Register("get_game_object", "get_game_object(obj_or_path) or get_game_object(obj_or_path){disable(comp1,comp2,...);remove(comp1,comp2,...);} function", new ExpressionFactoryHelper<GmCommands.GetGameObjectFunction>());
            registry.Register("get_parent", "get_parent(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetParentFunction>());
            registry.Register("get_child", "get_child(obj_or_path,child_path) function", new ExpressionFactoryHelper<GmCommands.GetChildFunction>());
            registry.Register("get_unity_type", "get_unity_type(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUnityTypeFunction>());
            registry.Register("get_unity_ui_type", "get_unity_ui_type(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUnityUiTypeFunction>());
            registry.Register("get_user_type", "get_user_type(type_str) function", new ExpressionFactoryHelper<GmCommands.GetUserTypeFunction>());

            registry.Register("get_position", "get_position(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionFunction>());
            registry.Register("get_position_x", "get_position_x(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionXFunction>());
            registry.Register("get_position_y", "get_position_y(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionYFunction>());
            registry.Register("get_position_z", "get_position_z(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetPositionZFunction>());
            registry.Register("get_rotation", "get_rotation(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationFunction>());
            registry.Register("get_rotation_x", "get_rotation_x(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationXFunction>());
            registry.Register("get_rotation_y", "get_rotation_y(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationYFunction>());
            registry.Register("get_rotation_z", "get_rotation_z(obj_or_path[,local0_or_world1]) function", new ExpressionFactoryHelper<GmCommands.GetRotationZFunction>());
            registry.Register("get_scale", "get_scale(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleFunction>());
            registry.Register("get_scale_x", "get_scale_x(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleXFunction>());
            registry.Register("get_scale_y", "get_scale_y(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleYFunction>());
            registry.Register("get_scale_z", "get_scale_z(obj_or_path) function", new ExpressionFactoryHelper<GmCommands.GetScaleZFunction>());

            registry.Register("position", "position(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());
            registry.Register("rotation", "rotation(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());
            registry.Register("scale", "scale(x,y,z) function", new ExpressionFactoryHelper<GmCommands.Vector3Function>());

            registry.Register("deg_2_rad", "deg_2_rad(deg) function", new ExpressionFactoryHelper<GmCommands.Deg2RadFunction>());
            registry.Register("rad_2_deg", "rad_2_deg(rad) function", new ExpressionFactoryHelper<GmCommands.Rad2DegFunction>());

            registry.Register("get_file_name", "get_file_name(path) function", new ExpressionFactoryHelper<GmCommands.GetFileNameFunction>());
            registry.Register("get_dir_name", "get_dir_name(path) function", new ExpressionFactoryHelper<GmCommands.GetDirectoryNameFunction>());
            registry.Register("get_extension", "get_extension(path) function", new ExpressionFactoryHelper<GmCommands.GetExtensionFunction>());
            registry.Register("combine_path", "combine_path(path1,path2) function", new ExpressionFactoryHelper<GmCommands.CombinePathFunction>());
            registry.Register("get_streaming_assets", "get_streaming_assets() function", new ExpressionFactoryHelper<GmCommands.GetStreamingAssetsFunction>());
            registry.Register("get_persistent_path", "get_persistent_path() function", new ExpressionFactoryHelper<GmCommands.GetPersistentPathFunction>());

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
