package(GameFrameworkMessage);

// ping消息，时间单位为毫秒
message(Msg_Ping) {
  member(send_ping_time, long, required);
};

// pong消息
message(Msg_Pong) {
  member(send_ping_time, long, required);
  member(send_pong_time, long, required);
};

// 位置信息
message(Position) {
  member(x, float, required);
  member(z, float, required);
};
// 位置信息
message(EncodePosition) {
  member(x, int, required);
  member(z, int, required);
};
// 位置信息
message(EncodePosition3D) {
  member(x, int, required);
  member(y, int, required);
  member(z, int, required);
};

// 玩家连接验证
message(Msg_CR_ShakeHands) {
  member(auth_key, uint, required);
};

enum(ArgType) {
  NULL;
  INT;
  FLOAT;
  STRING;
};

// 玩家连接验证结果
message(Msg_RC_ShakeHands_Ret) {
  enum(RetType) {
    SUCCESS;
    ERROR;
  };
  member(auth_result, RetType, required); // 0为成功，其它为错误码
};

message(Msg_CR_Observer) {  
};

// 玩家创建
message(Msg_CR_Enter) {
};

message(Msg_CR_Quit) {
	member(is_force, bool, required);
};

message(Msg_CR_UserMoveToPos) {
  member(target_pos, uint64, required);
  member(is_stop, bool, optional);
};

// 玩家技能
message(Msg_CR_Skill) {
  member(role_id, int, required);
  member(skill_id, int, required);
  member(target_id, int, required);
};

message(Msg_CR_OperateMode) {
  member(isauto, bool, required);
};

message(Msg_CR_GiveUpBattle) {
};

message(Msg_CR_SwitchDebug) {
  member(is_debug, bool, required);
};

message(Msg_CR_GmCommand)
{
  member(type, int, required);
  member(content, string, optional);
};

// 创建Npc
message(Msg_RC_CreateNpc) {
  member(npc_id, int, required);
  member(unit_id, int, required);
  member(cur_pos, Position, required);
  member(face_direction, float, required);
  member(link_id, int, required);
  member(camp_id, int, optional);
  member(owner_id, int, optional);
  member(leader_id, int, optional);
  member(key, uint32, optional);
  member(level, int, optional);  
};

message(Msg_RC_NpcDead) {
  member(npc_id, int, required);
};

message(Msg_RC_DestroyNpc) {
  member(npc_id, int, required);
};

message(Msg_RC_NpcMove) {
  member(npc_id, int, required);
  member(velocity, int, optional);
  member(cur_pos, uint64, optional);
  member(target_pos, uint64, optional);
};

message(Msg_RC_NpcFace) {
  member(npc_id, int, optional);
  member(face_direction, int, required);
};

message(Msg_RC_NpcSkill) {
  member(npc_id, int, required);
  member(skill_id, int, required);
  member(stand_pos, ulong, required);
  member(face_direction, int, required);
};

message(Msg_RC_NpcStopSkill) {
  member(npc_id, int, required);
};

message(Msg_RC_AddImpact) {
  member(sender_id, int, required);
  member(target_id, int, required);
  member(impact_id, int, required);
  member(skill_id, int, required);
  member(duration, int, required);
};

message(Msg_RC_RemoveImpact) {
  member(obj_id, int, required);
  member(impact_id, int, required);
};

message(Msg_RC_AddSkill) {
  member(obj_id, int, required);
  member(skill_id, int, required);
};

message(Msg_RC_RemoveSkill) {
  member(obj_id, int, required);
  member(skill_id, int, required);
};

message(Msg_RC_AdjustPosition) {
  member(role_id, int, required); 
  member(x, int, required);
  member(z, int, required);
  member(face_dir, int, required);
};

message(Msg_RC_SyncProperty) {
  member(role_id, int, required);	
	member(hp, int, required);
	member(np, int, required);
	member(shield, int, required);
	member(state, int, required);
};

message(Msg_RC_ImpactDamage) {
  enum(Flag) {
    IS_KILLER(1);
    IS_CRITICAL(2);
    IS_ORDINARY(4);
  };
  member(role_id, int, required);
  member(attacker_id, int, required);
  member(damage_status, int, required);
  member(hp, int, required);
  member(energy, int, optional);
};

message(Msg_RC_ChangeScene) {
  member(target_scene_id, int, required);
};

message(Msg_RC_SyncNpcOwnerId)
{
	member(npc_id, int, required);
  member(owner_id, int, required);
};

message(Msg_RC_CampChanged) {
  member(obj_id, int, required);
  member(camp_id, int, required);
};

message(Msg_RC_DebugSpaceInfo) {
  message(DebugSpaceInfo) {
    member(obj_id, int, required);
    member(is_player, bool, required);
    member(pos_x, float, required);
    member(pos_z, float, required);	
    member(face_dir, float, required);
  };
  member(space_infos, DebugSpaceInfo, repeated);
};

message(Msg_CRC_StoryMessage)
{
  message(MessageArg) {
    member(val_type, ArgType, required);
    member(str_val, string, required);
  };
  member(m_MsgId, string, required);
  member(m_Args, MessageArg, repeated);
};

message(Msg_RC_PublishEvent) {
  message(EventArg) {
    member(val_type, ArgType, required);
    member(str_val, string, required);
  };
  member(is_logic_event, bool, required);
  member(ev_name, string, required);
  member(group, string, required);
  member(args, EventArg, repeated);
};

message(Msg_RC_SendGfxMessage) {
  message(EventArg) {
    member(val_type, ArgType, required);
    member(str_val, string, required);
  };
  member(is_with_tag, bool, required);
  member(name, string, required);
  member(msg, string, required);
  member(args, EventArg, repeated);
};

message(Msg_RC_HighlightPrompt) {
	member(dict_id, string, required);
	member(argument, string, repeated);
};

message(Msg_RC_ShowDlg) {
  member(dialog_id, int, required);
};

message(Msg_CR_DlgClosed) {
  member(dialog_id, int, required);
};

message(Msg_RC_LockFrame) {
  member(scale, float, required);
};

message(Msg_RC_PlayAnimation) {
  member(obj_id, int, required);
  member(anim_name, string, required);
};