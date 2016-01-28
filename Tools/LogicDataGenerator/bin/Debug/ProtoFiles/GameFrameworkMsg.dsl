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
message(Msg_CRC_Create) {
  member(role_id, int, optional);	//玩家ID，上行时不设置，下同
  member(is_player_self, bool, optional);
  member(hero_id, int, optional);
  member(camp_id, int, optional);
  member(position, Position, optional);
  member(face_dirction, float, optional);
  member(skill_levels, int, repeated);
  member(scene_start_time, long, optional);
  member(nickname, string, optional);
  member(shop_equipments_id, int, repeated);
  member(role_level, int, optional);
  member(fashion_cloth_id, int, optional);
  member(owner_id, int, optional);
  member(partner_id, int, optional);
};

// 玩家进入
message(Msg_RC_Enter) {
  member(role_id, int, required);
  member(hero_id, int, required);
  member(camp_id, int, required);
  member(position, Position, required);
  member(face_dir, float, required);
  member(is_moving, bool, required);
  member(move_dir, float, required);
};

message(Msg_RC_Disappear) {
  member(role_id, int, required);
};

message(Msg_RC_Dead) {
  member(role_id, int, required);
};

message(Msg_RC_Revive) {
  member(role_id, int, required);	
  member(is_player_self, bool, required);
  member(hero_id, int, required);
  member(camp_id, int, required);
  member(position, Position, required);
  member(face_direction, float, required);
};

// 玩家离开
message(Msg_CRC_Exit) {
  member(role_id, int, optional);
};

message(Msg_CRC_MoveStart) {
  member(role_id, int, optional);
  member(position, ulong, required);
  member(dir, int, required);
  member(is_skill_moving, bool, required);
};

message(Msg_CRC_MoveStop) {
  member(role_id, int, optional);
  member(position, ulong, optional);
};

message(Msg_CRC_MoveMeetObstacle) {
  member(role_id, int, optional);
  member(cur_pos_x, int, required);
  member(cur_pos_z, int, required);
};

// 玩家方向改变
message(Msg_CRC_Face) {
  member(role_id, int, optional);
  member(face_direction, int, required);
};

// 玩家技能
message(Msg_CRC_Skill) {
  member(role_id, int, optional);
  member(skill_index_id, int, required);
  member(stand_pos, ulong, required);
  member(face_direction, int, required);
  member(want_face_dir, int, required);
};

message(Msg_CRC_StopSkill) {
  member(role_id, int, optional);
  member(skill_index_id, int, required);
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
  member(add_attr_id, int, optional);
  member(level, int, optional);
  member(hp_rate, int, optional);
};
message(Msg_RC_CreatePartner) {
  member(npc_id, int, required);
  member(unit_id, int, required);
  member(cur_pos, Position, required);
  member(face_direction, float, required);
  member(link_id, int, required);
  member(camp_id, int, optional);
  member(owner_id, int, optional);
  member(add_attr_id, int, optional);
};

message(Msg_RC_DestroyNpc) {
  member(npc_id, int, required);
  member(need_play_effect, bool, required);
};

// Npc进入
message(Msg_RC_NpcEnter) {
  member(npc_id, int, required);
  member(cur_pos_x, float, required);
  member(cur_pos_z, float, required);
  member(face_direction, float, required);
};

message(Msg_RC_NpcMove) {
  member(npc_id, int, required);
  member(move_mode, int, optional);
  member(move_direction, int, optional);
  member(velocity, int, optional);
  member(cur_pos_x, int, optional);
  member(cur_pos_z, int, optional);
};

message(Msg_RC_NpcFace) {
  member(npc_id, int, optional);
  member(face_direction, int, required);
};

message(Msg_RC_NpcSkill) {
  member(npc_id, int, required);
  member(skill_index_id, int, required);
  member(stand_pos, ulong, required);
  member(face_direction, int, required);
};

message(Msg_CRC_NpcStopSkill) {
  member(npc_id, int, required);
};

message(Msg_RC_NpcDead) {
  member(npc_id, int, required);
};

message(Msg_RC_NpcDisappear) {
  member(npc_id, int, required);
};

message(Msg_RC_SyncProperty) {
  member(role_id, int, required);	
	member(hp, int, required);
	member(np, int, required);
	member(state, int, required);
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

message(Msg_CR_SwitchDebug) {
  member(is_debug, bool, required);
};

message(Msg_RC_SyncCombatStatisticInfo) {
  member(role_id, int, required);
  member(kill_hero_count, int, required);
  member(assit_kill_count, int, required);
  member(kill_npc_count, int, required);
  member(dead_count, int, required);
};

message(Msg_RC_SyncCombatInfo) {
  member(hit_count, int, required);
};

message(Msg_CRC_SendImpactToEntity) {
  member(sender_id, int, required);
  member(target_id, int, required);
  member(impact_index_id, int, required);
  member(skill_index_id, int, required);
  member(duration, int, required);
  member(sender_pos, ulong, optional);
  member(sender_dir, int, optional);
  member(hit_count, int, optional);
  member(hit_count_id, long, optional);
};

message(Msg_CRC_SendImpactToEntityInPvp) {
  member(sender_id, int, required);
  member(target_id, int, required);
  member(impact_index_id, int, required);
  member(skill_index_id, int, required);
  member(duration, int, required);
  member(sender_pos, ulong, required);
  member(sender_dir, int, required);
  member(target_pos, ulong, required);
  member(target_dir, int, required);
  member(hit_count, int, optional);
  member(hit_count_id, long, optional);
};

message(Msg_CRC_StopGfxImpact) {
  member(target_id, int, required);
  member(impact_index_id, int, required);
};

message(Msg_CRC_InteractObject) {
  member(initiator_id, int, optional);
  member(receiver_id, int, required);	
};

message(Msg_RC_ImpactDamage) {
  member(role_id, int, required);
  member(attacker_id, int, required);
  member(damage_status, int, required);
  member(hp, int, required);
  member(energy, int, optional);
};
message(Msg_RC_ImpactRage) {
  member(role_id, int, required);
  member(rage, int, required);
};

message(Msg_RC_ControlObject) {
  member(controller_id, int, required);
  member(controlled_id, int, required);
  member(control_or_release, bool, required);
};

message(Msg_RC_RefreshItemSkills) {
  member(role_id, int, required);
};

message(Msg_RC_HighlightPrompt) {
	member(dict_id, int, required);
	member(argument, string, repeated);
};

message(Msg_CR_Quit) {
	member(is_force, bool, required);
};

message(Msg_RC_UserMove) {
  member(role_id, int, required);
  member(is_moving, bool, optional);
  member(move_direction, int, optional);
  member(cur_pos_x, int, optional);
  member(cur_pos_z, int, optional);
};

message(Msg_RC_UserFace) {
  member(role_id, int, required);
  member(face_direction, float, required);
};

message(Msg_RC_UserSkill) {
  member(user_id, int, required);
  member(skill_index_id, int, required);
  member(stand_pos, ulong, required);
  member(face_direction, int, required);
};

message(Msg_CRC_UserStopSkill) {
  member(user_id, int, required);
};

message(Msg_CR_UserMoveToPos) {
  member(target_pos_x, int, required);
	member(target_pos_z, int, required);
  member(cur_pos_x, int, required);
  member(cur_pos_z, int, required);
};

message(Msg_CR_UserMoveToAttack) {
  member(target_id, int, required);
  member(attack_range, int, required);
  member(cur_pos_x, int, required);
  member(cur_pos_z, int, required);
};

message(Msg_RC_UpdateUserBattleInfo) {
  member(role_id, int, required);
  message(PresetInfo) {
  	member(skill_id, int, required);
  	member(skill_level, int, required);
  };
  member(skill_info, PresetInfo, repeated);
  member(preset_index, int, required);
  message(EquipInfo) {
  	member(equip_id, int, required);
  	member(equip_level, int, required);
  	member(equip_random_property, int, required);
	member(equip_upgrade_star, int, required);
	member(equip_strength_level, int, required);
  };
  member(equip_info, EquipInfo, repeated);
  message(LegacyInfo) {
  	member(legacy_id, int, required);
  	member(legacy_level, int, required);
  	member(legacy_random_property, int, required);
  	member(legacy_IsUnlock, bool, required);
  };
  member(legacy_info, LegacyInfo, repeated);
  message(XSoulDataInfo) {
    member(ItemId, int, required);
    member(Level, int, required);
    member(ModelLevel, int, required);
    member(Experience, int, required);
  };
  member(XSouls, XSoulDataInfo, repeated);
  message(PartnerDataInfo) {
    member(PartnerId, int, required);
    member(PartnerLevel, int, required);
    member(PartnerStage, int, required);
	member(PartnerEquipState, bool, repeated);
  };
  member(Partners, PartnerDataInfo, repeated);
  message(TalentDataMsg) {
    member(Slot, int, required);
    member(ItemId, int, required);
    member(Level, int, required);
    member(Experience, int, required);
  };
  member(EquipTalents, TalentDataMsg, repeated);
  message(FashionMsg) {
	member(PartIndex, int, required);
	member(FsnId, int, required);
  };
  member(FashionInfo, FashionMsg, repeated);
};

message(Msg_RC_MissionCompleted) {
  member(target_scene_id, int, required);
};

message(Msg_RC_ChangeScene) {
  member(target_scene_id, int, required);
};

message(Msg_RC_CampChanged) {
  member(obj_id, int, required);
  member(camp_id, int, required);
};

message(Msg_RC_EnableInput) {
  member(is_enable, bool, required);
};

message(Msg_RC_ShowUi) {
  member(is_show, bool, required);
};

message(Msg_RC_ShowWall) {
  member(wall_name, string, required);
  member(is_show, bool, required);
};

message(Msg_RC_ShowDlg) {
  member(dialog_id, int, required);
};

message(Msg_CR_DlgClosed) {
  member(dialog_id, int, required);
};

message(Msg_RC_CameraLookat) {
  member(x, float, required);
  member(y, float, required);
  member(z, float, required);
  member(is_immediately, bool, required);
};

message(Msg_RC_CameraFollow) {
  member(obj_id, int, required);
  member(is_immediately, bool, required);  
};

message(Msg_RC_CameraYaw) {
  member(yaw, float, required);
  member(smooth_lag, int, required);
};

message(Msg_RC_CameraHeight) {
  member(height, float, required);
  member(smooth_lag, int, required);
};

message(Msg_RC_CameraDistance) {
  member(distance, float, required);
  member(smooth_lag, int, required);
};

message(Msg_RC_SetBlockedShader) {
  member(rim_color_1, uint, required);
  member(rim_power_1, float, required);
  member(rim_cutvalue_1, float, required);
  member(rim_color_2, uint, required);
  member(rim_power_2, float, required);
  member(rim_cutvalue_2, float, required);
};

message(Msg_CR_GfxControlMoveStart) {
  member(skill_or_impact_index_id, int, required);
  member(is_skill, bool, required);
  member(obj_id, int, required);
  member(cur_pos, ulong, required);
};

message(Msg_CR_GfxControlMoveStop) {
  member(skill_or_impact_index_id, int, required);
  member(is_skill, bool, required);
  member(obj_id, int, required);
  member(face_dir, int, required);
  member(target_pos, ulong, required);
};

message(Msg_CR_GiveUpBattle) {
};

message(Msg_CR_DeleteDeadNpc) {
  member(npc_id, int, required);
};

message(Msg_RC_UpdateCoefficient) {
  member(obj_id, int, required);
  member(hpmax_coefficient, float, required);
};

message(Msg_RC_AdjustPosition) {
  member(role_id, int, required); 
  member(x, int, required);
  member(z, int, required);
  member(face_dir, int, required);
};

message(Msg_RC_LockFrame) {
  member(scale, float, required);
};

message(Msg_RC_PlayAnimation) {
  member(obj_id, int, required);
  member(anim_type, int, required);
  member(anim_time, int, required);
  member(is_queued, bool, required);
};

message(Msg_RC_StartCountDown) {
  member(count_down_time, int, required);
};

message(Msg_RC_PublishEvent) {
  message(EventArg) {
    member(val_type, int, required);
    member(str_val, string, required);
  };
  member(is_logic_event, bool, required);
  member(ev_name, string, required);
  member(group, string, required);
  member(args, EventArg, repeated);
};

message(Msg_RC_CameraEnable) {
  member(camera_name, string, required);
  member(is_enable, bool, required);
};

message(Msg_CR_HitCountChanged) {
  member(max_multi_hit_count, int, required);
  member(hit_count, int, required);
};

message(Msg_RC_SendGfxMessage) {
  message(EventArg) {
    member(val_type, int, required);
    member(str_val, string, required);
  };
  member(is_with_tag, bool, required);
  member(name, string, required);
  member(msg, string, required);
  member(args, EventArg, repeated);
};

message(Msg_RC_SendGfxMessageById) {
  message(EventArg) {
    member(val_type, int, required);
    member(str_val, string, required);
  };
  member(obj_id, int, required);
  member(msg, string, required);
  member(args, EventArg, repeated);
};

message(Msg_RC_AddSkill) {
  member(obj_id, int, required);
  member(skill_id, int, required);
};

message(Msg_RC_RemoveSkill) {
  member(obj_id, int, required);
  member(skill_id, int, required);
};

message(Msg_RC_StopImpact) {
  member(obj_id, int, required);
  member(impact_index_id, int, required);
};
message(Msg_CR_SyncCharacterGfxState)
{
  member(obj_id, int, required);
  member(gfx_state, int, required);
};
message(Msg_CR_SummonPartner)
{
 member(obj_id, int, required);
};

message(Msg_CRC_SummonNpc)
{
  member(npc_id, int, required);
  member(owner_id, int, required);
  member(summon_owner_id, int, required);
  member(owner_skillid, int, required);
  member(link_id, int, required);
  member(model_prefab, string, required);
  member(skill_id, int, required);
  member(ai_id, int, required);
  member(follow_dead, bool, required);
  member(pos_x, float, required);
  member(pos_y, float, required);
  member(pos_z, float, required);
  member(ai_params, string, required);
};

message(Msg_RC_SyncNpcOwnerId)
{
	member(npc_id, int, required);
  member(owner_id, int, required);
};

message(Msg_CR_GmCommand)
{
  member(type, int, required);
  member(content, string, optional);
};

message(Msg_RC_DropNpc)
{
  member(npc_id, int, required); 
  member(link_id, int, required);
  member(owner_id, int, required);
  member(from_obj_id, int, required);
  member(drop_type, int, required);
  member(drop_num, int, required);
  member(camp_id, int, optional);
  member(model, string, optional);
  member(effect, string, optional);
  member(cur_pos, Position, required);
};

message(Msg_CR_PickUpNpc)
{
  member(npc_id, int, required);
};

message(Msg_CR_PvpIsReady)
{
  member(isReady, bool, required);
};

message(Msg_RC_PvpIsAllReady)
{
  member(isAllReady, bool, required);
};

message(Msg_CRC_BreakSkill) {
  member(RoleId, int, optional);
  member(SkillIndexId, int, required);
  member(Msg, string, required);
};

message(Msg_CRC_StoryMessage)
{
  message(MessageArg) {
    member(val_type, int, required);
    member(str_val, string, required);
  };
  member(m_MsgId, string, required);
  member(m_Args, MessageArg, repeated);
};

message(Msg_CR_TimeCounter) {
};

message(Msg_RC_ClientError) {
  member(ErrorCode, int, required);
};
