//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkMsg.dsl生成！！！
//----------------------------------------------------------------------------

var c_Null = null;
var s_GetSubData = function(data, index, obj){
	if(data instanceof Array && data.length>index && null!=data[index] && data[index] instanceof Array){
		obj.fromJson(data[index]);
	}
};
var s_GetSubDataArray = function(data, index, obj, factory) {
	if(data instanceof Array && data.length>index && null!=data[index] && data[index] instanceof Array){
		var ct = data[index].length;
		for(var i=0;i<ct;++i){
			var val = factory();
			val.fromJson(data[index]);
			obj.push(val);
		}
	}
};
var s_AddSubData = function(data, obj) {
	if(null!=obj){
	  data.push(obj.toJson());
	} else {
	  data.push(obj);
	}
};
var s_AddSubDataArray = function(data, obj) {
	if(null!=obj){
		var subData = new Array();
		for(var i in obj){
			if(null!=obj[i]){
				subData.push(obj[i].toJson());
			} else {
				subData.push(obj[i]);
			}
		}
		data.push(subData);
	} else {
		data.push(obj);
	}
};

function EncodePosition(){

	return {
		x : 0,
		z : 0,

		fromJson : function(data){
			this.x = data[0];
			this.z = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.x);
			data.push(this.z);
			return data;
		},
	};
}

function EncodePosition3D(){

	return {
		x : 0,
		y : 0,
		z : 0,

		fromJson : function(data){
			this.x = data[0];
			this.y = data[1];
			this.z = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.x);
			data.push(this.y);
			data.push(this.z);
			return data;
		},
	};
}

function Msg_CR_DeleteDeadNpc(){

	return {
		npc_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			return data;
		},
	};
}

function Msg_CR_DlgClosed(){

	return {
		dialog_id : 0,

		fromJson : function(data){
			this.dialog_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.dialog_id);
			return data;
		},
	};
}

function Msg_CR_GfxControlMoveStart(){

	return {
		skill_or_impact_index_id : 0,
		is_skill : false,
		obj_id : 0,
		cur_pos : 0,

		fromJson : function(data){
			this.skill_or_impact_index_id = data[0];
			this.is_skill = data[1];
			this.obj_id = data[2];
			this.cur_pos = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.skill_or_impact_index_id);
			data.push(this.is_skill);
			data.push(this.obj_id);
			data.push(this.cur_pos);
			return data;
		},
	};
}

function Msg_CR_GfxControlMoveStop(){

	return {
		skill_or_impact_index_id : 0,
		is_skill : false,
		obj_id : 0,
		face_dir : 0,
		target_pos : 0,

		fromJson : function(data){
			this.skill_or_impact_index_id = data[0];
			this.is_skill = data[1];
			this.obj_id = data[2];
			this.face_dir = data[3];
			this.target_pos = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.skill_or_impact_index_id);
			data.push(this.is_skill);
			data.push(this.obj_id);
			data.push(this.face_dir);
			data.push(this.target_pos);
			return data;
		},
	};
}

function Msg_CR_GiveUpBattle(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CR_GmCommand(){

	return {
		type : 0,
		content : "",

		fromJson : function(data){
			this.type = data[0];
			this.content = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.type);
			data.push(this.content);
			return data;
		},
	};
}

function Msg_CR_HitCountChanged(){

	return {
		max_multi_hit_count : 0,
		hit_count : 0,

		fromJson : function(data){
			this.max_multi_hit_count = data[0];
			this.hit_count = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.max_multi_hit_count);
			data.push(this.hit_count);
			return data;
		},
	};
}

function Msg_CR_Observer(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CR_PickUpNpc(){

	return {
		npc_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			return data;
		},
	};
}

function Msg_CR_PvpIsReady(){

	return {
		isReady : false,

		fromJson : function(data){
			this.isReady = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.isReady);
			return data;
		},
	};
}

function Msg_CR_Quit(){

	return {
		is_force : false,

		fromJson : function(data){
			this.is_force = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_force);
			return data;
		},
	};
}

function Msg_CR_ShakeHands(){

	return {
		auth_key : 0,

		fromJson : function(data){
			this.auth_key = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.auth_key);
			return data;
		},
	};
}

function Msg_CR_SummonPartner(){

	return {
		obj_id : 0,

		fromJson : function(data){
			this.obj_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			return data;
		},
	};
}

function Msg_CR_SwitchDebug(){

	return {
		is_debug : false,

		fromJson : function(data){
			this.is_debug = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_debug);
			return data;
		},
	};
}

function Msg_CR_SyncCharacterGfxState(){

	return {
		obj_id : 0,
		gfx_state : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.gfx_state = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.gfx_state);
			return data;
		},
	};
}

function Msg_CR_TimeCounter(){

	return {

		fromJson : function(data){
		},
		toJson : function(){
			var data = new Array();
			return data;
		},
	};
}

function Msg_CR_UserMoveToAttack(){

	return {
		target_id : 0,
		attack_range : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,

		fromJson : function(data){
			this.target_id = data[0];
			this.attack_range = data[1];
			this.cur_pos_x = data[2];
			this.cur_pos_z = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.target_id);
			data.push(this.attack_range);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			return data;
		},
	};
}

function Msg_CR_UserMoveToPos(){

	return {
		target_pos_x : 0,
		target_pos_z : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,

		fromJson : function(data){
			this.target_pos_x = data[0];
			this.target_pos_z = data[1];
			this.cur_pos_x = data[2];
			this.cur_pos_z = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.target_pos_x);
			data.push(this.target_pos_z);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			return data;
		},
	};
}

function Msg_CRC_BreakSkill(){

	return {
		RoleId : 0,
		SkillIndexId : 0,
		Msg : "",

		fromJson : function(data){
			this.RoleId = data[0];
			this.SkillIndexId = data[1];
			this.Msg = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.RoleId);
			data.push(this.SkillIndexId);
			data.push(this.Msg);
			return data;
		},
	};
}

function Msg_CRC_Create(){

	return {
		role_id : 0,
		is_player_self : false,
		hero_id : 0,
		camp_id : 0,
		position : c_Null/*Position*/,
		face_dirction : 0,
		skill_levels : new Array(),
		scene_start_time : 0,
		nickname : "",
		shop_equipments_id : new Array(),
		role_level : 0,
		fashion_cloth_id : 0,
		owner_id : 0,
		partner_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.is_player_self = data[1];
			this.hero_id = data[2];
			this.camp_id = data[3];
			this.position = new Position();
			s_GetSubData(data, 4, this.position);
			this.face_dirction = data[5];
			this.skill_levels = data[6];
			this.scene_start_time = data[7];
			this.nickname = data[8];
			this.shop_equipments_id = data[9];
			this.role_level = data[10];
			this.fashion_cloth_id = data[11];
			this.owner_id = data[12];
			this.partner_id = data[13];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.is_player_self);
			data.push(this.hero_id);
			data.push(this.camp_id);
			s_AddSubData(data, this.position);
			data.push(this.face_dirction);
			data.push(this.skill_levels);
			data.push(this.scene_start_time);
			data.push(this.nickname);
			data.push(this.shop_equipments_id);
			data.push(this.role_level);
			data.push(this.fashion_cloth_id);
			data.push(this.owner_id);
			data.push(this.partner_id);
			return data;
		},
	};
}

function Msg_CRC_Exit(){

	return {
		role_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			return data;
		},
	};
}

function Msg_CRC_Face(){

	return {
		role_id : 0,
		face_direction : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.face_direction = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_CRC_InteractObject(){

	return {
		initiator_id : 0,
		receiver_id : 0,

		fromJson : function(data){
			this.initiator_id = data[0];
			this.receiver_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.initiator_id);
			data.push(this.receiver_id);
			return data;
		},
	};
}

function Msg_CRC_MoveMeetObstacle(){

	return {
		role_id : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.cur_pos_x = data[1];
			this.cur_pos_z = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			return data;
		},
	};
}

function Msg_CRC_MoveStart(){

	return {
		role_id : 0,
		position : 0,
		dir : 0,
		is_skill_moving : false,

		fromJson : function(data){
			this.role_id = data[0];
			this.position = data[1];
			this.dir = data[2];
			this.is_skill_moving = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.position);
			data.push(this.dir);
			data.push(this.is_skill_moving);
			return data;
		},
	};
}

function Msg_CRC_MoveStop(){

	return {
		role_id : 0,
		position : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.position = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.position);
			return data;
		},
	};
}

function Msg_CRC_NpcStopSkill(){

	return {
		npc_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			return data;
		},
	};
}

function Msg_CRC_SendImpactToEntity(){

	return {
		sender_id : 0,
		target_id : 0,
		impact_index_id : 0,
		skill_index_id : 0,
		duration : 0,
		sender_pos : 0,
		sender_dir : 0,
		hit_count : 0,
		hit_count_id : 0,

		fromJson : function(data){
			this.sender_id = data[0];
			this.target_id = data[1];
			this.impact_index_id = data[2];
			this.skill_index_id = data[3];
			this.duration = data[4];
			this.sender_pos = data[5];
			this.sender_dir = data[6];
			this.hit_count = data[7];
			this.hit_count_id = data[8];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.sender_id);
			data.push(this.target_id);
			data.push(this.impact_index_id);
			data.push(this.skill_index_id);
			data.push(this.duration);
			data.push(this.sender_pos);
			data.push(this.sender_dir);
			data.push(this.hit_count);
			data.push(this.hit_count_id);
			return data;
		},
	};
}

function Msg_CRC_SendImpactToEntityInPvp(){

	return {
		sender_id : 0,
		target_id : 0,
		impact_index_id : 0,
		skill_index_id : 0,
		duration : 0,
		sender_pos : 0,
		sender_dir : 0,
		target_pos : 0,
		target_dir : 0,
		hit_count : 0,
		hit_count_id : 0,

		fromJson : function(data){
			this.sender_id = data[0];
			this.target_id = data[1];
			this.impact_index_id = data[2];
			this.skill_index_id = data[3];
			this.duration = data[4];
			this.sender_pos = data[5];
			this.sender_dir = data[6];
			this.target_pos = data[7];
			this.target_dir = data[8];
			this.hit_count = data[9];
			this.hit_count_id = data[10];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.sender_id);
			data.push(this.target_id);
			data.push(this.impact_index_id);
			data.push(this.skill_index_id);
			data.push(this.duration);
			data.push(this.sender_pos);
			data.push(this.sender_dir);
			data.push(this.target_pos);
			data.push(this.target_dir);
			data.push(this.hit_count);
			data.push(this.hit_count_id);
			return data;
		},
	};
}

function Msg_CRC_Skill(){

	return {
		role_id : 0,
		skill_index_id : 0,
		stand_pos : 0,
		face_direction : 0,
		want_face_dir : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.skill_index_id = data[1];
			this.stand_pos = data[2];
			this.face_direction = data[3];
			this.want_face_dir = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.skill_index_id);
			data.push(this.stand_pos);
			data.push(this.face_direction);
			data.push(this.want_face_dir);
			return data;
		},
	};
}

function Msg_CRC_StopGfxImpact(){

	return {
		target_id : 0,
		impact_index_id : 0,

		fromJson : function(data){
			this.target_id = data[0];
			this.impact_index_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.target_id);
			data.push(this.impact_index_id);
			return data;
		},
	};
}

function Msg_CRC_StopSkill(){

	return {
		role_id : 0,
		skill_index_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.skill_index_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.skill_index_id);
			return data;
		},
	};
}

function Msg_CRC_StoryMessage(){

	function MessageArg(){

		return {
			val_type : 0,
			str_val : "",

			fromJson : function(data){
				this.val_type = data[0];
				this.str_val = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.val_type);
				data.push(this.str_val);
				return data;
			},
		};
	}

	Msg_CRC_StoryMessage.MessageArg = MessageArg;

	return {
		m_MsgId : "",
		m_Args : new Array(),

		fromJson : function(data){
			this.m_MsgId = data[0];
			s_GetSubDataArray(data, 1, this.m_Args, function(){return new MessageArg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.m_MsgId);
			s_AddSubDataArray(data, this.m_Args);
			return data;
		},
	};
}

function Msg_CRC_SummonNpc(){

	return {
		npc_id : 0,
		owner_id : 0,
		summon_owner_id : 0,
		owner_skillid : 0,
		link_id : 0,
		model_prefab : "",
		skill_id : 0,
		ai_id : 0,
		follow_dead : false,
		pos_x : 0,
		pos_y : 0,
		pos_z : 0,
		ai_params : "",

		fromJson : function(data){
			this.npc_id = data[0];
			this.owner_id = data[1];
			this.summon_owner_id = data[2];
			this.owner_skillid = data[3];
			this.link_id = data[4];
			this.model_prefab = data[5];
			this.skill_id = data[6];
			this.ai_id = data[7];
			this.follow_dead = data[8];
			this.pos_x = data[9];
			this.pos_y = data[10];
			this.pos_z = data[11];
			this.ai_params = data[12];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.owner_id);
			data.push(this.summon_owner_id);
			data.push(this.owner_skillid);
			data.push(this.link_id);
			data.push(this.model_prefab);
			data.push(this.skill_id);
			data.push(this.ai_id);
			data.push(this.follow_dead);
			data.push(this.pos_x);
			data.push(this.pos_y);
			data.push(this.pos_z);
			data.push(this.ai_params);
			return data;
		},
	};
}

function Msg_CRC_UserStopSkill(){

	return {
		user_id : 0,

		fromJson : function(data){
			this.user_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.user_id);
			return data;
		},
	};
}

function Msg_Ping(){

	return {
		send_ping_time : 0,

		fromJson : function(data){
			this.send_ping_time = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.send_ping_time);
			return data;
		},
	};
}

function Msg_Pong(){

	return {
		send_ping_time : 0,
		send_pong_time : 0,

		fromJson : function(data){
			this.send_ping_time = data[0];
			this.send_pong_time = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.send_ping_time);
			data.push(this.send_pong_time);
			return data;
		},
	};
}

function Msg_RC_AddSkill(){

	return {
		obj_id : 0,
		skill_id : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.skill_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.skill_id);
			return data;
		},
	};
}

function Msg_RC_AdjustPosition(){

	return {
		role_id : 0,
		x : 0,
		z : 0,
		face_dir : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.x = data[1];
			this.z = data[2];
			this.face_dir = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.x);
			data.push(this.z);
			data.push(this.face_dir);
			return data;
		},
	};
}

function Msg_RC_CameraDistance(){

	return {
		distance : 0,
		smooth_lag : 0,

		fromJson : function(data){
			this.distance = data[0];
			this.smooth_lag = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.distance);
			data.push(this.smooth_lag);
			return data;
		},
	};
}

function Msg_RC_CameraEnable(){

	return {
		camera_name : "",
		is_enable : false,

		fromJson : function(data){
			this.camera_name = data[0];
			this.is_enable = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.camera_name);
			data.push(this.is_enable);
			return data;
		},
	};
}

function Msg_RC_CameraFollow(){

	return {
		obj_id : 0,
		is_immediately : false,

		fromJson : function(data){
			this.obj_id = data[0];
			this.is_immediately = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.is_immediately);
			return data;
		},
	};
}

function Msg_RC_CameraHeight(){

	return {
		height : 0,
		smooth_lag : 0,

		fromJson : function(data){
			this.height = data[0];
			this.smooth_lag = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.height);
			data.push(this.smooth_lag);
			return data;
		},
	};
}

function Msg_RC_CameraLookat(){

	return {
		x : 0,
		y : 0,
		z : 0,
		is_immediately : false,

		fromJson : function(data){
			this.x = data[0];
			this.y = data[1];
			this.z = data[2];
			this.is_immediately = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.x);
			data.push(this.y);
			data.push(this.z);
			data.push(this.is_immediately);
			return data;
		},
	};
}

function Msg_RC_CameraYaw(){

	return {
		yaw : 0,
		smooth_lag : 0,

		fromJson : function(data){
			this.yaw = data[0];
			this.smooth_lag = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.yaw);
			data.push(this.smooth_lag);
			return data;
		},
	};
}

function Msg_RC_CampChanged(){

	return {
		obj_id : 0,
		camp_id : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.camp_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.camp_id);
			return data;
		},
	};
}

function Msg_RC_ChangeScene(){

	return {
		target_scene_id : 0,

		fromJson : function(data){
			this.target_scene_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.target_scene_id);
			return data;
		},
	};
}

function Msg_RC_ClientError(){

	return {
		ErrorCode : 0,

		fromJson : function(data){
			this.ErrorCode = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.ErrorCode);
			return data;
		},
	};
}

function Msg_RC_ControlObject(){

	return {
		controller_id : 0,
		controlled_id : 0,
		control_or_release : false,

		fromJson : function(data){
			this.controller_id = data[0];
			this.controlled_id = data[1];
			this.control_or_release = data[2];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.controller_id);
			data.push(this.controlled_id);
			data.push(this.control_or_release);
			return data;
		},
	};
}

function Msg_RC_CreateNpc(){

	return {
		npc_id : 0,
		unit_id : 0,
		cur_pos : c_Null/*Position*/,
		face_direction : 0,
		link_id : 0,
		camp_id : 0,
		owner_id : 0,
		add_attr_id : 0,
		level : 0,
		hp_rate : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.unit_id = data[1];
			this.cur_pos = new Position();
			s_GetSubData(data, 2, this.cur_pos);
			this.face_direction = data[3];
			this.link_id = data[4];
			this.camp_id = data[5];
			this.owner_id = data[6];
			this.add_attr_id = data[7];
			this.level = data[8];
			this.hp_rate = data[9];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.unit_id);
			s_AddSubData(data, this.cur_pos);
			data.push(this.face_direction);
			data.push(this.link_id);
			data.push(this.camp_id);
			data.push(this.owner_id);
			data.push(this.add_attr_id);
			data.push(this.level);
			data.push(this.hp_rate);
			return data;
		},
	};
}

function Msg_RC_CreatePartner(){

	return {
		npc_id : 0,
		unit_id : 0,
		cur_pos : c_Null/*Position*/,
		face_direction : 0,
		link_id : 0,
		camp_id : 0,
		owner_id : 0,
		add_attr_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.unit_id = data[1];
			this.cur_pos = new Position();
			s_GetSubData(data, 2, this.cur_pos);
			this.face_direction = data[3];
			this.link_id = data[4];
			this.camp_id = data[5];
			this.owner_id = data[6];
			this.add_attr_id = data[7];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.unit_id);
			s_AddSubData(data, this.cur_pos);
			data.push(this.face_direction);
			data.push(this.link_id);
			data.push(this.camp_id);
			data.push(this.owner_id);
			data.push(this.add_attr_id);
			return data;
		},
	};
}

function Msg_RC_Dead(){

	return {
		role_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			return data;
		},
	};
}

function Msg_RC_DebugSpaceInfo(){

	function DebugSpaceInfo(){

		return {
			obj_id : 0,
			is_player : false,
			pos_x : 0,
			pos_z : 0,
			face_dir : 0,

			fromJson : function(data){
				this.obj_id = data[0];
				this.is_player = data[1];
				this.pos_x = data[2];
				this.pos_z = data[3];
				this.face_dir = data[4];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.obj_id);
				data.push(this.is_player);
				data.push(this.pos_x);
				data.push(this.pos_z);
				data.push(this.face_dir);
				return data;
			},
		};
	}

	Msg_RC_DebugSpaceInfo.DebugSpaceInfo = DebugSpaceInfo;

	return {
		space_infos : new Array(),

		fromJson : function(data){
			s_GetSubDataArray(data, 0, this.space_infos, function(){return new DebugSpaceInfo();});
		},
		toJson : function(){
			var data = new Array();
			s_AddSubDataArray(data, this.space_infos);
			return data;
		},
	};
}

function Msg_RC_DestroyNpc(){

	return {
		npc_id : 0,
		need_play_effect : false,

		fromJson : function(data){
			this.npc_id = data[0];
			this.need_play_effect = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.need_play_effect);
			return data;
		},
	};
}

function Msg_RC_Disappear(){

	return {
		role_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			return data;
		},
	};
}

function Msg_RC_DropNpc(){

	return {
		npc_id : 0,
		link_id : 0,
		owner_id : 0,
		from_obj_id : 0,
		drop_type : 0,
		drop_num : 0,
		camp_id : 0,
		model : "",
		effect : "",
		cur_pos : c_Null/*Position*/,

		fromJson : function(data){
			this.npc_id = data[0];
			this.link_id = data[1];
			this.owner_id = data[2];
			this.from_obj_id = data[3];
			this.drop_type = data[4];
			this.drop_num = data[5];
			this.camp_id = data[6];
			this.model = data[7];
			this.effect = data[8];
			this.cur_pos = new Position();
			s_GetSubData(data, 9, this.cur_pos);
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.link_id);
			data.push(this.owner_id);
			data.push(this.from_obj_id);
			data.push(this.drop_type);
			data.push(this.drop_num);
			data.push(this.camp_id);
			data.push(this.model);
			data.push(this.effect);
			s_AddSubData(data, this.cur_pos);
			return data;
		},
	};
}

function Msg_RC_EnableInput(){

	return {
		is_enable : false,

		fromJson : function(data){
			this.is_enable = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_enable);
			return data;
		},
	};
}

function Msg_RC_Enter(){

	return {
		role_id : 0,
		hero_id : 0,
		camp_id : 0,
		position : c_Null/*Position*/,
		face_dir : 0,
		is_moving : false,
		move_dir : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.hero_id = data[1];
			this.camp_id = data[2];
			this.position = new Position();
			s_GetSubData(data, 3, this.position);
			this.face_dir = data[4];
			this.is_moving = data[5];
			this.move_dir = data[6];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.hero_id);
			data.push(this.camp_id);
			s_AddSubData(data, this.position);
			data.push(this.face_dir);
			data.push(this.is_moving);
			data.push(this.move_dir);
			return data;
		},
	};
}

function Msg_RC_HighlightPrompt(){

	return {
		dict_id : 0,
		argument : new Array(),

		fromJson : function(data){
			this.dict_id = data[0];
			this.argument = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.dict_id);
			data.push(this.argument);
			return data;
		},
	};
}

function Msg_RC_ImpactDamage(){

	return {
		role_id : 0,
		attacker_id : 0,
		damage_status : 0,
		hp : 0,
		energy : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.attacker_id = data[1];
			this.damage_status = data[2];
			this.hp = data[3];
			this.energy = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.attacker_id);
			data.push(this.damage_status);
			data.push(this.hp);
			data.push(this.energy);
			return data;
		},
	};
}

function Msg_RC_ImpactRage(){

	return {
		role_id : 0,
		rage : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.rage = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.rage);
			return data;
		},
	};
}

function Msg_RC_LockFrame(){

	return {
		scale : 0,

		fromJson : function(data){
			this.scale = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.scale);
			return data;
		},
	};
}

function Msg_RC_MissionCompleted(){

	return {
		target_scene_id : 0,

		fromJson : function(data){
			this.target_scene_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.target_scene_id);
			return data;
		},
	};
}

function Msg_RC_NpcDead(){

	return {
		npc_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			return data;
		},
	};
}

function Msg_RC_NpcDisappear(){

	return {
		npc_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			return data;
		},
	};
}

function Msg_RC_NpcEnter(){

	return {
		npc_id : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,
		face_direction : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.cur_pos_x = data[1];
			this.cur_pos_z = data[2];
			this.face_direction = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_RC_NpcFace(){

	return {
		npc_id : 0,
		face_direction : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.face_direction = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_RC_NpcMove(){

	return {
		npc_id : 0,
		move_mode : 0,
		move_direction : 0,
		velocity : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.move_mode = data[1];
			this.move_direction = data[2];
			this.velocity = data[3];
			this.cur_pos_x = data[4];
			this.cur_pos_z = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.move_mode);
			data.push(this.move_direction);
			data.push(this.velocity);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			return data;
		},
	};
}

function Msg_RC_NpcSkill(){

	return {
		npc_id : 0,
		skill_index_id : 0,
		stand_pos : 0,
		face_direction : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.skill_index_id = data[1];
			this.stand_pos = data[2];
			this.face_direction = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.skill_index_id);
			data.push(this.stand_pos);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_RC_PlayAnimation(){

	return {
		obj_id : 0,
		anim_type : 0,
		anim_time : 0,
		is_queued : false,

		fromJson : function(data){
			this.obj_id = data[0];
			this.anim_type = data[1];
			this.anim_time = data[2];
			this.is_queued = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.anim_type);
			data.push(this.anim_time);
			data.push(this.is_queued);
			return data;
		},
	};
}

function Msg_RC_PublishEvent(){

	function EventArg(){

		return {
			val_type : 0,
			str_val : "",

			fromJson : function(data){
				this.val_type = data[0];
				this.str_val = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.val_type);
				data.push(this.str_val);
				return data;
			},
		};
	}

	Msg_RC_PublishEvent.EventArg = EventArg;

	return {
		is_logic_event : false,
		ev_name : "",
		group : "",
		args : new Array(),

		fromJson : function(data){
			this.is_logic_event = data[0];
			this.ev_name = data[1];
			this.group = data[2];
			s_GetSubDataArray(data, 3, this.args, function(){return new EventArg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_logic_event);
			data.push(this.ev_name);
			data.push(this.group);
			s_AddSubDataArray(data, this.args);
			return data;
		},
	};
}

function Msg_RC_PvpIsAllReady(){

	return {
		isAllReady : false,

		fromJson : function(data){
			this.isAllReady = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.isAllReady);
			return data;
		},
	};
}

function Msg_RC_RefreshItemSkills(){

	return {
		role_id : 0,

		fromJson : function(data){
			this.role_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			return data;
		},
	};
}

function Msg_RC_RemoveSkill(){

	return {
		obj_id : 0,
		skill_id : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.skill_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.skill_id);
			return data;
		},
	};
}

function Msg_RC_Revive(){

	return {
		role_id : 0,
		is_player_self : false,
		hero_id : 0,
		camp_id : 0,
		position : c_Null/*Position*/,
		face_direction : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.is_player_self = data[1];
			this.hero_id = data[2];
			this.camp_id = data[3];
			this.position = new Position();
			s_GetSubData(data, 4, this.position);
			this.face_direction = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.is_player_self);
			data.push(this.hero_id);
			data.push(this.camp_id);
			s_AddSubData(data, this.position);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_RC_SendGfxMessage(){

	function EventArg(){

		return {
			val_type : 0,
			str_val : "",

			fromJson : function(data){
				this.val_type = data[0];
				this.str_val = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.val_type);
				data.push(this.str_val);
				return data;
			},
		};
	}

	Msg_RC_SendGfxMessage.EventArg = EventArg;

	return {
		is_with_tag : false,
		name : "",
		msg : "",
		args : new Array(),

		fromJson : function(data){
			this.is_with_tag = data[0];
			this.name = data[1];
			this.msg = data[2];
			s_GetSubDataArray(data, 3, this.args, function(){return new EventArg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_with_tag);
			data.push(this.name);
			data.push(this.msg);
			s_AddSubDataArray(data, this.args);
			return data;
		},
	};
}

function Msg_RC_SendGfxMessageById(){

	function EventArg(){

		return {
			val_type : 0,
			str_val : "",

			fromJson : function(data){
				this.val_type = data[0];
				this.str_val = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.val_type);
				data.push(this.str_val);
				return data;
			},
		};
	}

	Msg_RC_SendGfxMessageById.EventArg = EventArg;

	return {
		obj_id : 0,
		msg : "",
		args : new Array(),

		fromJson : function(data){
			this.obj_id = data[0];
			this.msg = data[1];
			s_GetSubDataArray(data, 2, this.args, function(){return new EventArg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.msg);
			s_AddSubDataArray(data, this.args);
			return data;
		},
	};
}

function Msg_RC_SetBlockedShader(){

	return {
		rim_color_1 : 0,
		rim_power_1 : 0,
		rim_cutvalue_1 : 0,
		rim_color_2 : 0,
		rim_power_2 : 0,
		rim_cutvalue_2 : 0,

		fromJson : function(data){
			this.rim_color_1 = data[0];
			this.rim_power_1 = data[1];
			this.rim_cutvalue_1 = data[2];
			this.rim_color_2 = data[3];
			this.rim_power_2 = data[4];
			this.rim_cutvalue_2 = data[5];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.rim_color_1);
			data.push(this.rim_power_1);
			data.push(this.rim_cutvalue_1);
			data.push(this.rim_color_2);
			data.push(this.rim_power_2);
			data.push(this.rim_cutvalue_2);
			return data;
		},
	};
}

function Msg_RC_ShakeHands_Ret(){

	var RetType = {
		SUCCESS : 0,
		ERROR : 1,
	};

	Msg_RC_ShakeHands_Ret.RetType = RetType;

	return {
		auth_result : 0,

		fromJson : function(data){
			this.auth_result = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.auth_result);
			return data;
		},
	};
}

function Msg_RC_ShowDlg(){

	return {
		dialog_id : 0,

		fromJson : function(data){
			this.dialog_id = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.dialog_id);
			return data;
		},
	};
}

function Msg_RC_ShowUi(){

	return {
		is_show : false,

		fromJson : function(data){
			this.is_show = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.is_show);
			return data;
		},
	};
}

function Msg_RC_ShowWall(){

	return {
		wall_name : "",
		is_show : false,

		fromJson : function(data){
			this.wall_name = data[0];
			this.is_show = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.wall_name);
			data.push(this.is_show);
			return data;
		},
	};
}

function Msg_RC_StartCountDown(){

	return {
		count_down_time : 0,

		fromJson : function(data){
			this.count_down_time = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.count_down_time);
			return data;
		},
	};
}

function Msg_RC_StopImpact(){

	return {
		obj_id : 0,
		impact_index_id : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.impact_index_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.impact_index_id);
			return data;
		},
	};
}

function Msg_RC_SyncCombatInfo(){

	return {
		hit_count : 0,

		fromJson : function(data){
			this.hit_count = data[0];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.hit_count);
			return data;
		},
	};
}

function Msg_RC_SyncCombatStatisticInfo(){

	return {
		role_id : 0,
		kill_hero_count : 0,
		assit_kill_count : 0,
		kill_npc_count : 0,
		dead_count : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.kill_hero_count = data[1];
			this.assit_kill_count = data[2];
			this.kill_npc_count = data[3];
			this.dead_count = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.kill_hero_count);
			data.push(this.assit_kill_count);
			data.push(this.kill_npc_count);
			data.push(this.dead_count);
			return data;
		},
	};
}

function Msg_RC_SyncNpcOwnerId(){

	return {
		npc_id : 0,
		owner_id : 0,

		fromJson : function(data){
			this.npc_id = data[0];
			this.owner_id = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.npc_id);
			data.push(this.owner_id);
			return data;
		},
	};
}

function Msg_RC_SyncProperty(){

	return {
		role_id : 0,
		hp : 0,
		np : 0,
		state : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.hp = data[1];
			this.np = data[2];
			this.state = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.hp);
			data.push(this.np);
			data.push(this.state);
			return data;
		},
	};
}

function Msg_RC_UpdateCoefficient(){

	return {
		obj_id : 0,
		hpmax_coefficient : 0,

		fromJson : function(data){
			this.obj_id = data[0];
			this.hpmax_coefficient = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.obj_id);
			data.push(this.hpmax_coefficient);
			return data;
		},
	};
}

function Msg_RC_UpdateUserBattleInfo(){

	function EquipInfo(){

		return {
			equip_id : 0,
			equip_level : 0,
			equip_random_property : 0,
			equip_upgrade_star : 0,
			equip_strength_level : 0,

			fromJson : function(data){
				this.equip_id = data[0];
				this.equip_level = data[1];
				this.equip_random_property = data[2];
				this.equip_upgrade_star = data[3];
				this.equip_strength_level = data[4];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.equip_id);
				data.push(this.equip_level);
				data.push(this.equip_random_property);
				data.push(this.equip_upgrade_star);
				data.push(this.equip_strength_level);
				return data;
			},
		};
	}

	function FashionMsg(){

		return {
			PartIndex : 0,
			FsnId : 0,

			fromJson : function(data){
				this.PartIndex = data[0];
				this.FsnId = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.PartIndex);
				data.push(this.FsnId);
				return data;
			},
		};
	}

	function LegacyInfo(){

		return {
			legacy_id : 0,
			legacy_level : 0,
			legacy_random_property : 0,
			legacy_IsUnlock : false,

			fromJson : function(data){
				this.legacy_id = data[0];
				this.legacy_level = data[1];
				this.legacy_random_property = data[2];
				this.legacy_IsUnlock = data[3];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.legacy_id);
				data.push(this.legacy_level);
				data.push(this.legacy_random_property);
				data.push(this.legacy_IsUnlock);
				return data;
			},
		};
	}

	function PartnerDataInfo(){

		return {
			PartnerId : 0,
			PartnerLevel : 0,
			PartnerStage : 0,
			PartnerEquipState : new Array(),

			fromJson : function(data){
				this.PartnerId = data[0];
				this.PartnerLevel = data[1];
				this.PartnerStage = data[2];
				this.PartnerEquipState = data[3];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.PartnerId);
				data.push(this.PartnerLevel);
				data.push(this.PartnerStage);
				data.push(this.PartnerEquipState);
				return data;
			},
		};
	}

	function PresetInfo(){

		return {
			skill_id : 0,
			skill_level : 0,

			fromJson : function(data){
				this.skill_id = data[0];
				this.skill_level = data[1];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.skill_id);
				data.push(this.skill_level);
				return data;
			},
		};
	}

	function TalentDataMsg(){

		return {
			Slot : 0,
			ItemId : 0,
			Level : 0,
			Experience : 0,

			fromJson : function(data){
				this.Slot = data[0];
				this.ItemId = data[1];
				this.Level = data[2];
				this.Experience = data[3];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.Slot);
				data.push(this.ItemId);
				data.push(this.Level);
				data.push(this.Experience);
				return data;
			},
		};
	}

	function XSoulDataInfo(){

		return {
			ItemId : 0,
			Level : 0,
			ModelLevel : 0,
			Experience : 0,

			fromJson : function(data){
				this.ItemId = data[0];
				this.Level = data[1];
				this.ModelLevel = data[2];
				this.Experience = data[3];
			},
			toJson : function(){
				var data = new Array();
				data.push(this.ItemId);
				data.push(this.Level);
				data.push(this.ModelLevel);
				data.push(this.Experience);
				return data;
			},
		};
	}

	Msg_RC_UpdateUserBattleInfo.EquipInfo = EquipInfo;
	Msg_RC_UpdateUserBattleInfo.FashionMsg = FashionMsg;
	Msg_RC_UpdateUserBattleInfo.LegacyInfo = LegacyInfo;
	Msg_RC_UpdateUserBattleInfo.PartnerDataInfo = PartnerDataInfo;
	Msg_RC_UpdateUserBattleInfo.PresetInfo = PresetInfo;
	Msg_RC_UpdateUserBattleInfo.TalentDataMsg = TalentDataMsg;
	Msg_RC_UpdateUserBattleInfo.XSoulDataInfo = XSoulDataInfo;

	return {
		role_id : 0,
		skill_info : new Array(),
		preset_index : 0,
		equip_info : new Array(),
		legacy_info : new Array(),
		XSouls : new Array(),
		Partners : new Array(),
		EquipTalents : new Array(),
		FashionInfo : new Array(),

		fromJson : function(data){
			this.role_id = data[0];
			s_GetSubDataArray(data, 1, this.skill_info, function(){return new PresetInfo();});
			this.preset_index = data[2];
			s_GetSubDataArray(data, 3, this.equip_info, function(){return new EquipInfo();});
			s_GetSubDataArray(data, 4, this.legacy_info, function(){return new LegacyInfo();});
			s_GetSubDataArray(data, 5, this.XSouls, function(){return new XSoulDataInfo();});
			s_GetSubDataArray(data, 6, this.Partners, function(){return new PartnerDataInfo();});
			s_GetSubDataArray(data, 7, this.EquipTalents, function(){return new TalentDataMsg();});
			s_GetSubDataArray(data, 8, this.FashionInfo, function(){return new FashionMsg();});
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			s_AddSubDataArray(data, this.skill_info);
			data.push(this.preset_index);
			s_AddSubDataArray(data, this.equip_info);
			s_AddSubDataArray(data, this.legacy_info);
			s_AddSubDataArray(data, this.XSouls);
			s_AddSubDataArray(data, this.Partners);
			s_AddSubDataArray(data, this.EquipTalents);
			s_AddSubDataArray(data, this.FashionInfo);
			return data;
		},
	};
}

function Msg_RC_UserFace(){

	return {
		role_id : 0,
		face_direction : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.face_direction = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Msg_RC_UserMove(){

	return {
		role_id : 0,
		is_moving : false,
		move_direction : 0,
		cur_pos_x : 0,
		cur_pos_z : 0,

		fromJson : function(data){
			this.role_id = data[0];
			this.is_moving = data[1];
			this.move_direction = data[2];
			this.cur_pos_x = data[3];
			this.cur_pos_z = data[4];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.role_id);
			data.push(this.is_moving);
			data.push(this.move_direction);
			data.push(this.cur_pos_x);
			data.push(this.cur_pos_z);
			return data;
		},
	};
}

function Msg_RC_UserSkill(){

	return {
		user_id : 0,
		skill_index_id : 0,
		stand_pos : 0,
		face_direction : 0,

		fromJson : function(data){
			this.user_id = data[0];
			this.skill_index_id = data[1];
			this.stand_pos = data[2];
			this.face_direction = data[3];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.user_id);
			data.push(this.skill_index_id);
			data.push(this.stand_pos);
			data.push(this.face_direction);
			return data;
		},
	};
}

function Position(){

	return {
		x : 0,
		z : 0,

		fromJson : function(data){
			this.x = data[0];
			this.z = data[1];
		},
		toJson : function(){
			var data = new Array();
			data.push(this.x);
			data.push(this.z);
			return data;
		},
	};
}

var MessageDefine = {
	Msg_Ping : 1,
	Msg_Pong : 2,
	Position : 3,
	EncodePosition : 4,
	EncodePosition3D : 5,
	Msg_CR_ShakeHands : 6,
	Msg_RC_ShakeHands_Ret : 7,
	Msg_CR_Observer : 8,
	Msg_CRC_Create : 9,
	Msg_RC_Enter : 10,
	Msg_RC_Disappear : 11,
	Msg_RC_Dead : 12,
	Msg_RC_Revive : 13,
	Msg_CRC_Exit : 14,
	Msg_CRC_MoveStart : 15,
	Msg_CRC_MoveStop : 16,
	Msg_CRC_MoveMeetObstacle : 17,
	Msg_CRC_Face : 18,
	Msg_CRC_Skill : 19,
	Msg_CRC_StopSkill : 20,
	Msg_RC_CreateNpc : 21,
	Msg_RC_CreatePartner : 22,
	Msg_RC_DestroyNpc : 23,
	Msg_RC_NpcEnter : 24,
	Msg_RC_NpcMove : 25,
	Msg_RC_NpcFace : 26,
	Msg_RC_NpcSkill : 27,
	Msg_CRC_NpcStopSkill : 28,
	Msg_RC_NpcDead : 29,
	Msg_RC_NpcDisappear : 30,
	Msg_RC_SyncProperty : 31,
	Msg_RC_DebugSpaceInfo : 32,
	Msg_CR_SwitchDebug : 33,
	Msg_RC_SyncCombatStatisticInfo : 34,
	Msg_RC_SyncCombatInfo : 35,
	Msg_CRC_SendImpactToEntity : 36,
	Msg_CRC_SendImpactToEntityInPvp : 37,
	Msg_CRC_StopGfxImpact : 38,
	Msg_CRC_InteractObject : 39,
	Msg_RC_ImpactDamage : 40,
	Msg_RC_ImpactRage : 41,
	Msg_RC_ControlObject : 42,
	Msg_RC_RefreshItemSkills : 43,
	Msg_RC_HighlightPrompt : 44,
	Msg_CR_Quit : 45,
	Msg_RC_UserMove : 46,
	Msg_RC_UserFace : 47,
	Msg_RC_UserSkill : 48,
	Msg_CRC_UserStopSkill : 49,
	Msg_CR_UserMoveToPos : 50,
	Msg_CR_UserMoveToAttack : 51,
	Msg_RC_UpdateUserBattleInfo : 52,
	Msg_RC_MissionCompleted : 53,
	Msg_RC_ChangeScene : 54,
	Msg_RC_CampChanged : 55,
	Msg_RC_EnableInput : 56,
	Msg_RC_ShowUi : 57,
	Msg_RC_ShowWall : 58,
	Msg_RC_ShowDlg : 59,
	Msg_CR_DlgClosed : 60,
	Msg_RC_CameraLookat : 61,
	Msg_RC_CameraFollow : 62,
	Msg_RC_CameraYaw : 63,
	Msg_RC_CameraHeight : 64,
	Msg_RC_CameraDistance : 65,
	Msg_RC_SetBlockedShader : 66,
	Msg_CR_GfxControlMoveStart : 67,
	Msg_CR_GfxControlMoveStop : 68,
	Msg_CR_GiveUpBattle : 69,
	Msg_CR_DeleteDeadNpc : 70,
	Msg_RC_UpdateCoefficient : 71,
	Msg_RC_AdjustPosition : 72,
	Msg_RC_LockFrame : 73,
	Msg_RC_PlayAnimation : 74,
	Msg_RC_StartCountDown : 75,
	Msg_RC_PublishEvent : 76,
	Msg_RC_CameraEnable : 77,
	Msg_CR_HitCountChanged : 78,
	Msg_RC_SendGfxMessage : 79,
	Msg_RC_SendGfxMessageById : 80,
	Msg_RC_AddSkill : 81,
	Msg_RC_RemoveSkill : 82,
	Msg_RC_StopImpact : 83,
	Msg_CR_SyncCharacterGfxState : 84,
	Msg_CR_SummonPartner : 85,
	Msg_CRC_SummonNpc : 86,
	Msg_RC_SyncNpcOwnerId : 87,
	Msg_CR_GmCommand : 88,
	Msg_RC_DropNpc : 89,
	Msg_CR_PickUpNpc : 90,
	Msg_CR_PvpIsReady : 91,
	Msg_RC_PvpIsAllReady : 92,
	Msg_CRC_BreakSkill : 93,
	Msg_CRC_StoryMessage : 94,
	Msg_CR_TimeCounter : 95,
	Msg_RC_ClientError : 96,
};

function MessageDefine2Object(){
	var dict = new Object();
	dict[MessageDefine.EncodePosition] = function(){return new EncodePosition();};
	dict[MessageDefine.EncodePosition3D] = function(){return new EncodePosition3D();};
	dict[MessageDefine.Msg_CR_DeleteDeadNpc] = function(){return new Msg_CR_DeleteDeadNpc();};
	dict[MessageDefine.Msg_CR_DlgClosed] = function(){return new Msg_CR_DlgClosed();};
	dict[MessageDefine.Msg_CR_GfxControlMoveStart] = function(){return new Msg_CR_GfxControlMoveStart();};
	dict[MessageDefine.Msg_CR_GfxControlMoveStop] = function(){return new Msg_CR_GfxControlMoveStop();};
	dict[MessageDefine.Msg_CR_GiveUpBattle] = function(){return new Msg_CR_GiveUpBattle();};
	dict[MessageDefine.Msg_CR_GmCommand] = function(){return new Msg_CR_GmCommand();};
	dict[MessageDefine.Msg_CR_HitCountChanged] = function(){return new Msg_CR_HitCountChanged();};
	dict[MessageDefine.Msg_CR_Observer] = function(){return new Msg_CR_Observer();};
	dict[MessageDefine.Msg_CR_PickUpNpc] = function(){return new Msg_CR_PickUpNpc();};
	dict[MessageDefine.Msg_CR_PvpIsReady] = function(){return new Msg_CR_PvpIsReady();};
	dict[MessageDefine.Msg_CR_Quit] = function(){return new Msg_CR_Quit();};
	dict[MessageDefine.Msg_CR_ShakeHands] = function(){return new Msg_CR_ShakeHands();};
	dict[MessageDefine.Msg_CR_SummonPartner] = function(){return new Msg_CR_SummonPartner();};
	dict[MessageDefine.Msg_CR_SwitchDebug] = function(){return new Msg_CR_SwitchDebug();};
	dict[MessageDefine.Msg_CR_SyncCharacterGfxState] = function(){return new Msg_CR_SyncCharacterGfxState();};
	dict[MessageDefine.Msg_CR_TimeCounter] = function(){return new Msg_CR_TimeCounter();};
	dict[MessageDefine.Msg_CR_UserMoveToAttack] = function(){return new Msg_CR_UserMoveToAttack();};
	dict[MessageDefine.Msg_CR_UserMoveToPos] = function(){return new Msg_CR_UserMoveToPos();};
	dict[MessageDefine.Msg_CRC_BreakSkill] = function(){return new Msg_CRC_BreakSkill();};
	dict[MessageDefine.Msg_CRC_Create] = function(){return new Msg_CRC_Create();};
	dict[MessageDefine.Msg_CRC_Exit] = function(){return new Msg_CRC_Exit();};
	dict[MessageDefine.Msg_CRC_Face] = function(){return new Msg_CRC_Face();};
	dict[MessageDefine.Msg_CRC_InteractObject] = function(){return new Msg_CRC_InteractObject();};
	dict[MessageDefine.Msg_CRC_MoveMeetObstacle] = function(){return new Msg_CRC_MoveMeetObstacle();};
	dict[MessageDefine.Msg_CRC_MoveStart] = function(){return new Msg_CRC_MoveStart();};
	dict[MessageDefine.Msg_CRC_MoveStop] = function(){return new Msg_CRC_MoveStop();};
	dict[MessageDefine.Msg_CRC_NpcStopSkill] = function(){return new Msg_CRC_NpcStopSkill();};
	dict[MessageDefine.Msg_CRC_SendImpactToEntity] = function(){return new Msg_CRC_SendImpactToEntity();};
	dict[MessageDefine.Msg_CRC_SendImpactToEntityInPvp] = function(){return new Msg_CRC_SendImpactToEntityInPvp();};
	dict[MessageDefine.Msg_CRC_Skill] = function(){return new Msg_CRC_Skill();};
	dict[MessageDefine.Msg_CRC_StopGfxImpact] = function(){return new Msg_CRC_StopGfxImpact();};
	dict[MessageDefine.Msg_CRC_StopSkill] = function(){return new Msg_CRC_StopSkill();};
	dict[MessageDefine.Msg_CRC_StoryMessage] = function(){return new Msg_CRC_StoryMessage();};
	dict[MessageDefine.Msg_CRC_SummonNpc] = function(){return new Msg_CRC_SummonNpc();};
	dict[MessageDefine.Msg_CRC_UserStopSkill] = function(){return new Msg_CRC_UserStopSkill();};
	dict[MessageDefine.Msg_Ping] = function(){return new Msg_Ping();};
	dict[MessageDefine.Msg_Pong] = function(){return new Msg_Pong();};
	dict[MessageDefine.Msg_RC_AddSkill] = function(){return new Msg_RC_AddSkill();};
	dict[MessageDefine.Msg_RC_AdjustPosition] = function(){return new Msg_RC_AdjustPosition();};
	dict[MessageDefine.Msg_RC_CameraDistance] = function(){return new Msg_RC_CameraDistance();};
	dict[MessageDefine.Msg_RC_CameraEnable] = function(){return new Msg_RC_CameraEnable();};
	dict[MessageDefine.Msg_RC_CameraFollow] = function(){return new Msg_RC_CameraFollow();};
	dict[MessageDefine.Msg_RC_CameraHeight] = function(){return new Msg_RC_CameraHeight();};
	dict[MessageDefine.Msg_RC_CameraLookat] = function(){return new Msg_RC_CameraLookat();};
	dict[MessageDefine.Msg_RC_CameraYaw] = function(){return new Msg_RC_CameraYaw();};
	dict[MessageDefine.Msg_RC_CampChanged] = function(){return new Msg_RC_CampChanged();};
	dict[MessageDefine.Msg_RC_ChangeScene] = function(){return new Msg_RC_ChangeScene();};
	dict[MessageDefine.Msg_RC_ClientError] = function(){return new Msg_RC_ClientError();};
	dict[MessageDefine.Msg_RC_ControlObject] = function(){return new Msg_RC_ControlObject();};
	dict[MessageDefine.Msg_RC_CreateNpc] = function(){return new Msg_RC_CreateNpc();};
	dict[MessageDefine.Msg_RC_CreatePartner] = function(){return new Msg_RC_CreatePartner();};
	dict[MessageDefine.Msg_RC_Dead] = function(){return new Msg_RC_Dead();};
	dict[MessageDefine.Msg_RC_DebugSpaceInfo] = function(){return new Msg_RC_DebugSpaceInfo();};
	dict[MessageDefine.Msg_RC_DestroyNpc] = function(){return new Msg_RC_DestroyNpc();};
	dict[MessageDefine.Msg_RC_Disappear] = function(){return new Msg_RC_Disappear();};
	dict[MessageDefine.Msg_RC_DropNpc] = function(){return new Msg_RC_DropNpc();};
	dict[MessageDefine.Msg_RC_EnableInput] = function(){return new Msg_RC_EnableInput();};
	dict[MessageDefine.Msg_RC_Enter] = function(){return new Msg_RC_Enter();};
	dict[MessageDefine.Msg_RC_HighlightPrompt] = function(){return new Msg_RC_HighlightPrompt();};
	dict[MessageDefine.Msg_RC_ImpactDamage] = function(){return new Msg_RC_ImpactDamage();};
	dict[MessageDefine.Msg_RC_ImpactRage] = function(){return new Msg_RC_ImpactRage();};
	dict[MessageDefine.Msg_RC_LockFrame] = function(){return new Msg_RC_LockFrame();};
	dict[MessageDefine.Msg_RC_MissionCompleted] = function(){return new Msg_RC_MissionCompleted();};
	dict[MessageDefine.Msg_RC_NpcDead] = function(){return new Msg_RC_NpcDead();};
	dict[MessageDefine.Msg_RC_NpcDisappear] = function(){return new Msg_RC_NpcDisappear();};
	dict[MessageDefine.Msg_RC_NpcEnter] = function(){return new Msg_RC_NpcEnter();};
	dict[MessageDefine.Msg_RC_NpcFace] = function(){return new Msg_RC_NpcFace();};
	dict[MessageDefine.Msg_RC_NpcMove] = function(){return new Msg_RC_NpcMove();};
	dict[MessageDefine.Msg_RC_NpcSkill] = function(){return new Msg_RC_NpcSkill();};
	dict[MessageDefine.Msg_RC_PlayAnimation] = function(){return new Msg_RC_PlayAnimation();};
	dict[MessageDefine.Msg_RC_PublishEvent] = function(){return new Msg_RC_PublishEvent();};
	dict[MessageDefine.Msg_RC_PvpIsAllReady] = function(){return new Msg_RC_PvpIsAllReady();};
	dict[MessageDefine.Msg_RC_RefreshItemSkills] = function(){return new Msg_RC_RefreshItemSkills();};
	dict[MessageDefine.Msg_RC_RemoveSkill] = function(){return new Msg_RC_RemoveSkill();};
	dict[MessageDefine.Msg_RC_Revive] = function(){return new Msg_RC_Revive();};
	dict[MessageDefine.Msg_RC_SendGfxMessage] = function(){return new Msg_RC_SendGfxMessage();};
	dict[MessageDefine.Msg_RC_SendGfxMessageById] = function(){return new Msg_RC_SendGfxMessageById();};
	dict[MessageDefine.Msg_RC_SetBlockedShader] = function(){return new Msg_RC_SetBlockedShader();};
	dict[MessageDefine.Msg_RC_ShakeHands_Ret] = function(){return new Msg_RC_ShakeHands_Ret();};
	dict[MessageDefine.Msg_RC_ShowDlg] = function(){return new Msg_RC_ShowDlg();};
	dict[MessageDefine.Msg_RC_ShowUi] = function(){return new Msg_RC_ShowUi();};
	dict[MessageDefine.Msg_RC_ShowWall] = function(){return new Msg_RC_ShowWall();};
	dict[MessageDefine.Msg_RC_StartCountDown] = function(){return new Msg_RC_StartCountDown();};
	dict[MessageDefine.Msg_RC_StopImpact] = function(){return new Msg_RC_StopImpact();};
	dict[MessageDefine.Msg_RC_SyncCombatInfo] = function(){return new Msg_RC_SyncCombatInfo();};
	dict[MessageDefine.Msg_RC_SyncCombatStatisticInfo] = function(){return new Msg_RC_SyncCombatStatisticInfo();};
	dict[MessageDefine.Msg_RC_SyncNpcOwnerId] = function(){return new Msg_RC_SyncNpcOwnerId();};
	dict[MessageDefine.Msg_RC_SyncProperty] = function(){return new Msg_RC_SyncProperty();};
	dict[MessageDefine.Msg_RC_UpdateCoefficient] = function(){return new Msg_RC_UpdateCoefficient();};
	dict[MessageDefine.Msg_RC_UpdateUserBattleInfo] = function(){return new Msg_RC_UpdateUserBattleInfo();};
	dict[MessageDefine.Msg_RC_UserFace] = function(){return new Msg_RC_UserFace();};
	dict[MessageDefine.Msg_RC_UserMove] = function(){return new Msg_RC_UserMove();};
	dict[MessageDefine.Msg_RC_UserSkill] = function(){return new Msg_RC_UserSkill();};
	dict[MessageDefine.Position] = function(){return new Position();};

	function newObject(id){
	  var factory = dict[id];
	  if(factory){
	    return factory();
	  }
	  return null;
	}
	  
	return {
	  newObject : newObject
	};
}

exports.GameFrameworkMessage = {

	MessageDefine : MessageDefine,
	MessageDefine2Object : new MessageDefine2Object(),

	EncodePosition : EncodePosition,
	EncodePosition3D : EncodePosition3D,
	Msg_CR_DeleteDeadNpc : Msg_CR_DeleteDeadNpc,
	Msg_CR_DlgClosed : Msg_CR_DlgClosed,
	Msg_CR_GfxControlMoveStart : Msg_CR_GfxControlMoveStart,
	Msg_CR_GfxControlMoveStop : Msg_CR_GfxControlMoveStop,
	Msg_CR_GiveUpBattle : Msg_CR_GiveUpBattle,
	Msg_CR_GmCommand : Msg_CR_GmCommand,
	Msg_CR_HitCountChanged : Msg_CR_HitCountChanged,
	Msg_CR_Observer : Msg_CR_Observer,
	Msg_CR_PickUpNpc : Msg_CR_PickUpNpc,
	Msg_CR_PvpIsReady : Msg_CR_PvpIsReady,
	Msg_CR_Quit : Msg_CR_Quit,
	Msg_CR_ShakeHands : Msg_CR_ShakeHands,
	Msg_CR_SummonPartner : Msg_CR_SummonPartner,
	Msg_CR_SwitchDebug : Msg_CR_SwitchDebug,
	Msg_CR_SyncCharacterGfxState : Msg_CR_SyncCharacterGfxState,
	Msg_CR_TimeCounter : Msg_CR_TimeCounter,
	Msg_CR_UserMoveToAttack : Msg_CR_UserMoveToAttack,
	Msg_CR_UserMoveToPos : Msg_CR_UserMoveToPos,
	Msg_CRC_BreakSkill : Msg_CRC_BreakSkill,
	Msg_CRC_Create : Msg_CRC_Create,
	Msg_CRC_Exit : Msg_CRC_Exit,
	Msg_CRC_Face : Msg_CRC_Face,
	Msg_CRC_InteractObject : Msg_CRC_InteractObject,
	Msg_CRC_MoveMeetObstacle : Msg_CRC_MoveMeetObstacle,
	Msg_CRC_MoveStart : Msg_CRC_MoveStart,
	Msg_CRC_MoveStop : Msg_CRC_MoveStop,
	Msg_CRC_NpcStopSkill : Msg_CRC_NpcStopSkill,
	Msg_CRC_SendImpactToEntity : Msg_CRC_SendImpactToEntity,
	Msg_CRC_SendImpactToEntityInPvp : Msg_CRC_SendImpactToEntityInPvp,
	Msg_CRC_Skill : Msg_CRC_Skill,
	Msg_CRC_StopGfxImpact : Msg_CRC_StopGfxImpact,
	Msg_CRC_StopSkill : Msg_CRC_StopSkill,
	Msg_CRC_StoryMessage : Msg_CRC_StoryMessage,
	Msg_CRC_SummonNpc : Msg_CRC_SummonNpc,
	Msg_CRC_UserStopSkill : Msg_CRC_UserStopSkill,
	Msg_Ping : Msg_Ping,
	Msg_Pong : Msg_Pong,
	Msg_RC_AddSkill : Msg_RC_AddSkill,
	Msg_RC_AdjustPosition : Msg_RC_AdjustPosition,
	Msg_RC_CameraDistance : Msg_RC_CameraDistance,
	Msg_RC_CameraEnable : Msg_RC_CameraEnable,
	Msg_RC_CameraFollow : Msg_RC_CameraFollow,
	Msg_RC_CameraHeight : Msg_RC_CameraHeight,
	Msg_RC_CameraLookat : Msg_RC_CameraLookat,
	Msg_RC_CameraYaw : Msg_RC_CameraYaw,
	Msg_RC_CampChanged : Msg_RC_CampChanged,
	Msg_RC_ChangeScene : Msg_RC_ChangeScene,
	Msg_RC_ClientError : Msg_RC_ClientError,
	Msg_RC_ControlObject : Msg_RC_ControlObject,
	Msg_RC_CreateNpc : Msg_RC_CreateNpc,
	Msg_RC_CreatePartner : Msg_RC_CreatePartner,
	Msg_RC_Dead : Msg_RC_Dead,
	Msg_RC_DebugSpaceInfo : Msg_RC_DebugSpaceInfo,
	Msg_RC_DestroyNpc : Msg_RC_DestroyNpc,
	Msg_RC_Disappear : Msg_RC_Disappear,
	Msg_RC_DropNpc : Msg_RC_DropNpc,
	Msg_RC_EnableInput : Msg_RC_EnableInput,
	Msg_RC_Enter : Msg_RC_Enter,
	Msg_RC_HighlightPrompt : Msg_RC_HighlightPrompt,
	Msg_RC_ImpactDamage : Msg_RC_ImpactDamage,
	Msg_RC_ImpactRage : Msg_RC_ImpactRage,
	Msg_RC_LockFrame : Msg_RC_LockFrame,
	Msg_RC_MissionCompleted : Msg_RC_MissionCompleted,
	Msg_RC_NpcDead : Msg_RC_NpcDead,
	Msg_RC_NpcDisappear : Msg_RC_NpcDisappear,
	Msg_RC_NpcEnter : Msg_RC_NpcEnter,
	Msg_RC_NpcFace : Msg_RC_NpcFace,
	Msg_RC_NpcMove : Msg_RC_NpcMove,
	Msg_RC_NpcSkill : Msg_RC_NpcSkill,
	Msg_RC_PlayAnimation : Msg_RC_PlayAnimation,
	Msg_RC_PublishEvent : Msg_RC_PublishEvent,
	Msg_RC_PvpIsAllReady : Msg_RC_PvpIsAllReady,
	Msg_RC_RefreshItemSkills : Msg_RC_RefreshItemSkills,
	Msg_RC_RemoveSkill : Msg_RC_RemoveSkill,
	Msg_RC_Revive : Msg_RC_Revive,
	Msg_RC_SendGfxMessage : Msg_RC_SendGfxMessage,
	Msg_RC_SendGfxMessageById : Msg_RC_SendGfxMessageById,
	Msg_RC_SetBlockedShader : Msg_RC_SetBlockedShader,
	Msg_RC_ShakeHands_Ret : Msg_RC_ShakeHands_Ret,
	Msg_RC_ShowDlg : Msg_RC_ShowDlg,
	Msg_RC_ShowUi : Msg_RC_ShowUi,
	Msg_RC_ShowWall : Msg_RC_ShowWall,
	Msg_RC_StartCountDown : Msg_RC_StartCountDown,
	Msg_RC_StopImpact : Msg_RC_StopImpact,
	Msg_RC_SyncCombatInfo : Msg_RC_SyncCombatInfo,
	Msg_RC_SyncCombatStatisticInfo : Msg_RC_SyncCombatStatisticInfo,
	Msg_RC_SyncNpcOwnerId : Msg_RC_SyncNpcOwnerId,
	Msg_RC_SyncProperty : Msg_RC_SyncProperty,
	Msg_RC_UpdateCoefficient : Msg_RC_UpdateCoefficient,
	Msg_RC_UpdateUserBattleInfo : Msg_RC_UpdateUserBattleInfo,
	Msg_RC_UserFace : Msg_RC_UserFace,
	Msg_RC_UserMove : Msg_RC_UserMove,
	Msg_RC_UserSkill : Msg_RC_UserSkill,
	Position : Position,
};
