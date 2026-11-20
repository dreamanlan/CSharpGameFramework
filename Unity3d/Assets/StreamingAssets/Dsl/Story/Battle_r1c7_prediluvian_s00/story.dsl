story(battle_main)
{
  // r1c7_s00 | 天外锚点 | 洪水前层 | branch | level=10151
  local
  {
    @wave(0);
    @spawned(0);
    @advancing(0);
    @total(2);
  };
  onmessage("start")
  {
    create_npc(1000,vector3(60,16.5,70),0,3,1,"ai_player",string_list("Ai/ailogic_player.dsl"));
    set_leader_id(unit_id_2_obj_id(1000));
    npc_set_formation(1000,0);
    loop(4){
      create_npc(1001+$$,vector3(60+rnd_int(0,10),16.5,70+rnd_int(0,10)),0,3,$$+2,"ai_member",string_list("Ai/ailogic_member.dsl"),unit_id_2_obj_id(1000));
      npc_set_formation(1001+$$,$$+1);
    };
    camera_follow();
    publish_gfx_event("loading_complete", "ui");
    fire_message("set_map_image", "Minimap/r1c7_s00_minimap");
    send_room_story_message("enter_parallel", "r1c7_s00");
    start_story("auto_battle");
    fire_message("push_tip_info", "【天外锚点】信标仍在发送坐标，可接收者早已离开这颗星球。");
    fire_message("push_tip_info", "时代层：洪水前层。支线记载，或许能补上别处缺的那一页。");
    fire_message("push_tip_info", "这是一处次要勘察点。");
    fire_message("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    fire_message("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      fire_message("push_tip_info", "坐标串起来了，但发送者仍未露面。");
      clear_message("show_dlg");
      fire_message("show_dlg",998);
    } else {
      if(@wave<2){
        fire_message("push_tip_info", "天外锚点：信标残座·A 降临与设施 · G 回归");
        fire_message("wave_spawn_1");
      } else {
      if(@wave<3){
        fire_message("push_tip_info", "天外锚点：废弃中转舱·A 降临与设施 · G 回归");
        fire_message("wave_advance_2");
      } else {
      };
      };
    };
  };
  onmessage("wave_spawn_1")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      create_npc(1026,vector3(get_position_x(unit_id_2_obj_id(1000))+14.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+0.0),0,4,6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1027,vector3(get_position_x(unit_id_2_obj_id(1000))+7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+12.1),0,4,7,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1028,vector3(get_position_x(unit_id_2_obj_id(1000))-7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+12.1),0,4,8,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1029,vector3(get_position_x(unit_id_2_obj_id(1000))-14.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+0.0),0,4,9,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1030,vector3(get_position_x(unit_id_2_obj_id(1000))-7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))-12.1),0,4,10,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1031,vector3(get_position_x(unit_id_2_obj_id(1000))+7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))-12.1),0,4,11,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
    };
  };
  onmessage("wave_spawn_2")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      create_npc(1046,vector3(get_position_x(unit_id_2_obj_id(1000))+14.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+0.0),0,4,6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1047,vector3(get_position_x(unit_id_2_obj_id(1000))+7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+12.1),0,4,7,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1048,vector3(get_position_x(unit_id_2_obj_id(1000))-7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+12.1),0,4,8,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1049,vector3(get_position_x(unit_id_2_obj_id(1000))-14.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+0.0),0,4,9,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1050,vector3(get_position_x(unit_id_2_obj_id(1000))-7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))-12.1),0,4,10,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1051,vector3(get_position_x(unit_id_2_obj_id(1000))+7.0,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))-12.1),0,4,11,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
    };
  };
  onmessage("wave_advance_2")
  {
    @advancing=1;
    fire_message("push_tip_info", "向下一处遗迹推进 20 步（第 2 波）");
    obj_move(unit_id_2_obj_id(1000),vector3(get_position_x(unit_id_2_obj_id(1000))+11.9,get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))+16.1),"npcarrived:1000");
    wait(4000);
    if(@advancing>0){
      @advancing=0;
      fire_message("wave_spawn_2");
    };
  };
  onmessage("npcarrived:1000")
  {
    if(@advancing>0){
      @advancing=0;
      if(@wave<2){
        fire_message("wave_spawn_1");
      } else {
      if(@wave<3){
        fire_message("wave_spawn_2");
      } else {
      };
      };
    };
  };
  onmessage("all_killed")
  {
    fire_message("wave_go");
  };
  onmessage("npc_killed:1000")
  {
    clear_message("show_dlg");
    fire_message("show_dlg",999);
  };
  onmessage("all_died")
  {
    clear_message("show_dlg");
    fire_message("show_dlg",999);
  };
  onmessage("dialog_over",998){ terminate(); close_battle(10151); };
  onmessage("dialog_over",999){ terminate(); close_battle(10151); };
};
