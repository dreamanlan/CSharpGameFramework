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
  onmessage("client:touchnpc")params($userId)
  {
    log("touchnpc");
    $bi = propget("battleIndex", 0);
    if($bi<10001){
      $bi=10001;
    };
    if($bi>10064){
      $bi=10001;
    };
    sendclientstorymessage("open_battle", $bi)touser($userId);
    inc($bi);
    if($bi>10064){
      $bi=10001;
    };
    propset("battleIndex", $bi);
  };
};