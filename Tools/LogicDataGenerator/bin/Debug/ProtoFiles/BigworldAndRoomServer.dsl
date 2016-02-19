package(GameFrameworkMessage);

import("MessageDefine.dsl");

message(Msg_LBL_Message)
{
	enum(MsgTypeEnum) {
    Node;
    Room;
  };
  member(MsgType, MsgTypeEnum, required);
  member(TargetName, string, optional);
  member(Data, bytes, required);
};

message(Msg_LB_UpdateUserServerInfo)
{
  member(WorldId, int32, required);
  member(UserCount, int32, required);
};

message(Msg_LB_QueryUserState)
{
  member(Guid, uint64, required);
};

message(Msg_BL_QueryUserStateResult)
{
  member(Guid, uint64, required);
  member(State, int32, required);
};

message(Msg_BL_BroadcastText)
{
  member(BroadcastType, int32, required);
  member(Content, string, required);
  member(RollCount, int32, required);
};

message(Msg_LB_UserOffline)
{
  member(Guid, uint64, required);
};

message(Msg_BL_UserOffline)
{
  member(Guid, uint64, required);
};

message(Msg_LB_UserRelogin)
{
  member(Guid, uint64, required);
};

message(Msg_LB_CancelMatch)
{
  member(Guid, uint64, required);
  member(Type, int32, required);
};

message(Msg_LB_BigworldUserBaseInfo)
{
  member(NodeName, string, required);
  member(WorldId, int32, required);
  member(AccountId, string, required);
  member(ClientInfo, string, required);
  member(StartServerTime, string, required);
  member(FightingCapacity, int32, required);
};

message(Msg_LB_RequestEnterScene)
{
  member(BaseInfo, Msg_LB_BigworldUserBaseInfo, required);
  member(User, Msg_LR_RoomUserInfo, required);
  member(SceneId, int32, required);
  member(WantRoomId, int32, required);
  member(FromSceneId, int32, required);
};