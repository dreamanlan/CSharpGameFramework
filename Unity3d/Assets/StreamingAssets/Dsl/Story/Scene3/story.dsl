story(story_main)
{
  local
  {
    @userId(0);
    @userUnitId(0);
    @campId(0);
    @unitId(0);
  	@pt(0);
  	@monsterRefresh(0);
  	@monsterInfo(0);
  	@index(0);
  };
  onmessage("start")
  {
  };
  onmessage("user_enter_scene")
  {
    @userId=$0;
    @userUnitId=$1;
    @campId=$2;
    objsetformation(@userId,0);
    loop(4){
      @unitId = @userId*100+$$;
      createnpc(@unitId,vector3(25+rndint(0,10),0,25+rndint(0,10)),0,@campId,$$+2,4,stringlist(""),@userId);
      npcsetformation(@unitId,$$+1);
    };
    wait(1000);
    objaddskill(@userId,22);
    objaddskill(@userId,23);
    publishgfxevent("ui_add_skill_button", "ui", 1, @userId, 22)touser(@userId);
    publishgfxevent("ui_add_skill_button", "ui", 1, @userId, 23)touser(@userId);
    publishgfxevent("ui_add_skill_button", "ui", 1, @userId, 0)touser(@userId);
    camerafollow(@userUnitId)touser(@userId);
    loop(4){
      @unitId = @userId*100+$$;
      publishgfxevent("ui_add_skill_button","ui",$$+2,unitid2objid(@unitId),0)touser(@userId);
    };
		sendserverstorymessage("msg_from_room")touser(@userId);
    showdlg(2)touser(@userId);
  };
  onmessage("user_leave_scene")
  {
  };
  onmessage("server:msg_from_userserver")
  {
		log("msg_from_userserver:{0} {1}",$0,getentityinfo($0).CustomData.Guid);
		sendserverstorymessage("msg_from_room")touser($0);
  };
  onmessage("dialog_over",2)
  {
  };
  onmessage("refreshmonsters")
  {
    log("scene2");    
    if(@monsterRefresh==0){
      @monsterRefresh=1;
      /*
      loop(26){
      	@pt = vector3(75+rndint(0,10),0,75+rndint(0,10));//getcomponent("pt"+($$/7),"PositionMarker").Position;
        createnpc(1006+$$,rndvector3(@pt,10),0,2,$$+6,2,stringlist(""));
      };
     	*/
      loop(64){      	
	      @monsterInfo = getmonsterinfo(2,$$);
	      if(!isnull(@monsterInfo)){
	      	createnpc(1006+$$,vector3(@monsterInfo.x,0,@monsterInfo.y),@monsterInfo.dir*3.1415926/180,2,@monsterInfo.actorID,2,stringlist(""));
	      };
	    };
      
    };
    highlightprompt(0,"Tip_1");
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