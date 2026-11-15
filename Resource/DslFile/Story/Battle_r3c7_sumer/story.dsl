story(battle_main)
{
  // r3c7 | 两河渡口 | 苏美尔层 | D 文明与权能
  local
  {
    @wave(0);
    @spawned(0);
    @total(2);
  };
  onmessage("start")
  {
    createnpc(1000,vector3(60,10.2,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    loop(4){
      createnpc(1001+$$,vector3(60+rndint(0,10),10.2,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow();
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "Minimap/r3c7_minimap_sumer");
    startstory("auto_battle");
    firemessage("push_tip_info", "【两河渡口】泥板写着「王权自天而降」，城邦的砖缝里却全是灰。");
    firemessage("push_tip_info", "时代层：苏美尔层。支线记载，或许能补上别处缺的那一页。");
    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");
    firemessage("wave_go");
  };
  onmessage("wave_go")
  {
    inc(@wave);
    if(@wave>@total){
      firemessage("push_tip_info", "「王权自天而降」不再是一句颂词。");
      clearmessage("show_dlg");
      firemessage("show_dlg",998);
    } else {
      if(@wave<2){
        firemessage("push_tip_info", "两河渡口：城邦外廓·D 文明与权能");
        objmove(getleaderid(), vector3(232,8.8,167), "wave_spawn_1");
        wait(3000);
        firemessage("wave_spawn_1");
      } else {
      if(@wave<3){
        firemessage("push_tip_info", "两河渡口：泥板库前庭·D 文明与权能");
        objmove(getleaderid(), vector3(254,8.8,298), "wave_spawn_2");
        wait(3000);
        firemessage("wave_spawn_2");
      } else {
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
    closebattle(10032);
  };
  onmessage("dialog_over",999)
  {
    terminate();
    closebattle(10032);
  };
};
