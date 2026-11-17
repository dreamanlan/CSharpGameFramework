using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story
{
    public sealed class GfxStorySystem
    {
        public int SceneId
        {
            get { return m_SceneId; }
            set { m_SceneId = value; }
        }
        public void Init()
        {
            var registry = DslCalculatorHost.GetSharedApiRegistry();

            //register story commands
            registry.Register("preload", "preload command", new ExpressionFactoryHelper<Story.Commands.PreloadCommand>());
            registry.Register("start_story", "start_story command", new ExpressionFactoryHelper<Story.Commands.StartStoryCommand>());
            registry.Register("stop_story", "stop_story command", new ExpressionFactoryHelper<Story.Commands.StopStoryCommand>());
            registry.Register("wait_story", "wait_story command", new ExpressionFactoryHelper<Story.Commands.WaitStoryCommand>());
            registry.Register("pause_story", "pause_story command", new ExpressionFactoryHelper<Story.Commands.PauseStoryCommand>());
            registry.Register("resume_story", "resume_story command", new ExpressionFactoryHelper<Story.Commands.ResumeStoryCommand>());
            registry.Register("fire_message", "fire_message command", new ExpressionFactoryHelper<Story.Commands.FireMessageCommand>());
            registry.Register("fire_concurrent_message", "fire_concurrent_message command", new ExpressionFactoryHelper<Story.Commands.FireConcurrentMessageCommand>());
            registry.Register("wait_all_message", "wait_all_message command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageCommand>());
            registry.Register("wait_all_message_handler", "wait_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
            registry.Register("suspend_all_message_handler", "suspend_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
            registry.Register("resume_all_message_handler", "resume_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
            registry.Register("send_room_story_message", "send_room_story_message command", new ExpressionFactoryHelper<Story.Commands.SendRoomStoryMessageCommand>());
            registry.Register("send_server_story_message", "send_server_story_message command", new ExpressionFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
            registry.Register("send_client_story_message", "send_client_story_message command", new ExpressionFactoryHelper<Story.Commands.DummyCommand>());
            registry.Register("publish_gfx_event", "publish_gfx_event command", new ExpressionFactoryHelper<Story.Commands.PublishGfxEventCommand>());
            registry.Register("send_gfx_message", "send_gfx_message command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageCommand>());
            registry.Register("send_gfx_message_with_tag", "send_gfx_message_with_tag command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
            registry.Register("send_gfx_message_with_game_object", "send_gfx_message_with_game_object command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithGameObjectCommand>());
            registry.Register("send_skill_message", "send_skill_message command", new ExpressionFactoryHelper<Story.Commands.SendSkillMessageCommand>());
            registry.Register("create_game_object", "create_game_object command", new ExpressionFactoryHelper<Story.Commands.CreateGameObjectCommand>());
            registry.Register("set_transform", "set_transform command", new ExpressionFactoryHelper<Story.Commands.SetTransformCommand>());
            registry.Register("add_transform", "add_transform command", new ExpressionFactoryHelper<Story.Commands.AddTransformCommand>());
            registry.Register("destroy_game_object", "destroy_game_object command", new ExpressionFactoryHelper<Story.Commands.DestroyGameObjectCommand>());
            registry.Register("set_parent", "set_parent command", new ExpressionFactoryHelper<Story.Commands.SetParentCommand>());
            registry.Register("set_active", "set_active command", new ExpressionFactoryHelper<Story.Commands.SetActiveCommand>());
            registry.Register("set_visible", "set_visible command", new ExpressionFactoryHelper<Story.Commands.SetVisibleCommand>());
            registry.Register("put_on_ground", "put_on_ground command", new ExpressionFactoryHelper<Story.Commands.PutOnGroundCommand>());
            registry.Register("set_nav_mesh_agent_enable", "set_nav_mesh_agent_enable command", new ExpressionFactoryHelper<Story.Commands.SetNavMeshAgentEnabledCommand>());
            registry.Register("set_nav_mesh_agent_enabled", "set_nav_mesh_agent_enabled command", new ExpressionFactoryHelper<Story.Commands.SetNavMeshAgentEnabledCommand>());
            registry.Register("add_component", "add_component command", new ExpressionFactoryHelper<Story.Commands.AddComponentCommand>());
            registry.Register("remove_component", "remove_component command", new ExpressionFactoryHelper<Story.Commands.RemoveComponentCommand>());
            registry.Register("install_plugin", "install_plugin command", new ExpressionFactoryHelper<Story.Commands.InstallPluginCommand>());
            registry.Register("remove_plugin", "remove_plugin command", new ExpressionFactoryHelper<Story.Commands.RemovePluginCommand>());
            registry.Register("open_url", "open_url command", new ExpressionFactoryHelper<Story.Commands.OpenUrlCommand>());
            registry.Register("quit", "quit command", new ExpressionFactoryHelper<Story.Commands.QuitCommand>());
            registry.Register("load_ui", "load_ui command", new ExpressionFactoryHelper<Story.Commands.LoadUiCommand>());
            registry.Register("bind_ui", "bind_ui command", new ExpressionFactoryHelper<Story.Commands.BindUiCommand>());
            registry.Register("set_actor_scale", "set_actor_scale command", new ExpressionFactoryHelper<Story.Commands.SetActorScaleCommand>());

            registry.Register("change_scene", "change_scene command", new ExpressionFactoryHelper<Story.Commands.ChangeSceneCommand>());
            registry.Register("open_battle", "open_battle command", new ExpressionFactoryHelper<Story.Commands.OpenBattleCommand>());
            registry.Register("close_battle", "close_battle command", new ExpressionFactoryHelper<Story.Commands.CloseBattleCommand>());
            registry.Register("create_scene_logic", "create_scene_logic command", new ExpressionFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
            registry.Register("destroy_scene_logic", "destroy_scene_logic command", new ExpressionFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
            registry.Register("pause_scene_logic", "pause_scene_logic command", new ExpressionFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
            registry.Register("restart_timeout", "restart_timeout command", new ExpressionFactoryHelper<Story.Commands.RestartTimeoutCommand>());
            registry.Register("highlight_prompt_with_dict", "highlight_prompt_with_dict command", new ExpressionFactoryHelper<Story.Commands.HighlightPromptWithDictCommand>());
            registry.Register("highlight_prompt", "highlight_prompt command", new ExpressionFactoryHelper<Story.Commands.HighlightPromptCommand>());

            registry.Register("blackboard_set", "blackboard_set command", new ExpressionFactoryHelper<Story.Commands.BlackboardSetCommand>());
            registry.Register("blackboard_clear", "blackboard_clear command", new ExpressionFactoryHelper<Story.Commands.BlackboardClearCommand>());
            registry.Register("camera_follow", "camera_follow command", new ExpressionFactoryHelper<Story.Commands.CameraFollowCommand>());
            registry.Register("camera_look", "camera_look command", new ExpressionFactoryHelper<Story.Commands.CameraLookCommand>());
            registry.Register("camera_look_immediately", "camera_look_immediately command", new ExpressionFactoryHelper<Story.Commands.CameraLookImmediatelyCommand>());
            registry.Register("camera_look_toward", "camera_look_toward command", new ExpressionFactoryHelper<Story.Commands.CameraLookTowardCommand>());
            registry.Register("camera_fixed_yaw", "camera_fixed_yaw command", new ExpressionFactoryHelper<Story.Commands.CameraFixedYawCommand>());
            registry.Register("camera_yaw", "camera_yaw command", new ExpressionFactoryHelper<Story.Commands.CameraYawCommand>());
            registry.Register("camera_height", "camera_height command", new ExpressionFactoryHelper<Story.Commands.CameraHeightCommand>());
            registry.Register("camera_distance", "camera_distance command", new ExpressionFactoryHelper<Story.Commands.CameraDistanceCommand>());
            registry.Register("camera_set_distance_height", "camera_set_distance_height command", new ExpressionFactoryHelper<Story.Commands.CameraSetDistanceHeightCommand>());
            registry.Register("camera_reset_distance_height", "camera_reset_distance_height command", new ExpressionFactoryHelper<Story.Commands.CameraResetDistanceHeightCommand>());
            registry.Register("camera_set_follow_speed", "camera_set_follow_speed command", new ExpressionFactoryHelper<Story.Commands.CameraSetFollowSpeedCommand>());
            registry.Register("camera_reset_follow_speed", "camera_reset_follow_speed command", new ExpressionFactoryHelper<Story.Commands.CameraResetFollowSpeedCommand>());
            registry.Register("camera_follow_obj", "camera_follow_obj command", new ExpressionFactoryHelper<Story.Commands.CameraFollowObjCommand>());
            registry.Register("camera_look_obj", "camera_look_obj command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjCommand>());
            registry.Register("camera_look_obj_immediately", "camera_look_obj_immediately command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjImmediatelyCommand>());
            registry.Register("camera_look_toward_obj", "camera_look_toward_obj command", new ExpressionFactoryHelper<Story.Commands.CameraLookTowardObjCommand>());
            registry.Register("camera_look_copy", "camera_look_copy command", new ExpressionFactoryHelper<Story.Commands.CameraLookCopyCommand>());
            registry.Register("camera_look_obj_copy", "camera_look_obj_copy command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjCopyCommand>());
            registry.Register("camera_enable", "camera_enable command", new ExpressionFactoryHelper<Story.Commands.CameraEnableCommand>());
            registry.Register("lock_frame", "lock_frame command", new ExpressionFactoryHelper<Story.Commands.LockFrameCommand>());
            registry.Register("show_dlg", "show_dlg command", new ExpressionFactoryHelper<Story.Commands.ShowDlgCommand>());
            registry.Register("area_detect", "area_detect command", new ExpressionFactoryHelper<Story.Commands.AreaDetectCommand>());

            registry.Register("game_object_animation", "game_object_animation command", new ExpressionFactoryHelper<Story.Commands.GameObjectAnimationCommand>());
            registry.Register("game_object_animation_param", "game_object_animation_param command", new ExpressionFactoryHelper<Story.Commands.GameObjectAnimationParamCommand>());
            registry.Register("game_object_cast_skill", "game_object_cast_skill command", new ExpressionFactoryHelper<Story.Commands.GameObjectCastSkillCommand>());
            registry.Register("game_object_stop_skill", "game_object_stop_skill command", new ExpressionFactoryHelper<Story.Commands.GameObjectStopSkillCommand>());

            registry.Register("create_npc", "create_npc command", new ExpressionFactoryHelper<Story.Commands.CreateNpcCommand>());
            registry.Register("destroy_npc", "destroy_npc command", new ExpressionFactoryHelper<Story.Commands.DestroyNpcCommand>());
            registry.Register("destroy_npc_with_obj_id", "destroy_npc_with_obj_id command", new ExpressionFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
            registry.Register("npc_face", "npc_face command", new ExpressionFactoryHelper<Story.Commands.NpcFaceCommand>());
            registry.Register("npc_move", "npc_move command", new ExpressionFactoryHelper<Story.Commands.NpcMoveCommand>());
            registry.Register("npc_move_with_waypoints", "npc_move_with_waypoints command", new ExpressionFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
            registry.Register("npc_stop", "npc_stop command", new ExpressionFactoryHelper<Story.Commands.NpcStopCommand>());
            registry.Register("npc_attack", "npc_attack command", new ExpressionFactoryHelper<Story.Commands.NpcAttackCommand>());
            registry.Register("npc_set_formation", "npc_set_formation command", new ExpressionFactoryHelper<Story.Commands.NpcSetFormationCommand>());
            registry.Register("npc_enable_ai", "npc_enable_ai command", new ExpressionFactoryHelper<Story.Commands.NpcEnableAiCommand>());
            registry.Register("npc_set_ai", "npc_set_ai command", new ExpressionFactoryHelper<Story.Commands.NpcSetAiCommand>());
            registry.Register("npc_set_ai_target", "npc_set_ai_target command", new ExpressionFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
            registry.Register("npc_animation", "npc_animation command", new ExpressionFactoryHelper<Story.Commands.NpcAnimationCommand>());
            registry.Register("npc_animation_param", "npc_animation_param command", new ExpressionFactoryHelper<Story.Commands.NpcAnimationParamCommand>());
            registry.Register("npc_add_impact", "npc_add_impact command", new ExpressionFactoryHelper<Story.Commands.NpcAddImpactCommand>());
            registry.Register("npc_remove_impact", "npc_remove_impact command", new ExpressionFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
            registry.Register("npc_cast_skill", "npc_cast_skill command", new ExpressionFactoryHelper<Story.Commands.NpcCastSkillCommand>());
            registry.Register("npc_stop_skill", "npc_stop_skill command", new ExpressionFactoryHelper<Story.Commands.NpcStopSkillCommand>());
            registry.Register("npc_listen", "npc_listen command", new ExpressionFactoryHelper<Story.Commands.NpcListenCommand>());
            registry.Register("npc_set_camp", "npc_set_camp command", new ExpressionFactoryHelper<Story.Commands.NpcSetCampCommand>());
            registry.Register("npc_set_summoner_id", "npc_set_summoner_id command", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
            registry.Register("npc_set_summon_skill_id", "npc_set_summon_skill_id command", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
            registry.Register("obj_face", "obj_face command", new ExpressionFactoryHelper<Story.Commands.ObjFaceCommand>());
            registry.Register("obj_move", "obj_move command", new ExpressionFactoryHelper<Story.Commands.ObjMoveCommand>());
            registry.Register("obj_move_with_waypoints", "obj_move_with_waypoints command", new ExpressionFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
            registry.Register("obj_stop", "obj_stop command", new ExpressionFactoryHelper<Story.Commands.ObjStopCommand>());
            registry.Register("obj_attack", "obj_attack command", new ExpressionFactoryHelper<Story.Commands.ObjAttackCommand>());
            registry.Register("obj_set_formation", "obj_set_formation command", new ExpressionFactoryHelper<Story.Commands.ObjSetFormationCommand>());
            registry.Register("obj_enable_ai", "obj_enable_ai command", new ExpressionFactoryHelper<Story.Commands.ObjEnableAiCommand>());
            registry.Register("obj_set_ai", "obj_set_ai command", new ExpressionFactoryHelper<Story.Commands.ObjSetAiCommand>());
            registry.Register("obj_set_ai_target", "obj_set_ai_target command", new ExpressionFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
            registry.Register("obj_animation", "obj_animation command", new ExpressionFactoryHelper<Story.Commands.ObjAnimationCommand>());
            registry.Register("obj_animation_param", "obj_animation_param command", new ExpressionFactoryHelper<Story.Commands.ObjAnimationParamCommand>());
            registry.Register("obj_add_impact", "obj_add_impact command", new ExpressionFactoryHelper<Story.Commands.ObjAddImpactCommand>());
            registry.Register("obj_remove_impact", "obj_remove_impact command", new ExpressionFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
            registry.Register("obj_cast_skill", "obj_cast_skill command", new ExpressionFactoryHelper<Story.Commands.ObjCastSkillCommand>());
            registry.Register("obj_stop_skill", "obj_stop_skill command", new ExpressionFactoryHelper<Story.Commands.ObjStopSkillCommand>());
            registry.Register("obj_listen", "obj_listen command", new ExpressionFactoryHelper<Story.Commands.ObjListenCommand>());
            registry.Register("obj_set_camp", "obj_set_camp command", new ExpressionFactoryHelper<Story.Commands.ObjSetCampCommand>());
            registry.Register("obj_set_summoner_id", "obj_set_summoner_id command", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
            registry.Register("obj_set_summon_skill_id", "obj_set_summon_skill_id command", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());

            registry.Register("set_hp", "set_hp command", new ExpressionFactoryHelper<Story.Commands.SetHpCommand>());
            registry.Register("set_energy", "set_energy command", new ExpressionFactoryHelper<Story.Commands.SetEnergyCommand>());
            registry.Register("obj_set", "obj_set command", new ExpressionFactoryHelper<Story.Commands.ObjSetCommand>());
            registry.Register("set_level", "set_level command", new ExpressionFactoryHelper<Story.Commands.SetLevelCommand>());
            registry.Register("set_attr", "set_attr command", new ExpressionFactoryHelper<Story.Commands.SetAttrCommand>());
            registry.Register("set_unit_id", "set_unit_id command", new ExpressionFactoryHelper<Story.Commands.SetUnitIdCommand>());
            registry.Register("set_leader_id", "set_leader_id command", new ExpressionFactoryHelper<Story.Commands.SetLeaderIdCommand>());
            registry.Register("mark_control_by_story", "mark_control_by_story command", new ExpressionFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

            //register value or functions
            registry.Register("get_time", "get_time function", new ExpressionFactoryHelper<Story.Functions.GetTimeFunction>());
            registry.Register("get_time_scale", "get_time_scale function", new ExpressionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
            registry.Register("is_active", "is_active function", new ExpressionFactoryHelper<Story.Functions.IsActiveFunction>());
            registry.Register("is_really_active", "is_really_active function", new ExpressionFactoryHelper<Story.Functions.IsReallyActiveFunction>());
            registry.Register("is_visible", "is_visible function", new ExpressionFactoryHelper<Story.Functions.IsVisibleFunction>());
            registry.Register("is_navmesh_agent_enabled", "is_navmesh_agent_enabled function", new ExpressionFactoryHelper<Story.Functions.IsNavmeshAgentEnabledFunction>());
            registry.Register("get_component", "get_component function", new ExpressionFactoryHelper<Story.Functions.GetComponentFunction>());
            registry.Register("get_component_in_parent", "get_component_in_parent function", new ExpressionFactoryHelper<Story.Functions.GetComponentInParentFunction>());
            registry.Register("get_component_in_children", "get_component_in_children function", new ExpressionFactoryHelper<Story.Functions.GetComponentInChildrenFunction>());
            registry.Register("get_components", "get_components function", new ExpressionFactoryHelper<Story.Functions.GetComponentsFunction>());
            registry.Register("get_components_in_parent", "get_components_in_parent function", new ExpressionFactoryHelper<Story.Functions.GetComponentsInParentFunction>());
            registry.Register("get_components_in_children", "get_components_in_children function", new ExpressionFactoryHelper<Story.Functions.GetComponentsInChildrenFunction>());
            registry.Register("get_game_object", "get_game_object function", new ExpressionFactoryHelper<Story.Functions.GetGameObjectFunction>());
            registry.Register("get_parent", "get_parent function", new ExpressionFactoryHelper<Story.Functions.GetParentFunction>());
            registry.Register("find_child", "find_child function", new ExpressionFactoryHelper<Story.Functions.FindChildFunction>());
            registry.Register("get_child_count", "get_child_count function", new ExpressionFactoryHelper<Story.Functions.GetChildCountFunction>());
            registry.Register("get_child", "get_child function", new ExpressionFactoryHelper<Story.Functions.GetChildFunction>());
            registry.Register("get_unity_type", "get_unity_type function", new ExpressionFactoryHelper<Story.Functions.GetUnityTypeFunction>());
            registry.Register("get_unity_ui_type", "get_unity_ui_type function", new ExpressionFactoryHelper<Story.Functions.GetUnityUiTypeFunction>());
            registry.Register("get_user_type", "get_user_type function", new ExpressionFactoryHelper<Story.Functions.GetUserTypeFunction>());
            registry.Register("get_actor", "get_actor function", new ExpressionFactoryHelper<Story.Functions.GetActorFunction>());
            registry.Register("get_entity_info", "get_entity_info function", new ExpressionFactoryHelper<Story.Functions.GetEntityInfoFunction>());
            registry.Register("get_global", "get_global function", new ExpressionFactoryHelper<Story.Functions.GetGlobalFunction>());
            registry.Register("pluginFramework", "pluginFramework function", new ExpressionFactoryHelper<Story.Functions.PluginFrameworkFunction>());
            registry.Register("get_entity_view", "get_entity_view function", new ExpressionFactoryHelper<Story.Functions.GetEntityViewFunction>());
            registry.Register("dict_get", "dict_get function", new ExpressionFactoryHelper<Story.Functions.DictGetFunction>());
            registry.Register("dict_format", "dict_format function", new ExpressionFactoryHelper<Story.Functions.DictFormatFunction>());
            registry.Register("is_client", "is_client function", new ExpressionFactoryHelper<Story.Functions.IsClientFunction>());
            registry.Register("get_scene_id", "get_scene_id function", new ExpressionFactoryHelper<Story.Functions.GetSceneIdFunction>());

            registry.Register("blackboard_get", "blackboard_get function", new ExpressionFactoryHelper<Story.Functions.BlackboardGetFunction>());
            registry.Register("get_dialog_item", "get_dialog_item function", new ExpressionFactoryHelper<Story.Functions.GetDialogItemFunction>());
            registry.Register("offset_spline", "offset_spline function", new ExpressionFactoryHelper<Story.Functions.OffsetSplineFunction>());
            registry.Register("offset_vector3", "offset_vector3 function", new ExpressionFactoryHelper<Story.Functions.OffsetVector3Function>());
            registry.Register("get_actor_icon", "get_actor_icon function", new ExpressionFactoryHelper<Story.Functions.GetActorIconFunction>());
            registry.Register("get_monster_info", "get_monster_info function", new ExpressionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
            registry.Register("get_ai_data", "get_ai_data function", new ExpressionFactoryHelper<Story.Functions.GetAiDataFunction>());

            registry.Register("npc_id_list", "npc_id_list function", new ExpressionFactoryHelper<Story.Functions.NpcIdListFunction>());
            registry.Register("combat_npc_count", "combat_npc_count function", new ExpressionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
            registry.Register("npc_count", "npc_count function", new ExpressionFactoryHelper<Story.Functions.NpcCountFunction>());
            registry.Register("unit_id_2_obj_id", "unit_id_2_obj_id function", new ExpressionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
            registry.Register("obj_id_2_unit_id", "obj_id_2_unit_id function", new ExpressionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
            registry.Register("unit_id_2_unique_id", "unit_id_2_unique_id function", new ExpressionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
            registry.Register("obj_id_2_unique_id", "obj_id_2_unique_id function", new ExpressionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());

            registry.Register("npc_get_formation", "npc_get_formation function", new ExpressionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
            registry.Register("npc_get_npc_type", "npc_get_npc_type function", new ExpressionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
            registry.Register("npc_get_summoner_id", "npc_get_summoner_id function", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
            registry.Register("npc_get_summon_skill_id", "npc_get_summon_skill_id function", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
            registry.Register("npc_find_impact_seq_by_id", "npc_find_impact_seq_by_id function", new ExpressionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

            registry.Register("obj_get_formation", "obj_get_formation function", new ExpressionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
            registry.Register("obj_get_npc_type", "obj_get_npc_type function", new ExpressionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
            registry.Register("obj_get_summoner_id", "obj_get_summoner_id function", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
            registry.Register("obj_get_summon_skill_id", "obj_get_summon_skill_id function", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
            registry.Register("obj_find_impact_seq_by_id", "obj_find_impact_seq_by_id function", new ExpressionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
            registry.Register("is_enemy", "is_enemy function", new ExpressionFactoryHelper<Story.Functions.IsEnemyFunction>());
            registry.Register("is_friend", "is_friend function", new ExpressionFactoryHelper<Story.Functions.IsFriendFunction>());

            registry.Register("get_position", "get_position function", new ExpressionFactoryHelper<Story.Functions.GetPositionFunction>());
            registry.Register("get_position_x", "get_position_x function", new ExpressionFactoryHelper<Story.Functions.GetPositionXFunction>());
            registry.Register("get_position_y", "get_position_y function", new ExpressionFactoryHelper<Story.Functions.GetPositionYFunction>());
            registry.Register("get_position_z", "get_position_z function", new ExpressionFactoryHelper<Story.Functions.GetPositionZFunction>());
            registry.Register("get_rotation", "get_rotation function", new ExpressionFactoryHelper<Story.Functions.GetRotationFunction>());
            registry.Register("get_rotation_x", "get_rotation_x function", new ExpressionFactoryHelper<Story.Functions.GetRotationXFunction>());
            registry.Register("get_rotation_y", "get_rotation_y function", new ExpressionFactoryHelper<Story.Functions.GetRotationYFunction>());
            registry.Register("get_rotation_z", "get_rotation_z function", new ExpressionFactoryHelper<Story.Functions.GetRotationZFunction>());
            registry.Register("get_scale", "get_scale function", new ExpressionFactoryHelper<Story.Functions.GetScaleFunction>());
            registry.Register("get_scale_x", "get_scale_x function", new ExpressionFactoryHelper<Story.Functions.GetScaleXFunction>());
            registry.Register("get_scale_y", "get_scale_y function", new ExpressionFactoryHelper<Story.Functions.GetScaleYFunction>());
            registry.Register("get_scale_z", "get_scale_z function", new ExpressionFactoryHelper<Story.Functions.GetScaleZFunction>());
            registry.Register("position", "position function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("rotation", "rotation function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("scale", "scale function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("get_camp", "get_camp function", new ExpressionFactoryHelper<Story.Functions.GetCampFunction>());
            registry.Register("is_combat_npc", "is_combat_npc function", new ExpressionFactoryHelper<Story.Functions.IsCombatNpcFunction>());
            registry.Register("get_hp", "get_hp function", new ExpressionFactoryHelper<Story.Functions.GetHpFunction>());
            registry.Register("get_energy", "get_energy function", new ExpressionFactoryHelper<Story.Functions.GetEnergyFunction>());
            registry.Register("get_max_hp", "get_max_hp function", new ExpressionFactoryHelper<Story.Functions.GetMaxHpFunction>());
            registry.Register("get_max_energy", "get_max_energy function", new ExpressionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
            registry.Register("calc_offset", "calc_offset function", new ExpressionFactoryHelper<Story.Functions.CalcOffsetFunction>());
            registry.Register("calc_dir", "calc_dir function", new ExpressionFactoryHelper<Story.Functions.CalcDirFunction>());
            registry.Register("obj_get", "obj_get function", new ExpressionFactoryHelper<Story.Functions.ObjGetFunction>());
            registry.Register("get_table_id", "get_table_id function", new ExpressionFactoryHelper<Story.Functions.GetTableIdFunction>());
            registry.Register("get_level", "get_level function", new ExpressionFactoryHelper<Story.Functions.GetLevelFunction>());
            registry.Register("get_attr", "get_attr function", new ExpressionFactoryHelper<Story.Functions.GetAttrFunction>());
            registry.Register("is_control_by_story", "is_control_by_story function", new ExpressionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
            registry.Register("can_cast_skill", "can_cast_skill function", new ExpressionFactoryHelper<Story.Functions.CanCastSkillFunction>());
            registry.Register("is_under_control", "is_under_control function", new ExpressionFactoryHelper<Story.Functions.IsUnderControlFunction>());
            registry.Register("get_player_id", "get_player_id function", new ExpressionFactoryHelper<Story.Functions.GetPlayerIdFunction>());
            registry.Register("get_player_unit_id", "get_player_unit_id function", new ExpressionFactoryHelper<Story.Functions.GetPlayerUnitIdFunction>());
            registry.Register("get_leader_id", "get_leader_id function", new ExpressionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
            registry.Register("get_leader_table_id", "get_leader_table_id function", new ExpressionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());
            registry.Register("get_member_count", "get_member_count function", new ExpressionFactoryHelper<Story.Functions.GetMemberCountFunction>());
            registry.Register("get_member_table_id", "get_member_table_id function", new ExpressionFactoryHelper<Story.Functions.GetMemberTableIdFunction>());
            registry.Register("get_member_level", "get_member_level function", new ExpressionFactoryHelper<Story.Functions.GetMemberLevelFunction>());

            GmCommands.GmExpressionRegistrar.RegisterGmExpressions(registry);
        }
        public void Reset()
        {
            Reset(true);
        }
        public void Reset(bool logIfTriggered)
        {
            m_ContextVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info) {
                    info.Reset(logIfTriggered);
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }
        public void LoadSceneStories()
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(m_SceneId);
            if (null != cfg) {
                string[] filePath;
                int ct1 = cfg.SceneDslFile.Count;
                int ct2 = cfg.ClientDslFile.Count;
                filePath = new string[ct1 + ct2];
                for (int i = 0; i < ct1; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.SceneDslFile[i]);
                }
                for (int i = 0; i < ct2; i++) {
                    filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.ClientDslFile[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, string.Empty, filePath);
                Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(0);
                if (null != stories) {
                    foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                        AddStoryInstance(pair.Key, pair.Value.Clone());
                    }
                }
            }
        }
        public void LoadBattleStories(int sceneId)
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(sceneId);
            if (null != cfg) {
                string[] filePath;
                int ct1 = cfg.SceneDslFile.Count;
                int ct2 = cfg.ClientDslFile.Count;
                filePath = new string[ct1 + ct2];
                for (int i = 0; i < ct1; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.SceneDslFile[i]);
                }
                for (int i = 0; i < ct2; i++) {
                    filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.ClientDslFile[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, string.Empty, filePath);
                Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(0);
                if (null != stories) {
                    foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                        AddStoryInstance(pair.Key, pair.Value.Clone());
                    }
                }
            }
        }
        public void LoadStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AddStoryInstance(pair.Key, pair.Value.Clone());
                }
            }
        }
        public void LoadAiStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                    info.m_StoryInstance = pair.Value.Clone();
                    info.m_IsUsed = false;
                    AddAiStoryInstanceInfoToPool(pair.Key, info);
                }
            }
        }
        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
            m_AiStoryInstancePool.Clear();
        }

        public AiStoryInstanceInfo NewAiStoryInstance(string storyId, string _namespace, params string[] aiFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            AiStoryInstanceInfo instInfo = GetUnusedAiStoryInstanceInfoFromPool(storyId);
            if (null == instInfo) {
                int ct;
                string[] filePath;
                ct = aiFiles.Length;
                filePath = new string[ct];
                for (int i = 0; i < ct; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + aiFiles[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
                StoryInstance instance = StoryConfigManager.Instance.NewStoryInstance(storyId, 0);
                if (instance == null) {
                    LogSystem.Error("Can't load story config, story:{0} scene:{1} !", storyId, m_SceneId);
                    return null;
                }
                for (int ix = 0; ix < filePath.Length; ++ix) {
                    Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath[ix]);
                    if (null != stories) {
                        foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                            if (pair.Key != storyId) {
                                AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                                info.m_StoryInstance = pair.Value.Clone();
                                info.m_IsUsed = false;
                                AddAiStoryInstanceInfoToPool(pair.Key, info);
                            }
                        }
                    }
                }
                AiStoryInstanceInfo res = new AiStoryInstanceInfo();
                res.m_StoryInstance = instance;
                res.m_IsUsed = true;

                AddAiStoryInstanceInfoToPool(storyId, res);
                return res;
            } else {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }
        public void RecycleAiStoryInstance(AiStoryInstanceInfo info)
        {
            info.m_StoryInstance.Reset();
            info.m_IsUsed = false;
        }

        public int ActiveStoryCount
        {
            get
            {
                return m_StoryLogicInfos.Count;
            }
        }
        public IList<StoryInstance> ActiveStories
        {
            get { return m_StoryLogicInfos; }
        }
        public StrBoxedValueDict ContextVariables
        {
            get { return m_ContextVariables; }
        }
        public StoryInstance GetStory(string storyId)
        {
            return GetStory(storyId, string.Empty);
        }
        public StoryInstance GetStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            return GetStoryInstance(storyId);
        }
        public void StartStories(string storyId)
        {
            StartStories(storyId, string.Empty);
        }
        public void StartStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            foreach(var pair in m_StoryInstancePool) {
                var info = pair.Value;
                if (IsMatch(info.StoryId, storyId)) {
                    StopStory(info.StoryId);
                    m_StoryLogicInfos.Add(info);
                    info.Context = null;
                    info.ContextVariables = m_ContextVariables;
                    info.Start();

                    LogSystem.Info("StartStory {0} scene {1}", info.StoryId, m_SceneId);
                }
            }
        }
        public void PauseStories(string storyId, bool pause)
        {
            PauseStories(storyId, string.Empty, pause);
        }
        public void PauseStories(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStories(string storyId)
        {
            StopStories(storyId, string.Empty);
        }
        public void StopStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStories(string storyId)
        {
            return CountStories(storyId, string.Empty);
        }
        public int CountStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && IsMatch(info.StoryId, storyId) && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoriesTerminated(string storyId)
        {
            MarkStoriesTerminated(storyId, string.Empty);
        }
        public void MarkStoriesTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsTerminated = true;
                }
            }
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId)
        {
            SkipCurMessageHandlers(endMsg, storyId, string.Empty);
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.ClearMessage();
                    var enumer = info.GetMessageHandlerEnumerator();
                    while (enumer.MoveNext()) {
                        var handler = enumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                    var cenumer = info.GetConcurrentMessageHandlerEnumerator();
                    while (cenumer.MoveNext()) {
                        var handler = cenumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                }
            }
        }
        public void StartStory(string storyId)
        {
            StartStory(storyId, string.Empty);
        }
        public void StartStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            StoryInstance inst = GetStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = null;
                inst.ContextVariables = m_ContextVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            } else {
                LogSystem.Error("Can't load story, story:{0} scene:{1} !", storyId, m_SceneId);
            }
        }
        public void PauseStory(string storyId, bool pause)
        {
            PauseStory(storyId, string.Empty, pause);
        }
        public void PauseStory(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStory(string storyId)
        {
            StopStory(storyId, string.Empty);
        }
        public void StopStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStory(string storyId)
        {
            return CountStory(storyId, string.Empty);
        }
        public int CountStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && info.StoryId == storyId && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoryTerminated(string storyId)
        {
            MarkStoryTerminated(storyId, string.Empty);
        }
        public void MarkStoryTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsTerminated = true;
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
        public void AddBindedStory(UnityEngine.Object obj, StoryInstance inst)
        {
            m_BindedStoryInstances.Add(new BindedStoryInfo { Object = obj, Instance = inst });
        }
        public BoxedValueList NewBoxedValueList()
        {
            var args = m_BoxedValueListPool.Alloc();
            args.Clear();
            return args;
        }
        public void SendMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendMessage(msgId, args);
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
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        var newArgs = infos[ix].m_StoryInstance.NewBoxedValueList();
                        newArgs.AddRange(args);
                        infos[ix].m_StoryInstance.SendMessage(msgId, newArgs);
                    }
                }
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public void SendConcurrentMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendConcurrentMessage(msgId, newArgs);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        var newArgs = infos[ix].m_StoryInstance.NewBoxedValueList();
                        newArgs.AddRange(args);
                        infos[ix].m_StoryInstance.SendConcurrentMessage(msgId, newArgs);
                    }
                }
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public int CountMessage(string msgId)
        {
            int sum = 0;
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                sum += info.CountMessage(msgId);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        sum += infos[ix].m_StoryInstance.CountMessage(msgId);
                    }
                }
            }
            return sum;
        }
        public void SuspendMessageHandler(string msgId, bool suspend)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SuspendMessageHandler(msgId, suspend);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        infos[ix].m_StoryInstance.SuspendMessageHandler(msgId, suspend);
                    }
                }
            }
        }

        private void AddStoryInstance(string storyId, StoryInstance info)
        {
            if (!m_StoryInstancePool.ContainsKey(storyId)) {
                m_StoryInstancePool.Add(storyId, info);
            } else {
                m_StoryInstancePool[storyId] = info;
            }
        }
        private StoryInstance GetStoryInstance(string storyId)
        {
            StoryInstance info;
            m_StoryInstancePool.TryGetValue(storyId, out info);
            return info;
        }
        private void AddAiStoryInstanceInfoToPool(string storyId, AiStoryInstanceInfo info)
        {
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                infos.Add(info);
            } else {
                infos = new List<AiStoryInstanceInfo>();
                infos.Add(info);
                m_AiStoryInstancePool.Add(storyId, infos);
            }
        }
        private AiStoryInstanceInfo GetUnusedAiStoryInstanceInfoFromPool(string storyId)
        {
            AiStoryInstanceInfo info = null;
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                int ct = infos.Count;
                for (int ix = 0; ix < ct; ++ix) {
                    if (!infos[ix].m_IsUsed) {
                        info = infos[ix];
                        break;
                    }
                }
            }
            return info;
        }
        private bool IsMatch(string realId, string prefixId)
        {
            if (realId == prefixId || realId.Length > prefixId.Length && realId.StartsWith(prefixId) && realId[prefixId.Length] == ':')
                return true;
            return false;
        }

        private GfxStorySystem() { }

        private class BindedStoryInfo
        {
            internal UnityEngine.Object Object;
            internal StoryInstance Instance;
        }
        private List<BindedStoryInfo> m_BindedStoryInstances = new List<BindedStoryInfo>();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private int m_SceneId = 0;
        private StrBoxedValueDict m_ContextVariables = new StrBoxedValueDict();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Dictionary<string, List<AiStoryInstanceInfo>> m_AiStoryInstancePool = new Dictionary<string, List<AiStoryInstanceInfo>>();

        public static GfxStorySystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxStorySystem s_Instance = new GfxStorySystem();

        public static void ThreadInitMask()
        {
            //DslCalculator does not use thread masks
        }
    }
}
