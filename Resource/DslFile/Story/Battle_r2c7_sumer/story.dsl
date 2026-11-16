story(battle_main)
{
  // r2c7 | 巴别塔 | 苏美尔层 | D 文明与权能
  local
  {
    @wave(0);
    @spawned(0);
    @total(3);
  };
  onmessage("start")
  {
    create_npc(1000,vector_3(60,16.7,70),0,3,1,"ai_player",string_list("Ai/ailogic_player.dsl"));
    set_leader_id(unit_id_2_obj_id(1000));
    npc_set_formation(1000,0);
    loop(4){
      create_npc(1001+$$,vector_3(60+rnd_int(0,10),16.7,70+rnd_int(0,10)),0,3,$$+2,"ai_member",string_list("Ai/ailogic_member.dsl"),get_leader_id());
      npc_set_formation(1001+$$,$$+1);
    };
    camera_follow();
    publish_gfx_event("loading_complete", "ui");
    fire_message("set_map_image", "Minimap/r2c7_minimap_sumer");
    send_room_story_message("enter_parallel", "r2c7");
    start_story("auto_battle");
    fire_message("push_tip_info", "【巴别塔】泥板写着「王权自天而降」，城邦的砖缝里却全是灰。");
    fire_message("push_tip_info", "时代层：苏美尔层。这条线索连着主线，别急着离开。");
    fire_message("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    fire_message("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      fire_message("push_tip_info", "「王权自天而降」不再是一句颂词。");
      clear_message("show_dlg");
      fire_message("show_dlg",998);
    } else {
      if(@wave<2){
        fire_message("push_tip_info", "巴别塔：城邦外廓·D 文明与权能");
        fire_message("wave_spawn_1");
      } else {
      if(@wave<3){
        fire_message("push_tip_info", "巴别塔：泥板库前庭·D 文明与权能");
        fire_message("wave_spawn_2");
      } else {
      if(@wave<4){
        fire_message("push_tip_info", "巴别塔：塔庙阶道·D 文明与权能");
        fire_message("wave_spawn_3");
      } else {
      };
      };
      };
    };
  };
  onmessage("wave_spawn_1")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      create_npc(1026,vector_3(get_position_x(get_leader_id())+14.0,get_position_y(get_leader_id()),get_position_z(get_leader_id())+0.0),0,4,6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1027,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+13.3),0,4,7,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1028,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+8.2),0,4,8,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1029,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-8.2),0,4,9,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1030,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-13.3),0,4,10,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
    };
  };
  onmessage("wave_spawn_2")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      create_npc(1046,vector_3(get_position_x(get_leader_id())+14.0,get_position_y(get_leader_id()),get_position_z(get_leader_id())+0.0),0,4,6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1047,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+13.3),0,4,7,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1048,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+8.2),0,4,8,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1049,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-8.2),0,4,9,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1050,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-13.3),0,4,10,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
    };
  };
  onmessage("wave_spawn_3")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      create_npc(1066,vector_3(get_position_x(get_leader_id())+14.0,get_position_y(get_leader_id()),get_position_z(get_leader_id())+0.0),0,4,6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1067,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+13.3),0,4,7,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1068,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())+8.2),0,4,8,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1069,vector_3(get_position_x(get_leader_id())-11.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-8.2),0,4,9,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
      create_npc(1070,vector_3(get_position_x(get_leader_id())+4.3,get_position_y(get_leader_id()),get_position_z(get_leader_id())-13.3),0,4,10,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
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
  onmessage("dialog_over",998)
  {
    terminate();
    close_battle(10024);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    close_battle(10024);
  };
};
