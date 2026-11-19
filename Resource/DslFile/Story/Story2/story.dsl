story(story_main)
{
  local
  {
  	@monsterRefresh(0);
  };
  onmessage("start")
  {
  };
  onmessage("user_enter_scene")params($userId,$userUnitId,$campId)
  {
    wait(1000);
    publish_gfx_event("loading_complete", "ui")touser($userId);
    camera_follow($userUnitId)touser($userId);
		//send_server_story_message("msg_from_room")touser($userId);
    send_gfx_message("Main Camera", "LightScreen", 3000)touser($userId);
  };
  onmessage("user_leave_scene")
  {
  };
  onmessage("server:msg_from_userserver")
  {
		//log("msg_from_userserver:{0} {1}",$0,get_entity_info($0).CustomData.Guid);
		//send_server_story_message("msg_from_room")touser($0);
  };
  onmessage("client:touchnpc")
  {
    $userId = $0;
    show_dlg(2)touser($userId);
  };
  onmessage("dialog_over",2)
  {
    $userId = $0;
    $bi = prop_get("battleIndex", 0);
    if($bi<10001){
      $bi=10001;
    };
    if($bi>10640){
      $bi=10001;
    };
    send_client_story_message("open_battle", $bi)touser($userId);
    inc($bi);
    if($bi>10640){
      $bi=10001;
    };
    prop_set("battleIndex", $bi);

  };
  onmessage("refreshmonsters")
  {
    log("scene2");
    if(@monsterRefresh==0){
      @monsterRefresh=1;
      loop(32){
	      $monsterInfo = get_monster_info(1,$$);
	      if(!is_null($monsterInfo)){
	      	create_npc(1006+$$,vector3($monsterInfo.x,0,$monsterInfo.y),$monsterInfo.dir*3.1415926/180,2,$monsterInfo.actorID,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
	      };
	    };
    };
  };
};