story(user_main)
{
	local
	{
	};
	onmessage("start")
	{
	};
	onmessage("server:msg_from_room")
	{
		log("msg_from_room:{0} {1} {2}",$0,get_user_info($0).NodeName,get_user_info($0).Key);
		send_server_story_message("msg_from_userserver")touser($0);
	};
};