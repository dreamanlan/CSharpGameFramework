package(GameFrameworkMessage);

import("MessageDefine.dsl");

message(Msg_LBL_Message)
{
	enum(MsgTypeEnum) {
    Node;
    Room;
  };
  member(MsgType, MsgTypeEnum, required);
  member(TargetName, string, required);
  member(Data, bytes, required);
};

message(Msg_LB_UpdateLobbyInfo)
{
  member(WorldId, int32, required);
  member(UserCount, int32, required);
  member(RoomCount, int32, required);
  member(RoomUserCount, int32, required);
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
  member(LogicServerId, int32, required);
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
  member(LogicServerId, int32, required);
  member(AccountId, string, required);
  member(FightingScore, int32, required);
  member(ClientGameVersion, string, required);
  member(StartServerTime, string, required);
};

message(Msg_LBL_GowInfo)
{
  member(GowElo, int32, required);
  member(GowMatchElo, int32, required);
  member(GowMatches, int32, required);
  member(GowWinMatches, int32, required);
  member(LeftMatchCount, int32, required);
  member(RankId, int32, required);
  member(Point, int32, required);
  member(CriticalTotalMatches, int32, required);
  member(CriticalAmassWinMatches, int32, required);
  member(CriticalAmassLossMatches, int32, required);
  member(ContinueLossMatches, int32, required);
  member(ContinueWinMatches, int32, required);
  member(IsAcquirePrize, bool, required);
  member(LastTourneyDate, string, optional);
};

message(Msg_LB_RequestGowMatch)
{  
  member(BaseInfo, Msg_LB_BigworldUserBaseInfo, required);
  member(User, Msg_LR_RoomUserInfo, required);
  member(GowInfo, Msg_LBL_GowInfo, required);
  member(Type, int32, required);
  member(SceneDifficulty, int32, required);
};

message(Msg_BL_GowBattleEnd)
{
  member(BattleInfo, Msg_RL_UserBattleInfo, required);
  member(GowBattleResult, bytes, required);
  member(GowInfo, Msg_LBL_GowInfo, required);
};

message(Msg_LB_RequestEnterField)
{
  member(BaseInfo, Msg_LB_BigworldUserBaseInfo, required);
  member(User, Msg_LR_RoomUserInfo, required);
  member(SceneId, int32, required);
  member(WantRoomId, int32, required);
  member(FromSceneId, int32, required);
};