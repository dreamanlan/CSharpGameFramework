story(story_main)
{
  local
  {
  	@monsterRefresh(0);
  };
  onmessage("start")
  {
  };
  onmessage("user_enter_scene")
  {
    $userId=$0;
    $userUnitId=$1;
    $campId=$2;
    wait(1000);
    publishgfxevent("loading_complete", "ui")touser($userId);
    camerafollow($userUnitId)touser($userId);
		//sendserverstorymessage("msg_from_room")touser($userId);
    sendgfxmessage("Main Camera", "LightScreen", 3000)touser($userId);
  };
  onmessage("user_leave_scene")
  {
  };
  onmessage("server:msg_from_userserver")
  {
		//log("msg_from_userserver:{0} {1}",$0,getentityinfo($0).CustomData.Guid);
		//sendserverstorymessage("msg_from_room")touser($0);
  };
  onmessage("client:touchnpc")
  {
    $userId = $0;
    showdlg(2)touser($userId);
  };
  onmessage("dialog_over",2)
  {
    $userId = $0;
    sendclientstorymessage("open_battle", 10001)touser($userId);
  };
  onmessage("refreshmonsters")
  {
    log("scene2");    
    if(@monsterRefresh==0){
      @monsterRefresh=1;
      loop(32){      	
	      $monsterInfo = getmonsterinfo(1,$$);
	      if(!isnull($monsterInfo)){
	      	createnpc(1006+$$,vector3($monsterInfo.x,0,$monsterInfo.y),$monsterInfo.dir*3.1415926/180,2,$monsterInfo.actorID,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));
	      };
	    };      
    };
  };
};