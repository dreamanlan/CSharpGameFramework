story(battle_main)
{
  // r4c4 | 阿布祖金矿 | 洪水前层 | A 降临 · B 造人
  local
  {
    @wave(0);
    @spawned(0);
    @total(4);
  };
  onmessage("start")
  {
    createnpc(1000,vector3(60,31.6,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    loop(4){
      createnpc(1001+$$,vector3(60+rndint(0,10),31.6,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow();
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "Minimap/r4c4_minimap_prediluvian");
    startstory("auto_battle");
    firemessage("push_tip_info", "【阿布祖金矿】竖井太深了，深到不像为了开采，更像为了离开。");
    firemessage("push_tip_info", "时代层：洪水前层。这条线索连着主线，别急着离开。");
    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    firemessage("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      firemessage("push_tip_info", "原来最初要找的不是奴隶，是金。");
      clearmessage("show_dlg");
      firemessage("show_dlg",998);
    } else {
      if(@wave<2){
        firemessage("push_tip_info", "阿布祖金矿：采掘竖井·A 降临 · B 造人");
        objmove(getleaderid(), vector3(228,8.8,386), "wave_spawn_1");
        wait(3000);
        firemessage("wave_spawn_1");
      } else {
      if(@wave<3){
        firemessage("push_tip_info", "阿布祖金矿：矿工营墟·A 降临 · B 造人");
        objmove(getleaderid(), vector3(302,8.8,270), "wave_spawn_2");
        wait(3000);
        firemessage("wave_spawn_2");
      } else {
      if(@wave<4){
        firemessage("push_tip_info", "阿布祖金矿：基因工房·A 降临 · B 造人");
        objmove(getleaderid(), vector3(388,8.8,167), "wave_spawn_3");
        wait(3000);
        firemessage("wave_spawn_3");
      } else {
      if(@wave<5){
        firemessage("push_tip_info", "阿布祖金矿：伊甸残圃·A 降临 · B 造人");
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
  onmessage("wave_spawn_4")
  {
    if(@spawned<@wave){
      @spawned=@wave;
      loop(5){
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
    closebattle(10037);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    closebattle(10037);
  };
};
