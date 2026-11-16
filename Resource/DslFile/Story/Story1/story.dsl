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
  onmessage("client:touchnpc")params($userId)
  {
    log("touchnpc");
    $bi = prop_get("battleIndex", 0);
    if($bi<10001){
      $bi=10001;
    };
    if($bi>10064){
      $bi=10001;
    };
    send_client_story_message("open_battle", $bi)touser($userId);
    inc($bi);
    if($bi>10064){
      $bi=10001;
    };
    prop_set("battleIndex", $bi);
  };
};