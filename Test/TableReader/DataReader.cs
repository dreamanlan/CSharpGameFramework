//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由TableReaderGenerator按table.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using DemoCommon;

namespace BinaryTableConfig
{
	public sealed partial class Skill : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 232)]
		private struct SkillRecord
		{
			internal int id;
			internal int desc;
			internal int type;
			internal int icon;
			internal float distance;
			internal float cooldown;
			internal float BaseCooldown;
			internal float duration;
			internal float interval;
			internal int damage;
			internal int mpRecover;
			internal int hpRecover;
			internal int addAttack;
			internal int addShield;
			internal float addSpeed;
			internal int canmove;
			internal int leadAnim;
			internal int leadAnimTime;
			internal int leadEffect;
			internal int leadEffectBone;
			internal int leadEffectStartTime;
			internal int leadEffectDeleteTime;
			internal int castAnim;
			internal int castAnimTime;
			internal int selfEffect;
			internal int selfEffectBone;
			internal int selfEffectStartTime;
			internal int selfEffectDeleteTime;
			internal int targetEffect;
			internal int targetEffectBone;
			internal int targetEffectStartTime;
			internal int targetEffectDeleteTime;
			internal int emitEffect;
			internal int emitEffectBone;
			internal int emitEffectStartTime;
			internal int emitSpeed;
			internal int hitAnim;
			internal int hitAnimTime;
			internal int hitEffect;
			internal int hitEffectBone;
			internal int hitEffectStartTime;
			internal int hitEffectDeleteTime;
			internal int impactToSelf;
			internal int impactToTarget;
			internal int interruptPriority;
			internal int isInterrupt;
			internal int targetType;
			internal int aoeType;
			internal float aoeSize;
			internal float aoeAngleOrLength;
			internal int maxAoeTargetCount;
			internal int dslSkillId;
			internal int dslFile;
			internal int sound;
			internal int soundDelay;
			internal float skilltimescale;
			internal int skilltimescalestarttime;
			internal int skilltimescaleendtime;
		}

		public int id;
		public string desc;
		public int type;
		public int icon;
		public float distance;
		public float cooldown;
		public float BaseCooldown;
		public float duration;
		public float interval;
		public int damage;
		public int mpRecover;
		public int hpRecover;
		public int addAttack;
		public int addShield;
		public float addSpeed;
		public int canmove;
		public string leadAnim;
		public int leadAnimTime;
		public string leadEffect;
		public string leadEffectBone;
		public int leadEffectStartTime;
		public int leadEffectDeleteTime;
		public string castAnim;
		public int castAnimTime;
		public string selfEffect;
		public string selfEffectBone;
		public int selfEffectStartTime;
		public int selfEffectDeleteTime;
		public string targetEffect;
		public string targetEffectBone;
		public int targetEffectStartTime;
		public int targetEffectDeleteTime;
		public string emitEffect;
		public string emitEffectBone;
		public int emitEffectStartTime;
		public int emitSpeed;
		public string hitAnim;
		public int hitAnimTime;
		public string hitEffect;
		public string hitEffectBone;
		public int hitEffectStartTime;
		public int hitEffectDeleteTime;
		public int impactToSelf;
		public int impactToTarget;
		public int interruptPriority;
		public bool isInterrupt;
		public int targetType;
		public int aoeType;
		public float aoeSize;
		public float aoeAngleOrLength;
		public int maxAoeTargetCount;
		public int dslSkillId;
		public string dslFile;
		public string sound;
		public int soundDelay;
		public float skilltimescale;
		public int skilltimescalestarttime;
		public int skilltimescaleendtime;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			distance = DataRecordUtility.ExtractFloat(table, record.distance, 0);
			cooldown = DataRecordUtility.ExtractFloat(table, record.cooldown, 0);
			BaseCooldown = DataRecordUtility.ExtractFloat(table, record.BaseCooldown, 0);
			duration = DataRecordUtility.ExtractFloat(table, record.duration, 0);
			interval = DataRecordUtility.ExtractFloat(table, record.interval, 0);
			damage = DataRecordUtility.ExtractInt(table, record.damage, 0);
			mpRecover = DataRecordUtility.ExtractInt(table, record.mpRecover, 0);
			hpRecover = DataRecordUtility.ExtractInt(table, record.hpRecover, 0);
			addAttack = DataRecordUtility.ExtractInt(table, record.addAttack, 0);
			addShield = DataRecordUtility.ExtractInt(table, record.addShield, 0);
			addSpeed = DataRecordUtility.ExtractFloat(table, record.addSpeed, 0);
			canmove = DataRecordUtility.ExtractInt(table, record.canmove, 0);
			leadAnim = DataRecordUtility.ExtractString(table, record.leadAnim, "");
			leadAnimTime = DataRecordUtility.ExtractInt(table, record.leadAnimTime, 0);
			leadEffect = DataRecordUtility.ExtractString(table, record.leadEffect, "");
			leadEffectBone = DataRecordUtility.ExtractString(table, record.leadEffectBone, "");
			leadEffectStartTime = DataRecordUtility.ExtractInt(table, record.leadEffectStartTime, 0);
			leadEffectDeleteTime = DataRecordUtility.ExtractInt(table, record.leadEffectDeleteTime, 0);
			castAnim = DataRecordUtility.ExtractString(table, record.castAnim, "");
			castAnimTime = DataRecordUtility.ExtractInt(table, record.castAnimTime, 0);
			selfEffect = DataRecordUtility.ExtractString(table, record.selfEffect, "");
			selfEffectBone = DataRecordUtility.ExtractString(table, record.selfEffectBone, "");
			selfEffectStartTime = DataRecordUtility.ExtractInt(table, record.selfEffectStartTime, 0);
			selfEffectDeleteTime = DataRecordUtility.ExtractInt(table, record.selfEffectDeleteTime, 0);
			targetEffect = DataRecordUtility.ExtractString(table, record.targetEffect, "");
			targetEffectBone = DataRecordUtility.ExtractString(table, record.targetEffectBone, "");
			targetEffectStartTime = DataRecordUtility.ExtractInt(table, record.targetEffectStartTime, 0);
			targetEffectDeleteTime = DataRecordUtility.ExtractInt(table, record.targetEffectDeleteTime, 0);
			emitEffect = DataRecordUtility.ExtractString(table, record.emitEffect, "");
			emitEffectBone = DataRecordUtility.ExtractString(table, record.emitEffectBone, "");
			emitEffectStartTime = DataRecordUtility.ExtractInt(table, record.emitEffectStartTime, 0);
			emitSpeed = DataRecordUtility.ExtractInt(table, record.emitSpeed, 0);
			hitAnim = DataRecordUtility.ExtractString(table, record.hitAnim, "");
			hitAnimTime = DataRecordUtility.ExtractInt(table, record.hitAnimTime, 0);
			hitEffect = DataRecordUtility.ExtractString(table, record.hitEffect, "");
			hitEffectBone = DataRecordUtility.ExtractString(table, record.hitEffectBone, "");
			hitEffectStartTime = DataRecordUtility.ExtractInt(table, record.hitEffectStartTime, 0);
			hitEffectDeleteTime = DataRecordUtility.ExtractInt(table, record.hitEffectDeleteTime, 0);
			impactToSelf = DataRecordUtility.ExtractInt(table, record.impactToSelf, 0);
			impactToTarget = DataRecordUtility.ExtractInt(table, record.impactToTarget, 0);
			interruptPriority = DataRecordUtility.ExtractInt(table, record.interruptPriority, 0);
			isInterrupt = DataRecordUtility.ExtractBool(table, record.isInterrupt, false);
			targetType = DataRecordUtility.ExtractInt(table, record.targetType, 0);
			aoeType = DataRecordUtility.ExtractInt(table, record.aoeType, 0);
			aoeSize = DataRecordUtility.ExtractFloat(table, record.aoeSize, 0);
			aoeAngleOrLength = DataRecordUtility.ExtractFloat(table, record.aoeAngleOrLength, 0);
			maxAoeTargetCount = DataRecordUtility.ExtractInt(table, record.maxAoeTargetCount, 0);
			dslSkillId = DataRecordUtility.ExtractInt(table, record.dslSkillId, 0);
			dslFile = DataRecordUtility.ExtractString(table, record.dslFile, "");
			sound = DataRecordUtility.ExtractString(table, record.sound, "");
			soundDelay = DataRecordUtility.ExtractInt(table, record.soundDelay, 0);
			skilltimescale = DataRecordUtility.ExtractFloat(table, record.skilltimescale, 0);
			skilltimescalestarttime = DataRecordUtility.ExtractInt(table, record.skilltimescalestarttime, 0);
			skilltimescaleendtime = DataRecordUtility.ExtractInt(table, record.skilltimescaleendtime, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillRecord record = new SkillRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.desc = DataRecordUtility.SetValue(table, desc, "");
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.icon = DataRecordUtility.SetValue(table, icon, 0);
			record.distance = DataRecordUtility.SetValue(table, distance, 0);
			record.cooldown = DataRecordUtility.SetValue(table, cooldown, 0);
			record.BaseCooldown = DataRecordUtility.SetValue(table, BaseCooldown, 0);
			record.duration = DataRecordUtility.SetValue(table, duration, 0);
			record.interval = DataRecordUtility.SetValue(table, interval, 0);
			record.damage = DataRecordUtility.SetValue(table, damage, 0);
			record.mpRecover = DataRecordUtility.SetValue(table, mpRecover, 0);
			record.hpRecover = DataRecordUtility.SetValue(table, hpRecover, 0);
			record.addAttack = DataRecordUtility.SetValue(table, addAttack, 0);
			record.addShield = DataRecordUtility.SetValue(table, addShield, 0);
			record.addSpeed = DataRecordUtility.SetValue(table, addSpeed, 0);
			record.canmove = DataRecordUtility.SetValue(table, canmove, 0);
			record.leadAnim = DataRecordUtility.SetValue(table, leadAnim, "");
			record.leadAnimTime = DataRecordUtility.SetValue(table, leadAnimTime, 0);
			record.leadEffect = DataRecordUtility.SetValue(table, leadEffect, "");
			record.leadEffectBone = DataRecordUtility.SetValue(table, leadEffectBone, "");
			record.leadEffectStartTime = DataRecordUtility.SetValue(table, leadEffectStartTime, 0);
			record.leadEffectDeleteTime = DataRecordUtility.SetValue(table, leadEffectDeleteTime, 0);
			record.castAnim = DataRecordUtility.SetValue(table, castAnim, "");
			record.castAnimTime = DataRecordUtility.SetValue(table, castAnimTime, 0);
			record.selfEffect = DataRecordUtility.SetValue(table, selfEffect, "");
			record.selfEffectBone = DataRecordUtility.SetValue(table, selfEffectBone, "");
			record.selfEffectStartTime = DataRecordUtility.SetValue(table, selfEffectStartTime, 0);
			record.selfEffectDeleteTime = DataRecordUtility.SetValue(table, selfEffectDeleteTime, 0);
			record.targetEffect = DataRecordUtility.SetValue(table, targetEffect, "");
			record.targetEffectBone = DataRecordUtility.SetValue(table, targetEffectBone, "");
			record.targetEffectStartTime = DataRecordUtility.SetValue(table, targetEffectStartTime, 0);
			record.targetEffectDeleteTime = DataRecordUtility.SetValue(table, targetEffectDeleteTime, 0);
			record.emitEffect = DataRecordUtility.SetValue(table, emitEffect, "");
			record.emitEffectBone = DataRecordUtility.SetValue(table, emitEffectBone, "");
			record.emitEffectStartTime = DataRecordUtility.SetValue(table, emitEffectStartTime, 0);
			record.emitSpeed = DataRecordUtility.SetValue(table, emitSpeed, 0);
			record.hitAnim = DataRecordUtility.SetValue(table, hitAnim, "");
			record.hitAnimTime = DataRecordUtility.SetValue(table, hitAnimTime, 0);
			record.hitEffect = DataRecordUtility.SetValue(table, hitEffect, "");
			record.hitEffectBone = DataRecordUtility.SetValue(table, hitEffectBone, "");
			record.hitEffectStartTime = DataRecordUtility.SetValue(table, hitEffectStartTime, 0);
			record.hitEffectDeleteTime = DataRecordUtility.SetValue(table, hitEffectDeleteTime, 0);
			record.impactToSelf = DataRecordUtility.SetValue(table, impactToSelf, 0);
			record.impactToTarget = DataRecordUtility.SetValue(table, impactToTarget, 0);
			record.interruptPriority = DataRecordUtility.SetValue(table, interruptPriority, 0);
			record.isInterrupt = DataRecordUtility.SetValue(table, isInterrupt, false);
			record.targetType = DataRecordUtility.SetValue(table, targetType, 0);
			record.aoeType = DataRecordUtility.SetValue(table, aoeType, 0);
			record.aoeSize = DataRecordUtility.SetValue(table, aoeSize, 0);
			record.aoeAngleOrLength = DataRecordUtility.SetValue(table, aoeAngleOrLength, 0);
			record.maxAoeTargetCount = DataRecordUtility.SetValue(table, maxAoeTargetCount, 0);
			record.dslSkillId = DataRecordUtility.SetValue(table, dslSkillId, 0);
			record.dslFile = DataRecordUtility.SetValue(table, dslFile, "");
			record.sound = DataRecordUtility.SetValue(table, sound, "");
			record.soundDelay = DataRecordUtility.SetValue(table, soundDelay, 0);
			record.skilltimescale = DataRecordUtility.SetValue(table, skilltimescale, 0);
			record.skilltimescalestarttime = DataRecordUtility.SetValue(table, skilltimescalestarttime, 0);
			record.skilltimescaleendtime = DataRecordUtility.SetValue(table, skilltimescaleendtime, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe SkillRecord GetRecord(BinaryTable table, int index)
		{
			SkillRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(SkillRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(SkillRecord record)
		{
			byte[] bytes = new byte[sizeof(SkillRecord)];
			fixed (byte* p = bytes) {
				SkillRecord* temp = (SkillRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class SkillProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_Skill);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_Skill);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_SkillMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("Skill is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_SkillMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_SkillMgr.Clear();
		}

		public DataDictionaryMgr<Skill,int> SkillMgr
		{
			get { return m_SkillMgr; }
		}

		public int GetSkillCount()
		{
			return m_SkillMgr.GetDataCount();
		}

		public Skill GetSkill(int id)
		{
			return m_SkillMgr.GetDataById(id);
		}

		private DataDictionaryMgr<Skill,int> m_SkillMgr = new DataDictionaryMgr<Skill,int>();

		public static SkillProvider Instance
		{
			get { return s_Instance; }
		}
		private static SkillProvider s_Instance = new SkillProvider();
	}
}
