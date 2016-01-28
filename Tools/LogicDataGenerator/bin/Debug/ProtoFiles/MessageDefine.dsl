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
	member(Guid, uint64, required);
	member(Nick, string, required);
	member(Key, uint32, required);
	member(Hero, int32, required);
	member(ArgScore, int32, required);
	member(Camp, int32, required);
	member(IsMachine, bool, required);
	member(ShopEquipmentsId, int32, repeated);
	message(SkillInfo) {
    member(skill_id, int32, required);
    member(skill_level, int32, required);
  };
  member(Skills, SkillInfo, repeated);
	member(PresetIndex, int32, optional);
	message(EquipInfo) {
    member(equip_id, int32, required);
    member(equip_level, int32, required);
    member(equip_random_property, int32, required);
	  member(equip_upgrade_star, int32, required);
	  member(equip_strength_level, int32, required);
  };
  member(Equips, EquipInfo, repeated);
  member(Level, int32, required);
  member(EnterX, float, optional);
  member(EnterY, float, optional);
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

message(Msg_LR_CreateBattleRoom)
{
	member(RoomId, int32, required);
	member(SceneType, int32, required);
	member(SceneDifficulty, int32, required);
	member(Users, Msg_LR_RoomUserInfo, repeated);
	member(Monsters, int32, repeated);
	member(Hps, int32, repeated);
};

message(Msg_RL_ReplyCreateBattleRoom)
{
	member(RoomId, int32, required);
	member(IsSuccess, bool, required);
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

message(Msg_RL_UserBattleInfo)
{
	member(UserGuid, uint64, required);
	enum(BattleResultEnum) {
    Win;
    Lost;
    Unfinish;
  };
	member(BattleResult, BattleResultEnum, required);
	member(Money, int32, optional);
	member(HitCount, int32, optional);
	member(KillNpcCount, int32, optional);
	member(MaxMultiHitCount, int32, optional);
	member(TotalDamageToMyself, int32, optional);
	member(TotalDamageFromMyself, int32, optional);
	member(AwardId, int32, optional);
	member(NickName, string, optional);
	member(HeroId, int32, optional);
  member(ReliveTime, int32, optional);
};

message(Msg_RL_BattleEnd)
{
	member(RoomID, int32, required);
	enum(WinnerCampEnum) {
    None;
    Red;
    Blue;
  };
  member(WinnerCamp, WinnerCampEnum, required);
	member(UserBattleInfos, Msg_RL_UserBattleInfo, repeated);
	member(Monsters, int32, repeated);
	member(Hps, int32, repeated);
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

message(Msg_LR_EnterField)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(UserInfo, Msg_LR_RoomUserInfo, required);
	member(HP, int32, optional);
	member(MP, int32, optional);	
};

message(Msg_RL_EnterFieldResult)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(Result, int32, required);
};

message(Msg_LR_ChangeField)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(TargetRoomID, int32, required);
};

message(Msg_RL_ChangeFieldResult)
{
	member(UserGuid, uint64, required);
	member(RoomID, int32, required);
	member(TargetRoomID, int32, required);
	member(Result, int32, required);
	member(HP, int32, optional);
	member(MP, int32, optional);
};

message(Msg_RL_ChangeField)
{
	member(UserGuid, uint64, required);
	member(SceneID, int32, required);
};