story(battle_main)
{
  // r5c2 | Zep Tepi 标记 | 洪水前层 | E 设施 · F 神族战争
  local
  {
    @wave(0);
    @spawned(0);
    @total(3);
  };
  onmessage("start")
  {
    createnpc(1000,vector3(60,18.9,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    loop(4){
      createnpc(1001+$$,vector3(60+rndint(0,10),18.9,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow();
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "Minimap/r5c2_minimap_prediluvian");
    startstory("auto_battle");
    firemessage("push_tip_info", "【Zep Tepi 标记】金字塔不是陵墓，它的甬道对准某颗尚未归来的星。");
    firemessage("push_tip_info", "时代层：洪水前层。支线记载，或许能补上别处缺的那一页。");
    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    firemessage("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      firemessage("push_tip_info", "星象对齐的结果指向回归的时刻表。");
      clearmessage("show_dlg");
      firemessage("show_dlg",998);
    } else {
      if(@wave<2){
        firemessage("push_tip_info", "Zep Tepi 标记：甬道入口·E 设施 · F 神族战争");
        objmove(getleaderid(), vector3(262,8.8,378), "wave_spawn_1");
        wait(3000);
        firemessage("wave_spawn_1");
      } else {
      if(@wave<3){
        firemessage("push_tip_info", "Zep Tepi 标记：狮身台地·E 设施 · F 神族战争");
        objmove(getleaderid(), vector3(156,8.8,258), "wave_spawn_2");
        wait(3000);
        firemessage("wave_spawn_2");
      } else {
      if(@wave<4){
        firemessage("push_tip_info", "Zep Tepi 标记：顶端晶石室·E 设施 · F 神族战争");
        objmove(getleaderid(), vector3(178,8.8,300), "wave_spawn_3");
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
    closebattle(10043);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    closebattle(10043);
  };
};
