//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/GameFrameworkMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using LitJson;
using GameFramework;

namespace GameFrameworkMessage
{

	public class EncodePosition : IJsonMessage
	{
		public int x;
		public int z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref x);
			data.Get(1, ref z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(x);
			data.ArrayAdd(z);
			return data;
		}
	}

	public class EncodePosition3D : IJsonMessage
	{
		public int x;
		public int y;
		public int z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref x);
			data.Get(1, ref y);
			data.Get(2, ref z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(x);
			data.ArrayAdd(y);
			data.ArrayAdd(z);
			return data;
		}
	}

	public class Msg_CR_DeleteDeadNpc : IJsonMessage
	{
		public int npc_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			return data;
		}
	}

	public class Msg_CR_DlgClosed : IJsonMessage
	{
		public int dialog_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref dialog_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(dialog_id);
			return data;
		}
	}

	public class Msg_CR_GfxControlMoveStart : IJsonMessage
	{
		public int skill_or_impact_index_id;
		public bool is_skill;
		public int obj_id;
		public ulong cur_pos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref skill_or_impact_index_id);
			data.Get(1, ref is_skill);
			data.Get(2, ref obj_id);
			data.Get(3, ref cur_pos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(skill_or_impact_index_id);
			data.ArrayAdd(is_skill);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(cur_pos);
			return data;
		}
	}

	public class Msg_CR_GfxControlMoveStop : IJsonMessage
	{
		public int skill_or_impact_index_id;
		public bool is_skill;
		public int obj_id;
		public int face_dir;
		public ulong target_pos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref skill_or_impact_index_id);
			data.Get(1, ref is_skill);
			data.Get(2, ref obj_id);
			data.Get(3, ref face_dir);
			data.Get(4, ref target_pos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(skill_or_impact_index_id);
			data.ArrayAdd(is_skill);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(face_dir);
			data.ArrayAdd(target_pos);
			return data;
		}
	}

	public class Msg_CR_GiveUpBattle : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CR_GmCommand : IJsonMessage
	{
		public int type;
		public string content;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref type);
			data.Get(1, ref content);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(type);
			data.ArrayAdd(content);
			return data;
		}
	}

	public class Msg_CR_HitCountChanged : IJsonMessage
	{
		public int max_multi_hit_count;
		public int hit_count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref max_multi_hit_count);
			data.Get(1, ref hit_count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(max_multi_hit_count);
			data.ArrayAdd(hit_count);
			return data;
		}
	}

	public class Msg_CR_Observer : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CR_PickUpNpc : IJsonMessage
	{
		public int npc_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			return data;
		}
	}

	public class Msg_CR_PvpIsReady : IJsonMessage
	{
		public bool isReady;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref isReady);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(isReady);
			return data;
		}
	}

	public class Msg_CR_Quit : IJsonMessage
	{
		public bool is_force;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_force);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_force);
			return data;
		}
	}

	public class Msg_CR_ShakeHands : IJsonMessage
	{
		public uint auth_key;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref auth_key);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(auth_key);
			return data;
		}
	}

	public class Msg_CR_SummonPartner : IJsonMessage
	{
		public int obj_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			return data;
		}
	}

	public class Msg_CR_SwitchDebug : IJsonMessage
	{
		public bool is_debug;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_debug);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_debug);
			return data;
		}
	}

	public class Msg_CR_SyncCharacterGfxState : IJsonMessage
	{
		public int obj_id;
		public int gfx_state;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref gfx_state);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(gfx_state);
			return data;
		}
	}

	public class Msg_CR_TimeCounter : IJsonMessage
	{

		public void FromJson(JsonData data)
		{
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			return data;
		}
	}

	public class Msg_CR_UserMoveToAttack : IJsonMessage
	{
		public int target_id;
		public int attack_range;
		public int cur_pos_x;
		public int cur_pos_z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref target_id);
			data.Get(1, ref attack_range);
			data.Get(2, ref cur_pos_x);
			data.Get(3, ref cur_pos_z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(target_id);
			data.ArrayAdd(attack_range);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			return data;
		}
	}

	public class Msg_CR_UserMoveToPos : IJsonMessage
	{
		public int target_pos_x;
		public int target_pos_z;
		public int cur_pos_x;
		public int cur_pos_z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref target_pos_x);
			data.Get(1, ref target_pos_z);
			data.Get(2, ref cur_pos_x);
			data.Get(3, ref cur_pos_z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(target_pos_x);
			data.ArrayAdd(target_pos_z);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			return data;
		}
	}

	public class Msg_CRC_BreakSkill : IJsonMessage
	{
		public int RoleId;
		public int SkillIndexId;
		public string Msg;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref RoleId);
			data.Get(1, ref SkillIndexId);
			data.Get(2, ref Msg);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(RoleId);
			data.ArrayAdd(SkillIndexId);
			data.ArrayAdd(Msg);
			return data;
		}
	}

	public class Msg_CRC_Create : IJsonMessage
	{
		public int role_id;
		public bool is_player_self;
		public int hero_id;
		public int camp_id;
		public Position position;
		public float face_dirction;
		public List<int> skill_levels = new List<int>();
		public long scene_start_time;
		public string nickname;
		public List<int> shop_equipments_id = new List<int>();
		public int role_level;
		public int fashion_cloth_id;
		public int owner_id;
		public int partner_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref is_player_self);
			data.Get(2, ref hero_id);
			data.Get(3, ref camp_id);
			JsonMessageUtility.GetSubData(data, 4, out position);
			data.Get(5, ref face_dirction);
			JsonMessageUtility.GetSimpleArray(data, 6, ref skill_levels);
			data.Get(7, ref scene_start_time);
			data.Get(8, ref nickname);
			JsonMessageUtility.GetSimpleArray(data, 9, ref shop_equipments_id);
			data.Get(10, ref role_level);
			data.Get(11, ref fashion_cloth_id);
			data.Get(12, ref owner_id);
			data.Get(13, ref partner_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(is_player_self);
			data.ArrayAdd(hero_id);
			data.ArrayAdd(camp_id);
			JsonMessageUtility.AddSubData(data, position);
			data.ArrayAdd(face_dirction);
			JsonMessageUtility.AddSimpleArray(data, skill_levels);
			data.ArrayAdd(scene_start_time);
			data.ArrayAdd(nickname);
			JsonMessageUtility.AddSimpleArray(data, shop_equipments_id);
			data.ArrayAdd(role_level);
			data.ArrayAdd(fashion_cloth_id);
			data.ArrayAdd(owner_id);
			data.ArrayAdd(partner_id);
			return data;
		}
	}

	public class Msg_CRC_Exit : IJsonMessage
	{
		public int role_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			return data;
		}
	}

	public class Msg_CRC_Face : IJsonMessage
	{
		public int role_id;
		public int face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_CRC_InteractObject : IJsonMessage
	{
		public int initiator_id;
		public int receiver_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref initiator_id);
			data.Get(1, ref receiver_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(initiator_id);
			data.ArrayAdd(receiver_id);
			return data;
		}
	}

	public class Msg_CRC_MoveMeetObstacle : IJsonMessage
	{
		public int role_id;
		public int cur_pos_x;
		public int cur_pos_z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref cur_pos_x);
			data.Get(2, ref cur_pos_z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			return data;
		}
	}

	public class Msg_CRC_MoveStart : IJsonMessage
	{
		public int role_id;
		public ulong position;
		public int dir;
		public bool is_skill_moving;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref position);
			data.Get(2, ref dir);
			data.Get(3, ref is_skill_moving);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(position);
			data.ArrayAdd(dir);
			data.ArrayAdd(is_skill_moving);
			return data;
		}
	}

	public class Msg_CRC_MoveStop : IJsonMessage
	{
		public int role_id;
		public ulong position;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref position);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(position);
			return data;
		}
	}

	public class Msg_CRC_NpcStopSkill : IJsonMessage
	{
		public int npc_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			return data;
		}
	}

	public class Msg_CRC_SendImpactToEntity : IJsonMessage
	{
		public int sender_id;
		public int target_id;
		public int impact_index_id;
		public int skill_index_id;
		public int duration;
		public ulong sender_pos;
		public int sender_dir;
		public int hit_count;
		public long hit_count_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref sender_id);
			data.Get(1, ref target_id);
			data.Get(2, ref impact_index_id);
			data.Get(3, ref skill_index_id);
			data.Get(4, ref duration);
			data.Get(5, ref sender_pos);
			data.Get(6, ref sender_dir);
			data.Get(7, ref hit_count);
			data.Get(8, ref hit_count_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(sender_id);
			data.ArrayAdd(target_id);
			data.ArrayAdd(impact_index_id);
			data.ArrayAdd(skill_index_id);
			data.ArrayAdd(duration);
			data.ArrayAdd(sender_pos);
			data.ArrayAdd(sender_dir);
			data.ArrayAdd(hit_count);
			data.ArrayAdd(hit_count_id);
			return data;
		}
	}

	public class Msg_CRC_SendImpactToEntityInPvp : IJsonMessage
	{
		public int sender_id;
		public int target_id;
		public int impact_index_id;
		public int skill_index_id;
		public int duration;
		public ulong sender_pos;
		public int sender_dir;
		public ulong target_pos;
		public int target_dir;
		public int hit_count;
		public long hit_count_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref sender_id);
			data.Get(1, ref target_id);
			data.Get(2, ref impact_index_id);
			data.Get(3, ref skill_index_id);
			data.Get(4, ref duration);
			data.Get(5, ref sender_pos);
			data.Get(6, ref sender_dir);
			data.Get(7, ref target_pos);
			data.Get(8, ref target_dir);
			data.Get(9, ref hit_count);
			data.Get(10, ref hit_count_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(sender_id);
			data.ArrayAdd(target_id);
			data.ArrayAdd(impact_index_id);
			data.ArrayAdd(skill_index_id);
			data.ArrayAdd(duration);
			data.ArrayAdd(sender_pos);
			data.ArrayAdd(sender_dir);
			data.ArrayAdd(target_pos);
			data.ArrayAdd(target_dir);
			data.ArrayAdd(hit_count);
			data.ArrayAdd(hit_count_id);
			return data;
		}
	}

	public class Msg_CRC_Skill : IJsonMessage
	{
		public int role_id;
		public int skill_index_id;
		public ulong stand_pos;
		public int face_direction;
		public int want_face_dir;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref skill_index_id);
			data.Get(2, ref stand_pos);
			data.Get(3, ref face_direction);
			data.Get(4, ref want_face_dir);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(skill_index_id);
			data.ArrayAdd(stand_pos);
			data.ArrayAdd(face_direction);
			data.ArrayAdd(want_face_dir);
			return data;
		}
	}

	public class Msg_CRC_StopGfxImpact : IJsonMessage
	{
		public int target_id;
		public int impact_index_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref target_id);
			data.Get(1, ref impact_index_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(target_id);
			data.ArrayAdd(impact_index_id);
			return data;
		}
	}

	public class Msg_CRC_StopSkill : IJsonMessage
	{
		public int role_id;
		public int skill_index_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref skill_index_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(skill_index_id);
			return data;
		}
	}

	public class Msg_CRC_StoryMessage : IJsonMessage
	{

		public class MessageArg : IJsonMessage
		{
			public int val_type;
			public string str_val;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref val_type);
				data.Get(1, ref str_val);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(val_type);
				data.ArrayAdd(str_val);
				return data;
			}
		}
		public string m_MsgId;
		public List<MessageArg> m_Args = new List<MessageArg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref m_MsgId);
			JsonMessageUtility.GetSubDataArray(data, 1, ref m_Args);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(m_MsgId);
			JsonMessageUtility.AddSubDataArray(data, m_Args);
			return data;
		}
	}

	public class Msg_CRC_SummonNpc : IJsonMessage
	{
		public int npc_id;
		public int owner_id;
		public int summon_owner_id;
		public int owner_skillid;
		public int link_id;
		public string model_prefab;
		public int skill_id;
		public int ai_id;
		public bool follow_dead;
		public float pos_x;
		public float pos_y;
		public float pos_z;
		public string ai_params;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref owner_id);
			data.Get(2, ref summon_owner_id);
			data.Get(3, ref owner_skillid);
			data.Get(4, ref link_id);
			data.Get(5, ref model_prefab);
			data.Get(6, ref skill_id);
			data.Get(7, ref ai_id);
			data.Get(8, ref follow_dead);
			data.Get(9, ref pos_x);
			data.Get(10, ref pos_y);
			data.Get(11, ref pos_z);
			data.Get(12, ref ai_params);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(owner_id);
			data.ArrayAdd(summon_owner_id);
			data.ArrayAdd(owner_skillid);
			data.ArrayAdd(link_id);
			data.ArrayAdd(model_prefab);
			data.ArrayAdd(skill_id);
			data.ArrayAdd(ai_id);
			data.ArrayAdd(follow_dead);
			data.ArrayAdd(pos_x);
			data.ArrayAdd(pos_y);
			data.ArrayAdd(pos_z);
			data.ArrayAdd(ai_params);
			return data;
		}
	}

	public class Msg_CRC_UserStopSkill : IJsonMessage
	{
		public int user_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref user_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(user_id);
			return data;
		}
	}

	public class Msg_Ping : IJsonMessage
	{
		public long send_ping_time;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref send_ping_time);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(send_ping_time);
			return data;
		}
	}

	public class Msg_Pong : IJsonMessage
	{
		public long send_ping_time;
		public long send_pong_time;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref send_ping_time);
			data.Get(1, ref send_pong_time);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(send_ping_time);
			data.ArrayAdd(send_pong_time);
			return data;
		}
	}

	public class Msg_RC_AddSkill : IJsonMessage
	{
		public int obj_id;
		public int skill_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref skill_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(skill_id);
			return data;
		}
	}

	public class Msg_RC_AdjustPosition : IJsonMessage
	{
		public int role_id;
		public int x;
		public int z;
		public int face_dir;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref x);
			data.Get(2, ref z);
			data.Get(3, ref face_dir);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(x);
			data.ArrayAdd(z);
			data.ArrayAdd(face_dir);
			return data;
		}
	}

	public class Msg_RC_CameraDistance : IJsonMessage
	{
		public float distance;
		public int smooth_lag;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref distance);
			data.Get(1, ref smooth_lag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(distance);
			data.ArrayAdd(smooth_lag);
			return data;
		}
	}

	public class Msg_RC_CameraEnable : IJsonMessage
	{
		public string camera_name;
		public bool is_enable;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref camera_name);
			data.Get(1, ref is_enable);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(camera_name);
			data.ArrayAdd(is_enable);
			return data;
		}
	}

	public class Msg_RC_CameraFollow : IJsonMessage
	{
		public int obj_id;
		public bool is_immediately;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref is_immediately);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(is_immediately);
			return data;
		}
	}

	public class Msg_RC_CameraHeight : IJsonMessage
	{
		public float height;
		public int smooth_lag;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref height);
			data.Get(1, ref smooth_lag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(height);
			data.ArrayAdd(smooth_lag);
			return data;
		}
	}

	public class Msg_RC_CameraLookat : IJsonMessage
	{
		public float x;
		public float y;
		public float z;
		public bool is_immediately;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref x);
			data.Get(1, ref y);
			data.Get(2, ref z);
			data.Get(3, ref is_immediately);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(x);
			data.ArrayAdd(y);
			data.ArrayAdd(z);
			data.ArrayAdd(is_immediately);
			return data;
		}
	}

	public class Msg_RC_CameraYaw : IJsonMessage
	{
		public float yaw;
		public int smooth_lag;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref yaw);
			data.Get(1, ref smooth_lag);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(yaw);
			data.ArrayAdd(smooth_lag);
			return data;
		}
	}

	public class Msg_RC_CampChanged : IJsonMessage
	{
		public int obj_id;
		public int camp_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref camp_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(camp_id);
			return data;
		}
	}

	public class Msg_RC_ChangeScene : IJsonMessage
	{
		public int target_scene_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref target_scene_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(target_scene_id);
			return data;
		}
	}

	public class Msg_RC_ClientError : IJsonMessage
	{
		public int ErrorCode;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref ErrorCode);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(ErrorCode);
			return data;
		}
	}

	public class Msg_RC_ControlObject : IJsonMessage
	{
		public int controller_id;
		public int controlled_id;
		public bool control_or_release;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref controller_id);
			data.Get(1, ref controlled_id);
			data.Get(2, ref control_or_release);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(controller_id);
			data.ArrayAdd(controlled_id);
			data.ArrayAdd(control_or_release);
			return data;
		}
	}

	public class Msg_RC_CreateNpc : IJsonMessage
	{
		public int npc_id;
		public int unit_id;
		public Position cur_pos;
		public float face_direction;
		public int link_id;
		public int camp_id;
		public int owner_id;
		public int add_attr_id;
		public int level;
		public int hp_rate;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref unit_id);
			JsonMessageUtility.GetSubData(data, 2, out cur_pos);
			data.Get(3, ref face_direction);
			data.Get(4, ref link_id);
			data.Get(5, ref camp_id);
			data.Get(6, ref owner_id);
			data.Get(7, ref add_attr_id);
			data.Get(8, ref level);
			data.Get(9, ref hp_rate);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(unit_id);
			JsonMessageUtility.AddSubData(data, cur_pos);
			data.ArrayAdd(face_direction);
			data.ArrayAdd(link_id);
			data.ArrayAdd(camp_id);
			data.ArrayAdd(owner_id);
			data.ArrayAdd(add_attr_id);
			data.ArrayAdd(level);
			data.ArrayAdd(hp_rate);
			return data;
		}
	}

	public class Msg_RC_CreatePartner : IJsonMessage
	{
		public int npc_id;
		public int unit_id;
		public Position cur_pos;
		public float face_direction;
		public int link_id;
		public int camp_id;
		public int owner_id;
		public int add_attr_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref unit_id);
			JsonMessageUtility.GetSubData(data, 2, out cur_pos);
			data.Get(3, ref face_direction);
			data.Get(4, ref link_id);
			data.Get(5, ref camp_id);
			data.Get(6, ref owner_id);
			data.Get(7, ref add_attr_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(unit_id);
			JsonMessageUtility.AddSubData(data, cur_pos);
			data.ArrayAdd(face_direction);
			data.ArrayAdd(link_id);
			data.ArrayAdd(camp_id);
			data.ArrayAdd(owner_id);
			data.ArrayAdd(add_attr_id);
			return data;
		}
	}

	public class Msg_RC_Dead : IJsonMessage
	{
		public int role_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			return data;
		}
	}

	public class Msg_RC_DebugSpaceInfo : IJsonMessage
	{

		public class DebugSpaceInfo : IJsonMessage
		{
			public int obj_id;
			public bool is_player;
			public float pos_x;
			public float pos_z;
			public float face_dir;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref obj_id);
				data.Get(1, ref is_player);
				data.Get(2, ref pos_x);
				data.Get(3, ref pos_z);
				data.Get(4, ref face_dir);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(obj_id);
				data.ArrayAdd(is_player);
				data.ArrayAdd(pos_x);
				data.ArrayAdd(pos_z);
				data.ArrayAdd(face_dir);
				return data;
			}
		}
		public List<DebugSpaceInfo> space_infos = new List<DebugSpaceInfo>();

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetSubDataArray(data, 0, ref space_infos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddSubDataArray(data, space_infos);
			return data;
		}
	}

	public class Msg_RC_DestroyNpc : IJsonMessage
	{
		public int npc_id;
		public bool need_play_effect;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref need_play_effect);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(need_play_effect);
			return data;
		}
	}

	public class Msg_RC_Disappear : IJsonMessage
	{
		public int role_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			return data;
		}
	}

	public class Msg_RC_DropNpc : IJsonMessage
	{
		public int npc_id;
		public int link_id;
		public int owner_id;
		public int from_obj_id;
		public int drop_type;
		public int drop_num;
		public int camp_id;
		public string model;
		public string effect;
		public Position cur_pos;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref link_id);
			data.Get(2, ref owner_id);
			data.Get(3, ref from_obj_id);
			data.Get(4, ref drop_type);
			data.Get(5, ref drop_num);
			data.Get(6, ref camp_id);
			data.Get(7, ref model);
			data.Get(8, ref effect);
			JsonMessageUtility.GetSubData(data, 9, out cur_pos);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(link_id);
			data.ArrayAdd(owner_id);
			data.ArrayAdd(from_obj_id);
			data.ArrayAdd(drop_type);
			data.ArrayAdd(drop_num);
			data.ArrayAdd(camp_id);
			data.ArrayAdd(model);
			data.ArrayAdd(effect);
			JsonMessageUtility.AddSubData(data, cur_pos);
			return data;
		}
	}

	public class Msg_RC_EnableInput : IJsonMessage
	{
		public bool is_enable;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_enable);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_enable);
			return data;
		}
	}

	public class Msg_RC_Enter : IJsonMessage
	{
		public int role_id;
		public int hero_id;
		public int camp_id;
		public Position position;
		public float face_dir;
		public bool is_moving;
		public float move_dir;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref hero_id);
			data.Get(2, ref camp_id);
			JsonMessageUtility.GetSubData(data, 3, out position);
			data.Get(4, ref face_dir);
			data.Get(5, ref is_moving);
			data.Get(6, ref move_dir);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(hero_id);
			data.ArrayAdd(camp_id);
			JsonMessageUtility.AddSubData(data, position);
			data.ArrayAdd(face_dir);
			data.ArrayAdd(is_moving);
			data.ArrayAdd(move_dir);
			return data;
		}
	}

	public class Msg_RC_HighlightPrompt : IJsonMessage
	{
		public int dict_id;
		public List<string> argument = new List<string>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref dict_id);
			JsonMessageUtility.GetSimpleArray(data, 1, ref argument);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(dict_id);
			JsonMessageUtility.AddSimpleArray(data, argument);
			return data;
		}
	}

	public class Msg_RC_ImpactDamage : IJsonMessage
	{
		public int role_id;
		public int attacker_id;
		public int damage_status;
		public int hp;
		public int energy;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref attacker_id);
			data.Get(2, ref damage_status);
			data.Get(3, ref hp);
			data.Get(4, ref energy);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(attacker_id);
			data.ArrayAdd(damage_status);
			data.ArrayAdd(hp);
			data.ArrayAdd(energy);
			return data;
		}
	}

	public class Msg_RC_ImpactRage : IJsonMessage
	{
		public int role_id;
		public int rage;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref rage);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(rage);
			return data;
		}
	}

	public class Msg_RC_LockFrame : IJsonMessage
	{
		public float scale;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref scale);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(scale);
			return data;
		}
	}

	public class Msg_RC_MissionCompleted : IJsonMessage
	{
		public int target_scene_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref target_scene_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(target_scene_id);
			return data;
		}
	}

	public class Msg_RC_NpcDead : IJsonMessage
	{
		public int npc_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			return data;
		}
	}

	public class Msg_RC_NpcDisappear : IJsonMessage
	{
		public int npc_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			return data;
		}
	}

	public class Msg_RC_NpcEnter : IJsonMessage
	{
		public int npc_id;
		public float cur_pos_x;
		public float cur_pos_z;
		public float face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref cur_pos_x);
			data.Get(2, ref cur_pos_z);
			data.Get(3, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_RC_NpcFace : IJsonMessage
	{
		public int npc_id;
		public int face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_RC_NpcMove : IJsonMessage
	{
		public int npc_id;
		public int move_mode;
		public int move_direction;
		public int velocity;
		public int cur_pos_x;
		public int cur_pos_z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref move_mode);
			data.Get(2, ref move_direction);
			data.Get(3, ref velocity);
			data.Get(4, ref cur_pos_x);
			data.Get(5, ref cur_pos_z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(move_mode);
			data.ArrayAdd(move_direction);
			data.ArrayAdd(velocity);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			return data;
		}
	}

	public class Msg_RC_NpcSkill : IJsonMessage
	{
		public int npc_id;
		public int skill_index_id;
		public ulong stand_pos;
		public int face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref skill_index_id);
			data.Get(2, ref stand_pos);
			data.Get(3, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(skill_index_id);
			data.ArrayAdd(stand_pos);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_RC_PlayAnimation : IJsonMessage
	{
		public int obj_id;
		public int anim_type;
		public int anim_time;
		public bool is_queued;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref anim_type);
			data.Get(2, ref anim_time);
			data.Get(3, ref is_queued);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(anim_type);
			data.ArrayAdd(anim_time);
			data.ArrayAdd(is_queued);
			return data;
		}
	}

	public class Msg_RC_PublishEvent : IJsonMessage
	{

		public class EventArg : IJsonMessage
		{
			public int val_type;
			public string str_val;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref val_type);
				data.Get(1, ref str_val);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(val_type);
				data.ArrayAdd(str_val);
				return data;
			}
		}
		public bool is_logic_event;
		public string ev_name;
		public string group;
		public List<EventArg> args = new List<EventArg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_logic_event);
			data.Get(1, ref ev_name);
			data.Get(2, ref group);
			JsonMessageUtility.GetSubDataArray(data, 3, ref args);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_logic_event);
			data.ArrayAdd(ev_name);
			data.ArrayAdd(group);
			JsonMessageUtility.AddSubDataArray(data, args);
			return data;
		}
	}

	public class Msg_RC_PvpIsAllReady : IJsonMessage
	{
		public bool isAllReady;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref isAllReady);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(isAllReady);
			return data;
		}
	}

	public class Msg_RC_RefreshItemSkills : IJsonMessage
	{
		public int role_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			return data;
		}
	}

	public class Msg_RC_RemoveSkill : IJsonMessage
	{
		public int obj_id;
		public int skill_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref skill_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(skill_id);
			return data;
		}
	}

	public class Msg_RC_Revive : IJsonMessage
	{
		public int role_id;
		public bool is_player_self;
		public int hero_id;
		public int camp_id;
		public Position position;
		public float face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref is_player_self);
			data.Get(2, ref hero_id);
			data.Get(3, ref camp_id);
			JsonMessageUtility.GetSubData(data, 4, out position);
			data.Get(5, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(is_player_self);
			data.ArrayAdd(hero_id);
			data.ArrayAdd(camp_id);
			JsonMessageUtility.AddSubData(data, position);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_RC_SendGfxMessage : IJsonMessage
	{

		public class EventArg : IJsonMessage
		{
			public int val_type;
			public string str_val;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref val_type);
				data.Get(1, ref str_val);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(val_type);
				data.ArrayAdd(str_val);
				return data;
			}
		}
		public bool is_with_tag;
		public string name;
		public string msg;
		public List<EventArg> args = new List<EventArg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_with_tag);
			data.Get(1, ref name);
			data.Get(2, ref msg);
			JsonMessageUtility.GetSubDataArray(data, 3, ref args);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_with_tag);
			data.ArrayAdd(name);
			data.ArrayAdd(msg);
			JsonMessageUtility.AddSubDataArray(data, args);
			return data;
		}
	}

	public class Msg_RC_SendGfxMessageById : IJsonMessage
	{

		public class EventArg : IJsonMessage
		{
			public int val_type;
			public string str_val;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref val_type);
				data.Get(1, ref str_val);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(val_type);
				data.ArrayAdd(str_val);
				return data;
			}
		}
		public int obj_id;
		public string msg;
		public List<EventArg> args = new List<EventArg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref msg);
			JsonMessageUtility.GetSubDataArray(data, 2, ref args);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(msg);
			JsonMessageUtility.AddSubDataArray(data, args);
			return data;
		}
	}

	public class Msg_RC_SetBlockedShader : IJsonMessage
	{
		public uint rim_color_1;
		public float rim_power_1;
		public float rim_cutvalue_1;
		public uint rim_color_2;
		public float rim_power_2;
		public float rim_cutvalue_2;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref rim_color_1);
			data.Get(1, ref rim_power_1);
			data.Get(2, ref rim_cutvalue_1);
			data.Get(3, ref rim_color_2);
			data.Get(4, ref rim_power_2);
			data.Get(5, ref rim_cutvalue_2);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(rim_color_1);
			data.ArrayAdd(rim_power_1);
			data.ArrayAdd(rim_cutvalue_1);
			data.ArrayAdd(rim_color_2);
			data.ArrayAdd(rim_power_2);
			data.ArrayAdd(rim_cutvalue_2);
			return data;
		}
	}

	public class Msg_RC_ShakeHands_Ret : IJsonMessage
	{

		public enum RetType
		{
			SUCCESS = 0,
			ERROR = 1,
		}
		public RetType auth_result;

		public void FromJson(JsonData data)
		{
			JsonMessageUtility.GetEnum(data, 0, out auth_result);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			JsonMessageUtility.AddEnum(data, auth_result);
			return data;
		}
	}

	public class Msg_RC_ShowDlg : IJsonMessage
	{
		public int dialog_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref dialog_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(dialog_id);
			return data;
		}
	}

	public class Msg_RC_ShowUi : IJsonMessage
	{
		public bool is_show;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref is_show);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(is_show);
			return data;
		}
	}

	public class Msg_RC_ShowWall : IJsonMessage
	{
		public string wall_name;
		public bool is_show;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref wall_name);
			data.Get(1, ref is_show);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(wall_name);
			data.ArrayAdd(is_show);
			return data;
		}
	}

	public class Msg_RC_StartCountDown : IJsonMessage
	{
		public int count_down_time;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref count_down_time);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(count_down_time);
			return data;
		}
	}

	public class Msg_RC_StopImpact : IJsonMessage
	{
		public int obj_id;
		public int impact_index_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref impact_index_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(impact_index_id);
			return data;
		}
	}

	public class Msg_RC_SyncCombatInfo : IJsonMessage
	{
		public int hit_count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref hit_count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(hit_count);
			return data;
		}
	}

	public class Msg_RC_SyncCombatStatisticInfo : IJsonMessage
	{
		public int role_id;
		public int kill_hero_count;
		public int assit_kill_count;
		public int kill_npc_count;
		public int dead_count;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref kill_hero_count);
			data.Get(2, ref assit_kill_count);
			data.Get(3, ref kill_npc_count);
			data.Get(4, ref dead_count);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(kill_hero_count);
			data.ArrayAdd(assit_kill_count);
			data.ArrayAdd(kill_npc_count);
			data.ArrayAdd(dead_count);
			return data;
		}
	}

	public class Msg_RC_SyncNpcOwnerId : IJsonMessage
	{
		public int npc_id;
		public int owner_id;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref npc_id);
			data.Get(1, ref owner_id);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(npc_id);
			data.ArrayAdd(owner_id);
			return data;
		}
	}

	public class Msg_RC_SyncProperty : IJsonMessage
	{
		public int role_id;
		public int hp;
		public int np;
		public int state;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref hp);
			data.Get(2, ref np);
			data.Get(3, ref state);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(hp);
			data.ArrayAdd(np);
			data.ArrayAdd(state);
			return data;
		}
	}

	public class Msg_RC_UpdateCoefficient : IJsonMessage
	{
		public int obj_id;
		public float hpmax_coefficient;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref obj_id);
			data.Get(1, ref hpmax_coefficient);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(obj_id);
			data.ArrayAdd(hpmax_coefficient);
			return data;
		}
	}

	public class Msg_RC_UpdateUserBattleInfo : IJsonMessage
	{

		public class EquipInfo : IJsonMessage
		{
			public int equip_id;
			public int equip_level;
			public int equip_random_property;
			public int equip_upgrade_star;
			public int equip_strength_level;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref equip_id);
				data.Get(1, ref equip_level);
				data.Get(2, ref equip_random_property);
				data.Get(3, ref equip_upgrade_star);
				data.Get(4, ref equip_strength_level);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(equip_id);
				data.ArrayAdd(equip_level);
				data.ArrayAdd(equip_random_property);
				data.ArrayAdd(equip_upgrade_star);
				data.ArrayAdd(equip_strength_level);
				return data;
			}
		}

		public class FashionMsg : IJsonMessage
		{
			public int PartIndex;
			public int FsnId;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref PartIndex);
				data.Get(1, ref FsnId);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(PartIndex);
				data.ArrayAdd(FsnId);
				return data;
			}
		}

		public class LegacyInfo : IJsonMessage
		{
			public int legacy_id;
			public int legacy_level;
			public int legacy_random_property;
			public bool legacy_IsUnlock;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref legacy_id);
				data.Get(1, ref legacy_level);
				data.Get(2, ref legacy_random_property);
				data.Get(3, ref legacy_IsUnlock);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(legacy_id);
				data.ArrayAdd(legacy_level);
				data.ArrayAdd(legacy_random_property);
				data.ArrayAdd(legacy_IsUnlock);
				return data;
			}
		}

		public class PartnerDataInfo : IJsonMessage
		{
			public int PartnerId;
			public int PartnerLevel;
			public int PartnerStage;
			public List<bool> PartnerEquipState = new List<bool>();

			public void FromJson(JsonData data)
			{
				data.Get(0, ref PartnerId);
				data.Get(1, ref PartnerLevel);
				data.Get(2, ref PartnerStage);
				JsonMessageUtility.GetSimpleArray(data, 3, ref PartnerEquipState);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(PartnerId);
				data.ArrayAdd(PartnerLevel);
				data.ArrayAdd(PartnerStage);
				JsonMessageUtility.AddSimpleArray(data, PartnerEquipState);
				return data;
			}
		}

		public class PresetInfo : IJsonMessage
		{
			public int skill_id;
			public int skill_level;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref skill_id);
				data.Get(1, ref skill_level);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(skill_id);
				data.ArrayAdd(skill_level);
				return data;
			}
		}

		public class TalentDataMsg : IJsonMessage
		{
			public int Slot;
			public int ItemId;
			public int Level;
			public int Experience;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref Slot);
				data.Get(1, ref ItemId);
				data.Get(2, ref Level);
				data.Get(3, ref Experience);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(Slot);
				data.ArrayAdd(ItemId);
				data.ArrayAdd(Level);
				data.ArrayAdd(Experience);
				return data;
			}
		}

		public class XSoulDataInfo : IJsonMessage
		{
			public int ItemId;
			public int Level;
			public int ModelLevel;
			public int Experience;

			public void FromJson(JsonData data)
			{
				data.Get(0, ref ItemId);
				data.Get(1, ref Level);
				data.Get(2, ref ModelLevel);
				data.Get(3, ref Experience);
			}
			public JsonData ToJson()
			{
				JsonData data = new JsonData();
				data.SetJsonType(JsonType.Array);
				data.ArrayAdd(ItemId);
				data.ArrayAdd(Level);
				data.ArrayAdd(ModelLevel);
				data.ArrayAdd(Experience);
				return data;
			}
		}
		public int role_id;
		public List<PresetInfo> skill_info = new List<PresetInfo>();
		public int preset_index;
		public List<EquipInfo> equip_info = new List<EquipInfo>();
		public List<LegacyInfo> legacy_info = new List<LegacyInfo>();
		public List<XSoulDataInfo> XSouls = new List<XSoulDataInfo>();
		public List<PartnerDataInfo> Partners = new List<PartnerDataInfo>();
		public List<TalentDataMsg> EquipTalents = new List<TalentDataMsg>();
		public List<FashionMsg> FashionInfo = new List<FashionMsg>();

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			JsonMessageUtility.GetSubDataArray(data, 1, ref skill_info);
			data.Get(2, ref preset_index);
			JsonMessageUtility.GetSubDataArray(data, 3, ref equip_info);
			JsonMessageUtility.GetSubDataArray(data, 4, ref legacy_info);
			JsonMessageUtility.GetSubDataArray(data, 5, ref XSouls);
			JsonMessageUtility.GetSubDataArray(data, 6, ref Partners);
			JsonMessageUtility.GetSubDataArray(data, 7, ref EquipTalents);
			JsonMessageUtility.GetSubDataArray(data, 8, ref FashionInfo);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			JsonMessageUtility.AddSubDataArray(data, skill_info);
			data.ArrayAdd(preset_index);
			JsonMessageUtility.AddSubDataArray(data, equip_info);
			JsonMessageUtility.AddSubDataArray(data, legacy_info);
			JsonMessageUtility.AddSubDataArray(data, XSouls);
			JsonMessageUtility.AddSubDataArray(data, Partners);
			JsonMessageUtility.AddSubDataArray(data, EquipTalents);
			JsonMessageUtility.AddSubDataArray(data, FashionInfo);
			return data;
		}
	}

	public class Msg_RC_UserFace : IJsonMessage
	{
		public int role_id;
		public float face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Msg_RC_UserMove : IJsonMessage
	{
		public int role_id;
		public bool is_moving;
		public int move_direction;
		public int cur_pos_x;
		public int cur_pos_z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref role_id);
			data.Get(1, ref is_moving);
			data.Get(2, ref move_direction);
			data.Get(3, ref cur_pos_x);
			data.Get(4, ref cur_pos_z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(role_id);
			data.ArrayAdd(is_moving);
			data.ArrayAdd(move_direction);
			data.ArrayAdd(cur_pos_x);
			data.ArrayAdd(cur_pos_z);
			return data;
		}
	}

	public class Msg_RC_UserSkill : IJsonMessage
	{
		public int user_id;
		public int skill_index_id;
		public ulong stand_pos;
		public int face_direction;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref user_id);
			data.Get(1, ref skill_index_id);
			data.Get(2, ref stand_pos);
			data.Get(3, ref face_direction);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(user_id);
			data.ArrayAdd(skill_index_id);
			data.ArrayAdd(stand_pos);
			data.ArrayAdd(face_direction);
			return data;
		}
	}

	public class Position : IJsonMessage
	{
		public float x;
		public float z;

		public void FromJson(JsonData data)
		{
			data.Get(0, ref x);
			data.Get(1, ref z);
		}
		public JsonData ToJson()
		{
			JsonData data = new JsonData();
			data.SetJsonType(JsonType.Array);
			data.ArrayAdd(x);
			data.ArrayAdd(z);
			return data;
		}
	}
}
