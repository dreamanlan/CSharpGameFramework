story(battle_main)
{
  local
  {
  	@pt(0);
  };
  onmessage("start")
  {    
    showdlg(2);
    wait(200);
    createnpc(1000,vector3(65,1000,70),0,3,1);
    setleaderid(unitid2objid(1000));
    npcsetformation(1000,0);
    npcaddskill(1000,22);
    npcaddskill(1000,23);
    loop(4){
      createnpc(1001+$$,vector3(55+rndint(0,10),1000,65+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());
      npcsetformation(1001+$$,$$+1);
    };
    camerafollow(1000);
    publishgfxevent("loading_complete", "ui");
    
    startstory("auto_battle");
  };
  onmessage("dialog_over",2)
  {
    sendgfxmessage("Main Camera", "LightScreen", 3000);
    log("scene2");
    loop(26){
    	@pt = getcomponent("pt"+($$%7),"PositionMarker").Position;
      createnpc(1006+$$,rndvector3(@pt,10),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
    };
    highlightprompt(0,"Tip_1");
    wait(1000);
    publishgfxevent("ui_show_paopao","ui",getleaderid(),"鼠标点击地面移动。");
    firemessage("push_tip_info", "史前，或许还没有人类或者不能称之为人类的时候，最早降临地球的那些智慧生物，被称为神");
    wait(5000);
    firemessage("push_tip_info", "距今天到底有多久已经无法考证了。。。");
    wait(5000);
    firemessage("push_tip_info", "不过，各种历史资料总会不经意的反复提及这些久远的故事。");
    wait(5000);
    firemessage("push_tip_info", "苏美尔人、玛雅文明、古埃及。。。");
    wait(5000);
    firemessage("push_tip_info", "还有神秘的水晶头骨的传说。。。");
    wait(5000);
    firemessage("push_tip_info", "不管传说怎样，尼比鲁星球再一次靠近地球，好像已经很近了");
    wait(5000);
    firemessage("push_tip_info", "月球背面真的有基地么，还是真的需要千年之久所谓的神才重新降临地球呢");
    wait(5000);
    firemessage("push_tip_info", "真相究竟是什么？");
    wait(5000);
    firemessage("push_tip_info", "我们回到当年的大陆去看看吧");
    wait(5000);
    firemessage("push_tip_info", "序章--人？神？");
    wait(5000);
    publishgfxevent("ui_show_paopao","ui",getleaderid(),"史前，或许还没有人类或者不能称之为人类的时候，最早降临地球的那些智慧生物，被称为神");
  };
  onmessage("move_to")
  {
  	npcmove(1000,vector3($0,$1,$2));
  };
  onmessage("all_killed")
  {
  	firemessage("show_dlg",998);
  	clearmessage("show_dlg");
  };
  onmessage("dialog_over",998)
  {
    terminate();
    changescene(1);
  };
};