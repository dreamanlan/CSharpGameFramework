story(battle_main)
{
  // r0c1 | 方舟残骸 | 洪水层 | C 大洪水
  local
  {
    @wave(0);
    @spawned(0);
    @total(3);
  };
  onmessage("start")
  {
    createnpc(1000,vector3(60,39.5,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    loop(4){
      createnpc(1001+$$,vector3(60+rndint(0,10),39.5,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow();
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "Minimap/r0c1_minimap_flood");
    startstory("auto_battle");
    firemessage("push_tip_info", "【方舟残骸】洪水退去后，冰盖裂隙里还留着方舟的木痕。");
    firemessage("push_tip_info", "时代层：洪水层。这条线索连着主线，别急着离开。");
    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    firemessage("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      firemessage("push_tip_info", "碎片拼出一段被抹去的洪水记录。");
      clearmessage("show_dlg");
      firemessage("show_dlg",998);
    } else {
      if(@wave<2){
        firemessage("push_tip_info", "方舟残骸：冰蚀裂口·C 大洪水");
        objmove(getleaderid(), vector3(320,8.8,174), "wave_spawn_1");
        wait(3000);
        firemessage("wave_spawn_1");
      } else {
      if(@wave<3){
        firemessage("push_tip_info", "方舟残骸：方舟残骸带·C 大洪水");
        objmove(getleaderid(), vector3(142,8.8,344), "wave_spawn_2");
        wait(3000);
        firemessage("wave_spawn_2");
      } else {
      if(@wave<4){
        firemessage("push_tip_info", "方舟残骸：洪水冲刷阶地·C 大洪水");
        objmove(getleaderid(), vector3(370,8.8,380), "wave_spawn_3");
        wait(3000);
        firemessage("wave_spawn_3");
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
      loop(5){
        createnpc(1026+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("wave_spawn_2")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(5){
        createnpc(1046+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
      };
    };
  };
  onmessage("wave_spawn_3")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(5){
        createnpc(1066+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
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
    closebattle(10002);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    closebattle(10002);
  };
};
