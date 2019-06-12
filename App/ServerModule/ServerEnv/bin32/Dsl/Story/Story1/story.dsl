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
    log("touchnpc");
    $userId = $0;
    sendclientstorymessage("open_battle", 10001)touser($userId);
  };
};