story(battle_main)
{
  // r0c6 | 月球背面 | 洪水前层 | A 降临与设施 · G 回归
  local
  {
    @wave(0);
    @spawned(0);
    @total(4);
  };
  onmessage("start")
  {
    createnpc(1000,vector3(60,29.6,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    loop(4){
      createnpc(1001+$$,vector3(60+rndint(0,10),29.6,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow();
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "Minimap/r0c6_minimap_prediluvian");
    startstory("auto_battle");
    firemessage("push_tip_info", "【月球背面】信标仍在发送坐标，可接收者早已离开这颗星球。");
    firemessage("push_tip_info", "时代层：洪水前层。这条线索连着主线，别急着离开。");
    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    firemessage("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      firemessage("push_tip_info", "坐标串起来了，但发送者仍未露面。");
      clearmessage("show_dlg");
      firemessage("show_dlg",998);
    } else {
      if(@wave<2){
        firemessage("push_tip_info", "月球背面：信标残座·A 降临与设施 · G 回归");
        objmove(getleaderid(), vector3(228,8.8,386), "wave_spawn_1");
        wait(3000);
        firemessage("wave_spawn_1");
      } else {
      if(@wave<3){
        firemessage("push_tip_info", "月球背面：废弃中转舱·A 降临与设施 · G 回归");
        objmove(getleaderid(), vector3(302,8.8,270), "wave_spawn_2");
        wait(3000);
        firemessage("wave_spawn_2");
      } else {
      if(@wave<4){
        firemessage("push_tip_info", "月球背面：环形陨坑·A 降临与设施 · G 回归");
        objmove(getleaderid(), vector3(388,8.8,167), "wave_spawn_3");
        wait(3000);
        firemessage("wave_spawn_3");
      } else {
      if(@wave<5){
        firemessage("push_tip_info", "月球背面：归来之门·A 降临与设施 · G 回归");
        objmove(getleaderid(), vector3(348,8.8,180), "wave_spawn_4");
        wait(3000);
        firemessage("wave_spawn_4");
      } else {
      };
      };
      };
      };
    };
  };
  onmessage("wave_spawn_1")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(6){
        createnpc(1026+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("wave_spawn_2")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(6){
        createnpc(1046+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("wave_spawn_3")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(6){
        createnpc(1066+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("wave_spawn_4")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(6){
        createnpc(1086+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("all_killed")
  {
    firemessage("wave_go");
  };
  onmessage("npc_killed:1000")
  {
    clearmessage("show_dlg");
    firemessage("show_dlg",999);
  };
  onmessage("all_died")
  {
    clearmessage("show_dlg");
    firemessage("show_dlg",999);
  };
  onmessage("dialog_over",998)
  {
    terminate();
    closebattle(10007);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    closebattle(10007);
  };
};
