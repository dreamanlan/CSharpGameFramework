package(GameFrameworkMessage);

message(Msg_RL_RegisterRoomServer)
{
	member(ServerName, string, required);
	member(MaxRoomNum, int32, required);
	member(ServerIp, string, required);
	member(ServerPort, uint32, required);
};

message(Msg_LR_ReplyRegisterRoomServer)
{
	member(IsOk, bool, required);
};

message(Msg_LR_RoomUserInfo)
{
  message(MemberInfo)
  {
    member(Hero, int32, required);
    member(Level, int32, required);
  };
	member(Guid, uint64, required);
	member(Nick, string, required);
	member(Key, uint32, required);
	member(Hero, int32, required);
	member(Camp, int32, required);
	member(IsMachine, bool, required);
  member(Level, int32, required);
  member(EnterX, float, optional);
  member(EnterY, float, optional);
  member(Members, MemberInfo, repeated);
  member(SummonerSkillId, int32, required);
};

message(Msg_LR_ReconnectUser)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
};

message(Msg_RL_ReplyReconnectUser)
{
	enum(ReconnectResultEnum) {
    Drop;
    NotExist;
    Online;
  };
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);	
	member(Result, int32, required);
};

message(Msg_RL_RoomServerUpdateInfo)
{
	member(ServerName, string, required);
	member(IdleRoomNum, int32, required);
	member(UserNum, int32, required);
};

message(Msg_RL_UserLobbyItemInfo)
{
	member(ItemId, int32, required);
	member(ItemNum, int32, required);
};

message(Msg_RL_PickMoney)
{
  member(UserGuid, uint64, required);
  member(Num, int32, required);
};

message(Msg_RL_PickItem)
{
  member(UserGuid, uint64, required);
	member(RoomID, int32, required);
  member(ItemId, int32, required);
  member(Model, string, required);
  member(Particle, string, required);
  member(RoomSvrName, string, optional);
};

message(Msg_LR_ReclaimItem)
{
  member(UserGuid, uint64, required);
	member(RoomID, int32, required);
  member(ItemId, int32, required);
  member(Model, string, required);
  member(Particle, string, required);
  member(TipDict, int32, required);
};

message(Msg_RL_UserDrop)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(IsBattleEnd, bool, required);
};

message(Msg_LR_UserQuit)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
};

message(Msg_RL_UserQuit)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
};

message(Msg_LR_UserReLive)
{
  member(UserGuid, uint64, required);
  member(RoomID, int32, required);
};

message(Msg_LR_ActiveScene)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(SceneId, int32, required);
	member(UserInfos, Msg_LR_RoomUserInfo, repeated);
	member(HPs, int32, optional);
	member(MPs, int32, optional);
};

message(Msg_RL_ActiveSceneResult)
{
	member(UserGuids, uint64, repeated);
	member(RoomID, int32, required);
	member(Result, int32, required);
};

message(Msg_LR_EnterScene)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(UserInfo, Msg_LR_RoomUserInfo, required);
	member(HP, int32, optional);
	member(MP, int32, optional);
};

message(Msg_RL_EnterSceneResult)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(Result, int32, required);
};

message(Msg_LR_ChangeScene)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(TargetRoomID, int32, required);
};

message(Msg_RL_ChangeSceneResult)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(TargetRoomID, int32, required);
	member(Result, int32, required);
	member(HP, int32, optional);
	member(MP, int32, optional);
};

message(Msg_RL_ChangeScene)
{
	member(UserGuid, uint64, required);
	member(SceneID, int32, required);
};

message(Msg_LRL_StoryMessage)
{
  enum(ArgType) {
    NULL;
    INT;
    FLOAT;
    STRING;
  };
  message(MessageArg) {
    member(val_type, ArgType, required);
    member(str_val, string, required);
  };
  member(RoomId, int, optional);
	member(UserGuid, uint64, required);
  member(MsgId, string, required);
  member(Args, MessageArg, repeated);
};