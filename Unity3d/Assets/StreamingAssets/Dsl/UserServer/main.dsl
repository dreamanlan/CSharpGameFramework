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
		log("msg_from_room:{0} {1} {2}",$0,getuserinfo($0).NodeName,getuserinfo($0).Key);
		sendserverstorymessage("msg_from_userserver")touser($0);
	};
};